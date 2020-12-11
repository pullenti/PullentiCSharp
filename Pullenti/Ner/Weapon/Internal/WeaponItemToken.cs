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

namespace Pullenti.Ner.Weapon.Internal
{
    class WeaponItemToken : Pullenti.Ner.MetaToken
    {
        public WeaponItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public enum Typs : int
        {
            Noun,
            Brand,
            Model,
            Number,
            Name,
            Class,
            Date,
            Caliber,
            Developer,
        }

        public Typs Typ;
        public string Value;
        public string AltValue;
        public bool IsDoubt;
        public bool IsAfterConjunction;
        public bool IsInternal;
        List<WeaponItemToken> InnerTokens = new List<WeaponItemToken>();
        public Pullenti.Ner.Referent Ref;
        public override string ToString()
        {
            return string.Format("{0}: {1} {2}{3}", Typ.ToString(), Value ?? ((Ref == null ? "" : Ref.ToString())), AltValue ?? "", (IsInternal ? "[int]" : ""));
        }
        public static List<WeaponItemToken> TryParseList(Pullenti.Ner.Token t, int maxCount = 10)
        {
            WeaponItemToken tr = TryParse(t, null, false, false);
            if (tr == null) 
                return null;
            if (tr.Typ == Typs.Class || tr.Typ == Typs.Date) 
                return null;
            WeaponItemToken tr0 = tr;
            List<WeaponItemToken> res = new List<WeaponItemToken>();
            if (tr.InnerTokens.Count > 0) 
            {
                res.AddRange(tr.InnerTokens);
                if (res[0].BeginChar > tr.BeginChar) 
                    res[0].BeginToken = tr.BeginToken;
            }
            res.Add(tr);
            t = tr.EndToken.Next;
            if (tr.Typ == Typs.Noun) 
            {
                for (; t != null; t = t.Next) 
                {
                    if (t.IsChar(':') || t.IsHiphen) 
                    {
                    }
                    else 
                        break;
                }
            }
            bool andConj = false;
            for (; t != null; t = t.Next) 
            {
                if (maxCount > 0 && res.Count >= maxCount) 
                    break;
                if (t.IsChar(':')) 
                    continue;
                if (tr0.Typ == Typs.Noun) 
                {
                    if (t.IsHiphen && t.Next != null) 
                        t = t.Next;
                }
                tr = TryParse(t, tr0, false, false);
                if (tr == null) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t, true, null, false) && t.Next != null) 
                    {
                        if (tr0.Typ == Typs.Model || tr0.Typ == Typs.Brand) 
                        {
                            Pullenti.Ner.Token tt1 = t.Next;
                            if (tt1 != null && tt1.IsComma) 
                                tt1 = tt1.Next;
                            tr = TryParse(tt1, tr0, false, false);
                        }
                    }
                }
                if (tr == null && (t is Pullenti.Ner.ReferentToken)) 
                {
                    Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                    if (rt.BeginToken == rt.EndToken && (rt.BeginToken is Pullenti.Ner.TextToken)) 
                    {
                        tr = TryParse(rt.BeginToken, tr0, false, false);
                        if (tr != null && tr.BeginToken == tr.EndToken) 
                            tr.BeginToken = (tr.EndToken = t);
                    }
                }
                if (tr == null && t.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        Pullenti.Ner.Token tt = br.EndToken.Next;
                        if (tt != null && tt.IsComma) 
                            tt = tt.Next;
                        tr = TryParse(tt, tr0, false, false);
                        if (tr != null && tr.Typ == Typs.Number) 
                        {
                        }
                        else 
                            tr = null;
                    }
                }
                if (tr == null && t.IsHiphen) 
                {
                    if (tr0.Typ == Typs.Brand || tr0.Typ == Typs.Model) 
                        tr = TryParse(t.Next, tr0, false, false);
                }
                if (tr == null && t.IsComma) 
                {
                    if ((tr0.Typ == Typs.Name || tr0.Typ == Typs.Brand || tr0.Typ == Typs.Model) || tr0.Typ == Typs.Class || tr0.Typ == Typs.Date) 
                    {
                        tr = TryParse(t.Next, tr0, true, false);
                        if (tr != null) 
                        {
                            if (tr.Typ == Typs.Number) 
                            {
                            }
                            else 
                                tr = null;
                        }
                    }
                }
                if (tr == null) 
                    break;
                if (t.IsNewlineBefore) 
                {
                    if (tr.Typ != Typs.Number) 
                        break;
                }
                if (tr.InnerTokens.Count > 0) 
                    res.AddRange(tr.InnerTokens);
                res.Add(tr);
                tr0 = tr;
                t = tr.EndToken;
                if (andConj) 
                    break;
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (res[i].Typ == Typs.Model && res[i + 1].Typ == Typs.Model) 
                {
                    res[i].EndToken = res[i + 1].EndToken;
                    res[i].Value = string.Format("{0}{1}{2}", res[i].Value, (res[i].EndToken.Next != null && res[i].EndToken.Next.IsHiphen ? '-' : ' '), res[i + 1].Value);
                    res.RemoveAt(i + 1);
                    i--;
                }
            }
            return res;
        }
        public static WeaponItemToken TryParse(Pullenti.Ner.Token t, WeaponItemToken prev, bool afterConj, bool attachHigh = false)
        {
            WeaponItemToken res = _TryParse(t, prev, afterConj, attachHigh);
            if (res == null) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.Noun.BeginChar > npt.BeginChar) 
                {
                    res = _TryParse(npt.Noun.BeginToken, prev, afterConj, attachHigh);
                    if (res != null) 
                    {
                        if (res.Typ == Typs.Noun) 
                        {
                            string str = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                            if (str == "РУЧНОЙ ГРАНАТ") 
                                str = "РУЧНАЯ ГРАНАТА";
                            if (((str ?? "")).EndsWith(res.Value)) 
                            {
                                if (res.AltValue == null) 
                                    res.AltValue = str;
                                else 
                                {
                                    str = str.Substring(0, str.Length - res.Value.Length).Trim();
                                    res.AltValue = string.Format("{0} {1}", str, res.AltValue);
                                }
                                res.BeginToken = t;
                                return res;
                            }
                        }
                    }
                }
                return null;
            }
            if (res.Typ == Typs.Name) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(res.EndToken.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null && br.IsChar('(')) 
                {
                    string alt = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                    if (Pullenti.Ner.Core.MiscHelper.CanBeEqualCyrAndLatSS(res.Value, alt)) 
                    {
                        res.AltValue = alt;
                        res.EndToken = br.EndToken;
                    }
                }
            }
            return res;
        }
        static WeaponItemToken _TryParse(Pullenti.Ner.Token t, WeaponItemToken prev, bool afterConj, bool attachHigh = false)
        {
            if (t == null) 
                return null;
            if (Pullenti.Ner.Core.BracketHelper.IsBracket(t, true)) 
            {
                WeaponItemToken wit = _TryParse(t.Next, prev, afterConj, attachHigh);
                if (wit != null) 
                {
                    if (wit.EndToken.Next == null) 
                    {
                        wit.BeginToken = t;
                        return wit;
                    }
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(wit.EndToken.Next, true)) 
                    {
                        wit.BeginToken = t;
                        wit.EndToken = wit.EndToken.Next;
                        return wit;
                    }
                }
            }
            Pullenti.Ner.Core.TerminToken tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null) 
            {
                WeaponItemToken res = new WeaponItemToken(t, tok.EndToken);
                res.Typ = (Typs)tok.Termin.Tag;
                if (res.Typ == Typs.Noun) 
                {
                    res.Value = tok.Termin.CanonicText;
                    if (tok.Termin.Tag2 != null) 
                        res.IsDoubt = true;
                    for (Pullenti.Ner.Token tt = res.EndToken.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.WhitespacesBeforeCount > 2) 
                            break;
                        WeaponItemToken wit = _TryParse(tt, null, false, false);
                        if (wit != null) 
                        {
                            if (wit.Typ == Typs.Brand) 
                            {
                                res.InnerTokens.Add(wit);
                                res.EndToken = (tt = wit.EndToken);
                                continue;
                            }
                            break;
                        }
                        if (!(tt is Pullenti.Ner.TextToken)) 
                            break;
                        Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                        if (mc == Pullenti.Morph.MorphClass.Adjective) 
                        {
                            if (res.AltValue == null) 
                                res.AltValue = res.Value;
                            if (res.AltValue.EndsWith(res.Value)) 
                                res.AltValue = res.AltValue.Substring(0, res.AltValue.Length - res.Value.Length);
                            res.AltValue = string.Format("{0}{1} {2}", res.AltValue, (tt as Pullenti.Ner.TextToken).Term, res.Value);
                            res.EndToken = tt;
                            continue;
                        }
                        break;
                    }
                    return res;
                }
                if (res.Typ == Typs.Brand || res.Typ == Typs.Name) 
                {
                    res.Value = tok.Termin.CanonicText;
                    return res;
                }
                if (res.Typ == Typs.Model) 
                {
                    res.Value = tok.Termin.CanonicText;
                    if (tok.Termin.Tag2 is List<Pullenti.Ner.Core.Termin>) 
                    {
                        List<Pullenti.Ner.Core.Termin> li = tok.Termin.Tag2 as List<Pullenti.Ner.Core.Termin>;
                        foreach (Pullenti.Ner.Core.Termin to in li) 
                        {
                            WeaponItemToken wit = new WeaponItemToken(t, tok.EndToken) { Typ = (Typs)to.Tag, Value = to.CanonicText, IsInternal = tok.BeginToken == tok.EndToken };
                            res.InnerTokens.Add(wit);
                            if (to.AdditionalVars != null && to.AdditionalVars.Count > 0) 
                                wit.AltValue = to.AdditionalVars[0].CanonicText;
                        }
                    }
                    res._correctModel();
                    return res;
                }
            }
            Pullenti.Ner.Token nnn = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t);
            if (nnn != null) 
            {
                Pullenti.Ner.Transport.Internal.TransItemToken tit = Pullenti.Ner.Transport.Internal.TransItemToken._attachNumber(nnn, true);
                if (tit != null) 
                {
                    WeaponItemToken res = new WeaponItemToken(t, tit.EndToken) { Typ = Typs.Number };
                    res.Value = tit.Value;
                    res.AltValue = tit.AltValue;
                    return res;
                }
            }
            if (((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && t.Chars.IsAllUpper) && (t.LengthChar < 4)) 
            {
                if ((t.Next != null && ((t.Next.IsHiphen || t.Next.IsChar('.'))) && (t.Next.WhitespacesAfterCount < 2)) && (t.Next.Next is Pullenti.Ner.NumberToken)) 
                {
                    WeaponItemToken res = new WeaponItemToken(t, t.Next) { Typ = Typs.Model, IsDoubt = true };
                    res.Value = (t as Pullenti.Ner.TextToken).Term;
                    res._correctModel();
                    return res;
                }
                if ((t.Next is Pullenti.Ner.NumberToken) && !t.IsWhitespaceAfter) 
                {
                    WeaponItemToken res = new WeaponItemToken(t, t) { Typ = Typs.Model, IsDoubt = true };
                    res.Value = (t as Pullenti.Ner.TextToken).Term;
                    res._correctModel();
                    return res;
                }
                if ((t as Pullenti.Ner.TextToken).Term == "СП" && (t.WhitespacesAfterCount < 3) && (t.Next is Pullenti.Ner.TextToken)) 
                {
                    WeaponItemToken pp = _TryParse(t.Next, null, false, false);
                    if (pp != null && ((pp.Typ == Typs.Model || pp.Typ == Typs.Brand))) 
                    {
                        WeaponItemToken res = new WeaponItemToken(t, t) { Typ = Typs.Noun };
                        res.Value = "ПИСТОЛЕТ";
                        res.AltValue = "СЛУЖЕБНЫЙ ПИСТОЛЕТ";
                        return res;
                    }
                }
            }
            if (((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && !t.Chars.IsAllLower) && t.LengthChar > 2) 
            {
                bool ok = false;
                if (prev != null && ((prev.Typ == Typs.Noun || prev.Typ == Typs.Model || prev.Typ == Typs.Brand))) 
                    ok = true;
                else if (prev == null && t.Previous != null && t.Previous.IsCommaAnd) 
                    ok = true;
                if (ok) 
                {
                    WeaponItemToken res = new WeaponItemToken(t, t) { Typ = Typs.Name, IsDoubt = true };
                    res.Value = (t as Pullenti.Ner.TextToken).Term;
                    if ((t.Next != null && t.Next.IsHiphen && (t.Next.Next is Pullenti.Ner.TextToken)) && t.Next.Next.Chars == t.Chars) 
                    {
                        res.Value = string.Format("{0}-{1}", res.Value, (t.Next.Next as Pullenti.Ner.TextToken).Term);
                        res.EndToken = t.Next.Next;
                    }
                    if (prev != null && prev.Typ == Typs.Noun) 
                        res.Typ = Typs.Brand;
                    if (res.EndToken.Next != null && res.EndToken.Next.IsHiphen && (res.EndToken.Next.Next is Pullenti.Ner.NumberToken)) 
                    {
                        res.Typ = Typs.Model;
                        res._correctModel();
                    }
                    else if (!res.EndToken.IsWhitespaceAfter && (res.EndToken.Next is Pullenti.Ner.NumberToken)) 
                    {
                        res.Typ = Typs.Model;
                        res._correctModel();
                    }
                    return res;
                }
            }
            if (t.IsValue("МАРКА", null)) 
            {
                WeaponItemToken res = _TryParse(t.Next, prev, afterConj, false);
                if (res != null && res.Typ == Typs.Brand) 
                {
                    res.BeginToken = t;
                    return res;
                }
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Next, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                        return new WeaponItemToken(t, br.EndToken) { Typ = Typs.Brand, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken, br.EndToken, Pullenti.Ner.Core.GetTextAttr.No) };
                }
                if (((t is Pullenti.Ner.TextToken) && (t.Next is Pullenti.Ner.TextToken) && t.Next.LengthChar > 1) && !t.Next.Chars.IsAllLower) 
                    return new WeaponItemToken(t, t.Next) { Typ = Typs.Brand, Value = (t as Pullenti.Ner.TextToken).Term };
            }
            if (t.IsValue("КАЛИБР", "КАЛІБР")) 
            {
                Pullenti.Ner.Token tt1 = t.Next;
                if (tt1 != null && ((tt1.IsHiphen || tt1.IsChar(':')))) 
                    tt1 = tt1.Next;
                Pullenti.Ner.Measure.Internal.NumbersWithUnitToken num = Pullenti.Ner.Measure.Internal.NumbersWithUnitToken.TryParse(tt1, null, false, false, false, false);
                if (num != null && num.SingleVal != null) 
                    return new WeaponItemToken(t, num.EndToken) { Typ = Typs.Caliber, Value = Pullenti.Ner.Core.NumberHelper.DoubleToString(num.SingleVal.Value) };
            }
            if (t is Pullenti.Ner.NumberToken) 
            {
                Pullenti.Ner.Measure.Internal.NumbersWithUnitToken num = Pullenti.Ner.Measure.Internal.NumbersWithUnitToken.TryParse(t, null, false, false, false, false);
                if (num != null && num.SingleVal != null) 
                {
                    if (num.Units.Count == 1 && num.Units[0].Unit != null && num.Units[0].Unit.NameCyr == "мм") 
                        return new WeaponItemToken(t, num.EndToken) { Typ = Typs.Caliber, Value = Pullenti.Ner.Core.NumberHelper.DoubleToString(num.SingleVal.Value) };
                    if (num.EndToken.Next != null && num.EndToken.Next.IsValue("КАЛИБР", "КАЛІБР")) 
                        return new WeaponItemToken(t, num.EndToken.Next) { Typ = Typs.Caliber, Value = Pullenti.Ner.Core.NumberHelper.DoubleToString(num.SingleVal.Value) };
                }
            }
            if (t.IsValue("ПРОИЗВОДСТВО", "ВИРОБНИЦТВО")) 
            {
                Pullenti.Ner.Token tt1 = t.Next;
                if (tt1 != null && ((tt1.IsHiphen || tt1.IsChar(':')))) 
                    tt1 = tt1.Next;
                if (tt1 is Pullenti.Ner.ReferentToken) 
                {
                    if ((tt1.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) || (tt1.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                        return new WeaponItemToken(t, tt1) { Typ = Typs.Developer, Ref = tt1.GetReferent() };
                }
            }
            return null;
        }
        void _correctModel()
        {
            Pullenti.Ner.Token tt = EndToken.Next;
            if (tt == null || tt.WhitespacesBeforeCount > 2) 
                return;
            if (tt.IsValue(":\\/.", null) || tt.IsHiphen) 
                tt = tt.Next;
            if (tt is Pullenti.Ner.NumberToken) 
            {
                StringBuilder tmp = new StringBuilder();
                tmp.Append((tt as Pullenti.Ner.NumberToken).Value);
                bool isLat = Pullenti.Morph.LanguageHelper.IsLatinChar(Value[0]);
                EndToken = tt;
                for (tt = tt.Next; tt != null; tt = tt.Next) 
                {
                    if ((tt is Pullenti.Ner.TextToken) && tt.LengthChar == 1 && tt.Chars.IsLetter) 
                    {
                        if (!tt.IsWhitespaceBefore || ((tt.Previous != null && tt.Previous.IsHiphen))) 
                        {
                            char ch = (tt as Pullenti.Ner.TextToken).Term[0];
                            EndToken = tt;
                            char ch2 = (char)0;
                            if (Pullenti.Morph.LanguageHelper.IsLatinChar(ch) && !isLat) 
                            {
                                ch2 = Pullenti.Morph.LanguageHelper.GetCyrForLat(ch);
                                if (ch2 != ((char)0)) 
                                    ch = ch2;
                            }
                            else if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(ch) && isLat) 
                            {
                                ch2 = Pullenti.Morph.LanguageHelper.GetLatForCyr(ch);
                                if (ch2 != ((char)0)) 
                                    ch = ch2;
                            }
                            tmp.Append(ch);
                            continue;
                        }
                    }
                    break;
                }
                Value = string.Format("{0}-{1}", Value, tmp.ToString());
                AltValue = Pullenti.Ner.Core.MiscHelper.CreateCyrLatAlternative(Value);
            }
            if (!EndToken.IsWhitespaceAfter && EndToken.Next != null && ((EndToken.Next.IsHiphen || EndToken.Next.IsCharOf("\\/")))) 
            {
                if (!EndToken.Next.IsWhitespaceAfter && (EndToken.Next.Next is Pullenti.Ner.NumberToken)) 
                {
                    EndToken = EndToken.Next.Next;
                    Value = string.Format("{0}-{1}", Value, (EndToken as Pullenti.Ner.NumberToken).Value);
                    if (AltValue != null) 
                        AltValue = string.Format("{0}-{1}", AltValue, (EndToken as Pullenti.Ner.NumberToken).Value);
                }
            }
        }
        static Pullenti.Ner.Core.TerminCollection m_Ontology;
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            Pullenti.Ner.Core.Termin tt;
            List<Pullenti.Ner.Core.Termin> li;
            t = new Pullenti.Ner.Core.Termin("ПИСТОЛЕТ") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("РЕВОЛЬВЕР") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВИНТОВКА") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("РУЖЬЕ") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АВТОМАТ") { Tag = Typs.Noun, Tag2 = 1 };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КАРАБИН") { Tag = Typs.Noun, Tag2 = 1 };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПИСТОЛЕТ-ПУЛЕМЕТ") { CanonicText = "ПИСТОЛЕТ-ПУЛЕМЕТ", Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПУЛЕМЕТ") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГРАНАТОМЕТ") { Tag = Typs.Noun };
            t.AddVariant("СТРЕЛКОВО ГРАНАТОМЕТНЫЙ КОМПЛЕКС", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОГНЕМЕТ") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИНОМЕТ") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПЕРЕНОСНОЙ ЗЕНИТНО РАКЕТНЫЙ КОМПЛЕКС") { Acronym = "ПЗРК", Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОТИВОТАНКОВЫЙ РАКЕТНЫЙ КОМПЛЕКС") { Acronym = "ПТРК", Tag = Typs.Noun };
            t.AddVariant("ПЕРЕНОСНОЙ ПРОТИВОТАНКОВЫЙ РАКЕТНЫЙ КОМПЛЕКС", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АВИАЦИОННАЯ ПУШКА") { Tag = Typs.Noun };
            t.AddVariant("АВИАПУШКА", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НАРУЧНИКИ") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("БРОНЕЖИЛЕТ") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГРАНАТА") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛИМОНКА") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НОЖ") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЗРЫВАТЕЛЬ") { Tag = Typs.Noun };
            m_Ontology.Add(t);
            foreach (string s in new string[] {"МАКАРОВ", "КАЛАШНИКОВ", "СИМОНОВ", "СТЕЧКИН", "ШМАЙСЕР", "МОСИН", "СЛОСТИН", "НАГАН", "МАКСИМ", "ДРАГУНОВ", "СЕРДЮКОВ", "ЯРЫГИН", "НИКОНОВ", "МАУЗЕР", "БРАУНИНГ", "КОЛЬТ", "ВИНЧЕСТЕР"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Typs.Brand });
            }
            foreach (string s in new string[] {"УЗИ"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Typs.Name });
            }
            t = new Pullenti.Ner.Core.Termin("ТУЛЬСКИЙ ТОКАРЕВА") { CanonicText = "ТТ", Acronym = "ТТ", Tag = Typs.Model };
            li = new List<Pullenti.Ner.Core.Termin>();
            li.Add(new Pullenti.Ner.Core.Termin("ПИСТОЛЕТ") { Tag = Typs.Noun });
            li.Add(new Pullenti.Ner.Core.Termin("ТОКАРЕВ") { Tag = Typs.Brand });
            t.Tag2 = li;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПИСТОЛЕТ МАКАРОВА") { CanonicText = "ПМ", Acronym = "ПМ", Tag = Typs.Model };
            li = new List<Pullenti.Ner.Core.Termin>();
            li.Add(new Pullenti.Ner.Core.Termin("ПИСТОЛЕТ") { Tag = Typs.Noun });
            li.Add(new Pullenti.Ner.Core.Termin("МАКАРОВ") { Tag = Typs.Brand });
            t.Tag2 = li;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПИСТОЛЕТ МАКАРОВА МОДЕРНИЗИРОВАННЫЙ") { CanonicText = "ПММ", Acronym = "ПММ", Tag = Typs.Model };
            li = new List<Pullenti.Ner.Core.Termin>();
            li.Add((tt = new Pullenti.Ner.Core.Termin("ПИСТОЛЕТ") { Tag = Typs.Noun }));
            tt.AddVariant("МОДЕРНИЗИРОВАННЫЙ ПИСТОЛЕТ", false);
            li.Add(new Pullenti.Ner.Core.Termin("МАКАРОВ") { Tag = Typs.Brand });
            t.Tag2 = li;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АВТОМАТ КАЛАШНИКОВА") { CanonicText = "АК", Acronym = "АК", Tag = Typs.Model };
            li = new List<Pullenti.Ner.Core.Termin>();
            li.Add(new Pullenti.Ner.Core.Termin("АВТОМАТ") { Tag = Typs.Noun });
            li.Add(new Pullenti.Ner.Core.Termin("КАЛАШНИКОВ") { Tag = Typs.Brand });
            t.Tag2 = li;
            m_Ontology.Add(t);
        }
    }
}