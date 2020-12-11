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
using System.IO;
using System.Text;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Внутренний аналитический контейнер данных. Создаётся автоматически внутри при вызове Processor.Process(...). 
    /// Все токены Token ссылаются через поле Kit на экземпляр контейнера, связанного с обрабатываемым текстом.
    /// </summary>
    public class AnalysisKit
    {
        public AnalysisKit(Pullenti.Ner.SourceOfAnalysis sofa = null, bool onlyTokenizing = false, Pullenti.Morph.MorphLang lang = null, ProgressChangedEventHandler progress = null)
        {
            if (sofa == null) 
                return;
            m_Sofa = sofa;
            StartDate = DateTime.Now;
            List<Pullenti.Morph.MorphToken> tokens = Pullenti.Morph.MorphologyService.Process(sofa.Text, lang, progress);
            Pullenti.Ner.Token t0 = null;
            if (tokens != null) 
            {
                for (int ii = 0; ii < tokens.Count; ii++) 
                {
                    Pullenti.Morph.MorphToken mt = tokens[ii];
                    if (mt.BeginChar == 733860) 
                    {
                    }
                    Pullenti.Ner.TextToken tt = new Pullenti.Ner.TextToken(mt, this);
                    if (sofa.CorrectionDict != null) 
                    {
                        string corw;
                        if (sofa.CorrectionDict.TryGetValue(mt.Term, out corw)) 
                        {
                            List<Pullenti.Morph.MorphToken> ccc = Pullenti.Morph.MorphologyService.Process(corw, lang, null);
                            if (ccc != null && ccc.Count == 1) 
                            {
                                Pullenti.Ner.TextToken tt1 = new Pullenti.Ner.TextToken(ccc[0], this, tt.BeginChar, tt.EndChar) { Term0 = tt.Term };
                                tt1.Chars = tt.Chars;
                                tt = tt1;
                                if (CorrectedTokens == null) 
                                    CorrectedTokens = new Dictionary<Pullenti.Ner.Token, string>();
                                CorrectedTokens.Add(tt, tt.GetSourceText());
                            }
                        }
                    }
                    if (t0 == null) 
                        FirstToken = tt;
                    else 
                        t0.Next = tt;
                    t0 = tt;
                }
            }
            if (sofa.ClearDust) 
                this.ClearDust();
            if (sofa.DoWordsMergingByMorph) 
                this.CorrectWordsByMerging(lang);
            if (sofa.DoWordCorrectionByMorph) 
                this.CorrectWordsByMorph(lang);
            this.MergeLetters();
            this.DefineBaseLanguage();
            if (sofa.CreateNumberTokens) 
            {
                for (Pullenti.Ner.Token t = FirstToken; t != null; t = t.Next) 
                {
                    Pullenti.Ner.NumberToken nt = NumberHelper.TryParseNumber(t);
                    if (nt == null) 
                        continue;
                    this.EmbedToken(nt);
                    t = nt;
                }
            }
            if (onlyTokenizing) 
                return;
            for (Pullenti.Ner.Token t = FirstToken; t != null; t = t.Next) 
            {
                if (t.Morph.Class.IsPreposition) 
                    continue;
                Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                if (mc.IsUndefined && t.Chars.IsCyrillicLetter && t.LengthChar > 4) 
                {
                    string tail = sofa.Text.Substring(t.EndChar - 1, 2);
                    Pullenti.Ner.Token tte = null;
                    Pullenti.Ner.Token tt = t.Previous;
                    if (tt != null && ((tt.IsCommaAnd || tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction))) 
                        tt = tt.Previous;
                    if ((tt != null && !tt.GetMorphClassInDictionary().IsUndefined && ((tt.Morph.Class.Value & t.Morph.Class.Value)) != 0) && tt.LengthChar > 4) 
                    {
                        string tail2 = sofa.Text.Substring(tt.EndChar - 1, 2);
                        if (tail2 == tail) 
                            tte = tt;
                    }
                    if (tte == null) 
                    {
                        tt = t.Next;
                        if (tt != null && ((tt.IsCommaAnd || tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction))) 
                            tt = tt.Next;
                        if ((tt != null && !tt.GetMorphClassInDictionary().IsUndefined && ((tt.Morph.Class.Value & t.Morph.Class.Value)) != 0) && tt.LengthChar > 4) 
                        {
                            string tail2 = sofa.Text.Substring(tt.EndChar - 1, 2);
                            if (tail2 == tail) 
                                tte = tt;
                        }
                    }
                    if (tte != null) 
                        t.Morph.RemoveItemsEx(tte.Morph, tte.GetMorphClassInDictionary());
                }
                continue;
            }
            this.CreateStatistics();
        }
        internal void InitFrom(Pullenti.Ner.AnalysisResult ar)
        {
            m_Sofa = ar.Sofa;
            FirstToken = ar.FirstToken;
            BaseLanguage = ar.BaseLanguage;
            this.CreateStatistics();
        }
        internal DateTime StartDate;
        /// <summary>
        /// Токены, подправленные по корректировочному словарю (SourceOfAnalysis.CorrectionDict). 
        /// Здесь Value - исходый токен
        /// </summary>
        public Dictionary<Pullenti.Ner.Token, string> CorrectedTokens = null;
        void ClearDust()
        {
            for (Pullenti.Ner.Token t = FirstToken; t != null; t = t.Next) 
            {
                int cou = CalcAbnormalCoef(t);
                int norm = 0;
                if (cou < 1) 
                    continue;
                Pullenti.Ner.Token t1 = t;
                for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
                {
                    int co = CalcAbnormalCoef(tt);
                    if (co == 0) 
                        continue;
                    if (co < 0) 
                    {
                        norm++;
                        if (norm > 1) 
                            break;
                    }
                    else 
                    {
                        norm = 0;
                        cou += co;
                        t1 = tt;
                    }
                }
                int len = t1.EndChar - t.BeginChar;
                if (cou > 20 && len > 500) 
                {
                    for (int p = t.BeginChar; p < t1.EndChar; p++) 
                    {
                        if (Sofa.Text[p] == Sofa.Text[p + 1]) 
                            len--;
                    }
                    if (len > 500) 
                    {
                        if (t.Previous != null) 
                            t.Previous.Next = t1.Next;
                        else 
                            FirstToken = t1.Next;
                        t = t1;
                    }
                    else 
                        t = t1;
                }
                else 
                    t = t1;
            }
        }
        static int CalcAbnormalCoef(Pullenti.Ner.Token t)
        {
            if (t is Pullenti.Ner.NumberToken) 
                return 0;
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return 0;
            if (!tt.Chars.IsLetter) 
                return 0;
            if (!tt.Chars.IsLatinLetter && !tt.Chars.IsCyrillicLetter) 
                return 2;
            if (tt.LengthChar < 4) 
                return 0;
            foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
            {
                if ((wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                    return -1;
            }
            if (tt.LengthChar > 15) 
                return 2;
            return 1;
        }
        void CorrectWordsByMerging(Pullenti.Morph.MorphLang lang)
        {
            for (Pullenti.Ner.Token t = FirstToken; t != null && t.Next != null; t = t.Next) 
            {
                if (!t.Chars.IsLetter || (t.LengthChar < 2)) 
                    continue;
                Pullenti.Morph.MorphClass mc0 = t.GetMorphClassInDictionary();
                if (t.Morph.ContainsAttr("прдктв.", null)) 
                    continue;
                Pullenti.Ner.Token t1 = t.Next;
                if (t1.IsHiphen && t1.Next != null && !t1.IsNewlineAfter) 
                    t1 = t1.Next;
                if (t1.LengthChar == 1) 
                    continue;
                if (!t1.Chars.IsLetter || !t.Chars.IsLetter || t1.Chars.IsLatinLetter != t.Chars.IsLatinLetter) 
                    continue;
                if (t1.Chars.IsAllUpper && !t.Chars.IsAllUpper) 
                    continue;
                else if (!t1.Chars.IsAllLower) 
                    continue;
                else if (t.Chars.IsAllUpper) 
                    continue;
                if (t1.Morph.ContainsAttr("прдктв.", null)) 
                    continue;
                Pullenti.Morph.MorphClass mc1 = t1.GetMorphClassInDictionary();
                if (!mc1.IsUndefined && !mc0.IsUndefined) 
                    continue;
                if (((t as Pullenti.Ner.TextToken).Term.Length + (t1 as Pullenti.Ner.TextToken).Term.Length) < 6) 
                    continue;
                string corw = (t as Pullenti.Ner.TextToken).Term + (t1 as Pullenti.Ner.TextToken).Term;
                List<Pullenti.Morph.MorphToken> ccc = Pullenti.Morph.MorphologyService.Process(corw, lang, null);
                if (ccc == null || ccc.Count != 1) 
                    continue;
                if (corw == "ПОСТ" || corw == "ВРЕД") 
                    continue;
                Pullenti.Ner.TextToken tt = new Pullenti.Ner.TextToken(ccc[0], this, t.BeginChar, t1.EndChar);
                if (tt.GetMorphClassInDictionary().IsUndefined) 
                    continue;
                tt.Chars = t.Chars;
                if (t == FirstToken) 
                    FirstToken = tt;
                else 
                    t.Previous.Next = tt;
                if (t1.Next != null) 
                    tt.Next = t1.Next;
                t = tt;
            }
        }
        void CorrectWordsByMorph(Pullenti.Morph.MorphLang lang)
        {
            for (Pullenti.Ner.Token tt = FirstToken; tt != null; tt = tt.Next) 
            {
                if (!(tt is Pullenti.Ner.TextToken)) 
                    continue;
                if (tt.Morph.ContainsAttr("прдктв.", null)) 
                    continue;
                Pullenti.Morph.MorphClass dd = tt.GetMorphClassInDictionary();
                if (!dd.IsUndefined || (tt.LengthChar < 4)) 
                    continue;
                if (tt.Morph.Class.IsProperSurname && !tt.Chars.IsAllLower) 
                    continue;
                if (tt.Chars.IsAllUpper) 
                    continue;
                string corw = Pullenti.Morph.MorphologyService.CorrectWord((tt as Pullenti.Ner.TextToken).Term, (tt.Morph.Language.IsUndefined ? lang : tt.Morph.Language));
                if (corw == null) 
                    continue;
                List<Pullenti.Morph.MorphToken> ccc = Pullenti.Morph.MorphologyService.Process(corw, lang, null);
                if (ccc == null || ccc.Count != 1) 
                    continue;
                Pullenti.Ner.TextToken tt1 = new Pullenti.Ner.TextToken(ccc[0], this, tt.BeginChar, tt.EndChar) { Chars = tt.Chars, Term0 = (tt as Pullenti.Ner.TextToken).Term };
                Pullenti.Morph.MorphClass mc = tt1.GetMorphClassInDictionary();
                if (mc.IsProperSurname) 
                    continue;
                if (tt == FirstToken) 
                    FirstToken = tt1;
                else 
                    tt.Previous.Next = tt1;
                tt1.Next = tt.Next;
                tt = tt1;
                if (CorrectedTokens == null) 
                    CorrectedTokens = new Dictionary<Pullenti.Ner.Token, string>();
                CorrectedTokens.Add(tt, tt.GetSourceText());
            }
        }
        void MergeLetters()
        {
            bool beforeWord = false;
            StringBuilder tmp = new StringBuilder();
            for (Pullenti.Ner.Token t = FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (!tt.Chars.IsLetter || tt.LengthChar != 1) 
                {
                    beforeWord = false;
                    continue;
                }
                int i = t.WhitespacesBeforeCount;
                if (i > 2 || ((i == 2 && beforeWord))) 
                {
                }
                else 
                {
                    beforeWord = false;
                    continue;
                }
                i = 0;
                Pullenti.Ner.Token t1;
                tmp.Length = 0;
                tmp.Append(tt.GetSourceText());
                for (t1 = t; t1.Next != null; t1 = t1.Next) 
                {
                    tt = t1.Next as Pullenti.Ner.TextToken;
                    if (tt.LengthChar != 1 || tt.WhitespacesBeforeCount != 1) 
                        break;
                    i++;
                    tmp.Append(tt.GetSourceText());
                }
                if (i > 3 || ((i > 1 && beforeWord))) 
                {
                }
                else 
                {
                    beforeWord = false;
                    continue;
                }
                beforeWord = false;
                List<Pullenti.Morph.MorphToken> mt = Pullenti.Morph.MorphologyService.Process(tmp.ToString(), null, null);
                if (mt == null || mt.Count != 1) 
                {
                    t = t1;
                    continue;
                }
                foreach (Pullenti.Morph.MorphWordForm wf in mt[0].WordForms) 
                {
                    if (wf.IsInDictionary) 
                    {
                        beforeWord = true;
                        break;
                    }
                }
                if (!beforeWord) 
                {
                    t = t1;
                    continue;
                }
                tt = new Pullenti.Ner.TextToken(mt[0], this, t.BeginChar, t1.EndChar);
                if (t == FirstToken) 
                    FirstToken = tt;
                else 
                    tt.Previous = t.Previous;
                tt.Next = t1.Next;
                t = tt;
            }
        }
        /// <summary>
        /// Встроить токен в основную цепочку токенов
        /// </summary>
        /// <param name="mt">встраиваемый метатокен</param>
        public void EmbedToken(Pullenti.Ner.MetaToken mt)
        {
            if (mt == null) 
                return;
            if (mt.BeginChar > mt.EndChar) 
            {
                Pullenti.Ner.Token bg = mt.BeginToken;
                mt.BeginToken = mt.EndToken;
                mt.EndToken = bg;
            }
            if (mt.BeginChar > mt.EndChar) 
                return;
            if (mt.BeginToken == FirstToken) 
                FirstToken = mt;
            else 
            {
                Pullenti.Ner.Token tp = mt.BeginToken.Previous;
                mt.Previous = tp;
            }
            Pullenti.Ner.Token tn = mt.EndToken.Next;
            mt.Next = tn;
            if (mt is Pullenti.Ner.ReferentToken) 
            {
                if ((mt as Pullenti.Ner.ReferentToken).Referent != null) 
                    (mt as Pullenti.Ner.ReferentToken).Referent.AddOccurence(new Pullenti.Ner.TextAnnotation() { Sofa = Sofa, BeginChar = mt.BeginChar, EndChar = mt.EndChar });
            }
        }
        /// <summary>
        /// Убрать метатокен из цепочки, восстановив исходное
        /// </summary>
        /// <param name="t">удаляемый из цепочки метатокен</param>
        /// <return>первый токен удалённого метатокена</return>
        public Pullenti.Ner.Token DebedToken(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.Referent r = t.GetReferent();
            if (r != null) 
            {
                foreach (Pullenti.Ner.TextAnnotation o in r.Occurrence) 
                {
                    if (o.BeginChar == t.BeginChar && o.EndChar == t.EndChar) 
                    {
                        r.Occurrence.Remove(o);
                        break;
                    }
                }
            }
            Pullenti.Ner.MetaToken mt = t as Pullenti.Ner.MetaToken;
            if (mt == null) 
                return t;
            if (t.Next != null) 
                t.Next.Previous = mt.EndToken;
            if (t.Previous != null) 
                t.Previous.Next = mt.BeginToken;
            if (mt == FirstToken) 
                FirstToken = mt.BeginToken;
            if (r != null && r.Occurrence.Count == 0) 
            {
                foreach (AnalyzerData d in m_Datas.Values) 
                {
                    if (d.Referents.Contains(r)) 
                    {
                        d.RemoveReferent(r);
                        break;
                    }
                }
            }
            return mt.BeginToken;
        }
        /// <summary>
        /// Это начало цепочки токенов (первый токен)
        /// </summary>
        public Pullenti.Ner.Token FirstToken;
        /// <summary>
        /// Список сущностей Referent, выделенных в ходе анализа
        /// </summary>
        public List<Pullenti.Ner.Referent> Entities
        {
            get
            {
                return m_Entities;
            }
        }
        List<Pullenti.Ner.Referent> m_Entities = new List<Pullenti.Ner.Referent>();
        /// <summary>
        /// Внешняя онтология - параметр Processor.Process(, ...)
        /// </summary>
        public Pullenti.Ner.ExtOntology Ontology;
        /// <summary>
        /// Базовый язык (определяется по тексту)
        /// </summary>
        public Pullenti.Morph.MorphLang BaseLanguage = new Pullenti.Morph.MorphLang();
        /// <summary>
        /// Ссылка на исходный текст
        /// </summary>
        public Pullenti.Ner.SourceOfAnalysis Sofa
        {
            get
            {
                if (m_Sofa == null) 
                    m_Sofa = new Pullenti.Ner.SourceOfAnalysis("");
                return m_Sofa;
            }
        }
        Pullenti.Ner.SourceOfAnalysis m_Sofa;
        /// <summary>
        /// Статистическая информация
        /// </summary>
        public StatisticCollection Statistics;
        /// <summary>
        /// Получить символ из исходного текста
        /// </summary>
        /// <param name="position">позиция</param>
        /// <return>символ (0, если выход за границу)</return>
        public char GetTextCharacter(int position)
        {
            if ((position < 0) || position >= m_Sofa.Text.Length) 
                return (char)0;
            return m_Sofa.Text[position];
        }
        /// <summary>
        /// Получить данные, полученные в настоящий момент конкретным анализатором
        /// </summary>
        /// <param name="analyzerName">имя анализатора</param>
        /// <return>связанные с ним данные</return>
        public AnalyzerData GetAnalyzerDataByAnalyzerName(string analyzerName)
        {
            Pullenti.Ner.Analyzer a = Processor.FindAnalyzer(analyzerName);
            if (a == null) 
                return null;
            return this.GetAnalyzerData(a);
        }
        // Получить данные, полученные в настоящий момент конкретным анализатором
        public AnalyzerData GetAnalyzerData(Pullenti.Ner.Analyzer analyzer)
        {
            if (analyzer == null || analyzer.Name == null) 
                return null;
            AnalyzerData d;
            if (m_Datas.TryGetValue(analyzer.Name, out d)) 
            {
                d.Kit = this;
                return d;
            }
            AnalyzerData defaultData = analyzer.CreateAnalyzerData();
            if (defaultData == null) 
                return null;
            if (analyzer.PersistReferentsRegim) 
            {
                if (analyzer.PersistAnalizerData == null) 
                    analyzer.PersistAnalizerData = defaultData;
                else 
                    defaultData = analyzer.PersistAnalizerData;
            }
            m_Datas.Add(analyzer.Name, defaultData);
            defaultData.Kit = this;
            return defaultData;
        }
        Dictionary<string, AnalyzerData> m_Datas = new Dictionary<string, AnalyzerData>();
        // Используется анализаторами произвольным образом
        public Dictionary<string, object> MiscData = new Dictionary<string, object>();
        void CreateStatistics()
        {
            Statistics = new StatisticCollection();
            Statistics.Prepare(FirstToken);
        }
        void DefineBaseLanguage()
        {
            Dictionary<short, int> stat = new Dictionary<short, int>();
            int total = 0;
            for (Pullenti.Ner.Token t = FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                    continue;
                if (tt.Morph.Language.IsUndefined) 
                    continue;
                if (!stat.ContainsKey(tt.Morph.Language.Value)) 
                    stat.Add(tt.Morph.Language.Value, 1);
                else 
                    stat[tt.Morph.Language.Value]++;
                total++;
            }
            short val = (short)0;
            foreach (KeyValuePair<short, int> kp in stat) 
            {
                if (kp.Value > (total / 2)) 
                    val |= kp.Key;
            }
            BaseLanguage.Value = val;
        }
        // Заменить везде, где только возможно, старую сущность на новую (используется при объединении сущностей)
        public void ReplaceReferent(Pullenti.Ner.Referent oldReferent, Pullenti.Ner.Referent newReferent)
        {
            for (Pullenti.Ner.Token t = FirstToken; t != null; t = t.Next) 
            {
                if (t is Pullenti.Ner.ReferentToken) 
                    (t as Pullenti.Ner.ReferentToken).ReplaceReferent(oldReferent, newReferent);
            }
            foreach (AnalyzerData d in m_Datas.Values) 
            {
                foreach (Pullenti.Ner.Referent r in d.Referents) 
                {
                    foreach (Pullenti.Ner.Slot s in r.Slots) 
                    {
                        if (s.Value == oldReferent) 
                            r.UploadSlot(s, newReferent);
                    }
                }
                if (d.Referents.Contains(oldReferent)) 
                    d.Referents.Remove(oldReferent);
            }
        }
        public Pullenti.Ner.Processor Processor;
        /// <summary>
        /// Попытаться выделить с заданного токена сущность указанным анализатором. 
        /// Используется, если нужно "забежать вперёд" и проверить гипотезу, есть ли тут сущность конкретного типа или нет.
        /// </summary>
        /// <param name="analyzerName">имя анализатора</param>
        /// <param name="t">токен, с которого попробовать выделение</param>
        /// <return>метатокен с сущностью ReferentToken или null. Отметим, что сущность не сохранена и полученный метатокен никуда не встроен.</return>
        public Pullenti.Ner.ReferentToken ProcessReferent(string analyzerName, Pullenti.Ner.Token t)
        {
            if (Processor == null) 
                return null;
            if (m_AnalyzerStack.Contains(analyzerName)) 
                return null;
            if (IsRecurceOverflow) 
                return null;
            Pullenti.Ner.Analyzer a = Processor.FindAnalyzer(analyzerName);
            if (a == null) 
                return null;
            RecurseLevel++;
            m_AnalyzerStack.Add(analyzerName);
            Pullenti.Ner.ReferentToken res = a.ProcessReferent(t, null);
            m_AnalyzerStack.Remove(analyzerName);
            RecurseLevel--;
            return res;
        }
        /// <summary>
        /// Создать экземпляр сущности заданного типа
        /// </summary>
        /// <param name="typeName">имя типа сущности</param>
        /// <return>экземпляр класса, наследного от Referent, или null</return>
        public Pullenti.Ner.Referent CreateReferent(string typeName)
        {
            if (Processor == null) 
                return null;
            else 
                foreach (Pullenti.Ner.Analyzer a in Processor.Analyzers) 
                {
                    Pullenti.Ner.Referent res = a.CreateReferent(typeName);
                    if (res != null) 
                        return res;
                }
            return null;
        }
        public void RefreshGenerals()
        {
            Pullenti.Ner.Core.Internal.GeneralRelationHelper.RefreshGenerals(Processor, this);
        }
        public int RecurseLevel = 0;
        public bool IsRecurceOverflow
        {
            get
            {
                return RecurseLevel > 5;
            }
        }
        // Используется для предотвращения большого числа рекурсий
        internal List<string> m_AnalyzerStack = new List<string>();
        // Используется внутренним образом
        public bool OntoRegime = false;
        public void Serialize(Stream stream)
        {
            stream.WriteByte((byte)0xAA);
            stream.WriteByte((byte)1);
            m_Sofa.Serialize(stream);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, (int)BaseLanguage.Value);
            if (m_Entities.Count == 0) 
            {
                foreach (KeyValuePair<string, AnalyzerData> d in m_Datas) 
                {
                    m_Entities.AddRange(d.Value.Referents);
                }
            }
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, m_Entities.Count);
            for (int i = 0; i < m_Entities.Count; i++) 
            {
                m_Entities[i].Tag = i + 1;
                Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, m_Entities[i].TypeName);
            }
            foreach (Pullenti.Ner.Referent e in m_Entities) 
            {
                e.Serialize(stream);
            }
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeTokens(stream, FirstToken, 0);
        }
        public bool Deserialize(Stream stream)
        {
            int vers = 0;
            byte b = (byte)stream.ReadByte();
            if (b == 0xAA) 
            {
                b = (byte)stream.ReadByte();
                vers = b;
            }
            else 
                stream.Position--;
            m_Sofa = new Pullenti.Ner.SourceOfAnalysis(null);
            m_Sofa.Deserialize(stream);
            BaseLanguage = new Pullenti.Morph.MorphLang() { Value = (short)Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream) };
            m_Entities = new List<Pullenti.Ner.Referent>();
            int cou = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            for (int i = 0; i < cou; i++) 
            {
                string typ = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
                Pullenti.Ner.Referent r = Pullenti.Ner.ProcessorService.CreateReferent(typ);
                if (r == null) 
                    r = new Pullenti.Ner.Referent("UNDEFINED");
                m_Entities.Add(r);
            }
            for (int i = 0; i < cou; i++) 
            {
                m_Entities[i].Deserialize(stream, m_Entities, m_Sofa);
            }
            FirstToken = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeTokens(stream, this, vers);
            this.CreateStatistics();
            return true;
        }
    }
}