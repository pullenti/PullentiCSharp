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
    class Sentence : IComparable<Sentence>
    {
        void _createLists(NGSegmentVariant s)
        {
            for (int i = 0; i < s.Links.Count; i++) 
            {
                List<NGItem> list = s.GetList(i);
                if (list == null) 
                    continue;
                if (list[0].Source.Result == null) 
                    continue;
                SentItem root = list[0].Source;
                root.ResultList = new List<Pullenti.Semantic.SemObject>();
                foreach (NGItem li in list) 
                {
                    if (li.Source.Result != null) 
                        root.ResultList.Add(li.Source.Result);
                    if (li != list[0] && li.OrBefore) 
                        root.ResultListOr = true;
                }
            }
        }
        void _setLastAltLinks(Pullenti.Semantic.SemGraph fr)
        {
            if (fr.Links.Count > 1) 
            {
                Pullenti.Semantic.SemLink li0 = fr.Links[fr.Links.Count - 2];
                Pullenti.Semantic.SemLink li1 = fr.Links[fr.Links.Count - 1];
                li0.AltLink = li1;
                li1.AltLink = li0;
            }
        }
        void _createLinks(NGSegmentVariant s)
        {
            for (int i = 0; i < s.Links.Count; i++) 
            {
                NGLink link0 = s.Links[i];
                if (link0 == null) 
                    continue;
                if (link0.Typ == NGLinkType.List) 
                    continue;
                for (int k = 0; k < 2; k++) 
                {
                    NGLink li = link0;
                    if (k == 1) 
                        li = li.AltLink;
                    if (li == null) 
                        break;
                    if (li.From.ResObject == null) 
                        continue;
                    if (k == 1) 
                    {
                    }
                    Pullenti.Semantic.SemGraph gr = li.From.ResObject.Graph;
                    if (li.To != null && li.To.ResObject != null) 
                    {
                        Pullenti.Semantic.SemLink link = null;
                        if (li.Typ == NGLinkType.Participle && li.From.Source.SubTyp == SentItemSubtype.Wich) 
                        {
                            link = gr.AddLink(Pullenti.Semantic.SemLinkType.Anafor, li.From.ResObject, li.To.ResObject, null, false, null);
                            if (k > 0) 
                                this._setLastAltLinks(gr);
                            continue;
                        }
                        if (li.Typ == NGLinkType.Participle && li.From.Source.Typ == SentItemType.PartBefore) 
                        {
                            link = gr.AddLink(Pullenti.Semantic.SemLinkType.Participle, li.To.ResObject, li.From.ResObject, "какой", false, null);
                            if (k > 0) 
                                this._setLastAltLinks(gr);
                            if (li.From.Source.ResultList != null && li.Typ == NGLinkType.Participle) 
                            {
                                link.IsOr = li.From.Source.ResultListOr;
                                for (int ii = 1; ii < li.From.Source.ResultList.Count; ii++) 
                                {
                                    gr.AddLink(link.Typ, link.Source, li.From.Source.ResultList[ii], link.Question, link.IsOr, null);
                                }
                            }
                            continue;
                        }
                        if (li.Typ == NGLinkType.Be) 
                        {
                            if ((li.From.Source.Source is Pullenti.Ner.Measure.Internal.NumbersWithUnitToken) && li.To != null) 
                            {
                                gr.AddLink(Pullenti.Semantic.SemLinkType.Detail, li.To.ResObject, li.From.ResObject, "какой", false, li.FromPrep);
                                continue;
                            }
                            Pullenti.Semantic.SemObject be = new Pullenti.Semantic.SemObject(gr) { Typ = Pullenti.Semantic.SemObjectType.Verb };
                            be.Tokens.Add(li.From.Source.Source);
                            be.Morph.NormalCase = (be.Morph.NormalFull = "БЫТЬ");
                            gr.Objects.Add(be);
                            gr.AddLink(Pullenti.Semantic.SemLinkType.Agent, be, li.To.ResObject, null, false, null);
                            gr.AddLink(Pullenti.Semantic.SemLinkType.Pacient, be, li.From.ResObject, null, false, null);
                            continue;
                        }
                        Pullenti.Semantic.SemLinkType ty = Pullenti.Semantic.SemLinkType.Undefined;
                        string ques = null;
                        if (li.Typ == NGLinkType.Genetive) 
                        {
                            if (li.CanBePacient) 
                                ty = Pullenti.Semantic.SemLinkType.Pacient;
                            else 
                            {
                                ty = Pullenti.Semantic.SemLinkType.Detail;
                                ques = "чего";
                            }
                            if (!string.IsNullOrEmpty(li.FromPrep)) 
                                ques = CreateHelper.CreateQuestion(li.From);
                        }
                        else if (li.Typ == NGLinkType.Name) 
                            ty = Pullenti.Semantic.SemLinkType.Naming;
                        link = gr.AddLink(ty, li.To.ResObject, li.From.ResObject, ques, false, li.FromPrep);
                        if (li.From.Source.ResultList != null) 
                        {
                            link.IsOr = li.From.Source.ResultListOr;
                            for (int ii = 1; ii < li.From.Source.ResultList.Count; ii++) 
                            {
                                Pullenti.Semantic.SemLink link1 = gr.AddLink(ty, link.Source, li.From.Source.ResultList[ii], ques, link.IsOr, null);
                                link1.Preposition = link.Preposition;
                            }
                        }
                        List<NGItem> list = null;
                        if (li.ToAllListItems) 
                        {
                            list = s.GetListByLastItem(li.To);
                            if (list != null) 
                            {
                                bool ok = true;
                                for (int j = 0; j < (list.Count - 1); j++) 
                                {
                                    if (list[j].ResObject != null && list[j].ResObject.LinksFrom.Count > 0) 
                                    {
                                        ok = false;
                                        break;
                                    }
                                }
                                if (ok) 
                                {
                                    for (int j = 0; j < (list.Count - 1); j++) 
                                    {
                                        gr.AddLink(link.Typ, list[j].ResObject, link.Target, link.Question, false, link.Preposition);
                                    }
                                }
                            }
                        }
                        if (k > 0) 
                            this._setLastAltLinks(gr);
                    }
                    if (li.ToVerb != null && li.From.ResObject != null) 
                    {
                        Pullenti.Semantic.SemLink link = null;
                        SentItem vitem = null;
                        foreach (SentItem iii in Items) 
                        {
                            if (iii.Source == li.ToVerb) 
                            {
                                vitem = iii;
                                break;
                            }
                        }
                        if (li.Typ == NGLinkType.Agent && vitem != null && vitem.Result != null) 
                        {
                            Pullenti.Semantic.SemObject verb = vitem.Result;
                            if (verb.Typ == Pullenti.Semantic.SemObjectType.Participle && li.CanBeParticiple) 
                                link = gr.AddLink(Pullenti.Semantic.SemLinkType.Participle, li.From.ResObject, verb, "какой", false, null);
                            else 
                                link = gr.AddLink(Pullenti.Semantic.SemLinkType.Agent, verb, li.From.ResObject, null, false, null);
                            if (k > 0) 
                                this._setLastAltLinks(gr);
                        }
                        else if (((li.Typ == NGLinkType.Pacient || li.Typ == NGLinkType.Actant)) && vitem != null && vitem.ResultVerbLast != null) 
                        {
                            Pullenti.Semantic.SemObject verb = vitem.ResultVerbLast;
                            string ques = null;
                            if (li.Typ == NGLinkType.Actant) 
                                ques = CreateHelper.CreateQuestion(li.From);
                            if (verb.Typ == Pullenti.Semantic.SemObjectType.Participle && li.Typ == NGLinkType.Pacient && li.CanBeParticiple) 
                                link = gr.AddLink(Pullenti.Semantic.SemLinkType.Participle, li.From.ResObject, verb, "какой", false, null);
                            else 
                                link = gr.AddLink((li.Typ == NGLinkType.Pacient ? Pullenti.Semantic.SemLinkType.Pacient : Pullenti.Semantic.SemLinkType.Detail), verb, li.From.ResObject, ques, false, li.FromPrep);
                            if (k > 0) 
                                this._setLastAltLinks(gr);
                        }
                        if (link == null) 
                            continue;
                        if (li.From.Source.ResultList != null) 
                        {
                            link.IsOr = li.From.Source.ResultListOr;
                            for (int jj = 1; jj < li.From.Source.ResultList.Count; jj++) 
                            {
                                if (link.Typ == Pullenti.Semantic.SemLinkType.Participle) 
                                    gr.AddLink(link.Typ, li.From.Source.ResultList[jj], link.Target, link.Question, link.IsOr, null);
                                else 
                                    gr.AddLink(link.Typ, link.Source, li.From.Source.ResultList[jj], link.Question, link.IsOr, null);
                            }
                        }
                    }
                }
            }
        }
        void CreateResult(Pullenti.Semantic.SemBlock blk)
        {
            if (BestVar != null) 
            {
                foreach (NGSegmentVariant s in BestVar.Segs) 
                {
                    if (s != null) 
                        s.CorrectMorph();
                }
                BestVar.CreateAltLinks();
            }
            List<SentItem> allItems = new List<SentItem>();
            foreach (SentItem it in Items) 
            {
                if (it.ResGraph == null) 
                    continue;
                if (it.Result == null) 
                {
                    if (it.Source is Pullenti.Ner.Core.NounPhraseToken) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = it.Source as Pullenti.Ner.Core.NounPhraseToken;
                        if (it.Plural == 1 && ((it.Source.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) != Pullenti.Morph.MorphNumber.Undefined) 
                            it.Source.Morph.RemoveItems(Pullenti.Morph.MorphNumber.Plural);
                        it.Result = CreateHelper.CreateNounGroup(it.ResGraph, npt);
                        if (npt.MultiNouns && it.Result.Quantity == null) 
                        {
                            it.ResultList = new List<Pullenti.Semantic.SemObject>();
                            it.ResultList.Add(it.Result);
                            if (npt.Adjectives.Count > 0 && ((npt.Adjectives[0].BeginToken.Morph.Number & Pullenti.Morph.MorphNumber.Singular)) == Pullenti.Morph.MorphNumber.Singular) 
                            {
                                it.Result.Morph.Number = Pullenti.Morph.MorphNumber.Singular;
                                if (it.Result.Morph.NormalFull != null) 
                                    it.Result.Morph.NormalCase = it.Result.Morph.NormalFull;
                            }
                            for (int i = 1; i < npt.Adjectives.Count; i++) 
                            {
                                Pullenti.Semantic.SemObject so = new Pullenti.Semantic.SemObject(it.ResGraph) { Typ = it.Result.Typ };
                                so.Tokens.Add(npt.Noun);
                                Pullenti.Morph.MorphWordForm wf = new Pullenti.Morph.MorphWordForm();
                                wf.CopyFromWordForm(it.Result.Morph);
                                so.Morph = wf;
                                foreach (Pullenti.Semantic.SemAttribute a in it.Result.Attrs) 
                                {
                                    so.Attrs.Add(a);
                                }
                                so.Concept = it.Result.Concept;
                                so.Not = it.Result.Not;
                                Pullenti.Semantic.SemObject asem = CreateHelper.CreateNptAdj(it.ResGraph, npt, npt.Adjectives[i]);
                                if (asem != null) 
                                    it.ResGraph.AddLink(Pullenti.Semantic.SemLinkType.Detail, so, asem, "какой", false, null);
                                it.ResultList.Add(so);
                                it.ResGraph.Objects.Add(so);
                            }
                        }
                    }
                    else if (it.Source is Pullenti.Ner.Core.VerbPhraseToken) 
                    {
                        it.Result = CreateHelper.CreateVerbGroup(it.ResGraph, it.Source as Pullenti.Ner.Core.VerbPhraseToken);
                        it.ResultVerbLast = (it.Source as Pullenti.Ner.Core.VerbPhraseToken).LastVerb.Tag as Pullenti.Semantic.SemObject;
                    }
                    else if (it.Source is Pullenti.Ner.Measure.Internal.NumbersWithUnitToken) 
                        it.Result = CreateHelper.CreateNumber(it.ResGraph, it.Source as Pullenti.Ner.Measure.Internal.NumbersWithUnitToken);
                    if (it.Result != null && it.Quant != null) 
                        it.Result.Quantity = it.Quant;
                    if (it.Result != null && it.Attrs != null) 
                    {
                        foreach (SemAttributeEx a in it.Attrs) 
                        {
                            it.Result.Attrs.Add(a.Attr);
                            it.Result.Tokens.Add(a.Token);
                        }
                    }
                }
                if (it.Result != null) 
                {
                    if (it.Result.Graph != it.ResGraph) 
                    {
                    }
                    allItems.Add(it);
                }
            }
            if (BestVar != null) 
            {
                foreach (NGSegmentVariant s in BestVar.Segs) 
                {
                    if (s != null) 
                        this._createLists(s);
                }
            }
            if (BestVar != null) 
            {
                foreach (NGSegmentVariant s in BestVar.Segs) 
                {
                    if (s != null) 
                        this._createLinks(s);
                }
            }
            for (int i = 0; i < Items.Count; i++) 
            {
                SentItem it = Items[i];
                if (it.Typ != SentItemType.Adverb || it.ResGraph == null) 
                    continue;
                AdverbToken adv = it.Source as AdverbToken;
                if (adv.Typ != Pullenti.Semantic.SemAttributeType.Undefined) 
                    continue;
                SentItem before = null;
                SentItem after = null;
                for (int ii = i - 1; ii >= 0; ii--) 
                {
                    SentItem it0 = Items[ii];
                    if (it0.Typ == SentItemType.Verb) 
                    {
                        before = it0;
                        break;
                    }
                    else if (it0.Typ == SentItemType.Adverb || it0.Typ == SentItemType.Noun) 
                    {
                    }
                    else 
                        break;
                }
                if (before == null) 
                {
                    for (int ii = i - 1; ii >= 0; ii--) 
                    {
                        SentItem it0 = Items[ii];
                        if (it0.Typ == SentItemType.Verb || it0.Typ == SentItemType.Noun) 
                        {
                            before = it0;
                            break;
                        }
                        else if (it0.Typ == SentItemType.Adverb) 
                        {
                        }
                        else 
                            break;
                    }
                }
                bool commaAfter = false;
                for (int ii = i + 1; ii < Items.Count; ii++) 
                {
                    SentItem it0 = Items[ii];
                    if (it0.Typ == SentItemType.Verb || it0.Typ == SentItemType.Noun) 
                    {
                        after = it0;
                        break;
                    }
                    else if (it0.Typ == SentItemType.Adverb) 
                    {
                    }
                    else if (it0.CanBeCommaEnd) 
                    {
                        if (before != null && before.Typ == SentItemType.Verb) 
                            break;
                        if (((ii + 1) < Items.Count) && ((Items[ii + 1].Typ == SentItemType.Adverb || Items[ii + 1].Typ == SentItemType.Verb))) 
                        {
                        }
                        else 
                            commaAfter = true;
                    }
                    else 
                        break;
                }
                if (before != null && after != null) 
                {
                    if (commaAfter) 
                        after = null;
                    else if (before.Typ == SentItemType.Noun && after.Typ == SentItemType.Verb) 
                        before = null;
                    else if (before.Typ == SentItemType.Verb && after.Typ == SentItemType.Noun) 
                        after = null;
                }
                it.Result = CreateHelper.CreateAdverb(it.ResGraph, adv);
                if (it.Attrs != null) 
                {
                    foreach (SemAttributeEx a in it.Attrs) 
                    {
                        it.Result.Attrs.Add(a.Attr);
                        it.Result.Tokens.Add(a.Token);
                    }
                }
                if (after != null || before != null) 
                    it.ResGraph.AddLink(Pullenti.Semantic.SemLinkType.Detail, (after == null ? before.Result : after.Result), it.Result, "как", false, null);
            }
            List<SentItem> preds = new List<SentItem>();
            SentItem agent = null;
            foreach (SentItem it in Items) 
            {
                if (it.Result != null && it.Typ == SentItemType.Verb && (it.Source is Pullenti.Ner.Core.VerbPhraseToken)) 
                {
                    if (agent != null) 
                    {
                        bool hasPac = false;
                        foreach (Pullenti.Semantic.SemLink li in it.ResGraph.Links) 
                        {
                            if (li.Typ == Pullenti.Semantic.SemLinkType.Pacient && li.Source == it.Result) 
                            {
                                hasPac = true;
                                break;
                            }
                        }
                        if (!hasPac) 
                        {
                            NGItem ni0 = new NGItem() { Source = agent };
                            NGLink gli0 = new NGLink() { From = ni0, ToVerb = it.Source as Pullenti.Ner.Core.VerbPhraseToken, Typ = NGLinkType.Pacient };
                            if (agent.ResultList != null) 
                            {
                                gli0.FromIsPlural = true;
                                gli0.CalcCoef(false);
                                if (gli0.Coef > 0 && gli0.Plural == 1) 
                                {
                                    foreach (Pullenti.Semantic.SemObject ii in agent.ResultList) 
                                    {
                                        it.ResGraph.AddLink(Pullenti.Semantic.SemLinkType.Pacient, it.Result, ii, null, false, null);
                                    }
                                    Coef += 1;
                                }
                            }
                            else 
                            {
                                gli0.CalcCoef(true);
                                if (gli0.Coef > 0) 
                                {
                                    it.ResGraph.AddLink(Pullenti.Semantic.SemLinkType.Pacient, it.Result, agent.Result, null, false, null);
                                    Coef += 1;
                                }
                            }
                        }
                    }
                    Pullenti.Semantic.SemLink ali = null;
                    foreach (Pullenti.Semantic.SemLink li in it.ResGraph.Links) 
                    {
                        if (li.Typ == Pullenti.Semantic.SemLinkType.Agent && li.Source == it.Result) 
                        {
                            ali = li;
                            break;
                        }
                    }
                    if (ali != null) 
                    {
                        agent = this._findItemByRes(ali.Target);
                        continue;
                    }
                    if (agent == null) 
                        continue;
                    NGItem ni = new NGItem() { Source = agent };
                    NGLink gli = new NGLink() { From = ni, ToVerb = it.Source as Pullenti.Ner.Core.VerbPhraseToken, Typ = NGLinkType.Agent };
                    if (agent.ResultList != null) 
                    {
                        gli.FromIsPlural = true;
                        gli.CalcCoef(false);
                        if (gli.Coef > 0 && gli.Plural == 1) 
                        {
                            foreach (Pullenti.Semantic.SemObject ii in agent.ResultList) 
                            {
                                it.ResGraph.AddLink(Pullenti.Semantic.SemLinkType.Agent, it.Result, ii, null, false, null);
                            }
                            Coef += 1;
                        }
                    }
                    else 
                    {
                        gli.CalcCoef(true);
                        if (gli.Coef > 0) 
                        {
                            it.ResGraph.AddLink(Pullenti.Semantic.SemLinkType.Agent, it.Result, agent.Result, null, false, null);
                            Coef += 1;
                        }
                    }
                }
            }
            agent = null;
            for (int i = 0; i < Items.Count; i++) 
            {
                SentItem it = Items[i];
                if (it.Result != null && it.Typ == SentItemType.Deepart) 
                {
                }
                else 
                    continue;
                Pullenti.Semantic.SemLink link = null;
                for (int j = i - 1; j >= 0; j--) 
                {
                    SentItem itt = Items[j];
                    if (itt.Typ != SentItemType.Noun) 
                        continue;
                    if (!(itt.Source.Morph.Case.IsNominative)) 
                        continue;
                    bool ispacad = false;
                    foreach (Pullenti.Semantic.SemLink li in itt.ResGraph.Links) 
                    {
                        if (((li.Typ == Pullenti.Semantic.SemLinkType.Agent || li.Typ == Pullenti.Semantic.SemLinkType.Pacient)) && li.Target == itt.Result) 
                            ispacad = true;
                    }
                    if (!ispacad) 
                        continue;
                    if (link == null) 
                        link = itt.ResGraph.AddLink(Pullenti.Semantic.SemLinkType.Agent, it.Result, itt.Result, null, false, null);
                    else if (link.AltLink == null) 
                    {
                        link.AltLink = itt.ResGraph.AddLink(Pullenti.Semantic.SemLinkType.Agent, it.Result, itt.Result, null, false, null);
                        link.AltLink.AltLink = link;
                        break;
                    }
                }
                if (link == null) 
                {
                    for (int j = i + 1; j < Items.Count; j++) 
                    {
                        SentItem itt = Items[j];
                        if (itt.Typ != SentItemType.Noun) 
                            continue;
                        if (!(itt.Source.Morph.Case.IsNominative)) 
                            continue;
                        bool ispacad = false;
                        foreach (Pullenti.Semantic.SemLink li in itt.ResGraph.Links) 
                        {
                            if (((li.Typ == Pullenti.Semantic.SemLinkType.Agent || li.Typ == Pullenti.Semantic.SemLinkType.Pacient)) && li.Target == itt.Result) 
                                ispacad = true;
                        }
                        if (!ispacad) 
                            continue;
                        if (link == null) 
                            link = itt.ResGraph.AddLink(Pullenti.Semantic.SemLinkType.Agent, it.Result, itt.Result, null, false, null);
                        else if (link.AltLink == null) 
                        {
                            link.AltLink = itt.ResGraph.AddLink(Pullenti.Semantic.SemLinkType.Agent, it.Result, itt.Result, null, false, null);
                            link.AltLink.AltLink = link;
                            break;
                        }
                    }
                }
                if (link != null) 
                    Coef++;
            }
            foreach (Pullenti.Semantic.SemFragment fr in ResBlock.Fragments) 
            {
                if (fr.CanBeErrorStructure) 
                    Coef /= 2;
            }
            if (ResBlock.Fragments.Count > 0 && ResBlock.Fragments[0].Graph.Objects.Count > 0) 
            {
                Pullenti.Semantic.SemObject it = ResBlock.Fragments[0].Graph.Objects[0];
                if (LastChar != null && LastChar.IsChar('?')) 
                {
                    if (it.Morph.NormalFull == "КАКОЙ" || it.Morph.NormalFull == "СКОЛЬКО") 
                        it.Typ = Pullenti.Semantic.SemObjectType.Question;
                }
            }
        }
        SentItem _findItemByRes(Pullenti.Semantic.SemObject s)
        {
            foreach (SentItem it in Items) 
            {
                if (it.Result == s) 
                    return it;
            }
            return null;
        }
        public List<SentItem> Items = new List<SentItem>();
        public double Coef = 0;
        public SentenceVariant BestVar;
        public List<Subsent> Subs = new List<Subsent>();
        public Pullenti.Semantic.SemBlock ResBlock;
        public NGLinkType LastNounToFirstVerb = NGLinkType.Undefined;
        public NGLinkType NotLastNounToFirstVerb = NGLinkType.Undefined;
        public Pullenti.Ner.TextToken LastChar;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Coef > 0) 
                res.AppendFormat("{0}: ", Coef);
            foreach (SentItem it in Items) 
            {
                if (it != Items[0]) 
                    res.Append("; \r\n");
                res.Append(it.ToString());
            }
            return res.ToString();
        }
        public void AddToBlock(Pullenti.Semantic.SemBlock blk, Pullenti.Semantic.SemGraph gr = null)
        {
            if (ResBlock != null) 
            {
                if (gr == null) 
                    blk.AddFragments(ResBlock);
                else 
                    foreach (Pullenti.Semantic.SemFragment fr in ResBlock.Fragments) 
                    {
                        gr.Objects.AddRange(fr.Graph.Objects);
                        gr.Links.AddRange(fr.Graph.Links);
                    }
            }
            foreach (SentItem it in Items) 
            {
                if (it.SubSent != null) 
                    it.SubSent.AddToBlock(blk, gr ?? it.ResFrag.Graph);
            }
        }
        public static List<Sentence> ParseVariants(Pullenti.Ner.Token t0, Pullenti.Ner.Token t1, int lev, int maxCount = 0, SentItemType regime = SentItemType.Undefined)
        {
            if ((t0 == null || t1 == null || t0.EndChar > t1.EndChar) || lev > 100) 
                return null;
            List<Sentence> res = new List<Sentence>();
            Sentence sent = new Sentence();
            for (Pullenti.Ner.Token t = t0; t != null && t.EndChar <= t1.EndChar; t = t.Next) 
            {
                if (t.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        t = br.EndToken;
                        continue;
                    }
                }
                List<SentItem> items = SentItem.ParseNearItems(t, t1, lev + 1, sent.Items);
                if (items == null || items.Count == 0) 
                    continue;
                if (items.Count == 1 || ((maxCount > 0 && res.Count > maxCount))) 
                {
                    sent.Items.Add(items[0]);
                    t = items[0].EndToken;
                    if (regime != SentItemType.Undefined) 
                    {
                        SentItem it = items[0];
                        if (it.CanBeNoun) 
                        {
                        }
                        else if (it.Typ == SentItemType.Delim) 
                            break;
                        else if (it.Typ == SentItemType.Verb) 
                        {
                            if (regime == SentItemType.PartBefore) 
                                break;
                        }
                    }
                    continue;
                }
                Dictionary<int, List<Sentence>> m_Nexts = new Dictionary<int, List<Sentence>>();
                foreach (SentItem it in items) 
                {
                    List<Sentence> nexts = null;
                    if (!m_Nexts.TryGetValue(it.EndToken.EndChar, out nexts)) 
                    {
                        nexts = ParseVariants(it.EndToken.Next, t1, lev + 1, maxCount, SentItemType.Undefined);
                        m_Nexts.Add(it.EndToken.EndChar, nexts);
                    }
                    if (nexts == null || nexts.Count == 0) 
                    {
                        Sentence se = new Sentence();
                        foreach (SentItem itt in sent.Items) 
                        {
                            SentItem itt1 = new SentItem(null);
                            itt1.CopyFrom(itt);
                            se.Items.Add(itt1);
                        }
                        SentItem itt0 = new SentItem(null);
                        itt0.CopyFrom(it);
                        se.Items.Add(itt0);
                        res.Add(se);
                    }
                    else 
                        foreach (Sentence sn in nexts) 
                        {
                            Sentence se = new Sentence();
                            foreach (SentItem itt in sent.Items) 
                            {
                                SentItem itt1 = new SentItem(null);
                                itt1.CopyFrom(itt);
                                se.Items.Add(itt1);
                            }
                            SentItem itt0 = new SentItem(null);
                            itt0.CopyFrom(it);
                            se.Items.Add(itt0);
                            foreach (SentItem itt in sn.Items) 
                            {
                                SentItem itt1 = new SentItem(null);
                                itt1.CopyFrom(itt);
                                se.Items.Add(itt1);
                            }
                            res.Add(se);
                        }
                }
                return res;
            }
            if (sent.Items.Count == 0) 
                return null;
            res.Add(sent);
            return res;
        }
        public int CompareTo(Sentence other)
        {
            if (Coef > other.Coef) 
                return -1;
            if (Coef < other.Coef) 
                return 1;
            return 0;
        }
        public void CalcCoef(bool noResult)
        {
            Coef = 0;
            for (int i = 0; i < Items.Count; i++) 
            {
                SentItem it = Items[i];
                if (it.Typ != SentItemType.Adverb) 
                    continue;
                AdverbToken adv = it.Source as AdverbToken;
                if (adv.Typ == Pullenti.Semantic.SemAttributeType.Undefined) 
                    continue;
                SentItem before = null;
                SentItem after = null;
                for (int ii = i - 1; ii >= 0; ii--) 
                {
                    SentItem it0 = Items[ii];
                    if (it0.Typ == SentItemType.Verb) 
                    {
                        before = it0;
                        break;
                    }
                    else if (it0.Typ == SentItemType.Adverb) 
                    {
                        if ((it0.Source as AdverbToken).Typ == Pullenti.Semantic.SemAttributeType.Undefined) 
                        {
                            before = it0;
                            break;
                        }
                    }
                    else if (it0.CanBeCommaEnd) 
                        break;
                    else if (it0.Typ == SentItemType.Formula && ((adv.Typ == Pullenti.Semantic.SemAttributeType.Great || adv.Typ == Pullenti.Semantic.SemAttributeType.Less))) 
                    {
                        before = it0;
                        break;
                    }
                }
                bool commaAfter = false;
                for (int ii = i + 1; ii < Items.Count; ii++) 
                {
                    SentItem it0 = Items[ii];
                    if (it0.Typ == SentItemType.Verb) 
                    {
                        after = it0;
                        break;
                    }
                    else if (it0.Typ == SentItemType.Adverb) 
                    {
                        if ((it0.Source as AdverbToken).Typ == Pullenti.Semantic.SemAttributeType.Undefined) 
                        {
                            after = it0;
                            break;
                        }
                    }
                    else if (it0.CanBeCommaEnd) 
                        commaAfter = true;
                    else if (it0.Typ == SentItemType.Formula && ((adv.Typ == Pullenti.Semantic.SemAttributeType.Great || adv.Typ == Pullenti.Semantic.SemAttributeType.Less))) 
                    {
                        before = it0;
                        break;
                    }
                    else if (it0.Typ == SentItemType.Noun) 
                        commaAfter = true;
                    else 
                        break;
                }
                if (before != null && after != null) 
                {
                    if (before.Typ == SentItemType.Formula) 
                        after = null;
                    else if (after.Typ == SentItemType.Formula) 
                        before = null;
                    else if (commaAfter) 
                        after = null;
                }
                if (after != null) 
                {
                    after.AddAttr(adv);
                    Items.RemoveAt(i);
                    i--;
                    continue;
                }
                if (before != null) 
                {
                    before.AddAttr(adv);
                    Items.RemoveAt(i);
                    i--;
                    continue;
                }
            }
            List<NGSegment> segs = NGSegment.CreateSegments(this);
            if (LastNounToFirstVerb != NGLinkType.Undefined || NotLastNounToFirstVerb != NGLinkType.Undefined) 
            {
                if (segs.Count != 1 || segs[0].Items.Count == 0) 
                {
                    if (LastNounToFirstVerb != NGLinkType.Undefined) 
                    {
                        Coef = -1;
                        return;
                    }
                }
                else 
                {
                    NGItem last = segs[0].Items[segs[0].Items.Count - 1];
                    for (int i = last.Links.Count - 1; i >= 0; i--) 
                    {
                        NGLink li = last.Links[i];
                        if (LastNounToFirstVerb != NGLinkType.Undefined) 
                        {
                            if (li.Typ == LastNounToFirstVerb && li.ToVerb == segs[0].BeforeVerb) 
                                li.CanBeParticiple = true;
                            else 
                                last.Links.RemoveAt(i);
                        }
                        else if (NotLastNounToFirstVerb != NGLinkType.Undefined) 
                        {
                            if (li.Typ == NotLastNounToFirstVerb && li.ToVerb == segs[0].BeforeVerb) 
                            {
                                last.Links.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    if (last.Links.Count == 0) 
                    {
                        Coef = -1;
                        return;
                    }
                }
            }
            foreach (NGSegment seg in segs) 
            {
                seg.Ind = 0;
                seg.CreateVariants(100);
            }
            List<SentenceVariant> svars = new List<SentenceVariant>();
            SentenceVariant svar = null;
            for (int kkk = 0; kkk < 1000; kkk++) 
            {
                if (svar == null) 
                    svar = new SentenceVariant();
                else 
                    svar.Segs.Clear();
                for (int i = 0; i < segs.Count; i++) 
                {
                    NGSegment it = segs[i];
                    if (it.Ind < it.Variants.Count) 
                        svar.Segs.Add(it.Variants[it.Ind]);
                    else 
                        svar.Segs.Add(null);
                }
                svar.CalcCoef();
                if (svar.Coef >= 0) 
                {
                    svars.Add(svar);
                    svar = null;
                    if (svars.Count > 100) 
                    {
                        this._sortVars(svars);
                        svars.RemoveRange(10, svars.Count - 10);
                    }
                }
                int j;
                for (j = segs.Count - 1; j >= 0; j--) 
                {
                    NGSegment it = segs[j];
                    if ((++it.Ind) >= it.Variants.Count) 
                        it.Ind = 0;
                    else 
                        break;
                }
                if (j < 0) 
                    break;
            }
            this._sortVars(svars);
            if (svars.Count > 0) 
            {
                BestVar = svars[0];
                Coef = BestVar.Coef;
            }
            else 
            {
            }
            foreach (SentItem it in Items) 
            {
                if (it.SubSent != null) 
                    Coef += it.SubSent.Coef;
            }
            foreach (SentItem it in Items) 
            {
                if (it.ParticipleCoef > 0) 
                    Coef *= it.ParticipleCoef;
            }
            Subs = Subsent.CreateSubsents(this);
            if (Items.Count == 0) 
                return;
            if (noResult) 
                return;
            ResBlock = new Pullenti.Semantic.SemBlock(null);
            foreach (Subsent sub in Subs) 
            {
                sub.ResFrag = new Pullenti.Semantic.SemFragment(ResBlock);
                ResBlock.Fragments.Add(sub.ResFrag);
                sub.ResFrag.IsOr = sub.IsOr;
                foreach (SentItem it in sub.Items) 
                {
                    if (sub.ResFrag.BeginToken == null) 
                        sub.ResFrag.BeginToken = it.BeginToken;
                    sub.ResFrag.EndToken = it.EndToken;
                    if (it.ResGraph != null) 
                    {
                    }
                    it.ResGraph = sub.ResFrag.Graph;
                    it.ResFrag = sub.ResFrag;
                }
            }
            foreach (Subsent sub in Subs) 
            {
                if (sub.ResFrag == null || sub.Owner == null || sub.Owner.ResFrag == null) 
                    continue;
                if (sub.Typ == Pullenti.Semantic.SemFraglinkType.Undefined) 
                    continue;
                ResBlock.AddLink(sub.Typ, sub.ResFrag, sub.Owner.ResFrag, sub.Question);
            }
            this.CreateResult(ResBlock);
        }
        void _sortVars(List<SentenceVariant> vars)
        {
            vars.Sort();
        }
        public bool TruncOborot(bool isParticiple)
        {
            if (BestVar == null || BestVar.Segs.Count == 0) 
            {
                if (Items.Count > 1) 
                {
                    Items.RemoveRange(1, Items.Count - 1);
                    return true;
                }
                return false;
            }
            bool ret = false;
            int ind = 0;
            if (BestVar.Segs[0] == null && !isParticiple) 
            {
                for (ind = 1; ind < Items.Count; ind++) 
                {
                    if (Items[ind].CanBeCommaEnd) 
                        break;
                }
            }
            else 
                foreach (NGSegmentVariant seg in BestVar.Segs) 
                {
                    if (seg == null) 
                        break;
                    foreach (NGLink li in seg.Links) 
                    {
                        if (li == null) 
                            continue;
                        ret = true;
                        int ii = Items.IndexOf(li.From.Source);
                        if (ii < 0) 
                            continue;
                        if (li.ToVerb != null) 
                        {
                            if (li.ToVerb == seg.Source.BeforeVerb) 
                                ind = ii + 1;
                            else if (!isParticiple && seg == BestVar.Segs[0] && li.ToVerb == seg.Source.AfterVerb) 
                            {
                                for (ii = ind; ii < Items.Count; ii++) 
                                {
                                    if (Items[ii].Source == li.ToVerb) 
                                    {
                                        ind = ii + 1;
                                        break;
                                    }
                                }
                            }
                            else 
                                break;
                        }
                        else 
                        {
                            int jj = Items.IndexOf(li.To.Source);
                            if (jj < 0) 
                                continue;
                            if (jj < ii) 
                                ind = ii + 1;
                            else 
                                break;
                        }
                    }
                    if (!isParticiple && seg == BestVar.Segs[0]) 
                    {
                    }
                    else 
                        break;
                }
            if (!ret && ind == 0) 
            {
                for (ind = 1; ind < Items.Count; ind++) 
                {
                    if (Items[ind].CanBeCommaEnd) 
                        break;
                }
            }
            if (ind > 0 && (ind < (Items.Count - 1))) 
                Items.RemoveRange(ind, Items.Count - ind);
            return ret;
        }
    }
}