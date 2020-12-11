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

namespace Pullenti.Ner.Keyword
{
    /// <summary>
    /// Анализатор ключевых комбинаций. 
    /// Специфический анализатор, то есть нужно явно создавать процессор через функцию CreateSpecificProcessor, 
    /// указав имя анализатора.
    /// </summary>
    public class KeywordAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("KEYWORD")
        /// </summary>
        public const string ANALYZER_NAME = "KEYWORD";
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
                return "Ключевые комбинации";
            }
        }
        public override string Description
        {
            get
            {
                return "Ключевые слова для различных аналитических систем";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new KeywordAnalyzer();
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {"ALL"};
            }
        }
        /// <summary>
        /// Этот анализатор является специфическим (IsSpecific = true)
        /// </summary>
        public override bool IsSpecific
        {
            get
            {
                return true;
            }
        }
        public override Pullenti.Ner.Core.AnalyzerData CreateAnalyzerData()
        {
            return new Pullenti.Ner.Core.AnalyzerDataWithOntology();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Keyword.Internal.KeywordMeta.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Keyword.Internal.KeywordMeta.ImageObj, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("kwobject.png"));
                res.Add(Pullenti.Ner.Keyword.Internal.KeywordMeta.ImagePred, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("kwpredicate.png"));
                res.Add(Pullenti.Ner.Keyword.Internal.KeywordMeta.ImageRef, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("kwreferent.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == KeywordReferent.OBJ_TYPENAME) 
                return new KeywordReferent();
            return null;
        }
        public override int ProgressWeight
        {
            get
            {
                return 1;
            }
        }
        // Основная функция выделения телефонов
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            bool hasDenoms = false;
            foreach (Pullenti.Ner.Analyzer a in kit.Processor.Analyzers) 
            {
                if ((a is Pullenti.Ner.Denomination.DenominationAnalyzer) && !a.IgnoreThisAnalyzer) 
                    hasDenoms = true;
            }
            if (!hasDenoms) 
            {
                Pullenti.Ner.Denomination.DenominationAnalyzer a = new Pullenti.Ner.Denomination.DenominationAnalyzer();
                a.Process(kit);
            }
            List<KeywordReferent> li = new List<KeywordReferent>();
            StringBuilder tmp = new StringBuilder();
            List<string> tmp2 = new List<string>();
            int max = 0;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                max++;
            }
            int cur = 0;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next,cur++) 
            {
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r != null) 
                {
                    t = this._addReferents(ad, t, cur, max);
                    continue;
                }
                if (!(t is Pullenti.Ner.TextToken)) 
                    continue;
                if (!t.Chars.IsLetter || (t.LengthChar < 3)) 
                    continue;
                string term = (t as Pullenti.Ner.TextToken).Term;
                if (term == "ЕСТЬ") 
                {
                    if ((t.Previous is Pullenti.Ner.TextToken) && t.Previous.Morph.Class.IsVerb) 
                    {
                    }
                    else 
                        continue;
                }
                Pullenti.Ner.Core.NounPhraseToken npt = null;
                npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.AdjectiveCanBeLast | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                if (npt == null) 
                {
                    Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                    if (mc.IsVerb && !mc.IsPreposition) 
                    {
                        if ((t as Pullenti.Ner.TextToken).IsVerbBe) 
                            continue;
                        if (t.IsValue("МОЧЬ", null) || t.IsValue("WOULD", null)) 
                            continue;
                        KeywordReferent kref = new KeywordReferent() { Typ = KeywordType.Predicate };
                        string norm = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Verb, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                        if (norm == null) 
                            norm = (t as Pullenti.Ner.TextToken).Lemma;
                        if (norm.EndsWith("ЬСЯ")) 
                            norm = norm.Substring(0, norm.Length - 2);
                        kref.AddSlot(KeywordReferent.ATTR_VALUE, norm, false, 0);
                        List<Pullenti.Semantic.Utils.DerivateGroup> drv = Pullenti.Semantic.Utils.DerivateService.FindDerivates(norm, true, t.Morph.Language);
                        _addNormals(kref, drv, norm);
                        kref = ad.RegisterReferent(kref) as KeywordReferent;
                        _setRank(kref, cur, max);
                        Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(kref), t, t) { Morph = t.Morph };
                        kit.EmbedToken(rt1);
                        t = rt1;
                        continue;
                    }
                    continue;
                }
                if (npt.InternalNoun != null) 
                    continue;
                if (npt.EndToken.IsValue("ЦЕЛОМ", null) || npt.EndToken.IsValue("ЧАСТНОСТИ", null)) 
                {
                    if (npt.Preposition != null) 
                    {
                        t = npt.EndToken;
                        continue;
                    }
                }
                if (npt.EndToken.IsValue("СТОРОНЫ", null) && npt.Preposition != null && npt.Preposition.Normal == "С") 
                {
                    t = npt.EndToken;
                    continue;
                }
                if (npt.BeginToken == npt.EndToken) 
                {
                    Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                    if (mc.IsPreposition) 
                        continue;
                    else if (mc.IsAdverb) 
                    {
                        if (t.IsValue("ПОТОМ", null)) 
                            continue;
                    }
                }
                else 
                {
                }
                li.Clear();
                Pullenti.Ner.Token t0 = t;
                for (Pullenti.Ner.Token tt = t; tt != null && tt.EndChar <= npt.EndChar; tt = tt.Next) 
                {
                    if (!(tt is Pullenti.Ner.TextToken)) 
                        continue;
                    if (tt.IsValue("NATURAL", null)) 
                    {
                    }
                    if ((tt.LengthChar < 3) || !tt.Chars.IsLetter) 
                        continue;
                    Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                    if ((mc.IsPreposition || mc.IsPronoun || mc.IsPersonalPronoun) || mc.IsConjunction) 
                    {
                        if (tt.IsValue("ОТНОШЕНИЕ", null)) 
                        {
                        }
                        else 
                            continue;
                    }
                    if (mc.IsMisc) 
                    {
                        if (Pullenti.Ner.Core.MiscHelper.IsEngArticle(tt)) 
                            continue;
                    }
                    KeywordReferent kref = new KeywordReferent() { Typ = KeywordType.Object };
                    string norm = (tt as Pullenti.Ner.TextToken).Lemma;
                    kref.AddSlot(KeywordReferent.ATTR_VALUE, norm, false, 0);
                    if (norm != "ЕСТЬ") 
                    {
                        List<Pullenti.Semantic.Utils.DerivateGroup> drv = Pullenti.Semantic.Utils.DerivateService.FindDerivates(norm, true, tt.Morph.Language);
                        _addNormals(kref, drv, norm);
                    }
                    kref = ad.RegisterReferent(kref) as KeywordReferent;
                    _setRank(kref, cur, max);
                    Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(kref, tt, tt) { Morph = tt.Morph };
                    kit.EmbedToken(rt1);
                    if (tt == t && li.Count == 0) 
                        t0 = rt1;
                    t = rt1;
                    li.Add(kref);
                }
                if (li.Count > 1) 
                {
                    KeywordReferent kref = new KeywordReferent() { Typ = KeywordType.Object };
                    tmp.Length = 0;
                    tmp2.Clear();
                    bool hasNorm = false;
                    foreach (KeywordReferent kw in li) 
                    {
                        string s = kw.GetStringValue(KeywordReferent.ATTR_VALUE);
                        if (tmp.Length > 0) 
                            tmp.Append(' ');
                        tmp.Append(s);
                        string n = kw.GetStringValue(KeywordReferent.ATTR_NORMAL);
                        if (n != null) 
                        {
                            hasNorm = true;
                            tmp2.Add(n);
                        }
                        else 
                            tmp2.Add(s);
                        kref.AddSlot(KeywordReferent.ATTR_REF, kw, false, 0);
                    }
                    string val = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                    kref.AddSlot(KeywordReferent.ATTR_VALUE, val, false, 0);
                    tmp.Length = 0;
                    tmp2.Sort();
                    foreach (string s in tmp2) 
                    {
                        if (tmp.Length > 0) 
                            tmp.Append(' ');
                        tmp.Append(s);
                    }
                    string norm = tmp.ToString();
                    if (norm != val) 
                        kref.AddSlot(KeywordReferent.ATTR_NORMAL, norm, false, 0);
                    kref = ad.RegisterReferent(kref) as KeywordReferent;
                    _setRank(kref, cur, max);
                    Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(kref, t0, t) { Morph = npt.Morph };
                    kit.EmbedToken(rt1);
                    t = rt1;
                }
            }
            cur = 0;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next,cur++) 
            {
                KeywordReferent kw = t.GetReferent() as KeywordReferent;
                if (kw == null || kw.Typ != KeywordType.Object) 
                    continue;
                if (t.Next == null || kw.ChildWords > 2) 
                    continue;
                Pullenti.Ner.Token t1 = t.Next;
                if (t1.IsValue("OF", null) && (t1.WhitespacesAfterCount < 3) && t1.Next != null) 
                {
                    t1 = t1.Next;
                    if ((t1 is Pullenti.Ner.TextToken) && Pullenti.Ner.Core.MiscHelper.IsEngArticle(t1) && t1.Next != null) 
                        t1 = t1.Next;
                }
                else if (!t1.Morph.Case.IsGenitive || t.WhitespacesAfterCount > 1) 
                    continue;
                KeywordReferent kw2 = t1.GetReferent() as KeywordReferent;
                if (kw2 == null) 
                    continue;
                if (kw == kw2) 
                    continue;
                if (kw2.Typ != KeywordType.Object || (kw.ChildWords + kw2.ChildWords) > 3) 
                    continue;
                KeywordReferent kwUn = new KeywordReferent();
                kwUn.Union(kw, kw2, Pullenti.Ner.Core.MiscHelper.GetTextValue(t1, t1, Pullenti.Ner.Core.GetTextAttr.No));
                kwUn = ad.RegisterReferent(kwUn) as KeywordReferent;
                _setRank(kwUn, cur, max);
                Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(kwUn, t, t1) { Morph = t.Morph };
                kit.EmbedToken(rt1);
                t = rt1;
            }
            if (SortKeywordsByRank) 
            {
                List<Pullenti.Ner.Referent> all = new List<Pullenti.Ner.Referent>(ad.Referents);
                all.Sort(new CompByRank());
                ad.Referents = all;
            }
            if (AnnotationMaxSentences > 0) 
            {
                KeywordReferent ano = Pullenti.Ner.Keyword.Internal.AutoannoSentToken.CreateAnnotation(kit, AnnotationMaxSentences);
                if (ano != null) 
                    ad.RegisterReferent(ano);
            }
        }
        static int _calcRank(Pullenti.Semantic.Utils.DerivateGroup gr)
        {
            if (gr.IsDummy) 
                return 0;
            int res = 0;
            foreach (Pullenti.Semantic.Utils.DerivateWord w in gr.Words) 
            {
                if (w.Lang.IsRu && w.Class != null) 
                {
                    if (w.Class.IsVerb && w.Class.IsAdjective) 
                    {
                    }
                    else 
                        res++;
                }
            }
            if (gr.Prefix == null) 
                res += 3;
            return res;
        }
        static void _addNormals(KeywordReferent kref, List<Pullenti.Semantic.Utils.DerivateGroup> grs, string norm)
        {
            if (grs == null || grs.Count == 0) 
                return;
            for (int k = 0; k < grs.Count; k++) 
            {
                bool ch = false;
                for (int i = 0; i < (grs.Count - 1); i++) 
                {
                    if (_calcRank(grs[i]) < _calcRank(grs[i + 1])) 
                    {
                        Pullenti.Semantic.Utils.DerivateGroup gr = grs[i];
                        grs[i] = grs[i + 1];
                        grs[i + 1] = gr;
                        ch = true;
                    }
                }
                if (!ch) 
                    break;
            }
            for (int i = 0; (i < 3) && (i < grs.Count); i++) 
            {
                if (!grs[i].IsDummy && grs[i].Words.Count > 0) 
                {
                    if (grs[i].Words[0].Spelling != norm) 
                        kref.AddSlot(KeywordReferent.ATTR_NORMAL, grs[i].Words[0].Spelling, false, 0);
                }
            }
        }
        class CompByRank : IComparer<Pullenti.Ner.Referent>
        {
            public int Compare(Pullenti.Ner.Referent x, Pullenti.Ner.Referent y)
            {
                double d1 = (x as Pullenti.Ner.Keyword.KeywordReferent).Rank;
                double d2 = (y as Pullenti.Ner.Keyword.KeywordReferent).Rank;
                if (d1 > d2) 
                    return -1;
                if (d1 < d2) 
                    return 1;
                return 0;
            }
        }

        Pullenti.Ner.Token _addReferents(Pullenti.Ner.Core.AnalyzerData ad, Pullenti.Ner.Token t, int cur, int max)
        {
            if (!(t is Pullenti.Ner.ReferentToken)) 
                return t;
            Pullenti.Ner.Referent r = t.GetReferent();
            if (r == null) 
                return t;
            if (r is Pullenti.Ner.Denomination.DenominationReferent) 
            {
                Pullenti.Ner.Denomination.DenominationReferent dr = r as Pullenti.Ner.Denomination.DenominationReferent;
                KeywordReferent kref0 = new KeywordReferent() { Typ = KeywordType.Referent };
                foreach (Pullenti.Ner.Slot s in dr.Slots) 
                {
                    if (s.TypeName == Pullenti.Ner.Denomination.DenominationReferent.ATTR_VALUE) 
                        kref0.AddSlot(KeywordReferent.ATTR_NORMAL, s.Value, false, 0);
                }
                kref0.AddSlot(KeywordReferent.ATTR_REF, dr, false, 0);
                Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(kref0), t, t);
                t.Kit.EmbedToken(rt0);
                return rt0;
            }
            if ((r is Pullenti.Ner.Phone.PhoneReferent) || (r is Pullenti.Ner.Uri.UriReferent) || (r is Pullenti.Ner.Bank.BankDataReferent)) 
                return t;
            if (r is Pullenti.Ner.Money.MoneyReferent) 
            {
                Pullenti.Ner.Money.MoneyReferent mr = r as Pullenti.Ner.Money.MoneyReferent;
                KeywordReferent kref0 = new KeywordReferent() { Typ = KeywordType.Object };
                kref0.AddSlot(KeywordReferent.ATTR_NORMAL, mr.Currency, false, 0);
                Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(kref0), t, t);
                t.Kit.EmbedToken(rt0);
                return rt0;
            }
            if (r.TypeName == "DATE" || r.TypeName == "DATERANGE" || r.TypeName == "BOOKLINKREF") 
                return t;
            for (Pullenti.Ner.Token tt = (t as Pullenti.Ner.MetaToken).BeginToken; tt != null && tt.EndChar <= t.EndChar; tt = tt.Next) 
            {
                if (tt is Pullenti.Ner.ReferentToken) 
                    this._addReferents(ad, tt, cur, max);
            }
            KeywordReferent kref = new KeywordReferent() { Typ = KeywordType.Referent };
            string norm = null;
            if (r.TypeName == "GEO") 
                norm = r.GetStringValue("ALPHA2");
            if (norm == null) 
                norm = r.ToString(true, null, 0);
            if (norm != null) 
                kref.AddSlot(KeywordReferent.ATTR_NORMAL, norm.ToUpper(), false, 0);
            kref.AddSlot(KeywordReferent.ATTR_REF, t.GetReferent(), false, 0);
            _setRank(kref, cur, max);
            Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(kref), t, t);
            t.Kit.EmbedToken(rt1);
            return rt1;
        }
        static void _setRank(KeywordReferent kr, int cur, int max)
        {
            double rank = (double)1;
            KeywordType ty = kr.Typ;
            if (ty == KeywordType.Predicate) 
                rank = 1;
            else if (ty == KeywordType.Object) 
            {
                string v = kr.GetStringValue(KeywordReferent.ATTR_VALUE) ?? kr.GetStringValue(KeywordReferent.ATTR_NORMAL);
                if (v != null) 
                {
                    for (int i = 0; i < v.Length; i++) 
                    {
                        if (v[i] == ' ' || v[i] == '-') 
                            rank++;
                    }
                }
            }
            else if (ty == KeywordType.Referent) 
            {
                rank = 3;
                Pullenti.Ner.Referent r = kr.GetSlotValue(KeywordReferent.ATTR_REF) as Pullenti.Ner.Referent;
                if (r != null) 
                {
                    if (r.TypeName == "PERSON") 
                        rank = 4;
                }
            }
            if (max > 0) 
                rank *= ((1 - (((0.5 * cur) / max))));
            kr.Rank += rank;
        }
        /// <summary>
        /// Сортировать ли в списке Entities ключевые слова в порядке убывания ранга
        /// </summary>
        public static bool SortKeywordsByRank = true;
        /// <summary>
        /// Максимально предложений в автоаннотацию (KeywordReferent с типом Annotation). 
        /// Если 0, то не делать автоаннотацию. По умолчанию = 3.
        /// </summary>
        public static int AnnotationMaxSentences = 3;
        static bool m_Initialized = false;
        public static void Initialize()
        {
            if (m_Initialized) 
                return;
            m_Initialized = true;
            try 
            {
                Pullenti.Ner.Keyword.Internal.KeywordMeta.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Denomination.DenominationAnalyzer.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
                Pullenti.Ner.ProcessorService.RegisterAnalyzer(new KeywordAnalyzer());
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}