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

namespace Pullenti.Semantic.Internal
{
    class SentItem
    {
        public SentItem(Pullenti.Ner.MetaToken mt)
        {
            Source = mt;
            if (mt is Pullenti.Ner.Core.NounPhraseToken) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = mt as Pullenti.Ner.Core.NounPhraseToken;
                if (npt.Preposition != null) 
                    Prep = npt.Preposition.Normal;
                else 
                    Prep = "";
                Typ = SentItemType.Noun;
                string Normal = npt.Noun.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Masculine, false);
                if (Normal != null) 
                    DrGroups = Pullenti.Semantic.Utils.DerivateService.FindDerivates(Normal, true, null);
            }
            else if ((mt is Pullenti.Ner.ReferentToken) || (mt is Pullenti.Ner.Measure.Internal.NumbersWithUnitToken)) 
                Typ = SentItemType.Noun;
            else if (mt is AdverbToken) 
                Typ = SentItemType.Adverb;
            else if (mt is Pullenti.Ner.Core.ConjunctionToken) 
                Typ = SentItemType.Conj;
            else if (mt is DelimToken) 
                Typ = SentItemType.Delim;
            else if (mt is Pullenti.Ner.Core.VerbPhraseToken) 
            {
                Pullenti.Ner.Core.VerbPhraseToken vpt = mt as Pullenti.Ner.Core.VerbPhraseToken;
                string Normal = (vpt.FirstVerb.VerbMorph == null ? null : vpt.FirstVerb.VerbMorph.NormalFull ?? vpt.FirstVerb.VerbMorph.NormalCase);
                if (Normal != null) 
                    DrGroups = Pullenti.Semantic.Utils.DerivateService.FindDerivates(Normal, true, null);
                if (vpt.FirstVerb != vpt.LastVerb) 
                {
                    Normal = (vpt.LastVerb.VerbMorph == null ? vpt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) : vpt.LastVerb.VerbMorph.NormalFull ?? vpt.LastVerb.VerbMorph.NormalCase);
                    DrGroups2 = Pullenti.Semantic.Utils.DerivateService.FindDerivates(Normal, true, null);
                }
                else 
                    DrGroups2 = DrGroups;
                Prep = (vpt.Preposition == null ? "" : vpt.Preposition.Normal);
                Typ = SentItemType.Verb;
            }
        }
        public void CopyFrom(SentItem it)
        {
            Source = it.Source;
            Typ = it.Typ;
            SubTyp = it.SubTyp;
            Prep = it.Prep;
            ParticipleCoef = it.ParticipleCoef;
            DrGroups = it.DrGroups;
            DrGroups2 = it.DrGroups2;
            PartVerbTyp = it.PartVerbTyp;
            m_BeginToken = it.m_BeginToken;
            m_EndToken = it.m_EndToken;
            Plural = it.Plural;
            SubSent = it.SubSent;
            Quant = it.Quant;
            Attrs = it.Attrs;
            CanBeQuestion = it.CanBeQuestion;
            Result = it.Result;
            ResultList = it.ResultList;
            ResultListOr = it.ResultListOr;
            ResultVerbLast = it.ResultVerbLast;
            ResGraph = it.ResGraph;
            ResFrag = it.ResFrag;
        }
        public Pullenti.Ner.MetaToken Source;
        public string Prep;
        public SentItemType Typ = SentItemType.Undefined;
        public SentItemSubtype SubTyp = SentItemSubtype.Undefined;
        public Sentence SubSent;
        public int Plural = -1;
        public List<Pullenti.Semantic.Utils.DerivateGroup> DrGroups;
        public List<Pullenti.Semantic.Utils.DerivateGroup> DrGroups2;
        public NGLinkType PartVerbTyp = NGLinkType.Undefined;
        public double ParticipleCoef = 1;
        public Pullenti.Semantic.SemQuantity Quant;
        public List<SemAttributeEx> Attrs = null;
        public bool CanBeQuestion;
        public Pullenti.Semantic.SemObject Result;
        public Pullenti.Semantic.SemObject ResultVerbLast;
        public Pullenti.Semantic.SemGraph ResGraph
        {
            get
            {
                return m_ResGraph;
            }
            set
            {
                if (m_ResGraph == null) 
                    m_ResGraph = value;
                else if (value != null && m_ResGraph != value) 
                    m_ResGraph = value;
            }
        }
        Pullenti.Semantic.SemGraph m_ResGraph;
        public Pullenti.Semantic.SemFragment ResFrag;
        public List<Pullenti.Semantic.SemObject> ResultList = null;
        public bool ResultListOr;
        public void AddAttr(AdverbToken adv)
        {
            Pullenti.Semantic.SemAttribute sa = new Pullenti.Semantic.SemAttribute() { Spelling = adv.Spelling, Typ = adv.Typ, Not = adv.Not };
            if (Attrs == null) 
                Attrs = new List<SemAttributeEx>();
            Attrs.Add(new SemAttributeEx(adv) { Attr = sa });
        }
        public Pullenti.Ner.Token BeginToken
        {
            get
            {
                if (m_BeginToken != null) 
                    return m_BeginToken;
                if (Source != null) 
                    return Source.BeginToken;
                return null;
            }
            set
            {
                m_BeginToken = value;
            }
        }
        Pullenti.Ner.Token m_BeginToken;
        public Pullenti.Ner.Token EndToken
        {
            get
            {
                if (m_EndToken != null) 
                    return m_EndToken;
                if (Source != null) 
                {
                    Pullenti.Ner.Token ret = Source.EndToken;
                    if (Attrs != null) 
                    {
                        foreach (SemAttributeEx a in Attrs) 
                        {
                            if (a.Token.EndChar > ret.EndChar) 
                                ret = a.Token.EndToken;
                        }
                    }
                    return ret;
                }
                return null;
            }
            set
            {
                m_EndToken = value;
            }
        }
        Pullenti.Ner.Token m_EndToken;
        public bool CanBeNoun
        {
            get
            {
                if (((Typ == SentItemType.Noun || Typ == SentItemType.Deepart || Typ == SentItemType.PartAfter) || Typ == SentItemType.PartBefore || Typ == SentItemType.SubSent) || Typ == SentItemType.Formula) 
                    return true;
                if (Source is Pullenti.Ner.Core.VerbPhraseToken) 
                {
                    if ((Source as Pullenti.Ner.Core.VerbPhraseToken).FirstVerb.VerbMorph != null && (Source as Pullenti.Ner.Core.VerbPhraseToken).FirstVerb.VerbMorph.ContainsAttr("инф.", null)) 
                        return true;
                }
                return false;
            }
        }
        public bool CanBeCommaEnd
        {
            get
            {
                Pullenti.Ner.Core.ConjunctionToken cnj = Source as Pullenti.Ner.Core.ConjunctionToken;
                if (cnj == null) 
                    return false;
                return cnj.Typ == Pullenti.Ner.Core.ConjunctionType.Comma || cnj.Typ == Pullenti.Ner.Core.ConjunctionType.And || cnj.Typ == Pullenti.Ner.Core.ConjunctionType.Or;
            }
        }
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (!string.IsNullOrEmpty(Prep)) 
                res.AppendFormat("{0} ", Prep);
            res.AppendFormat("{0}(", Typ.ToString());
            if (SubTyp != SentItemSubtype.Undefined) 
                res.AppendFormat("{0}:", SubTyp);
            if (Source != null) 
            {
                res.Append(Source.ToString());
                if (SubSent != null) 
                    res.AppendFormat(" <= {0}", SubSent.ToString());
            }
            else if (SubSent != null) 
                res.Append(SubSent.ToString());
            res.Append(')');
            return res.ToString();
        }
        public static List<SentItem> ParseNearItems(Pullenti.Ner.Token t, Pullenti.Ner.Token t1, int lev, List<SentItem> prev)
        {
            if (lev > 100) 
                return null;
            if (t == null || t.BeginChar > t1.EndChar) 
                return null;
            List<SentItem> res = new List<SentItem>();
            if (t is Pullenti.Ner.ReferentToken) 
            {
                res.Add(new SentItem(t as Pullenti.Ner.MetaToken));
                return res;
            }
            DelimToken delim = DelimToken.TryParse(t);
            if (delim != null) 
            {
                res.Add(new SentItem(delim));
                return res;
            }
            Pullenti.Ner.Core.ConjunctionToken conj = Pullenti.Ner.Core.ConjunctionHelper.TryParse(t);
            if (conj != null) 
            {
                res.Add(new SentItem(conj));
                return res;
            }
            Pullenti.Ner.Core.PrepositionToken prep = Pullenti.Ner.Core.PrepositionHelper.TryParse(t);
            Pullenti.Ner.Token t111 = (prep == null ? t : prep.EndToken.Next);
            if ((t111 is Pullenti.Ner.NumberToken) && ((t111.Morph.Class.IsAdjective && !t111.Morph.Class.IsNoun))) 
                t111 = null;
            Pullenti.Ner.Measure.Internal.NumbersWithUnitToken num = (t111 == null ? null : Pullenti.Ner.Measure.Internal.NumbersWithUnitToken.TryParse(t111, null, false, false, false, false));
            if (num != null) 
            {
                if (num.Units.Count == 0) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(num.EndToken.Next, m_NptAttrs, 0, null);
                    if (npt1 == null && num.EndToken.Next != null && num.EndToken.Next.IsValue("РАЗ", null)) 
                    {
                        npt1 = new Pullenti.Ner.Core.NounPhraseToken(num.EndToken.Next, num.EndToken.Next);
                        npt1.Noun = new Pullenti.Ner.MetaToken(num.EndToken.Next, num.EndToken.Next);
                    }
                    if (npt1 != null && prep != null) 
                    {
                        if (npt1.Noun.EndToken.IsValue("РАЗ", null)) 
                            npt1.Morph.RemoveItems(prep.NextCase);
                        else if (((npt1.Morph.Case & prep.NextCase)).IsUndefined) 
                            npt1 = null;
                        else 
                            npt1.Morph.RemoveItems(prep.NextCase);
                    }
                    if ((npt1 != null && npt1.EndToken.IsValue("ОНИ", null) && npt1.Preposition != null) && npt1.Preposition.Normal == "ИЗ") 
                    {
                        npt1.Morph = new Pullenti.Ner.MorphCollection(num.EndToken.Morph);
                        npt1.Preposition = null;
                        string nn = num.ToString();
                        SentItem si1 = new SentItem(npt1);
                        if (nn == "1" && (num.EndToken is Pullenti.Ner.NumberToken) && (num.EndToken as Pullenti.Ner.NumberToken).EndToken.IsValue("ОДИН", null)) 
                        {
                            Pullenti.Semantic.SemAttribute a = new Pullenti.Semantic.SemAttribute() { Typ = Pullenti.Semantic.SemAttributeType.OneOf, Spelling = (num.EndToken as Pullenti.Ner.NumberToken).EndToken.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false) };
                            SemAttributeEx aex = new SemAttributeEx(num) { Attr = a };
                            si1.Attrs = new List<SemAttributeEx>();
                            si1.Attrs.Add(aex);
                        }
                        else 
                            si1.Quant = new Pullenti.Semantic.SemQuantity(nn, num.BeginToken, num.EndToken);
                        if (prep != null) 
                            si1.Prep = prep.Normal;
                        res.Add(si1);
                        return res;
                    }
                    if (npt1 != null) 
                    {
                        SentItem si1 = new SentItem(npt1) { Quant = new Pullenti.Semantic.SemQuantity(num.ToString(), num.BeginToken, num.EndToken) };
                        if (prep != null) 
                            si1.Prep = prep.Normal;
                        if (npt1.EndToken.IsValue("РАЗ", null)) 
                            si1.Typ = SentItemType.Formula;
                        if (((npt1.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) == Pullenti.Morph.MorphNumber.Undefined && si1.Quant.Spelling != "1") 
                        {
                            bool ok = false;
                            if (si1.Quant.Spelling.EndsWith("1")) 
                                ok = true;
                            else if (si1.Typ == SentItemType.Formula) 
                                ok = true;
                            else if (si1.Quant.Spelling.EndsWith("2") && npt1.Morph.Case.IsGenitive) 
                                ok = true;
                            else if (si1.Quant.Spelling.EndsWith("3") && npt1.Morph.Case.IsGenitive) 
                                ok = true;
                            else if (si1.Quant.Spelling.EndsWith("4") && npt1.Morph.Case.IsGenitive) 
                                ok = true;
                            if (ok) 
                            {
                                npt1.Morph = new Pullenti.Ner.MorphCollection();
                                npt1.Morph.Number = Pullenti.Morph.MorphNumber.Plural;
                            }
                        }
                        res.Add(si1);
                        return res;
                    }
                }
                num.BeginToken = t;
                num.Morph = new Pullenti.Ner.MorphCollection(num.EndToken.Morph);
                SentItem si = new SentItem(num);
                if (prep != null) 
                    si.Prep = prep.Normal;
                res.Add(si);
                if (si.Prep == "НА") 
                {
                    AdverbToken aa = AdverbToken.TryParse(si.EndToken.Next);
                    if (aa != null && ((aa.Typ == Pullenti.Semantic.SemAttributeType.Less || aa.Typ == Pullenti.Semantic.SemAttributeType.Great))) 
                    {
                        si.AddAttr(aa);
                        si.EndToken = aa.EndToken;
                    }
                }
                return res;
            }
            Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
            AdverbToken adv = AdverbToken.TryParse(t);
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, m_NptAttrs, 0, null);
            if (npt != null && (npt.EndToken is Pullenti.Ner.TextToken) && (npt.EndToken as Pullenti.Ner.TextToken).Term == "БЫЛИ") 
                npt = null;
            if (npt != null && adv != null) 
            {
                if (adv.EndChar > npt.EndChar) 
                    npt = null;
                else if (adv.EndChar == npt.EndChar) 
                {
                    res.Add(new SentItem(npt));
                    res.Add(new SentItem(adv));
                    return res;
                }
            }
            if (npt != null && npt.Adjectives.Count == 0) 
            {
                if (npt.EndToken.IsValue("КОТОРЫЙ", null) && t.Previous != null && t.Previous.IsCommaAnd) 
                {
                    List<SentItem> res1 = ParseSubsent(npt, t1, lev + 1, prev);
                    if (res1 != null) 
                        return res1;
                }
                if (npt.EndToken.IsValue("СКОЛЬКО", null)) 
                {
                    Pullenti.Ner.Token tt1 = npt.EndToken.Next;
                    if (tt1 != null && tt1.IsValue("ВСЕГО", null)) 
                        tt1 = tt1.Next;
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt1 != null && !npt1.Morph.Case.IsUndefined && prep != null) 
                    {
                        if (((prep.NextCase & npt1.Morph.Case)).IsUndefined) 
                            npt1 = null;
                        else 
                            npt1.Morph.RemoveItems(prep.NextCase);
                    }
                    if (npt1 != null) 
                    {
                        npt1.BeginToken = npt.BeginToken;
                        npt1.Preposition = npt.Preposition;
                        npt1.Adjectives.Add(new Pullenti.Ner.MetaToken(npt.EndToken, npt.EndToken));
                        npt = npt1;
                    }
                }
                if (npt.EndToken.Morph.Class.IsAdjective) 
                {
                    if (Pullenti.Ner.Core.VerbPhraseHelper.TryParse(t, true, false, false) != null) 
                        npt = null;
                }
            }
            Pullenti.Ner.Core.VerbPhraseToken vrb = null;
            if (npt != null && npt.Adjectives.Count > 0) 
            {
                vrb = Pullenti.Ner.Core.VerbPhraseHelper.TryParse(t, true, false, false);
                if (vrb != null && vrb.FirstVerb.IsParticiple) 
                    npt = null;
            }
            else if (adv == null || npt != null) 
                vrb = Pullenti.Ner.Core.VerbPhraseHelper.TryParse(t, true, false, false);
            if (npt != null) 
                res.Add(new SentItem(npt));
            if (vrb != null && !vrb.FirstVerb.IsParticiple && !vrb.FirstVerb.IsDeeParticiple) 
            {
                List<Pullenti.Morph.MorphWordForm> vars = new List<Pullenti.Morph.MorphWordForm>();
                foreach (Pullenti.Morph.MorphBaseInfo wf in vrb.FirstVerb.Morph.Items) 
                {
                    if (wf.Class.IsVerb && (wf is Pullenti.Morph.MorphWordForm) && (wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                        vars.Add(wf as Pullenti.Morph.MorphWordForm);
                }
                if (vars.Count < 2) 
                    res.Add(new SentItem(vrb));
                else 
                {
                    vrb.FirstVerb.VerbMorph = vars[0];
                    res.Add(new SentItem(vrb));
                    for (int i = 1; i < vars.Count; i++) 
                    {
                        vrb = Pullenti.Ner.Core.VerbPhraseHelper.TryParse(t, false, false, false);
                        if (vrb == null) 
                            break;
                        vrb.FirstVerb.VerbMorph = vars[i];
                        res.Add(new SentItem(vrb));
                    }
                    if (vars[0].Misc.Mood == Pullenti.Morph.MorphMood.Imperative && vars[1].Misc.Mood != Pullenti.Morph.MorphMood.Imperative) 
                    {
                        SentItem rr = res[0];
                        res[0] = res[1];
                        res[1] = rr;
                    }
                }
                return res;
            }
            if (vrb != null) 
            {
                List<SentItem> res1 = ParseParticiples(vrb, t1, lev + 1);
                if (res1 != null) 
                    res.AddRange(res1);
            }
            if (res.Count > 0) 
                return res;
            if (adv != null) 
            {
                if (adv.Typ == Pullenti.Semantic.SemAttributeType.Other) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(adv.EndToken.Next, m_NptAttrs, 0, null);
                    if (npt1 != null && npt1.EndToken.IsValue("ОНИ", null) && npt1.Preposition != null) 
                    {
                        SentItem si1 = new SentItem(npt1);
                        Pullenti.Semantic.SemAttribute a = new Pullenti.Semantic.SemAttribute() { Typ = Pullenti.Semantic.SemAttributeType.Other, Spelling = adv.EndToken.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) };
                        SemAttributeEx aex = new SemAttributeEx(num) { Attr = a };
                        si1.Attrs = new List<SemAttributeEx>();
                        si1.Attrs.Add(aex);
                        if (prep != null) 
                            si1.Prep = prep.Normal;
                        res.Add(si1);
                        return res;
                    }
                    for (int i = prev.Count - 1; i >= 0; i--) 
                    {
                        if (prev[i].Attrs != null) 
                        {
                            foreach (SemAttributeEx a in prev[i].Attrs) 
                            {
                                if (a.Attr.Typ == Pullenti.Semantic.SemAttributeType.OneOf) 
                                {
                                    SentItem si1 = new SentItem(prev[i].Source);
                                    Pullenti.Semantic.SemAttribute aa = new Pullenti.Semantic.SemAttribute() { Typ = Pullenti.Semantic.SemAttributeType.Other, Spelling = adv.EndToken.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) };
                                    SemAttributeEx aex = new SemAttributeEx(adv) { Attr = aa };
                                    si1.Attrs = new List<SemAttributeEx>();
                                    si1.Attrs.Add(aex);
                                    if (prep != null) 
                                        si1.Prep = prep.Normal;
                                    si1.BeginToken = adv.BeginToken;
                                    si1.EndToken = adv.EndToken;
                                    res.Add(si1);
                                    return res;
                                }
                            }
                        }
                    }
                }
                res.Add(new SentItem(adv));
                return res;
            }
            if (mc.IsAdjective) 
            {
                npt = new Pullenti.Ner.Core.NounPhraseToken(t, t) { Morph = new Pullenti.Ner.MorphCollection(t.Morph) };
                npt.Noun = new Pullenti.Ner.MetaToken(t, t);
                res.Add(new SentItem(npt));
                return res;
            }
            return null;
        }
        static List<SentItem> ParseSubsent(Pullenti.Ner.Core.NounPhraseToken npt, Pullenti.Ner.Token t1, int lev, List<SentItem> prev)
        {
            bool ok = false;
            if (prev != null) 
            {
                for (int i = prev.Count - 1; i >= 0; i--) 
                {
                    SentItem it = prev[i];
                    if (it.Typ == SentItemType.Conj || it.Typ == SentItemType.Delim) 
                    {
                        ok = true;
                        break;
                    }
                    if (it.Typ == SentItemType.Verb) 
                        break;
                }
            }
            if (!ok) 
                return null;
            List<Sentence> sents = Sentence.ParseVariants(npt.EndToken.Next, t1, lev + 1, 20, SentItemType.SubSent) ?? new List<Sentence>();
            List<int> endpos = new List<int>();
            List<SentItem> res = new List<SentItem>();
            foreach (Sentence s in sents) 
            {
                s.Items.Insert(0, new SentItem(npt));
                s.CalcCoef(true);
                s.TruncOborot(false);
                int end = s.Items[s.Items.Count - 1].EndToken.EndChar;
                if (endpos.Contains(end)) 
                    continue;
                endpos.Add(end);
                s.CalcCoef(false);
                SentItem part = new SentItem(npt);
                part.Typ = SentItemType.SubSent;
                part.SubTyp = SentItemSubtype.Wich;
                part.SubSent = s;
                part.Result = s.Items[0].Result;
                part.EndToken = s.Items[s.Items.Count - 1].EndToken;
                res.Add(part);
            }
            return res;
        }
        static List<SentItem> ParseParticiples(Pullenti.Ner.Core.VerbPhraseToken vb, Pullenti.Ner.Token t1, int lev)
        {
            List<Sentence> sents = Sentence.ParseVariants(vb.EndToken.Next, t1, lev + 1, 20, SentItemType.PartBefore) ?? new List<Sentence>();
            NGLinkType typ = NGLinkType.Agent;
            if (vb.FirstVerb.Morph.ContainsAttr("страд.з.", null)) 
                typ = NGLinkType.Pacient;
            else if (vb.FirstVerb.Morph.ContainsAttr("возвр.", null)) 
                typ = NGLinkType.Pacient;
            List<int> endpos = new List<int>();
            List<SentItem> res = new List<SentItem>();
            bool changed = false;
            foreach (Sentence s in sents) 
            {
                if (vb.FirstVerb.IsDeeParticiple) 
                    break;
                for (int i = 0; i < s.Items.Count; i++) 
                {
                    SentItem it = s.Items[i];
                    if (!it.CanBeNoun || it.Typ == SentItemType.Verb) 
                        continue;
                    if (!string.IsNullOrEmpty(it.Prep)) 
                        continue;
                    if (it.Typ == SentItemType.PartBefore || it.Typ == SentItemType.PartAfter) 
                        continue;
                    NGLink li = new NGLink() { Typ = typ, From = new NGItem() { Source = it }, ToVerb = vb };
                    li.CalcCoef(true);
                    if (li.Coef < 0) 
                        continue;
                    if (endpos.Contains(it.EndToken.EndChar)) 
                        continue;
                    Sentence ss = new Sentence() { LastNounToFirstVerb = typ };
                    ss.Items.Add(new SentItem(vb));
                    for (int j = 0; j <= i; j++) 
                    {
                        SentItem si = new SentItem(null);
                        si.CopyFrom(s.Items[j]);
                        ss.Items.Add(si);
                    }
                    ss.CalcCoef(false);
                    changed = true;
                    if (ss.Coef < 0) 
                        continue;
                    SentItem part = new SentItem(it.Source);
                    part.Typ = SentItemType.PartAfter;
                    part.SubSent = ss;
                    if (vb.Preposition != null) 
                        part.Prep = vb.Preposition.Normal;
                    part.BeginToken = vb.BeginToken;
                    part.EndToken = it.Source.EndToken;
                    if ((i + 1) < ss.Items.Count) 
                        part.Result = ss.Items[i + 1].Result;
                    endpos.Add(it.EndToken.EndChar);
                    res.Add(part);
                }
            }
            endpos.Clear();
            if (changed) 
                sents = Sentence.ParseVariants(vb.EndToken.Next, t1, lev + 1, 20, SentItemType.PartBefore) ?? new List<Sentence>();
            foreach (Sentence s in sents) 
            {
                s.Items.Insert(0, new SentItem(vb));
                s.CalcCoef(true);
                s.TruncOborot(true);
                int end = s.Items[s.Items.Count - 1].EndToken.EndChar;
                endpos.Add(end);
                s.NotLastNounToFirstVerb = typ;
                s.CalcCoef(false);
                SentItem part = new SentItem(vb);
                part.PartVerbTyp = typ;
                part.Typ = (vb.FirstVerb.IsDeeParticiple ? SentItemType.Deepart : SentItemType.PartBefore);
                part.SubSent = s;
                part.Result = s.Items[0].Result;
                part.ResultVerbLast = s.Items[0].ResultVerbLast;
                part.EndToken = s.Items[s.Items.Count - 1].EndToken;
                res.Add(part);
            }
            if (res.Count == 0 && sents.Count == 0) 
            {
                SentItem part = new SentItem(vb);
                part.PartVerbTyp = typ;
                part.Typ = (vb.FirstVerb.IsDeeParticiple ? SentItemType.Deepart : SentItemType.PartBefore);
                res.Add(part);
            }
            return res;
        }
        static Pullenti.Ner.Core.NounPhraseParseAttr m_NptAttrs = (((Pullenti.Ner.Core.NounPhraseParseAttr.AdjectiveCanBeLast | Pullenti.Ner.Core.NounPhraseParseAttr.IgnoreBrackets | Pullenti.Ner.Core.NounPhraseParseAttr.ParseAdverbs) | Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition) | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns | Pullenti.Ner.Core.NounPhraseParseAttr.ParseVerbs) | Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun | Pullenti.Ner.Core.NounPhraseParseAttr.MultiNouns;
    }
}