/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Атрибуты функции CanBeEqualsEx класса MiscHelper. Битовая маска.
    /// </summary>
    public enum CanBeEqualsAttr : int
    {
        No = 0,
        /// <summary>
        /// Игнорировать небуквенные символы (они как бы выбрасываются)
        /// </summary>
        IgnoreNonletters = 1,
        /// <summary>
        /// Игнорировать регистр символов
        /// </summary>
        IgnoreUppercase = 2,
        /// <summary>
        /// После первого существительного слова должны полностью совпадать 
        /// (иначе совпадение с точностью до морфологии)
        /// </summary>
        CheckMorphEquAfterFirstNoun = 4,
        /// <summary>
        /// Даже если указано IgnoreNonletters, кавычки проверять!
        /// </summary>
        UseBrackets = 8,
        /// <summary>
        /// Игнорировать регистр символов только первого слова
        /// </summary>
        IgnoreUppercaseFirstWord = 0x10,
        /// <summary>
        /// Первое слово может быть короче (то есть второе должно начинаться на первое слово)
        /// </summary>
        FirstCanBeShorter = 0x20,
    }
}