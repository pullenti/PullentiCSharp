/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Measure.Internal
{
    public class MeasureToken : Pullenti.Ner.MetaToken
    {
        public MeasureToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public NumbersWithUnitToken Nums;
        public string Name;
        public List<MeasureToken> Internals = new List<MeasureToken>();
        public MeasureToken InternalEx;
        public bool IsSet;
        /// <summary>
        /// Очень хорошее выделение
        /// </summary>
        public bool Reliable;
        public bool IsEmpty;
        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Nums.ToString());
        }
        public string GetNormValues()
        {
            List<Pullenti.Ner.ReferentToken> li = this.CreateRefenetsTokensWithRegister(null, false);
            if (li == null || (li.Count < 1)) 
                return null;
            Pullenti.Ner.Measure.MeasureReferent mr = li[li.Count - 1].Referent as Pullenti.Ner.Measure.MeasureReferent;
            if (mr == null) 
                return null;
            return mr.ToString(true, null, 0);
        }
        public List<Pullenti.Ner.ReferentToken> CreateRefenetsTokensWithRegister(Pullenti.Ner.Core.AnalyzerData ad, bool register = true)
        {
            if (Internals.Count == 0 && !Reliable) 
            {
                if (Nums.Units.Count == 1 && Nums.Units[0].IsDoubt) 
                {
                    if (Nums.Units[0].UnknownName != null) 
                    {
                    }
                    else if (Nums.IsNewlineBefore) 
                    {
                    }
                    else if (Nums.Units[0].BeginToken.LengthChar > 1 && Nums.Units[0].BeginToken.GetMorphClassInDictionary().IsUndefined) 
                    {
                    }
                    else if (Nums.FromVal == null || Nums.ToVal == null) 
                        return null;
                }
            }
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            if (((Nums == null || Nums.PlusMinusPercent)) && Internals.Count > 0) 
            {
                List<Pullenti.Ner.ReferentToken> liEx = null;
                if (InternalEx != null) 
                {
                    liEx = InternalEx.CreateRefenetsTokensWithRegister(ad, true);
                    if (liEx != null) 
                        res.AddRange(liEx);
                }
                Pullenti.Ner.Measure.MeasureReferent mr = new Pullenti.Ner.Measure.MeasureReferent();
                string templ0 = "1";
                string templ = null;
                if (Name != null) 
                    mr.AddSlot(Pullenti.Ner.Measure.MeasureReferent.ATTR_NAME, Name, false, 0);
                List<Pullenti.Ner.Measure.MeasureReferent> ints = new List<Pullenti.Ner.Measure.MeasureReferent>();
                for (int k = 0; k < Internals.Count; k++) 
                {
                    MeasureToken ii = Internals[k];
                    ii.Reliable = true;
                    List<Pullenti.Ner.ReferentToken> li = ii.CreateRefenetsTokensWithRegister(ad, false);
                    if (li == null) 
                        continue;
                    res.AddRange(li);
                    Pullenti.Ner.Measure.MeasureReferent mr0 = res[res.Count - 1].Referent as Pullenti.Ner.Measure.MeasureReferent;
                    if (liEx != null) 
                        mr0.AddSlot(Pullenti.Ner.Measure.MeasureReferent.ATTR_REF, liEx[liEx.Count - 1], false, 0);
                    if (k == 0 && !IsEmpty) 
                    {
                        templ0 = mr0.Template;
                        mr0.Template = "1";
                    }
                    if (ad != null) 
                        mr0 = ad.RegisterReferent(mr0) as Pullenti.Ner.Measure.MeasureReferent;
                    mr.AddSlot(Pullenti.Ner.Measure.MeasureReferent.ATTR_VALUE, mr0, false, 0);
                    ints.Add(mr0);
                    if (templ == null) 
                        templ = "1";
                    else 
                    {
                        int nu = mr.GetStringValues(Pullenti.Ner.Measure.MeasureReferent.ATTR_VALUE).Count;
                        templ = string.Format("{0}{1}{2}", templ, (IsSet ? ", " : " × "), nu);
                    }
                }
                if (IsSet) 
                    templ = "{" + templ + "}";
                if (templ0 != "1") 
                    templ = templ0.Replace("1", templ);
                if (Nums != null && Nums.PlusMinusPercent && Nums.SingleVal != null) 
                {
                    templ = string.Format("[{0} ±{1}%]", templ, Internals.Count + 1);
                    mr.AddValue(Nums.SingleVal.Value);
                }
                mr.Template = templ;
                int i;
                bool hasLength = false;
                Pullenti.Ner.Measure.UnitReferent uref = null;
                for (i = 0; i < ints.Count; i++) 
                {
                    if (ints[i].Kind == Pullenti.Ner.Measure.MeasureKind.Length) 
                    {
                        hasLength = true;
                        uref = ints[i].GetSlotValue(Pullenti.Ner.Measure.MeasureReferent.ATTR_UNIT) as Pullenti.Ner.Measure.UnitReferent;
                    }
                    else if (ints[i].Units.Count > 0) 
                        break;
                }
                if (ints.Count > 1 && hasLength && uref != null) 
                {
                    foreach (Pullenti.Ner.Measure.MeasureReferent ii in ints) 
                    {
                        if (ii.FindSlot(Pullenti.Ner.Measure.MeasureReferent.ATTR_UNIT, null, true) == null) 
                        {
                            ii.AddSlot(Pullenti.Ner.Measure.MeasureReferent.ATTR_UNIT, uref, false, 0);
                            ii.Kind = Pullenti.Ner.Measure.MeasureKind.Length;
                        }
                    }
                }
                if (ints.Count == 3) 
                {
                    if (ints[0].Kind == Pullenti.Ner.Measure.MeasureKind.Length && ints[1].Kind == Pullenti.Ner.Measure.MeasureKind.Length && ints[2].Kind == Pullenti.Ner.Measure.MeasureKind.Length) 
                        mr.Kind = Pullenti.Ner.Measure.MeasureKind.Volume;
                    else if (ints[0].Units.Count == 0 && ints[1].Units.Count == 0 && ints[2].Units.Count == 0) 
                    {
                        string nam = mr.GetStringValue(Pullenti.Ner.Measure.MeasureReferent.ATTR_NAME);
                        if (nam != null) 
                        {
                            if (nam.Contains("РАЗМЕР") || nam.Contains("ГАБАРИТ")) 
                                mr.Kind = Pullenti.Ner.Measure.MeasureKind.Volume;
                        }
                    }
                }
                if (ints.Count == 2) 
                {
                    if (ints[0].Kind == Pullenti.Ner.Measure.MeasureKind.Length && ints[1].Kind == Pullenti.Ner.Measure.MeasureKind.Length) 
                        mr.Kind = Pullenti.Ner.Measure.MeasureKind.Area;
                }
                if (!IsEmpty) 
                {
                    if (ad != null) 
                        mr = ad.RegisterReferent(mr) as Pullenti.Ner.Measure.MeasureReferent;
                    res.Add(new Pullenti.Ner.ReferentToken(mr, BeginToken, EndToken));
                }
                return res;
            }
            List<Pullenti.Ner.ReferentToken> re2 = Nums.CreateRefenetsTokensWithRegister(ad, Name, register);
            foreach (MeasureToken ii in Internals) 
            {
                List<Pullenti.Ner.ReferentToken> li = ii.CreateRefenetsTokensWithRegister(ad, true);
                if (li == null) 
                    continue;
                res.AddRange(li);
                re2[re2.Count - 1].Referent.AddSlot(Pullenti.Ner.Measure.MeasureReferent.ATTR_REF, res[res.Count - 1].Referent, false, 0);
            }
            re2[re2.Count - 1].BeginToken = BeginToken;
            re2[re2.Count - 1].EndToken = EndToken;
            res.AddRange(re2);
            return res;
        }
        public static MeasureToken TryParseMinimal(Pullenti.Ner.Token t, Pullenti.Ner.Core.TerminCollection addUnits, bool canOmitNumber = false)
        {
            if (t == null || (t is Pullenti.Ner.ReferentToken)) 
                return null;
            List<NumbersWithUnitToken> mt = NumbersWithUnitToken.TryParseMulti(t, addUnits, canOmitNumber, false, false, false);
            if (mt == null) 
                return null;
            if (mt[0].Units.Count == 0) 
                return null;
            if ((mt.Count == 1 && mt[0].Units.Count == 1 && mt[0].Units[0].IsDoubt) && !mt[0].IsNewlineBefore) 
                return null;
            MeasureToken res;
            if (mt.Count == 1) 
            {
                res = new MeasureToken(mt[0].BeginToken, mt[mt.Count - 1].EndToken) { Nums = mt[0] };
                res._parseInternals(addUnits);
                return res;
            }
            res = new MeasureToken(mt[0].BeginToken, mt[mt.Count - 1].EndToken);
            foreach (NumbersWithUnitToken m in mt) 
            {
                res.Internals.Add(new MeasureToken(m.BeginToken, m.EndToken) { Nums = m });
            }
            return res;
        }
        void _parseInternals(Pullenti.Ner.Core.TerminCollection addUnits)
        {
            if (EndToken.Next != null && ((EndToken.Next.IsCharOf("\\/") || EndToken.Next.IsValue("ПРИ", null)))) 
            {
                MeasureToken mt1 = TryParse(EndToken.Next.Next, addUnits, true, false, false, false);
                if (mt1 != null) 
                {
                    Internals.Add(mt1);
                    EndToken = mt1.EndToken;
                }
                else 
                {
                    NumbersWithUnitToken mt = NumbersWithUnitToken.TryParse(EndToken.Next.Next, addUnits, false, false, false, false);
                    if (mt != null && mt.Units.Count > 0 && !UnitToken.CanBeEquals(Nums.Units, mt.Units)) 
                    {
                        Internals.Add(new MeasureToken(mt.BeginToken, mt.EndToken) { Nums = mt });
                        EndToken = mt.EndToken;
                    }
                }
            }
        }
        /// <summary>
        /// Выделение вместе с наименованием
        /// </summary>
        public static MeasureToken TryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.TerminCollection addUnits, bool canBeSet = true, bool canUnitsAbsent = false, bool isResctriction = false, bool isSubval = false)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            if (t.IsTableControlChar) 
                return null;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.MetaToken whd = null;
            int minmax = 0;
            Pullenti.Ner.Token tt = NumbersWithUnitToken._isMinOrMax(t0, ref minmax);
            if (tt != null) 
                t = tt.Next;
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition | Pullenti.Ner.Core.NounPhraseParseAttr.IgnoreBrackets, 0, null);
            if (npt == null) 
            {
                whd = NumbersWithUnitToken._tryParseWHL(t);
                if (whd != null) 
                    npt = new Pullenti.Ner.Core.NounPhraseToken(t0, whd.EndToken);
                else if (t0.IsValue("КПД", null)) 
                    npt = new Pullenti.Ner.Core.NounPhraseToken(t0, t0);
                else if ((t0 is Pullenti.Ner.TextToken) && t0.LengthChar > 3 && t0.GetMorphClassInDictionary().IsUndefined) 
                    npt = new Pullenti.Ner.Core.NounPhraseToken(t0, t0);
                else if (t0.IsValue("T", null) && t0.Chars.IsAllLower) 
                {
                    npt = new Pullenti.Ner.Core.NounPhraseToken(t0, t0);
                    t = t0;
                    if (t.Next != null && t.Next.IsChar('=')) 
                        npt.EndToken = t.Next;
                }
                else if ((t0 is Pullenti.Ner.TextToken) && t0.Chars.IsLetter && isSubval) 
                {
                    if (NumbersWithUnitToken.TryParse(t, addUnits, false, false, false, false) != null) 
                        return null;
                    npt = new Pullenti.Ner.Core.NounPhraseToken(t0, t0);
                    for (t = t0.Next; t != null; t = t.Next) 
                    {
                        if (t.WhitespacesBeforeCount > 2) 
                            break;
                        else if (!(t is Pullenti.Ner.TextToken)) 
                            break;
                        else if (!t.Chars.IsLetter) 
                        {
                            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br != null) 
                                npt.EndToken = (t = br.EndToken);
                            else 
                                break;
                        }
                        else if (NumbersWithUnitToken.TryParse(t, addUnits, false, false, false, false) != null) 
                            break;
                        else 
                            npt.EndToken = t;
                    }
                }
                else 
                    return null;
            }
            else if (Pullenti.Ner.Core.NumberHelper.TryParseRealNumber(t, true, false) != null) 
                return null;
            else 
            {
                Pullenti.Ner.Date.Internal.DateItemToken dtok = Pullenti.Ner.Date.Internal.DateItemToken.TryAttach(t, null, false);
                if (dtok != null) 
                    return null;
            }
            Pullenti.Ner.Token t1 = npt.EndToken;
            t = npt.EndToken;
            Pullenti.Ner.MetaToken name = new Pullenti.Ner.MetaToken(npt.BeginToken, npt.EndToken) { Morph = npt.Morph };
            List<UnitToken> units = null;
            List<UnitToken> units2 = null;
            List<MeasureToken> internals = new List<MeasureToken>();
            bool not = false;
            for (tt = t1.Next; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore) 
                    break;
                if (tt.IsTableControlChar) 
                    break;
                Pullenti.Ner.Token tt2 = NumbersWithUnitToken._isMinOrMax(tt, ref minmax);
                if (tt2 != null) 
                {
                    t1 = (t = (tt = tt2));
                    continue;
                }
                if ((tt.IsValue("БЫТЬ", null) || tt.IsValue("ДОЛЖЕН", null) || tt.IsValue("ДОЛЖНЫЙ", null)) || tt.IsValue("МОЖЕТ", null) || ((tt.IsValue("СОСТАВЛЯТЬ", null) && !tt.GetMorphClassInDictionary().IsAdjective))) 
                {
                    t1 = (t = tt);
                    if (tt.Previous.IsValue("НЕ", null)) 
                        not = true;
                    continue;
                }
                Pullenti.Ner.MetaToken www = NumbersWithUnitToken._tryParseWHL(tt);
                if (www != null) 
                {
                    whd = www;
                    t1 = (t = (tt = www.EndToken));
                    continue;
                }
                if (tt.IsValue("ПРИ", null)) 
                {
                    MeasureToken mt1 = TryParse(tt.Next, addUnits, false, false, true, false);
                    if (mt1 != null) 
                    {
                        internals.Add(mt1);
                        t1 = (t = (tt = mt1.EndToken));
                        continue;
                    }
                    NumbersWithUnitToken n1 = NumbersWithUnitToken.TryParse(tt.Next, addUnits, false, false, false, false);
                    if (n1 != null && n1.Units.Count > 0) 
                    {
                        mt1 = new MeasureToken(n1.BeginToken, n1.EndToken) { Nums = n1 };
                        internals.Add(mt1);
                        t1 = (t = (tt = mt1.EndToken));
                        continue;
                    }
                }
                if (tt.IsValue("ПО", null) && tt.Next != null && tt.Next.IsValue("U", null)) 
                {
                    t1 = (t = (tt = tt.Next));
                    continue;
                }
                if (internals.Count > 0) 
                {
                    if (tt.IsChar(':')) 
                        break;
                    MeasureToken mt1 = TryParse(tt.Next, addUnits, false, false, true, false);
                    if (mt1 != null && mt1.Reliable) 
                    {
                        internals.Add(mt1);
                        t1 = (t = (tt = mt1.EndToken));
                        continue;
                    }
                }
                if ((tt is Pullenti.Ner.NumberToken) && (tt as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Words) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt3 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective, 0, null);
                    if (npt3 != null) 
                    {
                        t1 = (tt = npt3.EndToken);
                        if (internals.Count == 0) 
                            name.EndToken = t1;
                        continue;
                    }
                }
                if (((tt.IsHiphen && !tt.IsWhitespaceBefore && !tt.IsWhitespaceAfter) && (tt.Next is Pullenti.Ner.NumberToken) && (tt.Previous is Pullenti.Ner.TextToken)) && tt.Previous.Chars.IsAllUpper) 
                {
                    t1 = (tt = (t = tt.Next));
                    if (internals.Count == 0) 
                        name.EndToken = t1;
                    continue;
                }
                if (((tt is Pullenti.Ner.NumberToken) && !tt.IsWhitespaceBefore && (tt.Previous is Pullenti.Ner.TextToken)) && tt.Previous.Chars.IsAllUpper) 
                {
                    t1 = (t = tt);
                    if (internals.Count == 0) 
                        name.EndToken = t1;
                    continue;
                }
                if ((((tt is Pullenti.Ner.NumberToken) && !tt.IsWhitespaceAfter && tt.Next.IsHiphen) && !tt.Next.IsWhitespaceAfter && (tt.Next.Next is Pullenti.Ner.TextToken)) && tt.Next.Next.LengthChar > 2) 
                {
                    t1 = (t = (tt = tt.Next.Next));
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt1 != null && npt1.EndChar > tt.EndChar) 
                        t1 = (t = (tt = npt1.EndToken));
                    if (internals.Count == 0) 
                        name.EndToken = t1;
                    continue;
                }
                if ((tt is Pullenti.Ner.NumberToken) && tt.Previous != null) 
                {
                    if (tt.Previous.IsValue("USB", null)) 
                    {
                        t1 = (t = tt);
                        if (internals.Count == 0) 
                            name.EndToken = t1;
                        for (Pullenti.Ner.Token ttt = tt.Next; ttt != null; ttt = ttt.Next) 
                        {
                            if (ttt.IsWhitespaceBefore) 
                                break;
                            if (ttt.IsCharOf(",:")) 
                                break;
                            t1 = (t = (tt = ttt));
                            if (internals.Count == 0) 
                                name.EndToken = t1;
                        }
                        continue;
                    }
                }
                NumbersWithUnitToken mt0 = NumbersWithUnitToken.TryParse(tt, addUnits, false, false, false, false);
                if (mt0 != null) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                    if (npt1 != null && npt1.EndChar > mt0.EndChar) 
                    {
                        t1 = (t = (tt = npt1.EndToken));
                        if (internals.Count == 0) 
                            name.EndToken = t1;
                        continue;
                    }
                    break;
                }
                if (((tt.IsComma || tt.IsChar('('))) && tt.Next != null) 
                {
                    www = NumbersWithUnitToken._tryParseWHL(tt.Next);
                    if (www != null) 
                    {
                        whd = www;
                        t1 = (t = (tt = www.EndToken));
                        if (tt.Next != null && tt.Next.IsComma) 
                            t1 = (tt = tt.Next);
                        if (tt.Next != null && tt.Next.IsChar(')')) 
                        {
                            t1 = (tt = tt.Next);
                            continue;
                        }
                    }
                    List<UnitToken> uu = UnitToken.TryParseList(tt.Next, addUnits, false);
                    if (uu != null) 
                    {
                        t1 = (t = uu[uu.Count - 1].EndToken);
                        units = uu;
                        if (tt.IsChar('(') && t1.Next != null && t1.Next.IsChar(')')) 
                        {
                            t1 = (t = (tt = t1.Next));
                            continue;
                        }
                        else if (t1.Next != null && t1.Next.IsChar('(')) 
                        {
                            uu = UnitToken.TryParseList(t1.Next.Next, addUnits, false);
                            if (uu != null && uu[uu.Count - 1].EndToken.Next != null && uu[uu.Count - 1].EndToken.Next.IsChar(')')) 
                            {
                                units2 = uu;
                                t1 = (t = (tt = uu[uu.Count - 1].EndToken.Next));
                                continue;
                            }
                            www = NumbersWithUnitToken._tryParseWHL(t1.Next);
                            if (www != null) 
                            {
                                whd = www;
                                t1 = (t = (tt = www.EndToken));
                                continue;
                            }
                        }
                        if (uu != null && uu.Count > 0 && !uu[0].IsDoubt) 
                            break;
                        if (t1.Next != null) 
                        {
                            if (t1.Next.IsTableControlChar || t1.IsNewlineAfter) 
                                break;
                        }
                        units = null;
                    }
                }
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false) && !(tt.Next is Pullenti.Ner.NumberToken)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        t1 = (t = (tt = br.EndToken));
                        continue;
                    }
                }
                if (tt.IsValue("НЕ", null) && tt.Next != null) 
                {
                    Pullenti.Morph.MorphClass mc = tt.Next.GetMorphClassInDictionary();
                    if (mc.IsAdverb || mc.IsMisc) 
                        break;
                    continue;
                }
                if (tt.IsValue("ЯМЗ", null)) 
                {
                }
                Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition | Pullenti.Ner.Core.NounPhraseParseAttr.IgnoreBrackets | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns, 0, null);
                if (npt2 == null) 
                {
                    if (tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction) 
                    {
                        Pullenti.Ner.Core.TerminToken to = NumbersWithUnitToken.m_Termins.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (to != null) 
                        {
                            if ((to.EndToken.Next is Pullenti.Ner.TextToken) && to.EndToken.Next.IsLetters) 
                            {
                            }
                            else 
                                break;
                        }
                        t1 = tt;
                        continue;
                    }
                    Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                    if (((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter && tt.LengthChar > 1) && (((tt.Chars.IsAllUpper || mc.IsAdverb || mc.IsUndefined) || mc.IsAdjective))) 
                    {
                        List<UnitToken> uu = UnitToken.TryParseList(tt, addUnits, false);
                        if (uu != null) 
                        {
                            if (uu[0].LengthChar > 1 || uu.Count > 1) 
                            {
                                units = uu;
                                t1 = (t = uu[uu.Count - 1].EndToken);
                                break;
                            }
                        }
                        t1 = (t = tt);
                        if (internals.Count == 0) 
                            name.EndToken = tt;
                        continue;
                    }
                    if (tt.IsComma) 
                        continue;
                    if (tt.IsChar('.')) 
                    {
                        if (!Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt.Next)) 
                            continue;
                        List<UnitToken> uu = UnitToken.TryParseList(tt.Next, addUnits, false);
                        if (uu != null) 
                        {
                            if (uu[0].LengthChar > 2 || uu.Count > 1) 
                            {
                                units = uu;
                                t1 = (t = uu[uu.Count - 1].EndToken);
                                break;
                            }
                        }
                    }
                    break;
                }
                t1 = (t = (tt = npt2.EndToken));
                if (internals.Count > 0) 
                {
                }
                else if (t.IsValue("ПРЕДЕЛ", null) || t.IsValue("ГРАНИЦА", null) || t.IsValue("ДИАПАЗОН", null)) 
                {
                }
                else if (t.Chars.IsLetter) 
                    name.EndToken = t1;
            }
            Pullenti.Ner.Token t11 = t1;
            for (t1 = t1.Next; t1 != null; t1 = t1.Next) 
            {
                if (t1.IsTableControlChar) 
                {
                }
                else if (t1.IsCharOf(":,_")) 
                {
                    if (isResctriction) 
                        return null;
                    Pullenti.Ner.MetaToken www = NumbersWithUnitToken._tryParseWHL(t1.Next);
                    if (www != null) 
                    {
                        whd = www;
                        t1 = (t = www.EndToken);
                        continue;
                    }
                    List<UnitToken> uu = UnitToken.TryParseList(t1.Next, addUnits, false);
                    if (uu != null) 
                    {
                        if (uu[0].LengthChar > 1 || uu.Count > 1) 
                        {
                            units = uu;
                            t1 = (t = uu[uu.Count - 1].EndToken);
                            continue;
                        }
                    }
                    if (t1.IsChar(':')) 
                    {
                        List<MeasureToken> li = new List<MeasureToken>();
                        for (Pullenti.Ner.Token ttt = t1.Next; ttt != null; ttt = ttt.Next) 
                        {
                            if (ttt.IsHiphen || ttt.IsTableControlChar) 
                                continue;
                            if ((ttt is Pullenti.Ner.TextToken) && !ttt.Chars.IsLetter) 
                                continue;
                            MeasureToken mt1 = TryParse(ttt, addUnits, true, true, false, true);
                            if (mt1 == null) 
                                break;
                            li.Add(mt1);
                            ttt = mt1.EndToken;
                            if (ttt.Next != null && ttt.Next.IsChar(';')) 
                                ttt = ttt.Next;
                            if (ttt.IsChar(';')) 
                            {
                            }
                            else if (ttt.IsNewlineAfter && mt1.IsNewlineBefore) 
                            {
                            }
                            else 
                                break;
                        }
                        if (li.Count > 1) 
                        {
                            MeasureToken res0 = new MeasureToken(t0, li[li.Count - 1].EndToken) { Internals = li, IsEmpty = true };
                            if (internals != null && internals.Count > 0) 
                                res0.InternalEx = internals[0];
                            string nam = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(name, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                            li[0].BeginToken = t0;
                            foreach (MeasureToken v in li) 
                            {
                                v.Name = string.Format("{0} ({1})", nam, v.Name ?? "").Trim();
                                if (v.Nums != null && v.Nums.Units.Count == 0 && units != null) 
                                    v.Nums.Units = units;
                            }
                            return res0;
                        }
                    }
                }
                else if (t1.IsHiphen && t1.IsWhitespaceAfter && t1.IsWhitespaceBefore) 
                {
                }
                else if (t1.IsHiphen && t1.Next != null && t1.Next.IsChar('(')) 
                {
                }
                else 
                    break;
            }
            if (t1 == null) 
                return null;
            List<NumbersWithUnitToken> mts = NumbersWithUnitToken.TryParseMulti(t1, addUnits, false, not, true, isResctriction);
            if (mts == null) 
            {
                if (units != null && units.Count > 0) 
                {
                    if (t1 == null || t1.Previous.IsChar(':')) 
                    {
                        mts = new List<NumbersWithUnitToken>();
                        if (t1 == null) 
                        {
                            for (t1 = t11; t1 != null && t1.Next != null; t1 = t1.Next) 
                            {
                            }
                        }
                        else 
                            t1 = t1.Previous;
                        mts.Add(new NumbersWithUnitToken(t0, t1) { SingleVal = double.NaN });
                    }
                }
                if (mts == null) 
                    return null;
            }
            NumbersWithUnitToken mt = mts[0];
            if (mt.BeginToken == mt.EndToken && !(mt.BeginToken is Pullenti.Ner.NumberToken)) 
                return null;
            if (!isSubval && name.BeginToken.Morph.Class.IsPreposition) 
                name.BeginToken = name.BeginToken.Next;
            if (mt.WHL != null) 
                whd = mt.WHL;
            for (int kk = 0; kk < 10; kk++) 
            {
                if (whd != null && whd.EndToken == name.EndToken) 
                {
                    name.EndToken = whd.BeginToken.Previous;
                    continue;
                }
                if (units != null) 
                {
                    if (units[units.Count - 1].EndToken == name.EndToken) 
                    {
                        name.EndToken = units[0].BeginToken.Previous;
                        continue;
                    }
                }
                break;
            }
            if (mts.Count > 1 && internals.Count == 0) 
            {
                if (mt.Units.Count == 0) 
                {
                    if (units != null) 
                    {
                        foreach (NumbersWithUnitToken m in mts) 
                        {
                            m.Units = units;
                        }
                    }
                }
                MeasureToken res1 = new MeasureToken(t0, mts[mts.Count - 1].EndToken) { Morph = name.Morph, Reliable = true };
                res1.Name = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(name, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                for (int k = 0; k < mts.Count; k++) 
                {
                    MeasureToken ttt = new MeasureToken(mts[k].BeginToken, mts[k].EndToken) { Nums = mts[k] };
                    if (whd != null) 
                    {
                        List<string> nams = whd.Tag as List<string>;
                        if (k < nams.Count) 
                            ttt.Name = nams[k];
                    }
                    res1.Internals.Add(ttt);
                }
                Pullenti.Ner.Token tt1 = res1.EndToken.Next;
                if (tt1 != null && tt1.IsChar('±')) 
                {
                    NumbersWithUnitToken nn = NumbersWithUnitToken._tryParse(tt1, addUnits, true, false, false);
                    if (nn != null && nn.PlusMinusPercent) 
                    {
                        res1.EndToken = nn.EndToken;
                        res1.Nums = nn;
                        if (nn.Units.Count > 0 && units == null && mt.Units.Count == 0) 
                        {
                            foreach (NumbersWithUnitToken m in mts) 
                            {
                                m.Units = nn.Units;
                            }
                        }
                    }
                }
                return res1;
            }
            if (!mt.IsWhitespaceBefore) 
            {
                if (mt.BeginToken.Previous == null) 
                    return null;
                if (mt.BeginToken.Previous.IsCharOf(":),") || mt.BeginToken.Previous.IsTableControlChar || mt.BeginToken.Previous.IsValue("IP", null)) 
                {
                }
                else if (mt.BeginToken.IsHiphen && mt.Units.Count > 0 && !mt.Units[0].IsDoubt) 
                {
                }
                else 
                    return null;
            }
            if (mt.Units.Count == 0 && units != null) 
            {
                mt.Units = units;
                if (mt.DivNum != null && units.Count > 1 && mt.DivNum.Units.Count == 0) 
                {
                    for (int i = 1; i < units.Count; i++) 
                    {
                        if (units[i].Pow == -1) 
                        {
                            for (int j = i; j < units.Count; j++) 
                            {
                                mt.DivNum.Units.Add(units[j]);
                                units[j].Pow = -units[j].Pow;
                            }
                            mt.Units.RemoveRange(i, units.Count - i);
                            break;
                        }
                    }
                }
            }
            if ((minmax < 0) && mt.SingleVal != null) 
            {
                mt.FromVal = mt.SingleVal;
                mt.FromInclude = true;
                mt.SingleVal = null;
            }
            if (minmax > 0 && mt.SingleVal != null) 
            {
                mt.ToVal = mt.SingleVal;
                mt.ToInclude = true;
                mt.SingleVal = null;
            }
            if (mt.Units.Count == 0) 
            {
                units = UnitToken.TryParseList(mt.EndToken.Next, addUnits, true);
                if (units == null) 
                {
                    if (canUnitsAbsent) 
                    {
                    }
                    else 
                        return null;
                }
                else 
                    mt.Units = units;
            }
            MeasureToken res = new MeasureToken(t0, mt.EndToken) { Morph = name.Morph, Internals = internals };
            if (((!t0.IsWhitespaceBefore && t0.Previous != null && t0 == name.BeginToken) && t0.Previous.IsHiphen && !t0.Previous.IsWhitespaceBefore) && (t0.Previous.Previous is Pullenti.Ner.TextToken)) 
                name.BeginToken = (res.BeginToken = name.BeginToken.Previous.Previous);
            res.Name = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(name, (!isSubval ? Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative : Pullenti.Ner.Core.GetTextAttr.No));
            res.Nums = mt;
            foreach (UnitToken u in res.Nums.Units) 
            {
                if (u.Keyword != null) 
                {
                    if (u.Keyword.BeginChar >= res.BeginChar) 
                        res.Reliable = true;
                }
            }
            res._parseInternals(addUnits);
            if (res.Internals.Count > 0 || !canBeSet) 
                return res;
            t1 = res.EndToken.Next;
            if (t1 != null && t1.IsCommaAnd) 
                t1 = t1.Next;
            List<NumbersWithUnitToken> mts1 = NumbersWithUnitToken.TryParseMulti(t1, addUnits, false, false, false, false);
            if ((mts1 != null && mts1.Count == 1 && (t1.WhitespacesBeforeCount < 3)) && mts1[0].Units.Count > 0 && !UnitToken.CanBeEquals(mts[0].Units, mts1[0].Units)) 
            {
                res.IsSet = true;
                res.Nums = null;
                res.Internals.Add(new MeasureToken(mt.BeginToken, mt.EndToken) { Nums = mt });
                res.Internals.Add(new MeasureToken(mts1[0].BeginToken, mts1[0].EndToken) { Nums = mts1[0] });
                res.EndToken = mts1[0].EndToken;
            }
            return res;
        }
    }
}