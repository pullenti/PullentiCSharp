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
    // Элемент внутреннего онтологического словаря
    public class IntOntologyItem
    {
        public IntOntologyItem(Pullenti.Ner.Referent r)
        {
            Referent = r;
        }
        /// <summary>
        /// Список терминов, ассоциированных со словарной записью
        /// </summary>
        public List<Termin> Termins = new List<Termin>();
        /// <summary>
        /// Каноноический текст
        /// </summary>
        public string CanonicText
        {
            get
            {
                if (m_CanonicText == null && Termins.Count > 0) 
                    m_CanonicText = Termins[0].CanonicText;
                return m_CanonicText ?? "?";
            }
            set
            {
                m_CanonicText = value;
            }
        }
        string m_CanonicText;
        /// <summary>
        /// В качестве канонического текста установить самый короткий среди терминов
        /// </summary>
        public void SetShortestCanonicalText(bool ignoreTerminsWithNotnullTags = false)
        {
            m_CanonicText = null;
            foreach (Termin t in Termins) 
            {
                if (ignoreTerminsWithNotnullTags && t.Tag != null) 
                    continue;
                if (t.Terms.Count == 0) 
                    continue;
                string s = t.CanonicText;
                if (!Pullenti.Morph.LanguageHelper.IsCyrillicChar(s[0])) 
                    continue;
                if (m_CanonicText == null) 
                    m_CanonicText = s;
                else if (s.Length < m_CanonicText.Length) 
                    m_CanonicText = s;
            }
        }
        /// <summary>
        /// Необязательный тип элемента
        /// </summary>
        public string Typ;
        /// <summary>
        /// Используется произвольным образом (для некоторого дополнительного признака)
        /// </summary>
        public object MiscAttr;
        /// <summary>
        /// Ссылка на онтологию
        /// </summary>
        public IntOntologyCollection Owner;
        /// <summary>
        /// Ссылка на сущность
        /// </summary>
        public Pullenti.Ner.Referent Referent;
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Typ != null) 
                res.AppendFormat("{0}: ", Typ);
            res.Append(CanonicText);
            foreach (Termin t in Termins) 
            {
                string tt = t.ToString();
                if (tt == CanonicText) 
                    continue;
                res.Append("; ");
                res.Append(tt);
            }
            if (Referent != null) 
                res.AppendFormat(" [{0}]", Referent);
            return res.ToString();
        }
    }
}