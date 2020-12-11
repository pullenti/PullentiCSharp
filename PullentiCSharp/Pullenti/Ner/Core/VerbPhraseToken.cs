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
    /// <summary>
    /// Метатокен - глагольная группа (последовательность глаголов, наречий и причастий). 
    /// Создаётся методом VerbPhraseHelper.TryParse.
    /// </summary>
    public class VerbPhraseToken : Pullenti.Ner.MetaToken
    {
        public VerbPhraseToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        /// <summary>
        /// Элементы глагольной группы - список VerbPhraseItemToken
        /// </summary>
        public List<VerbPhraseItemToken> Items = new List<VerbPhraseItemToken>();
        /// <summary>
        /// Первый глагол (всегда есть, иначе это не группа)
        /// </summary>
        public VerbPhraseItemToken FirstVerb
        {
            get
            {
                foreach (VerbPhraseItemToken it in Items) 
                {
                    if (!it.IsAdverb) 
                        return it;
                }
                return null;
            }
        }
        /// <summary>
        /// Последний глагол (если один, то совпадает с первым)
        /// </summary>
        public VerbPhraseItemToken LastVerb
        {
            get
            {
                for (int i = Items.Count - 1; i >= 0; i--) 
                {
                    if (!Items[i].IsAdverb) 
                        return Items[i];
                }
                return null;
            }
        }
        /// <summary>
        /// Предлог перед (для причастий)
        /// </summary>
        public PrepositionToken Preposition;
        /// <summary>
        /// Признак того, что вся группа в пассивном залоге (по первому глаголу)
        /// </summary>
        public bool IsVerbPassive
        {
            get
            {
                VerbPhraseItemToken fi = FirstVerb;
                if (fi == null || fi.VerbMorph == null) 
                    return false;
                return fi.VerbMorph.Misc.Voice == Pullenti.Morph.MorphVoice.Passive;
            }
        }
        public void MergeWith(VerbPhraseToken v)
        {
            Items.AddRange(v.Items);
            EndToken = v.EndToken;
        }
        public override string ToString()
        {
            if (Items.Count == 1) 
                return string.Format("{0}, {1}", Items[0].ToString(), Morph.ToString());
            StringBuilder tmp = new StringBuilder();
            foreach (VerbPhraseItemToken it in Items) 
            {
                if (tmp.Length > 0) 
                    tmp.Append(' ');
                tmp.Append(it);
            }
            tmp.AppendFormat(", {0}", Morph.ToString());
            return tmp.ToString();
        }
        public override string GetNormalCaseText(Pullenti.Morph.MorphClass mc = null, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined, bool keepChars = false)
        {
            return base.GetNormalCaseText(Pullenti.Morph.MorphClass.Verb, num, gender, keepChars);
        }
    }
}