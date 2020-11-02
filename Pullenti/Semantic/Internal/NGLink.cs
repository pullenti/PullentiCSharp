/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Semantic.Internal
{
    class NGLink : IComparable<NGLink>
    {
        public NGLinkType Typ = NGLinkType.Undefined;
        public NGItem From;
        public Pullenti.Ner.MorphCollection FromMorph
        {
            get
            {
                if (From.Source.Source != null) 
                    return From.Source.Source.Morph;
                return null;
            }
        }
        public string FromPrep
        {
            get
            {
                return From.Source.Prep ?? "";
            }
        }
        public NGItem To;
        public Pullenti.Ner.MorphCollection ToMorph
        {
            get
            {
                if (To != null && To.Source.Source != null) 
                    return To.Source.Source.Morph;
                return null;
            }
        }
        public Pullenti.Ner.Core.VerbPhraseToken ToVerb;
        public double Coef;
        public int Plural = -1;
        public bool FromIsPlural;
        public bool Reverce;
        /// <summary>
        /// Применима ко всем To списка, а не только к последнему
        /// </summary>
        public bool ToAllListItems;
        public bool CanBePacient;
        public bool CanBeParticiple;
        public NGLink AltLink;
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            tmp.AppendFormat("{0}: {1} ", Coef, Typ.ToString());
            if (Plural == 1) 
                tmp.Append(" PLURAL ");
            else if (Plural == 0) 
                tmp.Append(" SINGLE ");
            if (Reverce) 
                tmp.Append(" REVERCE ");
            tmp.AppendFormat("{0}", From.Source.ToString());
            if (ToAllListItems) 
                tmp.Append(" ALLLISTITEMS ");
            if (To != null) 
                tmp.AppendFormat(" -> {0}", To.Source.ToString());
            else if (ToVerb != null) 
                tmp.AppendFormat(" -> {0}", ToVerb.ToString());
            if (AltLink != null) 
                tmp.AppendFormat(" / ALTLINK: {0}", AltLink.ToString());
            return tmp.ToString();
        }
        public int CompareTo(NGLink other)
        {
            if (Coef > other.Coef) 
                return -1;
            if (Coef < other.Coef) 
                return 1;
            return 0;
        }
        public void CalcCoef(bool noplural = false)
        {
            Coef = -1;
            CanBePacient = false;
            ToAllListItems = false;
            Plural = -1;
            if (Typ == NGLinkType.Genetive && To != null) 
                this._calcGenetive();
            else if (Typ == NGLinkType.Name && To != null) 
                this._calcName(noplural);
            else if (Typ == NGLinkType.Be && To != null) 
                this._calcBe();
            else if (Typ == NGLinkType.List) 
                this._calcList();
            else if (Typ == NGLinkType.Participle && To != null) 
                this._calcParticiple(noplural);
            else if (ToVerb != null && ToVerb.FirstVerb != null) 
            {
                if (Typ == NGLinkType.Agent) 
                    this._calcAgent(noplural);
                else if (Typ == NGLinkType.Pacient) 
                    this._calcPacient(noplural);
                else if (Typ == NGLinkType.Actant) 
                    this._calcActant();
            }
            else if (Typ == NGLinkType.Adverb) 
                this._calcAdverb();
        }
        void _calcGenetive()
        {
            if (!From.Source.CanBeNoun) 
                return;
            if (From.Source.Typ == SentItemType.Formula) 
            {
                if (To.Source.Typ != SentItemType.Noun) 
                    return;
                Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                return;
            }
            Pullenti.Ner.MorphCollection frmorph = FromMorph;
            if (To.Source.Typ == SentItemType.Formula) 
            {
                if (From.Source.Typ != SentItemType.Noun) 
                    return;
                if (frmorph.Case.IsGenitive) 
                    Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                else if (frmorph.Case.IsUndefined) 
                    Coef = 0;
                return;
            }
            if (From.Source.Source is Pullenti.Ner.Measure.Internal.NumbersWithUnitToken) 
            {
                if (From.Order != (To.Order + 1)) 
                    return;
                Pullenti.Ner.Measure.Internal.NumbersWithUnitToken num = From.Source.Source as Pullenti.Ner.Measure.Internal.NumbersWithUnitToken;
                Pullenti.Ner.Measure.MeasureKind ki = Pullenti.Ner.Measure.Internal.UnitToken.CalcKind(num.Units);
                if (ki != Pullenti.Ner.Measure.MeasureKind.Undefined) 
                {
                    if (Pullenti.Ner.Measure.Internal.UnitsHelper.CheckKeyword(ki, To.Source.Source)) 
                    {
                        Coef = Pullenti.Semantic.SemanticService.Params.NextModel * 3;
                        return;
                    }
                }
                if (To.Source.Source is Pullenti.Ner.Measure.Internal.NumbersWithUnitToken) 
                    return;
            }
            bool nonGenText = false;
            if (string.IsNullOrEmpty(FromPrep) && !(From.Source.Source is Pullenti.Ner.Core.VerbPhraseToken)) 
            {
                if (From.Order != (To.Order + 1)) 
                    nonGenText = true;
            }
            if (To.Source.DrGroups != null) 
            {
                foreach (Pullenti.Semantic.Utils.DerivateGroup gr in To.Source.DrGroups) 
                {
                    if (gr.Cm.Transitive && string.IsNullOrEmpty(FromPrep)) 
                    {
                        bool ok = false;
                        if (To.Source.Source is Pullenti.Ner.Core.VerbPhraseToken) 
                        {
                            if (frmorph.Case.IsAccusative) 
                            {
                                ok = true;
                                CanBePacient = true;
                            }
                        }
                        else if (frmorph.Case.IsGenitive && From.Order == (To.Order + 1)) 
                            ok = true;
                        if (ok) 
                        {
                            Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                            return;
                        }
                    }
                    if (((gr.Cm.Questions & Pullenti.Semantic.Utils.QuestionType.WhatToDo)) != Pullenti.Semantic.Utils.QuestionType.Undefined && (From.Source.Source is Pullenti.Ner.Core.VerbPhraseToken)) 
                    {
                        Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                        return;
                    }
                    if (gr.Cm.Nexts != null) 
                    {
                        if (gr.Cm.Nexts.ContainsKey(FromPrep)) 
                        {
                            Pullenti.Morph.MorphCase cas = gr.Cm.Nexts[FromPrep];
                            if (!((cas & frmorph.Case)).IsUndefined) 
                            {
                                if (string.IsNullOrEmpty(FromPrep) && From.Order != (To.Order + 1) && ((cas & frmorph.Case)).IsGenitive) 
                                {
                                }
                                else 
                                {
                                    Coef = Pullenti.Semantic.SemanticService.Params.NextModel;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            if (nonGenText || !string.IsNullOrEmpty(FromPrep)) 
                return;
            Pullenti.Morph.MorphCase cas0 = frmorph.Case;
            if (cas0.IsGenitive || cas0.IsInstrumental || cas0.IsDative) 
            {
                if ((To.Source.Source is Pullenti.Ner.Measure.Internal.NumbersWithUnitToken) && cas0.IsGenitive) 
                    Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                else 
                {
                    Coef = Pullenti.Semantic.SemanticService.Params.NgLink;
                    if (cas0.IsNominative || From.Source.Typ == SentItemType.PartBefore) 
                        Coef /= 2;
                    if (!cas0.IsGenitive) 
                        Coef /= 2;
                }
            }
            else if (From.Source.Source is Pullenti.Ner.Core.VerbPhraseToken) 
                Coef = 0.1;
            if ((To.Source.Source is Pullenti.Ner.Measure.Internal.NumbersWithUnitToken) && To.Source.EndToken.IsValue("ЧЕМ", null)) 
                Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef * 2;
        }
        void _calcBe()
        {
            if (To.Source.Typ != SentItemType.Noun || From.Source.Typ != SentItemType.Noun) 
                return;
            Pullenti.Ner.MorphCollection fm = From.Source.Source.Morph;
            Pullenti.Ner.MorphCollection tm = To.Source.Source.Morph;
            if (!(tm.Case.IsNominative)) 
                return;
            if (!string.IsNullOrEmpty(FromPrep)) 
                return;
            if (From.Source.Source is Pullenti.Ner.Measure.Internal.NumbersWithUnitToken) 
            {
                Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                return;
            }
            if (!fm.Case.IsUndefined) 
            {
                if (!fm.Case.IsNominative) 
                    return;
            }
            Coef = 0;
        }
        void _calcName(bool noplural)
        {
            if (!string.IsNullOrEmpty(FromPrep)) 
                return;
            if (!(From.Source.Source is Pullenti.Ner.Core.NounPhraseToken) || From.Source.Typ != SentItemType.Noun) 
                return;
            if (From.Source.BeginToken.Chars.IsAllLower) 
                return;
            if (!(To.Source.Source is Pullenti.Ner.Core.NounPhraseToken) || To.Source.Typ != SentItemType.Noun) 
                return;
            if (From.Order != (To.Order + 1) && !noplural) 
                return;
            Pullenti.Ner.MorphCollection fm = From.Source.Source.Morph;
            Pullenti.Ner.MorphCollection tm = To.Source.Source.Morph;
            if (!fm.Case.IsUndefined && !tm.Case.IsUndefined) 
            {
                if (((tm.Case & fm.Case)).IsUndefined) 
                    return;
            }
            if (fm.Number == Pullenti.Morph.MorphNumber.Plural) 
            {
                if (noplural) 
                {
                    if (FromIsPlural) 
                    {
                    }
                    else if (((tm.Number & Pullenti.Morph.MorphNumber.Singular)) != Pullenti.Morph.MorphNumber.Undefined) 
                        return;
                }
                Plural = 1;
                Coef = Pullenti.Semantic.SemanticService.Params.VerbPlural;
            }
            else 
            {
                if (fm.Number == Pullenti.Morph.MorphNumber.Singular) 
                    Plural = 0;
                if (_checkMorphAccord(fm, false, tm)) 
                    Coef = Pullenti.Semantic.SemanticService.Params.MorphAccord;
            }
        }
        void _calcAdverb()
        {
            if (ToVerb != null) 
                Coef = 1;
            else if (To == null) 
                return;
            else if (To.Source.Typ == SentItemType.Adverb) 
                Coef = 1;
            else 
                Coef = 0.5;
        }
        void _calcList()
        {
            Pullenti.Morph.MorphCase cas0 = FromMorph.Case;
            if (To == null) 
            {
                if (ToVerb == null) 
                    return;
                return;
            }
            if (From.Source.Typ != To.Source.Typ) 
            {
                if (From.Source.Prep == To.Source.Prep && ((From.Source.Typ == SentItemType.Noun || From.Source.Typ == SentItemType.PartBefore || From.Source.Typ == SentItemType.PartAfter)) && ((To.Source.Typ == SentItemType.Noun || To.Source.Typ == SentItemType.PartBefore || To.Source.Typ == SentItemType.PartAfter))) 
                {
                }
                else 
                    return;
            }
            Pullenti.Morph.MorphCase cas1 = ToMorph.Case;
            if (!((cas0 & cas1)).IsUndefined) 
            {
                Coef = Pullenti.Semantic.SemanticService.Params.List;
                if (string.IsNullOrEmpty(FromPrep) && !string.IsNullOrEmpty(To.Source.Prep)) 
                    Coef /= 2;
                else if (!string.IsNullOrEmpty(FromPrep) && string.IsNullOrEmpty(To.Source.Prep)) 
                    Coef /= 4;
            }
            else 
            {
                if (!cas0.IsUndefined && !cas1.IsUndefined) 
                    return;
                if (!string.IsNullOrEmpty(FromPrep) && string.IsNullOrEmpty(To.Source.Prep)) 
                    return;
                Coef = Pullenti.Semantic.SemanticService.Params.List;
            }
            Pullenti.Ner.TextToken t1 = From.Source.EndToken as Pullenti.Ner.TextToken;
            Pullenti.Ner.TextToken t2 = To.Source.EndToken as Pullenti.Ner.TextToken;
            if (t1 != null && t2 != null) 
            {
                if (t1.IsValue(t2.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false), null)) 
                    Coef *= 10;
            }
            if (From.Source.Typ != To.Source.Typ) 
                Coef /= 2;
        }
        double _calcParticiple(bool noplural)
        {
            Pullenti.Ner.MorphCollection fm = From.Source.Source.Morph;
            Pullenti.Ner.MorphCollection tm = To.Source.Source.Morph;
            if (To.Source.Typ == SentItemType.PartBefore) 
                return Coef = -1;
            if (From.Source.Typ == SentItemType.Deepart) 
            {
                if (!string.IsNullOrEmpty(To.Source.Prep)) 
                    return Coef = -1;
                if (tm.Case.IsNominative) 
                    return Coef = Pullenti.Semantic.SemanticService.Params.MorphAccord;
                if (tm.Case.IsUndefined) 
                    return Coef = 0;
                return Coef = -1;
            }
            if (From.Source.Typ != SentItemType.PartBefore && From.Source.Typ != SentItemType.SubSent) 
                return Coef = -1;
            if (!fm.Case.IsUndefined && !tm.Case.IsUndefined) 
            {
                if (((fm.Case & tm.Case)).IsUndefined) 
                {
                    if (From.Source.Typ == SentItemType.PartBefore) 
                        return Coef = -1;
                }
            }
            if (fm.Number == Pullenti.Morph.MorphNumber.Plural) 
            {
                if (noplural) 
                {
                    if (FromIsPlural) 
                    {
                    }
                    else if (((tm.Number & Pullenti.Morph.MorphNumber.Plural)) == Pullenti.Morph.MorphNumber.Undefined) 
                        return Coef = -1;
                }
                Plural = 1;
                Coef = Pullenti.Semantic.SemanticService.Params.VerbPlural;
            }
            else 
            {
                if (fm.Number == Pullenti.Morph.MorphNumber.Singular) 
                    Plural = 0;
                if (fm.Items.Count > 0) 
                {
                    foreach (Pullenti.Morph.MorphBaseInfo wf in fm.Items) 
                    {
                        if (_checkMorphAccord(tm, false, wf)) 
                        {
                            Coef = Pullenti.Semantic.SemanticService.Params.MorphAccord;
                            if (tm.Gender != Pullenti.Morph.MorphGender.Undefined && wf.Gender != Pullenti.Morph.MorphGender.Undefined) 
                            {
                                if (((tm.Gender & wf.Gender)) == Pullenti.Morph.MorphGender.Undefined) 
                                    Coef /= 2;
                            }
                            break;
                        }
                    }
                }
            }
            return Coef;
        }
        double _calcAgent(bool noplural)
        {
            if (!string.IsNullOrEmpty(FromPrep)) 
                return Coef = -1;
            Pullenti.Morph.MorphWordForm vf = ToVerb.FirstVerb.VerbMorph;
            if (vf == null) 
                return Coef = -1;
            Pullenti.Morph.MorphWordForm vf2 = ToVerb.LastVerb.VerbMorph;
            if (vf2 == null) 
                return Coef = -1;
            if (vf.Misc.Mood == Pullenti.Morph.MorphMood.Imperative) 
                return Coef = -1;
            Pullenti.Ner.MorphCollection morph = FromMorph;
            if (vf2.Misc.Voice == Pullenti.Morph.MorphVoice.Passive || ToVerb.LastVerb.Morph.ContainsAttr("страд.з.", null)) 
            {
                if (!morph.Case.IsUndefined) 
                {
                    if (morph.Case.IsInstrumental) 
                    {
                        Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                        if (vf2.Case.IsInstrumental) 
                            Coef /= 2;
                        return Coef;
                    }
                    return Coef = -1;
                }
                return Coef = 0;
            }
            if (vf.Misc.Attrs.Contains("инф.")) 
                return Coef = -1;
            if (_isRevVerb(vf2)) 
            {
                Pullenti.Morph.MorphCase agCase = Pullenti.Morph.MorphCase.Undefined;
                List<Pullenti.Semantic.Utils.DerivateGroup> grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(vf2.NormalFull ?? vf2.NormalCase, true, null);
                if (grs != null) 
                {
                    foreach (Pullenti.Semantic.Utils.DerivateGroup gr in grs) 
                    {
                        if (gr.CmRev.Agent != null) 
                        {
                            agCase = gr.CmRev.Agent.Case;
                            break;
                        }
                    }
                }
                if (!morph.Case.IsUndefined) 
                {
                    if (agCase.IsDative) 
                    {
                        if (morph.Case.IsDative) 
                        {
                            Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                            if (morph.Case.IsGenitive) 
                                Coef /= 2;
                            return Coef;
                        }
                        return Coef = -1;
                    }
                    if (agCase.IsInstrumental) 
                    {
                        if (morph.Case.IsInstrumental) 
                        {
                            if (morph.Case.IsNominative) 
                                return Coef = 0;
                            return Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                        }
                        return Coef = -1;
                    }
                    if (!morph.Case.IsNominative) 
                        return Coef = -1;
                }
                else 
                    return Coef = 0;
            }
            if (vf.Number == Pullenti.Morph.MorphNumber.Plural) 
            {
                if (!morph.Case.IsUndefined) 
                {
                    if (vf.Case.IsUndefined) 
                    {
                        if (!morph.Case.IsNominative) 
                            return Coef = -1;
                    }
                    else if (((vf.Case & morph.Case)).IsUndefined) 
                        return Coef = -1;
                }
                if (noplural) 
                {
                    if (FromIsPlural) 
                    {
                    }
                    else if (((morph.Number & Pullenti.Morph.MorphNumber.Plural)) == Pullenti.Morph.MorphNumber.Undefined) 
                        return Coef = -1;
                    else if (!_checkMorphAccord(morph, false, vf)) 
                        return Coef = -1;
                    else if (morph.Items.Count > 0 && !vf.Case.IsUndefined) 
                    {
                        bool ok = false;
                        foreach (Pullenti.Morph.MorphBaseInfo it in morph.Items) 
                        {
                            if (((it.Number & Pullenti.Morph.MorphNumber.Plural)) == Pullenti.Morph.MorphNumber.Plural) 
                            {
                                if (!it.Case.IsUndefined && ((it.Case & vf.Case)).IsUndefined) 
                                    continue;
                                ok = true;
                                break;
                            }
                        }
                        if (!ok) 
                            return Coef = -1;
                    }
                }
                Plural = 1;
                Coef = Pullenti.Semantic.SemanticService.Params.VerbPlural;
                if (vf2.NormalCase == "БЫТЬ") 
                {
                    if (morph.Case.IsUndefined && From.Source.BeginToken.BeginChar > ToVerb.EndChar) 
                        Coef /= 2;
                }
            }
            else 
            {
                if (vf.Number == Pullenti.Morph.MorphNumber.Singular) 
                {
                    Plural = 0;
                    if (FromIsPlural) 
                        return Coef = -1;
                }
                if (!_checkMorphAccord(morph, false, vf)) 
                    return Coef = -1;
                if (!morph.Case.IsUndefined) 
                {
                    if (!morph.Case.IsNominative) 
                    {
                        if (ToVerb.FirstVerb.IsParticiple) 
                        {
                        }
                        else 
                            return Coef = -1;
                    }
                }
                if (vf.Misc.Person != Pullenti.Morph.MorphPerson.Undefined) 
                {
                    if (((vf.Misc.Person & Pullenti.Morph.MorphPerson.Third)) == Pullenti.Morph.MorphPerson.Undefined) 
                    {
                        if (((vf.Misc.Person & Pullenti.Morph.MorphPerson.First)) == Pullenti.Morph.MorphPerson.First) 
                        {
                            if (!morph.ContainsAttr("1 л.", null)) 
                                return Coef = -1;
                        }
                        if (((vf.Misc.Person & Pullenti.Morph.MorphPerson.Second)) == Pullenti.Morph.MorphPerson.Second) 
                        {
                            if (!morph.ContainsAttr("2 л.", null)) 
                                return Coef = -1;
                        }
                    }
                }
                Coef = Pullenti.Semantic.SemanticService.Params.MorphAccord;
                if (morph.Case.IsUndefined) 
                    Coef /= 4;
            }
            return Coef;
        }
        static bool _isRevVerb(Pullenti.Morph.MorphWordForm vf)
        {
            if (vf.Misc.Attrs.Contains("возвр.")) 
                return true;
            if (vf.NormalCase != null) 
            {
                if (vf.NormalCase.EndsWith("СЯ") || vf.NormalCase.EndsWith("СЬ")) 
                    return true;
            }
            return false;
        }
        double _calcPacient(bool noplural)
        {
            if (!string.IsNullOrEmpty(FromPrep)) 
                return Coef = -1;
            Pullenti.Morph.MorphWordForm vf = ToVerb.FirstVerb.VerbMorph;
            if (vf == null) 
                return -1;
            Pullenti.Morph.MorphWordForm vf2 = ToVerb.LastVerb.VerbMorph;
            if (vf2 == null) 
                return -1;
            Pullenti.Ner.MorphCollection morph = FromMorph;
            if (vf2.Misc.Voice == Pullenti.Morph.MorphVoice.Passive || ToVerb.LastVerb.Morph.ContainsAttr("страд.з.", null)) 
            {
                if (vf.Number == Pullenti.Morph.MorphNumber.Plural) 
                {
                    if (noplural) 
                    {
                        if (FromIsPlural) 
                        {
                        }
                        else if (!_checkMorphAccord(morph, false, vf)) 
                            return -1;
                        else if (morph.Items.Count > 0 && !vf.Case.IsUndefined) 
                        {
                            bool ok = false;
                            foreach (Pullenti.Morph.MorphBaseInfo it in morph.Items) 
                            {
                                if (((it.Number & Pullenti.Morph.MorphNumber.Plural)) == Pullenti.Morph.MorphNumber.Plural) 
                                {
                                    if (!it.Case.IsUndefined && ((it.Case & vf.Case)).IsUndefined) 
                                        continue;
                                    ok = true;
                                    break;
                                }
                            }
                            if (!ok) 
                                return Coef = -1;
                        }
                    }
                    Coef = Pullenti.Semantic.SemanticService.Params.VerbPlural;
                    Plural = 1;
                }
                else 
                {
                    if (vf.Number == Pullenti.Morph.MorphNumber.Singular) 
                    {
                        Plural = 0;
                        if (FromIsPlural) 
                            return -1;
                    }
                    if (!_checkMorphAccord(morph, false, vf)) 
                        return -1;
                    Coef = Pullenti.Semantic.SemanticService.Params.MorphAccord;
                }
                return Coef;
            }
            bool isTrans = false;
            bool isRefDative = false;
            List<Pullenti.Semantic.Utils.DerivateGroup> grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(vf2.NormalFull ?? vf2.NormalCase, true, null);
            if (grs != null) 
            {
                foreach (Pullenti.Semantic.Utils.DerivateGroup gr in grs) 
                {
                    if (gr.Cm.Transitive) 
                        isTrans = true;
                    if (gr.CmRev.Agent != null && !gr.CmRev.Agent.Case.IsNominative) 
                        isRefDative = true;
                }
            }
            if (_isRevVerb(vf2)) 
            {
                if (!string.IsNullOrEmpty(FromPrep)) 
                    return -1;
                if (!morph.Case.IsUndefined) 
                {
                    if (isRefDative) 
                    {
                        if (morph.Case.IsNominative) 
                            return Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                    }
                    else if (morph.Case.IsInstrumental) 
                        return Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                    return -1;
                }
                return Coef = 0;
            }
            if (vf2 != vf && !isTrans) 
            {
                grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(vf.NormalFull ?? vf.NormalCase, true, null);
                if (grs != null) 
                {
                    foreach (Pullenti.Semantic.Utils.DerivateGroup gr in grs) 
                    {
                        if (gr.Cm.Transitive) 
                            isTrans = true;
                    }
                }
            }
            if (isTrans) 
            {
                if (!string.IsNullOrEmpty(FromPrep)) 
                    return -1;
                if (!morph.Case.IsUndefined) 
                {
                    if (morph.Case.IsAccusative) 
                    {
                        Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                        if (morph.Case.IsDative) 
                            Coef /= 2;
                        if (morph.Case.IsGenitive) 
                            Coef /= 2;
                        if (morph.Case.IsInstrumental) 
                            Coef /= 2;
                        return Coef;
                    }
                    else 
                        return -1;
                }
            }
            if (vf2.NormalCase == "БЫТЬ") 
            {
                if (!string.IsNullOrEmpty(FromPrep)) 
                    return -1;
                if (morph.Case.IsInstrumental) 
                    return Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                if (morph.Case.IsNominative) 
                {
                    if (From.Source.BeginToken.BeginChar > ToVerb.EndChar) 
                        return Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef;
                    else 
                        return Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef / 2;
                }
                if (morph.Case.IsUndefined) 
                    return Coef = Pullenti.Semantic.SemanticService.Params.TransitiveCoef / 2;
            }
            return -1;
        }
        double _calcActant()
        {
            if (CanBeParticiple) 
                return Coef = -1;
            Pullenti.Morph.MorphWordForm vf2 = ToVerb.LastVerb.VerbMorph;
            if (vf2 == null) 
                return -1;
            if (FromPrep == null) 
                return Coef = 0;
            Pullenti.Ner.MorphCollection fm = From.Source.Source.Morph;
            List<Pullenti.Semantic.Utils.DerivateGroup> grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(vf2.NormalFull ?? vf2.NormalCase, true, null);
            if (grs != null) 
            {
                foreach (Pullenti.Semantic.Utils.DerivateGroup gr in grs) 
                {
                    if (gr.Cm.Nexts == null || !gr.Cm.Nexts.ContainsKey(FromPrep)) 
                        continue;
                    Pullenti.Morph.MorphCase cas = gr.Cm.Nexts[FromPrep];
                    if (!((cas & fm.Case)).IsUndefined) 
                    {
                        Coef = Pullenti.Semantic.SemanticService.Params.NextModel;
                        if (string.IsNullOrEmpty(FromPrep)) 
                        {
                            if (fm.Case.IsNominative) 
                                Coef /= 2;
                            Coef /= 2;
                        }
                        return Coef;
                    }
                    if (From.Source.Source.Morph.Case.IsUndefined) 
                        return Coef = 0;
                }
            }
            return Coef = 0.1;
        }
        static bool _checkMorphAccord(Pullenti.Ner.MorphCollection m, bool plural, Pullenti.Morph.MorphBaseInfo vf)
        {
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