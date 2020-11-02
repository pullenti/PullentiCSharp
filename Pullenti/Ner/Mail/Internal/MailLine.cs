/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Mail.Internal
{
    public class MailLine : Pullenti.Ner.MetaToken
    {
        private MailLine(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public int CharsCount
        {
            get
            {
                int cou = 0;
                for (Pullenti.Ner.Token t = BeginToken; t != null; t = t.Next) 
                {
                    cou += t.LengthChar;
                    if (t == EndToken) 
                        break;
                }
                return cou;
            }
        }
        public int Words
        {
            get
            {
                int cou = 0;
                for (Pullenti.Ner.Token t = BeginToken; t != null && t.EndChar <= EndChar; t = t.Next) 
                {
                    if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && t.LengthChar > 2) 
                    {
                        if (t.Tag == null) 
                            cou++;
                    }
                }
                return cou;
            }
        }
        public bool IsPureEn
        {
            get
            {
                int en = 0;
                int ru = 0;
                for (Pullenti.Ner.Token t = BeginToken; t != null && t.EndChar <= EndChar; t = t.Next) 
                {
                    if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) 
                    {
                        if (t.Chars.IsCyrillicLetter) 
                            ru++;
                        else if (t.Chars.IsLatinLetter) 
                            en++;
                    }
                }
                if (en > 0 && ru == 0) 
                    return true;
                return false;
            }
        }
        public bool IsPureRu
        {
            get
            {
                int en = 0;
                int ru = 0;
                for (Pullenti.Ner.Token t = BeginToken; t != null && t.EndChar <= EndChar; t = t.Next) 
                {
                    if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) 
                    {
                        if (t.Chars.IsCyrillicLetter) 
                            ru++;
                        else if (t.Chars.IsLatinLetter) 
                            en++;
                    }
                }
                if (ru > 0 && en == 0) 
                    return true;
                return false;
            }
        }
        /// <summary>
        /// Это кол-во элементов, которые характеризуют вложенность письма
        /// </summary>
        public int Lev = 0;
        public enum Types : int
        {
            Undefined = 0,
            Hello = 1,
            BestRegards = 2,
            From = 3,
        }

        public Types Typ = Types.Undefined;
        public List<Pullenti.Ner.Referent> Refs = new List<Pullenti.Ner.Referent>();
        public bool MustBeFirstLine = false;
        public Pullenti.Ner.Referent MailAddr
        {
            get
            {
                for (Pullenti.Ner.Token t = BeginToken; t != null && t.EndChar <= EndChar; t = t.Next) 
                {
                    if (t.GetReferent() != null && t.GetReferent().TypeName == "URI") 
                    {
                        if (t.GetReferent().GetStringValue("SCHEME") == "mailto") 
                            return t.GetReferent();
                    }
                }
                return null;
            }
        }
        public bool IsRealFrom
        {
            get
            {
                Pullenti.Ner.TextToken tt = BeginToken as Pullenti.Ner.TextToken;
                if (tt == null) 
                    return false;
                return tt.Term == "FROM" || tt.Term == "ОТ";
            }
        }
        public override string ToString()
        {
            return string.Format("{0}{1} {2}: {3}", (MustBeFirstLine ? "(1) " : ""), Lev, Typ, this.GetSourceText());
        }
        public static MailLine Parse(Pullenti.Ner.Token t0, int lev, int maxCount = 0)
        {
            if (t0 == null) 
                return null;
            MailLine res = new MailLine(t0, t0);
            bool pr = true;
            int cou = 0;
            for (Pullenti.Ner.Token t = t0; t != null; t = t.Next,cou++) 
            {
                if (t.IsNewlineBefore && t0 != t) 
                    break;
                if (maxCount > 0 && cou > maxCount) 
                    break;
                res.EndToken = t;
                if (t.IsTableControlChar || t.IsHiphen) 
                    continue;
                if (pr) 
                {
                    if ((t is Pullenti.Ner.TextToken) && t.IsCharOf(">|")) 
                        res.Lev++;
                    else 
                    {
                        pr = false;
                        Pullenti.Ner.Core.TerminToken tok = m_FromWords.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok != null && tok.EndToken.Next != null && tok.EndToken.Next.IsChar(':')) 
                        {
                            res.Typ = Types.From;
                            t = tok.EndToken.Next;
                            continue;
                        }
                    }
                }
                if (t is Pullenti.Ner.ReferentToken) 
                {
                    Pullenti.Ner.Referent r = t.GetReferent();
                    if (r != null) 
                    {
                        if ((((r is Pullenti.Ner.Person.PersonReferent) || (r is Pullenti.Ner.Geo.GeoReferent) || (r is Pullenti.Ner.Address.AddressReferent)) || r.TypeName == "PHONE" || r.TypeName == "URI") || (r is Pullenti.Ner.Person.PersonPropertyReferent) || r.TypeName == "ORGANIZATION") 
                            res.Refs.Add(r);
                    }
                }
            }
            if (res.Typ == Types.Undefined) 
            {
                Pullenti.Ner.Token t = t0;
                for (; t != null && (t.EndChar < res.EndChar); t = t.Next) 
                {
                    if (!t.IsHiphen && t.Chars.IsLetter) 
                        break;
                }
                int ok = 0;
                int nams = 0;
                int oth = 0;
                Pullenti.Ner.Token lastComma = null;
                for (; t != null && (t.EndChar < res.EndChar); t = t.Next) 
                {
                    if (t.GetReferent() is Pullenti.Ner.Person.PersonReferent) 
                    {
                        nams++;
                        continue;
                    }
                    if (t is Pullenti.Ner.TextToken) 
                    {
                        if (!t.Chars.IsLetter) 
                        {
                            lastComma = t;
                            continue;
                        }
                        Pullenti.Ner.Core.TerminToken tok = m_HelloWords.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok != null) 
                        {
                            ok++;
                            t = tok.EndToken;
                            continue;
                        }
                        if (t.IsValue("ВСЕ", null) || t.IsValue("ALL", null) || t.IsValue("TEAM", null)) 
                        {
                            nams++;
                            continue;
                        }
                        Pullenti.Ner.Person.Internal.PersonItemToken pit = Pullenti.Ner.Person.Internal.PersonItemToken.TryAttach(t, null, Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr.No, null);
                        if (pit != null) 
                        {
                            nams++;
                            t = pit.EndToken;
                            continue;
                        }
                    }
                    if ((++oth) > 3) 
                    {
                        if (ok > 0 && lastComma != null) 
                        {
                            res.EndToken = lastComma;
                            oth = 0;
                        }
                        break;
                    }
                }
                if ((oth < 3) && ok > 0) 
                    res.Typ = Types.Hello;
            }
            if (res.Typ == Types.Undefined) 
            {
                int okWords = 0;
                if (t0.IsValue("HAVE", null)) 
                {
                }
                for (Pullenti.Ner.Token t = t0; t != null && t.EndChar <= res.EndChar; t = t.Next) 
                {
                    if (!(t is Pullenti.Ner.TextToken)) 
                        continue;
                    if (t.IsChar('<')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            t = br.EndToken;
                            continue;
                        }
                    }
                    if (!t.IsLetters || t.IsTableControlChar) 
                        continue;
                    Pullenti.Ner.Core.TerminToken tok = m_RegardWords.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null) 
                    {
                        okWords++;
                        for (; t != null && t.EndChar <= tok.EndChar; t = t.Next) 
                        {
                            t.Tag = tok.Termin;
                        }
                        t = tok.EndToken;
                        if ((t.Next is Pullenti.Ner.TextToken) && t.Next.Morph.Case.IsGenitive) 
                        {
                            for (t = t.Next; t.EndChar <= res.EndChar; t = t.Next) 
                            {
                                if (t.Morph.Class.IsConjunction) 
                                    continue;
                                Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt1 == null) 
                                    break;
                                if (!npt1.Morph.Case.IsGenitive) 
                                    break;
                                for (; t.EndChar < npt1.EndChar; t = t.Next) 
                                {
                                    t.Tag = t;
                                }
                                t.Tag = t;
                            }
                        }
                        continue;
                    }
                    if ((t.Morph.Class.IsPreposition || t.Morph.Class.IsConjunction || t.Morph.Class.IsMisc) || t.IsValue("C", null)) 
                        continue;
                    if ((okWords > 0 && t.Previous != null && t.Previous.IsComma) && t.Previous.BeginChar > t0.BeginChar && !t.Chars.IsAllLower) 
                    {
                        res.EndToken = t.Previous;
                        break;
                    }
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt == null) 
                    {
                        if ((res.EndChar - t.EndChar) > 10) 
                            okWords = 0;
                        break;
                    }
                    tok = m_RegardWords.TryParse(npt.EndToken, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null && (npt.EndToken is Pullenti.Ner.TextToken)) 
                    {
                        string term = (npt.EndToken as Pullenti.Ner.TextToken).Term;
                        if (term == "ДЕЛ") 
                            tok = null;
                    }
                    if (tok == null) 
                    {
                        if (npt.Noun.IsValue("НАДЕЖДА", null)) 
                            t.Tag = t;
                        else if (okWords > 0 && t.IsValue("NICE", null) && ((res.EndChar - npt.EndChar) < 13)) 
                            t.Tag = t;
                        else 
                            okWords = 0;
                        break;
                    }
                    okWords++;
                    for (; t != null && t.EndChar <= tok.EndChar; t = t.Next) 
                    {
                        t.Tag = tok.Termin;
                    }
                    t = tok.EndToken;
                }
                if (okWords > 0) 
                    res.Typ = Types.BestRegards;
            }
            if (res.Typ == Types.Undefined) 
            {
                Pullenti.Ner.Token t = t0;
                for (; t != null && (t.EndChar < res.EndChar); t = t.Next) 
                {
                    if (!(t is Pullenti.Ner.TextToken)) 
                        break;
                    else if (!t.IsHiphen && t.Chars.IsLetter) 
                        break;
                }
                if (t != null) 
                {
                    if (t != t0) 
                    {
                    }
                    if (((t.IsValue("ПЕРЕСЫЛАЕМОЕ", null) || t.IsValue("ПЕРЕАДРЕСОВАННОЕ", null))) && t.Next != null && t.Next.IsValue("СООБЩЕНИЕ", null)) 
                    {
                        res.Typ = Types.From;
                        res.MustBeFirstLine = true;
                    }
                    else if ((t.IsValue("НАЧАЛО", null) && t.Next != null && ((t.Next.IsValue("ПЕРЕСЫЛАЕМОЕ", null) || t.Next.IsValue("ПЕРЕАДРЕСОВАННОЕ", null)))) && t.Next.Next != null && t.Next.Next.IsValue("СООБЩЕНИЕ", null)) 
                    {
                        res.Typ = Types.From;
                        res.MustBeFirstLine = true;
                    }
                    else if (t.IsValue("ORIGINAL", null) && t.Next != null && ((t.Next.IsValue("MESSAGE", null) || t.Next.IsValue("APPOINTMENT", null)))) 
                    {
                        res.Typ = Types.From;
                        res.MustBeFirstLine = true;
                    }
                    else if (t.IsValue("ПЕРЕСЛАНО", null) && t.Next != null && t.Next.IsValue("ПОЛЬЗОВАТЕЛЕМ", null)) 
                    {
                        res.Typ = Types.From;
                        res.MustBeFirstLine = true;
                    }
                    else if (((t.GetReferent() != null && t.GetReferent().TypeName == "DATE")) || ((t.IsValue("IL", null) && t.Next != null && t.Next.IsValue("GIORNO", null))) || ((t.IsValue("ON", null) && (t.Next is Pullenti.Ner.ReferentToken) && t.Next.GetReferent().TypeName == "DATE"))) 
                    {
                        bool hasFrom = false;
                        bool hasDate = t.GetReferent() != null && t.GetReferent().TypeName == "DATE";
                        if (t.IsNewlineAfter && (lev < 5)) 
                        {
                            MailLine res1 = Parse(t.Next, lev + 1, 0);
                            if (res1 != null && res1.Typ == Types.Hello) 
                                res.Typ = Types.From;
                        }
                        MailLine next = Parse(res.EndToken.Next, lev + 1, 0);
                        if (next != null) 
                        {
                            if (next.Typ != Types.Undefined) 
                                next = null;
                        }
                        int tmax = res.EndChar;
                        if (next != null) 
                            tmax = next.EndChar;
                        Pullenti.Ner.Core.BracketSequenceToken br1 = null;
                        for (; t != null && t.EndChar <= tmax; t = t.Next) 
                        {
                            if (t.IsValue("ОТ", null) || t.IsValue("FROM", null)) 
                                hasFrom = true;
                            else if (t.GetReferent() != null && ((t.GetReferent().TypeName == "URI" || (t.GetReferent() is Pullenti.Ner.Person.PersonReferent)))) 
                            {
                                if (t.GetReferent().TypeName == "URI" && hasDate) 
                                {
                                    if (br1 != null) 
                                    {
                                        hasFrom = true;
                                        next = null;
                                    }
                                    if (t.Previous.IsChar('<') && t.Next != null && t.Next.IsChar('>')) 
                                    {
                                        t = t.Next;
                                        if (t.Next != null && t.Next.IsChar(':')) 
                                            t = t.Next;
                                        if (t.IsNewlineAfter) 
                                        {
                                            hasFrom = true;
                                            next = null;
                                        }
                                    }
                                }
                                for (t = t.Next; t != null && t.EndChar <= res.EndChar; t = t.Next) 
                                {
                                    if (t.IsValue("HA", null) && t.Next != null && t.Next.IsValue("SCRITTO", null)) 
                                    {
                                        hasFrom = true;
                                        break;
                                    }
                                    else if (((t.IsValue("НАПИСАТЬ", null) || t.IsValue("WROTE", null))) && ((res.EndChar - t.EndChar) < 10)) 
                                    {
                                        hasFrom = true;
                                        break;
                                    }
                                }
                                if (hasFrom) 
                                {
                                    res.Typ = Types.From;
                                    if (next != null && t.EndChar >= next.BeginChar) 
                                        res.EndToken = next.EndToken;
                                }
                                break;
                            }
                            else if (br1 == null && !t.IsChar('<') && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                            {
                                br1 = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br1 != null) 
                                    t = br1.EndToken;
                            }
                        }
                    }
                    else 
                    {
                        bool hasUri = false;
                        for (; t != null && (t.EndChar < res.EndChar); t = t.Next) 
                        {
                            if (t.GetReferent() != null && ((t.GetReferent().TypeName == "URI" || (t.GetReferent() is Pullenti.Ner.Person.PersonReferent)))) 
                                hasUri = true;
                            else if (t.IsValue("ПИСАТЬ", null) && hasUri) 
                            {
                                if (t.Next != null && t.Next.IsChar('(')) 
                                {
                                    if (hasUri) 
                                        res.Typ = Types.From;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return res;
        }
        static Pullenti.Ner.Core.TerminCollection m_RegardWords;
        static Pullenti.Ner.Core.TerminCollection m_FromWords;
        static Pullenti.Ner.Core.TerminCollection m_HelloWords;
        public static bool IsKeyword(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            if (m_RegardWords.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                return true;
            if (m_FromWords.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                return true;
            if (m_HelloWords.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                return true;
            return false;
        }
        public static void Initialize()
        {
            if (m_RegardWords != null) 
                return;
            m_RegardWords = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"УВАЖЕНИЕ", "ПОЧТЕНИЕ", "С УВАЖЕНИЕМ", "ПОЖЕЛАНИE", "ДЕНЬ", "ХОРОШЕГО ДНЯ", "ИСКРЕННЕ ВАШ", "УДАЧА", "СПАСИБО", "ЦЕЛОВАТЬ", "ПОВАГА", "З ПОВАГОЮ", "ПОБАЖАННЯ", "ДЕНЬ", "ЩИРО ВАШ", "ДЯКУЮ", "ЦІЛУВАТИ", "BEST REGARDS", "REGARDS", "BEST WISHES", "KIND REGARDS", "GOOD BYE", "BYE", "THANKS", "THANK YOU", "MANY THANKS", "DAY", "VERY MUCH", "HAVE", "LUCK", "Yours sincerely", "sincerely Yours", "Looking forward", "Ar cieņu"}) 
            {
                m_RegardWords.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()));
            }
            m_FromWords = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"FROM", "TO", "CC", "SENT", "SUBJECT", "SENDER", "TIME", "ОТ КОГО", "КОМУ", "ДАТА", "ТЕМА", "КОПИЯ", "ОТ", "ОТПРАВЛЕНО", "WHEN", "WHERE"}) 
            {
                m_FromWords.Add(new Pullenti.Ner.Core.Termin(s));
            }
            m_HelloWords = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"HI", "HELLO", "DEAR", "GOOD MORNING", "GOOD DAY", "GOOD EVENING", "GOOD NIGHT", "ЗДРАВСТВУЙ", "ЗДРАВСТВУЙТЕ", "ПРИВЕТСТВУЮ", "ПРИВЕТ", "ПРИВЕТИК", "УВАЖАЕМЫЙ", "ДОРОГОЙ", "ЛЮБЕЗНЫЙ", "ДОБРОЕ УТРО", "ДОБРЫЙ ДЕНЬ", "ДОБРЫЙ ВЕЧЕР", "ДОБРОЙ НОЧИ", "ЗДРАСТУЙ", "ЗДРАСТУЙТЕ", "ВІТАЮ", "ПРИВІТ", "ПРИВІТ", "ШАНОВНИЙ", "ДОРОГИЙ", "ЛЮБИЙ", "ДОБРОГО РАНКУ", "ДОБРИЙ ДЕНЬ", "ДОБРИЙ ВЕЧІР", "ДОБРОЇ НОЧІ"}) 
            {
                m_HelloWords.Add(new Pullenti.Ner.Core.Termin(s));
            }
        }
    }
}