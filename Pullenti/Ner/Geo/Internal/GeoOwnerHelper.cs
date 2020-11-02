/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Geo.Internal
{
    public static class GeoOwnerHelper
    {
        static string _getTypesString(Pullenti.Ner.Geo.GeoReferent g)
        {
            StringBuilder tmp = new StringBuilder();
            foreach (Pullenti.Ner.Slot s in g.Slots) 
            {
                if (s.TypeName == Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE) 
                    tmp.AppendFormat("{0};", s.Value);
            }
            return tmp.ToString();
        }
        internal static bool CanBeHigherToken(Pullenti.Ner.Token rhi, Pullenti.Ner.Token rlo)
        {
            if (rhi == null || rlo == null) 
                return false;
            if (rhi.Morph.Case.IsInstrumental && !rhi.Morph.Case.IsGenitive) 
                return false;
            Pullenti.Ner.Geo.GeoReferent hi = rhi.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
            Pullenti.Ner.Geo.GeoReferent lo = rlo.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
            if (hi == null || lo == null) 
                return false;
            bool citiInReg = false;
            if (hi.IsCity && lo.IsRegion) 
            {
                if (hi.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, "город", true) != null || hi.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, "місто", true) != null || hi.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, "city", true) != null) 
                {
                    string s = _getTypesString(lo);
                    if (((s.Contains("район") || s.Contains("административный округ") || s.Contains("муниципальный округ")) || s.Contains("адміністративний округ") || s.Contains("муніципальний округ")) || lo.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, "округ", true) != null) 
                    {
                        if (rhi.Next == rlo && rlo.Morph.Case.IsGenitive) 
                            citiInReg = true;
                    }
                }
            }
            if (hi.IsRegion && lo.IsCity) 
            {
                if (lo.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, "город", true) != null || lo.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, "місто", true) != null || lo.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, "city", true) != null) 
                {
                    string s = _getTypesString(hi);
                    if (s == "район;") 
                    {
                        if (hi.Higher != null && hi.Higher.IsRegion) 
                            citiInReg = true;
                        else if (rhi.EndChar <= rlo.BeginChar && rhi.Next.IsComma && !rlo.Morph.Case.IsGenitive) 
                            citiInReg = true;
                        else if (rhi.EndChar <= rlo.BeginChar && rhi.Next.IsComma) 
                            citiInReg = true;
                    }
                }
                else 
                    citiInReg = true;
            }
            if (rhi.EndChar <= rlo.BeginChar) 
            {
                if (!rhi.Morph.Class.IsAdjective) 
                {
                    if (hi.IsState && !rhi.Chars.IsLatinLetter) 
                        return false;
                }
                if (rhi.IsNewlineAfter || rlo.IsNewlineBefore) 
                {
                    if (!citiInReg) 
                        return false;
                }
            }
            else 
            {
            }
            if (rlo.Previous != null && rlo.Previous.Morph.Class.IsPreposition) 
            {
                if (rlo.Previous.Morph.Language.IsUa) 
                {
                    if ((rlo.Previous.IsValue("У", null) && !rlo.Morph.Case.IsDative && !rlo.Morph.Case.IsPrepositional) && !rlo.Morph.Case.IsUndefined) 
                        return false;
                    if (rlo.Previous.IsValue("З", null) && !rlo.Morph.Case.IsGenitive && !rlo.Morph.Case.IsUndefined) 
                        return false;
                }
                else 
                {
                    if ((rlo.Previous.IsValue("В", null) && !rlo.Morph.Case.IsDative && !rlo.Morph.Case.IsPrepositional) && !rlo.Morph.Case.IsUndefined) 
                        return false;
                    if (rlo.Previous.IsValue("ИЗ", null) && !rlo.Morph.Case.IsGenitive && !rlo.Morph.Case.IsUndefined) 
                        return false;
                }
            }
            if (!CanBeHigher(hi, lo)) 
                return citiInReg;
            return true;
        }
        public static bool CanBeHigher(Pullenti.Ner.Geo.GeoReferent hi, Pullenti.Ner.Geo.GeoReferent lo)
        {
            if (hi == null || lo == null || hi == lo) 
                return false;
            if (lo.Higher != null) 
                return lo.Higher == hi;
            if (lo.IsState) 
            {
                if (lo.IsRegion && hi.IsState && !hi.IsRegion) 
                    return true;
                return false;
            }
            if (hi.IsTerritory) 
                return false;
            if (lo.IsTerritory) 
                return true;
            string hit = _getTypesString(hi);
            string lot = _getTypesString(lo);
            if (hi.IsCity) 
            {
                if (lo.IsRegion) 
                {
                    if (hit.Contains("город;") || hit.Contains("місто") || hit.Contains("city")) 
                    {
                        if ((lot.Contains("район") || lot.Contains("административный округ") || lot.Contains("адміністративний округ")) || lot.Contains("муниципальн") || lot.Contains("муніципаль")) 
                            return true;
                        if (lo.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, "округ", true) != null && !lot.Contains("автономн")) 
                            return true;
                    }
                }
                if (lo.IsCity) 
                {
                    if (!hit.Contains("станция") && lot.Contains("станция")) 
                        return true;
                    if (!hit.Contains("станція") && lot.Contains("станція")) 
                        return true;
                    if (hit.Contains("город;") || hit.Contains("місто") || hit.Contains("city")) 
                    {
                        if ((lot.Contains("поселок") || lot.Contains("селище") || lot.Contains("село")) || lot.Contains("деревня") || lot.Contains("городок")) 
                            return true;
                    }
                    if (hit.Contains("поселение") || hit.Contains("поселок")) 
                    {
                        if (lot.Contains("село;") || lot.Contains("деревня") || lot.Contains("хутор")) 
                            return true;
                    }
                    if (hit.Contains("поселение") && lot.Contains("поселок")) 
                        return true;
                    if (hit.Contains("село;")) 
                    {
                        if (lot.Contains("поселение") || lot.Contains("поселок")) 
                            return true;
                    }
                    if (hi.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME, "МОСКВА", true) != null) 
                    {
                        if (lot.Contains("город;") || lot.Contains("місто") || lot.Contains("city")) 
                        {
                            if (lo.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME, "ЗЕЛЕНОГРАД", true) != null || lo.FindSlot(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME, "ТРОИЦК", true) != null) 
                                return true;
                        }
                    }
                }
            }
            else if (lo.IsCity) 
            {
                if (!lot.Contains("город") && !lot.Contains("місто") && !lot.Contains("city")) 
                {
                    if (hi.IsRegion) 
                        return true;
                }
                else 
                {
                    if (hi.IsState) 
                        return true;
                    if ((hit.Contains("административный округ") || hit.Contains("адміністративний округ") || hit.Contains("муниципальн")) || hit.Contains("муніципаль")) 
                        return false;
                    if (!hit.Contains("район")) 
                        return true;
                    if (hi.Higher != null && hi.Higher.IsRegion) 
                        return true;
                }
            }
            else if (lo.IsRegion) 
            {
                foreach (Pullenti.Ner.Slot s in hi.Slots) 
                {
                    if (s.TypeName == Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE) 
                    {
                        if (((string)s.Value) != "регион" && ((string)s.Value) != "регіон") 
                        {
                            if (lo.FindSlot(s.TypeName, s.Value, true) != null) 
                                return false;
                        }
                    }
                }
                if (hit.Contains("почтовое отделение")) 
                    return false;
                if (lot.Contains("почтовое отделение")) 
                    return true;
                if (hi.IsState) 
                    return true;
                if (lot.Contains("волость")) 
                    return true;
                if (lot.Contains("county") || lot.Contains("borough") || lot.Contains("parish")) 
                {
                    if (hit.Contains("state")) 
                        return true;
                }
                if (lot.Contains("район")) 
                {
                    if ((hit.Contains("область") || hit.Contains("регион") || hit.Contains("край")) || hit.Contains("регіон")) 
                        return true;
                    if (hit.Contains("округ") && !hit.Contains("сельский") && !hit.Contains("поселковый")) 
                        return true;
                }
                if (lot.Contains("область")) 
                {
                    if (hit.Contains("край")) 
                        return true;
                    if (hit.Contains("округ") && !hit.Contains("сельский") && !hit.Contains("поселковый")) 
                        return true;
                }
                if (lot.Contains("округ")) 
                {
                    if (lot.Contains("сельский") || lot.Contains("поселковый")) 
                        return true;
                    if (hit.Contains("край")) 
                        return true;
                    if (lot.Contains("округ")) 
                    {
                        if (hit.Contains("область") || hit.Contains("республика")) 
                            return true;
                    }
                }
                if (lot.Contains("муницип")) 
                {
                    if (hit.Contains("область") || hit.Contains("район") || hit.Contains("округ")) 
                        return true;
                }
            }
            return false;
        }
    }
}