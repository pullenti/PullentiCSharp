/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Uri
{
    /// <summary>
    /// Сущность URI - всё, что укладывается в СХЕМА:ЗНАЧЕНИЕ (www, email, ISBN, УДК, ББК, ICQ и пр.)
    /// </summary>
    public class UriReferent : Pullenti.Ner.Referent
    {
        public UriReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Uri.Internal.MetaUri.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("URI")
        /// </summary>
        public const string OBJ_TYPENAME = "URI";
        /// <summary>
        /// Имя атрибута - значение (без схемы)
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - детализация
        /// </summary>
        public const string ATTR_DETAIL = "DETAIL";
        /// <summary>
        /// Имя атрибута - схема
        /// </summary>
        public const string ATTR_SCHEME = "SCHEME";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            if (Scheme != null) 
            {
                string split = ":";
                if (Scheme == "ISBN" || Scheme == "ББК" || Scheme == "УДК") 
                    split = " ";
                else if (Scheme == "http" || Scheme == "ftp" || Scheme == "https") 
                    split = "://";
                return string.Format("{0}{1}{2}", Scheme, split, Value ?? "?");
            }
            else 
                return Value;
        }
        /// <summary>
        /// Значение
        /// </summary>
        public string Value
        {
            get
            {
                return this.GetStringValue(ATTR_VALUE);
            }
            set
            {
                string val = value;
                this.AddSlot(ATTR_VALUE, val, true, 0);
            }
        }
        /// <summary>
        /// Схема
        /// </summary>
        public string Scheme
        {
            get
            {
                return this.GetStringValue(ATTR_SCHEME);
            }
            set
            {
                this.AddSlot(ATTR_SCHEME, value, true, 0);
            }
        }
        /// <summary>
        /// Детализация кода (если есть)
        /// </summary>
        public string Detail
        {
            get
            {
                return this.GetStringValue(ATTR_DETAIL);
            }
            set
            {
                this.AddSlot(ATTR_DETAIL, value, true, 0);
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            UriReferent uri = obj as UriReferent;
            if (uri == null) 
                return false;
            return string.Compare(Value, uri.Value, true) == 0;
        }
    }
}