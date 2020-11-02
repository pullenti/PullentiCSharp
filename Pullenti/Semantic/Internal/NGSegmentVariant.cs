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
    class NGSegmentVariant : IComparable<NGSegmentVariant>
    {
        public double Coef;
        public NGSegment Source;
        public List<NGLink> Links = new List<NGLink>();
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0} = ", Coef);
            foreach (NGLink it in Links) 
            {
                if (it != Links[0]) 
                    res.Append("; \r\n");
                if (it == null) 
                    res.Append("<null>");
                else 
                    res.Append(it.ToString());
            }
            return res.ToString();
        }
        static bool _compareListItemTails(Pullenti.Ner.MetaToken mt1, Pullenti.Ner.MetaToken mt2)
        {
            Pullenti.Ner.TextToken t1 = mt1.EndToken as Pullenti.Ner.TextToken;
            Pullenti.Ner.TextToken t2 = mt2.EndToken as Pullenti.Ner.TextToken;
            if (t1 == null || t2 == null) 
                return true;
            int k = 0;
            int i1 = t1.Term.Length - 1;
            int i2 = t2.Term.Length - 1;
            for (; i1 > 0 && i2 > 0; i1--,i2--,k++) 
            {
                if (t1.Term[i1] != t2.Term[i2]) 
                    break;
            }
            if (k >= 2) 
                return true;
            string nn = t2.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
            if (t1.IsValue(nn, null)) 
                return true;
            if (((t1.Morph.Number & t2.Morph.Number)) == Pullenti.Morph.MorphNumber.Undefined) 
                return false;
            if (((t1.Morph.Case & t2.Morph.Case)).IsUndefined) 
                return false;
            if (t1.Morph.Class.IsVerb != t2.Morph.Class.IsVerb && t1.Morph.Class.IsAdjective != t2.Morph.Class.IsAdjective) 
                return false;
            return true;
        }
        public double CalcCoef()
        {
            Coef = 0;
            foreach (NGLink it in Links) 
            {
                if (it != null) 
                    Coef += it.Coef;
            }
            for (int i = 0; i < Links.Count; i++) 
            {
                NGLink li1 = Links[i];
                if (li1 == null || li1.To == null) 
                    continue;
                if (li1.Reverce) 
                    continue;
                int i0 = li1.To.Order;
                if (i0 >= li1.From.Order) 
                    return Coef = -1;
                for (int k = i0 + 1; k < i; k++) 
                {
                    NGLink li = Links[k];
                    if (li == null) 
                        continue;
                    if (li.ToVerb != null) 
                        return Coef = -1;
                    int i1 = li.To.Order;
                    if ((i1 < i0) || i1 > i) 
                        return Coef = -1;
                    if (li.Typ == NGLinkType.List && li1.Typ == NGLinkType.List && i0 == i1) 
                        return Coef = -1;
                }
            }
            for (int i = 0; i < Links.Count; i++) 
            {
                List<NGItem> list = this.GetList(i);
                if (list == null) 
                    continue;
                int k;
                for (k = 1; k < (list.Count - 1); k++) 
                {
                    if (list[k].AndBefore) 
                        break;
                }
                if (k >= (list.Count - 1) && list[k].AndBefore) 
                    Coef += Pullenti.Semantic.SemanticService.Params.List;
                else 
                {
                    int ors = 0;
                    int ands = 0;
                    for (k = 1; k < list.Count; k++) 
                    {
                        if (list[k].OrBefore) 
                            ors++;
                        else if (list[k].AndBefore) 
                            ands++;
                    }
                    if (ands > 0 && ors > 0) 
                        return Coef = -1;
                    for (k = 1; k < list.Count; k++) 
                    {
                        if (!list[k].AndBefore) 
                            break;
                    }
                    if (k >= list.Count) 
                    {
                    }
                    else 
                        return Coef = -1;
                }
                NGLink ngli = new NGLink() { Typ = NGLinkType.List };
                for (k = 0; k < (list.Count - 2); k++) 
                {
                    for (int kk = k + 2; kk < list.Count; kk++) 
                    {
                        ngli.From = list[kk];
                        ngli.To = list[k];
                        ngli.CalcCoef(false);
                        if (ngli.Coef < 0) 
                            return Coef = -1;
                    }
                }
                bool prepIsNotExiAll = false;
                for (k = 0; k < (list.Count - 1); k++) 
                {
                    for (int kk = k + 1; kk < list.Count; kk++) 
                    {
                        if (!_compareListItemTails(list[k].Source.Source, list[kk].Source.Source)) 
                            Coef /= 2;
                        if (string.IsNullOrEmpty(list[k].Source.Prep) != string.IsNullOrEmpty(list[kk].Source.Prep)) 
                        {
                            string str1 = list[k].Source.EndToken.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                            string str2 = list[kk].Source.EndToken.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                            if (str1 != str2) 
                                prepIsNotExiAll = true;
                        }
                    }
                }
                if (prepIsNotExiAll) 
                    Coef /= 2;
                NGItem last = list[list.Count - 1];
                bool ok = true;
                NGLink lalink = null;
                foreach (NGLink ll in Links) 
                {
                    if (ll != null && ll.Typ == NGLinkType.Genetive) 
                    {
                        if (ll.To == last) 
                            lalink = ll;
                        else if (list.Contains(ll.To)) 
                        {
                            ok = false;
                            break;
                        }
                    }
                }
                if (!ok || lalink == null) 
                    continue;
                NGLink test = new NGLink() { From = lalink.From, Typ = lalink.Typ };
                int j;
                for (j = 0; j < (list.Count - 1); j++) 
                {
                    test.To = list[j];
                    int ord = test.To.Order;
                    test.To.Order = last.Order;
                    test.CalcCoef(false);
                    test.To.Order = ord;
                    if (test.Coef < 0) 
                        break;
                }
                if (j >= (list.Count - 1)) 
                    lalink.ToAllListItems = true;
            }
            int befAg = 0;
            int befPac = 0;
            int aftAg = 0;
            int aftPac = 0;
            for (int i = 0; i < Links.Count; i++) 
            {
                NGLink li = Links[i];
                if (li == null) 
                    continue;
                if (li.Typ == NGLinkType.List) 
                    continue;
                if (li.Typ == NGLinkType.Participle) 
                {
                    if (li.From.Source.PartVerbTyp != NGLinkType.Undefined) 
                    {
                    }
                }
                if ((li.Typ == NGLinkType.Agent || li.Typ == NGLinkType.Pacient || li.Typ == NGLinkType.Genetive) || li.Typ == NGLinkType.Participle) 
                {
                    if (li.Plural == 1) 
                    {
                        bool ok = false;
                        if (li.Typ == NGLinkType.Participle && li.To != null && this.GetList(li.To.Order) != null) 
                            ok = true;
                        else if (li.Typ != NGLinkType.Participle && this.GetList(i) != null) 
                            ok = true;
                        else 
                        {
                            double co = li.Coef;
                            li.CalcCoef(true);
                            if (li.Coef > 0) 
                                ok = true;
                            li.Coef = co;
                            li.Plural = 1;
                        }
                        if (!ok) 
                            return Coef = -1;
                    }
                    else if (li.Plural == 0) 
                    {
                        if (li.Typ != NGLinkType.Participle && this.GetList(i) != null) 
                            return Coef = -1;
                        if (li.Typ == NGLinkType.Participle && li.To != null && this.GetList(li.To.Order) != null) 
                            return Coef = -1;
                    }
                }
                if (li.Typ == NGLinkType.Agent || li.Typ == NGLinkType.Pacient || li.Typ == NGLinkType.Actant) 
                {
                }
                else 
                    continue;
                if (li.ToVerb != null && li.ToVerb == Source.BeforeVerb) 
                {
                    if (Source.AfterVerb != null && !Source.BeforeVerb.FirstVerb.IsParticiple) 
                    {
                        bool hasDelim = false;
                        int ind = li.From.Order;
                        List<NGItem> list = this.GetList(ind);
                        if (list != null) 
                            ind = list[list.Count - 1].Order;
                        for (int ii = ind; ii < Source.Items.Count; ii++) 
                        {
                            if (Source.Items[ii].AndAfter || Source.Items[ii].CommaAfter) 
                                hasDelim = true;
                        }
                        if (!hasDelim) 
                            return Coef = -1;
                    }
                    if (li.Typ == NGLinkType.Agent && li.ToVerb.FirstVerb.IsDeeParticiple) 
                    {
                        bool hasDelim = false;
                        for (int ii = 0; ii <= li.From.Order; ii++) 
                        {
                            if (Source.Items[ii].AndBefore || Source.Items[ii].CommaBefore) 
                                hasDelim = true;
                        }
                        if (!hasDelim) 
                            return Coef = -1;
                    }
                    if (li.Typ == NGLinkType.Agent) 
                        befAg++;
                    else if (li.Typ == NGLinkType.Pacient) 
                        befPac++;
                    if (li.From.Source.SubSent != null) 
                        continue;
                }
                else if (li.ToVerb != null && li.ToVerb == Source.AfterVerb) 
                {
                    if (Source.BeforeVerb != null && !Source.BeforeVerb.FirstVerb.IsParticiple) 
                    {
                        bool hasDelim = false;
                        for (int ii = 0; ii <= li.From.Order; ii++) 
                        {
                            if (Source.Items[ii].AndBefore || Source.Items[ii].CommaBefore) 
                                hasDelim = true;
                        }
                        if (!hasDelim) 
                            return Coef = -1;
                    }
                    if (li.From.Source.SubSent != null) 
                        continue;
                    if (li.Typ == NGLinkType.Agent) 
                        aftAg++;
                    else if (li.Typ == NGLinkType.Pacient) 
                        aftPac++;
                }
                if (li.Typ == NGLinkType.Actant) 
                    continue;
            }
            if ((befAg > 1 || befPac > 1 || aftAg > 1) || aftPac > 1) 
                return Coef = -1;
            for (int i = 0; i < Links.Count; i++) 
            {
                NGLink li = Links[i];
                if (li == null) 
                    continue;
                if (li.Typ != NGLinkType.Actant || li.ToVerb == null) 
                    continue;
            }
            for (int i = 0; i < Links.Count; i++) 
            {
                NGLink li = Links[i];
                if (li == null) 
                    continue;
                if (li.Typ != NGLinkType.Genetive || li.To == null) 
                    continue;
                if (li.From.Source.Typ == SentItemType.Formula) 
                {
                    foreach (NGLink li0 in Links) 
                    {
                        if ((li0 != null && li0 != li && li0.Typ == NGLinkType.Genetive) && li0.From == li.To) 
                            Coef /= 2;
                    }
                }
                if (li.To.Source.Typ == SentItemType.Formula) 
                {
                    foreach (NGLink li0 in Links) 
                    {
                        if ((li0 != null && li0 != li && li0.Typ == NGLinkType.Genetive) && li0.To == li.To) 
                        {
                            if (li0.From.Order < li.From.Order) 
                                Coef /= 2;
                        }
                    }
                }
            }
            return Coef;
        }
        public int CompareTo(NGSegmentVariant other)
        {
            if (Coef > other.Coef) 
                return -1;
            if (Coef < other.Coef) 
                return 1;
            return 0;
        }
        public List<NGItem> GetListByLastItem(NGItem it)
        {
            List<NGItem> res = new List<NGItem>();
            res.Add(it);
            for (int i = Links.Count - 1; i >= 0; i--) 
            {
                if ((Links[i] != null && Links[i].From == it && Links[i].Typ == NGLinkType.List) && Links[i].To != null) 
                {
                    it = Links[i].To;
                    res.Insert(0, it);
                }
            }
            if (res.Count > 1) 
                return res;
            return null;
        }
        public List<NGItem> GetList(int ord)
        {
            if (ord >= Source.Items.Count) 
                return null;
            NGLink li = Links[ord];
            if (li == null) 
                return null;
            List<NGItem> res = null;
            NGItem ngit = Source.Items[ord];
            if (li.Typ == NGLinkType.List) 
            {
                if (li.ToVerb == null) 
                    return null;
                res = new List<NGItem>();
                res.Add(new NGItem() { Source = new SentItem(li.ToVerb), Order = ord - 1 });
                res.Add(ngit);
            }
            for (int i = ord + 1; i < Links.Count; i++) 
            {
                li = Links[i];
                if (li == null || li.Typ != NGLinkType.List || li.To == null) 
                    continue;
                if (li.To == ngit) 
                {
                    if (res == null) 
                    {
                        res = new List<NGItem>();
                        res.Add(ngit);
                    }
                    ngit = Source.Items[i];
                    res.Add(ngit);
                }
            }
            return res;
        }
        public void CorrectMorph()
        {
            for (int i = 0; i < Links.Count; i++) 
            {
                NGLink li = Links[i];
                if (li == null) 
                    continue;
                if (li.Typ == NGLinkType.Agent || li.Typ == NGLinkType.Pacient) 
                {
                    if (li.Plural == 1) 
                    {
                        List<NGItem> list = this.GetList(i);
                        if (list != null) 
                            continue;
                        li.From.Source.Plural = 1;
                    }
                }
            }
        }
    }
}