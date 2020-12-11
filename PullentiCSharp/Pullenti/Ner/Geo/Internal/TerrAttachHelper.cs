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

namespace Pullenti.Ner.Geo.Internal
{
    static class TerrAttachHelper
    {
        static Pullenti.Ner.ReferentToken _tryAttachMoscowAO(List<TerrItemToken> li, Pullenti.Ner.Core.AnalyzerData ad)
        {
            if (li[0].TerminItem == null || !li[0].TerminItem.IsMoscowRegion) 
                return null;
            if (li[0].IsDoubt) 
            {
                bool ok = false;
                if (CityAttachHelper.CheckCityAfter(li[0].EndToken.Next)) 
                    ok = true;
                else 
                {
                    List<Pullenti.Ner.Address.Internal.AddressItemToken> ali = Pullenti.Ner.Address.Internal.AddressItemToken.TryParseList(li[0].EndToken.Next, null, 2);
                    if (ali != null && ali.Count > 0 && ali[0].Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street) 
                        ok = true;
                }
                if (!ok) 
                    return null;
            }
            Pullenti.Ner.Geo.GeoReferent reg = new Pullenti.Ner.Geo.GeoReferent();
            string typ = "АДМИНИСТРАТИВНЫЙ ОКРУГ";
            reg.AddTyp(typ);
            string name = li[0].TerminItem.CanonicText;
            if (Pullenti.Morph.LanguageHelper.EndsWith(name, typ)) 
                name = name.Substring(0, name.Length - typ.Length - 1).Trim();
            reg.AddName(name);
            return new Pullenti.Ner.ReferentToken(reg, li[0].BeginToken, li[0].EndToken);
        }
        static Pullenti.Ner.ReferentToken _tryAttachPureTerr(List<TerrItemToken> li, Pullenti.Ner.Core.AnalyzerData ad)
        {
            Pullenti.Ner.Address.Internal.AddressItemToken aid = null;
            Pullenti.Ner.Token t = li[0].EndToken.Next;
            if (t == null) 
                return null;
            Pullenti.Ner.Token tt = t;
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false)) 
                tt = tt.Next;
            if (li.Count > 1) 
            {
                List<TerrItemToken> tmp = new List<TerrItemToken>(li);
                tmp.RemoveAt(0);
                Pullenti.Ner.ReferentToken rt0 = TryAttachTerritory(tmp, ad, false, null, null);
                if (rt0 == null && tmp.Count == 2) 
                {
                    if (((tmp[0].TerminItem == null && tmp[1].TerminItem != null)) || ((tmp[0].TerminItem != null && tmp[1].TerminItem == null))) 
                    {
                        if (aid == null) 
                            rt0 = TryAttachTerritory(tmp, ad, true, null, null);
                    }
                }
                if (rt0 != null) 
                {
                    if ((rt0.Referent as Pullenti.Ner.Geo.GeoReferent).IsState) 
                        return null;
                    rt0.BeginToken = li[0].BeginToken;
                    rt0.Morph = li[0].Morph;
                    return rt0;
                }
            }
            if (aid == null) 
                aid = Pullenti.Ner.Address.Internal.AddressItemToken.TryAttachOrg(tt);
            if (aid != null) 
            {
                Pullenti.Ner.ReferentToken rt = aid.CreateGeoOrgTerr();
                if (rt == null) 
                    return null;
                rt.BeginToken = li[0].BeginToken;
                Pullenti.Ner.Token t1 = rt.EndToken;
                if (tt != t && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next, false, null, false)) 
                    rt.EndToken = (t1 = t1.Next);
                return rt;
            }
            return null;
        }
        public static Pullenti.Ner.ReferentToken TryAttachTerritory(List<TerrItemToken> li, Pullenti.Ner.Core.AnalyzerData ad, bool attachAlways = false, List<CityItemToken> cits = null, List<Pullenti.Ner.Geo.GeoReferent> exists = null)
        {
            if (li == null || li.Count == 0) 
                return null;
            TerrItemToken exObj = null;
            TerrItemToken newName = null;
            List<TerrItemToken> adjList = new List<TerrItemToken>();
            TerrItemToken noun = null;
            TerrItemToken addNoun = null;
            Pullenti.Ner.ReferentToken rt = _tryAttachMoscowAO(li, ad);
            if (rt != null) 
                return rt;
            if (li[0].TerminItem != null && li[0].TerminItem.CanonicText == "ТЕРРИТОРИЯ") 
            {
                Pullenti.Ner.ReferentToken res2 = _tryAttachPureTerr(li, ad);
                return res2;
            }
            if (li.Count == 2) 
            {
                if (li[0].Rzd != null && li[1].RzdDir != null) 
                {
                    Pullenti.Ner.Geo.GeoReferent rzd = new Pullenti.Ner.Geo.GeoReferent();
                    rzd.AddName(li[1].RzdDir);
                    rzd.AddTypTer(li[0].Kit.BaseLanguage);
                    rzd.AddSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_REF, li[0].Rzd.Referent, false, 0);
                    rzd.AddExtReferent(li[0].Rzd);
                    return new Pullenti.Ner.ReferentToken(rzd, li[0].BeginToken, li[1].EndToken);
                }
                if (li[1].Rzd != null && li[0].RzdDir != null) 
                {
                    Pullenti.Ner.Geo.GeoReferent rzd = new Pullenti.Ner.Geo.GeoReferent();
                    rzd.AddName(li[0].RzdDir);
                    rzd.AddTypTer(li[0].Kit.BaseLanguage);
                    rzd.AddSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_REF, li[1].Rzd.Referent, false, 0);
                    rzd.AddExtReferent(li[1].Rzd);
                    return new Pullenti.Ner.ReferentToken(rzd, li[0].BeginToken, li[1].EndToken);
                }
            }
            bool canBeCityBefore = false;
            bool adjTerrBefore = false;
            if (cits != null) 
            {
                if (cits[0].Typ == CityItemToken.ItemType.City) 
                    canBeCityBefore = true;
                else if (cits[0].Typ == CityItemToken.ItemType.Noun && cits.Count > 1) 
                    canBeCityBefore = true;
            }
            int k;
            for (k = 0; k < li.Count; k++) 
            {
                if (li[k].OntoItem != null) 
                {
                    if (exObj != null || newName != null) 
                        break;
                    if (noun != null) 
                    {
                        if (k == 1) 
                        {
                            if (noun.TerminItem.CanonicText == "РАЙОН" || noun.TerminItem.CanonicText == "ОБЛАСТЬ" || noun.TerminItem.CanonicText == "СОЮЗ") 
                            {
                                if (li[k].OntoItem.Referent is Pullenti.Ner.Geo.GeoReferent) 
                                {
                                    if ((li[k].OntoItem.Referent as Pullenti.Ner.Geo.GeoReferent).IsState) 
                                        break;
                                }
                                bool ok = false;
                                Pullenti.Ner.Token tt = li[k].EndToken.Next;
                                if (tt == null) 
                                    ok = true;
                                else if (tt.IsCharOf(",.")) 
                                    ok = true;
                                if (!ok) 
                                    ok = MiscLocationHelper.CheckGeoObjectBefore(li[0].BeginToken);
                                if (!ok) 
                                {
                                    Pullenti.Ner.Address.Internal.AddressItemToken adr = Pullenti.Ner.Address.Internal.AddressItemToken.TryParse(tt, null, false, false, null);
                                    if (adr != null) 
                                    {
                                        if (adr.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Street) 
                                            ok = true;
                                    }
                                }
                                if (!ok) 
                                    break;
                            }
                            if (li[k].OntoItem != null) 
                            {
                                if (noun.BeginToken.IsValue("МО", null) || noun.BeginToken.IsValue("ЛО", null)) 
                                    return null;
                            }
                        }
                    }
                    exObj = li[k];
                }
                else if (li[k].TerminItem != null) 
                {
                    if (noun != null) 
                        break;
                    if (li[k].TerminItem.IsAlwaysPrefix && k > 0) 
                        break;
                    if (k > 0 && li[k].IsDoubt) 
                    {
                        if (li[k].BeginToken == li[k].EndToken && li[k].BeginToken.IsValue("ЗАО", null)) 
                            break;
                    }
                    if (li[k].TerminItem.IsAdjective || li[k].IsGeoInDictionary) 
                        adjList.Add(li[k]);
                    else 
                    {
                        if (exObj != null) 
                        {
                            Pullenti.Ner.Geo.GeoReferent geo = exObj.OntoItem.Referent as Pullenti.Ner.Geo.GeoReferent;
                            if (geo == null) 
                                break;
                            if (exObj.IsAdjective && ((li[k].TerminItem.CanonicText == "СОЮЗ" || li[k].TerminItem.CanonicText == "ФЕДЕРАЦИЯ"))) 
                            {
                                string str = exObj.OntoItem.ToString();
                                if (!str.Contains(li[k].TerminItem.CanonicText)) 
                                    return null;
                            }
                            if (li[k].TerminItem.CanonicText == "РАЙОН" || li[k].TerminItem.CanonicText == "ОКРУГ" || li[k].TerminItem.CanonicText == "КРАЙ") 
                            {
                                StringBuilder tmp = new StringBuilder();
                                foreach (Pullenti.Ner.Slot s in geo.Slots) 
                                {
                                    if (s.TypeName == Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE) 
                                        tmp.AppendFormat("{0};", s.Value);
                                }
                                if (!tmp.ToString().ToUpper().Contains(li[k].TerminItem.CanonicText)) 
                                {
                                    if (k != 1 || newName != null) 
                                        break;
                                    newName = li[0];
                                    newName.IsAdjective = true;
                                    newName.OntoItem = null;
                                    exObj = null;
                                }
                            }
                        }
                        noun = li[k];
                        if (k == 0) 
                        {
                            TerrItemToken tt = TerrItemToken.TryParse(li[k].BeginToken.Previous, null, true, false, null);
                            if (tt != null && tt.Morph.Class.IsAdjective) 
                                adjTerrBefore = true;
                        }
                    }
                }
                else 
                {
                    if (exObj != null) 
                        break;
                    if (newName != null) 
                        break;
                    newName = li[k];
                }
            }
            string name = null;
            string altName = null;
            string fullName = null;
            Pullenti.Ner.MorphCollection morph = null;
            if (exObj != null) 
            {
                if (exObj.IsAdjective && !exObj.Morph.Language.IsEn && noun == null) 
                {
                    if (attachAlways && exObj.EndToken.Next != null) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(exObj.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (exObj.EndToken.Next.IsCommaAnd) 
                        {
                        }
                        else if (npt == null) 
                        {
                        }
                        else 
                        {
                            Pullenti.Ner.Address.Internal.StreetItemToken str = Pullenti.Ner.Address.Internal.StreetItemToken.TryParse(exObj.EndToken.Next, null, false, null, false);
                            if (str != null) 
                            {
                                if (str.Typ == Pullenti.Ner.Address.Internal.StreetItemType.Noun && str.EndToken == npt.EndToken) 
                                    return null;
                            }
                        }
                    }
                    else 
                    {
                        CityItemToken cit = CityItemToken.TryParse(exObj.EndToken.Next, null, false, null);
                        if (cit != null && ((cit.Typ == CityItemToken.ItemType.Noun || cit.Typ == CityItemToken.ItemType.City))) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(exObj.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt != null && npt.EndToken == cit.EndToken) 
                            {
                            }
                            else 
                                return null;
                        }
                        else if (exObj.BeginToken.IsValue("ПОДНЕБЕСНЫЙ", null)) 
                        {
                        }
                        else 
                            return null;
                    }
                }
                if (noun == null && exObj.CanBeCity) 
                {
                    CityItemToken cit0 = CityItemToken.TryParseBack(exObj.BeginToken.Previous);
                    if (cit0 != null && cit0.Typ != CityItemToken.ItemType.ProperName) 
                        return null;
                }
                if (exObj.IsDoubt && noun == null) 
                {
                    bool ok2 = false;
                    if (_canBeGeoAfter(exObj.EndToken.Next)) 
                        ok2 = true;
                    else if (!exObj.CanBeSurname && !exObj.CanBeCity) 
                    {
                        if ((exObj.EndToken.Next != null && exObj.EndToken.Next.IsChar(')') && exObj.BeginToken.Previous != null) && exObj.BeginToken.Previous.IsChar('(')) 
                            ok2 = true;
                        else if (exObj.Chars.IsLatinLetter && exObj.BeginToken.Previous != null) 
                        {
                            if (exObj.BeginToken.Previous.IsValue("IN", null)) 
                                ok2 = true;
                            else if (exObj.BeginToken.Previous.IsValue("THE", null) && exObj.BeginToken.Previous.Previous != null && exObj.BeginToken.Previous.Previous.IsValue("IN", null)) 
                                ok2 = true;
                        }
                    }
                    if (!ok2) 
                    {
                        CityItemToken cit0 = CityItemToken.TryParseBack(exObj.BeginToken.Previous);
                        if (cit0 != null && cit0.Typ != CityItemToken.ItemType.ProperName) 
                        {
                        }
                        else if (MiscLocationHelper.CheckGeoObjectBefore(exObj.BeginToken.Previous)) 
                        {
                        }
                        else 
                            return null;
                    }
                }
                name = exObj.OntoItem.CanonicText;
                morph = exObj.Morph;
            }
            else if (newName != null) 
            {
                if (noun == null) 
                    return null;
                for (int j = 1; j < k; j++) 
                {
                    if (li[j].IsNewlineBefore && !li[0].IsNewlineBefore) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(li[j].BeginToken, false, false)) 
                        {
                        }
                        else 
                            return null;
                    }
                }
                morph = noun.Morph;
                if (newName.IsAdjective) 
                {
                    if (noun.TerminItem.Acronym == "АО") 
                    {
                        if (noun.BeginToken != noun.EndToken) 
                            return null;
                        if (newName.Morph.Gender != Pullenti.Morph.MorphGender.Feminie) 
                            return null;
                    }
                    Pullenti.Ner.Geo.GeoReferent geoBefore = null;
                    Pullenti.Ner.Token tt0 = li[0].BeginToken.Previous;
                    if (tt0 != null && tt0.IsCommaAnd) 
                        tt0 = tt0.Previous;
                    if (!li[0].IsNewlineBefore && tt0 != null) 
                        geoBefore = tt0.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                    if (li.IndexOf(noun) < li.IndexOf(newName)) 
                    {
                        if (noun.TerminItem.IsState) 
                            return null;
                        if (newName.CanBeSurname && geoBefore == null) 
                        {
                            if (((noun.Morph.Case & newName.Morph.Case)).IsUndefined) 
                                return null;
                        }
                        if (Pullenti.Ner.Core.MiscHelper.IsExistsInDictionary(newName.BeginToken, newName.EndToken, Pullenti.Morph.MorphClass.Adjective | Pullenti.Morph.MorphClass.Pronoun | Pullenti.Morph.MorphClass.Verb)) 
                        {
                            if (noun.BeginToken != newName.BeginToken) 
                            {
                                if (geoBefore == null) 
                                {
                                    if (li.Count == 2 && _canBeGeoAfter(li[1].EndToken.Next)) 
                                    {
                                    }
                                    else if (li.Count == 3 && li[2].TerminItem != null && _canBeGeoAfter(li[2].EndToken.Next)) 
                                    {
                                    }
                                    else if (newName.IsGeoInDictionary) 
                                    {
                                    }
                                    else if (newName.EndToken.IsNewlineAfter) 
                                    {
                                    }
                                    else 
                                        return null;
                                }
                            }
                        }
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(newName.EndToken, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns, 0, null);
                        if (npt != null && npt.EndToken != newName.EndToken) 
                        {
                            if (li.Count >= 3 && li[2].TerminItem != null && npt.EndToken == li[2].EndToken) 
                                addNoun = li[2];
                            else 
                                return null;
                        }
                        Pullenti.Ner.ReferentToken rtp = newName.Kit.ProcessReferent("PERSON", newName.BeginToken);
                        if (rtp != null) 
                            return null;
                        name = Pullenti.Ner.Core.ProperNameHelper.GetNameEx(newName.BeginToken, newName.EndToken, Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphCase.Undefined, noun.TerminItem.Gender, false, false);
                    }
                    else 
                    {
                        bool ok = false;
                        if (((k + 1) < li.Count) && li[k].TerminItem == null && li[k + 1].TerminItem != null) 
                            ok = true;
                        else if ((k < li.Count) && li[k].OntoItem != null) 
                            ok = true;
                        else if (k == li.Count && !newName.IsAdjInDictionary) 
                            ok = true;
                        else if (MiscLocationHelper.CheckGeoObjectBefore(li[0].BeginToken) || canBeCityBefore) 
                            ok = true;
                        else if (MiscLocationHelper.CheckGeoObjectAfter(li[k - 1].EndToken, false)) 
                            ok = true;
                        else if (li.Count == 3 && k == 2) 
                        {
                            CityItemToken cit = CityItemToken.TryParse(li[2].BeginToken, null, false, null);
                            if (cit != null) 
                            {
                                if (cit.Typ == CityItemToken.ItemType.City || cit.Typ == CityItemToken.ItemType.Noun) 
                                    ok = true;
                            }
                        }
                        else if (li.Count == 2) 
                            ok = _canBeGeoAfter(li[li.Count - 1].EndToken.Next);
                        if (!ok && !li[0].IsNewlineBefore && !li[0].Chars.IsAllLower) 
                        {
                            Pullenti.Ner.ReferentToken rt00 = li[0].Kit.ProcessReferent("PERSONPROPERTY", li[0].BeginToken.Previous);
                            if (rt00 != null) 
                                ok = true;
                        }
                        if (noun.TerminItem != null && noun.TerminItem.IsStrong && newName.IsAdjective) 
                            ok = true;
                        if (noun.IsDoubt && adjList.Count == 0 && geoBefore == null) 
                            return null;
                        name = Pullenti.Ner.Core.ProperNameHelper.GetNameEx(newName.BeginToken, newName.EndToken, Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphCase.Undefined, noun.TerminItem.Gender, false, false);
                        if (!ok && !attachAlways) 
                        {
                            if (Pullenti.Ner.Core.MiscHelper.IsExistsInDictionary(newName.BeginToken, newName.EndToken, Pullenti.Morph.MorphClass.Adjective | Pullenti.Morph.MorphClass.Pronoun | Pullenti.Morph.MorphClass.Verb)) 
                            {
                                if (exists != null) 
                                {
                                    foreach (Pullenti.Ner.Geo.GeoReferent e in exists) 
                                    {
                                        if (e.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME, name, true) != null) 
                                        {
                                            ok = true;
                                            break;
                                        }
                                    }
                                }
                                if (!ok) 
                                    return null;
                            }
                        }
                        fullName = string.Format("{0} {1}", Pullenti.Ner.Core.ProperNameHelper.GetNameEx(li[0].BeginToken, noun.BeginToken.Previous, Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphCase.Undefined, noun.TerminItem.Gender, false, false), noun.TerminItem.CanonicText);
                    }
                }
                else 
                {
                    if (!attachAlways || ((noun.TerminItem != null && noun.TerminItem.CanonicText == "ФЕДЕРАЦИЯ"))) 
                    {
                        bool isLatin = noun.Chars.IsLatinLetter && newName.Chars.IsLatinLetter;
                        if (li.IndexOf(noun) > li.IndexOf(newName)) 
                        {
                            if (!isLatin) 
                                return null;
                        }
                        if (!newName.IsDistrictName && !Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(newName.BeginToken, false, false)) 
                        {
                            if (adjList.Count == 0 && Pullenti.Ner.Core.MiscHelper.IsExistsInDictionary(newName.BeginToken, newName.EndToken, Pullenti.Morph.MorphClass.Noun | Pullenti.Morph.MorphClass.Pronoun)) 
                            {
                                if (li.Count == 2 && noun.IsCityRegion && (noun.WhitespacesAfterCount < 2)) 
                                {
                                }
                                else 
                                    return null;
                            }
                            if (!isLatin) 
                            {
                                if ((noun.TerminItem.IsRegion && !attachAlways && ((!adjTerrBefore || newName.IsDoubt))) && !noun.IsCityRegion && !noun.TerminItem.IsSpecificPrefix) 
                                {
                                    if (!MiscLocationHelper.CheckGeoObjectBefore(noun.BeginToken)) 
                                    {
                                        if (!noun.IsDoubt && noun.BeginToken != noun.EndToken) 
                                        {
                                        }
                                        else if ((noun.TerminItem.IsAlwaysPrefix && li.Count == 2 && li[0] == noun) && li[1] == newName) 
                                        {
                                        }
                                        else 
                                            return null;
                                    }
                                }
                                if (noun.IsDoubt && adjList.Count == 0) 
                                {
                                    if (noun.TerminItem.Acronym == "МО" || noun.TerminItem.Acronym == "ЛО") 
                                    {
                                        if (k == (li.Count - 1) && li[k].TerminItem != null) 
                                        {
                                            addNoun = li[k];
                                            k++;
                                        }
                                        else if (li.Count == 2 && noun == li[0] && newName.ToString().EndsWith("совет")) 
                                        {
                                        }
                                        else 
                                            return null;
                                    }
                                    else 
                                        return null;
                                }
                                Pullenti.Ner.ReferentToken pers = newName.Kit.ProcessReferent("PERSON", newName.BeginToken);
                                if (pers != null) 
                                    return null;
                            }
                        }
                    }
                    name = Pullenti.Ner.Core.MiscHelper.GetTextValue(newName.BeginToken, newName.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    if (newName.BeginToken != newName.EndToken) 
                    {
                        for (Pullenti.Ner.Token ttt = newName.BeginToken.Next; ttt != null && ttt.EndChar <= newName.EndChar; ttt = ttt.Next) 
                        {
                            if (ttt.Chars.IsLetter) 
                            {
                                TerrItemToken ty = TerrItemToken.TryParse(ttt, null, false, false, null);
                                if ((ty != null && ty.TerminItem != null && noun != null) && ((ty.TerminItem.CanonicText.Contains(noun.TerminItem.CanonicText) || noun.TerminItem.CanonicText.Contains(ty.TerminItem.CanonicText)))) 
                                {
                                    name = Pullenti.Ner.Core.MiscHelper.GetTextValue(newName.BeginToken, ttt.Previous, Pullenti.Ner.Core.GetTextAttr.No);
                                    break;
                                }
                            }
                        }
                    }
                    if (adjList.Count > 0) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(adjList[0].BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null && npt.EndToken == noun.EndToken) 
                            altName = string.Format("{0} {1}", npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false), name);
                    }
                }
            }
            else 
            {
                if ((li.Count == 1 && noun != null && noun.EndToken.Next != null) && (noun.EndToken.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    Pullenti.Ner.Geo.GeoReferent g = noun.EndToken.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                    if (noun.TerminItem != null) 
                    {
                        string tyy = noun.TerminItem.CanonicText.ToLower();
                        bool ooo = false;
                        if (g.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, tyy, true) != null) 
                            ooo = true;
                        else if (tyy.EndsWith("район") && g.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, "район", true) != null) 
                            ooo = true;
                        if (ooo) 
                            return new Pullenti.Ner.ReferentToken(g, noun.BeginToken, noun.EndToken.Next) { Morph = noun.BeginToken.Morph };
                    }
                }
                if ((li.Count == 1 && noun == li[0] && li[0].TerminItem != null) && TerrItemToken.TryParse(li[0].EndToken.Next, null, true, false, null) == null && TerrItemToken.TryParse(li[0].BeginToken.Previous, null, true, false, null) == null) 
                {
                    if (li[0].Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                        return null;
                    int cou = 0;
                    string str = li[0].TerminItem.CanonicText.ToLower();
                    for (Pullenti.Ner.Token tt = li[0].BeginToken.Previous; tt != null; tt = tt.Previous) 
                    {
                        if (tt.IsNewlineAfter) 
                            cou += 10;
                        else 
                            cou++;
                        if (cou > 500) 
                            break;
                        Pullenti.Ner.Geo.GeoReferent g = tt.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                        if (g == null) 
                            continue;
                        bool ok = true;
                        cou = 0;
                        for (tt = li[0].EndToken.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.IsNewlineBefore) 
                                cou += 10;
                            else 
                                cou++;
                            if (cou > 500) 
                                break;
                            TerrItemToken tee = TerrItemToken.TryParse(tt, null, true, false, null);
                            if (tee == null) 
                                continue;
                            ok = false;
                            break;
                        }
                        if (ok) 
                        {
                            for (int ii = 0; g != null && (ii < 3); g = g.Higher,ii++) 
                            {
                                if (g.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, str, true) != null) 
                                    return new Pullenti.Ner.ReferentToken(g, li[0].BeginToken, li[0].EndToken) { Morph = noun.BeginToken.Morph };
                            }
                        }
                        break;
                    }
                }
                return null;
            }
            Pullenti.Ner.Geo.GeoReferent ter = null;
            if (exObj != null && (exObj.Tag is Pullenti.Ner.Geo.GeoReferent)) 
                ter = exObj.Tag as Pullenti.Ner.Geo.GeoReferent;
            else 
            {
                ter = new Pullenti.Ner.Geo.GeoReferent();
                if (exObj != null) 
                {
                    Pullenti.Ner.Geo.GeoReferent geo = exObj.OntoItem.Referent as Pullenti.Ner.Geo.GeoReferent;
                    if (geo != null && !geo.IsCity) 
                        ter.MergeSlots2(geo, li[0].Kit.BaseLanguage);
                    else 
                        ter.AddName(name);
                    if (noun == null && exObj.CanBeCity) 
                        ter.AddTypCity(li[0].Kit.BaseLanguage);
                    else 
                    {
                    }
                }
                else if (newName != null) 
                {
                    ter.AddName(name);
                    if (altName != null) 
                        ter.AddName(altName);
                }
                if (noun != null) 
                {
                    if (noun.TerminItem.CanonicText == "АО") 
                        ter.AddTyp((li[0].Kit.BaseLanguage.IsUa ? "АВТОНОМНИЙ ОКРУГ" : "АВТОНОМНЫЙ ОКРУГ"));
                    else if (noun.TerminItem.CanonicText == "МУНИЦИПАЛЬНОЕ СОБРАНИЕ" || noun.TerminItem.CanonicText == "МУНІЦИПАЛЬНЕ ЗБОРИ") 
                        ter.AddTyp((li[0].Kit.BaseLanguage.IsUa ? "МУНІЦИПАЛЬНЕ УТВОРЕННЯ" : "МУНИЦИПАЛЬНОЕ ОБРАЗОВАНИЕ"));
                    else if (noun.TerminItem.Acronym == "МО" && addNoun != null) 
                        ter.AddTyp(addNoun.TerminItem.CanonicText);
                    else 
                    {
                        if (noun.TerminItem.CanonicText == "СОЮЗ" && exObj != null && exObj.EndChar > noun.EndChar) 
                            return new Pullenti.Ner.ReferentToken(ter, exObj.BeginToken, exObj.EndToken) { Morph = exObj.Morph };
                        ter.AddTyp(noun.TerminItem.CanonicText);
                        if (noun.TerminItem.IsRegion && ter.IsState) 
                            ter.AddTypReg(li[0].Kit.BaseLanguage);
                    }
                }
                if (ter.IsState && ter.IsRegion) 
                {
                    foreach (TerrItemToken a in adjList) 
                    {
                        if (a.TerminItem.IsRegion) 
                        {
                            ter.AddTypReg(li[0].Kit.BaseLanguage);
                            break;
                        }
                    }
                }
                if (ter.IsState) 
                {
                    if (fullName != null) 
                        ter.AddName(fullName);
                }
            }
            Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(ter, li[0].BeginToken, li[k - 1].EndToken);
            if (noun != null && noun.Morph.Class.IsNoun) 
                res.Morph = noun.Morph;
            else 
            {
                res.Morph = new Pullenti.Ner.MorphCollection();
                for (int ii = 0; ii < k; ii++) 
                {
                    foreach (Pullenti.Morph.MorphBaseInfo v in li[ii].Morph.Items) 
                    {
                        Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo();
                        bi.CopyFrom(v);
                        if (noun != null) 
                        {
                            if (bi.Class.IsAdjective) 
                                bi.Class = Pullenti.Morph.MorphClass.Noun;
                        }
                        res.Morph.AddItem(bi);
                    }
                }
            }
            if (li[0].TerminItem != null && li[0].TerminItem.IsSpecificPrefix) 
                res.BeginToken = li[0].EndToken.Next;
            if (addNoun != null && addNoun.EndChar > res.EndChar) 
                res.EndToken = addNoun.EndToken;
            if ((res.BeginToken.Previous is Pullenti.Ner.TextToken) && (res.WhitespacesBeforeCount < 2)) 
            {
                Pullenti.Ner.TextToken tt = res.BeginToken.Previous as Pullenti.Ner.TextToken;
                if (tt.Term == "АР") 
                {
                    foreach (string ty in ter.Typs) 
                    {
                        if (ty.Contains("республика") || ty.Contains("республіка")) 
                        {
                            res.BeginToken = tt;
                            break;
                        }
                    }
                }
            }
            return res;
        }
        static bool _canBeGeoAfter(Pullenti.Ner.Token tt)
        {
            while (tt != null && ((tt.IsComma || Pullenti.Ner.Core.BracketHelper.IsBracket(tt, true)))) 
            {
                tt = tt.Next;
            }
            if (tt == null) 
                return false;
            if (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                return true;
            List<TerrItemToken> tli = TerrItemToken.TryParseList(tt, null, 2);
            if (tli != null && tli.Count > 1) 
            {
                if (tli[0].TerminItem == null && tli[1].TerminItem != null) 
                    return true;
                else if (tli[0].TerminItem != null && tli[1].TerminItem == null) 
                    return true;
            }
            if (CityAttachHelper.CheckCityAfter(tt)) 
                return true;
            if (TryAttachStateUSATerritory(tt) != null) 
                return true;
            return false;
        }
        public static Pullenti.Ner.ReferentToken TryAttachStateUSATerritory(Pullenti.Ner.Token t)
        {
            if (t == null || !t.Chars.IsLatinLetter) 
                return null;
            Pullenti.Ner.Core.TerminToken tok = TerrItemToken.m_GeoAbbrs.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok == null) 
                return null;
            Pullenti.Ner.Geo.GeoReferent g = tok.Termin.Tag as Pullenti.Ner.Geo.GeoReferent;
            if (g == null) 
                return null;
            if (tok.EndToken.Next != null && tok.EndToken.Next.IsChar('.')) 
                tok.EndToken = tok.EndToken.Next;
            Pullenti.Ner.Referent gg = g.Clone();
            gg.Occurrence.Clear();
            return new Pullenti.Ner.ReferentToken(gg, tok.BeginToken, tok.EndToken);
        }
    }
}