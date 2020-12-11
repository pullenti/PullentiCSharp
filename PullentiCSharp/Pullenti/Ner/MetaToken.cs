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

namespace Pullenti.Ner
{
    /// <summary>
    /// Метатокен - надстройка над диапазоном других токенов. Базовый класс для подавляющего числа всех токенов: 
    /// NumberToken, ReferentToken, NounPhraseToken и пр.
    /// </summary>
    public class MetaToken : Token
    {
        public MetaToken(Token begin, Token end, Pullenti.Ner.Core.AnalysisKit kit = null) : base((kit != null ? kit : (begin != null ? begin.Kit : null)), (begin == null ? 0 : begin.BeginChar), (end == null ? 0 : end.EndChar))
        {
            if (begin == this || end == this) 
            {
            }
            m_BeginToken = begin;
            m_EndToken = end;
            if (begin == null || end == null) 
                return;
            Chars = begin.Chars;
            if (begin != end) 
            {
                for (Token t = begin.Next; t != null; t = t.Next) 
                {
                    if (t.Chars.IsLetter) 
                    {
                        if (Chars.IsCapitalUpper && t.Chars.IsAllLower) 
                        {
                        }
                        else 
                            Chars = new Pullenti.Morph.CharsInfo() { Value = (short)((Chars.Value & t.Chars.Value)) };
                    }
                    if (t == end) 
                        break;
                }
            }
        }
        void _RefreshCharsInfo()
        {
            if (m_BeginToken == null) 
                return;
            Chars = m_BeginToken.Chars;
            int cou = 0;
            if (m_BeginToken != m_EndToken && m_EndToken != null) 
            {
                for (Token t = m_BeginToken.Next; t != null; t = t.Next) 
                {
                    if ((++cou) > 100) 
                        break;
                    if (t.EndChar > m_EndToken.EndChar) 
                        break;
                    if (t.Chars.IsLetter) 
                        Chars = new Pullenti.Morph.CharsInfo() { Value = (short)((Chars.Value & t.Chars.Value)) };
                    if (t == m_EndToken) 
                        break;
                }
            }
        }
        /// <summary>
        /// Начальный токен диапазона
        /// </summary>
        public virtual Token BeginToken
        {
            get
            {
                return m_BeginToken;
            }
            set
            {
                if (m_BeginToken != value) 
                {
                    if (m_BeginToken == this) 
                    {
                    }
                    else 
                    {
                        m_BeginToken = value;
                        this._RefreshCharsInfo();
                    }
                }
            }
        }
        internal Token m_BeginToken;
        /// <summary>
        /// Конечный токен диапазона
        /// </summary>
        public virtual Token EndToken
        {
            get
            {
                return m_EndToken;
            }
            set
            {
                if (m_EndToken != value) 
                {
                    if (m_EndToken == this) 
                    {
                    }
                    else 
                    {
                        m_EndToken = value;
                        this._RefreshCharsInfo();
                    }
                }
            }
        }
        internal Token m_EndToken;
        public override int BeginChar
        {
            get
            {
                Token bt = BeginToken;
                return (bt == null ? 0 : bt.BeginChar);
            }
        }
        public override int EndChar
        {
            get
            {
                Token et = EndToken;
                return (et == null ? 0 : et.EndChar);
            }
        }
        /// <summary>
        /// Количество токенов в диапазоне
        /// </summary>
        public int TokensCount
        {
            get
            {
                int count = 1;
                for (Token t = m_BeginToken; t != m_EndToken && t != null; t = t.Next) 
                {
                    if (count > 1 && t == m_BeginToken) 
                        break;
                    count++;
                }
                return count;
            }
        }
        public override bool IsWhitespaceBefore
        {
            get
            {
                return m_BeginToken.IsWhitespaceBefore;
            }
            set
            {
                this.SetAttr(1, value);
            }
        }
        public override bool IsWhitespaceAfter
        {
            get
            {
                return m_EndToken.IsWhitespaceAfter;
            }
            set
            {
                this.SetAttr(2, value);
            }
        }
        public override bool IsNewlineBefore
        {
            get
            {
                return m_BeginToken.IsNewlineBefore;
            }
            set
            {
                this.SetAttr(3, value);
            }
        }
        public override bool IsNewlineAfter
        {
            get
            {
                return m_EndToken.IsNewlineAfter;
            }
            set
            {
                this.SetAttr(4, value);
            }
        }
        public override int WhitespacesBeforeCount
        {
            get
            {
                return m_BeginToken.WhitespacesBeforeCount;
            }
        }
        public override int WhitespacesAfterCount
        {
            get
            {
                return m_EndToken.WhitespacesAfterCount;
            }
        }
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            for (Token t = m_BeginToken; t != null; t = t.Next) 
            {
                if (res.Length > 0 && t.IsWhitespaceBefore) 
                    res.Append(' ');
                res.Append(t.GetSourceText());
                if (t == m_EndToken) 
                    break;
            }
            return res.ToString();
        }
        public override bool IsValue(string term, string termUA = null)
        {
            return BeginToken.IsValue(term, termUA);
        }
        public override List<Referent> GetReferents()
        {
            List<Referent> res = null;
            for (Token t = BeginToken; t != null && t.EndChar <= EndChar; t = t.Next) 
            {
                List<Referent> li = t.GetReferents();
                if (li == null) 
                    continue;
                if (res == null) 
                    res = li;
                else 
                    foreach (Referent r in li) 
                    {
                        if (!res.Contains(r)) 
                            res.Add(r);
                    }
            }
            return res;
        }
        public static bool Check(List<ReferentToken> li)
        {
            if (li == null || (li.Count < 1)) 
                return false;
            int i;
            for (i = 0; i < (li.Count - 1); i++) 
            {
                if (li[i].BeginChar > li[i].EndChar) 
                    return false;
                if (li[i].EndChar >= li[i + 1].BeginChar) 
                    return false;
            }
            if (li[i].BeginChar > li[i].EndChar) 
                return false;
            return true;
        }
        public override string GetNormalCaseText(Pullenti.Morph.MorphClass mc = null, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined, bool keepChars = false)
        {
            Pullenti.Ner.Core.GetTextAttr attr = Pullenti.Ner.Core.GetTextAttr.No;
            if (num == Pullenti.Morph.MorphNumber.Singular) 
                attr |= Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle;
            else 
                attr |= Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative;
            if (keepChars) 
                attr |= Pullenti.Ner.Core.GetTextAttr.KeepRegister;
            if (BeginToken == EndToken) 
                return BeginToken.GetNormalCaseText(mc, num, gender, keepChars);
            else 
                return Pullenti.Ner.Core.MiscHelper.GetTextValue(BeginToken, EndToken, attr);
        }
    }
}