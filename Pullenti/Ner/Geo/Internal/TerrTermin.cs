/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Geo.Internal
{
    public class TerrTermin : Pullenti.Ner.Core.Termin
    {
        public TerrTermin(string source, Pullenti.Morph.MorphLang lang = null) : base(null, lang, false)
        {
            this.InitByNormalText(source, lang);
        }
        public bool IsState;
        public bool IsRegion;
        public bool IsAdjective;
        public bool IsAlwaysPrefix;
        public bool IsDoubt;
        public bool IsMoscowRegion;
        public bool IsStrong;
        public bool IsSpecificPrefix;
        public bool IsSovet;
    }
}