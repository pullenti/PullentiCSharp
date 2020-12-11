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
    /// Результат анализа
    /// </summary>
    public class AnalysisResult
    {
        /// <summary>
        /// Анализируемый текст
        /// </summary>
        public SourceOfAnalysis Sofa
        {
            get
            {
                return m_Sofa;
            }
            set
            {
                m_Sofa = value;
            }
        }
        SourceOfAnalysis m_Sofa;
        /// <summary>
        /// Выделенные сущности
        /// </summary>
        public List<Referent> Entities
        {
            get
            {
                return m_Entities;
            }
        }
        List<Referent> m_Entities = new List<Referent>();
        /// <summary>
        /// Ссылка на первый токен текста, который был проанализирован последним
        /// </summary>
        public Token FirstToken;
        /// <summary>
        /// Используемая внешняя онтология
        /// </summary>
        public ExtOntology Ontology;
        /// <summary>
        /// Базовый язык
        /// </summary>
        public Pullenti.Morph.MorphLang BaseLanguage;
        /// <summary>
        /// Это некоторые информационные сообщения
        /// </summary>
        public List<string> Log
        {
            get
            {
                return m_Log;
            }
        }
        List<string> m_Log = new List<string>();
        /// <summary>
        /// Возникшие исключения (одинаковые исключаются)
        /// </summary>
        public List<Exception> Exceptions = new List<Exception>();
        internal void AddException(Exception ex)
        {
            string str = ex.ToString();
            foreach (Exception e in Exceptions) 
            {
                if (e.ToString() == str) 
                    return;
            }
            Exceptions.Add(ex);
        }
        /// <summary>
        /// Процесс был прерван по таймауту (если был задан)
        /// </summary>
        public bool IsTimeoutBreaked = false;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("Общая длина {0} знаков", Sofa.Text.Length);
            if (BaseLanguage != null) 
                res.AppendFormat(", базовый язык {0}", BaseLanguage.ToString());
            res.AppendFormat(", найдено {0} сущностей", Entities.Count);
            if (IsTimeoutBreaked) 
                res.Append(", прервано по таймауту");
            return res.ToString();
        }
    }
}