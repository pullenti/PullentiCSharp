/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Core
{
    // Поддержка работы с собственными именами
    public static class ProperNameHelper
    {
        static string corrChars(string str, Pullenti.Morph.CharsInfo ci, bool keepChars)
        {
            if (!keepChars) 
                return str;
            if (ci.IsAllLower) 
                return str.ToLower();
            if (ci.IsCapitalUpper) 
                return MiscHelper.ConvertFirstCharUpperAndOtherLower(str);
            return str;
        }
        static string GetNameWithoutBrackets(Pullenti.Ner.Token begin, Pullenti.Ner.Token end, bool normalizeFirstNounGroup = false, bool normalFirstGroupSingle = false, bool ignoreGeoReferent = false)
        {
            string res = null;
            if (BracketHelper.CanBeStartOfSequence(begin, false, false) && BracketHelper.CanBeEndOfSequence(end, false, begin, false)) 
            {
                begin = begin.Next;
                end = end.Previous;
            }
            if (normalizeFirstNounGroup && !begin.Morph.Class.IsPreposition) 
            {
                NounPhraseToken npt = NounPhraseHelper.TryParse(begin, NounPhraseParseAttr.ReferentCanBeNoun, 0, null);
                if (npt != null) 
                {
                    if (npt.Noun.GetMorphClassInDictionary().IsUndefined && npt.Adjectives.Count == 0) 
                        npt = null;
                }
                if (npt != null && npt.EndToken.EndChar > end.EndChar) 
                    npt = null;
                if (npt != null) 
                {
                    res = npt.GetNormalCaseText(null, (normalFirstGroupSingle ? Pullenti.Morph.MorphNumber.Singular : Pullenti.Morph.MorphNumber.Undefined), Pullenti.Morph.MorphGender.Undefined, false);
                    Pullenti.Ner.Token te = npt.EndToken.Next;
                    if (((te != null && te.Next != null && te.IsComma) && (te.Next is Pullenti.Ner.TextToken) && te.Next.EndChar <= end.EndChar) && te.Next.Morph.Class.IsVerb && te.Next.Morph.Class.IsAdjective) 
                    {
                        foreach (Pullenti.Morph.MorphBaseInfo it in te.Next.Morph.Items) 
                        {
                            if (it.Gender == npt.Morph.Gender || ((it.Gender & npt.Morph.Gender)) != Pullenti.Morph.MorphGender.Undefined) 
                            {
                                if (!((it.Case & npt.Morph.Case)).IsUndefined) 
                                {
                                    if (it.Number == npt.Morph.Number || ((it.Number & npt.Morph.Number)) != Pullenti.Morph.MorphNumber.Undefined) 
                                    {
                                        string var = (te.Next as Pullenti.Ner.TextToken).Term;
                                        if (it is Pullenti.Morph.MorphWordForm) 
                                            var = (it as Pullenti.Morph.MorphWordForm).NormalCase;
                                        Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective, Gender = npt.Morph.Gender, Number = npt.Morph.Number, Language = npt.Morph.Language };
                                        var = Pullenti.Morph.MorphologyService.GetWordform(var, bi);
                                        if (var != null) 
                                        {
                                            res = string.Format("{0}, {1}", res, var);
                                            te = te.Next.Next;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (te != null && te.EndChar <= end.EndChar) 
                    {
                        string s = GetNameEx(te, end, Pullenti.Morph.MorphClass.Undefined, Pullenti.Morph.MorphCase.Undefined, Pullenti.Morph.MorphGender.Undefined, true, ignoreGeoReferent);
                        if (!string.IsNullOrEmpty(s)) 
                        {
                            if (!char.IsLetterOrDigit(s[0])) 
                                res = string.Format("{0}{1}", res, s);
                            else 
                                res = string.Format("{0} {1}", res, s);
                        }
                    }
                }
                else if ((begin is Pullenti.Ner.TextToken) && begin.Chars.IsCyrillicLetter) 
                {
                    Pullenti.Morph.MorphClass mm = begin.GetMorphClassInDictionary();
                    if (!mm.IsUndefined) 
                    {
                        res = begin.GetNormalCaseText(mm, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                        if (begin.EndChar < end.EndChar) 
                            res = string.Format("{0} {1}", res, GetNameEx(begin.Next, end, Pullenti.Morph.MorphClass.Undefined, Pullenti.Morph.MorphCase.Undefined, Pullenti.Morph.MorphGender.Undefined, true, false));
                    }
                }
            }
            if (res == null) 
                res = GetNameEx(begin, end, Pullenti.Morph.MorphClass.Undefined, Pullenti.Morph.MorphCase.Undefined, Pullenti.Morph.MorphGender.Undefined, true, ignoreGeoReferent);
            if (!string.IsNullOrEmpty(res)) 
            {
                int k = 0;
                for (int i = res.Length - 1; i >= 0; i--,k++) 
                {
                    if (res[i] == '*' || char.IsWhiteSpace(res[i])) 
                    {
                    }
                    else 
                        break;
                }
                if (k > 0) 
                {
                    if (k == res.Length) 
                        return null;
                    res = res.Substring(0, res.Length - k);
                }
            }
            return res;
        }
        static string GetName(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            string res = GetNameEx(begin, end, Pullenti.Morph.MorphClass.Undefined, Pullenti.Morph.MorphCase.Undefined, Pullenti.Morph.MorphGender.Undefined, false, false);
            return res;
        }
        public static string GetNameEx(Pullenti.Ner.Token begin, Pullenti.Ner.Token end, Pullenti.Morph.MorphClass cla, Pullenti.Morph.MorphCase mc, Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined, bool ignoreBracketsAndHiphens = false, bool ignoreGeoReferent = false)
        {
            if (end == null || begin == null) 
                return null;
            if (begin.EndChar > end.BeginChar && begin != end) 
                return null;
            StringBuilder res = new StringBuilder();
            string prefix = null;
            for (Pullenti.Ner.Token t = begin; t != null && t.EndChar <= end.EndChar; t = t.Next) 
            {
                if (res.Length > 1000) 
                    break;
                if (t.IsTableControlChar) 
                    continue;
                if (ignoreBracketsAndHiphens) 
                {
                    if (BracketHelper.IsBracket(t, false)) 
                    {
                        if (t == end) 
                            break;
                        if (t.IsCharOf("(<[")) 
                        {
                            BracketSequenceToken br = BracketHelper.TryParse(t, BracketParseAttr.No, 100);
                            if (br != null && br.EndChar <= end.EndChar) 
                            {
                                string tmp = GetNameEx(br.BeginToken.Next, br.EndToken.Previous, Pullenti.Morph.MorphClass.Undefined, Pullenti.Morph.MorphCase.Undefined, Pullenti.Morph.MorphGender.Undefined, ignoreBracketsAndHiphens, false);
                                if (tmp != null) 
                                {
                                    if ((br.EndChar == end.EndChar && br.BeginToken.Next == br.EndToken.Previous && !br.BeginToken.Next.Chars.IsLetter) && !(br.BeginToken.Next is Pullenti.Ner.ReferentToken)) 
                                    {
                                    }
                                    else 
                                        res.AppendFormat(" {0}{1}{2}", t.GetSourceText(), tmp, br.EndToken.GetSourceText());
                                }
                                t = br.EndToken;
                            }
                        }
                        continue;
                    }
                    if (t.IsHiphen) 
                    {
                        if (t == end) 
                            break;
                        else if (t.IsWhitespaceBefore || t.IsWhitespaceAfter) 
                            continue;
                    }
                }
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt != null) 
                {
                    if (!ignoreBracketsAndHiphens) 
                    {
                        if ((tt.Next != null && tt.Next.IsHiphen && (tt.Next.Next is Pullenti.Ner.TextToken)) && tt != end && tt.Next != end) 
                        {
                            if (prefix == null) 
                                prefix = tt.Term;
                            else 
                                prefix = string.Format("{0}-{1}", prefix, tt.Term);
                            t = tt.Next;
                            if (t == end) 
                                break;
                            else 
                                continue;
                        }
                    }
                    string s = null;
                    if (cla.Value != 0 || !mc.IsUndefined || gender != Pullenti.Morph.MorphGender.Undefined) 
                    {
                        foreach (Pullenti.Morph.MorphBaseInfo wff in tt.Morph.Items) 
                        {
                            Pullenti.Morph.MorphWordForm wf = wff as Pullenti.Morph.MorphWordForm;
                            if (wf == null) 
                                continue;
                            if (cla.Value != 0) 
                            {
                                if (((wf.Class.Value & cla.Value)) == 0) 
                                    continue;
                            }
                            if (!mc.IsUndefined) 
                            {
                                if (((wf.Case & mc)).IsUndefined) 
                                    continue;
                            }
                            if (gender != Pullenti.Morph.MorphGender.Undefined) 
                            {
                                if (((wf.Gender & gender)) == Pullenti.Morph.MorphGender.Undefined) 
                                    continue;
                            }
                            if (s == null || wf.NormalCase == tt.Term) 
                                s = wf.NormalCase;
                        }
                        if (s == null && gender != Pullenti.Morph.MorphGender.Undefined) 
                        {
                            foreach (Pullenti.Morph.MorphBaseInfo wff in tt.Morph.Items) 
                            {
                                Pullenti.Morph.MorphWordForm wf = wff as Pullenti.Morph.MorphWordForm;
                                if (wf == null) 
                                    continue;
                                if (cla.Value != 0) 
                                {
                                    if (((wf.Class.Value & cla.Value)) == 0) 
                                        continue;
                                }
                                if (!mc.IsUndefined) 
                                {
                                    if (((wf.Case & mc)).IsUndefined) 
                                        continue;
                                }
                                if (s == null || wf.NormalCase == tt.Term) 
                                    s = wf.NormalCase;
                            }
                        }
                    }
                    if (s == null) 
                    {
                        s = tt.Term;
                        if (tt.Chars.IsLastLower && tt.LengthChar > 2) 
                        {
                            s = tt.GetSourceText();
                            for (int i = s.Length - 1; i >= 0; i--) 
                            {
                                if (char.IsUpper(s[i])) 
                                {
                                    s = s.Substring(0, i + 1);
                                    break;
                                }
                            }
                        }
                    }
                    if (prefix != null) 
                    {
                        string delim = "-";
                        if (ignoreBracketsAndHiphens) 
                            delim = " ";
                        s = string.Format("{0}{1}{2}", prefix, delim, s);
                    }
                    prefix = null;
                    if (res.Length > 0 && s.Length > 0) 
                    {
                        if (char.IsLetterOrDigit(s[0])) 
                        {
                            char ch0 = res[res.Length - 1];
                            if (ch0 == '-') 
                            {
                            }
                            else 
                                res.Append(' ');
                        }
                        else if (!ignoreBracketsAndHiphens && BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                            res.Append(' ');
                    }
                    res.Append(s);
                }
                else if (t is Pullenti.Ner.NumberToken) 
                {
                    if (res.Length > 0) 
                    {
                        if (!t.IsWhitespaceBefore && res[res.Length - 1] == '-') 
                        {
                        }
                        else 
                            res.Append(' ');
                    }
                    Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                    if ((t.Morph.Class.IsAdjective && nt.Typ == Pullenti.Ner.NumberSpellingType.Words && nt.BeginToken == nt.EndToken) && (nt.BeginToken is Pullenti.Ner.TextToken)) 
                        res.Append((nt.BeginToken as Pullenti.Ner.TextToken).Term);
                    else 
                        res.Append(nt.Value);
                }
                else if (t is Pullenti.Ner.MetaToken) 
                {
                    if ((ignoreGeoReferent && t != begin && t.GetReferent() != null) && t.GetReferent().TypeName == "GEO") 
                        continue;
                    string s = GetNameEx((t as Pullenti.Ner.MetaToken).BeginToken, (t as Pullenti.Ner.MetaToken).EndToken, cla, mc, gender, ignoreBracketsAndHiphens, ignoreGeoReferent);
                    if (!string.IsNullOrEmpty(s)) 
                    {
                        if (res.Length > 0) 
                        {
                            if (!t.IsWhitespaceBefore && res[res.Length - 1] == '-') 
                            {
                            }
                            else 
                                res.Append(' ');
                        }
                        res.Append(s);
                    }
                }
                if (t == end) 
                    break;
            }
            if (res.Length == 0) 
                return null;
            return res.ToString();
        }
    }
}