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
using System.Text;

namespace Pullenti.Morph.Internal
{
    public class MorphEngine
    {
        internal object m_Lock = new object();
        ByteArrayWrapper m_LazyBuf;
        ByteArrayWrapper GetLazyBuf()
        {
            return m_LazyBuf;
        }
        public MorphTreeNode m_Root = new MorphTreeNode();
        public MorphTreeNode m_RootReverce = new MorphTreeNode();
        List<MorphRule> m_Rules = new List<MorphRule>();
        public void AddRule(MorphRule r)
        {
            m_Rules.Add(r);
        }
        public MorphRule GetRule(int id)
        {
            if (id > 0 && id <= m_Rules.Count) 
                return m_Rules[id - 1];
            return null;
        }
        public MorphRule GetMutRule(int id)
        {
            if (id > 0 && id <= m_Rules.Count) 
                return m_Rules[id - 1];
            return null;
        }
        public MorphRuleVariant GetRuleVar(int rid, int vid)
        {
            MorphRule r = this.GetRule(rid);
            if (r == null) 
                return null;
            return r.FindVar(vid);
        }
        List<Pullenti.Morph.MorphMiscInfo> m_MiscInfos = new List<Pullenti.Morph.MorphMiscInfo>();
        public void AddMiscInfo(Pullenti.Morph.MorphMiscInfo mi)
        {
            if (mi.Id == 0) 
                mi.Id = m_MiscInfos.Count + 1;
            m_MiscInfos.Add(mi);
        }
        public Pullenti.Morph.MorphMiscInfo GetMiscInfo(int id)
        {
            if (id > 0 && id <= m_MiscInfos.Count) 
                return m_MiscInfos[id - 1];
            return null;
        }
        public bool Initialize(Pullenti.Morph.MorphLang lang, bool lazyLoad)
        {
            if (!Language.IsUndefined) 
                return false;
            lock (m_Lock) 
            {
                if (!Language.IsUndefined) 
                    return false;
                Language = lang;
                Assembly assembly = Assembly.GetExecutingAssembly();
                string rsname = string.Format("m_{0}.dat", lang.ToString());
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
                            this.Deserialize(stream, false, lazyLoad);
                        }
                        return true;
                    }
                }
                return false;
            }
        }
        void _loadTreeNode(MorphTreeNode tn)
        {
            lock (m_Lock) 
            {
                int pos = tn.LazyPos;
                if (pos > 0) 
                    tn.DeserializeLazy(m_LazyBuf, this, ref pos);
                tn.LazyPos = 0;
            }
        }
        public Pullenti.Morph.MorphLang Language = new Pullenti.Morph.MorphLang();
        public List<Pullenti.Morph.MorphWordForm> Process(string word)
        {
            if (string.IsNullOrEmpty(word)) 
                return null;
            List<Pullenti.Morph.MorphWordForm> res = null;
            int i;
            if (word.Length > 1) 
            {
                for (i = 0; i < word.Length; i++) 
                {
                    char ch = word[i];
                    if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(ch) || Pullenti.Morph.LanguageHelper.IsLatinVowel(ch)) 
                        break;
                }
                if (i >= word.Length) 
                    return res;
            }
            List<MorphRuleVariant> mvs;
            MorphTreeNode tn = m_Root;
            for (i = 0; i <= word.Length; i++) 
            {
                if (tn.LazyPos > 0) 
                    this._loadTreeNode(tn);
                if (tn.RuleIds != null) 
                {
                    string wordBegin = null;
                    string wordEnd = null;
                    if (i == 0) 
                        wordEnd = word;
                    else if (i < word.Length) 
                        wordEnd = word.Substring(i);
                    else 
                        wordEnd = string.Empty;
                    if (res == null) 
                        res = new List<Pullenti.Morph.MorphWordForm>();
                    foreach (int rid in tn.RuleIds) 
                    {
                        MorphRule r = this.GetRule(rid);
                        mvs = r.GetVars(wordEnd);
                        if (mvs == null) 
                            continue;
                        if (wordBegin == null) 
                        {
                            if (i == word.Length) 
                                wordBegin = word;
                            else if (i > 0) 
                                wordBegin = word.Substring(0, i);
                            else 
                                wordBegin = string.Empty;
                        }
                        this.ProcessResult(res, wordBegin, mvs);
                    }
                }
                if (tn.Nodes == null || i >= word.Length) 
                    break;
                short ch = (short)word[i];
                if (!tn.Nodes.TryGetValue(ch, out tn)) 
                    break;
            }
            bool needTestUnknownVars = true;
            if (res != null) 
            {
                foreach (Pullenti.Morph.MorphWordForm r in res) 
                {
                    if ((r.Class.IsPronoun || r.Class.IsNoun || r.Class.IsAdjective) || (r.Class.IsMisc && r.Class.IsConjunction) || r.Class.IsPreposition) 
                        needTestUnknownVars = false;
                    else if (r.Class.IsAdverb && r.NormalCase != null) 
                    {
                        if (!Pullenti.Morph.LanguageHelper.EndsWithEx(r.NormalCase, "О", "А", null, null)) 
                            needTestUnknownVars = false;
                        else if (r.NormalCase == "МНОГО") 
                            needTestUnknownVars = false;
                    }
                    else if (r.Class.IsVerb && res.Count > 1) 
                    {
                        bool ok = false;
                        foreach (Pullenti.Morph.MorphWordForm rr in res) 
                        {
                            if (rr != r && rr.Class != r.Class) 
                            {
                                ok = true;
                                break;
                            }
                        }
                        if (ok && !Pullenti.Morph.LanguageHelper.EndsWith(word, "ИМ")) 
                            needTestUnknownVars = false;
                    }
                }
            }
            if (needTestUnknownVars && Pullenti.Morph.LanguageHelper.IsCyrillicChar(word[0])) 
            {
                int gl = 0;
                int sog = 0;
                for (int j = 0; j < word.Length; j++) 
                {
                    if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(word[j])) 
                        gl++;
                    else 
                        sog++;
                }
                if ((gl < 2) || (sog < 2)) 
                    needTestUnknownVars = false;
            }
            if (needTestUnknownVars && res != null && res.Count == 1) 
            {
                if (res[0].Class.IsVerb) 
                {
                    if (res[0].Misc.Attrs.Contains("н.вр.") && res[0].Misc.Attrs.Contains("нес.в.") && !res[0].Misc.Attrs.Contains("страд.з.")) 
                        needTestUnknownVars = false;
                    else if (res[0].Misc.Attrs.Contains("б.вр.") && res[0].Misc.Attrs.Contains("сов.в.")) 
                        needTestUnknownVars = false;
                    else if (res[0].Misc.Attrs.Contains("инф.") && res[0].Misc.Attrs.Contains("сов.в.")) 
                        needTestUnknownVars = false;
                    else if (res[0].NormalCase != null && Pullenti.Morph.LanguageHelper.EndsWith(res[0].NormalCase, "СЯ")) 
                        needTestUnknownVars = false;
                }
                if (res[0].Class.IsUndefined && res[0].Misc.Attrs.Contains("прдктв.")) 
                    needTestUnknownVars = false;
            }
            if (needTestUnknownVars) 
            {
                if (m_RootReverce == null) 
                    return res;
                tn = m_RootReverce;
                MorphTreeNode tn0 = m_RootReverce;
                for (i = word.Length - 1; i >= 0; i--) 
                {
                    if (tn.LazyPos > 0) 
                        this._loadTreeNode(tn);
                    short ch = (short)word[i];
                    if (tn.Nodes == null) 
                        break;
                    if (!tn.Nodes.ContainsKey(ch)) 
                        break;
                    tn = tn.Nodes[ch];
                    if (tn.LazyPos > 0) 
                        this._loadTreeNode(tn);
                    if (tn.ReverceVariants != null) 
                    {
                        tn0 = tn;
                        break;
                    }
                }
                if (tn0 != m_RootReverce) 
                {
                    bool glas = i < 4;
                    for (; i >= 0; i--) 
                    {
                        if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(word[i]) || Pullenti.Morph.LanguageHelper.IsLatinVowel(word[i])) 
                        {
                            glas = true;
                            break;
                        }
                    }
                    if (glas) 
                    {
                        foreach (MorphRuleVariantRef mvref in tn0.ReverceVariants) 
                        {
                            MorphRuleVariant mv = this.GetRuleVar(mvref.RuleId, mvref.VariantId);
                            if (mv == null) 
                                continue;
                            if (((!mv.Class.IsVerb && !mv.Class.IsAdjective && !mv.Class.IsNoun) && !mv.Class.IsProperSurname && !mv.Class.IsProperGeo) && !mv.Class.IsProperSecname) 
                                continue;
                            bool ok = false;
                            foreach (Pullenti.Morph.MorphWordForm rr in res) 
                            {
                                if (rr.IsInDictionary) 
                                {
                                    if (rr.Class == mv.Class || rr.Class.IsNoun) 
                                    {
                                        ok = true;
                                        break;
                                    }
                                    if (!mv.Class.IsAdjective && rr.Class.IsVerb) 
                                    {
                                        ok = true;
                                        break;
                                    }
                                }
                            }
                            if (ok) 
                                continue;
                            if (mv.Tail.Length > 0 && !Pullenti.Morph.LanguageHelper.EndsWith(word, mv.Tail)) 
                                continue;
                            Pullenti.Morph.MorphWordForm r = new Pullenti.Morph.MorphWordForm(mv, word, this.GetMiscInfo(mv.MiscInfoId));
                            if (!r.HasMorphEquals(res)) 
                            {
                                r.UndefCoef = mvref.Coef;
                                if (res == null) 
                                    res = new List<Pullenti.Morph.MorphWordForm>();
                                res.Add(r);
                            }
                        }
                    }
                }
            }
            if (word == "ПРИ" && res != null) 
            {
                for (i = res.Count - 1; i >= 0; i--) 
                {
                    if (res[i].Class.IsProperGeo) 
                        res.RemoveAt(i);
                }
            }
            if (res == null || res.Count == 0) 
                return null;
            this.Sort(res, word);
            foreach (Pullenti.Morph.MorphWordForm v in res) 
            {
                if (v.NormalCase == null) 
                    v.NormalCase = word;
                if (v.Class.IsVerb) 
                {
                    if (v.NormalFull == null && Pullenti.Morph.LanguageHelper.EndsWith(v.NormalCase, "ТЬСЯ")) 
                        v.NormalFull = v.NormalCase.Substring(0, v.NormalCase.Length - 2);
                }
                v.Language = Language;
                if (v.Class.IsPreposition) 
                    v.NormalCase = Pullenti.Morph.LanguageHelper.NormalizePreposition(v.NormalCase);
            }
            Pullenti.Morph.MorphClass mc = new Pullenti.Morph.MorphClass();
            for (i = res.Count - 1; i >= 0; i--) 
            {
                if (!res[i].IsInDictionary && res[i].Class.IsAdjective && res.Count > 1) 
                {
                    if (res[i].Misc.Attrs.Contains("к.ф.") || res[i].Misc.Attrs.Contains("неизм.")) 
                    {
                        res.RemoveAt(i);
                        continue;
                    }
                }
                if (res[i].IsInDictionary) 
                    mc.Value |= res[i].Class.Value;
            }
            if (mc == Pullenti.Morph.MorphClass.Verb && res.Count > 1) 
            {
                foreach (Pullenti.Morph.MorphWordForm r in res) 
                {
                    if (r.UndefCoef > 100 && r.Class == Pullenti.Morph.MorphClass.Adjective) 
                        r.UndefCoef = 0;
                }
            }
            if (res.Count == 0) 
                return null;
            return res;
        }
        void ProcessResult(List<Pullenti.Morph.MorphWordForm> res, string wordBegin, List<MorphRuleVariant> mvs)
        {
            foreach (MorphRuleVariant mv in mvs) 
            {
                Pullenti.Morph.MorphWordForm r = new Pullenti.Morph.MorphWordForm(mv, null, this.GetMiscInfo(mv.MiscInfoId));
                    {
                        if (mv.NormalTail != null && mv.NormalTail.Length > 0 && mv.NormalTail[0] != '-') 
                            r.NormalCase = wordBegin + mv.NormalTail;
                        else 
                            r.NormalCase = wordBegin;
                    }
                if (mv.FullNormalTail != null) 
                {
                    if (mv.FullNormalTail.Length > 0 && mv.FullNormalTail[0] != '-') 
                        r.NormalFull = wordBegin + mv.FullNormalTail;
                    else 
                        r.NormalFull = wordBegin;
                }
                if (!r.HasMorphEquals(res)) 
                {
                    r.UndefCoef = 0;
                    res.Add(r);
                }
            }
        }
        public List<Pullenti.Morph.MorphWordForm> GetAllWordforms(string word)
        {
            List<Pullenti.Morph.MorphWordForm> res = new List<Pullenti.Morph.MorphWordForm>();
            int i;
            MorphTreeNode tn = m_Root;
            for (i = 0; i <= word.Length; i++) 
            {
                if (tn.LazyPos > 0) 
                    this._loadTreeNode(tn);
                if (tn.RuleIds != null) 
                {
                    string wordBegin = string.Empty;
                    string wordEnd = string.Empty;
                    if (i > 0) 
                        wordBegin = word.Substring(0, i);
                    else 
                        wordEnd = word;
                    if (i < word.Length) 
                        wordEnd = word.Substring(i);
                    else 
                        wordBegin = word;
                    foreach (int rid in tn.RuleIds) 
                    {
                        MorphRule r = this.GetRule(rid);
                        if (r.ContainsVar(wordEnd)) 
                        {
                            foreach (List<MorphRuleVariant> vl in r.MorphVars) 
                            {
                                foreach (MorphRuleVariant v in vl) 
                                {
                                    Pullenti.Morph.MorphWordForm wf = new Pullenti.Morph.MorphWordForm(v, null, this.GetMiscInfo(v.MiscInfoId));
                                    if (!wf.HasMorphEquals(res)) 
                                    {
                                        wf.NormalCase = wordBegin + v.Tail;
                                        wf.UndefCoef = 0;
                                        res.Add(wf);
                                    }
                                }
                            }
                        }
                    }
                }
                if (tn.Nodes == null || i >= word.Length) 
                    break;
                short ch = (short)word[i];
                if (!tn.Nodes.TryGetValue(ch, out tn)) 
                    break;
            }
            for (i = 0; i < res.Count; i++) 
            {
                Pullenti.Morph.MorphWordForm wf = res[i];
                if (wf.ContainsAttr("инф.", null)) 
                    continue;
                Pullenti.Morph.MorphCase cas = wf.Case;
                for (int j = i + 1; j < res.Count; j++) 
                {
                    Pullenti.Morph.MorphWordForm wf1 = res[j];
                    if (wf1.ContainsAttr("инф.", null)) 
                        continue;
                    if ((wf.Class == wf1.Class && wf.Gender == wf1.Gender && wf.Number == wf1.Number) && wf.NormalCase == wf1.NormalCase) 
                    {
                        cas |= wf1.Case;
                        res.RemoveAt(j);
                        j--;
                    }
                }
                if (cas != wf.Case) 
                    res[i].Case = cas;
            }
            for (i = 0; i < res.Count; i++) 
            {
                Pullenti.Morph.MorphWordForm wf = res[i];
                if (wf.ContainsAttr("инф.", null)) 
                    continue;
                for (int j = i + 1; j < res.Count; j++) 
                {
                    Pullenti.Morph.MorphWordForm wf1 = res[j];
                    if (wf1.ContainsAttr("инф.", null)) 
                        continue;
                    if ((wf.Class == wf1.Class && wf.Case == wf1.Case && wf.Number == wf1.Number) && wf.NormalCase == wf1.NormalCase) 
                    {
                        wf.Gender |= wf1.Gender;
                        res.RemoveAt(j);
                        j--;
                    }
                }
            }
            return res;
        }
        public string GetWordform(string word, Pullenti.Morph.MorphClass cla, Pullenti.Morph.MorphGender gender, Pullenti.Morph.MorphCase cas, Pullenti.Morph.MorphNumber num, Pullenti.Morph.MorphWordForm addInfo)
        {
            int i;
            MorphTreeNode tn = m_Root;
            bool find = false;
            string res = null;
            int maxCoef = -10;
            for (i = 0; i <= word.Length; i++) 
            {
                if (tn.LazyPos > 0) 
                    this._loadTreeNode(tn);
                if (tn.RuleIds != null) 
                {
                    string wordBegin = string.Empty;
                    string wordEnd = string.Empty;
                    if (i > 0) 
                        wordBegin = word.Substring(0, i);
                    else 
                        wordEnd = word;
                    if (i < word.Length) 
                        wordEnd = word.Substring(i);
                    else 
                        wordBegin = word;
                    foreach (int rid in tn.RuleIds) 
                    {
                        MorphRule r = this.GetRule(rid);
                        if (r != null && r.ContainsVar(wordEnd)) 
                        {
                            foreach (List<MorphRuleVariant> li in r.MorphVars) 
                            {
                                foreach (MorphRuleVariant v in li) 
                                {
                                    if (((cla.Value & v.Class.Value)) != 0 && v.NormalTail != null) 
                                    {
                                        if (cas.IsUndefined) 
                                        {
                                            if (v.Case.IsNominative || v.Case.IsUndefined) 
                                            {
                                            }
                                            else 
                                                continue;
                                        }
                                        else if (((v.Case & cas)).IsUndefined) 
                                            continue;
                                        bool sur = cla.IsProperSurname;
                                        bool sur0 = v.Class.IsProperSurname;
                                        if (sur || sur0) 
                                        {
                                            if (sur != sur0) 
                                                continue;
                                        }
                                        find = true;
                                        if (gender != Pullenti.Morph.MorphGender.Undefined) 
                                        {
                                            if (((gender & v.Gender)) == Pullenti.Morph.MorphGender.Undefined) 
                                            {
                                                if (num == Pullenti.Morph.MorphNumber.Plural) 
                                                {
                                                }
                                                else 
                                                    continue;
                                            }
                                        }
                                        if (num != Pullenti.Morph.MorphNumber.Undefined) 
                                        {
                                            if (((num & v.Number)) == Pullenti.Morph.MorphNumber.Undefined) 
                                                continue;
                                        }
                                        string re = wordBegin + v.Tail;
                                        int co = 0;
                                        if (addInfo != null) 
                                            co = this._calcEqCoef(v, addInfo);
                                        if (res == null || co > maxCoef) 
                                        {
                                            res = re;
                                            maxCoef = co;
                                        }
                                        if (maxCoef == 0) 
                                        {
                                            if ((wordBegin + v.NormalTail) == word) 
                                                return wordBegin + v.Tail;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (tn.Nodes == null || i >= word.Length) 
                    break;
                short ch = (short)word[i];
                if (!tn.Nodes.TryGetValue(ch, out tn)) 
                    break;
            }
            if (find) 
                return res;
            tn = m_RootReverce;
            MorphTreeNode tn0 = m_RootReverce;
            for (i = word.Length - 1; i >= 0; i--) 
            {
                if (tn.LazyPos > 0) 
                    this._loadTreeNode(tn);
                short ch = (short)word[i];
                if (tn.Nodes == null) 
                    break;
                if (!tn.Nodes.ContainsKey(ch)) 
                    break;
                tn = tn.Nodes[ch];
                if (tn.LazyPos > 0) 
                    this._loadTreeNode(tn);
                if (tn.ReverceVariants != null) 
                {
                    tn0 = tn;
                    break;
                }
            }
            if (tn0 == m_RootReverce) 
                return null;
            foreach (MorphRuleVariantRef mvr in tn0.ReverceVariants) 
            {
                MorphRule rule = this.GetRule(mvr.RuleId);
                if (rule == null) 
                    continue;
                MorphRuleVariant mv = rule.FindVar(mvr.VariantId);
                if (mv == null) 
                    continue;
                if (((mv.Class.Value & cla.Value)) != 0) 
                {
                    if (mv.Tail.Length > 0 && !Pullenti.Morph.LanguageHelper.EndsWith(word, mv.Tail)) 
                        continue;
                    string wordBegin = word.Substring(0, word.Length - mv.Tail.Length);
                    foreach (List<MorphRuleVariant> liv in rule.MorphVars) 
                    {
                        foreach (MorphRuleVariant v in liv) 
                        {
                            if (((v.Class.Value & cla.Value)) != 0) 
                            {
                                bool sur = cla.IsProperSurname;
                                bool sur0 = v.Class.IsProperSurname;
                                if (sur || sur0) 
                                {
                                    if (sur != sur0) 
                                        continue;
                                }
                                if (!cas.IsUndefined) 
                                {
                                    if (((cas & v.Case)).IsUndefined && !v.Case.IsUndefined) 
                                        continue;
                                }
                                if (num != Pullenti.Morph.MorphNumber.Undefined) 
                                {
                                    if (v.Number != Pullenti.Morph.MorphNumber.Undefined) 
                                    {
                                        if (((v.Number & num)) == Pullenti.Morph.MorphNumber.Undefined) 
                                            continue;
                                    }
                                }
                                if (gender != Pullenti.Morph.MorphGender.Undefined) 
                                {
                                    if (v.Gender != Pullenti.Morph.MorphGender.Undefined) 
                                    {
                                        if (((v.Gender & gender)) == Pullenti.Morph.MorphGender.Undefined) 
                                            continue;
                                    }
                                }
                                if (addInfo != null) 
                                {
                                    if (this._calcEqCoef(v, addInfo) < 0) 
                                        continue;
                                }
                                res = wordBegin + v.Tail;
                                if (res == word) 
                                    return word;
                                return res;
                            }
                        }
                    }
                }
            }
            if (cla.IsProperSurname) 
            {
                if ((gender == Pullenti.Morph.MorphGender.Feminie && cla.IsProperSurname && !cas.IsUndefined) && !cas.IsNominative) 
                {
                    if (word.EndsWith("ВА") || word.EndsWith("НА")) 
                    {
                        if (cas.IsAccusative) 
                            return word.Substring(0, word.Length - 1) + "У";
                        return word.Substring(0, word.Length - 1) + "ОЙ";
                    }
                }
                if (gender == Pullenti.Morph.MorphGender.Feminie) 
                {
                    char last = word[word.Length - 1];
                    if (last == 'А' || last == 'Я' || last == 'О') 
                        return word;
                    if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(last)) 
                        return word.Substring(0, word.Length - 1) + "А";
                    else if (last == 'Й') 
                        return word.Substring(0, word.Length - 2) + "АЯ";
                    else 
                        return word + "А";
                }
            }
            return res;
        }
        public string CorrectWordByMorph(string word)
        {
            List<string> vars = new List<string>();
            StringBuilder tmp = new StringBuilder(word.Length);
            for (int ch = 1; ch < word.Length; ch++) 
            {
                tmp.Length = 0;
                tmp.Append(word);
                tmp[ch] = '*';
                string var = this._checkCorrVar(tmp.ToString(), m_Root, 0);
                if (var != null) 
                {
                    if (!vars.Contains(var)) 
                        vars.Add(var);
                }
            }
            if (vars.Count == 0) 
            {
                for (int ch = 1; ch < word.Length; ch++) 
                {
                    tmp.Length = 0;
                    tmp.Append(word);
                    tmp.Insert(ch, '*');
                    string var = this._checkCorrVar(tmp.ToString(), m_Root, 0);
                    if (var != null) 
                    {
                        if (!vars.Contains(var)) 
                            vars.Add(var);
                    }
                }
            }
            if (vars.Count == 0) 
            {
                for (int ch = 1; ch < (word.Length - 1); ch++) 
                {
                    tmp.Length = 0;
                    tmp.Append(word);
                    tmp.Remove(ch, 1);
                    string var = this._checkCorrVar(tmp.ToString(), m_Root, 0);
                    if (var != null) 
                    {
                        if (!vars.Contains(var)) 
                            vars.Add(var);
                    }
                }
            }
            if (vars.Count != 1) 
                return null;
            return vars[0];
        }
        string _checkCorrVar(string word, MorphTreeNode tn, int i)
        {
            for (; i <= word.Length; i++) 
            {
                if (tn.LazyPos > 0) 
                    this._loadTreeNode(tn);
                if (tn.RuleIds != null) 
                {
                    string wordBegin = string.Empty;
                    string wordEnd = string.Empty;
                    if (i > 0) 
                        wordBegin = word.Substring(0, i);
                    else 
                        wordEnd = word;
                    if (i < word.Length) 
                        wordEnd = word.Substring(i);
                    else 
                        wordBegin = word;
                    foreach (int rid in tn.RuleIds) 
                    {
                        MorphRule r = this.GetRule(rid);
                        if (r.ContainsVar(wordEnd)) 
                            return wordBegin + wordEnd;
                        if (wordEnd.IndexOf('*') >= 0) 
                        {
                            foreach (string v in r.Tails) 
                            {
                                if (v.Length == wordEnd.Length) 
                                {
                                    int j;
                                    for (j = 0; j < v.Length; j++) 
                                    {
                                        if (wordEnd[j] == '*' || wordEnd[j] == v[j]) 
                                        {
                                        }
                                        else 
                                            break;
                                    }
                                    if (j >= v.Length) 
                                        return wordBegin + v;
                                }
                            }
                        }
                    }
                }
                if (tn.Nodes == null || i >= word.Length) 
                    break;
                short ch = (short)word[i];
                if (ch != 0x2A) 
                {
                    if (!tn.Nodes.ContainsKey(ch)) 
                        break;
                    tn = tn.Nodes[ch];
                    continue;
                }
                if (tn.Nodes != null) 
                {
                    foreach (KeyValuePair<short, MorphTreeNode> tnn in tn.Nodes) 
                    {
                        string ww = word.Replace('*', (char)tnn.Key);
                        string res = this._checkCorrVar(ww, tnn.Value, i + 1);
                        if (res != null) 
                            return res;
                    }
                }
                break;
            }
            return null;
        }
        public void ProcessSurnameVariants(string word, List<Pullenti.Morph.MorphWordForm> res)
        {
            this.ProcessProperVariants(word, res, false);
        }
        public void ProcessGeoVariants(string word, List<Pullenti.Morph.MorphWordForm> res)
        {
            this.ProcessProperVariants(word, res, true);
        }
        void ProcessProperVariants(string word, List<Pullenti.Morph.MorphWordForm> res, bool geo)
        {
            MorphTreeNode tn = m_RootReverce;
            List<MorphTreeNode> nodesWithVars = null;
            int i;
            for (i = word.Length - 1; i >= 0; i--) 
            {
                if (tn.LazyPos > 0) 
                    this._loadTreeNode(tn);
                short ch = (short)word[i];
                if (tn.Nodes == null) 
                    break;
                if (!tn.Nodes.ContainsKey(ch)) 
                    break;
                tn = tn.Nodes[ch];
                if (tn.LazyPos > 0) 
                    this._loadTreeNode(tn);
                if (tn.ReverceVariants != null) 
                {
                    if (nodesWithVars == null) 
                        nodesWithVars = new List<MorphTreeNode>();
                    nodesWithVars.Add(tn);
                }
            }
            if (nodesWithVars == null) 
                return;
            for (int j = nodesWithVars.Count - 1; j >= 0; j--) 
            {
                tn = nodesWithVars[j];
                if (tn.LazyPos > 0) 
                    this._loadTreeNode(tn);
                bool ok = false;
                foreach (MorphRuleVariantRef vr in tn.ReverceVariants) 
                {
                    MorphRuleVariant v = this.GetRuleVar(vr.RuleId, vr.VariantId);
                    if (v == null) 
                        continue;
                    if (geo && v.Class.IsProperGeo) 
                    {
                    }
                    else if (!geo && v.Class.IsProperSurname) 
                    {
                    }
                    else 
                        continue;
                    Pullenti.Morph.MorphWordForm r = new Pullenti.Morph.MorphWordForm(v, word, this.GetMiscInfo(v.MiscInfoId));
                    if (!r.HasMorphEquals(res)) 
                    {
                        r.UndefCoef = vr.Coef;
                        res.Add(r);
                    }
                    ok = true;
                }
                if (ok) 
                    break;
            }
        }
        int _compare(Pullenti.Morph.MorphWordForm x, Pullenti.Morph.MorphWordForm y)
        {
            if (x.IsInDictionary && !y.IsInDictionary) 
                return -1;
            if (!x.IsInDictionary && y.IsInDictionary) 
                return 1;
            if (x.UndefCoef > 0) 
            {
                if (x.UndefCoef > (y.UndefCoef * 2)) 
                    return -1;
                if ((x.UndefCoef * 2) < y.UndefCoef) 
                    return 1;
            }
            if (x.Class != y.Class) 
            {
                if ((x.Class.IsPreposition || x.Class.IsConjunction || x.Class.IsPronoun) || x.Class.IsPersonalPronoun) 
                    return -1;
                if ((y.Class.IsPreposition || y.Class.IsConjunction || y.Class.IsPronoun) || y.Class.IsPersonalPronoun) 
                    return 1;
                if (x.Class.IsVerb) 
                    return 1;
                if (y.Class.IsVerb) 
                    return -1;
                if (x.Class.IsNoun) 
                    return -1;
                if (y.Class.IsNoun) 
                    return 1;
            }
            int cx = this._calcCoef(x);
            int cy = this._calcCoef(y);
            if (cx > cy) 
                return -1;
            if (cx < cy) 
                return 1;
            if (x.Number == Pullenti.Morph.MorphNumber.Plural && y.Number != Pullenti.Morph.MorphNumber.Plural) 
                return 1;
            if (y.Number == Pullenti.Morph.MorphNumber.Plural && x.Number != Pullenti.Morph.MorphNumber.Plural) 
                return -1;
            return 0;
        }
        int _calcCoef(Pullenti.Morph.MorphWordForm wf)
        {
            int k = 0;
            if (!wf.Case.IsUndefined) 
                k++;
            if (wf.Gender != Pullenti.Morph.MorphGender.Undefined) 
                k++;
            if (wf.Number != Pullenti.Morph.MorphNumber.Undefined) 
                k++;
            if (wf.Misc.IsSynonymForm) 
                k -= 3;
            if (wf.NormalCase == null || (wf.NormalCase.Length < 4)) 
                return k;
            if (wf.Class.IsAdjective && wf.Number != Pullenti.Morph.MorphNumber.Plural) 
            {
                char last = wf.NormalCase[wf.NormalCase.Length - 1];
                char last1 = wf.NormalCase[wf.NormalCase.Length - 2];
                bool ok = false;
                if (wf.Gender == Pullenti.Morph.MorphGender.Feminie) 
                {
                    if (last == 'Я') 
                        ok = true;
                }
                if (wf.Gender == Pullenti.Morph.MorphGender.Masculine) 
                {
                    if (last == 'Й') 
                    {
                        if (last1 == 'И') 
                            k++;
                        ok = true;
                    }
                }
                if (wf.Gender == Pullenti.Morph.MorphGender.Neuter) 
                {
                    if (last == 'Е') 
                        ok = true;
                }
                if (ok) 
                {
                    if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(last1)) 
                        k++;
                }
            }
            else if (wf.Class.IsAdjective && wf.Number == Pullenti.Morph.MorphNumber.Plural) 
            {
                char last = wf.NormalCase[wf.NormalCase.Length - 1];
                char last1 = wf.NormalCase[wf.NormalCase.Length - 2];
                if (last == 'Й' || last == 'Е') 
                    k++;
            }
            return k;
        }
        int _calcEqCoef(MorphRuleVariant v, Pullenti.Morph.MorphWordForm wf)
        {
            if (wf.Class.Value != 0) 
            {
                if (((v.Class.Value & wf.Class.Value)) == 0) 
                    return -1;
            }
            if (v.MiscInfoId != wf.Misc.Id) 
            {
                Pullenti.Morph.MorphMiscInfo vi = this.GetMiscInfo(v.MiscInfoId);
                if (vi.Mood != Pullenti.Morph.MorphMood.Undefined && wf.Misc.Mood != Pullenti.Morph.MorphMood.Undefined) 
                {
                    if (vi.Mood != wf.Misc.Mood) 
                        return -1;
                }
                if (vi.Tense != Pullenti.Morph.MorphTense.Undefined && wf.Misc.Tense != Pullenti.Morph.MorphTense.Undefined) 
                {
                    if (((vi.Tense & wf.Misc.Tense)) == Pullenti.Morph.MorphTense.Undefined) 
                        return -1;
                }
                if (vi.Voice != Pullenti.Morph.MorphVoice.Undefined && wf.Misc.Voice != Pullenti.Morph.MorphVoice.Undefined) 
                {
                    if (vi.Voice != wf.Misc.Voice) 
                        return -1;
                }
                if (vi.Person != Pullenti.Morph.MorphPerson.Undefined && wf.Misc.Person != Pullenti.Morph.MorphPerson.Undefined) 
                {
                    if (((vi.Person & wf.Misc.Person)) == Pullenti.Morph.MorphPerson.Undefined) 
                        return -1;
                }
                return 0;
            }
            if (!v.CheckAccord(wf, false, false)) 
                return -1;
            return 1;
        }
        void Sort(List<Pullenti.Morph.MorphWordForm> res, string word)
        {
            if (res == null || (res.Count < 2)) 
                return;
            for (int k = 0; k < res.Count; k++) 
            {
                bool ch = false;
                for (int i = 0; i < (res.Count - 1); i++) 
                {
                    int j = this._compare(res[i], res[i + 1]);
                    if (j > 0) 
                    {
                        Pullenti.Morph.MorphWordForm r1 = res[i];
                        Pullenti.Morph.MorphWordForm r2 = res[i + 1];
                        res[i] = r2;
                        res[i + 1] = r1;
                        ch = true;
                    }
                }
                if (!ch) 
                    break;
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                for (int j = i + 1; j < res.Count; j++) 
                {
                    if (this.Comp1(res[i], res[j])) 
                    {
                        if ((res[i].Class.IsAdjective && res[j].Class.IsNoun && !res[j].IsInDictionary) && !res[i].IsInDictionary) 
                            res.RemoveAt(j);
                        else if ((res[i].Class.IsNoun && res[j].Class.IsAdjective && !res[j].IsInDictionary) && !res[i].IsInDictionary) 
                            res.RemoveAt(i);
                        else if (res[i].Class.IsAdjective && res[j].Class.IsPronoun) 
                            res.RemoveAt(i);
                        else if (res[i].Class.IsPronoun && res[j].Class.IsAdjective) 
                        {
                            if (res[j].NormalFull == "ОДИН" || res[j].NormalCase == "ОДИН") 
                                continue;
                            res.RemoveAt(j);
                        }
                        else 
                            continue;
                        i--;
                        break;
                    }
                }
            }
        }
        bool Comp1(Pullenti.Morph.MorphWordForm r1, Pullenti.Morph.MorphWordForm r2)
        {
            if (r1.Number != r2.Number || r1.Gender != r2.Gender) 
                return false;
            if (r1.Case != r2.Case) 
                return false;
            if (r1.NormalCase != r2.NormalCase) 
                return false;
            return true;
        }
        public void Deserialize(Stream str0, bool ignoreRevTree, bool lazyLoad)
        {
            MemoryStream tmp = new MemoryStream();
            MorphDeserializer.DeflateGzip(str0, tmp);
            byte[] arr = tmp.ToArray();
            ByteArrayWrapper buf = new ByteArrayWrapper(arr);
            int pos = 0;
            int cou = buf.DeserializeInt(ref pos);
            for (; cou > 0; cou--) 
            {
                Pullenti.Morph.MorphMiscInfo mi = new Pullenti.Morph.MorphMiscInfo();
                mi.Deserialize(buf, ref pos);
                this.AddMiscInfo(mi);
            }
            cou = buf.DeserializeInt(ref pos);
            for (; cou > 0; cou--) 
            {
                int p1 = buf.DeserializeInt(ref pos);
                MorphRule r = new MorphRule();
                if (lazyLoad) 
                {
                    r.LazyPos = pos;
                    pos = p1;
                }
                else 
                    r.Deserialize(buf, ref pos);
                this.AddRule(r);
            }
            MorphTreeNode root = new MorphTreeNode();
            if (lazyLoad) 
                root.DeserializeLazy(buf, this, ref pos);
            else 
                root.Deserialize(buf, ref pos);
            m_Root = root;
            if (!ignoreRevTree) 
            {
                MorphTreeNode rootRev = new MorphTreeNode();
                if (lazyLoad) 
                    rootRev.DeserializeLazy(buf, this, ref pos);
                else 
                    rootRev.Deserialize(buf, ref pos);
                m_RootReverce = rootRev;
            }
            tmp.Dispose();
            if (lazyLoad) 
                m_LazyBuf = buf;
        }
    }
}