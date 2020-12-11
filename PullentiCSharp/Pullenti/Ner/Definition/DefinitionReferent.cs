/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Definition
{
    /// <summary>
    /// Сущность, моделирующая тезис (утверждение, определения)
    /// </summary>
    public class DefinitionReferent : Pullenti.Ner.Referent
    {
        public DefinitionReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Definition.Internal.MetaDefin.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("THESIS")
        /// </summary>
        public const string OBJ_TYPENAME = "THESIS";
        /// <summary>
        /// Имя атрибута - определяемый термин
        /// </summary>
        public const string ATTR_TERMIN = "TERMIN";
        /// <summary>
        /// Имя атрибута - дополнительный атрибут термина
        /// </summary>
        public const string ATTR_TERMIN_ADD = "TERMINADD";
        /// <summary>
        /// Имя атрибута - основной текст
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - разное
        /// </summary>
        public const string ATTR_MISC = "MISC";
        /// <summary>
        /// Имя атрибута - тип тезиса (DefinitionKind)
        /// </summary>
        public const string ATTR_KIND = "KIND";
        /// <summary>
        /// Имя атрибута - ссылка на НПА (DecreeReferent или DecreePartReferent)
        /// </summary>
        public const string ATTR_DECREE = "DECREE";
        /// <summary>
        /// Определяемый термин (условно левая часть)
        /// </summary>
        public string Termin
        {
            get
            {
                return this.GetStringValue(ATTR_TERMIN);
            }
        }
        /// <summary>
        /// Дополнительный атрибут термина ("как наука", "в широком смысле" ...)
        /// </summary>
        public string TerminAdd
        {
            get
            {
                return this.GetStringValue(ATTR_TERMIN_ADD);
            }
        }
        /// <summary>
        /// Собственно текст (условно правая часть)
        /// </summary>
        public string Value
        {
            get
            {
                return this.GetStringValue(ATTR_VALUE);
            }
        }
        /// <summary>
        /// Тип тезиса
        /// </summary>
        public DefinitionKind Kind
        {
            get
            {
                string s = this.GetStringValue(ATTR_KIND);
                if (s == null) 
                    return DefinitionKind.Undefined;
                try 
                {
                    object res = Enum.Parse(typeof(DefinitionKind), s, true);
                    if (res is DefinitionKind) 
                        return (DefinitionKind)res;
                }
                catch(Exception ex1466) 
                {
                }
                return DefinitionKind.Undefined;
            }
            set
            {
                this.AddSlot(ATTR_KIND, value.ToString(), true, 0);
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            string misc = this.GetStringValue(ATTR_TERMIN_ADD);
            if (misc == null) 
                misc = this.GetStringValue(ATTR_MISC);
            return string.Format("[{0}] {1}{2} = {3}", Kind.ToString(), Termin ?? "?", (misc == null ? "" : string.Format(" ({0})", misc)), Value ?? "?");
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            DefinitionReferent dr = obj as DefinitionReferent;
            if (dr == null) 
                return false;
            if (Termin != dr.Termin) 
                return false;
            if (Value != dr.Value) 
                return false;
            if (TerminAdd != dr.TerminAdd) 
                return false;
            return true;
        }
    }
}