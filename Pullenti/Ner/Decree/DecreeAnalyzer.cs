/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Decree
{
    /// <summary>
    /// Анализатор ссылок на НПА
    /// </summary>
    public class DecreeAnalyzer : Pullenti.Ner.Analyzer
    {
        internal static List<Pullenti.Ner.MetaToken> TryAttachParts(List<Pullenti.Ner.Decree.Internal.PartToken> parts, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp, Pullenti.Ner.Referent _defOwner)
        {
            if (parts == null || parts.Count == 0) 
                return null;
            int i;
            int j;
            Pullenti.Ner.Token tt = parts[parts.Count - 1].EndToken.Next;
            if (_defOwner != null && tt != null) 
            {
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null && br.EndToken.Next != null) 
                        tt = br.EndToken.Next;
                }
                if (tt.GetReferent() is DecreeReferent) 
                    _defOwner = null;
                else if (tt.IsValue("К", null) && tt.Next != null && (tt.Next.GetReferent() is DecreeReferent)) 
                    _defOwner = null;
            }
            if ((parts.Count == 1 && parts[0].IsNewlineBefore && parts[0].BeginToken.Chars.IsLetter) && !parts[0].BeginToken.Chars.IsAllLower) 
            {
                Pullenti.Ner.Token t1 = parts[0].EndToken.Next;
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                    t1 = br.EndToken.Next;
                if (t1 != null && (t1.GetReferent() is DecreeReferent) && !parts[0].IsNewlineAfter) 
                {
                }
                else 
                {
                    Pullenti.Ner.Instrument.Internal.InstrToken1 li = Pullenti.Ner.Instrument.Internal.InstrToken1.Parse(parts[0].BeginToken, true, null, 0, null, false, 0, false, false);
                    if (li != null && li.HasVerb) 
                    {
                        if ((parts.Count == 1 && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part && parts[0].ToString().Contains("резолют")) && parts[0].IsNewlineBefore) 
                            return null;
                    }
                    else 
                        return null;
                }
            }
            ThisDecree thisDec = null;
            bool isProgram = false;
            bool isAddAgree = false;
            if (parts[parts.Count - 1].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram && parts[parts.Count - 1].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.AddAgree) 
            {
                thisDec = ThisDecree.TryAttach(parts[parts.Count - 1], baseTyp);
                if (thisDec != null) 
                {
                    if ((_defOwner is DecreeReferent) && (((_defOwner as DecreeReferent).Typ0 == thisDec.Typ || parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix))) 
                    {
                    }
                    else 
                        _defOwner = null;
                }
                if (thisDec == null && _defOwner == null) 
                    thisDec = ThisDecree.TryAttachBack(parts[0].BeginToken, baseTyp);
                if (thisDec == null) 
                {
                    foreach (Pullenti.Ner.Decree.Internal.PartToken p in parts) 
                    {
                        if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part) 
                        {
                            bool hasClause = false;
                            foreach (Pullenti.Ner.Decree.Internal.PartToken pp in parts) 
                            {
                                if (pp != p) 
                                {
                                    if (Pullenti.Ner.Decree.Internal.PartToken._getRank(pp.Typ) >= Pullenti.Ner.Decree.Internal.PartToken._getRank(Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause)) 
                                        hasClause = true;
                                }
                            }
                            if (_defOwner is DecreePartReferent) 
                            {
                                if ((_defOwner as DecreePartReferent).Clause != null) 
                                    hasClause = true;
                            }
                            if (!hasClause) 
                                p.Typ = Pullenti.Ner.Decree.Internal.PartToken.ItemType.DocPart;
                            else if ((((p == parts[parts.Count - 1] && p.EndToken.Next != null && p.Values.Count == 1) && (p.EndToken.Next.GetReferent() is DecreeReferent) && (p.BeginToken is Pullenti.Ner.TextToken)) && (p.BeginToken as Pullenti.Ner.TextToken).Term == "ЧАСТИ" && (p.EndToken is Pullenti.Ner.NumberToken)) && p.BeginToken.Next == p.EndToken) 
                                p.Typ = Pullenti.Ner.Decree.Internal.PartToken.ItemType.DocPart;
                        }
                    }
                }
            }
            else if (parts[parts.Count - 1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.AddAgree) 
                isAddAgree = true;
            else 
            {
                if (parts.Count > 1) 
                    parts.RemoveRange(0, parts.Count - 1);
                isProgram = true;
            }
            DecreeReferent defOwner = _defOwner as DecreeReferent;
            if (_defOwner is DecreePartReferent) 
                defOwner = (_defOwner as DecreePartReferent).Owner;
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            bool hasPrefix = false;
            if (parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix) 
            {
                parts.RemoveAt(0);
                hasPrefix = true;
                if (parts.Count == 0) 
                    return null;
            }
            if ((parts.Count == 1 && thisDec == null && parts[0].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram) && parts[0].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.AddAgree) 
            {
                if (parts[0].IsDoubt) 
                    return null;
                if (parts[0].IsNewlineBefore && parts[0].Values.Count <= 1) 
                {
                    Pullenti.Ner.Token tt1 = parts[0].EndToken;
                    if (tt1.Next == null) 
                        return null;
                    tt1 = tt1.Next;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt1, false, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null && br.EndToken.Next != null) 
                            tt1 = br.EndToken.Next;
                    }
                    if (tt1.IsChar(',')) 
                    {
                    }
                    else if (tt1.GetReferent() is DecreeReferent) 
                    {
                    }
                    else if (tt1.IsValue("К", null) && tt1.Next != null && (tt1.Next.GetReferent() is DecreeReferent)) 
                    {
                    }
                    else if (_checkOtherTyp(tt1, true) != null) 
                    {
                    }
                    else if (_defOwner == null) 
                        return null;
                    else if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt1)) 
                        return null;
                    else if (tt1.IsChar('.')) 
                        return null;
                }
            }
            List<Pullenti.Ner.Decree.Internal.PartToken> asc = new List<Pullenti.Ner.Decree.Internal.PartToken>();
            List<Pullenti.Ner.Decree.Internal.PartToken> desc = new List<Pullenti.Ner.Decree.Internal.PartToken>();
            List<Pullenti.Ner.Decree.Internal.PartToken.ItemType> typs = new List<Pullenti.Ner.Decree.Internal.PartToken.ItemType>();
            int ascCount = 0;
            int descCount = 0;
            int terminators = 0;
            for (i = 0; i < (parts.Count - 1); i++) 
            {
                if (!parts[i].HasTerminator) 
                {
                    if (parts[i].CanBeNextNarrow(parts[i + 1])) 
                        ascCount++;
                    if (parts[i + 1].CanBeNextNarrow(parts[i])) 
                        descCount++;
                }
                else if ((ascCount > 0 && parts[i].Values.Count == 1 && parts[i + 1].Values.Count == 1) && parts[i].CanBeNextNarrow(parts[i + 1])) 
                    ascCount++;
                else if ((descCount > 0 && parts[i].Values.Count == 1 && parts[i + 1].Values.Count == 1) && parts[i + 1].CanBeNextNarrow(parts[i])) 
                    descCount++;
                else 
                    terminators++;
            }
            if (terminators == 0 && ((((descCount > 0 && ascCount == 0)) || ((descCount == 0 && ascCount > 0))))) 
            {
                for (i = 0; i < (parts.Count - 1); i++) 
                {
                    parts[i].HasTerminator = false;
                }
            }
            for (i = 0; i < parts.Count; i++) 
            {
                if (parts[i].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix) 
                    continue;
                asc.Clear();
                asc.Add(parts[i]);
                typs.Clear();
                typs.Add(parts[i].Typ);
                for (j = i + 1; j < parts.Count; j++) 
                {
                    if (parts[j].Values.Count == 0 && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Preamble) 
                        break;
                    else if (!typs.Contains(parts[j].Typ) && parts[j - 1].CanBeNextNarrow(parts[j])) 
                    {
                        if (parts[j - 1].DelimAfter && terminators == 0) 
                        {
                            if (descCount > ascCount) 
                                break;
                            if (((j + 1) < parts.Count) && !parts[j].DelimAfter && !parts[j].HasTerminator) 
                                break;
                            if (parts[j - 1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Item && parts[j].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.SubItem) 
                            {
                                if (parts[j].Values.Count > 0 && parts[j].Values[0].ToString().Contains(".")) 
                                    break;
                            }
                        }
                        asc.Add(parts[j]);
                        typs.Add(parts[j].Typ);
                        if (parts[j].HasTerminator) 
                            break;
                    }
                    else 
                        break;
                }
                desc.Clear();
                desc.Add(parts[i]);
                typs.Clear();
                typs.Add(parts[i].Typ);
                for (j = i + 1; j < parts.Count; j++) 
                {
                    if (parts[j].Values.Count == 0 && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Preamble) 
                        break;
                    else if (((!typs.Contains(parts[j].Typ) || parts[j].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.SubItem)) && parts[j].CanBeNextNarrow(parts[j - 1])) 
                    {
                        if (parts[j - 1].DelimAfter && terminators == 0) 
                        {
                            if (descCount <= ascCount) 
                                break;
                        }
                        desc.Add(parts[j]);
                        typs.Add(parts[j].Typ);
                        if (parts[j].HasTerminator) 
                            break;
                    }
                    else if (((!typs.Contains(parts[j].Typ) && parts[j - 1].CanBeNextNarrow(parts[j]) && (j + 1) == (parts.Count - 1)) && parts[j + 1].CanBeNextNarrow(parts[j]) && parts[j + 1].CanBeNextNarrow(parts[j - 1])) && !parts[j].HasTerminator) 
                    {
                        desc.Insert(desc.Count - 1, parts[j]);
                        typs.Add(parts[j].Typ);
                    }
                    else 
                        break;
                }
                desc.Reverse();
                List<Pullenti.Ner.Decree.Internal.PartToken> li = (asc.Count < desc.Count ? desc : asc);
                for (j = 0; j < li.Count; j++) 
                {
                    li[j].Ind = 0;
                }
                while (true) 
                {
                    DecreePartReferent dr = new DecreePartReferent();
                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dr, parts[i].BeginToken, parts[(i + li.Count) - 1].EndToken);
                    if (parts[i].Name != null) 
                        dr.AddSlot(DecreePartReferent.ATTR_NAME, parts[i].Name, false, 0);
                    res.Add(rt);
                    List<Pullenti.Ner.Slot> slList = new List<Pullenti.Ner.Slot>();
                    foreach (Pullenti.Ner.Decree.Internal.PartToken p in li) 
                    {
                        string nam = Pullenti.Ner.Decree.Internal.PartToken._getAttrNameByTyp(p.Typ);
                        if (nam != null) 
                        {
                            Pullenti.Ner.Slot sl = new Pullenti.Ner.Slot() { TypeName = nam, Tag = p, Count = 1 };
                            slList.Add(sl);
                            if (p.Ind < p.Values.Count) 
                            {
                                sl.Value = p.Values[p.Ind];
                                if (string.IsNullOrEmpty(p.Values[p.Ind].Value)) 
                                    sl.Value = "0";
                            }
                            else 
                                sl.Value = "0";
                        }
                        if (p.Ind > 0) 
                            rt.BeginToken = p.Values[p.Ind].BeginToken;
                        if ((p.Ind + 1) < p.Values.Count) 
                            rt.EndToken = p.Values[p.Ind].EndToken;
                    }
                    foreach (Pullenti.Ner.Decree.Internal.PartToken p in parts) 
                    {
                        foreach (Pullenti.Ner.Slot s in slList) 
                        {
                            if (s.Tag == p) 
                            {
                                dr.AddSlot(s.TypeName, s.Value, false, 0);
                                break;
                            }
                        }
                    }
                    for (j = li.Count - 1; j >= 0; j--) 
                    {
                        if ((++li[j].Ind) >= li[j].Values.Count) 
                            li[j].Ind = 0;
                        else 
                            break;
                    }
                    if (j < 0) 
                        break;
                }
                i += (li.Count - 1);
            }
            if (res.Count == 0) 
                return null;
            for (j = res.Count - 1; j > 0; j--) 
            {
                DecreePartReferent d0 = res[j].Referent as DecreePartReferent;
                DecreePartReferent d = res[j - 1].Referent as DecreePartReferent;
                if (d0.Clause != null && d.Clause == null) 
                    d.Clause = d0.Clause;
            }
            tt = parts[i - 1].EndToken;
            DecreeReferent owner = defOwner;
            Pullenti.Ner.Token te = tt.Next;
            if ((te != null && owner == null && te.IsChar('(')) && parts[0].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram && parts[0].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.AddAgree) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(te, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    if (te.Next.Morph.Class.IsAdverb) 
                    {
                    }
                    else if (te.Next.GetReferent() is DecreeReferent) 
                    {
                        if (owner == null && te.Next.Next == br.EndToken) 
                        {
                            owner = te.Next.GetReferent() as DecreeReferent;
                            te = br.EndToken;
                        }
                    }
                    else 
                    {
                        string s = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                        if (s != null) 
                        {
                            Pullenti.Ner.ReferentToken rt = res[res.Count - 1];
                            (rt.Referent as DecreePartReferent).AddName(s);
                            rt.EndToken = br.EndToken;
                            te = rt.EndToken.Next;
                        }
                    }
                }
            }
            if (te != null && te.IsCharOf(",;")) 
                te = te.Next;
            if (owner == null && (te is Pullenti.Ner.ReferentToken)) 
            {
                if ((((owner = te.GetReferent() as DecreeReferent))) != null) 
                    res[res.Count - 1].EndToken = te;
            }
            if (owner == null) 
            {
                for (j = 0; j < i; j++) 
                {
                    if ((((owner = parts[j].Decree))) != null) 
                        break;
                }
            }
            if (te != null && te.IsValue("К", null) && te.Next != null) 
            {
                if (te.Next.GetReferent() is DecreeReferent) 
                {
                    te = te.Next;
                    res[res.Count - 1].EndToken = te;
                    owner = te.GetReferent() as DecreeReferent;
                }
                else if (owner != null && thisDec != null && thisDec.EndChar > te.EndChar) 
                    res[res.Count - 1].EndToken = thisDec.EndToken;
            }
            if (owner == null && thisDec != null) 
            {
                Pullenti.Ner.Token tt0 = res[0].BeginToken;
                if (tt0.Previous != null && tt0.Previous.IsChar('(')) 
                    tt0 = tt0.Previous;
                if (tt0.Previous != null) 
                {
                    if ((((owner = tt0.Previous.GetReferent() as DecreeReferent))) != null) 
                    {
                        if (thisDec.Typ == owner.Typ0) 
                            thisDec = null;
                        else 
                            owner = null;
                    }
                }
            }
            if (owner == null && thisDec != null && thisDec.Real != null) 
                owner = thisDec.Real;
            if (owner != null && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram && owner.Kind != DecreeKind.Program) 
                owner = null;
            if (owner != null && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.AddAgree && owner.Kind != DecreeKind.Contract) 
                owner = null;
            DecreePartReferent ownerPaer = null;
            string locTyp = null;
            if ((thisDec == null || !thisDec.HasThisRef)) 
            {
                Pullenti.Ner.TextToken anaforRef = null;
                foreach (Pullenti.Ner.Decree.Internal.PartToken p in parts) 
                {
                    if ((((anaforRef = p.AnaforRef))) != null) 
                        break;
                }
                bool isChangeWordAfter = false;
                Pullenti.Ner.Token tt2 = res[res.Count - 1].EndToken.Next;
                if (tt2 != null) 
                {
                    if (((tt2.IsChar(':') || tt2.IsValue("ДОПОЛНИТЬ", null) || tt2.IsValue("СЛОВО", null)) || tt2.IsValue("ИСКЛЮЧИТЬ", null) || tt2.IsValue("ИЗЛОЖИТЬ", null)) || tt2.IsValue("СЧИТАТЬ", null) || tt2.IsValue("ПРИЗНАТЬ", null)) 
                        isChangeWordAfter = true;
                }
                tt2 = parts[0].BeginToken.Previous;
                if (tt2 != null) 
                {
                    if (((tt2.IsValue("ДОПОЛНИТЬ", null) || tt2.IsValue("ИСКЛЮЧИТЬ", null) || tt2.IsValue("ИЗЛОЖИТЬ", null)) || tt2.IsValue("СЧИТАТЬ", null) || tt2.IsValue("УСТАНОВЛЕННЫЙ", null)) || tt2.IsValue("ОПРЕДЕЛЕННЫЙ", null)) 
                        isChangeWordAfter = true;
                }
                int cou = 0;
                bool ugolDelo = false;
                int brackLevel = 0;
                Pullenti.Ner.Token bt = null;
                int coefBefore = 0;
                bool isOverBrr = false;
                if (parts[0].BeginToken.Previous != null && parts[0].BeginToken.Previous.IsChar('(')) 
                {
                    if (parts[parts.Count - 1].EndToken.Next != null && parts[parts.Count - 1].EndToken.Next.IsChar(')')) 
                    {
                        if (parts.Count == 1 && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix) 
                        {
                        }
                        else 
                        {
                            isOverBrr = true;
                            if (owner != null && _getDecree(parts[0].BeginToken.Previous.Previous) != null) 
                                owner = null;
                        }
                    }
                }
                for (tt = parts[0].BeginToken.Previous; tt != null; tt = tt.Previous,coefBefore++) 
                {
                    if (tt.IsNewlineAfter) 
                    {
                        coefBefore += 2;
                        if (((anaforRef == null && !isOverBrr && !ugolDelo) && thisDec == null && !isChangeWordAfter) && !isProgram && !isAddAgree) 
                        {
                            if (!tt.IsTableControlChar) 
                                break;
                        }
                    }
                    if (thisDec != null && thisDec.HasThisRef) 
                        break;
                    if (tt.IsTableControlChar) 
                        break;
                    if (tt.Morph.Class.IsPreposition) 
                    {
                        coefBefore--;
                        continue;
                    }
                    if (tt is Pullenti.Ner.TextToken) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt, false, null, false)) 
                        {
                            brackLevel++;
                            continue;
                        }
                        if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                        {
                            if (tt.IsChar('(') && tt == parts[0].BeginToken.Previous) 
                            {
                            }
                            else 
                            {
                                brackLevel--;
                                coefBefore--;
                            }
                            continue;
                        }
                    }
                    if (tt.IsNewlineBefore) 
                        brackLevel = 0;
                    if ((++cou) > 100) 
                    {
                        if (((ugolDelo || isProgram || isAddAgree) || anaforRef != null || thisDec != null) || isOverBrr) 
                        {
                            if (cou > 1000) 
                                break;
                        }
                        else if (isChangeWordAfter) 
                        {
                            if (cou > 250) 
                                break;
                        }
                        else 
                            break;
                    }
                    if (cou < 4) 
                    {
                        if (tt.IsValue("УГОЛОВНЫЙ", "КРИМІНАЛЬНИЙ") && tt.Next != null && tt.Next.IsValue("ДЕЛО", "СПРАВА")) 
                            ugolDelo = true;
                    }
                    if (tt.IsCharOf(".")) 
                    {
                        coefBefore += 50;
                        if (tt.IsNewlineAfter) 
                            coefBefore += 100;
                        continue;
                    }
                    if (brackLevel > 0) 
                        continue;
                    DecreeReferent dr = _getDecree(tt);
                    if (dr != null && dr.Kind != DecreeKind.Publisher) 
                    {
                        if (ugolDelo && ((dr.Name == "УГОЛОВНЫЙ КОДЕКС" || dr.Name == "КРИМІНАЛЬНИЙ КОДЕКС"))) 
                            coefBefore = 0;
                        if (dr.Kind == DecreeKind.Program) 
                        {
                            if (isProgram) 
                            {
                                bt = tt;
                                break;
                            }
                            else 
                                continue;
                        }
                        if (dr.Kind == DecreeKind.Contract) 
                        {
                            if (isAddAgree) 
                            {
                                bt = tt;
                                break;
                            }
                            else if (thisDec != null && ((dr.Typ == thisDec.Typ || dr.Typ0 == thisDec.Typ))) 
                            {
                                bt = tt;
                                break;
                            }
                            else 
                                continue;
                        }
                        if (thisDec != null) 
                        {
                            DecreePartReferent dpr = tt.GetReferent() as DecreePartReferent;
                            if (thisDec.Typ == dr.Typ || thisDec.Typ == dr.Typ0) 
                                thisDec.Real = dr;
                            else if (dr.Name != null && thisDec.Typ != null && dr.Name.StartsWith(thisDec.Typ)) 
                                thisDec.Real = dr;
                            else if ((thisDec.HasOtherRef && dpr != null && dpr.Clause != null) && thisDec.Typ == "СТАТЬЯ") 
                            {
                                foreach (Pullenti.Ner.ReferentToken r in res) 
                                {
                                    DecreePartReferent dpr0 = r.Referent as DecreePartReferent;
                                    if (dpr0.Clause == null) 
                                    {
                                        dpr0.Clause = dpr.Clause;
                                        owner = (dpr0.Owner = dpr.Owner);
                                    }
                                }
                            }
                            else 
                                continue;
                        }
                        else if (isChangeWordAfter) 
                        {
                            if (owner == null) 
                                coefBefore = 0;
                            else if (owner == _getDecree(tt)) 
                                coefBefore = 0;
                        }
                        bt = tt;
                        break;
                    }
                    if (dr != null) 
                        continue;
                    DecreePartReferent dpr2 = tt.GetReferent() as DecreePartReferent;
                    if (dpr2 != null) 
                    {
                        bt = tt;
                        break;
                    }
                    Pullenti.Ner.Decree.Internal.DecreeToken dit = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt, null, false);
                    if (dit != null && dit.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                    {
                        if (thisDec != null) 
                            continue;
                        if (dit.Chars.IsCapitalUpper || anaforRef != null) 
                        {
                            bt = tt;
                            break;
                        }
                    }
                }
                cou = 0;
                Pullenti.Ner.Token at = null;
                int coefAfter = 0;
                string alocTyp = null;
                Pullenti.Ner.Token tt0 = parts[parts.Count - 1].EndToken.Next;
                bool hasNewline = false;
                for (Pullenti.Ner.Token ttt = parts[parts.Count - 1].BeginToken; ttt.EndChar < parts[parts.Count - 1].EndChar; ttt = ttt.Next) 
                {
                    if (ttt.IsNewlineAfter) 
                        hasNewline = true;
                }
                for (tt = tt0; tt != null; tt = tt.Next,coefAfter++) 
                {
                    if (owner != null && coefAfter > 0) 
                        break;
                    if (tt.IsNewlineBefore) 
                        break;
                    if (tt.IsTableControlChar) 
                        break;
                    if (tt.IsValue("СМ", null)) 
                        break;
                    if (anaforRef != null) 
                        break;
                    if (thisDec != null) 
                    {
                        if (tt != tt0) 
                            break;
                        if (thisDec.Real != null) 
                            break;
                    }
                    if (Pullenti.Ner.Instrument.Internal.InstrToken._checkEntered(tt) != null) 
                        break;
                    if (tt.Morph.Class.IsPreposition || tt.IsCommaAnd) 
                    {
                        coefAfter--;
                        continue;
                    }
                    if (tt.Morph.Class == Pullenti.Morph.MorphClass.Verb) 
                        break;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt, false, null, false)) 
                        break;
                    List<Pullenti.Ner.Decree.Internal.PartToken> pts = Pullenti.Ner.Decree.Internal.PartToken.TryAttachList(tt, false, 40);
                    if (pts != null) 
                    {
                        tt = pts[pts.Count - 1].EndToken;
                        coefAfter--;
                        Pullenti.Ner.Token ttnn = tt.Next;
                        if (ttnn != null && ttnn.IsChar('.')) 
                            ttnn = ttnn.Next;
                        Pullenti.Ner.Decree.Internal.DecreeToken dit = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(ttnn, null, false);
                        if (dit != null && dit.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        {
                            locTyp = dit.Value;
                            break;
                        }
                        continue;
                    }
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            coefAfter--;
                            tt = br.EndToken;
                            continue;
                        }
                    }
                    if ((++cou) > 100) 
                        break;
                    if (cou > 1 && hasNewline) 
                        break;
                    if (tt.IsCharOf(".")) 
                    {
                        coefAfter += 50;
                        if (tt.IsNewlineAfter) 
                            coefAfter += 100;
                        continue;
                    }
                    DecreeReferent dr = tt.GetReferent() as DecreeReferent;
                    if (dr != null && dr.Kind != DecreeKind.Publisher) 
                    {
                        if (dr.Kind == DecreeKind.Program) 
                        {
                            if (isProgram) 
                            {
                                at = tt;
                                break;
                            }
                            else 
                                continue;
                        }
                        if (dr.Kind == DecreeKind.Contract) 
                        {
                            if (isAddAgree) 
                            {
                                at = tt;
                                break;
                            }
                            else 
                                continue;
                        }
                        at = tt;
                        break;
                    }
                    if (isProgram || isAddAgree) 
                        break;
                    if (dr != null) 
                        continue;
                    Pullenti.Ner.Token tte2 = _checkOtherTyp(tt, tt == tt0);
                    if (tte2 != null) 
                    {
                        at = tte2;
                        if (tt == tt0 && thisDec != null && thisDec.Real == null) 
                        {
                            if (thisDec.Typ == (at.Tag as string)) 
                                at = null;
                            else 
                                thisDec = null;
                        }
                        break;
                    }
                }
                if (bt != null && at != null) 
                {
                    if (coefBefore < coefAfter) 
                        at = null;
                    else if ((bt is Pullenti.Ner.ReferentToken) && (at is Pullenti.Ner.TextToken)) 
                        at = null;
                    else 
                        bt = null;
                }
                if (owner == null) 
                {
                    if (at != null) 
                    {
                        owner = _getDecree(at);
                        if (at is Pullenti.Ner.TextToken) 
                        {
                            if (at.Tag is string) 
                                locTyp = at.Tag as string;
                            else 
                                locTyp = (at as Pullenti.Ner.TextToken).Lemma;
                        }
                    }
                    else if (bt != null) 
                    {
                        owner = _getDecree(bt);
                        ownerPaer = bt.GetReferent() as DecreePartReferent;
                        if (ownerPaer != null && locTyp == null) 
                            locTyp = ownerPaer.LocalTyp;
                    }
                }
                else if (coefAfter == 0 && at != null) 
                    owner = _getDecree(at);
                else if (coefBefore == 0 && bt != null) 
                {
                    owner = _getDecree(bt);
                    ownerPaer = bt.GetReferent() as DecreePartReferent;
                    if (ownerPaer != null && locTyp == null) 
                        locTyp = ownerPaer.LocalTyp;
                }
                if (((bt != null && parts.Count == 1 && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.DocPart) && (bt.GetReferent() is DecreePartReferent) && (bt.GetReferent() as DecreePartReferent).Clause != null) && res.Count == 1 && owner == (bt.GetReferent() as DecreePartReferent).Owner) 
                {
                    foreach (Pullenti.Ner.Slot s in res[0].Referent.Slots) 
                    {
                        if (s.TypeName == DecreePartReferent.ATTR_DOCPART) 
                            s.TypeName = DecreePartReferent.ATTR_PART;
                    }
                    (res[0].Referent as DecreePartReferent).AddHighLevelInfo(bt.GetReferent() as DecreePartReferent);
                }
            }
            if (owner == null) 
            {
                if (thisDec == null && locTyp == null) 
                {
                    if ((parts.Count == 1 && parts[0].Values.Count == 1 && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix) && parts[0].BeginToken.Chars.IsCapitalUpper) 
                    {
                    }
                    else if ((parts[0].BeginToken.Previous != null && parts[0].BeginToken.Previous.IsChar('(') && parts[parts.Count - 1].EndToken.Next != null) && parts[parts.Count - 1].EndToken.Next.IsChar(')')) 
                    {
                        if (parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Page) 
                            return null;
                    }
                    else 
                        return null;
                }
                foreach (Pullenti.Ner.ReferentToken r in res) 
                {
                    DecreePartReferent dr = r.Referent as DecreePartReferent;
                    if (thisDec != null) 
                    {
                        dr.LocalTyp = thisDec.Typ;
                        if (thisDec.BeginChar > r.EndChar && r == res[res.Count - 1]) 
                            r.EndToken = thisDec.EndToken;
                    }
                    else if (locTyp != null) 
                    {
                        if (locTyp == "СТАТЬЯ" && dr.Clause != null) 
                        {
                        }
                        else if (locTyp == "ГЛАВА" && dr.Chapter != null) 
                        {
                        }
                        else if (locTyp == "ПАРАГРАФ" && dr.Paragraph != null) 
                        {
                        }
                        else if (locTyp == "ЧАСТЬ" && dr.Part != null) 
                        {
                        }
                        else 
                        {
                            dr.LocalTyp = locTyp;
                            if (r == res[res.Count - 1] && !r.IsNewlineAfter) 
                            {
                                Pullenti.Ner.Token ttt1 = r.EndToken.Next;
                                if (ttt1 != null && ttt1.IsComma) 
                                    ttt1 = ttt1.Next;
                                Pullenti.Ner.Token at = _checkOtherTyp(ttt1, true);
                                if (at != null && (at.Tag as string) == locTyp) 
                                    r.EndToken = at;
                            }
                        }
                    }
                }
            }
            else 
                foreach (Pullenti.Ner.ReferentToken r in res) 
                {
                    DecreePartReferent dr = r.Referent as DecreePartReferent;
                    dr.Owner = owner;
                    if (thisDec != null && thisDec.Real == owner) 
                    {
                        if (thisDec.BeginChar > r.EndChar && r == res[res.Count - 1]) 
                            r.EndToken = thisDec.EndToken;
                    }
                }
            if (res.Count > 0) 
            {
                Pullenti.Ner.ReferentToken rt = res[res.Count - 1];
                tt = rt.EndToken.Next;
                if (owner != null && tt != null && tt.GetReferent() == owner) 
                {
                    rt.EndToken = tt;
                    tt = tt.Next;
                }
                if (tt != null && ((tt.IsHiphen || tt.IsChar(':')))) 
                    tt = tt.Next;
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, (isProgram ? Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines : Pullenti.Ner.Core.BracketParseAttr.No), 100);
                if (br != null) 
                {
                    bool ok = true;
                    if (br.OpenChar == '(') 
                    {
                        if (parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram) 
                            ok = false;
                        else if (Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt.Next, null, false, false) != null) 
                            ok = false;
                        else 
                            for (Pullenti.Ner.Token ttt = tt.Next; ttt != null && (ttt.EndChar < br.EndChar); ttt = ttt.Next) 
                            {
                                if (ttt == tt.Next && tt.Next.Morph.Class.IsAdverb) 
                                    ok = false;
                                if ((ttt.GetReferent() is DecreeReferent) || (ttt.GetReferent() is DecreePartReferent)) 
                                    ok = false;
                                if (ttt.IsValue("РЕДАКЦИЯ", null) && ttt == br.EndToken.Previous) 
                                    ok = false;
                            }
                    }
                    if (ok) 
                    {
                        string s = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                        if (s != null) 
                        {
                            (rt.Referent as DecreePartReferent).AddName(s);
                            rt.EndToken = br.EndToken;
                            if ((rt.EndToken.Next is Pullenti.Ner.ReferentToken) && rt.EndToken.Next.GetReferent() == owner) 
                                rt.EndToken = rt.EndToken.Next;
                        }
                    }
                }
                else if ((isProgram && parts[0].Values.Count > 0 && tt != null) && tt.IsTableControlChar && Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt.Next)) 
                {
                    for (Pullenti.Ner.Token tt1 = tt.Next; tt1 != null; tt1 = tt1.Next) 
                    {
                        if (tt1.IsTableControlChar) 
                        {
                            string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt.Next, tt1.Previous, Pullenti.Ner.Core.GetTextAttr.No);
                            if (s != null) 
                            {
                                (rt.Referent as DecreePartReferent).AddName(s);
                                rt.EndToken = tt1;
                            }
                            break;
                        }
                        else if (tt1.IsNewlineBefore) 
                            break;
                    }
                }
                if (thisDec != null) 
                {
                    if (thisDec.EndChar > res[res.Count - 1].EndChar) 
                        res[res.Count - 1].EndToken = thisDec.EndToken;
                }
            }
            if (ownerPaer != null) 
            {
                for (int ii = 0; ii < res.Count; ii++) 
                {
                    (res[ii].Referent as DecreePartReferent).AddHighLevelInfo((ii == 0 ? ownerPaer : res[ii - 1].Referent as DecreePartReferent));
                }
            }
            if (res.Count == 1 && (res[0].Referent as DecreePartReferent).Name == null) 
            {
                if ((res[0].BeginToken.Previous != null && res[0].BeginToken.Previous.IsChar('(') && res[0].EndToken.Next != null) && res[0].EndToken.Next.IsChar(')')) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(res[0].BeginToken.Previous.Previous, false, null, false)) 
                    {
                        Pullenti.Ner.Token beg = null;
                        for (tt = res[0].BeginToken.Previous.Previous.Previous; tt != null; tt = tt.Previous) 
                        {
                            if (tt.IsNewlineAfter) 
                                break;
                            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null && ((br.EndChar + 10) < res[0].BeginChar)) 
                                    break;
                                if (tt.Next.Chars.IsLetter && !tt.Next.Chars.IsAllLower) 
                                    beg = tt;
                            }
                        }
                        if (beg != null) 
                        {
                            (res[0].Referent as DecreePartReferent).AddName(Pullenti.Ner.Core.MiscHelper.GetTextValue(beg, res[0].BeginToken.Previous.Previous, Pullenti.Ner.Core.GetTextAttr.No));
                            res[0].BeginToken = beg;
                            res[0].EndToken = res[0].EndToken.Next;
                        }
                    }
                }
            }
            if (isProgram) 
            {
                for (i = res.Count - 1; i >= 0; i--) 
                {
                    DecreePartReferent pa = res[i].Referent as DecreePartReferent;
                    if (pa.Subprogram == null) 
                        continue;
                    if (pa.Owner == null || pa.Name == null || pa.Owner.Kind != DecreeKind.Program) 
                        res.RemoveAt(i);
                }
            }
            if (isAddAgree) 
            {
                for (i = res.Count - 1; i >= 0; i--) 
                {
                    DecreePartReferent pa = res[i].Referent as DecreePartReferent;
                    if (pa.Addagree == null) 
                        continue;
                    if (pa.Owner == null || pa.Owner.Kind != DecreeKind.Contract) 
                        res.RemoveAt(i);
                }
            }
            List<Pullenti.Ner.MetaToken> res1 = new List<Pullenti.Ner.MetaToken>();
            for (i = 0; i < res.Count; i++) 
            {
                List<DecreePartReferent> li = new List<DecreePartReferent>();
                for (j = i; j < res.Count; j++) 
                {
                    if (res[j].BeginToken != res[i].BeginToken) 
                        break;
                    else 
                        li.Add(res[j].Referent as DecreePartReferent);
                }
                Pullenti.Ner.Token et;
                if (j < res.Count) 
                    et = res[j].BeginToken.Previous;
                else 
                    et = res[res.Count - 1].EndToken;
                while (et.BeginChar > res[i].BeginChar) 
                {
                    if (et.IsChar(',') || et.Morph.Class.IsConjunction || et.IsHiphen) 
                        et = et.Previous;
                    else if (Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(et) != null) 
                        et = et.Previous;
                    else 
                        break;
                }
                res1.Add(new Pullenti.Ner.MetaToken(res[i].BeginToken, et) { Tag = li });
                i = j - 1;
            }
            return res1;
        }
        class ThisDecree : Pullenti.Ner.MetaToken
        {
            public ThisDecree(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
            {
            }
            public string Typ;
            public bool HasThisRef;
            public bool HasOtherRef;
            public Pullenti.Ner.Decree.DecreeReferent Real;
            public override string ToString()
            {
                return string.Format("{0} ({1})", Typ ?? "?", (HasThisRef ? "This" : (HasOtherRef ? "Other" : "?")));
            }
            public static ThisDecree TryAttachBack(Pullenti.Ner.Token t, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp)
            {
                if (t == null) 
                    return null;
                Pullenti.Ner.Token ukaz = null;
                for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Previous) 
                {
                    if (tt.IsCharOf(",") || tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction) 
                        continue;
                    if ((((((tt.IsValue("ОПРЕДЕЛЕННЫЙ", "ПЕВНИЙ") || tt.IsValue("ЗАДАННЫЙ", "ЗАДАНИЙ") || tt.IsValue("ПРЕДУСМОТРЕННЫЙ", "ПЕРЕДБАЧЕНИЙ")) || tt.IsValue("УКАЗАННЫЙ", "ЗАЗНАЧЕНИЙ") || tt.IsValue("ПЕРЕЧИСЛЕННЫЙ", "ПЕРЕРАХОВАНИЙ")) || tt.IsValue("ОПРЕДЕЛИТЬ", "ВИЗНАЧИТИ") || tt.IsValue("ОПРЕДЕЛЯТЬ", null)) || tt.IsValue("ЗАДАВАТЬ", "ЗАДАВАТИ") || tt.IsValue("ПРЕДУСМАТРИВАТЬ", "ПЕРЕДБАЧАТИ")) || tt.IsValue("УКАЗЫВАТЬ", "ВКАЗУВАТИ") || tt.IsValue("УКАЗАТЬ", "ВКАЗАТИ")) || tt.IsValue("СИЛА", "ЧИННІСТЬ")) 
                    {
                        ukaz = tt;
                        continue;
                    }
                    if (tt == t) 
                        continue;
                    Pullenti.Ner.Token ttt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt, false);
                    if (tt != ttt || !(tt is Pullenti.Ner.TextToken)) 
                        break;
                    if (ttt.IsValue("УСЛОВИЕ", null)) 
                        continue;
                    if (ttt.IsValue("ПОРЯДОК", null) && ukaz != null) 
                        return null;
                    ThisDecree res = new ThisDecree(tt, tt);
                    res.Typ = (tt as Pullenti.Ner.TextToken).Lemma;
                    t = tt.Previous;
                    if (t != null && ((t.Morph.Class.IsAdjective || t.Morph.Class.IsPronoun))) 
                    {
                        if (t.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ") || t.IsValue("ТЕКУЩИЙ", "ПОТОЧНИЙ") || t.IsValue("ДАННЫЙ", "ДАНИЙ")) 
                        {
                            res.HasThisRef = true;
                            res.BeginToken = t;
                        }
                        else if ((t.IsValue("ЭТОТ", "ЦЕЙ") || t.IsValue("ВЫШЕУКАЗАННЫЙ", "ВИЩЕВКАЗАНИЙ") || t.IsValue("УКАЗАННЫЙ", "ЗАЗНАЧЕНИЙ")) || t.IsValue("НАЗВАННЫЙ", "НАЗВАНИЙ")) 
                        {
                            res.HasOtherRef = true;
                            res.BeginToken = t;
                        }
                    }
                    if (!res.HasThisRef && tt.IsNewlineAfter) 
                        return null;
                    if (baseTyp != null && baseTyp.Value == res.Typ) 
                        res.HasThisRef = true;
                    return res;
                }
                if (ukaz != null) 
                {
                    if (baseTyp != null && baseTyp.Value != null && ((baseTyp.Value.Contains("ДОГОВОР") || baseTyp.Value.Contains("ДОГОВІР")))) 
                        return new ThisDecree(ukaz, ukaz) { HasThisRef = true, Typ = baseTyp.Value };
                }
                return null;
            }
            public static ThisDecree TryAttach(Pullenti.Ner.Decree.Internal.PartToken dtok, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp)
            {
                Pullenti.Ner.Token t = dtok.EndToken.Next;
                if (t == null) 
                    return null;
                if (t.IsNewlineBefore) 
                {
                    if (t.Chars.IsCyrillicLetter && t.Chars.IsAllLower) 
                    {
                    }
                    else 
                        return null;
                }
                Pullenti.Ner.Token t0 = t;
                if (t.IsChar('.') && t.Next != null && !t.IsNewlineAfter) 
                {
                    if (dtok.IsNewlineBefore) 
                        return null;
                    t = t.Next;
                }
                if (t.IsValue("К", null) && t.Next != null) 
                    t = t.Next;
                if (t != null && (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                    return null;
                Pullenti.Ner.Token tt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false);
                bool br = false;
                if (tt == null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                {
                    tt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t.Next, false);
                    if ((tt is Pullenti.Ner.TextToken) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt.Next, false, null, false)) 
                        br = true;
                }
                if (!(tt is Pullenti.Ner.TextToken)) 
                {
                    if ((tt is Pullenti.Ner.ReferentToken) && (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                        return new ThisDecree(t, tt) { Real = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent };
                    return null;
                }
                if (tt.Chars.IsAllLower) 
                {
                    if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt, true) != null) 
                    {
                        if (tt != t && t.Chars.IsCapitalUpper) 
                        {
                        }
                        else 
                            return null;
                    }
                }
                if (!(t is Pullenti.Ner.TextToken)) 
                    return null;
                ThisDecree res = new ThisDecree(t0, (br ? tt.Next : tt));
                res.Typ = (tt as Pullenti.Ner.TextToken).Lemma;
                if (tt.Previous is Pullenti.Ner.TextToken) 
                {
                    Pullenti.Ner.Token tt1 = tt.Previous;
                    Pullenti.Morph.MorphClass mc = tt1.GetMorphClassInDictionary();
                    if (mc.IsAdjective && !mc.IsVerb && !tt1.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ")) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (nnn != null) 
                            res.Typ = nnn.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                        if (tt1.Previous is Pullenti.Ner.TextToken) 
                        {
                            tt1 = tt1.Previous;
                            mc = tt1.GetMorphClassInDictionary();
                            if (mc.IsAdjective && !mc.IsVerb && !tt1.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ")) 
                            {
                                nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (nnn != null) 
                                    res.Typ = nnn.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                            }
                        }
                    }
                }
                if (tt.IsChar('.') && (tt.Previous is Pullenti.Ner.TextToken)) 
                    res.Typ = (tt.Previous as Pullenti.Ner.TextToken).Lemma;
                if (t.Morph.Class.IsAdjective || t.Morph.Class.IsPronoun) 
                {
                    if (t.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ") || t.IsValue("ТЕКУЩИЙ", "ПОТОЧНИЙ") || t.IsValue("ДАННЫЙ", "ДАНИЙ")) 
                        res.HasThisRef = true;
                    else if ((t.IsValue("ЭТОТ", "ЦЕЙ") || t.IsValue("ВЫШЕУКАЗАННЫЙ", "ВИЩЕВКАЗАНИЙ") || t.IsValue("УКАЗАННЫЙ", "ЗАЗНАЧЕНИЙ")) || t.IsValue("НАЗВАННЫЙ", "НАЗВАНИЙ")) 
                        res.HasOtherRef = true;
                }
                if (!tt.IsNewlineAfter && !res.HasThisRef) 
                {
                    Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt.Next, null, false);
                    if (dt != null && dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Misc) 
                        return null;
                    if (Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName(tt.Next, res.Typ, false, false) != null) 
                        return null;
                }
                if (baseTyp != null && baseTyp.Value == res.Typ) 
                    res.HasThisRef = true;
                return res;
            }
        }

        internal static List<Pullenti.Ner.ReferentToken> TryAttach(List<Pullenti.Ner.Decree.Internal.DecreeToken> dts, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp, Pullenti.Ner.Core.AnalyzerData ad)
        {
            List<Pullenti.Ner.ReferentToken> res = _TryAttach(dts, baseTyp, false, ad);
            return res;
        }
        static List<Pullenti.Ner.ReferentToken> _TryAttach(List<Pullenti.Ner.Decree.Internal.DecreeToken> dts, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp, bool afterDecree, Pullenti.Ner.Core.AnalyzerData ad)
        {
            if (dts == null || (dts.Count < 1)) 
                return null;
            if (dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Edition && dts.Count > 1) 
                dts.RemoveAt(0);
            if (dts.Count == 1) 
            {
                if (dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.DecreeRef && dts[0].Ref != null) 
                {
                    if (baseTyp != null) 
                    {
                        Pullenti.Ner.Referent re = dts[0].Ref.GetReferent();
                        DecreeReferent dre = re as DecreeReferent;
                        if (dre == null && (re is DecreePartReferent)) 
                            dre = (re as DecreePartReferent).Owner;
                        if (dre != null) 
                        {
                            if (dre.Typ == baseTyp.Value || dre.Typ0 == baseTyp.Value) 
                                return null;
                        }
                    }
                    List<Pullenti.Ner.ReferentToken> reli = new List<Pullenti.Ner.ReferentToken>();
                    reli.Add(new Pullenti.Ner.ReferentToken(dts[0].Ref.Referent, dts[0].BeginToken, dts[0].EndToken));
                    return reli;
                }
            }
            DecreeReferent dec0 = null;
            bool kodeks = false;
            int maxEmpty = 30;
            for (Pullenti.Ner.Token t = dts[0].BeginToken.Previous; t != null; t = t.Previous) 
            {
                if (t.IsCommaAnd) 
                    continue;
                if (t.IsChar(')')) 
                {
                    int cou = 0;
                    for (t = t.Previous; t != null; t = t.Previous) 
                    {
                        if (t.IsChar('(')) 
                            break;
                        else if ((++cou) > 200) 
                            break;
                    }
                    if (t != null && t.IsChar('(')) 
                        continue;
                    break;
                }
                if ((--maxEmpty) < 0) 
                    break;
                if (!t.Chars.IsLetter) 
                    continue;
                dec0 = t.GetReferent() as DecreeReferent;
                if (dec0 != null) 
                {
                    if (Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dec0.Typ) == DecreeKind.Kodex) 
                        kodeks = true;
                    else if (dec0.Kind == DecreeKind.Publisher) 
                        dec0 = null;
                }
                break;
            }
            DecreeReferent dec = new DecreeReferent();
            int i = 0;
            Pullenti.Ner.MorphCollection morph = null;
            bool isNounDoubt = false;
            Pullenti.Ner.Decree.Internal.DecreeToken numTok = null;
            for (i = 0; i < dts.Count; i++) 
            {
                if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    if (dts[i].Value == null) 
                        break;
                    if (dts[i].IsNewlineBefore) 
                    {
                        if (dec.Date != null || dec.Number != null) 
                            break;
                    }
                    if (dec.Typ != null) 
                    {
                        if (((dec.Typ == "РЕШЕНИЕ" || dec.Typ == "РІШЕННЯ")) && dts[i].Value == "ПРОТОКОЛ") 
                        {
                        }
                        else if (dec.Typ == dts[i].Value && dec.Typ == "ГОСТ") 
                            continue;
                        else 
                            break;
                    }
                    DecreeKind ki = Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dts[i].Value);
                    if (ki == DecreeKind.Standard) 
                    {
                        if (i > 0) 
                        {
                            if (dts.Count == 2 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && dts[i].Value == "ТЕХНИЧЕСКИЕ УСЛОВИЯ") 
                            {
                            }
                            else 
                                return null;
                        }
                    }
                    if (ki == DecreeKind.Kodex) 
                    {
                        if (i > 0) 
                            break;
                        if (dts[i].Value != "ОСНОВЫ ЗАКОНОДАТЕЛЬСТВА" && dts[i].Value != "ОСНОВИ ЗАКОНОДАВСТВА") 
                            kodeks = true;
                        else 
                            kodeks = false;
                    }
                    else 
                        kodeks = false;
                    morph = dts[i].Morph;
                    dec.Typ = dts[i].Value;
                    if (dts[i].FullValue != null) 
                        dec.AddNameStr(dts[i].FullValue);
                    isNounDoubt = dts[i].IsDoubtful;
                    if (isNounDoubt && i == 0) 
                    {
                        if (Pullenti.Ner.Decree.Internal.PartToken.IsPartBefore(dts[i].BeginToken)) 
                            isNounDoubt = false;
                    }
                    if (dts[i].Ref != null) 
                    {
                        if (dec.FindSlot(DecreeReferent.ATTR_GEO, null, true) == null) 
                        {
                            dec.AddSlot(DecreeReferent.ATTR_GEO, dts[i].Ref.Referent, false, 0);
                            dec.AddExtReferent(dts[i].Ref);
                        }
                    }
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                {
                    if (dec.Date != null) 
                        break;
                    if (kodeks) 
                    {
                        if (i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                        {
                        }
                        else 
                            break;
                    }
                    if (i == (dts.Count - 1)) 
                    {
                        if (!dts[i].BeginToken.IsValue("ОТ", "ВІД")) 
                        {
                            DecreeKind ty = Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dec.Typ);
                            if ((ty == DecreeKind.Konvention || ty == DecreeKind.Contract || dec.Typ0 == "ПИСЬМО") || dec.Typ0 == "ЛИСТ") 
                            {
                            }
                            else 
                                break;
                        }
                    }
                    dec.AddDate(dts[i]);
                    dec.AddExtReferent(dts[i].Ref);
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.DateRange) 
                {
                    if (dec.Kind != DecreeKind.Program) 
                        break;
                    dec.AddDate(dts[i]);
                    dec.AddExtReferent(dts[i].Ref);
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Edition) 
                {
                    if (dts[i].IsNewlineBefore && !dts[i].BeginToken.Chars.IsAllLower && !dts[i].BeginToken.IsChar('(')) 
                        break;
                    if (((i + 2) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        break;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                {
                    if (kodeks) 
                    {
                        if (((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        {
                        }
                        else 
                            break;
                    }
                    numTok = dts[i];
                    if (dts[i].IsDelo) 
                    {
                        if (dec.CaseNumber != null) 
                            break;
                        dec.AddSlot(DecreeReferent.ATTR_CASENUMBER, dts[i].Value, true, 0);
                        continue;
                    }
                    if (dec.Number != null) 
                    {
                        if (i > 2 && ((dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org)) && dts[i - 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                        {
                        }
                        else 
                            break;
                    }
                    if (dts[i].IsNewlineBefore) 
                    {
                        if (dec.Typ == null && dec0 == null) 
                            break;
                    }
                    if (Pullenti.Morph.LanguageHelper.EndsWith(dts[i].Value, "ФЗ")) 
                        dec.Typ = "ФЕДЕРАЛЬНЫЙ ЗАКОН";
                    if (Pullenti.Morph.LanguageHelper.EndsWith(dts[i].Value, "ФКЗ")) 
                        dec.Typ = "ФЕДЕРАЛЬНЫЙ КОНСТИТУЦИОННЫЙ ЗАКОН";
                    if (dts[i].Value != null && dts[i].Value.StartsWith("ПР", StringComparison.OrdinalIgnoreCase) && dec.Typ == null) 
                        dec.Typ = "ПОРУЧЕНИЕ";
                    if (dec.Typ == null) 
                    {
                        if (dec0 == null && !afterDecree && baseTyp == null) 
                            break;
                    }
                    dec.AddNumber(dts[i]);
                    if (dts[i].Children != null) 
                    {
                        int cou = 0;
                        foreach (Pullenti.Ner.Slot s in dec.Slots) 
                        {
                            if (s.TypeName == DecreeReferent.ATTR_SOURCE) 
                                cou++;
                        }
                        if (cou == (dts[i].Children.Count + 1)) 
                        {
                            foreach (Pullenti.Ner.Decree.Internal.DecreeToken dd in dts[i].Children) 
                            {
                                dec.AddNumber(dd);
                            }
                            dts[i].Children = null;
                        }
                    }
                    continue;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) 
                {
                    if (dec.Typ == null && dec.Number == null && dec0 == null) 
                        break;
                    if (dec.GetStringValue(DecreeReferent.ATTR_NAME) != null) 
                    {
                        if (kodeks) 
                            break;
                        if (i > 0 && dts[i - 1].EndToken.Next == dts[i].BeginToken) 
                        {
                        }
                        else 
                            break;
                    }
                    string nam = dts[i].Value;
                    if (kodeks && !nam.ToUpper().Contains("КОДЕКС")) 
                        nam = "Кодекс " + nam;
                    dec.AddNameStr(nam);
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Between) 
                {
                    if (dec.Kind != DecreeKind.Contract) 
                        break;
                    foreach (Pullenti.Ner.Decree.Internal.DecreeToken chh in dts[i].Children) 
                    {
                        dec.AddSlot(DecreeReferent.ATTR_SOURCE, chh.Ref.Referent, false, 0).Tag = chh.GetSourceText();
                        if (chh.Ref.Referent is Pullenti.Ner.Person.PersonPropertyReferent) 
                            dec.AddExtReferent(chh.Ref);
                    }
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner) 
                {
                    if (kodeks) 
                        break;
                    if (dec.Name != null) 
                        break;
                    if (((i == 0 || i == (dts.Count - 1))) && dts[i].BeginToken.Chars.IsAllLower) 
                        break;
                    if (i == 0 && dts.Count > 1 && dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        break;
                    if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) != null) 
                    {
                    }
                    if (dts[i].Ref != null) 
                    {
                        DecreeKind ty = Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dec.Typ);
                        if (ty == DecreeKind.Ustav) 
                        {
                            if (!(dts[i].Ref.Referent is Pullenti.Ner.Org.OrganizationReferent)) 
                                break;
                        }
                        dec.AddSlot(DecreeReferent.ATTR_SOURCE, dts[i].Ref.Referent, false, 0).Tag = dts[i].GetSourceText();
                        if (dts[i].Ref.Referent is Pullenti.Ner.Person.PersonPropertyReferent) 
                            dec.AddExtReferent(dts[i].Ref);
                    }
                    else 
                        dec.AddSlot(DecreeReferent.ATTR_SOURCE, Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(dts[i].Value), false, 0).Tag = dts[i].GetSourceText();
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                {
                    if (kodeks) 
                        break;
                    if (dec.Name != null) 
                        break;
                    if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) != null) 
                    {
                        if (i > 2 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && ((dts[i - 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org || dts[i - 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner))) 
                        {
                        }
                        else if (dts[i].BeginToken.Previous != null && dts[i].BeginToken.Previous.IsAnd) 
                        {
                        }
                        else if (i > 0 && ((dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org))) 
                        {
                        }
                        else 
                            break;
                    }
                    Pullenti.Ner.Slot sl = dec.AddSlot(DecreeReferent.ATTR_SOURCE, dts[i].Ref.Referent, false, 0);
                    sl.Tag = dts[i].GetSourceText();
                    if (((i + 2) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Unknown && (dts[i + 1].WhitespacesBeforeCount < 2)) 
                    {
                        if (dts[i + 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dts[i + 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        {
                            sl.Tag = (new Pullenti.Ner.MetaToken(dts[i].BeginToken, dts[i + 1].EndToken)).GetSourceText();
                            i++;
                        }
                    }
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                {
                    if (dec.FindSlot(DecreeReferent.ATTR_GEO, null, true) != null) 
                        break;
                    if (i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) 
                        break;
                    if (dts[i].IsNewlineBefore && ((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        break;
                    dec.AddSlot(DecreeReferent.ATTR_GEO, dts[i].Ref.Referent, false, 0);
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Unknown) 
                {
                    if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) != null) 
                        break;
                    if (kodeks) 
                        break;
                    if ((dec.Kind == DecreeKind.Contract && i == 1 && ((i + 1) < dts.Count)) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                    {
                        dec.AddNameStr(Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(dts[i], Pullenti.Ner.Core.GetTextAttr.KeepRegister));
                        continue;
                    }
                    if (i == 0) 
                    {
                        if (dec0 == null && !afterDecree) 
                            break;
                        bool ok1 = false;
                        if (((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            ok1 = true;
                        else if (((i + 2) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr && dts[i + 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            ok1 = true;
                        if (!ok1) 
                            break;
                    }
                    else if (dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                        continue;
                    if ((i + 1) >= dts.Count) 
                        break;
                    if (dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dts[0].IsDoubtful) 
                        break;
                    if (dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) 
                    {
                        dec.AddSlot(DecreeReferent.ATTR_SOURCE, dts[i].Value, false, 0).Tag = dts[i].GetSourceText();
                        continue;
                    }
                    if (dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                    {
                        dec.AddSlot(DecreeReferent.ATTR_SOURCE, dts[i].Value, false, 0).Tag = dts[i].GetSourceText();
                        continue;
                    }
                    if (dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner) 
                    {
                        string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(dts[i].BeginToken, dts[i + 1].EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                        dts[i].EndToken = dts[i + 1].EndToken;
                        dec.AddSlot(DecreeReferent.ATTR_SOURCE, s, false, 0).Tag = dts[i].GetSourceText();
                        i++;
                        continue;
                    }
                    break;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Misc) 
                {
                    if (i == 0 || kodeks) 
                        break;
                    if ((i + 1) >= dts.Count) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(dts[i].EndToken.Next, true, false)) 
                            continue;
                        if (i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                        {
                            if (Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName(dts[i].EndToken.Next, null, true, false) != null) 
                                continue;
                        }
                    }
                    else if (dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name || dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        continue;
                    break;
                }
                else 
                    break;
            }
            if (i == 0) 
                return null;
            if (dec.Typ == null || ((dec0 != null && dts[0].Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ))) 
            {
                if (dec0 != null) 
                {
                    if (dec.Number == null && dec.Date == null && dec.FindSlot(DecreeReferent.ATTR_NAME, null, true) == null) 
                        return null;
                    if (dec.Typ == null) 
                        dec.Typ = dec0.Typ;
                    if (dec.FindSlot(DecreeReferent.ATTR_GEO, null, true) == null) 
                        dec.AddSlot(DecreeReferent.ATTR_GEO, dec0.GetStringValue(DecreeReferent.ATTR_GEO), false, 0);
                    if (dec.FindSlot(DecreeReferent.ATTR_DATE, null, true) == null && dec0.Date != null) 
                        dec.AddSlot(DecreeReferent.ATTR_DATE, dec0.GetSlotValue(DecreeReferent.ATTR_DATE), false, 0);
                    Pullenti.Ner.Slot sl;
                    if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) == null) 
                    {
                        if ((((sl = dec0.FindSlot(DecreeReferent.ATTR_SOURCE, null, true)))) != null) 
                            dec.AddSlot(DecreeReferent.ATTR_SOURCE, sl.Value, false, 0).Tag = sl.Tag;
                    }
                }
                else if (baseTyp != null && afterDecree) 
                    dec.Typ = baseTyp.Value;
                else 
                    return null;
            }
            Pullenti.Ner.Token et = dts[i - 1].EndToken;
            if ((((!afterDecree && dts.Count == i && i == 3) && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) && dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) != null && et.Next != null) && et.Next.IsComma && dec.Number != null) 
            {
                for (Pullenti.Ner.Token tt = et.Next; tt != null; tt = tt.Next) 
                {
                    if (!tt.IsChar(',')) 
                        break;
                    List<Pullenti.Ner.Decree.Internal.DecreeToken> ddd = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(tt.Next, dts[0], 10, false);
                    if (ddd == null || (ddd.Count < 2) || ddd[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        break;
                    bool hasNum = false;
                    foreach (Pullenti.Ner.Decree.Internal.DecreeToken d in ddd) 
                    {
                        if (d.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            hasNum = true;
                        else if (d.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        {
                            hasNum = false;
                            break;
                        }
                    }
                    if (!hasNum) 
                        break;
                    List<Pullenti.Ner.ReferentToken> rtt = _TryAttach(ddd, dts[0], true, ad);
                    if (rtt == null) 
                        break;
                    dec.MergeSlots(rtt[0].Referent, true);
                    et = (tt = rtt[0].EndToken);
                }
            }
            if (((et.Next != null && et.Next.IsChar('<') && (et.Next.Next is Pullenti.Ner.ReferentToken)) && et.Next.Next.Next != null && et.Next.Next.Next.IsChar('>')) && et.Next.Next.GetReferent().TypeName == "URI") 
                et = et.Next.Next.Next;
            string num = dec.Number;
            if ((dec.FindSlot(DecreeReferent.ATTR_NAME, null, true) == null && (i < dts.Count) && dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) && dec.Kind == DecreeKind.Project) 
            {
                List<Pullenti.Ner.Decree.Internal.DecreeToken> dts1 = new List<Pullenti.Ner.Decree.Internal.DecreeToken>(dts);
                dts1.RemoveRange(0, i);
                List<Pullenti.Ner.ReferentToken> rt1 = _TryAttach(dts1, null, true, ad);
                if (rt1 != null) 
                {
                    dec.AddNameStr(Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(rt1[0], Pullenti.Ner.Core.GetTextAttr.KeepRegister));
                    et = rt1[0].EndToken;
                }
            }
            if (dec.FindSlot(DecreeReferent.ATTR_NAME, null, true) == null && !kodeks && et.Next != null) 
            {
                Pullenti.Ner.Decree.Internal.DecreeToken dn = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName((et.Next.IsChar(':') ? et.Next.Next : et.Next), dec.Typ, false, false);
                if (dn != null && et.Next.Chars.IsAllLower && num != null) 
                {
                    if (ad != null) 
                    {
                        foreach (Pullenti.Ner.Referent r in ad.Referents) 
                        {
                            if (r.FindSlot(DecreeReferent.ATTR_NUMBER, num, true) != null) 
                            {
                                if (r.CanBeEquals(dec, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                {
                                    if (r.FindSlot(DecreeReferent.ATTR_NAME, dn.Value, true) == null) 
                                        dn = null;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dn != null && et.IsNewlineAfter) 
                    dn = null;
                if (dn != null) 
                {
                    if (dec.Kind == DecreeKind.Program) 
                    {
                        for (Pullenti.Ner.Token tt1 = dn.EndToken.Previous; tt1 != null && tt1.BeginChar > dn.BeginChar; tt1 = (tt1 == null ? null : tt1.Previous)) 
                        {
                            if (tt1.IsChar(')') && tt1.Previous != null) 
                                tt1 = tt1.Previous;
                            if (tt1.GetReferent() is Pullenti.Ner.Date.DateRangeReferent) 
                                dec.AddSlot(DecreeReferent.ATTR_DATE, tt1.GetReferent(), false, 0);
                            else if ((tt1.GetReferent() is Pullenti.Ner.Date.DateReferent) && tt1.Previous != null && tt1.Previous.IsValue("ДО", null)) 
                            {
                                Pullenti.Ner.ReferentToken rt11 = tt1.Kit.ProcessReferent("DATE", tt1.Previous);
                                if (rt11 != null && (rt11.Referent is Pullenti.Ner.Date.DateRangeReferent)) 
                                {
                                    dec.AddSlot(DecreeReferent.ATTR_DATE, rt11.Referent, false, 0);
                                    dec.AddExtReferent(rt11);
                                    tt1 = tt1.Previous;
                                }
                                else 
                                    break;
                            }
                            else if ((tt1.GetReferent() is Pullenti.Ner.Date.DateReferent) && tt1.Previous != null && ((tt1.Previous.IsValue("НА", null) || tt1.Previous.IsValue("В", null)))) 
                            {
                                dec.AddSlot(DecreeReferent.ATTR_DATE, tt1.GetReferent(), false, 0);
                                tt1 = tt1.Previous;
                            }
                            else 
                                break;
                            for (tt1 = tt1.Previous; tt1 != null && tt1.BeginChar > dn.BeginChar; tt1 = (tt1 == null ? null : tt1.Previous)) 
                            {
                                if (tt1.Morph.Class.IsConjunction || tt1.Morph.Class.IsPreposition) 
                                    continue;
                                if (tt1.IsValue("ПЕРИОД", "ПЕРІОД") || tt1.IsValue("ПЕРСПЕКТИВА", null)) 
                                    continue;
                                if (tt1.IsChar('(')) 
                                    continue;
                                break;
                            }
                            if (tt1 != null && tt1.EndChar > dn.BeginChar) 
                            {
                                if (dn.FullValue == null) 
                                    dn.FullValue = dn.Value;
                                dn.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(dn.BeginToken, tt1, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                            }
                            tt1 = tt1.Next;
                        }
                    }
                    if (dn.FullValue != null) 
                        dec.AddNameStr(dn.FullValue);
                    dec.AddNameStr(dn.Value);
                    et = dn.EndToken;
                    bool br = false;
                    for (Pullenti.Ner.Token tt = et.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsChar('(')) 
                        {
                            br = true;
                            continue;
                        }
                        if (tt.IsChar(')') && br) 
                        {
                            et = tt;
                            continue;
                        }
                        if ((tt.GetReferent() is Pullenti.Ner.Date.DateRangeReferent) && dec.Kind == DecreeKind.Program) 
                        {
                            dec.AddSlot(DecreeReferent.ATTR_DATE, tt.GetReferent(), false, 0);
                            et = tt;
                            continue;
                        }
                        dn = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt, null, false);
                        if (dn == null) 
                            break;
                        if (dn.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date && dec.Date == null) 
                        {
                            if (dec.AddDate(dn)) 
                            {
                                et = (tt = dn.EndToken);
                                continue;
                            }
                        }
                        if (dn.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && dec.Number == null) 
                        {
                            dec.AddNumber(dn);
                            et = (tt = dn.EndToken);
                            continue;
                        }
                        if (dn.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.DateRange && dec.Kind == DecreeKind.Program) 
                        {
                            if (dec.AddDate(dn)) 
                            {
                                et = (tt = dn.EndToken);
                                continue;
                            }
                        }
                        break;
                    }
                }
            }
            if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) == null) 
            {
                Pullenti.Ner.Token tt0 = dts[0].BeginToken.Previous;
                if ((tt0 != null && tt0.IsValue("В", "У") && tt0.Previous != null) && (tt0.Previous.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                    dec.AddSlot(DecreeReferent.ATTR_SOURCE, tt0.Previous.GetReferent(), false, 0);
            }
            if (!dec.CheckCorrection(isNounDoubt)) 
            {
                string ty = dec.Typ;
                Pullenti.Ner.Slot sl = null;
                if (dec0 != null && dec.Date != null && dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) == null) 
                    sl = dec0.FindSlot(DecreeReferent.ATTR_SOURCE, null, true);
                if (sl != null && (((((ty == "ПОСТАНОВЛЕНИЕ" || ty == "ПОСТАНОВА" || ty == "ОПРЕДЕЛЕНИЕ") || ty == "ВИЗНАЧЕННЯ" || ty == "РЕШЕНИЕ") || ty == "РІШЕННЯ" || ty == "ПРИГОВОР") || ty == "ВИРОК"))) 
                    dec.AddSlot(sl.TypeName, sl.Value, false, 0).Tag = sl.Tag;
                else 
                {
                    int eqDecs = 0;
                    DecreeReferent dr0 = null;
                    if (num != null) 
                    {
                        if (ad != null) 
                        {
                            foreach (Pullenti.Ner.Referent r in ad.Referents) 
                            {
                                if (r.FindSlot(DecreeReferent.ATTR_NUMBER, num, true) != null) 
                                {
                                    if (r.CanBeEquals(dec, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                    {
                                        eqDecs++;
                                        dr0 = r as DecreeReferent;
                                    }
                                }
                            }
                        }
                    }
                    if (eqDecs == 1) 
                        dec.MergeSlots(dr0, true);
                    else 
                    {
                        bool ok1 = false;
                        if (num != null) 
                        {
                            for (Pullenti.Ner.Token tt = dts[0].BeginToken.Previous; tt != null; tt = tt.Previous) 
                            {
                                if (tt.IsCharOf(":,") || tt.IsHiphen || Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                                {
                                }
                                else 
                                {
                                    if (tt.IsValue("ДАЛЕЕ", "ДАЛІ")) 
                                        ok1 = true;
                                    break;
                                }
                            }
                        }
                        if (!ok1) 
                            return null;
                    }
                }
            }
            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dec, dts[0].BeginToken, et);
            if (dec.Slots.Count == 2 && dec.Slots[0].TypeName == DecreeReferent.ATTR_TYPE && dec.Slots[1].TypeName == DecreeReferent.ATTR_NAME) 
            {
                bool err = true;
                for (Pullenti.Ner.Token tt = rt.BeginToken; tt != null && tt.EndChar <= rt.EndChar; tt = tt.Next) 
                {
                    if (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                    {
                    }
                    else if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter && !tt.Chars.IsAllLower) 
                    {
                        err = false;
                        break;
                    }
                }
                if (err) 
                    return null;
            }
            if (morph != null) 
                rt.Morph = morph;
            if (rt.Chars.IsAllLower) 
            {
                if (dec.Typ0 == "ДЕКЛАРАЦИЯ" || dec.Typ0 == "ДЕКЛАРАЦІЯ") 
                    return null;
                if (((dec.Typ0 == "КОНСТИТУЦИЯ" || dec.Typ0 == "КОНСТИТУЦІЯ")) && rt.BeginToken == rt.EndToken) 
                {
                    bool ok1 = false;
                    int cou = 10;
                    for (Pullenti.Ner.Token tt = rt.BeginToken.Previous; tt != null && cou > 0; tt = tt.Previous,cou--) 
                    {
                        if (tt.IsNewlineAfter) 
                            break;
                        Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt, null, false, false);
                        if (pt != null && pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix && pt.EndToken.Next == rt.BeginToken) 
                        {
                            ok1 = true;
                            break;
                        }
                    }
                    if (!ok1) 
                        return null;
                }
            }
            if (num != null && ((num.IndexOf('/') > 0 || num.IndexOf(',') > 0))) 
            {
                int cou = 0;
                foreach (Pullenti.Ner.Slot s in dec.Slots) 
                {
                    if (s.TypeName == DecreeReferent.ATTR_NUMBER) 
                        cou++;
                }
                if (cou == 1) 
                {
                    int owns = 0;
                    foreach (Pullenti.Ner.Slot s in dec.Slots) 
                    {
                        if (s.TypeName == DecreeReferent.ATTR_SOURCE) 
                            owns++;
                    }
                    if (owns > 1) 
                    {
                        string[] nums = num.Split('/');
                        string[] nums2 = num.Split(',');
                        string strNum = null;
                        for (int ii = 0; ii < dts.Count; ii++) 
                        {
                            if (dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            {
                                strNum = dts[ii].GetSourceText();
                                break;
                            }
                        }
                        if (nums2.Length == owns && owns > 1) 
                        {
                            dec.AddSlot(DecreeReferent.ATTR_NUMBER, null, true, 0);
                            foreach (string n in nums2) 
                            {
                                dec.AddSlot(DecreeReferent.ATTR_NUMBER, n.Trim(), false, 0).Tag = strNum;
                            }
                        }
                        else if (nums.Length == owns && owns > 1) 
                        {
                            dec.AddSlot(DecreeReferent.ATTR_NUMBER, null, true, 0);
                            foreach (string n in nums) 
                            {
                                dec.AddSlot(DecreeReferent.ATTR_NUMBER, n.Trim(), false, 0).Tag = strNum;
                            }
                        }
                    }
                }
            }
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(rt.BeginToken.Previous, false, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt.EndToken.Next, false, null, false)) 
            {
                rt.BeginToken = rt.BeginToken.Previous;
                rt.EndToken = rt.EndToken.Next;
                List<Pullenti.Ner.Decree.Internal.DecreeToken> dts1 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(rt.EndToken.Next, null, 10, false);
                if (dts1 != null && dts1[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date && dec.FindSlot(DecreeReferent.ATTR_DATE, null, true) == null) 
                {
                    dec.AddDate(dts1[0]);
                    rt.EndToken = dts1[0].EndToken;
                }
            }
            if (dec.Kind == DecreeKind.Standard && dec.Name == null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(rt.EndToken.Next, true, false)) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(rt.EndToken.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    dec.AddNameStr(Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.KeepRegister));
                    rt.EndToken = br.EndToken;
                }
            }
            if (dec.Kind == DecreeKind.Program && dec.FindSlot(DecreeReferent.ATTR_DATE, null, true) == null) 
            {
                if (rt.BeginToken.Previous != null && rt.BeginToken.Previous.IsValue("ПАСПОРТ", null)) 
                {
                    int cou = 0;
                    for (Pullenti.Ner.Token tt = rt.EndToken.Next; tt != null && (cou < 1000); tt = (tt == null ? null : tt.Next)) 
                    {
                        if (tt.IsValue("СРОК", "ТЕРМІН") && tt.Next != null && tt.Next.IsValue("РЕАЛИЗАЦИЯ", "РЕАЛІЗАЦІЯ")) 
                        {
                        }
                        else 
                            continue;
                        tt = tt.Next.Next;
                        if (tt == null) 
                            break;
                        Pullenti.Ner.Decree.Internal.DecreeToken dtok = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt, null, false);
                        if (dtok != null && dtok.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && ((dtok.Value == "ПРОГРАММА" || dtok.Value == "ПРОГРАМА"))) 
                            tt = dtok.EndToken.Next;
                        for (; tt != null; tt = tt.Next) 
                        {
                            if (tt.IsHiphen || tt.IsTableControlChar || tt.IsValue("ПРОГРАММА", "ПРОГРАМА")) 
                            {
                            }
                            else if (tt.GetReferent() is Pullenti.Ner.Date.DateRangeReferent) 
                            {
                                dec.AddSlot(DecreeReferent.ATTR_DATE, tt.GetReferent(), false, 0);
                                break;
                            }
                            else 
                                break;
                        }
                        break;
                    }
                }
            }
            if (rt.EndToken.Next != null && rt.EndToken.Next.IsChar('(')) 
            {
                Pullenti.Ner.Date.DateReferent dt = null;
                for (Pullenti.Ner.Token tt = rt.EndToken.Next.Next; tt != null; tt = tt.Next) 
                {
                    Pullenti.Ner.Referent r = tt.GetReferent();
                    if (r is Pullenti.Ner.Geo.GeoReferent) 
                        continue;
                    if (r is Pullenti.Ner.Date.DateReferent) 
                    {
                        dt = r as Pullenti.Ner.Date.DateReferent;
                        continue;
                    }
                    if (tt.Morph.Class.IsPreposition) 
                        continue;
                    if (tt.Morph.Class.IsVerb) 
                        continue;
                    if (tt.IsChar(')') && dt != null) 
                    {
                        dec.AddSlot(DecreeReferent.ATTR_DATE, dt, false, 0);
                        rt.EndToken = tt;
                    }
                    break;
                }
            }
            List<Pullenti.Ner.ReferentToken> rtLi = new List<Pullenti.Ner.ReferentToken>();
            if (((i + 1) < dts.Count) && dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Edition && !dts[i].IsNewlineBefore) 
            {
                dts.RemoveRange(0, i + 1);
                List<Pullenti.Ner.ReferentToken> ed = _TryAttach(dts, baseTyp, true, ad);
                if (ed != null && ed.Count > 0) 
                {
                    rtLi.AddRange(ed);
                    foreach (Pullenti.Ner.ReferentToken e in ed) 
                    {
                        dec.AddSlot(DecreeReferent.ATTR_EDITION, e.Referent, false, 0);
                    }
                    rt.EndToken = ed[ed.Count - 1].EndToken;
                }
            }
            else if (((i < (dts.Count - 1)) && i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Edition) && !dts[i - 1].IsNewlineBefore) 
            {
                dts.RemoveRange(0, i);
                List<Pullenti.Ner.ReferentToken> ed = _TryAttach(dts, baseTyp, true, ad);
                if (ed != null && ed.Count > 0) 
                {
                    rtLi.AddRange(ed);
                    foreach (Pullenti.Ner.ReferentToken e in ed) 
                    {
                        dec.AddSlot(DecreeReferent.ATTR_EDITION, e.Referent, false, 0);
                    }
                    rt.EndToken = ed[ed.Count - 1].EndToken;
                }
            }
            Pullenti.Ner.ReferentToken rt22 = DecreeAnalyzer._tryAttachApproved(rt.EndToken.Next, ad, true);
            if (rt22 != null) 
            {
                rt.EndToken = rt22.EndToken;
                DecreeReferent dr00 = rt22.Referent as DecreeReferent;
                if (dr00.Typ == null) 
                {
                    foreach (Pullenti.Ner.Slot s in dr00.Slots) 
                    {
                        if (s.TypeName == DecreeReferent.ATTR_DATE || s.TypeName == DecreeReferent.ATTR_SOURCE) 
                        {
                            if (dec.FindSlot(s.TypeName, null, true) == null) 
                                dec.AddSlot(s.TypeName, s.Value, false, 0);
                        }
                    }
                    dr00 = null;
                }
                if (dr00 != null) 
                {
                    rtLi.Add(rt22);
                    dec.AddSlot(DecreeReferent.ATTR_EDITION, rt22.Referent, false, 0);
                }
            }
            rtLi.Add(rt);
            if (numTok != null && numTok.Children != null) 
            {
                Pullenti.Ner.Token end = rt.EndToken;
                rt.EndToken = numTok.Children[0].BeginToken.Previous;
                if (rt.EndToken.IsCommaAnd) 
                    rt.EndToken = rt.EndToken.Previous;
                for (int ii = 0; ii < numTok.Children.Count; ii++) 
                {
                    DecreeReferent dr1 = new DecreeReferent();
                    foreach (Pullenti.Ner.Slot s in rt.Referent.Slots) 
                    {
                        if (s.TypeName == DecreeReferent.ATTR_NUMBER) 
                            dr1.AddSlot(s.TypeName, numTok.Children[ii].Value, false, 0).Tag = numTok.Children[ii].GetSourceText();
                        else 
                        {
                            Pullenti.Ner.Slot ss = dr1.AddSlot(s.TypeName, s.Value, false, 0);
                            if (ss != null) 
                                ss.Tag = s.Tag;
                        }
                    }
                    Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(dr1, numTok.Children[ii].BeginToken, numTok.Children[ii].EndToken);
                    if (ii == (numTok.Children.Count - 1)) 
                        rt1.EndToken = end;
                    rtLi.Add(rt1);
                }
            }
            if ((dts.Count == 2 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dts[0].TypKind == DecreeKind.Standard) && dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
            {
                for (Pullenti.Ner.Token ttt = dts[1].EndToken.Next; ttt != null; ttt = ttt.Next) 
                {
                    if (!ttt.IsCommaAnd) 
                        break;
                    Pullenti.Ner.Decree.Internal.DecreeToken nu = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(ttt.Next, dts[0], false);
                    if (nu == null || nu.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                        break;
                    DecreeReferent dr1 = new DecreeReferent() { Typ = dec.Typ };
                    dr1.AddNumber(nu);
                    rtLi.Add(new Pullenti.Ner.ReferentToken(dr1, ttt.Next, nu.EndToken));
                    if (!ttt.IsComma) 
                        break;
                    ttt = nu.EndToken;
                }
            }
            return rtLi;
        }
        /// <summary>
        /// Имя анализатора ("DECREE")
        /// </summary>
        public const string ANALYZER_NAME = "DECREE";
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Законы и указы";
            }
        }
        public override string Description
        {
            get
            {
                return "Законы, указы, постановления, распоряжения и т.п.";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new DecreeAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Decree.Internal.MetaDecree.GlobalMeta, Pullenti.Ner.Decree.Internal.MetaDecreePart.GlobalMeta, Pullenti.Ner.Decree.Internal.MetaDecreeChange.GlobalMeta, Pullenti.Ner.Decree.Internal.MetaDecreeChangeValue.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecree.DecreeImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("decree.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecree.StandadrImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("decreestd.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecreePart.PartImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("part.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecreePart.PartLocImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("document_into.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecree.PublishImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("publish.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecreeChange.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("decreechange.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecreeChangeValue.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("decreechangevalue.png"));
                return res;
            }
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {Pullenti.Ner.Date.DateReferent.OBJ_TYPENAME, Pullenti.Ner.Geo.GeoReferent.OBJ_TYPENAME, Pullenti.Ner.Org.OrganizationReferent.OBJ_TYPENAME, Pullenti.Ner.Person.PersonReferent.OBJ_TYPENAME};
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == DecreeReferent.OBJ_TYPENAME) 
                return new DecreeReferent();
            if (type == DecreePartReferent.OBJ_TYPENAME) 
                return new DecreePartReferent();
            if (type == DecreeChangeReferent.OBJ_TYPENAME) 
                return new DecreeChangeReferent();
            if (type == DecreeChangeValueReferent.OBJ_TYPENAME) 
                return new DecreeChangeValueReferent();
            return null;
        }
        public override int ProgressWeight
        {
            get
            {
                return 10;
            }
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            Pullenti.Ner.Decree.Internal.DecreeToken baseTyp = null;
            Pullenti.Ner.Referent ref0 = null;
            Pullenti.Ner.Core.TerminCollection aliases = new Pullenti.Ner.Core.TerminCollection();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r == null) 
                    continue;
                if (!(r is Pullenti.Ner.Org.OrganizationReferent)) 
                    continue;
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                if (!rt.BeginToken.Chars.IsAllUpper || rt.BeginToken.LengthChar > 4) 
                    continue;
                Pullenti.Ner.Decree.Internal.DecreeToken dtr = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(rt.BeginToken, null, false);
                if (dtr == null || dtr.TypKind != DecreeKind.Kodex) 
                    continue;
                if (rt.BeginToken == rt.EndToken) 
                {
                }
                else if (rt.BeginToken.Next == rt.EndToken && (rt.EndToken.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                }
                else 
                    continue;
                t = kit.DebedToken(rt);
            }
            int lastDecDist = 0;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next,lastDecDist++) 
            {
                List<Pullenti.Ner.Decree.Internal.DecreeToken> dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(t, null, 10, lastDecDist > 1000);
                Pullenti.Ner.Core.TerminToken tok = aliases.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null && tok.BeginToken == tok.EndToken && tok.Chars.IsAllLower) 
                {
                    bool ok = false;
                    for (Pullenti.Ner.Token tt = t.Previous; tt != null && ((t.EndChar - tt.EndChar) < 20); tt = tt.Previous) 
                    {
                        Pullenti.Ner.Decree.Internal.PartToken p = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt, null, false, false);
                        if (p != null && p.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix && p.EndToken.Next == t) 
                        {
                            ok = true;
                            break;
                        }
                    }
                    if (!ok) 
                        tok = null;
                }
                if (tok != null) 
                {
                    Pullenti.Ner.ReferentToken rt0 = TryAttachApproved(t, ad);
                    if (rt0 != null) 
                        tok = null;
                }
                if (tok != null) 
                {
                    DecreeReferent dec0 = tok.Termin.Tag as DecreeReferent;
                    Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(tok.Termin.Tag as Pullenti.Ner.Referent, tok.BeginToken, tok.EndToken);
                    if (dec0 != null && (rt0.EndToken.Next is Pullenti.Ner.ReferentToken) && (rt0.EndToken.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                        Pullenti.Ner.Geo.GeoReferent geo0 = dec0.GetSlotValue(DecreeReferent.ATTR_GEO) as Pullenti.Ner.Geo.GeoReferent;
                        Pullenti.Ner.Geo.GeoReferent geo1 = rt0.EndToken.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                        if (geo0 == null) 
                        {
                            dec0.AddSlot(DecreeReferent.ATTR_GEO, geo1, false, 0);
                            rt0.EndToken = rt0.EndToken.Next;
                        }
                        else if (geo0 == geo1) 
                            rt0.EndToken = rt0.EndToken.Next;
                        else 
                            continue;
                    }
                    kit.EmbedToken(rt0);
                    t = rt0;
                    rt0.MiscAttrs = 1;
                    lastDecDist = 0;
                    continue;
                }
                if (dts == null || dts.Count == 0 || ((dts.Count == 1 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ))) 
                {
                    Pullenti.Ner.ReferentToken rt0 = TryAttachApproved(t, ad);
                    if (rt0 != null) 
                    {
                        rt0.Referent = ad.RegisterReferent(rt0.Referent);
                        Pullenti.Ner.MetaToken mt = _checkAliasAfter(rt0.EndToken.Next);
                        if (mt != null) 
                        {
                            if (aliases != null) 
                            {
                                Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                                term.InitBy(mt.BeginToken, mt.EndToken.Previous, rt0.Referent, false);
                                aliases.Add(term);
                            }
                            rt0.EndToken = mt.EndToken;
                        }
                        else if ((((mt = rt0.Tag as Pullenti.Ner.MetaToken))) != null) 
                        {
                            if (aliases != null) 
                            {
                                Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                                term.InitBy(mt.BeginToken, mt.EndToken.Previous, rt0.Referent, false);
                                aliases.Add(term);
                            }
                        }
                        kit.EmbedToken(rt0);
                        lastDecDist = 0;
                        t = rt0;
                        continue;
                    }
                    if (dts == null || dts.Count == 0) 
                        continue;
                }
                if (dts[0].IsNewlineAfter && dts[0].IsNewlineBefore) 
                {
                    bool ignore = false;
                    if (t == kit.FirstToken) 
                        ignore = true;
                    else if ((dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org && dts.Count > 1 && dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) && dts[1].IsWhitespaceAfter) 
                        ignore = true;
                    if (ignore) 
                    {
                        t = dts[dts.Count - 1].EndToken;
                        continue;
                    }
                }
                if (baseTyp == null) 
                {
                    foreach (Pullenti.Ner.Decree.Internal.DecreeToken dd in dts) 
                    {
                        if (dd.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        {
                            baseTyp = dd;
                            break;
                        }
                    }
                }
                if (dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dts[0].Value) == DecreeKind.Publisher) 
                {
                    List<Pullenti.Ner.ReferentToken> rts = this.TryAttachPulishers(dts);
                    if (rts != null) 
                    {
                        for (int i = 0; i < rts.Count; i++) 
                        {
                            Pullenti.Ner.ReferentToken rtt = rts[i];
                            if (rtt.Referent is DecreePartReferent) 
                                (rtt.Referent as DecreePartReferent).Owner = ad.RegisterReferent((rtt.Referent as DecreePartReferent).Owner) as DecreeReferent;
                            rtt.Referent = ad.RegisterReferent(rtt.Referent);
                            kit.EmbedToken(rtt);
                            t = rtt;
                            if ((rtt.Referent is DecreeReferent) && ((i + 1) < rts.Count) && (rts[i + 1].Referent is DecreePartReferent)) 
                                rts[i + 1].BeginToken = t;
                            lastDecDist = 0;
                        }
                        Pullenti.Ner.MetaToken mt = _checkAliasAfter(t.Next);
                        if (mt != null) 
                        {
                            for (Pullenti.Ner.Token tt = dts[0].BeginToken.Previous; tt != null; tt = tt.Previous) 
                            {
                                if (tt.IsComma) 
                                    continue;
                                DecreeReferent d = tt.GetReferent() as DecreeReferent;
                                if (d != null) 
                                {
                                    if (aliases != null) 
                                    {
                                        Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                                        term.InitBy(mt.BeginToken, mt.EndToken.Previous, d, false);
                                        aliases.Add(term);
                                    }
                                    t = mt.EndToken;
                                }
                                break;
                            }
                        }
                    }
                    continue;
                }
                List<Pullenti.Ner.ReferentToken> rtli = TryAttach(dts, baseTyp, ad);
                if (rtli == null || ((rtli.Count == 1 && (dts.Count < 3) && dts[0].Value == "РЕГЛАМЕНТ"))) 
                {
                    Pullenti.Ner.ReferentToken rt = TryAttachApproved(t, ad);
                    if (rt != null) 
                    {
                        rtli = new List<Pullenti.Ner.ReferentToken>();
                        rtli.Add(rt);
                    }
                }
                if (rtli != null) 
                {
                    for (int ii = 0; ii < rtli.Count; ii++) 
                    {
                        Pullenti.Ner.ReferentToken rt = rtli[ii];
                        lastDecDist = 0;
                        rt.Referent = ad.RegisterReferent(rt.Referent);
                        Pullenti.Ner.MetaToken mt = _checkAliasAfter(rt.EndToken.Next);
                        if (mt != null) 
                        {
                            if (aliases != null) 
                            {
                                Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                                term.InitBy(mt.BeginToken, mt.EndToken.Previous, rt.Referent, false);
                                aliases.Add(term);
                            }
                            rt.EndToken = mt.EndToken;
                        }
                        else if ((((mt = rt.Tag as Pullenti.Ner.MetaToken))) != null) 
                        {
                            if (aliases != null) 
                            {
                                Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                                term.InitBy(mt.BeginToken, mt.EndToken.Previous, rt.Referent, false);
                                aliases.Add(term);
                            }
                        }
                        ref0 = rt.Referent;
                        kit.EmbedToken(rt);
                        t = rt;
                        if ((ii + 1) < rtli.Count) 
                        {
                            if (rt.EndToken.Next == rtli[ii + 1].BeginToken) 
                                rtli[ii + 1].BeginToken = rt;
                        }
                    }
                }
                else if (dts.Count == 1 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    if (dts[0].Chars.IsCapitalUpper && !dts[0].IsDoubtful) 
                    {
                        lastDecDist = 0;
                        if (baseTyp != null && dts[0].Ref != null) 
                        {
                            DecreeReferent drr = dts[0].Ref.GetReferent() as DecreeReferent;
                            if (drr != null) 
                            {
                                if (baseTyp.Value == drr.Typ0 || baseTyp.Value == drr.Typ) 
                                    continue;
                            }
                        }
                        Pullenti.Ner.ReferentToken rt0 = Pullenti.Ner.Decree.Internal.DecreeToken._findBackTyp(dts[0].BeginToken.Previous, dts[0].Value);
                        if (rt0 != null) 
                        {
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(rt0.Referent, dts[0].BeginToken, dts[0].EndToken);
                            kit.EmbedToken(rt);
                            t = rt;
                            rt.Tag = rt0.Referent;
                        }
                    }
                }
            }
            if (ad.Referents.Count > 0) 
            {
                for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
                {
                    DecreeReferent dr = t.GetReferent() as DecreeReferent;
                    if (dr == null) 
                        continue;
                    List<DecreeReferent> li = null;
                    for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                    {
                        if (!tt.IsCommaAnd) 
                            break;
                        if (tt.Next == null || !(tt.Next.GetReferent() is DecreeReferent)) 
                            break;
                        if (li == null) 
                        {
                            li = new List<DecreeReferent>();
                            li.Add(dr);
                        }
                        dr = tt.Next.GetReferent() as DecreeReferent;
                        li.Add(dr);
                        dr.Tag = null;
                        tt = tt.Next;
                        if (dr.Date != null) 
                        {
                            List<Pullenti.Ner.Decree.Internal.DecreeToken> dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList((tt as Pullenti.Ner.ReferentToken).BeginToken, null, 10, false);
                            if (dts != null) 
                            {
                                foreach (Pullenti.Ner.Decree.Internal.DecreeToken dt in dts) 
                                {
                                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                                        dr.Tag = dr;
                                }
                            }
                        }
                    }
                    if (li == null) 
                        continue;
                    int i;
                    for (i = li.Count - 1; i > 0; i--) 
                    {
                        if (li[i].Typ == li[i - 1].Typ) 
                        {
                            if (li[i].Date != null && li[i].Tag != null && li[i - 1].Date == null) 
                                li[i - 1].AddSlot(DecreeReferent.ATTR_DATE, li[i].GetSlotValue(DecreeReferent.ATTR_DATE), false, 0);
                        }
                    }
                    for (i = 0; i < (li.Count - 1); i++) 
                    {
                        if (li[i].Typ == li[i + 1].Typ) 
                        {
                            Pullenti.Ner.Slot sl = li[i].FindSlot(DecreeReferent.ATTR_SOURCE, null, true);
                            if (sl != null && li[i + 1].FindSlot(DecreeReferent.ATTR_SOURCE, null, true) == null) 
                                li[i + 1].AddSlot(sl.TypeName, sl.Value, false, 0);
                        }
                    }
                    for (i = 0; i < li.Count; i++) 
                    {
                        if (li[i].Name != null) 
                            break;
                    }
                    if (i == (li.Count - 1)) 
                    {
                        for (i = li.Count - 1; i > 0; i--) 
                        {
                            if (li[i - 1].Typ == li[i].Typ) 
                                li[i - 1].AddName(li[i]);
                        }
                    }
                }
            }
            List<DecreePartReferent> undefinedDecrees = new List<DecreePartReferent>();
            DecreeChangeReferent rootChange = null;
            DecreeChangeReferent lastChange = null;
            List<Pullenti.Ner.Referent> changeStack = new List<Pullenti.Ner.Referent>();
            bool expireRegime = false;
            int hasStartChange = 0;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                List<Pullenti.Ner.Decree.Internal.PartToken> dts = null;
                if (t.IsNewlineBefore && (t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).Value == "25") 
                {
                }
                Pullenti.Ner.Decree.Internal.DecreeChangeToken dcht = null;
                if (t.IsNewlineBefore) 
                    dcht = Pullenti.Ner.Decree.Internal.DecreeChangeToken.TryAttach(t, rootChange, false, changeStack, false);
                if (dcht != null && dcht.IsStart) 
                {
                    if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartMultu) 
                    {
                        expireRegime = false;
                        hasStartChange = 3;
                        rootChange = null;
                    }
                    else if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Single) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeChangeToken dcht1 = Pullenti.Ner.Decree.Internal.DecreeChangeToken.TryAttach(dcht.EndToken.Next, rootChange, false, changeStack, false);
                        if (dcht1 != null && dcht1.IsStart) 
                        {
                            hasStartChange = 2;
                            if (dcht.DecreeTok != null && dcht.Decree != null) 
                            {
                                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dcht.Decree, dcht.DecreeTok.BeginToken, dcht.DecreeTok.EndToken);
                                kit.EmbedToken(rt);
                                t = rt;
                                if (dcht.EndChar == t.EndChar) 
                                    dcht.EndToken = t;
                            }
                        }
                    }
                    else if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartSingle && dcht.Decree != null && !expireRegime) 
                    {
                        expireRegime = false;
                        hasStartChange = 2;
                        if (dcht.DecreeTok != null) 
                        {
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dcht.Decree, dcht.DecreeTok.BeginToken, dcht.DecreeTok.EndToken);
                            kit.EmbedToken(rt);
                            t = rt;
                            if (dcht.EndChar == t.EndChar) 
                                dcht.EndToken = t;
                        }
                        else 
                            rootChange = null;
                    }
                    if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartSingle && rootChange != null && dcht.Decree == null) 
                        hasStartChange = 2;
                    else if ((dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Single && dcht.Decree != null && dcht.EndToken.IsChar(':')) && dcht.IsNewlineAfter) 
                        hasStartChange = 2;
                    if (hasStartChange <= 0) 
                    {
                        dts = Pullenti.Ner.Decree.Internal.PartToken.TryAttachList(t, false, 40);
                        changeStack.Clear();
                    }
                    else 
                    {
                        if (dcht.Decree != null) 
                        {
                            changeStack.Clear();
                            changeStack.Add(dcht.Decree);
                        }
                        else if (dcht.ActKind == DecreeChangeKind.Expire && dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartMultu) 
                            expireRegime = true;
                        dts = dcht.Parts;
                    }
                }
                else 
                {
                    dts = Pullenti.Ner.Decree.Internal.PartToken.TryAttachList(t, false, 40);
                    if (dcht == null && t.IsNewlineBefore) 
                    {
                        expireRegime = false;
                        hasStartChange--;
                    }
                }
                if (dts != null) 
                {
                }
                List<Pullenti.Ner.MetaToken> rts = TryAttachParts(dts, baseTyp, (hasStartChange > 0 && changeStack.Count > 0 ? changeStack[0] : null));
                if (rts != null) 
                {
                }
                List<DecreePartReferent> dprs = null;
                Dictionary<DecreePartReferent, DecreePartReferent> diaps = null;
                Dictionary<int, Pullenti.Ner.Token> begs = null;
                Dictionary<int, Pullenti.Ner.Token> ends = null;
                if (rts != null) 
                {
                    foreach (Pullenti.Ner.MetaToken kp in rts) 
                    {
                        List<DecreePartReferent> dprList = kp.Tag as List<DecreePartReferent>;
                        if (dprList == null) 
                            continue;
                        for (int i = 0; i < dprList.Count; i++) 
                        {
                            DecreePartReferent dr = dprList[i];
                            if (dr.Owner == null && dr.Clause != null && dr.LocalTyp == null) 
                            {
                                if (!undefinedDecrees.Contains(dr)) 
                                    undefinedDecrees.Add(dr);
                            }
                            if (dr.Owner != null && dr.Clause != null) 
                            {
                                foreach (DecreePartReferent d in undefinedDecrees) 
                                {
                                    d.Owner = dr.Owner;
                                }
                                undefinedDecrees.Clear();
                            }
                            if (dcht != null && changeStack.Count > 0) 
                            {
                                while (changeStack.Count > 0) 
                                {
                                    if (dr.IsAllItemsLessLevel(changeStack[0], false)) 
                                    {
                                        if (changeStack[0] is DecreePartReferent) 
                                            dr.AddHighLevelInfo(changeStack[0] as DecreePartReferent);
                                        break;
                                    }
                                    if (changeStack[0] is DecreePartReferent) 
                                        changeStack.RemoveAt(0);
                                }
                            }
                            if (lastChange != null && lastChange.Owners.Count > 0) 
                            {
                                DecreePartReferent dr0 = lastChange.Owners[0] as DecreePartReferent;
                                if (dr0 != null && dr.Owner == dr0.Owner) 
                                {
                                    int mle = dr.GetMinLevel();
                                    if (mle == 0 || mle <= Pullenti.Ner.Decree.Internal.PartToken._getRank(Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause)) 
                                    {
                                    }
                                    else 
                                        dr.AddHighLevelInfo(dr0);
                                }
                            }
                            dr = ad.RegisterReferent(dr) as DecreePartReferent;
                            if (dprs == null) 
                                dprs = new List<DecreePartReferent>();
                            dprs.Add(dr);
                            Pullenti.Ner.ReferentToken rt;
                            if (i == 0) 
                                rt = new Pullenti.Ner.ReferentToken(dr, kp.BeginToken, kp.EndToken);
                            else 
                                rt = new Pullenti.Ner.ReferentToken(dr, t, t);
                            kit.EmbedToken(rt);
                            t = rt;
                            if (dprs.Count == 2 && t.Previous != null && t.Previous.IsHiphen) 
                            {
                                if (diaps == null) 
                                    diaps = new Dictionary<DecreePartReferent, DecreePartReferent>();
                                if (!diaps.ContainsKey(dprs[0])) 
                                    diaps.Add(dprs[0], dprs[1]);
                            }
                            if (begs == null) 
                                begs = new Dictionary<int, Pullenti.Ner.Token>();
                            if (!begs.ContainsKey(t.BeginChar)) 
                                begs.Add(t.BeginChar, t);
                            else 
                                begs[t.BeginChar] = t;
                            if (ends == null) 
                                ends = new Dictionary<int, Pullenti.Ner.Token>();
                            if (!ends.ContainsKey(t.EndChar)) 
                                ends.Add(t.EndChar, t);
                            else 
                                ends[t.EndChar] = t;
                            if (dcht != null) 
                            {
                                if (dcht.BeginChar == t.BeginChar) 
                                    dcht.BeginToken = t;
                                if (dcht.EndChar == t.EndChar) 
                                    dcht.EndToken = t;
                                if (t.EndChar > dcht.EndChar) 
                                    dcht.EndToken = t;
                            }
                        }
                    }
                }
                if (dts != null && dts.Count > 0 && dts[dts.Count - 1].EndChar > t.EndChar) 
                    t = dts[dts.Count - 1].EndToken;
                if (dcht != null && hasStartChange > 0) 
                {
                    if (dcht.EndChar > t.EndChar) 
                        t = dcht.EndToken;
                    List<Pullenti.Ner.ReferentToken> chrt = null;
                    if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartMultu) 
                    {
                        rootChange = null;
                        changeStack.Clear();
                        if (dcht.Decree != null) 
                            changeStack.Add(dcht.Decree);
                        if (dprs != null && dprs.Count > 0) 
                        {
                            if (changeStack.Count == 0 && dprs[0].Owner != null) 
                                changeStack.Add(dprs[0].Owner);
                            changeStack.Insert(0, dprs[0]);
                        }
                        if (changeStack.Count > 0 || dcht.Decree != null) 
                        {
                            rootChange = ad.RegisterReferent(new DecreeChangeReferent() { Kind = DecreeChangeKind.Container }) as DecreeChangeReferent;
                            if (changeStack.Count > 0) 
                                rootChange.AddSlot(DecreeChangeReferent.ATTR_OWNER, changeStack[0], false, 0);
                            else 
                                rootChange.AddSlot(DecreeChangeReferent.ATTR_OWNER, dcht.Decree, false, 0);
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(rootChange, dcht.BeginToken, dcht.EndToken);
                            if (rt.EndToken.IsChar(':')) 
                                rt.EndToken = rt.EndToken.Previous;
                            kit.EmbedToken(rt);
                            t = rt;
                            if (t.Next != null && t.Next.IsChar(':')) 
                                t = t.Next;
                        }
                        continue;
                    }
                    if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Single && dprs != null && dprs.Count == 1) 
                    {
                        while (changeStack.Count > 0) 
                        {
                            if (dprs[0].IsAllItemsLessLevel(changeStack[0], true)) 
                                break;
                            else 
                                changeStack.RemoveAt(0);
                        }
                        changeStack.Insert(0, dprs[0]);
                        if (dprs[0].Owner != null && changeStack[changeStack.Count - 1] != dprs[0].Owner) 
                        {
                            changeStack.Clear();
                            changeStack.Insert(0, dprs[0].Owner);
                            changeStack.Insert(0, dprs[0]);
                        }
                        continue;
                    }
                    if (dprs == null && dcht.RealPart != null) 
                    {
                        dprs = new List<DecreePartReferent>();
                        dprs.Add(dcht.RealPart);
                    }
                    if (dprs != null && dprs.Count > 0) 
                    {
                        chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(dprs[0], dcht);
                        if (chrt == null && expireRegime) 
                        {
                            chrt = new List<Pullenti.Ner.ReferentToken>();
                            DecreeChangeReferent dcr = new DecreeChangeReferent() { Kind = DecreeChangeKind.Expire };
                            chrt.Add(new Pullenti.Ner.ReferentToken(dcr, dcht.BeginToken, dcht.EndToken));
                        }
                    }
                    else if (dcht.ActKind == DecreeChangeKind.Append) 
                    {
                        bool ee = false;
                        if (dcht.PartTyp != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Undefined) 
                        {
                            foreach (Pullenti.Ner.Referent ss in changeStack) 
                            {
                                if (ss is DecreePartReferent) 
                                {
                                    if ((ss as DecreePartReferent).IsAllItemsOverThisLevel(dcht.PartTyp)) 
                                    {
                                        ee = true;
                                        chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(ss, dcht);
                                        break;
                                    }
                                }
                                else if (ss is DecreeReferent) 
                                {
                                    ee = true;
                                    chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(ss, dcht);
                                    break;
                                }
                            }
                        }
                        if (lastChange != null && !ee && lastChange.Owners.Count > 0) 
                            chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(lastChange.Owners[0], dcht);
                    }
                    if (dprs == null && ((dcht.HasName || dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Value || dcht.ChangeVal != null)) && changeStack.Count > 0) 
                        chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(changeStack[0], dcht);
                    if ((chrt == null && ((expireRegime || dcht.ActKind == DecreeChangeKind.Expire)) && dcht.Decree != null) && dprs == null) 
                    {
                        chrt = new List<Pullenti.Ner.ReferentToken>();
                        DecreeChangeReferent dcr = new DecreeChangeReferent() { Kind = DecreeChangeKind.Expire };
                        dcr.AddSlot(DecreeChangeReferent.ATTR_OWNER, dcht.Decree, false, 0);
                        chrt.Add(new Pullenti.Ner.ReferentToken(dcr, dcht.BeginToken, dcht.EndToken));
                        for (Pullenti.Ner.Token tt = dcht.EndToken.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.Next == null) 
                                break;
                            if (tt.IsChar('(')) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null) 
                                {
                                    tt = br.EndToken;
                                    chrt[chrt.Count - 1].EndToken = tt;
                                    continue;
                                }
                            }
                            if (!tt.IsCommaAnd && !tt.IsChar(';')) 
                                break;
                            tt = tt.Next;
                            if (tt.GetReferent() is DecreeReferent) 
                            {
                                dcr = new DecreeChangeReferent() { Kind = DecreeChangeKind.Expire };
                                dcr.AddSlot(DecreeChangeReferent.ATTR_OWNER, tt.GetReferent(), false, 0);
                                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dcr, tt, tt);
                                if (tt.Next != null && tt.Next.IsChar('(')) 
                                {
                                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                    if (br != null) 
                                        rt.EndToken = (tt = br.EndToken);
                                }
                                chrt.Add(rt);
                                continue;
                            }
                            break;
                        }
                    }
                    if (chrt != null) 
                    {
                        foreach (Pullenti.Ner.ReferentToken rt in chrt) 
                        {
                            rt.Referent = ad.RegisterReferent(rt.Referent);
                            if (rt.Referent is DecreeChangeReferent) 
                            {
                                lastChange = rt.Referent as DecreeChangeReferent;
                                if (dprs != null) 
                                {
                                    int ii;
                                    for (ii = 0; ii < (dprs.Count - 1); ii++) 
                                    {
                                        lastChange.AddSlot(DecreeChangeReferent.ATTR_OWNER, dprs[ii], false, 0);
                                    }
                                    if (diaps != null) 
                                    {
                                        foreach (KeyValuePair<DecreePartReferent, DecreePartReferent> kp in diaps) 
                                        {
                                            List<DecreePartReferent> diap = Pullenti.Ner.Decree.Internal.PartToken.TryCreateBetween(kp.Key, kp.Value);
                                            if (diap != null) 
                                            {
                                                foreach (DecreePartReferent d in diap) 
                                                {
                                                    Pullenti.Ner.Referent dd = ad.RegisterReferent(d);
                                                    lastChange.AddSlot(DecreeChangeReferent.ATTR_OWNER, dd, false, 0);
                                                }
                                            }
                                        }
                                    }
                                    for (; ii < dprs.Count; ii++) 
                                    {
                                        lastChange.AddSlot(DecreeChangeReferent.ATTR_OWNER, dprs[ii], false, 0);
                                    }
                                }
                            }
                            if (begs != null && begs.ContainsKey(rt.BeginChar)) 
                                rt.BeginToken = begs[rt.BeginChar];
                            if (ends != null && ends.ContainsKey(rt.EndChar)) 
                                rt.EndToken = ends[rt.EndChar];
                            if (rootChange != null && (rt.Referent is DecreeChangeReferent)) 
                                rootChange.AddSlot(DecreeChangeReferent.ATTR_CHILD, rt.Referent, false, 0);
                            kit.EmbedToken(rt);
                            t = rt;
                            if (begs == null) 
                                begs = new Dictionary<int, Pullenti.Ner.Token>();
                            if (!begs.ContainsKey(t.BeginChar)) 
                                begs.Add(t.BeginChar, t);
                            else 
                                begs[t.BeginChar] = t;
                            if (ends == null) 
                                ends = new Dictionary<int, Pullenti.Ner.Token>();
                            if (!ends.ContainsKey(t.EndChar)) 
                                ends.Add(t.EndChar, t);
                            else 
                                ends[t.EndChar] = t;
                        }
                    }
                }
            }
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (t.Tag != null && (t is Pullenti.Ner.ReferentToken) && (t.Tag is DecreeReferent)) 
                {
                    t = kit.DebedToken(t);
                    if (t == null) 
                        break;
                }
            }
        }
        internal static Pullenti.Ner.MetaToken _checkAliasAfter(Pullenti.Ner.Token t)
        {
            if ((t != null && t.IsChar('<') && t.Next != null) && t.Next.Next != null && t.Next.Next.IsChar('>')) 
                t = t.Next.Next.Next;
            if (t == null || t.Next == null || !t.IsChar('(')) 
                return null;
            t = t.Next;
            if (t.IsValue("ДАЛЕЕ", "ДАЛІ")) 
            {
            }
            else 
                return null;
            t = t.Next;
            if (t != null && !t.Chars.IsLetter) 
                t = t.Next;
            if (t == null) 
                return null;
            Pullenti.Ner.Token t1 = null;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore) 
                    break;
                else if (tt.IsChar(')')) 
                {
                    t1 = tt.Previous;
                    break;
                }
            }
            if (t1 == null) 
                return null;
            return new Pullenti.Ner.MetaToken(t, t1.Next);
        }
        internal static Pullenti.Ner.ReferentToken TryAttachApproved(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerData ad)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Core.BracketSequenceToken br = null;
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
            else if ((t.Previous is Pullenti.Ner.TextToken) && t.Previous.LengthChar == 1 && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Previous, true, false)) 
                br = Pullenti.Ner.Core.BracketHelper.TryParse(t.Previous, Pullenti.Ner.Core.BracketParseAttr.No, 100);
            if (br != null && br.LengthChar > 20) 
            {
                Pullenti.Ner.ReferentToken rt0 = _tryAttachApproved(br.EndToken.Next, ad, false);
                if (rt0 != null) 
                {
                    DecreeReferent dr = rt0.Referent as DecreeReferent;
                    rt0.BeginToken = br.BeginToken;
                    string nam = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                    if (dr.Typ == null) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(br.BeginToken.Next, null, false);
                        if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        {
                            dr.Typ = dt.Value;
                            if (dt.EndToken.Next != null && dt.EndToken.Next.IsValue("О", null)) 
                                nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(dt.EndToken.Next, br.EndToken, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        }
                    }
                    if (nam != null) 
                        dr.AddNameStr(nam);
                    return rt0;
                }
            }
            if (!t.Chars.IsCyrillicLetter || t.Chars.IsAllLower) 
                return null;
            Pullenti.Ner.Token tt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false);
            if (tt == null || tt.Next == null) 
                return null;
            int cou = 0;
            Pullenti.Ner.MetaToken alias = null;
            Pullenti.Ner.Token aliasT0 = null;
            for (tt = tt.Next; tt != null; tt = tt.Next) 
            {
                if ((++cou) > 100) 
                    break;
                if (tt.IsNewlineBefore) 
                {
                    if (tt.IsValue("ИСТОЧНИК", null)) 
                        break;
                }
                if ((((tt is Pullenti.Ner.NumberToken) && (tt as Pullenti.Ner.NumberToken).Value == "1")) || tt.IsValue("ДРУГОЙ", null)) 
                {
                    if (tt.Next != null && tt.Next.IsValue("СТОРОНА", null)) 
                        return null;
                }
                if (tt.WhitespacesBeforeCount > 15) 
                    break;
                if (tt.IsChar('(')) 
                {
                    Pullenti.Ner.MetaToken mt = _checkAliasAfter(tt);
                    if (mt != null) 
                    {
                        aliasT0 = tt;
                        alias = mt;
                        tt = mt.EndToken;
                        continue;
                    }
                }
                if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt, false) != null && tt.Chars.IsCapitalUpper) 
                    break;
                Pullenti.Ner.ReferentToken rt0 = _tryAttachApproved(tt, ad, true);
                if (rt0 != null) 
                {
                    Pullenti.Ner.Token t1 = tt.Previous;
                    if (aliasT0 != null) 
                        t1 = aliasT0.Previous;
                    string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                    Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                    if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && (rt0.Referent as DecreeReferent).Typ == null) 
                    {
                        (rt0.Referent as DecreeReferent).Typ = dt.Value;
                        if (dt.EndToken.Next != null && dt.EndToken.Next.IsValue("О", "ПРО")) 
                            nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(dt.EndToken.Next, t1, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                    }
                    DecreeReferent dec = rt0.Referent as DecreeReferent;
                    if (nam != null) 
                        dec.AddNameStr(nam);
                    rt0.BeginToken = t;
                    rt0.Tag = alias;
                    if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) == null) 
                    {
                        if (t.Previous != null && t.Previous.IsValue("В", null) && (t.Previous.Previous is Pullenti.Ner.ReferentToken)) 
                        {
                            if (t.Previous.Previous.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) 
                                dec.AddSlot(DecreeReferent.ATTR_SOURCE, t.Previous.Previous.GetReferent(), false, 0);
                        }
                    }
                    return rt0;
                }
                if (tt.IsChar('.')) 
                    break;
                if (tt.IsNewlineBefore && tt.Previous != null && tt.Previous.IsChar('.')) 
                    break;
            }
            return null;
        }
        internal static Pullenti.Ner.ReferentToken _tryAttachApproved(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerData ad, bool mustBeComma = true)
        {
            if (t == null || t.Next == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            if (!t.IsCharOf("(,")) 
            {
                if (mustBeComma) 
                    return null;
            }
            else 
                t = t.Next;
            bool ok = false;
            for (; t != null; t = t.Next) 
            {
                if (t.IsCommaAnd || t.Morph.Class.IsPreposition) 
                    continue;
                if ((t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && (t.GetReferent() as Pullenti.Ner.Geo.GeoReferent).IsCity) 
                    continue;
                if ((((((((t.IsValue("УТВ", null) || t.IsValue("УТВЕРЖДАТЬ", "СТВЕРДЖУВАТИ") || t.IsValue("УТВЕРДИТЬ", "ЗАТВЕРДИТИ")) || t.IsValue("УТВЕРЖДЕННЫЙ", "ЗАТВЕРДЖЕНИЙ") || t.IsValue("ЗАТВЕРДЖУВАТИ", null)) || t.IsValue("СТВЕРДИТИ", null) || t.IsValue("ЗАТВЕРДИТИ", null)) || t.IsValue("ПРИНЯТЬ", "ПРИЙНЯТИ") || t.IsValue("ПРИНЯТЫЙ", "ПРИЙНЯТИЙ")) || t.IsValue("ВВОДИТЬ", "ВВОДИТИ") || t.IsValue("ВВЕСТИ", null)) || t.IsValue("ВВЕДЕННЫЙ", "ВВЕДЕНИЙ") || t.IsValue("ПОДПИСАТЬ", "ПІДПИСАТИ")) || t.IsValue("ПОДПИСЫВАТЬ", "ПІДПИСУВАТИ") || t.IsValue("ЗАКЛЮЧИТЬ", "УКЛАСТИ")) || t.IsValue("ЗАКЛЮЧАТЬ", "УКЛАДАТИ")) 
                {
                    ok = true;
                    if (t.Next != null && t.Next.IsChar('.')) 
                        t = t.Next;
                }
                else if (t.IsValue("ДЕЙСТВИЕ", null) || t.IsValue("ДІЯ", null)) 
                {
                }
                else 
                    break;
            }
            if (!ok) 
                return null;
            if (t == null) 
                return null;
            Pullenti.Ner.Core.AnalysisKit kit = t.Kit;
            object olev = null;
            int lev = 0;
            if (!kit.MiscData.TryGetValue("dovr", out olev)) 
                kit.MiscData.Add("dovr", (lev = 1));
            else 
            {
                lev = (int)olev;
                if (lev > 2) 
                    return null;
                lev++;
                kit.MiscData["dovr"] = lev;
            }
            try 
            {
                List<Pullenti.Ner.Decree.Internal.DecreeToken> dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(t, null, 10, false);
                if (dts == null) 
                    return null;
                List<Pullenti.Ner.ReferentToken> rt = TryAttach(dts, null, ad);
                if (rt == null) 
                {
                    int hasDate = 0;
                    int hasNum = 0;
                    int hasOwn = 0;
                    int hasTyp = 0;
                    int ii;
                    for (ii = 0; ii < dts.Count; ii++) 
                    {
                        if (dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            hasNum++;
                        else if ((dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date && dts[ii].Ref != null && (dts[ii].Ref.Referent is Pullenti.Ner.Date.DateReferent)) && (dts[ii].Ref.Referent as Pullenti.Ner.Date.DateReferent).Dt != null) 
                            hasDate++;
                        else if (dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                            hasOwn++;
                        else if (dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                            hasTyp++;
                        else 
                            break;
                    }
                    if (ii >= dts.Count && hasOwn > 0 && ((hasDate == 1 || hasNum == 1))) 
                    {
                        DecreeReferent dr = new DecreeReferent();
                        foreach (Pullenti.Ner.Decree.Internal.DecreeToken dt in dts) 
                        {
                            if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                                dr.AddDate(dt);
                            else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                                dr.AddNumber(dt);
                            else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                                dr.AddSlot(DecreeReferent.ATTR_TYPE, dt.Value, false, 0);
                            else 
                            {
                                object val = (object)dt.Value;
                                if (dt.Ref != null && dt.Ref.Referent != null) 
                                    val = dt.Ref.Referent;
                                dr.AddSlot(DecreeReferent.ATTR_SOURCE, val, false, 0).Tag = dt.GetSourceText();
                                if (dt.Ref != null && (dt.Ref.Referent is Pullenti.Ner.Person.PersonPropertyReferent)) 
                                    dr.AddExtReferent(dt.Ref);
                            }
                        }
                        rt = new List<Pullenti.Ner.ReferentToken>();
                        rt.Add(new Pullenti.Ner.ReferentToken(dr, dts[0].BeginToken, dts[dts.Count - 1].EndToken));
                    }
                }
                if (((rt == null && dts.Count == 1 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) && dts[0].Ref != null && (dts[0].Ref.Referent is Pullenti.Ner.Date.DateReferent)) && (dts[0].Ref.Referent as Pullenti.Ner.Date.DateReferent).Dt != null) 
                {
                    DecreeReferent dr = new DecreeReferent();
                    dr.AddDate(dts[0]);
                    rt = new List<Pullenti.Ner.ReferentToken>();
                    rt.Add(new Pullenti.Ner.ReferentToken(dr, dts[0].BeginToken, dts[dts.Count - 1].EndToken));
                }
                if (rt == null) 
                    return null;
                if (t0.IsChar('(') && rt[0].EndToken.Next != null && rt[0].EndToken.Next.IsChar(')')) 
                    rt[0].EndToken = rt[0].EndToken.Next;
                rt[0].BeginToken = t0;
                return rt[0];
            }
            finally
            {
                lev--;
                if (lev < 0) 
                    lev = 0;
                kit.MiscData["dovr"] = lev;
            }
        }
        internal static DecreeReferent _getDecree(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.ReferentToken)) 
                return null;
            Pullenti.Ner.Referent r = t.GetReferent();
            if (r is DecreeReferent) 
                return r as DecreeReferent;
            if (r is DecreePartReferent) 
                return (r as DecreePartReferent).Owner;
            return null;
        }
        internal static Pullenti.Ner.Token _checkOtherTyp(Pullenti.Ner.Token t, bool first)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Decree.Internal.DecreeToken dit = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
            Pullenti.Ner.Core.NounPhraseToken npt = null;
            if (dit == null) 
            {
                npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.BeginToken != npt.EndToken) 
                    dit = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(npt.EndToken, null, false);
            }
            if (dit != null && dit.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
            {
                if (dit.Chars.IsCapitalUpper || first) 
                {
                    dit.EndToken.Tag = dit.Value;
                    return dit.EndToken;
                }
                else 
                    return null;
            }
            if (npt != null) 
                t = npt.EndToken;
            if (t.Chars.IsCapitalUpper || first) 
            {
                if (t.Previous != null && t.Previous.IsChar('.') && !first) 
                    return null;
                Pullenti.Ner.Token tt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false);
                if (tt != null) 
                    return tt;
            }
            return null;
        }
        List<Pullenti.Ner.ReferentToken> TryAttachPulishers(List<Pullenti.Ner.Decree.Internal.DecreeToken> dts)
        {
            int i = 0;
            Pullenti.Ner.Token t1 = null;
            string typ = null;
            Pullenti.Ner.ReferentToken geo = null;
            Pullenti.Ner.ReferentToken org = null;
            Pullenti.Ner.Decree.Internal.DecreeToken date = null;
            for (i = 0; i < dts.Count; i++) 
            {
                if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dts[i].Value) == DecreeKind.Publisher) 
                {
                    typ = dts[i].Value;
                    if (dts[i].Ref != null && (dts[i].Ref.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                        geo = dts[i].Ref;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                {
                    geo = dts[i].Ref;
                    t1 = dts[i].EndToken;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                {
                    date = dts[i];
                    t1 = dts[i].EndToken;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                {
                    org = dts[i].Ref;
                    t1 = dts[i].EndToken;
                }
                else 
                    break;
            }
            if (typ == null) 
                return null;
            Pullenti.Ner.Token t = dts[i - 1].EndToken.Next;
            if (t == null) 
                return null;
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            Pullenti.Ner.Decree.Internal.DecreeToken num = null;
            Pullenti.Ner.Token t0 = dts[0].BeginToken;
            if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t, false, null, false)) 
            {
                t = t.Next;
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t0.Previous, false, false)) 
                    t0 = t0.Previous;
            }
            DecreeReferent pub0 = null;
            DecreePartReferent pubPart0 = null;
            for (; t != null; t = t.Next) 
            {
                if (t.IsCharOf(",;.") || t.IsAnd) 
                    continue;
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, dts[0], false);
                if (dt != null) 
                {
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                    {
                        num = dt;
                        pub0 = null;
                        pubPart0 = null;
                        if (t0 == null) 
                            t0 = t;
                        t1 = (t = dt.EndToken);
                        continue;
                    }
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                    {
                        if (t0 == null) 
                            t0 = t;
                        date = dt;
                        pub0 = null;
                        pubPart0 = null;
                        t1 = (t = dt.EndToken);
                        continue;
                    }
                    if (dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Misc && t.LengthChar > 2) 
                        break;
                }
                Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t, null, false, false);
                if (pt == null && t.IsChar('(')) 
                {
                    pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t.Next, null, false, false);
                    if (pt != null) 
                    {
                        if (pt.EndToken.Next != null && pt.EndToken.Next.IsChar(')')) 
                            pt.EndToken = pt.EndToken.Next;
                        else 
                            pt = null;
                    }
                }
                if (pt != null) 
                {
                    if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Page) 
                    {
                        t = pt.EndToken;
                        continue;
                    }
                    if (pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause && pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part && pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Page) 
                        break;
                    if (num == null) 
                        break;
                    if (pubPart0 != null) 
                    {
                        if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part && pubPart0.Part == null) 
                        {
                        }
                        else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause && pubPart0.Clause == null) 
                        {
                        }
                        else 
                            pubPart0 = null;
                    }
                    DecreeReferent pub = pub0;
                    DecreePartReferent pubPart = pubPart0;
                    if (pub == null) 
                    {
                        pub = new DecreeReferent();
                        pub.Typ = typ;
                        if (geo != null) 
                            pub.AddSlot(DecreeReferent.ATTR_GEO, geo.Referent, false, 0);
                        if (org != null) 
                            pub.AddSlot(DecreeReferent.ATTR_SOURCE, org.Referent, false, 0).Tag = org.GetSourceText();
                        if (date != null) 
                            pub.AddDate(date);
                        pub.AddNumber(num);
                        res.Add(new Pullenti.Ner.ReferentToken(pub, t0 ?? t, pt.BeginToken.Previous));
                    }
                    if (pubPart == null) 
                    {
                        pubPart = new DecreePartReferent() { Owner = pub };
                        res.Add(new Pullenti.Ner.ReferentToken(pubPart, pt.BeginToken, pt.EndToken));
                    }
                    pub0 = pub;
                    if (pt.Values.Count == 1) 
                    {
                        if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause) 
                            pubPart.AddSlot(DecreePartReferent.ATTR_CLAUSE, pt.Values[0].Value, false, 0).Tag = pt.Values[0].SourceValue;
                        else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part) 
                            pubPart.AddSlot(DecreePartReferent.ATTR_PART, pt.Values[0].Value, false, 0).Tag = pt.Values[0].SourceValue;
                    }
                    else if (pt.Values.Count > 1) 
                    {
                        for (int ii = 0; ii < pt.Values.Count; ii++) 
                        {
                            if (ii > 0) 
                            {
                                pubPart = new DecreePartReferent() { Owner = pub };
                                res.Add(new Pullenti.Ner.ReferentToken(pubPart, pt.Values[ii].BeginToken, pt.Values[ii].EndToken));
                            }
                            else 
                                res[res.Count - 1].EndToken = pt.Values[ii].EndToken;
                            if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause) 
                                pubPart.AddSlot(DecreePartReferent.ATTR_CLAUSE, pt.Values[ii].Value, false, 0).Tag = pt.Values[ii].SourceValue;
                            else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part) 
                                pubPart.AddSlot(DecreePartReferent.ATTR_PART, pt.Values[ii].Value, false, 0).Tag = pt.Values[ii].SourceValue;
                        }
                    }
                    if (pubPart.Clause == "6878") 
                    {
                    }
                    pubPart0 = pubPart;
                    res[res.Count - 1].EndToken = pt.EndToken;
                    t0 = null;
                    t = pt.EndToken;
                    continue;
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("DATE", t);
                    if (rt != null) 
                    {
                        date = new Pullenti.Ner.Decree.Internal.DecreeToken(rt.BeginToken, rt.EndToken) { Typ = Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date };
                        date.Ref = rt;
                        pub0 = null;
                        pubPart0 = null;
                        if (t0 == null) 
                            t0 = t;
                        t1 = (t = rt.EndToken);
                        continue;
                    }
                    if (t.Next != null && t.Next.IsChar(';')) 
                    {
                        if (pubPart0 != null && pubPart0.Clause != null && pub0 != null) 
                        {
                            DecreePartReferent pubPart = new DecreePartReferent();
                            foreach (Pullenti.Ner.Slot s in pubPart0.Slots) 
                            {
                                pubPart.AddSlot(s.TypeName, s.Value, false, 0);
                            }
                            pubPart0 = pubPart;
                            pubPart0.Clause = (t as Pullenti.Ner.NumberToken).Value.ToString();
                            res.Add(new Pullenti.Ner.ReferentToken(pubPart0, t, t));
                            continue;
                        }
                    }
                }
                if (((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && (t.LengthChar < 3)) && (t.Next is Pullenti.Ner.NumberToken)) 
                {
                    t = t.Next;
                    continue;
                }
                if ((t.IsChar('(') && t.Next != null && t.Next.Next != null) && t.Next.Next.IsChar(')')) 
                {
                    t = t.Next.Next;
                    continue;
                }
                break;
            }
            if ((res.Count == 0 && date != null && num != null) && t1 != null) 
            {
                DecreeReferent pub = new DecreeReferent();
                pub.Typ = typ;
                if (geo != null) 
                    pub.AddSlot(DecreeReferent.ATTR_GEO, geo.Referent, false, 0);
                if (org != null) 
                    pub.AddSlot(DecreeReferent.ATTR_SOURCE, org.Referent, false, 0).Tag = org.GetSourceText();
                if (date != null) 
                    pub.AddDate(date);
                pub.AddNumber(num);
                res.Add(new Pullenti.Ner.ReferentToken(pub, t0, t1));
            }
            return res;
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            Pullenti.Ner.Decree.Internal.DecreeToken dp = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(begin, null, false);
            if (dp != null && dp.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                return new Pullenti.Ner.ReferentToken(null, dp.BeginToken, dp.EndToken);
            return null;
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Ner.Decree.Internal.MetaDecree.Initialize();
            Pullenti.Ner.Decree.Internal.MetaDecreePart.Initialize();
            Pullenti.Ner.Decree.Internal.MetaDecreeChange.Initialize();
            Pullenti.Ner.Decree.Internal.MetaDecreeChangeValue.Initialize();
            try 
            {
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
                Pullenti.Ner.Decree.Internal.DecreeChangeToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Decree.Internal.DecreeToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new DecreeAnalyzer());
        }
    }
}