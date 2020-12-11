/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Semantic.Core
{
    /// <summary>
    /// Полезные фукнции для семантического анализа
    /// </summary>
    public static class SemanticHelper
    {
        public static string GetKeyword(Pullenti.Ner.MetaToken mt)
        {
            Pullenti.Ner.Core.VerbPhraseToken vpt = mt as Pullenti.Ner.Core.VerbPhraseToken;
            if (vpt != null) 
                return vpt.LastVerb.VerbMorph.NormalFull ?? vpt.LastVerb.VerbMorph.NormalCase;
            Pullenti.Ner.Core.NounPhraseToken npt = mt as Pullenti.Ner.Core.NounPhraseToken;
            if (npt != null) 
                return npt.Noun.EndToken.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
            return null;
        }
        public static List<Pullenti.Semantic.Utils.DerivateGroup> FindDerivates(Pullenti.Ner.Token t)
        {
            List<Pullenti.Semantic.Utils.DerivateGroup> res = null;
            Pullenti.Morph.MorphClass cla = null;
            if (t is Pullenti.Ner.Core.NounPhraseToken) 
            {
                t = (t as Pullenti.Ner.Core.NounPhraseToken).Noun.EndToken;
                cla = Pullenti.Morph.MorphClass.Noun;
            }
            if (t is Pullenti.Ner.TextToken) 
            {
                foreach (Pullenti.Morph.MorphBaseInfo f in t.Morph.Items) 
                {
                    if (f is Pullenti.Morph.MorphWordForm) 
                    {
                        if (cla != null) 
                        {
                            if (((cla & f.Class)).IsUndefined) 
                                continue;
                        }
                        res = Pullenti.Semantic.Utils.DerivateService.FindDerivates((f as Pullenti.Morph.MorphWordForm).NormalFull ?? (f as Pullenti.Morph.MorphWordForm).NormalCase, true, null);
                        if (res != null && res.Count > 0) 
                            return res;
                    }
                }
                return null;
            }
            if (t is Pullenti.Ner.Core.VerbPhraseToken) 
                return FindDerivates((t as Pullenti.Ner.Core.VerbPhraseToken).LastVerb);
            if (t is Pullenti.Ner.Core.VerbPhraseItemToken) 
            {
                Pullenti.Ner.Core.VerbPhraseItemToken vpt = t as Pullenti.Ner.Core.VerbPhraseItemToken;
                if (vpt.VerbMorph != null) 
                {
                    res = Pullenti.Semantic.Utils.DerivateService.FindDerivates(vpt.VerbMorph.NormalCase, true, t.Morph.Language);
                    if (res == null || (res.Count == 0 && vpt.VerbMorph.NormalFull != null && vpt.VerbMorph.NormalCase != vpt.VerbMorph.NormalFull)) 
                        res = Pullenti.Semantic.Utils.DerivateService.FindDerivates(vpt.VerbMorph.NormalFull, true, t.Morph.Language);
                }
                return res;
            }
            if (t is Pullenti.Ner.NumberToken) 
            {
                if ((t as Pullenti.Ner.NumberToken).Value == "1") 
                    return Pullenti.Semantic.Utils.DerivateService.FindDerivates("ОДИН", true, Pullenti.Morph.MorphLang.RU);
            }
            if (t is Pullenti.Ner.MetaToken) 
                return FindDerivates((t as Pullenti.Ner.MetaToken).EndToken);
            return null;
        }
        public static Pullenti.Semantic.Utils.DerivateWord FindWordInGroup(Pullenti.Ner.MetaToken mt, Pullenti.Semantic.Utils.DerivateGroup gr)
        {
            if (gr == null || mt == null) 
                return null;
            Pullenti.Ner.Token t = null;
            if (mt is Pullenti.Ner.Core.NounPhraseToken) 
                t = (mt as Pullenti.Ner.Core.NounPhraseToken).Noun.EndToken;
            else if ((mt is SemanticAbstractSlave) && ((mt as SemanticAbstractSlave).Source is Pullenti.Ner.Core.NounPhraseToken)) 
                t = ((mt as SemanticAbstractSlave).Source as Pullenti.Ner.Core.NounPhraseToken).Noun.EndToken;
            else 
                t = mt.EndToken;
            foreach (Pullenti.Semantic.Utils.DerivateWord w in gr.Words) 
            {
                if (w.Class != null && w.Class.IsNoun && w.Lang.IsRu) 
                {
                    if (t.IsValue(w.Spelling, null)) 
                        return w;
                }
            }
            return null;
        }
        static Pullenti.Semantic.Utils.ControlModelItem FindControlItem(Pullenti.Ner.MetaToken mt, Pullenti.Semantic.Utils.DerivateGroup gr)
        {
            if (gr == null) 
                return null;
            if (mt is Pullenti.Ner.Core.NounPhraseToken) 
            {
                Pullenti.Ner.Token t = (mt as Pullenti.Ner.Core.NounPhraseToken).Noun.EndToken;
                foreach (Pullenti.Semantic.Utils.ControlModelItem m in gr.Model.Items) 
                {
                    if (m.Word != null) 
                    {
                        if (t.IsValue(m.Word, null)) 
                            return m;
                    }
                }
                foreach (Pullenti.Semantic.Utils.DerivateWord w in gr.Words) 
                {
                    if (w.Attrs.IsVerbNoun) 
                    {
                        if (t.IsValue(w.Spelling, null)) 
                            return gr.Model.FindItemByTyp(Pullenti.Semantic.Utils.ControlModelItemType.Noun);
                    }
                }
                return null;
            }
            if (mt is Pullenti.Ner.Core.VerbPhraseItemToken) 
            {
                Pullenti.Ner.Core.VerbPhraseItemToken ti = mt as Pullenti.Ner.Core.VerbPhraseItemToken;
                bool rev = ti.IsVerbReversive || ti.IsVerbPassive;
                foreach (Pullenti.Semantic.Utils.ControlModelItem it in gr.Model.Items) 
                {
                    if (rev && it.Typ == Pullenti.Semantic.Utils.ControlModelItemType.Reflexive) 
                        return it;
                    else if (!rev && it.Typ == Pullenti.Semantic.Utils.ControlModelItemType.Verb) 
                        return it;
                }
            }
            return null;
        }
        /// <summary>
        /// Попробовать создать семантическую связь между элементами. 
        /// Элементом м.б. именная (NounPhraseToken) или глагольная группа (VerbPhraseToken).
        /// </summary>
        /// <param name="master">основной элемент</param>
        /// <param name="slave">стыкуемый элемент (также м.б. SemanticAbstractSlave)</param>
        /// <param name="onto">дополнительный онтологический словарь</param>
        /// <return>список вариантов (возможно, пустой)</return>
        public static List<SemanticLink> TryCreateLinks(Pullenti.Ner.MetaToken master, Pullenti.Ner.MetaToken slave, ISemanticOnto onto = null)
        {
            List<SemanticLink> res = new List<SemanticLink>();
            Pullenti.Ner.Core.VerbPhraseToken vpt1 = master as Pullenti.Ner.Core.VerbPhraseToken;
            Pullenti.Ner.Core.VerbPhraseToken vpt2 = slave as Pullenti.Ner.Core.VerbPhraseToken;
            Pullenti.Ner.Core.NounPhraseToken npt1 = master as Pullenti.Ner.Core.NounPhraseToken;
            if (slave is Pullenti.Ner.Core.NounPhraseToken) 
                slave = SemanticAbstractSlave.CreateFromNoun(slave as Pullenti.Ner.Core.NounPhraseToken);
            SemanticAbstractSlave sla2 = slave as SemanticAbstractSlave;
            if (vpt2 != null) 
            {
                if (!vpt2.FirstVerb.IsVerbInfinitive || !vpt2.LastVerb.IsVerbInfinitive) 
                    return res;
            }
            List<Pullenti.Semantic.Utils.DerivateGroup> grs = FindDerivates(master);
            if (grs == null || grs.Count == 0) 
            {
                List<SemanticLink> rl = (vpt1 != null ? _tryCreateVerb(vpt1, slave, null) : _tryCreateNoun(npt1, slave, null));
                if (rl != null) 
                    res.AddRange(rl);
            }
            else 
                foreach (Pullenti.Semantic.Utils.DerivateGroup gr in grs) 
                {
                    List<SemanticLink> rl = (vpt1 != null ? _tryCreateVerb(vpt1, slave, gr) : _tryCreateNoun(npt1, slave, gr));
                    if (rl == null || rl.Count == 0) 
                        continue;
                    res.AddRange(rl);
                }
            if ((npt1 != null && sla2 != null && sla2.Morph.Case.IsGenitive) && sla2.Preposition == null) 
            {
                if (npt1.Noun.BeginToken.GetMorphClassInDictionary().IsPersonalPronoun) 
                {
                }
                else 
                {
                    bool hasGen = false;
                    foreach (SemanticLink r in res) 
                    {
                        if (r.Question == Pullenti.Semantic.Utils.ControlModelQuestion.BaseGenetive) 
                        {
                            hasGen = true;
                            break;
                        }
                    }
                    if (!hasGen) 
                        res.Add(new SemanticLink() { Modelled = true, Master = npt1, Slave = sla2, Rank = 0.5, Question = Pullenti.Semantic.Utils.ControlModelQuestion.BaseGenetive });
                }
            }
            if (onto != null) 
            {
                string str1 = GetKeyword(master);
                string str2 = GetKeyword(slave);
                if (str2 != null) 
                {
                    if (onto.CheckLink(str1, str2)) 
                    {
                        if (res.Count > 0) 
                        {
                            foreach (SemanticLink r in res) 
                            {
                                r.Rank += 3;
                                if (r.Role == SemanticRole.Common) 
                                    r.Role = SemanticRole.Strong;
                            }
                        }
                        else 
                            res.Add(new SemanticLink() { Role = SemanticRole.Strong, Master = master, Slave = slave, Rank = 3 });
                    }
                }
            }
            if (npt1 != null) 
            {
                if (((npt1.Adjectives.Count > 0 && npt1.Adjectives[0].BeginToken.Morph.Class.IsPronoun)) || npt1.Anafor != null) 
                {
                    foreach (SemanticLink r in res) 
                    {
                        if (r.Question == Pullenti.Semantic.Utils.ControlModelQuestion.BaseGenetive) 
                        {
                            r.Rank -= 0.5;
                            if (r.Role == SemanticRole.Strong) 
                                r.Role = SemanticRole.Common;
                        }
                    }
                }
            }
            foreach (SemanticLink r in res) 
            {
                if (r.Role == SemanticRole.Strong) 
                {
                    foreach (SemanticLink rr in res) 
                    {
                        if (rr != r && rr.Role != SemanticRole.Strong) 
                            rr.Rank /= 2;
                    }
                }
            }
            for (int i = 0; i < res.Count; i++) 
            {
                for (int j = 0; j < (res.Count - 1); j++) 
                {
                    if (res[j].CompareTo(res[j + 1]) > 0) 
                    {
                        SemanticLink r = res[j];
                        res[j] = res[j + 1];
                        res[j + 1] = r;
                    }
                }
            }
            foreach (SemanticLink r in res) 
            {
                r.Master = master;
                r.Slave = slave;
            }
            return res;
        }
        static List<SemanticLink> _tryCreateInf(Pullenti.Ner.MetaToken master, Pullenti.Ner.Core.VerbPhraseToken vpt2, Pullenti.Semantic.Utils.DerivateGroup gr)
        {
            Pullenti.Semantic.Utils.ControlModelItem cit = FindControlItem(master, gr);
            List<SemanticLink> res = new List<SemanticLink>();
            SemanticRole? rol = null;
            if (cit != null && cit.Links.ContainsKey(Pullenti.Semantic.Utils.ControlModelQuestion.ToDo)) 
                rol = cit.Links[Pullenti.Semantic.Utils.ControlModelQuestion.ToDo];
            if (rol != null) 
                res.Add(new SemanticLink() { Rank = (rol.Value != SemanticRole.Common ? 2 : 1), Question = Pullenti.Semantic.Utils.ControlModelQuestion.ToDo });
            return res;
        }
        static List<SemanticLink> _tryCreateNoun(Pullenti.Ner.Core.NounPhraseToken npt1, Pullenti.Ner.MetaToken slave, Pullenti.Semantic.Utils.DerivateGroup gr)
        {
            if (npt1 == null || slave == null) 
                return null;
            if (slave is Pullenti.Ner.Core.VerbPhraseToken) 
                return _tryCreateInf(npt1, slave as Pullenti.Ner.Core.VerbPhraseToken, gr);
            SemanticAbstractSlave sla2 = slave as SemanticAbstractSlave;
            List<SemanticLink> res = new List<SemanticLink>();
            if (sla2 == null) 
                return res;
            Pullenti.Semantic.Utils.ControlModelItem cit = FindControlItem(npt1, gr);
            _createRoles(cit, sla2.Preposition, sla2.Morph.Case, res, false, false);
            if (res.Count == 1 && res[0].Role == SemanticRole.Agent && res[0].Question == Pullenti.Semantic.Utils.ControlModelQuestion.BaseInstrumental) 
            {
                if (gr.Model.Items.Count > 0 && gr.Model.Items[0].Typ == Pullenti.Semantic.Utils.ControlModelItemType.Verb && gr.Model.Items[0].Links.ContainsKey(Pullenti.Semantic.Utils.ControlModelQuestion.BaseInstrumental)) 
                    res[0].Role = gr.Model.Items[0].Links[Pullenti.Semantic.Utils.ControlModelQuestion.BaseInstrumental];
            }
            bool ok = false;
            Pullenti.Semantic.Utils.DerivateWord w = FindWordInGroup(npt1, gr);
            if (w != null && w.NextWords != null && w.NextWords.Count > 0) 
            {
                foreach (string n in w.NextWords) 
                {
                    if (sla2.Source != null) 
                    {
                        if (sla2.Source.EndToken.IsValue(n, null)) 
                        {
                            ok = true;
                            break;
                        }
                    }
                }
            }
            if (gr != null && gr.Model.Pacients.Count > 0) 
            {
                foreach (string n in gr.Model.Pacients) 
                {
                    if (sla2.Source != null) 
                    {
                        if (sla2.Source.EndToken.IsValue(n, null)) 
                        {
                            ok = true;
                            break;
                        }
                    }
                }
            }
            if (ok) 
            {
                if (res.Count == 0) 
                    res.Add(new SemanticLink() { Question = Pullenti.Semantic.Utils.ControlModelQuestion.BaseGenetive, Role = SemanticRole.Pacient, Idiom = true });
                foreach (SemanticLink r in res) 
                {
                    r.Rank += 4;
                    if (r.Role == SemanticRole.Common) 
                        r.Role = SemanticRole.Strong;
                    if (npt1.EndToken.Next == sla2.BeginToken) 
                        r.Rank += 2;
                    r.Idiom = true;
                }
            }
            return res;
        }
        static List<SemanticLink> _tryCreateVerb(Pullenti.Ner.Core.VerbPhraseToken vpt1, Pullenti.Ner.MetaToken slave, Pullenti.Semantic.Utils.DerivateGroup gr)
        {
            if (slave is Pullenti.Ner.Core.VerbPhraseToken) 
                return _tryCreateInf(vpt1, slave as Pullenti.Ner.Core.VerbPhraseToken, gr);
            SemanticAbstractSlave sla2 = slave as SemanticAbstractSlave;
            List<SemanticLink> res = new List<SemanticLink>();
            if (sla2 == null) 
                return res;
            Pullenti.Semantic.Utils.ControlModelItem cit = FindControlItem(vpt1.LastVerb, gr);
            string prep = sla2.Preposition;
            Pullenti.Morph.MorphBaseInfo morph = (Pullenti.Morph.MorphBaseInfo)sla2.Morph;
            bool isRev1 = vpt1.LastVerb.IsVerbReversive || vpt1.LastVerb.IsVerbPassive;
            bool noNomin = false;
            bool noInstr = false;
            if (prep == null && morph.Case.IsNominative && !vpt1.FirstVerb.IsParticiple) 
            {
                bool ok = true;
                bool err = false;
                Pullenti.Morph.MorphWordForm vm = vpt1.FirstVerb.VerbMorph;
                if (vm == null) 
                    return res;
                if (vm.Number == Pullenti.Morph.MorphNumber.Singular) 
                {
                    if (morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                    {
                        if (!vpt1.FirstVerb.IsVerbInfinitive) 
                            ok = false;
                    }
                }
                if (!CheckMorphAccord(morph, false, vm, false)) 
                {
                    if (!err && !vpt1.FirstVerb.IsVerbInfinitive) 
                        ok = false;
                }
                else if (vm.Misc.Person != Pullenti.Morph.MorphPerson.Undefined) 
                {
                    if (((vm.Misc.Person & Pullenti.Morph.MorphPerson.Third)) == Pullenti.Morph.MorphPerson.Undefined) 
                    {
                        if (((vm.Misc.Person & Pullenti.Morph.MorphPerson.First)) == Pullenti.Morph.MorphPerson.First) 
                        {
                            if (!morph.ContainsAttr("1 л.", null)) 
                                ok = false;
                        }
                        if (((vm.Misc.Person & Pullenti.Morph.MorphPerson.Second)) == Pullenti.Morph.MorphPerson.Second) 
                        {
                            if (!morph.ContainsAttr("2 л.", null)) 
                                ok = false;
                        }
                    }
                }
                noNomin = true;
                if (ok) 
                {
                    Pullenti.Semantic.Utils.ControlModelItem cit00 = cit;
                    bool isRev0 = isRev1;
                    if (vpt1.FirstVerb != vpt1.LastVerb && ((vpt1.FirstVerb.IsVerbReversive || vpt1.FirstVerb.IsVerbPassive || vpt1.FirstVerb.Normal == "ИМЕТЬ"))) 
                    {
                        cit00 = null;
                        isRev0 = true;
                        List<Pullenti.Semantic.Utils.DerivateGroup> grs = FindDerivates(vpt1.FirstVerb);
                        if (grs != null) 
                        {
                            foreach (Pullenti.Semantic.Utils.DerivateGroup gg in grs) 
                            {
                                if ((((cit00 = FindControlItem(vpt1.FirstVerb, gg)))) != null) 
                                    break;
                            }
                        }
                    }
                    SemanticLink sl = null;
                    bool addagent = false;
                    if (cit00 == null) 
                        sl = new SemanticLink() { Modelled = true, Role = (isRev0 ? SemanticRole.Pacient : SemanticRole.Agent), Rank = 1, Question = Pullenti.Semantic.Utils.ControlModelQuestion.BaseNominative, IsPassive = isRev0 };
                    else 
                        foreach (KeyValuePair<Pullenti.Semantic.Utils.ControlModelQuestion, SemanticRole> kp in cit00.Links) 
                        {
                            Pullenti.Semantic.Utils.ControlModelQuestion q = kp.Key;
                            if (q.Check(null, Pullenti.Morph.MorphCase.Nominative)) 
                            {
                                sl = new SemanticLink() { Role = kp.Value, Rank = 2, Question = q, IsPassive = isRev0 };
                                if (sl.Role == SemanticRole.Agent) 
                                    sl.IsPassive = false;
                                else if (sl.Role == SemanticRole.Pacient && cit00.NominativeCanBeAgentAndPacient && vpt1.LastVerb.IsVerbReversive) 
                                    addagent = true;
                                break;
                            }
                        }
                    if (sl != null) 
                    {
                        if (cit00 == null && morph.Case.IsInstrumental && isRev0) 
                            sl.Rank -= 0.5;
                        if (morph.Case.IsAccusative) 
                            sl.Rank -= 0.5;
                        if (sla2.BeginChar > vpt1.BeginChar) 
                            sl.Rank -= 0.5;
                        if (err) 
                            sl.Rank -= 0.5;
                        res.Add(sl);
                        if (addagent) 
                            res.Add(new SemanticLink() { Role = SemanticRole.Agent, Rank = sl.Rank, Question = sl.Question });
                    }
                }
            }
            if (prep == null && isRev1 && morph.Case.IsInstrumental) 
            {
                noInstr = true;
                Pullenti.Semantic.Utils.ControlModelItem cit00 = cit;
                SemanticLink sl = null;
                if (cit00 == null) 
                    sl = new SemanticLink() { Modelled = true, Role = SemanticRole.Agent, Rank = 1, Question = Pullenti.Semantic.Utils.ControlModelQuestion.BaseInstrumental, IsPassive = true };
                else 
                    foreach (KeyValuePair<Pullenti.Semantic.Utils.ControlModelQuestion, SemanticRole> kp in cit00.Links) 
                    {
                        Pullenti.Semantic.Utils.ControlModelQuestion q = kp.Key;
                        if (q.Check(null, Pullenti.Morph.MorphCase.Instrumental)) 
                        {
                            sl = new SemanticLink() { Role = kp.Value, Rank = 2, Question = q };
                            if (sl.Role == SemanticRole.Agent) 
                                sl.IsPassive = true;
                            break;
                        }
                    }
                if (sl != null) 
                {
                    if (cit00 == null && morph.Case.IsNominative) 
                        sl.Rank -= 0.5;
                    if (morph.Case.IsAccusative) 
                        sl.Rank -= 0.5;
                    if (sla2.BeginChar < vpt1.BeginChar) 
                        sl.Rank -= 0.5;
                    res.Add(sl);
                    if ((gr != null && gr.Model.Items.Count > 0 && gr.Model.Items[0].Typ == Pullenti.Semantic.Utils.ControlModelItemType.Verb) && gr.Model.Items[0].Links.ContainsKey(Pullenti.Semantic.Utils.ControlModelQuestion.BaseInstrumental)) 
                    {
                        sl.Rank = 0;
                        SemanticLink sl0 = new SemanticLink() { Question = sl.Question, Rank = 1, Role = gr.Model.Items[0].Links[Pullenti.Semantic.Utils.ControlModelQuestion.BaseInstrumental] };
                        res.Insert(0, sl0);
                    }
                }
            }
            if (prep == null && morph.Case.IsDative && ((cit == null || !cit.Links.ContainsKey(Pullenti.Semantic.Utils.ControlModelQuestion.BaseDative)))) 
            {
                SemanticLink sl = new SemanticLink() { Modelled = cit == null, Role = SemanticRole.Strong, Rank = 1, Question = Pullenti.Semantic.Utils.ControlModelQuestion.BaseDative };
                if (morph.Case.IsAccusative || morph.Case.IsNominative) 
                    sl.Rank -= 0.5;
                if (vpt1.EndToken.Next != sla2.BeginToken) 
                    sl.Rank -= 0.5;
                if (cit != null) 
                    sl.Rank -= 0.5;
                res.Add(sl);
            }
            _createRoles(cit, prep, morph.Case, res, noNomin, noInstr);
            if (gr != null && gr.Model.Pacients.Count > 0) 
            {
                bool ok = false;
                foreach (string n in gr.Model.Pacients) 
                {
                    if (sla2.Source != null) 
                    {
                        if (sla2.Source.EndToken.IsValue(n, null)) 
                        {
                            ok = true;
                            break;
                        }
                    }
                    else if (sla2.EndToken.IsValue(n, null)) 
                    {
                        ok = true;
                        break;
                    }
                }
                if (ok) 
                {
                    if (res.Count == 0) 
                    {
                        ok = false;
                        if (prep == null && isRev1 && morph.Case.IsNominative) 
                            ok = true;
                        else if (prep == null && !isRev1 && morph.Case.IsAccusative) 
                            ok = true;
                        if (ok) 
                            res.Add(new SemanticLink() { Role = SemanticRole.Pacient, Question = (isRev1 ? Pullenti.Semantic.Utils.ControlModelQuestion.BaseNominative : Pullenti.Semantic.Utils.ControlModelQuestion.BaseAccusative), Idiom = true });
                    }
                    else 
                        foreach (SemanticLink r in res) 
                        {
                            r.Rank += 4;
                            if (r.Role == SemanticRole.Common) 
                                r.Role = SemanticRole.Strong;
                            if (vpt1.EndToken.Next == sla2.BeginToken) 
                                r.Rank += 2;
                            r.Idiom = true;
                        }
                }
            }
            return res;
        }
        static void _createRoles(Pullenti.Semantic.Utils.ControlModelItem cit, string prep, Pullenti.Morph.MorphCase cas, List<SemanticLink> res, bool ignoreNominCase = false, bool ignoreInstrCase = false)
        {
            if (cit == null) 
                return;
            Dictionary<Pullenti.Semantic.Utils.ControlModelQuestion, SemanticRole> roles = null;
            foreach (KeyValuePair<Pullenti.Semantic.Utils.ControlModelQuestion, SemanticRole> li in cit.Links) 
            {
                Pullenti.Semantic.Utils.ControlModelQuestion q = li.Key;
                if (q.Check(prep, cas)) 
                {
                    if (ignoreNominCase && q.Case.IsNominative && q.Preposition == null) 
                        continue;
                    if (ignoreInstrCase && q.Case.IsInstrumental && q.Preposition == null) 
                        continue;
                    if (roles == null) 
                        roles = new Dictionary<Pullenti.Semantic.Utils.ControlModelQuestion, SemanticRole>();
                    SemanticRole r = li.Value;
                    if (q.IsAbstract) 
                    {
                        Pullenti.Semantic.Utils.ControlModelQuestion qq = q.CheckAbstract(prep, cas);
                        if (qq != null) 
                        {
                            q = qq;
                            r = SemanticRole.Common;
                        }
                    }
                    if (!roles.ContainsKey(q)) 
                        roles.Add(q, r);
                    else if (r != SemanticRole.Common) 
                        roles[q] = r;
                }
            }
            if (roles != null) 
            {
                foreach (KeyValuePair<Pullenti.Semantic.Utils.ControlModelQuestion, SemanticRole> kp in roles) 
                {
                    SemanticLink sl = new SemanticLink() { Role = kp.Value, Rank = 2, Question = kp.Key };
                    if (kp.Value == SemanticRole.Agent) 
                    {
                        if (!kp.Key.IsBase) 
                            sl.Role = SemanticRole.Common;
                    }
                    if (sl.Role == SemanticRole.Strong) 
                        sl.Rank += 2;
                    res.Add(sl);
                }
            }
        }
        public static bool CheckMorphAccord(Pullenti.Morph.MorphBaseInfo m, bool plural, Pullenti.Morph.MorphBaseInfo vf, bool checkCase = false)
        {
            if (checkCase && !m.Case.IsUndefined && !vf.Case.IsUndefined) 
            {
                if (((m.Case & vf.Case)).IsUndefined) 
                    return false;
            }
            double coef = (double)0;
            if (vf.Number == Pullenti.Morph.MorphNumber.Plural) 
            {
                if (plural) 
                    coef++;
                else if (m.Number != Pullenti.Morph.MorphNumber.Undefined) 
                {
                    if (((m.Number & Pullenti.Morph.MorphNumber.Plural)) == Pullenti.Morph.MorphNumber.Plural) 
                        coef++;
                    else 
                        return false;
                }
            }
            else if (vf.Number == Pullenti.Morph.MorphNumber.Singular) 
            {
                if (plural) 
                    return false;
                if (m.Number != Pullenti.Morph.MorphNumber.Undefined) 
                {
                    if (((m.Number & Pullenti.Morph.MorphNumber.Singular)) == Pullenti.Morph.MorphNumber.Singular) 
                        coef++;
                    else 
                        return false;
                }
                if (m.Gender != Pullenti.Morph.MorphGender.Undefined) 
                {
                    if (vf.Gender != Pullenti.Morph.MorphGender.Undefined) 
                    {
                        if (m.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        {
                            if (((vf.Gender & Pullenti.Morph.MorphGender.Feminie)) != Pullenti.Morph.MorphGender.Undefined) 
                                coef++;
                            else 
                                return false;
                        }
                        else if (((m.Gender & vf.Gender)) != Pullenti.Morph.MorphGender.Undefined) 
                            coef++;
                        else if (m.Gender == Pullenti.Morph.MorphGender.Masculine && vf.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        {
                        }
                        else 
                            return false;
                    }
                }
            }
            return coef >= 0;
        }
    }
}