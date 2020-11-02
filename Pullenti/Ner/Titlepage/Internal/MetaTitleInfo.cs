/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Titlepage.Internal
{
    class MetaTitleInfo : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaTitleInfo();
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_NAME, "Название", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_TYPE, "Тип", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_AUTHOR, "Автор", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_SUPERVISOR, "Руководитель", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_EDITOR, "Редактор", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_CONSULTANT, "Консультант", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_OPPONENT, "Оппонент", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_AFFIRMANT, "Утверждающий", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_TRANSLATOR, "Переводчик", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_ORG, "Организация", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_STUDENTYEAR, "Номер курса", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_DATE, "Дата", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_CITY, "Город", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_SPECIALITY, "Специальность", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_ATTR, "Атрибут", 0, 0);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Titlepage.TitlePageReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Заголовок";
            }
        }
        public static string TitleInfoImageId = "titleinfo";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return TitleInfoImageId;
        }
        internal static MetaTitleInfo GlobalMeta;
    }
}