/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Instrument.Internal
{
    static class ContractHelper
    {
        public static void CorrectDummyNewlines(FragToken fr)
        {
            int i;
            for (i = 0; i < fr.Children.Count; i++) 
            {
                FragToken ch = fr.Children[i];
                if ((ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Keyword || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Number || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Name) || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment) 
                {
                }
                else 
                    break;
            }
            if ((i < fr.Children.Count) && fr.Children[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
            {
                int j;
                for (j = i + 1; j < fr.Children.Count; j++) 
                {
                    if (fr.Children[j].Kind != Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                        break;
                    else if (_calcNewlineBetweenCoef(fr.Children[j - 1], fr.Children[j]) > 0) 
                        break;
                }
                if (j >= fr.Children.Count) 
                {
                    j--;
                    fr.Children[i].Kind = Pullenti.Ner.Instrument.InstrumentKind.Content;
                    fr.Children[i].Number = 0;
                    fr.Children[i].EndToken = fr.Children[j].EndToken;
                    if ((i + 1) < fr.Children.Count) 
                        fr.Children.RemoveRange(i + 1, fr.Children.Count - i - 1);
                    if (fr.Kind == Pullenti.Ner.Instrument.InstrumentKind.Preamble && fr.Children.Count == 1) 
                        fr.Children.Clear();
                }
                else 
                {
                    bool ch = false;
                    for (j = i + 1; j < fr.Children.Count; j++) 
                    {
                        if (fr.Children[j - 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention && fr.Children[j].Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention && (_calcNewlineBetweenCoef(fr.Children[j - 1], fr.Children[j]) < 0)) 
                        {
                            fr.Children[j - 1].EndToken = fr.Children[j].EndToken;
                            fr.Children.RemoveAt(j);
                            j--;
                            ch = true;
                        }
                    }
                    if (ch) 
                    {
                        int num = 1;
                        for (j = i; j < fr.Children.Count; j++) 
                        {
                            if (fr.Children[j].Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                                fr.Children[j].Number = num++;
                        }
                    }
                }
            }
            foreach (FragToken ch in fr.Children) 
            {
                CorrectDummyNewlines(ch);
            }
        }
        static int _calcNewlineBetweenCoef(FragToken fr1, FragToken fr2)
        {
            if (fr1.NewlinesAfterCount > 1) 
                return 1;
            for (Pullenti.Ner.Token tt = fr1.BeginToken; tt != null && tt.EndChar <= fr1.EndChar; tt = tt.Next) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines, 100);
                    if (br != null && br.EndChar >= fr2.BeginChar) 
                        return -1;
                }
            }
            Pullenti.Ner.Token t = fr1.EndToken;
            if (t.IsCharOf(":;.")) 
                return 1;
            if ((t is Pullenti.Ner.TextToken) && ((t.Morph.Class.IsPreposition || t.Morph.Class.IsConjunction))) 
                return -1;
            Pullenti.Ner.Token t1 = fr2.BeginToken;
            if (t1 is Pullenti.Ner.TextToken) 
            {
                if (t1.Chars.IsAllLower) 
                    return -1;
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t1, false, false)) 
                {
                    if (t.Chars.IsAllLower) 
                        return -1;
                }
            }
            else if (t1 is Pullenti.Ner.NumberToken) 
            {
                if (t.Chars.IsAllLower) 
                    return -1;
            }
            if (t.Chars.IsAllLower) 
            {
                if (fr2.EndToken.IsChar(';')) 
                    return -1;
            }
            return 0;
        }
    }
}