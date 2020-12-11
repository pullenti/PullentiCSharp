/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Phone.Internal
{
    class MetaPhone : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaPhone();
            GlobalMeta.AddFeature(Pullenti.Ner.Phone.PhoneReferent.ATTR_NUNBER, "Номер", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Phone.PhoneReferent.ATTR_ADDNUMBER, "Добавочный номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Phone.PhoneReferent.ATTR_COUNTRYCODE, "Код страны", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Referent.ATTR_GENERAL, "Обобщающий номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Phone.PhoneReferent.ATTR_KIND, "Тип", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Phone.PhoneReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Телефонный номер";
            }
        }
        public static string PhoneImageId = "phone";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return PhoneImageId;
        }
        internal static MetaPhone GlobalMeta;
    }
}