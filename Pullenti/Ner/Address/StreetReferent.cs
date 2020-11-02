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
    /// Сущность: улица, проспект, площадь, шоссе и т.п. Выделяется анализатором AddressAnalyzer.
    /// </summary>
    public class StreetReferent : Pullenti.Ner.Referent
    {
        public StreetReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Address.Internal.MetaStreet.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("STREET")
        /// </summary>
        public const string OBJ_TYPENAME = "STREET";
        /// <summary>
        /// Имя атрибута - тип (улица, переулок, площадь...)
        /// </summary>
        public const string ATTR_TYP = "TYP";
        /// <summary>
        /// Имя атрибута - наименование (м.б. несколько вариантов)
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - номер
        /// </summary>
        public const string ATTR_NUMBER = "NUMBER";
        /// <summary>
        /// Имя атрибута - дополнительный номер
        /// </summary>
        public const string ATTR_SECNUMBER = "SECNUMBER";
        /// <summary>
        /// Имя атрибута - географический объект
        /// </summary>
        public const string ATTR_GEO = "GEO";
        /// <summary>
        /// Имя атрибута - код ФИАС (определяется анализатором FiasAnalyzer)
        /// </summary>
        public const string ATTR_FIAS = "FIAS";
        public const string ATTR_BTI = "BTI";
        public const string ATTR_OKM = "OKM";
        /// <summary>
        /// Тип(ы)
        /// </summary>
        public List<string> Typs
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_TYP) 
                        res.Add((string)s.Value);
                }
                return res;
            }
        }
        /// <summary>
        /// Наименования
        /// </summary>
        public List<string> Names
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NAME) 
                        res.Add((string)s.Value);
                }
                return res;
            }
        }
        /// <summary>
        /// Номер улицы (16-я Парковая)
        /// </summary>
        public string Number
        {
            get
            {
                return this.GetStringValue(ATTR_NUMBER);
            }
            set
            {
                this.AddSlot(ATTR_NUMBER, value, true, 0);
            }
        }
        /// <summary>
        /// Дополнительный номер (3-я 1 Мая)
        /// </summary>
        public string SecNumber
        {
            get
            {
                return this.GetStringValue(ATTR_SECNUMBER);
            }
            set
            {
                this.AddSlot(ATTR_SECNUMBER, value, true, 0);
            }
        }
        /// <summary>
        /// Ссылка на географические объекты
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
                }
                return res;
            }
        }
        /// <summary>
        /// Город
        /// </summary>
        public Pullenti.Ner.Geo.GeoReferent City
        {
            get
            {
                foreach (Pullenti.Ner.Geo.GeoReferent g in Geos) 
                {
                    if (g.IsCity) 
                        return g;
                    else if (g.Higher != null && g.Higher.IsCity) 
                        return g.Higher;
                }
                return null;
            }
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return this.GetSlotValue(ATTR_GEO) as Pullenti.Ner.Geo.GeoReferent;
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder tmp = new StringBuilder();
            string nam = this.GetStringValue(ATTR_NAME);
            List<string> typs = Typs;
            if (typs.Count > 0) 
            {
                for (int i = 0; i < typs.Count; i++) 
                {
                    if (nam != null && nam.Contains(typs[i].ToUpper())) 
                        continue;
                    if (tmp.Length > 0) 
                        tmp.Append('/');
                    tmp.Append(typs[i]);
                }
            }
            else 
                tmp.Append((lang != null && lang.IsUa ? "вулиця" : "улица"));
            if (Number != null) 
            {
                tmp.AppendFormat(" {0}", Number);
                if (SecNumber != null) 
                    tmp.AppendFormat(" {0}", SecNumber);
            }
            if (nam != null) 
                tmp.AppendFormat(" {0}", Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(nam));
            if (!shortVariant) 
            {
                object kladr = this.GetSlotValue(ATTR_FIAS);
                if (kladr is Pullenti.Ner.Referent) 
                {
                    tmp.AppendFormat(" (ФИАС: {0}", (kladr as Pullenti.Ner.Referent).GetStringValue("GUID") ?? "?");
                    foreach (Pullenti.Ner.Slot s in Slots) 
                    {
                        if (s.TypeName == ATTR_FIAS && (s.Value is Pullenti.Ner.Referent) && s.Value != kladr) 
                            tmp.AppendFormat(", {0}", (s.Value as Pullenti.Ner.Referent).GetStringValue("GUID") ?? "?");
                    }
                    tmp.Append(')');
                }
                string bti = this.GetStringValue(ATTR_BTI);
                if (bti != null) 
                    tmp.AppendFormat(" (БТИ {0})", bti);
                string okm = this.GetStringValue(ATTR_OKM);
                if (okm != null) 
                    tmp.AppendFormat(" (ОКМ УМ {0})", okm);
            }
            if (!shortVariant && City != null) 
                tmp.AppendFormat("; {0}", City.ToString(true, lang, lev + 1));
            return tmp.ToString();
        }
        /// <summary>
        /// Классификатор
        /// </summary>
        public StreetKind Kind
        {
            get
            {
                foreach (string t in Typs) 
                {
                    if (t.Contains("дорога")) 
                        return StreetKind.Road;
                    else if (t.Contains("метро")) 
                        return StreetKind.Metro;
                }
                return StreetKind.Undefined;
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            return this._canBeEquals(obj, typ, false);
        }
        bool _canBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ, bool ignoreGeo)
        {
            StreetReferent stri = obj as StreetReferent;
            if (stri == null) 
                return false;
            if (Kind != stri.Kind) 
                return false;
            List<string> typs1 = Typs;
            List<string> typs2 = stri.Typs;
            bool ok = false;
            if (typs1.Count > 0 && typs2.Count > 0) 
            {
                foreach (string t in typs1) 
                {
                    if (typs2.Contains(t)) 
                    {
                        ok = true;
                        break;
                    }
                }
                if (!ok) 
                    return false;
            }
            string num = Number;
            string num1 = stri.Number;
            if (num != null || num1 != null) 
            {
                if (num == null || num1 == null) 
                    return false;
                string sec = SecNumber;
                string sec1 = stri.SecNumber;
                if (sec == null && sec1 == null) 
                {
                    if (num != num1) 
                        return false;
                }
                else if (num == num1) 
                {
                    if (sec != sec1) 
                        return false;
                }
                else if (sec == num1 && sec1 == num) 
                {
                }
                else 
                    return false;
            }
            List<string> names1 = Names;
            List<string> names2 = stri.Names;
            if (names1.Count > 0 || names2.Count > 0) 
            {
                ok = false;
                foreach (string n in names1) 
                {
                    if (names2.Contains(n)) 
                    {
                        ok = true;
                        break;
                    }
                }
                if (!ok) 
                    return false;
            }
            if (ignoreGeo) 
                return true;
            List<Pullenti.Ner.Geo.GeoReferent> geos1 = Geos;
            List<Pullenti.Ner.Geo.GeoReferent> geos2 = stri.Geos;
            if (geos1.Count > 0 && geos2.Count > 0) 
            {
                ok = false;
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
                {
                    if (City != null && stri.City != null) 
                        ok = City.CanBeEquals(stri.City, typ);
                }
                if (!ok) 
                    return false;
            }
            return true;
        }
        public override Pullenti.Ner.Slot AddSlot(string attrName, object attrValue, bool clearOldValue, int statCount = 0)
        {
            if (attrName == ATTR_NAME && (attrValue is string)) 
            {
                string str = attrValue as string;
                if (str.IndexOf('.') > 0) 
                {
                    for (int i = 1; i < (str.Length - 1); i++) 
                    {
                        if (str[i] == '.' && str[i + 1] != ' ') 
                            str = str.Substring(0, i + 1) + " " + str.Substring(i + 1);
                    }
                }
                attrValue = str;
            }
            return base.AddSlot(attrName, attrValue, clearOldValue, statCount);
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            base.MergeSlots(obj, mergeStatistic);
        }
        public override bool CanBeGeneralFor(Pullenti.Ner.Referent obj)
        {
            if (!this._canBeEquals(obj, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText, true)) 
                return false;
            List<Pullenti.Ner.Geo.GeoReferent> geos1 = Geos;
            List<Pullenti.Ner.Geo.GeoReferent> geos2 = (obj as StreetReferent).Geos;
            if (geos2.Count == 0 || geos1.Count > 0) 
                return false;
            return true;
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            Pullenti.Ner.Core.IntOntologyItem oi = new Pullenti.Ner.Core.IntOntologyItem(this);
            List<string> names = Names;
            foreach (string n in names) 
            {
                oi.Termins.Add(new Pullenti.Ner.Core.Termin(n));
            }
            return oi;
        }
        internal void Correct()
        {
            List<string> names = Names;
            for (int i = names.Count - 1; i >= 0; i--) 
            {
                string ss = names[i];
                int jj = ss.IndexOf(' ');
                if (jj < 0) 
                    continue;
                if (ss.LastIndexOf(' ') != jj) 
                    continue;
                string[] pp = ss.Split(' ');
                if (pp.Length == 2) 
                {
                    string ss2 = string.Format("{0} {1}", pp[1], pp[0]);
                    if (!names.Contains(ss2)) 
                        this.AddSlot(ATTR_NAME, ss2, false, 0);
                }
            }
        }
    }
}