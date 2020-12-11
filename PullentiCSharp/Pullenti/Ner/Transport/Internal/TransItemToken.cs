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

namespace Pullenti.Ner.Transport.Internal
{
    public class TransItemToken : Pullenti.Ner.MetaToken
    {
        public TransItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public enum Typs : int
        {
            Noun,
            Brand,
            Model,
            Number,
            Name,
            Org,
            Route,
            Class,
            Date,
            Geo,
        }

        public Typs Typ;
        public string Value;
        public string AltValue;
        public Pullenti.Ner.Transport.TransportKind Kind;
        public bool IsDoubt;
        public bool IsAfterConjunction;
        public Pullenti.Ner.ReferentToken State;
        public Pullenti.Ner.Referent Ref;
        public List<object> RouteItems;
        public override string ToString()
        {
            return string.Format("{0}: {1} {2}", Typ.ToString(), Value ?? ((Ref == null ? "" : Ref.ToString())), AltValue ?? "");
        }
        public static List<TransItemToken> TryParseList(Pullenti.Ner.Token t, int maxCount = 10)
        {
            TransItemToken tr = TryParse(t, null, false, false);
            if (tr == null) 
                return null;
            if ((tr.Typ == Typs.Org || tr.Typ == Typs.Number || tr.Typ == Typs.Class) || tr.Typ == Typs.Date) 
                return null;
            TransItemToken tr0 = tr;
            List<TransItemToken> res = new List<TransItemToken>();
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
            bool brareg = false;
            for (; t != null; t = t.Next) 
            {
                if (maxCount > 0 && res.Count >= maxCount) 
                    break;
                if (tr0.Typ == Typs.Noun || tr0.Typ == Typs.Org) 
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
                        brareg = true;
                        tr = TryParse(t.Next, tr0, false, false);
                        if (tr != null) 
                        {
                            if (tr.Typ != Typs.Number && tr.Typ != Typs.Geo) 
                                tr = null;
                            else if (tr.EndToken.Next != null) 
                            {
                                tr.BeginToken = t;
                                if (tr.EndToken.Next.IsChar(')')) 
                                {
                                    tr.EndToken = tr.EndToken.Next;
                                    brareg = false;
                                }
                            }
                        }
                        if (tr == null) 
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
                }
                if (tr == null && t.IsHiphen) 
                {
                    if (tr0.Typ == Typs.Brand || tr0.Typ == Typs.Model) 
                        tr = TryParse(t.Next, tr0, false, false);
                }
                if (tr == null && t.IsComma) 
                {
                    if (((tr0.Typ == Typs.Name || tr0.Typ == Typs.Brand || tr0.Typ == Typs.Model) || tr0.Typ == Typs.Class || tr0.Typ == Typs.Date) || tr0.Typ == Typs.Geo) 
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
                {
                    if (tr0.Typ == Typs.Name) 
                    {
                        if (t.IsChar(',')) 
                            tr = TryParse(t.Next, tr0, true, false);
                        else if (t.Morph.Class.IsConjunction && t.IsAnd) 
                        {
                            tr = TryParse(t.Next, tr0, true, false);
                            andConj = true;
                        }
                    }
                    if (tr != null) 
                    {
                        if (tr.Typ != Typs.Name) 
                            break;
                        tr.IsAfterConjunction = true;
                    }
                }
                if (t.IsCommaAnd && tr == null) 
                {
                    TransItemToken ne = TryParse(t.Next, tr0, true, false);
                    if (ne != null && ne.Typ == Typs.Number) 
                    {
                        bool exi = false;
                        foreach (TransItemToken v in res) 
                        {
                            if (v.Typ == ne.Typ) 
                            {
                                exi = true;
                                break;
                            }
                        }
                        if (!exi) 
                            tr = ne;
                    }
                }
                if (tr == null && brareg && t.IsChar(')')) 
                {
                    brareg = false;
                    tr0.EndToken = t;
                    continue;
                }
                if (tr == null && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t, true, null, false)) 
                {
                    tr0.EndToken = t;
                    continue;
                }
                if (tr == null) 
                    break;
                if (t.IsNewlineBefore) 
                {
                    if (tr.Typ != Typs.Number) 
                        break;
                }
                res.Add(tr);
                if (tr.Typ == Typs.Org && tr0.Typ == Typs.Noun) 
                {
                }
                else 
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
            if ((res.Count > 1 && res[0].Typ == Typs.Brand && res[1].Typ == Typs.Model) && res[1].LengthChar == 1 && !(res[1].BeginToken is Pullenti.Ner.NumberToken)) 
                return null;
            return res;
        }
        public static TransItemToken TryParse(Pullenti.Ner.Token t, TransItemToken prev, bool afterConj, bool attachHigh = false)
        {
            TransItemToken res = _TryParse(t, prev, afterConj, attachHigh);
            if (res == null) 
                return null;
            if (res.Typ == Typs.Name) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(res.EndToken.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null && br.BeginToken.IsChar('(')) 
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
        static TransItemToken _TryParse(Pullenti.Ner.Token t, TransItemToken prev, bool afterConj, bool attachHigh = false)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t1 = t;
            if (t1.IsChar(',')) 
                t1 = t1.Next;
            if (t1 != null) 
            {
                if (t1.IsValue("ПРИНАДЛЕЖАТЬ", "НАЛЕЖАТИ") || t1.IsValue("СУДОВЛАДЕЛЕЦ", "СУДНОВЛАСНИК") || t1.IsValue("ВЛАДЕЛЕЦ", "ВЛАСНИК")) 
                    t1 = t1.Next;
            }
            if (t1 is Pullenti.Ner.ReferentToken) 
            {
                if (t1.GetReferent().TypeName == "ORGANIZATION") 
                    return new TransItemToken(t, t1) { Typ = Typs.Org, Ref = t1.GetReferent(), Morph = t1.Morph };
            }
            if (t1 != null && t1.IsValue("ФЛАГ", null)) 
            {
                Pullenti.Ner.Token tt = t1.Next;
                while (tt != null) 
                {
                    if (tt.IsHiphen || tt.IsChar(':')) 
                        tt = tt.Next;
                    else 
                        break;
                }
                if ((tt is Pullenti.Ner.ReferentToken) && (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    return new TransItemToken(t, tt) { Typ = Typs.Geo, Ref = tt.GetReferent() };
            }
            if (t1 != null && t1.IsValue("ПОРТ", null)) 
            {
                Pullenti.Ner.Token tt = t1.Next;
                for (; tt != null; tt = tt.Next) 
                {
                    if (tt.IsValue("ПРИПИСКА", null) || tt.IsChar(':')) 
                    {
                    }
                    else 
                        break;
                }
                if (tt != null && (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    return new TransItemToken(t, tt) { Typ = Typs.Geo, Ref = tt.GetReferent() };
            }
            bool route = false;
            if (t1 != null && ((t1.IsValue("СЛЕДОВАТЬ", "СЛІДУВАТИ") || t1.IsValue("ВЫПОЛНЯТЬ", "ВИКОНУВАТИ")))) 
            {
                t1 = t1.Next;
                route = true;
            }
            if (t1 != null && t1.Morph.Class.IsPreposition) 
                t1 = t1.Next;
            if (t1 != null && ((t1.IsValue("РЕЙС", null) || t1.IsValue("МАРШРУТ", null)))) 
            {
                t1 = t1.Next;
                route = true;
            }
            if (t1 is Pullenti.Ner.ReferentToken) 
            {
                if (t1.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                {
                    Pullenti.Ner.Geo.GeoReferent geo = t1.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                    if (geo.IsState || geo.IsCity) 
                    {
                        TransItemToken tit = new TransItemToken(t, t1) { Typ = Typs.Route, RouteItems = new List<object>() };
                        tit.RouteItems.Add(geo);
                        for (t1 = t1.Next; t1 != null; t1 = t1.Next) 
                        {
                            if (t1.IsHiphen) 
                                continue;
                            if (t1.Morph.Class.IsPreposition || t1.Morph.Class.IsConjunction) 
                                continue;
                            geo = t1.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                            if (geo == null) 
                                break;
                            if (!geo.IsCity && !geo.IsState) 
                                break;
                            tit.RouteItems.Add(geo);
                            tit.EndToken = t1;
                        }
                        if (tit.RouteItems.Count > 1 || route) 
                            return tit;
                    }
                }
                else if ((t1.GetReferent() is Pullenti.Ner.Date.DateReferent) && (t1.WhitespacesBeforeCount < 3)) 
                {
                    TransItemToken tit = new TransItemToken(t, t1) { Typ = Typs.Date, Ref = t1.GetReferent() };
                    if (t1.Next != null) 
                    {
                        if (t1.Next.IsValue("В", null) && t1.Next.Next != null && t1.Next.Next.IsChar('.')) 
                            tit.EndToken = t1.Next.Next;
                        else if (t1.Next.IsValue("ВЫП", null) || t1.Next.IsValue("ВЫПУСК", null)) 
                        {
                            tit.EndToken = t1.Next;
                            if (t1.Next.Next != null && t1.Next.Next.IsChar('.')) 
                                tit.EndToken = t1.Next.Next;
                        }
                    }
                    return tit;
                }
            }
            if (t is Pullenti.Ner.TextToken) 
            {
                Pullenti.Ner.Token num = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t);
                if (num != null) 
                {
                    TransItemToken tit = _attachRusAutoNumber(num);
                    if (tit == null) 
                        tit = _attachNumber(num, false);
                    if (tit != null) 
                    {
                        tit.BeginToken = t;
                        return tit;
                    }
                }
                Pullenti.Ner.Core.TerminToken tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok == null && ((t.IsValue("С", null) || t.IsValue("C", null) || t.IsValue("ЗА", null)))) 
                    tok = m_Ontology.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                bool isBr = false;
                if (tok == null && Pullenti.Ner.Core.BracketHelper.IsBracket(t, true)) 
                {
                    Pullenti.Ner.Core.TerminToken tok1 = m_Ontology.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok1 != null && Pullenti.Ner.Core.BracketHelper.IsBracket(tok1.EndToken.Next, true)) 
                    {
                        tok = tok1;
                        tok.BeginToken = t;
                        tok.EndToken = tok.EndToken.Next;
                        tok.BeginToken = t;
                        isBr = true;
                    }
                    else if (tok1 != null) 
                    {
                        TransTermin tt = tok1.Termin as TransTermin;
                        if (tt.Typ == Typs.Brand) 
                        {
                            tok = tok1;
                            tok.BeginToken = t;
                        }
                    }
                    if (tok != null && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tok.EndToken.Next, true, null, false)) 
                    {
                        tok.EndToken = tok.EndToken.Next;
                        isBr = true;
                    }
                }
                if (tok == null && t.IsValue("МАРКА", null)) 
                {
                    TransItemToken res1 = _TryParse(t.Next, prev, afterConj, false);
                    if (res1 != null) 
                    {
                        if (res1.Typ == Typs.Name || res1.Typ == Typs.Brand) 
                        {
                            res1.BeginToken = t;
                            res1.Typ = Typs.Brand;
                            return res1;
                        }
                    }
                }
                if (tok != null) 
                {
                    TransTermin tt = tok.Termin as TransTermin;
                    TransItemToken tit;
                    if (tt.Typ == Typs.Number) 
                    {
                        tit = _attachRusAutoNumber(tok.EndToken.Next);
                        if (tit == null) 
                            tit = _attachNumber(tok.EndToken.Next, false);
                        if (tit != null) 
                        {
                            tit.BeginToken = t;
                            return tit;
                        }
                        else 
                            return null;
                    }
                    if (tt.IsDoubt && !attachHigh) 
                    {
                        if (prev == null || prev.Typ != Typs.Noun) 
                        {
                            if ((prev != null && prev.Typ == Typs.Brand && tt.Typ == Typs.Brand) && string.Compare(tt.CanonicText, prev.Value, true) == 0) 
                            {
                            }
                            else 
                                return null;
                        }
                    }
                    if (tt.CanonicText == "СУДНО") 
                    {
                        if (((tok.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) != Pullenti.Morph.MorphNumber.Undefined) 
                        {
                            if (!Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tok.EndToken.Next, false, false)) 
                                return null;
                        }
                    }
                    tit = new TransItemToken(tok.BeginToken, tok.EndToken) { Kind = tt.Kind, Typ = tt.Typ, IsDoubt = tt.IsDoubt && !isBr, Chars = tok.Chars, Morph = tok.Morph };
                    tit.Value = tt.CanonicText;
                    if (tit.Typ == Typs.Noun) 
                    {
                        tit.Value = tit.Value.ToLower();
                        if (((tit.EndToken.Next != null && tit.EndToken.Next.IsHiphen && !tit.EndToken.IsWhitespaceAfter) && (tit.EndToken.Next.Next is Pullenti.Ner.TextToken) && !tit.EndToken.Next.IsWhitespaceAfter) && tit.EndToken.Next.Next.GetMorphClassInDictionary().IsNoun) 
                        {
                            tit.EndToken = tit.EndToken.Next.Next;
                            tit.Value = string.Format("{0}-{1}", tit.Value, tit.EndToken.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) ?? "?").ToLower();
                        }
                    }
                    else 
                        tit.Value = tit.Value.ToUpper();
                    return tit;
                }
                if (tok == null && t.Morph.Class.IsAdjective) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.Adjectives.Count > 0) 
                    {
                        Pullenti.Ner.ReferentToken state = null;
                        for (Pullenti.Ner.Token tt = t; tt != null && tt.Previous != npt.EndToken; tt = tt.Next) 
                        {
                            tok = m_Ontology.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                            if (tok == null && state == null) 
                                state = tt.Kit.ProcessReferent("GEO", tt);
                            if (tok != null && tok.EndToken == npt.EndToken) 
                            {
                                if ((tok.Termin as TransTermin).Typ == Typs.Noun) 
                                {
                                    TransItemToken tit = new TransItemToken(t, tok.EndToken) { Kind = (tok.Termin as TransTermin).Kind, Typ = Typs.Noun, IsDoubt = (tok.Termin as TransTermin).IsDoubt, Chars = tok.Chars, Morph = npt.Morph };
                                    tit.Value = (tok.Termin as TransTermin).CanonicText.ToLower();
                                    tit.AltValue = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false).ToLower();
                                    if (Pullenti.Morph.LanguageHelper.EndsWithEx(tit.AltValue, "суд", "суда", null, null)) 
                                    {
                                        if (!Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tok.EndToken.Next, false, false)) 
                                            continue;
                                    }
                                    if (state != null) 
                                    {
                                        if ((state.Referent as Pullenti.Ner.Geo.GeoReferent).IsState) 
                                            tit.State = state;
                                    }
                                    return tit;
                                }
                            }
                        }
                    }
                }
            }
            if (t != null && t.IsValue("КЛАСС", null) && t.Next != null) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                    return new TransItemToken(t, br.EndToken) { Typ = Typs.Class, Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No) };
            }
            Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
            if (nt != null) 
            {
                if (prev == null || nt.Typ != Pullenti.Ner.NumberSpellingType.Digit) 
                    return null;
                if (prev.Typ == Typs.Brand) 
                    return _attachModel(t, false, prev);
                else 
                    return null;
            }
            TransItemToken res;
            if ((((res = _attachRusAutoNumber(t)))) != null) 
            {
                if (!res.IsDoubt) 
                    return res;
                if (prev != null && prev.Typ == Typs.Noun && prev.Kind == Pullenti.Ner.Transport.TransportKind.Auto) 
                    return res;
                if (prev != null && ((prev.Typ == Typs.Brand || prev.Typ == Typs.Model))) 
                    return res;
            }
            t1 = t;
            if (t.IsHiphen) 
                t1 = t.Next;
            if (prev != null && prev.Typ == Typs.Brand && t1 != null) 
            {
                TransItemToken tit = _attachModel(t1, true, prev);
                if (tit != null) 
                {
                    tit.BeginToken = t;
                    return tit;
                }
            }
            if (prev != null && ((prev.Typ == Typs.Noun || afterConj))) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null && br.IsQuoteType) 
                {
                    TransItemToken tit = TryParse(br.BeginToken.Next, prev, afterConj, false);
                    if (tit != null && tit.EndToken.Next == br.EndToken) 
                    {
                        if (!tit.IsDoubt || tit.Typ == Typs.Brand) 
                        {
                            tit.BeginToken = br.BeginToken;
                            tit.EndToken = br.EndToken;
                            return tit;
                        }
                    }
                    string s = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                    if (!string.IsNullOrEmpty(s) && (s.Length < 30)) 
                    {
                        int chars = 0;
                        int digs = 0;
                        int un = 0;
                        foreach (char c in s) 
                        {
                            if (!char.IsWhiteSpace(c)) 
                            {
                                if (char.IsLetter(c)) 
                                    chars++;
                                else if (char.IsDigit(c)) 
                                    digs++;
                                else 
                                    un++;
                            }
                        }
                        if (((digs == 0 && un == 0 && t.Next.Chars.IsCapitalUpper)) || prev.Kind == Pullenti.Ner.Transport.TransportKind.Ship || prev.Kind == Pullenti.Ner.Transport.TransportKind.Space) 
                            return new TransItemToken(br.BeginToken, br.EndToken) { Typ = Typs.Name, Value = s };
                        if (digs > 0 && (chars < 5)) 
                            return new TransItemToken(br.BeginToken, br.EndToken) { Typ = Typs.Model, Value = s.Replace(" ", "") };
                    }
                }
            }
            if (prev != null && (((prev.Typ == Typs.Noun || prev.Typ == Typs.Brand || prev.Typ == Typs.Name) || prev.Typ == Typs.Model))) 
            {
                TransItemToken tit = _attachModel(t, prev.Typ != Typs.Name, prev);
                if (tit != null) 
                    return tit;
            }
            if (((prev != null && prev.Typ == Typs.Noun && prev.Kind == Pullenti.Ner.Transport.TransportKind.Auto) && (t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) && !t.Chars.IsAllLower && (t.WhitespacesBeforeCount < 2)) 
            {
                Pullenti.Ner.ReferentToken pt = t.Kit.ProcessReferent("PERSON", t);
                if (pt == null) 
                {
                    TransItemToken tit = new TransItemToken(t, t) { Typ = Typs.Brand };
                    tit.Value = (t as Pullenti.Ner.TextToken).Term;
                    Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                    if (mc.IsNoun) 
                        tit.IsDoubt = true;
                    return tit;
                }
            }
            if (((prev != null && prev.Typ == Typs.Noun && ((prev.Kind == Pullenti.Ner.Transport.TransportKind.Ship || prev.Kind == Pullenti.Ner.Transport.TransportKind.Space)))) || afterConj) 
            {
                if (t.Chars.IsCapitalUpper) 
                {
                    bool ok = true;
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.Adjectives.Count > 0) 
                        ok = false;
                    else 
                    {
                        Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("PERSON", t);
                        if (rt != null) 
                            ok = false;
                    }
                    if (t.GetMorphClassInDictionary().IsProperSurname) 
                    {
                        if (!t.Morph.Case.IsNominative) 
                            ok = false;
                    }
                    if (ok) 
                    {
                        t1 = t;
                        TransItemToken tit;
                        for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.WhitespacesBeforeCount > 1) 
                                break;
                            if (tt.Chars != t.Chars) 
                                break;
                            if ((((tit = TryParse(tt, null, false, false)))) != null) 
                                break;
                            t1 = tt;
                        }
                        string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1, Pullenti.Ner.Core.GetTextAttr.No);
                        if (s != null) 
                        {
                            TransItemToken res1 = new TransItemToken(t, t1) { Typ = Typs.Name, IsDoubt = true, Value = s };
                            if (!t1.IsNewlineAfter) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null) 
                                {
                                    res1.EndToken = br.EndToken;
                                    res1.AltValue = res1.Value;
                                    res1.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                                }
                            }
                            return res1;
                        }
                    }
                }
            }
            return null;
        }
        static TransItemToken _attachModel(Pullenti.Ner.Token t, bool canBeFirstWord, TransItemToken prev)
        {
            TransItemToken res = new TransItemToken(t, t) { Typ = Typs.Model };
            StringBuilder cyr = new StringBuilder();
            StringBuilder lat = new StringBuilder();
            Pullenti.Ner.Token t0 = t;
            bool num = false;
            for (; t != null; t = t.Next) 
            {
                if (t != t0 && t.WhitespacesBeforeCount > 1) 
                    break;
                if (t == t0) 
                {
                    if (t.IsHiphen || t.Chars.IsAllLower) 
                    {
                        if (prev == null || prev.Typ != Typs.Brand) 
                            return null;
                    }
                }
                else 
                {
                    TransItemToken pp = TryParse(t, null, false, false);
                    if (pp != null) 
                        break;
                }
                if (t.IsHiphen) 
                {
                    num = false;
                    continue;
                }
                Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                if (nt != null) 
                {
                    if (num) 
                        break;
                    num = true;
                    if (nt.Typ != Pullenti.Ner.NumberSpellingType.Digit) 
                        break;
                    if (cyr != null) 
                        cyr.Append(nt.Value);
                    if (lat != null) 
                        lat.Append(nt.Value);
                    res.EndToken = t;
                    continue;
                }
                if (t != t0 && TryParse(t, null, false, false) != null) 
                    break;
                if (num && t.IsWhitespaceBefore) 
                    break;
                num = false;
                Pullenti.Ner.Core.MiscHelper.CyrLatWord vv = Pullenti.Ner.Core.MiscHelper.GetCyrLatWord(t, 3);
                if (vv == null) 
                {
                    if (canBeFirstWord && t == t0) 
                    {
                        if (t.Chars.IsLetter && t.Chars.IsCapitalUpper) 
                        {
                            if ((((vv = Pullenti.Ner.Core.MiscHelper.GetCyrLatWord(t, 0)))) != null) 
                            {
                                if (t.Morph.Case.IsGenitive && ((prev == null || prev.Typ != Typs.Brand))) 
                                    vv = null;
                                else if (prev != null && prev.Typ == Typs.Noun && ((prev.Kind == Pullenti.Ner.Transport.TransportKind.Ship || prev.Kind == Pullenti.Ner.Transport.TransportKind.Space))) 
                                    vv = null;
                                else 
                                    res.IsDoubt = true;
                            }
                        }
                        if (((vv == null && (t is Pullenti.Ner.TextToken) && !t.Chars.IsAllLower) && t.Chars.IsLatinLetter && prev != null) && prev.Typ == Typs.Brand) 
                        {
                            lat.Append((t as Pullenti.Ner.TextToken).Term);
                            res.EndToken = t;
                            continue;
                        }
                    }
                    if (vv == null) 
                        break;
                }
                if ((vv.Length < 4) || t.Morph.Class.IsPreposition || t.Morph.Class.IsConjunction) 
                {
                    if (t.IsWhitespaceBefore && t.IsWhitespaceAfter) 
                    {
                        if (t.Previous != null && !t.Previous.IsHiphen) 
                        {
                            if (t.Chars.IsAllLower) 
                                break;
                        }
                    }
                }
                if (cyr != null) 
                {
                    if (vv.CyrWord != null) 
                        cyr.Append(vv.CyrWord);
                    else 
                        cyr = null;
                }
                if (lat != null) 
                {
                    if (vv.LatWord != null) 
                        lat.Append(vv.LatWord);
                    else 
                        lat = null;
                }
                res.EndToken = t;
            }
            if (lat == null && cyr == null) 
                return null;
            if (lat != null && lat.Length > 0) 
            {
                res.Value = lat.ToString();
                if (cyr != null && cyr.Length > 0 && res.Value != cyr.ToString()) 
                    res.AltValue = cyr.ToString();
            }
            else if (cyr != null && cyr.Length > 0) 
                res.Value = cyr.ToString();
            if (string.IsNullOrEmpty(res.Value)) 
                return null;
            if (res.Kit.ProcessReferent("PERSON", res.BeginToken) != null) 
                return null;
            return res;
        }
        internal static TransItemToken _attachNumber(Pullenti.Ner.Token t, bool ignoreRegion = false)
        {
            if (t == null) 
                return null;
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    TransItemToken res1 = _attachNumber(t.Next, false);
                    if (res1 != null && res1.EndToken.Next == br.EndToken) 
                    {
                        res1.BeginToken = t;
                        res1.EndToken = br.EndToken;
                        return res1;
                    }
                }
            }
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = t;
            if (t.IsValue("НА", null)) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.Noun.IsValue("ФОН", null)) 
                    t = npt.EndToken.Next;
            }
            StringBuilder res = null;
            for (; t != null; t = t.Next) 
            {
                if (t.IsNewlineBefore) 
                    break;
                if (t != t0 && t.WhitespacesBeforeCount > 1) 
                    break;
                if (t.IsHiphen) 
                    continue;
                Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                if (nt != null) 
                {
                    if (nt.Typ != Pullenti.Ner.NumberSpellingType.Digit || nt.Morph.Class.IsAdjective) 
                        break;
                    if (res == null) 
                        res = new StringBuilder();
                    else if (char.IsDigit(res[res.Length - 1])) 
                        res.Append(' ');
                    res.Append(nt.GetSourceText());
                    t1 = t;
                    continue;
                }
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                {
                    if ((t is Pullenti.Ner.MetaToken) && ((t as Pullenti.Ner.MetaToken).BeginToken.LengthChar < 3) && ((t as Pullenti.Ner.MetaToken).BeginToken is Pullenti.Ner.TextToken)) 
                        tt = (t as Pullenti.Ner.MetaToken).BeginToken as Pullenti.Ner.TextToken;
                    else 
                        break;
                }
                if (!tt.Chars.IsLetter) 
                    break;
                if (!tt.Chars.IsAllUpper && tt.IsWhitespaceBefore) 
                    break;
                if (tt.LengthChar > 3) 
                    break;
                if (res == null) 
                    res = new StringBuilder();
                res.Append(tt.Term);
                t1 = t;
            }
            if (res == null || (res.Length < 4)) 
                return null;
            TransItemToken re = new TransItemToken(t0, t1) { Typ = Typs.Number, Value = res.ToString() };
            if (!ignoreRegion) 
            {
                for (int k = 0, i = res.Length - 1; i > 4; i--,k++) 
                {
                    if (!char.IsDigit(res[i])) 
                    {
                        if (res[i] == ' ' && ((k == 2 || k == 3))) 
                        {
                            re.AltValue = re.Value.Substring(i + 1);
                            re.Value = re.Value.Substring(0, i);
                        }
                        break;
                    }
                }
            }
            re.Value = re.Value.Replace(" ", "");
            if (ignoreRegion) 
                re.AltValue = Pullenti.Ner.Core.MiscHelper.CreateCyrLatAlternative(re.Value);
            return re;
        }
        internal static TransItemToken _attachRusAutoNumber(Pullenti.Ner.Token t)
        {
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    TransItemToken res1 = _attachRusAutoNumber(t.Next);
                    if (res1 != null && res1.EndToken.Next == br.EndToken) 
                    {
                        res1.BeginToken = t;
                        res1.EndToken = br.EndToken;
                        return res1;
                    }
                }
            }
            Pullenti.Ner.Core.MiscHelper.CyrLatWord v1 = Pullenti.Ner.Core.MiscHelper.GetCyrLatWord(t, 1);
            if (v1 == null || v1.CyrWord == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            int doubt = 0;
            if (!t.Chars.IsAllUpper || t.IsWhitespaceAfter) 
                doubt++;
            t = t.Next;
            Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
            if ((nt == null || nt.Typ != Pullenti.Ner.NumberSpellingType.Digit || nt.Morph.Class.IsAdjective) || (nt.EndChar - nt.BeginChar) != 2) 
                return null;
            t = t.Next;
            Pullenti.Ner.Core.MiscHelper.CyrLatWord v2 = Pullenti.Ner.Core.MiscHelper.GetCyrLatWord(t, 2);
            if (v2 == null || v2.CyrWord == null || v2.Length != 2) 
                return null;
            if (!t.Chars.IsAllUpper || t.IsWhitespaceAfter) 
                doubt++;
            TransItemToken res = new TransItemToken(t0, t) { Typ = Typs.Number, Kind = Pullenti.Ner.Transport.TransportKind.Auto };
            res.Value = string.Format("{0}{1}{2}", v1.CyrWord, nt.GetSourceText(), v2.CyrWord);
            nt = t.Next as Pullenti.Ner.NumberToken;
            if (((nt != null && nt.IntValue != null && nt.Typ == Pullenti.Ner.NumberSpellingType.Digit) && !nt.Morph.Class.IsAdjective && nt.IntValue != null) && (nt.IntValue.Value < 1000) && (t.WhitespacesAfterCount < 2)) 
            {
                string n = nt.Value;
                if (n.Length < 2) 
                    n = "0" + n;
                res.AltValue = n;
                res.EndToken = nt;
            }
            if (res.EndToken.Next != null && res.EndToken.Next.IsValue("RUS", null)) 
            {
                res.EndToken = res.EndToken.Next;
                doubt = 0;
            }
            if (doubt > 1) 
                res.IsDoubt = true;
            return res;
        }
        static Pullenti.Ner.Core.TerminCollection m_Ontology;
        public static Pullenti.Ner.Token CheckNumberKeyword(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.Core.TerminToken tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok == null) 
                return null;
            TransTermin tt = tok.Termin as TransTermin;
            if (tt != null && tt.Typ == Typs.Number) 
                return tok.EndToken.Next;
            return null;
        }
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new Pullenti.Ner.Core.TerminCollection();
            TransTermin t;
            t = new TransTermin("автомобиль", true) { Typ = Typs.Noun, Kind = Pullenti.Ner.Transport.TransportKind.Auto };
            t.AddAbridge("а-м");
            t.AddVariant("автомашина", false);
            t.AddVariant("ТРАНСПОРТНОЕ СРЕДСТВО", false);
            t.AddVariant("автомобіль", false);
            m_Ontology.Add(t);
            foreach (string s in new string[] {"ВНЕДОРОЖНИК", "ПОЗАШЛЯХОВИК", "АВТОБУС", "МИКРОАВТОБУС", "ГРУЗОВИК", "МОТОЦИКЛ", "МОПЕД"}) 
            {
                m_Ontology.Add(new TransTermin(s, true) { Typ = Typs.Noun, Kind = Pullenti.Ner.Transport.TransportKind.Auto });
            }
            t = new TransTermin("", true) { Typ = Typs.Noun, Kind = Pullenti.Ner.Transport.TransportKind.Auto };
            t.AddAbridge("а-м");
            m_Ontology.Add(t);
            t = new TransTermin("государственный номер", true) { Typ = Typs.Number, Acronym = "ИМО" };
            t.AddAbridge("г-н");
            t.AddAbridge("н\\з");
            t.AddAbridge("г\\н");
            t.AddVariant("госномер", false);
            t.AddAbridge("гос.номер");
            t.AddAbridge("гос.ном.");
            t.AddAbridge("г.н.з.");
            t.AddAbridge("г.р.з.");
            t.AddVariant("ГРЗ", false);
            t.AddVariant("ГНЗ", false);
            t.AddVariant("регистрационный знак", false);
            t.AddAbridge("рег. знак");
            t.AddVariant("государственный регистрационный знак", false);
            t.AddVariant("бортовой номер", false);
            m_Ontology.Add(t);
            t = new TransTermin("державний номер", true) { Typ = Typs.Number, Lang = Pullenti.Morph.MorphLang.UA };
            t.AddVariant("держномер", false);
            t.AddAbridge("держ.номер");
            t.AddAbridge("держ.ном.");
            m_Ontology.Add(t);
            t = new TransTermin("номер", true) { Typ = Typs.Number };
            m_Ontology.Add(t);
            foreach (string s in new string[] {"КРУИЗНЫЙ ЛАЙНЕР", "ТЕПЛОХОД", "ПАРОХОД", "ЯХТА", "ЛОДКА", "КАТЕР", "КОРАБЛЬ", "СУДНО", "ПОДВОДНАЯ ЛОДКА", "АПК", "ШХУНА", "ПАРОМ", "КРЕЙСЕР", "АВИАНОСЕЦ", "ЭСМИНЕЦ", "ФРЕГАТ", "ЛИНКОР", "АТОМОХОД", "ЛЕДОКОЛ", "ПЛАВБАЗА", "ТАНКЕР", "СУПЕРТАНКЕР", "СУХОГРУЗ", "ТРАУЛЕР", "РЕФРИЖЕРАТОР"}) 
            {
                m_Ontology.Add((t = new TransTermin(s, true) { Typ = Typs.Noun, Kind = Pullenti.Ner.Transport.TransportKind.Ship }));
                if (s == "АПК") 
                    t.IsDoubt = true;
            }
            foreach (string s in new string[] {"КРУЇЗНИЙ ЛАЙНЕР", "ПАРОПЛАВ", "ПАРОПЛАВ", "ЯХТА", "ЧОВЕН", "КОРАБЕЛЬ", "СУДНО", "ПІДВОДНИЙ ЧОВЕН", "АПК", "ШХУНА", "ПОРОМ", "КРЕЙСЕР", "АВІАНОСЕЦЬ", "ЕСМІНЕЦЬ", "ФРЕГАТ", "ЛІНКОР", "АТОМОХІД", "КРИГОЛАМ", "ПЛАВБАЗА", "ТАНКЕР", "СУПЕРТАНКЕР", "СУХОВАНТАЖ", "ТРАУЛЕР", "РЕФРИЖЕРАТОР"}) 
            {
                m_Ontology.Add((t = new TransTermin(s, true) { Typ = Typs.Noun, Lang = Pullenti.Morph.MorphLang.UA, Kind = Pullenti.Ner.Transport.TransportKind.Ship }));
                if (s == "АПК") 
                    t.IsDoubt = true;
            }
            foreach (string s in new string[] {"САМОЛЕТ", "АВИАЛАЙНЕР", "ИСТРЕБИТЕЛЬ", "БОМБАРДИРОВЩИК", "ВЕРТОЛЕТ"}) 
            {
                m_Ontology.Add(new TransTermin(s, true) { Typ = Typs.Noun, Kind = Pullenti.Ner.Transport.TransportKind.Fly });
            }
            foreach (string s in new string[] {"ЛІТАК", "АВІАЛАЙНЕР", "ВИНИЩУВАЧ", "БОМБАРДУВАЛЬНИК", "ВЕРТОЛІТ"}) 
            {
                m_Ontology.Add(new TransTermin(s, true) { Typ = Typs.Noun, Lang = Pullenti.Morph.MorphLang.UA, Kind = Pullenti.Ner.Transport.TransportKind.Fly });
            }
            foreach (string s in new string[] {"КОСМИЧЕСКИЙ КОРАБЛЬ", "ЗВЕЗДОЛЕТ", "КОСМИЧЕСКАЯ СТАНЦИЯ", "РАКЕТА-НОСИТЕЛЬ"}) 
            {
                m_Ontology.Add(new TransTermin(s, true) { Typ = Typs.Noun, Kind = Pullenti.Ner.Transport.TransportKind.Space });
            }
            foreach (string s in new string[] {"КОСМІЧНИЙ КОРАБЕЛЬ", "ЗОРЕЛІТ", "КОСМІЧНА СТАНЦІЯ", "РАКЕТА-НОСІЙ"}) 
            {
                m_Ontology.Add(new TransTermin(s, true) { Typ = Typs.Noun, Lang = Pullenti.Morph.MorphLang.UA, Kind = Pullenti.Ner.Transport.TransportKind.Space });
            }
            _loadBrands(m_Cars, Pullenti.Ner.Transport.TransportKind.Auto);
            _loadBrands(m_Flys, Pullenti.Ner.Transport.TransportKind.Fly);
        }
        class TransTermin : Pullenti.Ner.Core.Termin
        {
            public Pullenti.Ner.Transport.TransportKind Kind;
            public Pullenti.Ner.Transport.Internal.TransItemToken.Typs Typ;
            public bool IsDoubt;
            public TransTermin(string source, bool addLemmaVariant = false) : base(null, null, false)
            {
                this.InitByNormalText(source, null);
            }
        }

        static void _loadBrands(string str, Pullenti.Ner.Transport.TransportKind kind)
        {
            string[] cars = str.Split(';');
            List<string> vars = new List<string>();
            foreach (string c in cars) 
            {
                string[] its = c.Split(',');
                vars.Clear();
                bool doubt = false;
                foreach (string it in its) 
                {
                    string s = it.Trim();
                    if (!string.IsNullOrEmpty(s)) 
                    {
                        if (s == "true") 
                            doubt = true;
                        else 
                            vars.Add(s);
                    }
                }
                if (vars.Count == 0) 
                    continue;
                foreach (string v in vars) 
                {
                    TransTermin t = new TransTermin(v);
                    t.CanonicText = vars[0];
                    t.Kind = kind;
                    t.Typ = Typs.Brand;
                    t.IsDoubt = doubt;
                    m_Ontology.Add(t);
                }
            }
        }
        static string m_Flys = "\n        Boeing, Боинг;\n        Airbus, Аэробус, Эрбас;\n        Ил, Илюшин, true;\n        Ту, Туполев, true;\n        Ан, Антонов, true;\n        Су, Сухой, Sukhoi, Sukhoy, true;\n        Як, Яковлев, true;\n        BAE Systems, БАЕ Системз;\n        ATR, АТР, true;\n        AVIC;\n        Bombardier, Бомбардье;  \n        Britten-Norman, Бриттен-Норман;\n        Cessna, Цессна;\n        Dornier, Дорнье;\n        Embraer, Эмбраер;\n        Fairchild, Fairchild Aerospace, Фэйрчайлд;\n        Fokker, Фоккер;\n        Hawker Beechcraft, Хокер Бичкрафт;\n        Indonesian Aerospace, Индонезиан;\n        Lockheed Martin, Локхид Мартин;\n        LZ Auronautical Industries, LET;\n        Douglas, McDonnell Douglas, Дуглас;\n        NAMC, НАМК;\n        Pilatus, Пилатус, true;\n        Piper Aircraft;\n        Saab, Сааб, true;\n        Shorts, Шортс, true;\n";
        static string m_Cars = "\n        AC Cars;\n        Acura, Акура;\n        Abarth;\n        Alfa Romeo, Альфа Ромео;\n        ALPINA, Альпина, true;\n        Ariel Motor, Ариэль Мотор;\n        ARO, true;\n        Artega, true;\n        Aston Martin;\n        AUDI, Ауди;\n        Austin Healey;\n        BAW;\n        Beijing Jeep;\n        Bentley, Бентли;\n        Bitter, Биттер, true;\n        BMW, БМВ;\n        Brilliance;\n        Bristol, Бристоль, true;\n        Bugatti, Бугатти;\n        Buick, Бьюик;\n        BYD, true;\n        Cadillac, Кадиллак, Кадилак;\n        Caterham;\n        Chery, trye;\n        Chevrolet, Шевроле, Шеврале;\n        Chrysler, Крайслер;\n        Citroen, Ситроен, Ситроэн;\n        Dacia;\n        DADI;\n        Daewoo, Дэо;\n        Dodge, Додж;\n        Daihatsu;\n        Daimler, Даймлер;\n        DKW;\n        Derways;\n        Eagle, true;\n        Elfin Sports Cars;\n        FAW, true;\n        Ferrari, Феррари, Ферари;\n        FIAT, Фиат;\n        Fisker Karma;\n        Ford, Форд;\n        Geely;\n        GEO, true;\n        GMC, true;\n        Gonow;\n        Great Wall, true;\n        Gumpert;\n        Hafei;\n        Haima;\n        Honda, Хонда;\n        Horch;\n        Hudson, true;\n        Hummer, Хаммер;\n        Harley, Харлей;\n        Hyundai, Хюндай, Хундай;\n        Infiniti, true;\n        Isuzu, Исузу;\n        Jaguar, Ягуар, true;\n        Jeep, Джип, true;\n        Kia, Киа, true;\n        Koenigsegg;\n        Lamborghini, Ламборджини;\n        Land Rover, Лендровер, Лэндровер;\n        Landwind;\n        Lancia;\n        Lexus, Лексус;\n        Leyland;\n        Lifan;\n        Lincoln, Линкольн, true;\n        Lotus, true;\n        Mahindra;\n        Maserati;\n        Maybach;\n        Mazda, Мазда;\n        Mercedes-Benz, Mercedes, Мерседес, Мэрседес, Мерседес-бенц;\n        Mercury, true;\n        Mini, true;\n        Mitsubishi, Mitsubishi Motors, Мицубиши, Мицубиси;\n        Morgan, true;\n        Nissan, Nissan Motor, Ниссан, Нисан;\n        Opel, Опель;\n        Pagani;\n        Peugeot, Пежо;\n        Plymouth;\n        Pontiac, Понтиак;\n        Porsche, Порше;\n        Renault, Рено;\n        Rinspeed;\n        Rolls-Royce, Роллс-Ройс;\n        SAAB, Сааб;\n        Saleen;\n        Saturn, Сатурн, true;\n        Scion;\n        Seat, true;\n        Skoda, Шкода;\n        Smart, true;\n        Spyker, true;\n        Ssang Yong, Ссанг янг;\n        Subaru, Субару;\n        Suzuki, Судзуки;\n        Tesla, true;\n        Toyota, Тойота;\n        Vauxhall;\n        Volkswagen, Фольксваген;\n        Volvo, Вольво;\n        Wartburg;\n        Wiesmann;\n        Yamaha, Ямаха;\n        Zenvo;\n\n        ВАЗ, VAZ;\n        ГАЗ, GAZ, true;\n        ЗАЗ, ZAZ;\n        ЗИЛ, ZIL;\n        АЗЛК, AZLK;\n        Иж, true;\n        Москвич, true;\n        УАЗ, UAZ;\n        ТАГАЗ, TaGAZ;\n        Лада, Жигули, true;\n\n";
    }
}