/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Работа с глагольными группами (последовательность из глаголов и наречий)
    /// </summary>
    public static class VerbPhraseHelper
    {
        /// <summary>
        /// Создать глагольную группу
        /// </summary>
        /// <param name="t">первый токен группы</param>
        /// <param name="canBePartition">выделять ли причастия</param>
        /// <param name="canBeAdjPartition">это бывают чистые прилагательные используются в режиме причастий (действия, опасные для жизни)</param>
        /// <param name="forceParse">всегда ли пытаться выделять, даже при сомнительных случаях (false по умолчанию)</param>
        /// <return>группа или null</return>
        public static VerbPhraseToken TryParse(Pullenti.Ner.Token t, bool canBePartition = false, bool canBeAdjPartition = false, bool forceParse = false)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            if (!t.Chars.IsLetter) 
                return null;
            if (t.Chars.IsCyrillicLetter) 
                return TryParseRu(t, canBePartition, canBeAdjPartition, forceParse);
            return null;
        }
        static VerbPhraseToken TryParseRu(Pullenti.Ner.Token t, bool canBePartition, bool canBeAdjPartition, bool forceParse)
        {
            VerbPhraseToken res = null;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token not = null;
            bool hasVerb = false;
            bool verbBeBefore = false;
            PrepositionToken prep = null;
            for (; t != null; t = t.Next) 
            {
                if (!(t is Pullenti.Ner.TextToken)) 
                    break;
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                bool isParticiple = false;
                if (tt.Term == "НЕ") 
                {
                    not = t;
                    continue;
                }
                int ty = 0;
                string norm = null;
                Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                if (tt.Term == "НЕТ") 
                {
                    if (hasVerb) 
                        break;
                    ty = 1;
                }
                else if (tt.Term == "ДОПУСТИМО") 
                    ty = 3;
                else if (mc.IsAdverb && !mc.IsVerb) 
                    ty = 2;
                else if (tt.IsPureVerb || tt.IsVerbBe) 
                {
                    ty = 1;
                    if (hasVerb) 
                    {
                        if (!tt.Morph.ContainsAttr("инф.", null)) 
                        {
                            if (verbBeBefore) 
                            {
                            }
                            else 
                                break;
                        }
                    }
                }
                else if (mc.IsVerb) 
                {
                    if (mc.IsPreposition || mc.IsMisc || mc.IsPronoun) 
                    {
                    }
                    else if (mc.IsNoun) 
                    {
                        if (tt.Term == "СТАЛИ" || tt.Term == "СТЕКЛО" || tt.Term == "БЫЛИ") 
                            ty = 1;
                        else if (!tt.Chars.IsAllLower && !MiscHelper.CanBeStartOfSentence(tt)) 
                            ty = 1;
                        else if (mc.IsAdjective && canBePartition) 
                            ty = 1;
                        else if (forceParse) 
                            ty = 1;
                    }
                    else if (mc.IsProper) 
                    {
                        if (tt.Chars.IsAllLower) 
                            ty = 1;
                    }
                    else 
                        ty = 1;
                    if (mc.IsAdjective) 
                        isParticiple = true;
                    if (!tt.Morph.Case.IsUndefined) 
                        isParticiple = true;
                    if (!canBePartition && isParticiple) 
                        break;
                    if (hasVerb) 
                    {
                        if (tt.Morph.ContainsAttr("инф.", null)) 
                        {
                        }
                        else if (!isParticiple) 
                        {
                        }
                        else 
                            break;
                    }
                }
                else if ((mc.IsAdjective && tt.Morph.ContainsAttr("к.ф.", null) && tt.Term.EndsWith("О")) && NounPhraseHelper.TryParse(tt, NounPhraseParseAttr.No, 0, null) == null) 
                    ty = 2;
                else if (mc.IsAdjective && ((canBePartition || canBeAdjPartition))) 
                {
                    if (tt.Morph.ContainsAttr("к.ф.", null) && !canBeAdjPartition) 
                        break;
                    norm = tt.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Masculine, false);
                    if (norm.EndsWith("ЙШИЙ")) 
                    {
                    }
                    else 
                    {
                        List<Pullenti.Semantic.Utils.DerivateGroup> grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(norm, true, null);
                        if (grs != null && grs.Count > 0) 
                        {
                            bool hVerb = false;
                            bool hPart = false;
                            foreach (Pullenti.Semantic.Utils.DerivateGroup gr in grs) 
                            {
                                foreach (Pullenti.Semantic.Utils.DerivateWord w in gr.Words) 
                                {
                                    if (w.Class.IsAdjective && w.Class.IsVerb) 
                                    {
                                        if (w.Spelling == norm) 
                                            hPart = true;
                                    }
                                    else if (w.Class.IsVerb) 
                                        hVerb = true;
                                }
                            }
                            if (hPart && hVerb) 
                                ty = 3;
                            else if (canBeAdjPartition) 
                                ty = 3;
                            if (ty != 3 && !string.IsNullOrEmpty(grs[0].Prefix) && norm.StartsWith(grs[0].Prefix)) 
                            {
                                hVerb = false;
                                hPart = false;
                                string norm1 = norm.Substring(grs[0].Prefix.Length);
                                grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(norm1, true, null);
                                if (grs != null && grs.Count > 0) 
                                {
                                    foreach (Pullenti.Semantic.Utils.DerivateGroup gr in grs) 
                                    {
                                        foreach (Pullenti.Semantic.Utils.DerivateWord w in gr.Words) 
                                        {
                                            if (w.Class.IsAdjective && w.Class.IsVerb) 
                                            {
                                                if (w.Spelling == norm1) 
                                                    hPart = true;
                                            }
                                            else if (w.Class.IsVerb) 
                                                hVerb = true;
                                        }
                                    }
                                }
                                if (hPart && hVerb) 
                                    ty = 3;
                            }
                        }
                    }
                }
                if (ty == 0 && t == t0 && canBePartition) 
                {
                    prep = PrepositionHelper.TryParse(t);
                    if (prep != null) 
                    {
                        t = prep.EndToken;
                        continue;
                    }
                }
                if (ty == 0) 
                    break;
                if (res == null) 
                    res = new VerbPhraseToken(t0, t);
                res.EndToken = t;
                VerbPhraseItemToken it = new VerbPhraseItemToken(t, t) { Morph = new Pullenti.Ner.MorphCollection(t.Morph) };
                if (not != null) 
                {
                    it.BeginToken = not;
                    it.Not = true;
                    not = null;
                }
                it.IsAdverb = ty == 2;
                if (prep != null && !t.Morph.Case.IsUndefined && res.Items.Count == 0) 
                {
                    if (((prep.NextCase & t.Morph.Case)).IsUndefined) 
                        return null;
                    it.Morph.RemoveItems(prep.NextCase);
                    res.Preposition = prep;
                }
                if (norm == null) 
                {
                    norm = t.GetNormalCaseText((ty == 3 ? Pullenti.Morph.MorphClass.Adjective : (ty == 2 ? Pullenti.Morph.MorphClass.Adverb : Pullenti.Morph.MorphClass.Verb)), Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Masculine, false);
                    if (ty == 1 && !tt.Morph.Case.IsUndefined) 
                    {
                        Pullenti.Morph.MorphWordForm mi = new Pullenti.Morph.MorphWordForm() { Case = Pullenti.Morph.MorphCase.Nominative, Number = Pullenti.Morph.MorphNumber.Singular, Gender = Pullenti.Morph.MorphGender.Masculine };
                        foreach (Pullenti.Morph.MorphBaseInfo mit in tt.Morph.Items) 
                        {
                            if (mit is Pullenti.Morph.MorphWordForm) 
                            {
                                mi.Misc = (mit as Pullenti.Morph.MorphWordForm).Misc;
                                break;
                            }
                        }
                        string nnn = Pullenti.Morph.MorphologyService.GetWordform("КК" + (t as Pullenti.Ner.TextToken).Term, mi);
                        if (nnn != null) 
                            norm = nnn.Substring(2);
                    }
                }
                it.Normal = norm;
                res.Items.Add(it);
                if (!hasVerb && ((ty == 1 || ty == 3))) 
                {
                    res.Morph = it.Morph;
                    hasVerb = true;
                }
                if (ty == 1 || ty == 3) 
                {
                    if (ty == 1 && tt.IsVerbBe) 
                        verbBeBefore = true;
                    else 
                        verbBeBefore = false;
                }
            }
            if (!hasVerb) 
                return null;
            for (int i = res.Items.Count - 1; i > 0; i--) 
            {
                if (res.Items[i].IsAdverb) 
                {
                    res.Items.RemoveAt(i);
                    res.EndToken = res.Items[i - 1].EndToken;
                }
                else 
                    break;
            }
            return res;
        }
    }
}