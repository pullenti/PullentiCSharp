/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Org.Internal
{
    class OrgItemNumberToken : Pullenti.Ner.MetaToken
    {
        public OrgItemNumberToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public string Number;
        public override string ToString()
        {
            return string.Format("№ {0}", Number ?? "?");
        }
        public static OrgItemNumberToken TryAttach(Pullenti.Ner.Token t, bool canBePureNumber = false, OrgItemTypeToken typ = null)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt != null) 
            {
                Pullenti.Ner.Token t1 = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(tt);
                if ((t1 is Pullenti.Ner.NumberToken) && !t1.IsNewlineBefore) 
                {
                    OrgItemNumberToken res = new OrgItemNumberToken(tt, t1) { Number = (t1 as Pullenti.Ner.NumberToken).Value.ToString() };
                    if (t1.Next != null && t1.Next.IsCharOf("\\/") && (t1.Next.Next is Pullenti.Ner.NumberToken)) 
                    {
                        if (typ != null && ((typ.Typ == "офис" || typ.Typ == "банк" || typ.Typ == "отделение"))) 
                        {
                            res.EndToken = res.EndToken.Next.Next;
                            res.Number = string.Format("{0}/{1}", res.Number, (res.EndToken as Pullenti.Ner.NumberToken).Value);
                        }
                    }
                    return res;
                }
            }
            if ((t.IsHiphen && (t.Next is Pullenti.Ner.NumberToken) && !t.IsWhitespaceBefore) && !t.IsWhitespaceAfter) 
            {
                if (Pullenti.Ner.Core.NumberHelper.TryParseAge(t.Next) == null) 
                    return new OrgItemNumberToken(t, t.Next) { Number = (t.Next as Pullenti.Ner.NumberToken).Value.ToString() };
            }
            if (t is Pullenti.Ner.NumberToken) 
            {
                if ((!t.IsWhitespaceBefore && t.Previous != null && t.Previous.IsHiphen)) 
                    return new OrgItemNumberToken(t, t) { Number = (t as Pullenti.Ner.NumberToken).Value.ToString() };
                if (typ != null && typ.Typ != null && (((typ.Typ == "войсковая часть" || typ.Typ == "військова частина" || typ.Typ.Contains("колония")) || typ.Typ.Contains("колонія") || typ.Typ.Contains("школа")))) 
                {
                    if (t.LengthChar >= 4 || t.LengthChar <= 6) 
                    {
                        OrgItemNumberToken res = new OrgItemNumberToken(t, t) { Number = (t as Pullenti.Ner.NumberToken).Value.ToString() };
                        if (t.Next != null && ((t.Next.IsHiphen || t.Next.IsCharOf("\\/"))) && !t.Next.IsWhitespaceAfter) 
                        {
                            if ((t.Next.Next is Pullenti.Ner.NumberToken) && ((t.LengthChar + t.Next.Next.LengthChar) < 9)) 
                            {
                                res.EndToken = t.Next.Next;
                                res.Number = string.Format("{0}-{1}", res.Number, (res.EndToken as Pullenti.Ner.NumberToken).Value);
                            }
                            else if ((t.Next.Next is Pullenti.Ner.TextToken) && t.Next.Next.LengthChar == 1 && t.Next.Next.Chars.IsLetter) 
                            {
                                res.EndToken = t.Next.Next;
                                res.Number = string.Format("{0}{1}", res.Number, (res.EndToken as Pullenti.Ner.TextToken).Term);
                            }
                        }
                        else if (((t.Next is Pullenti.Ner.TextToken) && t.Next.LengthChar == 1 && t.Next.Chars.IsLetter) && !t.IsWhitespaceAfter) 
                        {
                            res.EndToken = t.Next;
                            res.Number = string.Format("{0}{1}", res.Number, (res.EndToken as Pullenti.Ner.TextToken).Term);
                        }
                        return res;
                    }
                }
            }
            if (((t is Pullenti.Ner.TextToken) && t.LengthChar == 1 && t.Chars.IsLetter) && ((!t.IsWhitespaceAfter || (((t.WhitespacesAfterCount < 2) && t.Chars.IsAllUpper))))) 
            {
                if (typ != null && typ.Typ != null && (((typ.Typ == "войсковая часть" || typ.Typ == "військова частина" || typ.Typ.Contains("колония")) || typ.Typ.Contains("колонія")))) 
                {
                    Pullenti.Ner.Token tt1 = t.Next;
                    if (tt1 != null && tt1.IsHiphen) 
                        tt1 = tt1.Next;
                    if (tt1 is Pullenti.Ner.NumberToken) 
                    {
                        OrgItemNumberToken res = new OrgItemNumberToken(t, tt1);
                        res.Number = string.Format("{0}{1}", (t as Pullenti.Ner.TextToken).Term, (tt1 as Pullenti.Ner.NumberToken).Value);
                        return res;
                    }
                }
            }
            return null;
        }
    }
}