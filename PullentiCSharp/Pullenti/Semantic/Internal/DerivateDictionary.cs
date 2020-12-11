/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Pullenti.Semantic.Internal
{
    public class DerivateDictionary
    {
        public Pullenti.Morph.MorphLang Lang;
        bool m_Inited = false;
        Pullenti.Morph.Internal.ByteArrayWrapper m_Buf;
        public void Load(byte[] dat)
        {
            using (MemoryStream mem = new MemoryStream(dat)) 
            {
                m_AllGroups.Clear();
                m_Root = new ExplanTreeNode();
                this.Deserialize(mem, true);
                m_Inited = true;
            }
        }
        public bool Init(Pullenti.Morph.MorphLang lang, bool lazy)
        {
            if (m_Inited) 
                return true;
            Assembly assembly = Assembly.GetExecutingAssembly();
            string rsname = string.Format("d_{0}.dat", lang.ToString());
            string[] names = assembly.GetManifestResourceNames();
            foreach (string n in names) 
            {
                if (n.EndsWith(rsname, StringComparison.OrdinalIgnoreCase)) 
                {
                    object inf = assembly.GetManifestResourceInfo(n);
                    if (inf == null) 
                        continue;
                    using (Stream stream = assembly.GetManifestResourceStream(n)) 
                    {
                        stream.Position = 0;
                        m_AllGroups.Clear();
                        this.Deserialize(stream, lazy);
                        Lang = lang;
                    }
                    m_Inited = true;
                    return true;
                }
            }
            return false;
        }
        internal ExplanTreeNode m_Root = new ExplanTreeNode();
        public void Unload()
        {
            m_Root = new ExplanTreeNode();
            m_AllGroups.Clear();
            Lang = new Pullenti.Morph.MorphLang();
        }
        internal List<Pullenti.Semantic.Utils.DerivateGroup> m_AllGroups = new List<Pullenti.Semantic.Utils.DerivateGroup>();
        internal Pullenti.Semantic.Utils.DerivateGroup GetGroup(int id)
        {
            if (id >= 1 && id <= m_AllGroups.Count) 
                return m_AllGroups[id - 1];
            return null;
        }
        internal object m_Lock = new object();
        void _loadTreeNode(ExplanTreeNode tn)
        {
            lock (m_Lock) 
            {
                int pos = tn.LazyPos;
                if (pos > 0) 
                    tn.Deserialize(m_Buf, this, true, ref pos);
                tn.LazyPos = 0;
            }
        }
        internal void Deserialize(Stream str, bool lazyLoad)
        {
            Pullenti.Morph.Internal.ByteArrayWrapper wr = null;
            using (MemoryStream tmp = new MemoryStream()) 
            {
                Pullenti.Morph.Internal.MorphDeserializer.DeflateGzip(str, tmp);
                wr = new Pullenti.Morph.Internal.ByteArrayWrapper(tmp.ToArray());
                int pos = 0;
                int cou = wr.DeserializeInt(ref pos);
                for (; cou > 0; cou--) 
                {
                    int p1 = wr.DeserializeInt(ref pos);
                    Pullenti.Semantic.Utils.DerivateGroup ew = new Pullenti.Semantic.Utils.DerivateGroup();
                    if (lazyLoad) 
                    {
                        ew.LazyPos = pos;
                        pos = p1;
                    }
                    else 
                        ew.Deserialize(wr, ref pos);
                    ew.Id = m_AllGroups.Count + 1;
                    m_AllGroups.Add(ew);
                }
                m_Root = new ExplanTreeNode();
                m_Root.Deserialize(wr, this, lazyLoad, ref pos);
            }
            m_Buf = wr;
        }
        public List<Pullenti.Semantic.Utils.DerivateGroup> Find(string word, bool tryCreate, Pullenti.Morph.MorphLang lang)
        {
            if (string.IsNullOrEmpty(word)) 
                return null;
            ExplanTreeNode tn = m_Root;
            int i;
            for (i = 0; i < word.Length; i++) 
            {
                short k = (short)word[i];
                if (tn.Nodes == null) 
                    break;
                if (!tn.Nodes.ContainsKey(k)) 
                    break;
                tn = tn.Nodes[k];
                if (tn.LazyPos > 0) 
                    this._loadTreeNode(tn);
            }
            List<Pullenti.Semantic.Utils.DerivateGroup> li = null;
            if (i >= word.Length && tn.Groups != null) 
            {
                li = new List<Pullenti.Semantic.Utils.DerivateGroup>();
                foreach (int g in tn.Groups) 
                {
                    li.Add(this.GetGroup(g));
                }
                bool gen = false;
                bool nogen = false;
                foreach (Pullenti.Semantic.Utils.DerivateGroup g in li) 
                {
                    if (g.IsGenerated) 
                        gen = true;
                    else 
                        nogen = true;
                }
                if (gen && nogen) 
                {
                    for (i = li.Count - 1; i >= 0; i--) 
                    {
                        if (li[i].IsGenerated) 
                            li.RemoveAt(i);
                    }
                }
            }
            if (li != null && lang != null && !lang.IsUndefined) 
            {
                for (i = li.Count - 1; i >= 0; i--) 
                {
                    if (!li[i].ContainsWord(word, lang)) 
                        li.RemoveAt(i);
                }
            }
            if (li != null && li.Count > 0) 
                return li;
            if (word.Length < 4) 
                return null;
            char ch0 = word[word.Length - 1];
            char ch1 = word[word.Length - 2];
            char ch2 = word[word.Length - 3];
            if (ch0 == 'О' || ((ch0 == 'И' && ch1 == 'К'))) 
            {
                string word1 = word.Substring(0, word.Length - 1);
                if ((((li = this.Find(word1 + "ИЙ", false, lang)))) != null) 
                    return li;
                if ((((li = this.Find(word1 + "ЫЙ", false, lang)))) != null) 
                    return li;
                if (ch0 == 'О' && ch1 == 'Н') 
                {
                    if ((((li = this.Find(word1 + "СКИЙ", false, lang)))) != null) 
                        return li;
                }
            }
            else if (((ch0 == 'Я' || ch0 == 'Ь')) && (word[word.Length - 2] == 'С')) 
            {
                string word1 = word.Substring(0, word.Length - 2);
                if (word1 == "ЯТЬ") 
                    return null;
                if ((((li = this.Find(word1, false, lang)))) != null) 
                    return li;
            }
            else if (ch0 == 'Е' && ch1 == 'Ь') 
            {
                string word1 = word.Substring(0, word.Length - 2) + "ИЕ";
                if ((((li = this.Find(word1, false, lang)))) != null) 
                    return li;
            }
            else if (ch0 == 'Й' && ch2 == 'Н' && tryCreate) 
            {
                char ch3 = word[word.Length - 4];
                string word1 = null;
                if (ch3 != 'Н') 
                {
                    if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(ch3)) 
                        word1 = word.Substring(0, word.Length - 3) + "Н" + word.Substring(word.Length - 3);
                }
                else 
                    word1 = word.Substring(0, word.Length - 4) + word.Substring(word.Length - 3);
                if (word1 != null) 
                {
                    if ((((li = this.Find(word1, false, lang)))) != null) 
                        return li;
                }
            }
            if (ch0 == 'Й' && ch1 == 'О') 
            {
                string word2 = word.Substring(0, word.Length - 2);
                if ((((li = this.Find(word2 + "ИЙ", false, lang)))) != null) 
                    return li;
                if ((((li = this.Find(word2 + "ЫЙ", false, lang)))) != null) 
                    return li;
            }
            if (!tryCreate) 
                return null;
            return null;
        }
    }
}