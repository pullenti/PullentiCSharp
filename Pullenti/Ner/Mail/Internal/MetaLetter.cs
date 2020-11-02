/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Mail.Internal
{
    class MetaLetter : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaLetter();
            GlobalMeta.AddFeature(Pullenti.Ner.Mail.MailReferent.ATTR_KIND, "Тип блока", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Mail.MailReferent.ATTR_TEXT, "Текст блока", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Mail.MailReferent.ATTR_REF, "Ссылка на объект", 0, 0);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Mail.MailReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Блок письма";
            }
        }
        public static string ImageId = "letter";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        internal static MetaLetter GlobalMeta;
    }
}