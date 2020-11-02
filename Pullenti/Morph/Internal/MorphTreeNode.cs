/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Morph.Internal
{
    public class MorphTreeNode
    {
        /// <summary>
        /// Движение дальше по дереву
        /// </summary>
        public Dictionary<short, MorphTreeNode> Nodes;
        /// <summary>
        /// Конечные правила
        /// </summary>
        public List<int> RuleIds;
        public List<MorphRuleVariantRef> ReverceVariants;
        public int CalcTotalNodes()
        {
            int res = 0;
            if (Nodes != null) 
            {
                foreach (KeyValuePair<short, MorphTreeNode> v in Nodes) 
                {
                    res += (v.Value.CalcTotalNodes() + 1);
                }
            }
            return res;
        }
        public override string ToString()
        {
            int cou = (RuleIds == null ? 0 : RuleIds.Count);
            return string.Format("{0} ({1}, {2})", "?", this.CalcTotalNodes(), cou);
        }
        public int LazyPos;
        void _deserializeBase(ByteArrayWrapper str, ref int pos)
        {
            int cou = str.DeserializeShort(ref pos);
            if (cou > 0) 
            {
                RuleIds = new List<int>();
                for (; cou > 0; cou--) 
                {
                    int id = str.DeserializeShort(ref pos);
                    if (id == 0) 
                    {
                    }
                    RuleIds.Add(id);
                }
            }
            cou = str.DeserializeShort(ref pos);
            if (cou > 0) 
            {
                ReverceVariants = new List<MorphRuleVariantRef>();
                for (; cou > 0; cou--) 
                {
                    int rid = str.DeserializeShort(ref pos);
                    if (rid == 0) 
                    {
                    }
                    int id = str.DeserializeShort(ref pos);
                    int co = str.DeserializeShort(ref pos);
                    ReverceVariants.Add(new MorphRuleVariantRef(rid, (short)id, (short)co));
                }
            }
        }
        internal int Deserialize(ByteArrayWrapper str, ref int pos)
        {
            int res = 0;
            this._deserializeBase(str, ref pos);
            int cou = str.DeserializeShort(ref pos);
            if (cou > 0) 
            {
                Nodes = new Dictionary<short, MorphTreeNode>();
                for (; cou > 0; cou--) 
                {
                    int i = str.DeserializeShort(ref pos);
                    int pp = str.DeserializeInt(ref pos);
                    MorphTreeNode child = new MorphTreeNode();
                    int res1 = child.Deserialize(str, ref pos);
                    res += (1 + res1);
                    Nodes.Add((short)i, child);
                }
            }
            return res;
        }
        internal void DeserializeLazy(ByteArrayWrapper str, MorphEngine me, ref int pos)
        {
            this._deserializeBase(str, ref pos);
            int cou = str.DeserializeShort(ref pos);
            if (cou > 0) 
            {
                Nodes = new Dictionary<short, MorphTreeNode>();
                for (; cou > 0; cou--) 
                {
                    int i = str.DeserializeShort(ref pos);
                    int pp = str.DeserializeInt(ref pos);
                    MorphTreeNode child = new MorphTreeNode();
                    child.LazyPos = pos;
                    Nodes.Add((short)i, child);
                    pos = pp;
                }
            }
            int p = pos;
            if (RuleIds != null) 
            {
                foreach (int rid in RuleIds) 
                {
                    MorphRule r = me.GetMutRule(rid);
                    if (r.LazyPos > 0) 
                    {
                        pos = r.LazyPos;
                        r.Deserialize(str, ref pos);
                        r.LazyPos = 0;
                    }
                }
                pos = p;
            }
            if (ReverceVariants != null) 
            {
                foreach (MorphRuleVariantRef rv in ReverceVariants) 
                {
                    MorphRule r = me.GetMutRule(rv.RuleId);
                    if (r.LazyPos > 0) 
                    {
                        pos = r.LazyPos;
                        r.Deserialize(str, ref pos);
                        r.LazyPos = 0;
                    }
                }
                pos = p;
            }
        }
    }
}