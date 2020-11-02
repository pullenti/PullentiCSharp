/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Titlepage.Internal
{
    class PersonRelation
    {
        public Pullenti.Ner.Person.PersonReferent Person;
        public Dictionary<TitleItemToken.Types, float> Coefs = new Dictionary<TitleItemToken.Types, float>();
        public TitleItemToken.Types Best
        {
            get
            {
                TitleItemToken.Types res = TitleItemToken.Types.Undefined;
                float max = (float)0;
                foreach (KeyValuePair<TitleItemToken.Types, float> v in Coefs) 
                {
                    if (v.Value > max) 
                    {
                        res = v.Key;
                        max = v.Value;
                    }
                    else if (v.Value == max) 
                        res = TitleItemToken.Types.Undefined;
                }
                return res;
            }
        }
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0} {1}", Person.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0), Best);
            foreach (KeyValuePair<TitleItemToken.Types, float> v in Coefs) 
            {
                res.AppendFormat(" {0}({1})", v.Value, v.Key.ToString());
            }
            return res.ToString();
        }
    }
}