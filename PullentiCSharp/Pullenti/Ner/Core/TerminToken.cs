/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Метатокен - результат привязки термина Termin словаря TerminCollection. Формируется методом TryParse или TryParseAll у TerminCollection.
    /// </summary>
    public class TerminToken : Pullenti.Ner.MetaToken
    {
        public TerminToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        /// <summary>
        /// Ссылка на термин словаря
        /// </summary>
        public Termin Termin;
        public bool AbridgeWithoutPoint;
    }
}