/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Статистическая информация о токене - возвращается StatisticCollection.GetWordInfo
    /// </summary>
    public class StatisticWordInfo
    {
        /// <summary>
        /// Нормализация
        /// </summary>
        public string Normal;
        public override string ToString()
        {
            return Normal;
        }
        /// <summary>
        /// Общее число вхождений в тексте
        /// </summary>
        public int TotalCount;
        /// <summary>
        /// Число вхождений словоформы написаний в нижнем регистре
        /// </summary>
        public int LowerCount;
        /// <summary>
        /// Число вхождений словоформы написаний в верхнем регистре
        /// </summary>
        public int UpperCount;
        /// <summary>
        /// Число вхождений словоформы написаний с заглавной буквой
        /// </summary>
        public int CapitalCount;
        /// <summary>
        /// Сколько раз за словом идёт глагол мужского рода
        /// </summary>
        public int MaleVerbsAfterCount;
        /// <summary>
        /// Сколько раз за словом идёт глагол женского рода
        /// </summary>
        public int FemaleVerbsAfterCount;
        /// <summary>
        /// Есть ли перед атрибут персон (вычисляется только в процессе отработки соотв. анализатора на его 1-м проходе)
        /// </summary>
        public bool HasBeforePersonAttr;
        /// <summary>
        /// Количество слов перед этим, которые не тексты или в нижнем регистре 
        /// (например, для проверки отчеств - фамилия ли это)
        /// </summary>
        public int NotCapitalBeforeCount;
        public Dictionary<StatisticWordInfo, int> LikeCharsBeforeWords;
        public Dictionary<StatisticWordInfo, int> LikeCharsAfterWords;
        public void AddBefore(StatisticWordInfo w)
        {
            if (LikeCharsBeforeWords == null) 
                LikeCharsBeforeWords = new Dictionary<StatisticWordInfo, int>();
            if (!LikeCharsBeforeWords.ContainsKey(w)) 
                LikeCharsBeforeWords.Add(w, 1);
            else 
                LikeCharsBeforeWords[w]++;
        }
        public void AddAfter(StatisticWordInfo w)
        {
            if (LikeCharsAfterWords == null) 
                LikeCharsAfterWords = new Dictionary<StatisticWordInfo, int>();
            if (!LikeCharsAfterWords.ContainsKey(w)) 
                LikeCharsAfterWords.Add(w, 1);
            else 
                LikeCharsAfterWords[w]++;
        }
    }
}