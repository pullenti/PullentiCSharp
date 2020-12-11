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
    // Это для моделирования разных числовых диапазонов + единицы изменерия
    public class NumbersWithUnitToken : Pullenti.Ner.MetaToken
    {
        public NumbersWithUnitToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public double? SingleVal;
        public double? PlusMinus;
        public bool PlusMinusPercent;
        public bool FromInclude;
        public double? FromVal;
        public bool ToInclude;
        public double? ToVal;
        public bool About;
        public bool Not = false;
        public Pullenti.Ner.MetaToken WHL;
        public List<UnitToken> Units = new List<UnitToken>();
        public NumbersWithUnitToken DivNum;
        public bool IsAge;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (SingleVal != null) 
            {
                if (PlusMinus != null) 
                    res.AppendFormat("[{0} ±{1}{2}]", SingleVal.Value, PlusMinus.Value, (PlusMinusPercent ? "%" : ""));
                else 
                    res.Append(SingleVal.Value);
            }
            else 
            {
                if (FromVal != null) 
                    res.AppendFormat("{0}{1}", (FromInclude ? '[' : ']'), FromVal.Value);
                else 
                    res.Append("]");
                res.Append(" .. ");
                if (ToVal != null) 
                    res.AppendFormat("{0}{1}", ToVal.Value, (ToInclude ? ']' : '['));
                else 
                    res.Append("[");
            }
            foreach (UnitToken u in Units) 
            {
                res.AppendFormat(" {0}", u.ToString());
            }
            if (DivNum != null) 
            {
                res.Append(" / ");
                res.Append(DivNum);
            }
            return res.ToString();
        }
        public List<Pullenti.Ner.ReferentToken> CreateRefenetsTokensWithRegister(Pullenti.Ner.Core.AnalyzerData ad, string name, bool regist = true)
        {
            if (name == "T =") 
                name = "ТЕМПЕРАТУРА";
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            foreach (UnitToken u in Units) 
            {
                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(u.CreateReferentWithRegister(ad), u.BeginToken, u.EndToken);
                res.Add(rt);
            }
            Pullenti.Ner.Measure.MeasureReferent mr = new Pullenti.Ner.Measure.MeasureReferent();
            string templ = "1";
            if (SingleVal != null) 
            {
                mr.AddValue(SingleVal.Value);
                if (PlusMinus != null) 
                {
                    templ = string.Format("[1 ±2{0}]", (PlusMinusPercent ? "%" : ""));
                    mr.AddValue(PlusMinus.Value);
                }
                else if (About) 
                    templ = "~1";
            }
            else 
            {
                if (Not && ((FromVal == null || ToVal == null))) 
                {
                    bool b = FromInclude;
                    FromInclude = ToInclude;
                    ToInclude = b;
                    double? v = FromVal;
                    FromVal = ToVal;
                    ToVal = v;
                }
                int num = 1;
                if (FromVal != null) 
                {
                    mr.AddValue(FromVal.Value);
                    templ = (FromInclude ? "[1" : "]1");
                    num++;
                }
                else 
                    templ = "]";
                if (ToVal != null) 
                {
                    mr.AddValue(ToVal.Value);
                    templ = string.Format("{0} .. {1}{2}", templ, num, (ToInclude ? ']' : '['));
                }
                else 
                    templ += " .. [";
            }
            mr.Template = templ;
            foreach (Pullenti.Ner.ReferentToken rt in res) 
            {
                mr.AddSlot(Pullenti.Ner.Measure.MeasureReferent.ATTR_UNIT, rt.Referent, false, 0);
            }
            if (name != null) 
                mr.AddSlot(Pullenti.Ner.Measure.MeasureReferent.ATTR_NAME, name, false, 0);
            if (DivNum != null) 
            {
                List<Pullenti.Ner.ReferentToken> dn = DivNum.CreateRefenetsTokensWithRegister(ad, null, true);
                res.AddRange(dn);
                mr.AddSlot(Pullenti.Ner.Measure.MeasureReferent.ATTR_REF, dn[dn.Count - 1].Referent, false, 0);
            }
            Pullenti.Ner.Measure.MeasureKind ki = UnitToken.CalcKind(Units);
            if (ki != Pullenti.Ner.Measure.MeasureKind.Undefined) 
                mr.Kind = ki;
            if (regist && ad != null) 
                mr = ad.RegisterReferent(mr) as Pullenti.Ner.Measure.MeasureReferent;
            res.Add(new Pullenti.Ner.ReferentToken(mr, BeginToken, EndToken));
            return res;
        }
        public static List<NumbersWithUnitToken> TryParseMulti(Pullenti.Ner.Token t, Pullenti.Ner.Core.TerminCollection addUnits, bool canOmitNumber = false, bool not = false, bool canBeNon = false, bool isResctriction = false)
        {
            if (t == null || (t is Pullenti.Ner.ReferentToken)) 
                return null;
            Pullenti.Ner.Token tt0 = t;
            if (tt0.IsChar('(')) 
            {
                Pullenti.Ner.MetaToken whd = _tryParseWHL(tt0);
                if (whd != null) 
                    tt0 = whd.EndToken;
                List<NumbersWithUnitToken> res0 = TryParseMulti(tt0.Next, addUnits, false, canOmitNumber, canBeNon, false);
                if (res0 != null) 
                {
                    res0[0].WHL = whd;
                    Pullenti.Ner.Token tt2 = res0[res0.Count - 1].EndToken.Next;
                    if (tt2 != null && tt2.IsCharOf(",")) 
                        tt2 = tt2.Next;
                    if (whd != null) 
                        return res0;
                    if (tt2 != null && tt2.IsChar(')')) 
                    {
                        res0[res0.Count - 1].EndToken = tt2;
                        return res0;
                    }
                }
            }
            NumbersWithUnitToken mt = TryParse(t, addUnits, canOmitNumber, not, canBeNon, isResctriction);
            if (mt == null) 
                return null;
            List<NumbersWithUnitToken> res = new List<NumbersWithUnitToken>();
            Pullenti.Ner.Token nnn = null;
            if (mt.WhitespacesAfterCount < 2) 
            {
                if (MeasureHelper.IsMultChar(mt.EndToken.Next)) 
                    nnn = mt.EndToken.Next.Next;
                else if ((mt.EndToken is Pullenti.Ner.NumberToken) && MeasureHelper.IsMultChar((mt.EndToken as Pullenti.Ner.NumberToken).EndToken)) 
                    nnn = mt.EndToken.Next;
            }
            if (nnn != null) 
            {
                NumbersWithUnitToken mt2 = NumbersWithUnitToken.TryParse(nnn, addUnits, not, false, false, false);
                if (mt2 != null) 
                {
                    NumbersWithUnitToken mt3 = null;
                    nnn = null;
                    if (mt2.WhitespacesAfterCount < 2) 
                    {
                        if (MeasureHelper.IsMultChar(mt2.EndToken.Next)) 
                            nnn = mt2.EndToken.Next.Next;
                        else if ((mt2.EndToken is Pullenti.Ner.NumberToken) && MeasureHelper.IsMultChar((mt2.EndToken as Pullenti.Ner.NumberToken).EndToken)) 
                            nnn = mt2.EndToken.Next;
                    }
                    if (nnn != null) 
                        mt3 = NumbersWithUnitToken.TryParse(nnn, addUnits, false, false, false, false);
                    if (mt3 == null) 
                    {
                        Pullenti.Ner.Token tt2 = mt2.EndToken.Next;
                        if (tt2 != null && !tt2.IsWhitespaceBefore) 
                        {
                            if (!tt2.IsCharOf(",.;")) 
                                return null;
                        }
                    }
                    if (mt3 != null && mt3.Units.Count > 0) 
                    {
                        if (mt2.Units.Count == 0) 
                            mt2.Units = mt3.Units;
                    }
                    res.Add(mt);
                    if (mt2 != null) 
                    {
                        if (mt2.Units.Count > 0 && mt.Units.Count == 0) 
                            mt.Units = mt2.Units;
                        res.Add(mt2);
                        if (mt3 != null) 
                            res.Add(mt3);
                    }
                    return res;
                }
            }
            if ((!mt.IsWhitespaceAfter && MeasureHelper.IsMultCharEnd(mt.EndToken.Next) && (mt.EndToken.Next.Next is Pullenti.Ner.NumberToken)) && mt.Units.Count == 0) 
            {
                string utxt = (mt.EndToken.Next as Pullenti.Ner.TextToken).Term;
                utxt = utxt.Substring(0, utxt.Length - 1);
                List<Pullenti.Ner.Core.Termin> terms = UnitsHelper.Termins.FindTerminsByString(utxt, null);
                if (terms != null && terms.Count > 0) 
                {
                    mt.Units.Add(new UnitToken(mt.EndToken.Next, mt.EndToken.Next) { Unit = terms[0].Tag as Unit });
                    mt.EndToken = mt.EndToken.Next;
                    List<NumbersWithUnitToken> res1 = TryParseMulti(mt.EndToken.Next, addUnits, false, false, false, false);
                    if (res1 != null) 
                    {
                        res1.Insert(0, mt);
                        return res1;
                    }
                }
            }
            res.Add(mt);
            return res;
        }
        public static NumbersWithUnitToken TryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.TerminCollection addUnits, bool canOmitNumber = false, bool not = false, bool canBeNan = false, bool isResctriction = false)
        {
            if (t == null) 
                return null;
            NumbersWithUnitToken res = _tryParse(t, addUnits, isResctriction, canOmitNumber, canBeNan);
            if (res != null) 
                res.Not = not;
            return res;
        }
        internal static Pullenti.Ner.Token _isMinOrMax(Pullenti.Ner.Token t, ref int res)
        {
            if (t == null) 
                return null;
            if (t.IsValue("МИНИМАЛЬНЫЙ", null) || t.IsValue("МИНИМУМ", null) || t.IsValue("MINIMUM", null)) 
            {
                res = -1;
                return t;
            }
            if (t.IsValue("MIN", null) || t.IsValue("МИН", null)) 
            {
                res = -1;
                if (t.Next != null && t.Next.IsChar('.')) 
                    t = t.Next;
                return t;
            }
            if (t.IsValue("МАКСИМАЛЬНЫЙ", null) || t.IsValue("МАКСИМУМ", null) || t.IsValue("MAXIMUM", null)) 
            {
                res = 1;
                return t;
            }
            if (t.IsValue("MAX", null) || t.IsValue("МАКС", null) || t.IsValue("МАХ", null)) 
            {
                res = 1;
                if (t.Next != null && t.Next.IsChar('.')) 
                    t = t.Next;
                return t;
            }
            if (t.IsChar('(')) 
            {
                t = _isMinOrMax(t.Next, ref res);
                if (t != null && t.Next != null && t.Next.IsChar(')')) 
                    t = t.Next;
                return t;
            }
            return null;
        }
        internal static NumbersWithUnitToken _tryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.TerminCollection addUnits, bool second, bool canOmitNumber, bool canBeNan)
        {
            if (t == null) 
                return null;
            while (t != null) 
            {
                if (t.IsCommaAnd || t.IsValue("НО", null)) 
                    t = t.Next;
                else 
                    break;
            }
            Pullenti.Ner.Token t0 = t;
            bool about = false;
            bool hasKeyw = false;
            bool isDiapKeyw = false;
            int minMax = 0;
            Pullenti.Ner.Token ttt = _isMinOrMax(t, ref minMax);
            if (ttt != null) 
            {
                t = ttt.Next;
                if (t == null) 
                    return null;
            }
            if (t == null) 
                return null;
            if (t.IsChar('~') || t.IsValue("ОКОЛО", null) || t.IsValue("ПРИМЕРНО", null)) 
            {
                t = t.Next;
                about = true;
                hasKeyw = true;
                if (t == null) 
                    return null;
            }
            if (t.IsValue("В", null) && t.Next != null) 
            {
                if (t.Next.IsValue("ПРЕДЕЛ", null) || t.IsValue("ДИАПАЗОН", null)) 
                {
                    t = t.Next.Next;
                    if (t == null) 
                        return null;
                    isDiapKeyw = true;
                }
            }
            if (t0.IsChar('(')) 
            {
                NumbersWithUnitToken mt0 = _tryParse(t.Next, addUnits, false, false, false);
                if (mt0 != null && mt0.EndToken.Next != null && mt0.EndToken.Next.IsChar(')')) 
                {
                    if (second) 
                    {
                        if (mt0.FromVal != null && mt0.ToVal != null && mt0.FromVal.Value == (-mt0.ToVal.Value)) 
                        {
                        }
                        else 
                            return null;
                    }
                    mt0.BeginToken = t0;
                    mt0.EndToken = mt0.EndToken.Next;
                    List<UnitToken> uu = UnitToken.TryParseList(mt0.EndToken.Next, addUnits, false);
                    if (uu != null && mt0.Units.Count == 0) 
                    {
                        mt0.Units = uu;
                        mt0.EndToken = uu[uu.Count - 1].EndToken;
                    }
                    return mt0;
                }
            }
            bool plusminus = false;
            bool unitBefore = false;
            bool isAge = false;
            DiapTyp dty = DiapTyp.Undefined;
            Pullenti.Ner.MetaToken whd = null;
            List<UnitToken> uni = null;
            Pullenti.Ner.Core.TerminToken tok = (m_Termins == null ? null : m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No));
            if (tok != null) 
            {
                if (tok.EndToken.IsValue("СТАРШЕ", null) || tok.EndToken.IsValue("МЛАДШЕ", null)) 
                    isAge = true;
                t = tok.EndToken.Next;
                dty = (DiapTyp)tok.Termin.Tag;
                hasKeyw = true;
                if (!tok.IsWhitespaceAfter) 
                {
                    if (t == null) 
                        return null;
                    if (t is Pullenti.Ner.NumberToken) 
                    {
                        if (tok.BeginToken == tok.EndToken && !tok.Chars.IsAllLower) 
                            return null;
                    }
                    else if (t.IsComma && t.Next != null && t.Next.IsValue("ЧЕМ", null)) 
                    {
                        t = t.Next.Next;
                        if (t != null && t.Morph.Class.IsPreposition) 
                            t = t.Next;
                    }
                    else if (t.IsCharOf(":,(") || t.IsTableControlChar) 
                    {
                    }
                    else 
                        return null;
                }
                if (t != null && t.IsChar('(')) 
                {
                    uni = UnitToken.TryParseList(t.Next, addUnits, false);
                    if (uni != null) 
                    {
                        t = uni[uni.Count - 1].EndToken.Next;
                        while (t != null) 
                        {
                            if (t.IsCharOf("):")) 
                                t = t.Next;
                            else 
                                break;
                        }
                        NumbersWithUnitToken mt0 = _tryParse(t, addUnits, false, canOmitNumber, false);
                        if (mt0 != null && mt0.Units.Count == 0) 
                        {
                            mt0.BeginToken = t0;
                            mt0.Units = uni;
                            return mt0;
                        }
                    }
                    whd = _tryParseWHL(t);
                    if (whd != null) 
                        t = whd.EndToken.Next;
                }
                else if (t != null && t.IsValue("IP", null)) 
                {
                    uni = UnitToken.TryParseList(t, addUnits, false);
                    if (uni != null) 
                        t = uni[uni.Count - 1].EndToken.Next;
                }
                if ((t != null && t.IsHiphen && t.IsWhitespaceBefore) && t.IsWhitespaceAfter) 
                    t = t.Next;
            }
            else if (t.IsChar('<')) 
            {
                dty = DiapTyp.Ls;
                t = t.Next;
                hasKeyw = true;
                if (t != null && t.IsChar('=')) 
                {
                    t = t.Next;
                    dty = DiapTyp.Le;
                }
            }
            else if (t.IsChar('>')) 
            {
                dty = DiapTyp.Gt;
                t = t.Next;
                hasKeyw = true;
                if (t != null && t.IsChar('=')) 
                {
                    t = t.Next;
                    dty = DiapTyp.Ge;
                }
            }
            else if (t.IsChar('≤')) 
            {
                dty = DiapTyp.Le;
                hasKeyw = true;
                t = t.Next;
            }
            else if (t.IsChar('≥')) 
            {
                dty = DiapTyp.Ge;
                hasKeyw = true;
                t = t.Next;
            }
            else if (t.IsValue("IP", null)) 
            {
                uni = UnitToken.TryParseList(t, addUnits, false);
                if (uni != null) 
                    t = uni[uni.Count - 1].EndToken.Next;
            }
            else if (t.IsValue("ЗА", null) && (t.Next is Pullenti.Ner.NumberToken)) 
            {
                dty = DiapTyp.Ge;
                t = t.Next;
            }
            while (t != null && ((t.IsCharOf(":,") || t.IsValue("ЧЕМ", null) || t.IsTableControlChar))) 
            {
                t = t.Next;
            }
            if (t != null) 
            {
                if (t.IsChar('+') || t.IsValue("ПЛЮС", null)) 
                {
                    t = t.Next;
                    if (t != null && !t.IsWhitespaceBefore) 
                    {
                        if (t.IsHiphen) 
                        {
                            t = t.Next;
                            plusminus = true;
                        }
                        else if ((t.IsCharOf("\\/") && t.Next != null && !t.IsNewlineAfter) && t.Next.IsHiphen) 
                        {
                            t = t.Next.Next;
                            plusminus = true;
                        }
                    }
                }
                else if (second && (t.IsCharOf("\\/÷…~"))) 
                    t = t.Next;
                else if ((t.IsHiphen && t == t0 && !second) && m_Termins.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                {
                    tok = m_Termins.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                    t = tok.EndToken.Next;
                    dty = (DiapTyp)tok.Termin.Tag;
                }
                else if (t.IsHiphen && t == t0 && ((t.IsWhitespaceAfter || second))) 
                    t = t.Next;
                else if (t.IsChar('±')) 
                {
                    t = t.Next;
                    plusminus = true;
                    hasKeyw = true;
                }
                else if ((second && t.IsChar('.') && t.Next != null) && t.Next.IsChar('.')) 
                {
                    t = t.Next.Next;
                    if (t != null && t.IsChar('.')) 
                        t = t.Next;
                }
            }
            Pullenti.Ner.NumberToken num = Pullenti.Ner.Core.NumberHelper.TryParseRealNumber(t, true, false);
            if (num == null) 
            {
                uni = UnitToken.TryParseList(t, addUnits, false);
                if (uni != null) 
                {
                    unitBefore = true;
                    t = uni[uni.Count - 1].EndToken.Next;
                    bool delim = false;
                    while (t != null) 
                    {
                        if (t.IsCharOf(":,")) 
                        {
                            delim = true;
                            t = t.Next;
                        }
                        else if (t.IsHiphen && t.IsWhitespaceAfter) 
                        {
                            delim = true;
                            t = t.Next;
                        }
                        else 
                            break;
                    }
                    if (!delim) 
                    {
                        if (t == null) 
                        {
                            if (hasKeyw && canBeNan) 
                            {
                            }
                            else 
                                return null;
                        }
                        else if (!t.IsWhitespaceBefore) 
                            return null;
                        if (t.Next != null && t.IsHiphen && t.IsWhitespaceAfter) 
                        {
                            delim = true;
                            t = t.Next;
                        }
                    }
                    num = Pullenti.Ner.Core.NumberHelper.TryParseRealNumber(t, true, false);
                }
            }
            NumbersWithUnitToken res = null;
            double rval = (double)0;
            if (num == null) 
            {
                Pullenti.Ner.Core.TerminToken tt = m_Spec.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tt != null) 
                {
                    rval = (double)tt.Termin.Tag;
                    string unam = (string)tt.Termin.Tag2;
                    foreach (Unit u in UnitsHelper.Units) 
                    {
                        if (u.FullnameCyr == unam) 
                        {
                            uni = new List<UnitToken>();
                            uni.Add(new UnitToken(t, t) { Unit = u });
                            break;
                        }
                    }
                    if (uni == null) 
                        return null;
                    res = new NumbersWithUnitToken(t0, tt.EndToken) { About = about };
                    t = tt.EndToken.Next;
                }
                else 
                {
                    if (!canOmitNumber && !hasKeyw && !canBeNan) 
                        return null;
                    if ((uni != null && uni.Count == 1 && uni[0].BeginToken == uni[0].EndToken) && uni[0].LengthChar > 3) 
                    {
                        rval = 1;
                        res = new NumbersWithUnitToken(t0, uni[uni.Count - 1].EndToken) { About = about };
                        t = res.EndToken.Next;
                    }
                    else if (hasKeyw && canBeNan) 
                    {
                        rval = double.NaN;
                        res = new NumbersWithUnitToken(t0, t0) { About = about };
                        if (t != null) 
                            res.EndToken = t.Previous;
                        else 
                            for (t = t0; t != null; t = t.Next) 
                            {
                                res.EndToken = t;
                            }
                    }
                    else 
                        return null;
                }
            }
            else 
            {
                if ((t == t0 && t0.IsHiphen && !t.IsWhitespaceBefore) && !t.IsWhitespaceAfter && (num.RealValue < 0)) 
                {
                    num = Pullenti.Ner.Core.NumberHelper.TryParseRealNumber(t.Next, true, false);
                    if (num == null) 
                        return null;
                }
                if (t == t0 && (t is Pullenti.Ner.NumberToken) && t.Morph.Class.IsAdjective) 
                {
                    Pullenti.Ner.TextToken nn = (t as Pullenti.Ner.NumberToken).EndToken as Pullenti.Ner.TextToken;
                    if (nn == null) 
                        return null;
                    string norm = nn.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                    if ((norm.EndsWith("Ь") || norm == "ЧЕТЫРЕ" || norm == "ТРИ") || norm == "ДВА") 
                    {
                    }
                    else 
                    {
                        Pullenti.Morph.MorphWordForm mi = Pullenti.Morph.MorphologyService.GetWordBaseInfo("КОКО" + nn.Term, null, false, false);
                        if (mi.Class.IsAdjective) 
                            return null;
                    }
                }
                t = num.EndToken.Next;
                res = new NumbersWithUnitToken(t0, num.EndToken) { About = about };
                rval = num.RealValue;
            }
            if (uni == null) 
            {
                uni = UnitToken.TryParseList(t, addUnits, false);
                if (uni != null) 
                {
                    if ((plusminus && second && uni.Count >= 1) && uni[0].Unit == UnitsHelper.uPercent) 
                    {
                        res.EndToken = uni[0].EndToken;
                        res.PlusMinusPercent = true;
                        Pullenti.Ner.Token tt1 = uni[0].EndToken.Next;
                        uni = UnitToken.TryParseList(tt1, addUnits, false);
                        if (uni != null) 
                        {
                            res.Units = uni;
                            res.EndToken = uni[uni.Count - 1].EndToken;
                        }
                    }
                    else 
                    {
                        res.Units = uni;
                        res.EndToken = uni[uni.Count - 1].EndToken;
                    }
                    t = res.EndToken.Next;
                }
            }
            else 
            {
                res.Units = uni;
                if (uni.Count > 1) 
                {
                    List<UnitToken> uni1 = UnitToken.TryParseList(t, addUnits, false);
                    if (((uni1 != null && uni1[0].Unit == uni[0].Unit && (uni1.Count < uni.Count)) && uni[uni1.Count].Pow == -1 && uni1[uni1.Count - 1].EndToken.Next != null) && uni1[uni1.Count - 1].EndToken.Next.IsCharOf("/\\")) 
                    {
                        NumbersWithUnitToken num2 = _tryParse(uni1[uni1.Count - 1].EndToken.Next.Next, addUnits, false, false, false);
                        if (num2 != null && num2.Units != null && num2.Units[0].Unit == uni[uni1.Count].Unit) 
                        {
                            res.Units = uni1;
                            res.DivNum = num2;
                            res.EndToken = num2.EndToken;
                        }
                    }
                }
            }
            res.WHL = whd;
            if (dty != DiapTyp.Undefined) 
            {
                if (dty == DiapTyp.Ge || dty == DiapTyp.From) 
                {
                    res.FromInclude = true;
                    res.FromVal = rval;
                }
                else if (dty == DiapTyp.Gt) 
                {
                    res.FromInclude = false;
                    res.FromVal = rval;
                }
                else if (dty == DiapTyp.Le || dty == DiapTyp.To) 
                {
                    res.ToInclude = true;
                    res.ToVal = rval;
                }
                else if (dty == DiapTyp.Ls) 
                {
                    res.ToInclude = false;
                    res.ToVal = rval;
                }
            }
            bool isSecondMax = false;
            if (!second) 
            {
                int iii = 0;
                ttt = _isMinOrMax(t, ref iii);
                if (ttt != null && iii > 0) 
                {
                    isSecondMax = true;
                    t = ttt.Next;
                }
            }
            NumbersWithUnitToken next = (second || plusminus || ((t != null && ((t.IsTableControlChar || t.IsNewlineBefore)))) ? null : _tryParse(t, addUnits, true, false, canBeNan));
            if (next != null && (t.Previous is Pullenti.Ner.NumberToken)) 
            {
                if (MeasureHelper.IsMultChar((t.Previous as Pullenti.Ner.NumberToken).EndToken)) 
                    next = null;
            }
            if (next != null && ((next.ToVal != null || next.SingleVal != null)) && next.FromVal == null) 
            {
                if ((((next.BeginToken.IsChar('+') && next.SingleVal != null && !double.IsNaN(next.SingleVal.Value)) && next.EndToken.Next != null && next.EndToken.Next.IsCharOf("\\/")) && next.EndToken.Next.Next != null && next.EndToken.Next.Next.IsHiphen) && !hasKeyw && !double.IsNaN(rval)) 
                {
                    NumbersWithUnitToken next2 = _tryParse(next.EndToken.Next.Next.Next, addUnits, true, false, false);
                    if (next2 != null && next2.SingleVal != null && !double.IsNaN(next2.SingleVal.Value)) 
                    {
                        res.FromVal = rval - next2.SingleVal.Value;
                        res.FromInclude = true;
                        res.ToVal = rval + next.SingleVal.Value;
                        res.ToInclude = true;
                        if (next2.Units != null && res.Units.Count == 0) 
                            res.Units = next2.Units;
                        res.EndToken = next2.EndToken;
                        return res;
                    }
                }
                if (next.Units.Count > 0) 
                {
                    if (res.Units.Count == 0) 
                        res.Units = next.Units;
                    else if (!UnitToken.CanBeEquals(res.Units, next.Units)) 
                        next = null;
                }
                else if (res.Units.Count > 0 && !unitBefore && !next.PlusMinusPercent) 
                    next = null;
                if (next != null) 
                    res.EndToken = next.EndToken;
                if (next != null && next.ToVal != null) 
                {
                    res.ToVal = next.ToVal;
                    res.ToInclude = next.ToInclude;
                }
                else if (next != null && next.SingleVal != null) 
                {
                    if (next.BeginToken.IsCharOf("/\\")) 
                    {
                        res.DivNum = next;
                        res.SingleVal = rval;
                        return res;
                    }
                    else if (next.PlusMinusPercent) 
                    {
                        res.SingleVal = rval;
                        res.PlusMinus = next.SingleVal;
                        res.PlusMinusPercent = true;
                        res.ToInclude = true;
                    }
                    else 
                    {
                        res.ToVal = next.SingleVal;
                        res.ToInclude = true;
                    }
                }
                if (next != null) 
                {
                    if (res.FromVal == null) 
                    {
                        res.FromVal = rval;
                        res.FromInclude = true;
                    }
                    return res;
                }
            }
            else if ((next != null && next.FromVal != null && next.ToVal != null) && next.ToVal.Value == (-next.FromVal.Value)) 
            {
                if (next.Units.Count == 1 && next.Units[0].Unit == UnitsHelper.uPercent && res.Units.Count > 0) 
                {
                    res.SingleVal = rval;
                    res.PlusMinus = next.ToVal.Value;
                    res.PlusMinusPercent = true;
                    res.EndToken = next.EndToken;
                    return res;
                }
                if (next.Units.Count == 0) 
                {
                    res.SingleVal = rval;
                    res.PlusMinus = next.ToVal.Value;
                    res.EndToken = next.EndToken;
                    return res;
                }
                res.FromVal = next.FromVal + rval;
                res.FromInclude = true;
                res.ToVal = next.ToVal + rval;
                res.ToInclude = true;
                res.EndToken = next.EndToken;
                if (next.Units.Count > 0) 
                    res.Units = next.Units;
                return res;
            }
            if (dty == DiapTyp.Undefined) 
            {
                if (plusminus && ((!res.PlusMinusPercent || !second))) 
                {
                    res.FromInclude = true;
                    res.FromVal = -rval;
                    res.ToInclude = true;
                    res.ToVal = rval;
                }
                else 
                {
                    res.SingleVal = rval;
                    res.PlusMinusPercent = plusminus;
                }
            }
            if (isAge) 
                res.IsAge = true;
            return res;
        }
        public static Pullenti.Ner.MetaToken _tryParseWHL(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            if (t.IsCharOf(":-")) 
            {
                Pullenti.Ner.MetaToken re0 = _tryParseWHL(t.Next);
                if (re0 != null) 
                    return re0;
            }
            if (t.IsCharOf("(")) 
            {
                Pullenti.Ner.MetaToken re0 = _tryParseWHL(t.Next);
                if (re0 != null) 
                {
                    if (re0.EndToken.Next != null && re0.EndToken.Next.IsChar(')')) 
                    {
                        re0.BeginToken = t;
                        re0.EndToken = re0.EndToken.Next;
                        return re0;
                    }
                }
            }
            string txt = (t as Pullenti.Ner.TextToken).Term;
            List<string> nams = null;
            if (txt.Length == 5 && ((txt[1] == 'Х' || txt[1] == 'X')) && ((txt[3] == 'Х' || txt[3] == 'X'))) 
            {
                nams = new List<string>();
                for (int i = 0; i < 3; i++) 
                {
                    char ch = txt[i * 2];
                    if (ch == 'Г') 
                        nams.Add("ГЛУБИНА");
                    else if (ch == 'В' || ch == 'H' || ch == 'Н') 
                        nams.Add("ВЫСОТА");
                    else if (ch == 'Ш' || ch == 'B' || ch == 'W') 
                        nams.Add("ШИРИНА");
                    else if (ch == 'Д' || ch == 'L') 
                        nams.Add("ДЛИНА");
                    else if (ch == 'D') 
                        nams.Add("ДИАМЕТР");
                    else 
                        return null;
                }
                return new Pullenti.Ner.MetaToken(t, t) { Tag = nams };
            }
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = t;
            for (; t != null; t = t.Next) 
            {
                if (!(t is Pullenti.Ner.TextToken) || ((t.WhitespacesBeforeCount > 1 && t != t0))) 
                    break;
                string term = (t as Pullenti.Ner.TextToken).Term;
                if (term.EndsWith("X") || term.EndsWith("Х")) 
                    term = term.Substring(0, term.Length - 1);
                string nam = null;
                if (((t.IsValue("ДЛИНА", null) || t.IsValue("ДЛИННА", null) || term == "Д") || term == "ДЛ" || term == "ДЛИН") || term == "L") 
                    nam = "ДЛИНА";
                else if (((t.IsValue("ШИРИНА", null) || t.IsValue("ШИРОТА", null) || term == "Ш") || term == "ШИР" || term == "ШИРИН") || term == "W" || term == "B") 
                    nam = "ШИРИНА";
                else if ((t.IsValue("ГЛУБИНА", null) || term == "Г" || term == "ГЛ") || term == "ГЛУБ") 
                    nam = "ГЛУБИНА";
                else if ((t.IsValue("ВЫСОТА", null) || term == "В" || term == "ВЫС") || term == "H" || term == "Н") 
                    nam = "ВЫСОТА";
                else if (t.IsValue("ДИАМЕТР", null) || term == "D" || term == "ДИАМ") 
                    nam = "ДИАМЕТР";
                else 
                    break;
                if (nams == null) 
                    nams = new List<string>();
                nams.Add(nam);
                t1 = t;
                if (t.Next != null && t.Next.IsChar('.')) 
                    t1 = (t = t.Next);
                if (t.Next == null) 
                    break;
                if (MeasureHelper.IsMultChar(t.Next) || t.Next.IsComma || t.Next.IsCharOf("\\/")) 
                    t = t.Next;
            }
            if (nams == null || (nams.Count < 2)) 
                return null;
            return new Pullenti.Ner.MetaToken(t0, t1) { Tag = nams };
        }
        internal static Pullenti.Ner.Core.TerminCollection m_Termins;
        internal static Pullenti.Ner.Core.TerminCollection m_Spec;
        internal static void Initialize()
        {
            if (m_Termins != null) 
                return;
            m_Termins = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin("НЕ МЕНЕЕ") { Tag = DiapTyp.Ge };
            t.AddVariant("НЕ МЕНЬШЕ", false);
            t.AddVariant("НЕ КОРОЧЕ", false);
            t.AddVariant("НЕ МЕДЛЕННЕЕ", false);
            t.AddVariant("НЕ НИЖЕ", false);
            t.AddVariant("НЕ МОЛОЖЕ", false);
            t.AddVariant("НЕ ДЕШЕВЛЕ", false);
            t.AddVariant("НЕ РЕЖЕ", false);
            t.AddVariant("НЕ МЕНЕ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("МЕНЕЕ") { Tag = DiapTyp.Ls };
            t.AddVariant("МЕНЬШЕ", false);
            t.AddVariant("МЕНЕ", false);
            t.AddVariant("КОРОЧЕ", false);
            t.AddVariant("МЕДЛЕННЕЕ", false);
            t.AddVariant("НИЖЕ", false);
            t.AddVariant("МЛАДШЕ", false);
            t.AddVariant("ДЕШЕВЛЕ", false);
            t.AddVariant("РЕЖЕ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("НЕ БОЛЕЕ") { Tag = DiapTyp.Le };
            t.AddVariant("НЕ БОЛЬШЕ", false);
            t.AddVariant("НЕ БОЛЕ", false);
            t.AddVariant("НЕ ДЛИННЕЕ", false);
            t.AddVariant("НЕ БЫСТРЕЕ", false);
            t.AddVariant("НЕ ВЫШЕ", false);
            t.AddVariant("НЕ ПОЗДНЕЕ", false);
            t.AddVariant("НЕ ДОЛЬШЕ", false);
            t.AddVariant("НЕ СТАРШЕ", false);
            t.AddVariant("НЕ ДОРОЖЕ", false);
            t.AddVariant("НЕ ЧАЩЕ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("БОЛЕЕ") { Tag = DiapTyp.Gt };
            t.AddVariant("БОЛЬШЕ", false);
            t.AddVariant("ДЛИННЕЕ", false);
            t.AddVariant("БЫСТРЕЕ", false);
            t.AddVariant("БОЛЕ", false);
            t.AddVariant("ЧАЩЕ", false);
            t.AddVariant("ГЛУБЖЕ", false);
            t.AddVariant("ВЫШЕ", false);
            t.AddVariant("СВЫШЕ", false);
            t.AddVariant("СТАРШЕ", false);
            t.AddVariant("ДОРОЖЕ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОТ") { Tag = DiapTyp.From };
            t.AddVariant("С", false);
            t.AddVariant("C", false);
            t.AddVariant("НАЧИНАЯ С", false);
            t.AddVariant("НАЧИНАЯ ОТ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДО") { Tag = DiapTyp.To };
            t.AddVariant("ПО", false);
            t.AddVariant("ЗАКАНЧИВАЯ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("НЕ ХУЖЕ") { Tag = DiapTyp.Undefined };
            m_Termins.Add(t);
            m_Spec = new Pullenti.Ner.Core.TerminCollection();
            t = new Pullenti.Ner.Core.Termin("ПОЛЛИТРА") { Tag = 0.5, Tag2 = "литр" };
            t.AddVariant("ПОЛУЛИТРА", false);
            m_Spec.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОЛКИЛО") { Tag = 0.5, Tag2 = "килограмм" };
            t.AddVariant("ПОЛКИЛОГРАММА", false);
            m_Spec.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОЛМЕТРА") { Tag = 0.5, Tag2 = "метр" };
            t.AddVariant("ПОЛУМЕТРА", false);
            m_Spec.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОЛТОННЫ") { Tag = 0.5, Tag2 = "тонна" };
            t.AddVariant("ПОЛУТОННЫ", false);
            m_Spec.Add(t);
            m_Spec.Add(t);
        }
        enum DiapTyp : int
        {
            Undefined,
            Ls,
            Le,
            Gt,
            Ge,
            From,
            To,
        }

    }
}