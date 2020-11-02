/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Person.Internal
{
    public static class PersonHelper
    {
        internal static Pullenti.Ner.ReferentToken CreateReferentToken(Pullenti.Ner.Person.PersonReferent p, Pullenti.Ner.Token begin, Pullenti.Ner.Token end, Pullenti.Ner.MorphCollection morph, List<PersonAttrToken> attrs, Pullenti.Ner.Person.PersonAnalyzer.PersonAnalyzerData ad, bool forAttribute, bool afterBePredicate)
        {
            if (p == null) 
                return null;
            bool hasPrefix = false;
            if (attrs != null) 
            {
                foreach (PersonAttrToken a in attrs) 
                {
                    if (a.Typ == PersonAttrTerminType.BestRegards) 
                        hasPrefix = true;
                    else 
                    {
                        if (a.BeginChar < begin.BeginChar) 
                        {
                            begin = a.BeginToken;
                            if ((a.EndToken.Next != null && a.EndToken.Next.IsChar(')') && begin.Previous != null) && begin.Previous.IsChar('(')) 
                                begin = begin.Previous;
                        }
                        if (a.Typ != PersonAttrTerminType.Prefix) 
                        {
                            if (a.Age != null) 
                                p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_AGE, a.Age, false, 0);
                            if (a.PropRef == null) 
                                p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_ATTR, a.Value, false, 0);
                            else 
                                p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_ATTR, a, false, 0);
                        }
                        else if (a.Gender == Pullenti.Morph.MorphGender.Feminie && !p.IsFemale) 
                            p.IsFemale = true;
                        else if (a.Gender == Pullenti.Morph.MorphGender.Masculine && !p.IsMale) 
                            p.IsMale = true;
                    }
                }
            }
            else if ((begin.Previous is Pullenti.Ner.TextToken) && (begin.WhitespacesBeforeCount < 3)) 
            {
                if ((begin.Previous as Pullenti.Ner.TextToken).Term == "ИП") 
                {
                    PersonAttrToken a = new PersonAttrToken(begin.Previous, begin.Previous);
                    a.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent();
                    a.PropRef.Name = "индивидуальный предприниматель";
                    p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_ATTR, a, false, 0);
                    begin = begin.Previous;
                }
            }
            Pullenti.Ner.MorphCollection m0 = new Pullenti.Ner.MorphCollection();
            foreach (Pullenti.Morph.MorphBaseInfo it in morph.Items) 
            {
                Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo();
                bi.CopyFrom(it);
                bi.Number = Pullenti.Morph.MorphNumber.Singular;
                if (bi.Gender == Pullenti.Morph.MorphGender.Undefined) 
                {
                    if (p.IsMale && !p.IsFemale) 
                        bi.Gender = Pullenti.Morph.MorphGender.Masculine;
                    if (!p.IsMale && p.IsFemale) 
                        bi.Gender = Pullenti.Morph.MorphGender.Feminie;
                }
                m0.AddItem(bi);
            }
            morph = m0;
            if ((attrs != null && attrs.Count > 0 && !attrs[0].Morph.Case.IsUndefined) && morph.Case.IsUndefined) 
            {
                morph.Case = attrs[0].Morph.Case;
                if (attrs[0].Morph.Number == Pullenti.Morph.MorphNumber.Singular) 
                    morph.Number = Pullenti.Morph.MorphNumber.Singular;
                if (p.IsMale && !p.IsFemale) 
                    morph.Gender = Pullenti.Morph.MorphGender.Masculine;
                else if (p.IsFemale) 
                    morph.Gender = Pullenti.Morph.MorphGender.Feminie;
            }
            if (begin.Previous != null) 
            {
                Pullenti.Ner.Token ttt = begin.Previous;
                if (ttt.IsValue("ИМЕНИ", "ІМЕНІ")) 
                    forAttribute = true;
                else 
                {
                    if (ttt.IsChar('.') && ttt.Previous != null) 
                        ttt = ttt.Previous;
                    if (ttt.WhitespacesAfterCount < 3) 
                    {
                        if (ttt.IsValue("ИМ", "ІМ")) 
                            forAttribute = true;
                    }
                }
            }
            if (forAttribute) 
                return new Pullenti.Ner.ReferentToken(p, begin, end) { Morph = morph, MiscAttrs = (int)p.m_PersonIdentityTyp };
            if ((begin.Previous != null && begin.Previous.IsCommaAnd && (begin.Previous.Previous is Pullenti.Ner.ReferentToken)) && (begin.Previous.Previous.GetReferent() is Pullenti.Ner.Person.PersonReferent)) 
            {
                Pullenti.Ner.ReferentToken rt00 = begin.Previous.Previous as Pullenti.Ner.ReferentToken;
                for (Pullenti.Ner.Token ttt = (Pullenti.Ner.Token)rt00; ttt != null; ) 
                {
                    if (ttt.Previous == null || !(ttt.Previous.Previous is Pullenti.Ner.ReferentToken)) 
                        break;
                    if (!ttt.Previous.IsCommaAnd || !(ttt.Previous.Previous.GetReferent() is Pullenti.Ner.Person.PersonReferent)) 
                        break;
                    rt00 = ttt.Previous.Previous as Pullenti.Ner.ReferentToken;
                    ttt = rt00;
                }
                if (rt00.BeginToken.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent) 
                {
                    bool ok = false;
                    if ((rt00.BeginToken as Pullenti.Ner.ReferentToken).EndToken.Next != null && (rt00.BeginToken as Pullenti.Ner.ReferentToken).EndToken.Next.IsChar(':')) 
                        ok = true;
                    else if (rt00.BeginToken.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                        ok = true;
                    if (ok) 
                        p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_ATTR, rt00.BeginToken.GetReferent(), false, 0);
                }
            }
            if (ad != null) 
            {
                if (ad.OverflowLevel > 10) 
                    return new Pullenti.Ner.ReferentToken(p, begin, end) { Morph = morph, MiscAttrs = (int)p.m_PersonIdentityTyp };
                ad.OverflowLevel++;
            }
            List<PersonAttrToken> attrs1 = null;
            bool hasPosition = false;
            bool openBr = false;
            for (Pullenti.Ner.Token t = end.Next; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar) 
                    break;
                if (t.IsNewlineBefore) 
                {
                    if (t.NewlinesBeforeCount > 2) 
                        break;
                    if (attrs1 != null && attrs1.Count > 0) 
                        break;
                    Pullenti.Ner.Mail.Internal.MailLine ml = Pullenti.Ner.Mail.Internal.MailLine.Parse(t, 0, 0);
                    if (ml != null && ml.Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From) 
                        break;
                    if (t.Chars.IsCapitalUpper) 
                    {
                        PersonAttrToken attr1 = PersonAttrToken.TryAttach(t, (ad == null ? null : ad.LocalOntology), PersonAttrToken.PersonAttrAttachAttrs.No);
                        bool ok1 = false;
                        if (attr1 != null) 
                        {
                            if (hasPrefix || attr1.IsNewlineAfter || ((attr1.EndToken.Next != null && attr1.EndToken.Next.IsTableControlChar))) 
                                ok1 = true;
                            else 
                                for (Pullenti.Ner.Token tt2 = t.Next; tt2 != null && tt2.EndChar <= attr1.EndChar; tt2 = tt2.Next) 
                                {
                                    if (tt2.IsWhitespaceBefore) 
                                        ok1 = true;
                                }
                        }
                        else 
                        {
                            Pullenti.Ner.Token ttt = CorrectTailAttributes(p, t);
                            if (ttt != null && ttt != t) 
                            {
                                end = (t = ttt);
                                continue;
                            }
                        }
                        if (!ok1) 
                            break;
                    }
                }
                if (t.IsHiphen || t.IsCharOf("_>|")) 
                    continue;
                if (t.IsValue("МОДЕЛЬ", null)) 
                    break;
                Pullenti.Ner.Token tt = CorrectTailAttributes(p, t);
                if (tt != t && tt != null) 
                {
                    end = (t = tt);
                    continue;
                }
                bool isBe = false;
                if (t.IsChar('(') && t == end.Next) 
                {
                    openBr = true;
                    t = t.Next;
                    if (t == null) 
                        break;
                    PersonItemToken pit1 = PersonItemToken.TryAttach(t, null, PersonItemToken.ParseAttr.No, null);
                    if ((pit1 != null && t.Chars.IsCapitalUpper && pit1.EndToken.Next != null) && (t is Pullenti.Ner.TextToken) && pit1.EndToken.Next.IsChar(')')) 
                    {
                        if (pit1.Lastname != null) 
                        {
                            Pullenti.Morph.MorphBaseInfo inf = new Pullenti.Morph.MorphBaseInfo() { Case = Pullenti.Morph.MorphCase.Nominative };
                            if (p.IsMale) 
                                inf.Gender |= Pullenti.Morph.MorphGender.Masculine;
                            if (p.IsFemale) 
                                inf.Gender |= Pullenti.Morph.MorphGender.Feminie;
                            PersonMorphCollection sur = PersonIdentityToken.CreateLastname(pit1, inf);
                            if (sur != null) 
                            {
                                p.AddFioIdentity(sur, null, null);
                                end = (t = pit1.EndToken.Next);
                                continue;
                            }
                        }
                    }
                    if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLatinLetter) 
                    {
                        List<PersonItemToken> pits = PersonItemToken.TryAttachList(t, null, PersonItemToken.ParseAttr.CanBeLatin, 10);
                        if (((pits != null && pits.Count >= 2 && pits.Count <= 3) && pits[0].Chars.IsLatinLetter && pits[1].Chars.IsLatinLetter) && pits[pits.Count - 1].EndToken.Next != null && pits[pits.Count - 1].EndToken.Next.IsChar(')')) 
                        {
                            Pullenti.Ner.Person.PersonReferent pr2 = new Pullenti.Ner.Person.PersonReferent();
                            int cou = 0;
                            foreach (PersonItemToken pi in pits) 
                            {
                                foreach (Pullenti.Ner.Slot si in p.Slots) 
                                {
                                    if (si.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME || si.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME || si.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME) 
                                    {
                                        if (Pullenti.Ner.Core.MiscHelper.CanBeEqualCyrAndLatSS(si.Value.ToString(), pi.Value)) 
                                        {
                                            cou++;
                                            pr2.AddSlot(si.TypeName, pi.Value, false, 0);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (cou == pits.Count) 
                            {
                                foreach (Pullenti.Ner.Slot si in pr2.Slots) 
                                {
                                    p.AddSlot(si.TypeName, si.Value, false, 0);
                                }
                                end = (t = pits[pits.Count - 1].EndToken.Next);
                                continue;
                            }
                        }
                    }
                }
                else if (t.IsComma) 
                {
                    t = t.Next;
                    if ((t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).IsValue("WHO", null)) 
                        continue;
                    if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLatinLetter) 
                    {
                        List<PersonItemToken> pits = PersonItemToken.TryAttachList(t, null, PersonItemToken.ParseAttr.CanBeLatin, 10);
                        if ((pits != null && pits.Count >= 2 && pits.Count <= 3) && pits[0].Chars.IsLatinLetter && pits[1].Chars.IsLatinLetter) 
                        {
                            Pullenti.Ner.Person.PersonReferent pr2 = new Pullenti.Ner.Person.PersonReferent();
                            int cou = 0;
                            foreach (PersonItemToken pi in pits) 
                            {
                                foreach (Pullenti.Ner.Slot si in p.Slots) 
                                {
                                    if (si.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME || si.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME || si.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME) 
                                    {
                                        if (Pullenti.Ner.Core.MiscHelper.CanBeEqualCyrAndLatSS(si.Value.ToString(), pi.Value)) 
                                        {
                                            cou++;
                                            pr2.AddSlot(si.TypeName, pi.Value, false, 0);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (cou == pits.Count) 
                            {
                                foreach (Pullenti.Ner.Slot si in pr2.Slots) 
                                {
                                    p.AddSlot(si.TypeName, si.Value, false, 0);
                                }
                                end = (t = pits[pits.Count - 1].EndToken);
                                continue;
                            }
                        }
                    }
                }
                else if ((t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).IsVerbBe) 
                    t = t.Next;
                else if (t.IsAnd && t.IsWhitespaceAfter && !t.IsNewlineAfter) 
                {
                    if (t == end.Next) 
                        break;
                    t = t.Next;
                }
                else if (t.IsHiphen && t == end.Next) 
                    t = t.Next;
                else if (t.IsChar('.') && t == end.Next && hasPrefix) 
                    t = t.Next;
                Pullenti.Ner.Token ttt2 = CreateNickname(p, t);
                if (ttt2 != null) 
                {
                    t = (end = ttt2);
                    continue;
                }
                if (t == null) 
                    break;
                PersonAttrToken attr = null;
                attr = PersonAttrToken.TryAttach(t, (ad == null ? null : ad.LocalOntology), PersonAttrToken.PersonAttrAttachAttrs.No);
                if (attr == null) 
                {
                    if ((t != null && t.GetReferent() != null && t.GetReferent().TypeName == "GEO") && attrs1 != null && openBr) 
                        continue;
                    if ((t.Chars.IsCapitalUpper && openBr && t.Next != null) && t.Next.IsChar(')')) 
                    {
                        if (p.FindSlot(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, null, true) == null) 
                        {
                            p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, t.GetSourceText().ToUpper(), false, 0);
                            t = t.Next;
                            end = t;
                        }
                    }
                    if (t != null && t.IsValue("КОТОРЫЙ", null) && t.Morph.Number == Pullenti.Morph.MorphNumber.Singular) 
                    {
                        if (!p.IsFemale && t.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        {
                            p.IsFemale = true;
                            p.CorrectData();
                        }
                        else if (!p.IsMale && t.Morph.Gender == Pullenti.Morph.MorphGender.Masculine) 
                        {
                            p.IsMale = true;
                            p.CorrectData();
                        }
                    }
                    break;
                }
                if (attr.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                    break;
                if (attr.Typ == PersonAttrTerminType.BestRegards) 
                    break;
                if (attr.IsDoubt) 
                {
                    if (hasPrefix) 
                    {
                    }
                    else if (t.IsNewlineBefore && attr.IsNewlineAfter) 
                    {
                    }
                    else if (t.Previous != null && ((t.Previous.IsHiphen || t.Previous.IsChar(':')))) 
                    {
                    }
                    else 
                        break;
                }
                if (!morph.Case.IsUndefined && !attr.Morph.Case.IsUndefined) 
                {
                    if (((morph.Case & attr.Morph.Case)).IsUndefined && !isBe) 
                        break;
                }
                if (openBr) 
                {
                    if (Pullenti.Ner.Person.PersonAnalyzer.TryAttachPerson(t, ad, false, 0, true) != null) 
                        break;
                }
                if (attrs1 == null) 
                {
                    if (t.Previous.IsComma && t.Previous == end.Next) 
                    {
                        Pullenti.Ner.Token ttt = attr.EndToken.Next;
                        if (ttt != null) 
                        {
                            if (ttt.Morph.Class.IsVerb) 
                            {
                                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(begin)) 
                                {
                                }
                                else 
                                    break;
                            }
                        }
                    }
                    attrs1 = new List<PersonAttrToken>();
                }
                attrs1.Add(attr);
                if (attr.Typ == PersonAttrTerminType.Position || attr.Typ == PersonAttrTerminType.King) 
                {
                    if (!isBe) 
                        hasPosition = true;
                }
                else if (attr.Typ != PersonAttrTerminType.Prefix) 
                {
                    if (attr.Typ == PersonAttrTerminType.Other && attr.Age != null) 
                    {
                    }
                    else 
                    {
                        attrs1 = null;
                        break;
                    }
                }
                t = attr.EndToken;
            }
            if (attrs1 != null && hasPosition && attrs != null) 
            {
                Pullenti.Ner.Token te1 = attrs[attrs.Count - 1].EndToken.Next;
                Pullenti.Ner.Token te2 = attrs1[0].BeginToken;
                if (te1.WhitespacesAfterCount > te2.WhitespacesBeforeCount && (te2.WhitespacesBeforeCount < 2)) 
                {
                }
                else if (attrs1[0].Age != null) 
                {
                }
                else if (((te1.IsHiphen || te1.IsChar(':'))) && !attrs1[0].IsNewlineBefore && ((te2.Previous.IsComma || te2.Previous == end))) 
                {
                }
                else 
                    foreach (PersonAttrToken a in attrs) 
                    {
                        if (a.Typ == PersonAttrTerminType.Position) 
                        {
                            Pullenti.Ner.Token te = attrs1[attrs1.Count - 1].EndToken;
                            if (te.Next != null) 
                            {
                                if (!te.Next.IsChar('.')) 
                                {
                                    attrs1 = null;
                                    break;
                                }
                            }
                        }
                    }
            }
            if (attrs1 != null && !hasPrefix) 
            {
                PersonAttrToken attr = attrs1[attrs1.Count - 1];
                bool ok = false;
                if (attr.EndToken.Next != null && attr.EndToken.Next.Chars.IsCapitalUpper) 
                    ok = true;
                else 
                {
                    Pullenti.Ner.ReferentToken rt = Pullenti.Ner.Person.PersonAnalyzer.TryAttachPerson(attr.BeginToken, ad, false, -1, false);
                    if (rt != null && (rt.Referent is Pullenti.Ner.Person.PersonReferent)) 
                        ok = true;
                }
                if (ok) 
                {
                    if (attr.BeginToken.WhitespacesBeforeCount > attr.EndToken.WhitespacesAfterCount) 
                        attrs1 = null;
                    else if (attr.BeginToken.WhitespacesBeforeCount == attr.EndToken.WhitespacesAfterCount) 
                    {
                        Pullenti.Ner.ReferentToken rt1 = Pullenti.Ner.Person.PersonAnalyzer.TryAttachPerson(attr.BeginToken, ad, false, -1, false);
                        if (rt1 != null) 
                            attrs1 = null;
                    }
                }
            }
            if (attrs1 != null) 
            {
                foreach (PersonAttrToken a in attrs1) 
                {
                    if (a.Typ != PersonAttrTerminType.Prefix) 
                    {
                        if (a.Age != null) 
                            p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_AGE, a.Age, true, 0);
                        else if (a.PropRef == null) 
                            p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_ATTR, a.Value, false, 0);
                        else 
                            p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_ATTR, a, false, 0);
                        end = a.EndToken;
                        if (a.Gender != Pullenti.Morph.MorphGender.Undefined && !p.IsFemale && !p.IsMale) 
                        {
                            if (a.Gender == Pullenti.Morph.MorphGender.Masculine && !p.IsMale) 
                            {
                                p.IsMale = true;
                                p.CorrectData();
                            }
                            else if (a.Gender == Pullenti.Morph.MorphGender.Feminie && !p.IsFemale) 
                            {
                                p.IsFemale = true;
                                p.CorrectData();
                            }
                        }
                    }
                }
                if (openBr) 
                {
                    if (end.Next != null && end.Next.IsChar(')')) 
                        end = end.Next;
                }
            }
            int crlfCou = 0;
            for (Pullenti.Ner.Token t = end.Next; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar) 
                    break;
                if (t.IsNewlineBefore) 
                {
                    Pullenti.Ner.Mail.Internal.MailLine ml = Pullenti.Ner.Mail.Internal.MailLine.Parse(t, 0, 0);
                    if (ml != null && ml.Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From) 
                        break;
                    crlfCou++;
                }
                if (t.IsCharOf(":,(") || t.IsHiphen) 
                    continue;
                if (t.IsChar('.') && t == end.Next) 
                    continue;
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r != null) 
                {
                    if (r.TypeName == "PHONE" || r.TypeName == "URI" || r.TypeName == "ADDRESS") 
                    {
                        string ty = r.GetStringValue("SCHEME");
                        if (r.TypeName == "URI") 
                        {
                            if ((ty != "mailto" && ty != "skype" && ty != "ICQ") && ty != "http") 
                                break;
                        }
                        p.AddContact(r);
                        end = t;
                        crlfCou = 0;
                        continue;
                    }
                }
                if (r is Pullenti.Ner.Person.PersonIdentityReferent) 
                {
                    p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_IDDOC, r, false, 0);
                    end = t;
                    crlfCou = 0;
                    continue;
                }
                if (r != null && r.TypeName == "ORGANIZATION") 
                {
                    if (t.Next != null && t.Next.Morph.Class.IsVerb) 
                        break;
                    if (begin.Previous != null && begin.Previous.Morph.Class.IsVerb) 
                        break;
                    if (t.WhitespacesAfterCount == 1) 
                        break;
                    bool exist = false;
                    foreach (Pullenti.Ner.Slot s in p.Slots) 
                    {
                        if (s.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_ATTR && (s.Value is Pullenti.Ner.Person.PersonPropertyReferent)) 
                        {
                            Pullenti.Ner.Person.PersonPropertyReferent pr = s.Value as Pullenti.Ner.Person.PersonPropertyReferent;
                            if (pr.FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r, true) != null) 
                            {
                                exist = true;
                                break;
                            }
                        }
                        else if (s.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_ATTR && (s.Value is PersonAttrToken)) 
                        {
                            PersonAttrToken pr = s.Value as PersonAttrToken;
                            if (pr.Referent.FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r, true) != null) 
                            {
                                exist = true;
                                break;
                            }
                        }
                    }
                    if (!exist) 
                    {
                        PersonAttrToken pat = new PersonAttrToken(t, t);
                        pat.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent() { Name = "сотрудник" };
                        pat.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r, false, 0);
                        p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_ATTR, pat, false, 0);
                    }
                    continue;
                }
                if (r != null) 
                    break;
                if (!hasPrefix || crlfCou >= 2) 
                    break;
                Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("PERSON", t);
                if (rt != null) 
                    break;
            }
            if (ad != null) 
                ad.OverflowLevel--;
            if (begin.IsValue("НА", null) && begin.Next != null && begin.Next.IsValue("ИМЯ", null)) 
            {
                Pullenti.Ner.Token t0 = begin.Previous;
                if (t0 != null && t0.IsComma) 
                    t0 = t0.Previous;
                if (t0 != null && (t0.GetReferent() is Pullenti.Ner.Person.PersonIdentityReferent)) 
                    p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_IDDOC, t0.GetReferent(), false, 0);
            }
            return new Pullenti.Ner.ReferentToken(p, begin, end) { Morph = morph, MiscAttrs = (int)p.m_PersonIdentityTyp };
        }
        /// <summary>
        /// Выделить пол
        /// </summary>
        public static Pullenti.Ner.Token CreateSex(Pullenti.Ner.Person.PersonReferent pr, Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            while (t.Next != null) 
            {
                if (t.IsValue("ПОЛ", null) || t.IsHiphen || t.IsChar(':')) 
                    t = t.Next;
                else 
                    break;
            }
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return null;
            bool ok = false;
            if ((tt.Term == "МУЖ" || tt.Term == "МУЖС" || tt.Term == "МУЖСК") || tt.IsValue("МУЖСКОЙ", null)) 
            {
                pr.IsMale = true;
                ok = true;
            }
            else if ((tt.Term == "ЖЕН" || tt.Term == "ЖЕНС" || tt.Term == "ЖЕНСК") || tt.IsValue("ЖЕНСКИЙ", null)) 
            {
                pr.IsFemale = true;
                ok = true;
            }
            if (!ok) 
                return null;
            while (t.Next != null) 
            {
                if (t.Next.IsValue("ПОЛ", null) || t.Next.IsChar('.')) 
                    t = t.Next;
                else 
                    break;
            }
            return t;
        }
        /// <summary>
        /// Выделить кличку
        /// </summary>
        /// <param name="t">начальный токен</param>
        /// <return>если не null, то последний токен клички, а в pr запишет саму кличку</return>
        public static Pullenti.Ner.Token CreateNickname(Pullenti.Ner.Person.PersonReferent pr, Pullenti.Ner.Token t)
        {
            bool hasKeyw = false;
            bool isBr = false;
            for (; t != null; t = t.Next) 
            {
                if (t.IsHiphen || t.IsComma || t.IsCharOf(".:;")) 
                    continue;
                if (t.Morph.Class.IsPreposition) 
                    continue;
                if (t.IsChar('(')) 
                {
                    isBr = true;
                    continue;
                }
                if ((t.IsValue("ПРОЗВИЩЕ", "ПРІЗВИСЬКО") || t.IsValue("КЛИЧКА", null) || t.IsValue("ПСЕВДОНИМ", "ПСЕВДОНІМ")) || t.IsValue("ПСЕВДО", null) || t.IsValue("ПОЗЫВНОЙ", "ПОЗИВНИЙ")) 
                {
                    hasKeyw = true;
                    continue;
                }
                break;
            }
            if (!hasKeyw || t == null) 
                return null;
            if (Pullenti.Ner.Core.BracketHelper.IsBracket(t, true)) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    string ni = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken.Next, br.EndToken.Previous, Pullenti.Ner.Core.GetTextAttr.No);
                    if (ni != null) 
                    {
                        pr.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, ni, false, 0);
                        t = br.EndToken;
                        for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.IsCommaAnd) 
                                continue;
                            if (!Pullenti.Ner.Core.BracketHelper.IsBracket(tt, true)) 
                                break;
                            br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br == null) 
                                break;
                            ni = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken.Next, br.EndToken.Previous, Pullenti.Ner.Core.GetTextAttr.No);
                            if (ni != null) 
                                pr.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, ni, false, 0);
                            t = (tt = br.EndToken);
                        }
                        if (isBr && t.Next != null && t.Next.IsChar(')')) 
                            t = t.Next;
                        return t;
                    }
                }
            }
            else 
            {
                Pullenti.Ner.Token ret = null;
                for (; t != null; t = t.Next) 
                {
                    if (t.IsCommaAnd) 
                        continue;
                    if (ret != null && t.Chars.IsAllLower) 
                        break;
                    if (t.WhitespacesBeforeCount > 2) 
                        break;
                    List<PersonItemToken> pli = PersonItemToken.TryAttachList(t, null, PersonItemToken.ParseAttr.No, 10);
                    if (pli != null && ((pli.Count == 1 || pli.Count == 2))) 
                    {
                        string ni = Pullenti.Ner.Core.MiscHelper.GetTextValue(pli[0].BeginToken, pli[pli.Count - 1].EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                        if (ni != null) 
                        {
                            pr.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, ni, false, 0);
                            t = pli[pli.Count - 1].EndToken;
                            if (isBr && t.Next != null && t.Next.IsChar(')')) 
                                t = t.Next;
                            ret = t;
                            continue;
                        }
                    }
                    if ((t is Pullenti.Ner.ReferentToken) && !t.Chars.IsAllLower && (t as Pullenti.Ner.ReferentToken).BeginToken == (t as Pullenti.Ner.ReferentToken).EndToken) 
                    {
                        string val = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(t as Pullenti.Ner.ReferentToken, Pullenti.Ner.Core.GetTextAttr.No);
                        pr.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, val, false, 0);
                        if (isBr && t.Next != null && t.Next.IsChar(')')) 
                            t = t.Next;
                        ret = t;
                        continue;
                    }
                    break;
                }
                return ret;
            }
            return null;
        }
        public static bool IsPersonSayOrAttrAfter(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            Pullenti.Ner.Token tt = CorrectTailAttributes(null, t);
            if (tt != null && tt != t) 
                return true;
            if (t.IsComma && t.Next != null) 
                t = t.Next;
            if (t.Chars.IsLatinLetter) 
            {
                if (t.IsValue("SAY", null) || t.IsValue("ASK", null) || t.IsValue("WHO", null)) 
                    return true;
            }
            if (t.IsChar('.') && (t.Next is Pullenti.Ner.TextToken) && ((t.Next.Morph.Class.IsPronoun || t.Next.Morph.Class.IsPersonalPronoun))) 
            {
                if (t.Next.Morph.Gender == Pullenti.Morph.MorphGender.Feminie || t.Next.Morph.Gender == Pullenti.Morph.MorphGender.Masculine) 
                    return true;
            }
            if (t.IsComma && t.Next != null) 
                t = t.Next;
            if (PersonAttrToken.TryAttach(t, null, PersonAttrToken.PersonAttrAttachAttrs.No) != null) 
                return true;
            return false;
        }
        static Pullenti.Ner.Token CorrectTailAttributes(Pullenti.Ner.Person.PersonReferent p, Pullenti.Ner.Token t0)
        {
            Pullenti.Ner.Token res = t0;
            Pullenti.Ner.Token t = t0;
            if (t != null && t.IsChar(',')) 
                t = t.Next;
            bool born = false;
            bool die = false;
            if (t != null && ((t.IsValue("РОДИТЬСЯ", "НАРОДИТИСЯ") || t.IsValue("BORN", null)))) 
            {
                t = t.Next;
                born = true;
            }
            else if (t != null && ((t.IsValue("УМЕРЕТЬ", "ПОМЕРТИ") || t.IsValue("СКОНЧАТЬСЯ", null) || t.IsValue("DIED", null)))) 
            {
                t = t.Next;
                die = true;
            }
            else if ((t != null && t.IsValue("ДАТА", null) && t.Next != null) && t.Next.IsValue("РОЖДЕНИЕ", "НАРОДЖЕННЯ")) 
            {
                t = t.Next.Next;
                born = true;
            }
            while (t != null) 
            {
                if (t.Morph.Class.IsPreposition || t.IsHiphen || t.IsChar(':')) 
                    t = t.Next;
                else 
                    break;
            }
            if (t != null && t.GetReferent() != null) 
            {
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r.TypeName == "DATE") 
                {
                    Pullenti.Ner.Token t1 = t;
                    if (t.Next != null && ((t.Next.IsValue("Р", null) || t.Next.IsValue("РОЖДЕНИЕ", "НАРОДЖЕННЯ")))) 
                    {
                        born = true;
                        t1 = t.Next;
                        if (t1.Next != null && t1.Next.IsChar('.')) 
                            t1 = t1.Next;
                    }
                    if (born) 
                    {
                        if (p != null) 
                            p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_BORN, r, false, 0);
                        res = t1;
                        t = t1;
                    }
                    else if (die) 
                    {
                        if (p != null) 
                            p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_DIE, r, false, 0);
                        res = t1;
                        t = t1;
                    }
                }
            }
            if (die && t != null) 
            {
                Pullenti.Ner.NumberToken ag = Pullenti.Ner.Core.NumberHelper.TryParseAge(t.Next);
                if (ag != null) 
                {
                    if (p != null) 
                        p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_AGE, ag.Value.ToString(), false, 0);
                    t = ag.EndToken.Next;
                    res = ag.EndToken;
                }
            }
            if (t == null) 
                return res;
            if (t.IsChar('(')) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    Pullenti.Ner.Token t1 = t.Next;
                    born = false;
                    if (t1.IsValue("РОД", null)) 
                    {
                        born = true;
                        t1 = t1.Next;
                        if (t1 != null && t1.IsChar('.')) 
                            t1 = t1.Next;
                    }
                    if (t1 is Pullenti.Ner.ReferentToken) 
                    {
                        Pullenti.Ner.Referent r = t1.GetReferent();
                        if (r.TypeName == "DATERANGE" && t1.Next == br.EndToken) 
                        {
                            Pullenti.Ner.Referent bd = r.GetSlotValue("FROM") as Pullenti.Ner.Referent;
                            Pullenti.Ner.Referent to = r.GetSlotValue("TO") as Pullenti.Ner.Referent;
                            if (bd != null && to != null) 
                            {
                                if (p != null) 
                                {
                                    p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_BORN, bd, false, 0);
                                    p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_DIE, to, false, 0);
                                }
                                t = (res = br.EndToken);
                            }
                        }
                        else if (r.TypeName == "DATE" && t1.Next == br.EndToken) 
                        {
                            if (p != null) 
                                p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_BORN, r, false, 0);
                            t = (res = br.EndToken);
                        }
                    }
                }
            }
            return res;
        }
    }
}