/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Semantic.Utils
{
    /// <summary>
    /// Сервис для получение информации о словах. Однокоренные слова объединены в так называемые дериватные группы. 
    /// В настоящий момент поддержаны русский и украинский языки.
    /// </summary>
    public static class DerivateService
    {
        /// <summary>
        /// Инициализация внутренних словарей. 
        /// Можно не вызывать, но тогда будет автоматически вызвано при первом обращении, 
        /// и соответственно первое обращение отработает на несколько секунд дольше. 
        /// Если инициализация идёт через Sdk.Initialize или ProcessorService.Initialize, то эту функцию вызывать не надо.
        /// </summary>
        /// <param name="langs">по умолчанию, русский с украинским</param>
        public static void Initialize(Pullenti.Morph.MorphLang langs = null)
        {
            if (langs == null || langs.IsUndefined) 
                langs = Pullenti.Morph.MorphLang.RU;
            Pullenti.Semantic.Internal.NextModelHelper.Initialize();
            ControlModelQuestion.Initialize();
            LoadLanguages(langs);
        }
        static Pullenti.Semantic.Internal.DerivateDictionary m_DerRu = new Pullenti.Semantic.Internal.DerivateDictionary();
        public static Pullenti.Morph.MorphLang LoadedLanguages
        {
            get
            {
                if (m_DerRu.m_AllGroups.Count > 0) 
                    return Pullenti.Morph.MorphLang.RU | Pullenti.Morph.MorphLang.UA;
                return Pullenti.Morph.MorphLang.Unknown;
            }
        }
        public static void LoadLanguages(Pullenti.Morph.MorphLang langs)
        {
            if (langs.IsRu || langs.IsUa) 
            {
                if (!m_DerRu.Init(Pullenti.Morph.MorphLang.RU, true)) 
                    throw new Exception("Not found resource file e_ru.dat in Enplanatory");
            }
            if (langs.IsUa) 
            {
            }
        }
        public static void LoadDictionaryRu(byte[] dat)
        {
            m_DerRu.Load(dat);
        }
        public static void UnloadLanguages(Pullenti.Morph.MorphLang langs)
        {
            if (langs.IsRu || langs.IsUa) 
            {
                if (langs.IsRu && langs.IsUa) 
                    m_DerRu.Unload();
            }
            GC.Collect();
        }
        /// <summary>
        /// Найти для слова дериватные группы DerivateGroup, в которые входит это слово 
        /// (групп может быть несколько, но в большинстве случаев - одна)
        /// </summary>
        /// <param name="word">слово в верхнем регистре и нормальной форме</param>
        /// <param name="tryVariants">пытаться ли для неизвестных слов делать варианты</param>
        /// <param name="lang">язык (по умолчанию, русский)</param>
        /// <return>список дериватных групп DerivateGroup</return>
        public static List<DerivateGroup> FindDerivates(string word, bool tryVariants = true, Pullenti.Morph.MorphLang lang = null)
        {
            return m_DerRu.Find(word, tryVariants, lang);
        }
        /// <summary>
        /// Найти для слова его толковую информацию (среди дериватных групп)
        /// </summary>
        /// <param name="word">слово в верхнем регистре и нормальной форме</param>
        /// <param name="lang">возможный язык</param>
        /// <return>список слов DerivateWord</return>
        public static List<DerivateWord> FindWords(string word, Pullenti.Morph.MorphLang lang = null)
        {
            List<DerivateGroup> grs = m_DerRu.Find(word, false, lang);
            if (grs == null) 
                return null;
            List<DerivateWord> res = null;
            foreach (DerivateGroup g in grs) 
            {
                foreach (DerivateWord w in g.Words) 
                {
                    if (w.Spelling == word) 
                    {
                        if (res == null) 
                            res = new List<DerivateWord>();
                        res.Add(w);
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// Получить слова однокоренное слово заданной части речи. 
        /// Например, для существительного "ГЛАГОЛ" вариант прилагательного: "ГЛАГОЛЬНЫЙ"
        /// </summary>
        /// <param name="word">слово в верхнем регистре и нормальной форме</param>
        /// <param name="cla">нужная часть речи</param>
        /// <param name="lang">возможный язык</param>
        /// <return>вариант или null при ненахождении</return>
        public static string GetWordClassVar(string word, Pullenti.Morph.MorphClass cla, Pullenti.Morph.MorphLang lang = null)
        {
            List<DerivateGroup> grs = m_DerRu.Find(word, false, lang);
            if (grs == null) 
                return null;
            foreach (DerivateGroup g in grs) 
            {
                foreach (DerivateWord w in g.Words) 
                {
                    if (w.Class == cla) 
                        return w.Spelling;
                }
            }
            return null;
        }
        /// <summary>
        /// Может ли быть одушевлённым
        /// </summary>
        /// <param name="word">слово в верхнем регистре и нормальной форме</param>
        /// <param name="lang">язык (по умолчанию, русский)</param>
        /// <return>да-нет</return>
        public static bool IsAnimated(string word, Pullenti.Morph.MorphLang lang = null)
        {
            List<DerivateGroup> grs = m_DerRu.Find(word, false, lang);
            if (grs == null) 
                return false;
            foreach (DerivateGroup g in grs) 
            {
                foreach (DerivateWord w in g.Words) 
                {
                    if (w.Spelling == word) 
                    {
                        if (w.Attrs.IsAnimated) 
                            return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Может ли иметь собственное имя
        /// </summary>
        /// <param name="word">слово в верхнем регистре и нормальной форме</param>
        /// <param name="lang">язык (по умолчанию, русский)</param>
        /// <return>да-нет</return>
        public static bool IsNamed(string word, Pullenti.Morph.MorphLang lang = null)
        {
            List<DerivateGroup> grs = m_DerRu.Find(word, false, lang);
            if (grs == null) 
                return false;
            foreach (DerivateGroup g in grs) 
            {
                foreach (DerivateWord w in g.Words) 
                {
                    if (w.Spelling == word) 
                    {
                        if (w.Attrs.IsNamed) 
                            return true;
                    }
                }
            }
            return false;
        }
        internal static object m_Lock = new object();
        public static void SetDictionary(Pullenti.Semantic.Internal.DerivateDictionary dic)
        {
            m_DerRu = dic;
        }
    }
}