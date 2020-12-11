/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Morph
{
    /// <summary>
    /// Часть речи
    /// </summary>
    public class MorphClass
    {
        public short Value;
        bool GetValue(int i)
        {
            return ((((Value >> i)) & 1)) != 0;
        }
        void SetValue(int i, bool val)
        {
            if (val) 
                Value |= ((short)(1 << i));
            else 
                Value &= ((short)(~(1 << i)));
        }
        /// <summary>
        /// Неопределённый тип
        /// </summary>
        public bool IsUndefined
        {
            get
            {
                return Value == 0;
            }
            set
            {
                Value = 0;
            }
        }
        /// <summary>
        /// Существительное
        /// </summary>
        public bool IsNoun
        {
            get
            {
                return this.GetValue(0);
            }
            set
            {
                if (value) 
                    Value = 0;
                this.SetValue(0, value);
            }
        }
        /// <summary>
        /// Прилагательное
        /// </summary>
        public bool IsAdjective
        {
            get
            {
                return this.GetValue(1);
            }
            set
            {
                if (value) 
                    Value = 0;
                this.SetValue(1, value);
            }
        }
        /// <summary>
        /// Глагол
        /// </summary>
        public bool IsVerb
        {
            get
            {
                return this.GetValue(2);
            }
            set
            {
                if (value) 
                    Value = 0;
                this.SetValue(2, value);
            }
        }
        /// <summary>
        /// Наречие
        /// </summary>
        public bool IsAdverb
        {
            get
            {
                return this.GetValue(3);
            }
            set
            {
                if (value) 
                    Value = 0;
                this.SetValue(3, value);
            }
        }
        /// <summary>
        /// Местоимение
        /// </summary>
        public bool IsPronoun
        {
            get
            {
                return this.GetValue(4);
            }
            set
            {
                if (value) 
                    Value = 0;
                this.SetValue(4, value);
            }
        }
        /// <summary>
        /// Разное (частицы, междометия)
        /// </summary>
        public bool IsMisc
        {
            get
            {
                return this.GetValue(5);
            }
            set
            {
                if (value) 
                    Value = 0;
                this.SetValue(5, value);
            }
        }
        /// <summary>
        /// Предлог
        /// </summary>
        public bool IsPreposition
        {
            get
            {
                return this.GetValue(6);
            }
            set
            {
                this.SetValue(6, value);
            }
        }
        /// <summary>
        /// Союз
        /// </summary>
        public bool IsConjunction
        {
            get
            {
                return this.GetValue(7);
            }
            set
            {
                this.SetValue(7, value);
            }
        }
        /// <summary>
        /// Собственное имя (фамилия, имя, отчество, геогр.название и др.)
        /// </summary>
        public bool IsProper
        {
            get
            {
                return this.GetValue(8);
            }
            set
            {
                this.SetValue(8, value);
            }
        }
        /// <summary>
        /// Фамилия
        /// </summary>
        public bool IsProperSurname
        {
            get
            {
                return this.GetValue(9);
            }
            set
            {
                if (value) 
                    IsProper = true;
                this.SetValue(9, value);
            }
        }
        /// <summary>
        /// Фамилия
        /// </summary>
        public bool IsProperName
        {
            get
            {
                return this.GetValue(10);
            }
            set
            {
                if (value) 
                    IsProper = true;
                this.SetValue(10, value);
            }
        }
        /// <summary>
        /// Отчество
        /// </summary>
        public bool IsProperSecname
        {
            get
            {
                return this.GetValue(11);
            }
            set
            {
                if (value) 
                    IsProper = true;
                this.SetValue(11, value);
            }
        }
        /// <summary>
        /// Географическое название
        /// </summary>
        public bool IsProperGeo
        {
            get
            {
                return this.GetValue(12);
            }
            set
            {
                if (value) 
                    IsProper = true;
                this.SetValue(12, value);
            }
        }
        /// <summary>
        /// Личное местоимение (я, мой, ты, он ...)
        /// </summary>
        public bool IsPersonalPronoun
        {
            get
            {
                return this.GetValue(13);
            }
            set
            {
                this.SetValue(13, value);
            }
        }
        static string[] m_Names = new string[] {"существ.", "прилаг.", "глагол", "наречие", "местоим.", "разное", "предлог", "союз", "собств.", "фамилия", "имя", "отч.", "геогр.", "личн.местоим."};
        public override string ToString()
        {
            StringBuilder tmpStr = new StringBuilder();
            if (IsNoun) 
                tmpStr.Append("существ.|");
            if (IsAdjective) 
                tmpStr.Append("прилаг.|");
            if (IsVerb) 
                tmpStr.Append("глагол|");
            if (IsAdverb) 
                tmpStr.Append("наречие|");
            if (IsPronoun) 
                tmpStr.Append("местоим.|");
            if (IsMisc) 
            {
                if (IsConjunction || IsPreposition || IsProper) 
                {
                }
                else 
                    tmpStr.Append("разное|");
            }
            if (IsPreposition) 
                tmpStr.Append("предлог|");
            if (IsConjunction) 
                tmpStr.Append("союз|");
            if (IsProper) 
                tmpStr.Append("собств.|");
            if (IsProperSurname) 
                tmpStr.Append("фамилия|");
            if (IsProperName) 
                tmpStr.Append("имя|");
            if (IsProperSecname) 
                tmpStr.Append("отч.|");
            if (IsProperGeo) 
                tmpStr.Append("геогр.|");
            if (IsPersonalPronoun) 
                tmpStr.Append("личн.местоим.|");
            if (tmpStr.Length > 0) 
                tmpStr.Length--;
            return tmpStr.ToString();
        }
        /// <summary>
        /// Неопределённое
        /// </summary>
        public static MorphClass Undefined = new MorphClass() { IsUndefined = true };
        /// <summary>
        /// Существительное
        /// </summary>
        public static MorphClass Noun = new MorphClass() { IsNoun = true };
        /// <summary>
        /// Местоимение
        /// </summary>
        public static MorphClass Pronoun = new MorphClass() { IsPronoun = true };
        /// <summary>
        /// Личное местоимение
        /// </summary>
        public static MorphClass PersonalPronoun = new MorphClass() { IsPersonalPronoun = true };
        /// <summary>
        /// Глагол
        /// </summary>
        public static MorphClass Verb = new MorphClass() { IsVerb = true };
        /// <summary>
        /// Прилагательное
        /// </summary>
        public static MorphClass Adjective = new MorphClass() { IsAdjective = true };
        /// <summary>
        /// Наречие
        /// </summary>
        public static MorphClass Adverb = new MorphClass() { IsAdverb = true };
        /// <summary>
        /// Предлог
        /// </summary>
        public static MorphClass Preposition = new MorphClass() { IsPreposition = true };
        /// <summary>
        /// Союз
        /// </summary>
        public static MorphClass Conjunction = new MorphClass() { IsConjunction = true };
        public override bool Equals(object obj)
        {
            if (!(obj is MorphClass)) 
                return false;
            return Value == ((MorphClass)obj).Value;
        }
        public override int GetHashCode()
        {
            return Value;
        }
        /// <summary>
        /// Моделирование побитного "AND"
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 & arg2</return>
        public static MorphClass operator &(MorphClass arg1, MorphClass arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return new MorphClass() { Value = (short)((val1 & val2)) };
        }
        /// <summary>
        /// Моделирование побитного "OR"
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 | arg2</return>
        public static MorphClass operator |(MorphClass arg1, MorphClass arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return new MorphClass() { Value = (short)((val1 | val2)) };
        }
        /// <summary>
        /// Моделирование побитного "XOR"
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 ^ arg2</return>
        public static MorphClass operator ^(MorphClass arg1, MorphClass arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return new MorphClass() { Value = (short)((val1 ^ val2)) };
        }
        /// <summary>
        /// Моделирование сравнения ==
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 == arg2</return>
        public static bool operator ==(MorphClass arg1, MorphClass arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return val1 == val2;
        }
        /// <summary>
        /// Моделирование неравенства !=
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 != arg2</return>
        public static bool operator !=(MorphClass arg1, MorphClass arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return val1 != val2;
        }
    }
}