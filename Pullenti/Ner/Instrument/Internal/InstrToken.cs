/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Instrument.Internal
{
    class InstrToken : Pullenti.Ner.MetaToken
    {
        public InstrToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public ILTypes Typ;
        public string Value;
        public object Ref;
        static Pullenti.Ner.Core.TerminCollection m_Ontology;
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("МЕСТО ПЕЧАТИ");
            t.AddAbridge("М.П.");
            t.AddAbridge("M.П.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МІСЦЕ ПЕЧАТКИ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("М.П.");
            t.AddAbridge("M.П.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОДПИСЬ");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПІДПИС", Pullenti.Morph.MorphLang.UA);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ФАМИЛИЯ ИМЯ ОТЧЕСТВО") { Acronym = "ФИО" };
            t.AddAbridge("Ф.И.О.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРІЗВИЩЕ ІМЯ ПО БАТЬКОВІ", Pullenti.Morph.MorphLang.UA) { Acronym = "ФИО" };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ФАМИЛИЯ");
            t.AddAbridge("ФАМ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРІЗВИЩЕ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("ФАМ.");
            m_Ontology.Add(t);
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ИМЯ"));
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ІМЯ", Pullenti.Morph.MorphLang.UA));
        }
        public bool IsPurePerson
        {
            get
            {
                if (Ref is Pullenti.Ner.ReferentToken) 
                {
                    Pullenti.Ner.ReferentToken rt = Ref as Pullenti.Ner.ReferentToken;
                    if ((rt.Referent is Pullenti.Ner.Person.PersonReferent) || (rt.Referent is Pullenti.Ner.Person.PersonPropertyReferent)) 
                        return true;
                    if (rt.Referent is Pullenti.Ner.Instrument.InstrumentParticipantReferent) 
                    {
                        for (Pullenti.Ner.Token t = rt.BeginToken; t != null && t.EndChar <= rt.EndChar; t = t.Next) 
                        {
                            if ((t.GetReferent() is Pullenti.Ner.Person.PersonReferent) || (t.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent)) 
                                return true;
                            else if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) 
                                break;
                        }
                        return false;
                    }
                }
                return Ref is Pullenti.Ner.Person.PersonReferent;
            }
        }
        public bool IsPodpisStoron
        {
            get
            {
                if (!IsNewlineBefore || !IsNewlineAfter) 
                    return false;
                if (!BeginToken.IsValue("ПОДПИСЬ", "ПІДПИС")) 
                    return false;
                Pullenti.Ner.Token t = BeginToken.Next;
                if (t != null && t.IsValue("СТОРОНА", null)) 
                    t = t.Next;
                if (t != null && t.IsCharOf(":.")) 
                    t = t.Next;
                if (EndToken.Next == t) 
                    return true;
                return false;
            }
        }
        public bool HasVerb;
        public bool NoWords;
        public bool HasTableChars
        {
            get
            {
                for (Pullenti.Ner.Token t = BeginToken; t != null && t.EndChar <= EndChar; t = t.Next) 
                {
                    if (t.IsTableControlChar) 
                        return true;
                }
                if (EndToken.Next != null && EndToken.Next.IsTableControlChar && !EndToken.Next.IsChar((char)0x1E)) 
                    return true;
                if (BeginToken.Previous != null && BeginToken.Previous.IsTableControlChar && !BeginToken.Previous.IsChar((char)0x1F)) 
                    return true;
                return false;
            }
        }
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            if (IsNewlineBefore) 
                tmp.Append("<<");
            tmp.AppendFormat(Typ.ToString());
            if (Value != null) 
                tmp.AppendFormat(" '{0}'", Value);
            if (Ref != null) 
                tmp.AppendFormat(" -> {0}", Ref.ToString());
            if (HasVerb) 
                tmp.AppendFormat(" HasVerb");
            if (NoWords) 
                tmp.Append(" NoWords");
            if (HasTableChars) 
                tmp.Append(" HasTableChars");
            if (IsNewlineAfter) 
                tmp.Append(">>");
            tmp.AppendFormat(": {0}", this.GetSourceText());
            return tmp.ToString();
        }
        public static List<InstrToken> ParseList(Pullenti.Ner.Token t0, int maxChar = 0)
        {
            List<InstrToken> res = new List<InstrToken>();
            for (Pullenti.Ner.Token t = t0; t != null; t = t.Next) 
            {
                if (maxChar > 0) 
                {
                    if (t.BeginChar > maxChar) 
                        break;
                }
                if (res.Count == 272) 
                {
                }
                InstrToken it = InstrToken.Parse(t, maxChar, (res.Count > 0 ? res[res.Count - 1] : null));
                if (it == null) 
                    break;
                if (res.Count == 286) 
                {
                }
                if (it.Typ == ILTypes.Appendix) 
                {
                }
                if (it.Typ == ILTypes.Typ) 
                {
                }
                if (res.Count > 0) 
                {
                    if (res[res.Count - 1].EndChar > it.BeginChar) 
                        break;
                }
                if ((it.EndToken.Next is Pullenti.Ner.TextToken) && it.EndToken.Next.IsChar('.')) 
                    it.EndToken = it.EndToken.Next;
                if (it.Typ == ILTypes.Undefined && t.IsNewlineBefore) 
                {
                    InstrToken1 it1 = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                    if (it1 != null && it1.HasChanges && it1.EndChar > it.EndChar) 
                        it.EndToken = it1.EndToken;
                }
                res.Add(it);
                if (it.EndChar > t.BeginChar) 
                    t = it.EndToken;
            }
            return res;
        }
        static InstrToken _correctPerson(InstrToken res)
        {
            int specChars = 0;
            if (!res.IsPurePerson) 
            {
                res.Typ = ILTypes.Undefined;
                return res;
            }
            for (Pullenti.Ner.Token t = res.EndToken.Next; t != null; t = t.Next) 
            {
                if ((t is Pullenti.Ner.ReferentToken) && (res.Ref is Pullenti.Ner.ReferentToken)) 
                {
                    bool ok = false;
                    if (t.GetReferent() == (res.Ref as Pullenti.Ner.ReferentToken).Referent) 
                        ok = true;
                    Pullenti.Ner.Instrument.InstrumentParticipantReferent ip = (res.Ref as Pullenti.Ner.ReferentToken).Referent as Pullenti.Ner.Instrument.InstrumentParticipantReferent;
                    if (ip != null && ip.ContainsRef(t.GetReferent())) 
                        ok = true;
                    if (!ok && t.Previous != null && t.Previous.IsTableControlChar) 
                    {
                        if (((res.Ref as Pullenti.Ner.ReferentToken).Referent is Pullenti.Ner.Person.PersonPropertyReferent) && (t.GetReferent() is Pullenti.Ner.Person.PersonReferent)) 
                        {
                            ok = true;
                            res.Ref = t;
                        }
                    }
                    if (ok) 
                    {
                        res.EndToken = t;
                        continue;
                    }
                }
                Pullenti.Ner.Core.TerminToken tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                {
                    if ((((tok.Termin.CanonicText == "ПОДПИСЬ" || tok.Termin.CanonicText == "ПІДПИС")) && t.IsNewlineBefore && t.Next != null) && t.Next.IsValue("СТОРОНА", null)) 
                        break;
                    res.EndToken = (t = tok.EndToken);
                    continue;
                }
                if (t.IsChar(',')) 
                    continue;
                if (t.IsTableControlChar && !t.IsNewlineBefore) 
                    continue;
                if (t.IsCharOf("_/\\")) 
                {
                    res.EndToken = t;
                    specChars++;
                    continue;
                }
                if (t.IsChar('(') && t.Next != null) 
                {
                    if ((((tok = m_Ontology.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No)))) != null) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            res.EndToken = (t = br.EndToken);
                            continue;
                        }
                    }
                }
                break;
            }
            Pullenti.Ner.ReferentToken rt0 = res.Ref as Pullenti.Ner.ReferentToken;
            if (rt0 != null && (rt0.Referent is Pullenti.Ner.Instrument.InstrumentParticipantReferent)) 
            {
                for (Pullenti.Ner.Token tt = res.BeginToken; tt != null && tt.EndChar <= res.EndChar; tt = tt.Next) 
                {
                    if ((tt.GetReferent() is Pullenti.Ner.Person.PersonReferent) || (tt.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent)) 
                    {
                        res.Ref = tt;
                        return res;
                    }
                    else if ((tt is Pullenti.Ner.TextToken) && tt.IsCharOf("_/\\")) 
                        specChars++;
                    else if (tt is Pullenti.Ner.MetaToken) 
                    {
                        for (Pullenti.Ner.Token ttt = (tt as Pullenti.Ner.MetaToken).BeginToken; ttt != null && ttt.EndChar <= tt.EndChar; ttt = ttt.Next) 
                        {
                            if ((ttt.GetReferent() is Pullenti.Ner.Person.PersonReferent) || (ttt.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent)) 
                            {
                                res.Ref = ttt;
                                return res;
                            }
                            else if ((ttt is Pullenti.Ner.TextToken) && ttt.IsCharOf("_/\\")) 
                                specChars++;
                        }
                    }
                }
                if (specChars < 10) 
                    res.Typ = ILTypes.Undefined;
            }
            return res;
        }
        public static InstrToken Parse(Pullenti.Ner.Token t, int maxChar = 0, InstrToken prev = null)
        {
            bool isStartOfLine = false;
            Pullenti.Ner.Token t00 = t;
            if (t != null) 
            {
                isStartOfLine = t00.IsNewlineBefore;
                while (t != null) 
                {
                    if (t.IsTableControlChar && !t.IsChar((char)0x1F)) 
                    {
                        if (t.IsNewlineAfter && !isStartOfLine) 
                            isStartOfLine = true;
                        t = t.Next;
                    }
                    else 
                        break;
                }
            }
            if (t == null) 
                return null;
            if (t.IsNewlineBefore) 
                isStartOfLine = true;
            if (isStartOfLine) 
            {
                if ((t.IsValue("СОДЕРЖИМОЕ", "ВМІСТ") || t.IsValue("СОДЕРЖАНИЕ", "ЗМІСТ") || t.IsValue("ОГЛАВЛЕНИЕ", "ЗМІСТ")) || ((t.IsValue("СПИСОК", null) && t.Next != null && t.Next.IsValue("РАЗДЕЛ", null)))) 
                {
                    InstrToken1 cont = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                    if (cont != null && cont.Typ == InstrToken1.Types.Index) 
                        return new InstrToken(t, cont.EndToken);
                }
            }
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = null;
            bool hasWord = false;
            for (; t != null; t = t.Next) 
            {
                if (t.IsNewlineBefore && t != t0) 
                    break;
                if (maxChar > 0 && t.BeginChar > maxChar) 
                    break;
                if (isStartOfLine && t == t0) 
                {
                    if (t.IsValue("ГЛАВА", null)) 
                    {
                        InstrToken next = Parse(t.Next, 0, null);
                        if (next != null && next.Typ == ILTypes.Person) 
                        {
                            next.BeginToken = t;
                            return next;
                        }
                    }
                    Pullenti.Ner.Token tt = null;
                    if ((t.GetReferent() is Pullenti.Ner.Person.PersonReferent) || (t.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent) || (t.GetReferent() is Pullenti.Ner.Instrument.InstrumentParticipantReferent)) 
                        return _correctPerson(new InstrToken(t00, t) { Typ = ILTypes.Person, Ref = t });
                    bool isRef = false;
                    if (t.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent) 
                    {
                        tt = t.Next;
                        isRef = true;
                    }
                    else if (prev != null && prev.Typ == ILTypes.Person) 
                    {
                        Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent(Pullenti.Ner.Person.PersonAnalyzer.ANALYZER_NAME, t);
                        if (rt != null) 
                        {
                            if (rt.Referent is Pullenti.Ner.Person.PersonReferent) 
                                return new InstrToken(t00, rt.EndToken) { Typ = ILTypes.Person };
                            tt = rt.EndToken.Next;
                        }
                    }
                    int cou = 0;
                    Pullenti.Ner.Token t11 = (tt == null ? null : tt.Previous);
                    for (; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsTableControlChar) 
                            continue;
                        Pullenti.Ner.Referent re = tt.GetReferent();
                        if (re is Pullenti.Ner.Person.PersonReferent) 
                            return new InstrToken(t00, tt) { Typ = ILTypes.Person, Ref = tt };
                        if (re is Pullenti.Ner.Geo.GeoReferent) 
                        {
                            t11 = tt;
                            continue;
                        }
                        if (re != null) 
                            break;
                        if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt, false) != null) 
                            break;
                        if (tt.IsNewlineBefore) 
                        {
                            if ((++cou) > 4) 
                                break;
                        }
                    }
                    if (tt == null && isRef) 
                        return new InstrToken(t00, t11 ?? t) { Typ = ILTypes.Person, Ref = t };
                }
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                if (dt != null) 
                {
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && !t.Chars.IsAllLower) 
                    {
                        if (t != t0) 
                            break;
                        bool hasVerb = false;
                        for (Pullenti.Ner.Token tt = dt.EndToken; tt != null; tt = tt.Next) 
                        {
                            if (tt.IsNewlineBefore) 
                                break;
                            else if ((tt is Pullenti.Ner.TextToken) && (tt as Pullenti.Ner.TextToken).IsPureVerb) 
                            {
                                hasVerb = true;
                                break;
                            }
                        }
                        if (!hasVerb) 
                        {
                            InstrToken res2 = new InstrToken(t0, dt.EndToken) { Typ = ILTypes.Typ, Value = dt.FullValue ?? dt.Value };
                            if (res2.Value == "ДОПОЛНИТЕЛЬНОЕ СОГЛАШЕНИЕ" || res2.Value == "ДОДАТКОВА УГОДА") 
                            {
                                if (res2.BeginChar > 500 && res2.NewlinesBeforeCount > 1) 
                                    res2.Typ = ILTypes.Appendix;
                            }
                            return res2;
                        }
                    }
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                    {
                        if (t != t0) 
                            break;
                        return new InstrToken(t0, dt.EndToken) { Typ = ILTypes.RegNumber, Value = dt.Value };
                    }
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                    {
                        if (t != t0) 
                            break;
                        return new InstrToken(t0, dt.EndToken) { Typ = ILTypes.Organization, Ref = dt.Ref, Value = dt.Value };
                    }
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                    {
                        if (t != t0) 
                            break;
                        InstrToken re = new InstrToken(t0, dt.EndToken) { Typ = ILTypes.Geo, Ref = dt.Ref, Value = dt.Value };
                        t1 = re.EndToken.Next;
                        if (t1 != null && t1.IsChar(',')) 
                            t1 = t1.Next;
                        if (t1 != null && t1.IsValue("КРЕМЛЬ", null)) 
                            re.EndToken = t1;
                        else if ((t1 != null && t1.IsValue("ДОМ", "БУДИНОК") && t1.Next != null) && t1.Next.IsValue("СОВЕТ", "РАД")) 
                        {
                            re.EndToken = t1.Next;
                            if (t1.Next.Next != null && (t1.Next.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                                re.EndToken = t1.Next.Next;
                        }
                        return re;
                    }
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner) 
                    {
                        if (t != t0) 
                            break;
                        if (dt.Ref != null && dt.Ref.Referent.ToString().StartsWith("агент")) 
                            dt = null;
                        if (dt != null) 
                        {
                            InstrToken res1 = new InstrToken(t0, dt.EndToken) { Typ = ILTypes.Person, Ref = dt.Ref, Value = dt.Value };
                            return _correctPerson(res1);
                        }
                    }
                }
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        t = (t1 = br.EndToken);
                        continue;
                    }
                    if (t.Next != null && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t.Next, false, null, false)) 
                    {
                        t = (t1 = t.Next);
                        continue;
                    }
                }
                if (t is Pullenti.Ner.TextToken) 
                {
                    if (t.IsChar('_')) 
                    {
                        t1 = t;
                        continue;
                    }
                }
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r is Pullenti.Ner.Date.DateReferent) 
                {
                    Pullenti.Ner.Token tt = t;
                    if (tt.Next != null && tt.Next.IsCharOf(",;")) 
                        tt = tt.Next;
                    if (!t.IsNewlineBefore && !tt.IsNewlineAfter) 
                    {
                        t1 = tt;
                        continue;
                    }
                    if (!hasWord) 
                        return new InstrToken(t, tt) { Typ = ILTypes.Date, Ref = t };
                    if (t != t0) 
                        break;
                }
                hasWord = true;
                if (r is Pullenti.Ner.Instrument.InstrumentParticipantReferent) 
                {
                    for (Pullenti.Ner.Token tt = (t as Pullenti.Ner.ReferentToken).BeginToken; tt != null && (tt.EndChar < t.EndChar); tt = tt.Next) 
                    {
                        Pullenti.Ner.Referent rr = tt.GetReferent();
                        if (rr == null) 
                            continue;
                        if ((rr is Pullenti.Ner.Org.OrganizationReferent) || (rr is Pullenti.Ner.Bank.BankDataReferent) || (rr is Pullenti.Ner.Uri.UriReferent)) 
                        {
                            r = null;
                            break;
                        }
                    }
                }
                if ((r is Pullenti.Ner.Person.PersonReferent) || (r is Pullenti.Ner.Person.PersonPropertyReferent) || (r is Pullenti.Ner.Instrument.InstrumentParticipantReferent)) 
                {
                    if (t != t0) 
                        break;
                    if (r is Pullenti.Ner.Instrument.InstrumentParticipantReferent) 
                    {
                    }
                    InstrToken res1 = new InstrToken(t, t) { Typ = ILTypes.Person, Ref = t };
                    return _correctPerson(res1);
                }
                if (r is Pullenti.Ner.Org.OrganizationReferent) 
                {
                    if (t != t0) 
                        break;
                    return new InstrToken(t, t) { Typ = ILTypes.Organization, Ref = t };
                }
                if (r is Pullenti.Ner.Decree.DecreePartReferent) 
                {
                    Pullenti.Ner.Decree.DecreePartReferent dpr = r as Pullenti.Ner.Decree.DecreePartReferent;
                    if (dpr.Appendix != null) 
                    {
                        if (t.IsNewlineBefore || isStartOfLine) 
                        {
                            if (t.IsNewlineAfter || t.WhitespacesBeforeCount > 30) 
                                return new InstrToken(t, t) { Typ = ILTypes.Appendix, Value = "ПРИЛОЖЕНИЕ" };
                            bool ok = true;
                            for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                            {
                                if (tt.IsNewlineBefore) 
                                    break;
                                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt != null) 
                                {
                                    tt = npt.EndToken;
                                    continue;
                                }
                                ok = false;
                                break;
                            }
                            if (ok) 
                                return new InstrToken(t, t) { Typ = ILTypes.Appendix, Value = "ПРИЛОЖЕНИЕ" };
                        }
                    }
                }
                if ((r is Pullenti.Ner.Decree.DecreeReferent) && (r as Pullenti.Ner.Decree.DecreeReferent).Kind == Pullenti.Ner.Decree.DecreeKind.Publisher && t == t0) 
                {
                    InstrToken res1 = new InstrToken(t, t) { Typ = ILTypes.Approved };
                    for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsCharOf(",;")) 
                            continue;
                        if ((tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && (tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent).Kind == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                            res1.EndToken = t;
                        else 
                            break;
                    }
                    return res1;
                }
                if (t.IsValue("ЗА", null) && t.Next != null && t.IsNewlineBefore) 
                {
                    Pullenti.Ner.Referent rr = t.Next.GetReferent();
                    if ((rr is Pullenti.Ner.Person.PersonReferent) || (rr is Pullenti.Ner.Person.PersonPropertyReferent) || (rr is Pullenti.Ner.Instrument.InstrumentParticipantReferent)) 
                    {
                        if (t != t0) 
                            break;
                        InstrToken res1 = new InstrToken(t, t.Next) { Typ = ILTypes.Person, Ref = t.Next };
                        t = t.Next.Next;
                        if ((rr is Pullenti.Ner.Instrument.InstrumentParticipantReferent) && t != null) 
                        {
                            if ((((r = t.GetReferent()))) != null) 
                            {
                                if ((r is Pullenti.Ner.Person.PersonReferent) || (r is Pullenti.Ner.Person.PersonPropertyReferent)) 
                                {
                                    res1.EndToken = t;
                                    res1.Ref = t;
                                }
                            }
                        }
                        return res1;
                    }
                }
                for (int ii = 0; ii < m_Directives.Count; ii++) 
                {
                    if (t.IsValue(m_Directives[ii], null)) 
                    {
                        if (t.Next != null && t.Next.IsValue("СЛЕДУЮЩЕЕ", "НАСТУПНЕ")) 
                        {
                            if (t != t0) 
                                break;
                            Pullenti.Ner.Token t11 = t.Next;
                            bool ok = false;
                            if (t11.Next != null && t11.Next.IsCharOf(":.") && t11.Next.IsNewlineAfter) 
                            {
                                ok = true;
                                t11 = t11.Next;
                            }
                            if (ok) 
                                return new InstrToken(t, t11) { Typ = ILTypes.Directive, Value = m_DirectivesNorm[ii] };
                        }
                        if (t.IsNewlineAfter || ((t.Next != null && t.Next.IsChar(':') && t.Next.IsNewlineAfter))) 
                        {
                            if (t != t0) 
                                break;
                            if (!t.IsNewlineBefore) 
                            {
                                if ((m_DirectivesNorm[ii] != "ПРИКАЗ" && m_DirectivesNorm[ii] != "ПОСТАНОВЛЕНИЕ" && m_DirectivesNorm[ii] != "НАКАЗ") && m_DirectivesNorm[ii] != "ПОСТАНОВУ") 
                                    break;
                            }
                            return new InstrToken(t, (t.IsNewlineAfter ? t : t.Next)) { Typ = ILTypes.Directive, Value = m_DirectivesNorm[ii] };
                        }
                        break;
                    }
                }
                if (t.IsNewlineBefore && t.Chars.IsLetter && t.LengthChar == 1) 
                {
                    foreach (string d in m_Directives) 
                    {
                        Pullenti.Ner.Token t11 = Pullenti.Ner.Core.MiscHelper.TryAttachWordByLetters(d, t, true);
                        if (t11 != null) 
                        {
                            if (t11.Next != null && t11.Next.IsChar(':')) 
                                t11 = t11.Next;
                            return new InstrToken(t, t11) { Typ = ILTypes.Directive };
                        }
                    }
                }
                Pullenti.Ner.Token tte = (t is Pullenti.Ner.MetaToken ? (t as Pullenti.Ner.MetaToken).BeginToken : t);
                string term = (tte is Pullenti.Ner.TextToken ? (tte as Pullenti.Ner.TextToken).Term : null);
                if (isStartOfLine && !tte.Chars.IsAllLower && t == t0) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tte, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && ((term == "ПРИЛОЖЕНИЯ" || term == "ДОДАТКИ"))) 
                        // if (tte.Next != null && tte.Next.IsChar(':'))
                        npt = null;
                    if (npt != null && npt.Morph.Case.IsNominative && (npt.EndToken is Pullenti.Ner.TextToken)) 
                    {
                        string term1 = (npt.EndToken as Pullenti.Ner.TextToken).Term;
                        if (((term1 == "ПРИЛОЖЕНИЕ" || term1 == "ДОДАТОК" || term1 == "МНЕНИЕ") || term1 == "ДУМКА" || term1 == "АКТ") || term1 == "ФОРМА" || term == "ЗАЯВКА") 
                        {
                            Pullenti.Ner.Token tt1 = npt.EndToken.Next;
                            Pullenti.Ner.Decree.Internal.DecreeToken dt1 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt1, null, false);
                            if (dt1 != null && dt1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                                tt1 = dt1.EndToken.Next;
                            else if (tt1 is Pullenti.Ner.NumberToken) 
                                tt1 = tt1.Next;
                            else if ((tt1 is Pullenti.Ner.TextToken) && tt1.LengthChar == 1 && tt1.Chars.IsLetter) 
                                tt1 = tt1.Next;
                            bool ok = true;
                            if (tt1 == null) 
                                ok = false;
                            else if (tt1.IsValue("В", "У")) 
                                ok = false;
                            else if (tt1.IsValue("К", null) && tt1.IsNewlineBefore) 
                                return new InstrToken(t, t) { Typ = ILTypes.Appendix, Value = term1 };
                            else if (!tt1.IsNewlineBefore && _checkEntered(tt1) != null) 
                                ok = false;
                            else if (tt1 == t.Next && ((tt1.IsChar(':') || ((tt1.IsValue("НА", null) && term1 != "ЗАЯВКА"))))) 
                                ok = false;
                            if (ok) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null) 
                                {
                                    tt1 = br.EndToken.Next;
                                    if (br.EndToken.Next == null || !br.EndToken.IsNewlineAfter || br.EndToken.Next.IsCharOf(";,")) 
                                        ok = false;
                                    if (tt1 != null && tt1.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
                                        ok = false;
                                }
                            }
                            if (prev != null && prev.Typ == ILTypes.Appendix) 
                                ok = false;
                            if (ok) 
                            {
                                int cou = 0;
                                for (Pullenti.Ner.Token ttt = tte.Previous; ttt != null && (cou < 300); ttt = ttt.Previous,cou++) 
                                {
                                    if (ttt.IsTableControlChar) 
                                    {
                                        if (!ttt.IsChar((char)0x1F)) 
                                        {
                                            if (ttt == tte.Previous && ttt.IsChar((char)0x1E)) 
                                            {
                                            }
                                            else 
                                                ok = false;
                                        }
                                        break;
                                    }
                                }
                            }
                            if (ok) 
                            {
                                InstrToken1 it1 = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                                if (it1 != null) 
                                {
                                    if (it1.HasVerb) 
                                        ok = false;
                                }
                            }
                            if (ok && t.Previous != null) 
                            {
                                for (Pullenti.Ner.Token ttp = t.Previous; ttp != null; ttp = ttp.Previous) 
                                {
                                    if (ttp.IsTableControlChar && !ttp.IsChar((char)0x1F)) 
                                        continue;
                                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(ttp, false) && !Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(ttp, false, null, false)) 
                                        continue;
                                    if (ttp.IsCharOf(";:")) 
                                        ok = false;
                                    break;
                                }
                            }
                            if ((ok && t.Previous != null && (t.NewlinesBeforeCount < 3)) && !t.IsNewlineAfter) 
                            {
                                int lines = 0;
                                for (Pullenti.Ner.Token ttp = t.Previous; ttp != null; ttp = ttp.Previous) 
                                {
                                    if (!ttp.IsNewlineBefore) 
                                        continue;
                                    for (; ttp != null && (ttp.EndChar < t.BeginChar); ttp = ttp.Next) 
                                    {
                                        if (ttp is Pullenti.Ner.NumberToken) 
                                        {
                                        }
                                        else if ((ttp is Pullenti.Ner.TextToken) && ttp.LengthChar > 1) 
                                        {
                                            if (ttp.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
                                                ok = false;
                                            break;
                                        }
                                        else 
                                            break;
                                    }
                                    if ((++lines) > 1) 
                                        break;
                                }
                            }
                            if (ok && ((term1 != "ПРИЛОЖЕНИЕ" && term1 != "ДОДАТОК" && term1 != "МНЕНИЕ"))) 
                            {
                                if (t.NewlinesBeforeCount < 3) 
                                    ok = false;
                            }
                            if (ok) 
                                return new InstrToken(t, t) { Typ = ILTypes.Appendix, Value = term1 };
                        }
                    }
                }
                bool app = false;
                if ((((term == "ОСОБОЕ" || term == "ОСОБЛИВЕ")) && t.Next != null && t.Next.IsValue("МНЕНИЕ", "ДУМКА")) && t == t0 && isStartOfLine) 
                    app = true;
                if ((((term == "ДОПОЛНИТЕЛЬНОЕ" || term == "ДОДАТКОВА")) && t.Next != null && t.Next.IsValue("СОГЛАШЕНИЕ", "УГОДА")) && t == t0 && isStartOfLine) 
                    app = true;
                if (app) 
                {
                    for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsNewlineBefore) 
                            break;
                        else if (tt.GetMorphClassInDictionary() == Pullenti.Morph.MorphClass.Verb) 
                        {
                            app = false;
                            break;
                        }
                    }
                    if (app) 
                        return new InstrToken(t, t.Next) { Typ = ILTypes.Appendix };
                }
                if (!t.Chars.IsAllLower && t == t0) 
                {
                    Pullenti.Ner.Token tt = _checkApproved(t);
                    if (tt != null) 
                    {
                        if (tt.Next != null && (tt.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                            return new InstrToken(t, tt) { Typ = ILTypes.Approved, Ref = tt.Next.GetReferent() };
                        Pullenti.Ner.Decree.Internal.DecreeToken dt1 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt.Next, null, false);
                        if (dt1 != null && dt1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                            return new InstrToken(t, tt) { Typ = ILTypes.Approved };
                    }
                }
                t1 = t;
                isStartOfLine = false;
            }
            if (t1 == null) 
                return null;
            InstrToken res = new InstrToken(t00, t1) { Typ = ILTypes.Undefined };
            res.NoWords = true;
            for (t = t0; t != null && t.EndChar <= t1.EndChar; t = t.Next) 
            {
                if (!(t is Pullenti.Ner.TextToken)) 
                {
                    if (t is Pullenti.Ner.ReferentToken) 
                        res.NoWords = false;
                    continue;
                }
                if (!t.Chars.IsLetter) 
                    continue;
                res.NoWords = false;
                if ((t as Pullenti.Ner.TextToken).IsPureVerb) 
                    res.HasVerb = true;
            }
            if (t0.IsValue("ВОПРОС", "ПИТАННЯ") && t0.Next != null && t0.Next.IsCharOf(":.")) 
                res.Typ = ILTypes.Question;
            return res;
        }
        internal static Pullenti.Ner.Token _checkApproved(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if (((!t.IsValue("УТВЕРЖДЕН", "ЗАТВЕРДЖЕНИЙ") && !t.IsValue("УТВЕРЖДАТЬ", "СТВЕРДЖУВАТИ") && !t.IsValue("УТВЕРДИТЬ", "ЗАТВЕРДИТИ")) && !t.IsValue("ВВЕСТИ", null) && !t.IsValue("СОГЛАСОВАНО", "ПОГОДЖЕНО")) && !t.IsValue("СОГЛАСОВАТЬ", "ПОГОДИТИ")) 
                return null;
            if (t.Morph.ContainsAttr("инф.", null) && t.Morph.ContainsAttr("сов.в.", null)) 
                return null;
            if (t.Morph.ContainsAttr("возвр.", null)) 
                return null;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = t;
            for (t = t.Next; t != null; t = t.Next) 
            {
                if (t.Morph.Class.IsPreposition || t.Morph.Class.IsConjunction) 
                    continue;
                if (t.IsChar(':')) 
                    continue;
                if (t.IsValue("ДЕЙСТВИЕ", "ДІЯ") || t.IsValue("ВВЕСТИ", null) || t.IsValue("ВВОДИТЬ", "ВВОДИТИ")) 
                {
                    t1 = t;
                    continue;
                }
                Pullenti.Ner.Token tt = _checkApproved(t);
                if (tt != null) 
                {
                    if (!tt.IsNewlineBefore && tt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) != t0.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false)) 
                    {
                        t1 = (tt = t);
                        continue;
                    }
                }
                break;
            }
            return t1;
        }
        internal static Pullenti.Ner.Token _checkEntered(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if ((((t.IsValue("ВСТУПАТЬ", "ВСТУПАТИ") || t.IsValue("ВСТУПИТЬ", "ВСТУПИТИ"))) && t.Next != null && t.Next.IsValue("В", "У")) && t.Next.Next != null && t.Next.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
                return t.Next.Next;
            if (t.IsValue("УТРАТИТЬ", "ВТРАТИТИ") && t.Next != null && t.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
                return t.Next;
            if (t.IsValue("ДЕЙСТВОВАТЬ", "ДІЯТИ") && t.Next != null && t.Next.IsValue("ДО", null)) 
                return t.Next;
            if (((t.IsValue("В", null) || t.IsValue("B", null))) && t.Next != null) 
            {
                if (t.Next.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                    return t.Next;
                if (t.Next.IsValue("РЕД", null)) 
                {
                    if (t.Next.Next != null && t.Next.Next.IsChar('.')) 
                        return t.Next.Next;
                    return t.Next;
                }
            }
            if (t.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                return t.Next;
            if (t.IsValue("РЕД", null)) 
            {
                if (t.Next != null && t.Next.IsChar('.')) 
                    return t.Next;
                return t;
            }
            return _checkApproved(t);
        }
        internal static List<string> m_Directives = new List<string>(new string[] {"ПРИКАЗЫВАТЬ", "ПРИКАЗАТЬ", "ОПРЕДЕЛЯТЬ", "ОПРЕДЕЛЯТЬ", "ОПРЕДЕЛИТЬ", "ПОСТАНОВЛЯТЬ", "ПОСТАНОВИТЬ", "УСТАНОВИТЬ", "РЕШИЛ", "РЕШИТЬ", "ПРОСИТЬ", "ПРИГОВАРИВАТЬ", "ПРИГОВОРИТЬ", "НАКАЗУВАТИ", "ВИЗНАЧАТИ", "ВИЗНАЧИТИ", "УХВАЛЮВАТИ", "УХВАЛИТИ", "ПОСТАНОВЛЯТИ", "ПОСТАНОВИТИ", "ВСТАНОВИТИ", "ВИРІШИВ", "ВИРІШИТИ", "ПРОСИТИ", "ПРИМОВЛЯТИ", "ЗАСУДИТИ"});
        internal static List<string> m_DirectivesNorm = new List<string>(new string[] {"ПРИКАЗ", "ПРИКАЗ", "ОПРЕДЕЛЕНИЕ", "ОПРЕДЕЛЕНИЕ", "ОПРЕДЕЛЕНИЕ", "ПОСТАНОВЛЕНИЕ", "ПОСТАНОВЛЕНИЕ", "УСТАНОВЛЕНИЕ", "РЕШЕНИЕ", "РЕШЕНИЕ", "ЗАЯВЛЕНИЕ", "ПРИГОВОР", "ПРИГОВОР", "НАКАЗ", "УХВАЛА", "УХВАЛА", "УХВАЛА", "УХВАЛА", "ПОСТАНОВА", "ПОСТАНОВА", "ВСТАНОВЛЕННЯ", "РІШЕННЯ", "РІШЕННЯ", "ЗАЯВА", "ВИРОК", "ВИРОК"});
    }
}