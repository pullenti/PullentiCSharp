/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic.Internal
{
    enum SentItemType : int
    {
        Undefined,
        Noun,
        Verb,
        Conj,
        Delim,
        Adverb,
        /// <summary>
        /// Деепричастие
        /// </summary>
        Deepart,
        /// <summary>
        /// Причастие с сущ. перед (возможным сущ.!)
        /// </summary>
        PartBefore,
        /// <summary>
        /// Причастие с сущ. после
        /// </summary>
        PartAfter,
        /// <summary>
        /// Деепричастие или придаточное предложение
        /// </summary>
        SubSent,
        /// <summary>
        /// Это всякие формулы и отношения
        /// </summary>
        Formula,
    }
}