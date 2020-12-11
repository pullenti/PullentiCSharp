/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Date.Internal
{
    class MetaDateRange : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaDateRange();
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateRangeReferent.ATTR_FROM, "Начало периода", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateRangeReferent.ATTR_TO, "Конец периода", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Date.DateRangeReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Период";
            }
        }
        public static string DateRangeImageId = "daterange";
        public static string DateRangeRelImageId = "daterangerel";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            if (obj is Pullenti.Ner.Date.DateRangeReferent) 
            {
                if ((obj as Pullenti.Ner.Date.DateRangeReferent).IsRelative) 
                    return DateRangeRelImageId;
            }
            return DateRangeImageId;
        }
        public static MetaDateRange GlobalMeta;
    }
}