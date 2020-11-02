/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Morph
{
    /// <summary>
    /// Язык
    /// </summary>
    public class MorphLang
    {
        public short Value;
        private bool GetValue(int i)
        {
            return ((((Value >> i)) & 1)) != 0;
        }
        private void SetValue(int i, bool val)
        {
            if (val) 
                Value |= ((short)(1 << i));
            else 
                Value &= ((short)(~(1 << i)));
        }
        /// <summary>
        /// Неопределённый язык
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
        /// Русский язык
        /// </summary>
        public bool IsRu
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
        /// Украинский язык
        /// </summary>
        public bool IsUa
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
        /// Белорусский язык
        /// </summary>
        public bool IsBy
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
        /// Русский, украинский, белорусский или казахский язык
        /// </summary>
        public bool IsCyrillic
        {
            get
            {
                return (IsRu | IsUa | IsBy) | IsKz;
            }
        }
        /// <summary>
        /// Английский язык
        /// </summary>
        public bool IsEn
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
        /// Итальянский язык
        /// </summary>
        public bool IsIt
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
        /// Казахский язык
        /// </summary>
        public bool IsKz
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
        static string[] m_Names = new string[] {"RU", "UA", "BY", "EN", "IT", "KZ"};
        public override string ToString()
        {
            StringBuilder tmpStr = new StringBuilder();
            if (IsRu) 
                tmpStr.Append("RU;");
            if (IsUa) 
                tmpStr.Append("UA;");
            if (IsBy) 
                tmpStr.Append("BY;");
            if (IsEn) 
                tmpStr.Append("EN;");
            if (IsIt) 
                tmpStr.Append("IT;");
            if (IsKz) 
                tmpStr.Append("KZ;");
            if (tmpStr.Length > 0) 
                tmpStr.Length--;
            return tmpStr.ToString();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is MorphLang)) 
                return false;
            return Value == ((MorphLang)obj).Value;
        }
        public override int GetHashCode()
        {
            return Value;
        }
        /// <summary>
        /// Преобразовать из строки
        /// </summary>
        public static bool TryParse(string str, out MorphLang lang)
        {
            lang = new MorphLang();
            while (!string.IsNullOrEmpty(str)) 
            {
                int i;
                for (i = 0; i < m_Names.Length; i++) 
                {
                    if (str.StartsWith(m_Names[i], StringComparison.OrdinalIgnoreCase)) 
                        break;
                }
                if (i >= m_Names.Length) 
                    break;
                lang.Value |= ((short)(1 << i));
                for (i = 2; i < str.Length; i++) 
                {
                    if (char.IsLetter(str[i])) 
                        break;
                }
                if (i >= str.Length) 
                    break;
                str = str.Substring(i);
            }
            if (lang.IsUndefined) 
                return false;
            return true;
        }
        /// <summary>
        /// Моделирование побитного "AND"
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 & arg2</return>
        public static MorphLang operator &(MorphLang arg1, MorphLang arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return new MorphLang() { Value = (short)((val1 & val2)) };
        }
        /// <summary>
        /// Моделирование побитного "OR"
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 | arg2</return>
        public static MorphLang operator |(MorphLang arg1, MorphLang arg2)
        {
            short val1 = (short)0;
            short val2 = (short)0;
            if (!object.ReferenceEquals(arg1, null)) 
                val1 = arg1.Value;
            if (!object.ReferenceEquals(arg2, null)) 
                val2 = arg2.Value;
            return new MorphLang() { Value = (short)((val1 | val2)) };
        }
        /// <summary>
        /// Моделирование сравнения ==
        /// </summary>
        /// <param name="arg1">первый аргумент</param>
        /// <param name="arg2">второй аргумент</param>
        /// <return>arg1 == arg2</return>
        public static bool operator ==(MorphLang arg1, MorphLang arg2)
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
        public static bool operator !=(MorphLang arg1, MorphLang arg2)
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
        public static MorphLang Unknown = new MorphLang();
        /// <summary>
        /// Русский
        /// </summary>
        public static MorphLang RU = new MorphLang() { IsRu = true };
        /// <summary>
        /// Украинский
        /// </summary>
        public static MorphLang UA = new MorphLang() { IsUa = true };
        /// <summary>
        /// Белорусский
        /// </summary>
        public static MorphLang BY = new MorphLang() { IsBy = true };
        /// <summary>
        /// Английский
        /// </summary>
        public static MorphLang EN = new MorphLang() { IsEn = true };
        /// <summary>
        /// Итальянский
        /// </summary>
        public static MorphLang IT = new MorphLang() { IsIt = true };
        /// <summary>
        /// Казахский
        /// </summary>
        public static MorphLang KZ = new MorphLang() { IsKz = true };
    }
}