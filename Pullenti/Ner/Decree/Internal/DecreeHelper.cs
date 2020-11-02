/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Decree.Internal
{
    // Некоторые полезные функции для НПА
    public static class DecreeHelper
    {
        public static DateTime? ParseDateTime(string str)
        {
            if (string.IsNullOrEmpty(str)) 
                return null;
            try 
            {
                string[] prts = str.Split('.');
                int y;
                if (!int.TryParse(prts[0], out y)) 
                    return null;
                int mon = 0;
                int day = 0;
                if (prts.Length > 1) 
                {
                    if (int.TryParse(prts[1], out mon)) 
                    {
                        if (prts.Length > 2) 
                            int.TryParse(prts[2], out day);
                    }
                }
                if (mon <= 0) 
                    mon = 1;
                if (day <= 0) 
                    day = 1;
                if (day > DateTime.DaysInMonth(y, mon)) 
                    day = DateTime.DaysInMonth(y, mon);
                return new DateTime(y, mon, day);
            }
            catch(Exception ex) 
            {
            }
            return null;
        }
        /// <summary>
        /// Это для оформления ссылок по некоторым стандартам (когда гиперссылкой нужно выделить не всю сущность, 
        /// а лишь некоторую её часть)
        /// </summary>
        public static CanonicDecreeRefUri TryCreateCanonicDecreeRefUri(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.ReferentToken)) 
                return null;
            Pullenti.Ner.Decree.DecreeReferent dr = t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
            CanonicDecreeRefUri res;
            if (dr != null) 
            {
                if (dr.Kind == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                    return null;
                res = new CanonicDecreeRefUri(t.Kit.Sofa.Text) { Ref = dr, BeginChar = t.BeginChar, EndChar = t.EndChar };
                if ((t.Previous != null && t.Previous.IsChar('(') && t.Next != null) && t.Next.IsChar(')')) 
                    return res;
                if ((t as Pullenti.Ner.ReferentToken).MiscAttrs != 0) 
                    return res;
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                if (rt.BeginToken.IsChar('(') && rt.EndToken.IsChar(')')) 
                {
                    res = new CanonicDecreeRefUri(t.Kit.Sofa.Text) { Ref = dr, BeginChar = rt.BeginToken.Next.BeginChar, EndChar = rt.EndToken.Previous.EndChar };
                    return res;
                }
                List<DecreeToken> nextDecreeItems = null;
                if ((t.Next != null && t.Next.IsCommaAnd && (t.Next.Next is Pullenti.Ner.ReferentToken)) && (t.Next.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                {
                    nextDecreeItems = DecreeToken.TryAttachList((t.Next.Next as Pullenti.Ner.ReferentToken).BeginToken, null, 10, false);
                    if (nextDecreeItems != null && nextDecreeItems.Count > 1) 
                    {
                        for (int i = 0; i < (nextDecreeItems.Count - 1); i++) 
                        {
                            if (nextDecreeItems[i].IsNewlineAfter) 
                            {
                                nextDecreeItems.RemoveRange(i + 1, nextDecreeItems.Count - i - 1);
                                break;
                            }
                        }
                    }
                }
                bool wasTyp = false;
                bool wasNum = false;
                for (Pullenti.Ner.Token tt = (t as Pullenti.Ner.MetaToken).BeginToken; tt != null && tt.EndChar <= t.EndChar; tt = tt.Next) 
                {
                    if (tt.BeginChar == t.BeginChar && tt.IsChar('(') && tt.Next != null) 
                        res.BeginChar = tt.Next.BeginChar;
                    if (tt.IsChar('(') && tt.Next != null && tt.Next.IsValue("ДАЛЕЕ", null)) 
                    {
                        if (res.EndChar >= tt.BeginChar) 
                            res.EndChar = tt.Previous.EndChar;
                        break;
                    }
                    if (tt.EndChar == t.EndChar && tt.IsChar(')')) 
                    {
                        res.EndChar = tt.Previous.EndChar;
                        for (Pullenti.Ner.Token tt1 = tt.Previous; tt1 != null && tt1.BeginChar >= res.BeginChar; tt1 = tt1.Previous) 
                        {
                            if (tt1.IsChar('(') && tt1.Previous != null) 
                            {
                                if (res.BeginChar < tt1.Previous.BeginChar) 
                                    res.EndChar = tt1.Previous.EndChar;
                            }
                        }
                    }
                    List<DecreeToken> li = DecreeToken.TryAttachList(tt, null, 10, false);
                    if (li != null && li.Count > 0) 
                    {
                        for (int ii = 0; ii < (li.Count - 1); ii++) 
                        {
                            if (li[ii].Typ == DecreeToken.ItemType.Typ && li[ii + 1].Typ == DecreeToken.ItemType.Terr) 
                                res.TypeWithGeo = Pullenti.Ner.Core.MiscHelper.GetTextValue(li[ii].BeginToken, li[ii + 1].EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle);
                        }
                        if ((nextDecreeItems != null && nextDecreeItems.Count > 1 && (nextDecreeItems.Count < li.Count)) && nextDecreeItems[0].Typ != DecreeToken.ItemType.Typ) 
                        {
                            int d = li.Count - nextDecreeItems.Count;
                            int j;
                            for (j = 0; j < nextDecreeItems.Count; j++) 
                            {
                                if (nextDecreeItems[j].Typ != li[d + j].Typ) 
                                    break;
                            }
                            if (j >= nextDecreeItems.Count) 
                            {
                                li.RemoveRange(0, d);
                                res.BeginChar = li[0].BeginChar;
                            }
                        }
                        else if ((nextDecreeItems != null && nextDecreeItems.Count == 1 && nextDecreeItems[0].Typ == DecreeToken.ItemType.Name) && li.Count == 2 && li[1].Typ == DecreeToken.ItemType.Name) 
                        {
                            res.BeginChar = li[1].BeginChar;
                            res.EndChar = li[1].EndChar;
                            break;
                        }
                        else if ((nextDecreeItems != null && nextDecreeItems.Count == 1 && nextDecreeItems[0].Typ == DecreeToken.ItemType.Number) && li[li.Count - 1].Typ == DecreeToken.ItemType.Number) 
                        {
                            res.BeginChar = li[li.Count - 1].BeginChar;
                            res.EndChar = li[li.Count - 1].EndChar;
                        }
                        for (int i = 0; i < li.Count; i++) 
                        {
                            DecreeToken l = li[i];
                            if (l.BeginChar > t.EndChar) 
                            {
                                li.RemoveRange(i, li.Count - i);
                                break;
                            }
                            if (l.Typ == DecreeToken.ItemType.Name) 
                            {
                                if (!wasNum) 
                                {
                                    if (dr.Kind == Pullenti.Ner.Decree.DecreeKind.Contract) 
                                        continue;
                                    if (((i + 1) < li.Count) && ((li[i + 1].Typ == DecreeToken.ItemType.Date || li[i + 1].Typ == DecreeToken.ItemType.Number))) 
                                        continue;
                                }
                                int ee = l.BeginToken.Previous.EndChar;
                                if (ee > res.BeginChar && (ee < res.EndChar)) 
                                    res.EndChar = ee;
                                break;
                            }
                            if (l.Typ == DecreeToken.ItemType.Number) 
                                wasNum = true;
                            if (i == 0) 
                            {
                                if (l.Typ == DecreeToken.ItemType.Typ) 
                                    wasTyp = true;
                                else if (l.Typ == DecreeToken.ItemType.Owner || l.Typ == DecreeToken.ItemType.Org) 
                                {
                                    if (((i + 1) < li.Count) && ((li[1].Typ == DecreeToken.ItemType.Date || li[1].Typ == DecreeToken.ItemType.Number))) 
                                        wasTyp = true;
                                }
                                if (wasTyp) 
                                {
                                    Pullenti.Ner.Token tt0 = l.BeginToken.Previous;
                                    if (tt0 != null && tt0.IsChar('.')) 
                                        tt0 = tt0.Previous;
                                    if (tt0 != null && ((tt0.IsValue("УТВЕРЖДЕННЫЙ", null) || tt0.IsValue("УТВЕРДИТЬ", null) || tt0.IsValue("УТВ", null)))) 
                                    {
                                        if (l.BeginChar > res.BeginChar) 
                                        {
                                            res.BeginChar = l.BeginChar;
                                            if (res.EndChar < res.BeginChar) 
                                                res.EndChar = t.EndChar;
                                            res.IsAdopted = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (li.Count > 0) 
                        {
                            tt = li[li.Count - 1].EndToken;
                            if (tt.IsChar(')')) 
                                tt = tt.Previous;
                            continue;
                        }
                    }
                    if (wasTyp) 
                    {
                        DecreeToken na = DecreeToken.TryAttachName(tt, dr.Typ0, true, false);
                        if (na != null && tt.BeginChar > t.BeginChar) 
                        {
                            Pullenti.Ner.Token tt1 = na.EndToken.Next;
                            if (tt1 != null && tt1.IsCharOf(",()")) 
                                tt1 = tt1.Next;
                            if (tt1 != null && (tt1.EndChar < t.EndChar)) 
                            {
                                if (tt1.IsValue("УТВЕРЖДЕННЫЙ", null) || tt1.IsValue("УТВЕРДИТЬ", null) || tt1.IsValue("УТВ", null)) 
                                {
                                    tt = tt1;
                                    continue;
                                }
                            }
                            if (tt.Previous != null && tt.Previous.IsChar(':') && na.EndChar <= res.EndChar) 
                            {
                                res.BeginChar = tt.BeginChar;
                                break;
                            }
                            if (tt.Previous.EndChar > res.BeginChar) 
                            {
                                res.EndChar = tt.Previous.EndChar;
                                break;
                            }
                        }
                    }
                }
                return res;
            }
            Pullenti.Ner.Decree.DecreePartReferent dpr = t.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
            if (dpr == null) 
                return null;
            if ((t.Previous != null && t.Previous.IsHiphen && (t.Previous.Previous is Pullenti.Ner.ReferentToken)) && (t.Previous.Previous.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent)) 
            {
                if (Pullenti.Ner.Decree.DecreePartReferent.CreateRangeReferent(t.Previous.Previous.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent, dpr) != null) 
                    return null;
            }
            Pullenti.Ner.Token t1 = t;
            bool hasDiap = false;
            Pullenti.Ner.ReferentToken DiapRef = null;
            if ((t.Next != null && t.Next.IsHiphen && (t.Next.Next is Pullenti.Ner.ReferentToken)) && (t.Next.Next.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent)) 
            {
                Pullenti.Ner.Decree.DecreePartReferent diap = Pullenti.Ner.Decree.DecreePartReferent.CreateRangeReferent(dpr as Pullenti.Ner.Decree.DecreePartReferent, t.Next.Next.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent);
                if (diap != null) 
                {
                    dpr = diap;
                    hasDiap = true;
                    t1 = t.Next.Next;
                    DiapRef = t1 as Pullenti.Ner.ReferentToken;
                }
            }
            res = new CanonicDecreeRefUri(t.Kit.Sofa.Text) { Ref = dpr, BeginChar = t.BeginChar, EndChar = t1.EndChar, IsDiap = hasDiap };
            if ((t.Previous != null && t.Previous.IsChar('(') && t1.Next != null) && t1.Next.IsChar(')')) 
                return res;
            for (Pullenti.Ner.Token tt = (t as Pullenti.Ner.MetaToken).BeginToken; tt != null && tt.EndChar <= t.EndChar; tt = tt.Next) 
            {
                if (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                {
                    if (tt.BeginChar > t.BeginChar) 
                    {
                        res.EndChar = tt.Previous.EndChar;
                        if (tt.Previous.Morph.Class.IsPreposition && tt.Previous.Previous != null) 
                            res.EndChar = tt.Previous.Previous.EndChar;
                    }
                    else if (tt.EndChar < t.EndChar) 
                        res.BeginChar = tt.BeginChar;
                    break;
                }
            }
            bool hasSameBefore = _hasSameDecree(t, dpr, true);
            bool hasSameAfter = _hasSameDecree(t, dpr, false);
            PartToken.ItemType ptmin = PartToken.ItemType.Prefix;
            PartToken.ItemType ptmin2 = PartToken.ItemType.Prefix;
            int max = 0;
            int max2 = 00;
            foreach (Pullenti.Ner.Slot s in dpr.Slots) 
            {
                PartToken.ItemType pt = PartToken._getTypeByAttrName(s.TypeName);
                if (pt == PartToken.ItemType.Prefix) 
                    continue;
                int co = PartToken._getRank(pt);
                if (co < 1) 
                {
                    if (pt == PartToken.ItemType.Part && dpr.FindSlot(Pullenti.Ner.Decree.DecreePartReferent.ATTR_CLAUSE, null, true) != null) 
                        co = PartToken._getRank(PartToken.ItemType.Paragraph);
                    else 
                        continue;
                }
                if (co > max) 
                {
                    max2 = max;
                    ptmin2 = ptmin;
                    max = co;
                    ptmin = pt;
                }
                else if (co > max2) 
                {
                    max2 = co;
                    ptmin2 = pt;
                }
            }
            if (ptmin != PartToken.ItemType.Prefix) 
            {
                for (Pullenti.Ner.Token tt = (t as Pullenti.Ner.MetaToken).BeginToken; tt != null && tt.EndChar <= res.EndChar; tt = tt.Next) 
                {
                    if (tt.BeginChar >= res.BeginChar) 
                    {
                        PartToken pt = PartToken.TryAttach(tt, null, false, false);
                        if (pt != null && pt.Typ == ptmin) 
                        {
                            res.BeginChar = pt.BeginChar;
                            res.EndChar = pt.EndChar;
                            if (pt.Typ == PartToken.ItemType.Appendix && pt.EndToken.IsValue("К", null) && pt.BeginToken != pt.EndToken) 
                                res.EndChar = pt.EndToken.Previous.EndChar;
                            if (pt.EndChar == t.EndChar) 
                            {
                                if ((t.Next != null && t.Next.IsCommaAnd && (t.Next.Next is Pullenti.Ner.ReferentToken)) && (t.Next.Next.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent)) 
                                {
                                    Pullenti.Ner.Token tt1 = (t.Next.Next as Pullenti.Ner.ReferentToken).BeginToken;
                                    bool ok = true;
                                    if (tt1.Chars.IsLetter) 
                                        ok = false;
                                    if (ok) 
                                    {
                                        foreach (PartToken.PartValue v in pt.Values) 
                                        {
                                            res.BeginChar = v.BeginChar;
                                            res.EndChar = v.EndChar;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!hasDiap) 
                                return res;
                            break;
                        }
                    }
                }
                if (hasDiap && DiapRef != null) 
                {
                    for (Pullenti.Ner.Token tt = DiapRef.BeginToken; tt != null && tt.EndChar <= DiapRef.EndChar; tt = tt.Next) 
                    {
                        if (tt.IsChar(',')) 
                            break;
                        if (tt != DiapRef.BeginToken && tt.IsWhitespaceBefore) 
                            break;
                        res.EndChar = tt.EndChar;
                    }
                    return res;
                }
            }
            if (((hasSameBefore || hasSameAfter)) && ptmin != PartToken.ItemType.Prefix) 
            {
                for (Pullenti.Ner.Token tt = (t as Pullenti.Ner.MetaToken).BeginToken; tt != null && tt.EndChar <= res.EndChar; tt = tt.Next) 
                {
                    if (tt.BeginChar >= res.BeginChar) 
                    {
                        PartToken pt = (!hasSameBefore ? PartToken.TryAttach(tt, null, false, false) : null);
                        if (pt != null) 
                        {
                            if (pt.Typ == ptmin) 
                            {
                                foreach (PartToken.PartValue v in pt.Values) 
                                {
                                    res.BeginChar = v.BeginChar;
                                    res.EndChar = v.EndChar;
                                    return res;
                                }
                            }
                            tt = pt.EndToken;
                            continue;
                        }
                        if ((tt is Pullenti.Ner.NumberToken) && tt.BeginChar == res.BeginChar) 
                        {
                            res.EndChar = tt.EndChar;
                            for (; tt != null && tt.Next != null; ) 
                            {
                                if (!tt.Next.IsChar('.') || tt.IsWhitespaceAfter || tt.Next.IsWhitespaceAfter) 
                                    break;
                                if (!(tt.Next.Next is Pullenti.Ner.NumberToken)) 
                                    break;
                                tt = tt.Next.Next;
                                res.EndChar = tt.EndChar;
                            }
                            if (tt.Next != null && tt.Next.IsHiphen) 
                            {
                                if (tt.Next.Next is Pullenti.Ner.NumberToken) 
                                {
                                    tt = tt.Next.Next;
                                    res.EndChar = tt.EndChar;
                                    for (; tt != null && tt.Next != null; ) 
                                    {
                                        if (!tt.Next.IsChar('.') || tt.IsWhitespaceAfter || tt.Next.IsWhitespaceAfter) 
                                            break;
                                        if (!(tt.Next.Next is Pullenti.Ner.NumberToken)) 
                                            break;
                                        tt = tt.Next.Next;
                                        res.EndChar = tt.EndChar;
                                    }
                                }
                                else if (tt.Next.Next != null && (tt.Next.Next.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) && hasDiap) 
                                    res.EndChar = (tt.Next.Next as Pullenti.Ner.MetaToken).BeginToken.EndChar;
                            }
                            return res;
                        }
                        if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false) && tt.BeginChar == res.BeginChar && hasSameBefore) 
                        {
                            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br != null && br.EndToken.Previous == tt.Next) 
                            {
                                res.EndChar = br.EndChar;
                                return res;
                            }
                        }
                    }
                }
                return res;
            }
            if (!hasSameBefore && !hasSameAfter && ptmin != PartToken.ItemType.Prefix) 
            {
                for (Pullenti.Ner.Token tt = (t as Pullenti.Ner.MetaToken).BeginToken; tt != null && tt.EndChar <= res.EndChar; tt = tt.Next) 
                {
                    if (tt.BeginChar >= res.BeginChar) 
                    {
                        List<PartToken> pts = PartToken.TryAttachList(tt, false, 40);
                        if (pts == null || pts.Count == 0) 
                            break;
                        for (int i = 0; i < pts.Count; i++) 
                        {
                            if (pts[i].Typ == ptmin) 
                            {
                                res.BeginChar = pts[i].BeginChar;
                                res.EndChar = pts[i].EndChar;
                                tt = pts[i].EndToken;
                                if (tt.Next != null && tt.Next.IsHiphen) 
                                {
                                    if (tt.Next.Next is Pullenti.Ner.NumberToken) 
                                        res.EndChar = tt.Next.Next.EndChar;
                                    else if (tt.Next.Next != null && (tt.Next.Next.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) && hasDiap) 
                                        res.EndChar = (tt.Next.Next as Pullenti.Ner.MetaToken).BeginToken.EndChar;
                                }
                                return res;
                            }
                        }
                    }
                }
            }
            return res;
        }
        static bool _hasSameDecree(Pullenti.Ner.Token t, Pullenti.Ner.Decree.DecreePartReferent dpr, bool before)
        {
            if (((before ? t.Previous : t.Next)) == null) 
                return false;
            t = (before ? t.Previous : t.Next);
            if (t.IsCommaAnd || t.Morph.Class.IsConjunction) 
            {
            }
            else 
                return false;
            t = (before ? t.Previous : t.Next);
            if (t == null) 
                return false;
            Pullenti.Ner.Decree.DecreePartReferent dpr0 = t.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
            if (dpr0 == null) 
                return false;
            if (dpr0.Owner != dpr.Owner) 
                return false;
            if (dpr0.Owner == null) 
            {
                if (dpr0.LocalTyp != dpr.LocalTyp) 
                    return false;
            }
            foreach (Pullenti.Ner.Slot s in dpr0.Slots) 
            {
                if (PartToken._getTypeByAttrName(s.TypeName) != PartToken.ItemType.Prefix) 
                {
                    if (dpr.FindSlot(s.TypeName, null, true) == null) 
                        return false;
                }
            }
            foreach (Pullenti.Ner.Slot s in dpr.Slots) 
            {
                if (PartToken._getTypeByAttrName(s.TypeName) != PartToken.ItemType.Prefix) 
                {
                    if (dpr0.FindSlot(s.TypeName, null, true) == null) 
                        return false;
                }
            }
            return true;
        }
        static string _outMoney(Pullenti.Ner.Money.MoneyReferent m)
        {
            string res = m.ToString();
            res = res.Replace('.', ' ').Replace("RUR", "руб.").Replace("RUB", "руб.");
            return res;
        }
        /// <summary>
        /// Проверка корректности НДС для суммы
        /// </summary>
        /// <param name="t">Указывает на значение, для которой должно далее следовать НДС</param>
        public static Pullenti.Ner.MetaToken CheckNds(Pullenti.Ner.Token t, double nds = (double)18F, bool ndsMustbeMoney = false)
        {
            if (t == null || nds <= 0) 
                return null;
            Pullenti.Ner.Money.MoneyReferent m = t.GetReferent() as Pullenti.Ner.Money.MoneyReferent;
            if (m == null) 
                return null;
            bool hasNds = false;
            bool hasNdsPerc = false;
            bool hasAll = false;
            bool incl = false;
            Pullenti.Ner.Token tt;
            Pullenti.Ner.Money.MoneyReferent m1 = null;
            Pullenti.Ner.Token ndsT0 = null;
            Pullenti.Ner.Token ndsT1 = null;
            for (tt = t.Next; tt != null; tt = tt.Next) 
            {
                if (tt.IsValue("НДС", null)) 
                {
                    hasNds = true;
                    ndsT0 = (ndsT1 = tt);
                    continue;
                }
                if (tt is Pullenti.Ner.ReferentToken) 
                {
                    m1 = tt.GetReferent() as Pullenti.Ner.Money.MoneyReferent;
                    break;
                }
                if (tt is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.Core.NumberExToken ne = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(tt);
                    if (ne != null && ne.ExTyp == Pullenti.Ner.Core.NumberExType.Percent) 
                    {
                        if (Math.Abs(ne.RealValue - nds) > 0.0001) 
                        {
                            bool ok = false;
                            if (hasNds) 
                                ok = true;
                            if (ok) 
                                return new Pullenti.Ner.MetaToken(tt, ne.EndToken) { Tag = string.Format("Размер НДС должен быть {0}%, а не {1}%", nds, ne.RealValue) };
                        }
                        tt = (ndsT1 = ne.EndToken);
                        hasNdsPerc = true;
                        continue;
                    }
                }
                if (tt.IsValue("ВСЕГО", null)) 
                {
                    hasAll = true;
                    continue;
                }
                if (tt.IsValue("ТОМ", null) || tt.IsValue("ЧИСЛО", null) || tt.IsValue("ВКЛЮЧАЯ", null)) 
                {
                    incl = true;
                    continue;
                }
                if ((tt.IsValue("КРОМЕ", null) || tt.IsValue("ТОГО", null) || tt.IsValue("РАЗМЕР", null)) || tt.IsValue("СУММА", null) || tt.IsValue("СТАВКА", null)) 
                    continue;
                if (((tt.IsValue("Т", null) && tt.Next != null && tt.Next.IsChar('.')) && tt.Next.Next != null && tt.Next.Next.IsValue("Ч", null)) && tt.Next.Next.Next != null && tt.Next.Next.Next.IsChar('.')) 
                {
                    incl = true;
                    tt = tt.Next.Next.Next;
                    continue;
                }
                if (!tt.Chars.IsLetter || tt.Morph.Class.IsPreposition) 
                    continue;
                break;
            }
            if (!hasNds) 
                return null;
            if (m1 == null) 
            {
                if (ndsMustbeMoney) 
                    return new Pullenti.Ner.MetaToken(ndsT0, ndsT1) { Tag = "Размер НДС должен быть в денежном выражении" };
                return null;
            }
            if (hasAll) 
                return null;
            double mustBe = m.RealValue;
            mustBe = mustBe * ((nds / 100));
            if (incl) 
                mustBe /= ((1 + ((nds / 100))));
            double dd = mustBe * 100;
            dd -= ((long)dd);
            dd /= 100;
            mustBe -= dd;
            if (dd >= 0.005) 
                mustBe += 0.01;
            double real = m1.RealValue;
            double delta = mustBe - real;
            if (delta < 0) 
                delta = -delta;
            if (delta > 0.011) 
            {
                if ((delta < 1) && m1.Rest == 0 && m.Rest == 0) 
                {
                }
                else 
                {
                    Pullenti.Ner.Money.MoneyReferent mr = new Pullenti.Ner.Money.MoneyReferent() { Currency = m1.Currency, RealValue = mustBe };
                    return new Pullenti.Ner.MetaToken(t, tt) { Tag = string.Format("Размер НДС должен быть {0}, а не {1}", _outMoney(mr), _outMoney(m1)) };
                }
            }
            if (incl) 
                return null;
            Pullenti.Ner.Money.MoneyReferent m2 = null;
            hasAll = false;
            for (tt = tt.Next; tt != null; tt = tt.Next) 
            {
                if (tt is Pullenti.Ner.ReferentToken) 
                {
                    m2 = tt.GetReferent() as Pullenti.Ner.Money.MoneyReferent;
                    break;
                }
                if (!tt.Chars.IsLetter || tt.Morph.Class.IsPreposition) 
                    continue;
                if (tt.IsValue("ВСЕГО", null)) 
                {
                    hasAll = true;
                    continue;
                }
                if (tt.IsValue("НДС", null) || tt.IsValue("ВМЕСТЕ", null)) 
                    continue;
                break;
            }
            if (m2 != null && hasAll) 
            {
                mustBe = m.RealValue + m1.RealValue;
                delta = mustBe - m2.RealValue;
                if (delta < 0) 
                    delta = -delta;
                if (delta > 0.01) 
                {
                    Pullenti.Ner.Money.MoneyReferent mr = new Pullenti.Ner.Money.MoneyReferent() { Currency = m1.Currency, RealValue = mustBe };
                    string err = string.Format("Всего с НДС должно быть {0}, а не {1}", _outMoney(mr), _outMoney(m2));
                    return new Pullenti.Ner.MetaToken(t, tt) { Tag = err };
                }
            }
            return null;
        }
    }
}