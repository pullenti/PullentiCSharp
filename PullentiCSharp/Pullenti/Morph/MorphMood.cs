/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Morph
{
    /// <summary>
    /// Наклонение (для глаголов)
    /// </summary>
    public enum MorphMood : short
    {
        /// <summary>
        /// Неопределено
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Изъявительное
        /// </summary>
        Indicative = 1,
        /// <summary>
        /// Условное
        /// </summary>
        Subjunctive = 2,
        /// <summary>
        /// Повелительное
        /// </summary>
        Imperative = 4,
    }
}