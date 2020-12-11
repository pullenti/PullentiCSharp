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

namespace Pullenti.Morph
{
    /// <summary>
    /// Элементы, на которые разбивается исходный текст (токены)
    /// </summary>
    public class MorphToken
    {
        /// <summary>
        /// Начальная позиция
        /// </summary>
        public int BeginChar;
        /// <summary>
        /// Конечная позиция
        /// </summary>
        public int EndChar;
        /// <summary>
        /// Нормализованный фрагмент исходного текста (верхний регистр, замена некотрых букв)
        /// </summary>
        public string Term;
        /// <summary>
        /// Число символов (нормализованного фрагмента = Term.Length)
        /// </summary>
        public int Length
        {
            get
            {
                return (Term == null ? 0 : Term.Length);
            }
        }
        /// <summary>
        /// Извлечь фрагмент из исходного текста, соответствующий токену
        /// </summary>
        /// <param name="text">полный исходный текст</param>
        /// <return>фрагмент</return>
        public string GetSourceText(string text)
        {
            return text.Substring(BeginChar, (EndChar + 1) - BeginChar);
        }
        /// <summary>
        /// Лемма (вариант морфологической нормализации)
        /// </summary>
        public string GetLemma()
        {
            if (m_Lemma != null) 
                return m_Lemma;
            string res = null;
            if (WordForms != null && WordForms.Count > 0) 
            {
                if (WordForms.Count == 1) 
                    res = WordForms[0].NormalFull ?? WordForms[0].NormalCase;
                if (res == null && !CharInfo.IsAllLower) 
                {
                    foreach (MorphWordForm m in WordForms) 
                    {
                        if (m.Class.IsProperSurname) 
                        {
                            string s = m.NormalFull ?? m.NormalCase ?? "";
                            if (LanguageHelper.EndsWithEx(s, "ОВ", "ЕВ", null, null)) 
                            {
                                res = s;
                                break;
                            }
                        }
                        else if (m.Class.IsProperName && m.IsInDictionary) 
                            return m.NormalCase;
                    }
                }
                if (res == null) 
                {
                    MorphWordForm best = null;
                    foreach (MorphWordForm m in WordForms) 
                    {
                        if (best == null) 
                            best = m;
                        else if (this.CompareForms(best, m) > 0) 
                            best = m;
                    }
                    res = best.NormalFull ?? best.NormalCase;
                }
            }
            if (res != null) 
            {
                if (LanguageHelper.EndsWithEx(res, "АНЫЙ", "ЕНЫЙ", null, null)) 
                    res = res.Substring(0, res.Length - 3) + "ННЫЙ";
                else if (LanguageHelper.EndsWith(res, "ЙСЯ")) 
                    res = res.Substring(0, res.Length - 2);
                else if (LanguageHelper.EndsWith(res, "АНИЙ") && res == Term) 
                {
                    foreach (MorphWordForm wf in WordForms) 
                    {
                        if (wf.IsInDictionary) 
                            return res;
                    }
                    return res.Substring(0, res.Length - 1) + "Е";
                }
                return res;
            }
            return Term ?? "?";
        }
        string m_Lemma;
        int CompareForms(MorphWordForm x, MorphWordForm y)
        {
            string vx = x.NormalFull ?? x.NormalCase;
            string vy = y.NormalFull ?? y.NormalCase;
            if (vx == vy) 
                return 0;
            if (string.IsNullOrEmpty(vx)) 
                return 1;
            if (string.IsNullOrEmpty(vy)) 
                return -1;
            char lastx = vx[vx.Length - 1];
            char lasty = vy[vy.Length - 1];
            if (x.Class.IsProperSurname && !CharInfo.IsAllLower) 
            {
                if (LanguageHelper.EndsWithEx(vx, "ОВ", "ЕВ", "ИН", null)) 
                {
                    if (!y.Class.IsProperSurname) 
                        return -1;
                }
            }
            if (y.Class.IsProperSurname && !CharInfo.IsAllLower) 
            {
                if (LanguageHelper.EndsWithEx(vy, "ОВ", "ЕВ", "ИН", null)) 
                {
                    if (!x.Class.IsProperSurname) 
                        return 1;
                    if (vx.Length > vy.Length) 
                        return -1;
                    if (vx.Length < vy.Length) 
                        return 1;
                    return 0;
                }
            }
            if (x.Class == y.Class) 
            {
                if (x.Class.IsAdjective) 
                {
                    if (lastx == 'Й' && lasty != 'Й') 
                        return -1;
                    if (lastx != 'Й' && lasty == 'Й') 
                        return 1;
                    if (!LanguageHelper.EndsWith(vx, "ОЙ") && LanguageHelper.EndsWith(vy, "ОЙ")) 
                        return -1;
                    if (LanguageHelper.EndsWith(vx, "ОЙ") && !LanguageHelper.EndsWith(vy, "ОЙ")) 
                        return 1;
                }
                if (x.Class.IsNoun) 
                {
                    if (x.Number == MorphNumber.Singular && y.Number == MorphNumber.Plural && vx.Length <= (vy.Length + 1)) 
                        return -1;
                    if (x.Number == MorphNumber.Plural && y.Number == MorphNumber.Singular && vx.Length >= (vy.Length - 1)) 
                        return 1;
                }
                if (vx.Length < vy.Length) 
                    return -1;
                if (vx.Length > vy.Length) 
                    return 1;
                return 0;
            }
            if (x.Class.IsAdverb) 
                return 1;
            if (x.Class.IsNoun && x.IsInDictionary) 
            {
                if (y.Class.IsAdjective && y.IsInDictionary) 
                {
                    if (!y.Misc.Attrs.Contains("к.ф.")) 
                        return 1;
                }
                return -1;
            }
            if (x.Class.IsAdjective) 
            {
                if (!x.IsInDictionary && y.Class.IsNoun && y.IsInDictionary) 
                    return 1;
                return -1;
            }
            if (x.Class.IsVerb) 
            {
                if (y.Class.IsNoun || y.Class.IsAdjective || y.Class.IsPreposition) 
                    return 1;
                return -1;
            }
            if (y.Class.IsAdverb) 
                return -1;
            if (y.Class.IsNoun && y.IsInDictionary) 
                return 1;
            if (y.Class.IsAdjective) 
            {
                if (((x.Class.IsNoun || x.Class.IsProperSecname)) && x.IsInDictionary) 
                    return -1;
                if (x.Class.IsNoun && !y.IsInDictionary) 
                {
                    if (vx.Length < vy.Length) 
                        return -1;
                }
                return 1;
            }
            if (y.Class.IsVerb) 
            {
                if (x.Class.IsNoun || x.Class.IsAdjective || x.Class.IsPreposition) 
                    return -1;
                if (x.Class.IsProper) 
                    return -1;
                return 1;
            }
            if (vx.Length < vy.Length) 
                return -1;
            if (vx.Length > vy.Length) 
                return 1;
            return 0;
        }
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag;
        /// <summary>
        /// Язык(и)
        /// </summary>
        public MorphLang Language
        {
            get
            {
                if (m_Language != null && m_Language != MorphLang.Unknown) 
                    return m_Language;
                MorphLang lang = new MorphLang();
                if (WordForms != null) 
                {
                    foreach (MorphWordForm wf in WordForms) 
                    {
                        if (wf.Language != MorphLang.Unknown) 
                            lang |= wf.Language;
                    }
                }
                return lang;
            }
            set
            {
                m_Language = value;
            }
        }
        MorphLang m_Language;
        /// <summary>
        /// Варианты словоформ
        /// </summary>
        public List<MorphWordForm> WordForms;
        /// <summary>
        /// Информация о токене
        /// </summary>
        public CharsInfo CharInfo;
        public MorphToken()
        {
        }
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Term)) 
                return "Null";
            string str = Term;
            if (CharInfo.IsAllLower) 
                str = str.ToLower();
            else if (CharInfo.IsCapitalUpper && str.Length > 0) 
                str = string.Format("{0}{1}", Term[0], Term.Substring(1).ToLower());
            else if (CharInfo.IsLastLower) 
                str = string.Format("{0}{1}", Term.Substring(0, Term.Length - 1), Term.Substring(Term.Length - 1).ToLower());
            if (WordForms == null) 
                return str;
            StringBuilder res = new StringBuilder(str);
            foreach (MorphWordForm l in WordForms) 
            {
                res.AppendFormat(", {0}", l.ToString());
            }
            return res.ToString();
        }
    }
}