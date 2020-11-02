/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Поддержка анализа скобок и кавычек
    /// </summary>
    public static class BracketHelper
    {
        /// <summary>
        /// Проверка, что с этого токена может начинаться последовательность, а сам токен является открывающей скобкой или кавычкой
        /// </summary>
        /// <param name="t">проверяемый токен</param>
        /// <param name="quotesOnly">должны быть именно кавычка, а не скобка</param>
        /// <return>да-нет</return>
        public static bool CanBeStartOfSequence(Pullenti.Ner.Token t, bool quotesOnly = false, bool ignoreWhitespaces = false)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null || tt.Next == null) 
                return false;
            char ch = tt.Term[0];
            if (char.IsLetterOrDigit(ch)) 
                return false;
            if (quotesOnly && (m_Quotes.IndexOf(ch) < 0)) 
                return false;
            if (t.Next == null) 
                return false;
            if (m_OpenChars.IndexOf(ch) < 0) 
                return false;
            if (!ignoreWhitespaces) 
            {
                if (t.IsWhitespaceAfter) 
                {
                    if (!t.IsWhitespaceBefore) 
                    {
                        if (t.Previous != null && t.Previous.IsTableControlChar) 
                        {
                        }
                        else 
                            return false;
                    }
                    if (t.IsNewlineAfter) 
                        return false;
                }
                else if (!t.IsWhitespaceBefore) 
                {
                    if (char.IsLetterOrDigit(t.Kit.GetTextCharacter(t.BeginChar - 1))) 
                    {
                        if (t.Next != null && ((t.Next.Chars.IsAllLower || !t.Next.Chars.IsLetter))) 
                        {
                            if (ch != '(') 
                                return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Проверка, что на этом токене может заканчиваться последовательность, а сам токен является закрывающей скобкой или кавычкой
        /// </summary>
        /// <param name="t">проверяемый токен</param>
        /// <param name="quotesOnly">должны быть именно кавычка, а не скобка</param>
        /// <param name="openToken">это ссылка на токен, который был открывающим</param>
        /// <return>да-нет</return>
        public static bool CanBeEndOfSequence(Pullenti.Ner.Token t, bool quotesOnly = false, Pullenti.Ner.Token openToken = null, bool ignoreWhitespaces = false)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return false;
            char ch = tt.Term[0];
            if (char.IsLetterOrDigit(ch)) 
                return false;
            if (t.Previous == null) 
                return false;
            if (m_CloseChars.IndexOf(ch) < 0) 
                return false;
            if (quotesOnly) 
            {
                if (m_Quotes.IndexOf(ch) < 0) 
                    return false;
            }
            if (!ignoreWhitespaces) 
            {
                if (!t.IsWhitespaceAfter) 
                {
                    if (t.IsWhitespaceBefore) 
                    {
                        if (t.Next != null && t.Next.IsTableControlChar) 
                        {
                        }
                        else 
                            return false;
                    }
                    if (t.IsNewlineBefore) 
                        return false;
                }
                else if (t.IsWhitespaceBefore) 
                {
                    if (char.IsLetterOrDigit(t.Kit.GetTextCharacter(t.EndChar + 1))) 
                        return false;
                    if (!t.IsWhitespaceAfter) 
                        return false;
                }
            }
            if (openToken is Pullenti.Ner.TextToken) 
            {
                char ch0 = (openToken as Pullenti.Ner.TextToken).Term[0];
                int i = m_OpenChars.IndexOf(ch0);
                if (i < 0) 
                    return m_CloseChars.IndexOf(ch) < 0;
                int ii = m_CloseChars.IndexOf(ch);
                return ii == i;
            }
            return true;
        }
        /// <summary>
        /// Проверка символа, что он может быть скобкой или кавычкой
        /// </summary>
        /// <param name="ch">проверяемый символ</param>
        /// <param name="quotesOnly">должны быть именно кавычка, а не скобка</param>
        /// <return>да-нет</return>
        public static bool IsBracketChar(char ch, bool quotesOnly = false)
        {
            if (m_OpenChars.IndexOf(ch) >= 0 || m_CloseChars.IndexOf(ch) >= 0) 
            {
                if (!quotesOnly) 
                    return true;
                return m_Quotes.IndexOf(ch) >= 0;
            }
            return false;
        }
        /// <summary>
        /// Проверка токена, что он является скобкой или кавычкой
        /// </summary>
        /// <param name="t">проверяемый токен</param>
        /// <param name="quotesOnly">должны быть именно кавычка, а не скобка</param>
        /// <return>да-нет</return>
        public static bool IsBracket(Pullenti.Ner.Token t, bool quotesOnly = false)
        {
            if (t == null) 
                return false;
            if (t.IsCharOf(m_OpenChars)) 
            {
                if (quotesOnly) 
                {
                    if (t is Pullenti.Ner.TextToken) 
                    {
                        if (m_Quotes.IndexOf((t as Pullenti.Ner.TextToken).Term[0]) < 0) 
                            return false;
                    }
                }
                return true;
            }
            if (t.IsCharOf(m_CloseChars)) 
            {
                if (quotesOnly) 
                {
                    if (t is Pullenti.Ner.TextToken) 
                    {
                        if (m_Quotes.IndexOf((t as Pullenti.Ner.TextToken).Term[0]) < 0) 
                            return false;
                    }
                }
                return true;
            }
            return false;
        }
        class Bracket
        {
            public Bracket(Pullenti.Ner.Token t)
            {
                Source = t;
                if (t is Pullenti.Ner.TextToken) 
                    Char = (t as Pullenti.Ner.TextToken).Term[0];
                CanBeOpen = Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false);
                CanBeClose = Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t, false, null, false);
            }
            public Pullenti.Ner.Token Source;
            public char Char;
            public bool CanBeOpen;
            public bool CanBeClose;
            public override string ToString()
            {
                StringBuilder res = new StringBuilder();
                res.AppendFormat("!{0} ", Char);
                if (CanBeOpen) 
                    res.Append(" Open");
                if (CanBeClose) 
                    res.Append(" Close");
                return res.ToString();
            }
        }

        /// <summary>
        /// Попробовать восстановить последовательность, обрамляемую кавычками или скобками. Поддерживается 
        /// вложенность, возможность отсутствия закрывающего элемента и др.
        /// </summary>
        /// <param name="t">начальный токен</param>
        /// <param name="attrs">параметры выделения</param>
        /// <param name="maxTokens">максимально токенов (вдруг забыли закрывающую кавычку)</param>
        /// <return>метатокен BracketSequenceToken</return>
        public static BracketSequenceToken TryParse(Pullenti.Ner.Token t, BracketParseAttr attrs = BracketParseAttr.No, int maxTokens = 100)
        {
            Pullenti.Ner.Token t0 = t;
            int cou = 0;
            if (!CanBeStartOfSequence(t0, false, false)) 
                return null;
            List<Bracket> brList = new List<Bracket>();
            brList.Add(new Bracket(t0));
            cou = 0;
            int crlf = 0;
            Pullenti.Ner.Token last = null;
            int lev = 1;
            bool isAssim = brList[0].Char != '«' && m_AssymOPenChars.IndexOf(brList[0].Char) >= 0;
            bool genCase = false;
            for (t = t0.Next; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar) 
                    break;
                last = t;
                if (t.IsCharOf(m_OpenChars) || t.IsCharOf(m_CloseChars)) 
                {
                    if (t.IsNewlineBefore && ((attrs & BracketParseAttr.CanBeManyLines)) == BracketParseAttr.No) 
                    {
                        if (t.WhitespacesBeforeCount > 10 || CanBeStartOfSequence(t, false, false)) 
                        {
                            if (t.IsChar('(') && !t0.IsChar('(')) 
                            {
                            }
                            else 
                            {
                                last = t.Previous;
                                break;
                            }
                        }
                    }
                    Bracket bb = new Bracket(t);
                    brList.Add(bb);
                    if (brList.Count > 20) 
                        break;
                    if ((brList.Count == 3 && brList[1].CanBeOpen && bb.CanBeClose) && MustBeCloseChar(bb.Char, brList[1].Char) && MustBeCloseChar(bb.Char, brList[0].Char)) 
                    {
                        bool ok = false;
                        for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.IsNewlineBefore) 
                                break;
                            if (tt.IsChar(',')) 
                                break;
                            if (tt.IsChar('.')) 
                            {
                                for (tt = tt.Next; tt != null; tt = tt.Next) 
                                {
                                    if (tt.IsNewlineBefore) 
                                        break;
                                    else if (tt.IsCharOf(m_OpenChars) || tt.IsCharOf(m_CloseChars)) 
                                    {
                                        Bracket bb2 = new Bracket(tt);
                                        if (BracketHelper.CanBeEndOfSequence(tt, false, null, false) && CanBeCloseChar(bb2.Char, brList[0].Char)) 
                                            ok = true;
                                        break;
                                    }
                                }
                                break;
                            }
                            if (t.IsCharOf(m_OpenChars) || t.IsCharOf(m_CloseChars)) 
                            {
                                ok = true;
                                break;
                            }
                        }
                        if (!ok) 
                            break;
                    }
                    if (isAssim) 
                    {
                        if (bb.CanBeOpen && !bb.CanBeClose && bb.Char == brList[0].Char) 
                            lev++;
                        else if (bb.CanBeClose && !bb.CanBeOpen && m_OpenChars.IndexOf(brList[0].Char) == m_CloseChars.IndexOf(bb.Char)) 
                        {
                            lev--;
                            if (lev == 0) 
                                break;
                        }
                    }
                }
                else 
                {
                    if ((++cou) > maxTokens) 
                        break;
                    if (((attrs & BracketParseAttr.CanContainsVerbs)) == BracketParseAttr.No) 
                    {
                        if (t.Morph.Language.IsCyrillic) 
                        {
                            if (t.GetMorphClassInDictionary() == Pullenti.Morph.MorphClass.Verb) 
                            {
                                if (!t.Morph.Class.IsAdjective && !t.Morph.ContainsAttr("страд.з.", null)) 
                                {
                                    if (t.Chars.IsAllLower) 
                                    {
                                        string norm = t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                                        if (!Pullenti.Morph.LanguageHelper.EndsWith(norm, "СЯ")) 
                                        {
                                            if (brList.Count > 1) 
                                                break;
                                            if (brList[0].Char != '(') 
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (t.Morph.Language.IsEn) 
                        {
                            if (t.Morph.Class == Pullenti.Morph.MorphClass.Verb && t.Chars.IsAllLower) 
                                break;
                        }
                        Pullenti.Ner.Referent r = t.GetReferent();
                        if (r != null && r.TypeName == "ADDRESS") 
                        {
                            if (!t0.IsChar('(')) 
                                break;
                        }
                    }
                }
                if (((attrs & BracketParseAttr.CanBeManyLines)) != BracketParseAttr.No) 
                {
                    if (t.IsNewlineBefore) 
                    {
                        if (t.NewlinesBeforeCount > 1) 
                            break;
                        crlf++;
                    }
                    continue;
                }
                if (t.IsNewlineBefore) 
                {
                    if (t.WhitespacesBeforeCount > 15) 
                    {
                        last = t.Previous;
                        break;
                    }
                    crlf++;
                    if (!t.Chars.IsAllLower) 
                    {
                        if (MiscHelper.CanBeStartOfSentence(t)) 
                        {
                            bool has = false;
                            for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                            {
                                if (tt.IsNewlineBefore) 
                                    break;
                                else if (tt.LengthChar == 1 && tt.IsCharOf(m_OpenChars) && tt.IsWhitespaceBefore) 
                                    break;
                                else if (tt.LengthChar == 1 && tt.IsCharOf(m_CloseChars) && !tt.IsWhitespaceBefore) 
                                {
                                    has = true;
                                    break;
                                }
                            }
                            if (!has) 
                            {
                                last = t.Previous;
                                break;
                            }
                        }
                    }
                    if ((t.Previous is Pullenti.Ner.MetaToken) && CanBeEndOfSequence((t.Previous as Pullenti.Ner.MetaToken).EndToken, false, null, false)) 
                    {
                        last = t.Previous;
                        break;
                    }
                }
                if (crlf > 1) 
                {
                    if (brList.Count > 1) 
                        break;
                    if (crlf > 10) 
                        break;
                }
                if (t.IsChar(';') && t.IsNewlineAfter) 
                    break;
                NounPhraseToken npt = NounPhraseHelper.TryParse(t, NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    if (t.IsNewlineBefore) 
                        genCase = npt.Morph.Case.IsGenitive;
                    last = (t = npt.EndToken);
                }
            }
            if ((brList.Count == 1 && brList[0].CanBeOpen && (last is Pullenti.Ner.MetaToken)) && last.IsNewlineAfter) 
            {
                if (BracketHelper.CanBeEndOfSequence((last as Pullenti.Ner.MetaToken).EndToken, false, null, false)) 
                    return new BracketSequenceToken(t0, last);
            }
            if ((brList.Count == 1 && brList[0].CanBeOpen && genCase) && last.IsNewlineAfter && crlf <= 2) 
                return new BracketSequenceToken(t0, last);
            if (brList.Count < 1) 
                return null;
            for (int i = 1; i < (brList.Count - 1); i++) 
            {
                if (brList[i].Char == '<' && brList[i + 1].Char == '>') 
                {
                    brList[i].CanBeOpen = true;
                    brList[i + 1].CanBeClose = true;
                }
            }
            List<BracketSequenceToken> internals = null;
            while (brList.Count > 3) 
            {
                int i = brList.Count - 1;
                if ((brList[i].CanBeClose && brList[i - 1].CanBeOpen && !CanBeCloseChar(brList[i].Char, brList[0].Char)) && CanBeCloseChar(brList[i].Char, brList[i - 1].Char)) 
                {
                    brList.RemoveRange(brList.Count - 2, 2);
                    continue;
                }
                break;
            }
            while (brList.Count >= 4) 
            {
                bool changed = false;
                for (int i = 1; i < (brList.Count - 2); i++) 
                {
                    if ((brList[i].CanBeOpen && !brList[i].CanBeClose && brList[i + 1].CanBeClose) && !brList[i + 1].CanBeOpen) 
                    {
                        bool ok = false;
                        if (MustBeCloseChar(brList[i + 1].Char, brList[i].Char) || brList[i].Char != brList[0].Char) 
                        {
                            ok = true;
                            if ((i == 1 && ((i + 2) < brList.Count) && brList[i + 2].Char == ')') && brList[i + 1].Char != ')' && CanBeCloseChar(brList[i + 1].Char, brList[i - 1].Char)) 
                                brList[i + 2] = brList[i + 1];
                        }
                        else if (i > 1 && ((i + 2) < brList.Count) && MustBeCloseChar(brList[i + 2].Char, brList[i - 1].Char)) 
                            ok = true;
                        if (ok) 
                        {
                            if (internals == null) 
                                internals = new List<BracketSequenceToken>();
                            internals.Add(new BracketSequenceToken(brList[i].Source, brList[i + 1].Source));
                            brList.RemoveRange(i, 2);
                            changed = true;
                            break;
                        }
                    }
                }
                if (!changed) 
                    break;
            }
            BracketSequenceToken res = null;
            if ((brList.Count >= 4 && brList[1].CanBeOpen && brList[2].CanBeClose) && brList[3].CanBeClose && !brList[3].CanBeOpen) 
            {
                if (CanBeCloseChar(brList[3].Char, brList[0].Char)) 
                {
                    res = new BracketSequenceToken(brList[0].Source, brList[3].Source);
                    if (brList[0].Source.Next != brList[1].Source || brList[2].Source.Next != brList[3].Source) 
                        res.Internal.Add(new BracketSequenceToken(brList[1].Source, brList[2].Source));
                    if (internals != null) 
                        res.Internal.AddRange(internals);
                }
            }
            if ((res == null && brList.Count >= 3 && brList[2].CanBeClose) && !brList[2].CanBeOpen) 
            {
                if (((attrs & BracketParseAttr.NearCloseBracket)) != BracketParseAttr.No) 
                {
                    if (CanBeCloseChar(brList[1].Char, brList[0].Char)) 
                        return new BracketSequenceToken(brList[0].Source, brList[1].Source);
                }
                bool ok = true;
                if (CanBeCloseChar(brList[2].Char, brList[0].Char) && CanBeCloseChar(brList[1].Char, brList[0].Char) && brList[1].CanBeClose) 
                {
                    for (t = brList[1].Source; t != brList[2].Source && t != null; t = t.Next) 
                    {
                        if (t.IsNewlineBefore) 
                        {
                            ok = false;
                            break;
                        }
                        if (t.Chars.IsLetter && t.Chars.IsAllLower) 
                        {
                            ok = false;
                            break;
                        }
                        NounPhraseToken npt = NounPhraseHelper.TryParse(t, NounPhraseParseAttr.No, 0, null);
                        if (npt != null) 
                            t = npt.EndToken;
                    }
                    if (ok) 
                    {
                        for (t = brList[0].Source.Next; t != brList[1].Source && t != null; t = t.Next) 
                        {
                            if (t.IsNewlineBefore) 
                                return new BracketSequenceToken(brList[0].Source, t.Previous);
                        }
                    }
                    int lev1 = 0;
                    for (Pullenti.Ner.Token tt = brList[0].Source.Previous; tt != null; tt = tt.Previous) 
                    {
                        if (tt.IsNewlineAfter || tt.IsTableControlChar) 
                            break;
                        if (!(tt is Pullenti.Ner.TextToken)) 
                            continue;
                        if (tt.Chars.IsLetter || tt.LengthChar > 1) 
                            continue;
                        char ch = (tt as Pullenti.Ner.TextToken).Term[0];
                        if (CanBeCloseChar(ch, brList[0].Char)) 
                            lev1++;
                        else if (CanBeCloseChar(brList[1].Char, ch)) 
                        {
                            lev1--;
                            if (lev1 < 0) 
                                return new BracketSequenceToken(brList[0].Source, brList[1].Source);
                        }
                    }
                }
                if (ok && CanBeCloseChar(brList[2].Char, brList[0].Char)) 
                {
                    BracketSequenceToken intern = new BracketSequenceToken(brList[1].Source, brList[2].Source);
                    res = new BracketSequenceToken(brList[0].Source, brList[2].Source);
                    res.Internal.Add(intern);
                }
                else if (ok && CanBeCloseChar(brList[2].Char, brList[1].Char) && brList[0].CanBeOpen) 
                {
                    if (CanBeCloseChar(brList[2].Char, brList[0].Char)) 
                    {
                        BracketSequenceToken intern = new BracketSequenceToken(brList[1].Source, brList[2].Source);
                        res = new BracketSequenceToken(brList[0].Source, brList[2].Source);
                        res.Internal.Add(intern);
                    }
                    else if (brList.Count == 3) 
                        return null;
                }
            }
            if (res == null && brList.Count > 1 && brList[1].CanBeClose) 
                res = new BracketSequenceToken(brList[0].Source, brList[1].Source);
            if (res == null && brList.Count > 1 && CanBeCloseChar(brList[1].Char, brList[0].Char)) 
                res = new BracketSequenceToken(brList[0].Source, brList[1].Source);
            if (res == null && brList.Count == 2 && brList[0].Char == brList[1].Char) 
                res = new BracketSequenceToken(brList[0].Source, brList[1].Source);
            if (res != null && internals != null) 
            {
                foreach (BracketSequenceToken i in internals) 
                {
                    if (i.BeginChar < res.EndChar) 
                        res.Internal.Add(i);
                }
            }
            if (res == null) 
            {
                cou = 0;
                for (Pullenti.Ner.Token tt = t0.Next; tt != null; tt = tt.Next,cou++) 
                {
                    if (tt.IsTableControlChar) 
                        break;
                    if (MiscHelper.CanBeStartOfSentence(tt)) 
                        break;
                    if (maxTokens > 0 && cou > maxTokens) 
                        break;
                    Pullenti.Ner.MetaToken mt = tt as Pullenti.Ner.MetaToken;
                    if (mt == null) 
                        continue;
                    if (mt.EndToken is Pullenti.Ner.TextToken) 
                    {
                        if ((mt.EndToken as Pullenti.Ner.TextToken).IsCharOf(m_CloseChars)) 
                        {
                            Bracket bb = new Bracket(mt.EndToken as Pullenti.Ner.TextToken);
                            if (bb.CanBeClose && CanBeCloseChar(bb.Char, brList[0].Char)) 
                                return new BracketSequenceToken(t0, tt);
                        }
                    }
                }
            }
            return res;
        }
        static string m_OpenChars = "\"'`’<{([«“„”";
        static string m_CloseChars = "\"'`’>})]»”“";
        static string m_Quotes = "\"'`’«“<”„»>";
        static string m_AssymOPenChars = "<{([«";
        static bool CanBeCloseChar(char close, char open)
        {
            int i = m_OpenChars.IndexOf(open);
            if (i < 0) 
                return false;
            int j = m_CloseChars.IndexOf(close);
            return i == j;
        }
        static bool MustBeCloseChar(char close, char open)
        {
            if (m_AssymOPenChars.IndexOf(open) < 0) 
                return false;
            int i = m_OpenChars.IndexOf(open);
            int j = m_CloseChars.IndexOf(close);
            return i == j;
        }
    }
}