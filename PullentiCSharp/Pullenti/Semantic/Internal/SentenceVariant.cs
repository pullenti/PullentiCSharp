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

namespace Pullenti.Semantic.Internal
{
    class SentenceVariant : IComparable<SentenceVariant>
    {
        public double Coef;
        public List<NGSegmentVariant> Segs = new List<NGSegmentVariant>();
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            tmp.AppendFormat("{0}: ", Coef);
            foreach (NGSegmentVariant s in Segs) 
            {
                if (s != Segs[0]) 
                    tmp.Append("; \r\n");
                if (s != null) 
                    tmp.Append(s.ToString());
                else 
                    tmp.Append("null");
            }
            return tmp.ToString();
        }
        public double CalcCoef()
        {
            Coef = 0;
            for (int i = 0; i < Segs.Count; i++) 
            {
                if (Segs[i] != null) 
                    Coef += Segs[i].Coef;
            }
            for (int i = 0; i < (Segs.Count - 1); i++) 
            {
                NGSegmentVariant seg0 = Segs[i];
                if (seg0 == null) 
                    continue;
                NGSegmentVariant seg1 = Segs[i + 1];
                if (seg1 == null) 
                    continue;
                bool hasAgent = false;
                bool hasPacient = false;
                foreach (NGLink li in seg0.Links) 
                {
                    if (li != null && li.ToVerb == seg1.Source.BeforeVerb) 
                    {
                        if (li.Typ == NGLinkType.Agent) 
                            hasAgent = true;
                        else if (li.Typ == NGLinkType.Pacient) 
                        {
                            hasPacient = true;
                            foreach (NGLink lii in li.From.Links) 
                            {
                                if ((lii != null && lii.Typ == NGLinkType.Agent && lii.Coef >= li.Coef) && lii.ToVerb == li.ToVerb) 
                                {
                                    foreach (NGLink liii in seg1.Links) 
                                    {
                                        if (liii != null && liii.ToVerb == li.ToVerb && liii.Typ == NGLinkType.Agent) 
                                        {
                                            if (liii.Coef < ((lii.Coef / 3))) 
                                                return Coef = -1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (NGLink li in seg1.Links) 
                {
                    if (li != null && li.ToVerb == seg1.Source.BeforeVerb) 
                    {
                        if (li.Typ == NGLinkType.Agent && hasAgent) 
                            return Coef = -1;
                        else if (li.Typ == NGLinkType.Pacient && hasPacient) 
                            return Coef = -1;
                    }
                }
            }
            return Coef;
        }
        public int CompareTo(SentenceVariant other)
        {
            if (Coef > other.Coef) 
                return -1;
            if (Coef < other.Coef) 
                return 1;
            return 0;
        }
        public void CreateAltLinks()
        {
            double coef0 = Coef;
            for (int i = 0; i < Segs.Count; i++) 
            {
                NGSegmentVariant seg = Segs[i];
                if (seg == null) 
                    continue;
                for (int j = 0; j < seg.Links.Count; j++) 
                {
                    NGLink li = seg.Links[j];
                    if (li == null || li.Typ == NGLinkType.List) 
                        continue;
                    if (li.From.Links.Count < 2) 
                        continue;
                    if (li.From.Source.Typ == SentItemType.Formula) 
                        continue;
                    if (li.To != null && li.To.Source.Typ == SentItemType.Formula) 
                        continue;
                    foreach (NGLink l in li.From.Links) 
                    {
                        if (l != li && l.Typ != NGLinkType.List) 
                        {
                            if (l.To != null && l.To.Source.Typ == SentItemType.Formula) 
                                continue;
                            if (l.Typ == NGLinkType.Actant) 
                            {
                                if (li.Typ == NGLinkType.Agent || li.Typ == NGLinkType.Pacient) 
                                    continue;
                            }
                            seg.Links[j] = l;
                            seg.CalcCoef();
                            double coef = coef0 - 100;
                            if (seg.Coef > 0) 
                            {
                                this.CalcCoef();
                                coef = Coef;
                            }
                            Coef = coef0;
                            seg.Links[j] = li;
                            seg.CalcCoef();
                            if (coef >= Coef) 
                            {
                                li.AltLink = l;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}