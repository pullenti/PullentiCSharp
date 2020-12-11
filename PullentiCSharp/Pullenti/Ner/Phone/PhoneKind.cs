/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Phone
{
    /// <summary>
    /// Тип телефонного номера
    /// </summary>
    public enum PhoneKind : int
    {
        Undefined = 0,
        /// <summary>
        /// Домашний
        /// </summary>
        Home,
        /// <summary>
        /// Мобильный
        /// </summary>
        Mobile,
        /// <summary>
        /// Рабочий
        /// </summary>
        Work,
        /// <summary>
        /// Факс
        /// </summary>
        Fax,
    }
}