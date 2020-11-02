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
    class NGSegment
    {
        public Pullenti.Ner.Core.VerbPhraseToken BeforeVerb;
        public List<NGItem> Items = new List<NGItem>();
        public Pullenti.Ner.Core.VerbPhraseToken AfterVerb;
        public List<NGSegmentVariant> Variants = new List<NGSegmentVariant>();
        public int Ind;
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            if (BeforeVerb != null) 
                tmp.AppendFormat("<{0}>: ", BeforeVerb.ToString());
            foreach (NGItem it in Items) 
            {
                if (it != Items[0]) 
                    tmp.Append("; \r\n");
                tmp.Append(it.ToString());
            }
            if (AfterVerb != null) 
                tmp.AppendFormat(" :<{0}>", AfterVerb.ToString());
            return tmp.ToString();
        }
        public static List<NGSegment> CreateSegments(Sentence s)
        {
            List<NGSegment> res = new List<NGSegment>();
            for (int i = 0; i < s.Items.Count; i++) 
            {
                SentItem it = s.Items[i];
                if (it.Typ == SentItemType.Verb || it.Typ == SentItemType.Delim) 
                    continue;
                NGSegment seg = new NGSegment();
                NGItem nit = new NGItem() { Source = it };
                for (int j = i - 1; j >= 0; j--) 
                {
                    it = s.Items[j];
                    if (it.Typ == SentItemType.Verb) 
                    {
                        seg.BeforeVerb = it.Source as Pullenti.Ner.Core.VerbPhraseToken;
                        break;
                    }
                    if (it.Typ == SentItemType.Delim) 
                        break;
                    if (it.CanBeCommaEnd) 
                    {
                        if ((it.Source as Pullenti.Ner.Core.ConjunctionToken).Typ == Pullenti.Ner.Core.ConjunctionType.Comma) 
                            nit.CommaBefore = true;
                        else 
                        {
                            nit.AndBefore = true;
                            if ((it.Source as Pullenti.Ner.Core.ConjunctionToken).Typ == Pullenti.Ner.Core.ConjunctionType.Or) 
                                nit.OrBefore = true;
                        }
                    }
                    if (it.Typ == SentItemType.Conj || it.CanBeNoun) 
                        break;
                }
                bool comma = false;
                bool and = false;
                bool or = false;
                for (; i < s.Items.Count; i++) 
                {
                    it = s.Items[i];
                    if (it.CanBeCommaEnd) 
                    {
                        comma = false;
                        and = false;
                        or = false;
                        if ((it.Source as Pullenti.Ner.Core.ConjunctionToken).Typ == Pullenti.Ner.Core.ConjunctionType.Comma) 
                            comma = true;
                        else 
                        {
                            and = true;
                            if ((it.Source as Pullenti.Ner.Core.ConjunctionToken).Typ == Pullenti.Ner.Core.ConjunctionType.Or) 
                                or = true;
                        }
                        if (seg.Items.Count > 0) 
                        {
                            if (comma) 
                                seg.Items[seg.Items.Count - 1].CommaAfter = true;
                            else 
                            {
                                seg.Items[seg.Items.Count - 1].AndAfter = true;
                                if (or) 
                                    seg.Items[seg.Items.Count - 1].OrAfter = true;
                            }
                        }
                        continue;
                    }
                    if (it.CanBeNoun || it.Typ == SentItemType.Adverb) 
                    {
                        nit = new NGItem() { Source = it, CommaBefore = comma, AndBefore = and, OrBefore = or };
                        seg.Items.Add(nit);
                        comma = false;
                        and = false;
                        or = false;
                    }
                    else if (it.Typ == SentItemType.Verb || it.Typ == SentItemType.Conj || it.Typ == SentItemType.Delim) 
                        break;
                }
                for (int j = i; j < s.Items.Count; j++) 
                {
                    it = s.Items[j];
                    if (it.Typ == SentItemType.Verb) 
                    {
                        seg.AfterVerb = it.Source as Pullenti.Ner.Core.VerbPhraseToken;
                        break;
                    }
                    if ((it.Typ == SentItemType.Conj || it.CanBeNoun || it.Typ == SentItemType.Delim) || it.Typ == SentItemType.Adverb) 
                        break;
                }
                res.Add(seg);
            }
            foreach (NGSegment ss in res) 
            {
                ss.CreateLinks(false);
            }
            return res;
        }
        /// <summary>
        /// А это создание вариантов связей между элементами
        /// </summary>
        public void CreateLinks(bool afterPart = false)
        {
            for (int i = 0; i < Items.Count; i++) 
            {
                Items[i].Order = i;
                Items[i].Prepare();
            }
            NGLink li = null;
            for (int i = 0; i < Items.Count; i++) 
            {
                NGItem it = Items[i];
                if (it.Source.Typ == SentItemType.Adverb) 
                    continue;
                bool ignoreBefore = false;
                double mult = (double)1;
                if (it.CommaBefore || it.AndBefore) 
                {
                    for (int j = i - 1; j >= 0; j--) 
                    {
                        if (li == null) 
                            li = new NGLink();
                        li.Typ = NGLinkType.List;
                        li.From = it;
                        li.To = Items[j];
                        li.ToVerb = null;
                        li.CalcCoef(false);
                        if (li.Coef >= 0) 
                        {
                            it.Links.Add(li);
                            li = null;
                        }
                        if (it.Source.Typ == SentItemType.PartBefore || it.Source.Typ == SentItemType.SubSent || it.Source.Typ == SentItemType.Deepart) 
                        {
                            if (it.CommaBefore) 
                            {
                                if (li == null) 
                                    li = new NGLink();
                                li.Typ = NGLinkType.Participle;
                                li.From = it;
                                li.To = Items[j];
                                li.ToVerb = null;
                                li.CalcCoef(false);
                                if (li.Coef >= 0) 
                                {
                                    it.Links.Add(li);
                                    li = null;
                                }
                            }
                        }
                        if ((!it.AndBefore && it.Source.Typ == SentItemType.Noun && Items[j].Source.Typ == SentItemType.Noun) && Items[i - 1].Source.Typ == SentItemType.PartBefore) 
                        {
                            bool ok = true;
                            for (int jj = j + 1; jj < i; jj++) 
                            {
                                if ((Items[jj].Source.Typ == SentItemType.Delim || Items[jj].Source.Typ == SentItemType.Noun || Items[jj].Source.Typ == SentItemType.SubSent) || Items[jj].Source.Typ == SentItemType.PartBefore) 
                                {
                                }
                                else 
                                {
                                    ok = false;
                                    break;
                                }
                            }
                            if (ok) 
                            {
                                if (li == null) 
                                    li = new NGLink();
                                li.Typ = NGLinkType.Genetive;
                                li.From = it;
                                li.To = Items[j];
                                li.ToVerb = null;
                                li.CalcCoef(false);
                                if (li.Coef >= 0) 
                                {
                                    it.Links.Add(li);
                                    li = null;
                                }
                            }
                        }
                    }
                    ignoreBefore = true;
                }
                else 
                {
                    for (int j = i - 1; j >= 0; j--) 
                    {
                        if (Items[j].Source.Typ == SentItemType.SubSent) 
                            continue;
                        if (li == null) 
                            li = new NGLink();
                        li.Typ = NGLinkType.Genetive;
                        li.From = it;
                        li.To = Items[j];
                        li.ToVerb = null;
                        li.CalcCoef(false);
                        if (li.Coef >= 0) 
                        {
                            it.Links.Add(li);
                            li = null;
                        }
                        if (li == null) 
                            li = new NGLink();
                        li.Typ = NGLinkType.Name;
                        li.From = it;
                        li.To = Items[j];
                        li.ToVerb = null;
                        li.CalcCoef(false);
                        if (li.Coef >= 0) 
                        {
                            it.Links.Add(li);
                            li = null;
                        }
                        bool nodelim = true;
                        for (int jj = j + 1; jj <= i; jj++) 
                        {
                            if (Items[jj].CommaBefore || Items[jj].AndBefore) 
                            {
                                nodelim = false;
                                break;
                            }
                        }
                        if (nodelim) 
                        {
                            if (li == null) 
                                li = new NGLink();
                            li.Typ = NGLinkType.Be;
                            li.From = it;
                            li.To = Items[j];
                            li.ToVerb = null;
                            li.CalcCoef(false);
                            if (li.Coef >= 0) 
                            {
                                it.Links.Add(li);
                                li = null;
                            }
                        }
                        if (it.Source.Typ == SentItemType.PartBefore || it.Source.Typ == SentItemType.SubSent || it.Source.Typ == SentItemType.Deepart) 
                        {
                            bool hasDelim = false;
                            for (int jj = i - 1; jj > j; jj--) 
                            {
                                if (Items[jj].Source.CanBeCommaEnd) 
                                {
                                    hasDelim = true;
                                    break;
                                }
                            }
                            if (hasDelim) 
                            {
                                if (li == null) 
                                    li = new NGLink();
                                li.Typ = NGLinkType.Participle;
                                li.From = it;
                                li.To = Items[j];
                                li.ToVerb = null;
                                li.CalcCoef(false);
                                if (li.Coef >= 0) 
                                {
                                    it.Links.Add(li);
                                    li = null;
                                }
                            }
                        }
                        if (Items[j].Source.Typ == SentItemType.PartBefore) 
                            mult *= 0.5;
                        if (Items[j].Source.Typ == SentItemType.Verb) 
                        {
                            ignoreBefore = true;
                            break;
                        }
                    }
                    if (BeforeVerb != null && !ignoreBefore && it.Source.Typ != SentItemType.Deepart) 
                    {
                        bool ok = false;
                        if (li == null) 
                            li = new NGLink();
                        li.Typ = NGLinkType.Agent;
                        li.From = it;
                        li.To = null;
                        li.ToVerb = BeforeVerb;
                        li.CalcCoef(false);
                        li.Coef *= mult;
                        if (li.Coef >= 0) 
                        {
                            it.Links.Add(li);
                            ok = true;
                            li = null;
                        }
                        if (li == null) 
                            li = new NGLink();
                        li.Typ = NGLinkType.Pacient;
                        li.From = it;
                        li.To = null;
                        li.ToVerb = BeforeVerb;
                        li.CalcCoef(false);
                        li.Coef *= mult;
                        if (li.Coef >= 0) 
                        {
                            it.Links.Add(li);
                            ok = true;
                            li = null;
                        }
                            {
                                if (li == null) 
                                    li = new NGLink();
                                li.Typ = NGLinkType.Actant;
                                li.From = it;
                                li.To = null;
                                li.ToVerb = BeforeVerb;
                                li.CalcCoef(false);
                                li.Coef *= mult;
                                if (ok) 
                                    li.Coef /= 2;
                                if (li.Coef >= 0) 
                                {
                                    it.Links.Add(li);
                                    ok = true;
                                    li = null;
                                }
                            }
                    }
                }
                if (AfterVerb != null && it.Source.Typ != SentItemType.Deepart) 
                {
                    bool ok = false;
                    if (afterPart && BeforeVerb != null) 
                    {
                        foreach (NGLink l in it.Links) 
                        {
                            if (l.ToVerb == BeforeVerb && ((l.Typ == NGLinkType.Agent || l.Typ == NGLinkType.Pacient))) 
                                ok = true;
                        }
                        if (ok) 
                            continue;
                    }
                    if (li == null) 
                        li = new NGLink();
                    li.Typ = NGLinkType.Agent;
                    li.From = it;
                    li.To = null;
                    li.ToVerb = AfterVerb;
                    li.CalcCoef(false);
                    if (li.Coef >= 0) 
                    {
                        it.Links.Add(li);
                        ok = true;
                        li = null;
                    }
                    if (li == null) 
                        li = new NGLink();
                    li.Typ = NGLinkType.Pacient;
                    li.From = it;
                    li.To = null;
                    li.ToVerb = AfterVerb;
                    li.CalcCoef(false);
                    if (li.Coef >= 0) 
                    {
                        it.Links.Add(li);
                        ok = true;
                        li = null;
                    }
                    if (li == null) 
                        li = new NGLink();
                    li.Typ = NGLinkType.Actant;
                    li.From = it;
                    li.To = null;
                    li.ToVerb = AfterVerb;
                    li.CalcCoef(false);
                    if (li.Coef >= 0) 
                    {
                        it.Links.Add(li);
                        ok = true;
                        li = null;
                    }
                }
            }
            for (int i = 1; i < Items.Count; i++) 
            {
                NGItem it = Items[i];
                if (it.Source.Typ != SentItemType.Noun) 
                    continue;
                NGItem it0 = Items[i - 1];
                if (it0.Source.Typ != SentItemType.Noun) 
                    continue;
                if (it0.Links.Count > 0) 
                    continue;
                li = new NGLink() { Typ = NGLinkType.Genetive, From = it0, To = it, Reverce = true };
                li.CalcCoef(true);
                if (li.Coef > 0) 
                    it0.Links.Add(li);
            }
        }
        public void CreateVariants(int maxCount = 5)
        {
            Variants.Clear();
            for (int i = 0; i < Items.Count; i++) 
            {
                Items[i].Ind = 0;
            }
            NGSegmentVariant var = null;
            for (int kkk = 0; kkk < 1000; kkk++) 
            {
                if (var == null) 
                    var = new NGSegmentVariant() { Source = this };
                else 
                    var.Links.Clear();
                for (int i = 0; i < Items.Count; i++) 
                {
                    NGItem it = Items[i];
                    if (it.Ind < it.Links.Count) 
                        var.Links.Add(it.Links[it.Ind]);
                    else 
                        var.Links.Add(null);
                }
                var.CalcCoef();
                if (var.Coef >= 0) 
                {
                    Variants.Add(var);
                    var = null;
                    if (Variants.Count > (maxCount * 5)) 
                    {
                        _sortVars(Variants);
                        Variants.RemoveRange(maxCount, Variants.Count - maxCount);
                    }
                }
                int j;
                for (j = Items.Count - 1; j >= 0; j--) 
                {
                    NGItem it = Items[j];
                    if ((++it.Ind) >= it.Links.Count) 
                        it.Ind = 0;
                    else 
                        break;
                }
                if (j < 0) 
                    break;
            }
            _sortVars(Variants);
            if (Variants.Count > maxCount) 
                Variants.RemoveRange(maxCount, Variants.Count - maxCount);
        }
        static void _sortVars(List<NGSegmentVariant> vars)
        {
            vars.Sort();
        }
    }
}