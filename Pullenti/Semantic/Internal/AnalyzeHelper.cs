/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Pullenti.Semantic.Internal
{
    class AnalyzeHelper
    {
        public static Pullenti.Semantic.SemDocument Process(Pullenti.Ner.AnalysisResult ar, Pullenti.Semantic.SemProcessParams pars)
        {
            Pullenti.Semantic.SemDocument txt = new Pullenti.Semantic.SemDocument();
            for (Pullenti.Ner.Token t = ar.FirstToken; t != null; t = t.Next) 
            {
                t.Tag = null;
            }
            if (pars.Progress != null) 
                pars.Progress(null, new ProgressChangedEventArgs(0, null)) /* error */;
            int pers0 = 0;
            for (Pullenti.Ner.Token t = ar.FirstToken; t != null; t = t.Next) 
            {
                if (pars.Progress != null) 
                {
                    int p = t.BeginChar;
                    if (ar.Sofa.Text.Length < 100000) 
                        p = (p * 100) / ar.Sofa.Text.Length;
                    else 
                        p /= ((ar.Sofa.Text.Length / 100));
                    if (p != pers0) 
                    {
                        pers0 = p;
                        pars.Progress(null, new ProgressChangedEventArgs(p, null)) /* error */;
                    }
                }
                Pullenti.Ner.Token t1 = t;
                for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsNewlineBefore) 
                    {
                        if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                            break;
                    }
                    t1 = tt;
                }
                try 
                {
                    _processBlock(txt, ar, t, t1);
                }
                catch(Exception ex) 
                {
                }
                t = t1;
                if (pars.MaxChar > 0 && t.EndChar > pars.MaxChar) 
                    break;
            }
            OptimizerHelper.Optimize(txt, pars);
            if (pars.Progress != null) 
                pars.Progress(null, new ProgressChangedEventArgs(100, null)) /* error */;
            return txt;
        }
        static void _processBlock(Pullenti.Semantic.SemDocument res, Pullenti.Ner.AnalysisResult ar, Pullenti.Ner.Token t0, Pullenti.Ner.Token t1)
        {
            Pullenti.Semantic.SemBlock blk = new Pullenti.Semantic.SemBlock(res);
            for (Pullenti.Ner.Token t = t0; t != null && t.EndChar <= t1.EndChar; t = t.Next) 
            {
                Pullenti.Ner.Token te = t;
                for (Pullenti.Ner.Token tt = t.Next; tt != null && tt.EndChar <= t1.EndChar; tt = tt.Next) 
                {
                    if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                        break;
                    else 
                        te = tt;
                }
                _processSentence(blk, ar, t, te);
                t = te;
            }
            if (blk.Fragments.Count > 0) 
                res.Blocks.Add(blk);
        }
        static void _processSentence(Pullenti.Semantic.SemBlock blk, Pullenti.Ner.AnalysisResult ar, Pullenti.Ner.Token t0, Pullenti.Ner.Token t1)
        {
            int cou = 0;
            for (Pullenti.Ner.Token t = t0; t != null && (t.EndChar < t1.EndChar); t = t.Next,cou++) 
            {
            }
            if (cou > 70) 
            {
                int cou2 = 0;
                for (Pullenti.Ner.Token t = t0; t != null && (t.EndChar < t1.EndChar); t = t.Next,cou2++) 
                {
                    if (cou2 >= 70) 
                    {
                        t1 = t;
                        break;
                    }
                }
            }
            List<Sentence> sents = Sentence.ParseVariants(t0, t1, 0, 100, SentItemType.Undefined);
            if (sents == null) 
                return;
            double max = (double)-1;
            Sentence best = null;
            Sentence alt = null;
            foreach (Sentence s in sents) 
            {
                if ((t1 is Pullenti.Ner.TextToken) && !t1.Chars.IsLetter) 
                    s.LastChar = t1 as Pullenti.Ner.TextToken;
                s.CalcCoef(false);
                if (s.Coef > max) 
                {
                    max = s.Coef;
                    best = s;
                    alt = null;
                }
                else if (s.Coef == max && max > 0) 
                    alt = s;
            }
            if (best != null && best.ResBlock != null) 
                best.AddToBlock(blk, null);
        }
    }
}