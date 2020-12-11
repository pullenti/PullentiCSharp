/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Titlepage.Internal
{
    class Line : Pullenti.Ner.MetaToken
    {
        private Line(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public int CharsCount
        {
            get
            {
                int cou = 0;
                for (Pullenti.Ner.Token t = BeginToken; t != null; t = t.Next) 
                {
                    cou += t.LengthChar;
                    if (t == EndToken) 
                        break;
                }
                return cou;
            }
        }
        public bool IsPureEn
        {
            get
            {
                int en = 0;
                int ru = 0;
                for (Pullenti.Ner.Token t = BeginToken; t != null && t.EndChar <= EndChar; t = t.Next) 
                {
                    if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) 
                    {
                        if (t.Chars.IsCyrillicLetter) 
                            ru++;
                        else if (t.Chars.IsLatinLetter) 
                            en++;
                    }
                }
                if (en > 0 && ru == 0) 
                    return true;
                return false;
            }
        }
        public bool IsPureRu
        {
            get
            {
                int en = 0;
                int ru = 0;
                for (Pullenti.Ner.Token t = BeginToken; t != null && t.EndChar <= EndChar; t = t.Next) 
                {
                    if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) 
                    {
                        if (t.Chars.IsCyrillicLetter) 
                            ru++;
                        else if (t.Chars.IsLatinLetter) 
                            en++;
                    }
                }
                if (ru > 0 && en == 0) 
                    return true;
                return false;
            }
        }
        public static List<Line> Parse(Pullenti.Ner.Token t0, int maxLines, int maxChars, int maxEndChar)
        {
            List<Line> res = new List<Line>();
            int totalChars = 0;
            for (Pullenti.Ner.Token t = t0; t != null; t = t.Next) 
            {
                if (maxEndChar > 0) 
                {
                    if (t.BeginChar > maxEndChar) 
                        break;
                }
                Pullenti.Ner.Token t1;
                for (t1 = t; t1 != null && t1.Next != null; t1 = t1.Next) 
                {
                    if (t1.IsNewlineAfter) 
                    {
                        if (t1.Next == null || Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t1.Next)) 
                            break;
                    }
                    if (t1 == t && t.IsNewlineBefore && (t.GetReferent() is Pullenti.Ner.Person.PersonReferent)) 
                    {
                        if (t1.Next == null) 
                            continue;
                        if ((t1.Next is Pullenti.Ner.TextToken) && t1.Next.Chars.IsLetter && !t1.Next.Chars.IsAllLower) 
                            break;
                    }
                }
                if (t1 == null) 
                    t1 = t;
                TitleItemToken tit = TitleItemToken.TryAttach(t);
                if (tit != null) 
                {
                    if (tit.Typ == TitleItemToken.Types.Keywords) 
                        break;
                }
                Pullenti.Ner.Core.Internal.BlockTitleToken bl = Pullenti.Ner.Core.Internal.BlockTitleToken.TryAttach(t, false, null);
                if (bl != null) 
                {
                    if (bl.Typ != Pullenti.Ner.Core.Internal.BlkTyps.Undefined) 
                        break;
                }
                Line l = new Line(t, t1);
                res.Add(l);
                totalChars += l.CharsCount;
                if (res.Count >= maxLines || totalChars >= maxChars) 
                    break;
                t = t1;
            }
            return res;
        }
    }
}