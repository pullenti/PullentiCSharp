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

namespace Pullenti.Ner.Keyword.Internal
{
    class AutoannoSentToken : Pullenti.Ner.MetaToken
    {
        public AutoannoSentToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public double Rank;
        public string Value;
        public override string ToString()
        {
            return string.Format("{0}: {1}", Rank, Value);
        }
        static AutoannoSentToken TryParse(Pullenti.Ner.Token t)
        {
            if (t == null || !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                return null;
            AutoannoSentToken res = new AutoannoSentToken(t, t);
            bool hasVerb = false;
            for (; t != null; t = t.Next) 
            {
                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t) && t != res.BeginToken) 
                    break;
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r is Pullenti.Ner.Keyword.KeywordReferent) 
                {
                    res.Rank += (r as Pullenti.Ner.Keyword.KeywordReferent).Rank;
                    if ((r as Pullenti.Ner.Keyword.KeywordReferent).Typ == Pullenti.Ner.Keyword.KeywordType.Predicate) 
                        hasVerb = true;
                }
                else if (t is Pullenti.Ner.TextToken) 
                {
                    Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                    if (mc.IsPronoun || mc.IsPersonalPronoun) 
                        res.Rank -= 1;
                    else if (t.LengthChar > 1) 
                        res.Rank -= 0.1;
                }
                res.EndToken = t;
            }
            if (!hasVerb) 
                res.Rank /= 3;
            res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(res, Pullenti.Ner.Core.GetTextAttr.KeepRegister | Pullenti.Ner.Core.GetTextAttr.KeepQuotes);
            return res;
        }
        public static Pullenti.Ner.Keyword.KeywordReferent CreateAnnotation(Pullenti.Ner.Core.AnalysisKit kit, int maxSents)
        {
            List<AutoannoSentToken> sents = new List<AutoannoSentToken>();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                AutoannoSentToken sent = TryParse(t);
                if (sent == null) 
                    continue;
                if (sent.Rank > 0) 
                    sents.Add(sent);
                t = sent.EndToken;
            }
            if (sents.Count < 2) 
                return null;
            for (int i = 0; i < sents.Count; i++) 
            {
                sents[i].Rank *= (((double)((sents.Count - i))) / sents.Count);
            }
            if ((maxSents * 3) > sents.Count) 
            {
                maxSents = sents.Count / 3;
                if (maxSents == 0) 
                    maxSents = 1;
            }
            while (sents.Count > maxSents) 
            {
                int mini = 0;
                double min = sents[0].Rank;
                for (int i = 1; i < sents.Count; i++) 
                {
                    if (sents[i].Rank <= min) 
                    {
                        min = sents[i].Rank;
                        mini = i;
                    }
                }
                sents.RemoveAt(mini);
            }
            Pullenti.Ner.Keyword.KeywordReferent ano = new Pullenti.Ner.Keyword.KeywordReferent();
            ano.Typ = Pullenti.Ner.Keyword.KeywordType.Annotation;
            StringBuilder tmp = new StringBuilder();
            foreach (AutoannoSentToken s in sents) 
            {
                if (tmp.Length > 0) 
                    tmp.Append(' ');
                tmp.Append(s.Value);
                ano.Occurrence.Add(new Pullenti.Ner.TextAnnotation() { BeginChar = s.BeginChar, EndChar = s.EndChar, OccurenceOf = ano, Sofa = kit.Sofa });
            }
            ano.AddSlot(Pullenti.Ner.Keyword.KeywordReferent.ATTR_VALUE, tmp.ToString(), true, 0);
            return ano;
        }
    }
}