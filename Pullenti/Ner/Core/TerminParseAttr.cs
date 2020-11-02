/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Атрибуты привязки токена к термину словаря TerminCollection методом TryParse. Битовая маска.
    /// </summary>
    public enum TerminParseAttr : int
    {
        /// <summary>
        /// Атрибут не задан
        /// </summary>
        No = 0,
        /// <summary>
        /// Не использовать сокращения
        /// </summary>
        FullwordsOnly = 1,
        /// <summary>
        /// Рассматривать только варианты из морфологического словаря
        /// </summary>
        InDictionaryOnly = 2,
        /// <summary>
        /// Игнорировать морфологические варианты, а брать только термы (TextToken.Term)
        /// </summary>
        TermOnly = 4,
        /// <summary>
        /// Может иметь географический объект в середине (Министерство РФ по делам ...) - игнорируем его при привязке!
        /// </summary>
        CanBeGeoObject = 8,
        /// <summary>
        /// Игнорировать скобки внутри нескольких термов
        /// </summary>
        IgnoreBrackets = 0x10,
        /// <summary>
        /// Игнорировать знаки препинания, числа, союзы и предлоги
        /// </summary>
        IgnoreStopWords = 0x20,
    }
}