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

namespace Pullenti.Morph.Internal
{
    public class InnerMorphology
    {
        public InnerMorphology()
        {
        }
        MorphEngine m_EngineRu = new MorphEngine();
        MorphEngine m_EngineEn = new MorphEngine();
        MorphEngine m_EngineUa = new MorphEngine();
        MorphEngine m_EngineBy = new MorphEngine();
        MorphEngine m_EngineKz = new MorphEngine();
        object m_Lock = new object();
        internal Pullenti.Morph.MorphLang LoadedLanguages
        {
            get
            {
                return (m_EngineRu.Language | m_EngineEn.Language | m_EngineUa.Language) | m_EngineBy.Language | m_EngineKz.Language;
            }
        }
        internal void LoadLanguages(Pullenti.Morph.MorphLang langs, bool lazyLoad)
        {
            if (langs.IsRu && !m_EngineRu.Language.IsRu) 
            {
                lock (m_Lock) 
                {
                    if (!m_EngineRu.Language.IsRu) 
                    {
                        if (!m_EngineRu.Initialize(Pullenti.Morph.MorphLang.RU, lazyLoad)) 
                            throw new Exception("Not found resource file m_ru.dat in Morphology");
                    }
                }
            }
            if (langs.IsEn && !m_EngineEn.Language.IsEn) 
            {
                lock (m_Lock) 
                {
                    if (!m_EngineEn.Language.IsEn) 
                    {
                        if (!m_EngineEn.Initialize(Pullenti.Morph.MorphLang.EN, lazyLoad)) 
                            throw new Exception("Not found resource file m_en.dat in Morphology");
                    }
                }
            }
            if (langs.IsUa && !m_EngineUa.Language.IsUa) 
            {
                lock (m_Lock) 
                {
                    if (!m_EngineUa.Language.IsUa) 
                        m_EngineUa.Initialize(Pullenti.Morph.MorphLang.UA, lazyLoad);
                }
            }
            if (langs.IsBy && !m_EngineBy.Language.IsBy) 
            {
                lock (m_Lock) 
                {
                    if (!m_EngineBy.Language.IsBy) 
                        m_EngineBy.Initialize(Pullenti.Morph.MorphLang.BY, lazyLoad);
                }
            }
            if (langs.IsKz && !m_EngineKz.Language.IsKz) 
            {
                lock (m_Lock) 
                {
                    if (!m_EngineKz.Language.IsKz) 
                        m_EngineKz.Initialize(Pullenti.Morph.MorphLang.KZ, lazyLoad);
                }
            }
        }
        internal void UnloadLanguages(Pullenti.Morph.MorphLang langs)
        {
            if (langs.IsRu && m_EngineRu.Language.IsRu) 
                m_EngineRu = new MorphEngine();
            if (langs.IsEn && m_EngineEn.Language.IsEn) 
                m_EngineEn = new MorphEngine();
            if (langs.IsUa && m_EngineUa.Language.IsUa) 
                m_EngineUa = new MorphEngine();
            if (langs.IsBy && m_EngineBy.Language.IsBy) 
                m_EngineBy = new MorphEngine();
            if (langs.IsKz && m_EngineKz.Language.IsKz) 
                m_EngineKz = new MorphEngine();
            GC.Collect();
        }
        public void SetEngines(MorphEngine engine)
        {
            if (engine != null) 
            {
                m_EngineRu = engine;
                m_EngineEn = engine;
                m_EngineUa = engine;
                m_EngineBy = engine;
            }
        }
        void OnProgress(int val, int max, ProgressChangedEventHandler progress)
        {
            int p = val;
            if (max > 0xFFFF) 
                p = p / ((max / 100));
            else 
                p = (p * 100) / max;
            if (p != lastPercent && progress != null) 
                progress(null, new ProgressChangedEventArgs((int)p, null)) /* error */;
            lastPercent = p;
        }
        int lastPercent;
        public List<Pullenti.Morph.MorphToken> Run(string text, bool onlyTokenizing, Pullenti.Morph.MorphLang dlang, bool goodText, ProgressChangedEventHandler progress)
        {
            if (string.IsNullOrEmpty(text)) 
                return null;
            TextWrapper twr = new TextWrapper(text, goodText);
            TextWrapper.CharsList twrch = twr.Chars;
            List<Pullenti.Morph.MorphToken> res = new List<Pullenti.Morph.MorphToken>(text.Length / 6);
            Dictionary<string, UniLexWrap> uniLex = new Dictionary<string, UniLexWrap>();
            int i;
            int j;
            string term0 = null;
            int pureRusWords = 0;
            int pureUkrWords = 0;
            int pureByWords = 0;
            int pureKzWords = 0;
            int totRusWords = 0;
            int totUkrWords = 0;
            int totByWords = 0;
            int totKzWords = 0;
            for (i = 0; i < twr.Length; i++) 
            {
                int ty = this.GetCharTyp(twrch[i]);
                if (ty == 0) 
                    continue;
                if (ty > 2) 
                    j = i + 1;
                else 
                    for (j = i + 1; j < twr.Length; j++) 
                    {
                        if (this.GetCharTyp(twrch[j]) != ty) 
                            break;
                    }
                string wstr = text.Substring(i, j - i);
                string term = null;
                if (goodText) 
                    term = wstr;
                else 
                {
                    string trstr = Pullenti.Morph.LanguageHelper.TransliteralCorrection(wstr, term0, false);
                    term = Pullenti.Morph.LanguageHelper.CorrectWord(trstr);
                }
                if (string.IsNullOrEmpty(term)) 
                {
                    i = j - 1;
                    continue;
                }
                Pullenti.Morph.MorphLang lang = Pullenti.Morph.LanguageHelper.GetWordLang(term);
                if (lang == Pullenti.Morph.MorphLang.UA) 
                    pureUkrWords++;
                else if (lang == Pullenti.Morph.MorphLang.RU) 
                    pureRusWords++;
                else if (lang == Pullenti.Morph.MorphLang.BY) 
                    pureByWords++;
                else if (lang == Pullenti.Morph.MorphLang.KZ) 
                    pureKzWords++;
                if (((lang & Pullenti.Morph.MorphLang.RU)) != Pullenti.Morph.MorphLang.Unknown) 
                    totRusWords++;
                if (((lang & Pullenti.Morph.MorphLang.UA)) != Pullenti.Morph.MorphLang.Unknown) 
                    totUkrWords++;
                if (((lang & Pullenti.Morph.MorphLang.BY)) != Pullenti.Morph.MorphLang.Unknown) 
                    totByWords++;
                if (((lang & Pullenti.Morph.MorphLang.KZ)) != Pullenti.Morph.MorphLang.Unknown) 
                    totKzWords++;
                if (ty == 1) 
                    term0 = term;
                UniLexWrap lemmas = null;
                if (ty == 1 && !onlyTokenizing) 
                {
                    if (!uniLex.TryGetValue(term, out lemmas)) 
                    {
                        UniLexWrap nuni = new UniLexWrap(lang);
                        uniLex.Add(term, nuni);
                        lemmas = nuni;
                    }
                }
                Pullenti.Morph.MorphToken tok = new Pullenti.Morph.MorphToken();
                tok.Term = term;
                tok.BeginChar = i;
                if (i == 733860) 
                {
                }
                tok.EndChar = j - 1;
                tok.Tag = lemmas;
                res.Add(tok);
                i = j - 1;
            }
            Pullenti.Morph.MorphLang defLang = new Pullenti.Morph.MorphLang();
            if (dlang != null) 
                defLang.Value = dlang.Value;
            if (pureRusWords > pureUkrWords && pureRusWords > pureByWords && pureRusWords > pureKzWords) 
                defLang = Pullenti.Morph.MorphLang.RU;
            else if (totRusWords > totUkrWords && totRusWords > totByWords && totRusWords > totKzWords) 
                defLang = Pullenti.Morph.MorphLang.RU;
            else if (pureUkrWords > pureRusWords && pureUkrWords > pureByWords && pureUkrWords > pureKzWords) 
                defLang = Pullenti.Morph.MorphLang.UA;
            else if (totUkrWords > totRusWords && totUkrWords > totByWords && totUkrWords > totKzWords) 
                defLang = Pullenti.Morph.MorphLang.UA;
            else if (pureKzWords > pureRusWords && pureKzWords > pureUkrWords && pureKzWords > pureByWords) 
                defLang = Pullenti.Morph.MorphLang.KZ;
            else if (totKzWords > totRusWords && totKzWords > totUkrWords && totKzWords > totByWords) 
                defLang = Pullenti.Morph.MorphLang.KZ;
            else if (pureByWords > pureRusWords && pureByWords > pureUkrWords && pureByWords > pureKzWords) 
                defLang = Pullenti.Morph.MorphLang.BY;
            else if (totByWords > totRusWords && totByWords > totUkrWords && totByWords > totKzWords) 
            {
                if (totRusWords > 10 && totByWords > (totRusWords + 20)) 
                    defLang = Pullenti.Morph.MorphLang.BY;
                else if (totRusWords == 0 || totByWords >= (totRusWords * 2)) 
                    defLang = Pullenti.Morph.MorphLang.BY;
            }
            if (((defLang.IsUndefined || defLang.IsUa)) && totRusWords > 0) 
            {
                if (((totUkrWords > totRusWords && m_EngineUa.Language.IsUa)) || ((totByWords > totRusWords && m_EngineBy.Language.IsBy)) || ((totKzWords > totRusWords && m_EngineKz.Language.IsKz))) 
                {
                    int cou0 = 0;
                    totRusWords = (totByWords = (totUkrWords = (totKzWords = 0)));
                    foreach (KeyValuePair<string, UniLexWrap> kp in uniLex) 
                    {
                        Pullenti.Morph.MorphLang lang = new Pullenti.Morph.MorphLang();
                        kp.Value.WordForms = this.ProcessOneWord(kp.Key, ref lang);
                        if (kp.Value.WordForms != null) 
                        {
                            foreach (Pullenti.Morph.MorphWordForm wf in kp.Value.WordForms) 
                            {
                                lang |= wf.Language;
                            }
                        }
                        kp.Value.Lang = lang;
                        if (lang.IsRu) 
                            totRusWords++;
                        if (lang.IsUa) 
                            totUkrWords++;
                        if (lang.IsBy) 
                            totByWords++;
                        if (lang.IsKz) 
                            totKzWords++;
                        if (lang.IsCyrillic) 
                            cou0++;
                        if (cou0 >= 100) 
                            break;
                    }
                    if (totRusWords > ((totByWords / 2)) && totRusWords > ((totUkrWords / 2))) 
                        defLang = Pullenti.Morph.MorphLang.RU;
                    else if (totUkrWords > ((totRusWords / 2)) && totUkrWords > ((totByWords / 2))) 
                        defLang = Pullenti.Morph.MorphLang.UA;
                    else if (totByWords > ((totRusWords / 2)) && totByWords > ((totUkrWords / 2))) 
                        defLang = Pullenti.Morph.MorphLang.BY;
                }
                else if (defLang.IsUndefined) 
                    defLang = Pullenti.Morph.MorphLang.RU;
            }
            int cou = 0;
            totRusWords = (totByWords = (totUkrWords = (totKzWords = 0)));
            foreach (KeyValuePair<string, UniLexWrap> kp in uniLex) 
            {
                Pullenti.Morph.MorphLang lang = defLang;
                if (lang.IsUndefined) 
                {
                    if (totRusWords > totByWords && totRusWords > totUkrWords && totRusWords > totKzWords) 
                        lang = Pullenti.Morph.MorphLang.RU;
                    else if (totUkrWords > totRusWords && totUkrWords > totByWords && totUkrWords > totKzWords) 
                        lang = Pullenti.Morph.MorphLang.UA;
                    else if (totByWords > totRusWords && totByWords > totUkrWords && totByWords > totKzWords) 
                        lang = Pullenti.Morph.MorphLang.BY;
                    else if (totKzWords > totRusWords && totKzWords > totUkrWords && totKzWords > totByWords) 
                        lang = Pullenti.Morph.MorphLang.KZ;
                }
                kp.Value.WordForms = this.ProcessOneWord(kp.Key, ref lang);
                kp.Value.Lang = lang;
                if (((lang & Pullenti.Morph.MorphLang.RU)) != Pullenti.Morph.MorphLang.Unknown) 
                    totRusWords++;
                if (((lang & Pullenti.Morph.MorphLang.UA)) != Pullenti.Morph.MorphLang.Unknown) 
                    totUkrWords++;
                if (((lang & Pullenti.Morph.MorphLang.BY)) != Pullenti.Morph.MorphLang.Unknown) 
                    totByWords++;
                if (((lang & Pullenti.Morph.MorphLang.KZ)) != Pullenti.Morph.MorphLang.Unknown) 
                    totKzWords++;
                if (progress != null) 
                    this.OnProgress(cou, uniLex.Count, progress);
                cou++;
            }
            List<Pullenti.Morph.MorphWordForm> emptyList = null;
            foreach (Pullenti.Morph.MorphToken r in res) 
            {
                UniLexWrap uni = r.Tag as UniLexWrap;
                r.Tag = null;
                if (uni == null || uni.WordForms == null || uni.WordForms.Count == 0) 
                {
                    if (emptyList == null) 
                        emptyList = new List<Pullenti.Morph.MorphWordForm>();
                    r.WordForms = emptyList;
                    if (uni != null) 
                        r.Language = uni.Lang;
                }
                else 
                    r.WordForms = uni.WordForms;
            }
            if (!goodText) 
            {
                for (i = 0; i < (res.Count - 2); i++) 
                {
                    UnicodeInfo ui0 = twrch[res[i].BeginChar];
                    UnicodeInfo ui1 = twrch[res[i + 1].BeginChar];
                    UnicodeInfo ui2 = twrch[res[i + 2].BeginChar];
                    if (ui1.IsQuot) 
                    {
                        int p = res[i + 1].BeginChar;
                        if ((p >= 2 && "БбТт".IndexOf(text[p - 1]) >= 0 && ((p + 3) < text.Length)) && "ЕеЯяЁё".IndexOf(text[p + 1]) >= 0) 
                        {
                            string wstr = Pullenti.Morph.LanguageHelper.TransliteralCorrection(Pullenti.Morph.LanguageHelper.CorrectWord(string.Format("{0}Ъ{1}", res[i].GetSourceText(text), res[i + 2].GetSourceText(text))), null, false);
                            List<Pullenti.Morph.MorphWordForm> li = this.ProcessOneWord0(wstr);
                            if (li != null && li.Count > 0 && li[0].IsInDictionary) 
                            {
                                res[i].EndChar = res[i + 2].EndChar;
                                res[i].Term = wstr;
                                res[i].WordForms = li;
                                res.RemoveRange(i + 1, 2);
                            }
                        }
                        else if ((ui1.IsApos && p > 0 && char.IsLetter(text[p - 1])) && ((p + 1) < text.Length) && char.IsLetter(text[p + 1])) 
                        {
                            if (defLang == Pullenti.Morph.MorphLang.UA || ((res[i].Language & Pullenti.Morph.MorphLang.UA)) != Pullenti.Morph.MorphLang.Unknown || ((res[i + 2].Language & Pullenti.Morph.MorphLang.UA)) != Pullenti.Morph.MorphLang.Unknown) 
                            {
                                string wstr = Pullenti.Morph.LanguageHelper.TransliteralCorrection(Pullenti.Morph.LanguageHelper.CorrectWord(string.Format("{0}{1}", res[i].GetSourceText(text), res[i + 2].GetSourceText(text))), null, false);
                                List<Pullenti.Morph.MorphWordForm> li = this.ProcessOneWord0(wstr);
                                bool okk = true;
                                if (okk) 
                                {
                                    res[i].EndChar = res[i + 2].EndChar;
                                    res[i].Term = wstr;
                                    if (li == null) 
                                        li = new List<Pullenti.Morph.MorphWordForm>();
                                    if (li != null && li.Count > 0) 
                                        res[i].Language = li[0].Language;
                                    res[i].WordForms = li;
                                    res.RemoveRange(i + 1, 2);
                                }
                            }
                        }
                    }
                    else if (((ui1.UniChar == '3' || ui1.UniChar == '4')) && res[i + 1].Length == 1) 
                    {
                        string src = (ui1.UniChar == '3' ? "З" : "Ч");
                        int i0 = i + 1;
                        if ((res[i].EndChar + 1) == res[i + 1].BeginChar && ui0.IsCyrillic) 
                        {
                            i0--;
                            src = res[i0].GetSourceText(text) + src;
                        }
                        int i1 = i + 1;
                        if ((res[i + 1].EndChar + 1) == res[i + 2].BeginChar && ui2.IsCyrillic) 
                        {
                            i1++;
                            src += res[i1].GetSourceText(text);
                        }
                        if (src.Length > 2) 
                        {
                            string wstr = Pullenti.Morph.LanguageHelper.TransliteralCorrection(Pullenti.Morph.LanguageHelper.CorrectWord(src), null, false);
                            List<Pullenti.Morph.MorphWordForm> li = this.ProcessOneWord0(wstr);
                            if (li != null && li.Count > 0 && li[0].IsInDictionary) 
                            {
                                res[i0].EndChar = res[i1].EndChar;
                                res[i0].Term = wstr;
                                res[i0].WordForms = li;
                                res.RemoveRange(i0 + 1, i1 - i0);
                            }
                        }
                    }
                    else if ((ui1.IsHiphen && ui0.IsLetter && ui2.IsLetter) && res[i].EndChar > res[i].BeginChar && res[i + 2].EndChar > res[i + 2].BeginChar) 
                    {
                        bool newline = false;
                        int sps = 0;
                        for (j = res[i + 1].EndChar + 1; j < res[i + 2].BeginChar; j++) 
                        {
                            if (text[j] == '\r' || text[j] == '\n') 
                            {
                                newline = true;
                                sps++;
                            }
                            else if (!char.IsWhiteSpace(text[j])) 
                                break;
                            else 
                                sps++;
                        }
                        string fullWord = Pullenti.Morph.LanguageHelper.CorrectWord(res[i].GetSourceText(text) + res[i + 2].GetSourceText(text));
                        if (!newline) 
                        {
                            if (uniLex.ContainsKey(fullWord) || fullWord == "ИЗЗА") 
                                newline = true;
                            else if (text[res[i + 1].BeginChar] == ((char)0x00AD)) 
                                newline = true;
                            else if (Pullenti.Morph.LanguageHelper.EndsWithEx(res[i].GetSourceText(text), "О", "о", null, null) && res[i + 2].WordForms.Count > 0 && res[i + 2].WordForms[0].IsInDictionary) 
                            {
                                if (text[res[i + 1].BeginChar] == '¬') 
                                {
                                    List<Pullenti.Morph.MorphWordForm> li = this.ProcessOneWord0(fullWord);
                                    if (li != null && li.Count > 0 && li[0].IsInDictionary) 
                                        newline = true;
                                }
                            }
                            else if ((res[i].EndChar + 2) == res[i + 2].BeginChar) 
                            {
                                if (!char.IsUpper(text[res[i + 2].BeginChar]) && (sps < 2) && fullWord.Length > 4) 
                                {
                                    newline = true;
                                    if ((i + 3) < res.Count) 
                                    {
                                        UnicodeInfo ui3 = twrch[res[i + 3].BeginChar];
                                        if (ui3.IsHiphen) 
                                            newline = false;
                                    }
                                }
                            }
                            else if (((res[i].EndChar + 1) == res[i + 1].BeginChar && sps > 0 && (sps < 3)) && fullWord.Length > 4) 
                                newline = true;
                        }
                        if (newline) 
                        {
                            List<Pullenti.Morph.MorphWordForm> li = this.ProcessOneWord0(fullWord);
                            if (li != null && li.Count > 0 && ((li[0].IsInDictionary || uniLex.ContainsKey(fullWord)))) 
                            {
                                res[i].EndChar = res[i + 2].EndChar;
                                res[i].Term = fullWord;
                                res[i].WordForms = li;
                                res.RemoveRange(i + 1, 2);
                            }
                        }
                        else 
                        {
                        }
                    }
                    else if ((ui1.IsLetter && ui0.IsLetter && res[i].Length > 2) && res[i + 1].Length > 1) 
                    {
                        if (ui0.IsUpper != ui1.IsUpper) 
                            continue;
                        if (!ui0.IsCyrillic || !ui1.IsCyrillic) 
                            continue;
                        bool newline = false;
                        for (j = res[i].EndChar + 1; j < res[i + 1].BeginChar; j++) 
                        {
                            if (twrch[j].Code == 0xD || twrch[j].Code == 0xA) 
                            {
                                newline = true;
                                break;
                            }
                        }
                        if (!newline) 
                            continue;
                        string fullWord = Pullenti.Morph.LanguageHelper.CorrectWord(res[i].GetSourceText(text) + res[i + 1].GetSourceText(text));
                        if (!uniLex.ContainsKey(fullWord)) 
                            continue;
                        List<Pullenti.Morph.MorphWordForm> li = this.ProcessOneWord0(fullWord);
                        if (li != null && li.Count > 0 && li[0].IsInDictionary) 
                        {
                            res[i].EndChar = res[i + 1].EndChar;
                            res[i].Term = fullWord;
                            res[i].WordForms = li;
                            res.RemoveAt(i + 1);
                        }
                    }
                }
            }
            for (i = 0; i < res.Count; i++) 
            {
                Pullenti.Morph.MorphToken mt = res[i];
                mt.CharInfo = new Pullenti.Morph.CharsInfo();
                UnicodeInfo ui0 = twrch[mt.BeginChar];
                UnicodeInfo ui00 = UnicodeInfo.AllChars[(int)(mt.Term[0])];
                for (j = mt.BeginChar + 1; j <= mt.EndChar; j++) 
                {
                    if (ui0.IsLetter) 
                        break;
                    ui0 = twrch[j];
                }
                if (ui0.IsLetter) 
                {
                    mt.CharInfo.IsLetter = true;
                    if (ui00.IsLatin) 
                        mt.CharInfo.IsLatinLetter = true;
                    else if (ui00.IsCyrillic) 
                        mt.CharInfo.IsCyrillicLetter = true;
                    if (mt.Language == Pullenti.Morph.MorphLang.Unknown) 
                    {
                        if (Pullenti.Morph.LanguageHelper.IsCyrillic(mt.Term)) 
                            mt.Language = (defLang.IsUndefined ? Pullenti.Morph.MorphLang.RU : defLang);
                    }
                    if (goodText) 
                        continue;
                    bool allUp = true;
                    bool allLo = true;
                    for (j = mt.BeginChar; j <= mt.EndChar; j++) 
                    {
                        if (twrch[j].IsUpper || twrch[j].IsDigit) 
                            allLo = false;
                        else 
                            allUp = false;
                    }
                    if (allUp) 
                        mt.CharInfo.IsAllUpper = true;
                    else if (allLo) 
                        mt.CharInfo.IsAllLower = true;
                    else if (((ui0.IsUpper || twrch[mt.BeginChar].IsDigit)) && mt.EndChar > mt.BeginChar) 
                    {
                        allLo = true;
                        for (j = mt.BeginChar + 1; j <= mt.EndChar; j++) 
                        {
                            if (twrch[j].IsUpper || twrch[j].IsDigit) 
                            {
                                allLo = false;
                                break;
                            }
                        }
                        if (allLo) 
                            mt.CharInfo.IsCapitalUpper = true;
                        else if (twrch[mt.EndChar].IsLower && (mt.EndChar - mt.BeginChar) > 1) 
                        {
                            allUp = true;
                            for (j = mt.BeginChar; j < mt.EndChar; j++) 
                            {
                                if (twrch[j].IsLower) 
                                {
                                    allUp = false;
                                    break;
                                }
                            }
                            if (allUp) 
                                mt.CharInfo.IsLastLower = true;
                        }
                    }
                }
                if (mt.CharInfo.IsLastLower && mt.Length > 2 && mt.CharInfo.IsCyrillicLetter) 
                {
                    string pref = text.Substring(mt.BeginChar, mt.EndChar - mt.BeginChar);
                    bool ok = false;
                    foreach (Pullenti.Morph.MorphWordForm wf in mt.WordForms) 
                    {
                        if (wf.NormalCase == pref || wf.NormalFull == pref) 
                        {
                            ok = true;
                            break;
                        }
                    }
                    if (!ok) 
                    {
                        Pullenti.Morph.MorphWordForm wf0 = new Pullenti.Morph.MorphWordForm() { NormalCase = pref, Class = Pullenti.Morph.MorphClass.Noun, UndefCoef = 1 };
                        mt.WordForms = new List<Pullenti.Morph.MorphWordForm>(mt.WordForms);
                        mt.WordForms.Insert(0, wf0);
                    }
                }
            }
            if (goodText || onlyTokenizing) 
                return res;
            for (i = 0; i < res.Count; i++) 
            {
                if (res[i].Length == 1 && res[i].CharInfo.IsLatinLetter) 
                {
                    char ch = res[i].Term[0];
                    if (ch == 'C' || ch == 'A' || ch == 'P') 
                    {
                    }
                    else 
                        continue;
                    bool isRus = false;
                    for (int ii = i - 1; ii >= 0; ii--) 
                    {
                        if ((res[ii].EndChar + 1) != res[ii + 1].BeginChar) 
                            break;
                        else if (res[ii].CharInfo.IsLetter) 
                        {
                            isRus = res[ii].CharInfo.IsCyrillicLetter;
                            break;
                        }
                    }
                    if (!isRus) 
                    {
                        for (int ii = i + 1; ii < res.Count; ii++) 
                        {
                            if ((res[ii - 1].EndChar + 1) != res[ii].BeginChar) 
                                break;
                            else if (res[ii].CharInfo.IsLetter) 
                            {
                                isRus = res[ii].CharInfo.IsCyrillicLetter;
                                break;
                            }
                        }
                    }
                    if (isRus) 
                    {
                        res[i].Term = Pullenti.Morph.LanguageHelper.TransliteralCorrection(res[i].Term, null, true);
                        res[i].CharInfo.IsCyrillicLetter = true;
                        res[i].CharInfo.IsLatinLetter = true;
                    }
                }
            }
            foreach (Pullenti.Morph.MorphToken r in res) 
            {
                if (r.CharInfo.IsAllUpper || r.CharInfo.IsCapitalUpper) 
                {
                    if (r.Language.IsCyrillic) 
                    {
                        bool ok = false;
                        foreach (Pullenti.Morph.MorphWordForm wf in r.WordForms) 
                        {
                            if (wf.Class.IsProperSurname) 
                            {
                                ok = true;
                                break;
                            }
                        }
                        if (!ok) 
                        {
                            r.WordForms = new List<Pullenti.Morph.MorphWordForm>(r.WordForms);
                            m_EngineRu.ProcessSurnameVariants(r.Term, r.WordForms);
                        }
                    }
                }
            }
            foreach (Pullenti.Morph.MorphToken r in res) 
            {
                foreach (Pullenti.Morph.MorphWordForm mv in r.WordForms) 
                {
                    if (mv.NormalCase == null) 
                        mv.NormalCase = r.Term;
                }
            }
            for (i = 0; i < (res.Count - 2); i++) 
            {
                if (res[i].CharInfo.IsLatinLetter && res[i].CharInfo.IsAllUpper && res[i].Length == 1) 
                {
                    if (twrch[res[i + 1].BeginChar].IsQuot && res[i + 2].CharInfo.IsLatinLetter && res[i + 2].Length > 2) 
                    {
                        if ((res[i].EndChar + 1) == res[i + 1].BeginChar && (res[i + 1].EndChar + 1) == res[i + 2].BeginChar) 
                        {
                            string wstr = string.Format("{0}{1}", res[i].Term, res[i + 2].Term);
                            List<Pullenti.Morph.MorphWordForm> li = this.ProcessOneWord0(wstr);
                            if (li != null) 
                                res[i].WordForms = li;
                            res[i].EndChar = res[i + 2].EndChar;
                            res[i].Term = wstr;
                            if (res[i + 2].CharInfo.IsAllLower) 
                            {
                                res[i].CharInfo.IsAllUpper = false;
                                res[i].CharInfo.IsCapitalUpper = true;
                            }
                            else if (!res[i + 2].CharInfo.IsAllUpper) 
                                res[i].CharInfo.IsAllUpper = false;
                            res.RemoveRange(i + 1, 2);
                        }
                    }
                }
            }
            for (i = 0; i < (res.Count - 1); i++) 
            {
                if (!res[i].CharInfo.IsLetter && !res[i + 1].CharInfo.IsLetter && (res[i].EndChar + 1) == res[i + 1].BeginChar) 
                {
                    if (twrch[res[i].BeginChar].IsHiphen && twrch[res[i + 1].BeginChar].IsHiphen) 
                    {
                        if (i == 0 || !twrch[res[i - 1].BeginChar].IsHiphen) 
                        {
                        }
                        else 
                            continue;
                        if ((i + 2) == res.Count || !twrch[res[i + 2].BeginChar].IsHiphen) 
                        {
                        }
                        else 
                            continue;
                        res[i].EndChar = res[i + 1].EndChar;
                        res.RemoveAt(i + 1);
                    }
                }
            }
            return res;
        }
        internal int GetCharTyp(UnicodeInfo ui)
        {
            if (ui.IsLetter) 
                return 1;
            if (ui.IsDigit) 
                return 2;
            if (ui.IsWhitespace) 
                return 0;
            if (ui.IsUdaren) 
                return 1;
            return ui.Code;
        }
        public List<Pullenti.Morph.MorphWordForm> GetAllWordforms(string word, Pullenti.Morph.MorphLang lang)
        {
            if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(word[0])) 
            {
                if (lang != null) 
                {
                    if (m_EngineRu.Language.IsRu && lang.IsRu) 
                        return m_EngineRu.GetAllWordforms(word);
                    if (m_EngineUa.Language.IsUa && lang.IsUa) 
                        return m_EngineUa.GetAllWordforms(word);
                    if (m_EngineBy.Language.IsBy && lang.IsBy) 
                        return m_EngineBy.GetAllWordforms(word);
                    if (m_EngineKz.Language.IsKz && lang.IsKz) 
                        return m_EngineKz.GetAllWordforms(word);
                }
                return m_EngineRu.GetAllWordforms(word);
            }
            else 
                return m_EngineEn.GetAllWordforms(word);
        }
        public string GetWordform(string word, Pullenti.Morph.MorphClass cla, Pullenti.Morph.MorphGender gender, Pullenti.Morph.MorphCase cas, Pullenti.Morph.MorphNumber num, Pullenti.Morph.MorphLang lang, Pullenti.Morph.MorphWordForm addInfo)
        {
            if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(word[0])) 
            {
                if (m_EngineRu.Language.IsRu && lang.IsRu) 
                    return m_EngineRu.GetWordform(word, cla, gender, cas, num, addInfo);
                if (m_EngineUa.Language.IsUa && lang.IsUa) 
                    return m_EngineUa.GetWordform(word, cla, gender, cas, num, addInfo);
                if (m_EngineBy.Language.IsBy && lang.IsBy) 
                    return m_EngineBy.GetWordform(word, cla, gender, cas, num, addInfo);
                if (m_EngineKz.Language.IsKz && lang.IsKz) 
                    return m_EngineKz.GetWordform(word, cla, gender, cas, num, addInfo);
                return m_EngineRu.GetWordform(word, cla, gender, cas, num, addInfo);
            }
            else 
                return m_EngineEn.GetWordform(word, cla, gender, cas, num, addInfo);
        }
        public string CorrectWordByMorph(string word, Pullenti.Morph.MorphLang lang)
        {
            if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(word[0])) 
            {
                if (lang != null) 
                {
                    if (m_EngineRu.Language.IsRu && lang.IsRu) 
                        return m_EngineRu.CorrectWordByMorph(word);
                    if (m_EngineUa.Language.IsUa && lang.IsUa) 
                        return m_EngineUa.CorrectWordByMorph(word);
                    if (m_EngineBy.Language.IsBy && lang.IsBy) 
                        return m_EngineBy.CorrectWordByMorph(word);
                    if (m_EngineKz.Language.IsKz && lang.IsKz) 
                        return m_EngineKz.CorrectWordByMorph(word);
                }
                return m_EngineRu.CorrectWordByMorph(word);
            }
            else 
                return m_EngineEn.CorrectWordByMorph(word);
        }
        List<Pullenti.Morph.MorphWordForm> ProcessOneWord0(string wstr)
        {
            Pullenti.Morph.MorphLang dl = new Pullenti.Morph.MorphLang();
            return this.ProcessOneWord(wstr, ref dl);
        }
        List<Pullenti.Morph.MorphWordForm> ProcessOneWord(string wstr, ref Pullenti.Morph.MorphLang defLang)
        {
            Pullenti.Morph.MorphLang lang = Pullenti.Morph.LanguageHelper.GetWordLang(wstr);
            if (lang == Pullenti.Morph.MorphLang.Unknown) 
            {
                defLang = new Pullenti.Morph.MorphLang();
                return null;
            }
            if (lang == Pullenti.Morph.MorphLang.EN) 
                return m_EngineEn.Process(wstr);
            if (defLang == Pullenti.Morph.MorphLang.RU) 
            {
                if (lang.IsRu) 
                    return m_EngineRu.Process(wstr);
            }
            if (lang == Pullenti.Morph.MorphLang.RU) 
            {
                defLang = lang;
                return m_EngineRu.Process(wstr);
            }
            if (defLang == Pullenti.Morph.MorphLang.UA) 
            {
                if (lang.IsUa) 
                    return m_EngineUa.Process(wstr);
            }
            if (lang == Pullenti.Morph.MorphLang.UA) 
            {
                defLang = lang;
                return m_EngineUa.Process(wstr);
            }
            if (defLang == Pullenti.Morph.MorphLang.BY) 
            {
                if (lang.IsBy) 
                    return m_EngineBy.Process(wstr);
            }
            if (lang == Pullenti.Morph.MorphLang.BY) 
            {
                defLang = lang;
                return m_EngineBy.Process(wstr);
            }
            if (defLang == Pullenti.Morph.MorphLang.KZ) 
            {
                if (lang.IsKz) 
                    return m_EngineKz.Process(wstr);
            }
            if (lang == Pullenti.Morph.MorphLang.KZ) 
            {
                defLang = lang;
                return m_EngineKz.Process(wstr);
            }
            List<Pullenti.Morph.MorphWordForm> ru = null;
            if (lang.IsRu) 
                ru = m_EngineRu.Process(wstr);
            List<Pullenti.Morph.MorphWordForm> ua = null;
            if (lang.IsUa) 
                ua = m_EngineUa.Process(wstr);
            List<Pullenti.Morph.MorphWordForm> by = null;
            if (lang.IsBy) 
                by = m_EngineBy.Process(wstr);
            List<Pullenti.Morph.MorphWordForm> kz = null;
            if (lang.IsKz) 
                kz = m_EngineKz.Process(wstr);
            bool hasRu = false;
            bool hasUa = false;
            bool hasBy = false;
            bool hasKz = false;
            if (ru != null) 
            {
                foreach (Pullenti.Morph.MorphWordForm wf in ru) 
                {
                    if (wf.IsInDictionary) 
                        hasRu = true;
                }
            }
            if (ua != null) 
            {
                foreach (Pullenti.Morph.MorphWordForm wf in ua) 
                {
                    if (wf.IsInDictionary) 
                        hasUa = true;
                }
            }
            if (by != null) 
            {
                foreach (Pullenti.Morph.MorphWordForm wf in by) 
                {
                    if (wf.IsInDictionary) 
                        hasBy = true;
                }
            }
            if (kz != null) 
            {
                foreach (Pullenti.Morph.MorphWordForm wf in kz) 
                {
                    if (wf.IsInDictionary) 
                        hasKz = true;
                }
            }
            if ((hasRu && !hasUa && !hasBy) && !hasKz) 
            {
                defLang = Pullenti.Morph.MorphLang.RU;
                return ru;
            }
            if ((hasUa && !hasRu && !hasBy) && !hasKz) 
            {
                defLang = Pullenti.Morph.MorphLang.UA;
                return ua;
            }
            if ((hasBy && !hasRu && !hasUa) && !hasKz) 
            {
                defLang = Pullenti.Morph.MorphLang.BY;
                return by;
            }
            if ((hasKz && !hasRu && !hasUa) && !hasBy) 
            {
                defLang = Pullenti.Morph.MorphLang.KZ;
                return kz;
            }
            if ((ru == null && ua == null && by == null) && kz == null) 
                return null;
            if ((ru != null && ua == null && by == null) && kz == null) 
                return ru;
            if ((ua != null && ru == null && by == null) && kz == null) 
                return ua;
            if ((by != null && ru == null && ua == null) && kz == null) 
                return by;
            if ((kz != null && ru == null && ua == null) && by == null) 
                return kz;
            List<Pullenti.Morph.MorphWordForm> res = new List<Pullenti.Morph.MorphWordForm>();
            if (ru != null) 
            {
                lang |= Pullenti.Morph.MorphLang.RU;
                res.AddRange(ru);
            }
            if (ua != null) 
            {
                lang |= Pullenti.Morph.MorphLang.UA;
                res.AddRange(ua);
            }
            if (by != null) 
            {
                lang |= Pullenti.Morph.MorphLang.BY;
                res.AddRange(by);
            }
            if (kz != null) 
            {
                lang |= Pullenti.Morph.MorphLang.KZ;
                res.AddRange(kz);
            }
            return res;
        }
    }
}