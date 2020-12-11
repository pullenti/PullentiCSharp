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

namespace Pullenti.Ner.Org
{
    /// <summary>
    /// Анализатор организаций
    /// </summary>
    public class OrganizationAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("ORGANIZATION")
        /// </summary>
        public const string ANALYZER_NAME = "ORGANIZATION";
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new OrganizationAnalyzer();
        }
        public override string Caption
        {
            get
            {
                return "Организации";
            }
        }
        public override string Description
        {
            get
            {
                return "Организации, предприятия, компании...";
            }
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Org.Internal.MetaOrganization.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(OrgProfile.Unit.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("dep.png"));
                res.Add(OrgProfile.Union.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("party.png"));
                res.Add(OrgProfile.Competition.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("festival.png"));
                res.Add(OrgProfile.Holding.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("holding.png"));
                res.Add(OrgProfile.State.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("gov.png"));
                res.Add(OrgProfile.Finance.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("bank.png"));
                res.Add(OrgProfile.Education.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("study.png"));
                res.Add(OrgProfile.Science.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("science.png"));
                res.Add(OrgProfile.Industry.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("factory.png"));
                res.Add(OrgProfile.Trade.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("trade.png"));
                res.Add(OrgProfile.Policy.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("politics.png"));
                res.Add(OrgProfile.Justice.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("justice.png"));
                res.Add(OrgProfile.Enforcement.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("gov.png"));
                res.Add(OrgProfile.Army.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("military.png"));
                res.Add(OrgProfile.Sport.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("sport.png"));
                res.Add(OrgProfile.Religion.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("church.png"));
                res.Add(OrgProfile.Music.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("music.png"));
                res.Add(OrgProfile.Media.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("media.png"));
                res.Add(OrgProfile.Press.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("press.png"));
                res.Add(OrgProfile.Hotel.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("hotel.png"));
                res.Add(OrgProfile.Medicine.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("medicine.png"));
                res.Add(OrgProfile.Transport.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("train.png"));
                res.Add(OrganizationKind.Bank.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("bank.png"));
                res.Add(OrganizationKind.Culture.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("culture.png"));
                res.Add(OrganizationKind.Department.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("dep.png"));
                res.Add(OrganizationKind.Factory.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("factory.png"));
                res.Add(OrganizationKind.Govenment.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("gov.png"));
                res.Add(OrganizationKind.Medical.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("medicine.png"));
                res.Add(OrganizationKind.Party.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("party.png"));
                res.Add(OrganizationKind.Study.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("study.png"));
                res.Add(OrganizationKind.Federation.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("federation.png"));
                res.Add(OrganizationKind.Church.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("church.png"));
                res.Add(OrganizationKind.Military.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("military.png"));
                res.Add(OrganizationKind.Airport.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("avia.png"));
                res.Add(OrganizationKind.Festival.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("festival.png"));
                res.Add(Pullenti.Ner.Org.Internal.MetaOrganization.OrgImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("org.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == OrganizationReferent.OBJ_TYPENAME) 
                return new OrganizationReferent();
            return null;
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {Pullenti.Ner.Geo.GeoReferent.OBJ_TYPENAME, Pullenti.Ner.Address.AddressReferent.OBJ_TYPENAME};
            }
        }
        public override int ProgressWeight
        {
            get
            {
                return 45;
            }
        }
        public class OrgAnalyzerData : Pullenti.Ner.Core.AnalyzerDataWithOntology
        {
            public override Pullenti.Ner.Referent RegisterReferent(Pullenti.Ner.Referent referent)
            {
                if (referent is Pullenti.Ner.Org.OrganizationReferent) 
                    (referent as Pullenti.Ner.Org.OrganizationReferent).FinalCorrection();
                int slots = referent.Slots.Count;
                Pullenti.Ner.Referent res = base.RegisterReferent(referent);
                if (!LargeTextRegim && (res is Pullenti.Ner.Org.OrganizationReferent) && ((res == referent || res.Slots.Count != slots))) 
                {
                    Pullenti.Ner.Core.IntOntologyItem ioi = (res as Pullenti.Ner.Org.OrganizationReferent).CreateOntologyItemEx(2, true, false);
                    if (ioi != null) 
                        LocOrgs.AddItem(ioi);
                    List<string> names = (res as Pullenti.Ner.Org.OrganizationReferent)._getPureNames();
                    if (names != null) 
                    {
                        foreach (string n in names) 
                        {
                            OrgPureNames.Add(new Pullenti.Ner.Core.Termin(n));
                        }
                    }
                }
                return res;
            }
            public Pullenti.Ner.Core.IntOntologyCollection LocOrgs = new Pullenti.Ner.Core.IntOntologyCollection();
            public Pullenti.Ner.Core.TerminCollection OrgPureNames = new Pullenti.Ner.Core.TerminCollection();
            public Pullenti.Ner.Core.TerminCollection Aliases = new Pullenti.Ner.Core.TerminCollection();
            public bool LargeTextRegim = false;
        }

        public override Pullenti.Ner.Core.AnalyzerData CreateAnalyzerData()
        {
            return new OrgAnalyzerData();
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            OrgAnalyzerData ad = kit.GetAnalyzerData(this) as OrgAnalyzerData;
            if (kit.Sofa.Text.Length > 400000) 
                ad.LargeTextRegim = true;
            else 
                ad.LargeTextRegim = false;
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
                    if (step > 0 && (t is Pullenti.Ner.ReferentToken) && (t.GetReferent() is OrganizationReferent)) 
                    {
                        Pullenti.Ner.MetaToken mt = _checkAliasAfter(t as Pullenti.Ner.ReferentToken, t.Next);
                        if (mt != null) 
                        {
                            if (ad != null) 
                            {
                                Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                                term.InitBy(mt.BeginToken, mt.EndToken.Previous, t.GetReferent(), false);
                                ad.Aliases.Add(term);
                            }
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(t.GetReferent(), t, mt.EndToken);
                            kit.EmbedToken(rt);
                            t = rt;
                        }
                    }
                    while (true) 
                    {
                        List<Pullenti.Ner.ReferentToken> rts = this.TryAttachOrgs(t, ad, step);
                        if (rts == null || rts.Count == 0) 
                            break;
                        if (!Pullenti.Ner.MetaToken.Check(rts)) 
                            break;
                        bool emb = false;
                        foreach (Pullenti.Ner.ReferentToken rt in rts) 
                        {
                            if (!(rt.Referent as OrganizationReferent).CheckCorrection()) 
                                continue;
                            rt.Referent = ad.RegisterReferent(rt.Referent);
                            if (rt.BeginToken.GetReferent() == rt.Referent || rt.EndToken.GetReferent() == rt.Referent) 
                                continue;
                            kit.EmbedToken(rt);
                            emb = true;
                            if (rt.BeginChar <= t.BeginChar) 
                                t = rt;
                        }
                        if ((rts.Count == 1 && t == rts[0] && (t.Next is Pullenti.Ner.ReferentToken)) && (t.Next.GetReferent() is OrganizationReferent)) 
                        {
                            OrganizationReferent org0 = rts[0].Referent as OrganizationReferent;
                            OrganizationReferent org1 = t.Next.GetReferent() as OrganizationReferent;
                            if (org1.Higher == null && Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org0, org1, false) && !Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org1, org0, false)) 
                            {
                                Pullenti.Ner.ReferentToken rtt = t.Next as Pullenti.Ner.ReferentToken;
                                kit.DebedToken(rtt);
                                org1.Higher = org0;
                                Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(org1), t, rtt.EndToken) { Morph = t.Next.Morph };
                                kit.EmbedToken(rt1);
                                t = rt1;
                            }
                        }
                        if (emb && !(t is Pullenti.Ner.ReferentToken)) 
                            continue;
                        break;
                    }
                    if (step > 0) 
                    {
                        Pullenti.Ner.ReferentToken rt = this.CheckOwnership(t);
                        if (rt != null) 
                        {
                            kit.EmbedToken(rt);
                            t = rt;
                        }
                    }
                    if ((t is Pullenti.Ner.ReferentToken) && (t.GetReferent() is OrganizationReferent)) 
                    {
                        Pullenti.Ner.ReferentToken rt0 = t as Pullenti.Ner.ReferentToken;
                        while (rt0 != null) 
                        {
                            rt0 = this.TryAttachOrgBefore(rt0, ad);
                            if (rt0 == null) 
                                break;
                            this._doPostAnalyze(rt0, ad);
                            rt0.Referent = ad.RegisterReferent(rt0.Referent);
                            kit.EmbedToken(rt0);
                            t = rt0;
                        }
                    }
                    if (step > 0 && (t is Pullenti.Ner.ReferentToken) && (t.GetReferent() is OrganizationReferent)) 
                    {
                        Pullenti.Ner.MetaToken mt = _checkAliasAfter(t as Pullenti.Ner.ReferentToken, t.Next);
                        if (mt != null) 
                        {
                            if (ad != null) 
                            {
                                Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                                term.InitBy(mt.BeginToken, mt.EndToken.Previous, t.GetReferent(), false);
                                ad.Aliases.Add(term);
                            }
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(t.GetReferent(), t, mt.EndToken);
                            kit.EmbedToken(rt);
                            t = rt;
                        }
                    }
                }
                if (ad.Referents.Count == 0) 
                {
                    if (!kit.MiscData.ContainsKey("o2step")) 
                        break;
                }
            }
            List<Pullenti.Ner.ReferentToken> list = new List<Pullenti.Ner.ReferentToken>();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                OrganizationReferent org = t.GetReferent() as OrganizationReferent;
                if (org == null) 
                    continue;
                Pullenti.Ner.Token t1 = t.Next;
                if (((t1 != null && t1.IsChar('(') && t1.Next != null) && (t1.Next.GetReferent() is OrganizationReferent) && t1.Next.Next != null) && t1.Next.Next.IsChar(')')) 
                {
                    OrganizationReferent org0 = t1.Next.GetReferent() as OrganizationReferent;
                    if (org0 == org || org.Higher == org0) 
                    {
                        Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(org, t, t1.Next.Next) { Morph = t.Morph };
                        kit.EmbedToken(rt1);
                        t = rt1;
                        t1 = t.Next;
                    }
                    else if (org.Higher == null && Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org0, org, false) && !Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org, org0, false)) 
                    {
                        org.Higher = org0;
                        Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(org, t, t1.Next.Next) { Morph = t.Morph };
                        kit.EmbedToken(rt1);
                        t = rt1;
                        t1 = t.Next;
                    }
                }
                Pullenti.Ner.TextToken ofTok = null;
                if (t1 != null) 
                {
                    if (t1.IsCharOf(",") || t1.IsHiphen) 
                        t1 = t1.Next;
                    else if (!kit.OntoRegime && t1.IsChar(';')) 
                        t1 = t1.Next;
                    else if (t1.IsValue("ПРИ", null) || t1.IsValue("OF", null) || t1.IsValue("AT", null)) 
                    {
                        ofTok = t1 as Pullenti.Ner.TextToken;
                        t1 = t1.Next;
                    }
                }
                if (t1 == null) 
                    break;
                OrganizationReferent org1 = t1.GetReferent() as OrganizationReferent;
                if (org1 == null) 
                    continue;
                if (ofTok == null) 
                {
                    if (org.Higher == null) 
                    {
                        if (!Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org1, org, false)) 
                        {
                            if (t1.Previous != t || t1.WhitespacesAfterCount > 2) 
                                continue;
                            Pullenti.Ner.ReferentToken pp = t.Kit.ProcessReferent("PERSON", t1.Next);
                            if (pp != null) 
                            {
                            }
                            else 
                                continue;
                        }
                    }
                }
                if (org.Higher != null) 
                {
                    if (!org.Higher.CanBeEquals(org1, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                        continue;
                }
                list.Clear();
                list.Add(t as Pullenti.Ner.ReferentToken);
                list.Add(t1 as Pullenti.Ner.ReferentToken);
                if (ofTok != null && org.Higher == null) 
                {
                    for (Pullenti.Ner.Token t2 = t1.Next; t2 != null; t2 = t2.Next) 
                    {
                        if (((t2 is Pullenti.Ner.TextToken) && (t2 as Pullenti.Ner.TextToken).Term == ofTok.Term && t2.Next != null) && (t2.Next.GetReferent() is OrganizationReferent)) 
                        {
                            t2 = t2.Next;
                            if (org1.Higher != null) 
                            {
                                if (!org1.Higher.CanBeEquals(t2.GetReferent(), Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                    break;
                            }
                            list.Add(t2 as Pullenti.Ner.ReferentToken);
                            org1 = t2.GetReferent() as OrganizationReferent;
                        }
                        else 
                            break;
                    }
                }
                Pullenti.Ner.ReferentToken rt0 = list[list.Count - 1];
                for (int i = list.Count - 2; i >= 0; i--) 
                {
                    org = list[i].Referent as OrganizationReferent;
                    org1 = rt0.Referent as OrganizationReferent;
                    if (org.Higher == null) 
                    {
                        org.Higher = org1;
                        org = ad.RegisterReferent(org) as OrganizationReferent;
                    }
                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(org, list[i], rt0);
                    kit.EmbedToken(rt);
                    t = rt;
                    rt0 = rt;
                }
            }
            Dictionary<string, List<OrganizationReferent>> owners = new Dictionary<string, List<OrganizationReferent>>();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                OrganizationReferent org = t.GetReferent() as OrganizationReferent;
                if (org == null) 
                    continue;
                OrganizationReferent hi = org.Higher;
                if (hi == null) 
                    continue;
                foreach (string ty in org.Types) 
                {
                    List<OrganizationReferent> li;
                    if (!owners.TryGetValue(ty, out li)) 
                        owners.Add(ty, (li = new List<OrganizationReferent>()));
                    List<OrganizationReferent> childs = null;
                    if (!li.Contains(hi)) 
                    {
                        li.Add(hi);
                        hi.Tag = (childs = new List<OrganizationReferent>());
                    }
                    else 
                        childs = hi.Tag as List<OrganizationReferent>;
                    if (childs != null && !childs.Contains(org)) 
                        childs.Add(org);
                }
            }
            List<OrganizationReferent> owns = new List<OrganizationReferent>();
            Pullenti.Ner.Token lastMvdOrg = null;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                OrganizationReferent org = t.GetReferent() as OrganizationReferent;
                if (org == null) 
                    continue;
                if (_isMvdOrg(org) != null) 
                    lastMvdOrg = t;
                if (org.Higher != null) 
                    continue;
                owns.Clear();
                foreach (string ty in org.Types) 
                {
                    List<OrganizationReferent> li;
                    if (!owners.TryGetValue(ty, out li)) 
                        continue;
                    foreach (OrganizationReferent h in li) 
                    {
                        if (!owns.Contains(h)) 
                            owns.Add(h);
                    }
                }
                if (owns.Count != 1) 
                    continue;
                if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(owns[0], org, true)) 
                {
                    List<OrganizationReferent> childs = owns[0].Tag as List<OrganizationReferent>;
                    if (childs == null) 
                        continue;
                    bool hasNum = false;
                    bool hasGeo = false;
                    foreach (OrganizationReferent oo in childs) 
                    {
                        if (oo.FindSlot(OrganizationReferent.ATTR_GEO, null, true) != null) 
                            hasGeo = true;
                        if (oo.FindSlot(OrganizationReferent.ATTR_NUMBER, null, true) != null) 
                            hasNum = true;
                    }
                    if (hasNum != (org.FindSlot(OrganizationReferent.ATTR_NUMBER, null, true) != null)) 
                        continue;
                    if (hasGeo != (org.FindSlot(OrganizationReferent.ATTR_GEO, null, true) != null)) 
                        continue;
                    org.Higher = owns[0];
                    if (org.Kind != OrganizationKind.Department) 
                        org.Higher = null;
                }
            }
            for (Pullenti.Ner.Token t = lastMvdOrg; t != null; t = t.Previous) 
            {
                if (!(t is Pullenti.Ner.ReferentToken)) 
                    continue;
                OrganizationReferent mvd = _isMvdOrg(t.GetReferent() as OrganizationReferent);
                if (mvd == null) 
                    continue;
                Pullenti.Ner.Token t1 = null;
                bool br = false;
                for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
                {
                    if (tt.IsChar(')')) 
                    {
                        br = true;
                        continue;
                    }
                    if (br) 
                    {
                        if (tt.IsChar('(')) 
                            br = false;
                        continue;
                    }
                    if (!(tt is Pullenti.Ner.TextToken)) 
                        break;
                    if (tt.LengthChar < 2) 
                        continue;
                    if (tt.Chars.IsAllUpper || ((!tt.Chars.IsAllUpper && !tt.Chars.IsAllLower && !tt.Chars.IsCapitalUpper))) 
                        t1 = tt;
                    break;
                }
                if (t1 == null) 
                    continue;
                Pullenti.Ner.Token t0 = t1;
                if ((t0.Previous is Pullenti.Ner.TextToken) && (t0.WhitespacesBeforeCount < 2) && t0.Previous.LengthChar >= 2) 
                {
                    if (t0.Previous.Chars.IsAllUpper || ((!t0.Previous.Chars.IsAllUpper && !t0.Previous.Chars.IsAllLower && !t0.Previous.Chars.IsCapitalUpper))) 
                        t0 = t0.Previous;
                }
                string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t0, t1, Pullenti.Ner.Core.GetTextAttr.No);
                if ((nam == "ОВД" || nam == "ГУВД" || nam == "УВД") || nam == "ГУ") 
                    continue;
                Pullenti.Morph.MorphClass mc = t0.GetMorphClassInDictionary();
                if (!mc.IsUndefined) 
                    continue;
                mc = t1.GetMorphClassInDictionary();
                if (!mc.IsUndefined) 
                    continue;
                OrganizationReferent org = new OrganizationReferent();
                org.AddProfile(OrgProfile.Unit);
                org.AddName(nam, true, null);
                org.Higher = mvd;
                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(org), t0, t1);
                kit.EmbedToken(rt);
                t = rt.Next;
                if (t == null) 
                    break;
            }
        }
        static OrganizationReferent _isMvdOrg(OrganizationReferent org)
        {
            if (org == null) 
                return null;
            OrganizationReferent res = null;
            for (int i = 0; i < 5; i++) 
            {
                if (res == null) 
                {
                    foreach (Pullenti.Ner.Slot s in org.Slots) 
                    {
                        if (s.TypeName == OrganizationReferent.ATTR_TYPE) 
                        {
                            res = org;
                            break;
                        }
                    }
                }
                if (org.FindSlot(OrganizationReferent.ATTR_NAME, "МВД", true) != null || org.FindSlot(OrganizationReferent.ATTR_NAME, "ФСБ", true) != null) 
                    return res ?? org;
                org = org.Higher;
                if (org == null) 
                    break;
            }
            return null;
        }
        internal static Pullenti.Ner.MetaToken _checkAliasAfter(Pullenti.Ner.ReferentToken rt, Pullenti.Ner.Token t)
        {
            if ((t != null && t.IsChar('<') && t.Next != null) && t.Next.Next != null && t.Next.Next.IsChar('>')) 
                t = t.Next.Next.Next;
            if (t == null || t.Next == null || !t.IsChar('(')) 
                return null;
            t = t.Next;
            if (t.IsValue("ДАЛЕЕ", null) || t.IsValue("ДАЛІ", null)) 
                t = t.Next;
            else if (t.IsValue("HEREINAFTER", null) || t.IsValue("ABBREVIATED", null) || t.IsValue("HEREAFTER", null)) 
            {
                t = t.Next;
                if (t != null && t.IsValue("REFER", null)) 
                    t = t.Next;
            }
            else 
                return null;
            while (t != null) 
            {
                if (!(t is Pullenti.Ner.TextToken)) 
                    break;
                else if (!t.Chars.IsLetter) 
                    t = t.Next;
                else if (t.Morph.Class.IsPreposition || t.Morph.Class.IsMisc || t.IsValue("ИМЕНОВАТЬ", null)) 
                    t = t.Next;
                else 
                    break;
            }
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
            Pullenti.Ner.MetaToken mt = new Pullenti.Ner.MetaToken(t, t1.Next);
            string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
            mt.Tag = nam;
            if (nam.IndexOf(' ') < 0) 
            {
                for (Pullenti.Ner.Token tt = rt.BeginToken; tt != null && tt.EndChar <= rt.EndChar; tt = tt.Next) 
                {
                    if (tt.IsValue(mt.Tag as string, null)) 
                        return mt;
                }
                return null;
            }
            return mt;
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            if (begin == null) 
                return null;
            if (begin.Kit.RecurseLevel > 2) 
                return null;
            begin.Kit.RecurseLevel++;
            Pullenti.Ner.ReferentToken rt = this.TryAttachOrg(begin, null, AttachType.Normal, null, false, 0, -1);
            if (rt == null) 
                rt = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttachOrg(begin, false);
            if (rt == null) 
                rt = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttachOrg(begin, true);
            if (rt == null) 
                rt = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttachReferenceToExistOrg(begin);
            begin.Kit.RecurseLevel--;
            if (rt == null) 
                return null;
            rt.Data = begin.Kit.GetAnalyzerData(this);
            return rt;
        }
        enum AttachType : int
        {
            Normal,
            NormalAfterDep,
            Multiple,
            High,
            ExtOntology,
        }

        List<Pullenti.Ner.ReferentToken> TryAttachOrgs(Pullenti.Ner.Token t, OrgAnalyzerData ad, int step)
        {
            if (t == null) 
                return null;
            if (ad != null && ad.LocalOntology.Items.Count > 1000) 
                ad = null;
            if (t.Chars.IsLatinLetter && Pullenti.Ner.Core.MiscHelper.IsEngArticle(t)) 
            {
                List<Pullenti.Ner.ReferentToken> res11 = this.TryAttachOrgs(t.Next, ad, step);
                if (res11 != null && res11.Count > 0) 
                {
                    res11[0].BeginToken = t;
                    return res11;
                }
            }
            Pullenti.Ner.ReferentToken rt = null;
            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ = null;
            if (step == 0 || t.InnerBool) 
            {
                typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t, false, null);
                if (typ != null) 
                    t.InnerBool = true;
                if (typ == null || typ.Chars.IsLatinLetter) 
                {
                    Pullenti.Ner.Org.Internal.OrgItemEngItem ltyp = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttach(t, false);
                    if (ltyp != null) 
                        t.InnerBool = true;
                    else if (t.Chars.IsLatinLetter) 
                    {
                        Pullenti.Ner.ReferentToken rte = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttachOrg(t, false);
                        if (rte != null) 
                        {
                            this._doPostAnalyze(rte, ad);
                            List<Pullenti.Ner.ReferentToken> ree = new List<Pullenti.Ner.ReferentToken>();
                            ree.Add(rte);
                            return ree;
                        }
                    }
                }
            }
            Pullenti.Ner.ReferentToken rt00 = this.TryAttachSpec(t, ad);
            if (rt00 == null) 
                rt00 = this._tryAttachOrgByAlias(t, ad);
            if (rt00 != null) 
            {
                List<Pullenti.Ner.ReferentToken> res0 = new List<Pullenti.Ner.ReferentToken>();
                this._doPostAnalyze(rt00, ad);
                res0.Add(rt00);
                return res0;
            }
            if (typ != null) 
            {
                if (typ.Root == null || !typ.Root.IsPurePrefix) 
                {
                    if (((typ.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) != Pullenti.Morph.MorphNumber.Undefined) 
                    {
                        Pullenti.Ner.Token t1 = typ.EndToken;
                        bool ok = true;
                        bool ok1 = false;
                        if (t1.Next != null && t1.Next.IsChar(',')) 
                        {
                            t1 = t1.Next;
                            ok1 = true;
                            if (t1.Next != null && t1.Next.IsValue("КАК", null)) 
                                t1 = t1.Next;
                            else 
                                ok = false;
                        }
                        if (t1.Next != null && t1.Next.IsValue("КАК", null)) 
                        {
                            t1 = t1.Next;
                            ok1 = true;
                        }
                        if (t1.Next != null && t1.Next.IsChar(':')) 
                            t1 = t1.Next;
                        if (t1 == t && t1.IsNewlineAfter) 
                            ok = false;
                        rt = null;
                        if (ok) 
                        {
                            if (!ok1 && typ.Coef > 0) 
                                ok1 = true;
                            if (ok1) 
                                rt = this.TryAttachOrg(t1.Next, ad, AttachType.Multiple, typ, false, 0, -1);
                        }
                        if (rt != null) 
                        {
                            this._doPostAnalyze(rt, ad);
                            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
                            res.Add(rt);
                            OrganizationReferent org = rt.Referent as OrganizationReferent;
                            if (ok1) 
                                rt.BeginToken = t;
                            t1 = rt.EndToken.Next;
                            ok = true;
                            for (; t1 != null; t1 = t1.Next) 
                            {
                                if (t1.IsNewlineBefore) 
                                {
                                    ok = false;
                                    break;
                                }
                                bool last = false;
                                if (t1.IsChar(',')) 
                                {
                                }
                                else if (t1.IsAnd || t1.IsOr) 
                                    last = true;
                                else 
                                {
                                    if (res.Count < 2) 
                                        ok = false;
                                    break;
                                }
                                t1 = t1.Next;
                                Pullenti.Ner.Org.Internal.OrgItemTypeToken typ1 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1, true, ad);
                                if (typ1 != null) 
                                {
                                    ok = false;
                                    break;
                                }
                                rt = this.TryAttachOrg(t1, ad, AttachType.Multiple, typ, false, 0, -1);
                                if (rt != null && rt.BeginToken == rt.EndToken) 
                                {
                                    if (!rt.BeginToken.GetMorphClassInDictionary().IsUndefined && rt.BeginToken.Chars.IsAllUpper) 
                                        rt = null;
                                }
                                if (rt == null) 
                                {
                                    if (res.Count < 2) 
                                        ok = false;
                                    break;
                                }
                                this._doPostAnalyze(rt, ad);
                                res.Add(rt);
                                if (res.Count > 100) 
                                {
                                    ok = false;
                                    break;
                                }
                                org = rt.Referent as OrganizationReferent;
                                org.AddType(typ, false);
                                if (last) 
                                    break;
                                t1 = rt.EndToken;
                            }
                            if (ok && res.Count > 1) 
                                return res;
                        }
                    }
                }
            }
            rt = null;
            if (typ != null && ((typ.IsDep || typ.CanBeDepBeforeOrganization))) 
            {
                rt = this.TryAttachDepBeforeOrg(typ, null);
                if (rt == null) 
                    rt = this.TryAttachDepAfterOrg(typ);
                if (rt == null) 
                    rt = this.TryAttachOrg(typ.EndToken.Next, ad, AttachType.NormalAfterDep, null, false, 0, -1);
            }
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (((step == 0 && rt == null && tt != null) && !tt.Chars.IsAllLower && tt.Chars.IsCyrillicLetter) && tt.GetMorphClassInDictionary().IsUndefined) 
            {
                string s = tt.Term;
                if (((s.StartsWith("ГУ") || s.StartsWith("РУ"))) && s.Length > 3 && ((s.Length > 4 || s == "ГУВД"))) 
                {
                    tt.Term = (s == "ГУВД" ? "МВД" : tt.Term.Substring(2));
                    short inv = tt.InvariantPrefixLengthOfMorphVars;
                    tt.InvariantPrefixLengthOfMorphVars = 0;
                    short max = tt.MaxLengthOfMorphVars;
                    tt.MaxLengthOfMorphVars = (short)tt.Term.Length;
                    rt = this.TryAttachOrg(tt, ad, AttachType.NormalAfterDep, null, false, 0, -1);
                    tt.Term = s;
                    tt.InvariantPrefixLengthOfMorphVars = inv;
                    tt.MaxLengthOfMorphVars = max;
                    if (rt != null) 
                    {
                        if (ad != null && ad.LocOrgs.TryAttach(tt, null, false) != null) 
                            rt = null;
                        if (t.Kit.Ontology != null && t.Kit.Ontology.AttachToken(OrganizationReferent.OBJ_TYPENAME, tt) != null) 
                            rt = null;
                    }
                    if (rt != null) 
                    {
                        typ = new Pullenti.Ner.Org.Internal.OrgItemTypeToken(tt, tt);
                        typ.Typ = (s.StartsWith("ГУ") ? "главное управление" : "региональное управление");
                        Pullenti.Ner.ReferentToken rt0 = this.TryAttachDepBeforeOrg(typ, rt);
                        if (rt0 != null) 
                        {
                            if (ad != null) 
                                rt.Referent = ad.RegisterReferent(rt.Referent);
                            rt.Referent.AddOccurence(new Pullenti.Ner.TextAnnotation(t, rt.EndToken, rt.Referent));
                            (rt0.Referent as OrganizationReferent).Higher = rt.Referent as OrganizationReferent;
                            List<Pullenti.Ner.ReferentToken> li2 = new List<Pullenti.Ner.ReferentToken>();
                            this._doPostAnalyze(rt0, ad);
                            li2.Add(rt0);
                            return li2;
                        }
                    }
                }
                else if ((((((((((s[0] == 'У' && s.Length > 3 && tt.GetMorphClassInDictionary().IsUndefined)) || s == "ОВД" || s == "РОВД") || s == "ОМВД" || s == "ОСБ") || s == "УПФ" || s == "УФНС") || s == "ИФНС" || s == "ИНФС") || s == "УВД" || s == "УФМС") || s == "УФСБ" || s == "ОУФМС") || s == "ОФМС" || s == "УФК") || s == "УФССП") 
                {
                    if (s == "ОВД" || s == "УВД" || s == "РОВД") 
                        tt.Term = "МВД";
                    else if (s == "ОСБ") 
                        tt.Term = "СБЕРБАНК";
                    else if (s == "УПФ") 
                        tt.Term = "ПФР";
                    else if (s == "УФНС" || s == "ИФНС" || s == "ИНФС") 
                        tt.Term = "ФНС";
                    else if (s == "УФМС" || s == "ОУФМС" || s == "ОФМС") 
                        tt.Term = "ФМС";
                    else 
                        tt.Term = tt.Term.Substring(1);
                    short inv = tt.InvariantPrefixLengthOfMorphVars;
                    tt.InvariantPrefixLengthOfMorphVars = 0;
                    short max = tt.MaxLengthOfMorphVars;
                    tt.MaxLengthOfMorphVars = (short)tt.Term.Length;
                    rt = this.TryAttachOrg(tt, ad, AttachType.NormalAfterDep, null, false, 0, -1);
                    tt.Term = s;
                    tt.InvariantPrefixLengthOfMorphVars = inv;
                    tt.MaxLengthOfMorphVars = max;
                    if (rt != null) 
                    {
                        OrganizationReferent org1 = rt.Referent as OrganizationReferent;
                        if (org1.GeoObjects.Count == 0 && rt.EndToken.Next != null) 
                        {
                            Pullenti.Ner.Geo.GeoReferent g = rt.EndToken.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                            if (g != null && g.IsState) 
                            {
                                org1.AddGeoObject(g);
                                rt.EndToken = rt.EndToken.Next;
                            }
                        }
                        typ = new Pullenti.Ner.Org.Internal.OrgItemTypeToken(tt, tt);
                        typ.Typ = (s[0] == 'О' ? "отделение" : (s[0] == 'И' ? "инспекция" : "управление"));
                        Pullenti.Morph.MorphGender gen = (s[0] == 'И' ? Pullenti.Morph.MorphGender.Feminie : Pullenti.Morph.MorphGender.Neuter);
                        if (s.StartsWith("ОУ")) 
                            typ.Typ = "управление";
                        else if (s.StartsWith("РО")) 
                        {
                            typ.Typ = "отдел";
                            typ.AltTyp = "районный отдел";
                            typ.NameIsName = true;
                            gen = Pullenti.Morph.MorphGender.Masculine;
                        }
                        Pullenti.Ner.ReferentToken rt0 = this.TryAttachDepBeforeOrg(typ, rt);
                        if (rt0 != null) 
                        {
                            OrganizationReferent org0 = rt0.Referent as OrganizationReferent;
                            org0.AddProfile(OrgProfile.Unit);
                            if (org0.Number == null && !tt.IsNewlineAfter) 
                            {
                                Pullenti.Ner.Org.Internal.OrgItemNumberToken num = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(tt.Next, true, typ);
                                if (num != null) 
                                {
                                    org0.Number = num.Number;
                                    rt0.EndToken = num.EndToken;
                                }
                            }
                            object geo;
                            if (rt0.Referent.FindSlot(OrganizationReferent.ATTR_GEO, null, true) == null) 
                            {
                                if ((((geo = this.IsGeo(rt0.EndToken.Next, false)))) != null) 
                                {
                                    if ((rt0.Referent as OrganizationReferent).AddGeoObject(geo)) 
                                        rt0.EndToken = this.GetGeoEndToken(geo, rt0.EndToken.Next);
                                }
                                else if (rt0.EndToken.WhitespacesAfterCount < 3) 
                                {
                                    Pullenti.Ner.Org.Internal.OrgItemNameToken nam = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(rt0.EndToken.Next, null, false, true);
                                    if (nam != null && !nam.Value.StartsWith("СУБЪЕКТ")) 
                                    {
                                        if ((((geo = this.IsGeo(nam.EndToken.Next, false)))) != null) 
                                        {
                                            if ((rt0.Referent as OrganizationReferent).AddGeoObject(geo)) 
                                                rt0.EndToken = this.GetGeoEndToken(geo, nam.EndToken.Next);
                                            (rt0.Referent as OrganizationReferent).AddName(nam.Value, true, null);
                                        }
                                    }
                                }
                            }
                            if (rt0.Referent.Slots.Count > 3) 
                            {
                                if (tt.Previous != null && ((tt.Previous.Morph.Class.IsAdjective && !tt.Previous.Morph.Class.IsVerb)) && tt.WhitespacesBeforeCount == 1) 
                                {
                                    string adj = Pullenti.Morph.MorphologyService.GetWordform(tt.Previous.GetSourceText().ToUpper(), new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective, Gender = gen, Language = tt.Previous.Morph.Language });
                                    if (adj != null && !adj.StartsWith("УПОЛНОМОЧ") && !adj.StartsWith("ОПЕРУПОЛНОМОЧ")) 
                                    {
                                        string tyy = string.Format("{0} {1}", adj.ToLower(), typ.Typ);
                                        rt0.BeginToken = tt.Previous;
                                        if (rt0.BeginToken.Previous != null && rt0.BeginToken.Previous.IsHiphen && rt0.BeginToken.Previous.Previous != null) 
                                        {
                                            Pullenti.Ner.Token tt0 = rt0.BeginToken.Previous.Previous;
                                            if (tt0.Chars == rt0.BeginToken.Chars && (tt0 is Pullenti.Ner.TextToken)) 
                                            {
                                                adj = (tt0 as Pullenti.Ner.TextToken).Term;
                                                if (tt0.Morph.Class.IsAdjective && !tt0.Morph.ContainsAttr("неизм.", null)) 
                                                    adj = Pullenti.Morph.MorphologyService.GetWordform(adj, new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective, Gender = gen, Language = tt0.Morph.Language });
                                                tyy = string.Format("{0} {1}", adj.ToLower(), tyy);
                                                rt0.BeginToken = tt0;
                                            }
                                        }
                                        if (typ.NameIsName) 
                                            org0.AddName(tyy.ToUpper(), true, null);
                                        else 
                                            org0.AddTypeStr(tyy);
                                    }
                                }
                                foreach (Pullenti.Ner.Geo.GeoReferent g in org1.GeoObjects) 
                                {
                                    if (!g.IsState) 
                                    {
                                        Pullenti.Ner.Slot sl = org1.FindSlot(OrganizationReferent.ATTR_GEO, g, true);
                                        if (sl != null) 
                                            org1.Slots.Remove(sl);
                                        if (rt.BeginToken.BeginChar < rt0.BeginToken.BeginChar) 
                                            rt0.BeginToken = rt.BeginToken;
                                        org0.AddGeoObject(g);
                                        org1.MoveExtReferent(org0, g);
                                    }
                                }
                                if (ad != null) 
                                    rt.Referent = ad.RegisterReferent(rt.Referent);
                                rt.Referent.AddOccurence(new Pullenti.Ner.TextAnnotation(t, rt.EndToken, rt.Referent));
                                (rt0.Referent as OrganizationReferent).Higher = rt.Referent as OrganizationReferent;
                                this._doPostAnalyze(rt0, ad);
                                List<Pullenti.Ner.ReferentToken> li2 = new List<Pullenti.Ner.ReferentToken>();
                                li2.Add(rt0);
                                return li2;
                            }
                        }
                        rt = null;
                    }
                }
            }
            if (rt == null) 
            {
                if (step > 0 && typ == null) 
                {
                    if (!Pullenti.Ner.Core.BracketHelper.IsBracket(t, false)) 
                    {
                        if (!t.Chars.IsLetter) 
                            return null;
                        if (t.Chars.IsAllLower) 
                            return null;
                    }
                }
                rt = this.TryAttachOrg(t, ad, AttachType.Normal, null, false, 0, step);
                if (rt == null && step == 0) 
                    rt = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttachOrg(t, false);
                if (rt != null) 
                {
                }
            }
            if (((rt == null && step == 1 && typ != null) && typ.IsDep && typ.Root != null) && !typ.Root.CanBeNormalDep) 
            {
                if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.CheckOrgSpecialWordBefore(typ.BeginToken.Previous)) 
                    rt = this.TryAttachDep(typ, AttachType.High, true);
            }
            if (rt == null && step == 0 && t != null) 
            {
                bool ok = false;
                if (t.LengthChar > 2 && !t.Chars.IsAllLower && t.Chars.IsLatinLetter) 
                    ok = true;
                else if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                    ok = true;
                if (ok && t.WhitespacesBeforeCount != 1) 
                    ok = false;
                if (ok && !Pullenti.Ner.Org.Internal.OrgItemTypeToken.CheckPersonProperty(t.Previous)) 
                    ok = false;
                if (ok) 
                {
                    OrganizationReferent org = new OrganizationReferent();
                    rt = new Pullenti.Ner.ReferentToken(org, t, t);
                    if (t.Chars.IsLatinLetter && Pullenti.Ner.Core.NumberHelper.TryParseRoman(t) == null) 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemNameToken nam = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(t, null, false, true);
                        if (nam != null) 
                        {
                            StringBuilder name = new StringBuilder();
                            name.Append(nam.Value);
                            rt.EndToken = nam.EndToken;
                            for (Pullenti.Ner.Token ttt = nam.EndToken.Next; ttt != null; ttt = ttt.Next) 
                            {
                                if (!ttt.Chars.IsLatinLetter) 
                                    break;
                                nam = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(ttt, null, false, false);
                                if (nam == null) 
                                    break;
                                rt.EndToken = nam.EndToken;
                                if (!nam.IsStdTail) 
                                    name.AppendFormat(" {0}", nam.Value);
                                else 
                                {
                                    Pullenti.Ner.Org.Internal.OrgItemEngItem ei = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttach(nam.BeginToken, false);
                                    if (ei != null) 
                                    {
                                        org.AddTypeStr(ei.FullValue);
                                        if (ei.ShortValue != null) 
                                            org.AddTypeStr(ei.ShortValue);
                                    }
                                }
                            }
                            org.AddName(name.ToString(), true, null);
                        }
                    }
                    else 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            Pullenti.Ner.ReferentToken rt11 = this.TryAttachOrg(t.Next, ad, AttachType.Normal, null, false, 0, -1);
                            if (rt11 != null && ((rt11.EndToken == br.EndToken.Previous || rt11.EndToken == br.EndToken))) 
                            {
                                rt11.BeginToken = t;
                                rt11.EndToken = br.EndToken;
                                rt = rt11;
                                org = rt11.Referent as OrganizationReferent;
                            }
                            else 
                            {
                                org.AddName(Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative), true, null);
                                org.AddName(Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No), true, br.BeginToken.Next);
                                if (br.BeginToken.Next == br.EndToken.Previous && br.BeginToken.Next.GetMorphClassInDictionary().IsUndefined) 
                                {
                                    foreach (Pullenti.Morph.MorphBaseInfo wf in br.BeginToken.Next.Morph.Items) 
                                    {
                                        if (wf.Case.IsGenitive && (wf is Pullenti.Morph.MorphWordForm)) 
                                            org.AddName((wf as Pullenti.Morph.MorphWordForm).NormalCase, true, null);
                                    }
                                }
                                rt.EndToken = br.EndToken;
                            }
                        }
                    }
                    if (org.Slots.Count == 0) 
                        rt = null;
                }
            }
            if (rt == null) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br == null || br.LengthChar > 100) 
                        br = null;
                    if (br != null) 
                    {
                        Pullenti.Ner.Token t1 = br.EndToken.Next;
                        if (t1 != null && t1.IsComma) 
                            t1 = t1.Next;
                        if (t1 != null && (t1.WhitespacesBeforeCount < 3)) 
                        {
                            if ((((typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1, false, null)))) != null && typ.Root != null && typ.Root.Typ == Pullenti.Ner.Org.Internal.OrgItemTypeTyp.Prefix) 
                            {
                                Pullenti.Ner.Token t2 = typ.EndToken.Next;
                                bool ok = false;
                                if (t2 == null || t2.IsNewlineBefore) 
                                    ok = true;
                                else if (t2.IsCharOf(".,:;")) 
                                    ok = true;
                                else if (t2 is Pullenti.Ner.ReferentToken) 
                                    ok = true;
                                if (ok) 
                                {
                                    OrganizationReferent org = new OrganizationReferent();
                                    rt = new Pullenti.Ner.ReferentToken(org, t, typ.EndToken);
                                    org.AddType(typ, false);
                                    string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken.Next, br.EndToken.Previous, Pullenti.Ner.Core.GetTextAttr.No);
                                    org.AddName(nam, true, null);
                                    Pullenti.Ner.ReferentToken rt11 = this.TryAttachOrg(br.BeginToken.Next, ad, AttachType.Normal, null, false, 0, -1);
                                    if (rt11 != null && rt11.EndChar <= typ.EndChar) 
                                        org.MergeSlots(rt11.Referent, true);
                                }
                            }
                        }
                    }
                }
                if (rt == null) 
                    return null;
            }
            this._doPostAnalyze(rt, ad);
            if (step > 0) 
            {
                Pullenti.Ner.MetaToken mt = _checkAliasAfter(rt, rt.EndToken.Next);
                if (mt != null) 
                {
                    if (ad != null) 
                    {
                        Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                        term.InitBy(mt.BeginToken, mt.EndToken.Previous, rt.Referent, false);
                        ad.Aliases.Add(term);
                    }
                    rt.EndToken = mt.EndToken;
                }
            }
            List<Pullenti.Ner.ReferentToken> li = new List<Pullenti.Ner.ReferentToken>();
            li.Add(rt);
            Pullenti.Ner.Token tt1 = rt.EndToken.Next;
            if (tt1 != null && tt1.IsChar('(')) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                    tt1 = br.EndToken.Next;
            }
            if (tt1 != null && tt1.IsCommaAnd) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt1.Next, true, false)) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt.EndToken, true, null, false)) 
                    {
                        bool ok = false;
                        for (Pullenti.Ner.Token ttt = tt1; ttt != null; ttt = ttt.Next) 
                        {
                            if (ttt.IsChar('.')) 
                            {
                                ok = true;
                                break;
                            }
                            if (ttt.IsChar('(')) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br1 = Pullenti.Ner.Core.BracketHelper.TryParse(ttt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br1 != null) 
                                {
                                    ttt = br1.EndToken;
                                    continue;
                                }
                            }
                            if (!ttt.IsCommaAnd) 
                                break;
                            if (!Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(ttt.Next, true, false)) 
                                break;
                            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(ttt.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br == null) 
                                break;
                            bool addTyp = false;
                            Pullenti.Ner.ReferentToken rt1 = this._TryAttachOrg_(ttt.Next.Next, ttt.Next.Next, ad, null, true, AttachType.Normal, null, false, 0);
                            if (rt1 == null || (rt1.EndChar < (br.EndChar - 1))) 
                            {
                                addTyp = true;
                                rt1 = this._TryAttachOrg_(ttt.Next, ttt.Next, ad, null, true, AttachType.High, null, false, 0);
                            }
                            if (rt1 == null || (rt1.EndChar < (br.EndChar - 1))) 
                                break;
                            li.Add(rt1);
                            OrganizationReferent org1 = rt1.Referent as OrganizationReferent;
                            if (typ != null) 
                                ok = true;
                            if (org1.Types.Count == 0) 
                                addTyp = true;
                            if (addTyp) 
                            {
                                if (typ != null) 
                                    org1.AddType(typ, false);
                                string s = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                                if (s != null) 
                                {
                                    bool ex = false;
                                    foreach (string n in org1.Names) 
                                    {
                                        if (s.StartsWith(n)) 
                                        {
                                            ex = true;
                                            break;
                                        }
                                    }
                                    if (!ex) 
                                        org1.AddName(s, true, br.BeginToken.Next);
                                }
                            }
                            if (ttt.IsAnd) 
                            {
                                ok = true;
                                break;
                            }
                            ttt = rt1.EndToken;
                        }
                        if (!ok && li.Count > 1) 
                            li.RemoveRange(1, li.Count - 1);
                    }
                }
            }
            return li;
        }
        Pullenti.Ner.ReferentToken TryAttachSpec(Pullenti.Ner.Token t, OrgAnalyzerData ad)
        {
            Pullenti.Ner.ReferentToken rt = this.TryAttachPropNames(t, ad);
            if (rt == null) 
                rt = this.TryAttachPoliticParty(t, ad, false);
            if (rt == null) 
                rt = this.TryAttachArmy(t, ad);
            return rt;
        }
        static bool _corrBrackets(Pullenti.Ner.ReferentToken rt)
        {
            if (!Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(rt.BeginToken.Previous, true, false) || !Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt.EndToken.Next, true, null, false)) 
                return false;
            rt.BeginToken = rt.BeginToken.Previous;
            rt.EndToken = rt.EndToken.Next;
            return true;
        }
        void _doPostAnalyze(Pullenti.Ner.ReferentToken rt, OrgAnalyzerData ad)
        {
            if (rt.Morph.Case.IsUndefined) 
            {
                if (!rt.BeginToken.Chars.IsAllUpper) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(rt.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt1 == null) 
                        npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(rt.BeginToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt1 != null) 
                        rt.Morph = npt1.Morph;
                }
            }
            OrganizationReferent o = rt.Referent as OrganizationReferent;
            if ((rt.Kit.Ontology != null && o.OntologyItems == null && o.Higher == null) && o.m_TempParentOrg == null) 
            {
                List<Pullenti.Ner.ExtOntologyItem> ot = rt.Kit.Ontology.AttachReferent(o);
                if (ot != null && ot.Count == 1 && (ot[0].Referent is OrganizationReferent)) 
                {
                    OrganizationReferent oo = ot[0].Referent as OrganizationReferent;
                    o.MergeSlots(oo, false);
                    o.OntologyItems = ot;
                    foreach (Pullenti.Ner.Slot sl in o.Slots) 
                    {
                        if (sl.Value is Pullenti.Ner.Referent) 
                        {
                            bool ext = false;
                            foreach (Pullenti.Ner.Slot ss in oo.Slots) 
                            {
                                if (ss.Value == sl.Value) 
                                {
                                    ext = true;
                                    break;
                                }
                            }
                            if (!ext) 
                                continue;
                            Pullenti.Ner.Referent rr = (sl.Value as Pullenti.Ner.Referent).Clone();
                            rr.Occurrence.Clear();
                            o.UploadSlot(sl, rr);
                            Pullenti.Ner.ReferentToken rtEx = new Pullenti.Ner.ReferentToken(rr, rt.BeginToken, rt.EndToken);
                            rtEx.SetDefaultLocalOnto(rt.Kit.Processor);
                            o.AddExtReferent(rtEx);
                            foreach (Pullenti.Ner.Slot sss in rr.Slots) 
                            {
                                if (sss.Value is Pullenti.Ner.Referent) 
                                {
                                    Pullenti.Ner.Referent rrr = (sss.Value as Pullenti.Ner.Referent).Clone();
                                    rrr.Occurrence.Clear();
                                    rr.UploadSlot(sss, rrr);
                                    Pullenti.Ner.ReferentToken rtEx2 = new Pullenti.Ner.ReferentToken(rrr, rt.BeginToken, rt.EndToken);
                                    rtEx2.SetDefaultLocalOnto(rt.Kit.Processor);
                                    (sl.Value as Pullenti.Ner.Referent).AddExtReferent(rtEx2);
                                }
                            }
                        }
                    }
                }
            }
            if (o.Higher == null && o.m_TempParentOrg == null) 
            {
                if ((rt.BeginToken.Previous is Pullenti.Ner.ReferentToken) && (rt.BeginToken.Previous.GetReferent() is OrganizationReferent)) 
                {
                    OrganizationReferent oo = rt.BeginToken.Previous.GetReferent() as OrganizationReferent;
                    if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(oo, o, false)) 
                        o.m_TempParentOrg = oo;
                }
                if (o.m_TempParentOrg == null && (rt.EndToken.Next is Pullenti.Ner.ReferentToken) && (rt.EndToken.Next.GetReferent() is OrganizationReferent)) 
                {
                    OrganizationReferent oo = rt.EndToken.Next.GetReferent() as OrganizationReferent;
                    if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(oo, o, false)) 
                        o.m_TempParentOrg = oo;
                }
                if (o.m_TempParentOrg == null) 
                {
                    Pullenti.Ner.ReferentToken rt1 = this.TryAttachOrg(rt.EndToken.Next, null, AttachType.NormalAfterDep, null, false, 0, -1);
                    if (rt1 != null && rt.EndToken.Next == rt1.BeginToken) 
                    {
                        if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(rt1.Referent as OrganizationReferent, o, false)) 
                            o.m_TempParentOrg = rt1.Referent as OrganizationReferent;
                    }
                }
            }
            if (rt.EndToken.Next == null) 
                return;
            _corrBrackets(rt);
            if (rt.BeginToken.Previous != null && rt.BeginToken.Previous.Morph.Class.IsAdjective && (rt.WhitespacesBeforeCount < 2)) 
            {
                if ((rt.Referent as OrganizationReferent).GeoObjects.Count == 0) 
                {
                    object geo = this.IsGeo(rt.BeginToken.Previous, true);
                    if (geo != null) 
                    {
                        if ((rt.Referent as OrganizationReferent).AddGeoObject(geo)) 
                            rt.BeginToken = rt.BeginToken.Previous;
                    }
                }
            }
            Pullenti.Ner.Token ttt = rt.EndToken.Next;
            int errs = 1;
            bool br = false;
            if (ttt != null && ttt.IsChar('(')) 
            {
                br = true;
                ttt = ttt.Next;
            }
            List<Pullenti.Ner.Referent> refs = new List<Pullenti.Ner.Referent>();
            bool keyword = false;
            bool hasInn = false;
            int hasOk = 0;
            Pullenti.Ner.Token te = null;
            for (; ttt != null; ttt = ttt.Next) 
            {
                if (ttt.IsCharOf(",;") || ttt.Morph.Class.IsPreposition) 
                    continue;
                if (ttt.IsChar(')')) 
                {
                    if (br) 
                        te = ttt;
                    break;
                }
                Pullenti.Ner.Referent rr = ttt.GetReferent();
                if (rr != null) 
                {
                    if (rr.TypeName == "ADDRESS" || rr.TypeName == "DATE" || ((rr.TypeName == "GEO" && br))) 
                    {
                        if (keyword || br || (ttt.WhitespacesBeforeCount < 2)) 
                        {
                            refs.Add(rr);
                            te = ttt;
                            continue;
                        }
                        break;
                    }
                    if (rr.TypeName == "URI") 
                    {
                        string sch = rr.GetStringValue("SCHEME");
                        if (sch == null) 
                            break;
                        if (sch == "ИНН") 
                        {
                            errs = 5;
                            hasInn = true;
                        }
                        else if (sch.StartsWith("ОК")) 
                            hasOk++;
                        else if (sch != "КПП" && sch != "ОГРН" && !br) 
                            break;
                        refs.Add(rr);
                        te = ttt;
                        if (ttt.Next != null && ttt.Next.IsChar('(')) 
                        {
                            Pullenti.Ner.Core.BracketSequenceToken brrr = Pullenti.Ner.Core.BracketHelper.TryParse(ttt.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (brrr != null) 
                                ttt = brrr.EndToken;
                        }
                        continue;
                    }
                    else if (rr == rt.Referent) 
                        continue;
                }
                if (ttt.IsNewlineBefore && !br) 
                    break;
                if (ttt is Pullenti.Ner.TextToken) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null) 
                    {
                        if ((npt.EndToken.IsValue("ДАТА", null) || npt.EndToken.IsValue("РЕГИСТРАЦИЯ", null) || npt.EndToken.IsValue("ЛИЦО", null)) || npt.EndToken.IsValue("ЮР", null) || npt.EndToken.IsValue("АДРЕС", null)) 
                        {
                            ttt = npt.EndToken;
                            keyword = true;
                            continue;
                        }
                    }
                    if (ttt.IsValue("REGISTRATION", null) && ttt.Next != null && ttt.Next.IsValue("NUMBER", null)) 
                    {
                        StringBuilder tmp = new StringBuilder();
                        for (Pullenti.Ner.Token tt3 = ttt.Next.Next; tt3 != null; tt3 = tt3.Next) 
                        {
                            if (tt3.IsWhitespaceBefore && tmp.Length > 0) 
                                break;
                            if (((tt3.IsCharOf(":") || tt3.IsHiphen)) && tmp.Length == 0) 
                                continue;
                            if (tt3 is Pullenti.Ner.TextToken) 
                                tmp.Append((tt3 as Pullenti.Ner.TextToken).Term);
                            else if (tt3 is Pullenti.Ner.NumberToken) 
                                tmp.Append(tt3.GetSourceText());
                            else 
                                break;
                            rt.EndToken = (ttt = tt3);
                        }
                        if (tmp.Length > 0) 
                            rt.Referent.AddSlot(OrganizationReferent.ATTR_MISC, tmp.ToString(), false, 0);
                        continue;
                    }
                    if ((ttt.IsValue("REGISTERED", null) && ttt.Next != null && ttt.Next.IsValue("IN", null)) && (ttt.Next.Next is Pullenti.Ner.ReferentToken) && (ttt.Next.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                        rt.Referent.AddSlot(OrganizationReferent.ATTR_MISC, ttt.Next.Next.GetReferent(), false, 0);
                        rt.EndToken = (ttt = ttt.Next.Next);
                        continue;
                    }
                    if (br) 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemTypeToken otyp = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(ttt, true, null);
                        if (otyp != null && (ttt.WhitespacesBeforeCount < 2) && otyp.Geo == null) 
                        {
                            OrganizationReferent or1 = new OrganizationReferent();
                            or1.AddType(otyp, false);
                            if (!Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticOO(o, or1) && otyp.EndToken.Next != null && otyp.EndToken.Next.IsChar(')')) 
                            {
                                o.AddType(otyp, false);
                                rt.EndToken = (ttt = otyp.EndToken);
                                if (br && ttt.Next != null && ttt.Next.IsChar(')')) 
                                {
                                    rt.EndToken = ttt.Next;
                                    break;
                                }
                                continue;
                            }
                        }
                    }
                }
                keyword = false;
                if ((--errs) <= 0) 
                    break;
            }
            if (te != null && refs.Count > 0 && ((te.IsChar(')') || hasInn || hasOk > 0))) 
            {
                foreach (Pullenti.Ner.Referent rr in refs) 
                {
                    if (rr.TypeName == GEONAME) 
                        (rt.Referent as OrganizationReferent).AddGeoObject(rr);
                    else 
                        rt.Referent.AddSlot(OrganizationReferent.ATTR_MISC, rr, false, 0);
                }
                rt.EndToken = te;
            }
            if ((rt.WhitespacesBeforeCount < 2) && (rt.BeginToken.Previous is Pullenti.Ner.TextToken) && rt.BeginToken.Previous.Chars.IsAllUpper) 
            {
                string term = (rt.BeginToken.Previous as Pullenti.Ner.TextToken).Term;
                foreach (Pullenti.Ner.Slot s in o.Slots) 
                {
                    if (s.Value is string) 
                    {
                        string a = Pullenti.Ner.Core.MiscHelper.GetAbbreviation(s.Value as string);
                        if (a != null && a == term) 
                        {
                            rt.BeginToken = rt.BeginToken.Previous;
                            break;
                        }
                    }
                }
            }
        }
        Pullenti.Ner.ReferentToken _tryAttachOrgByAlias(Pullenti.Ner.Token t, OrgAnalyzerData ad)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            bool br = false;
            if (t0.Next != null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t0, true, false)) 
            {
                t = t0.Next;
                br = true;
            }
            if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && !t.Chars.IsAllLower) 
            {
                if (t.LengthChar > 3) 
                {
                }
                else if (t.LengthChar > 1 && t.Chars.IsAllUpper) 
                {
                }
                else 
                    return null;
            }
            else 
                return null;
            if (ad != null) 
            {
                Pullenti.Ner.Core.TerminToken tok = ad.Aliases.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                {
                    Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(tok.Termin.Tag as Pullenti.Ner.Referent, t0, tok.EndToken);
                    if (br) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tok.EndToken.Next, true, null, false)) 
                            rt0.EndToken = tok.EndToken.Next;
                        else 
                            return null;
                    }
                    return rt0;
                }
            }
            if (!br) 
            {
                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                    return null;
                if (!Pullenti.Ner.Org.Internal.OrgItemTypeToken.CheckOrgSpecialWordBefore(t0.Previous)) 
                    return null;
                if (t.Chars.IsLatinLetter) 
                {
                    if (t.Next != null && t.Next.Chars.IsLatinLetter) 
                        return null;
                }
                else if (t.Next != null && ((t.Next.Chars.IsCyrillicLetter || !t.Next.Chars.IsAllLower))) 
                    return null;
            }
            else if (!Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t.Next, true, null, false)) 
                return null;
            int cou = 0;
            for (Pullenti.Ner.Token ttt = t.Previous; ttt != null && (cou < 100); ttt = ttt.Previous,cou++) 
            {
                OrganizationReferent org00 = ttt.GetReferent() as OrganizationReferent;
                if (org00 == null) 
                    continue;
                foreach (string n in org00.Names) 
                {
                    string str = n;
                    int ii = n.IndexOf(' ');
                    if (ii > 0) 
                        str = n.Substring(0, ii);
                    if (t.IsValue(str, null)) 
                    {
                        if (ad != null) 
                            ad.Aliases.Add(new Pullenti.Ner.Core.Termin(str) { Tag = org00 });
                        string term = (t as Pullenti.Ner.TextToken).Term;
                        if (ii < 0) 
                            org00.AddName(term, true, t);
                        if (br) 
                            t = t.Next;
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(org00, t0, t);
                        return rt;
                    }
                }
            }
            return null;
        }
        Pullenti.Ner.Token AttachMiddleAttributes(OrganizationReferent org, Pullenti.Ner.Token t)
        {
            Pullenti.Ner.Token te = null;
            for (; t != null; t = t.Next) 
            {
                Pullenti.Ner.Org.Internal.OrgItemNumberToken ont = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(t, false, null);
                if (ont != null) 
                {
                    org.Number = ont.Number;
                    te = (t = ont.EndToken);
                    continue;
                }
                Pullenti.Ner.Org.Internal.OrgItemEponymToken oet = Pullenti.Ner.Org.Internal.OrgItemEponymToken.TryAttach(t, false);
                if (oet != null) 
                {
                    foreach (string v in oet.Eponyms) 
                    {
                        org.AddEponym(v);
                    }
                    te = (t = oet.EndToken);
                    continue;
                }
                break;
            }
            return te;
        }
        const string GEONAME = "GEO";
        object IsGeo(Pullenti.Ner.Token t, bool canBeAdjective = false)
        {
            if (t == null) 
                return null;
            if (t.IsValue("В", null) && t.Next != null) 
                t = t.Next;
            Pullenti.Ner.Referent r = t.GetReferent();
            if (r != null) 
            {
                if (r.TypeName == GEONAME) 
                {
                    if (t.WhitespacesBeforeCount <= 15 || t.Morph.Case.IsGenitive) 
                        return r;
                }
                if (r is Pullenti.Ner.Address.AddressReferent) 
                {
                    Pullenti.Ner.Token tt = (t as Pullenti.Ner.ReferentToken).BeginToken;
                    if (tt.GetReferent() != null && tt.GetReferent().TypeName == GEONAME) 
                    {
                        if (t.WhitespacesBeforeCount < 3) 
                            return tt.GetReferent();
                    }
                }
                return null;
            }
            if (t.WhitespacesBeforeCount > 15 && !canBeAdjective) 
                return null;
            Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("GEO", t);
            if (rt == null) 
                return null;
            if (t.Previous != null && t.Previous.IsValue("ОРДЕН", null)) 
                return null;
            if (!canBeAdjective) 
            {
                if (rt.Morph.Class.IsAdjective) 
                    return null;
            }
            return rt;
        }
        Pullenti.Ner.Token GetGeoEndToken(object geo, Pullenti.Ner.Token t)
        {
            if (geo is Pullenti.Ner.ReferentToken) 
            {
                if ((geo as Pullenti.Ner.ReferentToken).GetReferent() is Pullenti.Ner.Address.AddressReferent) 
                    return t.Previous;
                return (geo as Pullenti.Ner.ReferentToken).EndToken;
            }
            else if (t != null && t.Next != null && t.Morph.Class.IsPreposition) 
                return t.Next;
            else 
                return t;
        }
        Pullenti.Ner.Token AttachTailAttributes(OrganizationReferent org, Pullenti.Ner.Token t, OrgAnalyzerData ad, bool attachForNewOrg, AttachType attachTyp, bool isGlobal = false)
        {
            Pullenti.Ner.Token t1 = null;
            OrganizationKind ki = org.Kind;
            bool canHasGeo = true;
            if (!canHasGeo) 
            {
                if (org._typesContains("комитет") || org._typesContains("академия") || org._typesContains("инспекция")) 
                    canHasGeo = true;
            }
            for (; t != null; t = (t == null ? null : t.Next)) 
            {
                if (((t.IsValue("ПО", null) || t.IsValue("В", null) || t.IsValue("IN", null))) && t.Next != null) 
                {
                    if (attachTyp == AttachType.NormalAfterDep) 
                        break;
                    if (!canHasGeo) 
                        break;
                    object r = this.IsGeo(t.Next, false);
                    if (r == null) 
                        break;
                    if (!org.AddGeoObject(r)) 
                        break;
                    t1 = this.GetGeoEndToken(r, t.Next);
                    t = t1;
                    continue;
                }
                if (t.IsValue("ИЗ", null) && t.Next != null) 
                {
                    if (attachTyp == AttachType.NormalAfterDep) 
                        break;
                    if (!canHasGeo) 
                        break;
                    object r = this.IsGeo(t.Next, false);
                    if (r == null) 
                        break;
                    if (!org.AddGeoObject(r)) 
                        break;
                    t1 = this.GetGeoEndToken(r, t.Next);
                    t = t1;
                    continue;
                }
                if (canHasGeo && org.FindSlot(OrganizationReferent.ATTR_GEO, null, true) == null && !t.IsNewlineBefore) 
                {
                    object r = this.IsGeo(t, false);
                    if (r != null) 
                    {
                        if (!org.AddGeoObject(r)) 
                            break;
                        t = (t1 = this.GetGeoEndToken(r, t));
                        continue;
                    }
                    if (t.IsChar('(')) 
                    {
                        r = this.IsGeo(t.Next, false);
                        if ((r is Pullenti.Ner.ReferentToken) && (r as Pullenti.Ner.ReferentToken).EndToken.Next != null && (r as Pullenti.Ner.ReferentToken).EndToken.Next.IsChar(')')) 
                        {
                            if (!org.AddGeoObject(r)) 
                                break;
                            t = (t1 = (r as Pullenti.Ner.ReferentToken).EndToken.Next);
                            continue;
                        }
                        if ((r is Pullenti.Ner.Geo.GeoReferent) && t.Next.Next != null && t.Next.Next.IsChar(')')) 
                        {
                            if (!org.AddGeoObject(r)) 
                                break;
                            t = (t1 = t.Next.Next);
                            continue;
                        }
                    }
                }
                if ((t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && (t.WhitespacesBeforeCount < 2)) 
                {
                    if (org.FindSlot(OrganizationReferent.ATTR_GEO, t.GetReferent(), true) != null) 
                    {
                        t1 = t;
                        continue;
                    }
                }
                if (((t.IsValue("ПРИ", null) || t.IsValue("В", null))) && t.Next != null && (t.Next is Pullenti.Ner.ReferentToken)) 
                {
                    Pullenti.Ner.Referent r = t.Next.GetReferent();
                    if (r is OrganizationReferent) 
                    {
                        if (t.IsValue("В", null) && !Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(r as OrganizationReferent, org, false)) 
                        {
                        }
                        else 
                        {
                            org.Higher = r as OrganizationReferent;
                            t1 = t.Next;
                            t = t1;
                            continue;
                        }
                    }
                }
                if (t.Chars.IsLatinLetter && (t.WhitespacesBeforeCount < 2)) 
                {
                    bool hasLatinName = false;
                    foreach (string s in org.Names) 
                    {
                        if (Pullenti.Morph.LanguageHelper.IsLatinChar(s[0])) 
                        {
                            hasLatinName = true;
                            break;
                        }
                    }
                    if (hasLatinName) 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemEngItem eng = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttach(t, false);
                        if (eng != null) 
                        {
                            org.AddTypeStr(eng.FullValue);
                            if (eng.ShortValue != null) 
                                org.AddTypeStr(eng.ShortValue);
                            t = (t1 = eng.EndToken);
                            continue;
                        }
                    }
                }
                object re = this.IsGeo(t, false);
                if (re == null && t.IsChar(',')) 
                    re = this.IsGeo(t.Next, false);
                if (re != null) 
                {
                    if (attachTyp != AttachType.NormalAfterDep) 
                    {
                        if ((!canHasGeo && ki != OrganizationKind.Bank && ki != OrganizationKind.Federation) && !org.Types.Contains("университет")) 
                            break;
                        if (org.ToString().Contains("Сбербанк") && org.FindSlot(OrganizationReferent.ATTR_GEO, null, true) != null) 
                            break;
                        if (!org.AddGeoObject(re)) 
                            break;
                        if (t.IsChar(',')) 
                            t = t.Next;
                        t1 = this.GetGeoEndToken(re, t);
                        if (t1.EndChar <= t.EndChar) 
                            break;
                        t = t1;
                        continue;
                    }
                    else 
                        break;
                }
                if (t.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br == null) 
                        break;
                    if (t.Next != null && t.Next.GetReferent() != null) 
                    {
                        if (t.Next.Next != br.EndToken) 
                            break;
                        Pullenti.Ner.Referent r = t.Next.GetReferent();
                        if (r.TypeName == GEONAME) 
                        {
                            if (!org.AddGeoObject(r)) 
                                break;
                            t = (t1 = br.EndToken);
                            continue;
                        }
                        if ((r is OrganizationReferent) && !isGlobal) 
                        {
                            if (!attachForNewOrg && !org.CanBeEquals(r, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                break;
                            org.MergeSlots(r, true);
                            t = (t1 = br.EndToken);
                            continue;
                        }
                        break;
                    }
                    if (!isGlobal) 
                    {
                        if (attachTyp != AttachType.ExtOntology) 
                        {
                            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t.Next, true, null);
                            if (typ != null && typ.EndToken == br.EndToken.Previous && !typ.IsDep) 
                            {
                                org.AddType(typ, false);
                                if (typ.Name != null) 
                                    org.AddTypeStr(typ.Name.ToLower());
                                t = (t1 = br.EndToken);
                                continue;
                            }
                        }
                        Pullenti.Ner.ReferentToken rte = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttachOrg(br.BeginToken, false);
                        if (rte != null) 
                        {
                            if (org.CanBeEquals(rte.Referent, Pullenti.Ner.Core.ReferentsEqualType.ForMerging)) 
                            {
                                org.MergeSlots(rte.Referent, true);
                                t = (t1 = rte.EndToken);
                                continue;
                            }
                        }
                        string nam = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                        if (nam != null) 
                        {
                            bool eq = false;
                            foreach (Pullenti.Ner.Slot s in org.Slots) 
                            {
                                if (s.TypeName == OrganizationReferent.ATTR_NAME) 
                                {
                                    if (Pullenti.Ner.Core.MiscHelper.CanBeEqualCyrAndLatSS(nam, (string)s.Value)) 
                                    {
                                        org.AddName(nam, true, br.BeginToken.Next);
                                        eq = true;
                                        break;
                                    }
                                }
                            }
                            if (eq) 
                            {
                                t = (t1 = br.EndToken);
                                continue;
                            }
                        }
                        bool oldName = false;
                        Pullenti.Ner.Token tt0 = t.Next;
                        if (tt0 != null) 
                        {
                            if (tt0.IsValue("РАНЕЕ", null)) 
                            {
                                oldName = true;
                                tt0 = tt0.Next;
                            }
                            else if (tt0.Morph.Class.IsAdjective && tt0.Next != null && ((tt0.Next.IsValue("НАЗВАНИЕ", null) || tt0.Next.IsValue("НАИМЕНОВАНИЕ", null)))) 
                            {
                                oldName = true;
                                tt0 = tt0.Next.Next;
                            }
                            if (oldName && tt0 != null) 
                            {
                                if (tt0.IsHiphen || tt0.IsCharOf(",:")) 
                                    tt0 = tt0.Next;
                            }
                        }
                        Pullenti.Ner.ReferentToken rt = this.TryAttachOrg(tt0, ad, AttachType.High, null, false, 0, -1);
                        if (rt == null) 
                            break;
                        if (!org.CanBeEquals(rt.Referent, Pullenti.Ner.Core.ReferentsEqualType.ForMerging)) 
                            break;
                        if (rt.EndToken != br.EndToken.Previous) 
                            break;
                        if (!attachForNewOrg && !org.CanBeEquals(rt.Referent, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            break;
                        if (attachTyp == AttachType.Normal) 
                        {
                            if (!oldName && !OrganizationReferent.CanBeSecondDefinition(org, rt.Referent as OrganizationReferent)) 
                                break;
                            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t.Next, true, null);
                            if (typ != null && typ.IsDouterOrg) 
                                break;
                        }
                        org.MergeSlots(rt.Referent, true);
                        t = (t1 = br.EndToken);
                        continue;
                    }
                    break;
                }
                else if (attachTyp == AttachType.ExtOntology && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br == null) 
                        break;
                    string nam = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                    if (nam != null) 
                        org.AddName(nam, true, br.BeginToken.Next);
                    Pullenti.Ner.ReferentToken rt1 = this.TryAttachOrg(t.Next, ad, AttachType.High, null, true, 0, -1);
                    if (rt1 != null && rt1.EndToken.Next == br.EndToken) 
                    {
                        org.MergeSlots(rt1.Referent, true);
                        t = (t1 = br.EndToken);
                    }
                }
                else 
                    break;
            }
            if (t != null && (t.WhitespacesBeforeCount < 2) && ((ki == OrganizationKind.Undefined || ki == OrganizationKind.Bank))) 
            {
                Pullenti.Ner.Org.Internal.OrgItemTypeToken ty1 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t, false, null);
                if (ty1 != null && ty1.Root != null && ty1.Root.IsPurePrefix) 
                {
                    if (t.Kit.RecurseLevel > 2) 
                        return null;
                    t.Kit.RecurseLevel++;
                    Pullenti.Ner.ReferentToken rt22 = this.TryAttachOrg(t, ad, AttachType.Normal, null, false, 0, -1);
                    t.Kit.RecurseLevel--;
                    if (rt22 == null) 
                    {
                        org.AddType(ty1, false);
                        t1 = ty1.EndToken;
                    }
                }
            }
            return t1;
        }
        void CorrectOwnerBefore(Pullenti.Ner.ReferentToken res)
        {
            if (res == null) 
                return;
            if ((res.Referent as OrganizationReferent).Kind == OrganizationKind.Press) 
            {
                if (res.BeginToken.IsValue("КОРРЕСПОНДЕНТ", null) && res.BeginToken != res.EndToken) 
                    res.BeginToken = res.BeginToken.Next;
            }
            OrganizationReferent org = res.Referent as OrganizationReferent;
            if (org.Higher != null || org.m_TempParentOrg != null) 
                return;
            OrganizationReferent hiBefore = null;
            int couBefore = 0;
            Pullenti.Ner.Token t0 = null;
            for (Pullenti.Ner.Token t = res.BeginToken.Previous; t != null; t = t.Previous) 
            {
                couBefore += t.WhitespacesAfterCount;
                if (t.IsChar(',')) 
                {
                    couBefore += 5;
                    continue;
                }
                else if (t.IsValue("ПРИ", null)) 
                    return;
                if (t is Pullenti.Ner.ReferentToken) 
                {
                    if ((((hiBefore = t.GetReferent() as OrganizationReferent))) != null) 
                        t0 = t;
                }
                break;
            }
            if (t0 == null) 
                return;
            if (!Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(hiBefore, org, false)) 
                return;
            if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org, hiBefore, false)) 
                return;
            OrganizationReferent hiAfter = null;
            int couAfter = 0;
            for (Pullenti.Ner.Token t = res.EndToken.Next; t != null; t = t.Next) 
            {
                couBefore += t.WhitespacesBeforeCount;
                if (t.IsChar(',') || t.IsValue("ПРИ", null)) 
                {
                    couAfter += 5;
                    continue;
                }
                if (t is Pullenti.Ner.ReferentToken) 
                {
                    hiAfter = t.GetReferent() as OrganizationReferent;
                    break;
                }
                Pullenti.Ner.ReferentToken rt = this.TryAttachOrg(t, null, AttachType.Normal, null, false, 0, -1);
                if (rt != null) 
                    hiAfter = rt.Referent as OrganizationReferent;
                break;
            }
            if (hiAfter != null) 
            {
                if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(hiAfter, org, false)) 
                {
                    if (couBefore >= couAfter) 
                        return;
                }
            }
            if (org.Kind == hiBefore.Kind && org.Kind != OrganizationKind.Undefined) 
            {
                if (org.Kind != OrganizationKind.Department & org.Kind != OrganizationKind.Govenment) 
                    return;
            }
            org.Higher = hiBefore;
            res.BeginToken = t0;
        }
        Pullenti.Ner.ReferentToken CheckOwnership(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.ReferentToken res = null;
            OrganizationReferent org = t.GetReferent() as OrganizationReferent;
            if (org == null) 
                return null;
            Pullenti.Ner.Token tt0 = t;
            for (; t != null; ) 
            {
                Pullenti.Ner.Token tt = t.Next;
                bool always = false;
                bool br = false;
                if (tt != null && tt.Morph.Class.IsPreposition) 
                {
                    if (tt.IsValue("ПРИ", null)) 
                        always = true;
                    else if (tt.IsValue("В", null)) 
                    {
                    }
                    else 
                        break;
                    tt = tt.Next;
                }
                if ((tt != null && tt.IsChar('(') && (tt.Next is Pullenti.Ner.ReferentToken)) && tt.Next.Next != null && tt.Next.Next.IsChar(')')) 
                {
                    br = true;
                    tt = tt.Next;
                }
                if (tt is Pullenti.Ner.ReferentToken) 
                {
                    OrganizationReferent org2 = tt.GetReferent() as OrganizationReferent;
                    if (org2 != null) 
                    {
                        bool ok = Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org2, org, false);
                        if (always || ok) 
                            ok = true;
                        else if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org2, org, true)) 
                        {
                            Pullenti.Ner.Token t0 = t.Previous;
                            if (t0 != null && t0.IsChar(',')) 
                                t0 = t0.Previous;
                            Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("PERSON", t0);
                            if (rt != null && rt.Referent.TypeName == "PERSONPROPERTY" && rt.Morph.Number == Pullenti.Morph.MorphNumber.Singular) 
                                ok = true;
                        }
                        if (ok && ((org.Higher == null || org.Higher.CanBeEquals(org2, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)))) 
                        {
                            org.Higher = org2;
                            if (br) 
                                tt = tt.Next;
                            if (org.Higher == org2) 
                            {
                                if (res == null) 
                                    res = new Pullenti.Ner.ReferentToken(org, t, tt) { Morph = tt0.Morph };
                                else 
                                    res.EndToken = tt;
                                t = tt;
                                if (org.GeoObjects.Count == 0) 
                                {
                                    Pullenti.Ner.Token ttt = t.Next;
                                    if (ttt != null && ttt.IsValue("В", null)) 
                                        ttt = ttt.Next;
                                    if (this.IsGeo(ttt, false) != null) 
                                    {
                                        org.AddGeoObject(ttt);
                                        res.EndToken = ttt;
                                        t = ttt;
                                    }
                                }
                                org = org2;
                                continue;
                            }
                        }
                        if (org.Higher != null && org.Higher.Higher == null && Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org2, org.Higher, false)) 
                        {
                            org.Higher.Higher = org2;
                            res = new Pullenti.Ner.ReferentToken(org, t, tt);
                            if (br) 
                                res.EndToken = tt.Next;
                            return res;
                        }
                        if ((org.Higher != null && org2.Higher == null && Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org.Higher, org2, false)) && Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org2, org, false)) 
                        {
                            org2.Higher = org.Higher;
                            org.Higher = org2;
                            res = new Pullenti.Ner.ReferentToken(org, t, tt);
                            if (br) 
                                res.EndToken = tt.Next;
                            return res;
                        }
                    }
                }
                break;
            }
            if (res != null) 
                return res;
            if (org.Kind == OrganizationKind.Department && org.Higher == null && org.m_TempParentOrg == null) 
            {
                int cou = 0;
                for (Pullenti.Ner.Token tt = tt0.Previous; tt != null; tt = tt.Previous) 
                {
                    if (tt.IsNewlineAfter) 
                        cou += 10;
                    if ((++cou) > 100) 
                        break;
                    OrganizationReferent org0 = tt.GetReferent() as OrganizationReferent;
                    if (org0 == null) 
                        continue;
                    List<OrganizationReferent> tmp = new List<OrganizationReferent>();
                    for (; org0 != null; org0 = org0.Higher) 
                    {
                        if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org0, org, false)) 
                        {
                            org.Higher = org0;
                            break;
                        }
                        if (org0.Kind != OrganizationKind.Department) 
                            break;
                        if (tmp.Contains(org0)) 
                            break;
                        tmp.Add(org0);
                    }
                    break;
                }
            }
            return null;
        }
        public override Pullenti.Ner.ReferentToken ProcessOntologyItem(Pullenti.Ner.Token begin)
        {
            if (begin == null) 
                return null;
            Pullenti.Ner.ReferentToken rt = this.TryAttachOrg(begin, null, AttachType.ExtOntology, null, begin.Previous != null, 0, -1);
            if (rt != null) 
            {
                OrganizationReferent r = rt.Referent as OrganizationReferent;
                if (r.Higher == null && rt.EndToken.Next != null) 
                {
                    OrganizationReferent h = rt.EndToken.Next.GetReferent() as OrganizationReferent;
                    if (h != null) 
                    {
                        if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(h, r, true) || !Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(r, h, true)) 
                        {
                            r.Higher = h;
                            rt.EndToken = rt.EndToken.Next;
                        }
                    }
                }
                if (rt.BeginToken != begin) 
                {
                    string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(begin, rt.BeginToken.Previous, Pullenti.Ner.Core.GetTextAttr.No);
                    if (!string.IsNullOrEmpty(nam)) 
                    {
                        OrganizationReferent org0 = new OrganizationReferent();
                        org0.AddName(nam, true, begin);
                        org0.Higher = r;
                        rt = new Pullenti.Ner.ReferentToken(org0, begin, rt.EndToken);
                    }
                }
                return rt;
            }
            Pullenti.Ner.Token t = begin;
            Pullenti.Ner.Token et = begin;
            for (; t != null; t = t.Next) 
            {
                if (t.IsCharOf(",;")) 
                    break;
                et = t;
            }
            string name = Pullenti.Ner.Core.MiscHelper.GetTextValue(begin, et, Pullenti.Ner.Core.GetTextAttr.No);
            if (string.IsNullOrEmpty(name)) 
                return null;
            OrganizationReferent org = new OrganizationReferent();
            org.AddName(name, true, begin);
            return new Pullenti.Ner.ReferentToken(org, begin, et);
        }
        static bool m_Inited = false;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Ner.Org.Internal.MetaOrganization.Initialize();
            try 
            {
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                _initSport();
                _initPolitic();
                Pullenti.Ner.Org.Internal.OrgItemTypeToken.Initialize();
                Pullenti.Ner.Org.Internal.OrgItemEngItem.Initialize();
                Pullenti.Ner.Org.Internal.OrgItemNameToken.Initialize();
                Pullenti.Ner.Org.Internal.OrgGlobal.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new OrganizationAnalyzer());
        }
        Pullenti.Ner.ReferentToken TryAttachPoliticParty(Pullenti.Ner.Token t, OrgAnalyzerData ad, bool onlyAbbrs = false)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            Pullenti.Ner.Core.TerminToken nameTok = null;
            Pullenti.Ner.Token root = null;
            List<Pullenti.Ner.Core.TerminToken> prevToks = null;
            int prevWords = 0;
            Pullenti.Ner.ReferentToken geo = null;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = t;
            double coef = (double)0;
            int wordsAfter = 0;
            bool isFraction = false;
            bool isPolitic = false;
            for (; t != null; t = t.Next) 
            {
                if (t != t0 && t.IsNewlineBefore) 
                    break;
                if (onlyAbbrs) 
                    break;
                if (t.IsHiphen) 
                {
                    if (prevToks == null) 
                        return null;
                    continue;
                }
                Pullenti.Ner.Core.TerminToken tokN = m_PoliticNames.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tokN != null) 
                {
                    if (!t.Chars.IsAllLower) 
                        break;
                    t1 = tokN.EndToken;
                }
                Pullenti.Ner.Core.TerminToken tok = m_PoliticPrefs.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok == null) 
                {
                    if (t.Morph.Class.IsAdjective) 
                    {
                        Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("GEO", t);
                        if (rt != null) 
                        {
                            geo = rt;
                            t1 = (t = rt.EndToken);
                            coef += 0.5;
                            continue;
                        }
                    }
                    if (t.EndChar < t1.EndChar) 
                        continue;
                    break;
                }
                if (tok.Termin.Tag != null && tok.Termin.Tag2 != null) 
                {
                    if (t.EndChar < t1.EndChar) 
                        continue;
                    break;
                }
                if (tok.Termin.Tag == null && tok.Termin.Tag2 == null) 
                    isPolitic = true;
                if (prevToks == null) 
                    prevToks = new List<Pullenti.Ner.Core.TerminToken>();
                prevToks.Add(tok);
                if (tok.Termin.Tag == null) 
                {
                    coef += 1;
                    prevWords++;
                }
                else if (tok.Morph.Class.IsAdjective) 
                    coef += 0.5;
                t = tok.EndToken;
                if (t.EndChar > t1.EndChar) 
                    t1 = t;
            }
            if (t == null) 
                return null;
            if (t.IsValue("ПАРТИЯ", null) || t.IsValue("ФРОНТ", null) || t.IsValue("ГРУППИРОВКА", null)) 
            {
                if (!t.IsValue("ПАРТИЯ", null)) 
                    isPolitic = true;
                root = t;
                coef += 0.5;
                if (t.Chars.IsCapitalUpper && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                    coef += 0.5;
                t1 = t;
                t = t.Next;
            }
            else if (t.IsValue("ФРАКЦИЯ", null)) 
            {
                root = (t1 = t);
                isFraction = true;
                if (t.Next != null && (t.Next.GetReferent() is OrganizationReferent)) 
                    coef += 2;
                else 
                    return null;
            }
            Pullenti.Ner.Core.BracketSequenceToken br = null;
            if ((((nameTok = m_PoliticNames.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No)))) != null && !t.Chars.IsAllLower) 
            {
                coef += 0.5;
                isPolitic = true;
                if (!t.Chars.IsAllLower) 
                    coef += 0.5;
                if (nameTok.LengthChar > 10) 
                    coef += 0.5;
                else if (t.Chars.IsAllUpper) 
                    coef += 0.5;
                t1 = nameTok.EndToken;
                t = t1.Next;
            }
            else if ((((br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 10)))) != null) 
            {
                if (!Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                    return null;
                if ((((nameTok = m_PoliticNames.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No)))) != null) 
                    coef += 1.5;
                else if (onlyAbbrs) 
                    return null;
                else if (t.Next != null && t.Next.IsValue("О", null)) 
                    return null;
                else 
                    for (Pullenti.Ner.Token tt = t.Next; tt != null && tt.EndChar <= br.EndChar; tt = tt.Next) 
                    {
                        Pullenti.Ner.Core.TerminToken tok2 = m_PoliticPrefs.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok2 != null && tok2.Termin.Tag == null) 
                        {
                            if (tok2.Termin.Tag2 == null) 
                                isPolitic = true;
                            coef += 0.5;
                            wordsAfter++;
                        }
                        else if (m_PoliticSuffs.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                        {
                            coef += 0.5;
                            wordsAfter++;
                        }
                        else if (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                            coef += 0.5;
                        else if (tt is Pullenti.Ner.ReferentToken) 
                        {
                            coef = 0;
                            break;
                        }
                        else 
                        {
                            Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                            if ((mc == Pullenti.Morph.MorphClass.Verb || mc == Pullenti.Morph.MorphClass.Adverb || mc.IsPronoun) || mc.IsPersonalPronoun) 
                            {
                                coef = 0;
                                break;
                            }
                            if (mc.IsNoun || mc.IsUndefined) 
                                coef -= 0.5;
                        }
                    }
                t1 = br.EndToken;
                t = t1.Next;
            }
            else if (onlyAbbrs) 
                return null;
            else if (root != null) 
            {
                for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
                {
                    if (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                        break;
                    if (tt.WhitespacesBeforeCount > 2) 
                        break;
                    if (tt.Morph.Class.IsPreposition) 
                    {
                        if (tt != root.Next) 
                            break;
                        continue;
                    }
                    if (tt.IsAnd) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun, 0, null);
                        if (npt2 != null && m_PoliticSuffs.TryParse(npt2.EndToken, Pullenti.Ner.Core.TerminParseAttr.No) != null && npt2.EndToken.Chars == tt.Previous.Chars) 
                            continue;
                        break;
                    }
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun, 0, null);
                    if (npt == null) 
                        break;
                    if (npt.Noun.IsValue("ПАРТИЯ", null) || npt.Noun.IsValue("ФРОНТ", null)) 
                        break;
                    double co = (double)0;
                    for (Pullenti.Ner.Token ttt = tt; ttt != null && ttt.EndChar <= npt.EndChar; ttt = ttt.Next) 
                    {
                        Pullenti.Ner.Core.TerminToken tok2 = m_PoliticPrefs.TryParse(ttt, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok2 != null && tok2.Termin.Tag == null) 
                        {
                            if (tok2.Termin.Tag2 == null) 
                                isPolitic = true;
                            co += 0.5;
                            wordsAfter++;
                        }
                        else if (m_PoliticSuffs.TryParse(ttt, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                        {
                            co += 0.5;
                            wordsAfter++;
                        }
                        else if (ttt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                            co += 0.5;
                    }
                    if (co == 0) 
                    {
                        if (!npt.Morph.Case.IsGenitive) 
                            break;
                        Pullenti.Ner.Core.TerminToken lastSuf = m_PoliticSuffs.TryParse(tt.Previous, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (((wordsAfter > 0 && npt.EndToken.Chars == tt.Previous.Chars)) || ((lastSuf != null && lastSuf.Termin.Tag != null)) || ((tt.Previous == root && npt.EndToken.Chars.IsAllLower && npt.Morph.Number == Pullenti.Morph.MorphNumber.Plural) && root.Chars.IsCapitalUpper)) 
                        {
                            Pullenti.Ner.ReferentToken pp = tt.Kit.ProcessReferent("PERSON", tt);
                            if (pp != null) 
                                break;
                            wordsAfter++;
                        }
                        else 
                            break;
                    }
                    t1 = (tt = npt.EndToken);
                    t = t1.Next;
                    coef += co;
                }
            }
            if (t != null && (t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && (t.WhitespacesBeforeCount < 3)) 
            {
                t1 = t;
                coef += 0.5;
            }
            for (Pullenti.Ner.Token tt = t0.Previous; tt != null; tt = tt.Previous) 
            {
                if (!(tt is Pullenti.Ner.TextToken)) 
                {
                    OrganizationReferent org1 = tt.GetReferent() as OrganizationReferent;
                    if (org1 != null && org1.ContainsProfile(OrgProfile.Policy)) 
                        coef += 0.5;
                    continue;
                }
                if (!tt.Chars.IsLetter) 
                    continue;
                if (tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction) 
                    continue;
                if (m_PoliticPrefs.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                {
                    coef += 0.5;
                    if (tt.IsValue("ФРАКЦИЯ", null)) 
                        coef += 0.5;
                }
                else 
                    break;
            }
            if (coef < 1) 
                return null;
                {
                    if (root == null) 
                    {
                        if (nameTok == null && br == null) 
                            return null;
                    }
                    else if ((nameTok == null && wordsAfter == 0 && br == null) && !isFraction) 
                    {
                        if ((coef < 2) || prevWords == 0) 
                            return null;
                    }
                }
            OrganizationReferent org = new OrganizationReferent();
            if (br != null && nameTok != null && (nameTok.EndChar < br.EndToken.Previous.EndChar)) 
                nameTok = null;
            if (nameTok != null) 
                isPolitic = true;
            if (isFraction) 
            {
                org.AddProfile(OrgProfile.Policy);
                org.AddProfile(OrgProfile.Unit);
            }
            else if (isPolitic) 
            {
                org.AddProfile(OrgProfile.Policy);
                org.AddProfile(OrgProfile.Union);
            }
            else 
                org.AddProfile(OrgProfile.Union);
            if (nameTok != null) 
            {
                isPolitic = true;
                org.AddName(nameTok.Termin.CanonicText, true, null);
                if (nameTok.Termin.AdditionalVars != null) 
                {
                    foreach (Pullenti.Ner.Core.Termin v in nameTok.Termin.AdditionalVars) 
                    {
                        org.AddName(v.CanonicText, true, null);
                    }
                }
                if (nameTok.Termin.Acronym != null) 
                {
                    Pullenti.Ner.Geo.GeoReferent geo1 = nameTok.Termin.Tag as Pullenti.Ner.Geo.GeoReferent;
                    if (geo1 == null) 
                        org.AddName(nameTok.Termin.Acronym, true, null);
                    else if (geo != null) 
                    {
                        if (geo1.CanBeEquals(geo.Referent, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            org.AddName(nameTok.Termin.Acronym, true, null);
                    }
                    else if (t1.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                    {
                        if (geo1.CanBeEquals(t1.GetReferent(), Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            org.AddName(nameTok.Termin.Acronym, true, null);
                    }
                    else if (nameTok.BeginToken == nameTok.EndToken && nameTok.BeginToken.IsValue(nameTok.Termin.Acronym, null)) 
                    {
                        org.AddName(nameTok.Termin.Acronym, true, null);
                        Pullenti.Ner.ReferentToken rtg = new Pullenti.Ner.ReferentToken(geo1.Clone(), nameTok.BeginToken, nameTok.EndToken);
                        rtg.SetDefaultLocalOnto(t0.Kit.Processor);
                        org.AddGeoObject(rtg);
                    }
                }
            }
            else if (br != null) 
            {
                string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken, br.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                org.AddName(nam, true, null);
                if (root == null) 
                {
                    string nam2 = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken, br.EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                    if (nam2 != nam) 
                        org.AddName(nam, true, null);
                }
            }
            if (root != null) 
            {
                Pullenti.Ner.Token typ1 = root;
                if (geo != null) 
                    typ1 = geo.BeginToken;
                if (prevToks != null) 
                {
                    foreach (Pullenti.Ner.Core.TerminToken p in prevToks) 
                    {
                        if (p.Termin.Tag == null) 
                        {
                            if (p.BeginChar < typ1.BeginChar) 
                                typ1 = p.BeginToken;
                            break;
                        }
                    }
                }
                string typ = Pullenti.Ner.Core.MiscHelper.GetTextValue(typ1, root, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                if (typ != null) 
                {
                    if (br == null) 
                    {
                        string nam = null;
                        Pullenti.Ner.Token t2 = t1;
                        if (t2.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                            t2 = t2.Previous;
                        if (t2.EndChar > root.EndChar) 
                        {
                            nam = string.Format("{0} {1}", typ, Pullenti.Ner.Core.MiscHelper.GetTextValue(root.Next, t2, Pullenti.Ner.Core.GetTextAttr.No));
                            org.AddName(nam, true, null);
                        }
                    }
                    if (org.Names.Count == 0 && typ1 != root) 
                        org.AddName(typ, true, null);
                    else 
                        org.AddTypeStr(typ.ToLower());
                }
                if (isFraction && (t1.Next is Pullenti.Ner.ReferentToken)) 
                {
                    org.AddTypeStr("фракция");
                    t1 = t1.Next;
                    org.Higher = t1.GetReferent() as OrganizationReferent;
                    if (t1.Next != null && t1.Next.IsValue("В", null) && (t1.Next.Next is Pullenti.Ner.ReferentToken)) 
                    {
                        OrganizationReferent oo = t1.Next.Next.GetReferent() as OrganizationReferent;
                        if (oo != null && oo.Kind == OrganizationKind.Govenment) 
                        {
                            t1 = t1.Next.Next;
                            org.AddSlot(OrganizationReferent.ATTR_MISC, oo, false, 0);
                        }
                        else if (t1.Next.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                        {
                            t1 = t1.Next.Next;
                            org.AddSlot(OrganizationReferent.ATTR_MISC, t1.GetReferent(), false, 0);
                        }
                    }
                }
            }
            if (geo != null) 
                org.AddGeoObject(geo);
            else if (t1.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                org.AddGeoObject(t1.GetReferent());
            return new Pullenti.Ner.ReferentToken(org, t0, t1);
        }
        static void _initPolitic()
        {
            m_PoliticPrefs = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"либеральный", "либерал", "лейбористский", "демократический", "коммунистрический", "большевистский", "социальный", "социал", "национал", "националистическая", "свободный", "радикальный", "леворадикальный", "радикал", "революционная", "левый", "правый", "социалистический", "рабочий", "трудовой", "республиканский", "народный", "аграрный", "монархический", "анархический", "прогрессивый", "прогрессистский", "консервативный", "гражданский", "фашистский", "марксистский", "ленинский", "маоистский", "имперский", "славянский", "анархический", "баскский", "конституционный", "пиратский", "патриотический", "русский"}) 
            {
                m_PoliticPrefs.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()));
            }
            foreach (string s in new string[] {"объединенный", "всероссийский", "общероссийский", "христианский", "независимый", "альтернативный"}) 
            {
                m_PoliticPrefs.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag2 = s });
            }
            foreach (string s in new string[] {"политический", "правящий", "оппозиционный", "запрешенный", "террористический", "запрещенный", "экстремистский"}) 
            {
                m_PoliticPrefs.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag = s });
            }
            foreach (string s in new string[] {"активист", "член", "руководство", "лидер", "глава", "демонстрация", "фракция", "съезд", "пленум", "террорист", "парламент", "депутат", "парламентарий", "оппозиция", "дума", "рада"}) 
            {
                m_PoliticPrefs.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag = s, Tag2 = s });
            }
            m_PoliticSuffs = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"коммунист", "социалист", "либерал", "республиканец", "националист", "радикал", "лейборист", "анархист", "патриот", "консерватор", "левый", "правый", "новый", "зеленые", "демократ", "фашист", "защитник", "труд", "равенство", "прогресс", "жизнь", "мир", "родина", "отечество", "отчизна", "республика", "революция", "революционер", "народовластие", "фронт", "сила", "платформа", "воля", "справедливость", "преображение", "преобразование", "солидарность", "управление", "демократия", "народ", "гражданин", "предприниматель", "предпринимательство", "бизнес", "пенсионер", "христианин"}) 
            {
                m_PoliticSuffs.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()));
            }
            foreach (string s in new string[] {"реформа", "свобода", "единство", "развитие", "освобождение", "любитель", "поддержка", "возрождение", "независимость"}) 
            {
                m_PoliticSuffs.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag = s });
            }
            m_PoliticNames = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"Республиканская партия", "Демократическая партия;Демпартия", "Христианско демократический союз;ХДС", "Свободная демократическая партия;СвДП", "ЯБЛОКО", "ПАРНАС", "ПАМЯТЬ", "Движение против нелегальной иммиграции;ДПНИ", "НАЦИОНАЛ БОЛЬШЕВИСТСКАЯ ПАРТИЯ;НБП", "НАЦИОНАЛЬНЫЙ ФРОНТ;НАЦФРОНТ", "Национальный патриотический фронт;НПФ", "Батькивщина;Батькiвщина", "НАРОДНАЯ САМООБОРОНА", "Гражданская платформа", "Народная воля", "Славянский союз", "ПРАВЫЙ СЕКТОР", "ПЕГИДА;PEGIDA", "Венгерский гражданский союз;ФИДЕС", "БЛОК ЮЛИИ ТИМОШЕНКО;БЮТ", "Аль Каида;Аль Каеда;Аль Кайда;Al Qaeda;Al Qaida", "Талибан;движение талибан", "Бригады мученников Аль Аксы", "Хезболла;Хезбалла;Хизбалла", "Народный фронт освобождения палестины;НФОП", "Организация освобождения палестины;ООП", "Союз исламского джихада;Исламский джихад", "Аль-Джихад;Египетский исламский джихад", "Братья-мусульмане;Аль Ихван альМуслимун", "ХАМАС", "Движение за освобождение Палестины;ФАТХ", "Фронт Аль Нусра;Аль Нусра", "Джабхат ан Нусра"}) 
            {
                string[] pp = s.ToUpper().Split(';');
                Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin(pp[0]) { Tag = OrgProfile.Policy };
                for (int i = 0; i < pp.Length; i++) 
                {
                    if ((pp[i].Length < 5) && t.Acronym == null) 
                    {
                        t.Acronym = pp[i];
                        if (t.Acronym.EndsWith("Р") || t.Acronym.EndsWith("РФ")) 
                            t.Tag = Pullenti.Ner.Geo.Internal.MiscLocationHelper.GetGeoReferentByName("RU");
                        else if (t.Acronym.EndsWith("У")) 
                            t.Tag = Pullenti.Ner.Geo.Internal.MiscLocationHelper.GetGeoReferentByName("UA");
                        else if (t.Acronym.EndsWith("СС")) 
                            t.Tag = Pullenti.Ner.Geo.Internal.MiscLocationHelper.GetGeoReferentByName("СССР");
                    }
                    else 
                        t.AddVariant(pp[i], false);
                }
                m_PoliticNames.Add(t);
            }
        }
        static Pullenti.Ner.Core.TerminCollection m_PoliticPrefs;
        static Pullenti.Ner.Core.TerminCollection m_PoliticSuffs;
        static Pullenti.Ner.Core.TerminCollection m_PoliticNames;
        const int MaxOrgName = 200;
        Pullenti.Ner.ReferentToken TryAttachOrg(Pullenti.Ner.Token t, OrgAnalyzerData ad, AttachType attachTyp, Pullenti.Ner.Org.Internal.OrgItemTypeToken multTyp = null, bool isAdditionalAttach = false, int level = 0, int step = -1)
        {
            if (level > 2 || t == null) 
                return null;
            if (t.Chars.IsLatinLetter && Pullenti.Ner.Core.MiscHelper.IsEngArticle(t)) 
            {
                Pullenti.Ner.ReferentToken re = this.TryAttachOrg(t.Next, ad, attachTyp, multTyp, isAdditionalAttach, level, step);
                if (re != null) 
                {
                    re.BeginToken = t;
                    return re;
                }
            }
            OrganizationReferent org = null;
            List<Pullenti.Ner.Org.Internal.OrgItemTypeToken> types = null;
            if (multTyp != null) 
            {
                types = new List<Pullenti.Ner.Org.Internal.OrgItemTypeToken>();
                types.Add(multTyp);
            }
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = t;
            List<Pullenti.Ner.Core.IntOntologyToken> otExLi = null;
            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ = null;
            bool hiph = false;
            bool specWordBefore = false;
            bool ok;
            bool inBrackets = false;
            Pullenti.Ner.ReferentToken rt0 = null;
            for (; t != null; t = t.Next) 
            {
                if (t.GetReferent() is OrganizationReferent) 
                    break;
                rt0 = this.AttachGlobalOrg(t, attachTyp, ad, null);
                if ((rt0 == null && typ != null && typ.Geo != null) && typ.BeginToken.Next == typ.EndToken) 
                {
                    rt0 = this.AttachGlobalOrg(typ.EndToken, attachTyp, ad, typ.Geo);
                    if (rt0 != null) 
                        rt0.BeginToken = typ.BeginToken;
                }
                if (rt0 != null) 
                {
                    if (attachTyp == AttachType.Multiple) 
                    {
                        if (types == null || types.Count == 0) 
                            return null;
                        if (!Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypeAccords(rt0.Referent as OrganizationReferent, types[0])) 
                            return null;
                        (rt0.Referent as OrganizationReferent).AddType(types[0], false);
                        if ((rt0.BeginToken.BeginChar - types[0].EndToken.Next.EndChar) < 3) 
                            rt0.BeginToken = types[0].BeginToken;
                        break;
                    }
                    if (typ != null && !typ.EndToken.Morph.Class.IsVerb) 
                    {
                        if (_isMvdOrg(rt0.Referent as OrganizationReferent) != null && typ.Typ != null && typ.Typ.Contains("служба")) 
                        {
                            rt0 = null;
                            break;
                        }
                        if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypeAccords(rt0.Referent as OrganizationReferent, typ)) 
                        {
                            rt0.BeginToken = typ.BeginToken;
                            (rt0.Referent as OrganizationReferent).AddType(typ, false);
                        }
                    }
                    break;
                }
                if (t.IsHiphen) 
                {
                    if (t == t0 || types == null) 
                    {
                        if (otExLi != null) 
                            break;
                        return null;
                    }
                    if ((typ != null && typ.Root != null && typ.Root.CanHasNumber) && (t.Next is Pullenti.Ner.NumberToken)) 
                    {
                    }
                    else 
                        hiph = true;
                    continue;
                }
                if (ad != null && otExLi == null) 
                {
                    bool ok1 = false;
                    Pullenti.Ner.Token tt = t;
                    if (t.InnerBool) 
                        ok1 = true;
                    else if (t.Chars.IsAllLower) 
                    {
                    }
                    else if (t.Chars.IsLetter) 
                        ok1 = true;
                    else if (t.Previous != null && Pullenti.Ner.Core.BracketHelper.IsBracket(t.Previous, false)) 
                        ok1 = true;
                    else if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false) && t.Next != null) 
                    {
                        ok1 = true;
                        tt = t.Next;
                    }
                    if (ok1 && tt != null) 
                    {
                        otExLi = ad.LocOrgs.TryAttach(tt, null, false);
                        if (otExLi == null && t.Kit.Ontology != null) 
                        {
                            if ((((otExLi = t.Kit.Ontology.AttachToken(OrganizationReferent.OBJ_TYPENAME, tt)))) != null) 
                            {
                            }
                        }
                        if (otExLi == null && tt.LengthChar == 2 && tt.Chars.IsAllUpper) 
                        {
                            otExLi = ad.LocalOntology.TryAttach(tt, null, false);
                            if (otExLi != null) 
                            {
                                if (tt.Kit.Sofa.Text.Length > 300) 
                                    otExLi = null;
                            }
                        }
                    }
                    if (otExLi != null) 
                        t.InnerBool = true;
                }
                if ((step >= 0 && !t.InnerBool && t == t0) && (t is Pullenti.Ner.TextToken)) 
                    typ = null;
                else 
                {
                    typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t, attachTyp == AttachType.ExtOntology, ad);
                    if (typ == null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t.Next, attachTyp == AttachType.ExtOntology, ad);
                            if (typ != null && typ.EndToken == br.EndToken.Previous && ((Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(br.EndToken.Next, true, false) || t.IsChar('(')))) 
                            {
                                typ.EndToken = br.EndToken;
                                typ.BeginToken = t;
                            }
                            else 
                                typ = null;
                        }
                    }
                }
                if (typ == null) 
                    break;
                if (types == null) 
                {
                    if ((((typ.Typ == "главное управление" || typ.Typ == "главное территориальное управление" || typ.Typ == "головне управління") || typ.Typ == "головне територіальне управління" || typ.Typ == "пограничное управление")) && otExLi != null) 
                        break;
                    types = new List<Pullenti.Ner.Org.Internal.OrgItemTypeToken>();
                    t0 = typ.BeginToken;
                    if (typ.IsNotTyp && typ.EndToken.Next != null) 
                        t0 = typ.EndToken.Next;
                    if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.CheckOrgSpecialWordBefore(typ.BeginToken.Previous)) 
                        specWordBefore = true;
                }
                else 
                {
                    ok = true;
                    foreach (Pullenti.Ner.Org.Internal.OrgItemTypeToken ty in types) 
                    {
                        if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticTT(ty, typ)) 
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (!ok) 
                        break;
                    if (typ.IsDep) 
                        break;
                    if (inBrackets) 
                        break;
                    Pullenti.Ner.Org.Internal.OrgItemTypeToken typ0 = _lastTyp(types);
                    if (hiph && ((t.WhitespacesBeforeCount > 0 && ((typ0 != null && typ0.IsDoubtRootWord))))) 
                        break;
                    if (typ.EndToken == typ.BeginToken) 
                    {
                        if (typ.IsValue("ОРГАНИЗАЦИЯ", "ОРГАНІЗАЦІЯ") || typ.IsValue("УПРАВЛІННЯ", "")) 
                            break;
                    }
                    if (typ0.Typ == "банк" && typ.Root != null && typ.Root.Typ == Pullenti.Ner.Org.Internal.OrgItemTypeTyp.Prefix) 
                    {
                        Pullenti.Ner.ReferentToken rt = this.TryAttachOrg(typ.BeginToken, ad, attachTyp, null, false, 0, -1);
                        if (rt != null && rt.Referent.ToString().Contains("Сбербанк")) 
                            return null;
                    }
                    if (typ0.IsDep || typ0.Typ == "департамент") 
                        break;
                    if ((typ0.Root != null && typ0.Root.IsPurePrefix && typ.Root != null) && !typ.Root.IsPurePrefix && !typ.BeginToken.Chars.IsAllLower) 
                    {
                        if (typ0.Typ.Contains("НИИ")) 
                            break;
                    }
                    bool pref0 = typ0.Root != null && typ0.Root.IsPurePrefix;
                    bool pref = typ.Root != null && typ.Root.IsPurePrefix;
                    if (!pref0 && !pref) 
                    {
                        if (typ0.Name != null && typ0.Name.Length != typ0.Typ.Length) 
                        {
                            if (t.WhitespacesBeforeCount > 1) 
                                break;
                        }
                        if (!typ0.Morph.Case.IsUndefined && !typ.Morph.Case.IsUndefined) 
                        {
                            if (!((typ0.Morph.Case & typ.Morph.Case)).IsNominative && !hiph) 
                            {
                                if (!typ.Morph.Case.IsNominative) 
                                    break;
                            }
                        }
                        if (typ0.Morph.Number != Pullenti.Morph.MorphNumber.Undefined && typ.Morph.Number != Pullenti.Morph.MorphNumber.Undefined) 
                        {
                            if (((typ0.Morph.Number & typ.Morph.Number)) == Pullenti.Morph.MorphNumber.Undefined) 
                                break;
                        }
                    }
                    if (!pref0 && pref && !hiph) 
                    {
                        bool nom = false;
                        foreach (Pullenti.Morph.MorphBaseInfo m in typ.Morph.Items) 
                        {
                            if (m.Number == Pullenti.Morph.MorphNumber.Singular && m.Case.IsNominative) 
                            {
                                nom = true;
                                break;
                            }
                        }
                        if (!nom) 
                        {
                            if (Pullenti.Morph.LanguageHelper.EndsWith(typ0.Typ, "фракция") || Pullenti.Morph.LanguageHelper.EndsWith(typ0.Typ, "фракція") || typ0.Typ == "банк") 
                            {
                            }
                            else 
                                break;
                        }
                    }
                    foreach (Pullenti.Ner.Org.Internal.OrgItemTypeToken ty in types) 
                    {
                        if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticTT(ty, typ)) 
                            return null;
                    }
                }
                types.Add(typ);
                inBrackets = false;
                if (typ.Name != null) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(typ.BeginToken.Previous, true, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(typ.EndToken.Next, false, null, false)) 
                    {
                        typ.BeginToken = typ.BeginToken.Previous;
                        typ.EndToken = typ.EndToken.Next;
                        if (typ.BeginToken.EndChar < t0.BeginChar) 
                            t0 = typ.BeginToken;
                        inBrackets = true;
                    }
                }
                t = typ.EndToken;
                hiph = false;
            }
            if ((types == null && otExLi == null && ((attachTyp == AttachType.Normal || attachTyp == AttachType.NormalAfterDep))) && rt0 == null) 
            {
                ok = false;
                if (!ok) 
                {
                    if (t0 != null && t0.Morph.Class.IsAdjective && t0.Next != null) 
                    {
                        if ((((rt0 = this.TryAttachOrg(t0.Next, ad, attachTyp, multTyp, isAdditionalAttach, level + 1, step)))) != null) 
                        {
                            if (rt0.BeginToken == t0) 
                                return rt0;
                        }
                    }
                    if (attachTyp == AttachType.Normal) 
                    {
                        if ((((rt0 = this.TryAttachOrgMed(t, ad)))) != null) 
                            return rt0;
                    }
                    if ((((t0.Kit.RecurseLevel < 4) && (t0 is Pullenti.Ner.TextToken) && t0.Previous != null) && t0.LengthChar > 2 && !t0.Chars.IsAllLower) && !t0.IsNewlineAfter && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t0)) 
                    {
                        typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t0.Next, false, null);
                        if (typ != null) 
                        {
                            t0.Kit.RecurseLevel++;
                            Pullenti.Ner.ReferentToken rrr = this.TryAttachOrg(t0.Next, ad, attachTyp, multTyp, isAdditionalAttach, level + 1, step);
                            t0.Kit.RecurseLevel--;
                            if (rrr == null) 
                            {
                                if (specWordBefore || t0.Previous.IsValue("ТЕРРИТОРИЯ", null)) 
                                {
                                    OrganizationReferent org0 = new OrganizationReferent();
                                    org0.AddType(typ, false);
                                    org0.AddName((t0 as Pullenti.Ner.TextToken).Term, false, t0);
                                    t1 = typ.EndToken;
                                    t1 = this.AttachTailAttributes(org0, t1.Next, ad, false, AttachType.Normal, false) ?? t1;
                                    return new Pullenti.Ner.ReferentToken(org0, t0, t1);
                                }
                            }
                        }
                    }
                    for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsAnd) 
                        {
                            if (tt == t) 
                                break;
                            continue;
                        }
                        if ((((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter && !tt.Chars.IsAllLower) && !tt.Chars.IsCapitalUpper && tt.LengthChar > 1) && (tt.WhitespacesAfterCount < 2)) 
                        {
                            Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                            if (mc.IsUndefined) 
                            {
                            }
                            else if (((tt.LengthChar < 5) && !mc.IsConjunction && !mc.IsPreposition) && !mc.IsNoun) 
                            {
                            }
                            else if ((tt.LengthChar <= 3 && (tt.Previous is Pullenti.Ner.TextToken) && tt.Previous.Chars.IsLetter) && !tt.Previous.Chars.IsAllUpper) 
                            {
                            }
                            else 
                                break;
                        }
                        else 
                            break;
                        if ((tt.Next is Pullenti.Ner.ReferentToken) && (tt.Next.GetReferent() is OrganizationReferent)) 
                        {
                            Pullenti.Ner.Token ttt = t.Previous;
                            if ((((ttt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter && !ttt.Chars.IsAllLower) && !ttt.Chars.IsCapitalUpper && ttt.LengthChar > 1) && ttt.GetMorphClassInDictionary().IsUndefined && (ttt.WhitespacesAfterCount < 2)) 
                                break;
                            Pullenti.Ner.Token tt0 = t;
                            for (t = t.Previous; t != null; t = t.Previous) 
                            {
                                if (!(t is Pullenti.Ner.TextToken) || t.WhitespacesAfterCount > 2) 
                                    break;
                                else if (t.IsAnd) 
                                {
                                }
                                else if ((t.Chars.IsLetter && !t.Chars.IsAllLower && !t.Chars.IsCapitalUpper) && t.LengthChar > 1 && t.GetMorphClassInDictionary().IsUndefined) 
                                    tt0 = t;
                                else 
                                    break;
                            }
                            string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt0, tt, Pullenti.Ner.Core.GetTextAttr.No);
                            if (nam == "СЭД" || nam == "ЕОСЗ") 
                                break;
                            OrganizationReferent own = tt.Next.GetReferent() as OrganizationReferent;
                            if (own.Profiles.Contains(OrgProfile.Unit)) 
                                break;
                            if (nam == "НК" || nam == "ГК") 
                                return new Pullenti.Ner.ReferentToken(own, t, tt.Next);
                            OrganizationReferent org0 = new OrganizationReferent();
                            org0.AddProfile(OrgProfile.Unit);
                            org0.AddName(nam, true, null);
                            if (nam.IndexOf(' ') > 0) 
                                org0.AddName(nam.Replace(" ", ""), true, null);
                            org0.Higher = own;
                            t1 = tt.Next;
                            Pullenti.Ner.Token ttt1 = this.AttachTailAttributes(org0, t1, ad, true, attachTyp, false);
                            if (tt0.Kit.Ontology != null) 
                            {
                                List<Pullenti.Ner.Core.IntOntologyToken> li = tt0.Kit.Ontology.AttachToken(OrganizationReferent.OBJ_TYPENAME, tt0);
                                if (li != null) 
                                {
                                    foreach (Pullenti.Ner.Core.IntOntologyToken v in li) 
                                    {
                                    }
                                }
                            }
                            return new Pullenti.Ner.ReferentToken(org0, tt0, ttt1 ?? t1);
                        }
                    }
                    if (((t is Pullenti.Ner.TextToken) && t.IsNewlineBefore && t.LengthChar > 1) && !t.Chars.IsAllLower && t.GetMorphClassInDictionary().IsUndefined) 
                    {
                        t1 = t.Next;
                        if (t1 != null && !t1.IsNewlineBefore && (t1 is Pullenti.Ner.TextToken)) 
                            t1 = t1.Next;
                        if (t1 != null && t1.IsNewlineBefore) 
                        {
                            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ0 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1, false, null);
                            if ((typ0 != null && typ0.Root != null && typ0.Root.Typ == Pullenti.Ner.Org.Internal.OrgItemTypeTyp.Prefix) && typ0.IsNewlineAfter) 
                            {
                                if (this.TryAttachOrg(t1, ad, AttachType.Normal, null, false, 0, -1) == null) 
                                {
                                    org = new OrganizationReferent();
                                    org.AddType(typ0, false);
                                    org.AddName(Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1.Previous, Pullenti.Ner.Core.GetTextAttr.No), true, null);
                                    t1 = typ0.EndToken;
                                    Pullenti.Ner.Token ttt1 = this.AttachTailAttributes(org, t1.Next, ad, true, attachTyp, false);
                                    return new Pullenti.Ner.ReferentToken(org, t, ttt1 ?? t1);
                                }
                            }
                            if (t1.IsChar('(')) 
                            {
                                if ((((typ0 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1.Next, false, null)))) != null) 
                                {
                                    if (typ0.EndToken.Next != null && typ0.EndToken.Next.IsChar(')') && typ0.EndToken.Next.IsNewlineAfter) 
                                    {
                                        org = new OrganizationReferent();
                                        org.AddType(typ0, false);
                                        org.AddName(Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1.Previous, Pullenti.Ner.Core.GetTextAttr.No), true, null);
                                        t1 = typ0.EndToken.Next;
                                        Pullenti.Ner.Token ttt1 = this.AttachTailAttributes(org, t1.Next, ad, true, attachTyp, false);
                                        return new Pullenti.Ner.ReferentToken(org, t, ttt1 ?? t1);
                                    }
                                }
                            }
                        }
                    }
                    if ((t is Pullenti.Ner.TextToken) && t.IsNewlineBefore && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null && br.IsNewlineAfter && (br.LengthChar < 100)) 
                        {
                            t1 = br.EndToken.Next;
                            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ0 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1, false, null);
                            if ((typ0 != null && typ0.Root != null && typ0.Root.Typ == Pullenti.Ner.Org.Internal.OrgItemTypeTyp.Prefix) && typ0.IsNewlineAfter) 
                            {
                                if (this.TryAttachOrg(t1, ad, AttachType.Normal, null, false, 0, -1) == null) 
                                {
                                    org = new OrganizationReferent();
                                    org.AddType(typ0, false);
                                    org.AddName(Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1.Previous, Pullenti.Ner.Core.GetTextAttr.No), true, null);
                                    t1 = typ0.EndToken;
                                    Pullenti.Ner.Token ttt1 = this.AttachTailAttributes(org, t1.Next, ad, true, attachTyp, false);
                                    return new Pullenti.Ner.ReferentToken(org, t, ttt1 ?? t1);
                                }
                            }
                            if (t1 != null && t1.IsChar('(')) 
                            {
                                if ((((typ0 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1.Next, false, null)))) != null) 
                                {
                                    if (typ0.EndToken.Next != null && typ0.EndToken.Next.IsChar(')') && typ0.EndToken.Next.IsNewlineAfter) 
                                    {
                                        org = new OrganizationReferent();
                                        org.AddType(typ0, false);
                                        org.AddName(Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1.Previous, Pullenti.Ner.Core.GetTextAttr.No), true, null);
                                        t1 = typ0.EndToken.Next;
                                        Pullenti.Ner.Token ttt1 = this.AttachTailAttributes(org, t1.Next, ad, true, attachTyp, false);
                                        return new Pullenti.Ner.ReferentToken(org, t, ttt1 ?? t1);
                                    }
                                }
                            }
                        }
                    }
                    return null;
                }
            }
            if (types != null && types.Count > 1 && attachTyp != AttachType.ExtOntology) 
            {
                if (types[0].Typ == "предприятие" || types[0].Typ == "підприємство") 
                {
                    types.RemoveAt(0);
                    t0 = types[0].BeginToken;
                }
            }
            if (rt0 == null) 
            {
                rt0 = this._TryAttachOrg_(t0, t, ad, types, specWordBefore, attachTyp, multTyp, isAdditionalAttach, level);
                if (rt0 != null && otExLi != null) 
                {
                    foreach (Pullenti.Ner.Core.IntOntologyToken ot in otExLi) 
                    {
                        if ((ot.EndChar > rt0.EndChar && ot.Item != null && ot.Item.Owner != null) && ot.Item.Owner.IsExtOntology) 
                        {
                            rt0 = null;
                            break;
                        }
                        else if (ot.EndChar < rt0.BeginChar) 
                        {
                            otExLi = null;
                            break;
                        }
                        else if (ot.EndChar < rt0.EndChar) 
                        {
                            if (ot.EndToken.Next.GetMorphClassInDictionary().IsPreposition) 
                            {
                                rt0 = null;
                                break;
                            }
                        }
                    }
                }
                if (rt0 != null) 
                {
                    if (types != null && rt0.BeginToken == types[0].BeginToken) 
                    {
                        foreach (Pullenti.Ner.Org.Internal.OrgItemTypeToken ty in types) 
                        {
                            (rt0.Referent as OrganizationReferent).AddType(ty, true);
                        }
                    }
                    if ((rt0.BeginToken == t0 && t0.Previous != null && t0.Previous.Morph.Class.IsAdjective) && (t0.WhitespacesBeforeCount < 2)) 
                    {
                        if ((rt0.Referent as OrganizationReferent).GeoObjects.Count == 0) 
                        {
                            object geo = this.IsGeo(t0.Previous, true);
                            if (geo != null) 
                            {
                                if ((rt0.Referent as OrganizationReferent).AddGeoObject(geo)) 
                                    rt0.BeginToken = t0.Previous;
                            }
                        }
                    }
                }
            }
            if (otExLi != null && rt0 == null && (otExLi.Count < 10)) 
            {
                foreach (Pullenti.Ner.Core.IntOntologyToken ot in otExLi) 
                {
                    OrganizationReferent org0 = ot.Item.Referent as OrganizationReferent;
                    if (org0 == null) 
                        continue;
                    if (org0.Names.Count == 0 && org0.Eponyms.Count == 0) 
                        continue;
                    Pullenti.Ner.Org.Internal.OrgItemTypeToken tyty = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(ot.BeginToken, true, null);
                    if (tyty != null && tyty.BeginToken == ot.EndToken) 
                        continue;
                    Pullenti.Ner.Token ts = ot.BeginToken;
                    Pullenti.Ner.Token te = ot.EndToken;
                    bool isQuots = false;
                    bool isVeryDoubt = false;
                    bool nameEq = false;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(ts.Previous, false, false) && Pullenti.Ner.Core.BracketHelper.IsBracket(ts.Previous, false)) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(te.Next, false, null, false)) 
                        {
                            if (ot.LengthChar < 2) 
                                continue;
                            if (ot.LengthChar == 2 && !org0.Names.Contains(te.GetSourceText())) 
                            {
                            }
                            else 
                            {
                                isQuots = true;
                                ts = ts.Previous;
                                te = te.Next;
                            }
                        }
                        else 
                            continue;
                    }
                    ok = types != null;
                    if (ot.EndToken.Next != null && (ot.EndToken.Next.GetReferent() is OrganizationReferent)) 
                        ok = true;
                    else if (ot.EndToken != ot.BeginToken) 
                    {
                        if (step == 0) 
                        {
                            if (!t.Kit.MiscData.ContainsKey("o2step")) 
                                t.Kit.MiscData.Add("o2step", null);
                            continue;
                        }
                        if (!ot.BeginToken.Chars.IsAllLower) 
                            ok = true;
                        else if (specWordBefore || isQuots) 
                            ok = true;
                    }
                    else if (ot.BeginToken is Pullenti.Ner.TextToken) 
                    {
                        if (step == 0) 
                        {
                            if (!t.Kit.MiscData.ContainsKey("o2step")) 
                                t.Kit.MiscData.Add("o2step", null);
                            continue;
                        }
                        ok = false;
                        int len = ot.BeginToken.LengthChar;
                        if (!ot.Chars.IsAllLower) 
                        {
                            if (!ot.Chars.IsAllUpper && ot.Morph.Class.IsPreposition) 
                                continue;
                            foreach (string n in org0.Names) 
                            {
                                if (ot.BeginToken.IsValue(n, null)) 
                                {
                                    nameEq = true;
                                    break;
                                }
                            }
                            Pullenti.Ner.TextAnnotation ano = org0.FindNearOccurence(ot.BeginToken);
                            if (ano == null) 
                            {
                                if (!ot.Item.Owner.IsExtOntology) 
                                {
                                    if (len < 3) 
                                        continue;
                                    else 
                                        isVeryDoubt = true;
                                }
                            }
                            else 
                            {
                                if (len == 2 && !t.Chars.IsAllUpper) 
                                    continue;
                                int d = ano.BeginChar - ot.BeginToken.BeginChar;
                                if (d < 0) 
                                    d = -d;
                                if (d > 2000) 
                                {
                                    if (len < 3) 
                                        continue;
                                    else if (len < 5) 
                                        isVeryDoubt = true;
                                }
                                else if (d > 300) 
                                {
                                    if (len < 3) 
                                        continue;
                                }
                                else if (len < 3) 
                                {
                                    if (d > 100 || !ot.BeginToken.Chars.IsAllUpper) 
                                        isVeryDoubt = true;
                                }
                            }
                            if (((ot.BeginToken.Chars.IsAllUpper || ot.BeginToken.Chars.IsLastLower)) && ((len > 3 || ((len == 3 && ((nameEq || ano != null))))))) 
                                ok = true;
                            else if ((specWordBefore || types != null || isQuots) || nameEq) 
                                ok = true;
                            else if ((ot.LengthChar < 3) && isVeryDoubt) 
                                continue;
                            else if (ot.Item.Owner.IsExtOntology && ot.BeginToken.GetMorphClassInDictionary().IsUndefined && ((len > 3 || ((len == 3 && ((nameEq || ano != null))))))) 
                                ok = true;
                            else if (ot.BeginToken.Chars.IsLatinLetter) 
                                ok = true;
                            else if ((nameEq && !ot.Chars.IsAllLower && !ot.Item.Owner.IsExtOntology) && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(ot.BeginToken)) 
                                ok = true;
                        }
                    }
                    else if (ot.BeginToken is Pullenti.Ner.ReferentToken) 
                    {
                        Pullenti.Ner.Referent r = ot.BeginToken.GetReferent();
                        if (r.TypeName != "DENOMINATION" && !isQuots) 
                            ok = false;
                    }
                    if (!ok) 
                    {
                    }
                    if (ok) 
                    {
                        ok = false;
                        org = new OrganizationReferent();
                        if (types != null) 
                        {
                            foreach (Pullenti.Ner.Org.Internal.OrgItemTypeToken ty in types) 
                            {
                                org.AddType(ty, false);
                            }
                            if (!org.CanBeEquals(org0, Pullenti.Ner.Core.ReferentsEqualType.ForMerging)) 
                                continue;
                        }
                        else 
                            foreach (string ty in org0.Types) 
                            {
                                org.AddTypeStr(ty);
                            }
                        if (org0.Number != null && (ot.BeginToken.Previous is Pullenti.Ner.NumberToken) && org.Number == null) 
                        {
                            if (org0.Number != (ot.BeginToken.Previous as Pullenti.Ner.NumberToken).Value.ToString() && (ot.BeginToken.WhitespacesBeforeCount < 2)) 
                            {
                                if (org.Names.Count > 0 || org.Higher != null) 
                                {
                                    isVeryDoubt = false;
                                    ok = true;
                                    org.Number = (ot.BeginToken.Previous as Pullenti.Ner.NumberToken).Value.ToString();
                                    if (org0.Higher != null) 
                                        org.Higher = org0.Higher;
                                    t0 = ot.BeginToken.Previous;
                                }
                            }
                        }
                        if (org.Number == null) 
                        {
                            Pullenti.Ner.Token ttt = ot.EndToken.Next;
                            Pullenti.Ner.Org.Internal.OrgItemNumberToken nnn = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(ttt, (org0.Number != null || !ot.IsWhitespaceAfter), null);
                            if (nnn == null && !ot.IsWhitespaceAfter && ttt != null) 
                            {
                                if (ttt.IsHiphen && ttt.Next != null) 
                                    ttt = ttt.Next;
                                if (ttt is Pullenti.Ner.NumberToken) 
                                    nnn = new Pullenti.Ner.Org.Internal.OrgItemNumberToken(ot.EndToken.Next, ttt) { Number = (ttt as Pullenti.Ner.NumberToken).Value.ToString() };
                            }
                            if (nnn != null) 
                            {
                                org.Number = nnn.Number;
                                te = nnn.EndToken;
                            }
                        }
                        bool norm = (ot.EndToken.EndChar - ot.BeginToken.BeginChar) > 5;
                        string s = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(ot, ((norm ? Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative : Pullenti.Ner.Core.GetTextAttr.No)) | Pullenti.Ner.Core.GetTextAttr.IgnoreArticles);
                        org.AddName(s, true, (norm ? null : ot.BeginToken));
                        if (types == null || types.Count == 0) 
                        {
                            string s1 = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(ot, Pullenti.Ner.Core.GetTextAttr.IgnoreArticles);
                            if (s1 != s && norm) 
                                org.AddName(s1, true, ot.BeginToken);
                        }
                        t1 = te;
                        if (t1.IsChar(')') && t1.IsNewlineAfter) 
                        {
                        }
                        else 
                        {
                            t1 = this.AttachMiddleAttributes(org, t1.Next) ?? t1;
                            if (attachTyp != AttachType.NormalAfterDep) 
                                t1 = this.AttachTailAttributes(org, t1.Next, ad, false, AttachType.Normal, false) ?? t1;
                        }
                        OrganizationReferent hi = null;
                        if (t1.Next != null) 
                            hi = t1.Next.GetReferent() as OrganizationReferent;
                        if (org0.Higher != null && hi != null && otExLi.Count == 1) 
                        {
                            if (hi.CanBeEquals(org0.Higher, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            {
                                org.Higher = hi;
                                t1 = t1.Next;
                            }
                        }
                        if ((org.Eponyms.Count == 0 && org.Number == null && isVeryDoubt) && !nameEq && types == null) 
                            continue;
                        if (!org.CanBeEqualsEx(org0, true, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                        {
                            if (t != null && Pullenti.Ner.Org.Internal.OrgItemTypeToken.CheckOrgSpecialWordBefore(t.Previous)) 
                                ok = true;
                            else if (!isVeryDoubt && ok) 
                            {
                            }
                            else 
                            {
                                if (!isVeryDoubt) 
                                {
                                    if (org.Eponyms.Count > 0 || org.Number != null || org.Higher != null) 
                                        ok = true;
                                }
                                ok = false;
                            }
                        }
                        else if (org.CanBeEquals(org0, Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts)) 
                        {
                            org.MergeSlots(org0, false);
                            ok = true;
                        }
                        else if (org0.Higher == null || org.Higher != null || ot.Item.Owner.IsExtOntology) 
                        {
                            ok = true;
                            org.MergeSlots(org0, false);
                        }
                        else if (!ot.Item.Owner.IsExtOntology && org.CanBeEquals(org0, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                        {
                            if (org0.Higher == null) 
                                org.MergeSlots(org0, false);
                            ok = true;
                        }
                        if (!ok) 
                            continue;
                        if (ts.BeginChar < t0.BeginChar) 
                            t0 = ts;
                        rt0 = new Pullenti.Ner.ReferentToken(org, t0, t1);
                        if (org.Kind == OrganizationKind.Department) 
                            this.CorrectDepAttrs(rt0, typ, false);
                        this._correctAfter(rt0);
                        if (ot.Item.Owner.IsExtOntology) 
                        {
                            foreach (Pullenti.Ner.Slot sl in org.Slots) 
                            {
                                if (sl.Value is Pullenti.Ner.Referent) 
                                {
                                    bool ext = false;
                                    foreach (Pullenti.Ner.Slot ss in org0.Slots) 
                                    {
                                        if (ss.Value == sl.Value) 
                                        {
                                            ext = true;
                                            break;
                                        }
                                    }
                                    if (!ext) 
                                        continue;
                                    Pullenti.Ner.Referent rr = (sl.Value as Pullenti.Ner.Referent).Clone();
                                    rr.Occurrence.Clear();
                                    org.UploadSlot(sl, rr);
                                    Pullenti.Ner.ReferentToken rtEx = new Pullenti.Ner.ReferentToken(rr, t0, t1);
                                    rtEx.SetDefaultLocalOnto(t0.Kit.Processor);
                                    org.AddExtReferent(rtEx);
                                    foreach (Pullenti.Ner.Slot sss in rr.Slots) 
                                    {
                                        if (sss.Value is Pullenti.Ner.Referent) 
                                        {
                                            Pullenti.Ner.Referent rrr = (sss.Value as Pullenti.Ner.Referent).Clone();
                                            rrr.Occurrence.Clear();
                                            rr.UploadSlot(sss, rrr);
                                            Pullenti.Ner.ReferentToken rtEx2 = new Pullenti.Ner.ReferentToken(rrr, t0, t1);
                                            rtEx2.SetDefaultLocalOnto(t0.Kit.Processor);
                                            (sl.Value as Pullenti.Ner.Referent).AddExtReferent(rtEx2);
                                        }
                                    }
                                }
                            }
                        }
                        this._correctAfter(rt0);
                        return rt0;
                    }
                }
            }
            if ((rt0 == null && types != null && types.Count == 1) && types[0].Name == null) 
            {
                Pullenti.Ner.Token tt0 = null;
                if (Pullenti.Ner.Core.MiscHelper.IsEngArticle(types[0].BeginToken)) 
                    tt0 = types[0].BeginToken;
                else if (Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(types[0].EndToken.Next)) 
                    tt0 = types[0].BeginToken;
                else 
                {
                    Pullenti.Ner.Token tt00 = types[0].BeginToken.Previous;
                    if (tt00 != null && (tt00.WhitespacesAfterCount < 2) && tt00.Chars.IsLatinLetter == types[0].Chars.IsLatinLetter) 
                    {
                        if (Pullenti.Ner.Core.MiscHelper.IsEngArticle(tt00)) 
                            tt0 = tt00;
                        else if (tt00.Morph.Class.IsPreposition || tt00.Morph.Class.IsPronoun) 
                            tt0 = tt00.Next;
                    }
                }
                int cou = 100;
                if (tt0 != null) 
                {
                    for (Pullenti.Ner.Token tt00 = tt0.Previous; tt00 != null && cou > 0; tt00 = tt00.Previous,cou--) 
                    {
                        if (tt00.GetReferent() is OrganizationReferent) 
                        {
                            if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypeAccords(tt00.GetReferent() as OrganizationReferent, types[0])) 
                            {
                                if ((types[0].WhitespacesAfterCount < 3) && Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(types[0].EndToken.Next, true, null) != null) 
                                {
                                }
                                else 
                                    rt0 = new Pullenti.Ner.ReferentToken(tt00.GetReferent(), tt0, types[0].EndToken);
                            }
                            break;
                        }
                    }
                }
            }
            if (rt0 != null) 
                this.CorrectOwnerBefore(rt0);
            if (hiph && !inBrackets && ((attachTyp == AttachType.Normal || attachTyp == AttachType.NormalAfterDep))) 
            {
                bool ok1 = false;
                if (rt0 != null && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt0.EndToken, true, null, false)) 
                {
                    if (types.Count > 0) 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemTypeToken ty = types[types.Count - 1];
                        if (ty.EndToken.Next != null && ty.EndToken.Next.IsHiphen && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(ty.EndToken.Next.Next, true, false)) 
                            ok1 = true;
                    }
                }
                else if (rt0 != null && rt0.EndToken.Next != null && rt0.EndToken.Next.IsHiphen) 
                {
                    Pullenti.Ner.Org.Internal.OrgItemTypeToken ty = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(rt0.EndToken.Next.Next, false, null);
                    if (ty == null) 
                        ok1 = true;
                }
                if (!ok1) 
                    return null;
            }
            if (attachTyp == AttachType.Multiple && t != null) 
            {
                if (t.Chars.IsAllLower) 
                    return null;
            }
            if (rt0 == null) 
                return rt0;
            bool doubt = rt0.Tag != null;
            org = rt0.Referent as OrganizationReferent;
            if (doubt && ad != null) 
            {
                List<Pullenti.Ner.Referent> rli = ad.LocalOntology.TryAttachByReferent(org, null, true);
                if (rli != null && rli.Count > 0) 
                    doubt = false;
                else 
                    foreach (Pullenti.Ner.Core.IntOntologyItem it in ad.LocalOntology.Items) 
                    {
                        if (it.Referent != null) 
                        {
                            if (it.Referent.CanBeEquals(org, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            {
                                doubt = false;
                                break;
                            }
                        }
                    }
            }
            if ((ad != null && t != null && t.Kit.Ontology != null) && attachTyp == AttachType.Normal && doubt) 
            {
                List<Pullenti.Ner.ExtOntologyItem> rli = t.Kit.Ontology.AttachReferent(org);
                if (rli != null) 
                {
                    if (rli.Count >= 1) 
                        doubt = false;
                }
            }
            if (doubt) 
                return null;
            this._correctAfter(rt0);
            return rt0;
        }
        void _correctAfter(Pullenti.Ner.ReferentToken rt0)
        {
            if (rt0 == null) 
                return;
            if (!rt0.IsNewlineAfter && rt0.EndToken.Next != null && rt0.EndToken.Next.IsChar('(')) 
            {
                Pullenti.Ner.Token tt = rt0.EndToken.Next.Next;
                if (tt is Pullenti.Ner.TextToken) 
                {
                    if (tt.IsChar(')')) 
                        rt0.EndToken = tt;
                    else if ((tt.LengthChar > 2 && (tt.LengthChar < 7) && tt.Chars.IsLatinLetter) && tt.Chars.IsAllUpper) 
                    {
                        string act = tt.GetSourceText().ToUpper();
                        if ((tt.Next is Pullenti.Ner.NumberToken) && !tt.IsWhitespaceAfter && (tt.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) 
                        {
                            tt = tt.Next;
                            act += tt.GetSourceText();
                        }
                        if (tt.Next != null && tt.Next.IsChar(')')) 
                        {
                            rt0.Referent.AddSlot(OrganizationReferent.ATTR_MISC, act, false, 0);
                            rt0.EndToken = tt.Next;
                        }
                    }
                    else 
                    {
                        OrganizationReferent org = rt0.Referent as OrganizationReferent;
                        if (org.Kind == OrganizationKind.Bank && tt.Chars.IsLatinLetter) 
                        {
                        }
                    }
                }
            }
            if (rt0.IsNewlineBefore && rt0.IsNewlineAfter && rt0.EndToken.Next != null) 
            {
                Pullenti.Ner.Token t1 = rt0.EndToken.Next;
                Pullenti.Ner.Org.Internal.OrgItemTypeToken typ1 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1, false, null);
                if ((typ1 != null && typ1.IsNewlineAfter && typ1.Root != null) && typ1.Root.Typ == Pullenti.Ner.Org.Internal.OrgItemTypeTyp.Prefix) 
                {
                    if (this.TryAttachOrg(t1, null, AttachType.Normal, null, false, 0, -1) == null) 
                    {
                        (rt0.Referent as OrganizationReferent).AddType(typ1, false);
                        rt0.EndToken = typ1.EndToken;
                    }
                }
                if (t1.IsChar('(')) 
                {
                    if ((((typ1 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1.Next, false, null)))) != null) 
                    {
                        if ((typ1.Root != null && typ1.Root.Typ == Pullenti.Ner.Org.Internal.OrgItemTypeTyp.Prefix && typ1.EndToken.Next != null) && typ1.EndToken.Next.IsChar(')') && typ1.EndToken.Next.IsNewlineAfter) 
                        {
                            (rt0.Referent as OrganizationReferent).AddType(typ1, false);
                            rt0.EndToken = typ1.EndToken.Next;
                        }
                    }
                }
            }
        }
        static Pullenti.Ner.Org.Internal.OrgItemTypeToken _lastTyp(List<Pullenti.Ner.Org.Internal.OrgItemTypeToken> types)
        {
            if (types == null) 
                return null;
            for (int i = types.Count - 1; i >= 0; i--) 
            {
                return types[i];
            }
            return null;
        }
        Pullenti.Ner.ReferentToken _TryAttachOrg_(Pullenti.Ner.Token t0, Pullenti.Ner.Token t, OrgAnalyzerData ad, List<Pullenti.Ner.Org.Internal.OrgItemTypeToken> types, bool specWordBefore, AttachType attachTyp, Pullenti.Ner.Org.Internal.OrgItemTypeToken multTyp, bool isAdditionalAttach, int level)
        {
            if (t0 == null) 
                return null;
            Pullenti.Ner.Token t1 = t;
            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ = _lastTyp(types);
            if (typ != null) 
            {
                if (typ.IsDep) 
                {
                    Pullenti.Ner.ReferentToken rt0 = this.TryAttachDep(typ, attachTyp, specWordBefore);
                    if (rt0 != null) 
                        return rt0;
                    if (typ.Typ == "группа" || typ.Typ == "група") 
                        typ.IsDep = false;
                    else 
                        return null;
                }
                if (typ.IsNewlineAfter && typ.Name == null) 
                {
                    if (t1 != null && (t1.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && typ.Profiles.Contains(OrgProfile.State)) 
                    {
                    }
                    else if (typ.Root != null && ((typ.Root.Coeff >= 3 || typ.Root.IsPurePrefix))) 
                    {
                    }
                    else if (typ.Coef >= 4) 
                    {
                    }
                    else if ((typ.Coef >= 3 && (typ.NewlinesAfterCount < 2) && typ.EndToken.Next != null) && typ.EndToken.Next.Morph.Class.IsPreposition) 
                    {
                    }
                    else 
                        return null;
                }
                if (typ != multTyp && ((typ.Morph.Number == Pullenti.Morph.MorphNumber.Plural && !char.IsUpper(typ.Typ[0])))) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                    {
                    }
                    else if (typ.EndToken.IsValue("ВЛАСТЬ", null)) 
                    {
                    }
                    else 
                        return null;
                }
                if (attachTyp == AttachType.Normal || attachTyp == AttachType.NormalAfterDep) 
                {
                    if (((typ.Typ == "предприятие" || typ.Typ == "підприємство")) && !specWordBefore && types.Count == 1) 
                        return null;
                }
            }
            OrganizationReferent org = new OrganizationReferent();
            if (types != null) 
            {
                foreach (Pullenti.Ner.Org.Internal.OrgItemTypeToken ty in types) 
                {
                    org.AddType(ty, false);
                }
            }
            if (typ != null && typ.Root != null && typ.Root.IsPurePrefix) 
            {
                if ((t is Pullenti.Ner.TextToken) && t.Chars.IsAllUpper && !t.IsNewlineAfter) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken b = Pullenti.Ner.Core.BracketHelper.TryParse(t.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (b != null && b.IsQuoteType) 
                    {
                        org.AddTypeStr((t as Pullenti.Ner.TextToken).Term);
                        t = t.Next;
                    }
                    else 
                    {
                        string s = (t as Pullenti.Ner.TextToken).Term;
                        if (s.Length == 2 && s[s.Length - 1] == 'К') 
                        {
                            org.AddTypeStr(s);
                            t = t.Next;
                        }
                        else if (((t.GetMorphClassInDictionary().IsUndefined && t.Next != null && (t.Next is Pullenti.Ner.TextToken)) && t.Next.Chars.IsCapitalUpper && t.Next.Next != null) && !t.Next.IsNewlineAfter) 
                        {
                            if (t.Next.Next.IsCharOf(",.;") || Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t.Next.Next, false, null, false)) 
                            {
                                org.AddTypeStr(s);
                                t = t.Next;
                            }
                        }
                    }
                }
                else if ((t is Pullenti.Ner.TextToken) && t.Morph.Class.IsAdjective && !t.Chars.IsAllLower) 
                {
                    Pullenti.Ner.ReferentToken rtg = this.IsGeo(t, true) as Pullenti.Ner.ReferentToken;
                    if (rtg != null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(rtg.EndToken.Next, false, false)) 
                    {
                        org.AddGeoObject(rtg);
                        t = rtg.EndToken.Next;
                    }
                }
                else if ((t != null && (t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && t.Next != null) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Next, true, false)) 
                {
                    org.AddGeoObject(t.GetReferent());
                    t = t.Next;
                }
            }
            Pullenti.Ner.Token te = null;
            OrganizationKind ki0 = org.Kind;
            if (((((ki0 == OrganizationKind.Govenment || ki0 == OrganizationKind.Airport || ki0 == OrganizationKind.Factory) || ki0 == OrganizationKind.Seaport || ki0 == OrganizationKind.Party) || ki0 == OrganizationKind.Justice || ki0 == OrganizationKind.Military)) && t != null) 
            {
                object g = this.IsGeo(t, false);
                if (g == null && t.Morph.Class.IsPreposition && t.Next != null) 
                    g = this.IsGeo(t.Next, false);
                if (g != null) 
                {
                    if (org.AddGeoObject(g)) 
                    {
                        te = (t1 = this.GetGeoEndToken(g, t));
                        t = t1.Next;
                        List<Pullenti.Ner.Core.IntOntologyToken> gt = Pullenti.Ner.Org.Internal.OrgGlobal.GlobalOrgs.TryAttach(t, null, false);
                        if (gt == null && t != null && t.Kit.BaseLanguage.IsUa) 
                            gt = Pullenti.Ner.Org.Internal.OrgGlobal.GlobalOrgsUa.TryAttach(t, null, false);
                        if (gt != null && gt.Count == 1) 
                        {
                            if (org.CanBeEquals(gt[0].Item.Referent, Pullenti.Ner.Core.ReferentsEqualType.ForMerging)) 
                            {
                                org.MergeSlots(gt[0].Item.Referent, false);
                                return new Pullenti.Ner.ReferentToken(org, t0, gt[0].EndToken);
                            }
                        }
                    }
                }
            }
            if (typ != null && typ.Root != null && ((typ.Root.CanBeSingleGeo && !typ.Root.CanHasSingleName))) 
            {
                if (org.GeoObjects.Count > 0 && te != null) 
                    return new Pullenti.Ner.ReferentToken(org, t0, te);
                object r = null;
                te = (t1 = (typ != multTyp ? typ.EndToken : t0.Previous));
                if (t != null && t1.Next != null) 
                {
                    r = this.IsGeo(t1.Next, false);
                    if (r == null && t1.Next.Morph.Class.IsPreposition) 
                        r = this.IsGeo(t1.Next.Next, false);
                }
                if (r != null) 
                {
                    if (!org.AddGeoObject(r)) 
                        return null;
                    te = this.GetGeoEndToken(r, t1.Next);
                }
                if (org.GeoObjects.Count > 0 && te != null) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt11 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(te.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt11 != null && (te.WhitespacesAfterCount < 2) && npt11.Noun.IsValue("ДЕПУТАТ", null)) 
                    {
                    }
                    else 
                    {
                        Pullenti.Ner.ReferentToken res11 = new Pullenti.Ner.ReferentToken(org, t0, te);
                        if (org.FindSlot(OrganizationReferent.ATTR_TYPE, "посольство", true) != null) 
                        {
                            if (te.Next != null && te.Next.IsValue("В", null)) 
                            {
                                r = this.IsGeo(te.Next.Next, false);
                                if (org.AddGeoObject(r)) 
                                    res11.EndToken = this.GetGeoEndToken(r, te.Next.Next);
                            }
                        }
                        if (typ.Root.CanHasNumber) 
                        {
                            Pullenti.Ner.Org.Internal.OrgItemNumberToken num11 = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(res11.EndToken.Next, false, null);
                            if (num11 != null) 
                            {
                                res11.EndToken = num11.EndToken;
                                org.Number = num11.Number;
                            }
                        }
                        return res11;
                    }
                }
            }
            if (typ != null && (((typ.Typ == "милиция" || typ.Typ == "полиция" || typ.Typ == "міліція") || typ.Typ == "поліція"))) 
            {
                if (org.GeoObjects.Count > 0 && te != null) 
                    return new Pullenti.Ner.ReferentToken(org, t0, te);
                else 
                    return null;
            }
            if (t != null && t.Morph.Class.IsProperName) 
            {
                Pullenti.Ner.ReferentToken rt1 = t.Kit.ProcessReferent("PERSON", t);
                if (rt1 != null && (rt1.WhitespacesAfterCount < 2)) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(rt1.EndToken.Next, true, false)) 
                        t = rt1.EndToken.Next;
                    else if (rt1.EndToken.Next != null && rt1.EndToken.Next.IsHiphen && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(rt1.EndToken.Next.Next, true, false)) 
                        t = rt1.EndToken.Next.Next;
                }
            }
            else if ((t != null && t.Chars.IsCapitalUpper && t.Morph.Class.IsProperSurname) && t.Next != null && (t.WhitespacesAfterCount < 2)) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Next, true, false)) 
                    t = t.Next;
                else if (((t.Next.IsCharOf(":") || t.Next.IsHiphen)) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Next.Next, true, false)) 
                    t = t.Next.Next;
            }
            Pullenti.Ner.Token tMax = null;
            Pullenti.Ner.Core.BracketSequenceToken br = null;
            if (t != null) 
            {
                br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (typ != null && br == null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false)) 
                {
                    if (t.Next != null && (t.Next.GetReferent() is OrganizationReferent)) 
                    {
                        OrganizationReferent org0 = t.Next.GetReferent() as OrganizationReferent;
                        if (!Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticOO(org, org0)) 
                        {
                            org0.MergeSlots(org, false);
                            return new Pullenti.Ner.ReferentToken(org0, t0, t.Next);
                        }
                    }
                    if (((typ.Typ == "компания" || typ.Typ == "предприятие" || typ.Typ == "организация") || typ.Typ == "компанія" || typ.Typ == "підприємство") || typ.Typ == "організація") 
                    {
                        if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsDecreeKeyword(t0.Previous, 1)) 
                            return null;
                    }
                    Pullenti.Ner.Org.Internal.OrgItemTypeToken ty2 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t.Next, false, null);
                    if (ty2 != null) 
                    {
                        List<Pullenti.Ner.Org.Internal.OrgItemTypeToken> typs2 = new List<Pullenti.Ner.Org.Internal.OrgItemTypeToken>();
                        typs2.Add(ty2);
                        Pullenti.Ner.ReferentToken rt2 = this._TryAttachOrg_(t.Next, ty2.EndToken.Next, ad, typs2, true, AttachType.High, null, isAdditionalAttach, level + 1);
                        if (rt2 != null) 
                        {
                            OrganizationReferent org0 = rt2.Referent as OrganizationReferent;
                            if (!Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticOO(org, org0)) 
                            {
                                org0.MergeSlots(org, false);
                                rt2.BeginToken = t0;
                                if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt2.EndToken.Next, false, null, false)) 
                                    rt2.EndToken = rt2.EndToken.Next;
                                return rt2;
                            }
                        }
                    }
                }
            }
            if (br != null && typ != null && org.Kind == OrganizationKind.Govenment) 
            {
                if (typ.Root != null && !typ.Root.CanHasSingleName) 
                    br = null;
            }
            if (br != null && br.IsQuoteType) 
            {
                if (br.BeginToken.Next.IsValue("О", null) || br.BeginToken.Next.IsValue("ОБ", null)) 
                    br = null;
                else if (br.BeginToken.Previous != null && br.BeginToken.Previous.IsChar(':')) 
                    br = null;
            }
            if (br != null && br.IsQuoteType && ((br.OpenChar != '<' || ((typ != null && typ.Root != null && typ.Root.IsPurePrefix))))) 
            {
                if (t.IsNewlineBefore && ((attachTyp == AttachType.Normal || attachTyp == AttachType.NormalAfterDep))) 
                {
                    if (!br.IsNewlineAfter) 
                        return null;
                }
                if (org.FindSlot(OrganizationReferent.ATTR_TYPE, "организация", true) != null || org.FindSlot(OrganizationReferent.ATTR_TYPE, "організація", true) != null) 
                {
                    if (typ.BeginToken == typ.EndToken) 
                    {
                        if (!specWordBefore) 
                            return null;
                    }
                }
                if (typ != null && ((((typ.Typ == "компания" || typ.Typ == "предприятие" || typ.Typ == "организация") || typ.Typ == "компанія" || typ.Typ == "підприємство") || typ.Typ == "організація"))) 
                {
                    if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsDecreeKeyword(t0.Previous, 1)) 
                        return null;
                }
                Pullenti.Ner.Org.Internal.OrgItemNameToken nn = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(t.Next, null, false, true);
                if (nn != null && nn.IsIgnoredPart) 
                    t = nn.EndToken;
                OrganizationReferent org0 = t.Next.GetReferent() as OrganizationReferent;
                if (org0 != null) 
                {
                    if (!Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticOO(org, org0) && t.Next.Next != null) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t.Next.Next, false, null, false)) 
                        {
                            org0.MergeSlots(org, false);
                            return new Pullenti.Ner.ReferentToken(org0, t0, t.Next.Next);
                        }
                        if ((t.Next.Next.GetReferent() is OrganizationReferent) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t.Next.Next.Next, false, null, false)) 
                        {
                            org0.MergeSlots(org, false);
                            return new Pullenti.Ner.ReferentToken(org0, t0, t.Next);
                        }
                    }
                    return null;
                }
                Pullenti.Ner.Org.Internal.OrgItemNameToken na0 = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(br.BeginToken.Next, null, false, true);
                if (na0 != null && na0.IsEmptyWord && na0.EndToken.Next == br.EndToken) 
                    return null;
                Pullenti.Ner.ReferentToken rt0 = this.TryAttachOrg(t.Next, null, attachTyp, null, isAdditionalAttach, level + 1, -1);
                if (br.Internal.Count > 1) 
                {
                    if (rt0 != null && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt0.EndToken, false, null, false)) 
                        br.EndToken = rt0.EndToken;
                    else 
                        return null;
                }
                string abbr = null;
                Pullenti.Ner.Token tt00 = (rt0 == null ? null : rt0.BeginToken);
                if (((rt0 == null && t.Next != null && (t.Next is Pullenti.Ner.TextToken)) && t.Next.Chars.IsAllUpper && t.Next.LengthChar > 2) && t.Next.Chars.IsCyrillicLetter) 
                {
                    rt0 = this.TryAttachOrg(t.Next.Next, null, attachTyp, null, isAdditionalAttach, level + 1, -1);
                    if (rt0 != null && rt0.BeginToken == t.Next.Next) 
                    {
                        tt00 = t.Next;
                        abbr = t.Next.GetSourceText();
                    }
                    else 
                        rt0 = null;
                }
                bool ok2 = false;
                if (rt0 != null) 
                {
                    if (rt0.EndToken == br.EndToken.Previous || rt0.EndToken == br.EndToken) 
                        ok2 = true;
                    else if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt0.EndToken, false, null, false) && rt0.EndChar > br.EndChar) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br2 = Pullenti.Ner.Core.BracketHelper.TryParse(br.EndToken.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br2 != null && rt0.EndToken == br2.EndToken) 
                            ok2 = true;
                    }
                }
                if (ok2 && (rt0.Referent is OrganizationReferent)) 
                {
                    org0 = rt0.Referent as OrganizationReferent;
                    if (typ != null && typ.Typ == "служба" && ((org0.Kind == OrganizationKind.Media || org0.Kind == OrganizationKind.Press))) 
                    {
                        if (br.BeginToken == rt0.BeginToken && br.EndToken == rt0.EndToken) 
                            return rt0;
                    }
                    Pullenti.Ner.Org.Internal.OrgItemTypeToken typ1 = null;
                    if (tt00 != t.Next) 
                    {
                        typ1 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t.Next, false, ad);
                        if (typ1 != null && typ1.EndToken.Next == tt00) 
                            org.AddType(typ1, false);
                    }
                    bool hi = false;
                    if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org0, org, true)) 
                    {
                        if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticOO(org0, org)) 
                            hi = true;
                    }
                    if (hi) 
                    {
                        org.Higher = org0;
                        rt0.SetDefaultLocalOnto(t.Kit.Processor);
                        org.AddExtReferent(rt0);
                        if (typ1 != null) 
                            org.AddType(typ1, true);
                        if (abbr != null) 
                            org.AddName(abbr, true, null);
                    }
                    else if (!Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticOO(org0, org)) 
                    {
                        org.MergeSlots(org0, true);
                        if (abbr != null) 
                        {
                            foreach (Pullenti.Ner.Slot s in org.Slots) 
                            {
                                if (s.TypeName == OrganizationReferent.ATTR_NAME) 
                                    org.UploadSlot(s, string.Format("{0} {1}", abbr, s.Value));
                            }
                        }
                    }
                    else 
                        rt0 = null;
                    if (rt0 != null) 
                    {
                        Pullenti.Ner.Token t11 = br.EndToken;
                        if (rt0.EndChar > t11.EndChar) 
                            t11 = rt0.EndToken;
                        Pullenti.Ner.Org.Internal.OrgItemEponymToken ep11 = Pullenti.Ner.Org.Internal.OrgItemEponymToken.TryAttach(t11.Next, true);
                        if (ep11 != null) 
                        {
                            t11 = ep11.EndToken;
                            foreach (string e in ep11.Eponyms) 
                            {
                                org.AddEponym(e);
                            }
                        }
                        t1 = this.AttachTailAttributes(org, t11.Next, null, true, attachTyp, false);
                        if (t1 == null) 
                            t1 = t11;
                        if (typ != null) 
                        {
                            if ((typ.Name != null && typ.Geo == null && org.Names.Count > 0) && !org.Names.Contains(typ.Name)) 
                                org.AddTypeStr(typ.Name.ToLower());
                        }
                        return new Pullenti.Ner.ReferentToken(org, t0, t1);
                    }
                }
                if (rt0 != null && (rt0.EndChar < br.EndToken.Previous.EndChar)) 
                {
                    Pullenti.Ner.ReferentToken rt1 = this.TryAttachOrg(rt0.EndToken.Next, null, attachTyp, null, isAdditionalAttach, level + 1, -1);
                    if (rt1 != null && rt1.EndToken.Next == br.EndToken) 
                        return rt1;
                    OrganizationReferent org1 = rt0.EndToken.Next.GetReferent() as OrganizationReferent;
                    if (org1 != null && br.EndToken.Previous == rt0.EndToken) 
                    {
                    }
                }
                for (int step = 0; step < 2; step++) 
                {
                    Pullenti.Ner.Token tt0 = t.Next;
                    Pullenti.Ner.Token tt1 = null;
                    bool pref = true;
                    int notEmpty = 0;
                    for (t1 = t.Next; t1 != null && t1 != br.EndToken; t1 = t1.Next) 
                    {
                        if (t1.IsChar('(')) 
                        {
                            if (notEmpty == 0) 
                                break;
                            Pullenti.Ner.Referent r = null;
                            if (t1.Next != null) 
                                r = t1.Next.GetReferent();
                            if (r != null && t1.Next.Next != null && t1.Next.Next.IsChar(')')) 
                            {
                                if (r.TypeName == GEONAME) 
                                {
                                    org.AddGeoObject(r);
                                    break;
                                }
                            }
                            if (level == 0) 
                            {
                                Pullenti.Ner.ReferentToken rt = this.TryAttachOrg(t1.Next, null, AttachType.High, null, false, level + 1, -1);
                                if (rt != null && rt.EndToken.Next != null && rt.EndToken.Next.IsChar(')')) 
                                {
                                    if (!OrganizationReferent.CanBeSecondDefinition(org, rt.Referent as OrganizationReferent)) 
                                        break;
                                    org.MergeSlots(rt.Referent, false);
                                }
                            }
                            break;
                        }
                        else if ((((org0 = t1.GetReferent() as OrganizationReferent))) != null) 
                        {
                            if (((t1.Previous is Pullenti.Ner.NumberToken) && t1.Previous.Previous == br.BeginToken && !Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticOO(org, org0)) && org0.Number == null) 
                            {
                                org0.Number = (t1.Previous as Pullenti.Ner.NumberToken).Value.ToString();
                                org0.MergeSlots(org, false);
                                if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next, false, null, false)) 
                                    t1 = t1.Next;
                                return new Pullenti.Ner.ReferentToken(org0, t0, t1);
                            }
                            Pullenti.Ner.Org.Internal.OrgItemNameToken ne = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(br.BeginToken.Next, null, attachTyp == AttachType.ExtOntology, true);
                            if (ne != null && ne.IsIgnoredPart && ne.EndToken.Next == t1) 
                            {
                                org0.MergeSlots(org, false);
                                if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next, false, null, false)) 
                                    t1 = t1.Next;
                                return new Pullenti.Ner.ReferentToken(org0, t0, t1);
                            }
                            return null;
                        }
                        else 
                        {
                            typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1, false, null);
                            if (typ != null && types != null) 
                            {
                                foreach (Pullenti.Ner.Org.Internal.OrgItemTypeToken ty in types) 
                                {
                                    if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticTT(ty, typ)) 
                                    {
                                        typ = null;
                                        break;
                                    }
                                }
                            }
                            if (typ != null) 
                            {
                                if (typ.IsDoubtRootWord && ((typ.EndToken.Next == br.EndToken || ((typ.EndToken.Next != null && typ.EndToken.Next.IsHiphen))))) 
                                    typ = null;
                                else if (typ.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                                    typ = null;
                                else if (!typ.Morph.Case.IsUndefined && !typ.Morph.Case.IsNominative) 
                                    typ = null;
                                else if (typ.BeginToken == typ.EndToken) 
                                {
                                    Pullenti.Ner.Token ttt = typ.EndToken.Next;
                                    if (ttt != null && ttt.IsHiphen) 
                                        ttt = ttt.Next;
                                    if (ttt != null) 
                                    {
                                        if (ttt.IsValue("БАНК", null)) 
                                            typ = null;
                                    }
                                }
                            }
                            Pullenti.Ner.Org.Internal.OrgItemEponymToken ep = null;
                            if (typ == null) 
                                ep = Pullenti.Ner.Org.Internal.OrgItemEponymToken.TryAttach(t1, false);
                            Pullenti.Ner.Org.Internal.OrgItemNumberToken nu = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(t1, false, null);
                            if (nu != null && !(t1 is Pullenti.Ner.NumberToken)) 
                            {
                                org.Number = nu.Number;
                                tt1 = t1.Previous;
                                t1 = nu.EndToken;
                                notEmpty += 2;
                                continue;
                            }
                            bool brSpec = false;
                            if ((br.Internal.Count == 0 && (br.EndToken.Next is Pullenti.Ner.TextToken) && ((!br.EndToken.Next.Chars.IsAllLower && br.EndToken.Next.Chars.IsLetter))) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(br.EndToken.Next.Next, true, null, false)) 
                                brSpec = true;
                            if (typ != null && ((pref || !typ.IsDep))) 
                            {
                                if (notEmpty > 1) 
                                {
                                    Pullenti.Ner.ReferentToken rrr = this.TryAttachOrg(typ.BeginToken, ad, AttachType.Normal, null, false, level + 1, -1);
                                    if (rrr != null) 
                                    {
                                        br.EndToken = (t1 = typ.BeginToken.Previous);
                                        break;
                                    }
                                }
                                if (((attachTyp == AttachType.ExtOntology || attachTyp == AttachType.High)) && ((typ.Root == null || !typ.Root.IsPurePrefix))) 
                                    pref = false;
                                else if (typ.Name == null) 
                                {
                                    org.AddType(typ, false);
                                    if (pref) 
                                        tt0 = typ.EndToken.Next;
                                    else if (typ.Root != null && typ.Root.IsPurePrefix) 
                                    {
                                        tt1 = typ.BeginToken.Previous;
                                        break;
                                    }
                                }
                                else if (typ.EndToken.Next != br.EndToken) 
                                {
                                    org.AddType(typ, false);
                                    if (typ.Typ == "банк") 
                                        pref = false;
                                    else 
                                    {
                                        org.AddTypeStr(typ.Name.ToLower());
                                        org.AddTypeStr(typ.AltTyp);
                                        if (pref) 
                                            tt0 = typ.EndToken.Next;
                                    }
                                }
                                else if (brSpec) 
                                {
                                    org.AddType(typ, false);
                                    org.AddTypeStr(typ.Name.ToLower());
                                    notEmpty += 2;
                                    tt0 = br.EndToken.Next;
                                    t1 = tt0.Next;
                                    br.EndToken = t1;
                                    break;
                                }
                                if (typ != multTyp) 
                                {
                                    t1 = typ.EndToken;
                                    if (typ.Geo != null) 
                                        org.AddType(typ, false);
                                }
                            }
                            else if (ep != null) 
                            {
                                foreach (string e in ep.Eponyms) 
                                {
                                    org.AddEponym(e);
                                }
                                notEmpty += 3;
                                t1 = ep.BeginToken.Previous;
                                break;
                            }
                            else if (t1 == t.Next && (t1 is Pullenti.Ner.TextToken) && t1.Chars.IsAllLower) 
                                return null;
                            else if (t1.Chars.IsLetter || (t1 is Pullenti.Ner.NumberToken)) 
                            {
                                if (brSpec) 
                                {
                                    tt0 = br.BeginToken;
                                    t1 = br.EndToken.Next.Next;
                                    string ss = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.EndToken, t1, Pullenti.Ner.Core.GetTextAttr.No);
                                    if (!string.IsNullOrEmpty(ss)) 
                                    {
                                        org.AddName(ss, true, br.EndToken.Next);
                                        br.EndToken = t1;
                                    }
                                    break;
                                }
                                pref = false;
                                notEmpty++;
                            }
                        }
                    }
                    bool canHasNum = false;
                    bool canHasLatinName = false;
                    if (types != null) 
                    {
                        foreach (Pullenti.Ner.Org.Internal.OrgItemTypeToken ty in types) 
                        {
                            if (ty.Root != null) 
                            {
                                if (ty.Root.CanHasNumber) 
                                    canHasNum = true;
                                if (ty.Root.CanHasLatinName) 
                                    canHasLatinName = true;
                            }
                        }
                    }
                    te = tt1 ?? t1;
                    if (te != null && tt0 != null && (tt0.BeginChar < te.BeginChar)) 
                    {
                        for (Pullenti.Ner.Token ttt = tt0; ttt != te && ttt != null; ttt = ttt.Next) 
                        {
                            Pullenti.Ner.Org.Internal.OrgItemNameToken oin = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(ttt, null, attachTyp == AttachType.ExtOntology, ttt == tt0);
                            if (oin != null) 
                            {
                                if (oin.IsIgnoredPart && ttt == tt0) 
                                {
                                    tt0 = oin.EndToken.Next;
                                    if (tt0 == null) 
                                        break;
                                    ttt = tt0.Previous;
                                    continue;
                                }
                                if (oin.IsStdTail) 
                                {
                                    Pullenti.Ner.Org.Internal.OrgItemEngItem ei = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttach(oin.BeginToken, false);
                                    if (ei == null && oin.BeginToken.IsComma) 
                                        ei = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttach(oin.BeginToken.Next, false);
                                    if (ei != null) 
                                    {
                                        org.AddTypeStr(ei.FullValue);
                                        if (ei.ShortValue != null) 
                                            org.AddTypeStr(ei.ShortValue);
                                    }
                                    te = ttt.Previous;
                                    break;
                                }
                            }
                            if ((ttt != tt0 && (ttt is Pullenti.Ner.ReferentToken) && ttt.Next == te) && (ttt.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                            {
                                if (ttt.Previous != null && ttt.Previous.GetMorphClassInDictionary().IsAdjective) 
                                    continue;
                                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun, 0, null);
                                if (npt != null && npt.EndToken == ttt) 
                                {
                                }
                                else 
                                {
                                    te = ttt.Previous;
                                    if (te.Morph.Class.IsPreposition && te.Previous != null) 
                                        te = te.Previous;
                                }
                                org.AddGeoObject(ttt.GetReferent());
                                break;
                            }
                        }
                    }
                    if (te != null && tt0 != null && (tt0.BeginChar < te.BeginChar)) 
                    {
                        if ((te.Previous is Pullenti.Ner.NumberToken) && canHasNum) 
                        {
                            bool err = false;
                            Pullenti.Ner.NumberToken num1 = te.Previous as Pullenti.Ner.NumberToken;
                            if (org.Number != null && org.Number != num1.Value.ToString()) 
                                err = true;
                            else if (te.Previous.Previous == null) 
                                err = true;
                            else if (!te.Previous.Previous.IsHiphen && !te.Previous.Previous.Chars.IsLetter) 
                                err = true;
                            else if (num1.Value == "0") 
                                err = true;
                            if (!err) 
                            {
                                org.Number = num1.Value.ToString();
                                te = te.Previous.Previous;
                                if (te != null && ((te.IsHiphen || te.IsValue("N", null) || te.IsValue("№", null)))) 
                                    te = te.Previous;
                            }
                        }
                    }
                    string s = (te == null ? null : Pullenti.Ner.Core.MiscHelper.GetTextValue(tt0, te, Pullenti.Ner.Core.GetTextAttr.No));
                    string s1 = (te == null ? null : Pullenti.Ner.Core.MiscHelper.GetTextValue(tt0, te, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative));
                    if ((te != null && (te.Previous is Pullenti.Ner.NumberToken) && canHasNum) && org.Number == null) 
                    {
                        org.Number = (te.Previous as Pullenti.Ner.NumberToken).Value.ToString();
                        Pullenti.Ner.Token tt11 = te.Previous;
                        if (tt11.Previous != null && tt11.Previous.IsHiphen) 
                            tt11 = tt11.Previous;
                        if (tt11.Previous != null) 
                        {
                            s = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt0, tt11.Previous, Pullenti.Ner.Core.GetTextAttr.No);
                            s1 = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt0, tt11.Previous, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                        }
                    }
                    if (!string.IsNullOrEmpty(s)) 
                    {
                        if (tt0.Morph.Class.IsPreposition && tt0 != br.BeginToken.Next) 
                        {
                            foreach (string ty in org.Types) 
                            {
                                if (!ty.Contains(" ") && char.IsLower(ty[0])) 
                                {
                                    s = string.Format("{0} {1}", ty.ToUpper(), s);
                                    s1 = null;
                                    break;
                                }
                            }
                        }
                        if (s.Length > MaxOrgName) 
                            return null;
                        if (s1 != null && s1 != s && s1.Length <= s.Length) 
                            org.AddName(s1, true, null);
                        org.AddName(s, true, tt0);
                        typ = _lastTyp(types);
                        if (typ != null && typ.Root != null && typ.Root.CanonicText.StartsWith("ИНДИВИДУАЛЬН")) 
                        {
                            Pullenti.Ner.ReferentToken pers = typ.Kit.ProcessReferent("PERSON", tt0);
                            if (pers != null && pers.EndToken.Next == te) 
                            {
                                org.AddExtReferent(pers);
                                org.AddSlot(OrganizationReferent.ATTR_OWNER, pers.Referent, false, 0);
                            }
                        }
                        bool ok1 = false;
                        foreach (char c in s) 
                        {
                            if (char.IsLetterOrDigit(c)) 
                            {
                                ok1 = true;
                                break;
                            }
                        }
                        if (!ok1) 
                            return null;
                        if (br.BeginToken.Next.Chars.IsAllLower) 
                            return null;
                        if (org.Types.Count == 0) 
                        {
                            Pullenti.Ner.Org.Internal.OrgItemTypeToken ty = _lastTyp(types);
                            if (ty != null && ty.Coef >= 4) 
                            {
                            }
                            else 
                            {
                                if (attachTyp == AttachType.Normal) 
                                    return null;
                                if (org.Names.Count == 1 && (org.Names[0].Length < 2) && (br.LengthChar < 5)) 
                                    return null;
                            }
                        }
                    }
                    else if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t1, false, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br1 = Pullenti.Ner.Core.BracketHelper.TryParse(t1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br1 == null) 
                            break;
                        t = br1.BeginToken;
                        br = br1;
                        continue;
                    }
                    else if (((org.Number != null || org.Eponyms.Count > 0)) && t1 == br.EndToken) 
                    {
                    }
                    else if (org.GeoObjects.Count > 0 && org.Types.Count > 2) 
                    {
                    }
                    else 
                        return null;
                    t1 = br.EndToken;
                    if (org.Number == null && t1.Next != null && (t1.WhitespacesAfterCount < 2)) 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemNumberToken num1 = (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsDecreeKeyword(t0.Previous, 1) ? null : Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(t1.Next, false, typ));
                        if (num1 != null) 
                        {
                            org.Number = num1.Number;
                            t1 = num1.EndToken;
                        }
                        else 
                            t1 = this.AttachTailAttributes(org, t1.Next, null, true, attachTyp, false);
                    }
                    else 
                        t1 = this.AttachTailAttributes(org, t1.Next, null, true, attachTyp, false);
                    if (t1 == null) 
                        t1 = br.EndToken;
                    bool ok0 = false;
                    if (types != null) 
                    {
                        foreach (Pullenti.Ner.Org.Internal.OrgItemTypeToken ty in types) 
                        {
                            if (ty.Name != null) 
                                org.AddTypeStr(ty.Name.ToLower());
                            if (attachTyp != AttachType.Multiple && (ty.BeginChar < t0.BeginChar) && !ty.IsNotTyp) 
                                t0 = ty.BeginToken;
                            if (!ty.IsDoubtRootWord || ty.Coef > 0 || ty.Geo != null) 
                                ok0 = true;
                            else if (ty.Typ == "движение" && ((!br.BeginToken.Next.Chars.IsAllLower || !ty.Chars.IsAllLower))) 
                            {
                                if (!br.BeginToken.Next.Morph.Case.IsGenitive) 
                                    ok0 = true;
                            }
                            else if (ty.Typ == "АО") 
                            {
                                if (ty.BeginToken.Chars.IsAllUpper && (ty.WhitespacesAfterCount < 2) && Pullenti.Ner.Core.BracketHelper.IsBracket(ty.EndToken.Next, true)) 
                                    ok0 = true;
                                else 
                                    for (Pullenti.Ner.Token tt2 = t1.Next; tt2 != null; tt2 = tt2.Next) 
                                    {
                                        if (tt2.IsComma) 
                                            continue;
                                        if (tt2.IsValue("ИМЕНОВАТЬ", null)) 
                                            ok0 = true;
                                        if (tt2.IsValue("В", null) && tt2.Next != null) 
                                        {
                                            if (tt2.Next.IsValue("ЛИЦО", null) || tt2.Next.IsValue("ДАЛЬШЕЙШЕМ", null) || tt2.Next.IsValue("ДАЛЕЕ", null)) 
                                                ok0 = true;
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                    if (org.Eponyms.Count == 0 && (t1.WhitespacesAfterCount < 2)) 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemEponymToken ep = Pullenti.Ner.Org.Internal.OrgItemEponymToken.TryAttach(t1.Next, false);
                        if (ep != null) 
                        {
                            foreach (string e in ep.Eponyms) 
                            {
                                org.AddEponym(e);
                            }
                            ok0 = true;
                            t1 = ep.EndToken;
                        }
                    }
                    if (org.Names.Count == 0) 
                    {
                        s = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                        s1 = (te == null ? null : Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative));
                        org.AddName(s, true, br.BeginToken.Next);
                        org.AddName(s1, true, null);
                    }
                    if (!ok0) 
                    {
                        if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.CheckOrgSpecialWordBefore(t0.Previous)) 
                            ok0 = true;
                    }
                    if (!ok0 && attachTyp != AttachType.Normal) 
                        ok0 = true;
                    typ = _lastTyp(types);
                    if (typ != null && typ.BeginToken != typ.EndToken) 
                        ok0 = true;
                    if (ok0) 
                        return new Pullenti.Ner.ReferentToken(org, t0, t1);
                    else 
                        return new Pullenti.Ner.ReferentToken(org, t0, t1) { Tag = org };
                }
            }
            Pullenti.Ner.Org.Internal.OrgItemNumberToken num = null;
            Pullenti.Ner.Org.Internal.OrgItemNumberToken _num;
            Pullenti.Ner.Org.Internal.OrgItemEponymToken epon = null;
            Pullenti.Ner.Org.Internal.OrgItemEponymToken _epon;
            List<Pullenti.Ner.Org.Internal.OrgItemNameToken> names = null;
            Pullenti.Ner.Org.Internal.OrgItemNameToken pr = null;
            Pullenti.Ner.ReferentToken ownOrg = null;
            if (t1 == null) 
                t1 = t0;
            else if (t != null && t.Previous != null && t.Previous.BeginChar >= t0.BeginChar) 
                t1 = t.Previous;
            br = null;
            bool ok = false;
            for (; t != null; t = t.Next) 
            {
                if (t.GetReferent() is OrganizationReferent) 
                {
                }
                Pullenti.Ner.ReferentToken rt;
                if ((((rt = this.AttachGlobalOrg(t, attachTyp, ad, null)))) != null) 
                {
                    if (t == t0) 
                    {
                        if (!t.Chars.IsAllLower) 
                            return rt;
                        return null;
                    }
                    if (level == 0) 
                    {
                        rt = this.TryAttachOrg(t, null, attachTyp, multTyp, isAdditionalAttach, level + 1, -1);
                        if (rt != null) 
                            return rt;
                    }
                }
                if ((((_num = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(t, typ != null && typ.Root != null && typ.Root.CanHasNumber, typ)))) != null) 
                {
                    if ((typ == null || typ.Root == null || !typ.Root.CanHasNumber) || num != null) 
                        break;
                    if (t.WhitespacesBeforeCount > 2) 
                    {
                        if (typ.EndToken.Next == t && Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t) != null) 
                        {
                        }
                        else 
                            break;
                    }
                    if (typ.Root.CanonicText == "СУД" && typ.Name != null) 
                    {
                        if ((((typ.Name.StartsWith("ВЕРХОВНЫЙ") || typ.Name.StartsWith("АРБИТРАЖНЫЙ") || typ.Name.StartsWith("ВЫСШИЙ")) || typ.Name.StartsWith("КОНСТИТУЦИОН") || typ.Name.StartsWith("ВЕРХОВНИЙ")) || typ.Name.StartsWith("АРБІТРАЖНИЙ") || typ.Name.StartsWith("ВИЩИЙ")) || typ.Name.StartsWith("КОНСТИТУЦІЙН")) 
                        {
                            typ.Coef = 3;
                            break;
                        }
                    }
                    num = _num;
                    t1 = (t = num.EndToken);
                    continue;
                }
                if ((((_epon = Pullenti.Ner.Org.Internal.OrgItemEponymToken.TryAttach(t, false)))) != null) 
                {
                    epon = _epon;
                    t1 = (t = epon.EndToken);
                    continue;
                }
                if ((((typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t, false, ad)))) != null) 
                {
                    if (typ.Morph.Case.IsGenitive) 
                    {
                        if (typ.EndToken.IsValue("СЛУЖБА", null) || typ.EndToken.IsValue("УПРАВЛЕНИЕ", "УПРАВЛІННЯ") || typ.EndToken.IsValue("ХОЗЯЙСТВО", null)) 
                            typ = null;
                    }
                    if (typ != null) 
                    {
                        if (!typ.IsDoubtRootWord && attachTyp != AttachType.ExtOntology) 
                            break;
                        if (types == null && t0 == t) 
                            break;
                        if (_lastTyp(types) != null && attachTyp != AttachType.ExtOntology) 
                        {
                            if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticTT(typ, _lastTyp(types))) 
                            {
                                if (names != null && ((typ.Morph.Case.IsGenitive || typ.Morph.Case.IsInstrumental)) && (t.WhitespacesBeforeCount < 2)) 
                                {
                                }
                                else 
                                    break;
                            }
                        }
                    }
                }
                if ((((br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100)))) != null) 
                {
                    if (ownOrg != null && !(ownOrg.Referent as OrganizationReferent).IsFromGlobalOntos) 
                        break;
                    if (t.IsNewlineBefore && ((attachTyp == AttachType.Normal || attachTyp == AttachType.NormalAfterDep))) 
                        break;
                    typ = _lastTyp(types);
                    if ((org.FindSlot(OrganizationReferent.ATTR_TYPE, "организация", true) != null || org.FindSlot(OrganizationReferent.ATTR_TYPE, "движение", true) != null || org.FindSlot(OrganizationReferent.ATTR_TYPE, "організація", true) != null) || org.FindSlot(OrganizationReferent.ATTR_TYPE, "рух", true) != null) 
                    {
                        if (((typ == null || (typ.Coef < 2))) && !specWordBefore) 
                            return null;
                    }
                    if (br.IsQuoteType) 
                    {
                        if (br.OpenChar == '<' || br.WhitespacesBeforeCount > 1) 
                            break;
                        rt = this.TryAttachOrg(t, null, AttachType.High, null, false, level + 1, -1);
                        if (rt == null) 
                            break;
                        OrganizationReferent org0 = rt.Referent as OrganizationReferent;
                        if (names != null && names.Count == 1) 
                        {
                            if (((!names[0].IsNounPhrase && names[0].Chars.IsAllUpper)) || org0.Names.Count > 0) 
                            {
                                if (!names[0].BeginToken.Morph.Class.IsPreposition) 
                                {
                                    if (org0.Names.Count == 0) 
                                        org.AddTypeStr(names[0].Value);
                                    else 
                                    {
                                        foreach (string n in org0.Names) 
                                        {
                                            org.AddName(string.Format("{0} {1}", names[0].Value, n), true, null);
                                            if (typ != null && typ.Root != null && typ.Root.Typ != Pullenti.Ner.Org.Internal.OrgItemTypeTyp.Prefix) 
                                                org.AddName(string.Format("{0} {1} {2}", typ.Typ.ToUpper(), Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(names[0], Pullenti.Ner.Core.GetTextAttr.No), n), true, null);
                                        }
                                        if (typ != null) 
                                            typ.Coef = 4;
                                    }
                                    names = null;
                                }
                            }
                        }
                        if (names != null && names.Count > 0 && !specWordBefore) 
                            break;
                        if (!org.CanBeEquals(org0, Pullenti.Ner.Core.ReferentsEqualType.ForMerging)) 
                            break;
                        org.MergeSlots(org0, true);
                        t1 = (tMax = (t = rt.EndToken));
                        ok = true;
                        continue;
                    }
                    else if (br.OpenChar == '(') 
                    {
                        if (t.Next.GetReferent() != null && t.Next.Next == br.EndToken) 
                        {
                            Pullenti.Ner.Referent r = t.Next.GetReferent();
                            if (r.TypeName == GEONAME) 
                            {
                                org.AddGeoObject(r);
                                tMax = (t1 = (t = br.EndToken));
                                continue;
                            }
                        }
                        else if (((t.Next is Pullenti.Ner.TextToken) && t.Next.Chars.IsLetter && !t.Next.Chars.IsAllLower) && t.Next.Next == br.EndToken) 
                        {
                            typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t.Next, true, null);
                            if (typ != null) 
                            {
                                OrganizationReferent or0 = new OrganizationReferent();
                                or0.AddType(typ, false);
                                if (or0.Kind != OrganizationKind.Undefined && org.Kind != OrganizationKind.Undefined) 
                                {
                                    if (org.Kind != or0.Kind) 
                                        break;
                                }
                                if (Pullenti.Ner.Core.MiscHelper.TestAcronym(t.Next, t0, t.Previous)) 
                                    org.AddName(t.Next.GetSourceText(), true, null);
                                else 
                                    org.AddType(typ, false);
                                t1 = (t = (tMax = br.EndToken));
                                continue;
                            }
                            else 
                            {
                                Pullenti.Ner.Org.Internal.OrgItemNameToken nam = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(t.Next, null, attachTyp == AttachType.ExtOntology, true);
                                if (nam != null && nam.IsEmptyWord) 
                                    break;
                                if (attachTyp == AttachType.Normal) 
                                {
                                    OrganizationReferent org0 = new OrganizationReferent();
                                    org0.AddName((t.Next as Pullenti.Ner.TextToken).Term, true, t.Next);
                                    if (!OrganizationReferent.CanBeSecondDefinition(org, org0)) 
                                        break;
                                }
                                org.AddName((t.Next as Pullenti.Ner.TextToken).Term, true, t.Next);
                                tMax = (t1 = (t = br.EndToken));
                                continue;
                            }
                        }
                    }
                    break;
                }
                if (ownOrg != null) 
                {
                    if (names == null && t.IsValue("ПО", null)) 
                    {
                    }
                    else if (names != null && t.IsCommaAnd) 
                    {
                    }
                    else 
                        break;
                }
                typ = _lastTyp(types);
                if (typ != null && typ.Root != null && typ.Root.IsPurePrefix) 
                {
                    if (pr == null && names == null) 
                    {
                        pr = new Pullenti.Ner.Org.Internal.OrgItemNameToken(t, t);
                        pr.Morph.Case = Pullenti.Morph.MorphCase.Nominative;
                    }
                }
                Pullenti.Ner.Org.Internal.OrgItemNameToken na = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(t, pr, attachTyp == AttachType.ExtOntology, names == null);
                if (na == null && t != null) 
                {
                    if (org.Kind == OrganizationKind.Church || ((typ != null && typ.Typ != null && typ.Typ.Contains("фермер")))) 
                    {
                        Pullenti.Ner.ReferentToken prt = t.Kit.ProcessReferent("PERSON", t);
                        if (prt != null) 
                        {
                            na = new Pullenti.Ner.Org.Internal.OrgItemNameToken(t, prt.EndToken) { IsStdName = true };
                            na.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(na, Pullenti.Ner.Core.GetTextAttr.No);
                            na.Chars = new Pullenti.Morph.CharsInfo() { IsCapitalUpper = true };
                            na.Morph = prt.Morph;
                            string sur = prt.Referent.GetStringValue("LASTNAME");
                            if (sur != null) 
                            {
                                for (Pullenti.Ner.Token tt = t; tt != null && tt.EndChar <= prt.EndChar; tt = tt.Next) 
                                {
                                    if (tt.IsValue(sur, null)) 
                                    {
                                        na.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt, tt, Pullenti.Ner.Core.GetTextAttr.No);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (na == null) 
                {
                    if (attachTyp == AttachType.ExtOntology) 
                    {
                        if (t.IsChar(',') || t.IsAnd) 
                            continue;
                    }
                    if (t.GetReferent() is OrganizationReferent) 
                    {
                        ownOrg = t as Pullenti.Ner.ReferentToken;
                        continue;
                    }
                    if (t.IsValue("ПРИ", null) && (t.Next is Pullenti.Ner.ReferentToken) && (t.Next.GetReferent() is OrganizationReferent)) 
                    {
                        t = t.Next;
                        ownOrg = t as Pullenti.Ner.ReferentToken;
                        continue;
                    }
                    if ((((names == null && t.IsChar('/') && (t.Next is Pullenti.Ner.TextToken)) && !t.IsWhitespaceAfter && t.Next.Chars.IsAllUpper) && t.Next.LengthChar >= 3 && (t.Next.Next is Pullenti.Ner.TextToken)) && !t.Next.IsWhitespaceAfter && t.Next.Next.IsChar('/')) 
                        na = new Pullenti.Ner.Org.Internal.OrgItemNameToken(t, t.Next.Next) { Value = t.Next.GetSourceText().ToUpper(), Chars = t.Next.Chars };
                    else if (names == null && typ != null && ((typ.Typ == "движение" || org.Kind == OrganizationKind.Party))) 
                    {
                        Pullenti.Ner.Token tt1 = null;
                        if (t.IsValue("ЗА", null) || t.IsValue("ПРОТИВ", null)) 
                            tt1 = t.Next;
                        else if (t.IsValue("В", null) && t.Next != null) 
                        {
                            if (t.Next.IsValue("ЗАЩИТА", null) || t.Next.IsValue("ПОДДЕРЖКА", null)) 
                                tt1 = t.Next;
                        }
                        else if (typ.Chars.IsCapitalUpper && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(typ.BeginToken)) 
                        {
                            Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                            if ((mc.IsAdverb || mc.IsPronoun || mc.IsPersonalPronoun) || mc.IsVerb || mc.IsConjunction) 
                            {
                            }
                            else if (t.Chars.IsLetter) 
                                tt1 = t;
                            else if (typ.BeginToken != typ.EndToken) 
                                typ.Coef += 3;
                        }
                        if (tt1 != null) 
                        {
                            na = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(tt1, pr, true, false);
                            if (na != null) 
                            {
                                na.BeginToken = t;
                                typ.Coef += 3;
                            }
                        }
                    }
                    if (na == null) 
                        break;
                }
                if (num != null || epon != null) 
                    break;
                if (attachTyp == AttachType.Multiple || attachTyp == AttachType.Normal || attachTyp == AttachType.NormalAfterDep) 
                {
                    if (!na.IsStdTail && !na.Chars.IsLatinLetter && na.StdOrgNameNouns == 0) 
                    {
                        if (t.Morph.Class.IsProperName) 
                            break;
                        Pullenti.Morph.MorphClass cla = t.GetMorphClassInDictionary();
                        if (cla.IsProperSurname || ((t.Morph.Language.IsUa && t.Morph.Class.IsProperSurname))) 
                        {
                            if (names == null && ((org.Kind == OrganizationKind.Airport || org.Kind == OrganizationKind.Seaport))) 
                            {
                            }
                            else if (typ != null && typ.Root != null && typ.Root.Acronym == "ФОП") 
                            {
                            }
                            else if (typ != null && typ.Typ.Contains("фермер")) 
                            {
                            }
                            else 
                                break;
                        }
                        if (cla.IsUndefined && na.Chars.IsCyrillicLetter && na.Chars.IsCapitalUpper) 
                        {
                            if ((t.Previous != null && !t.Previous.Morph.Class.IsPreposition && !t.Previous.Morph.Class.IsConjunction) && t.Previous.Chars.IsAllLower) 
                            {
                                if ((t.Next != null && (t.Next is Pullenti.Ner.TextToken) && t.Next.Chars.IsLetter) && !t.Next.Chars.IsAllLower) 
                                    break;
                            }
                        }
                        if (typ != null && typ.Typ == "союз" && !t.Morph.Case.IsGenitive) 
                            break;
                        Pullenti.Ner.ReferentToken pit = t.Kit.ProcessReferent("PERSONPROPERTY", t);
                        if (pit != null) 
                        {
                            if (pit.Morph.Number == Pullenti.Morph.MorphNumber.Singular && pit.BeginToken != pit.EndToken) 
                                break;
                        }
                        pit = t.Kit.ProcessReferent("DECREE", t);
                        if (pit != null) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken nptt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (nptt != null && nptt.EndToken.IsValue("РЕШЕНИЕ", null)) 
                            {
                            }
                            else 
                                break;
                        }
                        if (t.NewlinesBeforeCount > 1) 
                            break;
                    }
                }
                if (t.IsValue("ИМЕНИ", "ІМЕНІ") || t.IsValue("ИМ", "ІМ")) 
                    break;
                pr = na;
                if (attachTyp == AttachType.ExtOntology) 
                {
                    if (names == null) 
                        names = new List<Pullenti.Ner.Org.Internal.OrgItemNameToken>();
                    names.Add(na);
                    t1 = (t = na.EndToken);
                    continue;
                }
                if (names == null) 
                {
                    if (tMax != null) 
                        break;
                    if (t.Previous != null && t.IsNewlineBefore && attachTyp != AttachType.ExtOntology) 
                    {
                        if (typ != null && typ.EndToken.Next == t && typ.IsNewlineBefore) 
                        {
                        }
                        else 
                        {
                            if (t.NewlinesAfterCount > 1 || !t.Chars.IsAllLower) 
                                break;
                            if (t.Morph.Class.IsPreposition && typ != null && (((typ.Typ == "комитет" || typ.Typ == "комиссия" || typ.Typ == "комітет") || typ.Typ == "комісія"))) 
                            {
                            }
                            else if (na.StdOrgNameNouns > 0) 
                            {
                            }
                            else 
                                break;
                        }
                    }
                    else if (t.Previous != null && t.WhitespacesBeforeCount > 1 && attachTyp != AttachType.ExtOntology) 
                    {
                        if (t.WhitespacesBeforeCount > 10) 
                            break;
                        if (t.Chars != t.Previous.Chars) 
                            break;
                    }
                    if (t.Chars.IsAllLower && org.Kind == OrganizationKind.Justice) 
                    {
                        if (t.IsValue("ПО", null) && t.Next != null && t.Next.IsValue("ПРАВО", null)) 
                        {
                        }
                        else if (t.IsValue("З", null) && t.Next != null && t.Next.IsValue("ПРАВ", null)) 
                        {
                        }
                        else 
                            break;
                    }
                    if (org.Kind == OrganizationKind.Federation) 
                    {
                        if (t.Morph.Class.IsPreposition || t.Morph.Class.IsConjunction) 
                            break;
                    }
                    if (t.Chars.IsAllLower && ((org.Kind == OrganizationKind.Airport || org.Kind == OrganizationKind.Seaport || org.Kind == OrganizationKind.Hotel))) 
                        break;
                    if ((typ != null && typ.LengthChar == 2 && ((typ.Typ == "АО" || typ.Typ == "СП"))) && !specWordBefore && attachTyp == AttachType.Normal) 
                    {
                        if (!na.Chars.IsLatinLetter) 
                            break;
                    }
                    if (t.Chars.IsLatinLetter && typ != null && Pullenti.Morph.LanguageHelper.EndsWithEx(typ.Typ, "служба", "сервис", "сервіс", null)) 
                        break;
                    if (typ != null && ((typ.Root == null || !typ.Root.IsPurePrefix))) 
                    {
                        if (typ.Chars.IsLatinLetter && na.Chars.IsLatinLetter) 
                        {
                            if (!t.IsValue("OF", null)) 
                                break;
                        }
                        if ((na.IsInDictionary && na.Morph.Language.IsCyrillic && na.Chars.IsAllLower) && !na.Morph.Case.IsUndefined) 
                        {
                            if (na.Preposition == null) 
                            {
                                if (!na.Morph.Case.IsGenitive) 
                                    break;
                                if (org.Kind == OrganizationKind.Party && !specWordBefore) 
                                {
                                    if (typ.Typ == "лига") 
                                    {
                                    }
                                    else 
                                        break;
                                }
                                if (na.Morph.Number != Pullenti.Morph.MorphNumber.Plural) 
                                {
                                    Pullenti.Ner.ReferentToken prr = t.Kit.ProcessReferent("PERSONPROPERTY", t);
                                    if (prr != null) 
                                    {
                                        if (Pullenti.Ner.Org.Internal.OrgItemEponymToken.TryAttach(na.EndToken.Next, false) != null) 
                                        {
                                        }
                                        else 
                                            break;
                                    }
                                }
                            }
                        }
                        if (na.Preposition != null) 
                        {
                            if (org.Kind == OrganizationKind.Party) 
                            {
                                if (na.Preposition == "ЗА" || na.Preposition == "ПРОТИВ") 
                                {
                                }
                                else if (na.Preposition == "В") 
                                {
                                    if (na.Value.StartsWith("ЗАЩИТ") && na.Value.StartsWith("ПОДДЕРЖ")) 
                                    {
                                    }
                                    else 
                                        break;
                                }
                                else 
                                    break;
                            }
                            else 
                            {
                                if (na.Preposition == "В") 
                                    break;
                                if (typ.IsDoubtRootWord) 
                                {
                                    if (Pullenti.Morph.LanguageHelper.EndsWithEx(typ.Typ, "комитет", "комиссия", "комітет", "комісія") && ((t.IsValue("ПО", null) || t.IsValue("З", null)))) 
                                    {
                                    }
                                    else if (names == null && na.StdOrgNameNouns > 0) 
                                    {
                                    }
                                    else 
                                        break;
                                }
                            }
                        }
                        else if (na.Chars.IsCapitalUpper && na.Chars.IsCyrillicLetter) 
                        {
                            Pullenti.Ner.ReferentToken prt = na.Kit.ProcessReferent("PERSON", na.BeginToken);
                            if (prt != null) 
                            {
                                if (org.Kind == OrganizationKind.Church) 
                                {
                                    na.EndToken = prt.EndToken;
                                    na.IsStdName = true;
                                    na.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(na, Pullenti.Ner.Core.GetTextAttr.No);
                                }
                                else if ((typ != null && typ.Typ != null && typ.Typ.Contains("фермер")) && names == null) 
                                    na.EndToken = prt.EndToken;
                                else 
                                    break;
                            }
                        }
                    }
                    if (na.IsEmptyWord) 
                        break;
                    if (na.IsStdTail) 
                    {
                        if (na.Chars.IsLatinLetter && na.Chars.IsAllUpper && (na.LengthChar < 4)) 
                        {
                            na.IsStdTail = false;
                            na.Value = na.GetSourceText().ToUpper();
                        }
                        else 
                            break;
                    }
                    names = new List<Pullenti.Ner.Org.Internal.OrgItemNameToken>();
                }
                else 
                {
                    Pullenti.Ner.Org.Internal.OrgItemNameToken na0 = names[names.Count - 1];
                    if (na0.IsStdTail) 
                        break;
                    if (na.Preposition == null) 
                    {
                        if ((!na.Chars.IsLatinLetter && na.Chars.IsAllLower && !na.IsAfterConjunction) && !na.Morph.Case.IsGenitive) 
                            break;
                    }
                }
                names.Add(na);
                t1 = (t = na.EndToken);
            }
            typ = _lastTyp(types);
            bool doHigherAlways = false;
            if (typ != null) 
            {
                if (((attachTyp == AttachType.Normal || attachTyp == AttachType.NormalAfterDep)) && typ.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                    return null;
                if (Pullenti.Morph.LanguageHelper.EndsWithEx(typ.Typ, "комитет", "комиссия", "комітет", "комісія")) 
                {
                }
                else if (typ.Typ == "служба" && ownOrg != null && typ.Name != null) 
                {
                    OrganizationKind ki = (ownOrg.Referent as OrganizationReferent).Kind;
                    if (ki == OrganizationKind.Press || ki == OrganizationKind.Media) 
                    {
                        typ.Coef += 3;
                        doHigherAlways = true;
                    }
                    else 
                        ownOrg = null;
                }
                else if ((typ.Typ == "служба" && ownOrg != null && num == null) && _isMvdOrg(ownOrg.Referent as OrganizationReferent) != null && (((((typ.BeginToken.Previous is Pullenti.Ner.NumberToken) && (typ.WhitespacesBeforeCount < 3))) || names != null))) 
                {
                    typ.Coef += 4;
                    if (typ.BeginToken.Previous is Pullenti.Ner.NumberToken) 
                    {
                        t0 = typ.BeginToken.Previous;
                        num = new Pullenti.Ner.Org.Internal.OrgItemNumberToken(t0, t0) { Number = (typ.BeginToken.Previous as Pullenti.Ner.NumberToken).Value };
                    }
                }
                else if ((((typ.IsDoubtRootWord || typ.Typ == "организация" || typ.Typ == "управление") || typ.Typ == "служба" || typ.Typ == "общество") || typ.Typ == "союз" || typ.Typ == "організація") || typ.Typ == "керування" || typ.Typ == "суспільство") 
                    ownOrg = null;
                if (org.Kind == OrganizationKind.Govenment) 
                {
                    if (names == null && ((typ.Name == null || string.Compare(typ.Name, typ.Typ, true) == 0))) 
                    {
                        if ((attachTyp != AttachType.ExtOntology && typ.Typ != "следственный комитет" && typ.Typ != "кабинет министров") && typ.Typ != "слідчий комітет") 
                        {
                            if (((typ.Typ == "администрация" || typ.Typ == "адміністрація")) && (typ.EndToken.Next is Pullenti.Ner.TextToken)) 
                            {
                                Pullenti.Ner.ReferentToken rt1 = typ.Kit.ProcessReferent("PERSONPROPERTY", typ.EndToken.Next);
                                if (rt1 != null && typ.EndToken.Next.Morph.Case.IsGenitive) 
                                {
                                    Pullenti.Ner.Geo.GeoReferent geo = rt1.Referent.GetSlotValue("REF") as Pullenti.Ner.Geo.GeoReferent;
                                    if (geo != null) 
                                    {
                                        org.AddName("АДМИНИСТРАЦИЯ " + (typ.EndToken.Next as Pullenti.Ner.TextToken).Term, true, null);
                                        org.AddGeoObject(geo);
                                        return new Pullenti.Ner.ReferentToken(org, typ.BeginToken, rt1.EndToken);
                                    }
                                }
                            }
                            if ((typ.Coef < 5) || typ.Chars.IsAllLower) 
                                return null;
                        }
                    }
                }
            }
            else if (names != null && names[0].Chars.IsAllLower) 
            {
                if (attachTyp != AttachType.ExtOntology) 
                    return null;
            }
            bool always = false;
            string name = null;
            if (((num != null || org.Number != null || epon != null) || attachTyp == AttachType.High || attachTyp == AttachType.ExtOntology) || ownOrg != null) 
            {
                int cou0 = org.Slots.Count;
                if (names != null) 
                {
                    if ((names.Count == 1 && names[0].Chars.IsAllUpper && attachTyp == AttachType.ExtOntology) && isAdditionalAttach) 
                        org.AddName(Pullenti.Ner.Core.MiscHelper.GetTextValue(names[0].BeginToken, names[names.Count - 1].EndToken, Pullenti.Ner.Core.GetTextAttr.No), true, names[0].BeginToken);
                    else 
                    {
                        name = Pullenti.Ner.Core.MiscHelper.GetTextValue(names[0].BeginToken, names[names.Count - 1].EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                        if ((names[0].IsNounPhrase && typ != null && typ.Root != null) && !typ.Root.IsPurePrefix && multTyp == null) 
                            name = string.Format("{0} {1}", typ.Name ?? typ.Typ.ToUpper(), name);
                    }
                }
                else if (typ != null && typ.Name != null && ((typ.Root == null || !typ.Root.IsPurePrefix))) 
                {
                    if (typ.Chars.IsAllLower && !typ.CanBeOrganization && (typ.NameWordsCount < 3)) 
                        org.AddTypeStr(typ.Name.ToLower());
                    else 
                        name = typ.Name;
                    if (typ != multTyp) 
                    {
                        if (t1.EndChar < typ.EndToken.EndChar) 
                            t1 = typ.EndToken;
                    }
                }
                if (name != null) 
                {
                    if (name.Length > MaxOrgName) 
                        return null;
                    org.AddName(name, true, null);
                }
                if (num != null) 
                    org.Number = num.Number;
                if (epon != null) 
                {
                    foreach (string e in epon.Eponyms) 
                    {
                        org.AddEponym(e);
                    }
                }
                ok = attachTyp == AttachType.ExtOntology;
                if (typ != null && typ.Root != null && typ.Root.CanBeNormalDep) 
                    ok = true;
                foreach (Pullenti.Ner.Slot a in org.Slots) 
                {
                    if (a.TypeName == OrganizationReferent.ATTR_NUMBER) 
                    {
                        if (typ != null && typ.Typ == "корпус") 
                        {
                        }
                        else 
                            ok = true;
                    }
                    else if (a.TypeName == OrganizationReferent.ATTR_GEO) 
                    {
                        if (typ.Root != null && typ.Root.CanBeSingleGeo) 
                            ok = true;
                    }
                    else if (a.TypeName != OrganizationReferent.ATTR_TYPE && a.TypeName != OrganizationReferent.ATTR_PROFILE) 
                    {
                        ok = true;
                        break;
                    }
                }
                if (attachTyp == AttachType.Normal) 
                {
                    if (typ == null) 
                        ok = false;
                    else if ((typ.EndChar - typ.BeginChar) < 2) 
                    {
                        if (num == null && epon == null) 
                            ok = false;
                        else if (epon == null) 
                        {
                            if (t1.IsWhitespaceAfter || t1.Next == null) 
                            {
                            }
                            else if (t1.Next.IsCharOf(".,;") && t1.Next.IsWhitespaceAfter) 
                            {
                            }
                            else 
                                ok = false;
                        }
                    }
                }
                if ((!ok && typ != null && typ.CanBeDepBeforeOrganization) && ownOrg != null) 
                {
                    org.AddTypeStr((ownOrg.Kit.BaseLanguage.IsUa ? "підрозділ" : "подразделение"));
                    org.Higher = ownOrg.Referent as OrganizationReferent;
                    t1 = ownOrg;
                    ok = true;
                }
                else if (typ != null && ownOrg != null && Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(ownOrg.Referent as OrganizationReferent, org, true)) 
                {
                    if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticOO(ownOrg.Referent as OrganizationReferent, org)) 
                    {
                        if (org.Kind == OrganizationKind.Department && !typ.CanBeDepBeforeOrganization) 
                        {
                        }
                        else 
                        {
                            org.Higher = ownOrg.Referent as OrganizationReferent;
                            if (t1.EndChar < ownOrg.EndChar) 
                                t1 = ownOrg;
                            ok = true;
                        }
                    }
                    else if (typ.Root != null && ((typ.Root.CanBeNormalDep || ownOrg.Referent.ToString().Contains("Сбербанк")))) 
                    {
                        org.Higher = ownOrg.Referent as OrganizationReferent;
                        if (t1.EndChar < ownOrg.EndChar) 
                            t1 = ownOrg;
                        ok = true;
                    }
                }
            }
            else if (names != null) 
            {
                if (typ == null) 
                {
                    if (names[0].IsStdName && specWordBefore) 
                    {
                        org.AddName(names[0].Value, true, null);
                        t1 = names[0].EndToken;
                        t = this.AttachTailAttributes(org, t1.Next, null, true, attachTyp, false);
                        if (t != null) 
                            t1 = t;
                        return new Pullenti.Ner.ReferentToken(org, t0, t1);
                    }
                    return null;
                }
                if (typ.Root != null && typ.Root.MustHasCapitalName) 
                {
                    if (names[0].Chars.IsAllLower) 
                        return null;
                }
                if (names[0].Chars.IsLatinLetter) 
                {
                    if (typ.Root != null && !typ.Root.CanHasLatinName) 
                    {
                        if (!typ.Chars.IsLatinLetter) 
                            return null;
                    }
                    if (names[0].Chars.IsAllLower && !typ.Chars.IsLatinLetter) 
                        return null;
                    StringBuilder tmp = new StringBuilder();
                    tmp.Append(names[0].Value);
                    t1 = names[0].EndToken;
                    for (int j = 1; j < names.Count; j++) 
                    {
                        if (!names[j].IsStdTail && ((names[j].IsNewlineBefore || !names[j].Chars.IsLatinLetter))) 
                        {
                            tMax = names[j].BeginToken.Previous;
                            if (typ.Geo == null && org.FindSlot(OrganizationReferent.ATTR_GEO, null, true) != null) 
                                org.Slots.Remove(org.FindSlot(OrganizationReferent.ATTR_GEO, null, true));
                            break;
                        }
                        else 
                        {
                            t1 = names[j].EndToken;
                            if (names[j].IsStdTail) 
                            {
                                Pullenti.Ner.Org.Internal.OrgItemEngItem ei = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttach(names[j].BeginToken, false);
                                if (ei != null) 
                                {
                                    org.AddTypeStr(ei.FullValue);
                                    if (ei.ShortValue != null) 
                                        org.AddTypeStr(ei.ShortValue);
                                }
                                break;
                            }
                            if (names[j - 1].EndToken.IsChar('.') && !names[j - 1].Value.EndsWith(".")) 
                                tmp.AppendFormat(".{0}", names[j].Value);
                            else 
                                tmp.AppendFormat(" {0}", names[j].Value);
                        }
                    }
                    if (tmp.Length > MaxOrgName) 
                        return null;
                    string nnn = tmp.ToString();
                    if (nnn.StartsWith("OF ") || nnn.StartsWith("IN ")) 
                        tmp.Insert(0, ((typ.Name ?? typ.Typ)).ToUpper() + " ");
                    if (tmp.Length < 3) 
                    {
                        if (tmp.Length < 2) 
                            return null;
                        if (types != null && names[0].Chars.IsAllUpper) 
                        {
                        }
                        else 
                            return null;
                    }
                    ok = true;
                    org.AddName(tmp.ToString(), true, null);
                }
                else if (typ.Root != null && typ.Root.IsPurePrefix) 
                {
                    Pullenti.Ner.TextToken tt = typ.EndToken as Pullenti.Ner.TextToken;
                    if (tt == null) 
                        return null;
                    if (tt.IsNewlineAfter) 
                    {
                        if (names[0].IsNewlineAfter && typ.IsNewlineBefore) 
                        {
                        }
                        else 
                            return null;
                    }
                    if (typ.BeginToken == typ.EndToken && tt.Chars.IsAllLower) 
                        return null;
                    if (names[0].Chars.IsAllLower) 
                    {
                        if (!names[0].Morph.Case.IsGenitive) 
                            return null;
                    }
                    t1 = names[0].EndToken;
                    for (int j = 1; j < names.Count; j++) 
                    {
                        if (names[j].IsNewlineBefore || names[j].Chars != names[0].Chars) 
                            break;
                        else 
                            t1 = names[j].EndToken;
                    }
                    ok = true;
                    name = Pullenti.Ner.Core.MiscHelper.GetTextValue(names[0].BeginToken, t1, Pullenti.Ner.Core.GetTextAttr.No);
                    if (num == null && (t1 is Pullenti.Ner.NumberToken) && (t1 as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) 
                    {
                        Pullenti.Ner.Token tt1 = t1.Previous;
                        if (tt1 != null && tt1.IsHiphen) 
                            tt1 = tt1.Previous;
                        if (tt1 != null && tt1.EndChar > names[0].BeginChar && (tt1 is Pullenti.Ner.TextToken)) 
                        {
                            name = Pullenti.Ner.Core.MiscHelper.GetTextValue(names[0].BeginToken, tt1, Pullenti.Ner.Core.GetTextAttr.No);
                            org.Number = (t1 as Pullenti.Ner.NumberToken).Value.ToString();
                        }
                    }
                    if (name.Length > MaxOrgName) 
                        return null;
                    org.AddName(name, true, names[0].BeginToken);
                }
                else 
                {
                    if (typ.IsDep) 
                        return null;
                    if (typ.Morph.Number == Pullenti.Morph.MorphNumber.Plural && attachTyp != AttachType.Multiple) 
                        return null;
                    StringBuilder tmp = new StringBuilder();
                    float koef = typ.Coef;
                    if (koef >= 4) 
                        always = true;
                    if (org.FindSlot(OrganizationReferent.ATTR_GEO, null, true) != null) 
                        koef += 1;
                    if (specWordBefore) 
                        koef += 1;
                    if (names[0].Chars.IsAllLower && typ.Chars.IsAllLower && !specWordBefore) 
                    {
                        if (koef >= 3) 
                        {
                            if (t != null && (t.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                            {
                            }
                            else 
                                koef -= 3;
                        }
                    }
                    if (typ.CharsRoot.IsCapitalUpper) 
                        koef += ((float)0.5);
                    if (types.Count > 1) 
                        koef += (types.Count - 1);
                    if (typ.Name != null) 
                    {
                        for (Pullenti.Ner.Token to = typ.BeginToken; to != typ.EndToken && to != null; to = to.Next) 
                        {
                            if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsStdAdjective(to, false)) 
                                koef += 2;
                            if (to.Chars.IsCapitalUpper) 
                                koef += ((float)0.5);
                        }
                    }
                    OrganizationKind ki = org.Kind;
                    if (attachTyp == AttachType.Multiple && ((typ.Name == null || typ.Name.Length == typ.Typ.Length))) 
                    {
                    }
                    else if ((((((ki == OrganizationKind.Media || ki == OrganizationKind.Party || ki == OrganizationKind.Press) || ki == OrganizationKind.Factory || ki == OrganizationKind.Airport) || ki == OrganizationKind.Seaport || ((typ.Root != null && typ.Root.MustHasCapitalName))) || ki == OrganizationKind.Bank || typ.Typ.Contains("предприятие")) || typ.Typ.Contains("организация") || typ.Typ.Contains("підприємство")) || typ.Typ.Contains("організація")) 
                    {
                        if (typ.Name != null) 
                            org.AddTypeStr(typ.Name.ToLower());
                    }
                    else 
                        tmp.Append(typ.Name ?? typ.Typ.ToUpper());
                    if (typ != multTyp) 
                        t1 = typ.EndToken;
                    for (int j = 0; j < names.Count; j++) 
                    {
                        if (((names[j].IsNewlineBefore && j > 0)) || names[j].IsNounPhrase != names[0].IsNounPhrase) 
                            break;
                        else if (names[j].Chars != names[0].Chars && names[j].BeginToken.Chars != names[0].Chars) 
                            break;
                        else 
                        {
                            if (j == 0 && names[j].Preposition == null && names[j].IsInDictionary) 
                            {
                                if (!names[j].Morph.Case.IsGenitive && ((typ.Root != null && !typ.Root.CanHasSingleName))) 
                                    break;
                            }
                            if (j == 0 && names[0].Preposition == "ПО" && (((typ.Typ == "комитет" || typ.Typ == "комиссия" || typ.Typ == "комітет") || typ.Typ == "комісія"))) 
                                koef += 2.5F;
                            if ((j == 0 && names[j].WhitespacesBeforeCount > 2 && names[j].NewlinesBeforeCount == 0) && names[j].BeginToken.Previous != null) 
                                koef -= (((float)names[j].WhitespacesBeforeCount) / 2);
                            if (names[j].IsStdName) 
                                koef += 4;
                            else if (names[j].StdOrgNameNouns > 0 && ((ki == OrganizationKind.Govenment || Pullenti.Morph.LanguageHelper.EndsWith(typ.Typ, "центр")))) 
                                koef += names[j].StdOrgNameNouns;
                            if (((ki == OrganizationKind.Airport || ki == OrganizationKind.Seaport)) && j == 0) 
                                koef++;
                            t1 = names[j].EndToken;
                            if (names[j].IsNounPhrase) 
                            {
                                if (!names[j].Chars.IsAllLower) 
                                {
                                    Pullenti.Morph.MorphCase ca = names[j].Morph.Case;
                                    if ((ca.IsDative || ca.IsGenitive || ca.IsInstrumental) || ca.IsPrepositional) 
                                        koef += ((float)0.5);
                                    else 
                                        continue;
                                }
                                else if (((j == 0 || names[j].IsAfterConjunction)) && names[j].Morph.Case.IsGenitive && names[j].Preposition == null) 
                                    koef += ((float)0.5);
                                if (j == (names.Count - 1)) 
                                {
                                    if (names[j].EndToken.Next is Pullenti.Ner.TextToken) 
                                    {
                                        if (names[j].EndToken.Next.GetMorphClassInDictionary().IsVerb) 
                                            koef += 0.5F;
                                    }
                                }
                            }
                            for (Pullenti.Ner.Token to = names[j].BeginToken; to != null; to = to.Next) 
                            {
                                if (to is Pullenti.Ner.TextToken) 
                                {
                                    if (attachTyp == AttachType.Normal || attachTyp == AttachType.NormalAfterDep) 
                                    {
                                        if (to.Chars.IsCapitalUpper) 
                                            koef += ((float)0.5);
                                        else if ((j == 0 && ((to.Chars.IsAllUpper || to.Chars.IsLastLower)) && to.LengthChar > 2) && typ.Root != null && typ.Root.CanHasLatinName) 
                                            koef += 1;
                                    }
                                    else if (to.Chars.IsAllUpper || to.Chars.IsCapitalUpper) 
                                        koef += 1;
                                }
                                if (to == names[j].EndToken) 
                                    break;
                            }
                        }
                    }
                    for (Pullenti.Ner.Token ttt = typ.BeginToken.Previous; ttt != null; ttt = ttt.Previous) 
                    {
                        if (ttt.GetReferent() is OrganizationReferent) 
                        {
                            koef += 1;
                            break;
                        }
                        else if (!(ttt is Pullenti.Ner.TextToken)) 
                            break;
                        else if (ttt.Chars.IsLetter) 
                            break;
                    }
                    OrganizationKind oki = org.Kind;
                    if (oki == OrganizationKind.Govenment || oki == OrganizationKind.Study || oki == OrganizationKind.Party) 
                        koef += names.Count;
                    if (attachTyp != AttachType.Normal && attachTyp != AttachType.NormalAfterDep) 
                        koef += 3;
                    Pullenti.Ner.Core.BracketSequenceToken br1 = null;
                    if ((t1.WhitespacesAfterCount < 2) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t1.Next, true, false)) 
                    {
                        br1 = Pullenti.Ner.Core.BracketHelper.TryParse(t1.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br1 != null && (br1.LengthChar < 30)) 
                        {
                            string sss = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br1, Pullenti.Ner.Core.GetTextAttr.No);
                            if (sss != null && sss.Length > 2) 
                            {
                                org.AddName(sss, true, br1.BeginToken.Next);
                                koef += 1;
                                t1 = br1.EndToken;
                            }
                            else 
                                br1 = null;
                        }
                    }
                    if (koef >= 3 && t1.Next != null) 
                    {
                        Pullenti.Ner.Referent r = t1.Next.GetReferent();
                        if (r != null && ((r.TypeName == GEONAME || r.TypeName == OrganizationReferent.OBJ_TYPENAME))) 
                            koef += ((float)1);
                        else if (this.IsGeo(t1.Next, false) != null) 
                            koef += ((float)1);
                        else if (t1.Next.IsChar('(') && this.IsGeo(t1.Next.Next, false) != null) 
                            koef += ((float)1);
                        else if (specWordBefore && t1.Kit.ProcessReferent("PERSON", t1.Next) != null) 
                            koef += ((float)1);
                    }
                    if (koef >= 4) 
                        ok = true;
                    if (!ok) 
                    {
                        if ((oki == OrganizationKind.Press || oki == OrganizationKind.Federation || org.Types.Contains("агентство")) || ((oki == OrganizationKind.Party && Pullenti.Ner.Org.Internal.OrgItemTypeToken.CheckOrgSpecialWordBefore(t0.Previous)))) 
                        {
                            if (!names[0].IsNewlineBefore && !names[0].Morph.Class.IsProper) 
                            {
                                if (names[0].Morph.Case.IsGenitive && names[0].IsInDictionary) 
                                {
                                    if (typ.Chars.IsAllLower && !names[0].Chars.IsAllLower) 
                                    {
                                        ok = true;
                                        t1 = names[0].EndToken;
                                    }
                                }
                                else if (!names[0].IsInDictionary && names[0].Chars.IsAllUpper) 
                                {
                                    ok = true;
                                    tmp.Length = 0;
                                    t1 = names[0].EndToken;
                                }
                            }
                        }
                    }
                    if ((!ok && oki == OrganizationKind.Federation && names[0].Morph.Case.IsGenitive) && koef > 0) 
                    {
                        if (this.IsGeo(names[names.Count - 1].EndToken.Next, false) != null) 
                            ok = true;
                    }
                    if (!ok && typ != null && typ.Root != null) 
                    {
                        if (names.Count == 1 && ((names[0].Chars.IsAllUpper || names[0].Chars.IsLastLower))) 
                        {
                            if ((ki == OrganizationKind.Bank || ki == OrganizationKind.Culture || ki == OrganizationKind.Hotel) || ki == OrganizationKind.Media || ki == OrganizationKind.Medical) 
                                ok = true;
                        }
                    }
                    if (ok) 
                    {
                        Pullenti.Ner.Token tt1 = t1;
                        if (br1 != null) 
                            tt1 = br1.BeginToken.Previous;
                        if ((tt1.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && (tt1.GetReferent() as Pullenti.Ner.Geo.GeoReferent).IsState) 
                        {
                            if (names[0].BeginToken != tt1) 
                            {
                                tt1 = t1.Previous;
                                org.AddGeoObject(t1.GetReferent());
                            }
                        }
                        string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(names[0].BeginToken, tt1, Pullenti.Ner.Core.GetTextAttr.No);
                        if ((tt1 == names[0].EndToken && typ != null && typ.Typ != null) && typ.Typ.Contains("фермер") && names[0].Value != null) 
                            s = names[0].Value;
                        Pullenti.Morph.MorphClass cla = tt1.GetMorphClassInDictionary();
                        if ((names[0].BeginToken == t1 && s != null && t1.Morph.Case.IsGenitive) && t1.Chars.IsCapitalUpper) 
                        {
                            if (cla.IsUndefined || cla.IsProperGeo) 
                            {
                                if (ki == OrganizationKind.Medical || ki == OrganizationKind.Justice) 
                                {
                                    Pullenti.Ner.Geo.GeoReferent geo = new Pullenti.Ner.Geo.GeoReferent();
                                    geo.AddSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME, t1.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false), false, 0);
                                    geo.AddSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, (t1.Kit.BaseLanguage.IsUa ? "місто" : "город"), false, 0);
                                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(geo, t1, t1);
                                    rt.Data = ad;
                                    org.AddGeoObject(rt);
                                    s = null;
                                }
                            }
                        }
                        if (s != null) 
                        {
                            if (tmp.Length == 0) 
                            {
                                if (names[0].Morph.Case.IsGenitive || names[0].Preposition != null) 
                                {
                                    if (names[0].Chars.IsAllLower) 
                                        tmp.Append(typ.Name ?? typ.Typ);
                                }
                            }
                            if (tmp.Length > 0) 
                                tmp.Append(' ');
                            tmp.Append(s);
                        }
                        if (tmp.Length > MaxOrgName) 
                            return null;
                        org.AddName(tmp.ToString(), true, names[0].BeginToken);
                        if (types.Count > 1 && types[0].Name != null) 
                            org.AddTypeStr(types[0].Name.ToLower());
                    }
                }
            }
            else 
            {
                if (typ == null) 
                    return null;
                if (types.Count == 2 && types[0].Coef > typ.Coef) 
                    typ = types[0];
                if ((typ.Typ == "банк" && (t is Pullenti.Ner.ReferentToken) && !t.IsNewlineBefore) && typ.Morph.Number == Pullenti.Morph.MorphNumber.Singular) 
                {
                    if (typ.Name != null) 
                    {
                        if (typ.BeginToken.Chars.IsAllLower) 
                            org.AddTypeStr(typ.Name.ToLower());
                        else 
                        {
                            org.AddName(typ.Name, true, null);
                            string s0 = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(typ, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                            if (s0 != typ.Name) 
                                org.AddName(s0, true, null);
                        }
                    }
                    Pullenti.Ner.Referent r = t.GetReferent();
                    if (r.TypeName == GEONAME && t.Morph.Case != Pullenti.Morph.MorphCase.Nominative) 
                    {
                        org.AddGeoObject(r);
                        if (types.Count == 1 && (t.WhitespacesAfterCount < 3)) 
                        {
                            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ1 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t.Next, false, null);
                            if (typ1 != null && typ1.Root != null && typ1.Root.Typ == Pullenti.Ner.Org.Internal.OrgItemTypeTyp.Prefix) 
                            {
                                org.AddType(typ1, false);
                                t = typ1.EndToken;
                            }
                        }
                        return new Pullenti.Ner.ReferentToken(org, t0, t);
                    }
                }
                if (((typ.Root != null && typ.Root.IsPurePrefix)) && (typ.Coef < 4)) 
                    return null;
                if (typ.Root != null && typ.Root.MustHasCapitalName) 
                    return null;
                if (typ.Name == null) 
                {
                    if (((typ.Typ.EndsWith("университет") || typ.Typ.EndsWith("університет"))) && this.IsGeo(typ.EndToken.Next, false) != null) 
                        always = true;
                    else if (((org.Kind == OrganizationKind.Justice || org.Kind == OrganizationKind.Airport || org.Kind == OrganizationKind.Seaport)) && org.FindSlot(OrganizationReferent.ATTR_GEO, null, true) != null) 
                    {
                    }
                    else if (typ.Coef >= 4) 
                        always = true;
                    else if (typ.Chars.IsCapitalUpper) 
                    {
                        if (typ.EndToken.Next != null && ((typ.EndToken.Next.IsHiphen || typ.EndToken.Next.IsCharOf(":")))) 
                        {
                        }
                        else 
                        {
                            List<Pullenti.Ner.Core.IntOntologyItem> li = (ad == null ? null : ad.LocalOntology.TryAttachByItem(org.CreateOntologyItem()));
                            if (li != null && li.Count > 0) 
                            {
                                foreach (Pullenti.Ner.Core.IntOntologyItem ll in li) 
                                {
                                    Pullenti.Ner.Referent r = ll.Referent ?? (ll.Tag as Pullenti.Ner.Referent);
                                    if (r != null) 
                                    {
                                        if (org.CanBeEquals(r, Pullenti.Ner.Core.ReferentsEqualType.ForMerging)) 
                                        {
                                            Pullenti.Ner.Token ttt = typ.EndToken;
                                            Pullenti.Ner.Org.Internal.OrgItemNumberToken nu = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(ttt.Next, true, null);
                                            if (nu != null) 
                                            {
                                                if ((r as OrganizationReferent).Number != nu.Number) 
                                                    ttt = null;
                                                else 
                                                {
                                                    org.Number = nu.Number;
                                                    ttt = nu.EndToken;
                                                }
                                            }
                                            else if (li.Count > 1) 
                                                ttt = null;
                                            if (ttt != null) 
                                                return new Pullenti.Ner.ReferentToken(r, typ.BeginToken, ttt);
                                        }
                                    }
                                }
                            }
                        }
                        return null;
                    }
                    else 
                    {
                        int cou = 0;
                        for (Pullenti.Ner.Token tt = typ.BeginToken.Previous; tt != null && (cou < 200); tt = tt.Previous,cou++) 
                        {
                            OrganizationReferent org0 = tt.GetReferent() as OrganizationReferent;
                            if (org0 == null) 
                                continue;
                            if (!org0.CanBeEquals(org, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                continue;
                            tt = this.AttachTailAttributes(org, typ.EndToken.Next, ad, false, attachTyp, false) ?? typ.EndToken;
                            if (!org0.CanBeEquals(org, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                break;
                            org.MergeSlots(org0, true);
                            return new Pullenti.Ner.ReferentToken(org, typ.BeginToken, tt);
                        }
                        if (typ.Root != null && typ.Root.CanBeSingleGeo && t1.Next != null) 
                        {
                            object ggg = this.IsGeo(t1.Next, false);
                            if (ggg != null) 
                            {
                                org.AddGeoObject(ggg);
                                t1 = this.GetGeoEndToken(ggg, t1.Next);
                                return new Pullenti.Ner.ReferentToken(org, t0, t1);
                            }
                        }
                        return null;
                    }
                }
                if (typ.Morph.Number == Pullenti.Morph.MorphNumber.Plural || typ == multTyp) 
                    return null;
                float koef = typ.Coef;
                if (typ.NameWordsCount == 1 && typ.Name != null && typ.Name.Length > typ.Typ.Length) 
                    koef++;
                if (specWordBefore) 
                    koef += 1;
                ok = false;
                if (typ.CharsRoot.IsCapitalUpper) 
                {
                    koef += ((float)0.5);
                    if (typ.NameWordsCount == 1) 
                        koef += ((float)0.5);
                }
                if (epon != null) 
                    koef += 2;
                bool hasNonstdWords = false;
                for (Pullenti.Ner.Token to = typ.BeginToken; to != typ.EndToken && to != null; to = to.Next) 
                {
                    if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsStdAdjective(to, false)) 
                    {
                        if (typ.Root != null && typ.Root.Coeff > 0) 
                            koef += (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsStdAdjective(to, true) ? 1 : (int)0.5F);
                    }
                    else 
                        hasNonstdWords = true;
                    if (to.Chars.IsCapitalUpper && !to.Morph.Class.IsPronoun) 
                        koef += ((float)0.5);
                }
                if (!hasNonstdWords && org.Kind == OrganizationKind.Govenment) 
                    koef -= 2;
                if (typ.Chars.IsAllLower && (typ.Coef < 4)) 
                    koef -= 2;
                if (koef > 1 && typ.NameWordsCount > 2) 
                    koef += 2;
                for (Pullenti.Ner.Token ttt = typ.BeginToken.Previous; ttt != null; ttt = ttt.Previous) 
                {
                    if (ttt.GetReferent() is OrganizationReferent) 
                    {
                        koef += 1;
                        break;
                    }
                    else if (!(ttt is Pullenti.Ner.TextToken)) 
                        break;
                    else if (ttt.Chars.IsLetter) 
                        break;
                }
                for (Pullenti.Ner.Token ttt = typ.EndToken.Next; ttt != null; ttt = ttt.Next) 
                {
                    if (ttt.GetReferent() is OrganizationReferent) 
                    {
                        koef += 1;
                        break;
                    }
                    else if (!(ttt is Pullenti.Ner.TextToken)) 
                        break;
                    else if (ttt.Chars.IsLetter) 
                        break;
                }
                if (typ.WhitespacesBeforeCount > 4 && typ.WhitespacesAfterCount > 4) 
                    koef += ((float)0.5);
                if (typ.CanBeOrganization) 
                {
                    foreach (Pullenti.Ner.Slot s in org.Slots) 
                    {
                        if ((s.TypeName == OrganizationReferent.ATTR_EPONYM || s.TypeName == OrganizationReferent.ATTR_NAME || s.TypeName == OrganizationReferent.ATTR_GEO) || s.TypeName == OrganizationReferent.ATTR_NUMBER) 
                        {
                            koef += 3;
                            break;
                        }
                    }
                }
                org.AddType(typ, false);
                if (((org.Kind == OrganizationKind.Bank || org.Kind == OrganizationKind.Justice)) && typ.Name != null && typ.Name.Length > typ.Typ.Length) 
                    koef += 1;
                if (org.Kind == OrganizationKind.Justice && org.GeoObjects.Count > 0) 
                    always = true;
                if (org.Kind == OrganizationKind.Airport || org.Kind == OrganizationKind.Seaport) 
                {
                    foreach (Pullenti.Ner.Geo.GeoReferent g in org.GeoObjects) 
                    {
                        if (g.IsCity) 
                            always = true;
                    }
                }
                if (koef > 3 || always) 
                    ok = true;
                if (((org.Kind == OrganizationKind.Party || org.Kind == OrganizationKind.Justice)) && typ.Morph.Number == Pullenti.Morph.MorphNumber.Singular) 
                {
                    if (org.FindSlot(OrganizationReferent.ATTR_GEO, null, true) != null && typ.Name != null && typ.Name.Length > typ.Typ.Length) 
                        ok = true;
                    else if (typ.Coef >= 4) 
                        ok = true;
                    else if (typ.NameWordsCount > 2) 
                        ok = true;
                }
                if (ok) 
                {
                    if (typ.Name != null && !typ.IsNotTyp) 
                    {
                        if (typ.Name.Length > MaxOrgName || string.Compare(typ.Name, typ.Typ, true) == 0) 
                            return null;
                        org.AddName(typ.Name, true, null);
                    }
                    t1 = typ.EndToken;
                }
            }
            if (!ok || org.Slots.Count == 0) 
                return null;
            if (attachTyp == AttachType.Normal || attachTyp == AttachType.NormalAfterDep) 
            {
                ok = always;
                foreach (Pullenti.Ner.Slot s in org.Slots) 
                {
                    if (s.TypeName != OrganizationReferent.ATTR_TYPE && s.TypeName != OrganizationReferent.ATTR_PROFILE) 
                    {
                        ok = true;
                        break;
                    }
                }
                if (!ok) 
                    return null;
            }
            if (tMax != null && (t1.EndChar < tMax.BeginChar)) 
                t1 = tMax;
            t = this.AttachTailAttributes(org, t1.Next, null, true, attachTyp, false);
            if (t != null) 
                t1 = t;
            if (ownOrg != null && org.Higher == null) 
            {
                if (doHigherAlways || Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(ownOrg.Referent as OrganizationReferent, org, false)) 
                {
                    org.Higher = ownOrg.Referent as OrganizationReferent;
                    if (ownOrg.BeginChar > t1.BeginChar) 
                    {
                        t1 = ownOrg;
                        t = this.AttachTailAttributes(org, t1.Next, null, true, attachTyp, false);
                        if (t != null) 
                            t1 = t;
                    }
                }
            }
            if (((ownOrg != null && typ != null && typ.Typ == "банк") && typ.Geo != null && org.Higher == ownOrg.Referent) && ownOrg.Referent.ToString().Contains("Сбербанк")) 
            {
                Pullenti.Ner.Token tt2 = t1.Next;
                if (tt2 != null) 
                {
                    if (tt2.IsComma || tt2.IsValue("В", null)) 
                        tt2 = tt2.Next;
                }
                if (tt2 != null && (tt2.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    Pullenti.Ner.Slot s = org.FindSlot(OrganizationReferent.ATTR_GEO, null, true);
                    if (s != null) 
                        org.Slots.Remove(s);
                    if (org.AddGeoObject(tt2)) 
                        t1 = tt2;
                }
            }
            if (t1.IsNewlineAfter && t0.IsNewlineBefore) 
            {
                Pullenti.Ner.Org.Internal.OrgItemTypeToken typ1 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1.Next, false, null);
                if (typ1 != null && typ1.IsNewlineAfter) 
                {
                    if (this.TryAttachOrg(t1.Next, ad, AttachType.Normal, null, false, 0, -1) == null) 
                    {
                        org.AddType(typ1, false);
                        t1 = typ1.EndToken;
                    }
                }
                if (t1.Next != null && t1.Next.IsChar('(')) 
                {
                    if ((((typ1 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1.Next.Next, false, null)))) != null) 
                    {
                        if (typ1.EndToken.Next != null && typ1.EndToken.Next.IsChar(')') && typ1.EndToken.Next.IsNewlineAfter) 
                        {
                            org.AddType(typ1, false);
                            t1 = typ1.EndToken.Next;
                        }
                    }
                }
            }
            if (attachTyp == AttachType.Normal && ((typ == null || (typ.Coef < 4)))) 
            {
                if (org.FindSlot(OrganizationReferent.ATTR_GEO, null, true) == null || ((typ != null && typ.Geo != null))) 
                {
                    bool isAllLow = true;
                    for (t = t0; t != t1.Next; t = t.Next) 
                    {
                        if (t.Chars.IsLetter) 
                        {
                            if (!t.Chars.IsAllLower) 
                                isAllLow = false;
                        }
                        else if (!(t is Pullenti.Ner.TextToken)) 
                            isAllLow = false;
                    }
                    if (isAllLow && !specWordBefore) 
                        return null;
                }
            }
            Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(org, t0, t1);
            if (types != null && types.Count > 0) 
            {
                res.Morph = types[0].Morph;
                if (types[0].IsNotTyp && types[0].BeginToken == t0 && (types[0].EndChar < t1.EndChar)) 
                    res.BeginToken = types[0].EndToken.Next;
            }
            else 
                res.Morph = t0.Morph;
            if ((org.Number == null && t1.Next != null && (t1.WhitespacesAfterCount < 2)) && typ != null && ((typ.Root == null || typ.Root.CanHasNumber))) 
            {
                Pullenti.Ner.Org.Internal.OrgItemNumberToken num1 = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(t1.Next, false, typ);
                if (num1 == null && t1.Next.IsHiphen) 
                    num1 = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(t1.Next.Next, false, typ);
                if (num1 != null) 
                {
                    if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsDecreeKeyword(t0.Previous, 2)) 
                    {
                    }
                    else 
                    {
                        org.Number = num1.Number;
                        t1 = num1.EndToken;
                        res.EndToken = t1;
                    }
                }
            }
            return res;
        }
        Pullenti.Ner.ReferentToken TryAttachOrgBefore(Pullenti.Ner.Token t, OrgAnalyzerData ad)
        {
            if (t == null || t.Previous == null) 
                return null;
            int minEndChar = t.Previous.EndChar;
            int maxEndChar = t.EndChar;
            Pullenti.Ner.Token t0 = t.Previous;
            if ((t0 is Pullenti.Ner.ReferentToken) && (t0.GetReferent() is OrganizationReferent) && t0.Previous != null) 
            {
                minEndChar = t0.Previous.EndChar;
                t0 = t0.Previous;
            }
            Pullenti.Ner.ReferentToken res = null;
            for (; t0 != null; t0 = t0.Previous) 
            {
                if (t0.WhitespacesAfterCount > 1) 
                    break;
                int cou = 0;
                Pullenti.Ner.Token tt0 = t0;
                string num = null;
                Pullenti.Ner.Token numEt = null;
                for (Pullenti.Ner.Token ttt = t0; ttt != null; ttt = ttt.Previous) 
                {
                    if (ttt.WhitespacesAfterCount > 1) 
                        break;
                    if (ttt.IsHiphen || ttt.IsChar('.')) 
                        continue;
                    if (ttt is Pullenti.Ner.NumberToken) 
                    {
                        if (num != null) 
                            break;
                        num = (ttt as Pullenti.Ner.NumberToken).Value.ToString();
                        numEt = ttt;
                        tt0 = ttt.Previous;
                        continue;
                    }
                    Pullenti.Ner.Org.Internal.OrgItemNumberToken nn = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(ttt, false, null);
                    if (nn != null) 
                    {
                        num = nn.Number;
                        numEt = nn.EndToken;
                        tt0 = ttt.Previous;
                        continue;
                    }
                    if ((++cou) > 10) 
                        break;
                    if (ttt.IsValue("НАПРАВЛЕНИЕ", "НАПРЯМОК")) 
                    {
                        if (num != null || (((ttt.Previous is Pullenti.Ner.NumberToken) && (ttt.WhitespacesBeforeCount < 3)))) 
                        {
                            OrganizationReferent oo = new OrganizationReferent();
                            oo.AddProfile(OrgProfile.Unit);
                            oo.AddTypeStr(((ttt.Morph.Language.IsUa ? "НАПРЯМОК" : "НАПРАВЛЕНИЕ")).ToLower());
                            Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(oo, ttt, ttt);
                            if (numEt != null && num != null) 
                            {
                                oo.AddSlot(OrganizationReferent.ATTR_NUMBER, num, false, 0);
                                rt0.EndToken = numEt;
                                return rt0;
                            }
                            if (ttt.Previous is Pullenti.Ner.NumberToken) 
                            {
                                rt0.BeginToken = ttt.Previous;
                                oo.AddSlot(OrganizationReferent.ATTR_NUMBER, (ttt.Previous as Pullenti.Ner.NumberToken).Value.ToString(), false, 0);
                                return rt0;
                            }
                        }
                    }
                    Pullenti.Ner.Org.Internal.OrgItemTypeToken typ1 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(ttt, true, null);
                    if (typ1 == null) 
                    {
                        if (cou == 1) 
                            break;
                        continue;
                    }
                    if (typ1.EndToken == tt0) 
                        t0 = ttt;
                }
                Pullenti.Ner.ReferentToken rt = this.TryAttachOrg(t0, ad, AttachType.Normal, null, false, 0, -1);
                if (rt != null) 
                {
                    if (rt.EndChar >= minEndChar && rt.EndChar <= maxEndChar) 
                    {
                        OrganizationReferent oo = rt.Referent as OrganizationReferent;
                        if (oo.Higher != null && oo.Higher.Higher != null && oo.Higher == rt.EndToken.GetReferent()) 
                            return rt;
                        if (rt.BeginChar < t.BeginChar) 
                            return rt;
                        res = rt;
                    }
                    else 
                        break;
                }
                else if (!(t0 is Pullenti.Ner.TextToken)) 
                    break;
                else if (!t0.Chars.IsLetter) 
                {
                    if (!Pullenti.Ner.Core.BracketHelper.IsBracket(t0, false)) 
                        break;
                }
            }
            if (res != null) 
                return null;
            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ = null;
            for (t0 = t.Previous; t0 != null; t0 = t0.Previous) 
            {
                if (t0.WhitespacesAfterCount > 1) 
                    break;
                if (t0 is Pullenti.Ner.NumberToken) 
                    continue;
                if (t0.IsChar('.') || t0.IsHiphen) 
                    continue;
                if (!(t0 is Pullenti.Ner.TextToken)) 
                    break;
                if (!t0.Chars.IsLetter) 
                    break;
                Pullenti.Ner.Org.Internal.OrgItemTypeToken ty = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t0, true, ad);
                if (ty != null) 
                {
                    Pullenti.Ner.Org.Internal.OrgItemNumberToken nn = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(ty.EndToken.Next, true, ty);
                    if (nn != null) 
                    {
                        ty.EndToken = nn.EndToken;
                        ty.Number = nn.Number;
                    }
                    else if ((ty.EndToken.Next is Pullenti.Ner.NumberToken) && (ty.WhitespacesAfterCount < 2)) 
                    {
                        ty.EndToken = ty.EndToken.Next;
                        ty.Number = (ty.EndToken as Pullenti.Ner.NumberToken).Value.ToString();
                    }
                    if (ty.EndChar >= minEndChar && ty.EndChar <= maxEndChar) 
                        typ = ty;
                    else 
                        break;
                }
            }
            if (typ != null && typ.IsDep) 
                res = this.TryAttachDepBeforeOrg(typ, null);
            return res;
        }
        Pullenti.Ner.ReferentToken TryAttachDepBeforeOrg(Pullenti.Ner.Org.Internal.OrgItemTypeToken typ, Pullenti.Ner.ReferentToken rtOrg)
        {
            if (typ == null) 
                return null;
            OrganizationReferent org = (rtOrg == null ? null : rtOrg.Referent as OrganizationReferent);
            Pullenti.Ner.Token t = typ.EndToken;
            if (org == null) 
            {
                t = t.Next;
                if (t != null && ((t.IsValue("ПРИ", null) || t.IsValue("AT", null) || t.IsValue("OF", null)))) 
                    t = t.Next;
                if (t == null) 
                    return null;
                org = t.GetReferent() as OrganizationReferent;
            }
            else 
                t = rtOrg.EndToken;
            if (org == null) 
                return null;
            Pullenti.Ner.Token t1 = t;
            if (t1.Next is Pullenti.Ner.ReferentToken) 
            {
                Pullenti.Ner.Geo.GeoReferent geo0 = t1.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                if (geo0 != null && geo0.Alpha2 == "RU") 
                    t1 = t1.Next;
            }
            OrganizationReferent dep = new OrganizationReferent();
            dep.AddType(typ, false);
            if (typ.Name != null) 
            {
                string nam = typ.Name;
                if (char.IsDigit(nam[0])) 
                {
                    int i = nam.IndexOf(' ');
                    if (i > 0) 
                    {
                        dep.Number = nam.Substring(0, i);
                        nam = nam.Substring(i + 1).Trim();
                    }
                }
                dep.AddName(nam, true, null);
            }
            string ttt = (typ.Root != null ? typ.Root.CanonicText : typ.Typ.ToUpper());
            if ((((ttt == "ОТДЕЛЕНИЕ" || ttt == "ИНСПЕКЦИЯ" || ttt == "ВІДДІЛЕННЯ") || ttt == "ІНСПЕКЦІЯ")) && !t1.IsNewlineAfter) 
            {
                Pullenti.Ner.Org.Internal.OrgItemNumberToken num = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(t1.Next, false, typ);
                if (num != null) 
                {
                    dep.Number = num.Number;
                    t1 = num.EndToken;
                }
            }
            if (dep.Types.Contains("главное управление") || dep.Types.Contains("головне управління") || dep.TypeName.Contains("пограничное управление")) 
            {
                if (typ.BeginToken == typ.EndToken) 
                {
                    if (org.Kind != OrganizationKind.Govenment && org.Kind != OrganizationKind.Bank) 
                        return null;
                }
            }
            if (!Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org, dep, false) && ((typ.Root == null || !typ.Root.CanBeNormalDep))) 
            {
                if (dep.Types.Count > 0 && org.Types.Contains(dep.Types[0]) && dep.CanBeEquals(org, Pullenti.Ner.Core.ReferentsEqualType.ForMerging)) 
                    dep.MergeSlots(org, false);
                else if (typ.Typ == "управление" || typ.Typ == "управління") 
                    dep.Higher = org;
                else 
                    return null;
            }
            else 
                dep.Higher = org;
            Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(dep, typ.BeginToken, t1);
            this.CorrectDepAttrs(res, typ, false);
            if (typ.Root != null && !typ.Root.CanBeNormalDep && dep.Number == null) 
            {
                if (typ.Name != null && typ.Name.Contains(" ")) 
                {
                }
                else if (dep.FindSlot(OrganizationReferent.ATTR_GEO, null, true) != null) 
                {
                }
                else if (typ.Root.Coeff > 0 && typ.Morph.Number != Pullenti.Morph.MorphNumber.Plural) 
                {
                }
                else if (typ.Typ == "управління" && typ.Chars.IsCapitalUpper) 
                {
                }
                else 
                    return null;
            }
            return res;
        }
        Pullenti.Ner.ReferentToken TryAttachDepAfterOrg(Pullenti.Ner.Org.Internal.OrgItemTypeToken typ)
        {
            if (typ == null) 
                return null;
            Pullenti.Ner.Token t = typ.BeginToken.Previous;
            if (t != null && t.IsCharOf(":(")) 
                t = t.Previous;
            if (t == null) 
                return null;
            OrganizationReferent org = t.GetReferent() as OrganizationReferent;
            if (org == null) 
                return null;
            Pullenti.Ner.Token t1 = typ.EndToken;
            OrganizationReferent dep = new OrganizationReferent();
            dep.AddType(typ, false);
            if (typ.Name != null) 
                dep.AddName(typ.Name, true, null);
            if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(org, dep, false)) 
                dep.Higher = org;
            else if (Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(dep, org, false) && org.Higher == null) 
            {
                org.Higher = dep;
                t = t.Next;
            }
            else 
                t = t.Next;
            Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(dep, t, t1);
            this.CorrectDepAttrs(res, typ, false);
            if (dep.FindSlot(OrganizationReferent.ATTR_GEO, null, true) == null) 
                return null;
            return res;
        }
        Pullenti.Ner.ReferentToken TryAttachDep(Pullenti.Ner.Org.Internal.OrgItemTypeToken typ, AttachType attachTyp, bool specWordBefore)
        {
            if (typ == null) 
                return null;
            OrganizationReferent afterOrg = null;
            bool afterOrgTemp = false;
            if ((typ.IsNewlineAfter && typ.Name == null && typ.Typ != "курс") && ((typ.Root == null || !typ.Root.CanBeNormalDep))) 
            {
                Pullenti.Ner.Token tt2 = typ.EndToken.Next;
                if (!specWordBefore || tt2 == null) 
                    return null;
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt2, false, false)) 
                {
                }
                else 
                    return null;
            }
            if (typ.EndToken.Next != null && (typ.EndToken.WhitespacesAfterCount < 2)) 
            {
                Pullenti.Ner.Org.Internal.OrgItemNameToken na0 = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(typ.EndToken.Next, null, false, true);
                bool inBr = false;
                if (na0 != null && ((na0.StdOrgNameNouns > 0 || na0.IsStdName))) 
                    specWordBefore = true;
                else 
                {
                    Pullenti.Ner.ReferentToken rt00 = this.TryAttachOrg(typ.EndToken.Next, null, AttachType.NormalAfterDep, null, false, 0, -1);
                    if (rt00 == null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(typ.EndToken.Next, true, false)) 
                    {
                        rt00 = this.TryAttachOrg(typ.EndToken.Next.Next, null, AttachType.NormalAfterDep, null, false, 0, -1);
                        if (rt00 != null) 
                        {
                            inBr = true;
                            if (rt00.EndToken.Next == null) 
                            {
                            }
                            else if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt00.EndToken, true, null, false)) 
                            {
                            }
                            else if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt00.EndToken.Next, true, null, false)) 
                                rt00.EndToken = rt00.EndToken.Next;
                            else 
                                rt00 = null;
                            if (rt00 != null) 
                                rt00.BeginToken = typ.EndToken.Next;
                        }
                    }
                    if (rt00 != null) 
                    {
                        afterOrg = rt00.Referent as OrganizationReferent;
                        specWordBefore = true;
                        afterOrgTemp = true;
                        if (afterOrg.ContainsProfile(OrgProfile.Unit) && inBr) 
                        {
                            afterOrg = null;
                            afterOrgTemp = false;
                        }
                    }
                    else if ((typ.EndToken.Next is Pullenti.Ner.TextToken) && typ.EndToken.Next.Chars.IsAllUpper) 
                    {
                        List<Pullenti.Ner.ReferentToken> rrr = this.TryAttachOrgs(typ.EndToken.Next, null, 0);
                        if (rrr != null && rrr.Count == 1) 
                        {
                            afterOrg = rrr[0].Referent as OrganizationReferent;
                            specWordBefore = true;
                            afterOrgTemp = true;
                        }
                    }
                }
            }
            if (((((((typ.Root != null && typ.Root.CanBeNormalDep && !specWordBefore) && typ.Typ != "отделение" && typ.Typ != "инспекция") && typ.Typ != "филиал" && typ.Typ != "аппарат") && typ.Typ != "відділення" && typ.Typ != "інспекція") && typ.Typ != "філія" && typ.Typ != "апарат") && typ.Typ != "совет" && typ.Typ != "рада") && (typ.Typ.IndexOf(' ') < 0) && attachTyp != AttachType.ExtOntology) 
                return null;
            if (typ.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
            {
                if (!typ.BeginToken.IsValue("ОСП", null)) 
                    return null;
            }
            OrganizationReferent dep = null;
            Pullenti.Ner.Token t0 = typ.BeginToken;
            Pullenti.Ner.Token t1 = typ.EndToken;
            dep = new OrganizationReferent();
            dep.AddTypeStr(typ.Typ.ToLower());
            dep.AddProfile(OrgProfile.Unit);
            if (typ.Number != null) 
                dep.Number = typ.Number;
            else if (typ.Typ == "курс" && !typ.IsNewlineBefore) 
            {
                Pullenti.Ner.NumberToken nnn = Pullenti.Ner.Core.NumberHelper.TryParseRomanBack(typ.BeginToken.Previous);
                if (nnn != null && nnn.IntValue != null) 
                {
                    if (nnn.IntValue.Value >= 1 && nnn.IntValue.Value <= 6) 
                    {
                        dep.Number = nnn.Value.ToString();
                        t0 = nnn.BeginToken;
                    }
                }
            }
            Pullenti.Ner.Token t = typ.EndToken.Next;
            t1 = typ.EndToken;
            if ((t is Pullenti.Ner.TextToken) && afterOrg == null && (((Pullenti.Morph.LanguageHelper.EndsWith(typ.Typ, "аппарат") || Pullenti.Morph.LanguageHelper.EndsWith(typ.Typ, "апарат") || Pullenti.Morph.LanguageHelper.EndsWith(typ.Typ, "совет")) || Pullenti.Morph.LanguageHelper.EndsWith(typ.Typ, "рада")))) 
            {
                Pullenti.Ner.Token tt1 = t;
                if (tt1.IsValue("ПРИ", null)) 
                    tt1 = tt1.Next;
                Pullenti.Ner.ReferentToken pr1 = t.Kit.ProcessReferent("PERSON", tt1);
                if (pr1 != null && pr1.Referent.TypeName == "PERSONPROPERTY") 
                {
                    dep.AddSlot(OrganizationReferent.ATTR_OWNER, pr1.Referent, true, 0);
                    pr1.SetDefaultLocalOnto(t.Kit.Processor);
                    dep.AddExtReferent(pr1);
                    if (Pullenti.Morph.LanguageHelper.EndsWith(typ.Typ, "рат")) 
                        return new Pullenti.Ner.ReferentToken(dep, t0, pr1.EndToken);
                    t1 = pr1.EndToken;
                    t = t1.Next;
                }
            }
            Pullenti.Ner.Referent beforeOrg = null;
            for (Pullenti.Ner.Token ttt = typ.BeginToken.Previous; ttt != null; ttt = ttt.Previous) 
            {
                if (ttt.GetReferent() is OrganizationReferent) 
                {
                    beforeOrg = ttt.GetReferent();
                    break;
                }
                else if (!(ttt is Pullenti.Ner.TextToken)) 
                    break;
                else if (ttt.Chars.IsLetter) 
                    break;
            }
            Pullenti.Ner.Org.Internal.OrgItemNumberToken num = null;
            List<Pullenti.Ner.Org.Internal.OrgItemNameToken> names = null;
            Pullenti.Ner.Core.BracketSequenceToken br = null;
            Pullenti.Ner.Core.BracketSequenceToken br00 = null;
            Pullenti.Ner.Org.Internal.OrgItemNameToken pr = null;
            Pullenti.Ner.Org.Internal.OrgItemTypeToken ty0;
            bool isPureOrg = false;
            bool isPureDep = false;
            if (typ.Typ == "операционное управление" || typ.Typ == "операційне управління") 
                isPureDep = true;
            Pullenti.Ner.Token afterOrgTok = null;
            Pullenti.Ner.Core.BracketSequenceToken brName = null;
            float coef = typ.Coef;
            for (; t != null; t = t.Next) 
            {
                if (afterOrgTemp) 
                    break;
                if (t.IsChar(':')) 
                {
                    if (t.IsNewlineAfter) 
                        break;
                    if (names != null || typ.Name != null) 
                        break;
                    continue;
                }
                if ((((num = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(t, false, typ)))) != null) 
                {
                    if (t.IsNewlineBefore || typ.Number != null) 
                        break;
                    if ((typ.BeginToken.Previous is Pullenti.Ner.NumberToken) && (typ.WhitespacesBeforeCount < 2)) 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemTypeToken typ2 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(num.EndToken.Next, true, null);
                        if (typ2 != null && typ2.Root != null && ((typ2.Root.CanHasNumber || typ2.IsDep))) 
                        {
                            typ.BeginToken = typ.BeginToken.Previous;
                            typ.Number = (typ.BeginToken as Pullenti.Ner.NumberToken).Value;
                            dep.Number = typ.Number;
                            num = null;
                            coef += 1;
                            break;
                        }
                    }
                    t1 = num.EndToken;
                    t = num.EndToken.Next;
                    break;
                }
                else if ((((ty0 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t, true, null)))) != null && ty0.Morph.Number != Pullenti.Morph.MorphNumber.Plural && !ty0.IsDoubtRootWord) 
                    break;
                else if ((((br00 = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100)))) != null && names == null) 
                {
                    br = br00;
                    if (!br.IsQuoteType || brName != null) 
                        br = null;
                    else if (t.IsNewlineBefore && !specWordBefore) 
                        br = null;
                    else 
                    {
                        bool ok1 = true;
                        for (Pullenti.Ner.Token tt = br.BeginToken; tt != br.EndToken; tt = tt.Next) 
                        {
                            if (tt is Pullenti.Ner.ReferentToken) 
                            {
                                ok1 = false;
                                break;
                            }
                        }
                        if (ok1) 
                        {
                            brName = br;
                            t1 = br.EndToken;
                            t = t1.Next;
                        }
                        else 
                            br = null;
                    }
                    break;
                }
                else 
                {
                    Pullenti.Ner.Referent r = t.GetReferent();
                    if ((r == null && t.Morph.Class.IsPreposition && t.Next != null) && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                        dep.AddGeoObject(t.Next.GetReferent());
                        t = t.Next;
                        break;
                    }
                    if (r != null) 
                    {
                        if (r is OrganizationReferent) 
                        {
                            afterOrg = r as OrganizationReferent;
                            afterOrgTok = t;
                            break;
                        }
                        if ((r is Pullenti.Ner.Geo.GeoReferent) && names != null && t.Previous != null) 
                        {
                            bool isName = false;
                            if (t.Previous.IsValue("СУБЪЕКТ", null) || t.Previous.IsValue("СУБЄКТ", null)) 
                                isName = true;
                            if (!isName) 
                                break;
                        }
                        else 
                            break;
                    }
                    Pullenti.Ner.Org.Internal.OrgItemEponymToken epo = Pullenti.Ner.Org.Internal.OrgItemEponymToken.TryAttach(t, true);
                    if (epo != null) 
                    {
                        foreach (string e in epo.Eponyms) 
                        {
                            dep.AddEponym(e);
                        }
                        t1 = epo.EndToken;
                        break;
                    }
                    if (!typ.Chars.IsAllUpper && t.Chars.IsAllUpper) 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemNameToken na1 = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(t, pr, attachTyp == AttachType.ExtOntology, false);
                        if (na1 != null && ((na1.IsStdName || na1.StdOrgNameNouns > 0))) 
                        {
                        }
                        else 
                            break;
                    }
                    if ((t is Pullenti.Ner.NumberToken) && typ.Root != null && dep.Number == null) 
                    {
                        if (t.WhitespacesBeforeCount > 1) 
                            break;
                        if ((typ.BeginToken.Previous is Pullenti.Ner.NumberToken) && (typ.WhitespacesBeforeCount < 2)) 
                        {
                            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ2 = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t.Next, true, null);
                            if (typ2 != null && typ2.Root != null && ((typ2.Root.CanHasNumber || typ2.IsDep))) 
                            {
                                typ.BeginToken = typ.BeginToken.Previous;
                                dep.Number = (typ.Number = (typ.BeginToken as Pullenti.Ner.NumberToken).Value);
                                coef += 1;
                                break;
                            }
                        }
                        dep.Number = (t as Pullenti.Ner.NumberToken).Value.ToString();
                        t1 = t;
                        continue;
                    }
                    if (isPureDep) 
                        break;
                    if (!t.Chars.IsAllLower) 
                    {
                        Pullenti.Ner.ReferentToken rtp = t.Kit.ProcessReferent("PERSON", t);
                        if (rtp != null && rtp.Referent.TypeName == "PERSONPROPERTY") 
                        {
                            if (rtp.Morph.Case.IsGenitive && t == typ.EndToken.Next && (t.WhitespacesBeforeCount < 4)) 
                                rtp = null;
                        }
                        if (rtp != null) 
                            break;
                    }
                    if (typ.Typ == "генеральный штаб" || typ.Typ == "генеральний штаб") 
                    {
                        Pullenti.Ner.ReferentToken rtp = t.Kit.ProcessReferent("PERSONPROPERTY", t);
                        if (rtp != null) 
                            break;
                    }
                    Pullenti.Ner.Org.Internal.OrgItemNameToken na = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(t, pr, attachTyp == AttachType.ExtOntology, names == null);
                    if (t.IsValue("ПО", null) && t.Next != null && t.Next.IsValue("РАЙОН", null)) 
                        na = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(t.Next.Next, pr, attachTyp == AttachType.ExtOntology, true);
                    if (t.Morph.Class.IsPreposition && ((t.IsValue("ПРИ", null) || t.IsValue("OF", null) || t.IsValue("AT", null)))) 
                    {
                        if ((t.Next is Pullenti.Ner.ReferentToken) && (t.Next.GetReferent() is OrganizationReferent)) 
                        {
                            afterOrg = t.Next.GetReferent() as OrganizationReferent;
                            break;
                        }
                        Pullenti.Ner.ReferentToken rt0 = this.TryAttachOrg(t.Next, null, AttachType.NormalAfterDep, null, false, 0, -1);
                        if (rt0 != null) 
                        {
                            afterOrg = rt0.Referent as OrganizationReferent;
                            afterOrgTemp = true;
                            break;
                        }
                    }
                    if (na == null) 
                        break;
                    if (names == null) 
                    {
                        if (t.IsNewlineBefore) 
                            break;
                        if (Pullenti.Ner.Core.NumberHelper.TryParseRoman(t) != null) 
                            break;
                        Pullenti.Ner.ReferentToken rt0 = this.TryAttachOrg(t, null, AttachType.NormalAfterDep, null, false, 0, -1);
                        if (rt0 != null) 
                        {
                            afterOrg = rt0.Referent as OrganizationReferent;
                            afterOrgTemp = true;
                            break;
                        }
                        names = new List<Pullenti.Ner.Org.Internal.OrgItemNameToken>();
                    }
                    else 
                    {
                        if (t.WhitespacesBeforeCount > 2 && na.Chars != pr.Chars) 
                            break;
                        if (t.NewlinesBeforeCount > 2) 
                            break;
                    }
                    names.Add(na);
                    pr = na;
                    t1 = (t = na.EndToken);
                }
            }
            if (afterOrg == null) 
            {
                for (Pullenti.Ner.Token ttt = t; ttt != null; ttt = ttt.Next) 
                {
                    if (ttt.GetReferent() is OrganizationReferent) 
                    {
                        afterOrg = ttt.GetReferent() as OrganizationReferent;
                        break;
                    }
                    else if (!(ttt is Pullenti.Ner.TextToken)) 
                        break;
                    else if ((ttt.Chars.IsLetter && !ttt.IsValue("ПРИ", null) && !ttt.IsValue("В", null)) && !ttt.IsValue("OF", null) && !ttt.IsValue("AT", null)) 
                        break;
                }
            }
            if ((afterOrg == null && t != null && t != t0) && (t.WhitespacesBeforeCount < 2)) 
            {
                Pullenti.Ner.ReferentToken rt0 = this.TryAttachOrg(t, null, AttachType.NormalAfterDep, null, false, 0, -1);
                if (rt0 == null && (((t.IsValue("В", null) || t.IsValue("ПРИ", null) || t.IsValue("OF", null)) || t.IsValue("AT", null)))) 
                    rt0 = this.TryAttachOrg(t.Next, null, AttachType.NormalAfterDep, null, false, 0, -1);
                if (rt0 != null) 
                {
                    afterOrg = rt0.Referent as OrganizationReferent;
                    afterOrgTemp = true;
                }
            }
            if (typ.Chars.IsCapitalUpper) 
                coef += 0.5F;
            if (br != null && names == null) 
            {
                string nam = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                if (!string.IsNullOrEmpty(nam)) 
                {
                    if (nam.Length > 100) 
                        return null;
                    coef += 3;
                    Pullenti.Ner.Org.Internal.OrgItemNameToken na = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(br.BeginToken.Next, null, false, true);
                    if (na != null && na.IsStdName) 
                    {
                        coef += 1;
                        if (typ.Typ == "группа") 
                        {
                            dep.Slots.Clear();
                            typ.Typ = "группа компаний";
                            isPureOrg = true;
                        }
                        else if (typ.Typ == "група") 
                        {
                            dep.Slots.Clear();
                            typ.Typ = "група компаній";
                            isPureOrg = true;
                        }
                    }
                    if (isPureOrg) 
                    {
                        dep.AddType(typ, false);
                        dep.AddName(nam, true, null);
                    }
                    else 
                        dep.AddNameStr(nam, typ, 1);
                }
            }
            else if (names != null) 
            {
                int j;
                if (afterOrg != null || attachTyp == AttachType.High) 
                {
                    coef += 3;
                    j = names.Count;
                }
                else 
                    for (j = 0; j < names.Count; j++) 
                    {
                        if (((names[j].IsNewlineBefore && !typ.IsNewlineBefore && !names[j].IsAfterConjunction)) || ((names[j].Chars != names[0].Chars && names[j].StdOrgNameNouns == 0))) 
                            break;
                        else 
                        {
                            if (names[j].Chars == typ.Chars && !typ.Chars.IsAllLower) 
                                coef += ((float)0.5);
                            if (names[j].IsStdName) 
                                coef += 2;
                            if (names[j].StdOrgNameNouns > 0) 
                            {
                                if (!typ.Chars.IsAllLower) 
                                    coef += names[j].StdOrgNameNouns;
                            }
                        }
                    }
                t1 = names[j - 1].EndToken;
                string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(names[0].BeginToken, t1, Pullenti.Ner.Core.GetTextAttr.No);
                if (!string.IsNullOrEmpty(s)) 
                {
                    if (s.Length > 150 && attachTyp != AttachType.ExtOntology) 
                        return null;
                    dep.AddNameStr(s, typ, 1);
                }
                if (num != null) 
                {
                    dep.Number = num.Number;
                    coef += 2;
                    t1 = num.EndToken;
                }
            }
            else if (num != null) 
            {
                dep.Number = num.Number;
                coef += 2;
                t1 = num.EndToken;
                if (typ != null && ((typ.Typ == "лаборатория" || typ.Typ == "лабораторія"))) 
                    coef += 1;
                if (typ.Name != null) 
                    dep.AddNameStr(null, typ, 1);
            }
            else if (typ.Name != null) 
            {
                if (typ.Typ == "курс" && char.IsDigit(typ.Name[0])) 
                    dep.Number = typ.Name.Substring(0, typ.Name.IndexOf(' '));
                else 
                    dep.AddNameStr(null, typ, 1);
            }
            else if (typ.Typ == "кафедра" || typ.Typ == "факультет") 
            {
                t = typ.EndToken.Next;
                if (t != null && t.IsChar(':')) 
                    t = t.Next;
                if ((t != null && (t is Pullenti.Ner.TextToken) && !t.IsNewlineBefore) && t.Morph.Class.IsAdjective) 
                {
                    if (typ.Morph.Gender == t.Morph.Gender) 
                    {
                        string s = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                        if (s != null) 
                        {
                            dep.AddNameStr(string.Format("{0} {1}", s, typ.Typ.ToUpper()), null, 1);
                            coef += 2;
                            t1 = t;
                        }
                    }
                }
            }
            else if (typ.Typ == "курс") 
            {
                t = typ.EndToken.Next;
                if (t != null && t.IsChar(':')) 
                    t = t.Next;
                if (t != null && !t.IsNewlineBefore) 
                {
                    int val = 0;
                    if (t is Pullenti.Ner.NumberToken) 
                    {
                        if (!t.Morph.Class.IsNoun && (t as Pullenti.Ner.NumberToken).IntValue != null) 
                        {
                            if (t.IsWhitespaceAfter || t.Next.IsCharOf(";,")) 
                                val = (t as Pullenti.Ner.NumberToken).IntValue.Value;
                        }
                    }
                    else 
                    {
                        Pullenti.Ner.NumberToken nt = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t);
                        if (nt != null && nt.IntValue != null) 
                        {
                            val = nt.IntValue.Value;
                            t = nt.EndToken;
                        }
                    }
                    if (val > 0 && (val < 8)) 
                    {
                        dep.Number = val.ToString();
                        t1 = t;
                        coef += 4;
                    }
                }
                if (dep.Number == null) 
                {
                    t = typ.BeginToken.Previous;
                    if (t != null && !t.IsNewlineAfter) 
                    {
                        int val = 0;
                        if (t is Pullenti.Ner.NumberToken) 
                        {
                            if (!t.Morph.Class.IsNoun && (t as Pullenti.Ner.NumberToken).IntValue != null) 
                            {
                                if (t.IsWhitespaceBefore || t.Previous.IsCharOf(",")) 
                                    val = (t as Pullenti.Ner.NumberToken).IntValue.Value;
                            }
                        }
                        else 
                        {
                            Pullenti.Ner.NumberToken nt = Pullenti.Ner.Core.NumberHelper.TryParseRomanBack(t);
                            if (nt != null && nt.IntValue != null) 
                            {
                                val = nt.IntValue.Value;
                                t = nt.BeginToken;
                            }
                        }
                        if (val > 0 && (val < 8)) 
                        {
                            dep.Number = val.ToString();
                            t0 = t;
                            coef += 4;
                        }
                    }
                }
            }
            else if (typ.Root != null && typ.Root.CanBeNormalDep && afterOrg != null) 
            {
                coef += 3;
                if (!afterOrgTemp) 
                    dep.Higher = afterOrg as OrganizationReferent;
                else 
                    dep.m_TempParentOrg = afterOrg as OrganizationReferent;
                if (afterOrgTok != null) 
                    t1 = afterOrgTok;
            }
            else if (typ.Typ == "генеральный штаб" || typ.Typ == "генеральний штаб") 
                coef += 3;
            if (beforeOrg != null) 
                coef += 1;
            if (afterOrg != null) 
            {
                coef += 2;
                if (((typ.Name != null || ((typ.Root != null && typ.Root.CanBeNormalDep)))) && Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(afterOrg as OrganizationReferent, dep, false)) 
                {
                    coef += 1;
                    if (!typ.Chars.IsAllLower) 
                        coef += 0.5F;
                }
            }
            if (typ.Typ == "курс" || typ.Typ == "группа" || typ.Typ == "група") 
            {
                if (dep.Number == null) 
                    coef = 0;
                else if (typ.Typ == "курс") 
                {
                    int n;
                    if (int.TryParse(dep.Number, out n)) 
                    {
                        if (n > 0 && (n < 9)) 
                            coef += 2;
                    }
                }
            }
            if (t1.Next != null && t1.Next.IsChar('(')) 
            {
                Pullenti.Ner.Token ttt = t1.Next.Next;
                if ((ttt != null && ttt.Next != null && ttt.Next.IsChar(')')) && (ttt is Pullenti.Ner.TextToken)) 
                {
                    if (dep.NameVars.ContainsKey((ttt as Pullenti.Ner.TextToken).Term)) 
                    {
                        coef += 2;
                        dep.AddName((ttt as Pullenti.Ner.TextToken).Term, true, ttt);
                        t1 = ttt.Next;
                    }
                }
            }
            Pullenti.Ner.Org.Internal.OrgItemEponymToken ep = Pullenti.Ner.Org.Internal.OrgItemEponymToken.TryAttach(t1.Next, false);
            if (ep != null) 
            {
                coef += 2;
                foreach (string e in ep.Eponyms) 
                {
                    dep.AddEponym(e);
                }
                t1 = ep.EndToken;
            }
            if (brName != null) 
            {
                string str1 = Pullenti.Ner.Core.MiscHelper.GetTextValue(brName.BeginToken.Next, brName.EndToken.Previous, Pullenti.Ner.Core.GetTextAttr.No);
                if (str1 != null) 
                    dep.AddName(str1, true, null);
            }
            if (dep.Slots.Count == 0) 
                return null;
            Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(dep, t0, t1);
            this.CorrectDepAttrs(res, typ, afterOrgTemp);
            if (dep.Number != null) 
                coef += 2;
            if (isPureDep) 
                coef += 2;
            if (specWordBefore) 
            {
                if (dep.FindSlot(OrganizationReferent.ATTR_NAME, null, true) != null) 
                    coef += 2;
            }
            if (coef > 3 || attachTyp == AttachType.ExtOntology) 
                return res;
            else 
                return null;
        }
        void CorrectDepAttrs(Pullenti.Ner.ReferentToken res, Pullenti.Ner.Org.Internal.OrgItemTypeToken typ, bool afterTempOrg = false)
        {
            Pullenti.Ner.Token t0 = res.BeginToken;
            OrganizationReferent dep = res.Referent as OrganizationReferent;
            if ((((((((typ != null && typ.Root != null && typ.Root.CanHasNumber)) || dep.Types.Contains("офис") || dep.Types.Contains("офіс")) || dep.Types.Contains("отдел") || dep.Types.Contains("отделение")) || dep.Types.Contains("инспекция") || dep.Types.Contains("лаборатория")) || dep.Types.Contains("управление") || dep.Types.Contains("управління")) || dep.Types.Contains("відділ") || dep.Types.Contains("відділення")) || dep.Types.Contains("інспекція") || dep.Types.Contains("лабораторія")) 
            {
                if (((t0.Previous is Pullenti.Ner.NumberToken) && (t0.WhitespacesBeforeCount < 3) && !t0.Previous.Morph.Class.IsNoun) && t0.Previous.IsWhitespaceBefore) 
                {
                    string nn = (t0.Previous as Pullenti.Ner.NumberToken).Value.ToString();
                    if (dep.Number == null || dep.Number == nn) 
                    {
                        dep.Number = nn;
                        t0 = t0.Previous;
                        res.BeginToken = t0;
                    }
                }
                if (Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(res.EndToken.Next) != null && (res.EndToken.WhitespacesAfterCount < 3) && dep.Number == null) 
                {
                    Pullenti.Ner.Org.Internal.OrgItemNumberToken num = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(res.EndToken.Next, false, typ);
                    if (num != null) 
                    {
                        dep.Number = num.Number;
                        res.EndToken = num.EndToken;
                    }
                }
            }
            if (dep.Types.Contains("управление") || dep.Types.Contains("департамент") || dep.Types.Contains("управління")) 
            {
                foreach (Pullenti.Ner.Slot s in dep.Slots) 
                {
                    if (s.TypeName == OrganizationReferent.ATTR_GEO && (s.Value is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                        Pullenti.Ner.Geo.GeoReferent g = s.Value as Pullenti.Ner.Geo.GeoReferent;
                        if (g.IsState && g.Alpha2 == "RU") 
                        {
                            dep.Slots.Remove(s);
                            break;
                        }
                    }
                }
            }
            Pullenti.Ner.Token t1 = res.EndToken;
            if (t1.Next == null || afterTempOrg) 
                return;
            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
            if (br != null && (t1.WhitespacesAfterCount < 2) && br.IsQuoteType) 
            {
                object g = this.IsGeo(br.BeginToken.Next, false);
                if (g is Pullenti.Ner.ReferentToken) 
                {
                    if ((g as Pullenti.Ner.ReferentToken).EndToken.Next == br.EndToken) 
                    {
                        dep.AddGeoObject(g);
                        t1 = (res.EndToken = br.EndToken);
                    }
                }
                else if ((g is Pullenti.Ner.Referent) && br.BeginToken.Next.Next == br.EndToken) 
                {
                    dep.AddGeoObject(g);
                    t1 = (res.EndToken = br.EndToken);
                }
                else if (br.BeginToken.Next.IsValue("О", null) || br.BeginToken.Next.IsValue("ОБ", null)) 
                {
                }
                else 
                {
                    string nam = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                    if (nam != null) 
                    {
                        dep.AddName(nam, true, br.BeginToken.Next);
                        t1 = (res.EndToken = br.EndToken);
                    }
                }
            }
            bool prep = false;
            if (t1.Next != null) 
            {
                if (t1.Next.Morph.Class.IsPreposition) 
                {
                    if (t1.Next.IsValue("В", null) || t1.Next.IsValue("ПО", null)) 
                    {
                        t1 = t1.Next;
                        prep = true;
                    }
                }
                if (t1.Next != null && (t1.Next.WhitespacesBeforeCount < 3)) 
                {
                    if (t1.Next.IsValue("НА", null) && t1.Next.Next != null && t1.Next.Next.IsValue("ТРАНСПОРТ", null)) 
                        res.EndToken = (t1 = t1.Next.Next);
                }
            }
            for (int k = 0; k < 2; k++) 
            {
                if (t1.Next == null) 
                    return;
                Pullenti.Ner.Geo.GeoReferent geo = t1.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                bool ge = false;
                if (geo != null) 
                {
                    if (!dep.AddGeoObject(geo)) 
                        return;
                    res.EndToken = t1.Next;
                    ge = true;
                }
                else 
                {
                    Pullenti.Ner.ReferentToken rgeo = t1.Kit.ProcessReferent("GEO", t1.Next);
                    if (rgeo != null) 
                    {
                        if (!rgeo.Morph.Class.IsAdjective) 
                        {
                            if (!dep.AddGeoObject(rgeo)) 
                                return;
                            res.EndToken = rgeo.EndToken;
                            ge = true;
                        }
                    }
                }
                if (!ge) 
                    return;
                t1 = res.EndToken;
                if (t1.Next == null) 
                    return;
                bool isAnd = false;
                if (t1.Next.IsAnd) 
                    t1 = t1.Next;
                if (t1 == null) 
                    return;
            }
        }
        Pullenti.Ner.ReferentToken AttachGlobalOrg(Pullenti.Ner.Token t, AttachType attachTyp, Pullenti.Ner.Core.AnalyzerData ad, object extGeo = null)
        {
            if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLatinLetter) 
            {
                if (Pullenti.Ner.Core.MiscHelper.IsEngArticle(t)) 
                {
                    Pullenti.Ner.ReferentToken res11 = this.AttachGlobalOrg(t.Next, attachTyp, ad, extGeo);
                    if (res11 != null) 
                    {
                        res11.BeginToken = t;
                        return res11;
                    }
                }
            }
            Pullenti.Ner.ReferentToken rt00 = this.TryAttachPoliticParty(t, ad as OrgAnalyzerData, true);
            if (rt00 != null) 
                return rt00;
            if (!(t is Pullenti.Ner.TextToken)) 
            {
                if (t != null && t.GetReferent() != null && t.GetReferent().TypeName == "URI") 
                {
                    Pullenti.Ner.ReferentToken rt = this.AttachGlobalOrg((t as Pullenti.Ner.ReferentToken).BeginToken, attachTyp, ad, null);
                    if (rt != null && rt.EndChar == t.EndChar) 
                    {
                        rt.BeginToken = (rt.EndToken = t);
                        return rt;
                    }
                }
                return null;
            }
            string term = (t as Pullenti.Ner.TextToken).Term;
            if (t.Chars.IsAllUpper && term == "ВС") 
            {
                if (t.Previous != null) 
                {
                    if (t.Previous.IsValue("ПРЕЗИДИУМ", null) || t.Previous.IsValue("ПЛЕНУМ", null) || t.Previous.IsValue("СЕССИЯ", null)) 
                    {
                        OrganizationReferent org00 = new OrganizationReferent();
                        org00.AddName("ВЕРХОВНЫЙ СОВЕТ", true, null);
                        org00.AddName("ВС", true, null);
                        org00.AddTypeStr("совет");
                        org00.AddProfile(OrgProfile.State);
                        Pullenti.Ner.Token te = this.AttachTailAttributes(org00, t.Next, null, false, AttachType.Normal, true);
                        return new Pullenti.Ner.ReferentToken(org00, t, te ?? t);
                    }
                }
                if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    bool isVc = false;
                    if (t.Previous != null && (t.Previous.GetReferent() is OrganizationReferent) && (t.Previous.GetReferent() as OrganizationReferent).Kind == OrganizationKind.Military) 
                        isVc = true;
                    else if (ad != null) 
                    {
                        foreach (Pullenti.Ner.Referent r in ad.Referents) 
                        {
                            if (r.FindSlot(OrganizationReferent.ATTR_NAME, "ВООРУЖЕННЫЕ СИЛЫ", true) != null) 
                            {
                                isVc = true;
                                break;
                            }
                        }
                    }
                    if (isVc) 
                    {
                        OrganizationReferent org00 = new OrganizationReferent();
                        org00.AddName("ВООРУЖЕННЫЕ СИЛЫ", true, null);
                        org00.AddName("ВС", true, null);
                        org00.AddTypeStr("армия");
                        org00.AddProfile(OrgProfile.Army);
                        Pullenti.Ner.Token te = this.AttachTailAttributes(org00, t.Next, null, false, AttachType.Normal, true);
                        return new Pullenti.Ner.ReferentToken(org00, t, te ?? t);
                    }
                }
            }
            if ((t.Chars.IsAllUpper && ((term == "АН" || term == "ВАС")) && t.Next != null) && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
            {
                OrganizationReferent org00 = new OrganizationReferent();
                if (term == "АН") 
                {
                    org00.AddName("АКАДЕМИЯ НАУК", true, null);
                    org00.AddTypeStr("академия");
                    org00.AddProfile(OrgProfile.Science);
                }
                else 
                {
                    org00.AddName("ВЫСШИЙ АРБИТРАЖНЫЙ СУД", true, null);
                    org00.AddName("ВАС", true, null);
                    org00.AddTypeStr("суд");
                    org00.AddProfile(OrgProfile.Justice);
                }
                Pullenti.Ner.Token te = this.AttachTailAttributes(org00, t.Next, null, false, AttachType.Normal, true);
                return new Pullenti.Ner.ReferentToken(org00, t, te ?? t);
            }
            if (t.Chars.IsAllUpper && term == "ГД" && t.Previous != null) 
            {
                Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("PERSONPROPERTY", t.Previous);
                if (rt != null && rt.Referent != null && rt.Referent.TypeName == "PERSONPROPERTY") 
                {
                    OrganizationReferent org00 = new OrganizationReferent();
                    org00.AddName("ГОСУДАРСТВЕННАЯ ДУМА", true, null);
                    org00.AddName("ГОСДУМА", true, null);
                    org00.AddName("ГД", true, null);
                    org00.AddTypeStr("парламент");
                    org00.AddProfile(OrgProfile.State);
                    Pullenti.Ner.Token te = this.AttachTailAttributes(org00, t.Next, null, false, AttachType.Normal, true);
                    return new Pullenti.Ner.ReferentToken(org00, t, te ?? t);
                }
            }
            if (t.Chars.IsAllUpper && term == "МЮ") 
            {
                bool ok = false;
                if ((t.Previous != null && t.Previous.IsValue("В", null) && t.Previous.Previous != null) && t.Previous.Previous.IsValue("ЗАРЕГИСТРИРОВАТЬ", null)) 
                    ok = true;
                else if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    ok = true;
                if (ok) 
                {
                    OrganizationReferent org00 = new OrganizationReferent();
                    org00.AddTypeStr("министерство");
                    org00.AddProfile(OrgProfile.State);
                    org00.AddName("МИНИСТЕРСТВО ЮСТИЦИИ", true, null);
                    org00.AddName("МИНЮСТ", true, null);
                    Pullenti.Ner.Token t1 = t;
                    if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                        t1 = t.Next;
                        org00.AddGeoObject(t1.GetReferent());
                    }
                    return new Pullenti.Ner.ReferentToken(org00, t, t1);
                }
            }
            if (t.Chars.IsAllUpper && term == "ФС") 
            {
                if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    OrganizationReferent org00 = new OrganizationReferent();
                    org00.AddTypeStr("парламент");
                    org00.AddProfile(OrgProfile.State);
                    org00.AddName("ФЕДЕРАЛЬНОЕ СОБРАНИЕ", true, null);
                    org00.AddGeoObject(t.Next.GetReferent());
                    return new Pullenti.Ner.ReferentToken(org00, t, t.Next);
                }
            }
            if (t.Chars.IsAllUpper && term == "МП") 
            {
                Pullenti.Ner.Token tt0 = t.Previous;
                if (tt0 != null && tt0.IsChar('(')) 
                    tt0 = tt0.Previous;
                OrganizationReferent org0 = null;
                bool prev = false;
                if (tt0 != null) 
                {
                    org0 = tt0.GetReferent() as OrganizationReferent;
                    if (org0 != null) 
                        prev = true;
                }
                if (t.Next != null && org0 == null) 
                    org0 = t.Next.GetReferent() as OrganizationReferent;
                if (org0 != null && org0.Kind == OrganizationKind.Church) 
                {
                    OrganizationReferent glob = new OrganizationReferent();
                    glob.AddTypeStr("патриархия");
                    glob.AddName("МОСКОВСКАЯ ПАТРИАРХИЯ", true, null);
                    glob.Higher = org0;
                    glob.AddProfile(OrgProfile.Religion);
                    Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(glob, t, t);
                    if (!prev) 
                        res.EndToken = t.Next;
                    else 
                    {
                        res.BeginToken = tt0;
                        if (tt0 != t.Previous && res.EndToken.Next != null && res.EndToken.Next.IsChar(')')) 
                            res.EndToken = res.EndToken.Next;
                    }
                    return res;
                }
            }
            if (t.Chars.IsAllUpper && term == "ГШ") 
            {
                if (t.Next != null && (t.Next.GetReferent() is OrganizationReferent) && (t.Next.GetReferent() as OrganizationReferent).Kind == OrganizationKind.Military) 
                {
                    OrganizationReferent org00 = new OrganizationReferent();
                    org00.AddTypeStr("генеральный штаб");
                    org00.AddProfile(OrgProfile.Army);
                    org00.Higher = t.Next.GetReferent() as OrganizationReferent;
                    return new Pullenti.Ner.ReferentToken(org00, t, t.Next);
                }
            }
            if (t.Chars.IsAllUpper && term == "ЗС") 
            {
                if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    OrganizationReferent org00 = new OrganizationReferent();
                    org00.AddTypeStr("парламент");
                    org00.AddProfile(OrgProfile.State);
                    org00.AddName("ЗАКОНОДАТЕЛЬНОЕ СОБРАНИЕ", true, null);
                    org00.AddGeoObject(t.Next.GetReferent());
                    return new Pullenti.Ner.ReferentToken(org00, t, t.Next);
                }
            }
            if (t.Chars.IsAllUpper && term == "СФ") 
            {
                t.InnerBool = true;
                if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    OrganizationReferent org00 = new OrganizationReferent();
                    org00.AddTypeStr("совет");
                    org00.AddProfile(OrgProfile.State);
                    org00.AddName("СОВЕТ ФЕДЕРАЦИИ", true, null);
                    org00.AddGeoObject(t.Next.GetReferent());
                    return new Pullenti.Ner.ReferentToken(org00, t, t.Next);
                }
                if (t.Next != null) 
                {
                    if (t.Next.IsValue("ФС", null) || (((t.Next.GetReferent() is OrganizationReferent) && t.Next.GetReferent().FindSlot(OrganizationReferent.ATTR_NAME, "ФЕДЕРАЛЬНОЕ СОБРАНИЕ", true) != null))) 
                    {
                        OrganizationReferent org00 = new OrganizationReferent();
                        org00.AddTypeStr("совет");
                        org00.AddProfile(OrgProfile.State);
                        org00.AddName("СОВЕТ ФЕДЕРАЦИИ", true, null);
                        return new Pullenti.Ner.ReferentToken(org00, t, t);
                    }
                }
            }
            if (t.Chars.IsAllUpper && term == "ФК") 
            {
                if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    OrganizationReferent org00 = new OrganizationReferent();
                    org00.AddTypeStr("казначейство");
                    org00.AddProfile(OrgProfile.Finance);
                    org00.AddName("ФЕДЕРАЛЬНОЕ КАЗНАЧЕЙСТВО", true, null);
                    org00.AddGeoObject(t.Next.GetReferent());
                    return new Pullenti.Ner.ReferentToken(org00, t, t.Next);
                }
                if (attachTyp == AttachType.NormalAfterDep) 
                {
                    OrganizationReferent org00 = new OrganizationReferent();
                    org00.AddTypeStr("казначейство");
                    org00.AddProfile(OrgProfile.Finance);
                    org00.AddName("ФЕДЕРАЛЬНОЕ КАЗНАЧЕЙСТВО", true, null);
                    return new Pullenti.Ner.ReferentToken(org00, t, t);
                }
            }
            if (t.Chars.IsAllUpper && ((term == "СК" || term == "CK"))) 
            {
                if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
                    {
                        if (tt is Pullenti.Ner.TextToken) 
                        {
                            if (tt.IsCommaAnd) 
                                continue;
                            if (tt is Pullenti.Ner.NumberToken) 
                                continue;
                            if (!tt.Chars.IsLetter) 
                                continue;
                            if ((tt.IsValue("ЧАСТЬ", null) || tt.IsValue("СТАТЬЯ", null) || tt.IsValue("ПУНКТ", null)) || tt.IsValue("СТ", null) || tt.IsValue("П", null)) 
                                return null;
                            break;
                        }
                    }
                    OrganizationReferent org00 = new OrganizationReferent();
                    org00.AddTypeStr("комитет");
                    org00.AddProfile(OrgProfile.Unit);
                    org00.AddName("СЛЕДСТВЕННЫЙ КОМИТЕТ", true, null);
                    org00.AddGeoObject(t.Next.GetReferent());
                    return new Pullenti.Ner.ReferentToken(org00, t, t.Next);
                }
                List<Pullenti.Ner.Core.IntOntologyToken> gt1 = Pullenti.Ner.Org.Internal.OrgGlobal.GlobalOrgs.TryAttach(t.Next, null, false);
                if (gt1 == null && t.Next != null && t.Kit.BaseLanguage.IsUa) 
                    gt1 = Pullenti.Ner.Org.Internal.OrgGlobal.GlobalOrgsUa.TryAttach(t.Next, null, false);
                bool ok = false;
                if (gt1 != null && gt1[0].Item.Referent.FindSlot(OrganizationReferent.ATTR_NAME, "МВД", true) != null) 
                    ok = true;
                if (ok) 
                {
                    OrganizationReferent org00 = new OrganizationReferent();
                    org00.AddTypeStr("комитет");
                    org00.AddName("СЛЕДСТВЕННЫЙ КОМИТЕТ", true, null);
                    org00.AddProfile(OrgProfile.Unit);
                    return new Pullenti.Ner.ReferentToken(org00, t, t);
                }
            }
            List<Pullenti.Ner.Core.IntOntologyToken> gt = Pullenti.Ner.Org.Internal.OrgGlobal.GlobalOrgs.TryAttach(t, null, true);
            if (gt == null) 
                gt = Pullenti.Ner.Org.Internal.OrgGlobal.GlobalOrgs.TryAttach(t, null, false);
            if (gt == null && t != null && t.Kit.BaseLanguage.IsUa) 
            {
                gt = Pullenti.Ner.Org.Internal.OrgGlobal.GlobalOrgsUa.TryAttach(t, null, true);
                if (gt == null) 
                    gt = Pullenti.Ner.Org.Internal.OrgGlobal.GlobalOrgsUa.TryAttach(t, null, false);
            }
            if (gt == null) 
                return null;
            foreach (Pullenti.Ner.Core.IntOntologyToken ot in gt) 
            {
                OrganizationReferent org0 = ot.Item.Referent as OrganizationReferent;
                if (org0 == null) 
                    continue;
                if (ot.BeginToken == ot.EndToken) 
                {
                    if (gt.Count == 1) 
                    {
                        if ((ot.BeginToken is Pullenti.Ner.TextToken) && (ot.BeginToken as Pullenti.Ner.TextToken).Term == "МГТУ") 
                        {
                            Pullenti.Ner.Org.Internal.OrgItemTypeToken ty = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(ot.BeginToken, false, null);
                            if (ty != null) 
                                continue;
                        }
                    }
                    else 
                    {
                        if (ad == null) 
                            return null;
                        bool ok = false;
                        foreach (Pullenti.Ner.Referent o in ad.Referents) 
                        {
                            if (o.CanBeEquals(org0, Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts)) 
                            {
                                ok = true;
                                break;
                            }
                        }
                        if (!ok) 
                            return null;
                    }
                }
                if (((t.Chars.IsAllLower && attachTyp != AttachType.ExtOntology && extGeo == null) && !t.IsValue("МИД", null) && !org0._typesContains("факультет")) && org0.Kind != OrganizationKind.Justice) 
                {
                    if (ot.BeginToken == ot.EndToken) 
                        continue;
                    if (ot.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                        continue;
                    Pullenti.Ner.Org.Internal.OrgItemTypeToken tyty = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t, true, null);
                    if (tyty != null && tyty.EndToken == ot.EndToken) 
                        continue;
                    if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                    }
                    else if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.CheckOrgSpecialWordBefore(t.Previous)) 
                    {
                    }
                    else 
                        continue;
                }
                if ((ot.BeginToken == ot.EndToken && (t.LengthChar < 6) && !t.Chars.IsAllUpper) && !t.Chars.IsLastLower) 
                {
                    if (org0.FindSlot(OrganizationReferent.ATTR_NAME, (t as Pullenti.Ner.TextToken).Term, true) == null) 
                    {
                        if (t.IsValue("МИД", null)) 
                        {
                        }
                        else 
                            continue;
                    }
                    else if (t.Chars.IsAllLower) 
                        continue;
                    else if (t.LengthChar < 3) 
                        continue;
                    else if (t.LengthChar == 4) 
                    {
                        bool hasVow = false;
                        foreach (char ch in (t as Pullenti.Ner.TextToken).Term) 
                        {
                            if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(ch) || Pullenti.Morph.LanguageHelper.IsLatinVowel(ch)) 
                                hasVow = true;
                        }
                        if (hasVow) 
                            continue;
                    }
                }
                if (ot.BeginToken == ot.EndToken && term == "МЭР") 
                    continue;
                if (ot.BeginToken == ot.EndToken) 
                {
                    if (t.Previous == null || t.IsWhitespaceBefore) 
                    {
                    }
                    else if ((t.Previous is Pullenti.Ner.TextToken) && ((t.Previous.IsCharOf(",:") || Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Previous, false, false)))) 
                    {
                    }
                    else if (t.GetMorphClassInDictionary().IsUndefined && t.Chars.IsCapitalUpper) 
                    {
                    }
                    else 
                        continue;
                    if (t.Next == null || t.IsWhitespaceAfter) 
                    {
                    }
                    else if ((t.Next is Pullenti.Ner.TextToken) && ((t.Next.IsCharOf(",.") || Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t.Next, false, null, false)))) 
                    {
                    }
                    else if (t.GetMorphClassInDictionary().IsUndefined && t.Chars.IsCapitalUpper) 
                    {
                    }
                    else 
                        continue;
                    if (t is Pullenti.Ner.TextToken) 
                    {
                        bool hasName = false;
                        foreach (string n in org0.Names) 
                        {
                            if (t.IsValue(n, null)) 
                            {
                                hasName = true;
                                break;
                            }
                        }
                        if (!hasName) 
                            continue;
                        if (t.LengthChar < 3) 
                        {
                            bool ok1 = true;
                            if (t.Next != null && !t.IsNewlineBefore) 
                            {
                                if (Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t.Next) != null) 
                                    ok1 = false;
                                else if (t.Next.IsHiphen || (t.Next is Pullenti.Ner.NumberToken)) 
                                    ok1 = false;
                            }
                            if (!ok1) 
                                continue;
                        }
                    }
                    Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("TRANSPORT", t);
                    if (rt != null) 
                        continue;
                }
                OrganizationReferent org = null;
                if (t is Pullenti.Ner.TextToken) 
                {
                    if ((t.IsValue("ДЕПАРТАМЕНТ", null) || t.IsValue("КОМИТЕТ", "КОМІТЕТ") || t.IsValue("МИНИСТЕРСТВО", "МІНІСТЕРСТВО")) || t.IsValue("КОМИССИЯ", "КОМІСІЯ")) 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemNameToken nnn = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(t.Next, null, true, true);
                        if (nnn != null && nnn.EndChar > ot.EndChar) 
                        {
                            org = new OrganizationReferent();
                            foreach (OrgProfile p in org0.Profiles) 
                            {
                                org.AddProfile(p);
                            }
                            org.AddTypeStr((t as Pullenti.Ner.TextToken).Lemma.ToLower());
                            org.AddName(Pullenti.Ner.Core.MiscHelper.GetTextValue(t, nnn.EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle), true, null);
                            ot.EndToken = nnn.EndToken;
                        }
                    }
                }
                if (org == null) 
                {
                    org = org0.Clone() as OrganizationReferent;
                    if (org.GeoObjects.Count > 0) 
                    {
                        foreach (Pullenti.Ner.Slot s in org.Slots) 
                        {
                            if (s.TypeName == OrganizationReferent.ATTR_GEO && (s.Value is Pullenti.Ner.Geo.GeoReferent)) 
                            {
                                Pullenti.Ner.Referent gg = (s.Value as Pullenti.Ner.Geo.GeoReferent).Clone();
                                gg.Occurrence.Clear();
                                Pullenti.Ner.ReferentToken rtg = new Pullenti.Ner.ReferentToken(gg, t, t);
                                rtg.Data = t.Kit.GetAnalyzerDataByAnalyzerName("GEO");
                                org.Slots.Remove(s);
                                org.AddGeoObject(rtg);
                                break;
                            }
                        }
                    }
                    org.AddName(ot.Termin.CanonicText, true, null);
                }
                if (extGeo != null) 
                    org.AddGeoObject(extGeo);
                org.IsFromGlobalOntos = true;
                for (Pullenti.Ner.Token tt = ot.BeginToken; tt != null && (tt.EndChar < ot.EndChar); tt = tt.Next) 
                {
                    if (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                    {
                        org.AddGeoObject(tt);
                        break;
                    }
                }
                if ((t.Previous is Pullenti.Ner.TextToken) && (t.WhitespacesBeforeCount < 2) && t.Previous.Morph.Class.IsAdjective) 
                {
                    Pullenti.Ner.ReferentToken gg = t.Kit.ProcessReferent("GEO", t.Previous);
                    if (gg != null && gg.Morph.Class.IsAdjective) 
                    {
                        t = t.Previous;
                        org.AddGeoObject(gg);
                    }
                }
                Pullenti.Ner.Token t1 = null;
                if (!org0.Types.Contains("академия") && attachTyp != AttachType.NormalAfterDep && attachTyp != AttachType.ExtOntology) 
                    t1 = this.AttachTailAttributes(org, ot.EndToken.Next, null, false, AttachType.Normal, true);
                else if ((((((org0.Types.Contains("министерство") || org0.Types.Contains("парламент") || org0.Types.Contains("совет")) || org0.Kind == OrganizationKind.Science || org0.Kind == OrganizationKind.Govenment) || org0.Kind == OrganizationKind.Study || org0.Kind == OrganizationKind.Justice) || org0.Kind == OrganizationKind.Military)) && (ot.EndToken.Next is Pullenti.Ner.ReferentToken)) 
                {
                    Pullenti.Ner.Geo.GeoReferent geo = ot.EndToken.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                    if (geo != null && geo.IsState) 
                    {
                        org.AddGeoObject(geo);
                        t1 = ot.EndToken.Next;
                    }
                }
                if (t1 == null) 
                    t1 = ot.EndToken;
                Pullenti.Ner.Org.Internal.OrgItemEponymToken epp = Pullenti.Ner.Org.Internal.OrgItemEponymToken.TryAttach(t1.Next, false);
                if (epp != null) 
                {
                    bool exi = false;
                    foreach (string v in epp.Eponyms) 
                    {
                        if (org.FindSlot(OrganizationReferent.ATTR_EPONYM, v, true) != null) 
                        {
                            exi = true;
                            break;
                        }
                    }
                    if (!exi) 
                    {
                        for (int i = org.Slots.Count - 1; i >= 0; i--) 
                        {
                            if (org.Slots[i].TypeName == OrganizationReferent.ATTR_EPONYM) 
                                org.Slots.RemoveAt(i);
                        }
                        foreach (string vv in epp.Eponyms) 
                        {
                            org.AddEponym(vv);
                        }
                    }
                    t1 = epp.EndToken;
                }
                if (t1.WhitespacesAfterCount < 2) 
                {
                    Pullenti.Ner.Org.Internal.OrgItemTypeToken typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t1.Next, false, null);
                    if (typ != null) 
                    {
                        if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypeAccords(org, typ)) 
                        {
                            if (typ.Chars.IsLatinLetter && typ.Root != null && typ.Root.CanBeNormalDep) 
                            {
                            }
                            else 
                            {
                                org.AddType(typ, false);
                                t1 = typ.EndToken;
                            }
                        }
                    }
                }
                if (org.GeoObjects.Count == 0 && t.Previous != null && t.Previous.Morph.Class.IsAdjective) 
                {
                    Pullenti.Ner.ReferentToken grt = t.Kit.ProcessReferent("GEO", t.Previous);
                    if (grt != null && grt.EndToken.Next == t) 
                    {
                        org.AddGeoObject(grt);
                        t = t.Previous;
                    }
                }
                if (org.FindSlot(OrganizationReferent.ATTR_NAME, "ВТБ", true) != null && t1.Next != null) 
                {
                    Pullenti.Ner.Token tt = t1.Next;
                    if (tt.IsHiphen && tt.Next != null) 
                        tt = tt.Next;
                    if (tt is Pullenti.Ner.NumberToken) 
                    {
                        org.Number = (tt as Pullenti.Ner.NumberToken).Value.ToString();
                        t1 = tt;
                    }
                }
                if (!t.IsWhitespaceBefore && !t1.IsWhitespaceAfter) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Previous, true, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next, true, null, false)) 
                    {
                        t = t.Previous;
                        t1 = t1.Next;
                    }
                }
                return new Pullenti.Ner.ReferentToken(org, t, t1);
            }
            return null;
        }
        static Pullenti.Ner.MetaToken _tryAttachOrgMedTyp(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            string s = (t as Pullenti.Ner.TextToken).Term;
            if (((t != null && s == "Г" && t.Next != null) && t.Next.IsCharOf("\\/.") && t.Next.Next != null) && t.Next.Next.IsValue("Б", null)) 
            {
                Pullenti.Ner.Token t1 = t.Next.Next;
                if (t.Next.IsChar('.') && t1.Next != null && t1.Next.IsChar('.')) 
                    t1 = t1.Next;
                return new Pullenti.Ner.MetaToken(t, t1) { Tag = "городская больница", Morph = new Pullenti.Ner.MorphCollection() { Gender = Pullenti.Morph.MorphGender.Feminie } };
            }
            if ((s == "ИН" && t.Next != null && t.Next.IsHiphen) && t.Next.Next != null && t.Next.Next.IsValue("Т", null)) 
                return new Pullenti.Ner.MetaToken(t, t.Next.Next) { Tag = "институт", Morph = new Pullenti.Ner.MorphCollection() { Gender = Pullenti.Morph.MorphGender.Masculine } };
            if ((s == "Б" && t.Next != null && t.Next.IsHiphen) && (t.Next.Next is Pullenti.Ner.TextToken) && ((t.Next.Next.IsValue("ЦА", null) || t.Next.Next.IsValue("ЦУ", null)))) 
                return new Pullenti.Ner.MetaToken(t, t.Next.Next) { Tag = "больница", Morph = new Pullenti.Ner.MorphCollection() { Gender = Pullenti.Morph.MorphGender.Feminie } };
            if (s == "ГКБ") 
                return new Pullenti.Ner.MetaToken(t, t) { Tag = "городская клиническая больница", Morph = new Pullenti.Ner.MorphCollection() { Gender = Pullenti.Morph.MorphGender.Feminie } };
            if (t.IsValue("ПОЛИКЛИНИКА", null)) 
                return new Pullenti.Ner.MetaToken(t, t) { Tag = "поликлиника", Morph = new Pullenti.Ner.MorphCollection() { Gender = Pullenti.Morph.MorphGender.Feminie } };
            if (t.IsValue("БОЛЬНИЦА", null)) 
                return new Pullenti.Ner.MetaToken(t, t) { Tag = "больница", Morph = new Pullenti.Ner.MorphCollection() { Gender = Pullenti.Morph.MorphGender.Feminie } };
            if (t.IsValue("ДЕТСКИЙ", null)) 
            {
                Pullenti.Ner.MetaToken mt = _tryAttachOrgMedTyp(t.Next);
                if (mt != null) 
                {
                    mt.BeginToken = t;
                    mt.Tag = string.Format("{0} {1}", (mt.Morph.Gender == Pullenti.Morph.MorphGender.Feminie ? "детская" : "детский"), mt.Tag);
                    return mt;
                }
            }
            return null;
        }
        Pullenti.Ner.ReferentToken TryAttachOrgMed(Pullenti.Ner.Token t, OrgAnalyzerData ad)
        {
            if (t == null) 
                return null;
            if (t.Previous == null || t.Previous.Previous == null) 
                return null;
            if ((t.Previous.Morph.Class.IsPreposition && t.Previous.Previous.IsValue("ДОСТАВИТЬ", null)) || t.Previous.Previous.IsValue("ПОСТУПИТЬ", null)) 
            {
            }
            else 
                return null;
            if (t.IsValue("ТРАВМПУНКТ", null)) 
                t = t.Next;
            else if (t.IsValue("ТРАВМ", null)) 
            {
                if ((t.Next != null && t.Next.IsChar('.') && t.Next.Next != null) && t.Next.Next.IsValue("ПУНКТ", null)) 
                    t = t.Next.Next.Next;
            }
            if (t is Pullenti.Ner.NumberToken) 
            {
                Pullenti.Ner.MetaToken tt = _tryAttachOrgMedTyp(t.Next);
                if (tt != null) 
                {
                    OrganizationReferent org1 = new OrganizationReferent();
                    org1.AddTypeStr((tt.Tag as string).ToLower());
                    org1.Number = (t as Pullenti.Ner.NumberToken).Value.ToString();
                    return new Pullenti.Ner.ReferentToken(org1, t, tt.EndToken);
                }
            }
            Pullenti.Ner.MetaToken typ = _tryAttachOrgMedTyp(t);
            string adj = null;
            if (typ == null && t.Chars.IsCapitalUpper && t.Morph.Class.IsAdjective) 
            {
                typ = _tryAttachOrgMedTyp(t.Next);
                if (typ != null) 
                    adj = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, typ.Morph.Gender, false);
            }
            if (typ == null) 
                return null;
            OrganizationReferent org = new OrganizationReferent();
            string s = typ.Tag as string;
            org.AddTypeStr(s.ToLower());
            if (adj != null) 
                org.AddName(string.Format("{0} {1}", adj, s.ToUpper()), true, null);
            Pullenti.Ner.Token t1 = typ.EndToken;
            Pullenti.Ner.Org.Internal.OrgItemEponymToken epo = Pullenti.Ner.Org.Internal.OrgItemEponymToken.TryAttach(t1.Next, false);
            if (epo != null) 
            {
                foreach (string v in epo.Eponyms) 
                {
                    org.AddEponym(v);
                }
                t1 = epo.EndToken;
            }
            if (t1.Next is Pullenti.Ner.TextToken) 
            {
                if (t1.Next.IsValue("СКЛИФОСОФСКОГО", null) || t1.Next.IsValue("СЕРБСКОГО", null) || t1.Next.IsValue("БОТКИНА", null)) 
                {
                    org.AddEponym((t1.Next as Pullenti.Ner.TextToken).Term);
                    t1 = t1.Next;
                }
            }
            Pullenti.Ner.Org.Internal.OrgItemNumberToken num = Pullenti.Ner.Org.Internal.OrgItemNumberToken.TryAttach(t1.Next, false, null);
            if (num != null) 
            {
                org.Number = num.Number;
                t1 = num.EndToken;
            }
            if (org.Slots.Count > 1) 
                return new Pullenti.Ner.ReferentToken(org, t, t1);
            return null;
        }
        Pullenti.Ner.ReferentToken TryAttachPropNames(Pullenti.Ner.Token t, OrgAnalyzerData ad)
        {
            Pullenti.Ner.ReferentToken rt = this._tryAttachOrgSportAssociations(t, ad);
            if (rt == null) 
                rt = this._tryAttachOrgNames(t, ad);
            if (rt == null) 
                return null;
            Pullenti.Ner.Token t0 = rt.BeginToken.Previous;
            if ((t0 is Pullenti.Ner.TextToken) && (t0.WhitespacesAfterCount < 2) && t0.Morph.Class.IsAdjective) 
            {
                Pullenti.Ner.ReferentToken rt0 = t0.Kit.ProcessReferent("GEO", t0);
                if (rt0 != null && rt0.Morph.Class.IsAdjective) 
                {
                    rt.BeginToken = rt0.BeginToken;
                    (rt.Referent as OrganizationReferent).AddGeoObject(rt0);
                }
            }
            if (rt.EndToken.WhitespacesAfterCount < 2) 
            {
                Pullenti.Ner.Token tt1 = this.AttachTailAttributes(rt.Referent as OrganizationReferent, rt.EndToken.Next, ad, true, AttachType.Normal, true);
                if (tt1 != null) 
                    rt.EndToken = tt1;
            }
            return rt;
        }
        Pullenti.Ner.ReferentToken _tryAttachOrgNames(Pullenti.Ner.Token t, OrgAnalyzerData ad)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Core.BracketSequenceToken br = null;
            Pullenti.Ner.Token tName1 = null;
            OrgProfile prof = OrgProfile.Undefined;
            OrgProfile prof2 = OrgProfile.Undefined;
            string typ = null;
            bool ok = false;
            Pullenti.Ner.ReferentToken uri = null;
            if (!(t is Pullenti.Ner.TextToken) || !t.Chars.IsLetter) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                {
                    if ((((br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 15)))) != null) 
                        t = t0.Next;
                    else 
                        return null;
                }
                else if (t.GetReferent() != null && t.GetReferent().TypeName == "URI") 
                {
                    Pullenti.Ner.Referent r = t.GetReferent();
                    string s = r.GetStringValue("SCHEME");
                    if (s == "HTTP") 
                    {
                        prof = OrgProfile.Media;
                        tName1 = t;
                    }
                }
                else if ((t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && t.Chars.IsLetter) 
                {
                    if ((t.Next != null && (t.Next.WhitespacesAfterCount < 3) && t.Next.Chars.IsLatinLetter) && ((t.Next.IsValue("POST", null) || t.Next.IsValue("TODAY", null)))) 
                    {
                        tName1 = t.Next;
                        if (_isStdPressEnd(tName1)) 
                            prof = OrgProfile.Media;
                    }
                    else 
                        return null;
                }
                else 
                    return null;
            }
            else if (t.Chars.IsAllUpper && (t as Pullenti.Ner.TextToken).Term == "ИА") 
            {
                prof = OrgProfile.Media;
                t = t.Next;
                typ = "информационное агенство";
                if (t == null || t.WhitespacesBeforeCount > 2) 
                    return null;
                Pullenti.Ner.ReferentToken re = this._tryAttachOrgNames(t, ad);
                if (re != null) 
                {
                    re.BeginToken = t0;
                    (re.Referent as OrganizationReferent).AddTypeStr(typ);
                    return re;
                }
                if (t.Chars.IsLatinLetter) 
                {
                    Pullenti.Ner.Org.Internal.OrgItemEngItem nam = Pullenti.Ner.Org.Internal.OrgItemEngItem.TryAttach(t, false);
                    if (nam != null) 
                    {
                        ok = true;
                        tName1 = nam.EndToken;
                    }
                    else 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemNameToken nam1 = Pullenti.Ner.Org.Internal.OrgItemNameToken.TryAttach(t, null, false, true);
                        if (nam1 != null) 
                        {
                            ok = true;
                            tName1 = nam1.EndToken;
                        }
                    }
                }
            }
            else if (((t.Chars.IsLatinLetter && t.Next != null && t.Next.IsChar('.')) && !t.Next.IsWhitespaceAfter && t.Next.Next != null) && t.Next.Next.Chars.IsLatinLetter) 
            {
                tName1 = t.Next.Next;
                prof = OrgProfile.Media;
                if (tName1.Next == null) 
                {
                }
                else if (tName1.WhitespacesAfterCount > 0) 
                {
                }
                else if (tName1.Next.IsChar(',')) 
                {
                }
                else if (tName1.LengthChar > 1 && tName1.Next.IsCharOf(".") && tName1.Next.IsWhitespaceAfter) 
                {
                }
                else if (br != null && br.EndToken.Previous == tName1) 
                {
                }
                else 
                    return null;
            }
            else if (t.Chars.IsAllLower && br == null) 
                return null;
            Pullenti.Ner.Token t00 = t0.Previous;
            if (t00 != null && t00.Morph.Class.IsAdjective) 
                t00 = t00.Previous;
            if (t00 != null && t00.Morph.Class.IsPreposition) 
                t00 = t00.Previous;
            Pullenti.Ner.Core.TerminToken tok = m_PropNames.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok == null && t.Chars.IsLatinLetter && t.IsValue("THE", null)) 
                tok = m_PropNames.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null && t.IsValue("ВЕДУЩИЙ", null) && tok.BeginToken == tok.EndToken) 
                tok = null;
            if (tok != null) 
                prof = (OrgProfile)tok.Termin.Tag;
            if (br != null) 
            {
                Pullenti.Ner.Token t1 = br.EndToken.Previous;
                for (Pullenti.Ner.Token tt = br.BeginToken; tt != null && tt.EndChar <= br.EndChar; tt = tt.Next) 
                {
                    Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                    if (mc == Pullenti.Morph.MorphClass.Verb) 
                        return null;
                    if (mc == Pullenti.Morph.MorphClass.Adverb) 
                        return null;
                    if (tt.IsCharOf("?:")) 
                        return null;
                    if (tt == br.BeginToken.Next || tt == br.EndToken.Previous) 
                    {
                        if (((tt.IsValue("ЖУРНАЛ", null) || tt.IsValue("ГАЗЕТА", null) || tt.IsValue("ПРАВДА", null)) || tt.IsValue("ИЗВЕСТИЯ", null) || tt.IsValue("НОВОСТИ", null)) || tt.IsValue("ВЕДОМОСТИ", null)) 
                        {
                            ok = true;
                            prof = OrgProfile.Media;
                            prof2 = OrgProfile.Press;
                        }
                    }
                }
                if (!ok && _isStdPressEnd(t1)) 
                {
                    if (br.BeginToken.Next.Chars.IsCapitalUpper && (br.LengthChar < 15)) 
                    {
                        ok = true;
                        prof = OrgProfile.Media;
                        prof2 = OrgProfile.Press;
                    }
                }
                else if (t1.IsValue("FM", null)) 
                {
                    ok = true;
                    prof = OrgProfile.Media;
                    typ = "радиостанция";
                }
                else if (((t1.IsValue("РУ", null) || t1.IsValue("RU", null) || t1.IsValue("NET", null))) && t1.Previous != null && t1.Previous.IsChar('.')) 
                    prof = OrgProfile.Media;
                Pullenti.Ner.Token b = br.BeginToken.Next;
                if (b.IsValue("THE", null)) 
                    b = b.Next;
                if (_isStdPressEnd(b) || b.IsValue("ВЕЧЕРНИЙ", null)) 
                {
                    ok = true;
                    prof = OrgProfile.Media;
                }
            }
            if ((tok == null && !ok && tName1 == null) && prof == OrgProfile.Undefined) 
            {
                if (br == null || !t.Chars.IsCapitalUpper) 
                    return null;
                Pullenti.Ner.Core.TerminToken tok1 = m_PropPref.TryParse(t00, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok1 != null) 
                {
                    OrgProfile pr = (OrgProfile)tok1.Termin.Tag;
                    if (prof != OrgProfile.Undefined && prof != pr) 
                        return null;
                }
                else 
                {
                    if (t.Chars.IsLetter && !t.Chars.IsCyrillicLetter) 
                    {
                        for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                                continue;
                            if (tt.WhitespacesBeforeCount > 2) 
                                break;
                            if (!tt.Chars.IsLetter || tt.Chars.IsCyrillicLetter) 
                                break;
                            if (_isStdPressEnd(tt)) 
                            {
                                tName1 = tt;
                                prof = OrgProfile.Media;
                                ok = true;
                                break;
                            }
                        }
                    }
                    if (tName1 == null) 
                        return null;
                }
            }
            if (tok != null) 
            {
                if (tok.BeginToken.Chars.IsAllLower && br == null) 
                {
                }
                else if (tok.BeginToken != tok.EndToken) 
                    ok = true;
                else if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tok.BeginToken)) 
                    return null;
                else if (br == null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tok.BeginToken.Previous, false, false)) 
                    return null;
                else if (tok.Chars.IsAllUpper) 
                    ok = true;
            }
            if (!ok) 
            {
                int cou = 0;
                for (Pullenti.Ner.Token tt = t0.Previous; tt != null && (cou < 100); tt = tt.Previous,cou++) 
                {
                    if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt.Next)) 
                        break;
                    Pullenti.Ner.Core.TerminToken tok1 = m_PropPref.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok1 != null) 
                    {
                        OrgProfile pr = (OrgProfile)tok1.Termin.Tag;
                        if (prof != OrgProfile.Undefined && prof != pr) 
                            continue;
                        if (tok1.Termin.Tag2 != null && prof == OrgProfile.Undefined) 
                            continue;
                        prof = pr;
                        ok = true;
                        break;
                    }
                    OrganizationReferent org1 = tt.GetReferent() as OrganizationReferent;
                    if (org1 != null && org1.FindSlot(OrganizationReferent.ATTR_PROFILE, null, true) != null) 
                    {
                        if ((org1.ContainsProfile(prof) || prof == OrgProfile.Undefined)) 
                        {
                            ok = true;
                            prof = org1.Profiles[0];
                            break;
                        }
                    }
                }
                cou = 0;
                if (!ok) 
                {
                    for (Pullenti.Ner.Token tt = t.Next; tt != null && (cou < 10); tt = tt.Next,cou++) 
                    {
                        if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt) && prof != OrgProfile.Sport) 
                            break;
                        Pullenti.Ner.Core.TerminToken tok1 = m_PropPref.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok1 != null) 
                        {
                            OrgProfile pr = (OrgProfile)tok1.Termin.Tag;
                            if (prof != OrgProfile.Undefined && prof != pr) 
                                continue;
                            if (tok1.Termin.Tag2 != null && prof == OrgProfile.Undefined) 
                                continue;
                            prof = pr;
                            ok = true;
                            break;
                        }
                        OrganizationReferent org1 = tt.GetReferent() as OrganizationReferent;
                        if (org1 != null && org1.FindSlot(OrganizationReferent.ATTR_PROFILE, null, true) != null) 
                        {
                            if ((org1.ContainsProfile(prof) || prof == OrgProfile.Undefined)) 
                            {
                                ok = true;
                                prof = org1.Profiles[0];
                                break;
                            }
                        }
                    }
                }
                if (!ok) 
                    return null;
            }
            if (prof == OrgProfile.Undefined) 
                return null;
            OrganizationReferent org = new OrganizationReferent();
            org.AddProfile(prof);
            if (prof2 != OrgProfile.Undefined) 
                org.AddProfile(prof2);
            if (prof == OrgProfile.Sport) 
                org.AddTypeStr("спортивный клуб");
            if (typ != null) 
                org.AddTypeStr(typ);
            if (br != null && ((tok == null || tok.EndToken != br.EndToken.Previous))) 
            {
                string nam;
                if (tok != null) 
                {
                    nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(tok.EndToken.Next, br.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    if (nam != null) 
                        nam = string.Format("{0} {1}", tok.Termin.CanonicText, nam);
                    else 
                        nam = tok.Termin.CanonicText;
                }
                else 
                    nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken, br.EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                if (nam != null) 
                    org.AddName(nam, true, null);
            }
            else if (tName1 != null) 
            {
                string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, tName1, Pullenti.Ner.Core.GetTextAttr.No);
                if (nam != null) 
                    nam = nam.Replace(". ", ".");
                org.AddName(nam, true, null);
            }
            else if (tok != null) 
            {
                org.AddName(tok.Termin.CanonicText, true, null);
                if (tok.Termin.Acronym != null) 
                    org.AddName(tok.Termin.Acronym, true, null);
                if (tok.Termin.AdditionalVars != null) 
                {
                    foreach (Pullenti.Ner.Core.Termin v in tok.Termin.AdditionalVars) 
                    {
                        org.AddName(v.CanonicText, true, null);
                    }
                }
            }
            else 
                return null;
            if ((((prof & OrgProfile.Media)) != OrgProfile.Undefined) && t0.Previous != null) 
            {
                if ((t0.Previous.IsValue("ЖУРНАЛ", null) || t0.Previous.IsValue("ИЗДАНИЕ", null) || t0.Previous.IsValue("ИЗДАТЕЛЬСТВО", null)) || t0.Previous.IsValue("АГЕНТСТВО", null)) 
                {
                    t0 = t0.Previous;
                    org.AddTypeStr(t0.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false).ToLower());
                    if (!t0.Previous.IsValue("АГЕНТСТВО", null)) 
                        org.AddProfile(OrgProfile.Press);
                }
            }
            Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(org, t0, t);
            if (br != null) 
                res.EndToken = br.EndToken;
            else if (tok != null) 
                res.EndToken = tok.EndToken;
            else if (tName1 != null) 
                res.EndToken = tName1;
            else 
                return null;
            return res;
        }
        static bool _isStdPressEnd(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return false;
            string str = (t as Pullenti.Ner.TextToken).Term;
            if ((((((((str == "NEWS" || str == "PRESS" || str == "PRESSE") || str == "ПРЕСС" || str == "НЬЮС") || str == "TIMES" || str == "TIME") || str == "ТАЙМС" || str == "POST") || str == "ПОСТ" || str == "TODAY") || str == "ТУДЕЙ" || str == "DAILY") || str == "ДЕЙЛИ" || str == "ИНФОРМ") || str == "INFORM") 
                return true;
            return false;
        }
        Pullenti.Ner.ReferentToken _tryAttachOrgSportAssociations(Pullenti.Ner.Token t, OrgAnalyzerData ad)
        {
            if (t == null) 
                return null;
            int cou = 0;
            string typ = null;
            Pullenti.Ner.Token t1 = null;
            Pullenti.Ner.Geo.GeoReferent geo = null;
            if (t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
            {
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                if (rt.EndToken.IsValue("ФЕДЕРАЦИЯ", null) || rt.BeginToken.IsValue("ФЕДЕРАЦИЯ", null)) 
                {
                    typ = "федерация";
                    geo = t.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                }
                t1 = t;
                if (t.Previous != null && t.Previous.Morph.Class.IsAdjective) 
                {
                    if (m_Sports.TryParse(t.Previous, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                    {
                        cou++;
                        t = t.Previous;
                    }
                }
            }
            else 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt == null) 
                    return null;
                if (npt.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                    return null;
                if (((npt.Noun.IsValue("АССОЦИАЦИЯ", null) || npt.Noun.IsValue("ФЕДЕРАЦИЯ", null) || npt.Noun.IsValue("СОЮЗ", null)) || npt.Noun.IsValue("СБОРНАЯ", null) || npt.Noun.IsValue("КОМАНДА", null)) || npt.Noun.IsValue("КЛУБ", null)) 
                    typ = npt.Noun.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false).ToLower();
                else if ((t is Pullenti.Ner.TextToken) && t.Chars.IsAllUpper && (t as Pullenti.Ner.TextToken).Term == "ФК") 
                    typ = "команда";
                else 
                    return null;
                if (typ == "команда") 
                    cou--;
                foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
                {
                    Pullenti.Ner.Core.TerminToken tok = m_Sports.TryParse(a.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null) 
                        cou++;
                    else if (a.BeginToken.IsValue("ОЛИМПИЙСКИЙ", null)) 
                        cou++;
                }
                if (t1 == null) 
                    t1 = npt.EndToken;
            }
            Pullenti.Ner.Token t11 = t1;
            string propname = null;
            string delWord = null;
            for (Pullenti.Ner.Token tt = t1.Next; tt != null; tt = tt.Next) 
            {
                if (tt.WhitespacesBeforeCount > 3) 
                    break;
                if (tt.IsCommaAnd) 
                    continue;
                if (tt.Morph.Class.IsPreposition && !tt.Morph.Class.IsAdverb && !tt.Morph.Class.IsVerb) 
                    continue;
                if (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                {
                    t1 = tt;
                    geo = tt.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                    if (typ == "сборная") 
                        cou++;
                    continue;
                }
                if (tt.IsValue("СТРАНА", null) && (tt is Pullenti.Ner.TextToken)) 
                {
                    t1 = (t11 = tt);
                    delWord = (tt as Pullenti.Ner.TextToken).Term;
                    continue;
                }
                Pullenti.Ner.Core.TerminToken tok = m_Sports.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                {
                    cou++;
                    t1 = (t11 = (tt = tok.EndToken));
                    continue;
                }
                if (tt.Chars.IsAllLower || tt.GetMorphClassInDictionary().IsVerb) 
                {
                }
                else 
                    tok = m_PropNames.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                {
                    propname = tok.Termin.CanonicText;
                    cou++;
                    t1 = (tt = tok.EndToken);
                    if (cou == 0 && typ == "команда") 
                        cou++;
                    continue;
                }
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br == null) 
                        break;
                    tok = m_PropNames.TryParse(tt.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null || cou > 0) 
                    {
                        propname = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt.Next, br.EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                        cou++;
                        tt = (t1 = br.EndToken);
                        continue;
                    }
                    break;
                }
                Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt1 == null) 
                    break;
                tok = m_Sports.TryParse(npt1.Noun.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok == null) 
                    break;
                cou++;
                t1 = (t11 = (tt = tok.EndToken));
            }
            if (cou <= 0) 
                return null;
            OrganizationReferent org = new OrganizationReferent();
            org.AddTypeStr(typ);
            if (typ == "федерация") 
                org.AddTypeStr("ассоциация");
            string name = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t11, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative | Pullenti.Ner.Core.GetTextAttr.IgnoreGeoReferent);
            if (name != null && delWord != null) 
            {
                if (name.Contains(" " + delWord)) 
                    name = name.Replace(" " + delWord, "");
            }
            if (name != null) 
                name = name.Replace(" РОССИЯ", "").Replace(" РОССИИ", "");
            if (propname != null) 
            {
                org.AddName(propname, true, null);
                if (name != null) 
                    org.AddTypeStr(name.ToLower());
            }
            else if (name != null) 
                org.AddName(name, true, null);
            if (geo != null) 
                org.AddGeoObject(geo);
            org.AddProfile(OrgProfile.Sport);
            return new Pullenti.Ner.ReferentToken(org, t, t1);
        }
        static Pullenti.Ner.Core.TerminCollection m_Sports;
        static Pullenti.Ner.Core.TerminCollection m_PropNames;
        static Pullenti.Ner.Core.TerminCollection m_PropPref;
        static void _initSport()
        {
            m_Sports = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"акробатика;акробатический;акробат", "бадминтон;бадминтонный;бадминтонист", "баскетбол;баскетбольный;баскетболист", "бейсбол;бейсбольный;бейсболист", "биатлон;биатлонный;биатлонист", "бильярд;бильярдный;бильярдист", "бобслей;бобслейный;бобслеист", "боулинг", "боевое искуство", "бокс;боксерский;боксер", "борьба;борец", "водное поло", "волейбол;волейбольный;волейболист", "гандбол;гандбольный;гандболист", "гольф;гольфный;гольфист", "горнолыжный спорт", "слалом;;слаломист", "сквош", "гребля", "дзюдо;дзюдоистский;дзюдоист", "карате;;каратист", "керлинг;;керлингист", "коньки;конькобежный;конькобежец", "легкая атлетика;легкоатлетический;легкоатлет", "лыжных гонок", "мотоцикл;мотоциклетный;мотоциклист", "тяжелая атлетика;тяжелоатлетический;тяжелоатлет", "ориентирование", "плавание;;пловец", "прыжки", "регби;;регбист", "пятиборье", "гимнастика;гимнастический;гимнаст", "самбо;;самбист", "сумо;;сумист", "сноуборд;сноубордический;сноубордист", "софтбол;софтбольный;софтболист", "стрельба;стрелковый", "спорт;спортивный", "теннис;теннисный;теннисист", "триатлон", "тхэквондо", "ушу;;ушуист", "фехтование;фехтовальный;фехтовальщик", "фигурное катание;;фигурист", "фристайл;фристальный", "футбол;футбольный;футболист", "мини-футбол", "хоккей;хоккейный;хоккеист", "хоккей на траве", "шахматы;шахматный;шахматист", "шашки;шашечный"}) 
            {
                string[] pp = s.ToUpper().Split(';');
                Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin();
                t.InitByNormalText(pp[0], Pullenti.Morph.MorphLang.RU);
                if (pp.Length > 1 && !string.IsNullOrEmpty(pp[1])) 
                    t.AddVariant(pp[1], true);
                if (pp.Length > 2 && !string.IsNullOrEmpty(pp[2])) 
                    t.AddVariant(pp[2], true);
                m_Sports.Add(t);
            }
            foreach (string s in new string[] {"байдарка", "каноэ", "лук", "трава", "коньки", "трамплин", "двоеборье", "батут", "вода", "шпага", "сабля", "лыжи", "скелетон"}) 
            {
                m_Sports.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag2 = s });
            }
            m_PropNames = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"СПАРТАК", "ЦСКА", "ЗЕНИТ!", "ТЕРЕК", "КРЫЛЬЯ СОВЕТОВ", "ДИНАМО", "АНЖИ", "КУБАНЬ", "АЛАНИЯ", "ТОРПЕДО", "АРСЕНАЛ!", "ЛОКОМОТИВ", "МЕТАЛЛУРГ!", "РОТОР", "СКА", "СОКОЛ!", "ХИМИК!", "ШИННИК", "РУБИН", "ШАХТЕР", "САЛАВАТ ЮЛАЕВ", "ТРАКТОР!", "АВАНГАРД!", "АВТОМОБИЛИСТ!", "АТЛАНТ!", "ВИТЯЗЬ!", "НАЦИОНАЛЬНАЯ ХОККЕЙНАЯ ЛИГА;НХЛ", "КОНТИНЕНТАЛЬНАЯ ХОККЕЙНАЯ ЛИГА;КХЛ", "СОЮЗ ЕВРОПЕЙСКИХ ФУТБОЛЬНЫХ АССОЦИАЦИЙ;УЕФА;UEFA", "Женская теннисная ассоциация;WTA", "Международная федерация бокса;IBF", "Всемирная боксерская организация;WBO", "РЕАЛ", "МАНЧЕСТЕР ЮНАЙТЕД", "манчестер сити", "БАРСЕЛОНА!", "БАВАРИЯ!", "ЧЕЛСИ", "ЛИВЕРПУЛЬ!", "ЮВЕНТУС", "НАПОЛИ", "БОЛОНЬЯ", "ФУЛХЭМ", "ЭВЕРТОН", "ФИЛАДЕЛЬФИЯ", "ПИТТСБУРГ", "ИНТЕР!", "Аякс", "ФЕРРАРИ;FERRARI", "РЕД БУЛЛ;RED BULL", "МАКЛАРЕН;MCLAREN", "МАКЛАРЕН-МЕРСЕДЕС;MCLAREN-MERCEDES"}) 
            {
                string ss = s.ToUpper();
                bool isBad = false;
                if (ss.EndsWith("!")) 
                {
                    isBad = true;
                    ss = ss.Substring(0, ss.Length - 1);
                }
                string[] pp = ss.Split(';');
                Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin(pp[0]) { Tag = OrgProfile.Sport };
                if (!isBad) 
                    t.Tag2 = ss;
                if (pp.Length > 1) 
                {
                    if (pp[1].Length < 4) 
                        t.Acronym = pp[1];
                    else 
                        t.AddVariant(pp[1], false);
                }
                m_PropNames.Add(t);
            }
            foreach (string s in new string[] {"ИТАР ТАСС;ТАСС;Телеграфное агентство советского союза", "Интерфакс;Interfax", "REGNUM", "ЛЕНТА.РУ;Lenta.ru", "Частный корреспондент;ЧасКор", "РИА Новости;Новости!;АПН", "Росбалт;RosBalt", "УНИАН", "ИНФОРОС;inforos", "Эхо Москвы", "Сноб!", "Серебряный дождь", "Вечерняя Москва;Вечерка", "Московский Комсомолец;Комсомолка", "Коммерсантъ;Коммерсант", "Афиша", "Аргументы и факты;АИФ", "Викиновости", "РосБизнесКонсалтинг;РБК", "Газета.ру", "Русский Репортер!", "Ведомости", "Вести!", "Рамблер Новости", "Живой Журнал;ЖЖ;livejournal;livejournal.ru", "Новый Мир", "Новая газета", "Правда!", "Известия!", "Бизнес!", "Русская жизнь!", "НТВ Плюс", "НТВ", "ВГТРК", "ТНТ", "Муз ТВ;МузТВ", "АСТ", "Эксмо", "Астрель", "Терра!", "Финанс!", "Собеседник!", "Newsru.com", "Nature!", "Россия сегодня;Russia Today;RT!", "БЕЛТА", "Ассошиэйтед Пресс;Associated Press", "France Press;France Presse;Франс пресс;Agence France Presse;AFP", "СИНЬХУА", "Gallup", "Cable News Network;CNN", "CBS News", "ABC News", "GoogleNews;Google News", "FoxNews;Fox News", "Reuters;Рейтер", "British Broadcasting Corporation;BBC;БиБиСи;BBC News", "MSNBC", "Голос Америки", "Аль Джазира;Al Jazeera", "Радио Свобода", "Радио Свободная Европа", "Guardian;Гардиан", "Daily Telegraph", "Times;Таймс!", "Independent!", "Financial Times", "Die Welt", "Bild!", "La Pepublica;Република!", "Le Monde", "People Daily", "BusinessWeek", "Economist!", "Forbes;Форбс", "Los Angeles Times", "New York Times", "Wall Street Journal;WSJ", "Washington Post", "Le Figaro;Фигаро", "Bloomberg", "DELFI!"}) 
            {
                string ss = s.ToUpper();
                bool isBad = false;
                if (ss.EndsWith("!")) 
                {
                    isBad = true;
                    ss = ss.Substring(0, ss.Length - 1);
                }
                string[] pp = ss.Split(';');
                Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin(pp[0]) { Tag = OrgProfile.Media };
                if (!isBad) 
                    t.Tag2 = ss;
                for (int ii = 1; ii < pp.Length; ii++) 
                {
                    if ((pp[ii].Length < 4) && t.Acronym == null) 
                        t.Acronym = pp[ii];
                    else 
                        t.AddVariant(pp[ii], false);
                }
                m_PropNames.Add(t);
            }
            foreach (string s in new string[] {"Машина времени!", "ДДТ", "Биттлз;Bittles", "ABBA;АББА", "Океан Эльзы;Океан Эльзи", "Аквариум!", "Крематорий!", "Наутилус;Наутилус Помпилиус!", "Пусси Райот;Пусси Риот;Pussy Riot", "Кино!", "Алиса!", "Агата Кристи!", "Чайф", "Ария!", "Земфира!", "Браво!", "Черный кофе!", "Воскресение!", "Урфин Джюс", "Сплин!", "Пикник!", "Мумий Троль", "Коррозия металла", "Арсенал!", "Ночные снайперы!", "Любэ", "Ласковый май!", "Noize MC", "Linkin Park", "ac dc", "green day!", "Pink Floyd;Пинк Флойд", "Depeche Mode", "Bon Jovi", "Nirvana;Нирвана!", "Queen;Квин!", "Nine Inch Nails", "Radioheads", "Pet Shop Boys", "Buggles"}) 
            {
                string ss = s.ToUpper();
                bool isBad = false;
                if (ss.EndsWith("!")) 
                {
                    isBad = true;
                    ss = ss.Substring(0, ss.Length - 1);
                }
                string[] pp = ss.Split(';');
                Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin(pp[0]) { Tag = OrgProfile.Music };
                if (!isBad) 
                    t.Tag2 = ss;
                for (int ii = 1; ii < pp.Length; ii++) 
                {
                    if ((pp[ii].Length < 4) && t.Acronym == null) 
                        t.Acronym = pp[ii];
                    else 
                        t.AddVariant(pp[ii], false);
                }
                m_PropNames.Add(t);
            }
            m_PropPref = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"ФАНАТ", "БОЛЕЛЬЩИК", "гонщик", "вратарь", "нападающий", "голкипер", "полузащитник", "полу-защитник", "центрфорвард", "центр-форвард", "форвард", "игрок", "легионер", "спортсмен"}) 
            {
                m_PropPref.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag = OrgProfile.Sport });
            }
            foreach (string s in new string[] {"защитник", "капитан", "пилот", "игра", "поле", "стадион", "гонка", "чемпионат", "турнир", "заезд", "матч", "кубок", "олипмиада", "финал", "полуфинал", "победа", "поражение", "разгром", "дивизион", "олипмиада", "финал", "полуфинал", "играть", "выигрывать", "выиграть", "проигрывать", "проиграть", "съиграть"}) 
            {
                m_PropPref.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag = OrgProfile.Sport, Tag2 = s });
            }
            foreach (string s in new string[] {"корреспондент", "фотокорреспондент", "репортер", "журналист", "тележурналист", "телеоператор", "главный редактор", "главред", "телеведущий", "редколлегия", "обозреватель", "сообщать", "сообщить", "передавать", "передать", "писать", "написать", "издавать", "пояснить", "пояснять", "разъяснить", "разъяснять", "сказать", "говорить", "спрашивать", "спросить", "отвечать", "ответить", "выяснять", "выяснить", "цитировать", "процитировать", "рассказать", "рассказывать", "информировать", "проинформировать", "поведать", "напечатать", "напоминать", "напомнить", "узнать", "узнавать", "репортаж", "интервью", "информации", "сведение", "ИА", "информагенство", "информагентство", "информационный", "газета", "журнал"}) 
            {
                m_PropPref.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag = OrgProfile.Media });
            }
            foreach (string s in new string[] {"сообщение", "статья", "номер", "журнал", "издание", "издательство", "агентство", "цитата", "редактор", "комментатор", "по данным", "оператор", "вышедший", "отчет", "вопрос", "читатель", "слушатель", "телезритель", "источник", "собеедник"}) 
            {
                m_PropPref.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag = OrgProfile.Media, Tag2 = s });
            }
            foreach (string s in new string[] {"музыкант", "певец", "певица", "ударник", "гитарист", "клавишник", "солист", "солистка", "исполнитель", "исполнительница", "исполнять", "исполнить", "концерт", "гастроль", "выступление", "известный", "известнейший", "популярный", "популярнейший", "рокгруппа", "панкгруппа", "группа", "альбом", "пластинка", "грампластинка", "концертный", "музыка", "песня", "сингл", "хит", "суперхит", "запись", "студия"}) 
            {
                m_PropPref.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag = OrgProfile.Media });
            }
        }
        Pullenti.Ner.ReferentToken TryAttachArmy(Pullenti.Ner.Token t, OrgAnalyzerData ad)
        {
            if (!(t is Pullenti.Ner.NumberToken) || t.WhitespacesAfterCount > 2) 
                return null;
            Pullenti.Ner.Org.Internal.OrgItemTypeToken typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t.Next, true, ad);
            if (typ == null) 
                return null;
            if (typ.Root != null && typ.Root.Profiles.Contains(OrgProfile.Army)) 
            {
                Pullenti.Ner.ReferentToken rt = this.TryAttachOrg(t.Next, ad, AttachType.High, null, false, 0, -1);
                if (rt != null) 
                {
                    if (rt.BeginToken == typ.BeginToken) 
                    {
                        rt.BeginToken = t;
                        (rt.Referent as OrganizationReferent).Number = (t as Pullenti.Ner.NumberToken).Value.ToString();
                    }
                    return rt;
                }
                OrganizationReferent org = new OrganizationReferent();
                org.AddType(typ, true);
                org.Number = (t as Pullenti.Ner.NumberToken).Value.ToString();
                return new Pullenti.Ner.ReferentToken(org, t, typ.EndToken);
            }
            return null;
        }
    }
}