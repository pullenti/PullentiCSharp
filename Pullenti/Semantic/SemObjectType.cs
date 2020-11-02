/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Тип семантического объекта
    /// </summary>
    public enum SemObjectType : int
    {
        Undefined,
        /// <summary>
        /// Существительное (в широком смысле, например, сущности)
        /// </summary>
        Noun,
        /// <summary>
        /// Прилагательное
        /// </summary>
        Adjective,
        /// <summary>
        /// Предикат (глагол)
        /// </summary>
        Verb,
        /// <summary>
        /// Причастие или деепричастие
        /// </summary>
        Participle,
        /// <summary>
        /// Наречие
        /// </summary>
        Adverb,
        /// <summary>
        /// Местоимение
        /// </summary>
        Pronoun,
        /// <summary>
        /// Личное местоимение
        /// </summary>
        PersonalPronoun,
        /// <summary>
        /// Вопрос
        /// </summary>
        Question,
    }
}