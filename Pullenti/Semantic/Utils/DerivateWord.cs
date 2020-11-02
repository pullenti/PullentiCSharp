/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Semantic.Utils
{
    /// <summary>
    /// Слово дериватной группы DerivateWord
    /// </summary>
    public class DerivateWord
    {
        /// <summary>
        /// Само слово в нормальной форме
        /// </summary>
        public string Spelling;
        /// <summary>
        /// Часть речи
        /// </summary>
        public Pullenti.Morph.MorphClass Class;
        /// <summary>
        /// Совершенный\несовершенный (для глаголов и причастий)
        /// </summary>
        public Pullenti.Morph.MorphAspect Aspect;
        /// <summary>
        /// Действительный\страдательный (для глаголов и причастий)
        /// </summary>
        public Pullenti.Morph.MorphVoice Voice;
        /// <summary>
        /// Время (для глаголов и причастий)
        /// </summary>
        public Pullenti.Morph.MorphTense Tense;
        /// <summary>
        /// Возвратность (для глаголов и причастий)
        /// </summary>
        public bool Reflexive;
        /// <summary>
        /// Язык
        /// </summary>
        public Pullenti.Morph.MorphLang Lang;
        /// <summary>
        /// Дополнительные характеристики
        /// </summary>
        public ExplanWordAttr Attrs = new ExplanWordAttr();
        /// <summary>
        /// Возможные частые продолжения слова (идиомы)
        /// </summary>
        public List<string> NextWords = null;
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            tmp.Append(Spelling);
            if (Class != null && !Class.IsUndefined) 
                tmp.AppendFormat(", {0}", Class.ToString());
            if (Aspect != Pullenti.Morph.MorphAspect.Undefined) 
                tmp.AppendFormat(", {0}", (Aspect == Pullenti.Morph.MorphAspect.Perfective ? "соверш." : "несоверш."));
            if (Voice != Pullenti.Morph.MorphVoice.Undefined) 
                tmp.AppendFormat(", {0}", (Voice == Pullenti.Morph.MorphVoice.Active ? "действ." : (Voice == Pullenti.Morph.MorphVoice.Passive ? "страдат." : "средн.")));
            if (Tense != Pullenti.Morph.MorphTense.Undefined) 
                tmp.AppendFormat(", {0}", (Tense == Pullenti.Morph.MorphTense.Past ? "прош." : (Tense == Pullenti.Morph.MorphTense.Present ? "настоящ." : "будущ.")));
            if (Reflexive) 
                tmp.Append(", возвр.");
            if (Attrs.Value != 0) 
                tmp.AppendFormat(", {0}", Attrs.ToString());
            return tmp.ToString();
        }
    }
}