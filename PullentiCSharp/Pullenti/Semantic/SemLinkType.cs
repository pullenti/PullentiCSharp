/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Тип семантической связи
    /// </summary>
    public enum SemLinkType : int
    {
        Undefined,
        /// <summary>
        /// Детализация (какой?)
        /// </summary>
        Detail,
        /// <summary>
        /// Именование
        /// </summary>
        Naming,
        /// <summary>
        /// Агент (кто действует)
        /// </summary>
        Agent,
        /// <summary>
        /// Пациент (на кого действуют)
        /// </summary>
        Pacient,
        /// <summary>
        /// Причастный и деепричастный оборот
        /// </summary>
        Participle,
        /// <summary>
        /// Анафорическая ссылка (он, который, ...)
        /// </summary>
        Anafor,
    }
}