/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Instrument
{
    /// <summary>
    /// Представление всего документа
    /// </summary>
    public class InstrumentReferent : InstrumentBlockReferent
    {
        public InstrumentReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Instrument.Internal.MetaInstrument.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("INSTRUMENT")
        /// </summary>
        public const string OBJ_TYPENAME = "INSTRUMENT";
        /// <summary>
        /// Имя атрибута - тип документа
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - регистрационный номер
        /// </summary>
        public const string ATTR_REGNUMBER = "NUMBER";
        /// <summary>
        /// Имя атрибута - номер судебного дела
        /// </summary>
        public const string ATTR_CASENUMBER = "CASENUMBER";
        /// <summary>
        /// Имя атрибута - дата
        /// </summary>
        public const string ATTR_DATE = "DATE";
        /// <summary>
        /// Имя атрибута - подписант
        /// </summary>
        public const string ATTR_SIGNER = "SIGNER";
        /// <summary>
        /// Имя атрибута - публикующий орган
        /// </summary>
        public const string ATTR_SOURCE = "SOURCE";
        /// <summary>
        /// Имя атрибута - географический объект
        /// </summary>
        public const string ATTR_GEO = "GEO";
        /// <summary>
        /// Имя атрибута - номер части (если это часть другого документа)
        /// </summary>
        public const string ATTR_PART = "PART";
        /// <summary>
        /// Имя атрибута - номер приложения (если это приложение)
        /// </summary>
        public const string ATTR_APPENDIX = "APPENDIX";
        /// <summary>
        /// Имя атрибута - участник (InstrumentParticipant)
        /// </summary>
        public const string ATTR_PARTICIPANT = "PARTICIPANT";
        /// <summary>
        /// Имя атрибута - артефакт (InstrumentArtefact)
        /// </summary>
        public const string ATTR_ARTEFACT = "ARTEFACT";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            string str;
            if ((((str = this.GetStringValue(ATTR_APPENDIX)))) != null) 
            {
                List<string> strs = this.GetStringValues(ATTR_APPENDIX);
                if (strs.Count == 1) 
                    res.AppendFormat("Приложение{0}{1}; ", (str.Length == 0 ? "" : " "), str);
                else 
                {
                    res.Append("Приложения ");
                    for (int i = 0; i < strs.Count; i++) 
                    {
                        if (i > 0) 
                            res.Append(",");
                        res.Append(strs[i]);
                    }
                    res.Append("; ");
                }
            }
            if ((((str = this.GetStringValue(ATTR_PART)))) != null) 
                res.AppendFormat("Часть {0}; ", str);
            if (Typ != null) 
                res.Append(Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(Typ));
            else 
                res.Append("Документ");
            if (RegNumber != null) 
            {
                res.AppendFormat(" №{0}", RegNumber);
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_REGNUMBER && s.Value.ToString() != RegNumber) 
                        res.AppendFormat("/{0}", s.Value);
                }
            }
            if (CaseNumber != null) 
                res.AppendFormat(" дело №{0}", CaseNumber);
            string dt = this.GetStringValue(ATTR_DATE);
            if (dt != null) 
                res.AppendFormat(" от {0}", dt);
            if ((((str = this.GetStringValue(InstrumentBlockReferent.ATTR_NAME)))) != null) 
            {
                if (str.Length > 100) 
                    str = str.Substring(0, 100) + "...";
                res.AppendFormat(" \"{0}\"", str);
            }
            if ((((str = this.GetStringValue(ATTR_GEO)))) != null) 
                res.AppendFormat(" ({0})", str);
            return res.ToString().Trim();
        }
        /// <summary>
        /// Тип
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
        /// Номер
        /// </summary>
        public string RegNumber
        {
            get
            {
                return this.GetStringValue(ATTR_REGNUMBER);
            }
            set
            {
                if (string.IsNullOrEmpty(value)) 
                {
                    this.AddSlot(ATTR_REGNUMBER, null, true, 0);
                    return;
                }
                if (".,".IndexOf(value[value.Length - 1]) >= 0) 
                    value = value.Substring(0, value.Length - 1);
                this.AddSlot(ATTR_REGNUMBER, value, true, 0);
            }
        }
        /// <summary>
        /// Номер дела
        /// </summary>
        public string CaseNumber
        {
            get
            {
                return this.GetStringValue(ATTR_CASENUMBER);
            }
            set
            {
                if (string.IsNullOrEmpty(value)) 
                    return;
                if (".,".IndexOf(value[value.Length - 1]) >= 0) 
                    value = value.Substring(0, value.Length - 1);
                this.AddSlot(ATTR_CASENUMBER, value, true, 0);
            }
        }
        /// <summary>
        /// Дата подписания
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
        internal bool AddDate(object dt)
        {
            if (dt == null) 
                return false;
            if (dt is Pullenti.Ner.Decree.Internal.DecreeToken) 
            {
                if ((dt as Pullenti.Ner.Decree.Internal.DecreeToken).Ref is Pullenti.Ner.ReferentToken) 
                    return this.AddDate(((dt as Pullenti.Ner.Decree.Internal.DecreeToken).Ref as Pullenti.Ner.ReferentToken).Referent);
                if ((dt as Pullenti.Ner.Decree.Internal.DecreeToken).Value != null) 
                {
                    this.AddSlot(ATTR_DATE, (dt as Pullenti.Ner.Decree.Internal.DecreeToken).Value, true, 0);
                    return true;
                }
                return false;
            }
            if (dt is Pullenti.Ner.ReferentToken) 
                return this.AddDate((dt as Pullenti.Ner.ReferentToken).Referent);
            if (dt is Pullenti.Ner.Date.DateReferent) 
            {
                Pullenti.Ner.Date.DateReferent dr = dt as Pullenti.Ner.Date.DateReferent;
                int year = dr.Year;
                int mon = dr.Month;
                int day = dr.Day;
                if (year == 0) 
                    return dr.Pointer == Pullenti.Ner.Date.DatePointerType.Undefined;
                DateTime? exDate = Date;
                if (exDate != null && exDate.Value.Year == year) 
                {
                    if (mon == 0 && exDate.Value.Month > 0) 
                        return false;
                    if (day == 0 && exDate.Value.Day > 0) 
                        return false;
                    bool delExist = false;
                    if (mon > 0 && exDate.Value.Month == 0) 
                        delExist = true;
                    if (delExist) 
                    {
                        foreach (Pullenti.Ner.Slot s in Slots) 
                        {
                            if (s.TypeName == ATTR_DATE) 
                            {
                                Slots.Remove(s);
                                break;
                            }
                        }
                    }
                }
                StringBuilder tmp = new StringBuilder();
                tmp.Append(year);
                if (mon > 0) 
                    tmp.AppendFormat(".{0}", mon.ToString("D02"));
                if (day > 0) 
                    tmp.AppendFormat(".{0}", day.ToString("D02"));
                this.AddSlot(Pullenti.Ner.Decree.DecreeReferent.ATTR_DATE, tmp.ToString(), false, 0);
                return true;
            }
            if (dt is string) 
            {
                this.AddSlot(ATTR_DATE, dt as string, true, 0);
                return true;
            }
            return false;
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            return obj == this;
        }
    }
}