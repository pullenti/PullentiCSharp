/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic.Core
{
    /// <summary>
    /// Интерфейс внешней дополнительной онтологии 
    /// (для улучшения качества семантичсекой обработки)
    /// </summary>
    public interface ISemanticOnto
    {
        /// <summary>
        /// Проверка, что в онтологии слова master и slave образуют устойчивую пару
        /// </summary>
        bool CheckLink(string master, string slave);
    }
}