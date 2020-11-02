/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Uri
{
    /// <summary>
    /// Анализатор для выделения URI-объектов (схема:значение)
    /// </summary>
    public class UriAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("URI")
        /// </summary>
        public const string ANALYZER_NAME = "URI";
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "URI";
            }
        }
        public override string Description
        {
            get
            {
                return "URI (URL, EMail), ISBN, УДК, ББК ...";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new UriAnalyzer();
        }
        public override int ProgressWeight
        {
            get
            {
                return 2;
            }
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Uri.Internal.MetaUri.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Uri.Internal.MetaUri.MailImageId, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("email.png"));
                res.Add(Pullenti.Ner.Uri.Internal.MetaUri.UriImageId, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("uri.png"));
                return res;
            }
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {"PHONE"};
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == UriReferent.OBJ_TYPENAME) 
                return new UriReferent();
            return null;
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.Token tt = t;
                int i;
                Pullenti.Ner.Core.TerminToken tok = m_Schemes.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                {
                    i = (int)tok.Termin.Tag;
                    tt = tok.EndToken;
                    if (tt.Next != null && tt.Next.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.TerminToken tok1 = m_Schemes.TryParse(tt.Next.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                        if ((tok1 != null && tok1.Termin.CanonicText == tok.Termin.CanonicText && tok1.EndToken.Next != null) && tok1.EndToken.Next.IsChar(')')) 
                            tt = tok1.EndToken.Next;
                    }
                    if (i == 0) 
                    {
                        if ((tt.Next == null || ((!tt.Next.IsCharOf(":|") && !tt.IsTableControlChar)) || tt.Next.IsWhitespaceBefore) || tt.Next.WhitespacesAfterCount > 2) 
                            continue;
                        Pullenti.Ner.Token t1 = tt.Next.Next;
                        while (t1 != null && t1.IsCharOf("/\\")) 
                        {
                            t1 = t1.Next;
                        }
                        if (t1 == null || t1.WhitespacesBeforeCount > 2) 
                            continue;
                        Pullenti.Ner.Uri.Internal.UriItemToken ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachUriContent(t1, false);
                        if (ut == null) 
                            continue;
                        UriReferent ur = ad.RegisterReferent(new UriReferent() { Scheme = tok.Termin.CanonicText.ToLower(), Value = ut.Value }) as UriReferent;
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(ur), t, ut.EndToken);
                        rt.BeginToken = _siteBefore(t.Previous) ?? t;
                        if (rt.EndToken.Next != null && rt.EndToken.Next.IsCharOf("/\\")) 
                            rt.EndToken = rt.EndToken.Next;
                        kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                    if (i == 10) 
                    {
                        tt = tt.Next;
                        if (tt == null || !tt.IsChar(':')) 
                            continue;
                        for (tt = tt.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.IsCharOf("/\\")) 
                            {
                            }
                            else 
                                break;
                        }
                        if (tt == null) 
                            continue;
                        if (tt.IsValue("WWW", null) && tt.Next != null && tt.Next.IsChar('.')) 
                            tt = tt.Next.Next;
                        if (tt == null || tt.IsNewlineBefore) 
                            continue;
                        Pullenti.Ner.Uri.Internal.UriItemToken ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachUriContent(tt, true);
                        if (ut == null) 
                            continue;
                        if (ut.Value.Length < 4) 
                            continue;
                        UriReferent ur = ad.RegisterReferent(new UriReferent() { Scheme = tok.Termin.CanonicText.ToLower(), Value = ut.Value }) as UriReferent;
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(ur), t, ut.EndToken);
                        rt.BeginToken = _siteBefore(t.Previous) ?? t;
                        if (rt.EndToken.Next != null && rt.EndToken.Next.IsCharOf("/\\")) 
                            rt.EndToken = rt.EndToken.Next;
                        kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                    if (i == 2) 
                    {
                        if (tt.Next == null || !tt.Next.IsChar('.') || tt.Next.IsWhitespaceBefore) 
                            continue;
                        if (tt.Next.IsWhitespaceAfter && tok.Termin.CanonicText != "WWW") 
                            continue;
                        Pullenti.Ner.Uri.Internal.UriItemToken ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachUriContent(tt.Next.Next, true);
                        if (ut == null) 
                            continue;
                        UriReferent ur = ad.RegisterReferent(new UriReferent() { Scheme = "http", Value = ut.Value }) as UriReferent;
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ur, t, ut.EndToken);
                        rt.BeginToken = _siteBefore(t.Previous) ?? t;
                        if (rt.EndToken.Next != null && rt.EndToken.Next.IsCharOf("/\\")) 
                            rt.EndToken = rt.EndToken.Next;
                        kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                    if (i == 1) 
                    {
                        string sch = tok.Termin.CanonicText;
                        Pullenti.Ner.Uri.Internal.UriItemToken ut = null;
                        if (sch == "ISBN") 
                        {
                            ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachISBN(tt.Next);
                            if ((ut == null && t.Previous != null && t.Previous.IsChar('(')) && t.Next != null && t.Next.IsChar(')')) 
                            {
                                for (Pullenti.Ner.Token tt0 = t.Previous.Previous; tt0 != null; tt0 = tt0.Previous) 
                                {
                                    if (tt0.WhitespacesAfterCount > 2) 
                                        break;
                                    if (tt0.IsWhitespaceBefore) 
                                    {
                                        ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachISBN(tt0);
                                        if (ut != null && ut.EndToken.Next != t.Previous) 
                                            ut = null;
                                        break;
                                    }
                                }
                            }
                        }
                        else if ((sch == "RFC" || sch == "ISO" || sch == "ОКФС") || sch == "ОКОПФ") 
                            ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachISOContent(tt.Next, ":");
                        else if (sch == "ГОСТ") 
                            ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachISOContent(tt.Next, "-.");
                        else if (sch == "ТУ") 
                        {
                            if (tok.Chars.IsAllUpper) 
                            {
                                ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachISOContent(tt.Next, "-.");
                                if (ut != null && (ut.LengthChar < 10)) 
                                    ut = null;
                            }
                        }
                        else 
                            ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachBBK(tt.Next);
                        if (ut == null) 
                            continue;
                        UriReferent ur = ad.RegisterReferent(new UriReferent() { Value = ut.Value, Scheme = sch }) as UriReferent;
                        Pullenti.Ner.ReferentToken rt;
                        if (ut.BeginChar < t.BeginChar) 
                        {
                            rt = new Pullenti.Ner.ReferentToken(ur, ut.BeginToken, t);
                            if (t.Next != null && t.Next.IsChar(')')) 
                                rt.EndToken = t.Next;
                        }
                        else 
                            rt = new Pullenti.Ner.ReferentToken(ur, t, ut.EndToken);
                        if (t.Previous != null && t.Previous.IsValue("КОД", null)) 
                            rt.BeginToken = t.Previous;
                        if (ur.Scheme.StartsWith("ОК")) 
                            _checkDetail(rt);
                        kit.EmbedToken(rt);
                        t = rt;
                        if (ur.Scheme.StartsWith("ОК")) 
                        {
                            while (t.Next != null) 
                            {
                                if (t.Next.IsCommaAnd && (t.Next.Next is Pullenti.Ner.NumberToken)) 
                                {
                                }
                                else 
                                    break;
                                ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachBBK(t.Next.Next);
                                if (ut == null) 
                                    break;
                                ur = ad.RegisterReferent(new UriReferent() { Value = ut.Value, Scheme = sch }) as UriReferent;
                                rt = new Pullenti.Ner.ReferentToken(ur, t.Next.Next, ut.EndToken);
                                _checkDetail(rt);
                                kit.EmbedToken(rt);
                                t = rt;
                            }
                        }
                        continue;
                    }
                    if (i == 3) 
                    {
                        Pullenti.Ner.Token t0 = tt.Next;
                        while (t0 != null) 
                        {
                            if (t0.IsCharOf(":|") || t0.IsTableControlChar || t0.IsHiphen) 
                                t0 = t0.Next;
                            else 
                                break;
                        }
                        if (t0 == null) 
                            continue;
                        Pullenti.Ner.Uri.Internal.UriItemToken ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachSkype(t0);
                        if (ut == null) 
                            continue;
                        UriReferent ur = ad.RegisterReferent(new UriReferent() { Value = ut.Value.ToLower(), Scheme = (tok.Termin.CanonicText == "SKYPE" ? "skype" : tok.Termin.CanonicText) }) as UriReferent;
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ur, t, ut.EndToken);
                        kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                    if (i == 4) 
                    {
                        Pullenti.Ner.Token t0 = tt.Next;
                        if (t0 != null && ((t0.IsChar(':') || t0.IsHiphen))) 
                            t0 = t0.Next;
                        if (t0 == null) 
                            continue;
                        Pullenti.Ner.Uri.Internal.UriItemToken ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachIcqContent(t0);
                        if (ut == null) 
                            continue;
                        UriReferent ur = ad.RegisterReferent(new UriReferent() { Value = ut.Value, Scheme = "ICQ" }) as UriReferent;
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ur, t, t0);
                        kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                    if (i == 5 || i == 6) 
                    {
                        Pullenti.Ner.Token t0 = tt.Next;
                        bool hasTabCel = false;
                        bool isIban = false;
                        for (; t0 != null; t0 = t0.Next) 
                        {
                            if ((((t0.IsValue("БАНК", null) || t0.Morph.Class.IsPreposition || t0.IsHiphen) || t0.IsCharOf(".:") || t0.IsValue("РУБЛЬ", null)) || t0.IsValue("РУБ", null) || t0.IsValue("ДОЛЛАР", null)) || t0.IsValue("№", null) || t0.IsValue("N", null)) 
                            {
                            }
                            else if (t0.IsTableControlChar) 
                                hasTabCel = true;
                            else if (t0.IsCharOf("\\/") && t0.Next != null && t0.Next.IsValue("IBAN", null)) 
                            {
                                isIban = true;
                                t0 = t0.Next;
                            }
                            else if (t0.IsValue("IBAN", null)) 
                                isIban = true;
                            else if (t0 is Pullenti.Ner.TextToken) 
                            {
                                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t0, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt != null && npt.Morph.Case.IsGenitive) 
                                {
                                    t0 = npt.EndToken;
                                    continue;
                                }
                                break;
                            }
                            else 
                                break;
                        }
                        if (t0 == null) 
                            continue;
                        UriReferent ur2 = null;
                        Pullenti.Ner.Token ur2Begin = null;
                        Pullenti.Ner.Token ur2End = null;
                        Pullenti.Ner.Token t00 = t0;
                        string val = t0.GetSourceText();
                        if (char.IsDigit(val[0]) && ((((i == 6 || tok.Termin.CanonicText == "ИНН" || tok.Termin.CanonicText == "БИК") || tok.Termin.CanonicText == "ОГРН" || tok.Termin.CanonicText == "СНИЛС") || tok.Termin.CanonicText == "ОКПО"))) 
                        {
                            if (t0.Chars.IsLetter) 
                                continue;
                            if (string.IsNullOrEmpty(val) || !char.IsDigit(val[0])) 
                                continue;
                            if (t0.LengthChar < 9) 
                            {
                                StringBuilder tmp = new StringBuilder();
                                tmp.Append(val);
                                for (Pullenti.Ner.Token ttt = t0.Next; ttt != null; ttt = ttt.Next) 
                                {
                                    if (ttt.WhitespacesBeforeCount > 1) 
                                        break;
                                    if (ttt is Pullenti.Ner.NumberToken) 
                                    {
                                        tmp.Append(ttt.GetSourceText());
                                        t0 = ttt;
                                        continue;
                                    }
                                    if (ttt.IsHiphen || ttt.IsChar('.')) 
                                    {
                                        if (ttt.Next == null || !(ttt.Next is Pullenti.Ner.NumberToken)) 
                                            break;
                                        if (ttt.IsWhitespaceAfter || ttt.IsWhitespaceBefore) 
                                            break;
                                        continue;
                                    }
                                    break;
                                }
                                val = null;
                                if (tmp.Length == 20) 
                                    val = tmp.ToString();
                                else if (tmp.Length == 9 && tok.Termin.CanonicText == "БИК") 
                                    val = tmp.ToString();
                                else if (((tmp.Length == 10 || tmp.Length == 12)) && tok.Termin.CanonicText == "ИНН") 
                                    val = tmp.ToString();
                                else if (tmp.Length >= 15 && tok.Termin.CanonicText == "Л/С") 
                                    val = tmp.ToString();
                                else if (tmp.Length >= 11 && ((tok.Termin.CanonicText == "ОГРН" || tok.Termin.CanonicText == "СНИЛС"))) 
                                    val = tmp.ToString();
                                else if (tok.Termin.CanonicText == "ОКПО") 
                                    val = tmp.ToString();
                            }
                            if (val == null) 
                                continue;
                        }
                        else if (!(t0 is Pullenti.Ner.NumberToken)) 
                        {
                            if ((t0 is Pullenti.Ner.TextToken) && isIban) 
                            {
                                StringBuilder tmp1 = new StringBuilder();
                                Pullenti.Ner.Token t1 = null;
                                for (Pullenti.Ner.Token ttt = t0; ttt != null; ttt = ttt.Next) 
                                {
                                    if (ttt.IsNewlineBefore && ttt != t0) 
                                        break;
                                    if (ttt.IsHiphen) 
                                        continue;
                                    if (!(ttt is Pullenti.Ner.NumberToken)) 
                                    {
                                        if (!(ttt is Pullenti.Ner.TextToken) || !ttt.Chars.IsLatinLetter) 
                                            break;
                                    }
                                    tmp1.Append(ttt.GetSourceText());
                                    t1 = ttt;
                                    if (tmp1.Length >= 34) 
                                        break;
                                }
                                if (tmp1.Length < 10) 
                                    continue;
                                UriReferent ur1 = new UriReferent() { Value = tmp1.ToString(), Scheme = tok.Termin.CanonicText };
                                ur1.AddSlot(UriReferent.ATTR_DETAIL, "IBAN", false, 0);
                                Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(ur1), t, t1);
                                kit.EmbedToken(rt1);
                                t = rt1;
                                continue;
                            }
                            if (!t0.IsCharOf("/\\") || t0.Next == null) 
                                continue;
                            Pullenti.Ner.Core.TerminToken tok2 = m_Schemes.TryParse(t0.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                            if (tok2 == null || !(tok2.Termin.Tag is int) || ((int)tok2.Termin.Tag) != i) 
                                continue;
                            t0 = tok2.EndToken.Next;
                            while (t0 != null) 
                            {
                                if (t0.IsCharOf(":N№")) 
                                    t0 = t0.Next;
                                else if (t0.IsTableControlChar) 
                                {
                                    t0 = t0.Next;
                                    t00 = t0;
                                    hasTabCel = true;
                                }
                                else 
                                    break;
                            }
                            if (!(t0 is Pullenti.Ner.NumberToken)) 
                                continue;
                            StringBuilder tmp = new StringBuilder();
                            for (; t0 != null; t0 = t0.Next) 
                            {
                                if (!(t0 is Pullenti.Ner.NumberToken)) 
                                    break;
                                tmp.Append(t0.GetSourceText());
                            }
                            if (t0 == null || !t0.IsCharOf("/\\,") || !(t0.Next is Pullenti.Ner.NumberToken)) 
                                continue;
                            val = tmp.ToString();
                            tmp.Length = 0;
                            ur2Begin = t0.Next;
                            for (t0 = t0.Next; t0 != null; t0 = t0.Next) 
                            {
                                if (!(t0 is Pullenti.Ner.NumberToken)) 
                                    break;
                                if (t0.WhitespacesBeforeCount > 4 && tmp.Length > 0) 
                                    break;
                                tmp.Append(t0.GetSourceText());
                                ur2End = t0;
                            }
                            ur2 = ad.RegisterReferent(new UriReferent() { Scheme = tok2.Termin.CanonicText, Value = tmp.ToString() }) as UriReferent;
                        }
                        if (val.Length < 5) 
                            continue;
                        UriReferent ur = ad.RegisterReferent(new UriReferent() { Value = val, Scheme = tok.Termin.CanonicText }) as UriReferent;
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ur, t, (ur2Begin == null ? t0 : ur2Begin.Previous));
                        if (hasTabCel) 
                            rt.BeginToken = t00;
                        if (ur.Scheme.StartsWith("ОК")) 
                            _checkDetail(rt);
                        for (Pullenti.Ner.Token ttt = t.Previous; ttt != null; ttt = ttt.Previous) 
                        {
                            if (ttt.IsTableControlChar) 
                                break;
                            if (ttt.Morph.Class.IsPreposition) 
                                continue;
                            if (ttt.IsValue("ОРГАНИЗАЦИЯ", null)) 
                                continue;
                            if (ttt.IsValue("НОМЕР", null) || ttt.IsValue("КОД", null)) 
                                t = (rt.BeginToken = ttt);
                            break;
                        }
                        kit.EmbedToken(rt);
                        t = rt;
                        if (ur2 != null) 
                        {
                            Pullenti.Ner.ReferentToken rt2 = new Pullenti.Ner.ReferentToken(ur2, ur2Begin, ur2End);
                            kit.EmbedToken(rt2);
                            t = rt2;
                        }
                        while ((t.Next != null && t.Next.IsCommaAnd && (t.Next.Next is Pullenti.Ner.NumberToken)) && t.Next.Next.LengthChar == val.Length && (t.Next.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) 
                        {
                            string val2 = t.Next.Next.GetSourceText();
                            ur2 = new UriReferent();
                            ur2.Scheme = ur.Scheme;
                            ur2.Value = val2;
                            ur2 = ad.RegisterReferent(ur2) as UriReferent;
                            Pullenti.Ner.ReferentToken rt2 = new Pullenti.Ner.ReferentToken(ur2, t.Next, t.Next.Next);
                            kit.EmbedToken(rt2);
                            t = rt2;
                        }
                        continue;
                    }
                    continue;
                }
                if (t.IsChar('@')) 
                {
                    List<Pullenti.Ner.Uri.Internal.UriItemToken> u1s = Pullenti.Ner.Uri.Internal.UriItemToken.AttachMailUsers(t.Previous);
                    if (u1s == null) 
                        continue;
                    Pullenti.Ner.Uri.Internal.UriItemToken u2 = Pullenti.Ner.Uri.Internal.UriItemToken.AttachDomainName(t.Next, false, true);
                    if (u2 == null) 
                        continue;
                    for (int ii = u1s.Count - 1; ii >= 0; ii--) 
                    {
                        UriReferent ur = ad.RegisterReferent(new UriReferent() { Value = string.Format("{0}@{1}", u1s[ii].Value, u2.Value).ToLower(), Scheme = "mailto" }) as UriReferent;
                        Pullenti.Ner.Token b = u1s[ii].BeginToken;
                        Pullenti.Ner.Token t0 = b.Previous;
                        if (t0 != null && t0.IsChar(':')) 
                            t0 = t0.Previous;
                        if (t0 != null && ii == 0) 
                        {
                            bool br = false;
                            for (Pullenti.Ner.Token ttt = t0; ttt != null; ttt = ttt.Previous) 
                            {
                                if (!(ttt is Pullenti.Ner.TextToken)) 
                                    break;
                                if (ttt != t0 && ttt.WhitespacesAfterCount > 1) 
                                    break;
                                if (ttt.IsChar(')')) 
                                {
                                    br = true;
                                    continue;
                                }
                                if (ttt.IsChar('(')) 
                                {
                                    if (!br) 
                                        break;
                                    br = false;
                                    continue;
                                }
                                if (ttt.IsValue("EMAIL", null) || ttt.IsValue("MAILTO", null)) 
                                {
                                    b = ttt;
                                    break;
                                }
                                if (ttt.IsValue("MAIL", null)) 
                                {
                                    b = ttt;
                                    if ((ttt.Previous != null && ttt.Previous.IsHiphen && ttt.Previous.Previous != null) && ((ttt.Previous.Previous.IsValue("E", null) || ttt.Previous.Previous.IsValue("Е", null)))) 
                                        b = ttt.Previous.Previous;
                                    break;
                                }
                                if (ttt.IsValue("ПОЧТА", null) || ttt.IsValue("АДРЕС", null)) 
                                {
                                    b = t0;
                                    ttt = ttt.Previous;
                                    if (ttt != null && ttt.IsChar('.')) 
                                        ttt = ttt.Previous;
                                    if (ttt != null && ((t0.IsValue("ЭЛ", null) || ttt.IsValue("ЭЛЕКТРОННЫЙ", null)))) 
                                        b = ttt;
                                    if (b.Previous != null && b.Previous.IsValue("АДРЕС", null)) 
                                        b = b.Previous;
                                    break;
                                }
                                if (ttt.Morph.Class.IsPreposition) 
                                    continue;
                            }
                        }
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ur, b, (ii == (u1s.Count - 1) ? u2.EndToken : u1s[ii].EndToken));
                        kit.EmbedToken(rt);
                        t = rt;
                    }
                    continue;
                }
                if (!t.Chars.IsCyrillicLetter) 
                {
                    if (t.IsWhitespaceBefore || ((t.Previous != null && t.Previous.IsCharOf(",(")))) 
                    {
                        Pullenti.Ner.Uri.Internal.UriItemToken u1 = Pullenti.Ner.Uri.Internal.UriItemToken.AttachUrl(t);
                        if (u1 != null) 
                        {
                            if (u1.IsWhitespaceAfter || u1.EndToken.Next == null || !u1.EndToken.Next.IsChar('@')) 
                            {
                                if (u1.EndToken.Next != null && u1.EndToken.Next.IsCharOf("\\/")) 
                                {
                                    Pullenti.Ner.Uri.Internal.UriItemToken u2 = Pullenti.Ner.Uri.Internal.UriItemToken.AttachUriContent(t, false);
                                    if (u2 != null) 
                                        u1 = u2;
                                }
                                UriReferent ur = ad.RegisterReferent(new UriReferent() { Scheme = "http", Value = u1.Value }) as UriReferent;
                                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ur, u1.BeginToken, u1.EndToken);
                                rt.BeginToken = _siteBefore(u1.BeginToken.Previous) ?? u1.BeginToken;
                                kit.EmbedToken(rt);
                                t = rt;
                                continue;
                            }
                        }
                    }
                }
                if ((t is Pullenti.Ner.TextToken) && !t.IsWhitespaceAfter && t.LengthChar > 2) 
                {
                    if (_siteBefore(t.Previous) != null) 
                    {
                        Pullenti.Ner.Uri.Internal.UriItemToken ut = Pullenti.Ner.Uri.Internal.UriItemToken.AttachUriContent(t, true);
                        if (ut == null || ut.Value.IndexOf('.') <= 0 || ut.Value.IndexOf('@') > 0) 
                            continue;
                        UriReferent ur = ad.RegisterReferent(new UriReferent() { Scheme = "http", Value = ut.Value }) as UriReferent;
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ur, t, ut.EndToken);
                        rt.BeginToken = _siteBefore(t.Previous);
                        if (rt.EndToken.Next != null && rt.EndToken.Next.IsCharOf("/\\")) 
                            rt.EndToken = rt.EndToken.Next;
                        kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                }
                if ((t.Chars.IsLatinLetter && !t.Chars.IsAllLower && t.Next != null) && !t.IsWhitespaceAfter) 
                {
                    if (t.Next.IsChar('/')) 
                    {
                        Pullenti.Ner.ReferentToken rt = _TryAttachLotus(t as Pullenti.Ner.TextToken);
                        if (rt != null) 
                        {
                            rt.Referent = ad.RegisterReferent(rt.Referent);
                            kit.EmbedToken(rt);
                            t = rt;
                            continue;
                        }
                    }
                }
            }
        }
        static void _checkDetail(Pullenti.Ner.ReferentToken rt)
        {
            if (rt.EndToken.WhitespacesAfterCount > 2 || rt.EndToken.Next == null) 
                return;
            if (rt.EndToken.Next.IsChar('(')) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(rt.EndToken.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    (rt.Referent as UriReferent).Detail = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken.Next, br.EndToken.Previous, Pullenti.Ner.Core.GetTextAttr.No);
                    rt.EndToken = br.EndToken;
                }
            }
        }
        static Pullenti.Ner.Token _siteBefore(Pullenti.Ner.Token t)
        {
            if (t != null && t.IsChar(':')) 
                t = t.Previous;
            if (t == null) 
                return null;
            if ((t.IsValue("ВЕБСАЙТ", null) || t.IsValue("WEBSITE", null) || t.IsValue("WEB", null)) || t.IsValue("WWW", null)) 
                return t;
            Pullenti.Ner.Token t0 = null;
            if (t.IsValue("САЙТ", null) || t.IsValue("SITE", null)) 
            {
                t0 = t;
                t = t.Previous;
            }
            else if (t.IsValue("АДРЕС", null)) 
            {
                t0 = t.Previous;
                if (t0 != null && t0.IsChar('.')) 
                    t0 = t0.Previous;
                if (t0 != null) 
                {
                    if (t0.IsValue("ЭЛ", null) || t0.IsValue("ЭЛЕКТРОННЫЙ", null)) 
                        return t0;
                }
                return null;
            }
            else 
                return null;
            if (t != null && t.IsHiphen) 
                t = t.Previous;
            if (t == null) 
                return t0;
            if (t.IsValue("WEB", null) || t.IsValue("ВЕБ", null)) 
                t0 = t;
            if (t0.Previous != null && t0.Previous.Morph.Class.IsAdjective && (t0.WhitespacesBeforeCount < 3)) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t0.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                    t0 = npt.BeginToken;
            }
            return t0;
        }
        static Pullenti.Ner.ReferentToken _TryAttachLotus(Pullenti.Ner.TextToken t)
        {
            if (t == null || t.Next == null) 
                return null;
            Pullenti.Ner.Token t1 = t.Next.Next;
            List<string> tails = null;
            for (Pullenti.Ner.Token tt = t1; tt != null; tt = tt.Next) 
            {
                if (tt.IsWhitespaceBefore) 
                {
                    if (!tt.IsNewlineBefore) 
                        break;
                    if (tails == null || (tails.Count < 2)) 
                        break;
                }
                if (!tt.IsLetters || tt.Chars.IsAllLower) 
                    return null;
                if (!(tt is Pullenti.Ner.TextToken)) 
                    return null;
                if (tails == null) 
                    tails = new List<string>();
                tails.Add((tt as Pullenti.Ner.TextToken).Term);
                t1 = tt;
                if (tt.IsWhitespaceAfter || tt.Next == null) 
                    break;
                tt = tt.Next;
                if (!tt.IsChar('/')) 
                    break;
            }
            if (tails == null || (tails.Count < 3)) 
                return null;
            List<string> heads = new List<string>();
            heads.Add(t.Term);
            Pullenti.Ner.Token t0 = (Pullenti.Ner.Token)t;
            bool ok = true;
            for (int k = 0; k < 2; k++) 
            {
                if (!(t0.Previous is Pullenti.Ner.TextToken)) 
                    break;
                if (t0.WhitespacesBeforeCount != 1) 
                {
                    if (!t0.IsNewlineBefore || k > 0) 
                        break;
                }
                if (!t0.IsWhitespaceBefore && t0.Previous.IsChar('/')) 
                    break;
                if (t0.Previous.Chars == t.Chars) 
                {
                    t0 = t0.Previous;
                    heads.Insert(0, (t0 as Pullenti.Ner.TextToken).Term);
                    ok = true;
                    continue;
                }
                if ((t0.Previous.Chars.IsLatinLetter && t0.Previous.Chars.IsAllUpper && t0.Previous.LengthChar == 1) && k == 0) 
                {
                    t0 = t0.Previous;
                    heads.Insert(0, (t0 as Pullenti.Ner.TextToken).Term);
                    ok = false;
                    continue;
                }
                break;
            }
            if (!ok) 
                heads.RemoveAt(0);
            StringBuilder tmp = new StringBuilder();
            for (int i = 0; i < heads.Count; i++) 
            {
                if (i > 0) 
                    tmp.Append(' ');
                tmp.Append(Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(heads[i]));
            }
            foreach (string tail in tails) 
            {
                tmp.AppendFormat("/{0}", tail);
            }
            if (((t1.Next != null && t1.Next.IsChar('@') && t1.Next.Next != null) && t1.Next.Next.Chars.IsLatinLetter && !t1.Next.IsWhitespaceAfter) && !t1.IsWhitespaceAfter) 
                t1 = t1.Next.Next;
            UriReferent uri = new UriReferent() { Scheme = "lotus", Value = tmp.ToString() };
            return new Pullenti.Ner.ReferentToken(uri, t0, t1);
        }
        internal static Pullenti.Ner.Core.TerminCollection m_Schemes;
        public static void Initialize()
        {
            if (m_Schemes != null) 
                return;
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
            Pullenti.Ner.Uri.Internal.MetaUri.Initialize();
            try 
            {
                m_Schemes = new Pullenti.Ner.Core.TerminCollection();
                string obj = Pullenti.Ner.Bank.Internal.ResourceHelper.GetString("UriSchemes.csv");
                if (obj == null) 
                    throw new Exception(string.Format("Can't file resource file {0} in Organization analyzer", "UriSchemes.csv"));
                foreach (string line0 in obj.Split('\n')) 
                {
                    string line = line0.Trim();
                    if (string.IsNullOrEmpty(line)) 
                        continue;
                    m_Schemes.Add(new Pullenti.Ner.Core.Termin(line, Pullenti.Morph.MorphLang.Unknown, true) { Tag = 0 });
                }
                foreach (string s in new string[] {"ISBN", "УДК", "ББК", "ТНВЭД", "ОКВЭД"}) 
                {
                    m_Schemes.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.Unknown, true) { Tag = 1 });
                }
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Общероссийский классификатор форм собственности") { CanonicText = "ОКФС", Tag = 1, Acronym = "ОКФС" });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Общероссийский классификатор организационно правовых форм") { CanonicText = "ОКОПФ", Tag = 1, Acronym = "ОКОПФ" });
                Pullenti.Ner.Core.Termin t;
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("WWW", Pullenti.Morph.MorphLang.Unknown, true) { Tag = 2 });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("HTTP", Pullenti.Morph.MorphLang.Unknown, true) { Tag = 10 });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("HTTPS", Pullenti.Morph.MorphLang.Unknown, true) { Tag = 10 });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("SHTTP", Pullenti.Morph.MorphLang.Unknown, true) { Tag = 10 });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("FTP", Pullenti.Morph.MorphLang.Unknown, true) { Tag = 10 });
                t = new Pullenti.Ner.Core.Termin("SKYPE", Pullenti.Morph.MorphLang.Unknown, true) { Tag = 3 };
                t.AddVariant("СКАЙП", true);
                t.AddVariant("SKYPEID", true);
                t.AddVariant("SKYPE ID", true);
                m_Schemes.Add(t);
                t = new Pullenti.Ner.Core.Termin("SWIFT", Pullenti.Morph.MorphLang.Unknown, true) { Tag = 3 };
                t.AddVariant("СВИФТ", true);
                m_Schemes.Add(t);
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("ICQ", Pullenti.Morph.MorphLang.Unknown, true) { Tag = 4 });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("International Mobile Equipment Identity") { CanonicText = "IMEI", Tag = 5, Acronym = "IMEI", AcronymCanBeLower = true });
                t = new Pullenti.Ner.Core.Termin("основной государственный регистрационный номер") { CanonicText = "ОГРН", Tag = 5, Acronym = "ОГРН", AcronymCanBeLower = true };
                t.AddVariant("ОГРН ИП", true);
                m_Schemes.Add(t);
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Индивидуальный идентификационный номер") { CanonicText = "ИИН", Tag = 5, Acronym = "ИИН", AcronymCanBeLower = true });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Индивидуальный номер налогоплательщика") { CanonicText = "ИНН", Tag = 5, Acronym = "ИНН", AcronymCanBeLower = true });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Код причины постановки на учет") { CanonicText = "КПП", Tag = 5, Acronym = "КПП", AcronymCanBeLower = true });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Банковский идентификационный код") { CanonicText = "БИК", Tag = 5, Acronym = "БИК", AcronymCanBeLower = true });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("основной государственный регистрационный номер индивидуального предпринимателя") { CanonicText = "ОГРНИП", Tag = 5, Acronym = "ОГРНИП", AcronymCanBeLower = true });
                t = new Pullenti.Ner.Core.Termin("Страховой номер индивидуального лицевого счёта") { CanonicText = "СНИЛС", Tag = 5, Acronym = "СНИЛС", AcronymCanBeLower = true };
                t.AddVariant("Свидетельство пенсионного страхования", false);
                t.AddVariant("Страховое свидетельство обязательного пенсионного страхования", false);
                t.AddVariant("Страховое свидетельство", false);
                m_Schemes.Add(t);
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Общероссийский классификатор предприятий и организаций") { CanonicText = "ОКПО", Tag = 5, Acronym = "ОКПО", AcronymCanBeLower = true });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Общероссийский классификатор объектов административно-территориального деления") { CanonicText = "ОКАТО", Tag = 5, Acronym = "ОКАТО", AcronymCanBeLower = true });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Общероссийский классификатор территорий муниципальных образований") { CanonicText = "ОКТМО", Tag = 5, Acronym = "ОКТМО", AcronymCanBeLower = true });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Общероссийский классификатор органов государственной власти и управления") { CanonicText = "ОКОГУ", Tag = 5, Acronym = "ОКОГУ", AcronymCanBeLower = true });
                m_Schemes.Add(new Pullenti.Ner.Core.Termin("Общероссийский классификатор Отрасли народного хозяйства") { CanonicText = "ОКОНХ", Tag = 5, Acronym = "ОКОНХ", AcronymCanBeLower = true });
                t = new Pullenti.Ner.Core.Termin("РАСЧЕТНЫЙ СЧЕТ", Pullenti.Morph.MorphLang.Unknown, true) { CanonicText = "Р/С", Tag = 6, Tag2 = 20 };
                t.AddAbridge("Р.С.");
                t.AddAbridge("Р.СЧ.");
                t.AddAbridge("P.C.");
                t.AddAbridge("РАСЧ.СЧЕТ");
                t.AddAbridge("РАС.СЧЕТ");
                t.AddAbridge("РАСЧ.СЧ.");
                t.AddAbridge("РАС.СЧ.");
                t.AddAbridge("Р.СЧЕТ");
                t.AddVariant("СЧЕТ ПОЛУЧАТЕЛЯ", false);
                t.AddVariant("СЧЕТ ОТПРАВИТЕЛЯ", false);
                t.AddVariant("СЧЕТ", false);
                m_Schemes.Add(t);
                t = new Pullenti.Ner.Core.Termin("ЛИЦЕВОЙ СЧЕТ") { CanonicText = "Л/С", Tag = 6, Tag2 = 20 };
                t.AddAbridge("Л.С.");
                t.AddAbridge("Л.СЧ.");
                t.AddAbridge("Л/С");
                t.AddAbridge("ЛИЦ.СЧЕТ");
                t.AddAbridge("ЛИЦ.СЧ.");
                t.AddAbridge("Л.СЧЕТ");
                m_Schemes.Add(t);
                t = new Pullenti.Ner.Core.Termin("СПЕЦИАЛЬНЫЙ ЛИЦЕВОЙ СЧЕТ", Pullenti.Morph.MorphLang.Unknown, true) { CanonicText = "СПЕЦ/С", Tag = 6, Tag2 = 20 };
                t.AddAbridge("СПЕЦ.С.");
                t.AddAbridge("СПЕЦ.СЧЕТ");
                t.AddAbridge("СПЕЦ.СЧ.");
                t.AddVariant("СПЕЦСЧЕТ", true);
                t.AddVariant("СПЕЦИАЛЬНЫЙ СЧЕТ", true);
                m_Schemes.Add(t);
                t = new Pullenti.Ner.Core.Termin("КОРРЕСПОНДЕНТСКИЙ СЧЕТ", Pullenti.Morph.MorphLang.Unknown, true) { CanonicText = "К/С", Tag = 6, Tag2 = 20 };
                t.AddAbridge("КОРР.СЧЕТ");
                t.AddAbridge("КОР.СЧЕТ");
                t.AddAbridge("КОРР.СЧ.");
                t.AddAbridge("КОР.СЧ.");
                t.AddAbridge("К.СЧЕТ");
                t.AddAbridge("КОР.С.");
                t.AddAbridge("К.С.");
                t.AddAbridge("K.C.");
                t.AddAbridge("К-С");
                t.AddAbridge("К/С");
                t.AddAbridge("К.СЧ.");
                t.AddAbridge("К/СЧ");
                m_Schemes.Add(t);
                t = new Pullenti.Ner.Core.Termin("КОД БЮДЖЕТНОЙ КЛАССИФИКАЦИИ") { CanonicText = "КБК", Acronym = "КБК", Tag = 6, Tag2 = 20, AcronymCanBeLower = true };
                m_Schemes.Add(t);
                Pullenti.Ner.Uri.Internal.UriItemToken.Initialize();
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new UriAnalyzer());
        }
    }
}