/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Instrument.Internal
{
    class ParticipantToken : Pullenti.Ner.MetaToken
    {
        public ParticipantToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public string Typ;
        public enum Kinds : int
        {
            Undefined,
            Pure,
            NamedAs,
            NamedAsParts,
        }

        public Kinds Kind = Kinds.Undefined;
        public List<Pullenti.Ner.Referent> Parts = null;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0}: {1}", Kind, Typ ?? "?");
            if (Parts != null) 
            {
                foreach (Pullenti.Ner.Referent p in Parts) 
                {
                    res.AppendFormat("; {0}", p.ToString(true, null, 0));
                }
            }
            return res.ToString();
        }
        public static ParticipantToken TryAttach(Pullenti.Ner.Token t, Pullenti.Ner.Instrument.InstrumentParticipantReferent p1 = null, Pullenti.Ner.Instrument.InstrumentParticipantReferent p2 = null, bool isContract = false)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token tt = t;
            bool br = false;
            if (p1 == null && p2 == null && isContract) 
            {
                Pullenti.Ner.Referent r1 = t.GetReferent();
                if ((r1 != null && t.Next != null && t.Next.IsCommaAnd) && (t.Next.Next is Pullenti.Ner.ReferentToken)) 
                {
                    Pullenti.Ner.Referent r2 = t.Next.Next.GetReferent();
                    if (r1.TypeName == r2.TypeName) 
                    {
                        Pullenti.Ner.Token ttt = t.Next.Next.Next;
                        List<Pullenti.Ner.Referent> refs = new List<Pullenti.Ner.Referent>();
                        refs.Add(r1);
                        refs.Add(r2);
                        for (; ttt != null; ttt = ttt.Next) 
                        {
                            if ((ttt.IsCommaAnd && ttt.Next != null && ttt.Next.GetReferent() != null) && ttt.Next.GetReferent().TypeName == r1.TypeName) 
                            {
                                ttt = ttt.Next;
                                if (!refs.Contains(ttt.GetReferent())) 
                                    refs.Add(ttt.GetReferent());
                                continue;
                            }
                            break;
                        }
                        for (; ttt != null; ttt = ttt.Next) 
                        {
                            if (ttt.IsComma || ttt.Morph.Class.IsPreposition) 
                                continue;
                            if ((ttt.IsValue("ИМЕНОВАТЬ", null) || ttt.IsValue("ДАЛЬНЕЙШИЙ", null) || ttt.IsValue("ДАЛЕЕ", null)) || ttt.IsValue("ТЕКСТ", null)) 
                                continue;
                            if (ttt.IsValue("ДОГОВАРИВАТЬСЯ", null)) 
                                continue;
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt != null && npt.Noun.IsValue("СТОРОНА", null) && npt.Morph.Number != Pullenti.Morph.MorphNumber.Singular) 
                            {
                                ParticipantToken re = new ParticipantToken(t, npt.EndToken) { Kind = Kinds.NamedAsParts };
                                re.Parts = refs;
                                return re;
                            }
                            break;
                        }
                    }
                }
                if ((r1 is Pullenti.Ner.Org.OrganizationReferent) || (r1 is Pullenti.Ner.Person.PersonReferent)) 
                {
                    bool hasBr = false;
                    bool hasNamed = false;
                    if (r1 is Pullenti.Ner.Person.PersonReferent) 
                    {
                        if (t.Previous != null && t.Previous.IsValue("ЛИЦО", null)) 
                            return null;
                    }
                    else if (t.Previous != null && ((t.Previous.IsValue("ВЫДАВАТЬ", null) || t.Previous.IsValue("ВЫДАТЬ", null)))) 
                        return null;
                    for (Pullenti.Ner.Token ttt = (t as Pullenti.Ner.ReferentToken).BeginToken; ttt != null && (ttt.EndChar < t.EndChar); ttt = ttt.Next) 
                    {
                        if (ttt.IsChar('(')) 
                            hasBr = true;
                        else if ((ttt.IsValue("ИМЕНОВАТЬ", null) || ttt.IsValue("ДАЛЬНЕЙШИЙ", null) || ttt.IsValue("ДАЛЕЕ", null)) || ttt.IsValue("ТЕКСТ", null)) 
                            hasNamed = true;
                        else if ((ttt.IsComma || ttt.Morph.Class.IsPreposition || ttt.IsHiphen) || ttt.IsChar(':')) 
                        {
                        }
                        else if (ttt is Pullenti.Ner.ReferentToken) 
                        {
                        }
                        else if (hasBr || hasNamed) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun, 0, null);
                            if (npt == null) 
                                break;
                            if (hasBr) 
                            {
                                if (npt.EndToken.Next == null || !npt.EndToken.Next.IsChar(')')) 
                                    break;
                            }
                            if (!hasNamed) 
                            {
                                if (m_Ontology.TryParse(ttt, Pullenti.Ner.Core.TerminParseAttr.No) == null) 
                                    break;
                            }
                            ParticipantToken re = new ParticipantToken(t, t) { Kind = Kinds.NamedAs };
                            re.Typ = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                            re.Parts = new List<Pullenti.Ner.Referent>();
                            re.Parts.Add(r1);
                            return re;
                        }
                    }
                    hasBr = false;
                    hasNamed = false;
                    Pullenti.Ner.Token endSide = null;
                    Pullenti.Ner.Core.BracketSequenceToken brr = null;
                    List<Pullenti.Ner.Referent> addRefs = null;
                    for (Pullenti.Ner.Token ttt = t.Next; ttt != null; ttt = ttt.Next) 
                    {
                        if ((ttt is Pullenti.Ner.NumberToken) && (ttt.Next is Pullenti.Ner.TextToken) && (ttt.Next as Pullenti.Ner.TextToken).Term == "СТОРОНЫ") 
                        {
                            endSide = (ttt = ttt.Next);
                            if (ttt.Next != null && ttt.Next.IsComma) 
                                ttt = ttt.Next;
                            if (ttt.Next != null && ttt.Next.IsAnd) 
                                break;
                        }
                        if (brr != null && ttt.BeginChar > brr.EndChar) 
                            brr = null;
                        if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(ttt, false, false)) 
                        {
                            brr = Pullenti.Ner.Core.BracketHelper.TryParse(ttt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (brr != null && (brr.LengthChar < 7) && ttt.IsChar('(')) 
                            {
                                ttt = brr.EndToken;
                                brr = null;
                                continue;
                            }
                        }
                        else if ((ttt.IsValue("ИМЕНОВАТЬ", null) || ttt.IsValue("ДАЛЬНЕЙШИЙ", null) || ttt.IsValue("ДАЛЕЕ", null)) || ttt.IsValue("ТЕКСТ", null)) 
                            hasNamed = true;
                        else if ((ttt.IsComma || ttt.Morph.Class.IsPreposition || ttt.IsHiphen) || ttt.IsChar(':')) 
                        {
                        }
                        else if (brr != null || hasNamed) 
                        {
                            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(ttt, true, false)) 
                                ttt = ttt.Next;
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.ReferentCanBeNoun, 0, null);
                            string typ22 = null;
                            if (npt != null) 
                            {
                                ttt = npt.EndToken;
                                if (npt.EndToken.IsValue("ДОГОВОР", null)) 
                                    continue;
                            }
                            else 
                            {
                                Pullenti.Ner.Core.TerminToken ttok = null;
                                if (ttt is Pullenti.Ner.MetaToken) 
                                    ttok = m_Ontology.TryParse((ttt as Pullenti.Ner.MetaToken).BeginToken, Pullenti.Ner.Core.TerminParseAttr.No);
                                if (ttok != null) 
                                    typ22 = ttok.Termin.CanonicText;
                                else if (hasNamed && ttt.Morph.Class.IsAdjective) 
                                    typ22 = ttt.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                                else if (brr != null) 
                                    continue;
                                else 
                                    break;
                            }
                            if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(ttt.Next, true, null, false)) 
                                ttt = ttt.Next;
                            if (brr != null) 
                            {
                                if (ttt.Next == null) 
                                {
                                    ttt = brr.EndToken;
                                    continue;
                                }
                                ttt = ttt.Next;
                            }
                            if (!hasNamed && typ22 == null) 
                            {
                                if (m_Ontology.TryParse(npt.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No) == null) 
                                    break;
                            }
                            ParticipantToken re = new ParticipantToken(t, ttt) { Kind = Kinds.NamedAs };
                            re.Typ = typ22 ?? npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                            re.Parts = new List<Pullenti.Ner.Referent>();
                            re.Parts.Add(r1);
                            return re;
                        }
                        else if ((ttt.IsValue("ЗАРЕГИСТРИРОВАННЫЙ", null) || ttt.IsValue("КАЧЕСТВО", null) || ttt.IsValue("ПРОЖИВАЮЩИЙ", null)) || ttt.IsValue("ЗАРЕГ", null)) 
                        {
                        }
                        else if (ttt.GetReferent() == r1) 
                        {
                        }
                        else if ((ttt.GetReferent() is Pullenti.Ner.Person.PersonIdentityReferent) || (ttt.GetReferent() is Pullenti.Ner.Address.AddressReferent)) 
                        {
                            if (addRefs == null) 
                                addRefs = new List<Pullenti.Ner.Referent>();
                            addRefs.Add(ttt.GetReferent());
                        }
                        else 
                        {
                            Pullenti.Ner.ReferentToken prr = ttt.Kit.ProcessReferent("PERSONPROPERTY", ttt);
                            if (prr != null) 
                            {
                                ttt = prr.EndToken;
                                continue;
                            }
                            if (ttt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                                continue;
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt != null) 
                            {
                                if ((npt.Noun.IsValue("МЕСТО", null) || npt.Noun.IsValue("ЖИТЕЛЬСТВО", null) || npt.Noun.IsValue("ПРЕДПРИНИМАТЕЛЬ", null)) || npt.Noun.IsValue("ПОЛ", null) || npt.Noun.IsValue("РОЖДЕНИЕ", null)) 
                                {
                                    ttt = npt.EndToken;
                                    continue;
                                }
                            }
                            if (ttt.IsNewlineBefore) 
                                break;
                            if (ttt.LengthChar < 3) 
                                continue;
                            Pullenti.Morph.MorphClass mc = ttt.GetMorphClassInDictionary();
                            if (mc.IsAdverb || mc.IsAdjective) 
                                continue;
                            if (ttt.Chars.IsAllUpper) 
                                continue;
                            break;
                        }
                    }
                    if (endSide != null || ((addRefs != null && t.Previous != null && t.Previous.IsAnd))) 
                    {
                        ParticipantToken re = new ParticipantToken(t, endSide ?? t) { Kind = Kinds.NamedAs };
                        re.Typ = null;
                        re.Parts = new List<Pullenti.Ner.Referent>();
                        re.Parts.Add(r1);
                        if (addRefs != null) 
                            re.Parts.AddRange(addRefs);
                        return re;
                    }
                }
                Pullenti.Ner.Core.TerminToken too = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (too != null) 
                {
                    if ((t.Previous is Pullenti.Ner.TextToken) && t.Previous.IsValue("ЛИЦО", null)) 
                        too = null;
                }
                if (too != null && too.Termin.Tag != null && too.Termin.CanonicText != "СТОРОНА") 
                {
                    Pullenti.Ner.Token tt1 = too.EndToken.Next;
                    if (tt1 != null) 
                    {
                        if (tt1.IsHiphen || tt1.IsChar(':')) 
                            tt1 = tt1.Next;
                    }
                    if (tt1 is Pullenti.Ner.ReferentToken) 
                    {
                        r1 = tt1.GetReferent();
                        if ((r1 is Pullenti.Ner.Person.PersonReferent) || (r1 is Pullenti.Ner.Org.OrganizationReferent)) 
                        {
                            ParticipantToken re = new ParticipantToken(t, tt1) { Kind = Kinds.NamedAs };
                            re.Typ = too.Termin.CanonicText;
                            re.Parts = new List<Pullenti.Ner.Referent>();
                            re.Parts.Add(r1);
                            return re;
                        }
                    }
                }
            }
            string addTyp1 = (p1 == null ? null : p1.Typ);
            string addTyp2 = (p2 == null ? null : p2.Typ);
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false) && tt.Next != null) 
            {
                br = true;
                tt = tt.Next;
            }
            Pullenti.Ner.Core.Termin term1 = null;
            Pullenti.Ner.Core.Termin term2 = null;
            if (addTyp1 != null && addTyp1.IndexOf(' ') > 0 && !addTyp1.StartsWith("СТОРОНА")) 
                term1 = new Pullenti.Ner.Core.Termin(addTyp1);
            if (addTyp2 != null && addTyp2.IndexOf(' ') > 0 && !addTyp2.StartsWith("СТОРОНА")) 
                term2 = new Pullenti.Ner.Core.Termin(addTyp2);
            bool named = false;
            string typ = null;
            Pullenti.Ner.Token t1 = null;
            Pullenti.Ner.Token t0 = tt;
            for (; tt != null; tt = tt.Next) 
            {
                if (tt.Morph.Class.IsPreposition && typ != null) 
                    continue;
                if (tt.IsCharOf("(:)") || tt.IsHiphen) 
                    continue;
                if (tt.IsTableControlChar) 
                    break;
                if (tt.IsNewlineBefore && tt != t0) 
                {
                    if (tt is Pullenti.Ner.NumberToken) 
                        break;
                    if ((tt is Pullenti.Ner.TextToken) && (tt.Previous is Pullenti.Ner.TextToken)) 
                    {
                        if (tt.Previous.IsValue((tt as Pullenti.Ner.TextToken).Term, null)) 
                            break;
                    }
                }
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, false)) 
                    continue;
                Pullenti.Ner.Core.TerminToken tok = (m_Ontology != null ? m_Ontology.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) : null);
                if (tok != null && (tt.Previous is Pullenti.Ner.TextToken)) 
                {
                    if (tt.Previous.IsValue("ЛИЦО", null)) 
                        return null;
                }
                if (tok == null) 
                {
                    if (addTyp1 != null && ((Pullenti.Ner.Core.MiscHelper.IsNotMoreThanOneError(addTyp1, tt) || (((tt is Pullenti.Ner.MetaToken) && (tt as Pullenti.Ner.MetaToken).BeginToken.IsValue(addTyp1, null)))))) 
                    {
                        if (typ != null) 
                        {
                            if (!_isTypesEqual(addTyp1, typ)) 
                                break;
                        }
                        typ = addTyp1;
                        t1 = tt;
                        continue;
                    }
                    if (addTyp2 != null && ((Pullenti.Ner.Core.MiscHelper.IsNotMoreThanOneError(addTyp2, tt) || (((tt is Pullenti.Ner.MetaToken) && (tt as Pullenti.Ner.MetaToken).BeginToken.IsValue(addTyp2, null)))))) 
                    {
                        if (typ != null) 
                        {
                            if (!_isTypesEqual(addTyp2, typ)) 
                                break;
                        }
                        typ = addTyp2;
                        t1 = tt;
                        continue;
                    }
                    if (tt.Chars.IsLetter) 
                    {
                        if (term1 != null) 
                        {
                            Pullenti.Ner.Core.TerminToken tok1 = term1.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                            if (tok1 != null) 
                            {
                                if (typ != null) 
                                {
                                    if (!_isTypesEqual(addTyp1, typ)) 
                                        break;
                                }
                                typ = addTyp1;
                                t1 = (tt = tok1.EndToken);
                                continue;
                            }
                        }
                        if (term2 != null) 
                        {
                            Pullenti.Ner.Core.TerminToken tok2 = term2.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                            if (tok2 != null) 
                            {
                                if (typ != null) 
                                {
                                    if (!_isTypesEqual(addTyp2, typ)) 
                                        break;
                                }
                                typ = addTyp2;
                                t1 = (tt = tok2.EndToken);
                                continue;
                            }
                        }
                        if (named && tt.GetMorphClassInDictionary().IsNoun) 
                        {
                            if (!tt.Chars.IsAllLower || Pullenti.Ner.Core.BracketHelper.IsBracket(tt.Previous, true)) 
                            {
                                if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt, false) == null) 
                                {
                                    string val = tt.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                                    if (typ != null) 
                                    {
                                        if (!_isTypesEqual(typ, val)) 
                                            break;
                                    }
                                    typ = val;
                                    t1 = tt;
                                    continue;
                                }
                            }
                        }
                    }
                    if (named && typ == null && isContract) 
                    {
                        if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsCyrillicLetter && tt.Chars.IsCapitalUpper) 
                        {
                            Pullenti.Morph.MorphClass dc = tt.GetMorphClassInDictionary();
                            if (dc.IsUndefined || dc.IsNoun) 
                            {
                                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt, null, false);
                                bool ok = true;
                                if (dt != null) 
                                    ok = false;
                                else if (tt.IsValue("СТОРОНА", null)) 
                                    ok = false;
                                if (ok) 
                                {
                                    typ = (tt as Pullenti.Ner.TextToken).Lemma;
                                    t1 = tt;
                                    continue;
                                }
                            }
                            if (dc.IsAdjective) 
                            {
                                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt != null && npt.Adjectives.Count > 0 && npt.Noun.GetMorphClassInDictionary().IsNoun) 
                                {
                                    typ = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                                    t1 = npt.EndToken;
                                    continue;
                                }
                            }
                        }
                    }
                    if (tt == t) 
                        break;
                    if ((tt is Pullenti.Ner.NumberToken) || tt.IsChar('.')) 
                        break;
                    if (tt.LengthChar < 4) 
                    {
                        if (typ != null) 
                            continue;
                    }
                    break;
                }
                if (tok.Termin.Tag == null) 
                    named = true;
                else 
                {
                    if (typ != null) 
                        break;
                    if (tok.Termin.CanonicText == "СТОРОНА") 
                    {
                        Pullenti.Ner.Token tt1 = tt.Next;
                        if (tt1 != null && tt1.IsHiphen) 
                            tt1 = tt1.Next;
                        if (!(tt1 is Pullenti.Ner.NumberToken)) 
                            break;
                        if (tt1.IsNewlineBefore) 
                            break;
                        typ = string.Format("{0} {1}", tok.Termin.CanonicText, (tt1 as Pullenti.Ner.NumberToken).Value);
                        t1 = tt1;
                    }
                    else 
                    {
                        typ = tok.Termin.CanonicText;
                        t1 = tok.EndToken;
                    }
                    break;
                }
                tt = tok.EndToken;
            }
            if (typ == null) 
                return null;
            if (!named && t1 != t && !typ.StartsWith("СТОРОНА")) 
            {
                if (!_isTypesEqual(typ, addTyp1) && !_isTypesEqual(typ, addTyp2)) 
                    return null;
            }
            if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next, false, null, false)) 
            {
                t1 = t1.Next;
                if (!t.IsWhitespaceBefore && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Previous, false, false)) 
                    t = t.Previous;
            }
            else if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next, true, t, true)) 
                t1 = t1.Next;
            if (br && t1.Next != null && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next, false, null, false)) 
                t1 = t1.Next;
            ParticipantToken res = new ParticipantToken(t, t1) { Kind = (named ? Kinds.NamedAs : Kinds.Pure), Typ = typ };
            if (t.IsChar(':')) 
                res.BeginToken = t.Next;
            return res;
        }
        static bool _isTypesEqual(string t1, string t2)
        {
            if (t1 == t2) 
                return true;
            if (t1 == "ЗАЙМОДАВЕЦ" || t1 == "ЗАИМОДАВЕЦ") 
                t1 = "ЗАИМОДАТЕЛЬ";
            if (t2 == "ЗАЙМОДАВЕЦ" || t2 == "ЗАИМОДАВЕЦ") 
                t2 = "ЗАИМОДАТЕЛЬ";
            if (t1 == "ПРОДАВЕЦ") 
                t1 = "ПОСТАВЩИК";
            if (t2 == "ПРОДАВЕЦ") 
                t2 = "ПОСТАВЩИК";
            if (t1 == "ПОКУПАТЕЛЬ") 
                t1 = "ЗАКАЗЧИК";
            if (t2 == "ПОКУПАТЕЛЬ") 
                t2 = "ЗАКАЗЧИК";
            return t1 == t2;
        }
        public static Pullenti.Ner.ReferentToken TryAttachToExist(Pullenti.Ner.Token t, Pullenti.Ner.Instrument.InstrumentParticipantReferent p1, Pullenti.Ner.Instrument.InstrumentParticipantReferent p2)
        {
            if (t == null) 
                return null;
            if (t.BeginChar >= 7674 && (t.BeginChar < 7680)) 
            {
            }
            ParticipantToken pp = ParticipantToken.TryAttach(t, p1, p2, false);
            Pullenti.Ner.Instrument.InstrumentParticipantReferent p = null;
            Pullenti.Ner.ReferentToken rt = null;
            if (pp == null || pp.Kind != Kinds.Pure) 
            {
                Pullenti.Ner.Referent pers = t.GetReferent();
                if ((pers is Pullenti.Ner.Person.PersonReferent) || (pers is Pullenti.Ner.Geo.GeoReferent) || (pers is Pullenti.Ner.Org.OrganizationReferent)) 
                {
                    if (p1 != null && p1.ContainsRef(pers)) 
                        p = p1;
                    else if (p2 != null && p2.ContainsRef(pers)) 
                        p = p2;
                    if (p != null) 
                        rt = new Pullenti.Ner.ReferentToken(p, t, t);
                }
            }
            else 
            {
                if (p1 != null && _isTypesEqual(pp.Typ, p1.Typ)) 
                    p = p1;
                else if (p2 != null && _isTypesEqual(pp.Typ, p2.Typ)) 
                    p = p2;
                if (p != null) 
                {
                    rt = new Pullenti.Ner.ReferentToken(p, pp.BeginToken, pp.EndToken);
                    if (rt.BeginToken.Previous != null && rt.BeginToken.Previous.IsValue("ОТ", null)) 
                        rt.BeginToken = rt.BeginToken.Previous;
                }
            }
            if (rt == null) 
                return null;
            if (rt.EndToken.Next != null && rt.EndToken.Next.IsChar(':')) 
            {
                Pullenti.Ner.ReferentToken rt1 = TryAttachRequisites(rt.EndToken.Next.Next, p, (p == p1 ? p2 : p1), false);
                if (rt1 != null) 
                {
                    rt1.BeginToken = rt.BeginToken;
                    return rt1;
                }
                rt.EndToken = rt.EndToken.Next;
            }
            while (rt.EndToken.Next != null && (rt.EndToken.Next.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
            {
                Pullenti.Ner.Org.OrganizationReferent org = rt.EndToken.Next.GetReferent() as Pullenti.Ner.Org.OrganizationReferent;
                if (rt.Referent.FindSlot(null, org, true) != null) 
                {
                    rt.EndToken = rt.EndToken.Next;
                    continue;
                }
                break;
            }
            return rt;
        }
        public static Pullenti.Ner.ReferentToken TryAttachRequisites(Pullenti.Ner.Token t, Pullenti.Ner.Instrument.InstrumentParticipantReferent cur, Pullenti.Ner.Instrument.InstrumentParticipantReferent other, bool cantBeEmpty = false)
        {
            if (t == null || cur == null) 
                return null;
            if (t.IsTableControlChar) 
                return null;
            int err = 0;
            int specChars = 0;
            Pullenti.Ner.ReferentToken rt = null;
            Pullenti.Ner.Token t0 = t;
            bool isInTabCell = false;
            int cou = 0;
            for (Pullenti.Ner.Token tt = t.Next; tt != null && (cou < 300); tt = tt.Next,cou++) 
            {
                if (tt.IsTableControlChar) 
                {
                    isInTabCell = true;
                    break;
                }
            }
            for (; t != null; t = t.Next) 
            {
                if (t.BeginChar == 8923) 
                {
                }
                if (t.IsTableControlChar) 
                {
                    if (t != t0) 
                    {
                        if (rt != null) 
                            rt.EndToken = t.Previous;
                        else if (!cantBeEmpty) 
                            rt = new Pullenti.Ner.ReferentToken(cur, t0, t.Previous);
                        break;
                    }
                    else 
                        continue;
                }
                if ((t.IsCharOf(":.") || t.IsValue("М", null) || t.IsValue("M", null)) || t.IsValue("П", null)) 
                {
                    if (rt != null) 
                        rt.EndToken = t;
                    continue;
                }
                Pullenti.Ner.ReferentToken pp = ParticipantToken.TryAttachToExist(t, cur, other);
                if (pp != null) 
                {
                    if (pp.Referent != cur) 
                        break;
                    if (rt == null) 
                        rt = new Pullenti.Ner.ReferentToken(cur, t, t);
                    rt.EndToken = pp.EndToken;
                    err = 0;
                    continue;
                }
                if (t.IsNewlineBefore) 
                {
                    InstrToken iii = InstrToken.Parse(t, 0, null);
                    if (iii != null) 
                    {
                        if (iii.Typ == ILTypes.Appendix) 
                            break;
                    }
                }
                if (t.WhitespacesBeforeCount > 25 && !isInTabCell) 
                {
                    if (t != t0) 
                    {
                        if (t.Previous != null && t.Previous.IsCharOf(",;")) 
                        {
                        }
                        else if (t.NewlinesBeforeCount > 1) 
                            break;
                    }
                    if ((t.GetReferent() is Pullenti.Ner.Person.PersonReferent) || (t.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                    {
                        if (!cur.ContainsRef(t.GetReferent())) 
                            break;
                    }
                }
                if ((t.IsCharOf(";:,.") || t.IsHiphen || t.Morph.Class.IsPreposition) || t.Morph.Class.IsConjunction) 
                    continue;
                if (t.IsCharOf("_/\\")) 
                {
                    if ((++specChars) > 10 && rt == null) 
                        rt = new Pullenti.Ner.ReferentToken(cur, t0, t);
                    if (rt != null) 
                        rt.EndToken = t;
                    continue;
                }
                if (t.IsNewlineBefore && (t is Pullenti.Ner.NumberToken)) 
                    break;
                if (t.IsValue("ОФИС", null)) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Next, true, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            t = br.EndToken;
                            continue;
                        }
                    }
                    if ((t.Next is Pullenti.Ner.TextToken) && !t.Next.Chars.IsAllLower) 
                        t = t.Next;
                    continue;
                }
                Pullenti.Ner.Referent r = t.GetReferent();
                if ((((r is Pullenti.Ner.Person.PersonReferent) || (r is Pullenti.Ner.Address.AddressReferent) || (r is Pullenti.Ner.Uri.UriReferent)) || (r is Pullenti.Ner.Org.OrganizationReferent) || (r is Pullenti.Ner.Phone.PhoneReferent)) || (r is Pullenti.Ner.Person.PersonIdentityReferent) || (r is Pullenti.Ner.Bank.BankDataReferent)) 
                {
                    if (other != null && other.FindSlot(null, r, true) != null) 
                    {
                        if (!(r is Pullenti.Ner.Uri.UriReferent)) 
                            break;
                    }
                    if (rt == null) 
                        rt = new Pullenti.Ner.ReferentToken(cur, t, t);
                    if (cur.FindSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_DELEGATE, r, true) != null) 
                    {
                    }
                    else 
                        cur.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, r, false, 0);
                    rt.EndToken = t;
                    err = 0;
                }
                else 
                {
                    if ((t is Pullenti.Ner.TextToken) && t.LengthChar > 1) 
                        ++err;
                    if (isInTabCell && rt != null) 
                    {
                        if (err > 300) 
                            break;
                    }
                    else if (err > 4) 
                        break;
                }
            }
            return rt;
        }
        public Pullenti.Ner.ReferentToken AttachFirst(Pullenti.Ner.Instrument.InstrumentParticipantReferent p, int minChar, int maxChar)
        {
            Pullenti.Ner.Token t;
            Pullenti.Ner.Token tt0 = BeginToken;
            List<Pullenti.Ner.Referent> refs = new List<Pullenti.Ner.Referent>();
            for (t = tt0.Previous; t != null && t.BeginChar >= minChar; t = t.Previous) 
            {
                if (t.IsNewlineAfter) 
                {
                    if (t.NewlinesAfterCount > 1) 
                        break;
                    if (t.Next is Pullenti.Ner.NumberToken) 
                        break;
                }
                Pullenti.Ner.Token tt = _tryAttachContractGround(t, p, false);
                if (tt != null) 
                    continue;
                Pullenti.Ner.Referent r = t.GetReferent();
                if (((((r is Pullenti.Ner.Org.OrganizationReferent) || (r is Pullenti.Ner.Phone.PhoneReferent) || (r is Pullenti.Ner.Person.PersonReferent)) || (r is Pullenti.Ner.Person.PersonPropertyReferent) || (r is Pullenti.Ner.Address.AddressReferent)) || (r is Pullenti.Ner.Uri.UriReferent) || (r is Pullenti.Ner.Person.PersonIdentityReferent)) || (r is Pullenti.Ner.Bank.BankDataReferent)) 
                {
                    if (!refs.Contains(r)) 
                        refs.Insert(0, r);
                    tt0 = t;
                }
            }
            if (refs.Count > 0) 
            {
                foreach (Pullenti.Ner.Referent r in refs) 
                {
                    if (r != refs[0] && (refs[0] is Pullenti.Ner.Org.OrganizationReferent) && (((r is Pullenti.Ner.Person.PersonReferent) || (r is Pullenti.Ner.Person.PersonPropertyReferent)))) 
                        p.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_DELEGATE, r, false, 0);
                    else 
                        p.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, r, false, 0);
                }
            }
            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(p, tt0, EndToken);
            t = EndToken.Next;
            if (Pullenti.Ner.Core.BracketHelper.IsBracket(t, false)) 
                t = t.Next;
            if (t != null && t.IsChar(',')) 
                t = t.Next;
            for (; t != null && ((maxChar == 0 || t.BeginChar <= maxChar)); t = t.Next) 
            {
                if (t.IsValue("СТОРОНА", null)) 
                    break;
                Pullenti.Ner.Referent r = t.GetReferent();
                if (((((r is Pullenti.Ner.Org.OrganizationReferent) || (r is Pullenti.Ner.Phone.PhoneReferent) || (r is Pullenti.Ner.Person.PersonReferent)) || (r is Pullenti.Ner.Person.PersonPropertyReferent) || (r is Pullenti.Ner.Address.AddressReferent)) || (r is Pullenti.Ner.Uri.UriReferent) || (r is Pullenti.Ner.Person.PersonIdentityReferent)) || (r is Pullenti.Ner.Bank.BankDataReferent)) 
                {
                    if ((((r is Pullenti.Ner.Person.PersonPropertyReferent) && t.Next != null && t.Next.IsComma) && (t.Next.Next is Pullenti.Ner.ReferentToken) && (t.Next.Next.GetReferent() is Pullenti.Ner.Person.PersonReferent)) && !t.Next.IsNewlineAfter) 
                    {
                        Pullenti.Ner.Person.PersonReferent pe = t.Next.Next.GetReferent() as Pullenti.Ner.Person.PersonReferent;
                        pe.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_ATTR, r, false, 0);
                        r = pe;
                        t = t.Next.Next;
                    }
                    bool isDelegate = false;
                    if (t.Previous.IsValue("ЛИЦО", null) || t.Previous.IsValue("ИМЯ", null)) 
                        isDelegate = true;
                    if (t.Previous.IsValue("КОТОРЫЙ", null) && t.Previous.Previous != null && ((t.Previous.Previous.IsValue("ИМЯ", null) || t.Previous.Previous.IsValue("ЛИЦО", null)))) 
                        isDelegate = true;
                    p.AddSlot(((((r is Pullenti.Ner.Person.PersonReferent) || (r is Pullenti.Ner.Person.PersonPropertyReferent))) && isDelegate ? Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_DELEGATE : Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF), r, false, 0);
                    rt.EndToken = t;
                    continue;
                }
                Pullenti.Ner.Token tt = _tryAttachContractGround(t, p, false);
                if (tt != null) 
                {
                    t = (rt.EndToken = tt);
                    if (rt.BeginChar == tt.BeginChar) 
                        rt.BeginToken = tt;
                    continue;
                }
                if (t.IsValue("В", null) && t.Next != null && t.Next.IsValue("ЛИЦО", null)) 
                {
                    t = t.Next;
                    continue;
                }
                if (t.IsValue("ОТ", null) && t.Next != null && t.Next.IsValue("ИМЯ", null)) 
                {
                    t = t.Next;
                    continue;
                }
                if (t.IsValue("ПО", null) && t.Next != null && t.Next.IsValue("ПОРУЧЕНИЕ", null)) 
                {
                    t = t.Next;
                    continue;
                }
                if (t.IsNewlineBefore) 
                    break;
                if (t.GetMorphClassInDictionary() == Pullenti.Morph.MorphClass.Verb) 
                {
                    if ((!t.IsValue("УДОСТОВЕРЯТЬ", null) && !t.IsValue("ПРОЖИВАТЬ", null) && !t.IsValue("ЗАРЕГИСТРИРОВАТЬ", null)) && !t.IsValue("ДЕЙСТВОВАТЬ", null)) 
                        break;
                }
                if (t.IsAnd && t.Previous != null && t.Previous.IsComma) 
                    break;
                if (t.IsAnd && t.Next.GetReferent() != null) 
                {
                    if (t.Next.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) 
                        break;
                    Pullenti.Ner.Person.PersonReferent pe = t.Next.GetReferent() as Pullenti.Ner.Person.PersonReferent;
                    if (pe != null) 
                    {
                        bool hasIp = false;
                        foreach (Pullenti.Ner.Slot s in pe.Slots) 
                        {
                            if (s.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_ATTR) 
                            {
                                if (s.Value.ToString().StartsWith("индивидуальный предприниматель")) 
                                {
                                    hasIp = true;
                                    break;
                                }
                            }
                        }
                        if (hasIp) 
                            break;
                    }
                }
            }
            for (t = rt.BeginToken; t != null && t.EndChar <= rt.EndChar; t = t.Next) 
            {
                Pullenti.Ner.Token tt = _tryAttachContractGround(t, p, true);
                if (tt != null) 
                {
                    if (tt.EndChar > rt.EndChar) 
                        rt.EndToken = tt;
                    t = tt;
                }
            }
            return rt;
        }
        static Pullenti.Ner.Token _tryAttachContractGround(Pullenti.Ner.Token t, Pullenti.Ner.Instrument.InstrumentParticipantReferent ip, bool canBePassport = false)
        {
            bool ok = false;
            for (; t != null; t = t.Next) 
            {
                if (t.IsChar(',') || t.Morph.Class.IsPreposition) 
                    continue;
                if (t.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        t = br.EndToken;
                        continue;
                    }
                }
                if (t.IsValue("ОСНОВАНИЕ", null) || t.IsValue("ДЕЙСТВОВАТЬ", null) || t.IsValue("ДЕЙСТВУЮЩИЙ", null)) 
                {
                    ok = true;
                    if (t.Next != null && t.Next.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null && (br.LengthChar < 10)) 
                            t = br.EndToken;
                    }
                    continue;
                }
                Pullenti.Ner.Decree.DecreeReferent dr = t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                if (dr != null) 
                {
                    ip.Ground = dr;
                    return t;
                }
                Pullenti.Ner.Person.PersonIdentityReferent pir = t.GetReferent() as Pullenti.Ner.Person.PersonIdentityReferent;
                if (pir != null && canBePassport) 
                {
                    if (pir.Typ != null && !pir.Typ.Contains("паспорт")) 
                    {
                        ip.Ground = pir;
                        return t;
                    }
                }
                if (t.IsValue("УСТАВ", null)) 
                {
                    ip.Ground = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                    return t;
                }
                if (t.IsValue("ДОВЕРЕННОСТЬ", null)) 
                {
                    List<Pullenti.Ner.Decree.Internal.DecreeToken> dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(t.Next, null, 10, false);
                    if (dts == null) 
                    {
                        bool hasSpec = false;
                        for (Pullenti.Ner.Token ttt = t.Next; ttt != null && ((ttt.EndChar - t.EndChar) < 200); ttt = ttt.Next) 
                        {
                            if (ttt.IsComma) 
                                continue;
                            if (ttt.IsValue("УДОСТОВЕРИТЬ", null) || ttt.IsValue("УДОСТОВЕРЯТЬ", null)) 
                            {
                                hasSpec = true;
                                continue;
                            }
                            Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(ttt, null, false);
                            if (dt != null) 
                            {
                                if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                                {
                                    dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(ttt, null, 10, false);
                                    break;
                                }
                            }
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt != null) 
                            {
                                if (npt.EndToken.IsValue("НОТАРИУС", null)) 
                                {
                                    ttt = npt.EndToken;
                                    hasSpec = true;
                                    continue;
                                }
                            }
                            if (ttt.GetReferent() != null) 
                            {
                                if (hasSpec) 
                                    continue;
                            }
                            break;
                        }
                    }
                    if (dts != null && dts.Count > 0) 
                    {
                        Pullenti.Ner.Token t0 = t;
                        dr = new Pullenti.Ner.Decree.DecreeReferent();
                        dr.Typ = "ДОВЕРЕННОСТЬ";
                        foreach (Pullenti.Ner.Decree.Internal.DecreeToken d in dts) 
                        {
                            if (d.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                            {
                                dr.AddDate(d);
                                t = d.EndToken;
                            }
                            else if (d.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            {
                                dr.AddNumber(d);
                                t = d.EndToken;
                            }
                            else 
                                break;
                        }
                        Pullenti.Ner.Core.AnalyzerData ad = t.Kit.GetAnalyzerDataByAnalyzerName(Pullenti.Ner.Instrument.InstrumentAnalyzer.ANALYZER_NAME);
                        ip.Ground = ad.RegisterReferent(dr);
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ip.Ground as Pullenti.Ner.Referent, t0, t);
                        t.Kit.EmbedToken(rt);
                        return rt;
                    }
                    ip.Ground = "ДОВЕРЕННОСТЬ";
                    return t;
                }
                break;
            }
            return null;
        }
        public static List<string> GetDocTypes(string name, string name2)
        {
            List<string> res = new List<string>();
            if (name == null) 
                return res;
            if (name == "АРЕНДОДАТЕЛЬ") 
            {
                res.Add("ДОГОВОР АРЕНДЫ");
                res.Add("ДОГОВОР СУБАРЕНДЫ");
            }
            else if (name == "АРЕНДАТОР") 
                res.Add("ДОГОВОР АРЕНДЫ");
            else if (name == "СУБАРЕНДАТОР") 
                res.Add("ДОГОВОР СУБАРЕНДЫ");
            else if (name == "НАЙМОДАТЕЛЬ" || name == "НАНИМАТЕЛЬ") 
                res.Add("ДОГОВОР НАЙМА");
            else if (name == "АГЕНТ" || name == "ПРИНЦИПАЛ") 
                res.Add("АГЕНТСКИЙ ДОГОВОР");
            else if (name == "ПРОДАВЕЦ" || name == "ПОКУПАТЕЛЬ") 
                res.Add("ДОГОВОР КУПЛИ-ПРОДАЖИ");
            else if (name == "ЗАКАЗЧИК" || name == "ИСПОЛНИТЕЛЬ" || Pullenti.Morph.LanguageHelper.EndsWith(name, "ПОДРЯДЧИК")) 
                res.Add("ДОГОВОР УСЛУГ");
            else if (name == "ПОСТАВЩИК") 
                res.Add("ДОГОВОР ПОСТАВКИ");
            else if (name == "ЛИЦЕНЗИАР" || name == "ЛИЦЕНЗИАТ") 
                res.Add("ЛИЦЕНЗИОННЫЙ ДОГОВОР");
            else if (name == "СТРАХОВЩИК" || name == "СТРАХОВАТЕЛЬ") 
                res.Add("ДОГОВОР СТРАХОВАНИЯ");
            if (name2 == null) 
                return res;
            List<string> tmp = GetDocTypes(name2, null);
            for (int i = res.Count - 1; i >= 0; i--) 
            {
                if (!tmp.Contains(res[i])) 
                    res.RemoveAt(i);
            }
            return res;
        }
        internal static Pullenti.Ner.Core.TerminCollection m_Ontology;
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            foreach (string s in new string[] {"АРЕНДОДАТЕЛЬ", "АРЕНДАТОР", "СУБАРЕНДАТОР", "НАЙМОДАТЕЛЬ", "НАНИМАТЕЛЬ", "АГЕНТ", "ПРИНЦИПАЛ", "ПРОДАВЕЦ", "ПОКУПАТЕЛЬ", "ЗАКАЗЧИК", "ИСПОЛНИТЕЛЬ", "ПОСТАВЩИК", "ПОДРЯДЧИК", "СУБПОДРЯДЧИК", "СТОРОНА", "ЛИЦЕНЗИАР", "ЛИЦЕНЗИАТ", "СТРАХОВЩИК", "СТРАХОВАТЕЛЬ", "ПРОВАЙДЕР", "АБОНЕНТ", "ЗАСТРОЙЩИК", "УЧАСТНИК ДОЛЕВОГО СТРОИТЕЛЬСТВА", "КЛИЕНТ", "ЗАЕМЩИК", "УПРАВЛЯЮЩИЙ"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = m_Ontology });
            }
            t = new Pullenti.Ner.Core.Termin("ГЕНПОДРЯДЧИК") { Tag = m_Ontology };
            t.AddVariant("ГЕНЕРАЛЬНЫЙ ПОДРЯДЧИК", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗАИМОДАТЕЛЬ") { Tag = m_Ontology };
            t.AddVariant("ЗАЙМОДАТЕЛЬ", false);
            t.AddVariant("ЗАЙМОДАВЕЦ", false);
            t.AddVariant("ЗАИМОДАВЕЦ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ИМЕНУЕМЫЙ");
            t.AddVariant("ИМЕНОВАТЬСЯ", false);
            t.AddVariant("ИМЕНУЕМ", false);
            t.AddVariant("ДАЛЬНЕЙШИЙ", false);
            t.AddVariant("ДАЛЕЕ", false);
            t.AddVariant("ДАЛЕЕ ПО ТЕКСТУ", false);
            m_Ontology.Add(t);
        }
    }
}