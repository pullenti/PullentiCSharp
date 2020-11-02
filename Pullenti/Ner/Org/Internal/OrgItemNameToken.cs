/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Org.Internal
{
    class OrgItemNameToken : Pullenti.Ner.MetaToken
    {
        public OrgItemNameToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public string Value;
        public bool IsNounPhrase;
        public bool IsDenomination;
        public bool IsInDictionary;
        /// <summary>
        /// Стандартное окончание (типа Ltd, Inc)
        /// </summary>
        public bool IsStdTail;
        /// <summary>
        /// Стандартное название (типа "Разработки ПО", ")
        /// </summary>
        public bool IsStdName;
        /// <summary>
        /// Это паразитные слова типа "Заказчик", "Вкладчик" и т.п.
        /// </summary>
        public bool IsEmptyWord;
        /// <summary>
        /// Это "паразитная" комбинация типа "ордена Трудового Красного знамени"
        /// </summary>
        public bool IsIgnoredPart;
        /// <summary>
        /// Имя состоит из слов, которыми обычно называются госучреждения, министерства, департаменты и т.п.
        /// </summary>
        public int StdOrgNameNouns = 0;
        /// <summary>
        /// Стандартный профиль ...
        /// </summary>
        public Pullenti.Ner.Org.OrgProfile OrgStdProf = Pullenti.Ner.Org.OrgProfile.Undefined;
        public bool IsAfterConjunction;
        public string Preposition;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder(Value);
            if (IsNounPhrase) 
                res.Append(" NounPrase");
            if (IsDenomination) 
                res.Append(" Denom");
            if (IsInDictionary) 
                res.Append(" InDictionary");
            if (IsAfterConjunction) 
                res.Append(" IsAfterConjunction");
            if (IsStdTail) 
                res.Append(" IsStdTail");
            if (IsStdName) 
                res.Append(" IsStdName");
            if (IsIgnoredPart) 
                res.Append(" IsIgnoredPart");
            if (Preposition != null) 
                res.AppendFormat(" IsAfterPreposition '{0}'", Preposition);
            res.AppendFormat(" {0} ({1})", Chars.ToString(), this.GetSourceText());
            return res.ToString();
        }
        public static OrgItemNameToken TryAttach(Pullenti.Ner.Token t, OrgItemNameToken prev, bool extOnto, bool first)
        {
            if (t == null) 
                return null;
            if (t.IsValue("ОРДЕНА", null) && t.Next != null) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    Pullenti.Ner.Token t1 = npt.EndToken;
                    if (((t1.IsValue("ЗНАК", null) || t1.IsValue("ДРУЖБА", null))) && (t1.WhitespacesAfterCount < 2)) 
                    {
                        npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t1.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null) 
                            t1 = npt.EndToken;
                    }
                    return new OrgItemNameToken(t, t1) { IsIgnoredPart = true };
                }
                if (t.Next.GetMorphClassInDictionary().IsProperSurname) 
                    return new OrgItemNameToken(t, t.Next) { IsIgnoredPart = true };
                Pullenti.Ner.ReferentToken ppp = t.Kit.ProcessReferent("PERSON", t.Next);
                if (ppp != null) 
                    return new OrgItemNameToken(t, ppp.EndToken) { IsIgnoredPart = true };
                if ((t.WhitespacesAfterCount < 2) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Next, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t.Next, Pullenti.Ner.Core.BracketParseAttr.NearCloseBracket, 10);
                    if (br != null && (br.LengthChar < 40)) 
                        return new OrgItemNameToken(t, br.EndToken) { IsIgnoredPart = true };
                }
            }
            if (first && t.Chars.IsCyrillicLetter && t.Morph.Class.IsPreposition) 
            {
                if (!t.IsValue("ПО", null) && !t.IsValue("ПРИ", null)) 
                    return null;
            }
            OrgItemNameToken res = _TryAttach(t, prev, extOnto);
            if (res == null) 
            {
                if (extOnto) 
                {
                    if ((t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) || (((t is Pullenti.Ner.TextToken) && !t.IsChar(';')))) 
                        return new OrgItemNameToken(t, t) { Value = t.GetSourceText() };
                }
                return null;
            }
            if (prev == null && !extOnto) 
            {
                if (t.Kit.Ontology != null) 
                {
                    Pullenti.Ner.Org.OrganizationAnalyzer.OrgAnalyzerData ad = t.Kit.Ontology._getAnalyzerData(Pullenti.Ner.Org.OrganizationAnalyzer.ANALYZER_NAME) as Pullenti.Ner.Org.OrganizationAnalyzer.OrgAnalyzerData;
                    if (ad != null) 
                    {
                        Pullenti.Ner.Core.TerminToken tok = ad.OrgPureNames.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok != null && tok.EndChar > res.EndChar) 
                            res.EndToken = tok.EndToken;
                    }
                }
            }
            if (prev != null && !extOnto) 
            {
                if ((prev.Chars.IsAllLower && !res.Chars.IsAllLower && !res.IsStdTail) && !res.IsStdName) 
                {
                    if (prev.Chars.IsLatinLetter && res.Chars.IsLatinLetter) 
                    {
                    }
                    else if (m_StdNouns.TryParse(res.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                    {
                    }
                    else 
                        return null;
                }
            }
            if ((res.EndToken.Next != null && !res.EndToken.IsWhitespaceAfter && res.EndToken.Next.IsHiphen) && !res.EndToken.Next.IsWhitespaceAfter) 
            {
                Pullenti.Ner.TextToken tt = res.EndToken.Next.Next as Pullenti.Ner.TextToken;
                if (tt != null) 
                {
                    if (tt.Chars == res.Chars || tt.Chars.IsAllUpper) 
                    {
                        res.EndToken = tt;
                        res.Value = string.Format("{0}-{1}", res.Value, tt.Term);
                    }
                }
            }
            if ((res.EndToken.Next != null && res.EndToken.Next.IsAnd && res.EndToken.WhitespacesAfterCount == 1) && res.EndToken.Next.WhitespacesAfterCount == 1) 
            {
                OrgItemNameToken res1 = _TryAttach(res.EndToken.Next.Next, prev, extOnto);
                if (res1 != null && res1.Chars == res.Chars && OrgItemTypeToken.TryAttach(res.EndToken.Next.Next, false, null) == null) 
                {
                    if (!((res1.Morph.Case & res.Morph.Case)).IsUndefined) 
                    {
                        res.EndToken = res1.EndToken;
                        res.Value = string.Format("{0} {1} {2}", res.Value, (res.Kit.BaseLanguage.IsUa ? "ТА" : "И"), res1.Value);
                    }
                }
            }
            for (Pullenti.Ner.Token tt = res.BeginToken; tt != null && tt.EndChar <= res.EndChar; tt = tt.Next) 
            {
                if (m_StdNouns.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                    res.StdOrgNameNouns++;
            }
            if (m_StdNouns.TryParse(res.EndToken, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
            {
                int cou = 1;
                bool non = false;
                Pullenti.Ner.Token et = res.EndToken;
                if (!_isNotTermNoun(res.EndToken)) 
                    non = true;
                bool br = false;
                for (Pullenti.Ner.Token tt = res.EndToken.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsTableControlChar) 
                        break;
                    if (tt.IsChar('(')) 
                    {
                        if (!non) 
                            break;
                        br = true;
                        continue;
                    }
                    if (tt.IsChar(')')) 
                    {
                        br = false;
                        et = tt;
                        break;
                    }
                    if (!(tt is Pullenti.Ner.TextToken)) 
                        break;
                    if (tt.WhitespacesBeforeCount > 1) 
                    {
                        if (tt.NewlinesBeforeCount > 1) 
                            break;
                        if (tt.Chars != res.EndToken.Chars) 
                            break;
                    }
                    if (tt.Morph.Class.IsPreposition || tt.IsCommaAnd) 
                        continue;
                    Pullenti.Morph.MorphClass dd = tt.GetMorphClassInDictionary();
                    if (!dd.IsNoun && !dd.IsAdjective) 
                        break;
                    Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt2 == null) 
                    {
                        if (dd == Pullenti.Morph.MorphClass.Adjective) 
                            continue;
                        break;
                    }
                    if (m_StdNouns.TryParse(npt2.EndToken, Pullenti.Ner.Core.TerminParseAttr.No) == null) 
                        break;
                    if (npt2.EndToken.Chars != res.EndToken.Chars) 
                        break;
                    if ((npt2.EndToken.IsValue("УПРАВЛЕНИЕ", null) || npt2.EndToken.IsValue("ИНСТИТУТ", null) || npt2.EndToken.IsValue("УПРАВЛІННЯ", null)) || npt2.EndToken.IsValue("ІНСТИТУТ", null) || tt.Previous.IsValue("ПРИ", null)) 
                    {
                        Pullenti.Ner.ReferentToken rt = tt.Kit.ProcessReferent(Pullenti.Ner.Org.OrganizationAnalyzer.ANALYZER_NAME, tt);
                        if (rt != null) 
                            break;
                    }
                    cou++;
                    tt = npt2.EndToken;
                    if (!_isNotTermNoun(tt)) 
                    {
                        non = true;
                        et = tt;
                    }
                }
                if (non && !br) 
                {
                    res.StdOrgNameNouns += cou;
                    res.EndToken = et;
                }
            }
            return res;
        }
        static List<string> m_NotTerminateNouns = new List<string>(new string[] {"РАБОТА", "ВОПРОС", "ДЕЛО", "УПРАВЛЕНИЕ", "ОРГАНИЗАЦИЯ", "ОБЕСПЕЧЕНИЕ", "РОБОТА", "ПИТАННЯ", "СПРАВА", "УПРАВЛІННЯ", "ОРГАНІЗАЦІЯ", "ЗАБЕЗПЕЧЕННЯ"});
        static bool _isNotTermNoun(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return false;
            if (!(t.Previous is Pullenti.Ner.TextToken)) 
                return false;
            if ((t.Previous as Pullenti.Ner.TextToken).Term != "ПО") 
                return false;
            foreach (string v in m_NotTerminateNouns) 
            {
                if (t.IsValue(v, null)) 
                    return true;
            }
            return false;
        }
        static OrgItemNameToken _TryAttach(Pullenti.Ner.Token t, OrgItemNameToken prev, bool extOnto)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Referent r = t.GetReferent();
            if (r != null) 
            {
                if (r.TypeName == "DENOMINATION") 
                    return new OrgItemNameToken(t, t) { Value = r.ToString(true, t.Kit.BaseLanguage, 0), IsDenomination = true };
                if ((r is Pullenti.Ner.Geo.GeoReferent) && t.Chars.IsLatinLetter) 
                {
                    OrgItemNameToken res2 = _TryAttach(t.Next, prev, extOnto);
                    if (res2 != null && res2.Chars.IsLatinLetter) 
                    {
                        res2.BeginToken = t;
                        res2.Value = string.Format("{0} {1}", Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(t as Pullenti.Ner.MetaToken, Pullenti.Ner.Core.GetTextAttr.No), res2.Value);
                        res2.IsInDictionary = false;
                        return res2;
                    }
                }
                return null;
            }
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return null;
            OrgItemNameToken res = null;
            Pullenti.Ner.Core.TerminToken tok = m_StdTails.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok == null && t.IsChar(',')) 
                tok = m_StdTails.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null) 
                return new OrgItemNameToken(t, tok.EndToken) { Value = tok.Termin.CanonicText, IsStdTail = tok.Termin.Tag == null, IsEmptyWord = tok.Termin.Tag != null, Morph = tok.Morph };
            if ((((tok = m_StdNames.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No)))) != null) 
                return new OrgItemNameToken(t, tok.EndToken) { Value = tok.Termin.CanonicText, IsStdName = true };
            OrgItemEngItem eng = OrgItemEngItem.TryAttach(t, false);
            if (eng == null && t.IsChar(',')) 
                eng = OrgItemEngItem.TryAttach(t.Next, false);
            if (eng != null) 
                return new OrgItemNameToken(t, eng.EndToken) { Value = eng.FullValue, IsStdTail = true };
            if (tt.Chars.IsAllLower && prev != null) 
            {
                if (!prev.Chars.IsAllLower && !prev.Chars.IsCapitalUpper) 
                    return null;
            }
            if (tt.IsChar(',') && prev != null) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt1 == null || npt1.Chars != prev.Chars || ((npt1.Morph.Case & prev.Morph.Case)).IsUndefined) 
                    return null;
                OrgItemTypeToken ty = OrgItemTypeToken.TryAttach(t.Next, false, null);
                if (ty != null) 
                    return null;
                if (npt1.EndToken.Next == null || !npt1.EndToken.Next.IsValue("И", null)) 
                    return null;
                Pullenti.Ner.Token t1 = npt1.EndToken.Next;
                Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t1.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt2 == null || npt2.Chars != prev.Chars || ((npt2.Morph.Case & npt1.Morph.Case & prev.Morph.Case)).IsUndefined) 
                    return null;
                ty = OrgItemTypeToken.TryAttach(t1.Next, false, null);
                if (ty != null) 
                    return null;
                res = new OrgItemNameToken(npt1.BeginToken, npt1.EndToken) { Morph = npt1.Morph, Value = npt1.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) };
                res.IsNounPhrase = true;
                res.IsAfterConjunction = true;
                if (prev.Preposition != null) 
                    res.Preposition = prev.Preposition;
                return res;
            }
            if (((tt.IsChar('&') || tt.IsValue("AND", null) || tt.IsValue("UND", null))) && prev != null) 
            {
                if ((tt.Next is Pullenti.Ner.TextToken) && tt.LengthChar == 1 && tt.Next.Chars.IsLatinLetter) 
                {
                    res = new OrgItemNameToken(tt, tt.Next) { Chars = tt.Next.Chars };
                    res.IsAfterConjunction = true;
                    res.Value = "& " + (tt.Next as Pullenti.Ner.TextToken).Term;
                    return res;
                }
                res = OrgItemNameToken.TryAttach(tt.Next, null, extOnto, false);
                if (res == null || res.Chars != prev.Chars) 
                    return null;
                res.IsAfterConjunction = true;
                res.Value = "& " + res.Value;
                return res;
            }
            if (!tt.Chars.IsLetter) 
                return null;
            List<Pullenti.Semantic.Utils.DerivateGroup> expinf = null;
            if (prev != null && prev.EndToken.GetMorphClassInDictionary().IsNoun) 
            {
                string wo = prev.EndToken.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                expinf = Pullenti.Semantic.Utils.DerivateService.FindDerivates(wo, true, prev.EndToken.Morph.Language);
            }
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
            if (npt != null && npt.InternalNoun != null) 
                npt = null;
            bool explOk = false;
            if (npt != null && prev != null && prev.EndToken.GetMorphClassInDictionary().IsNoun) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt0 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(prev.EndToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt0 != null) 
                {
                    List<Pullenti.Semantic.Core.SemanticLink> links = Pullenti.Semantic.Core.SemanticHelper.TryCreateLinks(npt0, npt, null);
                    if (links.Count > 0) 
                        explOk = true;
                }
            }
            if (npt != null && ((explOk || npt.Morph.Case.IsGenitive || ((prev != null && !((prev.Morph.Case & npt.Morph.Case)).IsUndefined))))) 
            {
                Pullenti.Morph.MorphClass mc = npt.BeginToken.GetMorphClassInDictionary();
                if (mc.IsVerb || mc.IsPronoun) 
                    return null;
                if (mc.IsAdverb) 
                {
                    if (npt.BeginToken.Next != null && npt.BeginToken.Next.IsHiphen) 
                    {
                    }
                    else 
                        return null;
                }
                if (mc.IsPreposition) 
                    return null;
                if (mc.IsNoun && npt.Chars.IsAllLower) 
                {
                    Pullenti.Morph.MorphCase ca = npt.Morph.Case;
                    if ((!ca.IsDative && !ca.IsGenitive && !ca.IsInstrumental) && !ca.IsPrepositional) 
                        return null;
                }
                res = new OrgItemNameToken(npt.BeginToken, npt.EndToken) { Morph = npt.Morph, Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) };
                res.IsNounPhrase = true;
                if ((npt.EndToken.WhitespacesAfterCount < 2) && (npt.EndToken.Next is Pullenti.Ner.TextToken)) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(npt.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt2 != null && npt2.Morph.Case.IsGenitive && npt2.Chars.IsAllLower) 
                    {
                        OrgItemTypeToken typ = OrgItemTypeToken.TryAttach(npt.EndToken.Next, true, null);
                        OrgItemEponymToken epo = OrgItemEponymToken.TryAttach(npt.EndToken.Next, false);
                        Pullenti.Ner.ReferentToken rtt = t.Kit.ProcessReferent("PERSONPROPERTY", npt.EndToken.Next);
                        if (typ == null && epo == null && ((rtt == null || rtt.Morph.Number == Pullenti.Morph.MorphNumber.Plural))) 
                        {
                            res.EndToken = npt2.EndToken;
                            res.Value = string.Format("{0} {1}", res.Value, Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(npt2, Pullenti.Ner.Core.GetTextAttr.No));
                        }
                    }
                    else if (npt.EndToken.Next.IsComma && (npt.EndToken.Next.Next is Pullenti.Ner.TextToken)) 
                    {
                        Pullenti.Ner.Token tt2 = npt.EndToken.Next.Next;
                        Pullenti.Morph.MorphClass mv2 = tt2.GetMorphClassInDictionary();
                        if (mv2.IsAdjective && mv2.IsVerb) 
                        {
                            Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo() { Case = npt.Morph.Case, Gender = npt.Morph.Gender, Number = npt.Morph.Number };
                            if (tt2.Morph.CheckAccord(bi, false, false)) 
                            {
                                npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt2.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt2 != null && ((npt2.Morph.Case.IsDative || npt2.Morph.Case.IsGenitive)) && npt2.Chars.IsAllLower) 
                                {
                                    res.EndToken = npt2.EndToken;
                                    res.Value = string.Format("{0} {1}", res.Value, Pullenti.Ner.Core.MiscHelper.GetTextValue(npt.EndToken.Next, res.EndToken, Pullenti.Ner.Core.GetTextAttr.No));
                                }
                            }
                        }
                    }
                }
                if (explOk) 
                    res.IsAfterConjunction = true;
            }
            else if (npt != null && ((((prev != null && prev.IsNounPhrase && npt.Morph.Case.IsInstrumental)) || extOnto))) 
            {
                res = new OrgItemNameToken(npt.BeginToken, npt.EndToken) { Morph = npt.Morph, Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) };
                res.IsNounPhrase = true;
            }
            else if (tt.IsAnd) 
            {
                res = TryAttach(tt.Next, prev, extOnto, false);
                if (res == null || !res.IsNounPhrase || prev == null) 
                    return null;
                if (((prev.Morph.Case & res.Morph.Case)).IsUndefined) 
                    return null;
                if (prev.Morph.Number != Pullenti.Morph.MorphNumber.Undefined && res.Morph.Number != Pullenti.Morph.MorphNumber.Undefined) 
                {
                    if (((prev.Morph.Number & res.Morph.Number)) == Pullenti.Morph.MorphNumber.Undefined) 
                    {
                        if (prev.Chars != res.Chars) 
                            return null;
                        OrgItemTypeToken ty = OrgItemTypeToken.TryAttach(res.EndToken.Next, false, null);
                        if (ty != null) 
                            return null;
                    }
                }
                Pullenti.Morph.CharsInfo ci = res.Chars;
                res.Chars = ci;
                res.IsAfterConjunction = true;
                return res;
            }
            else if (((tt.Term == "ПО" || tt.Term == "ПРИ" || tt.Term == "ЗА") || tt.Term == "С" || tt.Term == "В") || tt.Term == "НА") 
            {
                npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    if (m_VervotWords.TryParse(npt.EndToken, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                        return null;
                    bool ok = false;
                    if (tt.Term == "ПО") 
                        ok = npt.Morph.Case.IsDative;
                    else if (tt.Term == "С") 
                        ok = npt.Morph.Case.IsInstrumental;
                    else if (tt.Term == "ЗА") 
                        ok = npt.Morph.Case.IsGenitive | npt.Morph.Case.IsInstrumental;
                    else if (tt.Term == "НА") 
                        ok = npt.Morph.Case.IsPrepositional;
                    else if (tt.Term == "В") 
                    {
                        ok = npt.Morph.Case.IsDative | npt.Morph.Case.IsPrepositional;
                        if (ok) 
                        {
                            ok = false;
                            if (t.Next.IsValue("СФЕРА", null) || t.Next.IsValue("ОБЛАСТЬ", null)) 
                                ok = true;
                        }
                    }
                    else if (tt.Term == "ПРИ") 
                    {
                        ok = npt.Morph.Case.IsPrepositional;
                        if (ok) 
                        {
                            if (OrgItemTypeToken.TryAttach(tt.Next, true, null) != null) 
                                ok = false;
                            else 
                            {
                                Pullenti.Ner.ReferentToken rt = tt.Kit.ProcessReferent(Pullenti.Ner.Org.OrganizationAnalyzer.ANALYZER_NAME, tt.Next);
                                if (rt != null) 
                                    ok = false;
                            }
                        }
                        string s = npt.Noun.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                        if (s == "ПОДДЕРЖКА" || s == "УЧАСТИЕ") 
                            ok = false;
                    }
                    else 
                        ok = npt.Morph.Case.IsPrepositional;
                    if (ok) 
                    {
                        res = new OrgItemNameToken(t, npt.EndToken) { Morph = npt.Morph, Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false), Chars = npt.Chars };
                        res.IsNounPhrase = true;
                        res.Preposition = tt.Term;
                        if (((res.Value == "ДЕЛО" || res.Value == "ВОПРОС")) && !res.IsNewlineAfter) 
                        {
                            OrgItemNameToken res2 = _TryAttach(res.EndToken.Next, res, extOnto);
                            if (res2 != null && res2.Morph.Case.IsGenitive) 
                            {
                                res.Value = string.Format("{0} {1}", res.Value, res2.Value);
                                res.EndToken = res2.EndToken;
                                for (Pullenti.Ner.Token ttt = res2.EndToken.Next; ttt != null; ttt = ttt.Next) 
                                {
                                    if (!ttt.IsCommaAnd) 
                                        break;
                                    OrgItemNameToken res3 = _TryAttach(ttt.Next, res2, extOnto);
                                    if (res3 == null) 
                                        break;
                                    res.Value = string.Format("{0} {1}", res.Value, res3.Value);
                                    res.EndToken = res3.EndToken;
                                    if (ttt.IsAnd) 
                                        break;
                                    ttt = res.EndToken;
                                }
                            }
                        }
                    }
                }
                if (res == null) 
                    return null;
            }
            else if (tt.Term == "OF") 
            {
                Pullenti.Ner.Token t1 = tt.Next;
                if (t1 != null && Pullenti.Ner.Core.MiscHelper.IsEngArticle(t1)) 
                    t1 = t1.Next;
                if (t1 != null && t1.Chars.IsLatinLetter && !t1.Chars.IsAllLower) 
                {
                    res = new OrgItemNameToken(t, t1) { Chars = t1.Chars, Morph = t1.Morph };
                    for (Pullenti.Ner.Token ttt = t1.Next; ttt != null; ttt = ttt.Next) 
                    {
                        if (ttt.WhitespacesBeforeCount > 2) 
                            break;
                        if (Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(ttt)) 
                        {
                            ttt = ttt.Next;
                            continue;
                        }
                        if (!ttt.Chars.IsLatinLetter) 
                            break;
                        if (ttt.Morph.Class.IsPreposition) 
                            break;
                        t1 = (res.EndToken = ttt);
                    }
                    res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1, Pullenti.Ner.Core.GetTextAttr.IgnoreArticles);
                    res.Preposition = tt.Term;
                    return res;
                }
            }
            if (res == null) 
            {
                if (tt.Chars.IsLatinLetter && tt.LengthChar == 1) 
                {
                }
                else if (tt.Chars.IsAllLower || (tt.LengthChar < 2)) 
                {
                    if (!tt.Chars.IsLatinLetter || prev == null || !prev.Chars.IsLatinLetter) 
                        return null;
                }
                if (tt.Chars.IsCyrillicLetter) 
                {
                    Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                    if (mc.IsVerb || mc.IsAdverb) 
                        return null;
                }
                else if (tt.Chars.IsLatinLetter && !tt.IsWhitespaceAfter) 
                {
                    if (!tt.IsWhitespaceAfter && (tt.LengthChar < 5)) 
                    {
                        if (tt.Next is Pullenti.Ner.NumberToken) 
                            return null;
                    }
                }
                res = new OrgItemNameToken(tt, tt) { Value = tt.Term, Morph = tt.Morph };
                for (t = tt.Next; t != null; t = t.Next) 
                {
                    if ((((t.IsHiphen || t.IsCharOf("\\/"))) && t.Next != null && (t.Next is Pullenti.Ner.TextToken)) && !t.IsWhitespaceBefore && !t.IsWhitespaceAfter) 
                    {
                        t = t.Next;
                        res.EndToken = t;
                        res.Value = string.Format("{0}{1}{2}", res.Value, (t.Previous.IsChar('.') ? '.' : '-'), (t as Pullenti.Ner.TextToken).Term);
                    }
                    else if (t.IsChar('.')) 
                    {
                        if (!t.IsWhitespaceAfter && !t.IsWhitespaceBefore && (t.Next is Pullenti.Ner.TextToken)) 
                        {
                            res.EndToken = t.Next;
                            t = t.Next;
                            res.Value = string.Format("{0}.{1}", res.Value, (t as Pullenti.Ner.TextToken).Term);
                        }
                        else if ((t.Next != null && !t.IsNewlineAfter && t.Next.Chars.IsLatinLetter) && tt.Chars.IsLatinLetter) 
                            res.EndToken = t;
                        else 
                            break;
                    }
                    else 
                        break;
                }
            }
            for (Pullenti.Ner.Token t0 = res.BeginToken; t0 != null; t0 = t0.Next) 
            {
                if ((((tt = t0 as Pullenti.Ner.TextToken))) != null && tt.IsLetters) 
                {
                    if (!tt.Morph.Class.IsConjunction && !tt.Morph.Class.IsPreposition) 
                    {
                        foreach (Pullenti.Morph.MorphBaseInfo mf in tt.Morph.Items) 
                        {
                            if ((mf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                                res.IsInDictionary = true;
                        }
                    }
                }
                if (t0 == res.EndToken) 
                    break;
            }
            if (res.BeginToken == res.EndToken && res.BeginToken.Chars.IsAllUpper) 
            {
                if (res.EndToken.Next != null && !res.EndToken.IsWhitespaceAfter) 
                {
                    Pullenti.Ner.Token t1 = res.EndToken.Next;
                    if (t1.Next != null && !t1.IsWhitespaceAfter && t1.IsHiphen) 
                        t1 = t1.Next;
                    if (t1 is Pullenti.Ner.NumberToken) 
                    {
                        res.Value += (t1 as Pullenti.Ner.NumberToken).Value;
                        res.EndToken = t1;
                    }
                }
            }
            if (res.BeginToken == res.EndToken && res.BeginToken.Chars.IsLastLower) 
            {
                string src = res.BeginToken.GetSourceText();
                for (int i = src.Length - 1; i >= 0; i--) 
                {
                    if (char.IsUpper(src[i])) 
                    {
                        res.Value = src.Substring(0, i + 1);
                        break;
                    }
                }
            }
            return res;
        }
        static Pullenti.Ner.Core.TerminCollection m_StdNames;
        static Pullenti.Ner.Core.TerminCollection m_StdTails;
        static Pullenti.Ner.Core.TerminCollection m_VervotWords;
        static Pullenti.Ner.Core.TerminCollection m_StdNouns;
        internal static Pullenti.Ner.Core.TerminCollection m_DepStdNames;
        public static void Initialize()
        {
            m_StdTails = new Pullenti.Ner.Core.TerminCollection();
            m_StdNames = new Pullenti.Ner.Core.TerminCollection();
            m_VervotWords = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("INCORPORATED");
            t.AddAbridge("INC.");
            m_StdTails.Add(t);
            t = new Pullenti.Ner.Core.Termin("CORPORATION");
            t.AddAbridge("CORP.");
            m_StdTails.Add(t);
            t = new Pullenti.Ner.Core.Termin("LIMITED");
            t.AddAbridge("LTD.");
            m_StdTails.Add(t);
            t = new Pullenti.Ner.Core.Termin("AG");
            m_StdTails.Add(t);
            t = new Pullenti.Ner.Core.Termin("GMBH");
            m_StdTails.Add(t);
            foreach (string s in new string[] {"ЗАКАЗЧИК", "ИСПОЛНИТЕЛЬ", "РАЗРАБОТЧИК", "БЕНЕФИЦИАР", "ПОЛУЧАТЕЛЬ", "ОТПРАВИТЕЛЬ", "ИЗГОТОВИТЕЛЬ", "ПРОИЗВОДИТЕЛЬ", "ПОСТАВЩИК", "АБОНЕНТ", "КЛИЕНТ", "ВКЛАДЧИК", "СУБЪЕКТ", "ПРОДАВЕЦ", "ПОКУПАТЕЛЬ", "АРЕНДОДАТЕЛЬ", "АРЕНДАТОР", "СУБАРЕНДАТОР", "НАЙМОДАТЕЛЬ", "НАНИМАТЕЛЬ", "АГЕНТ", "ПРИНЦИПАЛ", "ПРОДАВЕЦ", "ПОСТАВЩИК", "ПОДРЯДЧИК", "СУБПОДРЯДЧИК"}) 
            {
                m_StdTails.Add(new Pullenti.Ner.Core.Termin(s) { Tag = s });
            }
            foreach (string s in new string[] {"ЗАМОВНИК", "ВИКОНАВЕЦЬ", "РОЗРОБНИК", "БЕНЕФІЦІАР", "ОДЕРЖУВАЧ", "ВІДПРАВНИК", "ВИРОБНИК", "ВИРОБНИК", "ПОСТАЧАЛЬНИК", "АБОНЕНТ", "КЛІЄНТ", "ВКЛАДНИК", "СУБ'ЄКТ", "ПРОДАВЕЦЬ", "ПОКУПЕЦЬ", "ОРЕНДОДАВЕЦЬ", "ОРЕНДАР", "СУБОРЕНДАР", "НАЙМОДАВЕЦЬ", "НАЙМАЧ", "АГЕНТ", "ПРИНЦИПАЛ", "ПРОДАВЕЦЬ", "ПОСТАЧАЛЬНИК", "ПІДРЯДНИК", "СУБПІДРЯДНИК"}) 
            {
                m_StdTails.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.UA) { Tag = s });
            }
            t = new Pullenti.Ner.Core.Termin("РАЗРАБОТКА ПРОГРАММНОГО ОБЕСПЕЧЕНИЯ");
            t.AddAbridge("РАЗРАБОТКИ ПО");
            m_StdNames.Add(t);
            foreach (string s in new string[] {"СПЕЦИАЛЬНОСТЬ", "ДИАГНОЗ"}) 
            {
                m_VervotWords.Add(new Pullenti.Ner.Core.Termin(s));
            }
            foreach (string s in new string[] {"СПЕЦІАЛЬНІСТЬ", "ДІАГНОЗ"}) 
            {
                m_VervotWords.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.UA));
            }
            m_StdNouns = new Pullenti.Ner.Core.TerminCollection();
            for (int k = 0; k < 2; k++) 
            {
                string name = (k == 0 ? "NameNouns_ru.dat" : "NameNouns_ua.dat");
                byte[] dat = ResourceHelper.GetBytes(name);
                if (dat == null) 
                    throw new Exception(string.Format("Can't file resource file {0} in Organization analyzer", name));
                string str = Encoding.UTF8.GetString(OrgItemTypeToken.Deflate(dat));
                foreach (string line0 in str.Split('\n')) 
                {
                    string line = line0.Trim();
                    if (string.IsNullOrEmpty(line)) 
                        continue;
                    if (k == 0) 
                        m_StdNouns.Add(new Pullenti.Ner.Core.Termin(line));
                    else 
                        m_StdNouns.Add(new Pullenti.Ner.Core.Termin(line) { Lang = Pullenti.Morph.MorphLang.UA });
                }
            }
        }
    }
}