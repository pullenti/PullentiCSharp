/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Date.Internal
{
    // Примитив, из которых состоит дата
    public class DateItemToken : Pullenti.Ner.MetaToken
    {
        public DateItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public enum DateItemType : int
        {
            Number,
            Year,
            Month,
            Delim,
            Hour,
            Minute,
            Second,
            Halfyear,
            Quartal,
            Pointer,
            Century,
        }

        public DateItemType Typ;
        public string StringValue;
        public int IntValue;
        public Pullenti.Morph.MorphLang Lang;
        public int NewAge = 0;
        public override string ToString()
        {
            return string.Format("{0} {1}", Typ.ToString(), (IntValue == 0 ? StringValue : IntValue.ToString()));
        }
        public int Year
        {
            get
            {
                if (m_Year > 0) 
                    return m_Year;
                if (IntValue == 0) 
                    return 0;
                if (NewAge == 0) 
                {
                    if (IntValue < 16) 
                        return 2000 + IntValue;
                    if (IntValue <= ((DateTime.Today.Year - 2000) + 5)) 
                        return 2000 + IntValue;
                    if (IntValue < 100) 
                        return 1900 + IntValue;
                }
                return IntValue;
            }
            set
            {
                m_Year = value;
            }
        }
        int m_Year = -1;
        public int Year0
        {
            get
            {
                if (NewAge < 0) 
                    return -Year;
                return Year;
            }
        }
        public bool CanBeYear
        {
            get
            {
                if (Typ == DateItemType.Year) 
                    return true;
                if (Typ == DateItemType.Month || Typ == DateItemType.Quartal || Typ == DateItemType.Halfyear) 
                    return false;
                if (IntValue >= 50 && (IntValue < 100)) 
                    return true;
                if ((IntValue < 1000) || IntValue > 2100) 
                    return false;
                return true;
            }
        }
        public bool CanByMonth
        {
            get
            {
                if (m_CanByMonth >= 0) 
                    return m_CanByMonth == 1;
                if (Typ == DateItemType.Month) 
                    return true;
                if (Typ == DateItemType.Quartal || Typ == DateItemType.Halfyear || Typ == DateItemType.Pointer) 
                    return false;
                return IntValue > 0 && IntValue <= 12;
            }
            set
            {
                m_CanByMonth = (value ? 1 : 0);
            }
        }
        int m_CanByMonth = -1;
        public bool CanBeDay
        {
            get
            {
                if ((Typ == DateItemType.Month || Typ == DateItemType.Quartal || Typ == DateItemType.Halfyear) || Typ == DateItemType.Pointer) 
                    return false;
                return IntValue > 0 && IntValue <= 31;
            }
        }
        public bool CanBeHour
        {
            get
            {
                if (Typ != DateItemType.Number) 
                    return Typ == DateItemType.Hour;
                if (LengthChar != 2) 
                {
                    if (LengthChar == 1 && IntValue == 0) 
                        return true;
                    return false;
                }
                return IntValue >= 0 && (IntValue < 24);
            }
        }
        public bool CanBeMinute
        {
            get
            {
                if (Typ != DateItemType.Number) 
                    return Typ == DateItemType.Minute;
                if (LengthChar != 2) 
                    return false;
                return IntValue >= 0 && (IntValue < 60);
            }
        }
        public bool IsZeroHeaded
        {
            get
            {
                return Kit.Sofa.Text[BeginChar] == '0';
            }
        }
        public static DateItemToken TryAttach(Pullenti.Ner.Token t, List<DateItemToken> prev, bool detailRegime = false)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            if (t0.IsChar('_')) 
            {
                for (t = t.Next; t != null; t = t.Next) 
                {
                    if (t.IsNewlineBefore) 
                        return null;
                    if (!t.IsChar('_')) 
                        break;
                }
            }
            else if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t0, true, false)) 
            {
                bool ok = false;
                for (t = t.Next; t != null; t = t.Next) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t, true, t0, false)) 
                    {
                        ok = true;
                        break;
                    }
                    else if (!t.IsChar('_')) 
                        break;
                }
                if (!ok) 
                    t = t0;
                else 
                    for (t = t.Next; t != null; t = t.Next) 
                    {
                        if (!t.IsChar('_')) 
                            break;
                    }
            }
            else if ((t0 is Pullenti.Ner.TextToken) && t0.IsValue("THE", null)) 
            {
                DateItemToken res0 = _TryAttach(t.Next, prev, detailRegime);
                if (res0 != null) 
                {
                    res0.BeginToken = t;
                    return res0;
                }
            }
            DateItemToken res = _TryAttach(t, prev, detailRegime);
            if (res == null) 
                return null;
            res.BeginToken = t0;
            if (!res.IsWhitespaceAfter && res.EndToken.Next != null && res.EndToken.Next.IsChar('_')) 
            {
                for (t = res.EndToken.Next; t != null; t = t.Next) 
                {
                    if (!t.IsChar('_')) 
                        break;
                    else 
                        res.EndToken = t;
                }
            }
            if (res.Typ == DateItemType.Year || res.Typ == DateItemType.Century || res.Typ == DateItemType.Number) 
            {
                Pullenti.Ner.Core.TerminToken tok = null;
                int ii = 0;
                t = res.EndToken.Next;
                if (t != null && t.IsValue("ДО", null)) 
                {
                    tok = m_NewAge.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                    ii = -1;
                }
                else if (t != null && t.IsValue("ОТ", "ВІД")) 
                {
                    tok = m_NewAge.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                    ii = 1;
                }
                else 
                {
                    tok = m_NewAge.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                    ii = 1;
                }
                if (tok != null) 
                {
                    res.NewAge = (ii < 0 ? -1 : 1);
                    res.EndToken = tok.EndToken;
                    if (res.Typ == DateItemType.Number) 
                        res.Typ = DateItemType.Year;
                }
            }
            return res;
        }
        static bool _isNewAge(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            if (t.IsValue("ДО", null)) 
                return m_NewAge.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No) != null;
            else if (t.IsValue("ОТ", "ВІД")) 
                return m_NewAge.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No) != null;
            return m_NewAge.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No) != null;
        }
        static DateItemToken _TryAttach(Pullenti.Ner.Token t, List<DateItemToken> prev, bool detailRegime)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
            Pullenti.Ner.Token begin = t;
            Pullenti.Ner.Token end = t;
            bool isInBrack = false;
            if ((Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false) && t.Next != null && (t.Next is Pullenti.Ner.NumberToken)) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t.Next.Next, false, null, false)) 
            {
                nt = t.Next as Pullenti.Ner.NumberToken;
                end = t.Next.Next;
                isInBrack = true;
            }
            if ((t.IsNewlineBefore && Pullenti.Ner.Core.BracketHelper.IsBracket(t, false) && (t.Next is Pullenti.Ner.NumberToken)) && Pullenti.Ner.Core.BracketHelper.IsBracket(t.Next.Next, false)) 
            {
                nt = t.Next as Pullenti.Ner.NumberToken;
                end = t.Next.Next;
                isInBrack = true;
            }
            if (nt != null) 
            {
                if (nt.IntValue == null) 
                    return null;
                if (nt.Typ == Pullenti.Ner.NumberSpellingType.Words) 
                {
                    if (nt.Morph.Class.IsNoun && !nt.Morph.Class.IsAdjective) 
                    {
                        if (t.Next != null && ((t.Next.IsValue("КВАРТАЛ", null) || t.Next.IsValue("ПОЛУГОДИЕ", null) || t.Next.IsValue("ПІВРІЧЧЯ", null)))) 
                        {
                        }
                        else 
                            return null;
                    }
                }
                if (Pullenti.Ner.Core.NumberHelper.TryParseAge(nt) != null) 
                    return null;
                Pullenti.Ner.Token tt;
                DateItemToken res = new DateItemToken(begin, end) { Typ = DateItemType.Number, IntValue = nt.IntValue.Value, Morph = nt.Morph };
                if ((res.IntValue == 20 && (nt.Next is Pullenti.Ner.NumberToken) && (nt.Next as Pullenti.Ner.NumberToken).IntValue != null) && nt.Next.LengthChar == 2 && prev != null) 
                {
                    int num = 2000 + (nt.Next as Pullenti.Ner.NumberToken).IntValue.Value;
                    if ((num < 2030) && prev.Count > 0 && prev[prev.Count - 1].Typ == DateItemType.Month) 
                    {
                        bool ok = false;
                        if (nt.WhitespacesAfterCount == 1) 
                            ok = true;
                        else if (nt.IsNewlineAfter && nt.IsNewlineAfter) 
                            ok = true;
                        if (ok) 
                        {
                            nt = nt.Next as Pullenti.Ner.NumberToken;
                            res.EndToken = nt;
                            res.IntValue = num;
                        }
                    }
                }
                if (res.IntValue == 20 || res.IntValue == 201) 
                {
                    tt = t.Next;
                    if (tt != null && tt.IsChar('_')) 
                    {
                        for (; tt != null; tt = tt.Next) 
                        {
                            if (!tt.IsChar('_')) 
                                break;
                        }
                        tt = TestYearRusWord(tt, false);
                        if (tt != null) 
                        {
                            res.IntValue = 0;
                            res.EndToken = tt;
                            res.Typ = DateItemType.Year;
                            return res;
                        }
                    }
                }
                if (res.IntValue <= 12 && t.Next != null && (t.WhitespacesAfterCount < 3)) 
                {
                    tt = t.Next;
                    if (tt.IsValue("ЧАС", null)) 
                    {
                        if (((t.Previous is Pullenti.Ner.TextToken) && !t.Previous.Chars.IsLetter && !t.IsWhitespaceBefore) && (t.Previous.Previous is Pullenti.Ner.NumberToken) && !t.Previous.IsWhitespaceBefore) 
                        {
                        }
                        else 
                        {
                            res.Typ = DateItemType.Hour;
                            res.EndToken = tt;
                            tt = tt.Next;
                            if (tt != null && tt.IsChar('.')) 
                            {
                                res.EndToken = tt;
                                tt = tt.Next;
                            }
                        }
                    }
                    for (; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsValue("УТРО", "РАНОК")) 
                        {
                            res.EndToken = tt;
                            res.Typ = DateItemType.Hour;
                            return res;
                        }
                        if (tt.IsValue("ВЕЧЕР", "ВЕЧІР")) 
                        {
                            res.EndToken = tt;
                            res.IntValue += 12;
                            res.Typ = DateItemType.Hour;
                            return res;
                        }
                        if (tt.IsValue("ДЕНЬ", null)) 
                        {
                            res.EndToken = tt;
                            if (res.IntValue < 10) 
                                res.IntValue += 12;
                            res.Typ = DateItemType.Hour;
                            return res;
                        }
                        if (tt.IsValue("НОЧЬ", "НІЧ")) 
                        {
                            res.EndToken = tt;
                            if (res.IntValue == 12) 
                                res.IntValue = 0;
                            else if (res.IntValue > 9) 
                                res.IntValue += 12;
                            res.Typ = DateItemType.Hour;
                            return res;
                        }
                        if (tt.IsComma || tt.Morph.Class.IsAdverb) 
                            continue;
                        break;
                    }
                    if (res.Typ == DateItemType.Hour) 
                        return res;
                }
                bool canBeYear = true;
                if (prev != null && prev.Count > 0 && prev[prev.Count - 1].Typ == DateItemType.Month) 
                {
                }
                else if ((prev != null && prev.Count >= 4 && prev[prev.Count - 1].Typ == DateItemType.Delim) && prev[prev.Count - 2].CanByMonth) 
                {
                }
                else if (nt.Next != null && ((nt.Next.IsValue("ГОД", null) || nt.Next.IsValue("РІК", null)))) 
                {
                    if (res.IntValue < 1000) 
                        canBeYear = false;
                }
                tt = TestYearRusWord(nt.Next, false);
                if (tt != null && _isNewAge(tt.Next)) 
                {
                    res.Typ = DateItemType.Year;
                    res.EndToken = tt;
                }
                else if (canBeYear) 
                {
                    if (res.CanBeYear || res.Typ == DateItemType.Number) 
                    {
                        if ((((tt = TestYearRusWord(nt.Next, res.IsNewlineBefore)))) != null) 
                        {
                            if ((tt.IsValue("Г", null) && !tt.IsWhitespaceBefore && t.Previous != null) && ((t.Previous.IsValue("КОРПУС", null) || t.Previous.IsValue("КОРП", null)))) 
                            {
                            }
                            else if ((((nt.Next.IsValue("Г", null) && (t.WhitespacesBeforeCount < 3) && t.Previous != null) && t.Previous.IsValue("Я", null) && t.Previous.Previous != null) && t.Previous.Previous.IsCharOf("\\/") && t.Previous.Previous.Previous != null) && t.Previous.Previous.Previous.IsValue("А", null)) 
                                return null;
                            else if (nt.Next.LengthChar == 1 && !res.CanBeYear && ((prev == null || ((prev.Count > 0 && prev[prev.Count - 1].Typ != DateItemType.Delim))))) 
                            {
                            }
                            else 
                            {
                                res.EndToken = tt;
                                res.Typ = DateItemType.Year;
                                res.Lang = tt.Morph.Language;
                            }
                        }
                    }
                    else if (tt != null && (nt.WhitespacesAfterCount < 2) && (nt.EndChar - nt.BeginChar) == 1) 
                    {
                        res.EndToken = tt;
                        res.Typ = DateItemType.Year;
                        res.Lang = tt.Morph.Language;
                    }
                }
                if (nt.Previous != null) 
                {
                    if (nt.Previous.IsValue("В", "У") || nt.Previous.IsValue("К", null) || nt.Previous.IsValue("ДО", null)) 
                    {
                        if ((((tt = TestYearRusWord(nt.Next, false)))) != null) 
                        {
                            bool ok = false;
                            if ((res.IntValue < 100) && (tt is Pullenti.Ner.TextToken) && (((tt as Pullenti.Ner.TextToken).Term == "ГОДА" || (tt as Pullenti.Ner.TextToken).Term == "РОКИ"))) 
                            {
                            }
                            else 
                            {
                                ok = true;
                                if (nt.Previous.IsValue("ДО", null) && nt.Next.IsValue("Г", null)) 
                                {
                                    int cou = 0;
                                    for (Pullenti.Ner.Token ttt = nt.Previous.Previous; ttt != null && (cou < 10); ttt = ttt.Previous,cou++) 
                                    {
                                        Pullenti.Ner.Measure.Internal.MeasureToken mt = Pullenti.Ner.Measure.Internal.MeasureToken.TryParse(ttt, null, false, false, false, false);
                                        if (mt != null && mt.EndChar > nt.EndChar) 
                                        {
                                            ok = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (ok) 
                            {
                                res.EndToken = tt;
                                res.Typ = DateItemType.Year;
                                res.Lang = tt.Morph.Language;
                                res.BeginToken = nt.Previous;
                            }
                        }
                    }
                    else if (((nt.Previous.IsValue("IN", null) || nt.Previous.IsValue("SINCE", null))) && res.CanBeYear) 
                    {
                        Pullenti.Ner.Measure.Internal.NumbersWithUnitToken uu = (nt.Previous.IsValue("IN", null) ? Pullenti.Ner.Measure.Internal.NumbersWithUnitToken.TryParse(nt, null, false, false, false, false) : null);
                        if (uu != null && uu.Units.Count > 0) 
                        {
                        }
                        else 
                        {
                            res.Typ = DateItemType.Year;
                            res.BeginToken = nt.Previous;
                        }
                    }
                    else if (nt.Previous.IsValue("NEL", null) || nt.Previous.IsValue("DEL", null)) 
                    {
                        if (res.CanBeYear) 
                        {
                            res.Typ = DateItemType.Year;
                            res.Lang = Pullenti.Morph.MorphLang.IT;
                            res.BeginToken = nt.Previous;
                        }
                    }
                    else if (nt.Previous.IsValue("IL", null) && res.CanBeDay) 
                    {
                        res.Lang = Pullenti.Morph.MorphLang.IT;
                        res.BeginToken = nt.Previous;
                    }
                }
                Pullenti.Ner.Token t1 = res.EndToken.Next;
                if (t1 != null) 
                {
                    if (t1.IsValue("ЧАС", "ГОДИНА") || t1.IsValue("HOUR", null)) 
                    {
                        if ((((prev != null && prev.Count == 2 && prev[0].CanBeHour) && prev[1].Typ == DateItemType.Delim && !prev[1].IsWhitespaceAfter) && !prev[1].IsWhitespaceAfter && res.IntValue >= 0) && (res.IntValue < 59)) 
                        {
                            prev[0].Typ = DateItemType.Hour;
                            res.Typ = DateItemType.Minute;
                            res.EndToken = t1;
                        }
                        else if (res.IntValue < 24) 
                        {
                            if (t1.Next != null && t1.Next.IsChar('.')) 
                                t1 = t1.Next;
                            res.Typ = DateItemType.Hour;
                            res.EndToken = t1;
                        }
                    }
                    else if ((res.IntValue < 60) && ((t1.IsValue("МИНУТА", "ХВИЛИНА") || t1.IsValue("МИН", null) || t.IsValue("MINUTE", null)))) 
                    {
                        if (t1.Next != null && t1.Next.IsChar('.')) 
                            t1 = t1.Next;
                        res.Typ = DateItemType.Minute;
                        res.EndToken = t1;
                    }
                    else if ((res.IntValue < 60) && ((t1.IsValue("СЕКУНДА", null) || t1.IsValue("СЕК", null) || t1.IsValue("SECOND", null)))) 
                    {
                        if (t1.Next != null && t1.Next.IsChar('.')) 
                            t1 = t1.Next;
                        res.Typ = DateItemType.Second;
                        res.EndToken = t1;
                    }
                    else if ((res.IntValue < 30) && ((t1.IsValue("ВЕК", "ВІК") || t1.IsValue("СТОЛЕТИЕ", "СТОЛІТТЯ")))) 
                    {
                        res.Typ = DateItemType.Century;
                        res.EndToken = t1;
                    }
                    else if (res.IntValue <= 4 && t1.IsValue("КВАРТАЛ", null)) 
                    {
                        res.Typ = DateItemType.Quartal;
                        res.EndToken = t1;
                    }
                    else if (res.IntValue <= 2 && ((t1.IsValue("ПОЛУГОДИЕ", null) || t1.IsValue("ПІВРІЧЧЯ", null)))) 
                    {
                        res.Typ = DateItemType.Halfyear;
                        res.EndToken = t1;
                    }
                }
                return res;
            }
            Pullenti.Ner.TextToken t0 = t as Pullenti.Ner.TextToken;
            if (t0 == null) 
                return null;
            string txt = t0.GetSourceText();
            if ((txt[0] == 'I' || txt[0] == 'X' || txt[0] == 'Х') || txt[0] == 'V') 
            {
                Pullenti.Ner.NumberToken lat = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t);
                if (lat != null && lat.EndToken.Next != null && lat.IntValue != null) 
                {
                    int val = lat.IntValue.Value;
                    Pullenti.Ner.Token tt = lat.EndToken.Next;
                    if (tt.IsValue("КВАРТАЛ", null) && val > 0 && val <= 4) 
                        return new DateItemToken(t, tt) { Typ = DateItemType.Quartal, IntValue = val };
                    if (tt.IsValue("ПОЛУГОДИЕ", "ПІВРІЧЧЯ") && val > 0 && val <= 2) 
                        return new DateItemToken(t, lat.EndToken.Next) { Typ = DateItemType.Halfyear, IntValue = val };
                    if (tt.IsValue("ВЕК", "ВІК") || tt.IsValue("СТОЛЕТИЕ", "СТОЛІТТЯ")) 
                        return new DateItemToken(t, lat.EndToken.Next) { Typ = DateItemType.Century, IntValue = val };
                    if (tt.IsValue("В", null) && tt.Next != null && tt.Next.IsChar('.')) 
                    {
                        if (prev != null && prev.Count > 0 && prev[prev.Count - 1].Typ == DateItemType.Pointer) 
                            return new DateItemToken(t, tt.Next) { Typ = DateItemType.Century, IntValue = val };
                        if (_isNewAge(tt.Next.Next)) 
                            return new DateItemToken(t, tt.Next) { Typ = DateItemType.Century, IntValue = val };
                    }
                    if (tt.IsHiphen) 
                    {
                        Pullenti.Ner.NumberToken lat2 = Pullenti.Ner.Core.NumberHelper.TryParseRoman(tt.Next);
                        if (lat2 != null && lat2.IntValue != null && lat2.EndToken.Next != null) 
                        {
                            if (lat2.EndToken.Next.IsValue("ВЕК", "ВІК") || lat2.EndToken.Next.IsValue("СТОЛЕТИЕ", "СТОЛІТТЯ")) 
                            {
                                DateItemToken ddd = TryAttach(tt.Next, null, false);
                                return new DateItemToken(t, lat.EndToken) { Typ = DateItemType.Century, IntValue = val, NewAge = (ddd != null ? ddd.NewAge : 0) };
                            }
                        }
                    }
                }
            }
            if (t != null && t.IsValue("НАПРИКІНЦІ", null)) 
                return new DateItemToken(t, t) { Typ = DateItemType.Pointer, StringValue = "конец" };
            if (t != null && t.IsValue("ДОНЕДАВНА", null)) 
                return new DateItemToken(t, t) { Typ = DateItemType.Pointer, StringValue = "сегодня" };
            if (prev == null) 
            {
                if (t != null) 
                {
                    if (t.IsValue("ОКОЛО", "БІЛЯ") || t.IsValue("ПРИМЕРНО", "ПРИБЛИЗНО") || t.IsValue("ABOUT", null)) 
                        return new DateItemToken(t, t) { Typ = DateItemType.Pointer, StringValue = "около" };
                }
                if (t.IsValue("ОК", null) || t.IsValue("OK", null)) 
                {
                    if (t.Next != null && t.Next.IsChar('.')) 
                        return new DateItemToken(t, t.Next) { Typ = DateItemType.Pointer, StringValue = "около" };
                    return new DateItemToken(t, t) { Typ = DateItemType.Pointer, StringValue = "около" };
                }
            }
            Pullenti.Ner.Core.TerminToken tok = m_Seasons.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if ((tok != null && ((Pullenti.Ner.Date.DatePointerType)tok.Termin.Tag) == Pullenti.Ner.Date.DatePointerType.Summer && t.Morph.Language.IsRu) && (t is Pullenti.Ner.TextToken)) 
            {
                string str = (t as Pullenti.Ner.TextToken).Term;
                if (str != "ЛЕТОМ" && str != "ЛЕТА" && str != "ЛЕТО") 
                    tok = null;
            }
            if (tok != null) 
                return new DateItemToken(t, tok.EndToken) { Typ = DateItemType.Pointer, IntValue = (int)((Pullenti.Ner.Date.DatePointerType)tok.Termin.Tag) };
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
            if (npt != null) 
            {
                tok = m_Seasons.TryParse(npt.EndToken, Pullenti.Ner.Core.TerminParseAttr.No);
                if ((tok != null && ((Pullenti.Ner.Date.DatePointerType)tok.Termin.Tag) == Pullenti.Ner.Date.DatePointerType.Summer && t.Morph.Language.IsRu) && (t is Pullenti.Ner.TextToken)) 
                {
                    string str = (t as Pullenti.Ner.TextToken).Term;
                    if (str != "ЛЕТОМ" && str != "ЛЕТА" && str != "ЛЕТО") 
                        tok = null;
                }
                if (tok != null) 
                    return new DateItemToken(t, tok.EndToken) { Typ = DateItemType.Pointer, IntValue = (int)((Pullenti.Ner.Date.DatePointerType)tok.Termin.Tag) };
                DateItemType typ = DateItemType.Number;
                if (npt.Noun.IsValue("КВАРТАЛ", null)) 
                    typ = DateItemType.Quartal;
                else if (npt.EndToken.IsValue("ПОЛУГОДИЕ", null) || npt.EndToken.IsValue("ПІВРІЧЧЯ", null)) 
                    typ = DateItemType.Halfyear;
                else if (npt.EndToken.IsValue("НАЧАЛО", null) || npt.EndToken.IsValue("ПОЧАТОК", null)) 
                    return new DateItemToken(t, npt.EndToken) { Typ = DateItemType.Pointer, StringValue = "начало" };
                else if (npt.EndToken.IsValue("СЕРЕДИНА", null)) 
                    return new DateItemToken(t, npt.EndToken) { Typ = DateItemType.Pointer, StringValue = "середина" };
                else if (npt.EndToken.IsValue("КОНЕЦ", null) || npt.EndToken.IsValue("КІНЕЦЬ", null) || npt.EndToken.IsValue("НАПРИКІНЕЦЬ", null)) 
                    return new DateItemToken(t, npt.EndToken) { Typ = DateItemType.Pointer, StringValue = "конец" };
                else if (npt.EndToken.IsValue("ВРЕМЯ", null) && npt.Adjectives.Count > 0 && npt.EndToken.Previous.IsValue("НАСТОЯЩЕЕ", null)) 
                    return new DateItemToken(t, npt.EndToken) { Typ = DateItemType.Pointer, StringValue = "сегодня" };
                else if (npt.EndToken.IsValue("ЧАС", null) && npt.Adjectives.Count > 0 && npt.EndToken.Previous.IsValue("ДАНИЙ", null)) 
                    return new DateItemToken(t, npt.EndToken) { Typ = DateItemType.Pointer, StringValue = "сегодня" };
                if (typ != DateItemType.Number || detailRegime) 
                {
                    int delta = 0;
                    if (npt.Adjectives.Count > 0) 
                    {
                        if (npt.Adjectives[0].IsValue("ПОСЛЕДНИЙ", "ОСТАННІЙ")) 
                            return new DateItemToken(t0, npt.EndToken) { Typ = typ, IntValue = (typ == DateItemType.Quartal ? 4 : 2) };
                        if (npt.Adjectives[0].IsValue("ПРЕДЫДУЩИЙ", "ПОПЕРЕДНІЙ") || npt.Adjectives[0].IsValue("ПРОШЛЫЙ", null)) 
                            delta = -1;
                        else if (npt.Adjectives[0].IsValue("СЛЕДУЮЩИЙ", null) || npt.Adjectives[0].IsValue("ПОСЛЕДУЮЩИЙ", null) || npt.Adjectives[0].IsValue("НАСТУПНИЙ", null)) 
                            delta = 1;
                        else 
                            return null;
                    }
                    int cou = 0;
                    for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
                    {
                        if (cou > 200) 
                            break;
                        Pullenti.Ner.Date.DateRangeReferent dr = tt.GetReferent() as Pullenti.Ner.Date.DateRangeReferent;
                        if (dr == null) 
                            continue;
                        if (typ == DateItemType.Quartal) 
                        {
                            int ii = dr.QuarterNumber;
                            if (ii < 1) 
                                continue;
                            ii += delta;
                            if ((ii < 1) || ii > 4) 
                                continue;
                            return new DateItemToken(t0, npt.EndToken) { Typ = typ, IntValue = ii };
                        }
                        if (typ == DateItemType.Halfyear) 
                        {
                            int ii = dr.HalfyearNumber;
                            if (ii < 1) 
                                continue;
                            ii += delta;
                            if ((ii < 1) || ii > 2) 
                                continue;
                            return new DateItemToken(t0, npt.EndToken) { Typ = typ, IntValue = ii };
                        }
                    }
                }
            }
            string term = t0.Term;
            if (!char.IsLetterOrDigit(term[0])) 
            {
                if (t0.IsCharOf(".\\/:") || t0.IsHiphen) 
                    return new DateItemToken(t0, t0) { Typ = DateItemType.Delim, StringValue = term };
                else if (t0.IsChar(',')) 
                    return new DateItemToken(t0, t0) { Typ = DateItemType.Delim, StringValue = term };
                else 
                    return null;
            }
            if (term == "O" || term == "О") 
            {
                if ((t.Next is Pullenti.Ner.NumberToken) && !t.IsWhitespaceAfter && (t.Next as Pullenti.Ner.NumberToken).Value.Length == 1) 
                    return new DateItemToken(t, t.Next) { Typ = DateItemType.Number, IntValue = (t.Next as Pullenti.Ner.NumberToken).IntValue.Value };
            }
            if (char.IsLetter(term[0])) 
            {
                Pullenti.Ner.Core.TerminToken inf = m_Monthes.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (inf != null && inf.Termin.Tag == null) 
                    inf = m_Monthes.TryParse(inf.EndToken.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                if (inf != null && (inf.Termin.Tag is int)) 
                    return new DateItemToken(inf.BeginToken, inf.EndToken) { Typ = DateItemType.Month, IntValue = (int)inf.Termin.Tag, Lang = inf.Termin.Lang };
            }
            return null;
        }
        public static Pullenti.Ner.Core.TerminCollection DaysOfWeek;
        static Pullenti.Ner.Core.TerminCollection m_NewAge;
        static Pullenti.Ner.Core.TerminCollection m_Monthes;
        static Pullenti.Ner.Core.TerminCollection m_Seasons;
        public static void Initialize()
        {
            if (m_NewAge != null) 
                return;
            m_NewAge = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin tt = new Pullenti.Ner.Core.Termin("НОВАЯ ЭРА", Pullenti.Morph.MorphLang.RU, true) { CanonicText = "НОВОЙ ЭРЫ" };
            tt.AddVariant("НАША ЭРА", true);
            tt.AddAbridge("Н.Э.");
            m_NewAge.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("НОВА ЕРА", Pullenti.Morph.MorphLang.UA, true) { CanonicText = "НОВОЇ ЕРИ" };
            tt.AddVariant("НАША ЕРА", true);
            tt.AddAbridge("Н.Е.");
            m_NewAge.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("РОЖДЕСТВО ХРИСТОВО", Pullenti.Morph.MorphLang.RU, true);
            tt.AddAbridge("Р.Х.");
            m_NewAge.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("РІЗДВА ХРИСТОВОГО", Pullenti.Morph.MorphLang.UA, true);
            tt.AddAbridge("Р.Х.");
            m_NewAge.Add(tt);
            m_Seasons = new Pullenti.Ner.Core.TerminCollection();
            m_Seasons.Add(new Pullenti.Ner.Core.Termin("ЗИМА", Pullenti.Morph.MorphLang.RU, true) { Tag = Pullenti.Ner.Date.DatePointerType.Winter });
            m_Seasons.Add(new Pullenti.Ner.Core.Termin("WINTER", Pullenti.Morph.MorphLang.EN, true) { Tag = Pullenti.Ner.Date.DatePointerType.Winter });
            Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin("ВЕСНА", Pullenti.Morph.MorphLang.RU, true) { Tag = Pullenti.Ner.Date.DatePointerType.Spring };
            t.AddVariant("ПРОВЕСНА", true);
            m_Seasons.Add(t);
            m_Seasons.Add(new Pullenti.Ner.Core.Termin("SPRING", Pullenti.Morph.MorphLang.EN, true) { Tag = Pullenti.Ner.Date.DatePointerType.Spring });
            t = new Pullenti.Ner.Core.Termin("ЛЕТО", Pullenti.Morph.MorphLang.RU, true) { Tag = Pullenti.Ner.Date.DatePointerType.Summer };
            m_Seasons.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛІТО", Pullenti.Morph.MorphLang.UA, true) { Tag = Pullenti.Ner.Date.DatePointerType.Summer };
            m_Seasons.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОСЕНЬ", Pullenti.Morph.MorphLang.RU, true) { Tag = Pullenti.Ner.Date.DatePointerType.Autumn };
            m_Seasons.Add(t);
            t = new Pullenti.Ner.Core.Termin("AUTUMN", Pullenti.Morph.MorphLang.EN, true) { Tag = Pullenti.Ner.Date.DatePointerType.Autumn };
            m_Seasons.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОСІНЬ", Pullenti.Morph.MorphLang.UA, true) { Tag = Pullenti.Ner.Date.DatePointerType.Autumn };
            m_Seasons.Add(t);
            m_Monthes = new Pullenti.Ner.Core.TerminCollection();
            string[] months = new string[] {"ЯНВАРЬ", "ФЕВРАЛЬ", "МАРТ", "АПРЕЛЬ", "МАЙ", "ИЮНЬ", "ИЮЛЬ", "АВГУСТ", "СЕНТЯБРЬ", "ОКТЯБРЬ", "НОЯБРЬ", "ДЕКАБРЬ"};
            for (int i = 0; i < months.Length; i++) 
            {
                t = new Pullenti.Ner.Core.Termin(months[i], Pullenti.Morph.MorphLang.RU, true) { Tag = i + 1 };
                m_Monthes.Add(t);
            }
            months = new string[] {"СІЧЕНЬ", "ЛЮТИЙ", "БЕРЕЗЕНЬ", "КВІТЕНЬ", "ТРАВЕНЬ", "ЧЕРВЕНЬ", "ЛИПЕНЬ", "СЕРПЕНЬ", "ВЕРЕСЕНЬ", "ЖОВТЕНЬ", "ЛИСТОПАД", "ГРУДЕНЬ"};
            for (int i = 0; i < months.Length; i++) 
            {
                t = new Pullenti.Ner.Core.Termin(months[i], Pullenti.Morph.MorphLang.UA, true) { Tag = i + 1 };
                m_Monthes.Add(t);
            }
            months = new string[] {"JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"};
            for (int i = 0; i < months.Length; i++) 
            {
                t = new Pullenti.Ner.Core.Termin(months[i], Pullenti.Morph.MorphLang.EN, true) { Tag = i + 1 };
                m_Monthes.Add(t);
            }
            months = new string[] {"GENNAIO", "FEBBRAIO", "MARZO", "APRILE", "MAGGIO", "GUINGO", "LUGLIO", "AGOSTO", "SETTEMBRE", "OTTOBRE", "NOVEMBRE", "DICEMBRE"};
            for (int i = 0; i < months.Length; i++) 
            {
                t = new Pullenti.Ner.Core.Termin(months[i], Pullenti.Morph.MorphLang.IT, true) { Tag = i + 1 };
                m_Monthes.Add(t);
            }
            foreach (string m in new string[] {"ЯНВ", "ФЕВ", "ФЕВР", "МАР", "АПР", "ИЮН", "ИЮЛ", "АВГ", "СЕН", "СЕНТ", "ОКТ", "НОЯ", "НОЯБ", "ДЕК", "JAN", "FEB", "MAR", "APR", "JUN", "JUL", "AUG", "SEP", "SEPT", "OCT", "NOV", "DEC"}) 
            {
                foreach (Pullenti.Ner.Core.Termin ttt in m_Monthes.Termins) 
                {
                    if (ttt.Terms[0].CanonicalText.StartsWith(m)) 
                    {
                        ttt.AddAbridge(m);
                        m_Monthes.Reindex(ttt);
                        break;
                    }
                }
            }
            foreach (string m in new string[] {"OF"}) 
            {
                m_Monthes.Add(new Pullenti.Ner.Core.Termin(m, Pullenti.Morph.MorphLang.EN, true));
            }
            m_EmptyWords = new Dictionary<string, object>();
            m_EmptyWords.Add("IN", Pullenti.Morph.MorphLang.EN);
            m_EmptyWords.Add("SINCE", Pullenti.Morph.MorphLang.EN);
            m_EmptyWords.Add("THE", Pullenti.Morph.MorphLang.EN);
            m_EmptyWords.Add("NEL", Pullenti.Morph.MorphLang.IT);
            m_EmptyWords.Add("DEL", Pullenti.Morph.MorphLang.IT);
            m_EmptyWords.Add("IL", Pullenti.Morph.MorphLang.IT);
            DaysOfWeek = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin te = new Pullenti.Ner.Core.Termin("SUNDAY", Pullenti.Morph.MorphLang.EN, true) { Tag = 7 };
            te.AddAbridge("SUN");
            te.AddVariant("ВОСКРЕСЕНЬЕ", true);
            te.AddVariant("ВОСКРЕСЕНИЕ", true);
            te.AddAbridge("ВС");
            te.AddVariant("НЕДІЛЯ", true);
            DaysOfWeek.Add(te);
            te = new Pullenti.Ner.Core.Termin("MONDAY", Pullenti.Morph.MorphLang.EN, true) { Tag = 1 };
            te.AddAbridge("MON");
            te.AddVariant("ПОНЕДЕЛЬНИК", true);
            te.AddAbridge("ПОН");
            te.AddVariant("ПОНЕДІЛОК", true);
            DaysOfWeek.Add(te);
            te = new Pullenti.Ner.Core.Termin("TUESDAY", Pullenti.Morph.MorphLang.EN, true) { Tag = 2 };
            te.AddAbridge("TUE");
            te.AddVariant("ВТОРНИК", true);
            te.AddAbridge("ВТ");
            te.AddVariant("ВІВТОРОК", true);
            DaysOfWeek.Add(te);
            te = new Pullenti.Ner.Core.Termin("WEDNESDAY", Pullenti.Morph.MorphLang.EN, true) { Tag = 3 };
            te.AddAbridge("WED");
            te.AddVariant("СРЕДА", true);
            te.AddAbridge("СР");
            te.AddVariant("СЕРЕДА", true);
            DaysOfWeek.Add(te);
            te = new Pullenti.Ner.Core.Termin("THURSDAY", Pullenti.Morph.MorphLang.EN, true) { Tag = 4 };
            te.AddAbridge("THU");
            te.AddVariant("ЧЕТВЕРГ", true);
            te.AddAbridge("ЧТ");
            te.AddVariant("ЧЕТВЕР", true);
            DaysOfWeek.Add(te);
            te = new Pullenti.Ner.Core.Termin("FRIDAY", Pullenti.Morph.MorphLang.EN, true) { Tag = 5 };
            te.AddAbridge("FRI");
            te.AddVariant("ПЯТНИЦА", true);
            te.AddAbridge("ПТ");
            te.AddVariant("ПЯТНИЦЯ", true);
            DaysOfWeek.Add(te);
            te = new Pullenti.Ner.Core.Termin("SATURDAY", Pullenti.Morph.MorphLang.EN, true) { Tag = 6 };
            te.AddAbridge("SAT");
            te.AddVariant("СУББОТА", true);
            te.AddAbridge("СБ");
            te.AddVariant("СУБОТА", true);
            DaysOfWeek.Add(te);
        }
        static Dictionary<string, object> m_EmptyWords;
        static Pullenti.Ner.Token TestYearRusWord(Pullenti.Ner.Token t0, bool ignoreNewline = false)
        {
            Pullenti.Ner.Token tt = t0;
            if (tt == null) 
                return null;
            if (!ignoreNewline && tt.Previous != null && tt.IsNewlineBefore) 
                return null;
            if (tt.IsValue("ГОД", null) || tt.IsValue("РІК", null)) 
                return tt;
            if ((tt.IsValue("Г", null) && tt.Next != null && tt.Next.IsCharOf("\\/.")) && tt.Next.Next != null && tt.Next.Next.IsValue("Б", null)) 
                return null;
            if (((tt.Morph.Language.IsRu && ((tt.IsValue("ГГ", null) || tt.IsValue("Г", null))))) || ((tt.Morph.Language.IsUa && ((tt.IsValue("Р", null) || tt.IsValue("РР", null)))))) 
            {
                if (tt.Next != null && tt.Next.IsChar('.')) 
                {
                    tt = tt.Next;
                    if ((tt.Next != null && (tt.WhitespacesAfterCount < 4) && ((((tt.Next.IsValue("Г", null) && tt.Next.Morph.Language.IsRu)) || ((tt.Next.Morph.Language.IsUa && tt.Next.IsValue("Р", null)))))) && tt.Next.Next != null && tt.Next.Next.IsChar('.')) 
                        tt = tt.Next.Next;
                    return tt;
                }
                else 
                    return tt;
            }
            return null;
        }
        public static List<DateItemToken> TryAttachList(Pullenti.Ner.Token t, int maxCount = 20)
        {
            DateItemToken p = TryAttach(t, null, false);
            if (p == null) 
                return null;
            if (p.Typ == DateItemType.Delim) 
                return null;
            List<DateItemToken> res = new List<DateItemToken>();
            res.Add(p);
            Pullenti.Ner.Token tt = p.EndToken.Next;
            while (tt != null) 
            {
                if (tt is Pullenti.Ner.TextToken) 
                {
                    if ((tt as Pullenti.Ner.TextToken).CheckValue(m_EmptyWords) != null) 
                    {
                        tt = tt.Next;
                        continue;
                    }
                }
                DateItemToken p0 = TryAttach(tt, res, false);
                if (p0 == null) 
                {
                    if (tt.IsNewlineBefore) 
                        break;
                    if (tt.Chars.IsLatinLetter) 
                        break;
                    if (tt.Morph != null && tt.Morph.Check(Pullenti.Morph.MorphClass.Adjective | Pullenti.Morph.MorphClass.Pronoun)) 
                    {
                        tt = tt.Next;
                        continue;
                    }
                    break;
                }
                if (tt.IsNewlineBefore) 
                {
                    if (p.Typ == DateItemType.Month && p0.CanBeYear) 
                    {
                    }
                    else if (p.Typ == DateItemType.Number && p.CanBeDay && p0.Typ == DateItemType.Month) 
                    {
                    }
                    else 
                        break;
                }
                if (p0.CanBeYear && p0.Typ == DateItemType.Number) 
                {
                    if (p.Typ == DateItemType.Halfyear || p.Typ == DateItemType.Quartal) 
                        p0.Typ = DateItemType.Year;
                    else if (p.Typ == DateItemType.Pointer && p0.IntValue > 1990) 
                        p0.Typ = DateItemType.Year;
                }
                p = p0;
                res.Add(p);
                if (maxCount > 0 && res.Count >= maxCount) 
                    break;
                tt = p.EndToken.Next;
            }
            for (int i = res.Count - 1; i >= 0; i--) 
            {
                if (res[i].Typ == DateItemType.Delim) 
                    res.RemoveAt(i);
                else 
                    break;
            }
            if (res.Count > 0 && res[res.Count - 1].Typ == DateItemType.Number) 
            {
                Pullenti.Ner.Core.NumberExToken nex = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(res[res.Count - 1].BeginToken);
                if (nex != null && nex.ExTyp != Pullenti.Ner.Core.NumberExType.Hour) 
                {
                    if (res.Count > 3 && res[res.Count - 2].Typ == DateItemType.Delim && res[res.Count - 2].StringValue == ":") 
                    {
                    }
                    else 
                        res.RemoveAt(res.Count - 1);
                }
            }
            if (res.Count == 0) 
                return null;
            for (int i = 1; i < (res.Count - 1); i++) 
            {
                if (res[i].Typ == DateItemType.Delim && res[i].BeginToken.IsComma) 
                {
                    if ((i == 1 && res[i - 1].Typ == DateItemType.Month && res[i + 1].CanBeYear) && (i + 1) == (res.Count - 1)) 
                        res.RemoveAt(i);
                }
            }
            if (res[res.Count - 1].Typ == DateItemType.Number) 
            {
                DateItemToken rr = res[res.Count - 1];
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(rr.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.EndChar > rr.EndChar) 
                {
                    res.RemoveAt(res.Count - 1);
                    if (res.Count > 0 && res[res.Count - 1].Typ == DateItemType.Delim) 
                        res.RemoveAt(res.Count - 1);
                }
            }
            if (res.Count == 0) 
                return null;
            if (res.Count == 2 && !res[0].IsWhitespaceAfter) 
            {
                if (!res[0].IsWhitespaceBefore && !res[1].IsWhitespaceAfter) 
                    return null;
            }
            return res;
        }
    }
}