/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Поддержка работы с союзами (запятая тоже считается союзом). Союзы могут быть из нескольких слов, 
    /// например, "а также и".
    /// </summary>
    public static class ConjunctionHelper
    {
        /// <summary>
        /// Попытаться выделить союз с указанного токена.
        /// </summary>
        /// <param name="t">начальный токен</param>
        /// <return>результат или null</return>
        public static ConjunctionToken TryParse(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            if (t.IsComma) 
            {
                ConjunctionToken ne = TryParse(t.Next);
                if (ne != null) 
                {
                    ne.BeginToken = t;
                    ne.IsSimple = false;
                    return ne;
                }
                return new ConjunctionToken(t, t) { Typ = ConjunctionType.Comma, IsSimple = true, Normal = "," };
            }
            TerminToken tok = m_Ontology.TryParse(t, TerminParseAttr.No);
            if (tok != null) 
            {
                if (t.IsValue("ТО", null)) 
                {
                    NounPhraseToken npt = NounPhraseHelper.TryParse(t, NounPhraseParseAttr.ParseAdverbs, 0, null);
                    if (npt != null && npt.EndChar > tok.EndToken.EndChar) 
                        return null;
                }
                if (tok.Termin.Tag2 != null) 
                {
                    if (!(tok.EndToken is Pullenti.Ner.TextToken)) 
                        return null;
                    if (tok.EndToken.GetMorphClassInDictionary().IsVerb) 
                    {
                        if (!(tok.EndToken as Pullenti.Ner.TextToken).Term.EndsWith("АЯ")) 
                            return null;
                    }
                }
                return new ConjunctionToken(t, tok.EndToken) { Normal = tok.Termin.CanonicText, Typ = (ConjunctionType)tok.Termin.Tag };
            }
            if (!t.GetMorphClassInDictionary().IsConjunction) 
                return null;
            if (t.IsAnd || t.IsOr) 
            {
                ConjunctionToken res = new ConjunctionToken(t, t) { Normal = (t as Pullenti.Ner.TextToken).Term, IsSimple = true, Typ = (t.IsOr ? ConjunctionType.Or : ConjunctionType.And) };
                if (((t.Next != null && t.Next.IsChar('(') && (t.Next.Next is Pullenti.Ner.TextToken)) && t.Next.Next.IsOr && t.Next.Next.Next != null) && t.Next.Next.Next.IsChar(')')) 
                    res.EndToken = t.Next.Next.Next;
                else if ((t.Next != null && t.Next.IsCharOf("\\/") && (t.Next.Next is Pullenti.Ner.TextToken)) && t.Next.Next.IsOr) 
                    res.EndToken = t.Next.Next;
                return res;
            }
            string term = (t as Pullenti.Ner.TextToken).Term;
            if (term == "НИ") 
                return new ConjunctionToken(t, t) { Normal = term, Typ = ConjunctionType.Not };
            if ((term == "А" || term == "НО" || term == "ЗАТО") || term == "ОДНАКО") 
                return new ConjunctionToken(t, t) { Normal = term, Typ = ConjunctionType.But };
            return null;
        }
        static TerminCollection m_Ontology;
        internal static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new TerminCollection();
            Termin te;
            te = new Termin("ТАКЖЕ") { Tag = ConjunctionType.And };
            te.AddVariant("А ТАКЖЕ", false);
            te.AddVariant("КАК И", false);
            te.AddVariant("ТАК И", false);
            te.AddVariant("А РАВНО", false);
            te.AddVariant("А РАВНО И", false);
            m_Ontology.Add(te);
            te = new Termin("ЕСЛИ") { Tag = ConjunctionType.If };
            m_Ontology.Add(te);
            te = new Termin("ТО") { Tag = ConjunctionType.Then };
            m_Ontology.Add(te);
            te = new Termin("ИНАЧЕ") { Tag = ConjunctionType.Else };
            m_Ontology.Add(te);
            te = new Termin("ИНАЧЕ КАК") { Tag = ConjunctionType.Except, Tag2 = true };
            te.AddVariant("ИНАЧЕ, КАК", false);
            te.AddVariant("ЗА ИСКЛЮЧЕНИЕМ", false);
            te.AddVariant("ИСКЛЮЧАЯ", false);
            te.AddAbridge("КРОМЕ");
            te.AddAbridge("КРОМЕ КАК");
            te.AddAbridge("КРОМЕ, КАК");
            m_Ontology.Add(te);
            te = new Termin("ВКЛЮЧАЯ") { Tag = ConjunctionType.Include, Tag2 = true };
            te.AddVariant("В ТОМ ЧИСЛЕ", false);
            m_Ontology.Add(te);
        }
    }
}