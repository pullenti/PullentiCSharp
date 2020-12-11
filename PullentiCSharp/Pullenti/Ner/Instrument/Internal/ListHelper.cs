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
    static class ListHelper
    {
        public static void Analyze(FragToken res)
        {
            if (res.Number == 4) 
            {
            }
            if (res.Children.Count == 0) 
            {
                Pullenti.Ner.Instrument.InstrumentKind ki = res.Kind;
                if (((ki == Pullenti.Ner.Instrument.InstrumentKind.Chapter || ki == Pullenti.Ner.Instrument.InstrumentKind.Clause || ki == Pullenti.Ner.Instrument.InstrumentKind.Content) || ki == Pullenti.Ner.Instrument.InstrumentKind.Item || ki == Pullenti.Ner.Instrument.InstrumentKind.Subitem) || ki == Pullenti.Ner.Instrument.InstrumentKind.ClausePart || ki == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                {
                    List<FragToken> tmp = new List<FragToken>();
                    tmp.Add(res);
                    _analizeListItems(tmp, 0);
                }
                return;
            }
            if (res.Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause && res.Number == 12) 
            {
            }
            for (int i = 0; i < res.Children.Count; i++) 
            {
                if (res.Children[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention && ((res.Children[i].EndToken.IsCharOf(":;") || ((((i + 1) < res.Children.Count) && res.Children[i + 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions && res.Children[i + 1].EndToken.IsCharOf(":;")))))) 
                {
                    int j;
                    int cou = 1;
                    char listBullet = (char)0;
                    for (j = i + 1; j < res.Children.Count; j++) 
                    {
                        FragToken ch = res.Children[j];
                        if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                            continue;
                        if (ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                            break;
                        if (ch.EndToken.IsCharOf(";") || ((((j + 1) < res.Children.Count) && res.Children[j + 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions && res.Children[j + 1].EndToken.IsChar(';')))) 
                        {
                            cou++;
                            if ((ch.BeginToken is Pullenti.Ner.TextToken) && !ch.Chars.IsLetter) 
                                listBullet = ch.Kit.GetTextCharacter(ch.BeginChar);
                            continue;
                        }
                        if (ch.EndToken.IsCharOf(".")) 
                        {
                            cou++;
                            j++;
                            break;
                        }
                        if (ch.EndToken.IsCharOf(":")) 
                        {
                            if (listBullet != 0 && ch.BeginToken.IsChar(listBullet)) 
                            {
                                for (Pullenti.Ner.Token tt = ch.BeginToken.Next; tt != null && (tt.EndChar < ch.EndChar); tt = tt.Next) 
                                {
                                    if (tt.Previous.IsChar('.') && Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                                    {
                                        FragToken ch2 = new FragToken(tt, ch.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Indention, Number = ch.Number };
                                        ch.EndToken = tt.Previous;
                                        res.Children.Insert(j + 1, ch2);
                                        for (int k = j + 1; k < res.Children.Count; k++) 
                                        {
                                            if (res.Children[k].Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                                                res.Children[k].Number++;
                                        }
                                        cou++;
                                        j++;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                        cou++;
                        j++;
                        break;
                    }
                    if (cou < 3) 
                    {
                        i = j;
                        continue;
                    }
                    if ((i > 0 && !res.Children[i].EndToken.IsChar(':') && res.Children[i - 1].Kind2 == Pullenti.Ner.Instrument.InstrumentKind.Undefined) && res.Children[i - 1].EndToken.IsChar(':')) 
                        res.Children[i - 1].Kind2 = Pullenti.Ner.Instrument.InstrumentKind.ListHead;
                    for (; i < j; i++) 
                    {
                        FragToken ch = res.Children[i];
                        if (ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                            continue;
                        if (ch.EndToken.IsChar(':')) 
                            ch.Kind2 = Pullenti.Ner.Instrument.InstrumentKind.ListHead;
                        else if (((i + 1) < j) && res.Children[i + 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions && res.Children[i + 1].EndToken.IsChar(':')) 
                            ch.Kind2 = Pullenti.Ner.Instrument.InstrumentKind.ListHead;
                        else 
                            ch.Kind2 = Pullenti.Ner.Instrument.InstrumentKind.ListItem;
                    }
                }
            }
            List<FragToken> changed = new List<FragToken>();
            for (int i = 0; i < res.Children.Count; i++) 
            {
                if (res.Number == 7) 
                {
                }
                if (res.Children[i].Children.Count > 0) 
                    Analyze(res.Children[i]);
                else 
                {
                    int co = _analizeListItems(res.Children, i);
                    if (co > 0) 
                    {
                        changed.Add(res.Children[i]);
                        if (co > 1) 
                            res.Children.RemoveRange(i + 1, co - 1);
                        i += (co - 1);
                    }
                }
            }
            for (int i = changed.Count - 1; i >= 0; i--) 
            {
                if (changed[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
                {
                    int j = res.Children.IndexOf(changed[i]);
                    if (j < 0) 
                        continue;
                    res.Children.RemoveAt(j);
                    res.Children.InsertRange(j, changed[i].Children);
                }
            }
        }
        static int _analizeListItems(List<FragToken> chi, int ind)
        {
            if (ind >= chi.Count) 
                return -1;
            FragToken res = chi[ind];
            Pullenti.Ner.Instrument.InstrumentKind ki = res.Kind;
            if (((ki == Pullenti.Ner.Instrument.InstrumentKind.Chapter || ki == Pullenti.Ner.Instrument.InstrumentKind.Clause || ki == Pullenti.Ner.Instrument.InstrumentKind.Content) || ki == Pullenti.Ner.Instrument.InstrumentKind.Item || ki == Pullenti.Ner.Instrument.InstrumentKind.Subitem) || ki == Pullenti.Ner.Instrument.InstrumentKind.ClausePart || ki == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
            {
            }
            else 
                return -1;
            if (res.HasChanges && res.MultilineChangesValue != null) 
            {
                Pullenti.Ner.MetaToken ci = res.MultilineChangesValue;
                FragToken cit = new FragToken(ci.BeginToken, ci.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Citation };
                res.Children.Add(cit);
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(cit.BeginToken.Previous, true)) 
                    cit.BeginToken = cit.BeginToken.Previous;
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(cit.EndToken.Next, true)) 
                {
                    cit.EndToken = cit.EndToken.Next;
                    if (cit.EndToken.Next != null && cit.EndToken.Next.IsCharOf(";.")) 
                        cit.EndToken = cit.EndToken.Next;
                }
                res.FillByContentChildren();
                if (res.Children[0].HasChanges) 
                {
                }
                Pullenti.Ner.Instrument.InstrumentKind citKind = Pullenti.Ner.Instrument.InstrumentKind.Undefined;
                if (ci.Tag is Pullenti.Ner.Decree.DecreeChangeReferent) 
                {
                    Pullenti.Ner.Decree.DecreeChangeReferent dcr = ci.Tag as Pullenti.Ner.Decree.DecreeChangeReferent;
                    if (dcr.Value != null && dcr.Value.NewItems.Count > 0) 
                    {
                        string mnem = dcr.Value.NewItems[0];
                        int i;
                        if ((((i = mnem.IndexOf(' ')))) > 0) 
                            mnem = mnem.Substring(0, i);
                        citKind = Pullenti.Ner.Decree.Internal.PartToken._getInstrKindByTyp(Pullenti.Ner.Decree.Internal.PartToken._getTypeByAttrName(mnem));
                    }
                    else if (dcr.Owners.Count > 0 && (dcr.Owners[0] is Pullenti.Ner.Decree.DecreePartReferent) && dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.New) 
                    {
                        Pullenti.Ner.Decree.DecreePartReferent pat = dcr.Owners[0] as Pullenti.Ner.Decree.DecreePartReferent;
                        int min = 0;
                        foreach (Pullenti.Ner.Slot s in pat.Slots) 
                        {
                            Pullenti.Ner.Decree.Internal.PartToken.ItemType ty = Pullenti.Ner.Decree.Internal.PartToken._getTypeByAttrName(s.TypeName);
                            if (ty == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Undefined) 
                                continue;
                            int l = Pullenti.Ner.Decree.Internal.PartToken._getRank(ty);
                            if (l == 0) 
                                continue;
                            if (l > min || min == 0) 
                            {
                                min = l;
                                citKind = Pullenti.Ner.Decree.Internal.PartToken._getInstrKindByTyp(ty);
                            }
                        }
                    }
                }
                FragToken sub = null;
                if (citKind != Pullenti.Ner.Instrument.InstrumentKind.Undefined && citKind != Pullenti.Ner.Instrument.InstrumentKind.Appendix) 
                {
                    sub = new FragToken(ci.BeginToken, ci.EndToken);
                    ContentAnalyzeWhapper wr = new ContentAnalyzeWhapper();
                    wr.Analyze(sub, null, true, citKind);
                    sub.Kind = Pullenti.Ner.Instrument.InstrumentKind.Content;
                }
                else 
                    sub = FragToken.CreateDocument(ci.BeginToken, ci.EndChar, citKind);
                if (sub == null || sub.Children.Count == 0) 
                {
                }
                else if ((sub.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content && sub.Children.Count > 0 && sub.Children[0].BeginToken == sub.BeginToken) && sub.Children[sub.Children.Count - 1].EndToken == sub.EndToken) 
                    cit.Children.AddRange(sub.Children);
                else 
                    cit.Children.Add(sub);
                return 1;
            }
            int endChar = res.EndChar;
            if (res.Itok == null) 
                res.Itok = InstrToken1.Parse(res.BeginToken, true, null, 0, null, false, res.EndChar, false, false);
            List<LineToken> lines = LineToken.ParseList(res.BeginToken, endChar, null);
            if (lines == null || (lines.Count < 1)) 
                return -1;
            int ret = 1;
            if (res.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
            {
                for (int j = ind + 1; j < chi.Count; j++) 
                {
                    if (chi[j].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
                    {
                        List<LineToken> lines2 = LineToken.ParseList(chi[j].BeginToken, chi[j].EndChar, lines[lines.Count - 1]);
                        if (lines2 == null || (lines2.Count < 1)) 
                            break;
                        if (!lines2[0].IsListItem) 
                        {
                            if ((lines2.Count > 1 && lines2[1].IsListItem && lines2[0].EndToken.IsCharOf(":")) && !lines2[0].BeginToken.Chars.IsCapitalUpper) 
                                lines2[0].IsListItem = true;
                            else 
                                break;
                        }
                        lines.AddRange(lines2);
                        ret = (j - ind) + 1;
                    }
                    else if (chi[j].Kind != Pullenti.Ner.Instrument.InstrumentKind.Editions && chi[j].Kind != Pullenti.Ner.Instrument.InstrumentKind.Comment) 
                        break;
                }
            }
            if (lines.Count < 2) 
                return -1;
            if ((lines.Count > 1 && lines[0].IsListItem && lines[1].IsListItem) && lines[0].Number != 1) 
            {
                if (lines.Count == 2 || !lines[2].IsListItem) 
                    lines[0].IsListItem = (lines[1].IsListItem = false);
            }
            for (int i = 0; i < lines.Count; i++) 
            {
                if (lines[i].IsListItem) 
                {
                    if (i > 0 && lines[i - 1].IsListItem) 
                        continue;
                    if (((i + 1) < lines.Count) && lines[i + 1].IsListItem) 
                    {
                    }
                    else 
                    {
                        lines[i].IsListItem = false;
                        continue;
                    }
                    int j;
                    bool newLine = false;
                    for (j = i + 1; j < lines.Count; j++) 
                    {
                        if (!lines[j].IsListItem) 
                            break;
                        else if (lines[j].IsNewlineBefore) 
                            newLine = true;
                    }
                    if (newLine) 
                        continue;
                    if (i > 0 && lines[i - 1].EndToken.IsChar(':')) 
                        continue;
                    for (j = i; j < lines.Count; j++) 
                    {
                        if (!lines[j].IsListItem) 
                            break;
                        else 
                            lines[j].IsListItem = false;
                    }
                }
            }
            if (lines.Count > 2) 
            {
                LineToken last = lines[lines.Count - 1];
                LineToken last2 = lines[lines.Count - 2];
                if ((!last.IsListItem && last.EndToken.IsChar('.') && last2.IsListItem) && last2.EndToken.IsChar(';')) 
                {
                    if ((last.LengthChar < (last2.LengthChar * 2)) || last.BeginToken.Chars.IsAllLower) 
                        last.IsListItem = true;
                }
            }
            for (int i = 0; i < (lines.Count - 1); i++) 
            {
                if (!lines[i].IsListItem && !lines[i + 1].IsListItem) 
                {
                    if (((i + 2) < lines.Count) && lines[i + 2].IsListItem && lines[i + 1].EndToken.IsChar(':')) 
                    {
                    }
                    else 
                    {
                        lines[i].EndToken = lines[i + 1].EndToken;
                        lines.RemoveAt(i + 1);
                        i--;
                    }
                }
            }
            for (int i = 0; i < (lines.Count - 1); i++) 
            {
                if (lines[i].IsListItem) 
                {
                    if (lines[i].Number == 1) 
                    {
                        bool ok = true;
                        int num = 1;
                        int nonum = 0;
                        for (int j = i + 1; j < lines.Count; j++) 
                        {
                            if (!lines[j].IsListItem) 
                            {
                                ok = false;
                                break;
                            }
                            else if (lines[j].Number > 0) 
                            {
                                num++;
                                if (lines[j].Number != num) 
                                {
                                    ok = false;
                                    break;
                                }
                            }
                            else 
                                nonum++;
                        }
                        if (!ok || nonum == 0 || (num < 2)) 
                            break;
                        LineToken lt = lines[i];
                        for (int j = i + 1; j < lines.Count; j++) 
                        {
                            if (lines[j].Number > 0) 
                                lt = lines[j];
                            else 
                            {
                                List<LineToken> chli = lt.Tag as List<LineToken>;
                                if (chli == null) 
                                    lt.Tag = (chli = new List<LineToken>());
                                lt.EndToken = lines[j].EndToken;
                                chli.Add(lines[j]);
                                lines.RemoveAt(j);
                                j--;
                            }
                        }
                    }
                }
            }
            int cou = 0;
            foreach (LineToken li in lines) 
            {
                if (li.IsListItem) 
                    cou++;
            }
            if (cou < 2) 
                return -1;
            for (int i = 0; i < lines.Count; i++) 
            {
                if (lines[i].IsListItem) 
                {
                    int i0 = i;
                    bool ok = true;
                    cou = 1;
                    for (; i < lines.Count; i++,cou++) 
                    {
                        if (!lines[i].IsListItem) 
                            break;
                        else if (lines[i].Number != cou) 
                            ok = false;
                    }
                    if (!ok) 
                    {
                        for (i = i0; i < lines.Count; i++) 
                        {
                            if (!lines[i].IsListItem) 
                                break;
                            else 
                                lines[i].Number = 0;
                        }
                    }
                    if (cou > 3 && lines[i0].BeginToken.GetSourceText() != lines[i0 + 1].BeginToken.GetSourceText() && lines[i0 + 1].BeginToken.GetSourceText() == lines[i0 + 2].BeginToken.GetSourceText()) 
                    {
                        string pref = lines[i0 + 1].BeginToken.GetSourceText();
                        ok = true;
                        for (int j = i0 + 2; j < i; j++) 
                        {
                            if (pref != lines[j].BeginToken.GetSourceText()) 
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (!ok) 
                            continue;
                        Pullenti.Ner.Token tt = null;
                        ok = false;
                        for (tt = lines[i0].EndToken.Previous; tt != null && tt != lines[i0].BeginToken; tt = tt.Previous) 
                        {
                            if (tt.GetSourceText() == pref) 
                            {
                                ok = true;
                                break;
                            }
                        }
                        if (ok) 
                        {
                            LineToken li0 = new LineToken(lines[i0].BeginToken, tt.Previous);
                            lines[i0].BeginToken = tt;
                            lines.Insert(i0, li0);
                            i++;
                        }
                    }
                }
            }
            foreach (LineToken li in lines) 
            {
                li.CorrectBeginToken();
                FragToken ch = new FragToken(li.BeginToken, li.EndToken) { Kind = (li.IsListItem ? Pullenti.Ner.Instrument.InstrumentKind.ListItem : Pullenti.Ner.Instrument.InstrumentKind.Content), Number = li.Number };
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content && ch.EndToken.IsChar(':')) 
                    ch.Kind = Pullenti.Ner.Instrument.InstrumentKind.ListHead;
                res.Children.Add(ch);
                List<LineToken> chli = li.Tag as List<LineToken>;
                if (chli != null) 
                {
                    foreach (LineToken lt in chli) 
                    {
                        ch.Children.Add(new FragToken(lt.BeginToken, lt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.ListItem });
                    }
                    if (ch.BeginChar < ch.Children[0].BeginChar) 
                        ch.Children.Insert(0, new FragToken(ch.BeginToken, ch.Children[0].BeginToken.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
                }
            }
            return ret;
        }
        class LineToken : Pullenti.Ner.MetaToken
        {
            public LineToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
            {
            }
            public bool IsListItem;
            public bool IsListHead;
            public int Number;
            public void CorrectBeginToken()
            {
                if (!IsListItem) 
                    return;
                if (BeginToken.IsHiphen && BeginToken.Next != null) 
                    BeginToken = BeginToken.Next;
                else if ((Number > 0 && BeginToken.Next != null && BeginToken.Next.IsChar(')')) && BeginToken.Next.Next != null) 
                    BeginToken = BeginToken.Next.Next;
            }
            public override string ToString()
            {
                return string.Format("{0}: {1}", (IsListItem ? "LISTITEM" : "TEXT"), this.GetSourceText());
            }
            public static LineToken Parse(Pullenti.Ner.Token t, int maxChar, LineToken prev)
            {
                if (t == null || t.EndChar > maxChar) 
                    return null;
                LineToken res = new LineToken(t, t);
                for (; t != null && t.EndChar <= maxChar; t = t.Next) 
                {
                    if (t.IsChar(':')) 
                    {
                        if (res.IsNewlineBefore && res.BeginToken.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
                            res.IsListHead = true;
                        res.EndToken = t;
                        break;
                    }
                    if (t.IsChar(';')) 
                    {
                        if (!t.IsWhitespaceAfter) 
                        {
                        }
                        if (t.Previous != null && (t.Previous.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                        {
                            if (!t.IsWhitespaceAfter) 
                                continue;
                            if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                                continue;
                        }
                        res.IsListItem = true;
                        res.EndToken = t;
                        break;
                    }
                    if (t.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            res.EndToken = (t = br.EndToken);
                            continue;
                        }
                    }
                    if (t.IsNewlineBefore && t != res.BeginToken) 
                    {
                        bool next = true;
                        if (t.Previous.IsComma || t.Previous.IsAnd || t.IsCharOf("(")) 
                            next = false;
                        else if (t.Chars.IsLetter || (t is Pullenti.Ner.NumberToken)) 
                        {
                            if (t.Chars.IsAllLower) 
                                next = false;
                            else if (t.Previous.Chars.IsLetter) 
                                next = false;
                        }
                        if (next) 
                            break;
                    }
                    res.EndToken = t;
                }
                if (res.BeginToken.IsHiphen) 
                    res.IsListItem = res.BeginToken.Next != null && !res.BeginToken.Next.IsHiphen;
                else if (res.BeginToken.IsCharOf("·")) 
                {
                    res.IsListItem = true;
                    res.BeginToken = res.BeginToken.Next;
                }
                else if (res.BeginToken.Next != null && ((res.BeginToken.Next.IsChar(')') || ((prev != null && ((prev.IsListItem || prev.IsListHead))))))) 
                {
                    if (res.BeginToken.LengthChar == 1 || (res.BeginToken is Pullenti.Ner.NumberToken)) 
                    {
                        res.IsListItem = true;
                        if ((res.BeginToken is Pullenti.Ner.NumberToken) && (res.BeginToken as Pullenti.Ner.NumberToken).IntValue != null) 
                            res.Number = (res.BeginToken as Pullenti.Ner.NumberToken).IntValue.Value;
                        else if ((res.BeginToken is Pullenti.Ner.TextToken) && res.BeginToken.LengthChar == 1) 
                        {
                            string te = (res.BeginToken as Pullenti.Ner.TextToken).Term;
                            if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(te[0])) 
                                res.Number = ((int)te[0]) - ((int)'А');
                            else if (Pullenti.Morph.LanguageHelper.IsLatinChar(te[0])) 
                                res.Number = ((int)te[0]) - ((int)'A');
                        }
                    }
                }
                return res;
            }
            public static List<LineToken> ParseList(Pullenti.Ner.Token t, int maxChar, LineToken prev)
            {
                LineToken lt = Parse(t, maxChar, prev);
                if (lt == null) 
                    return null;
                List<LineToken> res = new List<LineToken>();
                res.Add(lt);
                string ss = lt.ToString();
                for (t = lt.EndToken.Next; t != null; t = t.Next) 
                {
                    LineToken lt0 = Parse(t, maxChar, lt);
                    if (lt0 == null) 
                        break;
                    res.Add((lt = lt0));
                    t = lt0.EndToken;
                }
                if ((res.Count < 2) && !res[0].IsListItem) 
                {
                    if ((prev != null && prev.IsListItem && res[0].EndToken.IsChar('.')) && !res[0].BeginToken.Chars.IsCapitalUpper) 
                    {
                        res[0].IsListItem = true;
                        return res;
                    }
                    return null;
                }
                int i;
                for (i = 0; i < res.Count; i++) 
                {
                    if (res[i].IsListItem) 
                        break;
                }
                if (i >= res.Count) 
                    return null;
                int j;
                int cou = 0;
                for (j = i; j < res.Count; j++) 
                {
                    if (!res[j].IsListItem) 
                    {
                        if (res[j - 1].IsListItem && res[j].EndToken.IsChar('.')) 
                        {
                            if (res[j].BeginToken.GetSourceText() == res[i].BeginToken.GetSourceText() || res[j].BeginToken.Chars.IsAllLower) 
                            {
                                res[j].IsListItem = true;
                                j++;
                                cou++;
                            }
                        }
                    }
                    else 
                        cou++;
                }
                return res;
            }
        }

        public static void CorrectAppList(List<InstrToken1> lines)
        {
            for (int i = 0; i < (lines.Count - 1); i++) 
            {
                if ((lines[i].Typ == InstrToken1.Types.Line && lines[i].Numbers.Count == 0 && lines[i].BeginToken.IsValue("ПРИЛОЖЕНИЯ", "ДОДАТОК")) && lines[i + 1].Numbers.Count > 0 && lines[i].EndToken.IsChar(':')) 
                {
                    int num = 1;
                    for (++i; i < lines.Count; i++) 
                    {
                        if (lines[i].Numbers.Count == 0) 
                        {
                                {
                                    if (((i + 1) < lines.Count) && lines[i + 1].Numbers.Count == 1 && lines[i + 1].Numbers[0] == num.ToString()) 
                                    {
                                        lines[i - 1].EndToken = lines[i].EndToken;
                                        lines.RemoveAt(i);
                                        i--;
                                        continue;
                                    }
                                }
                            break;
                        }
                        else 
                        {
                            int nn;
                            if (int.TryParse(lines[i].Numbers[0], out nn)) 
                                num = nn + 1;
                            lines[i].NumTyp = NumberTypes.Undefined;
                            lines[i].Numbers.Clear();
                        }
                    }
                }
            }
        }
        public static void CorrectIndex(List<InstrToken1> lines)
        {
            if (lines.Count < 10) 
                return;
            if (lines[0].Typ == InstrToken1.Types.Clause || lines[0].Typ == InstrToken1.Types.Chapter) 
            {
            }
            else 
                return;
            List<InstrToken1> index = new List<InstrToken1>();
            index.Add(lines[0]);
            List<InstrToken1> content = new List<InstrToken1>();
            int i;
            int indText = 0;
            int conText = 0;
            for (i = 1; i < lines.Count; i++) 
            {
                if (lines[i].Typ == lines[0].Typ) 
                {
                    if (_canBeEquals(lines[i], lines[0])) 
                        break;
                    else 
                        index.Add(lines[i]);
                }
                else 
                    indText += lines[i].LengthChar;
            }
            int cInd = i;
            for (; i < lines.Count; i++) 
            {
                if (lines[i].Typ == lines[0].Typ) 
                    content.Add(lines[i]);
                else 
                    conText += lines[i].LengthChar;
            }
            if (index.Count == content.Count && index.Count > 2) 
            {
                if ((indText * 10) < conText) 
                {
                    lines[0] = new InstrToken1(lines[0].BeginToken, lines[cInd - 1].EndToken) { IndexNoKeyword = true, Typ = InstrToken1.Types.Index };
                    lines.RemoveRange(1, cInd - 1);
                }
            }
        }
        static bool _canBeEquals(InstrToken1 i1, InstrToken1 i2)
        {
            if (i1.Typ != i2.Typ) 
                return false;
            if (i1.Numbers.Count > 0 && i2.Numbers.Count > 0) 
            {
                if (i1.Numbers.Count != i2.Numbers.Count) 
                    return false;
                for (int i = 0; i < i1.Numbers.Count; i++) 
                {
                    if (i1.Numbers[i] != i2.Numbers[i]) 
                        return false;
                }
            }
            if (!Pullenti.Ner.Core.MiscHelper.CanBeEqualsEx(i1.Value, i2.Value, Pullenti.Ner.Core.CanBeEqualsAttr.IgnoreNonletters | Pullenti.Ner.Core.CanBeEqualsAttr.IgnoreUppercase)) 
                return false;
            return true;
        }
    }
}