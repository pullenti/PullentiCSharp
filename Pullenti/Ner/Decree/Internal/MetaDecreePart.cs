/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Decree.Internal
{
    internal class MetaDecreePart : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaDecreePart();
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_NAME, "Наименование", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_OWNER, "Владелец", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_LOCALTYP, "Локальный тип", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SECTION, "Раздел", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBSECTION, "Подраздел", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_APPENDIX, "Приложение", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_CHAPTER, "Глава", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_PREAMBLE, "Преамбула", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_CLAUSE, "Статья", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_PART, "Часть", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_DOCPART, "Часть документа", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_PARAGRAPH, "Параграф", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBPARAGRAPH, "Подпараграф", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_ITEM, "Пункт", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBITEM, "Подпункт", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_INDENTION, "Абзац", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBINDENTION, "Подабзац", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBPROGRAM, "Подпрограмма", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_ADDAGREE, "Допсоглашение", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_NOTICE, "Примечание", 0, 1);
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
                return "Ссылка на часть НПА";
            }
        }
        public static string PartImageId = "part";
        public static string PartLocImageId = "partloc";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            Pullenti.Ner.Decree.DecreePartReferent dpr = obj as Pullenti.Ner.Decree.DecreePartReferent;
            if (dpr != null) 
            {
                if (dpr.Owner == null) 
                    return PartLocImageId;
            }
            return PartImageId;
        }
        public static MetaDecreePart GlobalMeta;
    }
}