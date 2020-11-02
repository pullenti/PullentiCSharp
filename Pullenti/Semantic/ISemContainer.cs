/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Интерфейс владельца семантического графа
    /// </summary>
    public interface ISemContainer
    {
        /// <summary>
        /// Сам граф объектов и связей
        /// </summary>
        SemGraph Graph { get; }
        /// <summary>
        /// Вышестоящий элемент
        /// </summary>
        ISemContainer Higher { get; }
        /// <summary>
        /// Начальная позиция в тексте
        /// </summary>
        int BeginChar { get; }
        /// <summary>
        /// Конечная позиция в тексте
        /// </summary>
        int EndChar { get; }
    }
}