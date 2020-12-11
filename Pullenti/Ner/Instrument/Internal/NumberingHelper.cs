/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Instrument.Internal
{
    // Поддержка анализа нумерации
    public static class NumberingHelper
    {
        public static int CalcDelta(InstrToken1 prev, InstrToken1 next, bool canSubNumbers)
        {
            int n1 = prev.LastNumber;
            int n2 = next.LastNumber;
            if (next.LastMinNumber > 0) 
                n2 = next.LastMinNumber;
            if (prev.Numbers.Count == next.Numbers.Count) 
            {
                if (prev.TypContainerRank > 0 && prev.TypContainerRank == next.TypContainerRank) 
                {
                }
                else if (prev.NumTyp == next.NumTyp) 
                {
                }
                else 
                    return 0;
                if (prev.Numbers.Count > 1) 
                {
                    for (int i = 0; i < (prev.Numbers.Count - 1); i++) 
                    {
                        if (prev.Numbers[i] != next.Numbers[i]) 
                            return 0;
                    }
                }
                if (n1 >= n2) 
                    return 0;
                return n2 - n1;
            }
            if (!canSubNumbers) 
                return 0;
            if ((prev.Numbers.Count + 1) == next.Numbers.Count && next.Numbers.Count > 0) 
            {
                if (prev.TypContainerRank > 0 && prev.TypContainerRank == next.TypContainerRank) 
                {
                }
                else if (prev.NumTyp == NumberTypes.Digit && next.NumTyp == NumberTypes.TwoDigits) 
                {
                }
                else if (prev.NumTyp == NumberTypes.TwoDigits && next.NumTyp == NumberTypes.ThreeDigits) 
                {
                }
                else if (prev.NumTyp == NumberTypes.ThreeDigits && next.NumTyp == NumberTypes.FourDigits) 
                {
                }
                else if (prev.NumTyp == NumberTypes.Letter && next.NumTyp == NumberTypes.TwoDigits && char.IsLetter(next.Numbers[0][0])) 
                {
                }
                else 
                    return 0;
                for (int i = 0; i < prev.Numbers.Count; i++) 
                {
                    if (prev.Numbers[i] != next.Numbers[i]) 
                        return 0;
                }
                return n2;
            }
            if ((prev.Numbers.Count - 1) == next.Numbers.Count && prev.Numbers.Count > 1) 
            {
                if (prev.TypContainerRank > 0 && prev.TypContainerRank == next.TypContainerRank) 
                {
                }
                else if (prev.NumTyp == NumberTypes.TwoDigits) 
                {
                    if (next.NumTyp == NumberTypes.Digit) 
                    {
                    }
                    else if (next.NumTyp == NumberTypes.Letter && char.IsLetter(prev.Numbers[0][0])) 
                    {
                    }
                }
                else if (prev.NumTyp == NumberTypes.ThreeDigits && next.NumTyp == NumberTypes.TwoDigits) 
                {
                }
                else if (prev.NumTyp == NumberTypes.FourDigits && next.NumTyp == NumberTypes.ThreeDigits) 
                {
                }
                else 
                    return 0;
                for (int i = 0; i < (prev.Numbers.Count - 2); i++) 
                {
                    if (prev.Numbers[i] != next.Numbers[i]) 
                        return 0;
                }
                if (!int.TryParse(prev.Numbers[prev.Numbers.Count - 2], out n1)) 
                {
                    if (prev.Numbers.Count == 2) 
                        n1 = prev.FirstNumber;
                    else 
                        return 0;
                }
                if ((n1 + 1) != n2) 
                    return 0;
                return n2 - n1;
            }
            if ((prev.Numbers.Count - 2) == next.Numbers.Count && prev.Numbers.Count > 2) 
            {
                if (prev.TypContainerRank > 0 && prev.TypContainerRank == next.TypContainerRank) 
                {
                }
                else if (prev.NumTyp == NumberTypes.ThreeDigits && next.NumTyp == NumberTypes.Digit) 
                {
                }
                else if (prev.NumTyp == NumberTypes.FourDigits && next.NumTyp == NumberTypes.TwoDigits) 
                {
                }
                else 
                    return 0;
                for (int i = 0; i < (prev.Numbers.Count - 3); i++) 
                {
                    if (prev.Numbers[i] != next.Numbers[i]) 
                        return 0;
                }
                if (!int.TryParse(prev.Numbers[prev.Numbers.Count - 3], out n1)) 
                    return 0;
                if ((n1 + 1) != n2) 
                    return 0;
                return n2 - n1;
            }
            if ((prev.Numbers.Count - 3) == next.Numbers.Count && prev.Numbers.Count > 3) 
            {
                if (prev.TypContainerRank > 0 && prev.TypContainerRank == next.TypContainerRank) 
                {
                }
                else if (prev.NumTyp == NumberTypes.FourDigits && next.NumTyp == NumberTypes.Digit) 
                {
                }
                else 
                    return 0;
                for (int i = 0; i < (prev.Numbers.Count - 4); i++) 
                {
                    if (prev.Numbers[i] != next.Numbers[i]) 
                        return 0;
                }
                if (!int.TryParse(prev.Numbers[prev.Numbers.Count - 4], out n1)) 
                    return 0;
                if ((n1 + 1) != n2) 
                    return 0;
                return n2 - n1;
            }
            return 0;
        }
        public static List<InstrToken1> ExtractMainSequence(List<InstrToken1> lines, bool checkSpecTexts, bool canSubNumbers)
        {
            List<InstrToken1> res = null;
            int manySpecCharLines = 0;
            for (int i = 0; i < lines.Count; i++) 
            {
                InstrToken1 li = lines[i];
                if (li.AllUpper && li.TitleTyp != InstrToken1.StdTitleType.Undefined) 
                {
                    if (res != null && res.Count > 0 && res[res.Count - 1].Tag == null) 
                        res[res.Count - 1].Tag = li;
                }
                if (li.Numbers.Count == 0) 
                    continue;
                if (li.LastNumber == 901) 
                {
                }
                if (li.NumTyp == NumberTypes.Letter) 
                {
                }
                if (li.Typ != InstrToken1.Types.Line) 
                    continue;
                if (res == null) 
                {
                    res = new List<InstrToken1>();
                    if (li.Numbers.Count == 1 && li.Numbers[0] == "1" && li.NumTyp == NumberTypes.Digit) 
                    {
                        if ((((i + 1) < lines.Count) && lines[i + 1].Numbers.Count == 1 && lines[i + 1].Numbers[0] == "1") && lines[i + 1].NumTyp == NumberTypes.Digit) 
                        {
                            for (int ii = i + 2; ii < lines.Count; ii++) 
                            {
                                if (lines[ii].NumTyp == NumberTypes.Roman && lines[ii].Numbers.Count > 0) 
                                {
                                    if (lines[ii].Numbers[0] == "2") 
                                        li.NumTyp = NumberTypes.Roman;
                                    break;
                                }
                            }
                        }
                    }
                }
                else 
                {
                    if (res[0].NumSuffix != null) 
                    {
                        if (li.NumSuffix != null && li.NumSuffix != res[0].NumSuffix) 
                            continue;
                    }
                    if (res[0].Numbers.Count != li.Numbers.Count) 
                    {
                        if (li.BeginToken.Previous != null && li.BeginToken.Previous.IsChar(':')) 
                            continue;
                        if (res[0].NumSuffix == null || CalcDelta(res[res.Count - 1], li, true) != 1) 
                            continue;
                        if (!canSubNumbers) 
                        {
                            if (((i + 1) < lines.Count) && CalcDelta(res[res.Count - 1], lines[i + 1], false) == 1 && CalcDelta(li, lines[i + 1], true) == 1) 
                            {
                            }
                            else 
                                continue;
                        }
                    }
                    else 
                    {
                        if (res[0].NumTyp == NumberTypes.Roman && li.NumTyp != NumberTypes.Roman) 
                            continue;
                        if (res[0].NumTyp != NumberTypes.Roman && li.NumTyp == NumberTypes.Roman) 
                        {
                            if (li.Numbers.Count == 1 && li.Numbers[0] == "1" && res.Count == 1) 
                            {
                                res.Clear();
                                res.Add(li);
                                continue;
                            }
                            continue;
                        }
                        if (res[0].NumTyp != NumberTypes.Letter && li.NumTyp == NumberTypes.Letter) 
                            continue;
                    }
                }
                res.Add(li);
                if (li.HasManySpecChars) 
                    manySpecCharLines++;
            }
            if (res == null) 
                return null;
            if (checkSpecTexts) 
            {
                if (manySpecCharLines > (res.Count / 2)) 
                    return null;
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (CalcDelta(res[i], res[i + 1], false) == 2) 
                {
                    int ii0 = lines.IndexOf(res[i]);
                    int ii1 = lines.IndexOf(res[i + 1], ii0);
                    for (int j = ii0 + 1; j < ii1; j++) 
                    {
                        if (lines[j].Numbers.Count > 0) 
                        {
                            if (CalcDelta(res[i], lines[j], true) == 1 && NumberingHelper.CalcDelta(lines[j], res[i + 1], true) == 1) 
                            {
                                res.Insert(i + 1, lines[j]);
                                break;
                            }
                        }
                    }
                }
            }
            bool ch = true;
            while (ch) 
            {
                ch = false;
                for (int i = 1; i < res.Count; i++) 
                {
                    int d = CalcDelta(res[i - 1], res[i], false);
                    if (res[i - 1].NumSuffix == res[i].NumSuffix) 
                    {
                        if (d == 1) 
                            continue;
                        if (((d > 1 && (d < 20))) || ((d == 0 && res[i - 1].NumTyp == res[i].NumTyp && res[i - 1].Numbers.Count == res[i].Numbers.Count))) 
                        {
                            if (CalcDelta(res[i], res[i - 1], false) > 0) 
                            {
                                if (res[i - 1].Tag != null && i > 2) 
                                {
                                    res.RemoveRange(i, res.Count - i);
                                    ch = true;
                                    i--;
                                    continue;
                                }
                            }
                            if ((i + 1) < res.Count) 
                            {
                                int dd = CalcDelta(res[i], res[i + 1], false);
                                if (dd == 1) 
                                {
                                    if (res[i].LastNumber == 1 && res[i].Numbers.Count == res[i - 1].Numbers.Count) 
                                    {
                                    }
                                    else 
                                        continue;
                                }
                                else 
                                {
                                    dd = CalcDelta(res[i - 1], res[i + 1], false);
                                    if (dd == 1) 
                                    {
                                        res.RemoveAt(i);
                                        i--;
                                        ch = true;
                                        continue;
                                    }
                                }
                            }
                            else if (d > 3) 
                            {
                                res.RemoveAt(i);
                                i--;
                                ch = true;
                                continue;
                            }
                            else 
                                continue;
                        }
                    }
                    int j;
                    for (j = i + 1; j < res.Count; j++) 
                    {
                        int dd = CalcDelta(res[j - 1], res[j], false);
                        if (dd != 1 && dd != 2) 
                            break;
                        if (res[j - 1].NumSuffix != res[j].NumSuffix) 
                            break;
                    }
                    if ((d == 0 && CalcDelta(res[i - 1], res[i], true) == 1 && res[i - 1].NumSuffix != null) && res[i].NumSuffix == res[i - 1].NumSuffix) 
                        d = 1;
                    if (d != 1 && j > (i + 1)) 
                    {
                        res.RemoveRange(i, j - i);
                        i--;
                        ch = true;
                        continue;
                    }
                    if (d == 1) 
                    {
                        if ((i + 1) >= res.Count) 
                            continue;
                        int dd = CalcDelta(res[i], res[i + 1], false);
                        if (dd == 1 && res[i - 1].NumSuffix == res[i + 1].NumSuffix) 
                        {
                            if (res[i].NumSuffix != res[i - 1].NumSuffix) 
                            {
                                res[i].NumSuffix = res[i - 1].NumSuffix;
                                res[i].IsNumDoubt = false;
                                ch = true;
                            }
                            continue;
                        }
                    }
                    if ((i + 1) < res.Count) 
                    {
                        int dd = CalcDelta(res[i - 1], res[i + 1], false);
                        if (dd == 1 && res[i - 1].NumSuffix == res[i + 1].NumSuffix) 
                        {
                            if (d == 1 && CalcDelta(res[i], res[i + 1], true) == 1) 
                            {
                            }
                            else 
                            {
                                res.RemoveAt(i);
                                ch = true;
                                continue;
                            }
                        }
                    }
                    else if (d == 0 || d > 10 || res[i - 1].NumSuffix != res[i].NumSuffix) 
                    {
                        res.RemoveAt(i);
                        ch = true;
                        continue;
                    }
                }
            }
            int hasSuf = 0;
            foreach (InstrToken1 r in res) 
            {
                if ((r.NumSuffix != null || r.TypContainerRank > 0 || r.Numbers.Count > 1) || r.AllUpper || r.NumTyp == NumberTypes.Roman) 
                    hasSuf++;
            }
            if (hasSuf == 0) 
            {
                if (res.Count < 5) 
                    return null;
            }
            if (res.Count >= 2) 
            {
                if (res[0] != lines[0]) 
                {
                    int tot = res[0].BeginToken.BeginChar - lines[0].BeginToken.BeginChar;
                    tot += (lines[lines.Count - 1].EndToken.EndChar - res[res.Count - 1].EndToken.EndChar);
                    int blk = res[res.Count - 1].EndToken.EndChar - res[0].BeginToken.BeginChar;
                    int i = lines.IndexOf(res[res.Count - 1]);
                    if (i > 0) 
                    {
                        List<InstrToken1> lines1 = new List<InstrToken1>(lines);
                        lines1.RemoveRange(0, i + 1);
                        List<InstrToken1> res1 = ExtractMainSequence(lines1, checkSpecTexts, canSubNumbers);
                        if (res1 != null && res1.Count > 2) 
                            blk += (res1[res1.Count - 1].EndChar - res1[0].BeginChar);
                    }
                    if ((blk * 3) < tot) 
                    {
                        if ((blk * 5) < tot) 
                            return null;
                        foreach (InstrToken1 r in res) 
                        {
                            if (!r.AllUpper && !r.HasChanges) 
                                return null;
                        }
                    }
                }
                if (res[0].LastNumber == 1 && res[0].Numbers.Count == 1) 
                {
                    List<InstrToken1> res0 = new List<InstrToken1>();
                    res0.Add(res[0]);
                    int i;
                    for (i = 1; i < res.Count; i++) 
                    {
                        int j;
                        for (j = i + 1; j < res.Count; j++) 
                        {
                            if (res[j].LastNumber == 1 && res[j].Numbers.Count == 1) 
                                break;
                        }
                        if ((j - i) < 3) 
                            break;
                        j--;
                        int jj;
                        int errs = 0;
                        for (jj = i + 1; jj < j; jj++) 
                        {
                            int d = CalcDelta(res[jj - 1], res[jj], false);
                            if (d == 1) 
                            {
                            }
                            else if (d > 1 && (d < 3)) 
                                errs++;
                            else 
                                break;
                        }
                        if ((jj < j) || errs > 1) 
                            break;
                        if (j < (res.Count - 1)) 
                        {
                            if (CalcDelta(res0[res0.Count - 1], res[j], false) != 1) 
                                break;
                            res0.Add(res[j]);
                        }
                        i = j;
                    }
                    if (i >= res.Count && res0.Count > 1) 
                        return res0;
                }
                if (res.Count > 500) 
                    return null;
                return res;
            }
            if (res.Count == 1 && lines[0] == res[0]) 
            {
                if (hasSuf > 0) 
                    return res;
                if (lines.Count > 1 && lines[1].Numbers.Count == (lines[0].Numbers.Count + 1)) 
                {
                    for (int i = 0; i < lines[0].Numbers.Count; i++) 
                    {
                        if (lines[1].Numbers[i] != lines[0].Numbers[i]) 
                            return null;
                    }
                    return res;
                }
            }
            return null;
        }
        public static void CreateNumber(FragToken owner, InstrToken1 itok)
        {
            if (itok.NumBeginToken == null || itok.NumEndToken == null) 
                return;
            FragToken num = new FragToken(itok.NumBeginToken, itok.NumEndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Number, DefVal = true, Itok = itok };
            owner.Children.Add(num);
            if (itok.NumTyp == NumberTypes.TwoDigits) 
            {
                owner.Number = itok.FirstNumber;
                owner.SubNumber = itok.LastNumber;
            }
            else if (itok.NumTyp == NumberTypes.ThreeDigits) 
            {
                owner.Number = itok.FirstNumber;
                owner.SubNumber = itok.MiddleNumber;
                owner.SubNumber2 = itok.LastNumber;
            }
            else if (itok.NumTyp == NumberTypes.FourDigits && itok.Numbers.Count == 4) 
            {
                owner.Number = itok.FirstNumber;
                owner.SubNumber = Pullenti.Ner.Decree.Internal.PartToken.GetNumber(itok.Numbers[1]);
                owner.SubNumber2 = Pullenti.Ner.Decree.Internal.PartToken.GetNumber(itok.Numbers[2]);
                owner.SubNumber3 = itok.LastNumber;
            }
            else 
                owner.Number = itok.LastNumber;
            owner.MinNumber = itok.LastMinNumber;
            owner.Itok = itok;
        }
        public static void ParseNumber(Pullenti.Ner.Token t, InstrToken1 res, InstrToken1 prev)
        {
            _parseNumber(t, res, prev);
            if ((res.Numbers.Count > 0 && res.NumEndToken != null && !res.IsNewlineAfter) && res.NumEndToken.Next != null && res.NumEndToken.Next.IsHiphen) 
            {
                InstrToken1 res1 = new InstrToken1(res.NumEndToken.Next.Next, res.NumEndToken.Next.Next);
                _parseNumber(res1.BeginToken, res1, res);
                if (res1.Numbers.Count == res.Numbers.Count) 
                {
                    int i;
                    for (i = 0; i < (res.Numbers.Count - 1); i++) 
                    {
                        if (res.Numbers[i] != res1.Numbers[i]) 
                            break;
                    }
                    if (i >= (res.Numbers.Count - 1) && (res.LastNumber < res1.LastNumber) && res1.NumEndToken != null) 
                    {
                        res.MinNumber = res.Numbers[res.Numbers.Count - 1];
                        res.Numbers[res.Numbers.Count - 1] = res1.Numbers[res.Numbers.Count - 1];
                        res.NumSuffix = res1.NumSuffix;
                        res.EndToken = (res.NumEndToken = res1.NumEndToken);
                    }
                }
            }
            if (res.Numbers.Count > 0 && res.NumEndToken != null && res.Typ == InstrToken1.Types.Line) 
            {
                Pullenti.Ner.Token tt = res.NumEndToken;
                bool ok = true;
                if (tt.Next != null && tt.Next.IsHiphen) 
                    ok = false;
                else if (!tt.IsWhitespaceAfter) 
                {
                    if (tt.Next != null && ((tt.Next.Chars.IsCapitalUpper || tt.Next.Chars.IsAllUpper || (tt.Next is Pullenti.Ner.ReferentToken)))) 
                    {
                    }
                    else 
                        ok = false;
                }
                if (!ok) 
                {
                    res.Numbers.Clear();
                    res.NumEndToken = (res.NumBeginToken = null);
                }
            }
        }
        static void _parseNumber(Pullenti.Ner.Token t, InstrToken1 res, InstrToken1 prev)
        {
            if (((t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).IntValue != null && (t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) && ((t as Pullenti.Ner.NumberToken).IntValue.Value < 3000)) 
            {
                if (res.Numbers.Count >= 4) 
                {
                }
                if (t.Morph.Class.IsAdjective && res.TypContainerRank == 0) 
                    return;
                Pullenti.Ner.Core.NumberExToken nwp = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(t);
                if (nwp != null) 
                {
                    if (nwp.EndToken.IsWhitespaceBefore) 
                    {
                    }
                    else 
                        return;
                }
                if ((t.Next != null && (t.WhitespacesAfterCount < 3) && t.Next.Chars.IsLetter) && t.Next.Chars.IsAllLower) 
                {
                    if (!t.IsWhitespaceAfter && t.Next.LengthChar == 1) 
                    {
                    }
                    else if (res.Numbers.Count == 0) 
                    {
                        res.NumTyp = NumberTypes.Digit;
                        res.Numbers.Add((t as Pullenti.Ner.NumberToken).Value.ToString());
                        res.NumBeginToken = (res.NumEndToken = (res.EndToken = t));
                        return;
                    }
                    else 
                        return;
                }
                if (res.NumTyp == NumberTypes.Undefined) 
                    res.NumTyp = NumberTypes.Digit;
                else 
                    res.NumTyp = NumberTypes.Combo;
                if (res.Numbers.Count > 0 && t.IsWhitespaceBefore) 
                    return;
                if (res.Numbers.Count == 0) 
                    res.NumBeginToken = t;
                if ((t.Next != null && t.Next.IsHiphen && (t.Next.Next is Pullenti.Ner.NumberToken)) && (t.Next.Next as Pullenti.Ner.NumberToken).IntValue != null && (t.Next.Next as Pullenti.Ner.NumberToken).IntValue.Value > (t as Pullenti.Ner.NumberToken).IntValue.Value) 
                {
                    res.MinNumber = (t as Pullenti.Ner.NumberToken).Value.ToString();
                    t = t.Next.Next;
                }
                else if (((t.Next != null && t.Next.IsCharOf(")") && t.Next.Next != null) && t.Next.Next.IsHiphen && (t.Next.Next.Next is Pullenti.Ner.NumberToken)) && (t.Next.Next.Next as Pullenti.Ner.NumberToken).IntValue != null && (t.Next.Next.Next as Pullenti.Ner.NumberToken).IntValue.Value > (t as Pullenti.Ner.NumberToken).IntValue.Value) 
                {
                    res.MinNumber = (t as Pullenti.Ner.NumberToken).Value.ToString();
                    t = t.Next.Next.Next;
                }
                res.Numbers.Add((t as Pullenti.Ner.NumberToken).Value.ToString());
                res.EndToken = (res.NumEndToken = t);
                res.NumSuffix = null;
                for (Pullenti.Ner.Token ttt = t.Next; ttt != null && (res.Numbers.Count < 4); ttt = ttt.Next) 
                {
                    bool ok1 = false;
                    bool ok2 = false;
                    if ((ttt.IsCharOf("._") && !ttt.IsWhitespaceAfter && (ttt.Next is Pullenti.Ner.NumberToken)) && (((ttt.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit || (((ttt.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Words) && ttt.Next.Chars.IsLatinLetter && !ttt.IsWhitespaceAfter)))) 
                        ok1 = true;
                    else if ((ttt.IsCharOf("(<") && (ttt.Next is Pullenti.Ner.NumberToken) && ttt.Next.Next != null) && ttt.Next.Next.IsCharOf(")>")) 
                        ok2 = true;
                    if (ok1 || ok2) 
                    {
                        ttt = ttt.Next;
                        res.Numbers.Add((ttt as Pullenti.Ner.NumberToken).Value.ToString());
                        res.NumTyp = (res.Numbers.Count == 2 ? NumberTypes.TwoDigits : (res.Numbers.Count == 3 ? NumberTypes.ThreeDigits : NumberTypes.FourDigits));
                        if ((ttt.Next != null && ttt.Next.IsCharOf(")>") && ttt.Next.Next != null) && ttt.Next.Next.IsChar('.')) 
                            ttt = ttt.Next;
                        else if (ok2) 
                            ttt = ttt.Next;
                        t = (res.EndToken = (res.NumEndToken = ttt));
                        continue;
                    }
                    if (((ttt is Pullenti.Ner.TextToken) && ttt.LengthChar == 1 && ttt.Chars.IsLetter) && !ttt.IsWhitespaceBefore && res.Numbers.Count == 1) 
                    {
                        res.Numbers.Add((ttt as Pullenti.Ner.TextToken).Term);
                        res.NumTyp = NumberTypes.Combo;
                        t = (res.EndToken = (res.NumEndToken = ttt));
                        continue;
                    }
                    break;
                }
                if (t.Next != null && t.Next.IsCharOf(").")) 
                {
                    res.NumSuffix = t.Next.GetSourceText();
                    t = (res.EndToken = (res.NumEndToken = t.Next));
                }
                return;
            }
            if (((t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Words && res.TypContainerRank > 0) && res.Numbers.Count == 0) 
            {
                res.Numbers.Add((t as Pullenti.Ner.NumberToken).Value.ToString());
                res.NumTyp = NumberTypes.Digit;
                res.NumBeginToken = t;
                if (t.Next != null && t.Next.IsChar('.')) 
                {
                    t = t.Next;
                    res.NumSuffix = ".";
                }
                res.EndToken = (res.NumEndToken = t);
                return;
            }
            Pullenti.Ner.NumberToken nt = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t);
            if ((nt != null && nt.Value == "10" && t.Next != null) && t.Next.IsChar(')')) 
                nt = null;
            if (nt != null && nt.Value == "100") 
                nt = null;
            if (nt != null && nt.Typ == Pullenti.Ner.NumberSpellingType.Roman) 
            {
                if (res.NumTyp == NumberTypes.Undefined) 
                    res.NumTyp = NumberTypes.Roman;
                else 
                    res.NumTyp = NumberTypes.Combo;
                if (res.Numbers.Count > 0 && t.IsWhitespaceBefore) 
                    return;
                if (res.Numbers.Count == 0) 
                    res.NumBeginToken = t;
                res.Numbers.Add(nt.Value.ToString());
                t = (res.EndToken = (res.NumEndToken = nt.EndToken));
                if (res.NumTyp == NumberTypes.Roman && ((res.Typ == InstrToken1.Types.Chapter || res.Typ == InstrToken1.Types.Section || res.Typ == InstrToken1.Types.Line))) 
                {
                    if ((t.Next != null && t.Next.IsCharOf("._<") && (t.Next.Next is Pullenti.Ner.NumberToken)) && (t.Next.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) 
                    {
                        t = t.Next.Next;
                        res.Numbers.Add((t as Pullenti.Ner.NumberToken).Value.ToString());
                        res.NumTyp = NumberTypes.TwoDigits;
                        if (t.Next != null && t.Next.IsChar('>')) 
                            t = t.Next;
                        res.EndToken = (res.NumEndToken = t);
                        if ((t.Next != null && t.Next.IsCharOf("._<") && (t.Next.Next is Pullenti.Ner.NumberToken)) && (t.Next.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) 
                        {
                            t = t.Next.Next;
                            res.Numbers.Add((t as Pullenti.Ner.NumberToken).Value.ToString());
                            res.NumTyp = NumberTypes.ThreeDigits;
                            if (t.Next != null && t.Next.IsChar('>')) 
                                t = t.Next;
                            res.EndToken = (res.NumEndToken = t);
                        }
                    }
                }
                if (t.Next != null && t.Next.IsCharOf(").")) 
                {
                    res.NumSuffix = t.Next.GetSourceText();
                    t = (res.EndToken = (res.NumEndToken = t.Next));
                }
                return;
            }
            if (((t is Pullenti.Ner.TextToken) && t.LengthChar == 1 && t.Chars.IsLetter) && t == res.BeginToken) 
            {
                if ((!t.IsWhitespaceAfter && (t.Next is Pullenti.Ner.NumberToken) && t.Next.Next != null) && t.Next.Next.IsChar('.')) 
                {
                    res.NumBeginToken = t;
                    res.NumTyp = NumberTypes.Digit;
                    res.Numbers.Add((t.Next as Pullenti.Ner.NumberToken).Value.ToString());
                    res.NumSuffix = (t as Pullenti.Ner.TextToken).Term + ".";
                    t = (res.EndToken = (res.NumEndToken = t.Next.Next));
                    return;
                }
                if (t.Next != null && t.Next.IsCharOf(".)")) 
                {
                    if (((t.Next.IsChar('.') && (t.Next.Next is Pullenti.Ner.NumberToken) && t.Next.Next.Next != null) && t.Next.Next.Next.IsChar(')') && !t.Next.IsWhitespaceAfter) && !t.Next.Next.IsWhitespaceAfter) 
                    {
                        res.NumTyp = NumberTypes.TwoDigits;
                        res.Numbers.Add((t as Pullenti.Ner.TextToken).Term);
                        res.Numbers.Add((t.Next.Next as Pullenti.Ner.NumberToken).Value.ToString());
                        res.NumSuffix = ")";
                        res.NumBeginToken = t;
                        t = (res.EndToken = (res.NumEndToken = t.Next.Next.Next));
                        return;
                    }
                    if (t.Next.IsChar('.') && ((t.Chars.IsAllUpper || (t.Next.Next is Pullenti.Ner.NumberToken)))) 
                    {
                    }
                    else 
                    {
                        InstrToken1 tmp1 = new InstrToken1(t, t.Next);
                        tmp1.Numbers.Add((t as Pullenti.Ner.TextToken).Term);
                        if (tmp1.LastNumber > 1 && t.Next.IsCharOf(".") && ((prev == null || (prev.LastNumber + 1) != tmp1.LastNumber))) 
                        {
                        }
                        else 
                        {
                            if (res.Numbers.Count == 0) 
                                res.NumBeginToken = t;
                            res.NumTyp = NumberTypes.Letter;
                            res.Numbers.Add((t as Pullenti.Ner.TextToken).Term);
                            res.NumBeginToken = t;
                            t = (res.EndToken = (res.NumEndToken = t.Next));
                            res.NumSuffix = t.GetSourceText();
                            return;
                        }
                    }
                }
            }
        }
        public static bool CorrectChildNumbers(FragToken root, List<FragToken> children)
        {
            bool hasNum = false;
            if (root.Number > 0) 
            {
                foreach (FragToken ch in root.Children) 
                {
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Number) 
                    {
                        hasNum = true;
                        break;
                    }
                    else if (ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Keyword) 
                        break;
                }
            }
            if (!hasNum) 
                return false;
            if (root.SubNumber == 0) 
            {
                bool ok = true;
                foreach (FragToken ch in children) 
                {
                    if (ch.Number > 0) 
                    {
                        if (ch.Number == root.Number && ch.SubNumber > 0) 
                        {
                        }
                        else 
                            ok = false;
                    }
                }
                if (ok) 
                {
                    foreach (FragToken ch in children) 
                    {
                        if (ch.Number > 0) 
                        {
                            ch.Number = ch.SubNumber;
                            ch.SubNumber = ch.SubNumber2;
                            ch.SubNumber2 = ch.SubNumber3;
                            ch.SubNumber3 = 0;
                        }
                    }
                }
                return ok;
            }
            if (root.SubNumber > 0 && root.SubNumber2 == 0) 
            {
                bool ok = true;
                foreach (FragToken ch in children) 
                {
                    if (ch.Number > 0) 
                    {
                        if (ch.Number == root.Number && ch.SubNumber == root.SubNumber && ch.SubNumber2 > 0) 
                        {
                        }
                        else 
                            ok = false;
                    }
                }
                if (ok) 
                {
                    foreach (FragToken ch in children) 
                    {
                        if (ch.Number > 0) 
                        {
                            ch.Number = ch.SubNumber2;
                            ch.SubNumber = ch.SubNumber3;
                            ch.SubNumber2 = (ch.SubNumber3 = 0);
                        }
                    }
                }
                return ok;
            }
            if (root.SubNumber > 0 && root.SubNumber2 > 0 && root.SubNumber3 == 0) 
            {
                bool ok = true;
                foreach (FragToken ch in children) 
                {
                    if (ch.Number > 0) 
                    {
                        if ((ch.Number == root.Number && ch.SubNumber == root.SubNumber && ch.SubNumber2 == root.SubNumber2) && ch.SubNumber3 > 0) 
                        {
                        }
                        else 
                            ok = false;
                    }
                }
                if (ok) 
                {
                    foreach (FragToken ch in children) 
                    {
                        if (ch.Number > 0) 
                        {
                            ch.Number = ch.SubNumber3;
                            ch.SubNumber = (ch.SubNumber2 = (ch.SubNumber3 = 0));
                        }
                    }
                }
                return ok;
            }
            return false;
        }
        public static List<string> CreateDiap(string s1, string s2)
        {
            int n1;
            int n2;
            int i;
            string pref = null;
            if (s2.StartsWith(s1)) 
            {
                i = s1.Length;
                if (((i + 1) < s2.Length) && s2[i] == '.' && char.IsDigit(s2[i + 1])) 
                {
                    if (int.TryParse(s2.Substring(i + 1), out n2)) 
                    {
                        List<string> res0 = new List<string>();
                        res0.Add(s1);
                        for (i = 1; i <= n2; i++) 
                        {
                            res0.Add(string.Format("{0}.{1}", s1, i));
                        }
                        return res0;
                    }
                }
            }
            if ((((i = s1.LastIndexOf('.')))) > 0) 
            {
                pref = s1.Substring(0, i + 1);
                if (!int.TryParse(s1.Substring(i + 1), out n1)) 
                    return null;
                if (!s2.StartsWith(pref)) 
                    return null;
                if (!int.TryParse(s2.Substring(i + 1), out n2)) 
                    return null;
            }
            else 
            {
                if (!int.TryParse(s1, out n1)) 
                    return null;
                if (!int.TryParse(s2, out n2)) 
                    return null;
            }
            if (n2 <= n1) 
                return null;
            List<string> res = new List<string>();
            for (i = n1; i <= n2; i++) 
            {
                if (pref == null) 
                    res.Add(i.ToString());
                else 
                    res.Add(pref + (i.ToString()));
            }
            return res;
        }
    }
}