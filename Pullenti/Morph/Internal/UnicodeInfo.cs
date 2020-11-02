/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Morph.Internal
{
    // Ввели для оптимизации на Питоне.
    public class UnicodeInfo
    {
        public static List<UnicodeInfo> AllChars;
        public static UnicodeInfo GetChar(char ch)
        {
            return AllChars[(int)ch]._clone();
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            AllChars = new List<UnicodeInfo>(0x10000);
            string cyrvowel = "АЕЁИОУЮЯЫЭЄІЇЎӘӨҰҮІ";
            cyrvowel += cyrvowel.ToLower();
            for (int i = 0; i < 0x10000; i++) 
            {
                char ch = (char)i;
                UnicodeInfo ui = new UnicodeInfo((short)i);
                if (char.IsWhiteSpace(ch)) 
                    ui.IsWhitespace = true;
                else if (char.IsDigit(ch)) 
                    ui.IsDigit = true;
                else if (ch == 'º' || ch == '°') 
                {
                }
                else if (char.IsLetter(ch)) 
                {
                    ui.IsLetter = true;
                    if (i >= 0x400 && (i < 0x500)) 
                    {
                        ui.IsCyrillic = true;
                        if (cyrvowel.IndexOf(ch) >= 0) 
                            ui.IsVowel = true;
                    }
                    else if (i < 0x200) 
                    {
                        ui.IsLatin = true;
                        if ("AEIOUYaeiouy".IndexOf(ch) >= 0) 
                            ui.IsVowel = true;
                    }
                    if (char.IsUpper(ch)) 
                        ui.IsUpper = true;
                    if (char.IsLower(ch)) 
                        ui.IsLower = true;
                }
                else 
                {
                    if (((((ch == '-' || ch == '–' || ch == '¬') || ch == '-' || ch == ((char)0x00AD)) || ch == ((char)0x2011) || ch == '-') || ch == '—' || ch == '–') || ch == '−' || ch == '-') 
                        ui.IsHiphen = true;
                    if ("\"'`“”’".IndexOf(ch) >= 0) 
                        ui.IsQuot = true;
                    if ("'`’".IndexOf(ch) >= 0) 
                    {
                        ui.IsApos = true;
                        ui.IsQuot = true;
                    }
                }
                if (i >= 0x300 && (i < 0x370)) 
                    ui.IsUdaren = true;
                AllChars.Add(ui);
            }
        }
        UnicodeInfo _clone()
        {
            UnicodeInfo res = new UnicodeInfo();
            res.UniChar = UniChar;
            res.m_Value = m_Value;
            res.Code = Code;
            return res;
        }
        UnicodeInfo(short v = (short)0)
        {
            UniChar = (char)v;
            Code = v;
            m_Value = 0;
        }
        short m_Value;
        public char UniChar;
        public int Code;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("'{0}'({1})", UniChar, Code);
            if (IsWhitespace) 
                res.Append(", whitespace");
            if (IsDigit) 
                res.Append(", digit");
            if (IsLetter) 
                res.Append(", letter");
            if (IsLatin) 
                res.Append(", latin");
            if (IsCyrillic) 
                res.Append(", cyrillic");
            if (IsUpper) 
                res.Append(", upper");
            if (IsLower) 
                res.Append(", lower");
            if (IsHiphen) 
                res.Append(", hiphen");
            if (IsQuot) 
                res.Append(", quot");
            if (IsApos) 
                res.Append(", apos");
            if (IsVowel) 
                res.Append(", vowel");
            if (IsUdaren) 
                res.Append(", udaren");
            return res.ToString();
        }
        bool GetValue(int i)
        {
            return ((((m_Value >> i)) & 1)) != 0;
        }
        void SetValue(int i, bool val)
        {
            if (val) 
                m_Value |= ((short)(1 << i));
            else 
                m_Value &= ((short)(~(1 << i)));
        }
        public bool IsWhitespace
        {
            get
            {
                return ((m_Value & 0x1)) != 0;
            }
            set
            {
                this.SetValue(0, value);
            }
        }
        public bool IsDigit
        {
            get
            {
                return ((m_Value & 0x2)) != 0;
            }
            set
            {
                this.SetValue(1, value);
            }
        }
        public bool IsLetter
        {
            get
            {
                return ((m_Value & 0x4)) != 0;
            }
            set
            {
                this.SetValue(2, value);
            }
        }
        public bool IsUpper
        {
            get
            {
                return ((m_Value & 0x8)) != 0;
            }
            set
            {
                this.SetValue(3, value);
            }
        }
        public bool IsLower
        {
            get
            {
                return ((m_Value & 0x10)) != 0;
            }
            set
            {
                this.SetValue(4, value);
            }
        }
        public bool IsLatin
        {
            get
            {
                return ((m_Value & 0x20)) != 0;
            }
            set
            {
                this.SetValue(5, value);
            }
        }
        public bool IsCyrillic
        {
            get
            {
                return ((m_Value & 0x40)) != 0;
            }
            set
            {
                this.SetValue(6, value);
            }
        }
        public bool IsHiphen
        {
            get
            {
                return ((m_Value & 0x80)) != 0;
            }
            set
            {
                this.SetValue(7, value);
            }
        }
        public bool IsVowel
        {
            get
            {
                return ((m_Value & 0x100)) != 0;
            }
            set
            {
                this.SetValue(8, value);
            }
        }
        public bool IsQuot
        {
            get
            {
                return ((m_Value & 0x200)) != 0;
            }
            set
            {
                this.SetValue(9, value);
            }
        }
        public bool IsApos
        {
            get
            {
                return ((m_Value & 0x400)) != 0;
            }
            set
            {
                this.SetValue(10, value);
            }
        }
        public bool IsUdaren
        {
            get
            {
                return ((m_Value & 0x800)) != 0;
            }
            set
            {
                this.SetValue(11, value);
            }
        }
    }
}