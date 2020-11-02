/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Date
{
    /// <summary>
    /// Сущность, представляющая дату
    /// </summary>
    public class DateReferent : Pullenti.Ner.Referent
    {
        public DateReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Date.Internal.MetaDate.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("DATE")
        /// </summary>
        public const string OBJ_TYPENAME = "DATE";
        /// <summary>
        /// Имя атрибута - век
        /// </summary>
        public const string ATTR_CENTURY = "CENTURY";
        /// <summary>
        /// Имя атрибута - год
        /// </summary>
        public const string ATTR_YEAR = "YEAR";
        /// <summary>
        /// Имя атрибута - квартал
        /// </summary>
        public const string ATTR_QUARTAL = "QUARTAL";
        /// <summary>
        /// Имя атрибута - месяц
        /// </summary>
        public const string ATTR_MONTH = "MONTH";
        /// <summary>
        /// Имя атрибута - неделя
        /// </summary>
        public const string ATTR_WEEK = "WEEK";
        /// <summary>
        /// Имя атрибута - день
        /// </summary>
        public const string ATTR_DAY = "DAY";
        /// <summary>
        /// Имя атрибута - день недели
        /// </summary>
        public const string ATTR_DAYOFWEEK = "DAYOFWEEK";
        /// <summary>
        /// Имя атрибута - час
        /// </summary>
        public const string ATTR_HOUR = "HOUR";
        /// <summary>
        /// Имя атрибута - минута
        /// </summary>
        public const string ATTR_MINUTE = "MINUTE";
        /// <summary>
        /// Имя атрибута - секунда
        /// </summary>
        public const string ATTR_SECOND = "SECOND";
        /// <summary>
        /// Имя атрибута - ссылка на вышележащуу сущность-дату
        /// </summary>
        public const string ATTR_HIGHER = "HIGHER";
        /// <summary>
        /// Имя атрибута - дополнительный указатель
        /// </summary>
        public const string ATTR_POINTER = "POINTER";
        /// <summary>
        /// Имя атрибута - признак относительности
        /// </summary>
        public const string ATTR_ISRELATIVE = "ISRELATIVE";
        /// <summary>
        /// Дата в стандартной структуре DateTime (null, если что-либо неопределено или дата некорректна)
        /// </summary>
        public DateTime? Dt
        {
            get
            {
                if (Year > 0 && Month > 0 && Day > 0) 
                {
                    if (Month > 12) 
                        return null;
                    if (Day > DateTime.DaysInMonth(Year, Month)) 
                        return null;
                    int h = Hour;
                    int m = Minute;
                    int s = Second;
                    if (h < 0) 
                        h = 0;
                    if (m < 0) 
                        m = 0;
                    if (s < 0) 
                        s = 0;
                    try 
                    {
                        return new DateTime(Year, Month, Day, h, m, (s >= 0 && (s < 60) ? s : 0));
                    }
                    catch(Exception ex) 
                    {
                    }
                }
                return null;
            }
            set
            {
            }
        }
        /// <summary>
        /// Элемент даты относителен (послезавтра, пару лет назад ...)
        /// </summary>
        public bool IsRelative
        {
            get
            {
                if (this.GetStringValue(ATTR_ISRELATIVE) == "true") 
                    return true;
                if (Pointer == DatePointerType.Today) 
                    return true;
                if (Higher == null) 
                    return false;
                return Higher.IsRelative;
            }
            set
            {
                this.AddSlot(ATTR_ISRELATIVE, (value ? "true" : null), true, 0);
            }
        }
        /// <summary>
        /// Вычислить дату-время (одну)
        /// </summary>
        /// <param name="now">текущая дата (для относительных дат)</param>
        /// <param name="tense">время (-1 - прошлое, 0 - любое, 1 - будущее) - испрользуется 
        /// при неоднозначных случаях</param>
        /// <return>дата-время или null</return>
        public DateTime? CalculateDate(DateTime now, int tense = 0)
        {
            return Pullenti.Ner.Date.Internal.DateRelHelper.CalculateDate(this, now, tense);
        }
        /// <summary>
        /// Вычислить диапазон дат (если не диапазон, то from = to)
        /// </summary>
        /// <param name="now">текущая дата-время</param>
        /// <param name="from">результирующее начало диапазона</param>
        /// <param name="to">результирующий конец диапазона</param>
        /// <param name="tense">время (-1 - прошлое, 0 - любое, 1 - будущее) - испрользуется 
        /// при неоднозначных случаях 
        /// Например, 7 сентября, а сейчас лето, то какой это год? При +1 - этот, при -1 - предыдущий</param>
        /// <return>признак корректности</return>
        public bool CalculateDateRange(DateTime now, out DateTime from, out DateTime to, int tense = 0)
        {
            return Pullenti.Ner.Date.Internal.DateRelHelper.CalculateDateRange(this, now, out from, out to, tense);
        }
        /// <summary>
        /// Век (0 - неопределён)
        /// </summary>
        public int Century
        {
            get
            {
                if (Higher != null) 
                    return Higher.Century;
                int cent = this.GetIntValue(ATTR_CENTURY, 0);
                if (cent != 0) 
                    return cent;
                int year = Year;
                if (year > 0) 
                {
                    cent = year / 100;
                    cent++;
                    return cent;
                }
                else if (year < 0) 
                {
                    cent = year / 100;
                    cent--;
                    return cent;
                }
                return 0;
            }
            set
            {
                this.AddSlot(ATTR_CENTURY, value, true, 0);
            }
        }
        /// <summary>
        /// Год (0 - неопределён)
        /// </summary>
        public int Year
        {
            get
            {
                if (Higher != null) 
                    return Higher.Year;
                else 
                    return this.GetIntValue(ATTR_YEAR, 0);
            }
            set
            {
                this.AddSlot(ATTR_YEAR, value, true, 0);
            }
        }
        /// <summary>
        /// Квартал (0 - неопределён)
        /// </summary>
        public int Quartal
        {
            get
            {
                if (this.FindSlot(ATTR_QUARTAL, null, true) == null && Higher != null) 
                    return Higher.Quartal;
                else 
                    return this.GetIntValue(ATTR_QUARTAL, 0);
            }
            set
            {
                this.AddSlot(ATTR_QUARTAL, value, true, 0);
            }
        }
        /// <summary>
        /// Месяц (0 - неопределён)
        /// </summary>
        public int Month
        {
            get
            {
                if (this.FindSlot(ATTR_MONTH, null, true) == null && Higher != null) 
                    return Higher.Month;
                else 
                    return this.GetIntValue(ATTR_MONTH, 0);
            }
            set
            {
                this.AddSlot(ATTR_MONTH, value, true, 0);
            }
        }
        /// <summary>
        /// Неделя (0 - неопределён)
        /// </summary>
        public int Week
        {
            get
            {
                if (this.FindSlot(ATTR_WEEK, null, true) == null && Higher != null) 
                    return Higher.Week;
                else 
                    return this.GetIntValue(ATTR_WEEK, 0);
            }
            set
            {
                this.AddSlot(ATTR_WEEK, value, true, 0);
            }
        }
        /// <summary>
        /// День месяца (0 - неопределён)
        /// </summary>
        public int Day
        {
            get
            {
                if (this.FindSlot(ATTR_DAY, null, true) == null && Higher != null) 
                    return Higher.Day;
                else 
                    return this.GetIntValue(ATTR_DAY, 0);
            }
            set
            {
                this.AddSlot(ATTR_DAY, value, true, 0);
            }
        }
        /// <summary>
        /// День недели (0 - неопределён, 1 - понедельник ...)
        /// </summary>
        public int DayOfWeek
        {
            get
            {
                if (this.FindSlot(ATTR_DAYOFWEEK, null, true) == null && Higher != null) 
                    return Higher.DayOfWeek;
                else 
                    return this.GetIntValue(ATTR_DAYOFWEEK, 0);
            }
            set
            {
                this.AddSlot(ATTR_DAYOFWEEK, value, true, 0);
            }
        }
        /// <summary>
        /// Час (-1 - неопределён)
        /// </summary>
        public int Hour
        {
            get
            {
                return this.GetIntValue(ATTR_HOUR, -1);
            }
            set
            {
                this.AddSlot(ATTR_HOUR, value, true, 0);
            }
        }
        /// <summary>
        /// Минуты (-1 - неопределён)
        /// </summary>
        public int Minute
        {
            get
            {
                return this.GetIntValue(ATTR_MINUTE, -1);
            }
            set
            {
                this.AddSlot(ATTR_MINUTE, value, true, 0);
            }
        }
        /// <summary>
        /// Секунд (-1 - неопределён)
        /// </summary>
        public int Second
        {
            get
            {
                return this.GetIntValue(ATTR_SECOND, -1);
            }
            set
            {
                this.AddSlot(ATTR_SECOND, value, true, 0);
            }
        }
        /// <summary>
        /// Вышестоящая дата
        /// </summary>
        public DateReferent Higher
        {
            get
            {
                return this.GetSlotValue(ATTR_HIGHER) as DateReferent;
            }
            set
            {
                this.AddSlot(ATTR_HIGHER, value, true, 0);
            }
        }
        /// <summary>
        /// Дополнительный указатель примерной даты
        /// </summary>
        public DatePointerType Pointer
        {
            get
            {
                string s = this.GetStringValue(ATTR_POINTER);
                if (s == null) 
                    return DatePointerType.No;
                try 
                {
                    object res = Enum.Parse(typeof(DatePointerType), s, true);
                    if (res is DatePointerType) 
                        return (DatePointerType)res;
                }
                catch(Exception ex725) 
                {
                }
                return DatePointerType.No;
            }
            set
            {
                if (value != DatePointerType.No) 
                    this.AddSlot(ATTR_POINTER, value.ToString(), true, 0);
            }
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return Higher;
            }
        }
        internal static bool CanBeHigher(DateReferent hi, DateReferent lo)
        {
            if (lo == null || hi == null) 
                return false;
            if (lo.Higher == hi) 
                return true;
            if (lo.Higher != null && lo.Higher.CanBeEquals(hi, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                return true;
            if (lo.Higher != null) 
                return false;
            if (lo.Hour >= 0) 
            {
                if (hi.Hour >= 0) 
                    return false;
                if (lo.Day > 0) 
                    return false;
                return true;
            }
            if (hi.Year > 0 && lo.Year <= 0) 
            {
                if (hi.Month > 0) 
                    return false;
                return true;
            }
            return false;
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            return this._ToString(shortVariant, lang, lev, 0);
        }
        internal string _ToString(bool shortVariant, Pullenti.Morph.MorphLang lang, int lev, int fromRange)
        {
            StringBuilder res = new StringBuilder();
            DatePointerType p = Pointer;
            if (lang == null) 
                lang = Pullenti.Morph.MorphLang.RU;
            if (IsRelative) 
            {
                if (Pointer == DatePointerType.Today) 
                {
                    res.AppendFormat("сейчас");
                    if (!shortVariant) 
                        Pullenti.Ner.Date.Internal.DateRelHelper.AppendToString(this, res);
                    return res.ToString();
                }
                string word = null;
                int val = 0;
                bool back = false;
                bool isLocalRel = this.GetStringValue(ATTR_ISRELATIVE) == "true";
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_CENTURY) 
                    {
                        word = "век";
                        int.TryParse(s.Value as string, out val);
                    }
                    else if (s.TypeName == ATTR_YEAR) 
                    {
                        word = "год";
                        int.TryParse(s.Value as string, out val);
                    }
                    else if (s.TypeName == ATTR_MONTH) 
                    {
                        word = "месяц";
                        int.TryParse(s.Value as string, out val);
                        if (!isLocalRel && val >= 1 && val <= 12) 
                            res.Append(m_Month0[val - 1]);
                    }
                    else if (s.TypeName == ATTR_DAY) 
                    {
                        word = "день";
                        int.TryParse(s.Value as string, out val);
                        if ((!isLocalRel && Month > 0 && Month <= 12) && Higher != null && Higher.GetStringValue(ATTR_ISRELATIVE) != "true") 
                            res.AppendFormat("{0} {1}", val, m_Month[Month - 1]);
                        else if (!isLocalRel) 
                            res.AppendFormat("{0} число", val);
                    }
                    else if (s.TypeName == ATTR_QUARTAL) 
                    {
                        word = "квартал";
                        int.TryParse(s.Value as string, out val);
                    }
                    else if (s.TypeName == ATTR_WEEK) 
                    {
                        word = "неделя";
                        int.TryParse(s.Value as string, out val);
                    }
                    else if (s.TypeName == ATTR_HOUR) 
                    {
                        word = "час";
                        int.TryParse(s.Value as string, out val);
                        if (!isLocalRel) 
                            res.AppendFormat("{0}:{1}", val.ToString("D02"), Minute.ToString("D02"));
                    }
                    else if (s.TypeName == ATTR_MINUTE) 
                    {
                        word = "минута";
                        int.TryParse(s.Value as string, out val);
                    }
                    else if (s.TypeName == ATTR_DAYOFWEEK) 
                    {
                        int.TryParse(s.Value as string, out val);
                        if (!isLocalRel) 
                            res.Append((val >= 1 && val <= 7 ? m_WeekDayEx[val - 1] : "?"));
                        else 
                        {
                            if (val < 0) 
                            {
                                val = -val;
                                back = true;
                            }
                            if (val >= 0 && val <= 7) 
                            {
                                res.AppendFormat("{0} {1}", (val == 7 ? (back ? "прошлое" : "будущее") : ((val == 3 || val == 6) ? (back ? "прошлая" : "будущая") : (back ? "прошлый" : "будущий"))), m_WeekDayEx[val - 1]);
                                break;
                            }
                        }
                    }
                }
                if (word != null && isLocalRel) 
                {
                    if (val == 0) 
                        res.AppendFormat("{0} {1}", (word == "неделя" || word == "минута" ? "текущая" : "текущий"), word);
                    else if (val > 0 && !back) 
                        res.AppendFormat("{0} {1} вперёд", val, Pullenti.Ner.Core.MiscHelper.GetTextMorphVarByCaseAndNumberEx(word, null, Pullenti.Morph.MorphNumber.Undefined, val.ToString()));
                    else 
                    {
                        val = -val;
                        res.AppendFormat("{0} {1} назад", val, Pullenti.Ner.Core.MiscHelper.GetTextMorphVarByCaseAndNumberEx(word, null, Pullenti.Morph.MorphNumber.Undefined, val.ToString()));
                    }
                }
                else if (!isLocalRel && res.Length == 0) 
                    res.AppendFormat("{0} {1}", val, Pullenti.Ner.Core.MiscHelper.GetTextMorphVarByCaseAndNumberEx(word, null, Pullenti.Morph.MorphNumber.Undefined, val.ToString()));
                if (!shortVariant) 
                    Pullenti.Ner.Date.Internal.DateRelHelper.AppendToString(this, res);
                if (fromRange == 1) 
                    res.Insert(0, string.Format("{0} ", (lang.IsUa ? "з" : (lang.IsEn ? "from" : "с"))));
                else if (fromRange == 2) 
                    res.Insert(0, (lang.IsEn ? "to " : "по "));
                return res.ToString();
            }
            if (fromRange == 1) 
                res.AppendFormat("{0} ", (lang.IsUa ? "з" : (lang.IsEn ? "from" : "с")));
            else if (fromRange == 2) 
                res.AppendFormat((lang.IsEn ? "to " : "по "));
            if (p != DatePointerType.No) 
            {
                string val = Pullenti.Ner.Date.Internal.MetaDate.Pointer.ConvertInnerValueToOuterValue(p.ToString(), lang);
                if (fromRange == 0 || lang.IsEn) 
                {
                }
                else if (fromRange == 1) 
                {
                    if (p == DatePointerType.Begin) 
                        val = (lang.IsUa ? "початку" : "начала");
                    else if (p == DatePointerType.Center) 
                        val = (lang.IsUa ? "середини" : "середины");
                    else if (p == DatePointerType.End) 
                        val = (lang.IsUa ? "кінця" : "конца");
                    else if (p == DatePointerType.Today) 
                        val = (lang.IsUa ? "цього часу" : "настоящего времени");
                }
                else if (fromRange == 2) 
                {
                    if (p == DatePointerType.Begin) 
                        val = (lang.IsUa ? "початок" : "начало");
                    else if (p == DatePointerType.Center) 
                        val = (lang.IsUa ? "середину" : "середину");
                    else if (p == DatePointerType.End) 
                        val = (lang.IsUa ? "кінець" : "конец");
                    else if (p == DatePointerType.Today) 
                        val = (lang.IsUa ? "теперішній час" : "настоящее время");
                }
                res.AppendFormat("{0} ", val);
            }
            if (DayOfWeek > 0) 
            {
                if (lang.IsEn) 
                    res.AppendFormat("{0}, ", m_WeekDayEn[DayOfWeek - 1]);
                else 
                    res.AppendFormat("{0}, ", m_WeekDay[DayOfWeek - 1]);
            }
            int y = Year;
            int m = Month;
            int d = Day;
            int cent = Century;
            if (y == 0 && cent != 0) 
            {
                bool isBc = cent < 0;
                if (cent < 0) 
                    cent = -cent;
                res.Append(Pullenti.Ner.Core.NumberHelper.GetNumberRoman(cent));
                if (lang.IsUa) 
                    res.Append(" century");
                else if (m > 0 || p != DatePointerType.No || fromRange == 1) 
                    res.Append((lang.IsUa ? " віка" : " века"));
                else 
                    res.Append((lang.IsUa ? " вік" : " век"));
                if (isBc) 
                    res.Append((lang.IsUa ? " до н.е." : " до н.э."));
                return res.ToString();
            }
            if (d > 0) 
                res.Append(d);
            if (m > 0 && m <= 12) 
            {
                if (res.Length > 0 && res[res.Length - 1] != ' ') 
                    res.Append(' ');
                if (lang.IsUa) 
                    res.Append((d > 0 || p != DatePointerType.No || fromRange != 0 ? m_MonthUA[m - 1] : m_Month0UA[m - 1]));
                else if (lang.IsEn) 
                    res.Append(m_MonthEN[m - 1]);
                else 
                    res.Append((d > 0 || p != DatePointerType.No || fromRange != 0 ? m_Month[m - 1] : m_Month0[m - 1]));
            }
            if (y != 0) 
            {
                bool isBc = y < 0;
                if (y < 0) 
                    y = -y;
                if (res.Length > 0 && res[res.Length - 1] != ' ') 
                    res.Append(' ');
                if (lang != null && lang.IsEn) 
                    res.AppendFormat("{0}", y);
                else if (shortVariant) 
                    res.AppendFormat("{0}{1}", y, (lang.IsUa ? "р" : "г"));
                else if (m > 0 || p != DatePointerType.No || fromRange == 1) 
                    res.AppendFormat("{0} {1}", y, (lang.IsUa ? "року" : "года"));
                else 
                    res.AppendFormat("{0} {1}", y, (lang.IsUa ? "рік" : "год"));
                if (isBc) 
                    res.Append((lang.IsUa ? " до н.е." : (lang.IsEn ? "BC" : " до н.э.")));
            }
            int h = Hour;
            int mi = Minute;
            int se = Second;
            if (h >= 0 && mi >= 0) 
            {
                if (res.Length > 0) 
                    res.Append(' ');
                res.AppendFormat("{0}:{1}", h.ToString("D02"), mi.ToString("D02"));
                if (se >= 0) 
                    res.AppendFormat(":{0}", se.ToString("D02"));
            }
            if (res.Length == 0) 
            {
                if (Quartal != 0) 
                    res.AppendFormat("{0}-й квартал", Quartal);
            }
            if (res.Length == 0) 
                return "?";
            while (res[res.Length - 1] == ' ' || res[res.Length - 1] == ',') 
            {
                res.Length--;
            }
            if (!shortVariant && IsRelative) 
                Pullenti.Ner.Date.Internal.DateRelHelper.AppendToString(this, res);
            return res.ToString().Trim();
        }
        static string[] m_Month = new string[] {"января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря"};
        static string[] m_Month0 = new string[] {"январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь"};
        static string[] m_MonthEN = new string[] {"jan", "fab", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec"};
        static string[] m_MonthUA = new string[] {"січня", "лютого", "березня", "квітня", "травня", "червня", "липня", "серпня", "вересня", "жовтня", "листопада", "грудня"};
        static string[] m_Month0UA = new string[] {"січень", "лютий", "березень", "квітень", "травень", "червень", "липень", "серпень", "вересень", "жовтень", "листопад", "грудень"};
        static string[] m_WeekDay = new string[] {"Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"};
        static string[] m_WeekDayEx = new string[] {"понедельник", "вторник", "среда", "четверг", "пятница", "суббота", "воскресенье"};
        static string[] m_WeekDayEn = new string[] {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            DateReferent sd = obj as DateReferent;
            if (sd == null) 
                return false;
            if (sd.IsRelative != IsRelative) 
                return false;
            if (sd.Century != Century) 
                return false;
            if (sd.Year != Year) 
                return false;
            if (sd.Month != Month) 
                return false;
            if (sd.Day != Day) 
                return false;
            if (sd.Hour != Hour) 
                return false;
            if (sd.Minute != Minute) 
                return false;
            if (sd.Second != Second) 
                return false;
            if (sd.Pointer != Pointer) 
                return false;
            if (sd.DayOfWeek > 0 && DayOfWeek > 0) 
            {
                if (sd.DayOfWeek != DayOfWeek) 
                    return false;
            }
            return true;
        }
        public static int Compare(DateReferent d1, DateReferent d2)
        {
            if (d1.Year < d2.Year) 
                return -1;
            if (d1.Year > d2.Year) 
                return 1;
            if (d1.Month < d2.Month) 
                return -1;
            if (d1.Month > d2.Month) 
                return 1;
            if (d1.Day < d2.Day) 
                return -1;
            if (d1.Day > d2.Day) 
                return 1;
            if (d1.Hour < d2.Hour) 
                return -1;
            if (d1.Hour > d2.Hour) 
                return 1;
            if (d1.Minute < d2.Minute) 
                return -1;
            if (d1.Minute > d2.Minute) 
                return 1;
            if (d1.Second > d2.Second) 
                return -1;
            if (d1.Second < d2.Second) 
                return 1;
            return 0;
        }
        /// <summary>
        /// Проверка, что дата или диапазон определены с точностью до одного месяца
        /// </summary>
        public static bool IsMonthDefined(Pullenti.Ner.Referent obj)
        {
            DateReferent sd = obj as DateReferent;
            if (sd != null) 
                return (sd.Year > 0 && sd.Month > 0);
            DateRangeReferent sdr = obj as DateRangeReferent;
            if (sdr != null) 
            {
                if (sdr.DateFrom == null || sdr.DateTo == null) 
                    return false;
                if (sdr.DateFrom.Year == 0 || sdr.DateTo.Year != sdr.DateFrom.Year) 
                    return false;
                if (sdr.DateFrom.Month == 0 || sdr.DateTo.Month != sdr.DateFrom.Month) 
                    return false;
                return true;
            }
            return false;
        }
    }
}