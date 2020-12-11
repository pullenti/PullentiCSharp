/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Статистическая информация о биграмме - возвращается StatisticCollection.GetBigrammInfo
    /// </summary>
    public class StatisticBigrammInfo
    {
        /// <summary>
        /// Сколько всего первых словоформ в тексте
        /// </summary>
        public int FirstCount;
        /// <summary>
        /// Сколько всего вторых словоформ в тексте
        /// </summary>
        public int SecondCount;
        /// <summary>
        /// Сколько всего они встречаются вместе
        /// </summary>
        public int PairCount;
        /// <summary>
        /// Признак, что первая словоформа имеет и другие продолжения
        /// </summary>
        public bool FirstHasOtherSecond;
        /// <summary>
        /// Признак, что вторая словоформа имеет перед собой и другие слова
        /// </summary>
        public bool SecondHasOtherFirst;
    }
}