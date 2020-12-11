/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Pullenti.Morph
{
    /// <summary>
    /// Сервис морфологического анализа текстов (POS-tagger).
    /// </summary>
    public static class MorphologyService
    {
        /// <summary>
        /// Инициализация внутренних словарей. 
        /// Можно не вызывать, но тогда будет автоматически вызвано при первом обращении к морфологии, 
        /// и соответственно первый разбор отработает на несколько секунд дольше. 
        /// Если используете Sdk.Initialize() или ProcessorService.Initialize(), то тогда эту функцию вызывать не нужно, 
        /// так как там внутри это делается.
        /// </summary>
        /// <param name="langs">по умолчанию, русский и английский</param>
        public static void Initialize(MorphLang langs = null)
        {
            Pullenti.Morph.Internal.UnicodeInfo.Initialize();
            if (langs == null || langs.IsUndefined) 
                langs = MorphLang.RU | MorphLang.EN;
            m_Morph.LoadLanguages(langs, LazyLoad);
        }
        /// <summary>
        /// Языки, морфологические словари для которых загружены в память
        /// </summary>
        public static MorphLang LoadedLanguages
        {
            get
            {
                return m_Morph.LoadedLanguages;
            }
        }
        /// <summary>
        /// Загрузить язык(и), если они ещё не загружены
        /// </summary>
        /// <param name="langs">загружаемые языки</param>
        public static void LoadLanguages(MorphLang langs)
        {
            m_Morph.LoadLanguages(langs, LazyLoad);
        }
        /// <summary>
        /// Выгрузить язык(и), если они больше не нужны
        /// </summary>
        /// <param name="langs">выгружаемые языки</param>
        public static void UnloadLanguages(MorphLang langs)
        {
            m_Morph.UnloadLanguages(langs);
        }
        static List<MorphWordForm> m_EmptyWordForms = new List<MorphWordForm>();
        static MorphMiscInfo m_EmptyMisc = new MorphMiscInfo();
        /// <summary>
        /// Произвести чистую токенизацию без формирования морф-вариантов
        /// </summary>
        /// <param name="text">исходный текст</param>
        /// <return>последовательность результирующих лексем</return>
        public static List<MorphToken> Tokenize(string text)
        {
            if (string.IsNullOrEmpty(text)) 
                return null;
            List<MorphToken> res = m_Morph.Run(text, true, MorphLang.Unknown, false, null);
            if (res != null) 
            {
                foreach (MorphToken r in res) 
                {
                    if (r.WordForms == null) 
                        r.WordForms = m_EmptyWordForms;
                    foreach (MorphWordForm wf in r.WordForms) 
                    {
                        if (wf.Misc == null) 
                            wf.Misc = m_EmptyMisc;
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// Произвести морфологический анализ текста. Если используете морфологию в составе лингвистического процессора из 
        /// ProcessorService, то эту функцию явно вызывать не придётся.
        /// </summary>
        /// <param name="text">исходный текст</param>
        /// <param name="lang">базовый язык (если null, то будет определён автоматически)</param>
        /// <param name="progress">это для бегунка</param>
        /// <return>последовательность результирующих лексем MorphToken</return>
        public static List<MorphToken> Process(string text, MorphLang lang = null, ProgressChangedEventHandler progress = null)
        {
            if (string.IsNullOrEmpty(text)) 
                return null;
            List<MorphToken> res = m_Morph.Run(text, false, lang, false, progress);
            if (res != null) 
            {
                foreach (MorphToken r in res) 
                {
                    if (r.WordForms == null) 
                        r.WordForms = m_EmptyWordForms;
                    foreach (MorphWordForm wf in r.WordForms) 
                    {
                        if (wf.Misc == null) 
                            wf.Misc = m_EmptyMisc;
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// Получить все варианты словоформ для нормальной формы слова
        /// </summary>
        /// <param name="lang">язык (по умолчанию, русский)</param>
        /// <return>список словоформ MorphWordForm</return>
        public static List<MorphWordForm> GetAllWordforms(string word, MorphLang lang = null)
        {
            List<MorphWordForm> res = m_Morph.GetAllWordforms(word, lang);
            if (res != null) 
            {
                foreach (MorphWordForm r in res) 
                {
                    if (r.Misc == null) 
                        r.Misc = m_EmptyMisc;
                }
            }
            return res;
        }
        /// <summary>
        /// Получить вариант написания словоформы
        /// </summary>
        /// <param name="word">слово</param>
        /// <param name="morphInfo">морфологическая информация</param>
        /// <return>вариант написания</return>
        public static string GetWordform(string word, MorphBaseInfo morphInfo)
        {
            if (morphInfo == null || string.IsNullOrEmpty(word)) 
                return word;
            MorphClass cla = morphInfo.Class;
            if (cla.IsUndefined) 
            {
                MorphWordForm mi0 = GetWordBaseInfo(word, null, false, false);
                if (mi0 != null) 
                    cla = mi0.Class;
            }
            string word1 = word;
            foreach (char ch in word) 
            {
                if (char.IsLower(ch)) 
                {
                    word1 = word.ToUpper();
                    break;
                }
            }
            MorphWordForm wf = morphInfo as MorphWordForm;
            string res = m_Morph.GetWordform(word1, cla, morphInfo.Gender, morphInfo.Case, morphInfo.Number, morphInfo.Language, wf);
            if (string.IsNullOrEmpty(res)) 
                return word;
            return res;
        }
        /// <summary>
        /// Получить для словоформы род\число\падеж
        /// </summary>
        /// <param name="word">словоформа</param>
        /// <param name="lang">возможный язык</param>
        /// <param name="isCaseNominative">исходное слово в именительном падеже (иначе считается падеж любым)</param>
        /// <param name="inDictOnly">при true не строить гипотезы для несловарных слов</param>
        /// <return>базовая морфологическая информация</return>
        public static MorphWordForm GetWordBaseInfo(string word, MorphLang lang = null, bool isCaseNominative = false, bool inDictOnly = false)
        {
            List<MorphToken> mt = m_Morph.Run(word, false, lang, false, null);
            MorphWordForm bi = new MorphWordForm();
            MorphClass cla = new MorphClass();
            if (mt != null && mt.Count > 0) 
            {
                for (int k = 0; k < 2; k++) 
                {
                    bool ok = false;
                    foreach (MorphWordForm wf in mt[0].WordForms) 
                    {
                        if (k == 0) 
                        {
                            if (!wf.IsInDictionary) 
                                continue;
                        }
                        else if (wf.IsInDictionary) 
                            continue;
                        if (isCaseNominative) 
                        {
                            if (!wf.Case.IsNominative && !wf.Case.IsUndefined) 
                                continue;
                        }
                        cla.Value |= wf.Class.Value;
                        bi.Gender |= wf.Gender;
                        bi.Case |= wf.Case;
                        bi.Number |= wf.Number;
                        if (wf.Misc != null && bi.Misc == null) 
                            bi.Misc = wf.Misc;
                        ok = true;
                    }
                    if (ok || inDictOnly) 
                        break;
                }
            }
            bi.Class = cla;
            return bi;
        }
        /// <summary>
        /// Попробовать откорретировать одну букву словоформы, чтобы получилось словарное слово
        /// </summary>
        /// <param name="word">искаженное слово</param>
        /// <param name="lang">возможный язык</param>
        /// <return>откорректированное слово или null при невозможности</return>
        public static string CorrectWord(string word, MorphLang lang = null)
        {
            return m_Morph.CorrectWordByMorph(word, lang);
        }
        /// <summary>
        /// Преобразовать наречие в прилагательное (это пока только для русского языка)
        /// </summary>
        /// <param name="adverb">наречие</param>
        /// <param name="bi">род число падеж</param>
        /// <return>прилагательное</return>
        public static string ConvertAdverbToAdjective(string adverb, MorphBaseInfo bi)
        {
            if (adverb == null || (adverb.Length < 4)) 
                return null;
            char last = adverb[adverb.Length - 1];
            if (last != 'О' && last != 'Е') 
                return adverb;
            string var1 = adverb.Substring(0, adverb.Length - 1) + "ИЙ";
            string var2 = adverb.Substring(0, adverb.Length - 1) + "ЫЙ";
            MorphWordForm bi1 = GetWordBaseInfo(var1, null, false, false);
            MorphWordForm bi2 = GetWordBaseInfo(var2, null, false, false);
            string var = var1;
            if (!bi1.Class.IsAdjective && bi2.Class.IsAdjective) 
                var = var2;
            if (bi == null) 
                return var;
            return m_Morph.GetWordform(var, MorphClass.Adjective, bi.Gender, bi.Case, bi.Number, MorphLang.Unknown, null) ?? var;
        }
        // При и нициализации не грузить всю морфологиюю сразу в память, а подгружать по мере необходимости.
        // Сильно экономит время (при инициализации) и память.
        public static bool LazyLoad = true;
        static Pullenti.Morph.Internal.InnerMorphology m_Morph = new Pullenti.Morph.Internal.InnerMorphology();
    }
}