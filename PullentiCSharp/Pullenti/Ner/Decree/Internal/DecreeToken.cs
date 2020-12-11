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
    // Примитив, из которых состоит декрет
    public class DecreeToken : Pullenti.Ner.MetaToken
    {
        public DecreeToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public enum ItemType : int
        {
            Typ,
            Owner,
            Date,
            Edition,
            Number,
            Name,
            StdName,
            Terr,
            Org,
            Unknown,
            Misc,
            DecreeRef,
            DateRange,
            Between,
            Reading,
        }

        public ItemType Typ;
        public string Value;
        public string FullValue;
        public Pullenti.Ner.ReferentToken Ref;
        public List<DecreeToken> Children = null;
        public bool IsDoubtful;
        public Pullenti.Ner.Decree.DecreeKind TypKind = Pullenti.Ner.Decree.DecreeKind.Undefined;
        public int NumYear;
        public Pullenti.Ner.MetaToken AliasToken;
        public bool IsDelo
        {
            get
            {
                if (BeginToken.IsValue("ДЕЛО", "СПРАВА")) 
                    return true;
                if (BeginToken.Next != null && BeginToken.Next.IsValue("ДЕЛО", "СПРАВА")) 
                    return true;
                return false;
            }
        }
        public override string ToString()
        {
            string v = Value;
            if (v == null) 
                v = Ref.Referent.ToString(true, Kit.BaseLanguage, 0);
            return string.Format("{0} {1} {2}", Typ.ToString(), v, FullValue ?? "");
        }
        public static DecreeToken TryAttach(Pullenti.Ner.Token t, DecreeToken prev = null, bool mustByTyp = false)
        {
            if (t == null) 
                return null;
            if (t.IsValue("НАЗВАННЫЙ", null)) 
            {
            }
            if (t.Kit.IsRecurceOverflow) 
                return null;
            t.Kit.RecurseLevel++;
            DecreeToken res = _TryAttach(t, prev, 0, mustByTyp);
            t.Kit.RecurseLevel--;
            if (res == null) 
            {
                if (t.IsHiphen) 
                {
                    res = _TryAttach(t.Next, prev, 0, mustByTyp);
                    if (res != null && res.Typ == ItemType.Name) 
                    {
                        res.BeginToken = t;
                        return res;
                    }
                }
                if (t.IsValue("ПРОЕКТ", null)) 
                {
                    res = _TryAttach(t.Next, prev, 0, false);
                    if (res != null && res.Typ == ItemType.Typ && res.Value != null) 
                    {
                        if (res.Value.Contains("ЗАКОН") || !(res.EndToken is Pullenti.Ner.TextToken)) 
                            res.Value = "ПРОЕКТ ЗАКОНА";
                        else 
                            res.Value = "ПРОЕКТ " + (res.EndToken as Pullenti.Ner.TextToken).Term;
                        res.BeginToken = t;
                        return res;
                    }
                    else if (res != null && res.Typ == ItemType.Number) 
                    {
                        DecreeToken res1 = _TryAttach(res.EndToken.Next, prev, 0, false);
                        if (res1 != null && res1.Typ == ItemType.Typ && (res1.EndToken is Pullenti.Ner.TextToken)) 
                        {
                            res = new DecreeToken(t, t) { Typ = ItemType.Typ };
                            res.Value = "ПРОЕКТ " + (res1.EndToken as Pullenti.Ner.TextToken).Term;
                            return res;
                        }
                    }
                }
                if (t.IsValue("ИНФОРМАЦИЯ", "ІНФОРМАЦІЯ") && (t.WhitespacesAfterCount < 3)) 
                {
                    List<DecreeToken> dts = TryAttachList(t.Next, null, 10, false);
                    if (dts == null || (dts.Count < 2)) 
                        return null;
                    bool hasNum = false;
                    bool hasOwn = false;
                    bool hasDate = false;
                    bool hasName = false;
                    foreach (DecreeToken dt in dts) 
                    {
                        if (dt.Typ == ItemType.Number) 
                            hasNum = true;
                        else if (dt.Typ == ItemType.Owner || dt.Typ == ItemType.Org) 
                            hasOwn = true;
                        else if (dt.Typ == ItemType.Date) 
                            hasDate = true;
                        else if (dt.Typ == ItemType.Name) 
                            hasName = true;
                    }
                    if (hasOwn && ((hasNum || ((hasDate && hasName))))) 
                    {
                        res = new DecreeToken(t, t) { Typ = ItemType.Typ };
                        res.Value = "ИНФОРМАЦИЯ";
                        return res;
                    }
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    if ((npt.EndToken.IsValue("СОБРАНИЕ", null) || npt.EndToken.IsValue("УЧАСТНИК", null) || npt.EndToken.IsValue("СОБСТВЕННИК", null)) || npt.EndToken.IsValue("УЧРЕДИТЕЛЬ", null)) 
                    {
                        res = new DecreeToken(t, npt.EndToken) { Typ = ItemType.Owner };
                        Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt2 != null && npt2.Morph.Case.IsGenitive) 
                            res.EndToken = npt2.EndToken;
                        res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, res.EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                        return res;
                    }
                }
                return null;
            }
            if (res.Typ == ItemType.Date) 
            {
                if (res.Ref == null) 
                    return null;
                Pullenti.Ner.Date.DateReferent dre = res.Ref.Referent as Pullenti.Ner.Date.DateReferent;
                if (dre == null) 
                    return null;
            }
            if (res.BeginToken.BeginChar > res.EndToken.EndChar) 
            {
            }
            if (res.Typ == ItemType.Number) 
            {
                for (Pullenti.Ner.Token tt = res.EndToken.Next; tt != null; tt = tt.Next) 
                {
                    if (!tt.IsCommaAnd || tt.IsNewlineBefore) 
                        break;
                    tt = tt.Next;
                    if (!(tt is Pullenti.Ner.NumberToken)) 
                        break;
                    if (tt.WhitespacesBeforeCount > 2) 
                        break;
                    DecreeToken ddd = _TryAttach(tt, res, 0, false);
                    if (ddd != null) 
                    {
                        if (ddd.Typ != ItemType.Number) 
                            break;
                        if (res.Children == null) 
                            res.Children = new List<DecreeToken>();
                        res.Children.Add(ddd);
                        res.EndToken = ddd.EndToken;
                        continue;
                    }
                    if ((tt as Pullenti.Ner.NumberToken).IntValue != null && (tt as Pullenti.Ner.NumberToken).IntValue.Value > 1970) 
                        break;
                    if (tt.IsWhitespaceAfter) 
                    {
                    }
                    else if (!tt.Next.IsCharOf(",.")) 
                    {
                    }
                    else 
                        break;
                    StringBuilder tmp = new StringBuilder();
                    Pullenti.Ner.Token tee = _tryAttachNumber(tt, tmp, true);
                    if (res.Children == null) 
                        res.Children = new List<DecreeToken>();
                    DecreeToken add = new DecreeToken(tt, tee) { Typ = ItemType.Number, Value = tmp.ToString() };
                    res.Children.Add(add);
                    res.EndToken = (tt = tee);
                }
            }
            if (res.Typ != ItemType.Typ) 
                return res;
            if (res.BeginToken == res.EndToken) 
            {
                Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(res.BeginToken.Previous, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null && (tok.Termin.Tag is ItemType) && tok.EndToken == res.EndToken) 
                {
                    if (((ItemType)tok.Termin.Tag) == ItemType.Typ) 
                        return null;
                }
            }
            if (((prev != null && prev.Typ == ItemType.Typ && prev.Value != null) && ((prev.Value.Contains("ДОГОВОР") || prev.Value.Contains("ДОГОВІР"))) && res.Value != null) && !res.Value.Contains("ДОГОВОР") && !res.Value.Contains("ДОГОВІР")) 
                return null;
            foreach (string e in m_EmptyAdjectives) 
            {
                if (t.IsValue(e, null)) 
                {
                    res = _TryAttach(t.Next, prev, 0, false);
                    if (res == null || res.Typ != ItemType.Typ) 
                        return null;
                    break;
                }
            }
            if (res.EndToken.Next != null && res.EndToken.Next.IsChar('(')) 
            {
                DecreeToken res1 = _TryAttach(res.EndToken.Next, prev, 0, false);
                if (res1 != null && res1.EndToken.IsChar(')')) 
                {
                    if (res1.Value == res.Value && res.Typ == ItemType.Typ) 
                        res.EndToken = res1.EndToken;
                    else if (res.Value == "ЕДИНЫЙ ОТРАСЛЕВОЙ СТАНДАРТ ЗАКУПОК" && res1.Value != null && res1.Value.StartsWith("ПОЛОЖЕНИЕ О ЗАКУПК")) 
                        res.EndToken = res1.EndToken;
                }
            }
            if (res.Value != null && res.Value.Contains(" ")) 
            {
                foreach (string s in m_AllTypesRU) 
                {
                    if (res.Value.Contains(s) && res.Value != s) 
                    {
                        if (s == "КОДЕКС") 
                        {
                            res.FullValue = res.Value;
                            res.Value = s;
                            break;
                        }
                    }
                }
            }
            if (res.Value == "КОДЕКС" && res.FullValue == null) 
            {
                Pullenti.Ner.Token t1 = res.EndToken;
                for (Pullenti.Ner.Token tt = t1.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsNewlineBefore) 
                        break;
                    DecreeChangeToken cha = DecreeChangeToken.TryAttach(tt, null, false, null, false);
                    if (cha != null) 
                        break;
                    if (tt == t1.Next && res.BeginToken.Previous != null && res.BeginToken.Previous.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ")) 
                        break;
                    if (!(tt is Pullenti.Ner.TextToken)) 
                        break;
                    if (tt == t1.Next && tt.IsValue("ЗАКОН", null)) 
                    {
                        if (tt.Next != null && ((tt.Next.IsValue("О", null) || tt.Next.IsValue("ПРО", null)))) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt0 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt.Next.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt0 == null || !npt0.Morph.Case.IsPrepositional) 
                                break;
                            t1 = npt0.EndToken;
                            break;
                        }
                    }
                    bool ooo = false;
                    if (tt.Morph.Class.IsPreposition && tt.Next != null) 
                    {
                        if (tt.IsValue("ПО", null)) 
                            tt = tt.Next;
                        else if (tt.IsValue("О", null) || tt.IsValue("ОБ", null) || tt.IsValue("ПРО", null)) 
                        {
                            ooo = true;
                            tt = tt.Next;
                        }
                    }
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt == null) 
                        break;
                    if (tt == t1.Next && npt.Morph.Case.IsGenitive) 
                        t1 = (tt = npt.EndToken);
                    else if (ooo && npt.Morph.Case.IsPrepositional) 
                    {
                        t1 = (tt = npt.EndToken);
                        for (Pullenti.Ner.Token ttt = tt.Next; ttt != null; ttt = ttt.Next) 
                        {
                            if (!ttt.IsCommaAnd) 
                                break;
                            npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt == null || !npt.Morph.Case.IsPrepositional) 
                                break;
                            t1 = (tt = npt.EndToken);
                            if (ttt.IsAnd) 
                                break;
                            ttt = npt.EndToken;
                        }
                    }
                    else 
                        break;
                }
                if (t1 != res.EndToken) 
                {
                    res.EndToken = t1;
                    res.FullValue = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(res, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                }
            }
            if (res.Value != null && ((res.Value.StartsWith("ВЕДОМОСТИ СЪЕЗДА") || res.Value.StartsWith("ВІДОМОСТІ ЗЇЗДУ")))) 
            {
                Pullenti.Ner.Token tt = res.EndToken.Next;
                if (tt != null && (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    res.Ref = tt as Pullenti.Ner.ReferentToken;
                    res.EndToken = tt;
                    tt = tt.Next;
                }
                if (tt != null && tt.IsAnd) 
                    tt = tt.Next;
                if (tt != null && (tt.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                {
                    res.EndToken = tt;
                    tt = tt.Next;
                }
            }
            return res;
        }
        static DecreeToken _TryAttach(Pullenti.Ner.Token t, DecreeToken prev, int lev, bool mustByTyp = false)
        {
            if (t == null || lev > 4) 
                return null;
            if (prev != null && prev.Typ == ItemType.Typ) 
            {
                while (t.IsCharOf(":-") && t.Next != null && !t.IsNewlineAfter) 
                {
                    t = t.Next;
                }
            }
            if (prev != null) 
            {
                if (t.IsValue("ПРИ", "ЗА") && t.Next != null) 
                    t = t.Next;
            }
            if ((!mustByTyp && t.IsValue("МЕЖДУ", "МІЖ") && (t.Next is Pullenti.Ner.ReferentToken)) && t.Next.Next != null) 
            {
                Pullenti.Ner.Token t11 = t.Next.Next;
                bool isBr = false;
                if ((t11.IsChar('(') && (t11.Next is Pullenti.Ner.TextToken) && t11.Next.Next != null) && t11.Next.Next.IsChar(')')) 
                {
                    t11 = t11.Next.Next.Next;
                    isBr = true;
                }
                if (t11 != null && t11.IsCommaAnd && (t11.Next is Pullenti.Ner.ReferentToken)) 
                {
                    DecreeToken rr = new DecreeToken(t, t11.Next) { Typ = ItemType.Between };
                    rr.Children = new List<DecreeToken>();
                    rr.Children.Add(new DecreeToken(t.Next, t.Next) { Typ = ItemType.Owner, Ref = t.Next as Pullenti.Ner.ReferentToken });
                    rr.Children.Add(new DecreeToken(t11.Next, t11.Next) { Typ = ItemType.Owner, Ref = t11.Next as Pullenti.Ner.ReferentToken });
                    for (t = rr.EndToken.Next; t != null; t = t.Next) 
                    {
                        if ((isBr && t.IsChar('(') && (t.Next is Pullenti.Ner.TextToken)) && t.Next.Next != null && t.Next.Next.IsChar(')')) 
                        {
                            t = t.Next.Next;
                            rr.EndToken = t;
                            rr.Children[rr.Children.Count - 1].EndToken = t;
                            continue;
                        }
                        if ((t.IsCommaAnd && t.Next != null && (t.Next is Pullenti.Ner.ReferentToken)) && !(t.Next.GetReferent() is Pullenti.Ner.Date.DateReferent)) 
                        {
                            rr.Children.Add(new DecreeToken(t.Next, t.Next) { Typ = ItemType.Owner, Ref = t.Next as Pullenti.Ner.ReferentToken });
                            t = (rr.EndToken = t.Next);
                            continue;
                        }
                        break;
                    }
                    return rr;
                }
            }
            Pullenti.Ner.Referent r = t.GetReferent();
            if (r is Pullenti.Ner.Org.OrganizationReferent) 
            {
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                Pullenti.Ner.Org.OrganizationReferent org = r as Pullenti.Ner.Org.OrganizationReferent;
                DecreeToken res1 = null;
                if (org.ContainsProfile(Pullenti.Ner.Org.OrgProfile.Media)) 
                {
                    Pullenti.Ner.Token tt1 = rt.BeginToken;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt1, false, false)) 
                        tt1 = tt1.Next;
                    res1 = _TryAttach(tt1, prev, lev + 1, false);
                    if (res1 != null && res1.Typ == ItemType.Typ) 
                        res1.BeginToken = (res1.EndToken = t);
                    else 
                        res1 = null;
                }
                if (res1 == null && org.ContainsProfile(Pullenti.Ner.Org.OrgProfile.Press)) 
                {
                    res1 = new DecreeToken(t, t) { Typ = ItemType.Typ };
                    res1.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(t as Pullenti.Ner.ReferentToken, Pullenti.Ner.Core.GetTextAttr.No);
                }
                if (res1 != null) 
                {
                    Pullenti.Ner.Token t11 = res1.EndToken;
                    if (t11.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                        res1.Ref = t11 as Pullenti.Ner.ReferentToken;
                    else if (t11 is Pullenti.Ner.MetaToken) 
                        t11 = (t11 as Pullenti.Ner.MetaToken).EndToken;
                    if (t11.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                        res1.Ref = t11 as Pullenti.Ner.ReferentToken;
                    else if (Pullenti.Ner.Core.BracketHelper.IsBracket(t11, false) && (t11.Previous.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                        res1.Ref = t11.Previous as Pullenti.Ner.ReferentToken;
                    return res1;
                }
            }
            if (r != null && !mustByTyp) 
            {
                if (r is Pullenti.Ner.Geo.GeoReferent) 
                    return new DecreeToken(t, t) { Typ = ItemType.Terr, Ref = t as Pullenti.Ner.ReferentToken, Value = r.ToString(true, t.Kit.BaseLanguage, 0) };
                if (r is Pullenti.Ner.Date.DateReferent) 
                {
                    if (prev != null && prev.Typ == ItemType.Typ && prev.TypKind == Pullenti.Ner.Decree.DecreeKind.Standard) 
                    {
                        DecreeToken ree = TryAttach((t as Pullenti.Ner.ReferentToken).BeginToken, prev, false);
                        if ((ree != null && ree.Typ == ItemType.Number && ree.NumYear > 0) && ((ree.EndToken == (t as Pullenti.Ner.ReferentToken).EndToken || ree.EndToken.IsChar('*')))) 
                        {
                            if ((t.Next is Pullenti.Ner.TextToken) && t.Next.IsChar('*')) 
                                t = t.Next;
                            ree.BeginToken = (ree.EndToken = t);
                            return ree;
                        }
                    }
                    if (t.Previous != null && t.Previous.Morph.Class.IsPreposition && t.Previous.IsValue("ДО", null)) 
                        return null;
                    return new DecreeToken(t, t) { Typ = ItemType.Date, Ref = t as Pullenti.Ner.ReferentToken };
                }
                if (r is Pullenti.Ner.Org.OrganizationReferent) 
                {
                    if ((t.Next != null && t.Next.IsValue("В", "У") && t.Next.Next != null) && t.Next.Next.IsValue("СОСТАВ", "СКЛАДІ")) 
                        return null;
                    return new DecreeToken(t, t) { Typ = ItemType.Org, Ref = t as Pullenti.Ner.ReferentToken, Value = r.ToString() };
                }
                if (r is Pullenti.Ner.Person.PersonReferent) 
                {
                    bool ok = false;
                    if (prev != null && ((prev.Typ == ItemType.Typ || prev.Typ == ItemType.Date))) 
                        ok = true;
                    else if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                        ok = true;
                    else 
                    {
                        DecreeToken ne = _TryAttach(t.Next, null, lev + 1, false);
                        if (ne != null && ((ne.Typ == ItemType.Typ || ne.Typ == ItemType.Date || ne.Typ == ItemType.Owner))) 
                            ok = true;
                    }
                    if (ok) 
                    {
                        Pullenti.Ner.Person.PersonPropertyReferent prop = r.GetSlotValue(Pullenti.Ner.Person.PersonReferent.ATTR_ATTR) as Pullenti.Ner.Person.PersonPropertyReferent;
                        if (prop != null && ((prop.Kind == Pullenti.Ner.Person.PersonPropertyKind.Boss || ((prop.Name ?? "")).StartsWith("глава")))) 
                            return new DecreeToken(t, t) { Typ = ItemType.Owner, Ref = new Pullenti.Ner.ReferentToken(prop, t, t) };
                    }
                }
                if (r is Pullenti.Ner.Person.PersonPropertyReferent) 
                    return new DecreeToken(t, t) { Typ = ItemType.Owner, Ref = new Pullenti.Ner.ReferentToken(r, t, t) };
                if (r is Pullenti.Ner.Denomination.DenominationReferent) 
                {
                    string s = r.ToString();
                    if (s.Length > 1 && ((s[0] == 'A' || s[0] == 'А')) && char.IsDigit(s[1])) 
                        return new DecreeToken(t, t) { Typ = ItemType.Number, Value = s };
                }
                return null;
            }
            if (!mustByTyp) 
            {
                Pullenti.Ner.Token tdat = null;
                if (t.IsValue("ОТ", "ВІД") || t.IsValue("ПРИНЯТЬ", "ПРИЙНЯТИ")) 
                    tdat = t.Next;
                else if (t.IsValue("ВВЕСТИ", null) || t.IsValue("ВВОДИТЬ", "ВВОДИТИ")) 
                {
                    tdat = t.Next;
                    if (tdat != null && tdat.IsValue("В", "У")) 
                        tdat = tdat.Next;
                    if (tdat != null && tdat.IsValue("ДЕЙСТВИЕ", "ДІЯ")) 
                        tdat = tdat.Next;
                }
                if (tdat != null) 
                {
                    if (tdat.Next != null && tdat.Morph.Class.IsPreposition) 
                        tdat = tdat.Next;
                    if (tdat.GetReferent() is Pullenti.Ner.Date.DateReferent) 
                        return new DecreeToken(t, tdat) { Typ = ItemType.Date, Ref = tdat as Pullenti.Ner.ReferentToken };
                    Pullenti.Ner.ReferentToken dr = t.Kit.ProcessReferent("DATE", tdat);
                    if (dr != null) 
                        return new DecreeToken(t, dr.EndToken) { Typ = ItemType.Date, Ref = dr };
                }
                if (t.IsValue("НА", null) && t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Date.DateRangeReferent)) 
                    return new DecreeToken(t, t.Next) { Typ = ItemType.DateRange, Ref = t.Next as Pullenti.Ner.ReferentToken };
                if (t.IsChar('(')) 
                {
                    Pullenti.Ner.Token tt = _isEdition(t.Next);
                    if (tt != null) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                            return new DecreeToken(t, br.EndToken) { Typ = ItemType.Edition };
                    }
                    if (t.Next != null && t.Next.IsValue("ПРОЕКТ", null)) 
                        return new DecreeToken(t.Next, t.Next) { Typ = ItemType.Typ, Value = "ПРОЕКТ" };
                    if ((t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Date.DateRangeReferent) && t.Next.Next != null) && t.Next.Next.IsChar(')')) 
                        return new DecreeToken(t, t.Next.Next) { Typ = ItemType.DateRange, Ref = t.Next as Pullenti.Ner.ReferentToken };
                }
                else 
                {
                    Pullenti.Ner.Token tt = _isEdition(t);
                    if (tt != null) 
                        tt = tt.Next;
                    if (tt != null) 
                    {
                        DecreeToken xxx = DecreeToken.TryAttach(tt, null, false);
                        if (xxx != null) 
                            return new DecreeToken(t, tt.Previous) { Typ = ItemType.Edition };
                    }
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    if (prev != null && ((prev.Typ == ItemType.Typ || prev.Typ == ItemType.Date))) 
                    {
                        StringBuilder tmp = new StringBuilder();
                        Pullenti.Ner.Token t11 = _tryAttachNumber(t, tmp, false);
                        if (t11 != null) 
                        {
                            DecreeToken ne = _TryAttach(t11.Next, null, lev + 1, false);
                            string valnum = tmp.ToString();
                            if (ne != null && ((ne.Typ == ItemType.Date || ne.Typ == ItemType.Owner || ne.Typ == ItemType.Name))) 
                                return new DecreeToken(t, t11) { Typ = ItemType.Number, Value = valnum };
                            if (Pullenti.Morph.LanguageHelper.EndsWithEx(valnum, "ФЗ", "ФКЗ", null, null)) 
                                return new DecreeToken(t, t11) { Typ = ItemType.Number, Value = valnum };
                            int year = 0;
                            if (prev.Typ == ItemType.Typ) 
                            {
                                bool ok = false;
                                if (prev.TypKind == Pullenti.Ner.Decree.DecreeKind.Standard) 
                                {
                                    ok = true;
                                    if (t11.Next != null && t11.Next.IsChar('*')) 
                                        t11 = t11.Next;
                                    if (valnum.EndsWith("(E)", StringComparison.OrdinalIgnoreCase)) 
                                        valnum = valnum.Substring(0, valnum.Length - 3).Trim();
                                    while (true) 
                                    {
                                        if ((t11.WhitespacesAfterCount < 2) && (t11.Next is Pullenti.Ner.NumberToken)) 
                                        {
                                            tmp.Length = 0;
                                            Pullenti.Ner.Token t22 = _tryAttachNumber(t11.Next, tmp, false);
                                            if (t22 == null) 
                                                break;
                                            valnum = string.Format("{0}.{1}", valnum, tmp.ToString());
                                            t11 = t22;
                                        }
                                        else 
                                            break;
                                    }
                                    for (int ii = valnum.Length - 1; ii >= 0; ii--) 
                                    {
                                        if (!char.IsDigit(valnum[ii])) 
                                        {
                                            if (ii == valnum.Length || ii == 0) 
                                                break;
                                            if ((valnum[ii] != '-' && valnum[ii] != ':' && valnum[ii] != '.') && valnum[ii] != '/' && valnum[ii] != '\\') 
                                                break;
                                            int nn = 0;
                                            string ss = valnum.Substring(ii + 1);
                                            if (ss.Length != 2 && ss.Length != 4) 
                                                break;
                                            if (ss[0] == '0' && ss.Length == 2) 
                                                nn = 2000 + ((int)((ss[1] - '0')));
                                            else if (int.TryParse(ss, out nn)) 
                                            {
                                                if (nn > 50 && nn <= 99) 
                                                    nn += 1900;
                                                else if (ss.Length == 2 && ((2000 + nn) <= DateTime.Now.Year)) 
                                                    nn += 2000;
                                            }
                                            if (nn >= 1950 && nn <= DateTime.Now.Year) 
                                            {
                                                year = nn;
                                                valnum = valnum.Substring(0, ii);
                                            }
                                            break;
                                        }
                                    }
                                    valnum = valnum.Replace('-', '.');
                                    if (year < 1) 
                                    {
                                        if (t11.Next != null && t11.Next.IsHiphen) 
                                        {
                                            if ((t11.Next.Next is Pullenti.Ner.NumberToken) && (t11.Next.Next as Pullenti.Ner.NumberToken).IntValue != null) 
                                            {
                                                int nn = (t11.Next.Next as Pullenti.Ner.NumberToken).IntValue.Value;
                                                if (nn > 50 && nn <= 99) 
                                                    nn += 1900;
                                                if (nn >= 1950 && nn <= DateTime.Now.Year) 
                                                {
                                                    year = nn;
                                                    t11 = t11.Next.Next;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (prev.BeginToken == prev.EndToken && prev.BeginToken.Chars.IsAllUpper && ((prev.BeginToken.IsValue("ФЗ", null) || prev.BeginToken.IsValue("ФКЗ", null)))) 
                                    ok = true;
                                if (ok) 
                                    return new DecreeToken(t, t11) { Typ = ItemType.Number, Value = valnum, NumYear = year };
                            }
                        }
                        if ((t as Pullenti.Ner.NumberToken).IntValue != null) 
                        {
                            int val = (t as Pullenti.Ner.NumberToken).IntValue.Value;
                            if (val > 1910 && (val < 2030)) 
                                return new DecreeToken(t, t) { Typ = ItemType.Date, Value = val.ToString() };
                        }
                    }
                    Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("PERSON", t);
                    if (rt != null) 
                    {
                        Pullenti.Ner.Person.PersonPropertyReferent pr = rt.Referent as Pullenti.Ner.Person.PersonPropertyReferent;
                        if (pr != null) 
                            return new DecreeToken(rt.BeginToken, rt.EndToken) { Typ = ItemType.Owner, Ref = rt, Morph = rt.Morph };
                    }
                    if (t.Next != null && t.Next.Chars.IsLetter) 
                    {
                        DecreeToken res1 = _TryAttach(t.Next, prev, lev + 1, false);
                        if (res1 != null && res1.Typ == ItemType.Owner) 
                        {
                            res1.BeginToken = t;
                            return res1;
                        }
                    }
                }
            }
            List<Pullenti.Ner.Core.TerminToken> toks = null;
            if (!(t is Pullenti.Ner.TextToken)) 
            {
                if ((t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).Value == "100") 
                {
                    if ((t as Pullenti.Ner.NumberToken).BeginToken.IsValue("СТО", null) && (t as Pullenti.Ner.NumberToken).BeginToken.Chars.IsAllUpper) 
                    {
                        toks = m_Termins.TryParseAll((t as Pullenti.Ner.NumberToken).BeginToken, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (toks != null && toks.Count == 1) 
                            toks[0].BeginToken = (toks[0].EndToken = t);
                    }
                }
                if (toks == null) 
                    return null;
            }
            else 
                toks = m_Termins.TryParseAll(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (toks != null) 
            {
                foreach (Pullenti.Ner.Core.TerminToken tok in toks) 
                {
                    if (tok.EndToken.IsChar('.') && tok.BeginToken != tok.EndToken) 
                        tok.EndToken = tok.EndToken.Previous;
                    if (tok.Termin.CanonicText == "РЕГИСТРАЦИЯ" || tok.Termin.CanonicText == "РЕЄСТРАЦІЯ") 
                    {
                        if (tok.EndToken.Next != null && ((tok.EndToken.Next.IsValue("В", null) || tok.EndToken.Next.IsValue("ПО", null)))) 
                            tok.EndToken = tok.EndToken.Next;
                    }
                    bool doubt = false;
                    if ((tok.EndChar - tok.BeginChar) < 3) 
                    {
                        if (t.IsValue("СП", null)) 
                        {
                            if (!(t.Next is Pullenti.Ner.NumberToken)) 
                            {
                                if (Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t.Next) == null) 
                                    return null;
                            }
                        }
                        doubt = true;
                        if (tok.EndToken.Next == null || !tok.Chars.IsAllUpper) 
                        {
                        }
                        else 
                        {
                            r = tok.EndToken.Next.GetReferent();
                            if (r is Pullenti.Ner.Geo.GeoReferent) 
                                doubt = false;
                        }
                    }
                    if (tok.BeginToken == tok.EndToken && (tok.LengthChar < 4) && toks.Count > 1) 
                    {
                        int cou = 0;
                        for (Pullenti.Ner.Token tt = t.Previous; tt != null && (cou < 500); tt = tt.Previous,cou++) 
                        {
                            Pullenti.Ner.Decree.DecreeReferent dr = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                            if (dr == null) 
                                continue;
                            foreach (Pullenti.Ner.Core.TerminToken tok1 in toks) 
                            {
                                if (dr.FindSlot(Pullenti.Ner.Decree.DecreeReferent.ATTR_NAME, tok1.Termin.CanonicText, true) != null) 
                                    return new DecreeToken(tok.BeginToken, tok.EndToken) { Typ = (ItemType)tok1.Termin.Tag, Value = tok1.Termin.CanonicText, Morph = tok1.Morph };
                            }
                        }
                        if (tok.BeginToken.IsValue("ТК", null) && tok.Termin.CanonicText.StartsWith("ТРУД")) 
                        {
                            bool hasTamoz = false;
                            cou = 0;
                            for (Pullenti.Ner.Token tt = t.Previous; tt != null && (cou < 500); tt = tt.Previous,cou++) 
                            {
                                if (tt.IsValue("ТАМОЖНЯ", null) || tt.IsValue("ТАМОЖЕННЫЙ", null) || tt.IsValue("ГРАНИЦА", null)) 
                                {
                                    hasTamoz = true;
                                    break;
                                }
                            }
                            if (hasTamoz) 
                                continue;
                            cou = 0;
                            for (Pullenti.Ner.Token tt = t.Next; tt != null && (cou < 500); tt = tt.Next,cou++) 
                            {
                                if (tt.IsValue("ТАМОЖНЯ", null) || tt.IsValue("ТАМОЖЕННЫЙ", null) || tt.IsValue("ГРАНИЦА", null)) 
                                {
                                    hasTamoz = true;
                                    break;
                                }
                            }
                            if (hasTamoz) 
                                continue;
                        }
                    }
                    if (doubt && tok.Chars.IsAllUpper) 
                    {
                        if (PartToken.IsPartBefore(tok.BeginToken)) 
                            doubt = false;
                        else if (tok.GetSourceText().EndsWith("ТС")) 
                            doubt = false;
                    }
                    DecreeToken res = new DecreeToken(tok.BeginToken, tok.EndToken) { Typ = (ItemType)tok.Termin.Tag, Value = tok.Termin.CanonicText, Morph = tok.Morph, IsDoubtful = doubt };
                    if (tok.Termin.Tag2 is Pullenti.Ner.Decree.DecreeKind) 
                        res.TypKind = (Pullenti.Ner.Decree.DecreeKind)tok.Termin.Tag2;
                    if (res.Value == "ГОСТ" && tok.EndToken.Next != null) 
                    {
                        if (tok.EndToken.Next.IsValue("Р", null) || tok.EndToken.Next.IsValue("P", null)) 
                            res.EndToken = tok.EndToken.Next;
                        else 
                        {
                            Pullenti.Ner.Geo.GeoReferent g = tok.EndToken.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                            if (g != null && ((g.Alpha2 == "RU" || g.Alpha2 == "SU"))) 
                                res.EndToken = tok.EndToken.Next;
                        }
                    }
                    if (res.Value == "КОНСТИТУЦИЯ" && tok.EndToken.Next != null && tok.EndToken.Next.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tok.EndToken.Next.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if ((npt != null && npt.EndToken.IsValue("ЗАКОН", null) && npt.EndToken.Next != null) && npt.EndToken.Next.IsChar(')')) 
                            res.EndToken = npt.EndToken.Next;
                    }
                    if ((tok.Termin.Tag2 is string) && res.Typ == ItemType.Typ) 
                    {
                        res.FullValue = tok.Termin.CanonicText;
                        res.Value = tok.Termin.Tag2 as string;
                        res.IsDoubtful = false;
                    }
                    if (res.TypKind == Pullenti.Ner.Decree.DecreeKind.Standard) 
                    {
                        int cou = 0;
                        for (Pullenti.Ner.Token tt = res.EndToken.Next; tt != null && (cou < 3); tt = tt.Next,cou++) 
                        {
                            if (tt.WhitespacesBeforeCount > 2) 
                                break;
                            Pullenti.Ner.Core.TerminToken tok2 = m_Termins.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                            if (tok2 != null) 
                            {
                                if ((tok2.Termin.Tag2 is Pullenti.Ner.Decree.DecreeKind) && ((Pullenti.Ner.Decree.DecreeKind)tok2.Termin.Tag2) == Pullenti.Ner.Decree.DecreeKind.Standard) 
                                {
                                    tt = (res.EndToken = tok2.EndToken);
                                    res.IsDoubtful = false;
                                    if (res.Value == "СТАНДАРТ") 
                                        res.Value = tok2.Termin.CanonicText;
                                    continue;
                                }
                            }
                            if ((tt is Pullenti.Ner.TextToken) && (tt.LengthChar < 4) && tt.Chars.IsAllUpper) 
                            {
                                res.EndToken = tt;
                                continue;
                            }
                            if (((tt.IsCharOf("/\\") || tt.IsHiphen)) && (tt.Next is Pullenti.Ner.TextToken) && tt.Next.Chars.IsAllUpper) 
                            {
                                tt = tt.Next;
                                res.EndToken = tt;
                                continue;
                            }
                            break;
                        }
                        if (res.Value == "СТАНДАРТ") 
                            res.IsDoubtful = true;
                        if (res.IsDoubtful && !res.IsNewlineAfter) 
                        {
                            DecreeToken num1 = TryAttach(res.EndToken.Next, res, false);
                            if (num1 != null && num1.Typ == ItemType.Number) 
                            {
                                if (num1.NumYear > 0) 
                                    res.IsDoubtful = false;
                            }
                        }
                        if (res.Value == "СТАНДАРТ" && res.IsDoubtful) 
                            return null;
                    }
                    return res;
                }
            }
            if (((t.Morph.Class.IsAdjective && ((t.IsValue("УКАЗАННЫЙ", "ЗАЗНАЧЕНИЙ") || t.IsValue("ВЫШЕУКАЗАННЫЙ", "ВИЩЕВКАЗАНИЙ") || t.IsValue("НАЗВАННЫЙ", "НАЗВАНИЙ"))))) || ((t.Morph.Class.IsPronoun && (((t.IsValue("ЭТОТ", "ЦЕЙ") || t.IsValue("ТОТ", "ТОЙ") || t.IsValue("ДАННЫЙ", "ДАНИЙ")) || t.IsValue("САМЫЙ", "САМИЙ")))))) 
            {
                Pullenti.Ner.Token t11 = t.Next;
                if (t11 != null && t11.IsValue("ЖЕ", null)) 
                    t11 = t11.Next;
                Pullenti.Ner.Core.NounPhraseToken nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                Pullenti.Ner.Core.TerminToken tok;
                if ((((tok = m_Termins.TryParse(t11, Pullenti.Ner.Core.TerminParseAttr.No)))) != null) 
                {
                    if (((tok.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) == Pullenti.Morph.MorphNumber.Undefined || ((nnn != null && nnn.Morph.Number == Pullenti.Morph.MorphNumber.Singular))) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null && ((npt.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) != Pullenti.Morph.MorphNumber.Undefined) 
                        {
                        }
                        else 
                        {
                            Pullenti.Ner.ReferentToken te = _findBackTyp(t.Previous, tok.Termin.CanonicText);
                            if (te != null) 
                                return new DecreeToken(t, tok.EndToken) { Typ = ItemType.DecreeRef, Ref = te };
                        }
                    }
                }
            }
            if (t.Morph.Class.IsAdjective && t.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ")) 
            {
                Pullenti.Ner.Core.TerminToken tok;
                if ((((tok = m_Termins.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No)))) != null) 
                    return new DecreeToken(t, tok.EndToken) { Typ = ItemType.DecreeRef, Ref = null };
            }
            if (mustByTyp) 
                return null;
            if ((((t is Pullenti.Ner.TextToken) && prev != null && prev.Typ == ItemType.Typ) && t.Chars.IsAllUpper && t.LengthChar >= 2) && t.LengthChar <= 5) 
            {
                if (((prev.Value == "ТЕХНИЧЕСКИЕ УСЛОВИЯ" && t.Next != null && t.Next.IsChar('.')) && (t.Next.Next is Pullenti.Ner.NumberToken) && t.Next.Next.Next != null) && t.Next.Next.Next.IsChar('.') && (t.Next.Next.Next.Next is Pullenti.Ner.NumberToken)) 
                {
                    DecreeToken res = new DecreeToken(t, t) { Typ = ItemType.Number, Value = (t as Pullenti.Ner.TextToken).Term };
                    t = t.Next.Next;
                    res.Value = string.Format("{0}.{1}.{2}", res.Value, t.GetSourceText(), t.Next.Next.GetSourceText());
                    res.EndToken = (t = t.Next.Next);
                    if ((t.WhitespacesAfterCount < 2) && t.Next != null && t.Next.IsValue("ТУ", null)) 
                        res.EndToken = res.EndToken.Next;
                    return res;
                }
            }
            if ((((((t is Pullenti.Ner.TextToken) && t.LengthChar == 4 && t.Chars.IsAllUpper) && t.Next != null && !t.IsWhitespaceAfter) && t.Next.IsChar('.') && (t.Next.Next is Pullenti.Ner.NumberToken)) && !t.Next.IsWhitespaceAfter && t.Next.Next.Next != null) && t.Next.Next.Next.IsChar('.') && (t.Next.Next.Next.Next is Pullenti.Ner.NumberToken)) 
            {
                if (t.Next.Next.Next.Next.Next != null && t.Next.Next.Next.Next.Next.IsValue("ТУ", null)) 
                {
                    DecreeToken res = new DecreeToken(t, t.Next.Next.Next.Next) { Typ = ItemType.Number, Value = (t as Pullenti.Ner.TextToken).Term };
                    res.Value = string.Format("{0}.{1}.{2}", t.GetSourceText(), t.Next.Next.GetSourceText(), t.Next.Next.Next.Next.GetSourceText());
                    return res;
                }
            }
            if (t.Morph.Class.IsAdjective) 
            {
                DecreeToken dt = _TryAttach(t.Next, prev, lev + 1, false);
                if (dt != null && dt.Ref == null) 
                {
                    Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("GEO", t);
                    if (rt != null) 
                    {
                        dt.Ref = rt;
                        dt.BeginToken = t;
                        return dt;
                    }
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.InternalNoun != null) 
                    npt = null;
                if ((npt != null && dt != null && dt.Typ == ItemType.Typ) && dt.Value == "КОДЕКС") 
                {
                    dt.Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                    dt.BeginToken = t;
                    dt.IsDoubtful = true;
                    return dt;
                }
                if (npt != null && ((npt.EndToken.IsValue("ДОГОВОР", null) || npt.EndToken.IsValue("КОНТРАКТ", null)))) 
                {
                    dt = new DecreeToken(t, npt.EndToken) { Typ = ItemType.Typ };
                    dt.Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                    if (t.GetMorphClassInDictionary().IsVerb) 
                        dt.Value = npt.EndToken.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                    return dt;
                }
                bool tryNpt = false;
                Pullenti.Ner.Core.TerminToken tok;
                if (!t.Chars.IsAllLower) 
                    tryNpt = true;
                else 
                    foreach (string a in m_StdAdjectives) 
                    {
                        if (t.IsValue(a, null)) 
                        {
                            tryNpt = true;
                            break;
                        }
                    }
                if (tryNpt) 
                {
                    if (npt != null) 
                    {
                        if (npt.EndToken.IsValue("ГАЗЕТА", null) || npt.EndToken.IsValue("БЮЛЛЕТЕНЬ", "БЮЛЕТЕНЬ")) 
                            return new DecreeToken(t, npt.EndToken) { Typ = ItemType.Typ, Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false), Morph = npt.Morph };
                        if (npt.Adjectives.Count > 0 && npt.EndToken.GetMorphClassInDictionary().IsNoun) 
                        {
                            if ((((tok = m_Termins.TryParse(npt.EndToken, Pullenti.Ner.Core.TerminParseAttr.No)))) != null) 
                            {
                                if (npt.BeginToken.IsValue("ОБЩИЙ", "ЗАГАЛЬНИЙ")) 
                                    return null;
                                return new DecreeToken(npt.BeginToken, tok.EndToken) { Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false), Morph = npt.Morph };
                            }
                        }
                        if (prev != null && prev.Typ == ItemType.Typ) 
                        {
                            if (npt.EndToken.IsValue("КОЛЛЕГИЯ", "КОЛЕГІЯ")) 
                            {
                                DecreeToken res1 = new DecreeToken(t, npt.EndToken) { Typ = ItemType.Owner, Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false), Morph = npt.Morph };
                                for (t = npt.EndToken.Next; t != null; t = t.Next) 
                                {
                                    if (t.IsAnd || t.Morph.Class.IsPreposition) 
                                        continue;
                                    Pullenti.Ner.Referent re = t.GetReferent();
                                    if ((re is Pullenti.Ner.Geo.GeoReferent) || (re is Pullenti.Ner.Org.OrganizationReferent)) 
                                    {
                                        res1.EndToken = t;
                                        continue;
                                    }
                                    else if (re != null) 
                                        break;
                                    DecreeToken dt1 = _TryAttach(t, res1, lev + 1, false);
                                    if (dt1 != null && dt1.Typ != ItemType.Unknown) 
                                    {
                                        if (dt1.Typ != ItemType.Owner) 
                                            break;
                                        t = (res1.EndToken = dt1.EndToken);
                                        continue;
                                    }
                                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                    if (npt1 == null) 
                                        break;
                                    t = (res1.EndToken = npt1.EndToken);
                                }
                                if (res1.EndToken != npt.EndToken) 
                                    res1.Value = string.Format("{0} {1}", res1.Value, Pullenti.Ner.Core.MiscHelper.GetTextValue(npt.EndToken.Next, res1.EndToken, Pullenti.Ner.Core.GetTextAttr.KeepQuotes));
                                return res1;
                            }
                        }
                    }
                }
            }
            Pullenti.Ner.Token t1 = null;
            Pullenti.Ner.Token t0 = t;
            bool num = false;
            if ((((t1 = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t)))) != null) 
                num = true;
            else if (_isJusNumber(t)) 
                t1 = t;
            if (t1 != null) 
            {
                if ((t1.WhitespacesBeforeCount < 15) && ((!t1.IsNewlineBefore || (t1 is Pullenti.Ner.NumberToken) || _isJusNumber(t1)))) 
                {
                    StringBuilder tmp = new StringBuilder();
                    Pullenti.Ner.Token t11 = _tryAttachNumber(t1, tmp, num);
                    if (t11 != null) 
                    {
                        if (t11.Next != null && t11.Next.IsValue("ДСП", null)) 
                        {
                            t11 = t11.Next;
                            tmp.Append("ДСП");
                        }
                        return new DecreeToken(t0, t11) { Typ = ItemType.Number, Value = tmp.ToString() };
                    }
                }
                if (t1.IsNewlineBefore && num) 
                    return new DecreeToken(t0, t1.Previous) { Typ = ItemType.Number };
            }
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false)) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false) && ((((t.Next.IsValue("О", null) || t.Next.IsValue("ОБ", null) || t.Next.IsValue("ПРО", null)) || t.Next.IsValue("ПО", null) || t.Chars.IsCapitalUpper) || ((prev != null && (t.Next is Pullenti.Ner.TextToken) && ((prev.Typ == ItemType.Date || prev.Typ == ItemType.Number))))))) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.CanContainsVerbs, 200);
                    if (br != null) 
                    {
                        Pullenti.Ner.Token tt = br.EndToken;
                        if (tt.Previous != null && tt.Previous.IsChar('>')) 
                            tt = tt.Previous;
                        if ((tt.IsChar('>') && (tt.Previous is Pullenti.Ner.NumberToken) && tt.Previous.Previous != null) && tt.Previous.Previous.IsChar('<')) 
                        {
                            tt = tt.Previous.Previous.Previous;
                            if (tt == null || tt.BeginChar <= br.BeginChar) 
                                return null;
                            br.EndToken = tt;
                        }
                        Pullenti.Ner.Token tt1 = _tryAttachStdChangeName(t.Next);
                        if (tt1 != null && tt1.EndChar > br.EndChar) 
                            br.EndToken = tt1;
                        else 
                            for (tt = br.BeginToken.Next; tt != null && (tt.EndChar < br.EndChar); tt = tt.Next) 
                            {
                                if (tt.IsChar('(')) 
                                {
                                    DecreeToken dt = DecreeToken.TryAttach(tt.Next, null, false);
                                    if (dt == null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt.Next, true, false)) 
                                        dt = DecreeToken.TryAttach(tt.Next.Next, null, false);
                                    if (dt != null && dt.Typ == ItemType.Typ) 
                                    {
                                        if (DecreeToken.GetKind(dt.Value) == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                                        {
                                            br.EndToken = tt.Previous;
                                            break;
                                        }
                                    }
                                }
                            }
                        return new DecreeToken(br.BeginToken, br.EndToken) { Typ = ItemType.Name, Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No) };
                    }
                    else 
                    {
                        Pullenti.Ner.Token tt1 = _tryAttachStdChangeName(t.Next);
                        if (tt1 != null) 
                            return new DecreeToken(t, tt1) { Typ = ItemType.Name, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, tt1, Pullenti.Ner.Core.GetTextAttr.No) };
                    }
                }
                else if (t.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        if (!t.Next.IsValue("ДАЛЕЕ", "ДАЛІ")) 
                        {
                            if ((br.EndChar - br.BeginChar) < 30) 
                                return new DecreeToken(br.BeginToken, br.EndToken) { Typ = ItemType.Misc, Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No) };
                        }
                    }
                }
            }
            if (t.InnerBool) 
            {
                Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("PERSON", t);
                if (rt != null) 
                {
                    Pullenti.Ner.Person.PersonPropertyReferent pr = rt.Referent as Pullenti.Ner.Person.PersonPropertyReferent;
                    if (pr == null) 
                        return null;
                    if (pr.Kind != Pullenti.Ner.Person.PersonPropertyKind.Undefined) 
                    {
                    }
                    else if (pr.Name.StartsWith("ГРАЖДАН", StringComparison.OrdinalIgnoreCase) || pr.Name.StartsWith("ГРОМАДЯН", StringComparison.OrdinalIgnoreCase)) 
                        return null;
                    return new DecreeToken(rt.BeginToken, rt.EndToken) { Typ = ItemType.Owner, Ref = rt, Morph = rt.Morph };
                }
            }
            if (t.IsValue("О", null) || t.IsValue("ОБ", null) || t.IsValue("ПРО", null)) 
            {
                Pullenti.Ner.Token et = null;
                if ((t.Next != null && t.Next.IsValue("ВНЕСЕНИЕ", "ВНЕСЕННЯ") && t.Next.Next != null) && t.Next.Next.IsValue("ИЗМЕНЕНИЕ", "ЗМІНА")) 
                    et = t.Next;
                else if (t.Next != null && t.Next.IsValue("ПОПРАВКА", null)) 
                    et = t.Next;
                else if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                    et = t.Next;
                if (et != null && et.Next != null && et.Next.Morph.Class.IsPreposition) 
                    et = et.Next;
                if (et != null && et.Next != null) 
                {
                    List<DecreeToken> dts2 = TryAttachList(et.Next, null, 10, false);
                    if (dts2 != null && dts2[0].Typ == ItemType.Typ) 
                    {
                        et = dts2[0].EndToken;
                        if (dts2.Count > 1 && dts2[1].Typ == ItemType.Terr) 
                            et = dts2[1].EndToken;
                        return new DecreeToken(t, et) { Typ = ItemType.Name, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, et, Pullenti.Ner.Core.GetTextAttr.No) };
                    }
                    if (et.Next.IsCharOf(",(") || (et is Pullenti.Ner.ReferentToken)) 
                        return new DecreeToken(t, et) { Typ = ItemType.Name, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, et, Pullenti.Ner.Core.GetTextAttr.No) };
                }
                else if (et != null) 
                    return new DecreeToken(t, et) { Typ = ItemType.Name, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, et, Pullenti.Ner.Core.GetTextAttr.No) };
                return null;
            }
            if (t.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
                return null;
            if (prev != null && prev.Typ == ItemType.Typ) 
            {
                if (t.IsValue("ПРАВИТЕЛЬСТВО", "УРЯД") || t.IsValue("ПРЕЗИДЕНТ", null)) 
                    return new DecreeToken(t, t) { Typ = ItemType.Owner, Morph = t.Morph, Value = t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) };
            }
            Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
            if (npt2 != null) 
            {
                if (npt2.EndToken.IsValue("ПОЗИЦИЯ", null)) 
                    return null;
            }
            if ((((t.Chars.IsCyrillicLetter && ((!t.Chars.IsAllLower || ((prev != null && prev.Typ == ItemType.Unknown))))) || t.IsValue("ЗАСЕДАНИЕ", "ЗАСІДАННЯ") || t.IsValue("СОБРАНИЕ", "ЗБОРИ")) || t.IsValue("ПЛЕНУМ", null) || t.IsValue("КОЛЛЕГИЯ", "КОЛЕГІЯ")) || t.IsValue("АДМИНИСТРАЦИЯ", "АДМІНІСТРАЦІЯ")) 
            {
                bool ok = false;
                if (prev != null && ((prev.Typ == ItemType.Typ || prev.Typ == ItemType.Owner || prev.Typ == ItemType.Org))) 
                {
                    if (!t.Morph.Class.IsPreposition && !t.Morph.Class.IsConjunction) 
                        ok = true;
                }
                else if (prev != null && prev.Typ == ItemType.Unknown && !t.Morph.Class.IsVerb) 
                    ok = true;
                else if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && !t.IsValue("ИМЕНЕМ", null)) 
                    ok = true;
                else if ((t.Previous != null && t.Previous.IsChar(',') && t.Previous.Previous != null) && (t.Previous.Previous.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                    ok = true;
                if (ok) 
                {
                    if (PartToken.TryAttach(t, null, false, false) != null) 
                        ok = false;
                }
                if (ok) 
                {
                    t1 = t;
                    ItemType ty = ItemType.Unknown;
                    StringBuilder tmp = new StringBuilder();
                    for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
                    {
                        if (!(tt is Pullenti.Ner.TextToken)) 
                        {
                            Pullenti.Ner.Org.OrganizationReferent org = tt.GetReferent() as Pullenti.Ner.Org.OrganizationReferent;
                            if (org != null && tt.Previous == t1) 
                            {
                                ty = ItemType.Owner;
                                if (tmp.Length > 0) 
                                    tmp.Append(' ');
                                tmp.Append(tt.GetSourceText().ToUpper());
                                t1 = tt;
                                break;
                            }
                            break;
                        }
                        if (tt.IsNewlineBefore && tt != t1) 
                            break;
                        if (!tt.Chars.IsCyrillicLetter) 
                            break;
                        if (tt != t) 
                        {
                            if (_TryAttach(tt, null, lev + 1, false) != null) 
                                break;
                        }
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (tt.Chars.IsAllLower && tt != t) 
                        {
                            if (npt != null && npt.Morph.Case.IsGenitive) 
                            {
                            }
                            else 
                                break;
                        }
                        if (npt != null) 
                        {
                            if (tmp.Length > 0) 
                                tmp.AppendFormat(" {0}", npt.GetSourceText());
                            else 
                                tmp.Append(npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                            t1 = (tt = npt.EndToken);
                        }
                        else if (tmp.Length > 0) 
                        {
                            tmp.AppendFormat(" {0}", tt.GetSourceText());
                            t1 = tt;
                        }
                        else 
                        {
                            string s = null;
                            if (tt == t) 
                                s = tt.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                            if (s == null) 
                                s = (tt as Pullenti.Ner.TextToken).Term;
                            tmp.Append(s);
                            t1 = tt;
                        }
                    }
                    string ss = Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(tmp.ToString());
                    return new DecreeToken(t, t1) { Typ = ty, Value = ss };
                }
            }
            if (t.IsValue("ДАТА", null)) 
            {
                t1 = t.Next;
                if (t1 != null && t1.Morph.Case.IsGenitive) 
                    t1 = t1.Next;
                if (t1 != null && t1.IsChar(':')) 
                    t1 = t1.Next;
                DecreeToken res1 = _TryAttach(t1, prev, lev + 1, false);
                if (res1 != null && res1.Typ == ItemType.Date) 
                {
                    res1.BeginToken = t;
                    return res1;
                }
            }
            if (t.IsValue("ВЕСТНИК", "ВІСНИК") || t.IsValue("БЮЛЛЕТЕНЬ", "БЮЛЕТЕНЬ")) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                    return new DecreeToken(t, npt.EndToken) { Typ = ItemType.Typ, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, npt.EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative) };
                else if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                    return new DecreeToken(t, t.Next) { Typ = ItemType.Typ, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t.Next, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative) };
            }
            if ((prev != null && prev.Typ == ItemType.Typ && prev.Value != null) && ((prev.Value.Contains("ДОГОВОР") || prev.Value.Contains("ДОГОВІР")))) 
            {
                DecreeToken nn = TryAttachName(t, prev.Value, false, false);
                if (nn != null) 
                    return nn;
                t1 = null;
                for (Pullenti.Ner.Token ttt = t; ttt != null; ttt = ttt.Next) 
                {
                    if (ttt.IsNewlineBefore) 
                        break;
                    DecreeToken ddt1 = _TryAttach(ttt, null, lev + 1, false);
                    if (ddt1 != null) 
                        break;
                    if (ttt.Morph.Class.IsPreposition || ttt.Morph.Class.IsConjunction) 
                        continue;
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt == null) 
                        break;
                    ttt = (t1 = npt.EndToken);
                }
                if (t1 != null) 
                {
                    nn = new DecreeToken(t, t1) { Typ = ItemType.Name };
                    nn.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1, Pullenti.Ner.Core.GetTextAttr.No);
                    return nn;
                }
            }
            if ((t is Pullenti.Ner.TextToken) && t.LengthChar == 1 && t.Next != null) 
            {
                if (((t as Pullenti.Ner.TextToken).Term == "Б" && t.Next.IsCharOf("\\/") && (t.Next.Next is Pullenti.Ner.TextToken)) && (t.Next.Next as Pullenti.Ner.TextToken).Term == "Н") 
                    return new DecreeToken(t, t.Next.Next) { Typ = ItemType.Number, Value = "Б/Н" };
            }
            return null;
        }
        static bool _isJusNumber(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return false;
            if (tt.Term != "A" && tt.Term != "А") 
                return false;
            if ((t.Next is Pullenti.Ner.NumberToken) && (t.WhitespacesAfterCount < 2)) 
            {
                if ((t.Next as Pullenti.Ner.NumberToken).IntValue != null && (t.Next as Pullenti.Ner.NumberToken).IntValue.Value > 20) 
                    return true;
                return false;
            }
            return false;
        }
        static Pullenti.Ner.Token _isEdition(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if (t.Morph.Class.IsPreposition && t.Next != null) 
                t = t.Next;
            if (t.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ") || t.IsValue("РЕД", null)) 
            {
                if (t.Next != null && t.Next.IsChar('.')) 
                    return t.Next;
                else 
                    return t;
            }
            if (t.IsValue("ИЗМЕНЕНИЕ", "ЗМІНА") || t.IsValue("ИЗМ", null)) 
            {
                if (t.Next != null && t.Next.IsChar('.')) 
                    t = t.Next;
                if ((t.Next != null && t.Next.IsComma && t.Next.Next != null) && t.Next.Next.IsValue("ВНЕСЕННЫЙ", "ВНЕСЕНИЙ")) 
                    return t.Next.Next;
                return t;
            }
            if ((t is Pullenti.Ner.NumberToken) && t.Next != null && t.Next.IsValue("ЧТЕНИЕ", "ЧИТАННЯ")) 
                return t.Next.Next;
            return null;
        }
        internal static Pullenti.Ner.ReferentToken _findBackTyp(Pullenti.Ner.Token t, string typeName)
        {
            if (t == null) 
                return null;
            if (t.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ")) 
                return null;
            int cou = 0;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Previous) 
            {
                cou++;
                if (tt.IsNewlineBefore) 
                    cou += 10;
                if (cou > 500) 
                    break;
                Pullenti.Ner.Decree.DecreeReferent d = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                if (d == null && (tt.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent)) 
                    d = (tt.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent).Owner;
                if (d == null) 
                    continue;
                if (d.Typ0 == typeName || d.Typ == typeName) 
                    return tt as Pullenti.Ner.ReferentToken;
            }
            return null;
        }
        static Pullenti.Ner.Token _tryAttachNumber(Pullenti.Ner.Token t, StringBuilder tmp, bool afterNum)
        {
            Pullenti.Ner.Token t2 = t;
            Pullenti.Ner.Token res = null;
            bool digs = false;
            bool br = false;
            for (; t2 != null; t2 = t2.Next) 
            {
                if (t2.IsCharOf("(),;")) 
                    break;
                if (t2.IsTableControlChar) 
                    break;
                if (t2.IsChar('.') && t2.IsWhitespaceAfter) 
                    break;
                if (t2 != t && t2.WhitespacesBeforeCount > 1) 
                    break;
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(t2, false)) 
                {
                    if (!afterNum) 
                        break;
                    if (!br && t2 != t) 
                        break;
                    res = t2;
                    if (br) 
                        break;
                    br = true;
                    continue;
                }
                if (!(t2 is Pullenti.Ner.NumberToken) && !(t2 is Pullenti.Ner.TextToken)) 
                {
                    Pullenti.Ner.Date.DateReferent dr = t2.GetReferent() as Pullenti.Ner.Date.DateReferent;
                    if (dr != null && !dr.IsRelative && ((t2 == t || !t2.IsWhitespaceBefore))) 
                    {
                        if (dr.Year > 0 && t2.LengthChar == 4) 
                        {
                            res = t2;
                            tmp.Append(dr.Year);
                            digs = true;
                            continue;
                        }
                    }
                    Pullenti.Ner.Denomination.DenominationReferent den = t2.GetReferent() as Pullenti.Ner.Denomination.DenominationReferent;
                    if (den != null) 
                    {
                        res = t2;
                        tmp.Append(t2.GetSourceText().ToUpper());
                        foreach (char c in den.Value) 
                        {
                            if (char.IsDigit(c)) 
                                digs = true;
                        }
                        if (t2.IsWhitespaceAfter) 
                            break;
                        continue;
                    }
                    if ((t2.LengthChar < 10) && afterNum && !t2.IsWhitespaceBefore) 
                    {
                    }
                    else 
                        break;
                }
                string s = t2.GetSourceText();
                if (s == null) 
                    break;
                if (t2.IsHiphen) 
                    s = "-";
                if (t2.IsValue("ОТ", "ВІД")) 
                    break;
                if (s == "\\") 
                    s = "/";
                if (char.IsDigit(s[0])) 
                {
                    foreach (char d in s) 
                    {
                        digs = true;
                    }
                }
                if (!t2.IsCharOf("_@")) 
                    tmp.Append(s);
                res = t2;
                if (t2.IsWhitespaceAfter) 
                {
                    if (t2.WhitespacesAfterCount > 1) 
                        break;
                    if (digs) 
                    {
                        if ((t2.Next != null && ((t2.Next.IsHiphen || t2.Next.IsCharOf(".:"))) && !t2.Next.IsWhitespaceAfter) && (t2.Next.Next is Pullenti.Ner.NumberToken)) 
                            continue;
                    }
                    if (!afterNum) 
                        break;
                    if (t2.IsHiphen) 
                    {
                        if (t2.Next != null && t2.Next.IsValue("СМ", null)) 
                            break;
                        continue;
                    }
                    if (t2.IsChar('/')) 
                        continue;
                    if (t2.Next != null) 
                    {
                        if (((t2.Next.IsHiphen || (t2.Next is Pullenti.Ner.NumberToken))) && !digs) 
                            continue;
                    }
                    if (t2 == t && t2.Chars.IsAllUpper) 
                        continue;
                    if (t2.Next is Pullenti.Ner.NumberToken) 
                    {
                        if (t2 is Pullenti.Ner.NumberToken) 
                            tmp.Append(" ");
                        continue;
                    }
                    break;
                }
            }
            if (tmp.Length == 0) 
            {
                if (t != null && t.IsChar('_')) 
                {
                    for (t2 = t; t2 != null; t2 = t2.Next) 
                    {
                        if (!t2.IsChar('_') || ((t2 != t && t2.IsWhitespaceBefore))) 
                        {
                            tmp.Append('?');
                            return t2.Previous;
                        }
                    }
                }
                return null;
            }
            if (!digs && !afterNum) 
                return null;
            char ch = tmp[tmp.Length - 1];
            if (!char.IsLetterOrDigit(ch) && (res is Pullenti.Ner.TextToken) && !res.IsChar('_')) 
            {
                tmp.Length--;
                res = res.Previous;
            }
            if ((res.Next != null && res.Next.IsHiphen && (res.Next.Next is Pullenti.Ner.NumberToken)) && (res.Next.Next as Pullenti.Ner.NumberToken).IntValue != null) 
            {
                int min;
                if (int.TryParse(tmp.ToString(), out min)) 
                {
                    if (min < (res.Next.Next as Pullenti.Ner.NumberToken).IntValue.Value) 
                    {
                        res = res.Next.Next;
                        tmp.AppendFormat("-{0}", (res as Pullenti.Ner.NumberToken).Value);
                    }
                }
            }
            if (res.Next != null && !res.IsWhitespaceAfter && res.Next.IsChar('(')) 
            {
                int cou = 0;
                StringBuilder tmp2 = new StringBuilder();
                for (Pullenti.Ner.Token tt = res.Next.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsChar(')')) 
                    {
                        tmp.AppendFormat("({0})", tmp2.ToString());
                        res = tt;
                        break;
                    }
                    if ((++cou) > 5) 
                        break;
                    if (tt.IsWhitespaceBefore || tt.IsWhitespaceAfter) 
                        break;
                    if (tt is Pullenti.Ner.ReferentToken) 
                        break;
                    tmp2.Append(tt.GetSourceText());
                }
            }
            if (tmp.Length > 2) 
            {
                if (tmp[tmp.Length - 1] == '3') 
                {
                    if (tmp[tmp.Length - 2] == 'К' || tmp[tmp.Length - 2] == 'Ф') 
                        tmp[tmp.Length - 1] = 'З';
                }
            }
            if ((res.Next is Pullenti.Ner.TextToken) && (res.WhitespacesAfterCount < 2) && res.Next.Chars.IsAllUpper) 
            {
                if (res.Next.IsValue("РД", null) || res.Next.IsValue("ПД", null)) 
                {
                    tmp.AppendFormat(" {0}", (res.Next as Pullenti.Ner.TextToken).Term);
                    res = res.Next;
                }
            }
            if ((res.Next is Pullenti.Ner.TextToken) && res.Next.IsChar('*')) 
                res = res.Next;
            return res;
        }
        public static List<DecreeToken> TryAttachList(Pullenti.Ner.Token t, DecreeToken prev = null, int maxCount = 10, bool mustStartByTyp = false)
        {
            DecreeToken p = TryAttach(t, prev, mustStartByTyp);
            if (p == null) 
                return null;
            if (p.Typ == ItemType.Org || p.Typ == ItemType.Owner) 
            {
                if (t.Previous != null && t.Previous.IsValue("РАССМОТРЕНИЕ", "РОЗГЛЯД")) 
                    return null;
            }
            if (p.Typ == ItemType.Number && (t is Pullenti.Ner.NumberToken)) 
            {
            }
            List<DecreeToken> res = new List<DecreeToken>();
            res.Add(p);
            Pullenti.Ner.Token tt = p.EndToken.Next;
            if (tt != null && t.Previous != null) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Previous, false, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt, false, null, false)) 
                {
                    p.BeginToken = t.Previous;
                    p.EndToken = tt;
                    tt = tt.Next;
                }
            }
            for (; tt != null; tt = tt.Next) 
            {
                bool ws = false;
                if (tt.WhitespacesBeforeCount > 15) 
                    ws = true;
                if (maxCount > 0 && res.Count >= maxCount) 
                {
                    DecreeToken la = res[res.Count - 1];
                    if (la.Typ != ItemType.Typ && la.Typ != ItemType.Date && la.Typ != ItemType.Number) 
                        break;
                    if (res.Count > (maxCount * 3)) 
                        break;
                }
                DecreeToken p0 = TryAttach(tt, prev ?? p, false);
                if (ws) 
                {
                    if (p0 == null || p == null) 
                        break;
                    if ((((p.Typ == ItemType.Typ && p0.Typ == ItemType.Number)) || ((p.Typ == ItemType.Date && p0.Typ == ItemType.Number)) || ((p0.Typ == ItemType.Name && p.Typ != ItemType.Name))) || ((p0.Typ == ItemType.Org && p.Typ == ItemType.Org))) 
                    {
                    }
                    else if ((((p0.Typ == ItemType.Date || p0.Typ == ItemType.Number)) && p.Typ == ItemType.Org && res.Count == 2) && res[0].Typ == ItemType.Typ) 
                    {
                    }
                    else 
                        break;
                }
                if (p0 == null) 
                {
                    if (tt.IsNewlineBefore) 
                        break;
                    if (tt.Morph.Class.IsPreposition && res[0].Typ == ItemType.Typ) 
                        continue;
                    if (tt.IsChar('.') && p.Typ == ItemType.Number && (tt.WhitespacesAfterCount < 3)) 
                    {
                        p0 = _TryAttach(tt.Next, p, 0, false);
                        if (p0 != null && ((p0.Typ == ItemType.Name || p0.Typ == ItemType.Date))) 
                            continue;
                        p0 = null;
                    }
                    if (((tt.IsCommaAnd || tt.IsHiphen)) && res[0].Typ == ItemType.Typ) 
                    {
                        p0 = TryAttach(tt.Next, p, false);
                        if (p0 != null) 
                        {
                            ItemType ty0 = p0.Typ;
                            if (ty0 == ItemType.Org || ty0 == ItemType.Owner) 
                                ty0 = ItemType.Unknown;
                            ItemType ty = p.Typ;
                            if (ty == ItemType.Org || ty == ItemType.Owner) 
                                ty = ItemType.Unknown;
                            if (ty0 == ty || p0.Typ == ItemType.Edition) 
                            {
                                p = p0;
                                res.Add(p);
                                tt = p.EndToken;
                                continue;
                            }
                        }
                        p0 = null;
                    }
                    if (tt.IsChar(':')) 
                    {
                        p0 = TryAttach(tt.Next, p, false);
                        if (p0 != null) 
                        {
                            if (p0.Typ == ItemType.Number || p0.Typ == ItemType.Date) 
                            {
                                p = p0;
                                res.Add(p);
                                tt = p.EndToken;
                                continue;
                            }
                        }
                    }
                    if (tt.IsComma && p.Typ == ItemType.Number) 
                    {
                        p0 = TryAttach(tt.Next, p, false);
                        if (p0 != null && p0.Typ == ItemType.Date) 
                        {
                            p = p0;
                            res.Add(p);
                            tt = p.EndToken;
                            continue;
                        }
                        int cou = 0;
                        if (res[0].Typ == ItemType.Typ) 
                        {
                            for (int ii = 1; ii < res.Count; ii++) 
                            {
                                if ((res[ii].Typ == ItemType.Org || res[ii].Typ == ItemType.Terr || res[ii].Typ == ItemType.Unknown) || res[ii].Typ == ItemType.Owner) 
                                    cou++;
                                else 
                                    break;
                            }
                            if (cou > 1) 
                            {
                                StringBuilder num = new StringBuilder(p.Value);
                                StringBuilder tmp = new StringBuilder();
                                Pullenti.Ner.Token tEnd = null;
                                for (Pullenti.Ner.Token tt1 = tt; tt1 != null; tt1 = tt1.Next) 
                                {
                                    if (!tt1.IsCommaAnd) 
                                        break;
                                    DecreeToken pp = TryAttach(tt1.Next, p, false);
                                    if (pp != null) 
                                        break;
                                    if (!(tt1.Next is Pullenti.Ner.NumberToken)) 
                                        break;
                                    tmp.Length = 0;
                                    Pullenti.Ner.Token tt2 = _tryAttachNumber(tt1.Next, tmp, true);
                                    if (tt2 == null) 
                                        break;
                                    num.AppendFormat(",{0}", tmp.ToString());
                                    cou--;
                                    tt1 = (tEnd = tt2);
                                }
                                if (cou == 1) 
                                {
                                    p.Value = num.ToString();
                                    tt = (p.EndToken = tEnd);
                                    continue;
                                }
                            }
                        }
                        p0 = null;
                    }
                    if (tt.IsComma && p.Typ == ItemType.Date) 
                    {
                        p0 = TryAttach(tt.Next, p, false);
                        if (p0 != null && p0.Typ == ItemType.Number) 
                        {
                            p = p0;
                            res.Add(p);
                            tt = p.EndToken;
                            continue;
                        }
                        p0 = null;
                    }
                    if (tt.IsCommaAnd && ((p.Typ == ItemType.Org || p.Typ == ItemType.Owner))) 
                    {
                        p0 = TryAttach(tt.Next, p, false);
                        if (p0 != null && ((p0.Typ == ItemType.Org || p.Typ == ItemType.Owner))) 
                        {
                            p = p0;
                            res.Add(p);
                            tt = p.EndToken;
                            continue;
                        }
                        p0 = null;
                    }
                    if (res[0].Typ == ItemType.Typ) 
                    {
                        if (GetKind(res[0].Value) == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                        {
                            if (tt.IsCharOf(",;")) 
                                continue;
                            if ((((p = TryAttach(tt, prev ?? res[0], false)))) != null) 
                            {
                                res.Add(p);
                                tt = p.EndToken;
                                continue;
                            }
                        }
                    }
                    if (res[res.Count - 1].Typ == ItemType.Unknown && prev != null) 
                    {
                        p0 = TryAttach(tt, res[res.Count - 1], false);
                        if (p0 != null) 
                        {
                            p = p0;
                            res.Add(p);
                            tt = p.EndToken;
                            continue;
                        }
                    }
                    if ((((tt is Pullenti.Ner.TextToken) && tt.Chars.IsAllUpper && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt.Next, false, false)) && res.Count > 1 && res[res.Count - 1].Typ == ItemType.Number) && res[res.Count - 2].Typ == ItemType.Typ && res[res.Count - 2].TypKind == Pullenti.Ner.Decree.DecreeKind.Standard) 
                        continue;
                    if (tt.IsChar('(')) 
                    {
                        p = TryAttach(tt.Next, null, false);
                        if (p != null && p.Typ == ItemType.Edition) 
                        {
                            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br != null) 
                            {
                                res.Add(p);
                                for (tt = p.EndToken.Next; tt != null; tt = tt.Next) 
                                {
                                    if (tt.EndChar >= br.EndChar) 
                                        break;
                                    p = TryAttach(tt, null, false);
                                    if (p != null) 
                                    {
                                        res.Add(p);
                                        tt = p.EndToken;
                                    }
                                }
                                tt = (res[res.Count - 1].EndToken = br.EndToken);
                                continue;
                            }
                        }
                    }
                    if ((tt is Pullenti.Ner.NumberToken) && res[res.Count - 1].Typ == ItemType.Date) 
                    {
                        if (tt.Previous != null && tt.Previous.Morph.Class.IsPreposition) 
                        {
                        }
                        else if (Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(tt) != null) 
                        {
                        }
                        else 
                        {
                            StringBuilder tmp = new StringBuilder();
                            Pullenti.Ner.Token t11 = _tryAttachNumber(tt, tmp, false);
                            if (t11 != null) 
                                p0 = new DecreeToken(tt, t11) { Typ = ItemType.Number, Value = tmp.ToString() };
                        }
                    }
                    if (p0 == null) 
                        break;
                }
                p = p0;
                res.Add(p);
                tt = p.EndToken;
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (res[i].EndToken.Next.IsComma) 
                    continue;
                if (res[i].Typ == ItemType.Unknown && res[i + 1].Typ == ItemType.Unknown) 
                {
                    res[i].Value = string.Format("{0} {1}", res[i].Value, res[i + 1].Value);
                    res[i].EndToken = res[i + 1].EndToken;
                    res.RemoveAt(i + 1);
                    i--;
                }
                else if (((res[i].Typ == ItemType.Org || res[i].Typ == ItemType.Owner)) && res[i + 1].Typ == ItemType.Unknown) 
                {
                    bool ok = false;
                    if (res[i + 1].BeginToken.Previous.IsComma) 
                    {
                    }
                    else if (((i + 2) < res.Count) && res[i + 2].Typ == ItemType.Date) 
                        ok = true;
                    if (ok) 
                    {
                        res[i].Typ = ItemType.Owner;
                        res[i].Value = string.Format("{0} {1}", res[i].Value, res[i + 1].Value);
                        res[i].EndToken = res[i + 1].EndToken;
                        res[i].Ref = null;
                        res.RemoveAt(i + 1);
                        i--;
                    }
                }
                else if (((res[i].Typ == ItemType.Unknown || res[i].Typ == ItemType.Owner)) && ((res[i + 1].Typ == ItemType.Org || res[i + 1].Typ == ItemType.Owner))) 
                {
                    bool ok = false;
                    if ((res[i].Typ == ItemType.Owner || res[i + 1].Typ == ItemType.Owner || res[i].Value == "Пленум") || res[i].Value == "Сессия" || res[i].Value == "Съезд") 
                        ok = true;
                    if (ok) 
                    {
                        res[i].Typ = ItemType.Owner;
                        res[i].EndToken = res[i + 1].EndToken;
                        if (res[i].Value != null) 
                        {
                            string s1 = res[i + 1].Value;
                            if (s1 == null) 
                                s1 = res[i + 1].Ref.Referent.ToString();
                            res[i].Value = string.Format("{0}, {1}", res[i].Value, s1);
                        }
                        res.RemoveAt(i + 1);
                        i--;
                    }
                }
                else if ((res[i].Typ == ItemType.Typ && res[i + 1].Typ == ItemType.Terr && ((i + 2) < res.Count)) && res[i + 2].Typ == ItemType.StdName) 
                {
                    res[i].FullValue = string.Format("{0} {1}", res[i].Value, res[i + 2].Value);
                    res[i + 1].EndToken = res[i + 2].EndToken;
                    res.RemoveAt(i + 2);
                    i--;
                }
                else 
                {
                    bool ok = false;
                    if (res[i].Typ == ItemType.Unknown && ((((res[i + 1].Typ == ItemType.Terr && prev != null)) || res[i + 1].Typ == ItemType.Owner))) 
                        ok = true;
                    else if (((res[i].Typ == ItemType.Unknown || res[i].Typ == ItemType.Org || res[i].Typ == ItemType.Owner)) && res[i + 1].Typ == ItemType.Terr) 
                        ok = true;
                    if (ok) 
                    {
                        res[i].Typ = ItemType.Owner;
                        res[i].EndToken = res[i + 1].EndToken;
                        string s1 = res[i + 1].Value;
                        if (s1 == null) 
                            s1 = res[i + 1].Ref.Referent.ToString();
                        res[i].Value = string.Format("{0}, {1}", res[i].Value, s1);
                        res.RemoveAt(i + 1);
                        i--;
                    }
                }
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (res[i].Typ == ItemType.Unknown) 
                {
                    int j;
                    bool ok = false;
                    for (j = i + 1; j < res.Count; j++) 
                    {
                        if (res[j].BeginToken.Previous.IsComma) 
                            break;
                        else if (res[j].Typ == ItemType.Date || res[j].Typ == ItemType.Number) 
                        {
                            ok = true;
                            break;
                        }
                        else if (res[j].Typ == ItemType.Terr || res[j].Typ == ItemType.Org || res[j].Typ == ItemType.Unknown) 
                        {
                        }
                        else 
                            break;
                    }
                    if (!ok) 
                        continue;
                    if (j == (i + 1)) 
                    {
                        if (res[i].BeginToken.Previous.IsComma) 
                            res[i].Typ = ItemType.Owner;
                        continue;
                    }
                    StringBuilder tmp = new StringBuilder();
                    for (int ii = i; ii < j; ii++) 
                    {
                        if (ii > i) 
                        {
                            if (res[ii].Typ == ItemType.Terr) 
                                tmp.AppendFormat(", ");
                            else 
                                tmp.Append(' ');
                        }
                        if (res[ii].Value != null) 
                            tmp.Append(res[ii].Value);
                        else if (res[ii].Ref != null && res[ii].Ref.Referent != null) 
                            tmp.Append(res[ii].Ref.Referent.ToString());
                    }
                    res[i].Value = tmp.ToString();
                    res[i].EndToken = res[j - 1].EndToken;
                    res[i].Typ = ItemType.Owner;
                    res.RemoveRange(i + 1, j - i - 1);
                }
            }
            if ((res.Count == 3 && res[0].Typ == ItemType.Typ && ((res[1].Typ == ItemType.Owner || res[1].Typ == ItemType.Org || res[1].Typ == ItemType.Terr))) && res[2].Typ == ItemType.Number) 
            {
                Pullenti.Ner.Token te = res[2].EndToken.Next;
                for (; te != null; te = te.Next) 
                {
                    if (!te.IsChar(',') || te.Next == null) 
                        break;
                    List<DecreeToken> res1 = TryAttachList(te.Next, res[0], 10, false);
                    if (res1 == null || (res1.Count < 2)) 
                        break;
                    if (((res1[0].Typ == ItemType.Owner || res1[0].Typ == ItemType.Org || res1[0].Typ == ItemType.Terr)) && res1[1].Typ == ItemType.Number) 
                    {
                        res.AddRange(res1);
                        te = res1[res1.Count - 1].EndToken;
                    }
                    else 
                        break;
                }
            }
            if (res.Count > 1 && ((res[res.Count - 1].Typ == ItemType.Owner || res[res.Count - 1].Typ == ItemType.Org))) 
            {
                Pullenti.Ner.Token te = res[res.Count - 1].EndToken.Next;
                if (te != null && te.IsCommaAnd) 
                {
                    List<DecreeToken> res1 = TryAttachList(te.Next, res[0], 10, false);
                    if (res1 != null && res1.Count > 0) 
                    {
                        if (res1[0].Typ == ItemType.Owner || res1[0].Typ == ItemType.Org) 
                            res.AddRange(res1);
                    }
                }
            }
            return res;
        }
        public static DecreeToken TryAttachName(Pullenti.Ner.Token t, string typ, bool veryProbable = false, bool inTitleDocRef = false)
        {
            if (t == null) 
                return null;
            if (t.IsChar(';')) 
                t = t.Next;
            if (t == null) 
                return null;
            Pullenti.Ner.Mail.Internal.MailLine li = Pullenti.Ner.Mail.Internal.MailLine.Parse(t, 0, 20);
            if (li != null) 
            {
                if (li.Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.Hello) 
                    return null;
            }
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = t;
            bool abou = false;
            Pullenti.Ner.Decree.DecreeKind ty = GetKind(typ);
            if (t.IsValue("О", null) || t.IsValue("ОБ", null) || t.IsValue("ПРО", null)) 
            {
                t = t.Next;
                abou = true;
            }
            else if (t.IsValue("ПО", null)) 
            {
                if (Pullenti.Morph.LanguageHelper.EndsWith(typ, "ЗАКОН")) 
                    return null;
                t = t.Next;
                abou = true;
                if (t != null) 
                {
                    if (t.IsValue("ПОЗИЦИЯ", null)) 
                        return null;
                }
            }
            else if (t.Next != null) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null && br.IsQuoteType) 
                    {
                        Pullenti.Ner.Referent re = t.Next.GetReferent();
                        if (re != null && re.TypeName == "URI") 
                            return null;
                        if (t.Next.Chars.IsLetter) 
                        {
                            if (t.Next.Chars.IsAllLower || (((t.Next is Pullenti.Ner.TextToken) && (t.Next as Pullenti.Ner.TextToken).IsPureVerb))) 
                                return null;
                        }
                        t1 = br.EndToken;
                        Pullenti.Ner.Token tt1 = _tryAttachStdChangeName(t.Next);
                        if (tt1 != null) 
                            t1 = tt1;
                        string s0 = Pullenti.Ner.Core.MiscHelper.GetTextValue(t0, t1, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        if (string.IsNullOrEmpty(s0)) 
                            return null;
                        if ((s0.Length < 10) && typ != "ПРОГРАММА" && typ != "ПРОГРАМА") 
                            return null;
                        return new DecreeToken(t, t1) { Typ = ItemType.Name, Value = s0 };
                    }
                    DecreeToken dt = TryAttachName(t.Next, typ, false, false);
                    if (dt != null) 
                    {
                        dt.BeginToken = t;
                        return dt;
                    }
                }
                if (ty != Pullenti.Ner.Decree.DecreeKind.Konvention && ty != Pullenti.Ner.Decree.DecreeKind.Program) 
                    return null;
            }
            if (t == null) 
                return null;
            if (t.IsValue("ЗАЯВЛЕНИЕ", "ЗАЯВА")) 
                return null;
            Pullenti.Ner.Token tt;
            int cou = 0;
            List<Pullenti.Ner.Core.NounPhraseToken> npts = new List<Pullenti.Ner.Core.NounPhraseToken>();
            for (tt = t; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore && tt != t) 
                {
                    if (tt.WhitespacesBeforeCount > 15 || !abou) 
                        break;
                    if (tt.IsValue("ИСТОЧНИК", null)) 
                        break;
                    if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter && tt.Chars.IsAllLower) 
                    {
                    }
                    else 
                        break;
                }
                if (tt.IsCharOf("(,") && tt.Next != null) 
                {
                    if (tt.Next.IsValue("УТВЕРЖДЕННЫЙ", "ЗАТВЕРДЖЕНИЙ") || tt.Next.IsValue("ПРИНЯТЫЙ", "ПРИЙНЯТИЙ") || tt.Next.IsValue("УТВ", "ЗАТВ")) 
                    {
                        Pullenti.Ner.Token ttt = tt.Next.Next;
                        if (ttt != null && ttt.IsChar('.') && tt.Next.IsValue("УТВ", null)) 
                            ttt = ttt.Next;
                        DecreeToken dt = DecreeToken.TryAttach(ttt, null, false);
                        if (dt != null && dt.Typ == ItemType.Typ) 
                            break;
                        if (dt != null && ((dt.Typ == ItemType.Org || dt.Typ == ItemType.Owner))) 
                        {
                            DecreeToken dt2 = DecreeToken.TryAttach(dt.EndToken.Next, null, false);
                            if (dt2 != null && dt2.Typ == ItemType.Date) 
                                break;
                        }
                    }
                }
                if (veryProbable && abou && !tt.IsNewlineBefore) 
                {
                    t1 = tt;
                    continue;
                }
                if (tt.IsValue("ОТ", "ВІД")) 
                {
                    DecreeToken dt = DecreeToken.TryAttach(tt, null, false);
                    if (dt != null) 
                        break;
                }
                if (tt.Morph.Class.IsPreposition && tt.Next != null && (((tt.Next.GetReferent() is Pullenti.Ner.Date.DateReferent) || (tt.Next.GetReferent() is Pullenti.Ner.Date.DateRangeReferent)))) 
                    break;
                if (inTitleDocRef) 
                {
                    t1 = tt;
                    continue;
                }
                Pullenti.Ner.Core.ConjunctionToken conj = Pullenti.Ner.Core.ConjunctionHelper.TryParse(tt);
                if (conj != null) 
                {
                    if (cou == 0) 
                        break;
                    if (conj.EndToken.Next == null) 
                        break;
                    tt = conj.EndToken;
                    continue;
                }
                if (!tt.Chars.IsCyrillicLetter) 
                    break;
                if (tt.Morph.Class.IsPersonalPronoun || tt.Morph.Class.IsPronoun) 
                {
                    if (!tt.IsValue("ВСЕ", "ВСІ") && !tt.IsValue("ВСЯКИЙ", null) && !tt.IsValue("ДАННЫЙ", "ДАНИЙ")) 
                        break;
                }
                if (tt is Pullenti.Ner.NumberToken) 
                    break;
                PartToken pit = PartToken.TryAttach(tt, null, false, false);
                if (pit != null) 
                    break;
                Pullenti.Ner.Referent r = tt.GetReferent();
                if (r != null) 
                {
                    if (((r is Pullenti.Ner.Decree.DecreeReferent) || (r is Pullenti.Ner.Date.DateReferent) || (r is Pullenti.Ner.Org.OrganizationReferent)) || (r is Pullenti.Ner.Geo.GeoReferent) || r.TypeName == "NAMEDENTITY") 
                    {
                        if (tt.IsNewlineBefore) 
                            break;
                        t1 = tt;
                        continue;
                    }
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition | Pullenti.Ner.Core.NounPhraseParseAttr.ParseVerbs, 0, null);
                if (npt == null) 
                {
                    if (tt.Morph.Class.IsPreposition) 
                        continue;
                    break;
                }
                Pullenti.Morph.MorphClass dd = npt.EndToken.GetMorphClassInDictionary();
                if (dd.IsVerb && npt.EndToken == npt.BeginToken) 
                {
                    if (!dd.IsNoun) 
                        break;
                    if (tt.IsValue("БЫТЬ", "БУТИ")) 
                        break;
                }
                if (!npt.Morph.Case.IsGenitive) 
                {
                    if (cou > 0) 
                    {
                        if ((npt.Morph.Case.IsInstrumental && tt.Previous != null && tt.Previous.Previous != null) && (tt.Previous.Previous.IsValue("РАБОТА", "РОБОТА"))) 
                        {
                        }
                        else if (abou && veryProbable) 
                        {
                        }
                        else if (npt.Noun.IsValue("ГОД", "РІК") || npt.Noun.IsValue("ПЕРИОД", "ПЕРІОД")) 
                        {
                        }
                        else 
                        {
                            bool ok = false;
                            foreach (Pullenti.Ner.Core.NounPhraseToken n in npts) 
                            {
                                List<Pullenti.Semantic.Core.SemanticLink> links = Pullenti.Semantic.Core.SemanticHelper.TryCreateLinks(n, npt, null);
                                if (links.Count > 0) 
                                {
                                    ok = true;
                                    break;
                                }
                                if (n == npts[npts.Count - 1]) 
                                {
                                    if (!((npts[npts.Count - 1].Morph.Case & npt.Morph.Case)).IsUndefined) 
                                        ok = true;
                                }
                            }
                            if (!ok) 
                                break;
                        }
                    }
                    if (!abou) 
                        break;
                }
                cou++;
                tt = (t1 = npt.EndToken);
                if (npt.Noun.IsValue("НАЛОГОПЛАТЕЛЬЩИК", null)) 
                {
                    Pullenti.Ner.Token ttn = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(tt.Next);
                    if ((ttn is Pullenti.Ner.NumberToken) && (ttn as Pullenti.Ner.NumberToken).Value == "1") 
                        tt = (t1 = ttn);
                }
                npts.Add(npt);
            }
            if (tt == t) 
                return null;
            if (abou) 
            {
                Pullenti.Ner.Token tt1 = _tryAttachStdChangeName(t0);
                if (tt1 != null && tt1.EndChar > t1.EndChar) 
                    t1 = tt1;
            }
            if (t0.Previous != null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t0.Previous, true, false) && !Pullenti.Ner.Core.BracketHelper.IsBracket(t1, false)) 
            {
                int co = 0;
                for (Pullenti.Ner.Token ttt = t1.Next; ttt != null && (cou < 40); ttt = ttt.Next,co++) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(ttt, true, t0.Previous, false)) 
                    {
                        t1 = ttt;
                        t0 = t0.Previous;
                        break;
                    }
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(ttt, true)) 
                        break;
                }
            }
            string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(t0, t1, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
            if (string.IsNullOrEmpty(s) || (s.Length < 10)) 
                return null;
            return new DecreeToken(t, t1) { Typ = ItemType.Name, Value = s };
        }
        internal static Pullenti.Ner.Token _tryAttachStdChangeName(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken) || t.Next == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            string term = (t as Pullenti.Ner.TextToken).Term;
            if ((term != "О" && term != "O" && term != "ОБ") && term != "ПРО") 
                return null;
            t = t.Next;
            if (((t.IsValue("ВНЕСЕНИЕ", "ВНЕСЕННЯ") || t.IsValue("УТВЕРЖДЕНИЕ", "ТВЕРДЖЕННЯ") || t.IsValue("ПРИНЯТИЕ", "ПРИЙНЯТТЯ")) || t.IsValue("ВВЕДЕНИЕ", "ВВЕДЕННЯ") || t.IsValue("ПРИОСТАНОВЛЕНИЕ", "ПРИЗУПИНЕННЯ")) || t.IsValue("ОТМЕНА", "СКАСУВАННЯ") || t.IsValue("МЕРА", "ЗАХІД")) 
            {
            }
            else if (t.IsValue("ПРИЗНАНИЕ", "ВИЗНАННЯ") && t.Next != null && t.Next.IsValue("УТРАТИТЬ", "ВТРАТИТИ")) 
            {
            }
            else 
                return null;
            Pullenti.Ner.Token t1 = t;
            for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore) 
                {
                }
                if (tt.WhitespacesBeforeCount > 15) 
                    break;
                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                    break;
                if (tt.Morph.Class.IsConjunction || tt.Morph.Class.IsPreposition) 
                    continue;
                if (tt.IsComma) 
                    continue;
                Pullenti.Ner.ReferentToken rtt = Pullenti.Ner.Decree.DecreeAnalyzer.TryAttachApproved(tt, null);
                if (rtt != null && t0.Previous != null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t0.Previous, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t0.Previous, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null && (br.EndChar < rtt.EndChar)) 
                        rtt = null;
                }
                if (rtt != null) 
                {
                    t1 = (tt = rtt.EndToken);
                    continue;
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    if ((((((npt.Noun.IsValue("ВВЕДЕНИЕ", "ВВЕДЕННЯ") || npt.Noun.IsValue("ПРИОСТАНОВЛЕНИЕ", "ПРИЗУПИНЕННЯ") || npt.Noun.IsValue("ВНЕСЕНИЕ", "ВНЕСЕННЯ")) || npt.Noun.IsValue("ИЗМЕНЕНИЕ", "ЗМІНА") || npt.Noun.IsValue("ДОПОЛНЕНИЕ", "ДОДАТОК")) || npt.Noun.IsValue("АКТ", null) || npt.Noun.IsValue("ПРИЗНАНИЕ", "ВИЗНАННЯ")) || npt.Noun.IsValue("ПРИНЯТИЕ", "ПРИЙНЯТТЯ") || npt.Noun.IsValue("СИЛА", "ЧИННІСТЬ")) || npt.Noun.IsValue("ДЕЙСТВИЕ", "ДІЯ") || npt.Noun.IsValue("СВЯЗЬ", "ЗВЯЗОК")) || npt.Noun.IsValue("РЕАЛИЗАЦИЯ", "РЕАЛІЗАЦІЯ") || npt.Noun.IsValue("РЯД", null)) 
                    {
                        t1 = (tt = npt.EndToken);
                        continue;
                    }
                }
                if (tt.IsValue("ТАКЖЕ", "ТАКОЖ") || tt.IsValue("НЕОБХОДИМЫЙ", "НЕОБХІДНИЙ")) 
                    continue;
                Pullenti.Ner.Referent r = tt.GetReferent();
                if ((r is Pullenti.Ner.Geo.GeoReferent) || (r is Pullenti.Ner.Decree.DecreeReferent) || (r is Pullenti.Ner.Decree.DecreePartReferent)) 
                {
                    t1 = tt;
                    continue;
                }
                if ((r is Pullenti.Ner.Org.OrganizationReferent) && tt.IsNewlineAfter) 
                {
                    t1 = tt;
                    continue;
                }
                List<PartToken> pts = PartToken.TryAttachList(tt, false, 40);
                while (pts != null && pts.Count > 0) 
                {
                    if (pts[0].Typ == PartToken.ItemType.Prefix) 
                        pts.RemoveAt(0);
                    else 
                        break;
                }
                if (pts != null && pts.Count > 0) 
                {
                    t1 = (tt = pts[pts.Count - 1].EndToken);
                    continue;
                }
                List<DecreeToken> dts = DecreeToken.TryAttachList(tt, null, 10, false);
                if (dts != null && dts.Count > 0) 
                {
                    List<Pullenti.Ner.ReferentToken> rts = Pullenti.Ner.Decree.DecreeAnalyzer.TryAttach(dts, null, null);
                    if (rts != null) 
                    {
                        t1 = (tt = rts[0].EndToken);
                        continue;
                    }
                    if (dts[0].Typ == ItemType.Typ) 
                    {
                        Pullenti.Ner.ReferentToken rt = Pullenti.Ner.Decree.DecreeAnalyzer.TryAttachApproved(tt, null);
                        if (rt != null) 
                        {
                            t1 = (tt = rt.EndToken);
                            continue;
                        }
                    }
                }
                Pullenti.Ner.Token tt1 = IsKeyword(tt, false);
                if (tt1 != null) 
                {
                    t1 = (tt = tt1);
                    continue;
                }
                if (tt is Pullenti.Ner.NumberToken) 
                    continue;
                if (!tt.Chars.IsAllLower && tt.LengthChar > 2 && tt.GetMorphClassInDictionary().IsUndefined) 
                {
                    t1 = tt;
                    continue;
                }
                break;
            }
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t0.Previous, true, false)) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next, true, t0.Previous, false)) 
                    t1 = t1.Next;
            }
            return t1;
        }
        static Pullenti.Ner.Core.TerminCollection m_Termins;
        static Pullenti.Ner.Core.TerminCollection m_Keywords;
        public static void Initialize()
        {
            if (m_Termins != null) 
                return;
            m_Termins = new Pullenti.Ner.Core.TerminCollection();
            m_Keywords = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in m_MiscTypesRU) 
            {
                m_Keywords.Add(new Pullenti.Ner.Core.Termin(s));
            }
            foreach (string s in m_MiscTypesUA) 
            {
                m_Keywords.Add(new Pullenti.Ner.Core.Termin(s) { Lang = Pullenti.Morph.MorphLang.UA });
            }
            Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin("ТЕХНИЧЕСКОЕ ЗАДАНИЕ") { Acronym = "ТЗ" };
            t.AddVariant("ТЕХЗАДАНИЕ", false);
            t.AddAbridge("ТЕХ. ЗАДАНИЕ");
            m_Keywords.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТЕХНИКО КОММЕРЧЕСКОЕ ПРЕДЛОЖЕНИЕ") { Acronym = "ТКП" };
            t.AddVariant("КОММЕРЧЕСКОЕ ПРЕДЛОЖЕНИЕ", false);
            m_Keywords.Add(t);
            foreach (string s in m_AllTypesRU) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = ItemType.Typ });
                m_Keywords.Add(new Pullenti.Ner.Core.Termin(s) { Tag = ItemType.Typ });
            }
            foreach (string s in m_AllTypesUA) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = ItemType.Typ, Lang = Pullenti.Morph.MorphLang.UA });
                m_Keywords.Add(new Pullenti.Ner.Core.Termin(s) { Tag = ItemType.Typ, Lang = Pullenti.Morph.MorphLang.UA });
            }
            m_Termins.Add(new Pullenti.Ner.Core.Termin("ОТРАСЛЕВОЕ СОГЛАШЕНИЕ") { Tag = ItemType.Typ });
            m_Termins.Add(new Pullenti.Ner.Core.Termin("ГАЛУЗЕВА УГОДА", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ });
            m_Termins.Add(new Pullenti.Ner.Core.Termin("МЕЖОТРАСЛЕВОЕ СОГЛАШЕНИЕ") { Tag = ItemType.Typ });
            m_Termins.Add(new Pullenti.Ner.Core.Termin("МІЖГАЛУЗЕВА УГОДА", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ });
            m_Termins.Add(new Pullenti.Ner.Core.Termin("ОСНОВЫ ЗАКОНОДАТЕЛЬСТВА") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex });
            m_Termins.Add(new Pullenti.Ner.Core.Termin("ОСНОВИ ЗАКОНОДАВСТВА", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex });
            m_Termins.Add(new Pullenti.Ner.Core.Termin("ОСНОВЫ ГРАЖДАНСКОГО ЗАКОНОДАТЕЛЬСТВА") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex });
            m_Termins.Add(new Pullenti.Ner.Core.Termin("ОСНОВИ ЦИВІЛЬНОГО ЗАКОНОДАВСТВА", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex });
            t = new Pullenti.Ner.Core.Termin("ФЕДЕРАЛЬНЫЙ ЗАКОН") { Tag = ItemType.Typ, Acronym = "ФЗ" };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ФЕДЕРАЛЬНИЙ ЗАКОН", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ФЗ" };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОЕКТ ЗАКОНА") { Tag = ItemType.Typ };
            t.AddVariant("ЗАКОНОПРОЕКТ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПАСПОРТ ПРОЕКТА") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОЕКТ ЗАКОНУ", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ };
            t.AddVariant("ЗАКОНОПРОЕКТ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПАСПОРТ ПРОЕКТУ", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОСУДАРСТВЕННАЯ ПРОГРАММА") { CanonicText = "ПРОГРАММА", Tag = ItemType.Typ };
            t.AddVariant("ГОСУДАРСТВЕННАЯ ЦЕЛЕВАЯ ПРОГРАММА", false);
            t.AddVariant("ФЕДЕРАЛЬНАЯ ЦЕЛЕВАЯ ПРОГРАММА", false);
            t.AddAbridge("ФЕДЕРАЛЬНАЯ ПРОГРАММА");
            t.AddVariant("МЕЖГОСУДАРСТВЕННАЯ ЦЕЛЕВАЯ ПРОГРАММА", false);
            t.AddAbridge("МЕЖГОСУДАРСТВЕННАЯ ПРОГРАММА");
            t.AddVariant("ГОСПРОГРАММА", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДЕРЖАВНА ПРОГРАМА") { CanonicText = "ПРОГРАМА", Lang = Pullenti.Morph.MorphLang.UA, Tag = ItemType.Typ };
            t.AddVariant("ДЕРЖАВНА ЦІЛЬОВА ПРОГРАМА", false);
            t.AddVariant("ФЕДЕРАЛЬНА ЦІЛЬОВА ПРОГРАМА", false);
            t.AddAbridge("ФЕДЕРАЛЬНА ПРОГРАМА");
            t.AddVariant("ДЕРЖПРОГРАМА", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ФЕДЕРАЛЬНЫЙ КОНСТИТУЦИОННЫЙ ЗАКОН") { Tag = ItemType.Typ, Acronym = "ФКЗ" };
            t.AddVariant("КОНСТИТУЦИОННЫЙ ЗАКОН", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ФЕДЕРАЛЬНИЙ КОНСТИТУЦІЙНИЙ ЗАКОН", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ФКЗ" };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("УГОЛОВНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "УК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КРИМИНАЛЬНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "КК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КРИМІНАЛЬНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "КК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("УГОЛОВНО-ПРОЦЕССУАЛЬНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "УПК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КРИМІНАЛЬНО-ПРОЦЕСУАЛЬНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "КПК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("УГОЛОВНО-ИСПОЛНИТЕЛЬНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "УИК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КРИМІНАЛЬНО-ВИКОНАВЧИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "КВК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГРАЖДАНСКИЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ГК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЦИВІЛЬНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ЦК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГРАЖДАНСКИЙ ПРОЦЕССУАЛЬНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ГПК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЦИВІЛЬНИЙ ПРОЦЕСУАЛЬНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ЦПК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГРАДОСТРОИТЕЛЬНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ГРК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("МІСТОБУДІВНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "МБК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ХОЗЯЙСТВЕННЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ХК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОСПОДАРСЬКИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ГК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ХОЗЯЙСТВЕННЫЙ ПРОЦЕССУАЛЬНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ХПК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОСПОДАРСЬКИЙ ПРОЦЕСУАЛЬНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ГПК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("АРБИТРАЖНЫЙ ПРОЦЕССУАЛЬНЫЙ КОДЕКС") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            t.AddAbridge("АПК");
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("АРБІТРАЖНИЙ ПРОЦЕСУАЛЬНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            t.AddAbridge("АПК");
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОДЕКС ВНУТРЕННЕГО ВОДНОГО ТРАНСПОРТА") { Tag = ItemType.Typ, Acronym = "КВВТ", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            t.AddVariant("КВ ВТ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТРУДОВОЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ТК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТРУДОВИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ТК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОДЕКС ЗАКОНОВ О ТРУДЕ") { Acronym = "КЗОТ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОДЕКС ЗАКОНІВ ПРО ПРАЦЮ", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "КЗПП", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЖИЛИЩНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ЖК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЖИТЛОВИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ЖК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗЕМЕЛЬНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ЗК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗЕМЕЛЬНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ЗК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛЕСНОЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ЛК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛІСОВИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ЛК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЮДЖЕТНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "БК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЮДЖЕТНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "БК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("НАЛОГОВЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "НК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОДАТКОВИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ПК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕМЕЙНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "СК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СІМЕЙНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "СК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВОДНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ВК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВОДНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ВК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВОЗДУШНЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ВК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОВІТРЯНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ПК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОДЕКС ОБ АДМИНИСТРАТИВНЫХ ПРАВОНАРУШЕНИЯХ") { Tag = ItemType.Typ, Acronym = "КОАП", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОДЕКС ПРО АДМІНІСТРАТИВНІ ПРАВОПОРУШЕННЯ", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "КОАП", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОБ АДМИНИСТРАТИВНЫХ ПРАВОНАРУШЕНИЯХ") { Tag = ItemType.StdName };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРО АДМІНІСТРАТИВНІ ПРАВОПОРУШЕННЯ", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.StdName };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОДЕКС ОБ АДМИНИСТРАТИВНЫХ ПРАВОНАРУШЕНИЯХ") { Tag = ItemType.Typ, Acronym = "КРКОАП", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            t.AddVariant("КРК ОБ АП", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОДЕКС АДМИНИСТРАТИВНОГО СУДОПРОИЗВОДСТВА") { Tag = ItemType.Typ, Acronym = "КАС", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОДЕКС АДМІНІСТРАТИВНОГО СУДОЧИНСТВА", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "КАС", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТАМОЖЕННЫЙ КОДЕКС") { Tag = ItemType.Typ, Acronym = "ТК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИТНИЙ КОДЕКС", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "МК", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОДЕКС ТОРГОВОГО МОРЕПЛАВАНИЯ") { Tag = ItemType.Typ, Acronym = "КТМ", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОДЕКС ТОРГОВЕЛЬНОГО МОРЕПЛАВСТВА", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "КТМ", Tag2 = Pullenti.Ner.Decree.DecreeKind.Kodex };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРАВИЛА ДОРОЖНОГО ДВИЖЕНИЯ") { Tag = ItemType.Typ, Acronym = "ПДД", Tag2 = "ПРАВИЛА" };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРАВИЛА ДОРОЖНЬОГО РУХУ", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ, Acronym = "ПДР", Tag2 = "ПРАВИЛА" };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СОБРАНИЕ ЗАКОНОДАТЕЛЬСТВА") { Tag = ItemType.Typ };
            t.AddAbridge("СЗ");
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОФИЦИАЛЬНЫЙ ВЕСТНИК") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОФІЦІЙНИЙ ВІСНИК", Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СВОД ЗАКОНОВ") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЮЛЛЕТЕНЬ НОРМАТИВНЫХ АКТОВ ФЕДЕРАЛЬНЫХ ОРГАНОВ ИСПОЛНИТЕЛЬНОЙ ВЛАСТИ") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЮЛЛЕТЕНЬ МЕЖДУНАРОДНЫХ ДОГОВОРОВ") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЮЛЛЕТЕНЬ ВЕРХОВНОГО СУДА") { Tag = ItemType.Typ };
            t.AddVariant("БЮЛЛЕТЕНЬ ВС", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕСТНИК ВЫСШЕГО АРБИТРАЖНОГО СУДА") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕСТНИК БАНКА РОССИИ") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("РОССИЙСКАЯ ГАЗЕТА") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("РОССИЙСКИЕ ВЕСТИ") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СОБРАНИЕ АКТОВ ПРЕЗИДЕНТА И ПРАВИТЕЛЬСТВА") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕДОМОСТИ ВЕРХОВНОГО СОВЕТА") { Tag = ItemType.Typ };
            t.AddVariant("ВЕДОМОСТИ ВС", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕДОМОСТИ СЪЕЗДА НАРОДНЫХ ДЕПУТАТОВ И ВЕРХОВНОГО СОВЕТА") { Tag = ItemType.Typ };
            t.AddVariant("ВЕДОМОСТИ СЪЕЗДА НАРОДНЫХ ДЕПУТАТОВ РФ И ВЕРХОВНОГО СОВЕТА", false);
            t.AddVariant("ВЕДОМОСТИ СЪЕЗДА НАРОДНЫХ ДЕПУТАТОВ", false);
            t.AddVariant("ВЕДОМОСТИ СНД РФ И ВС", false);
            t.AddVariant("ВЕДОМОСТИ СНД И ВС", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЮЛЛЕТЕНЬ НОРМАТИВНЫХ АКТОВ МИНИСТЕРСТВ И ВЕДОМСТВ") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            m_Termins.Add(new Pullenti.Ner.Core.Termin("СВОД ЗАКОНОВ") { Tag = ItemType.Typ });
            m_Termins.Add(new Pullenti.Ner.Core.Termin("ВЕДОМОСТИ") { Tag = ItemType.Typ });
            t = new Pullenti.Ner.Core.Termin("ЗАРЕГИСТРИРОВАТЬ") { CanonicText = "РЕГИСТРАЦИЯ", Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗАРЕЄСТРУВАТИ", Pullenti.Morph.MorphLang.UA) { CanonicText = "РЕЄСТРАЦІЯ", Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТАНДАРТ") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("МЕЖДУНАРОДНЫЙ СТАНДАРТ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЕДИНЫЙ ОТРАСЛЕВОЙ СТАНДАРТ ЗАКУПОК") { Tag = ItemType.Typ, Acronym = "ЕОСЗ" };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЕДИНЫЙ ОТРАСЛЕВОЙ ПОРЯДОК") { Tag = ItemType.Typ };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОСТ") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("ГОСУДАРСТВЕННЫЙ СТАНДАРТ", false);
            t.AddVariant("ГОССТАНДАРТ", false);
            t.AddVariant("НАЦИОНАЛЬНЫЙ СТАНДАРТ", false);
            t.AddVariant("МЕЖГОСУДАРСТВЕННЫЙ СТАНДАРТ", false);
            t.AddVariant("ДЕРЖАВНИЙ СТАНДАРТ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОСТ") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("ОТРАСЛЕВОЙ СТАНДАРТ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПНСТ") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("ПРЕДВАРИТЕЛЬНЫЙ НАЦИОНАЛЬНЫЙ СТАНДАРТ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("РСТ") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("РЕСПУБЛИКАНСКИЙ СТАНДАРТ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПБУ") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("ПОЛОЖЕНИЕ ПО БУХГАЛТЕРСКОМУ УЧЕТУ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ISO") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("ИСО", false);
            t.AddVariant("ISO/IEC", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТЕХНИЧЕСКИЕ УСЛОВИЯ") { Acronym = "ТУ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("ТЕХУСЛОВИЯ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ФЕДЕРАЛЬНЫЕ НОРМЫ И ПРАВИЛА") { Acronym = "ФНП", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("НОРМАТИВНЫЕ ПРАВИЛА") { Acronym = "НП", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТРОИТЕЛЬНЫЕ НОРМЫ И ПРАВИЛА") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("СНИП", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТРОИТЕЛЬНЫЕ НОРМЫ") { Acronym = "СН", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("CH", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕДОМСТВЕННЫЕ СТРОИТЕЛЬНЫЕ НОРМЫ") { Acronym = "ВСН", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("BCH", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("РЕСПУБЛИКАНСКИЕ СТРОИТЕЛЬНЫЕ НОРМЫ") { Acronym = "РСН", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("PCH", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРАВИЛА БЕЗОПАСНОСТИ") { Acronym = "ПБ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("НОРМЫ РАДИАЦИОННОЙ БЕЗОПАСНОСТИ") { Acronym = "НРБ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРАВИЛА РАДИАЦИОННОЙ БЕЗОПАСНОСТИ") { Acronym = "ПРБ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("НОРМЫ ПОЖАРНОЙ БЕЗОПАСНОСТИ") { Acronym = "НПБ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРАВИЛА ПОЖАРНОЙ БЕЗОПАСНОСТИ") { Acronym = "ППБ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТРОИТЕЛЬНЫЕ ПРАВИЛА") { Acronym = "СП", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("МОСКОВСКИЕ ГОРОДСКИЕ СТРОИТЕЛЬНЫЕ НОРМЫ") { Acronym = "МГСН", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("АВОК") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("ABOK", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТЕХНИЧЕСКИЙ РЕГЛАМЕНТ") { Acronym = "ТР", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОБЩИЕ ПРАВИЛА БЕЗОПАСНОСТИ") { Acronym = "ОПБ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРАВИЛА ЯДЕРНОЙ БЕЗОПАСНОСТИ") { Acronym = "ПБЯ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТАНДАРТ ОРГАНИЗАЦИИ") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("СТО", false);
            t.AddVariant("STO", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРАВИЛА ПО ОХРАНЕ ТРУДА") { Acronym = "ПОТ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("ПРАВИЛА ОХРАНЫ ТРУДА", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ИНСТРУКЦИЯ ПО ОХРАНЕ ТРУДА") { Acronym = "ИОТ", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("ИНСТРУКЦИЯ ОХРАНЫ ТРУДА", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("РУКОВОДЯЩИЙ ДОКУМЕНТ") { Acronym = "РД", Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("САНИТАРНЫЕ НОРМЫ И ПРАВИЛА") { Tag = ItemType.Typ, Tag2 = Pullenti.Ner.Decree.DecreeKind.Standard };
            t.AddVariant("САНПИН", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТЕХНИЧЕСКОЕ ЗАДАНИЕ") { Tag = ItemType.Typ, Acronym = "ТЗ" };
            t.AddVariant("ТЕХЗАДАНИЕ", false);
            t.AddAbridge("ТЕХ. ЗАДАНИЕ");
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТЕХНИКО КОММЕРЧЕСКОЕ ПРЕДЛОЖЕНИЕ") { Acronym = "ТКП" };
            t.AddVariant("КОММЕРЧЕСКОЕ ПРЕДЛОЖЕНИЕ", false);
            m_Keywords.Add(t);
        }
        public static Pullenti.Ner.Token IsKeyword(Pullenti.Ner.Token t, bool isMiscTypeOnly = false)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Core.TerminToken tok = m_Keywords.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null) 
            {
                if (isMiscTypeOnly && tok.Termin.Tag != null) 
                    return null;
                tok.EndToken.Tag = tok.Termin.CanonicText;
                return tok.EndToken;
            }
            if (!t.Morph.Class.IsAdjective && !t.Morph.Class.IsPronoun) 
                return null;
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
            if (npt == null || npt.BeginToken == npt.EndToken) 
            {
                if ((t.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ") || t.IsValue("НАЗВАННЫЙ", "НАЗВАНИЙ") || t.IsValue("ДАННЫЙ", "ДАНИЙ")) || ((t.GetMorphClassInDictionary().IsVerb && t.GetMorphClassInDictionary().IsAdjective))) 
                {
                    if ((((tok = m_Keywords.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No)))) != null) 
                    {
                        tok.EndToken.Tag = tok.Termin.CanonicText;
                        return tok.EndToken;
                    }
                }
                return null;
            }
            if ((((tok = m_Keywords.TryParse(npt.EndToken, Pullenti.Ner.Core.TerminParseAttr.No)))) != null) 
            {
                if (isMiscTypeOnly && tok.Termin.Tag != null) 
                    return null;
                tok.EndToken.Tag = tok.Termin.CanonicText;
                return tok.EndToken;
            }
            PartToken pp = PartToken.TryAttach(npt.EndToken, null, false, true);
            if (pp != null) 
                return pp.EndToken;
            return null;
        }
        public static bool IsKeywordStr(string word, bool isMiscTypeOnly = false)
        {
            if (!isMiscTypeOnly) 
            {
                if (m_AllTypesRU.Contains(word) || m_AllTypesUA.Contains(word)) 
                    return true;
            }
            if (m_MiscTypesRU.Contains(word) || m_MiscTypesUA.Contains(word)) 
                return true;
            return false;
        }
        public static void AddNewType(string typ, string acronym = null)
        {
            Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin(typ) { Tag = ItemType.Typ, Acronym = acronym };
            m_Termins.Add(t);
            m_Keywords.Add(new Pullenti.Ner.Core.Termin(typ) { Tag = ItemType.Typ });
        }
        static List<string> m_AllTypesRU = new List<string>(new string[] {"УКАЗ", "УКАЗАНИЕ", "ПОСТАНОВЛЕНИЕ", "РАСПОРЯЖЕНИЕ", "ПРИКАЗ", "ДИРЕКТИВА", "ПИСЬМО", "ЗАПИСКА", "ИНФОРМАЦИОННОЕ ПИСЬМО", "ИНСТРУКЦИЯ", "ЗАКОН", "КОДЕКС", "КОНСТИТУЦИЯ", "РЕШЕНИЕ", "ПОЛОЖЕНИЕ", "РАСПОРЯЖЕНИЕ", "ПОРУЧЕНИЕ", "РЕЗОЛЮЦИЯ", "ДОГОВОР", "СУБДОГОВОР", "АГЕНТСКИЙ ДОГОВОР", "ДОВЕРЕННОСТЬ", "КОММЕРЧЕСКОЕ ПРЕДЛОЖЕНИЕ", "КОНТРАКТ", "ГОСУДАРСТВЕННЫЙ КОНТРАКТ", "ОПРЕДЕЛЕНИЕ", "ПРИГОВОР", "СОГЛАШЕНИЕ", "ПРОТОКОЛ", "ЗАЯВЛЕНИЕ", "УВЕДОМЛЕНИЕ", "РАЗЪЯСНЕНИЕ", "УСТАВ", "ХАРТИЯ", "КОНВЕНЦИЯ", "ПАКТ", "БИЛЛЬ", "ДЕКЛАРАЦИЯ", "РЕГЛАМЕНТ", "ТЕЛЕГРАММА", "ТЕЛЕФОНОГРАММА", "ТЕЛЕФАКСОГРАММА", "ТЕЛЕТАЙПОГРАММА", "ФАКСОГРАММА", "ОТВЕТЫ НА ВОПРОСЫ", "ВЫПИСКА ИЗ ПРОТОКОЛА", "ЗАКЛЮЧЕНИЕ", "ДЕКРЕТ"});
        static List<string> m_AllTypesUA = new List<string>(new string[] {"УКАЗ", "НАКАЗ", "ПОСТАНОВА", "РОЗПОРЯДЖЕННЯ", "НАКАЗ", "ДИРЕКТИВА", "ЛИСТ", "ЗАПИСКА", "ІНФОРМАЦІЙНИЙ ЛИСТ", "ІНСТРУКЦІЯ", "ЗАКОН", "КОДЕКС", "КОНСТИТУЦІЯ", "РІШЕННЯ", "ПОЛОЖЕННЯ", "РОЗПОРЯДЖЕННЯ", "ДОРУЧЕННЯ", "РЕЗОЛЮЦІЯ", "ДОГОВІР", "СУБКОНТРАКТ", "АГЕНТСЬКИЙ ДОГОВІР", "ДОРУЧЕННЯ", "КОМЕРЦІЙНА ПРОПОЗИЦІЯ", "КОНТРАКТ", "ДЕРЖАВНИЙ КОНТРАКТ", "ВИЗНАЧЕННЯ", "ВИРОК", "УГОДА", "ПРОТОКОЛ", "ЗАЯВА", "ПОВІДОМЛЕННЯ", "РОЗ'ЯСНЕННЯ", "СТАТУТ", "ХАРТІЯ", "КОНВЕНЦІЯ", "ПАКТ", "БІЛЛЬ", "ДЕКЛАРАЦІЯ", "РЕГЛАМЕНТ", "ТЕЛЕГРАМА", "ТЕЛЕФОНОГРАМА", "ТЕЛЕФАКСОГРАММА", "ТЕЛЕТАЙПОГРАМА", "ФАКСОГРАМА", "ВІДПОВІДІ НА ЗАПИТАННЯ", "ВИТЯГ З ПРОТОКОЛУ", "ВИСНОВОК", "ДЕКРЕТ"});
        static List<string> m_MiscTypesRU = new List<string>(new string[] {"ПРАВИЛО", "ПРОГРАММА", "ПЕРЕЧЕНЬ", "ПОСОБИЕ", "РЕКОМЕНДАЦИЯ", "НАСТАВЛЕНИЕ", "СТАНДАРТ", "СОГЛАШЕНИЕ", "МЕТОДИКА", "ТРЕБОВАНИЕ", "ПОЛОЖЕНИЕ", "СПИСОК", "ЛИСТ", "ТАБЛИЦА", "ЗАЯВКА", "АКТ", "ФОРМА", "НОРМАТИВ", "РЕЕСТР", "ПОРЯДОК", "ИНФОРМАЦИЯ", "НОМЕНКЛАТУРА", "ОСНОВА", "ОБЗОР", "КОНЦЕПЦИЯ", "СТРАТЕГИЯ", "СТРУКТУРА", "УСЛОВИЕ", "КЛАССИФИКАТОР", "ОБЩЕРОССИЙСКИЙ КЛАССИФИКАТОР", "СПЕЦИФИКАЦИЯ", "ОБРАЗЕЦ"});
        static List<string> m_MiscTypesUA = new List<string>(new string[] {"ПРАВИЛО", "ПРОГРАМА", "ПЕРЕЛІК", "ДОПОМОГА", "РЕКОМЕНДАЦІЯ", "ПОВЧАННЯ", "СТАНДАРТ", "УГОДА", "МЕТОДИКА", "ВИМОГА", "ПОЛОЖЕННЯ", "СПИСОК", "ТАБЛИЦЯ", "ЗАЯВКА", "АКТ", "ФОРМА", "НОРМАТИВ", "РЕЄСТР", "ПОРЯДОК", "ІНФОРМАЦІЯ", "НОМЕНКЛАТУРА", "ОСНОВА", "ОГЛЯД", "КОНЦЕПЦІЯ", "СТРАТЕГІЯ", "СТРУКТУРА", "УМОВА", "КЛАСИФІКАТОР", "ЗАГАЛЬНОРОСІЙСЬКИЙ КЛАСИФІКАТОР", "СПЕЦИФІКАЦІЯ", "ЗРАЗОК"});
        static List<string> m_StdAdjectives = new List<string>(new string[] {"ВСЕОБЩИЙ", "МЕЖДУНАРОДНЫЙ", "ЗАГАЛЬНИЙ", "МІЖНАРОДНИЙ", "НОРМАТИВНЫЙ", "НОРМАТИВНИЙ", "КАССАЦИОННЫЙ", "АПЕЛЛЯЦИОННЫЙ", "КАСАЦІЙНИЙ", "АПЕЛЯЦІЙНИЙ"});
        static List<string> m_EmptyAdjectives = new List<string>(new string[] {"НЫНЕШНИЙ", "ПРЕДЫДУЩИЙ", "ДЕЙСТВУЮЩИЙ", "НАСТОЯЩИЙ", "НИНІШНІЙ", "ПОПЕРЕДНІЙ", "СПРАВЖНІЙ"});
        public static Pullenti.Ner.Decree.DecreeKind GetKind(string typ)
        {
            if (typ == null) 
                return Pullenti.Ner.Decree.DecreeKind.Undefined;
            if (Pullenti.Morph.LanguageHelper.EndsWithEx(typ, "КОНСТИТУЦИЯ", "КОНСТИТУЦІЯ", "КОДЕКС", null)) 
                return Pullenti.Ner.Decree.DecreeKind.Kodex;
            if (typ.StartsWith("ОСНОВ") && Pullenti.Morph.LanguageHelper.EndsWithEx(typ, "ЗАКОНОДАТЕЛЬСТВА", "ЗАКОНОДАВСТВА", null, null)) 
                return Pullenti.Ner.Decree.DecreeKind.Kodex;
            if ((typ == "УСТАВ" || typ == "СТАТУТ" || typ == "ХАРТИЯ") || typ == "ХАРТІЯ" || typ == "РЕГЛАМЕНТ") 
                return Pullenti.Ner.Decree.DecreeKind.Ustav;
            if ((typ.Contains("ДОГОВОР") || typ.Contains("ДОГОВІР") || typ.Contains("КОНТРАКТ")) || typ.Contains("СОГЛАШЕНИЕ") || typ.Contains("ПРОТОКОЛ")) 
                return Pullenti.Ner.Decree.DecreeKind.Contract;
            if (typ.StartsWith("ПРОЕКТ")) 
                return Pullenti.Ner.Decree.DecreeKind.Project;
            if (typ == "ПРОГРАММА" || typ == "ПРОГРАМА") 
                return Pullenti.Ner.Decree.DecreeKind.Program;
            if (((((typ == "ГОСТ" || typ == "ОСТ" || typ == "ISO") || typ == "СНИП" || typ == "RFC") || typ.Contains("НОРМЫ") || typ.Contains("ПРАВИЛА")) || typ.Contains("УСЛОВИЯ") || typ.Contains("СТАНДАРТ")) || typ == "РУКОВОДЯЩИЙ ДОКУМЕНТ" || typ == "АВОК") 
                return Pullenti.Ner.Decree.DecreeKind.Standard;
            if ((Pullenti.Morph.LanguageHelper.EndsWithEx(typ, "КОНВЕНЦИЯ", "КОНВЕНЦІЯ", null, null) || Pullenti.Morph.LanguageHelper.EndsWithEx(typ, "ДОГОВОР", "ДОГОВІР", null, null) || Pullenti.Morph.LanguageHelper.EndsWithEx(typ, "ПАКТ", "БИЛЛЬ", "БІЛЛЬ", null)) || Pullenti.Morph.LanguageHelper.EndsWithEx(typ, "ДЕКЛАРАЦИЯ", "ДЕКЛАРАЦІЯ", null, null)) 
                return Pullenti.Ner.Decree.DecreeKind.Konvention;
            if ((((((typ.StartsWith("СОБРАНИЕ") || typ.StartsWith("ЗБОРИ") || typ.StartsWith("РЕГИСТРАЦИЯ")) || typ.StartsWith("РЕЄСТРАЦІЯ") || typ.Contains("БЮЛЛЕТЕНЬ")) || typ.Contains("БЮЛЕТЕНЬ") || typ.Contains("ВЕДОМОСТИ")) || typ.Contains("ВІДОМОСТІ") || typ.StartsWith("СВОД")) || typ.StartsWith("ЗВЕДЕННЯ") || Pullenti.Morph.LanguageHelper.EndsWithEx(typ, "ГАЗЕТА", "ВЕСТИ", "ВІСТІ", null)) || typ.Contains("ВЕСТНИК") || Pullenti.Morph.LanguageHelper.EndsWith(typ, "ВІСНИК")) 
                return Pullenti.Ner.Decree.DecreeKind.Publisher;
            return Pullenti.Ner.Decree.DecreeKind.Undefined;
        }
        public static bool IsLaw(string typ)
        {
            if (typ == null) 
                return false;
            Pullenti.Ner.Decree.DecreeKind ki = GetKind(typ);
            if (ki == Pullenti.Ner.Decree.DecreeKind.Kodex) 
                return true;
            if (Pullenti.Morph.LanguageHelper.EndsWith(typ, "ЗАКОН")) 
                return true;
            return false;
        }
    }
}