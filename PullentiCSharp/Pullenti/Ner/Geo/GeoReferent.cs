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

namespace Pullenti.Ner.Geo
{
    /// <summary>
    /// Сущность, описывающая территорию как административную единицу. 
    /// Это страны, автономные образования, области, административные районы, 
    /// населённые пункты, территории и пр.
    /// </summary>
    public class GeoReferent : Pullenti.Ner.Referent
    {
        public GeoReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Geo.Internal.MetaGeo.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("GEO")
        /// </summary>
        public const string OBJ_TYPENAME = "GEO";
        /// <summary>
        /// Имя атрибута - наименование
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - тип
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - для страны 2-х значный идентификатор
        /// </summary>
        public const string ATTR_ALPHA2 = "ALPHA2";
        /// <summary>
        /// Имя атрибута - вышележащий географический объект
        /// </summary>
        public const string ATTR_HIGHER = "HIGHER";
        /// <summary>
        /// Имя атрибута - дополнительная ссылка
        /// </summary>
        public const string ATTR_REF = "REF";
        /// <summary>
        /// Имя атрибута - код ФИАС (определяется анализатором FiasAnalyzer)
        /// </summary>
        public const string ATTR_FIAS = "FIAS";
        public const string ATTR_BTI = "BTI";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            return this._ToString(shortVariant, lang, true, lev);
        }
        string _ToString(bool shortVariant, Pullenti.Morph.MorphLang lang, bool outCladr, int lev)
        {
            if (IsUnion && !IsState) 
            {
                StringBuilder res = new StringBuilder();
                res.Append(this.GetStringValue(ATTR_TYPE));
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_REF && (s.Value is Pullenti.Ner.Referent)) 
                        res.AppendFormat("; {0}", (s.Value as Pullenti.Ner.Referent).ToString(true, lang, 0));
                }
                return res.ToString();
            }
            string name = Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(this._getName(lang != null && lang.IsEn));
            if (!shortVariant) 
            {
                if (!IsState) 
                {
                    if (IsCity && IsRegion) 
                    {
                    }
                    else 
                    {
                        string typ = this.GetStringValue(ATTR_TYPE);
                        if (typ != null) 
                        {
                            if (!IsCity) 
                            {
                                int i = typ.LastIndexOf(' ');
                                if (i > 0) 
                                    typ = typ.Substring(i + 1);
                            }
                            name = string.Format("{0} {1}", typ, name);
                        }
                    }
                }
            }
            if (!shortVariant && outCladr) 
            {
                object kladr = this.GetSlotValue(ATTR_FIAS);
                if (kladr is Pullenti.Ner.Referent) 
                    name = string.Format("{0} (ФИАС: {1})", name, (kladr as Pullenti.Ner.Referent).GetStringValue("GUID") ?? "?");
                string bti = this.GetStringValue(ATTR_BTI);
                if (bti != null) 
                    name = string.Format("{0} (БТИ {1})", name, bti);
            }
            if (!shortVariant && Higher != null && (lev < 10)) 
            {
                if (((Higher.IsCity && IsRegion)) || ((this.FindSlot(ATTR_TYPE, "город", true) == null && this.FindSlot(ATTR_TYPE, "місто", true) == null && IsCity))) 
                    return string.Format("{0}; {1}", name, Higher._ToString(false, lang, false, lev + 1));
            }
            return name;
        }
        string _getName(bool cyr)
        {
            string name = null;
            for (int i = 0; i < 2; i++) 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NAME) 
                    {
                        string v = s.Value.ToString();
                        if (string.IsNullOrEmpty(v)) 
                            continue;
                        if (i == 0) 
                        {
                            if (!Pullenti.Morph.LanguageHelper.IsCyrillicChar(v[0])) 
                            {
                                if (cyr) 
                                    continue;
                            }
                            else if (!cyr) 
                                continue;
                        }
                        if (name == null) 
                            name = v;
                        else if (name.Length > v.Length) 
                        {
                            if ((v.Length < 4) && (name.Length < 20)) 
                            {
                            }
                            else if (name[name.Length - 1] == 'В') 
                            {
                            }
                            else 
                                name = v;
                        }
                        else if ((name.Length < 4) && v.Length >= 4 && (v.Length < 10)) 
                            name = v;
                    }
                }
                if (name != null) 
                    break;
            }
            if (name == "МОЛДОВА") 
                name = "МОЛДАВИЯ";
            else if (name == "БЕЛАРУСЬ") 
                name = "БЕЛОРУССИЯ";
            else if (name == "АПСНЫ") 
                name = "АБХАЗИЯ";
            return name ?? "?";
        }
        public override string ToSortString()
        {
            string typ = "GEO4";
            if (IsState) 
                typ = "GEO1";
            else if (IsRegion) 
                typ = "GEO2";
            else if (IsCity) 
                typ = "GEO3";
            return typ + this._getName(false);
        }
        public override List<string> GetCompareStrings()
        {
            List<string> res = new List<string>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NAME) 
                    res.Add(s.Value.ToString());
            }
            if (res.Count > 0) 
                return res;
            else 
                return base.GetCompareStrings();
        }
        internal void AddName(string v)
        {
            if (v != null) 
            {
                if (v.IndexOf('-') > 0) 
                    v = v.Replace(" - ", "-");
                this.AddSlot(ATTR_NAME, v.ToUpper(), false, 0);
            }
        }
        internal void AddTyp(string v)
        {
            if (v != null) 
            {
                if (v == "ТЕРРИТОРИЯ" && IsState) 
                    return;
                this.AddSlot(ATTR_TYPE, v.ToLower(), false, 0);
            }
        }
        internal void AddTypCity(Pullenti.Morph.MorphLang lang)
        {
            if (lang.IsEn) 
                this.AddSlot(ATTR_TYPE, "city", false, 0);
            else if (lang.IsUa) 
                this.AddSlot(ATTR_TYPE, "місто", false, 0);
            else 
                this.AddSlot(ATTR_TYPE, "город", false, 0);
        }
        internal void AddTypReg(Pullenti.Morph.MorphLang lang)
        {
            if (lang.IsEn) 
                this.AddSlot(ATTR_TYPE, "region", false, 0);
            else if (lang.IsUa) 
                this.AddSlot(ATTR_TYPE, "регіон", false, 0);
            else 
                this.AddSlot(ATTR_TYPE, "регион", false, 0);
        }
        internal void AddTypState(Pullenti.Morph.MorphLang lang)
        {
            if (lang.IsEn) 
                this.AddSlot(ATTR_TYPE, "country", false, 0);
            else if (lang.IsUa) 
                this.AddSlot(ATTR_TYPE, "держава", false, 0);
            else 
                this.AddSlot(ATTR_TYPE, "государство", false, 0);
        }
        internal void AddTypUnion(Pullenti.Morph.MorphLang lang)
        {
            if (lang.IsEn) 
                this.AddSlot(ATTR_TYPE, "union", false, 0);
            else if (lang.IsUa) 
                this.AddSlot(ATTR_TYPE, "союз", false, 0);
            else 
                this.AddSlot(ATTR_TYPE, "союз", false, 0);
        }
        internal void AddTypTer(Pullenti.Morph.MorphLang lang)
        {
            if (lang.IsEn) 
                this.AddSlot(ATTR_TYPE, "territory", false, 0);
            else if (lang.IsUa) 
                this.AddSlot(ATTR_TYPE, "територія", false, 0);
            else 
                this.AddSlot(ATTR_TYPE, "территория", false, 0);
        }
        public override Pullenti.Ner.Slot AddSlot(string attrName, object attrValue, bool clearOldValue, int statCount = 0)
        {
            m_TmpBits = 0;
            return base.AddSlot(attrName, attrValue, clearOldValue, statCount);
        }
        public override void UploadSlot(Pullenti.Ner.Slot slot, object newVal)
        {
            m_TmpBits = 0;
            base.UploadSlot(slot, newVal);
        }
        const int BIT_ISCITY = 2;
        const int BIT_ISREGION = 4;
        const int BIT_ISSTATE = 8;
        const int BIT_ISBIGCITY = 0x10;
        const int BIT_ISTERRITORY = 0x20;
        short m_TmpBits = 0;
        void _recalcTmpBits()
        {
            m_TmpBits = 1;
            m_Higher = null;
            GeoReferent hi = this.GetSlotValue(ATTR_HIGHER) as GeoReferent;
            if (hi == this || hi == null) 
            {
            }
            else 
            {
                List<Pullenti.Ner.Referent> li = null;
                bool err = false;
                for (Pullenti.Ner.Referent r = hi.GetSlotValue(ATTR_HIGHER) as Pullenti.Ner.Referent; r != null; r = r.GetSlotValue(ATTR_HIGHER) as Pullenti.Ner.Referent) 
                {
                    if (r == hi || r == this) 
                    {
                        err = true;
                        break;
                    }
                    if (li == null) 
                        li = new List<Pullenti.Ner.Referent>();
                    else if (li.Contains(r)) 
                    {
                        err = true;
                        break;
                    }
                    li.Add(r);
                }
                if (!err) 
                    m_Higher = hi;
            }
            int isState = -1;
            int isReg = -1;
            foreach (Pullenti.Ner.Slot t in Slots) 
            {
                if (t.TypeName == ATTR_TYPE) 
                {
                    string val = t.Value as string;
                    if (val == "территория" || val == "територія" || val == "territory") 
                    {
                        m_TmpBits = 1 | BIT_ISTERRITORY;
                        return;
                    }
                    if (_isCity(val)) 
                    {
                        m_TmpBits |= BIT_ISCITY;
                        if ((val == "город" || val == "місто" || val == "city") || val == "town") 
                            m_TmpBits |= BIT_ISBIGCITY;
                        continue;
                    }
                    if ((val == "государство" || val == "держава" || val == "империя") || val == "імперія" || val == "country") 
                    {
                        m_TmpBits |= BIT_ISSTATE;
                        isReg = 0;
                        continue;
                    }
                    if (_isRegion(val)) 
                    {
                        if (isState < 0) 
                            isState = 0;
                        if (isReg < 0) 
                            isReg = 1;
                    }
                }
                else if (t.TypeName == ATTR_ALPHA2) 
                {
                    m_TmpBits = 1 | BIT_ISSTATE;
                    if (this.FindSlot(ATTR_TYPE, "город", true) != null || this.FindSlot(ATTR_TYPE, "місто", true) != null || this.FindSlot(ATTR_TYPE, "city", true) != null) 
                        m_TmpBits |= (BIT_ISBIGCITY | BIT_ISCITY);
                    return;
                }
            }
            if (isState != 0) 
            {
                if ((isState < 0) && ((m_TmpBits & BIT_ISCITY)) != 0) 
                {
                }
                else 
                    m_TmpBits |= BIT_ISSTATE;
            }
            if (isReg != 0) 
            {
                if ((isState < 0) && ((m_TmpBits & BIT_ISCITY)) != 0) 
                {
                }
                else 
                    m_TmpBits |= BIT_ISREGION;
            }
        }
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
                    if (s.TypeName == ATTR_TYPE) 
                        res.Add((string)s.Value);
                }
                return res;
            }
        }
        /// <summary>
        /// Это может быть населенным пунктом
        /// </summary>
        public bool IsCity
        {
            get
            {
                if (((m_TmpBits & 1)) == 0) 
                    this._recalcTmpBits();
                return ((m_TmpBits & BIT_ISCITY)) != 0;
            }
        }
        /// <summary>
        /// Это именно город, а не деревня или поселок
        /// </summary>
        public bool IsBigCity
        {
            get
            {
                if (((m_TmpBits & 1)) == 0) 
                    this._recalcTmpBits();
                return ((m_TmpBits & BIT_ISBIGCITY)) != 0;
            }
        }
        /// <summary>
        /// Это может быть отдельным государством
        /// </summary>
        public bool IsState
        {
            get
            {
                if (((m_TmpBits & 1)) == 0) 
                    this._recalcTmpBits();
                return ((m_TmpBits & BIT_ISSTATE)) != 0;
            }
        }
        /// <summary>
        /// Это может быть регионом в составе другого образования
        /// </summary>
        public bool IsRegion
        {
            get
            {
                if (((m_TmpBits & 1)) == 0) 
                    this._recalcTmpBits();
                return ((m_TmpBits & BIT_ISREGION)) != 0;
            }
        }
        /// <summary>
        /// Просто территория (например, территория аэропорта Шереметьево)
        /// </summary>
        public bool IsTerritory
        {
            get
            {
                if (((m_TmpBits & 1)) == 0) 
                    this._recalcTmpBits();
                return ((m_TmpBits & BIT_ISTERRITORY)) != 0;
            }
        }
        /// <summary>
        /// Союз России и Белоруссии
        /// </summary>
        public bool IsUnion
        {
            get
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_TYPE) 
                    {
                        string v = s.Value as string;
                        if (v.EndsWith("союз")) 
                            return true;
                    }
                }
                return false;
            }
        }
        static bool _isCity(string v)
        {
            if (((((((((((v.Contains("поселок") || v.Contains("селение") || v.Contains("село")) || v.Contains("деревня") || v.Contains("станица")) || v.Contains("пункт") || v.Contains("станция")) || v.Contains("аул") || v.Contains("хутор")) || v.Contains("местечко") || v.Contains("урочище")) || v.Contains("усадьба") || v.Contains("аал")) || v.Contains("выселки") || v.Contains("арбан")) || v.Contains("місто") || v.Contains("селище")) || v.Contains("сіло") || v.Contains("станиця")) || v.Contains("станція") || v.Contains("city")) || v.Contains("municipality") || v.Contains("town")) 
                return true;
            if (v.Contains("город") || v.Contains("місто")) 
            {
                if (!_isRegion(v)) 
                    return true;
            }
            return false;
        }
        static bool _isRegion(string v)
        {
            if ((((((((((((v.Contains("район") || v.Contains("штат") || v.Contains("область")) || v.Contains("волость") || v.Contains("провинция")) || v.Contains("регион") || v.Contains("округ")) || v.Contains("край") || v.Contains("префектура")) || v.Contains("улус") || v.Contains("провінція")) || v.Contains("регіон") || v.Contains("образование")) || v.Contains("утворення") || v.Contains("автономия")) || v.Contains("автономія") || v.Contains("district")) || v.Contains("county") || v.Contains("state")) || v.Contains("area") || v.Contains("borough")) || v.Contains("parish") || v.Contains("region")) || v.Contains("province") || v.Contains("prefecture")) 
                return true;
            if (v.Contains("городск") || v.Contains("міськ")) 
            {
                if (v.Contains("образование") || v.Contains("освіта")) 
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 2-х символьный идентификатор страны (ISO 3166)
        /// </summary>
        public string Alpha2
        {
            get
            {
                return this.GetStringValue(ATTR_ALPHA2);
            }
            set
            {
                this.AddSlot(ATTR_ALPHA2, value, true, 0);
            }
        }
        GeoReferent m_Higher;
        /// <summary>
        /// Вышестоящий объект
        /// </summary>
        public GeoReferent Higher
        {
            get
            {
                if (((m_TmpBits & 1)) == 0) 
                    this._recalcTmpBits();
                return m_Higher;
            }
            set
            {
                if (value == this) 
                    return;
                if (value != null) 
                {
                    GeoReferent d = value;
                    List<GeoReferent> li = new List<GeoReferent>();
                    for (; d != null; d = d.Higher) 
                    {
                        if (d == this) 
                            return;
                        else if (d.ToString() == this.ToString()) 
                            return;
                        if (li.Contains(d)) 
                            return;
                        li.Add(d);
                    }
                }
                this.AddSlot(ATTR_HIGHER, null, true, 0);
                if (value != null) 
                    this.AddSlot(ATTR_HIGHER, value, true, 0);
            }
        }
        static bool _checkRoundDep(GeoReferent d)
        {
            if (d == null) 
                return true;
            GeoReferent d0 = d;
            List<GeoReferent> li = new List<GeoReferent>();
            for (d = d.Higher; d != null; d = d.Higher) 
            {
                if (d == d0) 
                    return true;
                if (li.Contains(d)) 
                    return true;
                li.Add(d);
            }
            return false;
        }
        public GeoReferent TopHigher
        {
            get
            {
                if (_checkRoundDep(this)) 
                    return this;
                for (GeoReferent hi = this; hi != null; hi = hi.Higher) 
                {
                    if (hi.Higher == null) 
                        return hi;
                }
                return this;
            }
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return Higher;
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            GeoReferent geo = obj as GeoReferent;
            if (geo == null) 
                return false;
            if (geo.Alpha2 != null && geo.Alpha2 == Alpha2) 
                return true;
            if (IsCity != geo.IsCity) 
                return false;
            if (IsUnion != geo.IsUnion) 
                return false;
            if (IsUnion) 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_REF) 
                    {
                        if (obj.FindSlot(ATTR_REF, s.Value, true) == null) 
                            return false;
                    }
                }
                foreach (Pullenti.Ner.Slot s in obj.Slots) 
                {
                    if (s.TypeName == ATTR_REF) 
                    {
                        if (this.FindSlot(ATTR_REF, s.Value, true) == null) 
                            return false;
                    }
                }
                return true;
            }
            Pullenti.Ner.Referent ref1 = this.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
            Pullenti.Ner.Referent ref2 = geo.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
            if (ref1 != null && ref2 != null) 
            {
                if (ref1 != ref2) 
                    return false;
            }
            bool r = IsRegion || IsState;
            bool r1 = geo.IsRegion || geo.IsState;
            if (r != r1) 
            {
                if (IsTerritory != geo.IsTerritory) 
                    return false;
                return false;
            }
            bool eqNames = false;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NAME) 
                {
                    if (geo.FindSlot(s.TypeName, s.Value, true) != null) 
                    {
                        eqNames = true;
                        break;
                    }
                }
            }
            if (!eqNames) 
                return false;
            if (IsRegion && geo.IsRegion) 
            {
                List<string> typs1 = Typs;
                List<string> typs2 = geo.Typs;
                bool ok = false;
                foreach (string t in typs1) 
                {
                    if (typs2.Contains(t)) 
                        ok = true;
                    else 
                        foreach (string tt in typs2) 
                        {
                            if (Pullenti.Morph.LanguageHelper.EndsWith(tt, t) || Pullenti.Morph.LanguageHelper.EndsWith(t, tt)) 
                                ok = true;
                        }
                }
                if (!ok) 
                    return false;
            }
            if (Higher != null && geo.Higher != null) 
            {
                if (_checkRoundDep(this) || _checkRoundDep(geo)) 
                    return false;
                if (Higher.CanBeEquals(geo.Higher, typ)) 
                {
                }
                else if (geo.Higher.Higher != null && Higher.CanBeEquals(geo.Higher.Higher, typ)) 
                {
                }
                else if (Higher.Higher != null && Higher.Higher.CanBeEquals(geo.Higher, typ)) 
                {
                }
                else 
                    return false;
            }
            return true;
        }
        internal void MergeSlots2(Pullenti.Ner.Referent obj, Pullenti.Morph.MorphLang lang)
        {
            bool mergeStatistic = true;
            foreach (Pullenti.Ner.Slot s in obj.Slots) 
            {
                if (s.TypeName == ATTR_NAME || s.TypeName == ATTR_TYPE) 
                {
                    string nam = (string)s.Value;
                    if (Pullenti.Morph.LanguageHelper.IsLatinChar(nam[0])) 
                    {
                        if (!lang.IsEn) 
                            continue;
                    }
                    else if (lang.IsEn) 
                        continue;
                    if (Pullenti.Morph.LanguageHelper.EndsWith(nam, " ССР")) 
                        continue;
                }
                this.AddSlot(s.TypeName, s.Value, false, (mergeStatistic ? s.Count : 0));
            }
            if (this.FindSlot(ATTR_NAME, null, true) == null && obj.FindSlot(ATTR_NAME, null, true) != null) 
            {
                foreach (Pullenti.Ner.Slot s in obj.Slots) 
                {
                    if (s.TypeName == ATTR_NAME) 
                        this.AddSlot(s.TypeName, s.Value, false, (mergeStatistic ? s.Count : 0));
                }
            }
            if (this.FindSlot(ATTR_TYPE, null, true) == null && obj.FindSlot(ATTR_TYPE, null, true) != null) 
            {
                foreach (Pullenti.Ner.Slot s in obj.Slots) 
                {
                    if (s.TypeName == ATTR_TYPE) 
                        this.AddSlot(s.TypeName, s.Value, false, (mergeStatistic ? s.Count : 0));
                }
            }
            if (IsTerritory) 
            {
                if (((Alpha2 != null || this.FindSlot(ATTR_TYPE, "государство", true) != null || this.FindSlot(ATTR_TYPE, "держава", true) != null) || this.FindSlot(ATTR_TYPE, "империя", true) != null || this.FindSlot(ATTR_TYPE, "імперія", true) != null) || this.FindSlot(ATTR_TYPE, "state", true) != null) 
                {
                    Pullenti.Ner.Slot s = this.FindSlot(ATTR_TYPE, "территория", true);
                    if (s != null) 
                        Slots.Remove(s);
                }
            }
            if (IsState) 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_TYPE && ((s.Value.ToString() == "регион" || s.Value.ToString() == "регіон" || s.Value.ToString() == "region"))) 
                    {
                        Slots.Remove(s);
                        break;
                    }
                }
            }
            if (IsCity) 
            {
                Pullenti.Ner.Slot s = this.FindSlot(ATTR_TYPE, "город", true) ?? this.FindSlot(ATTR_TYPE, "місто", true) ?? this.FindSlot(ATTR_TYPE, "city", true);
                if (s != null) 
                {
                    foreach (Pullenti.Ner.Slot ss in Slots) 
                    {
                        if (ss.TypeName == ATTR_TYPE && ss != s && _isCity((string)ss.Value)) 
                        {
                            Slots.Remove(s);
                            break;
                        }
                    }
                }
            }
            bool has = false;
            for (int i = 0; i < Slots.Count; i++) 
            {
                if (Slots[i].TypeName == ATTR_HIGHER) 
                {
                    if (!has) 
                        has = true;
                    else 
                    {
                        Slots.RemoveAt(i);
                        i--;
                    }
                }
            }
            this._mergeExtReferents(obj);
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            bool isCity = IsCity;
            Pullenti.Ner.Core.IntOntologyItem oi = new Pullenti.Ner.Core.IntOntologyItem(this);
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_NAME) 
                {
                    string s = a.Value.ToString();
                    Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin();
                    t.InitByNormalText(s, null);
                    if (isCity) 
                        t.AddStdAbridges();
                    oi.Termins.Add(t);
                }
            }
            return oi;
        }
        internal bool CheckAbbr(string abbr)
        {
            if (abbr.Length != 2) 
                return false;
            bool nameq = false;
            bool typeq = false;
            bool nameq2 = false;
            bool typeq2 = false;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NAME) 
                {
                    string val = s.Value as string;
                    char ch = val[0];
                    if (ch == abbr[0]) 
                    {
                        nameq = true;
                        int ii = val.IndexOf(' ');
                        if (ii > 0) 
                        {
                            if (abbr[1] == val[ii + 1]) 
                            {
                                if (val.IndexOf(' ', ii + 1) < 0) 
                                    return true;
                            }
                        }
                    }
                    if (ch == abbr[1]) 
                        nameq2 = true;
                }
                else if (s.TypeName == ATTR_TYPE) 
                {
                    string ty = (string)s.Value;
                    if (ty == "государство" || ty == "держава" || ty == "country") 
                        continue;
                    char ch = char.ToUpper(ty[0]);
                    if (ch == abbr[1]) 
                        typeq = true;
                    if (ch == abbr[0]) 
                        typeq2 = true;
                }
            }
            if (typeq && nameq) 
                return true;
            if (typeq2 && nameq2) 
                return true;
            return false;
        }
        // Добавляем ссылку на организацию, также добавляем имена
        internal void AddOrgReferent(Pullenti.Ner.Referent org)
        {
            if (org == null) 
                return;
            bool nam = false;
            this.AddSlot(ATTR_REF, org, false, 0);
            GeoReferent geo = null;
            string specTyp = null;
            string num = org.GetStringValue("NUMBER");
            foreach (Pullenti.Ner.Slot s in org.Slots) 
            {
                if (s.TypeName == "NAME") 
                {
                    if (num == null) 
                        this.AddName(s.Value as string);
                    else 
                        this.AddName(string.Format("{0}-{1}", s.Value, num));
                    nam = true;
                }
                else if (s.TypeName == "TYPE") 
                {
                    string v = s.Value as string;
                    if (v == "СЕЛЬСКИЙ СОВЕТ") 
                        this.AddTyp("сельский округ");
                    else if (v == "ГОРОДСКОЙ СОВЕТ") 
                        this.AddTyp("городской округ");
                    else if (v == "ПОСЕЛКОВЫЙ СОВЕТ") 
                        this.AddTyp("поселковый округ");
                    else if (v == "аэропорт") 
                        specTyp = v.ToUpper();
                }
                else if (s.TypeName == "GEO" && (s.Value is GeoReferent)) 
                    geo = s.Value as GeoReferent;
            }
            if (!nam) 
            {
                foreach (Pullenti.Ner.Slot s in org.Slots) 
                {
                    if (s.TypeName == "EPONYM") 
                    {
                        if (num == null) 
                            this.AddName((s.Value as string).ToUpper());
                        else 
                            this.AddName(string.Format("{0}-{1}", (s.Value as string).ToUpper(), num));
                        nam = true;
                    }
                }
            }
            if (!nam && num != null) 
            {
                foreach (Pullenti.Ner.Slot s in org.Slots) 
                {
                    if (s.TypeName == "TYPE") 
                    {
                        this.AddName(string.Format("{0}-{1}", (s.Value as string).ToUpper(), num));
                        nam = true;
                    }
                }
            }
            if (geo != null && !nam) 
            {
                foreach (string n in geo.GetStringValues(ATTR_NAME)) 
                {
                    this.AddName(n);
                    if (specTyp != null) 
                    {
                        this.AddName(string.Format("{0} {1}", n, specTyp));
                        this.AddName(string.Format("{0} {1}", specTyp, n));
                    }
                    nam = true;
                }
            }
            if (!nam) 
                this.AddName(org.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0).ToUpper());
        }
    }
}