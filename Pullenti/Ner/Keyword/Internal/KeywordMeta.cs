/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Keyword.Internal
{
    class KeywordMeta : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new KeywordMeta();
            GlobalMeta.AddFeature(Pullenti.Ner.Keyword.KeywordReferent.ATTR_TYPE, "Тип", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Keyword.KeywordReferent.ATTR_VALUE, "Значение", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Keyword.KeywordReferent.ATTR_NORMAL, "Нормализация", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Keyword.KeywordReferent.ATTR_REF, "Ссылка", 0, 0);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Keyword.KeywordReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Ключевое слово";
            }
        }
        public static string ImageObj = "kwobject";
        public static string ImagePred = "kwpredicate";
        public static string ImageRef = "kwreferent";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            Pullenti.Ner.Keyword.KeywordReferent m = obj as Pullenti.Ner.Keyword.KeywordReferent;
            if (m != null) 
            {
                if (m.Typ == Pullenti.Ner.Keyword.KeywordType.Predicate) 
                    return ImagePred;
                if (m.Typ == Pullenti.Ner.Keyword.KeywordType.Referent) 
                    return ImageRef;
            }
            return ImageObj;
        }
        public static KeywordMeta GlobalMeta;
    }
}