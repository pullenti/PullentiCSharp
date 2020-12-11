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

namespace Pullenti.Ner.Goods.Internal
{
    public class GoodAttrToken : Pullenti.Ner.MetaToken
    {
        public GoodAttrToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public Pullenti.Ner.Goods.GoodAttrType Typ;
        public string Value;
        public string AltValue;
        public string Name;
        public Pullenti.Ner.Referent Ref;
        public Pullenti.Ner.ReferentToken RefTok;
        public override string ToString()
        {
            return string.Format("{0}: {1}{2} {3}", Typ, Value, (AltValue == null ? "" : string.Format(" / {0}", AltValue)), (Ref == null ? "" : Ref.ToString()));
        }
        internal Pullenti.Ner.Goods.GoodAttributeReferent _createAttr()
        {
            if (Ref is Pullenti.Ner.Goods.GoodAttributeReferent) 
                return Ref as Pullenti.Ner.Goods.GoodAttributeReferent;
            Pullenti.Ner.Goods.GoodAttributeReferent ar = new Pullenti.Ner.Goods.GoodAttributeReferent();
            if (Typ != Pullenti.Ner.Goods.GoodAttrType.Undefined) 
                ar.Typ = Typ;
            if (Name != null) 
                ar.AddSlot(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_NAME, Name, false, 0);
            if (Ref != null) 
                ar.AddSlot(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_REF, Ref, true, 0);
            else if (RefTok != null) 
            {
                ar.AddSlot(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_REF, RefTok.Referent, true, 0);
                ar.AddExtReferent(RefTok);
            }
            if (Typ == Pullenti.Ner.Goods.GoodAttrType.Numeric) 
            {
            }
            List<string> vals = null;
                {
                    vals = new List<string>();
                    if (Value != null) 
                        vals.Add(Value);
                    if (AltValue != null) 
                        vals.Add(AltValue);
                }
            foreach (string v in vals) 
            {
                string v1 = v;
                if (ar.Typ == Pullenti.Ner.Goods.GoodAttrType.Proper) 
                {
                    v1 = v.ToUpper();
                    if (v1.IndexOf('\'') >= 0) 
                        v1 = v1.Replace("'", "");
                }
                if (string.IsNullOrEmpty(v1)) 
                    continue;
                ar.AddSlot((v == Value ? Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_VALUE : Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_ALTVALUE), v1, false, 0);
                if ((v1.Length < 10) && Pullenti.Morph.LanguageHelper.IsLatinChar(v1[0]) && ar.Typ == Pullenti.Ner.Goods.GoodAttrType.Proper) 
                {
                    List<string> rus = Pullenti.Ner.Core.Internal.RusLatAccord.GetVariants(v1);
                    if (rus == null || rus.Count == 0) 
                        continue;
                    foreach (string vv in rus) 
                    {
                        if (ar.FindSlot(null, vv, true) == null) 
                        {
                            ar.AddSlot(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_ALTVALUE, vv, false, 0);
                            if (ar.Slots.Count > 20) 
                                break;
                        }
                    }
                }
            }
            if (ar.FindSlot(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_VALUE, null, true) == null && ar.FindSlot(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_REF, null, true) == null) 
                return null;
            return ar;
        }
        public static List<Pullenti.Ner.ReferentToken> TryParseList(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            List<GoodAttrToken> li = _tryParseList(t);
            if (li == null || li.Count == 0) 
                return null;
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            foreach (GoodAttrToken a in li) 
            {
                Pullenti.Ner.Goods.GoodAttributeReferent attr = a._createAttr();
                if (attr != null) 
                    res.Add(new Pullenti.Ner.ReferentToken(attr, a.BeginToken, a.EndToken));
            }
            return res;
        }
        static List<GoodAttrToken> _tryParseList(Pullenti.Ner.Token t)
        {
            List<GoodAttrToken> res = new List<GoodAttrToken>();
            GoodAttrToken key = null;
            bool nextSeq = false;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (tt != t && tt.IsNewlineBefore) 
                    break;
                if (tt != t && Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt) && !tt.IsChar('(')) 
                {
                    nextSeq = true;
                    if (key == null) 
                        break;
                    GoodAttrToken re2 = GoodAttrToken.TryParse(tt, key, t != tt, false);
                    if (re2 != null && ((re2.Typ == Pullenti.Ner.Goods.GoodAttrType.Numeric || re2.Typ == Pullenti.Ner.Goods.GoodAttrType.Model))) 
                    {
                    }
                    else if (re2 != null && ((re2.RefTok != null || re2.Ref != null))) 
                        nextSeq = false;
                    else if ((tt.GetMorphClassInDictionary().IsVerb && re2 != null && re2.Typ == Pullenti.Ner.Goods.GoodAttrType.Character) && _isSpecVerb(tt)) 
                    {
                    }
                    else 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt == null) 
                            break;
                        string noun = npt.Noun.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                        if (key.Value == null) 
                        {
                            if (key.Ref == null) 
                                break;
                            if (key.Ref.ToString().ToUpper().Contains(noun)) 
                            {
                            }
                            else 
                                break;
                        }
                        else if (noun.Contains(key.Value) || key.Value.Contains(noun)) 
                        {
                        }
                        else 
                            break;
                    }
                }
                if ((tt is Pullenti.Ner.TextToken) && nextSeq) 
                {
                    Pullenti.Morph.MorphClass dc = tt.GetMorphClassInDictionary();
                    if (dc == Pullenti.Morph.MorphClass.Verb) 
                    {
                        if (!_isSpecVerb(tt)) 
                            break;
                    }
                }
                if (tt.IsValue("ДОЛЖЕН", null) || tt.IsValue("ДОЛЖНА", null) || tt.IsValue("ДОЛЖНО", null)) 
                {
                    if (tt.Next != null && tt.Next.GetMorphClassInDictionary().IsVerb) 
                        tt = tt.Next;
                    continue;
                }
                GoodAttrToken re = GoodAttrToken.TryParse(tt, key, tt != t, false);
                if (re != null) 
                {
                    if (key == null) 
                    {
                        if (re.Typ == Pullenti.Ner.Goods.GoodAttrType.Keyword) 
                            key = re;
                        else if (re.Typ == Pullenti.Ner.Goods.GoodAttrType.Numeric || re.Typ == Pullenti.Ner.Goods.GoodAttrType.Model) 
                            return null;
                    }
                    res.Add(re);
                    tt = re.EndToken;
                    continue;
                }
                if ((tt is Pullenti.Ner.TextToken) && !tt.Chars.IsLetter) 
                    continue;
                if (tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction) 
                    continue;
                if (tt is Pullenti.Ner.NumberToken) 
                    res.Add(new GoodAttrToken(tt, tt) { Value = tt.GetSourceText() });
            }
            if (res.Count > 0 && res[res.Count - 1].Typ == Pullenti.Ner.Goods.GoodAttrType.Character) 
            {
                if (res[res.Count - 1].EndToken == res[res.Count - 1].BeginToken && res[res.Count - 1].EndToken.GetMorphClassInDictionary().IsAdverb) 
                    res.RemoveAt(res.Count - 1);
            }
            return res;
        }
        static bool _isSpecVerb(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            if ((t.IsValue("ПРИМЕНЯТЬ", null) || t.IsValue("ИСПОЛЬЗОВАТЬ", null) || t.IsValue("ИЗГОТАВЛИВАТЬ", null)) || t.IsValue("ПРИМЕНЯТЬ", null) || t.IsValue("ИЗГОТОВИТЬ", null)) 
                return true;
            return false;
        }
        public static GoodAttrToken TryParse(Pullenti.Ner.Token t, GoodAttrToken key, bool canBeMeasure, bool isChars = false)
        {
            GoodAttrToken res = _tryParse_(t, key, canBeMeasure, isChars);
            if (res == null || res.Value == null) 
                return res;
            if ((res != null && res.Typ == Pullenti.Ner.Goods.GoodAttrType.Character && ((res.EndToken == res.BeginToken || (res.Value.IndexOf(' ') < 0)))) && res.AltValue == null) 
            {
                if (res.Value == "ДЛЯ") 
                    return TryParse(t.Next, key, false, false);
                if (res.Value != null) 
                {
                    if (res.Value.StartsWith("ДВУ", StringComparison.OrdinalIgnoreCase) && !res.Value.StartsWith("ДВУХ", StringComparison.OrdinalIgnoreCase)) 
                        res.Value = "ДВУХ" + res.Value.Substring(3);
                }
            }
            if ((res != null && res.Typ == Pullenti.Ner.Goods.GoodAttrType.Character && res.BeginToken.Morph.Class.IsPreposition) && res.EndToken != res.BeginToken && res.AltValue == null) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.BeginToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.EndToken == res.EndToken) 
                    res.AltValue = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
            }
            return res;
        }
        static GoodAttrToken _tryParse_(Pullenti.Ner.Token t, GoodAttrToken key, bool canBeMeasure, bool isChars)
        {
            if (t == null) 
                return null;
            if (t.IsValue("ПРЕДНАЗНАЧЕН", null)) 
            {
            }
            GoodAttrToken res;
            Pullenti.Ner.Referent r = t.GetReferent();
            if (r != null) 
            {
                if (r.TypeName == "ORGANIZATION" || r.TypeName == "GEO") 
                    return new GoodAttrToken(t, t) { Typ = Pullenti.Ner.Goods.GoodAttrType.Referent, Ref = r };
            }
            if (canBeMeasure) 
            {
                if ((((res = _tryParseNum(t)))) != null) 
                    return res;
            }
            if (isChars) 
            {
                if ((((res = _tryParseChars(t)))) != null) 
                    return res;
            }
            Pullenti.Ner.Measure.Internal.MeasureToken ms = Pullenti.Ner.Measure.Internal.MeasureToken.TryParse(t, null, true, false, false, false);
            if (ms != null && ms.Nums != null) 
            {
                GoodAttrToken nres = new GoodAttrToken(t, ms.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Numeric };
                nres.Name = ms.Name;
                nres.Value = ms.GetNormValues();
                return nres;
            }
            if (t.Kit.Ontology != null) 
            {
                List<Pullenti.Ner.Core.IntOntologyToken> li = t.Kit.Ontology.AttachToken(Pullenti.Ner.Goods.GoodAttributeReferent.OBJ_TYPENAME, t);
                if (li != null && li[0].Item != null && (li[0].Item.Referent is Pullenti.Ner.Goods.GoodAttributeReferent)) 
                {
                    res = new GoodAttrToken(li[0].BeginToken, li[0].EndToken);
                    res.Typ = (li[0].Item.Referent as Pullenti.Ner.Goods.GoodAttributeReferent).Typ;
                    res.Ref = li[0].Item.Referent.Clone();
                    return res;
                }
            }
            Pullenti.Ner.Core.TerminToken tok;
            if ((((tok = m_StdAbbrs.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No)))) != null) 
            {
                Pullenti.Ner.Goods.GoodAttrType ty = (Pullenti.Ner.Goods.GoodAttrType)tok.Termin.Tag;
                if (ty == Pullenti.Ner.Goods.GoodAttrType.Undefined && tok.Termin.Tag2 != null) 
                {
                    Pullenti.Ner.Token tt2 = tok.EndToken.Next;
                    if (tt2 != null && ((tt2.IsChar(':') || tt2.IsHiphen))) 
                        tt2 = tt2.Next;
                    res = _tryParse_(tt2, key, false, isChars);
                    if (res != null && ((res.Typ == Pullenti.Ner.Goods.GoodAttrType.Proper || res.Typ == Pullenti.Ner.Goods.GoodAttrType.Model))) 
                    {
                        res.BeginToken = t;
                        res.Name = tok.Termin.CanonicText;
                        return res;
                    }
                    Pullenti.Ner.Core.TerminToken tok2 = m_StdAbbrs.TryParse(tt2, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok2 != null && (tok2.Termin.Tag2 as string) == "NO") 
                    {
                        res = new GoodAttrToken(t, tok2.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Undefined };
                        return res;
                    }
                    res = _tryParseModel(tt2);
                    if (res != null) 
                    {
                        res.BeginToken = t;
                        res.Name = tok.Termin.CanonicText;
                        return res;
                    }
                }
                if (ty != Pullenti.Ner.Goods.GoodAttrType.Referent) 
                {
                    res = new GoodAttrToken(t, tok.EndToken) { Typ = ty, Value = tok.Termin.CanonicText, Morph = tok.Morph };
                    if (res.EndToken.Next != null && res.EndToken.Next.IsChar('.')) 
                        res.EndToken = res.EndToken.Next;
                    return res;
                }
                if (ty == Pullenti.Ner.Goods.GoodAttrType.Referent) 
                {
                    Pullenti.Ner.Token tt = tok.EndToken.Next;
                    for (; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsNewlineBefore) 
                            break;
                        if (tt.IsHiphen || tt.IsCharOf(":")) 
                            continue;
                        if (tt.GetMorphClassInDictionary().IsAdverb) 
                            continue;
                        Pullenti.Ner.Core.TerminToken tok2 = m_StdAbbrs.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok2 != null) 
                        {
                            Pullenti.Ner.Goods.GoodAttrType ty2 = (Pullenti.Ner.Goods.GoodAttrType)tok2.Termin.Tag;
                            if (ty2 == Pullenti.Ner.Goods.GoodAttrType.Referent || ty2 == Pullenti.Ner.Goods.GoodAttrType.Undefined) 
                            {
                                tt = tok2.EndToken;
                                continue;
                            }
                        }
                        break;
                    }
                    if (tt == null) 
                        return null;
                    if (tt.GetReferent() != null) 
                        return new GoodAttrToken(t, tt) { Ref = tt.GetReferent(), Typ = Pullenti.Ner.Goods.GoodAttrType.Referent };
                    if ((tt is Pullenti.Ner.TextToken) && !tt.Chars.IsAllLower && tt.Chars.IsLetter) 
                    {
                        Pullenti.Ner.ReferentToken rt = tt.Kit.ProcessReferent("ORGANIZATION", tt);
                        if (rt != null) 
                            return new GoodAttrToken(t, rt.EndToken) { RefTok = rt, Typ = Pullenti.Ner.Goods.GoodAttrType.Referent };
                    }
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                    {
                        Pullenti.Ner.ReferentToken rt = tt.Kit.ProcessReferent("ORGANIZATION", tt.Next);
                        if (rt != null) 
                        {
                            Pullenti.Ner.Token t1 = rt.EndToken;
                            if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next, false, null, false)) 
                                t1 = t1.Next;
                            return new GoodAttrToken(t, t1) { RefTok = rt, Typ = Pullenti.Ner.Goods.GoodAttrType.Referent };
                        }
                    }
                }
            }
            if (t.IsValue("КАТАЛОЖНЫЙ", null)) 
            {
                Pullenti.Ner.Token tt = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t.Next);
                if (tt != null) 
                {
                    if (tt.IsCharOf(":") || tt.IsHiphen) 
                        tt = tt.Next;
                    res = _tryParseModel(tt);
                    if (res != null) 
                    {
                        res.BeginToken = t;
                        res.Name = "КАТАЛОЖНЫЙ НОМЕР";
                        return res;
                    }
                }
            }
            if (t.IsValue("ФАСОВКА", null) || t.IsValue("УПАКОВКА", null)) 
            {
                if (!(t.Previous is Pullenti.Ner.NumberToken)) 
                {
                    Pullenti.Ner.Token tt = t.Next;
                    if (tt != null) 
                    {
                        if (tt.IsCharOf(":") || tt.IsHiphen) 
                            tt = tt.Next;
                    }
                    if (tt == null) 
                        return null;
                    res = new GoodAttrToken(t, tt) { Typ = Pullenti.Ner.Goods.GoodAttrType.Numeric, Name = "ФАСОВКА" };
                    Pullenti.Ner.Token et = null;
                    for (; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsComma) 
                            break;
                        if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                            break;
                        if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter && !tt.Chars.IsAllLower) 
                            break;
                        et = tt;
                    }
                    if (et != null) 
                    {
                        res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(res.EndToken, et, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        res.EndToken = et;
                    }
                    return res;
                }
            }
            if ((t is Pullenti.Ner.ReferentToken) && (((t.GetReferent() is Pullenti.Ner.Uri.UriReferent) || t.GetReferent().TypeName == "DECREE"))) 
            {
                res = new GoodAttrToken(t, t) { Typ = Pullenti.Ner.Goods.GoodAttrType.Model, Name = "СПЕЦИФИКАЦИЯ" };
                res.Value = t.GetReferent().ToString();
                return res;
            }
            if (key == null && !isChars) 
            {
                bool isAllUpper = true;
                for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
                {
                    if (tt != t && tt.IsNewlineBefore) 
                        break;
                    if (tt.Chars.IsCyrillicLetter && !tt.Chars.IsAllUpper) 
                    {
                        isAllUpper = false;
                        break;
                    }
                }
                if ((((!t.Chars.IsAllUpper || isAllUpper)) && ((t.Morph.Class.IsNoun || t.Morph.Class.IsUndefined)) && t.Chars.IsCyrillicLetter) && (t is Pullenti.Ner.TextToken)) 
                {
                    if (t.IsValue("СООТВЕТСТВИЕ", null)) 
                    {
                        Pullenti.Ner.Token tt1 = t.Next;
                        if (tt1 != null && ((tt1.IsChar(':') || tt1.IsHiphen))) 
                            tt1 = tt1.Next;
                        res = _tryParse_(tt1, key, false, isChars);
                        if (res != null) 
                            res.BeginToken = t;
                        return res;
                    }
                    bool ok = true;
                    if (t.Morph.Class.IsAdjective || t.Morph.Class.IsVerb) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParseVerbs, 0, null);
                        if (npt1 != null && npt1.EndToken != t && npt1.Adjectives.Count > 0) 
                            ok = false;
                    }
                    if (ok) 
                    {
                        res = new GoodAttrToken(t, t) { Typ = Pullenti.Ner.Goods.GoodAttrType.Keyword, Morph = t.Morph };
                        res.Value = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                        if ((t.Next != null && t.Next.IsHiphen && (t.Next.Next is Pullenti.Ner.TextToken)) && ((t.Next.Next.Chars.IsAllLower || t.Next.Next.Chars == t.Chars))) 
                        {
                            if (!t.IsWhitespaceAfter && !t.Next.IsWhitespaceAfter) 
                            {
                                res.EndToken = (t = t.Next.Next);
                                res.Value = string.Format("{0}-{1}", res.Value, (t as Pullenti.Ner.TextToken).Term);
                            }
                        }
                        return res;
                    }
                }
            }
            if ((t.IsWhitespaceBefore && (t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) && (t.LengthChar < 5) && !isChars) 
            {
                Pullenti.Ner.ReferentToken rt = m_DenomAn.TryAttach(t, false);
                if ((rt == null && t.WhitespacesAfterCount == 1 && (t.Next is Pullenti.Ner.NumberToken)) && (t.LengthChar < 3) && _tryParseNum(t.Next) == null) 
                    rt = m_DenomAn.TryAttach(t, true);
                if (rt != null) 
                {
                    res = new GoodAttrToken(t, rt.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Model };
                    Pullenti.Ner.Denomination.DenominationReferent dr = rt.Referent as Pullenti.Ner.Denomination.DenominationReferent;
                    foreach (Pullenti.Ner.Slot s in dr.Slots) 
                    {
                        if (s.TypeName == Pullenti.Ner.Denomination.DenominationReferent.ATTR_VALUE) 
                        {
                            if (res.Value == null) 
                                res.Value = s.Value as string;
                            else 
                                res.AltValue = s.Value as string;
                        }
                    }
                    return res;
                }
                if (!t.IsWhitespaceAfter && (t.Next is Pullenti.Ner.NumberToken) && _tryParseNum(t.Next) == null) 
                {
                    res = _tryParseModel(t);
                    return res;
                }
            }
            if (t.Chars.IsLatinLetter && t.IsWhitespaceBefore) 
            {
                res = new GoodAttrToken(t, t) { Typ = Pullenti.Ner.Goods.GoodAttrType.Proper };
                for (Pullenti.Ner.Token ttt = t.Next; ttt != null; ttt = ttt.Next) 
                {
                    if (ttt.Chars.IsLatinLetter && ttt.Chars == t.Chars) 
                        res.EndToken = ttt;
                    else if (((ttt is Pullenti.Ner.TextToken) && !ttt.IsLetters && ttt.Next != null) && ttt.Next.Chars.IsLatinLetter) 
                    {
                    }
                    else 
                        break;
                }
                if (res.EndToken.IsWhitespaceAfter) 
                {
                    res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(res, Pullenti.Ner.Core.GetTextAttr.No);
                    if (res.Value.IndexOf(' ') > 0) 
                        res.AltValue = res.Value.Replace(" ", "");
                    if (res.LengthChar < 2) 
                        return null;
                    return res;
                }
            }
            string pref = null;
            Pullenti.Ner.Token t0 = t;
            if (t.Morph.Class.IsPreposition && t.Next != null && t.Next.Chars.IsLetter) 
            {
                pref = (t as Pullenti.Ner.TextToken).GetNormalCaseText(Pullenti.Morph.MorphClass.Preposition, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                t = t.Next;
                if ((t.IsCommaAnd && (t.Next is Pullenti.Ner.TextToken) && t.Next.Morph.Class.IsPreposition) && t.Next.Next != null) 
                {
                    pref = string.Format("{0} И {1}", pref, (t.Next as Pullenti.Ner.TextToken).GetNormalCaseText(Pullenti.Morph.MorphClass.Preposition, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                    t = t.Next.Next;
                }
            }
            else if ((((((t.IsValue("Д", null) || t.IsValue("Б", null) || t.IsValue("Н", null)) || t.IsValue("H", null))) && t.Next != null && t.Next.IsCharOf("\\/")) && !t.IsWhitespaceAfter && !t.Next.IsWhitespaceAfter) && (t.Next.Next is Pullenti.Ner.TextToken)) 
            {
                pref = (t.IsValue("Д", null) ? "ДЛЯ" : (t.IsValue("Б", null) ? "БЕЗ" : "НЕ"));
                t = t.Next.Next;
                if (pref == "НЕ") 
                {
                    GoodAttrToken re = _tryParse_(t, key, false, isChars);
                    if (re != null && re.Typ == Pullenti.Ner.Goods.GoodAttrType.Character && re.Value != null) 
                    {
                        re.BeginToken = t0;
                        re.Value = "НЕ" + re.Value;
                        if (re.AltValue != null) 
                            re.AltValue = "НЕ" + re.AltValue;
                        return re;
                    }
                }
            }
            if (pref != null) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt == null && t.GetMorphClassInDictionary().IsAdverb) 
                    npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && ((npt.Chars.IsAllLower || npt.Chars.IsAllUpper)) && npt.Chars.IsCyrillicLetter) 
                {
                    GoodAttrToken re = new GoodAttrToken(t0, npt.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character };
                    Pullenti.Morph.MorphCase cas = new Pullenti.Morph.MorphCase();
                    for (Pullenti.Ner.Token tt = npt.EndToken.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsNewlineBefore || tt.IsChar(';')) 
                            break;
                        if (tt.IsCommaAnd && tt.Next != null) 
                            tt = tt.Next;
                        Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                        if (npt1 == null && tt.GetMorphClassInDictionary().IsAdverb) 
                            npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt1 == null) 
                            break;
                        if (npt1.Chars != npt.Chars) 
                            break;
                        if (tt.Previous.IsComma) 
                        {
                            if (!cas.IsUndefined && ((cas & npt1.Morph.Case)).IsUndefined) 
                                break;
                            GoodAttrToken re2 = _tryParseNum(tt);
                            if (re2 != null && re2.Typ == Pullenti.Ner.Goods.GoodAttrType.Numeric) 
                                break;
                        }
                        tt = (re.EndToken = npt1.EndToken);
                        cas = npt1.Morph.Case;
                    }
                    re.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(npt.BeginToken, re.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    if (npt.EndToken == re.EndToken && npt.Adjectives.Count == 0) 
                    {
                        if (pref == "ДЛЯ" || pref == "ИЗ") 
                        {
                            string noun = npt.Noun.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                            List<Pullenti.Semantic.Utils.DerivateGroup> grs = Pullenti.Semantic.Utils.DerivateService.FindDerivates(noun, true, null);
                            if (grs != null) 
                            {
                                foreach (Pullenti.Semantic.Utils.DerivateGroup g in grs) 
                                {
                                    if (re.AltValue != null) 
                                        break;
                                    foreach (Pullenti.Semantic.Utils.DerivateWord v in g.Words) 
                                    {
                                        if (v.Class.IsAdjective) 
                                        {
                                            re.AltValue = v.Spelling;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (pref != null) 
                        re.Value = string.Format("{0} {1}", pref, re.Value);
                    return re;
                }
            }
            if (t.Chars.IsCyrillicLetter || (t is Pullenti.Ner.NumberToken)) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.AdjectiveCanBeLast | Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective, 0, null);
                if (npt1 != null) 
                {
                    if (((npt1.Noun.BeginToken.IsValue("СОРТ", null) || npt1.Noun.BeginToken.IsValue("КЛАСС", null) || npt1.Noun.BeginToken.IsValue("ГРУППА", null)) || npt1.Noun.BeginToken.IsValue("КАТЕГОРИЯ", null) || npt1.Noun.BeginToken.IsValue("ТИП", null)) || npt1.Noun.BeginToken.IsValue("ПОДТИП", null)) 
                    {
                        res = new GoodAttrToken(t, npt1.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character };
                        res.Value = npt1.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                        if (res.BeginToken == res.EndToken) 
                        {
                            if (t.Next != null && t.Next.IsValue("ВЫСШ", null)) 
                            {
                                res.Value = ((((npt1.Noun.BeginToken.Morph.Gender & Pullenti.Morph.MorphGender.Feminie)) != Pullenti.Morph.MorphGender.Undefined ? "ВЫСШАЯ" : "ВЫСШИЙ ")) + res.Value;
                                res.EndToken = t.Next;
                                if (res.EndToken.Next != null && res.EndToken.Next.IsChar('.')) 
                                    res.EndToken = res.EndToken.Next;
                            }
                            else if (t.WhitespacesAfterCount < 2) 
                            {
                                if ((t.Next is Pullenti.Ner.NumberToken) && (t.Next as Pullenti.Ner.NumberToken).IntValue != null) 
                                {
                                    res.Value = string.Format("{0} {1}", Pullenti.Ner.Core.NumberHelper.GetNumberAdjective((t.Next as Pullenti.Ner.NumberToken).IntValue.Value, (((npt1.Morph.Gender & Pullenti.Morph.MorphGender.Feminie)) != Pullenti.Morph.MorphGender.Undefined ? Pullenti.Morph.MorphGender.Feminie : Pullenti.Morph.MorphGender.Masculine), Pullenti.Morph.MorphNumber.Singular), t.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false));
                                    res.EndToken = t.Next;
                                }
                                else 
                                {
                                    Pullenti.Ner.NumberToken rom = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t.Next);
                                    if (rom != null && rom.IntValue != null) 
                                    {
                                        res.Value = string.Format("{0} {1}", Pullenti.Ner.Core.NumberHelper.GetNumberAdjective(rom.IntValue.Value, (((npt1.Morph.Gender & Pullenti.Morph.MorphGender.Feminie)) != Pullenti.Morph.MorphGender.Undefined ? Pullenti.Morph.MorphGender.Feminie : Pullenti.Morph.MorphGender.Masculine), Pullenti.Morph.MorphNumber.Singular), t.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false));
                                        res.EndToken = rom.EndToken;
                                    }
                                }
                            }
                        }
                        if (res.BeginToken != res.EndToken) 
                            return res;
                    }
                }
                if (((t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).IntValue != null && (t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) && (t.Next is Pullenti.Ner.TextToken) && (t.WhitespacesAfterCount < 2)) 
                {
                    if (((t.Next.IsValue("СОРТ", null) || t.Next.IsValue("КЛАСС", null) || t.Next.IsValue("ГРУППА", null)) || t.Next.IsValue("КАТЕГОРИЯ", null) || t.Next.IsValue("ТИП", null)) || t.Next.IsValue("ПОДТИП", null)) 
                    {
                        res = new GoodAttrToken(t, t.Next) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character };
                        res.Value = string.Format("{0} {1}", Pullenti.Ner.Core.NumberHelper.GetNumberAdjective((t as Pullenti.Ner.NumberToken).IntValue.Value, (((t.Next.Morph.Gender & Pullenti.Morph.MorphGender.Feminie)) != Pullenti.Morph.MorphGender.Undefined ? Pullenti.Morph.MorphGender.Feminie : Pullenti.Morph.MorphGender.Masculine), Pullenti.Morph.MorphNumber.Singular), t.Next.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false));
                        return res;
                    }
                }
                if (npt1 != null && npt1.Noun.BeginToken.IsValue("ХАРАКТЕРИСТИКА", null)) 
                {
                    Pullenti.Ner.Token t11 = npt1.EndToken.Next;
                    if (t11 != null && ((t11.IsValue("УКАЗАТЬ", null) || t11.IsValue("УКАЗЫВАТЬ", null)))) 
                    {
                        res = new GoodAttrToken(t, t11) { Typ = Pullenti.Ner.Goods.GoodAttrType.Undefined };
                        Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t11.Next, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                        if (npt2 != null) 
                            res.EndToken = npt2.EndToken;
                        else if (t11.Next != null && t11.Next.IsValue("В", null)) 
                        {
                            res.EndToken = t11.Next;
                            if (res.EndToken.Next != null) 
                                res.EndToken = res.EndToken.Next;
                        }
                        return res;
                    }
                }
            }
            if ((t.Chars.IsCyrillicLetter && pref == null && (t is Pullenti.Ner.TextToken)) && t.Morph.Class.IsAdjective) 
            {
                if (t.Morph.ContainsAttr("к.ф.", null) && t.Next != null && t.Next.IsHiphen) 
                {
                    string val = (t as Pullenti.Ner.TextToken).Term;
                    Pullenti.Ner.Token tt;
                    for (tt = t.Next.Next; tt != null; ) 
                    {
                        if (((tt is Pullenti.Ner.TextToken) && tt.Next != null && tt.Next.IsHiphen) && (tt.Next.Next is Pullenti.Ner.TextToken)) 
                        {
                            val = string.Format("{0}-{1}", val, (tt as Pullenti.Ner.TextToken).Term);
                            tt = tt.Next.Next;
                            continue;
                        }
                        GoodAttrToken re = _tryParse_(tt, key, false, isChars);
                        if (re != null && re.Typ == Pullenti.Ner.Goods.GoodAttrType.Character) 
                        {
                            re.BeginToken = t;
                            re.Value = string.Format("{0}-{1}", val, re.Value);
                            return re;
                        }
                        break;
                    }
                }
                bool isChar = false;
                if (key != null && t.Morph.CheckAccord(key.Morph, false, false) && ((t.Chars.IsAllLower || Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)))) 
                    isChar = true;
                else if (t.GetMorphClassInDictionary().IsAdjective && !t.Morph.ContainsAttr("неизм.", null)) 
                    isChar = true;
                if (isChar && t.Morph.Class.IsVerb) 
                {
                    if ((t.IsValue("ПРЕДНАЗНАЧИТЬ", null) || t.IsValue("ПРЕДНАЗНАЧАТЬ", null) || t.IsValue("ИЗГОТОВИТЬ", null)) || t.IsValue("ИЗГОТОВЛЯТЬ", null)) 
                        isChar = false;
                }
                if (isChar) 
                {
                    res = new GoodAttrToken(t, t) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character };
                    res.Value = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Masculine, false);
                    return res;
                }
            }
            if ((t.Chars.IsCyrillicLetter && pref == null && (t is Pullenti.Ner.TextToken)) && t.Morph.Class.IsVerb) 
            {
                GoodAttrToken re = _tryParse_(t.Next, key, false, isChars);
                if (re != null && re.Typ == Pullenti.Ner.Goods.GoodAttrType.Character) 
                {
                    re.BeginToken = t;
                    re.AltValue = string.Format("{0} {1}", (t as Pullenti.Ner.TextToken).Term, re.Value);
                    return re;
                }
            }
            if (t.Chars.IsCyrillicLetter) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParseVerbs, 0, null);
                if ((npt != null && npt.Adjectives.Count > 0 && npt.Adjectives[0].Chars.IsAllLower) && !npt.Noun.Chars.IsAllLower) 
                    npt = null;
                if (pref == null && npt != null && npt.Noun.EndToken.GetMorphClassInDictionary().IsAdjective) 
                    npt = null;
                if (npt != null && !npt.EndToken.Chars.IsCyrillicLetter) 
                    npt = null;
                if (npt != null) 
                {
                    bool isProp = false;
                    if (pref != null) 
                        isProp = true;
                    else if (npt.Chars.IsAllLower) 
                        isProp = true;
                    if (npt.Adjectives.Count > 0 && pref == null) 
                    {
                        if (key == null) 
                            return new GoodAttrToken(t0, npt.Adjectives[0].EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Value = npt.Adjectives[0].GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Masculine, false) };
                    }
                    if (pref == null && key != null && npt.Noun.IsValue(key.Value, null)) 
                    {
                        if (npt.Adjectives.Count == 0) 
                            return new GoodAttrToken(t0, npt.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Keyword, Value = npt.Noun.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false), Morph = npt.Morph };
                        return new GoodAttrToken(t0, npt.Adjectives[0].EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Value = npt.Adjectives[0].GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Masculine, false) };
                    }
                    if (isProp) 
                    {
                        res = new GoodAttrToken(t0, npt.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character };
                        res.Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                        return res;
                    }
                    if (!npt.Chars.IsAllLower) 
                        return new GoodAttrToken(t0, npt.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Proper, Value = npt.GetSourceText(), Morph = npt.Morph };
                }
                if (t is Pullenti.Ner.TextToken) 
                {
                    if (((t.GetMorphClassInDictionary().IsAdjective || t.Morph.Class == Pullenti.Morph.MorphClass.Adjective)) && pref == null) 
                        return new GoodAttrToken(t0, t) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Value = (t as Pullenti.Ner.TextToken).Lemma, Morph = t.Morph };
                }
                if ((t is Pullenti.Ner.NumberToken) && pref != null) 
                {
                    GoodAttrToken num = _tryParseNum(t);
                    if (num != null) 
                    {
                        num.BeginToken = t0;
                        return num;
                    }
                }
                if (pref != null && t.Morph.Class.IsAdjective && (t is Pullenti.Ner.TextToken)) 
                {
                    res = new GoodAttrToken(t0, t) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character };
                    res.Value = (t as Pullenti.Ner.TextToken).GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Masculine, false);
                    return res;
                }
                if (pref != null && t.Next != null && t.Next.IsValue("WC", null)) 
                    return new GoodAttrToken(t, t.Next) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Value = "туалет" };
                if (pref != null) 
                    return null;
            }
            if (t != null && t.IsValue("№", null) && (t.Next is Pullenti.Ner.NumberToken)) 
                return new GoodAttrToken(t, t.Next) { Typ = Pullenti.Ner.Goods.GoodAttrType.Model, Value = string.Format("№{0}", (t.Next as Pullenti.Ner.NumberToken).Value) };
            if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter) 
            {
                if (t.LengthChar > 2 && ((!t.Chars.IsAllLower || t.Chars.IsLatinLetter))) 
                    return new GoodAttrToken(t, t) { Typ = Pullenti.Ner.Goods.GoodAttrType.Proper, Value = (t as Pullenti.Ner.TextToken).Term };
                return null;
            }
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    GoodAttrToken res1 = _tryParse_(t.Next, key, false, isChars);
                    if (res1 != null && res1.EndToken.Next == br.EndToken) 
                    {
                        if (res1.Typ == Pullenti.Ner.Goods.GoodAttrType.Character) 
                            res1.Typ = Pullenti.Ner.Goods.GoodAttrType.Proper;
                        res1.BeginToken = t;
                        res1.EndToken = br.EndToken;
                    }
                    else 
                    {
                        res1 = new GoodAttrToken(br.BeginToken, br.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Proper };
                        res1.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.No);
                    }
                    return res1;
                }
            }
            if (t.IsChar('(')) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    if (t.Next.IsValue("ПРИЛОЖЕНИЕ", null)) 
                        return new GoodAttrToken(t, br.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Undefined };
                }
            }
            GoodAttrToken nnn = _tryParseNum2(t);
            if (nnn != null) 
                return nnn;
            return null;
        }
        static GoodAttrToken _tryParseModel(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            GoodAttrToken res = new GoodAttrToken(t, t) { Typ = Pullenti.Ner.Goods.GoodAttrType.Model };
            StringBuilder tmp = new StringBuilder();
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (tt.IsWhitespaceBefore && tt != t) 
                    break;
                if (tt is Pullenti.Ner.NumberToken) 
                {
                    if (tmp.Length > 0 && char.IsDigit(tmp[tmp.Length - 1])) 
                        tmp.Append('-');
                    tmp.Append((tt as Pullenti.Ner.NumberToken).GetSourceText());
                    res.EndToken = tt;
                    continue;
                }
                if (tt is Pullenti.Ner.ReferentToken) 
                {
                    Pullenti.Ner.Denomination.DenominationReferent den = tt.GetReferent() as Pullenti.Ner.Denomination.DenominationReferent;
                    if (den != null) 
                    {
                        tmp.Append(den.Value);
                        continue;
                    }
                }
                if (!(tt is Pullenti.Ner.TextToken)) 
                    break;
                if (!tt.Chars.IsLetter) 
                {
                    if (tt.IsCharOf("\\/-:")) 
                    {
                        if (tt.IsCharOf(":") && tt.IsWhitespaceAfter) 
                            break;
                        tmp.Append('-');
                    }
                    else if (tt.IsChar('.')) 
                    {
                        if (tt.IsWhitespaceAfter) 
                            break;
                        tmp.Append('.');
                    }
                    else 
                        break;
                }
                else 
                    tmp.Append((tt as Pullenti.Ner.TextToken).Term);
                res.EndToken = tt;
            }
            res.Value = tmp.ToString();
            return res;
        }
        static GoodAttrToken _tryParseNum(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Measure.Internal.MeasureToken mt = Pullenti.Ner.Measure.Internal.MeasureToken.TryParse(t, null, true, false, false, false);
            if (mt == null) 
                mt = Pullenti.Ner.Measure.Internal.MeasureToken.TryParseMinimal(t, null, false);
            if (mt != null) 
            {
                List<Pullenti.Ner.ReferentToken> mrs = mt.CreateRefenetsTokensWithRegister(null, false);
                if (mrs != null && mrs.Count > 0 && (mrs[mrs.Count - 1].Referent is Pullenti.Ner.Measure.MeasureReferent)) 
                {
                    Pullenti.Ner.Measure.MeasureReferent mr = mrs[mrs.Count - 1].Referent as Pullenti.Ner.Measure.MeasureReferent;
                    GoodAttrToken res = new GoodAttrToken(t, mt.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Numeric, Name = mr.GetStringValue(Pullenti.Ner.Measure.MeasureReferent.ATTR_NAME) };
                    res.Value = mr.ToString(true, null, 0);
                    return res;
                }
            }
            List<Pullenti.Ner.Measure.Internal.NumbersWithUnitToken> mts = Pullenti.Ner.Measure.Internal.NumbersWithUnitToken.TryParseMulti(t, null, false, false, false, false);
            if ((mts != null && mts.Count == 1 && mts[0].Units != null) && mts[0].Units.Count > 0) 
            {
                List<Pullenti.Ner.ReferentToken> mrs = mts[0].CreateRefenetsTokensWithRegister(null, null, true);
                Pullenti.Ner.ReferentToken mr = mrs[mrs.Count - 1];
                GoodAttrToken res = new GoodAttrToken(t, mr.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Numeric };
                res.Value = mr.Referent.ToString(true, null, 0);
                return res;
            }
            return null;
        }
        static GoodAttrToken _tryParseNum2(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.NumberToken) || (t as Pullenti.Ner.NumberToken).IntValue == null) 
                return null;
            Pullenti.Ner.Core.TerminToken tok = m_NumSuff.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null && (t.WhitespacesAfterCount < 3)) 
            {
                GoodAttrToken res = new GoodAttrToken(t, tok.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Numeric };
                res.Value = (t as Pullenti.Ner.NumberToken).Value + tok.Termin.CanonicText.ToLower();
                if (res.EndToken.Next != null && res.EndToken.Next.IsChar('.')) 
                    res.EndToken = res.EndToken.Next;
                return res;
            }
            Pullenti.Ner.NumberToken num = Pullenti.Ner.Core.NumberHelper.TryParseRealNumber(t, true, false);
            if (num != null) 
            {
                Pullenti.Ner.Token tt = num.EndToken;
                if (tt is Pullenti.Ner.MetaToken) 
                {
                    if ((tt as Pullenti.Ner.MetaToken).EndToken.IsValue("СП", null)) 
                    {
                        if (num.Value == "1") 
                            return new GoodAttrToken(t, tt) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Value = "односпальный" };
                        if (num.Value == "1.5") 
                            return new GoodAttrToken(t, tt) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Value = "полутораспальный" };
                        if (num.Value == "2") 
                            return new GoodAttrToken(t, tt) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Value = "вдухспальный" };
                    }
                }
                tt = tt.Next;
                if (tt != null && tt.IsHiphen) 
                    tt = tt.Next;
                if (tt != null && tt.IsValue("СП", null)) 
                {
                    if (num.Value == "1") 
                        return new GoodAttrToken(t, tt) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Value = "односпальный" };
                    if (num.Value == "1.5") 
                        return new GoodAttrToken(t, tt) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Value = "полутораспальный" };
                    if (num.Value == "2") 
                        return new GoodAttrToken(t, tt) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Value = "вдухспальный" };
                }
                return new GoodAttrToken(t, num.EndToken) { Typ = Pullenti.Ner.Goods.GoodAttrType.Numeric, Value = num.Value };
            }
            return null;
        }
        static GoodAttrToken _tryParseChars(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t1 = null;
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
            if (npt != null) 
                t1 = npt.EndToken;
            else if (((t is Pullenti.Ner.TextToken) && t.LengthChar > 2 && t.GetMorphClassInDictionary().IsUndefined) && !t.Chars.IsAllLower) 
                t1 = t;
            if (t1 == null) 
                return null;
            Pullenti.Ner.Token t11 = t1;
            Pullenti.Ner.Token t2 = null;
            for (Pullenti.Ner.Token tt = t1.Next; tt != null; tt = tt.Next) 
            {
                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt) || tt.IsChar(';')) 
                    break;
                if (tt.IsChar(':') || tt.IsHiphen) 
                {
                    t2 = tt.Next;
                    break;
                }
                if (tt.IsValue("ДА", null) || tt.IsValue("НЕТ", null)) 
                {
                    t2 = tt;
                    break;
                }
                Pullenti.Ner.Core.VerbPhraseToken vvv = Pullenti.Ner.Core.VerbPhraseHelper.TryParse(tt, false, false, false);
                if (vvv != null) 
                {
                    t2 = vvv.EndToken.Next;
                    break;
                }
                t1 = tt;
            }
            if (t2 == null) 
            {
                if (t11.Next != null && t11.Next.GetMorphClassInDictionary().IsAdjective && Pullenti.Ner.Core.NounPhraseHelper.TryParse(t11.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null) == null) 
                {
                    t1 = t11;
                    t2 = t11.Next;
                }
            }
            if (t2 == null) 
                return null;
            Pullenti.Ner.Token t3 = t2;
            for (Pullenti.Ner.Token tt = t2; tt != null; tt = tt.Next) 
            {
                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                    break;
                if (tt.IsChar(';')) 
                    break;
                t3 = tt;
            }
            string name = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1, Pullenti.Ner.Core.GetTextAttr.No);
            string val = Pullenti.Ner.Core.MiscHelper.GetTextValue(t2, (t3.IsChar('.') ? t3.Previous : t3), Pullenti.Ner.Core.GetTextAttr.No);
            if (string.IsNullOrEmpty(val)) 
                return null;
            return new GoodAttrToken(t, t3) { Typ = Pullenti.Ner.Goods.GoodAttrType.Character, Name = name, Value = val };
        }
        static Pullenti.Ner.Core.TerminCollection m_NumSuff = new Pullenti.Ner.Core.TerminCollection();
        static Pullenti.Ner.Core.TerminCollection m_StdAbbrs = new Pullenti.Ner.Core.TerminCollection();
        static Pullenti.Ner.Denomination.DenominationAnalyzer m_DenomAn = new Pullenti.Ner.Denomination.DenominationAnalyzer();
        static bool m_Inited = false;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("ПР");
            t.AddVariant("ПРЕДМЕТ", false);
            m_NumSuff.Add(t);
            t = new Pullenti.Ner.Core.Termin("ШТ");
            t.AddVariant("ШТУКА", false);
            m_NumSuff.Add(t);
            t = new Pullenti.Ner.Core.Termin("УП");
            t.AddVariant("УПАКОВКА", false);
            m_NumSuff.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЯЩ");
            t.AddVariant("ЯЩИК", false);
            m_NumSuff.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОРОБ");
            t.AddVariant("КОРОБКА", false);
            m_NumSuff.Add(t);
            t = new Pullenti.Ner.Core.Termin("БУТ");
            t.AddVariant("БУТЫЛКА", false);
            m_NumSuff.Add(t);
            t = new Pullenti.Ner.Core.Termin("МЕШ");
            t.AddVariant("МЕШОК", false);
            m_NumSuff.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЕРШ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Keyword };
            t.AddVariant("ЕРШИК", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОНДИЦИОНЕР") { Tag = Pullenti.Ner.Goods.GoodAttrType.Keyword };
            t.AddVariant("КОНДИЦ", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("УДЛИНИТЕЛЬ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Keyword };
            t.AddAbridge("УДЛ-ЛЬ");
            t.AddAbridge("УДЛИН-ЛЬ");
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("УСТРОЙСТВО") { Tag = Pullenti.Ner.Goods.GoodAttrType.Keyword };
            t.AddAbridge("УСТР-ВО");
            t.AddAbridge("УСТР.");
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОКЛАДКИ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Keyword };
            t.AddVariant("ПРОКЛ", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДЕЗОДОРАНТ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Keyword };
            t.AddVariant("ДЕЗ", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОХЛАЖДЕННЫЙ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Character };
            t.AddVariant("ОХЛ", false);
            t.AddVariant("ОХЛАЖД", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("МЕДИЦИНСКИЙ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Character };
            t.AddVariant("МЕД", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТЕРИЛЬНЫЙ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Character };
            t.AddVariant("СТЕР", false);
            t.AddVariant("СТ", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("ХЛОПЧАТОБУМАЖНЫЙ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Character };
            t.AddAbridge("Х/Б");
            t.AddAbridge("ХБ");
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДЕТСКИЙ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Character };
            t.AddVariant("ДЕТ", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("МУЖСКОЙ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Character };
            t.AddVariant("МУЖ", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЖЕНСКИЙ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Character };
            t.AddVariant("ЖЕН", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТРАНА") { Tag = Pullenti.Ner.Goods.GoodAttrType.Referent };
            t.AddVariant("СТРАНА ПРОИСХОЖДЕНИЯ", false);
            t.AddVariant("ПРОИСХОЖДЕНИЕ", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОИЗВОДИТЕЛЬ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Referent };
            t.AddAbridge("ПР-ЛЬ");
            t.AddAbridge("ПРОИЗВ-ЛЬ");
            t.AddAbridge("ПРОИЗВ.");
            t.AddVariant("ПРОИЗВОДСТВО", false);
            t.AddAbridge("ПР-ВО");
            t.AddVariant("ПРОИЗВЕСТИ", false);
            t.AddVariant("КОМПАНИЯ", false);
            t.AddVariant("ФИРМА", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТОВАРНЫЙ ЗНАК") { Tag = Pullenti.Ner.Goods.GoodAttrType.Undefined, Tag2 = "" };
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("КАТАЛОЖНЫЙ НОМЕР") { Tag = Pullenti.Ner.Goods.GoodAttrType.Undefined, Tag2 = "" };
            t.AddVariant("НОМЕР В КАТАЛОГЕ", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("МАРКА") { Tag = Pullenti.Ner.Goods.GoodAttrType.Undefined, Tag2 = "" };
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("ФИРМА") { Tag = Pullenti.Ner.Goods.GoodAttrType.Undefined, Tag2 = "" };
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("МОДЕЛЬ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Undefined, Tag2 = "" };
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("НЕТ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Undefined, Tag2 = "NO" };
            t.AddVariant("ОТСУТСТВОВАТЬ", false);
            t.AddVariant("НЕ ИМЕТЬ", false);
            m_StdAbbrs.Add(t);
            t = new Pullenti.Ner.Core.Termin("БОЛЕЕ") { Tag = Pullenti.Ner.Goods.GoodAttrType.Undefined };
            t.AddVariant("МЕНЕЕ", false);
            t.AddVariant("НЕ БОЛЕЕ", false);
            t.AddVariant("НЕ МЕНЕЕ", false);
            m_StdAbbrs.Add(t);
        }
    }
}