/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner
{
    /// <summary>
    /// Тип написания числительного NumberToken
    /// </summary>
    public enum NumberSpellingType : int
    {
        /// <summary>
        /// Цифрами
        /// </summary>
        Digit = 0,
        /// <summary>
        /// Римскими цифрами
        /// </summary>
        Roman = 1,
        /// <summary>
        /// Прописью (словами)
        /// </summary>
        Words = 2,
        /// <summary>
        /// Возраст (летие)
        /// </summary>
        Age = 3,
    }
}