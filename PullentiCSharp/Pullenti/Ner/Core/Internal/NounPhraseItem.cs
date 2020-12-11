/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core.Internal
{
    /// <summary>
    /// Элемент именной группы
    /// </summary>
    class NounPhraseItem : Pullenti.Ner.MetaToken
    {
        public NounPhraseItem(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public bool ConjBefore;
        public List<NounPhraseItemTextVar> AdjMorph = new List<NounPhraseItemTextVar>();
        public bool CanBeAdj;
        public List<NounPhraseItemTextVar> NounMorph = new List<NounPhraseItemTextVar>();
        public bool CanBeNoun;
        public bool MultiNouns;
        public bool CanBeSurname;
        public bool IsStdAdjective;
        public bool IsDoubtAdjective;
        /// <summary>
        /// Это признак количественного (число, НЕСКОЛЬКО, МНОГО)
        /// </summary>
        public bool CanBeNumericAdj
        {
            get
            {
                Pullenti.Ner.NumberToken num = BeginToken as Pullenti.Ner.NumberToken;
                if (num != null) 
                {
                    if (num.IntValue != null && num.IntValue.Value > 1) 
                        return true;
                    else 
                        return false;
                }
                if ((BeginToken.IsValue("НЕСКОЛЬКО", null) || BeginToken.IsValue("МНОГО", null) || BeginToken.IsValue("ПАРА", null)) || BeginToken.IsValue("ПОЛТОРА", null)) 
                    return true;
                return false;
            }
        }
        public bool IsPronoun
        {
            get
            {
                return BeginToken.Morph.Class.IsPronoun;
            }
        }
        public bool IsPersonalPronoun
        {
            get
            {
                return BeginToken.Morph.Class.IsPersonalPronoun;
            }
        }
        /// <summary>
        /// Это признак причастия
        /// </summary>
        public bool IsVerb
        {
            get
            {
                return BeginToken.Morph.Class.IsVerb;
            }
        }
        public bool IsAdverb
        {
            get
            {
                return BeginToken.Morph.Class.IsAdverb;
            }
        }
        public bool CanBeAdjForPersonalPronoun
        {
            get
            {
                if (IsPronoun && CanBeAdj) 
                {
                    if (BeginToken.IsValue("ВСЕ", null) || BeginToken.IsValue("ВЕСЬ", null) || BeginToken.IsValue("САМ", null)) 
                        return true;
                }
                return false;
            }
        }
        string _corrChars(string str, bool keep)
        {
            if (!keep) 
                return str;
            if (Chars.IsAllLower) 
                return str.ToLower();
            if (Chars.IsCapitalUpper) 
                return Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(str);
            return str;
        }
        public override string GetNormalCaseText(Pullenti.Morph.MorphClass mc = null, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined, bool keepChars = false)
        {
            if ((BeginToken is Pullenti.Ner.ReferentToken) && BeginToken == EndToken) 
                return BeginToken.GetNormalCaseText(mc, num, gender, keepChars);
            string res = null;
            int maxCoef = 0;
            int defCoef = -1;
            foreach (Pullenti.Morph.MorphBaseInfo it in Morph.Items) 
            {
                NounPhraseItemTextVar v = it as NounPhraseItemTextVar;
                if (v == null) 
                    continue;
                if (v.UndefCoef > 0 && (((v.UndefCoef < maxCoef) || defCoef >= 0))) 
                    continue;
                if (num == Pullenti.Morph.MorphNumber.Singular && v.SingleNumberValue != null) 
                {
                    if (mc != null && ((gender == Pullenti.Morph.MorphGender.Neuter || gender == Pullenti.Morph.MorphGender.Feminie)) && mc.IsAdjective) 
                    {
                        Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo() { Class = new Pullenti.Morph.MorphClass() { Value = mc.Value }, Gender = gender, Number = Pullenti.Morph.MorphNumber.Singular, Case = Pullenti.Morph.MorphCase.Nominative, Language = Morph.Language };
                        string str = Pullenti.Morph.MorphologyService.GetWordform(v.SingleNumberValue, bi);
                        if (str != null) 
                            res = str;
                    }
                    else 
                        res = v.SingleNumberValue;
                    if (v.UndefCoef == 0) 
                        break;
                    maxCoef = v.UndefCoef;
                    continue;
                }
                if (string.IsNullOrEmpty(v.NormalValue)) 
                    continue;
                if (char.IsDigit(v.NormalValue[0]) && mc != null && mc.IsAdjective) 
                {
                    int val;
                    if (int.TryParse(v.NormalValue, out val)) 
                    {
                        string str = Pullenti.Ner.Core.NumberHelper.GetNumberAdjective(val, gender, (num == Pullenti.Morph.MorphNumber.Singular || val == 1 ? Pullenti.Morph.MorphNumber.Singular : Pullenti.Morph.MorphNumber.Plural));
                        if (str != null) 
                        {
                            res = str;
                            if (v.UndefCoef == 0) 
                                break;
                            maxCoef = v.UndefCoef;
                            continue;
                        }
                    }
                }
                string res1 = (it as NounPhraseItemTextVar).NormalValue;
                if (num == Pullenti.Morph.MorphNumber.Singular) 
                {
                    if (res1 == "ДЕТИ") 
                        res1 = "РЕБЕНОК";
                    else if (res1 == "ЛЮДИ") 
                        res1 = "ЧЕЛОВЕК";
                }
                maxCoef = v.UndefCoef;
                if (v.UndefCoef > 0) 
                {
                    res = res1;
                    continue;
                }
                int defCo = 0;
                if (mc != null && mc.IsAdjective && v.UndefCoef == 0) 
                {
                }
                else if (((BeginToken is Pullenti.Ner.TextToken) && res1 == (BeginToken as Pullenti.Ner.TextToken).Term && it.Case.IsNominative) && it.Number == Pullenti.Morph.MorphNumber.Singular) 
                    defCo = 1;
                if (num == Pullenti.Morph.MorphNumber.Plural && ((v.Number & Pullenti.Morph.MorphNumber.Plural)) == Pullenti.Morph.MorphNumber.Plural) 
                    defCo += 3;
                if (res == null || defCo > defCoef) 
                {
                    res = res1;
                    defCoef = defCo;
                    if (defCo > 0) 
                        break;
                }
            }
            if (res != null) 
                return this._corrChars(res, keepChars);
            if (res == null && BeginToken == EndToken) 
                res = BeginToken.GetNormalCaseText(mc, num, gender, keepChars);
            else if (res == null) 
            {
                res = BeginToken.GetNormalCaseText(mc, num, gender, keepChars);
                if (res == null) 
                    res = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(this, (keepChars ? Pullenti.Ner.Core.GetTextAttr.KeepRegister : Pullenti.Ner.Core.GetTextAttr.No));
                else 
                    res = string.Format("{0} {1}", res, Pullenti.Ner.Core.MiscHelper.GetTextValue(BeginToken.Next, EndToken, (keepChars ? Pullenti.Ner.Core.GetTextAttr.KeepRegister : Pullenti.Ner.Core.GetTextAttr.No)));
            }
            return res ?? "?";
        }
        public override bool IsValue(string term, string term2 = null)
        {
            if (BeginToken != null) 
                return BeginToken.IsValue(term, term2);
            else 
                return false;
        }
        public static NounPhraseItem TryParse(Pullenti.Ner.Token t, List<NounPhraseItem> items, Pullenti.Ner.Core.NounPhraseParseAttr attrs)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            bool _canBeSurname = false;
            bool _isDoubtAdj = false;
            Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
            if (rt != null && rt.BeginToken == rt.EndToken && (rt.BeginToken is Pullenti.Ner.TextToken)) 
            {
                NounPhraseItem res = TryParse(rt.BeginToken, items, attrs);
                if (res != null) 
                {
                    res.BeginToken = (res.EndToken = t);
                    res.CanBeNoun = true;
                    return res;
                }
            }
            if (rt != null) 
            {
                NounPhraseItem res = new NounPhraseItem(t, t);
                foreach (Pullenti.Morph.MorphBaseInfo m in t.Morph.Items) 
                {
                    NounPhraseItemTextVar v = new NounPhraseItemTextVar(m, null);
                    v.NormalValue = t.GetReferent().ToString();
                    res.NounMorph.Add(v);
                }
                res.CanBeNoun = true;
                return res;
            }
            if (t is Pullenti.Ner.NumberToken) 
            {
            }
            bool hasLegalVerb = false;
            if (t is Pullenti.Ner.TextToken) 
            {
                if (!t.Chars.IsLetter) 
                    return null;
                string str = (t as Pullenti.Ner.TextToken).Term;
                if (str[str.Length - 1] == 'А' || str[str.Length - 1] == 'О') 
                {
                    foreach (Pullenti.Morph.MorphBaseInfo wf in t.Morph.Items) 
                    {
                        if ((wf is Pullenti.Morph.MorphWordForm) && (wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                        {
                            if (wf.Class.IsVerb) 
                            {
                                Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                                if (!mc.IsNoun && ((attrs & Pullenti.Ner.Core.NounPhraseParseAttr.IgnoreParticiples)) == Pullenti.Ner.Core.NounPhraseParseAttr.No) 
                                {
                                    if (!Pullenti.Morph.LanguageHelper.EndsWithEx(str, "ОГО", "ЕГО", null, null)) 
                                        return null;
                                }
                                hasLegalVerb = true;
                            }
                            if (wf.Class.IsAdverb) 
                            {
                                if (t.Next == null || !t.Next.IsHiphen) 
                                {
                                    if ((str == "ВСЕГО" || str == "ДОМА" || str == "НЕСКОЛЬКО") || str == "МНОГО" || str == "ПОРЯДКА") 
                                    {
                                    }
                                    else 
                                        return null;
                                }
                            }
                            if (wf.Class.IsAdjective) 
                            {
                                if (wf.ContainsAttr("к.ф.", null)) 
                                {
                                    if (t.GetMorphClassInDictionary() == Pullenti.Morph.MorphClass.Adjective) 
                                    {
                                    }
                                    else 
                                        _isDoubtAdj = true;
                                }
                            }
                        }
                    }
                }
                Pullenti.Morph.MorphClass mc0 = t.Morph.Class;
                if (mc0.IsProperSurname && !t.Chars.IsAllLower) 
                {
                    foreach (Pullenti.Morph.MorphBaseInfo wf in t.Morph.Items) 
                    {
                        if (wf.Class.IsProperSurname && wf.Number != Pullenti.Morph.MorphNumber.Plural) 
                        {
                            Pullenti.Morph.MorphWordForm wff = wf as Pullenti.Morph.MorphWordForm;
                            if (wff == null) 
                                continue;
                            string s = ((wff.NormalFull ?? wff.NormalCase)) ?? "";
                            if (Pullenti.Morph.LanguageHelper.EndsWithEx(s, "ИН", "ЕН", "ЫН", null)) 
                            {
                                if (!wff.IsInDictionary) 
                                    _canBeSurname = true;
                                else 
                                    return null;
                            }
                            if (wff.IsInDictionary && Pullenti.Morph.LanguageHelper.EndsWith(s, "ОВ")) 
                                _canBeSurname = true;
                        }
                    }
                }
                if (mc0.IsProperName && !t.Chars.IsAllLower) 
                {
                    foreach (Pullenti.Morph.MorphBaseInfo wff in t.Morph.Items) 
                    {
                        Pullenti.Morph.MorphWordForm wf = wff as Pullenti.Morph.MorphWordForm;
                        if (wf == null) 
                            continue;
                        if (wf.NormalCase == "ГОР") 
                            continue;
                        if (wf.Class.IsProperName && wf.IsInDictionary) 
                        {
                            if (wf.NormalCase == null || !wf.NormalCase.StartsWith("ЛЮБ")) 
                            {
                                if (mc0.IsAdjective && t.Morph.ContainsAttr("неизм.", null)) 
                                {
                                }
                                else if (((attrs & Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun)) == Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun) 
                                {
                                }
                                else 
                                {
                                    if (items == null || (items.Count < 1)) 
                                        return null;
                                    if (!items[0].IsStdAdjective) 
                                        return null;
                                }
                            }
                        }
                    }
                }
                if (mc0.IsAdjective && t.Morph.ItemsCount == 1) 
                {
                    if (t.Morph[0].ContainsAttr("в.ср.ст.", null)) 
                        return null;
                }
                Pullenti.Morph.MorphClass mc1 = t.GetMorphClassInDictionary();
                if (mc1 == Pullenti.Morph.MorphClass.Verb && t.Morph.Case.IsUndefined) 
                    return null;
                if ((((attrs & Pullenti.Ner.Core.NounPhraseParseAttr.IgnoreParticiples)) == Pullenti.Ner.Core.NounPhraseParseAttr.IgnoreParticiples && t.Morph.Class.IsVerb && !t.Morph.Class.IsNoun) && !t.Morph.Class.IsProper) 
                {
                    foreach (Pullenti.Morph.MorphBaseInfo wf in t.Morph.Items) 
                    {
                        if (wf.Class.IsVerb) 
                        {
                            if (wf.ContainsAttr("дейст.з.", null)) 
                            {
                                if (Pullenti.Morph.LanguageHelper.EndsWith((t as Pullenti.Ner.TextToken).Term, "СЯ")) 
                                {
                                }
                                else 
                                    return null;
                            }
                        }
                    }
                }
            }
            Pullenti.Ner.Token t1 = null;
            for (int k = 0; k < 2; k++) 
            {
                t = t1 ?? t0;
                if (k == 0) 
                {
                    if (((t0 is Pullenti.Ner.TextToken) && t0.Next != null && t0.Next.IsHiphen) && t0.Next.Next != null) 
                    {
                        if (!t0.IsWhitespaceAfter && !t0.Morph.Class.IsPronoun && !(t0.Next.Next is Pullenti.Ner.NumberToken)) 
                        {
                            if (!t0.Next.IsWhitespaceAfter) 
                                t = t0.Next.Next;
                            else if (t0.Next.Next.Chars.IsAllLower && Pullenti.Morph.LanguageHelper.EndsWith((t0 as Pullenti.Ner.TextToken).Term, "О")) 
                                t = t0.Next.Next;
                        }
                    }
                }
                NounPhraseItem it = new NounPhraseItem(t0, t) { CanBeSurname = _canBeSurname };
                if (t0 == t && (t0 is Pullenti.Ner.ReferentToken)) 
                {
                    it.CanBeNoun = true;
                    it.Morph = new Pullenti.Ner.MorphCollection(t0.Morph);
                }
                bool canBePrepos = false;
                foreach (Pullenti.Morph.MorphBaseInfo v in t.Morph.Items) 
                {
                    Pullenti.Morph.MorphWordForm wf = v as Pullenti.Morph.MorphWordForm;
                    if (v.Class.IsVerb && !v.Case.IsUndefined) 
                    {
                        it.CanBeAdj = true;
                        it.AdjMorph.Add(new NounPhraseItemTextVar(v, t));
                        continue;
                    }
                    if (v.Class.IsPreposition) 
                        canBePrepos = true;
                    if (v.Class.IsAdjective || ((v.Class.IsPronoun && !v.Class.IsPersonalPronoun && !v.ContainsAttr("неизм.", null))) || ((v.Class.IsNoun && (t is Pullenti.Ner.NumberToken)))) 
                    {
                        if (TryAccordVariant(items, (items == null ? 0 : items.Count), v, false)) 
                        {
                            bool isDoub = false;
                            if (v.ContainsAttr("к.ф.", null)) 
                                continue;
                            if (v.ContainsAttr("собир.", null) && !(t is Pullenti.Ner.NumberToken)) 
                            {
                                if (wf != null && wf.IsInDictionary) 
                                    return null;
                                continue;
                            }
                            if (v.ContainsAttr("сравн.", null)) 
                                continue;
                            bool ok = true;
                            if (t is Pullenti.Ner.TextToken) 
                            {
                                string s = (t as Pullenti.Ner.TextToken).Term;
                                if (s == "ПРАВО" || s == "ПРАВА") 
                                    ok = false;
                                else if (Pullenti.Morph.LanguageHelper.EndsWith(s, "ОВ") && t.GetMorphClassInDictionary().IsNoun) 
                                    ok = false;
                            }
                            else if (t is Pullenti.Ner.NumberToken) 
                            {
                                if (v.Class.IsNoun && t.Morph.Class.IsAdjective) 
                                    ok = false;
                                else if (t.Morph.Class.IsNoun && ((attrs & Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective)) == Pullenti.Ner.Core.NounPhraseParseAttr.No) 
                                    ok = false;
                            }
                            if (ok) 
                            {
                                it.AdjMorph.Add(new NounPhraseItemTextVar(v, t));
                                it.CanBeAdj = true;
                                if (_isDoubtAdj && t0 == t) 
                                    it.IsDoubtAdjective = true;
                                if (hasLegalVerb && wf != null && wf.IsInDictionary) 
                                    it.CanBeNoun = true;
                                if (wf != null && wf.Class.IsPronoun) 
                                {
                                    it.CanBeNoun = true;
                                    it.NounMorph.Add(new NounPhraseItemTextVar(v, t));
                                }
                            }
                        }
                    }
                    bool canBeNoun = false;
                    if (t is Pullenti.Ner.NumberToken) 
                    {
                    }
                    else if (v.Class.IsNoun || ((wf != null && wf.NormalCase == "САМ"))) 
                        canBeNoun = true;
                    else if (v.Class.IsPersonalPronoun) 
                    {
                        if (items == null || items.Count == 0) 
                            canBeNoun = true;
                        else 
                        {
                            foreach (NounPhraseItem it1 in items) 
                            {
                                if (it1.IsVerb) 
                                {
                                    if (items.Count == 1 && !v.Case.IsNominative) 
                                        canBeNoun = true;
                                    else 
                                        return null;
                                }
                            }
                            if (items.Count == 1) 
                            {
                                if (items[0].CanBeAdjForPersonalPronoun) 
                                    canBeNoun = true;
                            }
                        }
                    }
                    else if ((v.Class.IsPronoun && ((items == null || items.Count == 0 || ((items.Count == 1 && items[0].CanBeAdjForPersonalPronoun)))) && wf != null) && (((((wf.NormalCase == "ТОТ" || wf.NormalFull == "ТО" || wf.NormalCase == "ТО") || wf.NormalCase == "ЭТО" || wf.NormalCase == "ВСЕ") || wf.NormalCase == "ЧТО" || wf.NormalCase == "КТО") || wf.NormalFull == "КОТОРЫЙ" || wf.NormalCase == "КОТОРЫЙ"))) 
                    {
                        if (wf.NormalCase == "ВСЕ") 
                        {
                            if (t.Next != null && t.Next.IsValue("РАВНО", null)) 
                                return null;
                        }
                        canBeNoun = true;
                    }
                    else if (wf != null && ((wf.NormalFull ?? wf.NormalCase)) == "КОТОРЫЙ" && ((attrs & Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns)) == Pullenti.Ner.Core.NounPhraseParseAttr.No) 
                        return null;
                    else if (v.Class.IsProper && (t is Pullenti.Ner.TextToken)) 
                    {
                        if (t.LengthChar > 4 || v.Class.IsProperName) 
                            canBeNoun = true;
                    }
                    if (canBeNoun) 
                    {
                        bool added = false;
                        if (items != null && items.Count > 1 && ((attrs & Pullenti.Ner.Core.NounPhraseParseAttr.MultiNouns)) != Pullenti.Ner.Core.NounPhraseParseAttr.No) 
                        {
                            bool ok1 = true;
                            for (int ii = 1; ii < items.Count; ii++) 
                            {
                                if (!items[ii].ConjBefore) 
                                {
                                    ok1 = false;
                                    break;
                                }
                            }
                            if (ok1) 
                            {
                                if (TryAccordVariant(items, (items == null ? 0 : items.Count), v, true)) 
                                {
                                    it.NounMorph.Add(new NounPhraseItemTextVar(v, t));
                                    it.CanBeNoun = true;
                                    it.MultiNouns = true;
                                    added = true;
                                }
                            }
                        }
                        if (!added) 
                        {
                            if (TryAccordVariant(items, (items == null ? 0 : items.Count), v, false)) 
                            {
                                it.NounMorph.Add(new NounPhraseItemTextVar(v, t));
                                it.CanBeNoun = true;
                                if (v.Class.IsPersonalPronoun && t.Morph.ContainsAttr("неизм.", null) && !it.CanBeAdj) 
                                {
                                    NounPhraseItemTextVar itt = new NounPhraseItemTextVar(v, t);
                                    itt.Case = Pullenti.Morph.MorphCase.AllCases;
                                    itt.Number = Pullenti.Morph.MorphNumber.Undefined;
                                    if (itt.NormalValue == null) 
                                    {
                                    }
                                    it.AdjMorph.Add(itt);
                                    it.CanBeAdj = true;
                                }
                            }
                            else if ((items.Count > 0 && items[0].AdjMorph.Count > 0 && items[0].AdjMorph[0].Number == Pullenti.Morph.MorphNumber.Plural) && !((items[0].AdjMorph[0].Case & v.Case)).IsUndefined && !items[0].AdjMorph[0].Class.IsVerb) 
                            {
                                if (t.Next != null && t.Next.IsCommaAnd && (t.Next.Next is Pullenti.Ner.TextToken)) 
                                {
                                    Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next.Next, attrs, 0, null);
                                    if (npt2 != null && npt2.Preposition == null && !((npt2.Morph.Case & v.Case & items[0].AdjMorph[0].Case)).IsUndefined) 
                                    {
                                        it.NounMorph.Add(new NounPhraseItemTextVar(v, t));
                                        it.CanBeNoun = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if (t0 != t) 
                {
                    foreach (NounPhraseItemTextVar v in it.AdjMorph) 
                    {
                        v.CorrectPrefix(t0 as Pullenti.Ner.TextToken, false);
                    }
                    foreach (NounPhraseItemTextVar v in it.NounMorph) 
                    {
                        v.CorrectPrefix(t0 as Pullenti.Ner.TextToken, true);
                    }
                }
                if (k == 1 && it.CanBeNoun && !it.CanBeAdj) 
                {
                    if (t1 != null) 
                        it.EndToken = t1;
                    else 
                        it.EndToken = t0.Next.Next;
                    foreach (NounPhraseItemTextVar v in it.NounMorph) 
                    {
                        if (v.NormalValue != null && (v.NormalValue.IndexOf('-') < 0)) 
                            v.NormalValue = string.Format("{0}-{1}", v.NormalValue, it.EndToken.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                    }
                }
                if (it.CanBeAdj) 
                {
                    if (m_StdAdjectives.TryParse(it.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                        it.IsStdAdjective = true;
                }
                if (canBePrepos && it.CanBeNoun) 
                {
                    if (items != null && items.Count > 0) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns | Pullenti.Ner.Core.NounPhraseParseAttr.ParseVerbs, 0, null);
                        if (npt1 != null && npt1.EndChar > t.EndChar) 
                            return null;
                    }
                    else 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns | Pullenti.Ner.Core.NounPhraseParseAttr.ParseVerbs, 0, null);
                        if (npt1 != null) 
                        {
                            Pullenti.Morph.MorphCase mc = Pullenti.Morph.LanguageHelper.GetCaseAfterPreposition((t as Pullenti.Ner.TextToken).Lemma);
                            if (!((mc & npt1.Morph.Case)).IsUndefined) 
                                return null;
                        }
                    }
                }
                if (it.CanBeNoun || it.CanBeAdj || k == 1) 
                {
                    if (it.BeginToken.Morph.Class.IsPronoun) 
                    {
                        Pullenti.Ner.Token tt2 = it.EndToken.Next;
                        if ((tt2 != null && tt2.IsHiphen && !tt2.IsWhitespaceAfter) && !tt2.IsWhitespaceBefore) 
                            tt2 = tt2.Next;
                        if (tt2 is Pullenti.Ner.TextToken) 
                        {
                            string ss = (tt2 as Pullenti.Ner.TextToken).Term;
                            if ((ss == "ЖЕ" || ss == "БЫ" || ss == "ЛИ") || ss == "Ж") 
                                it.EndToken = tt2;
                            else if (ss == "НИБУДЬ" || ss == "ЛИБО" || (((ss == "ТО" && tt2.Previous.IsHiphen)) && it.CanBeAdj)) 
                            {
                                it.EndToken = tt2;
                                foreach (NounPhraseItemTextVar m in it.AdjMorph) 
                                {
                                    m.NormalValue = string.Format("{0}-{1}", m.NormalValue, ss);
                                    if (m.SingleNumberValue != null) 
                                        m.SingleNumberValue = string.Format("{0}-{1}", m.SingleNumberValue, ss);
                                }
                            }
                        }
                    }
                    return it;
                }
                if (t0 == t) 
                {
                    if (t0.IsValue("БИЗНЕС", null) && t0.Next != null && t0.Next.Chars == t0.Chars) 
                    {
                        t1 = t0.Next;
                        continue;
                    }
                    return it;
                }
            }
            return null;
        }
        public bool TryAccordVar(Pullenti.Morph.MorphBaseInfo v, bool multinouns = false)
        {
            foreach (NounPhraseItemTextVar vv in AdjMorph) 
            {
                if (vv.CheckAccord(v, false, multinouns)) 
                {
                    if (multinouns) 
                    {
                    }
                    return true;
                }
                else if (vv.NormalValue == "СКОЛЬКО") 
                    return true;
            }
            if (CanBeNumericAdj) 
            {
                if (v.Number == Pullenti.Morph.MorphNumber.Plural) 
                    return true;
                if (BeginToken is Pullenti.Ner.NumberToken) 
                {
                    int? val = (BeginToken as Pullenti.Ner.NumberToken).IntValue;
                    if (val == null) 
                        return false;
                    string num = (BeginToken as Pullenti.Ner.NumberToken).Value;
                    if (string.IsNullOrEmpty(num)) 
                        return false;
                    char dig = num[num.Length - 1];
                    if ((((dig == '2' || dig == '3' || dig == '4')) && (val.Value < 10)) || val.Value > 20) 
                    {
                        if (v.Case.IsGenitive) 
                            return true;
                    }
                }
                string term = null;
                if (v is Pullenti.Morph.MorphWordForm) 
                    term = (v as Pullenti.Morph.MorphWordForm).NormalCase;
                if (v is NounPhraseItemTextVar) 
                    term = (v as NounPhraseItemTextVar).NormalValue;
                if (term == "ЛЕТ" || term == "ЧЕЛОВЕК") 
                    return true;
            }
            if (AdjMorph.Count > 0 && BeginToken.Morph.Class.IsPersonalPronoun && BeginToken.Morph.ContainsAttr("3 л.", null)) 
                return true;
            return false;
        }
        public static bool TryAccordVariant(List<NounPhraseItem> items, int count, Pullenti.Morph.MorphBaseInfo v, bool multinouns = false)
        {
            if (items == null || items.Count == 0) 
                return true;
            for (int i = 0; i < count; i++) 
            {
                bool ok = items[i].TryAccordVar(v, multinouns);
                if (!ok) 
                    return false;
            }
            return true;
        }
        public static bool TryAccordAdjAndNoun(NounPhraseItem adj, NounPhraseItem noun)
        {
            foreach (NounPhraseItemTextVar v in adj.AdjMorph) 
            {
                foreach (NounPhraseItemTextVar vv in noun.NounMorph) 
                {
                    if (v.CheckAccord(vv, false, false)) 
                        return true;
                }
            }
            return false;
        }
        internal static void Initialize()
        {
            if (m_StdAdjectives != null) 
                return;
            m_StdAdjectives = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"СЕВЕРНЫЙ", "ЮЖНЫЙ", "ЗАПАДНЫЙ", "ВОСТОЧНЫЙ"}) 
            {
                m_StdAdjectives.Add(new Pullenti.Ner.Core.Termin(s));
            }
        }
        static Pullenti.Ner.Core.TerminCollection m_StdAdjectives;
    }
}