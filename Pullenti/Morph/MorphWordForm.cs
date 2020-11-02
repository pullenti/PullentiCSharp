/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Morph
{
    /// <summary>
    /// Словоформа (вариант морфанализа лексемы)
    /// </summary>
    public class MorphWordForm : MorphBaseInfo
    {
        /// <summary>
        /// Полная нормальная форма: 
        /// - для существ. и местоимений - именит. падеж единств. число; 
        /// - для прилаг.  - именит. падеж единств. число мужской род; 
        /// - для глаголов - инфинитив;
        /// </summary>
        public string NormalFull;
        /// <summary>
        /// Именительная нормальная форма (падежная нормализация - 
        /// только приведение к именительному падежу, остальные хар-ки без изменений), 
        /// для глаголов - инфинитив.
        /// </summary>
        public string NormalCase;
        /// <summary>
        /// Дополнительная морф.информация
        /// </summary>
        public MorphMiscInfo Misc;
        /// <summary>
        /// Находится ли словоформа в словаре (если false, то восстановлена по аналогии)
        /// </summary>
        public bool IsInDictionary
        {
            get
            {
                return UndefCoef == 0;
            }
        }
        /// <summary>
        /// Коэффициент достоверности для неизвестных словоформ (чем больше, тем вероятнее)
        /// </summary>
        public short UndefCoef;
        public void CopyFromWordForm(MorphWordForm src)
        {
            base.CopyFrom(src);
            UndefCoef = src.UndefCoef;
            NormalCase = src.NormalCase;
            NormalFull = src.NormalFull;
            Misc = src.Misc;
        }
        public MorphWordForm()
        {
        }
        public MorphWordForm(Pullenti.Morph.Internal.MorphRuleVariant v, string word, MorphMiscInfo mi)
        {
            if (v == null) 
                return;
            this.CopyFrom(v);
            Misc = mi;
            if (v.NormalTail != null && word != null) 
            {
                string wordBegin = word;
                if (LanguageHelper.EndsWith(word, v.Tail)) 
                    wordBegin = word.Substring(0, word.Length - v.Tail.Length);
                if (v.NormalTail.Length > 0) 
                    NormalCase = wordBegin + v.NormalTail;
                else 
                    NormalCase = wordBegin;
            }
            if (v.FullNormalTail != null && word != null) 
            {
                string wordBegin = word;
                if (LanguageHelper.EndsWith(word, v.Tail)) 
                    wordBegin = word.Substring(0, word.Length - v.Tail.Length);
                if (v.FullNormalTail.Length > 0) 
                    NormalFull = wordBegin + v.FullNormalTail;
                else 
                    NormalFull = wordBegin;
            }
        }
        public override string ToString()
        {
            return this.ToStringEx(false);
        }
        public string ToStringEx(bool ignoreNormals)
        {
            StringBuilder res = new StringBuilder();
            if (!ignoreNormals) 
            {
                res.Append(NormalCase ?? "");
                if (NormalFull != null && NormalFull != NormalCase) 
                    res.AppendFormat("\\{0}", NormalFull);
                if (res.Length > 0) 
                    res.Append(' ');
            }
            res.Append(base.ToString());
            string s = (Misc == null ? null : Misc.ToString());
            if (!string.IsNullOrEmpty(s)) 
                res.AppendFormat(" {0}", s);
            if (UndefCoef > 0) 
                res.AppendFormat(" (? {0})", UndefCoef);
            return res.ToString();
        }
        public override bool ContainsAttr(string attrValue, MorphClass cla = null)
        {
            if (Misc != null && Misc.Attrs != null) 
                return Misc.Attrs.Contains(attrValue);
            return false;
        }
        internal bool HasMorphEquals(List<MorphWordForm> list)
        {
            foreach (MorphWordForm mr in list) 
            {
                if ((Class == mr.Class && Number == mr.Number && Gender == mr.Gender) && NormalCase == mr.NormalCase && NormalFull == mr.NormalFull) 
                {
                    mr.Case |= Case;
                    return true;
                }
            }
            foreach (MorphWordForm mr in list) 
            {
                if ((Class == mr.Class && Number == mr.Number && Case == mr.Case) && NormalCase == mr.NormalCase && NormalFull == mr.NormalFull) 
                {
                    mr.Gender |= Gender;
                    return true;
                }
            }
            foreach (MorphWordForm mr in list) 
            {
                if ((Class == mr.Class && Gender == mr.Gender && Case == mr.Case) && NormalCase == mr.NormalCase && NormalFull == mr.NormalFull) 
                {
                    mr.Number |= Number;
                    return true;
                }
            }
            return false;
        }
    }
}