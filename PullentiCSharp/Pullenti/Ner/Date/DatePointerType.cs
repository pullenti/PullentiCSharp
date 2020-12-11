/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Date
{
    /// <summary>
    /// Дополнительные указатели для дат
    /// </summary>
    public enum DatePointerType : int
    {
        No,
        /// <summary>
        /// В начале
        /// </summary>
        Begin,
        /// <summary>
        /// В середине
        /// </summary>
        Center,
        /// <summary>
        /// В конце
        /// </summary>
        End,
        /// <summary>
        /// В настоящее время, сегодня
        /// </summary>
        Today,
        /// <summary>
        /// Зимой
        /// </summary>
        Winter,
        /// <summary>
        /// Весной
        /// </summary>
        Spring,
        /// <summary>
        /// Летом
        /// </summary>
        Summer,
        /// <summary>
        /// Осенью
        /// </summary>
        Autumn,
        /// <summary>
        /// Около, примерно
        /// </summary>
        About,
        /// <summary>
        /// Неопределено (например, 20__ года )
        /// </summary>
        Undefined,
    }
}