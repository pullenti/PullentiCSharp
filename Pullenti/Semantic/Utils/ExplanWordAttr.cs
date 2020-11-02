/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Semantic.Utils
{
    /// <summary>
    /// Атрибуты слова дериватной группы DerivateWord
    /// </summary>
    public class ExplanWordAttr
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
        /// Одушевлённое
        /// </summary>
        public bool IsAnimated
        {
            get
            {
                return this.GetValue(0);
            }
            set
            {
                this.SetValue(0, value);
            }
        }
        /// <summary>
        /// Может иметь собственное имя
        /// </summary>
        public bool IsNamed
        {
            get
            {
                return this.GetValue(1);
            }
            set
            {
                this.SetValue(1, value);
            }
        }
        /// <summary>
        /// Может иметь номер (например, Олимпиада 80)
        /// </summary>
        public bool IsNumbered
        {
            get
            {
                return this.GetValue(2);
            }
            set
            {
                this.SetValue(2, value);
            }
        }
        /// <summary>
        /// Может ли иметь числовую характеристику (длина, количество, деньги ...)
        /// </summary>
        public bool IsMeasured
        {
            get
            {
                return this.GetValue(3);
            }
            set
            {
                this.SetValue(3, value);
            }
        }
        /// <summary>
        /// Позитивная окраска
        /// </summary>
        public bool IsEmoPositive
        {
            get
            {
                return this.GetValue(4);
            }
            set
            {
                this.SetValue(4, value);
            }
        }
        /// <summary>
        /// Негативная окраска
        /// </summary>
        public bool IsEmoNegative
        {
            get
            {
                return this.GetValue(5);
            }
            set
            {
                this.SetValue(5, value);
            }
        }
        /// <summary>
        /// Это животное, а не человек (для IsAnimated = true)
        /// </summary>
        public bool IsAnimal
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
        /// Это человек, а не животное (для IsAnimated = true)
        /// </summary>
        public bool IsMan
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
        /// За словом может быть персона в родительном падеже (слуга Хозяина, отец Ивана ...)
        /// </summary>
        public bool IsCanPersonAfter
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
        /// Пространственный объект
        /// </summary>
        public bool IsSpaceObject
        {
            get
            {
                return this.GetValue(9);
            }
            set
            {
                this.SetValue(9, value);
            }
        }
        /// <summary>
        /// Временной объект
        /// </summary>
        public bool IsTimeObject
        {
            get
            {
                return this.GetValue(10);
            }
            set
            {
                this.SetValue(10, value);
            }
        }
        /// <summary>
        /// Временной объект
        /// </summary>
        public bool IsVerbNoun
        {
            get
            {
                return this.GetValue(11);
            }
            set
            {
                this.SetValue(11, value);
            }
        }
        public override string ToString()
        {
            StringBuilder tmpStr = new StringBuilder();
            if (IsAnimated) 
                tmpStr.Append("одуш.");
            if (IsAnimal) 
                tmpStr.Append("животн.");
            if (IsMan) 
                tmpStr.Append("чел.");
            if (IsSpaceObject) 
                tmpStr.Append("простр.");
            if (IsTimeObject) 
                tmpStr.Append("времен.");
            if (IsNamed) 
                tmpStr.Append("именов.");
            if (IsNumbered) 
                tmpStr.Append("нумеруем.");
            if (IsMeasured) 
                tmpStr.Append("измеряем.");
            if (IsEmoPositive) 
                tmpStr.Append("позитив.");
            if (IsEmoNegative) 
                tmpStr.Append("негатив.");
            if (IsCanPersonAfter) 
                tmpStr.Append("персона_за_родит.");
            if (IsVerbNoun) 
                tmpStr.Append("глаг.сущ.");
            return tmpStr.ToString();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is ExplanWordAttr)) 
                return false;
            return Value == ((ExplanWordAttr)obj).Value;
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
        public static ExplanWordAttr operator &(ExplanWordAttr arg1, ExplanWordAttr arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return new ExplanWordAttr() { Value = (short)((val1 & val2)) };
        }
        /// <summary>
        /// Моделирование побитного "OR"
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 | arg2</return>
        public static ExplanWordAttr operator |(ExplanWordAttr arg1, ExplanWordAttr arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return new ExplanWordAttr() { Value = (short)((val1 | val2)) };
        }
        /// <summary>
        /// Моделирование сравнения ==
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 == arg2</return>
        public static bool operator ==(ExplanWordAttr arg1, ExplanWordAttr arg2)
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
        public static bool operator !=(ExplanWordAttr arg1, ExplanWordAttr arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return val1 != val2;
        }
        /// <summary>
        /// Неопределённое
        /// </summary>
        public static ExplanWordAttr Undefined = new ExplanWordAttr();
    }
}