/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Instrument
{
    /// <summary>
    /// Для судебных решений формализованная резолюция (пока).
    /// </summary>
    public class InstrumentArtefactReferent : Pullenti.Ner.Referent
    {
        public InstrumentArtefactReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Instrument.Internal.InstrumentArtefactMeta.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("INSTRARTEFACT")
        /// </summary>
        public const string OBJ_TYPENAME = "INSTRARTEFACT";
        /// <summary>
        /// Имя атрибута - тип артефакта
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - значение артефакта
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - ссылка на сущность (если есть)
        /// </summary>
        public const string ATTR_REF = "REF";
        /// <summary>
        /// Тип
        /// </summary>
        public string Typ
        {
            get
            {
                return this.GetStringValue(ATTR_TYPE);
            }
            set
            {
                this.AddSlot(ATTR_TYPE, (value == null ? null : value.ToUpper()), true, 0);
            }
        }
        /// <summary>
        /// Значение
        /// </summary>
        public object Value
        {
            get
            {
                return this.GetSlotValue(ATTR_VALUE);
            }
            set
            {
                this.AddSlot(ATTR_VALUE, value, false, 0);
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            res.Append(Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(Typ ?? "?"));
            object val = Value;
            if (val != null) 
                res.AppendFormat(": {0}", val);
            if (!shortVariant && (lev < 30)) 
            {
                Pullenti.Ner.Referent re = this.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
                if (re != null) 
                    res.AppendFormat(" ({0})", re.ToString(shortVariant, lang, lev + 1));
            }
            return res.ToString();
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            InstrumentArtefactReferent p = obj as InstrumentArtefactReferent;
            if (p == null) 
                return false;
            if (Typ != p.Typ) 
                return false;
            if (Value != p.Value) 
                return false;
            return true;
        }
    }
}