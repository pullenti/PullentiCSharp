/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Mail
{
    /// <summary>
    /// Тип блока письма
    /// </summary>
    public enum MailKind : int
    {
        Undefined = 0,
        /// <summary>
        /// Заголовок
        /// </summary>
        Head = 1,
        /// <summary>
        /// Приветствие
        /// </summary>
        Hello = 2,
        /// <summary>
        /// Содержимое
        /// </summary>
        Body = 3,
        /// <summary>
        /// Подпись
        /// </summary>
        Tail = 4,
    }
}