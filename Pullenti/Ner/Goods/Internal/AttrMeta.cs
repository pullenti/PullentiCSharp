/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Goods.Internal
{
    internal class AttrMeta : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new AttrMeta();
            GlobalMeta.TypAttr = GlobalMeta.AddFeature(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_TYPE, "Тип", 0, 1);
            GlobalMeta.TypAttr.AddValue(Pullenti.Ner.Goods.GoodAttrType.Keyword.ToString(), "Ключевое слово", null, null);
            GlobalMeta.TypAttr.AddValue(Pullenti.Ner.Goods.GoodAttrType.Character.ToString(), "Качеств.свойство", null, null);
            GlobalMeta.TypAttr.AddValue(Pullenti.Ner.Goods.GoodAttrType.Model.ToString(), "Модель", null, null);
            GlobalMeta.TypAttr.AddValue(Pullenti.Ner.Goods.GoodAttrType.Numeric.ToString(), "Колич.свойство", null, null);
            GlobalMeta.TypAttr.AddValue(Pullenti.Ner.Goods.GoodAttrType.Proper.ToString(), "Имя собственное", null, null);
            GlobalMeta.TypAttr.AddValue(Pullenti.Ner.Goods.GoodAttrType.Referent.ToString(), "Ссылка", null, null);
            GlobalMeta.AddFeature(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_VALUE, "Значение", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_ALTVALUE, "Значание (альт.)", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_UNIT, "Единица измерения", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_NAME, "Название", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Goods.GoodAttributeReferent.ATTR_REF, "Ссылка", 0, 1);
        }
        public Pullenti.Ner.Metadata.Feature TypAttr;
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Goods.GoodAttributeReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Атрибут товара";
            }
        }
        public static string AttrImageId = "attr";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return AttrImageId;
        }
        public static AttrMeta GlobalMeta;
    }
}