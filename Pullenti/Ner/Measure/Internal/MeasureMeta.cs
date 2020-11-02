/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Measure.Internal
{
    class MeasureMeta : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MeasureMeta();
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.MeasureReferent.ATTR_TEMPLATE, "Шаблон", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.MeasureReferent.ATTR_VALUE, "Значение", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.MeasureReferent.ATTR_UNIT, "Единица измерения", 1, 2);
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.MeasureReferent.ATTR_REF, "Ссылка на уточняющее измерение", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.MeasureReferent.ATTR_NAME, "Наименование", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Measure.MeasureReferent.ATTR_KIND, "Тип", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Measure.MeasureReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Измеряемые величины";
            }
        }
        public static string ImageId = "measure";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        public static MeasureMeta GlobalMeta;
    }
}