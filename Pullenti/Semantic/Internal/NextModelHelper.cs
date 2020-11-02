/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Semantic.Internal
{
    public static class NextModelHelper
    {
        public static List<NextModelItem> Items;
        public static void Initialize()
        {
            if (Items != null) 
                return;
            Items = new List<NextModelItem>();
            Items.Add(new NextModelItem("", Pullenti.Morph.MorphCase.Nominative));
            Items.Add(new NextModelItem("", Pullenti.Morph.MorphCase.Genitive));
            Items.Add(new NextModelItem("", Pullenti.Morph.MorphCase.Dative));
            Items.Add(new NextModelItem("", Pullenti.Morph.MorphCase.Accusative));
            Items.Add(new NextModelItem("", Pullenti.Morph.MorphCase.Instrumental));
            Items.Add(new NextModelItem("", Pullenti.Morph.MorphCase.Prepositional));
            foreach (string s in new string[] {"ИЗ", "ОТ", "С", "ИЗНУТРИ"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Genitive, null, Pullenti.Semantic.Utils.QuestionType.WhereFrom));
            }
            Items.Add(new NextModelItem("В", Pullenti.Morph.MorphCase.Accusative, null, Pullenti.Semantic.Utils.QuestionType.WhereTo));
            Items.Add(new NextModelItem("НА", Pullenti.Morph.MorphCase.Accusative, null, Pullenti.Semantic.Utils.QuestionType.WhereTo));
            Items.Add(new NextModelItem("ПО", Pullenti.Morph.MorphCase.Accusative, null, Pullenti.Semantic.Utils.QuestionType.WhereTo));
            Items.Add(new NextModelItem("К", Pullenti.Morph.MorphCase.Dative, null, Pullenti.Semantic.Utils.QuestionType.WhereTo));
            Items.Add(new NextModelItem("НАВСТРЕЧУ", Pullenti.Morph.MorphCase.Dative, null, Pullenti.Semantic.Utils.QuestionType.WhereTo));
            Items.Add(new NextModelItem("ДО", Pullenti.Morph.MorphCase.Genitive, null, Pullenti.Semantic.Utils.QuestionType.WhereTo));
            foreach (string s in new string[] {"У", "ОКОЛО", "ВОКРУГ", "ВОЗЛЕ", "ВБЛИЗИ", "МИМО", "ПОЗАДИ", "ВПЕРЕДИ", "ВГЛУБЬ", "ВДОЛЬ", "ВНЕ", "КРОМЕ", "МЕЖДУ", "НАПРОТИВ", "ПОВЕРХ", "ПОДЛЕ", "ПОПЕРЕК", "ПОСЕРЕДИНЕ", "СВЕРХ", "СРЕДИ", "СНАРУЖИ", "ВНУТРИ"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Genitive, null, Pullenti.Semantic.Utils.QuestionType.Where));
            }
            foreach (string s in new string[] {"ПАРАЛЛЕЛЬНО"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Dative, null, Pullenti.Semantic.Utils.QuestionType.Where));
            }
            foreach (string s in new string[] {"СКВОЗЬ", "ЧЕРЕЗ", "ПОД"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Accusative, null, Pullenti.Semantic.Utils.QuestionType.Where));
            }
            foreach (string s in new string[] {"МЕЖДУ", "НАД", "ПОД", "ПЕРЕД", "ЗА"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Instrumental, null, Pullenti.Semantic.Utils.QuestionType.Where));
            }
            foreach (string s in new string[] {"В", "НА", "ПРИ"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Prepositional, null, Pullenti.Semantic.Utils.QuestionType.Where));
            }
            Items.Add(new NextModelItem("ПРЕЖДЕ", Pullenti.Morph.MorphCase.Genitive, null, Pullenti.Semantic.Utils.QuestionType.When));
            Items.Add(new NextModelItem("ПОСЛЕ", Pullenti.Morph.MorphCase.Genitive, null, Pullenti.Semantic.Utils.QuestionType.When));
            Items.Add(new NextModelItem("НАКАНУНЕ", Pullenti.Morph.MorphCase.Genitive, null, Pullenti.Semantic.Utils.QuestionType.When));
            Items.Add(new NextModelItem("СПУСТЯ", Pullenti.Morph.MorphCase.Accusative, null, Pullenti.Semantic.Utils.QuestionType.When));
            foreach (string s in new string[] {"БЕЗ", "ДЛЯ", "РАДИ", "ИЗЗА", "ВВИДУ", "ВЗАМЕН", "ВМЕСТО", "ПРОТИВ", "СВЫШЕ", "ВСЛЕДСТВИЕ", "ПОМИМО", "ПОСРЕДСТВОМ"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Genitive));
            }
            foreach (string s in new string[] {"ПО", "ПОДОБНО", "СОГЛАСНО", "СООТВЕТСТВЕННО", "СОРАЗМЕРНО", "ВОПРЕКИ"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Dative));
            }
            foreach (string s in new string[] {"ПРО", "О", "ЗА", "ВКЛЮЧАЯ", "С"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Accusative));
            }
            foreach (string s in new string[] {"С"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Instrumental));
            }
            foreach (string s in new string[] {"О", "ПО"}) 
            {
                Items.Add(new NextModelItem(s, Pullenti.Morph.MorphCase.Prepositional));
            }
            for (int i = 0; i < Items.Count; i++) 
            {
                for (int j = 0; j < (Items.Count - 1); j++) 
                {
                    if (Items[j].CompareTo(Items[j + 1]) > 0) 
                    {
                        NextModelItem it = Items[j];
                        Items[j] = Items[j + 1];
                        Items[j + 1] = it;
                    }
                }
            }
            for (int i = 0; i < Items.Count; i++) 
            {
                Items[i].Id = i + 1;
            }
        }
    }
}