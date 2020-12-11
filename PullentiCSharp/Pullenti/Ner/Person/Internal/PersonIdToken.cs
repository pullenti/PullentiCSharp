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

namespace Pullenti.Ner.Person.Internal
{
    class PersonIdToken : Pullenti.Ner.MetaToken
    {
        public PersonIdToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public enum Typs : int
        {
            Keyword,
            Seria,
            Number,
            Date,
            Org,
            Vidan,
            Code,
            Address,
        }

        public Typs Typ;
        public string Value;
        public Pullenti.Ner.Referent Referent;
        public bool HasPrefix;
        public static Pullenti.Ner.ReferentToken TryAttach(Pullenti.Ner.Token t)
        {
            if (t == null || !t.Chars.IsLetter) 
                return null;
            PersonIdToken noun = TryParse(t, null);
            if (noun == null) 
                return null;
            List<PersonIdToken> li = new List<PersonIdToken>();
            for (t = noun.EndToken.Next; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar) 
                    break;
                if (t.IsCharOf(",:")) 
                    continue;
                PersonIdToken idt = TryParse(t, (li.Count > 0 ? li[li.Count - 1] : noun));
                if (idt == null) 
                {
                    if (t.IsValue("ОТДЕЛ", null) || t.IsValue("ОТДЕЛЕНИЕ", null)) 
                        continue;
                    break;
                }
                if (idt.Typ == Typs.Keyword) 
                    break;
                li.Add(idt);
                t = idt.EndToken;
            }
            if (li.Count == 0) 
                return null;
            string num = null;
            int i = 0;
            if (li[0].Typ == Typs.Number) 
            {
                if (li.Count > 1 && li[1].Typ == Typs.Number && li[1].HasPrefix) 
                {
                    num = li[0].Value + li[1].Value;
                    i = 2;
                }
                else 
                {
                    num = li[0].Value;
                    i = 1;
                }
            }
            else if (li[0].Typ == Typs.Seria && li.Count > 1 && li[1].Typ == Typs.Number) 
            {
                num = li[0].Value + li[1].Value;
                i = 2;
            }
            else if (li[0].Typ == Typs.Seria && li[0].Value.Length > 5) 
            {
                num = li[0].Value;
                i = 1;
            }
            else 
                return null;
            Pullenti.Ner.Person.PersonIdentityReferent pid = new Pullenti.Ner.Person.PersonIdentityReferent();
            pid.Typ = noun.Value.ToLower();
            pid.Number = num;
            if (noun.Referent is Pullenti.Ner.Geo.GeoReferent) 
                pid.State = noun.Referent;
            for (; i < li.Count; i++) 
            {
                if (li[i].Typ == Typs.Vidan || li[i].Typ == Typs.Code) 
                {
                }
                else if (li[i].Typ == Typs.Date && li[i].Referent != null) 
                {
                    if (pid.FindSlot(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_DATE, null, true) != null) 
                        break;
                    pid.AddSlot(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_DATE, li[i].Referent, false, 0);
                }
                else if (li[i].Typ == Typs.Address && li[i].Referent != null) 
                {
                    if (pid.FindSlot(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_ADDRESS, null, true) != null) 
                        break;
                    pid.AddSlot(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_ADDRESS, li[i].Referent, false, 0);
                }
                else if (li[i].Typ == Typs.Org && li[i].Referent != null) 
                {
                    if (pid.FindSlot(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_ORG, null, true) != null) 
                        break;
                    pid.AddSlot(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_ORG, li[i].Referent, false, 0);
                }
                else 
                    break;
            }
            return new Pullenti.Ner.ReferentToken(pid, noun.BeginToken, li[i - 1].EndToken);
        }
        static PersonIdToken TryParse(Pullenti.Ner.Token t, PersonIdToken prev)
        {
            if (t.IsValue("СВИДЕТЕЛЬСТВО", null)) 
            {
                Pullenti.Ner.Token tt1 = t;
                bool ip = false;
                bool reg = false;
                for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsCommaAnd || tt.Morph.Class.IsPreposition) 
                        continue;
                    if (tt.IsValue("РЕГИСТРАЦИЯ", null) || tt.IsValue("РЕЕСТР", null) || tt.IsValue("ЗАРЕГИСТРИРОВАТЬ", null)) 
                    {
                        reg = true;
                        tt1 = tt;
                    }
                    else if (tt.IsValue("ИНДИВИДУАЛЬНЫЙ", null) || tt.IsValue("ИП", null)) 
                    {
                        ip = true;
                        tt1 = tt;
                    }
                    else if ((tt.IsValue("ВНЕСЕНИЕ", null) || tt.IsValue("ГОСУДАРСТВЕННЫЙ", null) || tt.IsValue("ЕДИНЫЙ", null)) || tt.IsValue("ЗАПИСЬ", null) || tt.IsValue("ПРЕДПРИНИМАТЕЛЬ", null)) 
                        tt1 = tt;
                    else if (tt.GetReferent() != null && tt.GetReferent().TypeName == "DATERANGE") 
                        tt1 = tt;
                    else 
                        break;
                }
                if (reg && ip) 
                    return new PersonIdToken(t, tt1) { Typ = Typs.Keyword, Value = "СВИДЕТЕЛЬСТВО О ГОСУДАРСТВЕННОЙ РЕГИСТРАЦИИ ФИЗИЧЕСКОГО ЛИЦА В КАЧЕСТВЕ ИНДИВИДУАЛЬНОГО ПРЕДПРИНИМАТЕЛЯ" };
            }
            Pullenti.Ner.Core.TerminToken tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null) 
            {
                Typs ty = (Typs)tok.Termin.Tag;
                PersonIdToken res = new PersonIdToken(tok.BeginToken, tok.EndToken) { Typ = ty, Value = tok.Termin.CanonicText };
                if (prev == null) 
                {
                    if (ty != Typs.Keyword) 
                        return null;
                    for (t = tok.EndToken.Next; t != null; t = t.Next) 
                    {
                        Pullenti.Ner.Referent r = t.GetReferent();
                        if (r != null && (r is Pullenti.Ner.Geo.GeoReferent)) 
                        {
                            res.Referent = r;
                            res.EndToken = t;
                            continue;
                        }
                        if (t.IsValue("ГРАЖДАНИН", null) && t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                        {
                            res.Referent = t.Next.GetReferent();
                            t = (res.EndToken = t.Next);
                            continue;
                        }
                        if (r != null) 
                            break;
                        PersonAttrToken ait = PersonAttrToken.TryAttach(t, null, PersonAttrToken.PersonAttrAttachAttrs.No);
                        if (ait != null) 
                        {
                            if (ait.Referent != null) 
                            {
                                foreach (Pullenti.Ner.Slot s in ait.Referent.Slots) 
                                {
                                    if (s.TypeName == Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF && (s.Value is Pullenti.Ner.Geo.GeoReferent)) 
                                        res.Referent = s.Value as Pullenti.Ner.Referent;
                                }
                            }
                            res.EndToken = ait.EndToken;
                            break;
                        }
                        if (t.IsValue("ДАННЫЙ", null)) 
                        {
                            res.EndToken = t;
                            continue;
                        }
                        break;
                    }
                    if ((res.Referent is Pullenti.Ner.Geo.GeoReferent) && !(res.Referent as Pullenti.Ner.Geo.GeoReferent).IsState) 
                        res.Referent = null;
                    return res;
                }
                if (ty == Typs.Number) 
                {
                    StringBuilder tmp = new StringBuilder();
                    Pullenti.Ner.Token tt = tok.EndToken.Next;
                    if (tt != null && tt.IsChar(':')) 
                        tt = tt.Next;
                    for (; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsNewlineBefore) 
                            break;
                        if (!(tt is Pullenti.Ner.NumberToken)) 
                            break;
                        tmp.Append(tt.GetSourceText());
                        res.EndToken = tt;
                    }
                    if (tmp.Length < 1) 
                        return null;
                    res.Value = tmp.ToString();
                    res.HasPrefix = true;
                    return res;
                }
                if (ty == Typs.Seria) 
                {
                    StringBuilder tmp = new StringBuilder();
                    Pullenti.Ner.Token tt = tok.EndToken.Next;
                    if (tt != null && tt.IsChar(':')) 
                        tt = tt.Next;
                    bool nextNum = false;
                    for (; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsNewlineBefore) 
                            break;
                        if (Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(tt) != null) 
                        {
                            nextNum = true;
                            break;
                        }
                        if (!(tt is Pullenti.Ner.NumberToken)) 
                        {
                            if (!(tt is Pullenti.Ner.TextToken)) 
                                break;
                            if (!tt.Chars.IsAllUpper) 
                                break;
                            Pullenti.Ner.NumberToken nu = Pullenti.Ner.Core.NumberHelper.TryParseRoman(tt);
                            if (nu != null) 
                            {
                                tmp.Append(nu.GetSourceText());
                                tt = nu.EndToken;
                            }
                            else if (tt.LengthChar != 2) 
                                break;
                            else 
                            {
                                tmp.Append((tt as Pullenti.Ner.TextToken).Term);
                                res.EndToken = tt;
                            }
                            if (tt.Next != null && tt.Next.IsHiphen) 
                                tt = tt.Next;
                            continue;
                        }
                        if (tmp.Length >= 4) 
                            break;
                        tmp.Append(tt.GetSourceText());
                        res.EndToken = tt;
                    }
                    if (tmp.Length < 4) 
                    {
                        if (tmp.Length < 2) 
                            return null;
                        Pullenti.Ner.Token tt1 = res.EndToken.Next;
                        if (tt1 != null && tt1.IsComma) 
                            tt1 = tt1.Next;
                        PersonIdToken next = TryParse(tt1, res);
                        if (next != null && next.Typ == Typs.Number) 
                        {
                        }
                        else 
                            return null;
                    }
                    res.Value = tmp.ToString();
                    res.HasPrefix = true;
                    return res;
                }
                if (ty == Typs.Code) 
                {
                    for (Pullenti.Ner.Token tt = res.EndToken.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsCharOf(":") || tt.IsHiphen) 
                            continue;
                        if (tt is Pullenti.Ner.NumberToken) 
                        {
                            res.EndToken = tt;
                            continue;
                        }
                        break;
                    }
                }
                if (ty == Typs.Address) 
                {
                    if (t.GetReferent() is Pullenti.Ner.Address.AddressReferent) 
                    {
                        res.Referent = t.GetReferent();
                        res.EndToken = t;
                        return res;
                    }
                    for (Pullenti.Ner.Token tt = res.EndToken.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsCharOf(":") || tt.IsHiphen || tt.Morph.Class.IsPreposition) 
                            continue;
                        if (tt.GetReferent() is Pullenti.Ner.Address.AddressReferent) 
                        {
                            res.Referent = tt.GetReferent();
                            res.EndToken = tt;
                        }
                        break;
                    }
                    if (res.Referent == null) 
                        return null;
                }
                return res;
            }
            else if (prev == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t0);
            if (t1 != null) 
                t = t1;
            if (t is Pullenti.Ner.NumberToken) 
            {
                StringBuilder tmp = new StringBuilder();
                PersonIdToken res = new PersonIdToken(t0, t) { Typ = Typs.Number };
                for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
                {
                    if (tt.IsNewlineBefore || !(tt is Pullenti.Ner.NumberToken)) 
                        break;
                    tmp.Append(tt.GetSourceText());
                    res.EndToken = tt;
                }
                if (tmp.Length < 4) 
                {
                    if (tmp.Length < 2) 
                        return null;
                    if (prev == null || prev.Typ != Typs.Keyword) 
                        return null;
                    PersonIdToken ne = TryParse(res.EndToken.Next, prev);
                    if (ne != null && ne.Typ == Typs.Number) 
                        res.Typ = Typs.Seria;
                    else 
                        return null;
                }
                res.Value = tmp.ToString();
                if (t0 != t) 
                    res.HasPrefix = true;
                return res;
            }
            if (t is Pullenti.Ner.ReferentToken) 
            {
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r != null) 
                {
                    if (r.TypeName == "DATE") 
                        return new PersonIdToken(t, t) { Typ = Typs.Date, Referent = r };
                    if (r.TypeName == "ORGANIZATION") 
                        return new PersonIdToken(t, t) { Typ = Typs.Org, Referent = r };
                    if (r.TypeName == "ADDRESS") 
                        return new PersonIdToken(t, t) { Typ = Typs.Address, Referent = r };
                }
            }
            if ((prev != null && prev.Typ == Typs.Keyword && (t is Pullenti.Ner.TextToken)) && !t.Chars.IsAllLower && t.Chars.IsLetter) 
            {
                PersonIdToken rr = TryParse(t.Next, prev);
                if (rr != null && rr.Typ == Typs.Number) 
                    return new PersonIdToken(t, t) { Typ = Typs.Seria, Value = (t as Pullenti.Ner.TextToken).Term };
            }
            if ((t != null && t.IsValue("ОТ", "ВІД") && (t.Next is Pullenti.Ner.ReferentToken)) && t.Next.GetReferent().TypeName == "DATE") 
                return new PersonIdToken(t, t.Next) { Typ = Typs.Date, Referent = t.Next.GetReferent() };
            return null;
        }
        static Pullenti.Ner.Core.TerminCollection m_Ontology;
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("ПАСПОРТ") { Tag = Typs.Keyword };
            t.AddVariant("ПАССПОРТ", false);
            t.AddVariant("ПАСПОРТНЫЕ ДАННЫЕ", false);
            t.AddVariant("ВНУТРЕННИЙ ПАСПОРТ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗАГРАНИЧНЫЙ ПАСПОРТ") { Tag = Typs.Keyword };
            t.AddVariant("ЗАГРАНПАСПОРТ", false);
            t.AddAbridge("ЗАГРАН. ПАСПОРТ");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("УДОСТОВЕРЕНИЕ ЛИЧНОСТИ") { Tag = Typs.Keyword };
            t.AddVariant("УДОСТОВЕРЕНИЕ ЛИЧНОСТИ ОФИЦЕРА", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СВИДЕТЕЛЬСТВО О ГОСУДАРСТВЕННОЙ РЕГИСТРАЦИИ ФИЗИЧЕСКОГО ЛИЦА В КАЧЕСТВЕ ИНДИВИДУАЛЬНОГО ПРЕДПРИНИМАТЕЛЯ") { Tag = Typs.Keyword };
            t.AddVariant("СВИДЕТЕЛЬСТВО О ГОСУДАРСТВЕННОЙ РЕГИСТРАЦИИ ФИЗИЧЕСКОГО ЛИЦА В КАЧЕСТВЕ ИП", false);
            t.AddVariant("СВИДЕТЕЛЬСТВО О ГОСРЕГИСТРАЦИИ ФИЗЛИЦА В КАЧЕСТВЕ ИП", false);
            t.AddVariant("СВИДЕТЕЛЬСТВО ГОСУДАРСТВЕННОЙ РЕГИСТРАЦИИ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВОДИТЕЛЬСКОЕ УДОСТОВЕРЕНИЕ") { Tag = Typs.Keyword };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛИЦЕНЗИЯ") { Tag = Typs.Keyword };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕРИЯ") { Tag = Typs.Seria };
            t.AddAbridge("СЕР.");
            t.AddVariant("СЕРИ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НОМЕР") { Tag = Typs.Number };
            t.AddAbridge("НОМ.");
            t.AddAbridge("Н-Р");
            t.AddVariant("№", false);
            t.AddVariant("N", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЫДАТЬ") { Tag = Typs.Vidan };
            t.AddVariant("ВЫДАВАТЬ", false);
            t.AddVariant("ДАТА ВЫДАЧИ", false);
            t.AddVariant("ДАТА РЕГИСТРАЦИИ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОД ПОДРАЗДЕЛЕНИЯ") { Tag = Typs.Code };
            t.AddAbridge("К/П");
            t.AddAbridge("К.П.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("РЕГИСТРАЦИЯ") { Tag = Typs.Address };
            t.AddVariant("ЗАРЕГИСТРИРОВАН", false);
            t.AddVariant("АДРЕС РЕГИСТРАЦИИ", false);
            t.AddVariant("ЗАРЕГИСТРИРОВАННЫЙ", false);
            t.AddAbridge("ПРОПИСАН");
            t.AddVariant("АДРЕС ПРОПИСКИ", false);
            t.AddVariant("АДРЕС ПО ПРОПИСКЕ", false);
            m_Ontology.Add(t);
        }
    }
}