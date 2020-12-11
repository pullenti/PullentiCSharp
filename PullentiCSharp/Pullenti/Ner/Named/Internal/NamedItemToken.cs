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

namespace Pullenti.Ner.Named.Internal
{
    class NamedItemToken : Pullenti.Ner.MetaToken
    {
        public NamedItemToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public Pullenti.Ner.Named.NamedEntityKind Kind;
        public string NameValue;
        public string TypeValue;
        public Pullenti.Ner.Referent Ref;
        public bool IsWellknown;
        public bool IsInBracket;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Kind != Pullenti.Ner.Named.NamedEntityKind.Undefined) 
                res.AppendFormat(" [{0}]", Kind);
            if (IsWellknown) 
                res.AppendFormat(" (!)");
            if (IsInBracket) 
                res.AppendFormat(" [br]");
            if (TypeValue != null) 
                res.AppendFormat(" {0}", TypeValue);
            if (NameValue != null) 
                res.AppendFormat(" \"{0}\"", NameValue);
            if (Ref != null) 
                res.AppendFormat(" -> {0}", Ref.ToString());
            return res.ToString();
        }
        public static List<NamedItemToken> TryParseList(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locOnto)
        {
            NamedItemToken ne = TryParse(t, locOnto);
            if (ne == null) 
                return null;
            List<NamedItemToken> res = new List<NamedItemToken>();
            res.Add(ne);
            for (t = ne.EndToken.Next; t != null; t = t.Next) 
            {
                if (t.WhitespacesBeforeCount > 2) 
                    break;
                ne = TryParse(t, locOnto);
                if (ne == null) 
                    break;
                if (t.IsValue("НЕТ", null)) 
                    break;
                res.Add(ne);
                t = ne.EndToken;
            }
            return res;
        }
        public static NamedItemToken TryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locOnto)
        {
            if (t == null) 
                return null;
            if (t is Pullenti.Ner.ReferentToken) 
            {
                Pullenti.Ner.Referent r = t.GetReferent();
                if ((r.TypeName == "PERSON" || r.TypeName == "PERSONPROPERTY" || (r is Pullenti.Ner.Geo.GeoReferent)) || r.TypeName == "ORGANIZATION") 
                    return new NamedItemToken(t, t) { Ref = r, Morph = t.Morph };
                return null;
            }
            Pullenti.Ner.Core.TerminToken typ = m_Types.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            Pullenti.Ner.Core.TerminToken nam = m_Names.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (typ != null) 
            {
                if (!(t is Pullenti.Ner.TextToken)) 
                    return null;
                NamedItemToken res = new NamedItemToken(typ.BeginToken, typ.EndToken) { Morph = typ.Morph, Chars = typ.Chars };
                res.Kind = (Pullenti.Ner.Named.NamedEntityKind)typ.Termin.Tag;
                res.TypeValue = typ.Termin.CanonicText;
                if ((nam != null && nam.EndToken == typ.EndToken && !t.Chars.IsAllLower) && ((Pullenti.Ner.Named.NamedEntityKind)nam.Termin.Tag) == res.Kind) 
                {
                    res.NameValue = nam.Termin.CanonicText;
                    res.IsWellknown = true;
                }
                return res;
            }
            if (nam != null) 
            {
                if (nam.BeginToken.Chars.IsAllLower) 
                    return null;
                NamedItemToken res = new NamedItemToken(nam.BeginToken, nam.EndToken) { Morph = nam.Morph, Chars = nam.Chars };
                res.Kind = (Pullenti.Ner.Named.NamedEntityKind)nam.Termin.Tag;
                res.NameValue = nam.Termin.CanonicText;
                bool ok = true;
                if (!t.IsWhitespaceBefore && t.Previous != null) 
                    ok = false;
                else if (!t.IsWhitespaceAfter && t.Next != null) 
                {
                    if (t.Next.IsCharOf(",.;!?") && t.Next.IsWhitespaceAfter) 
                    {
                    }
                    else 
                        ok = false;
                }
                if (ok) 
                {
                    res.IsWellknown = true;
                    res.TypeValue = nam.Termin.Tag2 as string;
                }
                return res;
            }
            Pullenti.Ner.MetaToken adj = Pullenti.Ner.Geo.Internal.MiscLocationHelper.TryAttachNordWest(t);
            if (adj != null) 
            {
                if (adj.Morph.Class.IsNoun) 
                {
                    if (adj.EndToken.IsValue("ВОСТОК", null)) 
                    {
                        if (adj.BeginToken == adj.EndToken) 
                            return null;
                        NamedItemToken re = new NamedItemToken(t, adj.EndToken) { Morph = adj.Morph };
                        re.Kind = Pullenti.Ner.Named.NamedEntityKind.Location;
                        re.NameValue = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, adj.EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                        re.IsWellknown = true;
                        return re;
                    }
                    return null;
                }
                if (adj.WhitespacesAfterCount > 2) 
                    return null;
                if ((adj.EndToken.Next is Pullenti.Ner.ReferentToken) && (adj.EndToken.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    NamedItemToken re = new NamedItemToken(t, adj.EndToken.Next) { Morph = adj.EndToken.Next.Morph };
                    re.Kind = Pullenti.Ner.Named.NamedEntityKind.Location;
                    re.NameValue = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, adj.EndToken.Next, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                    re.IsWellknown = true;
                    re.Ref = adj.EndToken.Next.GetReferent();
                    return re;
                }
                NamedItemToken res = TryParse(adj.EndToken.Next, locOnto);
                if (res != null && res.Kind == Pullenti.Ner.Named.NamedEntityKind.Location) 
                {
                    string s = adj.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, res.Morph.Gender, false);
                    if (s != null) 
                    {
                        if (res.NameValue == null) 
                            res.NameValue = s.ToUpper();
                        else 
                        {
                            res.NameValue = string.Format("{0} {1}", s.ToUpper(), res.NameValue);
                            res.TypeValue = null;
                        }
                        res.BeginToken = t;
                        res.Chars = t.Chars;
                        res.IsWellknown = true;
                        return res;
                    }
                }
            }
            if (t.Chars.IsCapitalUpper && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.Adjectives.Count > 0) 
                {
                    NamedItemToken test = TryParse(npt.Noun.BeginToken, locOnto);
                    if (test != null && test.EndToken == npt.EndToken && test.TypeValue != null) 
                    {
                        test.BeginToken = t;
                        StringBuilder tmp = new StringBuilder();
                        foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
                        {
                            string s = a.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, test.Morph.Gender, false);
                            if (tmp.Length > 0) 
                                tmp.Append(' ');
                            tmp.Append(s);
                        }
                        test.NameValue = tmp.ToString();
                        test.Chars = t.Chars;
                        if (test.Kind == Pullenti.Ner.Named.NamedEntityKind.Location) 
                            test.IsWellknown = true;
                        return test;
                    }
                }
            }
            if ((Pullenti.Ner.Core.BracketHelper.IsBracket(t, true) && t.Next != null && t.Next.Chars.IsLetter) && !t.Next.Chars.IsAllLower) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    NamedItemToken res = new NamedItemToken(t, br.EndToken);
                    res.IsInBracket = true;
                    res.NameValue = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, br.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    nam = m_Names.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (nam != null && nam.EndToken == br.EndToken.Previous) 
                    {
                        res.Kind = (Pullenti.Ner.Named.NamedEntityKind)nam.Termin.Tag;
                        res.IsWellknown = true;
                        res.NameValue = nam.Termin.CanonicText;
                    }
                    return res;
                }
            }
            if (((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && !t.Chars.IsAllLower) && t.LengthChar > 2) 
            {
                NamedItemToken res = new NamedItemToken(t, t) { Morph = t.Morph };
                string str = (t as Pullenti.Ner.TextToken).Term;
                if (str.EndsWith("О") || str.EndsWith("И") || str.EndsWith("Ы")) 
                    res.NameValue = str;
                else 
                    res.NameValue = t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                res.Chars = t.Chars;
                if (((!t.IsWhitespaceAfter && t.Next != null && t.Next.IsHiphen) && (t.Next.Next is Pullenti.Ner.TextToken) && !t.Next.Next.IsWhitespaceAfter) && t.Chars.IsCyrillicLetter == t.Next.Next.Chars.IsCyrillicLetter) 
                {
                    t = (res.EndToken = t.Next.Next);
                    res.NameValue = string.Format("{0}-{1}", res.NameValue, t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                }
                return res;
            }
            return null;
        }
        internal static void Initialize()
        {
            if (m_Types != null) 
                return;
            m_Types = new Pullenti.Ner.Core.TerminCollection();
            m_Names = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            foreach (string s in new string[] {"ПЛАНЕТА", "ЗВЕЗДА", "КОМЕТА", "МЕТЕОРИТ", "СОЗВЕЗДИЕ", "ГАЛАКТИКА"}) 
            {
                t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(s, null);
                t.Tag = Pullenti.Ner.Named.NamedEntityKind.Planet;
                m_Types.Add(t);
            }
            foreach (string s in new string[] {"СОЛНЦЕ", "МЕРКУРИЙ", "ВЕНЕРА", "ЗЕМЛЯ", "МАРС", "ЮПИТЕР", "САТУРН", "УРАН", "НЕПТУН", "ПЛУТОН", "ЛУНА", "ДЕЙМОС", "ФОБОС", "Ио", "Ганимед", "Каллисто"}) 
            {
                t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(s.ToUpper(), null);
                t.Tag = Pullenti.Ner.Named.NamedEntityKind.Planet;
                m_Names.Add(t);
            }
            foreach (string s in new string[] {"РЕКА", "ОЗЕРО", "МОРЕ", "ОКЕАН", "ЗАЛИВ", "ПРОЛИВ", "ПОБЕРЕЖЬЕ", "КОНТИНЕНТ", "ОСТРОВ", "ПОЛУОСТРОВ", "МЫС", "ГОРА", "ГОРНЫЙ ХРЕБЕТ", "ПЕРЕВАЛ", "ЛЕС", "САД", "ЗАПОВЕДНИК", "ЗАКАЗНИК", "ДОЛИНА", "УЩЕЛЬЕ", "РАВНИНА", "БЕРЕГ"}) 
            {
                t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(s, null);
                t.Tag = Pullenti.Ner.Named.NamedEntityKind.Location;
                m_Types.Add(t);
            }
            foreach (string s in new string[] {"ТИХИЙ", "АТЛАНТИЧЕСКИЙ", "ИНДИЙСКИЙ", "СЕВЕРО-ЛЕДОВИТЫЙ"}) 
            {
                t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(s, null);
                t.Tag = Pullenti.Ner.Named.NamedEntityKind.Location;
                t.Tag2 = "океан";
                m_Names.Add(t);
            }
            foreach (string s in new string[] {"ЕВРАЗИЯ", "АФРИКА", "АМЕРИКА", "АВСТРАЛИЯ", "АНТАРКТИДА"}) 
            {
                t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(s, null);
                t.Tag = Pullenti.Ner.Named.NamedEntityKind.Location;
                t.Tag2 = "континент";
                m_Names.Add(t);
            }
            foreach (string s in new string[] {"ВОЛГА", "НЕВА", "АМУР", "ОБЪ", "АНГАРА", "ЛЕНА", "ИРТЫШ", "ДНЕПР", "ДОН", "ДНЕСТР", "РЕЙН", "АМУДАРЬЯ", "СЫРДАРЬЯ", "ТИГР", "ЕВФРАТ", "ИОРДАН", "МИССИСИПИ", "АМАЗОНКА", "ТЕМЗА", "СЕНА", "НИЛ", "ЯНЦЗЫ", "ХУАНХЭ", "ПАРАНА", "МЕКОНГ", "МАККЕНЗИ", "НИГЕР", "ЕНИСЕЙ", "МУРРЕЙ", "САЛУИН", "ИНД", "РИО-ГРАНДЕ", "БРАХМАПУТРА", "ДАРЛИНГ", "ДУНАЙ", "ЮКОН", "ГАНГ", "МАРРАМБИДЖИ", "ЗАМБЕЗИ", "ТОКАНТИС", "ОРИНОКО", "СИЦЗЯН", "КОЛЫМА", "КАМА", "ОКА", "ЭЛЬЮА", "ВИСЛА", "ДАУГАВА", "ЗАПАДНАЯ ДВИНА", "НЕМАН", "МЕЗЕНЬ", "КУБАНЬ", "ЮЖНЫЙ БУГ"}) 
            {
                t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(s, null);
                t.Tag = Pullenti.Ner.Named.NamedEntityKind.Location;
                t.Tag2 = "река";
                m_Names.Add(t);
            }
            foreach (string s in new string[] {"ЕВРОПА", "АЗИЯ", "АРКТИКА", "КАВКАЗ", "ПРИБАЛТИКА", "СИБИРЬ", "ЗАПОЛЯРЬЕ", "ЧУКОТКА", "ПРИБАЛТИКА", "БАЛКАНЫ", "СКАНДИНАВИЯ", "ОКЕАНИЯ", "АЛЯСКА", "УРАЛ", "ПОВОЛЖЬЕ", "ПРИМОРЬЕ", "КУРИЛЫ", "ТИБЕТ", "ГИМАЛАИ", "АЛЬПЫ", "САХАРА", "ГОБИ", "СИНАЙ", "БАЙКОНУР", "ЧЕРНОБЫЛЬ", "САДОВОЕ КОЛЬЦО", "СТАРЫЙ ГОРОД"}) 
            {
                t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(s, null);
                t.Tag = Pullenti.Ner.Named.NamedEntityKind.Location;
                m_Names.Add(t);
            }
            foreach (string s in new string[] {"ПАМЯТНИК", "МОНУМЕНТ", "МЕМОРИАЛ", "БЮСТ", "ОБЕЛИСК"}) 
            {
                t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(s, null);
                t.Tag = Pullenti.Ner.Named.NamedEntityKind.Monument;
                m_Types.Add(t);
            }
            foreach (string s in new string[] {"ДВОРЕЦ", "КРЕМЛЬ", "ЗАМОК", "УСАДЬБА", "ДОМ", "ЗДАНИЕ", "ШТАБ-КВАРТИРА", "ЖЕЛЕЗНОДОРОЖНЫЙ ВОКЗАЛ", "ВОКЗАЛ", "АВТОВОКЗАЛ", "АЭРОПОРТ", "АЭРОДРОМ"}) 
            {
                t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(s, null);
                t.Tag = Pullenti.Ner.Named.NamedEntityKind.Building;
                m_Types.Add(t);
            }
            foreach (string s in new string[] {"КРЕМЛЬ", "КАПИТОЛИЙ", "БЕЛЫЙ ДОМ"}) 
            {
                t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(s, null);
                t.Tag = Pullenti.Ner.Named.NamedEntityKind.Building;
                m_Names.Add(t);
            }
            t = new Pullenti.Ner.Core.Termin("МЕЖДУНАРОДНАЯ КОСМИЧЕСКАЯ СТАНЦИЯ") { Tag = Pullenti.Ner.Named.NamedEntityKind.Building };
            t.Acronym = "МКС";
            m_Names.Add(t);
        }
        static Pullenti.Ner.Core.TerminCollection m_Types;
        static Pullenti.Ner.Core.TerminCollection m_Names;
    }
}