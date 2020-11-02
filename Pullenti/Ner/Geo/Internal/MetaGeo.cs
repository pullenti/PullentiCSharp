/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Geo.Internal
{
    internal class MetaGeo : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaGeo();
            GlobalMeta.AddFeature(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME, "Наименование", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Geo.GeoReferent.ATTR_TYPE, "Тип", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Geo.GeoReferent.ATTR_ALPHA2, "Код страны", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Geo.GeoReferent.ATTR_HIGHER, "Вышестоящий объект", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Geo.GeoReferent.ATTR_REF, "Ссылка на объект", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Geo.GeoReferent.ATTR_FIAS, "Объект ФИАС", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Geo.GeoReferent.ATTR_BTI, "Код БТИ", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Geo.GeoReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Территориальное образование";
            }
        }
        public static string CountryCityImageId = "countrycity";
        public static string CountryImageId = "country";
        public static string CityImageId = "city";
        public static string DistrictImageId = "district";
        public static string RegionImageId = "region";
        public static string TerrImageId = "territory";
        public static string UnionImageId = "union";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            Pullenti.Ner.Geo.GeoReferent ter = obj as Pullenti.Ner.Geo.GeoReferent;
            if (ter != null) 
            {
                if (ter.IsUnion) 
                    return UnionImageId;
                if (ter.IsCity && ((ter.IsState || ter.IsRegion))) 
                    return CountryCityImageId;
                if (ter.IsState) 
                    return CountryImageId;
                if (ter.IsCity) 
                    return CityImageId;
                if (ter.IsRegion && ter.Higher != null && ter.Higher.IsCity) 
                    return DistrictImageId;
                if (ter.IsTerritory) 
                    return TerrImageId;
            }
            return RegionImageId;
        }
        internal static MetaGeo GlobalMeta;
    }
}