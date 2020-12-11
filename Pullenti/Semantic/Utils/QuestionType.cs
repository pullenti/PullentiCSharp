/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic.Utils
{
    /// <summary>
    /// Абстрактные вопросы модели управления
    /// </summary>
    public enum QuestionType : int
    {
        /// <summary>
        /// Обычный вопрос
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Где
        /// </summary>
        Where = 1,
        /// <summary>
        /// Откуда
        /// </summary>
        WhereFrom = 2,
        /// <summary>
        /// Куда
        /// </summary>
        WhereTo = 4,
        /// <summary>
        /// Когда
        /// </summary>
        When = 8,
        /// <summary>
        /// Что делать (инфинитив за группой)
        /// </summary>
        WhatToDo = 0x10,
    }
}