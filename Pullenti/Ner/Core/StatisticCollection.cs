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
    /// Статистическая информация о словоформах и их биграммах в тексте - поле AnalysisKit.Statistic.
    /// </summary>
    public class StatisticCollection
    {
        internal void Prepare(Pullenti.Ner.Token first)
        {
            StatisticWordInfo prev = null;
            Pullenti.Ner.Token prevt = null;
            for (Pullenti.Ner.Token t = first; t != null; t = t.Next) 
            {
                if (t.IsHiphen) 
                    continue;
                StatisticWordInfo it = null;
                if (((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && t.LengthChar > 1) && !t.Chars.IsAllLower) 
                    it = this.AddToken(t as Pullenti.Ner.TextToken);
                else if ((((t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).LengthChar == 1 && t.Chars.IsAllUpper) && t.Next != null && t.Next.IsChar('.')) && !t.IsWhitespaceAfter) 
                {
                    it = this.AddToken(t as Pullenti.Ner.TextToken);
                    t = t.Next;
                }
                if (prev != null && it != null) 
                {
                    this.AddBigramm(prev, it);
                    if (prevt.Chars == t.Chars) 
                    {
                        prev.AddAfter(it);
                        it.AddBefore(prev);
                    }
                }
                prev = it;
                prevt = t;
            }
            for (Pullenti.Ner.Token t = first; t != null; t = t.Next) 
            {
                if (t.Chars.IsLetter && (t is Pullenti.Ner.TextToken)) 
                {
                    StatisticWordInfo it = this.FindItem(t as Pullenti.Ner.TextToken, false);
                    if (it != null) 
                    {
                        if (t.Chars.IsAllLower) 
                            it.LowerCount++;
                        else if (t.Chars.IsAllUpper) 
                            it.UpperCount++;
                        else if (t.Chars.IsCapitalUpper) 
                            it.CapitalCount++;
                    }
                }
            }
        }
        StatisticWordInfo AddToken(Pullenti.Ner.TextToken tt)
        {
            List<string> vars = new List<string>();
            vars.Add(tt.Term);
            string s = MiscHelper.GetAbsoluteNormalValue(tt.Term, false);
            if (s != null && !vars.Contains(s)) 
                vars.Add(s);
            foreach (Pullenti.Morph.MorphBaseInfo wff in tt.Morph.Items) 
            {
                Pullenti.Morph.MorphWordForm wf = wff as Pullenti.Morph.MorphWordForm;
                if (wf == null) 
                    continue;
                if (wf.NormalCase != null && !vars.Contains(wf.NormalCase)) 
                    vars.Add(wf.NormalCase);
                if (wf.NormalFull != null && !vars.Contains(wf.NormalFull)) 
                    vars.Add(wf.NormalFull);
            }
            StatisticWordInfo res = null;
            foreach (string v in vars) 
            {
                if (m_Items.TryGetValue(v, out res)) 
                    break;
            }
            if (res == null) 
                res = new StatisticWordInfo() { Normal = tt.Lemma };
            foreach (string v in vars) 
            {
                if (!m_Items.ContainsKey(v)) 
                    m_Items.Add(v, res);
            }
            res.TotalCount++;
            if ((tt.Next is Pullenti.Ner.TextToken) && tt.Next.Chars.IsAllLower) 
            {
                if (tt.Next.Chars.IsCyrillicLetter && tt.Next.GetMorphClassInDictionary().IsVerb) 
                {
                    Pullenti.Morph.MorphGender g = tt.Next.Morph.Gender;
                    if (g == Pullenti.Morph.MorphGender.Feminie) 
                        res.FemaleVerbsAfterCount++;
                    else if (((g & Pullenti.Morph.MorphGender.Masculine)) != Pullenti.Morph.MorphGender.Undefined) 
                        res.MaleVerbsAfterCount++;
                }
            }
            if (tt.Previous != null) 
            {
                if ((tt.Previous is Pullenti.Ner.TextToken) && tt.Previous.Chars.IsLetter && !tt.Previous.Chars.IsAllLower) 
                {
                }
                else 
                    res.NotCapitalBeforeCount++;
            }
            return res;
        }
        Dictionary<string, StatisticWordInfo> m_Items = new Dictionary<string, StatisticWordInfo>();
        StatisticWordInfo FindItem(Pullenti.Ner.TextToken tt, bool doAbsolute = true)
        {
            if (tt == null) 
                return null;
            StatisticWordInfo res;
            if (m_Items.TryGetValue(tt.Term, out res)) 
                return res;
            if (doAbsolute) 
            {
                string s = MiscHelper.GetAbsoluteNormalValue(tt.Term, false);
                if (s != null) 
                {
                    if (m_Items.TryGetValue(s, out res)) 
                        return res;
                }
            }
            foreach (Pullenti.Morph.MorphBaseInfo wff in tt.Morph.Items) 
            {
                Pullenti.Morph.MorphWordForm wf = wff as Pullenti.Morph.MorphWordForm;
                if (wf == null) 
                    continue;
                if (m_Items.TryGetValue(wf.NormalCase ?? "", out res)) 
                    return res;
                if (wf.NormalFull != null && m_Items.TryGetValue(wf.NormalFull, out res)) 
                    return res;
            }
            return null;
        }
        void AddBigramm(StatisticWordInfo b1, StatisticWordInfo b2)
        {
            Dictionary<string, int> di;
            if (!m_Bigramms.TryGetValue(b1.Normal, out di)) 
                m_Bigramms.Add(b1.Normal, (di = new Dictionary<string, int>()));
            if (di.ContainsKey(b2.Normal)) 
                di[b2.Normal]++;
            else 
                di.Add(b2.Normal, 1);
            if (!m_BigrammsRev.TryGetValue(b2.Normal, out di)) 
                m_BigrammsRev.Add(b2.Normal, (di = new Dictionary<string, int>()));
            if (di.ContainsKey(b1.Normal)) 
                di[b1.Normal]++;
            else 
                di.Add(b1.Normal, 1);
        }
        Dictionary<string, Dictionary<string, int>> m_Bigramms = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> m_BigrammsRev = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> m_Initials = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> m_InitialsRev = new Dictionary<string, Dictionary<string, int>>();
        /// <summary>
        /// Получить статистическую информацию о биграмме токенов
        /// </summary>
        /// <param name="t1">первый токен биграммы</param>
        /// <param name="t2">второй токен биграммы</param>
        /// <return>информация о биграмме по всему тексту</return>
        public StatisticBigrammInfo GetBigrammInfo(Pullenti.Ner.Token t1, Pullenti.Ner.Token t2)
        {
            StatisticWordInfo si1 = this.FindItem(t1 as Pullenti.Ner.TextToken, true);
            StatisticWordInfo si2 = this.FindItem(t2 as Pullenti.Ner.TextToken, true);
            if (si1 == null || si2 == null) 
                return null;
            return this._getBigramsInfo(si1, si2);
        }
        StatisticBigrammInfo _getBigramsInfo(StatisticWordInfo si1, StatisticWordInfo si2)
        {
            StatisticBigrammInfo res = new StatisticBigrammInfo() { FirstCount = si1.TotalCount, SecondCount = si2.TotalCount };
            Dictionary<string, int> di12 = null;
            m_Bigramms.TryGetValue(si1.Normal, out di12);
            Dictionary<string, int> di21 = null;
            m_BigrammsRev.TryGetValue(si2.Normal, out di21);
            if (di12 != null) 
            {
                if (!di12.ContainsKey(si2.Normal)) 
                    res.FirstHasOtherSecond = true;
                else 
                {
                    res.PairCount = di12[si2.Normal];
                    if (di12.Count > 1) 
                        res.FirstHasOtherSecond = true;
                }
            }
            if (di21 != null) 
            {
                if (!di21.ContainsKey(si1.Normal)) 
                    res.SecondHasOtherFirst = true;
                else if (!di21.ContainsKey(si1.Normal)) 
                    res.SecondHasOtherFirst = true;
                else if (di21.Count > 1) 
                    res.SecondHasOtherFirst = true;
            }
            return res;
        }
        public StatisticBigrammInfo GetInitialInfo(string ini, Pullenti.Ner.Token sur)
        {
            if (string.IsNullOrEmpty(ini)) 
                return null;
            StatisticWordInfo si2 = this.FindItem(sur as Pullenti.Ner.TextToken, true);
            if (si2 == null) 
                return null;
            StatisticWordInfo si1 = null;
            if (!m_Items.TryGetValue(ini.Substring(0, 1), out si1)) 
                return null;
            if (si1 == null) 
                return null;
            return this._getBigramsInfo(si1, si2);
        }
        /// <summary>
        /// Получить информацию о словоформе токена
        /// </summary>
        /// <param name="t">токен</param>
        /// <return>статистическая информация по тексту</return>
        public StatisticWordInfo GetWordInfo(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return null;
            return this.FindItem(tt, true);
        }
    }
}