/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */


namespace Pullenti.Semantic.Internal
{
    class SemAttributeEx
    {
        public SemAttributeEx(Pullenti.Ner.MetaToken mt)
        {
            Token = mt;
        }
        public Pullenti.Ner.MetaToken Token;
        public Pullenti.Semantic.SemAttribute Attr = new Pullenti.Semantic.SemAttribute();
    }
}