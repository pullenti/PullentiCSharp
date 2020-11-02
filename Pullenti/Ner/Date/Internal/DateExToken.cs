/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Date.Internal
{
    // ВСЁ, этот класс теперь используется внутренним робразом, а DateReferent поддерживает относительные даты-время
    // Используется для нахождения в тексте абсолютных и относительных дат и диапазонов,
    // например, "в прошлом году", "за первый квартал этого года", "два дня назад и т.п."
    class DateExToken : Pullenti.Ner.MetaToken
    {
        public DateExToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        /// <summary>
        /// Признак того, что это диапазон
        /// </summary>
        public bool IsDiap = false;
        /// <summary>
        /// Выделенные элементы (для диапазона начало периода)
        /// </summary>
        public List<DateExItemToken> ItemsFrom = new List<DateExItemToken>();
        /// <summary>
        /// Для диапазона конец периода
        /// </summary>
        public List<DateExItemToken> ItemsTo = new List<DateExItemToken>();
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            foreach (DateExItemToken it in ItemsFrom) 
            {
                tmp.AppendFormat("{0}{1}; ", (IsDiap ? "(fr)" : ""), it.ToString());
            }
            foreach (DateExItemToken it in ItemsTo) 
            {
                tmp.AppendFormat("(to){0}; ", it.ToString());
            }
            return tmp.ToString();
        }
        /// <summary>
        /// Получить дату-время (одну)
        /// </summary>
        /// <param name="now">текущая дата (для относительных вычислений)</param>
        /// <param name="tense">время (-1 - прошлое, 0 - любое, 1 - будущее) - испрользуется 
        /// при неоднозначных случаях</param>
        /// <return>дата-время или null</return>
        public DateTime? GetDate(DateTime now, int tense = 0)
        {
            DateValues dvl = DateValues.TryCreate((ItemsFrom.Count > 0 ? ItemsFrom : ItemsTo), now, tense);
            try 
            {
                DateTime dt = dvl.GenerateDate(now, false);
                dt = this._correctHours(dt, (ItemsFrom.Count > 0 ? ItemsFrom : ItemsTo), now);
                return dt;
            }
            catch(Exception ex) 
            {
                return null;
            }
        }
        /// <summary>
        /// Получить диапазон (если не диапазон, то from = to)
        /// </summary>
        /// <param name="now">текущая дата-время</param>
        /// <param name="from">начало диапазона</param>
        /// <param name="to">конец диапазона</param>
        /// <param name="tense">время (-1 - прошлое, 0 - любое, 1 - будущее) - испрользуется 
        /// при неоднозначных случаях 
        /// Например, 7 сентября, а сейчас лето, то какой это год? При true - этот, при false - предыдущий</param>
        /// <return>признак корректности</return>
        public bool GetDates(DateTime now, out DateTime from, out DateTime to, int tense = 0)
        {
            from = DateTime.MinValue;
            to = DateTime.MinValue;
            bool hasHours = false;
            foreach (DateExItemToken it in ItemsFrom) 
            {
                if (it.Typ == DateExItemTokenType.Hour || it.Typ == DateExItemTokenType.Minute) 
                    hasHours = true;
            }
            foreach (DateExItemToken it in ItemsTo) 
            {
                if (it.Typ == DateExItemTokenType.Hour || it.Typ == DateExItemTokenType.Minute) 
                    hasHours = true;
            }
            List<DateExItemToken> li = new List<DateExItemToken>();
            if (hasHours) 
            {
                foreach (DateExItemToken it in ItemsFrom) 
                {
                    if (it.Typ != DateExItemTokenType.Hour && it.Typ != DateExItemTokenType.Minute) 
                        li.Add(it);
                }
                foreach (DateExItemToken it in ItemsTo) 
                {
                    if (it.Typ != DateExItemTokenType.Hour && it.Typ != DateExItemTokenType.Minute) 
                    {
                        bool exi = false;
                        foreach (DateExItemToken itt in li) 
                        {
                            if (itt.Typ == it.Typ) 
                            {
                                exi = true;
                                break;
                            }
                        }
                        if (!exi) 
                            li.Add(it);
                    }
                }
                li.Sort();
                DateValues dvl = DateValues.TryCreate(li, now, tense);
                if (dvl == null) 
                    return false;
                try 
                {
                    from = dvl.GenerateDate(now, false);
                }
                catch(Exception ex) 
                {
                    return false;
                }
                to = from;
                from = this._correctHours(from, ItemsFrom, now);
                to = this._correctHours(to, (ItemsTo.Count == 0 ? ItemsFrom : ItemsTo), now);
                return true;
            }
            if (ItemsTo.Count == 0) 
            {
                DateValues dvl = DateValues.TryCreate(ItemsFrom, now, tense);
                if (dvl == null) 
                    return false;
                try 
                {
                    from = dvl.GenerateDate(now, false);
                }
                catch(Exception ex) 
                {
                    return false;
                }
                try 
                {
                    to = dvl.GenerateDate(now, true);
                }
                catch(Exception ex) 
                {
                    to = from;
                }
                return true;
            }
            li.Clear();
            foreach (DateExItemToken it in ItemsFrom) 
            {
                li.Add(it);
            }
            foreach (DateExItemToken it in ItemsTo) 
            {
                bool exi = false;
                foreach (DateExItemToken itt in li) 
                {
                    if (itt.Typ == it.Typ) 
                    {
                        exi = true;
                        break;
                    }
                }
                if (!exi) 
                    li.Add(it);
            }
            li.Sort();
            DateValues dvl1 = DateValues.TryCreate(li, now, tense);
            li.Clear();
            foreach (DateExItemToken it in ItemsTo) 
            {
                li.Add(it);
            }
            foreach (DateExItemToken it in ItemsFrom) 
            {
                bool exi = false;
                foreach (DateExItemToken itt in li) 
                {
                    if (itt.Typ == it.Typ) 
                    {
                        exi = true;
                        break;
                    }
                }
                if (!exi) 
                    li.Add(it);
            }
            li.Sort();
            DateValues dvl2 = DateValues.TryCreate(li, now, tense);
            try 
            {
                from = dvl1.GenerateDate(now, false);
            }
            catch(Exception ex) 
            {
                return false;
            }
            try 
            {
                to = dvl2.GenerateDate(now, true);
            }
            catch(Exception ex) 
            {
                return false;
            }
            return true;
        }
        DateTime _correctHours(DateTime dt, List<DateExItemToken> li, DateTime now)
        {
            bool hasHour = false;
            foreach (DateExItemToken it in li) 
            {
                if (it.Typ == DateExItemTokenType.Hour) 
                {
                    hasHour = true;
                    if (it.IsValueRelate) 
                    {
                        dt = new DateTime(dt.Year, dt.Month, dt.Day, now.Hour, now.Minute, 0);
                        dt = dt.AddHours(it.Value);
                    }
                    else if (it.Value > 0 && (it.Value < 24)) 
                        dt = new DateTime(dt.Year, dt.Month, dt.Day, it.Value, 0, 0);
                }
                else if (it.Typ == DateExItemTokenType.Minute) 
                {
                    if (!hasHour) 
                        dt = new DateTime(dt.Year, dt.Month, dt.Day, now.Hour, 0, 0);
                    if (it.IsValueRelate) 
                    {
                        dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
                        dt = dt.AddMinutes(it.Value);
                        if (!hasHour) 
                            dt = dt.AddMinutes(now.Minute);
                    }
                    else if (it.Value > 0 && (it.Value < 60)) 
                        dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, it.Value, 0);
                }
            }
            return dt;
        }
        class DateValues
        {
            public int Day1;
            public int Day2;
            public int Month1;
            public int Month2;
            public int Year;
            public override string ToString()
            {
                StringBuilder tmp = new StringBuilder();
                if (Year > 0) 
                    tmp.AppendFormat("Year:{0}", Year);
                if (Month1 > 0) 
                {
                    tmp.AppendFormat(" Month:{0}", Month1);
                    if (Month2 > Month1) 
                        tmp.AppendFormat("..{0}", Month2);
                }
                if (Day1 > 0) 
                {
                    tmp.AppendFormat(" Day:{0}", Day1);
                    if (Day2 > Day1) 
                        tmp.AppendFormat("..{0}", Day2);
                }
                return tmp.ToString().Trim();
            }
            public DateTime GenerateDate(DateTime today, bool endOfDiap)
            {
                int year = Year;
                if (year == 0) 
                    year = today.Year;
                int mon = Month1;
                if (mon == 0) 
                    mon = (endOfDiap ? 12 : 1);
                else if (endOfDiap && Month2 > 0) 
                    mon = Month2;
                int day = Day1;
                if (day == 0) 
                    day = (endOfDiap ? 31 : 1);
                else if (Day2 > 0 && endOfDiap) 
                    day = Day2;
                if (day > DateTime.DaysInMonth(year, mon)) 
                    day = DateTime.DaysInMonth(year, mon);
                return new DateTime(year, mon, day);
            }
            public static DateValues TryCreate(List<Pullenti.Ner.Date.Internal.DateExToken.DateExItemToken> list, DateTime today, int tense)
            {
                bool oo = false;
                if (list != null) 
                {
                    foreach (Pullenti.Ner.Date.Internal.DateExToken.DateExItemToken v in list) 
                    {
                        if (v.Typ != Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Hour && v.Typ != Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Minute) 
                            oo = true;
                    }
                }
                if (!oo) 
                    return new DateValues() { Year = today.Year, Month1 = today.Month, Day1 = today.Day };
                if (list == null || list.Count == 0) 
                    return null;
                for (int j = 0; j < list.Count; j++) 
                {
                    if (list[j].Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek) 
                    {
                        if (j > 0 && list[j - 1].Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Week) 
                            break;
                        Pullenti.Ner.Date.Internal.DateExToken.DateExItemToken we = new Pullenti.Ner.Date.Internal.DateExToken.DateExItemToken(list[j].BeginToken, list[j].EndToken) { Typ = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Week, IsValueRelate = true };
                        if (list[j].IsValueRelate) 
                        {
                            list[j].IsValueRelate = false;
                            if (list[j].Value < 0) 
                            {
                                we.Value = -1;
                                list[j].Value = -list[j].Value;
                            }
                        }
                        list.Insert(j, we);
                        break;
                    }
                }
                DateValues res = new DateValues();
                Pullenti.Ner.Date.Internal.DateExToken.DateExItemToken it;
                int i = 0;
                bool hasRel = false;
                if ((i < list.Count) && list[i].Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Year) 
                {
                    it = list[i];
                    if (!it.IsValueRelate) 
                        res.Year = it.Value;
                    else 
                    {
                        res.Year = today.Year + it.Value;
                        hasRel = true;
                    }
                    i++;
                }
                if ((i < list.Count) && list[i].Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Quartal) 
                {
                    it = list[i];
                    int v = 0;
                    if (!it.IsValueRelate) 
                    {
                        if (res.Year == 0) 
                        {
                            int v0 = 1 + ((((today.Month - 1)) / 3));
                            if (it.Value > v0 && (tense < 0)) 
                                res.Year = today.Year - 1;
                            else if ((it.Value < v0) && tense > 0) 
                                res.Year = today.Year + 1;
                            else 
                                res.Year = today.Year;
                        }
                        v = it.Value;
                    }
                    else 
                    {
                        if (res.Year == 0) 
                            res.Year = today.Year;
                        v = 1 + ((((today.Month - 1)) / 3)) + it.Value;
                    }
                    while (v > 3) 
                    {
                        v -= 3;
                        res.Year++;
                    }
                    while (v <= 0) 
                    {
                        v += 3;
                        res.Year--;
                    }
                    res.Month1 = (((v - 1)) * 3) + 1;
                    res.Month2 = res.Month1 + 2;
                    return res;
                }
                if ((i < list.Count) && list[i].Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Month) 
                {
                    it = list[i];
                    if (!it.IsValueRelate) 
                    {
                        if (res.Year == 0) 
                        {
                            if (it.Value > today.Month && (tense < 0)) 
                                res.Year = today.Year - 1;
                            else if ((it.Value < today.Month) && tense > 0) 
                                res.Year = today.Year + 1;
                            else 
                                res.Year = today.Year;
                        }
                        res.Month1 = it.Value;
                    }
                    else 
                    {
                        hasRel = true;
                        if (res.Year == 0) 
                            res.Year = today.Year;
                        int v = today.Month + it.Value;
                        while (v > 12) 
                        {
                            v -= 12;
                            res.Year++;
                        }
                        while (v <= 0) 
                        {
                            v += 12;
                            res.Year--;
                        }
                        res.Month1 = v;
                    }
                    i++;
                }
                if ((i < list.Count) && list[i].Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Weekend && i == 0) 
                {
                    it = list[i];
                    hasRel = true;
                    if (res.Year == 0) 
                        res.Year = today.Year;
                    if (res.Month1 == 0) 
                        res.Month1 = today.Month;
                    if (res.Day1 == 0) 
                        res.Day1 = today.Day;
                    DateTime dt0 = new DateTime(res.Year, res.Month1, res.Day1);
                    DayOfWeek dow = dt0.DayOfWeek;
                    if (dow == DayOfWeek.Monday) 
                        dt0 = dt0.AddDays(5);
                    else if (dow == DayOfWeek.Tuesday) 
                        dt0 = dt0.AddDays(4);
                    else if (dow == DayOfWeek.Wednesday) 
                        dt0 = dt0.AddDays(3);
                    else if (dow == DayOfWeek.Thursday) 
                        dt0 = dt0.AddDays(2);
                    else if (dow == DayOfWeek.Friday) 
                        dt0 = dt0.AddDays(1);
                    else if (dow == DayOfWeek.Saturday) 
                        dt0 = dt0.AddDays(-1);
                    else if (dow == DayOfWeek.Sunday) 
                    {
                    }
                    if (it.Value != 0) 
                        dt0 = dt0.AddDays(it.Value * 7);
                    res.Year = dt0.Year;
                    res.Month1 = dt0.Month;
                    res.Day1 = dt0.Day;
                    dt0 = dt0.AddDays(1);
                    res.Year = dt0.Year;
                    res.Month2 = dt0.Month;
                    res.Day2 = dt0.Day;
                    i++;
                }
                if (((i < list.Count) && list[i].Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Week && i == 0) && list[i].IsValueRelate) 
                {
                    it = list[i];
                    hasRel = true;
                    if (res.Year == 0) 
                        res.Year = today.Year;
                    if (res.Month1 == 0) 
                        res.Month1 = today.Month;
                    if (res.Day1 == 0) 
                        res.Day1 = today.Day;
                    DateTime dt0 = new DateTime(res.Year, res.Month1, res.Day1);
                    DayOfWeek dow = dt0.DayOfWeek;
                    if (dow == DayOfWeek.Tuesday) 
                        dt0 = dt0.AddDays(-1);
                    else if (dow == DayOfWeek.Wednesday) 
                        dt0 = dt0.AddDays(-2);
                    else if (dow == DayOfWeek.Thursday) 
                        dt0 = dt0.AddDays(-3);
                    else if (dow == DayOfWeek.Friday) 
                        dt0 = dt0.AddDays(-4);
                    else if (dow == DayOfWeek.Saturday) 
                        dt0 = dt0.AddDays(-5);
                    else if (dow == DayOfWeek.Sunday) 
                        dt0 = dt0.AddDays(-6);
                    if (it.Value != 0) 
                        dt0 = dt0.AddDays(it.Value * 7);
                    res.Year = dt0.Year;
                    res.Month1 = dt0.Month;
                    res.Day1 = dt0.Day;
                    dt0 = dt0.AddDays(6);
                    res.Year = dt0.Year;
                    res.Month2 = dt0.Month;
                    res.Day2 = dt0.Day;
                    i++;
                }
                if ((i < list.Count) && list[i].Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Day) 
                {
                    it = list[i];
                    if (!it.IsValueRelate) 
                    {
                        res.Day1 = it.Value;
                        if (res.Month1 == 0) 
                        {
                            if (res.Year == 0) 
                                res.Year = today.Year;
                            if (it.Value > today.Day && (tense < 0)) 
                            {
                                res.Month1 = today.Month - 1;
                                if (res.Month1 <= 0) 
                                {
                                    res.Month1 = 12;
                                    res.Year--;
                                }
                            }
                            else if ((it.Value < today.Day) && tense > 0) 
                            {
                                res.Month1 = today.Month + 1;
                                if (res.Month1 > 12) 
                                {
                                    res.Month1 = 1;
                                    res.Year++;
                                }
                            }
                            else 
                                res.Month1 = today.Month;
                        }
                    }
                    else 
                    {
                        hasRel = true;
                        if (res.Year == 0) 
                            res.Year = today.Year;
                        if (res.Month1 == 0) 
                            res.Month1 = today.Month;
                        int v = today.Day + it.Value;
                        while (v > DateTime.DaysInMonth(res.Year, res.Month1)) 
                        {
                            v -= DateTime.DaysInMonth(res.Year, res.Month1);
                            res.Month1++;
                            if (res.Month1 > 12) 
                            {
                                res.Month1 = 1;
                                res.Year++;
                            }
                        }
                        while (v <= 0) 
                        {
                            res.Month1--;
                            if (res.Month1 <= 0) 
                            {
                                res.Month1 = 12;
                                res.Year--;
                            }
                            v += DateTime.DaysInMonth(res.Year, res.Month1);
                        }
                        res.Day1 = v;
                    }
                    i++;
                }
                if ((i < list.Count) && list[i].Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek) 
                {
                    it = list[i];
                    if ((i > 0 && list[i - 1].Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Week && it.Value >= 1) && it.Value <= 7) 
                    {
                        res.Day1 = (res.Day1 + it.Value) - 1;
                        while (res.Day1 > DateTime.DaysInMonth(res.Year, res.Month1)) 
                        {
                            res.Day1 -= DateTime.DaysInMonth(res.Year, res.Month1);
                            res.Month1++;
                            if (res.Month1 > 12) 
                            {
                                res.Month1 = 1;
                                res.Year++;
                            }
                        }
                        res.Day2 = res.Day1;
                        res.Month2 = res.Month1;
                        i++;
                    }
                }
                return res;
            }
        }

        /// <summary>
        /// Выделить в тексте дату с указанной позиции
        /// </summary>
        public static DateExToken TryParse(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if (t.IsValue("ЗА", null) && t.Next != null && t.Next.IsValue("ПЕРИОД", null)) 
            {
                DateExToken ne = TryParse(t.Next.Next);
                if (ne != null && ne.IsDiap) 
                {
                    ne.BeginToken = t;
                    return ne;
                }
            }
            DateExToken res = null;
            bool toRegime = false;
            bool fromRegime = false;
            Pullenti.Ner.Token t0 = null;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                Pullenti.Ner.Date.DateRangeReferent drr = tt.GetReferent() as Pullenti.Ner.Date.DateRangeReferent;
                if (drr != null) 
                {
                    res = new DateExToken(t, tt) { IsDiap = true };
                    Pullenti.Ner.Date.DateReferent fr = drr.DateFrom;
                    if (fr != null) 
                    {
                        if (fr.Pointer == Pullenti.Ner.Date.DatePointerType.Today) 
                            return null;
                        _addItems(fr, res.ItemsFrom, tt);
                    }
                    Pullenti.Ner.Date.DateReferent to = drr.DateTo;
                    if (to != null) 
                    {
                        if (to.Pointer == Pullenti.Ner.Date.DatePointerType.Today) 
                            return null;
                        _addItems(to, res.ItemsTo, tt);
                    }
                    bool hasYear = false;
                    if (res.ItemsFrom.Count > 0 && res.ItemsFrom[0].Typ == DateExItemTokenType.Year) 
                        hasYear = true;
                    else if (res.ItemsTo.Count > 0 && res.ItemsTo[0].Typ == DateExItemTokenType.Year) 
                        hasYear = true;
                    if (!hasYear && (tt.WhitespacesAfterCount < 3)) 
                    {
                        DateExItemToken dit = DateExItemToken.TryParse(tt.Next, (res.ItemsTo.Count > 0 ? res.ItemsTo : res.ItemsFrom), 0);
                        if (dit != null && dit.Typ == DateExItemTokenType.Year) 
                        {
                            if (res.ItemsFrom.Count > 0) 
                                res.ItemsFrom.Insert(0, dit);
                            if (res.ItemsTo.Count > 0) 
                                res.ItemsTo.Insert(0, dit);
                            res.EndToken = dit.EndToken;
                        }
                    }
                    return res;
                }
                Pullenti.Ner.Date.DateReferent dr = tt.GetReferent() as Pullenti.Ner.Date.DateReferent;
                if (dr != null) 
                {
                    if (dr.Pointer == Pullenti.Ner.Date.DatePointerType.Today) 
                        return null;
                    if (res == null) 
                        res = new DateExToken(t, tt);
                    List<DateExItemToken> li = new List<DateExItemToken>();
                    _addItems(dr, li, tt);
                    if (li.Count == 0) 
                        continue;
                    if (toRegime) 
                    {
                        bool ok = true;
                        foreach (DateExItemToken v in li) 
                        {
                            foreach (DateExItemToken vv in res.ItemsTo) 
                            {
                                if (vv.Typ == v.Typ) 
                                    ok = false;
                            }
                        }
                        if (!ok) 
                            break;
                        res.ItemsTo.AddRange(li);
                        res.EndToken = tt;
                    }
                    else 
                    {
                        bool ok = true;
                        foreach (DateExItemToken v in li) 
                        {
                            foreach (DateExItemToken vv in res.ItemsFrom) 
                            {
                                if (vv.Typ == v.Typ) 
                                    ok = false;
                            }
                        }
                        if (!ok) 
                            break;
                        res.ItemsFrom.AddRange(li);
                        res.EndToken = tt;
                    }
                    bool hasYear = false;
                    if (res.ItemsFrom.Count > 0 && res.ItemsFrom[0].Typ == DateExItemTokenType.Year) 
                        hasYear = true;
                    else if (res.ItemsTo.Count > 0 && res.ItemsTo[0].Typ == DateExItemTokenType.Year) 
                        hasYear = true;
                    if (!hasYear && (tt.WhitespacesAfterCount < 3)) 
                    {
                        DateExItemToken dit = DateExItemToken.TryParse(tt.Next, null, 0);
                        if (dit != null && dit.Typ == DateExItemTokenType.Year) 
                        {
                            if (res.ItemsFrom.Count > 0) 
                                res.ItemsFrom.Insert(0, dit);
                            if (res.ItemsTo.Count > 0) 
                                res.ItemsTo.Insert(0, dit);
                            tt = (res.EndToken = dit.EndToken);
                        }
                    }
                    continue;
                }
                if (tt.Morph.Class.IsPreposition) 
                {
                    if (tt.IsValue("ПО", null) || tt.IsValue("ДО", null)) 
                    {
                        toRegime = true;
                        if (t0 == null) 
                            t0 = tt;
                    }
                    else if (tt.IsValue("С", null) || tt.IsValue("ОТ", null)) 
                    {
                        fromRegime = true;
                        if (t0 == null) 
                            t0 = tt;
                    }
                    continue;
                }
                DateExItemToken it = DateExItemToken.TryParse(tt, (res == null ? null : (toRegime ? res.ItemsTo : res.ItemsFrom)), 0);
                if (it == null) 
                    break;
                if (tt.IsValue("ДЕНЬ", null) && tt.Next != null && tt.Next.IsValue("НЕДЕЛЯ", null)) 
                    break;
                if (it.EndToken == tt && ((it.Typ == DateExItemTokenType.Hour || it.Typ == DateExItemTokenType.Minute))) 
                {
                    if (tt.Previous == null || !tt.Previous.Morph.Class.IsPreposition) 
                        break;
                }
                if (res == null) 
                    res = new DateExToken(t, tt);
                if (toRegime) 
                    res.ItemsTo.Add(it);
                else 
                    res.ItemsFrom.Add(it);
                tt = it.EndToken;
                res.EndToken = tt;
            }
            if (res != null) 
            {
                if (t0 != null && res.BeginToken.Previous == t0) 
                    res.BeginToken = t0;
                res.IsDiap = fromRegime || toRegime;
                res.ItemsFrom.Sort();
                res.ItemsTo.Sort();
            }
            return res;
        }
        static void _addItems(Pullenti.Ner.Date.DateReferent fr, List<DateExItemToken> res, Pullenti.Ner.Token tt)
        {
            if (fr.Year > 0) 
                res.Add(new DateExItemToken(tt, tt) { Typ = DateExItemTokenType.Year, Value = fr.Year, Src = fr });
            else if (fr.Pointer == Pullenti.Ner.Date.DatePointerType.Today) 
                res.Add(new DateExItemToken(tt, tt) { Typ = DateExItemTokenType.Year, Value = 0, IsValueRelate = true });
            if (fr.Month > 0) 
                res.Add(new DateExItemToken(tt, tt) { Typ = DateExItemTokenType.Month, Value = fr.Month, Src = fr });
            else if (fr.Pointer == Pullenti.Ner.Date.DatePointerType.Today) 
                res.Add(new DateExItemToken(tt, tt) { Typ = DateExItemTokenType.Month, Value = 0, IsValueRelate = true });
            if (fr.Day > 0) 
                res.Add(new DateExItemToken(tt, tt) { Typ = DateExItemTokenType.Day, Value = fr.Day, Src = fr });
            else if (fr.Pointer == Pullenti.Ner.Date.DatePointerType.Today) 
                res.Add(new DateExItemToken(tt, tt) { Typ = DateExItemTokenType.Day, Value = 0, IsValueRelate = true });
            if (fr.FindSlot(Pullenti.Ner.Date.DateReferent.ATTR_HOUR, null, true) != null) 
                res.Add(new DateExItemToken(tt, tt) { Typ = DateExItemTokenType.Hour, Value = fr.Hour, Src = fr });
            else if (fr.Pointer == Pullenti.Ner.Date.DatePointerType.Today) 
                res.Add(new DateExItemToken(tt, tt) { Typ = DateExItemTokenType.Hour, Value = 0, IsValueRelate = true });
            if (fr.FindSlot(Pullenti.Ner.Date.DateReferent.ATTR_MINUTE, null, true) != null) 
                res.Add(new DateExItemToken(tt, tt) { Typ = DateExItemTokenType.Minute, Value = fr.Minute, Src = fr });
            else if (fr.Pointer == Pullenti.Ner.Date.DatePointerType.Today) 
                res.Add(new DateExItemToken(tt, tt) { Typ = DateExItemTokenType.Minute, Value = 0, IsValueRelate = true });
        }
        public class DateExItemToken : Pullenti.Ner.MetaToken, IComparable<DateExItemToken>
        {
            public DateExItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
            {
            }
            public Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType Typ = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Undefined;
            /// <summary>
            /// Начало и конец диапазона, при совпадении значение точное
            /// </summary>
            public int Value;
            /// <summary>
            /// Признак относительности значения (относительно текущей даты)
            /// </summary>
            public bool IsValueRelate;
            /// <summary>
            /// Признак того, что значение примерное (в начале года)
            /// </summary>
            public bool IsValueNotstrict;
            public Pullenti.Ner.Date.DateReferent Src;
            public override string ToString()
            {
                StringBuilder tmp = new StringBuilder();
                tmp.AppendFormat("{0} ", Typ);
                if (IsValueNotstrict) 
                    tmp.Append("~");
                if (IsValueRelate) 
                    tmp.AppendFormat("{0}{1}", (Value < 0 ? "" : "+"), Value);
                else 
                    tmp.Append(Value);
                return tmp.ToString();
            }
            public static DateExItemToken TryParse(Pullenti.Ner.Token t, List<DateExItemToken> prev, int level = 0)
            {
                if (t == null || level > 10) 
                    return null;
                if (t.IsValue("СЕГОДНЯ", null)) 
                    return new DateExItemToken(t, t) { Typ = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Day, Value = 0, IsValueRelate = true };
                if (t.IsValue("ЗАВТРА", null)) 
                    return new DateExItemToken(t, t) { Typ = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Day, Value = 1, IsValueRelate = true };
                if (t.IsValue("ПОСЛЕЗАВТРА", null)) 
                    return new DateExItemToken(t, t) { Typ = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Day, Value = 2, IsValueRelate = true };
                if (t.IsValue("ВЧЕРА", null)) 
                    return new DateExItemToken(t, t) { Typ = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Day, Value = -1, IsValueRelate = true };
                if (t.IsValue("ПОЗАВЧЕРА", null)) 
                    return new DateExItemToken(t, t) { Typ = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Day, Value = -2, IsValueRelate = true };
                if (t.IsValue("ПОЛЧАСА", null)) 
                    return new DateExItemToken(t, t) { Typ = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Minute, Value = 30, IsValueRelate = true };
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                if (npt == null) 
                {
                    if ((t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).IntValue != null) 
                    {
                        DateExItemToken res0 = TryParse(t.Next, prev, level + 1);
                        if (res0 != null && ((res0.Value == 1 || res0.Value == 0))) 
                        {
                            res0.BeginToken = t;
                            res0.Value = (t as Pullenti.Ner.NumberToken).IntValue.Value;
                            if (t.Previous != null && ((t.Previous.IsValue("ЧЕРЕЗ", null) || t.Previous.IsValue("СПУСТЯ", null)))) 
                                res0.IsValueRelate = true;
                            else if (res0.EndToken.Next != null) 
                            {
                                if (res0.EndToken.Next.IsValue("СПУСТЯ", null)) 
                                {
                                    res0.IsValueRelate = true;
                                    res0.EndToken = res0.EndToken.Next;
                                }
                                else if (res0.EndToken.Next.IsValue("НАЗАД", null)) 
                                {
                                    res0.IsValueRelate = true;
                                    res0.Value = -res0.Value;
                                    res0.EndToken = res0.EndToken.Next;
                                }
                                else if (res0.EndToken.Next.IsValue("ТОМУ", null) && res0.EndToken.Next.Next != null && res0.EndToken.Next.Next.IsValue("НАЗАД", null)) 
                                {
                                    res0.IsValueRelate = true;
                                    res0.Value = -res0.Value;
                                    res0.EndToken = res0.EndToken.Next.Next;
                                }
                            }
                            return res0;
                        }
                        Pullenti.Ner.Date.Internal.DateItemToken dtt = Pullenti.Ner.Date.Internal.DateItemToken.TryAttach(t, null, false);
                        if (dtt != null && dtt.Typ == Pullenti.Ner.Date.Internal.DateItemToken.DateItemType.Year) 
                            return new DateExItemToken(t, dtt.EndToken) { Typ = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Year, Value = dtt.IntValue };
                        if (t.Next != null && t.Next.IsValue("ЧИСЛО", null)) 
                        {
                            DateExItemToken ne = TryParse(t.Next.Next, prev, level + 1);
                            if (ne != null && ne.Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Month) 
                                return new DateExItemToken(t, t.Next) { Typ = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Day, Value = (t as Pullenti.Ner.NumberToken).IntValue.Value };
                        }
                    }
                    return null;
                }
                Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Hour;
                int val = 0;
                if (npt.Noun.IsValue("ГОД", null) || npt.Noun.IsValue("ГОДИК", null) || npt.Noun.IsValue("ЛЕТ", null)) 
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Year;
                else if (npt.Noun.IsValue("КВАРТАЛ", null)) 
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Quartal;
                else if (npt.Noun.IsValue("МЕСЯЦ", null)) 
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Month;
                else if (npt.Noun.IsValue("ДЕНЬ", null) || npt.Noun.IsValue("ДЕНЕК", null)) 
                {
                    if (npt.EndToken.Next != null && npt.EndToken.Next.IsValue("НЕДЕЛЯ", null)) 
                        return null;
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Day;
                }
                else if (npt.Noun.IsValue("ЧИСЛО", null) && npt.Adjectives.Count > 0 && (npt.Adjectives[0].BeginToken is Pullenti.Ner.NumberToken)) 
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Day;
                else if (npt.Noun.IsValue("НЕДЕЛЯ", null) || npt.Noun.IsValue("НЕДЕЛЬКА", null)) 
                {
                    if (t.Previous != null && t.Previous.IsValue("ДЕНЬ", null)) 
                        return null;
                    if (t.Previous != null && ((t.Previous.IsValue("ЗА", null) || t.Previous.IsValue("НА", null)))) 
                        ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Week;
                    else if (t.IsValue("ЗА", null) || t.IsValue("НА", null)) 
                        ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Week;
                    else 
                    {
                        ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Day;
                        val = 7;
                    }
                }
                else if (npt.Noun.IsValue("ВЫХОДНОЙ", null)) 
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Weekend;
                else if (npt.Noun.IsValue("ЧАС", null) || npt.Noun.IsValue("ЧАСИК", null) || npt.Noun.IsValue("ЧАСОК", null)) 
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Hour;
                else if (npt.Noun.IsValue("МИНУТА", null) || npt.Noun.IsValue("МИНУТКА", null)) 
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Minute;
                else if (npt.Noun.IsValue("ПОНЕДЕЛЬНИК", null)) 
                {
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek;
                    val = 1;
                }
                else if (npt.Noun.IsValue("ВТОРНИК", null)) 
                {
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek;
                    val = 2;
                }
                else if (npt.Noun.IsValue("СРЕДА", null)) 
                {
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek;
                    val = 3;
                }
                else if (npt.Noun.IsValue("ЧЕТВЕРГ", null)) 
                {
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek;
                    val = 4;
                }
                else if (npt.Noun.IsValue("ПЯТНИЦА", null)) 
                {
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek;
                    val = 5;
                }
                else if (npt.Noun.IsValue("СУББОТА", null)) 
                {
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek;
                    val = 6;
                }
                else if (npt.Noun.IsValue("ВОСКРЕСЕНЬЕ", null) || npt.Noun.IsValue("ВОСКРЕСЕНИЕ", null)) 
                {
                    ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek;
                    val = 7;
                }
                else 
                {
                    Pullenti.Ner.Date.Internal.DateItemToken dti = Pullenti.Ner.Date.Internal.DateItemToken.TryAttach(npt.EndToken, null, false);
                    if (dti != null && dti.Typ == Pullenti.Ner.Date.Internal.DateItemToken.DateItemType.Month) 
                    {
                        ty = Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Month;
                        val = dti.IntValue;
                    }
                    else 
                        return null;
                }
                DateExItemToken res = new DateExItemToken(t, npt.EndToken) { Typ = ty, Value = val };
                bool heg = false;
                foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
                {
                    if (a.IsValue("СЛЕДУЮЩИЙ", null) || a.IsValue("БУДУЩИЙ", null) || a.IsValue("БЛИЖАЙШИЙ", null)) 
                    {
                        if (res.Value == 0 && ty != Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.Weekend) 
                            res.Value = 1;
                        res.IsValueRelate = true;
                    }
                    else if (a.IsValue("ПРЕДЫДУЩИЙ", null) || a.IsValue("ПРОШЛЫЙ", null) || a.IsValue("ПРОШЕДШИЙ", null)) 
                    {
                        if (res.Value == 0) 
                            res.Value = 1;
                        res.IsValueRelate = true;
                        heg = true;
                    }
                    else if (a.IsValue("ПОЗАПРОШЛЫЙ", null)) 
                    {
                        if (res.Value == 0) 
                            res.Value = 2;
                        res.IsValueRelate = true;
                        heg = true;
                    }
                    else if (a.BeginToken == a.EndToken && (a.BeginToken is Pullenti.Ner.NumberToken) && (a.BeginToken as Pullenti.Ner.NumberToken).IntValue != null) 
                    {
                        if (res.Typ != Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek) 
                            res.Value = (a.BeginToken as Pullenti.Ner.NumberToken).IntValue.Value;
                    }
                    else if (a.IsValue("ЭТОТ", null) || a.IsValue("ТЕКУЩИЙ", null)) 
                        res.IsValueRelate = true;
                    else if (a.IsValue("БЛИЖАЙШИЙ", null) && res.Typ == Pullenti.Ner.Date.Internal.DateExToken.DateExItemTokenType.DayOfWeek) 
                    {
                    }
                    else 
                        return null;
                }
                if (npt.Anafor != null) 
                {
                    if (npt.Anafor.IsValue("ЭТОТ", null)) 
                    {
                        if (npt.Morph.Number != Pullenti.Morph.MorphNumber.Singular) 
                            return null;
                        if (res.Value == 0) 
                            res.IsValueRelate = true;
                        if (prev == null || prev.Count == 0) 
                        {
                            if (t.Previous != null && t.Previous.GetMorphClassInDictionary().IsPreposition) 
                            {
                            }
                            else if (t.GetMorphClassInDictionary().IsPreposition) 
                            {
                            }
                            else 
                                return null;
                        }
                    }
                    else 
                        return null;
                }
                if (heg) 
                    res.Value = -res.Value;
                if (t.Previous != null) 
                {
                    if (t.Previous.IsValue("ЧЕРЕЗ", null) || t.Previous.IsValue("СПУСТЯ", null)) 
                    {
                        res.IsValueRelate = true;
                        if (res.Value == 0) 
                            res.Value = 1;
                        res.BeginToken = t.Previous;
                    }
                    else if (t.Previous.IsValue("ЗА", null) && res.Value == 0) 
                    {
                        if (!npt.Morph.Case.IsAccusative) 
                            return null;
                        if (npt.EndToken.Next != null && npt.EndToken.Next.IsValue("ДО", null)) 
                            return null;
                        if (npt.BeginToken == npt.EndToken) 
                            return null;
                        res.IsValueRelate = true;
                    }
                }
                return res;
            }
            public int CompareTo(DateExItemToken other)
            {
                if (((int)Typ) < ((int)other.Typ)) 
                    return -1;
                if (((int)Typ) > ((int)other.Typ)) 
                    return 1;
                return 0;
            }
        }

        public enum DateExItemTokenType : int
        {
            Undefined = 0,
            Century = 1,
            Year = 2,
            Quartal = 3,
            Month = 4,
            Week = 5,
            Day = 6,
            /// <summary>
            /// День недели
            /// </summary>
            DayOfWeek = 7,
            Hour = 8,
            Minute = 9,
            /// <summary>
            /// Выходные
            /// </summary>
            Weekend = 10,
        }

    }
}