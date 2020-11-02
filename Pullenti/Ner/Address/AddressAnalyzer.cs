/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Address
{
    /// <summary>
    /// Анализатор адресов
    /// </summary>
    public class AddressAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("ADDRESS")
        /// </summary>
        public const string ANALYZER_NAME = "ADDRESS";
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
                return "Адреса";
            }
        }
        public override string Description
        {
            get
            {
                return "Адреса (улицы, дома ...)";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new AddressAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Address.Internal.MetaAddress.GlobalMeta, Pullenti.Ner.Address.Internal.MetaStreet.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Address.Internal.MetaAddress.AddressImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("address.png"));
                res.Add(Pullenti.Ner.Address.Internal.MetaStreet.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("street.png"));
                return res;
            }
        }
        public override int ProgressWeight
        {
            get
            {
                return 10;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == AddressReferent.OBJ_TYPENAME) 
                return new AddressReferent();
            if (type == StreetReferent.OBJ_TYPENAME) 
                return new StreetReferent();
            return null;
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {Pullenti.Ner.Geo.GeoReferent.OBJ_TYPENAME, "PHONE", "URI"};
            }
        }
        class AddressAnalyzerData : Pullenti.Ner.Core.AnalyzerData
        {
            Pullenti.Ner.Core.AnalyzerData m_Addresses = new Pullenti.Ner.Core.AnalyzerData();
            public Pullenti.Ner.Core.AnalyzerDataWithOntology Streets = new Pullenti.Ner.Core.AnalyzerDataWithOntology();
            public override Pullenti.Ner.Referent RegisterReferent(Pullenti.Ner.Referent referent)
            {
                if (referent is Pullenti.Ner.Address.StreetReferent) 
                {
                    (referent as Pullenti.Ner.Address.StreetReferent).Correct();
                    return Streets.RegisterReferent(referent);
                }
                else 
                    return m_Addresses.RegisterReferent(referent);
            }
            public override ICollection<Pullenti.Ner.Referent> Referents
            {
                get
                {
                    if (Streets.Referents.Count == 0) 
                        return m_Addresses.Referents;
                    else if (m_Addresses.Referents.Count == 0) 
                        return Streets.Referents;
                    List<Pullenti.Ner.Referent> res = new List<Pullenti.Ner.Referent>(Streets.Referents);
                    res.AddRange(m_Addresses.Referents);
                    return res;
                }
                set
                {
                    m_Referents.Clear();
                    if (value != null) 
                        m_Referents.AddRange(value);
                }
            }
        }

        public override Pullenti.Ner.Core.AnalyzerData CreateAnalyzerData()
        {
            return new AddressAnalyzerData();
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            AddressAnalyzerData ad = kit.GetAnalyzerData(this) as AddressAnalyzerData;
            int steps = 1;
            int max = steps;
            int delta = 100000;
            int parts = (((kit.Sofa.Text.Length + delta) - 1)) / delta;
            if (parts == 0) 
                parts = 1;
            max *= parts;
            int cur = 0;
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
                List<Pullenti.Ner.Address.Internal.AddressItemToken> li = Pullenti.Ner.Address.Internal.AddressItemToken.TryParseList(t, ad.Streets.LocalOntology, 20);
                if (li == null) 
                    continue;
                AddressReferent addr = new AddressReferent();
                List<Pullenti.Ner.Address.Internal.AddressItemToken> streets = new List<Pullenti.Ner.Address.Internal.AddressItemToken>();
                int i;
                int j;
                Pullenti.Ner.Address.Internal.AddressItemToken metro = null;
                AddressDetailType detTyp = AddressDetailType.Undefined;
                int detParam = 0;
                List<Pullenti.Ner.Geo.GeoReferent> geos = null;
                bool err = false;
                Pullenti.Ner.Address.Internal.AddressItemToken nearCity = null;
                for (i = 0; i < li.Count; i++) 
                {
                    if ((li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Detail && li[i].DetailType == AddressDetailType.Cross && ((i + 2) < li.Count)) && li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street && li[i + 2].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street) 
                    {
                        detTyp = AddressDetailType.Cross;
                        streets.Add(li[i + 1]);
                        streets.Add(li[i + 2]);
                        li[i + 1].EndToken = li[i + 2].EndToken;
                        li[i].Tag = this;
                        li[i + 1].Tag = this;
                        li.RemoveAt(i + 2);
                        break;
                    }
                    else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street) 
                    {
                        if (((li[i].RefToken != null && !li[i].RefTokenIsGsk)) && streets.Count == 0) 
                        {
                            if (i > 0 && li[i].IsNewlineBefore) 
                                err = true;
                            else if ((i + 1) == li.Count) 
                                err = detTyp == AddressDetailType.Undefined && detParam == 0 && nearCity == null;
                            else if (((i + 1) < li.Count) && li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Number) 
                                err = true;
                            if (err && geos != null) 
                            {
                                for (int ii = i - 1; ii >= 0; ii--) 
                                {
                                    if (li[ii].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Zip || li[ii].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Prefix) 
                                        err = false;
                                }
                            }
                            if (err) 
                                break;
                        }
                        li[i].Tag = this;
                        streets.Add(li[i]);
                        if (((i + 1) < li.Count) && li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street) 
                        {
                        }
                        else 
                            break;
                    }
                    else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City || li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Region) 
                    {
                        if (geos == null) 
                            geos = new List<Pullenti.Ner.Geo.GeoReferent>();
                        geos.Insert(0, li[i].Referent as Pullenti.Ner.Geo.GeoReferent);
                        if (li[i].DetailType != AddressDetailType.Undefined && detTyp == AddressDetailType.Undefined) 
                        {
                            if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City && li[i].DetailType == AddressDetailType.Near && li[i].DetailMeters == 0) 
                                nearCity = li[i];
                            else 
                                detTyp = li[i].DetailType;
                        }
                        if (li[i].DetailMeters > 0 && detParam == 0) 
                            detParam = li[i].DetailMeters;
                    }
                    else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Detail) 
                    {
                        if (li[i].DetailType != AddressDetailType.Undefined && detTyp == AddressDetailType.Undefined) 
                        {
                            if (li[i].DetailType == AddressDetailType.Near && ((i + 1) < li.Count) && li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City) 
                            {
                                nearCity = li[i + 1];
                                li[i].Tag = this;
                                i++;
                            }
                            else 
                            {
                                detTyp = li[i].DetailType;
                                if (li[i].DetailMeters > 0) 
                                    detParam = li[i].DetailMeters;
                            }
                        }
                        li[i].Tag = this;
                    }
                }
                if (i >= li.Count && metro == null && detTyp == AddressDetailType.Undefined) 
                {
                    for (i = 0; i < li.Count; i++) 
                    {
                        bool cit = false;
                        if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City) 
                            cit = true;
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Region) 
                        {
                            foreach (Pullenti.Ner.Slot s in li[i].Referent.Slots) 
                            {
                                if (s.TypeName == Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE) 
                                {
                                    string ss = s.Value as string;
                                    if (ss.Contains("посел") || ss.Contains("сельск") || ss.Contains("почтовое отделение")) 
                                        cit = true;
                                }
                            }
                        }
                        if (cit) 
                        {
                            if (((i + 1) < li.Count) && ((((li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.House || li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Block || li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Plot) || li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Building || li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Corpus) || li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.PostOfficeBox || li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.CSP))) 
                                break;
                            if (((i + 1) < li.Count) && li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Number) 
                            {
                                if (li[i].EndToken.Next.IsComma) 
                                {
                                    if ((li[i].Referent is Pullenti.Ner.Geo.GeoReferent) && !(li[i].Referent as Pullenti.Ner.Geo.GeoReferent).IsBigCity && (li[i].Referent as Pullenti.Ner.Geo.GeoReferent).IsCity) 
                                    {
                                        li[i + 1].Typ = Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.House;
                                        break;
                                    }
                                }
                            }
                            if (li[0].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Zip || li[0].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Prefix) 
                                break;
                            continue;
                        }
                        if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Region) 
                        {
                            if ((li[i].Referent is Pullenti.Ner.Geo.GeoReferent) && (li[i].Referent as Pullenti.Ner.Geo.GeoReferent).Higher != null && (li[i].Referent as Pullenti.Ner.Geo.GeoReferent).Higher.IsCity) 
                            {
                                if (((i + 1) < li.Count) && li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.House) 
                                    break;
                            }
                        }
                    }
                    if (i >= li.Count) 
                        continue;
                }
                if (err) 
                    continue;
                int i0 = i;
                if (i > 0 && li[i - 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.House && li[i - 1].IsDigit) 
                {
                    addr.AddSlot(AddressReferent.ATTR_HOUSE, li[i - 1].Value, false, 0).Tag = li[i - 1];
                    li[i - 1].Tag = this;
                }
                else if ((i > 0 && li[i - 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Kilometer && li[i - 1].IsDigit) && (i < li.Count) && li[i].IsStreetRoad) 
                {
                    addr.AddSlot(AddressReferent.ATTR_KILOMETER, li[i - 1].Value, false, 0).Tag = li[i - 1];
                    li[i - 1].Tag = this;
                }
                else 
                {
                    if (i >= li.Count) 
                        i = -1;
                    for (i = 0; i < li.Count; i++) 
                    {
                        if (li[i].Tag != null) 
                            continue;
                        if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.House) 
                        {
                            if (addr.House != null) 
                                break;
                            if (li[i].Value != null) 
                            {
                                addr.AddSlot(AddressReferent.ATTR_HOUSE, li[i].Value, false, 0).Tag = li[i];
                                if (li[i].HouseType != AddressHouseType.Undefined) 
                                    addr.HouseType = li[i].HouseType;
                            }
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Kilometer && li[i].IsDigit && (((i0 < li.Count) && li[i0].IsStreetRoad))) 
                        {
                            if (addr.Kilometer != null) 
                                break;
                            Pullenti.Ner.Slot s = addr.AddSlot(AddressReferent.ATTR_KILOMETER, li[i].Value, false, 0);
                            if (s != null) 
                                s.Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Plot) 
                        {
                            if (addr.Plot != null) 
                                break;
                            Pullenti.Ner.Slot s = addr.AddSlot(AddressReferent.ATTR_PLOT, li[i].Value, false, 0);
                            if (s != null) 
                                s.Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Box && li[i].IsDigit) 
                        {
                            if (addr.Box != null) 
                                break;
                            Pullenti.Ner.Slot s = addr.AddSlot(AddressReferent.ATTR_BOX, li[i].Value, false, 0);
                            if (s != null) 
                                s.Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Block && li[i].IsDigit) 
                        {
                            if (addr.Block != null) 
                                break;
                            Pullenti.Ner.Slot s = addr.AddSlot(AddressReferent.ATTR_BLOCK, li[i].Value, false, 0);
                            if (s != null) 
                                s.Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Corpus) 
                        {
                            if (addr.Corpus != null) 
                                break;
                            if (li[i].Value != null) 
                            {
                                Pullenti.Ner.Slot s = addr.AddSlot(AddressReferent.ATTR_CORPUS, li[i].Value, false, 0);
                                if (s != null) 
                                    s.Tag = li[i];
                            }
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Building) 
                        {
                            if (addr.Building != null) 
                                break;
                            if (li[i].Value != null) 
                            {
                                Pullenti.Ner.Slot s = addr.AddSlot(AddressReferent.ATTR_BUILDING, li[i].Value, false, 0);
                                if (s != null) 
                                    s.Tag = li[i];
                                if (li[i].BuildingType != AddressBuildingType.Undefined) 
                                    addr.BuildingType = li[i].BuildingType;
                            }
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Floor && li[i].IsDigit) 
                        {
                            if (addr.Floor != null) 
                                break;
                            Pullenti.Ner.Slot s = addr.AddSlot(AddressReferent.ATTR_FLOOR, li[i].Value, false, 0);
                            if (s != null) 
                                s.Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Potch && li[i].IsDigit) 
                        {
                            if (addr.Potch != null) 
                                break;
                            Pullenti.Ner.Slot s = addr.AddSlot(AddressReferent.ATTR_PORCH, li[i].Value, false, 0);
                            if (s != null) 
                                s.Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Flat) 
                        {
                            if (addr.Flat != null) 
                                break;
                            if (li[i].Value != null) 
                                addr.AddSlot(AddressReferent.ATTR_FLAT, li[i].Value, false, 0).Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Office && li[i].IsDigit) 
                        {
                            if (addr.Office != null) 
                                break;
                            Pullenti.Ner.Slot s = addr.AddSlot(AddressReferent.ATTR_OFFICE, li[i].Value, false, 0);
                            if (s != null) 
                                s.Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.CorpusOrFlat && ((li[i].IsDigit || li[i].Value == null))) 
                        {
                            for (j = i + 1; j < li.Count; j++) 
                            {
                                if (li[j].IsDigit) 
                                {
                                    if (((li[j].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Flat || li[j].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.CorpusOrFlat || li[j].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Office) || li[j].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Floor || li[j].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Potch) || li[j].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.PostOfficeBox || li[j].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Building) 
                                        break;
                                }
                            }
                            if (li[i].Value != null) 
                            {
                                if ((j < li.Count) && addr.Corpus == null) 
                                    addr.AddSlot(AddressReferent.ATTR_CORPUS, li[i].Value, false, 0).Tag = li[i];
                                else if (addr.Corpus != null) 
                                    addr.AddSlot(AddressReferent.ATTR_FLAT, li[i].Value, false, 0).Tag = li[i];
                                else 
                                    addr.AddSlot(AddressReferent.ATTR_CORPUSORFLAT, li[i].Value, false, 0).Tag = li[i];
                            }
                            li[i].Tag = this;
                        }
                        else if ((!li[i].IsNewlineBefore && li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Number && li[i].IsDigit) && li[i - 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street) 
                        {
                            int v = 0;
                            if (!int.TryParse(li[i].Value, out v)) 
                            {
                                if (!int.TryParse(li[i].Value.Substring(0, li[i].Value.Length - 1), out v)) 
                                {
                                    if (!li[i].Value.Contains("/")) 
                                        break;
                                }
                            }
                            if (v > 400) 
                                break;
                            addr.AddSlot(AddressReferent.ATTR_HOUSE, li[i].Value, false, 0).Tag = li[i];
                            li[i].Tag = this;
                            if (((i + 1) < li.Count) && ((li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Number || li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Flat)) && !li[i + 1].IsNewlineBefore) 
                            {
                                if (!int.TryParse(li[i + 1].Value, out v)) 
                                    break;
                                if (v > 500) 
                                    break;
                                i++;
                                if ((((i + 1) < li.Count) && li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Number && !li[i + 1].IsNewlineBefore) && (v < 5)) 
                                {
                                    if (int.TryParse(li[i + 1].Value, out v)) 
                                    {
                                        if (v < 500) 
                                        {
                                            addr.AddSlot(AddressReferent.ATTR_CORPUS, li[i].Value, false, 0).Tag = li[i];
                                            li[i].Tag = this;
                                            i++;
                                        }
                                    }
                                }
                                addr.AddSlot(AddressReferent.ATTR_FLAT, li[i].Value, false, 0).Tag = li[i];
                                li[i].Tag = this;
                            }
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City) 
                        {
                            if (geos == null) 
                                geos = new List<Pullenti.Ner.Geo.GeoReferent>();
                            if (li[i].IsNewlineBefore) 
                            {
                                if (geos.Count > 0) 
                                {
                                    if ((i > 0 && li[i - 1].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City && li[i - 1].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Region) && li[i - 1].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Zip && li[i - 1].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Prefix) 
                                        break;
                                }
                                if (((i + 1) < li.Count) && li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street && i > i0) 
                                    break;
                            }
                            if (li[i].DetailType == AddressDetailType.Near && li[i].DetailMeters == 0) 
                            {
                                nearCity = li[i];
                                li[i].Tag = this;
                                continue;
                            }
                            int ii;
                            for (ii = 0; ii < geos.Count; ii++) 
                            {
                                if (geos[ii].IsCity) 
                                    break;
                            }
                            if (ii >= geos.Count) 
                                geos.Add(li[i].Referent as Pullenti.Ner.Geo.GeoReferent);
                            else if (i > 0 && li[i].IsNewlineBefore && i > i0) 
                            {
                                int jj;
                                for (jj = 0; jj < i; jj++) 
                                {
                                    if ((li[jj].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Prefix && li[jj].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Zip && li[jj].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Region) && li[jj].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Country && li[jj].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City) 
                                        break;
                                }
                                if (jj < i) 
                                    break;
                            }
                            if (li[i].DetailType != AddressDetailType.Undefined && detTyp == AddressDetailType.Undefined) 
                            {
                                detTyp = li[i].DetailType;
                                if (li[i].DetailMeters > 0) 
                                    detParam = li[i].DetailMeters;
                            }
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.PostOfficeBox) 
                        {
                            if (addr.PostOfficeBox != null) 
                                break;
                            addr.AddSlot(AddressReferent.ATTR_POSTOFFICEBOX, li[i].Value, false, 0).Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.CSP) 
                        {
                            if (addr.CSP != null) 
                                break;
                            addr.AddSlot(AddressReferent.ATTR_CSP, li[i].Value, false, 0).Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street) 
                        {
                            if (streets.Count > 1) 
                                break;
                            if (streets.Count > 0) 
                            {
                                if (li[i].IsNewlineBefore) 
                                    break;
                                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(li[i].BeginToken)) 
                                    break;
                            }
                            if (li[i].RefToken == null && i > 0 && li[i - 1].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street) 
                                break;
                            streets.Add(li[i]);
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Detail) 
                        {
                            if ((i + 1) == li.Count && li[i].DetailType == AddressDetailType.Near) 
                                break;
                            if (li[i].DetailType == AddressDetailType.Near && ((i + 1) < li.Count) && li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City) 
                            {
                                nearCity = li[i + 1];
                                li[i].Tag = this;
                                i++;
                            }
                            else if (li[i].DetailType != AddressDetailType.Undefined && detTyp == AddressDetailType.Undefined) 
                            {
                                detTyp = li[i].DetailType;
                                if (li[i].DetailMeters > 0) 
                                    detParam = li[i].DetailMeters;
                            }
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.BusinessCenter && li[i].RefToken != null) 
                        {
                            addr.AddExtReferent(li[i].RefToken);
                            addr.AddSlot(AddressReferent.ATTR_MISC, li[i].RefToken.Referent, false, 0).Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (i > i0) 
                            break;
                    }
                }
                List<string> typs = new List<string>();
                foreach (Pullenti.Ner.Slot s in addr.Slots) 
                {
                    if (!typs.Contains(s.TypeName)) 
                        typs.Add(s.TypeName);
                }
                if (streets.Count == 1 && !streets[0].IsDoubt && streets[0].RefToken == null) 
                {
                }
                else if (li.Count > 2 && li[0].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Zip && ((li[1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Country || li[1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Region))) 
                {
                }
                else if ((typs.Count + streets.Count) < 2) 
                {
                    if (typs.Count > 0) 
                    {
                        if ((((typs[0] != AddressReferent.ATTR_STREET && typs[0] != AddressReferent.ATTR_POSTOFFICEBOX && metro == null) && typs[0] != AddressReferent.ATTR_HOUSE && typs[0] != AddressReferent.ATTR_CORPUS) && typs[0] != AddressReferent.ATTR_BUILDING && typs[0] != AddressReferent.ATTR_PLOT) && typs[0] != AddressReferent.ATTR_DETAIL && detTyp == AddressDetailType.Undefined) 
                            continue;
                    }
                    else if (streets.Count == 0 && detTyp == AddressDetailType.Undefined) 
                    {
                        if (li[i - 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City && i > 2 && li[i - 2].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Zip) 
                        {
                        }
                        else 
                            continue;
                    }
                    else if ((i == li.Count && streets.Count == 1 && (streets[0].Referent is StreetReferent)) && streets[0].Referent.FindSlot(StreetReferent.ATTR_TYP, "квартал", true) != null) 
                        continue;
                    if (geos == null) 
                    {
                        bool hasGeo = false;
                        for (Pullenti.Ner.Token tt = li[0].BeginToken.Previous; tt != null; tt = tt.Previous) 
                        {
                            if (tt.Morph.Class.IsPreposition || tt.IsComma) 
                                continue;
                            Pullenti.Ner.Referent r = tt.GetReferent();
                            if (r == null) 
                                break;
                            if (r.TypeName == "DATE" || r.TypeName == "DATERANGE") 
                                continue;
                            if (r is Pullenti.Ner.Geo.GeoReferent) 
                            {
                                if (!(r as Pullenti.Ner.Geo.GeoReferent).IsState) 
                                {
                                    if (geos == null) 
                                        geos = new List<Pullenti.Ner.Geo.GeoReferent>();
                                    geos.Add(r as Pullenti.Ner.Geo.GeoReferent);
                                    hasGeo = true;
                                }
                            }
                            break;
                        }
                        if (!hasGeo) 
                            continue;
                    }
                }
                for (i = 0; i < li.Count; i++) 
                {
                    if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Prefix) 
                        li[i].Tag = this;
                    else if (li[i].Tag == null) 
                    {
                        if (li[i].IsNewlineBefore && i > i0) 
                        {
                            bool stop = false;
                            for (j = i + 1; j < li.Count; j++) 
                            {
                                if (li[j].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street) 
                                {
                                    stop = true;
                                    break;
                                }
                            }
                            if (stop) 
                                break;
                        }
                        if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Country || li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Region || li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City) 
                        {
                            if (geos == null) 
                                geos = new List<Pullenti.Ner.Geo.GeoReferent>();
                            if (!geos.Contains(li[i].Referent as Pullenti.Ner.Geo.GeoReferent)) 
                                geos.Add(li[i].Referent as Pullenti.Ner.Geo.GeoReferent);
                            if (li[i].Typ != Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Country) 
                            {
                                if (li[i].DetailType != AddressDetailType.Undefined && addr.Detail == AddressDetailType.Undefined) 
                                {
                                    addr.AddSlot(AddressReferent.ATTR_DETAIL, li[i].DetailType.ToString().ToUpper(), false, 0).Tag = li[i];
                                    if (li[i].DetailMeters > 0) 
                                        addr.AddSlot(AddressReferent.ATTR_DETAILPARAM, string.Format("{0}м", li[i].DetailMeters), false, 0);
                                }
                            }
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Zip) 
                        {
                            if (addr.Zip != null) 
                                break;
                            addr.AddSlot(AddressReferent.ATTR_ZIP, li[i].Value, false, 0).Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.PostOfficeBox) 
                        {
                            if (addr.PostOfficeBox != null) 
                                break;
                            addr.AddSlot(AddressReferent.ATTR_POSTOFFICEBOX, li[i].Value, false, 0).Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.CSP) 
                        {
                            if (addr.CSP != null) 
                                break;
                            addr.AddSlot(AddressReferent.ATTR_CSP, li[i].Value, false, 0).Tag = li[i];
                            li[i].Tag = this;
                        }
                        else if (li[i].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Number && li[i].IsDigit && li[i].Value.Length == 6) 
                        {
                            if (((i + 1) < li.Count) && li[i + 1].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.City) 
                            {
                                if (addr.Zip != null) 
                                    break;
                                addr.AddSlot(AddressReferent.ATTR_ZIP, li[i].Value, false, 0).Tag = li[i];
                                li[i].Tag = this;
                            }
                        }
                        else 
                            break;
                    }
                }
                Pullenti.Ner.Token t0 = null;
                Pullenti.Ner.Token t1 = null;
                for (i = 0; i < li.Count; i++) 
                {
                    if (li[i].Tag != null) 
                    {
                        t0 = li[i].BeginToken;
                        break;
                    }
                }
                for (i = li.Count - 1; i >= 0; i--) 
                {
                    if (li[i].Tag != null) 
                    {
                        t1 = li[i].EndToken;
                        break;
                    }
                }
                if (t0 == null || t1 == null) 
                    continue;
                if (addr.Slots.Count == 0) 
                {
                    int pureStreets = 0;
                    int gsks = 0;
                    foreach (Pullenti.Ner.Address.Internal.AddressItemToken s in streets) 
                    {
                        if (s.RefToken != null && s.RefTokenIsGsk) 
                            gsks++;
                        else if (s.RefToken == null) 
                            pureStreets++;
                    }
                    if ((pureStreets + gsks) == 0 && streets.Count > 0) 
                    {
                        if (((detTyp != AddressDetailType.Undefined || nearCity != null)) && geos != null) 
                        {
                        }
                        else 
                            addr = null;
                    }
                    else if (streets.Count < 2) 
                    {
                        if ((streets.Count == 1 && geos != null && geos.Count > 0) && ((streets[0].RefToken == null || streets[0].RefTokenIsGsk))) 
                        {
                        }
                        else if (detTyp != AddressDetailType.Undefined && geos != null && streets.Count == 0) 
                        {
                        }
                        else 
                            addr = null;
                    }
                }
                if (addr != null && detTyp != AddressDetailType.Undefined) 
                {
                    addr.Detail = detTyp;
                    if (detParam > 0) 
                        addr.AddSlot(AddressReferent.ATTR_DETAILPARAM, string.Format("{0}м", detParam), false, 0);
                }
                if (geos == null && streets.Count > 0 && !streets[0].IsStreetRoad) 
                {
                    int cou = 0;
                    for (Pullenti.Ner.Token tt = t0.Previous; tt != null && (cou < 200); tt = tt.Previous,cou++) 
                    {
                        if (tt.IsNewlineAfter) 
                            cou += 10;
                        Pullenti.Ner.Referent r = tt.GetReferent();
                        if ((r is Pullenti.Ner.Geo.GeoReferent) && !(r as Pullenti.Ner.Geo.GeoReferent).IsState) 
                        {
                            geos = new List<Pullenti.Ner.Geo.GeoReferent>();
                            geos.Add(r as Pullenti.Ner.Geo.GeoReferent);
                            break;
                        }
                        if (r is StreetReferent) 
                        {
                            List<Pullenti.Ner.Geo.GeoReferent> ggg = (r as StreetReferent).Geos;
                            if (ggg.Count > 0) 
                            {
                                geos = new List<Pullenti.Ner.Geo.GeoReferent>(ggg);
                                break;
                            }
                        }
                        if (r is AddressReferent) 
                        {
                            List<Pullenti.Ner.Geo.GeoReferent> ggg = (r as AddressReferent).Geos;
                            if (ggg.Count > 0) 
                            {
                                geos = new List<Pullenti.Ner.Geo.GeoReferent>(ggg);
                                break;
                            }
                        }
                    }
                }
                Pullenti.Ner.ReferentToken terrRef = null;
                Pullenti.Ner.ReferentToken terRef0 = null;
                Pullenti.Ner.ReferentToken rt;
                StreetReferent sr0 = null;
                for (int ii = 0; ii < streets.Count; ii++) 
                {
                    Pullenti.Ner.Address.Internal.AddressItemToken s = streets[ii];
                    StreetReferent sr = s.Referent as StreetReferent;
                    if ((sr == null && s.Referent != null && s.Referent.TypeName == "ORGANIZATION") && s.RefToken != null) 
                    {
                        if (s.RefTokenIsGsk && addr == null) 
                            addr = new AddressReferent();
                        if (addr != null) 
                        {
                            addr.AddReferent(s.Referent);
                            addr.AddExtReferent(s.RefToken);
                            terRef0 = s.RefToken;
                            if (geos == null || geos.Count == 0) 
                                continue;
                            int jj = li.IndexOf(s);
                            Pullenti.Ner.Geo.GeoReferent geo0 = null;
                            if (jj > 0 && (li[jj - 1].Referent is Pullenti.Ner.Geo.GeoReferent) && ((li[jj - 1] != nearCity || (li[jj - 1].Referent as Pullenti.Ner.Geo.GeoReferent).Higher != null))) 
                                geo0 = li[jj - 1].Referent as Pullenti.Ner.Geo.GeoReferent;
                            else if (jj > 1 && (li[jj - 2].Referent is Pullenti.Ner.Geo.GeoReferent)) 
                                geo0 = li[jj - 2].Referent as Pullenti.Ner.Geo.GeoReferent;
                            else if (nearCity != null) 
                                geo0 = nearCity.Referent as Pullenti.Ner.Geo.GeoReferent;
                            if (geo0 != null && ((geo0.IsRegion || geo0.IsCity))) 
                            {
                                Pullenti.Ner.Geo.GeoReferent geo = new Pullenti.Ner.Geo.GeoReferent();
                                geo.AddTypTer(kit.BaseLanguage);
                                if (geo0.IsRegion) 
                                    geo.AddTyp((kit.BaseLanguage.IsUa ? "населений пункт" : "населенный пункт"));
                                geo.AddOrgReferent(s.Referent);
                                if (nearCity != null && geo0 == nearCity.Referent) 
                                    geo.Higher = geo0.Higher;
                                else 
                                    geo.Higher = geo0;
                                Pullenti.Ner.Slot sl = addr.FindSlot(AddressReferent.ATTR_GEO, geo0, true);
                                if (sl != null) 
                                    addr.Slots.Remove(sl);
                                if ((((sl = addr.FindSlot(AddressReferent.ATTR_STREET, s.Referent, true)))) != null) 
                                    addr.Slots.Remove(sl);
                                geos.Remove(geo0);
                                if (nearCity != null && geos.Contains(nearCity.Referent as Pullenti.Ner.Geo.GeoReferent)) 
                                    geos.Remove(nearCity.Referent as Pullenti.Ner.Geo.GeoReferent);
                                geos.Add(geo);
                                streets.RemoveAt(ii);
                                Pullenti.Ner.ReferentToken rtt = new Pullenti.Ner.ReferentToken(geo, s.RefToken.BeginToken, s.RefToken.EndToken);
                                rtt.Data = kit.GetAnalyzerDataByAnalyzerName("GEO");
                                if (nearCity != null && (nearCity.Referent is Pullenti.Ner.Geo.GeoReferent)) 
                                {
                                    geo.AddSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_REF, nearCity.Referent, false, 0);
                                    if (nearCity.EndChar > rtt.EndChar) 
                                        rtt.EndToken = nearCity.EndToken;
                                    if (nearCity.BeginChar < rtt.BeginChar) 
                                        rtt.BeginToken = nearCity.BeginToken;
                                    if ((nearCity.Referent as Pullenti.Ner.Geo.GeoReferent).Higher == null && geo0 != nearCity.Referent) 
                                        (nearCity.Referent as Pullenti.Ner.Geo.GeoReferent).Higher = geo0;
                                }
                                addr.AddExtReferent(rtt);
                                terrRef = rtt;
                                ii--;
                                continue;
                            }
                            if ((geo0 != null && geo0.IsTerritory && jj > 0) && li[jj - 1].Referent == geo0) 
                            {
                                geo0.AddSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_REF, s.Referent, false, 0);
                                geo0.AddExtReferent(s.RefToken);
                                Pullenti.Ner.ReferentToken rtt = new Pullenti.Ner.ReferentToken(geo0, li[jj - 1].BeginToken, s.RefToken.EndToken);
                                rtt.Data = kit.GetAnalyzerDataByAnalyzerName("GEO");
                                addr.AddExtReferent(rtt);
                                terrRef = rtt;
                                streets.RemoveAt(ii);
                                ii--;
                                continue;
                            }
                            foreach (Pullenti.Ner.Geo.GeoReferent gr in geos) 
                            {
                                if (s.Referent.FindSlot("GEO", gr, true) != null) 
                                {
                                    geos.Remove(gr);
                                    Pullenti.Ner.Slot sl = addr.FindSlot(AddressReferent.ATTR_GEO, gr, true);
                                    if (sl != null) 
                                        addr.Slots.Remove(sl);
                                    break;
                                }
                            }
                        }
                        continue;
                    }
                    if (sr != null && terrRef != null) 
                    {
                        sr.AddSlot(StreetReferent.ATTR_GEO, terrRef.Referent, false, 0);
                        sr.AddExtReferent(terrRef);
                        if (geos != null && geos.Contains(terrRef.Referent as Pullenti.Ner.Geo.GeoReferent)) 
                            geos.Remove(terrRef.Referent as Pullenti.Ner.Geo.GeoReferent);
                    }
                    if (geos != null && sr != null && sr.Geos.Count == 0) 
                    {
                        foreach (Pullenti.Ner.Geo.GeoReferent gr in geos) 
                        {
                            if (gr.IsCity || ((gr.Higher != null && gr.Higher.IsCity))) 
                            {
                                sr.AddSlot(StreetReferent.ATTR_GEO, gr, false, 0);
                                if (li[0].Referent == gr) 
                                    streets[0].BeginToken = li[0].BeginToken;
                                for (int jj = ii + 1; jj < streets.Count; jj++) 
                                {
                                    if (streets[jj].Referent is StreetReferent) 
                                        streets[jj].Referent.AddSlot(StreetReferent.ATTR_GEO, gr, false, 0);
                                }
                                geos.Remove(gr);
                                break;
                            }
                        }
                    }
                    if (sr != null && sr.Geos.Count == 0) 
                    {
                        if (sr0 != null) 
                        {
                            foreach (Pullenti.Ner.Geo.GeoReferent g in sr0.Geos) 
                            {
                                sr.AddSlot(StreetReferent.ATTR_GEO, g, false, 0);
                            }
                        }
                        sr0 = sr;
                    }
                    if (s.Referent != null && s.Referent.FindSlot(StreetReferent.ATTR_NAME, "НЕТ", true) != null) 
                    {
                        foreach (Pullenti.Ner.Slot ss in s.Referent.Slots) 
                        {
                            if (ss.TypeName == StreetReferent.ATTR_GEO) 
                                addr.AddReferent(ss.Value as Pullenti.Ner.Referent);
                        }
                    }
                    else 
                    {
                        s.Referent = ad.RegisterReferent(s.Referent);
                        if (addr != null) 
                            addr.AddReferent(s.Referent);
                        t = (rt = new Pullenti.Ner.ReferentToken(s.Referent, s.BeginToken, s.EndToken));
                        kit.EmbedToken(rt);
                        if (s.BeginToken == t0) 
                            t0 = rt;
                        if (s.EndToken == t1) 
                            t1 = rt;
                    }
                }
                if (addr != null) 
                {
                    bool ok = false;
                    foreach (Pullenti.Ner.Slot s in addr.Slots) 
                    {
                        if (s.TypeName != AddressReferent.ATTR_DETAIL) 
                            ok = true;
                    }
                    if (!ok) 
                        addr = null;
                }
                if (addr == null) 
                {
                    if (terrRef != null) 
                    {
                        terrRef.Referent.AddExtReferent(terRef0);
                        terrRef.Referent = ad.RegisterReferent(terrRef.Referent);
                        kit.EmbedToken(terrRef);
                        t = terrRef;
                        continue;
                    }
                    continue;
                }
                if (geos != null) 
                {
                    if ((geos.Count == 1 && geos[0].IsRegion && streets.Count == 1) && streets[0].RefToken != null) 
                    {
                    }
                    if (streets.Count == 1 && streets[0].Referent != null) 
                    {
                        foreach (Pullenti.Ner.Slot s in streets[0].Referent.Slots) 
                        {
                            if (s.TypeName == StreetReferent.ATTR_GEO && (s.Value is Pullenti.Ner.Geo.GeoReferent)) 
                            {
                                int k = 0;
                                for (Pullenti.Ner.Geo.GeoReferent gg = s.Value as Pullenti.Ner.Geo.GeoReferent; gg != null && (k < 5); gg = gg.ParentReferent as Pullenti.Ner.Geo.GeoReferent,k++) 
                                {
                                    for (int ii = geos.Count - 1; ii >= 0; ii--) 
                                    {
                                        if (geos[ii] == gg) 
                                        {
                                            geos.RemoveAt(ii);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    while (geos.Count >= 2) 
                    {
                        if (geos[1].Higher == null && Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(geos[0], geos[1])) 
                        {
                            geos[1].Higher = geos[0];
                            geos.RemoveAt(0);
                        }
                        else if (geos[0].Higher == null && Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(geos[1], geos[0])) 
                        {
                            geos[0].Higher = geos[1];
                            geos.RemoveAt(1);
                        }
                        else if (geos[1].Higher != null && geos[1].Higher.Higher == null && Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(geos[0], geos[1].Higher)) 
                        {
                            geos[1].Higher.Higher = geos[0];
                            geos.RemoveAt(0);
                        }
                        else if (geos[0].Higher != null && geos[0].Higher.Higher == null && Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(geos[1], geos[0].Higher)) 
                        {
                            geos[0].Higher.Higher = geos[1];
                            geos.RemoveAt(1);
                        }
                        else 
                            break;
                    }
                    foreach (Pullenti.Ner.Geo.GeoReferent g in geos) 
                    {
                        addr.AddReferent(g);
                    }
                }
                bool ok1 = false;
                foreach (Pullenti.Ner.Slot s in addr.Slots) 
                {
                    if (s.TypeName != AddressReferent.ATTR_STREET) 
                    {
                        ok1 = true;
                        break;
                    }
                }
                if (!ok1) 
                    continue;
                if (addr.House != null && addr.Corpus == null && addr.FindSlot(AddressReferent.ATTR_STREET, null, true) == null) 
                {
                    if (geos != null && geos.Count > 0 && geos[0].FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME, "ЗЕЛЕНОГРАД", true) != null) 
                    {
                        addr.Corpus = addr.House;
                        addr.House = null;
                    }
                }
                rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(addr), t0, t1);
                kit.EmbedToken(rt);
                t = rt;
                if ((t.Next != null && ((t.Next.IsComma || t.Next.IsChar(';'))) && (t.Next.WhitespacesAfterCount < 2)) && (t.Next.Next is Pullenti.Ner.NumberToken)) 
                {
                    Pullenti.Ner.Address.Internal.AddressItemToken last = null;
                    foreach (Pullenti.Ner.Address.Internal.AddressItemToken ll in li) 
                    {
                        if (ll.Tag != null) 
                            last = ll;
                    }
                    string attrName = null;
                    if (last == null) 
                        continue;
                    if (last.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.House) 
                        attrName = AddressReferent.ATTR_HOUSE;
                    else if (last.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Corpus) 
                        attrName = AddressReferent.ATTR_CORPUS;
                    else if (last.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Building) 
                        attrName = AddressReferent.ATTR_BUILDING;
                    else if (last.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Flat) 
                        attrName = AddressReferent.ATTR_FLAT;
                    else if (last.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Plot) 
                        attrName = AddressReferent.ATTR_PLOT;
                    else if (last.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Box) 
                        attrName = AddressReferent.ATTR_BOX;
                    else if (last.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Potch) 
                        attrName = AddressReferent.ATTR_PORCH;
                    else if (last.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Block) 
                        attrName = AddressReferent.ATTR_BLOCK;
                    else if (last.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Office) 
                        attrName = AddressReferent.ATTR_OFFICE;
                    if (attrName != null) 
                    {
                        for (t = t.Next.Next; t != null; t = t.Next) 
                        {
                            if (!(t is Pullenti.Ner.NumberToken)) 
                                break;
                            AddressReferent addr1 = addr.Clone() as AddressReferent;
                            addr1.Occurrence.Clear();
                            addr1.AddSlot(attrName, (t as Pullenti.Ner.NumberToken).Value.ToString(), true, 0);
                            rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(addr1), t, t);
                            kit.EmbedToken(rt);
                            t = rt;
                            if ((t.Next != null && ((t.Next.IsComma || t.Next.IsChar(';'))) && (t.Next.WhitespacesAfterCount < 2)) && (t.Next.Next is Pullenti.Ner.NumberToken)) 
                            {
                            }
                            else 
                                break;
                        }
                    }
                }
            }
            List<StreetReferent> sli = new List<StreetReferent>();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = (t == null ? null : t.Next)) 
            {
                StreetReferent sr = t.GetReferent() as StreetReferent;
                if (sr == null) 
                    continue;
                if (t.Next == null || !t.Next.IsCommaAnd) 
                    continue;
                sli.Clear();
                sli.Add(sr);
                for (t = t.Next; t != null; t = t.Next) 
                {
                    if (t.IsCommaAnd) 
                        continue;
                    if ((((sr = t.GetReferent() as StreetReferent))) != null) 
                    {
                        sli.Add(sr);
                        continue;
                    }
                    AddressReferent adr = t.GetReferent() as AddressReferent;
                    if (adr == null) 
                        break;
                    if (adr.Streets.Count == 0) 
                        break;
                    foreach (Pullenti.Ner.Referent ss in adr.Streets) 
                    {
                        if (ss is StreetReferent) 
                            sli.Add(ss as StreetReferent);
                    }
                }
                if (sli.Count < 2) 
                    continue;
                bool ok = true;
                Pullenti.Ner.Geo.GeoReferent hi = null;
                foreach (StreetReferent s in sli) 
                {
                    if (s.Geos.Count == 0) 
                        continue;
                    else if (s.Geos.Count == 1) 
                    {
                        if (hi == null || hi == s.Geos[0]) 
                            hi = s.Geos[0];
                        else 
                        {
                            ok = false;
                            break;
                        }
                    }
                    else 
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok && hi != null) 
                {
                    foreach (StreetReferent s in sli) 
                    {
                        if (s.Geos.Count == 0) 
                            s.AddSlot(StreetReferent.ATTR_GEO, hi, false, 0);
                    }
                }
            }
            foreach (Pullenti.Ner.Referent a in ad.Referents) 
            {
                if (a is AddressReferent) 
                    (a as AddressReferent).Correct();
            }
        }
        public override Pullenti.Ner.ReferentToken ProcessOntologyItem(Pullenti.Ner.Token begin)
        {
            List<Pullenti.Ner.Address.Internal.StreetItemToken> li = Pullenti.Ner.Address.Internal.StreetItemToken.TryParseList(begin, null, 10);
            if (li == null || (li.Count < 2)) 
                return null;
            Pullenti.Ner.Address.Internal.AddressItemToken rt = Pullenti.Ner.Address.Internal.StreetDefineHelper.TryParseStreet(li, true, false);
            if (rt == null) 
                return null;
            StreetReferent street = rt.Referent as StreetReferent;
            for (Pullenti.Ner.Token t = rt.EndToken.Next; t != null; t = t.Next) 
            {
                if (!t.IsChar(';')) 
                    continue;
                t = t.Next;
                if (t == null) 
                    break;
                li = Pullenti.Ner.Address.Internal.StreetItemToken.TryParseList(begin, null, 10);
                Pullenti.Ner.Address.Internal.AddressItemToken rt1 = Pullenti.Ner.Address.Internal.StreetDefineHelper.TryParseStreet(li, true, false);
                if (rt1 != null) 
                {
                    t = (rt.EndToken = rt1.EndToken);
                    street.MergeSlots(rt1.Referent, true);
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
                            street.AddSlot(StreetReferent.ATTR_NAME, Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(str), false, 0);
                        t = (rt.EndToken = tt);
                    }
                }
            }
            return new Pullenti.Ner.ReferentToken(street, rt.BeginToken, rt.EndToken);
        }
        static bool m_Initialized = false;
        public static void Initialize()
        {
            if (m_Initialized) 
                return;
            m_Initialized = true;
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
            try 
            {
                Pullenti.Ner.Address.Internal.AddressItemToken.Initialize();
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new AddressAnalyzer());
        }
    }
}