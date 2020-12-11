/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */


namespace Pullenti.Ner.Core
{
    // Это привязка элемента внутренней отнологии к тексту
    public class IntOntologyToken : Pullenti.Ner.MetaToken
    {
        public IntOntologyToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public IntOntologyItem Item;
        public Termin Termin;
    }
}