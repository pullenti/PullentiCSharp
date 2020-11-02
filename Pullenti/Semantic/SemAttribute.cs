/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Семантический атрибут
    /// </summary>
    public class SemAttribute
    {
        /// <summary>
        /// Тип атрибута
        /// </summary>
        public SemAttributeType Typ = SemAttributeType.Undefined;
        /// <summary>
        /// Написание (нормализованное)
        /// </summary>
        public string Spelling;
        /// <summary>
        /// Признак отрицания
        /// </summary>
        public bool Not;
        public override string ToString()
        {
            return Spelling;
        }
    }
}