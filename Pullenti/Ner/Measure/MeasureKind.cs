/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Measure
{
    /// <summary>
    /// Что измеряется этой величиной
    /// </summary>
    public enum MeasureKind : int
    {
        Undefined,
        /// <summary>
        /// Время
        /// </summary>
        Time,
        /// <summary>
        /// Длина
        /// </summary>
        Length,
        /// <summary>
        /// Площадь
        /// </summary>
        Area,
        /// <summary>
        /// Объём
        /// </summary>
        Volume,
        /// <summary>
        /// Вес
        /// </summary>
        Weight,
        /// <summary>
        /// Скорость
        /// </summary>
        Speed,
        /// <summary>
        /// Температура
        /// </summary>
        Temperature,
        /// <summary>
        /// Класс защиты
        /// </summary>
        Ip,
        /// <summary>
        /// Процент
        /// </summary>
        Percent,
    }
}