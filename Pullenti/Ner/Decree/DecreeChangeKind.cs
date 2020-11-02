/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Decree
{
    /// <summary>
    /// Типы изменений структурных элементов (СЭ)
    /// </summary>
    public enum DecreeChangeKind : int
    {
        Undefined,
        /// <summary>
        /// Объединяет в себе другие изменения
        /// </summary>
        Container,
        /// <summary>
        /// Дополнить другим СЭ-м или текстовыми конструкциями
        /// </summary>
        Append,
        /// <summary>
        /// СЭ утратил силу
        /// </summary>
        Expire,
        /// <summary>
        /// Изложить в редакции
        /// </summary>
        New,
        /// <summary>
        /// Заменить одни текстовые конструкции другими
        /// </summary>
        Exchange,
        /// <summary>
        /// Удалить текстовые конструкции
        /// </summary>
        Remove,
        /// <summary>
        /// Считать как
        /// </summary>
        Consider,
    }
}