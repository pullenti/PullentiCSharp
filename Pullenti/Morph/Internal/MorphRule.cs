/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Morph.Internal
{
    public class MorphRule
    {
        public int Id;
        public List<string> Tails = new List<string>();
        public List<List<MorphRuleVariant>> MorphVars = new List<List<MorphRuleVariant>>();
        public bool ContainsVar(string tail)
        {
            return Tails.IndexOf(tail) >= 0;
        }
        public List<MorphRuleVariant> GetVars(string key)
        {
            int i = Tails.IndexOf(key);
            if (i >= 0) 
                return MorphVars[i];
            return null;
        }
        public MorphRuleVariant FindVar(int id)
        {
            foreach (List<MorphRuleVariant> li in MorphVars) 
            {
                foreach (MorphRuleVariant v in li) 
                {
                    if (v.Id == id) 
                        return v;
                }
            }
            return null;
        }
        public void Add(string tail, List<MorphRuleVariant> vars)
        {
            Tails.Add(tail);
            MorphVars.Add(vars);
        }
        public int LazyPos;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < Tails.Count; i++) 
            {
                if (res.Length > 0) 
                    res.Append(", ");
                res.AppendFormat("-{0}", Tails[i]);
            }
            return res.ToString();
        }
        internal void Deserialize(ByteArrayWrapper str, ref int pos)
        {
            int ii = str.DeserializeShort(ref pos);
            Id = ii;
            short id = (short)1;
            while (!str.IsEOF(pos)) 
            {
                byte b = str.DeserializeByte(ref pos);
                if (b == 0xFF) 
                    break;
                pos--;
                string key = str.DeserializeString(ref pos);
                if (key == null) 
                    key = "";
                List<MorphRuleVariant> li = new List<MorphRuleVariant>();
                while (!str.IsEOF(pos)) 
                {
                    MorphRuleVariant mrv = new MorphRuleVariant();
                    if (!mrv.Deserialize(str, ref pos)) 
                        break;
                    mrv.Tail = key;
                    mrv.RuleId = (short)ii;
                    mrv.Id = id++;
                    li.Add(mrv);
                }
                this.Add(key, li);
            }
        }
    }
}