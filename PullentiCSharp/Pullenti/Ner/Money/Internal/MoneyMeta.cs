/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Money.Internal
{
    internal class MoneyMeta : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MoneyMeta();
            GlobalMeta.AddFeature(Pullenti.Ner.Money.MoneyReferent.ATTR_CURRENCY, "Валюта", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Money.MoneyReferent.ATTR_VALUE, "Значение", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Money.MoneyReferent.ATTR_REST, "Остаток (100)", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Money.MoneyReferent.ATTR_ALTVALUE, "Другое значение", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Money.MoneyReferent.ATTR_ALTREST, "Другой остаток (100)", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Money.MoneyReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Денежная сумма";
            }
        }
        public static string ImageId = "sum";
        public static string Image2Id = "sumerr";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            Pullenti.Ner.Money.MoneyReferent m = obj as Pullenti.Ner.Money.MoneyReferent;
            if (m != null) 
            {
                if (m.AltValue != null || m.AltRest != null) 
                    return Image2Id;
            }
            return ImageId;
        }
        public static MoneyMeta GlobalMeta;
    }
}