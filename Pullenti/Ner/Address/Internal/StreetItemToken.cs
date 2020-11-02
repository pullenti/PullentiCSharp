/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Address.Internal
{
    public class StreetItemToken : Pullenti.Ner.MetaToken
    {
        private StreetItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public StreetItemType Typ;
        public Pullenti.Ner.Core.Termin Termin;
        public Pullenti.Ner.Core.Termin AltTermin;
        public Pullenti.Ner.Address.StreetReferent ExistStreet;
        public Pullenti.Ner.NumberToken Number;
        public bool NumberHasPrefix;
        public bool IsNumberKm;
        public string Value;
        public string AltValue;
        public string AltValue2;
        /// <summary>
        /// Признак сокращения
        /// </summary>
        public bool IsAbridge;
        public bool IsInDictionary;
        public bool IsInBrackets;
        public bool HasStdSuffix;
        public int NounIsDoubtCoef;
        public bool IsRoad
        {
            get
            {
                if (Termin == null) 
                    return false;
                if ((Termin.CanonicText == "АВТОДОРОГА" || Termin.CanonicText == "ШОССЕ" || Termin.CanonicText == "АВТОШЛЯХ") || Termin.CanonicText == "ШОСЕ") 
                    return true;
                return false;
            }
        }
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0}", Typ.ToString());
            if (Value != null) 
            {
                res.AppendFormat(" {0}", Value);
                if (AltValue != null) 
                    res.AppendFormat("/{0}", AltValue);
            }
            if (ExistStreet != null) 
                res.AppendFormat(" {0}", ExistStreet.ToString());
            if (Termin != null) 
            {
                res.AppendFormat(" {0}", Termin.ToString());
                if (AltTermin != null) 
                    res.AppendFormat("/{0}", AltTermin.ToString());
            }
            else if (Number != null) 
                res.AppendFormat(" {0}", Number.ToString());
            else 
                res.AppendFormat(" {0}", base.ToString());
            if (IsAbridge) 
                res.Append(" (?)");
            return res.ToString();
        }
        bool _isSurname()
        {
            if (Typ != StreetItemType.Name) 
                return false;
            if (!(EndToken is Pullenti.Ner.TextToken)) 
                return false;
            string nam = (EndToken as Pullenti.Ner.TextToken).Term;
            if (nam.Length > 4) 
            {
                if (Pullenti.Morph.LanguageHelper.EndsWithEx(nam, "А", "Я", "КО", "ЧУКА")) 
                {
                    if (!Pullenti.Morph.LanguageHelper.EndsWithEx(nam, "АЯ", "ЯЯ", null, null)) 
                        return true;
                }
            }
            return false;
        }
        public static StreetItemToken TryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locStreets, bool recurse = false, StreetItemToken prev = null, bool ignoreOnto = false)
        {
            if (t == null) 
                return null;
            if (t.Kit.IsRecurceOverflow) 
                return null;
            t.Kit.RecurseLevel++;
            StreetItemToken res = _tryParse(t, locStreets, recurse, prev, ignoreOnto);
            t.Kit.RecurseLevel--;
            return res;
        }
        public static StreetItemToken _tryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locStreets, bool recurse, StreetItemToken prev, bool ignoreOnto)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token tn = null;
            if (t.IsValue("ИМЕНИ", null) || t.IsValue("ІМЕНІ", null)) 
                tn = t;
            else if (t.IsValue("ИМ", null) || t.IsValue("ІМ", null)) 
            {
                tn = t;
                if (tn.Next != null && tn.Next.IsChar('.')) 
                    tn = tn.Next;
            }
            if (tn != null) 
            {
                if (tn.Next == null || tn.WhitespacesAfterCount > 2) 
                    return null;
                t = tn.Next;
            }
            Pullenti.Ner.NumberToken nt = Pullenti.Ner.Core.NumberHelper.TryParseAge(t);
            if (nt != null && nt.IntValue != null) 
                return new StreetItemToken(nt.BeginToken, nt.EndToken) { Typ = StreetItemType.Age, Number = nt };
            if ((((nt = t as Pullenti.Ner.NumberToken))) != null) 
            {
                if ((nt as Pullenti.Ner.NumberToken).IntValue == null || (nt as Pullenti.Ner.NumberToken).IntValue.Value == 0) 
                    return null;
                StreetItemToken res = new StreetItemToken(nt, nt) { Typ = StreetItemType.Number, Number = nt, Morph = nt.Morph };
                if ((t.Next != null && t.Next.IsHiphen && t.Next.Next != null) && t.Next.Next.IsValue("Я", null)) 
                    res.EndToken = t.Next.Next;
                Pullenti.Ner.Core.NumberExToken nex = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(t);
                if (nex != null) 
                {
                    if (nex.ExTyp == Pullenti.Ner.Core.NumberExType.Kilometer) 
                    {
                        res.IsNumberKm = true;
                        res.EndToken = nex.EndToken;
                    }
                    else 
                        return null;
                }
                AddressItemToken aaa = AddressItemToken.TryParse(t, null, false, true, null);
                if (aaa != null && aaa.Typ == AddressItemToken.ItemType.Number && aaa.EndChar > t.EndChar) 
                {
                    if (prev != null && prev.Typ == StreetItemType.Noun && prev.Termin.CanonicText == "КВАРТАЛ") 
                    {
                        res.EndToken = aaa.EndToken;
                        res.Value = aaa.Value;
                        res.Number = null;
                    }
                    else 
                        return null;
                }
                if (nt.Typ == Pullenti.Ner.NumberSpellingType.Words && nt.Morph.Class.IsAdjective) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt2 != null && npt2.EndChar > t.EndChar && npt2.Morph.Number != Pullenti.Morph.MorphNumber.Singular) 
                    {
                        if (t.Next != null && !t.Next.Chars.IsAllLower) 
                        {
                        }
                        else 
                            return null;
                    }
                }
                return res;
            }
            Pullenti.Ner.Token ntt = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t);
            if ((ntt != null && (ntt is Pullenti.Ner.NumberToken) && prev != null) && (ntt as Pullenti.Ner.NumberToken).IntValue != null) 
                return new StreetItemToken(t, ntt) { Typ = StreetItemType.Number, Number = ntt as Pullenti.Ner.NumberToken, NumberHasPrefix = true };
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt != null && tt.Morph.Class.IsAdjective) 
            {
                if (tt.Chars.IsCapitalUpper || ((prev != null && prev.Typ == StreetItemType.Number && tt.IsValue("ТРАНСПОРТНЫЙ", null)))) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(npt.Noun, Pullenti.Ner.Core.GetTextAttr.No).Contains("-")) 
                        npt = null;
                    Pullenti.Ner.Token tte = tt.Next;
                    if (npt != null && npt.Adjectives.Count == 1) 
                        tte = npt.EndToken;
                    if (tte != null) 
                    {
                        if ((((((((((tte.IsValue("ВАЛ", null) || tte.IsValue("ТРАКТ", null) || tte.IsValue("ПОЛЕ", null)) || tte.IsValue("МАГИСТРАЛЬ", null) || tte.IsValue("СПУСК", null)) || tte.IsValue("ВЗВОЗ", null) || tte.IsValue("РЯД", null)) || tte.IsValue("СЛОБОДА", null) || tte.IsValue("РОЩА", null)) || tte.IsValue("ПРУД", null) || tte.IsValue("СЪЕЗД", null)) || tte.IsValue("КОЛЬЦО", null) || tte.IsValue("МАГІСТРАЛЬ", null)) || tte.IsValue("УЗВІЗ", null) || tte.IsValue("ЛІНІЯ", null)) || tte.IsValue("УЗВІЗ", null) || tte.IsValue("ГАЙ", null)) || tte.IsValue("СТАВОК", null) || tte.IsValue("ЗЇЗД", null)) || tte.IsValue("КІЛЬЦЕ", null)) 
                        {
                            StreetItemToken sit = new StreetItemToken(tt, tte) { HasStdSuffix = true };
                            sit.Typ = StreetItemType.Name;
                            if (npt == null || npt.Adjectives.Count == 0) 
                                sit.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt, tte, Pullenti.Ner.Core.GetTextAttr.No);
                            else 
                                sit.Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                            Pullenti.Ner.Core.TerminToken tok2 = m_Ontology.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                            if (tok2 != null && tok2.Termin != null && tok2.EndToken == tte) 
                                sit.Termin = tok2.Termin;
                            return sit;
                        }
                    }
                    if (npt != null && npt.BeginToken != npt.EndToken && npt.Adjectives.Count <= 1) 
                    {
                        Pullenti.Ner.Token tt1 = npt.EndToken.Next;
                        if (tt1 != null && tt1.IsComma) 
                            tt1 = tt1.Next;
                        bool ok = false;
                        StreetItemToken sti1 = (recurse ? null : TryParse(tt1, locStreets, true, null, false));
                        if (sti1 != null && sti1.Typ == StreetItemType.Noun) 
                            ok = true;
                        else 
                        {
                            AddressItemToken ait = AddressItemToken.TryParse(tt1, locStreets, false, true, null);
                            if (ait != null) 
                            {
                                if (ait.Typ == AddressItemToken.ItemType.House) 
                                    ok = true;
                                else if (ait.Typ == AddressItemToken.ItemType.Number) 
                                {
                                    AddressItemToken ait2 = AddressItemToken.TryParse(npt.EndToken, locStreets, false, true, null);
                                    if (ait2 == null) 
                                        ok = true;
                                }
                            }
                        }
                        if (ok) 
                        {
                            sti1 = TryParse(npt.EndToken, locStreets, false, null, false);
                            if (sti1 != null && sti1.Typ == StreetItemType.Noun) 
                                ok = false;
                            else 
                            {
                                Pullenti.Ner.Core.TerminToken tok2 = m_Ontology.TryParse(npt.EndToken, Pullenti.Ner.Core.TerminParseAttr.No);
                                if (tok2 != null) 
                                {
                                    StreetItemType typ = (StreetItemType)tok2.Termin.Tag;
                                    if (typ == StreetItemType.Noun || typ == StreetItemType.StdPartOfName) 
                                        ok = false;
                                }
                            }
                        }
                        if (ok) 
                        {
                            StreetItemToken sit = new StreetItemToken(tt, npt.EndToken);
                            sit.Typ = StreetItemType.Name;
                            sit.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt, npt.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                            sit.AltValue = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                            return sit;
                        }
                    }
                }
            }
            if ((tt != null && (tt.Next is Pullenti.Ner.TextToken) && tt.Next.Chars.IsCapitalUpper) && !recurse) 
            {
                if ((tt.IsValue("ВАЛ", null) || tt.IsValue("ТРАКТ", null) || tt.IsValue("ПОЛЕ", null)) || tt.IsValue("КОЛЬЦО", null) || tt.IsValue("КІЛЬЦЕ", null)) 
                {
                    StreetItemToken sit = TryParse(tt.Next, locStreets, true, null, false);
                    if (sit != null && sit.Typ == StreetItemType.Name) 
                    {
                        if (sit.Value != null) 
                            sit.Value = string.Format("{0} {1}", sit.Value, tt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                        else 
                            sit.Value = string.Format("{0} {1}", sit.GetSourceText().ToUpper(), tt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                        if (sit.AltValue != null) 
                            sit.AltValue = string.Format("{0} {1}", sit.AltValue, tt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                        sit.BeginToken = tt;
                        return sit;
                    }
                }
            }
            if (((tt != null && tt.LengthChar == 1 && tt.Chars.IsAllLower) && tt.Next != null && tt.Next.IsChar('.')) && tt.Kit.BaseLanguage.IsRu) 
            {
                if (tt.IsValue("М", null) || tt.IsValue("M", null)) 
                {
                    if (prev != null && prev.Typ == StreetItemType.Noun) 
                    {
                    }
                    else 
                        return new StreetItemToken(tt, tt.Next) { Termin = m_Metro, Typ = StreetItemType.Noun, IsAbridge = true };
                }
            }
            Pullenti.Ner.Core.IntOntologyToken ot = null;
            if (locStreets != null) 
            {
                List<Pullenti.Ner.Core.IntOntologyToken> ots = locStreets.TryAttach(t, null, false);
                if (ots != null) 
                    ot = ots[0];
            }
            if (t.Kit.Ontology != null && ot == null) 
            {
                List<Pullenti.Ner.Core.IntOntologyToken> ots = t.Kit.Ontology.AttachToken(Pullenti.Ner.Address.AddressReferent.OBJ_TYPENAME, t);
                if (ots != null) 
                    ot = ots[0];
            }
            if (ot != null && ot.BeginToken == ot.EndToken && ot.Morph.Class.IsAdjective) 
            {
                Pullenti.Ner.Core.TerminToken tok0 = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok0 != null) 
                {
                    if (((StreetItemType)tok0.Termin.Tag) == StreetItemType.StdAdjective) 
                        ot = null;
                }
            }
            if (ot != null) 
            {
                StreetItemToken res0 = new StreetItemToken(ot.BeginToken, ot.EndToken) { Typ = StreetItemType.Name, ExistStreet = ot.Item.Referent as Pullenti.Ner.Address.StreetReferent, Morph = ot.Morph, IsInDictionary = true };
                return res0;
            }
            Pullenti.Ner.Core.TerminToken tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null && tok.Termin.CanonicText == "НАБЕРЕЖНАЯ" && !tok.Chars.IsAllLower) 
            {
                StreetItemToken nex = TryParse(tok.EndToken.Next, null, false, null, false);
                if (nex != null && ((nex.Typ == StreetItemType.Noun || nex.Typ == StreetItemType.StdAdjective))) 
                    tok = null;
                else if (((t.Morph.Gender & Pullenti.Morph.MorphGender.Feminie)) == Pullenti.Morph.MorphGender.Undefined && t.LengthChar > 7) 
                    tok = null;
            }
            if (((tok != null && t.LengthChar == 1 && t.IsValue("Б", null)) && prev != null && prev.Number != null) && prev.Number.Value == "26") 
                tok = null;
            if (tok != null && !ignoreOnto) 
            {
                if (((StreetItemType)tok.Termin.Tag) == StreetItemType.Number) 
                {
                    if ((tok.EndToken.Next is Pullenti.Ner.NumberToken) && (tok.EndToken.Next as Pullenti.Ner.NumberToken).IntValue != null) 
                        return new StreetItemToken(t, tok.EndToken.Next) { Typ = StreetItemType.Number, Number = tok.EndToken.Next as Pullenti.Ner.NumberToken, NumberHasPrefix = true, Morph = tok.Morph };
                    return null;
                }
                if (tt == null) 
                    return null;
                bool abr = true;
                switch ((StreetItemType)tok.Termin.Tag) { 
                case StreetItemType.StdAdjective:
                    if (tt.Chars.IsAllLower && prev == null) 
                        return null;
                    else if (tt.IsValue(tok.Termin.CanonicText, null)) 
                        abr = false;
                    else if (tt.LengthChar == 1) 
                    {
                        if (!tt.IsWhitespaceBefore && !tt.Previous.IsCharOf(":,.")) 
                            break;
                        if (!tok.EndToken.IsChar('.')) 
                        {
                            if (!tt.Chars.IsAllUpper) 
                                break;
                            bool oo2 = false;
                            if (tok.EndToken.IsNewlineAfter && prev != null) 
                                oo2 = true;
                            else 
                            {
                                StreetItemToken next = TryParse(tok.EndToken.Next, null, false, null, false);
                                if (next != null && ((next.Typ == StreetItemType.Name || next.Typ == StreetItemType.Noun))) 
                                    oo2 = true;
                                else if (AddressItemToken.CheckHouseAfter(tok.EndToken.Next, false, true) && prev != null) 
                                    oo2 = true;
                            }
                            if (oo2) 
                                return new StreetItemToken(tok.BeginToken, tok.EndToken) { Typ = StreetItemType.StdAdjective, Termin = tok.Termin, IsAbridge = abr, Morph = tok.Morph };
                            break;
                        }
                        Pullenti.Ner.Token tt2 = tok.EndToken.Next;
                        if (tt2 != null && tt2.IsHiphen) 
                            tt2 = tt2.Next;
                        if (tt2 is Pullenti.Ner.TextToken) 
                        {
                            if (tt2.LengthChar == 1 && tt2.Chars.IsAllUpper) 
                                break;
                            if (tt2.Chars.IsCapitalUpper) 
                            {
                                bool isSur = false;
                                string txt = (tt2 as Pullenti.Ner.TextToken).Term;
                                if (txt.EndsWith("ОГО")) 
                                    isSur = true;
                                else 
                                    foreach (Pullenti.Morph.MorphBaseInfo wf in tt2.Morph.Items) 
                                    {
                                        if (wf.Class.IsProperSurname && (wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                                        {
                                            if (wf.Case.IsGenitive) 
                                            {
                                                isSur = true;
                                                break;
                                            }
                                        }
                                    }
                                if (isSur) 
                                    break;
                            }
                        }
                    }
                    return new StreetItemToken(tok.BeginToken, tok.EndToken) { Typ = StreetItemType.StdAdjective, Termin = tok.Termin, IsAbridge = abr, Morph = tok.Morph };
                case StreetItemType.Noun:
                    if (tt.IsValue(tok.Termin.CanonicText, null) || tok.EndToken.IsValue(tok.Termin.CanonicText, null) || tt.IsValue("УЛ", null)) 
                        abr = false;
                    else if (tok.BeginToken != tok.EndToken && ((tok.BeginToken.Next.IsHiphen || tok.BeginToken.Next.IsCharOf("/\\")))) 
                    {
                    }
                    else if (!tt.Chars.IsAllLower && tt.LengthChar == 1) 
                        break;
                    else if (tt.LengthChar == 1) 
                    {
                        if (!tt.IsWhitespaceBefore) 
                        {
                            if (tt.Previous != null && tt.Previous.IsCharOf(",")) 
                            {
                            }
                            else 
                                return null;
                        }
                        if (tok.EndToken.IsChar('.')) 
                        {
                        }
                        else if (tok.BeginToken != tok.EndToken && tok.BeginToken.Next != null && ((tok.BeginToken.Next.IsHiphen || tok.BeginToken.Next.IsCharOf("/\\")))) 
                        {
                        }
                        else if (tok.LengthChar > 5) 
                        {
                        }
                        else if (tok.BeginToken == tok.EndToken && tt.IsValue("Ш", null) && tt.Chars.IsAllLower) 
                        {
                            if (prev != null && ((prev.Typ == StreetItemType.Name || prev.Typ == StreetItemType.StdName || prev.Typ == StreetItemType.StdPartOfName))) 
                            {
                            }
                            else 
                            {
                                StreetItemToken sii = TryParse(tt.Next, null, false, null, false);
                                if (sii != null && (((sii.Typ == StreetItemType.Name || sii.Typ == StreetItemType.StdName || sii.Typ == StreetItemType.StdPartOfName) || sii.Typ == StreetItemType.Age))) 
                                {
                                }
                                else 
                                    return null;
                            }
                        }
                        else 
                            return null;
                    }
                    else if (((tt.Term == "КВ" || tt.Term == "КВАРТ")) && !tok.EndToken.IsValue("Л", null)) 
                    {
                    }
                    if (!t.Chars.IsAllLower && t.Morph.Class.IsProperSurname && t.Chars.IsCyrillicLetter) 
                    {
                        if (((t.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) != Pullenti.Morph.MorphNumber.Undefined) 
                            return null;
                    }
                    if (tt.Term == "ДОРОГОЙ") 
                        return null;
                    Pullenti.Ner.Core.Termin alt = null;
                    if (tok.BeginToken.IsValue("ПР", null) && ((tok.BeginToken == tok.EndToken || tok.BeginToken.Next.IsChar('.')))) 
                        alt = m_Prospect;
                    return new StreetItemToken(tok.BeginToken, tok.EndToken) { Typ = StreetItemType.Noun, Termin = tok.Termin, AltTermin = alt, IsAbridge = abr, Morph = tok.Morph, NounIsDoubtCoef = (tok.Termin.Tag2 is int ? (int)tok.Termin.Tag2 : 0) };
                case StreetItemType.StdName:
                    bool isPostOff = tok.Termin.CanonicText == "ПОЧТОВОЕ ОТДЕЛЕНИЕ";
                    if (tok.BeginToken.Chars.IsAllLower && !isPostOff && tok.EndToken.Chars.IsAllLower) 
                        return null;
                    StreetItemToken sits = new StreetItemToken(tok.BeginToken, tok.EndToken) { Typ = StreetItemType.StdName, Morph = tok.Morph, Value = tok.Termin.CanonicText };
                    if (tok.BeginToken != tok.EndToken && !isPostOff) 
                    {
                        string vv = Pullenti.Ner.Core.MiscHelper.GetTextValue(tok.BeginToken, tok.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                        if (vv != sits.Value) 
                        {
                            if (vv.Length < sits.Value.Length) 
                                sits.AltValue = vv;
                            else 
                            {
                                sits.AltValue = sits.Value;
                                sits.Value = vv;
                            }
                        }
                        if (((m_StdOntMisc.TryParse(tok.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No) != null || tok.BeginToken.GetMorphClassInDictionary().IsProperName || (tok.BeginToken.LengthChar < 4))) && ((tok.EndToken.Morph.Class.IsProperSurname || !tok.EndToken.GetMorphClassInDictionary().IsProperName))) 
                            sits.AltValue2 = Pullenti.Ner.Core.MiscHelper.GetTextValue(tok.EndToken, tok.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                        else if (((tok.EndToken.GetMorphClassInDictionary().IsProperName || m_StdOntMisc.TryParse(tok.EndToken, Pullenti.Ner.Core.TerminParseAttr.No) != null)) && ((tok.BeginToken.Morph.Class.IsProperSurname || !tok.BeginToken.GetMorphClassInDictionary().IsProperName))) 
                            sits.AltValue2 = Pullenti.Ner.Core.MiscHelper.GetTextValue(tok.BeginToken, tok.BeginToken, Pullenti.Ner.Core.GetTextAttr.No);
                    }
                    return sits;
                case StreetItemType.StdPartOfName:
                    if (prev != null && prev.Typ == StreetItemType.Name) 
                    {
                        string nam = prev.Value ?? Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(prev, Pullenti.Ner.Core.GetTextAttr.No);
                        if (prev.AltValue == null) 
                            prev.AltValue = string.Format("{0} {1}", tok.Termin.CanonicText, nam);
                        else 
                            prev.AltValue = string.Format("{0} {1}", tok.Termin.CanonicText, prev.AltValue);
                        prev.EndToken = tok.EndToken;
                        prev.Value = nam;
                        return TryParse(tok.EndToken.Next, locStreets, recurse, prev, false);
                    }
                    StreetItemToken sit = TryParse(tok.EndToken.Next, locStreets, false, null, false);
                    if (sit == null) 
                    {
                        if (tok.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                            return new StreetItemToken(tok.BeginToken, tok.EndToken) { Typ = StreetItemType.Name, Morph = tok.Morph, Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(tok, Pullenti.Ner.Core.GetTextAttr.No) };
                        return null;
                    }
                    if (sit.Typ != StreetItemType.Name && sit.Typ != StreetItemType.Noun) 
                        return null;
                    if (sit.Typ == StreetItemType.Noun) 
                    {
                        if (tok.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                            return new StreetItemToken(tok.BeginToken, tok.EndToken) { Typ = StreetItemType.Name, Morph = tok.Morph, Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(tok, Pullenti.Ner.Core.GetTextAttr.No) };
                        else 
                            return new StreetItemToken(tok.BeginToken, tok.EndToken) { Typ = StreetItemType.Name, Morph = tok.Morph, Termin = tok.Termin };
                    }
                    if (sit.Value != null) 
                    {
                        if (sit.AltValue == null) 
                            sit.AltValue = string.Format("{0} {1}", tok.Termin.CanonicText, sit.Value);
                        else 
                            sit.Value = string.Format("{0} {1}", tok.Termin.CanonicText, sit.Value);
                    }
                    else if (sit.ExistStreet == null) 
                    {
                        sit.AltValue = (sit.BeginToken as Pullenti.Ner.TextToken).Term;
                        sit.Value = string.Format("{0} {1}", tok.Termin.CanonicText, (sit.BeginToken as Pullenti.Ner.TextToken).Term);
                    }
                    sit.BeginToken = tok.BeginToken;
                    return sit;
                case StreetItemType.Name:
                    if (tok.BeginToken.Chars.IsAllLower) 
                    {
                        if (prev != null && prev.Typ == StreetItemType.StdAdjective) 
                        {
                        }
                        else if (prev != null && prev.Typ == StreetItemType.Noun && AddressItemToken.CheckHouseAfter(tok.EndToken.Next, true, false)) 
                        {
                        }
                        else if (t.IsValue("ПРОЕКТИРУЕМЫЙ", null) || t.IsValue("МИРА", null)) 
                        {
                        }
                        else 
                        {
                            StreetItemToken nex = TryParse(tok.EndToken.Next, null, true, null, false);
                            if (nex != null && nex.Typ == StreetItemType.Noun) 
                            {
                                Pullenti.Ner.Token tt2 = nex.EndToken.Next;
                                while (tt2 != null && tt2.IsCharOf(",.")) 
                                {
                                    tt2 = tt2.Next;
                                }
                                if (tt2 == null || tt2.WhitespacesBeforeCount > 1) 
                                    return null;
                                if (AddressItemToken.CheckHouseAfter(tt2, false, true)) 
                                {
                                }
                                else 
                                    return null;
                            }
                            else 
                                return null;
                        }
                    }
                    StreetItemToken sit0 = TryParse(tok.BeginToken, null, false, prev, true);
                    if (sit0 != null && sit0.Typ == StreetItemType.Name && sit0.EndChar > tok.EndChar) 
                    {
                        sit0.IsInDictionary = true;
                        return sit0;
                    }
                    StreetItemToken sit1 = new StreetItemToken(tok.BeginToken, tok.EndToken) { Typ = StreetItemType.Name, Morph = tok.Morph, IsInDictionary = true };
                    if ((!tok.IsWhitespaceAfter && tok.EndToken.Next != null && tok.EndToken.Next.IsHiphen) && !tok.EndToken.Next.IsWhitespaceAfter) 
                    {
                        StreetItemToken sit2 = TryParse(tok.EndToken.Next.Next, locStreets, false, null, false);
                        if (sit2 != null && ((sit2.Typ == StreetItemType.Name || sit2.Typ == StreetItemType.StdPartOfName || sit2.Typ == StreetItemType.StdName))) 
                            sit1.EndToken = sit2.EndToken;
                    }
                    return sit1;
                case StreetItemType.Fix:
                    return new StreetItemToken(tok.BeginToken, tok.EndToken) { Typ = StreetItemType.Fix, Morph = tok.Morph, IsInDictionary = true, Termin = tok.Termin };
                }
            }
            if (tt != null) 
            {
                if ((prev != null && prev.Typ == StreetItemType.Number && prev.Number != null) && prev.Number.IntValue.Value == 26) 
                {
                    if (tt.IsValue("БАКИНСКИЙ", null) || "БАКИНСК".StartsWith((tt as Pullenti.Ner.TextToken).Term)) 
                    {
                        Pullenti.Ner.Token tt2 = (Pullenti.Ner.Token)tt;
                        if (tt2.Next != null && tt2.Next.IsChar('.')) 
                            tt2 = tt2.Next;
                        if (tt2.Next is Pullenti.Ner.TextToken) 
                        {
                            tt2 = tt2.Next;
                            if (tt2.IsValue("КОМИССАР", null) || tt2.IsValue("КОММИССАР", null) || "КОМИС".StartsWith((tt2 as Pullenti.Ner.TextToken).Term)) 
                            {
                                if (tt2.Next != null && tt2.Next.IsChar('.')) 
                                    tt2 = tt2.Next;
                                StreetItemToken sit = new StreetItemToken(tt, tt2) { Typ = StreetItemType.StdName, IsInDictionary = true, Value = "БАКИНСКИХ КОМИССАРОВ", Morph = tt2.Morph };
                                return sit;
                            }
                        }
                    }
                }
                if ((tt.Next != null && tt.Next.IsChar('.') && !tt.Chars.IsAllLower) && (tt.Next.WhitespacesAfterCount < 3) && (tt.Next.Next is Pullenti.Ner.TextToken)) 
                {
                    Pullenti.Ner.Token tt1 = tt.Next.Next;
                    if (tt1 != null && tt1.IsHiphen) 
                        tt1 = tt1.Next;
                    if (tt.LengthChar == 1 && tt1.LengthChar == 1 && (tt1.Next is Pullenti.Ner.TextToken)) 
                    {
                        if (tt1.IsAnd && tt1.Next.Chars.IsAllUpper && tt1.Next.LengthChar == 1) 
                            tt1 = tt1.Next;
                        if ((tt1.Chars.IsAllUpper && tt1.Next.IsChar('.') && (tt1.Next.WhitespacesAfterCount < 3)) && (tt1.Next.Next is Pullenti.Ner.TextToken)) 
                            tt1 = tt1.Next.Next;
                    }
                    StreetItemToken sit = StreetItemToken.TryParse(tt1, locStreets, false, null, false);
                    if (sit != null && (tt1 is Pullenti.Ner.TextToken)) 
                    {
                        string str = (tt1 as Pullenti.Ner.TextToken).Term;
                        bool ok = false;
                        Pullenti.Morph.MorphClass cla = tt.Next.Next.GetMorphClassInDictionary();
                        if (sit.IsInDictionary) 
                            ok = true;
                        else if (sit._isSurname() || cla.IsProperSurname) 
                            ok = true;
                        else if (Pullenti.Morph.LanguageHelper.EndsWith(str, "ОЙ") && ((cla.IsProperSurname || ((sit.Typ == StreetItemType.Name && sit.IsInDictionary))))) 
                            ok = true;
                        else if (Pullenti.Morph.LanguageHelper.EndsWithEx(str, "ГО", "ИХ", null, null)) 
                            ok = true;
                        else if (tt1.IsWhitespaceBefore && !tt1.GetMorphClassInDictionary().IsUndefined) 
                        {
                        }
                        else if (prev != null && prev.Typ == StreetItemType.Noun && ((!prev.IsAbridge || prev.LengthChar > 2))) 
                            ok = true;
                        else if ((prev != null && prev.Typ == StreetItemType.Name && sit.Typ == StreetItemType.Noun) && AddressItemToken.CheckHouseAfter(sit.EndToken.Next, false, true)) 
                            ok = true;
                        else if (sit.Typ == StreetItemType.Name && AddressItemToken.CheckHouseAfter(sit.EndToken.Next, false, true)) 
                        {
                            if (Pullenti.Ner.Geo.Internal.MiscLocationHelper.CheckGeoObjectBefore(tt)) 
                                ok = true;
                        }
                        if (ok) 
                        {
                            sit.BeginToken = tt;
                            sit.Value = str;
                            sit.AltValue = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt, sit.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                            if (sit.AltValue != null) 
                                sit.AltValue = sit.AltValue.Replace("-", "");
                            return sit;
                        }
                    }
                }
                if (tt.Chars.IsCyrillicLetter && tt.LengthChar > 1 && !tt.Morph.Class.IsPreposition) 
                {
                    if (tt.IsValue("ГЕРОЙ", null) || tt.IsValue("ЗАЩИТНИК", "ЗАХИСНИК")) 
                    {
                        if ((tt.Next is Pullenti.Ner.ReferentToken) && (tt.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                        {
                            StreetItemToken re = new StreetItemToken(tt, tt.Next) { Typ = StreetItemType.StdPartOfName, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt, tt.Next, Pullenti.Ner.Core.GetTextAttr.No) };
                            StreetItemToken sit = TryParse(tt.Next.Next, locStreets, false, null, false);
                            if (sit == null || sit.Typ != StreetItemType.Name) 
                            {
                                bool ok2 = false;
                                if (sit != null && ((sit.Typ == StreetItemType.StdAdjective || sit.Typ == StreetItemType.Noun))) 
                                    ok2 = true;
                                else if (AddressItemToken.CheckHouseAfter(tt.Next.Next, false, true)) 
                                    ok2 = true;
                                else if (tt.Next.IsNewlineAfter) 
                                    ok2 = true;
                                if (ok2) 
                                {
                                    sit = new StreetItemToken(tt, tt.Next) { Typ = StreetItemType.Name };
                                    sit.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt, tt.Next, Pullenti.Ner.Core.GetTextAttr.No);
                                    return sit;
                                }
                                return re;
                            }
                            if (sit.Value == null) 
                                sit.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(sit, Pullenti.Ner.Core.GetTextAttr.No);
                            if (sit.AltValue == null) 
                            {
                                sit.AltValue = sit.Value;
                                sit.Value = string.Format("{0} {1}", re.Value, sit.Value);
                            }
                            else 
                                sit.Value = string.Format("{0} {1}", re.Value, sit.Value);
                            sit.BeginToken = tt;
                            return sit;
                        }
                    }
                    Pullenti.Ner.NumberToken ani = Pullenti.Ner.Core.NumberHelper.TryParseAnniversary(t);
                    if (ani != null) 
                        return new StreetItemToken(t, ani.EndToken) { Typ = StreetItemType.Age, Number = ani, Value = ani.Value.ToString() };
                    bool ok1 = false;
                    if (!tt.Chars.IsAllLower) 
                    {
                        AddressItemToken ait = AddressItemToken.TryParse(tt, null, false, true, null);
                        if (ait != null) 
                        {
                        }
                        else 
                            ok1 = true;
                    }
                    else if (prev != null && prev.Typ == StreetItemType.Noun) 
                    {
                        Pullenti.Ner.Token tt1 = prev.BeginToken.Previous;
                        if (tt1 != null && tt1.IsComma) 
                            tt1 = tt1.Previous;
                        if (tt1 != null && (tt1.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                            ok1 = true;
                        else if (AddressItemToken.CheckHouseAfter(tt.Next, false, false)) 
                        {
                            if (!AddressItemToken.CheckHouseAfter(tt, false, false)) 
                                ok1 = true;
                        }
                    }
                    else if (tt.WhitespacesAfterCount < 2) 
                    {
                        StreetItemToken nex = TryParse(tt.Next, null, true, null, false);
                        if (nex != null && nex.Typ == StreetItemType.Noun) 
                        {
                            if (nex.Termin.CanonicText == "ПЛОЩАДЬ") 
                            {
                                if (tt.IsValue("ОБЩИЙ", null)) 
                                    return null;
                            }
                            Pullenti.Ner.Token tt1 = tt.Previous;
                            if (tt1 != null && tt1.IsComma) 
                                tt1 = tt1.Previous;
                            if (tt1 != null && (tt1.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                                ok1 = true;
                            else if (AddressItemToken.CheckHouseAfter(nex.EndToken.Next, false, false)) 
                                ok1 = true;
                        }
                    }
                    if (ok1) 
                    {
                        Pullenti.Morph.MorphClass dc = tt.GetMorphClassInDictionary();
                        if (dc.IsAdverb) 
                        {
                            if (!(dc.IsProper)) 
                                return null;
                        }
                        StreetItemToken res = new StreetItemToken(tt, tt) { Typ = StreetItemType.Name, Morph = tt.Morph };
                        if ((tt.Next != null && ((tt.Next.IsHiphen || tt.Next.IsCharOf("\\/"))) && (tt.Next.Next is Pullenti.Ner.TextToken)) && !tt.IsWhitespaceAfter && !tt.Next.IsWhitespaceAfter) 
                        {
                            bool ok2 = AddressItemToken.CheckHouseAfter(tt.Next.Next.Next, false, false) || tt.Next.Next.IsNewlineAfter;
                            if (!ok2) 
                            {
                                StreetItemToken te2 = TryParse(tt.Next.Next.Next, null, false, null, false);
                                if (te2 != null && te2.Typ == StreetItemType.Noun) 
                                    ok2 = true;
                            }
                            if (ok2) 
                            {
                                res.EndToken = tt.Next.Next;
                                res.Value = string.Format("{0} {1}", Pullenti.Ner.Core.MiscHelper.GetTextValue(res.BeginToken, res.BeginToken, Pullenti.Ner.Core.GetTextAttr.No), Pullenti.Ner.Core.MiscHelper.GetTextValue(res.EndToken, res.EndToken, Pullenti.Ner.Core.GetTextAttr.No));
                            }
                        }
                        else if ((tt.WhitespacesAfterCount < 2) && (tt.Next is Pullenti.Ner.TextToken) && tt.Next.Chars.IsLetter) 
                        {
                            if (!AddressItemToken.CheckHouseAfter(tt.Next, false, false) || tt.Next.IsNewlineAfter) 
                            {
                                Pullenti.Ner.Token tt1 = tt.Next;
                                bool isPref = false;
                                if ((tt1 is Pullenti.Ner.TextToken) && tt1.Chars.IsAllLower) 
                                {
                                    if (tt1.IsValue("ДЕ", null) || tt1.IsValue("ЛА", null)) 
                                    {
                                        tt1 = tt1.Next;
                                        isPref = true;
                                    }
                                }
                                StreetItemToken nn = TryParse(tt1, locStreets, false, null, false);
                                if (nn == null || nn.Typ == StreetItemType.Name) 
                                {
                                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                    if (npt != null) 
                                    {
                                        if (npt.BeginToken == npt.EndToken) 
                                            npt = null;
                                        else if (m_Ontology.TryParse(npt.EndToken, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                                            npt = null;
                                    }
                                    if (npt != null && ((npt.IsNewlineAfter || AddressItemToken.CheckHouseAfter(npt.EndToken.Next, false, false)))) 
                                    {
                                        res.EndToken = npt.EndToken;
                                        if (npt.Morph.Case.IsGenitive) 
                                        {
                                            res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(npt, Pullenti.Ner.Core.GetTextAttr.No);
                                            res.AltValue = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                                        }
                                        else 
                                        {
                                            res.Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                                            res.AltValue = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(npt, Pullenti.Ner.Core.GetTextAttr.No);
                                        }
                                    }
                                    else if (AddressItemToken.CheckHouseAfter(tt1.Next, false, false) && tt1.Chars.IsCyrillicLetter == tt.Chars.IsCyrillicLetter && (t.WhitespacesAfterCount < 2)) 
                                    {
                                        if (tt1.Morph.Class.IsVerb && !tt1.IsValue("ДАЛИ", null)) 
                                        {
                                        }
                                        else if (npt == null && !tt1.Chars.IsAllLower && !isPref) 
                                        {
                                        }
                                        else 
                                        {
                                            res.EndToken = tt1;
                                            res.Value = string.Format("{0} {1}", Pullenti.Ner.Core.MiscHelper.GetTextValue(res.BeginToken, res.BeginToken, Pullenti.Ner.Core.GetTextAttr.No), Pullenti.Ner.Core.MiscHelper.GetTextValue(res.EndToken, res.EndToken, Pullenti.Ner.Core.GetTextAttr.No));
                                        }
                                    }
                                }
                                else if (nn.Typ == StreetItemType.Noun) 
                                {
                                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                    if (npt != null && npt.EndToken == nn.EndToken) 
                                    {
                                        res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(res.BeginToken, res.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                                        string var = Pullenti.Morph.MorphologyService.GetWordform(res.Value, new Pullenti.Morph.MorphBaseInfo() { Case = Pullenti.Morph.MorphCase.Nominative, Class = Pullenti.Morph.MorphClass.Adjective, Number = Pullenti.Morph.MorphNumber.Singular, Gender = npt.Morph.Gender });
                                        if (var != null && var != res.Value) 
                                        {
                                            res.AltValue = res.Value;
                                            res.Value = var;
                                        }
                                    }
                                }
                            }
                        }
                        return res;
                    }
                }
                if (((tt.IsValue("РЕКА", null) || tt.IsValue("РІЧКА", null))) && tt.Next != null && tt.Next.Chars.IsCapitalUpper) 
                    return new StreetItemToken(tt, tt.Next) { Typ = StreetItemType.Name, Morph = tt.Morph, AltValue = tt.Next.GetSourceText().ToUpper() };
                if (tt.IsValue("№", null) || tt.IsValue("НОМЕР", null) || tt.IsValue("НОМ", null)) 
                {
                    Pullenti.Ner.Token tt1 = tt.Next;
                    if (tt1 != null && tt1.IsChar('.')) 
                        tt1 = tt1.Next;
                    if ((tt1 is Pullenti.Ner.NumberToken) && (tt1 as Pullenti.Ner.NumberToken).IntValue != null) 
                        return new StreetItemToken(tt, tt1) { Typ = StreetItemType.Number, Number = tt1 as Pullenti.Ner.NumberToken, NumberHasPrefix = true };
                }
                if (tt.IsHiphen && (tt.Next is Pullenti.Ner.NumberToken) && (tt.Next as Pullenti.Ner.NumberToken).IntValue != null) 
                {
                    if (prev != null && prev.Typ == StreetItemType.Noun) 
                    {
                        if (prev.Termin.CanonicText == "МИКРОРАЙОН" || Pullenti.Morph.LanguageHelper.EndsWith(prev.Termin.CanonicText, "ГОРОДОК")) 
                            return new StreetItemToken(tt, tt.Next) { Typ = StreetItemType.Number, Number = tt.Next as Pullenti.Ner.NumberToken, NumberHasPrefix = true };
                    }
                }
            }
            Pullenti.Ner.Referent r = (t == null ? null : t.GetReferent());
            if (r is Pullenti.Ner.Geo.GeoReferent) 
            {
                Pullenti.Ner.Geo.GeoReferent geo = r as Pullenti.Ner.Geo.GeoReferent;
                if (prev != null && prev.Typ == StreetItemType.Noun) 
                {
                    if (AddressItemToken.CheckHouseAfter(t.Next, false, false)) 
                        return new StreetItemToken(t, t) { Typ = StreetItemType.Name, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t, Pullenti.Ner.Core.GetTextAttr.No) };
                }
            }
            if (((tt is Pullenti.Ner.TextToken) && tt.Chars.IsCapitalUpper && tt.Chars.IsLatinLetter) && (tt.WhitespacesAfterCount < 2)) 
            {
                if (Pullenti.Ner.Core.MiscHelper.IsEngArticle(tt)) 
                    return null;
                Pullenti.Ner.Token tt2 = tt.Next;
                if (Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(tt2)) 
                    tt2 = tt2.Next.Next;
                Pullenti.Ner.Core.TerminToken tok1 = m_Ontology.TryParse(tt2, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok1 != null) 
                    return new StreetItemToken(tt, tt2.Previous) { Typ = StreetItemType.Name, Morph = tt.Morph, Value = (tt as Pullenti.Ner.TextToken).Term };
            }
            return null;
        }
        internal static List<StreetItemToken> TryParseSpec(Pullenti.Ner.Token t, StreetItemToken prev)
        {
            if (t == null) 
                return null;
            List<StreetItemToken> res = null;
            StreetItemToken sit;
            if (t.GetReferent() is Pullenti.Ner.Date.DateReferent) 
            {
                Pullenti.Ner.Date.DateReferent dr = t.GetReferent() as Pullenti.Ner.Date.DateReferent;
                if (!((t as Pullenti.Ner.ReferentToken).BeginToken is Pullenti.Ner.NumberToken)) 
                    return null;
                if (dr.Year == 0 && dr.Day > 0 && dr.Month > 0) 
                {
                    res = new List<StreetItemToken>();
                    res.Add(new StreetItemToken(t, t) { Typ = StreetItemType.Number, Number = new Pullenti.Ner.NumberToken(t, t, dr.Day.ToString(), Pullenti.Ner.NumberSpellingType.Digit) });
                    string tmp = dr.ToString(false, t.Morph.Language, 0);
                    int i = tmp.IndexOf(' ');
                    res.Add((sit = new StreetItemToken(t, t) { Typ = StreetItemType.StdName, Value = tmp.Substring(i + 1).ToUpper() }));
                    sit.Chars.IsCapitalUpper = true;
                    return res;
                }
                if (dr.Year > 0 && dr.Month == 0) 
                {
                    res = new List<StreetItemToken>();
                    res.Add(new StreetItemToken(t, t) { Typ = StreetItemType.Number, Number = new Pullenti.Ner.NumberToken(t, t, dr.Year.ToString(), Pullenti.Ner.NumberSpellingType.Digit) });
                    res.Add((sit = new StreetItemToken(t, t) { Typ = StreetItemType.StdName, Value = (t.Morph.Language.IsUa ? "РОКУ" : "ГОДА") }));
                    sit.Chars.IsCapitalUpper = true;
                    return res;
                }
                return null;
            }
            if (prev != null && prev.Typ == StreetItemType.Age) 
            {
                res = new List<StreetItemToken>();
                if (t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                    res.Add((sit = new StreetItemToken(t, t) { Typ = StreetItemType.Name, Value = t.GetSourceText().ToUpper(), AltValue = t.GetReferent().ToString(true, t.Kit.BaseLanguage, 0).ToUpper() }));
                else if (t.IsValue("ГОРОД", null) || t.IsValue("МІСТО", null)) 
                    res.Add((sit = new StreetItemToken(t, t) { Typ = StreetItemType.Name, Value = "ГОРОДА" }));
                else 
                    return null;
                return res;
            }
            if (prev != null && prev.Typ == StreetItemType.Noun) 
            {
                Pullenti.Ner.NumberToken num = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t);
                if (num != null && num.IntValue != null) 
                {
                    res = new List<StreetItemToken>();
                    res.Add((sit = new StreetItemToken(num.BeginToken, num.EndToken) { Typ = StreetItemType.Number, Number = num }));
                    t = num.EndToken.Next;
                    if ((num.Typ == Pullenti.Ner.NumberSpellingType.Digit && (t is Pullenti.Ner.TextToken) && !t.IsWhitespaceBefore) && t.LengthChar == 1) 
                    {
                        sit.EndToken = t;
                        sit.Value = string.Format("{0}{1}", num.Value, (t as Pullenti.Ner.TextToken).Term);
                        sit.Number = null;
                    }
                    return res;
                }
            }
            return null;
        }
        public static List<StreetItemToken> TryParseList(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locStreets, int maxCount = 10)
        {
            if (t == null) 
                return null;
            if (t.Kit.IsRecurceOverflow) 
                return null;
            t.Kit.RecurseLevel++;
            List<StreetItemToken> res = _tryParseList(t, locStreets, maxCount);
            t.Kit.RecurseLevel--;
            return res;
        }
        static List<StreetItemToken> _tryParseList(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locStreets, int maxCount)
        {
            List<StreetItemToken> res = null;
            StreetItemToken sit = TryParse(t, locStreets, false, null, false);
            if (sit != null) 
            {
                res = new List<StreetItemToken>();
                res.Add(sit);
                t = sit.EndToken.Next;
            }
            else 
            {
                res = TryParseSpec(t, null);
                if (res == null) 
                    return null;
                sit = res[res.Count - 1];
                t = sit.EndToken.Next;
                StreetItemToken sit2 = TryParse(t, locStreets, false, null, false);
                if (sit2 != null && sit2.Typ == StreetItemType.Noun) 
                {
                }
                else if (AddressItemToken.CheckHouseAfter(t, false, true)) 
                {
                }
                else 
                    return null;
            }
            for (; t != null; t = t.Next) 
            {
                if (maxCount > 0 && res.Count >= maxCount) 
                    break;
                if (t.IsNewlineBefore) 
                {
                    if (t.NewlinesBeforeCount > 1) 
                        break;
                    if (((t.WhitespacesAfterCount < 15) && sit != null && sit.Typ == StreetItemType.Noun) && t.Chars.IsCapitalUpper) 
                    {
                    }
                    else 
                        break;
                }
                if (t.IsHiphen && sit != null && ((sit.Typ == StreetItemType.Name || ((sit.Typ == StreetItemType.StdAdjective && !sit.IsAbridge))))) 
                {
                    StreetItemToken sit1 = TryParse(t.Next, locStreets, false, sit, false);
                    if (sit1 == null) 
                        break;
                    if (sit1.Typ == StreetItemType.Number) 
                    {
                        Pullenti.Ner.Token tt = sit1.EndToken.Next;
                        if (tt != null && tt.IsComma) 
                            tt = tt.Next;
                        bool ok = false;
                        AddressItemToken ait = AddressItemToken.TryParse(tt, locStreets, false, true, null);
                        if (ait != null) 
                        {
                            if (ait.Typ == AddressItemToken.ItemType.House) 
                                ok = true;
                        }
                        if (!ok) 
                        {
                            if (res.Count == 2 && res[0].Typ == StreetItemType.Noun) 
                            {
                                if (res[0].Termin.CanonicText == "МИКРОРАЙОН") 
                                    ok = true;
                            }
                        }
                        if (ok) 
                        {
                            sit = sit1;
                            res.Add(sit);
                            t = sit.EndToken;
                            sit.NumberHasPrefix = true;
                            continue;
                        }
                    }
                    if (sit1.Typ != StreetItemType.Name && sit1.Typ != StreetItemType.Name) 
                        break;
                    if (t.IsWhitespaceBefore && t.IsWhitespaceAfter) 
                        break;
                    if (res[0].BeginToken.Previous != null) 
                    {
                        AddressItemToken aaa = AddressItemToken.TryParse(res[0].BeginToken.Previous, null, false, true, null);
                        if (aaa != null && aaa.Typ == AddressItemToken.ItemType.Detail && aaa.DetailType == Pullenti.Ner.Address.AddressDetailType.Cross) 
                            break;
                    }
                    sit = sit1;
                    res.Add(sit);
                    t = sit.EndToken;
                    continue;
                }
                else if (t.IsHiphen && sit != null && sit.Typ == StreetItemType.Number) 
                {
                    StreetItemToken sit1 = TryParse(t.Next, locStreets, false, null, false);
                    if (sit1 != null && ((sit1.Typ == StreetItemType.StdAdjective || sit1.Typ == StreetItemType.StdName || sit1.Typ == StreetItemType.Name))) 
                    {
                        sit.NumberHasPrefix = true;
                        sit = sit1;
                        res.Add(sit);
                        t = sit.EndToken;
                        continue;
                    }
                }
                if (t.IsChar('.') && sit != null && sit.Typ == StreetItemType.Noun) 
                {
                    if (t.WhitespacesAfterCount > 1) 
                        break;
                    sit = TryParse(t.Next, locStreets, false, null, false);
                    if (sit == null) 
                        break;
                    if (sit.Typ == StreetItemType.Number || sit.Typ == StreetItemType.StdAdjective) 
                    {
                        StreetItemToken sit1 = TryParse(sit.EndToken.Next, null, false, null, false);
                        if (sit1 != null && ((sit1.Typ == StreetItemType.StdAdjective || sit1.Typ == StreetItemType.StdName || sit1.Typ == StreetItemType.Name))) 
                        {
                        }
                        else 
                            break;
                    }
                    else if (sit.Typ != StreetItemType.Name && sit.Typ != StreetItemType.StdName && sit.Typ != StreetItemType.Age) 
                        break;
                    if (t.Previous.GetMorphClassInDictionary().IsNoun) 
                    {
                        if (!sit.IsInDictionary) 
                        {
                            Pullenti.Ner.Token tt = sit.EndToken.Next;
                            bool hasHouse = false;
                            for (; tt != null; tt = tt.Next) 
                            {
                                if (tt.IsNewlineBefore) 
                                    break;
                                if (tt.IsComma) 
                                    continue;
                                AddressItemToken ai = AddressItemToken.TryParse(tt, null, false, true, null);
                                if (ai != null && ((ai.Typ == AddressItemToken.ItemType.House || ai.Typ == AddressItemToken.ItemType.Building || ai.Typ == AddressItemToken.ItemType.Corpus))) 
                                {
                                    hasHouse = true;
                                    break;
                                }
                                StreetItemToken vv = TryParse(tt, null, false, null, false);
                                if (vv == null || vv.Typ == StreetItemType.Noun) 
                                    break;
                                tt = vv.EndToken;
                            }
                            if (!hasHouse) 
                                break;
                        }
                        if (t.Previous.Previous != null) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt11 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Previous.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt11 != null && npt11.EndToken == t.Previous) 
                                break;
                        }
                    }
                    res.Add(sit);
                }
                else 
                {
                    sit = TryParse(t, locStreets, false, res[res.Count - 1], false);
                    if (sit == null) 
                    {
                        List<StreetItemToken> spli = TryParseSpec(t, res[res.Count - 1]);
                        if (spli != null && spli.Count > 0) 
                        {
                            res.AddRange(spli);
                            t = spli[spli.Count - 1].EndToken;
                            continue;
                        }
                        if (((t is Pullenti.Ner.TextToken) && ((res.Count == 2 || res.Count == 3)) && res[0].Typ == StreetItemType.Noun) && res[1].Typ == StreetItemType.Number && ((((t as Pullenti.Ner.TextToken).Term == "ГОДА" || (t as Pullenti.Ner.TextToken).Term == "МАЯ" || (t as Pullenti.Ner.TextToken).Term == "МАРТА") || (t as Pullenti.Ner.TextToken).Term == "СЪЕЗДА"))) 
                        {
                            res.Add((sit = new StreetItemToken(t, t) { Typ = StreetItemType.StdName, Value = (t as Pullenti.Ner.TextToken).Term }));
                            continue;
                        }
                        sit = res[res.Count - 1];
                        if (t == null) 
                            break;
                        if (sit.Typ == StreetItemType.Noun && ((sit.Termin.CanonicText == "МИКРОРАЙОН" || sit.Termin.CanonicText == "МІКРОРАЙОН")) && (t.WhitespacesBeforeCount < 2)) 
                        {
                            Pullenti.Ner.Token tt1 = t;
                            if (tt1.IsHiphen && tt1.Next != null) 
                                tt1 = tt1.Next;
                            if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt1, true) && tt1.Next != null) 
                                tt1 = tt1.Next;
                            Pullenti.Ner.Token tt2 = tt1.Next;
                            bool br = false;
                            if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt2, true)) 
                            {
                                tt2 = tt2.Next;
                                br = true;
                            }
                            if (((tt1 is Pullenti.Ner.TextToken) && tt1.LengthChar == 1 && tt1.Chars.IsLetter) && ((AddressItemToken.CheckHouseAfter(tt2, false, true) || tt2 == null))) 
                            {
                                sit = new StreetItemToken(t, (br ? tt1.Next : tt1)) { Typ = StreetItemType.Name, Value = (tt1 as Pullenti.Ner.TextToken).Term };
                                char ch1 = AddressItemToken.CorrectChar(sit.Value[0]);
                                if (ch1 != 0 && ch1 != sit.Value[0]) 
                                    sit.AltValue = string.Format("{0}", ch1);
                                res.Add(sit);
                                break;
                            }
                        }
                        if (t.IsComma && (((sit.Typ == StreetItemType.Name || sit.Typ == StreetItemType.StdName || sit.Typ == StreetItemType.StdPartOfName) || sit.Typ == StreetItemType.StdAdjective || ((sit.Typ == StreetItemType.Number && res.Count > 1 && (((res[res.Count - 2].Typ == StreetItemType.Name || res[res.Count - 2].Typ == StreetItemType.StdName || res[res.Count - 2].Typ == StreetItemType.StdAdjective) || res[res.Count - 2].Typ == StreetItemType.StdPartOfName))))))) 
                        {
                            sit = TryParse(t.Next, null, false, null, false);
                            if (sit != null && sit.Typ == StreetItemType.Noun) 
                            {
                                Pullenti.Ner.Token ttt = sit.EndToken.Next;
                                if (ttt != null && ttt.IsComma) 
                                    ttt = ttt.Next;
                                AddressItemToken add = AddressItemToken.TryParse(ttt, null, false, true, null);
                                if (add != null && ((add.Typ == AddressItemToken.ItemType.House || add.Typ == AddressItemToken.ItemType.Corpus || add.Typ == AddressItemToken.ItemType.Building))) 
                                {
                                    res.Add(sit);
                                    t = sit.EndToken;
                                    continue;
                                }
                            }
                        }
                        if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                        {
                            StreetItemToken sit1 = res[res.Count - 1];
                            if (sit1.Typ == StreetItemType.Noun && ((sit1.NounIsDoubtCoef == 0 || (((t.Next is Pullenti.Ner.TextToken) && !t.Next.Chars.IsAllLower))))) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null && (br.LengthChar < 50)) 
                                {
                                    StreetItemToken sit2 = TryParse(t.Next, locStreets, false, null, false);
                                    if (sit2 != null && sit2.EndToken.Next == br.EndToken) 
                                    {
                                        if (sit2.Value == null && sit2.Typ == StreetItemType.Name) 
                                            sit2.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(sit2.BeginToken, sit2.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                                        sit2.BeginToken = t;
                                        sit2.IsInBrackets = true;
                                        t = (sit2.EndToken = br.EndToken);
                                        res.Add(sit2);
                                        continue;
                                    }
                                    res.Add(new StreetItemToken(t, br.EndToken) { Typ = StreetItemType.Name, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, br.EndToken, Pullenti.Ner.Core.GetTextAttr.No), IsInBrackets = true });
                                    t = br.EndToken;
                                    continue;
                                }
                            }
                        }
                        if (t.IsHiphen && (t.Next is Pullenti.Ner.NumberToken) && (t.Next as Pullenti.Ner.NumberToken).IntValue != null) 
                        {
                            sit = res[res.Count - 1];
                            if (sit.Typ == StreetItemType.Noun && (((sit.Termin.CanonicText == "КВАРТАЛ" || sit.Termin.CanonicText == "МИКРОРАЙОН" || sit.Termin.CanonicText == "ГОРОДОК") || sit.Termin.CanonicText == "МІКРОРАЙОН"))) 
                            {
                                sit = new StreetItemToken(t, t.Next) { Typ = StreetItemType.Number, Number = t.Next as Pullenti.Ner.NumberToken, NumberHasPrefix = true };
                                res.Add(sit);
                                t = t.Next;
                                continue;
                            }
                        }
                        break;
                    }
                    res.Add(sit);
                    if (sit.Typ == StreetItemType.Name) 
                    {
                        int cou = 0;
                        int jj;
                        for (jj = res.Count - 1; jj >= 0; jj--) 
                        {
                            if (sit.Typ == StreetItemType.Name) 
                                cou++;
                            else 
                                break;
                        }
                        if (cou > 4) 
                        {
                            if (jj < 0) 
                                return null;
                            res.RemoveRange(jj, res.Count - jj);
                            break;
                        }
                    }
                }
                t = sit.EndToken;
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (res[i].Typ == StreetItemType.Name && res[i + 1].Typ == StreetItemType.Name && (res[i].WhitespacesAfterCount < 3)) 
                {
                    bool isProp = false;
                    bool isPers = false;
                    if (res[i].BeginToken.Morph.Class.IsNoun) 
                    {
                        Pullenti.Ner.ReferentToken rt = res[i].Kit.ProcessReferent("PERSON", res[i].BeginToken);
                        if (rt != null) 
                        {
                            if (rt.Referent.TypeName == "PERSONPROPERTY") 
                                isProp = true;
                            else if (rt.EndToken == res[i + 1].EndToken) 
                                isPers = true;
                        }
                    }
                    if ((i == 0 && ((!isProp && !isPers)) && ((i + 2) < res.Count)) && res[i + 2].Typ == StreetItemType.Noun && !res[i].BeginToken.Morph.Class.IsAdjective) 
                    {
                        if (Pullenti.Ner.Geo.Internal.MiscLocationHelper.CheckGeoObjectBefore(res[0].BeginToken) && res[0].EndToken.Next == res[1].BeginToken && (res[0].WhitespacesAfterCount < 2)) 
                        {
                        }
                        else 
                        {
                            res.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }
                    if (res[i].Morph.Class.IsAdjective && res[i + 1].Morph.Class.IsAdjective) 
                    {
                        if (res[i].EndToken.Next.IsHiphen) 
                        {
                        }
                        else if (i == 1 && res[0].Typ == StreetItemType.Noun && res.Count == 3) 
                        {
                        }
                        else if (i == 0 && res.Count == 3 && res[2].Typ == StreetItemType.Noun) 
                        {
                        }
                        else 
                            continue;
                    }
                    res[i].Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(res[i].BeginToken, res[i + 1].EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    if (res[i].Value.Contains("-")) 
                        res[i].Value = res[i].Value.Replace('-', ' ');
                    if (!res[i + 1].BeginToken.Previous.IsHiphen && ((!res[i].BeginToken.Morph.Class.IsAdjective || isProp || isPers))) 
                    {
                        if (isPers && res[i + 1].EndToken.GetMorphClassInDictionary().IsProperName) 
                            res[i].AltValue = Pullenti.Ner.Core.MiscHelper.GetTextValue(res[i].BeginToken, res[i].EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                        else 
                            res[i].AltValue = Pullenti.Ner.Core.MiscHelper.GetTextValue(res[i + 1].BeginToken, res[i + 1].EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                        if (res[i].AltValue.Contains("-")) 
                            res[i].AltValue = res[i].AltValue.Replace('-', ' ');
                    }
                    res[i].EndToken = res[i + 1].EndToken;
                    res[i].ExistStreet = null;
                    res[i].IsInDictionary = res[i + 1].IsInDictionary || res[i].IsInDictionary;
                    res.RemoveAt(i + 1);
                    i--;
                }
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (res[i].Typ == StreetItemType.StdAdjective && res[i].EndToken.IsChar('.') && res[i + 1]._isSurname()) 
                {
                    res[i + 1].Value = (res[i + 1].BeginToken as Pullenti.Ner.TextToken).Term;
                    res[i + 1].AltValue = Pullenti.Ner.Core.MiscHelper.GetTextValue(res[i].BeginToken, res[i + 1].EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    res[i + 1].BeginToken = res[i].BeginToken;
                    res.RemoveAt(i);
                    break;
                }
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if ((res[i + 1].Typ == StreetItemType.StdAdjective && res[i + 1].EndToken.IsChar('.') && res[i + 1].BeginToken.LengthChar == 1) && !res[i].BeginToken.Chars.IsAllLower) 
                {
                    if (res[i]._isSurname()) 
                    {
                        if (i == (res.Count - 2) || res[i + 2].Typ != StreetItemType.Noun) 
                        {
                            res[i].EndToken = res[i + 1].EndToken;
                            res.RemoveAt(i + 1);
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (res[i].Typ == StreetItemType.Name || res[i].Typ == StreetItemType.StdName || res[i].Typ == StreetItemType.StdAdjective) 
                {
                    if (res[i + 1].Typ == StreetItemType.Noun && !res[i + 1].IsAbridge) 
                    {
                        int i0 = -1;
                        if (i == 1 && res[0].Typ == StreetItemType.Noun && res.Count == 3) 
                            i0 = 0;
                        else if (i == 0 && res.Count == 3 && res[2].Typ == StreetItemType.Noun) 
                            i0 = 2;
                        if (i0 < 0) 
                            continue;
                        if (res[i0].Termin == res[i + 1].Termin) 
                            continue;
                        res[i].AltValue = res[i].Value ?? Pullenti.Ner.Core.MiscHelper.GetTextValue(res[i].BeginToken, res[i].EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                        if (res[i].Typ == StreetItemType.StdAdjective) 
                        {
                            List<string> adjs = Pullenti.Ner.Geo.Internal.MiscLocationHelper.GetStdAdjFull(res[i].BeginToken, res[i + 1].Morph.Gender, res[i + 1].Morph.Number, true);
                            if (adjs != null && adjs.Count > 0) 
                                res[i].AltValue = adjs[0];
                        }
                        res[i].Value = string.Format("{0} {1}", res[i].AltValue, res[i + 1].Termin.CanonicText);
                        res[i].Typ = StreetItemType.StdName;
                        res[i0].AltTermin = res[i + 1].Termin;
                        res[i].EndToken = res[i + 1].EndToken;
                        res.RemoveAt(i + 1);
                        i--;
                    }
                }
            }
            if ((res.Count >= 3 && res[0].Typ == StreetItemType.Noun && res[0].Termin.CanonicText == "КВАРТАЛ") && ((res[1].Typ == StreetItemType.Name || res[1].Typ == StreetItemType.StdName)) && res[2].Typ == StreetItemType.Noun) 
            {
                if (res.Count == 3 || res[3].Typ == StreetItemType.Number) 
                {
                    res[1].Value = string.Format("{0} {1}", Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(res[1], Pullenti.Ner.Core.GetTextAttr.No), res[2].Termin.CanonicText);
                    res[1].EndToken = res[2].EndToken;
                    res.RemoveAt(2);
                }
            }
            if ((res.Count >= 3 && res[0].Typ == StreetItemType.Noun && res[0].Termin.CanonicText == "КВАРТАЛ") && ((res[2].Typ == StreetItemType.Name || res[2].Typ == StreetItemType.StdName)) && res[1].Typ == StreetItemType.Noun) 
            {
                if (res.Count == 3 || res[3].Typ == StreetItemType.Number) 
                {
                    res[1].Value = string.Format("{0} {1}", Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(res[2], Pullenti.Ner.Core.GetTextAttr.No), res[1].Termin.CanonicText);
                    res[1].EndToken = res[2].EndToken;
                    res[1].Typ = StreetItemType.Name;
                    res.RemoveAt(2);
                }
            }
            if (res.Count >= 3 && res[0].Typ == StreetItemType.Number && res[1].Typ == StreetItemType.Noun) 
            {
                Pullenti.Ner.NumberToken nt = res[0].BeginToken as Pullenti.Ner.NumberToken;
                if (nt != null && nt.Typ == Pullenti.Ner.NumberSpellingType.Digit && nt.Morph.Class.IsUndefined) 
                    return null;
            }
            int ii0 = -1;
            int ii1 = -1;
            if (res[0].Typ == StreetItemType.Noun && res[0].IsRoad) 
            {
                ii0 = (ii1 = 0);
                if (((ii0 + 1) < res.Count) && res[ii0 + 1].Typ == StreetItemType.Number && res[ii0 + 1].IsNumberKm) 
                    ii0++;
            }
            else if ((res.Count > 1 && res[0].Typ == StreetItemType.Number && res[0].IsNumberKm) && res[1].Typ == StreetItemType.Noun && res[1].IsRoad) 
                ii0 = (ii1 = 1);
            if (ii0 >= 0) 
            {
                if (res.Count == (ii0 + 1)) 
                {
                    Pullenti.Ner.Token tt = res[ii0].EndToken.Next;
                    StreetItemToken num = _tryAttachRoadNum(tt);
                    if (num != null) 
                    {
                        res.Add(num);
                        tt = num.EndToken.Next;
                        res[0].IsAbridge = false;
                    }
                    if (tt != null && (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                        Pullenti.Ner.Geo.GeoReferent g1 = tt.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                        tt = tt.Next;
                        if (tt != null && tt.IsHiphen) 
                            tt = tt.Next;
                        Pullenti.Ner.Geo.GeoReferent g2 = (tt == null ? null : tt.GetReferent() as Pullenti.Ner.Geo.GeoReferent);
                        if (g2 != null) 
                        {
                            if (g1.IsCity && g2.IsCity) 
                            {
                                StreetItemToken nam = new StreetItemToken(res[0].EndToken.Next, tt) { Typ = StreetItemType.Name };
                                nam.Value = string.Format("{0} - {1}", g1.ToString(true, tt.Kit.BaseLanguage, 0), g2.ToString(true, tt.Kit.BaseLanguage, 0)).ToUpper();
                                nam.AltValue = string.Format("{0} - {1}", g2.ToString(true, tt.Kit.BaseLanguage, 0), g1.ToString(true, tt.Kit.BaseLanguage, 0)).ToUpper();
                                res.Add(nam);
                            }
                        }
                    }
                    else if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            StreetItemToken nam = new StreetItemToken(tt, br.EndToken) { Typ = StreetItemType.Name, IsInBrackets = true };
                            nam.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt.Next, br.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                            res.Add(nam);
                        }
                    }
                }
                else if ((res.Count == (ii0 + 2) && res[ii0 + 1].Typ == StreetItemType.Name && res[ii0 + 1].EndToken.Next != null) && res[ii0 + 1].EndToken.Next.IsHiphen) 
                {
                    Pullenti.Ner.Token tt = res[ii0 + 1].EndToken.Next.Next;
                    Pullenti.Ner.Geo.GeoReferent g2 = (tt == null ? null : tt.GetReferent() as Pullenti.Ner.Geo.GeoReferent);
                    Pullenti.Ner.Token te = null;
                    string name2 = null;
                    if (g2 == null && tt != null) 
                    {
                        Pullenti.Ner.ReferentToken rt = tt.Kit.ProcessReferent("GEO", tt);
                        if (rt != null) 
                        {
                            te = rt.EndToken;
                            name2 = rt.Referent.ToString(true, te.Kit.BaseLanguage, 0);
                        }
                        else 
                        {
                            List<Pullenti.Ner.Geo.Internal.CityItemToken> cits2 = Pullenti.Ner.Geo.Internal.CityItemToken.TryParseList(tt, null, 2);
                            if (cits2 != null) 
                            {
                                if (cits2.Count == 1 && cits2[0].Typ == Pullenti.Ner.Geo.Internal.CityItemToken.ItemType.ProperName) 
                                {
                                    name2 = cits2[0].Value;
                                    te = cits2[0].EndToken;
                                }
                            }
                        }
                    }
                    else 
                    {
                        te = tt;
                        name2 = g2.ToString(true, te.Kit.BaseLanguage, 0);
                    }
                    if (((g2 != null && g2.IsCity)) || ((g2 == null && name2 != null))) 
                    {
                        res[ii0 + 1].AltValue = string.Format("{0} - {1}", name2, res[ii0 + 1].Value ?? res[ii0 + 1].GetSourceText()).ToUpper();
                        res[ii0 + 1].Value = string.Format("{0} - {1}", res[ii0 + 1].Value ?? res[ii0 + 1].GetSourceText(), name2).ToUpper();
                        res[ii0 + 1].EndToken = te;
                    }
                }
                StreetItemToken nn = _tryAttachRoadNum(res[res.Count - 1].EndToken.Next);
                if (nn != null) 
                {
                    res.Add(nn);
                    res[ii1].IsAbridge = false;
                }
                if (res.Count > (ii0 + 1) && res[ii0 + 1].Typ == StreetItemType.Name && res[ii1].Termin.CanonicText == "АВТОДОРОГА") 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res[ii0 + 1].BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.Adjectives.Count > 0) 
                        return null;
                }
            }
            if (res.Count > 0) 
            {
                StreetItemToken it = res[res.Count - 1];
                StreetItemToken it0 = (res.Count > 1 ? res[res.Count - 2] : null);
                if (it.Typ == StreetItemType.Number && !it.NumberHasPrefix) 
                {
                    if (it.BeginToken is Pullenti.Ner.NumberToken) 
                    {
                        if (!it.BeginToken.Morph.Class.IsAdjective || it.BeginToken.Morph.Class.IsNoun) 
                        {
                            if (AddressItemToken.CheckHouseAfter(it.EndToken.Next, false, true)) 
                                it.NumberHasPrefix = true;
                            else if (it0 != null && it0.Typ == StreetItemType.Noun && (((it0.Termin.CanonicText == "МИКРОРАЙОН" || it0.Termin.CanonicText == "МІКРОРАЙОН" || it0.Termin.CanonicText == "КВАРТАЛ") || it0.Termin.CanonicText == "ГОРОДОК"))) 
                            {
                                AddressItemToken ait = AddressItemToken.TryParse(it.BeginToken, locStreets, false, true, null);
                                if (ait != null && ait.Typ == AddressItemToken.ItemType.Number && ait.EndChar > it.EndChar) 
                                {
                                    it.Number = null;
                                    it.Value = ait.Value;
                                    it.EndToken = ait.EndToken;
                                    it.Typ = StreetItemType.Name;
                                }
                            }
                            else if (it0 != null && it0.Termin != null && it0.Termin.CanonicText == "ПОЧТОВОЕ ОТДЕЛЕНИЕ") 
                                it.NumberHasPrefix = true;
                            else if (res.Count == 2 && res[0].Typ == StreetItemType.Noun && (res[0].WhitespacesAfterCount < 2)) 
                            {
                            }
                            else 
                                res.RemoveAt(res.Count - 1);
                        }
                        else 
                            it.NumberHasPrefix = true;
                    }
                }
            }
            if (res.Count == 0) 
                return null;
            for (int i = 0; i < res.Count; i++) 
            {
                if ((res[i].Typ == StreetItemType.Noun && res[i].Chars.IsCapitalUpper && (((res[i].Termin.CanonicText == "НАБЕРЕЖНАЯ" || res[i].Termin.CanonicText == "МИКРОРАЙОН" || res[i].Termin.CanonicText == "НАБЕРЕЖНА") || res[i].Termin.CanonicText == "МІКРОРАЙОН" || res[i].Termin.CanonicText == "ГОРОДОК"))) && res[i].BeginToken.IsValue(res[i].Termin.CanonicText, null)) 
                {
                    bool ok = false;
                    if (i > 0 && ((res[i - 1].Typ == StreetItemType.Noun || res[i - 1].Typ == StreetItemType.StdAdjective))) 
                        ok = true;
                    else if (i > 1 && ((res[i - 1].Typ == StreetItemType.StdAdjective || res[i - 1].Typ == StreetItemType.Number)) && res[i - 2].Typ == StreetItemType.Noun) 
                        ok = true;
                    if (ok) 
                    {
                        res[i].Termin = null;
                        res[i].Typ = StreetItemType.Name;
                    }
                }
            }
            StreetItemToken last = res[res.Count - 1];
            for (int kk = 0; kk < 2; kk++) 
            {
                Pullenti.Ner.Token ttt = last.EndToken.Next;
                if (((last.Typ == StreetItemType.Name && ttt != null && ttt.LengthChar == 1) && ttt.Chars.IsAllUpper && (ttt.WhitespacesBeforeCount < 2)) && ttt.Next != null && ttt.Next.IsChar('.')) 
                    last.EndToken = ttt.Next;
            }
            return res;
        }
        static StreetItemToken _tryAttachRoadNum(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if (!t.Chars.IsLetter || t.LengthChar != 1) 
                return null;
            Pullenti.Ner.Token tt = t.Next;
            if (tt != null && tt.IsHiphen) 
                tt = tt.Next;
            if (!(tt is Pullenti.Ner.NumberToken)) 
                return null;
            StreetItemToken res = new StreetItemToken(t, tt) { Typ = StreetItemType.Name };
            res.Value = string.Format("{0}{1}", t.GetSourceText().ToUpper(), (tt as Pullenti.Ner.NumberToken).Value);
            return res;
        }
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new Pullenti.Ner.Core.TerminCollection();
            m_StdOntMisc = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("УЛИЦА") { Tag = StreetItemType.Noun, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddAbridge("УЛ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВУЛИЦЯ") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddAbridge("ВУЛ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("STREET") { Tag = StreetItemType.Noun };
            t.AddAbridge("ST.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПЛОЩАДЬ") { Tag = StreetItemType.Noun, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddAbridge("ПЛ.");
            t.AddAbridge("ПЛОЩ.");
            t.AddAbridge("ПЛ-ДЬ");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПЛОЩА") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddAbridge("ПЛ.");
            t.AddAbridge("ПЛОЩ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МАЙДАН") { Tag = StreetItemType.Noun, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Masculine };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("SQUARE") { Tag = StreetItemType.Noun };
            t.AddAbridge("SQ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОЕЗД") { Tag = StreetItemType.Noun, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddAbridge("ПР.");
            t.AddAbridge("П-Д");
            t.AddAbridge("ПР-Д");
            t.AddAbridge("ПР-ЗД");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОЕЗД") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddAbridge("ПР.");
            t.AddAbridge("П-Д");
            t.AddAbridge("ПР-Д");
            t.AddAbridge("ПР-ЗД");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛИНИЯ") { Tag = StreetItemType.Noun, Tag2 = 2, Gender = Pullenti.Morph.MorphGender.Feminie };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛІНІЯ") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 2, Gender = Pullenti.Morph.MorphGender.Feminie };
            m_Ontology.Add(t);
            m_Prospect = (t = new Pullenti.Ner.Core.Termin("ПРОСПЕКТ") { Tag = StreetItemType.Noun, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Masculine });
            t.AddAbridge("ПРОС.");
            t.AddAbridge("ПРКТ");
            t.AddAbridge("ПРОСП.");
            t.AddAbridge("ПР-Т");
            t.AddAbridge("ПР-КТ");
            t.AddAbridge("П-Т");
            t.AddAbridge("П-КТ");
            t.AddAbridge("ПР Т");
            t.AddAbridge("ПР-ТЕ");
            t.AddAbridge("ПР-КТЕ");
            t.AddAbridge("П-ТЕ");
            t.AddAbridge("П-КТЕ");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПЕРЕУЛОК") { Tag = StreetItemType.Noun, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddAbridge("ПЕР.");
            t.AddAbridge("ПЕР-К");
            t.AddVariant("ПРЕУЛОК", false);
            t.AddVariant("ПРОУЛОК", false);
            t.AddAbridge("ПРОУЛ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОВУЛОК") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddAbridge("ПРОВ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("LANE") { Tag = StreetItemType.Noun, Tag2 = 0 };
            t.AddAbridge("LN.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТУПИК") { Tag = StreetItemType.Noun, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddAbridge("ТУП.");
            t.AddAbridge("Т.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("БУЛЬВАР") { Tag = StreetItemType.Noun, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddAbridge("БУЛЬВ.");
            t.AddAbridge("БУЛ.");
            t.AddAbridge("Б-Р");
            t.AddAbridge("Б-РЕ");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("BOULEVARD") { Tag = StreetItemType.Noun, Tag2 = 0 };
            t.AddAbridge("BLVD");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СКВЕР") { Tag = StreetItemType.Noun, Tag2 = 1 };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НАБЕРЕЖНАЯ") { Tag = StreetItemType.Noun, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddAbridge("НАБ.");
            t.AddAbridge("НАБЕР.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НАБЕРЕЖНА") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddAbridge("НАБ.");
            t.AddAbridge("НАБЕР.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АЛЛЕЯ") { Tag = StreetItemType.Noun, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddAbridge("АЛ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АЛЕЯ") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddAbridge("АЛ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ALLEY") { Tag = StreetItemType.Noun, Tag2 = 0 };
            t.AddAbridge("ALY.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОСЕКА") { Tag = StreetItemType.Noun, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddVariant("ПРОСЕК", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОСІКА") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Feminie };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ШОССЕ") { Tag = StreetItemType.Noun, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Neuter };
            t.AddAbridge("Ш.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ШОСЕ") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Neuter };
            t.AddAbridge("Ш.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ROAD") { Tag = StreetItemType.Noun, Tag2 = 1 };
            t.AddAbridge("RD.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МИКРОРАЙОН") { Tag = StreetItemType.Noun, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddAbridge("МКР.");
            t.AddAbridge("МИКР-Н");
            t.AddAbridge("МКР-Н");
            t.AddAbridge("МКРН.");
            t.AddAbridge("М-Н");
            t.AddAbridge("М-ОН");
            t.AddAbridge("М/Р");
            t.AddVariant("МІКРОРАЙОН", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КВАРТАЛ") { Tag = StreetItemType.Noun, Tag2 = 2, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddAbridge("КВАРТ.");
            t.AddAbridge("КВ-Л");
            t.AddAbridge("КВ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЖИЛОЙ КОМПЛЕКС") { Tag = StreetItemType.Noun, Acronym = "ЖК", Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddVariant("ЖИЛКОМПЛЕКС", false);
            t.AddAbridge("ЖИЛ.К.");
            t.AddAbridge("Ж/К");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОРОДОК") { Tag = StreetItemType.Noun, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Masculine };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МІСТЕЧКО") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Neuter };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("HILL") { Tag = StreetItemType.Noun, Tag2 = 0 };
            t.AddAbridge("HL.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВОЕННЫЙ ГОРОДОК") { Tag = StreetItemType.Noun, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddAbridge("В.ГОРОДОК");
            t.AddAbridge("В/Г");
            t.AddAbridge("В/ГОРОДОК");
            t.AddAbridge("В/ГОР");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРОМЗОНА") { Tag = StreetItemType.Noun, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddVariant("ПРОМЫШЛЕННАЯ ЗОНА", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЖИЛАЯ ЗОНА") { Tag = StreetItemType.Noun, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddVariant("ЖИЛЗОНА", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОММУНАЛЬНАЯ ЗОНА") { Tag = StreetItemType.Noun, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddVariant("КОМЗОНА", false);
            t.AddAbridge("КОММУН. ЗОНА");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МАССИВ") { Tag = StreetItemType.Noun, Tag2 = 2, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddVariant("ЖИЛОЙ МАССИВ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МОСТ") { Tag = StreetItemType.Noun, Tag2 = 2, Gender = Pullenti.Morph.MorphGender.Masculine };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МІСТ") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 2, Gender = Pullenti.Morph.MorphGender.Masculine };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПАРК") { Tag = StreetItemType.Noun, Tag2 = 2, Gender = Pullenti.Morph.MorphGender.Masculine };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("PLAZA") { Tag = StreetItemType.Noun, Tag2 = 1 };
            t.AddAbridge("PLZ");
            m_Ontology.Add(t);
            m_Metro = (t = new Pullenti.Ner.Core.Termin("СТАНЦИЯ МЕТРО") { CanonicText = "МЕТРО", Tag = StreetItemType.Noun, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Feminie });
            t.AddVariant("СТАНЦІЯ МЕТРО", false);
            t.AddAbridge("СТ.МЕТРО");
            t.AddAbridge("СТ.М.");
            t.AddAbridge("МЕТРО");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АВТОДОРОГА") { Tag = StreetItemType.Noun, Acronym = "ФАД", Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddVariant("ФЕДЕРАЛЬНАЯ АВТОДОРОГА", false);
            t.AddVariant("АВТОМОБИЛЬНАЯ ДОРОГА", false);
            t.AddVariant("АВТОТРАССА", false);
            t.AddVariant("ФЕДЕРАЛЬНАЯ ТРАССА", false);
            t.AddVariant("АВТОМАГИСТРАЛЬ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОРОГА") { CanonicText = "АВТОДОРОГА", Tag = StreetItemType.Noun, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddVariant("ТРАССА", false);
            t.AddVariant("МАГИСТРАЛЬ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АВТОДОРОГА") { Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 0, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddVariant("ФЕДЕРАЛЬНА АВТОДОРОГА", false);
            t.AddVariant("АВТОМОБІЛЬНА ДОРОГА", false);
            t.AddVariant("АВТОТРАСА", false);
            t.AddVariant("ФЕДЕРАЛЬНА ТРАСА", false);
            t.AddVariant("АВТОМАГІСТРАЛЬ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОРОГА") { CanonicText = "АВТОДОРОГА", Tag = StreetItemType.Noun, Lang = Pullenti.Morph.MorphLang.UA, Tag2 = 1, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddVariant("ТРАСА", false);
            t.AddVariant("МАГІСТРАЛЬ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МОСКОВСКАЯ КОЛЬЦЕВАЯ АВТОМОБИЛЬНАЯ ДОРОГА") { Acronym = "МКАД", Tag = StreetItemType.Fix, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddVariant("МОСКОВСКАЯ КОЛЬЦЕВАЯ АВТОДОРОГА", false);
            m_Ontology.Add(t);
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("САДОВОЕ КОЛЬЦО") { Tag = StreetItemType.Fix });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("БУЛЬВАРНОЕ КОЛЬЦО") { Tag = StreetItemType.Fix });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ТРАНСПОРТНОЕ КОЛЬЦО") { Tag = StreetItemType.Fix });
            t = new Pullenti.Ner.Core.Termin("ПОЧТОВОЕ ОТДЕЛЕНИЕ") { Tag = StreetItemType.StdName, Acronym = "ОПС", Gender = Pullenti.Morph.MorphGender.Neuter };
            t.AddAbridge("П.О.");
            t.AddAbridge("ПОЧТ.ОТД.");
            t.AddAbridge("ПОЧТОВ.ОТД.");
            t.AddAbridge("ПОЧТОВОЕ ОТД.");
            t.AddVariant("ОТДЕЛЕНИЕ ПОЧТОВОЙ СВЯЗИ", false);
            t.AddVariant("ПОЧТАМТ", false);
            t.AddVariant("ГЛАВПОЧТАМТ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("БОЛЬШОЙ") { Tag = StreetItemType.StdAdjective };
            t.AddAbridge("БОЛ.");
            t.AddAbridge("Б.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕЛИКИЙ") { Tag = StreetItemType.StdAdjective, Lang = Pullenti.Morph.MorphLang.UA };
            t.AddAbridge("ВЕЛ.");
            t.AddAbridge("В.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МАЛЫЙ") { Tag = StreetItemType.StdAdjective };
            t.AddAbridge("МАЛ.");
            t.AddAbridge("М.");
            t.AddVariant("МАЛИЙ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СРЕДНИЙ") { Tag = StreetItemType.StdAdjective };
            t.AddAbridge("СРЕД.");
            t.AddAbridge("СР.");
            t.AddAbridge("С.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕРЕДНІЙ") { Tag = StreetItemType.StdAdjective, Lang = Pullenti.Morph.MorphLang.UA };
            t.AddAbridge("СЕРЕД.");
            t.AddAbridge("СЕР.");
            t.AddAbridge("С.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕРХНИЙ") { Tag = StreetItemType.StdAdjective };
            t.AddAbridge("ВЕРХН.");
            t.AddAbridge("ВЕРХ.");
            t.AddAbridge("ВЕР.");
            t.AddAbridge("В.");
            t.AddVariant("ВЕРХНІЙ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НИЖНИЙ") { Tag = StreetItemType.StdAdjective };
            t.AddAbridge("НИЖН.");
            t.AddAbridge("НИЖ.");
            t.AddAbridge("Н.");
            t.AddVariant("НИЖНІЙ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТАРЫЙ") { Tag = StreetItemType.StdAdjective };
            t.AddAbridge("СТАР.");
            t.AddAbridge("СТ.");
            t.AddVariant("СТАРИЙ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НОВЫЙ") { Tag = StreetItemType.StdAdjective };
            t.AddAbridge("НОВ.");
            t.AddVariant("НОВИЙ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НОМЕР") { Tag = StreetItemType.StdAdjective };
            t.AddAbridge("N");
            t.AddAbridge("№");
            t.AddAbridge("НОМ.");
            m_Ontology.Add(t);
            foreach (string s in new string[] {"ФРИДРИХА ЭНГЕЛЬСА", "КАРЛА МАРКСА", "РОЗЫ ЛЮКСЕМБУРГ"}) 
            {
                t = new Pullenti.Ner.Core.Termin(s) { Tag = StreetItemType.StdName };
                t.AddAllAbridges(0, 0, 0);
                m_Ontology.Add(t);
            }
            foreach (string s in new string[] {"МАРТА", "МАЯ", "ОКТЯБРЯ", "НОЯБРЯ", "БЕРЕЗНЯ", "ТРАВНЯ", "ЖОВТНЯ", "ЛИСТОПАДА"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = StreetItemType.StdName });
            }
            foreach (string s in new string[] {"МАРШАЛА", "ГЕНЕРАЛА", "АДМИРАЛА", "КОСМОНАВТА", "ЛЕТЧИКА", "АВИАКОНСТРУКТОРА", "АРХИТЕКТОРА", "СКУЛЬПТОРА", "ХУДОЖНИКА", "КОНСТРУКТОРА", "АКАДЕМИКА", "ПРОФЕССОРА", "ЛЕЙТЕНАНТА", "КАПИТАНА", "МАЙОРА", "ПОДПОЛКОВНИКА", "ПОЛКОВНИКА", "ПОЛИЦИИ", "МИЛИЦИИ"}) 
            {
                m_StdOntMisc.Add(new Pullenti.Ner.Core.Termin(s));
                t = new Pullenti.Ner.Core.Termin(s) { Tag = StreetItemType.StdPartOfName };
                t.AddAllAbridges(0, 0, 2);
                t.AddAllAbridges(2, 5, 0);
                t.AddAbridge("ГЛ." + s);
                t.AddAbridge("ГЛАВ." + s);
                m_Ontology.Add(t);
            }
            foreach (string s in new string[] {"МАРШАЛА", "ГЕНЕРАЛА", "АДМІРАЛА", "КОСМОНАВТА", "ЛЬОТЧИКА", "АВІАКОНСТРУКТОРА", "АРХІТЕКТОРА", "СКУЛЬПТОРА", "ХУДОЖНИКА", "КОНСТРУКТОРА", "АКАДЕМІКА", "ПРОФЕСОРА", "ЛЕЙТЕНАНТА", "КАПІТАН", "МАЙОР", "ПІДПОЛКОВНИК", "ПОЛКОВНИК", "ПОЛІЦІЇ", "МІЛІЦІЇ"}) 
            {
                m_StdOntMisc.Add(new Pullenti.Ner.Core.Termin(s));
                t = new Pullenti.Ner.Core.Termin(s) { Tag = StreetItemType.StdPartOfName, Lang = Pullenti.Morph.MorphLang.UA };
                t.AddAllAbridges(0, 0, 2);
                t.AddAllAbridges(2, 5, 0);
                t.AddAbridge("ГЛ." + s);
                t.AddAbridge("ГЛАВ." + s);
                m_Ontology.Add(t);
            }
            t = new Pullenti.Ner.Core.Termin("ВАСИЛЬЕВСКОГО ОСТРОВА") { Tag = StreetItemType.StdName };
            t.AddAbridge("В.О.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПЕТРОГРАДСКОЙ СТОРОНЫ") { Tag = StreetItemType.StdName };
            t.AddAbridge("П.С.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОЛИМПИЙСКАЯ ДЕРЕВНЯ") { Tag = StreetItemType.Fix };
            t.AddAbridge("ОЛИМП. ДЕРЕВНЯ");
            t.AddAbridge("ОЛИМП. ДЕР.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛЕНИНСКИЕ ГОРЫ") { Tag = StreetItemType.Fix };
            m_Ontology.Add(t);
            byte[] obj = ResourceHelper.GetBytes("s.dat");
            if (obj == null) 
                throw new Exception("Can't file resource file s.dat in Location analyzer");
            string streets = Encoding.UTF8.GetString(Pullenti.Ner.Geo.Internal.MiscLocationHelper.Deflate(obj));
            StringBuilder name = new StringBuilder();
            Dictionary<string, bool> Names = new Dictionary<string, bool>();
            foreach (string line0 in streets.Split('\n')) 
            {
                string line = line0.Trim();
                if (string.IsNullOrEmpty(line)) 
                    continue;
                if (line.IndexOf(';') >= 0) 
                {
                    string[] parts = line.Split(';');
                    t = new Pullenti.Ner.Core.Termin() { Tag = StreetItemType.Name, IgnoreTermsOrder = true };
                    t.InitByNormalText(parts[0], null);
                    for (int j = 1; j < parts.Length; j++) 
                    {
                        t.AddVariant(parts[j], true);
                    }
                }
                else 
                {
                    t = new Pullenti.Ner.Core.Termin() { Tag = StreetItemType.Name, IgnoreTermsOrder = true };
                    t.InitByNormalText(line, null);
                }
                if (t.Terms.Count > 1) 
                    t.Tag = StreetItemType.StdName;
                m_Ontology.Add(t);
            }
        }
        static Pullenti.Ner.Core.TerminCollection m_Ontology;
        static Pullenti.Ner.Core.TerminCollection m_StdOntMisc;
        static Pullenti.Ner.Core.Termin m_Prospect;
        static Pullenti.Ner.Core.Termin m_Metro;
    }
}