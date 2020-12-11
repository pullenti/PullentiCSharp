/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Measure.Internal
{
    class UnitMeta : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new UnitMeta();
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.UnitReferent.ATTR_NAME, "Краткое наименование", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.UnitReferent.ATTR_FULLNAME, "Полное наименование", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.UnitReferent.ATTR_POW, "Степень", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.UnitReferent.ATTR_BASEFACTOR, "Мультипликатор для базовой единицы", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.UnitReferent.ATTR_BASEUNIT, "Базовая единица", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.UnitReferent.ATTR_UNKNOWN, "Неизвестная метрика", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Measure.UnitReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Единицы измерения";
            }
        }
        public static string ImageId = "munit";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        public static UnitMeta GlobalMeta;
    }
}