/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Person.Internal
{
    class MetaPersonProperty : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaPersonProperty();
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_NAME, "Наименование", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_HIGHER, "Вышестоящее свойство", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_ATTR, "Атрибут", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, "Ссылка на объект", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Referent.ATTR_GENERAL, "Обобщающее свойство", 1, 0);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Person.PersonPropertyReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Свойство персоны";
            }
        }
        public static string PersonPropImageId = "personprop";
        public static string PersonPropKingImageId = "king";
        public static string PersonPropBossImageId = "boss";
        public static string PersonPropKinImageId = "kin";
        public static string PersonPropMilitaryId = "militaryrank";
        public static string PersonPropNationId = "nationality";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            Pullenti.Ner.Person.PersonPropertyKind ki = Pullenti.Ner.Person.PersonPropertyKind.Undefined;
            if (obj is Pullenti.Ner.Person.PersonPropertyReferent) 
                ki = (obj as Pullenti.Ner.Person.PersonPropertyReferent).Kind;
            if (ki == Pullenti.Ner.Person.PersonPropertyKind.Boss) 
                return PersonPropBossImageId;
            if (ki == Pullenti.Ner.Person.PersonPropertyKind.King) 
                return PersonPropKingImageId;
            if (ki == Pullenti.Ner.Person.PersonPropertyKind.Kin) 
                return PersonPropKinImageId;
            if (ki == Pullenti.Ner.Person.PersonPropertyKind.MilitaryRank) 
                return PersonPropMilitaryId;
            if (ki == Pullenti.Ner.Person.PersonPropertyKind.Nationality) 
                return PersonPropNationId;
            return PersonPropImageId;
        }
        internal static MetaPersonProperty GlobalMeta;
    }
}