/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Date
{
    /// <summary>
    /// Сущность, представляющая диапазон дат
    /// </summary>
    public class DateRangeReferent : Pullenti.Ner.Referent
    {
        public DateRangeReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Date.Internal.MetaDateRange.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("DATERANGE")
        /// </summary>
        public const string OBJ_TYPENAME = "DATERANGE";
        /// <summary>
        /// Имя атрибута - дата начала диапазона (DateReferent)
        /// </summary>
        public const string ATTR_FROM = "FROM";
        /// <summary>
        /// Имя атрибута - дата окончания диапазона (DateReferent)
        /// </summary>
        public const string ATTR_TO = "TO";
        /// <summary>
        /// Начало диапазона
        /// </summary>
        public DateReferent DateFrom
        {
            get
            {
                return this.GetSlotValue(ATTR_FROM) as DateReferent;
            }
            set
            {
                this.AddSlot(ATTR_FROM, value, true, 0);
            }
        }
        /// <summary>
        /// Конец диапазона
        /// </summary>
        public DateReferent DateTo
        {
            get
            {
                return this.GetSlotValue(ATTR_TO) as DateReferent;
            }
            set
            {
                this.AddSlot(ATTR_TO, value, true, 0);
            }
        }
        /// <summary>
        /// Признак относительности диапазона (с 10 по 20 февраля прошлого года)
        /// </summary>
        public bool IsRelative
        {
            get
            {
                if (DateFrom != null && DateFrom.IsRelative) 
                    return true;
                if (DateTo != null && DateTo.IsRelative) 
                    return true;
                return false;
            }
        }
        /// <summary>
        /// Вычислить диапазон дат (если не диапазон, то from = to)
        /// </summary>
        /// <param name="now">текущая дата-время</param>
        /// <param name="from">результирующее начало диапазона</param>
        /// <param name="to">результирующий конец диапазона</param>
        /// <param name="tense">время (-1 - прошлое, 0 - любое, 1 - будущее) - используется 
        /// при неоднозначных случаях. 
        /// Например, 7 сентября, а сейчас лето, то какой это год? При +1 - этот, при -1 - предыдущий</param>
        /// <return>признак корректности</return>
        public bool CalculateDateRange(DateTime now, out DateTime from, out DateTime to, int tense = 0)
        {
            return Pullenti.Ner.Date.Internal.DateRelHelper.CalculateDateRange2(this, now, out from, out to, tense);
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            if (IsRelative && !shortVariant) 
            {
                StringBuilder res = new StringBuilder();
                res.Append(this.ToString(true, lang, lev));
                Pullenti.Ner.Date.Internal.DateRelHelper.AppendToString2(this, res);
                return res.ToString();
            }
            string fr = (DateFrom == null ? null : DateFrom._ToString(shortVariant, lang, lev, 1));
            string to = (DateTo == null ? null : DateTo._ToString(shortVariant, lang, lev, 2));
            if (fr != null && to != null) 
                return string.Format("{0} {1}", fr, (DateTo.Century > 0 && DateTo.Year == 0 ? to : to.ToLower()));
            if (fr != null) 
                return fr.ToString();
            if (to != null) 
                return to;
            return string.Format("{0} ? по ?", (lang.IsUa ? 'з' : 'с'));
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            DateRangeReferent dr = obj as DateRangeReferent;
            if (dr == null) 
                return false;
            if (DateFrom != null) 
            {
                if (!DateFrom.CanBeEquals(dr.DateFrom, typ)) 
                    return false;
            }
            else if (dr.DateFrom != null) 
                return false;
            if (DateTo != null) 
            {
                if (!DateTo.CanBeEquals(dr.DateTo, typ)) 
                    return false;
            }
            else if (dr.DateTo != null) 
                return false;
            return true;
        }
        /// <summary>
        /// Проверка, что диапазон задаёт квартал, возвращает номер 1..4
        /// </summary>
        public int QuarterNumber
        {
            get
            {
                if (DateFrom == null || DateTo == null || DateFrom.Year != DateTo.Year) 
                    return 0;
                int m1 = DateFrom.Month;
                int m2 = DateTo.Month;
                if (m1 == 1 && m2 == 3) 
                    return 1;
                if (m1 == 4 && m2 == 6) 
                    return 2;
                if (m1 == 7 && m2 == 9) 
                    return 3;
                if (m1 == 10 && m2 == 12) 
                    return 4;
                return 0;
            }
        }
        /// <summary>
        /// Проверка, что диапазон задаёт полугодие, возвращает номер 1..2
        /// </summary>
        public int HalfyearNumber
        {
            get
            {
                if (DateFrom == null || DateTo == null || DateFrom.Year != DateTo.Year) 
                    return 0;
                int m1 = DateFrom.Month;
                int m2 = DateTo.Month;
                if (m1 == 1 && m2 == 6) 
                    return 1;
                if (m1 == 7 && m2 == 12) 
                    return 2;
                return 0;
            }
        }
    }
}