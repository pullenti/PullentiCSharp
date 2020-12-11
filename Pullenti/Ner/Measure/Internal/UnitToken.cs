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

namespace Pullenti.Ner.Measure.Internal
{
    public class UnitToken : Pullenti.Ner.MetaToken
    {
        public UnitToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public Unit Unit;
        public int Pow = 1;
        public bool IsDoubt = false;
        public Pullenti.Ner.Token Keyword;
        public Pullenti.Ner.Measure.UnitReferent ExtOnto;
        public string UnknownName;
        public override string ToString()
        {
            string res = UnknownName ?? ((ExtOnto == null ? Unit.ToString() : ExtOnto.ToString()));
            if (Pow != 1) 
                res = string.Format("{0}<{1}>", res, Pow);
            if (IsDoubt) 
                res += "?";
            if (Keyword != null) 
                res = string.Format("{0} (<-{1})", res, Keyword.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
            return res;
        }
        public static bool CanBeEquals(List<UnitToken> ut1, List<UnitToken> ut2)
        {
            if (ut1.Count != ut2.Count) 
                return false;
            for (int i = 0; i < ut1.Count; i++) 
            {
                if (ut1[i].Unit != ut2[i].Unit || ut1[i].ExtOnto != ut2[i].ExtOnto) 
                    return false;
                if (ut1[i].Pow != ut2[i].Pow) 
                    return false;
            }
            return true;
        }
        public static Pullenti.Ner.Measure.MeasureKind CalcKind(List<UnitToken> units)
        {
            if (units == null || units.Count == 0) 
                return Pullenti.Ner.Measure.MeasureKind.Undefined;
            UnitToken u0 = units[0];
            if (u0.Unit == null) 
                return Pullenti.Ner.Measure.MeasureKind.Undefined;
            if (units.Count == 1) 
            {
                if (u0.Pow == 1) 
                    return u0.Unit.Kind;
                if (u0.Pow == 2) 
                {
                    if (u0.Unit.Kind == Pullenti.Ner.Measure.MeasureKind.Length) 
                        return Pullenti.Ner.Measure.MeasureKind.Area;
                }
                if (u0.Pow == 3) 
                {
                    if (u0.Unit.Kind == Pullenti.Ner.Measure.MeasureKind.Length) 
                        return Pullenti.Ner.Measure.MeasureKind.Volume;
                }
                return Pullenti.Ner.Measure.MeasureKind.Undefined;
            }
            if (units.Count == 2) 
            {
                if (units[1].Unit == null) 
                    return Pullenti.Ner.Measure.MeasureKind.Undefined;
                if ((u0.Unit.Kind == Pullenti.Ner.Measure.MeasureKind.Length && u0.Pow == 1 && units[1].Unit.Kind == Pullenti.Ner.Measure.MeasureKind.Time) && units[1].Pow == -1) 
                    return Pullenti.Ner.Measure.MeasureKind.Speed;
            }
            return Pullenti.Ner.Measure.MeasureKind.Undefined;
        }
        static Pullenti.Ner.Measure.UnitReferent _createReferent(Unit u)
        {
            Pullenti.Ner.Measure.UnitReferent ur = new Pullenti.Ner.Measure.UnitReferent();
            ur.AddSlot(Pullenti.Ner.Measure.UnitReferent.ATTR_NAME, u.NameCyr, false, 0);
            ur.AddSlot(Pullenti.Ner.Measure.UnitReferent.ATTR_NAME, u.NameLat, false, 0);
            ur.AddSlot(Pullenti.Ner.Measure.UnitReferent.ATTR_FULLNAME, u.FullnameCyr, false, 0);
            ur.AddSlot(Pullenti.Ner.Measure.UnitReferent.ATTR_FULLNAME, u.FullnameLat, false, 0);
            ur.Tag = u;
            ur.m_Unit = u;
            return ur;
        }
        public Pullenti.Ner.Measure.UnitReferent CreateReferentWithRegister(Pullenti.Ner.Core.AnalyzerData ad)
        {
            Pullenti.Ner.Measure.UnitReferent ur = ExtOnto;
            if (Unit != null) 
                ur = _createReferent(Unit);
            else if (UnknownName != null) 
            {
                ur = new Pullenti.Ner.Measure.UnitReferent();
                ur.AddSlot(Pullenti.Ner.Measure.UnitReferent.ATTR_NAME, UnknownName, false, 0);
                ur.IsUnknown = true;
            }
            if (Pow != 1) 
                ur.AddSlot(Pullenti.Ner.Measure.UnitReferent.ATTR_POW, Pow.ToString(), false, 0);
            List<Pullenti.Ner.Measure.UnitReferent> owns = new List<Pullenti.Ner.Measure.UnitReferent>();
            owns.Add(ur);
            if (Unit != null) 
            {
                for (Unit uu = Unit.BaseUnit; uu != null; uu = uu.BaseUnit) 
                {
                    Pullenti.Ner.Measure.UnitReferent ur0 = _createReferent(uu);
                    owns.Add(ur0);
                }
            }
            for (int i = owns.Count - 1; i >= 0; i--) 
            {
                if (ad != null) 
                    owns[i] = ad.RegisterReferent(owns[i]) as Pullenti.Ner.Measure.UnitReferent;
                if (i > 0) 
                {
                    owns[i - 1].AddSlot(Pullenti.Ner.Measure.UnitReferent.ATTR_BASEUNIT, owns[i], false, 0);
                    if ((owns[i - 1].Tag as Unit).BaseMultiplier != 0) 
                        owns[i - 1].AddSlot(Pullenti.Ner.Measure.UnitReferent.ATTR_BASEFACTOR, Pullenti.Ner.Core.NumberHelper.DoubleToString((owns[i - 1].Tag as Unit).BaseMultiplier), false, 0);
                }
            }
            return owns[0];
        }
        public static List<UnitToken> TryParseList(Pullenti.Ner.Token t, Pullenti.Ner.Core.TerminCollection addUnits, bool parseUnknownUnits = false)
        {
            UnitToken ut = TryParse(t, addUnits, null, parseUnknownUnits);
            if (ut == null) 
                return null;
            List<UnitToken> res = new List<UnitToken>();
            res.Add(ut);
            for (Pullenti.Ner.Token tt = ut.EndToken.Next; tt != null; tt = tt.Next) 
            {
                ut = TryParse(tt, addUnits, res[res.Count - 1], true);
                if (ut == null) 
                    break;
                if (ut.Unit != null && ut.Unit.Kind != Pullenti.Ner.Measure.MeasureKind.Undefined) 
                {
                    if (res[res.Count - 1].Unit != null && res[res.Count - 1].Unit.Kind == ut.Unit.Kind) 
                        break;
                }
                res.Add(ut);
                tt = ut.EndToken;
                if (res.Count > 2) 
                    break;
            }
            for (int i = 0; i < res.Count; i++) 
            {
                if (res[i].Unit != null && res[i].Unit.BaseUnit != null && res[i].Unit.MultUnit != null) 
                {
                    UnitToken ut2 = new UnitToken(res[i].BeginToken, res[i].EndToken);
                    ut2.Unit = res[i].Unit.MultUnit;
                    res.Insert(i + 1, ut2);
                    res[i].Unit = res[i].Unit.BaseUnit;
                }
            }
            if (res.Count > 1) 
            {
                foreach (UnitToken r in res) 
                {
                    r.IsDoubt = false;
                }
            }
            return res;
        }
        public static UnitToken TryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.TerminCollection addUnits, UnitToken prev, bool parseUnknownUnits = false)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            int pow = 1;
            bool isNeg = false;
            if ((t.IsCharOf("\\/") || t.IsValue("НА", null) || t.IsValue("OF", null)) || t.IsValue("PER", null)) 
            {
                isNeg = true;
                t = t.Next;
            }
            else if (t.IsValue("В", null) && prev != null) 
            {
                isNeg = true;
                t = t.Next;
            }
            else if (MeasureHelper.IsMultChar(t)) 
                t = t.Next;
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return null;
            if (tt.Term == "КВ" || tt.Term == "КВАДР" || tt.IsValue("КВАДРАТНЫЙ", null)) 
            {
                pow = 2;
                tt = tt.Next as Pullenti.Ner.TextToken;
                if (tt != null && tt.IsChar('.')) 
                    tt = tt.Next as Pullenti.Ner.TextToken;
                if (tt == null) 
                    return null;
            }
            else if (tt.Term == "КУБ" || tt.Term == "КУБИЧ" || tt.IsValue("КУБИЧЕСКИЙ", null)) 
            {
                pow = 3;
                tt = tt.Next as Pullenti.Ner.TextToken;
                if (tt != null && tt.IsChar('.')) 
                    tt = tt.Next as Pullenti.Ner.TextToken;
                if (tt == null) 
                    return null;
            }
            else if (tt.Term == "µ") 
            {
                UnitToken res = TryParse(tt.Next, addUnits, prev, false);
                if (res != null) 
                {
                    foreach (Unit u in UnitsHelper.Units) 
                    {
                        if (u.Factor == UnitsFactors.Micro && string.Compare("мк" + u.NameCyr, res.Unit.NameCyr, true) == 0) 
                        {
                            res.Unit = u;
                            res.BeginToken = tt;
                            res.Pow = pow;
                            if (isNeg) 
                                res.Pow = -pow;
                            return res;
                        }
                    }
                }
            }
            List<Pullenti.Ner.Core.TerminToken> toks = UnitsHelper.Termins.TryParseAll(tt, Pullenti.Ner.Core.TerminParseAttr.No);
            if (toks != null) 
            {
                if ((prev != null && tt == t0 && toks.Count == 1) && t.IsWhitespaceBefore) 
                    return null;
                if (toks[0].BeginToken == toks[0].EndToken && tt.Morph.Class.IsPreposition && (tt.WhitespacesAfterCount < 3)) 
                {
                    if (Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null) != null) 
                        return null;
                    if (tt.Next is Pullenti.Ner.NumberToken) 
                    {
                        if ((tt.Next as Pullenti.Ner.NumberToken).Typ != Pullenti.Ner.NumberSpellingType.Digit) 
                            return null;
                    }
                    UnitToken nex = TryParse(tt.Next, addUnits, null, false);
                    if (nex != null) 
                        return null;
                }
                if (toks[0].BeginToken == toks[0].EndToken && ((toks[0].BeginToken.IsValue("М", null) || toks[0].BeginToken.IsValue("M", null))) && toks[0].BeginToken.Chars.IsAllLower) 
                {
                    if (prev != null && prev.Unit != null && prev.Unit.Kind == Pullenti.Ner.Measure.MeasureKind.Length) 
                    {
                        UnitToken res = new UnitToken(t0, toks[0].EndToken) { Unit = UnitsHelper.uMinute };
                        res.Pow = pow;
                        if (isNeg) 
                            res.Pow = -pow;
                        return res;
                    }
                }
                List<UnitToken> uts = new List<UnitToken>();
                foreach (Pullenti.Ner.Core.TerminToken tok in toks) 
                {
                    UnitToken res = new UnitToken(t0, tok.EndToken) { Unit = tok.Termin.Tag as Unit };
                    res.Pow = pow;
                    if (isNeg) 
                        res.Pow = -pow;
                    if (res.Unit.BaseMultiplier == 1000000 && (t0 is Pullenti.Ner.TextToken) && char.IsLower((t0 as Pullenti.Ner.TextToken).GetSourceText()[0])) 
                    {
                        foreach (Unit u in UnitsHelper.Units) 
                        {
                            if (u.Factor == UnitsFactors.Milli && string.Compare(u.NameCyr, res.Unit.NameCyr, true) == 0) 
                            {
                                res.Unit = u;
                                break;
                            }
                        }
                    }
                    res._correct();
                    res._checkDoubt();
                    uts.Add(res);
                }
                int max = 0;
                UnitToken best = null;
                foreach (UnitToken ut in uts) 
                {
                    if (ut.Keyword != null) 
                    {
                        if (ut.Keyword.BeginChar >= max) 
                        {
                            max = ut.Keyword.BeginChar;
                            best = ut;
                        }
                    }
                }
                if (best != null) 
                    return best;
                foreach (UnitToken ut in uts) 
                {
                    if (!ut.IsDoubt) 
                        return ut;
                }
                return uts[0];
            }
            Pullenti.Ner.Token t1 = null;
            if (t.IsCharOf("º°")) 
                t1 = t;
            else if ((t.IsChar('<') && t.Next != null && t.Next.Next != null) && t.Next.Next.IsChar('>') && ((t.Next.IsValue("О", null) || t.Next.IsValue("O", null) || (((t.Next is Pullenti.Ner.NumberToken) && (t.Next as Pullenti.Ner.NumberToken).Value == "0"))))) 
                t1 = t.Next.Next;
            if (t1 != null) 
            {
                UnitToken res = new UnitToken(t0, t1) { Unit = UnitsHelper.uGradus };
                res._checkDoubt();
                t = t1.Next;
                if (t != null && t.IsComma) 
                    t = t.Next;
                if (t != null && t.IsValue("ПО", null)) 
                    t = t.Next;
                if (t is Pullenti.Ner.TextToken) 
                {
                    string vv = (t as Pullenti.Ner.TextToken).Term;
                    if (vv == "C" || vv == "С" || vv.StartsWith("ЦЕЛЬС")) 
                    {
                        res.Unit = UnitsHelper.uGradusC;
                        res.IsDoubt = false;
                        res.EndToken = t;
                    }
                    if (vv == "F" || vv.StartsWith("ФАР")) 
                    {
                        res.Unit = UnitsHelper.uGradusF;
                        res.IsDoubt = false;
                        res.EndToken = t;
                    }
                }
                return res;
            }
            if ((t is Pullenti.Ner.TextToken) && ((t.IsValue("ОС", null) || t.IsValue("OC", null)))) 
            {
                string str = t.GetSourceText();
                if (str == "оС" || str == "oC") 
                {
                    UnitToken res = new UnitToken(t, t) { Unit = UnitsHelper.uGradusC, IsDoubt = false };
                    return res;
                }
            }
            if (t.IsChar('%')) 
            {
                Pullenti.Ner.Token tt1 = t.Next;
                if (tt1 != null && tt1.IsChar('(')) 
                    tt1 = tt1.Next;
                if ((tt1 is Pullenti.Ner.TextToken) && (tt1 as Pullenti.Ner.TextToken).Term.StartsWith("ОБ")) 
                {
                    UnitToken re = new UnitToken(t, tt1) { Unit = UnitsHelper.uAlco };
                    if (re.EndToken.Next != null && re.EndToken.Next.IsChar('.')) 
                        re.EndToken = re.EndToken.Next;
                    if (re.EndToken.Next != null && re.EndToken.Next.IsChar(')') && t.Next.IsChar('(')) 
                        re.EndToken = re.EndToken.Next;
                    return re;
                }
                return new UnitToken(t, t) { Unit = UnitsHelper.uPercent };
            }
            if (addUnits != null) 
            {
                Pullenti.Ner.Core.TerminToken tok = addUnits.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                {
                    UnitToken res = new UnitToken(t0, tok.EndToken) { ExtOnto = tok.Termin.Tag as Pullenti.Ner.Measure.UnitReferent };
                    if (tok.EndToken.Next != null && tok.EndToken.Next.IsChar('.')) 
                        tok.EndToken = tok.EndToken.Next;
                    res.Pow = pow;
                    if (isNeg) 
                        res.Pow = -pow;
                    res._correct();
                    return res;
                }
            }
            if (!parseUnknownUnits) 
                return null;
            if ((t.WhitespacesBeforeCount > 2 || !t.Chars.IsLetter || t.LengthChar > 5) || !(t is Pullenti.Ner.TextToken)) 
                return null;
            if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                return null;
            t1 = t;
            if (t.Next != null && t.Next.IsChar('.')) 
                t1 = t;
            bool ok = false;
            if (t1.Next == null || t1.WhitespacesAfterCount > 2) 
                ok = true;
            else if (t1.Next.IsComma || t1.Next.IsCharOf("\\/") || t1.Next.IsTableControlChar) 
                ok = true;
            else if (MeasureHelper.IsMultChar(t1.Next)) 
                ok = true;
            if (!ok) 
                return null;
            Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
            if (mc.IsUndefined) 
            {
            }
            else if (t.LengthChar > 7) 
                return null;
            UnitToken res1 = new UnitToken(t0, t1) { Pow = pow, IsDoubt = true };
            res1.UnknownName = (t as Pullenti.Ner.TextToken).GetSourceText();
            res1._correct();
            return res1;
        }
        void _correct()
        {
            Pullenti.Ner.Token t = EndToken.Next;
            if (t == null) 
                return;
            int num = 0;
            bool neg = Pow < 0;
            if (t.IsChar('³')) 
                num = 3;
            else if (t.IsChar('²')) 
                num = 2;
            else if (!t.IsWhitespaceBefore && (t is Pullenti.Ner.NumberToken) && (((t as Pullenti.Ner.NumberToken).Value == "3" || (t as Pullenti.Ner.NumberToken).Value == "2"))) 
                num = (t as Pullenti.Ner.NumberToken).IntValue.Value;
            else if ((t.IsChar('<') && (t.Next is Pullenti.Ner.NumberToken) && (t.Next as Pullenti.Ner.NumberToken).IntValue != null) && t.Next.Next != null && t.Next.Next.IsChar('>')) 
            {
                num = (t.Next as Pullenti.Ner.NumberToken).IntValue.Value;
                t = t.Next.Next;
            }
            else if (((t.IsChar('<') && t.Next != null && t.Next.IsHiphen) && (t.Next.Next is Pullenti.Ner.NumberToken) && (t.Next.Next as Pullenti.Ner.NumberToken).IntValue != null) && t.Next.Next.Next != null && t.Next.Next.Next.IsChar('>')) 
            {
                num = (t.Next.Next as Pullenti.Ner.NumberToken).IntValue.Value;
                neg = true;
                t = t.Next.Next.Next;
            }
            else 
            {
                if (t.IsValue("B", null) && t.Next != null) 
                    t = t.Next;
                if ((t.IsValue("КВ", null) || t.IsValue("КВАДР", null) || t.IsValue("КВАДРАТНЫЙ", null)) || t.IsValue("КВАДРАТ", null)) 
                {
                    num = 2;
                    if (t.Next != null && t.Next.IsChar('.')) 
                        t = t.Next;
                }
                else if (t.IsValue("КУБ", null) || t.IsValue("КУБИЧ", null) || t.IsValue("КУБИЧЕСКИЙ", null)) 
                {
                    num = 3;
                    if (t.Next != null && t.Next.IsChar('.')) 
                        t = t.Next;
                }
            }
            if (num != 0) 
            {
                Pow = num;
                if (neg) 
                    Pow = -num;
                EndToken = t;
            }
            t = EndToken.Next;
            if ((t != null && t.IsValue("ПО", null) && t.Next != null) && t.Next.IsValue("U", null)) 
                EndToken = t.Next;
        }
        void _checkDoubt()
        {
            IsDoubt = false;
            if (Pow != 1) 
                return;
            if (BeginToken.LengthChar < 3) 
            {
                IsDoubt = true;
                if ((BeginToken.Chars.IsCapitalUpper || BeginToken.Chars.IsAllUpper || BeginToken.Chars.IsLastLower) || BeginToken.Chars.IsAllLower) 
                {
                }
                else if (Unit.Psevdo.Count > 0) 
                {
                }
                else 
                    IsDoubt = false;
            }
            int cou = 0;
            for (Pullenti.Ner.Token t = BeginToken.Previous; t != null && (cou < 30); t = t.Previous,cou++) 
            {
                Pullenti.Ner.Measure.MeasureReferent mr = t.GetReferent() as Pullenti.Ner.Measure.MeasureReferent;
                if (mr != null) 
                {
                    foreach (Pullenti.Ner.Slot s in mr.Slots) 
                    {
                        if (s.Value is Pullenti.Ner.Measure.UnitReferent) 
                        {
                            Pullenti.Ner.Measure.UnitReferent ur = s.Value as Pullenti.Ner.Measure.UnitReferent;
                            for (Unit u = Unit; u != null; u = u.BaseUnit) 
                            {
                                if (ur.FindSlot(Pullenti.Ner.Measure.UnitReferent.ATTR_NAME, u.NameCyr, true) != null) 
                                    IsDoubt = false;
                                else if (Unit.Psevdo.Count > 0) 
                                {
                                    foreach (Unit uu in Unit.Psevdo) 
                                    {
                                        if (ur.FindSlot(Pullenti.Ner.Measure.UnitReferent.ATTR_NAME, uu.NameCyr, true) != null) 
                                        {
                                            Unit = uu;
                                            IsDoubt = false;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!(t is Pullenti.Ner.TextToken) || (t.LengthChar < 3)) 
                    continue;
                for (Unit u = Unit; u != null; u = u.BaseUnit) 
                {
                    foreach (string k in u.Keywords) 
                    {
                        if (t.IsValue(k, null)) 
                        {
                            Keyword = t;
                            IsDoubt = false;
                            return;
                        }
                    }
                    foreach (Unit uu in u.Psevdo) 
                    {
                        foreach (string k in uu.Keywords) 
                        {
                            if (t.IsValue(k, null)) 
                            {
                                Unit = uu;
                                Keyword = t;
                                IsDoubt = false;
                                return;
                            }
                        }
                    }
                }
            }
        }
        public static string OutUnits(List<UnitToken> units)
        {
            if (units == null || units.Count == 0) 
                return null;
            StringBuilder res = new StringBuilder();
            res.Append(units[0].Unit.NameCyr);
            if (units[0].Pow != 1) 
                res.AppendFormat("<{0}>", units[0].Pow);
            for (int i = 1; i < units.Count; i++) 
            {
                string mnem = units[i].Unit.NameCyr;
                int pow = units[i].Pow;
                if (pow < 0) 
                {
                    res.AppendFormat("/{0}", mnem);
                    if (pow != -1) 
                        res.AppendFormat("<{0}>", -pow);
                }
                else 
                {
                    res.AppendFormat("*{0}", mnem);
                    if (pow > 1) 
                        res.AppendFormat("<{0}>", pow);
                }
            }
            return res.ToString();
        }
    }
}