/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Semantic.Utils
{
    /// <summary>
    /// Вопрос модели управления
    /// </summary>
    public class ControlModelQuestion
    {
        /// <summary>
        /// Тип вопроса
        /// </summary>
        public QuestionType Question = QuestionType.Undefined;
        /// <summary>
        /// Предлог (если есть)
        /// </summary>
        public string Preposition;
        /// <summary>
        /// Падеж
        /// </summary>
        public Pullenti.Morph.MorphCase Case;
        /// <summary>
        /// Краткое написание вопроса
        /// </summary>
        public string Spelling;
        /// <summary>
        /// Расширенное написание вопроса
        /// </summary>
        public string SpellingEx;
        public int Id;
        /// <summary>
        /// Признак вопроса базовой части модели
        /// </summary>
        public bool IsBase;
        /// <summary>
        /// Это абстрактные вопросы где, куда, откуда, когда
        /// </summary>
        public bool IsAbstract;
        public override string ToString()
        {
            return Spelling;
        }
        /// <summary>
        /// Проверить на соответствие вопросу предлога с падежом
        /// </summary>
        /// <param name="prep">предлог</param>
        /// <param name="cas">падеж</param>
        /// <return>да-нет</return>
        public bool Check(string prep, Pullenti.Morph.MorphCase cas)
        {
            if (IsAbstract) 
            {
                foreach (ControlModelQuestion it in Items) 
                {
                    if (!it.IsAbstract && it.Question == this.Question) 
                    {
                        if (it.Check(prep, cas)) 
                            return true;
                    }
                }
                return false;
            }
            if (((cas & Case)).IsUndefined) 
            {
                if (Preposition == "В" && prep == Preposition) 
                {
                    if (Case.IsAccusative) 
                    {
                        if (cas.IsUndefined || cas.IsNominative) 
                            return true;
                    }
                }
                return false;
            }
            if (prep != null && Preposition != null) 
            {
                if (prep == Preposition) 
                    return true;
                if (Preposition == "ОТ" && prep == "ОТ ИМЕНИ") 
                    return true;
            }
            return string.IsNullOrEmpty(prep) && string.IsNullOrEmpty(Preposition);
        }
        public ControlModelQuestion CheckAbstract(string prep, Pullenti.Morph.MorphCase cas)
        {
            foreach (ControlModelQuestion it in Items) 
            {
                if (!it.IsAbstract && it.Question == this.Question) 
                {
                    if (it.Check(prep, cas)) 
                        return it;
                }
            }
            return null;
        }
        private ControlModelQuestion(string prep, Pullenti.Morph.MorphCase cas, QuestionType typ = QuestionType.Undefined)
        {
            Preposition = prep;
            Case = cas;
            Question = typ;
            if (prep != null) 
            {
                if (cas.IsGenitive) 
                    Spelling = string.Format("{0} чего", prep.ToLower());
                else if (cas.IsDative) 
                    Spelling = string.Format("{0} чему", prep.ToLower());
                else if (cas.IsAccusative) 
                    Spelling = string.Format("{0} что", prep.ToLower());
                else if (cas.IsInstrumental) 
                    Spelling = string.Format("{0} чем", prep.ToLower());
                else if (cas.IsPrepositional) 
                    Spelling = string.Format("{0} чём", prep.ToLower());
                SpellingEx = Spelling;
                if (typ == QuestionType.When) 
                    SpellingEx = string.Format("{0}/когда", Spelling);
                else if (typ == QuestionType.Where) 
                    SpellingEx = string.Format("{0}/где", Spelling);
                else if (typ == QuestionType.WhereFrom) 
                    SpellingEx = string.Format("{0}/откуда", Spelling);
                else if (typ == QuestionType.WhereTo) 
                    SpellingEx = string.Format("{0}/куда", Spelling);
            }
            else if (cas != null) 
            {
                if (cas.IsNominative) 
                {
                    Spelling = "кто";
                    SpellingEx = "кто/что";
                }
                else if (cas.IsGenitive) 
                {
                    Spelling = "чего";
                    SpellingEx = "кого/чего";
                }
                else if (cas.IsDative) 
                {
                    Spelling = "чему";
                    SpellingEx = "кому/чему";
                }
                else if (cas.IsAccusative) 
                {
                    Spelling = "что";
                    SpellingEx = "кого/что";
                }
                else if (cas.IsInstrumental) 
                {
                    Spelling = "чем";
                    SpellingEx = "кем/чем";
                }
            }
            else if (typ == QuestionType.WhatToDo) 
            {
                Spelling = "что делать";
                SpellingEx = "что делать";
            }
            else if (typ == QuestionType.When) 
            {
                Spelling = "когда";
                SpellingEx = "когда";
            }
            else if (typ == QuestionType.Where) 
            {
                Spelling = "где";
                SpellingEx = "где";
            }
            else if (typ == QuestionType.WhereFrom) 
            {
                Spelling = "откуда";
                SpellingEx = "откуда";
            }
            else if (typ == QuestionType.WhereTo) 
            {
                Spelling = "куда";
                SpellingEx = "куда";
            }
        }
        /// <summary>
        /// Вопрос "кто-что"
        /// </summary>
        public static ControlModelQuestion BaseNominative
        {
            get
            {
                return Items[m_BaseNominativeInd];
            }
        }
        static int m_BaseNominativeInd;
        /// <summary>
        /// Вопрос "кого-чего"
        /// </summary>
        public static ControlModelQuestion BaseGenetive
        {
            get
            {
                return Items[m_BaseGenetiveInd];
            }
        }
        static int m_BaseGenetiveInd;
        /// <summary>
        /// Вопрос "кого-что"
        /// </summary>
        public static ControlModelQuestion BaseAccusative
        {
            get
            {
                return Items[m_BaseAccusativeInd];
            }
        }
        static int m_BaseAccusativeInd;
        /// <summary>
        /// Вопрос "кем-чем"
        /// </summary>
        public static ControlModelQuestion BaseInstrumental
        {
            get
            {
                return Items[m_BaseInstrumentalInd];
            }
        }
        static int m_BaseInstrumentalInd;
        /// <summary>
        /// Вопрос "кому-чему"
        /// </summary>
        public static ControlModelQuestion BaseDative
        {
            get
            {
                return Items[m_BaseDativeInd];
            }
        }
        static int m_BaseDativeInd;
        /// <summary>
        /// Вопрос "что делать"
        /// </summary>
        public static ControlModelQuestion ToDo
        {
            get
            {
                return Items[m_BaseToDoInd];
            }
        }
        static int m_BaseToDoInd;
        /// <summary>
        /// Список всех вопросов ControlModelQuestion
        /// </summary>
        public static List<ControlModelQuestion> Items;
        static Dictionary<string, int> m_HashBySpel;
        public static void Initialize()
        {
            if (Items != null) 
                return;
            Items = new List<ControlModelQuestion>();
            foreach (string s in new string[] {"ИЗ", "ОТ", "С", "ИЗНУТРИ"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Genitive, QuestionType.WhereFrom));
            }
            Items.Add(new ControlModelQuestion("В", Pullenti.Morph.MorphCase.Accusative, QuestionType.WhereTo));
            Items.Add(new ControlModelQuestion("НА", Pullenti.Morph.MorphCase.Accusative, QuestionType.WhereTo));
            Items.Add(new ControlModelQuestion("ПО", Pullenti.Morph.MorphCase.Accusative, QuestionType.WhereTo));
            Items.Add(new ControlModelQuestion("К", Pullenti.Morph.MorphCase.Dative, QuestionType.WhereTo));
            Items.Add(new ControlModelQuestion("НАВСТРЕЧУ", Pullenti.Morph.MorphCase.Dative, QuestionType.WhereTo));
            Items.Add(new ControlModelQuestion("ДО", Pullenti.Morph.MorphCase.Genitive, QuestionType.WhereTo));
            foreach (string s in new string[] {"У", "ОКОЛО", "ВОКРУГ", "ВОЗЛЕ", "ВБЛИЗИ", "МИМО", "ПОЗАДИ", "ВПЕРЕДИ", "ВГЛУБЬ", "ВДОЛЬ", "ВНЕ", "КРОМЕ", "МЕЖДУ", "НАПРОТИВ", "ПОВЕРХ", "ПОДЛЕ", "ПОПЕРЕК", "ПОСЕРЕДИНЕ", "СВЕРХ", "СРЕДИ", "СНАРУЖИ", "ВНУТРИ"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Genitive, QuestionType.Where));
            }
            foreach (string s in new string[] {"ПАРАЛЛЕЛЬНО"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Dative, QuestionType.Where));
            }
            foreach (string s in new string[] {"СКВОЗЬ", "ЧЕРЕЗ", "ПОД"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Accusative, QuestionType.Where));
            }
            foreach (string s in new string[] {"МЕЖДУ", "НАД", "ПОД", "ПЕРЕД", "ЗА"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Instrumental, QuestionType.Where));
            }
            foreach (string s in new string[] {"В", "НА", "ПРИ"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Prepositional, QuestionType.Where));
            }
            Items.Add(new ControlModelQuestion("ПРЕЖДЕ", Pullenti.Morph.MorphCase.Genitive, QuestionType.When));
            Items.Add(new ControlModelQuestion("ПОСЛЕ", Pullenti.Morph.MorphCase.Genitive, QuestionType.When));
            Items.Add(new ControlModelQuestion("НАКАНУНЕ", Pullenti.Morph.MorphCase.Genitive, QuestionType.When));
            Items.Add(new ControlModelQuestion("СПУСТЯ", Pullenti.Morph.MorphCase.Accusative, QuestionType.When));
            foreach (string s in new string[] {"БЕЗ", "ДЛЯ", "РАДИ", "ИЗЗА", "ВВИДУ", "ВЗАМЕН", "ВМЕСТО", "ПРОТИВ", "СВЫШЕ", "ВСЛЕДСТВИЕ", "ПОМИМО", "ПОСРЕДСТВОМ", "ПУТЕМ"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Genitive));
            }
            foreach (string s in new string[] {"ПО", "ПОДОБНО", "СОГЛАСНО", "СООТВЕТСТВЕННО", "СОРАЗМЕРНО", "ВОПРЕКИ"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Dative));
            }
            foreach (string s in new string[] {"ПРО", "О", "ЗА", "ВКЛЮЧАЯ", "С"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Accusative));
            }
            foreach (string s in new string[] {"С"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Instrumental));
            }
            foreach (string s in new string[] {"О", "ПО"}) 
            {
                Items.Add(new ControlModelQuestion(s, Pullenti.Morph.MorphCase.Prepositional));
            }
            for (int i = 0; i < Items.Count; i++) 
            {
                for (int j = 0; j < (Items.Count - 1); j++) 
                {
                    if (Items[j].CompareTo(Items[j + 1]) > 0) 
                    {
                        ControlModelQuestion it = Items[j];
                        Items[j] = Items[j + 1];
                        Items[j + 1] = it;
                    }
                }
            }
            Items.Insert((m_BaseNominativeInd = 0), new ControlModelQuestion(null, Pullenti.Morph.MorphCase.Nominative) { IsBase = true });
            Items.Insert((m_BaseGenetiveInd = 1), new ControlModelQuestion(null, Pullenti.Morph.MorphCase.Genitive) { IsBase = true });
            Items.Insert((m_BaseAccusativeInd = 2), new ControlModelQuestion(null, Pullenti.Morph.MorphCase.Accusative) { IsBase = true });
            Items.Insert((m_BaseInstrumentalInd = 3), new ControlModelQuestion(null, Pullenti.Morph.MorphCase.Instrumental) { IsBase = true });
            Items.Insert((m_BaseDativeInd = 4), new ControlModelQuestion(null, Pullenti.Morph.MorphCase.Dative) { IsBase = true });
            Items.Insert((m_BaseToDoInd = 5), new ControlModelQuestion(null, null, QuestionType.WhatToDo));
            Items.Insert(6, new ControlModelQuestion(null, null, QuestionType.Where) { IsAbstract = true });
            Items.Insert(7, new ControlModelQuestion(null, null, QuestionType.WhereTo) { IsAbstract = true });
            Items.Insert(8, new ControlModelQuestion(null, null, QuestionType.WhereFrom) { IsAbstract = true });
            Items.Insert(9, new ControlModelQuestion(null, null, QuestionType.When) { IsAbstract = true });
            m_HashBySpel = new Dictionary<string, int>();
            for (int i = 0; i < Items.Count; i++) 
            {
                ControlModelQuestion it = Items[i];
                it.Id = i + 1;
                m_HashBySpel.Add(it.Spelling, i);
            }
        }
        int CompareTo(ControlModelQuestion other)
        {
            int i = string.Compare(Preposition, other.Preposition);
            if (i != 0) 
                return i;
            if (this._casRank() < other._casRank()) 
                return -1;
            if (this._casRank() > other._casRank()) 
                return 1;
            return 0;
        }
        int _casRank()
        {
            if (Case.IsGenitive) 
                return 1;
            if (Case.IsDative) 
                return 2;
            if (Case.IsAccusative) 
                return 3;
            if (Case.IsInstrumental) 
                return 4;
            if (Case.IsPrepositional) 
                return 5;
            return 0;
        }
        public static ControlModelQuestion GetById(int id)
        {
            if (id >= 1 && id <= Items.Count) 
                return Items[id - 1];
            return null;
        }
        public static ControlModelQuestion FindBySpel(string spel)
        {
            int ind;
            if (!m_HashBySpel.TryGetValue(spel, out ind)) 
                return null;
            return Items[ind];
        }
    }
}