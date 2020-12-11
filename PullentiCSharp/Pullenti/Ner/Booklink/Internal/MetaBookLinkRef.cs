/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Booklink.Internal
{
    class MetaBookLinkRef : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaBookLinkRef();
            GlobalMeta.AddFeature(Pullenti.Ner.Booklink.BookLinkRefReferent.ATTR_BOOK, "Источник", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Booklink.BookLinkRefReferent.ATTR_TYPE, "Тип", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Booklink.BookLinkRefReferent.ATTR_PAGES, "Страницы", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Booklink.BookLinkRefReferent.ATTR_NUMBER, "Номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Booklink.BookLinkRefReferent.ATTR_MISC, "Разное", 0, 0);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Booklink.BookLinkRefReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Ссылка на внешний источник в тексте";
            }
        }
        public static string ImageId = "booklinkref";
        public static string ImageIdInline = "booklinkrefinline";
        public static string ImageIdLast = "booklinkreflast";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            Pullenti.Ner.Booklink.BookLinkRefReferent rr = obj as Pullenti.Ner.Booklink.BookLinkRefReferent;
            if (rr != null) 
            {
                if (rr.Typ == Pullenti.Ner.Booklink.BookLinkRefType.Inline) 
                    return ImageIdInline;
            }
            return ImageId;
        }
        internal static MetaBookLinkRef GlobalMeta;
    }
}