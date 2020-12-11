/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Pullenti.Ner
{
    /// <summary>
    /// Базовый класс для всех токенов. Наследные классы - TextToken (конечная словоформа) и MetaToken (связный фрагмент других токенов).
    /// </summary>
    public class Token
    {
        public Token(Pullenti.Ner.Core.AnalysisKit kit, int begin, int end)
        {
            Kit = kit;
            m_BeginChar = begin;
            m_EndChar = end;
        }
        /// <summary>
        /// Аналитический контейнер
        /// </summary>
        public Pullenti.Ner.Core.AnalysisKit Kit;
        /// <summary>
        /// Позиция в тексте начального символа
        /// </summary>
        public virtual int BeginChar
        {
            get
            {
                return m_BeginChar;
            }
        }
        int m_BeginChar;
        /// <summary>
        /// Позиция в тексте конечного символа
        /// </summary>
        public virtual int EndChar
        {
            get
            {
                return m_EndChar;
            }
        }
        int m_EndChar;
        /// <summary>
        /// Длина в текстовых символах
        /// </summary>
        public int LengthChar
        {
            get
            {
                return (EndChar - BeginChar) + 1;
            }
        }
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag;
        /// <summary>
        /// Предыдущий токен в цепочке токенов
        /// </summary>
        public Token Previous
        {
            get
            {
                return m_Previous;
            }
            set
            {
                m_Previous = value;
                if (value != null) 
                    value.m_Next = this;
                m_Attrs = 0;
            }
        }
        internal Token m_Previous;
        /// <summary>
        /// Следующий токен в цепочке токенов
        /// </summary>
        public Token Next
        {
            get
            {
                return m_Next;
            }
            set
            {
                m_Next = value;
                if (value != null) 
                    value.m_Previous = this;
                m_Attrs = 0;
            }
        }
        internal Token m_Next;
        /// <summary>
        /// Морфологическая информация
        /// </summary>
        public virtual MorphCollection Morph
        {
            get
            {
                if (m_Morph == null) 
                    m_Morph = new MorphCollection();
                return m_Morph;
            }
            set
            {
                m_Morph = value;
            }
        }
        MorphCollection m_Morph;
        /// <summary>
        /// Информация о символах
        /// </summary>
        public Pullenti.Morph.CharsInfo Chars;
        public override string ToString()
        {
            return Kit.Sofa.Text.Substring(BeginChar, (EndChar + 1) - BeginChar);
        }
        short m_Attrs;
        bool GetAttr(int i)
        {
            char ch;
            if (((m_Attrs & 1)) == 0) 
            {
                m_Attrs = 1;
                if (m_Previous == null) 
                {
                    this.SetAttr(1, true);
                    this.SetAttr(3, true);
                }
                else 
                    for (int j = m_Previous.EndChar + 1; j < BeginChar; j++) 
                    {
                        if (char.IsWhiteSpace(((ch = Kit.Sofa.Text[j])))) 
                        {
                            this.SetAttr(1, true);
                            if (ch == 0xD || ch == 0xA || ch == '\f') 
                                this.SetAttr(3, true);
                        }
                    }
                if (m_Next == null) 
                {
                    this.SetAttr(2, true);
                    this.SetAttr(4, true);
                }
                else 
                    for (int j = EndChar + 1; j < m_Next.BeginChar; j++) 
                    {
                        if (char.IsWhiteSpace((ch = Kit.Sofa.Text[j]))) 
                        {
                            this.SetAttr(2, true);
                            if (ch == 0xD || ch == 0xA || ch == '\f') 
                                this.SetAttr(4, true);
                        }
                    }
            }
            return ((((m_Attrs >> i)) & 1)) != 0;
        }
        protected void SetAttr(int i, bool val)
        {
            if (val) 
                m_Attrs |= ((short)(1 << i));
            else 
                m_Attrs &= ((short)(~(1 << i)));
        }
        /// <summary>
        /// Наличие пробельных символов перед
        /// </summary>
        public virtual bool IsWhitespaceBefore
        {
            get
            {
                return this.GetAttr(1);
            }
            set
            {
                this.SetAttr(1, value);
            }
        }
        /// <summary>
        /// Наличие пробельных символов после
        /// </summary>
        public virtual bool IsWhitespaceAfter
        {
            get
            {
                return this.GetAttr(2);
            }
            set
            {
                this.SetAttr(2, value);
            }
        }
        /// <summary>
        /// Элемент начинается с новой строки. 
        /// Для 1-го элемента всегда true.
        /// </summary>
        public virtual bool IsNewlineBefore
        {
            get
            {
                return this.GetAttr(3);
            }
            set
            {
                this.SetAttr(3, value);
            }
        }
        /// <summary>
        /// Элемент заканчивает строку. 
        /// Для последнего элемента всегда true.
        /// </summary>
        public virtual bool IsNewlineAfter
        {
            get
            {
                return this.GetAttr(4);
            }
            set
            {
                this.SetAttr(4, value);
            }
        }
        // Это используется внутренним образом
        public bool InnerBool
        {
            get
            {
                return this.GetAttr(5);
            }
            set
            {
                this.SetAttr(5, value);
            }
        }
        // Это используется внутренним образом
        // (признак того, что здесь не начинается именная группа, чтобы повторно не пытаться выделять)
        public bool NotNounPhrase
        {
            get
            {
                return this.GetAttr(6);
            }
            set
            {
                this.SetAttr(6, value);
            }
        }
        /// <summary>
        /// Количество пробелов перед, переход на новую строку = 10, табуляция = 5
        /// </summary>
        public virtual int WhitespacesBeforeCount
        {
            get
            {
                if (Previous == null) 
                    return 100;
                if ((Previous.EndChar + 1) == BeginChar) 
                    return 0;
                return this.CalcWhitespaces(Previous.EndChar + 1, BeginChar - 1);
            }
        }
        /// <summary>
        /// Количество переходов на новую строку перед
        /// </summary>
        public int NewlinesBeforeCount
        {
            get
            {
                char ch0 = (char)0;
                int res = 0;
                string txt = Kit.Sofa.Text;
                for (int p = BeginChar - 1; p >= 0; p--) 
                {
                    char ch = txt[p];
                    if (ch == 0xA) 
                        res++;
                    else if (ch == 0xD && ch0 != 0xA) 
                        res++;
                    else if (ch == '\f') 
                        res += 10;
                    else if (!char.IsWhiteSpace(ch)) 
                        break;
                    ch0 = ch;
                }
                return res;
            }
        }
        /// <summary>
        /// Количество переходов на новую строку перед
        /// </summary>
        public int NewlinesAfterCount
        {
            get
            {
                char ch0 = (char)0;
                int res = 0;
                string txt = Kit.Sofa.Text;
                for (int p = EndChar + 1; p < txt.Length; p++) 
                {
                    char ch = txt[p];
                    if (ch == 0xD) 
                        res++;
                    else if (ch == 0xA && ch0 != 0xD) 
                        res++;
                    else if (ch == '\f') 
                        res += 10;
                    else if (!char.IsWhiteSpace(ch)) 
                        break;
                    ch0 = ch;
                }
                return res;
            }
        }
        /// <summary>
        /// Количество пробелов перед, переход на новую строку = 10, табуляция = 5
        /// </summary>
        public virtual int WhitespacesAfterCount
        {
            get
            {
                if (Next == null) 
                    return 100;
                if ((EndChar + 1) == Next.BeginChar) 
                    return 0;
                return this.CalcWhitespaces(EndChar + 1, Next.BeginChar - 1);
            }
        }
        int CalcWhitespaces(int p0, int p1)
        {
            if ((p0 < 0) || p0 > p1 || p1 >= Kit.Sofa.Text.Length) 
                return -1;
            int res = 0;
            for (int i = p0; i <= p1; i++) 
            {
                char ch = Kit.GetTextCharacter(i);
                if (ch == '\r' || ch == '\n') 
                {
                    res += 10;
                    char ch1 = Kit.GetTextCharacter(i + 1);
                    if (ch != ch1 && ((ch1 == '\r' || ch1 == '\n'))) 
                        i++;
                }
                else if (ch == '\t') 
                    res += 5;
                else if (ch == '\u0007') 
                    res += 100;
                else if (ch == '\f') 
                    res += 100;
                else 
                    res++;
            }
            return res;
        }
        /// <summary>
        /// Это символ переноса
        /// </summary>
        public bool IsHiphen
        {
            get
            {
                char ch = Kit.Sofa.Text[BeginChar];
                return Pullenti.Morph.LanguageHelper.IsHiphen(ch);
            }
        }
        /// <summary>
        /// Это спец-символы для табличных элементов (7h, 1Eh, 1Fh)
        /// </summary>
        public bool IsTableControlChar
        {
            get
            {
                char ch = Kit.Sofa.Text[BeginChar];
                return ch == 7 || ch == 0x1F || ch == 0x1E;
            }
        }
        /// <summary>
        /// Это соединительный союз И (на всех языках)
        /// </summary>
        public virtual bool IsAnd
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Это соединительный союз ИЛИ (на всех языках)
        /// </summary>
        public virtual bool IsOr
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Это запятая
        /// </summary>
        public virtual bool IsComma
        {
            get
            {
                return this.IsChar(',');
            }
        }
        /// <summary>
        /// Это запятая или союз И
        /// </summary>
        public bool IsCommaAnd
        {
            get
            {
                return IsComma || IsAnd;
            }
        }
        /// <summary>
        /// Токен состоит из конкретного символа
        /// </summary>
        /// <param name="ch">проверяемый символ</param>
        public bool IsChar(char ch)
        {
            if (BeginChar != EndChar) 
                return false;
            return Kit.Sofa.Text[BeginChar] == ch;
        }
        /// <summary>
        /// Токен состоит из одного символа, который есть в указанной строке
        /// </summary>
        /// <param name="chars">строка возможных символов</param>
        public virtual bool IsCharOf(string chars)
        {
            if (BeginChar != EndChar) 
                return false;
            return chars.IndexOf(Kit.Sofa.Text[BeginChar]) >= 0;
        }
        /// <summary>
        /// Проверка конкретного значения слова
        /// </summary>
        /// <param name="term">слово (проверяется значение TextToken.Term)</param>
        /// <param name="termUA">слово для проверки на украинском языке</param>
        /// <return>да-нет</return>
        public virtual bool IsValue(string term, string termUA = null)
        {
            return false;
        }
        /// <summary>
        /// Признак того, что это буквенный текстовой токен (TextToken)
        /// </summary>
        public virtual bool IsLetters
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Получить ссылку на сущность (не null только для ReferentToken)
        /// </summary>
        public virtual Referent GetReferent()
        {
            return null;
        }
        /// <summary>
        /// Получить список ссылок на все сущности, скрывающиеся под элементом. 
        /// Дело в том, что одни сущности могут накрывать другие (например, адрес накроет город).
        /// </summary>
        public virtual List<Referent> GetReferents()
        {
            return null;
        }
        /// <summary>
        /// Получить связанный с токеном текст в именительном падеже
        /// </summary>
        /// <param name="mc">желательная часть речи</param>
        /// <param name="num">желательное число</param>
        /// <param name="gender">желательный пол</param>
        /// <param name="keepChars">сохранять регистр символов (по умолчанию, всё в верхний)</param>
        /// <return>строка текста</return>
        public virtual string GetNormalCaseText(Pullenti.Morph.MorphClass mc = null, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined, bool keepChars = false)
        {
            return this.ToString();
        }
        /// <summary>
        /// Получить фрагмент исходного текста, связанный с токеном
        /// </summary>
        /// <return>фрагмент исходного текста</return>
        public virtual string GetSourceText()
        {
            int len = (EndChar + 1) - BeginChar;
            if ((len < 1) || (BeginChar < 0)) 
                return null;
            if ((BeginChar + len) > Kit.Sofa.Text.Length) 
                return null;
            return Kit.Sofa.Text.Substring(BeginChar, len);
        }
        /// <summary>
        /// Проверка, что слово есть в словаре соответствующего языка
        /// </summary>
        /// <return>части речи, если не из словаря, то IsUndefined</return>
        public virtual Pullenti.Morph.MorphClass GetMorphClassInDictionary()
        {
            return Morph.Class;
        }
        internal virtual void Serialize(Stream stream)
        {
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, BeginChar);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, EndChar);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, m_Attrs);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, Chars.Value);
            if (m_Morph == null) 
                m_Morph = new MorphCollection();
            m_Morph.Serialize(stream);
        }
        internal virtual void Deserialize(Stream stream, Pullenti.Ner.Core.AnalysisKit kit, int vers)
        {
            Kit = kit;
            m_BeginChar = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            m_EndChar = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            m_Attrs = (short)Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            Chars = new Pullenti.Morph.CharsInfo() { Value = (short)Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream) };
            m_Morph = new MorphCollection();
            m_Morph.Deserialize(stream);
        }
    }
}