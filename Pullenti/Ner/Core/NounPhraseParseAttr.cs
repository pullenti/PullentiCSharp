/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Атрибуты выделения именных групп NounPhraseHelper.TryParse(). Битовая маска.
    /// </summary>
    public enum NounPhraseParseAttr : int
    {
        /// <summary>
        /// Нет атрибута
        /// </summary>
        No = 0,
        /// <summary>
        /// Выделять ли местоимения (моя страна)
        /// </summary>
        ParsePronouns = 1,
        /// <summary>
        /// Выделять ли в начале предлог
        /// </summary>
        ParsePreposition = 2,
        /// <summary>
        /// Игнорировать прилагательные превосходной степени
        /// </summary>
        IgnoreAdjBest = 4,
        /// <summary>
        /// Игнорировать причастия, брать только чистые прилагательные
        /// </summary>
        IgnoreParticiples = 8,
        /// <summary>
        /// Корнем группы может выступать сущность (необъятная Россия)
        /// </summary>
        ReferentCanBeNoun = 0x10,
        /// <summary>
        /// Между прилагательными не должно быть запятых и союзов
        /// </summary>
        CanNotHasCommaAnd = 0x20,
        /// <summary>
        /// Прилагательное м.б. на последнем месте (член моржовый)
        /// </summary>
        AdjectiveCanBeLast = 0x40,
        /// <summary>
        /// Выделять наречия
        /// </summary>
        ParseAdverbs = 0x80,
        /// <summary>
        /// Выделять причастия (это прилагательные и глаголы одновременно)
        /// </summary>
        ParseVerbs = 0x100,
        /// <summary>
        /// Выделять ли такие конструкции, как "двое сотрудников", "пять компаний" числа как прилагательные. 
        /// Это не касается ситуаций "второй сотрудник", "пятая компания" - это всегда как прилагательные.
        /// </summary>
        ParseNumericAsAdjective = 0x200,
        /// <summary>
        /// Группа может располагаться на нескольких строках (начало на одной, окончание на другой)
        /// </summary>
        Multilines = 0x400,
        /// <summary>
        /// Игнорировать содержимое в скобках (...) внутри именной группы
        /// </summary>
        IgnoreBrackets = 0x800,
        /// <summary>
        /// Это для случая "грузовой и легковой автомобили" - то есть прилагательные 
        /// относятся к одному существительному (как бы слепленному). См. NounPhraseMultivarToken.
        /// </summary>
        MultiNouns = 0x1000,
    }
}