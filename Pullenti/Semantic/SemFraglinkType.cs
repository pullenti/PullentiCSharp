/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Тип связи между фрагментами
    /// </summary>
    public enum SemFraglinkType : int
    {
        /// <summary>
        /// Не определён
        /// </summary>
        Undefined,
        /// <summary>
        /// Если - то
        /// </summary>
        IfThen,
        /// <summary>
        /// Если - иначе
        /// </summary>
        IfElse,
        /// <summary>
        /// Потому что
        /// </summary>
        Because,
        /// <summary>
        /// Но (..., однако ...)
        /// </summary>
        But,
        /// <summary>
        /// Для того, чтобы...
        /// </summary>
        For,
        /// <summary>
        /// Что
        /// </summary>
        What,
    }
}