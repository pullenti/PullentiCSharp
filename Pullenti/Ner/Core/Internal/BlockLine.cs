/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core.Internal
{
    public class BlockLine : Pullenti.Ner.MetaToken
    {
        public BlockLine(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public bool IsAllUpper;
        public bool HasVerb;
        public bool IsExistName;
        public bool HasContentItemTail;
        public int Words;
        public int NotWords;
        public Pullenti.Ner.Token NumberEnd;
        public BlkTyps Typ = BlkTyps.Undefined;
        public static BlockLine Create(Pullenti.Ner.Token t, Pullenti.Ner.Core.TerminCollection names)
        {
            if (t == null) 
                return null;
            BlockLine res = new BlockLine(t, t);
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (tt != t && tt.IsNewlineBefore) 
                    break;
                else 
                    res.EndToken = tt;
            }
            int nums = 0;
            while (t != null && t.Next != null && t.EndChar <= res.EndChar) 
            {
                if (t is Pullenti.Ner.NumberToken) 
                {
                }
                else 
                {
                    Pullenti.Ner.NumberToken rom = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t);
                    if (rom != null && rom.EndToken.Next != null) 
                        t = rom.EndToken;
                    else 
                        break;
                }
                if (t.Next.IsChar('.')) 
                {
                }
                else if ((t.Next is Pullenti.Ner.TextToken) && !t.Next.Chars.IsAllLower) 
                {
                }
                else 
                    break;
                res.NumberEnd = t;
                t = t.Next;
                if (t.IsChar('.') && t.Next != null) 
                {
                    res.NumberEnd = t;
                    t = t.Next;
                }
                if (t.IsNewlineBefore) 
                    return res;
                nums++;
            }
            Pullenti.Ner.Core.TerminToken tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok == null) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt1 != null && npt1.EndToken != npt1.BeginToken) 
                    tok = m_Ontology.TryParse(npt1.Noun.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No);
            }
            if (tok != null) 
            {
                if (t.Previous != null && t.Previous.IsChar(':')) 
                    tok = null;
            }
            if (tok != null) 
            {
                BlkTyps typ = (BlkTyps)tok.Termin.Tag;
                if (typ == BlkTyps.Conslusion) 
                {
                    if (t.IsNewlineAfter) 
                    {
                    }
                    else if (t.Next != null && t.Next.Morph.Class.IsPreposition && t.Next.Next != null) 
                    {
                        Pullenti.Ner.Core.TerminToken tok2 = m_Ontology.TryParse(t.Next.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok2 != null && ((BlkTyps)tok2.Termin.Tag) == BlkTyps.Chapter) 
                        {
                        }
                        else 
                            tok = null;
                    }
                    else 
                        tok = null;
                }
                if (t.Kit.BaseLanguage != t.Morph.Language) 
                    tok = null;
                if (typ == BlkTyps.Index && !t.IsValue("ОГЛАВЛЕНИЕ", null)) 
                {
                    if (!t.IsNewlineAfter && t.Next != null) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null && npt.IsNewlineAfter && npt.Morph.Case.IsGenitive) 
                            tok = null;
                        else if (npt == null) 
                            tok = null;
                    }
                }
                if ((typ == BlkTyps.Intro && tok != null && !tok.IsNewlineAfter) && t.IsValue("ВВЕДЕНИЕ", null)) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.Morph.Case.IsGenitive) 
                        tok = null;
                }
                if (tok != null) 
                {
                    if (res.NumberEnd == null) 
                    {
                        res.NumberEnd = tok.EndToken;
                        if (res.NumberEnd.EndChar > res.EndChar) 
                            res.EndToken = res.NumberEnd;
                    }
                    res.Typ = typ;
                    t = tok.EndToken;
                    if (t.Next != null && t.Next.IsCharOf(":.")) 
                    {
                        t = t.Next;
                        res.EndToken = t;
                    }
                    if (t.IsNewlineAfter || t.Next == null) 
                        return res;
                    t = t.Next;
                }
            }
            if (t.IsChar('§') && (t.Next is Pullenti.Ner.NumberToken)) 
            {
                res.Typ = BlkTyps.Chapter;
                res.NumberEnd = t;
                t = t.Next;
            }
            if (names != null) 
            {
                Pullenti.Ner.Core.TerminToken tok2 = names.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok2 != null && tok2.EndToken.IsNewlineAfter) 
                {
                    res.EndToken = tok2.EndToken;
                    res.IsExistName = true;
                    if (res.Typ == BlkTyps.Undefined) 
                    {
                        BlockLine li2 = Create((res.NumberEnd == null ? null : res.NumberEnd.Next), null);
                        if (li2 != null && ((li2.Typ == BlkTyps.Literature || li2.Typ == BlkTyps.Intro || li2.Typ == BlkTyps.Conslusion))) 
                            res.Typ = li2.Typ;
                        else 
                            res.Typ = BlkTyps.Chapter;
                    }
                    return res;
                }
            }
            Pullenti.Ner.Token t1 = res.EndToken;
            if ((((t1 is Pullenti.Ner.NumberToken) || t1.IsChar('.'))) && t1.Previous != null) 
            {
                t1 = t1.Previous;
                if (t1.IsChar('.')) 
                {
                    res.HasContentItemTail = true;
                    for (; t1 != null && t1.BeginChar > res.BeginChar; t1 = t1.Previous) 
                    {
                        if (!t1.IsChar('.')) 
                            break;
                    }
                }
            }
            res.IsAllUpper = true;
            for (; t != null && t.EndChar <= t1.EndChar; t = t.Next) 
            {
                if (!(t is Pullenti.Ner.TextToken) || !t.Chars.IsLetter) 
                    res.NotWords++;
                else 
                {
                    Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                    if (mc.IsUndefined) 
                        res.NotWords++;
                    else if (t.LengthChar > 2) 
                        res.Words++;
                    if (!t.Chars.IsAllUpper) 
                        res.IsAllUpper = false;
                    if ((t as Pullenti.Ner.TextToken).IsPureVerb) 
                    {
                        if (!(t as Pullenti.Ner.TextToken).Term.EndsWith("ING")) 
                            res.HasVerb = true;
                    }
                }
            }
            if (res.Typ == BlkTyps.Undefined) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse((res.NumberEnd == null ? res.BeginToken : res.NumberEnd.Next), Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    if (npt.Noun.IsValue("ХАРАКТЕРИСТИКА", null) || npt.Noun.IsValue("СОДЕРЖАНИЕ", "ЗМІСТ")) 
                    {
                        bool ok = true;
                        for (Pullenti.Ner.Token tt = npt.EndToken.Next; tt != null && tt.EndChar <= res.EndChar; tt = tt.Next) 
                        {
                            if (tt.IsChar('.')) 
                                continue;
                            Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt2 == null || !npt2.Morph.Case.IsGenitive) 
                            {
                                ok = false;
                                break;
                            }
                            tt = npt2.EndToken;
                            if (tt.EndChar > res.EndChar) 
                            {
                                res.EndToken = tt;
                                if (!tt.IsNewlineAfter) 
                                {
                                    for (; res.EndToken.Next != null; res.EndToken = res.EndToken.Next) 
                                    {
                                        if (res.EndToken.IsNewlineAfter) 
                                            break;
                                    }
                                }
                            }
                        }
                        if (ok) 
                        {
                            res.Typ = BlkTyps.Intro;
                            res.IsExistName = true;
                        }
                    }
                    else if (npt.Noun.IsValue("ВЫВОД", "ВИСНОВОК") || npt.Noun.IsValue("РЕЗУЛЬТАТ", "ДОСЛІДЖЕННЯ")) 
                    {
                        bool ok = true;
                        for (Pullenti.Ner.Token tt = npt.EndToken.Next; tt != null && tt.EndChar <= res.EndChar; tt = tt.Next) 
                        {
                            if (tt.IsCharOf(",.") || tt.IsAnd) 
                                continue;
                            Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt1 != null) 
                            {
                                if (npt1.Noun.IsValue("РЕЗУЛЬТАТ", "ДОСЛІДЖЕННЯ") || npt1.Noun.IsValue("РЕКОМЕНДАЦИЯ", "РЕКОМЕНДАЦІЯ") || npt1.Noun.IsValue("ИССЛЕДОВАНИЕ", "ДОСЛІДЖЕННЯ")) 
                                {
                                    tt = npt1.EndToken;
                                    if (tt.EndChar > res.EndChar) 
                                    {
                                        res.EndToken = tt;
                                        if (!tt.IsNewlineAfter) 
                                        {
                                            for (; res.EndToken.Next != null; res.EndToken = res.EndToken.Next) 
                                            {
                                                if (res.EndToken.IsNewlineAfter) 
                                                    break;
                                            }
                                        }
                                    }
                                    continue;
                                }
                            }
                            ok = false;
                            break;
                        }
                        if (ok) 
                        {
                            res.Typ = BlkTyps.Conslusion;
                            res.IsExistName = true;
                        }
                    }
                    if (res.Typ == BlkTyps.Undefined && npt != null && npt.EndChar <= res.EndChar) 
                    {
                        bool ok = false;
                        int publ = 0;
                        if (_isPub(npt)) 
                        {
                            ok = true;
                            publ = 1;
                        }
                        else if ((npt.Noun.IsValue("СПИСОК", null) || npt.Noun.IsValue("УКАЗАТЕЛЬ", "ПОКАЖЧИК") || npt.Noun.IsValue("ПОЛОЖЕНИЕ", "ПОЛОЖЕННЯ")) || npt.Noun.IsValue("ВЫВОД", "ВИСНОВОК") || npt.Noun.IsValue("РЕЗУЛЬТАТ", "ДОСЛІДЖЕННЯ")) 
                        {
                            if (npt.EndChar == res.EndChar) 
                                return null;
                            ok = true;
                        }
                        if (ok) 
                        {
                            if (npt.BeginToken == npt.EndToken && npt.Noun.IsValue("СПИСОК", null) && npt.EndChar == res.EndChar) 
                                ok = false;
                            for (Pullenti.Ner.Token tt = npt.EndToken.Next; tt != null && tt.EndChar <= res.EndChar; tt = tt.Next) 
                            {
                                if (tt.IsCharOf(",.:") || tt.IsAnd || tt.Morph.Class.IsPreposition) 
                                    continue;
                                if (tt.IsValue("ОТРАЖЕНЫ", "ВІДОБРАЖЕНІ")) 
                                    continue;
                                npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt == null) 
                                {
                                    ok = false;
                                    break;
                                }
                                if (((_isPub(npt) || npt.Noun.IsValue("РАБОТА", "РОБОТА") || npt.Noun.IsValue("ИССЛЕДОВАНИЕ", "ДОСЛІДЖЕННЯ")) || npt.Noun.IsValue("АВТОР", null) || npt.Noun.IsValue("ТРУД", "ПРАЦЯ")) || npt.Noun.IsValue("ТЕМА", null) || npt.Noun.IsValue("ДИССЕРТАЦИЯ", "ДИСЕРТАЦІЯ")) 
                                {
                                    tt = npt.EndToken;
                                    if (_isPub(npt)) 
                                        publ++;
                                    if (tt.EndChar > res.EndChar) 
                                    {
                                        res.EndToken = tt;
                                        if (!tt.IsNewlineAfter) 
                                        {
                                            for (; res.EndToken.Next != null; res.EndToken = res.EndToken.Next) 
                                            {
                                                if (res.EndToken.IsNewlineAfter) 
                                                    break;
                                            }
                                        }
                                    }
                                    continue;
                                }
                                ok = false;
                                break;
                            }
                            if (ok) 
                            {
                                res.Typ = BlkTyps.Literature;
                                res.IsExistName = true;
                                if (publ == 0 && (res.EndChar < (((res.Kit.Sofa.Text.Length * 2) / 3)))) 
                                {
                                    if (res.NumberEnd != null) 
                                        res.Typ = BlkTyps.Misc;
                                    else 
                                        res.Typ = BlkTyps.Undefined;
                                }
                            }
                        }
                    }
                }
            }
            return res;
        }
        static bool _isPub(Pullenti.Ner.Core.NounPhraseToken t)
        {
            if (t == null) 
                return false;
            if (((t.Noun.IsValue("ПУБЛИКАЦИЯ", "ПУБЛІКАЦІЯ") || t.Noun.IsValue("REFERENCE", null) || t.Noun.IsValue("ЛИТЕРАТУРА", "ЛІТЕРАТУРА")) || t.Noun.IsValue("ИСТОЧНИК", "ДЖЕРЕЛО") || t.Noun.IsValue("БИБЛИОГРАФИЯ", "БІБЛІОГРАФІЯ")) || t.Noun.IsValue("ДОКУМЕНТ", null)) 
                return true;
            foreach (Pullenti.Ner.MetaToken a in t.Adjectives) 
            {
                if (a.IsValue("БИБЛИОГРАФИЧЕСКИЙ", null)) 
                    return true;
            }
            return false;
        }
        static Pullenti.Ner.Core.TerminCollection m_Ontology;
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"СОДЕРЖАНИЕ", "СОДЕРЖИМОЕ", "ОГЛАВЛЕНИЕ", "ПЛАН", "PLAN", "ЗМІСТ", "CONTENTS", "INDEX"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = BlkTyps.Index });
            }
            foreach (string s in new string[] {"ГЛАВА", "CHAPTER", "РАЗДЕЛ", "ПАРАГРАФ", "VOLUME", "SECTION", "РОЗДІЛ"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = BlkTyps.Chapter });
            }
            foreach (string s in new string[] {"ВВЕДЕНИЕ", "ВСТУПЛЕНИЕ", "ПРЕДИСЛОВИЕ", "INTRODUCTION"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = BlkTyps.Intro });
            }
            foreach (string s in new string[] {"ВСТУП", "ПЕРЕДМОВА"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.UA) { Tag = BlkTyps.Intro });
            }
            foreach (string s in new string[] {"ВЫВОДЫ", "ВЫВОД", "ЗАКЛЮЧЕНИЕ", "CONCLUSION", "ВИСНОВОК", "ВИСНОВКИ"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = BlkTyps.Conslusion });
            }
            foreach (string s in new string[] {"ПРИЛОЖЕНИЕ", "APPENDIX", "ДОДАТОК"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = BlkTyps.Appendix });
            }
            foreach (string s in new string[] {"СПИСОК СОКРАЩЕНИЙ", "СПИСОК УСЛОВНЫХ СОКРАЩЕНИЙ", "СПИСОК ИСПОЛЬЗУЕМЫХ СОКРАЩЕНИЙ", "УСЛОВНЫЕ СОКРАЩЕНИЯ", "ОБЗОР ЛИТЕРАТУРЫ", "АННОТАЦИЯ", "ANNOTATION", "БЛАГОДАРНОСТИ", "SUPPLEMENT", "ABSTRACT", "СПИСОК СКОРОЧЕНЬ", "ПЕРЕЛІК УМОВНИХ СКОРОЧЕНЬ", "СПИСОК ВИКОРИСТОВУВАНИХ СКОРОЧЕНЬ", "УМОВНІ СКОРОЧЕННЯ", "ОГЛЯД ЛІТЕРАТУРИ", "АНОТАЦІЯ", "ПОДЯКИ"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = BlkTyps.Misc });
            }
        }
    }
}