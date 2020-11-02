/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Titlepage.Internal
{
    class TitleItemToken : Pullenti.Ner.MetaToken
    {
        private TitleItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end, Types typ) : base(begin, end, null)
        {
            Typ = typ;
        }
        public enum Types : int
        {
            Undefined,
            Typ,
            Theme,
            TypAndTheme,
            Boss,
            Worker,
            Editor,
            Consultant,
            Opponent,
            OtherRole,
            Translate,
            Adopt,
            Dust,
            Speciality,
            Keywords,
        }

        public Types Typ;
        public string Value;
        public override string ToString()
        {
            return string.Format("{0}: {1}", Typ.ToString(), Value ?? "");
        }
        public static TitleItemToken TryAttach(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt != null) 
            {
                Pullenti.Ner.Token t1 = (Pullenti.Ner.Token)tt;
                if (tt.Term == "ТЕМА") 
                {
                    TitleItemToken tit = TryAttach(tt.Next);
                    if (tit != null && tit.Typ == Types.Typ) 
                    {
                        t1 = tit.EndToken;
                        if (t1.Next != null && t1.Next.IsChar(':')) 
                            t1 = t1.Next;
                        return new TitleItemToken(t, t1, Types.TypAndTheme) { Value = tit.Value };
                    }
                    if (tt.Next != null && tt.Next.IsChar(':')) 
                        t1 = tt.Next;
                    return new TitleItemToken(tt, t1, Types.Theme);
                }
                if (tt.Term == "ПО" || tt.Term == "НА") 
                {
                    if (tt.Next != null && tt.Next.IsValue("ТЕМА", null)) 
                    {
                        t1 = tt.Next;
                        if (t1.Next != null && t1.Next.IsChar(':')) 
                            t1 = t1.Next;
                        return new TitleItemToken(tt, t1, Types.Theme);
                    }
                }
                if (tt.Term == "ПЕРЕВОД" || tt.Term == "ПЕР") 
                {
                    Pullenti.Ner.Token tt2 = tt.Next;
                    if (tt2 != null && tt2.IsChar('.')) 
                        tt2 = tt2.Next;
                    if (tt2 is Pullenti.Ner.TextToken) 
                    {
                        if ((tt2 as Pullenti.Ner.TextToken).Term == "C" || (tt2 as Pullenti.Ner.TextToken).Term == "С") 
                        {
                            tt2 = tt2.Next;
                            if (tt2 is Pullenti.Ner.TextToken) 
                                return new TitleItemToken(t, tt2, Types.Translate);
                        }
                    }
                }
                if (tt.Term == "СЕКЦИЯ" || tt.Term == "SECTION" || tt.Term == "СЕКЦІЯ") 
                {
                    t1 = tt.Next;
                    if (t1 != null && t1.IsChar(':')) 
                        t1 = t1.Next;
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                        t1 = br.EndToken;
                    else if (t1 != tt.Next) 
                    {
                        for (; t1 != null; t1 = t1.Next) 
                        {
                            if (t1.IsNewlineAfter) 
                                break;
                        }
                        if (t1 == null) 
                            return null;
                    }
                    if (t1 != tt.Next) 
                        return new TitleItemToken(tt, t1, Types.Dust);
                }
                t1 = null;
                if (tt.IsValue("СПЕЦИАЛЬНОСТЬ", "СПЕЦІАЛЬНІСТЬ")) 
                    t1 = tt.Next;
                else if (tt.Morph.Class.IsPreposition && tt.Next != null && tt.Next.IsValue("СПЕЦИАЛЬНОСТЬ", "СПЕЦІАЛЬНІСТЬ")) 
                    t1 = tt.Next.Next;
                else if (tt.IsChar('/') && tt.IsNewlineBefore) 
                    t1 = tt.Next;
                if (t1 != null) 
                {
                    if (t1.IsCharOf(":") || t1.IsHiphen) 
                        t1 = t1.Next;
                    TitleItemToken spec = TryAttachSpeciality(t1, true);
                    if (spec != null) 
                    {
                        spec.BeginToken = t;
                        return spec;
                    }
                }
            }
            TitleItemToken sss = TryAttachSpeciality(t, false);
            if (sss != null) 
                return sss;
            if (t is Pullenti.Ner.ReferentToken) 
                return null;
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
            if (npt != null) 
            {
                string s = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(npt.EndToken, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                {
                    Types ty = (Types)tok.Termin.Tag;
                    if (ty == Types.Typ) 
                    {
                        TitleItemToken tit = TryAttach(tok.EndToken.Next);
                        if (tit != null && tit.Typ == Types.Theme) 
                            return new TitleItemToken(npt.BeginToken, tit.EndToken, Types.TypAndTheme) { Value = s };
                        if (s == "РАБОТА" || s == "РОБОТА" || s == "ПРОЕКТ") 
                            return null;
                        Pullenti.Ner.Token t1 = tok.EndToken;
                        if (s == "ДИССЕРТАЦИЯ" || s == "ДИСЕРТАЦІЯ") 
                        {
                            int err = 0;
                            for (Pullenti.Ner.Token ttt = t1.Next; ttt != null; ttt = ttt.Next) 
                            {
                                if (ttt.Morph.Class.IsPreposition) 
                                    continue;
                                if (ttt.IsValue("СОИСКАНИЕ", "")) 
                                    continue;
                                Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt1 != null && npt1.Noun.IsValue("СТЕПЕНЬ", "СТУПІНЬ")) 
                                {
                                    t1 = (ttt = npt1.EndToken);
                                    continue;
                                }
                                Pullenti.Ner.ReferentToken rt = t1.Kit.ProcessReferent("PERSON", ttt);
                                if (rt != null && (rt.Referent is Pullenti.Ner.Person.PersonPropertyReferent)) 
                                {
                                    Pullenti.Ner.Person.PersonPropertyReferent ppr = rt.Referent as Pullenti.Ner.Person.PersonPropertyReferent;
                                    if (ppr.Name == "доктор наук") 
                                    {
                                        t1 = rt.EndToken;
                                        s = "ДОКТОРСКАЯ ДИССЕРТАЦИЯ";
                                        break;
                                    }
                                    else if (ppr.Name == "кандидат наук") 
                                    {
                                        t1 = rt.EndToken;
                                        s = "КАНДИДАТСКАЯ ДИССЕРТАЦИЯ";
                                        break;
                                    }
                                    else if (ppr.Name == "магистр") 
                                    {
                                        t1 = rt.EndToken;
                                        s = "МАГИСТЕРСКАЯ ДИССЕРТАЦИЯ";
                                        break;
                                    }
                                }
                                if (ttt.IsValue("ДОКТОР", null) || ttt.IsValue("КАНДИДАТ", null) || ttt.IsValue("МАГИСТР", "МАГІСТР")) 
                                {
                                    t1 = ttt;
                                    npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                    if (npt1 != null && npt1.EndToken.IsValue("НАУК", null)) 
                                        t1 = npt1.EndToken;
                                    s = (ttt.IsValue("МАГИСТР", "МАГІСТР") ? "МАГИСТЕРСКАЯ ДИССЕРТАЦИЯ" : (ttt.IsValue("ДОКТОР", null) ? "ДОКТОРСКАЯ ДИССЕРТАЦИЯ" : "КАНДИДАТСКАЯ ДИССЕРТАЦИЯ"));
                                    break;
                                }
                                if ((++err) > 3) 
                                    break;
                            }
                        }
                        if (t1.Next != null && t1.Next.IsChar('.')) 
                            t1 = t1.Next;
                        if (s.EndsWith("ОТЧЕТ") && t1.Next != null && t1.Next.IsValue("О", null)) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t1.Next, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                            if (npt1 != null && npt1.Morph.Case.IsPrepositional) 
                                t1 = npt1.EndToken;
                        }
                        return new TitleItemToken(npt.BeginToken, t1, ty) { Value = s };
                    }
                }
            }
            Pullenti.Ner.Core.TerminToken tok1 = m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok1 != null) 
            {
                Pullenti.Ner.Token t1 = tok1.EndToken;
                TitleItemToken re = new TitleItemToken(tok1.BeginToken, t1, (Types)tok1.Termin.Tag);
                return re;
            }
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false)) 
            {
                tok1 = m_Termins.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok1 != null && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tok1.EndToken.Next, false, null, false)) 
                {
                    Pullenti.Ner.Token t1 = tok1.EndToken.Next;
                    return new TitleItemToken(tok1.BeginToken, t1, (Types)tok1.Termin.Tag);
                }
            }
            return null;
        }
        static TitleItemToken TryAttachSpeciality(Pullenti.Ner.Token t, bool keyWordBefore)
        {
            if (t == null) 
                return null;
            bool susp = false;
            if (!keyWordBefore) 
            {
                if (!t.IsNewlineBefore) 
                    susp = true;
            }
            StringBuilder val = null;
            Pullenti.Ner.Token t0 = t;
            int digCount = 0;
            for (int i = 0; i < 3; i++) 
            {
                Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                if (nt == null) 
                    break;
                if (nt.Typ != Pullenti.Ner.NumberSpellingType.Digit || nt.Morph.Class.IsAdjective) 
                    break;
                if (val == null) 
                    val = new StringBuilder();
                if (susp && t.LengthChar != 2) 
                    return null;
                string digs = nt.GetSourceText();
                digCount += digs.Length;
                val.Append(digs);
                if (t.Next == null) 
                    break;
                t = t.Next;
                if (t.IsCharOf(".,") || t.IsHiphen) 
                {
                    if (susp && (i < 2)) 
                    {
                        if (!t.IsChar('.') || t.IsWhitespaceAfter || t.IsWhitespaceBefore) 
                            return null;
                    }
                    if (t.Next != null) 
                        t = t.Next;
                }
            }
            if (val == null || (digCount < 5)) 
                return null;
            if (digCount != 6) 
            {
                if (!keyWordBefore) 
                    return null;
            }
            else 
            {
                val.Insert(4, '.');
                val.Insert(2, '.');
            }
            for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore) 
                    break;
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    t = (tt = br.EndToken);
                    continue;
                }
                t = tt;
            }
            return new TitleItemToken(t0, t, Types.Speciality) { Value = val.ToString() };
        }
        static Pullenti.Ner.Core.TerminCollection m_Termins;
        public static void Initialize()
        {
            if (m_Termins != null) 
                return;
            m_Termins = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"РАБОТА", "ДИССЕРТАЦИЯ", "ОТЧЕТ", "ОБЗОР", "ДИПЛОМ", "ПРОЕКТ", "СПРАВКА", "АВТОРЕФЕРАТ", "РЕФЕРАТ", "TECHNOLOGY ISSUES", "TECHNOLOGY COURSE", "УЧЕБНИК", "УЧЕБНОЕ ПОСОБИЕ"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Types.Typ });
            }
            foreach (string s in new string[] {"РОБОТА", "ДИСЕРТАЦІЯ", "ЗВІТ", "ОГЛЯД", "ДИПЛОМ", "ПРОЕКТ", "ДОВІДКА", "АВТОРЕФЕРАТ", "РЕФЕРАТ"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.UA) { Tag = Types.Typ });
            }
            foreach (string s in new string[] {"ДОПУСТИТЬ К ЗАЩИТА", "РЕКОМЕНДОВАТЬ К ЗАЩИТА", "ДОЛЖНОСТЬ", "ЦЕЛЬ РАБОТЫ", "НА ПРАВАХ РУКОПИСИ", "ПО ИЗДАНИЮ", "ПОЛУЧЕНО"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Types.Dust });
            }
            foreach (string s in new string[] {"ДОПУСТИТИ ДО ЗАХИСТУ", "РЕКОМЕНДУВАТИ ДО ЗАХИСТ", "ПОСАДА", "МЕТА РОБОТИ", "НА ПРАВАХ РУКОПИСУ", "ПО ВИДАННЮ", "ОТРИМАНО"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.UA) { Tag = Types.Dust });
            }
            foreach (string s in new string[] {"УТВЕРЖДАТЬ", "СОГЛАСЕН", "СТВЕРДЖУВАТИ", "ЗГОДЕН"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Types.Adopt });
            }
            foreach (string s in new string[] {"НАУЧНЫЙ РУКОВОДИТЕЛЬ", "РУКОВОДИТЕЛЬ РАБОТА", "НАУКОВИЙ КЕРІВНИК", "КЕРІВНИК РОБОТА"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Types.Boss });
            }
            foreach (string s in new string[] {"НАУЧНЫЙ КОНСУЛЬТАНТ", "КОНСУЛЬТАНТ", "НАУКОВИЙ КОНСУЛЬТАНТ"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Types.Consultant });
            }
            foreach (string s in new string[] {"РЕДАКТОР", "РЕДАКТОРСКАЯ ГРУППА", "РЕЦЕНЗЕНТ", "РЕДАКТОРСЬКА ГРУПА"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Types.Editor });
            }
            foreach (string s in new string[] {"ОФИЦИАЛЬНЫЙ ОППОНЕНТ", "ОППОНЕНТ", "ОФІЦІЙНИЙ ОПОНЕНТ"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Types.Opponent });
            }
            foreach (string s in new string[] {"ИСПОЛНИТЕЛЬ", "ОТВЕТСТВЕННЫЙ ИСПОЛНИТЕЛЬ", "АВТОР", "ДИПЛОМНИК", "КОЛЛЕКТТИВ ИСПОЛНИТЕЛЕЙ", "ВЫПОЛНИТЬ", "ИСПОЛНИТЬ"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Types.Worker });
            }
            foreach (string s in new string[] {"ВИКОНАВЕЦЬ", "ВІДПОВІДАЛЬНИЙ ВИКОНАВЕЦЬ", "АВТОР", "ДИПЛОМНИК", "КОЛЛЕКТТИВ ВИКОНАВЦІВ", "ВИКОНАТИ", "ВИКОНАТИ"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.UA) { Tag = Types.Worker });
            }
            foreach (string s in new string[] {"КЛЮЧЕВЫЕ СЛОВА", "KEYWORDS", "КЛЮЧОВІ СЛОВА"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s) { Tag = Types.Keywords });
            }
        }
    }
}