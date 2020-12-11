/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Семантический объект
    /// </summary>
    public class SemObject : IComparable<SemObject>
    {
        public SemObject(SemGraph graph)
        {
            Graph = graph;
        }
        /// <summary>
        /// Ссылка на граф - владалец объекта.
        /// </summary>
        public SemGraph Graph;
        /// <summary>
        /// Морфологическая информация (падеж отсутствует в принципе), 
        /// часть речи Class тоже не задана - вместо этого поле Typ. 
        /// NormalFull - полная нормализация, NormalCase - только падежная нормализация.
        /// </summary>
        public Pullenti.Morph.MorphWordForm Morph = new Pullenti.Morph.MorphWordForm();
        /// <summary>
        /// Тип (определяется частью речи)
        /// </summary>
        public SemObjectType Typ = SemObjectType.Undefined;
        /// <summary>
        /// Количественная характеристика
        /// </summary>
        public SemQuantity Quantity;
        /// <summary>
        /// Ссылка на концепт - это абстрактное понятие, используется вовне. 
        /// Это и Referent, это и DerivateGroup. В принципе, приложения сами здесь будут расставлять 
        /// свои объекты.
        /// </summary>
        public object Concept;
        /// <summary>
        /// Атрибуты, список SemAttribute. Формируются частично из наречий, частично из служебных частей речи.
        /// </summary>
        public List<SemAttribute> Attrs = new List<SemAttribute>();
        /// <summary>
        /// Ну не знаю, потом нужно будет обобщить и куда-нибудь перенесём
        /// </summary>
        public Pullenti.Ner.Measure.MeasureKind Measure = Pullenti.Ner.Measure.MeasureKind.Undefined;
        /// <summary>
        /// Признак отрицания (потом перенесём в атрибуты)
        /// </summary>
        public bool Not;
        /// <summary>
        /// Токены MetaToken в исходном тексте
        /// </summary>
        public List<Pullenti.Ner.MetaToken> Tokens = new List<Pullenti.Ner.MetaToken>();
        /// <summary>
        /// Начальная позиция первого токена
        /// </summary>
        public int BeginChar
        {
            get
            {
                return (Tokens.Count > 0 ? Tokens[0].BeginChar : 0);
            }
        }
        /// <summary>
        /// Последняя позиция последнего токена
        /// </summary>
        public int EndChar
        {
            get
            {
                return (Tokens.Count > 0 ? Tokens[Tokens.Count - 1].EndChar : 0);
            }
        }
        /// <summary>
        /// Исходящие связи SemLink (текущий объект выступает как Source)
        /// </summary>
        public List<SemLink> LinksFrom = new List<SemLink>();
        /// <summary>
        /// Входящие связи SemLink (текущий объект выступает как Target)
        /// </summary>
        public List<SemLink> LinksTo = new List<SemLink>();
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Not) 
                res.Append("НЕ ");
            foreach (SemAttribute a in Attrs) 
            {
                res.AppendFormat("{0} ", a.ToString().ToLower());
            }
            if (Quantity != null) 
                res.AppendFormat("{0} ", Quantity);
            else if (Morph.Number == Pullenti.Morph.MorphNumber.Plural && Typ == SemObjectType.Noun) 
                res.AppendFormat("* ");
            res.Append(Morph.NormalCase ?? "?");
            return res.ToString();
        }
        public int CompareTo(SemObject other)
        {
            if (Tokens.Count == 0 || other.Tokens.Count == 0) 
                return 0;
            if (Tokens[0].BeginChar < other.Tokens[0].BeginChar) 
                return -1;
            if (Tokens[0].BeginChar > other.Tokens[0].BeginChar) 
                return 1;
            if (Tokens[Tokens.Count - 1].EndChar < other.Tokens[other.Tokens.Count - 1].EndChar) 
                return -1;
            if (Tokens[Tokens.Count - 1].EndChar > other.Tokens[other.Tokens.Count - 1].EndChar) 
                return 1;
            return 0;
        }
        /// <summary>
        /// Проверка значения
        /// </summary>
        public bool IsValue(string word, SemObjectType typ = SemObjectType.Undefined)
        {
            if (typ != SemObjectType.Undefined) 
            {
                if (typ != Typ) 
                    return false;
            }
            if (Morph.NormalFull == word || Morph.NormalCase == word) 
                return true;
            Pullenti.Semantic.Utils.DerivateGroup gr = Concept as Pullenti.Semantic.Utils.DerivateGroup;
            if (gr != null) 
            {
                if (gr.Words[0].Spelling == word) 
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Найти объект, кторый связан с текущим исходящий связью (Source = this)
        /// </summary>
        public SemObject FindFromObject(string word, SemLinkType typ = SemLinkType.Undefined, SemObjectType otyp = SemObjectType.Undefined)
        {
            foreach (SemLink li in LinksFrom) 
            {
                if (typ != SemLinkType.Undefined && typ != li.Typ) 
                    continue;
                if (li.Target.IsValue(word, otyp)) 
                    return li.Target;
            }
            return null;
        }
        /// <summary>
        /// Найти атрибут указанного типа
        /// </summary>
        public SemAttribute FindAttr(SemAttributeType typ)
        {
            foreach (SemAttribute a in Attrs) 
            {
                if (a.Typ == typ) 
                    return a;
            }
            return null;
        }
    }
}