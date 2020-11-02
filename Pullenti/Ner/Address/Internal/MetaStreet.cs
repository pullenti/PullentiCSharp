/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Address.Internal
{
    class MetaStreet : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaStreet();
            GlobalMeta.AddFeature(Pullenti.Ner.Address.StreetReferent.ATTR_TYP, "Тип", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.StreetReferent.ATTR_NAME, "Наименование", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.StreetReferent.ATTR_NUMBER, "Номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.StreetReferent.ATTR_SECNUMBER, "Доп.номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.StreetReferent.ATTR_GEO, "Географический объект", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.StreetReferent.ATTR_FIAS, "Объект ФИАС", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.StreetReferent.ATTR_BTI, "Объект БТИ", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.StreetReferent.ATTR_OKM, "Код ОКМ УМ", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Address.StreetReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Улица";
            }
        }
        public static string ImageId = "street";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        internal static MetaStreet GlobalMeta;
    }
}