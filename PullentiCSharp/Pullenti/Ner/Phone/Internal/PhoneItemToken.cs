/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Phone.Internal
{
    // Примитив, из которых состоит телефонный номер
    public class PhoneItemToken : Pullenti.Ner.MetaToken
    {
        public PhoneItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public enum PhoneItemType : int
        {
            Number,
            CityCode,
            Delim,
            Prefix,
            AddNumber,
            CountryCode,
            Alt,
        }

        public PhoneItemType ItemType;
        public string Value;
        public Pullenti.Ner.Phone.PhoneKind Kind = Pullenti.Ner.Phone.PhoneKind.Undefined;
        public bool IsInBrackets;
        public bool CanBeCountryPrefix
        {
            get
            {
                if (Value != null && PhoneHelper.GetCountryPrefix(Value) == Value) 
                    return true;
                else 
                    return false;
            }
        }
        public override string ToString()
        {
            return (ItemType + ": " + Value) + ((Kind == Pullenti.Ner.Phone.PhoneKind.Undefined ? "" : string.Format(" ({0})", Kind)));
        }
        public static PhoneItemToken TryAttach(Pullenti.Ner.Token t0)
        {
            PhoneItemToken res = _TryAttach(t0);
            if (res == null) 
                return null;
            if (res.ItemType != PhoneItemType.Prefix) 
                return res;
            for (Pullenti.Ner.Token t = res.EndToken.Next; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar) 
                    break;
                if (t.IsNewlineBefore) 
                    break;
                PhoneItemToken res2 = _TryAttach(t);
                if (res2 != null) 
                {
                    if (res2.ItemType == PhoneItemType.Prefix) 
                    {
                        if (res.Kind == Pullenti.Ner.Phone.PhoneKind.Undefined) 
                            res.Kind = res2.Kind;
                        t = (res.EndToken = res2.EndToken);
                        continue;
                    }
                    break;
                }
                if (t.IsChar(':')) 
                {
                    res.EndToken = t;
                    break;
                }
                if (!(t is Pullenti.Ner.TextToken)) 
                    break;
                if (t0.LengthChar == 1) 
                    break;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    t = npt.EndToken;
                    if (t.IsValue("ПОСЕЛЕНИЕ", null)) 
                        return null;
                    res.EndToken = t;
                    continue;
                }
                if (t.GetMorphClassInDictionary().IsProper) 
                {
                    res.EndToken = t;
                    continue;
                }
                if (t.Morph.Class.IsPreposition) 
                    continue;
                break;
            }
            return res;
        }
        static PhoneItemToken _TryAttach(Pullenti.Ner.Token t0)
        {
            if (t0 == null) 
                return null;
            if (t0 is Pullenti.Ner.NumberToken) 
            {
                if (Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(t0) != null && !t0.IsWhitespaceAfter) 
                {
                    Pullenti.Ner.ReferentToken rt = t0.Kit.ProcessReferent("PHONE", t0.Next);
                    if (rt == null) 
                        return null;
                }
                if ((t0 as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit && !t0.Morph.Class.IsAdjective) 
                    return new PhoneItemToken(t0, t0) { ItemType = PhoneItemType.Number, Value = t0.GetSourceText() };
                return null;
            }
            if (t0.IsChar('.')) 
                return new PhoneItemToken(t0, t0) { ItemType = PhoneItemType.Delim, Value = "." };
            if (t0.IsHiphen) 
                return new PhoneItemToken(t0, t0) { ItemType = PhoneItemType.Delim, Value = "-" };
            if (t0.IsChar('+')) 
            {
                if (!(t0.Next is Pullenti.Ner.NumberToken) || (t0.Next as Pullenti.Ner.NumberToken).Typ != Pullenti.Ner.NumberSpellingType.Digit) 
                    return null;
                else 
                {
                    string val = t0.Next.GetSourceText();
                    int i;
                    for (i = 0; i < val.Length; i++) 
                    {
                        if (val[i] != '0') 
                            break;
                    }
                    if (i >= val.Length) 
                        return null;
                    if (i > 0) 
                        val = val.Substring(i);
                    return new PhoneItemToken(t0, t0.Next) { ItemType = PhoneItemType.CountryCode, Value = val };
                }
            }
            if (t0.IsChar((char)0x2011) && (t0.Next is Pullenti.Ner.NumberToken) && t0.Next.LengthChar == 2) 
                return new PhoneItemToken(t0, t0) { ItemType = PhoneItemType.Delim, Value = "-" };
            if (t0.IsCharOf("(")) 
            {
                if (t0.Next is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.Token et = t0.Next;
                    StringBuilder val = new StringBuilder();
                    for (; et != null; et = et.Next) 
                    {
                        if (et.IsChar(')')) 
                            break;
                        if ((et is Pullenti.Ner.NumberToken) && (et as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) 
                            val.Append(et.GetSourceText());
                        else if (!et.IsHiphen && !et.IsChar('.')) 
                            return null;
                    }
                    if (et == null || val.Length == 0) 
                        return null;
                    else 
                        return new PhoneItemToken(t0, et) { ItemType = PhoneItemType.CityCode, Value = val.ToString(), IsInBrackets = true };
                }
                else 
                {
                    Pullenti.Ner.Core.TerminToken tt1 = m_PhoneTermins.TryParse(t0.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tt1 == null || tt1.Termin.Tag != null) 
                    {
                    }
                    else if (tt1.EndToken.Next == null || !tt1.EndToken.Next.IsChar(')')) 
                    {
                    }
                    else 
                        return new PhoneItemToken(t0, tt1.EndToken.Next) { ItemType = PhoneItemType.Prefix, IsInBrackets = true, Value = string.Empty };
                    return null;
                }
            }
            if ((t0.IsChar('/') && (t0.Next is Pullenti.Ner.NumberToken) && t0.Next.Next != null) && t0.Next.Next.IsChar('/') && t0.Next.LengthChar == 3) 
                return new PhoneItemToken(t0, t0.Next.Next) { ItemType = PhoneItemType.CityCode, Value = (t0.Next as Pullenti.Ner.NumberToken).Value.ToString(), IsInBrackets = true };
            Pullenti.Ner.Token t1 = null;
            Pullenti.Ner.Phone.PhoneKind ki = Pullenti.Ner.Phone.PhoneKind.Undefined;
            if ((t0.IsValue("Т", null) && t0.Next != null && t0.Next.IsCharOf("\\/")) && t0.Next.Next != null && ((t0.Next.Next.IsValue("Р", null) || t0.Next.Next.IsValue("М", null)))) 
            {
                t1 = t0.Next.Next;
                ki = (t1.IsValue("Р", null) ? Pullenti.Ner.Phone.PhoneKind.Work : Pullenti.Ner.Phone.PhoneKind.Mobile);
            }
            else 
            {
                Pullenti.Ner.Core.TerminToken tt = m_PhoneTermins.TryParse(t0, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tt == null || tt.Termin.Tag != null) 
                {
                    if (t0.IsValue("НОМЕР", null)) 
                    {
                        PhoneItemToken rr = _TryAttach(t0.Next);
                        if (rr != null && rr.ItemType == PhoneItemType.Prefix) 
                        {
                            rr.BeginToken = t0;
                            return rr;
                        }
                    }
                    return null;
                }
                if (tt.Termin.Tag2 is Pullenti.Ner.Phone.PhoneKind) 
                    ki = (Pullenti.Ner.Phone.PhoneKind)tt.Termin.Tag2;
                t1 = tt.EndToken;
            }
            PhoneItemToken res = new PhoneItemToken(t0, t1) { ItemType = PhoneItemType.Prefix, Value = string.Empty, Kind = ki };
            while (true) 
            {
                if (t1.Next != null && t1.Next.IsCharOf(".:")) 
                    res.EndToken = (t1 = t1.Next);
                else if (t1.Next != null && t1.Next.IsTableControlChar) 
                    t1 = t1.Next;
                else 
                    break;
            }
            if (t0 == t1 && ((t0.BeginChar == t0.EndChar || t0.Chars.IsAllUpper))) 
            {
                if (!t0.IsWhitespaceAfter) 
                    return null;
            }
            return res;
        }
        public static PhoneItemToken TryAttachAdditional(Pullenti.Ner.Token t0)
        {
            Pullenti.Ner.Token t = t0;
            if (t == null) 
                return null;
            if (t.IsChar(',')) 
                t = t.Next;
            else if (t.IsCharOf("*#") && (t.Next is Pullenti.Ner.NumberToken)) 
            {
                string val0 = (t.Next as Pullenti.Ner.NumberToken).GetSourceText();
                Pullenti.Ner.Token t1 = t.Next;
                if ((t1.Next != null && t1.Next.IsHiphen && !t1.IsWhitespaceAfter) && (t1.Next.Next is Pullenti.Ner.NumberToken) && !t1.Next.IsWhitespaceAfter) 
                {
                    t1 = t1.Next.Next;
                    val0 += t1.GetSourceText();
                }
                if (val0.Length >= 3 && (val0.Length < 7)) 
                    return new PhoneItemToken(t, t1) { ItemType = PhoneItemType.AddNumber, Value = val0 };
            }
            bool br = false;
            if (t != null && t.IsChar('(')) 
            {
                if (t.Previous != null && t.Previous.IsComma) 
                    return null;
                br = true;
                t = t.Next;
            }
            Pullenti.Ner.Core.TerminToken to = m_PhoneTermins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (to == null) 
            {
                if (!br) 
                    return null;
                if (t0.WhitespacesBeforeCount > 1) 
                    return null;
            }
            else if (to.Termin.Tag == null) 
                return null;
            else 
                t = to.EndToken.Next;
            if (t == null) 
                return null;
            if (((t.IsValue("НОМЕР", null) || t.IsValue("N", null) || t.IsValue("#", null)) || t.IsValue("№", null) || t.IsValue("NUMBER", null)) || ((t.IsChar('+') && br))) 
                t = t.Next;
            else if (to == null && !br) 
                return null;
            else if (t.IsValue("НОМ", null) || t.IsValue("ТЕЛ", null)) 
            {
                t = t.Next;
                if (t != null && t.IsChar('.')) 
                    t = t.Next;
            }
            if (t != null && t.IsCharOf(":,") && !t.IsNewlineAfter) 
                t = t.Next;
            if (!(t is Pullenti.Ner.NumberToken)) 
                return null;
            string val = (t as Pullenti.Ner.NumberToken).GetSourceText();
            if ((t.Next != null && t.Next.IsHiphen && !t.IsWhitespaceAfter) && (t.Next.Next is Pullenti.Ner.NumberToken)) 
            {
                val += t.Next.Next.GetSourceText();
                t = t.Next.Next;
            }
            if ((val.Length < 2) || val.Length > 7) 
                return null;
            if (br) 
            {
                if (t.Next == null || !t.Next.IsChar(')')) 
                    return null;
                t = t.Next;
            }
            PhoneItemToken res = new PhoneItemToken(t0, t) { ItemType = PhoneItemType.AddNumber, Value = val };
            return res;
        }
        public static List<PhoneItemToken> TryAttachAll(Pullenti.Ner.Token t0, int maxCount = 15)
        {
            if (t0 == null) 
                return null;
            PhoneItemToken p = TryAttach(t0);
            bool br = false;
            if (p == null && t0.IsChar('(')) 
            {
                br = true;
                p = TryAttach(t0.Next);
                if (p != null) 
                {
                    p.BeginToken = t0;
                    p.IsInBrackets = true;
                    if (p.ItemType == PhoneItemType.Prefix) 
                        br = false;
                }
            }
            if (p == null || p.ItemType == PhoneItemType.Delim) 
                return null;
            List<PhoneItemToken> res = new List<PhoneItemToken>();
            res.Add(p);
            Pullenti.Ner.Token t;
            for (t = p.EndToken.Next; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar) 
                {
                    if (res.Count == 1 && res[0].ItemType == PhoneItemType.Prefix) 
                        continue;
                    else 
                        break;
                }
                if (br && t.IsChar(')')) 
                {
                    br = false;
                    continue;
                }
                PhoneItemToken p0 = TryAttach(t);
                if (p0 == null) 
                {
                    if (t.IsNewlineBefore) 
                        break;
                    if (p.ItemType == PhoneItemType.Prefix && ((t.IsCharOf("\\/") || t.IsHiphen))) 
                    {
                        p0 = TryAttach(t.Next);
                        if (p0 != null && p0.ItemType == PhoneItemType.Prefix) 
                        {
                            p.EndToken = p0.EndToken;
                            t = p.EndToken;
                            continue;
                        }
                    }
                    if ((res[0].ItemType == PhoneItemType.Prefix && t.IsCharOf("\\/") && !t.IsWhitespaceAfter) && !t.IsWhitespaceBefore && (t.Next is Pullenti.Ner.NumberToken)) 
                    {
                        int sumNum = 0;
                        foreach (PhoneItemToken pp in res) 
                        {
                            if (pp.ItemType == PhoneItemType.CityCode || pp.ItemType == PhoneItemType.CountryCode || pp.ItemType == PhoneItemType.Number) 
                                sumNum += pp.Value.Length;
                        }
                        if (sumNum < 7) 
                        {
                            for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                            {
                                if (tt.IsWhitespaceBefore) 
                                    break;
                                else if (tt is Pullenti.Ner.NumberToken) 
                                    sumNum += tt.LengthChar;
                                else if ((tt is Pullenti.Ner.TextToken) && !tt.Chars.IsLetter) 
                                {
                                }
                                else 
                                    break;
                            }
                            if (sumNum == 10 || sumNum == 11) 
                                continue;
                        }
                    }
                    if (p0 == null) 
                        break;
                }
                if (t.IsNewlineBefore) 
                {
                    if (p.ItemType == PhoneItemType.Prefix) 
                    {
                    }
                    else 
                        break;
                }
                if (t.WhitespacesBeforeCount > 1) 
                {
                    bool ok = false;
                    foreach (PhoneItemToken pp in res) 
                    {
                        if (pp.ItemType == PhoneItemType.Prefix || pp.ItemType == PhoneItemType.CountryCode) 
                        {
                            ok = true;
                            break;
                        }
                    }
                    if (!ok) 
                        break;
                }
                if (br && p.ItemType == PhoneItemType.Number) 
                    p.ItemType = PhoneItemType.CityCode;
                p = p0;
                if (p.ItemType == PhoneItemType.Number && res[res.Count - 1].ItemType == PhoneItemType.Number) 
                    res.Add(new PhoneItemToken(t, t) { ItemType = PhoneItemType.Delim, Value = " " });
                if (br) 
                    p.IsInBrackets = true;
                res.Add(p);
                t = p.EndToken;
                if (res.Count > maxCount) 
                    break;
            }
            if ((((p = TryAttachAdditional(t)))) != null) 
                res.Add(p);
            for (int i = 1; i < (res.Count - 1); i++) 
            {
                if (res[i].ItemType == PhoneItemType.Delim && res[i + 1].IsInBrackets) 
                {
                    res.RemoveAt(i);
                    break;
                }
                else if (res[i].ItemType == PhoneItemType.Delim && res[i + 1].ItemType == PhoneItemType.Delim) 
                {
                    res[i].EndToken = res[i + 1].EndToken;
                    res.RemoveAt(i + 1);
                    i--;
                }
            }
            if ((res.Count > 1 && res[0].IsInBrackets && res[0].ItemType == PhoneItemType.Prefix) && res[res.Count - 1].EndToken.Next != null && res[res.Count - 1].EndToken.Next.IsChar(')')) 
                res[res.Count - 1].EndToken = res[res.Count - 1].EndToken.Next;
            if (res[0].ItemType == PhoneItemType.Prefix) 
            {
                for (int i = 2; i < (res.Count - 1); i++) 
                {
                    if (res[i].ItemType == PhoneItemType.Prefix && res[i + 1].ItemType != PhoneItemType.Prefix) 
                    {
                        res.RemoveRange(i, res.Count - i);
                        break;
                    }
                }
            }
            while (res.Count > 0) 
            {
                if (res[res.Count - 1].ItemType == PhoneItemType.Delim) 
                    res.RemoveAt(res.Count - 1);
                else 
                    break;
            }
            return res;
        }
        public static PhoneItemToken TryAttachAlternate(Pullenti.Ner.Token t0, Pullenti.Ner.Phone.PhoneReferent ph0, List<PhoneItemToken> pli)
        {
            if (t0 == null) 
                return null;
            if (t0.IsCharOf("\\/") && (t0.Next is Pullenti.Ner.NumberToken) && (t0.Next.EndChar - t0.Next.BeginChar) <= 1) 
            {
                List<PhoneItemToken> pli1 = PhoneItemToken.TryAttachAll(t0.Next, 15);
                if (pli1 != null && pli1.Count > 1) 
                {
                    if (pli1[pli1.Count - 1].ItemType == PhoneItemType.Delim) 
                        pli1.RemoveAt(pli1.Count - 1);
                    if (pli1.Count <= pli.Count) 
                    {
                        int ii;
                        string num = "";
                        for (ii = 0; ii < pli1.Count; ii++) 
                        {
                            PhoneItemToken p1 = pli1[ii];
                            PhoneItemToken p0 = pli[(pli.Count - pli1.Count) + ii];
                            if (p1.ItemType != p0.ItemType) 
                                break;
                            if (p1.ItemType != PhoneItemType.Number && p1.ItemType != PhoneItemType.Delim) 
                                break;
                            if (p1.ItemType == PhoneItemType.Number) 
                            {
                                if (p1.LengthChar != p0.LengthChar) 
                                    break;
                                num += p1.Value;
                            }
                        }
                        if (ii >= pli1.Count) 
                            return new PhoneItemToken(t0, pli1[pli1.Count - 1].EndToken) { ItemType = PhoneItemType.Alt, Value = num };
                    }
                }
                return new PhoneItemToken(t0, t0.Next) { ItemType = PhoneItemType.Alt, Value = t0.Next.GetSourceText() };
            }
            if (t0.IsHiphen && (t0.Next is Pullenti.Ner.NumberToken) && (t0.Next.EndChar - t0.Next.BeginChar) <= 1) 
            {
                Pullenti.Ner.Token t1 = t0.Next.Next;
                bool ok = false;
                if (t1 == null) 
                    ok = true;
                else if (t1.IsNewlineBefore || t1.IsCharOf(",.")) 
                    ok = true;
                if (ok) 
                    return new PhoneItemToken(t0, t0.Next) { ItemType = PhoneItemType.Alt, Value = t0.Next.GetSourceText() };
            }
            if ((t0.IsChar('(') && (t0.Next is Pullenti.Ner.NumberToken) && (t0.Next.EndChar - t0.Next.BeginChar) == 1) && t0.Next.Next != null && t0.Next.Next.IsChar(')')) 
                return new PhoneItemToken(t0, t0.Next.Next) { ItemType = PhoneItemType.Alt, Value = t0.Next.GetSourceText() };
            if ((t0.IsCharOf("/-") && (t0.Next is Pullenti.Ner.NumberToken) && ph0.m_Template != null) && Pullenti.Morph.LanguageHelper.EndsWith(ph0.m_Template, (((t0.Next.EndChar - t0.Next.BeginChar) + 1)).ToString())) 
                return new PhoneItemToken(t0, t0.Next) { ItemType = PhoneItemType.Alt, Value = t0.Next.GetSourceText() };
            return null;
        }
        public static void Initialize()
        {
            if (m_PhoneTermins != null) 
                return;
            m_PhoneTermins = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("ТЕЛЕФОН", Pullenti.Morph.MorphLang.RU, true);
            t.AddAbridge("ТЕЛ.");
            t.AddAbridge("TEL.");
            t.AddAbridge("Т-Н");
            t.AddAbridge("Т.");
            t.AddAbridge("T.");
            t.AddAbridge("TEL.EXT.");
            t.AddVariant("ТЛФ", false);
            t.AddVariant("ТЛФН", false);
            t.AddAbridge("Т/Ф");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("МОБИЛЬНЫЙ", Pullenti.Morph.MorphLang.RU, true) { Tag2 = Pullenti.Ner.Phone.PhoneKind.Mobile };
            t.AddAbridge("МОБ.");
            t.AddAbridge("Т.М.");
            t.AddAbridge("М.Т.");
            t.AddAbridge("М.");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СОТОВЫЙ", Pullenti.Morph.MorphLang.RU, true) { Tag2 = Pullenti.Ner.Phone.PhoneKind.Mobile };
            t.AddAbridge("СОТ.");
            t.AddAbridge("CELL.");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("РАБОЧИЙ", Pullenti.Morph.MorphLang.RU, true) { Tag2 = Pullenti.Ner.Phone.PhoneKind.Work };
            t.AddAbridge("РАБ.");
            t.AddAbridge("Т.Р.");
            t.AddAbridge("Р.Т.");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОРОДСКОЙ", Pullenti.Morph.MorphLang.RU, true);
            t.AddAbridge("ГОР.");
            t.AddAbridge("Г.Т.");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОМАШНИЙ", Pullenti.Morph.MorphLang.RU, true) { Tag2 = Pullenti.Ner.Phone.PhoneKind.Home };
            t.AddAbridge("ДОМ.");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОНТАКТНЫЙ", Pullenti.Morph.MorphLang.RU, true);
            t.AddVariant("КОНТАКТНЫЕ ДАННЫЕ", false);
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("МНОГОКАНАЛЬНЫЙ", Pullenti.Morph.MorphLang.RU, true);
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ФАКС", Pullenti.Morph.MorphLang.RU, true) { Tag2 = Pullenti.Ner.Phone.PhoneKind.Fax };
            t.AddAbridge("Ф.");
            t.AddAbridge("Т/ФАКС");
            t.AddAbridge("ТЕЛ/ФАКС");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗВОНИТЬ", Pullenti.Morph.MorphLang.RU, true);
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРИЕМНАЯ", Pullenti.Morph.MorphLang.RU, true) { Tag2 = Pullenti.Ner.Phone.PhoneKind.Work };
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("PHONE", Pullenti.Morph.MorphLang.EN, true);
            t.AddAbridge("PH.");
            t.AddVariant("TELEFON", true);
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("DIRECT LINE", Pullenti.Morph.MorphLang.EN, true) { Tag2 = Pullenti.Ner.Phone.PhoneKind.Work };
            t.AddVariant("DIRECT LINES", true);
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("MOBILE", Pullenti.Morph.MorphLang.EN, true) { Tag2 = Pullenti.Ner.Phone.PhoneKind.Mobile };
            t.AddAbridge("MOB.");
            t.AddVariant("MOBIL", true);
            t.AddAbridge("M.");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("FAX", Pullenti.Morph.MorphLang.EN, true) { Tag2 = Pullenti.Ner.Phone.PhoneKind.Fax };
            t.AddAbridge("F.");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("HOME", Pullenti.Morph.MorphLang.EN, true) { Tag2 = Pullenti.Ner.Phone.PhoneKind.Home };
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("CALL", Pullenti.Morph.MorphLang.EN, true);
            t.AddVariant("SEDIU", true);
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОПОЛНИТЕЛЬНЫЙ", Pullenti.Morph.MorphLang.RU, true);
            t.Tag = t;
            t.AddAbridge("ДОП.");
            t.AddAbridge("EXT.");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОБАВОЧНЫЙ", Pullenti.Morph.MorphLang.RU, true);
            t.Tag = t;
            t.AddAbridge("ДОБ.");
            t.AddAbridge("Д.");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВНУТРЕННИЙ", Pullenti.Morph.MorphLang.RU, true);
            t.Tag = t;
            t.AddAbridge("ВНУТР.");
            t.AddAbridge("ВН.");
            t.AddAbridge("ВНТ.");
            t.AddAbridge("Т.ВН.");
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("TONE MODE", Pullenti.Morph.MorphLang.EN, true);
            t.Tag = t;
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("TONE", Pullenti.Morph.MorphLang.EN, true);
            t.Tag = t;
            m_PhoneTermins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ADDITIONAL", Pullenti.Morph.MorphLang.EN, true);
            t.AddAbridge("ADD.");
            t.Tag = t;
            t.AddVariant("INTERNAL", true);
            t.AddAbridge("INT.");
            m_PhoneTermins.Add(t);
        }
        static Pullenti.Ner.Core.TerminCollection m_PhoneTermins;
    }
}