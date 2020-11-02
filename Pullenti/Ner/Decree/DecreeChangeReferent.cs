/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Decree
{
    /// <summary>
    /// Модель изменения структурной части НПА
    /// </summary>
    public class DecreeChangeReferent : Pullenti.Ner.Referent
    {
        public DecreeChangeReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Decree.Internal.MetaDecreeChange.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("DECREECHANGE")
        /// </summary>
        public const string OBJ_TYPENAME = "DECREECHANGE";
        /// <summary>
        /// Имя атрибута - Структурный элемент, в который вносится изменение (м.б. несколько), 
        /// DecreeReferent или DecreePartReferent.
        /// </summary>
        public const string ATTR_OWNER = "OWNER";
        /// <summary>
        /// Имя атрибута - тип изменения (DecreeChangeKind)
        /// </summary>
        public const string ATTR_KIND = "KIND";
        /// <summary>
        /// Имя атрибута - внутренние изменения (DecreeChangeReferent)
        /// </summary>
        public const string ATTR_CHILD = "CHILD";
        /// <summary>
        /// Имя атрибута - само изменение (DecreeChangeValueReferent)
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - дополнительный параметр DecreeChangeValueReferent (для типа Exchange - что заменяется, для Append - после чего)
        /// </summary>
        public const string ATTR_PARAM = "PARAM";
        /// <summary>
        /// Имя атрибута - разное
        /// </summary>
        public const string ATTR_MISC = "MISC";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            if (Kind != DecreeChangeKind.Undefined) 
                res.AppendFormat("{0} ", Pullenti.Ner.Decree.Internal.MetaDecreeChange.KindFeature.ConvertInnerValueToOuterValue(Kind.ToString(), lang));
            if (IsOwnerNameAndText) 
                res.Append("наименование и текст ");
            else if (IsOwnerName) 
                res.Append("наименование ");
            else if (IsOnlyText) 
                res.Append("текст ");
            foreach (Pullenti.Ner.Referent o in Owners) 
            {
                res.AppendFormat("'{0}' ", o.ToString(true, lang, 0));
            }
            if (Value != null) 
                res.AppendFormat("{0} ", Value.ToString(true, lang, 0));
            if (Param != null) 
            {
                if (Kind == DecreeChangeKind.Append) 
                    res.Append("после ");
                else if (Kind == DecreeChangeKind.Exchange) 
                    res.Append("вместо ");
                res.Append(Param.ToString(true, lang, 0));
            }
            return res.ToString().Trim();
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return this.GetSlotValue(ATTR_OWNER) as Pullenti.Ner.Referent;
            }
        }
        /// <summary>
        /// Классификатор
        /// </summary>
        public DecreeChangeKind Kind
        {
            get
            {
                string s = this.GetStringValue(ATTR_KIND);
                if (s == null) 
                    return DecreeChangeKind.Undefined;
                try 
                {
                    if (s == "Add") 
                        return DecreeChangeKind.Append;
                    object res = Enum.Parse(typeof(DecreeChangeKind), s, true);
                    if (res is DecreeChangeKind) 
                        return (DecreeChangeKind)res;
                }
                catch(Exception ex1053) 
                {
                }
                return DecreeChangeKind.Undefined;
            }
            set
            {
                if (value != DecreeChangeKind.Undefined) 
                    this.AddSlot(ATTR_KIND, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Структурный элемент, в который вносится изменение (м.б. несколько)
        /// </summary>
        public List<Pullenti.Ner.Referent> Owners
        {
            get
            {
                List<Pullenti.Ner.Referent> res = new List<Pullenti.Ner.Referent>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_OWNER && (s.Value is Pullenti.Ner.Referent)) 
                        res.Add(s.Value as Pullenti.Ner.Referent);
                }
                return res;
            }
        }
        /// <summary>
        /// Внутренние изменения
        /// </summary>
        public List<DecreeChangeReferent> Children
        {
            get
            {
                List<DecreeChangeReferent> res = new List<DecreeChangeReferent>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_CHILD && (s.Value is DecreeChangeReferent)) 
                        res.Add(s.Value as DecreeChangeReferent);
                }
                return res;
            }
        }
        /// <summary>
        /// Значение
        /// </summary>
        public DecreeChangeValueReferent Value
        {
            get
            {
                return this.GetSlotValue(ATTR_VALUE) as DecreeChangeValueReferent;
            }
            set
            {
                this.AddSlot(ATTR_VALUE, value, true, 0);
            }
        }
        /// <summary>
        /// Дополнительный параметр (для типа Exchange - что заменяется, для Append - после чего)
        /// </summary>
        public DecreeChangeValueReferent Param
        {
            get
            {
                return this.GetSlotValue(ATTR_PARAM) as DecreeChangeValueReferent;
            }
            set
            {
                this.AddSlot(ATTR_PARAM, value, true, 0);
            }
        }
        /// <summary>
        /// Признак того, что изменения касаются наименования структурного элемента
        /// </summary>
        public bool IsOwnerName
        {
            get
            {
                return this.FindSlot(ATTR_MISC, "NAME", true) != null;
            }
            set
            {
                if (value) 
                    this.AddSlot(ATTR_MISC, "NAME", false, 0);
            }
        }
        /// <summary>
        /// Признак того, что изменения касаются только текста (без заголовка)
        /// </summary>
        public bool IsOnlyText
        {
            get
            {
                return this.FindSlot(ATTR_MISC, "TEXT", true) != null;
            }
            set
            {
                if (value) 
                    this.AddSlot(ATTR_MISC, "TEXT", false, 0);
            }
        }
        /// <summary>
        /// Признак того, что изменения касаются наименования и текста структурного элемента
        /// </summary>
        public bool IsOwnerNameAndText
        {
            get
            {
                return this.FindSlot(ATTR_MISC, "NAMETEXT", true) != null;
            }
            set
            {
                if (value) 
                    this.AddSlot(ATTR_MISC, "NAMETEXT", false, 0);
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            return obj == this;
        }
        internal bool CheckCorrect()
        {
            if (Kind == DecreeChangeKind.Undefined) 
                return false;
            if (Kind == DecreeChangeKind.Expire || Kind == DecreeChangeKind.Remove) 
                return true;
            if (Value == null) 
                return false;
            if (Kind == DecreeChangeKind.Exchange) 
            {
                if (Param == null) 
                {
                    if (Owners.Count > 0 && Owners[0].FindSlot(DecreePartReferent.ATTR_INDENTION, null, true) != null) 
                        Kind = DecreeChangeKind.New;
                    else 
                        return false;
                }
            }
            return true;
        }
    }
}