/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Instrument.Internal
{
    internal class InstrumentArtefactMeta : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new InstrumentArtefactMeta();
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentArtefactReferent.ATTR_TYPE, "Тип", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentArtefactReferent.ATTR_VALUE, "Значение", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentArtefactReferent.ATTR_REF, "Ссылка на объект", 0, 1).ShowAsParent = true;
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
                return "Артефакт";
            }
        }
        public static string ImageId = "artefact";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        public static InstrumentArtefactMeta GlobalMeta;
    }
}