/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Transport
{
    /// <summary>
    /// Категории транспортных средств
    /// </summary>
    public enum TransportKind : int
    {
        /// <summary>
        /// Неопределено
        /// </summary>
        Undefined,
        /// <summary>
        /// Автомобильные
        /// </summary>
        Auto,
        /// <summary>
        /// Железнодорожные
        /// </summary>
        Train,
        /// <summary>
        /// Водные
        /// </summary>
        Ship,
        /// <summary>
        /// Воздушние
        /// </summary>
        Fly,
        /// <summary>
        /// Космические
        /// </summary>
        Space,
    }
}