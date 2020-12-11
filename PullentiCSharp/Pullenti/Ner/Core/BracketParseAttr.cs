/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Атрибуты выделения последовательности между скобок-кавычек. Битовая маска.
    /// </summary>
    public enum BracketParseAttr : int
    {
        /// <summary>
        /// Нет
        /// </summary>
        No = 0,
        /// <summary>
        /// По умолчанию, посл-ть не должна содержать чистых глаголов (если есть, то null). 
        /// Почему так? Да потому, что это используется в основном для имён у именованных 
        /// сущностей, а там не может быть глаголов. 
        /// Если же этот ключ указан, то глаголы не проверяются.
        /// </summary>
        CanContainsVerbs = 2,
        /// <summary>
        /// Брать первую же подходящую закрывающую кавычку. Если не задано, то может искать сложные 
        /// случаи вложенных кавычек.
        /// </summary>
        NearCloseBracket = 4,
        /// <summary>
        /// Внутри могут быть переходы на новую строку (многострочный)
        /// </summary>
        CanBeManyLines = 8,
    }
}