/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Количественная характеристика. Планируется переработка этого класса 
    /// (поддержка сложной модели диапазонов, составных значений и пр.).
    /// </summary>
    public class SemQuantity : Pullenti.Ner.MetaToken
    {
        public SemQuantity(string spelling, Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
            Spelling = spelling;
        }
        /// <summary>
        /// Суммарное написание
        /// </summary>
        public string Spelling;
        public override string ToString()
        {
            return Spelling;
        }
    }
}