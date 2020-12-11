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
    /// Падеж
    /// </summary>
    public class MorphCase
    {
        public short Value;
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
        /// Количество падежей
        /// </summary>
        public int Count
        {
            get
            {
                if (Value == 0) 
                    return 0;
                int cou = 0;
                for (int i = 0; i < 12; i++) 
                {
                    if (((Value & (1 << i))) != 0) 
                        cou++;
                }
                return cou;
            }
        }
        public static MorphCase Undefined = new MorphCase() { Value = 0 };
        /// <summary>
        /// Именительный падеж
        /// </summary>
        public static MorphCase Nominative = new MorphCase() { Value = 1 };
        /// <summary>
        /// Родительный падеж
        /// </summary>
        public static MorphCase Genitive = new MorphCase() { Value = 2 };
        /// <summary>
        /// Дательный падеж
        /// </summary>
        public static MorphCase Dative = new MorphCase() { Value = 4 };
        /// <summary>
        /// Винительный падеж
        /// </summary>
        public static MorphCase Accusative = new MorphCase() { Value = 8 };
        /// <summary>
        /// Творительный падеж
        /// </summary>
        public static MorphCase Instrumental = new MorphCase() { Value = 0x10 };
        /// <summary>
        /// Предложный падеж
        /// </summary>
        public static MorphCase Prepositional = new MorphCase() { Value = 0x20 };
        /// <summary>
        /// Звательный падеж
        /// </summary>
        public static MorphCase Vocative = new MorphCase() { Value = 0x40 };
        /// <summary>
        /// Частичный падеж
        /// </summary>
        public static MorphCase Partial = new MorphCase() { Value = 0x80 };
        /// <summary>
        /// Общий падеж
        /// </summary>
        public static MorphCase Common = new MorphCase() { Value = 0x100 };
        /// <summary>
        /// Притяжательный падеж
        /// </summary>
        public static MorphCase Possessive = new MorphCase() { Value = 0x200 };
        /// <summary>
        /// Все падежи одновременно
        /// </summary>
        public static MorphCase AllCases = new MorphCase() { Value = 0x3FF };
        /// <summary>
        /// Именительный
        /// </summary>
        public bool IsNominative
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
        /// Родительный
        /// </summary>
        public bool IsGenitive
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
        /// Дательный
        /// </summary>
        public bool IsDative
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
        /// Винительный
        /// </summary>
        public bool IsAccusative
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
        /// Творительный
        /// </summary>
        public bool IsInstrumental
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
        /// Предложный
        /// </summary>
        public bool IsPrepositional
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
        /// Звательный
        /// </summary>
        public bool IsVocative
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
        /// Частичный
        /// </summary>
        public bool IsPartial
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
        /// Общий (для английского)
        /// </summary>
        public bool IsCommon
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
        /// Притяжательный (для английского)
        /// </summary>
        public bool IsPossessive
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
        public override string ToString()
        {
            StringBuilder tmpStr = new StringBuilder();
            if (IsNominative) 
                tmpStr.Append("именит.|");
            if (IsGenitive) 
                tmpStr.Append("родит.|");
            if (IsDative) 
                tmpStr.Append("дател.|");
            if (IsAccusative) 
                tmpStr.Append("винит.|");
            if (IsInstrumental) 
                tmpStr.Append("творит.|");
            if (IsPrepositional) 
                tmpStr.Append("предлож.|");
            if (IsVocative) 
                tmpStr.Append("зват.|");
            if (IsPartial) 
                tmpStr.Append("частич.|");
            if (IsCommon) 
                tmpStr.Append("общ.|");
            if (IsPossessive) 
                tmpStr.Append("притяж.|");
            if (tmpStr.Length > 0) 
                tmpStr.Length--;
            return tmpStr.ToString();
        }
        static string[] m_Names = new string[] {"именит.", "родит.", "дател.", "винит.", "творит.", "предлож.", "зват.", "частич.", "общ.", "притяж."};
        /// <summary>
        /// Восстановить падежи из строки, полученной ToString
        /// </summary>
        public static MorphCase Parse(string str)
        {
            MorphCase res = new MorphCase();
            if (string.IsNullOrEmpty(str)) 
                return res;
            foreach (string s in str.Split('|')) 
            {
                for (int i = 0; i < m_Names.Length; i++) 
                {
                    if (s == m_Names[i]) 
                    {
                        res.SetValue(i, true);
                        break;
                    }
                }
            }
            return res;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is MorphCase)) 
                return false;
            return Value == ((MorphCase)obj).Value;
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
        public static MorphCase operator &(MorphCase arg1, MorphCase arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return new MorphCase() { Value = (short)((val1 & val2)) };
        }
        /// <summary>
        /// Моделирование побитного "OR"
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 | arg2</return>
        public static MorphCase operator |(MorphCase arg1, MorphCase arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return new MorphCase() { Value = (short)((val1 | val2)) };
        }
        /// <summary>
        /// Моделирование побитного "XOR"
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 ^ arg2</return>
        public static MorphCase operator ^(MorphCase arg1, MorphCase arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return new MorphCase() { Value = (short)((val1 ^ val2)) };
        }
        /// <summary>
        /// Моделирование сравнения ==
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 == arg2</return>
        public static bool operator ==(MorphCase arg1, MorphCase arg2)
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
        public static bool operator !=(MorphCase arg1, MorphCase arg2)
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