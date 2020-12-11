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
    public class AddressItemToken : Pullenti.Ner.MetaToken
    {
        public AddressItemToken(ItemType typ, Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
            Typ = typ;
        }
        public enum ItemType : int
        {
            Prefix,
            Street,
            House,
            Building,
            Corpus,
            Potch,
            Floor,
            Flat,
            CorpusOrFlat,
            Office,
            Plot,
            Block,
            Box,
            City,
            Region,
            Country,
            Number,
            NoNumber,
            Kilometer,
            Zip,
            PostOfficeBox,
            CSP,
            Detail,
            BusinessCenter,
        }

        public ItemType Typ;
        public string Value;
        public Pullenti.Ner.Referent Referent;
        public Pullenti.Ner.ReferentToken RefToken;
        public bool RefTokenIsGsk;
        public bool IsDoubt;
        public Pullenti.Ner.Address.AddressDetailType DetailType = Pullenti.Ner.Address.AddressDetailType.Undefined;
        public Pullenti.Ner.Address.AddressBuildingType BuildingType = Pullenti.Ner.Address.AddressBuildingType.Undefined;
        public Pullenti.Ner.Address.AddressHouseType HouseType = Pullenti.Ner.Address.AddressHouseType.Undefined;
        public int DetailMeters = 0;
        public bool IsStreetRoad
        {
            get
            {
                if (Typ != ItemType.Street) 
                    return false;
                if (!(Referent is Pullenti.Ner.Address.StreetReferent)) 
                    return false;
                return (Referent as Pullenti.Ner.Address.StreetReferent).Kind == Pullenti.Ner.Address.StreetKind.Road;
            }
        }
        public bool IsTerrOrRzd
        {
            get
            {
                if (Typ == ItemType.City && (Referent is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    if ((Referent as Pullenti.Ner.Geo.GeoReferent).IsTerritory) 
                        return true;
                }
                return false;
            }
        }
        public bool IsDigit
        {
            get
            {
                if (Value == "Б/Н") 
                    return true;
                if (string.IsNullOrEmpty(Value)) 
                    return false;
                if (char.IsDigit(Value[0])) 
                    return true;
                if (Value.Length > 1) 
                {
                    if (char.IsLetter(Value[0]) && char.IsDigit(Value[1])) 
                        return true;
                }
                if (Value.Length != 1 || !char.IsLetter(Value[0])) 
                    return false;
                if (!BeginToken.Chars.IsAllLower) 
                    return false;
                return true;
            }
        }
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0} {1}", Typ.ToString(), Value ?? "");
            if (Referent != null) 
                res.AppendFormat(" <{0}>", Referent.ToString());
            if (DetailType != Pullenti.Ner.Address.AddressDetailType.Undefined) 
                res.AppendFormat(" [{0}, {1}]", DetailType, DetailMeters);
            return res.ToString();
        }
        public static List<AddressItemToken> TryParseList(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locStreets, int maxCount = 20)
        {
            if (t is Pullenti.Ner.NumberToken) 
            {
                if ((t as Pullenti.Ner.NumberToken).IntValue == null) 
                    return null;
                int v = (t as Pullenti.Ner.NumberToken).IntValue.Value;
                if ((v < 100000) || v >= 10000000) 
                {
                    if ((t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit && !t.Morph.Class.IsAdjective) 
                    {
                        if (t.Next == null || (t.Next is Pullenti.Ner.NumberToken)) 
                        {
                            if (t.Previous == null || !t.Previous.Morph.Class.IsPreposition) 
                                return null;
                        }
                    }
                }
            }
            AddressItemToken it = TryParse(t, locStreets, false, false, null);
            if (it == null) 
                return null;
            if (it.Typ == ItemType.Number) 
                return null;
            if (it.Typ == ItemType.Kilometer && (it.BeginToken.Previous is Pullenti.Ner.NumberToken)) 
            {
                it.BeginToken = it.BeginToken.Previous;
                it.Value = (it.BeginToken as Pullenti.Ner.NumberToken).Value.ToString();
                if (it.BeginToken.Previous != null && it.BeginToken.Previous.Morph.Class.IsPreposition) 
                    it.BeginToken = it.BeginToken.Previous;
            }
            List<AddressItemToken> res = new List<AddressItemToken>();
            res.Add(it);
            bool pref = it.Typ == ItemType.Prefix;
            for (t = it.EndToken.Next; t != null; t = t.Next) 
            {
                if (maxCount > 0 && res.Count >= maxCount) 
                    break;
                AddressItemToken last = res[res.Count - 1];
                if (res.Count > 1) 
                {
                    if (last.IsNewlineBefore && res[res.Count - 2].Typ != ItemType.Prefix) 
                    {
                        int i;
                        for (i = 0; i < (res.Count - 1); i++) 
                        {
                            if (res[i].Typ == last.Typ) 
                            {
                                if (i == (res.Count - 2) && ((last.Typ == ItemType.City || last.Typ == ItemType.Region))) 
                                {
                                    int jj;
                                    for (jj = 0; jj < i; jj++) 
                                    {
                                        if ((res[jj].Typ != ItemType.Prefix && res[jj].Typ != ItemType.Zip && res[jj].Typ != ItemType.Region) && res[jj].Typ != ItemType.Country) 
                                            break;
                                    }
                                    if (jj >= i) 
                                        continue;
                                }
                                break;
                            }
                        }
                        if ((i < (res.Count - 1)) || last.Typ == ItemType.Zip) 
                        {
                            res.Remove(last);
                            break;
                        }
                    }
                }
                if (t.IsTableControlChar) 
                    break;
                if (t.IsChar(',')) 
                    continue;
                if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t, true, null, false) && last.Typ == ItemType.Street) 
                    continue;
                if (t.IsChar('.')) 
                {
                    if (t.IsNewlineAfter) 
                        break;
                    if (t.Previous != null && t.Previous.IsChar('.')) 
                        break;
                    continue;
                }
                if (t.IsHiphen || t.IsChar('_')) 
                {
                    if (((it.Typ == ItemType.Number || it.Typ == ItemType.Street)) && (t.Next is Pullenti.Ner.NumberToken)) 
                        continue;
                }
                if (it.Typ == ItemType.Detail && it.DetailType == Pullenti.Ner.Address.AddressDetailType.Cross) 
                {
                    AddressItemToken str1 = TryParse(t, locStreets, true, false, null);
                    if (str1 != null && str1.Typ == ItemType.Street) 
                    {
                        if (str1.EndToken.Next != null && ((str1.EndToken.Next.IsAnd || str1.EndToken.Next.IsHiphen))) 
                        {
                            AddressItemToken str2 = TryParse(str1.EndToken.Next.Next, locStreets, true, false, null);
                            if (str2 == null || str2.Typ != ItemType.Street) 
                            {
                                str2 = StreetDefineHelper.TryParseSecondStreet(str1.BeginToken, str1.EndToken.Next.Next, locStreets);
                                if (str2 != null) 
                                    str2.IsDoubt = false;
                            }
                            if (str2 != null && str2.Typ == ItemType.Street) 
                            {
                                res.Add(str1);
                                res.Add(str2);
                                t = str2.EndToken;
                                it = str2;
                                continue;
                            }
                        }
                    }
                }
                bool pre = pref;
                if (it.Typ == ItemType.Kilometer || it.Typ == ItemType.House) 
                {
                    if (!t.IsNewlineBefore) 
                        pre = true;
                }
                AddressItemToken it0 = TryParse(t, locStreets, pre, false, it);
                if (it0 == null) 
                {
                    bool ok2 = true;
                    if (it.Typ == ItemType.Building && it.BeginToken.IsValue("СТ", null)) 
                        ok2 = false;
                    else 
                        foreach (AddressItemToken rr in res) 
                        {
                            if (rr.Typ == ItemType.Building && rr.BeginToken.IsValue("СТ", null)) 
                                ok2 = false;
                        }
                    if (it.Typ == ItemType.PostOfficeBox) 
                        break;
                    if (ok2) 
                        it0 = TryAttachOrg(t);
                    if (it0 != null) 
                    {
                        res.Add(it0);
                        it = it0;
                        t = it.EndToken;
                        for (Pullenti.Ner.Token tt1 = t.Next; tt1 != null; tt1 = tt1.Next) 
                        {
                            if (tt1.IsComma) 
                            {
                            }
                            else 
                            {
                                if (tt1.IsValue("Л", null) && tt1.Next != null && tt1.Next.IsChar('.')) 
                                {
                                    AddressItemToken ait = AddressItemToken.TryParse(tt1.Next.Next, null, false, true, null);
                                    if (ait != null && ait.Typ == ItemType.Number) 
                                    {
                                        Pullenti.Ner.Address.StreetReferent st2 = new Pullenti.Ner.Address.StreetReferent();
                                        st2.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, "линия", false, 0);
                                        st2.Number = ait.Value;
                                        res.Add((it = new AddressItemToken(ItemType.Street, tt1, ait.EndToken) { Referent = st2 }));
                                        t = it.EndToken;
                                    }
                                }
                                break;
                            }
                        }
                        continue;
                    }
                    if (t.Morph.Class.IsPreposition) 
                    {
                        it0 = TryParse(t.Next, locStreets, false, false, it);
                        if (it0 != null && it0.Typ == ItemType.Building && it0.BeginToken.IsValue("СТ", null)) 
                        {
                            it0 = null;
                            break;
                        }
                        if (it0 != null) 
                        {
                            if ((it0.Typ == ItemType.House || it0.Typ == ItemType.Building || it0.Typ == ItemType.Corpus) || it0.Typ == ItemType.Street) 
                            {
                                res.Add((it = it0));
                                t = it.EndToken;
                                continue;
                            }
                        }
                    }
                    if (it.Typ == ItemType.House || it.Typ == ItemType.Building || it.Typ == ItemType.Number) 
                    {
                        if ((!t.IsWhitespaceBefore && t.LengthChar == 1 && t.Chars.IsLetter) && !t.IsWhitespaceAfter && (t.Next is Pullenti.Ner.NumberToken)) 
                        {
                            string ch = CorrectCharToken(t);
                            if (ch == "К" || ch == "С") 
                            {
                                it0 = new AddressItemToken((ch == "К" ? ItemType.Corpus : ItemType.Building), t, t.Next) { Value = (t.Next as Pullenti.Ner.NumberToken).Value.ToString() };
                                it = it0;
                                res.Add(it);
                                t = it.EndToken;
                                Pullenti.Ner.Token tt = t.Next;
                                if (((tt != null && !tt.IsWhitespaceBefore && tt.LengthChar == 1) && tt.Chars.IsLetter && !tt.IsWhitespaceAfter) && (tt.Next is Pullenti.Ner.NumberToken)) 
                                {
                                    ch = CorrectCharToken(tt);
                                    if (ch == "К" || ch == "С") 
                                    {
                                        it = new AddressItemToken((ch == "К" ? ItemType.Corpus : ItemType.Building), tt, tt.Next) { Value = (tt.Next as Pullenti.Ner.NumberToken).Value.ToString() };
                                        res.Add(it);
                                        t = it.EndToken;
                                    }
                                }
                                continue;
                            }
                        }
                    }
                    if (t.Morph.Class.IsPreposition) 
                    {
                        if ((((t.IsValue("У", null) || t.IsValue("ВОЗЛЕ", null) || t.IsValue("НАПРОТИВ", null)) || t.IsValue("НА", null) || t.IsValue("В", null)) || t.IsValue("ВО", null) || t.IsValue("ПО", null)) || t.IsValue("ОКОЛО", null)) 
                            continue;
                    }
                    if (t.Morph.Class.IsNoun) 
                    {
                        if ((t.IsValue("ДВОР", null) || t.IsValue("ПОДЪЕЗД", null) || t.IsValue("КРЫША", null)) || t.IsValue("ПОДВАЛ", null)) 
                            continue;
                    }
                    if (t.IsValue("ТЕРРИТОРИЯ", "ТЕРИТОРІЯ")) 
                        continue;
                    if (t.IsChar('(') && t.Next != null) 
                    {
                        it0 = TryParse(t.Next, locStreets, pre, false, null);
                        if (it0 != null && it0.EndToken.Next != null && it0.EndToken.Next.IsChar(')')) 
                        {
                            it0.BeginToken = t;
                            it0.EndToken = it0.EndToken.Next;
                            it = it0;
                            res.Add(it);
                            t = it.EndToken;
                            continue;
                        }
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null && (br.LengthChar < 100)) 
                        {
                            if (t.Next.IsValue("БЫВШИЙ", null) || t.Next.IsValue("БЫВШ", null)) 
                            {
                                it = new AddressItemToken(ItemType.Detail, t, br.EndToken);
                                res.Add(it);
                            }
                            t = br.EndToken;
                            continue;
                        }
                    }
                    bool checkKv = false;
                    if (t.IsValue("КВ", null) || t.IsValue("KB", null)) 
                    {
                        if (it.Typ == ItemType.Number && res.Count > 1 && res[res.Count - 2].Typ == ItemType.Street) 
                            checkKv = true;
                        else if ((it.Typ == ItemType.House || it.Typ == ItemType.Building || it.Typ == ItemType.Corpus) || it.Typ == ItemType.CorpusOrFlat) 
                        {
                            for (int jj = res.Count - 2; jj >= 0; jj--) 
                            {
                                if (res[jj].Typ == ItemType.Street || res[jj].Typ == ItemType.City) 
                                    checkKv = true;
                            }
                        }
                        if (checkKv) 
                        {
                            Pullenti.Ner.Token tt2 = t.Next;
                            if (tt2 != null && tt2.IsChar('.')) 
                                tt2 = tt2.Next;
                            AddressItemToken it22 = TryParse(tt2, locStreets, false, true, null);
                            if (it22 != null && it22.Typ == ItemType.Number) 
                            {
                                it22.BeginToken = t;
                                it22.Typ = ItemType.Flat;
                                res.Add(it22);
                                t = it22.EndToken;
                                continue;
                            }
                        }
                    }
                    if (res[res.Count - 1].Typ == ItemType.City) 
                    {
                        if (((t.IsHiphen || t.IsChar('_') || t.IsValue("НЕТ", null))) && t.Next != null && t.Next.IsComma) 
                        {
                            AddressItemToken att = _TryParse(t.Next.Next, null, false, true, null);
                            if (att != null) 
                            {
                                if (att.Typ == ItemType.House || att.Typ == ItemType.Building || att.Typ == ItemType.Corpus) 
                                {
                                    it = new AddressItemToken(ItemType.Street, t, t);
                                    res.Add(it);
                                    continue;
                                }
                            }
                        }
                    }
                    if (t.LengthChar == 2 && (t is Pullenti.Ner.TextToken) && t.Chars.IsAllUpper) 
                    {
                        string term = (t as Pullenti.Ner.TextToken).Term;
                        if (!string.IsNullOrEmpty(term) && term[0] == 'Р') 
                            continue;
                    }
                    break;
                }
                if (t.WhitespacesBeforeCount > 15) 
                {
                    if (it0.Typ == ItemType.Street && last.Typ == ItemType.City) 
                    {
                    }
                    else 
                        break;
                }
                if (it0.Typ == ItemType.Street && t.IsValue("КВ", null)) 
                {
                    if (it != null) 
                    {
                        if (it.Typ == ItemType.House || it.Typ == ItemType.Building || it.Typ == ItemType.Corpus) 
                        {
                            AddressItemToken it2 = TryParse(t, locStreets, false, true, null);
                            if (it2 != null && it2.Typ == ItemType.Flat) 
                                it0 = it2;
                        }
                    }
                }
                if (it0.Typ == ItemType.Prefix) 
                    break;
                if (it0.Typ == ItemType.Number) 
                {
                    if (string.IsNullOrEmpty(it0.Value)) 
                        break;
                    if (!char.IsDigit(it0.Value[0])) 
                        break;
                    int cou = 0;
                    for (int i = res.Count - 1; i >= 0; i--) 
                    {
                        if (res[i].Typ == ItemType.Number) 
                            cou++;
                        else 
                            break;
                    }
                    if (cou > 5) 
                        break;
                    if (it.IsDoubt && t.IsNewlineBefore) 
                        break;
                }
                if (it0.Typ == ItemType.CorpusOrFlat && it != null && it.Typ == ItemType.Flat) 
                    it0.Typ = ItemType.Office;
                if ((((it0.Typ == ItemType.Floor || it0.Typ == ItemType.Potch || it0.Typ == ItemType.Block) || it0.Typ == ItemType.Kilometer)) && string.IsNullOrEmpty(it0.Value) && it.Typ == ItemType.Number) 
                {
                    it.Typ = it0.Typ;
                    it.EndToken = it0.EndToken;
                }
                else if (((it.Typ == ItemType.Floor || it.Typ == ItemType.Potch)) && string.IsNullOrEmpty(it.Value) && it0.Typ == ItemType.Number) 
                {
                    it.Value = it0.Value;
                    it.EndToken = it0.EndToken;
                }
                else 
                {
                    it = it0;
                    res.Add(it);
                }
                t = it.EndToken;
            }
            if (res.Count > 0) 
            {
                it = res[res.Count - 1];
                AddressItemToken it0 = (res.Count > 1 ? res[res.Count - 2] : null);
                if (it.Typ == ItemType.Number && it0 != null && it0.RefToken != null) 
                {
                    foreach (Pullenti.Ner.Slot s in it0.RefToken.Referent.Slots) 
                    {
                        if (s.TypeName == "TYPE") 
                        {
                            string ss = s.Value as string;
                            if (ss.Contains("гараж") || ((ss[0] == 'Г' && ss[ss.Length - 1] == 'К'))) 
                            {
                                it.Typ = ItemType.Box;
                                break;
                            }
                        }
                    }
                }
                if (it.Typ == ItemType.Number || it.Typ == ItemType.Zip) 
                {
                    bool del = false;
                    if (it.BeginToken.Previous != null && it.BeginToken.Previous.Morph.Class.IsPreposition) 
                        del = true;
                    else if (it.Morph.Class.IsNoun) 
                        del = true;
                    if ((!del && it.EndToken.WhitespacesAfterCount == 1 && it.WhitespacesBeforeCount > 0) && it.Typ == ItemType.Number) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(it.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null) 
                            del = true;
                    }
                    if (del) 
                        res.RemoveAt(res.Count - 1);
                    else if ((it.Typ == ItemType.Number && it0 != null && it0.Typ == ItemType.Street) && it0.RefToken == null) 
                    {
                        if (it.BeginToken.Previous.IsChar(',') || it.IsNewlineAfter) 
                            it.Typ = ItemType.House;
                    }
                }
            }
            if (res.Count == 0) 
                return null;
            foreach (AddressItemToken r in res) 
            {
                if (r.Typ == ItemType.City || r.Typ == ItemType.Region) 
                {
                    AddressItemToken ty = _findAddrTyp(r.BeginToken, r.EndChar, 0);
                    if (ty != null) 
                    {
                        r.DetailType = ty.DetailType;
                        if (ty.DetailMeters > 0) 
                            r.DetailMeters = ty.DetailMeters;
                    }
                }
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (res[i].IsTerrOrRzd && res[i + 1].Typ == ItemType.Kilometer && (((i + 1) >= res.Count || !res[i + 1].IsTerrOrRzd))) 
                {
                    Pullenti.Ner.Address.StreetReferent str = new Pullenti.Ner.Address.StreetReferent();
                    str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, "километр", true, 0);
                    str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, res[i].Referent.GetStringValue(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME), false, 0);
                    str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_GEO, res[i].Referent, false, 0);
                    str.Number = res[i + 1].Value;
                    Pullenti.Ner.Token t11 = res[i + 1].EndToken;
                    bool remove2 = false;
                    if ((res[i].Value == null && ((i + 2) < res.Count) && res[i + 2].Typ == ItemType.Number) && res[i + 2].Value != null) 
                    {
                        str.Number = res[i + 2].Value + "км";
                        t11 = res[i + 2].EndToken;
                        remove2 = true;
                    }
                    AddressItemToken ai = new AddressItemToken(ItemType.Street, res[i].BeginToken, t11) { Referent = str, IsDoubt = false };
                    res[i] = ai;
                    res.RemoveAt(i + 1);
                    if (remove2) 
                        res.RemoveAt(i + 1);
                }
                else if (res[i + 1].IsTerrOrRzd && res[i].Typ == ItemType.Kilometer) 
                {
                    Pullenti.Ner.Address.StreetReferent str = new Pullenti.Ner.Address.StreetReferent();
                    str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, "километр", true, 0);
                    str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, res[i + 1].Referent.GetStringValue(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME), false, 0);
                    str.AddSlot(Pullenti.Ner.Address.StreetReferent.ATTR_GEO, res[i + 1].Referent, false, 0);
                    str.Number = res[i].Value;
                    Pullenti.Ner.Token t11 = res[i + 1].EndToken;
                    bool remove2 = false;
                    if ((res[i].Value == null && ((i + 2) < res.Count) && res[i + 2].Typ == ItemType.Number) && res[i + 2].Value != null) 
                    {
                        str.Number = res[i + 2].Value + "км";
                        t11 = res[i + 2].EndToken;
                        remove2 = true;
                    }
                    AddressItemToken ai = new AddressItemToken(ItemType.Street, res[i].BeginToken, t11) { Referent = str, IsDoubt = false };
                    res[i] = ai;
                    res.RemoveAt(i + 1);
                    if (remove2) 
                        res.RemoveAt(i + 1);
                }
            }
            for (int i = 0; i < (res.Count - 2); i++) 
            {
                if (res[i].Typ == ItemType.Street && res[i + 1].Typ == ItemType.Number) 
                {
                    if ((res[i + 2].Typ == ItemType.BusinessCenter || res[i + 2].Typ == ItemType.Building || res[i + 2].Typ == ItemType.Corpus) || res[i + 2].Typ == ItemType.Office || res[i + 2].Typ == ItemType.Flat) 
                        res[i + 1].Typ = ItemType.House;
                }
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if ((res[i].Typ == ItemType.Street && res[i + 1].Typ == ItemType.Kilometer && (res[i].Referent is Pullenti.Ner.Address.StreetReferent)) && (res[i].Referent as Pullenti.Ner.Address.StreetReferent).Number == null) 
                {
                    (res[i].Referent as Pullenti.Ner.Address.StreetReferent).Number = res[i + 1].Value + "км";
                    res[i].EndToken = res[i + 1].EndToken;
                    res.RemoveAt(i + 1);
                }
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if ((res[i + 1].Typ == ItemType.Street && res[i].Typ == ItemType.Kilometer && (res[i + 1].Referent is Pullenti.Ner.Address.StreetReferent)) && (res[i + 1].Referent as Pullenti.Ner.Address.StreetReferent).Number == null) 
                {
                    (res[i + 1].Referent as Pullenti.Ner.Address.StreetReferent).Number = res[i].Value + "км";
                    res[i + 1].BeginToken = res[i].BeginToken;
                    res.RemoveAt(i);
                    break;
                }
            }
            return res;
        }
        static AddressItemToken _findAddrTyp(Pullenti.Ner.Token t, int maxChar, int lev = 0)
        {
            if (t == null || t.EndChar > maxChar) 
                return null;
            if (lev > 5) 
                return null;
            if (t is Pullenti.Ner.ReferentToken) 
            {
                Pullenti.Ner.Geo.GeoReferent geo = t.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                if (geo != null) 
                {
                    foreach (Pullenti.Ner.Slot s in geo.Slots) 
                    {
                        if (s.TypeName == Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE) 
                        {
                            string ty = (string)s.Value;
                            if (ty.Contains("район")) 
                                return null;
                        }
                    }
                }
                for (Pullenti.Ner.Token tt = (t as Pullenti.Ner.ReferentToken).BeginToken; tt != null; tt = tt.Next) 
                {
                    if (tt.EndChar > maxChar) 
                        break;
                    AddressItemToken ty = _findAddrTyp(tt, maxChar, lev + 1);
                    if (ty != null) 
                        return ty;
                }
            }
            else 
            {
                AddressItemToken ai = TryAttachDetail(t);
                if (ai != null) 
                {
                    if (ai.DetailType != Pullenti.Ner.Address.AddressDetailType.Undefined || ai.DetailMeters > 0) 
                        return ai;
                }
            }
            return null;
        }
        public static AddressItemToken TryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locStreets, bool prefixBefore, bool ignoreStreet = false, AddressItemToken prev = null)
        {
            if (t == null) 
                return null;
            if (t.Kit.IsRecurceOverflow) 
                return null;
            t.Kit.RecurseLevel++;
            AddressItemToken res = _TryParse(t, locStreets, prefixBefore, ignoreStreet, prev);
            t.Kit.RecurseLevel--;
            if (((res != null && !res.IsWhitespaceAfter && res.EndToken.Next != null) && res.EndToken.Next.IsHiphen && !res.EndToken.Next.IsWhitespaceAfter) && res.Value != null) 
            {
                if (res.Typ == ItemType.House || res.Typ == ItemType.Building || res.Typ == ItemType.Corpus) 
                {
                    Pullenti.Ner.Token tt = res.EndToken.Next.Next;
                    if (tt is Pullenti.Ner.NumberToken) 
                    {
                        res.Value = string.Format("{0}-{1}", res.Value, (tt as Pullenti.Ner.NumberToken).Value);
                        res.EndToken = tt;
                        if ((!tt.IsWhitespaceAfter && (tt.Next is Pullenti.Ner.TextToken) && tt.Next.LengthChar == 1) && tt.Next.Chars.IsAllUpper) 
                        {
                            tt = tt.Next;
                            res.EndToken = tt;
                            res.Value += (tt as Pullenti.Ner.TextToken).Term;
                        }
                        if ((!tt.IsWhitespaceAfter && tt.Next != null && tt.Next.IsCharOf("\\/")) && (tt.Next.Next is Pullenti.Ner.NumberToken)) 
                        {
                            res.EndToken = (tt = tt.Next.Next);
                            res.Value = string.Format("{0}/{1}", res.Value, (tt as Pullenti.Ner.NumberToken).Value);
                        }
                        if ((!tt.IsWhitespaceAfter && tt.Next != null && tt.Next.IsHiphen) && (tt.Next.Next is Pullenti.Ner.NumberToken)) 
                        {
                            res.EndToken = (tt = tt.Next.Next);
                            res.Value = string.Format("{0}-{1}", res.Value, (tt as Pullenti.Ner.NumberToken).Value);
                            if ((!tt.IsWhitespaceAfter && (tt.Next is Pullenti.Ner.TextToken) && tt.Next.LengthChar == 1) && tt.Next.Chars.IsAllUpper) 
                            {
                                tt = tt.Next;
                                res.EndToken = tt;
                                res.Value += (tt as Pullenti.Ner.TextToken).Term;
                            }
                        }
                    }
                    else if ((tt is Pullenti.Ner.TextToken) && tt.LengthChar == 1 && tt.Chars.IsAllUpper) 
                    {
                        res.Value = string.Format("{0}-{1}", res.Value, (tt as Pullenti.Ner.TextToken).Term);
                        res.EndToken = tt;
                    }
                }
            }
            return res;
        }
        static AddressItemToken _TryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locStreets, bool prefixBefore, bool ignoreStreet, AddressItemToken prev)
        {
            if (t is Pullenti.Ner.ReferentToken) 
            {
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                ItemType ty;
                Pullenti.Ner.Geo.GeoReferent geo = rt.Referent as Pullenti.Ner.Geo.GeoReferent;
                if (geo != null) 
                {
                    if (geo.IsCity || geo.IsTerritory) 
                        ty = ItemType.City;
                    else if (geo.IsState) 
                        ty = ItemType.Country;
                    else 
                        ty = ItemType.Region;
                    return new AddressItemToken(ty, t, t) { Referent = rt.Referent };
                }
            }
            if (!ignoreStreet && t != null && prev != null) 
            {
                if (t.IsValue("КВ", null) || t.IsValue("КВАРТ", null)) 
                {
                    if ((((prev.Typ == ItemType.House || prev.Typ == ItemType.Number || prev.Typ == ItemType.Building) || prev.Typ == ItemType.Floor || prev.Typ == ItemType.Potch) || prev.Typ == ItemType.Corpus || prev.Typ == ItemType.CorpusOrFlat) || prev.Typ == ItemType.Detail) 
                        ignoreStreet = true;
                }
            }
            if (!ignoreStreet) 
            {
                List<StreetItemToken> sli = StreetItemToken.TryParseList(t, locStreets, 10);
                if (sli != null) 
                {
                    AddressItemToken rt = StreetDefineHelper.TryParseStreet(sli, prefixBefore, false);
                    if (rt != null) 
                    {
                        bool crlf = false;
                        for (Pullenti.Ner.Token ttt = rt.BeginToken; ttt != rt.EndToken; ttt = ttt.Next) 
                        {
                            if (ttt.IsNewlineAfter) 
                            {
                                crlf = true;
                                break;
                            }
                        }
                        if (crlf) 
                        {
                            for (Pullenti.Ner.Token ttt = rt.BeginToken.Previous; ttt != null; ttt = ttt.Previous) 
                            {
                                if (ttt.Morph.Class.IsPreposition || ttt.IsComma) 
                                    continue;
                                if (ttt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                                    crlf = false;
                                break;
                            }
                            if (sli[0].Typ == StreetItemType.Noun && sli[0].Termin.CanonicText.Contains("ДОРОГА")) 
                                crlf = false;
                        }
                        if (crlf) 
                        {
                            AddressItemToken aat = TryParse(rt.EndToken.Next, null, false, true, null);
                            if (aat == null) 
                                return null;
                            if (aat.Typ != ItemType.House) 
                                return null;
                        }
                        return rt;
                    }
                    if (sli.Count == 1 && sli[0].Typ == StreetItemType.Noun) 
                    {
                        Pullenti.Ner.Token tt = sli[0].EndToken.Next;
                        if (tt != null && ((tt.IsHiphen || tt.IsChar('_') || tt.IsValue("НЕТ", null)))) 
                        {
                            Pullenti.Ner.Token ttt = tt.Next;
                            if (ttt != null && ttt.IsComma) 
                                ttt = ttt.Next;
                            AddressItemToken att = TryParse(ttt, null, false, true, null);
                            if (att != null) 
                            {
                                if (att.Typ == ItemType.House || att.Typ == ItemType.Corpus || att.Typ == ItemType.Building) 
                                    return new AddressItemToken(ItemType.Street, t, tt);
                            }
                        }
                    }
                }
            }
            if (t is Pullenti.Ner.ReferentToken) 
                return null;
            if (t is Pullenti.Ner.NumberToken) 
            {
                Pullenti.Ner.NumberToken n = t as Pullenti.Ner.NumberToken;
                if (((n.LengthChar == 6 || n.LengthChar == 5)) && n.Typ == Pullenti.Ner.NumberSpellingType.Digit && !n.Morph.Class.IsAdjective) 
                    return new AddressItemToken(ItemType.Zip, t, t) { Value = n.Value.ToString() };
                bool ok = false;
                if ((t.Previous != null && t.Previous.Morph.Class.IsPreposition && t.Next != null) && t.Next.Chars.IsLetter && t.Next.Chars.IsAllLower) 
                    ok = true;
                else if (t.Morph.Class.IsAdjective && !t.Morph.Class.IsNoun) 
                    ok = true;
                Pullenti.Ner.Core.TerminToken tok0 = m_Ontology.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok0 != null && (tok0.Termin.Tag is ItemType)) 
                {
                    if (tok0.EndToken.Next == null || tok0.EndToken.Next.IsComma || tok0.EndToken.IsNewlineAfter) 
                        ok = true;
                    ItemType typ0 = (ItemType)tok0.Termin.Tag;
                    if (typ0 == ItemType.Flat) 
                    {
                        if ((t.Next is Pullenti.Ner.TextToken) && t.Next.IsValue("КВ", null)) 
                        {
                            if (t.Next.GetSourceText() == "кВ") 
                                return null;
                        }
                        if ((tok0.EndToken.Next is Pullenti.Ner.NumberToken) && (tok0.EndToken.WhitespacesAfterCount < 3)) 
                        {
                            if (prev != null && ((prev.Typ == ItemType.Street || prev.Typ == ItemType.City))) 
                                return new AddressItemToken(ItemType.Number, t, t) { Value = n.Value.ToString() };
                        }
                    }
                    if ((typ0 == ItemType.Kilometer || typ0 == ItemType.Floor || typ0 == ItemType.Block) || typ0 == ItemType.Potch || typ0 == ItemType.Flat) 
                        return new AddressItemToken(typ0, t, tok0.EndToken) { Value = n.Value.ToString() };
                }
            }
            bool prepos = false;
            Pullenti.Ner.Core.TerminToken tok = null;
            if (t.Morph.Class.IsPreposition) 
            {
                if ((((tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No)))) == null) 
                {
                    if (t.BeginChar < t.EndChar) 
                        return null;
                    if (!t.IsCharOf("КСкс")) 
                        t = t.Next;
                    prepos = true;
                }
            }
            if (tok == null) 
                tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            Pullenti.Ner.Token t1 = t;
            ItemType typ = ItemType.Number;
            Pullenti.Ner.Address.AddressHouseType houseTyp = Pullenti.Ner.Address.AddressHouseType.Undefined;
            Pullenti.Ner.Address.AddressBuildingType buildTyp = Pullenti.Ner.Address.AddressBuildingType.Undefined;
            if (tok != null) 
            {
                if (t.IsValue("УЖЕ", null)) 
                    return null;
                if (tok.Termin.CanonicText == "ТАМ ЖЕ") 
                {
                    int cou = 0;
                    for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
                    {
                        if (cou > 1000) 
                            break;
                        Pullenti.Ner.Referent r = tt.GetReferent();
                        if (r == null) 
                            continue;
                        if (r is Pullenti.Ner.Address.AddressReferent) 
                        {
                            Pullenti.Ner.Geo.GeoReferent g = r.GetSlotValue(Pullenti.Ner.Address.AddressReferent.ATTR_GEO) as Pullenti.Ner.Geo.GeoReferent;
                            if (g != null) 
                                return new AddressItemToken(ItemType.City, t, tok.EndToken) { Referent = g };
                            break;
                        }
                        else if (r is Pullenti.Ner.Geo.GeoReferent) 
                        {
                            Pullenti.Ner.Geo.GeoReferent g = r as Pullenti.Ner.Geo.GeoReferent;
                            if (!g.IsState) 
                                return new AddressItemToken(ItemType.City, t, tok.EndToken) { Referent = g };
                        }
                    }
                    return null;
                }
                if (tok.Termin.Tag is Pullenti.Ner.Address.AddressDetailType) 
                    return TryAttachDetail(t);
                t1 = tok.EndToken.Next;
                if (tok.Termin.Tag is ItemType) 
                {
                    if (tok.Termin.Tag2 is Pullenti.Ner.Address.AddressHouseType) 
                        houseTyp = (Pullenti.Ner.Address.AddressHouseType)tok.Termin.Tag2;
                    if (tok.Termin.Tag2 is Pullenti.Ner.Address.AddressBuildingType) 
                        buildTyp = (Pullenti.Ner.Address.AddressBuildingType)tok.Termin.Tag2;
                    typ = (ItemType)tok.Termin.Tag;
                    if (typ == ItemType.Prefix) 
                    {
                        for (; t1 != null; t1 = t1.Next) 
                        {
                            if (((t1.Morph.Class.IsPreposition || t1.Morph.Class.IsConjunction)) && t1.WhitespacesAfterCount == 1) 
                                continue;
                            if (t1.IsChar(':')) 
                            {
                                t1 = t1.Next;
                                break;
                            }
                            if (t1.IsChar('(')) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null && (br.LengthChar < 50)) 
                                {
                                    t1 = br.EndToken;
                                    continue;
                                }
                            }
                            if (t1 is Pullenti.Ner.TextToken) 
                            {
                                if (t1.Chars.IsAllLower || (t1.WhitespacesBeforeCount < 3)) 
                                {
                                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                    if (npt != null) 
                                    {
                                        t1 = npt.EndToken;
                                        continue;
                                    }
                                }
                            }
                            if (t1.IsValue("УКАЗАННЫЙ", null) || t1.IsValue("ЕГРИП", null) || t1.IsValue("ФАКТИЧЕСКИЙ", null)) 
                                continue;
                            if (t1.IsComma) 
                            {
                                if (t1.Next != null && t1.Next.IsValue("УКАЗАННЫЙ", null)) 
                                    continue;
                            }
                            break;
                        }
                        if (t1 != null) 
                        {
                            Pullenti.Ner.Token t0 = t;
                            if (((t0.Previous != null && !t0.IsNewlineBefore && t0.Previous.IsChar(')')) && (t0.Previous.Previous is Pullenti.Ner.TextToken) && t0.Previous.Previous.Previous != null) && t0.Previous.Previous.Previous.IsChar('(')) 
                            {
                                t = t0.Previous.Previous.Previous.Previous;
                                if (t != null && t.GetMorphClassInDictionary().IsAdjective && !t.IsNewlineAfter) 
                                    t0 = t;
                            }
                            AddressItemToken res = new AddressItemToken(ItemType.Prefix, t0, t1.Previous);
                            for (Pullenti.Ner.Token tt = t0.Previous; tt != null; tt = tt.Previous) 
                            {
                                if (tt.NewlinesAfterCount > 3) 
                                    break;
                                if (tt.IsCommaAnd || tt.IsCharOf("().")) 
                                    continue;
                                if (!(tt is Pullenti.Ner.TextToken)) 
                                    break;
                                if (((tt.IsValue("ПОЧТОВЫЙ", null) || tt.IsValue("ЮРИДИЧЕСКИЙ", null) || tt.IsValue("ЮР", null)) || tt.IsValue("ФАКТИЧЕСКИЙ", null) || tt.IsValue("ФАКТ", null)) || tt.IsValue("ПОЧТ", null) || tt.IsValue("АДРЕС", null)) 
                                    res.BeginToken = tt;
                                else 
                                    break;
                            }
                            return res;
                        }
                        else 
                            return null;
                    }
                    else if (typ == ItemType.BusinessCenter) 
                    {
                        Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("ORGANIZATION", t);
                        if (rt != null) 
                            return new AddressItemToken(typ, t, rt.EndToken) { RefToken = rt };
                    }
                    else if ((typ == ItemType.CorpusOrFlat && !tok.IsWhitespaceBefore && !tok.IsWhitespaceAfter) && tok.BeginToken == tok.EndToken && tok.BeginToken.IsValue("К", null)) 
                        typ = ItemType.Corpus;
                    if (typ == ItemType.Detail && t.IsValue("У", null)) 
                    {
                        if (!Pullenti.Ner.Geo.Internal.MiscLocationHelper.CheckGeoObjectBefore(t)) 
                            return null;
                    }
                    if (typ == ItemType.Flat && t.IsValue("КВ", null)) 
                    {
                        if (t.GetSourceText() == "кВ") 
                            return null;
                    }
                    if (typ == ItemType.Kilometer || typ == ItemType.Floor || typ == ItemType.Potch) 
                        return new AddressItemToken(typ, t, tok.EndToken);
                    if ((typ == ItemType.House || typ == ItemType.Building || typ == ItemType.Corpus) || typ == ItemType.Plot) 
                    {
                        if (t1 != null && ((t1.Morph.Class.IsPreposition || t1.Morph.Class.IsConjunction)) && (t1.WhitespacesAfterCount < 2)) 
                        {
                            Pullenti.Ner.Core.TerminToken tok2 = m_Ontology.TryParse(t1.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                            if (tok2 != null && (tok2.Termin.Tag is ItemType)) 
                            {
                                ItemType typ2 = (ItemType)tok2.Termin.Tag;
                                if (typ2 != typ && ((typ2 == ItemType.Plot || ((typ2 == ItemType.House && typ == ItemType.Plot))))) 
                                {
                                    typ = typ2;
                                    if (tok.Termin.Tag2 is Pullenti.Ner.Address.AddressHouseType) 
                                        houseTyp = (Pullenti.Ner.Address.AddressHouseType)tok.Termin.Tag2;
                                    t1 = tok2.EndToken.Next;
                                    if (t1 == null) 
                                        return new AddressItemToken(typ, t, tok2.EndToken) { Value = "0", HouseType = houseTyp };
                                }
                            }
                        }
                    }
                    if (typ != ItemType.Number) 
                    {
                        if (t1 == null && t.LengthChar > 1) 
                            return new AddressItemToken(typ, t, tok.EndToken) { HouseType = houseTyp, BuildingType = buildTyp };
                        if ((t1 is Pullenti.Ner.NumberToken) && (t1 as Pullenti.Ner.NumberToken).Value == "0") 
                            return new AddressItemToken(typ, t, t1) { Value = "0", HouseType = houseTyp, BuildingType = buildTyp };
                    }
                }
            }
            if (t1 != null && t1.IsChar('.') && t1.Next != null) 
            {
                if (!t1.IsWhitespaceAfter) 
                    t1 = t1.Next;
                else if ((t1.Next is Pullenti.Ner.NumberToken) && (t1.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit && (t1.WhitespacesAfterCount < 2)) 
                    t1 = t1.Next;
            }
            if ((t1 != null && !t1.IsWhitespaceAfter && ((t1.IsHiphen || t1.IsChar('_')))) && (t1.Next is Pullenti.Ner.NumberToken)) 
                t1 = t1.Next;
            tok = m_Ontology.TryParse(t1, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null && (tok.Termin.Tag is ItemType) && ((ItemType)tok.Termin.Tag) == ItemType.Number) 
                t1 = tok.EndToken.Next;
            else if (tok != null && (tok.Termin.Tag is ItemType) && ((ItemType)tok.Termin.Tag) == ItemType.NoNumber) 
            {
                AddressItemToken re0 = new AddressItemToken(typ, t, tok.EndToken) { Value = "0", HouseType = houseTyp, BuildingType = buildTyp };
                if (!re0.IsWhitespaceAfter && (re0.EndToken.Next is Pullenti.Ner.NumberToken)) 
                {
                    re0.EndToken = re0.EndToken.Next;
                    re0.Value = (re0.EndToken as Pullenti.Ner.NumberToken).Value.ToString();
                }
                return re0;
            }
            else if (t1 != null) 
            {
                if (typ == ItemType.Flat) 
                {
                    Pullenti.Ner.Core.TerminToken tok2 = m_Ontology.TryParse(t1, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok2 != null && ((ItemType)tok2.Termin.Tag) == ItemType.Flat) 
                        t1 = tok2.EndToken.Next;
                }
                if (t1.IsValue("СТРОИТЕЛЬНЫЙ", null) && t1.Next != null) 
                    t1 = t1.Next;
                Pullenti.Ner.Token ttt = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t1);
                if (ttt != null) 
                {
                    t1 = ttt;
                    if (t1.IsHiphen || t1.IsChar('_')) 
                        t1 = t1.Next;
                }
            }
            if (t1 == null) 
                return null;
            StringBuilder num = new StringBuilder();
            Pullenti.Ner.NumberToken nt = t1 as Pullenti.Ner.NumberToken;
            AddressItemToken re11;
            if (nt != null) 
            {
                if (nt.IntValue == null || nt.IntValue.Value == 0) 
                    return null;
                num.Append(nt.Value);
                if (nt.Typ == Pullenti.Ner.NumberSpellingType.Digit || nt.Typ == Pullenti.Ner.NumberSpellingType.Words) 
                {
                    if (((nt.EndToken is Pullenti.Ner.TextToken) && (nt.EndToken as Pullenti.Ner.TextToken).Term == "Е" && nt.EndToken.Previous == nt.BeginToken) && !nt.EndToken.IsWhitespaceBefore) 
                        num.Append("Е");
                    bool drob = false;
                    bool hiph = false;
                    bool lit = false;
                    Pullenti.Ner.Token et = nt.Next;
                    if (et != null && ((et.IsCharOf("\\/") || et.IsValue("ДРОБЬ", null)))) 
                    {
                        drob = true;
                        et = et.Next;
                        if (et != null && et.IsCharOf("\\/")) 
                            et = et.Next;
                        t1 = et;
                    }
                    else if (et != null && ((et.IsHiphen || et.IsChar('_')))) 
                    {
                        hiph = true;
                        et = et.Next;
                    }
                    else if ((et != null && et.IsChar('.') && (et.Next is Pullenti.Ner.NumberToken)) && !et.IsWhitespaceAfter) 
                        return null;
                    if (et is Pullenti.Ner.NumberToken) 
                    {
                        if (drob) 
                        {
                            num.AppendFormat("/{0}", (et as Pullenti.Ner.NumberToken).Value);
                            drob = false;
                            t1 = et;
                            et = et.Next;
                            if (et != null && et.IsCharOf("\\/") && (et.Next is Pullenti.Ner.NumberToken)) 
                            {
                                t1 = et.Next;
                                num.AppendFormat("/{0}", (t1 as Pullenti.Ner.NumberToken).Value);
                                et = t1.Next;
                            }
                        }
                        else if ((hiph && !t1.IsWhitespaceAfter && (et is Pullenti.Ner.NumberToken)) && !et.IsWhitespaceBefore) 
                        {
                            AddressItemToken numm = TryParse(et, null, false, true, null);
                            if (numm != null && numm.Typ == ItemType.Number) 
                            {
                                bool merge = false;
                                if (typ == ItemType.Flat || typ == ItemType.Plot) 
                                    merge = true;
                                else if (typ == ItemType.House || typ == ItemType.Building || typ == ItemType.Corpus) 
                                {
                                    Pullenti.Ner.Token ttt = numm.EndToken.Next;
                                    if (ttt != null && ttt.IsComma) 
                                        ttt = ttt.Next;
                                    AddressItemToken numm2 = TryParse(ttt, null, false, true, null);
                                    if (numm2 != null) 
                                    {
                                        if ((numm2.Typ == ItemType.Flat || numm2.Typ == ItemType.Building || ((numm2.Typ == ItemType.CorpusOrFlat && numm2.Value != null))) || numm2.Typ == ItemType.Corpus) 
                                            merge = true;
                                    }
                                }
                                if (merge) 
                                {
                                    num.AppendFormat("/{0}", numm.Value);
                                    t1 = numm.EndToken;
                                    et = t1.Next;
                                }
                            }
                        }
                    }
                    else if (et != null && ((et.IsHiphen || et.IsChar('_') || et.IsValue("НЕТ", null))) && drob) 
                        t1 = et;
                    Pullenti.Ner.Token ett = et;
                    if ((ett != null && ett.IsCharOf(",.") && (ett.WhitespacesAfterCount < 2)) && (ett.Next is Pullenti.Ner.TextToken) && Pullenti.Ner.Core.BracketHelper.IsBracket(ett.Next, false)) 
                        ett = ett.Next;
                    if (((Pullenti.Ner.Core.BracketHelper.IsBracket(ett, false) && (ett.Next is Pullenti.Ner.TextToken) && ett.Next.LengthChar == 1) && ett.Next.IsLetters && Pullenti.Ner.Core.BracketHelper.IsBracket(ett.Next.Next, false)) && !ett.IsWhitespaceAfter && !ett.Next.IsWhitespaceAfter) 
                    {
                        string ch = CorrectCharToken(ett.Next);
                        if (ch == null) 
                            return null;
                        num.Append(ch);
                        t1 = ett.Next.Next;
                    }
                    else if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(ett, true, false) && (ett.WhitespacesBeforeCount < 2)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(ett, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null && (br.BeginToken.Next is Pullenti.Ner.TextToken) && br.BeginToken.Next.Next == br.EndToken) 
                        {
                            string s = CorrectCharToken(br.BeginToken.Next);
                            if (s != null) 
                            {
                                num.Append(s);
                                t1 = br.EndToken;
                            }
                        }
                    }
                    else if ((et is Pullenti.Ner.TextToken) && (et as Pullenti.Ner.TextToken).LengthChar == 1) 
                    {
                        string s = CorrectCharToken(et);
                        if (s != null) 
                        {
                            if (((s == "К" || s == "С")) && (et.Next is Pullenti.Ner.NumberToken) && !et.IsWhitespaceAfter) 
                            {
                            }
                            else if ((s == "Б" && et.Next != null && et.Next.IsCharOf("/\\")) && (et.Next.Next is Pullenti.Ner.TextToken) && et.Next.Next.IsValue("Н", null)) 
                                t1 = (et = et.Next.Next);
                            else 
                            {
                                bool ok = false;
                                if (drob || hiph || lit) 
                                    ok = true;
                                else if (!et.IsWhitespaceBefore || ((et.WhitespacesBeforeCount == 1 && et.Chars.IsAllUpper))) 
                                {
                                    ok = true;
                                    if (et.Next is Pullenti.Ner.NumberToken) 
                                    {
                                        if (!et.IsWhitespaceBefore && et.IsWhitespaceAfter) 
                                        {
                                        }
                                        else 
                                            ok = false;
                                    }
                                }
                                else if (((et.Next == null || et.Next.IsComma)) && (et.WhitespacesBeforeCount < 2)) 
                                    ok = true;
                                else if (et.IsWhitespaceBefore && et.Chars.IsAllLower && et.IsValue("В", "У")) 
                                {
                                }
                                else 
                                {
                                    AddressItemToken aitNext = TryParse(et.Next, null, false, true, null);
                                    if (aitNext != null) 
                                    {
                                        if ((aitNext.Typ == ItemType.Corpus || aitNext.Typ == ItemType.Flat || aitNext.Typ == ItemType.Building) || aitNext.Typ == ItemType.Office) 
                                            ok = true;
                                    }
                                }
                                if (ok) 
                                {
                                    num.Append(s);
                                    t1 = et;
                                    if (et.Next != null && et.Next.IsCharOf("\\/") && et.Next.Next != null) 
                                    {
                                        if (et.Next.Next is Pullenti.Ner.NumberToken) 
                                        {
                                            num.AppendFormat("/{0}", (et.Next.Next as Pullenti.Ner.NumberToken).Value);
                                            t1 = (et = et.Next.Next);
                                        }
                                        else if (et.Next.Next.IsHiphen || et.Next.Next.IsChar('_') || et.Next.Next.IsValue("НЕТ", null)) 
                                            t1 = (et = et.Next.Next);
                                    }
                                }
                            }
                        }
                    }
                    else if ((et is Pullenti.Ner.TextToken) && !et.IsWhitespaceBefore) 
                    {
                        string val = (et as Pullenti.Ner.TextToken).Term;
                        if (val == "КМ" && typ == ItemType.House) 
                        {
                            t1 = et;
                            num.Append("КМ");
                        }
                        else if (val == "БН") 
                            t1 = et;
                        else if (((val.Length == 2 && val[1] == 'Б' && et.Next != null) && et.Next.IsCharOf("\\/") && et.Next.Next != null) && et.Next.Next.IsValue("Н", null)) 
                        {
                            num.Append(val[0]);
                            t1 = (et = et.Next.Next);
                        }
                    }
                }
            }
            else if ((((re11 = _tryAttachVCH(t1, typ)))) != null) 
            {
                re11.BeginToken = t;
                re11.HouseType = houseTyp;
                re11.BuildingType = buildTyp;
                return re11;
            }
            else if (((t1 is Pullenti.Ner.TextToken) && t1.LengthChar == 2 && t1.IsLetters) && !t1.IsWhitespaceBefore && (t1.Previous is Pullenti.Ner.NumberToken)) 
            {
                string src = t1.GetSourceText();
                if ((src != null && src.Length == 2 && ((src[0] == 'к' || src[0] == 'k'))) && char.IsUpper(src[1])) 
                {
                    char ch = CorrectChar(src[1]);
                    if (ch != ((char)0)) 
                        return new AddressItemToken(ItemType.Corpus, t1, t1) { Value = string.Format("{0}", ch) };
                }
            }
            else if ((t1 is Pullenti.Ner.TextToken) && t1.LengthChar == 1 && t1.IsLetters) 
            {
                string ch = CorrectCharToken(t1);
                if (ch != null) 
                {
                    if (typ == ItemType.Number) 
                        return null;
                    if (ch == "К" || ch == "С") 
                    {
                        if (!t1.IsWhitespaceAfter && (t1.Next is Pullenti.Ner.NumberToken)) 
                            return null;
                    }
                    if (ch == "Д" && typ == ItemType.Plot) 
                    {
                        AddressItemToken rrr = _TryParse(t1, null, false, true, null);
                        if (rrr != null) 
                        {
                            rrr.Typ = ItemType.Plot;
                            rrr.BeginToken = t;
                            return rrr;
                        }
                    }
                    if (t1.Chars.IsAllLower && ((t1.Morph.Class.IsPreposition || t1.Morph.Class.IsConjunction))) 
                    {
                        if ((t1.WhitespacesAfterCount < 2) && t1.Next.Chars.IsLetter) 
                            return null;
                    }
                    if (t.Chars.IsAllUpper && t.LengthChar == 1 && t.Next.IsChar('.')) 
                        return null;
                    num.Append(ch);
                    if ((t1.Next != null && ((t1.Next.IsHiphen || t1.Next.IsChar('_'))) && !t1.IsWhitespaceAfter) && (t1.Next.Next is Pullenti.Ner.NumberToken) && !t1.Next.IsWhitespaceAfter) 
                    {
                        num.Append((t1.Next.Next as Pullenti.Ner.NumberToken).Value);
                        t1 = t1.Next.Next;
                    }
                    else if ((t1.Next is Pullenti.Ner.NumberToken) && !t1.IsWhitespaceAfter && t1.Chars.IsAllUpper) 
                    {
                        num.Append((t1.Next as Pullenti.Ner.NumberToken).Value);
                        t1 = t1.Next;
                    }
                    if (num.Length == 1 && typ == ItemType.Office) 
                        return null;
                }
                if (typ == ItemType.Box && num.Length == 0) 
                {
                    Pullenti.Ner.NumberToken rom = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t1);
                    if (rom != null) 
                        return new AddressItemToken(typ, t, rom.EndToken) { Value = rom.Value.ToString() };
                }
            }
            else if (((Pullenti.Ner.Core.BracketHelper.IsBracket(t1, false) && (t1.Next is Pullenti.Ner.TextToken) && t1.Next.LengthChar == 1) && t1.Next.IsLetters && Pullenti.Ner.Core.BracketHelper.IsBracket(t1.Next.Next, false)) && !t1.IsWhitespaceAfter && !t1.Next.IsWhitespaceAfter) 
            {
                string ch = CorrectCharToken(t1.Next);
                if (ch == null) 
                    return null;
                num.Append(ch);
                t1 = t1.Next.Next;
            }
            else if ((t1 is Pullenti.Ner.TextToken) && ((((t1.LengthChar == 1 && ((t1.IsHiphen || t1.IsChar('_'))))) || t1.IsValue("НЕТ", null) || t1.IsValue("БН", null))) && (((typ == ItemType.Corpus || typ == ItemType.CorpusOrFlat || typ == ItemType.Building) || typ == ItemType.House || typ == ItemType.Flat))) 
            {
                while (t1.Next != null && ((t1.Next.IsHiphen || t1.Next.IsChar('_'))) && !t1.IsWhitespaceAfter) 
                {
                    t1 = t1.Next;
                }
                string val = null;
                if (!t1.IsWhitespaceAfter && (t1.Next is Pullenti.Ner.NumberToken)) 
                {
                    t1 = t1.Next;
                    val = (t1 as Pullenti.Ner.NumberToken).Value.ToString();
                }
                if (t1.IsValue("БН", null)) 
                    val = "0";
                return new AddressItemToken(typ, t, t1) { Value = val };
            }
            else 
            {
                if (((typ == ItemType.Floor || typ == ItemType.Kilometer || typ == ItemType.Potch)) && (t.Previous is Pullenti.Ner.NumberToken)) 
                    return new AddressItemToken(typ, t, t1.Previous);
                if ((t1 is Pullenti.Ner.ReferentToken) && (t1.GetReferent() is Pullenti.Ner.Date.DateReferent)) 
                {
                    AddressItemToken nn = _TryParse((t1 as Pullenti.Ner.ReferentToken).BeginToken, locStreets, prefixBefore, true, null);
                    if (nn != null && nn.EndChar == t1.EndChar && nn.Typ == ItemType.Number) 
                    {
                        nn.BeginToken = t;
                        nn.EndToken = t1;
                        nn.Typ = typ;
                        return nn;
                    }
                }
                if ((t1 is Pullenti.Ner.TextToken) && ((typ == ItemType.House || typ == ItemType.Building || typ == ItemType.Corpus))) 
                {
                    string ter = (t1 as Pullenti.Ner.TextToken).Term;
                    if (ter == "АБ" || ter == "АБВ" || ter == "МГУ") 
                        return new AddressItemToken(typ, t, t1) { Value = ter, HouseType = houseTyp, BuildingType = buildTyp };
                    if (prev != null && ((prev.Typ == ItemType.Street || prev.Typ == ItemType.City)) && t1.Chars.IsAllUpper) 
                        return new AddressItemToken(typ, t, t1) { Value = ter, HouseType = houseTyp, BuildingType = buildTyp };
                }
                if (typ == ItemType.Box) 
                {
                    Pullenti.Ner.NumberToken rom = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t1);
                    if (rom != null) 
                        return new AddressItemToken(typ, t, rom.EndToken) { Value = rom.Value.ToString() };
                }
                if (typ == ItemType.Plot && t1 != null) 
                {
                    if ((t1.IsValue("ОКОЛО", null) || t1.IsValue("РЯДОМ", null) || t1.IsValue("НАПРОТИВ", null)) || t1.IsValue("БЛИЗЬКО", null) || t1.IsValue("НАВПАКИ", null)) 
                        return new AddressItemToken(typ, t, t1) { Value = t1.GetSourceText().ToLower() };
                }
                return null;
            }
            if (typ == ItemType.Number && prepos) 
                return null;
            if (t1 == null) 
            {
                t1 = t;
                while (t1.Next != null) 
                {
                    t1 = t1.Next;
                }
            }
            return new AddressItemToken(typ, t, t1) { Value = num.ToString(), Morph = t.Morph, HouseType = houseTyp, BuildingType = buildTyp };
        }
        static AddressItemToken _tryAttachVCH(Pullenti.Ner.Token t, ItemType ty)
        {
            if (t == null) 
                return null;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if ((((tt.IsValue("В", null) || tt.IsValue("B", null))) && tt.Next != null && tt.Next.IsCharOf("./\\")) && (tt.Next.Next is Pullenti.Ner.TextToken) && tt.Next.Next.IsValue("Ч", null)) 
                {
                    tt = tt.Next.Next;
                    if (tt.Next != null && tt.Next.IsChar('.')) 
                        tt = tt.Next;
                    Pullenti.Ner.Token tt2 = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(tt.Next);
                    if (tt2 != null) 
                        tt = tt2;
                    if (tt.Next != null && (tt.Next is Pullenti.Ner.NumberToken) && (tt.WhitespacesAfterCount < 2)) 
                        tt = tt.Next;
                    return new AddressItemToken(ty, t, tt) { Value = "В/Ч" };
                }
                else if (((tt.IsValue("ВОЙСКОВОЙ", null) || tt.IsValue("ВОИНСКИЙ", null))) && tt.Next != null && tt.Next.IsValue("ЧАСТЬ", null)) 
                {
                    tt = tt.Next;
                    Pullenti.Ner.Token tt2 = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(tt.Next);
                    if (tt2 != null) 
                        tt = tt2;
                    if (tt.Next != null && (tt.Next is Pullenti.Ner.NumberToken) && (tt.WhitespacesAfterCount < 2)) 
                        tt = tt.Next;
                    return new AddressItemToken(ty, t, tt) { Value = "В/Ч" };
                }
                else if (ty == ItemType.Flat) 
                {
                    if (tt.WhitespacesBeforeCount > 1) 
                        break;
                    if (!(tt is Pullenti.Ner.TextToken)) 
                        break;
                    if ((tt as Pullenti.Ner.TextToken).Term.StartsWith("ОБЩ")) 
                    {
                        if (tt.Next != null && tt.Next.IsChar('.')) 
                            tt = tt.Next;
                        AddressItemToken re = _tryAttachVCH(tt.Next, ty);
                        if (re != null) 
                            return re;
                        return new AddressItemToken(ty, t, tt) { Value = "ОБЩ" };
                    }
                    if (tt.Chars.IsAllUpper && tt.LengthChar > 1) 
                    {
                        AddressItemToken re = new AddressItemToken(ty, t, tt) { Value = (tt as Pullenti.Ner.TextToken).Term };
                        if ((tt.WhitespacesAfterCount < 2) && (tt.Next is Pullenti.Ner.TextToken) && tt.Next.Chars.IsAllUpper) 
                        {
                            tt = tt.Next;
                            re.EndToken = tt;
                            re.Value += (tt as Pullenti.Ner.TextToken).Term;
                        }
                        return re;
                    }
                    break;
                }
                else 
                    break;
            }
            return null;
        }
        public static AddressItemToken TryAttachDetail(Pullenti.Ner.Token t)
        {
            if (t == null || (t is Pullenti.Ner.ReferentToken)) 
                return null;
            Pullenti.Ner.Token tt = t;
            if (t.Chars.IsCapitalUpper && !t.Morph.Class.IsPreposition) 
                return null;
            Pullenti.Ner.Core.TerminToken tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok == null && t.Morph.Class.IsPreposition && t.Next != null) 
            {
                tt = t.Next;
                if (tt is Pullenti.Ner.NumberToken) 
                {
                }
                else 
                {
                    if (tt.Chars.IsCapitalUpper && !tt.Morph.Class.IsPreposition) 
                        return null;
                    tok = m_Ontology.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                }
            }
            AddressItemToken res = null;
            bool firstNum = false;
            if (tok == null) 
            {
                if (tt is Pullenti.Ner.NumberToken) 
                {
                    firstNum = true;
                    Pullenti.Ner.Core.NumberExToken nex = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(tt);
                    if (nex != null && ((nex.ExTyp == Pullenti.Ner.Core.NumberExType.Meter || nex.ExTyp == Pullenti.Ner.Core.NumberExType.Kilometer))) 
                    {
                        res = new AddressItemToken(ItemType.Detail, t, nex.EndToken);
                        Pullenti.Ner.Core.NumberExType tyy = Pullenti.Ner.Core.NumberExType.Meter;
                        res.DetailMeters = (int)nex.NormalizeValue(ref tyy);
                    }
                }
                if (res == null) 
                    return null;
            }
            else 
            {
                if (!(tok.Termin.Tag is Pullenti.Ner.Address.AddressDetailType)) 
                    return null;
                if (t.IsValue("У", null)) 
                {
                    if (Pullenti.Ner.Geo.Internal.MiscLocationHelper.CheckGeoObjectBefore(t)) 
                    {
                    }
                    else if (Pullenti.Ner.Geo.Internal.MiscLocationHelper.CheckGeoObjectAfter(t, false)) 
                    {
                    }
                    else 
                        return null;
                }
                res = new AddressItemToken(ItemType.Detail, t, tok.EndToken) { DetailType = (Pullenti.Ner.Address.AddressDetailType)tok.Termin.Tag };
            }
            for (tt = res.EndToken.Next; tt != null; tt = tt.Next) 
            {
                if (tt is Pullenti.Ner.ReferentToken) 
                    break;
                if (!tt.Morph.Class.IsPreposition) 
                {
                    if (tt.Chars.IsCapitalUpper || tt.Chars.IsAllUpper) 
                        break;
                }
                tok = m_Ontology.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null && (tok.Termin.Tag is Pullenti.Ner.Address.AddressDetailType)) 
                {
                    Pullenti.Ner.Address.AddressDetailType ty = (Pullenti.Ner.Address.AddressDetailType)tok.Termin.Tag;
                    if (ty != Pullenti.Ner.Address.AddressDetailType.Undefined) 
                    {
                        if (ty == Pullenti.Ner.Address.AddressDetailType.Near && res.DetailType != Pullenti.Ner.Address.AddressDetailType.Undefined && res.DetailType != ty) 
                        {
                        }
                        else 
                            res.DetailType = ty;
                    }
                    res.EndToken = (tt = tok.EndToken);
                    continue;
                }
                if (tt.IsValue("ОРИЕНТИР", null) || tt.IsValue("НАПРАВЛЕНИЕ", null) || tt.IsValue("ОТ", null)) 
                {
                    res.EndToken = tt;
                    continue;
                }
                if (tt.IsComma || tt.Morph.Class.IsPreposition) 
                    continue;
                if ((tt is Pullenti.Ner.NumberToken) && tt.Next != null) 
                {
                    Pullenti.Ner.Core.NumberExToken nex = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(tt);
                    if (nex != null && ((nex.ExTyp == Pullenti.Ner.Core.NumberExType.Meter || nex.ExTyp == Pullenti.Ner.Core.NumberExType.Kilometer))) 
                    {
                        res.EndToken = (tt = nex.EndToken);
                        Pullenti.Ner.Core.NumberExType tyy = Pullenti.Ner.Core.NumberExType.Meter;
                        res.DetailMeters = (int)nex.NormalizeValue(ref tyy);
                        continue;
                    }
                }
                break;
            }
            if (firstNum && res.DetailType == Pullenti.Ner.Address.AddressDetailType.Undefined) 
                return null;
            if (res != null && res.EndToken.Next != null && res.EndToken.Next.Morph.Class.IsPreposition) 
            {
                if (res.EndToken.WhitespacesAfterCount == 1 && res.EndToken.Next.WhitespacesAfterCount == 1) 
                    res.EndToken = res.EndToken.Next;
            }
            return res;
        }
        public static AddressItemToken TryAttachOrg(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            if ((t.LengthChar > 5 && !t.Chars.IsAllUpper && !t.Chars.IsAllLower) && !t.Chars.IsCapitalUpper) 
            {
                string namm = (t as Pullenti.Ner.TextToken).GetSourceText();
                if (char.IsUpper(namm[0]) && char.IsUpper(namm[1])) 
                {
                    for (int i = 0; i < namm.Length; i++) 
                    {
                        if (char.IsLower(namm[i]) && i > 2) 
                        {
                            string abbr = namm.Substring(0, i - 1);
                            Pullenti.Ner.Core.Termin te = new Pullenti.Ner.Core.Termin(abbr) { Acronym = abbr };
                            List<Pullenti.Ner.Core.Termin> li = m_OrgOntology.FindTerminsByTermin(te);
                            if (li != null && li.Count > 0) 
                            {
                                Pullenti.Ner.Referent org00 = t.Kit.CreateReferent("ORGANIZATION");
                                org00.AddSlot("TYPE", li[0].CanonicText.ToLower(), false, 0);
                                org00.AddSlot("TYPE", abbr, false, 0);
                                namm = (t as Pullenti.Ner.TextToken).Term.Substring(i - 1);
                                Pullenti.Ner.ReferentToken rt00 = new Pullenti.Ner.ReferentToken(org00, t, t);
                                rt00.Data = t.Kit.GetAnalyzerDataByAnalyzerName("ORGANIZATION");
                                if (t.Next != null && t.Next.IsHiphen) 
                                {
                                    if (t.Next.Next is Pullenti.Ner.NumberToken) 
                                    {
                                        org00.AddSlot("NUMBER", (t.Next.Next as Pullenti.Ner.NumberToken).Value.ToString(), false, 0);
                                        rt00.EndToken = t.Next.Next;
                                    }
                                    else if ((t.Next.Next is Pullenti.Ner.TextToken) && !t.Next.IsWhitespaceAfter) 
                                    {
                                        namm = string.Format("{0}-{1}", namm, (t.Next.Next as Pullenti.Ner.TextToken).Term);
                                        rt00.EndToken = t.Next.Next;
                                    }
                                }
                                org00.AddSlot("NAME", namm, false, 0);
                                return new AddressItemToken(ItemType.Street, t, rt00.EndToken) { Referent = rt00.Referent, RefToken = rt00, RefTokenIsGsk = true };
                            }
                            break;
                        }
                    }
                }
            }
            if (t.IsValue("СТ", null)) 
            {
            }
            Pullenti.Ner.ReferentToken rt = null;
            string typ = null;
            string typ2 = null;
            string nam = null;
            string num = null;
            Pullenti.Ner.Token t1 = null;
            bool ok = false;
            Pullenti.Ner.Core.TerminToken tok = m_OrgOntology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            Pullenti.Ner.ReferentToken rt1 = t.Kit.ProcessReferent("ORGANIZATION", t);
            if (rt1 == null) 
            {
                rt1 = t.Kit.ProcessReferent("NAMEDENTITY", t);
                if (rt1 != null) 
                {
                    string tyy = rt1.Referent.GetStringValue("TYPE");
                    if (((tyy == "аэропорт" || tyy == "аэродром" || tyy == "заказник") || tyy == "лес" || tyy == "заповедник") || tyy == "сад") 
                    {
                    }
                    else 
                        rt1 = null;
                }
            }
            else 
            {
                if (rt1.Referent.FindSlot("TYPE", "ОПС", true) != null) 
                    return null;
                for (Pullenti.Ner.Token tt = rt1.BeginToken.Next; tt != null && (tt.EndChar < rt1.EndChar); tt = tt.Next) 
                {
                    if (tt.IsComma) 
                    {
                        rt1.EndToken = tt.Previous;
                        if (tt.Next is Pullenti.Ner.ReferentToken) 
                        {
                            Pullenti.Ner.Slot s = rt1.Referent.FindSlot(null, tt.Next.GetReferent(), true);
                            if (s != null) 
                                rt1.Referent.Slots.Remove(s);
                        }
                    }
                }
                for (Pullenti.Ner.Token tt = rt1.EndToken.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsHiphen || tt.IsComma) 
                    {
                    }
                    else if ((tt is Pullenti.Ner.TextToken) && (tt as Pullenti.Ner.TextToken).Term == "ПМК") 
                    {
                        Pullenti.Ner.Token tt2 = tt.Next;
                        if (tt2 != null && ((tt2.IsHiphen || tt2.IsCharOf(":")))) 
                            tt2 = tt2.Next;
                        if (tt2 is Pullenti.Ner.NumberToken) 
                        {
                            rt1.Referent.AddSlot("NUMBER", (tt2 as Pullenti.Ner.NumberToken).Value.ToString(), false, 0);
                            rt1.EndToken = tt2;
                            break;
                        }
                    }
                    else 
                        break;
                }
            }
            Pullenti.Ner.Token tt1 = t.Next;
            if (tt1 != null && tt1.IsValue("ПМК", null)) 
                tt1 = tt1.Next;
            if (tok != null) 
            {
                if (tok.BeginToken == tok.EndToken && tok.BeginToken.IsValue("СП", null)) 
                {
                    tok = m_OrgOntology.TryParse(tok.EndToken.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null) 
                    {
                        tok.BeginToken = t;
                        ok = true;
                        tt1 = tok.EndToken.Next;
                    }
                    if (rt1 == null) 
                    {
                        if ((((rt1 = t.Kit.ProcessReferent("ORGANIZATION", t.Next)))) != null) 
                            rt1.BeginToken = t;
                    }
                }
                else if (tok.BeginToken == tok.EndToken && tok.BeginToken.IsValue("ГПК", null)) 
                {
                    tt1 = tok.EndToken.Next;
                    if (tt1 == null || tok.IsNewlineAfter || !(tt1 is Pullenti.Ner.TextToken)) 
                        return null;
                    if (tt1.Kit.ProcessReferent("GEO", tt1) != null) 
                        return null;
                    if (tt1.Chars.IsAllUpper || Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt1, true, false)) 
                    {
                    }
                    else 
                        return null;
                }
                else 
                {
                    ok = true;
                    tt1 = tok.EndToken.Next;
                }
                Pullenti.Ner.Core.TerminToken tok2 = m_OrgOntology.TryParse(tt1, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok2 != null) 
                {
                    tt1 = tok2.EndToken.Next;
                    tok2 = m_OrgOntology.TryParse(tt1, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok2 != null) 
                        tt1 = tok2.EndToken.Next;
                }
                while (tt1 != null) 
                {
                    if (tt1.IsValue("ОБЩЕСТВО", null) || tt1.IsValue("ТЕРРИТОРИЯ", null) || tt1.IsValue("ПМК", null)) 
                        tt1 = tt1.Next;
                    else 
                        break;
                }
                if ((tt1 is Pullenti.Ner.TextToken) && tt1.Chars.IsAllLower && ((tt1.LengthChar == 2 || tt1.LengthChar == 3))) 
                {
                    if (tt1.WhitespacesBeforeCount < 2) 
                    {
                        if (AddressItemToken.CheckHouseAfter(tt1, false, false)) 
                            return null;
                        tt1 = tt1.Next;
                    }
                }
            }
            else if (t.LengthChar > 1 && t.Chars.IsCyrillicLetter) 
            {
                Pullenti.Ner.Token nt2 = t;
                Pullenti.Ner.Token num2 = null;
                if (t.Chars.IsAllUpper) 
                {
                    if (t.IsValue("ФЗ", null) || t.IsValue("ФКЗ", null)) 
                        return null;
                    ok = true;
                }
                else if (t.Chars.IsAllLower && t.GetMorphClassInDictionary().IsUndefined && !t.IsValue("ПСЕВДО", null)) 
                    ok = true;
                for (Pullenti.Ner.Token tt2 = t.Next; tt2 != null; tt2 = tt2.Next) 
                {
                    if (tt2.WhitespacesBeforeCount > 2) 
                        break;
                    Pullenti.Ner.Core.TerminToken ooo = m_OrgOntology.TryParse(tt2, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (ooo != null) 
                    {
                        AddressItemToken oooo = TryAttachOrg(tt2);
                        if (oooo == null) 
                        {
                            ok = true;
                            tok = ooo;
                            typ = tok.Termin.CanonicText.ToLower();
                            typ2 = tok.Termin.Acronym;
                            nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, nt2, Pullenti.Ner.Core.GetTextAttr.No);
                            if (num2 is Pullenti.Ner.NumberToken) 
                                num = (num2 as Pullenti.Ner.NumberToken).Value.ToString();
                            t1 = nt2;
                        }
                        break;
                    }
                    if (tt2.IsHiphen) 
                        continue;
                    if (tt2.IsValue("ИМ", null)) 
                    {
                        if (tt2.Next != null && tt2.Next.IsChar('.')) 
                            tt2 = tt2.Next;
                        continue;
                    }
                    if (tt2 is Pullenti.Ner.NumberToken) 
                    {
                        num2 = tt2;
                        continue;
                    }
                    Pullenti.Ner.NumberToken nuuu = Pullenti.Ner.Core.NumberHelper.TryParseAge(tt2);
                    if (nuuu != null) 
                    {
                        num = (nuuu as Pullenti.Ner.NumberToken).Value.ToString();
                        num2 = nuuu;
                        tt2 = nuuu.EndToken;
                        continue;
                    }
                    if (!(tt2 is Pullenti.Ner.TextToken) || !tt2.Chars.IsCyrillicLetter) 
                        break;
                    if (tt2.Chars.IsAllLower) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt2.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (nnn != null && nnn.EndToken == tt2) 
                        {
                        }
                        else if (tt2.GetMorphClassInDictionary().IsNoun && tt2.Morph.Case.IsGenitive) 
                        {
                        }
                        else 
                            break;
                    }
                    nt2 = tt2;
                }
            }
            else if (Pullenti.Ner.Core.BracketHelper.IsBracket(t, true)) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    if (CheckHouseAfter(br.EndToken.Next, false, false)) 
                    {
                        tt1 = t;
                        ok = true;
                    }
                    else 
                    {
                        string txt = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken, br.EndToken, Pullenti.Ner.Core.GetTextAttr.No) ?? "";
                        if ((txt.Contains("БИЗНЕС") || txt.Contains("БІЗНЕС") || txt.Contains("ПЛАЗА")) || txt.Contains("PLAZA")) 
                        {
                            tt1 = t;
                            ok = true;
                        }
                    }
                }
            }
            bool bracks = false;
            bool isVeryDoubt = false;
            if (ok && Pullenti.Ner.Core.BracketHelper.IsBracket(tt1, false)) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null && (br.LengthChar < 100)) 
                {
                    AddressItemToken res1 = TryAttachOrg(tt1.Next);
                    if (res1 != null && res1.RefToken != null) 
                    {
                        if (res1.EndToken == br.EndToken || res1.EndToken == br.EndToken.Previous) 
                        {
                            res1.RefToken.BeginToken = (res1.BeginToken = t);
                            res1.RefToken.EndToken = (res1.EndToken = br.EndToken);
                            res1.RefToken.Referent.AddSlot("TYPE", (tok == null ? t.GetSourceText().ToUpper() : tok.Termin.CanonicText.ToLower()), false, 0);
                            return res1;
                        }
                    }
                    typ = (tok == null ? (t == tt1 ? null : Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t, Pullenti.Ner.Core.GetTextAttr.No)) : tok.Termin.CanonicText.ToLower());
                    if (tok != null) 
                        typ2 = tok.Termin.Acronym;
                    Pullenti.Ner.Token tt = br.EndToken.Previous;
                    if (tt is Pullenti.Ner.NumberToken) 
                    {
                        num = (tt as Pullenti.Ner.NumberToken).Value.ToString();
                        tt = tt.Previous;
                        if (tt != null && (((tt.IsHiphen || tt.IsChar('_') || tt.IsValue("N", null)) || tt.IsValue("№", null)))) 
                            tt = tt.Previous;
                    }
                    if (tt != null) 
                        nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken, tt, Pullenti.Ner.Core.GetTextAttr.No);
                    t1 = br.EndToken;
                    bracks = true;
                }
            }
            if (ok && ((((typ == null && ((t.Chars.IsAllUpper && t.LengthChar == 3)))) || tok != null))) 
            {
                Pullenti.Ner.Token tt = tt1;
                if (tt != null && ((tt.IsHiphen || tt.IsChar('_')))) 
                    tt = tt.Next;
                AddressItemToken adt = TryParse(tt, null, false, true, null);
                if (adt != null && adt.Typ == ItemType.Number) 
                {
                    if (tt.Previous.IsHiphen || tt.Previous.IsChar('_') || !(tt is Pullenti.Ner.NumberToken)) 
                    {
                    }
                    else 
                        isVeryDoubt = true;
                    num = adt.Value;
                    t1 = adt.EndToken;
                    if (tok != null) 
                    {
                        typ = tok.Termin.CanonicText.ToLower();
                        typ2 = tok.Termin.Acronym;
                    }
                }
            }
            if (((tok != null && typ == null && (tt1 is Pullenti.Ner.TextToken)) && !tt1.Chars.IsAllLower && tt1.Chars.IsCyrillicLetter) && (tt1.WhitespacesBeforeCount < 3)) 
            {
                typ = tok.Termin.CanonicText.ToLower();
                typ2 = tok.Termin.Acronym;
                nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt1, tt1, Pullenti.Ner.Core.GetTextAttr.No);
                if (typ2 == "СТ" && nam == "СЭВ") 
                    return null;
                t1 = tt1;
            }
            else if (((tok != null && typ == null && tt1 != null) && (tt1.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && (tt1.WhitespacesBeforeCount < 3)) && (tt1 as Pullenti.Ner.ReferentToken).BeginToken == (tt1 as Pullenti.Ner.ReferentToken).EndToken) 
            {
                typ = tok.Termin.CanonicText.ToLower();
                typ2 = tok.Termin.Acronym;
                nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt1, tt1, Pullenti.Ner.Core.GetTextAttr.No);
                t1 = tt1;
            }
            if ((ok && typ == null && num != null) && t.LengthChar > 2 && (t.LengthChar < 5)) 
            {
                Pullenti.Ner.Token tt2 = t1.Next;
                if (tt2 != null && tt2.IsChar(',')) 
                    tt2 = tt2.Next;
                if (tt2 != null && (tt2.WhitespacesAfterCount < 2)) 
                {
                    AddressItemToken adt = TryParse(tt2, null, false, true, null);
                    if (adt != null) 
                    {
                        if (((adt.Typ == ItemType.Block || adt.Typ == ItemType.Box || adt.Typ == ItemType.Building) || adt.Typ == ItemType.Corpus || adt.Typ == ItemType.House) || adt.Typ == ItemType.Plot) 
                            typ = t.GetSourceText();
                    }
                }
            }
            if (typ == null && nam != null) 
            {
                if (nam.Contains("БИЗНЕС") || nam.Contains("ПЛАЗА") || nam.Contains("PLAZA")) 
                    typ = "бизнес центр";
                else if (nam.Contains("БІЗНЕС")) 
                    typ = "бізнес центр";
            }
            if (typ != null) 
            {
                Pullenti.Ner.Referent org = t.Kit.CreateReferent("ORGANIZATION");
                if (org == null) 
                    org = new Pullenti.Ner.Referent("ORGANIZATION");
                org.AddSlot("TYPE", typ, false, 0);
                if (typ2 != null) 
                    org.AddSlot("TYPE", typ2, false, 0);
                if (nam != null) 
                {
                    if ((!bracks && t1.Next != null && t1.Next.Chars.IsCyrillicLetter) && t1.WhitespacesAfterCount == 1) 
                    {
                        ok = false;
                        if (tok != null && t1.Next == tok.EndToken) 
                        {
                        }
                        else if (t1.Next.Next == null || Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next.Next, false, null, false)) 
                            ok = true;
                        else if (t1.Next.Next.IsChar(',')) 
                            ok = true;
                        else if ((t1.Next.Next is Pullenti.Ner.NumberToken) && ((t1.Next.Next.Next == null || Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next.Next.Next, false, null, false)))) 
                            ok = true;
                        else if (((t1.Next.Next.IsHiphen || t1.Next.Next.IsValue("N", null) || t1.Next.Next.IsValue("№", null))) && (t1.Next.Next.Next is Pullenti.Ner.NumberToken)) 
                            ok = true;
                        if (ok) 
                        {
                            nam = string.Format("{0} {1}", nam, t1.Next.GetSourceText().ToUpper());
                            t1 = t1.Next;
                        }
                    }
                    else if ((((!bracks && t1.Next != null && t1.Next.Next != null) && t1.Next.IsHiphen && !t1.IsWhitespaceAfter) && !t1.Next.IsWhitespaceAfter && (((t1.Next.Next is Pullenti.Ner.TextToken) || (t1.Next.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)))) && t1.Next.Next.Chars.IsCyrillicLetter) 
                    {
                        nam = string.Format("{0} {1}", nam, Pullenti.Ner.Core.MiscHelper.GetTextValue(t1.Next.Next, t1.Next.Next, Pullenti.Ner.Core.GetTextAttr.No));
                        t1 = t1.Next.Next;
                    }
                    if ((nam.StartsWith("ИМ.") || nam.StartsWith("ИМ ") || nam.StartsWith("ІМ.")) || nam.StartsWith("ІМ ")) 
                    {
                        org.AddSlot("NAME", nam.Substring(3).Trim(), false, 0);
                        nam = string.Format("{0} {1}", (nam.StartsWith("ІМ") ? "ІМЕНІ" : "ИМЕНИ"), nam.Substring(3).Trim());
                    }
                    if (nam.StartsWith("ИМЕНИ ") || nam.StartsWith("ІМЕНІ ")) 
                        org.AddSlot("NAME", nam.Substring(6).Trim(), false, 0);
                    org.AddSlot("NAME", nam, false, 0);
                }
                rt = new Pullenti.Ner.ReferentToken(org, t, t1) { Data = t.Kit.GetAnalyzerDataByAnalyzerName("ORGANIZATION") };
                bool emptyOrg = false;
                if ((t1.Next != null && t1.Next.IsHiphen && t1.Next.Next != null) && t1.Next.Next.IsValue("ГОРОДИЩЕ", null)) 
                    rt.EndToken = t1.Next.Next;
                if (t1.Next != null && t1.Next.IsValue("ПРИ", null)) 
                {
                    Pullenti.Ner.ReferentToken rtt = t1.Kit.ProcessReferent("ORGANIZATION", t1.Next.Next);
                    if (rtt != null) 
                    {
                        emptyOrg = true;
                        rt.EndToken = (t1 = rtt.EndToken);
                    }
                }
                if (t1.Next != null && t1.Next.IsValue("АПН", null)) 
                    rt.EndToken = (t1 = t1.Next);
                if (t1.WhitespacesAfterCount < 2) 
                {
                    Pullenti.Ner.ReferentToken rtt1 = t1.Kit.ProcessReferent("ORGANIZATION", t1.Next);
                    if (rtt1 != null) 
                    {
                        emptyOrg = true;
                        rt.EndToken = (t1 = rtt1.EndToken);
                    }
                }
                if (emptyOrg && (t1.WhitespacesAfterCount < 2)) 
                {
                    Pullenti.Ner.Geo.Internal.TerrItemToken terr = Pullenti.Ner.Geo.Internal.TerrItemToken.TryParse(t1.Next, null, false, false, null);
                    if (terr != null && terr.OntoItem != null) 
                        rt.EndToken = (t1 = terr.EndToken);
                }
                if (num != null) 
                    org.AddSlot("NUMBER", num, false, 0);
                else if (t1.Next != null && ((t1.Next.IsHiphen || t1.Next.IsValue("№", null) || t1.Next.IsValue("N", null))) && (t1.Next.Next is Pullenti.Ner.NumberToken)) 
                {
                    AddressItemToken nai = AddressItemToken.TryParse(t1.Next.Next, null, false, true, null);
                    if (nai != null && nai.Typ == ItemType.Number) 
                    {
                        org.AddSlot("NUMBER", nai.Value, false, 0);
                        t1 = (rt.EndToken = nai.EndToken);
                    }
                    else 
                    {
                        t1 = (rt.EndToken = t1.Next.Next);
                        org.AddSlot("NUMBER", (t1 as Pullenti.Ner.NumberToken).Value.ToString(), false, 0);
                    }
                }
                if (tok != null && (t1.EndChar < tok.EndChar)) 
                {
                    t1 = (rt.EndToken = tok.EndToken);
                    if (t1.Next != null && (t1.WhitespacesAfterCount < 2) && t1.Next.IsValue("ТЕРРИТОРИЯ", "ТЕРИТОРІЯ")) 
                        t1 = (rt.EndToken = t1.Next);
                }
            }
            if (rt == null) 
                rt = rt1;
            else if (rt1 != null && rt1.Referent.TypeName == "ORGANIZATION") 
            {
                if (isVeryDoubt) 
                    rt = rt1;
                else 
                {
                    rt.Referent.MergeSlots(rt1.Referent, true);
                    if (rt1.EndChar > rt.EndChar) 
                        rt.EndToken = rt1.EndToken;
                }
            }
            if (rt == null) 
                return null;
            if (t.IsValue("АО", null)) 
                return null;
            if (rt.Referent.FindSlot("TYPE", "администрация", true) != null || rt.Referent.FindSlot("TYPE", "адміністрація", true) != null) 
            {
                Pullenti.Ner.Geo.GeoReferent ge = rt.Referent.GetSlotValue("GEO") as Pullenti.Ner.Geo.GeoReferent;
                if (ge != null) 
                    return new AddressItemToken((ge.IsRegion ? ItemType.Region : ItemType.City), t, rt.EndToken) { Referent = ge };
            }
            AddressItemToken res = new AddressItemToken(ItemType.Street, t, rt.EndToken) { Referent = rt.Referent, RefToken = rt, RefTokenIsGsk = typ != null };
            return res;
        }
        public Pullenti.Ner.ReferentToken CreateGeoOrgTerr()
        {
            Pullenti.Ner.Geo.GeoReferent geo = new Pullenti.Ner.Geo.GeoReferent();
            Pullenti.Ner.Token t1 = EndToken;
            geo.AddOrgReferent(Referent);
            geo.AddExtReferent(RefToken);
            if (geo.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, null, true) == null) 
                geo.AddTypTer(Kit.BaseLanguage);
            return new Pullenti.Ner.ReferentToken(geo, BeginToken, EndToken);
        }
        public static bool CheckStreetAfter(Pullenti.Ner.Token t)
        {
            int cou = 0;
            for (; t != null && (cou < 4); t = t.Next,cou++) 
            {
                if (t.IsCharOf(",.") || t.IsHiphen || t.Morph.Class.IsPreposition) 
                {
                }
                else 
                    break;
            }
            if (t == null) 
                return false;
            if (t.IsNewlineBefore) 
                return false;
            AddressItemToken ait = TryParse(t, null, false, false, null);
            if (ait != null) 
            {
                if (ait.Typ == ItemType.Street) 
                    return true;
            }
            return false;
        }
        public static bool CheckHouseAfter(Pullenti.Ner.Token t, bool leek = false, bool pureHouse = false)
        {
            if (t == null) 
                return false;
            int cou = 0;
            for (; t != null && (cou < 4); t = t.Next,cou++) 
            {
                if (t.IsCharOf(",.") || t.Morph.Class.IsPreposition) 
                {
                }
                else 
                    break;
            }
            if (t == null) 
                return false;
            if (t.IsNewlineBefore) 
                return false;
            AddressItemToken ait = TryParse(t, null, false, true, null);
            if (ait != null) 
            {
                if (pureHouse) 
                    return ait.Typ == ItemType.House || ait.Typ == ItemType.Plot;
                if ((ait.Typ == ItemType.House || ait.Typ == ItemType.Floor || ait.Typ == ItemType.Office) || ait.Typ == ItemType.Flat || ait.Typ == ItemType.Plot) 
                {
                    if (((t is Pullenti.Ner.TextToken) && t.Chars.IsAllUpper && t.Next != null) && t.Next.IsHiphen && (t.Next.Next is Pullenti.Ner.NumberToken)) 
                        return false;
                    if ((t is Pullenti.Ner.TextToken) && t.Next == ait.EndToken && t.Next.IsHiphen) 
                        return false;
                    return true;
                }
                if (leek) 
                {
                    if (ait.Typ == ItemType.Number) 
                        return true;
                }
                if (ait.Typ == ItemType.Number) 
                {
                    Pullenti.Ner.Token t1 = t.Next;
                    while (t1 != null && t1.IsCharOf(".,")) 
                    {
                        t1 = t1.Next;
                    }
                    ait = TryParse(t1, null, false, true, null);
                    if (ait != null && (((ait.Typ == ItemType.Building || ait.Typ == ItemType.Corpus || ait.Typ == ItemType.Flat) || ait.Typ == ItemType.Floor || ait.Typ == ItemType.Office))) 
                        return true;
                }
            }
            return false;
        }
        public static bool CheckKmAfter(Pullenti.Ner.Token t)
        {
            int cou = 0;
            for (; t != null && (cou < 4); t = t.Next,cou++) 
            {
                if (t.IsCharOf(",.") || t.Morph.Class.IsPreposition) 
                {
                }
                else 
                    break;
            }
            if (t == null) 
                return false;
            AddressItemToken km = TryParse(t, null, false, true, null);
            if (km != null && km.Typ == ItemType.Kilometer) 
                return true;
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective, 0, null);
            if (npt != null) 
            {
                if (npt.EndToken.IsValue("КИЛОМЕТР", null) || npt.EndToken.IsValue("МЕТР", null)) 
                    return true;
            }
            return false;
        }
        public static bool CheckKmBefore(Pullenti.Ner.Token t)
        {
            int cou = 0;
            for (; t != null && (cou < 4); t = t.Previous,cou++) 
            {
                if (t.IsCharOf(",.")) 
                {
                }
                else if (t.IsValue("КМ", null) || t.IsValue("КИЛОМЕТР", null) || t.IsValue("МЕТР", null)) 
                    return true;
            }
            return false;
        }
        public static char CorrectChar(char v)
        {
            if (v == 'A' || v == 'А') 
                return 'А';
            if (v == 'Б' || v == 'Г') 
                return v;
            if (v == 'B' || v == 'В') 
                return 'В';
            if (v == 'C' || v == 'С') 
                return 'С';
            if (v == 'D' || v == 'Д') 
                return 'Д';
            if (v == 'E' || v == 'Е') 
                return 'Е';
            if (v == 'H' || v == 'Н') 
                return 'Н';
            if (v == 'K' || v == 'К') 
                return 'К';
            return (char)0;
        }
        static string CorrectCharToken(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return null;
            string v = tt.Term;
            if (v.Length != 1) 
                return null;
            char corr = CorrectChar(v[0]);
            if (corr != ((char)0)) 
                return string.Format("{0}", corr);
            if (t.Chars.IsCyrillicLetter) 
                return v;
            return null;
        }
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            StreetItemToken.Initialize();
            m_Ontology = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("ДОМ") { Tag = ItemType.House };
            t.AddAbridge("Д.");
            t.AddVariant("КОТТЕДЖ", false);
            t.AddAbridge("КОТ.");
            t.AddVariant("ДАЧА", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("БУДИНОК") { Tag = ItemType.House, Lang = Pullenti.Morph.MorphLang.UA };
            t.AddAbridge("Б.");
            t.AddVariant("КОТЕДЖ", false);
            t.AddAbridge("БУД.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЛАДЕНИЕ") { Tag = ItemType.House, Tag2 = Pullenti.Ner.Address.AddressHouseType.Estate };
            t.AddAbridge("ВЛАД.");
            t.AddAbridge("ВЛД.");
            t.AddAbridge("ВЛ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОМОВЛАДЕНИЕ") { Tag = ItemType.House, Tag2 = Pullenti.Ner.Address.AddressHouseType.HouseEstate };
            t.AddVariant("ДОМОВЛАДЕНИЕ", false);
            t.AddAbridge("ДВЛД.");
            t.AddAbridge("ДМВЛД.");
            t.AddVariant("ДОМОВЛ", false);
            t.AddVariant("ДОМОВА", false);
            t.AddVariant("ДОМОВЛАД", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОДЪЕЗД ДОМА") { Tag = ItemType.House };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОДВАЛ ДОМА") { Tag = ItemType.House };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КРЫША ДОМА") { Tag = ItemType.House };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЭТАЖ") { Tag = ItemType.Floor };
            t.AddAbridge("ЭТ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОДЪЕЗД") { Tag = ItemType.Potch };
            t.AddAbridge("ПОД.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОРПУС") { Tag = ItemType.Corpus };
            t.AddAbridge("КОРП.");
            t.AddAbridge("КОР.");
            t.AddAbridge("Д.КОРП.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("К") { Tag = ItemType.CorpusOrFlat };
            t.AddAbridge("К.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТРОЕНИЕ") { Tag = ItemType.Building };
            t.AddAbridge("СТРОЕН.");
            t.AddAbridge("СТР.");
            t.AddAbridge("СТ.");
            t.AddAbridge("ПОМ.СТР.");
            t.AddAbridge("Д.СТР.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СООРУЖЕНИЕ") { Tag = ItemType.Building, Tag2 = Pullenti.Ner.Address.AddressBuildingType.Construction };
            t.AddAbridge("СООР.");
            t.AddAbridge("СООРУЖ.");
            t.AddAbridge("СООРУЖЕН.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛИТЕРА") { Tag = ItemType.Building, Tag2 = Pullenti.Ner.Address.AddressBuildingType.Liter };
            t.AddAbridge("ЛИТ.");
            t.AddVariant("ЛИТЕР", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("УЧАСТОК") { Tag = ItemType.Plot };
            t.AddAbridge("УЧАСТ.");
            t.AddAbridge("УЧ.");
            t.AddAbridge("УЧ-К");
            t.AddVariant("ЗЕМЕЛЬНЫЙ УЧАСТОК", false);
            t.AddAbridge("ЗЕМ.УЧ.");
            t.AddAbridge("ЗЕМ.УЧ-К");
            t.AddAbridge("З/У");
            t.AddAbridge("ПОЗ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КВАРТИРА") { Tag = ItemType.Flat };
            t.AddAbridge("КВАРТ.");
            t.AddAbridge("КВАР.");
            t.AddAbridge("КВ.");
            t.AddAbridge("КВ-РА");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОФИС") { Tag = ItemType.Office };
            t.AddAbridge("ОФ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОФІС") { Tag = ItemType.Office, Lang = Pullenti.Morph.MorphLang.UA };
            t.AddAbridge("ОФ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("БИЗНЕС-ЦЕНТР") { Tag = ItemType.BusinessCenter };
            t.Acronym = "БЦ";
            t.AddVariant("БИЗНЕС ЦЕНТР", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЛОК") { Tag = ItemType.Block };
            t.AddVariant("РЯД", false);
            t.AddVariant("СЕКТОР", false);
            t.AddAbridge("СЕК.");
            t.AddVariant("МАССИВ", false);
            t.AddVariant("ОЧЕРЕДЬ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("БОКС") { Tag = ItemType.Box };
            t.AddVariant("ГАРАЖ", false);
            t.AddVariant("САРАЙ", false);
            t.AddAbridge("ГАР.");
            t.AddVariant("МАШИНОМЕСТО", false);
            t.AddVariant("ПОМЕЩЕНИЕ", false);
            t.AddAbridge("ПОМ.");
            t.AddVariant("НЕЖИЛОЕ ПОМЕЩЕНИЕ", false);
            t.AddAbridge("Н.П.");
            t.AddAbridge("НП");
            t.AddVariant("ПОДВАЛ", false);
            t.AddVariant("ПОГРЕБ", false);
            t.AddVariant("ПОДВАЛЬНОЕ ПОМЕЩЕНИЕ", false);
            t.AddVariant("ПОДЪЕЗД", false);
            t.AddAbridge("ГАРАЖ-БОКС");
            t.AddVariant("ГАРАЖНЫЙ БОКС", false);
            t.AddAbridge("ГБ.");
            t.AddAbridge("Г.Б.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОМНАТА") { Tag = ItemType.Office };
            t.AddAbridge("КОМ.");
            t.AddAbridge("КОМН.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КАБИНЕТ") { Tag = ItemType.Office };
            t.AddAbridge("КАБ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НОМЕР") { Tag = ItemType.Number };
            t.AddAbridge("НОМ.");
            t.AddAbridge("№");
            t.AddAbridge("N");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЕЗ НОМЕРА") { CanonicText = "Б/Н", Tag = ItemType.NoNumber };
            t.AddAbridge("Б.Н.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АБОНЕНТСКИЙ ЯЩИК") { Tag = ItemType.PostOfficeBox };
            t.AddAbridge("А.Я.");
            t.AddVariant("ПОЧТОВЫЙ ЯЩИК", false);
            t.AddAbridge("П.Я.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОРОДСКАЯ СЛУЖЕБНАЯ ПОЧТА") { Tag = ItemType.CSP, Acronym = "ГСП" };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АДРЕС") { Tag = ItemType.Prefix };
            t.AddVariant("ЮРИДИЧЕСКИЙ АДРЕС", false);
            t.AddVariant("ФАКТИЧЕСКИЙ АДРЕС", false);
            t.AddAbridge("ЮР.АДРЕС");
            t.AddAbridge("ПОЧТ.АДРЕС");
            t.AddAbridge("ФАКТ.АДРЕС");
            t.AddAbridge("П.АДРЕС");
            t.AddVariant("ЮРИДИЧЕСКИЙ/ФАКТИЧЕСКИЙ АДРЕС", false);
            t.AddVariant("ПОЧТОВЫЙ АДРЕС", false);
            t.AddVariant("АДРЕС ПРОЖИВАНИЯ", false);
            t.AddVariant("МЕСТО НАХОЖДЕНИЯ", false);
            t.AddVariant("МЕСТОНАХОЖДЕНИЕ", false);
            t.AddVariant("МЕСТОПОЛОЖЕНИЕ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АДРЕСА") { Tag = ItemType.Prefix };
            t.AddVariant("ЮРИДИЧНА АДРЕСА", false);
            t.AddVariant("ФАКТИЧНА АДРЕСА", false);
            t.AddVariant("ПОШТОВА АДРЕСА", false);
            t.AddVariant("АДРЕСА ПРОЖИВАННЯ", false);
            t.AddVariant("МІСЦЕ ПЕРЕБУВАННЯ", false);
            t.AddVariant("ПРОПИСКА", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КИЛОМЕТР") { Tag = ItemType.Kilometer };
            t.AddAbridge("КИЛОМ.");
            t.AddAbridge("КМ.");
            m_Ontology.Add(t);
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ПЕРЕСЕЧЕНИЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.Cross });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("НА ПЕРЕСЕЧЕНИИ") { Tag = Pullenti.Ner.Address.AddressDetailType.Cross });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ПЕРЕКРЕСТОК") { Tag = Pullenti.Ner.Address.AddressDetailType.Cross });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("НА ПЕРЕКРЕСТКЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.Cross });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("НА ТЕРРИТОРИИ") { Tag = Pullenti.Ner.Address.AddressDetailType.Near });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("СЕРЕДИНА") { Tag = Pullenti.Ner.Address.AddressDetailType.Near });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ПРИМЫКАТЬ") { Tag = Pullenti.Ner.Address.AddressDetailType.Near });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ГРАНИЧИТЬ") { Tag = Pullenti.Ner.Address.AddressDetailType.Near });
            t = new Pullenti.Ner.Core.Termin("ВБЛИЗИ") { Tag = Pullenti.Ner.Address.AddressDetailType.Near };
            t.AddVariant("У", false);
            t.AddAbridge("ВБЛ.");
            t.AddVariant("ВОЗЛЕ", false);
            t.AddVariant("ОКОЛО", false);
            t.AddVariant("НЕДАЛЕКО ОТ", false);
            t.AddVariant("РЯДОМ С", false);
            t.AddVariant("ГРАНИЦА", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("РАЙОН") { Tag = Pullenti.Ner.Address.AddressDetailType.Near };
            t.AddAbridge("Р-Н");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("В РАЙОНЕ") { CanonicText = "РАЙОН", Tag = Pullenti.Ner.Address.AddressDetailType.Near };
            t.AddAbridge("В Р-НЕ");
            m_Ontology.Add(t);
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ПРИМЕРНО") { Tag = Pullenti.Ner.Address.AddressDetailType.Undefined });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ПОРЯДКА") { Tag = Pullenti.Ner.Address.AddressDetailType.Undefined });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ПРИБЛИЗИТЕЛЬНО") { Tag = Pullenti.Ner.Address.AddressDetailType.Undefined });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("НАПРАВЛЕНИЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.Undefined });
            t = new Pullenti.Ner.Core.Termin("ОБЩЕЖИТИЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.Hostel };
            t.AddAbridge("ОБЩ.");
            t.AddAbridge("ПОМ.ОБЩ.");
            m_Ontology.Add(t);
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("СЕВЕРНЕЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.North });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("СЕВЕР") { Tag = Pullenti.Ner.Address.AddressDetailType.North });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ЮЖНЕЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.South });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ЮГ") { Tag = Pullenti.Ner.Address.AddressDetailType.South });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ЗАПАДНЕЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.West });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ЗАПАД") { Tag = Pullenti.Ner.Address.AddressDetailType.West });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ВОСТОЧНЕЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.East });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ВОСТОК") { Tag = Pullenti.Ner.Address.AddressDetailType.East });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("СЕВЕРО-ЗАПАДНЕЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.NorthWest });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("СЕВЕРО-ЗАПАД") { Tag = Pullenti.Ner.Address.AddressDetailType.NorthWest });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("СЕВЕРО-ВОСТОЧНЕЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.NorthEast });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("СЕВЕРО-ВОСТОК") { Tag = Pullenti.Ner.Address.AddressDetailType.NorthEast });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ЮГО-ЗАПАДНЕЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.SouthWest });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ЮГО-ЗАПАД") { Tag = Pullenti.Ner.Address.AddressDetailType.SouthWest });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ЮГО-ВОСТОЧНЕЕ") { Tag = Pullenti.Ner.Address.AddressDetailType.SouthEast });
            m_Ontology.Add(new Pullenti.Ner.Core.Termin("ЮГО-ВОСТОК") { Tag = Pullenti.Ner.Address.AddressDetailType.SouthEast });
            t = new Pullenti.Ner.Core.Termin("ТАМ ЖЕ");
            t.AddAbridge("ТАМЖЕ");
            m_Ontology.Add(t);
            m_OrgOntology = new Pullenti.Ner.Core.TerminCollection();
            t = new Pullenti.Ner.Core.Termin("САДОВОЕ ТОВАРИЩЕСТВО") { Acronym = "СТ" };
            t.AddVariant("САДОВОДЧЕСКОЕ ТОВАРИЩЕСТВО", false);
            t.Acronym = "СТ";
            t.AddAbridge("С/ТОВ");
            t.AddAbridge("ПК СТ");
            t.AddAbridge("САД.ТОВ.");
            t.AddAbridge("САДОВ.ТОВ.");
            t.AddAbridge("С/Т");
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДАЧНОЕ ТОВАРИЩЕСТВО");
            t.AddAbridge("Д/Т");
            t.AddAbridge("ДАЧ/Т");
            t.Acronym = "ДТ";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("САДОВЫЙ КООПЕРАТИВ");
            t.AddAbridge("С/К");
            t.Acronym = "СК";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОТРЕБИТЕЛЬСКИЙ КООПЕРАТИВ");
            t.AddVariant("ПОТРЕБКООПЕРАТИВ", false);
            t.Acronym = "ПК";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("САДОВОДЧЕСКОЕ ДАЧНОЕ ТОВАРИЩЕСТВО");
            t.AddVariant("САДОВОЕ ДАЧНОЕ ТОВАРИЩЕСТВО", false);
            t.Acronym = "СДТ";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДАЧНОЕ НЕКОММЕРЧЕСКОЕ ОБЪЕДИНЕНИЕ");
            t.Acronym = "ДНО";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДАЧНОЕ НЕКОММЕРЧЕСКОЕ ПАРТНЕРСТВО");
            t.Acronym = "ДНП";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДАЧНОЕ НЕКОММЕРЧЕСКОЕ ТОВАРИЩЕСТВО");
            t.Acronym = "ДНТ";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДАЧНЫЙ ПОТРЕБИТЕЛЬСКИЙ КООПЕРАТИВ");
            t.Acronym = "ДПК";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДАЧНО СТРОИТЕЛЬНЫЙ КООПЕРАТИВ");
            t.AddVariant("ДАЧНЫЙ СТРОИТЕЛЬНЫЙ КООПЕРАТИВ", false);
            t.Acronym = "ДСК";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТРОИТЕЛЬНО ПРОИЗВОДСТВЕННЫЙ КООПЕРАТИВ");
            t.Acronym = "СПК";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("САДОВОДЧЕСКОЕ НЕКОММЕРЧЕСКОЕ ТОВАРИЩЕСТВО");
            t.AddVariant("САДОВОЕ НЕКОММЕРЧЕСКОЕ ТОВАРИЩЕСТВО", false);
            t.Acronym = "СНТ";
            t.AcronymCanBeLower = true;
            t.AddAbridge("САДОВОЕ НЕКОМ-Е ТОВАРИЩЕСТВО");
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("САДОВОДЧЕСКОЕ НЕКОММЕРЧЕСКОЕ ОБЪЕДИНЕНИЕ") { Acronym = "СНО", AcronymCanBeLower = true };
            t.AddVariant("САДОВОЕ НЕКОММЕРЧЕСКОЕ ОБЪЕДИНЕНИЕ", false);
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("САДОВОДЧЕСКОЕ НЕКОММЕРЧЕСКОЕ ПАРТНЕРСТВО") { Acronym = "СНП", AcronymCanBeLower = true };
            t.AddVariant("САДОВОЕ НЕКОММЕРЧЕСКОЕ ПАРТНЕРСТВО", false);
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("САДОВОДЧЕСКОЕ НЕКОММЕРЧЕСКОЕ ТОВАРИЩЕСТВО") { Acronym = "СНТ", AcronymCanBeLower = true };
            t.AddVariant("САДОВОЕ НЕКОММЕРЧЕСКОЕ ТОВАРИЩЕСТВО", false);
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НЕКОММЕРЧЕСКОЕ САДОВОДЧЕСКОЕ ТОВАРИЩЕСТВО") { Acronym = "НСТ", AcronymCanBeLower = true };
            t.AddVariant("НЕКОММЕРЧЕСКОЕ САДОВОЕ ТОВАРИЩЕСТВО", false);
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОБЪЕДИНЕННОЕ НЕКОММЕРЧЕСКОЕ САДОВОДЧЕСКОЕ ТОВАРИЩЕСТВО") { Acronym = "ОНСТ", AcronymCanBeLower = true };
            t.AddVariant("ОБЪЕДИНЕННОЕ НЕКОММЕРЧЕСКОЕ САДОВОЕ ТОВАРИЩЕСТВО", false);
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("САДОВОДЧЕСКАЯ ПОТРЕБИТЕЛЬСКАЯ КООПЕРАЦИЯ") { Acronym = "СПК", AcronymCanBeLower = true };
            t.AddVariant("САДОВАЯ ПОТРЕБИТЕЛЬСКАЯ КООПЕРАЦИЯ", false);
            m_OrgOntology.Add(t);
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ДАЧНО СТРОИТЕЛЬНО ПРОИЗВОДСТВЕННЫЙ КООПЕРАТИВ") { Acronym = "ДСПК", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ЖИЛИЩНЫЙ СТРОИТЕЛЬНО ПРОИЗВОДСТВЕННЫЙ КООПЕРАТИВ") { Acronym = "ЖСПК", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ЖИЛИЩНЫЙ СТРОИТЕЛЬНЫЙ КООПЕРАТИВ") { Acronym = "ЖСК", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ЖИЛИЩНЫЙ СТРОИТЕЛЬНЫЙ КООПЕРАТИВ ИНДИВИДУАЛЬНЫХ ЗАСТРОЙЩИКОВ") { Acronym = "ЖСКИЗ", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ОГОРОДНИЧЕСКОЕ НЕКОММЕРЧЕСКОЕ ОБЪЕДИНЕНИЕ") { Acronym = "ОНО", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ОГОРОДНИЧЕСКОЕ НЕКОММЕРЧЕСКОЕ ПАРТНЕРСТВО") { Acronym = "ОНП", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ОГОРОДНИЧЕСКОЕ НЕКОММЕРЧЕСКОЕ ТОВАРИЩЕСТВО") { Acronym = "ОНТ", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ОГОРОДНИЧЕСКИЙ ПОТРЕБИТЕЛЬСКИЙ КООПЕРАТИВ") { Acronym = "ОПК", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ТОВАРИЩЕСТВО СОБСТВЕННИКОВ НЕДВИЖИМОСТИ") { Acronym = "СТСН", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("САДОВОДЧЕСКОЕ ТОВАРИЩЕСТВО СОБСТВЕННИКОВ НЕДВИЖИМОСТИ") { Acronym = "ТСН", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ТОВАРИЩЕСТВО СОБСТВЕННИКОВ ЖИЛЬЯ") { Acronym = "ТСЖ", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("САДОВЫЕ ЗЕМЕЛЬНЫЕ УЧАСТКИ") { Acronym = "СЗУ", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ТОВАРИЩЕСТВО ИНДИВИДУАЛЬНЫХ ЗАСТРОЙЩИКОВ") { Acronym = "ТИЗ", AcronymCanBeLower = true });
            t = new Pullenti.Ner.Core.Termin("КОЛЛЕКТИВ ИНДИВИДУАЛЬНЫХ ЗАСТРОЙЩИКОВ") { Acronym = "КИЗ", AcronymCanBeLower = true };
            t.AddVariant("КИЗК", false);
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("САДОВОЕ НЕКОММЕРЧЕСКОЕ ТОВАРИЩЕСТВО СОБСТВЕННИКОВ НЕДВИЖИМОСТИ") { Acronym = "СНТСН", AcronymCanBeLower = true };
            t.AddVariant("СНТ СН", false);
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СОВМЕСТНОЕ ПРЕДПРИЯТИЕ");
            t.Acronym = "СП";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НЕКОММЕРЧЕСКОЕ ПАРТНЕРСТВО");
            t.Acronym = "НП";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АВТОМОБИЛЬНЫЙ КООПЕРАТИВ");
            t.AddAbridge("А/К");
            t.Acronym = "АК";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГАРАЖНЫЙ КООПЕРАТИВ");
            t.AddAbridge("Г/К");
            t.AddAbridge("ГР.КОП.");
            t.AddAbridge("ГАР.КОП.");
            t.Acronym = "ГК";
            t.AcronymCanBeLower = true;
            m_OrgOntology.Add(t);
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ГАРАЖНО СТРОИТЕЛЬНЫЙ КООПЕРАТИВ") { Acronym = "ГСК", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ГАРАЖНО ЭКСПЛУАТАЦИОННЫЙ КООПЕРАТИВ") { Acronym = "ГЭК", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ГАРАЖНО ПОТРЕБИТЕЛЬСКИЙ КООПЕРАТИВ") { Acronym = "ГПК", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ПОТРЕБИТЕЛЬСКИЙ ГАРАЖНО СТРОИТЕЛЬНЫЙ КООПЕРАТИВ") { Acronym = "ПГСК", AcronymCanBeLower = true });
            m_OrgOntology.Add(new Pullenti.Ner.Core.Termin("ГАРАЖНЫЙ СТРОИТЕЛЬНО ПОТРЕБИТЕЛЬСКИЙ КООПЕРАТИВ") { Acronym = "ГСПК", AcronymCanBeLower = true });
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("САНАТОРИЙ");
            t.AddAbridge("САН.");
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОМ ОТДЫХА");
            t.AddAbridge("Д/О");
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СОВХОЗ");
            t.AddAbridge("С-ЗА");
            t.AddAbridge("С/ЗА");
            t.AddAbridge("С/З");
            t.AddAbridge("СХ.");
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПИОНЕРСКИЙ ЛАГЕРЬ");
            t.AddAbridge("П/Л");
            t.AddAbridge("П.Л.");
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КУРОРТ");
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОЛЛЕКТИВ ИНДИВИДУАЛЬНЫХ ВЛАДЕЛЬЦЕВ");
            m_OrgOntology.Add(t);
            t = new Pullenti.Ner.Core.Termin("БИЗНЕС ЦЕНТР");
            t.Acronym = "БЦ";
            t.AddVariant("БІЗНЕС ЦЕНТР", false);
            m_OrgOntology.Add(t);
        }
        static Pullenti.Ner.Core.TerminCollection m_Ontology;
        static Pullenti.Ner.Core.TerminCollection m_OrgOntology;
    }
}