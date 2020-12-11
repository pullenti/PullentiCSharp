/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Measure
{
    /// <summary>
    /// Единица измерения вместе с множителем
    /// </summary>
    public class UnitReferent : Pullenti.Ner.Referent
    {
        public UnitReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Measure.Internal.UnitMeta.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("MEASUREUNIT")
        /// </summary>
        public const string OBJ_TYPENAME = "MEASUREUNIT";
        /// <summary>
        /// Имя атрибута - полное имя единицы (например, километр)
        /// </summary>
        public const string ATTR_FULLNAME = "FULLNAME";
        /// <summary>
        /// Имя атрибута - краткое имя единицы (например, км)
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - степень, в которую нужно возвести
        /// </summary>
        public const string ATTR_POW = "POW";
        /// <summary>
        /// Имя атрибута - множитель для базовой единицы (чтобы приводить к единому знаменателю)
        /// </summary>
        public const string ATTR_BASEFACTOR = "BASEFACTOR";
        /// <summary>
        /// Имя атрибута - базовая единица
        /// </summary>
        public const string ATTR_BASEUNIT = "BASEUNIT";
        /// <summary>
        /// Имя атрибута - признак неизвестной (движку) метрики
        /// </summary>
        public const string ATTR_UNKNOWN = "UNKNOWN";
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return this.GetSlotValue(ATTR_BASEUNIT) as Pullenti.Ner.Referent;
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            string nam = null;
            for (int l = 0; l < 2; l++) 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (((s.TypeName == ATTR_NAME && shortVariant)) || ((s.TypeName == ATTR_FULLNAME && !shortVariant))) 
                    {
                        string val = s.Value as string;
                        if (lang != null && l == 0) 
                        {
                            if (lang.IsRu != Pullenti.Morph.LanguageHelper.IsCyrillic(val)) 
                                continue;
                        }
                        nam = val;
                        break;
                    }
                }
                if (nam != null) 
                    break;
            }
            if (nam == null) 
                nam = this.GetStringValue(ATTR_NAME);
            string pow = this.GetStringValue(ATTR_POW);
            if (string.IsNullOrEmpty(pow) || lev > 0) 
                return nam ?? "?";
            string res = (pow[0] != '-' ? string.Format("{0}{1}", nam, pow) : string.Format("{0}<{1}>", nam, pow));
            if (!shortVariant && IsUnknown) 
                res = "(?)" + res;
            return res;
        }
        /// <summary>
        /// Признак того, что это неизвестная метрика
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return this.GetStringValue(ATTR_UNKNOWN) == "true";
            }
            set
            {
                this.AddSlot(ATTR_UNKNOWN, (value ? "true" : null), true, 0);
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            UnitReferent ur = obj as UnitReferent;
            if (ur == null) 
                return false;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (ur.FindSlot(s.TypeName, s.Value, true) == null) 
                    return false;
            }
            foreach (Pullenti.Ner.Slot s in ur.Slots) 
            {
                if (this.FindSlot(s.TypeName, s.Value, true) == null) 
                    return false;
            }
            return true;
        }
        // Используется внутренним образом
        internal Pullenti.Ner.Measure.Internal.Unit m_Unit;
    }
}