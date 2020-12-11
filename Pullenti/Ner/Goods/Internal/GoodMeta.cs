/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Goods.Internal
{
    internal class GoodMeta : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new GoodMeta();
            GlobalMeta.AddFeature(Pullenti.Ner.Goods.GoodReferent.ATTR_ATTR, "Атрибут", 1, 0).ShowAsParent = true;
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Goods.GoodReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Товар";
            }
        }
        public static string ImageId = "good";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        public static GoodMeta GlobalMeta;
    }
}