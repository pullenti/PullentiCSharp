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
    class NGItem
    {
        public SentItem Source;
        public int Order;
        public bool CommaBefore;
        public bool CommaAfter;
        public bool AndBefore;
        public bool AndAfter;
        public bool OrBefore;
        public bool OrAfter;
        public List<NGLink> Links = new List<NGLink>();
        public int Ind;
        public Pullenti.Semantic.SemObject ResObject
        {
            get
            {
                return (Source == null ? null : Source.Result);
            }
        }
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            if (CommaBefore) 
                tmp.Append("[,] ");
            else if (OrBefore) 
                tmp.Append("[|] ");
            else if (AndBefore) 
                tmp.Append("[&] ");
            tmp.Append(Source.ToString());
            if (CommaAfter) 
                tmp.Append(" [,]");
            else if (OrAfter) 
                tmp.Append(" [|]");
            else if (AndAfter) 
                tmp.Append(" [&]");
            return tmp.ToString();
        }
        public void Prepare()
        {
            Links.Clear();
        }
    }
}