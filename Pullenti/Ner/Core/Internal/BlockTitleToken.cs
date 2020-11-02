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
    public class BlockTitleToken : Pullenti.Ner.MetaToken
    {
        public BlockTitleToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public BlkTyps Typ;
        public string Value;
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Typ, Value ?? "", this.GetSourceText());
        }
        public static List<BlockTitleToken> TryAttachList(Pullenti.Ner.Token t)
        {
            BlockTitleToken content = null;
            BlockTitleToken intro = null;
            List<BlockTitleToken> lits = null;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore) 
                {
                    BlockTitleToken btt = TryAttach(tt, false, null);
                    if (btt == null) 
                        continue;
                    if (btt.Typ == BlkTyps.Index) 
                    {
                        content = btt;
                        break;
                    }
                    if (btt.Typ == BlkTyps.Intro) 
                    {
                        Pullenti.Ner.Token tt2 = btt.EndToken.Next;
                        for (int k = 0; k < 5; k++) 
                        {
                            BlockLine li = BlockLine.Create(tt2, null);
                            if (li == null) 
                                break;
                            if (li.HasContentItemTail || li.Typ == BlkTyps.IndexItem) 
                            {
                                content = btt;
                                break;
                            }
                            if (li.HasVerb) 
                                break;
                            if (li.Typ != BlkTyps.Undefined) 
                            {
                                if ((li.BeginChar - btt.EndChar) < 400) 
                                {
                                    content = btt;
                                    break;
                                }
                            }
                            tt2 = li.EndToken.Next;
                        }
                        if (content == null) 
                            intro = btt;
                        break;
                    }
                    if (btt.Typ == BlkTyps.Literature) 
                    {
                        if (lits == null) 
                            lits = new List<BlockTitleToken>();
                        lits.Add(btt);
                    }
                }
            }
            if (content == null && intro == null && ((lits == null || lits.Count != 1))) 
                return null;
            List<BlockTitleToken> res = new List<BlockTitleToken>();
            Pullenti.Ner.Core.TerminCollection chapterNames = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Token t0 = null;
            if (content != null) 
            {
                res.Add(content);
                int cou = 0;
                int err = 0;
                for (Pullenti.Ner.Token tt = content.EndToken.Next; tt != null; tt = tt.Next) 
                {
                    if (!tt.IsNewlineBefore) 
                        continue;
                    BlockLine li = BlockLine.Create(tt, null);
                    if (li == null) 
                        break;
                    if (li.HasVerb) 
                    {
                        if (li.EndToken.IsChar('.')) 
                            break;
                        if (li.LengthChar > 100) 
                            break;
                    }
                    BlockTitleToken btt = TryAttach(tt, true, null);
                    if (btt == null) 
                        continue;
                    err = 0;
                    if (btt.Typ == BlkTyps.Intro) 
                    {
                        if (content.Typ == BlkTyps.Intro || cou > 2) 
                            break;
                    }
                    cou++;
                    tt = (content.EndToken = btt.EndToken);
                    if (btt.Value != null) 
                        chapterNames.AddString(btt.Value, null, null, false);
                }
                content.Typ = BlkTyps.Index;
                t0 = content.EndToken.Next;
            }
            else if (intro != null) 
                t0 = intro.BeginToken;
            else if (lits != null) 
                t0 = t;
            else 
                return null;
            bool first = true;
            for (Pullenti.Ner.Token tt = t0; tt != null; tt = tt.Next) 
            {
                if (!tt.IsNewlineBefore) 
                    continue;
                if (tt.IsValue("СЛАБОЕ", null)) 
                {
                }
                BlockTitleToken btt = TryAttach(tt, false, chapterNames);
                if (btt == null) 
                    continue;
                if (res.Count == 104) 
                {
                }
                tt = btt.EndToken;
                if (content != null && btt.Typ == BlkTyps.Index) 
                    continue;
                if (res.Count > 0 && res[res.Count - 1].Typ == BlkTyps.Literature) 
                {
                    if (btt.Typ != BlkTyps.Appendix && btt.Typ != BlkTyps.Misc && btt.Typ != BlkTyps.Literature) 
                    {
                        if (btt.Typ == BlkTyps.Chapter && (res[res.Count - 1].EndChar < ((tt.Kit.Sofa.Text.Length * 3) / 4))) 
                        {
                        }
                        else 
                            continue;
                    }
                }
                if (first) 
                {
                    if ((tt.BeginChar - t0.BeginChar) > 300) 
                    {
                        BlockTitleToken btt0 = new BlockTitleToken(t0, (t0.Previous == null ? t0 : t0.Previous));
                        btt0.Typ = BlkTyps.Chapter;
                        btt0.Value = "Похоже на начало";
                        res.Add(btt0);
                    }
                }
                res.Add(btt);
                tt = btt.EndToken;
                first = false;
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (res[i].Typ == BlkTyps.Literature && res[i + 1].Typ == res[i].Typ) 
                {
                    res.RemoveAt(i + 1);
                    i--;
                }
            }
            return res;
        }
        public static BlockTitleToken TryAttach(Pullenti.Ner.Token t, bool isContentItem = false, Pullenti.Ner.Core.TerminCollection names = null)
        {
            if (t == null) 
                return null;
            if (!t.IsNewlineBefore) 
                return null;
            if (t.Chars.IsAllLower) 
                return null;
            BlockLine li = BlockLine.Create(t, names);
            if (li == null) 
                return null;
            if (li.Words == 0 && li.Typ == BlkTyps.Undefined) 
                return null;
            if (li.Typ == BlkTyps.Index) 
            {
            }
            if (li.IsExistName) 
                return new BlockTitleToken(t, li.EndToken) { Typ = li.Typ };
            if (li.EndToken == li.NumberEnd || ((li.EndToken.IsCharOf(".:") && li.EndToken.Previous == li.NumberEnd))) 
            {
                BlockTitleToken res2 = new BlockTitleToken(t, li.EndToken) { Typ = li.Typ };
                if (li.Typ == BlkTyps.Chapter || li.Typ == BlkTyps.Appendix) 
                {
                    BlockLine li2 = BlockLine.Create(li.EndToken.Next, names);
                    if ((li2 != null && li2.Typ == BlkTyps.Undefined && li2.IsAllUpper) && li2.Words > 0) 
                    {
                        res2.EndToken = li2.EndToken;
                        for (Pullenti.Ner.Token tt = res2.EndToken.Next; tt != null; tt = tt.Next) 
                        {
                            li2 = BlockLine.Create(tt, names);
                            if (li2 == null) 
                                break;
                            if (li2.Typ != BlkTyps.Undefined || !li2.IsAllUpper || li2.Words == 0) 
                                break;
                            tt = (res2.EndToken = li2.EndToken);
                        }
                    }
                }
                return res2;
            }
            if (li.NumberEnd == null) 
                return null;
            BlockTitleToken res = new BlockTitleToken(t, li.EndToken) { Typ = li.Typ };
            if (res.Typ == BlkTyps.Undefined) 
            {
                if (li.Words < 1) 
                    return null;
                if (li.HasVerb) 
                    return null;
                if (!isContentItem) 
                {
                    if (!li.IsAllUpper || li.NotWords > (li.Words / 2)) 
                        return null;
                }
                res.Typ = BlkTyps.Chapter;
                if ((li.NumberEnd.EndChar - t.BeginChar) == 7 && li.NumberEnd.Next != null && li.NumberEnd.Next.IsHiphen) 
                    res.Typ = BlkTyps.Undefined;
            }
            if (li.HasContentItemTail && isContentItem) 
                res.Typ = BlkTyps.IndexItem;
            if (res.Typ == BlkTyps.Chapter || res.Typ == BlkTyps.Appendix) 
            {
                if (li.HasVerb) 
                    return null;
                if (li.NotWords > li.Words && !isContentItem) 
                    return null;
                for (t = li.EndToken.Next; t != null; t = t.Next) 
                {
                    BlockLine li2 = BlockLine.Create(t, names);
                    if (li2 == null) 
                        break;
                    if (li2.HasVerb || (li2.Words < 1)) 
                        break;
                    if (!li2.IsAllUpper && !isContentItem) 
                        break;
                    if (li2.Typ != BlkTyps.Undefined || li2.NumberEnd != null) 
                        break;
                    t = (res.EndToken = li2.EndToken);
                    if (isContentItem && li2.HasContentItemTail) 
                    {
                        res.Typ = BlkTyps.IndexItem;
                        break;
                    }
                }
            }
            for (Pullenti.Ner.Token tt = res.EndToken; tt != null && tt.BeginChar > li.NumberEnd.EndChar; tt = tt.Previous) 
            {
                if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter) 
                {
                    res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(li.NumberEnd.Next, tt, Pullenti.Ner.Core.GetTextAttr.No);
                    break;
                }
            }
            if ((res.Typ == BlkTyps.Index || res.Typ == BlkTyps.Intro || res.Typ == BlkTyps.Conslusion) || res.Typ == BlkTyps.Literature) 
            {
                if (res.Value != null && res.Value.Length > 100) 
                    return null;
                if (li.Words < li.NotWords) 
                    return null;
            }
            return res;
        }
    }
}