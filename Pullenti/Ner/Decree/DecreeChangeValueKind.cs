/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Decree
{
    /// <summary>
    /// Типы изменяющих структурный элемент значений
    /// </summary>
    public enum DecreeChangeValueKind : int
    {
        Undefined,
        /// <summary>
        /// Текстовой фрагмент
        /// </summary>
        Text,
        /// <summary>
        /// Слова (в точном значении)
        /// </summary>
        Words,
        /// <summary>
        /// Слова (в неточном значений)
        /// </summary>
        RobustWords,
        /// <summary>
        /// Цифры
        /// </summary>
        Numbers,
        /// <summary>
        /// Предложение
        /// </summary>
        Sequence,
        /// <summary>
        /// Сноска
        /// </summary>
        Footnote,
        /// <summary>
        /// Блок со словами
        /// </summary>
        Block,
    }
}