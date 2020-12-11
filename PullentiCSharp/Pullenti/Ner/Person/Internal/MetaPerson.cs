/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Person.Internal
{
    class MetaPerson : Pullenti.Ner.Metadata.ReferentClass
    {
        public const string ATTR_SEXMALE = "MALE";
        public const string ATTR_SEXFEMALE = "FEMALE";
        public static void Initialize()
        {
            GlobalMeta = new MetaPerson();
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_IDENTITY, "Идентификация", 0, 0);
            Pullenti.Ner.Metadata.Feature sex = GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_SEX, "Пол", 0, 0);
            sex.AddValue(ATTR_SEXMALE, "мужской", null, null);
            sex.AddValue(ATTR_SEXFEMALE, "женский", null, null);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, "Фамилия", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, "Имя", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME, "Отчество", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, "Псевдоним", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_ATTR, "Свойство", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_AGE, "Возраст", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_BORN, "Родился", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_DIE, "Умер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_CONTACT, "Контактные данные", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Person.PersonReferent.ATTR_IDDOC, "Удостоверение личности", 0, 0).ShowAsParent = true;
            GlobalMeta.AddFeature(Pullenti.Ner.Referent.ATTR_GENERAL, "Обобщающая персона", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Person.PersonReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Персона";
            }
        }
        public static string ManImageId = "man";
        public static string WomenImageId = "women";
        public static string PersonImageId = "person";
        public static string GeneralImageId = "general";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            Pullenti.Ner.Person.PersonReferent pers = obj as Pullenti.Ner.Person.PersonReferent;
            if (pers != null) 
            {
                if (pers.FindSlot("@GENERAL", null, true) != null) 
                    return GeneralImageId;
                if (pers.IsMale) 
                    return ManImageId;
                if (pers.IsFemale) 
                    return WomenImageId;
            }
            return PersonImageId;
        }
        internal static MetaPerson GlobalMeta;
    }
}