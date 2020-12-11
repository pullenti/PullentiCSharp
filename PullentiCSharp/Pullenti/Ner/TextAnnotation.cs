/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner
{
    /// <summary>
    /// Аннотация слитного фрагмента текста (фрагмент вхождения сущности в текст)
    /// </summary>
    public class TextAnnotation
    {
        public TextAnnotation(Token begin = null, Token end = null, Referent r = null)
        {
            if (begin != null) 
            {
                Sofa = begin.Kit.Sofa;
                BeginChar = begin.BeginChar;
            }
            if (end != null) 
                EndChar = end.EndChar;
            OccurenceOf = r;
        }
        /// <summary>
        /// Ссылка на текст
        /// </summary>
        public SourceOfAnalysis Sofa;
        /// <summary>
        /// Начальная позиция фрагмента
        /// </summary>
        public int BeginChar;
        /// <summary>
        /// Конечная позиция фрагмента
        /// </summary>
        public int EndChar;
        /// <summary>
        /// Ссылка на сущность
        /// </summary>
        public Referent OccurenceOf
        {
            get
            {
                return m_OccurenceOf;
            }
            set
            {
                m_OccurenceOf = value;
            }
        }
        Referent m_OccurenceOf;
        /// <summary>
        /// Указание на то, что текущая сущность была выделена на основе правил 
        /// именно на данном фрагменте текста.
        /// </summary>
        public bool EssentialForOccurence;
        public override string ToString()
        {
            if (Sofa == null) 
                return string.Format("{0}:{1}", BeginChar, EndChar);
            return this.GetText();
        }
        /// <summary>
        /// Извлечь фрагмент исходного текста, соответствующий аннотации
        /// </summary>
        /// <return>фрагмент текста</return>
        public string GetText()
        {
            if (Sofa == null || Sofa.Text == null) 
                return null;
            return Sofa.Text.Substring(BeginChar, (EndChar + 1) - BeginChar);
        }
        internal Pullenti.Ner.Core.Internal.TextsCompareType CompareWith(TextAnnotation loc)
        {
            if (loc.Sofa != Sofa) 
                return Pullenti.Ner.Core.Internal.TextsCompareType.Noncomparable;
            return this.Compare(loc.BeginChar, loc.EndChar);
        }
        internal Pullenti.Ner.Core.Internal.TextsCompareType Compare(int pos, int pos1)
        {
            if (EndChar < pos) 
                return Pullenti.Ner.Core.Internal.TextsCompareType.Early;
            if (pos1 < BeginChar) 
                return Pullenti.Ner.Core.Internal.TextsCompareType.Later;
            if (BeginChar == pos && EndChar == pos1) 
                return Pullenti.Ner.Core.Internal.TextsCompareType.Equivalent;
            if (BeginChar >= pos && EndChar <= pos1) 
                return Pullenti.Ner.Core.Internal.TextsCompareType.In;
            if (pos >= BeginChar && pos1 <= EndChar) 
                return Pullenti.Ner.Core.Internal.TextsCompareType.Contains;
            return Pullenti.Ner.Core.Internal.TextsCompareType.Intersect;
        }
        internal void Merge(TextAnnotation loc)
        {
            if (loc.Sofa != Sofa) 
                return;
            if (loc.BeginChar < BeginChar) 
                BeginChar = loc.BeginChar;
            if (EndChar < loc.EndChar) 
                EndChar = loc.EndChar;
            if (loc.EssentialForOccurence) 
                EssentialForOccurence = true;
        }
        public object Tag;
    }
}