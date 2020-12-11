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

namespace Pullenti.Ner.Uri.Internal
{
    class UriItemToken : Pullenti.Ner.MetaToken
    {
        UriItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public string Value;
        public static UriItemToken AttachUriContent(Pullenti.Ner.Token t0, bool afterHttp)
        {
            UriItemToken res = _AttachUriContent(t0, ".;:-_=+&%#@/\\?[]()!~", afterHttp);
            if (res == null) 
                return null;
            if (res.EndToken.IsCharOf(".;-:") && res.EndChar > 3) 
            {
                res.EndToken = res.EndToken.Previous;
                res.Value = res.Value.Substring(0, res.Value.Length - 1);
            }
            if (res.Value.EndsWith("/")) 
                res.Value = res.Value.Substring(0, res.Value.Length - 1);
            if (res.Value.EndsWith("\\")) 
                res.Value = res.Value.Substring(0, res.Value.Length - 1);
            if (res.Value.IndexOf('\\') > 0) 
                res.Value = res.Value.Replace('\\', '/');
            return res;
        }
        public static UriItemToken AttachISOContent(Pullenti.Ner.Token t0, string specChars)
        {
            Pullenti.Ner.Token t = t0;
            while (true) 
            {
                if (t == null) 
                    return null;
                if (t.IsCharOf(":/\\") || t.IsHiphen || t.IsValue("IEC", null)) 
                {
                    t = t.Next;
                    continue;
                }
                break;
            }
            if (!(t is Pullenti.Ner.NumberToken)) 
                return null;
            Pullenti.Ner.Token t1 = t;
            char delim = (char)0;
            StringBuilder txt = new StringBuilder();
            for (; t != null; t = t.Next) 
            {
                if (t.IsWhitespaceBefore && t != t1) 
                    break;
                if (t is Pullenti.Ner.NumberToken) 
                {
                    if (delim != ((char)0)) 
                        txt.Append(delim);
                    delim = (char)0;
                    t1 = t;
                    txt.Append(t.GetSourceText());
                    continue;
                }
                if (!(t is Pullenti.Ner.TextToken)) 
                    break;
                if (!t.IsCharOf(specChars)) 
                    break;
                delim = t.GetSourceText()[0];
            }
            if (txt.Length == 0) 
                return null;
            return new UriItemToken(t0, t1) { Value = txt.ToString() };
        }
        static UriItemToken _AttachUriContent(Pullenti.Ner.Token t0, string chars, bool canBeWhitespaces = false)
        {
            StringBuilder txt = new StringBuilder();
            Pullenti.Ner.Token t1 = t0;
            UriItemToken dom = AttachDomainName(t0, true, canBeWhitespaces);
            if (dom != null) 
            {
                if (dom.Value.Length < 3) 
                    return null;
            }
            char openChar = (char)0;
            Pullenti.Ner.Token t = t0;
            if (dom != null) 
                t = dom.EndToken.Next;
            for (; t != null; t = t.Next) 
            {
                if (t != t0 && t.IsWhitespaceBefore) 
                {
                    if (t.IsNewlineBefore || !canBeWhitespaces) 
                        break;
                    if (dom == null) 
                        break;
                    if (t.Previous.IsHiphen) 
                    {
                    }
                    else if (t.Previous.IsCharOf(",;")) 
                        break;
                    else if (t.Previous.IsChar('.') && t.Chars.IsLetter && t.LengthChar == 2) 
                    {
                    }
                    else 
                    {
                        bool ok = false;
                        Pullenti.Ner.Token tt1 = t;
                        if (t.IsCharOf("\\/")) 
                            tt1 = t.Next;
                        Pullenti.Ner.Token tt0 = tt1;
                        for (; tt1 != null; tt1 = tt1.Next) 
                        {
                            if (tt1 != tt0 && tt1.IsWhitespaceBefore) 
                                break;
                            if (tt1 is Pullenti.Ner.NumberToken) 
                                continue;
                            if (!(tt1 is Pullenti.Ner.TextToken)) 
                                break;
                            string term1 = (tt1 as Pullenti.Ner.TextToken).Term;
                            if (((term1 == "HTM" || term1 == "HTML" || term1 == "SHTML") || term1 == "ASP" || term1 == "ASPX") || term1 == "JSP") 
                            {
                                ok = true;
                                break;
                            }
                            if (!tt1.Chars.IsLetter) 
                            {
                                if (tt1.IsCharOf("\\/")) 
                                {
                                    ok = true;
                                    break;
                                }
                                if (!tt1.IsCharOf(chars)) 
                                    break;
                            }
                            else if (!tt1.Chars.IsLatinLetter) 
                                break;
                        }
                        if (!ok) 
                            break;
                    }
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                    txt.Append(nt.GetSourceText());
                    t1 = t;
                    continue;
                }
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                {
                    Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                    if (rt != null && rt.BeginToken.IsValue("РФ", null)) 
                    {
                        if (txt.Length > 0 && txt[txt.Length - 1] == '.') 
                        {
                            txt.Append(rt.BeginToken.GetSourceText());
                            t1 = t;
                            continue;
                        }
                    }
                    if (rt != null && rt.Chars.IsLatinLetter && rt.BeginToken == rt.EndToken) 
                    {
                        txt.Append(rt.BeginToken.GetSourceText());
                        t1 = t;
                        continue;
                    }
                    break;
                }
                string src = tt.GetSourceText();
                char ch = src[0];
                if (!char.IsLetter(ch)) 
                {
                    if (chars.IndexOf(ch) < 0) 
                        break;
                    if (ch == '(' || ch == '[') 
                        openChar = ch;
                    else if (ch == ')') 
                    {
                        if (openChar != '(') 
                            break;
                        openChar = (char)0;
                    }
                    else if (ch == ']') 
                    {
                        if (openChar != '[') 
                            break;
                        openChar = (char)0;
                    }
                }
                txt.Append(src);
                t1 = t;
            }
            if (txt.Length == 0) 
                return dom;
            int i;
            for (i = 0; i < txt.Length; i++) 
            {
                if (char.IsLetterOrDigit(txt[i])) 
                    break;
            }
            if (i >= txt.Length) 
                return dom;
            if (txt[txt.Length - 1] == '.' || txt[txt.Length - 1] == '/') 
            {
                txt.Length--;
                t1 = t1.Previous;
            }
            if (dom != null) 
                txt.Insert(0, dom.Value);
            string tmp = txt.ToString();
            if (tmp.StartsWith("\\\\")) 
            {
                txt.Replace("\\\\", "//");
                tmp = txt.ToString();
            }
            if (tmp.StartsWith("//")) 
                tmp = tmp.Substring(2);
            if (string.Compare(tmp, "WWW", true) == 0) 
                return null;
            UriItemToken res = new UriItemToken(t0, t1) { Value = txt.ToString() };
            return res;
        }
        public static UriItemToken AttachDomainName(Pullenti.Ner.Token t0, bool check, bool canBeWhitspaces)
        {
            StringBuilder txt = new StringBuilder();
            Pullenti.Ner.Token t1 = t0;
            int ipCount = 0;
            bool isIp = true;
            for (Pullenti.Ner.Token t = t0; t != null; t = t.Next) 
            {
                if (t.IsWhitespaceBefore && t != t0) 
                {
                    bool ok = false;
                    if (!t.IsNewlineBefore && canBeWhitspaces) 
                    {
                        for (Pullenti.Ner.Token tt1 = t; tt1 != null; tt1 = tt1.Next) 
                        {
                            if (tt1.IsChar('.') || tt1.IsHiphen) 
                                continue;
                            if (tt1.IsWhitespaceBefore) 
                            {
                                if (tt1.IsNewlineBefore) 
                                    break;
                                if (tt1.Previous != null && ((tt1.Previous.IsChar('.') || tt1.Previous.IsHiphen))) 
                                {
                                }
                                else 
                                    break;
                            }
                            if (!(tt1 is Pullenti.Ner.TextToken)) 
                                break;
                            if (m_StdGroups.TryParse(tt1, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                            {
                                ok = true;
                                break;
                            }
                            if (!tt1.Chars.IsLatinLetter) 
                                break;
                        }
                    }
                    if (!ok) 
                        break;
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                    if (nt.IntValue == null) 
                        break;
                    txt.Append(nt.GetSourceText());
                    t1 = t;
                    if (nt.Typ == Pullenti.Ner.NumberSpellingType.Digit && nt.IntValue.Value >= 0 && (nt.IntValue.Value < 256)) 
                        ipCount++;
                    else 
                        isIp = false;
                    continue;
                }
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                    break;
                string src = (tt as Pullenti.Ner.TextToken).Term;
                char ch = src[0];
                if (!char.IsLetter(ch)) 
                {
                    if (".-_".IndexOf(ch) < 0) 
                        break;
                    if (ch != '.') 
                        isIp = false;
                    if (ch == '-') 
                    {
                        if (string.Compare(txt.ToString(), "vk.com", true) == 0) 
                            return new UriItemToken(t0, t1) { Value = txt.ToString().ToLower() };
                    }
                }
                else 
                    isIp = false;
                txt.Append(src.ToLower());
                t1 = t;
            }
            if (txt.Length == 0) 
                return null;
            if (ipCount != 4) 
                isIp = false;
            int i;
            int points = 0;
            for (i = 0; i < txt.Length; i++) 
            {
                if (txt[i] == '.') 
                {
                    if (i == 0) 
                        return null;
                    if (i >= (txt.Length - 1)) 
                    {
                        txt.Length--;
                        t1 = t1.Previous;
                        break;
                    }
                    if (txt[i - 1] == '.' || txt[i + 1] == '.') 
                        return null;
                    points++;
                }
            }
            if (points == 0) 
                return null;
            string uri = txt.ToString();
            if (check) 
            {
                bool ok = isIp;
                if (!isIp) 
                {
                    if (txt.ToString() == "localhost") 
                        ok = true;
                }
                if (!ok && t1.Previous != null && t1.Previous.IsChar('.')) 
                {
                    if (m_StdGroups.TryParse(t1, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                        ok = true;
                }
                if (!ok) 
                    return null;
            }
            return new UriItemToken(t0, t1) { Value = txt.ToString().ToLower() };
        }
        public static List<UriItemToken> AttachMailUsers(Pullenti.Ner.Token t1)
        {
            if (t1 == null) 
                return null;
            if (t1.IsChar('}')) 
            {
                List<UriItemToken> res0 = AttachMailUsers(t1.Previous);
                if (res0 == null) 
                    return null;
                t1 = res0[0].BeginToken.Previous;
                for (; t1 != null; t1 = t1.Previous) 
                {
                    if (t1.IsChar('{')) 
                    {
                        res0[0].BeginToken = t1;
                        return res0;
                    }
                    if (t1.IsCharOf(";,")) 
                        continue;
                    List<UriItemToken> res1 = AttachMailUsers(t1);
                    if (res1 == null) 
                        return null;
                    res0.Insert(0, res1[0]);
                    t1 = res1[0].BeginToken;
                }
                return null;
            }
            StringBuilder txt = new StringBuilder();
            Pullenti.Ner.Token t0 = t1;
            for (Pullenti.Ner.Token t = t1; t != null; t = t.Previous) 
            {
                if (t.IsWhitespaceAfter) 
                    break;
                if (t is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                    txt.Insert(0, nt.GetSourceText());
                    t0 = t;
                    continue;
                }
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                    break;
                string src = tt.GetSourceText();
                char ch = src[0];
                if (!char.IsLetter(ch)) 
                {
                    if (".-_".IndexOf(ch) < 0) 
                        break;
                }
                txt.Insert(0, src);
                t0 = t;
            }
            if (txt.Length == 0) 
                return null;
            List<UriItemToken> res = new List<UriItemToken>();
            res.Add(new UriItemToken(t0, t1) { Value = txt.ToString().ToLower() });
            return res;
        }
        public static UriItemToken AttachUrl(Pullenti.Ner.Token t0)
        {
            UriItemToken srv = AttachDomainName(t0, true, false);
            if (srv == null) 
                return null;
            StringBuilder txt = new StringBuilder(srv.Value);
            Pullenti.Ner.Token t1 = srv.EndToken;
            if (t1.Next != null && t1.Next.IsChar(':') && (t1.Next.Next is Pullenti.Ner.NumberToken)) 
            {
                t1 = t1.Next.Next;
                txt.AppendFormat(":{0}", (t1 as Pullenti.Ner.NumberToken).Value);
            }
            else if ((srv.Value == "vk.com" && t1.Next != null && t1.Next.IsHiphen) && t1.Next.Next != null) 
            {
                t1 = t1.Next.Next;
                UriItemToken dat = _AttachUriContent(t1, ".-_+%", false);
                if (dat != null) 
                {
                    t1 = dat.EndToken;
                    txt.AppendFormat("/{0}", dat.Value);
                }
            }
            for (Pullenti.Ner.Token t = t1.Next; t != null; t = t.Next) 
            {
                if (t.IsWhitespaceBefore) 
                    break;
                if (!t.IsChar('/')) 
                    break;
                if (t.IsWhitespaceAfter) 
                {
                    t1 = t;
                    break;
                }
                UriItemToken dat = _AttachUriContent(t.Next, ".-_+%", false);
                if (dat == null) 
                {
                    t1 = t;
                    break;
                }
                t = (t1 = dat.EndToken);
                txt.AppendFormat("/{0}", dat.Value);
            }
            if ((t1.Next != null && t1.Next.IsChar('?') && !t1.Next.IsWhitespaceAfter) && !t1.IsWhitespaceAfter) 
            {
                UriItemToken dat = _AttachUriContent(t1.Next.Next, ".-_+%=&", false);
                if (dat != null) 
                {
                    t1 = dat.EndToken;
                    txt.AppendFormat("?{0}", dat.Value);
                }
            }
            if ((t1.Next != null && t1.Next.IsChar('#') && !t1.Next.IsWhitespaceAfter) && !t1.IsWhitespaceAfter) 
            {
                UriItemToken dat = _AttachUriContent(t1.Next.Next, ".-_+%", false);
                if (dat != null) 
                {
                    t1 = dat.EndToken;
                    txt.AppendFormat("#{0}", dat.Value);
                }
            }
            int i;
            for (i = 0; i < txt.Length; i++) 
            {
                if (char.IsLetter(txt[i])) 
                    break;
            }
            if (i >= txt.Length) 
                return null;
            return new UriItemToken(t0, t1) { Value = txt.ToString() };
        }
        public static UriItemToken AttachISBN(Pullenti.Ner.Token t0)
        {
            StringBuilder txt = new StringBuilder();
            Pullenti.Ner.Token t1 = t0;
            int digs = 0;
            for (Pullenti.Ner.Token t = t0; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar) 
                    break;
                if (t.IsNewlineBefore && t != t0) 
                {
                    if (t.Previous != null && t.Previous.IsHiphen) 
                    {
                    }
                    else 
                        break;
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                    if (nt.Typ != Pullenti.Ner.NumberSpellingType.Digit || !nt.Morph.Class.IsUndefined) 
                        break;
                    string d = nt.GetSourceText();
                    txt.Append(d);
                    digs += d.Length;
                    t1 = t;
                    if (digs > 13) 
                        break;
                    continue;
                }
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                    break;
                string s = tt.Term;
                if (s != "-" && s != "Х" && s != "X") 
                    break;
                if (s == "Х") 
                    s = "X";
                txt.Append(s);
                t1 = t;
                if (s != "-") 
                    break;
            }
            int i;
            int dig = 0;
            for (i = 0; i < txt.Length; i++) 
            {
                if (char.IsDigit(txt[i])) 
                    dig++;
            }
            if (dig < 7) 
                return null;
            return new UriItemToken(t0, t1) { Value = txt.ToString() };
        }
        public static UriItemToken AttachBBK(Pullenti.Ner.Token t0)
        {
            StringBuilder txt = new StringBuilder();
            Pullenti.Ner.Token t1 = t0;
            int digs = 0;
            for (Pullenti.Ner.Token t = t0; t != null; t = t.Next) 
            {
                if (t.IsNewlineBefore && t != t0) 
                    break;
                if (t.IsTableControlChar) 
                    break;
                if (t is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                    if (nt.Typ != Pullenti.Ner.NumberSpellingType.Digit || !nt.Morph.Class.IsUndefined) 
                        break;
                    string d = nt.GetSourceText();
                    txt.Append(d);
                    digs += d.Length;
                    t1 = t;
                    continue;
                }
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                    break;
                if (tt.IsChar(',')) 
                    break;
                if (tt.IsChar('(')) 
                {
                    if (!(tt.Next is Pullenti.Ner.NumberToken)) 
                        break;
                }
                string s = tt.GetSourceText();
                if (char.IsLetter(s[0])) 
                {
                    if (tt.IsWhitespaceBefore) 
                        break;
                }
                txt.Append(s);
                t1 = t;
            }
            if ((txt.Length < 3) || (digs < 2)) 
                return null;
            if (txt[txt.Length - 1] == '.') 
            {
                txt.Length--;
                t1 = t1.Previous;
            }
            return new UriItemToken(t0, t1) { Value = txt.ToString() };
        }
        public static UriItemToken AttachSkype(Pullenti.Ner.Token t0)
        {
            if (t0.Chars.IsCyrillicLetter) 
                return null;
            UriItemToken res = _AttachUriContent(t0, "._", false);
            if (res == null) 
                return null;
            if (res.Value.Length < 5) 
                return null;
            return res;
        }
        public static UriItemToken AttachIcqContent(Pullenti.Ner.Token t0)
        {
            if (!(t0 is Pullenti.Ner.NumberToken)) 
                return null;
            UriItemToken res = AttachISBN(t0);
            if (res == null) 
                return null;
            if (res.Value.Contains("-")) 
                res.Value = res.Value.Replace("-", "");
            foreach (char ch in res.Value) 
            {
                if (!char.IsDigit(ch)) 
                    return null;
            }
            if ((res.Value.Length < 6) || res.Value.Length > 10) 
                return null;
            return res;
        }
        static Pullenti.Ner.Core.TerminCollection m_StdGroups;
        public static void Initialize()
        {
            if (m_StdGroups != null) 
                return;
            m_StdGroups = new Pullenti.Ner.Core.TerminCollection();
            string[] domainGroups = new string[] {"com;net;org;inf;biz;name;aero;arpa;edu;int;gov;mil;coop;museum;mobi;travel", "ac;ad;ae;af;ag;ai;al;am;an;ao;aq;ar;as;at;au;aw;az", "ba;bb;bd;be;bf;bg;bh;bi;bj;bm;bn;bo;br;bs;bt;bv;bw;by;bz", "ca;cc;cd;cf;cg;ch;ci;ck;cl;cm;cn;co;cr;cu;cv;cx;cy;cz", "de;dj;dk;dm;do;dz", "ec;ee;eg;eh;er;es;et;eu", "fi;fj;fk;fm;fo;fr", "ga;gd;ge;gf;gg;gh;gi;gl;gm;gn;gp;gq;gr;gs;gt;gu;gw;gy", "hk;hm;hn;hr;ht;hu", "id;ie;il;im;in;io;iq;ir;is;it", "je;jm;jo;jp", "ke;kg;kh;ki;km;kn;kp;kr;kw;ky;kz", "la;lb;lc;li;lk;lr;ls;lt;lu;lv;ly", "ma;mc;md;mg;mh;mk;ml;mm;mn;mo;mp;mq;mr;ms;mt;mu;mv;mw;mx;my;mz", "na;nc;ne;nf;ng;ni;nl;no;np;nr;nu;nz", "om", "pa;pe;pf;pg;ph;pk;pl;pm;pn;pr;ps;pt;pw;py", "qa", "re;ro;ru;rw", "sa;sb;sc;sd;se;sg;sh;si;sj;sk;sl;sm;sn;so;sr;st;su;sv;sy;sz", "tc;td;tf;tg;th;tj;tk;tm;tn;to;tp;tr;tt;tv;tw;tz", "ua;ug;uk;um;us;uy;uz", "va;vc;ve;vg;vi;vn;vu", "wf;ws", "ye;yt;yu", "za;zm;zw"};
            char[] separator = new char[] {';'};
            foreach (string domainGroup in domainGroups) 
            {
                foreach (string domain in domainGroup.ToUpper().Split(separator, StringSplitOptions.RemoveEmptyEntries)) 
                {
                    m_StdGroups.Add(new Pullenti.Ner.Core.Termin(domain, Pullenti.Morph.MorphLang.Unknown, true));
                }
            }
        }
    }
}