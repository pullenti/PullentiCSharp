/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Named.Internal
{
    internal class MetaNamedEntity : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaNamedEntity();
            GlobalMeta.AddFeature(Pullenti.Ner.Named.NamedEntityReferent.ATTR_KIND, "Класс", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Named.NamedEntityReferent.ATTR_TYPE, "Тип", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Named.NamedEntityReferent.ATTR_NAME, "Наименование", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Named.NamedEntityReferent.ATTR_REF, "Ссылка", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Named.NamedEntityReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Именованная сущность";
            }
        }
        public static string ImageId = "monument";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            if (obj is Pullenti.Ner.Named.NamedEntityReferent) 
                return (obj as Pullenti.Ner.Named.NamedEntityReferent).Kind.ToString();
            return ImageId;
        }
        public static MetaNamedEntity GlobalMeta;
    }
}