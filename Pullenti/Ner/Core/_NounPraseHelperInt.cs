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
    class _NounPraseHelperInt
    {
        public static NounPhraseToken TryParse(Pullenti.Ner.Token first, NounPhraseParseAttr typ, int maxCharPos, Pullenti.Ner.Core.Internal.NounPhraseItem noun)
        {
            if (first == null) 
                return null;
            if (first.NotNounPhrase) 
            {
                if (((typ & ((((NounPhraseParseAttr.IgnoreParticiples | NounPhraseParseAttr.ReferentCanBeNoun | NounPhraseParseAttr.ParsePronouns) | NounPhraseParseAttr.ParseAdverbs | NounPhraseParseAttr.ParseNumericAsAdjective) | NounPhraseParseAttr.IgnoreBrackets)))) == NounPhraseParseAttr.No) 
                    return null;
            }
            int cou = 0;
            for (Pullenti.Ner.Token t = first; t != null; t = t.Next) 
            {
                if (maxCharPos > 0 && t.BeginChar > maxCharPos) 
                    break;
                if (t.Morph.Language.IsCyrillic || (((t is Pullenti.Ner.NumberToken) && t.Morph.Class.IsAdjective && !t.Chars.IsLatinLetter)) || (((t is Pullenti.Ner.ReferentToken) && ((typ & NounPhraseParseAttr.ReferentCanBeNoun)) != NounPhraseParseAttr.No && !t.Chars.IsLatinLetter))) 
                {
                    NounPhraseToken res = TryParseRu(first, typ, maxCharPos, noun);
                    if (res == null) 
                        first.NotNounPhrase = true;
                    return res;
                }
                else if (t.Chars.IsLatinLetter) 
                {
                    NounPhraseToken res = TryParseEn(first, typ, maxCharPos);
                    if (res == null) 
                        first.NotNounPhrase = true;
                    return res;
                }
                else if ((++cou) > 0) 
                    break;
            }
            return null;
        }
        static NounPhraseToken TryParseRu(Pullenti.Ner.Token first, NounPhraseParseAttr typ, int maxCharPos, Pullenti.Ner.Core.Internal.NounPhraseItem defNoun = null)
        {
            if (first == null) 
                return null;
            List<Pullenti.Ner.Core.Internal.NounPhraseItem> items = null;
            List<Pullenti.Ner.TextToken> adverbs = null;
            PrepositionToken prep = null;
            bool kak = false;
            Pullenti.Ner.Token t0 = first;
            if (((typ & NounPhraseParseAttr.ParsePreposition)) != NounPhraseParseAttr.No && t0.IsValue("КАК", null)) 
            {
                t0 = t0.Next;
                prep = PrepositionHelper.TryParse(t0);
                if (prep != null) 
                    t0 = prep.EndToken.Next;
                kak = true;
            }
            NounPhraseToken internalNounPrase = null;
            bool conjBefore = false;
            for (Pullenti.Ner.Token t = t0; t != null; t = t.Next) 
            {
                if (maxCharPos > 0 && t.BeginChar > maxCharPos) 
                    break;
                if ((t.Morph.Class.IsConjunction && !t.Morph.Class.IsAdjective && !t.Morph.Class.IsPronoun) && !t.Morph.Class.IsNoun) 
                {
                    if (conjBefore) 
                        break;
                    if (((typ & NounPhraseParseAttr.CanNotHasCommaAnd)) != NounPhraseParseAttr.No) 
                        break;
                    if (items != null && ((t.IsAnd || t.IsOr))) 
                    {
                        conjBefore = true;
                        if ((t.Next != null && t.Next.IsCharOf("\\/") && t.Next.Next != null) && t.Next.Next.IsOr) 
                            t = t.Next.Next;
                        if (((t.Next != null && t.Next.IsChar('(') && t.Next.Next != null) && t.Next.Next.IsOr && t.Next.Next.Next != null) && t.Next.Next.Next.IsChar(')')) 
                            t = t.Next.Next.Next;
                        continue;
                    }
                    break;
                }
                else if (t.IsComma) 
                {
                    if (conjBefore || items == null) 
                        break;
                    if (((typ & NounPhraseParseAttr.CanNotHasCommaAnd)) != NounPhraseParseAttr.No) 
                        break;
                    Pullenti.Morph.MorphClass mc = t.Previous.GetMorphClassInDictionary();
                    if (mc.IsProperSurname || mc.IsProperSecname) 
                        break;
                    conjBefore = true;
                    if (kak && t.Next != null && t.Next.IsValue("ТАК", null)) 
                    {
                        t = t.Next;
                        if (t.Next != null && t.Next.IsAnd) 
                            t = t.Next;
                        PrepositionToken pr = PrepositionHelper.TryParse(t.Next);
                        if (pr != null) 
                            t = pr.EndToken;
                    }
                    if (items[items.Count - 1].CanBeNoun && items[items.Count - 1].EndToken.Morph.Class.IsPronoun) 
                        break;
                    continue;
                }
                else if (t.IsChar('(')) 
                {
                    if (items == null) 
                        return null;
                    BracketSequenceToken brr = BracketHelper.TryParse(t, BracketParseAttr.No, 100);
                    if (brr == null) 
                        break;
                    if (brr.LengthChar > 100) 
                        break;
                    t = brr.EndToken;
                    continue;
                }
                if (t is Pullenti.Ner.ReferentToken) 
                {
                    if (((typ & NounPhraseParseAttr.ReferentCanBeNoun)) == NounPhraseParseAttr.No) 
                        break;
                }
                else if (t.Chars.IsLatinLetter) 
                    break;
                Pullenti.Ner.Core.Internal.NounPhraseItem it = Pullenti.Ner.Core.Internal.NounPhraseItem.TryParse(t, items, typ);
                if (it == null || ((!it.CanBeAdj && !it.CanBeNoun))) 
                {
                    if (((it != null && items != null && t.Chars.IsCapitalUpper) && (t.WhitespacesBeforeCount < 3) && t.LengthChar > 3) && !t.GetMorphClassInDictionary().IsNoun && !t.GetMorphClassInDictionary().IsAdjective) 
                    {
                        it.CanBeNoun = true;
                        items.Add(it);
                        break;
                    }
                    if (((typ & NounPhraseParseAttr.ParseAdverbs)) != NounPhraseParseAttr.No && (t is Pullenti.Ner.TextToken) && t.Morph.Class.IsAdverb) 
                    {
                        if (adverbs == null) 
                            adverbs = new List<Pullenti.Ner.TextToken>();
                        adverbs.Add(t as Pullenti.Ner.TextToken);
                        continue;
                    }
                    break;
                }
                it.ConjBefore = conjBefore;
                conjBefore = false;
                if (!it.CanBeAdj && !it.CanBeNoun) 
                    break;
                if (t.IsNewlineBefore && t != first) 
                {
                    if (((typ & NounPhraseParseAttr.Multilines)) != NounPhraseParseAttr.No) 
                    {
                    }
                    else if (items != null && t.Chars != items[items.Count - 1].Chars) 
                    {
                        if (t.Chars.IsAllLower && items[items.Count - 1].Chars.IsCapitalUpper) 
                        {
                        }
                        else 
                            break;
                    }
                }
                if (items == null) 
                    items = new List<Pullenti.Ner.Core.Internal.NounPhraseItem>();
                else 
                {
                    Pullenti.Ner.Core.Internal.NounPhraseItem it0 = items[items.Count - 1];
                    if (it0.CanBeNoun && it0.IsPersonalPronoun) 
                    {
                        if (it.IsPronoun) 
                            break;
                        if ((it0.BeginToken.Previous != null && it0.BeginToken.Previous.GetMorphClassInDictionary().IsVerb && !it0.BeginToken.Previous.GetMorphClassInDictionary().IsAdjective) && !it0.BeginToken.Previous.GetMorphClassInDictionary().IsPreposition) 
                        {
                            if (t.Morph.Case.IsNominative || t.Morph.Case.IsAccusative) 
                            {
                            }
                            else 
                                break;
                        }
                        if (it.CanBeNoun && it.IsVerb) 
                        {
                            if (it0.Previous == null) 
                            {
                            }
                            else if ((it0.Previous is Pullenti.Ner.TextToken) && !it0.Previous.Chars.IsLetter) 
                            {
                            }
                            else 
                                break;
                        }
                    }
                }
                items.Add(it);
                t = it.EndToken;
                if (t.IsNewlineAfter && !t.Chars.IsAllLower) 
                {
                    Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                    if (mc.IsProperSurname) 
                        break;
                    if (t.Morph.Class.IsProperSurname && mc.IsUndefined) 
                        break;
                }
            }
            if (items == null) 
                return null;
            Pullenti.Ner.Token tt1;
            if (items.Count == 1 && items[0].CanBeAdj) 
            {
                bool and = false;
                for (tt1 = items[0].EndToken.Next; tt1 != null; tt1 = tt1.Next) 
                {
                    if (tt1.IsAnd || tt1.IsOr) 
                    {
                        and = true;
                        break;
                    }
                    if (tt1.IsComma || tt1.IsValue("НО", null) || tt1.IsValue("ТАК", null)) 
                        continue;
                    break;
                }
                if (and) 
                {
                    if (items[0].CanBeNoun && items[0].IsPersonalPronoun) 
                        and = false;
                }
                if (and) 
                {
                    Pullenti.Ner.Token tt2 = tt1.Next;
                    if (tt2 != null && tt2.Morph.Class.IsPreposition) 
                        tt2 = tt2.Next;
                    NounPhraseToken npt1 = TryParseRu(tt2, typ, maxCharPos, null);
                    if (npt1 != null && npt1.Adjectives.Count > 0) 
                    {
                        bool ok1 = false;
                        foreach (Pullenti.Ner.Core.Internal.NounPhraseItemTextVar av in items[0].AdjMorph) 
                        {
                            foreach (Pullenti.Ner.Core.Internal.NounPhraseItemTextVar v in (npt1.Noun as Pullenti.Ner.Core.Internal.NounPhraseItem).NounMorph) 
                            {
                                if (v.CheckAccord(av, false, false)) 
                                {
                                    items[0].Morph.AddItem(av);
                                    ok1 = true;
                                }
                            }
                        }
                        if (ok1) 
                        {
                            npt1.BeginToken = items[0].BeginToken;
                            npt1.EndToken = tt1.Previous;
                            npt1.Adjectives.Clear();
                            npt1.Adjectives.Add(items[0]);
                            return npt1;
                        }
                    }
                }
            }
            if (defNoun != null) 
                items.Add(defNoun);
            Pullenti.Ner.Core.Internal.NounPhraseItem last1 = items[items.Count - 1];
            bool check = true;
            foreach (Pullenti.Ner.Core.Internal.NounPhraseItem it in items) 
            {
                if (!it.CanBeAdj) 
                {
                    check = false;
                    break;
                }
                else if (it.CanBeNoun && it.IsPersonalPronoun) 
                {
                    check = false;
                    break;
                }
            }
            tt1 = last1.EndToken.Next;
            if ((tt1 != null && check && ((tt1.Morph.Class.IsPreposition || tt1.Morph.Case.IsInstrumental))) && (tt1.WhitespacesBeforeCount < 2)) 
            {
                NounPhraseToken inp = NounPhraseHelper.TryParse(tt1, typ | NounPhraseParseAttr.ParsePreposition, maxCharPos, null);
                if (inp != null) 
                {
                    tt1 = inp.EndToken.Next;
                    NounPhraseToken npt1 = TryParseRu(tt1, typ, maxCharPos, null);
                    if (npt1 != null) 
                    {
                        bool ok = true;
                        for (int ii = 0; ii < items.Count; ii++) 
                        {
                            Pullenti.Ner.Core.Internal.NounPhraseItem it = items[ii];
                            if (Pullenti.Ner.Core.Internal.NounPhraseItem.TryAccordAdjAndNoun(it, npt1.Noun as Pullenti.Ner.Core.Internal.NounPhraseItem)) 
                                continue;
                            if (ii > 0) 
                            {
                                NounPhraseToken inp2 = NounPhraseHelper.TryParse(it.BeginToken, typ, maxCharPos, null);
                                if (inp2 != null && inp2.EndToken == inp.EndToken) 
                                {
                                    items.RemoveRange(ii, items.Count - ii);
                                    inp = inp2;
                                    break;
                                }
                            }
                            ok = false;
                            break;
                        }
                        if (ok) 
                        {
                            if (npt1.Morph.Case.IsGenitive && !inp.Morph.Case.IsInstrumental) 
                                ok = false;
                        }
                        if (ok) 
                        {
                            for (int i = 0; i < items.Count; i++) 
                            {
                                npt1.Adjectives.Insert(i, items[i]);
                            }
                            npt1.InternalNoun = inp;
                            Pullenti.Ner.MorphCollection mmm = new Pullenti.Ner.MorphCollection(npt1.Morph);
                            foreach (Pullenti.Ner.Core.Internal.NounPhraseItem it in items) 
                            {
                                mmm.RemoveItems(it.AdjMorph[0]);
                            }
                            if (mmm.Gender != Pullenti.Morph.MorphGender.Undefined || mmm.Number != Pullenti.Morph.MorphNumber.Undefined || !mmm.Case.IsUndefined) 
                                npt1.Morph = mmm;
                            if (adverbs != null) 
                            {
                                if (npt1.Adverbs == null) 
                                    npt1.Adverbs = adverbs;
                                else 
                                    npt1.Adverbs.InsertRange(0, adverbs);
                            }
                            npt1.BeginToken = first;
                            return npt1;
                        }
                    }
                    if (tt1 != null && tt1.Morph.Class.IsNoun && !tt1.Morph.Case.IsGenitive) 
                    {
                        Pullenti.Ner.Core.Internal.NounPhraseItem it = Pullenti.Ner.Core.Internal.NounPhraseItem.TryParse(tt1, items, typ);
                        if (it != null && it.CanBeNoun) 
                        {
                            internalNounPrase = inp;
                            inp.BeginToken = items[0].EndToken.Next;
                            items.Add(it);
                        }
                    }
                }
            }
            for (int i = 0; i < items.Count; i++) 
            {
                if (items[i].CanBeAdj && items[i].BeginToken.Morph.Class.IsVerb) 
                {
                    Pullenti.Ner.Token it = items[i].BeginToken;
                    if (!it.GetMorphClassInDictionary().IsVerb) 
                        continue;
                    if (it.IsValue("УПОЛНОМОЧЕННЫЙ", null)) 
                        continue;
                    if (((typ & NounPhraseParseAttr.ParseVerbs)) == NounPhraseParseAttr.No) 
                        continue;
                    NounPhraseToken inp = TryParseRu(items[i].EndToken.Next, NounPhraseParseAttr.No, maxCharPos, null);
                    if (inp == null) 
                        continue;
                    if (inp.Anafor != null && i == (items.Count - 1) && Pullenti.Ner.Core.Internal.NounPhraseItem.TryAccordAdjAndNoun(items[i], inp.Noun as Pullenti.Ner.Core.Internal.NounPhraseItem)) 
                    {
                        inp.BeginToken = first;
                        for (int ii = 0; ii < items.Count; ii++) 
                        {
                            inp.Adjectives.Insert(ii, items[ii]);
                        }
                        return inp;
                    }
                    if (inp.EndToken.WhitespacesAfterCount > 3) 
                        continue;
                    NounPhraseToken npt1 = TryParseRu(inp.EndToken.Next, NounPhraseParseAttr.No, maxCharPos, null);
                    if (npt1 == null) 
                        continue;
                    bool ok = true;
                    for (int j = 0; j <= i; j++) 
                    {
                        if (!Pullenti.Ner.Core.Internal.NounPhraseItem.TryAccordAdjAndNoun(items[j], npt1.Noun as Pullenti.Ner.Core.Internal.NounPhraseItem)) 
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (!ok) 
                        continue;
                    VerbPhraseToken verb = VerbPhraseHelper.TryParse(it, true, false, false);
                    if (verb == null) 
                        continue;
                    List<Pullenti.Semantic.Core.SemanticLink> vlinks = Pullenti.Semantic.Core.SemanticHelper.TryCreateLinks(verb, inp, null);
                    List<Pullenti.Semantic.Core.SemanticLink> nlinks = Pullenti.Semantic.Core.SemanticHelper.TryCreateLinks(inp, npt1, null);
                    if (vlinks.Count == 0 && nlinks.Count > 0) 
                        continue;
                    for (int j = 0; j <= i; j++) 
                    {
                        npt1.Adjectives.Insert(j, items[j]);
                    }
                    items[i].EndToken = inp.EndToken;
                    Pullenti.Ner.MorphCollection mmm = new Pullenti.Ner.MorphCollection(npt1.Morph);
                    List<Pullenti.Morph.MorphBaseInfo> bil = new List<Pullenti.Morph.MorphBaseInfo>();
                    for (int j = 0; j <= i; j++) 
                    {
                        bil.Clear();
                        foreach (Pullenti.Ner.Core.Internal.NounPhraseItemTextVar m in items[j].AdjMorph) 
                        {
                            bil.Add(m);
                        }
                        mmm.RemoveItemsListCla(bil, null);
                    }
                    if (mmm.Gender != Pullenti.Morph.MorphGender.Undefined || mmm.Number != Pullenti.Morph.MorphNumber.Undefined || !mmm.Case.IsUndefined) 
                        npt1.Morph = mmm;
                    if (adverbs != null) 
                    {
                        if (npt1.Adverbs == null) 
                            npt1.Adverbs = adverbs;
                        else 
                            npt1.Adverbs.InsertRange(0, adverbs);
                    }
                    npt1.BeginToken = first;
                    return npt1;
                }
            }
            bool ok2 = false;
            if ((items.Count == 1 && ((typ & NounPhraseParseAttr.AdjectiveCanBeLast)) != NounPhraseParseAttr.No && (items[0].WhitespacesAfterCount < 3)) && !items[0].IsAdverb) 
            {
                if (!items[0].CanBeAdj) 
                    ok2 = true;
                else if (items[0].IsPersonalPronoun && items[0].CanBeNoun) 
                    ok2 = true;
            }
            if (ok2) 
            {
                Pullenti.Ner.Core.Internal.NounPhraseItem it = Pullenti.Ner.Core.Internal.NounPhraseItem.TryParse(items[0].EndToken.Next, null, typ);
                if (it != null && it.CanBeAdj && it.BeginToken.Chars.IsAllLower) 
                {
                    ok2 = true;
                    if (it.IsAdverb || it.IsVerb) 
                        ok2 = false;
                    if (it.IsPronoun && items[0].IsPronoun) 
                    {
                        ok2 = false;
                        if (it.CanBeAdjForPersonalPronoun && items[0].IsPersonalPronoun) 
                            ok2 = true;
                    }
                    if (ok2 && Pullenti.Ner.Core.Internal.NounPhraseItem.TryAccordAdjAndNoun(it, items[0])) 
                    {
                        NounPhraseToken npt1 = TryParseRu(it.BeginToken, typ, maxCharPos, null);
                        if (npt1 != null && ((npt1.EndChar > it.EndChar || npt1.Adjectives.Count > 0))) 
                        {
                        }
                        else 
                            items.Insert(0, it);
                    }
                }
            }
            Pullenti.Ner.Core.Internal.NounPhraseItem noun = null;
            Pullenti.Ner.Core.Internal.NounPhraseItem adjAfter = null;
            for (int i = items.Count - 1; i >= 0; i--) 
            {
                if (items[i].CanBeNoun) 
                {
                    if (items[i].ConjBefore) 
                        continue;
                    if (i > 0 && !items[i - 1].CanBeAdj) 
                        continue;
                    if (i > 0 && items[i - 1].CanBeNoun) 
                    {
                        if (items[i - 1].IsDoubtAdjective) 
                            continue;
                        if (items[i - 1].IsPronoun && items[i].IsPronoun) 
                        {
                            if (items[i].IsPronoun && items[i - 1].CanBeAdjForPersonalPronoun) 
                            {
                            }
                            else 
                                continue;
                        }
                    }
                    noun = items[i];
                    items.RemoveRange(i, items.Count - i);
                    if (adjAfter != null) 
                        items.Add(adjAfter);
                    else if (items.Count > 0 && items[0].CanBeNoun && !items[0].CanBeAdj) 
                    {
                        noun = items[0];
                        items.Clear();
                    }
                    break;
                }
            }
            if (noun == null) 
                return null;
            NounPhraseToken res = new NounPhraseToken(first, noun.EndToken) { Preposition = prep };
            if (adverbs != null) 
            {
                foreach (Pullenti.Ner.TextToken a in adverbs) 
                {
                    if (a.BeginChar < noun.BeginChar) 
                    {
                        if (items.Count == 0 && prep == null) 
                            return null;
                        if (res.Adverbs == null) 
                            res.Adverbs = new List<Pullenti.Ner.TextToken>();
                        res.Adverbs.Add(a);
                    }
                }
            }
            res.Noun = noun;
            res.MultiNouns = noun.MultiNouns;
            if (kak) 
                res.MultiNouns = true;
            res.InternalNoun = internalNounPrase;
            foreach (Pullenti.Ner.Core.Internal.NounPhraseItemTextVar v in noun.NounMorph) 
            {
                noun.Morph.AddItem(v);
            }
            res.Morph = noun.Morph;
            if (res.Morph.Case.IsNominative && first.Previous != null && first.Previous.Morph.Class.IsPreposition) 
                res.Morph.Case ^= Pullenti.Morph.MorphCase.Nominative;
            if (((typ & NounPhraseParseAttr.ParsePronouns)) == NounPhraseParseAttr.No && ((res.Morph.Class.IsPronoun || res.Morph.Class.IsPersonalPronoun))) 
                return null;
            Dictionary<char, int> stat = null;
            if (items.Count > 1) 
                stat = new Dictionary<char, int>();
            bool needUpdateMorph = false;
            if (items.Count > 0) 
            {
                List<Pullenti.Morph.MorphBaseInfo> okList = new List<Pullenti.Morph.MorphBaseInfo>();
                bool isNumNot = false;
                foreach (Pullenti.Ner.Core.Internal.NounPhraseItemTextVar vv in noun.NounMorph) 
                {
                    int i;
                    Pullenti.Ner.Core.Internal.NounPhraseItemTextVar v = vv;
                    for (i = 0; i < items.Count; i++) 
                    {
                        bool ok = false;
                        foreach (Pullenti.Ner.Core.Internal.NounPhraseItemTextVar av in items[i].AdjMorph) 
                        {
                            if (v.CheckAccord(av, false, false)) 
                            {
                                ok = true;
                                if (!((av.Case & v.Case)).IsUndefined && av.Case != v.Case) 
                                    v.Case = (av.Case = av.Case & v.Case);
                                break;
                            }
                        }
                        if (!ok) 
                        {
                            if (items[i].CanBeNumericAdj && items[i].TryAccordVar(v, false)) 
                            {
                                ok = true;
                                Pullenti.Ner.Core.Internal.NounPhraseItemTextVar v1 = new Pullenti.Ner.Core.Internal.NounPhraseItemTextVar();
                                v1.CopyFromItem(v);
                                v1.Number = Pullenti.Morph.MorphNumber.Plural;
                                isNumNot = true;
                                v1.Case = new Pullenti.Morph.MorphCase();
                                foreach (Pullenti.Ner.Core.Internal.NounPhraseItemTextVar a in items[i].AdjMorph) 
                                {
                                    v1.Case |= a.Case;
                                }
                                v = v1;
                            }
                            else 
                                break;
                        }
                    }
                    if (i >= items.Count) 
                        okList.Add(v);
                }
                if (okList.Count > 0 && (((okList.Count < res.Morph.ItemsCount) || isNumNot))) 
                {
                    res.Morph = new Pullenti.Ner.MorphCollection();
                    foreach (Pullenti.Morph.MorphBaseInfo v in okList) 
                    {
                        res.Morph.AddItem(v);
                    }
                    if (!isNumNot) 
                        noun.Morph = res.Morph;
                }
            }
            for (int i = 0; i < items.Count; i++) 
            {
                foreach (Pullenti.Ner.Core.Internal.NounPhraseItemTextVar av in items[i].AdjMorph) 
                {
                    foreach (Pullenti.Ner.Core.Internal.NounPhraseItemTextVar v in noun.NounMorph) 
                    {
                        if (v.CheckAccord(av, false, false)) 
                        {
                            if (!((av.Case & v.Case)).IsUndefined && av.Case != v.Case) 
                            {
                                v.Case = (av.Case = av.Case & v.Case);
                                needUpdateMorph = true;
                            }
                            items[i].Morph.AddItem(av);
                            if (stat != null && av.NormalValue != null && av.NormalValue.Length > 1) 
                            {
                                char last = av.NormalValue[av.NormalValue.Length - 1];
                                if (!stat.ContainsKey(last)) 
                                    stat.Add(last, 1);
                                else 
                                    stat[last]++;
                            }
                        }
                    }
                }
                if (items[i].IsPronoun || items[i].IsPersonalPronoun) 
                {
                    res.Anafor = items[i].BeginToken;
                    if (((typ & NounPhraseParseAttr.ParsePronouns)) == NounPhraseParseAttr.No) 
                        continue;
                }
                Pullenti.Ner.TextToken tt = items[i].BeginToken as Pullenti.Ner.TextToken;
                if (tt != null && !tt.Term.StartsWith("ВЫСШ")) 
                {
                    bool err = false;
                    foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
                    {
                        if (wf.Class.IsAdjective) 
                        {
                            if (wf.ContainsAttr("прев.", null)) 
                            {
                                if (((typ & NounPhraseParseAttr.IgnoreAdjBest)) != NounPhraseParseAttr.No) 
                                    err = true;
                            }
                            if (wf.ContainsAttr("к.ф.", null) && tt.Morph.Class.IsPersonalPronoun) 
                                return null;
                        }
                    }
                    if (err) 
                        continue;
                }
                if (res.Morph.Case.IsNominative) 
                {
                    string v = MiscHelper.GetTextValueOfMetaToken(items[i], GetTextAttr.KeepQuotes);
                    if (!string.IsNullOrEmpty(v)) 
                    {
                        if (items[i].GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) != v) 
                        {
                            Pullenti.Ner.Core.Internal.NounPhraseItemTextVar wf = new Pullenti.Ner.Core.Internal.NounPhraseItemTextVar(items[i].Morph, null);
                            wf.NormalValue = v;
                            wf.Class = Pullenti.Morph.MorphClass.Adjective;
                            wf.Case = res.Morph.Case;
                            if (res.Morph.Case.IsPrepositional || res.Morph.Gender == Pullenti.Morph.MorphGender.Neuter || res.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                                items[i].Morph.AddItem(wf);
                            else 
                                items[i].Morph.InsertItem(0, wf);
                        }
                    }
                }
                res.Adjectives.Add(items[i]);
                if (items[i].EndChar > res.EndChar) 
                    res.EndToken = items[i].EndToken;
            }
            for (int i = 0; i < (res.Adjectives.Count - 1); i++) 
            {
                if (res.Adjectives[i].WhitespacesAfterCount > 5) 
                {
                    if (res.Adjectives[i].Chars != res.Adjectives[i + 1].Chars) 
                    {
                        if (!res.Adjectives[i + 1].Chars.IsAllLower) 
                            return null;
                        if (res.Adjectives[i].Chars.IsAllUpper && res.Adjectives[i + 1].Chars.IsCapitalUpper) 
                            return null;
                        if (res.Adjectives[i].Chars.IsCapitalUpper && res.Adjectives[i + 1].Chars.IsAllUpper) 
                            return null;
                    }
                    if (res.Adjectives[i].WhitespacesAfterCount > 10) 
                    {
                        if (res.Adjectives[i].NewlinesAfterCount == 1) 
                        {
                            if (res.Adjectives[i].Chars.IsCapitalUpper && i == 0 && res.Adjectives[i + 1].Chars.IsAllLower) 
                                continue;
                            if (res.Adjectives[i].Chars == res.Adjectives[i + 1].Chars) 
                                continue;
                        }
                        return null;
                    }
                }
            }
            if (needUpdateMorph) 
            {
                noun.Morph = new Pullenti.Ner.MorphCollection();
                foreach (Pullenti.Ner.Core.Internal.NounPhraseItemTextVar v in noun.NounMorph) 
                {
                    noun.Morph.AddItem(v);
                }
                res.Morph = noun.Morph;
            }
            if (res.Adjectives.Count > 0) 
            {
                if (noun.BeginToken.Previous != null) 
                {
                    if (noun.BeginToken.Previous.IsCommaAnd) 
                    {
                        if (res.Adjectives[0].BeginChar > noun.BeginChar) 
                        {
                        }
                        else 
                            return null;
                    }
                }
                int zap = 0;
                int and = 0;
                int cou = 0;
                bool lastAnd = false;
                for (int i = 0; i < (res.Adjectives.Count - 1); i++) 
                {
                    Pullenti.Ner.Token te = res.Adjectives[i].EndToken.Next;
                    if (te == null) 
                        return null;
                    if (te.IsChar('(')) 
                    {
                    }
                    else if (te.IsComma) 
                    {
                        zap++;
                        lastAnd = false;
                    }
                    else if (te.IsAnd || te.IsOr) 
                    {
                        and++;
                        lastAnd = true;
                    }
                    if (!res.Adjectives[i].BeginToken.Morph.Class.IsPronoun) 
                        cou++;
                }
                if ((zap + and) > 0) 
                {
                    if (and > 1) 
                        return null;
                    else if (and == 1 && !lastAnd) 
                        return null;
                    if ((zap + and) != cou) 
                    {
                        if (and == 1) 
                        {
                        }
                        else 
                            return null;
                    }
                    Pullenti.Ner.Core.Internal.NounPhraseItem last = res.Adjectives[res.Adjectives.Count - 1] as Pullenti.Ner.Core.Internal.NounPhraseItem;
                    if (last.IsPronoun && !lastAnd) 
                        return null;
                }
            }
            if (stat != null) 
            {
                foreach (Pullenti.Ner.Core.Internal.NounPhraseItem adj in items) 
                {
                    if (adj.Morph.ItemsCount > 1) 
                    {
                        Pullenti.Ner.Core.Internal.NounPhraseItemTextVar w1 = adj.Morph[0] as Pullenti.Ner.Core.Internal.NounPhraseItemTextVar;
                        Pullenti.Ner.Core.Internal.NounPhraseItemTextVar w2 = adj.Morph[1] as Pullenti.Ner.Core.Internal.NounPhraseItemTextVar;
                        if ((w1.NormalValue.Length < 2) || (w2.NormalValue.Length < 2)) 
                            break;
                        char l1 = w1.NormalValue[w1.NormalValue.Length - 1];
                        char l2 = w2.NormalValue[w2.NormalValue.Length - 1];
                        int i1 = 0;
                        int i2 = 0;
                        stat.TryGetValue(l1, out i1);
                        stat.TryGetValue(l2, out i2);
                        if (i1 < i2) 
                        {
                            adj.Morph.RemoveItem(1);
                            adj.Morph.InsertItem(0, w2);
                        }
                    }
                }
            }
            if (res.BeginToken.GetMorphClassInDictionary().IsVerb && items.Count > 0) 
            {
                if (!res.BeginToken.Chars.IsAllLower || res.BeginToken.Previous == null) 
                {
                }
                else if (res.BeginToken.Previous.Morph.Class.IsPreposition) 
                {
                }
                else 
                {
                    bool comma = false;
                    for (Pullenti.Ner.Token tt = res.BeginToken.Previous; tt != null && tt.EndChar <= res.EndChar; tt = tt.Previous) 
                    {
                        if (tt.Morph.Class.IsAdverb) 
                            continue;
                        if (tt.IsCharOf(".;")) 
                            break;
                        if (tt.IsComma) 
                        {
                            comma = true;
                            continue;
                        }
                        if (tt.IsValue("НЕ", null)) 
                            continue;
                        if (((tt.Morph.Class.IsNoun || tt.Morph.Class.IsProper)) && comma) 
                        {
                            foreach (Pullenti.Morph.MorphBaseInfo it in res.BeginToken.Morph.Items) 
                            {
                                if (it.Class.IsVerb && (it is Pullenti.Morph.MorphWordForm)) 
                                {
                                    if (tt.Morph.CheckAccord(it, false, false)) 
                                    {
                                        if (res.Morph.Case.IsInstrumental) 
                                            return null;
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            }
            if (res.BeginToken == res.EndToken) 
            {
                Pullenti.Morph.MorphClass mc = res.BeginToken.GetMorphClassInDictionary();
                if (mc.IsAdverb) 
                {
                    if (res.BeginToken.Previous != null && res.BeginToken.Previous.Morph.Class.IsPreposition) 
                    {
                    }
                    else if (mc.IsNoun && !mc.IsPreposition && !mc.IsConjunction) 
                    {
                    }
                    else if (res.BeginToken.IsValue("ВЕСЬ", null)) 
                    {
                    }
                    else 
                        return null;
                }
            }
            if (defNoun != null && defNoun.EndToken == res.EndToken && res.Adjectives.Count > 0) 
                res.EndToken = res.Adjectives[res.Adjectives.Count - 1].EndToken;
            return res;
        }
        static NounPhraseToken TryParseEn(Pullenti.Ner.Token first, NounPhraseParseAttr typ, int maxCharPos)
        {
            if (first == null) 
                return null;
            List<Pullenti.Ner.Core.Internal.NounPhraseItem> items = null;
            bool hasArticle = false;
            bool hasProp = false;
            bool hasMisc = false;
            if (first.Previous != null && first.Previous.Morph.Class.IsPreposition && (first.WhitespacesBeforeCount < 3)) 
                hasProp = true;
            for (Pullenti.Ner.Token t = first; t != null; t = t.Next) 
            {
                if (maxCharPos > 0 && t.BeginChar > maxCharPos) 
                    break;
                if (!t.Chars.IsLatinLetter) 
                    break;
                if (t != first && t.WhitespacesBeforeCount > 2) 
                {
                    if (((typ & NounPhraseParseAttr.Multilines)) != NounPhraseParseAttr.No) 
                    {
                    }
                    else if (MiscHelper.IsEngArticle(t.Previous)) 
                    {
                    }
                    else 
                        break;
                }
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (t == first && tt != null) 
                {
                    if (MiscHelper.IsEngArticle(tt)) 
                    {
                        hasArticle = true;
                        continue;
                    }
                }
                if (t is Pullenti.Ner.ReferentToken) 
                {
                    if (((typ & NounPhraseParseAttr.ReferentCanBeNoun)) == NounPhraseParseAttr.No) 
                        break;
                }
                else if (tt == null) 
                    break;
                if ((t.IsValue("SO", null) && t.Next != null && t.Next.IsHiphen) && t.Next.Next != null) 
                {
                    if (t.Next.Next.IsValue("CALL", null)) 
                    {
                        t = t.Next.Next;
                        continue;
                    }
                }
                Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                if (mc.IsConjunction || mc.IsPreposition) 
                    break;
                if (mc.IsPronoun || mc.IsPersonalPronoun) 
                {
                    if (((typ & NounPhraseParseAttr.ParsePronouns)) == NounPhraseParseAttr.No) 
                        break;
                }
                else if (mc.IsMisc) 
                {
                    if (t.IsValue("THIS", null) || t.IsValue("THAT", null)) 
                    {
                        hasMisc = true;
                        if (((typ & NounPhraseParseAttr.ParsePronouns)) == NounPhraseParseAttr.No) 
                            break;
                    }
                }
                bool isAdj = false;
                if (((hasArticle || hasProp || hasMisc)) && items == null) 
                {
                }
                else if (t is Pullenti.Ner.ReferentToken) 
                {
                }
                else 
                {
                    if (!mc.IsNoun && !mc.IsAdjective) 
                    {
                        if (mc.IsUndefined && hasArticle) 
                        {
                        }
                        else if (items == null && mc.IsUndefined && t.Chars.IsCapitalUpper) 
                        {
                        }
                        else if (mc.IsPronoun) 
                        {
                        }
                        else if (tt.Term.EndsWith("EAN")) 
                            isAdj = true;
                        else if (MiscHelper.IsEngAdjSuffix(tt.Next)) 
                        {
                        }
                        else 
                            break;
                    }
                    if (mc.IsVerb) 
                    {
                        if (t.Next != null && t.Next.Morph.Class.IsVerb && (t.WhitespacesAfterCount < 2)) 
                        {
                        }
                        else if (t.Chars.IsCapitalUpper && !MiscHelper.CanBeStartOfSentence(t)) 
                        {
                        }
                        else if ((t.Chars.IsCapitalUpper && mc.IsNoun && (t.Next is Pullenti.Ner.TextToken)) && t.Next.Chars.IsCapitalUpper) 
                        {
                        }
                        else if (t is Pullenti.Ner.ReferentToken) 
                        {
                        }
                        else 
                            break;
                    }
                }
                if (items == null) 
                    items = new List<Pullenti.Ner.Core.Internal.NounPhraseItem>();
                Pullenti.Ner.Core.Internal.NounPhraseItem it = new Pullenti.Ner.Core.Internal.NounPhraseItem(t, t);
                if (mc.IsNoun) 
                    it.CanBeNoun = true;
                if (mc.IsAdjective || mc.IsPronoun || isAdj) 
                    it.CanBeAdj = true;
                items.Add(it);
                t = it.EndToken;
                if (items.Count == 1) 
                {
                    if (MiscHelper.IsEngAdjSuffix(t.Next)) 
                    {
                        mc.IsNoun = false;
                        mc.IsAdjective = true;
                        t = t.Next.Next;
                    }
                }
            }
            if (items == null) 
                return null;
            Pullenti.Ner.Core.Internal.NounPhraseItem noun = items[items.Count - 1];
            NounPhraseToken res = new NounPhraseToken(first, noun.EndToken);
            res.Noun = noun;
            res.Morph = new Pullenti.Ner.MorphCollection();
            foreach (Pullenti.Morph.MorphBaseInfo v in noun.EndToken.Morph.Items) 
            {
                if (v.Class.IsVerb) 
                    continue;
                if (v.Class.IsProper && noun.BeginToken.Chars.IsAllLower) 
                    continue;
                if (v is Pullenti.Morph.MorphWordForm) 
                {
                    Pullenti.Morph.MorphWordForm wf = new Pullenti.Morph.MorphWordForm();
                    wf.CopyFromWordForm(v as Pullenti.Morph.MorphWordForm);
                    if (hasArticle && v.Number != Pullenti.Morph.MorphNumber.Singular) 
                        wf.Number = Pullenti.Morph.MorphNumber.Singular;
                    res.Morph.AddItem(wf);
                }
                else 
                {
                    Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo();
                    bi.CopyFrom(v);
                    if (hasArticle && v.Number != Pullenti.Morph.MorphNumber.Singular) 
                        bi.Number = Pullenti.Morph.MorphNumber.Singular;
                    res.Morph.AddItem(bi);
                }
            }
            if (res.Morph.ItemsCount == 0 && hasArticle) 
                res.Morph.AddItem(new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Noun, Number = Pullenti.Morph.MorphNumber.Singular });
            for (int i = 0; i < (items.Count - 1); i++) 
            {
                res.Adjectives.Add(items[i]);
            }
            return res;
        }
    }
}