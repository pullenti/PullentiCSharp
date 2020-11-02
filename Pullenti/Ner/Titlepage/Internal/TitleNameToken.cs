/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Titlepage.Internal
{
    /// <summary>
    /// Название статьи
    /// </summary>
    class TitleNameToken : Pullenti.Ner.MetaToken
    {
        private TitleNameToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        /// <summary>
        /// Ранг
        /// </summary>
        public int Rank;
        public Pullenti.Ner.Token BeginNameToken;
        public Pullenti.Ner.Token EndNameToken;
        /// <summary>
        /// Это значение типа работы (если есть)
        /// </summary>
        public string TypeValue;
        /// <summary>
        /// Специальность (если есть)
        /// </summary>
        public string Speciality;
        public override string ToString()
        {
            if (BeginNameToken == null || EndNameToken == null) 
                return "?";
            Pullenti.Ner.MetaToken mt = new Pullenti.Ner.MetaToken(BeginNameToken, EndNameToken);
            if (TypeValue == null) 
                return string.Format("{0}: {1}", Rank, mt.ToString());
            else 
                return string.Format("{0}: {1} ({2})", Rank, mt.ToString(), TypeValue);
        }
        public static void Sort(List<TitleNameToken> li)
        {
            for (int k = 0; k < li.Count; k++) 
            {
                bool ch = false;
                for (int i = 0; i < (li.Count - 1); i++) 
                {
                    if (li[i].Rank < li[i + 1].Rank) 
                    {
                        ch = true;
                        TitleNameToken v = li[i];
                        li[i] = li[i + 1];
                        li[i + 1] = v;
                    }
                }
                if (!ch) 
                    break;
            }
        }
        public static bool CanBeStartOfTextOrContent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            Pullenti.Ner.Token t;
            if (begin.IsValue("СОДЕРЖАНИЕ", "ЗМІСТ") || begin.IsValue("ОГЛАВЛЕНИЕ", null) || begin.IsValue("СОДЕРЖИМОЕ", null)) 
            {
                t = begin;
                if (t.Next != null && t.Next.IsCharOf(":.")) 
                    t = t.Next;
                if (t == end) 
                    return true;
            }
            if (begin.IsValue("ОТ", "ВІД") && begin.Next != null && begin.Next.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
            {
                if (begin.Next.Next != null && begin.Next.Next.IsChar(':')) 
                    return true;
            }
            int words = 0;
            int verbs = 0;
            for (t = begin; t != end.Next; t = t.Next) 
            {
                if (t is Pullenti.Ner.TextToken) 
                {
                    if (t.Chars.IsLetter) 
                        words++;
                    if (t.Chars.IsAllLower && (t as Pullenti.Ner.TextToken).IsPureVerb) 
                        verbs++;
                }
            }
            if (words > 10 && verbs > 1) 
                return true;
            return false;
        }
        public static TitleNameToken TryParse(Pullenti.Ner.Token begin, Pullenti.Ner.Token end, int minNewlinesCount)
        {
            TitleNameToken res = new TitleNameToken(begin, end);
            if (!res.CalcRankAndValue(minNewlinesCount)) 
                return null;
            if (res.BeginNameToken == null || res.EndNameToken == null) 
                return null;
            return res;
        }
        bool CalcRankAndValue(int minNewlinesCount)
        {
            Rank = 0;
            if (BeginToken.Chars.IsAllLower) 
                Rank -= 30;
            int words = 0;
            int upWords = 0;
            int notwords = 0;
            int lineNumber = 0;
            Pullenti.Ner.Token tstart = BeginToken;
            Pullenti.Ner.Token tend = EndToken;
            for (Pullenti.Ner.Token t = BeginToken; t != EndToken.Next && t != null && t.EndChar <= EndToken.EndChar; t = t.Next) 
            {
                if (t.IsNewlineBefore) 
                {
                }
                TitleItemToken tit = TitleItemToken.TryAttach(t);
                if (tit != null) 
                {
                    if (tit.Typ == TitleItemToken.Types.Theme || tit.Typ == TitleItemToken.Types.TypAndTheme) 
                    {
                        if (t != BeginToken) 
                        {
                            if (lineNumber > 0) 
                                return false;
                            words = (upWords = (notwords = 0));
                            tstart = tit.EndToken.Next;
                        }
                        t = tit.EndToken;
                        if (t.Next == null) 
                            return false;
                        if (t.Next.Chars.IsLetter && t.Next.Chars.IsAllLower) 
                            Rank += 20;
                        else 
                            Rank += 100;
                        tstart = t.Next;
                        if (tit.Typ == TitleItemToken.Types.TypAndTheme) 
                            TypeValue = tit.Value;
                        continue;
                    }
                    if (tit.Typ == TitleItemToken.Types.Typ) 
                    {
                        if (t == BeginToken) 
                        {
                            if (tit.EndToken.IsNewlineAfter) 
                            {
                                TypeValue = tit.Value;
                                Rank += 5;
                                tstart = tit.EndToken.Next;
                            }
                        }
                        t = tit.EndToken;
                        words++;
                        if (tit.BeginToken != tit.EndToken) 
                            words++;
                        if (tit.Chars.IsAllUpper) 
                            upWords++;
                        continue;
                    }
                    if (tit.Typ == TitleItemToken.Types.Dust || tit.Typ == TitleItemToken.Types.Speciality) 
                    {
                        if (t == BeginToken) 
                            return false;
                        Rank -= 20;
                        if (tit.Typ == TitleItemToken.Types.Speciality) 
                            Speciality = tit.Value;
                        t = tit.EndToken;
                        continue;
                    }
                    if (tit.Typ == TitleItemToken.Types.Consultant || tit.Typ == TitleItemToken.Types.Boss || tit.Typ == TitleItemToken.Types.Editor) 
                    {
                        t = tit.EndToken;
                        if (t.Next != null && ((t.Next.IsCharOf(":") || t.Next.IsHiphen || t.WhitespacesAfterCount > 4))) 
                            Rank -= 10;
                        else 
                            Rank -= 2;
                        continue;
                    }
                    return false;
                }
                Pullenti.Ner.Booklink.Internal.BookLinkToken blt = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(t, 0);
                if (blt != null) 
                {
                    if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Misc || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.N || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Pages) 
                        Rank -= 10;
                    else if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.N || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.PageRange) 
                        Rank -= 20;
                }
                if (t == BeginToken && Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParseAuthor(t, Pullenti.Ner.Person.Internal.FioTemplateType.Undefined) != null) 
                    Rank -= 20;
                if (t.IsNewlineBefore && t != BeginToken) 
                {
                    lineNumber++;
                    if (lineNumber > 4) 
                        return false;
                    if (t.Chars.IsAllLower) 
                        Rank += 10;
                    else if (t.Previous.IsChar('.')) 
                        Rank -= 10;
                    else if (t.Previous.IsCharOf(",-")) 
                        Rank += 10;
                    else 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null && npt.EndChar >= t.EndChar) 
                            Rank += 10;
                    }
                }
                if (t != BeginToken && t.NewlinesBeforeCount > minNewlinesCount) 
                    Rank -= (t.NewlinesBeforeCount - minNewlinesCount);
                Pullenti.Ner.Core.BracketSequenceToken bst = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (bst != null && bst.IsQuoteType && bst.EndToken.EndChar <= EndToken.EndChar) 
                {
                    if (words == 0) 
                    {
                        tstart = bst.BeginToken;
                        Rank += 10;
                        if (bst.EndToken == EndToken) 
                        {
                            tend = EndToken;
                            Rank += 10;
                        }
                    }
                }
                List<Pullenti.Ner.Referent> rli = t.GetReferents();
                if (rli != null) 
                {
                    foreach (Pullenti.Ner.Referent r in rli) 
                    {
                        if (r is Pullenti.Ner.Org.OrganizationReferent) 
                        {
                            if (t.IsNewlineBefore) 
                                Rank -= 10;
                            else 
                                Rank -= 4;
                            continue;
                        }
                        if ((r is Pullenti.Ner.Geo.GeoReferent) || (r is Pullenti.Ner.Person.PersonReferent)) 
                        {
                            if (t.IsNewlineBefore) 
                            {
                                Rank -= 5;
                                if (t.IsNewlineAfter || t.Next == null) 
                                    Rank -= 20;
                                else if (t.Next.IsHiphen || (t.Next is Pullenti.Ner.NumberToken) || (t.Next.GetReferent() is Pullenti.Ner.Date.DateReferent)) 
                                    Rank -= 20;
                                else if (t != BeginToken) 
                                    Rank -= 20;
                            }
                            continue;
                        }
                        if ((r is Pullenti.Ner.Geo.GeoReferent) || (r is Pullenti.Ner.Denomination.DenominationReferent)) 
                            continue;
                        if ((r is Pullenti.Ner.Uri.UriReferent) || (r is Pullenti.Ner.Phone.PhoneReferent)) 
                            return false;
                        if (t.IsNewlineBefore) 
                            Rank -= 4;
                        else 
                            Rank -= 2;
                        if (t == BeginToken && (EndToken.GetReferent() is Pullenti.Ner.Person.PersonReferent)) 
                            Rank -= 10;
                    }
                    words++;
                    if (t.Chars.IsAllUpper) 
                        upWords++;
                    if (t == BeginToken) 
                    {
                        if (t.IsNewlineAfter) 
                            Rank -= 10;
                        else if (t.Next != null && t.Next.IsChar('.') && t.Next.IsNewlineAfter) 
                            Rank -= 10;
                    }
                    continue;
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    if ((t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Words) 
                    {
                        words++;
                        if (t.Chars.IsAllUpper) 
                            upWords++;
                    }
                    else 
                        notwords++;
                    continue;
                }
                Pullenti.Ner.Person.Internal.PersonAttrToken pat = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(t, null, Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                if (pat != null) 
                {
                    if (t.IsNewlineBefore) 
                    {
                        if (!pat.Morph.Case.IsUndefined && !pat.Morph.Case.IsNominative) 
                        {
                        }
                        else if (pat.Chars.IsAllUpper) 
                        {
                        }
                        else 
                            Rank -= 20;
                    }
                    else if (t.Chars.IsAllLower) 
                        Rank--;
                    for (; t != null; t = t.Next) 
                    {
                        words++;
                        if (t.Chars.IsAllUpper) 
                            upWords++;
                        if (t == pat.EndToken) 
                            break;
                    }
                    continue;
                }
                Pullenti.Ner.Org.Internal.OrgItemTypeToken oitt = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t, true, null);
                if (oitt != null) 
                {
                    if (oitt.Morph.Number != Pullenti.Morph.MorphNumber.Plural && !oitt.IsDoubtRootWord) 
                    {
                        if (!oitt.Morph.Case.IsUndefined && !oitt.Morph.Case.IsNominative) 
                        {
                            words++;
                            if (t.Chars.IsAllUpper) 
                                upWords++;
                        }
                        else 
                        {
                            Rank -= 4;
                            if (t == BeginToken) 
                                Rank -= 5;
                        }
                    }
                    else 
                    {
                        words += 1;
                        if (t.Chars.IsAllUpper) 
                            upWords++;
                    }
                    t = oitt.EndToken;
                    continue;
                }
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt != null) 
                {
                    if (tt.IsChar('©')) 
                        Rank -= 10;
                    if (tt.IsChar('_')) 
                        Rank--;
                    if (tt.Chars.IsLetter) 
                    {
                        if (tt.LengthChar > 2) 
                        {
                            words++;
                            if (t.Chars.IsAllUpper) 
                                upWords++;
                        }
                    }
                    else if (!tt.IsChar(',')) 
                        notwords++;
                    if (tt.IsPureVerb) 
                    {
                            {
                                Rank -= 30;
                                words--;
                            }
                        break;
                    }
                    if (tt == EndToken) 
                    {
                        if (tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction) 
                            Rank -= 10;
                        else if (tt.IsChar('.')) 
                            Rank += 5;
                    }
                    else if (tt.IsCharOf("._")) 
                        Rank -= 5;
                }
            }
            Rank += words;
            Rank -= notwords;
            if ((words < 1) && (Rank < 50)) 
                return false;
            if (tstart == null || tend == null) 
                return false;
            if (tstart.EndChar > tend.EndChar) 
                return false;
            TitleItemToken tit1 = TitleItemToken.TryAttach(EndToken.Next);
            if (tit1 != null && ((tit1.Typ == TitleItemToken.Types.Typ || tit1.Typ == TitleItemToken.Types.Speciality))) 
            {
                if (tit1.EndToken.IsNewlineAfter) 
                    Rank += 15;
                else 
                    Rank += 10;
                if (tit1.Typ == TitleItemToken.Types.Speciality) 
                    Speciality = tit1.Value;
            }
            if (upWords > 4 && upWords > ((int)((0.8 * words)))) 
            {
                if (tstart.Previous != null && (tstart.Previous.GetReferent() is Pullenti.Ner.Person.PersonReferent)) 
                    Rank += (5 + upWords);
            }
            BeginNameToken = tstart;
            EndNameToken = tend;
            return true;
        }
    }
}