/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Transport.Internal
{
    class MetaTransport : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaTransport();
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_TYPE, "Тип", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_NAME, "Название", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_NUMBER, "Номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_NUMBER_REGION, "Регион номера", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_BRAND, "Марка", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_MODEL, "Модель", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_CLASS, "Класс", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_KIND, "Категория", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_GEO, "География", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_ORG, "Организация", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_DATE, "Дата создания", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Transport.TransportReferent.ATTR_ROUTEPOINT, "Пункт маршрута", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Transport.TransportReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Транспорт";
            }
        }
        public static string ImageId = "tansport";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            if (obj is Pullenti.Ner.Transport.TransportReferent) 
            {
                Pullenti.Ner.Transport.TransportKind ok = (obj as Pullenti.Ner.Transport.TransportReferent).Kind;
                if (ok != Pullenti.Ner.Transport.TransportKind.Undefined) 
                    return ok.ToString();
            }
            return ImageId;
        }
        internal static MetaTransport GlobalMeta;
    }
}