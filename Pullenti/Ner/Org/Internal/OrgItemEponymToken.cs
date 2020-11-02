/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Org.Internal
{
    public class OrgItemEponymToken : Pullenti.Ner.MetaToken
    {
        private OrgItemEponymToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public List<string> Eponyms = new List<string>();
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append("имени");
            foreach (string e in Eponyms) 
            {
                res.AppendFormat(" {0}", e);
            }
            return res.ToString();
        }
        public static OrgItemEponymToken TryAttach(Pullenti.Ner.Token t, bool mustHasPrefix = false)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
            {
                if (t == null) 
                    return null;
                Pullenti.Ner.Referent r1 = t.GetReferent();
                if (r1 != null && r1.TypeName == "DATE") 
                {
                    string str = r1.ToString().ToUpper();
                    if ((str == "1 МАЯ" || str == "7 ОКТЯБРЯ" || str == "9 МАЯ") || str == "8 МАРТА") 
                    {
                        OrgItemEponymToken dt = new OrgItemEponymToken(t, t) { Eponyms = new List<string>() };
                        dt.Eponyms.Add(str);
                        return dt;
                    }
                }
                Pullenti.Ner.NumberToken age = Pullenti.Ner.Core.NumberHelper.TryParseAge(t);
                if ((age != null && (((age.EndToken.Next is Pullenti.Ner.TextToken) || (age.EndToken.Next is Pullenti.Ner.ReferentToken))) && (age.WhitespacesAfterCount < 3)) && !age.EndToken.Next.Chars.IsAllLower && age.EndToken.Next.Chars.IsCyrillicLetter) 
                {
                    OrgItemEponymToken dt = new OrgItemEponymToken(t, age.EndToken.Next) { Eponyms = new List<string>() };
                    dt.Eponyms.Add(string.Format("{0} {1}", age.Value, dt.EndToken.GetSourceText().ToUpper()));
                    return dt;
                }
                return null;
            }
            Pullenti.Ner.Token t1 = null;
            bool full = false;
            bool hasName = false;
            if (tt.Term == "ИМЕНИ" || tt.Term == "ІМЕНІ") 
            {
                t1 = t.Next;
                full = true;
                hasName = true;
            }
            else if (((tt.Term == "ИМ" || tt.Term == "ІМ")) && tt.Next != null) 
            {
                if (tt.Next.IsChar('.')) 
                {
                    t1 = tt.Next.Next;
                    full = true;
                }
                else if ((tt.Next is Pullenti.Ner.TextToken) && tt.Chars.IsAllLower && !tt.Next.Chars.IsAllLower) 
                    t1 = tt.Next;
                hasName = true;
            }
            else if (tt.Previous != null && ((tt.Previous.IsValue("ФОНД", null) || tt.Previous.IsValue("ХРАМ", null) || tt.Previous.IsValue("ЦЕРКОВЬ", "ЦЕРКВА")))) 
            {
                if ((!tt.Chars.IsCyrillicLetter || tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction) || !tt.Chars.IsLetter) 
                    return null;
                if (tt.WhitespacesBeforeCount != 1) 
                    return null;
                if (tt.Chars.IsAllLower) 
                    return null;
                if (tt.Morph.Class.IsAdjective) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.BeginToken != npt.EndToken) 
                        return null;
                }
                OrgItemNameToken na = OrgItemNameToken.TryAttach(tt, null, false, true);
                if (na != null) 
                {
                    if (na.IsEmptyWord || na.IsStdName || na.IsStdTail) 
                        return null;
                }
                t1 = tt;
            }
            if (t1 == null || ((t1.IsNewlineBefore && !full))) 
                return null;
            if (tt.Previous != null && tt.Previous.Morph.Class.IsPreposition) 
                return null;
            if (mustHasPrefix && !hasName) 
                return null;
            Pullenti.Ner.Referent r = t1.GetReferent();
            if ((r != null && r.TypeName == "DATE" && full) && r.FindSlot("DAY", null, true) != null && r.FindSlot("YEAR", null, true) == null) 
            {
                OrgItemEponymToken dt = new OrgItemEponymToken(t, t1) { Eponyms = new List<string>() };
                dt.Eponyms.Add(r.ToString().ToUpper());
                return dt;
            }
            bool holy = false;
            if ((t1.IsValue("СВЯТОЙ", null) || t1.IsValue("СВЯТИЙ", null) || t1.IsValue("СВ", null)) || t1.IsValue("СВЯТ", null)) 
            {
                t1 = t1.Next;
                holy = true;
                if (t1 != null && t1.IsChar('.')) 
                    t1 = t1.Next;
            }
            if (t1 == null) 
                return null;
            Pullenti.Morph.MorphClass cl = t1.GetMorphClassInDictionary();
            if (cl.IsNoun || cl.IsAdjective) 
            {
                Pullenti.Ner.ReferentToken rt = t1.Kit.ProcessReferent("PERSON", t1);
                if (rt != null && rt.Referent.TypeName == "PERSON" && rt.BeginToken != rt.EndToken) 
                {
                    string e = rt.Referent.GetStringValue("LASTNAME");
                    if (e != null) 
                    {
                        if (rt.EndToken.IsValue(e, null)) 
                        {
                            OrgItemEponymToken re = new OrgItemEponymToken(t, rt.EndToken);
                            re.Eponyms.Add(rt.EndToken.GetSourceText());
                            return re;
                        }
                    }
                }
            }
            Pullenti.Ner.NumberToken nt = Pullenti.Ner.Core.NumberHelper.TryParseAnniversary(t1);
            if (nt != null && nt.Typ == Pullenti.Ner.NumberSpellingType.Age) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(nt.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    string s = string.Format("{0}-{1} {2}", nt.Value, (t.Kit.BaseLanguage.IsUa ? "РОКІВ" : "ЛЕТ"), Pullenti.Ner.Core.MiscHelper.GetTextValue(npt.BeginToken, npt.EndToken, Pullenti.Ner.Core.GetTextAttr.No));
                    OrgItemEponymToken res = new OrgItemEponymToken(t, npt.EndToken);
                    res.Eponyms.Add(s);
                    return res;
                }
            }
            List<PersonItemToken> its = PersonItemToken.TryAttach(t1);
            if (its == null) 
            {
                if ((t1 is Pullenti.Ner.ReferentToken) && (t1.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(t1, t1, Pullenti.Ner.Core.GetTextAttr.No);
                    OrgItemEponymToken re = new OrgItemEponymToken(t, t1);
                    re.Eponyms.Add(s);
                    return re;
                }
                return null;
            }
            List<string> eponims = new List<string>();
            int i = 0;
            int j;
            if (its[i].Typ == PersonItemType.LocaseWord) 
                i++;
            if (i >= its.Count) 
                return null;
            if (!full) 
            {
                if (its[i].BeginToken.Morph.Class.IsAdjective && !its[i].BeginToken.Morph.Class.IsProperSurname) 
                    return null;
            }
            if (its[i].Typ == PersonItemType.Initial) 
            {
                i++;
                while (true) 
                {
                    if ((i < its.Count) && its[i].Typ == PersonItemType.Initial) 
                        i++;
                    if (i >= its.Count || ((its[i].Typ != PersonItemType.Surname && its[i].Typ != PersonItemType.Name))) 
                        break;
                    eponims.Add(its[i].Value);
                    t1 = its[i].EndToken;
                    if ((i + 2) >= its.Count || its[i + 1].Typ != PersonItemType.And || its[i + 2].Typ != PersonItemType.Initial) 
                        break;
                    i += 3;
                }
            }
            else if (((i + 1) < its.Count) && its[i].Typ == PersonItemType.Name && its[i + 1].Typ == PersonItemType.Surname) 
            {
                eponims.Add(its[i + 1].Value);
                t1 = its[i + 1].EndToken;
                i += 2;
                if ((((i + 2) < its.Count) && its[i].Typ == PersonItemType.And && its[i + 1].Typ == PersonItemType.Name) && its[i + 2].Typ == PersonItemType.Surname) 
                {
                    eponims.Add(its[i + 2].Value);
                    t1 = its[i + 2].EndToken;
                }
            }
            else if (its[i].Typ == PersonItemType.Surname) 
            {
                if (its.Count == (i + 2) && its[i].Chars == its[i + 1].Chars) 
                {
                    its[i].Value += (" " + its[i + 1].Value);
                    its[i].EndToken = its[i + 1].EndToken;
                    its.RemoveAt(i + 1);
                }
                eponims.Add(its[i].Value);
                if (((i + 1) < its.Count) && its[i + 1].Typ == PersonItemType.Name) 
                {
                    if ((i + 2) == its.Count) 
                        i++;
                    else if (its[i + 2].Typ != PersonItemType.Surname) 
                        i++;
                }
                else if (((i + 1) < its.Count) && its[i + 1].Typ == PersonItemType.Initial) 
                {
                    if ((i + 2) == its.Count) 
                        i++;
                    else if (its[i + 2].Typ == PersonItemType.Initial && (i + 3) == its.Count) 
                        i += 2;
                }
                else if (((i + 2) < its.Count) && its[i + 1].Typ == PersonItemType.And && its[i + 2].Typ == PersonItemType.Surname) 
                {
                    bool ok = true;
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(its[i + 2].BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && !npt.Morph.Case.IsGenitive && !npt.Morph.Case.IsUndefined) 
                        ok = false;
                    if (ok) 
                    {
                        eponims.Add(its[i + 2].Value);
                        i += 2;
                    }
                }
                t1 = its[i].EndToken;
            }
            else if (its[i].Typ == PersonItemType.Name && holy) 
            {
                t1 = its[i].EndToken;
                bool sec = false;
                if (((i + 1) < its.Count) && its[i].Chars == its[i + 1].Chars && its[i + 1].Typ != PersonItemType.Initial) 
                {
                    sec = true;
                    t1 = its[i + 1].EndToken;
                }
                if (sec) 
                    eponims.Add(string.Format("СВЯТ.{0} {1}", its[i].Value, its[i + 1].Value));
                else 
                    eponims.Add(string.Format("СВЯТ.{0}", its[i].Value));
            }
            else if (full && (i + 1) == its.Count && ((its[i].Typ == PersonItemType.Name || its[i].Typ == PersonItemType.Surname))) 
            {
                t1 = its[i].EndToken;
                eponims.Add(its[i].Value);
            }
            else if ((its[i].Typ == PersonItemType.Name && its.Count == 3 && its[i + 1].Typ == PersonItemType.Name) && its[i + 2].Typ == PersonItemType.Surname) 
            {
                t1 = its[i + 2].EndToken;
                eponims.Add(string.Format("{0} {1} {2}", its[i].Value, its[i + 1].Value, its[i + 2].Value));
                i += 2;
            }
            if (eponims.Count == 0) 
                return null;
            return new OrgItemEponymToken(t, t1) { Eponyms = eponims };
        }
        enum PersonItemType : int
        {
            Surname,
            Name,
            Initial,
            And,
            LocaseWord,
        }

        class PersonItemToken : Pullenti.Ner.MetaToken
        {
            private PersonItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
            {
            }
            public Pullenti.Ner.Org.Internal.OrgItemEponymToken.PersonItemType Typ;
            public string Value;
            public override string ToString()
            {
                return string.Format("{0} {1}", Typ, Value ?? "");
            }
            public static List<PersonItemToken> TryAttach(Pullenti.Ner.Token t)
            {
                List<PersonItemToken> res = new List<PersonItemToken>();
                for (; t != null; t = t.Next) 
                {
                    if (t.IsNewlineBefore && res.Count > 0) 
                        break;
                    Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                    if (tt == null) 
                        break;
                    string s = tt.Term;
                    if (!char.IsLetter(s[0])) 
                        break;
                    if (((s.Length == 1 || s == "ДЖ")) && !tt.Chars.IsAllLower) 
                    {
                        Pullenti.Ner.Token t1 = t;
                        if (t1.Next != null && t1.Next.IsChar('.')) 
                            t1 = t1.Next;
                        res.Add(new PersonItemToken(t, t1) { Typ = Pullenti.Ner.Org.Internal.OrgItemEponymToken.PersonItemType.Initial, Value = s });
                        t = t1;
                        continue;
                    }
                    if (tt.IsAnd) 
                    {
                        res.Add(new PersonItemToken(t, t) { Typ = Pullenti.Ner.Org.Internal.OrgItemEponymToken.PersonItemType.And });
                        continue;
                    }
                    if (tt.Morph.Class.IsPronoun || tt.Morph.Class.IsPersonalPronoun) 
                        break;
                    if (tt.Chars.IsAllLower) 
                    {
                        Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                        if (mc.IsPreposition || mc.IsVerb || mc.IsAdverb) 
                            break;
                        Pullenti.Ner.Token t1 = t;
                        if (t1.Next != null && !t1.IsWhitespaceAfter && t1.Next.IsChar('.')) 
                            t1 = t1.Next;
                        res.Add(new PersonItemToken(t, t1) { Typ = Pullenti.Ner.Org.Internal.OrgItemEponymToken.PersonItemType.LocaseWord, Value = s });
                        t = t1;
                        continue;
                    }
                    if (tt.Morph.Class.IsProperName) 
                        res.Add(new PersonItemToken(t, t) { Typ = Pullenti.Ner.Org.Internal.OrgItemEponymToken.PersonItemType.Name, Value = s });
                    else if ((t.Next != null && t.Next.IsHiphen && (t.Next.Next is Pullenti.Ner.TextToken)) && !t.Next.IsWhitespaceAfter) 
                    {
                        res.Add(new PersonItemToken(t, t.Next.Next) { Typ = Pullenti.Ner.Org.Internal.OrgItemEponymToken.PersonItemType.Surname, Value = string.Format("{0}-{1}", s, (t.Next.Next as Pullenti.Ner.TextToken).Term) });
                        t = t.Next.Next;
                    }
                    else 
                        res.Add(new PersonItemToken(t, t) { Typ = Pullenti.Ner.Org.Internal.OrgItemEponymToken.PersonItemType.Surname, Value = s });
                }
                return (res.Count > 0 ? res : null);
            }
        }

    }
}