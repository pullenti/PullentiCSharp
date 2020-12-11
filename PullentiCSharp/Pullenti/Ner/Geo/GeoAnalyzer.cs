/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Geo
{
    /// <summary>
    /// Анализатор географических объектов (стран, регионов, населённых пунктов)
    /// </summary>
    public class GeoAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("GEO")
        /// </summary>
        public const string ANALYZER_NAME = "GEO";
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
                return "Страны, регионы, города";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new GeoAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Geo.Internal.MetaGeo.GlobalMeta};
            }
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {"PHONE"};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Geo.Internal.MetaGeo.CountryCityImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("countrycity.png"));
                res.Add(Pullenti.Ner.Geo.Internal.MetaGeo.CountryImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("country.png"));
                res.Add(Pullenti.Ner.Geo.Internal.MetaGeo.CityImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("city.png"));
                res.Add(Pullenti.Ner.Geo.Internal.MetaGeo.DistrictImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("district.png"));
                res.Add(Pullenti.Ner.Geo.Internal.MetaGeo.RegionImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("region.png"));
                res.Add(Pullenti.Ner.Geo.Internal.MetaGeo.TerrImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("territory.png"));
                res.Add(Pullenti.Ner.Geo.Internal.MetaGeo.UnionImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("union.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == GeoReferent.OBJ_TYPENAME) 
                return new GeoReferent();
            return null;
        }
        public override int ProgressWeight
        {
            get
            {
                return 15;
            }
        }
        class GeoAnalyzerDataWithOntology : Pullenti.Ner.Core.AnalyzerDataWithOntology
        {
            static string[] ends = new string[] {"КИЙ", "КОЕ", "КАЯ"};
            public override Pullenti.Ner.Referent RegisterReferent(Pullenti.Ner.Referent referent)
            {
                Pullenti.Ner.Geo.GeoReferent g = referent as Pullenti.Ner.Geo.GeoReferent;
                if (g != null) 
                {
                    if (g.IsState) 
                    {
                    }
                    else if (g.IsRegion || ((g.IsCity && !g.IsBigCity))) 
                    {
                        List<string> names = new List<string>();
                        Pullenti.Morph.MorphGender gen = Pullenti.Morph.MorphGender.Undefined;
                        string basNam = null;
                        foreach (Pullenti.Ner.Slot s in g.Slots) 
                        {
                            if (s.TypeName == Pullenti.Ner.Geo.GeoReferent.ATTR_NAME) 
                                names.Add(s.Value as string);
                            else if (s.TypeName == Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE) 
                            {
                                string typ = s.Value as string;
                                if (Pullenti.Morph.LanguageHelper.EndsWithEx(typ, "район", "край", "округ", "улус")) 
                                    gen |= Pullenti.Morph.MorphGender.Masculine;
                                else if (Pullenti.Morph.LanguageHelper.EndsWithEx(typ, "область", "территория", null, null)) 
                                    gen |= Pullenti.Morph.MorphGender.Feminie;
                            }
                        }
                        for (int i = 0; i < names.Count; i++) 
                        {
                            string n = names[i];
                            int ii = n.IndexOf(' ');
                            if (ii > 0) 
                            {
                                if (g.GetSlotValue(Pullenti.Ner.Geo.GeoReferent.ATTR_REF) is Pullenti.Ner.Referent) 
                                    continue;
                                string nn = string.Format("{0} {1}", n.Substring(ii + 1), n.Substring(0, ii));
                                if (!names.Contains(nn)) 
                                {
                                    names.Add(nn);
                                    g.AddSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME, nn, false, 0);
                                    continue;
                                }
                                continue;
                            }
                            foreach (string end in ends) 
                            {
                                if (Pullenti.Morph.LanguageHelper.EndsWith(n, end)) 
                                {
                                    string nn = n.Substring(0, n.Length - 3);
                                    foreach (string end2 in ends) 
                                    {
                                        if (end2 != end) 
                                        {
                                            if (!names.Contains(nn + end2)) 
                                            {
                                                names.Add(nn + end2);
                                                g.AddSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME, nn + end2, false, 0);
                                            }
                                        }
                                    }
                                    if (gen == Pullenti.Morph.MorphGender.Masculine) 
                                    {
                                        foreach (string na in names) 
                                        {
                                            if (Pullenti.Morph.LanguageHelper.EndsWith(na, "ИЙ")) 
                                                basNam = na;
                                        }
                                    }
                                    else if (gen == Pullenti.Morph.MorphGender.Feminie) 
                                    {
                                        foreach (string na in names) 
                                        {
                                            if (Pullenti.Morph.LanguageHelper.EndsWith(na, "АЯ")) 
                                                basNam = na;
                                        }
                                    }
                                    else if (gen == Pullenti.Morph.MorphGender.Neuter) 
                                    {
                                        foreach (string na in names) 
                                        {
                                            if (Pullenti.Morph.LanguageHelper.EndsWith(na, "ОЕ")) 
                                                basNam = na;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        if (basNam != null && names.Count > 0 && names[0] != basNam) 
                        {
                            Pullenti.Ner.Slot sl = g.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME, basNam, true);
                            if (sl != null) 
                            {
                                g.Slots.Remove(sl);
                                g.Slots.Insert(0, sl);
                            }
                        }
                    }
                }
                return base.RegisterReferent(referent);
            }
        }

        public override Pullenti.Ner.Core.AnalyzerData CreateAnalyzerData()
        {
            return new GeoAnalyzerDataWithOntology();
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerDataWithOntology ad = kit.GetAnalyzerData(this) as Pullenti.Ner.Core.AnalyzerDataWithOntology;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                t.InnerBool = false;
            }
            List<GeoReferent> nonRegistered = new List<GeoReferent>();
            for (int step = 0; step < 2; step++) 
            {
                for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
                {
                    if (ad.Referents.Count >= 2000) 
                        break;
                    if (step > 0 && (t is Pullenti.Ner.ReferentToken)) 
                    {
                        GeoReferent geo = t.GetReferent() as GeoReferent;
                        if (((geo != null && t.Next != null && t.Next.IsChar('(')) && t.Next.Next != null && geo.CanBeEquals(t.Next.Next.GetReferent(), Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) && t.Next.Next.Next != null && t.Next.Next.Next.IsChar(')')) 
                        {
                            Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(geo, t, t.Next.Next.Next) { Morph = t.Morph };
                            kit.EmbedToken(rt0);
                            t = rt0;
                            continue;
                        }
                        if ((geo != null && t.Next != null && t.Next.IsHiphen) && t.Next.Next != null && geo.CanBeEquals(t.Next.Next.GetReferent(), Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                        {
                            Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(geo, t, t.Next.Next) { Morph = t.Morph };
                            kit.EmbedToken(rt0);
                            t = rt0;
                            continue;
                        }
                    }
                    bool ok = false;
                    if (step == 0 || t.InnerBool) 
                        ok = true;
                    else if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && !t.Chars.IsAllLower) 
                        ok = true;
                    List<Pullenti.Ner.Geo.Internal.TerrItemToken> cli = null;
                    if (ok) 
                        cli = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParseList(t, ad.LocalOntology, 5);
                    if (cli == null) 
                        continue;
                    t.InnerBool = true;
                    Pullenti.Ner.ReferentToken rt = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(cli, ad, false, null, nonRegistered);
                    if ((rt == null && cli.Count == 1 && cli[0].IsAdjective) && cli[0].OntoItem != null) 
                    {
                        Pullenti.Ner.Token tt = cli[0].EndToken.Next;
                        if (tt != null) 
                        {
                            if (tt.IsChar(',')) 
                                tt = tt.Next;
                            else if (tt.Morph.Class.IsConjunction) 
                            {
                                tt = tt.Next;
                                if (tt != null && tt.Morph.Class.IsConjunction) 
                                    tt = tt.Next;
                            }
                            List<Pullenti.Ner.Geo.Internal.TerrItemToken> cli1 = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParseList(tt, ad.LocalOntology, 2);
                            if (cli1 != null && cli1[0].OntoItem != null) 
                            {
                                GeoReferent g0 = cli[0].OntoItem.Referent as GeoReferent;
                                GeoReferent g1 = cli1[0].OntoItem.Referent as GeoReferent;
                                if ((g0 != null && g1 != null && g0.IsRegion) && g1.IsRegion) 
                                {
                                    if (g0.IsCity == g1.IsCity || g0.IsRegion == g1.IsRegion || g0.IsState == g1.IsState) 
                                        rt = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(cli, ad, true, null, null);
                                }
                            }
                            if (rt == null && (cli[0].OntoItem.Referent as GeoReferent).IsState) 
                            {
                                if ((rt == null && tt != null && (tt.GetReferent() is GeoReferent)) && tt.WhitespacesBeforeCount == 1) 
                                {
                                    GeoReferent geo2 = tt.GetReferent() as GeoReferent;
                                    if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(cli[0].OntoItem.Referent as GeoReferent, geo2)) 
                                    {
                                        Pullenti.Ner.Referent cl = cli[0].OntoItem.Referent.Clone();
                                        cl.Occurrence.Clear();
                                        rt = new Pullenti.Ner.ReferentToken(cl, cli[0].BeginToken, cli[0].EndToken) { Morph = cli[0].Morph };
                                    }
                                }
                                if (rt == null && step == 0) 
                                {
                                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(cli[0].BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                    if (npt != null && npt.EndChar >= tt.BeginChar) 
                                    {
                                        List<Pullenti.Ner.Geo.Internal.CityItemToken> cits = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseList(tt, ad.LocalOntology, 5);
                                        Pullenti.Ner.ReferentToken rt1 = (cits == null ? null : Pullenti.Ner.Geo.Internal.CityAttachHelper.TryAttachCity(cits, ad, false));
                                        if (rt1 != null) 
                                        {
                                            rt1.Referent = ad.RegisterReferent(rt1.Referent);
                                            kit.EmbedToken(rt1);
                                            Pullenti.Ner.Referent cl = cli[0].OntoItem.Referent.Clone();
                                            cl.Occurrence.Clear();
                                            rt = new Pullenti.Ner.ReferentToken(cl, cli[0].BeginToken, cli[0].EndToken) { Morph = cli[0].Morph };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (rt == null) 
                    {
                        List<Pullenti.Ner.Geo.Internal.CityItemToken> cits = this.TryParseCityListBack(t.Previous);
                        if (cits != null) 
                            rt = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(cli, ad, false, cits, null);
                    }
                    if (rt == null && cli.Count > 1) 
                    {
                        Pullenti.Ner.Token te = cli[cli.Count - 1].EndToken.Next;
                        if (te != null) 
                        {
                            if (te.Morph.Class.IsPreposition || te.IsChar(',')) 
                                te = te.Next;
                        }
                        List<Pullenti.Ner.Address.Internal.AddressItemToken> li = Pullenti.Ner.Address.Internal.AddressItemToken.TryParseList(te, null, 2);
                        if (li != null && li.Count > 0) 
                        {
                            if (li[0].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street || li[0].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Kilometer || li[0].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.House) 
                            {
                                Pullenti.Ner.Address.Internal.StreetItemToken ad0 = Pullenti.Ner.Address.Internal.StreetItemToken.TryParse(cli[0].BeginToken.Previous, null, false, null, false);
                                if (ad0 != null && ad0.Typ == Pullenti.Ner.Address.Internal.StreetItemType.Noun) 
                                {
                                }
                                else if (!cli[0].IsAdjective) 
                                    rt = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(cli, ad, true, null, null);
                                else 
                                {
                                    Pullenti.Ner.Address.Internal.AddressItemToken aaa = Pullenti.Ner.Address.Internal.AddressItemToken.TryParse(cli[0].BeginToken, null, false, false, null);
                                    if (aaa != null && aaa.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street) 
                                    {
                                    }
                                    else 
                                        rt = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(cli, ad, true, null, null);
                                }
                            }
                        }
                    }
                    if ((rt == null && cli.Count > 2 && cli[0].TerminItem == null) && cli[1].TerminItem == null && cli[2].TerminItem != null) 
                    {
                        Pullenti.Ner.Geo.Internal.CityItemToken cit = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseBack(cli[0].BeginToken.Previous);
                        if (cit != null && cit.Typ == Pullenti.Ner.Geo.Internal.CityItemToken.ItemType.Noun) 
                        {
                            if (((cli.Count > 4 && cli[1].TerminItem == null && cli[2].TerminItem != null) && cli[3].TerminItem == null && cli[4].TerminItem != null) && cli[2].TerminItem.CanonicText.EndsWith(cli[4].TerminItem.CanonicText)) 
                            {
                            }
                            else 
                            {
                                cli.RemoveAt(0);
                                rt = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(cli, ad, true, null, null);
                            }
                        }
                    }
                    if (rt != null) 
                    {
                        GeoReferent geo = rt.Referent as GeoReferent;
                        if (!geo.IsCity && !geo.IsState && geo.FindSlot(GeoReferent.ATTR_TYPE, "республика", true) == null) 
                            nonRegistered.Add(geo);
                        else 
                            rt.Referent = ad.RegisterReferent(geo);
                        kit.EmbedToken(rt);
                        t = rt;
                        if (step == 0) 
                        {
                            Pullenti.Ner.Token tt = t;
                            while (true) 
                            {
                                Pullenti.Ner.ReferentToken rr = this.TryAttachTerritoryBeforeCity(tt, ad);
                                if (rr == null) 
                                    break;
                                geo = rr.Referent as GeoReferent;
                                if (!geo.IsCity && !geo.IsState) 
                                    nonRegistered.Add(geo);
                                else 
                                    rr.Referent = ad.RegisterReferent(geo);
                                kit.EmbedToken(rr);
                                tt = rr;
                            }
                            if (t.Next != null && ((t.Next.IsComma || t.Next.IsChar('(')))) 
                            {
                                Pullenti.Ner.ReferentToken rt1 = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachStateUSATerritory(t.Next.Next);
                                if (rt1 != null) 
                                {
                                    rt1.Referent = ad.RegisterReferent(rt1.Referent);
                                    kit.EmbedToken(rt1);
                                    t = rt1;
                                }
                            }
                        }
                        continue;
                    }
                }
                if (step == 0) 
                {
                    if (!this.OnProgress(1, 4, kit)) 
                        return;
                }
                else 
                {
                    if (!this.OnProgress(2, 4, kit)) 
                        return;
                    if (ad.Referents.Count == 0 && nonRegistered.Count == 0) 
                        break;
                }
            }
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = (t == null ? null : t.Next)) 
            {
                GeoReferent g = t.GetReferent() as GeoReferent;
                if (g == null) 
                    continue;
                if (!(t.Previous is Pullenti.Ner.TextToken)) 
                    continue;
                Pullenti.Ner.Token t0 = null;
                if (t.Previous.IsValue("СОЮЗ", null)) 
                    t0 = t.Previous;
                else if (t.Previous.IsValue("ГОСУДАРСТВО", null) && t.Previous.Previous != null && t.Previous.Previous.IsValue("СОЮЗНЫЙ", null)) 
                    t0 = t.Previous.Previous;
                if (t0 == null) 
                    continue;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t0.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.EndToken == t.Previous) 
                    t0 = t0.Previous;
                GeoReferent uni = new GeoReferent();
                string typ = Pullenti.Ner.Core.MiscHelper.GetTextValue(t0, t.Previous, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                if (typ == null) 
                    continue;
                uni.AddTypUnion(t0.Kit.BaseLanguage);
                uni.AddTyp(typ.ToLower());
                uni.AddSlot(GeoReferent.ATTR_REF, g, false, 0);
                Pullenti.Ner.Token t1 = t;
                int i = 1;
                for (t = t.Next; t != null; t = t.Next) 
                {
                    if (t.IsCommaAnd) 
                        continue;
                    if ((((g = t.GetReferent() as GeoReferent))) == null) 
                        break;
                    if (uni.FindSlot(GeoReferent.ATTR_REF, g, true) != null) 
                        break;
                    if (t.IsNewlineBefore) 
                        break;
                    t1 = t;
                    uni.AddSlot(GeoReferent.ATTR_REF, g, false, 0);
                    i++;
                }
                if (i < 2) 
                    continue;
                uni = ad.RegisterReferent(uni) as GeoReferent;
                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(uni, t0, t1);
                kit.EmbedToken(rt);
                t = rt;
            }
            bool newCities = false;
            bool isCityBefore = false;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (t.IsCharOf(".,")) 
                    continue;
                List<Pullenti.Ner.Geo.Internal.CityItemToken> li = null;
                li = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseList(t, ad.LocalOntology, 5);
                Pullenti.Ner.ReferentToken rt;
                if (li != null) 
                {
                    if ((((rt = Pullenti.Ner.Geo.Internal.CityAttachHelper.TryAttachCity(li, ad, false)))) != null) 
                    {
                        Pullenti.Ner.Token tt = t.Previous;
                        if (tt != null && tt.IsComma) 
                            tt = tt.Previous;
                        if (tt != null && (tt.GetReferent() is GeoReferent)) 
                        {
                            if (tt.GetReferent().CanBeEquals(rt.Referent, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            {
                                rt.BeginToken = tt;
                                rt.Referent = ad.RegisterReferent(rt.Referent);
                                kit.EmbedToken(rt);
                                t = rt;
                                continue;
                            }
                        }
                        if (ad.Referents.Count > 2000) 
                            break;
                        rt.Referent = ad.RegisterReferent(rt.Referent) as GeoReferent;
                        kit.EmbedToken(rt);
                        t = rt;
                        isCityBefore = true;
                        newCities = true;
                        tt = t;
                        while (true) 
                        {
                            Pullenti.Ner.ReferentToken rr = this.TryAttachTerritoryBeforeCity(tt, ad);
                            if (rr == null) 
                                break;
                            GeoReferent geo = rr.Referent as GeoReferent;
                            if (!geo.IsCity && !geo.IsState) 
                                nonRegistered.Add(geo);
                            else 
                                rr.Referent = ad.RegisterReferent(geo);
                            kit.EmbedToken(rr);
                            tt = rr;
                        }
                        rt = this.TryAttachTerritoryAfterCity(t, ad);
                        if (rt != null) 
                        {
                            rt.Referent = ad.RegisterReferent(rt.Referent);
                            kit.EmbedToken(rt);
                            t = rt;
                        }
                        continue;
                    }
                }
                if (!t.InnerBool) 
                {
                    isCityBefore = false;
                    continue;
                }
                if (!isCityBefore) 
                    continue;
                List<Pullenti.Ner.Geo.Internal.TerrItemToken> tts = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParseList(t, ad.LocalOntology, 5);
                if (tts != null && tts.Count > 1 && ((tts[0].TerminItem != null || tts[1].TerminItem != null))) 
                {
                    if ((((rt = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(tts, ad, true, null, null)))) != null) 
                    {
                        GeoReferent geo = rt.Referent as GeoReferent;
                        if (!geo.IsCity && !geo.IsState) 
                            nonRegistered.Add(geo);
                        else 
                            rt.Referent = ad.RegisterReferent(geo);
                        kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                }
                isCityBefore = false;
            }
            if (newCities && ad.LocalOntology.Items.Count > 0) 
            {
                for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
                {
                    if (!(t is Pullenti.Ner.TextToken)) 
                        continue;
                    if (t.Chars.IsAllLower) 
                        continue;
                    List<Pullenti.Ner.Core.IntOntologyToken> li = ad.LocalOntology.TryAttach(t, null, false);
                    if (li == null) 
                        continue;
                    Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                    if (mc.IsProperSurname || mc.IsProperName || mc.IsProperSecname) 
                        continue;
                    if (t.Morph.Class.IsAdjective) 
                        continue;
                    GeoReferent geo = li[0].Item.Referent as GeoReferent;
                    if (geo != null) 
                    {
                        geo = geo.Clone() as GeoReferent;
                        geo.Occurrence.Clear();
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(geo, li[0].BeginToken, li[0].EndToken) { Morph = t.Morph };
                        if (rt.BeginToken == rt.EndToken) 
                            geo.AddName((t as Pullenti.Ner.TextToken).Term);
                        if (rt.BeginToken.Previous != null && rt.BeginToken.Previous.IsValue("СЕЛО", null) && geo.IsCity) 
                        {
                            rt.BeginToken = rt.BeginToken.Previous;
                            rt.Morph = rt.BeginToken.Morph;
                            geo.AddSlot(GeoReferent.ATTR_TYPE, "село", true, 0);
                        }
                        kit.EmbedToken(rt);
                        t = li[0].EndToken;
                    }
                }
            }
            bool goBack = false;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (goBack) 
                {
                    goBack = false;
                    if (t.Previous != null) 
                        t = t.Previous;
                }
                GeoReferent geo = t.GetReferent() as GeoReferent;
                if (geo == null) 
                    continue;
                GeoReferent geo1 = null;
                Pullenti.Ner.Token tt = t.Next;
                bool bra = false;
                bool comma1 = false;
                bool comma2 = false;
                bool inp = false;
                bool adj = false;
                for (; tt != null; tt = tt.Next) 
                {
                    if (tt.IsCharOf(",")) 
                    {
                        comma1 = true;
                        continue;
                    }
                    if (tt.IsValue("IN", null) || tt.IsValue("В", null)) 
                    {
                        inp = true;
                        continue;
                    }
                    if (Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(tt)) 
                    {
                        adj = true;
                        tt = tt.Next;
                        continue;
                    }
                    Pullenti.Ner.Address.Internal.AddressItemToken det = Pullenti.Ner.Address.Internal.AddressItemToken.TryAttachDetail(tt);
                    if (det != null) 
                    {
                        tt = det.EndToken;
                        comma1 = true;
                        continue;
                    }
                    if (tt.Morph.Class.IsPreposition) 
                        continue;
                    if (tt.IsChar('(') && tt == t.Next) 
                    {
                        bra = true;
                        continue;
                    }
                    if ((tt is Pullenti.Ner.TextToken) && Pullenti.Ner.Core.BracketHelper.IsBracket(tt, true)) 
                        continue;
                    geo1 = tt.GetReferent() as GeoReferent;
                    break;
                }
                if (geo1 == null) 
                    continue;
                if (tt.WhitespacesBeforeCount > 15) 
                    continue;
                Pullenti.Ner.Token ttt = tt.Next;
                GeoReferent geo2 = null;
                for (; ttt != null; ttt = ttt.Next) 
                {
                    if (ttt.IsCommaAnd) 
                    {
                        comma2 = true;
                        continue;
                    }
                    Pullenti.Ner.Address.Internal.AddressItemToken det = Pullenti.Ner.Address.Internal.AddressItemToken.TryAttachDetail(ttt);
                    if (det != null) 
                    {
                        ttt = det.EndToken;
                        comma2 = true;
                        continue;
                    }
                    if (ttt.Morph.Class.IsPreposition) 
                        continue;
                    geo2 = ttt.GetReferent() as GeoReferent;
                    break;
                }
                if (ttt != null && ttt.WhitespacesBeforeCount > 15) 
                    geo2 = null;
                if (geo2 != null) 
                {
                    if ((comma1 && comma2 && Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(t, tt)) && Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(tt, ttt)) 
                    {
                        geo2.Higher = geo1;
                        geo1.Higher = geo;
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(geo2, t, ttt) { Morph = ttt.Morph };
                        kit.EmbedToken(rt);
                        t = rt;
                        goBack = true;
                        continue;
                    }
                    else if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(ttt, tt)) 
                    {
                        if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(t, ttt)) 
                        {
                            geo2.Higher = geo;
                            geo1.Higher = geo2;
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(geo1, t, ttt) { Morph = t.Morph };
                            kit.EmbedToken(rt);
                            t = rt;
                            goBack = true;
                            continue;
                        }
                        if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(ttt, t) && Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(t, tt)) 
                        {
                            geo.Higher = geo2;
                            geo1.Higher = geo;
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(geo1, t, ttt) { Morph = tt.Morph };
                            kit.EmbedToken(rt);
                            t = rt;
                            goBack = true;
                            continue;
                        }
                        if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(tt, t)) 
                        {
                            geo.Higher = geo1;
                            geo1.Higher = geo2;
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(geo, t, ttt) { Morph = t.Morph };
                            kit.EmbedToken(rt);
                            t = rt;
                            goBack = true;
                            continue;
                        }
                    }
                    if (comma2) 
                        continue;
                }
                if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(t, tt) && ((!Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(tt, t) || adj))) 
                {
                    geo1.Higher = geo;
                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(geo1, t, tt) { Morph = tt.Morph };
                    if ((geo1.IsCity && !geo.IsCity && t.Previous != null) && t.Previous.IsValue("СТОЛИЦА", "СТОЛИЦЯ")) 
                    {
                        rt.BeginToken = t.Previous;
                        rt.Morph = t.Previous.Morph;
                    }
                    kit.EmbedToken(rt);
                    t = rt;
                    goBack = true;
                    continue;
                }
                if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(tt, t) && ((!Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigherToken(t, tt) || inp))) 
                {
                    if (geo.Higher == null) 
                        geo.Higher = geo1;
                    else if (geo1.Higher == null && Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(geo.Higher, geo1) && !Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(geo1, geo.Higher)) 
                    {
                        geo1.Higher = geo.Higher;
                        geo.Higher = geo1;
                    }
                    else 
                        geo.Higher = geo1;
                    if (bra && tt.Next != null && tt.Next.IsChar(')')) 
                        tt = tt.Next;
                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(geo, t, tt) { Morph = t.Morph };
                    kit.EmbedToken(rt);
                    t = rt;
                    goBack = true;
                    continue;
                }
                if ((!tt.Morph.Class.IsAdjective && !t.Morph.Class.IsAdjective && tt.Chars.IsCyrillicLetter) && t.Chars.IsCyrillicLetter && !tt.Morph.Case.IsInstrumental) 
                {
                    for (GeoReferent geo0 = geo; geo0 != null; geo0 = geo0.Higher) 
                    {
                        if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(geo1, geo0)) 
                        {
                            geo0.Higher = geo1;
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(geo, t, tt) { Morph = t.Morph };
                            kit.EmbedToken(rt);
                            t = rt;
                            goBack = true;
                            break;
                        }
                    }
                }
            }
            if (nonRegistered.Count == 0) 
                return;
            for (int k = 0; k < nonRegistered.Count; k++) 
            {
                bool ch = false;
                for (int i = 0; i < (nonRegistered.Count - 1); i++) 
                {
                    if (geoComp(nonRegistered[i], nonRegistered[i + 1]) > 0) 
                    {
                        ch = true;
                        GeoReferent v = nonRegistered[i];
                        nonRegistered[i] = nonRegistered[i + 1];
                        nonRegistered[i + 1] = v;
                    }
                }
                if (!ch) 
                    break;
            }
            foreach (GeoReferent g in nonRegistered) 
            {
                g.Tag = null;
            }
            foreach (GeoReferent ng in nonRegistered) 
            {
                foreach (Pullenti.Ner.Slot s in ng.Slots) 
                {
                    if (s.Value is GeoReferent) 
                    {
                        if ((s.Value as GeoReferent).Tag is GeoReferent) 
                            ng.UploadSlot(s, (s.Value as GeoReferent).Tag as GeoReferent);
                    }
                }
                GeoReferent rg = ad.RegisterReferent(ng) as GeoReferent;
                if (rg == ng) 
                    continue;
                ng.Tag = rg;
                foreach (Pullenti.Ner.TextAnnotation oc in ng.Occurrence) 
                {
                    oc.OccurenceOf = rg;
                    rg.AddOccurence(oc);
                }
            }
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                GeoReferent geo = t.GetReferent() as GeoReferent;
                if (geo == null) 
                    continue;
                _replaceTerrs(t as Pullenti.Ner.ReferentToken);
            }
        }
        static void _replaceTerrs(Pullenti.Ner.ReferentToken mt)
        {
            if (mt == null) 
                return;
            GeoReferent geo = mt.Referent as GeoReferent;
            if (geo != null && (geo.Tag is GeoReferent)) 
                mt.Referent = geo.Tag as GeoReferent;
            if (geo != null) 
            {
                foreach (Pullenti.Ner.Slot s in geo.Slots) 
                {
                    if (s.Value is GeoReferent) 
                    {
                        GeoReferent g = s.Value as GeoReferent;
                        if (g.Tag is GeoReferent) 
                            geo.UploadSlot(s, g.Tag);
                    }
                }
            }
            for (Pullenti.Ner.Token t = mt.BeginToken; t != null; t = t.Next) 
            {
                if (t.EndChar > mt.EndToken.EndChar) 
                    break;
                else 
                {
                    if (t is Pullenti.Ner.ReferentToken) 
                        _replaceTerrs(t as Pullenti.Ner.ReferentToken);
                    if (t == mt.EndToken) 
                        break;
                }
            }
        }
        static int geoComp(GeoReferent x, GeoReferent y)
        {
            int xcou = 0;
            for (GeoReferent g = x.Higher; g != null; g = g.Higher) 
            {
                xcou++;
            }
            int ycou = 0;
            for (GeoReferent g = y.Higher; g != null; g = g.Higher) 
            {
                ycou++;
            }
            if (xcou < ycou) 
                return -1;
            if (xcou > ycou) 
                return 1;
            return string.Compare(x.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0), y.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0));
        }
        List<Pullenti.Ner.Geo.Internal.CityItemToken> TryParseCityListBack(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            while (t != null && ((t.Morph.Class.IsPreposition || t.IsCharOf(",.") || t.Morph.Class.IsConjunction))) 
            {
                t = t.Previous;
            }
            if (t == null) 
                return null;
            List<Pullenti.Ner.Geo.Internal.CityItemToken> res = null;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Previous) 
            {
                if (!(tt is Pullenti.Ner.TextToken)) 
                    break;
                if (tt.Previous != null && tt.Previous.IsHiphen && (tt.Previous.Previous is Pullenti.Ner.TextToken)) 
                {
                    if (!tt.IsWhitespaceBefore && !tt.Previous.IsWhitespaceBefore) 
                        tt = tt.Previous.Previous;
                }
                List<Pullenti.Ner.Geo.Internal.CityItemToken> ci = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseList(tt, null, 5);
                if (ci == null && tt.Previous != null) 
                    ci = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseList(tt.Previous, null, 5);
                if (ci == null) 
                    break;
                if (ci[ci.Count - 1].EndToken == t) 
                    res = ci;
            }
            if (res != null) 
                res.Reverse();
            return res;
        }
        Pullenti.Ner.ReferentToken TryAttachTerritoryBeforeCity(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerDataWithOntology ad)
        {
            if (t is Pullenti.Ner.ReferentToken) 
                t = t.Previous;
            for (; t != null; t = t.Previous) 
            {
                if (!t.IsCharOf(",.") && !t.Morph.Class.IsPreposition) 
                    break;
            }
            if (t == null) 
                return null;
            int i = 0;
            Pullenti.Ner.ReferentToken res = null;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Previous) 
            {
                i++;
                if (tt.IsNewlineAfter && !tt.InnerBool) 
                    break;
                if (i > 10) 
                    break;
                List<Pullenti.Ner.Geo.Internal.TerrItemToken> tits0 = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParseList(tt, ad.LocalOntology, 5);
                if (tits0 == null) 
                    continue;
                if (tits0[tits0.Count - 1].EndToken != t) 
                    break;
                List<Pullenti.Ner.Geo.Internal.TerrItemToken> tits1 = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParseList(tt.Previous, ad.LocalOntology, 5);
                if (tits1 != null && tits1[tits1.Count - 1].EndToken == t && tits1.Count == tits0.Count) 
                    tits0 = tits1;
                Pullenti.Ner.ReferentToken rr = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(tits0, ad, false, null, null);
                if (rr != null) 
                    res = rr;
            }
            return res;
        }
        Pullenti.Ner.ReferentToken TryAttachTerritoryAfterCity(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerDataWithOntology ad)
        {
            if (t == null) 
                return null;
            GeoReferent city = t.GetReferent() as GeoReferent;
            if (city == null) 
                return null;
            if (!city.IsCity) 
                return null;
            if (t.Next == null || !t.Next.IsComma || t.Next.WhitespacesAfterCount > 1) 
                return null;
            Pullenti.Ner.Token tt = t.Next.Next;
            if (tt == null || !tt.Chars.IsCapitalUpper || !(tt is Pullenti.Ner.TextToken)) 
                return null;
            if (tt.Chars.IsLatinLetter) 
            {
                Pullenti.Ner.ReferentToken re1 = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachStateUSATerritory(tt);
                if (re1 != null) 
                    return re1;
            }
            Pullenti.Ner.Token t0 = tt;
            Pullenti.Ner.Token t1 = tt;
            for (int i = 0; i < 2; i++) 
            {
                Pullenti.Ner.Geo.Internal.TerrItemToken tit0 = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParse(tt, ad.LocalOntology, false, false, null);
                if (tit0 == null || tit0.TerminItem != null) 
                {
                    if (i == 0) 
                        return null;
                }
                Pullenti.Ner.Geo.Internal.CityItemToken cit0 = Pullenti.Ner.Geo.Internal.CityItemToken.TryParse(tt, ad.LocalOntology, false, null);
                if (cit0 == null || cit0.Typ == Pullenti.Ner.Geo.Internal.CityItemToken.ItemType.Noun) 
                {
                    if (i == 0) 
                        return null;
                }
                Pullenti.Ner.Address.Internal.AddressItemToken ait0 = Pullenti.Ner.Address.Internal.AddressItemToken.TryParse(tt, null, false, false, null);
                if (ait0 != null) 
                    return null;
                if (tit0 == null) 
                {
                    if (!tt.Chars.IsCyrillicLetter) 
                        return null;
                    Pullenti.Morph.MorphClass cla = tt.GetMorphClassInDictionary();
                    if (!cla.IsNoun && !cla.IsAdjective) 
                        return null;
                    t1 = tt;
                }
                else 
                    t1 = (tt = tit0.EndToken);
                if (tt.Next == null) 
                    return null;
                if (tt.Next.IsComma) 
                {
                    tt = tt.Next.Next;
                    break;
                }
                if (i > 0) 
                    return null;
                tt = tt.Next;
            }
            Pullenti.Ner.Address.Internal.AddressItemToken ait = Pullenti.Ner.Address.Internal.AddressItemToken.TryParse(tt, null, false, false, null);
            if (ait == null) 
                return null;
            if (ait.Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street || ait.RefToken != null) 
                return null;
            GeoReferent reg = new GeoReferent();
            reg.AddTyp("муниципальный район");
            reg.AddName(Pullenti.Ner.Core.MiscHelper.GetTextValue(t0, t1, Pullenti.Ner.Core.GetTextAttr.No));
            return new Pullenti.Ner.ReferentToken(reg, t0, t1);
        }
        // Это привязка стран к прилагательным (например, "французский лидер")
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            if (!(begin is Pullenti.Ner.TextToken)) 
                return null;
            if (begin.Kit.RecurseLevel > 3) 
                return null;
            begin.Kit.RecurseLevel++;
            List<Pullenti.Ner.Core.TerminToken> toks = Pullenti.Ner.Geo.Internal.CityItemToken.m_CityAdjectives.TryParseAll(begin, Pullenti.Ner.Core.TerminParseAttr.FullwordsOnly);
            begin.Kit.RecurseLevel--;
            if (toks != null) 
            {
                foreach (Pullenti.Ner.Core.TerminToken tok in toks) 
                {
                    Pullenti.Ner.Core.IntOntologyItem cit = tok.Termin.Tag as Pullenti.Ner.Core.IntOntologyItem;
                    if (cit == null) 
                        continue;
                    GeoReferent city = new GeoReferent();
                    city.AddName(cit.CanonicText);
                    city.AddTypCity(begin.Kit.BaseLanguage);
                    return new Pullenti.Ner.ReferentToken(city, tok.BeginToken, tok.EndToken) { Morph = tok.Morph, Data = begin.Kit.GetAnalyzerData(this) };
                }
                return null;
            }
            Pullenti.Ner.Core.AnalyzerDataWithOntology ad = begin.Kit.GetAnalyzerData(this) as Pullenti.Ner.Core.AnalyzerDataWithOntology;
            if (!begin.Morph.Class.IsAdjective) 
            {
                Pullenti.Ner.TextToken te = begin as Pullenti.Ner.TextToken;
                if ((te.Chars.IsAllUpper && te.Chars.IsCyrillicLetter && te.LengthChar == 2) && te.GetMorphClassInDictionary().IsUndefined) 
                {
                    string abbr = te.Term;
                    GeoReferent geo0 = null;
                    int cou = 0;
                    foreach (Pullenti.Ner.Core.IntOntologyItem t in ad.LocalOntology.Items) 
                    {
                        GeoReferent geo = t.Referent as GeoReferent;
                        if (geo == null) 
                            continue;
                        if (!geo.IsRegion && !geo.IsState) 
                            continue;
                        if (geo.CheckAbbr(abbr)) 
                        {
                            cou++;
                            geo0 = geo;
                        }
                    }
                    if (cou == 1) 
                        return new Pullenti.Ner.ReferentToken(geo0, begin, begin) { Data = ad };
                }
                Pullenti.Ner.Geo.Internal.TerrItemToken tt0 = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParse(begin, ad.LocalOntology, true, false, null);
                if (tt0 != null && tt0.TerminItem != null && tt0.TerminItem.CanonicText == "РАЙОН") 
                {
                    Pullenti.Ner.Geo.Internal.TerrItemToken tt1 = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParse(tt0.EndToken.Next, ad.LocalOntology, true, false, null);
                    if ((tt1 != null && tt1.Chars.IsCapitalUpper && tt1.TerminItem == null) && tt1.OntoItem == null) 
                    {
                        List<Pullenti.Ner.Geo.Internal.TerrItemToken> li = new List<Pullenti.Ner.Geo.Internal.TerrItemToken>();
                        li.Add(tt0);
                        li.Add(tt1);
                        Pullenti.Ner.ReferentToken res = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(li, ad, true, null, null);
                        if (res == null) 
                            return null;
                        res.Morph = begin.Morph;
                        res.Data = ad;
                        return res;
                    }
                }
                begin.Kit.RecurseLevel++;
                List<Pullenti.Ner.Geo.Internal.CityItemToken> ctoks = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseList(begin, null, 3);
                if (ctoks == null && begin.Morph.Class.IsPreposition) 
                    ctoks = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseList(begin.Next, null, 3);
                begin.Kit.RecurseLevel--;
                if (ctoks != null) 
                {
                    if (((ctoks.Count == 2 && ctoks[0].Typ == Pullenti.Ner.Geo.Internal.CityItemToken.ItemType.Noun && ctoks[1].Typ == Pullenti.Ner.Geo.Internal.CityItemToken.ItemType.ProperName)) || ((ctoks.Count == 1 && ctoks[0].Typ == Pullenti.Ner.Geo.Internal.CityItemToken.ItemType.City))) 
                    {
                        if (ctoks.Count == 1 && ctoks[0].BeginToken.GetMorphClassInDictionary().IsProperSurname) 
                        {
                            begin.Kit.RecurseLevel++;
                            Pullenti.Ner.ReferentToken kk = begin.Kit.ProcessReferent("PERSON", ctoks[0].BeginToken);
                            begin.Kit.RecurseLevel--;
                            if (kk != null) 
                                return null;
                        }
                        Pullenti.Ner.ReferentToken res = Pullenti.Ner.Geo.Internal.CityAttachHelper.TryAttachCity(ctoks, ad, true);
                        if (res != null) 
                        {
                            res.Data = ad;
                            return res;
                        }
                    }
                }
                if ((ctoks != null && ctoks.Count == 1 && ctoks[0].Typ == Pullenti.Ner.Geo.Internal.CityItemToken.ItemType.Noun) && ctoks[0].Value == "ГОРОД") 
                {
                    int cou = 0;
                    for (Pullenti.Ner.Token t = begin.Previous; t != null; t = t.Previous) 
                    {
                        if ((++cou) > 500) 
                            break;
                        if (!(t is Pullenti.Ner.ReferentToken)) 
                            continue;
                        List<Pullenti.Ner.Referent> geos = t.GetReferents();
                        if (geos == null) 
                            continue;
                        foreach (Pullenti.Ner.Referent g in geos) 
                        {
                            GeoReferent gg = g as GeoReferent;
                            if (gg != null) 
                            {
                                if (gg.IsCity) 
                                    return new Pullenti.Ner.ReferentToken(gg, begin, ctoks[0].EndToken) { Morph = ctoks[0].Morph, Data = ad };
                                if (gg.Higher != null && gg.Higher.IsCity) 
                                    return new Pullenti.Ner.ReferentToken(gg.Higher, begin, ctoks[0].EndToken) { Morph = ctoks[0].Morph, Data = ad };
                            }
                        }
                    }
                }
                if (tt0 != null && tt0.OntoItem != null) 
                {
                }
                else 
                    return null;
            }
            begin.Kit.RecurseLevel++;
            Pullenti.Ner.Geo.Internal.TerrItemToken tt = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParse(begin, ad.LocalOntology, true, false, null);
            begin.Kit.RecurseLevel--;
            if (tt == null || tt.OntoItem == null) 
            {
                List<Pullenti.Ner.Core.IntOntologyToken> tok = Pullenti.Ner.Geo.Internal.TerrItemToken.m_TerrOntology.TryAttach(begin, null, false);
                if ((tok != null && tok[0].Item != null && (tok[0].Item.Referent is GeoReferent)) && (tok[0].Item.Referent as GeoReferent).IsState) 
                    tt = new Pullenti.Ner.Geo.Internal.TerrItemToken(tok[0].BeginToken, tok[0].EndToken) { OntoItem = tok[0].Item };
            }
            if (tt == null) 
                return null;
            if (tt.OntoItem != null) 
            {
                List<Pullenti.Ner.Geo.Internal.TerrItemToken> li = new List<Pullenti.Ner.Geo.Internal.TerrItemToken>();
                li.Add(tt);
                Pullenti.Ner.ReferentToken res = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(li, ad, true, null, null);
                if (res == null) 
                    tt.OntoItem = null;
                else 
                {
                    if (res.BeginToken == res.EndToken) 
                    {
                        Pullenti.Morph.MorphClass mc = res.BeginToken.GetMorphClassInDictionary();
                        if (mc.IsAdjective) 
                        {
                            GeoReferent geo = tt.OntoItem.Referent as GeoReferent;
                            if (geo.IsCity || geo.IsState) 
                            {
                            }
                            else if (geo.FindSlot(GeoReferent.ATTR_TYPE, "федеральный округ", true) != null) 
                                return null;
                        }
                    }
                    res.Data = ad;
                    return res;
                }
            }
            if (!tt.IsAdjective) 
                return null;
            if (tt.OntoItem == null) 
            {
                Pullenti.Ner.Token t1 = tt.EndToken.Next;
                if (t1 == null) 
                    return null;
                begin.Kit.RecurseLevel++;
                Pullenti.Ner.Geo.Internal.TerrItemToken ttyp = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParse(t1, ad.LocalOntology, true, true, null);
                begin.Kit.RecurseLevel--;
                if (ttyp == null || ttyp.TerminItem == null) 
                {
                    List<Pullenti.Ner.Geo.Internal.CityItemToken> cits = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseList(begin, null, 2);
                    if (cits != null && cits[0].Typ == Pullenti.Ner.Geo.Internal.CityItemToken.ItemType.City) 
                        return Pullenti.Ner.Geo.Internal.CityAttachHelper.TryAttachCity(cits, ad, true);
                    return null;
                }
                if (t1.GetMorphClassInDictionary().IsAdjective) 
                    return null;
                List<Pullenti.Ner.Geo.Internal.TerrItemToken> li = new List<Pullenti.Ner.Geo.Internal.TerrItemToken>();
                li.Add(tt);
                li.Add(ttyp);
                Pullenti.Ner.ReferentToken res = Pullenti.Ner.Geo.Internal.TerrAttachHelper.TryAttachTerritory(li, ad, true, null, null);
                if (res == null) 
                    return null;
                res.Morph = ttyp.Morph;
                res.Data = ad;
                return res;
            }
            return null;
        }
        public Pullenti.Ner.ReferentToken ProcessCitizen(Pullenti.Ner.Token begin)
        {
            if (!(begin is Pullenti.Ner.TextToken)) 
                return null;
            Pullenti.Ner.Core.TerminToken tok = Pullenti.Ner.Geo.Internal.TerrItemToken.m_MansByState.TryParse(begin, Pullenti.Ner.Core.TerminParseAttr.FullwordsOnly);
            if (tok != null) 
                tok.Morph.Gender = tok.Termin.Gender;
            if (tok == null) 
                return null;
            GeoReferent geo0 = tok.Termin.Tag as GeoReferent;
            if (geo0 == null) 
                return null;
            GeoReferent geo = new GeoReferent();
            geo.MergeSlots2(geo0, begin.Kit.BaseLanguage);
            Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(geo, tok.BeginToken, tok.EndToken);
            res.Morph = tok.Morph;
            Pullenti.Ner.Core.AnalyzerDataWithOntology ad = begin.Kit.GetAnalyzerData(this) as Pullenti.Ner.Core.AnalyzerDataWithOntology;
            res.Data = ad;
            return res;
        }
        public override Pullenti.Ner.ReferentToken ProcessOntologyItem(Pullenti.Ner.Token begin)
        {
            List<Pullenti.Ner.Geo.Internal.CityItemToken> li = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseList(begin, null, 4);
            if (li != null && li.Count > 1 && li[0].Typ == Pullenti.Ner.Geo.Internal.CityItemToken.ItemType.Noun) 
            {
                Pullenti.Ner.ReferentToken rt = Pullenti.Ner.Geo.Internal.CityAttachHelper.TryAttachCity(li, null, true);
                if (rt == null) 
                    return null;
                GeoReferent city = rt.Referent as GeoReferent;
                for (Pullenti.Ner.Token t = rt.EndToken.Next; t != null; t = t.Next) 
                {
                    if (!t.IsChar(';')) 
                        continue;
                    t = t.Next;
                    if (t == null) 
                        break;
                    li = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseList(t, null, 4);
                    Pullenti.Ner.ReferentToken rt1 = Pullenti.Ner.Geo.Internal.CityAttachHelper.TryAttachCity(li, null, false);
                    if (rt1 != null) 
                    {
                        t = (rt.EndToken = rt1.EndToken);
                        city.MergeSlots2(rt1.Referent, begin.Kit.BaseLanguage);
                    }
                    else 
                    {
                        Pullenti.Ner.Token tt = null;
                        for (Pullenti.Ner.Token ttt = t; ttt != null; ttt = ttt.Next) 
                        {
                            if (ttt.IsChar(';')) 
                                break;
                            else 
                                tt = ttt;
                        }
                        if (tt != null) 
                        {
                            string str = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, tt, Pullenti.Ner.Core.GetTextAttr.No);
                            if (str != null) 
                                city.AddName(str);
                            t = (rt.EndToken = tt);
                        }
                    }
                }
                return rt;
            }
            string typ = null;
            GeoReferent terr = null;
            Pullenti.Ner.Token te = null;
            for (Pullenti.Ner.Token t = begin; t != null; t = t.Next) 
            {
                Pullenti.Ner.Token t0 = t;
                Pullenti.Ner.Token t1 = null;
                Pullenti.Ner.Token tn0 = null;
                Pullenti.Ner.Token tn1 = null;
                for (Pullenti.Ner.Token tt = t0; tt != null; tt = tt.Next) 
                {
                    if (tt.IsCharOf(";")) 
                        break;
                    Pullenti.Ner.Geo.Internal.TerrItemToken tit = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParse(tt, null, false, false, null);
                    if (tit != null && tit.TerminItem != null) 
                    {
                        if (!tit.IsAdjective) 
                        {
                            if (typ == null) 
                                typ = tit.TerminItem.CanonicText;
                            tt = tit.EndToken;
                            t1 = tt;
                            continue;
                        }
                    }
                    else if (tit != null && tit.OntoItem != null) 
                    {
                    }
                    if (tn0 == null) 
                        tn0 = tt;
                    if (tit != null) 
                        tt = tit.EndToken;
                    t1 = (tn1 = tt);
                }
                if (t1 == null) 
                    continue;
                if (terr == null) 
                    terr = new GeoReferent();
                if (tn0 != null) 
                    terr.AddName(Pullenti.Ner.Core.MiscHelper.GetTextValue(tn0, tn1, Pullenti.Ner.Core.GetTextAttr.No));
                t = (te = t1);
            }
            if (terr == null || te == null) 
                return null;
            if (typ != null) 
                terr.AddTyp(typ);
            if (!terr.IsCity && !terr.IsRegion && !terr.IsState) 
                terr.AddTypReg(begin.Kit.BaseLanguage);
            return new Pullenti.Ner.ReferentToken(terr, begin, te);
        }
        /// <summary>
        /// Получить список всех стран из внутреннего словаря
        /// </summary>
        public static List<Pullenti.Ner.Referent> GetAllCountries()
        {
            return Pullenti.Ner.Geo.Internal.TerrItemToken.m_AllStates;
        }
        static bool m_Initialized = false;
        public static void Initialize()
        {
            if (m_Initialized) 
                return;
            m_Initialized = true;
            Pullenti.Ner.Geo.Internal.MetaGeo.Initialize();
            Pullenti.Ner.Address.Internal.MetaAddress.Initialize();
            Pullenti.Ner.Address.Internal.MetaStreet.Initialize();
            try 
            {
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Geo.Internal.MiscLocationHelper.Initialize();
                Pullenti.Ner.Geo.Internal.TerrItemToken.Initialize();
                Pullenti.Ner.Geo.Internal.CityItemToken.Initialize();
                Pullenti.Ner.Address.AddressAnalyzer.Initialize();
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new GeoAnalyzer());
        }
    }
}