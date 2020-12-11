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
    /// Термин, понятие, система обозначений чего-либо и варианты его написания. Элемент словаря TerminCollection.
    /// </summary>
    public class Termin
    {
        /// <summary>
        /// Создать термин из строки с добавлением всех морфологических вариантов написания
        /// </summary>
        /// <param name="source">строка</param>
        /// <param name="lang">возможный язык (null, если совпадает с текущим языком анализируемого текста)</param>
        /// <param name="sourceIsNormal">при true морфварианты не добавляются 
        /// (эквивалентно вызову InitByNormalText)</param>
        public Termin(string source = null, Pullenti.Morph.MorphLang lang = null, bool sourceIsNormal = false)
        {
            if (source == null) 
                return;
            if (sourceIsNormal || AssignAllTextsAsNormal) 
            {
                this.InitByNormalText(source, lang);
                return;
            }
            List<Pullenti.Morph.MorphToken> toks = Pullenti.Morph.MorphologyService.Process(source, lang, null);
            if (toks != null) 
            {
                for (int i = 0; i < toks.Count; i++) 
                {
                    Pullenti.Ner.TextToken tt = new Pullenti.Ner.TextToken(toks[i], null);
                    Terms.Add(new Term(tt, !sourceIsNormal));
                }
            }
            Lang = new Pullenti.Morph.MorphLang();
            if (lang != null) 
                Lang.Value = lang.Value;
        }
        // Используется внутренним образом (для ускорения Питона)
        public static bool AssignAllTextsAsNormal = false;
        /// <summary>
        /// Быстрая инициализация без морф.вариантов, производится только 
        /// токенизация текста. Используется для ускорения работы со словарём в случае, 
        /// когда изначально известно, что на входе уже нормализованные строки.
        /// </summary>
        /// <param name="text">исходно нормализованный текст</param>
        /// <param name="lang">возможный язык (можно null)</param>
        public void InitByNormalText(string text, Pullenti.Morph.MorphLang lang = null)
        {
            if (string.IsNullOrEmpty(text)) 
                return;
            text = text.ToUpper();
            if (text.IndexOf('\'') >= 0) 
                text = text.Replace("'", "");
            bool tok = false;
            bool sp = false;
            foreach (char ch in text) 
            {
                if (!char.IsLetter(ch)) 
                {
                    if (ch == ' ') 
                        sp = true;
                    else 
                    {
                        tok = true;
                        break;
                    }
                }
            }
            if (!tok && !sp) 
            {
                Pullenti.Ner.TextToken tt = new Pullenti.Ner.TextToken(null, null);
                tt.Term = text;
                Terms.Add(new Term(tt, false));
            }
            else if (!tok && sp) 
            {
                string[] wrds = text.Split(' ');
                for (int i = 0; i < wrds.Length; i++) 
                {
                    if (string.IsNullOrEmpty(wrds[i])) 
                        continue;
                    Pullenti.Ner.TextToken tt = new Pullenti.Ner.TextToken(null, null);
                    tt.Term = wrds[i];
                    Terms.Add(new Term(tt, false));
                }
            }
            else 
            {
                List<Pullenti.Morph.MorphToken> toks = Pullenti.Morph.MorphologyService.Tokenize(text);
                if (toks != null) 
                {
                    for (int i = 0; i < toks.Count; i++) 
                    {
                        Pullenti.Ner.TextToken tt = new Pullenti.Ner.TextToken(toks[i], null);
                        Terms.Add(new Term(tt, false));
                    }
                }
            }
            Lang = new Pullenti.Morph.MorphLang();
            if (lang != null) 
                Lang.Value = lang.Value;
        }
        public void InitBy(Pullenti.Ner.Token begin, Pullenti.Ner.Token end, object tag = null, bool addLemmaVariant = false)
        {
            if (tag != null) 
                Tag = tag;
            for (Pullenti.Ner.Token t = begin; t != null; t = t.Next) 
            {
                if (Lang.IsUndefined && !t.Morph.Language.IsUndefined) 
                    Lang = t.Morph.Language;
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt != null) 
                    Terms.Add(new Term(tt, addLemmaVariant));
                else if (t is Pullenti.Ner.NumberToken) 
                    Terms.Add(new Term(null, false, (t as Pullenti.Ner.NumberToken).Value));
                if (t == end) 
                    break;
            }
        }
        // Морфологические токены полного написания - список Term.
        public List<Term> Terms = new List<Term>();
        /// <summary>
        /// Дополнительные варианты (список Termin, обычно null)
        /// </summary>
        public List<Termin> AdditionalVars = null;
        /// <summary>
        /// Добавить дополнительный вариант полного написания
        /// </summary>
        /// <param name="var">строка варианта</param>
        /// <param name="sourceIsNormal">при true морфварианты не добавляются, иначе добавляются</param>
        public void AddVariant(string var, bool sourceIsNormal = false)
        {
            if (AdditionalVars == null) 
                AdditionalVars = new List<Termin>();
            AdditionalVars.Add(new Termin(var, Pullenti.Morph.MorphLang.Unknown, sourceIsNormal));
        }
        /// <summary>
        /// Добавить дополнительный вариант написания
        /// </summary>
        /// <param name="t">термин</param>
        public void AddVariantTerm(Termin t)
        {
            if (AdditionalVars == null) 
                AdditionalVars = new List<Termin>();
            AdditionalVars.Add(t);
        }
        // Элемент термина (слово или число)
        public class Term
        {
            public Term(Pullenti.Ner.TextToken src, bool addLemmaVariant = false, string number = null)
            {
                m_Source = src;
                if (src != null) 
                {
                    Variants.Add(src.Term);
                    if (src.Term.Length > 0 && char.IsDigit(src.Term[0])) 
                    {
                        Pullenti.Ner.NumberToken nt = new Pullenti.Ner.NumberToken(src, src, src.Term, Pullenti.Ner.NumberSpellingType.Digit);
                        m_Number = nt.Value;
                        m_Source = null;
                        return;
                    }
                    if (addLemmaVariant) 
                    {
                        string lemma = src.Lemma;
                        if (lemma != null && lemma != src.Term) 
                            Variants.Add(lemma);
                        foreach (Pullenti.Morph.MorphBaseInfo wff in src.Morph.Items) 
                        {
                            Pullenti.Morph.MorphWordForm wf = wff as Pullenti.Morph.MorphWordForm;
                            if (wf != null && wf.IsInDictionary) 
                            {
                                string s = wf.NormalFull ?? wf.NormalCase;
                                if (s != lemma && s != src.Term) 
                                    Variants.Add(s);
                            }
                        }
                    }
                }
                if (number != null) 
                {
                    m_Number = number;
                    Variants.Add(number);
                }
            }
            Pullenti.Ner.TextToken m_Source;
            public bool IsPatternAny;
            string m_Number;
            public ICollection<string> Variants
            {
                get
                {
                    return m_Variants;
                }
            }
            List<string> m_Variants = new List<string>();
            public string CanonicalText
            {
                get
                {
                    return (m_Variants.Count > 0 ? m_Variants[0] : "?");
                }
            }
            public override string ToString()
            {
                if (IsPatternAny) 
                    return "IsPatternAny";
                StringBuilder res = new StringBuilder();
                foreach (string v in Variants) 
                {
                    if (res.Length > 0) 
                        res.Append(", ");
                    res.Append(v);
                }
                return res.ToString();
            }
            public bool IsNumber
            {
                get
                {
                    return m_Source == null || m_Number != null;
                }
            }
            public bool IsHiphen
            {
                get
                {
                    return m_Source != null && m_Source.Term == "-";
                }
            }
            public bool IsPoint
            {
                get
                {
                    return m_Source != null && m_Source.Term == ".";
                }
            }
            public virtual Pullenti.Morph.MorphGender Gender
            {
                get
                {
                    if (m_Gender != Pullenti.Morph.MorphGender.Undefined) 
                        return m_Gender;
                    Pullenti.Morph.MorphGender res = Pullenti.Morph.MorphGender.Undefined;
                    if (m_Source != null) 
                    {
                        foreach (Pullenti.Morph.MorphBaseInfo wf in m_Source.Morph.Items) 
                        {
                            if ((wf is Pullenti.Morph.MorphWordForm) && (wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                                res |= wf.Gender;
                        }
                    }
                    return res;
                }
                set
                {
                    m_Gender = value;
                    if (m_Source != null) 
                    {
                        for (int i = m_Source.Morph.ItemsCount - 1; i >= 0; i--) 
                        {
                            if (((m_Source.Morph[i].Gender & value)) == Pullenti.Morph.MorphGender.Undefined) 
                                m_Source.Morph.RemoveItem(i);
                        }
                    }
                }
            }
            Pullenti.Morph.MorphGender m_Gender = Pullenti.Morph.MorphGender.Undefined;
            internal bool IsNoun
            {
                get
                {
                    if (m_Source != null) 
                    {
                        foreach (Pullenti.Morph.MorphBaseInfo wf in m_Source.Morph.Items) 
                        {
                            if (wf.Class.IsNoun) 
                                return true;
                        }
                    }
                    return false;
                }
            }
            internal bool IsAdjective
            {
                get
                {
                    if (m_Source != null) 
                    {
                        foreach (Pullenti.Morph.MorphBaseInfo wf in m_Source.Morph.Items) 
                        {
                            if (wf.Class.IsAdjective) 
                                return true;
                        }
                    }
                    return false;
                }
            }
            public IEnumerable<Pullenti.Morph.MorphWordForm> MorphWordForms
            {
                get
                {
                    List<Pullenti.Morph.MorphWordForm> res = new List<Pullenti.Morph.MorphWordForm>();
                    if (m_Source != null) 
                    {
                        foreach (Pullenti.Morph.MorphBaseInfo wf in m_Source.Morph.Items) 
                        {
                            if (wf is Pullenti.Morph.MorphWordForm) 
                                res.Add(wf as Pullenti.Morph.MorphWordForm);
                        }
                    }
                    return res;
                }
            }
            public bool CheckByTerm(Term t)
            {
                if (IsNumber) 
                    return m_Number == t.m_Number;
                if (m_Variants != null && t.m_Variants != null) 
                {
                    foreach (string v in m_Variants) 
                    {
                        if (t.m_Variants.Contains(v)) 
                            return true;
                    }
                }
                return false;
            }
            public bool CheckByToken(Pullenti.Ner.Token t)
            {
                return this._check(t, 0);
            }
            bool _check(Pullenti.Ner.Token t, int lev)
            {
                if (lev > 10) 
                    return false;
                if (IsPatternAny) 
                    return true;
                if (t is Pullenti.Ner.TextToken) 
                {
                    if (IsNumber) 
                        return false;
                    foreach (string v in Variants) 
                    {
                        if (t.IsValue(v, null)) 
                            return true;
                    }
                    return false;
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    if (IsNumber) 
                        return m_Number == (t as Pullenti.Ner.NumberToken).Value;
                    Pullenti.Ner.NumberToken num = t as Pullenti.Ner.NumberToken;
                    if (num.BeginToken == num.EndToken) 
                        return this._check(num.BeginToken, lev);
                    return false;
                }
                if (t is Pullenti.Ner.MetaToken) 
                {
                    Pullenti.Ner.MetaToken mt = t as Pullenti.Ner.MetaToken;
                    if (mt.BeginToken == mt.EndToken) 
                    {
                        if (this._check(mt.BeginToken, lev + 1)) 
                            return true;
                    }
                }
                return false;
            }
            public bool CheckByPrefToken(Term prefix, Pullenti.Ner.TextToken t)
            {
                if (prefix == null || prefix.m_Source == null || t == null) 
                    return false;
                string pref = prefix.CanonicalText;
                string tterm = t.Term;
                if (pref[0] != tterm[0]) 
                    return false;
                if (!tterm.StartsWith(pref)) 
                    return false;
                foreach (string v in Variants) 
                {
                    if (t.IsValue(pref + v, null)) 
                        return true;
                }
                return false;
            }
            public bool CheckByStrPrefToken(string pref, Pullenti.Ner.TextToken t)
            {
                if (pref == null || t == null) 
                    return false;
                foreach (string v in Variants) 
                {
                    if (v.StartsWith(pref) && v.Length > pref.Length) 
                    {
                        if (t.IsValue(v.Substring(pref.Length), null)) 
                            return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Канонический текст термина. Если явно не задан, то создаётся автоматически.
        /// </summary>
        public string CanonicText
        {
            get
            {
                if (m_CanonicText != null) 
                    return m_CanonicText;
                if (Terms.Count > 0) 
                {
                    StringBuilder tmp = new StringBuilder();
                    foreach (Term v in Terms) 
                    {
                        if (tmp.Length > 0) 
                            tmp.Append(' ');
                        tmp.Append(v.CanonicalText);
                    }
                    m_CanonicText = tmp.ToString();
                }
                else if (Acronym != null) 
                    m_CanonicText = Acronym;
                return m_CanonicText ?? "?";
            }
            set
            {
                m_CanonicText = value;
            }
        }
        string m_CanonicText;
        /// <summary>
        /// Порядок токенов неважен (то есть привязка с точностью до перестановок)
        /// </summary>
        public bool IgnoreTermsOrder;
        /// <summary>
        /// Возможная аббревиатура (всегда слитно в верхнем регистре)
        /// </summary>
        public string Acronym;
        /// <summary>
        /// "Мягкая" аббревиатура, допускающая разбивку, точки и т.п.
        /// </summary>
        public string AcronymSmart;
        /// <summary>
        /// Аббревиатура м.б. в нижнем регистре
        /// </summary>
        public bool AcronymCanBeLower;
        /// <summary>
        /// Установить стандартную аббревиатуру
        /// </summary>
        public void SetStdAcronim(bool smart)
        {
            StringBuilder acr = new StringBuilder();
            foreach (Term t in Terms) 
            {
                string s = t.CanonicalText;
                if (string.IsNullOrEmpty(s)) 
                    continue;
                if (s.Length > 2) 
                    acr.Append(s[0]);
            }
            if (acr.Length > 1) 
            {
                if (smart) 
                    AcronymSmart = acr.ToString();
                else 
                    Acronym = acr.ToString();
            }
        }
        // Список возможных сокращений Abridge
        public List<Abridge> Abridges;
        public class Abridge
        {
            public List<Pullenti.Ner.Core.Termin.AbridgePart> Parts = new List<Pullenti.Ner.Core.Termin.AbridgePart>();
            public void AddPart(string val, bool hasDelim = false)
            {
                Parts.Add(new Pullenti.Ner.Core.Termin.AbridgePart() { Value = val, HasDelim = hasDelim });
            }
            public string Tail;
            public override string ToString()
            {
                if (Tail != null) 
                    return string.Format("{0}-{1}", Parts[0], Tail);
                StringBuilder res = new StringBuilder();
                foreach (Pullenti.Ner.Core.Termin.AbridgePart p in Parts) 
                {
                    res.Append(p);
                }
                return res.ToString();
            }
            public Pullenti.Ner.Core.TerminToken TryAttach(Pullenti.Ner.Token t0)
            {
                Pullenti.Ner.TextToken t1 = t0 as Pullenti.Ner.TextToken;
                if (t1 == null) 
                    return null;
                if (t1.Term != Parts[0].Value) 
                {
                    if (Parts.Count != 1 || !t1.IsValue(Parts[0].Value, null)) 
                        return null;
                }
                if (Tail == null) 
                {
                    Pullenti.Ner.Token te = (Pullenti.Ner.Token)t1;
                    bool point = false;
                    if (te.Next != null) 
                    {
                        if (te.Next.IsChar('.')) 
                        {
                            te = te.Next;
                            point = true;
                        }
                        else if (Parts.Count > 1) 
                        {
                            while (te.Next != null) 
                            {
                                if (te.Next.IsCharOf("\\/.") || te.Next.IsHiphen) 
                                {
                                    te = te.Next;
                                    point = true;
                                }
                                else 
                                    break;
                            }
                        }
                    }
                    if (te == null) 
                        return null;
                    Pullenti.Ner.Token tt = te.Next;
                    for (int i = 1; i < Parts.Count; i++) 
                    {
                        if (tt != null && tt.WhitespacesBeforeCount > 2) 
                            return null;
                        if (tt != null && ((tt.IsHiphen || tt.IsCharOf("\\/.")))) 
                            tt = tt.Next;
                        else if (!point && Parts[i - 1].HasDelim) 
                            return null;
                        if (tt == null) 
                            return null;
                        if (tt is Pullenti.Ner.TextToken) 
                        {
                            Pullenti.Ner.TextToken tet = tt as Pullenti.Ner.TextToken;
                            if (tet.Term != Parts[i].Value) 
                            {
                                if (!tet.IsValue(Parts[i].Value, null)) 
                                    return null;
                            }
                        }
                        else if (tt is Pullenti.Ner.MetaToken) 
                        {
                            Pullenti.Ner.MetaToken mt = tt as Pullenti.Ner.MetaToken;
                            if (mt.BeginToken != mt.EndToken) 
                                return null;
                            if (!mt.BeginToken.IsValue(Parts[i].Value, null)) 
                                return null;
                        }
                        te = tt;
                        if (tt.Next != null && ((tt.Next.IsCharOf(".\\/") || tt.Next.IsHiphen))) 
                        {
                            tt = tt.Next;
                            point = true;
                            if (tt != null) 
                                te = tt;
                        }
                        else 
                            point = false;
                        tt = tt.Next;
                    }
                    Pullenti.Ner.Core.TerminToken res = new Pullenti.Ner.Core.TerminToken(t0, te) { AbridgeWithoutPoint = t0 == te };
                    if (point) 
                        res.Morph = new Pullenti.Ner.MorphCollection();
                    return res;
                }
                t1 = t1.Next as Pullenti.Ner.TextToken;
                if (t1 == null || !t1.IsCharOf("-\\/")) 
                    return null;
                t1 = t1.Next as Pullenti.Ner.TextToken;
                if (t1 == null) 
                    return null;
                if (t1.Term[0] != Tail[0]) 
                    return null;
                return new Pullenti.Ner.Core.TerminToken(t0, t1);
            }
        }

        public class AbridgePart
        {
            public string Value;
            public bool HasDelim;
            public override string ToString()
            {
                if (HasDelim) 
                    return Value + ".";
                else 
                    return Value;
            }
        }

        /// <summary>
        /// Добавить сокращение в термин
        /// </summary>
        /// <param name="abr">сокращение, например, "нас.п." или "д-р наук"</param>
        /// <return>разобранное сокращение, добавленное в термин</return>
        public Abridge AddAbridge(string abr)
        {
            if (abr == "В/ГОР") 
            {
            }
            Abridge a = new Abridge();
            if (Abridges == null) 
                Abridges = new List<Abridge>();
            int i;
            for (i = 0; i < abr.Length; i++) 
            {
                if (!char.IsLetter(abr[i])) 
                    break;
            }
            if (i == 0) 
                return null;
            a.Parts.Add(new AbridgePart() { Value = abr.Substring(0, i).ToUpper() });
            Abridges.Add(a);
            if (((i + 1) < abr.Length) && abr[i] == '-') 
                a.Tail = abr.Substring(i + 1).ToUpper();
            else if (i < abr.Length) 
            {
                if (!char.IsWhiteSpace(abr[i])) 
                    a.Parts[0].HasDelim = true;
                for (; i < abr.Length; i++) 
                {
                    if (char.IsLetter(abr[i])) 
                    {
                        int j;
                        for (j = i + 1; j < abr.Length; j++) 
                        {
                            if (!char.IsLetter(abr[j])) 
                                break;
                        }
                        AbridgePart p = new AbridgePart() { Value = abr.Substring(i, j - i).ToUpper() };
                        if (j < abr.Length) 
                        {
                            if (!char.IsWhiteSpace(abr[j])) 
                                p.HasDelim = true;
                        }
                        a.Parts.Add(p);
                        i = j;
                    }
                }
            }
            return a;
        }
        /// <summary>
        /// Язык
        /// </summary>
        public Pullenti.Morph.MorphLang Lang = new Pullenti.Morph.MorphLang();
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag;
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag2;
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag3;
        /// <summary>
        /// Род (вычисляется по первому слову термина)
        /// </summary>
        public Pullenti.Morph.MorphGender Gender
        {
            get
            {
                if (Terms.Count > 0) 
                {
                    if (Terms.Count > 0 && Terms[0].IsAdjective && Terms[Terms.Count - 1].IsNoun) 
                        return Terms[Terms.Count - 1].Gender;
                    return Terms[0].Gender;
                }
                else 
                    return Pullenti.Morph.MorphGender.Undefined;
            }
            set
            {
                if (Terms.Count > 0) 
                    Terms[0].Gender = value;
            }
        }
        public void CopyTo(Termin dst)
        {
            dst.Terms = Terms;
            dst.IgnoreTermsOrder = IgnoreTermsOrder;
            dst.Acronym = Acronym;
            dst.Abridges = Abridges;
            dst.Lang = Lang;
            dst.m_CanonicText = m_CanonicText;
        }
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Terms.Count > 0) 
            {
                for (int i = 0; i < Terms.Count; i++) 
                {
                    if (i > 0) 
                        res.Append(' ');
                    res.Append(Terms[i].CanonicalText);
                }
            }
            if (Acronym != null) 
            {
                if (res.Length > 0) 
                    res.Append(", ");
                res.Append(Acronym);
            }
            if (AcronymSmart != null) 
            {
                if (res.Length > 0) 
                    res.Append(", ");
                res.Append(AcronymSmart);
            }
            if (Abridges != null) 
            {
                foreach (Abridge a in Abridges) 
                {
                    if (res.Length > 0) 
                        res.Append(", ");
                    res.Append(a);
                }
            }
            return res.ToString();
        }
        static string[] m_StdAbridePrefixes = new string[] {"НИЖ", "ВЕРХ", "МАЛ", "БОЛЬШ", "НОВ", "СТАР"};
        public void AddStdAbridges()
        {
            if (Terms.Count != 2) 
                return;
            string first = Terms[0].CanonicalText;
            int i;
            for (i = 0; i < m_StdAbridePrefixes.Length; i++) 
            {
                if (first.StartsWith(m_StdAbridePrefixes[i])) 
                    break;
            }
            if (i >= m_StdAbridePrefixes.Length) 
                return;
            string head = m_StdAbridePrefixes[i];
            string second = Terms[1].CanonicalText;
            for (i = 0; i < head.Length; i++) 
            {
                if (!Pullenti.Morph.LanguageHelper.IsCyrillicVowel(head[i])) 
                {
                    Abridge a = new Abridge();
                    a.AddPart(head.Substring(0, i + 1), false);
                    a.AddPart(second, false);
                    if (Abridges == null) 
                        Abridges = new List<Abridge>();
                    Abridges.Add(a);
                }
            }
        }
        /// <summary>
        /// Добавить все сокращения (с первой буквы до любого согласного)
        /// </summary>
        public void AddAllAbridges(int tailLen = 0, int maxFirstLen = 0, int minFirstLen = 0)
        {
            if (Terms.Count < 1) 
                return;
            string txt = Terms[0].CanonicalText;
            if (tailLen == 0) 
            {
                for (int i = txt.Length - 2; i >= 0; i--) 
                {
                    if (!Pullenti.Morph.LanguageHelper.IsCyrillicVowel(txt[i])) 
                    {
                        if (minFirstLen > 0 && (i < (minFirstLen - 1))) 
                            break;
                        Abridge a = new Abridge();
                        a.AddPart(txt.Substring(0, i + 1), false);
                        for (int j = 1; j < Terms.Count; j++) 
                        {
                            a.AddPart(Terms[j].CanonicalText, false);
                        }
                        if (Abridges == null) 
                            Abridges = new List<Abridge>();
                        Abridges.Add(a);
                    }
                }
            }
            else 
            {
                string tail = txt.Substring(txt.Length - tailLen);
                txt = txt.Substring(0, txt.Length - tailLen - 1);
                for (int i = txt.Length - 2; i >= 0; i--) 
                {
                    if (maxFirstLen > 0 && i >= maxFirstLen) 
                    {
                    }
                    else if (!Pullenti.Morph.LanguageHelper.IsCyrillicVowel(txt[i])) 
                        this.AddAbridge(string.Format("{0}-{1}", txt.Substring(0, i + 1), tail));
                }
            }
        }
        internal List<string> GetHashVariants()
        {
            List<string> res = new List<string>();
            for (int j = 0; j < Terms.Count; j++) 
            {
                foreach (string v in Terms[j].Variants) 
                {
                    if (!res.Contains(v)) 
                        res.Add(v);
                }
                if (((j + 2) < Terms.Count) && Terms[j + 1].IsHiphen) 
                {
                    string pref = Terms[j].CanonicalText;
                    foreach (string v in Terms[j + 2].Variants) 
                    {
                        if (!res.Contains(pref + v)) 
                            res.Add(pref + v);
                    }
                }
                if (!IgnoreTermsOrder) 
                    break;
            }
            if (Acronym != null) 
            {
                if (!res.Contains(Acronym)) 
                    res.Add(Acronym);
            }
            if (AcronymSmart != null) 
            {
                if (!res.Contains(AcronymSmart)) 
                    res.Add(AcronymSmart);
            }
            if (Abridges != null) 
            {
                foreach (Abridge a in Abridges) 
                {
                    if (a.Parts[0].Value.Length > 1) 
                    {
                        if (!res.Contains(a.Parts[0].Value)) 
                            res.Add(a.Parts[0].Value);
                    }
                }
            }
            return res;
        }
        public bool IsEqual(Termin t)
        {
            if (t.Acronym != null) 
            {
                if (Acronym == t.Acronym || AcronymSmart == t.Acronym) 
                    return true;
            }
            if (t.AcronymSmart != null) 
            {
                if (Acronym == t.AcronymSmart || AcronymSmart == t.AcronymSmart) 
                    return true;
            }
            if (t.Terms.Count != Terms.Count) 
                return false;
            for (int i = 0; i < Terms.Count; i++) 
            {
                if (!Terms[i].CheckByTerm(t.Terms[i])) 
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Попробовать привязать термин
        /// </summary>
        /// <param name="t0">начальный токен</param>
        /// <param name="pars">дополнительные параметры привязки</param>
        /// <return>метатокен привязки или null</return>
        public TerminToken TryParse(Pullenti.Ner.Token t0, TerminParseAttr pars = TerminParseAttr.No)
        {
            if (t0 == null) 
                return null;
            string term = null;
            if (t0 is Pullenti.Ner.TextToken) 
                term = (t0 as Pullenti.Ner.TextToken).Term;
            if (AcronymSmart != null && ((pars & TerminParseAttr.FullwordsOnly)) == TerminParseAttr.No && term != null) 
            {
                if (AcronymSmart == term) 
                {
                    if (t0.Next != null && t0.Next.IsChar('.') && !t0.IsWhitespaceAfter) 
                        return new TerminToken(t0, t0.Next) { Termin = this };
                    else 
                        return new TerminToken(t0, t0) { Termin = this };
                }
                int i;
                Pullenti.Ner.TextToken t1 = t0 as Pullenti.Ner.TextToken;
                Pullenti.Ner.TextToken tt = t0 as Pullenti.Ner.TextToken;
                for (i = 0; i < Acronym.Length; i++) 
                {
                    if (tt == null) 
                        break;
                    string term1 = tt.Term;
                    if (term1.Length != 1 || tt.IsWhitespaceAfter) 
                        break;
                    if (i > 0 && tt.IsWhitespaceBefore) 
                        break;
                    if (term1[0] != Acronym[i]) 
                        break;
                    if (tt.Next == null || !tt.Next.IsChar('.')) 
                        break;
                    t1 = tt.Next as Pullenti.Ner.TextToken;
                    tt = tt.Next.Next as Pullenti.Ner.TextToken;
                }
                if (i >= Acronym.Length) 
                    return new TerminToken(t0, t1) { Termin = this };
            }
            if (Acronym != null && term != null && Acronym == term) 
            {
                if (t0.Chars.IsAllUpper || AcronymCanBeLower || ((!t0.Chars.IsAllLower && term.Length >= 3))) 
                    return new TerminToken(t0, t0) { Termin = this };
            }
            if (Acronym != null && t0.Chars.IsLastLower && t0.LengthChar > 3) 
            {
                if (t0.IsValue(Acronym, null)) 
                    return new TerminToken(t0, t0) { Termin = this };
            }
            int cou = 0;
            for (int i = 0; i < Terms.Count; i++) 
            {
                if (Terms[i].IsHiphen) 
                    cou--;
                else 
                    cou++;
            }
            if (Terms.Count > 0 && ((!IgnoreTermsOrder || cou == 1))) 
            {
                Pullenti.Ner.Token t1 = t0;
                Pullenti.Ner.Token tt = t0;
                Pullenti.Ner.Token e = null;
                Pullenti.Ner.Token eUp = null;
                bool ok = true;
                Pullenti.Ner.MorphCollection mc = null;
                bool dontChangeMc = false;
                int i;
                for (i = 0; i < Terms.Count; i++) 
                {
                    if (Terms[i].IsHiphen) 
                        continue;
                    if (tt != null && tt.IsHiphen && i > 0) 
                        tt = tt.Next;
                    if (i > 0 && tt != null) 
                    {
                        if (((pars & TerminParseAttr.IgnoreBrackets)) != TerminParseAttr.No && !tt.Chars.IsLetter && BracketHelper.IsBracket(tt, false)) 
                            tt = tt.Next;
                    }
                    if ((((pars & TerminParseAttr.CanBeGeoObject)) != TerminParseAttr.No && i > 0 && (tt is Pullenti.Ner.ReferentToken)) && tt.GetReferent().TypeName == "GEO") 
                        tt = tt.Next;
                    if ((tt is Pullenti.Ner.ReferentToken) && e == null) 
                    {
                        eUp = tt;
                        e = (tt as Pullenti.Ner.ReferentToken).EndToken;
                        tt = (tt as Pullenti.Ner.ReferentToken).BeginToken;
                    }
                    if (tt == null) 
                    {
                        ok = false;
                        break;
                    }
                    if (!Terms[i].CheckByToken(tt)) 
                    {
                        if (tt.Next != null && tt.IsCharOf(".,") && Terms[i].CheckByToken(tt.Next)) 
                            tt = tt.Next;
                        else if (((i > 0 && tt.Next != null && (tt is Pullenti.Ner.TextToken)) && ((tt.Morph.Class.IsPreposition || MiscHelper.IsEngArticle(tt))) && Terms[i].CheckByToken(tt.Next)) && !Terms[i - 1].IsPatternAny) 
                            tt = tt.Next;
                        else 
                        {
                            ok = false;
                            if (((i + 2) < Terms.Count) && Terms[i + 1].IsHiphen && Terms[i + 2].CheckByPrefToken(Terms[i], tt as Pullenti.Ner.TextToken)) 
                            {
                                i += 2;
                                ok = true;
                            }
                            else if (((!tt.IsWhitespaceAfter && tt.Next != null && (tt is Pullenti.Ner.TextToken)) && (tt as Pullenti.Ner.TextToken).LengthChar == 1 && tt.Next.IsCharOf("\"'`’“”")) && !tt.Next.IsWhitespaceAfter && (tt.Next.Next is Pullenti.Ner.TextToken)) 
                            {
                                if (Terms[i].CheckByStrPrefToken((tt as Pullenti.Ner.TextToken).Term, tt.Next.Next as Pullenti.Ner.TextToken)) 
                                {
                                    ok = true;
                                    tt = tt.Next.Next;
                                }
                            }
                            if (!ok) 
                            {
                                if (i > 0 && ((pars & TerminParseAttr.IgnoreStopWords)) != TerminParseAttr.No) 
                                {
                                    if (tt is Pullenti.Ner.TextToken) 
                                    {
                                        if (!tt.Chars.IsLetter) 
                                        {
                                            tt = tt.Next;
                                            i--;
                                            continue;
                                        }
                                        Pullenti.Morph.MorphClass mc1 = tt.GetMorphClassInDictionary();
                                        if (mc1.IsConjunction || mc1.IsPreposition) 
                                        {
                                            tt = tt.Next;
                                            i--;
                                            continue;
                                        }
                                    }
                                    if (tt is Pullenti.Ner.NumberToken) 
                                    {
                                        tt = tt.Next;
                                        i--;
                                        continue;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    if (tt.Morph.ItemsCount > 0 && !dontChangeMc) 
                    {
                        mc = new Pullenti.Ner.MorphCollection(tt.Morph);
                        if (((mc.Class.IsNoun || mc.Class.IsVerb)) && !mc.Class.IsAdjective) 
                        {
                            if (((i + 1) < Terms.Count) && Terms[i + 1].IsHiphen) 
                            {
                            }
                            else 
                                dontChangeMc = true;
                        }
                    }
                    if (tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction) 
                        dontChangeMc = true;
                    if (tt == e) 
                    {
                        tt = eUp;
                        eUp = null;
                        e = null;
                    }
                    if (e == null) 
                        t1 = tt;
                    tt = tt.Next;
                }
                if (ok && i >= Terms.Count) 
                {
                    if (t1.Next != null && t1.Next.IsChar('.') && Abridges != null) 
                    {
                        foreach (Abridge a in Abridges) 
                        {
                            if (a.TryAttach(t0) != null) 
                            {
                                t1 = t1.Next;
                                break;
                            }
                        }
                    }
                    if (t0 != t1 && t0.Morph.Class.IsAdjective) 
                    {
                        NounPhraseToken npt = NounPhraseHelper.TryParse(t0, NounPhraseParseAttr.No, 0, null);
                        if (npt != null && npt.EndChar <= t1.EndChar) 
                            mc = npt.Morph;
                    }
                    return new TerminToken(t0, t1) { Morph = mc };
                }
            }
            if (Terms.Count > 1 && IgnoreTermsOrder) 
            {
                List<Term> terms = new List<Term>(Terms);
                Pullenti.Ner.Token t1 = t0;
                Pullenti.Ner.Token tt = t0;
                while (terms.Count > 0) 
                {
                    if (tt != t0 && tt != null && tt.IsHiphen) 
                        tt = tt.Next;
                    if (tt == null) 
                        break;
                    int j;
                    for (j = 0; j < terms.Count; j++) 
                    {
                        if (terms[j].CheckByToken(tt)) 
                            break;
                    }
                    if (j >= terms.Count) 
                    {
                        if (tt != t0 && ((pars & TerminParseAttr.IgnoreStopWords)) != TerminParseAttr.No) 
                        {
                            if (tt is Pullenti.Ner.TextToken) 
                            {
                                if (!tt.Chars.IsLetter) 
                                {
                                    tt = tt.Next;
                                    continue;
                                }
                                Pullenti.Morph.MorphClass mc1 = tt.GetMorphClassInDictionary();
                                if (mc1.IsConjunction || mc1.IsPreposition) 
                                {
                                    tt = tt.Next;
                                    continue;
                                }
                            }
                            if (tt is Pullenti.Ner.NumberToken) 
                            {
                                tt = tt.Next;
                                continue;
                            }
                        }
                        break;
                    }
                    terms.RemoveAt(j);
                    t1 = tt;
                    tt = tt.Next;
                }
                for (int i = terms.Count - 1; i >= 0; i--) 
                {
                    if (terms[i].IsHiphen) 
                        terms.RemoveAt(i);
                }
                if (terms.Count == 0) 
                    return new TerminToken(t0, t1);
            }
            if (Abridges != null && ((pars & TerminParseAttr.FullwordsOnly)) == TerminParseAttr.No) 
            {
                TerminToken res = null;
                foreach (Abridge a in Abridges) 
                {
                    TerminToken r = a.TryAttach(t0);
                    if (r == null) 
                        continue;
                    if (r.AbridgeWithoutPoint && Terms.Count > 0) 
                    {
                        if (!(t0 is Pullenti.Ner.TextToken)) 
                            continue;
                        if (a.Parts[0].Value != (t0 as Pullenti.Ner.TextToken).Term) 
                            continue;
                    }
                    if (res == null || (res.LengthChar < r.LengthChar)) 
                        res = r;
                }
                if (res != null) 
                    return res;
            }
            return null;
        }
        // Попробовать привязать термин с использованием "похожести"
        public TerminToken TryParseSim(Pullenti.Ner.Token t0, double simD, TerminParseAttr pars = TerminParseAttr.No)
        {
            if (t0 == null) 
                return null;
            if (simD >= 1 || (simD < 0.05)) 
                return this.TryParse(t0, pars);
            string term = null;
            if (t0 is Pullenti.Ner.TextToken) 
                term = (t0 as Pullenti.Ner.TextToken).Term;
            if (AcronymSmart != null && ((pars & TerminParseAttr.FullwordsOnly)) == TerminParseAttr.No && term != null) 
            {
                if (AcronymSmart == term) 
                {
                    if (t0.Next != null && t0.Next.IsChar('.') && !t0.IsWhitespaceAfter) 
                        return new TerminToken(t0, t0.Next) { Termin = this };
                    else 
                        return new TerminToken(t0, t0) { Termin = this };
                }
                int i;
                Pullenti.Ner.TextToken t1 = t0 as Pullenti.Ner.TextToken;
                Pullenti.Ner.TextToken tt = t0 as Pullenti.Ner.TextToken;
                for (i = 0; i < Acronym.Length; i++) 
                {
                    if (tt == null) 
                        break;
                    string term1 = tt.Term;
                    if (term1.Length != 1 || tt.IsWhitespaceAfter) 
                        break;
                    if (i > 0 && tt.IsWhitespaceBefore) 
                        break;
                    if (term1[0] != Acronym[i]) 
                        break;
                    if (tt.Next == null || !tt.Next.IsChar('.')) 
                        break;
                    t1 = tt.Next as Pullenti.Ner.TextToken;
                    tt = tt.Next.Next as Pullenti.Ner.TextToken;
                }
                if (i >= Acronym.Length) 
                    return new TerminToken(t0, t1) { Termin = this };
            }
            if (Acronym != null && term != null && Acronym == term) 
            {
                if (t0.Chars.IsAllUpper || AcronymCanBeLower || ((!t0.Chars.IsAllLower && term.Length >= 3))) 
                    return new TerminToken(t0, t0) { Termin = this };
            }
            if (Acronym != null && t0.Chars.IsLastLower && t0.LengthChar > 3) 
            {
                if (t0.IsValue(Acronym, null)) 
                    return new TerminToken(t0, t0) { Termin = this };
            }
            if (Terms.Count > 0) 
            {
                Pullenti.Ner.Token t1 = null;
                Pullenti.Ner.Token tt = t0;
                Pullenti.Ner.MorphCollection mc = null;
                int termInd = -1;
                double termsLen = (double)0;
                double tkCnt = (double)0;
                double termsFoundCnt = (double)0;
                bool wrOder = false;
                foreach (Term it in Terms) 
                {
                    if ((it.CanonicalText.Length < 2) || it.IsHiphen || it.IsPoint) 
                        termsLen += 0.3;
                    else if (it.IsNumber || it.IsPatternAny) 
                        termsLen += 0.7;
                    else 
                        termsLen += 1;
                }
                double maxTksLen = termsLen / simD;
                double curJM = simD;
                List<int> termsFound = new List<int>();
                while (tt != null && (tkCnt < maxTksLen) && (termsFoundCnt < termsLen)) 
                {
                    Pullenti.Morph.MorphClass mcls = null;
                    Pullenti.Ner.TextToken ttt = tt as Pullenti.Ner.TextToken;
                    bool mm = false;
                    if (tt.LengthChar < 2) 
                        tkCnt += 0.3;
                    else if (tt is Pullenti.Ner.NumberToken) 
                        tkCnt += 0.7;
                    else if (ttt == null) 
                        tkCnt++;
                    else 
                    {
                        mcls = ttt.Morph.Class;
                        mm = ((mcls.IsConjunction || mcls.IsPreposition || mcls.IsPronoun) || mcls.IsMisc || mcls.IsUndefined);
                        if (mm) 
                            tkCnt += 0.3;
                        else 
                            tkCnt += 1;
                    }
                    for (int i = 0; i < Terms.Count; i++) 
                    {
                        if (!termsFound.Contains(i)) 
                        {
                            Term trm = Terms[i];
                            if (trm.IsPatternAny) 
                            {
                                termsFoundCnt += 0.7;
                                termsFound.Add(i);
                                break;
                            }
                            else if (trm.CanonicalText.Length < 2) 
                            {
                                termsFoundCnt += 0.3;
                                termsFound.Add(i);
                                break;
                            }
                            else if (trm.CheckByToken(tt)) 
                            {
                                termsFound.Add(i);
                                if (mm) 
                                {
                                    termsLen -= 0.7;
                                    termsFoundCnt += 0.3;
                                }
                                else 
                                    termsFoundCnt += (trm.IsNumber ? 0.7 : (double)1);
                                if (!wrOder) 
                                {
                                    if (i < termInd) 
                                        wrOder = true;
                                    else 
                                        termInd = i;
                                }
                                break;
                            }
                        }
                    }
                    if (termsFoundCnt < 0.2) 
                        return null;
                    double newJM = (termsFoundCnt / (((tkCnt + termsLen) - termsFoundCnt))) * ((wrOder ? 0.7 : (double)1));
                    if (curJM < newJM) 
                    {
                        t1 = tt;
                        curJM = newJM;
                    }
                    tt = tt.Next;
                }
                if (t1 == null) 
                    return null;
                if (t0.Morph.ItemsCount > 0) 
                    mc = new Pullenti.Ner.MorphCollection(t0.Morph);
                return new TerminToken(t0, t1) { Morph = mc, Termin = this };
            }
            if (Abridges != null && ((pars & TerminParseAttr.FullwordsOnly)) == TerminParseAttr.No) 
            {
                TerminToken res = null;
                foreach (Abridge a in Abridges) 
                {
                    TerminToken r = a.TryAttach(t0);
                    if (r == null) 
                        continue;
                    if (r.AbridgeWithoutPoint && Terms.Count > 0) 
                    {
                        if (!(t0 is Pullenti.Ner.TextToken)) 
                            continue;
                        if (a.Parts[0].Value != (t0 as Pullenti.Ner.TextToken).Term) 
                            continue;
                    }
                    if (res == null || (res.LengthChar < r.LengthChar)) 
                        res = r;
                }
                if (res != null) 
                    return res;
            }
            return null;
        }
    }
}