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
    /// Атрибуты получения текста методами GetTextValue и GetTextValueOfMetaToken класса MiscHelper. Битовая маска.
    /// </summary>
    public enum GetTextAttr : int
    {
        /// <summary>
        /// Не задано
        /// </summary>
        No = 0,
        /// <summary>
        /// Сохранять ли регистр букв (по умолчанию, верхний регистр)
        /// </summary>
        KeepRegister = 1,
        /// <summary>
        /// Первую именную группу преобразовывать к именительному падежу
        /// </summary>
        FirstNounGroupToNominative = 2,
        /// <summary>
        /// Первую именную группу преобразовывать к именительному падежу единственному числу
        /// </summary>
        FirstNounGroupToNominativeSingle = 4,
        /// <summary>
        /// Оставлять кавычки (по умолчанию, кавычки игнорируются). К скобкам это не относится.
        /// </summary>
        KeepQuotes = 8,
        /// <summary>
        /// Игнорировать географические объекты
        /// </summary>
        IgnoreGeoReferent = 0x10,
        /// <summary>
        /// Преобразовать ли числовые значения в цифры
        /// </summary>
        NormalizeNumbers = 0x20,
        /// <summary>
        /// Если все слова в верхнем регистре, то попытаться восстановить слова в нижнем регистре 
        /// на основе их встречаемости в других частях всего документа 
        /// (то есть если слово есть в нижнем, то оно переводится в нижний)
        /// </summary>
        RestoreRegister = 0x40,
        /// <summary>
        /// Для английского языка игнорировать артикли и суффикс 'S
        /// </summary>
        IgnoreArticles = 0x80,
    }
}