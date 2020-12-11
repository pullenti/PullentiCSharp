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

namespace Pullenti.Ner.Decree.Internal
{
    class DecreeChangeToken : Pullenti.Ner.MetaToken
    {
        public DecreeChangeToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public DecreeChangeTokenTyp Typ;
        public Pullenti.Ner.Decree.DecreeReferent Decree;
        public DecreeToken DecreeTok;
        public List<PartToken> Parts;
        public List<PartToken> NewParts;
        public Pullenti.Ner.Decree.DecreePartReferent RealPart;
        public Pullenti.Ner.Decree.DecreeChangeValueReferent ChangeVal;
        public bool HasName;
        public bool HasText;
        public Pullenti.Ner.Decree.DecreeChangeKind ActKind;
        public PartToken.ItemType PartTyp = PartToken.ItemType.Undefined;
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            tmp.Append(Typ.ToString());
            if (ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                tmp.AppendFormat(" Kind={0}", ActKind.ToString());
            if (HasName) 
                tmp.Append(" HasName");
            if (HasText) 
                tmp.Append(" HasText");
            if (Parts != null) 
            {
                foreach (PartToken p in Parts) 
                {
                    tmp.AppendFormat(" {0}", p);
                }
            }
            if (RealPart != null) 
                tmp.AppendFormat(" RealPart={0}", RealPart.ToString());
            if (NewParts != null) 
            {
                foreach (PartToken p in NewParts) 
                {
                    tmp.AppendFormat(" New={0}", p);
                }
            }
            if (PartTyp != PartToken.ItemType.Undefined) 
                tmp.AppendFormat(" PTyp={0}", PartTyp.ToString());
            if (DecreeTok != null) 
                tmp.AppendFormat(" DecTok={0}", DecreeTok.ToString());
            if (Decree != null) 
                tmp.AppendFormat(" Ref={0}", Decree.ToString(true, null, 0));
            if (ChangeVal != null) 
                tmp.AppendFormat(" ChangeVal={0}", ChangeVal.ToString(true, null, 0));
            return tmp.ToString();
        }
        public bool IsStart
        {
            get
            {
                return Typ == DecreeChangeTokenTyp.StartSingle || Typ == DecreeChangeTokenTyp.StartMultu || Typ == DecreeChangeTokenTyp.Single;
            }
        }
        public static DecreeChangeToken TryAttach(Pullenti.Ner.Token t, Pullenti.Ner.Decree.DecreeChangeReferent main = null, bool ignoreNewlines = false, List<Pullenti.Ner.Referent> changeStack = null, bool isInEdition = false)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token tt = t;
            if (t.IsNewlineBefore && !ignoreNewlines) 
            {
                for (tt = t; tt != null; tt = tt.Next) 
                {
                    if (tt == t && Pullenti.Ner.Core.BracketHelper.IsBracket(tt, false) && !tt.IsChar('(')) 
                        break;
                    else if ((tt == t && (tt is Pullenti.Ner.TextToken) && (((tt as Pullenti.Ner.TextToken).Term == "СТАТЬЯ" || (tt as Pullenti.Ner.TextToken).Term == "СТАТТЯ"))) && (tt.Next is Pullenti.Ner.NumberToken)) 
                    {
                        Pullenti.Ner.Token tt1 = tt.Next.Next;
                        if (tt1 != null && tt1.IsChar('.')) 
                        {
                            tt1 = tt1.Next;
                            if (tt1 != null && !tt1.IsNewlineBefore && tt1.IsValue("ВНЕСТИ", "УНЕСТИ")) 
                                continue;
                            if (tt1 != null && tt1.IsNewlineBefore) 
                                return null;
                            tt = tt1;
                        }
                        break;
                    }
                    else if (tt == t && PartToken.TryAttach(tt, null, false, false) != null) 
                        break;
                    else if ((tt is Pullenti.Ner.NumberToken) && (tt as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) 
                    {
                    }
                    else if (tt.IsHiphen) 
                    {
                    }
                    else if ((tt is Pullenti.Ner.TextToken) && !tt.Chars.IsLetter && !tt.IsWhitespaceBefore) 
                    {
                    }
                    else if (((tt is Pullenti.Ner.TextToken) && tt.LengthChar == 1 && (tt.Next is Pullenti.Ner.TextToken)) && !tt.Next.Chars.IsLetter) 
                    {
                    }
                    else 
                        break;
                }
            }
            if (tt == null) 
                return null;
            DecreeChangeToken res = null;
            if (((tt is Pullenti.Ner.TextToken) && t.IsNewlineBefore && !ignoreNewlines) && tt.IsValue("ВНЕСТИ", "УНЕСТИ") && ((((tt.Next != null && tt.Next.IsValue("В", "ДО"))) || (tt as Pullenti.Ner.TextToken).Term == "ВНЕСТИ" || (tt as Pullenti.Ner.TextToken).Term == "УНЕСТИ"))) 
            {
                res = new DecreeChangeToken(tt, tt) { Typ = DecreeChangeTokenTyp.StartMultu };
                if (tt.Next != null && tt.Next.IsValue("В", "ДО")) 
                    res.EndToken = (tt = tt.Next);
                bool hasChange = false;
                for (tt = tt.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsNewlineBefore) 
                        break;
                    if (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                    {
                        if (res.Decree != null && tt.GetReferent() != res.Decree) 
                            break;
                        res.Decree = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                        res.EndToken = tt;
                        continue;
                    }
                    List<PartToken> li = PartToken.TryAttachList(tt, false, 40);
                    if (li != null && li.Count > 0) 
                    {
                        res.Parts = li;
                        tt = (res.EndToken = li[li.Count - 1].EndToken);
                        continue;
                    }
                    if (tt.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            tt = br.EndToken;
                            continue;
                        }
                    }
                    if (tt.IsNewlineBefore) 
                        break;
                    res.EndToken = tt;
                    if (tt.IsChar(',') && hasChange) 
                    {
                        res.Typ = DecreeChangeTokenTyp.StartSingle;
                        break;
                    }
                    if (tt.IsValue("ИЗМЕНЕНИЕ", "ЗМІНА") || tt.IsValue("ДОПОЛНЕНИЕ", "ДОДАТОК")) 
                        hasChange = true;
                    else if (tt.IsValue("СЛЕДУЮЩИЙ", "НАСТУПНИЙ")) 
                    {
                    }
                    else if (tt.IsValue("ТАКОЙ", "ТАКИЙ")) 
                    {
                    }
                }
                if (!hasChange) 
                    return null;
                if (res.Decree == null) 
                    return null;
                tt = res.EndToken.Next;
                if (res.Typ == DecreeChangeTokenTyp.StartSingle && res.Parts == null && tt != null) 
                {
                    if ((tt.IsValue("ИЗЛОЖИВ", "ВИКЛАВШИ") || tt.IsValue("ДОПОЛНИВ", "ДОПОВНИВШИ") || tt.IsValue("ИСКЛЮЧИВ", "ВИКЛЮЧИВШИ")) || tt.IsValue("ЗАМЕНИВ", "ЗАМІНИВШИ")) 
                    {
                        tt = tt.Next;
                        if (tt != null && tt.Morph.Class.IsPreposition) 
                            tt = tt.Next;
                        res.Parts = PartToken.TryAttachList(tt, false, 40);
                        if (res.Parts != null) 
                        {
                            tt = res.EndToken.Next;
                            if (tt.IsValue("ДОПОЛНИВ", "ДОПОВНИВШИ")) 
                                res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                            else if (tt.IsValue("ИСКЛЮЧИВ", "ВИКЛЮЧИВШИ")) 
                                res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Remove;
                            else if (tt.IsValue("ИЗЛОЖИВ", "ВИКЛАВШИ")) 
                                res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.New;
                            else if (tt.IsValue("ЗАМЕНИВ", "ЗАМІНИВШИ")) 
                                res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Exchange;
                            res.EndToken = res.Parts[res.Parts.Count - 1];
                        }
                    }
                }
                return res;
            }
            if (((!ignoreNewlines && t.IsNewlineBefore && ((tt.IsValue("ПРИЗНАТЬ", "ВИЗНАТИ") || tt.IsValue("СЧИТАТЬ", "ВВАЖАТИ")))) && tt.Next != null && tt.Next.IsValue("УТРАТИТЬ", "ВТРАТИТИ")) && tt.Next.Next != null && tt.Next.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
            {
                res = new DecreeChangeToken(tt, tt.Next.Next) { Typ = DecreeChangeTokenTyp.Action, ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Expire };
                for (tt = tt.Next.Next.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsChar(':')) 
                    {
                        res.Typ = DecreeChangeTokenTyp.StartMultu;
                        res.EndToken = tt;
                        break;
                    }
                    if (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                    {
                        if (res.Decree != null) 
                            break;
                        res.Typ = DecreeChangeTokenTyp.StartSingle;
                        res.Decree = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                        res.EndToken = tt;
                        continue;
                    }
                    List<PartToken> li = PartToken.TryAttachList(tt, false, 40);
                    if (li != null && li.Count > 0) 
                    {
                        if (res.Parts != null) 
                            break;
                        res.Typ = DecreeChangeTokenTyp.StartSingle;
                        res.Parts = li;
                        tt = (res.EndToken = li[li.Count - 1].EndToken);
                        continue;
                    }
                    if (tt.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            tt = br.EndToken;
                            continue;
                        }
                    }
                    if (tt.IsNewlineBefore) 
                        break;
                }
                return res;
            }
            if ((!ignoreNewlines && ((t.IsNewlineBefore || tt == t)) && tt.IsValue("УТРАТИТЬ", "ВТРАТИТИ")) && tt.Next != null && tt.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
            {
                res = new DecreeChangeToken(tt, tt.Next) { Typ = DecreeChangeTokenTyp.Undefined };
                for (tt = tt.Next; tt != null; tt = tt.Next) 
                {
                    res.EndToken = tt;
                    if (tt.IsNewlineAfter) 
                        break;
                }
                return res;
            }
            if (!ignoreNewlines && t.IsNewlineBefore) 
            {
                if (tt.IsValue("СЛОВО", null)) 
                {
                }
                res = new DecreeChangeToken(tt, tt) { Typ = DecreeChangeTokenTyp.StartSingle };
                for (; tt != null; tt = tt.Next) 
                {
                    if (tt != t && tt.IsNewlineBefore) 
                        break;
                    if (tt.IsValue("К", null) || tt.IsValue("В", null) || tt.IsValue("ИЗ", null)) 
                        continue;
                    if (tt.IsValue("ПЕРЕЧЕНЬ", "ПЕРЕЛІК") && tt.Next != null && tt.Next.IsValue("ИЗМЕНЕНИЕ", "ЗМІНА")) 
                    {
                        if (tt == t) 
                            res.BeginToken = (res.EndToken = tt.Next);
                        tt = tt.Next.Next;
                        res.Typ = DecreeChangeTokenTyp.StartMultu;
                        if (tt != null && tt.IsChar(',')) 
                            tt = tt.Next;
                        if (tt != null && tt.IsValue("ВНОСИМЫЙ", "ВНЕСЕНИЙ")) 
                            tt = tt.Next;
                        if (tt == null) 
                            break;
                        continue;
                    }
                    if (tt.IsValue("НАИМЕНОВАНИЕ", "НАЙМЕНУВАННЯ") || tt.IsValue("НАЗВАНИЕ", "НАЗВА")) 
                    {
                        res.EndToken = tt;
                        if ((tt.Next != null && tt.Next.IsAnd && tt.Next.Next != null) && tt.Next.Next.IsValue("ТЕКСТ", null)) 
                        {
                            res.HasText = true;
                            res.EndToken = (tt = tt.Next.Next);
                        }
                        res.HasName = true;
                        continue;
                    }
                    if (tt.IsValue("ТЕКСТ", null)) 
                    {
                        PartToken pt = PartToken.TryAttach(tt.Next, null, false, true);
                        if (pt != null && pt.EndToken.Next != null && pt.EndToken.Next.IsValue("СЧИТАТЬ", "ВВАЖАТИ")) 
                        {
                            res.EndToken = pt.EndToken;
                            if (changeStack != null && changeStack.Count > 0 && (changeStack[0] is Pullenti.Ner.Decree.DecreePartReferent)) 
                                res.RealPart = changeStack[0] as Pullenti.Ner.Decree.DecreePartReferent;
                            res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Consider;
                            res.PartTyp = pt.Typ;
                            res.HasText = true;
                            return res;
                        }
                    }
                    if ((res.Parts == null && !res.HasName && tt.IsValue("ДОПОЛНИТЬ", "ДОПОВНИТИ")) && tt.Next != null) 
                    {
                        res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                        Pullenti.Ner.Token tt1 = DecreeToken.IsKeyword(tt.Next, false);
                        if (tt1 == null || tt1.Morph.Case.IsInstrumental) 
                            tt1 = tt.Next;
                        else 
                            tt1 = tt1.Next;
                        if (tt1 != null && tt1.IsValue("НОВЫЙ", "НОВИЙ")) 
                            tt1 = tt1.Next;
                        if (tt1 != null && tt1.Morph.Case.IsInstrumental) 
                        {
                            PartToken pt = PartToken.TryAttach(tt1, null, false, false);
                            if (pt == null) 
                                pt = PartToken.TryAttach(tt1, null, false, true);
                            if (pt != null && pt.Typ != PartToken.ItemType.Prefix) 
                            {
                                res.PartTyp = pt.Typ;
                                tt = (res.EndToken = pt.EndToken);
                                if (res.NewParts == null) 
                                    res.NewParts = new List<PartToken>();
                                res.NewParts.Add(pt);
                                if (tt.Next != null && tt.Next.IsAnd) 
                                {
                                    pt = PartToken.TryAttach(tt.Next.Next, null, false, false);
                                    if (pt == null) 
                                        pt = PartToken.TryAttach(tt.Next.Next, null, false, true);
                                    if (pt != null) 
                                    {
                                        res.NewParts.Add(pt);
                                        tt = (res.EndToken = pt.EndToken);
                                    }
                                }
                            }
                            continue;
                        }
                    }
                    List<PartToken> li = PartToken.TryAttachList(tt, false, 40);
                    if (li == null && tt.IsValue("ПРИМЕЧАНИЕ", "ПРИМІТКА")) 
                    {
                        li = new List<PartToken>();
                        li.Add(new PartToken(tt, tt) { Typ = PartToken.ItemType.Notice });
                    }
                    if (li != null && li.Count > 0 && li[0].Typ == PartToken.ItemType.Prefix) 
                        li = null;
                    if (li != null && li.Count > 0) 
                    {
                        if (li.Count == 1 && PartToken._getRank(li[0].Typ) > 0 && tt == t) 
                        {
                            if (li[0].IsNewlineAfter) 
                                return null;
                            if (li[0].EndToken.Next != null && li[0].EndToken.Next.IsChar('.')) 
                                return null;
                        }
                        if (res.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                        {
                            if (res.Parts != null) 
                                break;
                            res.Parts = li;
                        }
                        tt = (res.EndToken = li[li.Count - 1].EndToken);
                        continue;
                    }
                    if ((tt.Morph.Class.IsNoun && changeStack != null && changeStack.Count > 0) && (changeStack[0] is Pullenti.Ner.Decree.DecreePartReferent)) 
                    {
                        PartToken pa = PartToken.TryAttach(tt, null, false, true);
                        if (pa != null) 
                        {
                            if (changeStack[0].GetStringValue(PartToken._getAttrNameByTyp(pa.Typ)) != null) 
                            {
                                res.RealPart = changeStack[0] as Pullenti.Ner.Decree.DecreePartReferent;
                                res.EndToken = tt;
                                continue;
                            }
                        }
                    }
                    if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                    {
                        PartToken pa = PartToken.TryAttach(tt, null, false, true);
                        if (pa != null) 
                        {
                            if (res.NewParts == null) 
                                res.NewParts = new List<PartToken>();
                            res.NewParts.Add(pa);
                            res.EndToken = pa.EndToken;
                            continue;
                        }
                    }
                    if (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                    {
                        res.Decree = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                        res.EndToken = tt;
                        if (tt.Next != null && tt.Next.IsChar('(')) 
                        {
                            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br != null) 
                                res.EndToken = (tt = br.EndToken);
                        }
                        continue;
                    }
                    PartToken pt0 = PartToken.TryAttach(tt, null, false, true);
                    if (pt0 != null && ((res.HasName || pt0.Typ == PartToken.ItemType.Appendix)) && pt0.Typ != PartToken.ItemType.Prefix) 
                    {
                        tt = (res.EndToken = pt0.EndToken);
                        res.PartTyp = pt0.Typ;
                        if (pt0.Typ == PartToken.ItemType.Appendix && res.Parts == null) 
                        {
                            res.Parts = new List<PartToken>();
                            res.Parts.Add(pt0);
                        }
                        continue;
                    }
                    if (res.ChangeVal == null && !isInEdition) 
                    {
                        DecreeChangeToken res1 = null;
                        if (tt == res.BeginToken && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                        {
                        }
                        else 
                            res1 = TryAttach(tt, main, true, null, false);
                        if (res1 != null && res1.Typ == DecreeChangeTokenTyp.Value && res1.ChangeVal != null) 
                        {
                            res.ChangeVal = res1.ChangeVal;
                            if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                                res.ActKind = res1.ActKind;
                            tt = (res.EndToken = res1.EndToken);
                            if (tt.Next != null && tt.Next.IsValue("К", null)) 
                                tt = tt.Next;
                            continue;
                        }
                        if (tt.IsValue("ПОСЛЕ", "ПІСЛЯ")) 
                        {
                            pt0 = PartToken.TryAttach(tt.Next, null, true, false);
                            if (pt0 != null && pt0.Typ != PartToken.ItemType.Prefix) 
                            {
                                if (res.Parts == null) 
                                {
                                    res.Parts = new List<PartToken>();
                                    res.Parts.Add(pt0);
                                }
                                tt = (res.EndToken = pt0.EndToken);
                                continue;
                            }
                        }
                        if (tt.IsValue("ТЕКСТ", null) && tt.Previous != null && tt.Previous.IsValue("В", "У")) 
                            continue;
                        if (tt.IsValue("ИЗМЕНЕНИЕ", "ЗМІНА")) 
                        {
                            res.EndToken = tt;
                            continue;
                        }
                    }
                    if (tt != t && ((res.HasName || res.Parts != null)) && res.Decree == null) 
                    {
                        List<DecreeToken> dts = DecreeToken.TryAttachList(tt, null, 10, false);
                        if (dts != null && dts.Count > 0 && dts[0].Typ == DecreeToken.ItemType.Typ) 
                        {
                            tt = (res.EndToken = dts[dts.Count - 1].EndToken);
                            if (main != null && res.Decree == null && res.DecreeTok == null) 
                            {
                                Pullenti.Ner.Decree.DecreeReferent dec = null;
                                foreach (Pullenti.Ner.Referent v in main.Owners) 
                                {
                                    if (v is Pullenti.Ner.Decree.DecreeReferent) 
                                    {
                                        dec = v as Pullenti.Ner.Decree.DecreeReferent;
                                        break;
                                    }
                                    else if (v is Pullenti.Ner.Decree.DecreePartReferent) 
                                    {
                                        dec = (v as Pullenti.Ner.Decree.DecreePartReferent).Owner;
                                        if (dec != null) 
                                            break;
                                    }
                                }
                                if (dec != null && dec.Typ0 == dts[0].Value) 
                                {
                                    res.Decree = dec;
                                    res.DecreeTok = dts[0];
                                }
                            }
                            continue;
                        }
                    }
                    if (tt == res.BeginToken && main != null) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null) 
                        {
                            Pullenti.Ner.Token tt1 = npt.EndToken.Next;
                            if ((tt1 != null && tt1.IsValue("ИЗЛОЖИТЬ", "ВИКЛАСТИ") && tt1.Next != null) && tt1.Next.IsValue("В", null)) 
                            {
                                PartToken pt = new PartToken(tt, npt.EndToken) { Typ = PartToken.ItemType.Appendix };
                                pt.Name = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                                res.Parts = new List<PartToken>();
                                res.Parts.Add(pt);
                                res.EndToken = pt.EndToken;
                                break;
                            }
                        }
                    }
                    Pullenti.Ner.Token ttt = DecreeToken.IsKeyword(tt, false);
                    if (ttt != null && res.Parts == null) 
                    {
                        Pullenti.Ner.Token ttt0 = ttt;
                        for (; ttt != null; ttt = ttt.Next) 
                        {
                            if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(ttt)) 
                                break;
                            if (ttt.IsChar('(') && ttt.Next != null && ttt.Next.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
                            {
                                if (ttt.IsNewlineBefore) 
                                    break;
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(ttt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br == null) 
                                    break;
                                PartToken pt = PartToken.TryAttach(ttt.Next, null, false, false);
                                if (pt == null) 
                                    PartToken.TryAttach(ttt.Next, null, false, true);
                                if (pt != null) 
                                {
                                    res.Parts = new List<PartToken>();
                                    res.Parts.Add(pt);
                                    tt = (res.EndToken = br.EndToken);
                                    break;
                                }
                            }
                        }
                        if (res.Parts != null) 
                            continue;
                        if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                        {
                            tt = (res.EndToken = ttt0);
                            continue;
                        }
                        tt = ttt0;
                        continue;
                    }
                    break;
                }
                if (((res.HasName || res.Parts != null || res.Decree != null) || res.RealPart != null || res.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) || res.ChangeVal != null) 
                {
                    if (res.EndToken.Next != null && res.EndToken.Next.IsChar(':') && res.EndToken.Next.IsNewlineAfter) 
                    {
                        res.Typ = DecreeChangeTokenTyp.Single;
                        res.EndToken = res.EndToken.Next;
                    }
                    return res;
                }
                if (res.BeginToken == tt) 
                {
                    Pullenti.Ner.Core.TerminToken tok1 = m_Terms.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok1 != null) 
                    {
                    }
                    else 
                        return null;
                }
                else 
                    return null;
            }
            Pullenti.Ner.Core.TerminToken tok = m_Terms.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tt.Morph.Class.IsAdjective && (((tt is Pullenti.Ner.NumberToken) || tt.IsValue("ПОСЛЕДНИЙ", "ОСТАННІЙ") || tt.IsValue("ПРЕДПОСЛЕДНИЙ", "ПЕРЕДОСТАННІЙ")))) 
            {
                tok = m_Terms.TryParse(tt.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null && (tok.Termin.Tag is Pullenti.Ner.Decree.DecreeChangeValueKind)) 
                {
                }
                else 
                    tok = null;
            }
            if (tok != null) 
            {
                if (tok.Termin.Tag is Pullenti.Ner.Decree.DecreeChangeKind) 
                {
                    res = new DecreeChangeToken(tt, tok.EndToken) { Typ = DecreeChangeTokenTyp.Action, ActKind = (Pullenti.Ner.Decree.DecreeChangeKind)tok.Termin.Tag };
                    if (((res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append || res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Consider)) && tok.EndToken.Next != null && tok.EndToken.Next.Morph.Case.IsInstrumental) 
                    {
                        PartToken pt = PartToken.TryAttach(tok.EndToken.Next, null, false, false);
                        if (pt == null) 
                            pt = PartToken.TryAttach(tok.EndToken.Next, null, false, true);
                        if (pt != null && pt.Typ != PartToken.ItemType.Prefix) 
                        {
                            if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                            {
                                res.PartTyp = pt.Typ;
                                if (res.NewParts == null) 
                                    res.NewParts = new List<PartToken>();
                                res.NewParts.Add(pt);
                            }
                            else if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Consider) 
                            {
                                res.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent();
                                res.ChangeVal.Value = pt.GetSourceText();
                            }
                            tt = (res.EndToken = pt.EndToken);
                            if (tt.Next != null && tt.Next.IsAnd && res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                            {
                                pt = PartToken.TryAttach(tt.Next.Next, null, false, false);
                                if (pt == null) 
                                    pt = PartToken.TryAttach(tt.Next.Next, null, false, true);
                                if (pt != null) 
                                {
                                    res.NewParts.Add(pt);
                                    tt = (res.EndToken = pt.EndToken);
                                }
                            }
                        }
                    }
                    return res;
                }
                if (tok.Termin.Tag is Pullenti.Ner.Decree.DecreeChangeValueKind) 
                {
                    res = new DecreeChangeToken(tt, tok.EndToken) { Typ = DecreeChangeTokenTyp.Value };
                    res.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent();
                    res.ChangeVal.Kind = (Pullenti.Ner.Decree.DecreeChangeValueKind)tok.Termin.Tag;
                    tt = tok.EndToken.Next;
                    if (tt == null) 
                        return null;
                    if (res.ChangeVal.Kind == Pullenti.Ner.Decree.DecreeChangeValueKind.Sequence || res.ChangeVal.Kind == Pullenti.Ner.Decree.DecreeChangeValueKind.Footnote) 
                    {
                        if (tt is Pullenti.Ner.NumberToken) 
                        {
                            res.ChangeVal.Number = (tt as Pullenti.Ner.NumberToken).Value.ToString();
                            res.EndToken = tt;
                            tt = tt.Next;
                        }
                        else if (res.BeginToken is Pullenti.Ner.NumberToken) 
                            res.ChangeVal.Number = (res.BeginToken as Pullenti.Ner.NumberToken).Value.ToString();
                        else if (res.BeginToken.Morph.Class.IsAdjective) 
                            res.ChangeVal.Number = res.BeginToken.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                        else if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false) && (tt.Next is Pullenti.Ner.NumberToken) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt.Next.Next, false, null, false)) 
                        {
                            res.ChangeVal.Number = (tt.Next as Pullenti.Ner.NumberToken).Value.ToString();
                            res.EndToken = (tt = tt.Next.Next);
                            tt = tt.Next;
                        }
                    }
                    if (tt != null && tt.IsValue("ИЗЛОЖИТЬ", "ВИКЛАСТИ") && res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                    {
                        res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.New;
                        tt = tt.Next;
                        if (tt != null && tt.IsValue("В", null)) 
                            tt = tt.Next;
                    }
                    if ((tt != null && ((tt.IsValue("СЛЕДУЮЩИЙ", "НАСТУПНИЙ") || tt.IsValue("ТАКОЙ", "ТАКИЙ"))) && tt.Next != null) && ((tt.Next.IsValue("СОДЕРЖАНИЕ", "ЗМІСТ") || tt.Next.IsValue("СОДЕРЖИМОЕ", "ВМІСТ") || tt.Next.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")))) 
                        tt = tt.Next.Next;
                    else if (tt != null && tt.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                        tt = tt.Next;
                    if (tt != null && tt.IsChar(':')) 
                        tt = tt.Next;
                    bool canBeStart = false;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false)) 
                        canBeStart = true;
                    else if ((tt is Pullenti.Ner.MetaToken) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence((tt as Pullenti.Ner.MetaToken).BeginToken, true, false)) 
                        canBeStart = true;
                    else if (tt != null && tt.IsNewlineBefore && tt.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
                    {
                        if ((tt.Previous != null && tt.Previous.IsChar(':') && tt.Previous.Previous != null) && tt.Previous.Previous.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                            canBeStart = true;
                    }
                    if (canBeStart) 
                    {
                        for (Pullenti.Ner.Token ttt = (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false) ? tt.Next : tt); ttt != null; ttt = ttt.Next) 
                        {
                            if (ttt.IsCharOf(".;") && ttt.IsNewlineAfter) 
                            {
                                res.ChangeVal.Value = (new Pullenti.Ner.MetaToken(tt.Next, ttt.Previous)).GetSourceText();
                                res.EndToken = ttt;
                                break;
                            }
                            if (Pullenti.Ner.Core.BracketHelper.IsBracket(ttt, true)) 
                            {
                            }
                            else if ((ttt is Pullenti.Ner.MetaToken) && Pullenti.Ner.Core.BracketHelper.IsBracket((ttt as Pullenti.Ner.MetaToken).EndToken, true)) 
                            {
                            }
                            else 
                                continue;
                            if (ttt.Next == null || ttt.IsNewlineAfter) 
                            {
                            }
                            else if (ttt.Next.IsCharOf(".;") && ttt.Next.IsNewlineAfter) 
                            {
                            }
                            else if (ttt.Next.IsCommaAnd && TryAttach(ttt.Next.Next, main, false, changeStack, true) != null) 
                            {
                            }
                            else if (TryAttach(ttt.Next, main, false, changeStack, true) != null || m_Terms.TryParse(ttt.Next, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                            {
                            }
                            else 
                                continue;
                            string val = (new Pullenti.Ner.MetaToken((Pullenti.Ner.Core.BracketHelper.IsBracket(tt, true) ? tt.Next : tt), (Pullenti.Ner.Core.BracketHelper.IsBracket(ttt, true) ? ttt.Previous : ttt))).GetSourceText();
                            res.EndToken = ttt;
                            if (!Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false)) 
                                val = val.Substring(1);
                            if (!Pullenti.Ner.Core.BracketHelper.IsBracket(ttt, true)) 
                                val = val.Substring(0, val.Length - 1);
                            res.ChangeVal.Value = val;
                            break;
                        }
                        if (res.ChangeVal.Value == null) 
                            return null;
                        if (res.ChangeVal.Kind == Pullenti.Ner.Decree.DecreeChangeValueKind.Words) 
                        {
                            tok = m_Terms.TryParse(res.EndToken.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                            if (tok != null && (tok.Termin.Tag is Pullenti.Ner.Decree.DecreeChangeValueKind) && ((Pullenti.Ner.Decree.DecreeChangeValueKind)tok.Termin.Tag) == Pullenti.Ner.Decree.DecreeChangeValueKind.RobustWords) 
                            {
                                res.ChangeVal.Kind = Pullenti.Ner.Decree.DecreeChangeValueKind.RobustWords;
                                res.EndToken = tok.EndToken;
                            }
                        }
                    }
                    return res;
                }
            }
            int isNexChange = 0;
            if (t != null && t.IsValue("В", "У") && t.Next != null) 
            {
                t = t.Next;
                if (t.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ") && t.Next != null) 
                {
                    isNexChange = 1;
                    t = t.Next;
                }
            }
            if (((t.IsValue("СЛЕДУЮЩИЙ", "НАСТУПНИЙ") || tt.IsValue("ТАКОЙ", "ТАКИЙ"))) && t.Next != null && ((t.Next.IsValue("СОДЕРЖАНИЕ", "ЗМІСТ") || t.Next.IsValue("СОДЕРЖИМОЕ", "ВМІСТ") || t.Next.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")))) 
            {
                isNexChange = 2;
                t = t.Next.Next;
            }
            if (t.IsChar(':') && t.Next != null) 
            {
                if (t.Previous != null && t.Previous.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                    isNexChange++;
                tt = (t = t.Next);
                if (isNexChange > 0) 
                    isNexChange++;
            }
            if ((t == tt && t.Previous != null && t.Previous.IsChar(':')) && Pullenti.Ner.Core.BracketHelper.IsBracket(t, false) && !t.IsChar('(')) 
                isNexChange = 1;
            if (((isNexChange > 0 && Pullenti.Ner.Core.BracketHelper.IsBracket(t, true))) || ((isNexChange > 1 && t.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")))) 
            {
                res = new DecreeChangeToken(t, t) { Typ = DecreeChangeTokenTyp.Value };
                res.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent() { Kind = Pullenti.Ner.Decree.DecreeChangeValueKind.Text };
                if (isInEdition) 
                    return res;
                Pullenti.Ner.Token t0 = (Pullenti.Ner.Core.BracketHelper.IsBracket(t, true) ? t.Next : t);
                Pullenti.Ner.Token doubt1 = null;
                Pullenti.Ner.Instrument.Internal.InstrToken1 clauseLast = null;
                for (tt = t.Next; tt != null; tt = tt.Next) 
                {
                    if (!tt.IsNewlineAfter) 
                        continue;
                    bool isDoubt = false;
                    Pullenti.Ner.Instrument.Internal.InstrToken1 instr = Pullenti.Ner.Instrument.Internal.InstrToken1.Parse(tt.Next, true, null, 0, null, false, 0, false, false);
                    DecreeChangeToken dcNext = TryAttach(tt.Next, null, false, null, true);
                    if (dcNext == null) 
                        dcNext = TryAttach(tt.Next, null, true, null, true);
                    if (tt.Next == null) 
                    {
                    }
                    else if (dcNext != null && ((dcNext.IsStart || dcNext.ChangeVal != null || dcNext.Typ == DecreeChangeTokenTyp.Undefined))) 
                    {
                    }
                    else 
                    {
                        isDoubt = true;
                        PartToken pt = PartToken.TryAttach(tt.Next, null, false, false);
                        if (pt != null && pt.Typ == PartToken.ItemType.Clause && ((pt.IsNewlineAfter || ((pt.EndToken.Next != null && pt.EndToken.Next.IsChar('.')))))) 
                        {
                            isDoubt = false;
                            if (clauseLast != null && instr != null && Pullenti.Ner.Instrument.Internal.NumberingHelper.CalcDelta(clauseLast, instr, true) == 1) 
                                isDoubt = true;
                        }
                    }
                    if (instr != null && instr.Typ == Pullenti.Ner.Instrument.Internal.InstrToken1.Types.Clause) 
                        clauseLast = instr;
                    if (isDoubt && instr != null) 
                    {
                        for (Pullenti.Ner.Token ttt = tt; ttt != null && ttt.EndChar <= instr.EndChar; ttt = ttt.Next) 
                        {
                            if (ttt.IsValue("УТРАТИТЬ", "ВТРАТИТИ") && ttt.Next != null && ttt.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
                            {
                                isDoubt = false;
                                break;
                            }
                        }
                    }
                    res.EndToken = tt;
                    Pullenti.Ner.Token tt1 = tt;
                    if (tt1.IsCharOf(";.")) 
                        tt1 = (res.EndToken = tt1.Previous);
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt1, true)) 
                        tt1 = tt1.Previous;
                    else if ((tt1 is Pullenti.Ner.MetaToken) && Pullenti.Ner.Core.BracketHelper.IsBracket((tt1 as Pullenti.Ner.MetaToken).EndToken, true)) 
                    {
                    }
                    else 
                        continue;
                    if (isDoubt) 
                    {
                        if (doubt1 == null) 
                            doubt1 = tt1;
                        continue;
                    }
                    if (tt1.BeginChar > t.EndChar) 
                    {
                        res.ChangeVal.Value = (new Pullenti.Ner.MetaToken(t0, tt1)).GetSourceText();
                        return res;
                    }
                    break;
                }
                if (doubt1 != null) 
                {
                    res.ChangeVal.Value = (new Pullenti.Ner.MetaToken(t0, doubt1)).GetSourceText();
                    res.EndToken = doubt1;
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(doubt1.Next, true)) 
                        res.EndToken = doubt1.Next;
                    return res;
                }
                return null;
            }
            if (t.IsValue("ПОСЛЕ", "ПІСЛЯ")) 
            {
                res = TryAttach(t.Next, null, false, null, false);
                if (res != null && res.Typ == DecreeChangeTokenTyp.Value) 
                {
                    res.Typ = DecreeChangeTokenTyp.AfterValue;
                    res.BeginToken = t;
                    return res;
                }
            }
            return null;
        }
        static List<DecreeChangeToken> TryAttachList(Pullenti.Ner.Token t)
        {
            if (t == null || t.IsNewlineBefore) 
                return null;
            DecreeChangeToken d0 = TryAttach(t, null, false, null, false);
            if (d0 == null) 
                return null;
            List<DecreeChangeToken> res = new List<DecreeChangeToken>();
            res.Add(d0);
            t = d0.EndToken.Next;
            for (; t != null; t = t.Next) 
            {
                if (t.IsNewlineBefore) 
                {
                    if ((t.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК") && t.Previous != null && t.Previous.IsChar(':')) && t.Previous.Previous != null && t.Previous.Previous.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                    {
                    }
                    else 
                        break;
                }
                DecreeChangeToken d = TryAttach(t, null, false, null, false);
                if (d == null && t.IsChar('.') && !t.IsNewlineAfter) 
                    continue;
                if (d == null) 
                {
                    if (t.IsValue("НОВЫЙ", "НОВИЙ")) 
                        continue;
                    if (t.IsValue("НА", null)) 
                        continue;
                    if (t.IsChar(':') && ((!t.IsNewlineAfter || res[res.Count - 1].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.New))) 
                        continue;
                    if ((t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).Term == "ТЕКСТОМ") 
                        continue;
                    List<PartToken> pts = PartToken.TryAttachList(t, false, 40);
                    if (pts != null) 
                        d = new DecreeChangeToken(pts[0].BeginToken, pts[pts.Count - 1].EndToken) { Typ = DecreeChangeTokenTyp.Undefined, Parts = pts };
                    else 
                    {
                        PartToken pt = PartToken.TryAttach(t, null, true, false);
                        if (pt == null) 
                            pt = PartToken.TryAttach(t, null, true, true);
                        if (pt != null) 
                        {
                            d = new DecreeChangeToken(pt.BeginToken, pt.EndToken);
                            if (t.Previous != null && t.Previous.IsValue("НОВЫЙ", "НОВИЙ")) 
                            {
                                d.NewParts = new List<PartToken>();
                                d.NewParts.Add(pt);
                            }
                            else 
                                d.PartTyp = pt.Typ;
                        }
                    }
                }
                if (d == null) 
                    break;
                if (d.Typ == DecreeChangeTokenTyp.Single || d.Typ == DecreeChangeTokenTyp.StartMultu || d.Typ == DecreeChangeTokenTyp.StartSingle) 
                    break;
                res.Add(d);
                t = d.EndToken;
            }
            return res;
        }
        static Pullenti.Ner.Core.TerminCollection m_Terms;
        internal static void Initialize()
        {
            if (m_Terms != null) 
                return;
            m_Terms = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("ИЗЛОЖИТЬ В СЛЕДУЮЩЕЙ РЕДАКЦИИ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.New };
            t.AddVariant("ИЗЛОЖИВ ЕГО В СЛЕДУЮЩЕЙ РЕДАКЦИИ", false);
            t.AddVariant("ИЗЛОЖИТЬ В РЕДАКЦИИ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВИКЛАСТИ В НАСТУПНІЙ РЕДАКЦІЇ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.New };
            t.AddVariant("ВИКЛАВШИ В ТАКІЙ РЕДАКЦІЇ", false);
            t.AddVariant("ВИКЛАВШИ ЙОГО В НАСТУПНІЙ РЕДАКЦІЇ", false);
            t.AddVariant("ВИКЛАСТИ В РЕДАКЦІЇ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРИЗНАТЬ УТРАТИВШИМ СИЛУ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Expire };
            t.AddVariant("СЧИТАТЬ УТРАТИВШИМ СИЛУ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВИЗНАТИ таким, що ВТРАТИВ ЧИННІСТЬ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Expire };
            t.AddVariant("ВВАЖАТИ таким, що ВТРАТИВ ЧИННІСТЬ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ИСКЛЮЧИТЬ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Remove };
            t.AddVariant("ИСКЛЮЧИВ ИЗ НЕГО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВИКЛЮЧИТИ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Remove };
            t.AddVariant("ВИКЛЮЧИВШИ З НЬОГО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЧИТАТЬ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Consider };
            t.AddVariant("СЧИТАТЬ СООТВЕТСТВЕННО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВВАЖАТИ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Consider };
            t.AddVariant("ВВАЖАТИ ВІДПОВІДНО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗАМЕНИТЬ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Exchange };
            t.AddVariant("ЗАМЕНИВ В НЕМ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗАМІНИТИ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Exchange };
            t.AddVariant("ЗАМІНИВШИ В НЬОМУ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОПОЛНИТЬ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Append };
            t.AddVariant("ДОПОЛНИВ ЕГО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОПОВНИТИ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Append };
            t.AddVariant("ДОПОВНИВШИ ЙОГО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЛОВО") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Words };
            t.AddVariant("АББРЕВИАТУРА", false);
            t.AddVariant("АБРЕВІАТУРА", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЦИФРА") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Numbers };
            t.AddVariant("ЧИСЛО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРЕДЛОЖЕНИЕ") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Sequence };
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОПОЗИЦІЯ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Sequence };
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("СНОСКА") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Footnote };
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВИНОСКА", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Footnote };
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЛОК") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Block };
            t.AddVariant("БЛОК СО СЛОВАМИ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЛОК", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Block };
            t.AddVariant("БЛОК ЗІ СЛОВАМИ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("В СООТВЕТСТВУЮЩИХ ЧИСЛЕ И ПАДЕЖЕ") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.RobustWords };
            t.AddVariant("В СООТВЕТСТВУЮЩЕМ ПАДЕЖЕ", false);
            t.AddVariant("В СООТВЕТСТВУЮЩЕМ ЧИСЛЕ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("У ВІДПОВІДНОМУ ЧИСЛІ ТА ВІДМІНКУ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.RobustWords };
            t.AddVariant("У ВІДПОВІДНОМУ ВІДМІНКУ", false);
            t.AddVariant("У ВІДПОВІДНОМУ ЧИСЛІ", false);
            m_Terms.Add(t);
        }
        public static List<Pullenti.Ner.ReferentToken> AttachReferents(Pullenti.Ner.Referent dpr, DecreeChangeToken tok0)
        {
            if (dpr == null || tok0 == null) 
                return null;
            Pullenti.Ner.Token tt0 = tok0.EndToken.Next;
            if (tt0 != null && tt0.IsCommaAnd && tok0.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                tt0 = tt0.Next;
            if (tt0 != null && tt0.IsChar(':')) 
                tt0 = tt0.Next;
            List<DecreeChangeToken> toks = TryAttachList(tt0);
            if (toks == null) 
                toks = new List<DecreeChangeToken>();
            toks.Insert(0, tok0);
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            Pullenti.Ner.Decree.DecreeChangeReferent dcr = new Pullenti.Ner.Decree.DecreeChangeReferent();
            dcr.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_OWNER, dpr, false, 0);
            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dcr, tok0.BeginToken, tok0.EndToken);
            res.Add(rt);
            List<string> newItems = null;
            while (true) 
            {
                for (int i = 0; i < toks.Count; i++) 
                {
                    DecreeChangeToken tok = toks[i];
                    if (tok.HasText && tok.HasName) 
                        dcr.IsOwnerNameAndText = true;
                    else if (tok.HasName) 
                        dcr.IsOwnerName = true;
                    else if (tok.HasText) 
                        dcr.IsOnlyText = true;
                    rt.EndToken = tok.EndToken;
                    if (tok.Typ == DecreeChangeTokenTyp.AfterValue) 
                    {
                        if (tok.ChangeVal != null) 
                        {
                            dcr.Param = tok.ChangeVal;
                            if (tok.EndChar > rt.EndChar) 
                                rt.EndToken = tok.EndToken;
                            res.Insert(res.Count - 1, new Pullenti.Ner.ReferentToken(tok.ChangeVal, tok.BeginToken, tok.EndToken));
                        }
                        continue;
                    }
                    if (tok.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                    {
                        dcr.Kind = tok.ActKind;
                        if (tok.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Expire) 
                            break;
                    }
                    if (tok.ChangeVal != null) 
                    {
                        if (((i + 2) < toks.Count) && ((toks[i + 1].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Exchange || toks[i + 1].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.New)) && toks[i + 2].ChangeVal != null) 
                        {
                            dcr.Param = tok.ChangeVal;
                            Pullenti.Ner.ReferentToken rt11 = new Pullenti.Ner.ReferentToken(tok.ChangeVal, tok.BeginToken, tok.EndToken);
                            if (tok.Parts != null && tok.Parts.Count > 0) 
                                rt11.BeginToken = tok.Parts[tok.Parts.Count - 1].EndToken.Next;
                            res.Insert(res.Count - 1, rt11);
                            dcr.Value = toks[i + 2].ChangeVal;
                            dcr.Kind = toks[i + 1].ActKind;
                            i += 2;
                            tok = toks[i];
                        }
                        else if (((i + 1) < toks.Count) && toks[i + 1].ChangeVal != null && dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Exchange) 
                        {
                            dcr.Param = tok.ChangeVal;
                            res.Insert(res.Count - 1, new Pullenti.Ner.ReferentToken(tok.ChangeVal, tok.BeginToken, tok.EndToken));
                            dcr.Value = toks[i + 1].ChangeVal;
                            i += 1;
                            tok = toks[i];
                        }
                        else if (dcr.Value == null) 
                            dcr.Value = tok.ChangeVal;
                        else if ((dcr.Value.Kind != Pullenti.Ner.Decree.DecreeChangeValueKind.Text && tok.ChangeVal.Kind == Pullenti.Ner.Decree.DecreeChangeValueKind.Text && tok.ChangeVal.Value != null) && dcr.Value.Value == null) 
                            dcr.Value.Value = tok.ChangeVal.Value;
                        else 
                            dcr.Value = tok.ChangeVal;
                        if (tok.EndChar > rt.EndChar) 
                            rt.EndToken = tok.EndToken;
                        res.Insert(res.Count - 1, new Pullenti.Ner.ReferentToken(tok.ChangeVal, tok.BeginToken, tok.EndToken));
                        if (dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Consider || dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.New) 
                            break;
                    }
                    if (dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Append && tok.NewParts != null) 
                    {
                        foreach (PartToken np in tok.NewParts) 
                        {
                            int rank = PartToken._getRank(np.Typ);
                            if (rank == 0) 
                                continue;
                            string eqLevVal = null;
                            if (dpr is Pullenti.Ner.Decree.DecreePartReferent) 
                            {
                                if (!(dpr as Pullenti.Ner.Decree.DecreePartReferent).IsAllItemsOverThisLevel(np.Typ)) 
                                {
                                    eqLevVal = dpr.GetStringValue(PartToken._getAttrNameByTyp(np.Typ));
                                    if (eqLevVal == null) 
                                        continue;
                                }
                            }
                            dcr.Kind = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                            if (newItems == null) 
                                newItems = new List<string>();
                            string nam = PartToken._getAttrNameByTyp(np.Typ);
                            if (nam == null) 
                                continue;
                            if (np.Values.Count == 0) 
                            {
                                if (eqLevVal == null) 
                                    newItems.Add(nam);
                                else 
                                {
                                    int n;
                                    if (int.TryParse(eqLevVal, out n)) 
                                        newItems.Add(string.Format("{0} {1}", nam, n + 1));
                                    else 
                                        newItems.Add(nam);
                                }
                            }
                            else if (np.Values.Count == 2 && np.Values[0].EndToken.Next.IsHiphen) 
                            {
                                List<string> vv = Pullenti.Ner.Instrument.Internal.NumberingHelper.CreateDiap(np.Values[0].Value, np.Values[1].Value);
                                if (vv != null) 
                                {
                                    foreach (string v in vv) 
                                    {
                                        newItems.Add(string.Format("{0} {1}", nam, v));
                                    }
                                }
                            }
                            if (newItems.Count == 0) 
                            {
                                foreach (PartToken.PartValue v in np.Values) 
                                {
                                    newItems.Add(string.Format("{0} {1}", nam, v.Value));
                                }
                            }
                        }
                    }
                }
                if (!dcr.CheckCorrect()) 
                    return null;
                if (newItems != null && dcr.Value != null && dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                {
                    foreach (string v in newItems) 
                    {
                        dcr.Value.AddSlot(Pullenti.Ner.Decree.DecreeChangeValueReferent.ATTR_NEWITEM, v, false, 0);
                    }
                }
                newItems = null;
                if (rt.EndToken.Next == null || !rt.EndToken.Next.IsComma) 
                    break;
                toks = TryAttachList(rt.EndToken.Next.Next);
                if (toks == null) 
                    break;
                Pullenti.Ner.Decree.DecreeChangeReferent dts1 = new Pullenti.Ner.Decree.DecreeChangeReferent();
                foreach (Pullenti.Ner.Referent o in dcr.Owners) 
                {
                    dts1.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_OWNER, o, false, 0);
                }
                rt = new Pullenti.Ner.ReferentToken(dts1, toks[0].BeginToken, toks[0].EndToken);
                res.Add(rt);
                dcr = dts1;
            }
            return res;
        }
    }
}