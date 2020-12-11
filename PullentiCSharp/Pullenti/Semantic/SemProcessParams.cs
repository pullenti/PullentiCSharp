/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.ComponentModel;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Дополнительные параметры
    /// </summary>
    public class SemProcessParams
    {
        /// <summary>
        /// Не делать анафору, оставлять всё как есть
        /// </summary>
        public bool DontCreateAnafor;
        /// <summary>
        /// Максимальнкая длина (дальше этого символа обработки не будет)
        /// </summary>
        public int MaxChar = 0;
        /// <summary>
        /// Для реализации бегущей строки
        /// </summary>
        public ProgressChangedEventHandler Progress = null;
    }
}