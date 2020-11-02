/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core.Internal
{
    static class NumberExHelper
    {
        /// <summary>
        /// Выделение стандартных мер, типа: 10 кв.м.
        /// </summary>
        public static Pullenti.Ner.Core.NumberExToken TryParseNumberWithPostfix(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            string isDollar = null;
            if (t.LengthChar == 1 && t.Next != null) 
            {
                if ((((isDollar = Pullenti.Ner.Core.NumberHelper.IsMoneyChar(t)))) != null) 
                    t = t.Next;
            }
            Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
            if (nt == null) 
            {
                if ((!(t.Previous is Pullenti.Ner.NumberToken) && t.IsChar('(') && (t.Next is Pullenti.Ner.NumberToken)) && t.Next.Next != null && t.Next.Next.IsChar(')')) 
                {
                    Pullenti.Ner.Core.TerminToken toks1 = m_Postfixes.TryParse(t.Next.Next.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (toks1 != null && ((Pullenti.Ner.Core.NumberExType)toks1.Termin.Tag) == Pullenti.Ner.Core.NumberExType.Money) 
                    {
                        Pullenti.Ner.NumberToken nt0 = t.Next as Pullenti.Ner.NumberToken;
                        Pullenti.Ner.Core.NumberExToken res = new Pullenti.Ner.Core.NumberExToken(t, toks1.EndToken, nt0.Value, nt0.Typ, Pullenti.Ner.Core.NumberExType.Money) { AltRealValue = nt0.RealValue, Morph = toks1.BeginToken.Morph };
                        return _correctMoney(res, toks1.BeginToken);
                    }
                }
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null || !tt.Morph.Class.IsAdjective) 
                    return null;
                string val = tt.Term;
                for (int i = 4; i < (val.Length - 5); i++) 
                {
                    string v = val.Substring(0, i);
                    List<Pullenti.Ner.Core.Termin> li = Pullenti.Ner.Core.NumberHelper.m_Nums.FindTerminsByString(v, tt.Morph.Language);
                    if (li == null) 
                        continue;
                    string vv = val.Substring(i);
                    List<Pullenti.Ner.Core.Termin> lii = m_Postfixes.FindTerminsByString(vv, tt.Morph.Language);
                    if (lii != null && lii.Count > 0) 
                    {
                        Pullenti.Ner.Core.NumberExToken re = new Pullenti.Ner.Core.NumberExToken(t, t, ((int)li[0].Tag).ToString(), Pullenti.Ner.NumberSpellingType.Words, (Pullenti.Ner.Core.NumberExType)lii[0].Tag) { Morph = t.Morph };
                        _correctExtTypes(re);
                        return re;
                    }
                    break;
                }
                return null;
            }
            if (t.Next == null && isDollar == null) 
                return null;
            double f = nt.RealValue;
            if (double.IsNaN(f)) 
                return null;
            Pullenti.Ner.Token t1 = nt.Next;
            if (((t1 != null && t1.IsCharOf(",."))) || (((t1 is Pullenti.Ner.NumberToken) && (t1.WhitespacesBeforeCount < 3)))) 
            {
                double d;
                Pullenti.Ner.NumberToken tt11 = Pullenti.Ner.Core.NumberHelper.TryParseRealNumber(nt, false, false);
                if (tt11 != null) 
                {
                    t1 = tt11.EndToken.Next;
                    f = tt11.RealValue;
                }
            }
            if (t1 == null) 
            {
                if (isDollar == null) 
                    return null;
            }
            else if ((t1.Next != null && t1.Next.IsValue("С", "З") && t1.Next.Next != null) && t1.Next.Next.IsValue("ПОЛОВИНА", null)) 
            {
                f += 0.5;
                t1 = t1.Next.Next;
            }
            if (t1 != null && t1.IsHiphen && t1.Next != null) 
                t1 = t1.Next;
            bool det = false;
            double altf = f;
            if (((t1 is Pullenti.Ner.NumberToken) && t1.Previous != null && t1.Previous.IsHiphen) && (t1 as Pullenti.Ner.NumberToken).IntValue == 0 && t1.LengthChar == 2) 
                t1 = t1.Next;
            if ((t1 != null && t1.Next != null && t1.IsChar('(')) && (((t1.Next is Pullenti.Ner.NumberToken) || t1.Next.IsValue("НОЛЬ", null))) && t1.Next.Next != null) 
            {
                Pullenti.Ner.NumberToken nt1 = t1.Next as Pullenti.Ner.NumberToken;
                double val = (double)0;
                if (nt1 != null) 
                    val = nt1.RealValue;
                if (Math.Floor(f) == Math.Floor(val)) 
                {
                    Pullenti.Ner.Token ttt = t1.Next.Next;
                    if (ttt.IsChar(')')) 
                    {
                        t1 = ttt.Next;
                        det = true;
                        if ((t1 is Pullenti.Ner.NumberToken) && (t1 as Pullenti.Ner.NumberToken).IntValue != null && (t1 as Pullenti.Ner.NumberToken).IntValue.Value == 0) 
                            t1 = t1.Next;
                    }
                    else if (((((ttt is Pullenti.Ner.NumberToken) && ((ttt as Pullenti.Ner.NumberToken).RealValue < 100) && ttt.Next != null) && ttt.Next.IsChar('/') && ttt.Next.Next != null) && ttt.Next.Next.GetSourceText() == "100" && ttt.Next.Next.Next != null) && ttt.Next.Next.Next.IsChar(')')) 
                    {
                        int rest = GetDecimalRest100(f);
                        if ((ttt as Pullenti.Ner.NumberToken).IntValue != null && rest == (ttt as Pullenti.Ner.NumberToken).IntValue.Value) 
                        {
                            t1 = ttt.Next.Next.Next.Next;
                            det = true;
                        }
                    }
                    else if ((ttt.IsValue("ЦЕЛЫХ", null) && (ttt.Next is Pullenti.Ner.NumberToken) && ttt.Next.Next != null) && ttt.Next.Next.Next != null && ttt.Next.Next.Next.IsChar(')')) 
                    {
                        Pullenti.Ner.NumberToken num2 = ttt.Next as Pullenti.Ner.NumberToken;
                        altf = num2.RealValue;
                        if (ttt.Next.Next.IsValue("ДЕСЯТЫЙ", null)) 
                            altf /= 10;
                        else if (ttt.Next.Next.IsValue("СОТЫЙ", null)) 
                            altf /= 100;
                        else if (ttt.Next.Next.IsValue("ТЫСЯЧНЫЙ", null)) 
                            altf /= 1000;
                        else if (ttt.Next.Next.IsValue("ДЕСЯТИТЫСЯЧНЫЙ", null)) 
                            altf /= 10000;
                        else if (ttt.Next.Next.IsValue("СТОТЫСЯЧНЫЙ", null)) 
                            altf /= 100000;
                        else if (ttt.Next.Next.IsValue("МИЛЛИОННЫЙ", null)) 
                            altf /= 1000000;
                        if (altf < 1) 
                        {
                            altf += val;
                            t1 = ttt.Next.Next.Next.Next;
                            det = true;
                        }
                    }
                    else 
                    {
                        Pullenti.Ner.Core.TerminToken toks1 = m_Postfixes.TryParse(ttt, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (toks1 != null) 
                        {
                            if (((Pullenti.Ner.Core.NumberExType)toks1.Termin.Tag) == Pullenti.Ner.Core.NumberExType.Money) 
                            {
                                if (toks1.EndToken.Next != null && toks1.EndToken.Next.IsChar(')')) 
                                {
                                    Pullenti.Ner.Core.NumberExToken res = new Pullenti.Ner.Core.NumberExToken(t, toks1.EndToken.Next, nt.Value, nt.Typ, Pullenti.Ner.Core.NumberExType.Money) { RealValue = f, AltRealValue = altf, Morph = toks1.BeginToken.Morph };
                                    return _correctMoney(res, toks1.BeginToken);
                                }
                            }
                        }
                        Pullenti.Ner.Core.NumberExToken res2 = TryParseNumberWithPostfix(t1.Next);
                        if (res2 != null && res2.EndToken.Next != null && res2.EndToken.Next.IsChar(')')) 
                        {
                            res2.BeginToken = t;
                            res2.EndToken = res2.EndToken.Next;
                            res2.AltRealValue = res2.RealValue;
                            res2.RealValue = f;
                            _correctExtTypes(res2);
                            if (res2.WhitespacesAfterCount < 2) 
                            {
                                Pullenti.Ner.Core.TerminToken toks2 = m_Postfixes.TryParse(res2.EndToken.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                                if (toks2 != null) 
                                {
                                    if (((Pullenti.Ner.Core.NumberExType)toks2.Termin.Tag) == Pullenti.Ner.Core.NumberExType.Money) 
                                        res2.EndToken = toks2.EndToken;
                                }
                            }
                            return res2;
                        }
                    }
                }
                else if (nt1 != null && nt1.Typ == Pullenti.Ner.NumberSpellingType.Words && nt.Typ == Pullenti.Ner.NumberSpellingType.Digit) 
                {
                    altf = nt1.RealValue;
                    Pullenti.Ner.Token ttt = t1.Next.Next;
                    if (ttt.IsChar(')')) 
                    {
                        t1 = ttt.Next;
                        det = true;
                    }
                    if (!det) 
                        altf = f;
                }
            }
            if ((t1 != null && t1.IsChar('(') && t1.Next != null) && t1.Next.IsValue("СУММА", null)) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                    t1 = br.EndToken.Next;
            }
            if (isDollar != null) 
            {
                Pullenti.Ner.Token te = null;
                if (t1 != null) 
                    te = t1.Previous;
                else 
                    for (t1 = t0; t1 != null; t1 = t1.Next) 
                    {
                        if (t1.Next == null) 
                            te = t1;
                    }
                if (te == null) 
                    return null;
                if (te.IsHiphen && te.Next != null) 
                {
                    if (te.Next.IsValue("МИЛЛИОННЫЙ", null)) 
                    {
                        f *= 1000000;
                        altf *= 1000000;
                        te = te.Next;
                    }
                    else if (te.Next.IsValue("МИЛЛИАРДНЫЙ", null)) 
                    {
                        f *= 1000000000;
                        altf *= 1000000000;
                        te = te.Next;
                    }
                }
                if (!te.IsWhitespaceAfter && (te.Next is Pullenti.Ner.TextToken)) 
                {
                    if (te.Next.IsValue("M", null)) 
                    {
                        f *= 1000000;
                        altf *= 1000000;
                        te = te.Next;
                    }
                    else if (te.Next.IsValue("BN", null)) 
                    {
                        f *= 1000000000;
                        altf *= 1000000000;
                        te = te.Next;
                    }
                }
                return new Pullenti.Ner.Core.NumberExToken(t0, te, "", nt.Typ, Pullenti.Ner.Core.NumberExType.Money) { RealValue = f, AltRealValue = altf, ExTypParam = isDollar };
            }
            if (t1 == null || ((t1.IsNewlineBefore && !det))) 
                return null;
            Pullenti.Ner.Core.TerminToken toks = m_Postfixes.TryParse(t1, Pullenti.Ner.Core.TerminParseAttr.No);
            if ((toks == null && det && (t1 is Pullenti.Ner.NumberToken)) && (t1 as Pullenti.Ner.NumberToken).Value == "0") 
                toks = m_Postfixes.TryParse(t1.Next, Pullenti.Ner.Core.TerminParseAttr.No);
            if (toks == null && t1.IsChar('р')) 
            {
                int cou = 10;
                for (Pullenti.Ner.Token ttt = t0.Previous; ttt != null && cou > 0; ttt = ttt.Previous,cou--) 
                {
                    if (ttt.IsValue("СУММА", null) || ttt.IsValue("НАЛИЧНЫЙ", null) || ttt.IsValue("БАЛАНС", null)) 
                    {
                    }
                    else if (ttt.GetReferent() != null && ttt.GetReferent().TypeName == "MONEY") 
                    {
                    }
                    else 
                        continue;
                    toks = new Pullenti.Ner.Core.TerminToken(t1, t1) { Termin = m_Postfixes.FindTerminsByCanonicText("RUB")[0] };
                    if (t1.Next != null && t1.Next.IsChar('.')) 
                        toks.EndToken = t1.Next;
                    Pullenti.Ner.Core.NumberExType ty = (Pullenti.Ner.Core.NumberExType)toks.Termin.Tag;
                    return new Pullenti.Ner.Core.NumberExToken(t, toks.EndToken, nt.Value, nt.Typ, ty) { RealValue = f, AltRealValue = altf, Morph = toks.BeginToken.Morph, ExTypParam = "RUB" };
                }
            }
            if (toks != null) 
            {
                t1 = toks.EndToken;
                if (!t1.IsChar('.') && t1.Next != null && t1.Next.IsChar('.')) 
                {
                    if ((t1 is Pullenti.Ner.TextToken) && t1.IsValue(toks.Termin.Terms[0].CanonicalText, null)) 
                    {
                    }
                    else if (!t1.Chars.IsLetter) 
                    {
                    }
                    else 
                        t1 = t1.Next;
                }
                if (toks.Termin.CanonicText == "LTL") 
                    return null;
                if (toks.BeginToken == t1) 
                {
                    if (t1.Morph.Class.IsPreposition || t1.Morph.Class.IsConjunction) 
                    {
                        if (t1.IsWhitespaceBefore && t1.IsWhitespaceAfter) 
                            return null;
                    }
                }
                Pullenti.Ner.Core.NumberExType ty = (Pullenti.Ner.Core.NumberExType)toks.Termin.Tag;
                Pullenti.Ner.Core.NumberExToken res = new Pullenti.Ner.Core.NumberExToken(t, t1, nt.Value, nt.Typ, ty) { RealValue = f, AltRealValue = altf, Morph = toks.BeginToken.Morph };
                if (ty != Pullenti.Ner.Core.NumberExType.Money) 
                {
                    _correctExtTypes(res);
                    return res;
                }
                return _correctMoney(res, toks.BeginToken);
            }
            Pullenti.Ner.Core.NumberExToken pfx = _attachSpecPostfix(t1);
            if (pfx != null) 
            {
                pfx.BeginToken = t;
                pfx.Value = nt.Value;
                pfx.Typ = nt.Typ;
                pfx.RealValue = f;
                pfx.AltRealValue = altf;
                return pfx;
            }
            if (t1.Next != null && ((t1.Morph.Class.IsPreposition || t1.Morph.Class.IsConjunction))) 
            {
                if (t1.IsValue("НА", null)) 
                {
                }
                else 
                {
                    Pullenti.Ner.Core.NumberExToken nn = TryParseNumberWithPostfix(t1.Next);
                    if (nn != null) 
                        return new Pullenti.Ner.Core.NumberExToken(t, t, nt.Value, nt.Typ, nn.ExTyp) { RealValue = f, AltRealValue = altf, ExTyp2 = nn.ExTyp2, ExTypParam = nn.ExTypParam };
                }
            }
            if (!t1.IsWhitespaceAfter && (t1.Next is Pullenti.Ner.NumberToken) && (t1 is Pullenti.Ner.TextToken)) 
            {
                string term = (t1 as Pullenti.Ner.TextToken).Term;
                Pullenti.Ner.Core.NumberExType ty = Pullenti.Ner.Core.NumberExType.Undefined;
                if (term == "СМХ" || term == "CMX") 
                    ty = Pullenti.Ner.Core.NumberExType.Santimeter;
                else if (term == "MX" || term == "МХ") 
                    ty = Pullenti.Ner.Core.NumberExType.Meter;
                else if (term == "MMX" || term == "ММХ") 
                    ty = Pullenti.Ner.Core.NumberExType.Millimeter;
                if (ty != Pullenti.Ner.Core.NumberExType.Undefined) 
                    return new Pullenti.Ner.Core.NumberExToken(t, t1, nt.Value, nt.Typ, ty) { RealValue = f, AltRealValue = altf, MultAfter = true };
            }
            return null;
        }
        static int GetDecimalRest100(double f)
        {
            int rest = ((int)(((((f - Math.Truncate(f)) + 0.0001)) * 10000))) / 100;
            return rest;
        }
        /// <summary>
        /// Это попробовать только тип (постфикс) без самого числа
        /// </summary>
        public static Pullenti.Ner.Core.NumberExToken TryAttachPostfixOnly(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Core.TerminToken tok = m_Postfixes.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            Pullenti.Ner.Core.NumberExToken res = null;
            if (tok != null) 
                res = new Pullenti.Ner.Core.NumberExToken(t, tok.EndToken, "", Pullenti.Ner.NumberSpellingType.Digit, (Pullenti.Ner.Core.NumberExType)tok.Termin.Tag) { Tag = tok.Termin };
            else 
                res = _attachSpecPostfix(t);
            if (res != null) 
                _correctExtTypes(res);
            return res;
        }
        static Pullenti.Ner.Core.NumberExToken _attachSpecPostfix(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if (t.IsCharOf("%")) 
                return new Pullenti.Ner.Core.NumberExToken(t, t, "", Pullenti.Ner.NumberSpellingType.Digit, Pullenti.Ner.Core.NumberExType.Percent);
            string money = Pullenti.Ner.Core.NumberHelper.IsMoneyChar(t);
            if (money != null) 
                return new Pullenti.Ner.Core.NumberExToken(t, t, "", Pullenti.Ner.NumberSpellingType.Digit, Pullenti.Ner.Core.NumberExType.Money) { ExTypParam = money };
            return null;
        }
        static void _correctExtTypes(Pullenti.Ner.Core.NumberExToken ex)
        {
            Pullenti.Ner.Token t = ex.EndToken.Next;
            if (t == null) 
                return;
            Pullenti.Ner.Core.NumberExType ty = ex.ExTyp;
            Pullenti.Ner.Token tt = _corrExTyp2(t, ref ty);
            if (tt != null) 
            {
                ex.ExTyp = ty;
                ex.EndToken = tt;
                t = tt.Next;
            }
            if (t == null || t.Next == null) 
                return;
            if (t.IsCharOf("/\\") || t.IsValue("НА", null)) 
            {
            }
            else 
                return;
            Pullenti.Ner.Core.TerminToken tok = m_Postfixes.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null && (((Pullenti.Ner.Core.NumberExType)tok.Termin.Tag) != Pullenti.Ner.Core.NumberExType.Money)) 
            {
                ex.ExTyp2 = (Pullenti.Ner.Core.NumberExType)tok.Termin.Tag;
                ex.EndToken = tok.EndToken;
                ty = ex.ExTyp2;
                tt = _corrExTyp2(ex.EndToken.Next, ref ty);
                if (tt != null) 
                {
                    ex.ExTyp2 = ty;
                    ex.EndToken = tt;
                    t = tt.Next;
                }
            }
        }
        static Pullenti.Ner.Token _corrExTyp2(Pullenti.Ner.Token t, ref Pullenti.Ner.Core.NumberExType typ)
        {
            if (t == null) 
                return null;
            int num = 0;
            Pullenti.Ner.Token tt = t;
            if (t.IsChar('³')) 
                num = 3;
            else if (t.IsChar('²')) 
                num = 2;
            else if (!t.IsWhitespaceBefore && (t is Pullenti.Ner.NumberToken) && (((t as Pullenti.Ner.NumberToken).Value == "3" || (t as Pullenti.Ner.NumberToken).Value == "2"))) 
                num = (t as Pullenti.Ner.NumberToken).IntValue.Value;
            else if ((t.IsChar('<') && (t.Next is Pullenti.Ner.NumberToken) && t.Next.Next != null) && t.Next.Next.IsChar('>') && (t.Next as Pullenti.Ner.NumberToken).IntValue != null) 
            {
                num = (t.Next as Pullenti.Ner.NumberToken).IntValue.Value;
                tt = t.Next.Next;
            }
            if (num == 3) 
            {
                if (typ == Pullenti.Ner.Core.NumberExType.Meter) 
                {
                    typ = Pullenti.Ner.Core.NumberExType.Meter3;
                    return tt;
                }
                if (typ == Pullenti.Ner.Core.NumberExType.Santimeter) 
                {
                    typ = Pullenti.Ner.Core.NumberExType.Santimeter3;
                    return tt;
                }
            }
            if (num == 2) 
            {
                if (typ == Pullenti.Ner.Core.NumberExType.Meter) 
                {
                    typ = Pullenti.Ner.Core.NumberExType.Meter2;
                    return tt;
                }
                if (typ == Pullenti.Ner.Core.NumberExType.Santimeter) 
                {
                    typ = Pullenti.Ner.Core.NumberExType.Santimeter2;
                    return tt;
                }
            }
            return null;
        }
        static Pullenti.Ner.Core.NumberExToken _correctMoney(Pullenti.Ner.Core.NumberExToken res, Pullenti.Ner.Token t1)
        {
            if (t1 == null) 
                return null;
            List<Pullenti.Ner.Core.TerminToken> toks = m_Postfixes.TryParseAll(t1, Pullenti.Ner.Core.TerminParseAttr.No);
            if (toks == null || toks.Count == 0) 
                return null;
            Pullenti.Ner.Token tt = toks[0].EndToken.Next;
            Pullenti.Ner.Referent r = (tt == null ? null : tt.GetReferent());
            string alpha2 = null;
            if (r != null && r.TypeName == "GEO") 
                alpha2 = r.GetStringValue("ALPHA2");
            if (alpha2 != null && toks.Count > 0) 
            {
                for (int i = toks.Count - 1; i >= 0; i--) 
                {
                    if (!toks[i].Termin.CanonicText.StartsWith(alpha2)) 
                        toks.RemoveAt(i);
                }
                if (toks.Count == 0) 
                    toks = m_Postfixes.TryParseAll(t1, Pullenti.Ner.Core.TerminParseAttr.No);
            }
            if (toks.Count > 1) 
            {
                alpha2 = null;
                string str = toks[0].Termin.Terms[0].CanonicalText;
                if (str == "РУБЛЬ" || str == "RUBLE") 
                    alpha2 = "RU";
                else if (str == "ДОЛЛАР" || str == "ДОЛАР" || str == "DOLLAR") 
                    alpha2 = "US";
                else if (str == "ФУНТ" || str == "POUND") 
                    alpha2 = "UK";
                if (alpha2 != null) 
                {
                    for (int i = toks.Count - 1; i >= 0; i--) 
                    {
                        if (!toks[i].Termin.CanonicText.StartsWith(alpha2) && toks[i].Termin.CanonicText != "GBP") 
                            toks.RemoveAt(i);
                    }
                }
                alpha2 = null;
            }
            if (toks.Count < 1) 
                return null;
            res.ExTypParam = toks[0].Termin.CanonicText;
            if (alpha2 != null && tt != null) 
                res.EndToken = tt;
            tt = res.EndToken.Next;
            if (tt != null && tt.IsCommaAnd) 
                tt = tt.Next;
            if ((tt is Pullenti.Ner.NumberToken) && tt.Next != null && (tt.WhitespacesAfterCount < 4)) 
            {
                Pullenti.Ner.Token tt1 = tt.Next;
                if ((tt1 != null && tt1.IsChar('(') && (tt1.Next is Pullenti.Ner.NumberToken)) && tt1.Next.Next != null && tt1.Next.Next.IsChar(')')) 
                {
                    if ((tt as Pullenti.Ner.NumberToken).Value == (tt1.Next as Pullenti.Ner.NumberToken).Value) 
                        tt1 = tt1.Next.Next.Next;
                }
                Pullenti.Ner.Core.TerminToken tok = m_SmallMoney.TryParse(tt1, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok == null && tt1 != null && tt1.IsChar(')')) 
                    tok = m_SmallMoney.TryParse(tt1.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null && (tt as Pullenti.Ner.NumberToken).IntValue != null) 
                {
                    int max = (int)tok.Termin.Tag;
                    int val = (tt as Pullenti.Ner.NumberToken).IntValue.Value;
                    if (val < max) 
                    {
                        double f = (double)val;
                        f /= max;
                        double f0 = res.RealValue - ((long)res.RealValue);
                        int re0 = (int)(((f0 * 100) + 0.0001));
                        if (re0 > 0 && val != re0) 
                            res.AltRestMoney = val;
                        else if (f0 == 0) 
                            res.RealValue += f;
                        f0 = res.AltRealValue - ((long)res.AltRealValue);
                        re0 = (int)(((f0 * 100) + 0.0001));
                        if (re0 > 0 && val != re0) 
                            res.AltRestMoney = val;
                        else if (f0 == 0) 
                            res.AltRealValue += f;
                        res.EndToken = tok.EndToken;
                    }
                }
            }
            else if ((tt is Pullenti.Ner.TextToken) && tt.IsValue("НОЛЬ", null)) 
            {
                Pullenti.Ner.Core.TerminToken tok = m_SmallMoney.TryParse(tt.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                    res.EndToken = tok.EndToken;
            }
            return res;
        }
        internal static void Initialize()
        {
            if (m_Postfixes != null) 
                return;
            Pullenti.Ner.Core.Termin t;
            m_Postfixes = new Pullenti.Ner.Core.TerminCollection();
            t = new Pullenti.Ner.Core.Termin("КВАДРАТНЫЙ МЕТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "кв.м.", Tag = Pullenti.Ner.Core.NumberExType.Meter2 };
            t.AddAbridge("КВ.МЕТР");
            t.AddAbridge("КВ.МЕТРА");
            t.AddAbridge("КВ.М.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("КВАДРАТНИЙ МЕТР", Pullenti.Morph.MorphLang.UA, true) { CanonicText = "КВ.М.", Tag = Pullenti.Ner.Core.NumberExType.Meter2 };
            t.AddAbridge("КВ.МЕТР");
            t.AddAbridge("КВ.МЕТРА");
            t.AddAbridge("КВ.М.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("КВАДРАТНЫЙ КИЛОМЕТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "кв.км.", Tag = Pullenti.Ner.Core.NumberExType.Kilometer2 };
            t.AddVariant("КВАДРАТНИЙ КІЛОМЕТР", true);
            t.AddAbridge("КВ.КМ.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГЕКТАР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "га", Tag = Pullenti.Ner.Core.NumberExType.Gektar };
            t.AddAbridge("ГА");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("АР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "ар", Tag = Pullenti.Ner.Core.NumberExType.Ar };
            t.AddVariant("СОТКА", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("КУБИЧЕСКИЙ МЕТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "куб.м.", Tag = Pullenti.Ner.Core.NumberExType.Meter3 };
            t.AddVariant("КУБІЧНИЙ МЕТР", true);
            t.AddAbridge("КУБ.МЕТР");
            t.AddAbridge("КУБ.М.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МЕТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "м.", Tag = Pullenti.Ner.Core.NumberExType.Meter };
            t.AddAbridge("М.");
            t.AddAbridge("M.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МЕТРОВЫЙ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "м.", Tag = Pullenti.Ner.Core.NumberExType.Meter };
            t.AddVariant("МЕТРОВИЙ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИЛЛИМЕТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "мм.", Tag = Pullenti.Ner.Core.NumberExType.Millimeter };
            t.AddAbridge("ММ");
            t.AddAbridge("MM");
            t.AddVariant("МІЛІМЕТР", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИЛЛИМЕТРОВЫЙ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "мм.", Tag = Pullenti.Ner.Core.NumberExType.Millimeter };
            t.AddVariant("МІЛІМЕТРОВИЙ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("САНТИМЕТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "см.", Tag = Pullenti.Ner.Core.NumberExType.Santimeter };
            t.AddAbridge("СМ");
            t.AddAbridge("CM");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("САНТИМЕТРОВЫЙ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "см.", Tag = Pullenti.Ner.Core.NumberExType.Santimeter };
            t.AddVariant("САНТИМЕТРОВИЙ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("КВАДРАТНЫЙ САНТИМЕТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "кв.см.", Tag = Pullenti.Ner.Core.NumberExType.Santimeter2 };
            t.AddVariant("КВАДРАТНИЙ САНТИМЕТР", true);
            t.AddAbridge("КВ.СМ.");
            t.AddAbridge("СМ.КВ.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("КУБИЧЕСКИЙ САНТИМЕТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "куб.см.", Tag = Pullenti.Ner.Core.NumberExType.Santimeter3 };
            t.AddVariant("КУБІЧНИЙ САНТИМЕТР", true);
            t.AddAbridge("КУБ.САНТИМЕТР");
            t.AddAbridge("КУБ.СМ.");
            t.AddAbridge("СМ.КУБ.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("КИЛОМЕТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "км.", Tag = Pullenti.Ner.Core.NumberExType.Kilometer };
            t.AddAbridge("КМ");
            t.AddAbridge("KM");
            t.AddVariant("КІЛОМЕТР", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("КИЛОМЕТРОВЫЙ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "км.", Tag = Pullenti.Ner.Core.NumberExType.Kilometer };
            t.AddVariant("КІЛОМЕТРОВИЙ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИЛЯ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "миль", Tag = Pullenti.Ner.Core.NumberExType.Kilometer };
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГРАММ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "гр.", Tag = Pullenti.Ner.Core.NumberExType.Gramm };
            t.AddAbridge("ГР");
            t.AddAbridge("Г");
            t.AddVariant("ГРАМ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГРАММОВЫЙ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "гр.", Tag = Pullenti.Ner.Core.NumberExType.Gramm };
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("КИЛОГРАММ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "кг.", Tag = Pullenti.Ner.Core.NumberExType.Kilogram };
            t.AddAbridge("КГ");
            t.AddVariant("КІЛОГРАМ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("КИЛОГРАММОВЫЙ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "кг.", Tag = Pullenti.Ner.Core.NumberExType.Kilogram };
            t.AddVariant("КІЛОГРАМОВИЙ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИЛЛИГРАММ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "мг.", Tag = Pullenti.Ner.Core.NumberExType.Milligram };
            t.AddAbridge("МГ");
            t.AddVariant("МІЛІГРАМ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИЛЛИГРАММОВЫЙ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "мг.", Tag = Pullenti.Ner.Core.NumberExType.Milligram };
            t.AddVariant("МИЛЛИГРАМОВЫЙ", true);
            t.AddVariant("МІЛІГРАМОВИЙ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТОННА", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "т.", Tag = Pullenti.Ner.Core.NumberExType.Tonna };
            t.AddAbridge("Т");
            t.AddAbridge("T");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТОННЫЙ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "т.", Tag = Pullenti.Ner.Core.NumberExType.Tonna };
            t.AddVariant("ТОННИЙ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛИТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "л.", Tag = Pullenti.Ner.Core.NumberExType.Litr };
            t.AddAbridge("Л");
            t.AddVariant("ЛІТР", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛИТРОВЫЙ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "л.", Tag = Pullenti.Ner.Core.NumberExType.Litr };
            t.AddVariant("ЛІТРОВИЙ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИЛЛИЛИТР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "мл.", Tag = Pullenti.Ner.Core.NumberExType.Millilitr };
            t.AddAbridge("МЛ");
            t.AddVariant("МІЛІЛІТР", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИЛЛИЛИТРОВЫЙ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "мл.", Tag = Pullenti.Ner.Core.NumberExType.Millilitr };
            t.AddVariant("МІЛІЛІТРОВИЙ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЧАС", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "ч.", Tag = Pullenti.Ner.Core.NumberExType.Hour };
            t.AddAbridge("Ч.");
            t.AddVariant("ГОДИНА", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИНУТА", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "мин.", Tag = Pullenti.Ner.Core.NumberExType.Minute };
            t.AddAbridge("МИН.");
            t.AddVariant("ХВИЛИНА", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕКУНДА", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "сек.", Tag = Pullenti.Ner.Core.NumberExType.Second };
            t.AddAbridge("СЕК.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОД", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "г.", Tag = Pullenti.Ner.Core.NumberExType.Year };
            t.AddAbridge("Г.");
            t.AddAbridge("ЛЕТ");
            t.AddVariant("ЛЕТНИЙ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("МЕСЯЦ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "мес.", Tag = Pullenti.Ner.Core.NumberExType.Month };
            t.AddAbridge("МЕС.");
            t.AddVariant("МЕСЯЧНЫЙ", true);
            t.AddVariant("КАЛЕНДАРНЫЙ МЕСЯЦ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДЕНЬ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "дн.", Tag = Pullenti.Ner.Core.NumberExType.Day };
            t.AddAbridge("ДН.");
            t.AddVariant("ДНЕВНЫЙ", true);
            t.AddVariant("СУТКИ", true);
            t.AddVariant("СУТОЧНЫЙ", true);
            t.AddVariant("КАЛЕНДАРНЫЙ ДЕНЬ", true);
            t.AddVariant("РАБОЧИЙ ДЕНЬ", true);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("НЕДЕЛЯ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "нед.", Tag = Pullenti.Ner.Core.NumberExType.Week };
            t.AddVariant("НЕДЕЛЬНЫЙ", true);
            t.AddVariant("КАЛЕНДАРНАЯ НЕДЕЛЯ", false);
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОЦЕНТ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "%", Tag = Pullenti.Ner.Core.NumberExType.Percent };
            t.AddVariant("%", false);
            t.AddVariant("ПРОЦ", true);
            t.AddAbridge("ПРОЦ.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ШТУКА", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "шт.", Tag = Pullenti.Ner.Core.NumberExType.Shuk };
            t.AddVariant("ШТ", false);
            t.AddAbridge("ШТ.");
            t.AddAbridge("ШТ-К");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("УПАКОВКА", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "уп.", Tag = Pullenti.Ner.Core.NumberExType.Upak };
            t.AddVariant("УПАК", true);
            t.AddVariant("УП", true);
            t.AddAbridge("УПАК.");
            t.AddAbridge("УП.");
            t.AddAbridge("УП-КА");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("РУЛОН", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "рулон", Tag = Pullenti.Ner.Core.NumberExType.Rulon };
            t.AddVariant("РУЛ", true);
            t.AddAbridge("РУЛ.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("НАБОР", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "набор", Tag = Pullenti.Ner.Core.NumberExType.Nabor };
            t.AddVariant("НАБ", true);
            t.AddAbridge("НАБ.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОМПЛЕКТ", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "компл.", Tag = Pullenti.Ner.Core.NumberExType.Komplekt };
            t.AddVariant("КОМПЛ", true);
            t.AddAbridge("КОМПЛ.");
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПАРА", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "пар", Tag = Pullenti.Ner.Core.NumberExType.Para };
            m_Postfixes.Add(t);
            t = new Pullenti.Ner.Core.Termin("ФЛАКОН", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "флак.", Tag = Pullenti.Ner.Core.NumberExType.Flakon };
            t.AddVariant("ФЛ", true);
            t.AddAbridge("ФЛ.");
            t.AddVariant("ФЛАК", true);
            t.AddAbridge("ФЛАК.");
            m_Postfixes.Add(t);
            foreach (Pullenti.Ner.Core.Termin te in m_Postfixes.Termins) 
            {
                Pullenti.Ner.Core.NumberExType ty = (Pullenti.Ner.Core.NumberExType)te.Tag;
                if (!m_NormalsTyps.ContainsKey(ty)) 
                    m_NormalsTyps.Add(ty, te.CanonicText);
            }
            m_SmallMoney = new Pullenti.Ner.Core.TerminCollection();
            t = new Pullenti.Ner.Core.Termin("УСЛОВНАЯ ЕДИНИЦА") { CanonicText = "УЕ", Tag = Pullenti.Ner.Core.NumberExType.Money };
            t.AddAbridge("У.Е.");
            t.AddAbridge("У.E.");
            t.AddAbridge("Y.Е.");
            t.AddAbridge("Y.E.");
            m_Postfixes.Add(t);
            for (int k = 0; k < 3; k++) 
            {
                string str = ResourceHelper.GetString((k == 0 ? "Money.csv" : (k == 1 ? "MoneyUA.csv" : "MoneyEN.csv")));
                if (str == null) 
                    continue;
                Pullenti.Morph.MorphLang lang = (k == 0 ? Pullenti.Morph.MorphLang.RU : (k == 1 ? Pullenti.Morph.MorphLang.UA : Pullenti.Morph.MorphLang.EN));
                if (str == null) 
                    continue;
                foreach (string line0 in str.Split('\n')) 
                {
                    string line = line0.Trim();
                    if (string.IsNullOrEmpty(line)) 
                        continue;
                    string[] parts = line.ToUpper().Split(';');
                    if (parts == null || parts.Length != 5) 
                        continue;
                    if (string.IsNullOrEmpty(parts[1]) || string.IsNullOrEmpty(parts[2])) 
                        continue;
                    t = new Pullenti.Ner.Core.Termin();
                    t.InitByNormalText(parts[1], lang);
                    t.CanonicText = parts[2];
                    t.Acronym = parts[2];
                    t.Tag = Pullenti.Ner.Core.NumberExType.Money;
                    foreach (string p in parts[0].Split(',')) 
                    {
                        if (p != parts[1]) 
                        {
                            Pullenti.Ner.Core.Termin t0 = new Pullenti.Ner.Core.Termin();
                            t0.InitByNormalText(p, null);
                            t.AddVariantTerm(t0);
                        }
                    }
                    if (parts[1] == "РУБЛЬ") 
                        t.AddAbridge("РУБ.");
                    else if (parts[1] == "ГРИВНЯ" || parts[1] == "ГРИВНА") 
                        t.AddAbridge("ГРН.");
                    else if (parts[1] == "ДОЛЛАР") 
                    {
                        t.AddAbridge("ДОЛ.");
                        t.AddAbridge("ДОЛЛ.");
                    }
                    else if (parts[1] == "ДОЛАР") 
                        t.AddAbridge("ДОЛ.");
                    else if (parts[1] == "ИЕНА") 
                        t.AddVariant("ЙЕНА", false);
                    m_Postfixes.Add(t);
                    if (string.IsNullOrEmpty(parts[3])) 
                        continue;
                    int num = 0;
                    int i = parts[3].IndexOf(' ');
                    if (i < 2) 
                        continue;
                    if (!int.TryParse(parts[3].Substring(0, i), out num)) 
                        continue;
                    string vv = parts[3].Substring(i).Trim();
                    t = new Pullenti.Ner.Core.Termin();
                    t.InitByNormalText(parts[4], lang);
                    t.Tag = num;
                    if (vv != parts[4]) 
                    {
                        Pullenti.Ner.Core.Termin t0 = new Pullenti.Ner.Core.Termin();
                        t0.InitByNormalText(vv, null);
                        t.AddVariantTerm(t0);
                    }
                    if (parts[4] == "КОПЕЙКА" || parts[4] == "КОПІЙКА") 
                        t.AddAbridge("КОП.");
                    m_SmallMoney.Add(t);
                }
            }
        }
        internal static Pullenti.Ner.Core.TerminCollection m_Postfixes;
        internal static Dictionary<Pullenti.Ner.Core.NumberExType, string> m_NormalsTyps = new Dictionary<Pullenti.Ner.Core.NumberExType, string>();
        static Pullenti.Ner.Core.TerminCollection m_SmallMoney;
    }
}