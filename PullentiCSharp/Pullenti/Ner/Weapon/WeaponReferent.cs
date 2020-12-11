/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Weapon
{
    /// <summary>
    /// Сущность - оружие
    /// </summary>
    public class WeaponReferent : Pullenti.Ner.Referent
    {
        public WeaponReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Weapon.Internal.MetaWeapon.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("WEAPON")
        /// </summary>
        public const string OBJ_TYPENAME = "WEAPON";
        /// <summary>
        /// Имя атрибута - тип
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - производитель (бренд)
        /// </summary>
        public const string ATTR_BRAND = "BRAND";
        /// <summary>
        /// Имя атрибута - модель
        /// </summary>
        public const string ATTR_MODEL = "MODEL";
        /// <summary>
        /// Имя атрибута - собственное имя (если есть)
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - регистрационный номер
        /// </summary>
        public const string ATTR_NUMBER = "NUMBER";
        /// <summary>
        /// Имя атрибута - дата выпуска
        /// </summary>
        public const string ATTR_DATE = "DATE";
        /// <summary>
        /// Имя атрибута - ссылка на другую сущность
        /// </summary>
        public const string ATTR_REF = "REF";
        /// <summary>
        /// Имя атрибута - калибр
        /// </summary>
        public const string ATTR_CALIBER = "CALIBER";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            string str = null;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_TYPE) 
                {
                    string n = (string)s.Value;
                    if (str == null || (n.Length < str.Length)) 
                        str = n;
                }
            }
            if (str != null) 
                res.Append(str.ToLower());
            if ((((str = this.GetStringValue(ATTR_BRAND)))) != null) 
                res.AppendFormat(" {0}", Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(str));
            if ((((str = this.GetStringValue(ATTR_MODEL)))) != null) 
                res.AppendFormat(" {0}", str);
            if ((((str = this.GetStringValue(ATTR_NAME)))) != null) 
            {
                res.AppendFormat(" \"{0}\"", Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(str));
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NAME && str != ((string)s.Value)) 
                    {
                        if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(str[0]) != Pullenti.Morph.LanguageHelper.IsCyrillicChar(((string)s.Value)[0])) 
                        {
                            res.AppendFormat(" ({0})", Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower((string)s.Value));
                            break;
                        }
                    }
                }
            }
            if ((((str = this.GetStringValue(ATTR_NUMBER)))) != null) 
                res.AppendFormat(", номер {0}", str);
            return res.ToString();
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            WeaponReferent tr = obj as WeaponReferent;
            if (tr == null) 
                return false;
            string s1 = this.GetStringValue(ATTR_NUMBER);
            string s2 = tr.GetStringValue(ATTR_NUMBER);
            if (s1 != null || s2 != null) 
            {
                if (s1 == null || s2 == null) 
                {
                    if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                        return false;
                }
                else 
                {
                    if (s1 != s2) 
                        return false;
                    return true;
                }
            }
            bool eqTypes = false;
            foreach (string t in this.GetStringValues(ATTR_TYPE)) 
            {
                if (tr.FindSlot(ATTR_TYPE, t, true) != null) 
                {
                    eqTypes = true;
                    break;
                }
            }
            if (!eqTypes) 
                return false;
            s1 = this.GetStringValue(ATTR_BRAND);
            s2 = tr.GetStringValue(ATTR_BRAND);
            if (s1 != null || s2 != null) 
            {
                if (s1 == null || s2 == null) 
                {
                    if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                        return false;
                }
                else if (s1 != s2) 
                    return false;
            }
            s1 = this.GetStringValue(ATTR_MODEL);
            s2 = tr.GetStringValue(ATTR_MODEL);
            if (s1 != null || s2 != null) 
            {
                if (s1 == null || s2 == null) 
                {
                    if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                        return false;
                }
                else 
                {
                    if (this.FindSlot(ATTR_MODEL, s2, true) != null) 
                        return true;
                    if (tr.FindSlot(ATTR_MODEL, s1, true) != null) 
                        return true;
                    return false;
                }
            }
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NAME) 
                {
                    if (tr.FindSlot(ATTR_NAME, s.Value, true) != null) 
                        return true;
                }
            }
            if (s1 != null && s2 != null) 
                return true;
            return false;
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            base.MergeSlots(obj, mergeStatistic);
        }
    }
}