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
using System.Text;

namespace Pullenti.Ner
{
    /// <summary>
    /// Входной токен (после морфанализа)
    /// </summary>
    public class TextToken : Token
    {
        public TextToken(Pullenti.Morph.MorphToken source, Pullenti.Ner.Core.AnalysisKit kit, int bchar = -1, int echar = -1) : base(kit, (bchar >= 0 ? bchar : (source == null ? 0 : source.BeginChar)), (echar >= 0 ? echar : (source == null ? 0 : source.EndChar)))
        {
            if (source == null) 
                return;
            Chars = source.CharInfo;
            Term = source.Term;
            Lemma = source.GetLemma() ?? Term;
            MaxLengthOfMorphVars = (short)Term.Length;
            Morph = new MorphCollection();
            if (source.WordForms != null) 
            {
                foreach (Pullenti.Morph.MorphWordForm wf in source.WordForms) 
                {
                    Morph.AddItem(wf);
                    if (wf.NormalCase != null && (MaxLengthOfMorphVars < wf.NormalCase.Length)) 
                        MaxLengthOfMorphVars = (short)wf.NormalCase.Length;
                    if (wf.NormalFull != null && (MaxLengthOfMorphVars < wf.NormalFull.Length)) 
                        MaxLengthOfMorphVars = (short)wf.NormalFull.Length;
                }
            }
            for (int i = 0; i < Term.Length; i++) 
            {
                char ch = Term[i];
                int j;
                for (j = 0; j < Morph.ItemsCount; j++) 
                {
                    Pullenti.Morph.MorphWordForm wf = Morph[j] as Pullenti.Morph.MorphWordForm;
                    if (wf.NormalCase != null) 
                    {
                        if (i >= wf.NormalCase.Length) 
                            break;
                        if (wf.NormalCase[i] != ch) 
                            break;
                    }
                    if (wf.NormalFull != null) 
                    {
                        if (i >= wf.NormalFull.Length) 
                            break;
                        if (wf.NormalFull[i] != ch) 
                            break;
                    }
                }
                if (j < Morph.ItemsCount) 
                    break;
                InvariantPrefixLengthOfMorphVars = (short)((i + 1));
            }
            if (Morph.Language.IsUndefined && !source.Language.IsUndefined) 
                Morph.Language = source.Language;
        }
        /// <summary>
        /// Исходный фрагмент, слегка нормализованный (не морфологически, а символьно)
        /// </summary>
        public string Term;
        /// <summary>
        /// А это уже лемма (нормальная форма слова)
        /// </summary>
        public string Lemma;
        /// <summary>
        /// Это вариант до коррекции (если была коррекция)
        /// </summary>
        public string Term0;
        /// <summary>
        /// Это количество начальных символов, одинаковых для всех морфологических вариантов 
        /// (пригодится для оптимизации поиска)
        /// </summary>
        public short InvariantPrefixLengthOfMorphVars;
        /// <summary>
        /// Максимальная длина в символах варианта среди всех морфвариантов
        /// </summary>
        public short MaxLengthOfMorphVars;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder(Term);
            foreach (Pullenti.Morph.MorphBaseInfo l in Morph.Items) 
            {
                res.AppendFormat(", {0}", l.ToString());
            }
            return res.ToString();
        }
        /// <summary>
        /// Попробовать привязать словарь
        /// </summary>
        public object CheckValue(Dictionary<string, object> dict)
        {
            if (dict == null) 
                return null;
            object res;
            if (dict.TryGetValue(Term, out res)) 
                return res;
            if (Morph != null) 
            {
                foreach (Pullenti.Morph.MorphBaseInfo it in Morph.Items) 
                {
                    Pullenti.Morph.MorphWordForm mf = it as Pullenti.Morph.MorphWordForm;
                    if (mf != null) 
                    {
                        if (mf.NormalCase != null) 
                        {
                            if (dict.TryGetValue(mf.NormalCase, out res)) 
                                return res;
                        }
                        if (mf.NormalFull != null && mf.NormalCase != mf.NormalFull) 
                        {
                            if (dict.TryGetValue(mf.NormalFull, out res)) 
                                return res;
                        }
                    }
                }
            }
            return null;
        }
        public override string GetSourceText()
        {
            return base.GetSourceText();
        }
        public override bool IsValue(string term, string termUA = null)
        {
            if (termUA != null && Morph.Language.IsUa) 
            {
                if (this.IsValue(termUA, null)) 
                    return true;
            }
            if (term == null) 
                return false;
            if (InvariantPrefixLengthOfMorphVars > term.Length) 
                return false;
            if (MaxLengthOfMorphVars >= Term.Length && (MaxLengthOfMorphVars < term.Length)) 
                return false;
            if (term == Term) 
                return true;
            foreach (Pullenti.Morph.MorphBaseInfo wf in Morph.Items) 
            {
                if ((wf is Pullenti.Morph.MorphWordForm) && (((wf as Pullenti.Morph.MorphWordForm).NormalCase == term || (wf as Pullenti.Morph.MorphWordForm).NormalFull == term))) 
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Это соединительный союз И (на всех языках)
        /// </summary>
        public override bool IsAnd
        {
            get
            {
                if (!Morph.Class.IsConjunction) 
                {
                    if (LengthChar == 1 && this.IsChar('&')) 
                        return true;
                    return false;
                }
                string val = Term;
                if (val == "И" || val == "AND" || val == "UND") 
                    return true;
                if (Morph.Language.IsUa) 
                {
                    if (val == "І" || val == "ТА") 
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Это соединительный союз ИЛИ (на всех языках)
        /// </summary>
        public override bool IsOr
        {
            get
            {
                if (!Morph.Class.IsConjunction) 
                    return false;
                string val = Term;
                if (val == "ИЛИ" || val == "ЛИБО" || val == "OR") 
                    return true;
                if (Morph.Language.IsUa) 
                {
                    if (val == "АБО") 
                        return true;
                }
                return false;
            }
        }
        public override bool IsLetters
        {
            get
            {
                return char.IsLetter(Term[0]);
            }
        }
        public override Pullenti.Morph.MorphClass GetMorphClassInDictionary()
        {
            Pullenti.Morph.MorphClass res = new Pullenti.Morph.MorphClass();
            foreach (Pullenti.Morph.MorphBaseInfo wf in Morph.Items) 
            {
                if ((wf is Pullenti.Morph.MorphWordForm) && (wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                    res |= wf.Class;
            }
            return res;
        }
        public override string GetNormalCaseText(Pullenti.Morph.MorphClass mc = null, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined, bool keepChars = false)
        {
            bool empty = true;
            if (mc != null && mc.IsPreposition) 
                return Pullenti.Morph.LanguageHelper.NormalizePreposition(Term);
            foreach (Pullenti.Morph.MorphBaseInfo it in Morph.Items) 
            {
                if (mc != null && !mc.IsUndefined) 
                {
                    Pullenti.Morph.MorphClass cc = it.Class & mc;
                    if (cc.IsUndefined) 
                        continue;
                    if (cc.IsMisc && !cc.IsProper && mc != it.Class) 
                        continue;
                }
                Pullenti.Morph.MorphWordForm wf = it as Pullenti.Morph.MorphWordForm;
                bool normalFull = false;
                if (gender != Pullenti.Morph.MorphGender.Undefined) 
                {
                    if (((it.Gender & gender)) == Pullenti.Morph.MorphGender.Undefined) 
                    {
                        if ((gender == Pullenti.Morph.MorphGender.Masculine && ((it.Gender != Pullenti.Morph.MorphGender.Undefined || it.Number == Pullenti.Morph.MorphNumber.Plural)) && wf != null) && wf.NormalFull != null) 
                            normalFull = true;
                        else if (gender == Pullenti.Morph.MorphGender.Masculine && it.Class.IsPersonalPronoun) 
                        {
                        }
                        else 
                            continue;
                    }
                }
                if (!it.Case.IsUndefined) 
                    empty = false;
                if (wf != null) 
                {
                    string res;
                    if (num == Pullenti.Morph.MorphNumber.Singular && it.Number == Pullenti.Morph.MorphNumber.Plural && wf.NormalFull != null) 
                    {
                        int le = wf.NormalCase.Length;
                        if ((le == (wf.NormalFull.Length + 2) && le > 4 && wf.NormalCase[le - 2] == 'С') && wf.NormalCase[le - 1] == 'Я') 
                            res = wf.NormalCase;
                        else 
                            res = (normalFull ? wf.NormalFull : wf.NormalFull);
                    }
                    else 
                        res = (normalFull ? wf.NormalFull : (wf.NormalCase ?? Term));
                    if (num == Pullenti.Morph.MorphNumber.Singular && mc != null && mc == Pullenti.Morph.MorphClass.Noun) 
                    {
                        if (res == "ДЕТИ") 
                            res = "РЕБЕНОК";
                    }
                    if (keepChars) 
                    {
                        if (Chars.IsAllLower) 
                            res = res.ToLower();
                        else if (Chars.IsCapitalUpper) 
                            res = Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(res);
                    }
                    return res;
                }
            }
            if (!empty) 
                return null;
            string te = null;
            if (num == Pullenti.Morph.MorphNumber.Singular && mc != null) 
            {
                Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo() { Class = new Pullenti.Morph.MorphClass() { Value = mc.Value }, Gender = gender, Number = Pullenti.Morph.MorphNumber.Singular, Language = Morph.Language };
                string vars = Pullenti.Morph.MorphologyService.GetWordform(Term, bi);
                if (vars != null) 
                    te = vars;
            }
            if (te == null) 
                te = Term;
            if (keepChars) 
            {
                if (Chars.IsAllLower) 
                    return te.ToLower();
                else if (Chars.IsCapitalUpper) 
                    return Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(te);
            }
            return te;
        }
        public static List<TextToken> GetSourceTextTokens(Token begin, Token end)
        {
            List<TextToken> res = new List<TextToken>();
            for (Token t = begin; t != null && t != end.Next && t.EndChar <= end.EndChar; t = t.Next) 
            {
                if (t is TextToken) 
                    res.Add(t as TextToken);
                else if (t is MetaToken) 
                    res.AddRange(GetSourceTextTokens((t as MetaToken).BeginToken, (t as MetaToken).EndToken));
            }
            return res;
        }
        /// <summary>
        /// Признак того, что это чистый глагол
        /// </summary>
        public bool IsPureVerb
        {
            get
            {
                bool ret = false;
                if ((this.IsValue("МОЖНО", null) || this.IsValue("МОЖЕТ", null) || this.IsValue("ДОЛЖНЫЙ", null)) || this.IsValue("НУЖНО", null)) 
                    return true;
                foreach (Pullenti.Morph.MorphBaseInfo it in Morph.Items) 
                {
                    if ((it is Pullenti.Morph.MorphWordForm) && (it as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                    {
                        if (it.Class.IsVerb && it.Case.IsUndefined) 
                            ret = true;
                        else if (!it.Class.IsVerb) 
                        {
                            if (it.Class.IsAdjective && it.ContainsAttr("к.ф.", null)) 
                            {
                            }
                            else 
                                return false;
                        }
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// Проверка, что это глагол типа БЫТЬ, ЯВЛЯТЬСЯ и т.п.
        /// </summary>
        public bool IsVerbBe
        {
            get
            {
                if ((this.IsValue("БЫТЬ", null) || this.IsValue("ЕСТЬ", null) || this.IsValue("ЯВЛЯТЬ", null)) || this.IsValue("BE", null)) 
                    return true;
                if (Term == "IS" || Term == "WAS" || Term == "BECAME") 
                    return true;
                if (Term == "Є") 
                    return true;
                return false;
            }
        }
        internal override void Serialize(Stream stream)
        {
            base.Serialize(stream);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, Term);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, Lemma);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, InvariantPrefixLengthOfMorphVars);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, MaxLengthOfMorphVars);
        }
        internal override void Deserialize(Stream stream, Pullenti.Ner.Core.AnalysisKit kit, int vers)
        {
            base.Deserialize(stream, kit, vers);
            Term = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
            Lemma = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
            InvariantPrefixLengthOfMorphVars = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream);
            MaxLengthOfMorphVars = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream);
        }
    }
}