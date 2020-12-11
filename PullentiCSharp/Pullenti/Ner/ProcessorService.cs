/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner
{
    /// <summary>
    /// Служба лингвистических процессоров
    /// </summary>
    public static class ProcessorService
    {
        /// <summary>
        /// Версия системы
        /// </summary>
        public static string Version
        {
            get
            {
                return "4.1";
            }
        }
        /// <summary>
        /// Дата создания текущей версии
        /// </summary>
        public static string VersionDate
        {
            get
            {
                return "2020.12.8";
            }
        }
        /// <summary>
        /// Инициализация сервиса. Каждый анализатор нужно аинициализировать отдельно. 
        /// Если вызывается Sdk.Initialize(), то там инициализация сервиса и всех анализаторов делается.
        /// </summary>
        /// <param name="lang">необходимые языки (по умолчанию, русский и английский)</param>
        public static void Initialize(Pullenti.Morph.MorphLang lang = null)
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Morph.MorphologyService.Initialize(lang);
            Pullenti.Semantic.Utils.DerivateService.Initialize(lang);
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
            Pullenti.Ner.Core.PrepositionHelper.Initialize();
            Pullenti.Ner.Core.ConjunctionHelper.Initialize();
            Pullenti.Ner.Core.Internal.NounPhraseItem.Initialize();
            Pullenti.Ner.Core.NumberHelper.Initialize();
            Pullenti.Ner.Core.Internal.NumberExHelper.Initialize();
            Pullenti.Ner.Core.Internal.BlockLine.Initialize();
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
        }
        static bool m_Inited;
        /// <summary>
        /// Признак того, что инициализация сервиса уже была
        /// </summary>
        public static bool IsInitialized
        {
            get
            {
                return m_Inited;
            }
        }
        /// <summary>
        /// Создать процессор со стандартным списком анализаторов (у которых свойство IsSpecific = false)
        /// </summary>
        /// <return>экземпляр процессора</return>
        public static Processor CreateProcessor()
        {
            if (!m_Inited) 
                return null;
            Processor proc = new Processor();
            foreach (Analyzer t in m_AnalizerInstances) 
            {
                Analyzer a = t.Clone();
                if (a != null && !a.IsSpecific) 
                    proc.AddAnalyzer(a);
            }
            return proc;
        }
        /// <summary>
        /// Создать процессор с набором стандартных и указанных параметром специфических 
        /// анализаторов.
        /// </summary>
        /// <param name="specAnalyzerNames">можно несколько, разделённые запятой или точкой с запятой. 
        /// Если список пустой, то эквивалентно CreateProcessor()</param>
        /// <return>Экземпляр процессора</return>
        public static Processor CreateSpecificProcessor(string specAnalyzerNames)
        {
            if (!m_Inited) 
                return null;
            Processor proc = new Processor();
            List<string> names = new List<string>(((specAnalyzerNames ?? "")).Split(',', ';', ' '));
            foreach (Analyzer t in m_AnalizerInstances) 
            {
                Analyzer a = t.Clone();
                if (a != null) 
                {
                    if (!a.IsSpecific || names.Contains(a.Name)) 
                        proc.AddAnalyzer(a);
                }
            }
            return proc;
        }
        /// <summary>
        /// Создать экземпляр процессора с пустым списком анализаторов
        /// </summary>
        /// <return>Процессор без выделения сущностей</return>
        public static Processor CreateEmptyProcessor()
        {
            return new Processor();
        }
        // Регистрация анализатора. Вызывается при инициализации из инициализируемой сборки
        // (она сама знает, какие содержит анализаторы, и регистрирует их)
        public static void RegisterAnalyzer(Analyzer analyzer)
        {
            try 
            {
                m_AnalizerInstances.Add(analyzer);
                Dictionary<string, byte[]> img = analyzer.Images;
                if (img != null) 
                {
                    foreach (KeyValuePair<string, byte[]> kp in img) 
                    {
                        if (!m_Images.ContainsKey(kp.Key)) 
                            m_Images.Add(kp.Key, new Pullenti.Ner.Metadata.ImageWrapper() { Id = kp.Key, Content = kp.Value });
                    }
                }
            }
            catch(Exception ex) 
            {
            }
            _reorderCartridges();
        }
        static List<Analyzer> m_AnalizerInstances = new List<Analyzer>();
        static void _reorderCartridges()
        {
            if (m_AnalizerInstances.Count == 0) 
                return;
            for (int k = 0; k < m_AnalizerInstances.Count; k++) 
            {
                for (int i = 0; i < (m_AnalizerInstances.Count - 1); i++) 
                {
                    int maxInd = -1;
                    IEnumerable<string> li = m_AnalizerInstances[i].UsedExternObjectTypes;
                    if (li != null) 
                    {
                        foreach (string v in m_AnalizerInstances[i].UsedExternObjectTypes) 
                        {
                            for (int j = i + 1; j < m_AnalizerInstances.Count; j++) 
                            {
                                if (m_AnalizerInstances[j].TypeSystem != null) 
                                {
                                    foreach (Pullenti.Ner.Metadata.ReferentClass st in m_AnalizerInstances[j].TypeSystem) 
                                    {
                                        if (st.Name == v) 
                                        {
                                            if ((maxInd < 0) || (maxInd < j)) 
                                                maxInd = j;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (maxInd <= i) 
                    {
                        if (m_AnalizerInstances[i].IsSpecific && !m_AnalizerInstances[i + 1].IsSpecific) 
                        {
                        }
                        else 
                            continue;
                    }
                    Analyzer cart = m_AnalizerInstances[i];
                    m_AnalizerInstances.RemoveAt(i);
                    m_AnalizerInstances.Add(cart);
                }
            }
        }
        /// <summary>
        /// Экземпляры доступных анализаторов
        /// </summary>
        public static ICollection<Analyzer> Analyzers
        {
            get
            {
                return m_AnalizerInstances;
            }
        }
        /// <summary>
        /// Создать экземпляр объекта заданного типа
        /// </summary>
        /// <param name="typeName">имя типа</param>
        /// <return>результат</return>
        public static Referent CreateReferent(string typeName)
        {
            foreach (Analyzer cart in m_AnalizerInstances) 
            {
                Referent obj = cart.CreateReferent(typeName);
                if (obj != null) 
                    return obj;
            }
            return new Referent(typeName);
        }
        static Dictionary<string, Pullenti.Ner.Metadata.ImageWrapper> m_Images = new Dictionary<string, Pullenti.Ner.Metadata.ImageWrapper>();
        static Pullenti.Ner.Metadata.ImageWrapper m_UnknownImage;
        /// <summary>
        /// Получить иконку по идентификатору иконки
        /// </summary>
        /// <param name="imageId">идентификатор иконки</param>
        /// <return>обёртка над телом иконки</return>
        public static Pullenti.Ner.Metadata.ImageWrapper GetImageById(string imageId)
        {
            if (imageId != null) 
            {
                Pullenti.Ner.Metadata.ImageWrapper res;
                if (m_Images.TryGetValue(imageId, out res)) 
                    return res;
            }
            if (m_UnknownImage == null) 
                m_UnknownImage = new Pullenti.Ner.Metadata.ImageWrapper() { Id = "unknown", Content = Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("unknown.png") };
            return m_UnknownImage;
        }
        /// <summary>
        /// Добавить специфическую иконку
        /// </summary>
        /// <param name="imageId">идентификатор (возвращаемый Referent.GetImageId())</param>
        /// <param name="content">содержимое иконки</param>
        public static void AddImage(string imageId, byte[] content)
        {
            if (imageId == null) 
                return;
            Pullenti.Ner.Metadata.ImageWrapper wr = new Pullenti.Ner.Metadata.ImageWrapper() { Id = imageId, Content = content };
            if (m_Images.ContainsKey(imageId)) 
                m_Images[imageId] = wr;
            else 
                m_Images.Add(imageId, wr);
        }
        static Processor m_EmptyProcessor;
        /// <summary>
        /// Экземпляр процессора с пустым множеством анализаторов (используется для 
        /// разных лингвистических процедур, где не нужны сущности)
        /// </summary>
        public static Processor EmptyProcessor
        {
            get
            {
                if (m_EmptyProcessor == null) 
                    m_EmptyProcessor = CreateEmptyProcessor();
                return m_EmptyProcessor;
            }
        }
        /// <summary>
        /// Это нужно для автотестов, чтобы фиксировать дату-время, относительно которой 
        /// идут вычисления (если не задана, то берётся текущая)
        /// </summary>
        public static DateTime? DebugCurrentDateTime;
    }
}