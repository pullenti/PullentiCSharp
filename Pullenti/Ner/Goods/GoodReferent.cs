/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Goods
{
    /// <summary>
    /// Товар
    /// </summary>
    public class GoodReferent : Pullenti.Ner.Referent
    {
        public GoodReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Goods.Internal.GoodMeta.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("GOOD")
        /// </summary>
        public const string OBJ_TYPENAME = "GOOD";
        /// <summary>
        /// Имя атрибута - атрибут (характеристика) товара (GoodAttributeReferent)
        /// </summary>
        public const string ATTR_ATTR = "ATTR";
        /// <summary>
        /// Атрибуты товара (список GoodAttributeReferent)
        /// </summary>
        public ICollection<GoodAttributeReferent> Attrs
        {
            get
            {
                List<GoodAttributeReferent> res = new List<GoodAttributeReferent>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.Value is GoodAttributeReferent) 
                        res.Add(s.Value as GoodAttributeReferent);
                }
                return res;
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            foreach (GoodAttributeReferent a in Attrs) 
            {
                res.AppendFormat("{0} ", a.ToString(true, lang, lev));
            }
            return res.ToString().Trim();
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            return this == obj;
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            Pullenti.Ner.Core.IntOntologyItem re = new Pullenti.Ner.Core.IntOntologyItem(this);
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_ATTR) 
                    re.Termins.Add(new Pullenti.Ner.Core.Termin(s.Value.ToString()));
            }
            return re;
        }
    }
}