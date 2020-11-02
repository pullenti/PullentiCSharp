/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Org.Internal
{
    class MetaOrganization : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaOrganization();
            GlobalMeta.AddFeature(Pullenti.Ner.Org.OrganizationReferent.ATTR_NAME, "Название", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Org.OrganizationReferent.ATTR_TYPE, "Тип", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Org.OrganizationReferent.ATTR_EPONYM, "Эпоним (имени)", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Org.OrganizationReferent.ATTR_NUMBER, "Номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Org.OrganizationReferent.ATTR_HIGHER, "Вышестоящая организация", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Org.OrganizationReferent.ATTR_OWNER, "Объект-владелец", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Org.OrganizationReferent.ATTR_GEO, "Географический объект", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Referent.ATTR_GENERAL, "Обобщающая организация", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Org.OrganizationReferent.ATTR_MISC, "Разное", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Org.OrganizationReferent.ATTR_PROFILE, "Профиль", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Org.OrganizationReferent.ATTR_MARKER, "Маркер", 0, 0);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Org.OrganizationReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Организация";
            }
        }
        public static string OrgImageId = "org";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            if (obj is Pullenti.Ner.Org.OrganizationReferent) 
            {
                List<Pullenti.Ner.Org.OrgProfile> prs = (obj as Pullenti.Ner.Org.OrganizationReferent).Profiles;
                if (prs != null && prs.Count > 0) 
                {
                    Pullenti.Ner.Org.OrgProfile pr = prs[prs.Count - 1];
                    return pr.ToString();
                }
            }
            return OrgImageId;
        }
        internal static MetaOrganization GlobalMeta;
    }
}