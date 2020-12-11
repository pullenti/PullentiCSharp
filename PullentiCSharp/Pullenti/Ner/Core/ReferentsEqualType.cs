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
    /// Атрибут сравнения сущностей (методом Referent.CanBeEquals)
    /// </summary>
    public enum ReferentsEqualType : int
    {
        /// <summary>
        /// Сущности в рамках одного текста
        /// </summary>
        WithinOneText = 0,
        /// <summary>
        /// Сущности из разных текстов
        /// </summary>
        DifferentTexts = 1,
        /// <summary>
        /// Проверка для потенциального объединения сущностей
        /// </summary>
        ForMerging = 2,
    }
}