/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Instrument.Internal
{
    internal class InstrumentParticipantMeta : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new InstrumentParticipantMeta();
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_TYPE, "Тип", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, "Ссылка на объект", 0, 1).ShowAsParent = true;
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_DELEGATE, "Ссылка на представителя", 0, 1).ShowAsParent = true;
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_GROUND, "Основание", 0, 1).ShowAsParent = true;
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Instrument.InstrumentParticipantReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Участник";
            }
        }
        public static string ImageId = "participant";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        public static InstrumentParticipantMeta GlobalMeta;
    }
}