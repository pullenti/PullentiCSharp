/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Definition.Internal
{
    class DefinitionAnalyzerEn
    {
        public static void Process(Pullenti.Ner.Core.AnalysisKit kit, Pullenti.Ner.Core.AnalyzerData ad)
        {
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (!Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                    continue;
                Pullenti.Ner.ReferentToken rt = TryParseThesis(t);
                if (rt == null) 
                    continue;
                rt.Referent = ad.RegisterReferent(rt.Referent);
                kit.EmbedToken(rt);
                t = rt;
            }
        }
        static Pullenti.Ner.ReferentToken TryParseThesis(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token tt = t;
            Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
            Pullenti.Ner.MetaToken preamb = null;
            if (mc.IsConjunction) 
                return null;
            if (t.IsValue("LET", null)) 
                return null;
            if (mc.IsPreposition || mc.IsMisc || mc.IsAdverb) 
            {
                if (!Pullenti.Ner.Core.MiscHelper.IsEngArticle(tt)) 
                {
                    for (tt = tt.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsComma) 
                            break;
                        if (tt.IsChar('(')) 
                        {
                            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br != null) 
                            {
                                tt = br.EndToken;
                                continue;
                            }
                        }
                        if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                            break;
                        Pullenti.Ner.Core.NounPhraseToken npt0 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective | Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun, 0, null);
                        if (npt0 != null) 
                        {
                            tt = npt0.EndToken;
                            continue;
                        }
                        if (tt.GetMorphClassInDictionary().IsVerb) 
                            break;
                    }
                    if (tt == null || !tt.IsComma || tt.Next == null) 
                        return null;
                    preamb = new Pullenti.Ner.MetaToken(t0, tt.Previous);
                    tt = tt.Next;
                }
            }
            Pullenti.Ner.Token t1 = tt;
            mc = tt.GetMorphClassInDictionary();
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective | Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun | Pullenti.Ner.Core.NounPhraseParseAttr.ParseAdverbs, 0, null);
            if (npt == null && (tt is Pullenti.Ner.TextToken)) 
            {
                if (tt.Chars.IsAllUpper) 
                    npt = new Pullenti.Ner.Core.NounPhraseToken(tt, tt);
                else if (!tt.Chars.IsAllLower) 
                {
                    if (mc.IsProper || preamb != null) 
                        npt = new Pullenti.Ner.Core.NounPhraseToken(tt, tt);
                }
            }
            if (npt == null) 
                return null;
            if (mc.IsPersonalPronoun) 
                return null;
            Pullenti.Ner.Token t2 = npt.EndToken.Next;
            if (t2 == null || Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t2) || !(t2 is Pullenti.Ner.TextToken)) 
                return null;
            if (!t2.GetMorphClassInDictionary().IsVerb) 
                return null;
            Pullenti.Ner.Token t3 = t2;
            for (tt = t2.Next; tt != null; tt = tt.Next) 
            {
                if (!tt.GetMorphClassInDictionary().IsVerb) 
                    break;
            }
            for (; tt != null; tt = tt.Next) 
            {
                if (tt.Next == null) 
                {
                    t3 = tt;
                    break;
                }
                if (tt.IsCharOf(".;!?")) 
                {
                    if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt.Next)) 
                    {
                        t3 = tt;
                        break;
                    }
                }
                if (!(tt is Pullenti.Ner.TextToken)) 
                    continue;
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        tt = br.EndToken;
                        continue;
                    }
                }
            }
            tt = t3;
            if (t3.IsCharOf(";.!?")) 
                tt = tt.Previous;
            string txt = Pullenti.Ner.Core.MiscHelper.GetTextValue(t2, tt, Pullenti.Ner.Core.GetTextAttr.KeepRegister | Pullenti.Ner.Core.GetTextAttr.KeepQuotes);
            if (txt == null || (txt.Length < 15)) 
                return null;
            if (t0 != t1) 
            {
                tt = t1.Previous;
                if (tt.IsComma) 
                    tt = tt.Previous;
                string txt0 = Pullenti.Ner.Core.MiscHelper.GetTextValue(t0, tt, Pullenti.Ner.Core.GetTextAttr.KeepRegister | Pullenti.Ner.Core.GetTextAttr.KeepQuotes);
                if (txt0 != null && txt0.Length > 10) 
                {
                    if (t0.Chars.IsCapitalUpper) 
                        txt0 = char.ToLower(txt0[0]) + txt0.Substring(1);
                    txt = string.Format("{0}, {1}", txt, txt0);
                }
            }
            tt = t1;
            if (Pullenti.Ner.Core.MiscHelper.IsEngArticle(tt)) 
                tt = tt.Next;
            string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt, t2.Previous, Pullenti.Ner.Core.GetTextAttr.KeepQuotes);
            if (nam.StartsWith("SO-CALLED")) 
                nam = nam.Substring(9).Trim();
            Pullenti.Ner.Definition.DefinitionReferent dr = new Pullenti.Ner.Definition.DefinitionReferent();
            dr.Kind = Pullenti.Ner.Definition.DefinitionKind.Assertation;
            dr.AddSlot(Pullenti.Ner.Definition.DefinitionReferent.ATTR_TERMIN, nam, false, 0);
            dr.AddSlot(Pullenti.Ner.Definition.DefinitionReferent.ATTR_VALUE, txt, false, 0);
            return new Pullenti.Ner.ReferentToken(dr, t0, t3);
        }
    }
}