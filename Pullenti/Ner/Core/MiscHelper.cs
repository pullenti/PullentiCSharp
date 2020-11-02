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
    /// Разные полезные процедурки лингвистического анализа. Особо полезные функции выделены шрифтом.
    /// </summary>
    public static class MiscHelper
    {
        /// <summary>
        /// Сравнение, чтобы не было больше одной ошибки в написании. 
        /// Ошибка - это замена буквы или пропуск буквы.
        /// </summary>
        /// <param name="value">правильное написание</param>
        /// <param name="t">проверяемый токен</param>
        /// <return>да-нет</return>
        public static bool IsNotMoreThanOneError(string value, Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            if (t is Pullenti.Ner.TextToken) 
            {
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (t.IsValue(value, null)) 
                    return true;
                if (_isNotMoreThanOneError(value, tt.Term, true)) 
                    return true;
                foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
                {
                    if (wf is Pullenti.Morph.MorphWordForm) 
                    {
                        if (_isNotMoreThanOneError(value, (wf as Pullenti.Morph.MorphWordForm).NormalCase, true)) 
                            return true;
                    }
                }
            }
            else if ((t is Pullenti.Ner.MetaToken) && (t as Pullenti.Ner.MetaToken).BeginToken == (t as Pullenti.Ner.MetaToken).EndToken) 
                return IsNotMoreThanOneError(value, (t as Pullenti.Ner.MetaToken).BeginToken);
            else if (_isNotMoreThanOneError(value, t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false), true)) 
                return true;
            return false;
        }
        static bool _isNotMoreThanOneError(string pattern, string test, bool tmp = false)
        {
            if (test == null || pattern == null) 
                return false;
            if (test.Length == pattern.Length) 
            {
                int cou = 0;
                for (int i = 0; i < pattern.Length; i++) 
                {
                    if (pattern[i] != test[i]) 
                    {
                        if ((++cou) > 1) 
                            return false;
                    }
                }
                return true;
            }
            if (test.Length == (pattern.Length - 1)) 
            {
                int i;
                for (i = 0; i < test.Length; i++) 
                {
                    if (pattern[i] != test[i]) 
                        break;
                }
                if (i < 2) 
                    return false;
                if (i == test.Length) 
                    return true;
                for (; i < test.Length; i++) 
                {
                    if (pattern[i + 1] != test[i]) 
                        return false;
                }
                return true;
            }
            if (!tmp && (test.Length - 1) == pattern.Length) 
            {
                int i;
                for (i = 0; i < pattern.Length; i++) 
                {
                    if (pattern[i] != test[i]) 
                        break;
                }
                if (i < 2) 
                    return false;
                if (i == pattern.Length) 
                    return true;
                for (; i < pattern.Length; i++) 
                {
                    if (pattern[i] != test[i + 1]) 
                        return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Проверить написание слова вразбивку по буквам (например:   П Р И К А З)
        /// </summary>
        /// <param name="word">проверяемое слово</param>
        /// <param name="t">начальный токен</param>
        /// <param name="useMorphVariants">перебирать ли падежи у слова</param>
        /// <return>токен последней буквы или null</return>
        public static Pullenti.Ner.Token TryAttachWordByLetters(string word, Pullenti.Ner.Token t, bool useMorphVariants = false)
        {
            Pullenti.Ner.TextToken t1 = t as Pullenti.Ner.TextToken;
            if (t1 == null) 
                return null;
            int i = 0;
            int j;
            for (; t1 != null; t1 = t1.Next as Pullenti.Ner.TextToken) 
            {
                string s = t1.Term;
                for (j = 0; (j < s.Length) && ((i + j) < word.Length); j++) 
                {
                    if (word[i + j] != s[j]) 
                        break;
                }
                if (j < s.Length) 
                {
                    if (!useMorphVariants) 
                        return null;
                    if (i < 5) 
                        return null;
                    StringBuilder tmp = new StringBuilder();
                    tmp.Append(word.Substring(0, i));
                    for (Pullenti.Ner.Token tt = (Pullenti.Ner.Token)t1; tt != null; tt = tt.Next) 
                    {
                        if (!(tt is Pullenti.Ner.TextToken) || !tt.Chars.IsLetter || tt.IsNewlineBefore) 
                            break;
                        t1 = tt as Pullenti.Ner.TextToken;
                        tmp.Append(t1.Term);
                    }
                    List<Pullenti.Morph.MorphToken> li = Pullenti.Morph.MorphologyService.Process(tmp.ToString(), t.Morph.Language, null);
                    if (li != null) 
                    {
                        foreach (Pullenti.Morph.MorphToken l in li) 
                        {
                            if (l.WordForms != null) 
                            {
                                foreach (Pullenti.Morph.MorphWordForm wf in l.WordForms) 
                                {
                                    if (wf.NormalCase == word || wf.NormalFull == word) 
                                        return t1;
                                }
                            }
                        }
                    }
                    return null;
                }
                i += j;
                if (i == word.Length) 
                    return t1;
            }
            return null;
        }
        /// <summary>
        /// Сравнение 2-х строк на предмет равенства с учётом морфологии и пунктуации (то есть инвариантно относительно них). 
        /// Функция довольно трудоёмка, не использовать без крайней необходимости. 
        /// ВНИМАНИЕ! Вместо этой функции теперь используйте CanBeEqualsEx.
        /// </summary>
        /// <param name="s1">первая строка</param>
        /// <param name="s2">вторая строка</param>
        /// <param name="ignoreNonletters">игнорировать небуквенные символы</param>
        /// <param name="ignoreCase">игнорировать регистр символов</param>
        /// <param name="checkMorphEquAfterFirstNoun">после первого существительного слова должны полностью совпадать</param>
        /// <return>да-нет</return>
        public static bool CanBeEquals(string s1, string s2, bool ignoreNonletters = true, bool ignoreCase = true, bool checkMorphEquAfterFirstNoun = false)
        {
            CanBeEqualsAttr attrs = CanBeEqualsAttr.No;
            if (ignoreNonletters) 
                attrs |= CanBeEqualsAttr.IgnoreNonletters;
            if (ignoreCase) 
                attrs |= CanBeEqualsAttr.IgnoreUppercase;
            if (checkMorphEquAfterFirstNoun) 
                attrs |= CanBeEqualsAttr.CheckMorphEquAfterFirstNoun;
            return CanBeEqualsEx(s1, s2, attrs);
        }
        /// <summary>
        /// Сравнение 2-х строк на предмет равенства с учётом морфологии и пунктуации (то есть инвариантно относительно них). 
        /// Функция довольно трудоёмка, не использовать без крайней необходимости.
        /// </summary>
        /// <param name="s1">первая строка</param>
        /// <param name="s2">вторая строка</param>
        /// <param name="attrs">дополнительные атрибуты</param>
        /// <return>да-нет</return>
        public static bool CanBeEqualsEx(string s1, string s2, CanBeEqualsAttr attrs)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) 
                return false;
            if (s1 == s2) 
                return true;
            AnalysisKit ak1 = new AnalysisKit(new Pullenti.Ner.SourceOfAnalysis(s1));
            AnalysisKit ak2 = new AnalysisKit(new Pullenti.Ner.SourceOfAnalysis(s2));
            Pullenti.Ner.Token t1 = ak1.FirstToken;
            Pullenti.Ner.Token t2 = ak2.FirstToken;
            bool wasNoun = false;
            while (t1 != null || t2 != null) 
            {
                if (t1 != null) 
                {
                    if (t1 is Pullenti.Ner.TextToken) 
                    {
                        if (!t1.Chars.IsLetter && !t1.IsChar('№')) 
                        {
                            if (BracketHelper.IsBracket(t1, false) && ((attrs & CanBeEqualsAttr.UseBrackets)) != CanBeEqualsAttr.No) 
                            {
                            }
                            else 
                            {
                                if (t1.IsHiphen) 
                                    wasNoun = false;
                                if (((!t1.IsCharOf("()") && !t1.IsHiphen)) || ((attrs & CanBeEqualsAttr.IgnoreNonletters)) != CanBeEqualsAttr.No) 
                                {
                                    t1 = t1.Next;
                                    continue;
                                }
                            }
                        }
                    }
                }
                if (t2 != null) 
                {
                    if (t2 is Pullenti.Ner.TextToken) 
                    {
                        if (!t2.Chars.IsLetter && !t2.IsChar('№')) 
                        {
                            if (BracketHelper.IsBracket(t2, false) && ((attrs & CanBeEqualsAttr.UseBrackets)) != CanBeEqualsAttr.No) 
                            {
                            }
                            else 
                            {
                                if (t2.IsHiphen) 
                                    wasNoun = false;
                                if (((!t2.IsCharOf("()") && !t2.IsHiphen)) || ((attrs & CanBeEqualsAttr.IgnoreNonletters)) != CanBeEqualsAttr.No) 
                                {
                                    t2 = t2.Next;
                                    continue;
                                }
                            }
                        }
                    }
                }
                if (t1 is Pullenti.Ner.NumberToken) 
                {
                    if (!(t2 is Pullenti.Ner.NumberToken)) 
                        break;
                    if ((t1 as Pullenti.Ner.NumberToken).Value != (t2 as Pullenti.Ner.NumberToken).Value) 
                        break;
                    t1 = t1.Next;
                    t2 = t2.Next;
                    continue;
                }
                if (!(t1 is Pullenti.Ner.TextToken) || !(t2 is Pullenti.Ner.TextToken)) 
                    break;
                if (((attrs & CanBeEqualsAttr.IgnoreUppercase)) == CanBeEqualsAttr.No) 
                {
                    if (t1.Previous == null && ((attrs & CanBeEqualsAttr.IgnoreUppercaseFirstWord)) != CanBeEqualsAttr.No) 
                    {
                    }
                    else if (t1.Chars != t2.Chars) 
                        return false;
                }
                if (!t1.Chars.IsLetter) 
                {
                    bool bs1 = BracketHelper.CanBeStartOfSequence(t1, false, false);
                    bool bs2 = BracketHelper.CanBeStartOfSequence(t2, false, false);
                    if (bs1 != bs2) 
                        return false;
                    if (bs1) 
                    {
                        t1 = t1.Next;
                        t2 = t2.Next;
                        continue;
                    }
                    bs1 = BracketHelper.CanBeEndOfSequence(t1, false, null, false);
                    bs2 = BracketHelper.CanBeEndOfSequence(t2, false, null, false);
                    if (bs1 != bs2) 
                        return false;
                    if (bs1) 
                    {
                        t1 = t1.Next;
                        t2 = t2.Next;
                        continue;
                    }
                    if (t1.IsHiphen && t2.IsHiphen) 
                    {
                    }
                    else if ((t1 as Pullenti.Ner.TextToken).Term != (t2 as Pullenti.Ner.TextToken).Term) 
                        return false;
                    t1 = t1.Next;
                    t2 = t2.Next;
                    continue;
                }
                bool ok = false;
                if (wasNoun && ((attrs & CanBeEqualsAttr.CheckMorphEquAfterFirstNoun)) != CanBeEqualsAttr.No) 
                {
                    if ((t1 as Pullenti.Ner.TextToken).Term == (t2 as Pullenti.Ner.TextToken).Term) 
                        ok = true;
                }
                else 
                {
                    Pullenti.Ner.TextToken tt = t1 as Pullenti.Ner.TextToken;
                    foreach (Pullenti.Morph.MorphBaseInfo it in tt.Morph.Items) 
                    {
                        if (it is Pullenti.Morph.MorphWordForm) 
                        {
                            Pullenti.Morph.MorphWordForm wf = it as Pullenti.Morph.MorphWordForm;
                            if (t2.IsValue(wf.NormalCase, null) || t2.IsValue(wf.NormalFull, null)) 
                            {
                                ok = true;
                                break;
                            }
                        }
                    }
                    if (tt.GetMorphClassInDictionary().IsNoun) 
                        wasNoun = true;
                    if (!ok && t1.IsHiphen && t2.IsHiphen) 
                        ok = true;
                    if (!ok) 
                    {
                        if (t2.IsValue(tt.Term, null) || t2.IsValue(tt.Lemma, null)) 
                            ok = true;
                    }
                }
                if (ok) 
                {
                    t1 = t1.Next;
                    t2 = t2.Next;
                    continue;
                }
                break;
            }
            if (((attrs & CanBeEqualsAttr.FirstCanBeShorter)) != CanBeEqualsAttr.No) 
            {
                if (t1 == null) 
                    return true;
            }
            return t1 == null && t2 == null;
        }
        /// <summary>
        /// Проверка того, может ли здесь начинаться новое предложение. Для проверки токена 
        /// конца предложения используйте CanBeStartOfSentence(t.Next) проверку на начало следующего в цепочке токена.
        /// </summary>
        /// <param name="t">токен начала предложения</param>
        /// <return>да-нет</return>
        public static bool CanBeStartOfSentence(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            if (t.Previous == null) 
                return true;
            if (!t.IsWhitespaceBefore) 
            {
                if (t.Previous != null && t.Previous.IsTableControlChar) 
                {
                }
                else 
                    return false;
            }
            if (t.Chars.IsLetter && t.Chars.IsAllLower) 
            {
                if (t.Previous.Chars.IsLetter && t.Previous.Chars.IsAllLower) 
                    return false;
                if (((t.Previous.IsHiphen || t.Previous.IsComma)) && !t.Previous.IsWhitespaceBefore && t.Previous.Previous != null) 
                {
                    if (t.Previous.Previous.Chars.IsLetter && t.Previous.Previous.Chars.IsAllLower) 
                        return false;
                }
            }
            if (t.WhitespacesBeforeCount > 25 || t.NewlinesBeforeCount > 2) 
                return true;
            if (t.Previous.IsCommaAnd || t.Previous.Morph.Class.IsConjunction) 
                return false;
            if (MiscHelper.IsEngArticle(t.Previous)) 
                return false;
            if (t.Previous.IsChar(':')) 
                return false;
            if (t.Previous.IsChar(';') && t.IsNewlineBefore) 
                return true;
            if (t.Previous.IsHiphen) 
            {
                if (t.Previous.IsNewlineBefore) 
                    return true;
                Pullenti.Ner.Token pp = t.Previous.Previous;
                if (pp != null && pp.IsChar('.')) 
                    return true;
            }
            if (t.Chars.IsLetter && t.Chars.IsAllLower) 
                return false;
            if (t.IsNewlineBefore) 
                return true;
            if (t.Previous.IsCharOf("!?") || t.Previous.IsTableControlChar) 
                return true;
            if (t.Previous.IsChar('.') || (((t.Previous is Pullenti.Ner.ReferentToken) && (t.Previous as Pullenti.Ner.ReferentToken).EndToken.IsChar('.')))) 
            {
                if (t.WhitespacesBeforeCount > 1) 
                    return true;
                if (t.Next != null && t.Next.IsChar('.')) 
                {
                    if ((t.Previous.Previous is Pullenti.Ner.TextToken) && t.Previous.Previous.Chars.IsAllLower && !(t.Previous is Pullenti.Ner.ReferentToken)) 
                    {
                    }
                    else if (t.Previous.Previous is Pullenti.Ner.ReferentToken) 
                    {
                    }
                    else 
                        return false;
                }
                if ((t.Previous.Previous is Pullenti.Ner.NumberToken) && t.Previous.IsWhitespaceBefore) 
                {
                    if ((t.Previous.Previous as Pullenti.Ner.NumberToken).Typ != Pullenti.Ner.NumberSpellingType.Words) 
                        return false;
                }
                return true;
            }
            if (MiscHelper.IsEngArticle(t)) 
                return true;
            return false;
        }
        /// <summary>
        /// Переместиться на конец предложения
        /// </summary>
        /// <param name="t">токен, с которого идёт поиск</param>
        /// <return>последний токен предложения (не обязательно точка!)</return>
        public static Pullenti.Ner.Token FindEndOfSentence(Pullenti.Ner.Token t)
        {
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (tt.Next == null) 
                    return tt;
                else if (CanBeStartOfSentence(tt)) 
                    return (tt == t ? t : tt.Previous);
            }
            return null;
        }
        /// <summary>
        /// Проверка различных способов написания ключевых слов для номеров (ном., №, рег.номер и пр.)
        /// </summary>
        /// <param name="t">начало префикса</param>
        /// <return>null, если не префикс, или токен, следующий сразу за префиксом номера</return>
        public static Pullenti.Ner.Token CheckNumberPrefix(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            string s = (t as Pullenti.Ner.TextToken).Term;
            Pullenti.Ner.Token t1 = null;
            if (t.IsValue("ПО", null) && t.Next != null) 
                t = t.Next;
            if ((((t.IsValue("РЕГИСТРАЦИОННЫЙ", "РЕЄСТРАЦІЙНИЙ") || t.IsValue("ГОСУДАРСТВЕННЫЙ", "ДЕРЖАВНИЙ") || t.IsValue("ТРАНЗИТНЫЙ", "ТРАНЗИТНИЙ")) || t.IsValue("ДЕЛО", null) || t.IsValue("СПРАВА", null))) && (t.Next is Pullenti.Ner.TextToken)) 
            {
                t = t.Next;
                s = (t as Pullenti.Ner.TextToken).Term;
            }
            else if (s == "РЕГ" || s == "ГОС" || s == "ТРАНЗ") 
            {
                if (t.Next != null && t.Next.IsChar('.')) 
                    t = t.Next;
                if (t.Next is Pullenti.Ner.TextToken) 
                {
                    t = t.Next;
                    s = (t as Pullenti.Ner.TextToken).Term;
                }
                else 
                    return null;
            }
            if (((s == "НОМЕР" || s == "№" || s == "N") || s == "NO" || s == "NN") || s == "НР") 
            {
                t1 = t.Next;
                if (t1 != null && ((t1.IsCharOf("°№") || t1.IsValue("О", null)))) 
                    t1 = t1.Next;
                if (t1 != null && t1.IsChar('.')) 
                    t1 = t1.Next;
                if (t1 != null && t1.IsChar(':')) 
                    t1 = t1.Next;
            }
            else if (s == "НОМ") 
            {
                t1 = t.Next;
                if (t1 != null && t1.IsChar('.')) 
                    t1 = t1.Next;
            }
            while (t1 != null) 
            {
                if (t1.IsValue("ЗАПИСЬ", null)) 
                    t1 = t1.Next;
                else if (t1.IsValue("В", null) && t1.Next != null && t1.Next.IsValue("РЕЕСТР", null)) 
                    t1 = t1.Next.Next;
                else 
                    break;
            }
            return t1;
        }
        public static string _corrXmlText(string txt)
        {
            if (txt == null) 
                return "";
            foreach (char c in txt) 
            {
                if (((((int)c) < 0x20) && c != '\r' && c != '\n') && c != '\t') 
                {
                    StringBuilder tmp = new StringBuilder(txt);
                    for (int i = 0; i < tmp.Length; i++) 
                    {
                        char ch = tmp[i];
                        if (((((int)ch) < 0x20) && ch != '\r' && ch != '\n') && ch != '\t') 
                            tmp[i] = ' ';
                    }
                    return tmp.ToString();
                }
            }
            return txt;
        }
        /// <summary>
        /// Преобразовать строку, чтобы первая буква стала большой, остальные маленькие
        /// </summary>
        /// <param name="str">преобразуемая строка</param>
        /// <return>преобразованная строка</return>
        public static string ConvertFirstCharUpperAndOtherLower(string str)
        {
            if (string.IsNullOrEmpty(str)) 
                return str;
            StringBuilder fStrTmp = new StringBuilder();
            fStrTmp.Append(str.ToLower());
            int i;
            bool up = true;
            fStrTmp.Replace(" .", ".");
            for (i = 0; i < fStrTmp.Length; i++) 
            {
                if (char.IsLetter(fStrTmp[i])) 
                {
                    if (up) 
                    {
                        if (((i + 1) >= fStrTmp.Length || char.IsLetter(fStrTmp[i + 1]) || ((fStrTmp[i + 1] == '.' || fStrTmp[i + 1] == '-'))) || i == 0) 
                            fStrTmp[i] = char.ToUpper(fStrTmp[i]);
                    }
                    up = false;
                }
                else if (!char.IsDigit(fStrTmp[i])) 
                    up = true;
            }
            fStrTmp.Replace(" - ", "-");
            return fStrTmp.ToString();
        }
        /// <summary>
        /// Сделать аббревиатуру для строки из нескольких слов
        /// </summary>
        /// <param name="name">строка</param>
        /// <return>аббревиатура</return>
        public static string GetAbbreviation(string name)
        {
            StringBuilder abbr = new StringBuilder();
            int i;
            int j;
            for (i = 0; i < name.Length; i++) 
            {
                if (char.IsDigit(name[i])) 
                    break;
                else if (char.IsLetter(name[i])) 
                {
                    for (j = i + 1; j < name.Length; j++) 
                    {
                        if (!char.IsLetter(name[j])) 
                            break;
                    }
                    if ((j - i) > 2) 
                    {
                        string w = name.Substring(i, j - i);
                        if (w != "ПРИ") 
                            abbr.Append(name[i]);
                    }
                    i = j;
                }
            }
            if (abbr.Length < 2) 
                return null;
            return abbr.ToString().ToUpper();
        }
        // Получить аббревиатуру (уже не помню, какую именно...)
        public static string GetTailAbbreviation(string name)
        {
            int i;
            int j = 0;
            for (i = 0; i < name.Length; i++) 
            {
                if (name[i] == ' ') 
                    j++;
            }
            if (j < 2) 
                return null;
            char a0 = (char)0;
            char a1 = (char)0;
            j = 0;
            for (i = name.Length - 2; i > 0; i--) 
            {
                if (name[i] == ' ') 
                {
                    int le = 0;
                    for (int jj = i + 1; jj < name.Length; jj++) 
                    {
                        if (name[jj] == ' ') 
                            break;
                        else 
                            le++;
                    }
                    if (le < 4) 
                        break;
                    if (j == 0) 
                        a1 = name[i + 1];
                    else if (j == 1) 
                    {
                        a0 = name[i + 1];
                        if (char.IsLetter(a0) && char.IsLetter(a1)) 
                            return string.Format("{0} {1}{2}", name.Substring(0, i), a0, a1);
                        break;
                    }
                    j++;
                }
            }
            return null;
        }
        /// <summary>
        /// Попытка через транслитеральную замену сделать альтернативное написание строки 
        /// Например, А-10 => A-10  (здесь латиница и кириллица).
        /// </summary>
        /// <param name="str">исходная строка</param>
        /// <return>если null, то не получается (значит, есть непереводимые буквы)</return>
        public static string CreateCyrLatAlternative(string str)
        {
            if (str == null) 
                return null;
            int cyr = 0;
            int cyrToLat = 0;
            int lat = 0;
            int latToCyr = 0;
            for (int i = 0; i < str.Length; i++) 
            {
                char ch = str[i];
                if (Pullenti.Morph.LanguageHelper.IsLatinChar(ch)) 
                {
                    lat++;
                    if (Pullenti.Morph.LanguageHelper.GetCyrForLat(ch) != ((char)0)) 
                        latToCyr++;
                }
                else if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(ch)) 
                {
                    cyr++;
                    if (Pullenti.Morph.LanguageHelper.GetLatForCyr(ch) != ((char)0)) 
                        cyrToLat++;
                }
            }
            if (cyr > 0 && cyrToLat == cyr) 
            {
                if (lat > 0) 
                    return null;
                StringBuilder tmp = new StringBuilder(str);
                for (int i = 0; i < tmp.Length; i++) 
                {
                    if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(tmp[i])) 
                        tmp[i] = Pullenti.Morph.LanguageHelper.GetLatForCyr(tmp[i]);
                }
                return tmp.ToString();
            }
            if (lat > 0 && latToCyr == lat) 
            {
                if (cyr > 0) 
                    return null;
                StringBuilder tmp = new StringBuilder(str);
                for (int i = 0; i < tmp.Length; i++) 
                {
                    if (Pullenti.Morph.LanguageHelper.IsLatinChar(tmp[i])) 
                        tmp[i] = Pullenti.Morph.LanguageHelper.GetCyrForLat(tmp[i]);
                }
                return tmp.ToString();
            }
            return null;
        }
        /// <summary>
        /// Преобразовать слово, написанное по латыни, в варианты на русском языке. 
        /// Например, "Mikhail" -> "Михаил"
        /// </summary>
        /// <param name="str">строка на латыни</param>
        /// <return>варианты на русском языке</return>
        public static List<string> ConvertLatinWordToRussianVariants(string str)
        {
            return _ConvertWord(str, true);
        }
        /// <summary>
        /// Преобразовать слово, написанное в кириллице, в варианты на латинице.
        /// </summary>
        /// <param name="str">строка на кириллице</param>
        /// <return>варианты на латинице</return>
        public static List<string> ConvertRussianWordToLatinVariants(string str)
        {
            return _ConvertWord(str, false);
        }
        static List<string> _ConvertWord(string str, bool latinToRus)
        {
            if (str == null) 
                return null;
            if (str.Length == 0) 
                return null;
            str = str.ToUpper();
            List<string> res = new List<string>();
            List<List<string>> vars = new List<List<string>>();
            int i;
            int j;
            for (i = 0; i < str.Length; i++) 
            {
                List<string> v = new List<string>();
                if (latinToRus) 
                    j = Pullenti.Ner.Core.Internal.RusLatAccord.FindAccordsLatToRus(str, i, v);
                else 
                    j = Pullenti.Ner.Core.Internal.RusLatAccord.FindAccordsRusToLat(str, i, v);
                if (j < 1) 
                {
                    j = 1;
                    v.Add(str.Substring(i, 1));
                }
                vars.Add(v);
                i += (j - 1);
            }
            if (latinToRus && ("AEIJOUY".IndexOf(str[str.Length - 1]) < 0)) 
            {
                List<string> v = new List<string>();
                v.Add("");
                v.Add("Ь");
                vars.Add(v);
            }
            StringBuilder fStrTmp = new StringBuilder();
            List<int> inds = new List<int>(vars.Count);
            for (i = 0; i < vars.Count; i++) 
            {
                inds.Add(0);
            }
            while (true) 
            {
                fStrTmp.Length = 0;
                for (i = 0; i < vars.Count; i++) 
                {
                    if (vars[i].Count > 0) 
                        fStrTmp.Append(vars[i][inds[i]]);
                }
                res.Add(fStrTmp.ToString());
                for (i = inds.Count - 1; i >= 0; i--) 
                {
                    inds[i]++;
                    if (inds[i] < vars[i].Count) 
                        break;
                    inds[i] = 0;
                }
                if (i < 0) 
                    break;
            }
            return res;
        }
        /// <summary>
        /// Получение абсолютного нормализованного значения (с учётом гласных, удалением невидимых знаков и т.п.). 
        /// Используется для сравнений различных вариантов написаний. 
        /// Преобразования:  гласные заменяются на *, Щ на Ш, Х на Г, одинаковые соседние буквы сливаются, 
        /// Ъ и Ь выбрасываются. 
        /// Например, ХАБИБУЛЛИН -  Г*Б*Б*Л*Н
        /// </summary>
        /// <param name="str">строка</param>
        /// <return>если null, то не удалось нормализовать (слишком короткий)</return>
        public static string GetAbsoluteNormalValue(string str, bool getAlways = false)
        {
            StringBuilder res = new StringBuilder();
            int k = 0;
            for (int i = 0; i < str.Length; i++) 
            {
                if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(str[i]) || str[i] == 'Й' || Pullenti.Morph.LanguageHelper.IsLatinVowel(str[i])) 
                {
                    if (res.Length > 0 && res[res.Length - 1] == '*') 
                    {
                    }
                    else 
                        res.Append('*');
                }
                else if (str[i] != 'Ь' && str[i] != 'Ъ') 
                {
                    char ch = str[i];
                    if (ch == 'Щ') 
                        ch = 'Ш';
                    if (ch == 'Х') 
                        ch = 'Г';
                    if (ch == ' ') 
                        ch = '-';
                    res.Append(ch);
                    k++;
                }
            }
            if (res.Length > 0 && res[res.Length - 1] == '*') 
                res.Length -= 1;
            for (int i = res.Length - 1; i > 0; i--) 
            {
                if (res[i] == res[i - 1] && res[i] != '*') 
                    res.Remove(i, 1);
            }
            for (int i = res.Length - 1; i > 0; i--) 
            {
                if (res[i - 1] == '*' && res[i] == '-') 
                    res.Remove(i - 1, 1);
            }
            if (!getAlways) 
            {
                if ((res.Length < 3) || (k < 2)) 
                    return null;
            }
            return res.ToString();
        }
        /// <summary>
        /// Проверка, что хотя бы одно из слов внутри заданного диапазона находится в морфологическом словаре
        /// </summary>
        /// <param name="begin">начальный токен</param>
        /// <param name="end">конечный токен</param>
        /// <param name="cla">проверяемая часть речи</param>
        /// <return>да-нет</return>
        public static bool IsExistsInDictionary(Pullenti.Ner.Token begin, Pullenti.Ner.Token end, Pullenti.Morph.MorphClass cla)
        {
            bool ret = false;
            for (Pullenti.Ner.Token t = begin; t != null; t = t.Next) 
            {
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt != null) 
                {
                    if (tt.IsHiphen) 
                        ret = false;
                    foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
                    {
                        if (cla.Value == 0 || ((cla.Value & wf.Class.Value)) != 0) 
                        {
                            if ((wf is Pullenti.Morph.MorphWordForm) && (wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                            {
                                ret = true;
                                break;
                            }
                        }
                    }
                }
                if (t == end) 
                    break;
            }
            return ret;
        }
        /// <summary>
        /// Проверка, что токен - "одушевлённая" словоформа
        /// </summary>
        /// <param name="t">токен</param>
        /// <return>да-нет</return>
        public static bool IsTokenAnimate(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
            {
                if (t is Pullenti.Ner.MetaToken) 
                    return IsTokenAnimate((t as Pullenti.Ner.MetaToken).EndToken);
                return false;
            }
            if (t.Morph.ContainsAttr("одуш.", null)) 
                return true;
            List<Pullenti.Semantic.Utils.DerivateWord> ww = Pullenti.Semantic.Utils.DerivateService.FindWords((t as Pullenti.Ner.TextToken).Lemma, null);
            if (ww != null) 
            {
                foreach (Pullenti.Semantic.Utils.DerivateWord w in ww) 
                {
                    if (w.Attrs.IsAnimal || w.Attrs.IsAnimated || w.Attrs.IsMan) 
                        return true;
                }
            }
            return false;
        }
        // Проверка, что все в заданном диапазоне в нижнем регистре
        public static bool IsAllCharactersLower(Pullenti.Ner.Token begin, Pullenti.Ner.Token end, bool errorIfNotText = false)
        {
            for (Pullenti.Ner.Token t = begin; t != null; t = t.Next) 
            {
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                {
                    if (errorIfNotText) 
                        return false;
                }
                else if (!tt.Chars.IsAllLower) 
                    return false;
                if (t == end) 
                    break;
            }
            return true;
        }
        /// <summary>
        /// Проверка, что текстовой токен имеет хотя бы одну гласную
        /// </summary>
        /// <param name="t">токен</param>
        /// <return>да-нет</return>
        public static bool HasVowel(Pullenti.Ner.TextToken t)
        {
            if (t == null) 
                return false;
            string tmp = t.Term.Normalize(NormalizationForm.FormD);
            foreach (char ch in tmp) 
            {
                if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(ch) || Pullenti.Morph.LanguageHelper.IsLatinVowel(ch)) 
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Проверка акронима, что из первых букв слов диапазона может получиться проверяемый акроним. 
        /// Например,  РФ = Российская Федерация, ГосПлан = государственный план
        /// </summary>
        /// <param name="acr">акроним</param>
        /// <param name="begin">начало диапазона</param>
        /// <param name="end">конец диапазона</param>
        /// <return>да-нет</return>
        public static bool TestAcronym(Pullenti.Ner.Token acr, Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            if (!(acr is Pullenti.Ner.TextToken)) 
                return false;
            if (begin == null || end == null || begin.EndChar >= end.BeginChar) 
                return false;
            string str = (acr as Pullenti.Ner.TextToken).Term;
            int i = 0;
            for (Pullenti.Ner.Token t = begin; t != null && t.Previous != end; t = t.Next) 
            {
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                    break;
                if (i >= str.Length) 
                    return false;
                string s = tt.Term;
                if (s[0] != str[i]) 
                    return false;
                i++;
            }
            if (i >= str.Length) 
                return true;
            return false;
        }
        public class CyrLatWord
        {
            public string CyrWord;
            public string LatWord;
            public override string ToString()
            {
                if (CyrWord != null && LatWord != null) 
                    return string.Format("{0}\\{1}", CyrWord, LatWord);
                else if (CyrWord != null) 
                    return CyrWord;
                else 
                    return LatWord ?? "?";
            }
            public int Length
            {
                get
                {
                    return (CyrWord != null ? CyrWord.Length : (LatWord != null ? LatWord.Length : 0));
                }
            }
        }

        // Получить вариант на кириллице и\или латинице
        public static CyrLatWord GetCyrLatWord(Pullenti.Ner.Token t, int maxLen = 0)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
            {
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                if ((rt != null && (rt.LengthChar < 3) && rt.BeginToken == rt.EndToken) && (rt.BeginToken is Pullenti.Ner.TextToken)) 
                    tt = rt.BeginToken as Pullenti.Ner.TextToken;
                else 
                    return null;
            }
            if (!tt.Chars.IsLetter) 
                return null;
            string str = tt.GetSourceText();
            if (maxLen > 0 && str.Length > maxLen) 
                return null;
            StringBuilder cyr = new StringBuilder();
            StringBuilder lat = new StringBuilder();
            foreach (char s in str) 
            {
                if (Pullenti.Morph.LanguageHelper.IsLatinChar(s)) 
                {
                    if (lat != null) 
                        lat.Append(s);
                    int i = m_Lat.IndexOf(s);
                    if (i < 0) 
                        cyr = null;
                    else if (cyr != null) 
                        cyr.Append(m_Cyr[i]);
                }
                else if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(s)) 
                {
                    if (cyr != null) 
                        cyr.Append(s);
                    int i = m_Cyr.IndexOf(s);
                    if (i < 0) 
                        lat = null;
                    else if (lat != null) 
                        lat.Append(m_Lat[i]);
                }
                else 
                    return null;
            }
            if (cyr == null && lat == null) 
                return null;
            CyrLatWord res = new CyrLatWord();
            if (cyr != null) 
                res.CyrWord = cyr.ToString().ToUpper();
            if (lat != null) 
                res.LatWord = lat.ToString().ToUpper();
            return res;
        }
        static string m_Cyr = "АВДЕКМНОРСТХаекорсух";
        static string m_Lat = "ABDEKMHOPCTXaekopcyx";
        /// <summary>
        /// Проверка на возможную эквивалентность русского и латинского написания одного и того же слова
        /// </summary>
        /// <param name="t">токен</param>
        /// <param name="str">проверяемая строка</param>
        /// <return>да-нет</return>
        public static bool CanBeEqualCyrAndLatTS(Pullenti.Ner.Token t, string str)
        {
            if (t == null || string.IsNullOrEmpty(str)) 
                return false;
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return false;
            if (CanBeEqualCyrAndLatSS(tt.Term, str)) 
                return true;
            foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
            {
                if ((wf is Pullenti.Morph.MorphWordForm) && CanBeEqualCyrAndLatSS((wf as Pullenti.Morph.MorphWordForm).NormalCase, str)) 
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Проверка на возможную эквивалентность русского и латинского написания одного и того же слова. 
        /// Например,  ИКЕЯ ? IKEA
        /// </summary>
        /// <param name="t1">токен на одном языке</param>
        /// <param name="t2">токен на другом языке</param>
        /// <return>да-нет</return>
        public static bool CanBeEqualCyrAndLatTT(Pullenti.Ner.Token t1, Pullenti.Ner.Token t2)
        {
            Pullenti.Ner.TextToken tt1 = t1 as Pullenti.Ner.TextToken;
            Pullenti.Ner.TextToken tt2 = t2 as Pullenti.Ner.TextToken;
            if (tt1 == null || tt2 == null) 
                return false;
            if (CanBeEqualCyrAndLatTS(t2, tt1.Term)) 
                return true;
            foreach (Pullenti.Morph.MorphBaseInfo wf in tt1.Morph.Items) 
            {
                if ((wf is Pullenti.Morph.MorphWordForm) && CanBeEqualCyrAndLatTS(t2, (wf as Pullenti.Morph.MorphWordForm).NormalCase)) 
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Проверка на возможную эквивалентность русского и латинского написания одного и того же слова. 
        /// Например,  ИКЕЯ ? IKEA
        /// </summary>
        /// <param name="str1">слово на одном языке</param>
        /// <param name="str2">слово на другом языке</param>
        /// <return>да-нет</return>
        public static bool CanBeEqualCyrAndLatSS(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2)) 
                return false;
            if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(str1[0]) && Pullenti.Morph.LanguageHelper.IsLatinChar(str2[0])) 
                return Pullenti.Ner.Core.Internal.RusLatAccord.CanBeEquals(str1, str2);
            if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(str2[0]) && Pullenti.Morph.LanguageHelper.IsLatinChar(str1[0])) 
                return Pullenti.Ner.Core.Internal.RusLatAccord.CanBeEquals(str2, str1);
            return false;
        }
        /// <summary>
        /// Получить текст, покрываемый метатокеном. Текст корректируется в соответствии с атрибутами.
        /// </summary>
        /// <param name="mt">метатокен</param>
        /// <param name="attrs">атрибуты преобразования текста</param>
        /// <return>результирующая строка</return>
        public static string GetTextValueOfMetaToken(Pullenti.Ner.MetaToken mt, GetTextAttr attrs = GetTextAttr.No)
        {
            if (mt == null) 
                return null;
            if (mt is NounPhraseMultivarToken) 
            {
                NounPhraseMultivarToken nt = mt as NounPhraseMultivarToken;
                StringBuilder res = new StringBuilder();
                if (nt.Source.Preposition != null) 
                    res.AppendFormat("{0} ", _getTextValue_(nt.Source.Preposition.BeginToken, nt.Source.Preposition.EndToken, attrs, null));
                for (int k = nt.AdjIndex1; k <= nt.AdjIndex2; k++) 
                {
                    res.AppendFormat("{0} ", _getTextValue_(nt.Source.Adjectives[k].BeginToken, nt.Source.Adjectives[k].EndToken, attrs, null));
                }
                res.Append(_getTextValue_(nt.Source.Noun.BeginToken, nt.Source.Noun.EndToken, attrs, null));
                return res.ToString();
            }
            return _getTextValue_(mt.BeginToken, mt.EndToken, attrs, mt.GetReferent());
        }
        /// <summary>
        /// Получить текст, задаваемый диапазоном токенов. Текст корректируется в соответствии с атрибутами.
        /// </summary>
        /// <param name="begin">начальный токен</param>
        /// <param name="end">конечный токен</param>
        /// <param name="attrs">атрибуты преобразования текста</param>
        /// <return>результирующая строка</return>
        public static string GetTextValue(Pullenti.Ner.Token begin, Pullenti.Ner.Token end, GetTextAttr attrs = GetTextAttr.No)
        {
            return _getTextValue_(begin, end, attrs, null);
        }
        static string _getTextValue_(Pullenti.Ner.Token begin, Pullenti.Ner.Token end, GetTextAttr attrs, Pullenti.Ner.Referent r)
        {
            if (begin == null || end == null || begin.EndChar > end.EndChar) 
                return null;
            if (((attrs & GetTextAttr.KeepQuotes)) == GetTextAttr.No) 
            {
                for (; begin != null && begin.EndChar <= end.EndChar; begin = begin.Next) 
                {
                    if (BracketHelper.IsBracket(begin, true)) 
                    {
                    }
                    else 
                        break;
                }
            }
            StringBuilder res = new StringBuilder();
            if ((begin is Pullenti.Ner.MetaToken) && !(begin is Pullenti.Ner.NumberToken)) 
            {
                string str = _getTextValue_((begin as Pullenti.Ner.MetaToken).BeginToken, (begin as Pullenti.Ner.MetaToken).EndToken, attrs, begin.GetReferent());
                if (str != null) 
                {
                    if (end == begin) 
                        return str;
                    if ((end is Pullenti.Ner.MetaToken) && !(end is Pullenti.Ner.NumberToken) && begin.Next == end) 
                    {
                        if (((attrs & GetTextAttr.FirstNounGroupToNominative)) == GetTextAttr.FirstNounGroupToNominative || ((attrs & GetTextAttr.FirstNounGroupToNominativeSingle)) == GetTextAttr.FirstNounGroupToNominativeSingle) 
                        {
                            GetTextAttr attrs1 = attrs;
                            if (((attrs1 & GetTextAttr.FirstNounGroupToNominative)) == GetTextAttr.FirstNounGroupToNominative) 
                                attrs1 = attrs1 ^ GetTextAttr.FirstNounGroupToNominative;
                            if (((attrs1 & GetTextAttr.FirstNounGroupToNominativeSingle)) == GetTextAttr.FirstNounGroupToNominativeSingle) 
                                attrs1 = attrs1 ^ GetTextAttr.FirstNounGroupToNominativeSingle;
                            string str0 = _getTextValue_((begin as Pullenti.Ner.MetaToken).BeginToken, (begin as Pullenti.Ner.MetaToken).EndToken, attrs1, begin.GetReferent());
                            string str1 = _getTextValue_((end as Pullenti.Ner.MetaToken).BeginToken, (end as Pullenti.Ner.MetaToken).EndToken, attrs1, begin.GetReferent());
                            Pullenti.Ner.AnalysisResult ar0 = Pullenti.Ner.ProcessorService.EmptyProcessor.Process(new Pullenti.Ner.SourceOfAnalysis(string.Format("{0} {1}", str0, str1)), null, null);
                            NounPhraseToken npt1 = NounPhraseHelper.TryParse(ar0.FirstToken, NounPhraseParseAttr.No, 0, null);
                            if (npt1 != null && npt1.EndToken.Next == null) 
                                return _getTextValue_(npt1.BeginToken, npt1.EndToken, attrs, r);
                        }
                    }
                    res.Append(str);
                    begin = begin.Next;
                    if (((attrs & GetTextAttr.FirstNounGroupToNominative)) == GetTextAttr.FirstNounGroupToNominative) 
                        attrs = attrs ^ GetTextAttr.FirstNounGroupToNominative;
                    if (((attrs & GetTextAttr.FirstNounGroupToNominativeSingle)) == GetTextAttr.FirstNounGroupToNominativeSingle) 
                        attrs = attrs ^ GetTextAttr.FirstNounGroupToNominativeSingle;
                }
            }
            bool keepChars = ((attrs & GetTextAttr.KeepRegister)) != GetTextAttr.No;
            if (keepChars) 
            {
            }
            int restoreCharsEndPos = -1;
            if (((attrs & GetTextAttr.RestoreRegister)) != GetTextAttr.No) 
            {
                if (!hasNotAllUpper(begin, end)) 
                    restoreCharsEndPos = end.EndChar;
                else 
                    for (Pullenti.Ner.Token tt1 = begin; tt1 != null && (tt1.EndChar < end.EndChar); tt1 = tt1.Next) 
                    {
                        if (tt1.IsNewlineAfter && !tt1.IsHiphen) 
                        {
                            if (!hasNotAllUpper(begin, tt1)) 
                                restoreCharsEndPos = tt1.EndChar;
                            break;
                        }
                    }
            }
            if (((attrs & ((GetTextAttr.FirstNounGroupToNominative | GetTextAttr.FirstNounGroupToNominativeSingle)))) != GetTextAttr.No) 
            {
                NounPhraseToken npt = NounPhraseHelper.TryParse(begin, NounPhraseParseAttr.ParsePronouns, 0, null);
                if (npt != null && npt.EndChar <= end.EndChar) 
                {
                    string str = npt.GetNormalCaseText(null, (((attrs & GetTextAttr.FirstNounGroupToNominativeSingle)) != GetTextAttr.No ? Pullenti.Morph.MorphNumber.Singular : Pullenti.Morph.MorphNumber.Undefined), npt.Morph.Gender, keepChars);
                    if (str != null) 
                    {
                        begin = npt.EndToken.Next;
                        res.Append(str);
                        Pullenti.Ner.Token te = npt.EndToken.Next;
                        if (((te != null && te.Next != null && te.IsComma) && (te.Next is Pullenti.Ner.TextToken) && te.Next.EndChar <= end.EndChar) && te.Next.Morph.Class.IsVerb && te.Next.Morph.Class.IsAdjective) 
                        {
                            foreach (Pullenti.Morph.MorphBaseInfo it in te.Next.Morph.Items) 
                            {
                                if (it.Gender == npt.Morph.Gender || ((it.Gender & npt.Morph.Gender)) != Pullenti.Morph.MorphGender.Undefined) 
                                {
                                    if (!((it.Case & npt.Morph.Case)).IsUndefined) 
                                    {
                                        if (it.Number == npt.Morph.Number || ((it.Number & npt.Morph.Number)) != Pullenti.Morph.MorphNumber.Undefined) 
                                        {
                                            string var = (te.Next as Pullenti.Ner.TextToken).Term;
                                            if (it is Pullenti.Morph.MorphWordForm) 
                                                var = (it as Pullenti.Morph.MorphWordForm).NormalCase;
                                            Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective, Gender = npt.Morph.Gender, Number = npt.Morph.Number, Language = npt.Morph.Language };
                                            var = Pullenti.Morph.MorphologyService.GetWordform(var, bi);
                                            if (var != null) 
                                            {
                                                var = corrChars(var, te.Next.Chars, keepChars, te.Next as Pullenti.Ner.TextToken);
                                                res.AppendFormat(", {1}", res, var);
                                                te = te.Next.Next;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        begin = te;
                    }
                }
                if (((attrs & GetTextAttr.FirstNounGroupToNominative)) == GetTextAttr.FirstNounGroupToNominative) 
                    attrs = attrs ^ GetTextAttr.FirstNounGroupToNominative;
                if (((attrs & GetTextAttr.FirstNounGroupToNominativeSingle)) == GetTextAttr.FirstNounGroupToNominativeSingle) 
                    attrs = attrs ^ GetTextAttr.FirstNounGroupToNominativeSingle;
            }
            if (begin == null || begin.EndChar > end.EndChar) 
                return res.ToString();
            for (Pullenti.Ner.Token t = begin; t != null && t.EndChar <= end.EndChar; t = t.Next) 
            {
                char last = (res.Length > 0 ? res[res.Length - 1] : ' ');
                if (t.IsWhitespaceBefore && res.Length > 0) 
                {
                    if (t.IsHiphen && t.IsWhitespaceAfter && last != ' ') 
                    {
                        res.Append(" - ");
                        continue;
                    }
                    if ((last != ' ' && !t.IsHiphen && last != '-') && !BracketHelper.CanBeStartOfSequence(t.Previous, false, false)) 
                        res.Append(' ');
                }
                if (t.IsTableControlChar) 
                {
                    if (res.Length > 0 && res[res.Length - 1] == ' ') 
                    {
                    }
                    else 
                        res.Append(' ');
                    continue;
                }
                if (((attrs & GetTextAttr.IgnoreArticles)) != GetTextAttr.No) 
                {
                    if (IsEngAdjSuffix(t)) 
                    {
                        t = t.Next;
                        continue;
                    }
                    if (IsEngArticle(t)) 
                    {
                        if (t.IsWhitespaceAfter) 
                            continue;
                    }
                }
                if (((attrs & GetTextAttr.KeepQuotes)) == GetTextAttr.No) 
                {
                    if (BracketHelper.IsBracket(t, true)) 
                    {
                        if (res.Length > 0 && res[res.Length - 1] != ' ') 
                            res.Append(' ');
                        continue;
                    }
                }
                if (((attrs & GetTextAttr.IgnoreGeoReferent)) != GetTextAttr.No) 
                {
                    if ((t is Pullenti.Ner.ReferentToken) && t.GetReferent() != null) 
                    {
                        if (t.GetReferent().TypeName == "GEO") 
                            continue;
                    }
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    if (((attrs & GetTextAttr.NormalizeNumbers)) != GetTextAttr.No) 
                    {
                        if (res.Length > 0 && char.IsDigit(res[res.Length - 1])) 
                            res.Append(' ');
                        res.Append((t as Pullenti.Ner.NumberToken).Value);
                        continue;
                    }
                }
                if (t is Pullenti.Ner.MetaToken) 
                {
                    string str = _getTextValue_((t as Pullenti.Ner.MetaToken).BeginToken, (t as Pullenti.Ner.MetaToken).EndToken, attrs, t.GetReferent());
                    if (!string.IsNullOrEmpty(str)) 
                    {
                        if (char.IsDigit(str[0]) && res.Length > 0 && char.IsDigit(res[res.Length - 1])) 
                            res.Append(' ');
                        res.Append(str);
                    }
                    else 
                        res.Append(t.GetSourceText());
                    continue;
                }
                if (!(t is Pullenti.Ner.TextToken)) 
                {
                    res.Append(t.GetSourceText());
                    continue;
                }
                if (t.Chars.IsLetter) 
                {
                    string str = (t.EndChar <= restoreCharsEndPos ? restChars(t as Pullenti.Ner.TextToken, r) : corrChars((t as Pullenti.Ner.TextToken).Term, t.Chars, keepChars, t as Pullenti.Ner.TextToken));
                    res.Append(str);
                    continue;
                }
                if (last == ' ' && res.Length > 0) 
                {
                    if (((t.IsHiphen && !t.IsWhitespaceAfter)) || t.IsCharOf(",.;!?") || BracketHelper.CanBeEndOfSequence(t, false, null, false)) 
                        res.Length--;
                }
                if (t.IsHiphen) 
                {
                    res.Append('-');
                    if (t.IsWhitespaceBefore && t.IsWhitespaceAfter) 
                        res.Append(' ');
                }
                else 
                    res.Append((t as Pullenti.Ner.TextToken).Term);
            }
            for (int i = res.Length - 1; i >= 0; i--) 
            {
                if (res[i] == '*' || char.IsWhiteSpace(res[i])) 
                    res.Length--;
                else if (res[i] == '>' && ((attrs & GetTextAttr.KeepQuotes)) == GetTextAttr.No) 
                {
                    if (res[0] == '<') 
                    {
                        res.Length--;
                        res.Remove(0, 1);
                        i--;
                    }
                    else if (begin.Previous != null && begin.Previous.IsChar('<')) 
                        res.Length--;
                    else 
                        break;
                }
                else if (res[i] == ')' && ((attrs & GetTextAttr.KeepQuotes)) == GetTextAttr.No) 
                {
                    if (res[0] == '(') 
                    {
                        res.Length--;
                        res.Remove(0, 1);
                        i--;
                    }
                    else if (begin.Previous != null && begin.Previous.IsChar('(')) 
                        res.Length--;
                    else 
                        break;
                }
                else 
                    break;
            }
            return res.ToString();
        }
        // Проверка, что это суффикс прилагательного (street's)
        public static bool IsEngAdjSuffix(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            if (!BracketHelper.IsBracket(t, true)) 
                return false;
            if ((t.Next is Pullenti.Ner.TextToken) && (t.Next as Pullenti.Ner.TextToken).Term == "S") 
                return true;
            return false;
        }
        public static bool IsEngArticle(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken) || !t.Chars.IsLatinLetter) 
                return false;
            string str = (t as Pullenti.Ner.TextToken).Term;
            return ((str == "THE" || str == "A" || str == "AN") || str == "DER" || str == "DIE") || str == "DAS";
        }
        static bool hasNotAllUpper(Pullenti.Ner.Token b, Pullenti.Ner.Token e)
        {
            for (Pullenti.Ner.Token t = b; t != null && t.EndChar <= e.EndChar; t = t.Next) 
            {
                if (t is Pullenti.Ner.TextToken) 
                {
                    if (t.Chars.IsLetter && !t.Chars.IsAllUpper) 
                        return true;
                }
                else if (t is Pullenti.Ner.MetaToken) 
                {
                    if (hasNotAllUpper((t as Pullenti.Ner.MetaToken).BeginToken, (t as Pullenti.Ner.MetaToken).EndToken)) 
                        return true;
                }
            }
            return false;
        }
        static string corrChars(string str, Pullenti.Morph.CharsInfo ci, bool keepChars, Pullenti.Ner.TextToken t)
        {
            if (!keepChars) 
                return str;
            if (ci.IsAllLower) 
                return str.ToLower();
            if (ci.IsCapitalUpper) 
                return MiscHelper.ConvertFirstCharUpperAndOtherLower(str);
            if (ci.IsAllUpper || t == null) 
                return str;
            string src = t.GetSourceText();
            if (src.Length == str.Length) 
            {
                StringBuilder tmp = new StringBuilder(str);
                for (int i = 0; i < tmp.Length; i++) 
                {
                    if (char.IsLetter(src[i]) && char.IsLower(src[i])) 
                        tmp[i] = char.ToLower(tmp[i]);
                }
                str = tmp.ToString();
            }
            return str;
        }
        static string restChars(Pullenti.Ner.TextToken t, Pullenti.Ner.Referent r)
        {
            if (!t.Chars.IsAllUpper || !t.Chars.IsLetter) 
                return corrChars(t.Term, t.Chars, true, t);
            if (t.Term == "Г" || t.Term == "ГГ") 
            {
                if (t.Previous is Pullenti.Ner.NumberToken) 
                    return t.Term.ToLower();
            }
            else if (t.Term == "X") 
            {
                if ((t.Previous is Pullenti.Ner.NumberToken) || ((t.Previous != null && t.Previous.IsHiphen))) 
                    return t.Term.ToLower();
            }
            else if (t.Term == "N" || t.Term == "№") 
                return t.Term;
            bool canCapUp = false;
            if (BracketHelper.CanBeStartOfSequence(t.Previous, true, false)) 
                canCapUp = true;
            else if (t.Previous != null && t.Previous.IsChar('.') && t.IsWhitespaceBefore) 
                canCapUp = true;
            StatisticWordInfo stat = t.Kit.Statistics.GetWordInfo(t);
            if (stat == null || ((r != null && ((r.TypeName == "DATE" || r.TypeName == "DATERANGE"))))) 
                return (canCapUp ? MiscHelper.ConvertFirstCharUpperAndOtherLower(t.Term) : t.Term.ToLower());
            if (stat.LowerCount > 0) 
                return (canCapUp ? MiscHelper.ConvertFirstCharUpperAndOtherLower(t.Term) : t.Term.ToLower());
            Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
            if (mc.IsNoun) 
            {
                if (((t.IsValue("СОЗДАНИЕ", null) || t.IsValue("РАЗВИТИЕ", null) || t.IsValue("ВНЕСЕНИЕ", null)) || t.IsValue("ИЗМЕНЕНИЕ", null) || t.IsValue("УТВЕРЖДЕНИЕ", null)) || t.IsValue("ПРИНЯТИЕ", null)) 
                    return (canCapUp ? MiscHelper.ConvertFirstCharUpperAndOtherLower(t.Term) : t.Term.ToLower());
            }
            if (((mc.IsVerb || mc.IsAdverb || mc.IsConjunction) || mc.IsPreposition || mc.IsPronoun) || mc.IsPersonalPronoun) 
                return (canCapUp ? MiscHelper.ConvertFirstCharUpperAndOtherLower(t.Term) : t.Term.ToLower());
            if (stat.CapitalCount > 0) 
                return MiscHelper.ConvertFirstCharUpperAndOtherLower(t.Term);
            if (mc.IsProper) 
                return MiscHelper.ConvertFirstCharUpperAndOtherLower(t.Term);
            if (mc.IsAdjective) 
                return (canCapUp ? MiscHelper.ConvertFirstCharUpperAndOtherLower(t.Term) : t.Term.ToLower());
            if (mc == Pullenti.Morph.MorphClass.Noun) 
                return (canCapUp ? MiscHelper.ConvertFirstCharUpperAndOtherLower(t.Term) : t.Term.ToLower());
            return t.Term;
        }
        /// <summary>
        /// Преобразовать строку в нужный род, число и падеж (точнее, преобразуется 
        /// первая именная группа), регистр определяется соответствующими символами примера. 
        /// Морфология определяется по первой именной группе примера. 
        /// Фукнция полезна при замене по тексту одной комбинации на другую с учётом 
        /// морфологии и регистра.
        /// </summary>
        /// <param name="txt">преобразуемая строка</param>
        /// <param name="beginSample">начало фрагмента примера</param>
        /// <param name="useMopthSample">использовать именную группу примера для морфологии</param>
        /// <param name="useRegisterSample">регистр определять по фрагменту пример, при false регистр исходной строки</param>
        /// <return>результат, в худшем случае вернёт исходную строку</return>
        public static string GetTextMorphVarBySample(string txt, Pullenti.Ner.Token beginSample, bool useMorphSample, bool useRegisterSample)
        {
            if (string.IsNullOrEmpty(txt)) 
                return txt;
            NounPhraseToken npt = NounPhraseHelper.TryParse(beginSample, NounPhraseParseAttr.No, 0, null);
            if (npt != null && beginSample.Previous != null) 
            {
                for (Pullenti.Ner.Token tt = beginSample.Previous; tt != null; tt = tt.Previous) 
                {
                    if (tt.WhitespacesAfterCount > 2) 
                        break;
                    NounPhraseToken npt0 = NounPhraseHelper.TryParse(tt, NounPhraseParseAttr.No, 0, null);
                    if (npt0 != null) 
                    {
                        if (npt0.EndToken == npt.EndToken) 
                            npt.Morph = npt0.Morph;
                        else 
                        {
                            if (tt == beginSample.Previous && npt.BeginToken == npt.EndToken && npt.Morph.Case.IsGenitive) 
                                npt.Morph.RemoveItems(Pullenti.Morph.MorphCase.Genitive);
                            break;
                        }
                    }
                }
            }
            Pullenti.Ner.AnalysisResult ar = Pullenti.Ner.ProcessorService.EmptyProcessor.Process(new Pullenti.Ner.SourceOfAnalysis(txt), null, null);
            if (ar == null || ar.FirstToken == null) 
                return txt;
            NounPhraseToken npt1 = NounPhraseHelper.TryParse(ar.FirstToken, NounPhraseParseAttr.No, 0, null);
            Pullenti.Ner.Token t0 = beginSample;
            StringBuilder res = new StringBuilder();
            for (Pullenti.Ner.Token t = ar.FirstToken; t != null; t = t.Next,t0 = (t0 == null ? null : t0.Next)) 
            {
                if (t.IsWhitespaceBefore && t != ar.FirstToken) 
                    res.Append(' ');
                string word = null;
                if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) 
                {
                    word = (t as Pullenti.Ner.TextToken).Term;
                    if ((npt1 != null && t.EndChar <= npt1.EndChar && npt != null) && useMorphSample) 
                    {
                        Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo();
                        bi.Number = npt.Morph.Number;
                        bi.Case = npt.Morph.Case;
                        bi.Gender = npt1.Morph.Gender;
                        string ww = Pullenti.Morph.MorphologyService.GetWordform(word, bi);
                        if (ww != null) 
                            word = ww;
                    }
                    Pullenti.Morph.CharsInfo ci;
                    if (useRegisterSample && t0 != null) 
                        ci = t0.Chars;
                    else 
                        ci = t.Chars;
                    if (ci.IsAllLower) 
                        word = word.ToLower();
                    else if (ci.IsCapitalUpper) 
                        word = ConvertFirstCharUpperAndOtherLower(word);
                }
                else 
                    word = t.GetSourceText();
                res.Append(word);
            }
            return res.ToString();
        }
        /// <summary>
        /// Преобразовать строку к нужному падежу (и числу). 
        /// Преобразуется только начало строки, содержащее именную группу или персону.
        /// </summary>
        /// <param name="txt">исходная строка</param>
        /// <param name="cas">падеж</param>
        /// <param name="pluralNumber">множественное ли число</param>
        /// <return>результат (в крайнем случае, вернёт исходную строку, если ничего не получилось)</return>
        public static string GetTextMorphVarByCase(string txt, Pullenti.Morph.MorphCase cas, bool pluralNumber = false)
        {
            Pullenti.Ner.AnalysisResult ar = Pullenti.Ner.ProcessorService.EmptyProcessor.Process(new Pullenti.Ner.SourceOfAnalysis(txt), null, null);
            if (ar == null || ar.FirstToken == null) 
                return txt;
            StringBuilder res = new StringBuilder();
            Pullenti.Ner.Token t0 = ar.FirstToken;
            NounPhraseToken npt = NounPhraseHelper.TryParse(ar.FirstToken, NounPhraseParseAttr.ParseVerbs, 0, null);
            if (npt != null) 
            {
                bool accCorr = false;
                if (cas.IsAccusative && pluralNumber) 
                {
                    accCorr = true;
                    if (MiscHelper.IsTokenAnimate(npt.Noun.EndToken)) 
                        cas = Pullenti.Morph.MorphCase.Genitive;
                }
                for (Pullenti.Ner.Token t = npt.BeginToken; t != null && t.EndChar <= npt.EndChar; t = t.Next) 
                {
                    bool isNoun = t.BeginChar >= npt.Noun.BeginChar;
                    bool notCase = false;
                    if (npt.InternalNoun != null) 
                    {
                        if (t.BeginChar >= npt.InternalNoun.BeginChar && t.EndChar <= npt.InternalNoun.EndChar) 
                            notCase = true;
                    }
                    foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
                    {
                        if (a.BeginToken != a.EndToken && a.EndToken.GetMorphClassInDictionary().IsNoun) 
                        {
                            if (t.BeginChar >= a.BeginToken.Next.BeginChar && t.EndChar <= a.EndChar) 
                                notCase = true;
                        }
                        if ((t == a.BeginToken && t.Next != null && t.Next.IsHiphen) && a.EndChar > t.Next.EndChar) 
                            notCase = true;
                    }
                    string word = null;
                    if (t is Pullenti.Ner.NumberToken) 
                        word = t.GetSourceText();
                    else if (t is Pullenti.Ner.TextToken) 
                    {
                        foreach (Pullenti.Morph.MorphBaseInfo it in t.Morph.Items) 
                        {
                            if (notCase) 
                                break;
                            Pullenti.Morph.MorphWordForm wf = it as Pullenti.Morph.MorphWordForm;
                            if (wf == null) 
                                continue;
                            if (t == npt.Anafor) 
                            {
                                if (wf.Misc.Person == Pullenti.Morph.MorphPerson.Third) 
                                {
                                    notCase = true;
                                    break;
                                }
                            }
                            if (!npt.Morph.Case.IsUndefined) 
                            {
                                if (((npt.Morph.Case & wf.Case)).IsUndefined) 
                                    continue;
                            }
                            if (pluralNumber && wf.Number == Pullenti.Morph.MorphNumber.Singular && t != npt.Anafor) 
                                continue;
                            if (isNoun) 
                            {
                                if ((wf.Class.IsNoun || wf.Class.IsPersonalPronoun || wf.Class.IsPronoun) || wf.Class.IsProper) 
                                {
                                    word = wf.NormalCase;
                                    break;
                                }
                            }
                            else if (wf.Class.IsAdjective || wf.Class.IsPronoun || wf.Class.IsPersonalPronoun) 
                            {
                                word = wf.NormalCase;
                                if (accCorr && cas.IsAccusative && word.EndsWith("Х")) 
                                    word = word.Substring(0, word.Length - 1) + "Е";
                                break;
                            }
                        }
                        if (word == null) 
                            word = (notCase ? (t as Pullenti.Ner.TextToken).Term : (t as Pullenti.Ner.TextToken).Lemma);
                        if (!t.Chars.IsLetter) 
                        {
                        }
                        else if (!notCase) 
                        {
                            if ((t.Next != null && t.Next.IsHiphen && t.IsValue("ГЕНЕРАЛ", null)) || t.IsValue("КАПИТАН", null)) 
                            {
                            }
                            else 
                            {
                                Pullenti.Morph.MorphBaseInfo mbi = new Pullenti.Morph.MorphBaseInfo() { Gender = npt.Morph.Gender, Case = cas, Number = Pullenti.Morph.MorphNumber.Singular };
                                if (pluralNumber) 
                                    mbi.Number = Pullenti.Morph.MorphNumber.Plural;
                                string wcas = Pullenti.Morph.MorphologyService.GetWordform(word, mbi);
                                if (wcas != null) 
                                {
                                    word = wcas;
                                    if ((!isNoun && accCorr && cas.IsAccusative) && word.EndsWith("Х") && word != "ИХ") 
                                        word = word.Substring(0, word.Length - 1) + "Е";
                                }
                            }
                        }
                    }
                    if (t.Chars.IsAllLower) 
                        word = word.ToLower();
                    else if (t.Chars.IsCapitalUpper) 
                        word = ConvertFirstCharUpperAndOtherLower(word);
                    if (t != ar.FirstToken && t.IsWhitespaceBefore) 
                        res.Append(' ');
                    res.Append(word);
                    t0 = t.Next;
                }
            }
            if (t0 == ar.FirstToken) 
                return txt;
            if (t0 != null) 
            {
                if (t0.IsWhitespaceBefore) 
                    res.Append(' ');
                res.Append(txt.Substring(t0.BeginChar));
            }
            return res.ToString();
        }
        /// <summary>
        /// Корректировка числа и падежа строки. 
        /// Например, GetTextMorphVarByCaseAndNumberEx("год", MorphCase.Nominative,  MorphNumber.Undefined, "55") = "лет".
        /// </summary>
        /// <param name="str">исходная строка, изменяется только первая именная группа</param>
        /// <param name="cas">нужный падеж</param>
        /// <param name="num">нужное число</param>
        /// <param name="numVal">число, для которого строка является объектом, задающим количество</param>
        /// <return>результат</return>
        public static string GetTextMorphVarByCaseAndNumberEx(string str, Pullenti.Morph.MorphCase cas = null, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Singular, string numVal = null)
        {
            if (str == "коп" || str == "руб") 
                return str;
            if (str == "лет") 
                str = "год";
            Pullenti.Ner.AnalysisResult ar = Pullenti.Ner.ProcessorService.EmptyProcessor.Process(new Pullenti.Ner.SourceOfAnalysis(str) { CreateNumberTokens = false }, null, null);
            if (ar == null || ar.FirstToken == null) 
                return str;
            NounPhraseToken npt = NounPhraseHelper.TryParse(ar.FirstToken, NounPhraseParseAttr.ParseNumericAsAdjective, 0, null);
            if (npt == null && ((str == "раз" || ar.FirstToken.GetMorphClassInDictionary().IsProperName))) 
            {
                npt = new NounPhraseToken(ar.FirstToken, ar.FirstToken);
                npt.Noun = new Pullenti.Ner.MetaToken(ar.FirstToken, ar.FirstToken);
            }
            if (npt == null) 
                return str;
            if (numVal == null && num == Pullenti.Morph.MorphNumber.Undefined) 
                num = npt.Morph.Number;
            if (cas == null || cas.IsUndefined) 
                cas = Pullenti.Morph.MorphCase.Nominative;
            if (!string.IsNullOrEmpty(numVal) && num == Pullenti.Morph.MorphNumber.Undefined) 
            {
                if (cas != null && !cas.IsNominative && !cas.IsGenitive) 
                {
                    if (numVal == "1") 
                        num = Pullenti.Morph.MorphNumber.Singular;
                    else 
                        num = Pullenti.Morph.MorphNumber.Plural;
                }
            }
            Pullenti.Morph.MorphBaseInfo adjBi = new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective | Pullenti.Morph.MorphClass.Noun, Case = cas, Number = num, Gender = npt.Morph.Gender };
            Pullenti.Morph.MorphBaseInfo nounBi = new Pullenti.Morph.MorphBaseInfo() { Class = npt.Noun.Morph.Class, Case = cas, Number = num };
            if (npt.Noun.Morph.Class.IsNoun) 
                nounBi.Class = Pullenti.Morph.MorphClass.Noun;
            string year = null;
            string pair = null;
            if (!string.IsNullOrEmpty(numVal) && num == Pullenti.Morph.MorphNumber.Undefined) 
            {
                char ch = numVal[numVal.Length - 1];
                int n = 0;
                int.TryParse(numVal, out n);
                if (numVal == "1" || ((ch == '1' && n > 20 && ((n % 100)) != 11))) 
                {
                    adjBi.Number = (nounBi.Number = Pullenti.Morph.MorphNumber.Singular);
                    if (str == "год" || str == "раз") 
                        year = str;
                    else if (str == "пар" || str == "пара") 
                        pair = "пара";
                }
                else if (((ch == '2' || ch == '3' || ch == '4')) && (((n < 10) || n > 20))) 
                {
                    nounBi.Number = Pullenti.Morph.MorphNumber.Singular;
                    nounBi.Case = Pullenti.Morph.MorphCase.Genitive;
                    adjBi.Number = Pullenti.Morph.MorphNumber.Plural;
                    adjBi.Case = Pullenti.Morph.MorphCase.Nominative;
                    if (str == "год") 
                        year = (((n < 10) || n > 20) ? "года" : "лет");
                    else if (str == "раз") 
                        year = (((n < 10) || n > 20) ? "раза" : "раз");
                    else if (str == "пар" || str == "пара") 
                        pair = "пары";
                    else if (str == "стул") 
                        pair = "стула";
                }
                else 
                {
                    nounBi.Number = Pullenti.Morph.MorphNumber.Plural;
                    nounBi.Case = Pullenti.Morph.MorphCase.Genitive;
                    adjBi.Number = Pullenti.Morph.MorphNumber.Plural;
                    adjBi.Case = Pullenti.Morph.MorphCase.Genitive;
                    if (str == "год") 
                        year = (ch == '1' && n > 20 ? "год" : "лет");
                    else if (str == "раз") 
                        year = "раз";
                    else if (str == "пар" || str == "пара") 
                        pair = "пар";
                    else if (str == "стул") 
                        year = "стульев";
                }
            }
            StringBuilder res = new StringBuilder();
            string norm;
            string val;
            foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
            {
                norm = a.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                val = Pullenti.Morph.MorphologyService.GetWordform(norm, adjBi);
                if (val == null) 
                    val = a.GetSourceText();
                else if (a.Chars.IsAllLower) 
                    val = val.ToLower();
                else if (a.Chars.IsCapitalUpper) 
                    val = MiscHelper.ConvertFirstCharUpperAndOtherLower(val);
                if (res.Length > 0) 
                    res.Append(' ');
                res.Append(val);
            }
            norm = npt.Noun.GetNormalCaseText(nounBi.Class, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
            if (year != null) 
                val = year;
            else if (pair != null) 
                val = pair;
            else if (str == "мин" || str == "мес") 
                val = str;
            else 
            {
                val = Pullenti.Morph.MorphologyService.GetWordform(norm, nounBi);
                if (val == "РЕБЕНОК" && nounBi.Number == Pullenti.Morph.MorphNumber.Plural) 
                    val = Pullenti.Morph.MorphologyService.GetWordform("ДЕТИ", nounBi);
                if (val == "ЧЕЛОВЕКОВ") 
                    val = "ЧЕЛОВЕК";
                else if (val == "МОРОВ") 
                    val = "МОРЕЙ";
                else if (val == "ПАРОВ") 
                    val = "ПАР";
                if (val == null) 
                    val = npt.Noun.GetSourceText();
                else if (npt.Noun.Chars.IsAllLower) 
                    val = val.ToLower();
                else if (npt.Noun.Chars.IsCapitalUpper) 
                    val = MiscHelper.ConvertFirstCharUpperAndOtherLower(val);
            }
            if (res.Length > 0) 
                res.Append(' ');
            res.Append(val);
            if (npt.EndToken.Next != null) 
            {
                res.Append(" ");
                res.Append(str.Substring(npt.EndToken.Next.BeginChar));
            }
            return res.ToString();
        }
    }
}