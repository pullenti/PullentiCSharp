/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Instrument
{
    /// <summary>
    /// Анализатор структуры нормативных актов и договоров: восставовление иерархической структуры фрагментов, 
    /// выделение фигурантов (для договоров и судебных документов), артефактов. 
    /// Специфический анализатор, то есть нужно явно создавать процессор через функцию CreateSpecificProcessor, 
    /// указав имя анализатора.
    /// </summary>
    public class InstrumentAnalyzer : Pullenti.Ner.Analyzer
    {
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        /// <summary>
        /// Имя анализатора ("INSTRUMENT")
        /// </summary>
        public const string ANALYZER_NAME = "INSTRUMENT";
        public override string Caption
        {
            get
            {
                return "Структура нормативно-правовых документов (НПА)";
            }
        }
        public override string Description
        {
            get
            {
                return "Разбор структуры НПА на разделы и подразделы";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new InstrumentAnalyzer();
        }
        /// <summary>
        /// Этот анализатор является специфическим (IsSpecific = true)
        /// </summary>
        public override bool IsSpecific
        {
            get
            {
                return true;
            }
        }
        public override int ProgressWeight
        {
            get
            {
                return 1;
            }
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Instrument.Internal.MetaInstrument.GlobalMeta, Pullenti.Ner.Instrument.Internal.MetaInstrumentBlock.GlobalMeta, Pullenti.Ner.Instrument.Internal.InstrumentParticipantMeta.GlobalMeta, Pullenti.Ner.Instrument.Internal.InstrumentArtefactMeta.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Instrument.Internal.MetaInstrument.DocImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("decree.png"));
                res.Add(Pullenti.Ner.Instrument.Internal.MetaInstrumentBlock.PartImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("part.png"));
                res.Add(Pullenti.Ner.Instrument.Internal.InstrumentParticipantMeta.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("participant.png"));
                res.Add(Pullenti.Ner.Instrument.Internal.InstrumentArtefactMeta.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("artefact.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == InstrumentReferent.OBJ_TYPENAME) 
                return new InstrumentReferent();
            if (type == InstrumentBlockReferent.OBJ_TYPENAME) 
                return new InstrumentBlockReferent();
            if (type == InstrumentParticipantReferent.OBJ_TYPENAME) 
                return new InstrumentParticipantReferent();
            if (type == InstrumentArtefactReferent.OBJ_TYPENAME) 
                return new InstrumentArtefactReferent();
            return null;
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Token t = kit.FirstToken;
            Pullenti.Ner.Token t1 = t;
            if (t == null) 
                return;
            Pullenti.Ner.Instrument.Internal.FragToken dfr = Pullenti.Ner.Instrument.Internal.FragToken.CreateDocument(t, 0, InstrumentKind.Undefined);
            if (dfr == null) 
                return;
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            InstrumentBlockReferent res = dfr.CreateReferent(ad);
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Ner.Instrument.Internal.InstrumentArtefactMeta.Initialize();
            Pullenti.Ner.Instrument.Internal.MetaInstrumentBlock.Initialize();
            Pullenti.Ner.Instrument.Internal.MetaInstrument.Initialize();
            Pullenti.Ner.Instrument.Internal.InstrumentParticipantMeta.Initialize();
            try 
            {
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Instrument.Internal.InstrToken.Initialize();
                Pullenti.Ner.Instrument.Internal.ParticipantToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new InstrumentAnalyzer());
        }
    }
}