/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Semantic.Internal
{
    public class ExplanTreeNode
    {
        public Dictionary<short, ExplanTreeNode> Nodes;
        public List<int> Groups;
        public int LazyPos;
        internal void Deserialize(Pullenti.Morph.Internal.ByteArrayWrapper str, DerivateDictionary dic, bool lazyLoad, ref int pos)
        {
            int cou = str.DeserializeShort(ref pos);
            List<int> li = (cou > 0 ? new List<int>() : null);
            for (; cou > 0; cou--) 
            {
                int id = str.DeserializeInt(ref pos);
                if (id > 0 && id <= dic.m_AllGroups.Count) 
                {
                    Pullenti.Semantic.Utils.DerivateGroup gr = dic.m_AllGroups[id - 1];
                    if (gr.LazyPos > 0) 
                    {
                        int p0 = pos;
                        pos = gr.LazyPos;
                        gr.Deserialize(str, ref pos);
                        gr.LazyPos = 0;
                        pos = p0;
                    }
                }
                li.Add(id);
            }
            if (li != null) 
                Groups = li;
            cou = str.DeserializeShort(ref pos);
            if (cou == 0) 
                return;
            for (; cou > 0; cou--) 
            {
                int ke = str.DeserializeShort(ref pos);
                int p1 = str.DeserializeInt(ref pos);
                ExplanTreeNode tn1 = new ExplanTreeNode();
                if (Nodes == null) 
                    Nodes = new Dictionary<short, ExplanTreeNode>();
                short sh = (short)ke;
                if (lazyLoad) 
                {
                    tn1.LazyPos = pos;
                    pos = p1;
                }
                else 
                    tn1.Deserialize(str, dic, false, ref pos);
                if (!Nodes.ContainsKey(sh)) 
                    Nodes.Add(sh, tn1);
            }
        }
    }
}