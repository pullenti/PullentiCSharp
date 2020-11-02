/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pullenti.Ner
{
    /// <summary>
    /// Анализируемый текст, точнее, обёртка над ним
    /// </summary>
    public class SourceOfAnalysis
    {
        /// <summary>
        /// Исходный плоский текст
        /// </summary>
        public string Text
        {
            get;
            set;
        }
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag
        {
            get;
            set;
        }
        /// <summary>
        /// Игнорировать сбойные участки (это участки с неправильной кодировкой, 
        /// мусором и т.п.)
        /// </summary>
        public bool ClearDust = false;
        /// <summary>
        /// Создать контейнер на основе плоского текста. 
        /// При создании будут автоматически сделаны транслитеральные замены, если они будут найдены.
        /// </summary>
        /// <param name="txt">Анализируемый текст</param>
        public SourceOfAnalysis(string txt)
        {
            if (string.IsNullOrEmpty(txt)) 
            {
                Text = "";
                return;
            }
            Text = txt;
        }
        // Это анализ случаев принудительно отформатированного текста
        string DoCrLfCorrection(string txt)
        {
            int i;
            int j;
            int cou = 0;
            int totalLen = 0;
            for (i = 0; i < txt.Length; i++) 
            {
                char ch = txt[i];
                if (ch != 0xD && ch != 0xA) 
                    continue;
                int len = 0;
                char lastChar = ch;
                for (j = i + 1; j < txt.Length; j++) 
                {
                    ch = txt[j];
                    if (ch == 0xD || ch == 0xA) 
                        break;
                    else if (ch == 0x9) 
                        len += 5;
                    else 
                    {
                        lastChar = ch;
                        len++;
                    }
                }
                if (j >= txt.Length) 
                    break;
                if (len < 30) 
                    continue;
                if (lastChar != '.' && lastChar != ':' && lastChar != ';') 
                {
                    bool nextIsDig = false;
                    for (int k = j + 1; k < txt.Length; k++) 
                    {
                        if (!char.IsWhiteSpace(txt[k])) 
                        {
                            if (char.IsDigit(txt[k])) 
                                nextIsDig = true;
                            break;
                        }
                    }
                    if (!nextIsDig) 
                    {
                        cou++;
                        totalLen += len;
                    }
                }
                i = j;
            }
            if (cou < 4) 
                return txt;
            totalLen /= cou;
            if ((totalLen < 50) || totalLen > 100) 
                return txt;
            StringBuilder tmp = new StringBuilder(txt);
            for (i = 0; i < tmp.Length; i++) 
            {
                char ch = tmp[i];
                int jj;
                int len = 0;
                char lastChar = ch;
                for (j = i + 1; j < tmp.Length; j++) 
                {
                    ch = tmp[j];
                    if (ch == 0xD || ch == 0xA) 
                        break;
                    else if (ch == 0x9) 
                        len += 5;
                    else 
                    {
                        lastChar = ch;
                        len++;
                    }
                }
                if (j >= tmp.Length) 
                    break;
                for (jj = j - 1; jj >= 0; jj--) 
                {
                    if (!char.IsWhiteSpace((lastChar = tmp[jj]))) 
                        break;
                }
                bool notSingle = false;
                jj = j + 1;
                if ((jj < tmp.Length) && tmp[j] == 0xD && tmp[jj] == 0xA) 
                    jj++;
                for (; jj < tmp.Length; jj++) 
                {
                    ch = tmp[jj];
                    if (!char.IsWhiteSpace(ch)) 
                        break;
                    if (ch == 0xD || ch == 0xA) 
                    {
                        notSingle = true;
                        break;
                    }
                }
                if (((!notSingle && len > (totalLen - 20) && (len < (totalLen + 10))) && lastChar != '.' && lastChar != ':') && lastChar != ';') 
                {
                    tmp[j] = ' ';
                    CrlfCorrectedCount++;
                    if ((j + 1) < tmp.Length) 
                    {
                        ch = tmp[j + 1];
                        if (ch == 0xA) 
                        {
                            tmp[j + 1] = ' ';
                            j++;
                        }
                    }
                }
                i = j - 1;
            }
            return tmp.ToString();
        }
        /// <summary>
        /// Количество исправлений переходов на новую строку
        /// </summary>
        public int CrlfCorrectedCount = 0;
        /// <summary>
        /// Пытаться ли делать коррекцию слов, не попавших в словарь.
        /// </summary>
        public bool DoWordCorrectionByMorph = false;
        /// <summary>
        /// Объединять соседние слова, не попавшие в словарь, если при объединении получается слово из словаря 
        /// (очень полезно для текстов из PDF)
        /// </summary>
        public bool DoWordsMergingByMorph = true;
        /// <summary>
        /// Создавать автоматически NumberToken
        /// </summary>
        public bool CreateNumberTokens = true;
        /// <summary>
        /// Словарь корректировки типовых ошибок. 
        /// Ключ - ошибочное написание, Значение - правильное. 
        /// Ключи и значения должны быть в верхнем регистре и без Ё.
        /// </summary>
        public Dictionary<string, string> CorrectionDict = null;
        // Произвести транслитеральную коррекцию
        static int DoTransliteralCorrection(StringBuilder txt, StringBuilder info)
        {
            int i;
            int j;
            int k;
            int stat = 0;
            bool prefRusWord = false;
            for (i = 0; i < txt.Length; i++) 
            {
                if (char.IsLetter(txt[i])) 
                {
                    int rus = 0;
                    int pureLat = 0;
                    int unknown = 0;
                    for (j = i; j < txt.Length; j++) 
                    {
                        char ch = txt[j];
                        if (!char.IsLetter(ch)) 
                            break;
                        int code = (int)ch;
                        if (code >= 0x400 && (code < 0x500)) 
                            rus++;
                        else if (m_LatChars.IndexOf(ch) >= 0) 
                            unknown++;
                        else 
                            pureLat++;
                    }
                    if (((unknown > 0 && rus > 0)) || ((unknown > 0 && pureLat == 0 && prefRusWord))) 
                    {
                        if (info != null) 
                        {
                            if (info.Length > 0) 
                                info.Append("\r\n");
                            for (k = i; k < j; k++) 
                            {
                                info.Append(txt[k]);
                            }
                            info.Append(": ");
                        }
                        for (k = i; k < j; k++) 
                        {
                            int ii = m_LatChars.IndexOf(txt[k]);
                            if (ii >= 0) 
                            {
                                if (info != null) 
                                    info.AppendFormat("{0}->{1} ", txt[k], m_RusChars[ii]);
                                txt[k] = m_RusChars[ii];
                            }
                        }
                        stat += unknown;
                        prefRusWord = true;
                    }
                    else 
                        prefRusWord = rus > 0;
                    i = j;
                }
            }
            return stat;
        }
        static string m_LatChars = "ABEKMHOPCTYXaekmopctyx";
        static string m_RusChars = "АВЕКМНОРСТУХаекморстух";
        static int CalcTransliteralStatistics(string txt, StringBuilder info)
        {
            if (txt == null) 
                return 0;
            StringBuilder tmp = new StringBuilder(txt);
            return DoTransliteralCorrection(tmp, info);
        }
        int m_TotalTransliteralSubstitutions;
        int TotalTransliteralSubstitutions
        {
            get
            {
                return m_TotalTransliteralSubstitutions;
            }
        }
        /// <summary>
        /// Извлечь фрагмент из исходного текста. Переходы на новую строку заменяются пробелами.
        /// </summary>
        /// <param name="position">начальная позиция</param>
        /// <param name="length">длина</param>
        /// <return>фрагмент</return>
        public string Substring(int position, int length)
        {
            if (length < 0) 
                length = Text.Length - position;
            if ((position + length) <= Text.Length && length > 0) 
            {
                string res = Text.Substring(position, length);
                if (res.IndexOf("\r\n") >= 0) 
                    res = res.Replace("\r\n", " ");
                if (res.IndexOf('\n') >= 0) 
                    res = res.Replace("\n", " ");
                return res;
            }
            return "Position + Length > Text.Length";
        }
        // Вычислить расстояние в символах между соседними элементами
        public int CalcWhitespaceDistanceBetweenPositions(int posFrom, int posTo)
        {
            if (posFrom == (posTo + 1)) 
                return 0;
            if (posFrom > posTo || (posFrom < 0) || posTo >= Text.Length) 
                return -1;
            int res = 0;
            for (int i = posFrom; i <= posTo; i++) 
            {
                char ch = Text[i];
                if (!char.IsWhiteSpace(ch)) 
                    return -1;
                if (ch == '\r' || ch == '\n') 
                    res += 10;
                else if (ch == '\t') 
                    res += 5;
                else 
                    res++;
            }
            return res;
        }
        public void Serialize(Stream stream)
        {
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, Text);
        }
        public void Deserialize(Stream stream)
        {
            Text = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
        }
    }
}