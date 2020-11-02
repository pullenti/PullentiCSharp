/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Address
{
    /// <summary>
    /// Сущность, представляющая адрес
    /// </summary>
    public class AddressReferent : Pullenti.Ner.Referent
    {
        public AddressReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Address.Internal.MetaAddress.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("ADDRESS")
        /// </summary>
        public const string OBJ_TYPENAME = "ADDRESS";
        /// <summary>
        /// Имя атрибута - улица
        /// </summary>
        public const string ATTR_STREET = "STREET";
        /// <summary>
        /// Имя атрибута - дом
        /// </summary>
        public const string ATTR_HOUSE = "HOUSE";
        /// <summary>
        /// Имя атрибута - тип дома
        /// </summary>
        public const string ATTR_HOUSETYPE = "HOUSETYPE";
        /// <summary>
        /// Имя атрибута - корпус
        /// </summary>
        public const string ATTR_CORPUS = "CORPUS";
        /// <summary>
        /// Имя атрибута - строение
        /// </summary>
        public const string ATTR_BUILDING = "BUILDING";
        /// <summary>
        /// Имя атрибута - тип строения
        /// </summary>
        public const string ATTR_BUILDINGTYPE = "BUILDINGTYPE";
        /// <summary>
        /// Имя атрибута - корпус или квартира (когда неясно)
        /// </summary>
        public const string ATTR_CORPUSORFLAT = "CORPUSORFLAT";
        /// <summary>
        /// Имя атрибута - подъезд
        /// </summary>
        public const string ATTR_PORCH = "PORCH";
        /// <summary>
        /// Имя атрибута - этаж
        /// </summary>
        public const string ATTR_FLOOR = "FLOOR";
        /// <summary>
        /// Имя атрибута - офис
        /// </summary>
        public const string ATTR_OFFICE = "OFFICE";
        /// <summary>
        /// Имя атрибута - квартира
        /// </summary>
        public const string ATTR_FLAT = "FLAT";
        /// <summary>
        /// Имя атрибута - километр
        /// </summary>
        public const string ATTR_KILOMETER = "KILOMETER";
        /// <summary>
        /// Имя атрибута - участок
        /// </summary>
        public const string ATTR_PLOT = "PLOT";
        /// <summary>
        /// Имя атрибута - блок (ряд)
        /// </summary>
        public const string ATTR_BLOCK = "BLOCK";
        /// <summary>
        /// Имя атрибута - бокс (гараж)
        /// </summary>
        public const string ATTR_BOX = "BOX";
        /// <summary>
        /// Имя атрибута - географический объект (ближайший в иерархии)
        /// </summary>
        public const string ATTR_GEO = "GEO";
        /// <summary>
        /// Имя атрибута - почтовый индекс
        /// </summary>
        public const string ATTR_ZIP = "ZIP";
        /// <summary>
        /// Имя атрибута - почтовый ящик
        /// </summary>
        public const string ATTR_POSTOFFICEBOX = "POSTOFFICEBOX";
        /// <summary>
        /// Имя атрибута - ГСП
        /// </summary>
        public const string ATTR_CSP = "CSP";
        /// <summary>
        /// Имя атрибута - станция метро
        /// </summary>
        public const string ATTR_METRO = "METRO";
        /// <summary>
        /// Имя атрибута - дополнительная информация
        /// </summary>
        public const string ATTR_DETAIL = "DETAIL";
        /// <summary>
        /// Имя атрибута - параметр дополнительной информации
        /// </summary>
        public const string ATTR_DETAILPARAM = "DETAILPARAM";
        /// <summary>
        /// Имя атрибута - разное
        /// </summary>
        public const string ATTR_MISC = "MISC";
        /// <summary>
        /// Имя атрибута - код ФИАС (определяется анализатором FiasAnalyzer)
        /// </summary>
        public const string ATTR_FIAS = "FIAS";
        public const string ATTR_BTI = "BTI";
        /// <summary>
        /// Улица (кстати, их может быть несколько)
        /// </summary>
        public List<Pullenti.Ner.Referent> Streets
        {
            get
            {
                List<Pullenti.Ner.Referent> res = new List<Pullenti.Ner.Referent>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_STREET && (s.Value is Pullenti.Ner.Referent)) 
                        res.Add(s.Value as Pullenti.Ner.Referent);
                }
                return res;
            }
        }
        /// <summary>
        /// Дом
        /// </summary>
        public string House
        {
            get
            {
                return this.GetStringValue(ATTR_HOUSE);
            }
            set
            {
                this.AddSlot(ATTR_HOUSE, value, true, 0);
            }
        }
        /// <summary>
        /// Тип дома
        /// </summary>
        public AddressHouseType HouseType
        {
            get
            {
                string str = this.GetStringValue(ATTR_HOUSETYPE);
                if (string.IsNullOrEmpty(str)) 
                    return AddressHouseType.House;
                try 
                {
                    return (AddressHouseType)Enum.Parse(typeof(AddressHouseType), str, true);
                }
                catch(Exception ex351) 
                {
                    return AddressHouseType.House;
                }
            }
            set
            {
                this.AddSlot(ATTR_HOUSETYPE, value.ToString().ToUpper(), true, 0);
            }
        }
        /// <summary>
        /// Строение
        /// </summary>
        public string Building
        {
            get
            {
                return this.GetStringValue(ATTR_BUILDING);
            }
            set
            {
                this.AddSlot(ATTR_BUILDING, value, true, 0);
            }
        }
        /// <summary>
        /// Тип строения
        /// </summary>
        public AddressBuildingType BuildingType
        {
            get
            {
                string str = this.GetStringValue(ATTR_BUILDINGTYPE);
                if (string.IsNullOrEmpty(str)) 
                    return AddressBuildingType.Building;
                try 
                {
                    return (AddressBuildingType)Enum.Parse(typeof(AddressBuildingType), str, true);
                }
                catch(Exception ex352) 
                {
                    return AddressBuildingType.Building;
                }
            }
            set
            {
                this.AddSlot(ATTR_BUILDINGTYPE, value.ToString().ToUpper(), true, 0);
            }
        }
        /// <summary>
        /// Корпус
        /// </summary>
        public string Corpus
        {
            get
            {
                return this.GetStringValue(ATTR_CORPUS);
            }
            set
            {
                this.AddSlot(ATTR_CORPUS, value, true, 0);
            }
        }
        /// <summary>
        /// Корпус или квартира
        /// </summary>
        public string CorpusOrFlat
        {
            get
            {
                return this.GetStringValue(ATTR_CORPUSORFLAT);
            }
            set
            {
                this.AddSlot(ATTR_CORPUSORFLAT, value, true, 0);
            }
        }
        /// <summary>
        /// Этаж
        /// </summary>
        public string Floor
        {
            get
            {
                return this.GetStringValue(ATTR_FLOOR);
            }
            set
            {
                this.AddSlot(ATTR_FLOOR, value, true, 0);
            }
        }
        /// <summary>
        /// Подъезд
        /// </summary>
        public string Potch
        {
            get
            {
                return this.GetStringValue(ATTR_PORCH);
            }
            set
            {
                this.AddSlot(ATTR_PORCH, value, true, 0);
            }
        }
        /// <summary>
        /// Квартира
        /// </summary>
        public string Flat
        {
            get
            {
                return this.GetStringValue(ATTR_FLAT);
            }
            set
            {
                this.AddSlot(ATTR_FLAT, value, true, 0);
            }
        }
        /// <summary>
        /// Номер офиса
        /// </summary>
        public string Office
        {
            get
            {
                return this.GetStringValue(ATTR_OFFICE);
            }
            set
            {
                this.AddSlot(ATTR_OFFICE, value, true, 0);
            }
        }
        /// <summary>
        /// Номер участка
        /// </summary>
        public string Plot
        {
            get
            {
                return this.GetStringValue(ATTR_PLOT);
            }
            set
            {
                this.AddSlot(ATTR_PLOT, value, true, 0);
            }
        }
        /// <summary>
        /// Блок (ряд)
        /// </summary>
        public string Block
        {
            get
            {
                return this.GetStringValue(ATTR_BLOCK);
            }
            set
            {
                this.AddSlot(ATTR_BLOCK, value, true, 0);
            }
        }
        /// <summary>
        /// Бокс (гараж)
        /// </summary>
        public string Box
        {
            get
            {
                return this.GetStringValue(ATTR_BOX);
            }
            set
            {
                this.AddSlot(ATTR_BOX, value, true, 0);
            }
        }
        /// <summary>
        /// Станция метро
        /// </summary>
        public string Metro
        {
            get
            {
                return this.GetStringValue(ATTR_METRO);
            }
            set
            {
                this.AddSlot(ATTR_METRO, value, true, 0);
            }
        }
        /// <summary>
        /// Километр
        /// </summary>
        public string Kilometer
        {
            get
            {
                return this.GetStringValue(ATTR_KILOMETER);
            }
            set
            {
                this.AddSlot(ATTR_KILOMETER, value, true, 0);
            }
        }
        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string Zip
        {
            get
            {
                return this.GetStringValue(ATTR_ZIP);
            }
            set
            {
                this.AddSlot(ATTR_ZIP, value, true, 0);
            }
        }
        /// <summary>
        /// Почтовый ящик
        /// </summary>
        public string PostOfficeBox
        {
            get
            {
                return this.GetStringValue(ATTR_POSTOFFICEBOX);
            }
            set
            {
                this.AddSlot(ATTR_POSTOFFICEBOX, value, true, 0);
            }
        }
        /// <summary>
        /// ГСП (абонент городской служебной почты)
        /// </summary>
        public string CSP
        {
            get
            {
                return this.GetStringValue(ATTR_CSP);
            }
            set
            {
                this.AddSlot(ATTR_CSP, value, true, 0);
            }
        }
        /// <summary>
        /// Ссылки на географические объекты (самого нижнего уровня)
        /// </summary>
        public List<Pullenti.Ner.Geo.GeoReferent> Geos
        {
            get
            {
                List<Pullenti.Ner.Geo.GeoReferent> res = new List<Pullenti.Ner.Geo.GeoReferent>();
                foreach (Pullenti.Ner.Slot a in Slots) 
                {
                    if (a.TypeName == ATTR_GEO && (a.Value is Pullenti.Ner.Geo.GeoReferent)) 
                        res.Add(a.Value as Pullenti.Ner.Geo.GeoReferent);
                    else if (a.TypeName == ATTR_STREET && (a.Value is Pullenti.Ner.Referent)) 
                    {
                        foreach (Pullenti.Ner.Slot s in (a.Value as Pullenti.Ner.Referent).Slots) 
                        {
                            if (s.Value is Pullenti.Ner.Geo.GeoReferent) 
                                res.Add(s.Value as Pullenti.Ner.Geo.GeoReferent);
                        }
                    }
                }
                for (int i = res.Count - 1; i > 0; i--) 
                {
                    for (int j = i - 1; j >= 0; j--) 
                    {
                        if (_isHigher(res[i], res[j])) 
                        {
                            res.RemoveAt(i);
                            break;
                        }
                        else if (_isHigher(res[j], res[i])) 
                        {
                            res.RemoveAt(j);
                            i--;
                        }
                    }
                }
                return res;
            }
        }
        static bool _isHigher(Pullenti.Ner.Geo.GeoReferent gHi, Pullenti.Ner.Geo.GeoReferent gLo)
        {
            int i = 0;
            for (; gLo != null && (i < 10); gLo = gLo.Higher,i++) 
            {
                if (gLo.CanBeEquals(gHi, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                    return true;
            }
            return false;
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                Pullenti.Ner.Referent sr = this.GetSlotValue(ATTR_STREET) as Pullenti.Ner.Referent;
                if (sr != null) 
                    return sr;
                List<Pullenti.Ner.Geo.GeoReferent> geos = Geos;
                foreach (Pullenti.Ner.Geo.GeoReferent g in geos) 
                {
                    if (g.IsCity) 
                        return g;
                }
                foreach (Pullenti.Ner.Geo.GeoReferent g in geos) 
                {
                    if (g.IsRegion && !g.IsState) 
                        return g;
                }
                if (geos.Count > 0) 
                    return geos[0];
                return null;
            }
        }
        public void AddReferent(Pullenti.Ner.Referent r)
        {
            if (r == null) 
                return;
            Pullenti.Ner.Geo.GeoReferent geo = r as Pullenti.Ner.Geo.GeoReferent;
            if (geo != null) 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_GEO) 
                    {
                        Pullenti.Ner.Geo.GeoReferent geo0 = s.Value as Pullenti.Ner.Geo.GeoReferent;
                        if (geo0 == null) 
                            continue;
                        if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(geo0, geo)) 
                        {
                            if (geo.Higher == geo0 || geo.IsCity) 
                            {
                                this.UploadSlot(s, geo);
                                return;
                            }
                        }
                        if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(geo, geo0)) 
                            return;
                    }
                }
                this.AddSlot(ATTR_GEO, r, false, 0);
            }
            else if ((r is StreetReferent) || r.TypeName == "ORGANIZATION") 
                this.AddSlot(ATTR_STREET, r, false, 0);
        }
        /// <summary>
        /// Дополнительная детализация места (пересечение, около ...)
        /// </summary>
        public AddressDetailType Detail
        {
            get
            {
                string s = this.GetStringValue(ATTR_DETAIL);
                if (s == null) 
                    return AddressDetailType.Undefined;
                try 
                {
                    object res = Enum.Parse(typeof(AddressDetailType), s, true);
                    if (res is AddressDetailType) 
                        return (AddressDetailType)res;
                }
                catch(Exception ex353) 
                {
                }
                return AddressDetailType.Undefined;
            }
            set
            {
                if (value != AddressDetailType.Undefined) 
                    this.AddSlot(ATTR_DETAIL, value.ToString().ToUpper(), true, 0);
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            string str = this.GetStringValue(ATTR_DETAIL);
            if (str != null) 
                str = Pullenti.Ner.Address.Internal.MetaAddress.GlobalMeta.DetailFeature.ConvertInnerValueToOuterValue(str, lang) as string;
            if (str != null) 
            {
                res.AppendFormat("[{0}", str.ToLower());
                if ((((str = this.GetStringValue(ATTR_DETAILPARAM)))) != null) 
                    res.AppendFormat(", {0}", str);
                res.Append(']');
            }
            List<Pullenti.Ner.Referent> strs = Streets;
            if (strs.Count == 0) 
            {
                if (Metro != null) 
                {
                    if (res.Length > 0) 
                        res.Append(' ');
                    res.Append(Metro ?? "");
                }
            }
            else 
            {
                if (res.Length > 0) 
                    res.Append(' ');
                for (int i = 0; i < strs.Count; i++) 
                {
                    if (i > 0) 
                        res.Append(", ");
                    res.Append(strs[i].ToString(true, lang, 0));
                }
            }
            if (Kilometer != null) 
                res.AppendFormat(" {0}км.", Kilometer);
            if (House != null) 
            {
                AddressHouseType ty = HouseType;
                if (ty == AddressHouseType.Estate) 
                    res.Append(" влад.");
                else if (ty == AddressHouseType.HouseEstate) 
                    res.Append(" домовл.");
                else 
                    res.Append(" д.");
                res.Append((House == "0" ? "Б/Н" : House));
            }
            if (Corpus != null) 
                res.AppendFormat(" корп.{0}", (Corpus == "0" ? "Б/Н" : Corpus));
            if (Building != null) 
            {
                AddressBuildingType ty = BuildingType;
                if (ty == AddressBuildingType.Construction) 
                    res.Append(" сооруж.");
                else if (ty == AddressBuildingType.Liter) 
                    res.Append(" лит.");
                else 
                    res.Append(" стр.");
                res.Append((Building == "0" ? "Б/Н" : Building));
            }
            if (Potch != null) 
                res.AppendFormat(" под.{0}", Potch);
            if (Floor != null) 
                res.AppendFormat(" эт.{0}", Floor);
            if (Flat != null) 
                res.AppendFormat(" кв.{0}", Flat);
            if (CorpusOrFlat != null) 
                res.AppendFormat(" корп.(кв.?){0}", CorpusOrFlat);
            if (Office != null) 
                res.AppendFormat(" оф.{0}", Office);
            if (Block != null) 
                res.AppendFormat(" блок {0}", Block);
            if (Plot != null) 
                res.AppendFormat(" уч.{0}", Plot);
            if (Box != null) 
                res.AppendFormat(" бокс {0}", Box);
            if (PostOfficeBox != null) 
                res.AppendFormat(" а\\я{0}", PostOfficeBox);
            if (CSP != null) 
                res.AppendFormat(" ГСП-{0}", CSP);
            object kladr = this.GetSlotValue(ATTR_FIAS);
            if (kladr is Pullenti.Ner.Referent) 
            {
                res.AppendFormat(" (ФИАС: {0}", (kladr as Pullenti.Ner.Referent).GetStringValue("GUID") ?? "?");
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_FIAS && (s.Value is Pullenti.Ner.Referent) && s.Value != kladr) 
                        res.AppendFormat(", {0}", (s.Value as Pullenti.Ner.Referent).GetStringValue("GUID") ?? "?");
                }
                res.Append(')');
            }
            string bti = this.GetStringValue(ATTR_BTI);
            if (bti != null) 
                res.AppendFormat(" (БТИ {0})", bti);
            foreach (Pullenti.Ner.Geo.GeoReferent g in Geos) 
            {
                if (res.Length > 0 && res[res.Length - 1] == ' ') 
                    res.Length--;
                if (res.Length > 0 && res[res.Length - 1] == ']') 
                {
                }
                else if (res.Length > 0) 
                    res.Append(';');
                res.AppendFormat(" {0}", g.ToString(true, lang, lev + 1));
            }
            if (Zip != null) 
                res.AppendFormat("; {0}", Zip);
            return res.ToString().Trim();
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            AddressReferent addr = obj as AddressReferent;
            if (addr == null) 
                return false;
            List<Pullenti.Ner.Referent> strs1 = Streets;
            List<Pullenti.Ner.Referent> strs2 = addr.Streets;
            if (strs1.Count > 0 || strs2.Count > 0) 
            {
                bool ok = false;
                foreach (Pullenti.Ner.Referent s in strs1) 
                {
                    foreach (Pullenti.Ner.Referent ss in strs2) 
                    {
                        if (ss.CanBeEquals(s, typ)) 
                        {
                            ok = true;
                            break;
                        }
                    }
                }
                if (!ok) 
                    return false;
            }
            if (addr.House != null || House != null) 
            {
                if (addr.House != House) 
                    return false;
            }
            if (addr.Building != null || Building != null) 
            {
                if (addr.Building != Building) 
                    return false;
            }
            if (addr.Plot != null || Plot != null) 
            {
                if (addr.Plot != Plot) 
                    return false;
            }
            if (addr.Box != null || Box != null) 
            {
                if (addr.Box != Box) 
                    return false;
            }
            if (addr.Block != null || Block != null) 
            {
                if (addr.Block != Block) 
                    return false;
            }
            if (addr.Corpus != null || Corpus != null) 
            {
                if (addr.Corpus != Corpus) 
                {
                    if (addr.Corpus != null && addr.Corpus == CorpusOrFlat) 
                    {
                    }
                    else if (Corpus != null && addr.CorpusOrFlat == Corpus) 
                    {
                    }
                    else 
                        return false;
                }
            }
            if (addr.Flat != null || Flat != null) 
            {
                if (addr.Flat != Flat) 
                {
                    if (addr.Flat != null && addr.Flat == CorpusOrFlat) 
                    {
                    }
                    else if (Flat != null && addr.CorpusOrFlat == Flat) 
                    {
                    }
                    else 
                        return false;
                }
            }
            if (addr.CorpusOrFlat != null || CorpusOrFlat != null) 
            {
                if (CorpusOrFlat != null && addr.CorpusOrFlat != null) 
                {
                    if (CorpusOrFlat != addr.CorpusOrFlat) 
                        return false;
                }
                else if (CorpusOrFlat == null) 
                {
                    if (Corpus == null && Flat == null) 
                        return false;
                }
                else if (addr.CorpusOrFlat == null) 
                {
                    if (addr.Corpus == null && addr.Flat == null) 
                        return false;
                }
            }
            if (addr.Office != null || Office != null) 
            {
                if (addr.Office != Office) 
                    return false;
            }
            if (addr.Potch != null || Potch != null) 
            {
                if (addr.Potch != Potch) 
                    return false;
            }
            if (addr.Floor != null || Floor != null) 
            {
                if (addr.Floor != Floor) 
                    return false;
            }
            if (addr.PostOfficeBox != null || PostOfficeBox != null) 
            {
                if (addr.PostOfficeBox != PostOfficeBox) 
                    return false;
            }
            if (addr.CSP != null && CSP != null) 
            {
                if (addr.CSP != CSP) 
                    return false;
            }
            List<Pullenti.Ner.Geo.GeoReferent> geos1 = Geos;
            List<Pullenti.Ner.Geo.GeoReferent> geos2 = addr.Geos;
            if (geos1.Count > 0 && geos2.Count > 0) 
            {
                bool ok = false;
                foreach (Pullenti.Ner.Geo.GeoReferent g1 in geos1) 
                {
                    foreach (Pullenti.Ner.Geo.GeoReferent g2 in geos2) 
                    {
                        if (g1.CanBeEquals(g2, typ)) 
                        {
                            ok = true;
                            break;
                        }
                    }
                }
                if (!ok) 
                    return false;
            }
            return true;
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            base.MergeSlots(obj, mergeStatistic);
            if (CorpusOrFlat != null) 
            {
                if (Flat == CorpusOrFlat) 
                    CorpusOrFlat = null;
                else if (Corpus == CorpusOrFlat) 
                    CorpusOrFlat = null;
            }
            this.Correct();
        }
        internal void Correct()
        {
            List<Pullenti.Ner.Geo.GeoReferent> geos = new List<Pullenti.Ner.Geo.GeoReferent>();
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_GEO && (a.Value is Pullenti.Ner.Geo.GeoReferent)) 
                    geos.Add(a.Value as Pullenti.Ner.Geo.GeoReferent);
                else if (a.TypeName == ATTR_STREET && (a.Value is Pullenti.Ner.Referent)) 
                {
                    foreach (Pullenti.Ner.Slot s in (a.Value as Pullenti.Ner.Referent).Slots) 
                    {
                        if (s.Value is Pullenti.Ner.Geo.GeoReferent) 
                            geos.Add(s.Value as Pullenti.Ner.Geo.GeoReferent);
                    }
                }
            }
            for (int i = geos.Count - 1; i > 0; i--) 
            {
                for (int j = i - 1; j >= 0; j--) 
                {
                    if (_isHigher(geos[i], geos[j])) 
                    {
                        Pullenti.Ner.Slot s = this.FindSlot(ATTR_GEO, geos[i], true);
                        if (s != null) 
                            Slots.Remove(s);
                        geos.RemoveAt(i);
                        break;
                    }
                    else if (_isHigher(geos[j], geos[i])) 
                    {
                        Pullenti.Ner.Slot s = this.FindSlot(ATTR_GEO, geos[j], true);
                        if (s != null) 
                            Slots.Remove(s);
                        geos.RemoveAt(j);
                        i--;
                    }
                }
            }
            if (geos.Count == 2) 
            {
                Pullenti.Ner.Geo.GeoReferent reg = null;
                Pullenti.Ner.Geo.GeoReferent cit = null;
                for (int ii = 0; ii < geos.Count; ii++) 
                {
                    if (geos[ii].IsTerritory && geos[ii].Higher != null) 
                        geos[ii] = geos[ii].Higher;
                }
                if (geos[0].IsCity && geos[1].IsRegion) 
                {
                    cit = geos[0];
                    reg = geos[1];
                }
                else if (geos[1].IsCity && geos[0].IsRegion) 
                {
                    cit = geos[1];
                    reg = geos[0];
                }
                if (cit != null && cit.Higher == null && Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(reg, cit)) 
                {
                    cit.Higher = reg;
                    Pullenti.Ner.Slot ss = this.FindSlot(ATTR_GEO, reg, true);
                    if (ss != null) 
                        Slots.Remove(ss);
                    geos = Geos;
                }
                else 
                {
                    Pullenti.Ner.Geo.GeoReferent stat = null;
                    Pullenti.Ner.Geo.GeoReferent geo = null;
                    if (geos[0].IsState && !geos[1].IsState) 
                    {
                        stat = geos[0];
                        geo = geos[1];
                    }
                    else if (geos[1].IsState && !geos[0].IsState) 
                    {
                        stat = geos[1];
                        geo = geos[0];
                    }
                    if (stat != null) 
                    {
                        geo = geo.TopHigher;
                        if (geo.Higher == null) 
                        {
                            geo.Higher = stat;
                            Pullenti.Ner.Slot s = this.FindSlot(ATTR_GEO, stat, true);
                            if (s != null) 
                                Slots.Remove(s);
                        }
                    }
                }
            }
        }
    }
}