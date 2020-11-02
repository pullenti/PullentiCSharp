/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Словарь некоторых обозначений, терминов, сокращений. Очень полезный класс! 
    /// Рассчитан на быстрый поиск токена или группы токенов среди большого списка терминов.
    /// </summary>
    public class TerminCollection
    {
        /// <summary>
        /// Добавить термин. После добавления нельзя вносить изменения в термин, 
        /// кроме как в значения Tag и Tag2 (иначе потом нужно вызвать Reindex).
        /// </summary>
        /// <param name="term">термин</param>
        public void Add(Termin term)
        {
            Termins.Add(term);
            m_HashCanonic = null;
            this.Reindex(term);
        }
        /// <summary>
        /// Добавить строку в качестве записи словаря (термина).
        /// </summary>
        /// <param name="termins">строка, которая подвергается морфологическому анализу, и в термин добавляются все варианты разбора</param>
        /// <param name="tag">это просто значения Tag для термина</param>
        /// <param name="lang">язык (можно null, если язык анализируемого текста)</param>
        /// <param name="isNormalText">если true, то исходный текст не нужно морфологически разбирать - он уже в нормальной форме и верхнем регистре</param>
        /// <return>добавленный термин</return>
        public Termin AddString(string termins, object tag = null, Pullenti.Morph.MorphLang lang = null, bool isNormalText = false)
        {
            Termin t = new Termin(termins, lang, isNormalText || AllAddStrsNormalized);
            t.Tag = tag;
            if (tag != null && t.Terms.Count == 1) 
            {
            }
            this.Add(t);
            return t;
        }
        /// <summary>
        /// Полный список терминов (Termin)
        /// </summary>
        public List<Termin> Termins = new List<Termin>();
        // Если установлено true, то все входные термины уже нормализованы
        // (сделано для ускорения загрузки в Питоне).
        public bool AllAddStrsNormalized = false;
        /// <summary>
        /// Возможный словарь синонимов (если в словаре комбинация не найдена, а она есть в синонимах, 
        /// то синонимы ищутся в текущем словаре, и если есть, то ОК). Обычно null.
        /// </summary>
        public TerminCollection Synonyms = null;
        /// <summary>
        /// Используйте произвольным образом
        /// </summary>
        public object Tag;
        class CharNode
        {
            public Dictionary<short, CharNode> Children;
            public List<Pullenti.Ner.Core.Termin> Termins;
        }

        CharNode m_Root = new CharNode();
        CharNode m_RootUa = new CharNode();
        CharNode _getRoot(Pullenti.Morph.MorphLang lang, bool isLat)
        {
            if (lang != null && lang.IsUa && !lang.IsRu) 
                return m_RootUa;
            return m_Root;
        }
        Dictionary<short, List<Termin>> m_Hash1 = new Dictionary<short, List<Termin>>();
        Dictionary<string, List<Termin>> m_HashCanonic = null;
        /// <summary>
        /// Переиндексировать термин (если после добавления у него что-либо поменялось)
        /// </summary>
        /// <param name="t">термин для переиндексации</param>
        public void Reindex(Termin t)
        {
            if (t == null) 
                return;
            if (t.Terms.Count > 20) 
            {
            }
            if (t.AcronymSmart != null) 
                this.AddToHash1((short)t.AcronymSmart[0], t);
            if (t.Abridges != null) 
            {
                foreach (Termin.Abridge a in t.Abridges) 
                {
                    if (a.Parts[0].Value.Length == 1) 
                        this.AddToHash1((short)a.Parts[0].Value[0], t);
                }
            }
            foreach (string v in t.GetHashVariants()) 
            {
                this._AddToTree(v, t);
            }
            if (t.AdditionalVars != null) 
            {
                foreach (Termin av in t.AdditionalVars) 
                {
                    av.IgnoreTermsOrder = t.IgnoreTermsOrder;
                    foreach (string v in av.GetHashVariants()) 
                    {
                        this._AddToTree(v, t);
                    }
                }
            }
        }
        public void Remove(Termin t)
        {
            foreach (string v in t.GetHashVariants()) 
            {
                this._RemoveFromTree(v, t);
            }
            foreach (List<Termin> li in m_Hash1.Values) 
            {
                foreach (Termin tt in li) 
                {
                    if (tt == t) 
                    {
                        li.Remove(tt);
                        break;
                    }
                }
            }
            int i = Termins.IndexOf(t);
            if (i >= 0) 
                Termins.RemoveAt(i);
        }
        void _AddToTree(string key, Termin t)
        {
            if (key == null) 
                return;
            CharNode nod = this._getRoot(t.Lang, t.Lang.IsUndefined && Pullenti.Morph.LanguageHelper.IsLatin(key));
            for (int i = 0; i < key.Length; i++) 
            {
                short ch = (short)key[i];
                if (nod.Children == null) 
                    nod.Children = new Dictionary<short, CharNode>();
                CharNode nn;
                if (!nod.Children.TryGetValue(ch, out nn)) 
                    nod.Children.Add(ch, (nn = new CharNode()));
                nod = nn;
            }
            if (nod.Termins == null) 
                nod.Termins = new List<Termin>();
            if (!nod.Termins.Contains(t)) 
                nod.Termins.Add(t);
        }
        void _RemoveFromTree(string key, Termin t)
        {
            if (key == null) 
                return;
            CharNode nod = this._getRoot(t.Lang, t.Lang.IsUndefined && Pullenti.Morph.LanguageHelper.IsLatin(key));
            for (int i = 0; i < key.Length; i++) 
            {
                short ch = (short)key[i];
                if (nod.Children == null) 
                    return;
                CharNode nn;
                if (!nod.Children.TryGetValue(ch, out nn)) 
                    return;
                nod = nn;
            }
            if (nod.Termins == null) 
                return;
            if (nod.Termins.Contains(t)) 
                nod.Termins.Remove(t);
        }
        List<Termin> _FindInTree(string key, Pullenti.Morph.MorphLang lang)
        {
            if (key == null) 
                return null;
            CharNode nod = this._getRoot(lang, ((lang == null || lang.IsUndefined)) && Pullenti.Morph.LanguageHelper.IsLatin(key));
            for (int i = 0; i < key.Length; i++) 
            {
                short ch = (short)key[i];
                CharNode nn = null;
                if (nod.Children != null) 
                    nod.Children.TryGetValue(ch, out nn);
                if (nn == null) 
                {
                    if (ch == 32) 
                    {
                        if (nod.Termins != null) 
                        {
                            string[] pp = key.Split(' ');
                            List<Termin> res = null;
                            foreach (Termin t in nod.Termins) 
                            {
                                if (t.Terms.Count == pp.Length) 
                                {
                                    int k;
                                    for (k = 1; k < pp.Length; k++) 
                                    {
                                        if (!t.Terms[k].Variants.Contains(pp[k])) 
                                            break;
                                    }
                                    if (k >= pp.Length) 
                                    {
                                        if (res == null) 
                                            res = new List<Termin>();
                                        res.Add(t);
                                    }
                                }
                            }
                            return res;
                        }
                    }
                    return null;
                }
                nod = nn;
            }
            return nod.Termins;
        }
        void AddToHash1(short key, Termin t)
        {
            List<Termin> li = null;
            if (!m_Hash1.TryGetValue(key, out li)) 
                m_Hash1.Add(key, (li = new List<Termin>()));
            if (!li.Contains(t)) 
                li.Add(t);
        }
        public Termin Find(string key)
        {
            if (string.IsNullOrEmpty(key)) 
                return null;
            List<Termin> li;
            if (Pullenti.Morph.LanguageHelper.IsLatinChar(key[0])) 
                li = this._FindInTree(key, Pullenti.Morph.MorphLang.EN);
            else 
            {
                li = this._FindInTree(key, Pullenti.Morph.MorphLang.RU);
                if (li == null) 
                    li = this._FindInTree(key, Pullenti.Morph.MorphLang.UA);
            }
            return (li != null && li.Count > 0 ? li[0] : null);
        }
        /// <summary>
        /// Попытка найти термин в словаре для начального токена
        /// </summary>
        /// <param name="token">начальный токен</param>
        /// <param name="attrs">атрибуты выделения</param>
        /// <return>результирующий токен, если привязалось несколько, то первый, если ни одного, то null</return>
        public TerminToken TryParse(Pullenti.Ner.Token token, TerminParseAttr attrs = TerminParseAttr.No)
        {
            if (Termins.Count == 0) 
                return null;
            List<TerminToken> li = this.TryParseAll(token, attrs);
            if (li != null) 
                return li[0];
            else 
                return null;
        }
        /// <summary>
        /// Попытка привязать все возможные термины
        /// </summary>
        /// <param name="token">начальный токен</param>
        /// <param name="attrs">атрибуты выделения</param>
        /// <return>список из всех подходящих привязок TerminToken или null</return>
        public List<TerminToken> TryParseAll(Pullenti.Ner.Token token, TerminParseAttr attrs = TerminParseAttr.No)
        {
            if (token == null) 
                return null;
            List<TerminToken> re = this._TryAttachAll_(token, attrs, false);
            if (re == null && token.Morph.Language.IsUa) 
                re = this._TryAttachAll_(token, attrs, true);
            if (re == null && Synonyms != null) 
            {
                TerminToken re0 = Synonyms.TryParse(token, TerminParseAttr.No);
                if (re0 != null && (re0.Termin.Tag is List<string>)) 
                {
                    Termin term = this.Find(re0.Termin.CanonicText);
                    foreach (string syn in re0.Termin.Tag as List<string>) 
                    {
                        if (term != null) 
                            break;
                        term = this.Find(syn);
                    }
                    if (term != null) 
                    {
                        re0.Termin = term;
                        List<TerminToken> res1 = new List<TerminToken>();
                        res1.Add(re0);
                        return res1;
                    }
                }
            }
            return re;
        }
        // Привязка с точностью до похожести
        // simD - параметр "похожесть (0.05..1)"
        public List<TerminToken> TryParseAllSim(Pullenti.Ner.Token token, double simD)
        {
            if (simD >= 1 || (simD < 0.05)) 
                return this.TryParseAll(token, TerminParseAttr.No);
            if (Termins.Count == 0 || token == null) 
                return null;
            Pullenti.Ner.TextToken tt = token as Pullenti.Ner.TextToken;
            if (tt == null && (token is Pullenti.Ner.ReferentToken)) 
                tt = (token as Pullenti.Ner.ReferentToken).BeginToken as Pullenti.Ner.TextToken;
            List<TerminToken> res = null;
            foreach (Termin t in Termins) 
            {
                if (!t.Lang.IsUndefined) 
                {
                    if (!token.Morph.Language.IsUndefined) 
                    {
                        if (((token.Morph.Language & t.Lang)).IsUndefined) 
                            continue;
                    }
                }
                TerminToken ar = t.TryParseSim(tt, simD, TerminParseAttr.No);
                if (ar == null) 
                    continue;
                ar.Termin = t;
                if (res == null || ar.TokensCount > res[0].TokensCount) 
                {
                    res = new List<TerminToken>();
                    res.Add(ar);
                }
                else if (ar.TokensCount == res[0].TokensCount) 
                    res.Add(ar);
            }
            return res;
        }
        List<TerminToken> _TryAttachAll_(Pullenti.Ner.Token token, TerminParseAttr pars = TerminParseAttr.No, bool mainRoot = false)
        {
            if (Termins.Count == 0 || token == null) 
                return null;
            string s = null;
            Pullenti.Ner.TextToken tt = token as Pullenti.Ner.TextToken;
            if (tt == null && (token is Pullenti.Ner.ReferentToken)) 
                tt = (token as Pullenti.Ner.ReferentToken).BeginToken as Pullenti.Ner.TextToken;
            List<TerminToken> res = null;
            bool wasVars = false;
            CharNode root = (mainRoot ? m_Root : this._getRoot(token.Morph.Language, token.Chars.IsLatinLetter));
            if (tt != null) 
            {
                s = tt.Term;
                CharNode nod = root;
                bool noVars = false;
                int len0 = 0;
                if (((pars & TerminParseAttr.TermOnly)) != TerminParseAttr.No) 
                {
                }
                else if (tt.InvariantPrefixLengthOfMorphVars <= s.Length) 
                {
                    len0 = tt.InvariantPrefixLengthOfMorphVars;
                    for (int i = 0; i < tt.InvariantPrefixLengthOfMorphVars; i++) 
                    {
                        short ch = (short)s[i];
                        if (nod.Children == null) 
                        {
                            noVars = true;
                            break;
                        }
                        CharNode nn;
                        if (!nod.Children.TryGetValue(ch, out nn)) 
                        {
                            noVars = true;
                            break;
                        }
                        nod = nn;
                    }
                }
                if (!noVars) 
                {
                    if (this._manageVar(token, pars, s, nod, len0, ref res)) 
                        wasVars = true;
                    for (int i = 0; i < tt.Morph.ItemsCount; i++) 
                    {
                        if (((pars & TerminParseAttr.TermOnly)) != TerminParseAttr.No) 
                            continue;
                        Pullenti.Morph.MorphWordForm wf = tt.Morph[i] as Pullenti.Morph.MorphWordForm;
                        if (wf == null) 
                            continue;
                        if (((pars & TerminParseAttr.InDictionaryOnly)) != TerminParseAttr.No) 
                        {
                            if (!wf.IsInDictionary) 
                                continue;
                        }
                        int j;
                        bool ok = true;
                        if (wf.NormalCase == null || wf.NormalCase == s) 
                            ok = false;
                        else 
                        {
                            for (j = 0; j < i; j++) 
                            {
                                Pullenti.Morph.MorphWordForm wf2 = tt.Morph[j] as Pullenti.Morph.MorphWordForm;
                                if (wf2 != null) 
                                {
                                    if (wf2.NormalCase == wf.NormalCase || wf2.NormalFull == wf.NormalCase) 
                                        break;
                                }
                            }
                            if (j < i) 
                                ok = false;
                        }
                        if (ok) 
                        {
                            if (this._manageVar(token, pars, wf.NormalCase, nod, tt.InvariantPrefixLengthOfMorphVars, ref res)) 
                                wasVars = true;
                        }
                        if (wf.NormalFull == null || wf.NormalFull == wf.NormalCase || wf.NormalFull == s) 
                            continue;
                        for (j = 0; j < i; j++) 
                        {
                            Pullenti.Morph.MorphWordForm wf2 = tt.Morph[j] as Pullenti.Morph.MorphWordForm;
                            if (wf2 != null && wf2.NormalFull == wf.NormalFull) 
                                break;
                        }
                        if (j < i) 
                            continue;
                        if (this._manageVar(token, pars, wf.NormalFull, nod, tt.InvariantPrefixLengthOfMorphVars, ref res)) 
                            wasVars = true;
                    }
                }
            }
            else if (token is Pullenti.Ner.NumberToken) 
            {
                if (this._manageVar(token, pars, (token as Pullenti.Ner.NumberToken).Value.ToString(), root, 0, ref res)) 
                    wasVars = true;
            }
            else 
                return null;
            if (!wasVars && s != null && s.Length == 1) 
            {
                List<Termin> vars;
                if (m_Hash1.TryGetValue((short)s[0], out vars)) 
                {
                    foreach (Termin t in vars) 
                    {
                        if (!t.Lang.IsUndefined) 
                        {
                            if (!token.Morph.Language.IsUndefined) 
                            {
                                if (((token.Morph.Language & t.Lang)).IsUndefined) 
                                    continue;
                            }
                        }
                        TerminToken ar = t.TryParse(tt, TerminParseAttr.No);
                        if (ar == null) 
                            continue;
                        ar.Termin = t;
                        if (res == null) 
                        {
                            res = new List<TerminToken>();
                            res.Add(ar);
                        }
                        else if (ar.TokensCount > res[0].TokensCount) 
                        {
                            res.Clear();
                            res.Add(ar);
                        }
                        else if (ar.TokensCount == res[0].TokensCount) 
                            res.Add(ar);
                    }
                }
            }
            if (res != null) 
            {
                int ii = 0;
                int max = 0;
                for (int i = 0; i < res.Count; i++) 
                {
                    if (res[i].LengthChar > max) 
                    {
                        max = res[i].LengthChar;
                        ii = i;
                    }
                }
                if (ii > 0) 
                {
                    TerminToken v = res[ii];
                    res.RemoveAt(ii);
                    res.Insert(0, v);
                }
            }
            return res;
        }
        bool _manageVar(Pullenti.Ner.Token token, TerminParseAttr pars, string v, CharNode nod, int i0, ref List<TerminToken> res)
        {
            for (int i = i0; i < v.Length; i++) 
            {
                short ch = (short)v[i];
                if (nod.Children == null) 
                    return false;
                CharNode nn;
                if (!nod.Children.TryGetValue(ch, out nn)) 
                    return false;
                nod = nn;
            }
            List<Termin> vars = nod.Termins;
            if (vars == null || vars.Count == 0) 
                return false;
            foreach (Termin t in vars) 
            {
                TerminToken ar = t.TryParse(token, pars);
                if (ar != null) 
                {
                    ar.Termin = t;
                    if (res == null) 
                    {
                        res = new List<TerminToken>();
                        res.Add(ar);
                    }
                    else if (ar.TokensCount > res[0].TokensCount) 
                    {
                        res.Clear();
                        res.Add(ar);
                    }
                    else if (ar.TokensCount == res[0].TokensCount) 
                    {
                        int j;
                        for (j = 0; j < res.Count; j++) 
                        {
                            if (res[j].Termin == ar.Termin) 
                                break;
                        }
                        if (j >= res.Count) 
                            res.Add(ar);
                    }
                }
                if (t.AdditionalVars != null) 
                {
                    foreach (Termin av in t.AdditionalVars) 
                    {
                        ar = av.TryParse(token, pars);
                        if (ar == null) 
                            continue;
                        ar.Termin = t;
                        if (res == null) 
                        {
                            res = new List<TerminToken>();
                            res.Add(ar);
                        }
                        else if (ar.TokensCount > res[0].TokensCount) 
                        {
                            res.Clear();
                            res.Add(ar);
                        }
                        else if (ar.TokensCount == res[0].TokensCount) 
                        {
                            int j;
                            for (j = 0; j < res.Count; j++) 
                            {
                                if (res[j].Termin == ar.Termin) 
                                    break;
                            }
                            if (j >= res.Count) 
                                res.Add(ar);
                        }
                    }
                }
            }
            return v.Length > 1;
        }
        /// <summary>
        /// Поискать эквивалентные термины
        /// </summary>
        /// <param name="termin">термин</param>
        /// <return>список эквивалентных терминов Termin или null</return>
        public List<Termin> FindTerminsByTermin(Termin termin)
        {
            List<Termin> res = null;
            foreach (string v in termin.GetHashVariants()) 
            {
                List<Termin> vars = this._FindInTree(v, termin.Lang);
                if (vars == null) 
                    continue;
                foreach (Termin t in vars) 
                {
                    if (t.IsEqual(termin)) 
                    {
                        if (res == null) 
                            res = new List<Termin>();
                        if (!res.Contains(t)) 
                            res.Add(t);
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// Поискать термины по строке
        /// </summary>
        /// <param name="str">поисковая строка</param>
        /// <param name="lang">возможный язык (null)</param>
        /// <return>список терминов Termin или null</return>
        public List<Termin> FindTerminsByString(string str, Pullenti.Morph.MorphLang lang = null)
        {
            return this._FindInTree(str, lang);
        }
        public List<Termin> FindTerminsByCanonicText(string text)
        {
            if (m_HashCanonic == null) 
            {
                m_HashCanonic = new Dictionary<string, List<Termin>>();
                foreach (Termin t in Termins) 
                {
                    string ct = t.CanonicText;
                    List<Termin> li;
                    if (!m_HashCanonic.TryGetValue(ct, out li)) 
                        m_HashCanonic.Add(ct, (li = new List<Termin>()));
                    if (!li.Contains(t)) 
                        li.Add(t);
                }
            }
            List<Termin> res;
            if (!m_HashCanonic.TryGetValue(text, out res)) 
                return null;
            else 
                return res;
        }
    }
}