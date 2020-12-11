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

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Работа с числовыми значениями
    /// </summary>
    public static class NumberHelper
    {
        /// <summary>
        /// Попробовать создать числительное (без знака, целочисленное). 
        /// Внимание! Этот метод всегда вызывается процессором при формировании цепочки токенов в методе Process(), 
        /// так что все NumberToken уже созданы в основной цепочке, сфорированной для текста.
        /// </summary>
        /// <param name="token">начальный токен</param>
        /// <return>число-метатокен</return>
        internal static Pullenti.Ner.NumberToken TryParseNumber(Pullenti.Ner.Token token)
        {
            return _TryParse(token, null);
        }
        static Pullenti.Ner.NumberToken _TryParse(Pullenti.Ner.Token token, Pullenti.Ner.NumberToken prevVal = null)
        {
            if (token is Pullenti.Ner.NumberToken) 
                return token as Pullenti.Ner.NumberToken;
            Pullenti.Ner.TextToken tt = token as Pullenti.Ner.TextToken;
            if (tt == null) 
                return null;
            Pullenti.Ner.TextToken et = tt;
            string val = null;
            Pullenti.Ner.NumberSpellingType typ = Pullenti.Ner.NumberSpellingType.Digit;
            string term = tt.Term;
            int i;
            int j;
            if (char.IsDigit(term[0])) 
                val = term;
            if (val != null) 
            {
                bool hiph = false;
                if ((et.Next is Pullenti.Ner.TextToken) && et.Next.IsHiphen) 
                {
                    if ((et.WhitespacesAfterCount < 2) && (et.Next.WhitespacesAfterCount < 2)) 
                    {
                        et = et.Next as Pullenti.Ner.TextToken;
                        hiph = true;
                    }
                }
                Pullenti.Ner.MorphCollection mc = null;
                if (hiph || !et.IsWhitespaceAfter) 
                {
                    Pullenti.Ner.MetaToken rr = AnalizeNumberTail(et.Next as Pullenti.Ner.TextToken, val);
                    if (rr == null) 
                        et = tt;
                    else 
                    {
                        mc = rr.Morph;
                        et = rr.EndToken as Pullenti.Ner.TextToken;
                    }
                }
                else 
                    et = tt;
                if (et.Next != null && et.Next.IsChar('(')) 
                {
                    Pullenti.Ner.NumberToken num2 = TryParseNumber(et.Next.Next);
                    if ((num2 != null && num2.Value == val && num2.EndToken.Next != null) && num2.EndToken.Next.IsChar(')')) 
                        et = num2.EndToken.Next as Pullenti.Ner.TextToken;
                }
                while ((et.Next is Pullenti.Ner.TextToken) && !(et.Previous is Pullenti.Ner.NumberToken) && et.IsWhitespaceBefore) 
                {
                    if (et.WhitespacesAfterCount != 1) 
                        break;
                    string sss = (et.Next as Pullenti.Ner.TextToken).Term;
                    if (sss == "000") 
                    {
                        val = val + "000";
                        et = et.Next as Pullenti.Ner.TextToken;
                        continue;
                    }
                    if (char.IsDigit(sss[0]) && sss.Length == 3) 
                    {
                        string val2 = val;
                        for (Pullenti.Ner.Token ttt = et.Next; ttt != null; ttt = ttt.Next) 
                        {
                            string ss = ttt.GetSourceText();
                            if (ttt.WhitespacesBeforeCount == 1 && ttt.LengthChar == 3 && char.IsDigit(ss[0])) 
                            {
                                int ii;
                                if (!int.TryParse(ss, out ii)) 
                                    break;
                                val2 += ss;
                                continue;
                            }
                            if ((ttt.IsCharOf(".,") && !ttt.IsWhitespaceBefore && !ttt.IsWhitespaceAfter) && ttt.Next != null && char.IsDigit(ttt.Next.GetSourceText()[0])) 
                            {
                                if (ttt.Next.IsWhitespaceAfter && (ttt.Previous is Pullenti.Ner.TextToken)) 
                                {
                                    et = ttt.Previous as Pullenti.Ner.TextToken;
                                    val = val2;
                                    break;
                                }
                            }
                            if (((((ttt.IsValue("МИЛЛИАРД", "МІЛЬЯРД") || ttt.IsValue("BILLION", null) || ttt.IsValue("BN", null)) || ttt.IsValue("МЛРД", null) || ttt.IsValue("МИЛЛИОН", "МІЛЬЙОН")) || ttt.IsValue("MILLION", null) || ttt.IsValue("МЛН", null)) || ttt.IsValue("ТЫСЯЧА", "ТИСЯЧА") || ttt.IsValue("THOUSAND", null)) || ttt.IsValue("ТЫС", null) || ttt.IsValue("ТИС", null)) 
                            {
                                et = ttt.Previous as Pullenti.Ner.TextToken;
                                val = val2;
                                break;
                            }
                            break;
                        }
                    }
                    break;
                }
                for (int k = 0; k < 3; k++) 
                {
                    if ((et.Next is Pullenti.Ner.TextToken) && et.Next.Chars.IsLetter) 
                    {
                        tt = et.Next as Pullenti.Ner.TextToken;
                        Pullenti.Ner.Token t0 = (Pullenti.Ner.Token)et;
                        string coef = null;
                        if (k == 0) 
                        {
                            coef = "000000000";
                            if (tt.IsValue("МИЛЛИАРД", "МІЛЬЯРД") || tt.IsValue("BILLION", null) || tt.IsValue("BN", null)) 
                            {
                                et = tt;
                                val += coef;
                            }
                            else if (tt.IsValue("МЛРД", null)) 
                            {
                                et = tt;
                                val += coef;
                                if ((et.Next is Pullenti.Ner.TextToken) && et.Next.IsChar('.')) 
                                    et = et.Next as Pullenti.Ner.TextToken;
                            }
                            else 
                                continue;
                        }
                        else if (k == 1) 
                        {
                            coef = "000000";
                            if (tt.IsValue("МИЛЛИОН", "МІЛЬЙОН") || tt.IsValue("MILLION", null)) 
                            {
                                et = tt;
                                val += coef;
                            }
                            else if (tt.IsValue("МЛН", null)) 
                            {
                                et = tt;
                                val += coef;
                                if ((et.Next is Pullenti.Ner.TextToken) && et.Next.IsChar('.')) 
                                    et = et.Next as Pullenti.Ner.TextToken;
                            }
                            else if ((tt is Pullenti.Ner.TextToken) && (tt as Pullenti.Ner.TextToken).Term == "M") 
                            {
                                if (NumberHelper.IsMoneyChar(et.Previous) != null) 
                                {
                                    et = tt;
                                    val += coef;
                                }
                                else 
                                    break;
                            }
                            else 
                                continue;
                        }
                        else 
                        {
                            coef = "000";
                            if (tt.IsValue("ТЫСЯЧА", "ТИСЯЧА") || tt.IsValue("THOUSAND", null)) 
                            {
                                et = tt;
                                val += coef;
                            }
                            else if (tt.IsValue("ТЫС", null) || tt.IsValue("ТИС", null)) 
                            {
                                et = tt;
                                val += coef;
                                if ((et.Next is Pullenti.Ner.TextToken) && et.Next.IsChar('.')) 
                                    et = et.Next as Pullenti.Ner.TextToken;
                            }
                            else 
                                break;
                        }
                        if (((t0 == token && t0.LengthChar <= 3 && t0.Previous != null) && !t0.IsWhitespaceBefore && t0.Previous.IsCharOf(",.")) && !t0.Previous.IsWhitespaceBefore && (((t0.Previous.Previous is Pullenti.Ner.NumberToken) || prevVal != null))) 
                        {
                            if (t0.LengthChar == 1) 
                                val = val.Substring(0, val.Length - 1);
                            else if (t0.LengthChar == 2) 
                                val = val.Substring(0, val.Length - 2);
                            else 
                                val = val.Substring(0, val.Length - 3);
                            string hi = (t0.Previous.Previous is Pullenti.Ner.NumberToken ? (t0.Previous.Previous as Pullenti.Ner.NumberToken).Value : prevVal.Value);
                            int cou = coef.Length - val.Length;
                            for (; cou > 0; cou--) 
                            {
                                hi = hi + "0";
                            }
                            val = hi + val;
                            token = t0.Previous.Previous;
                        }
                        Pullenti.Ner.NumberToken next = _TryParse(et.Next, null);
                        if (next == null || next.Value.Length > coef.Length) 
                            break;
                        Pullenti.Ner.Token tt1 = next.EndToken;
                        if (((tt1.Next is Pullenti.Ner.TextToken) && !tt1.IsWhitespaceAfter && tt1.Next.IsCharOf(".,")) && !tt1.Next.IsWhitespaceAfter) 
                        {
                            Pullenti.Ner.NumberToken re1 = _TryParse(tt1.Next.Next, next);
                            if (re1 != null && re1.BeginToken == next.BeginToken) 
                                next = re1;
                        }
                        if (val.Length > next.Value.Length) 
                            val = val.Substring(0, val.Length - next.Value.Length);
                        val += next.Value;
                        et = next.EndToken as Pullenti.Ner.TextToken;
                        break;
                    }
                }
                Pullenti.Ner.NumberToken res = new Pullenti.Ner.NumberToken(token, et, val, typ) { Morph = mc };
                if (et.Next != null && (res.Value.Length < 4) && ((et.Next.IsHiphen || et.Next.IsValue("ДО", null)))) 
                {
                    for (Pullenti.Ner.Token tt1 = et.Next.Next; tt1 != null; tt1 = tt1.Next) 
                    {
                        if (!(tt1 is Pullenti.Ner.TextToken)) 
                            break;
                        if (char.IsDigit((tt1 as Pullenti.Ner.TextToken).Term[0])) 
                            continue;
                        if (tt1.IsCharOf(",.") || NumberHelper.IsMoneyChar(tt1) != null) 
                            continue;
                        if (tt1.IsValue("МИЛЛИОН", "МІЛЬЙОН") || tt1.IsValue("МЛН", null) || tt1.IsValue("MILLION", null)) 
                            res.Value += "000000";
                        else if ((tt1.IsValue("МИЛЛИАРД", "МІЛЬЯРД") || tt1.IsValue("МЛРД", null) || tt1.IsValue("BILLION", null)) || tt1.IsValue("BN", null)) 
                            res.Value += "000000000";
                        else if (tt1.IsValue("ТЫСЯЧА", "ТИСЯЧА") || tt1.IsValue("ТЫС", "ТИС") || tt1.IsValue("THOUSAND", null)) 
                            res.Value += "1000";
                        break;
                    }
                }
                return res;
            }
            int intVal = 0;
            et = null;
            int locValue = 0;
            bool isAdj = false;
            int jPrev = -1;
            for (Pullenti.Ner.TextToken t = tt; t != null; t = t.Next as Pullenti.Ner.TextToken) 
            {
                if (t != tt && t.NewlinesBeforeCount > 1) 
                    break;
                term = t.Term;
                if (!char.IsLetter(term[0])) 
                    break;
                TerminToken num = m_Nums.TryParse(t, TerminParseAttr.FullwordsOnly);
                if (num == null) 
                    break;
                j = (int)num.Termin.Tag;
                if (jPrev > 0 && (jPrev < 20) && (j < 20)) 
                    break;
                isAdj = ((j & PrilNumTagBit)) != 0;
                j &= (~PrilNumTagBit);
                if (isAdj && t != tt) 
                {
                    if ((t.IsValue("ДЕСЯТЫЙ", null) || t.IsValue("СОТЫЙ", null) || t.IsValue("ТЫСЯЧНЫЙ", null)) || t.IsValue("ДЕСЯТИТЫСЯЧНЫЙ", null) || t.IsValue("МИЛЛИОННЫЙ", null)) 
                        break;
                }
                if (j >= 1000) 
                {
                    if (locValue == 0) 
                        locValue = 1;
                    intVal += (locValue * j);
                    locValue = 0;
                }
                else 
                {
                    if (locValue > 0 && locValue <= j) 
                        break;
                    locValue += j;
                }
                et = t;
                if (j == 1000 || j == 1000000) 
                {
                    if ((et.Next is Pullenti.Ner.TextToken) && et.Next.IsChar('.')) 
                        t = (et = et.Next as Pullenti.Ner.TextToken);
                }
                jPrev = j;
            }
            if (locValue > 0) 
                intVal += locValue;
            if (intVal == 0 || et == null) 
                return null;
            Pullenti.Ner.NumberToken nt = new Pullenti.Ner.NumberToken(tt, et, intVal.ToString(), Pullenti.Ner.NumberSpellingType.Words);
            if (et.Morph != null) 
            {
                nt.Morph = new Pullenti.Ner.MorphCollection(et.Morph);
                foreach (Pullenti.Morph.MorphBaseInfo wff in et.Morph.Items) 
                {
                    Pullenti.Morph.MorphWordForm wf = wff as Pullenti.Morph.MorphWordForm;
                    if (wf != null && wf.Misc != null && wf.Misc.Attrs.Contains("собир.")) 
                    {
                        nt.Morph.Class = Pullenti.Morph.MorphClass.Noun;
                        break;
                    }
                }
                if (!isAdj) 
                {
                    nt.Morph.RemoveItems(Pullenti.Morph.MorphClass.Adjective | Pullenti.Morph.MorphClass.Noun, false);
                    if (nt.Morph.Class.IsUndefined) 
                        nt.Morph.Class = Pullenti.Morph.MorphClass.Noun;
                }
                if (et.Chars.IsLatinLetter && isAdj) 
                    nt.Morph.Class = Pullenti.Morph.MorphClass.Adjective;
            }
            return nt;
        }
        /// <summary>
        /// Попробовать выделить число в римской записи
        /// </summary>
        /// <param name="t">начальный токен</param>
        /// <return>числовой метатокен или null</return>
        public static Pullenti.Ner.NumberToken TryParseRoman(Pullenti.Ner.Token t)
        {
            if (t is Pullenti.Ner.NumberToken) 
                return t as Pullenti.Ner.NumberToken;
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null || !t.Chars.IsLetter) 
                return null;
            string term = tt.Term;
            if (!_isRomVal(term)) 
                return null;
            if (tt.Morph.Class.IsPreposition) 
            {
                if (tt.Chars.IsAllLower) 
                    return null;
            }
            Pullenti.Ner.NumberToken res = new Pullenti.Ner.NumberToken(t, t, "", Pullenti.Ner.NumberSpellingType.Roman);
            List<int> nums = new List<int>();
            int val = 0;
            for (; t != null; t = t.Next) 
            {
                if (t != res.BeginToken && t.IsWhitespaceBefore) 
                    break;
                if (!(t is Pullenti.Ner.TextToken)) 
                    break;
                term = (t as Pullenti.Ner.TextToken).Term;
                if (!_isRomVal(term)) 
                    break;
                foreach (char s in term) 
                {
                    int i = _romVal(s);
                    if (i > 0) 
                        nums.Add(i);
                }
                res.EndToken = t;
            }
            if (nums.Count == 0) 
                return null;
            for (int i = 0; i < nums.Count; i++) 
            {
                if ((i + 1) < nums.Count) 
                {
                    if (nums[i] == 1 && nums[i + 1] == 5) 
                    {
                        val += 4;
                        i++;
                    }
                    else if (nums[i] == 1 && nums[i + 1] == 10) 
                    {
                        val += 9;
                        i++;
                    }
                    else if (nums[i] == 10 && nums[i + 1] == 50) 
                    {
                        val += 40;
                        i++;
                    }
                    else if (nums[i] == 10 && nums[i + 1] == 100) 
                    {
                        val += 90;
                        i++;
                    }
                    else 
                        val += nums[i];
                }
                else 
                    val += nums[i];
            }
            res.IntValue = val;
            bool hiph = false;
            Pullenti.Ner.Token et = res.EndToken.Next;
            if (et == null) 
                return res;
            if (et.Next != null && et.Next.IsHiphen) 
            {
                et = et.Next;
                hiph = true;
            }
            if (hiph || !et.IsWhitespaceAfter) 
            {
                Pullenti.Ner.MetaToken mc = AnalizeNumberTail(et.Next as Pullenti.Ner.TextToken, res.Value);
                if (mc != null) 
                {
                    res.EndToken = mc.EndToken;
                    res.Morph = mc.Morph;
                }
            }
            if ((res.BeginToken == res.EndToken && val == 1 && res.BeginToken.Chars.IsAllLower) && res.BeginToken.Morph.Language.IsUa) 
                return null;
            return res;
        }
        static int _romVal(char ch)
        {
            if (ch == 'Х' || ch == 'X') 
                return 10;
            if (ch == 'І' || ch == 'I') 
                return 1;
            if (ch == 'V') 
                return 5;
            if (ch == 'L') 
                return 50;
            if (ch == 'C' || ch == 'С') 
                return 100;
            return 0;
        }
        static bool _isRomVal(string str)
        {
            foreach (char ch in str) 
            {
                if (_romVal(ch) < 1) 
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Выделить число в римской записи в обратном порядке
        /// </summary>
        /// <param name="token">токен на предполагаемой римской цифрой</param>
        /// <return>число-метатокен или null</return>
        public static Pullenti.Ner.NumberToken TryParseRomanBack(Pullenti.Ner.Token token)
        {
            Pullenti.Ner.Token t = token;
            if (t == null) 
                return null;
            if ((t.Chars.IsAllLower && t.Previous != null && t.Previous.IsHiphen) && t.Previous.Previous != null) 
                t = token.Previous.Previous;
            Pullenti.Ner.NumberToken res = null;
            for (; t != null; t = t.Previous) 
            {
                Pullenti.Ner.NumberToken nt = TryParseRoman(t);
                if (nt != null) 
                {
                    if (nt.EndToken == token) 
                        res = nt;
                    else 
                        break;
                }
                if (t.IsWhitespaceAfter) 
                    break;
            }
            return res;
        }
        /// <summary>
        /// Это выделение числительных типа 16-летие, 50-летний
        /// </summary>
        /// <param name="t">начальный токен</param>
        /// <return>числовой метатокен или null</return>
        public static Pullenti.Ner.NumberToken TryParseAge(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
            Pullenti.Ner.Token ntNext = null;
            if (nt != null) 
                ntNext = nt.Next;
            else 
            {
                if (t.IsValue("AGED", null) && (t.Next is Pullenti.Ner.NumberToken)) 
                    return new Pullenti.Ner.NumberToken(t, t.Next, (t.Next as Pullenti.Ner.NumberToken).Value, Pullenti.Ner.NumberSpellingType.Age);
                if ((((nt = TryParseRoman(t)))) != null) 
                    ntNext = nt.EndToken.Next;
            }
            if (nt != null) 
            {
                if (ntNext != null) 
                {
                    Pullenti.Ner.Token t1 = ntNext;
                    if (t1.IsHiphen) 
                        t1 = t1.Next;
                    if (t1 is Pullenti.Ner.TextToken) 
                    {
                        string v = (t1 as Pullenti.Ner.TextToken).Term;
                        if ((v == "ЛЕТ" || v == "ЛЕТИЯ" || v == "ЛЕТИЕ") || v == "РІЧЧЯ") 
                            return new Pullenti.Ner.NumberToken(t, t1, nt.Value, Pullenti.Ner.NumberSpellingType.Age) { Morph = t1.Morph };
                        if (t1.IsValue("ЛЕТНИЙ", "РІЧНИЙ")) 
                            return new Pullenti.Ner.NumberToken(t, t1, nt.Value, Pullenti.Ner.NumberSpellingType.Age) { Morph = t1.Morph };
                        if (v == "Л" || ((v == "Р" && nt.Morph.Language.IsUa))) 
                            return new Pullenti.Ner.NumberToken(t, (t1.Next != null && t1.Next.IsChar('.') ? t1.Next : t1), nt.Value, Pullenti.Ner.NumberSpellingType.Age);
                    }
                }
                return null;
            }
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return null;
            string s = tt.Term;
            if (Pullenti.Morph.LanguageHelper.EndsWithEx(s, "ЛЕТИЕ", "ЛЕТИЯ", "РІЧЧЯ", null)) 
            {
                Termin term = m_Nums.Find(s.Substring(0, s.Length - 5));
                if (term != null) 
                    return new Pullenti.Ner.NumberToken(tt, tt, term.Tag.ToString(), Pullenti.Ner.NumberSpellingType.Age) { Morph = tt.Morph };
            }
            s = tt.Lemma;
            if (Pullenti.Morph.LanguageHelper.EndsWithEx(s, "ЛЕТНИЙ", "РІЧНИЙ", null, null)) 
            {
                Termin term = m_Nums.Find(s.Substring(0, s.Length - 6));
                if (term != null) 
                    return new Pullenti.Ner.NumberToken(tt, tt, term.Tag.ToString(), Pullenti.Ner.NumberSpellingType.Age) { Morph = tt.Morph };
            }
            return null;
        }
        /// <summary>
        /// Выделение годовщин и летий (XX-летие) ...
        /// </summary>
        /// <param name="t">начальный токен</param>
        /// <return>числовой метатокен или null</return>
        public static Pullenti.Ner.NumberToken TryParseAnniversary(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
            Pullenti.Ner.Token t1 = null;
            if (nt != null) 
                t1 = nt.Next;
            else 
            {
                if ((((nt = TryParseRoman(t)))) == null) 
                {
                    if (t is Pullenti.Ner.TextToken) 
                    {
                        string v = (t as Pullenti.Ner.TextToken).Term;
                        int num = 0;
                        if (v.EndsWith("ЛЕТИЯ") || v.EndsWith("ЛЕТИЕ")) 
                        {
                            if (v.StartsWith("ВОСЕМЬСОТ") || v.StartsWith("ВОСЬМИСОТ")) 
                                num = 800;
                        }
                        if (num > 0) 
                            return new Pullenti.Ner.NumberToken(t, t, num.ToString(), Pullenti.Ner.NumberSpellingType.Age);
                    }
                    return null;
                }
                t1 = nt.EndToken.Next;
            }
            if (t1 == null) 
                return null;
            if (t1.IsHiphen) 
                t1 = t1.Next;
            if (t1 is Pullenti.Ner.TextToken) 
            {
                string v = (t1 as Pullenti.Ner.TextToken).Term;
                if ((v == "ЛЕТ" || v == "ЛЕТИЯ" || v == "ЛЕТИЕ") || t1.IsValue("ГОДОВЩИНА", null)) 
                    return new Pullenti.Ner.NumberToken(t, t1, nt.Value, Pullenti.Ner.NumberSpellingType.Age);
                if (t1.Morph.Language.IsUa) 
                {
                    if (v == "РОКІВ" || v == "РІЧЧЯ" || t1.IsValue("РІЧНИЦЯ", null)) 
                        return new Pullenti.Ner.NumberToken(t, t1, nt.Value, Pullenti.Ner.NumberSpellingType.Age);
                }
            }
            return null;
        }
        static string[] m_Samples = new string[] {"ДЕСЯТЫЙ", "ПЕРВЫЙ", "ВТОРОЙ", "ТРЕТИЙ", "ЧЕТВЕРТЫЙ", "ПЯТЫЙ", "ШЕСТОЙ", "СЕДЬМОЙ", "ВОСЬМОЙ", "ДЕВЯТЫЙ"};
        static Pullenti.Ner.MetaToken AnalizeNumberTail(Pullenti.Ner.TextToken tt, string val)
        {
            if (!(tt is Pullenti.Ner.TextToken)) 
                return null;
            string s = tt.Term;
            Pullenti.Ner.MorphCollection mc = null;
            if (!tt.Chars.IsLetter) 
            {
                if (((s == "<" || s == "(")) && (tt.Next is Pullenti.Ner.TextToken)) 
                {
                    s = (tt.Next as Pullenti.Ner.TextToken).Term;
                    if ((s == "TH" || s == "ST" || s == "RD") || s == "ND") 
                    {
                        if (tt.Next.Next != null && tt.Next.Next.IsCharOf(">)")) 
                        {
                            mc = new Pullenti.Ner.MorphCollection();
                            mc.Class = Pullenti.Morph.MorphClass.Adjective;
                            mc.Language = Pullenti.Morph.MorphLang.EN;
                            return new Pullenti.Ner.MetaToken(tt, tt.Next.Next) { Morph = mc };
                        }
                    }
                }
                return null;
            }
            if ((s == "TH" || s == "ST" || s == "RD") || s == "ND") 
            {
                mc = new Pullenti.Ner.MorphCollection();
                mc.Class = Pullenti.Morph.MorphClass.Adjective;
                mc.Language = Pullenti.Morph.MorphLang.EN;
                return new Pullenti.Ner.MetaToken(tt, tt) { Morph = mc };
            }
            if (!tt.Chars.IsCyrillicLetter) 
                return null;
            if (!tt.IsWhitespaceAfter) 
            {
                if (tt.Next != null && tt.Next.Chars.IsLetter) 
                    return null;
                if (tt.LengthChar == 1 && ((tt.IsValue("X", null) || tt.IsValue("Х", null)))) 
                    return null;
            }
            if (!tt.Chars.IsAllLower) 
            {
                string ss = (tt as Pullenti.Ner.TextToken).Term;
                if (ss == "Я" || ss == "Й" || ss == "Е") 
                {
                }
                else if (ss.Length == 2 && ((ss[1] == 'Я' || ss[1] == 'Й' || ss[1] == 'Е'))) 
                {
                }
                else 
                    return null;
            }
            if ((tt as Pullenti.Ner.TextToken).Term == "М") 
            {
                if (tt.Previous == null || !tt.Previous.IsHiphen) 
                    return null;
            }
            if (string.IsNullOrEmpty(val)) 
                return null;
            int dig = (int)((val[val.Length - 1] - '0'));
            if ((dig < 0) || dig >= 10) 
                return null;
            List<Pullenti.Morph.MorphWordForm> vars = Pullenti.Morph.MorphologyService.GetAllWordforms(m_Samples[dig], null);
            if (vars == null || vars.Count == 0) 
                return null;
            foreach (Pullenti.Morph.MorphWordForm v in vars) 
            {
                if (v.Class.IsAdjective && Pullenti.Morph.LanguageHelper.EndsWith(v.NormalCase, s) && v.Number != Pullenti.Morph.MorphNumber.Undefined) 
                {
                    if (mc == null) 
                        mc = new Pullenti.Ner.MorphCollection();
                    bool ok = false;
                    foreach (Pullenti.Morph.MorphBaseInfo it in mc.Items) 
                    {
                        if (it.Class == v.Class && it.Number == v.Number && ((it.Gender == v.Gender || v.Number == Pullenti.Morph.MorphNumber.Plural))) 
                        {
                            it.Case |= v.Case;
                            ok = true;
                            break;
                        }
                    }
                    if (!ok) 
                    {
                        Pullenti.Morph.MorphBaseInfo mm = new Pullenti.Morph.MorphBaseInfo();
                        mm.CopyFrom(v);
                        mc.AddItem(mm);
                    }
                }
            }
            if (tt.Morph.Language.IsUa && mc == null && s == "Ї") 
            {
                mc = new Pullenti.Ner.MorphCollection();
                mc.AddItem(new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective });
            }
            if (mc != null) 
                return new Pullenti.Ner.MetaToken(tt, tt) { Morph = mc };
            if ((((s.Length < 3) && !tt.IsWhitespaceBefore && tt.Previous != null) && tt.Previous.IsHiphen && !tt.Previous.IsWhitespaceBefore) && tt.WhitespacesAfterCount == 1 && s != "А") 
                return new Pullenti.Ner.MetaToken(tt, tt) { Morph = new Pullenti.Ner.MorphCollection() { Class = Pullenti.Morph.MorphClass.Adjective } };
            return null;
        }
        static Pullenti.Ner.Token _tryParseFloat(Pullenti.Ner.NumberToken t, out double d, bool noWs)
        {
            d = 0;
            if (t == null || t.Next == null || t.Typ != Pullenti.Ner.NumberSpellingType.Digit) 
                return null;
            for (Pullenti.Ner.Token tt = t.BeginToken; tt != null && tt.EndChar <= t.EndChar; tt = tt.Next) 
            {
                if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter) 
                    return null;
            }
            AnalysisKit kit = t.Kit;
            List<Pullenti.Ner.NumberToken> ns = null;
            List<char> sps = null;
            for (Pullenti.Ner.Token t1 = (Pullenti.Ner.Token)t; t1 != null; t1 = t1.Next) 
            {
                if (t1.Next == null) 
                    break;
                if (((t1.Next is Pullenti.Ner.NumberToken) && (t1.WhitespacesAfterCount < 3) && (t1.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) && t1.Next.LengthChar == 3) 
                {
                    if (ns == null) 
                    {
                        ns = new List<Pullenti.Ner.NumberToken>();
                        ns.Add(t);
                        sps = new List<char>();
                    }
                    else if (sps[0] != ' ') 
                        return null;
                    ns.Add(t1.Next as Pullenti.Ner.NumberToken);
                    sps.Add(' ');
                    continue;
                }
                if ((t1.Next.IsCharOf(",.") && (t1.Next.Next is Pullenti.Ner.NumberToken) && (t1.Next.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) && (t1.WhitespacesAfterCount < 2) && (t1.Next.WhitespacesAfterCount < 2)) 
                {
                    if (noWs) 
                    {
                        if (t1.IsWhitespaceAfter || t1.Next.IsWhitespaceAfter) 
                            break;
                    }
                    if (ns == null) 
                    {
                        ns = new List<Pullenti.Ner.NumberToken>();
                        ns.Add(t);
                        sps = new List<char>();
                    }
                    else if (t1.Next.IsWhitespaceAfter && t1.Next.Next.LengthChar != 3 && ((t1.Next.IsChar('.') ? '.' : ',')) == sps[sps.Count - 1]) 
                        break;
                    ns.Add(t1.Next.Next as Pullenti.Ner.NumberToken);
                    sps.Add((t1.Next.IsChar('.') ? '.' : ','));
                    t1 = t1.Next;
                    continue;
                }
                break;
            }
            if (sps == null) 
                return null;
            bool isLastDrob = false;
            bool notSetDrob = false;
            bool merge = false;
            char m_PrevPointChar = '.';
            if (sps.Count == 1) 
            {
                if (sps[0] == ' ') 
                    isLastDrob = false;
                else if (ns[1].LengthChar != 3) 
                {
                    isLastDrob = true;
                    if (ns.Count == 2) 
                    {
                        if (ns[1].EndToken.Chars.IsLetter) 
                            merge = true;
                        else if (ns[1].EndToken.IsChar('.') && ns[1].EndToken.Previous != null && ns[1].EndToken.Previous.Chars.IsLetter) 
                            merge = true;
                        if (ns[1].IsWhitespaceBefore) 
                        {
                            if ((ns[1].EndToken is Pullenti.Ner.TextToken) && (ns[1].EndToken as Pullenti.Ner.TextToken).Term.EndsWith("000")) 
                                return null;
                        }
                    }
                }
                else if (ns[0].LengthChar > 3 || ns[0].RealValue == 0) 
                    isLastDrob = true;
                else 
                {
                    bool ok = true;
                    if (ns.Count == 2 && ns[1].LengthChar == 3) 
                    {
                        TerminToken ttt = Pullenti.Ner.Core.Internal.NumberExHelper.m_Postfixes.TryParse(ns[1].EndToken.Next, TerminParseAttr.No);
                        if (ttt != null && ((NumberExType)ttt.Termin.Tag) == NumberExType.Money) 
                        {
                            isLastDrob = false;
                            ok = false;
                            notSetDrob = false;
                        }
                        else if (ns[1].EndToken.Next != null && ns[1].EndToken.Next.IsChar('(') && (ns[1].EndToken.Next.Next is Pullenti.Ner.NumberToken)) 
                        {
                            Pullenti.Ner.NumberToken nt1 = ns[1].EndToken.Next.Next as Pullenti.Ner.NumberToken;
                            if (nt1.RealValue == (((ns[0].RealValue * 1000) + ns[1].RealValue))) 
                            {
                                isLastDrob = false;
                                ok = false;
                                notSetDrob = false;
                            }
                        }
                    }
                    if (ok) 
                    {
                        if (t.Kit.MiscData.ContainsKey("pt")) 
                            m_PrevPointChar = (char)t.Kit.MiscData["pt"];
                        if (m_PrevPointChar == sps[0]) 
                        {
                            isLastDrob = true;
                            notSetDrob = true;
                        }
                        else 
                        {
                            isLastDrob = false;
                            notSetDrob = true;
                        }
                    }
                }
            }
            else 
            {
                char last = sps[sps.Count - 1];
                if (last == ' ' && sps[0] != last) 
                    return null;
                for (int i = 0; i < (sps.Count - 1); i++) 
                {
                    if (sps[i] != sps[0]) 
                        return null;
                    else if (ns[i + 1].LengthChar != 3) 
                        return null;
                }
                if (sps[0] != last) 
                    isLastDrob = true;
                else if (ns[ns.Count - 1].LengthChar != 3) 
                    return null;
                if (ns[0].LengthChar > 3) 
                    return null;
            }
            for (int i = 0; i < ns.Count; i++) 
            {
                if ((i < (ns.Count - 1)) || !isLastDrob) 
                {
                    if (i == 0) 
                        d = ns[i].RealValue;
                    else 
                        d = (d * 1000) + ns[i].RealValue;
                    if (i == (ns.Count - 1) && !notSetDrob) 
                    {
                        if (sps[sps.Count - 1] == ',') 
                            m_PrevPointChar = '.';
                        else if (sps[sps.Count - 1] == '.') 
                            m_PrevPointChar = ',';
                    }
                }
                else 
                {
                    if (!notSetDrob) 
                    {
                        m_PrevPointChar = sps[sps.Count - 1];
                        if (m_PrevPointChar == ',') 
                        {
                        }
                    }
                    double f2;
                    if (merge) 
                    {
                        string sss = ns[i].Value.ToString();
                        int kkk;
                        for (kkk = 0; kkk < (sss.Length - ns[i].BeginToken.LengthChar); kkk++) 
                        {
                            d *= 10;
                        }
                        f2 = ns[i].RealValue;
                        for (kkk = 0; kkk < ns[i].BeginToken.LengthChar; kkk++) 
                        {
                            f2 /= 10;
                        }
                        d += f2;
                    }
                    else 
                    {
                        f2 = ns[i].RealValue;
                        for (int kkk = 0; kkk < ns[i].LengthChar; kkk++) 
                        {
                            f2 /= 10;
                        }
                        d += f2;
                    }
                }
            }
            if (kit.MiscData.ContainsKey("pt")) 
                kit.MiscData["pt"] = m_PrevPointChar;
            else 
                kit.MiscData.Add("pt", m_PrevPointChar);
            return ns[ns.Count - 1];
        }
        /// <summary>
        /// Выделить действительное число, знак также выделяется, 
        /// разделители дроби могут быть точка или запятая, разделителями тысячных 
        /// могут быть точки, пробелы и запятые.
        /// </summary>
        /// <param name="t">начальный токен</param>
        /// <param name="canBeInteger">число может быть целым</param>
        /// <param name="noWhitespace">не должно быть пробелов</param>
        /// <return>числовой метатокен или null</return>
        public static Pullenti.Ner.NumberToken TryParseRealNumber(Pullenti.Ner.Token t, bool canBeInteger = false, bool noWhitespace = false)
        {
            bool isNot = false;
            Pullenti.Ner.Token t0 = t;
            if (t != null) 
            {
                if (t.IsHiphen || t.IsValue("МИНУС", null)) 
                {
                    t = t.Next;
                    isNot = true;
                }
                else if (t.IsChar('+') || t.IsValue("ПЛЮС", null)) 
                    t = t.Next;
            }
            if ((t is Pullenti.Ner.TextToken) && ((t.IsValue("НОЛЬ", null) || t.IsValue("НУЛЬ", null)))) 
            {
                if (t.Next == null) 
                    return new Pullenti.Ner.NumberToken(t, t, "0", Pullenti.Ner.NumberSpellingType.Words);
                if (t.Next.IsValue("ЦЕЛЫЙ", null)) 
                    t = t.Next;
                Pullenti.Ner.NumberToken res0 = new Pullenti.Ner.NumberToken(t, t.Next, "0", Pullenti.Ner.NumberSpellingType.Words);
                t = t.Next;
                if ((t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).IntValue != null) 
                {
                    int val = (t as Pullenti.Ner.NumberToken).IntValue.Value;
                    if (t.Next != null && val > 0) 
                    {
                        if (t.Next.IsValue("ДЕСЯТЫЙ", null)) 
                        {
                            res0.EndToken = t.Next;
                            res0.RealValue = ((double)val) / 10;
                        }
                        else if (t.Next.IsValue("СОТЫЙ", null)) 
                        {
                            res0.EndToken = t.Next;
                            res0.RealValue = ((double)val) / 100;
                        }
                        else if (t.Next.IsValue("ТЫСЯЧНЫЙ", null)) 
                        {
                            res0.EndToken = t.Next;
                            res0.RealValue = ((double)val) / 1000;
                        }
                    }
                    if (res0.RealValue == 0) 
                    {
                        res0.EndToken = t;
                        res0.Value = string.Format("0.{0}", val);
                    }
                }
                return res0;
            }
            if (t is Pullenti.Ner.TextToken) 
            {
                TerminToken tok = m_AfterPoints.TryParse(t, TerminParseAttr.No);
                if (tok != null) 
                {
                    NumberExToken res0 = new NumberExToken(t, tok.EndToken, null, Pullenti.Ner.NumberSpellingType.Words, NumberExType.Undefined);
                    res0.RealValue = (double)(tok.Termin.Tag);
                    return res0;
                }
            }
            if (t == null) 
                return null;
            if (!(t is Pullenti.Ner.NumberToken)) 
            {
                if (t.IsValue("СОТНЯ", null)) 
                    return new Pullenti.Ner.NumberToken(t, t, "100", Pullenti.Ner.NumberSpellingType.Words);
                if (t.IsValue("ТЫЩА", null) || t.IsValue("ТЫСЯЧА", null)) 
                    return new Pullenti.Ner.NumberToken(t, t, "1000", Pullenti.Ner.NumberSpellingType.Words);
                return null;
            }
            if (t.Next != null && t.Next.IsValue("ЦЕЛЫЙ", null) && (((t.Next.Next is Pullenti.Ner.NumberToken) || (((t.Next.Next is Pullenti.Ner.TextToken) && t.Next.Next.IsValue("НОЛЬ", null)))))) 
            {
                NumberExToken res0 = new NumberExToken(t, t.Next, (t as Pullenti.Ner.NumberToken).Value, Pullenti.Ner.NumberSpellingType.Words, NumberExType.Undefined);
                t = t.Next.Next;
                double val = (double)0;
                if (t is Pullenti.Ner.TextToken) 
                {
                    res0.EndToken = t;
                    t = t.Next;
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    res0.EndToken = t;
                    val = (t as Pullenti.Ner.NumberToken).RealValue;
                    t = t.Next;
                }
                if (t != null) 
                {
                    if (t.IsValue("ДЕСЯТЫЙ", null)) 
                    {
                        res0.EndToken = t;
                        res0.RealValue = ((((double)val) / 10)) + res0.RealValue;
                    }
                    else if (t.IsValue("СОТЫЙ", null)) 
                    {
                        res0.EndToken = t;
                        res0.RealValue = ((((double)val) / 100)) + res0.RealValue;
                    }
                    else if (t.IsValue("ТЫСЯЧНЫЙ", null)) 
                    {
                        res0.EndToken = t;
                        res0.RealValue = ((((double)val) / 1000)) + res0.RealValue;
                    }
                }
                if (res0.RealValue == 0) 
                {
                    string str = string.Format("0.{0}", val);
                    double dd = (double)0;
                    if (double.TryParse(str, out dd)) 
                    {
                    }
                    else if (double.TryParse(str.Replace('.', ','), out dd)) 
                    {
                    }
                    else 
                        return null;
                    res0.RealValue = dd + res0.RealValue;
                }
                return res0;
            }
            double d;
            Pullenti.Ner.Token tt = _tryParseFloat(t as Pullenti.Ner.NumberToken, out d, noWhitespace);
            if (tt == null) 
            {
                if ((t.Next == null || t.IsWhitespaceAfter || t.Next.Chars.IsLetter) || canBeInteger) 
                {
                    tt = t;
                    d = (t as Pullenti.Ner.NumberToken).RealValue;
                }
                else 
                    return null;
            }
            if (isNot) 
                d = -d;
            if (tt.Next != null && tt.Next.IsValue("ДЕСЯТОК", null)) 
            {
                d *= 10;
                tt = tt.Next;
            }
            return new NumberExToken(t0, tt, "", Pullenti.Ner.NumberSpellingType.Digit, NumberExType.Undefined) { RealValue = d };
        }
        /// <summary>
        /// Преобразовать целое число в записанное буквами числительное в нужном роде и числе именительного падежа. 
        /// Например, 5 жен.ед. - ПЯТАЯ,  26 мн. - ДВАДЦАТЬ ШЕСТЫЕ.
        /// </summary>
        /// <param name="value">целочисленное значение</param>
        /// <param name="gender">род</param>
        /// <param name="num">число</param>
        /// <return>значение</return>
        public static string GetNumberAdjective(int value, Pullenti.Morph.MorphGender gender, Pullenti.Morph.MorphNumber num)
        {
            if ((value < 1) || value >= 100) 
                return null;
            string[] words = null;
            if (num == Pullenti.Morph.MorphNumber.Plural) 
                words = m_PluralNumberWords;
            else if (gender == Pullenti.Morph.MorphGender.Feminie) 
                words = m_WomanNumberWords;
            else if (gender == Pullenti.Morph.MorphGender.Neuter) 
                words = m_NeutralNumberWords;
            else 
                words = m_ManNumberWords;
            if (value < 20) 
                return words[value - 1];
            int i = value / 10;
            int j = value % 10;
            i -= 2;
            if (i >= m_DecDumberWords.Length) 
                return null;
            if (j > 0) 
                return string.Format("{0} {1}", m_DecDumberWords[i], words[j - 1]);
            string[] decs = null;
            if (num == Pullenti.Morph.MorphNumber.Plural) 
                decs = m_PluralDecDumberWords;
            else if (gender == Pullenti.Morph.MorphGender.Feminie) 
                decs = m_WomanDecDumberWords;
            else if (gender == Pullenti.Morph.MorphGender.Neuter) 
                decs = m_NeutralDecDumberWords;
            else 
                decs = m_ManDecDumberWords;
            return decs[i];
        }
        static string[] m_ManNumberWords = new string[] {"ПЕРВЫЙ", "ВТОРОЙ", "ТРЕТИЙ", "ЧЕТВЕРТЫЙ", "ПЯТЫЙ", "ШЕСТОЙ", "СЕДЬМОЙ", "ВОСЬМОЙ", "ДЕВЯТЫЙ", "ДЕСЯТЫЙ", "ОДИННАДЦАТЫЙ", "ДВЕНАДЦАТЫЙ", "ТРИНАДЦАТЫЙ", "ЧЕТЫРНАДЦАТЫЙ", "ПЯТНАДЦАТЫЙ", "ШЕСТНАДЦАТЫЙ", "СЕМНАДЦАТЫЙ", "ВОСЕМНАДЦАТЫЙ", "ДЕВЯТНАДЦАТЫЙ"};
        static string[] m_NeutralNumberWords = new string[] {"ПЕРВОЕ", "ВТОРОЕ", "ТРЕТЬЕ", "ЧЕТВЕРТОЕ", "ПЯТОЕ", "ШЕСТОЕ", "СЕДЬМОЕ", "ВОСЬМОЕ", "ДЕВЯТОЕ", "ДЕСЯТОЕ", "ОДИННАДЦАТОЕ", "ДВЕНАДЦАТОЕ", "ТРИНАДЦАТОЕ", "ЧЕТЫРНАДЦАТОЕ", "ПЯТНАДЦАТОЕ", "ШЕСТНАДЦАТОЕ", "СЕМНАДЦАТОЕ", "ВОСЕМНАДЦАТОЕ", "ДЕВЯТНАДЦАТОЕ"};
        static string[] m_WomanNumberWords = new string[] {"ПЕРВАЯ", "ВТОРАЯ", "ТРЕТЬЯ", "ЧЕТВЕРТАЯ", "ПЯТАЯ", "ШЕСТАЯ", "СЕДЬМАЯ", "ВОСЬМАЯ", "ДЕВЯТАЯ", "ДЕСЯТАЯ", "ОДИННАДЦАТАЯ", "ДВЕНАДЦАТАЯ", "ТРИНАДЦАТАЯ", "ЧЕТЫРНАДЦАТАЯ", "ПЯТНАДЦАТАЯ", "ШЕСТНАДЦАТАЯ", "СЕМНАДЦАТАЯ", "ВОСЕМНАДЦАТАЯ", "ДЕВЯТНАДЦАТАЯ"};
        static string[] m_PluralNumberWords = new string[] {"ПЕРВЫЕ", "ВТОРЫЕ", "ТРЕТЬИ", "ЧЕТВЕРТЫЕ", "ПЯТЫЕ", "ШЕСТЫЕ", "СЕДЬМЫЕ", "ВОСЬМЫЕ", "ДЕВЯТЫЕ", "ДЕСЯТЫЕ", "ОДИННАДЦАТЫЕ", "ДВЕНАДЦАТЫЕ", "ТРИНАДЦАТЫЕ", "ЧЕТЫРНАДЦАТЫЕ", "ПЯТНАДЦАТЫЕ", "ШЕСТНАДЦАТЫЕ", "СЕМНАДЦАТЫЕ", "ВОСЕМНАДЦАТЫЕ", "ДЕВЯТНАДЦАТЫЕ"};
        static string[] m_DecDumberWords = new string[] {"ДВАДЦАТЬ", "ТРИДЦАТЬ", "СОРОК", "ПЯТЬДЕСЯТ", "ШЕСТЬДЕСЯТ", "СЕМЬДЕСЯТ", "ВОСЕМЬДЕСЯТ", "ДЕВЯНОСТО"};
        static string[] m_ManDecDumberWords = new string[] {"ДВАДЦАТЫЙ", "ТРИДЦАТЫЙ", "СОРОКОВОЙ", "ПЯТЬДЕСЯТЫЙ", "ШЕСТЬДЕСЯТЫЙ", "СЕМЬДЕСЯТЫЙ", "ВОСЕМЬДЕСЯТЫЙ", "ДЕВЯНОСТЫЙ"};
        static string[] m_WomanDecDumberWords = new string[] {"ДВАДЦАТАЯ", "ТРИДЦАТАЯ", "СОРОКОВАЯ", "ПЯТЬДЕСЯТАЯ", "ШЕСТЬДЕСЯТАЯ", "СЕМЬДЕСЯТАЯ", "ВОСЕМЬДЕСЯТАЯ", "ДЕВЯНОСТАЯ"};
        static string[] m_NeutralDecDumberWords = new string[] {"ДВАДЦАТОЕ", "ТРИДЦАТОЕ", "СОРОКОВОЕ", "ПЯТЬДЕСЯТОЕ", "ШЕСТЬДЕСЯТОЕ", "СЕМЬДЕСЯТОЕ", "ВОСЕМЬДЕСЯТОЕ", "ДЕВЯНОСТОЕ"};
        static string[] m_PluralDecDumberWords = new string[] {"ДВАДЦАТЫЕ", "ТРИДЦАТЫЕ", "СОРОКОВЫЕ", "ПЯТЬДЕСЯТЫЕ", "ШЕСТЬДЕСЯТЫЕ", "СЕМЬДЕСЯТЫЕ", "ВОСЕМЬДЕСЯТЫЕ", "ДЕВЯНОСТЫЕ"};
        static string[] m_100Words = new string[] {"СТО", "ДВЕСТИ", "ТРИСТА", "ЧЕТЫРЕСТА", "ПЯТЬСОТ", "ШЕСТЬСОТ", "СЕМЬСОТ", "ВОСЕМЬСОТ", "ДЕВЯТЬСОТ"};
        static string[] m_10Words = new string[] {"ДЕСЯТЬ", "ДВАДЦАТЬ", "ТРИДЦАТЬ", "СОРОК", "ПЯТЬДЕСЯТ", "ШЕСТЬДЕСЯТ", "СЕМЬДЕСЯТ", "ВОСЕМЬДЕСЯТ", "ДЕВЯНОСТО"};
        static string[] m_1Words = new string[] {"НОЛЬ", "ОДИН", "ДВА", "ТРИ", "ЧЕТЫРЕ", "ПЯТЬ", "ШЕСТЬ", "СЕМЬ", "ВОСЕМЬ", "ДЕВЯТЬ", "ДЕСЯТЬ", "ОДИННАДЦАТЬ", "ДВЕНАДЦАТЬ", "ТРИНАДЦАТЬ", "ЧЕТЫРНАДЦАТЬ", "ПЯТНАДЦАТЬ", "ШЕСТНАДЦАТЬ", "СЕМНАДЦАТЬ", "ВОСЕМНАДЦАТЬ", "ДЕВЯТНАДЦАТЬ"};
        internal static string[] m_Romans = new string[] {"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "XXI", "XXII", "XXIII", "XXIV", "XXV", "XXVI", "XXVII", "XXVIII", "XXIX", "XXX"};
        /// <summary>
        /// Получить для числа римскую запись
        /// </summary>
        /// <param name="val">целое число</param>
        /// <return>римская запись</return>
        public static string GetNumberRoman(int val)
        {
            if (val > 0 && val <= m_Romans.Length) 
                return m_Romans[val - 1];
            return val.ToString();
        }
        /// <summary>
        /// Получить строковое представление целого числа. Например, GetNumberString(38, "попугай") => "тридцать восемь попугаев".
        /// </summary>
        /// <param name="val">значение</param>
        /// <param name="units">единицы измерения (если не null, то они тоже будут преобразовываться в нужное число)</param>
        /// <return>строковое представление (пока на русском языке)</return>
        public static string GetNumberString(int val, string units = null)
        {
            if (val < 0) 
                return "минус " + GetNumberString(-val, units);
            string res;
            if (val >= 1000000000) 
            {
                int vv = val / 1000000000;
                res = GetNumberString(vv, "миллиард");
                vv = val % 1000000000;
                if (vv != 0) 
                    res = string.Format("{0} {1}", res, GetNumberString(vv, units));
                else if (units != null) 
                    res = string.Format("{0} {1}", res, MiscHelper.GetTextMorphVarByCaseAndNumberEx(units, Pullenti.Morph.MorphCase.Genitive, Pullenti.Morph.MorphNumber.Plural, null));
                return res.ToLower();
            }
            if (val >= 1000000) 
            {
                int vv = val / 1000000;
                res = GetNumberString(vv, "миллион");
                vv = val % 1000000;
                if (vv != 0) 
                    res = string.Format("{0} {1}", res, GetNumberString(vv, units));
                else if (units != null) 
                    res = string.Format("{0} {1}", res, MiscHelper.GetTextMorphVarByCaseAndNumberEx(units, Pullenti.Morph.MorphCase.Genitive, Pullenti.Morph.MorphNumber.Plural, null));
                return res.ToLower();
            }
            if (val >= 1000) 
            {
                int vv = val / 1000;
                res = GetNumberString(vv, "тысяча");
                vv = val % 1000;
                if (vv != 0) 
                    res = string.Format("{0} {1}", res, GetNumberString(vv, units));
                else if (units != null) 
                    res = string.Format("{0} {1}", res, MiscHelper.GetTextMorphVarByCaseAndNumberEx(units, Pullenti.Morph.MorphCase.Genitive, Pullenti.Morph.MorphNumber.Plural, null));
                return res.ToLower();
            }
            if (val >= 100) 
            {
                int vv = val / 100;
                res = m_100Words[vv - 1];
                vv = val % 100;
                if (vv != 0) 
                    res = string.Format("{0} {1}", res, GetNumberString(vv, units));
                else if (units != null) 
                    res = string.Format("{0} {1}", res, MiscHelper.GetTextMorphVarByCaseAndNumberEx(units, Pullenti.Morph.MorphCase.Genitive, Pullenti.Morph.MorphNumber.Plural, null));
                return res.ToLower();
            }
            if (val >= 20) 
            {
                int vv = val / 10;
                res = m_10Words[vv - 1];
                vv = val % 10;
                if (vv != 0) 
                    res = string.Format("{0} {1}", res, GetNumberString(vv, units));
                else if (units != null) 
                    res = string.Format("{0} {1}", res, MiscHelper.GetTextMorphVarByCaseAndNumberEx(units, Pullenti.Morph.MorphCase.Genitive, Pullenti.Morph.MorphNumber.Plural, null));
                return res.ToLower();
            }
            if (units != null) 
            {
                if (val == 1) 
                {
                    Pullenti.Morph.MorphWordForm bi = Pullenti.Morph.MorphologyService.GetWordBaseInfo(units.ToUpper(), null, false, false);
                    if (((bi.Gender & Pullenti.Morph.MorphGender.Feminie)) == Pullenti.Morph.MorphGender.Feminie) 
                        return "одна " + units;
                    if (((bi.Gender & Pullenti.Morph.MorphGender.Neuter)) == Pullenti.Morph.MorphGender.Neuter) 
                        return "одно " + units;
                    return "один " + units;
                }
                if (val == 2) 
                {
                    Pullenti.Morph.MorphWordForm bi = Pullenti.Morph.MorphologyService.GetWordBaseInfo(units.ToUpper(), null, false, false);
                    if (((bi.Gender & Pullenti.Morph.MorphGender.Feminie)) == Pullenti.Morph.MorphGender.Feminie) 
                        return "две " + MiscHelper.GetTextMorphVarByCaseAndNumberEx(units, null, Pullenti.Morph.MorphNumber.Plural, null);
                }
                return string.Format("{0} {1}", m_1Words[val].ToLower(), MiscHelper.GetTextMorphVarByCaseAndNumberEx(units, Pullenti.Morph.MorphCase.Genitive, Pullenti.Morph.MorphNumber.Undefined, val.ToString()));
            }
            return m_1Words[val].ToLower();
        }
        // Выделение стандартных мер, типа: 10 кв.м.
        // УСТАРЕЛО. Вместо этого лучше использовать возможности MeasureReferent.
        public static NumberExToken TryParseNumberWithPostfix(Pullenti.Ner.Token t)
        {
            return Pullenti.Ner.Core.Internal.NumberExHelper.TryParseNumberWithPostfix(t);
        }
        // Это попробовать только тип (постфикс) без самого числа.
        // Например, куб.м.
        public static NumberExToken TryParsePostfixOnly(Pullenti.Ner.Token t)
        {
            return Pullenti.Ner.Core.Internal.NumberExHelper.TryAttachPostfixOnly(t);
        }
        // Если это обозначение денежной единицы (н-р, $), то возвращает код валюты
        internal static string IsMoneyChar(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken) || t.LengthChar != 1) 
                return null;
            char ch = (t as Pullenti.Ner.TextToken).Term[0];
            if (ch == '$') 
                return "USD";
            if (ch == '£' || ch == ((char)0xA3) || ch == ((char)0x20A4)) 
                return "GBP";
            if (ch == '€') 
                return "EUR";
            if (ch == '¥' || ch == ((char)0xA5)) 
                return "JPY";
            if (ch == ((char)0x20A9)) 
                return "KRW";
            if (ch == ((char)0xFFE5) || ch == 'Ұ' || ch == 'Ұ') 
                return "CNY";
            if (ch == ((char)0x20BD)) 
                return "RUB";
            if (ch == ((char)0x20B4)) 
                return "UAH";
            if (ch == ((char)0x20AB)) 
                return "VND";
            if (ch == ((char)0x20AD)) 
                return "LAK";
            if (ch == ((char)0x20BA)) 
                return "TRY";
            if (ch == ((char)0x20B1)) 
                return "PHP";
            if (ch == ((char)0x17DB)) 
                return "KHR";
            if (ch == ((char)0x20B9)) 
                return "INR";
            if (ch == ((char)0x20A8)) 
                return "IDR";
            if (ch == ((char)0x20B5)) 
                return "GHS";
            if (ch == ((char)0x09F3)) 
                return "BDT";
            if (ch == ((char)0x20B8)) 
                return "KZT";
            if (ch == ((char)0x20AE)) 
                return "MNT";
            if (ch == ((char)0x0192)) 
                return "HUF";
            if (ch == ((char)0x20AA)) 
                return "ILS";
            return null;
        }
        /// <summary>
        /// Для парсинга действительного числа из строки используйте эту функцию, 
        /// которая работает назависимо от локализьных настроек и на всех языках программирования.
        /// </summary>
        /// <param name="str">строка с действительным числом</param>
        /// <return>double-число или null</return>
        public static double? StringToDouble(string str)
        {
            double res;
            if (str == "NaN") 
                return double.NaN;
            if (double.TryParse(str, out res)) 
                return res;
            if (double.TryParse(str.Replace('.', ','), out res)) 
                return res;
            return null;
        }
        /// <summary>
        /// Независимо от языка и локальных настроек выводит действительное число в строку, 
        /// разделитель - всегда точка. Ситуация типа 1.0000000001 или 23.7299999999999, 
        /// случающиеся на разных языках, округляются куда надо.
        /// </summary>
        /// <param name="d">число</param>
        /// <return>строковый результат</return>
        public static string DoubleToString(double d)
        {
            if (double.IsNaN(d)) 
                return "NaN";
            string res = null;
            if (Math.Truncate(d) == 0.0) 
                res = d.ToString().Replace(",", ".");
            else 
            {
                double rest = Math.Abs(d - Math.Truncate(d));
                if ((rest < 0.000000001) && rest > 0) 
                {
                    res = Math.Truncate(d).ToString();
                    if ((res.IndexOf('E') < 0) && (res.IndexOf('e') < 0)) 
                    {
                        int ii = res.IndexOf('.');
                        if (ii < 0) 
                            ii = res.IndexOf(',');
                        if (ii > 0) 
                            return res.Substring(0, ii);
                        else 
                            return res;
                    }
                }
                else 
                    res = d.ToString().Replace(",", ".");
            }
            if (res.EndsWith(".0")) 
                res = res.Substring(0, res.Length - 2);
            int i = res.IndexOf('e');
            if (i < 0) 
                i = res.IndexOf('E');
            if (i > 0) 
            {
                int exp = 0;
                bool neg = false;
                for (int jj = i + 1; jj < res.Length; jj++) 
                {
                    if (res[jj] == '+') 
                    {
                    }
                    else if (res[jj] == '-') 
                        neg = true;
                    else 
                        exp = (exp * 10) + ((int)((res[jj] - '0')));
                }
                res = res.Substring(0, i);
                if (res.EndsWith(".0")) 
                    res = res.Substring(0, res.Length - 2);
                bool nneg = false;
                if (res[0] == '-') 
                {
                    nneg = true;
                    res = res.Substring(1);
                }
                StringBuilder v1 = new StringBuilder();
                StringBuilder v2 = new StringBuilder();
                i = res.IndexOf('.');
                if (i < 0) 
                    v1.Append(res);
                else 
                {
                    v1.Append(res.Substring(0, i));
                    v2.Append(res.Substring(i + 1));
                }
                for (; exp > 0; exp--) 
                {
                    if (neg) 
                    {
                        if (v1.Length > 0) 
                        {
                            v2.Insert(0, v1[v1.Length - 1]);
                            v1.Length--;
                        }
                        else 
                            v2.Insert(0, '0');
                    }
                    else if (v2.Length > 0) 
                    {
                        v1.Append(v2[0]);
                        v2.Remove(0, 1);
                    }
                    else 
                        v1.Append('0');
                }
                if (v2.Length == 0) 
                    res = v1.ToString();
                else if (v1.Length == 0) 
                    res = "0." + v2;
                else 
                    res = string.Format("{0}.{1}", v1.ToString(), v2.ToString());
                if (nneg) 
                    res = "-" + res;
            }
            i = res.IndexOf('.');
            if (i < 0) 
                return res;
            i++;
            int j;
            for (j = i + 1; j < res.Length; j++) 
            {
                if (res[j] == '9') 
                {
                    int k = 0;
                    int jj;
                    for (jj = j; jj < res.Length; jj++) 
                    {
                        if (res[jj] != '9') 
                            break;
                        else 
                            k++;
                    }
                    if (jj >= res.Length || ((jj == (res.Length - 1) && res[jj] == '8'))) 
                    {
                        if (k > 5) 
                        {
                            for (; j > i; j--) 
                            {
                                if (res[j] != '9') 
                                {
                                    if (res[j] != '.') 
                                        return string.Format("{0}{1}", res.Substring(0, j), ((int)((res[j] - '0'))) + 1);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return res;
        }
        const int PrilNumTagBit = 0x40000000;
        internal static void Initialize()
        {
            if (m_Nums != null) 
                return;
            m_Nums = new TerminCollection();
            m_Nums.AllAddStrsNormalized = true;
            m_Nums.AddString("ОДИН", 1, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ПЕРВЫЙ", 1 | PrilNumTagBit, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ОДИН", 1, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ПЕРШИЙ", 1 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ОДНА", 1, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ОДНО", 1, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("FIRST", 1 | PrilNumTagBit, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("SEMEL", 1, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("ONE", 1, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("ДВА", 2, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ВТОРОЙ", 2 | PrilNumTagBit, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ДВОЕ", 2, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ДВЕ", 2, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ДВУХ", 2, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ОБА", 2, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ОБЕ", 2, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ДВА", 2, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ДРУГИЙ", 2 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ДВОЄ", 2, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ДВІ", 2, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ДВОХ", 2, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ОБОЄ", 2, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ОБИДВА", 2, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("SECOND", 2 | PrilNumTagBit, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("BIS", 2, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("TWO", 2, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("ТРИ", 3, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ТРЕТИЙ", 3 | PrilNumTagBit, null, false);
            m_Nums.AddString("ТРЕХ", 3, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ТРОЕ", 3, Pullenti.Morph.MorphLang.RU, true);
            m_Nums.AddString("ТРИ", 3, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ТРЕТІЙ", 3 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ТРЬОХ", 3, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("ТРОЄ", 3, Pullenti.Morph.MorphLang.UA, true);
            m_Nums.AddString("THIRD", 3 | PrilNumTagBit, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("TER", 3, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("THREE", 3, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("ЧЕТЫРЕ", 4, null, false);
            m_Nums.AddString("ЧЕТВЕРТЫЙ", 4 | PrilNumTagBit, null, false);
            m_Nums.AddString("ЧЕТЫРЕХ", 4, null, false);
            m_Nums.AddString("ЧЕТВЕРО", 4, null, false);
            m_Nums.AddString("ЧОТИРИ", 4, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ЧЕТВЕРТИЙ", 4 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ЧОТИРЬОХ", 4, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("FORTH", 4 | PrilNumTagBit, null, false);
            m_Nums.AddString("QUATER", 4, null, false);
            m_Nums.AddString("FOUR", 4, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("ПЯТЬ", 5, null, false);
            m_Nums.AddString("ПЯТЫЙ", 5 | PrilNumTagBit, null, false);
            m_Nums.AddString("ПЯТИ", 5, null, false);
            m_Nums.AddString("ПЯТЕРО", 5, null, false);
            m_Nums.AddString("ПЯТЬ", 5, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ПЯТИЙ", 5 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("FIFTH", 5 | PrilNumTagBit, null, false);
            m_Nums.AddString("QUINQUIES", 5, null, false);
            m_Nums.AddString("FIVE", 5, Pullenti.Morph.MorphLang.EN, true);
            m_Nums.AddString("ШЕСТЬ", 6, null, false);
            m_Nums.AddString("ШЕСТОЙ", 6 | PrilNumTagBit, null, false);
            m_Nums.AddString("ШЕСТИ", 6, null, false);
            m_Nums.AddString("ШЕСТЕРО", 6, null, false);
            m_Nums.AddString("ШІСТЬ", 6, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ШОСТИЙ", 6 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("SIX", 6, Pullenti.Morph.MorphLang.EN, false);
            m_Nums.AddString("SIXTH", 6 | PrilNumTagBit, null, false);
            m_Nums.AddString("SEXIES ", 6, null, false);
            m_Nums.AddString("СЕМЬ", 7, null, false);
            m_Nums.AddString("СЕДЬМОЙ", 7 | PrilNumTagBit, null, false);
            m_Nums.AddString("СЕМИ", 7, null, false);
            m_Nums.AddString("СЕМЕРО", 7, null, false);
            m_Nums.AddString("СІМ", 7, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("СЬОМИЙ", 7 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("SEVEN", 7, null, false);
            m_Nums.AddString("SEVENTH", 7 | PrilNumTagBit, null, false);
            m_Nums.AddString("SEPTIES", 7, null, false);
            m_Nums.AddString("ВОСЕМЬ", 8, null, false);
            m_Nums.AddString("ВОСЬМОЙ", 8 | PrilNumTagBit, null, false);
            m_Nums.AddString("ВОСЬМИ", 8, null, false);
            m_Nums.AddString("ВОСЬМЕРО", 8, null, false);
            m_Nums.AddString("ВІСІМ", 8, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ВОСЬМИЙ", 8 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("EIGHT", 8, null, false);
            m_Nums.AddString("EIGHTH", 8 | PrilNumTagBit, null, false);
            m_Nums.AddString("OCTIES", 8, null, false);
            m_Nums.AddString("ДЕВЯТЬ", 9, null, false);
            m_Nums.AddString("ДЕВЯТЫЙ", 9 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДЕВЯТИ", 9, null, false);
            m_Nums.AddString("ДЕВЯТЕРО", 9, null, false);
            m_Nums.AddString("ДЕВЯТЬ", 9, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДЕВЯТИЙ", 9 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("NINE", 9, null, false);
            m_Nums.AddString("NINTH", 9 | PrilNumTagBit, null, false);
            m_Nums.AddString("NOVIES", 9, null, false);
            m_Nums.AddString("ДЕСЯТЬ", 10, null, false);
            m_Nums.AddString("ДЕСЯТЫЙ", 10 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДЕСЯТИ", 10, null, false);
            m_Nums.AddString("ДЕСЯТИРО", 10, null, false);
            m_Nums.AddString("ДЕСЯТЬ", 10, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДЕСЯТИЙ", 10 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("TEN", 10, null, false);
            m_Nums.AddString("TENTH", 10 | PrilNumTagBit, null, false);
            m_Nums.AddString("DECIES", 10, null, false);
            m_Nums.AddString("ОДИННАДЦАТЬ", 11, null, false);
            m_Nums.AddString("ОДИНАДЦАТЬ", 11, null, false);
            m_Nums.AddString("ОДИННАДЦАТЫЙ", 11 | PrilNumTagBit, null, false);
            m_Nums.AddString("ОДИННАДЦАТИ", 11, null, false);
            m_Nums.AddString("ОДИННАДЦАТИРО", 11, null, false);
            m_Nums.AddString("ОДИНАДЦЯТЬ", 11, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ОДИНАДЦЯТИЙ", 11 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ОДИНАДЦЯТИ", 11, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ELEVEN", 11, null, false);
            m_Nums.AddString("ELEVENTH", 11 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДВЕНАДЦАТЬ", 12, null, false);
            m_Nums.AddString("ДВЕНАДЦАТЫЙ", 12 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДВЕНАДЦАТИ", 12, null, false);
            m_Nums.AddString("ДВАНАДЦЯТЬ", 12, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДВАНАДЦЯТИЙ", 12 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДВАНАДЦЯТИ", 12, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("TWELVE", 12, null, false);
            m_Nums.AddString("TWELFTH", 12 | PrilNumTagBit, null, false);
            m_Nums.AddString("ТРИНАДЦАТЬ", 13, null, false);
            m_Nums.AddString("ТРИНАДЦАТЫЙ", 13 | PrilNumTagBit, null, false);
            m_Nums.AddString("ТРИНАДЦАТИ", 13, null, false);
            m_Nums.AddString("ТРИНАДЦЯТЬ", 13, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ТРИНАДЦЯТИЙ", 13 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ТРИНАДЦЯТИ", 13, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("THIRTEEN", 13, null, false);
            m_Nums.AddString("THIRTEENTH", 13 | PrilNumTagBit, null, false);
            m_Nums.AddString("ЧЕТЫРНАДЦАТЬ", 14, null, false);
            m_Nums.AddString("ЧЕТЫРНАДЦАТЫЙ", 14 | PrilNumTagBit, null, false);
            m_Nums.AddString("ЧЕТЫРНАДЦАТИ", 14, null, false);
            m_Nums.AddString("ЧОТИРНАДЦЯТЬ", 14, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ЧОТИРНАДЦЯТИЙ", 14 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ЧОТИРНАДЦЯТИ", 14, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("FOURTEEN", 14, null, false);
            m_Nums.AddString("FOURTEENTH", 14 | PrilNumTagBit, null, false);
            m_Nums.AddString("ПЯТНАДЦАТЬ", 15, null, false);
            m_Nums.AddString("ПЯТНАДЦАТЫЙ", 15 | PrilNumTagBit, null, false);
            m_Nums.AddString("ПЯТНАДЦАТИ", 15, null, false);
            m_Nums.AddString("ПЯТНАДЦЯТЬ", 15, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ПЯТНАДЦЯТИЙ", 15 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ПЯТНАДЦЯТИ", 15, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("FIFTEEN", 15, null, false);
            m_Nums.AddString("FIFTEENTH", 15 | PrilNumTagBit, null, false);
            m_Nums.AddString("ШЕСТНАДЦАТЬ", 16, null, false);
            m_Nums.AddString("ШЕСТНАДЦАТЫЙ", 16 | PrilNumTagBit, null, false);
            m_Nums.AddString("ШЕСТНАДЦАТИ", 16, null, false);
            m_Nums.AddString("ШІСТНАДЦЯТЬ", 16, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ШІСТНАДЦЯТИЙ", 16 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ШІСТНАДЦЯТИ", 16, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("SIXTEEN", 16, null, false);
            m_Nums.AddString("SIXTEENTH", 16 | PrilNumTagBit, null, false);
            m_Nums.AddString("СЕМНАДЦАТЬ", 17, null, false);
            m_Nums.AddString("СЕМНАДЦАТЫЙ", 17 | PrilNumTagBit, null, false);
            m_Nums.AddString("СЕМНАДЦАТИ", 17, null, false);
            m_Nums.AddString("СІМНАДЦЯТЬ", 17, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("СІМНАДЦЯТИЙ", 17 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("СІМНАДЦЯТИ", 17, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("SEVENTEEN", 17, null, false);
            m_Nums.AddString("SEVENTEENTH", 17 | PrilNumTagBit, null, false);
            m_Nums.AddString("ВОСЕМНАДЦАТЬ", 18, null, false);
            m_Nums.AddString("ВОСЕМНАДЦАТЫЙ", 18 | PrilNumTagBit, null, false);
            m_Nums.AddString("ВОСЕМНАДЦАТИ", 18, null, false);
            m_Nums.AddString("ВІСІМНАДЦЯТЬ", 18, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ВІСІМНАДЦЯТИЙ", 18 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ВІСІМНАДЦЯТИ", 18, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("EIGHTEEN", 18, null, false);
            m_Nums.AddString("EIGHTEENTH", 18 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДЕВЯТНАДЦАТЬ", 19, null, false);
            m_Nums.AddString("ДЕВЯТНАДЦАТЫЙ", 19 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДЕВЯТНАДЦАТИ", 19, null, false);
            m_Nums.AddString("ДЕВЯТНАДЦЯТЬ", 19, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДЕВЯТНАДЦЯТИЙ", 19 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДЕВЯТНАДЦЯТИ", 19, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("NINETEEN", 19, null, false);
            m_Nums.AddString("NINETEENTH", 19 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДВАДЦАТЬ", 20, null, false);
            m_Nums.AddString("ДВАДЦАТЫЙ", 20 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДВАДЦАТИ", 20, null, false);
            m_Nums.AddString("ДВАДЦЯТЬ", 20, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДВАДЦЯТИЙ", 20 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДВАДЦЯТИ", 20, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("TWENTY", 20, null, false);
            m_Nums.AddString("TWENTIETH", 20 | PrilNumTagBit, null, false);
            m_Nums.AddString("ТРИДЦАТЬ", 30, null, false);
            m_Nums.AddString("ТРИДЦАТЫЙ", 30 | PrilNumTagBit, null, false);
            m_Nums.AddString("ТРИДЦАТИ", 30, null, false);
            m_Nums.AddString("ТРИДЦЯТЬ", 30, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ТРИДЦЯТИЙ", 30 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ТРИДЦЯТИ", 30, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("THIRTY", 30, null, false);
            m_Nums.AddString("THIRTIETH", 30 | PrilNumTagBit, null, false);
            m_Nums.AddString("СОРОК", 40, null, false);
            m_Nums.AddString("СОРОКОВОЙ", 40 | PrilNumTagBit, null, false);
            m_Nums.AddString("СОРОКА", 40, null, false);
            m_Nums.AddString("СОРОК", 40, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("СОРОКОВИЙ", 40 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("FORTY", 40, null, false);
            m_Nums.AddString("FORTIETH", 40 | PrilNumTagBit, null, false);
            m_Nums.AddString("ПЯТЬДЕСЯТ", 50, null, false);
            m_Nums.AddString("ПЯТИДЕСЯТЫЙ", 50 | PrilNumTagBit, null, false);
            m_Nums.AddString("ПЯТИДЕСЯТИ", 50, null, false);
            m_Nums.AddString("ПЯТДЕСЯТ", 50, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ПЯТДЕСЯТИЙ", 50 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ПЯТДЕСЯТИ", 50, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("FIFTY", 50, null, false);
            m_Nums.AddString("FIFTIETH", 50 | PrilNumTagBit, null, false);
            m_Nums.AddString("ШЕСТЬДЕСЯТ", 60, null, false);
            m_Nums.AddString("ШЕСТИДЕСЯТЫЙ", 60 | PrilNumTagBit, null, false);
            m_Nums.AddString("ШЕСТИДЕСЯТИ", 60, null, false);
            m_Nums.AddString("ШІСТДЕСЯТ", 60, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ШЕСИДЕСЯТЫЙ", 60 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ШІСТДЕСЯТИ", 60, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("SIXTY", 60, null, false);
            m_Nums.AddString("SIXTIETH", 60 | PrilNumTagBit, null, false);
            m_Nums.AddString("СЕМЬДЕСЯТ", 70, null, false);
            m_Nums.AddString("СЕМИДЕСЯТЫЙ", 70 | PrilNumTagBit, null, false);
            m_Nums.AddString("СЕМИДЕСЯТИ", 70, null, false);
            m_Nums.AddString("СІМДЕСЯТ", 70, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("СІМДЕСЯТИЙ", 70 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("СІМДЕСЯТИ", 70, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("SEVENTY", 70, null, false);
            m_Nums.AddString("SEVENTIETH", 70 | PrilNumTagBit, null, false);
            m_Nums.AddString("SEVENTIES", 70 | PrilNumTagBit, null, false);
            m_Nums.AddString("ВОСЕМЬДЕСЯТ", 80, null, false);
            m_Nums.AddString("ВОСЬМИДЕСЯТЫЙ", 80 | PrilNumTagBit, null, false);
            m_Nums.AddString("ВОСЬМИДЕСЯТИ", 80, null, false);
            m_Nums.AddString("ВІСІМДЕСЯТ", 80, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ВОСЬМИДЕСЯТИЙ", 80 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ВІСІМДЕСЯТИ", 80, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("EIGHTY", 80, null, false);
            m_Nums.AddString("EIGHTIETH", 80 | PrilNumTagBit, null, false);
            m_Nums.AddString("EIGHTIES", 80 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДЕВЯНОСТО", 90, null, false);
            m_Nums.AddString("ДЕВЯНОСТЫЙ", 90 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДЕВЯНОСТО", 90, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДЕВЯНОСТИЙ", 90 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("NINETY", 90, null, false);
            m_Nums.AddString("NINETIETH", 90 | PrilNumTagBit, null, false);
            m_Nums.AddString("NINETIES", 90 | PrilNumTagBit, null, false);
            m_Nums.AddString("СТО", 100, null, false);
            m_Nums.AddString("СОТЫЙ", 100 | PrilNumTagBit, null, false);
            m_Nums.AddString("СТА", 100, null, false);
            m_Nums.AddString("СТО", 100, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("СОТИЙ", 100 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("HUNDRED", 100, null, false);
            m_Nums.AddString("HUNDREDTH", 100 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДВЕСТИ", 200, null, false);
            m_Nums.AddString("ДВУХСОТЫЙ", 200 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДВУХСОТ", 200, null, false);
            m_Nums.AddString("ДВІСТІ", 200, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДВОХСОТИЙ", 200 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДВОХСОТ", 200, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ТРИСТА", 300, null, false);
            m_Nums.AddString("ТРЕХСОТЫЙ", 300 | PrilNumTagBit, null, false);
            m_Nums.AddString("ТРЕХСОТ", 300, null, false);
            m_Nums.AddString("ТРИСТА", 300, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ТРЬОХСОТИЙ", 300 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ТРЬОХСОТ", 300, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ЧЕТЫРЕСТА", 400, null, false);
            m_Nums.AddString("ЧЕТЫРЕХСОТЫЙ", 400 | PrilNumTagBit, null, false);
            m_Nums.AddString("ЧОТИРИСТА", 400, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ЧОТИРЬОХСОТИЙ", 400 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ПЯТЬСОТ", 500, null, false);
            m_Nums.AddString("ПЯТИСОТЫЙ", 500 | PrilNumTagBit, null, false);
            m_Nums.AddString("ПЯТСОТ", 500, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ПЯТИСОТИЙ", 500 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ШЕСТЬСОТ", 600, null, false);
            m_Nums.AddString("ШЕСТИСОТЫЙ", 600 | PrilNumTagBit, null, false);
            m_Nums.AddString("ШІСТСОТ", 600, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ШЕСТИСОТИЙ", 600 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("СЕМЬСОТ", 700, null, false);
            m_Nums.AddString("СЕМИСОТЫЙ", 700 | PrilNumTagBit, null, false);
            m_Nums.AddString("СІМСОТ", 700, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("СЕМИСОТИЙ", 700 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ВОСЕМЬСОТ", 800, null, false);
            m_Nums.AddString("ВОСЕМЬСОТЫЙ", 800 | PrilNumTagBit, null, false);
            m_Nums.AddString("ВОСЬМИСОТЫЙ", 800 | PrilNumTagBit, null, false);
            m_Nums.AddString("ВІСІМСОТ", 800, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ВОСЬМИСОТЫЙ", 800 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДЕВЯТЬСОТ", 900, null, false);
            m_Nums.AddString("ДЕВЯТЬСОТЫЙ", 900 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДЕВЯТИСОТЫЙ", 900 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДЕВЯТСОТ", 900, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДЕВЯТЬСОТЫЙ", 900 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДЕВЯТИСОТИЙ", 900 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ТЫС", 1000, null, false);
            m_Nums.AddString("ТЫСЯЧА", 1000, null, false);
            m_Nums.AddString("ТЫСЯЧНЫЙ", 1000 | PrilNumTagBit, null, false);
            m_Nums.AddString("ТИС", 1000, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ТИСЯЧА", 1000, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ТИСЯЧНИЙ", 1000 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("ДВУХТЫСЯЧНЫЙ", 2000 | PrilNumTagBit, null, false);
            m_Nums.AddString("ДВОХТИСЯЧНИЙ", 2000 | PrilNumTagBit, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("МИЛЛИОН", 1000000, null, false);
            m_Nums.AddString("МЛН", 1000000, null, false);
            m_Nums.AddString("МІЛЬЙОН", 1000000, Pullenti.Morph.MorphLang.UA, false);
            m_Nums.AddString("МИЛЛИАРД", 1000000000, null, false);
            m_Nums.AddString("МІЛЬЯРД", 1000000000, Pullenti.Morph.MorphLang.UA, false);
            m_AfterPoints = new TerminCollection();
            Termin t = new Termin("ПОЛОВИНА") { Tag = 0.5 };
            t.AddVariant("ОДНА ВТОРАЯ", false);
            t.AddVariant("ПОЛ", false);
            m_AfterPoints.Add(t);
            t = new Termin("ТРЕТЬ") { Tag = 0.33 };
            t.AddVariant("ОДНА ТРЕТЬ", false);
            m_AfterPoints.Add(t);
            t = new Termin("ЧЕТВЕРТЬ") { Tag = 0.25 };
            t.AddVariant("ОДНА ЧЕТВЕРТАЯ", false);
            m_AfterPoints.Add(t);
            t = new Termin("ПЯТАЯ ЧАСТЬ") { Tag = 0.2 };
            t.AddVariant("ОДНА ПЯТАЯ", false);
            m_AfterPoints.Add(t);
        }
        internal static TerminCollection m_Nums;
        static TerminCollection m_AfterPoints;
    }
}