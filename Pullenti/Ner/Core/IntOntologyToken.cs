/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
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
        /// <summary>
        /// Элемент словаря
        /// </summary>
        public IntOntologyItem Item;
        /// <summary>
        /// Или просто отдельный термин
        /// </summary>
        public Termin Termin;
    }
}