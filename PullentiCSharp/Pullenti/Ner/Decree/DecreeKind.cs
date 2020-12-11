/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Decree
{
    /// <summary>
    /// Классы нормативных актов
    /// </summary>
    public enum DecreeKind : int
    {
        Undefined,
        /// <summary>
        /// Кодекс
        /// </summary>
        Kodex,
        /// <summary>
        /// Устав
        /// </summary>
        Ustav,
        /// <summary>
        /// Конвенция
        /// </summary>
        Konvention,
        /// <summary>
        /// Договор, контракт
        /// </summary>
        Contract,
        /// <summary>
        /// Проект
        /// </summary>
        Project,
        /// <summary>
        /// Источники опубликований
        /// </summary>
        Publisher,
        /// <summary>
        /// Госпрограммы
        /// </summary>
        Program,
        /// <summary>
        /// Стандарт (ГОСТ, ТУ, ANSI и пр.)
        /// </summary>
        Standard,
    }
}