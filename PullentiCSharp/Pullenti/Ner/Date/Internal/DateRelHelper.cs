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

namespace Pullenti.Ner.Date.Internal
{
    static class DateRelHelper
    {
        public static List<Pullenti.Ner.ReferentToken> CreateReferents(DateExToken et)
        {
            if (!et.IsDiap || et.ItemsTo.Count == 0) 
            {
                List<Pullenti.Ner.ReferentToken> li = _createRefs(et.ItemsFrom);
                if (li == null || li.Count == 0) 
                    return null;
                return li;
            }
            List<Pullenti.Ner.ReferentToken> liFr = _createRefs(et.ItemsFrom);
            List<Pullenti.Ner.ReferentToken> liTo = _createRefs(et.ItemsTo);
            Pullenti.Ner.Date.DateRangeReferent ra = new Pullenti.Ner.Date.DateRangeReferent();
            if (liFr.Count > 0) 
                ra.DateFrom = liFr[0].Tag as Pullenti.Ner.Date.DateReferent;
            if (liTo.Count > 0) 
                ra.DateTo = liTo[0].Tag as Pullenti.Ner.Date.DateReferent;
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            res.AddRange(liFr);
            res.AddRange(liTo);
            res.Add(new Pullenti.Ner.ReferentToken(ra, et.BeginToken, et.EndToken));
            if (res.Count == 0) 
                return null;
            res[0].Tag = ra;
            return res;
        }
        static List<Pullenti.Ner.ReferentToken> _createRefs(List<DateExToken.DateExItemToken> its)
        {
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            Pullenti.Ner.Date.DateReferent own = null;
            for (int i = 0; i < its.Count; i++) 
            {
                DateExToken.DateExItemToken it = its[i];
                Pullenti.Ner.Date.DateReferent d = new Pullenti.Ner.Date.DateReferent();
                if (it.IsValueRelate) 
                    d.IsRelative = true;
                if (own != null) 
                    d.Higher = own;
                if (it.Typ == DateExToken.DateExItemTokenType.Day) 
                    d.Day = it.Value;
                else if (it.Typ == DateExToken.DateExItemTokenType.DayOfWeek) 
                    d.DayOfWeek = it.Value;
                else if (it.Typ == DateExToken.DateExItemTokenType.Hour) 
                {
                    d.Hour = it.Value;
                    if (((i + 1) < its.Count) && its[i + 1].Typ == DateExToken.DateExItemTokenType.Minute && !its[i + 1].IsValueRelate) 
                    {
                        d.Minute = its[i + 1].Value;
                        i++;
                    }
                }
                else if (it.Typ == DateExToken.DateExItemTokenType.Minute) 
                    d.Minute = it.Value;
                else if (it.Typ == DateExToken.DateExItemTokenType.Month) 
                    d.Month = it.Value;
                else if (it.Typ == DateExToken.DateExItemTokenType.Quartal) 
                    d.Quartal = it.Value;
                else if (it.Typ == DateExToken.DateExItemTokenType.Week) 
                    d.Week = it.Value;
                else if (it.Typ == DateExToken.DateExItemTokenType.Year) 
                    d.Year = it.Value;
                else 
                    continue;
                res.Add(new Pullenti.Ner.ReferentToken(d, it.BeginToken, it.EndToken));
                own = d;
                it.Src = d;
            }
            if (res.Count > 0) 
                res[0].Tag = own;
            return res;
        }
        static List<DateExToken.DateExItemToken> _createDateEx(Pullenti.Ner.Date.DateReferent dr)
        {
            List<DateExToken.DateExItemToken> res = new List<DateExToken.DateExItemToken>();
            for (; dr != null; dr = dr.Higher) 
            {
                int n;
                foreach (Pullenti.Ner.Slot s in dr.Slots) 
                {
                    DateExToken.DateExItemToken it = new DateExToken.DateExItemToken(null, null) { Typ = DateExToken.DateExItemTokenType.Undefined };
                    if (dr.GetStringValue(Pullenti.Ner.Date.DateReferent.ATTR_ISRELATIVE) == "true") 
                        it.IsValueRelate = true;
                    if (s.TypeName == Pullenti.Ner.Date.DateReferent.ATTR_YEAR) 
                    {
                        it.Typ = DateExToken.DateExItemTokenType.Year;
                        if (int.TryParse(s.Value as string, out n)) 
                            it.Value = n;
                    }
                    else if (s.TypeName == Pullenti.Ner.Date.DateReferent.ATTR_QUARTAL) 
                    {
                        it.Typ = DateExToken.DateExItemTokenType.Quartal;
                        if (int.TryParse(s.Value as string, out n)) 
                            it.Value = n;
                    }
                    else if (s.TypeName == Pullenti.Ner.Date.DateReferent.ATTR_MONTH) 
                    {
                        it.Typ = DateExToken.DateExItemTokenType.Month;
                        if (int.TryParse(s.Value as string, out n)) 
                            it.Value = n;
                    }
                    else if (s.TypeName == Pullenti.Ner.Date.DateReferent.ATTR_WEEK) 
                    {
                        it.Typ = DateExToken.DateExItemTokenType.Week;
                        if (int.TryParse(s.Value as string, out n)) 
                            it.Value = n;
                    }
                    else if (s.TypeName == Pullenti.Ner.Date.DateReferent.ATTR_DAYOFWEEK) 
                    {
                        it.Typ = DateExToken.DateExItemTokenType.DayOfWeek;
                        if (int.TryParse(s.Value as string, out n)) 
                            it.Value = n;
                    }
                    else if (s.TypeName == Pullenti.Ner.Date.DateReferent.ATTR_DAY) 
                    {
                        it.Typ = DateExToken.DateExItemTokenType.Day;
                        if (int.TryParse(s.Value as string, out n)) 
                            it.Value = n;
                    }
                    else if (s.TypeName == Pullenti.Ner.Date.DateReferent.ATTR_HOUR) 
                    {
                        it.Typ = DateExToken.DateExItemTokenType.Hour;
                        if (int.TryParse(s.Value as string, out n)) 
                            it.Value = n;
                    }
                    else if (s.TypeName == Pullenti.Ner.Date.DateReferent.ATTR_MINUTE) 
                    {
                        it.Typ = DateExToken.DateExItemTokenType.Minute;
                        if (int.TryParse(s.Value as string, out n)) 
                            it.Value = n;
                    }
                    if (it.Typ != DateExToken.DateExItemTokenType.Undefined) 
                        res.Insert(0, it);
                }
            }
            res.Sort();
            return res;
        }
        public static DateTime? CalculateDate(Pullenti.Ner.Date.DateReferent dr, DateTime now, int tense)
        {
            if (dr.Pointer == Pullenti.Ner.Date.DatePointerType.Today) 
                return now;
            if (!dr.IsRelative && dr.Dt != null) 
                return dr.Dt;
            DateExToken det = new DateExToken(null, null);
            det.ItemsFrom = _createDateEx(dr);
            return det.GetDate(now, tense);
        }
        public static bool CalculateDateRange(Pullenti.Ner.Date.DateReferent dr, DateTime now, out DateTime from, out DateTime to, int tense)
        {
            if (dr.Pointer == Pullenti.Ner.Date.DatePointerType.Today) 
            {
                from = now;
                to = now;
                return true;
            }
            if (!dr.IsRelative && dr.Dt != null) 
            {
                from = (to = dr.Dt.Value);
                return true;
            }
            DateExToken det = new DateExToken(null, null);
            det.ItemsFrom = _createDateEx(dr);
            return det.GetDates(now, out from, out to, tense);
        }
        public static bool CalculateDateRange2(Pullenti.Ner.Date.DateRangeReferent dr, DateTime now, out DateTime from, out DateTime to, int tense)
        {
            from = DateTime.MinValue;
            to = DateTime.MaxValue;
            DateTime dt0;
            DateTime dt1;
            if (dr.DateFrom == null) 
            {
                if (dr.DateTo == null) 
                    return false;
                if (!CalculateDateRange(dr.DateTo, now, out dt0, out dt1, tense)) 
                    return false;
                to = dt1;
                return true;
            }
            else if (dr.DateTo == null) 
            {
                if (!CalculateDateRange(dr.DateFrom, now, out dt0, out dt1, tense)) 
                    return false;
                from = dt0;
                return true;
            }
            if (!CalculateDateRange(dr.DateFrom, now, out dt0, out dt1, tense)) 
                return false;
            from = dt0;
            DateTime dt2;
            DateTime dt3;
            if (!CalculateDateRange(dr.DateTo, now, out dt2, out dt3, tense)) 
                return false;
            to = dt3;
            return true;
        }
        public static void AppendToString(Pullenti.Ner.Date.DateReferent dr, StringBuilder res)
        {
            DateTime dt0;
            DateTime dt1;
            DateTime cur = (Pullenti.Ner.ProcessorService.DebugCurrentDateTime == null ? DateTime.Now : Pullenti.Ner.ProcessorService.DebugCurrentDateTime.Value);
            if (!DateRelHelper.CalculateDateRange(dr, cur, out dt0, out dt1, 0)) 
                return;
            _appendDates(cur, dt0, dt1, res);
        }
        public static void AppendToString2(Pullenti.Ner.Date.DateRangeReferent dr, StringBuilder res)
        {
            DateTime dt0;
            DateTime dt1;
            DateTime cur = (Pullenti.Ner.ProcessorService.DebugCurrentDateTime == null ? DateTime.Now : Pullenti.Ner.ProcessorService.DebugCurrentDateTime.Value);
            if (!DateRelHelper.CalculateDateRange2(dr, cur, out dt0, out dt1, 0)) 
                return;
            _appendDates(cur, dt0, dt1, res);
        }
        static void _appendDates(DateTime cur, DateTime dt0, DateTime dt1, StringBuilder res)
        {
            int mon0 = dt0.Month;
            res.AppendFormat(" ({0}.{1}.{2}", dt0.Year, mon0.ToString("D02"), dt0.Day.ToString("D02"));
            if (dt0.Hour > 0 || dt0.Minute > 0) 
                res.AppendFormat(" {0}:{1}", dt0.Hour.ToString("D02"), dt0.Minute.ToString("D02"));
            if (dt0 != dt1) 
            {
                int mon1 = dt1.Month;
                res.AppendFormat("-{0}.{1}.{2}", dt1.Year, mon1.ToString("D02"), dt1.Day.ToString("D02"));
                if (dt1.Hour > 0 || dt1.Minute > 0) 
                    res.AppendFormat(" {0}:{1}", dt1.Hour.ToString("D02"), dt1.Minute.ToString("D02"));
            }
            int monc = cur.Month;
            res.AppendFormat(" отн. {0}.{1}.{2}", cur.Year, monc.ToString("D02"), cur.Day.ToString("D02"));
            if (cur.Hour > 0 || cur.Minute > 0) 
                res.AppendFormat(" {0}:{1}", cur.Hour.ToString("D02"), cur.Minute.ToString("D02"));
            res.Append(")");
        }
    }
}