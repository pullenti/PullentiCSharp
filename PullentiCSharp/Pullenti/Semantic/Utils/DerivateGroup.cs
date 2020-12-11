/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Semantic.Utils
{
    /// <summary>
    /// Дериватная группа - группа, содержащая однокоренные слова разных частей речи и языков, 
    /// а также модель управления (что может идти за словом).
    /// </summary>
    public class DerivateGroup
    {
        /// <summary>
        /// Слова дериватной группы - неупорядоченный список Words
        /// </summary>
        public List<DerivateWord> Words = new List<DerivateWord>();
        public string Prefix;
        public bool IsDummy;
        public bool NotGenerate;
        /// <summary>
        /// Группа сгенерирована на основе перебора приставок (если параметр tryVariants=true у Explanatory.FindDerivates)
        /// </summary>
        public bool IsGenerated;
        /// <summary>
        /// Модель управления (информация для вычисления семантических связей, что может идти за словами этой группы)
        /// </summary>
        public ControlModel Model = new ControlModel();
        public Pullenti.Semantic.Internal.ControlModelOld Cm = new Pullenti.Semantic.Internal.ControlModelOld();
        public Pullenti.Semantic.Internal.ControlModelOld CmRev = new Pullenti.Semantic.Internal.ControlModelOld();
        internal int LazyPos;
        public int Id;
        /// <summary>
        /// Содержит ли группа слово
        /// </summary>
        /// <param name="word">слово в верхнем регистре и нормальной форме</param>
        /// <param name="lang">возможный язык</param>
        /// <return>да-нет</return>
        public bool ContainsWord(string word, Pullenti.Morph.MorphLang lang)
        {
            foreach (DerivateWord w in Words) 
            {
                if (w.Spelling == word) 
                {
                    if (lang == null || lang.IsUndefined || w.Lang == null) 
                        return true;
                    if (!((lang & w.Lang)).IsUndefined) 
                        return true;
                }
            }
            return false;
        }
        public override string ToString()
        {
            string res = "?";
            if (Words.Count > 0) 
                res = string.Format("<{0}>", Words[0].Spelling);
            if (IsDummy) 
                res = string.Format("DUMMY: {0}", res);
            else if (IsGenerated) 
                res = string.Format("GEN: {0}", res);
            return res;
        }
        public DerivateGroup CreateByPrefix(string pref, Pullenti.Morph.MorphLang lang)
        {
            DerivateGroup res = new DerivateGroup() { IsGenerated = true, Prefix = pref };
            foreach (DerivateWord w in Words) 
            {
                if (lang != null && !lang.IsUndefined && ((w.Lang & lang)).IsUndefined) 
                    continue;
                DerivateWord rw = new DerivateWord() { Spelling = pref + w.Spelling, Lang = w.Lang, Class = w.Class, Aspect = w.Aspect, Reflexive = w.Reflexive, Tense = w.Tense, Voice = w.Voice, Attrs = w.Attrs };
                res.Words.Add(rw);
            }
            return res;
        }
        internal void Deserialize(Pullenti.Morph.Internal.ByteArrayWrapper str, ref int pos)
        {
            int attr = str.DeserializeShort(ref pos);
            if (((attr & 1)) != 0) 
                IsDummy = true;
            if (((attr & 2)) != 0) 
                NotGenerate = true;
            Prefix = str.DeserializeString(ref pos);
            Model.Deserialize(str, ref pos);
            Cm.Deserialize(str, ref pos);
            CmRev.Deserialize(str, ref pos);
            int cou = str.DeserializeShort(ref pos);
            for (; cou > 0; cou--) 
            {
                DerivateWord w = new DerivateWord();
                w.Spelling = str.DeserializeString(ref pos);
                int sh = str.DeserializeShort(ref pos);
                w.Class = new Pullenti.Morph.MorphClass();
                w.Class.Value = (short)sh;
                sh = str.DeserializeShort(ref pos);
                w.Lang = new Pullenti.Morph.MorphLang();
                w.Lang.Value = (short)sh;
                sh = str.DeserializeShort(ref pos);
                w.Attrs.Value = (short)sh;
                byte b = str.DeserializeByte(ref pos);
                w.Aspect = (Pullenti.Morph.MorphAspect)b;
                b = str.DeserializeByte(ref pos);
                w.Tense = (Pullenti.Morph.MorphTense)b;
                b = str.DeserializeByte(ref pos);
                w.Voice = (Pullenti.Morph.MorphVoice)b;
                b = str.DeserializeByte(ref pos);
                int cou1 = (int)b;
                for (; cou1 > 0; cou1--) 
                {
                    string n = str.DeserializeString(ref pos);
                    if (w.NextWords == null) 
                        w.NextWords = new List<string>();
                    if (n != null) 
                        w.NextWords.Add(n);
                }
                Words.Add(w);
            }
        }
    }
}