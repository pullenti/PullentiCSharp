/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic.Internal
{
    public class AlgoParam
    {
        public string Name;
        public double Value;
        public double Min;
        public double Max;
        public double Delta;
        public int Count
        {
            get
            {
                return ((int)((((Max - Min)) / Delta))) + 1;
            }
        }
        public override string ToString()
        {
            return string.Format("{0}={1} [{2} .. {3}] by {4}", Name, Value, Min, Max, Delta);
        }
    }
}