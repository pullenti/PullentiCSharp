/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Person.Internal
{
    class MetaPersonIdentity : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaPersonIdentity();
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_TYPE, "Тип", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_NUMBER, "Номер", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_DATE, "Дата выдачи", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_ORG, "Кто выдал", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonIdentityReferent.ATTR_ADDRESS, "Адрес регистрации", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Person.PersonIdentityReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Удостоверение личности";
            }
        }
        public static string ImageId = "identity";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        internal static MetaPersonIdentity GlobalMeta;
    }
}