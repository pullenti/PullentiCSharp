/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Decree
{
    /// <summary>
    /// Сущность - ссылка на НПА (закон, приказ, договор, постановление...)
    /// </summary>
    public class DecreeReferent : Pullenti.Ner.Referent
    {
        public DecreeReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Decree.Internal.MetaDecree.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("DECREE")
        /// </summary>
        public const string OBJ_TYPENAME = "DECREE";
        /// <summary>
        /// Имя атрибута - тип
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - наименование
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - регистрационный номер
        /// </summary>
        public const string ATTR_NUMBER = "NUMBER";
        /// <summary>
        /// Имя атрибута - дата принятия
        /// </summary>
        public const string ATTR_DATE = "DATE";
        /// <summary>
        /// Имя атрибута - публикующий орган
        /// </summary>
        public const string ATTR_SOURCE = "SOURCE";
        /// <summary>
        /// Имя атрибута - географический объект (GeoReferent)
        /// </summary>
        public const string ATTR_GEO = "GEO";
        /// <summary>
        /// Имя атрибута - номер чтения
        /// </summary>
        public const string ATTR_READING = "READING";
        /// <summary>
        /// Имя атрибута - номер судебного дела (для судебных документов)
        /// </summary>
        public const string ATTR_CASENUMBER = "CASENUMBER";
        /// <summary>
        /// Имя атрибута - редакция
        /// </summary>
        public const string ATTR_EDITION = "EDITION";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            DecreeKind ki = Kind;
            bool outPart = false;
            string nam = Name;
            if (Typ != null) 
            {
                if ((nam != null && !nam.StartsWith("О") && nam.Contains(Typ)) && ki != DecreeKind.Standard) 
                {
                    res.Append(Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(nam));
                    nam = null;
                }
                else if (ki == DecreeKind.Standard && (Typ.Length < 6)) 
                    res.Append(Typ);
                else 
                    res.Append(Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(Typ));
            }
            else 
                res.Append("?");
            bool outSrc = true;
            if (ki == DecreeKind.Contract && this.FindSlot(ATTR_SOURCE, null, true) != null) 
            {
                List<string> srcs = new List<string>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_SOURCE) 
                        srcs.Add(s.Value.ToString());
                }
                if (srcs.Count > 1) 
                {
                    for (int ii = 0; ii < srcs.Count; ii++) 
                    {
                        if (ii > 0 && ((ii + 1) < srcs.Count)) 
                            res.Append(", ");
                        else if (ii > 0) 
                            res.Append(" и ");
                        else 
                            res.Append(" между ");
                        res.Append(srcs[ii]);
                        outSrc = false;
                    }
                }
            }
            string num = Number;
            if (num != null) 
            {
                res.AppendFormat(" № {0}", num);
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NUMBER) 
                    {
                        string nn = s.Value.ToString();
                        if (nn != num) 
                            res.AppendFormat("/{0}", nn);
                    }
                }
            }
            if ((((num = CaseNumber))) != null) 
                res.AppendFormat(" по делу № {0}", num);
            if (this.GetStringValue(ATTR_DATE) != null) 
                res.AppendFormat(" {0}{1}", (ki == DecreeKind.Program ? "" : "от "), this.GetStringValue(ATTR_DATE));
            if (outSrc && this.GetSlotValue(ATTR_SOURCE) != null) 
                res.AppendFormat("; {0}", this.GetStringValue(ATTR_SOURCE));
            if (!shortVariant) 
            {
                string s = this.GetStringValue(ATTR_GEO);
                if (s != null) 
                    res.AppendFormat("; {0}", s);
                if (nam != null) 
                {
                    s = this._getShortName();
                    if (s != null) 
                        res.AppendFormat("; \"{0}\"", s);
                }
            }
            return res.ToString().Trim();
        }
        /// <summary>
        /// Наименование (если несколько, то самое короткое)
        /// </summary>
        public string Name
        {
            get
            {
                string nam = null;
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NAME) 
                    {
                        string n = s.Value.ToString();
                        if (nam == null || nam.Length > n.Length) 
                            nam = n;
                    }
                }
                return nam;
            }
        }
        string _getShortName()
        {
            string nam = Name;
            if (nam == null) 
                return null;
            if (nam.Length > 100) 
            {
                int i = 100;
                for (; i < nam.Length; i++) 
                {
                    if (!char.IsLetter(nam[i])) 
                        break;
                }
                if (i < nam.Length) 
                    nam = nam.Substring(0, i) + "...";
            }
            return Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(nam);
        }
        public override List<string> GetCompareStrings()
        {
            List<string> res = new List<string>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NAME || s.TypeName == ATTR_NUMBER) 
                    res.Add(s.Value.ToString());
            }
            if (res.Count == 0 && Typ != null) 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_GEO) 
                        res.Add(string.Format("{0} {1}", Typ, s.Value.ToString()));
                }
            }
            if (Typ == "КОНСТИТУЦИЯ") 
                res.Add(Typ);
            if (res.Count > 0) 
                return res;
            else 
                return base.GetCompareStrings();
        }
        /// <summary>
        /// Дата подписания (для законов дат может быть много - по редакциям)
        /// </summary>
        public DateTime? Date
        {
            get
            {
                string s = this.GetStringValue(ATTR_DATE);
                if (s == null) 
                    return null;
                return Pullenti.Ner.Decree.Internal.DecreeHelper.ParseDateTime(s);
            }
        }
        internal bool AddDate(Pullenti.Ner.Decree.Internal.DecreeToken dt)
        {
            if (dt == null) 
                return false;
            if (dt.Ref != null && (dt.Ref.Referent is Pullenti.Ner.Date.DateReferent)) 
            {
                Pullenti.Ner.Date.DateReferent dr = dt.Ref.Referent as Pullenti.Ner.Date.DateReferent;
                if (dr.IsRelative) 
                    return false;
                int year = dr.Year;
                int mon = dr.Month;
                int day = dr.Day;
                if (year == 0) 
                    return false;
                StringBuilder tmp = new StringBuilder();
                tmp.Append(year);
                if (mon > 0) 
                    tmp.AppendFormat(".{0}", mon.ToString("D02"));
                if (day > 0) 
                    tmp.AppendFormat(".{0}", day.ToString("D02"));
                this.AddSlot(ATTR_DATE, tmp.ToString(), false, 0);
                return true;
            }
            if (dt.Ref != null && (dt.Ref.Referent is Pullenti.Ner.Date.DateRangeReferent)) 
            {
                this.AddSlot(ATTR_DATE, dt.Ref.Referent, false, 0);
                return true;
            }
            if (dt.Value != null) 
            {
                this.AddSlot(ATTR_DATE, dt.Value, false, 0);
                return true;
            }
            return false;
        }
        List<int> _allYears()
        {
            List<int> res = new List<int>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_DATE) 
                {
                    string str = s.Value.ToString();
                    int i = str.IndexOf('.');
                    if (i == 4) 
                        str = str.Substring(0, 4);
                    if (int.TryParse(str, out i)) 
                        res.Add(i);
                }
            }
            return res;
        }
        /// <summary>
        /// Тип НПА
        /// </summary>
        public string Typ
        {
            get
            {
                return this.GetStringValue(ATTR_TYPE);
            }
            set
            {
                this.AddSlot(ATTR_TYPE, value, true, 0);
            }
        }
        /// <summary>
        /// Класс НПА
        /// </summary>
        public DecreeKind Kind
        {
            get
            {
                return Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(Typ);
            }
        }
        /// <summary>
        /// Признак того, что это именно закон, а не подзаконный акт. 
        /// Для законов возможны несколько номеров и дат (редакций)
        /// </summary>
        public bool IsLaw
        {
            get
            {
                return Pullenti.Ner.Decree.Internal.DecreeToken.IsLaw(Typ);
            }
        }
        public string Typ0
        {
            get
            {
                string typ = Typ;
                if (typ == null) 
                    return null;
                int i = typ.LastIndexOf(' ');
                if (i < 0) 
                    return typ;
                if (typ.StartsWith("ПАСПОРТ")) 
                    return "ПАСПОРТ";
                if (typ.StartsWith("ОСНОВЫ") || typ.StartsWith("ОСНОВИ")) 
                {
                    i = typ.IndexOf(' ');
                    return typ.Substring(0, i);
                }
                return typ.Substring(i + 1);
            }
        }
        /// <summary>
        /// Номер (для законов номеров может быть много)
        /// </summary>
        public string Number
        {
            get
            {
                return this.GetStringValue(ATTR_NUMBER);
            }
        }
        /// <summary>
        /// Номер судебного дела
        /// </summary>
        public string CaseNumber
        {
            get
            {
                return this.GetStringValue(ATTR_CASENUMBER);
            }
        }
        public override Pullenti.Ner.Slot AddSlot(string attrName, object attrValue, bool clearOldValue, int statCount = 0)
        {
            if (attrValue is Pullenti.Ner.Decree.Internal.PartToken.PartValue) 
                attrValue = (attrValue as Pullenti.Ner.Decree.Internal.PartToken.PartValue).Value;
            Pullenti.Ner.Slot s = base.AddSlot(attrName, attrValue, clearOldValue, statCount);
            if (attrValue is Pullenti.Ner.Decree.Internal.PartToken.PartValue) 
                s.Tag = (attrValue as Pullenti.Ner.Decree.Internal.PartToken.PartValue).SourceValue;
            return s;
        }
        internal void AddNumber(Pullenti.Ner.Decree.Internal.DecreeToken dt)
        {
            if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
            {
                if (dt.NumYear > 0) 
                    this.AddSlot(ATTR_DATE, dt.NumYear.ToString(), false, 0);
            }
            if (string.IsNullOrEmpty(dt.Value)) 
                return;
            string value = dt.Value;
            if (".,".IndexOf(value[value.Length - 1]) >= 0) 
                value = value.Substring(0, value.Length - 1);
            this.AddSlot(ATTR_NUMBER, value, false, 0);
        }
        internal void AddName(DecreeReferent dr)
        {
            Pullenti.Ner.Slot s = dr.FindSlot(ATTR_NAME, null, true);
            if (s == null) 
                return;
            Pullenti.Ner.Slot ss = this.AddSlot(ATTR_NAME, s.Value, false, 0);
            if (ss != null && ss.Tag == null) 
                ss.Tag = s.Tag;
        }
        internal void AddNameStr(string name)
        {
            if (name == null || name.Length == 0) 
                return;
            if (name[name.Length - 1] == '.') 
            {
                if (name.Length > 5 && char.IsLetter(name[name.Length - 2]) && !char.IsLetter(name[name.Length - 3])) 
                {
                }
                else 
                    name = name.Substring(0, name.Length - 1);
            }
            name = name.Trim();
            string uname = name.ToUpper();
            Pullenti.Ner.Slot s = this.AddSlot(ATTR_NAME, uname, false, 0);
            if (uname != name) 
                s.Tag = name;
        }
        string _getNumberDigits(string num)
        {
            if (num == null) 
                return "";
            StringBuilder tmp = new StringBuilder();
            for (int i = 0; i < num.Length; i++) 
            {
                if (char.IsDigit(num[i])) 
                {
                    if (num[i] == '0' && tmp.Length == 0) 
                    {
                    }
                    else if (num[i] == '3' && tmp.Length > 0 && num[i - 1] == 'Ф') 
                    {
                    }
                    else 
                        tmp.Append(num[i]);
                }
            }
            return tmp.ToString();
        }
        List<string> _allNumberDigits()
        {
            List<string> res = new List<string>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NUMBER) 
                    res.Add(this._getNumberDigits(s.Value as string));
            }
            return res;
        }
        List<DateTime> _allDates()
        {
            List<DateTime> res = new List<DateTime>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_DATE) 
                {
                    DateTime? dt = Pullenti.Ner.Decree.Internal.DecreeHelper.ParseDateTime(s.Value as string);
                    if (dt != null) 
                        res.Add(dt.Value);
                }
            }
            return res;
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            bool b = this._CanBeEquals(obj, typ, false);
            return b;
        }
        bool _CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ, bool ignoreGeo)
        {
            DecreeReferent dr = obj as DecreeReferent;
            if (dr == null) 
                return false;
            if (dr.Typ0 != null && Typ0 != null) 
            {
                if (dr.Typ0 != Typ0) 
                    return false;
            }
            int numEq = 0;
            if (Number != null || dr.Number != null) 
            {
                if (Number != null && dr.Number != null) 
                {
                    List<string> di1 = this._allNumberDigits();
                    List<string> di2 = dr._allNumberDigits();
                    foreach (string d1 in di1) 
                    {
                        if (di2.Contains(d1)) 
                        {
                            numEq = 1;
                            break;
                        }
                    }
                    if (numEq == 0 && !IsLaw) 
                        return false;
                    foreach (Pullenti.Ner.Slot s in Slots) 
                    {
                        if (s.TypeName == ATTR_NUMBER) 
                        {
                            if (dr.FindSlot(s.TypeName, s.Value, true) != null) 
                            {
                                numEq = 2;
                                break;
                            }
                        }
                    }
                    if (numEq == 0) 
                        return false;
                }
            }
            if (CaseNumber != null && dr.CaseNumber != null) 
            {
                if (CaseNumber != dr.CaseNumber) 
                    return false;
            }
            if (this.FindSlot(ATTR_GEO, null, true) != null && dr.FindSlot(ATTR_GEO, null, true) != null) 
            {
                if (this.GetStringValue(ATTR_GEO) != dr.GetStringValue(ATTR_GEO)) 
                    return false;
            }
            bool srcEq = false;
            bool srcNotEq = false;
            Pullenti.Ner.Slot src = this.FindSlot(ATTR_SOURCE, null, true);
            if (src != null && dr.FindSlot(ATTR_SOURCE, null, true) != null) 
            {
                if (dr.FindSlot(src.TypeName, src.Value, true) == null) 
                    srcNotEq = true;
                else 
                    srcEq = true;
            }
            bool dateNotEq = false;
            bool dateIsEqu = false;
            bool yearsIsEqu = false;
            string date1 = this.GetStringValue(ATTR_DATE);
            string date2 = dr.GetStringValue(ATTR_DATE);
            if (date1 != null || date2 != null) 
            {
                if (IsLaw) 
                {
                    List<int> ys1 = this._allYears();
                    List<int> ys2 = dr._allYears();
                    foreach (int y1 in ys1) 
                    {
                        if (ys2.Contains(y1)) 
                        {
                            yearsIsEqu = true;
                            break;
                        }
                    }
                    if (yearsIsEqu) 
                    {
                        List<DateTime> dts1 = this._allDates();
                        List<DateTime> dts2 = dr._allDates();
                        foreach (DateTime d1 in dts1) 
                        {
                            if (dts2.Contains(d1)) 
                            {
                                dateIsEqu = true;
                                break;
                            }
                        }
                    }
                    if (!dateIsEqu) 
                    {
                        if (Typ == "КОНСТИТУЦИЯ") 
                            return false;
                        if (Date != null && dr.Date != null) 
                            dateNotEq = true;
                    }
                }
                else if (date1 == date2 || ((Date != null && dr.Date != null && Date == dr.Date))) 
                {
                    if (numEq > 1) 
                        return true;
                    dateIsEqu = true;
                }
                else if (Date != null && dr.Date != null) 
                {
                    if (Date.Value.Year != dr.Date.Value.Year) 
                        return false;
                    if (numEq >= 1) 
                    {
                        if (srcEq) 
                            return true;
                        if (srcNotEq) 
                            return false;
                    }
                    else 
                        return false;
                }
                else if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts || Kind == DecreeKind.Publisher) 
                    dateNotEq = true;
            }
            if (this.FindSlot(ATTR_NAME, null, true) != null && dr.FindSlot(ATTR_NAME, null, true) != null) 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NAME) 
                    {
                        if (dr.FindSlot(s.TypeName, s.Value, true) != null) 
                            return true;
                        foreach (Pullenti.Ner.Slot ss in dr.Slots) 
                        {
                            if (ss.TypeName == s.TypeName) 
                            {
                                string n0 = s.Value.ToString();
                                string n1 = ss.Value.ToString();
                                if (n0.StartsWith(n1) || n1.StartsWith(n0)) 
                                    return true;
                            }
                        }
                    }
                }
                if (dateNotEq) 
                    return false;
                if (IsLaw && !dateIsEqu) 
                    return false;
                if (numEq > 0) 
                {
                    if (srcEq) 
                        return true;
                    if (srcNotEq && typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                        return false;
                    else if ((!srcNotEq && numEq > 1 && Date == null) && dr.Date == null) 
                        return true;
                    return false;
                }
            }
            else if (IsLaw && dateNotEq) 
                return false;
            if (dateNotEq) 
                return false;
            string ty = Typ;
            if (ty == null) 
                return numEq > 0;
            DecreeKind t = Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(ty);
            if (t == DecreeKind.Ustav || ty == "КОНСТИТУЦИЯ") 
                return true;
            if (numEq > 0) 
                return true;
            if (this.ToString() == obj.ToString()) 
                return true;
            return false;
        }
        public override bool CanBeGeneralFor(Pullenti.Ner.Referent obj)
        {
            if (!this._CanBeEquals(obj, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText, true)) 
                return false;
            Pullenti.Ner.Geo.GeoReferent g1 = this.GetSlotValue(ATTR_GEO) as Pullenti.Ner.Geo.GeoReferent;
            Pullenti.Ner.Geo.GeoReferent g2 = obj.GetSlotValue(ATTR_GEO) as Pullenti.Ner.Geo.GeoReferent;
            if (g1 == null && g2 != null) 
                return true;
            return false;
        }
        internal bool CheckCorrection(bool nounIsDoubtful)
        {
            string typ = Typ0;
            if (typ == null) 
                return false;
            if (typ == "КОНСТИТУЦИЯ" || typ == "КОНСТИТУЦІЯ") 
                return true;
            if (Typ == "ЕДИНЫЙ ОТРАСЛЕВОЙ СТАНДАРТ ЗАКУПОК") 
                return true;
            if ((typ == "КОДЕКС" || typ == "ОСНОВЫ ЗАКОНОДАТЕЛЬСТВА" || typ == "ПРОГРАММА") || typ == "ОСНОВИ ЗАКОНОДАВСТВА" || typ == "ПРОГРАМА") 
            {
                if (this.FindSlot(ATTR_NAME, null, true) == null) 
                    return false;
                if (this.FindSlot(ATTR_GEO, null, true) != null) 
                    return true;
                return !nounIsDoubtful;
            }
            if (typ.StartsWith("ОСНОВ")) 
            {
                if (this.FindSlot(ATTR_GEO, null, true) != null) 
                    return true;
                return false;
            }
            if (typ.Contains("ЗАКОН")) 
            {
                if (this.FindSlot(ATTR_NAME, null, true) == null && Number == null) 
                    return false;
                return true;
            }
            if ((((typ.Contains("ОПРЕДЕЛЕНИЕ") || typ.Contains("РЕШЕНИЕ") || typ.Contains("ПОСТАНОВЛЕНИЕ")) || typ.Contains("ПРИГОВОР") || typ.Contains("ВИЗНАЧЕННЯ")) || typ.Contains("РІШЕННЯ") || typ.Contains("ПОСТАНОВА")) || typ.Contains("ВИРОК")) 
            {
                if (Number != null) 
                {
                    if (this.FindSlot(ATTR_DATE, null, true) != null || this.FindSlot(ATTR_SOURCE, null, true) != null || this.FindSlot(ATTR_NAME, null, true) != null) 
                        return true;
                }
                else if (this.FindSlot(ATTR_DATE, null, true) != null && this.FindSlot(ATTR_SOURCE, null, true) != null) 
                    return true;
                return false;
            }
            DecreeKind ty = Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(typ);
            if (ty == DecreeKind.Ustav) 
            {
                if (this.FindSlot(ATTR_SOURCE, null, true) != null) 
                    return true;
            }
            if (ty == DecreeKind.Konvention) 
            {
                if (this.FindSlot(ATTR_NAME, null, true) != null) 
                {
                    if (typ != "ДОГОВОР" && typ != "ДОГОВІР") 
                        return true;
                }
            }
            if (ty == DecreeKind.Standard) 
            {
                if (Number != null && Number.Length > 4) 
                    return true;
            }
            if (Number == null) 
            {
                if (this.FindSlot(ATTR_NAME, null, true) == null || this.FindSlot(ATTR_SOURCE, null, true) == null || this.FindSlot(ATTR_DATE, null, true) == null) 
                {
                    if (ty == DecreeKind.Contract && this.FindSlot(ATTR_SOURCE, null, true) != null && this.FindSlot(ATTR_DATE, null, true) != null) 
                    {
                    }
                    else if (this.FindSlot(ATTR_NAME, "ПРАВИЛА ДОРОЖНОГО ДВИЖЕНИЯ", true) != null) 
                    {
                    }
                    else if (this.FindSlot(ATTR_NAME, "ПРАВИЛА ДОРОЖНЬОГО РУХУ", true) != null) 
                    {
                    }
                    else 
                        return false;
                }
            }
            else 
            {
                if ((typ == "ПАСПОРТ" || typ == "ГОСТ" || typ == "ПБУ") || typ == "ФОРМА") 
                    return true;
                if (this.FindSlot(ATTR_SOURCE, null, true) == null && this.FindSlot(ATTR_DATE, null, true) == null && this.FindSlot(ATTR_NAME, null, true) == null) 
                    return false;
            }
            return true;
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            base.MergeSlots(obj, mergeStatistic);
            for (int i = 0; i < (Slots.Count - 1); i++) 
            {
                for (int j = i + 1; j < Slots.Count; j++) 
                {
                    if (Slots[i].TypeName == Slots[j].TypeName && Slots[i].Value == Slots[j].Value) 
                    {
                        Slots.RemoveAt(j);
                        j--;
                    }
                }
            }
            List<string> nums = this.GetStringValues(ATTR_NUMBER);
            if (nums.Count > 1) 
            {
                nums.Sort();
                for (int i = 0; i < (nums.Count - 1); i++) 
                {
                    if (nums[i + 1].StartsWith(nums[i]) && nums[i + 1].Length > nums[i].Length && !char.IsDigit(nums[i + 1][nums[i].Length])) 
                    {
                        Pullenti.Ner.Slot s = this.FindSlot(ATTR_NUMBER, nums[i], true);
                        if (s != null) 
                            Slots.Remove(s);
                        nums.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            Pullenti.Ner.Core.IntOntologyItem oi = new Pullenti.Ner.Core.IntOntologyItem(this);
            List<string> vars = new List<string>();
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_NAME) 
                {
                    string s = a.Value.ToString();
                    if (!vars.Contains(s)) 
                        vars.Add(s);
                }
            }
            if (Number != null) 
            {
                foreach (string digs in this._allNumberDigits()) 
                {
                    if (!vars.Contains(digs)) 
                        vars.Add(digs);
                }
            }
            foreach (string v in vars) 
            {
                oi.Termins.Add(new Pullenti.Ner.Core.Termin(v));
            }
            return oi;
        }
    }
}