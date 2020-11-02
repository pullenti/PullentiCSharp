/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Semantic.Internal
{
    static class CreateHelper
    {
        public static void _setMorph(Pullenti.Semantic.SemObject obj, Pullenti.Morph.MorphWordForm wf)
        {
            if (wf == null) 
                return;
            obj.Morph.NormalCase = wf.NormalCase;
            obj.Morph.NormalFull = wf.NormalFull ?? wf.NormalCase;
            obj.Morph.Number = wf.Number;
            obj.Morph.Gender = wf.Gender;
            obj.Morph.Misc = wf.Misc;
        }
        public static void _setMorph0(Pullenti.Semantic.SemObject obj, Pullenti.Morph.MorphBaseInfo bi)
        {
            obj.Morph.Number = bi.Number;
            obj.Morph.Gender = bi.Gender;
        }
        public static Pullenti.Semantic.SemObject CreateNounGroup(Pullenti.Semantic.SemGraph gr, Pullenti.Ner.Core.NounPhraseToken npt)
        {
            Pullenti.Ner.Token noun = npt.Noun.BeginToken;
            Pullenti.Semantic.SemObject sem = new Pullenti.Semantic.SemObject(gr);
            sem.Tokens.Add(npt.Noun);
            sem.Typ = Pullenti.Semantic.SemObjectType.Noun;
            if (npt.Noun.Morph.Class.IsPersonalPronoun) 
                sem.Typ = Pullenti.Semantic.SemObjectType.PersonalPronoun;
            else if (npt.Noun.Morph.Class.IsPronoun) 
                sem.Typ = Pullenti.Semantic.SemObjectType.Pronoun;
            if (npt.Noun.BeginToken != npt.Noun.EndToken) 
            {
                sem.Morph.NormalCase = npt.Noun.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                sem.Morph.NormalFull = npt.Noun.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                sem.Morph.Class = Pullenti.Morph.MorphClass.Noun;
                sem.Morph.Number = npt.Morph.Number;
                sem.Morph.Gender = npt.Morph.Gender;
                sem.Morph.Case = npt.Morph.Case;
            }
            else if (noun is Pullenti.Ner.TextToken) 
            {
                foreach (Pullenti.Morph.MorphBaseInfo wf in noun.Morph.Items) 
                {
                    if (wf.CheckAccord(npt.Morph, false, false) && (wf is Pullenti.Morph.MorphWordForm)) 
                    {
                        _setMorph(sem, wf as Pullenti.Morph.MorphWordForm);
                        break;
                    }
                }
                if (sem.Morph.NormalCase == null) 
                {
                    sem.Morph.NormalCase = noun.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                    sem.Morph.NormalFull = noun.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                }
                List<Pullenti.Semantic.Utils.DerivateGroup> grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(sem.Morph.NormalFull, true, null);
                if (grs != null && grs.Count > 0) 
                    sem.Concept = grs[0];
            }
            else if (noun is Pullenti.Ner.ReferentToken) 
            {
                Pullenti.Ner.Referent r = (noun as Pullenti.Ner.ReferentToken).Referent;
                if (r == null) 
                    return null;
                sem.Morph.NormalFull = (sem.Morph.NormalCase = r.ToString());
                sem.Concept = r;
            }
            else if (noun is Pullenti.Ner.NumberToken) 
            {
                Pullenti.Ner.NumberToken num = noun as Pullenti.Ner.NumberToken;
                sem.Morph.Gender = noun.Morph.Gender;
                sem.Morph.Number = noun.Morph.Number;
                if (num.IntValue != null) 
                {
                    sem.Morph.NormalCase = Pullenti.Ner.Core.NumberHelper.GetNumberAdjective(num.IntValue.Value, noun.Morph.Gender, noun.Morph.Number);
                    sem.Morph.NormalFull = Pullenti.Ner.Core.NumberHelper.GetNumberAdjective(num.IntValue.Value, Pullenti.Morph.MorphGender.Masculine, Pullenti.Morph.MorphNumber.Singular);
                }
                else 
                    sem.Morph.NormalFull = (sem.Morph.NormalCase = noun.GetSourceText().ToUpper());
            }
            noun.Tag = sem;
            if (npt.Adjectives.Count > 0) 
            {
                foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
                {
                    if (npt.MultiNouns && a != npt.Adjectives[0]) 
                        break;
                    Pullenti.Semantic.SemObject asem = CreateNptAdj(gr, npt, a);
                    if (asem != null) 
                        gr.AddLink(Pullenti.Semantic.SemLinkType.Detail, sem, asem, "какой", false, null);
                }
            }
            if (npt.InternalNoun != null) 
            {
                Pullenti.Semantic.SemObject intsem = CreateNounGroup(gr, npt.InternalNoun);
                if (intsem != null) 
                    gr.AddLink(Pullenti.Semantic.SemLinkType.Detail, sem, intsem, null, false, null);
            }
            gr.Objects.Add(sem);
            return sem;
        }
        public static Pullenti.Semantic.SemObject CreateNumber(Pullenti.Semantic.SemGraph gr, Pullenti.Ner.Measure.Internal.NumbersWithUnitToken num)
        {
            List<Pullenti.Ner.ReferentToken> rs = num.CreateRefenetsTokensWithRegister(null, null, false);
            if (rs == null || rs.Count == 0) 
                return null;
            Pullenti.Ner.Measure.MeasureReferent mr = rs[rs.Count - 1].Referent as Pullenti.Ner.Measure.MeasureReferent;
            Pullenti.Semantic.SemObject sem = new Pullenti.Semantic.SemObject(gr);
            gr.Objects.Add(sem);
            sem.Tokens.Add(num);
            sem.Morph.NormalFull = (sem.Morph.NormalCase = mr.ToString(true, null, 0));
            sem.Typ = Pullenti.Semantic.SemObjectType.Noun;
            sem.Measure = mr.Kind;
            for (int i = 0; i < sem.Morph.NormalCase.Length; i++) 
            {
                char ch = sem.Morph.NormalCase[i];
                if (char.IsDigit(ch) || char.IsWhiteSpace(ch) || "[].+-".IndexOf(ch) >= 0) 
                    continue;
                sem.Quantity = new Pullenti.Semantic.SemQuantity(sem.Morph.NormalCase.Substring(0, i).Trim(), num.BeginToken, num.EndToken);
                sem.Morph.NormalCase = sem.Morph.NormalCase.Substring(i).Trim();
                if (num.Units.Count == 1 && num.Units[0].Unit != null) 
                {
                    sem.Morph.NormalFull = num.Units[0].Unit.FullnameCyr;
                    if (sem.Morph.NormalFull == "%") 
                        sem.Morph.NormalFull = "процент";
                }
                break;
            }
            sem.Concept = mr;
            return sem;
        }
        public static Pullenti.Semantic.SemObject CreateAdverb(Pullenti.Semantic.SemGraph gr, AdverbToken adv)
        {
            Pullenti.Semantic.SemObject res = new Pullenti.Semantic.SemObject(gr);
            gr.Objects.Add(res);
            res.Tokens.Add(adv);
            res.Typ = Pullenti.Semantic.SemObjectType.Adverb;
            res.Not = adv.Not;
            res.Morph.NormalCase = (res.Morph.NormalFull = adv.Spelling);
            List<Pullenti.Semantic.Utils.DerivateGroup> grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(res.Morph.NormalFull, true, null);
            if (grs != null && grs.Count > 0) 
                res.Concept = grs[0];
            return res;
        }
        public static Pullenti.Semantic.SemObject CreateNptAdj(Pullenti.Semantic.SemGraph gr, Pullenti.Ner.Core.NounPhraseToken npt, Pullenti.Ner.MetaToken a)
        {
            if (a.Morph.Class.IsPronoun) 
            {
                Pullenti.Semantic.SemObject asem = new Pullenti.Semantic.SemObject(gr);
                gr.Objects.Add(asem);
                asem.Tokens.Add(a);
                asem.Typ = (a.BeginToken.Morph.Class.IsPersonalPronoun ? Pullenti.Semantic.SemObjectType.PersonalPronoun : Pullenti.Semantic.SemObjectType.Pronoun);
                foreach (Pullenti.Morph.MorphBaseInfo it in a.BeginToken.Morph.Items) 
                {
                    Pullenti.Morph.MorphWordForm wf = it as Pullenti.Morph.MorphWordForm;
                    if (wf == null) 
                        continue;
                    if (!npt.Morph.Case.IsUndefined) 
                    {
                        if (((npt.Morph.Case & wf.Case)).IsUndefined) 
                            continue;
                    }
                    _setMorph(asem, wf);
                    if (asem.Morph.NormalFull == "КАКОВ") 
                        asem.Morph.NormalFull = "КАКОЙ";
                    break;
                }
                if (asem.Morph.NormalFull == null) 
                    asem.Morph.NormalFull = (asem.Morph.NormalCase = a.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                return asem;
            }
            if (!a.Morph.Class.IsVerb) 
            {
                Pullenti.Semantic.SemObject asem = new Pullenti.Semantic.SemObject(gr);
                gr.Objects.Add(asem);
                asem.Tokens.Add(a);
                asem.Typ = Pullenti.Semantic.SemObjectType.Adjective;
                foreach (Pullenti.Morph.MorphBaseInfo wf in a.BeginToken.Morph.Items) 
                {
                    if (wf.CheckAccord(npt.Morph, false, false) && wf.Class.IsAdjective && (wf is Pullenti.Morph.MorphWordForm)) 
                    {
                        _setMorph(asem, wf as Pullenti.Morph.MorphWordForm);
                        break;
                    }
                }
                if (asem.Morph.NormalCase == null) 
                {
                    asem.Morph.NormalCase = a.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                    asem.Morph.NormalFull = a.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Masculine, false);
                    _setMorph0(asem, a.BeginToken.Morph);
                }
                List<Pullenti.Semantic.Utils.DerivateGroup> grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(asem.Morph.NormalFull, true, null);
                if (grs != null && grs.Count > 0) 
                    asem.Concept = grs[0];
                return asem;
            }
            return null;
        }
        public static Pullenti.Semantic.SemObject CreateVerbGroup(Pullenti.Semantic.SemGraph gr, Pullenti.Ner.Core.VerbPhraseToken vpt)
        {
            List<Pullenti.Semantic.SemObject> sems = new List<Pullenti.Semantic.SemObject>();
            List<Pullenti.Semantic.SemAttribute> attrs = new List<Pullenti.Semantic.SemAttribute>();
            List<Pullenti.Semantic.SemObject> adverbs = new List<Pullenti.Semantic.SemObject>();
            for (int i = 0; i < vpt.Items.Count; i++) 
            {
                Pullenti.Ner.Core.VerbPhraseItemToken v = vpt.Items[i];
                if (v.IsAdverb) 
                {
                    AdverbToken adv = AdverbToken.TryParse(v.BeginToken);
                    if (adv == null) 
                        continue;
                    if (adv.Typ != Pullenti.Semantic.SemAttributeType.Undefined) 
                    {
                        attrs.Add(new Pullenti.Semantic.SemAttribute() { Not = adv.Not, Typ = adv.Typ, Spelling = adv.Spelling });
                        continue;
                    }
                    Pullenti.Semantic.SemObject adverb = CreateAdverb(gr, adv);
                    if (attrs.Count > 0) 
                    {
                        adverb.Attrs.AddRange(attrs);
                        attrs.Clear();
                    }
                    adverbs.Add(adverb);
                    continue;
                }
                if (v.Normal == "БЫТЬ") 
                {
                    int j;
                    for (j = i + 1; j < vpt.Items.Count; j++) 
                    {
                        if (!vpt.Items[j].IsAdverb) 
                            break;
                    }
                    if (j < vpt.Items.Count) 
                        continue;
                }
                Pullenti.Semantic.SemObject sem = new Pullenti.Semantic.SemObject(gr);
                gr.Objects.Add(sem);
                sem.Tokens.Add(v);
                v.Tag = sem;
                _setMorph(sem, v.VerbMorph);
                sem.Morph.NormalCase = (sem.Morph.NormalFull = v.Normal);
                if (v.IsParticiple || v.IsDeeParticiple) 
                {
                    sem.Typ = Pullenti.Semantic.SemObjectType.Participle;
                    sem.Morph.NormalFull = v.EndToken.GetNormalCaseText(Pullenti.Morph.MorphClass.Verb, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) ?? sem.Morph.NormalCase;
                    sem.Morph.NormalCase = v.EndToken.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                    if (sem.Morph.NormalCase == sem.Morph.NormalFull && v.Normal.EndsWith("Й")) 
                    {
                        List<Pullenti.Semantic.Utils.DerivateGroup> grs2 = Pullenti.Semantic.Utils.DerivateService.FindDerivates(v.Normal, true, null);
                        if (grs2 != null) 
                        {
                            foreach (Pullenti.Semantic.Utils.DerivateGroup g in grs2) 
                            {
                                foreach (Pullenti.Semantic.Utils.DerivateWord w in g.Words) 
                                {
                                    if (w.Lang == v.EndToken.Morph.Language && w.Class.IsVerb && !w.Class.IsAdjective) 
                                    {
                                        sem.Morph.NormalFull = w.Spelling;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (sem.Morph.NormalCase == sem.Morph.NormalFull && v.IsParticiple && sem.Morph.NormalFull.EndsWith("Ь")) 
                    {
                        foreach (Pullenti.Morph.MorphBaseInfo it in v.EndToken.Morph.Items) 
                        {
                            Pullenti.Morph.MorphWordForm wf = it as Pullenti.Morph.MorphWordForm;
                            if (wf == null) 
                                continue;
                            if (wf.NormalCase.EndsWith("Й") || ((wf.NormalFull != null && wf.NormalFull.EndsWith("Й")))) 
                            {
                                sem.Morph.NormalCase = wf.NormalFull ?? wf.NormalCase;
                                break;
                            }
                        }
                        if (sem.Morph.NormalCase == sem.Morph.NormalFull) 
                        {
                            List<Pullenti.Semantic.Utils.DerivateGroup> grs2 = Pullenti.Semantic.Utils.DerivateService.FindDerivates(sem.Morph.NormalCase, true, null);
                            if (grs2 != null) 
                            {
                                foreach (Pullenti.Semantic.Utils.DerivateGroup g in grs2) 
                                {
                                    foreach (Pullenti.Semantic.Utils.DerivateWord w in g.Words) 
                                    {
                                        if (w.Lang == v.EndToken.Morph.Language && w.Class.IsVerb && w.Class.IsAdjective) 
                                        {
                                            sem.Morph.NormalCase = w.Spelling;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                else 
                    sem.Typ = Pullenti.Semantic.SemObjectType.Verb;
                if (v.VerbMorph != null && v.VerbMorph.ContainsAttr("возвр.", null)) 
                {
                    if (sem.Morph.NormalFull.EndsWith("СЯ") || sem.Morph.NormalFull.EndsWith("СЬ")) 
                        sem.Morph.NormalFull = sem.Morph.NormalFull.Substring(0, sem.Morph.NormalFull.Length - 2);
                }
                List<Pullenti.Semantic.Utils.DerivateGroup> grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(sem.Morph.NormalFull, true, null);
                if (grs != null && grs.Count > 0) 
                {
                    sem.Concept = grs[0];
                    if (v.VerbMorph != null && v.VerbMorph.Misc.Aspect == Pullenti.Morph.MorphAspect.Imperfective) 
                    {
                        foreach (Pullenti.Semantic.Utils.DerivateWord w in grs[0].Words) 
                        {
                            if (w.Class.IsVerb && !w.Class.IsAdjective) 
                            {
                                if (w.Aspect == Pullenti.Morph.MorphAspect.Perfective) 
                                {
                                    sem.Morph.NormalFull = w.Spelling;
                                    break;
                                }
                            }
                        }
                    }
                }
                sem.Not = v.Not;
                sems.Add(sem);
                if (attrs.Count > 0) 
                {
                    sem.Attrs.AddRange(attrs);
                    attrs.Clear();
                }
                if (adverbs.Count > 0) 
                {
                    foreach (Pullenti.Semantic.SemObject a in adverbs) 
                    {
                        gr.AddLink(Pullenti.Semantic.SemLinkType.Detail, sem, a, "как", false, null);
                    }
                }
                adverbs.Clear();
            }
            if (sems.Count == 0) 
                return null;
            if (attrs.Count > 0) 
                sems[sems.Count - 1].Attrs.AddRange(attrs);
            if (adverbs.Count > 0) 
            {
                Pullenti.Semantic.SemObject sem = sems[sems.Count - 1];
                foreach (Pullenti.Semantic.SemObject a in adverbs) 
                {
                    gr.AddLink(Pullenti.Semantic.SemLinkType.Detail, sem, a, "как", false, null);
                }
            }
            for (int i = sems.Count - 1; i > 0; i--) 
            {
                gr.AddLink(Pullenti.Semantic.SemLinkType.Detail, sems[i - 1], sems[i], "что делать", false, null);
            }
            return sems[0];
        }
        public static string CreateQuestion(NGItem li)
        {
            string res = ((li.Source.Prep ?? "")).ToLower();
            if (res.Length > 0) 
                res += " ";
            Pullenti.Morph.MorphCase cas = li.Source.Source.Morph.Case;
            if (!string.IsNullOrEmpty(li.Source.Prep)) 
            {
                Pullenti.Morph.MorphCase cas1 = Pullenti.Morph.LanguageHelper.GetCaseAfterPreposition(li.Source.Prep);
                if (!cas1.IsUndefined) 
                {
                    if (!((cas1 & cas)).IsUndefined) 
                        cas = cas & cas1;
                }
            }
            if (cas.IsGenitive) 
                res += "чего";
            else if (cas.IsInstrumental) 
                res += "чем";
            else if (cas.IsDative) 
                res += "чему";
            else if (cas.IsAccusative) 
                res += "что";
            else if (cas.IsPrepositional) 
                res += "чём";
            return res;
        }
    }
}