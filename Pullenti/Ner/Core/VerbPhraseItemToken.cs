/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Элемент глагольной группы VerbPhraseToken
    /// </summary>
    public class VerbPhraseItemToken : Pullenti.Ner.MetaToken
    {
        public VerbPhraseItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        /// <summary>
        /// Частица НЕ
        /// </summary>
        public bool Not;
        /// <summary>
        /// Это наречие
        /// </summary>
        public bool IsAdverb;
        /// <summary>
        /// Это причастие
        /// </summary>
        public bool IsParticiple
        {
            get
            {
                if (m_IsParticiple >= 0) 
                    return m_IsParticiple > 0;
                foreach (Pullenti.Morph.MorphBaseInfo f in Morph.Items) 
                {
                    if (f.Class.IsAdjective && (f is Pullenti.Morph.MorphWordForm) && !(f as Pullenti.Morph.MorphWordForm).Misc.Attrs.Contains("к.ф.")) 
                        return true;
                    else if (f.Class.IsVerb && !f.Case.IsUndefined) 
                        return true;
                }
                m_IsParticiple = 0;
                Pullenti.Ner.TextToken tt = EndToken as Pullenti.Ner.TextToken;
                if (tt != null && tt.Term.EndsWith("СЯ")) 
                {
                    Pullenti.Morph.MorphWordForm mb = Pullenti.Morph.MorphologyService.GetWordBaseInfo(tt.Term.Substring(0, tt.Term.Length - 2), null, false, false);
                    if (mb != null) 
                    {
                        if (mb.Class.IsAdjective) 
                            m_IsParticiple = 1;
                    }
                }
                return m_IsParticiple > 0;
            }
            set
            {
                m_IsParticiple = (value ? 1 : 0);
            }
        }
        int m_IsParticiple = -1;
        /// <summary>
        /// Это деепричастие
        /// </summary>
        public bool IsDeeParticiple
        {
            get
            {
                Pullenti.Ner.TextToken tt = EndToken as Pullenti.Ner.TextToken;
                if (tt == null) 
                    return false;
                if (!tt.Term.EndsWith("Я") && !tt.Term.EndsWith("В")) 
                    return false;
                if (tt.Morph.Class.IsVerb && !tt.Morph.Class.IsAdjective) 
                {
                    if (tt.Morph.Gender == Pullenti.Morph.MorphGender.Undefined && tt.Morph.Case.IsUndefined && tt.Morph.Number == Pullenti.Morph.MorphNumber.Undefined) 
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Это глагол-инфиниитив
        /// </summary>
        public bool IsVerbInfinitive
        {
            get
            {
                foreach (Pullenti.Morph.MorphBaseInfo f in Morph.Items) 
                {
                    if (f.Class.IsVerb && (f is Pullenti.Morph.MorphWordForm) && (f as Pullenti.Morph.MorphWordForm).Misc.Attrs.Contains("инф.")) 
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Это глагол быть, являться...
        /// </summary>
        public bool IsVerbBe
        {
            get
            {
                Pullenti.Morph.MorphWordForm wf = VerbMorph;
                if (wf != null) 
                {
                    if (wf.NormalCase == "БЫТЬ" || wf.NormalCase == "ЯВЛЯТЬСЯ") 
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Это возвратный глагол
        /// </summary>
        public bool IsVerbReversive
        {
            get
            {
                if (IsVerbBe) 
                    return false;
                if (VerbMorph != null) 
                {
                    if (VerbMorph.ContainsAttr("возвр.", null)) 
                        return true;
                    if (VerbMorph.NormalCase != null) 
                    {
                        if (VerbMorph.NormalCase.EndsWith("СЯ") || VerbMorph.NormalCase.EndsWith("СЬ")) 
                            return true;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// Это глагол в страдательном залоге
        /// </summary>
        public bool IsVerbPassive
        {
            get
            {
                if (IsVerbBe) 
                    return false;
                if (Morph.ContainsAttr("страд.з", null)) 
                    return true;
                if (VerbMorph != null) 
                {
                    if (VerbMorph.Misc.Voice == Pullenti.Morph.MorphVoice.Passive) 
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Нормализованное значение
        /// </summary>
        public string Normal
        {
            get
            {
                Pullenti.Morph.MorphWordForm wf = VerbMorph;
                if (wf != null) 
                {
                    if (!wf.Class.IsAdjective && !wf.Case.IsUndefined && m_Normal != null) 
                        return m_Normal;
                    if (wf.Class.IsAdjective && !wf.Class.IsVerb) 
                        return wf.NormalFull ?? wf.NormalCase;
                    return wf.NormalCase;
                }
                return m_Normal;
            }
            set
            {
                m_Normal = value;
            }
        }
        string m_Normal;
        /// <summary>
        /// Полное морф.информация (для глагола)
        /// </summary>
        public Pullenti.Morph.MorphWordForm VerbMorph
        {
            get
            {
                if (m_VerbMorph != null) 
                    return m_VerbMorph;
                foreach (Pullenti.Morph.MorphBaseInfo f in Morph.Items) 
                {
                    if (f.Class.IsVerb && (f is Pullenti.Morph.MorphWordForm) && (((f as Pullenti.Morph.MorphWordForm).Misc.Person & Pullenti.Morph.MorphPerson.Third)) != Pullenti.Morph.MorphPerson.Undefined) 
                    {
                        if ((f as Pullenti.Morph.MorphWordForm).NormalCase.EndsWith("СЯ")) 
                            return f as Pullenti.Morph.MorphWordForm;
                    }
                }
                foreach (Pullenti.Morph.MorphBaseInfo f in Morph.Items) 
                {
                    if (f.Class.IsVerb && (f is Pullenti.Morph.MorphWordForm) && (((f as Pullenti.Morph.MorphWordForm).Misc.Person & Pullenti.Morph.MorphPerson.Third)) != Pullenti.Morph.MorphPerson.Undefined) 
                        return f as Pullenti.Morph.MorphWordForm;
                }
                foreach (Pullenti.Morph.MorphBaseInfo f in Morph.Items) 
                {
                    if (f.Class.IsVerb && (f is Pullenti.Morph.MorphWordForm)) 
                        return f as Pullenti.Morph.MorphWordForm;
                }
                foreach (Pullenti.Morph.MorphBaseInfo f in Morph.Items) 
                {
                    if (f.Class.IsAdjective && (f is Pullenti.Morph.MorphWordForm)) 
                        return f as Pullenti.Morph.MorphWordForm;
                }
                if (m_Normal == "НЕТ") 
                    return new Pullenti.Morph.MorphWordForm() { Class = Pullenti.Morph.MorphClass.Verb, Misc = new Pullenti.Morph.MorphMiscInfo() };
                return null;
            }
            set
            {
                m_VerbMorph = value;
            }
        }
        Pullenti.Morph.MorphWordForm m_VerbMorph;
        public override string ToString()
        {
            return ((Not ? "НЕ " : "")) + Normal;
        }
    }
}