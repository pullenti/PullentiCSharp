/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Weapon.Internal
{
    class MetaWeapon : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaWeapon();
            GlobalMeta.AddFeature(Pullenti.Ner.Weapon.WeaponReferent.ATTR_TYPE, "Тип", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Weapon.WeaponReferent.ATTR_NAME, "Название", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Weapon.WeaponReferent.ATTR_NUMBER, "Номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Weapon.WeaponReferent.ATTR_BRAND, "Марка", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Weapon.WeaponReferent.ATTR_MODEL, "Модель", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Weapon.WeaponReferent.ATTR_DATE, "Дата создания", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Weapon.WeaponReferent.ATTR_CALIBER, "Калибр", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Weapon.WeaponReferent.ATTR_REF, "Ссылка", 0, 0);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Weapon.WeaponReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Оружие";
            }
        }
        public static string ImageId = "weapon";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        internal static MetaWeapon GlobalMeta;
    }
}