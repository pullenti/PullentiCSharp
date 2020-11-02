/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Uri.Internal
{
    class MetaUri : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaUri();
            GlobalMeta.AddFeature(Pullenti.Ner.Uri.UriReferent.ATTR_VALUE, "Значение", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Uri.UriReferent.ATTR_SCHEME, "Схема", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Uri.UriReferent.ATTR_DETAIL, "Детализация", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Uri.UriReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "URI";
            }
        }
        public static string MailImageId = "mail";
        public static string UriImageId = "uri";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            Pullenti.Ner.Uri.UriReferent web = obj as Pullenti.Ner.Uri.UriReferent;
            if (web != null && web.Scheme == "mailto") 
                return MailImageId;
            else 
                return UriImageId;
        }
        internal static MetaUri GlobalMeta;
    }
}