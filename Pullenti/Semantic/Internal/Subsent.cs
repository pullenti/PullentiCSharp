/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Semantic.Internal
{
    class Subsent
    {
        public Subsent Owner;
        public Subsent OwnerRoot
        {
            get
            {
                int k = 0;
                for (Subsent s = this.Owner; s != null && (k < 100); s = Owner,k++) 
                {
                    if (s.Owner == null) 
                        return s;
                }
                return null;
            }
        }
        public List<SentItem> Items = new List<SentItem>();
        public List<Pullenti.Ner.MetaToken> Delims = new List<Pullenti.Ner.MetaToken>();
        public Pullenti.Semantic.SemFragment ResFrag;
        public bool IsOr;
        public bool Check(DelimType typ)
        {
            foreach (Pullenti.Ner.MetaToken d in Delims) 
            {
                if ((d is DelimToken) && (((d as DelimToken).Typ & typ)) != DelimType.Undefined) 
                    return true;
                else if ((d is Pullenti.Ner.Core.ConjunctionToken) && typ == DelimType.And) 
                    return true;
            }
            return false;
        }
        public bool CheckOr()
        {
            foreach (Pullenti.Ner.MetaToken d in Delims) 
            {
                if ((d is Pullenti.Ner.Core.ConjunctionToken) && (d as Pullenti.Ner.Core.ConjunctionToken).Typ == Pullenti.Ner.Core.ConjunctionType.Or) 
                    return true;
            }
            return false;
        }
        public bool OnlyConj()
        {
            foreach (Pullenti.Ner.MetaToken d in Delims) 
            {
                if (d is DelimToken) 
                    return false;
            }
            return true;
        }
        public bool CanBeNextInList(Subsent next)
        {
            if (next.Delims.Count == 0) 
                return true;
            foreach (Pullenti.Ner.MetaToken d in next.Delims) 
            {
                if (d is DelimToken) 
                {
                    if (!this.Check((d as DelimToken).Typ)) 
                        return false;
                }
            }
            return true;
        }
        public string Question;
        public bool IsThenElseRoot = false;
        public Pullenti.Semantic.SemFraglinkType Typ = Pullenti.Semantic.SemFraglinkType.Undefined;
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            if (IsOr) 
                tmp.Append("OR ");
            if (Question != null) 
                tmp.AppendFormat("({0}?) ", Question);
            foreach (Pullenti.Ner.MetaToken it in Delims) 
            {
                tmp.AppendFormat("<{0}> ", it);
            }
            tmp.Append('[');
            foreach (SentItem it in Items) 
            {
                if (it != Items[0]) 
                    tmp.Append(", ");
                tmp.Append(it);
            }
            tmp.Append("]");
            if (Owner != null) 
                tmp.AppendFormat(" -> {0}", Owner);
            return tmp.ToString();
        }
        bool hasCommaAnd(int b, int e)
        {
            foreach (SentItem it in Items) 
            {
                if (it.Typ == SentItemType.Conj) 
                {
                    if (it.Source.BeginToken.BeginChar >= b && it.Source.EndToken.EndChar <= e) 
                        return true;
                }
            }
            return false;
        }
        public static List<Subsent> CreateSubsents(Sentence sent)
        {
            if (sent.Items.Count == 0) 
                return null;
            List<Subsent> res = new List<Subsent>();
            int begin = sent.Items[0].BeginToken.BeginChar;
            int end = sent.Items[sent.Items.Count - 1].EndToken.EndChar;
            byte[] map = new byte[(end + 1) - begin];
            if (sent.BestVar != null) 
            {
                foreach (NGSegmentVariant seg in sent.BestVar.Segs) 
                {
                    if (seg != null) 
                    {
                        foreach (NGLink li in seg.Links) 
                        {
                            if (li != null && li.Typ == NGLinkType.List) 
                            {
                                for (int i = (li.To == null ? li.ToVerb.BeginChar : li.To.Source.BeginToken.BeginChar); i <= li.From.Source.EndToken.EndChar; i++) 
                                {
                                    int po = i - begin;
                                    if (po >= 0 && (po < map.Length)) 
                                        map[po] = 1;
                                }
                            }
                        }
                    }
                }
            }
            Subsent ss = new Subsent();
            bool hasVerb = false;
            for (int i = 0; i < sent.Items.Count; i++) 
            {
                SentItem it = sent.Items[i];
                bool delim = false;
                if (it.Typ == SentItemType.Delim) 
                    delim = true;
                else if (it.Typ == SentItemType.Conj && map[it.BeginToken.BeginChar - begin] == 0) 
                {
                    delim = true;
                    if ((it.Source as Pullenti.Ner.Core.ConjunctionToken).Typ == Pullenti.Ner.Core.ConjunctionType.Comma) 
                    {
                        if (!hasVerb) 
                            delim = false;
                    }
                }
                if (!delim) 
                {
                    if (it.Typ == SentItemType.Verb) 
                        hasVerb = true;
                    ss.Items.Add(it);
                    continue;
                }
                if (ss.Items.Count == 0) 
                {
                    ss.Delims.Add(it.Source);
                    continue;
                }
                if (ss.Items.Count > 0) 
                    res.Add(ss);
                ss = new Subsent();
                hasVerb = false;
                ss.Delims.Add(it.Source);
            }
            if (ss.Items.Count > 0) 
                res.Add(ss);
            for (int i = 0; i < res.Count; i++) 
            {
                Subsent r = res[i];
                int j;
                if (r.Check(DelimType.If)) 
                {
                    bool hasThen = false;
                    bool hasElse = false;
                    for (j = i + 1; j < res.Count; j++) 
                    {
                        if (res[j].Check(DelimType.Then)) 
                        {
                            if (hasThen) 
                                break;
                            res[j].Owner = r;
                            res[j].Question = "если";
                            res[j].Typ = Pullenti.Semantic.SemFraglinkType.IfThen;
                            hasThen = true;
                            r.IsThenElseRoot = true;
                        }
                        else if (res[j].Check(DelimType.Else)) 
                        {
                            if (hasElse) 
                                break;
                            res[j].Owner = r;
                            res[j].Question = "иначе";
                            res[j].Typ = Pullenti.Semantic.SemFraglinkType.IfElse;
                            hasElse = true;
                            r.IsThenElseRoot = true;
                        }
                        else if (res[j].Check(DelimType.If)) 
                        {
                            if (res[j].Check(DelimType.And)) 
                                res[j].Owner = r;
                            else 
                                break;
                        }
                    }
                    if (!hasThen && i > 0) 
                    {
                        if (res[0].Owner == null && res[0].OnlyConj()) 
                        {
                            res[0].Owner = r;
                            res[0].Question = "если";
                            r.IsThenElseRoot = true;
                            res[0].Typ = Pullenti.Semantic.SemFraglinkType.IfThen;
                        }
                        else if (res[0].Owner != null) 
                        {
                            r.Owner = res[0];
                            r.Question = "если";
                            r.Typ = Pullenti.Semantic.SemFraglinkType.IfThen;
                        }
                    }
                    continue;
                }
                if (r.Check(DelimType.Because)) 
                {
                    bool hasThen = false;
                    for (j = i + 1; j < res.Count; j++) 
                    {
                        if (res[j].Check(DelimType.Then)) 
                        {
                            if (hasThen) 
                                break;
                            res[j].Owner = r;
                            res[j].Question = "по причине";
                            res[j].Typ = Pullenti.Semantic.SemFraglinkType.Because;
                            hasThen = true;
                            r.IsThenElseRoot = true;
                        }
                    }
                    if (!hasThen && i > 0) 
                    {
                        if (res[0].Owner == null && res[0].OnlyConj()) 
                        {
                            res[0].Owner = r;
                            res[0].Question = "по причине";
                            r.IsThenElseRoot = true;
                            res[0].Typ = Pullenti.Semantic.SemFraglinkType.Because;
                            continue;
                        }
                    }
                    if (!hasThen && ((i + 1) < res.Count)) 
                    {
                        if (res[i + 1].Owner == null && res[i + 1].OnlyConj()) 
                        {
                            res[i + 1].Owner = r;
                            res[i + 1].Question = "по причине";
                            r.IsThenElseRoot = true;
                            res[i + 1].Typ = Pullenti.Semantic.SemFraglinkType.Because;
                            continue;
                        }
                    }
                    continue;
                }
                if (r.Check(DelimType.But)) 
                {
                    if (i > 0) 
                    {
                        if (res[i - 1].Owner == null && res[i - 1].OnlyConj()) 
                        {
                            res[i - 1].Owner = r;
                            res[i - 1].Question = "но";
                            r.IsThenElseRoot = true;
                            res[i - 1].Typ = Pullenti.Semantic.SemFraglinkType.But;
                            continue;
                        }
                    }
                }
                if (r.Check(DelimType.What)) 
                {
                    if (i > 0) 
                    {
                        if (res[i - 1].Owner == null && res[i - 1].OnlyConj()) 
                        {
                            res[i - 1].Owner = r;
                            res[i - 1].Question = "что";
                            r.IsThenElseRoot = true;
                            res[i - 1].Typ = Pullenti.Semantic.SemFraglinkType.What;
                            continue;
                        }
                    }
                }
                if (r.Check(DelimType.For)) 
                {
                    if ((i + 1) < res.Count) 
                    {
                        if (res[i + 1].Owner == null && res[i + 1].OnlyConj()) 
                        {
                            res[i + 1].Owner = r;
                            res[i + 1].Question = "чтобы";
                            r.IsThenElseRoot = true;
                            res[i + 1].Typ = Pullenti.Semantic.SemFraglinkType.For;
                            continue;
                        }
                    }
                    if (i > 0) 
                    {
                        if (res[i - 1].Owner == null && res[i - 1].OnlyConj()) 
                        {
                            res[i - 1].Owner = r;
                            res[i - 1].Question = "чтобы";
                            r.IsThenElseRoot = true;
                            res[i - 1].Typ = Pullenti.Semantic.SemFraglinkType.For;
                            continue;
                        }
                    }
                }
            }
            for (int i = 1; i < res.Count; i++) 
            {
                Subsent r = res[i];
                if (!r.Check(DelimType.And) || r.Owner != null) 
                    continue;
                for (int j = i - 1; j >= 0; j--) 
                {
                    Subsent rr = res[j];
                    if (rr.CanBeNextInList(r) && ((rr.Owner == null || ((rr.OwnerRoot != null && rr.OwnerRoot.CanBeNextInList(r)))))) 
                    {
                        if (r.CheckOr()) 
                            rr.IsOr = true;
                        rr.Items.AddRange(r.Items);
                        res.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            return res;
        }
    }
}