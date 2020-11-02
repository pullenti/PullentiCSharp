/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Поддержка работы с предлогами
    /// </summary>
    public static class PrepositionHelper
    {
        /// <summary>
        /// Попытаться выделить предлог с указанного токена
        /// </summary>
        /// <param name="t">начальный токен</param>
        /// <return>результат или null</return>
        public static PrepositionToken TryParse(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            TerminToken tok = m_Ontology.TryParse(t, TerminParseAttr.No);
            if (tok != null) 
                return new PrepositionToken(t, tok.EndToken) { Normal = tok.Termin.CanonicText, NextCase = (Pullenti.Morph.MorphCase)tok.Termin.Tag };
            Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
            if (!mc.IsPreposition) 
                return null;
            PrepositionToken res = new PrepositionToken(t, t);
            res.Normal = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Preposition, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
            res.NextCase = Pullenti.Morph.LanguageHelper.GetCaseAfterPreposition(res.Normal);
            if ((t.Next != null && t.Next.IsHiphen && !t.IsWhitespaceAfter) && (t.Next.Next is Pullenti.Ner.TextToken) && t.Next.Next.GetMorphClassInDictionary().IsPreposition) 
                res.EndToken = t.Next.Next;
            return res;
        }
        static TerminCollection m_Ontology;
        internal static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new TerminCollection();
            foreach (string s in new string[] {"близко от", "в виде", "в зависимости от", "в интересах", "в качестве", "в лице", "в отличие от", "в отношении", "в пандан", "в пользу", "в преддверии", "в продолжение", "в результате", "в роли", "в силу", "в случае", "в течение", "в целях", "в честь", "во имя", "вплоть до", "впредь до", "за вычетом", "за исключением", "за счет", "исходя из", "на благо", "на виду у", "на глазах у", "начиная с", "невзирая на", "недалеко от", "независимо от", "от имени", "от лица", "по линии", "по мере", "по поводу", "по причине", "по случаю", "поблизости от", "под видом", "под эгидой", "при помощи", "с ведома", "с помощью", "с точки зрения", "с целью"}) 
            {
                m_Ontology.Add(new Termin(s.ToUpper(), Pullenti.Morph.MorphLang.RU, true) { Tag = Pullenti.Morph.MorphCase.Genitive });
            }
            foreach (string s in new string[] {"вдоль по", "по направлению к", "применительно к", "смотря по", "судя по"}) 
            {
                m_Ontology.Add(new Termin(s.ToUpper(), Pullenti.Morph.MorphLang.RU, true) { Tag = Pullenti.Morph.MorphCase.Dative });
            }
            foreach (string s in new string[] {"несмотря на", "с прицелом на"}) 
            {
                m_Ontology.Add(new Termin(s.ToUpper(), Pullenti.Morph.MorphLang.RU, true) { Tag = Pullenti.Morph.MorphCase.Accusative });
            }
            foreach (string s in new string[] {"во славу"}) 
            {
                m_Ontology.Add(new Termin(s.ToUpper(), Pullenti.Morph.MorphLang.RU, true) { Tag = (Pullenti.Morph.MorphCase.Genitive | Pullenti.Morph.MorphCase.Dative) });
            }
            foreach (string s in new string[] {"не считая"}) 
            {
                m_Ontology.Add(new Termin(s.ToUpper(), Pullenti.Morph.MorphLang.RU, true) { Tag = (Pullenti.Morph.MorphCase.Genitive | Pullenti.Morph.MorphCase.Accusative) });
            }
            foreach (string s in new string[] {"в связи с", "в соответствии с", "вслед за", "лицом к лицу с", "наряду с", "по сравнению с", "рядом с", "следом за"}) 
            {
                m_Ontology.Add(new Termin(s.ToUpper(), Pullenti.Morph.MorphLang.RU, true) { Tag = Pullenti.Morph.MorphCase.Instrumental });
            }
        }
    }
}