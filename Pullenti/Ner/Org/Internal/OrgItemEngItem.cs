/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Org.Internal
{
    class OrgItemEngItem : Pullenti.Ner.MetaToken
    {
        internal OrgItemEngItem(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public string FullValue;
        public string ShortValue;
        public bool IsBank
        {
            get
            {
                return FullValue == "bank";
            }
        }
        public static OrgItemEngItem TryAttach(Pullenti.Ner.Token t, bool canBeCyr = false)
        {
            if (t == null || !(t is Pullenti.Ner.TextToken)) 
                return null;
            Pullenti.Ner.Core.TerminToken tok = (canBeCyr ? m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No) : null);
            if (!t.Chars.IsLatinLetter && tok == null) 
            {
                if (!t.IsAnd || t.Next == null) 
                    return null;
                if (t.Next.IsValue("COMPANY", null) || t.Next.IsValue("CO", null)) 
                {
                    OrgItemEngItem res = new OrgItemEngItem(t, t.Next);
                    res.FullValue = "company";
                    if (res.EndToken.Next != null && res.EndToken.Next.IsChar('.')) 
                        res.EndToken = res.EndToken.Next;
                    return res;
                }
                return null;
            }
            if (t.Chars.IsLatinLetter) 
                tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null) 
            {
                if (!_checkTok(tok)) 
                    return null;
                OrgItemEngItem res = new OrgItemEngItem(tok.BeginToken, tok.EndToken);
                res.FullValue = tok.Termin.CanonicText.ToLower();
                res.ShortValue = tok.Termin.Acronym;
                return res;
            }
            return null;
        }
        static bool _checkTok(Pullenti.Ner.Core.TerminToken tok)
        {
            if (tok.Termin.Acronym == "SA") 
            {
                Pullenti.Ner.Token tt0 = tok.BeginToken.Previous;
                if (tt0 != null && tt0.IsChar('.')) 
                    tt0 = tt0.Previous;
                if (tt0 is Pullenti.Ner.TextToken) 
                {
                    if ((tt0 as Pullenti.Ner.TextToken).Term == "U") 
                        return false;
                }
            }
            else if (tok.BeginToken.IsValue("CO", null) && tok.BeginToken == tok.EndToken) 
            {
                if (tok.EndToken.Next != null && tok.EndToken.Next.IsHiphen) 
                    return false;
            }
            if (!tok.IsWhitespaceAfter) 
            {
                if (tok.EndToken.Next is Pullenti.Ner.NumberToken) 
                    return false;
            }
            return true;
        }
        public static Pullenti.Ner.ReferentToken TryAttachOrg(Pullenti.Ner.Token t, bool canBeCyr = false)
        {
            if (t == null) 
                return null;
            bool br = false;
            if (t.IsChar('(') && t.Next != null) 
            {
                t = t.Next;
                br = true;
            }
            if (t is Pullenti.Ner.NumberToken) 
            {
                if ((t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Words && t.Morph.Class.IsAdjective && t.Chars.IsCapitalUpper) 
                {
                }
                else 
                    return null;
            }
            else 
            {
                if (t.Chars.IsAllLower) 
                    return null;
                if ((t.LengthChar < 3) && !t.Chars.IsLetter) 
                    return null;
                if (!t.Chars.IsLatinLetter) 
                {
                    if (!canBeCyr || !t.Chars.IsCyrillicLetter) 
                        return null;
                }
            }
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = t0;
            int namWo = 0;
            OrgItemEngItem tok = null;
            Pullenti.Ner.Geo.GeoReferent geo = null;
            OrgItemTypeToken addTyp = null;
            for (; t != null; t = t.Next) 
            {
                if (t != t0 && t.WhitespacesBeforeCount > 1) 
                    break;
                if (t.IsChar(')')) 
                    break;
                if (t.IsChar('(') && t.Next != null) 
                {
                    if ((t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && t.Next.Next != null && t.Next.Next.IsChar(')')) 
                    {
                        geo = t.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                        t = t.Next.Next;
                        continue;
                    }
                    OrgItemTypeToken typ = OrgItemTypeToken.TryAttach(t.Next, true, null);
                    if ((typ != null && typ.EndToken.Next != null && typ.EndToken.Next.IsChar(')')) && typ.Chars.IsLatinLetter) 
                    {
                        addTyp = typ;
                        t = typ.EndToken.Next;
                        continue;
                    }
                    if (((t.Next is Pullenti.Ner.TextToken) && t.Next.Next != null && t.Next.Next.IsChar(')')) && t.Next.Chars.IsCapitalUpper) 
                    {
                        t1 = (t = t.Next.Next);
                        continue;
                    }
                    break;
                }
                tok = TryAttach(t, canBeCyr);
                if (tok == null && t.IsCharOf(".,") && t.Next != null) 
                {
                    tok = TryAttach(t.Next, canBeCyr);
                    if (tok == null && t.Next.IsCharOf(",.")) 
                        tok = TryAttach(t.Next.Next, canBeCyr);
                }
                if (tok != null) 
                {
                    if (tok.LengthChar == 1 && t0.Chars.IsCyrillicLetter) 
                        return null;
                    break;
                }
                if (t.IsHiphen && !t.IsWhitespaceAfter && !t.IsWhitespaceBefore) 
                    continue;
                if (t.IsCharOf("&+") || t.IsAnd) 
                    continue;
                if (t.IsChar('.')) 
                {
                    if (t.Previous != null && t.Previous.LengthChar == 1) 
                        continue;
                    else if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t.Next)) 
                        break;
                }
                if (!t.Chars.IsLatinLetter) 
                {
                    if (!canBeCyr || !t.Chars.IsCyrillicLetter) 
                        break;
                }
                if (t.Chars.IsAllLower) 
                {
                    if (t.Morph.Class.IsPreposition || t.Morph.Class.IsConjunction) 
                        continue;
                    if (br) 
                        continue;
                    break;
                }
                Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                if (mc.IsVerb) 
                {
                    if (t.Next != null && t.Next.Morph.Class.IsPreposition) 
                        break;
                }
                if (t.Next != null && t.Next.IsValue("OF", null)) 
                    break;
                if (t is Pullenti.Ner.TextToken) 
                    namWo++;
                t1 = t;
            }
            if (tok == null) 
                return null;
            if (t0 == tok.BeginToken) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br2 = Pullenti.Ner.Core.BracketHelper.TryParse(tok.EndToken.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br2 != null) 
                {
                    Pullenti.Ner.Org.OrganizationReferent org1 = new Pullenti.Ner.Org.OrganizationReferent();
                    if (tok.ShortValue != null) 
                        org1.AddTypeStr(tok.ShortValue);
                    org1.AddTypeStr(tok.FullValue);
                    string nam1 = Pullenti.Ner.Core.MiscHelper.GetTextValue(br2.BeginToken, br2.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    if (nam1 != null) 
                    {
                        org1.AddName(nam1, true, null);
                        return new Pullenti.Ner.ReferentToken(org1, t0, br2.EndToken);
                    }
                }
                return null;
            }
            Pullenti.Ner.Org.OrganizationReferent org = new Pullenti.Ner.Org.OrganizationReferent();
            Pullenti.Ner.Token te = tok.EndToken;
            if (tok.IsBank) 
                t1 = tok.EndToken;
            if (tok.FullValue == "company" && (tok.WhitespacesAfterCount < 3)) 
            {
                OrgItemEngItem tok1 = TryAttach(tok.EndToken.Next, canBeCyr);
                if (tok1 != null) 
                {
                    t1 = tok.EndToken;
                    tok = tok1;
                    te = tok.EndToken;
                }
            }
            if (tok.FullValue == "company") 
            {
                if (namWo == 0) 
                    return null;
            }
            string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t0, t1, Pullenti.Ner.Core.GetTextAttr.IgnoreArticles);
            if (nam == "STOCK" && tok.FullValue == "company") 
                return null;
            string altNam = null;
            if (string.IsNullOrEmpty(nam)) 
                return null;
            if (nam.IndexOf('(') > 0) 
            {
                int i1 = nam.IndexOf('(');
                int i2 = nam.IndexOf(')');
                if (i1 < i2) 
                {
                    altNam = nam;
                    string tai = null;
                    if ((i2 + 1) < nam.Length) 
                        tai = nam.Substring(i2).Trim();
                    nam = nam.Substring(0, i1).Trim();
                    if (tai != null) 
                        nam = string.Format("{0} {1}", nam, tai);
                }
            }
            if (tok.IsBank) 
            {
                org.AddTypeStr((tok.Kit.BaseLanguage.IsEn ? "bank" : "банк"));
                org.AddProfile(Pullenti.Ner.Org.OrgProfile.Finance);
                if ((t1.Next != null && t1.Next.IsValue("OF", null) && t1.Next.Next != null) && t1.Next.Next.Chars.IsLatinLetter) 
                {
                    OrgItemNameToken nam0 = OrgItemNameToken.TryAttach(t1.Next, null, false, false);
                    if (nam0 != null) 
                        te = nam0.EndToken;
                    else 
                        te = t1.Next.Next;
                    nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t0, te, Pullenti.Ner.Core.GetTextAttr.No);
                    if (te.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                        org.AddGeoObject(te.GetReferent() as Pullenti.Ner.Geo.GeoReferent);
                }
                else if (t0 == t1) 
                    return null;
            }
            else 
            {
                if (tok.ShortValue != null) 
                    org.AddTypeStr(tok.ShortValue);
                org.AddTypeStr(tok.FullValue);
            }
            if (string.IsNullOrEmpty(nam)) 
                return null;
            org.AddName(nam, true, null);
            if (altNam != null) 
                org.AddName(altNam, true, null);
            Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(org, t0, te);
            t = te;
            while (t.Next != null) 
            {
                if (t.Next.IsCharOf(",.")) 
                    t = t.Next;
                else 
                    break;
            }
            if (t.WhitespacesAfterCount < 2) 
            {
                tok = TryAttach(t.Next, canBeCyr);
                if (tok != null) 
                {
                    if (tok.ShortValue != null) 
                        org.AddTypeStr(tok.ShortValue);
                    org.AddTypeStr(tok.FullValue);
                    res.EndToken = tok.EndToken;
                }
            }
            if (geo != null) 
                org.AddGeoObject(geo);
            if (addTyp != null) 
                org.AddType(addTyp, false);
            if (!br) 
                return res;
            t = res.EndToken;
            if (t.Next == null || t.Next.IsChar(')')) 
                res.EndToken = t.Next;
            else 
                return null;
            return res;
        }
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("BANK");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Public Limited Company".ToUpper()) { Acronym = "PLC" };
            t.AddAbridge("P.L.C.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Limited Liability Company".ToUpper()) { Acronym = "LLC" };
            t.AddAbridge("L.L.C.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Limited Liability Partnership".ToUpper()) { Acronym = "LLP" };
            t.AddAbridge("L.L.P.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Limited Liability Limited Partnership".ToUpper()) { Acronym = "LLLP" };
            t.AddAbridge("L.L.L.P.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Limited Duration Company".ToUpper()) { Acronym = "LDC" };
            t.AddAbridge("L.D.C.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("International Business Company".ToUpper()) { Acronym = "IBC" };
            t.AddAbridge("I.B.S.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Joint stock company".ToUpper()) { Acronym = "JSC" };
            t.AddAbridge("J.S.C.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Open Joint stock company".ToUpper()) { Acronym = "OJSC" };
            t.AddAbridge("O.J.S.C.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Sosiedad Anonima".ToUpper()) { Acronym = "SA" };
            t.AddVariant("Sociedad Anonima".ToUpper(), false);
            t.AddAbridge("S.A.");
            t.AddVariant("SPA", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Société en commandite".ToUpper()) { Acronym = "SC" };
            t.AddAbridge("S.C.");
            t.AddVariant("SCS", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Societas Europaea".ToUpper()) { Acronym = "SE" };
            t.AddAbridge("S.E.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Società in accomandita".ToUpper()) { Acronym = "SAS" };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Société en commandite par actions".ToUpper()) { Acronym = "SCA" };
            t.AddAbridge("S.C.A.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Société en nom collectif".ToUpper()) { Acronym = "SNC" };
            t.AddVariant("Società in nome collettivo".ToUpper(), false);
            t.AddAbridge("S.N.C.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("General Partnership".ToUpper()) { Acronym = "GP" };
            t.AddVariant("General Partners", false);
            t.AddAbridge("G.P.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Limited Partnership".ToUpper()) { Acronym = "LP" };
            t.AddAbridge("L.P.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Kommanditaktiengesellschaft".ToUpper()) { Acronym = "KGAA" };
            t.AddVariant("KOMMAG", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Societe a Responsidilite Limitee".ToUpper()) { Acronym = "SRL" };
            t.AddAbridge("S.A.R.L.");
            t.AddAbridge("S.R.L.");
            t.AddVariant("SARL", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Società a garanzia limitata".ToUpper()) { Acronym = "SAGL" };
            t.AddAbridge("S.A.G.L.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Società limitata".ToUpper()) { Acronym = "SL" };
            t.AddAbridge("S.L.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Vennootschap Met Beperkte Aansparkelij kheid".ToUpper()) { Acronym = "BV" };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Vennootschap Met Beperkte Aansparkelij".ToUpper()) { Acronym = "AVV" };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Naamlose Vennootschap".ToUpper()) { Acronym = "NV" };
            t.AddAbridge("N.V.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Gesellschaft mit beschrakter Haftung".ToUpper()) { Acronym = "GMBH" };
            t.AddVariant("ГМБХ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Aktiengesellschaft".ToUpper()) { Acronym = "AG" };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("International Company".ToUpper()) { Acronym = "IC" };
            t.AddAbridge("I.C.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("And Company".ToUpper());
            t.AddVariant("& Company", false);
            t.AddVariant("& Co", false);
            t.AddVariant("& Company", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Kollektivgesellschaft".ToUpper()) { Acronym = "KG" };
            t.AddAbridge("K.G.");
            t.AddVariant("OHG", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("Kommanditgesellschaft".ToUpper()) { Acronym = "KG" };
            t.AddVariant("KOMMG", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("LIMITED");
            t.AddAbridge("LTD");
            t.AddVariant("LTD", false);
            t.AddVariant("ЛИМИТЕД", false);
            t.AddVariant("ЛТД", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("PRIVATE LIMITED");
            t.AddVariant("PTE LTD", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("INCORPORATED");
            t.AddAbridge("INC");
            t.AddVariant("INC", false);
            t.AddVariant("ИНКОРПОРЕЙТЕД", false);
            t.AddVariant("ИНК", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("CORPORATION");
            t.AddVariant("CO", false);
            t.AddVariant("СО", false);
            t.AddVariant("КОРПОРЕЙШН", false);
            t.AddVariant("КОРПОРЕЙШЕН", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("COMPANY");
            m_Ontology.Add(t);
        }
        static Pullenti.Ner.Core.TerminCollection m_Ontology;
    }
}