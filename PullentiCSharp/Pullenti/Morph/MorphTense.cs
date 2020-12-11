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
    /// Время (для глаголов)
    /// </summary>
    public enum MorphTense : short
    {
        /// <summary>
        /// Неопределено
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Прошлое
        /// </summary>
        Past = 1,
        /// <summary>
        /// Настоящее
        /// </summary>
        Present = 2,
        /// <summary>
        /// Будущее
        /// </summary>
        Future = 4,
    }
}