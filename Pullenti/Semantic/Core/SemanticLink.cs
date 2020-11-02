/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Semantic.Core
{
    /// <summary>
    /// Семантическая связь двух элементов
    /// </summary>
    public class SemanticLink : IComparable<SemanticLink>
    {
        /// <summary>
        /// Основной элемент (м.б. глагольная группа VerbPhraseToken или 
        /// именная группа NounPhraseToken)
        /// </summary>
        public Pullenti.Ner.MetaToken Master;
        /// <summary>
        /// Пристыкованный элемент (м.б. именная или глагольная группа)
        /// </summary>
        public Pullenti.Ner.MetaToken Slave;
        /// <summary>
        /// Вопрос, на который отвечает связь
        /// </summary>
        public Pullenti.Semantic.Utils.ControlModelQuestion Question;
        /// <summary>
        /// Семантическая роль
        /// </summary>
        public SemanticRole Role = SemanticRole.Common;
        /// <summary>
        /// Страдательный залог или возвратный глагол (роли Агент и Пациент подлежит коррекции)
        /// </summary>
        public bool IsPassive = false;
        /// <summary>
        /// Сила связи (для сравнения и выбора лучшей)
        /// </summary>
        public double Rank = 0;
        /// <summary>
        /// Эта связь смоделирована, так как не нашлась подходящая модель управления основного элемента
        /// </summary>
        public bool Modelled;
        /// <summary>
        /// Идиомная связь (то есть устойчивое словосочетание)
        /// </summary>
        public bool Idiom;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Modelled) 
                res.Append("?");
            if (Idiom) 
                res.Append("!");
            if (Role != SemanticRole.Common) 
                res.AppendFormat("{0}: ", Role);
            if (IsPassive) 
                res.Append("Passive ");
            if (Rank > 0) 
                res.AppendFormat("{0} ", Rank);
            if (Question != null) 
                res.AppendFormat("{0}? ", Question.SpellingEx);
            res.AppendFormat("[{0}] <- [{1}]", (Master == null ? "?" : Master.ToString()), (Slave == null ? "?" : Slave.ToString()));
            return res.ToString();
        }
        public int CompareTo(SemanticLink other)
        {
            if (Rank > other.Rank) 
                return -1;
            if (Rank < other.Rank) 
                return 1;
            return 0;
        }
    }
}