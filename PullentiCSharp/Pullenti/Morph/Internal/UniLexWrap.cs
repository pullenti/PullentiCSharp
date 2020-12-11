/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Morph.Internal
{
    class UniLexWrap
    {
        public UniLexWrap(Pullenti.Morph.MorphLang lng)
        {
            Lang = lng;
        }
        public List<Pullenti.Morph.MorphWordForm> WordForms;
        public Pullenti.Morph.MorphLang Lang;
    }
}