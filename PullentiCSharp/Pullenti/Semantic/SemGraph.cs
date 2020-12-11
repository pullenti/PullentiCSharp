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

namespace Pullenti.Semantic
{
    /// <summary>
    /// Семантический граф
    /// </summary>
    public class SemGraph
    {
        /// <summary>
        /// Владелец графа (фрагмент, блок или документ)
        /// </summary>
        public ISemContainer Owner;
        /// <summary>
        /// Вышележащий граф (граф у вышележащего владельца)
        /// </summary>
        public SemGraph Higher
        {
            get
            {
                if (Owner != null && Owner.Higher != null) 
                    return Owner.Higher.Graph;
                else 
                    return null;
            }
        }
        /// <summary>
        /// Список объектов SemObject (узлы графа), упорядочиваются по первым позициям в тексте
        /// </summary>
        public List<SemObject> Objects = new List<SemObject>();
        /// <summary>
        /// Список связей SemLink, неупорядоченный, дублируется у объектов в LinksFrom и LinksTo
        /// </summary>
        public List<SemLink> Links = new List<SemLink>();
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            tmp.AppendFormat("{0}obj {1}links: ", Objects.Count, Links.Count);
            foreach (SemLink li in Links) 
            {
                if (li != Links[0]) 
                    tmp.Append("; ");
                tmp.Append(li);
                if (tmp.Length > 100) 
                    break;
            }
            if (Links.Count == 0) 
            {
                foreach (SemObject o in Objects) 
                {
                    if (o != Objects[0]) 
                        tmp.Append("; ");
                    tmp.Append(o);
                    if (tmp.Length > 100) 
                        break;
                }
            }
            return tmp.ToString();
        }
        public SemLink AddLink(SemLinkType typ, SemObject src, SemObject tgt, string ques = null, bool or = false, string prep = null)
        {
            if (src == null || tgt == null) 
                return null;
            foreach (SemLink li in src.Graph.Links) 
            {
                if (li.Typ == typ && li.Source == src && li.Target == tgt) 
                    return li;
            }
            if (src.Graph != tgt.Graph) 
            {
                foreach (SemLink li in tgt.Graph.Links) 
                {
                    if (li.Typ == typ && li.Source == src && li.Target == tgt) 
                        return li;
                }
            }
            if (tgt.Morph.NormalCase == "ДОМ") 
            {
            }
            SemLink res = new SemLink(this, src, tgt) { Typ = typ, Question = ques, IsOr = or, Preposition = prep };
            Links.Add(res);
            return res;
        }
        public void RemoveLink(SemLink li)
        {
            if (Links.Contains(li)) 
                Links.Remove(li);
            if (li.Source.LinksFrom.Contains(li)) 
                li.Source.LinksFrom.Remove(li);
            if (li.Target.LinksTo.Contains(li)) 
                li.Target.LinksTo.Remove(li);
            if (li.AltLink != null && li.AltLink.AltLink == li) 
                li.AltLink.AltLink = null;
        }
        public void MergeWith(SemGraph gr)
        {
            foreach (SemObject o in gr.Objects) 
            {
                if (!Objects.Contains(o)) 
                {
                    Objects.Add(o);
                    o.Graph = this;
                }
            }
            foreach (SemLink li in gr.Links) 
            {
                if (!Links.Contains(li)) 
                    Links.Add(li);
            }
        }
        public void RemoveObject(SemObject obj)
        {
            foreach (SemLink li in obj.LinksFrom) 
            {
                if (li.Target.LinksTo.Contains(li)) 
                    li.Target.LinksTo.Remove(li);
                if (Links.Contains(li)) 
                    Links.Remove(li);
                else if (li.Target.Graph.Links.Contains(li)) 
                    li.Target.Graph.Links.Remove(li);
            }
            foreach (SemLink li in obj.LinksTo) 
            {
                if (li.Source.LinksFrom.Contains(li)) 
                    li.Source.LinksFrom.Remove(li);
                if (Links.Contains(li)) 
                    Links.Remove(li);
                else if (li.Source.Graph.Links.Contains(li)) 
                    li.Source.Graph.Links.Remove(li);
            }
            if (Objects.Contains(obj)) 
                Objects.Remove(obj);
        }
    }
}