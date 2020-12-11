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

namespace Pullenti.Ner.Instrument.Internal
{
    public class InstrToken1 : Pullenti.Ner.MetaToken
    {
        public InstrToken1(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public Pullenti.Ner.Instrument.InstrumentReferent IRef;
        public bool IsExpired;
        public List<string> Numbers = new List<string>();
        public string MinNumber;
        public NumberTypes NumTyp = NumberTypes.Undefined;
        public string NumSuffix;
        public Pullenti.Ner.Token NumBeginToken;
        public Pullenti.Ner.Token NumEndToken;
        public bool IsNumDoubt = false;
        public int LastNumber
        {
            get
            {
                if (Numbers.Count < 1) 
                    return 0;
                return Pullenti.Ner.Decree.Internal.PartToken.GetNumber(Numbers[Numbers.Count - 1]);
            }
        }
        public int FirstNumber
        {
            get
            {
                if (Numbers.Count < 1) 
                    return 0;
                return Pullenti.Ner.Decree.Internal.PartToken.GetNumber(Numbers[0]);
            }
        }
        public int MiddleNumber
        {
            get
            {
                if (Numbers.Count < 2) 
                    return 0;
                return Pullenti.Ner.Decree.Internal.PartToken.GetNumber(Numbers[1]);
            }
        }
        public int LastMinNumber
        {
            get
            {
                if (MinNumber == null) 
                    return 0;
                return Pullenti.Ner.Decree.Internal.PartToken.GetNumber(MinNumber);
            }
        }
        public bool HasChanges
        {
            get
            {
                for (Pullenti.Ner.Token t = NumEndToken ?? BeginToken; t != null; t = t.Next) 
                {
                    if (t.GetReferent() is Pullenti.Ner.Decree.DecreeChangeReferent) 
                        return true;
                    if (t.EndChar > EndChar) 
                        break;
                }
                return false;
            }
        }
        public Types Typ = Types.Line;
        public enum Types : int
        {
            Line,
            FirstLine,
            Signs,
            Appendix,
            Approved,
            Base,
            Index,
            Title,
            Directive,
            Chapter,
            Clause,
            DocPart,
            Section,
            Subsection,
            Paragraph,
            Subparagraph,
            ClausePart,
            Editions,
            Comment,
            Notice,
        }

        public List<Pullenti.Ner.Decree.Internal.DecreeToken> SignValues = new List<Pullenti.Ner.Decree.Internal.DecreeToken>();
        public string Value;
        public bool AllUpper;
        public bool HasVerb;
        public bool HasManySpecChars;
        public StdTitleType TitleTyp = StdTitleType.Undefined;
        public bool IndexNoKeyword = false;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0} {1} ", Typ, NumTyp);
            if (IsNumDoubt) 
                res.Append("(?) ");
            if (IsExpired) 
                res.Append("(Expired) ");
            if (HasChanges) 
                res.Append("(HasChanges) ");
            for (int i = 0; i < Numbers.Count; i++) 
            {
                res.AppendFormat("{0}{1}", (i > 0 ? "." : ""), Numbers[i]);
            }
            if (NumSuffix != null) 
                res.AppendFormat(" Suf='{0}'", NumSuffix);
            if (Value != null) 
                res.AppendFormat(" '{0}'", Value);
            foreach (Pullenti.Ner.Decree.Internal.DecreeToken s in SignValues) 
            {
                res.AppendFormat(" [{0}]", s.ToString());
            }
            if (AllUpper) 
                res.Append(" AllUpper");
            if (HasVerb) 
                res.Append(" HasVerb");
            if (HasManySpecChars) 
                res.Append(" HasManySpecChars");
            if (TitleTyp != StdTitleType.Undefined) 
                res.AppendFormat(" {0}", TitleTyp);
            if (Value == null) 
                res.AppendFormat(": {0}", this.GetSourceText());
            return res.ToString();
        }
        public static InstrToken1 Parse(Pullenti.Ner.Token t, bool ignoreDirectives, FragToken cur = null, int lev = 0, InstrToken1 prev = null, bool isCitat = false, int maxChar = 0, bool canBeTableCell = false, bool isInIndex = false)
        {
            if (t == null) 
                return null;
            if (t.IsChar('(')) 
            {
                InstrToken1 edt = null;
                FragToken fr = FragToken._createEditions(t);
                if (fr != null) 
                    edt = new InstrToken1(fr.BeginToken, fr.EndToken) { Typ = Types.Editions };
                else 
                {
                    Pullenti.Ner.Token t2 = _createEdition(t);
                    if (t2 != null) 
                        edt = new InstrToken1(t, t2) { Typ = Types.Editions };
                }
                if (edt != null) 
                {
                    if (edt.EndToken.Next != null && edt.EndToken.Next.IsChar('.')) 
                        edt.EndToken = edt.EndToken.Next;
                    return edt;
                }
            }
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t00 = null;
            InstrToken1 res = new InstrToken1(t0, t) { AllUpper = true };
            for (; t != null; t = (t == null ? null : t.Next)) 
            {
                if (!t.IsTableControlChar) 
                    break;
                else 
                {
                    if (t.IsChar((char)0x1E)) 
                    {
                        bool isTable = false;
                        List<Pullenti.Ner.Core.TableRowToken> rows = Pullenti.Ner.Core.TableHelper.TryParseRows(t, 0, true);
                        if (rows != null && rows.Count > 0) 
                        {
                            isTable = true;
                            if (rows[0].Cells.Count > 2 || rows[0].Cells.Count == 0) 
                            {
                            }
                            else if (lev >= 10) 
                                isTable = false;
                            else 
                            {
                                InstrToken1 it11 = Parse(rows[0].BeginToken, true, null, 10, null, false, maxChar, canBeTableCell, false);
                                if (canBeTableCell) 
                                {
                                    if (it11 != null) 
                                        return it11;
                                }
                                if (it11 != null && it11.Numbers.Count > 0) 
                                {
                                    if (it11.TypContainerRank > 0 || it11.LastNumber == 1 || it11.TitleTyp != StdTitleType.Undefined) 
                                        isTable = false;
                                }
                            }
                        }
                        if (isTable) 
                        {
                            int le = 1;
                            for (t = t.Next; t != null; t = t.Next) 
                            {
                                if (t.IsChar((char)0x1E)) 
                                    le++;
                                else if (t.IsChar((char)0x1F)) 
                                {
                                    if ((--le) == 0) 
                                    {
                                        res.EndToken = t;
                                        res.HasVerb = true;
                                        res.AllUpper = false;
                                        return res;
                                    }
                                }
                            }
                        }
                    }
                    if (t != null) 
                        res.EndToken = t;
                }
            }
            if (t == null) 
            {
                if (t0 is Pullenti.Ner.TextToken) 
                    return null;
                t = res.EndToken;
            }
            Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
            if (dt == null && (((t.GetReferent() is Pullenti.Ner.Person.PersonReferent) || (t.GetReferent() is Pullenti.Ner.Instrument.InstrumentParticipantReferent)))) 
            {
                dt = new Pullenti.Ner.Decree.Internal.DecreeToken(t, t) { Typ = Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner };
                dt.Ref = t as Pullenti.Ner.ReferentToken;
            }
            if (dt != null && dt.EndToken.IsNewlineAfter) 
            {
                if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner) 
                {
                    res.Typ = Types.Signs;
                    res.SignValues.Add(dt);
                    res.EndToken = dt.EndToken;
                    res.AllUpper = false;
                    return res;
                }
            }
            if (t.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК") && t.Morph.Case.IsNominative && t.Morph.Number == Pullenti.Morph.MorphNumber.Singular) 
            {
                if (t.Next != null && ((t.Next.IsValue("В", null) || t.Next.IsChar(':')))) 
                {
                }
                else 
                {
                    res.Typ = Types.Appendix;
                    if (t.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) 
                        t = t.Kit.DebedToken(t);
                    for (t = t.Next; t != null; t = t.Next) 
                    {
                        if (res.NumEndToken == null) 
                        {
                            Pullenti.Ner.Token ttt = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t) ?? t;
                            NumberingHelper.ParseNumber(ttt, res, prev);
                            if (res.NumEndToken != null) 
                            {
                                res.EndToken = (t = res.NumEndToken);
                                continue;
                            }
                        }
                        dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                        if (dt != null) 
                        {
                            if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            {
                                res.NumBeginToken = dt.BeginToken;
                                res.NumEndToken = dt.EndToken;
                                if (dt.Value != null) 
                                    res.Numbers.Add(dt.Value.ToUpper());
                            }
                            t = (res.EndToken = dt.EndToken);
                            continue;
                        }
                        if ((t is Pullenti.Ner.NumberToken) && ((t.IsNewlineAfter || ((t.Next != null && t.Next.IsChar('.') && t.Next.IsNewlineAfter))))) 
                        {
                            res.NumBeginToken = t;
                            res.Numbers.Add((t as Pullenti.Ner.NumberToken).Value.ToString());
                            if (t.Next != null && t.Next.IsChar('.')) 
                                t = t.Next;
                            res.NumEndToken = t;
                            res.EndToken = t;
                            continue;
                        }
                        if (((t is Pullenti.Ner.NumberToken) && (t.Next is Pullenti.Ner.TextToken) && t.Next.LengthChar == 1) && ((t.Next.IsNewlineAfter || ((t.Next.Next != null && t.Next.Next.IsChar('.')))))) 
                        {
                            res.NumBeginToken = t;
                            res.Numbers.Add((t as Pullenti.Ner.NumberToken).Value.ToString());
                            res.Numbers.Add((t.Next as Pullenti.Ner.TextToken).Term);
                            res.NumTyp = NumberTypes.Combo;
                            t = t.Next;
                            if (t.Next != null && t.Next.IsChar('.')) 
                                t = t.Next;
                            res.NumEndToken = t;
                            res.EndToken = t;
                            continue;
                        }
                        if (res.NumEndToken == null) 
                        {
                            NumberingHelper.ParseNumber(t, res, prev);
                            if (res.NumEndToken != null) 
                            {
                                res.EndToken = (t = res.NumEndToken);
                                continue;
                            }
                        }
                        if (t.IsValue("К", "ДО") && t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                            break;
                        if (t.Chars.IsLetter) 
                        {
                            Pullenti.Ner.NumberToken lat = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t);
                            if (lat != null && !t.IsValue("C", null) && !t.IsValue("С", null)) 
                            {
                                res.NumBeginToken = t;
                                res.Numbers.Add(lat.Value.ToString());
                                res.NumTyp = NumberTypes.Roman;
                                t = lat.EndToken;
                                if (t.Next != null && ((t.Next.IsChar('.') || t.Next.IsChar(')')))) 
                                    t = t.Next;
                                res.NumEndToken = t;
                                res.EndToken = t;
                                continue;
                            }
                            if (t.LengthChar == 1 && t.Chars.IsAllUpper) 
                            {
                                res.NumBeginToken = t;
                                res.Numbers.Add((t as Pullenti.Ner.TextToken).Term);
                                res.NumTyp = NumberTypes.Letter;
                                if (t.Next != null && ((t.Next.IsChar('.') || t.Next.IsChar(')')))) 
                                    t = t.Next;
                                res.NumEndToken = t;
                                res.EndToken = t;
                                continue;
                            }
                        }
                        if (InstrToken._checkEntered(t) != null) 
                            break;
                        if (t is Pullenti.Ner.TextToken) 
                        {
                            if ((t as Pullenti.Ner.TextToken).IsPureVerb) 
                            {
                                res.Typ = Types.Line;
                                break;
                            }
                        }
                        break;
                    }
                    if (res.Typ != Types.Line) 
                        return res;
                }
            }
            if (t.IsNewlineBefore) 
            {
                if (t.IsValue("МНЕНИЕ", "ДУМКА") || ((t.IsValue("ОСОБОЕ", "ОСОБЛИВА") && t.Next != null && t.Next.IsValue("МНЕНИЕ", "ДУМКА")))) 
                {
                    Pullenti.Ner.Token t1 = t.Next;
                    if (t1 != null && t1.IsValue("МНЕНИЕ", "ДУМКА")) 
                        t1 = t1.Next;
                    bool ok = false;
                    if (t1 != null) 
                    {
                        if (t1.IsNewlineBefore || (t1.GetReferent() is Pullenti.Ner.Person.PersonReferent)) 
                            ok = true;
                    }
                    if (ok) 
                    {
                        res.Typ = Types.Appendix;
                        res.EndToken = t1.Previous;
                        return res;
                    }
                }
                if ((t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && (t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent).Kind == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                    res.Typ = Types.Approved;
            }
            if (t.IsValue("КОНСУЛЬТАНТПЛЮС", null) || t.IsValue("ГАРАНТ", null) || ((t.IsValue("ИНФОРМАЦИЯ", null) && t.IsNewlineBefore))) 
            {
                Pullenti.Ner.Token t1 = t.Next;
                bool ok = false;
                if (t.IsValue("ИНФОРМАЦИЯ", null)) 
                {
                    if (((t.Next != null && t.Next.IsValue("О", null) && t.Next.Next != null) && t.Next.Next.IsValue("ИЗМЕНЕНИЕ", null) && t.Next.Next.Next != null) && t.Next.Next.Next.IsChar(':')) 
                    {
                        t1 = t.Next.Next.Next.Next;
                        ok = true;
                    }
                }
                else if (t1 != null && t1.IsChar(':')) 
                {
                    t1 = t1.Next;
                    ok = true;
                }
                if (t1 != null && ((t1.IsValue("ПРИМЕЧАНИЕ", null) || ok))) 
                {
                    if (t1.Next != null && t1.Next.IsChar('.')) 
                        t1 = t1.Next;
                    InstrToken1 re = new InstrToken1(t, t1) { Typ = Types.Comment };
                    bool hiph = false;
                    for (t1 = t1.Next; t1 != null; t1 = t1.Next) 
                    {
                        re.EndToken = t1;
                        if (!t1.IsNewlineAfter) 
                            continue;
                        if (t1.Next == null) 
                            break;
                        if (t1.Next.IsValue("СМ", null) || t1.Next.IsValue("ПУНКТ", null)) 
                            continue;
                        if (!t1.Next.IsHiphen) 
                            hiph = false;
                        else if (t1.Next.IsHiphen) 
                        {
                            if (t1.IsChar(':')) 
                                hiph = true;
                            if (hiph) 
                                continue;
                        }
                        break;
                    }
                    return re;
                }
            }
            int checkComment = 0;
            for (Pullenti.Ner.Token ttt = t; ttt != null; ttt = ttt.Next) 
            {
                if (((ttt.IsNewlineBefore || ttt.IsTableControlChar)) && ttt != t) 
                    break;
                if (ttt.Morph.Class.IsPreposition) 
                    continue;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt == null) 
                    break;
                if (npt.Noun.IsValue("ПРИМЕНЕНИЕ", "ЗАСТОСУВАННЯ") || npt.Noun.IsValue("ВОПРОС", "ПИТАННЯ")) 
                {
                    checkComment++;
                    ttt = npt.EndToken;
                }
                else 
                    break;
            }
            if (checkComment > 0 || t.IsValue("О", "ПРО")) 
            {
                Pullenti.Ner.Token t1 = null;
                bool ok = false;
                Pullenti.Ner.Decree.DecreeReferent dref = null;
                for (Pullenti.Ner.Token ttt = t.Next; ttt != null; ttt = ttt.Next) 
                {
                    t1 = ttt;
                    if (t1.IsValue("СМ", null) && t1.Next != null && t1.Next.IsChar('.')) 
                    {
                        if (checkComment > 0) 
                            ok = true;
                        if ((t1.Next.Next is Pullenti.Ner.ReferentToken) && (((t1.Next.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) || (t1.Next.Next.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent)))) 
                        {
                            ok = true;
                            dref = t1.Next.Next.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                        }
                    }
                    if (ttt.IsNewlineAfter) 
                        break;
                }
                if (ok) 
                {
                    InstrToken1 cmt = new InstrToken1(t, t1) { Typ = Types.Comment };
                    if (dref != null && t1.Next != null && t1.Next.GetReferent() == dref) 
                    {
                        if (t1.Next.Next != null && t1.Next.Next.IsValue("УТРАТИТЬ", "ВТРАТИТИ")) 
                        {
                            for (Pullenti.Ner.Token ttt = t1.Next.Next; ttt != null; ttt = ttt.Next) 
                            {
                                if (ttt.IsNewlineBefore) 
                                    break;
                                cmt.EndToken = ttt;
                            }
                        }
                    }
                    return cmt;
                }
            }
            Pullenti.Ner.Token tt = InstrToken._checkApproved(t);
            if (tt != null) 
            {
                res.EndToken = tt;
                if (tt.Next != null && (tt.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                {
                    res.Typ = Types.Approved;
                    res.EndToken = tt.Next;
                    return res;
                }
                Pullenti.Ner.Token tt1 = tt;
                if (tt1.IsChar(':') && tt1.Next != null) 
                    tt1 = tt1.Next;
                if ((tt1.GetReferent() is Pullenti.Ner.Person.PersonReferent) || (tt1.GetReferent() is Pullenti.Ner.Instrument.InstrumentParticipantReferent)) 
                {
                    res.Typ = Types.Approved;
                    res.EndToken = tt1;
                    return res;
                }
                Pullenti.Ner.Decree.Internal.DecreeToken dt1 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt.Next, null, false);
                if (dt1 != null && dt1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    res.Typ = Types.Approved;
                    int err = 0;
                    for (Pullenti.Ner.Token ttt = dt1.EndToken.Next; ttt != null; ttt = ttt.Next) 
                    {
                        if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(ttt, false) != null) 
                            break;
                        dt1 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(ttt, null, false);
                        if (dt1 != null) 
                        {
                            if (dt1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ || dt1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) 
                                break;
                            res.EndToken = (ttt = dt1.EndToken);
                            continue;
                        }
                        if (ttt.Morph.Class.IsPreposition || ttt.Morph.Class.IsConjunction) 
                            continue;
                        if (ttt.WhitespacesBeforeCount > 15) 
                            break;
                        if ((++err) > 10) 
                            break;
                    }
                    return res;
                }
            }
            string val = null;
            Pullenti.Ner.Token tt2 = _checkDirective(t, out val);
            if (tt2 != null) 
            {
                if (tt2.IsNewlineAfter || ((tt2.Next != null && ((tt2.Next.IsCharOf(":") || ((tt2.Next.IsChar('.') && tt2 != t)))) && ((tt2.Next.IsNewlineAfter || t.Chars.IsAllUpper))))) 
                    return new InstrToken1(t, (tt2.IsNewlineAfter ? tt2 : tt2.Next)) { Typ = Types.Directive, Value = val };
            }
            if ((lev < 3) && t != null) 
            {
                if ((t.IsValue("СОДЕРЖИМОЕ", "ВМІСТ") || t.IsValue("СОДЕРЖАНИЕ", "ЗМІСТ") || t.IsValue("ОГЛАВЛЕНИЕ", "ЗМІСТ")) || ((t.IsValue("СПИСОК", null) && t.Next != null && t.Next.IsValue("РАЗДЕЛ", null)))) 
                {
                    Pullenti.Ner.Token t11 = t.Next;
                    if (t.IsValue("СПИСОК", null)) 
                        t11 = t11.Next;
                    if (t11 != null && !t11.IsNewlineAfter) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t11, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null && npt.Morph.Case.IsGenitive) 
                            t11 = npt.EndToken.Next;
                    }
                    if (t11 != null && t11.IsCharOf(":.;")) 
                        t11 = t11.Next;
                    if (t11 != null && t11.IsNewlineBefore) 
                    {
                        InstrToken1 first = Parse(t11, ignoreDirectives, null, lev + 1, null, false, 0, false, true);
                        if (first != null && (first.LengthChar < 4)) 
                            first = Parse(first.EndToken.Next, ignoreDirectives, null, lev + 1, null, false, 0, false, false);
                        string fstr = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(first, Pullenti.Ner.Core.GetTextAttr.No);
                        if (first != null) 
                        {
                            int cou = 0;
                            InstrToken1 itprev = null;
                            bool hasApp = false;
                            for (tt = first.EndToken.Next; tt != null; tt = tt.Next) 
                            {
                                if (tt.IsValue("ПРИЛОЖЕНИЕ", null)) 
                                {
                                }
                                if (tt.IsNewlineBefore) 
                                {
                                    if ((++cou) > 400) 
                                        break;
                                }
                                InstrToken1 it = Parse(tt, ignoreDirectives, null, lev + 1, null, false, 0, false, true);
                                if (it == null) 
                                    break;
                                bool ok = false;
                                if (first.Numbers.Count == 1 && it.Numbers.Count == 1) 
                                {
                                    if (it.Typ == Types.Appendix && first.Typ != Types.Appendix) 
                                    {
                                    }
                                    else if (first.Numbers[0] == it.Numbers[0]) 
                                        ok = true;
                                }
                                else if (first.Value != null && it.Value != null && first.Value.StartsWith(it.Value)) 
                                    ok = true;
                                else 
                                {
                                    string str = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(it, Pullenti.Ner.Core.GetTextAttr.No);
                                    if (str == fstr) 
                                        ok = true;
                                }
                                if ((ok && first.Typ != Types.Appendix && itprev != null) && itprev.Typ == Types.Appendix) 
                                {
                                    InstrToken1 it2 = Parse(it.EndToken.Next, ignoreDirectives, null, lev + 1, null, false, 0, false, true);
                                    if (it2 != null && it2.Typ == Types.Appendix) 
                                        ok = false;
                                }
                                if (!ok && cou > 4 && first.Numbers.Count > 0) 
                                {
                                    if (it.Numbers.Count == 1 && it.Numbers[0] == "1") 
                                    {
                                        if (it.TitleTyp == StdTitleType.Others) 
                                            ok = true;
                                    }
                                }
                                if (ok) 
                                {
                                    if (t.Previous == null) 
                                        return null;
                                    res.EndToken = tt.Previous;
                                    res.Typ = Types.Index;
                                    return res;
                                }
                                if (it.Typ == Types.Appendix) 
                                    hasApp = true;
                                tt = it.EndToken;
                                itprev = it;
                            }
                            cou = 0;
                            for (tt = first.BeginToken; tt != null && tt.EndChar <= first.EndChar; tt = tt.Next) 
                            {
                                if (tt.IsTableControlChar) 
                                    cou++;
                            }
                            if (cou > 5) 
                            {
                                res.EndToken = first.EndToken;
                                res.Typ = Types.Index;
                                return res;
                            }
                        }
                    }
                }
            }
            List<Pullenti.Ner.Decree.Internal.PartToken> pts = (t == null ? null : Pullenti.Ner.Decree.Internal.PartToken.TryAttachList((t.IsValue("ПОЛОЖЕНИЕ", "ПОЛОЖЕННЯ") ? t.Next : t), false, 40));
            if ((pts != null && pts.Count > 0 && pts[0].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix) && pts[0].Values.Count > 0 && !pts[0].IsNewlineAfter) 
            {
                bool ok = false;
                tt = pts[pts.Count - 1].EndToken.Next;
                if (tt != null && tt.IsCharOf(".)]")) 
                {
                }
                else 
                    for (; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsValue("ПРИМЕНЯТЬСЯ", "ЗАСТОСОВУВАТИСЯ")) 
                            ok = true;
                        if ((tt.IsValue("ВСТУПАТЬ", "ВСТУПАТИ") && tt.Next != null && tt.Next.Next != null) && tt.Next.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
                            ok = true;
                        if (tt.IsNewlineAfter) 
                        {
                            if (ok) 
                                return new InstrToken1(t, tt) { Typ = Types.Comment };
                            break;
                        }
                    }
            }
            if (t != null && (((t.IsNewlineBefore || isInIndex || isCitat) || ((t.Previous != null && t.Previous.IsTableControlChar)))) && !t.IsTableControlChar) 
            {
                bool ok = true;
                if (t.Next != null && t.Chars.IsAllLower) 
                {
                    if (!t.Morph.Case.IsNominative) 
                        ok = false;
                    else if (t.Next != null && t.Next.IsCharOf(",:;.")) 
                        ok = false;
                    else 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null && npt.EndToken == t) 
                            ok = false;
                    }
                }
                if (ok && (t is Pullenti.Ner.TextToken)) 
                {
                    ok = false;
                    string s = (t as Pullenti.Ner.TextToken).Term;
                    if (s == "ГЛАВА" || s == "ГОЛОВА") 
                    {
                        res.Typ = Types.Chapter;
                        t = t.Next;
                        ok = true;
                    }
                    else if (s == "СТАТЬЯ" || s == "СТАТТЯ") 
                    {
                        res.Typ = Types.Clause;
                        t = t.Next;
                        ok = true;
                    }
                    else if (s == "РАЗДЕЛ" || s == "РОЗДІЛ") 
                    {
                        res.Typ = Types.Section;
                        t = t.Next;
                        ok = true;
                    }
                    else if (s == "ЧАСТЬ" || s == "ЧАСТИНА") 
                    {
                        res.Typ = Types.DocPart;
                        t = t.Next;
                        ok = true;
                    }
                    else if (s == "ПОДРАЗДЕЛ" || s == "ПІДРОЗДІЛ") 
                    {
                        res.Typ = Types.Subsection;
                        t = t.Next;
                        ok = true;
                    }
                    else if ((s == "ПРИМЕЧАНИЕ" || s == "ПРИМІТКА" || s == "ПРИМЕЧАНИЯ") || s == "ПРИМІТКИ") 
                    {
                        res.Typ = Types.Notice;
                        t = t.Next;
                        if (t != null && t.IsCharOf(".:")) 
                            t = t.Next;
                        ok = true;
                    }
                    else if (s == "§" || s == "ПАРАГРАФ") 
                    {
                        res.Typ = Types.Paragraph;
                        t = t.Next;
                        ok = true;
                    }
                    if (ok) 
                    {
                        Pullenti.Ner.Token ttt = t;
                        if (ttt != null && (ttt is Pullenti.Ner.NumberToken)) 
                            ttt = ttt.Next;
                        if (ttt != null && !ttt.IsNewlineBefore) 
                        {
                            if (Pullenti.Ner.Decree.Internal.PartToken.TryAttach(ttt, null, false, false) != null) 
                                res.Typ = Types.Line;
                            else if (InstrToken._checkEntered(ttt) != null) 
                            {
                                res.Typ = Types.Editions;
                                t00 = res.BeginToken;
                            }
                            else if (res.BeginToken.Chars.IsAllLower) 
                            {
                                if (res.BeginToken.NewlinesBeforeCount > 3) 
                                {
                                }
                                else 
                                    res.Typ = Types.Line;
                            }
                        }
                    }
                }
            }
            bool num = res.Typ != Types.Editions;
            bool hasLetters = false;
            bool isApp = cur != null && ((cur.Kind == Pullenti.Ner.Instrument.InstrumentKind.Appendix || cur.Kind == Pullenti.Ner.Instrument.InstrumentKind.InternalDocument));
            for (; t != null; t = t.Next) 
            {
                if (maxChar > 0 && t.BeginChar > maxChar) 
                    break;
                if (t.IsNewlineBefore && t != res.BeginToken) 
                {
                    if (res.Numbers.Count == 2) 
                    {
                        if (res.Numbers[0] == "3" && res.Numbers[1] == "4") 
                        {
                        }
                    }
                    bool isNewLine = true;
                    if (t.NewlinesBeforeCount == 1 && t.Previous != null && t.Previous.Chars.IsLetter) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null && npt.EndChar > t.BeginChar) 
                            isNewLine = false;
                        else if (t.Previous.GetMorphClassInDictionary().IsAdjective) 
                        {
                            npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt != null && npt.Morph.CheckAccord(t.Previous.Morph, false, false)) 
                                isNewLine = false;
                        }
                        if (!isNewLine) 
                        {
                            InstrToken1 tes = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                            if (tes != null && tes.Numbers.Count > 0) 
                                break;
                        }
                        else if (res.Numbers.Count > 0) 
                        {
                            InstrToken1 tes = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                            if (tes != null && tes.Numbers.Count > 0) 
                                break;
                        }
                    }
                    if (isNewLine && t.Chars.IsLetter) 
                    {
                        if (!Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                        {
                            if (t.Previous != null && t.Previous.IsCharOf(":;.")) 
                            {
                            }
                            else if (t.IsValue("НЕТ", null) || t.IsValue("НЕ", null) || t.IsValue("ОТСУТСТВОВАТЬ", null)) 
                            {
                            }
                            else if ((res.Numbers.Count > 0 && t.Previous != null && t.Previous.Chars.IsAllUpper) && !t.Chars.IsAllUpper) 
                            {
                            }
                            else if (t.Previous != null && ((t.Previous.IsValue("ИЛИ", null) || t.Previous.IsCommaAnd)) && res.Numbers.Count > 0) 
                            {
                                InstrToken1 vvv = Parse(t, true, null, 0, null, false, 0, false, false);
                                if (vvv != null && vvv.Numbers.Count > 0) 
                                    isNewLine = true;
                            }
                            else 
                                isNewLine = false;
                        }
                    }
                    if (isNewLine) 
                        break;
                    else 
                    {
                    }
                }
                if (t.IsTableControlChar && t != res.BeginToken) 
                {
                    if (canBeTableCell || t.IsChar((char)0x1E) || t.IsChar((char)0x1F)) 
                        break;
                    if (num && res.Numbers.Count > 0) 
                        num = false;
                    else if (t.Previous == res.NumEndToken) 
                    {
                    }
                    else if (!t.IsNewlineAfter) 
                        continue;
                    else 
                        break;
                }
                if (isInIndex && !t.IsNewlineBefore && !t.Chars.IsAllLower) 
                {
                    Pullenti.Ner.Decree.Internal.PartToken typ = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t, null, false, false);
                    if (typ != null) 
                    {
                        if (((typ.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Chapter || typ.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause || typ.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Section) || typ.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.SubSection || typ.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Paragraph) || typ.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix) 
                        {
                            if (typ.Values.Count == 1) 
                                break;
                        }
                    }
                    Pullenti.Ner.Decree.DecreePartReferent dp = t.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                    if (dp != null) 
                    {
                        if (((dp.GetSlotValue(Pullenti.Ner.Decree.DecreePartReferent.ATTR_CHAPTER) != null || dp.GetSlotValue(Pullenti.Ner.Decree.DecreePartReferent.ATTR_CLAUSE) != null || dp.GetSlotValue(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SECTION) != null) || dp.GetSlotValue(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBSECTION) != null || dp.GetSlotValue(Pullenti.Ner.Decree.DecreePartReferent.ATTR_PARAGRAPH) != null) || dp.GetSlotValue(Pullenti.Ner.Decree.DecreePartReferent.ATTR_APPENDIX) != null) 
                        {
                            t = t.Kit.DebedToken(t);
                            break;
                        }
                    }
                }
                if ((t.IsChar('[') && t == t0 && (t.Next is Pullenti.Ner.NumberToken)) && t.Next.Next != null && t.Next.Next.IsChar(']')) 
                {
                    num = false;
                    res.Numbers.Add((t.Next as Pullenti.Ner.NumberToken).Value.ToString());
                    res.NumTyp = NumberTypes.Digit;
                    res.NumSuffix = "]";
                    res.NumBeginToken = t;
                    res.NumEndToken = t.Next.Next;
                    t = res.NumEndToken;
                    continue;
                }
                if (t.IsChar('(')) 
                {
                    num = false;
                    if (FragToken._createEditions(t) != null) 
                        break;
                    if (_createEdition(t) != null) 
                        break;
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        if (t == res.BeginToken) 
                        {
                            Pullenti.Ner.NumberToken lat = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t.Next);
                            if (lat != null && lat.EndToken.Next == br.EndToken) 
                            {
                                res.Numbers.Add(lat.Value.ToString());
                                res.NumSuffix = ")";
                                res.NumBeginToken = t;
                                res.NumEndToken = br.EndToken;
                                res.NumTyp = (lat.Typ == Pullenti.Ner.NumberSpellingType.Roman ? NumberTypes.Roman : NumberTypes.Digit);
                            }
                            else if (((t == t0 && t.IsNewlineBefore && br.LengthChar == 3) && br.EndToken == t.Next.Next && (t.Next is Pullenti.Ner.TextToken)) && t.Next.Chars.IsLatinLetter) 
                            {
                                res.NumBeginToken = t;
                                res.NumTyp = NumberTypes.Letter;
                                res.Numbers.Add((t.Next as Pullenti.Ner.TextToken).Term);
                                res.EndToken = (res.NumEndToken = t.Next.Next);
                            }
                        }
                        t = (res.EndToken = br.EndToken);
                        continue;
                    }
                }
                if (num) 
                {
                    NumberingHelper.ParseNumber(t, res, prev);
                    num = false;
                    if (res.Numbers.Count > 0) 
                    {
                    }
                    if (res.NumEndToken != null && res.NumEndToken.EndChar >= t.EndChar) 
                    {
                        t = res.NumEndToken;
                        continue;
                    }
                }
                if (res.Numbers.Count == 0) 
                    num = false;
                if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) 
                {
                    hasLetters = true;
                    if (t00 == null) 
                        t00 = t;
                    num = false;
                    if (t.Chars.IsCapitalUpper && res.LengthChar > 20) 
                    {
                        if (t.IsValue("РУКОВОДСТВУЯСЬ", null)) 
                        {
                            if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t) || t.Previous.IsComma) 
                                break;
                        }
                        else if (t.IsValue("НА", null) && t.Next != null && t.Next.IsValue("ОСНОВАНИЕ", null)) 
                        {
                            InstrToken1 ttt = Parse(t, true, null, 0, null, false, 0, false, false);
                            if (ttt != null && ttt.ToString().ToUpper().Contains("РУКОВОДСТВУЯСЬ")) 
                            {
                                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                                    break;
                            }
                        }
                    }
                    if (!t.Chars.IsAllUpper) 
                        res.AllUpper = false;
                    if ((t as Pullenti.Ner.TextToken).IsPureVerb) 
                    {
                        if (t.Chars.IsCyrillicLetter) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse((t.Morph.Class.IsPreposition ? t.Next : t), Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt != null) 
                            {
                            }
                            else 
                                res.HasVerb = true;
                        }
                    }
                }
                else if (t is Pullenti.Ner.ReferentToken) 
                {
                    hasLetters = true;
                    if (t00 == null) 
                        t00 = t;
                    num = false;
                    if (t.GetReferent() is Pullenti.Ner.Decree.DecreeChangeReferent) 
                    {
                        res.HasVerb = true;
                        res.AllUpper = false;
                    }
                    if (t.GetReferent() is Pullenti.Ner.Instrument.InstrumentParticipantReferent) 
                    {
                        if (!t.Chars.IsAllUpper) 
                            res.AllUpper = false;
                    }
                }
                if (t != res.BeginToken && _isFirstLine(t)) 
                    break;
                string tmp;
                tt2 = _checkDirective(t, out tmp);
                if (tt2 != null) 
                {
                    if (tt2.Next != null && tt2.Next.IsCharOf(":.") && tt2.Next.IsNewlineAfter) 
                    {
                        if (ignoreDirectives && !t.IsNewlineBefore) 
                            t = tt2;
                        else 
                            break;
                    }
                }
                res.EndToken = t;
            }
            if (res.TypContainerRank > 0 && t00 != null) 
            {
                if (t00.Chars.IsAllLower) 
                {
                    res.Typ = Types.Line;
                    res.Numbers.Clear();
                    res.NumTyp = NumberTypes.Undefined;
                }
            }
            if (t00 != null) 
            {
                int len = res.EndChar - t00.BeginChar;
                if (len < 1000) 
                {
                    res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t00, res.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    if (Pullenti.Morph.LanguageHelper.EndsWith(res.Value, ".")) 
                        res.Value = res.Value.Substring(0, res.Value.Length - 1);
                }
            }
            if (!hasLetters) 
                res.AllUpper = false;
            if (res.NumTyp != NumberTypes.Undefined && res.BeginToken == res.NumBeginToken && res.EndToken == res.NumEndToken) 
            {
                bool ok = false;
                if (prev != null) 
                {
                    if (NumberingHelper.CalcDelta(prev, res, true) == 1) 
                        ok = true;
                }
                if (!ok) 
                {
                    InstrToken1 res1 = Parse(res.EndToken.Next, true, null, 0, null, false, 0, false, false);
                    if (res1 != null) 
                    {
                        if (NumberingHelper.CalcDelta(res, res1, true) == 1) 
                            ok = true;
                    }
                }
                if (!ok) 
                {
                    res.NumTyp = NumberTypes.Undefined;
                    res.Numbers.Clear();
                }
            }
            if (res.Typ == Types.Appendix || res.TypContainerRank > 0) 
            {
                if (res.Typ == Types.Clause && res.LastNumber == 17) 
                {
                }
                tt = ((res.NumEndToken ?? res.BeginToken)).Next;
                if (tt != null) 
                {
                    Pullenti.Ner.Token ttt = InstrToken._checkEntered(tt);
                    if (ttt != null) 
                    {
                        if (tt.IsValue("УТРАТИТЬ", null) && tt.Previous != null && tt.Previous.IsChar('.')) 
                        {
                            res.Value = null;
                            res.EndToken = tt.Previous;
                            res.IsExpired = true;
                        }
                        else 
                        {
                            res.Typ = Types.Editions;
                            res.Numbers.Clear();
                            res.NumTyp = NumberTypes.Undefined;
                            res.Value = null;
                        }
                    }
                }
            }
            if (res.Typ == Types.DocPart) 
            {
            }
            bool badNumber = false;
            if ((res.TypContainerRank > 0 && res.NumTyp != NumberTypes.Undefined && res.NumEndToken != null) && !res.NumEndToken.IsNewlineAfter && res.NumEndToken.Next != null) 
            {
                Pullenti.Ner.Token t1 = res.NumEndToken.Next;
                bool bad = false;
                if (t1.Chars.IsAllLower) 
                    bad = true;
                if (bad) 
                    badNumber = true;
            }
            if (res.NumTyp != NumberTypes.Undefined && !isCitat) 
            {
                if (res.IsNewlineBefore || isInIndex) 
                {
                }
                else if (res.BeginToken.Previous != null && res.BeginToken.Previous.IsTableControlChar) 
                {
                }
                else 
                    badNumber = true;
                if (res.NumSuffix == "-") 
                    badNumber = true;
            }
            if (res.Typ == Types.Line && res.Numbers.Count > 0 && isCitat) 
            {
                Pullenti.Ner.Token tt0 = res.BeginToken.Previous;
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt0, true, true)) 
                    tt0 = tt0.Previous;
                if (tt0 != null) 
                    tt0 = tt0.Previous;
                for (; tt0 != null; tt0 = tt0.Previous) 
                {
                    if (tt0.IsValue("ГЛАВА", "ГОЛОВА")) 
                        res.Typ = Types.Chapter;
                    else if (tt0.IsValue("СТАТЬЯ", "СТАТТЯ")) 
                        res.Typ = Types.Clause;
                    else if (tt0.IsValue("РАЗДЕЛ", "РОЗДІЛ")) 
                        res.Typ = Types.Section;
                    else if (tt0.IsValue("ЧАСТЬ", "ЧАСТИНА")) 
                        res.Typ = Types.DocPart;
                    else if (tt0.IsValue("ПОДРАЗДЕЛ", "ПІДРОЗДІЛ")) 
                        res.Typ = Types.Subsection;
                    else if (tt0.IsValue("ПАРАГРАФ", null)) 
                        res.Typ = Types.Paragraph;
                    else if (tt0.IsValue("ПРИМЕЧАНИЕ", "ПРИМІТКА")) 
                        res.Typ = Types.Notice;
                    if (tt0.IsNewlineBefore) 
                        break;
                }
            }
            if (badNumber) 
            {
                res.Typ = Types.Line;
                res.NumTyp = NumberTypes.Undefined;
                res.Value = null;
                res.Numbers.Clear();
                res.NumBeginToken = (res.NumEndToken = null);
            }
            if ((res.Typ == Types.Section || res.Typ == Types.Paragraph || res.Typ == Types.Chapter) || res.Typ == Types.Clause) 
            {
                if (res.Numbers.Count == 0) 
                    res.Typ = Types.Line;
            }
            if (res.EndToken.IsChar('>') && res.BeginToken.IsValue("ПУТЕВОДИТЕЛЬ", null)) 
            {
                res.Typ = Types.Comment;
                for (Pullenti.Ner.Token ttt = res.EndToken.Next; ttt != null; ttt = ttt.Next) 
                {
                    InstrToken1 li2 = Parse(ttt, true, null, 0, null, false, 0, false, false);
                    if (li2 != null && li2.EndToken.IsChar('>')) 
                    {
                        res.EndToken = (ttt = li2.EndToken);
                        continue;
                    }
                    break;
                }
                return res;
            }
            if (res.Typ == Types.Line) 
            {
                if (res.NumTyp != NumberTypes.Undefined) 
                {
                    Pullenti.Ner.Token ttt = res.BeginToken.Previous;
                    if (ttt is Pullenti.Ner.TextToken) 
                    {
                        if (ttt.IsValue("ПУНКТ", null)) 
                        {
                            res.NumTyp = NumberTypes.Undefined;
                            res.Value = null;
                            res.Numbers.Clear();
                        }
                    }
                    foreach (string nn in res.Numbers) 
                    {
                        int vv;
                        if (int.TryParse(nn, out vv)) 
                        {
                            if (vv > 1000 && res.NumBeginToken == res.BeginToken) 
                            {
                                res.NumTyp = NumberTypes.Undefined;
                                res.Value = null;
                                res.Numbers.Clear();
                                break;
                            }
                        }
                    }
                }
                if (_isFirstLine(res.BeginToken)) 
                    res.Typ = Types.FirstLine;
                if (res.NumTyp == NumberTypes.Digit) 
                {
                    if (res.NumSuffix == null) 
                        res.IsNumDoubt = true;
                }
                if (res.Numbers.Count == 0) 
                {
                    Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(res.BeginToken, null, false, false);
                    if (pt != null && pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix) 
                    {
                        tt = pt.EndToken.Next;
                        if (tt != null && ((tt.IsCharOf(".") || tt.IsHiphen))) 
                            tt = tt.Next;
                        tt = InstrToken._checkEntered(tt);
                        if (tt != null) 
                        {
                            res.Typ = Types.Editions;
                            res.IsExpired = tt.IsValue("УТРАТИТЬ", "ВТРАТИТИ");
                        }
                    }
                    else 
                    {
                        tt = InstrToken._checkEntered(res.BeginToken);
                        if (tt != null && tt.Next != null && (tt.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                            res.Typ = Types.Editions;
                        else if (res.BeginToken.IsValue("АБЗАЦ", null) && res.BeginToken.Next != null && res.BeginToken.Next.IsValue("УТРАТИТЬ", "ВТРАТИТИ")) 
                            res.IsExpired = true;
                    }
                }
            }
            if (res.Typ == Types.Line && res.NumTyp == NumberTypes.Roman) 
            {
                InstrToken1 res1 = Parse(res.EndToken.Next, true, cur, lev + 1, null, false, 0, false, false);
                if (res1 != null && res1.Typ == Types.Clause) 
                    res.Typ = Types.Chapter;
            }
            int specs = 0;
            int chars = 0;
            if (res.Numbers.Count == 2 && res.Numbers[0] == "2" && res.Numbers[1] == "3") 
            {
            }
            for (tt = (res.NumEndToken == null ? res.BeginToken : res.NumEndToken.Next); tt != null; tt = tt.Next) 
            {
                if (tt.EndChar > res.EndToken.EndChar) 
                    break;
                Pullenti.Ner.TextToken tto = tt as Pullenti.Ner.TextToken;
                if (tto == null) 
                    continue;
                if (!tto.Chars.IsLetter) 
                {
                    if (!tto.IsCharOf(",;.():") && !Pullenti.Ner.Core.BracketHelper.IsBracket(tto, false)) 
                        specs += tto.LengthChar;
                }
                else 
                    chars += tto.LengthChar;
            }
            if ((specs + chars) > 0) 
            {
                if ((((specs * 100) / ((specs + chars)))) > 10) 
                    res.HasManySpecChars = true;
            }
            res.TitleTyp = StdTitleType.Undefined;
            int words = 0;
            for (tt = (res.NumBeginToken == null ? res.BeginToken : res.NumBeginToken.Next); tt != null && tt.EndChar <= res.EndChar; tt = tt.Next) 
            {
                if (!(tt is Pullenti.Ner.TextToken) || tt.IsChar('_')) 
                {
                    res.TitleTyp = StdTitleType.Undefined;
                    break;
                }
                if (!tt.Chars.IsLetter || tt.Morph.Class.IsConjunction || tt.Morph.Class.IsPreposition) 
                    continue;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    words++;
                    int ii;
                    for (ii = 0; ii < m_StdReqWords.Count; ii++) 
                    {
                        if (npt.Noun.IsValue(m_StdReqWords[ii], null)) 
                            break;
                    }
                    if (ii < m_StdReqWords.Count) 
                    {
                        tt = npt.EndToken;
                        res.TitleTyp = StdTitleType.Requisites;
                        continue;
                    }
                    if (npt.Noun.IsValue("ВВЕДЕНИЕ", "ВВЕДЕННЯ") || npt.Noun.IsValue("ВСТУПЛЕНИЕ", "ВСТУП")) 
                    {
                        words++;
                        tt = npt.EndToken;
                        res.TitleTyp = StdTitleType.Others;
                        continue;
                    }
                    if (((npt.Noun.IsValue("ПОЛОЖЕНИЕ", "ПОЛОЖЕННЯ") || npt.Noun.IsValue("СОКРАЩЕНИЕ", "СКОРОЧЕННЯ") || npt.Noun.IsValue("ТЕРМИН", "ТЕРМІН")) || npt.Noun.IsValue("ОПРЕДЕЛЕНИЕ", "ВИЗНАЧЕННЯ") || npt.Noun.IsValue("АББРЕВИАТУРА", "АБРЕВІАТУРА")) || npt.Noun.IsValue("ЛИТЕРАТУРА", "ЛІТЕРАТУРА") || npt.Noun.IsValue("НАЗВАНИЕ", "НАЗВА")) 
                    {
                        tt = npt.EndToken;
                        res.TitleTyp = StdTitleType.Others;
                        continue;
                    }
                    if (npt.Noun.IsValue("ПАСПОРТ", null)) 
                    {
                        tt = npt.EndToken;
                        res.TitleTyp = StdTitleType.Others;
                        Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(npt.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt2 != null && npt2.Morph.Case.IsGenitive && (npt2.WhitespacesBeforeCount < 3)) 
                            tt = npt2.EndToken;
                        continue;
                    }
                    if (npt.Noun.IsValue("ПРЕДМЕТ", null)) 
                    {
                        tt = npt.EndToken;
                        res.TitleTyp = StdTitleType.Subject;
                        continue;
                    }
                    if (npt.EndToken is Pullenti.Ner.TextToken) 
                    {
                        string term = (npt.EndToken as Pullenti.Ner.TextToken).Term;
                        if (term == "ПРИЛОЖЕНИЯ" || term == "ПРИЛОЖЕНИЙ") 
                        {
                            tt = npt.EndToken;
                            res.TitleTyp = StdTitleType.Others;
                            continue;
                        }
                    }
                    if (((npt.Noun.IsValue("МОМЕНТ", null) || npt.Noun.IsValue("ЗАКЛЮЧЕНИЕ", "ВИСНОВОК") || npt.Noun.IsValue("ДАННЫЕ", null)) || npt.IsValue("ДОГОВОР", "ДОГОВІР") || npt.IsValue("КОНТРАКТ", null)) || npt.IsValue("СПИСОК", null) || npt.IsValue("ПЕРЕЧЕНЬ", "ПЕРЕЛІК")) 
                    {
                        tt = npt.EndToken;
                        continue;
                    }
                }
                ParticipantToken pp = ParticipantToken.TryAttach(tt, null, null, false);
                if (pp != null && pp.Kind == ParticipantToken.Kinds.Pure) 
                {
                    tt = pp.EndToken;
                    continue;
                }
                res.TitleTyp = StdTitleType.Undefined;
                break;
            }
            if (res.TitleTyp != StdTitleType.Undefined && res.Numbers.Count == 0) 
            {
                t = res.BeginToken;
                if (!(t is Pullenti.Ner.TextToken) || !t.Chars.IsLetter || t.Chars.IsAllLower) 
                    res.TitleTyp = StdTitleType.Undefined;
            }
            if ((res.Numbers.Count == 0 && !res.IsNewlineBefore && res.BeginToken.Previous != null) && res.BeginToken.Previous.IsTableControlChar) 
                res.TitleTyp = StdTitleType.Undefined;
            for (t = res.EndToken.Next; t != null; t = t.Next) 
            {
                if (!t.IsTableControlChar) 
                    break;
                else if (t.IsChar((char)0x1E)) 
                    break;
                else 
                    res.EndToken = t;
            }
            return res;
        }
        static List<string> m_StdReqWords = new List<string>(new string[] {"РЕКВИЗИТ", "ПОДПИСЬ", "СТОРОНА", "АДРЕС", "ТЕЛЕФОН", "МЕСТО", "НАХОЖДЕНИЕ", "МЕСТОНАХОЖДЕНИЕ", "ТЕРМИН", "ОПРЕДЕЛЕНИЕ", "СЧЕТ", "РЕКВІЗИТ", "ПІДПИС", "СТОРОНА", "АДРЕСА", "МІСЦЕ", "ЗНАХОДЖЕННЯ", "МІСЦЕЗНАХОДЖЕННЯ", "ТЕРМІН", "ВИЗНАЧЕННЯ", "РАХУНОК"});
        static bool _isFirstLine(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return false;
            string v = tt.Term;
            if ((((v == "ИСХОДЯ" || v == "ВИХОДЯЧИ")) && t.Next != null && t.Next.IsValue("ИЗ", "З")) && t.Next.Next != null && t.Next.Next.IsValue("ИЗЛОЖЕННОЕ", "ВИКЛАДЕНЕ")) 
                return true;
            if ((((v == "НА" || v == "HA")) && t.Next != null && t.Next.IsValue("ОСНОВАНИЕ", "ПІДСТАВА")) && t.Next.Next != null && t.Next.Next.IsValue("ИЗЛОЖЕННОЕ", "ВИКЛАДЕНЕ")) 
                return true;
            if (((v == "УЧИТЫВАЯ" || v == "ВРАХОВУЮЧИ")) && t.Next != null && t.Next.IsValue("ИЗЛОЖЕННОЕ", "ВИКЛАДЕНЕ")) 
                return true;
            if ((v == "ЗАСЛУШАВ" || v == "РАССМОТРЕВ" || v == "ЗАСЛУХАВШИ") || v == "РОЗГЛЯНУВШИ") 
                return true;
            if (v == "РУКОВОДСТВУЯСЬ" || v == "КЕРУЮЧИСЬ") 
                return tt.IsNewlineBefore;
            return false;
        }
        public static Pullenti.Ner.Token _createEdition(Pullenti.Ner.Token t)
        {
            if (t == null || t.Next == null) 
                return null;
            bool ok = false;
            Pullenti.Ner.Token t1 = t;
            int br = 0;
            if (t.IsChar('(') && t.IsNewlineBefore) 
            {
                ok = true;
                br = 1;
                t1 = t.Next;
            }
            if (!ok || t1 == null) 
                return null;
            ok = false;
            List<Pullenti.Ner.Decree.Internal.PartToken> dts = Pullenti.Ner.Decree.Internal.PartToken.TryAttachList(t1, true, 40);
            if (dts != null && dts.Count > 0) 
                t1 = dts[dts.Count - 1].EndToken.Next;
            Pullenti.Ner.Token t2 = InstrToken._checkEntered(t1);
            if (t2 == null && t1 != null) 
                t2 = InstrToken._checkEntered(t1.Next);
            if (t2 != null) 
                ok = true;
            if (!ok) 
                return null;
            for (t1 = t2; t1 != null; t1 = t1.Next) 
            {
                if (t1.IsChar(')')) 
                {
                    if ((--br) == 0) 
                        return t1;
                }
                else if (t1.IsChar('(')) 
                    br++;
                else if (t1.IsNewlineAfter) 
                    break;
            }
            return null;
        }
        internal static Pullenti.Ner.Token _checkDirective(Pullenti.Ner.Token t, out string val)
        {
            val = null;
            if (t == null || t.Morph.Class.IsAdjective) 
                return null;
            for (int ii = 0; ii < InstrToken.m_Directives.Count; ii++) 
            {
                if (t.IsValue(InstrToken.m_Directives[ii], null)) 
                {
                    val = InstrToken.m_DirectivesNorm[ii];
                    if (t.WhitespacesBeforeCount < 7) 
                    {
                        if (((((val != "ПРИКАЗ" && val != "ПОСТАНОВЛЕНИЕ" && val != "УСТАНОВЛЕНИЕ") && val != "РЕШЕНИЕ" && val != "ЗАЯВЛЕНИЕ") && val != "НАКАЗ" && val != "ПОСТАНОВА") && val != "ВСТАНОВЛЕННЯ" && val != "РІШЕННЯ") && val != "ЗАЯВУ") 
                        {
                            if ((t.Next != null && t.Next.IsChar(':') && t.Next.IsNewlineAfter) && t.Chars.IsAllUpper) 
                            {
                            }
                            else 
                                break;
                        }
                    }
                    if (t.Next != null && t.Next.IsValue("СЛЕДУЮЩЕЕ", "НАСТУПНЕ")) 
                        return t.Next;
                    if (((val == "ЗАЯВЛЕНИЕ" || val == "ЗАЯВА")) && t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                        t = t.Next;
                    return t;
                }
            }
            if (t.Chars.IsLetter && t.LengthChar == 1) 
            {
                if (t.IsNewlineBefore || ((t.Next != null && t.Next.Chars.IsLetter && t.Next.LengthChar == 1))) 
                {
                    for (int ii = 0; ii < InstrToken.m_Directives.Count; ii++) 
                    {
                        Pullenti.Ner.Token res = Pullenti.Ner.Core.MiscHelper.TryAttachWordByLetters(InstrToken.m_Directives[ii], t, true);
                        if (res != null) 
                        {
                            val = InstrToken.m_DirectivesNorm[ii];
                            return res;
                        }
                    }
                }
            }
            return null;
        }
        public int TypContainerRank
        {
            get
            {
                int res = _calcRank(Typ);
                return res;
            }
        }
        public static int _calcRank(Types ty)
        {
            if (ty == Types.DocPart) 
                return 1;
            if (ty == Types.Section) 
                return 2;
            if (ty == Types.Subsection) 
                return 3;
            if (ty == Types.Chapter) 
                return 4;
            if (ty == Types.Paragraph) 
                return 5;
            if (ty == Types.Subparagraph) 
                return 6;
            if (ty == Types.Clause) 
                return 7;
            return 0;
        }
        public bool CanBeContainerFor(InstrToken1 lt)
        {
            int r = _calcRank(Typ);
            int r1 = _calcRank(lt.Typ);
            if (r > 0 && r1 > 0) 
                return r < r1;
            return false;
        }
        public enum StdTitleType : int
        {
            Undefined,
            Subject,
            Requisites,
            Others,
        }

    }
}