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

namespace Pullenti.Ner.Core
{
    // Элемент внутреннего онтологического словаря
    public class IntOntologyItem
    {
        public IntOntologyItem(Pullenti.Ner.Referent r)
        {
            Referent = r;
        }
        public List<Termin> Termins = new List<Termin>();
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
        public string Typ;
        public object MiscAttr;
        public IntOntologyCollection Owner;
        public Pullenti.Ner.Referent Referent;
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