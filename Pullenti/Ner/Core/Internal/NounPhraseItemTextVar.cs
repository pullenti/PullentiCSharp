/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core.Internal
{
    /// <summary>
    /// Морфологический вариант для элемента именной группы
    /// </summary>
    class NounPhraseItemTextVar : Pullenti.Morph.MorphBaseInfo
    {
        public NounPhraseItemTextVar(Pullenti.Morph.MorphBaseInfo src = null, Pullenti.Ner.Token t = null) : base()
        {
            if (src != null) 
                this.CopyFrom(src);
            Pullenti.Morph.MorphWordForm wf = src as Pullenti.Morph.MorphWordForm;
            if (wf != null) 
            {
                NormalValue = wf.NormalCase;
                if (wf.Number == Pullenti.Morph.MorphNumber.Plural && wf.NormalFull != null) 
                    SingleNumberValue = wf.NormalFull;
                UndefCoef = wf.UndefCoef;
            }
            else if (t != null) 
                NormalValue = t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
            if (Case.IsUndefined && src != null) 
            {
                if (src.ContainsAttr("неизм.", null)) 
                    Case = Pullenti.Morph.MorphCase.AllCases;
            }
        }
        /// <summary>
        /// Нормализованное значение
        /// </summary>
        public string NormalValue;
        /// <summary>
        /// Нормализованное значение в единственном числе
        /// </summary>
        public string SingleNumberValue;
        public int UndefCoef;
        public override string ToString()
        {
            return string.Format("{0} {1}", NormalValue, base.ToString());
        }
        public void CopyFromItem(NounPhraseItemTextVar src)
        {
            this.CopyFrom(src);
            NormalValue = src.NormalValue;
            SingleNumberValue = src.SingleNumberValue;
            UndefCoef = src.UndefCoef;
        }
        public void CorrectPrefix(Pullenti.Ner.TextToken t, bool ignoreGender)
        {
            if (t == null) 
                return;
            foreach (Pullenti.Morph.MorphBaseInfo v in t.Morph.Items) 
            {
                if (v.Class == Class && this.CheckAccord(v, ignoreGender, false)) 
                {
                    NormalValue = string.Format("{0}-{1}", (v as Pullenti.Morph.MorphWordForm).NormalCase, NormalValue);
                    if (SingleNumberValue != null) 
                        SingleNumberValue = string.Format("{0}-{1}", (v as Pullenti.Morph.MorphWordForm).NormalFull ?? (v as Pullenti.Morph.MorphWordForm).NormalCase, SingleNumberValue);
                    return;
                }
            }
            NormalValue = string.Format("{0}-{1}", t.Term, NormalValue);
            if (SingleNumberValue != null) 
                SingleNumberValue = string.Format("{0}-{1}", t.Term, SingleNumberValue);
        }
    }
}