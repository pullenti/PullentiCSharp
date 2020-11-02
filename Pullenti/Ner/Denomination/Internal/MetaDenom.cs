/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Denomination.Internal
{
    class MetaDenom : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaDenom();
            GlobalMeta.AddFeature(Pullenti.Ner.Denomination.DenominationReferent.ATTR_VALUE, "Значение", 0, 1);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Denomination.DenominationReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Обозначение";
            }
        }
        public static string DenomImageId = "denom";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return DenomImageId;
        }
        internal static MetaDenom GlobalMeta;
    }
}