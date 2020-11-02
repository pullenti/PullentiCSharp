/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Decree.Internal
{
    internal class MetaDecree : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaDecree();
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeReferent.ATTR_TYPE, "Тип", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeReferent.ATTR_NUMBER, "Номер", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeReferent.ATTR_CASENUMBER, "Номер дела", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeReferent.ATTR_DATE, "Дата", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeReferent.ATTR_SOURCE, "Источник", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeReferent.ATTR_GEO, "Географический объект", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeReferent.ATTR_NAME, "Наименование", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeReferent.ATTR_READING, "Чтение", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeReferent.ATTR_EDITION, "В редакции", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Referent.ATTR_GENERAL, "Обобщающий объект", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Decree.DecreeReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Декрет";
            }
        }
        public static string DecreeImageId = "decree";
        public static string PublishImageId = "publish";
        public static string StandadrImageId = "decreestd";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            if (obj is Pullenti.Ner.Decree.DecreeReferent) 
            {
                Pullenti.Ner.Decree.DecreeKind ki = (obj as Pullenti.Ner.Decree.DecreeReferent).Kind;
                if (ki == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                    return PublishImageId;
                if (ki == Pullenti.Ner.Decree.DecreeKind.Standard) 
                    return StandadrImageId;
            }
            return DecreeImageId;
        }
        public static MetaDecree GlobalMeta;
    }
}