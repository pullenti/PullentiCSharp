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

namespace Pullenti.Ner.Titlepage
{
    /// <summary>
    /// Сущность, описывающая информацию из заголовков статей, книг, диссертация и пр.
    /// </summary>
    public class TitlePageReferent : Pullenti.Ner.Referent
    {
        public TitlePageReferent(string name = null) : base(name ?? OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Titlepage.Internal.MetaTitleInfo.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("TITLEPAGE")
        /// </summary>
        public const string OBJ_TYPENAME = "TITLEPAGE";
        /// <summary>
        /// Имя атрибута - наименование
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - тип
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - автор (PersonReferent)
        /// </summary>
        public const string ATTR_AUTHOR = "AUTHOR";
        /// <summary>
        /// Имя атрибута - руководитель (PersonReferent)
        /// </summary>
        public const string ATTR_SUPERVISOR = "SUPERVISOR";
        /// <summary>
        /// Имя атрибута - редактор (PersonReferent)
        /// </summary>
        public const string ATTR_EDITOR = "EDITOR";
        /// <summary>
        /// Имя атрибута - консультант (PersonReferent)
        /// </summary>
        public const string ATTR_CONSULTANT = "CONSULTANT";
        /// <summary>
        /// Имя атрибута - оппонент (PersonReferent)
        /// </summary>
        public const string ATTR_OPPONENT = "OPPONENT";
        /// <summary>
        /// Имя атрибута - переводчик (PersonReferent)
        /// </summary>
        public const string ATTR_TRANSLATOR = "TRANSLATOR";
        /// <summary>
        /// Имя атрибута - утвердивший (PersonReferent)
        /// </summary>
        public const string ATTR_AFFIRMANT = "AFFIRMANT";
        /// <summary>
        /// Имя атрибута - организации (OrganizationReferent)
        /// </summary>
        public const string ATTR_ORG = "ORGANIZATION";
        /// <summary>
        /// Имя атрибута - курс студента
        /// </summary>
        public const string ATTR_STUDENTYEAR = "STUDENTYEAR";
        /// <summary>
        /// Имя атрибута - дата (год)
        /// </summary>
        public const string ATTR_DATE = "DATE";
        /// <summary>
        /// Имя атрибута - город (GeoReferent)
        /// </summary>
        public const string ATTR_CITY = "CITY";
        /// <summary>
        /// Имя атрибута - специальность (для диссертаций)
        /// </summary>
        public const string ATTR_SPECIALITY = "SPECIALITY";
        /// <summary>
        /// Имя атрибута - дополнительный атрибут
        /// </summary>
        public const string ATTR_ATTR = "ATTR";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            string str = this.GetStringValue(ATTR_NAME);
            res.AppendFormat("\"{0}\"", str ?? "?");
            if (!shortVariant) 
            {
                foreach (Pullenti.Ner.Slot r in Slots) 
                {
                    if (r.TypeName == ATTR_TYPE) 
                    {
                        res.AppendFormat(" ({0})", r.Value);
                        break;
                    }
                }
                foreach (Pullenti.Ner.Slot r in Slots) 
                {
                    if (r.TypeName == ATTR_AUTHOR && (r.Value is Pullenti.Ner.Referent)) 
                        res.AppendFormat(", {0}", (r.Value as Pullenti.Ner.Referent).ToString(true, lang, 0));
                }
            }
            if (City != null && !shortVariant) 
                res.AppendFormat(", {0}", City.ToString(true, lang, 0));
            if (Date != null) 
            {
                if (!shortVariant) 
                    res.AppendFormat(", {0}", Date.ToString(true, lang, 0));
                else 
                    res.AppendFormat(", {0}", Date.Year);
            }
            return res.ToString();
        }
        /// <summary>
        /// Список типов
        /// </summary>
        public List<string> Types
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_TYPE) 
                        res.Add(s.Value.ToString());
                }
                return res;
            }
        }
        internal void AddType(string typ)
        {
            if (!string.IsNullOrEmpty(typ)) 
            {
                this.AddSlot(ATTR_TYPE, typ.ToLower(), false, 0);
                this.CorrectData();
            }
        }
        /// <summary>
        /// Названия (одно или несколько)
        /// </summary>
        public List<string> Names
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NAME) 
                        res.Add(s.Value.ToString());
                }
                return res;
            }
        }
        internal Pullenti.Ner.Core.Termin AddName(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(begin, true, false)) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(begin, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null && br.EndToken == end) 
                {
                    begin = begin.Next;
                    end = end.Previous;
                }
            }
            string val = Pullenti.Ner.Core.MiscHelper.GetTextValue(begin, end, Pullenti.Ner.Core.GetTextAttr.KeepRegister | Pullenti.Ner.Core.GetTextAttr.KeepQuotes);
            if (val == null) 
                return null;
            if (val.EndsWith(".") && !val.EndsWith("..")) 
                val = val.Substring(0, val.Length - 1).Trim();
            this.AddSlot(ATTR_NAME, val, false, 0);
            return new Pullenti.Ner.Core.Termin(val.ToUpper());
        }
        void CorrectData()
        {
        }
        /// <summary>
        /// Дата
        /// </summary>
        public Pullenti.Ner.Date.DateReferent Date
        {
            get
            {
                return this.GetSlotValue(ATTR_DATE) as Pullenti.Ner.Date.DateReferent;
            }
            set
            {
                if (value == null) 
                    return;
                if (Date == null) 
                {
                    this.AddSlot(ATTR_DATE, value, true, 0);
                    return;
                }
                if (Date.Month > 0 && value.Month == 0) 
                    return;
                if (Date.Day > 0 && value.Day == 0) 
                    return;
                this.AddSlot(ATTR_DATE, value, true, 0);
            }
        }
        /// <summary>
        /// Номер курса (для студентов)
        /// </summary>
        public int StudentYear
        {
            get
            {
                return this.GetIntValue(ATTR_STUDENTYEAR, 0);
            }
            set
            {
                this.AddSlot(ATTR_STUDENTYEAR, value, true, 0);
            }
        }
        /// <summary>
        /// Организация
        /// </summary>
        public Pullenti.Ner.Org.OrganizationReferent Org
        {
            get
            {
                return this.GetSlotValue(ATTR_ORG) as Pullenti.Ner.Org.OrganizationReferent;
            }
            set
            {
                this.AddSlot(ATTR_ORG, value, true, 0);
            }
        }
        /// <summary>
        /// Город
        /// </summary>
        public Pullenti.Ner.Geo.GeoReferent City
        {
            get
            {
                return this.GetSlotValue(ATTR_CITY) as Pullenti.Ner.Geo.GeoReferent;
            }
            set
            {
                this.AddSlot(ATTR_CITY, value, true, 0);
            }
        }
        /// <summary>
        /// Специальность
        /// </summary>
        public string Speciality
        {
            get
            {
                return this.GetStringValue(ATTR_SPECIALITY);
            }
            set
            {
                this.AddSlot(ATTR_SPECIALITY, value, true, 0);
            }
        }
    }
}