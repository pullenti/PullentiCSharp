/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Instrument.Internal
{
    internal class MetaInstrument : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaInstrument();
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_TYPE, "Тип", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NUMBER, "Номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_CASENUMBER, "Номер дела", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_DATE, "Дата", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_SOURCE, "Публикующий орган", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_GEO, "Географический объект", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NAME, "Наименование", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_CHILD, "Внутренний элемент", 0, 0).ShowAsParent = true;
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_SIGNER, "Подписант", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PART, "Часть", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_APPENDIX, "Приложение", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PARTICIPANT, "Участник", 0, 0).ShowAsParent = true;
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_ARTEFACT, "Артефакт", 0, 0).ShowAsParent = true;
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Instrument.InstrumentReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Нормативно-правовой акт";
            }
        }
        public static string DocImageId = "decree";
        public static string PartImageId = "part";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return DocImageId;
        }
        public static MetaInstrument GlobalMeta;
    }
}