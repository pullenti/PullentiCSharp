/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Transport
{
    /// <summary>
    /// Сущность - транспортное средство
    /// </summary>
    public class TransportReferent : Pullenti.Ner.Referent
    {
        public TransportReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Transport.Internal.MetaTransport.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("TRANSPORT")
        /// </summary>
        public const string OBJ_TYPENAME = "TRANSPORT";
        /// <summary>
        /// Имя атрибута - тип
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - марка (производитель, бренд)
        /// </summary>
        public const string ATTR_BRAND = "BRAND";
        /// <summary>
        /// Имя атрибута - модель
        /// </summary>
        public const string ATTR_MODEL = "MODEL";
        /// <summary>
        /// Имя атрибута - класс
        /// </summary>
        public const string ATTR_CLASS = "CLASS";
        /// <summary>
        /// Имя атрибута - собственное имя (если есть, например, у кораблей)
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - номер
        /// </summary>
        public const string ATTR_NUMBER = "NUMBER";
        /// <summary>
        /// Имя атрибута - номер региона
        /// </summary>
        public const string ATTR_NUMBER_REGION = "NUMBER_REG";
        /// <summary>
        /// Имя атрибута - категория (TransportKind)
        /// </summary>
        public const string ATTR_KIND = "KIND";
        /// <summary>
        /// Имя атрибута - географический объект (GeoReferent)
        /// </summary>
        public const string ATTR_GEO = "GEO";
        /// <summary>
        /// Имя атрибута - организация (OrganizationReferent)
        /// </summary>
        public const string ATTR_ORG = "ORG";
        /// <summary>
        /// Имя атрибута - дата выпуска
        /// </summary>
        public const string ATTR_DATE = "DATE";
        /// <summary>
        /// Имя атрибута - пункт назначения
        /// </summary>
        public const string ATTR_ROUTEPOINT = "ROUTEPOINT";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang, int lev = 0)
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
                res.Append(str);
            else if (Kind == TransportKind.Auto) 
                res.Append("автомобиль");
            else if (Kind == TransportKind.Fly) 
                res.Append("самолет");
            else if (Kind == TransportKind.Ship) 
                res.Append("судно");
            else if (Kind == TransportKind.Space) 
                res.Append("космический корабль");
            else 
                res.Append(Kind.ToString());
            if ((((str = this.GetStringValue(ATTR_BRAND)))) != null) 
                res.AppendFormat(" {0}", Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(str));
            if ((((str = this.GetStringValue(ATTR_MODEL)))) != null) 
                res.AppendFormat(" {0}", Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(str));
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
            if ((((str = this.GetStringValue(ATTR_CLASS)))) != null) 
                res.AppendFormat(" класса \"{0}\"", Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(str));
            if ((((str = this.GetStringValue(ATTR_NUMBER)))) != null) 
            {
                res.AppendFormat(", номер {0}", str);
                if ((((str = this.GetStringValue(ATTR_NUMBER_REGION)))) != null) 
                    res.Append(str);
            }
            if (this.FindSlot(ATTR_ROUTEPOINT, null, true) != null) 
            {
                res.AppendFormat(" (");
                bool fi = true;
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_ROUTEPOINT) 
                    {
                        if (fi) 
                            fi = false;
                        else 
                            res.Append(" - ");
                        if (s.Value is Pullenti.Ner.Referent) 
                            res.Append((s.Value as Pullenti.Ner.Referent).ToString(true, lang, 0));
                        else 
                            res.Append(s.Value);
                    }
                }
                res.Append(")");
            }
            if (!shortVariant) 
            {
                if ((((str = this.GetStringValue(ATTR_GEO)))) != null) 
                    res.AppendFormat("; {0}", str);
                if ((((str = this.GetStringValue(ATTR_ORG)))) != null) 
                    res.AppendFormat("; {0}", str);
            }
            return res.ToString();
        }
        /// <summary>
        /// Категория транспорта (авто, авиа, аква ...)
        /// </summary>
        public TransportKind Kind
        {
            get
            {
                return this._getKind(this.GetStringValue(ATTR_KIND));
            }
            set
            {
                if (value != TransportKind.Undefined) 
                    this.AddSlot(ATTR_KIND, value.ToString(), true, 0);
            }
        }
        TransportKind _getKind(string s)
        {
            if (s == null) 
                return TransportKind.Undefined;
            try 
            {
                object res = Enum.Parse(typeof(TransportKind), s, true);
                if (res is TransportKind) 
                    return (TransportKind)res;
            }
            catch(Exception ex4022) 
            {
            }
            return TransportKind.Undefined;
        }
        internal void AddGeo(object r)
        {
            if (r is Pullenti.Ner.Geo.GeoReferent) 
                this.AddSlot(ATTR_GEO, r, false, 0);
            else if (r is Pullenti.Ner.ReferentToken) 
            {
                if ((r as Pullenti.Ner.ReferentToken).GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                {
                    this.AddSlot(ATTR_GEO, (r as Pullenti.Ner.ReferentToken).GetReferent(), false, 0);
                    this.AddExtReferent(r as Pullenti.Ner.ReferentToken);
                }
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            TransportReferent tr = obj as TransportReferent;
            if (tr == null) 
                return false;
            TransportKind k1 = Kind;
            TransportKind k2 = tr.Kind;
            if (k1 != k2) 
            {
                if (k1 == TransportKind.Space && tr.FindSlot(ATTR_TYPE, "КОРАБЛЬ", true) != null) 
                {
                }
                else if (k2 == TransportKind.Space && this.FindSlot(ATTR_TYPE, "КОРАБЛЬ", true) != null) 
                    k1 = TransportKind.Space;
                else 
                    return false;
            }
            Pullenti.Ner.Slot sl = this.FindSlot(ATTR_ORG, null, true);
            if (sl != null && tr.FindSlot(ATTR_ORG, null, true) != null) 
            {
                if (tr.FindSlot(ATTR_ORG, sl.Value, false) == null) 
                    return false;
            }
            sl = this.FindSlot(ATTR_GEO, null, true);
            if (sl != null && tr.FindSlot(ATTR_GEO, null, true) != null) 
            {
                if (tr.FindSlot(ATTR_GEO, sl.Value, true) == null) 
                    return false;
            }
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
                    s1 = this.GetStringValue(ATTR_NUMBER_REGION);
                    s2 = tr.GetStringValue(ATTR_NUMBER_REGION);
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
                }
            }
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
                else if (s1 != s2) 
                    return false;
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
            List<TransportKind> kinds = new List<TransportKind>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_KIND) 
                {
                    TransportKind ki = this._getKind((string)s.Value);
                    if (!kinds.Contains(ki)) 
                        kinds.Add(ki);
                }
            }
            if (kinds.Count > 0) 
            {
                if (kinds.Contains(TransportKind.Space)) 
                {
                    for (int i = Slots.Count - 1; i >= 0; i--) 
                    {
                        if (Slots[i].TypeName == ATTR_KIND && this._getKind((string)Slots[i].Value) != TransportKind.Space) 
                            Slots.RemoveAt(i);
                    }
                }
            }
        }
        internal bool Check(bool onAttach, bool brandisdoubt)
        {
            TransportKind ki = Kind;
            if (ki == TransportKind.Undefined) 
                return false;
            if (this.FindSlot(ATTR_NUMBER, null, true) != null) 
            {
                if (this.FindSlot(ATTR_NUMBER_REGION, null, true) == null && (Slots.Count < 3)) 
                    return false;
                return true;
            }
            string model = this.GetStringValue(ATTR_MODEL);
            bool hasNum = false;
            if (model != null) 
            {
                foreach (char s in model) 
                {
                    if (!char.IsLetter(s)) 
                    {
                        hasNum = true;
                        break;
                    }
                }
            }
            if (ki == TransportKind.Auto) 
            {
                if (this.FindSlot(ATTR_BRAND, null, true) != null) 
                {
                    if (onAttach) 
                        return true;
                    if (!hasNum && this.FindSlot(ATTR_TYPE, null, true) == null) 
                        return false;
                    if (brandisdoubt && model == null && !hasNum) 
                        return false;
                    return true;
                }
                if (model != null && onAttach) 
                    return true;
                return false;
            }
            if (model != null) 
            {
                if (!hasNum && ki == TransportKind.Fly && this.FindSlot(ATTR_BRAND, null, true) == null) 
                    return false;
                return true;
            }
            if (this.FindSlot(ATTR_NAME, null, true) != null) 
            {
                string nam = this.GetStringValue(ATTR_NAME);
                if (ki == TransportKind.Fly && nam.StartsWith("Аэрофлот")) 
                    return false;
                return true;
            }
            if (ki == TransportKind.Train) 
            {
            }
            return false;
        }
    }
}