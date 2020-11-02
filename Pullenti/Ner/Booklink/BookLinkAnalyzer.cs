/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Booklink
{
    /// <summary>
    /// Анализатор ссылок на внешнюю литературу (библиография)
    /// </summary>
    public class BookLinkAnalyzer : Pullenti.Ner.Analyzer
    {
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        /// <summary>
        /// Имя анализатора ("BOOKLINK")
        /// </summary>
        public const string ANALYZER_NAME = "BOOKLINK";
        public override string Caption
        {
            get
            {
                return "Ссылки на литературу";
            }
        }
        public override string Description
        {
            get
            {
                return "Ссылки из списка литературы";
            }
        }
        public override bool IsSpecific
        {
            get
            {
                return false;
            }
        }
        public override int ProgressWeight
        {
            get
            {
                return 1;
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new BookLinkAnalyzer();
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {Pullenti.Ner.Date.DateReferent.OBJ_TYPENAME, Pullenti.Ner.Geo.GeoReferent.OBJ_TYPENAME, Pullenti.Ner.Org.OrganizationReferent.OBJ_TYPENAME, Pullenti.Ner.Person.PersonReferent.OBJ_TYPENAME};
            }
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Booklink.Internal.MetaBookLink.GlobalMeta, Pullenti.Ner.Booklink.Internal.MetaBookLinkRef.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Booklink.Internal.MetaBookLink.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("booklink.png"));
                res.Add(Pullenti.Ner.Booklink.Internal.MetaBookLinkRef.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("booklinkref.png"));
                res.Add(Pullenti.Ner.Booklink.Internal.MetaBookLinkRef.ImageIdInline, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("booklinkrefinline.png"));
                res.Add(Pullenti.Ner.Booklink.Internal.MetaBookLinkRef.ImageIdLast, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("booklinkreflast.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == BookLinkReferent.OBJ_TYPENAME) 
                return new BookLinkReferent();
            if (type == BookLinkRefReferent.OBJ_TYPENAME) 
                return new BookLinkRefReferent();
            return null;
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            int isLitBlock = 0;
            Dictionary<string, List<BookLinkRefReferent>> refsByNum = new Dictionary<string, List<BookLinkRefReferent>>();
            List<Pullenti.Ner.ReferentToken> rts;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (t.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null && br.LengthChar > 70 && (br.LengthChar < 400)) 
                    {
                        if (br.IsNewlineAfter || ((br.EndToken.Next != null && br.EndToken.Next.IsCharOf(".;")))) 
                        {
                            rts = TryParse(t.Next, false, br.EndChar);
                            if (rts != null && rts.Count >= 1) 
                            {
                                if (rts.Count > 1) 
                                {
                                    rts[1].Referent = ad.RegisterReferent(rts[1].Referent);
                                    kit.EmbedToken(rts[1]);
                                    (rts[0].Referent as BookLinkRefReferent).Book = rts[1].Referent as BookLinkReferent;
                                    if (rts[0].BeginChar == rts[1].BeginChar) 
                                        rts[0].BeginToken = rts[1];
                                    if (rts[0].EndChar == rts[1].EndChar) 
                                        rts[0].EndToken = rts[1];
                                }
                                rts[0].BeginToken = t;
                                rts[0].EndToken = br.EndToken;
                                (rts[0].Referent as BookLinkRefReferent).Typ = BookLinkRefType.Inline;
                                rts[0].Referent = ad.RegisterReferent(rts[0].Referent);
                                kit.EmbedToken(rts[0]);
                                t = rts[0];
                                continue;
                            }
                        }
                    }
                }
                if (!t.IsNewlineBefore) 
                    continue;
                if (isLitBlock <= 0) 
                {
                    Pullenti.Ner.Token tt = Pullenti.Ner.Booklink.Internal.BookLinkToken.ParseStartOfLitBlock(t);
                    if (tt != null) 
                    {
                        isLitBlock = 5;
                        t = tt;
                        continue;
                    }
                }
                rts = TryParse(t, isLitBlock > 0, 0);
                if (rts == null || (rts.Count < 1)) 
                {
                    if ((--isLitBlock) < 0) 
                        isLitBlock = 0;
                    continue;
                }
                if ((++isLitBlock) > 5) 
                    isLitBlock = 5;
                if (rts.Count > 1) 
                {
                    rts[1].Referent = ad.RegisterReferent(rts[1].Referent);
                    kit.EmbedToken(rts[1]);
                    (rts[0].Referent as BookLinkRefReferent).Book = rts[1].Referent as BookLinkReferent;
                    if (rts[0].BeginChar == rts[1].BeginChar) 
                        rts[0].BeginToken = rts[1];
                    if (rts[0].EndChar == rts[1].EndChar) 
                        rts[0].EndToken = rts[1];
                }
                BookLinkRefReferent re = rts[0].Referent as BookLinkRefReferent;
                re = ad.RegisterReferent(re) as BookLinkRefReferent;
                rts[0].Referent = re;
                kit.EmbedToken(rts[0]);
                t = rts[0];
                if (re.Number != null) 
                {
                    List<BookLinkRefReferent> li;
                    if (!refsByNum.TryGetValue(re.Number, out li)) 
                        refsByNum.Add(re.Number, (li = new List<BookLinkRefReferent>()));
                    li.Add(re);
                }
            }
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (!(t is Pullenti.Ner.TextToken)) 
                    continue;
                Pullenti.Ner.ReferentToken rt = TryParseShortInline(t);
                if (rt == null) 
                    continue;
                BookLinkRefReferent re = rt.Referent as BookLinkRefReferent;
                List<BookLinkRefReferent> li;
                if (!refsByNum.TryGetValue(re.Number ?? "", out li)) 
                    continue;
                int i;
                for (i = 0; i < li.Count; i++) 
                {
                    if (t.BeginChar < li[i].Occurrence[0].BeginChar) 
                        break;
                }
                if (i >= li.Count) 
                    continue;
                re.Book = li[i].Book;
                if (re.Pages == null) 
                    re.Pages = li[i].Pages;
                re.Typ = BookLinkRefType.Inline;
                re = ad.RegisterReferent(re) as BookLinkRefReferent;
                rt.Referent = re;
                kit.EmbedToken(rt);
                t = rt;
            }
        }
        static Pullenti.Ner.ReferentToken TryParseShortInline(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            BookLinkRefReferent re;
            if (t.IsChar('[') && !t.IsNewlineBefore) 
            {
                Pullenti.Ner.Booklink.Internal.BookLinkToken bb = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(t, 0);
                if (bb != null && bb.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Number) 
                {
                    re = new BookLinkRefReferent();
                    re.Number = bb.Value;
                    return new Pullenti.Ner.ReferentToken(re, t, bb.EndToken);
                }
            }
            if (t.IsChar('(')) 
            {
                Pullenti.Ner.Booklink.Internal.BookLinkToken bbb = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(t.Next, 0);
                if (bbb == null) 
                    return null;
                if (bbb.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.See) 
                {
                    for (Pullenti.Ner.Token tt = bbb.EndToken.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsCharOf(",:.")) 
                            continue;
                        if (tt.IsChar('[')) 
                        {
                            if (((tt.Next is Pullenti.Ner.NumberToken) && tt.Next.Next != null && tt.Next.Next.IsChar(']')) && tt.Next.Next != null && tt.Next.Next.Next.IsChar(')')) 
                            {
                                re = new BookLinkRefReferent();
                                re.Number = (tt.Next as Pullenti.Ner.NumberToken).Value.ToString();
                                return new Pullenti.Ner.ReferentToken(re, t, tt.Next.Next.Next);
                            }
                        }
                        if ((tt is Pullenti.Ner.NumberToken) && tt.Next != null && tt.Next.IsChar(')')) 
                        {
                            re = new BookLinkRefReferent();
                            re.Number = (tt as Pullenti.Ner.NumberToken).Value.ToString();
                            return new Pullenti.Ner.ReferentToken(re, t, tt.Next);
                        }
                        break;
                    }
                    return null;
                }
                if (bbb.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Number) 
                {
                    Pullenti.Ner.Token tt1 = bbb.EndToken.Next;
                    if (tt1 != null && tt1.IsComma) 
                        tt1 = tt1.Next;
                    Pullenti.Ner.Booklink.Internal.BookLinkToken bbb2 = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(tt1, 0);
                    if ((bbb2 != null && bbb2.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.PageRange && bbb2.EndToken.Next != null) && bbb2.EndToken.Next.IsChar(')')) 
                    {
                        re = new BookLinkRefReferent();
                        re.Number = bbb.Value;
                        re.Pages = bbb2.Value;
                        return new Pullenti.Ner.ReferentToken(re, t, bbb2.EndToken.Next);
                    }
                }
            }
            return null;
        }
        static List<Pullenti.Ner.ReferentToken> TryParse(Pullenti.Ner.Token t, bool isInLit, int maxChar = 0)
        {
            if (t == null) 
                return null;
            bool isBracketRegime = false;
            if (t.Previous != null && t.Previous.IsChar('(')) 
                isBracketRegime = true;
            Pullenti.Ner.Booklink.Internal.BookLinkToken blt = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(t, 0);
            if (blt == null) 
                blt = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParseAuthor(t, Pullenti.Ner.Person.Internal.FioTemplateType.Undefined);
            if (blt == null && !isBracketRegime) 
                return null;
            Pullenti.Ner.Token t0 = t;
            double coef = (double)0;
            bool isElectrRes = false;
            Pullenti.Ner.Token decree = null;
            RegionTyp regtyp = RegionTyp.Undefined;
            string num = null;
            Pullenti.Ner.Booklink.Internal.BookLinkToken specSee = null;
            Pullenti.Ner.Referent bookPrev = null;
            if (isBracketRegime) 
                regtyp = RegionTyp.Authors;
            else if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Person) 
            {
                if (!isInLit) 
                    return null;
                regtyp = RegionTyp.Authors;
            }
            else if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Number) 
            {
                num = blt.Value;
                t = blt.EndToken.Next;
                if (t == null || t.IsNewlineBefore) 
                    return null;
                if (!t.IsWhitespaceBefore) 
                {
                    if (t is Pullenti.Ner.NumberToken) 
                    {
                        string n = (t as Pullenti.Ner.NumberToken).Value;
                        if ((((n == "3" || n == "0")) && !t.IsWhitespaceAfter && (t.Next is Pullenti.Ner.TextToken)) && t.Next.Chars.IsAllLower) 
                        {
                        }
                        else 
                            return null;
                    }
                    else if (!(t is Pullenti.Ner.TextToken) || t.Chars.IsAllLower) 
                    {
                        Pullenti.Ner.Referent r = t.GetReferent();
                        if (r is Pullenti.Ner.Person.PersonReferent) 
                        {
                        }
                        else if (isInLit && r != null && r.TypeName == "DECREE") 
                        {
                        }
                        else 
                            return null;
                    }
                }
                for (; t != null; t = t.Next) 
                {
                    if (t is Pullenti.Ner.NumberToken) 
                        break;
                    if (!(t is Pullenti.Ner.TextToken)) 
                        break;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                        break;
                    if (!t.Chars.IsLetter) 
                        continue;
                    Pullenti.Ner.Booklink.Internal.BookLinkToken bbb = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(t, 0);
                    if (bbb != null) 
                    {
                        if (bbb.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Tamze) 
                        {
                            specSee = bbb;
                            t = bbb.EndToken.Next;
                            break;
                        }
                        if (bbb.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.See) 
                        {
                            t = bbb.EndToken;
                            continue;
                        }
                    }
                    break;
                }
                if (specSee != null && specSee.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Tamze) 
                {
                    coef++;
                    int max = 1000;
                    for (Pullenti.Ner.Token tt = t0; tt != null && max > 0; tt = tt.Previous,max--) 
                    {
                        if (tt.GetReferent() is BookLinkRefReferent) 
                        {
                            bookPrev = (tt.GetReferent() as BookLinkRefReferent).Book;
                            break;
                        }
                    }
                }
                Pullenti.Ner.Booklink.Internal.BookLinkToken blt1 = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParseAuthor(t, Pullenti.Ner.Person.Internal.FioTemplateType.Undefined);
                if (blt1 != null && blt1.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Person) 
                    regtyp = RegionTyp.Authors;
                else 
                {
                    bool ok = false;
                    for (Pullenti.Ner.Token tt = t; tt != null; tt = (tt == null ? null : tt.Next)) 
                    {
                        if (tt.IsNewlineBefore) 
                            break;
                        if (isInLit && tt.GetReferent() != null && tt.GetReferent().TypeName == "DECREE") 
                        {
                            ok = true;
                            decree = tt;
                            break;
                        }
                        Pullenti.Ner.Booklink.Internal.BookLinkToken bbb = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(tt, 0);
                        if (bbb == null) 
                            continue;
                        if (bbb.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.ElectronRes) 
                        {
                            isElectrRes = true;
                            ok = true;
                            break;
                        }
                        if (bbb.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Delimeter) 
                        {
                            tt = bbb.EndToken.Next;
                            if (Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParseAuthor(tt, Pullenti.Ner.Person.Internal.FioTemplateType.Undefined) != null) 
                            {
                                ok = true;
                                break;
                            }
                            bbb = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(tt, 0);
                            if (bbb != null) 
                            {
                                if (bbb.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Editors || bbb.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Translate || bbb.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Sostavitel) 
                                {
                                    ok = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!ok && !isInLit) 
                    {
                        if (Pullenti.Ner.Booklink.Internal.BookLinkToken.CheckLinkBefore(t0, num)) 
                        {
                        }
                        else 
                            return null;
                    }
                    regtyp = RegionTyp.Name;
                }
            }
            else 
                return null;
            BookLinkReferent res = new BookLinkReferent();
            List<Pullenti.Ner.ReferentToken> corrAuthors = new List<Pullenti.Ner.ReferentToken>();
            Pullenti.Ner.Token t00 = t;
            Pullenti.Ner.Booklink.Internal.BookLinkToken blt00 = null;
            string startOfName = null;
            Pullenti.Ner.Person.Internal.FioTemplateType prevPersTempl = Pullenti.Ner.Person.Internal.FioTemplateType.Undefined;
            if (regtyp == RegionTyp.Authors) 
            {
                for (; t != null; t = t.Next) 
                {
                    if (maxChar > 0 && t.BeginChar >= maxChar) 
                        break;
                    if (t.IsCharOf(".;") || t.IsCommaAnd) 
                        continue;
                    if (t.IsChar('/')) 
                        break;
                    if ((t.IsChar('(') && t.Next != null && t.Next.IsValue("EDS", null)) && t.Next.Next != null && t.Next.Next.IsChar(')')) 
                    {
                        t = t.Next.Next.Next;
                        break;
                    }
                    blt = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParseAuthor(t, prevPersTempl);
                    if (blt == null && t.Previous != null && t.Previous.IsAnd) 
                        blt = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParseAuthor(t.Previous, Pullenti.Ner.Person.Internal.FioTemplateType.Undefined);
                    if (blt == null) 
                    {
                        if ((t.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) && blt00 != null) 
                        {
                            Pullenti.Ner.Booklink.Internal.BookLinkToken bbb2 = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(t.Next, 0);
                            if (bbb2 != null) 
                            {
                                if (bbb2.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Year) 
                                {
                                    res.AddSlot(BookLinkReferent.ATTR_AUTHOR, t.GetReferent(), false, 0);
                                    res.Year = int.Parse(bbb2.Value);
                                    coef += 0.5;
                                    t = bbb2.EndToken.Next;
                                }
                            }
                        }
                        break;
                    }
                    if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Person) 
                    {
                        Pullenti.Ner.Token tt2 = blt.EndToken.Next;
                        Pullenti.Ner.Booklink.Internal.BookLinkToken bbb2 = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(tt2, 0);
                        if (bbb2 != null) 
                        {
                            if (bbb2.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Year) 
                            {
                                res.Year = int.Parse(bbb2.Value);
                                coef += 0.5;
                                blt.EndToken = bbb2.EndToken;
                                blt00 = null;
                            }
                        }
                        if (blt00 != null && ((blt00.EndToken.Next == blt.BeginToken || blt.BeginToken.Previous.IsChar('.')))) 
                        {
                            Pullenti.Ner.Token tt11 = blt.EndToken.Next;
                            Pullenti.Ner.Booklink.Internal.BookLinkToken nex = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(tt11, 0);
                            if (nex != null && nex.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.AndOthers) 
                            {
                            }
                            else 
                            {
                                if (tt11 == null) 
                                    break;
                                if (tt11.IsChar('/') && tt11.Next != null && tt11.Next.IsChar('/')) 
                                    break;
                                if (tt11.IsChar(':')) 
                                    break;
                                if ((blt.ToString().IndexOf('.') < 0) && blt00.ToString().IndexOf('.') > 0) 
                                    break;
                                if ((tt11 is Pullenti.Ner.TextToken) && tt11.Chars.IsAllLower) 
                                    break;
                                if (tt11.IsCharOf(",.;") && tt11.Next != null) 
                                    tt11 = tt11.Next;
                                nex = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(tt11, 0);
                                if (nex != null && nex.Typ != Pullenti.Ner.Booklink.Internal.BookLinkTyp.Person && nex.Typ != Pullenti.Ner.Booklink.Internal.BookLinkTyp.AndOthers) 
                                    break;
                            }
                        }
                        else if ((blt00 != null && blt00.PersonTemplate != Pullenti.Ner.Person.Internal.FioTemplateType.Undefined && blt.PersonTemplate != blt00.PersonTemplate) && blt.PersonTemplate == Pullenti.Ner.Person.Internal.FioTemplateType.NameSurname) 
                        {
                            if (blt.EndToken.Next == null || !blt.EndToken.Next.IsCommaAnd) 
                                break;
                            if (Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParseAuthor(blt.EndToken.Next.Next, Pullenti.Ner.Person.Internal.FioTemplateType.Undefined) != null) 
                            {
                            }
                            else 
                                break;
                        }
                        if (blt00 == null && blt.PersonTemplate == Pullenti.Ner.Person.Internal.FioTemplateType.NameSurname) 
                        {
                            Pullenti.Ner.Token tt = blt.EndToken.Next;
                            if (tt != null && tt.IsHiphen) 
                                tt = tt.Next;
                            if (tt is Pullenti.Ner.NumberToken) 
                                break;
                        }
                        _addAuthor(res, blt);
                        coef++;
                        t = blt.EndToken;
                        if (t.GetReferent() is Pullenti.Ner.Person.PersonReferent) 
                            corrAuthors.Add(t as Pullenti.Ner.ReferentToken);
                        blt00 = blt;
                        prevPersTempl = blt.PersonTemplate;
                        if ((((startOfName = blt.StartOfName))) != null) 
                        {
                            t = t.Next;
                            break;
                        }
                        continue;
                    }
                    if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.AndOthers) 
                    {
                        coef += 0.5;
                        t = blt.EndToken.Next;
                        res.AuthorsAndOther = true;
                        break;
                    }
                    break;
                }
            }
            if (t == null) 
                return null;
            if ((t.IsNewlineBefore && t != t0 && num == null) && res.FindSlot(BookLinkReferent.ATTR_AUTHOR, null, true) == null) 
                return null;
            if (startOfName == null) 
            {
                if (t.Chars.IsAllLower) 
                    coef -= 1;
                if (t.Chars.IsLatinLetter && !isElectrRes && num == null) 
                {
                    if (res.GetSlotValue(BookLinkReferent.ATTR_AUTHOR) == null) 
                        return null;
                }
            }
            Pullenti.Ner.Token tn0 = t;
            Pullenti.Ner.Token tn1 = null;
            Pullenti.Ner.Uri.UriReferent uri = null;
            string nextNum = null;
            int nn;
            if (int.TryParse(num ?? "", out nn)) 
                nextNum = ((nn + 1)).ToString();
            Pullenti.Ner.Core.BracketSequenceToken br = (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false) ? Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.CanContainsVerbs | Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines, 100) : null);
            if (br != null) 
                t = t.Next;
            Pullenti.Ner.Booklink.Internal.BookLinkToken pages = null;
            for (; t != null; t = t.Next) 
            {
                if (maxChar > 0 && t.BeginChar >= maxChar) 
                    break;
                if (br != null && br.EndToken == t) 
                {
                    tn1 = t;
                    break;
                }
                Pullenti.Ner.Titlepage.Internal.TitleItemToken tit = Pullenti.Ner.Titlepage.Internal.TitleItemToken.TryAttach(t);
                if (tit != null) 
                {
                    if ((tit.Typ == Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Typ && tn0 == t && br == null) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tit.EndToken.Next, true, false)) 
                    {
                        br = Pullenti.Ner.Core.BracketHelper.TryParse(tit.EndToken.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            coef += 1;
                            if (num != null) 
                                coef++;
                            tn0 = br.BeginToken;
                            tn1 = br.EndToken;
                            res.Typ = tit.Value.ToLower();
                            t = br.EndToken.Next;
                            break;
                        }
                    }
                }
                if (t.IsNewlineBefore && t != tn0) 
                {
                    if (br != null && (t.EndChar < br.EndChar)) 
                    {
                    }
                    else if (!Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                    {
                    }
                    else 
                    {
                        if (t.NewlinesBeforeCount > 1) 
                            break;
                        if ((t is Pullenti.Ner.NumberToken) && num != null && (t as Pullenti.Ner.NumberToken).IntValue != null) 
                        {
                            if (num == (((t as Pullenti.Ner.NumberToken).IntValue.Value - 1)).ToString()) 
                                break;
                        }
                        else if (num != null) 
                        {
                        }
                        else 
                        {
                            Pullenti.Ner.Core.NounPhraseToken nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Previous, (Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition | Pullenti.Ner.Core.NounPhraseParseAttr.ParseAdverbs | Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective) | Pullenti.Ner.Core.NounPhraseParseAttr.Multilines, 0, null);
                            if (nnn != null && nnn.EndChar >= t.EndChar) 
                            {
                            }
                            else 
                                break;
                        }
                    }
                }
                if (t.IsCharOf(".;") && t.WhitespacesAfterCount > 0) 
                {
                    if ((((tit = Pullenti.Ner.Titlepage.Internal.TitleItemToken.TryAttach(t.Next)))) != null) 
                    {
                        if (tit.Typ == Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Typ) 
                            break;
                    }
                    bool stop = true;
                    int words = 0;
                    int notwords = 0;
                    for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                    {
                        Pullenti.Ner.Booklink.Internal.BookLinkToken blt0 = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(tt, 0);
                        if (blt0 == null) 
                        {
                            if (tt.IsNewlineBefore) 
                                break;
                            if ((tt is Pullenti.Ner.TextToken) && !tt.GetMorphClassInDictionary().IsUndefined) 
                                words++;
                            else 
                                notwords++;
                            if (words > 6 && words > (notwords * 4)) 
                            {
                                stop = false;
                                break;
                            }
                            continue;
                        }
                        if ((blt0.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Delimeter || blt0.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Translate || blt0.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Type) || blt0.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Geo || blt0.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Press) 
                            stop = false;
                        break;
                    }
                    if (br != null && br.EndToken.Previous.EndChar > t.EndChar) 
                        stop = false;
                    if (stop) 
                        break;
                }
                if (t == decree) 
                {
                    t = t.Next;
                    break;
                }
                blt = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(t, 0);
                if (blt == null) 
                {
                    tn1 = t;
                    continue;
                }
                if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Delimeter) 
                    break;
                if (((blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Misc || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Translate || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.NameTail) || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Type || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Volume) || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.PageRange || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Pages) 
                {
                    coef++;
                    break;
                }
                if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Geo || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Press) 
                {
                    if (t.Previous.IsHiphen || t.Previous.IsCharOf(".;") || blt.AddCoef > 0) 
                        break;
                }
                if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Year) 
                {
                    if (t.Previous != null && t.Previous.IsComma) 
                        break;
                }
                if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.ElectronRes) 
                {
                    isElectrRes = true;
                    break;
                }
                if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Url) 
                {
                    if (t == tn0 || t.Previous.IsCharOf(":.")) 
                    {
                        isElectrRes = true;
                        break;
                    }
                }
                tn1 = t;
            }
            if (tn1 == null && startOfName == null) 
            {
                if (isElectrRes) 
                {
                    BookLinkReferent uriRe = new BookLinkReferent();
                    Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(uriRe, t00, t);
                    List<Pullenti.Ner.ReferentToken> rts0 = new List<Pullenti.Ner.ReferentToken>();
                    BookLinkRefReferent bref0 = new BookLinkRefReferent() { Book = uriRe };
                    if (num != null) 
                        bref0.Number = num;
                    Pullenti.Ner.ReferentToken rt01 = new Pullenti.Ner.ReferentToken(bref0, t0, rt0.EndToken);
                    bool ok = false;
                    for (; t != null; t = t.Next) 
                    {
                        if (t.IsNewlineBefore) 
                            break;
                        Pullenti.Ner.Booklink.Internal.BookLinkToken blt0 = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(t, 0);
                        if (blt0 != null) 
                        {
                            if (blt0.Ref is Pullenti.Ner.Uri.UriReferent) 
                            {
                                uriRe.AddSlot(BookLinkReferent.ATTR_URL, blt0.Ref as Pullenti.Ner.Uri.UriReferent, false, 0);
                                ok = true;
                            }
                            t = blt0.EndToken;
                        }
                        rt0.EndToken = (rt01.EndToken = t);
                    }
                    if (ok) 
                    {
                        rts0.Add(rt01);
                        rts0.Add(rt0);
                        return rts0;
                    }
                }
                if (decree != null && num != null) 
                {
                    List<Pullenti.Ner.ReferentToken> rts0 = new List<Pullenti.Ner.ReferentToken>();
                    BookLinkRefReferent bref0 = new BookLinkRefReferent() { Book = decree.GetReferent() };
                    if (num != null) 
                        bref0.Number = num;
                    Pullenti.Ner.ReferentToken rt01 = new Pullenti.Ner.ReferentToken(bref0, t0, decree);
                    for (t = decree.Next; t != null; t = t.Next) 
                    {
                        if (t.IsNewlineBefore) 
                            break;
                        if (t is Pullenti.Ner.TextToken) 
                        {
                            if ((t as Pullenti.Ner.TextToken).IsPureVerb) 
                                return null;
                        }
                        rt01.EndToken = t;
                    }
                    rts0.Add(rt01);
                    return rts0;
                }
                if (bookPrev != null) 
                {
                    Pullenti.Ner.Token tt = t;
                    while (tt != null && ((tt.IsCharOf(",.") || tt.IsHiphen))) 
                    {
                        tt = tt.Next;
                    }
                    Pullenti.Ner.Booklink.Internal.BookLinkToken blt0 = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(tt, 0);
                    if (blt0 != null && blt0.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.PageRange) 
                    {
                        List<Pullenti.Ner.ReferentToken> rts0 = new List<Pullenti.Ner.ReferentToken>();
                        BookLinkRefReferent bref0 = new BookLinkRefReferent() { Book = bookPrev };
                        if (num != null) 
                            bref0.Number = num;
                        bref0.Pages = blt0.Value;
                        Pullenti.Ner.ReferentToken rt00 = new Pullenti.Ner.ReferentToken(bref0, t0, blt0.EndToken);
                        rts0.Add(rt00);
                        return rts0;
                    }
                }
                return null;
            }
            if (br != null && ((tn1 == br.EndToken || tn1 == br.EndToken.Previous))) 
            {
                tn0 = tn0.Next;
                tn1 = tn1.Previous;
            }
            if (startOfName == null) 
            {
                while (tn0 != null) 
                {
                    if (tn0.IsCharOf(":,~")) 
                        tn0 = tn0.Next;
                    else 
                        break;
                }
            }
            for (; tn1 != null && tn1.BeginChar > tn0.BeginChar; tn1 = tn1.Previous) 
            {
                if (tn1.IsCharOf(".;,:(~") || tn1.IsHiphen || tn1.IsValue("РЕД", null)) 
                {
                }
                else 
                    break;
            }
            string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(tn0, tn1, Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
            if (startOfName != null) 
            {
                if (nam == null || (nam.Length < 3)) 
                    nam = startOfName;
                else 
                    nam = string.Format("{0}{1}{2}", startOfName, (tn0.IsWhitespaceBefore ? " " : ""), nam);
            }
            if (nam == null) 
                return null;
            res.Name = nam;
            if (num == null && !isInLit) 
            {
                if (nam.Length < 20) 
                    return null;
                coef -= 2;
            }
            if (nam.Length > 500) 
                coef -= (nam.Length / 500);
            if (isBracketRegime) 
                coef--;
            if (nam.Length > 200) 
            {
                if (num == null) 
                    return null;
                if (res.FindSlot(BookLinkReferent.ATTR_AUTHOR, null, true) == null && !Pullenti.Ner.Booklink.Internal.BookLinkToken.CheckLinkBefore(t0, num)) 
                    return null;
            }
            int en = 0;
            int ru = 0;
            int ua = 0;
            int cha = 0;
            int nocha = 0;
            int chalen = 0;
            Pullenti.Ner.Token lt0 = tn0;
            Pullenti.Ner.Token lt1 = tn1;
            if (tn1 == null) 
            {
                if (t == null) 
                    return null;
                lt0 = t0;
                lt1 = t;
                tn1 = t.Previous;
            }
            for (Pullenti.Ner.Token tt = lt0; tt != null && tt.EndChar <= lt1.EndChar; tt = tt.Next) 
            {
                if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter) 
                {
                    if (tt.Chars.IsLatinLetter) 
                        en++;
                    else if (tt.Morph.Language.IsUa) 
                        ua++;
                    else if (tt.Morph.Language.IsRu) 
                        ru++;
                    if (tt.LengthChar > 2) 
                    {
                        cha++;
                        chalen += tt.LengthChar;
                    }
                }
                else if (!(tt is Pullenti.Ner.ReferentToken)) 
                    nocha++;
            }
            if (ru > (ua + en)) 
                res.Lang = "RU";
            else if (ua > (ru + en)) 
                res.Lang = "UA";
            else if (en > (ru + ua)) 
                res.Lang = "EN";
            if (nocha > 3 && nocha > cha && startOfName == null) 
            {
                if (nocha > (chalen / 3)) 
                    coef -= 2;
            }
            if (res.Lang == "EN") 
            {
                for (Pullenti.Ner.Token tt = tn0.Next; tt != null && (tt.EndChar < tn1.EndChar); tt = tt.Next) 
                {
                    if (tt.IsComma && tt.Next != null && ((!tt.Next.Chars.IsAllLower || (tt.Next is Pullenti.Ner.ReferentToken)))) 
                    {
                        if (tt.Next.Next != null && tt.Next.Next.IsCommaAnd) 
                        {
                            if (tt.Next is Pullenti.Ner.ReferentToken) 
                            {
                            }
                            else 
                                continue;
                        }
                        nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(tn0, tt.Previous, Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        if (nam != null && nam.Length > 15) 
                        {
                            res.Name = nam;
                            break;
                        }
                    }
                }
            }
            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(res, t00, tn1);
            bool authors = true;
            bool edits = false;
            br = null;
            for (; t != null; t = t.Next) 
            {
                if (maxChar > 0 && t.BeginChar >= maxChar) 
                    break;
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false)) 
                {
                    br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines, 100);
                    if (br != null && br.LengthChar > 300) 
                        br = null;
                }
                blt = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(t, 0);
                if (t.IsNewlineBefore && !t.IsChar('/') && !t.Previous.IsChar('/')) 
                {
                    if (blt != null && blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Number) 
                        break;
                    if (t.Previous.IsCharOf(":")) 
                    {
                    }
                    else if (blt != null && ((((blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Delimeter || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.PageRange || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Pages) || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Geo || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Press) || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.N))) 
                    {
                    }
                    else if (num != null && Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParseAuthor(t, Pullenti.Ner.Person.Internal.FioTemplateType.Undefined) != null) 
                    {
                    }
                    else if (num != null && blt != null && blt.Typ != Pullenti.Ner.Booklink.Internal.BookLinkTyp.Number) 
                    {
                    }
                    else if (br != null && (t.EndChar < br.EndChar) && t.BeginChar > br.BeginChar) 
                    {
                    }
                    else 
                    {
                        bool ok = false;
                        int mmm = 50;
                        for (Pullenti.Ner.Token tt = t.Next; tt != null && mmm > 0; tt = tt.Next,mmm--) 
                        {
                            if (tt.IsNewlineBefore) 
                            {
                                Pullenti.Ner.Booklink.Internal.BookLinkToken blt2 = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParse(tt, 0);
                                if (blt2 != null && blt2.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Number && blt2.Value == nextNum) 
                                {
                                    ok = true;
                                    break;
                                }
                                if (blt2 != null) 
                                {
                                    if (blt2.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Pages || blt2.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Geo || blt2.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Press) 
                                    {
                                        ok = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (!ok) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Previous, (Pullenti.Ner.Core.NounPhraseParseAttr.Multilines | Pullenti.Ner.Core.NounPhraseParseAttr.ParseAdverbs | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition) | Pullenti.Ner.Core.NounPhraseParseAttr.ParseVerbs | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns, 0, null);
                            if (npt != null && npt.EndChar >= t.EndChar) 
                                ok = true;
                        }
                        if (!ok) 
                            break;
                    }
                }
                rt.EndToken = t;
                if (blt != null) 
                    rt.EndToken = blt.EndToken;
                if (t.IsCharOf(".,") || t.IsHiphen) 
                    continue;
                if (t.IsValue("С", null)) 
                {
                }
                if (regtyp == RegionTyp.First && blt != null && blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Editors) 
                {
                    edits = true;
                    t = blt.EndToken;
                    coef++;
                    continue;
                }
                if (regtyp == RegionTyp.First && blt != null && blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Sostavitel) 
                {
                    edits = false;
                    t = blt.EndToken;
                    coef++;
                    continue;
                }
                if (regtyp == RegionTyp.First && authors) 
                {
                    Pullenti.Ner.Booklink.Internal.BookLinkToken blt2 = Pullenti.Ner.Booklink.Internal.BookLinkToken.TryParseAuthor(t, prevPersTempl);
                    if (blt2 != null && blt2.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Person) 
                    {
                        prevPersTempl = blt2.PersonTemplate;
                        if (!edits) 
                            _addAuthor(res, blt2);
                        coef++;
                        t = blt2.EndToken;
                        continue;
                    }
                    if (blt2 != null && blt2.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.AndOthers) 
                    {
                        if (!edits) 
                            res.AuthorsAndOther = true;
                        coef++;
                        t = blt2.EndToken;
                        continue;
                    }
                    authors = false;
                }
                if (blt == null) 
                    continue;
                if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.ElectronRes || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Url) 
                {
                    isElectrRes = true;
                    if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.ElectronRes) 
                        coef += 1.5;
                    else 
                        coef += 0.5;
                    if (blt.Ref is Pullenti.Ner.Uri.UriReferent) 
                        res.AddSlot(BookLinkReferent.ATTR_URL, blt.Ref as Pullenti.Ner.Uri.UriReferent, false, 0);
                }
                else if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Year) 
                {
                    if (res.Year == 0) 
                    {
                        res.Year = int.Parse(blt.Value);
                        coef += 0.5;
                    }
                }
                else if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Delimeter) 
                {
                    coef++;
                    if (blt.LengthChar == 2) 
                        regtyp = RegionTyp.Second;
                    else 
                        regtyp = RegionTyp.First;
                }
                else if ((((blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Misc || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Type || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Pages) || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.NameTail || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Translate) || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Press || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Volume) || blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.N) 
                    coef++;
                else if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.PageRange) 
                {
                    pages = blt;
                    coef++;
                    if (isBracketRegime && blt.EndToken.Next != null && blt.EndToken.Next.IsChar(')')) 
                    {
                        coef += 2;
                        if (res.Name != null && res.FindSlot(BookLinkReferent.ATTR_AUTHOR, null, true) != null) 
                            coef = 10;
                    }
                }
                else if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Geo && ((regtyp == RegionTyp.Second || regtyp == RegionTyp.First))) 
                    coef++;
                else if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.Geo && t.Previous != null && t.Previous.IsChar('.')) 
                    coef++;
                else if (blt.Typ == Pullenti.Ner.Booklink.Internal.BookLinkTyp.AndOthers) 
                {
                    coef++;
                    if (authors) 
                        res.AuthorsAndOther = true;
                }
                coef += blt.AddCoef;
                t = blt.EndToken;
            }
            if ((coef < 2.5) && num != null) 
            {
                if (Pullenti.Ner.Booklink.Internal.BookLinkToken.CheckLinkBefore(t0, num)) 
                    coef += 2;
                else if (Pullenti.Ner.Booklink.Internal.BookLinkToken.CheckLinkAfter(rt.EndToken, num)) 
                    coef += 1;
            }
            if (rt.LengthChar > 500) 
                return null;
            if (isInLit) 
                coef++;
            if (coef < 2.5) 
            {
                if (isElectrRes && uri != null) 
                {
                }
                else if (coef >= 2 && isInLit) 
                {
                }
                else 
                    return null;
            }
            foreach (Pullenti.Ner.ReferentToken rr in corrAuthors) 
            {
                List<Pullenti.Ner.Person.Internal.PersonItemToken> pits0 = Pullenti.Ner.Person.Internal.PersonItemToken.TryAttachList(rr.BeginToken, null, Pullenti.Ner.Person.Internal.PersonItemToken.ParseAttr.CanInitialBeDigit, 10);
                if (pits0 == null || (pits0.Count < 2)) 
                    continue;
                if (pits0[0].Typ == Pullenti.Ner.Person.Internal.PersonItemToken.ItemType.Value) 
                {
                    bool exi = false;
                    for (int i = rr.Referent.Slots.Count - 1; i >= 0; i--) 
                    {
                        Pullenti.Ner.Slot s = rr.Referent.Slots[i];
                        if (s.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME) 
                        {
                            string ln = s.Value as string;
                            if (ln == null) 
                                continue;
                            if (ln == pits0[0].Value) 
                            {
                                exi = true;
                                continue;
                            }
                            if (ln.IndexOf('-') > 0) 
                                ln = ln.Substring(0, ln.IndexOf('-'));
                            if (pits0[0].BeginToken.IsValue(ln, null)) 
                                rr.Referent.Slots.RemoveAt(i);
                        }
                    }
                    if (!exi) 
                        rr.Referent.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, pits0[0].Value, false, 0);
                }
            }
            List<Pullenti.Ner.ReferentToken> rts = new List<Pullenti.Ner.ReferentToken>();
            BookLinkRefReferent bref = new BookLinkRefReferent() { Book = res };
            if (num != null) 
                bref.Number = num;
            Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(bref, t0, rt.EndToken);
            if (pages != null) 
            {
                if (pages.Value != null) 
                    bref.Pages = pages.Value;
                rt.EndToken = pages.BeginToken.Previous;
            }
            rts.Add(rt1);
            rts.Add(rt);
            return rts;
        }
        static void _addAuthor(BookLinkReferent blr, Pullenti.Ner.Booklink.Internal.BookLinkToken tok)
        {
            if (tok.Ref != null) 
                blr.AddSlot(BookLinkReferent.ATTR_AUTHOR, tok.Ref, false, 0);
            else if (tok.Tok != null) 
            {
                blr.AddSlot(BookLinkReferent.ATTR_AUTHOR, tok.Tok.Referent, false, 0);
                blr.AddExtReferent(tok.Tok);
            }
            else if (tok.Value != null) 
                blr.AddSlot(BookLinkReferent.ATTR_AUTHOR, tok.Value, false, 0);
        }
        enum RegionTyp : int
        {
            Undefined,
            Authors,
            Name,
            First,
            Second,
        }

        public static void Initialize()
        {
            Pullenti.Ner.Booklink.Internal.MetaBookLink.Initialize2();
            Pullenti.Ner.Booklink.Internal.MetaBookLinkRef.Initialize();
            try 
            {
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Booklink.Internal.BookLinkToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new BookLinkAnalyzer());
        }
    }
}