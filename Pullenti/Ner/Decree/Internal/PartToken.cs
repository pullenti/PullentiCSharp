/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Decree.Internal
{
    // Примитив, из которых состоит часть декрета (статья, пункт и часть)
    public class PartToken : Pullenti.Ner.MetaToken
    {
        public PartToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public enum ItemType : int
        {
            Undefined,
            Prefix,
            Appendix,
            DocPart,
            Part,
            Section,
            SubSection,
            Chapter,
            Clause,
            Paragraph,
            Subparagraph,
            Item,
            SubItem,
            Indention,
            SubIndention,
            Preamble,
            Notice,
            Subprogram,
            Page,
            AddAgree,
        }

        /// <summary>
        /// Тип примитива
        /// </summary>
        public ItemType Typ;
        public ItemType AltTyp = ItemType.Undefined;
        public class PartValue : Pullenti.Ner.MetaToken
        {
            public PartValue(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
            {
            }
            public string Value;
            public string SourceValue
            {
                get
                {
                    Pullenti.Ner.Token t0 = BeginToken;
                    Pullenti.Ner.Token t1 = EndToken;
                    if (t1.IsChar('.')) 
                        t1 = t1.Previous;
                    else if (t1.IsChar(')') && !t0.IsChar('(')) 
                        t1 = t1.Previous;
                    return (new Pullenti.Ner.MetaToken(t0, t1)).GetSourceText();
                }
            }
            public int IntValue
            {
                get
                {
                    if (string.IsNullOrEmpty(Value)) 
                        return 0;
                    int num;
                    if (int.TryParse(Value, out num)) 
                        return num;
                    return 0;
                }
            }
            public override string ToString()
            {
                return Value;
            }
            public void CorrectValue()
            {
                if ((EndToken.Next is Pullenti.Ner.TextToken) && (EndToken.Next as Pullenti.Ner.TextToken).LengthChar == 1 && EndToken.Next.Chars.IsLetter) 
                {
                    if (!EndToken.IsWhitespaceAfter) 
                    {
                        Value += (EndToken.Next as Pullenti.Ner.TextToken).Term;
                        EndToken = EndToken.Next;
                    }
                    else if ((EndToken.WhitespacesAfterCount < 2) && EndToken.Next.Next != null && EndToken.Next.Next.IsChar(')')) 
                    {
                        Value += (EndToken.Next as Pullenti.Ner.TextToken).Term;
                        EndToken = EndToken.Next.Next;
                    }
                }
                if ((Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(EndToken.Next, false, false) && (EndToken.Next.Next is Pullenti.Ner.TextToken) && EndToken.Next.Next.LengthChar == 1) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(EndToken.Next.Next.Next, false, EndToken.Next, false)) 
                {
                    Value = string.Format("{0}.{1}", Value, (EndToken.Next.Next as Pullenti.Ner.TextToken).Term);
                    EndToken = EndToken.Next.Next.Next;
                }
                for (Pullenti.Ner.Token t = EndToken.Next; t != null; t = t.Next) 
                {
                    if (t.IsWhitespaceBefore) 
                    {
                        if (t.WhitespacesBeforeCount > 1) 
                            break;
                        if (((t is Pullenti.Ner.TextToken) && t.LengthChar == 1 && t.Next != null) && t.Next.IsChar(')')) 
                        {
                            Value = string.Format("{0}.{1}", Value ?? "", (t as Pullenti.Ner.TextToken).Term);
                            EndToken = (t = t.Next);
                        }
                        break;
                    }
                    if (t.IsCharOf("_.") && !t.IsWhitespaceAfter) 
                    {
                        if (t.Next is Pullenti.Ner.NumberToken) 
                        {
                            Value = string.Format("{0}.{1}", Value ?? "", (t.Next as Pullenti.Ner.NumberToken).Value);
                            EndToken = (t = t.Next);
                            continue;
                        }
                        if (((t.Next != null && t.Next.IsChar('(') && (t.Next.Next is Pullenti.Ner.NumberToken)) && !t.Next.IsWhitespaceAfter && t.Next.Next.Next != null) && t.Next.Next.Next.IsChar(')')) 
                        {
                            Value = string.Format("{0}.{1}", Value ?? "", (t.Next.Next as Pullenti.Ner.NumberToken).Value);
                            EndToken = t.Next.Next.Next;
                            continue;
                        }
                    }
                    if ((t.IsHiphen && !t.IsWhitespaceAfter && (t.Next is Pullenti.Ner.NumberToken)) && (t.Next as Pullenti.Ner.NumberToken).IntValue != null) 
                    {
                        int n1;
                        if (int.TryParse(Value, out n1)) 
                        {
                            if (n1 >= (t.Next as Pullenti.Ner.NumberToken).IntValue.Value) 
                            {
                                Value = string.Format("{0}.{1}", Value ?? "", (t.Next as Pullenti.Ner.NumberToken).Value);
                                EndToken = (t = t.Next);
                                continue;
                            }
                        }
                    }
                    if ((t.IsCharOf("(<") && (t.Next is Pullenti.Ner.NumberToken) && t.Next.Next != null) && t.Next.Next.IsCharOf(")>")) 
                    {
                        Value = string.Format("{0}.{1}", Value ?? "", (t.Next as Pullenti.Ner.NumberToken).Value);
                        EndToken = (t = t.Next.Next);
                        if (t.Next != null && t.Next.IsChar('.') && !t.IsWhitespaceAfter) 
                            t = t.Next;
                        continue;
                    }
                    break;
                }
                if (EndToken.Next != null && EndToken.Next.IsCharOf(".") && !EndToken.IsWhitespaceAfter) 
                {
                    if (EndToken.Next.Next != null && (EndToken.Next.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && !EndToken.Next.IsNewlineAfter) 
                        EndToken = EndToken.Next;
                }
                if (BeginToken == EndToken && EndToken.Next != null && EndToken.Next.IsChar(')')) 
                {
                    bool ok = true;
                    int lev = 0;
                    for (Pullenti.Ner.Token ttt = BeginToken.Previous; ttt != null; ttt = ttt.Previous) 
                    {
                        if (ttt.IsNewlineAfter) 
                            break;
                        if (ttt.IsChar(')')) 
                            lev++;
                        else if (ttt.IsChar('(')) 
                        {
                            lev--;
                            if (lev < 0) 
                            {
                                ok = false;
                                break;
                            }
                        }
                    }
                    if (ok) 
                    {
                        Pullenti.Ner.Token tt = EndToken.Next.Next;
                        if (tt != null) 
                        {
                            if ((tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) || Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt, null, false, false) != null) 
                                EndToken = EndToken.Next;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Значения
        /// </summary>
        public List<PartValue> Values = new List<PartValue>();
        public string Name;
        /// <summary>
        /// Эо для последующего перебора
        /// </summary>
        public int Ind;
        public Pullenti.Ner.Decree.DecreeReferent Decree;
        public bool IsDoubt;
        public bool DelimAfter;
        public bool HasTerminator;
        /// <summary>
        /// Анафорическая ссылка
        /// </summary>
        public Pullenti.Ner.TextToken AnaforRef;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder(Typ.ToString());
            foreach (PartValue v in Values) 
            {
                res.AppendFormat(" {0}", v);
            }
            if (DelimAfter) 
                res.Append(", DelimAfter");
            if (IsDoubt) 
                res.Append(", Doubt");
            if (HasTerminator) 
                res.Append(", Terminator");
            if (AnaforRef != null) 
                res.AppendFormat(", Ref='{0}'", AnaforRef.Term);
            return res.ToString();
        }
        /// <summary>
        /// Привязать с указанной позиции один примитив
        /// </summary>
        public static PartToken TryAttach(Pullenti.Ner.Token t, PartToken prev, bool inBracket = false, bool ignoreNumber = false)
        {
            if (t == null) 
                return null;
            PartToken res = null;
            if (t.Morph.Class.IsPersonalPronoun && (t.WhitespacesAfterCount < 2)) 
            {
                res = TryAttach(t.Next, prev, false, false);
                if (res != null) 
                {
                    res.AnaforRef = t as Pullenti.Ner.TextToken;
                    res.BeginToken = t;
                    return res;
                }
            }
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if ((t is Pullenti.Ner.NumberToken) && t.Next != null && (t.WhitespacesAfterCount < 3)) 
            {
                PartToken re = _createPartTyp0(t.Next, prev);
                if (re != null) 
                {
                    Pullenti.Ner.Token t11 = re.EndToken.Next;
                    bool ok1 = false;
                    if (t11 != null && (t11.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                        ok1 = true;
                    else if (prev != null && t11 != null && !(t11 is Pullenti.Ner.NumberToken)) 
                        ok1 = true;
                    if (!ok1) 
                    {
                        PartToken res1 = TryAttach(t11, null, false, false);
                        if (res1 != null) 
                            ok1 = true;
                    }
                    if (ok1 || inBracket) 
                    {
                        re.BeginToken = t;
                        re.Values.Add(new PartValue(t, t) { Value = (t as Pullenti.Ner.NumberToken).Value.ToString() });
                        return re;
                    }
                }
            }
            if (((t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit && prev == null) && t.Previous != null) 
            {
                Pullenti.Ner.Token t0 = t.Previous;
                bool delim = false;
                if (t0.IsChar(',') || t0.Morph.Class.IsConjunction) 
                {
                    delim = true;
                    t0 = t0.Previous;
                }
                if (t0 == null) 
                    return null;
                Pullenti.Ner.Decree.DecreePartReferent dr = t0.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                if (dr == null) 
                {
                    if (t0.IsChar('(') && t.Next != null) 
                    {
                        if (t.Next.IsValue("ЧАСТЬ", null) || t.Next.IsValue("Ч", null)) 
                        {
                            Pullenti.Ner.Token te = t.Next;
                            if (te.Next != null && te.Next.IsChar('.')) 
                                te = te.Next;
                            res = new PartToken(t, te) { Typ = ItemType.Part };
                            res.Values.Add(new PartValue(t, t) { Value = (t as Pullenti.Ner.NumberToken).Value.ToString() });
                            return res;
                        }
                    }
                    return null;
                }
                if (dr.Clause == null) 
                    return null;
                res = new PartToken(t, t) { Typ = ItemType.Clause, IsDoubt = !delim };
                PartValue pv = new PartValue(t, t) { Value = (t as Pullenti.Ner.NumberToken).Value.ToString() };
                res.Values.Add(pv);
                for (t = t.Next; t != null; t = t.Next) 
                {
                    if (t.IsWhitespaceBefore) 
                        break;
                    else if (t.IsCharOf("._") && (t.Next is Pullenti.Ner.NumberToken)) 
                    {
                        t = t.Next;
                        pv.EndToken = (res.EndToken = t);
                        pv.Value = string.Format("{0}.{1}", pv.Value, (t as Pullenti.Ner.NumberToken).Value);
                    }
                    else 
                        break;
                }
                return res;
            }
            if (((t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit && prev != null) && prev.Typ == ItemType.Prefix && (t.WhitespacesBeforeCount < 3)) 
            {
                PartValue pv = new PartValue(t, t) { Value = (t as Pullenti.Ner.NumberToken).Value.ToString() };
                pv.CorrectValue();
                Pullenti.Ner.Token ttt1 = pv.EndToken.Next;
                DecreeToken ne = DecreeToken.TryAttach(ttt1, null, false);
                bool ok = false;
                if (ne != null && ne.Typ == DecreeToken.ItemType.Typ) 
                    ok = true;
                else if (Pullenti.Ner.Decree.DecreeAnalyzer._checkOtherTyp(ttt1, true) != null) 
                    ok = true;
                else if (Pullenti.Ner.Decree.DecreeAnalyzer._getDecree(ttt1) != null) 
                    ok = true;
                if (ok) 
                {
                    res = new PartToken(t, pv.EndToken) { Typ = ItemType.Item };
                    res.Values.Add(pv);
                    return res;
                }
            }
            if (tt == null) 
                return null;
            if (tt.LengthChar == 1 && !tt.Chars.IsAllLower) 
            {
                if (!Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                    return null;
            }
            Pullenti.Ner.Token t1 = (Pullenti.Ner.Token)tt;
            res = _createPartTyp0(t1, prev);
            if (res != null) 
                t1 = res.EndToken;
            else if ((t1.IsValue("СИЛУ", null) || t1.IsValue("СОГЛАСНО", null) || t1.IsValue("СООТВЕТСТВИЕ", null)) || t1.IsValue("ПОЛОЖЕНИЕ", null)) 
            {
                if (t1.IsValue("СИЛУ", null) && t1.Previous != null && t1.Previous.Morph.Class.IsVerb) 
                    return null;
                res = new PartToken(t1, t1) { Typ = ItemType.Prefix };
                if (t1.Next != null && t1.Next.IsValue("С", null)) 
                    res.EndToken = t1.Next;
                return res;
            }
            else if (((t1.IsValue("УГОЛОВНОЕ", null) || t1.IsValue("КРИМІНАЛЬНА", null))) && t1.Next != null && ((t1.Next.IsValue("ДЕЛО", null) || t1.Next.IsValue("СПРАВА", null)))) 
            {
                t1 = t1.Next;
                if (t1.Next != null && t1.Next.IsValue("ПО", null)) 
                    t1 = t1.Next;
                return new PartToken(t, t1) { Typ = ItemType.Prefix };
            }
            else if ((((t1.IsValue("МОТИВИРОВОЧНЫЙ", null) || t1.IsValue("МОТИВУВАЛЬНИЙ", null) || t1.IsValue("РЕЗОЛЮТИВНЫЙ", null)) || t1.IsValue("РЕЗОЛЮТИВНИЙ", null))) && t1.Next != null && ((t1.Next.IsValue("ЧАСТЬ", null) || t1.Next.IsValue("ЧАСТИНА", null)))) 
            {
                PartToken rr = new PartToken(t1, t1.Next) { Typ = ItemType.Part };
                rr.Values.Add(new PartValue(t1, t1) { Value = (t1.IsValue("МОТИВИРОВОЧНЫЙ", null) || t1.IsValue("МОТИВУВАЛЬНИЙ", null) ? "мотивировочная" : "резолютивная") });
                return rr;
            }
            if (res == null) 
                return null;
            if (ignoreNumber) 
                return res;
            if (res.IsNewlineAfter) 
            {
                if (res.Chars.IsAllUpper) 
                    return null;
            }
            if (t1.Next != null && t1.Next.IsChar('.')) 
            {
                if (!t1.Next.IsNewlineAfter || (t1.LengthChar < 3)) 
                    t1 = t1.Next;
            }
            t1 = t1.Next;
            if (t1 == null) 
                return null;
            if (res.Typ == ItemType.Clause) 
            {
                if (((t1 is Pullenti.Ner.NumberToken) && (t1 as Pullenti.Ner.NumberToken).Value == "3" && !t1.IsWhitespaceAfter) && t1.Next != null && t1.Next.LengthChar == 2) 
                    return null;
            }
            if (res.Typ == ItemType.Clause && t1.IsValue("СТ", null)) 
            {
                t1 = t1.Next;
                if (t1 != null && t1.IsChar('.')) 
                    t1 = t1.Next;
            }
            else if (res.Typ == ItemType.Part && t1.IsValue("Ч", null)) 
            {
                t1 = t1.Next;
                if (t1 != null && t1.IsChar('.')) 
                    t1 = t1.Next;
            }
            else if (res.Typ == ItemType.Item && t1.IsValue("П", null)) 
            {
                t1 = t1.Next;
                if (t1 != null && t1.IsChar('.')) 
                    t1 = t1.Next;
                res.AltTyp = ItemType.SubItem;
            }
            else if ((res.Typ == ItemType.Item && t1.IsCharOf("\\/") && t1.Next != null) && t1.Next.IsValue("П", null)) 
            {
                t1 = t1.Next.Next;
                if (t1 != null && t1.IsChar('.')) 
                    t1 = t1.Next;
                res.AltTyp = ItemType.SubItem;
            }
            if (t1 == null) 
                return null;
            if (res.Typ == ItemType.Clause && (t1.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && t1.Next != null) 
            {
                res.Decree = t1.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                t1 = t1.Next;
            }
            Pullenti.Ner.Token ttn = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t1);
            Pullenti.Ner.TextToken firstNumPrefix = null;
            if (ttn != null) 
            {
                firstNumPrefix = t1 as Pullenti.Ner.TextToken;
                t1 = ttn;
            }
            if (t1 == null) 
                return null;
            res.EndToken = t1;
            bool and = false;
            Pullenti.Ner.NumberSpellingType ntyp = Pullenti.Ner.NumberSpellingType.Digit;
            Pullenti.Ner.Token tt1 = t1;
            while (t1 != null) 
            {
                if (t1.WhitespacesBeforeCount > 15) 
                    break;
                if (t1 != tt1 && t1.IsNewlineBefore) 
                    break;
                if (ttn != null) 
                {
                    ttn = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t1);
                    if (ttn != null) 
                        t1 = ttn;
                }
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t1, false, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br == null) 
                        break;
                    bool ok = true;
                    List<PartValue> newP = null;
                    for (Pullenti.Ner.Token ttt = t1.Next; ttt != null; ttt = ttt.Next) 
                    {
                        if (ttt.EndChar > br.EndToken.Previous.EndChar) 
                            break;
                        if (ttt.IsChar(',')) 
                            continue;
                        if (ttt is Pullenti.Ner.NumberToken) 
                        {
                            if ((ttt as Pullenti.Ner.NumberToken).Value == "0") 
                            {
                                ok = false;
                                break;
                            }
                            if (newP == null) 
                                newP = new List<PartValue>();
                            newP.Add(new PartValue(ttt, ttt) { Value = (ttt as Pullenti.Ner.NumberToken).Value.ToString() });
                            continue;
                        }
                        Pullenti.Ner.TextToken to = ttt as Pullenti.Ner.TextToken;
                        if (to == null) 
                        {
                            ok = false;
                            break;
                        }
                        if ((res.Typ != ItemType.Item && res.Typ != ItemType.SubItem && res.Typ != ItemType.Indention) && res.Typ != ItemType.SubIndention) 
                        {
                            ok = false;
                            break;
                        }
                        if (!to.Chars.IsLetter || to.LengthChar != 1) 
                        {
                            ok = false;
                            break;
                        }
                        if (newP == null) 
                            newP = new List<PartValue>();
                        PartValue pv = new PartValue(ttt, ttt) { Value = to.Term };
                        if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(ttt.Previous, false, false)) 
                            pv.BeginToken = ttt.Previous;
                        if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(ttt.Next, false, null, false)) 
                            pv.EndToken = ttt.Next;
                        newP.Add(pv);
                    }
                    if (newP == null || !ok) 
                        break;
                    res.Values.AddRange(newP);
                    res.EndToken = br.EndToken;
                    t1 = br.EndToken.Next;
                    if (and) 
                        break;
                    if (t1 != null && t1.IsHiphen && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t1.Next, false, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br1 = Pullenti.Ner.Core.BracketHelper.TryParse(t1.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if ((br1 != null && (t1.Next.Next is Pullenti.Ner.TextToken) && t1.Next.Next.LengthChar == 1) && t1.Next.Next.Next == br1.EndToken) 
                        {
                            res.Values.Add(new PartValue(br1.BeginToken, br1.EndToken) { Value = (t1.Next.Next as Pullenti.Ner.TextToken).Term });
                            res.EndToken = br1.EndToken;
                            t1 = br1.EndToken.Next;
                        }
                    }
                    continue;
                }
                if (((t1 is Pullenti.Ner.TextToken) && t1.LengthChar == 1 && t1.Chars.IsLetter) && res.Values.Count == 0) 
                {
                    if (t1.Chars.IsAllUpper && res.Typ == ItemType.Subprogram) 
                    {
                        res.Values.Add(new PartValue(t1, t1) { Value = (t1 as Pullenti.Ner.TextToken).Term });
                        res.EndToken = t1;
                        return res;
                    }
                    bool ok = true;
                    int lev = 0;
                    for (Pullenti.Ner.Token ttt = t1.Previous; ttt != null; ttt = ttt.Previous) 
                    {
                        if (ttt.IsNewlineAfter) 
                            break;
                        if (ttt.IsChar('(')) 
                        {
                            lev--;
                            if (lev < 0) 
                            {
                                ok = false;
                                break;
                            }
                        }
                        else if (ttt.IsChar(')')) 
                            lev++;
                    }
                    if (ok && t1.Next != null && t1.Next.IsChar(')')) 
                    {
                        res.Values.Add(new PartValue(t1, t1.Next) { Value = (t1 as Pullenti.Ner.TextToken).Term });
                        res.EndToken = t1.Next;
                        t1 = t1.Next.Next;
                        continue;
                    }
                    if (((ok && t1.Next != null && t1.Next.IsChar('.')) && !t1.Next.IsWhitespaceAfter && (t1.Next.Next is Pullenti.Ner.NumberToken)) && t1.Next.Next.Next != null && t1.Next.Next.Next.IsChar(')')) 
                    {
                        res.Values.Add(new PartValue(t1, t1.Next.Next.Next) { Value = string.Format("{0}.{1}", (t1 as Pullenti.Ner.TextToken).Term, (t1.Next.Next as Pullenti.Ner.NumberToken).Value) });
                        res.EndToken = t1.Next.Next.Next;
                        t1 = res.EndToken.Next;
                        continue;
                    }
                }
                Pullenti.Ner.Token prefTo = null;
                if (res.Values.Count > 0 && !(t1 is Pullenti.Ner.NumberToken) && firstNumPrefix != null) 
                {
                    ttn = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t1);
                    if (ttn != null) 
                    {
                        prefTo = t1;
                        t1 = ttn;
                    }
                }
                if (t1 is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.Token tt0 = prefTo ?? t1;
                    if (res.Values.Count > 0) 
                    {
                        if (res.Values[0].IntValue == 0 && !char.IsDigit(res.Values[0].Value[0])) 
                            break;
                        if ((t1 as Pullenti.Ner.NumberToken).Typ != ntyp) 
                            break;
                    }
                    ntyp = (t1 as Pullenti.Ner.NumberToken).Typ;
                    PartValue val = new PartValue(tt0, t1) { Value = (t1 as Pullenti.Ner.NumberToken).Value.ToString() };
                    val.CorrectValue();
                    res.Values.Add(val);
                    res.EndToken = val.EndToken;
                    t1 = res.EndToken.Next;
                    if (and) 
                        break;
                    continue;
                }
                Pullenti.Ner.NumberToken nt = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t1);
                if (nt != null) 
                {
                    PartValue pv = new PartValue(t1, nt.EndToken) { Value = nt.Value.ToString() };
                    res.Values.Add(pv);
                    pv.CorrectValue();
                    res.EndToken = pv.EndToken;
                    t1 = res.EndToken.Next;
                    continue;
                }
                if ((t1 == tt1 && ((res.Typ == ItemType.Appendix || res.Typ == ItemType.AddAgree)) && t1.IsValue("К", null)) && t1.Next != null && (t1.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                {
                    res.Values.Add(new PartValue(t1, t1) { Value = "" });
                    break;
                }
                if (res.Typ == ItemType.AddAgree && firstNumPrefix != null && res.Values.Count == 0) 
                {
                    DecreeToken ddd = DecreeToken.TryAttach(firstNumPrefix, null, false);
                    if (ddd != null && ddd.Typ == DecreeToken.ItemType.Number && ddd.Value != null) 
                    {
                        res.Values.Add(new PartValue(t1, ddd.EndToken) { Value = ddd.Value });
                        t1 = (res.EndToken = ddd.EndToken);
                        break;
                    }
                }
                if (res.Values.Count == 0) 
                    break;
                if (t1.IsCharOf(",.")) 
                {
                    if (t1.IsNewlineAfter && t1.IsChar('.')) 
                        break;
                    t1 = t1.Next;
                    continue;
                }
                if (t1.IsHiphen && res.Values[res.Values.Count - 1].Value.IndexOf('.') > 0) 
                {
                    t1 = t1.Next;
                    continue;
                }
                if (t1.IsAnd || t1.IsOr) 
                {
                    t1 = t1.Next;
                    and = true;
                    continue;
                }
                if (t1.IsHiphen) 
                {
                    if (!(t1.Next is Pullenti.Ner.NumberToken) || (t1.Next as Pullenti.Ner.NumberToken).IntValue == null) 
                        break;
                    int min = res.Values[res.Values.Count - 1].IntValue;
                    if (min == 0) 
                        break;
                    int max = (t1.Next as Pullenti.Ner.NumberToken).IntValue.Value;
                    if (max < min) 
                        break;
                    if ((max - min) > 200) 
                        break;
                    PartValue val = new PartValue(t1.Next, t1.Next) { Value = max.ToString() };
                    val.CorrectValue();
                    res.Values.Add(val);
                    res.EndToken = val.EndToken;
                    t1 = res.EndToken.Next;
                    continue;
                }
                break;
            }
            if (res.Values.Count == 0 && !res.IsNewlineAfter && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(res.EndToken, true, false)) 
            {
                int lev = _getRank(res.Typ);
                if (lev > 0 && (lev < _getRank(ItemType.Clause))) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(res.EndToken, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        res.Name = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                        res.EndToken = br.EndToken;
                    }
                }
            }
            if (res.Values.Count == 0 && res.Name == null) 
            {
                if (!ignoreNumber && res.Typ != ItemType.Preamble && res.Typ != ItemType.Subprogram) 
                    return null;
                if (res.BeginToken != res.EndToken) 
                    res.EndToken = res.EndToken.Previous;
            }
            return res;
        }
        static PartToken _createPartTyp0(Pullenti.Ner.Token t1, PartToken prev)
        {
            bool isShort;
            PartToken pt = __createPartTyp(t1, prev, out isShort);
            if (pt == null) 
                return null;
            if ((isShort && !pt.EndToken.IsWhitespaceAfter && pt.EndToken.Next != null) && pt.EndToken.Next.IsChar('.')) 
            {
                if (!pt.EndToken.Next.IsNewlineAfter) 
                    pt.EndToken = pt.EndToken.Next;
            }
            return pt;
        }
        static PartToken __createPartTyp(Pullenti.Ner.Token t1, PartToken prev, out bool isShort)
        {
            isShort = false;
            if (t1 == null) 
                return null;
            if (t1.IsValue("ЧАСТЬ", "ЧАСТИНА")) 
                return new PartToken(t1, t1) { Typ = ItemType.Part };
            if (t1.IsValue("Ч", null)) 
            {
                isShort = true;
                return new PartToken(t1, t1) { Typ = ItemType.Part };
            }
            if (t1.IsValue("ГЛАВА", null) || t1.IsValue("ГЛ", null)) 
            {
                isShort = t1.LengthChar == 2;
                return new PartToken(t1, t1) { Typ = ItemType.Chapter };
            }
            if (t1.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК") || t1.IsValue("ПРИЛ", null)) 
            {
                if ((t1.IsNewlineBefore && t1.LengthChar > 6 && t1.Next != null) && t1.Next.IsChar(':')) 
                    return null;
                isShort = t1.LengthChar < 5;
                return new PartToken(t1, t1) { Typ = ItemType.Appendix };
            }
            if (t1.IsValue("ПРИМЕЧАНИЕ", "ПРИМІТКА") || t1.IsValue("ПРИМ", null)) 
            {
                isShort = t1.LengthChar < 5;
                return new PartToken(t1, t1) { Typ = ItemType.Notice };
            }
            if (t1.IsValue("СТАТЬЯ", "СТАТТЯ") || t1.IsValue("СТ", null)) 
            {
                isShort = t1.LengthChar < 3;
                return new PartToken(t1, t1) { Typ = ItemType.Clause };
            }
            if (t1.IsValue("ПУНКТ", null) || t1.IsValue("П", null) || t1.IsValue("ПП", null)) 
            {
                isShort = t1.LengthChar < 3;
                return new PartToken(t1, t1) { Typ = ItemType.Item, AltTyp = (t1.IsValue("ПП", null) ? ItemType.SubItem : ItemType.Undefined) };
            }
            if (t1.IsValue("ПОДПУНКТ", "ПІДПУНКТ")) 
                return new PartToken(t1, t1) { Typ = ItemType.SubItem };
            if (t1.IsValue("ПРЕАМБУЛА", null)) 
                return new PartToken(t1, t1) { Typ = ItemType.Preamble };
            if (t1.IsValue("ПОДП", null) || t1.IsValue("ПІДП", null)) 
            {
                isShort = true;
                return new PartToken(t1, t1) { Typ = ItemType.SubItem };
            }
            if (t1.IsValue("РАЗДЕЛ", "РОЗДІЛ") || t1.IsValue("РАЗД", null)) 
            {
                isShort = t1.LengthChar < 5;
                return new PartToken(t1, t1) { Typ = ItemType.Section };
            }
            if (((t1.IsValue("Р", null) || t1.IsValue("P", null))) && t1.Next != null && t1.Next.IsChar('.')) 
            {
                if (prev != null) 
                {
                    if (prev.Typ == ItemType.Item || prev.Typ == ItemType.SubItem) 
                    {
                        isShort = true;
                        return new PartToken(t1, t1.Next) { Typ = ItemType.Section };
                    }
                }
            }
            if (t1.IsValue("ПОДРАЗДЕЛ", "ПІРОЗДІЛ")) 
                return new PartToken(t1, t1) { Typ = ItemType.SubSection };
            if (t1.IsValue("ПАРАГРАФ", null) || t1.IsValue("§", null)) 
                return new PartToken(t1, t1) { Typ = ItemType.Paragraph };
            if (t1.IsValue("АБЗАЦ", null) || t1.IsValue("АБЗ", null)) 
            {
                isShort = t1.LengthChar < 7;
                return new PartToken(t1, t1) { Typ = ItemType.Indention };
            }
            if (t1.IsValue("СТРАНИЦА", "СТОРІНКА") || t1.IsValue("СТР", "СТОР")) 
            {
                isShort = t1.LengthChar < 7;
                return new PartToken(t1, t1) { Typ = ItemType.Page };
            }
            if (t1.IsValue("ПОДАБЗАЦ", "ПІДАБЗАЦ") || t1.IsValue("ПОДАБЗ", "ПІДАБЗ")) 
                return new PartToken(t1, t1) { Typ = ItemType.SubIndention };
            if (t1.IsValue("ПОДПАРАГРАФ", "ПІДПАРАГРАФ")) 
                return new PartToken(t1, t1) { Typ = ItemType.Subparagraph };
            if (t1.IsValue("ПОДПРОГРАММА", "ПІДПРОГРАМА")) 
                return new PartToken(t1, t1) { Typ = ItemType.Subprogram };
            if (t1.IsValue("ДОПСОГЛАШЕНИЕ", null)) 
                return new PartToken(t1, t1) { Typ = ItemType.AddAgree };
            if (((t1.IsValue("ДОП", null) || t1.IsValue("ДОПОЛНИТЕЛЬНЫЙ", "ДОДАТКОВА"))) && t1.Next != null) 
            {
                Pullenti.Ner.Token tt = t1.Next;
                if (tt.IsChar('.') && tt.Next != null) 
                    tt = tt.Next;
                if (tt.IsValue("СОГЛАШЕНИЕ", "УГОДА")) 
                    return new PartToken(t1, tt) { Typ = ItemType.AddAgree };
            }
            return null;
        }
        /// <summary>
        /// Привязать примитивы в контейнере с указанной позиции
        /// </summary>
        /// <return>Список примитивов</return>
        public static List<PartToken> TryAttachList(Pullenti.Ner.Token t, bool inBracket = false, int maxCount = 40)
        {
            PartToken p = TryAttach(t, null, inBracket, false);
            if (p == null) 
                return null;
            List<PartToken> res = new List<PartToken>();
            res.Add(p);
            if (p.IsNewlineAfter && p.IsNewlineBefore) 
            {
                if (!p.BeginToken.Chars.IsAllLower) 
                    return res;
            }
            Pullenti.Ner.Token tt = p.EndToken.Next;
            while (tt != null) 
            {
                if (tt.WhitespacesBeforeCount > 15) 
                {
                    if (tt.Previous != null && tt.Previous.IsCommaAnd) 
                    {
                    }
                    else 
                        break;
                }
                if (maxCount > 0 && res.Count >= maxCount) 
                    break;
                bool delim = false;
                if (((tt.IsCharOf(",;.") || tt.IsAnd || tt.IsOr)) && tt.Next != null) 
                {
                    if (tt.IsCharOf(";.")) 
                        res[res.Count - 1].HasTerminator = true;
                    else 
                    {
                        res[res.Count - 1].DelimAfter = true;
                        if ((tt.Next != null && tt.Next.IsValue("А", null) && tt.Next.Next != null) && tt.Next.Next.IsValue("ТАКЖЕ", "ТАКОЖ")) 
                            tt = tt.Next.Next;
                    }
                    tt = tt.Next;
                    delim = true;
                }
                if (tt == null) 
                    break;
                if (tt.IsNewlineBefore) 
                {
                    if (tt.Chars.IsLetter && !tt.Chars.IsAllLower) 
                        break;
                }
                if (tt.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        List<PartToken> li = TryAttachList(tt.Next, true, 40);
                        if (li != null && li.Count > 0) 
                        {
                            if (li[0].Typ == ItemType.Paragraph || li[0].Typ == ItemType.Part || li[0].Typ == ItemType.Item) 
                            {
                                if (li[li.Count - 1].EndToken.Next == br.EndToken) 
                                {
                                    if (p.Values.Count > 1) 
                                    {
                                        for (int ii = 1; ii < p.Values.Count; ii++) 
                                        {
                                            PartToken pp = new PartToken(p.Values[ii].BeginToken, (ii == (p.Values.Count - 1) ? p.EndToken : p.Values[ii].EndToken)) { Typ = p.Typ };
                                            pp.Values.Add(p.Values[ii]);
                                            res.Add(pp);
                                        }
                                        if (p.Values[1].BeginToken.Previous != null && p.Values[1].BeginToken.Previous.EndChar >= p.BeginToken.BeginChar) 
                                            p.EndToken = p.Values[1].BeginToken.Previous;
                                        p.Values.RemoveRange(1, p.Values.Count - 1);
                                    }
                                    res.AddRange(li);
                                    li[li.Count - 1].EndToken = br.EndToken;
                                    tt = br.EndToken.Next;
                                    continue;
                                }
                            }
                        }
                    }
                }
                PartToken p0 = TryAttach(tt, p, inBracket, false);
                if (p0 == null && ((tt.IsValue("В", null) || tt.IsValue("К", null) || tt.IsValue("ДО", null)))) 
                    p0 = TryAttach(tt.Next, p, inBracket, false);
                if (p0 == null) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            p0 = TryAttach(br.EndToken.Next, null, false, false);
                            if (p0 != null && p0.Typ != ItemType.Prefix && p0.Values.Count > 0) 
                            {
                                res[res.Count - 1].EndToken = br.EndToken;
                                res[res.Count - 1].Name = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                                if (p0.IsNewlineBefore) 
                                    break;
                                p = p0;
                                res.Add(p);
                                tt = p.EndToken.Next;
                                continue;
                            }
                            if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, true) && (tt.WhitespacesBeforeCount < 3) && res[res.Count - 1].Typ == ItemType.Appendix) 
                            {
                                res[res.Count - 1].EndToken = br.EndToken;
                                res[res.Count - 1].Name = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                                tt = br.EndToken.Next;
                                continue;
                            }
                        }
                    }
                    if (tt.IsNewlineBefore) 
                    {
                        if (res.Count == 1 && res[0].IsNewlineBefore) 
                            break;
                        if (tt.Previous != null && tt.Previous.IsCommaAnd) 
                        {
                        }
                        else 
                            break;
                    }
                    if ((tt is Pullenti.Ner.NumberToken) && delim) 
                    {
                        p0 = null;
                        if (p.Typ == ItemType.Clause || inBracket) 
                            p0 = new PartToken(tt, tt) { Typ = ItemType.Clause };
                        else if (res.Count > 1 && res[res.Count - 2].Typ == ItemType.Clause && res[res.Count - 1].Typ == ItemType.Part) 
                            p0 = new PartToken(tt, tt) { Typ = ItemType.Clause };
                        else if ((res.Count > 2 && res[res.Count - 3].Typ == ItemType.Clause && res[res.Count - 2].Typ == ItemType.Part) && res[res.Count - 1].Typ == ItemType.Item) 
                            p0 = new PartToken(tt, tt) { Typ = ItemType.Clause };
                        else if (res.Count > 0 && res[res.Count - 1].Values.Count > 0 && res[res.Count - 1].Values[0].Value.Contains(".")) 
                            p0 = new PartToken(tt, tt) { Typ = res[res.Count - 1].Typ };
                        if (p0 == null) 
                            break;
                        PartValue vv = new PartValue(tt, tt) { Value = (tt as Pullenti.Ner.NumberToken).Value.ToString() };
                        p0.Values.Add(vv);
                        vv.CorrectValue();
                        p0.EndToken = vv.EndToken;
                        tt = p0.EndToken.Next;
                        if (tt != null && tt.IsHiphen && (tt.Next is Pullenti.Ner.NumberToken)) 
                        {
                            tt = tt.Next;
                            vv = new PartValue(tt, tt) { Value = (tt as Pullenti.Ner.NumberToken).Value.ToString() };
                            vv.CorrectValue();
                            p0.Values.Add(vv);
                            p0.EndToken = vv.EndToken;
                            tt = p0.EndToken.Next;
                        }
                    }
                }
                if (tt.IsChar(',') && !tt.IsNewlineAfter) 
                {
                    PartToken p1 = TryAttach(tt.Next, p, false, false);
                    if (p1 != null && _getRank(p1.Typ) > 0 && _getRank(p.Typ) > 0) 
                    {
                        if (_getRank(p1.Typ) < _getRank(p.Typ)) 
                            p0 = p1;
                    }
                }
                if (p0 == null) 
                    break;
                if (p0.IsNewlineBefore && res.Count == 1 && res[0].IsNewlineBefore) 
                    break;
                if (p0.Typ == ItemType.Item && p.Typ == ItemType.Item) 
                {
                    if (p0.AltTyp == ItemType.Undefined && p.AltTyp == ItemType.SubItem) 
                    {
                        p.Typ = ItemType.SubItem;
                        p.AltTyp = ItemType.Undefined;
                    }
                    else if (p.AltTyp == ItemType.Undefined && p0.AltTyp == ItemType.SubItem) 
                    {
                        p0.Typ = ItemType.SubItem;
                        p0.AltTyp = ItemType.Undefined;
                    }
                }
                p = p0;
                res.Add(p);
                tt = p.EndToken.Next;
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (res[i].Typ == ItemType.Part && res[i + 1].Typ == ItemType.Part && res[i].Values.Count > 1) 
                {
                    int v1 = res[i].Values[res[i].Values.Count - 2].IntValue;
                    int v2 = res[i].Values[res[i].Values.Count - 1].IntValue;
                    if (v1 == 0 || v2 == 0) 
                        continue;
                    if ((v2 - v1) < 10) 
                        continue;
                    PartToken pt = new PartToken(res[i].EndToken, res[i].EndToken) { Typ = ItemType.Clause };
                    pt.Values.Add(new PartValue(res[i].EndToken, res[i].EndToken) { Value = v2.ToString() });
                    res[i].Values.RemoveAt(res[i].Values.Count - 1);
                    if (res[i].EndToken != res[i].BeginToken) 
                        res[i].EndToken = res[i].EndToken.Previous;
                    res.Insert(i + 1, pt);
                }
            }
            if ((res.Count == 1 && res[0].Typ == ItemType.Subprogram && res[0].EndToken.Next != null) && res[0].EndToken.Next.IsChar('.')) 
                res[0].EndToken = res[0].EndToken.Next;
            for (int i = res.Count - 1; i >= 0; i--) 
            {
                p = res[i];
                if (p.IsNewlineAfter && p.IsNewlineBefore && p.Typ != ItemType.Subprogram) 
                {
                    res.RemoveRange(i, res.Count - i);
                    continue;
                }
                if (((i == 0 && p.IsNewlineBefore && p.HasTerminator) && p.EndToken.Next != null && p.EndToken.Next.IsChar('.')) && Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(p.EndToken.Next.Next)) 
                {
                    res.RemoveAt(i);
                    continue;
                }
            }
            return (res.Count == 0 ? null : res);
        }
        public bool CanBeNextNarrow(PartToken p)
        {
            if (Typ == p.Typ) 
            {
                if (Typ != ItemType.SubItem) 
                    return false;
                if (p.Values != null && p.Values.Count > 0 && p.Values[0].IntValue == 0) 
                {
                    if (Values != null && Values.Count > 0 && Values[0].IntValue > 0) 
                        return true;
                }
                return false;
            }
            if (Typ == ItemType.Part || p.Typ == ItemType.Part) 
                return true;
            int i1 = _getRank(Typ);
            int i2 = _getRank(p.Typ);
            if (i1 >= 0 && i2 >= 0) 
                return i1 < i2;
            return false;
        }
        public static bool IsPartBefore(Pullenti.Ner.Token t0)
        {
            if (t0 == null) 
                return false;
            int i = 0;
            for (Pullenti.Ner.Token tt = t0.Previous; tt != null; tt = tt.Previous) 
            {
                if (tt.IsNewlineAfter || (tt is Pullenti.Ner.ReferentToken)) 
                    break;
                else 
                {
                    PartToken st = PartToken.TryAttach(tt, null, false, false);
                    if (st != null) 
                    {
                        if (st.EndToken.Next == t0) 
                            return true;
                        break;
                    }
                    if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter) 
                    {
                        if ((++i) > 2) 
                            break;
                    }
                }
            }
            return false;
        }
        public static int _getRank(ItemType t)
        {
            if (t == ItemType.DocPart) 
                return 1;
            if (t == ItemType.Appendix) 
                return 1;
            if (t == ItemType.Section) 
                return 2;
            if (t == ItemType.Subprogram) 
                return 2;
            if (t == ItemType.SubSection) 
                return 3;
            if (t == ItemType.Chapter) 
                return 4;
            if (t == ItemType.Preamble) 
                return 5;
            if (t == ItemType.Paragraph) 
                return 5;
            if (t == ItemType.Subparagraph) 
                return 6;
            if (t == ItemType.Page) 
                return 6;
            if (t == ItemType.Clause) 
                return 7;
            if (t == ItemType.Part) 
                return 8;
            if (t == ItemType.Notice) 
                return 8;
            if (t == ItemType.Item) 
                return 9;
            if (t == ItemType.SubItem) 
                return 10;
            if (t == ItemType.Indention) 
                return 11;
            if (t == ItemType.SubIndention) 
                return 12;
            return 0;
        }
        public static string _getAttrNameByTyp(ItemType Typ)
        {
            if (Typ == ItemType.Chapter) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_CHAPTER;
            if (Typ == ItemType.Appendix) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_APPENDIX;
            if (Typ == ItemType.Clause) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_CLAUSE;
            if (Typ == ItemType.Indention) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_INDENTION;
            if (Typ == ItemType.Item) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_ITEM;
            if (Typ == ItemType.Paragraph) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_PARAGRAPH;
            if (Typ == ItemType.Subparagraph) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBPARAGRAPH;
            if (Typ == ItemType.Part) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_PART;
            if (Typ == ItemType.Section) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_SECTION;
            if (Typ == ItemType.SubSection) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBSECTION;
            if (Typ == ItemType.SubIndention) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBINDENTION;
            if (Typ == ItemType.SubItem) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBITEM;
            if (Typ == ItemType.Preamble) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_PREAMBLE;
            if (Typ == ItemType.Notice) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_NOTICE;
            if (Typ == ItemType.Subprogram) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBPROGRAM;
            if (Typ == ItemType.AddAgree) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_ADDAGREE;
            if (Typ == ItemType.DocPart) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_DOCPART;
            if (Typ == ItemType.Page) 
                return Pullenti.Ner.Decree.DecreePartReferent.ATTR_PAGE;
            return null;
        }
        public static Pullenti.Ner.Instrument.InstrumentKind _getInstrKindByTyp(ItemType Typ)
        {
            if (Typ == ItemType.Chapter) 
                return Pullenti.Ner.Instrument.InstrumentKind.Chapter;
            if (Typ == ItemType.Appendix) 
                return Pullenti.Ner.Instrument.InstrumentKind.Appendix;
            if (Typ == ItemType.Clause) 
                return Pullenti.Ner.Instrument.InstrumentKind.Clause;
            if (Typ == ItemType.Indention) 
                return Pullenti.Ner.Instrument.InstrumentKind.Indention;
            if (Typ == ItemType.Item) 
                return Pullenti.Ner.Instrument.InstrumentKind.Item;
            if (Typ == ItemType.Paragraph) 
                return Pullenti.Ner.Instrument.InstrumentKind.Paragraph;
            if (Typ == ItemType.Subparagraph) 
                return Pullenti.Ner.Instrument.InstrumentKind.Subparagraph;
            if (Typ == ItemType.Part) 
                return Pullenti.Ner.Instrument.InstrumentKind.ClausePart;
            if (Typ == ItemType.Section) 
                return Pullenti.Ner.Instrument.InstrumentKind.Section;
            if (Typ == ItemType.SubSection) 
                return Pullenti.Ner.Instrument.InstrumentKind.Subsection;
            if (Typ == ItemType.SubItem) 
                return Pullenti.Ner.Instrument.InstrumentKind.Subitem;
            if (Typ == ItemType.Preamble) 
                return Pullenti.Ner.Instrument.InstrumentKind.Preamble;
            if (Typ == ItemType.Notice) 
                return Pullenti.Ner.Instrument.InstrumentKind.Notice;
            if (Typ == ItemType.DocPart) 
                return Pullenti.Ner.Instrument.InstrumentKind.DocPart;
            return Pullenti.Ner.Instrument.InstrumentKind.Undefined;
        }
        public static ItemType _getTypeByAttrName(string name)
        {
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_CHAPTER) 
                return ItemType.Chapter;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_APPENDIX) 
                return ItemType.Appendix;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_CLAUSE) 
                return ItemType.Clause;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_INDENTION) 
                return ItemType.Indention;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_ITEM) 
                return ItemType.Item;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_PARAGRAPH) 
                return ItemType.Paragraph;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBPARAGRAPH) 
                return ItemType.Subparagraph;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_PART) 
                return ItemType.Part;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_SECTION) 
                return ItemType.Section;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBSECTION) 
                return ItemType.SubSection;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBINDENTION) 
                return ItemType.SubIndention;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBITEM) 
                return ItemType.SubItem;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_NOTICE) 
                return ItemType.Notice;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_PREAMBLE) 
                return ItemType.Preamble;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBPROGRAM) 
                return ItemType.Subprogram;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_ADDAGREE) 
                return ItemType.AddAgree;
            if (name == Pullenti.Ner.Decree.DecreePartReferent.ATTR_DOCPART) 
                return ItemType.DocPart;
            return ItemType.Prefix;
        }
        public static List<Pullenti.Ner.Decree.DecreePartReferent> TryCreateBetween(Pullenti.Ner.Decree.DecreePartReferent p1, Pullenti.Ner.Decree.DecreePartReferent p2)
        {
            string notEqAttr = null;
            string val1 = null;
            string val2 = null;
            foreach (Pullenti.Ner.Slot s1 in p1.Slots) 
            {
                if (p2.FindSlot(s1.TypeName, s1.Value, true) != null) 
                    continue;
                else 
                {
                    if (notEqAttr != null) 
                        return null;
                    val2 = p2.GetStringValue(s1.TypeName);
                    if (val2 == null) 
                        return null;
                    notEqAttr = s1.TypeName;
                    val1 = s1.Value as string;
                }
            }
            if (val1 == null || val2 == null) 
                return null;
            List<string> diap = Pullenti.Ner.Instrument.Internal.NumberingHelper.CreateDiap(val1, val2);
            if (diap == null || (diap.Count < 3)) 
                return null;
            List<Pullenti.Ner.Decree.DecreePartReferent> res = new List<Pullenti.Ner.Decree.DecreePartReferent>();
            for (int i = 1; i < (diap.Count - 1); i++) 
            {
                Pullenti.Ner.Decree.DecreePartReferent dpr = new Pullenti.Ner.Decree.DecreePartReferent();
                foreach (Pullenti.Ner.Slot s in p1.Slots) 
                {
                    object val = s.Value;
                    if (s.TypeName == notEqAttr) 
                        val = diap[i];
                    dpr.AddSlot(s.TypeName, val, false, 0);
                }
                res.Add(dpr);
            }
            return res;
        }
        public static int GetNumber(string str)
        {
            if (string.IsNullOrEmpty(str)) 
                return 0;
            int i;
            if (int.TryParse(str, out i)) 
                return i;
            if (!char.IsLetter(str[0])) 
                return 0;
            char ch = char.ToUpper(str[0]);
            if (((int)ch) < 0x80) 
            {
                i = (((int)ch) - ((int)'A')) + 1;
                if ((ch == 'Z' && str.Length > 2 && str[1] == '.') && char.IsDigit(str[2])) 
                {
                    int n;
                    if (int.TryParse(str.Substring(2), out n)) 
                        i += n;
                }
            }
            else if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(ch)) 
            {
                i = RuNums.IndexOf(ch);
                if (i < 0) 
                    return 0;
                i++;
                if ((ch == 'Я' && str.Length > 2 && str[1] == '.') && char.IsDigit(str[2])) 
                {
                    int n;
                    if (int.TryParse(str.Substring(2), out n)) 
                        i += n;
                }
            }
            if (i < 0) 
                return 0;
            return i;
        }
        static string RuNums = "АБВГДЕЖЗИКЛМНОПРСТУФХЦЧШЩЭЮЯ";
    }
}