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

namespace Pullenti.Ner.Address.Internal
{
    public static class StreetDefineHelper
    {
        public static bool CheckStreetAfter(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            while (t != null && ((t.IsCharOf(",;") || t.Morph.Class.IsPreposition))) 
            {
                t = t.Next;
            }
            List<StreetItemToken> li = StreetItemToken.TryParseList(t, null, 10);
            if (li == null) 
                return false;
            AddressItemToken rt = TryParseStreet(li, false, false);
            if (rt != null && rt.BeginToken == t) 
                return true;
            else 
                return false;
        }
        public static Pullenti.Ner.ReferentToken TryParseExtStreet(List<StreetItemToken> sli)
        {
            AddressItemToken a = TryParseStreet(sli, true, false);
            if (a != null) 
                return new Pullenti.Ner.ReferentToken(a.Referent, a.BeginToken, a.EndToken);
            return null;
        }
        internal static AddressItemToken TryParseStreet(List<StreetItemToken> sli, bool extOntoRegim = false, bool forMetro = false)
        {
            if (sli == null || sli.Count == 0) 
                return null;
            int i;
            int j;
            for (i = 0; i < sli.Count; i++) 
            {
                if (i == 0 && sli[i].Typ == StreetItemType.Fix && ((sli.Count == 1 || sli[1].Typ != StreetItemType.Noun))) 
                    return _tryParseFix(sli);
                else if (sli[i].Typ == StreetItemType.Noun) 
                {
                    if ((i == 0 && sli[i].Termin.CanonicText == "УЛИЦА" && ((i + 2) < sli.Count)) && sli[i + 1].Typ == StreetItemType.Noun && sli[i + 1].Termin.CanonicText == "МИКРОРАЙОН") 
                    {
                        sli[i + 1].BeginToken = sli[i].BeginToken;
                        sli.RemoveAt(i);
                    }
                    if (sli[i].Termin.CanonicText == "МЕТРО") 
                    {
                        if ((i + 1) < sli.Count) 
                        {
                            List<StreetItemToken> sli1 = new List<StreetItemToken>();
                            for (int ii = i + 1; ii < sli.Count; ii++) 
                            {
                                sli1.Add(sli[ii]);
                            }
                            AddressItemToken str1 = TryParseStreet(sli1, extOntoRegim, true);
                            if (str1 != null) 
                            {
                                str1.BeginToken = sli[i].BeginToken;
                                str1.IsDoubt = sli[i].IsAbridge;
                                if (sli[i + 1].IsInBrackets) 
                                    str1.IsDoubt = false;
                                return str1;
                            }
                        }
                        else if (i == 1 && sli[0].Typ == StreetItemType.Name) 
                        {
                            forMetro = true;
                            break;
                        }
                        if (i == 0 && sli.Count > 0) 
                        {
                            forMetro = true;
                            break;
                        }
                        return null;
                    }
                    if (i == 0 && (i + 1) >= sli.Count && ((sli[i].Termin.CanonicText == "ВОЕННЫЙ ГОРОДОК" || sli[i].Termin.CanonicText == "ПРОМЗОНА"))) 
                    {
                        Pullenti.Ner.Address.StreetReferent stri0 = new Pullenti.Ner.Address.StreetReferent();
                        stri0.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, "микрорайон", false, 0);
                        stri0.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, sli[i].Termin.CanonicText, false, 0);
                        return new AddressItemToken(AddressItemToken.ItemType.Street, sli[0].BeginToken, sli[0].EndToken) { Referent = stri0, IsDoubt = true };
                    }
                    if (i == 0 && (i + 1) >= sli.Count && sli[i].Termin.CanonicText == "МИКРОРАЙОН") 
                    {
                        Pullenti.Ner.Address.StreetReferent stri0 = new Pullenti.Ner.Address.StreetReferent();
                        stri0.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, sli[i].Termin.CanonicText.ToLower(), false, 0);
                        return new AddressItemToken(AddressItemToken.ItemType.Street, sli[0].BeginToken, sli[0].EndToken) { Referent = stri0, IsDoubt = true };
                    }
                    if (sli[i].Termin.CanonicText == "ПЛОЩАДЬ" || sli[i].Termin.CanonicText == "ПЛОЩА") 
                    {
                        Pullenti.Ner.Token tt = sli[i].EndToken.Next;
                        if (tt != null && ((tt.IsHiphen || tt.IsChar(':')))) 
                            tt = tt.Next;
                        Pullenti.Ner.Core.NumberExToken nex = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(tt);
                        if (nex != null) 
                            return null;
                    }
                    break;
                }
            }
            if (i >= sli.Count) 
                return TryDetectNonNoun(sli, extOntoRegim, forMetro);
            StreetItemToken name = null;
            string number = null;
            string age = null;
            StreetItemToken adj = null;
            StreetItemToken noun = sli[i];
            StreetItemToken altNoun = null;
            bool isMicroRaion = (noun.Termin.CanonicText == "МИКРОРАЙОН" || noun.Termin.CanonicText == "МІКРОРАЙОН" || noun.Termin.CanonicText == "КВАРТАЛ") || Pullenti.Morph.LanguageHelper.EndsWith(noun.Termin.CanonicText, "ГОРОДОК");
            int before = 0;
            int after = 0;
            for (j = 0; j < i; j++) 
            {
                if ((sli[j].Typ == StreetItemType.Name || sli[j].Typ == StreetItemType.StdName || sli[j].Typ == StreetItemType.Fix) || sli[j].Typ == StreetItemType.StdAdjective || sli[j].Typ == StreetItemType.StdPartOfName) 
                    before++;
                else if (sli[j].Typ == StreetItemType.Number) 
                {
                    if (sli[j].IsNewlineAfter) 
                        return null;
                    if (sli[j].Number.Morph.Class.IsAdjective) 
                        before++;
                    else if (isMicroRaion) 
                        before++;
                    else if (sli[i].NumberHasPrefix) 
                        before++;
                }
                else 
                    before++;
            }
            for (j = i + 1; j < sli.Count; j++) 
            {
                if ((sli[j].Typ == StreetItemType.Name || sli[j].Typ == StreetItemType.StdName || sli[j].Typ == StreetItemType.Fix) || sli[j].Typ == StreetItemType.StdAdjective || sli[j].Typ == StreetItemType.StdPartOfName) 
                    after++;
                else if (sli[j].Typ == StreetItemType.Number) 
                {
                    if (sli[j].Number != null && sli[j].Number.Morph.Class.IsAdjective) 
                        after++;
                    else if (isMicroRaion) 
                        after++;
                    else if (sli[j].NumberHasPrefix) 
                        after++;
                    else if (extOntoRegim) 
                        after++;
                }
                else if (sli[j].Typ == StreetItemType.Noun) 
                    break;
                else 
                    after++;
            }
            List<StreetItemToken> rli = new List<StreetItemToken>();
            int n0;
            int n1;
            if (before > after) 
            {
                if (noun.Termin.CanonicText == "МЕТРО") 
                    return null;
                Pullenti.Ner.Token tt = sli[0].BeginToken;
                if (tt == sli[0].EndToken && noun.BeginToken == sli[0].EndToken.Next) 
                {
                    if (!tt.Morph.Class.IsAdjective && !(tt is Pullenti.Ner.NumberToken)) 
                    {
                        if ((sli[0].IsNewlineBefore || !Pullenti.Ner.Geo.Internal.MiscLocationHelper.CheckGeoObjectBefore(sli[0].BeginToken) || noun.Morph.Case.IsGenitive) || noun.Morph.Case.IsInstrumental) 
                        {
                            bool ok = false;
                            if (AddressItemToken.CheckHouseAfter(noun.EndToken.Next, false, true)) 
                                ok = true;
                            else if (noun.EndToken.Next == null) 
                                ok = true;
                            else if (noun.IsNewlineAfter && Pullenti.Ner.Geo.Internal.MiscLocationHelper.CheckGeoObjectBefore(sli[0].BeginToken)) 
                                ok = true;
                            if (!ok) 
                            {
                                if ((noun.Chars.IsLatinLetter && noun.Chars.IsCapitalUpper && sli[0].Chars.IsLatinLetter) && sli[0].Chars.IsCapitalUpper) 
                                    ok = true;
                            }
                            if (!ok) 
                                return null;
                        }
                    }
                }
                n0 = 0;
                n1 = i - 1;
            }
            else if (i == 1 && sli[0].Typ == StreetItemType.Number) 
            {
                if (!sli[0].IsWhitespaceAfter) 
                    return null;
                number = (sli[0].Number == null ? sli[0].Value : sli[0].Number.IntValue.Value.ToString());
                if (sli[0].IsNumberKm) 
                    number += "км";
                n0 = i + 1;
                n1 = sli.Count - 1;
                rli.Add(sli[0]);
                rli.Add(sli[i]);
            }
            else if (after > before) 
            {
                n0 = i + 1;
                n1 = sli.Count - 1;
                rli.Add(sli[i]);
            }
            else if (after == 0) 
                return null;
            else if ((sli.Count > 2 && ((sli[0].Typ == StreetItemType.Name || sli[0].Typ == StreetItemType.StdAdjective || sli[0].Typ == StreetItemType.StdName)) && sli[1].Typ == StreetItemType.Noun) && sli[2].Typ == StreetItemType.Number) 
            {
                n0 = 0;
                n1 = 0;
                bool num = false;
                Pullenti.Ner.Token tt2 = sli[2].EndToken.Next;
                if (sli[2].IsNumberKm) 
                    num = true;
                else if (sli[0].BeginToken.Previous != null && sli[0].BeginToken.Previous.IsValue("КИЛОМЕТР", null)) 
                {
                    sli[2].IsNumberKm = true;
                    num = true;
                }
                else if (sli[2].BeginToken.Previous.IsComma) 
                {
                }
                else if (sli[2].BeginToken != sli[2].EndToken) 
                    num = true;
                else if (AddressItemToken.CheckHouseAfter(sli[2].EndToken.Next, false, true)) 
                    num = true;
                else if (sli[2].Morph.Class.IsAdjective && (sli[2].WhitespacesBeforeCount < 2)) 
                {
                    if (sli[2].EndToken.Next == null || sli[2].EndToken.IsComma || sli[2].IsNewlineAfter) 
                        num = true;
                }
                if (num) 
                {
                    number = (sli[2].Number == null ? sli[2].Value : sli[2].Number.IntValue.Value.ToString());
                    if (sli[2].IsNumberKm) 
                        number += "км";
                    rli.Add(sli[2]);
                }
                else 
                    sli.RemoveRange(2, sli.Count - 2);
            }
            else 
                return null;
            string secNumber = null;
            for (j = n0; j <= n1; j++) 
            {
                if (sli[j].Typ == StreetItemType.Number) 
                {
                    if (age != null || ((sli[j].IsNewlineBefore && j > 0))) 
                        break;
                    if (number != null) 
                    {
                        if (name != null && name.Typ == StreetItemType.StdName) 
                        {
                            secNumber = (sli[j].Number == null ? sli[j].Value : sli[j].Number.IntValue.Value.ToString());
                            if (sli[j].IsNumberKm) 
                                secNumber += "км";
                            rli.Add(sli[j]);
                            continue;
                        }
                        if (((j + 1) < sli.Count) && sli[j + 1].Typ == StreetItemType.StdName) 
                        {
                            secNumber = (sli[j].Number == null ? sli[j].Value : sli[j].Number.IntValue.Value.ToString());
                            if (sli[j].IsNumberKm) 
                                secNumber += "км";
                            rli.Add(sli[j]);
                            continue;
                        }
                        break;
                    }
                    if (sli[j].Number != null && sli[j].Number.Typ == Pullenti.Ner.NumberSpellingType.Digit && !sli[j].Number.Morph.Class.IsAdjective) 
                    {
                        if (sli[j].WhitespacesBeforeCount > 2 && j > 0) 
                            break;
                        if (sli[j].Number != null && sli[j].Number.IntValue.Value > 20) 
                        {
                            if (j > n0) 
                            {
                                if (((j + 1) < sli.Count) && sli[j + 1].Typ == StreetItemType.Noun) 
                                {
                                }
                                else 
                                    break;
                            }
                        }
                        if (j == n0 && n0 > 0) 
                        {
                        }
                        else if (j == n0 && n0 == 0 && sli[j].WhitespacesAfterCount == 1) 
                        {
                        }
                        else if (sli[j].NumberHasPrefix) 
                        {
                        }
                        else if (j == n1 && ((n1 + 1) < sli.Count) && sli[n1 + 1].Typ == StreetItemType.Noun) 
                        {
                        }
                        else 
                            break;
                    }
                    number = (sli[j].Number == null ? sli[j].Value : sli[j].Number.IntValue.Value.ToString());
                    if (sli[j].IsNumberKm) 
                        number += "км";
                    rli.Add(sli[j]);
                }
                else if (sli[j].Typ == StreetItemType.Age) 
                {
                    if (number != null || age != null) 
                        break;
                    age = sli[j].Number.IntValue.Value.ToString();
                    rli.Add(sli[j]);
                }
                else if (sli[j].Typ == StreetItemType.StdAdjective) 
                {
                    if (adj != null) 
                        return null;
                    adj = sli[j];
                    rli.Add(sli[j]);
                }
                else if (sli[j].Typ == StreetItemType.Name || sli[j].Typ == StreetItemType.StdName || sli[j].Typ == StreetItemType.Fix) 
                {
                    if (name != null) 
                    {
                        if (j > 1 && sli[j - 2].Typ == StreetItemType.Noun) 
                            break;
                        else if (i < j) 
                            break;
                        else 
                            return null;
                    }
                    name = sli[j];
                    rli.Add(sli[j]);
                }
                else if (sli[j].Typ == StreetItemType.StdPartOfName && j == n1) 
                {
                    if (name != null) 
                        break;
                    name = sli[j];
                    rli.Add(sli[j]);
                }
                else if (sli[j].Typ == StreetItemType.Noun) 
                {
                    if ((sli[0] == noun && ((noun.Termin.CanonicText == "УЛИЦА" || noun.Termin.CanonicText == "ВУЛИЦЯ")) && j > 0) && name == null) 
                    {
                        altNoun = noun;
                        noun = sli[j];
                        rli.Add(sli[j]);
                    }
                    else 
                        break;
                }
            }
            if (((n1 < i) && number == null && ((i + 1) < sli.Count)) && sli[i + 1].Typ == StreetItemType.Number && sli[i + 1].NumberHasPrefix) 
            {
                number = (sli[i + 1].Number == null ? sli[i + 1].Value : sli[i + 1].Number.IntValue.Value.ToString());
                rli.Add(sli[i + 1]);
            }
            else if ((((i < n0) && ((name != null || adj != null)) && (j < sli.Count)) && sli[j].Typ == StreetItemType.Noun && ((noun.Termin.CanonicText == "УЛИЦА" || noun.Termin.CanonicText == "ВУЛИЦЯ"))) && (((sli[j].Termin.CanonicText == "ПЛОЩАДЬ" || sli[j].Termin.CanonicText == "БУЛЬВАР" || sli[j].Termin.CanonicText == "ПЛОЩА") || sli[j].Termin.CanonicText == "МАЙДАН" || (j + 1) == sli.Count))) 
            {
                altNoun = noun;
                noun = sli[j];
                rli.Add(sli[j]);
            }
            if (name == null) 
            {
                if (number == null && adj == null) 
                    return null;
                if (noun.IsAbridge) 
                {
                    if (isMicroRaion) 
                    {
                    }
                    else if (noun.Termin != null && ((noun.Termin.CanonicText == "ПРОЕЗД" || noun.Termin.CanonicText == "ПРОЇЗД"))) 
                    {
                    }
                    else if (adj == null || adj.IsAbridge) 
                        return null;
                }
                if (adj != null && adj.IsAbridge) 
                    return null;
            }
            if (!rli.Contains(sli[i])) 
                rli.Add(sli[i]);
            Pullenti.Ner.Address.StreetReferent street = new Pullenti.Ner.Address.StreetReferent();
            if (!forMetro) 
            {
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, noun.Termin.CanonicText.ToLower(), false, 0);
                if (noun.AltTermin != null) 
                {
                    if (noun.AltTermin.CanonicText == "ПРОСПЕКТ" && number != null) 
                    {
                    }
                    else 
                        street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, noun.AltTermin.CanonicText.ToLower(), false, 0);
                }
            }
            else 
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, "метро", false, 0);
            AddressItemToken res = new AddressItemToken(AddressItemToken.ItemType.Street, rli[0].BeginToken, rli[0].EndToken) { Referent = street };
            foreach (StreetItemToken r in rli) 
            {
                if (res.BeginChar > r.BeginChar) 
                    res.BeginToken = r.BeginToken;
                if (res.EndChar < r.EndChar) 
                    res.EndToken = r.EndToken;
            }
            if (forMetro && rli.Contains(noun) && noun.Termin.CanonicText == "МЕТРО") 
                rli.Remove(noun);
            if (noun.IsAbridge && (noun.LengthChar < 4)) 
                res.IsDoubt = true;
            else if (noun.NounIsDoubtCoef > 0) 
            {
                res.IsDoubt = true;
                if ((name != null && name.EndChar > noun.EndChar && noun.Chars.IsAllLower) && !name.Chars.IsAllLower && !(name.BeginToken is Pullenti.Ner.ReferentToken)) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(name.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt2 != null && npt2.EndChar > name.EndChar) 
                    {
                    }
                    else if (AddressItemToken.CheckHouseAfter(res.EndToken.Next, false, false)) 
                        res.IsDoubt = false;
                    else if (name.Chars.IsCapitalUpper && noun.NounIsDoubtCoef == 1) 
                        res.IsDoubt = false;
                }
            }
            StringBuilder nameBase = new StringBuilder();
            StringBuilder nameAlt = new StringBuilder();
            string nameAlt2 = null;
            Pullenti.Morph.MorphGender gen = noun.Termin.Gender;
            Pullenti.Morph.MorphGender adjGen = Pullenti.Morph.MorphGender.Undefined;
            if (number != null) 
            {
                street.Number = number;
                if (secNumber != null) 
                    street.SecNumber = secNumber;
            }
            if (age != null) 
            {
                if (street.Number == null) 
                    street.Number = age;
                else 
                    street.SecNumber = age;
            }
            if (name != null && name.Value != null) 
            {
                if (street.Kind == Pullenti.Ner.Address.StreetKind.Road) 
                {
                    foreach (StreetItemToken r in rli) 
                    {
                        if (r.Typ == StreetItemType.Name && r != name) 
                        {
                            nameAlt.Append(r.Value);
                            break;
                        }
                    }
                }
                if (name.AltValue != null && nameAlt.Length == 0) 
                    nameAlt.AppendFormat("{0} {1}", nameBase.ToString(), name.AltValue);
                nameBase.AppendFormat(" {0}", name.Value);
            }
            else if (name != null) 
            {
                bool isAdj = false;
                if (name.EndToken is Pullenti.Ner.TextToken) 
                {
                    foreach (Pullenti.Morph.MorphBaseInfo wf in name.EndToken.Morph.Items) 
                    {
                        if ((wf is Pullenti.Morph.MorphWordForm) && (wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                        {
                            isAdj = wf.Class.IsAdjective | wf.Class.IsProperGeo;
                            adjGen = wf.Gender;
                            break;
                        }
                        else if (wf.Class.IsAdjective | wf.Class.IsProperGeo) 
                            isAdj = true;
                    }
                }
                if (isAdj) 
                {
                    StringBuilder tmp = new StringBuilder();
                    List<string> vars = new List<string>();
                    for (Pullenti.Ner.Token t = name.BeginToken; t != null; t = t.Next) 
                    {
                        Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                        if (tt == null) 
                            break;
                        if (tmp.Length > 0) 
                            tmp.Append(' ');
                        if (t == name.EndToken) 
                        {
                            bool isPadez = false;
                            if (!noun.IsAbridge) 
                            {
                                if (!noun.Morph.Case.IsUndefined && !noun.Morph.Case.IsNominative) 
                                    isPadez = true;
                                else if (noun.Termin.CanonicText == "ШОССЕ" || noun.Termin.CanonicText == "ШОСЕ") 
                                    isPadez = true;
                            }
                            if (res.BeginToken.Previous != null && res.BeginToken.Previous.Morph.Class.IsPreposition) 
                                isPadez = true;
                            if (!isPadez) 
                            {
                                tmp.Append(tt.Term);
                                break;
                            }
                            foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
                            {
                                if (((wf.Class.IsAdjective || wf.Class.IsProperGeo)) && ((wf.Gender & gen)) != Pullenti.Morph.MorphGender.Undefined) 
                                {
                                    if (noun.Morph.Case.IsUndefined || !((wf.Case & noun.Morph.Case)).IsUndefined) 
                                    {
                                        Pullenti.Morph.MorphWordForm wff = wf as Pullenti.Morph.MorphWordForm;
                                        if (wff == null) 
                                            continue;
                                        if (gen == Pullenti.Morph.MorphGender.Masculine && wff.NormalCase.Contains("ОЙ")) 
                                            continue;
                                        if (!vars.Contains(wff.NormalCase)) 
                                            vars.Add(wff.NormalCase);
                                    }
                                }
                            }
                            if (!vars.Contains(tt.Term) && sli.IndexOf(name) > sli.IndexOf(noun)) 
                                vars.Add(tt.Term);
                            if (vars.Count == 0) 
                                vars.Add(tt.Term);
                            break;
                        }
                        if (!tt.IsHiphen) 
                            tmp.Append(tt.Term);
                    }
                    if (vars.Count == 0) 
                        nameBase.AppendFormat(" {0}", tmp.ToString());
                    else 
                    {
                        string head = nameBase.ToString();
                        nameBase.AppendFormat(" {0}{1}", tmp.ToString(), vars[0]);
                        if (vars.Count > 1) 
                        {
                            nameAlt.Length = 0;
                            nameAlt.AppendFormat("{0} {1}{2}", head, tmp.ToString(), vars[1]);
                        }
                        if (vars.Count > 2) 
                            nameAlt2 = string.Format("{0} {1}{2}", head, tmp.ToString(), vars[2]);
                    }
                }
                else 
                {
                    string strNam = null;
                    List<string> nits = new List<string>();
                    bool hasAdj = false;
                    bool hasProperName = false;
                    for (Pullenti.Ner.Token t = name.BeginToken; t != null; t = t.Next) 
                    {
                        if (t.Morph.Class.IsAdjective || t.Morph.Class.IsConjunction) 
                            hasAdj = true;
                        if ((t is Pullenti.Ner.TextToken) && !t.IsHiphen) 
                        {
                            if (name.Termin != null) 
                            {
                                nits.Add(name.Termin.CanonicText);
                                break;
                            }
                            else if (!t.Chars.IsLetter && nits.Count > 0) 
                                nits[nits.Count - 1] += (t as Pullenti.Ner.TextToken).Term;
                            else 
                            {
                                nits.Add((t as Pullenti.Ner.TextToken).Term);
                                if (t == name.BeginToken && t.GetMorphClassInDictionary().IsProperName) 
                                    hasProperName = true;
                            }
                        }
                        else if ((t is Pullenti.Ner.ReferentToken) && name.Termin == null) 
                            nits.Add(t.GetSourceText().ToUpper());
                        if (t == name.EndToken) 
                            break;
                    }
                    if (!hasAdj && !hasProperName) 
                        nits.Sort();
                    strNam = string.Join(" ", nits.ToArray());
                    if (hasProperName && nits.Count == 2) 
                    {
                        nameAlt.Length = 0;
                        nameAlt.AppendFormat("{0} {1}", nameBase.ToString(), nits[1]);
                    }
                    nameBase.AppendFormat(" {0}", strNam);
                }
            }
            string adjStr = null;
            bool adjCanBeInitial = false;
            if (adj != null) 
            {
                string s;
                if (adjGen == Pullenti.Morph.MorphGender.Undefined && name != null && ((name.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) == Pullenti.Morph.MorphNumber.Undefined) 
                {
                    if (name.Morph.Gender == Pullenti.Morph.MorphGender.Feminie || name.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || name.Morph.Gender == Pullenti.Morph.MorphGender.Neuter) 
                        adjGen = name.Morph.Gender;
                }
                if (name != null && ((name.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) != Pullenti.Morph.MorphNumber.Undefined) 
                    s = Pullenti.Morph.MorphologyService.GetWordform(adj.Termin.CanonicText, new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective, Number = Pullenti.Morph.MorphNumber.Plural });
                else if (adjGen != Pullenti.Morph.MorphGender.Undefined) 
                    s = Pullenti.Morph.MorphologyService.GetWordform(adj.Termin.CanonicText, new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective, Gender = adjGen });
                else if (((adj.Morph.Gender & gen)) == Pullenti.Morph.MorphGender.Undefined) 
                    s = Pullenti.Morph.MorphologyService.GetWordform(adj.Termin.CanonicText, new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective, Gender = adj.Morph.Gender });
                else 
                    s = Pullenti.Morph.MorphologyService.GetWordform(adj.Termin.CanonicText, new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective, Gender = gen });
                adjStr = s;
                if (name != null && (sli.IndexOf(adj) < sli.IndexOf(name))) 
                {
                    if (adj.EndToken.IsChar('.') && adj.LengthChar <= 3 && !adj.BeginToken.Chars.IsAllLower) 
                        adjCanBeInitial = true;
                }
            }
            string s1 = nameBase.ToString().Trim();
            string s2 = nameAlt.ToString().Trim();
            if (s1.Length < 3) 
            {
                if (street.Number != null) 
                {
                    if (adjStr != null) 
                    {
                        if (adj.IsAbridge) 
                            return null;
                        street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, adjStr, false, 0);
                    }
                }
                else if (adjStr == null) 
                {
                    if (s1.Length < 1) 
                        return null;
                    if (isMicroRaion) 
                    {
                        street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, s1, false, 0);
                        if (!string.IsNullOrEmpty(s2)) 
                            street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, s2, false, 0);
                    }
                    else 
                        return null;
                }
                else 
                {
                    if (adj.IsAbridge) 
                        return null;
                    street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, adjStr, false, 0);
                }
            }
            else if (adjCanBeInitial) 
            {
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, s1, false, 0);
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, Pullenti.Ner.Core.MiscHelper.GetTextValue(adj.BeginToken, name.EndToken, Pullenti.Ner.Core.GetTextAttr.No), false, 0);
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, string.Format("{0} {1}", adjStr, s1), false, 0);
            }
            else if (adjStr == null) 
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, s1, false, 0);
            else 
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, string.Format("{0} {1}", adjStr, s1), false, 0);
            if (nameAlt.Length > 0) 
            {
                s1 = nameAlt.ToString().Trim();
                if (adjStr == null) 
                    street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, s1, false, 0);
                else 
                    street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, string.Format("{0} {1}", adjStr, s1), false, 0);
            }
            if (nameAlt2 != null) 
            {
                if (adjStr == null) 
                {
                    if (forMetro && noun != null) 
                        street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, string.Format("{0} {1}", altNoun.Termin.CanonicText, nameAlt2.Trim()), false, 0);
                    else 
                        street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, nameAlt2.Trim(), false, 0);
                }
                else 
                    street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, string.Format("{0} {1}", adjStr, nameAlt2.Trim()), false, 0);
            }
            if (name != null && name.AltValue2 != null) 
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, name.AltValue2, false, 0);
            if ((name != null && adj == null && name.ExistStreet != null) && !forMetro) 
            {
                foreach (string n in name.ExistStreet.Names) 
                {
                    street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, n, false, 0);
                }
            }
            if (altNoun != null && !forMetro) 
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, altNoun.Termin.CanonicText.ToLower(), false, 0);
            if (noun.Termin.CanonicText == "ПЛОЩАДЬ" || noun.Termin.CanonicText == "КВАРТАЛ" || noun.Termin.CanonicText == "ПЛОЩА") 
            {
                res.IsDoubt = true;
                if (name != null && name.IsInDictionary) 
                    res.IsDoubt = false;
                else if (altNoun != null || forMetro) 
                    res.IsDoubt = false;
                else if (res.BeginToken.Previous == null || Pullenti.Ner.Geo.Internal.MiscLocationHelper.CheckGeoObjectBefore(res.BeginToken.Previous)) 
                {
                    if (res.EndToken.Next == null || AddressItemToken.CheckHouseAfter(res.EndToken.Next, false, true)) 
                        res.IsDoubt = false;
                }
            }
            if (Pullenti.Morph.LanguageHelper.EndsWith(noun.Termin.CanonicText, "ГОРОДОК")) 
            {
                foreach (Pullenti.Ner.Slot s in street.Slots) 
                {
                    if (s.TypeName == Pullenti.Ner.Address.StreetReferent.ATTR_TYP) 
                        street.UploadSlot(s, "микрорайон");
                    else if (s.TypeName == Pullenti.Ner.Address.StreetReferent.ATTR_NAME) 
                        street.UploadSlot(s, string.Format("{0} {1}", noun.Termin.CanonicText, s.Value));
                }
                if (street.FindSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, null, true) == null) 
                    street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, noun.Termin.CanonicText, false, 0);
            }
            Pullenti.Ner.Token t1 = res.EndToken.Next;
            if (t1 != null && t1.IsComma) 
                t1 = t1.Next;
            StreetItemToken non = StreetItemToken.TryParse(t1, null, false, null, false);
            if (non != null && non.Typ == StreetItemType.Noun && street.Typs.Count > 0) 
            {
                if (AddressItemToken.CheckHouseAfter(non.EndToken.Next, false, true)) 
                {
                    street.Correct();
                    List<string> nams = street.Names;
                    foreach (string t in street.Typs) 
                    {
                        foreach (string n in nams) 
                        {
                            street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, string.Format("{0} {1}", t.ToUpper(), n), false, 0);
                        }
                    }
                    street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, non.Termin.CanonicText.ToLower(), false, 0);
                    res.EndToken = non.EndToken;
                }
            }
            if (res.IsDoubt) 
            {
                if (noun.IsRoad) 
                {
                    if (street.Number != null && street.Number.EndsWith("КМ", StringComparison.OrdinalIgnoreCase)) 
                        res.IsDoubt = false;
                    else if (AddressItemToken.CheckKmAfter(res.EndToken.Next)) 
                        res.IsDoubt = false;
                    else if (AddressItemToken.CheckKmBefore(res.BeginToken.Previous)) 
                        res.IsDoubt = false;
                }
                else if (noun.Termin.CanonicText == "ПРОЕЗД" && street.FindSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, "ПРОЕКТИРУЕМЫЙ", true) != null) 
                    res.IsDoubt = false;
                for (Pullenti.Ner.Token tt0 = res.BeginToken.Previous; tt0 != null; tt0 = tt0.Previous) 
                {
                    if (tt0.IsCharOf(",,") || tt0.IsCommaAnd) 
                        continue;
                    Pullenti.Ner.Address.StreetReferent str0 = tt0.GetReferent() as Pullenti.Ner.Address.StreetReferent;
                    if (str0 != null) 
                        res.IsDoubt = false;
                    break;
                }
            }
            if (noun.Termin.CanonicText == "КВАРТАЛ" && (res.WhitespacesAfterCount < 2) && number == null) 
            {
                AddressItemToken ait = AddressItemToken.TryParse(res.EndToken.Next, null, false, true, null);
                if (ait != null && ait.Typ == AddressItemToken.ItemType.Number && ait.Value != null) 
                {
                    street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NUMBER, ait.Value, false, 0);
                    res.EndToken = ait.EndToken;
                }
            }
            return res;
        }
        static AddressItemToken TryDetectNonNoun(List<StreetItemToken> sli, bool ontoRegim, bool forMetro)
        {
            if (sli.Count > 1 && sli[sli.Count - 1].Typ == StreetItemType.Number && !sli[sli.Count - 1].NumberHasPrefix) 
                sli.RemoveAt(sli.Count - 1);
            Pullenti.Ner.Address.StreetReferent street;
            if (sli.Count == 1 && sli[0].Typ == StreetItemType.Name && ((ontoRegim || forMetro))) 
            {
                string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(sli[0].BeginToken, sli[0].EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                if (s == null) 
                    return null;
                if (!forMetro && !sli[0].IsInDictionary && sli[0].ExistStreet == null) 
                {
                    Pullenti.Ner.Token tt = sli[0].EndToken.Next;
                    if (tt != null && tt.IsComma) 
                        tt = tt.Next;
                    AddressItemToken ait1 = AddressItemToken.TryParse(tt, null, true, false, null);
                    if (ait1 != null && ((ait1.Typ == AddressItemToken.ItemType.Number || ait1.Typ == AddressItemToken.ItemType.House))) 
                    {
                    }
                    else 
                        return null;
                }
                street = new Pullenti.Ner.Address.StreetReferent();
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, (forMetro ? "метро" : (sli[0].Kit.BaseLanguage.IsUa ? "вулиця" : "улица")), false, 0);
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, s, false, 0);
                AddressItemToken res0 = new AddressItemToken(AddressItemToken.ItemType.Street, sli[0].BeginToken, sli[0].EndToken) { Referent = street, IsDoubt = true };
                if (sli[0].IsInBrackets) 
                    res0.IsDoubt = false;
                return res0;
            }
            int i1 = 0;
            if (sli.Count == 1 && ((sli[0].Typ == StreetItemType.StdName || sli[0].Typ == StreetItemType.Name))) 
            {
                if (!ontoRegim) 
                {
                    bool isStreetBefore = false;
                    Pullenti.Ner.Token tt = sli[0].BeginToken.Previous;
                    if ((tt != null && tt.IsCommaAnd && tt.Previous != null) && (tt.Previous.GetReferent() is Pullenti.Ner.Address.StreetReferent)) 
                        isStreetBefore = true;
                    int cou = 0;
                    for (tt = sli[0].EndToken.Next; tt != null; tt = tt.Next) 
                    {
                        if (!tt.IsCommaAnd || tt.Next == null) 
                            break;
                        List<StreetItemToken> sli2 = StreetItemToken.TryParseList(tt.Next, null, 10);
                        if (sli2 == null) 
                            break;
                        StreetItemToken noun = null;
                        bool empty = true;
                        foreach (StreetItemToken si in sli2) 
                        {
                            if (si.Typ == StreetItemType.Noun) 
                                noun = si;
                            else if ((si.Typ == StreetItemType.Name || si.Typ == StreetItemType.StdName || si.Typ == StreetItemType.Number) || si.Typ == StreetItemType.StdAdjective) 
                                empty = false;
                        }
                        if (empty) 
                            break;
                        if (noun == null) 
                        {
                            if (tt.IsAnd && !isStreetBefore) 
                                break;
                            if ((++cou) > 4) 
                                break;
                            tt = sli2[sli2.Count - 1].EndToken;
                            continue;
                        }
                        if (!tt.IsAnd && !isStreetBefore) 
                            break;
                        List<StreetItemToken> tmp = new List<StreetItemToken>();
                        tmp.Add(sli[0]);
                        tmp.Add(noun);
                        AddressItemToken re = TryParseStreet(tmp, false, forMetro);
                        if (re != null) 
                        {
                            re.EndToken = tmp[0].EndToken;
                            return re;
                        }
                    }
                }
            }
            else if (sli.Count == 2 && ((sli[0].Typ == StreetItemType.StdAdjective || sli[0].Typ == StreetItemType.Number || sli[0].Typ == StreetItemType.Age)) && ((sli[1].Typ == StreetItemType.StdName || sli[1].Typ == StreetItemType.Name))) 
                i1 = 1;
            else if (sli.Count == 2 && ((sli[0].Typ == StreetItemType.StdName || sli[0].Typ == StreetItemType.Name)) && sli[1].Typ == StreetItemType.Number) 
                i1 = 0;
            else 
                return null;
            string val = sli[i1].Value;
            string altVal = sli[i1].AltValue;
            if (val == null) 
            {
                if (sli[i1].ExistStreet != null) 
                {
                    List<string> names = sli[i1].ExistStreet.Names;
                    if (names.Count > 0) 
                    {
                        val = names[0];
                        if (names.Count > 1) 
                            altVal = names[1];
                    }
                }
                else 
                {
                    Pullenti.Ner.TextToken te = sli[i1].BeginToken as Pullenti.Ner.TextToken;
                    if (te != null) 
                    {
                        foreach (Pullenti.Morph.MorphBaseInfo wf in te.Morph.Items) 
                        {
                            if (wf.Class.IsAdjective && wf.Gender == Pullenti.Morph.MorphGender.Feminie) 
                            {
                                val = (wf as Pullenti.Morph.MorphWordForm).NormalCase;
                                break;
                            }
                        }
                    }
                    if (i1 > 0 && sli[0].Typ == StreetItemType.Age) 
                        val = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(sli[i1], Pullenti.Ner.Core.GetTextAttr.No);
                }
            }
            bool veryDoubt = false;
            if (val == null && sli.Count == 1 && sli[0].Chars.IsCapitalUpper) 
            {
                veryDoubt = true;
                Pullenti.Ner.Token t0 = sli[0].BeginToken.Previous;
                if (t0 != null && t0.IsChar(',')) 
                    t0 = t0.Previous;
                if ((t0 is Pullenti.Ner.ReferentToken) && (t0.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    val = Pullenti.Ner.Core.MiscHelper.GetTextValue(sli[0].BeginToken, sli[0].EndToken, Pullenti.Ner.Core.GetTextAttr.No);
            }
            if (val == null) 
                return null;
            Pullenti.Ner.Token t = sli[sli.Count - 1].EndToken.Next;
            if (t != null && t.IsChar(',')) 
                t = t.Next;
            if (t == null || t.IsNewlineBefore) 
                return null;
            bool ok = false;
            bool doubt = true;
            if (sli[i1].Termin != null && ((StreetItemType)sli[i1].Termin.Tag) == StreetItemType.Fix) 
            {
                ok = true;
                doubt = false;
            }
            else if (((sli[i1].ExistStreet != null || sli[0].ExistStreet != null)) && sli[0].BeginToken != sli[i1].EndToken) 
            {
                ok = true;
                doubt = false;
                if (t.Kit.ProcessReferent("PERSON", sli[0].BeginToken) != null) 
                {
                    if (AddressItemToken.CheckHouseAfter(t, false, false)) 
                    {
                    }
                    else 
                        doubt = true;
                }
            }
            else if (AddressItemToken.CheckHouseAfter(t, false, false)) 
            {
                if (t.Previous != null) 
                {
                    if (t.Previous.IsValue("АРЕНДА", "ОРЕНДА") || t.Previous.IsValue("СДАЧА", "ЗДАЧА") || t.Previous.IsValue("СЪЕМ", "ЗНІМАННЯ")) 
                        return null;
                }
                Pullenti.Ner.Core.NounPhraseToken vv = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (vv != null && vv.EndChar >= t.BeginChar) 
                    return null;
                ok = true;
            }
            else 
            {
                AddressItemToken ait = AddressItemToken.TryParse(t, null, true, false, null);
                if (ait == null) 
                    return null;
                if (ait.Typ == AddressItemToken.ItemType.House && ait.Value != null) 
                    ok = true;
                else if (veryDoubt) 
                    return null;
                else if (((val == "ТАБЛИЦА" || val == "РИСУНОК" || val == "ДИАГРАММА") || val == "ТАБЛИЦЯ" || val == "МАЛЮНОК") || val == "ДІАГРАМА") 
                    return null;
                else if (ait.Typ == AddressItemToken.ItemType.Number && (ait.BeginToken.WhitespacesBeforeCount < 2)) 
                {
                    Pullenti.Ner.NumberToken nt = ait.BeginToken as Pullenti.Ner.NumberToken;
                    if ((nt == null || nt.IntValue == null || nt.Typ != Pullenti.Ner.NumberSpellingType.Digit) || nt.Morph.Class.IsAdjective) 
                        return null;
                    if (ait.EndToken.Next != null && !ait.EndToken.IsNewlineAfter) 
                    {
                        Pullenti.Morph.MorphClass mc = ait.EndToken.Next.GetMorphClassInDictionary();
                        if (mc.IsAdjective || mc.IsNoun) 
                            return null;
                    }
                    if (nt.IntValue.Value > 100) 
                        return null;
                    Pullenti.Ner.Core.NumberExToken nex = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(ait.BeginToken);
                    if (nex != null) 
                        return null;
                    for (t = sli[0].BeginToken.Previous; t != null; t = t.Previous) 
                    {
                        if (t.IsNewlineAfter) 
                            break;
                        if (t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                        {
                            ok = true;
                            break;
                        }
                        if (t.IsChar(',')) 
                            continue;
                        if (t.IsChar('.')) 
                            break;
                        AddressItemToken ait0 = AddressItemToken.TryParse(t, null, false, true, null);
                        if (ait != null) 
                        {
                            if (ait.Typ == AddressItemToken.ItemType.Prefix) 
                            {
                                ok = true;
                                break;
                            }
                        }
                        if (t.Chars.IsLetter) 
                            break;
                    }
                }
            }
            if (!ok) 
                return null;
            AddressItemToken ooo = AddressItemToken.TryAttachOrg(sli[0].BeginToken);
            if (ooo == null && sli.Count > 1) 
                ooo = AddressItemToken.TryAttachOrg(sli[1].BeginToken);
            if (ooo != null) 
                return null;
            street = new Pullenti.Ner.Address.StreetReferent();
            street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, (sli[0].Kit.BaseLanguage.IsUa ? "вулиця" : "улица"), false, 0);
            if (sli.Count > 1) 
            {
                if (sli[0].Typ == StreetItemType.Number || sli[0].Typ == StreetItemType.Age) 
                    street.Number = (sli[0].Number == null ? sli[0].Value : sli[0].Number.IntValue.Value.ToString());
                else if (sli[1].Typ == StreetItemType.Number || sli[1].Typ == StreetItemType.Age) 
                    street.Number = (sli[1].Number == null ? sli[1].Value : sli[1].Number.IntValue.Value.ToString());
                else 
                {
                    List<string> adjs = Pullenti.Ner.Geo.Internal.MiscLocationHelper.GetStdAdjFull(sli[0].BeginToken, sli[1].Morph.Gender, sli[1].Morph.Number, true);
                    if (adjs == null) 
                        adjs = Pullenti.Ner.Geo.Internal.MiscLocationHelper.GetStdAdjFull(sli[0].BeginToken, Pullenti.Morph.MorphGender.Feminie, Pullenti.Morph.MorphNumber.Singular, false);
                    if (adjs != null) 
                    {
                        if (adjs.Count > 1) 
                            altVal = string.Format("{0} {1}", adjs[1], val);
                        val = string.Format("{0} {1}", adjs[0], val);
                    }
                }
            }
            street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, val, false, 0);
            if (altVal != null) 
                street.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, altVal, false, 0);
            return new AddressItemToken(AddressItemToken.ItemType.Street, sli[0].BeginToken, sli[sli.Count - 1].EndToken) { Referent = street, IsDoubt = doubt };
        }
        static AddressItemToken _tryParseFix(List<StreetItemToken> sits)
        {
            if ((sits.Count < 1) || sits[0].Termin == null) 
                return null;
            if (sits[0].Termin.Acronym == "МКАД") 
            {
                Pullenti.Ner.Address.StreetReferent str = new Pullenti.Ner.Address.StreetReferent();
                str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, "автодорога", false, 0);
                str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, "МОСКОВСКАЯ КОЛЬЦЕВАЯ", false, 0);
                Pullenti.Ner.Token t0 = sits[0].BeginToken;
                Pullenti.Ner.Token t1 = sits[0].EndToken;
                if (sits.Count > 1 && sits[1].Typ == StreetItemType.Number) 
                {
                    string num = (sits[1].Number == null ? sits[1].Value : sits[1].Number.IntValue.Value.ToString());
                    if (t0.Previous != null && ((t0.Previous.IsValue("КИЛОМЕТР", null) || t0.Previous.IsValue("КМ", null)))) 
                    {
                        t0 = t0.Previous;
                        str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NUMBER, num + "км", false, 0);
                        t1 = sits[1].EndToken;
                    }
                    else if (sits[1].IsNumberKm) 
                    {
                        str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NUMBER, num + "км", false, 0);
                        t1 = sits[1].EndToken;
                    }
                }
                return new AddressItemToken(AddressItemToken.ItemType.Street, t0, t1) { Referent = str };
            }
            if (Pullenti.Ner.Geo.Internal.MiscLocationHelper.CheckGeoObjectBefore(sits[0].BeginToken) || AddressItemToken.CheckHouseAfter(sits[0].EndToken.Next, false, true)) 
            {
                Pullenti.Ner.Address.StreetReferent str = new Pullenti.Ner.Address.StreetReferent();
                str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, "улица", false, 0);
                str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, sits[0].Termin.CanonicText, false, 0);
                return new AddressItemToken(AddressItemToken.ItemType.Street, sits[0].BeginToken, sits[0].EndToken) { Referent = str };
            }
            return null;
        }
        internal static AddressItemToken TryParseSecondStreet(Pullenti.Ner.Token t1, Pullenti.Ner.Token t2, Pullenti.Ner.Core.IntOntologyCollection locStreets)
        {
            List<StreetItemToken> sli = StreetItemToken.TryParseList(t1, locStreets, 10);
            if (sli == null || (sli.Count < 1) || sli[0].Typ != StreetItemType.Noun) 
                return null;
            List<StreetItemToken> sli2 = StreetItemToken.TryParseList(t2, locStreets, 10);
            if (sli2 == null || sli2.Count == 0) 
                return null;
            sli2.Insert(0, sli[0]);
            AddressItemToken res = TryParseStreet(sli2, true, false);
            if (res == null) 
                return null;
            res.BeginToken = sli2[1].BeginToken;
            return res;
        }
    }
}