/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Definition
{
    /// <summary>
    /// Анализатор определений. 
    /// Специфический анализатор, то есть нужно явно создавать процессор через функцию CreateSpecificProcessor, 
    /// указав имя анализатора.
    /// </summary>
    public class DefinitionAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("THESIS")
        /// </summary>
        public const string ANALYZER_NAME = "THESIS";
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
                return "Тезисы";
            }
        }
        public override string Description
        {
            get
            {
                return "Утверждения и определения";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new DefinitionAnalyzer();
        }
        public override int ProgressWeight
        {
            get
            {
                return 1;
            }
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Definition.Internal.MetaDefin.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Definition.Internal.MetaDefin.ImageDefId, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("defin.png"));
                res.Add(Pullenti.Ner.Definition.Internal.MetaDefin.ImageAssId, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("assert.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == DefinitionReferent.OBJ_TYPENAME) 
                return new DefinitionReferent();
            return null;
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
            return new Pullenti.Ner.Core.AnalyzerData();
        }
        // Основная функция выделения объектов
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            if (kit.BaseLanguage == Pullenti.Morph.MorphLang.EN) 
            {
                Pullenti.Ner.Definition.Internal.DefinitionAnalyzerEn.Process(kit, ad);
                return;
            }
            bool glosRegime = false;
            Pullenti.Ner.Core.TerminCollection onto = null;
            Dictionary<string, bool> oh = new Dictionary<string, bool>();
            if (kit.Ontology != null) 
            {
                onto = new Pullenti.Ner.Core.TerminCollection();
                foreach (Pullenti.Ner.ExtOntologyItem it in kit.Ontology.Items) 
                {
                    if (it.Referent is DefinitionReferent) 
                    {
                        string termin = it.Referent.GetStringValue(DefinitionReferent.ATTR_TERMIN);
                        if (!oh.ContainsKey(termin)) 
                        {
                            oh.Add(termin, true);
                            onto.Add(new Pullenti.Ner.Core.Termin(termin) { CanonicText = termin });
                        }
                    }
                }
                if (onto.Termins.Count == 0) 
                    onto = null;
            }
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (!glosRegime && t.IsNewlineBefore) 
                {
                    Pullenti.Ner.Token tt = _tryAttachGlossary(t);
                    if (tt != null) 
                    {
                        t = tt;
                        glosRegime = true;
                        continue;
                    }
                }
                int maxChar = 0;
                bool ok = false;
                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                    ok = true;
                else if (((t.IsValue("ЧТО", null) && t.Next != null && t.Previous != null) && t.Previous.IsComma && t.Previous.Previous != null) && t.Previous.Previous.Morph.Class == Pullenti.Morph.MorphClass.Verb) 
                {
                    ok = true;
                    t = t.Next;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                        t = t.Next;
                }
                else if (t.IsNewlineBefore && glosRegime) 
                    ok = true;
                else if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false) && t.Previous != null && t.Previous.IsChar(':')) 
                {
                    ok = true;
                    t = t.Next;
                    for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt, true, t, false)) 
                        {
                            maxChar = tt.Previous.EndChar;
                            break;
                        }
                    }
                }
                else if (t.IsNewlineBefore && t.Previous != null && t.Previous.IsCharOf(";:")) 
                    ok = true;
                if (!ok) 
                    continue;
                List<Pullenti.Ner.ReferentToken> prs = TryAttach(t, glosRegime, onto, maxChar, false);
                if (prs == null) 
                    prs = this.TryAttachEnd(t, onto, maxChar);
                if (prs != null) 
                {
                    foreach (Pullenti.Ner.ReferentToken pr in prs) 
                    {
                        if (pr.Referent != null) 
                        {
                            pr.Referent = ad.RegisterReferent(pr.Referent);
                            pr.Referent.AddOccurenceOfRefTok(pr);
                        }
                        t = pr.EndToken;
                    }
                }
                else 
                {
                    if (t.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            t = br.EndToken;
                            continue;
                        }
                    }
                    bool ign = false;
                    for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                    {
                        if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                        {
                            if (tt.Previous.IsChar(';')) 
                                ign = true;
                            break;
                        }
                    }
                    if (glosRegime && !t.IsNewlineBefore) 
                    {
                    }
                    else if (!ign) 
                        glosRegime = false;
                }
            }
        }
        static Pullenti.Ner.Token _tryAttachGlossary(Pullenti.Ner.Token t)
        {
            if (t == null || !t.IsNewlineBefore) 
                return null;
            for (; t != null; t = t.Next) 
            {
                if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) 
                    break;
            }
            if (t == null) 
                return null;
            if (t.IsValue("ГЛОССАРИЙ", null) || t.IsValue("ОПРЕДЕЛЕНИЕ", null)) 
                t = t.Next;
            else if (t.IsValue("СПИСОК", null) && t.Next != null && t.Next.IsValue("ОПРЕДЕЛЕНИЕ", null)) 
                t = t.Next.Next;
            else 
            {
                bool use = false;
                bool ponat = false;
                Pullenti.Ner.Token t0 = t;
                for (; t != null; t = t.Next) 
                {
                    if (t.IsValue("ИСПОЛЬЗОВАТЬ", null)) 
                        use = true;
                    else if (t.IsValue("ПОНЯТИЕ", null) || t.IsValue("ОПРЕДЕЛЕНИЕ", null)) 
                        ponat = true;
                    else if (t.IsChar(':')) 
                    {
                        if (use && ponat && t.IsNewlineAfter) 
                            return t;
                    }
                    else if (t != t0 && Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                        break;
                }
                return null;
            }
            if (t == null) 
                return null;
            if (t.IsAnd && t.Next != null && t.Next.IsValue("СОКРАЩЕНИЕ", null)) 
                t = t.Next.Next;
            if (t != null && t.IsCharOf(":.")) 
                t = t.Next;
            if (t != null && t.IsNewlineBefore) 
                return t.Previous;
            return null;
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            List<Pullenti.Ner.ReferentToken> li = TryAttach(begin, false, null, 0, false);
            if (li == null || li.Count == 0) 
                return null;
            return li[0];
        }
        public override Pullenti.Ner.ReferentToken ProcessOntologyItem(Pullenti.Ner.Token begin)
        {
            if (begin == null) 
                return null;
            Pullenti.Ner.Token t1 = null;
            for (Pullenti.Ner.Token t = begin; t != null; t = t.Next) 
            {
                if (t.IsHiphen && ((t.IsWhitespaceBefore || t.IsWhitespaceAfter))) 
                    break;
                else 
                    t1 = t;
            }
            if (t1 == null) 
                return null;
            DefinitionReferent dre = new DefinitionReferent();
            dre.AddSlot(DefinitionReferent.ATTR_TERMIN, Pullenti.Ner.Core.MiscHelper.GetTextValue(begin, t1, Pullenti.Ner.Core.GetTextAttr.No), false, 0);
            return new Pullenti.Ner.ReferentToken(dre, begin, t1);
        }
        static Pullenti.Ner.Token _ignoreListPrefix(Pullenti.Ner.Token t)
        {
            for (; t != null; t = t.Next) 
            {
                if (t.IsNewlineAfter) 
                    break;
                if (t is Pullenti.Ner.NumberToken) 
                {
                    if ((t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Words) 
                        break;
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective, 0, null);
                    if (npt != null && npt.EndChar > t.EndChar) 
                        break;
                    continue;
                }
                if (!(t is Pullenti.Ner.TextToken)) 
                    break;
                if (!t.Chars.IsLetter) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                        break;
                    continue;
                }
                if (t.LengthChar == 1 && t.Next != null && t.Next.IsCharOf(").")) 
                    continue;
                break;
            }
            return t;
        }
        public static List<Pullenti.Ner.ReferentToken> TryAttach(Pullenti.Ner.Token t, bool glosRegime, Pullenti.Ner.Core.TerminCollection onto, int maxChar, bool thisIsDef = false)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            t = _ignoreListPrefix(t);
            if (t == null) 
                return null;
            bool hasPrefix = false;
            if (t0 != t) 
                hasPrefix = true;
            t0 = t;
            Pullenti.Ner.Referent decree = null;
            Pullenti.Ner.Definition.Internal.ParenthesisToken pt = Pullenti.Ner.Definition.Internal.ParenthesisToken.TryAttach(t);
            if (pt != null) 
            {
                decree = pt.Ref;
                t = pt.EndToken.Next;
                if (t != null && t.IsChar(',')) 
                    t = t.Next;
            }
            if (t == null) 
                return null;
            Pullenti.Ner.Token l0 = null;
            Pullenti.Ner.Token l1 = null;
            string altName = null;
            string name0 = null;
            bool normalLeft = false;
            bool canNextSent = false;
            DefinitionKind coef = DefinitionKind.Undefined;
            if (glosRegime) 
                coef = DefinitionKind.Definition;
            bool isOntoTermin = false;
            string ontoPrefix = null;
            if (t.IsValue("ПОД", null)) 
            {
                t = t.Next;
                normalLeft = true;
            }
            else if (t.IsValue("ИМЕННО", null)) 
                t = t.Next;
            if ((t != null && t.IsValue("УТРАТИТЬ", null) && t.Next != null) && t.Next.IsValue("СИЛА", null)) 
            {
                for (; t != null; t = t.Next) 
                {
                    if (t.IsNewlineAfter) 
                    {
                        List<Pullenti.Ner.ReferentToken> re0 = new List<Pullenti.Ner.ReferentToken>();
                        re0.Add(new Pullenti.Ner.ReferentToken(null, t0, t));
                        return re0;
                    }
                }
                return null;
            }
            Pullenti.Ner.MetaToken miscToken = null;
            for (; t != null; t = t.Next) 
            {
                if (t != t0 && Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                    break;
                if (maxChar > 0 && t.EndChar > maxChar) 
                    break;
                Pullenti.Ner.MetaToken mt = _tryAttachMiscToken(t);
                if (mt != null) 
                {
                    miscToken = mt;
                    t = mt.EndToken;
                    normalLeft = mt.Morph.Case.IsNominative;
                    continue;
                }
                if (!(t is Pullenti.Ner.TextToken)) 
                {
                    Pullenti.Ner.Referent r = t.GetReferent();
                    if (r != null && ((r.TypeName == "DECREE" || r.TypeName == "DECREEPART"))) 
                    {
                        decree = r;
                        if (l0 == null) 
                        {
                            if ((t.Next != null && t.Next.GetMorphClassInDictionary() == Pullenti.Morph.MorphClass.Verb && t.Next.Next != null) && t.Next.Next.IsComma) 
                            {
                                t = t.Next.Next;
                                if (t.Next != null && t.Next.IsValue("ЧТО", null)) 
                                    t = t.Next;
                                continue;
                            }
                            l0 = t;
                        }
                        l1 = t;
                        continue;
                    }
                    if (r != null && (((r.TypeName == "ORGANIZATION" || r.TypeName == "PERSONPROPERTY" || r.TypeName == "STREET") || r.TypeName == "GEO"))) 
                    {
                        if (l0 == null) 
                            l0 = t;
                        l1 = t;
                        continue;
                    }
                    if ((t is Pullenti.Ner.NumberToken) && Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null) != null) 
                    {
                    }
                    else 
                        break;
                }
                pt = Pullenti.Ner.Definition.Internal.ParenthesisToken.TryAttach(t);
                if (pt != null && pt.Ref != null) 
                {
                    if (pt.Ref.TypeName == "DECREE" || pt.Ref.TypeName == "DECREEPART") 
                        decree = pt.Ref;
                    t = pt.EndToken.Next;
                    if (l0 == null) 
                        continue;
                    break;
                }
                if (!t.Chars.IsLetter) 
                {
                    if (t.IsHiphen) 
                    {
                        if (t.IsWhitespaceAfter || t.IsWhitespaceBefore) 
                            break;
                        continue;
                    }
                    if (t.IsChar('(')) 
                    {
                        if (l1 == null) 
                            break;
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br == null) 
                            break;
                        Pullenti.Ner.Token tt1 = t.Next;
                        if (tt1.IsValue("ДАЛЕЕ", null)) 
                        {
                            tt1 = tt1.Next;
                            if (!tt1.Chars.IsLetter) 
                                tt1 = tt1.Next;
                            if (tt1 == null) 
                                return null;
                        }
                        altName = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt1, br.EndToken.Previous, Pullenti.Ner.Core.GetTextAttr.No);
                        if (br.BeginToken.Next == br.EndToken.Previous) 
                        {
                            t = br.EndToken;
                            continue;
                        }
                        t = br.EndToken.Next;
                        break;
                    }
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null && l0 == null && Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null) != null) 
                        {
                            l0 = t.Next;
                            l1 = br.EndToken.Previous;
                            altName = null;
                            t = br.EndToken.Next;
                        }
                        else if (br != null && l0 != null) 
                        {
                            l1 = br.EndToken;
                            altName = null;
                            t = br.EndToken;
                            continue;
                        }
                    }
                    break;
                }
                if (t.IsValue("ЭТО", null)) 
                    break;
                if (t.Morph.Class.IsConjunction) 
                {
                    if (!glosRegime || !t.IsAnd) 
                        break;
                    continue;
                }
                Pullenti.Ner.Core.NounPhraseToken npt;
                if (t.IsValue("ДАВАТЬ", null) || t.IsValue("ДАТЬ", null) || t.IsValue("ФОРМУЛИРОВАТЬ", null)) 
                {
                    npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.Noun.IsValue("ОПРЕДЕЛЕНИЕ", null)) 
                    {
                        t = npt.EndToken;
                        if (t.Next != null && t.Next.IsValue("ПОНЯТИЕ", null)) 
                            t = t.Next;
                        l0 = null;
                        l1 = null;
                        normalLeft = true;
                        canNextSent = true;
                        coef = DefinitionKind.Definition;
                        continue;
                    }
                }
                altName = null;
                if (onto != null) 
                {
                    Pullenti.Ner.Core.TerminToken took = onto.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (took != null) 
                    {
                        if (l0 != null) 
                        {
                            if (ontoPrefix != null) 
                                break;
                            ontoPrefix = Pullenti.Ner.Core.MiscHelper.GetTextValue(l0, l1, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        }
                        if (!isOntoTermin) 
                        {
                            isOntoTermin = true;
                            l0 = t;
                        }
                        name0 = took.Termin.CanonicText;
                        t = (l1 = took.EndToken);
                        continue;
                    }
                }
                npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                if (npt != null && npt.InternalNoun != null) 
                    break;
                if (npt == null) 
                {
                    if (l0 != null) 
                        break;
                    if (t.Morph.Class.IsPreposition || t.Morph.Class.IsVerb) 
                        break;
                    if (t.Morph.Class.IsAdjective) 
                    {
                        Pullenti.Ner.Token tt;
                        int ve = 0;
                        for (tt = t.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.GetMorphClassInDictionary().IsVerb) 
                                ve++;
                            else 
                                break;
                        }
                        if ((ve > 0 && tt != null && tt.IsValue("ТАКОЙ", null)) && Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null) != null) 
                        {
                            l0 = (l1 = t);
                            t = t.Next;
                            break;
                        }
                    }
                    if (!t.Chars.IsAllLower && t.LengthChar > 2 && t.GetMorphClassInDictionary().IsUndefined) 
                    {
                    }
                    else 
                        continue;
                }
                if (l0 == null) 
                {
                    if (t.Morph.Class.IsPreposition) 
                        break;
                    if (m_VerbotFirstWords.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No) != null && onto == null) 
                        break;
                    l0 = t;
                }
                else if (t.Morph.Class.IsPreposition) 
                {
                    if (m_VerbotLastWords.TryParse(npt.Noun.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No) != null || m_VerbotLastWords.TryParse(npt.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                    {
                        t = npt.EndToken.Next;
                        break;
                    }
                }
                if (npt != null) 
                {
                    if (m_VerbotFirstWords.TryParse(npt.Noun.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No) != null && onto == null) 
                        break;
                    bool ok1 = true;
                    if (!glosRegime) 
                    {
                        for (Pullenti.Ner.Token tt = npt.BeginToken; tt != null && tt.EndChar <= npt.EndChar; tt = tt.Next) 
                        {
                            if (tt.Morph.Class.IsPronoun || tt.Morph.Class.IsPersonalPronoun) 
                            {
                                if (tt.IsValue("ИНОЙ", null)) 
                                {
                                }
                                else 
                                {
                                    ok1 = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (!ok1) 
                        break;
                    t = (l1 = npt.EndToken);
                }
                else 
                    l1 = t;
            }
            if (!(t is Pullenti.Ner.TextToken) || ((l1 == null && !isOntoTermin)) || t.Next == null) 
                return null;
            if (onto != null && name0 == null) 
                return null;
            bool isNot = false;
            Pullenti.Ner.Token r0 = t;
            Pullenti.Ner.Token r1 = null;
            if (t.IsValue("НЕ", null)) 
            {
                t = t.Next;
                if (t == null) 
                    return null;
                isNot = true;
            }
            bool normalRight = false;
            int ok = 0;
            bool hasthis = false;
            if (t.IsHiphen || t.IsCharOf(":") || ((canNextSent && t.IsChar('.')))) 
            {
                if ((t.Next is Pullenti.Ner.TextToken) && (t.Next as Pullenti.Ner.TextToken).Term == "ЭТО") 
                {
                    ok = 2;
                    t = t.Next.Next;
                    hasthis = true;
                }
                else if (glosRegime) 
                {
                    ok = 2;
                    t = t.Next;
                }
                else if (isOntoTermin) 
                {
                    ok = 1;
                    t = t.Next;
                }
                else if (t.IsHiphen && t.IsWhitespaceBefore && t.IsWhitespaceAfter) 
                {
                    Pullenti.Ner.Token tt = t.Next;
                    if (tt != null && tt.IsValue("НЕ", null)) 
                    {
                        isNot = true;
                        tt = tt.Next;
                    }
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.Morph.Case.IsNominative) 
                    {
                        ok = 2;
                        t = tt;
                    }
                    else if ((tt != null && tt.Morph.Case.IsNominative && tt.Morph.Class.IsVerb) && tt.Morph.Class.IsAdjective) 
                    {
                        ok = 2;
                        t = tt;
                    }
                }
                else 
                {
                    List<Pullenti.Ner.ReferentToken> rt0 = TryAttach(t.Next, false, null, maxChar, false);
                    if (rt0 != null) 
                    {
                        foreach (Pullenti.Ner.ReferentToken rt in rt0) 
                        {
                            if (coef == DefinitionKind.Definition && (rt.Referent as DefinitionReferent).Kind == DefinitionKind.Assertation) 
                                (rt.Referent as DefinitionReferent).Kind = coef;
                        }
                        return rt0;
                    }
                }
            }
            else if ((t as Pullenti.Ner.TextToken).Term == "ЭТО") 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    ok = 1;
                    t = t.Next;
                    hasthis = true;
                }
            }
            else if (t.IsValue("ЯВЛЯТЬСЯ", null) || t.IsValue("ПРИЗНАВАТЬСЯ", null) || t.IsValue("ЕСТЬ", null)) 
            {
                if (t.IsValue("ЯВЛЯТЬСЯ", null)) 
                    normalRight = true;
                Pullenti.Ner.Token t11 = t.Next;
                for (; t11 != null; t11 = t11.Next) 
                {
                    if (t11.IsComma || t11.Morph.Class.IsPreposition || t11.Morph.Class.IsConjunction) 
                    {
                    }
                    else 
                        break;
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t11, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null || t11.GetMorphClassInDictionary().IsAdjective) 
                {
                    ok = 1;
                    t = t11;
                    normalLeft = true;
                }
                else if ((t11 != null && t11.IsValue("ОДИН", null) && t11.Next != null) && t11.Next.IsValue("ИЗ", null)) 
                {
                    ok = 1;
                    t = t11;
                    normalLeft = true;
                }
                if (isOntoTermin) 
                    ok = 1;
                else if (l0 == l1 && npt != null && l0.Morph.Class.IsAdjective) 
                {
                    if (((l0.Morph.Gender & npt.Morph.Gender)) != Pullenti.Morph.MorphGender.Undefined || ((l0.Morph.Number & npt.Morph.Number)) == Pullenti.Morph.MorphNumber.Plural) 
                        name0 = string.Format("{0} {1}", l0.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, npt.Morph.Gender, false), npt.Noun.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, npt.Morph.Gender, false));
                    else 
                        ok = 0;
                }
            }
            else if (t.IsValue("ОЗНАЧАТЬ", null) || t.IsValue("НЕСТИ", null)) 
            {
                Pullenti.Ner.Token t11 = t.Next;
                if (t11 != null && t11.IsChar(':')) 
                    t11 = t11.Next;
                if (t11.IsValue("НЕ", null) && t11.Next != null) 
                {
                    isNot = true;
                    t11 = t11.Next;
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t11, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null || isOntoTermin) 
                {
                    ok = 1;
                    t = t11;
                }
            }
            else if (t.IsValue("ВЫРАЖАТЬ", null)) 
            {
                Pullenti.Ner.Token t11 = t.Next;
                for (; t11 != null; t11 = t11.Next) 
                {
                    if ((t11.Morph.Class.IsPronoun || t11.IsComma || t11.Morph.Class.IsPreposition) || t11.Morph.Class.IsConjunction) 
                    {
                    }
                    else 
                        break;
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t11, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null || isOntoTermin) 
                {
                    ok = 1;
                    t = t11;
                }
            }
            else if (((t.IsValue("СЛЕДОВАТЬ", null) || t.IsValue("МОЖНО", null))) && t.Next != null && ((t.Next.IsValue("ПОНИМАТЬ", null) || t.Next.IsValue("ОПРЕДЕЛИТЬ", null) || t.Next.IsValue("СЧИТАТЬ", null)))) 
            {
                Pullenti.Ner.Token t11 = t.Next.Next;
                if (t11 == null) 
                    return null;
                if (t11.IsValue("КАК", null)) 
                    t11 = t11.Next;
                    {
                        ok = 2;
                        t = t11;
                    }
            }
            else if (t.IsValue("ПРЕДСТАВЛЯТЬ", null) && t.Next != null && t.Next.IsValue("СОБОЙ", null)) 
            {
                Pullenti.Ner.Token t11 = t.Next.Next;
                if (t11 == null) 
                    return null;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t11, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null || t11.Morph.Class.IsAdjective || isOntoTermin) 
                {
                    ok = 1;
                    t = t11;
                }
            }
            else if ((((t.IsValue("ДОЛЖЕН", null) || t.IsValue("ДОЛЖНЫЙ", null))) && t.Next != null && t.Next.IsValue("ПРЕДСТАВЛЯТЬ", null)) && t.Next.Next != null && t.Next.Next.IsValue("СОБОЙ", null)) 
            {
                Pullenti.Ner.Token t11 = t.Next.Next.Next;
                if (t11 == null) 
                    return null;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t11, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null || t11.Morph.Class.IsAdjective || isOntoTermin) 
                {
                    ok = 1;
                    t = t11;
                }
            }
            else if (t.IsValue("ДОЛЖНЫЙ", null)) 
            {
                if (t.Next != null && t.Next.Morph.Class.IsVerb) 
                    t = t.Next;
                ok = 1;
            }
            else if (((((((((t.IsValue("МОЖЕТ", null) || t.IsValue("МОЧЬ", null) || t.IsValue("ВПРАВЕ", null)) || t.IsValue("ЗАПРЕЩЕНО", null) || t.IsValue("РАЗРЕШЕНО", null)) || t.IsValue("ОТВЕЧАТЬ", null) || t.IsValue("ПРИЗНАВАТЬ", null)) || t.IsValue("ОСВОБОЖДАТЬ", null) || t.IsValue("ОСУЩЕСТВЛЯТЬ", null)) || t.IsValue("ПРОИЗВОДИТЬ", null) || t.IsValue("ПОДЛЕЖАТЬ", null)) || t.IsValue("ПРИНИМАТЬ", null) || t.IsValue("СЧИТАТЬ", null)) || t.IsValue("ИМЕТЬ", null) || t.IsValue("ВПРАВЕ", null)) || t.IsValue("ОБЯЗАН", null) || t.IsValue("ОБЯЗАТЬ", null))) 
                ok = 1;
            if (ok == 0) 
                return null;
            if (t == null) 
                return null;
            if (t.IsValue("НЕ", null)) 
            {
                if (!isOntoTermin) 
                    return null;
            }
            DefinitionReferent dr = new DefinitionReferent();
            normalLeft = true;
            string nam = name0 ?? Pullenti.Ner.Core.MiscHelper.GetTextValue(l0, l1, (normalLeft ? Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle : Pullenti.Ner.Core.GetTextAttr.No));
            if (nam == null) 
                return null;
            if (name0 == null) 
            {
            }
            if (name0 == null) 
                dr.Tag = new Pullenti.Ner.MetaToken(l0, l1) { Tag = normalLeft };
            if (l0 == l1 && l0.Morph.Class.IsAdjective && l0.Morph.Case.IsInstrumental) 
            {
                if (t != null && t.IsValue("ТАКОЙ", null)) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.Morph.Case.IsNominative) 
                    {
                        string str = l0.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, (npt.Morph.Number == Pullenti.Morph.MorphNumber.Plural ? Pullenti.Morph.MorphNumber.Singular : Pullenti.Morph.MorphNumber.Undefined), npt.Morph.Gender, false);
                        if (str == null) 
                            str = l0.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                        nam = string.Format("{0} {1}", str, npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                    }
                }
            }
            if (decree != null) 
            {
                for (Pullenti.Ner.Token tt = l0; tt != null && tt.EndChar <= l1.EndChar; tt = tt.Next) 
                {
                    if (tt.GetReferent() == decree) 
                    {
                        decree = null;
                        break;
                    }
                }
            }
            if (nam.EndsWith(")") && altName == null) 
            {
                int ii = nam.LastIndexOf('(');
                if (ii > 0) 
                {
                    altName = nam.Substring(ii + 1, nam.Length - ii - 2).Trim();
                    nam = nam.Substring(0, ii).Trim();
                }
            }
            dr.AddSlot(DefinitionReferent.ATTR_TERMIN, nam, false, 0);
            if (altName != null) 
                dr.AddSlot(DefinitionReferent.ATTR_TERMIN, altName, false, 0);
            if (!isOntoTermin) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(l0, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt2 != null && npt2.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                {
                    nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(l0, l1, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle);
                    if (nam != null) 
                        dr.AddSlot(DefinitionReferent.ATTR_TERMIN, nam, false, 0);
                }
            }
            if (miscToken != null) 
            {
                if (miscToken.Morph.Class.IsNoun) 
                    dr.AddSlot(DefinitionReferent.ATTR_TERMIN_ADD, miscToken.Tag as string, false, 0);
                else 
                    dr.AddSlot(DefinitionReferent.ATTR_MISC, miscToken.Tag as string, false, 0);
            }
            Pullenti.Ner.Token t1 = null;
            List<Pullenti.Ner.MetaToken> multiParts = null;
            for (; t != null; t = t.Next) 
            {
                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                    break;
                if (maxChar > 0 && t.EndChar > maxChar) 
                    break;
                t1 = t;
                if (t.IsChar('(') && (t.Next is Pullenti.Ner.ReferentToken)) 
                {
                    Pullenti.Ner.Referent r = t.Next.GetReferent();
                    if (r.TypeName == "DECREE" || r.TypeName == "DECREEPART") 
                    {
                        decree = r;
                        t1 = (t = t.Next);
                        while (t.Next != null) 
                        {
                            if (t.Next.IsCommaAnd && (t.Next.Next is Pullenti.Ner.ReferentToken) && ((t.Next.Next.GetReferent().TypeName == "DECREE" || t.Next.Next.GetReferent().TypeName == "DECREEPART"))) 
                                t1 = (t = t.Next.Next);
                            else 
                                break;
                        }
                        if (t1.Next != null && t1.Next.IsChar(')')) 
                            t = (t1 = t1.Next);
                        continue;
                    }
                }
                if (t.IsChar('(') && t.Next != null && t.Next.IsValue("ДАЛЕЕ", null)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        t = (t1 = br.EndToken);
                        continue;
                    }
                }
                if (t.IsChar(':') && t.IsWhitespaceAfter) 
                {
                    Pullenti.Ner.MetaToken mt = _tryParseListItem(t.Next);
                    if (mt != null) 
                    {
                        multiParts = new List<Pullenti.Ner.MetaToken>();
                        multiParts.Add(mt);
                        for (Pullenti.Ner.Token tt = mt.EndToken.Next; tt != null; tt = tt.Next) 
                        {
                            if (maxChar > 0 && tt.EndChar > maxChar) 
                                break;
                            mt = _tryParseListItem(tt);
                            if (mt == null) 
                                break;
                            multiParts.Add(mt);
                            tt = mt.EndToken;
                        }
                        break;
                    }
                }
                if (!t.IsCharOf(";.")) 
                    r1 = t;
            }
            if (r1 == null) 
                return null;
            if (r0.Next != null && (r0 is Pullenti.Ner.TextToken) && !r0.Chars.IsLetter) 
                r0 = r0.Next;
            normalRight = false;
            string df = Pullenti.Ner.Core.MiscHelper.GetTextValue(r0, r1, ((normalRight ? Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle : Pullenti.Ner.Core.GetTextAttr.No)) | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
            if (multiParts != null) 
            {
                List<Pullenti.Ner.ReferentToken> res1 = new List<Pullenti.Ner.ReferentToken>();
                dr.Kind = (isNot ? DefinitionKind.Negation : DefinitionKind.Assertation);
                foreach (Pullenti.Ner.MetaToken mp in multiParts) 
                {
                    Pullenti.Ner.Referent dr1 = dr.Clone();
                    StringBuilder tmp = new StringBuilder();
                    if (df != null) 
                    {
                        tmp.Append(df);
                        if (tmp.Length > 0 && tmp[tmp.Length - 1] == ':') 
                            tmp.Length--;
                        tmp.Append(": ");
                        tmp.Append(Pullenti.Ner.Core.MiscHelper.GetTextValue(mp.BeginToken, mp.EndToken, Pullenti.Ner.Core.GetTextAttr.KeepRegister));
                    }
                    dr1.AddSlot(DefinitionReferent.ATTR_VALUE, tmp.ToString(), false, 0);
                    res1.Add(new Pullenti.Ner.ReferentToken(dr1, (res1.Count == 0 ? t0 : mp.BeginToken), mp.EndToken));
                }
                return res1;
            }
            if (df == null || (df.Length < 20)) 
                return null;
            if (ontoPrefix != null) 
                df = string.Format("{0} {1}", ontoPrefix, df);
            if ((coef == DefinitionKind.Undefined && ok > 1 && !isNot) && multiParts == null) 
            {
                bool allNps = true;
                int couNpt = 0;
                for (Pullenti.Ner.Token tt = l0; tt != null && tt.EndChar <= l1.EndChar; tt = tt.Next) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun, 0, null);
                    if (npt == null && tt.Morph.Class.IsPreposition) 
                        npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt == null) 
                    {
                        allNps = false;
                        break;
                    }
                    couNpt++;
                    tt = npt.EndToken;
                }
                if (allNps && (couNpt < 5)) 
                {
                    if ((df.Length / 3) > nam.Length) 
                        coef = DefinitionKind.Definition;
                }
            }
            if ((t1.IsChar(';') && t1.IsNewlineAfter && onto != null) && !hasPrefix && multiParts == null) 
            {
                StringBuilder tmp = new StringBuilder();
                tmp.Append(df);
                for (t = t1.Next; t != null; t = t.Next) 
                {
                    if (t.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            t = br.EndToken;
                            continue;
                        }
                    }
                    Pullenti.Ner.Token tt = _ignoreListPrefix(t);
                    if (tt == null) 
                        break;
                    Pullenti.Ner.Token tt1 = null;
                    for (Pullenti.Ner.Token ttt1 = tt; ttt1 != null; ttt1 = ttt1.Next) 
                    {
                        if (ttt1.IsNewlineAfter) 
                        {
                            tt1 = ttt1;
                            break;
                        }
                    }
                    if (tt1 == null) 
                        break;
                    string df1 = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt, (tt1.IsCharOf(".;") ? tt1.Previous : tt1), Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                    if (df1 == null) 
                        break;
                    tmp.AppendFormat(";\n {0}", df1);
                    t = (t1 = tt1);
                    if (!tt1.IsChar(';')) 
                        break;
                }
                df = tmp.ToString();
            }
            dr.AddSlot(DefinitionReferent.ATTR_VALUE, df, false, 0);
            if (isNot) 
                coef = DefinitionKind.Negation;
            else if (hasthis && thisIsDef) 
                coef = DefinitionKind.Definition;
            else if (miscToken != null && !miscToken.Morph.Class.IsNoun) 
                coef = DefinitionKind.Assertation;
            if (coef == DefinitionKind.Undefined) 
                coef = DefinitionKind.Assertation;
            if (decree != null) 
                dr.AddSlot(DefinitionReferent.ATTR_DECREE, decree, false, 0);
            dr.Kind = coef;
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            res.Add(new Pullenti.Ner.ReferentToken(dr, t0, t1));
            return res;
        }
        // Это распознавание случая, когда термин находится в конце
        List<Pullenti.Ner.ReferentToken> TryAttachEnd(Pullenti.Ner.Token t, Pullenti.Ner.Core.TerminCollection onto, int maxChar)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            t = _ignoreListPrefix(t);
            if (t == null) 
                return null;
            bool hasPrefix = false;
            if (t0 != t) 
                hasPrefix = true;
            t0 = t;
            Pullenti.Ner.Referent decree = null;
            Pullenti.Ner.Definition.Internal.ParenthesisToken pt = Pullenti.Ner.Definition.Internal.ParenthesisToken.TryAttach(t);
            if (pt != null) 
            {
                decree = pt.Ref;
                t = pt.EndToken.Next;
                if (t != null && t.IsChar(',')) 
                    t = t.Next;
            }
            if (t == null) 
                return null;
            Pullenti.Ner.Token r0 = t0;
            Pullenti.Ner.Token r1 = null;
            Pullenti.Ner.Token l0 = null;
            for (; t != null; t = t.Next) 
            {
                if (t != t0 && Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                    break;
                if (maxChar > 0 && t.EndChar > maxChar) 
                    break;
                if (t.IsValue("НАЗЫВАТЬ", null) || t.IsValue("ИМЕНОВАТЬ", null)) 
                {
                }
                else 
                    continue;
                r1 = t.Previous;
                for (Pullenti.Ner.Token tt = r1; tt != null; tt = tt.Previous) 
                {
                    if ((tt.IsValue("БУДЕМ", null) || tt.IsValue("ДАЛЬНЕЙШИЙ", null) || tt.IsValue("ДАЛЕЕ", null)) || tt.IsValue("В", null)) 
                        r1 = tt.Previous;
                    else 
                        break;
                }
                l0 = t.Next;
                for (Pullenti.Ner.Token tt = l0; tt != null; tt = tt.Next) 
                {
                    if ((tt.IsValue("БУДЕМ", null) || tt.IsValue("ДАЛЬНЕЙШИЙ", null) || tt.IsValue("ДАЛЕЕ", null)) || tt.IsValue("В", null)) 
                        l0 = tt.Next;
                    else 
                        break;
                }
                break;
            }
            if (l0 == null || r1 == null) 
                return null;
            Pullenti.Ner.Token l1 = null;
            int cou = 0;
            for (t = l0; t != null; t = t.Next) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt == null && t != l0 && t.Morph.Class.IsPreposition) 
                    npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt == null) 
                    break;
                l1 = (t = npt.EndToken);
                cou++;
            }
            if (l1 == null || cou > 3) 
                return null;
            if ((((l1.EndChar - l0.EndChar)) * 2) > ((r1.EndChar - r0.EndChar))) 
                return null;
            DefinitionReferent dr = new DefinitionReferent() { Kind = DefinitionKind.Definition };
            string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(l0, l1, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
            if (nam == null) 
                return null;
            dr.AddSlot(DefinitionReferent.ATTR_TERMIN, nam, false, 0);
            string df = Pullenti.Ner.Core.MiscHelper.GetTextValue(r0, r1, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
            dr.AddSlot(DefinitionReferent.ATTR_VALUE, df, false, 0);
            t = l1.Next;
            if (t == null) 
            {
            }
            else if (t.IsCharOf(".;")) 
                l1 = t;
            else if (t.IsComma) 
                l1 = t;
            else if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
            {
            }
            else 
                return null;
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            res.Add(new Pullenti.Ner.ReferentToken(dr, r0, l1));
            return res;
        }
        static Pullenti.Ner.MetaToken _tryAttachMiscToken(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if (t.IsChar('(')) 
            {
                Pullenti.Ner.MetaToken mt = _tryAttachMiscToken(t.Next);
                if (mt != null && mt.EndToken.Next != null && mt.EndToken.Next.IsChar(')')) 
                {
                    mt.BeginToken = t;
                    mt.EndToken = mt.EndToken.Next;
                    return mt;
                }
                return null;
            }
            if (t.IsValue("КАК", null)) 
            {
                Pullenti.Ner.Token t1 = null;
                for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsNewlineBefore) 
                        break;
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt1 == null) 
                        break;
                    if (t1 == null || npt1.Morph.Case.IsGenitive) 
                    {
                        t1 = (tt = npt1.EndToken);
                        continue;
                    }
                    break;
                }
                if (t1 != null) 
                {
                    Pullenti.Ner.MetaToken res = new Pullenti.Ner.MetaToken(t, t1) { Tag = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1, Pullenti.Ner.Core.GetTextAttr.KeepQuotes) };
                    res.Morph.Class = Pullenti.Morph.MorphClass.Noun;
                    return res;
                }
                return null;
            }
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective, 0, null);
            if (npt != null) 
            {
                if (m_MiscFirstWords.TryParse(npt.Noun.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                {
                    Pullenti.Ner.MetaToken res = new Pullenti.Ner.MetaToken(t, npt.EndToken) { Tag = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false) };
                    res.Morph.Case = Pullenti.Morph.MorphCase.Nominative;
                    return res;
                }
            }
            if (t.IsValue("В", null)) 
            {
                npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    if (npt.Noun.IsValue("СМЫСЛ", null)) 
                    {
                        Pullenti.Ner.MetaToken res = new Pullenti.Ner.MetaToken(t, npt.EndToken) { Tag = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, npt.EndToken, Pullenti.Ner.Core.GetTextAttr.No) };
                        res.Morph.Class = Pullenti.Morph.MorphClass.Noun;
                        return res;
                    }
                }
            }
            return null;
        }
        static Pullenti.Ner.MetaToken _tryParseListItem(Pullenti.Ner.Token t)
        {
            if (t == null || !t.IsWhitespaceBefore) 
                return null;
            Pullenti.Ner.Token tt = null;
            int pr = 0;
            for (tt = t; tt != null; tt = tt.Next) 
            {
                if (tt.IsWhitespaceBefore && tt != t) 
                    break;
                if (tt is Pullenti.Ner.NumberToken) 
                {
                    pr++;
                    continue;
                }
                Pullenti.Ner.NumberToken nex = Pullenti.Ner.Core.NumberHelper.TryParseRoman(tt);
                if (nex != null) 
                {
                    pr++;
                    tt = nex.EndToken;
                    continue;
                }
                if (!(tt is Pullenti.Ner.TextToken)) 
                    break;
                if (!tt.Chars.IsLetter) 
                {
                    if (!tt.IsChar('(')) 
                        pr++;
                }
                else if (tt.LengthChar > 1 || tt.IsWhitespaceAfter) 
                    break;
                else 
                    pr++;
            }
            if (tt == null) 
                return null;
            if (pr == 0) 
            {
                if (t.IsChar('(')) 
                    return null;
                if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsAllLower) 
                    pr++;
            }
            if (pr == 0) 
                return null;
            Pullenti.Ner.MetaToken res = new Pullenti.Ner.MetaToken(tt, tt);
            for (; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore && tt != t) 
                    break;
                else 
                    res.EndToken = tt;
            }
            return res;
        }
        static Pullenti.Ner.Core.TerminCollection m_MiscFirstWords;
        static Pullenti.Ner.Core.TerminCollection m_VerbotFirstWords;
        static Pullenti.Ner.Core.TerminCollection m_VerbotLastWords;
        public static void Initialize()
        {
            if (m_Proc0 != null) 
                return;
            Pullenti.Ner.Definition.Internal.MetaDefin.Initialize();
            try 
            {
                m_Proc0 = Pullenti.Ner.ProcessorService.CreateEmptyProcessor();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                m_MiscFirstWords = new Pullenti.Ner.Core.TerminCollection();
                foreach (string s in new string[] {"ЧЕРТА", "ХАРАКТЕРИСТИКА", "ОСОБЕННОСТЬ", "СВОЙСТВО", "ПРИЗНАК", "ПРИНЦИП", "РАЗНОВИДНОСТЬ", "ВИД", "ПОКАЗАТЕЛЬ", "ЗНАЧЕНИЕ"}) 
                {
                    m_MiscFirstWords.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.RU, true));
                }
                m_VerbotFirstWords = new Pullenti.Ner.Core.TerminCollection();
                foreach (string s in new string[] {"ЦЕЛЬ", "БОЛЬШИНСТВО", "ЧАСТЬ", "ЗАДАЧА", "ИСКЛЮЧЕНИЕ", "ПРИМЕР", "ЭТАП", "ШАГ", "СЛЕДУЮЩИЙ", "ПОДОБНЫЙ", "АНАЛОГИЧНЫЙ", "ПРЕДЫДУЩИЙ", "ПОХОЖИЙ", "СХОЖИЙ", "НАЙДЕННЫЙ", "НАИБОЛЕЕ", "НАИМЕНЕЕ", "ВАЖНЫЙ", "РАСПРОСТРАНЕННЫЙ"}) 
                {
                    m_VerbotFirstWords.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.RU, true));
                }
                m_VerbotLastWords = new Pullenti.Ner.Core.TerminCollection();
                foreach (string s in new string[] {"СТАТЬЯ", "ГЛАВА", "РАЗДЕЛ", "КОДЕКС", "ЗАКОН", "ФОРМУЛИРОВКА", "НАСТОЯЩИЙ", "ВЫШЕУКАЗАННЫЙ", "ДАННЫЙ"}) 
                {
                    m_VerbotLastWords.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.RU, true));
                }
                Pullenti.Ner.Definition.Internal.ParenthesisToken.Initialize();
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new DefinitionAnalyzer());
        }
        /// <summary>
        /// Вычисление коэффициента семантической близости 2-х текстов. 
        /// Учитываются именные группы (существительные с возможными прилагательными).
        /// </summary>
        /// <param name="text1">первый текст</param>
        /// <param name="text2">второй текст</param>
        /// <return>0 - ничего общего, 100 - полное соответствие (тождество)</return>
        public static int CalcSemanticCoef(string text1, string text2)
        {
            Pullenti.Ner.AnalysisResult ar1 = m_Proc0.Process(new Pullenti.Ner.SourceOfAnalysis(text1), null, null);
            if (ar1 == null || ar1.FirstToken == null) 
                return 0;
            Pullenti.Ner.AnalysisResult ar2 = m_Proc0.Process(new Pullenti.Ner.SourceOfAnalysis(text2), null, null);
            if (ar2 == null || ar2.FirstToken == null) 
                return 0;
            List<string> terms1 = new List<string>();
            List<string> terms2 = new List<string>();
            for (int k = 0; k < 2; k++) 
            {
                List<string> terms = (k == 0 ? terms1 : terms2);
                for (Pullenti.Ner.Token t = (k == 0 ? ar1.FirstToken : ar2.FirstToken); t != null; t = t.Next) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null) 
                    {
                        string term = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                        if (term == null) 
                            continue;
                        if (!terms.Contains(term)) 
                            terms.Add(term);
                        continue;
                    }
                }
            }
            if (terms2.Count == 0 || terms1.Count == 0) 
                return 0;
            int coef = 0;
            foreach (string w in terms1) 
            {
                if (terms2.Contains(w)) 
                    coef += 2;
            }
            return (coef * 100) / ((terms1.Count + terms2.Count));
        }
        /// <summary>
        /// Выделить ключевые концепты из текста. 
        /// Концепт - это нормализованная комбинация ключевых слов, причём дериватная нормализация 
        /// (СЛУЖИТЬ -> СЛУЖБА).
        /// </summary>
        /// <param name="txt">текст</param>
        /// <param name="doNormalizeForEnglish">делать ли для английского языка нормализацию по дериватам</param>
        /// <return>список концептов</return>
        public static List<string> GetConcepts(string txt, bool doNormalizeForEnglish = false)
        {
            Pullenti.Ner.AnalysisResult ar = m_Proc0.Process(new Pullenti.Ner.SourceOfAnalysis(txt), null, null);
            List<string> res = new List<string>();
            List<string> tmp = new List<string>();
            StringBuilder tmp2 = new StringBuilder();
            if (ar != null) 
            {
                for (Pullenti.Ner.Token t = ar.FirstToken; t != null; t = t.Next) 
                {
                    Pullenti.Ner.Token t1 = null;
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective, 0, null);
                    if (npt != null) 
                        t1 = npt.EndToken;
                    else if ((t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).IsPureVerb) 
                        t1 = t;
                    if (t1 == null) 
                        continue;
                    for (Pullenti.Ner.Token tt = t1.Next; tt != null; tt = tt.Next) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt2;
                        if (tt.IsAnd) 
                        {
                            npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                            if (npt2 != null) 
                            {
                                tt = (t1 = npt2.EndToken);
                                continue;
                            }
                            break;
                        }
                        npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                        if (npt2 != null) 
                        {
                            if (npt2.Preposition != null) 
                            {
                                tt = (t1 = npt2.EndToken);
                                continue;
                            }
                            else if (npt2.Morph.Case.IsGenitive || npt2.Morph.Case.IsInstrumental) 
                            {
                                tt = (t1 = npt2.EndToken);
                                continue;
                            }
                        }
                        break;
                    }
                    List<List<string>> vars = new List<List<string>>();
                    for (Pullenti.Ner.Token tt = t; tt != null && tt.EndChar <= t1.EndChar; tt = tt.Next) 
                    {
                        if (!(tt is Pullenti.Ner.TextToken)) 
                            continue;
                        if (tt.IsCommaAnd || t.Morph.Class.IsPreposition) 
                            continue;
                        string w = (tt as Pullenti.Ner.TextToken).Lemma;
                        if (w.Length < 3) 
                            continue;
                        if (tt.Chars.IsLatinLetter && !doNormalizeForEnglish) 
                        {
                        }
                        else 
                        {
                            List<Pullenti.Semantic.Utils.DerivateGroup> dg = Pullenti.Semantic.Utils.DerivateService.FindDerivates(w, true, null);
                            if (dg != null && dg.Count == 1) 
                            {
                                if (dg[0].Words.Count > 0) 
                                    w = dg[0].Words[0].Spelling.ToUpper();
                            }
                        }
                        if (tt.Previous != null && tt.Previous.IsCommaAnd && vars.Count > 0) 
                            vars[vars.Count - 1].Add(w);
                        else 
                        {
                            List<string> li = new List<string>();
                            li.Add(w);
                            vars.Add(li);
                        }
                    }
                    t = t1;
                    if (vars.Count == 0) 
                        continue;
                    int[] inds = new int[vars.Count];
                    while (true) 
                    {
                        tmp.Clear();
                        for (int i = 0; i < vars.Count; i++) 
                        {
                            string w = vars[i][inds[i]];
                            if (!tmp.Contains(w)) 
                                tmp.Add(w);
                        }
                        tmp.Sort();
                        tmp2.Length = 0;
                        for (int i = 0; i < tmp.Count; i++) 
                        {
                            if (tmp2.Length > 0) 
                                tmp2.Append(' ');
                            tmp2.Append(tmp[i]);
                        }
                        string ww = tmp2.ToString();
                        if (!res.Contains(ww)) 
                            res.Add(ww);
                        int j;
                        for (j = vars.Count - 1; j >= 0; j--) 
                        {
                            if ((inds[j] + 1) < vars[j].Count) 
                            {
                                inds[j]++;
                                break;
                            }
                            else 
                                inds[j] = 0;
                        }
                        if (j < 0) 
                            break;
                    }
                }
            }
            return res;
        }
        static Pullenti.Ner.Processor m_Proc0;
    }
}