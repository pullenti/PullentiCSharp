/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Definition.Internal
{
    // Анализ вводных слов и словосочетаний
    public class ParenthesisToken : Pullenti.Ner.MetaToken
    {
        public ParenthesisToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public Pullenti.Ner.Referent Ref;
        public static ParenthesisToken TryAttach(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null) 
            {
                ParenthesisToken res = new ParenthesisToken(t, tok.EndToken);
                return res;
            }
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
            bool ok = false;
            Pullenti.Ner.Token t1;
            if (mc.IsAdverb) 
                ok = true;
            else if (mc.IsAdjective) 
            {
                if (t.Morph.ContainsAttr("сравн.", null) && t.Morph.ContainsAttr("кач.прил.", null)) 
                    ok = true;
            }
            if (ok && t.Next != null) 
            {
                if (t.Next.IsChar(',')) 
                    return new ParenthesisToken(t, t);
                t1 = t.Next;
                if (t1.GetMorphClassInDictionary() == Pullenti.Morph.MorphClass.Verb) 
                {
                    if (t1.Morph.ContainsAttr("н.вр.", null) && t1.Morph.ContainsAttr("нес.в.", null) && t1.Morph.ContainsAttr("дейст.з.", null)) 
                        return new ParenthesisToken(t, t1);
                }
            }
            t1 = null;
            if ((t.IsValue("В", null) && t.Next != null && t.Next.IsValue("СООТВЕТСТВИЕ", null)) && t.Next.Next != null && t.Next.Next.Morph.Class.IsPreposition) 
                t1 = t.Next.Next.Next;
            else if (t.IsValue("СОГЛАСНО", null)) 
                t1 = t.Next;
            else if (t.IsValue("В", null) && t.Next != null) 
            {
                if (t.Next.IsValue("СИЛА", null)) 
                    t1 = t.Next.Next;
                else if (t.Next.Morph.Class.IsAdjective || t.Next.Morph.Class.IsPronoun) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null) 
                    {
                        if (npt.Noun.IsValue("ВИД", null) || npt.Noun.IsValue("СЛУЧАЙ", null) || npt.Noun.IsValue("СФЕРА", null)) 
                            return new ParenthesisToken(t, npt.EndToken);
                    }
                }
            }
            if (t1 != null) 
            {
                if (t1.Next != null) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt1 != null) 
                    {
                        if (npt1.Noun.IsValue("НОРМА", null) || npt1.Noun.IsValue("ПОЛОЖЕНИЕ", null) || npt1.Noun.IsValue("УКАЗАНИЕ", null)) 
                            t1 = npt1.EndToken.Next;
                    }
                }
                Pullenti.Ner.Referent r = t1.GetReferent();
                if (r != null) 
                {
                    ParenthesisToken res = new ParenthesisToken(t, t1) { Ref = r };
                    if (t1.Next != null && t1.Next.IsComma) 
                    {
                        bool sila = false;
                        for (Pullenti.Ner.Token ttt = t1.Next.Next; ttt != null; ttt = ttt.Next) 
                        {
                            if (ttt.IsValue("СИЛА", null) || ttt.IsValue("ДЕЙСТВИЕ", null)) 
                            {
                                sila = true;
                                continue;
                            }
                            if (ttt.IsComma) 
                            {
                                if (sila) 
                                    res.EndToken = ttt.Previous;
                                break;
                            }
                            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(ttt, false, false)) 
                                break;
                        }
                    }
                    return res;
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                    return new ParenthesisToken(t, npt.EndToken);
            }
            Pullenti.Ner.Token tt = t;
            if (tt.IsValue("НЕ", null) && t != null) 
                tt = tt.Next;
            if (tt.Morph.Class.IsPreposition && tt != null) 
            {
                tt = tt.Next;
                Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt1 != null) 
                {
                    tt = npt1.EndToken;
                    if (tt.Next != null && tt.Next.IsComma) 
                        return new ParenthesisToken(t, tt.Next);
                    if (npt1.Noun.IsValue("ОЧЕРЕДЬ", null)) 
                        return new ParenthesisToken(t, tt);
                }
            }
            if (t.IsValue("ВЕДЬ", null)) 
                return new ParenthesisToken(t, t);
            return null;
        }
        public static void Initialize()
        {
            if (m_Termins != null) 
                return;
            m_Termins = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"ИТАК", "СЛЕДОВАТЕЛЬНО", "ТАКИМ ОБРАЗОМ"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.RU, true));
            }
        }
        static Pullenti.Ner.Core.TerminCollection m_Termins;
    }
}