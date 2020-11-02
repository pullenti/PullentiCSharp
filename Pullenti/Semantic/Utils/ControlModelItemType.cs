/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic.Utils
{
    /// <summary>
    /// Тип элемента модели управления
    /// </summary>
    public enum ControlModelItemType : int
    {
        Undefined = 0,
        /// <summary>
        /// Конкретное слово (не относится ко всем остальным)
        /// </summary>
        Word = 1,
        /// <summary>
        /// Все глаголы (не Reflexive)
        /// </summary>
        Verb = 2,
        /// <summary>
        /// Возвратные глаголы и страдательный залог
        /// </summary>
        Reflexive = 3,
        /// <summary>
        /// Существительное, которое можно отглаголить
        /// </summary>
        Noun = 4,
    }
}