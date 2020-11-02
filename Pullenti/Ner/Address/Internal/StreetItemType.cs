/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Address.Internal
{
    public enum StreetItemType : int
    {
        /// <summary>
        /// Это существительное - улица, проезд и пр.
        /// </summary>
        Noun,
        /// <summary>
        /// Это название
        /// </summary>
        Name,
        /// <summary>
        /// Номер
        /// </summary>
        Number,
        /// <summary>
        /// Стандартное прилагательное (Большой, Средний ...)
        /// </summary>
        StdAdjective,
        /// <summary>
        /// Стандартное имя
        /// </summary>
        StdName,
        /// <summary>
        /// Стандартная часть имени
        /// </summary>
        StdPartOfName,
        /// <summary>
        /// 40-летия чего-то там
        /// </summary>
        Age,
        /// <summary>
        /// Некоторое фиусированное название (МКАД)
        /// </summary>
        Fix,
    }
}