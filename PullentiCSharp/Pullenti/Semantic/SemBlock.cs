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
    /// Блок документа (абзац)
    /// </summary>
    public class SemBlock : ISemContainer
    {
        public SemBlock(SemDocument blk)
        {
            m_Higher = blk;
        }
        /// <summary>
        /// Семантический граф объектов этого блока
        /// </summary>
        public SemGraph Graph
        {
            get
            {
                return m_Graph;
            }
        }
        SemGraph m_Graph = new SemGraph();
        public ISemContainer Higher
        {
            get
            {
                return m_Higher;
            }
        }
        public SemDocument m_Higher;
        public SemDocument Document
        {
            get
            {
                return m_Higher as SemDocument;
            }
        }
        /// <summary>
        /// Фрагменты блока - список SemFragment
        /// </summary>
        public List<SemFragment> Fragments = new List<SemFragment>();
        /// <summary>
        /// А это межфрагментные связи - список SemFraglink
        /// </summary>
        public List<SemFraglink> Links = new List<SemFraglink>();
        public int BeginChar
        {
            get
            {
                return (Fragments.Count == 0 ? 0 : Fragments[0].BeginChar);
            }
        }
        public int EndChar
        {
            get
            {
                return (Fragments.Count == 0 ? 0 : Fragments[Fragments.Count - 1].EndChar);
            }
        }
        public void AddFragments(SemBlock blk)
        {
            foreach (SemFragment fr in blk.Fragments) 
            {
                fr.m_Higher = this;
                Fragments.Add(fr);
            }
            foreach (SemFraglink li in blk.Links) 
            {
                Links.Add(li);
            }
        }
        public SemFraglink AddLink(SemFraglinkType typ, SemFragment src, SemFragment tgt, string ques = null)
        {
            foreach (SemFraglink li in Links) 
            {
                if (li.Typ == typ && li.Source == src && li.Target == tgt) 
                    return li;
            }
            SemFraglink res = new SemFraglink() { Typ = typ, Source = src, Target = tgt, Question = ques };
            Links.Add(res);
            return res;
        }
        public void MergeWith(SemBlock blk)
        {
            Graph.MergeWith(blk.Graph);
            foreach (SemFragment fr in blk.Fragments) 
            {
                Fragments.Add(fr);
                fr.m_Higher = this;
            }
            foreach (SemFraglink li in blk.Links) 
            {
                Links.Add(li);
            }
        }
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            foreach (SemFragment fr in Fragments) 
            {
                string spel = fr.Spelling;
                if (spel.Length > 20) 
                    spel = spel.Substring(0, 20) + "...";
                tmp.AppendFormat("[{0}] ", spel);
            }
            return tmp.ToString();
        }
    }
}