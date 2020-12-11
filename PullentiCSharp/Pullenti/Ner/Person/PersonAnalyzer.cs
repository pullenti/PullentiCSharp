/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Person
{
    /// <summary>
    /// Анализатор выделения персон и их атрибутов (должности, звания и пр.)
    /// </summary>
    public class PersonAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("PERSON")
        /// </summary>
        public const string ANALYZER_NAME = "PERSON";
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
                return "Персоны";
            }
        }
        public override string Description
        {
            get
            {
                return "Персоны и их атрибуты";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new PersonAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Person.Internal.MetaPerson.GlobalMeta, Pullenti.Ner.Person.Internal.MetaPersonProperty.GlobalMeta, Pullenti.Ner.Person.Internal.MetaPersonIdentity.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Person.Internal.MetaPerson.ManImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("man.png"));
                res.Add(Pullenti.Ner.Person.Internal.MetaPerson.WomenImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("women.png"));
                res.Add(Pullenti.Ner.Person.Internal.MetaPerson.PersonImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("person.png"));
                res.Add(Pullenti.Ner.Person.Internal.MetaPerson.GeneralImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("general.png"));
                res.Add(Pullenti.Ner.Person.Internal.MetaPersonProperty.PersonPropImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("personproperty.png"));
                res.Add(Pullenti.Ner.Person.Internal.MetaPersonProperty.PersonPropBossImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("boss.png"));
                res.Add(Pullenti.Ner.Person.Internal.MetaPersonProperty.PersonPropKingImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("king.png"));
                res.Add(Pullenti.Ner.Person.Internal.MetaPersonProperty.PersonPropKinImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("kin.png"));
                res.Add(Pullenti.Ner.Person.Internal.MetaPersonProperty.PersonPropMilitaryId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("militaryrank.png"));
                res.Add(Pullenti.Ner.Person.Internal.MetaPersonProperty.PersonPropNationId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("nationality.png"));
                res.Add(Pullenti.Ner.Person.Internal.MetaPersonIdentity.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("identity.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == PersonReferent.OBJ_TYPENAME) 
                return new PersonReferent();
            if (type == PersonPropertyReferent.OBJ_TYPENAME) 
                return new PersonPropertyReferent();
            if (type == PersonIdentityReferent.OBJ_TYPENAME) 
                return new PersonIdentityReferent();
            return null;
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {"ORGANIZATION", "GEO", "ADDRESS", "TRANSPORT"};
            }
        }
        public override int ProgressWeight
        {
            get
            {
                return 35;
            }
        }
        internal class PersonAnalyzerData : Pullenti.Ner.Core.AnalyzerDataWithOntology
        {
            public bool NominativeCaseAlways = false;
            public bool TextStartsWithLastnameFirstnameMiddlename = false;
            public bool NeedSecondStep = false;
            public override Pullenti.Ner.Referent RegisterReferent(Pullenti.Ner.Referent referent)
            {
                if (referent is Pullenti.Ner.Person.PersonReferent) 
                {
                    List<Pullenti.Ner.Person.PersonPropertyReferent> existProps = null;
                    for (int i = 0; i < referent.Slots.Count; i++) 
                    {
                        Pullenti.Ner.Slot a = referent.Slots[i];
                        if (a.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_ATTR) 
                        {
                            Pullenti.Ner.Person.Internal.PersonAttrToken pat = a.Value as Pullenti.Ner.Person.Internal.PersonAttrToken;
                            if (pat == null || pat.PropRef == null) 
                            {
                                if (a.Value is Pullenti.Ner.Person.PersonPropertyReferent) 
                                {
                                    if (existProps == null) 
                                        existProps = new List<Pullenti.Ner.Person.PersonPropertyReferent>();
                                    existProps.Add(a.Value as Pullenti.Ner.Person.PersonPropertyReferent);
                                }
                                continue;
                            }
                            if (pat.PropRef != null) 
                            {
                                foreach (Pullenti.Ner.Slot ss in pat.PropRef.Slots) 
                                {
                                    if (ss.TypeName == Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF) 
                                    {
                                        if (ss.Value is Pullenti.Ner.ReferentToken) 
                                        {
                                            if ((ss.Value as Pullenti.Ner.ReferentToken).Referent == referent) 
                                            {
                                                pat.PropRef.Slots.Remove(ss);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (existProps != null) 
                            {
                                foreach (Pullenti.Ner.Person.PersonPropertyReferent pp in existProps) 
                                {
                                    if (pp.CanBeEquals(pat.PropRef, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                    {
                                        if (pat.PropRef.CanBeGeneralFor(pp)) 
                                        {
                                            pat.PropRef.MergeSlots(pp, true);
                                            break;
                                        }
                                    }
                                }
                            }
                            pat.Data = this;
                            pat.SaveToLocalOntology();
                            if (pat.PropRef != null) 
                            {
                                if (referent.FindSlot(a.TypeName, pat.PropRef, true) != null) 
                                {
                                    referent.Slots.RemoveAt(i);
                                    i--;
                                }
                                else 
                                    referent.UploadSlot(a, pat.Referent);
                            }
                        }
                    }
                }
                if (referent is Pullenti.Ner.Person.PersonPropertyReferent) 
                {
                    for (int i = 0; i < referent.Slots.Count; i++) 
                    {
                        Pullenti.Ner.Slot a = referent.Slots[i];
                        if (a.TypeName == Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF || a.TypeName == Pullenti.Ner.Person.PersonPropertyReferent.ATTR_HIGHER) 
                        {
                            Pullenti.Ner.ReferentToken pat = a.Value as Pullenti.Ner.ReferentToken;
                            if (pat != null) 
                            {
                                pat.Data = this;
                                pat.SaveToLocalOntology();
                                if (pat.Referent != null) 
                                    referent.UploadSlot(a, pat.Referent);
                            }
                            else if (a.Value is Pullenti.Ner.Person.PersonPropertyReferent) 
                            {
                                if (a.Value == referent) 
                                {
                                    referent.Slots.RemoveAt(i);
                                    i--;
                                    continue;
                                }
                                referent.UploadSlot(a, this.RegisterReferent(a.Value as Pullenti.Ner.Person.PersonPropertyReferent));
                            }
                        }
                    }
                }
                Pullenti.Ner.Referent res = base.RegisterReferent(referent);
                return res;
            }
            public Dictionary<int, bool> CanBePersonPropBeginChars = new Dictionary<int, bool>();
        }

        /// <summary>
        /// При анализе считать, что все персоны идут в именительном падеже
        /// </summary>
        public static bool NominativeCaseAlways = false;
        /// <summary>
        /// При анализе считать, что текст начинается с Фамилии Имени Отчества
        /// </summary>
        public static bool TextStartsWithLastnameFirstnameMiddlename = false;
        public override Pullenti.Ner.Core.AnalyzerData CreateAnalyzerData()
        {
            return new PersonAnalyzerData();
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            PersonAnalyzerData ad = kit.GetAnalyzerData(this) as PersonAnalyzerData;
            ad.NominativeCaseAlways = NominativeCaseAlways;
            ad.TextStartsWithLastnameFirstnameMiddlename = TextStartsWithLastnameFirstnameMiddlename;
            ad.NeedSecondStep = false;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                t.InnerBool = false;
            }
            int steps = 2;
            int max = steps;
            int delta = 100000;
            int parts = (((kit.Sofa.Text.Length + delta) - 1)) / delta;
            if (parts == 0) 
                parts = 1;
            max *= parts;
            int cur = 0;
            for (int step = 0; step < steps; step++) 
            {
                int nextPos = delta;
                for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
                {
                    if (t.BeginChar > nextPos) 
                    {
                        nextPos += delta;
                        cur++;
                        if (!this.OnProgress(cur, max, kit)) 
                            return;
                    }
                    List<Pullenti.Ner.ReferentToken> rts = this.TryAttachPersons(t, ad, step);
                    if (rts != null) 
                    {
                        if (!Pullenti.Ner.MetaToken.Check(rts)) 
                        {
                        }
                        else 
                            foreach (Pullenti.Ner.ReferentToken rt in rts) 
                            {
                                if (rt.Referent == null) 
                                    t = rt.EndToken;
                                else 
                                {
                                    List<Pullenti.Ner.Person.Internal.PersonAttrToken> pats = new List<Pullenti.Ner.Person.Internal.PersonAttrToken>();
                                    foreach (Pullenti.Ner.Slot s in rt.Referent.Slots) 
                                    {
                                        if (s.Value is Pullenti.Ner.Person.Internal.PersonAttrToken) 
                                        {
                                            Pullenti.Ner.Person.Internal.PersonAttrToken pat = s.Value as Pullenti.Ner.Person.Internal.PersonAttrToken;
                                            pats.Add(pat);
                                            if (pat.PropRef == null) 
                                                continue;
                                            foreach (Pullenti.Ner.Slot ss in pat.PropRef.Slots) 
                                            {
                                                if (ss.TypeName == PersonPropertyReferent.ATTR_REF && (ss.Value is Pullenti.Ner.ReferentToken)) 
                                                {
                                                    Pullenti.Ner.ReferentToken rt1 = ss.Value as Pullenti.Ner.ReferentToken;
                                                    rt1.Referent = ad.RegisterReferent(rt1.Referent);
                                                    ss.Value = rt1.Referent;
                                                    Pullenti.Ner.ReferentToken rr = new Pullenti.Ner.ReferentToken(rt1.Referent, rt1.BeginToken, rt1.EndToken) { Morph = rt1.Morph };
                                                    kit.EmbedToken(rr);
                                                    if (rr.BeginToken == rt.BeginToken) 
                                                        rt.BeginToken = rr;
                                                    if (rr.EndToken == rt.EndToken) 
                                                        rt.EndToken = rr;
                                                    if (rr.BeginToken == pat.BeginToken) 
                                                        pat.BeginToken = rr;
                                                    if (rr.EndToken == pat.EndToken) 
                                                        pat.EndToken = rr;
                                                }
                                            }
                                        }
                                        else if (s.Value is Pullenti.Ner.ReferentToken) 
                                        {
                                            Pullenti.Ner.ReferentToken rt0 = s.Value as Pullenti.Ner.ReferentToken;
                                            if (rt0.Referent != null) 
                                            {
                                                foreach (Pullenti.Ner.Slot s1 in rt0.Referent.Slots) 
                                                {
                                                    if (s1.Value is Pullenti.Ner.Person.Internal.PersonAttrToken) 
                                                    {
                                                        Pullenti.Ner.Person.Internal.PersonAttrToken pat = s1.Value as Pullenti.Ner.Person.Internal.PersonAttrToken;
                                                        if (pat.PropRef == null) 
                                                            continue;
                                                        foreach (Pullenti.Ner.Slot ss in pat.PropRef.Slots) 
                                                        {
                                                            if (ss.TypeName == PersonPropertyReferent.ATTR_REF && (ss.Value is Pullenti.Ner.ReferentToken)) 
                                                            {
                                                                Pullenti.Ner.ReferentToken rt1 = ss.Value as Pullenti.Ner.ReferentToken;
                                                                rt1.Referent = ad.RegisterReferent(rt1.Referent);
                                                                ss.Value = rt1.Referent;
                                                                Pullenti.Ner.ReferentToken rr = new Pullenti.Ner.ReferentToken(rt1.Referent, rt1.BeginToken, rt1.EndToken) { Morph = rt1.Morph };
                                                                kit.EmbedToken(rr);
                                                                if (rr.BeginToken == rt0.BeginToken) 
                                                                    rt0.BeginToken = rr;
                                                                if (rr.EndToken == rt0.EndToken) 
                                                                    rt0.EndToken = rr;
                                                                if (rr.BeginToken == pat.BeginToken) 
                                                                    pat.BeginToken = rr;
                                                                if (rr.EndToken == pat.EndToken) 
                                                                    pat.EndToken = rr;
                                                            }
                                                        }
                                                        pat.PropRef = ad.RegisterReferent(pat.PropRef) as PersonPropertyReferent;
                                                        Pullenti.Ner.ReferentToken rt2 = new Pullenti.Ner.ReferentToken(pat.PropRef, pat.BeginToken, pat.EndToken) { Morph = pat.Morph };
                                                        kit.EmbedToken(rt2);
                                                        if (rt2.BeginToken == rt0.BeginToken) 
                                                            rt0.BeginToken = rt2;
                                                        if (rt2.EndToken == rt0.EndToken) 
                                                            rt0.EndToken = rt2;
                                                        s1.Value = pat.PropRef;
                                                    }
                                                }
                                            }
                                            rt0.Referent = ad.RegisterReferent(rt0.Referent);
                                            if (rt0.BeginChar == rt.BeginChar) 
                                                rt.BeginToken = rt0;
                                            if (rt0.EndChar == rt.EndChar) 
                                                rt.EndToken = rt0;
                                            kit.EmbedToken(rt0);
                                            s.Value = rt0.Referent;
                                        }
                                    }
                                    rt.Referent = ad.RegisterReferent(rt.Referent);
                                    foreach (Pullenti.Ner.Person.Internal.PersonAttrToken p in pats) 
                                    {
                                        if (p.PropRef != null) 
                                        {
                                            Pullenti.Ner.ReferentToken rr = new Pullenti.Ner.ReferentToken(p.PropRef, p.BeginToken, p.EndToken) { Morph = p.Morph };
                                            kit.EmbedToken(rr);
                                            if (rr.BeginToken == rt.BeginToken) 
                                                rt.BeginToken = rr;
                                            if (rr.EndToken == rt.EndToken) 
                                                rt.EndToken = rr;
                                        }
                                    }
                                    kit.EmbedToken(rt);
                                    t = rt;
                                }
                            }
                    }
                    else if (step == 0) 
                    {
                        Pullenti.Ner.ReferentToken rt = Pullenti.Ner.Person.Internal.PersonIdToken.TryAttach(t);
                        if (rt != null) 
                        {
                            rt.Referent = ad.RegisterReferent(rt.Referent);
                            Pullenti.Ner.Token tt = t.Previous;
                            if (tt != null && tt.IsCharOf(":,")) 
                                tt = tt.Previous;
                            PersonReferent pers = (tt == null ? null : tt.GetReferent() as PersonReferent);
                            if (pers != null) 
                                pers.AddSlot(PersonReferent.ATTR_IDDOC, rt.Referent, false, 0);
                            kit.EmbedToken(rt);
                            t = rt;
                        }
                    }
                }
                if (ad.Referents.Count == 0 && !ad.NeedSecondStep) 
                    break;
            }
            Dictionary<PersonPropertyReferent, List<PersonReferent>> props = new Dictionary<PersonPropertyReferent, List<PersonReferent>>();
            foreach (Pullenti.Ner.Referent r in ad.Referents) 
            {
                PersonReferent p = r as PersonReferent;
                if (p == null) 
                    continue;
                foreach (Pullenti.Ner.Slot s in p.Slots) 
                {
                    if (s.TypeName == PersonReferent.ATTR_ATTR && (s.Value is PersonPropertyReferent)) 
                    {
                        PersonPropertyReferent pr = s.Value as PersonPropertyReferent;
                        List<PersonReferent> li;
                        if (!props.TryGetValue(pr, out li)) 
                            props.Add(pr, (li = new List<PersonReferent>()));
                        if (!li.Contains(p)) 
                            li.Add(p);
                    }
                }
            }
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (t is Pullenti.Ner.ReferentToken) 
                {
                    if (t.Chars.IsLatinLetter && Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(t.Next)) 
                    {
                    }
                    else 
                        continue;
                }
                if (!ad.CanBePersonPropBeginChars.ContainsKey(t.BeginChar)) 
                    continue;
                Pullenti.Ner.Person.Internal.PersonAttrToken pat = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(t, ad.LocalOntology, Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                if (pat == null) 
                    continue;
                if (pat.PropRef == null || ((pat.Typ != Pullenti.Ner.Person.Internal.PersonAttrTerminType.Position && pat.Typ != Pullenti.Ner.Person.Internal.PersonAttrTerminType.King))) 
                {
                    t = pat.EndToken;
                    continue;
                }
                List<PersonReferent> pers = new List<PersonReferent>();
                foreach (KeyValuePair<PersonPropertyReferent, List<PersonReferent>> kp in props) 
                {
                    if (kp.Key.CanBeEquals(pat.PropRef, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                    {
                        foreach (PersonReferent pp in kp.Value) 
                        {
                            if (!pers.Contains(pp)) 
                                pers.Add(pp);
                        }
                        if (pers.Count > 1) 
                            break;
                    }
                }
                if (pers.Count == 1) 
                {
                    Pullenti.Ner.Token tt = pat.EndToken.Next;
                    if (tt != null && ((tt.IsChar('_') || tt.IsNewlineBefore || tt.IsTableControlChar))) 
                    {
                    }
                    else 
                    {
                        pat.Data = ad;
                        pat.SaveToLocalOntology();
                        kit.EmbedToken(pat);
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(pers[0], pat, pat) { Morph = pat.Morph };
                        kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                }
                if (pat.PropRef != null) 
                {
                    if (pat.CanBeIndependentProperty || pers.Count > 0) 
                    {
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(pat.PropRef), pat.BeginToken, pat.EndToken) { Morph = pat.Morph };
                        kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                }
                t = pat.EndToken;
            }
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            if (begin == null || m_Level > 2) 
                return null;
            m_Level++;
            PersonAnalyzerData ad = begin.Kit.GetAnalyzerData(this) as PersonAnalyzerData;
            Pullenti.Ner.ReferentToken rt = TryAttachPerson(begin, ad, false, -1, false);
            m_Level--;
            if (rt != null && rt.Referent == null) 
                rt = null;
            if (rt != null) 
            {
                rt.Data = begin.Kit.GetAnalyzerData(this);
                return rt;
            }
            m_Level++;
            Pullenti.Ner.Person.Internal.PersonAttrToken pat = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(begin, null, Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
            m_Level--;
            if (pat == null || pat.PropRef == null) 
                return null;
            rt = new Pullenti.Ner.ReferentToken(pat.PropRef, pat.BeginToken, pat.EndToken) { Morph = pat.Morph };
            rt.Data = ad;
            return rt;
        }
        int m_Level = 0;
        List<Pullenti.Ner.ReferentToken> TryAttachPersons(Pullenti.Ner.Token t, PersonAnalyzerData ad, int step)
        {
            Pullenti.Ner.ReferentToken rt = TryAttachPerson(t, ad, false, step, false);
            if (rt == null) 
                return null;
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            res.Add(rt);
            List<Pullenti.Ner.Person.Internal.PersonItemToken> names = null;
            for (Pullenti.Ner.Token tt = rt.EndToken.Next; tt != null; tt = tt.Next) 
            {
                if (!tt.IsCommaAnd) 
                    break;
                List<Pullenti.Ner.Person.Internal.PersonItemToken> pits = Pullenti.Ner.Person.Internal.PersonItemToken.TryAttachList(tt.Next, null, Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr.No, 10);
                if (pits == null || pits.Count != 1) 
                    break;
                Pullenti.Ner.ReferentToken rt1 = TryAttachPerson(tt.Next, ad, false, step, false);
                if (rt1 != null) 
                    break;
                if (pits[0].Firstname == null || pits[0].Firstname.Vars.Count == 0) 
                    break;
                if (names == null) 
                    names = new List<Pullenti.Ner.Person.Internal.PersonItemToken>();
                names.Add(pits[0]);
                if (tt.IsAnd) 
                    break;
                tt = tt.Next;
            }
            if (names != null) 
            {
                foreach (Pullenti.Ner.Person.Internal.PersonItemToken n in names) 
                {
                    PersonReferent pers = new PersonReferent();
                    Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo() { Number = Pullenti.Morph.MorphNumber.Singular, Language = t.Kit.BaseLanguage };
                    bi.Class = new Pullenti.Morph.MorphClass() { IsProperSurname = true };
                    if (n.Firstname.Vars[0].Gender == Pullenti.Morph.MorphGender.Feminie) 
                    {
                        pers.IsFemale = true;
                        bi.Gender = Pullenti.Morph.MorphGender.Feminie;
                    }
                    else if (n.Firstname.Vars[0].Gender == Pullenti.Morph.MorphGender.Masculine) 
                    {
                        pers.IsMale = true;
                        bi.Gender = Pullenti.Morph.MorphGender.Masculine;
                    }
                    foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v in n.Firstname.Vars) 
                    {
                        pers.AddSlot(PersonReferent.ATTR_FIRSTNAME, v.Value, false, 0);
                    }
                    foreach (Pullenti.Ner.Slot s in rt.Referent.Slots) 
                    {
                        if (s.TypeName == PersonReferent.ATTR_ATTR) 
                            pers.AddSlot(s.TypeName, s.Value, false, 0);
                        else if (s.TypeName == PersonReferent.ATTR_LASTNAME) 
                        {
                            string sur = s.Value as string;
                            if (bi.Gender != Pullenti.Morph.MorphGender.Undefined) 
                            {
                                string sur0 = Pullenti.Morph.MorphologyService.GetWordform(sur, bi);
                                if (sur0 != null) 
                                    pers.AddSlot(PersonReferent.ATTR_LASTNAME, sur0, false, 0);
                            }
                            pers.AddSlot(PersonReferent.ATTR_LASTNAME, sur, false, 0);
                        }
                    }
                    res.Add(new Pullenti.Ner.ReferentToken(pers, n.BeginToken, n.EndToken) { Morph = n.Morph });
                }
            }
            return res;
        }
        internal static Pullenti.Ner.ReferentToken TryAttachPerson(Pullenti.Ner.Token t, PersonAnalyzerData ad, bool forExtOntos, int step, bool forAttribute = false)
        {
            List<Pullenti.Ner.Person.Internal.PersonAttrToken> attrs = null;
            Pullenti.Morph.MorphBaseInfo mi = new Pullenti.Morph.MorphBaseInfo();
            mi.Case = ((forExtOntos || ((ad != null && ad.NominativeCaseAlways))) ? Pullenti.Morph.MorphCase.Nominative : Pullenti.Morph.MorphCase.AllCases);
            mi.Gender = Pullenti.Morph.MorphGender.Masculine | Pullenti.Morph.MorphGender.Feminie;
            Pullenti.Ner.Token t0 = t;
            bool and = false;
            bool andWasTerminated = false;
            bool isGenitive = false;
            bool canAttachToPreviousPerson = true;
            bool isKing = false;
            bool afterBePredicate = false;
            for (; t != null; t = t.Next) 
            {
                if (attrs != null && t.Next != null) 
                {
                    if (and) 
                        break;
                    if (t.IsChar(',')) 
                        t = t.Next;
                    else if (t.IsAnd && t.IsWhitespaceAfter && t.Chars.IsAllLower) 
                    {
                        t = t.Next;
                        and = true;
                    }
                    else if (t.IsHiphen && t.IsNewlineAfter) 
                    {
                        t = t.Next;
                        and = true;
                    }
                    else if (t.IsHiphen && t.WhitespacesAfterCount == 1 && t.WhitespacesBeforeCount == 1) 
                    {
                        t = t.Next;
                        and = true;
                    }
                    else if ((t.IsHiphen && t.Next != null && t.Next.IsHiphen) && t.Next.WhitespacesAfterCount == 1 && t.WhitespacesBeforeCount == 1) 
                    {
                        t = t.Next.Next;
                        and = true;
                    }
                    else if (t.IsChar(':')) 
                    {
                        if (!attrs[attrs.Count - 1].Morph.Case.IsNominative && !attrs[attrs.Count - 1].Morph.Case.IsUndefined) 
                        {
                        }
                        else 
                        {
                            mi.Case = Pullenti.Morph.MorphCase.Nominative;
                            mi.Gender = Pullenti.Morph.MorphGender.Masculine | Pullenti.Morph.MorphGender.Feminie;
                        }
                        t = t.Next;
                        if (!Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false)) 
                            canAttachToPreviousPerson = false;
                    }
                    else if (t.IsChar('_')) 
                    {
                        int cou = 0;
                        Pullenti.Ner.Token te = t;
                        for (; te != null; te = te.Next) 
                        {
                            if (!te.IsChar('_') || ((te.IsWhitespaceBefore && te != t))) 
                                break;
                            else 
                                cou++;
                        }
                        if (cou > 2 && ((!t.IsNewlineBefore || ((te != null && !te.IsNewlineBefore))))) 
                        {
                            mi.Case = Pullenti.Morph.MorphCase.Nominative;
                            mi.Gender = Pullenti.Morph.MorphGender.Masculine | Pullenti.Morph.MorphGender.Feminie;
                            canAttachToPreviousPerson = false;
                            t = te;
                            if (t != null && t.IsChar('/') && t.Next != null) 
                                t = t.Next;
                            break;
                        }
                    }
                    else if ((t.IsValue("ЯВЛЯТЬСЯ", null) || t.IsValue("БЫТЬ", null) || t.IsValue("Є", null)) || t.IsValue("IS", null)) 
                    {
                        mi.Case = Pullenti.Morph.MorphCase.Nominative;
                        mi.Gender = Pullenti.Morph.MorphGender.Masculine | Pullenti.Morph.MorphGender.Feminie;
                        afterBePredicate = true;
                        continue;
                    }
                    else if (((t.IsValue("LIKE", null) || t.IsValue("AS", null))) && attrs != null) 
                    {
                        t = t.Next;
                        break;
                    }
                }
                if (t.Chars.IsLatinLetter && step == 0) 
                {
                    Pullenti.Ner.Token tt2 = t;
                    if (Pullenti.Ner.Core.MiscHelper.IsEngArticle(t)) 
                        tt2 = t.Next;
                    Pullenti.Ner.Person.Internal.PersonItemToken pit0 = Pullenti.Ner.Person.Internal.PersonItemToken.TryAttach(tt2, (ad == null ? null : ad.LocalOntology), Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr.CanBeLatin, null);
                    if (pit0 != null && Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(pit0.EndToken.Next) && ad != null) 
                    {
                        PersonReferent pp = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttachOntoForSingle(pit0, ad.LocalOntology);
                        if (pp == null) 
                            pp = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttachLatinSurname(pit0, ad.LocalOntology);
                        if (pp != null) 
                            return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pp, pit0.BeginToken, pit0.EndToken, pit0.Morph, attrs, ad, forAttribute, afterBePredicate);
                    }
                }
                Pullenti.Ner.Person.Internal.PersonAttrToken a = null;
                if ((step < 1) || t.InnerBool) 
                {
                    a = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(t, (ad == null ? null : ad.LocalOntology), Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                    if (step == 0 && a != null) 
                        t.InnerBool = true;
                }
                if ((a != null && a.BeginToken == a.EndToken && !a.BeginToken.Chars.IsAllLower) && (a.WhitespacesAfterCount < 3)) 
                {
                    List<Pullenti.Ner.Person.Internal.PersonItemToken> pits = Pullenti.Ner.Person.Internal.PersonItemToken.TryAttachList(t, (ad == null ? null : ad.LocalOntology), Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr.IgnoreAttrs, 10);
                    if (pits != null && pits.Count >= 6) 
                    {
                        if (pits[2].IsNewlineAfter && pits[5].IsNewlineAfter) 
                            a = null;
                    }
                }
                if ((a == null && t.IsValue("НА", null) && t.Next != null) && t.Next.IsValue("ИМЯ", null)) 
                {
                    a = new Pullenti.Ner.Person.Internal.PersonAttrToken(t, t.Next) { Morph = new Pullenti.Ner.MorphCollection() { Case = Pullenti.Morph.MorphCase.Genitive } };
                    isGenitive = true;
                }
                if (a == null) 
                    break;
                if (afterBePredicate) 
                    return null;
                if (!t.Chars.IsAllLower && a.BeginToken == a.EndToken) 
                {
                    Pullenti.Ner.Person.Internal.PersonItemToken pit = Pullenti.Ner.Person.Internal.PersonItemToken.TryAttach(t, (ad == null ? null : ad.LocalOntology), Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr.CanBeLatin, null);
                    if (pit != null && pit.Lastname != null && ((pit.Lastname.IsInOntology || pit.Lastname.IsInDictionary))) 
                        break;
                }
                if (ad != null && !ad.CanBePersonPropBeginChars.ContainsKey(a.BeginChar)) 
                    ad.CanBePersonPropBeginChars.Add(a.BeginChar, true);
                if (attrs == null) 
                {
                    if (a.IsDoubt) 
                    {
                        if (a.IsNewlineAfter) 
                            break;
                    }
                    attrs = new List<Pullenti.Ner.Person.Internal.PersonAttrToken>();
                }
                else if (!a.Morph.Case.IsUndefined && !mi.Case.IsUndefined) 
                {
                    if (((a.Morph.Case & mi.Case)).IsUndefined) 
                    {
                        attrs.Clear();
                        mi.Case = (forExtOntos ? Pullenti.Morph.MorphCase.Nominative : Pullenti.Morph.MorphCase.AllCases);
                        mi.Gender = Pullenti.Morph.MorphGender.Masculine | Pullenti.Morph.MorphGender.Feminie;
                        isKing = false;
                    }
                }
                attrs.Add(a);
                if (attrs.Count > 5) 
                    return new Pullenti.Ner.ReferentToken(null, attrs[0].BeginToken, a.EndToken);
                if (a.Typ == Pullenti.Ner.Person.Internal.PersonAttrTerminType.King) 
                    isKing = true;
                if (a.Typ == Pullenti.Ner.Person.Internal.PersonAttrTerminType.BestRegards) 
                    mi.Case = Pullenti.Morph.MorphCase.Nominative;
                if (and) 
                    andWasTerminated = true;
                if (a.CanHasPersonAfter == 0) 
                {
                    if (a.Gender != Pullenti.Morph.MorphGender.Undefined) 
                    {
                        if (a.Typ != Pullenti.Ner.Person.Internal.PersonAttrTerminType.Position) 
                            mi.Gender &= a.Gender;
                        else if (a.Gender == Pullenti.Morph.MorphGender.Feminie) 
                            mi.Gender &= a.Gender;
                    }
                    if (!a.Morph.Case.IsUndefined) 
                        mi.Case &= a.Morph.Case;
                }
                t = a.EndToken;
            }
            if (attrs != null && and && !andWasTerminated) 
            {
                if ((t != null && t.Previous != null && t.Previous.IsHiphen) && (t.WhitespacesBeforeCount < 2)) 
                {
                }
                else 
                    return null;
            }
            if (attrs != null) 
            {
                if (t != null && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t, false, null, false)) 
                    t = t.Next;
            }
            while (t != null && ((t.IsTableControlChar || t.IsChar('_')))) 
            {
                t = t.Next;
            }
            if (t == null) 
            {
                if (attrs != null) 
                {
                    Pullenti.Ner.Person.Internal.PersonAttrToken attr = attrs[attrs.Count - 1];
                    if (attr.CanBeSinglePerson && attr.PropRef != null) 
                        return new Pullenti.Ner.ReferentToken(attr.PropRef, attr.BeginToken, attr.EndToken);
                }
                return null;
            }
            if (attrs != null && t.IsChar('(')) 
            {
                Pullenti.Ner.ReferentToken pr = TryAttachPerson(t.Next, ad, forExtOntos, step, forAttribute);
                if (pr != null && pr.EndToken.Next != null && pr.EndToken.Next.IsChar(')')) 
                {
                    Pullenti.Ner.ReferentToken res = Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pr.Referent as PersonReferent, t, pr.EndToken.Next, attrs[0].Morph, attrs, ad, true, afterBePredicate);
                    if (res != null) 
                        res.EndToken = pr.EndToken.Next;
                    return res;
                }
                Pullenti.Ner.Person.Internal.PersonAttrToken attr = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(t.Next, (ad == null ? null : ad.LocalOntology), Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                if (attr != null && attr.EndToken.Next != null && attr.EndToken.Next.IsChar(')')) 
                {
                    attrs.Add(attr);
                    t = attr.EndToken.Next.Next;
                    while (t != null && ((t.IsTableControlChar || t.IsCharOf("_:")))) 
                    {
                        t = t.Next;
                    }
                }
            }
            if (attrs != null && t != null && t.IsChar('(')) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null && (br.LengthChar < 200)) 
                    t = br.EndToken.Next;
            }
            Pullenti.Ner.Token tt0 = t0.Previous;
            if (mi.Case == Pullenti.Morph.MorphCase.AllCases && tt0 != null) 
            {
                if (tt0 != null && tt0.IsCommaAnd) 
                {
                    tt0 = tt0.Previous;
                    if (tt0 != null && (tt0.GetReferent() is PersonReferent)) 
                    {
                        if (!tt0.Morph.Case.IsUndefined) 
                            mi.Case &= tt0.Morph.Case;
                    }
                }
            }
            if ((attrs != null && t != null && t.Previous != null) && t.Previous.IsChar(',')) 
            {
                if (attrs[0].Typ != Pullenti.Ner.Person.Internal.PersonAttrTerminType.BestRegards && !attrs[0].Chars.IsLatinLetter) 
                {
                    if (attrs[0].IsNewlineBefore) 
                    {
                    }
                    else 
                        return null;
                }
            }
            if (step == 1) 
            {
            }
            if (t == null) 
                return null;
            for (int k = 0; k < 2; k++) 
            {
                List<Pullenti.Ner.Person.Internal.PersonItemToken> pits = null;
                Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr pattr = Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr.No;
                if ((step < 1) || t.InnerBool) 
                {
                    if (k == 0) 
                        pattr |= Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr.AltVar;
                    if (forExtOntos || t.Chars.IsLatinLetter) 
                        pattr |= Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr.CanBeLatin;
                    if (attrs != null) 
                        pattr |= Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr.AfterAttribute;
                    pits = Pullenti.Ner.Person.Internal.PersonItemToken.TryAttachList(t, (ad == null ? null : ad.LocalOntology), pattr, 10);
                    if (pits != null && step == 0) 
                        t.InnerBool = true;
                    if (pits != null && isGenitive) 
                    {
                        foreach (Pullenti.Ner.Person.Internal.PersonItemToken p in pits) 
                        {
                            p.RemoveNotGenitive();
                        }
                    }
                }
                if (pits == null) 
                    continue;
                if (!forExtOntos) 
                {
                }
                if ((step == 0 && pits.Count == 1 && attrs != null) && attrs[attrs.Count - 1].EndToken == t.Previous && pits[0].EndToken == t) 
                {
                    Pullenti.Ner.Core.StatisticWordInfo stat = t.Kit.Statistics.GetWordInfo(t);
                    if (stat != null) 
                        stat.HasBeforePersonAttr = true;
                    if (ad != null) 
                        ad.NeedSecondStep = true;
                }
                if (pits != null && pits.Count == 1 && pits[0].Firstname != null) 
                {
                    if (pits[0].EndToken.Next != null && pits[0].EndToken.Next.IsAnd && (pits[0].EndToken.Next.Next is Pullenti.Ner.ReferentToken)) 
                    {
                        PersonReferent pr = pits[0].EndToken.Next.Next.GetReferent() as PersonReferent;
                        if (pr != null) 
                        {
                            if (pits[0].Firstname.Vars.Count < 1) 
                                return null;
                            Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v = pits[0].Firstname.Vars[0];
                            PersonReferent pers = new PersonReferent();
                            Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo() { Gender = v.Gender, Number = Pullenti.Morph.MorphNumber.Singular, Language = pits[0].Kit.BaseLanguage };
                            bi.Class = new Pullenti.Morph.MorphClass() { IsProperSurname = true };
                            if (v.Gender == Pullenti.Morph.MorphGender.Masculine) 
                                pers.IsMale = true;
                            else if (v.Gender == Pullenti.Morph.MorphGender.Feminie) 
                                pers.IsFemale = true;
                            foreach (Pullenti.Ner.Slot s in pr.Slots) 
                            {
                                if (s.TypeName == PersonReferent.ATTR_LASTNAME) 
                                {
                                    string str = s.Value as string;
                                    string str0 = Pullenti.Morph.MorphologyService.GetWordform(str, bi);
                                    pers.AddSlot(s.TypeName, str0, false, 0);
                                    if (str0 != str) 
                                        pers.AddSlot(s.TypeName, str, false, 0);
                                }
                            }
                            if (pers.Slots.Count == 0) 
                                return null;
                            pers.AddSlot(PersonReferent.ATTR_FIRSTNAME, v.Value, false, 0);
                            return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pers, pits[0].BeginToken, pits[0].EndToken, pits[0].Firstname.Morph, attrs, ad, forAttribute, afterBePredicate);
                        }
                    }
                    Pullenti.Ner.Person.Internal.PersonAttrToken attr = (attrs != null && attrs.Count > 0 ? attrs[attrs.Count - 1] : null);
                    if ((attr != null && attr.PropRef != null && attr.PropRef.Kind == PersonPropertyKind.Kin) && attr.Gender != Pullenti.Morph.MorphGender.Undefined) 
                    {
                        object vvv = attr.PropRef.GetSlotValue(PersonPropertyReferent.ATTR_REF);
                        PersonReferent pr = vvv as PersonReferent;
                        if (vvv is Pullenti.Ner.ReferentToken) 
                            pr = (vvv as Pullenti.Ner.ReferentToken).Referent as PersonReferent;
                        if (pr != null) 
                        {
                            PersonReferent pers = new PersonReferent();
                            Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo() { Number = Pullenti.Morph.MorphNumber.Singular, Gender = attr.Gender, Language = attr.Kit.BaseLanguage };
                            bi.Class = new Pullenti.Morph.MorphClass() { IsProperSurname = true };
                            foreach (Pullenti.Ner.Slot s in pr.Slots) 
                            {
                                if (s.TypeName == PersonReferent.ATTR_LASTNAME) 
                                {
                                    string sur = s.Value as string;
                                    string sur0 = Pullenti.Morph.MorphologyService.GetWordform(sur, bi);
                                    pers.AddSlot(s.TypeName, sur0, false, 0);
                                    if (sur0 != sur) 
                                        pers.AddSlot(s.TypeName, sur, false, 0);
                                }
                            }
                            Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v = pits[0].Firstname.Vars[0];
                            pers.AddSlot(PersonReferent.ATTR_FIRSTNAME, v.Value, false, 0);
                            if (attr.Gender == Pullenti.Morph.MorphGender.Masculine) 
                                pers.IsMale = true;
                            else if (attr.Gender == Pullenti.Morph.MorphGender.Feminie) 
                                pers.IsFemale = true;
                            return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pers, pits[0].BeginToken, pits[0].EndToken, pits[0].Firstname.Morph, attrs, ad, forAttribute, afterBePredicate);
                        }
                    }
                }
                if (pits != null && pits.Count == 1 && pits[0].Lastname != null) 
                {
                    if (t.Morph.Number == Pullenti.Morph.MorphNumber.Plural || ((t.Previous != null && ((t.Previous.IsValue("БРАТ", null) || t.Previous.IsValue("СЕСТРА", null)))))) 
                    {
                        Pullenti.Ner.Token t1 = pits[0].EndToken.Next;
                        if (t1 != null && ((t1.IsChar(':') || t1.IsHiphen))) 
                            t1 = t1.Next;
                        List<Pullenti.Ner.Person.Internal.PersonItemToken> pits1 = Pullenti.Ner.Person.Internal.PersonItemToken.TryAttachList(t1, (ad == null ? null : ad.LocalOntology), pattr, 10);
                        if (pits1 != null && pits1.Count == 1) 
                            pits.AddRange(pits1);
                        else if (pits1 != null && pits1.Count == 2 && pits1[1].Middlename != null) 
                            pits.AddRange(pits1);
                    }
                }
                if (mi.Case.IsUndefined) 
                {
                    if (pits[0].IsNewlineBefore && pits[pits.Count - 1].EndToken.IsNewlineAfter) 
                        mi.Case = Pullenti.Morph.MorphCase.Nominative;
                }
                if (ad != null) 
                {
                    if (pits.Count == 1) 
                    {
                    }
                    if (forAttribute && pits.Count > 1) 
                    {
                        List<Pullenti.Ner.Person.Internal.PersonItemToken> tmp = new List<Pullenti.Ner.Person.Internal.PersonItemToken>();
                        Pullenti.Ner.Person.Internal.PersonIdentityToken pit0 = null;
                        for (int i = 0; i < pits.Count; i++) 
                        {
                            tmp.Add(pits[i]);
                            Pullenti.Ner.Person.Internal.PersonIdentityToken pit = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttachOntoInt(tmp, 0, mi, ad.LocalOntology);
                            if (pit != null) 
                                pit0 = pit;
                        }
                        if (pit0 != null) 
                            return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pit0.OntologyPerson, pit0.BeginToken, pit0.EndToken, pit0.Morph, attrs, ad, forAttribute, afterBePredicate);
                    }
                    for (int i = 0; (i < pits.Count) && (i < 3); i++) 
                    {
                        Pullenti.Ner.Person.Internal.PersonIdentityToken pit = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttachOntoInt(pits, i, mi, ad.LocalOntology);
                        if (pit != null) 
                            return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pit.OntologyPerson, pit.BeginToken, pit.EndToken, pit.Morph, (pit.BeginToken == pits[0].BeginToken ? attrs : null), ad, forAttribute, afterBePredicate);
                    }
                    if (pits.Count == 1 && !forExtOntos) 
                    {
                        PersonReferent pp = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttachOntoForSingle(pits[0], ad.LocalOntology);
                        if (pp != null) 
                            return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pp, pits[0].BeginToken, pits[0].EndToken, pits[0].Morph, attrs, ad, forAttribute, afterBePredicate);
                    }
                    if ((pits.Count == 1 && !forExtOntos && attrs != null) && pits[0].Chars.IsLatinLetter && attrs[0].Chars.IsLatinLetter) 
                    {
                        PersonReferent pp = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttachLatinSurname(pits[0], ad.LocalOntology);
                        if (pp != null) 
                            return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pp, pits[0].BeginToken, pits[0].EndToken, pits[0].Morph, attrs, ad, forAttribute, afterBePredicate);
                    }
                    if (pits.Count == 2 && !forExtOntos) 
                    {
                        PersonReferent pp = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttachOntoForDuble(pits[0], pits[1], ad.LocalOntology);
                        if (pp != null) 
                            return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pp, pits[0].BeginToken, pits[1].EndToken, pits[0].Morph, attrs, ad, forAttribute, afterBePredicate);
                    }
                }
                if (pits[0].BeginToken.Kit.Ontology != null) 
                {
                    for (int i = 0; i < pits.Count; i++) 
                    {
                        Pullenti.Ner.Person.Internal.PersonIdentityToken pit = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttachOntoExt(pits, i, mi, pits[0].BeginToken.Kit.Ontology);
                        if (pit != null) 
                            return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pit.OntologyPerson, pit.BeginToken, pit.EndToken, pit.Morph, attrs, ad, forAttribute, afterBePredicate);
                    }
                }
                List<Pullenti.Ner.Person.Internal.PersonIdentityToken> pli0 = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttach(pits, 0, mi, t0, isKing, attrs != null);
                if (pli0.Count > 0 && pli0[0].Typ == Pullenti.Ner.Person.Internal.FioTemplateType.NameSurname) 
                {
                    if ((attrs != null && attrs.Count > 0 && attrs[attrs.Count - 1].BeginToken == attrs[attrs.Count - 1].EndToken) && attrs[attrs.Count - 1].BeginToken.Chars.IsCapitalUpper) 
                    {
                        List<Pullenti.Ner.Person.Internal.PersonItemToken> pits1 = Pullenti.Ner.Person.Internal.PersonItemToken.TryAttachList(attrs[attrs.Count - 1].BeginToken, (ad == null ? null : ad.LocalOntology), pattr, 10);
                        if (pits1 != null && pits1[0].Lastname != null) 
                        {
                            List<Pullenti.Ner.Person.Internal.PersonIdentityToken> pli11 = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttach(pits1, 0, mi, t0, isKing, attrs.Count > 1);
                            if ((pli11 != null && pli11.Count > 0 && pli11[0].Coef > 1) && pli11[0].EndToken == pli0[0].EndToken) 
                            {
                                pli0 = pli11;
                                attrs.RemoveAt(attrs.Count - 1);
                                if (attrs.Count == 0) 
                                    attrs = null;
                            }
                        }
                    }
                }
                if (t.Previous == null && ((ad != null && ad.TextStartsWithLastnameFirstnameMiddlename)) && pits.Count == 3) 
                {
                    bool exi = false;
                    foreach (Pullenti.Ner.Person.Internal.PersonIdentityToken pit in pli0) 
                    {
                        if (pit.Typ == Pullenti.Ner.Person.Internal.FioTemplateType.SurnameNameSecname) 
                        {
                            pit.Coef += 10;
                            exi = true;
                        }
                    }
                    if (!exi) 
                    {
                        Pullenti.Ner.Person.Internal.PersonIdentityToken pit = Pullenti.Ner.Person.Internal.PersonIdentityToken.CreateTyp(pits, Pullenti.Ner.Person.Internal.FioTemplateType.SurnameNameSecname, mi);
                        if (pit != null) 
                        {
                            pit.Coef = 10;
                            pli0.Add(pit);
                        }
                    }
                }
                if (forExtOntos) 
                {
                    bool te = false;
                    if (pli0 == null || pli0.Count == 0) 
                        te = true;
                    else 
                    {
                        Pullenti.Ner.Person.Internal.PersonIdentityToken.Sort(pli0);
                        if (pli0[0].Coef < 2) 
                            te = true;
                    }
                    if (te) 
                        pli0 = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttachForExtOnto(pits);
                }
                if (forExtOntos && pli0 != null) 
                {
                    Pullenti.Ner.Token et = pits[pits.Count - 1].EndToken;
                    foreach (Pullenti.Ner.Person.Internal.PersonIdentityToken pit in pli0) 
                    {
                        if (pit.EndToken == et) 
                            pit.Coef += 1;
                    }
                }
                List<Pullenti.Ner.Person.Internal.PersonIdentityToken> pli = pli0;
                List<Pullenti.Ner.Person.Internal.PersonIdentityToken> pli1 = null;
                if (!forExtOntos && ((attrs == null || attrs[attrs.Count - 1].Typ == Pullenti.Ner.Person.Internal.PersonAttrTerminType.Position))) 
                {
                    if ((pits.Count == 4 && pits[0].Firstname != null && pits[1].Firstname == null) && pits[2].Firstname != null && pits[3].Firstname == null) 
                    {
                    }
                    else 
                    {
                        pli1 = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttach(pits, 1, mi, t0, isKing, attrs != null);
                        if (pli0 != null && pli1 != null && pli1.Count > 0) 
                            Pullenti.Ner.Person.Internal.PersonIdentityToken.CorrectXFML(pli0, pli1, attrs);
                    }
                }
                if (pli == null) 
                    pli = pli1;
                else if (pli1 != null) 
                    pli.AddRange(pli1);
                if (((pli == null || pli.Count == 0)) && pits.Count == 1 && pits[0].Firstname != null) 
                {
                    if (isKing) 
                    {
                        Pullenti.Ner.Person.Internal.PersonIdentityToken first = new Pullenti.Ner.Person.Internal.PersonIdentityToken(pits[0].BeginToken, pits[0].EndToken);
                        Pullenti.Ner.Person.Internal.PersonIdentityToken.ManageFirstname(first, pits[0], mi);
                        first.Coef = 2;
                        if (first.Morph.Gender == Pullenti.Morph.MorphGender.Undefined && first.Firstname != null) 
                            first.Morph.Gender = first.Firstname.Gender;
                        pli.Add(first);
                        Pullenti.Ner.Person.Internal.PersonItemToken sur = ((attrs == null || attrs.Count == 0) ? null : attrs[attrs.Count - 1].KingSurname);
                        if (sur != null) 
                            Pullenti.Ner.Person.Internal.PersonIdentityToken.ManageLastname(first, sur, mi);
                    }
                    else if (attrs != null) 
                    {
                        foreach (Pullenti.Ner.Person.Internal.PersonAttrToken a in attrs) 
                        {
                            if (a.CanBeSameSurname && a.Referent != null) 
                            {
                                PersonReferent pr0 = a.Referent.GetSlotValue(PersonPropertyReferent.ATTR_REF) as PersonReferent;
                                if (pr0 != null) 
                                {
                                    Pullenti.Ner.Person.Internal.PersonIdentityToken first = new Pullenti.Ner.Person.Internal.PersonIdentityToken(pits[0].BeginToken, pits[0].EndToken);
                                    Pullenti.Ner.Person.Internal.PersonIdentityToken.ManageFirstname(first, pits[0], mi);
                                    first.Coef = 2;
                                    pli.Add(first);
                                    first.Lastname = new Pullenti.Ner.Person.Internal.PersonMorphCollection();
                                    foreach (Pullenti.Ner.Slot v in pr0.Slots) 
                                    {
                                        if (v.TypeName == PersonReferent.ATTR_LASTNAME) 
                                            first.Lastname.Add((string)v.Value, null, (pr0.IsMale ? Pullenti.Morph.MorphGender.Masculine : (pr0.IsFemale ? Pullenti.Morph.MorphGender.Feminie : Pullenti.Morph.MorphGender.Undefined)), true);
                                    }
                                }
                            }
                        }
                    }
                }
                if ((((pli == null || pli.Count == 0)) && pits.Count == 1 && pits[0].Lastname != null) && attrs != null && !pits[0].IsInDictionary) 
                {
                    foreach (Pullenti.Ner.Person.Internal.PersonAttrToken a in attrs) 
                    {
                        if (a.PropRef != null && ((a.Typ == Pullenti.Ner.Person.Internal.PersonAttrTerminType.Prefix || a.PropRef.Kind == PersonPropertyKind.Boss))) 
                        {
                            Pullenti.Ner.Person.Internal.PersonIdentityToken last = new Pullenti.Ner.Person.Internal.PersonIdentityToken(pits[0].BeginToken, pits[0].EndToken);
                            Pullenti.Ner.Person.Internal.PersonIdentityToken.ManageLastname(last, pits[0], mi);
                            last.Coef = 2;
                            pli.Add(last);
                            break;
                        }
                    }
                }
                if (pli != null && pli.Count > 0) 
                {
                    Pullenti.Ner.Person.Internal.PersonIdentityToken.Sort(pli);
                    Pullenti.Ner.Person.Internal.PersonIdentityToken best = pli[0];
                    float minCoef = (float)2;
                    if ((best.Coef < minCoef) && ((attrs != null || forExtOntos))) 
                    {
                        Pullenti.Ner.Person.Internal.PersonIdentityToken pit = Pullenti.Ner.Person.Internal.PersonIdentityToken.TryAttachIdentity(pits, mi);
                        if (pit != null && pit.Coef > best.Coef && pit.Coef > 0) 
                        {
                            PersonReferent pers = new PersonReferent();
                            pers.AddIdentity(pit.Lastname);
                            return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pers, pit.BeginToken, pit.EndToken, pit.Morph, attrs, ad, forAttribute, afterBePredicate);
                        }
                        if ((best.Kit.BaseLanguage.IsEn && best.Typ == Pullenti.Ner.Person.Internal.FioTemplateType.NameSurname && attrs != null) && attrs[0].Typ == Pullenti.Ner.Person.Internal.PersonAttrTerminType.BestRegards) 
                            best.Coef += 10;
                        if (best.Coef >= 0) 
                            best.Coef += (best.Chars.IsAllUpper ? 1 : 2);
                    }
                    if (best.Coef >= 0 && (best.Coef < minCoef)) 
                    {
                        Pullenti.Ner.Token tee = best.EndToken.Next;
                        Pullenti.Ner.Token tee1 = null;
                        if (tee != null && tee.IsChar('(')) 
                        {
                            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tee, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br != null && (br.LengthChar < 100)) 
                            {
                                tee1 = br.BeginToken.Next;
                                tee = br.EndToken.Next;
                            }
                        }
                        if (tee is Pullenti.Ner.TextToken) 
                        {
                            if (tee.IsCharOf(":,") || tee.IsHiphen || (tee as Pullenti.Ner.TextToken).IsVerbBe) 
                                tee = tee.Next;
                        }
                        Pullenti.Ner.Person.Internal.PersonAttrToken att = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(tee, (ad == null ? null : ad.LocalOntology), Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                        if (att == null && tee1 != null) 
                            att = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(tee1, (ad == null ? null : ad.LocalOntology), Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                        if (att != null) 
                        {
                            if (tee == best.EndToken.Next && !att.Morph.Case.IsNominative && !att.Morph.Case.IsUndefined) 
                            {
                            }
                            else 
                                best.Coef += 2;
                        }
                        else if (tee != null && tee.IsValue("АГЕНТ", null)) 
                            best.Coef += 1;
                        if (forAttribute) 
                            best.Coef += 1;
                    }
                    if (best.Coef >= minCoef) 
                    {
                        int i;
                        Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined;
                        for (i = 0; i < pli.Count; i++) 
                        {
                            if (pli[i].Coef != best.Coef) 
                            {
                                pli.RemoveRange(i, pli.Count - i);
                                break;
                            }
                            else if (pli[i].ProbableGender != Pullenti.Morph.MorphGender.Undefined) 
                                gender |= pli[i].ProbableGender;
                        }
                        if (pli.Count > 1) 
                            return null;
                        if (gender != Pullenti.Morph.MorphGender.Feminie && gender != Pullenti.Morph.MorphGender.Masculine) 
                        {
                            if ((pli[0].IsNewlineBefore && pli[0].IsNewlineAfter && pli[0].Lastname != null) && pli[0].Lastname.HasLastnameStandardTail) 
                            {
                                if (pli[0].Lastname.Values.Count == 2) 
                                {
                                    bool ok = true;
                                    int cou = 100;
                                    string sur = pli[0].Lastname.Items[0].Value;
                                    for (Pullenti.Ner.Token ttt = pli[0].EndToken.Next; ttt != null && cou > 0; ttt = ttt.Next,cou--) 
                                    {
                                        if (step > 0) 
                                            break;
                                        if (!ttt.IsValue(sur, null)) 
                                            continue;
                                        Pullenti.Ner.ReferentToken pr = TryAttachPerson(ttt, ad, forExtOntos, step, false);
                                        if (pr != null && !(pr.Referent as PersonReferent).IsFemale) 
                                            ok = false;
                                        break;
                                    }
                                    if (ok) 
                                    {
                                        pli[0].Lastname.Remove(null, Pullenti.Morph.MorphGender.Masculine);
                                        gender = Pullenti.Morph.MorphGender.Feminie;
                                        if (pli[0].Firstname != null && pli[0].Firstname.Values.Count == 2) 
                                            pli[0].Firstname.Remove(null, Pullenti.Morph.MorphGender.Masculine);
                                    }
                                }
                            }
                        }
                        if (gender == Pullenti.Morph.MorphGender.Undefined) 
                        {
                            if (pli[0].Firstname != null && pli[0].Lastname != null) 
                            {
                                Pullenti.Morph.MorphGender g = pli[0].Firstname.Gender;
                                if (pli[0].Lastname.Gender != Pullenti.Morph.MorphGender.Undefined) 
                                    g &= pli[0].Lastname.Gender;
                                if (g == Pullenti.Morph.MorphGender.Feminie || g == Pullenti.Morph.MorphGender.Masculine) 
                                    gender = g;
                                else if (pli[0].Firstname.Gender == Pullenti.Morph.MorphGender.Masculine || pli[0].Firstname.Gender == Pullenti.Morph.MorphGender.Feminie) 
                                    gender = pli[0].Firstname.Gender;
                                else if (pli[0].Lastname.Gender == Pullenti.Morph.MorphGender.Masculine || pli[0].Lastname.Gender == Pullenti.Morph.MorphGender.Feminie) 
                                    gender = pli[0].Lastname.Gender;
                            }
                        }
                        PersonReferent pers = new PersonReferent();
                        if (gender == Pullenti.Morph.MorphGender.Masculine) 
                            pers.IsMale = true;
                        else if (gender == Pullenti.Morph.MorphGender.Feminie) 
                            pers.IsFemale = true;
                        foreach (Pullenti.Ner.Person.Internal.PersonIdentityToken v in pli) 
                        {
                            if (v.OntologyPerson != null) 
                            {
                                foreach (Pullenti.Ner.Slot s in v.OntologyPerson.Slots) 
                                {
                                    pers.AddSlot(s.TypeName, s.Value, false, 0);
                                }
                            }
                            else if (v.Typ == Pullenti.Ner.Person.Internal.FioTemplateType.AsianName) 
                                pers.AddIdentity(v.Lastname);
                            else 
                            {
                                pers.AddFioIdentity(v.Lastname, v.Firstname, v.Middlename);
                                if (v.Typ == Pullenti.Ner.Person.Internal.FioTemplateType.AsianSurnameName) 
                                    pers.AddSlot("NAMETYPE", "china", false, 0);
                            }
                        }
                        if (!forExtOntos) 
                            pers.m_PersonIdentityTyp = pli[0].Typ;
                        if (pli[0].BeginToken != pits[0].BeginToken && attrs != null) 
                        {
                            if (pits[0].WhitespacesBeforeCount > 2) 
                                attrs = null;
                            else 
                            {
                                string s = pits[0].GetSourceText();
                                Pullenti.Ner.Person.Internal.PersonAttrToken pat = attrs[attrs.Count - 1];
                                if (pat.Typ == Pullenti.Ner.Person.Internal.PersonAttrTerminType.Position && !string.IsNullOrEmpty(s) && !pat.IsNewlineBefore) 
                                {
                                    if (pat.Value == null && pat.PropRef != null) 
                                    {
                                        for (; pat != null; pat = pat.HigherPropRef) 
                                        {
                                            if (pat.PropRef == null) 
                                                break;
                                            else if (pat.HigherPropRef == null) 
                                            {
                                                string str = s.ToLower();
                                                if (pat.PropRef.Name != null && !Pullenti.Morph.LanguageHelper.EndsWith(pat.PropRef.Name, str)) 
                                                    pat.PropRef.Name += (" " + str);
                                                if (pat.AddOuterOrgAsRef) 
                                                {
                                                    pat.PropRef.AddSlot(PersonPropertyReferent.ATTR_REF, null, true, 0);
                                                    pat.AddOuterOrgAsRef = false;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    else if (pat.Value != null) 
                                        pat.Value = string.Format("{0} {1}", pat.Value, s.ToLower());
                                    pat.EndToken = pits[0].EndToken;
                                }
                            }
                        }
                        Pullenti.Ner.Person.Internal.PersonIdentityToken latin = Pullenti.Ner.Person.Internal.PersonIdentityToken.CheckLatinAfter(pli[0]);
                        if (latin != null) 
                            pers.AddFioIdentity(latin.Lastname, latin.Firstname, latin.Middlename);
                        return Pullenti.Ner.Person.Internal.PersonHelper.CreateReferentToken(pers, pli[0].BeginToken, (latin != null ? latin.EndToken : pli[0].EndToken), pli[0].Morph, attrs, ad, forAttribute, afterBePredicate);
                    }
                }
            }
            if (attrs != null) 
            {
                Pullenti.Ner.Person.Internal.PersonAttrToken attr = attrs[attrs.Count - 1];
                if (attr.CanBeSinglePerson && attr.PropRef != null) 
                    return new Pullenti.Ner.ReferentToken(attr.PropRef, attr.BeginToken, attr.EndToken) { Morph = attr.Morph };
            }
            return null;
        }
        public override Pullenti.Ner.ReferentToken ProcessOntologyItem(Pullenti.Ner.Token begin)
        {
            if (begin == null) 
                return null;
            Pullenti.Ner.ReferentToken rt = TryAttachPerson(begin, null, true, -1, false);
            if (rt == null) 
            {
                Pullenti.Ner.Person.Internal.PersonAttrToken pat = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(begin, null, Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                if (pat != null && pat.PropRef != null) 
                    return new Pullenti.Ner.ReferentToken(pat.PropRef, pat.BeginToken, pat.EndToken);
                return null;
            }
            Pullenti.Ner.Token t = rt.EndToken.Next;
            for (; t != null; t = t.Next) 
            {
                if (t.IsChar(';') && t.Next != null) 
                {
                    Pullenti.Ner.ReferentToken rt1 = TryAttachPerson(t.Next, null, true, -1, false);
                    if (rt1 != null && rt1.Referent.TypeName == rt.Referent.TypeName) 
                    {
                        rt.Referent.MergeSlots(rt1.Referent, true);
                        t = (rt.EndToken = rt1.EndToken);
                    }
                    else if (rt1 != null) 
                        t = rt1.EndToken;
                }
            }
            return rt;
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            try 
            {
                Pullenti.Ner.Person.Internal.MetaPerson.Initialize();
                Pullenti.Ner.Person.Internal.MetaPersonIdentity.Initialize();
                Pullenti.Ner.Person.Internal.MetaPersonProperty.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Person.Internal.PersonItemToken.Initialize();
                Pullenti.Ner.Person.Internal.PersonAttrToken.Initialize();
                Pullenti.Ner.Person.Internal.ShortNameHelper.Initialize();
                Pullenti.Ner.Person.Internal.PersonIdToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
                Pullenti.Ner.Mail.Internal.MailLine.Initialize();
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new PersonAnalyzer());
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new Pullenti.Ner.Person.Internal.PersonPropAnalyzer());
        }
    }
}