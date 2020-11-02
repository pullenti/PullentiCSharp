/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Morph.Internal
{
    public class MorphRuleVariantRef : IComparable<MorphRuleVariantRef>
    {
        public MorphRuleVariantRef(int rid, short vid, short co)
        {
            RuleId = rid;
            VariantId = vid;
            Coef = co;
        }
        public int RuleId;
        public short VariantId;
        public short Coef;
        public override string ToString()
        {
            return string.Format("{0} {1}", RuleId, VariantId);
        }
        public int CompareTo(MorphRuleVariantRef other)
        {
            if (Coef > other.Coef) 
                return -1;
            if (Coef < other.Coef) 
                return 1;
            return 0;
        }
    }
}