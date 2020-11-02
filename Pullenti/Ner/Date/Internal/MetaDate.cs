/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Date.Internal
{
    internal class MetaDate : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaDate();
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_ISRELATIVE, "Относительность", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_CENTURY, "Век", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_YEAR, "Год", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_QUARTAL, "Квартал", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_MONTH, "Месяц", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_WEEK, "Неделя", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_DAY, "День", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_HOUR, "Час", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_MINUTE, "Минут", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_SECOND, "Секунд", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_DAYOFWEEK, "День недели", 0, 1);
            Pointer = GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_POINTER, "Указатель", 0, 1);
            Pointer.AddValue(Pullenti.Ner.Date.DatePointerType.Begin.ToString(), "в начале", "на початку", "in the beginning");
            Pointer.AddValue(Pullenti.Ner.Date.DatePointerType.Center.ToString(), "в середине", "в середині", "in the middle");
            Pointer.AddValue(Pullenti.Ner.Date.DatePointerType.End.ToString(), "в конце", "в кінці", "in the end");
            Pointer.AddValue(Pullenti.Ner.Date.DatePointerType.Today.ToString(), "настоящее время", "теперішній час", "today");
            Pointer.AddValue(Pullenti.Ner.Date.DatePointerType.Winter.ToString(), "зимой", "взимку", "winter");
            Pointer.AddValue(Pullenti.Ner.Date.DatePointerType.Spring.ToString(), "весной", "навесні", "spring");
            Pointer.AddValue(Pullenti.Ner.Date.DatePointerType.Summer.ToString(), "летом", "влітку", "summer");
            Pointer.AddValue(Pullenti.Ner.Date.DatePointerType.Autumn.ToString(), "осенью", "восени", "autumn");
            Pointer.AddValue(Pullenti.Ner.Date.DatePointerType.About.ToString(), "около", "біля", "about");
            Pointer.AddValue(Pullenti.Ner.Date.DatePointerType.Undefined.ToString(), "Не определена", null, null);
            GlobalMeta.AddFeature(Pullenti.Ner.Date.DateReferent.ATTR_HIGHER, "Вышестоящая дата", 0, 1);
        }
        public static Pullenti.Ner.Metadata.Feature Pointer;
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Date.DateReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Дата";
            }
        }
        public static string DateFullImageId = "datefull";
        public static string DateRelImageId = "daterel";
        public static string DateImageId = "date";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            Pullenti.Ner.Date.DateReferent dat = obj as Pullenti.Ner.Date.DateReferent;
            if (dat != null && dat.IsRelative) 
                return DateRelImageId;
            if (dat != null && dat.Hour >= 0) 
                return DateImageId;
            else 
                return DateFullImageId;
        }
        public static MetaDate GlobalMeta;
    }
}