/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pullenti.Ner
{
    /// <summary>
    /// Токен, соответствующий сущности
    /// </summary>
    public class ReferentToken : MetaToken
    {
        public ReferentToken(Referent entity, Token begin, Token end, Pullenti.Ner.Core.AnalysisKit kit = null) : base(begin, end, kit)
        {
            Referent = entity;
            if (Morph == null) 
                Morph = new MorphCollection();
        }
        /// <summary>
        /// Ссылка на сущность
        /// </summary>
        public Referent Referent;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder((Referent == null ? "Null" : Referent.ToString()));
            if (Morph != null) 
                res.AppendFormat(" {0}", Morph.ToString());
            return res.ToString();
        }
        public override Referent GetReferent()
        {
            return Referent;
        }
        public override List<Referent> GetReferents()
        {
            List<Referent> res = new List<Referent>();
            if (Referent != null) 
                res.Add(Referent);
            List<Referent> ri = base.GetReferents();
            if (ri != null) 
                res.AddRange(ri);
            return res;
        }
        public virtual void SaveToLocalOntology()
        {
            if (Data == null) 
                return;
            Referent r = Data.RegisterReferent(Referent);
            Data = null;
            if (r != null) 
            {
                Referent = r;
                TextAnnotation anno = new TextAnnotation();
                anno.Sofa = Kit.Sofa;
                anno.OccurenceOf = Referent;
                anno.BeginChar = BeginChar;
                anno.EndChar = EndChar;
                Referent.AddOccurence(anno);
            }
        }
        public void SetDefaultLocalOnto(Processor proc)
        {
            if (Referent == null || Kit == null || proc == null) 
                return;
            foreach (Analyzer a in proc.Analyzers) 
            {
                if (a.CreateReferent(Referent.TypeName) != null) 
                {
                    Data = Kit.GetAnalyzerData(a);
                    break;
                }
            }
        }
        public Pullenti.Ner.Core.AnalyzerData Data;
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public int MiscAttrs;
        internal void ReplaceReferent(Referent oldReferent, Referent newReferent)
        {
            if (Referent == oldReferent) 
                Referent = newReferent;
            if (EndToken == null) 
                return;
            for (Token t = BeginToken; t != null; t = t.Next) 
            {
                if (t.EndChar > EndChar) 
                    break;
                if (t is ReferentToken) 
                    (t as ReferentToken).ReplaceReferent(oldReferent, newReferent);
                if (t == EndToken) 
                    break;
            }
        }
        internal override void Serialize(Stream stream)
        {
            base.Serialize(stream);
            int id = 0;
            if (Referent != null && (Referent.Tag is int)) 
                id = (int)Referent.Tag;
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, id);
        }
        internal override void Deserialize(Stream stream, Pullenti.Ner.Core.AnalysisKit kit, int vers)
        {
            base.Deserialize(stream, kit, vers);
            int id = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            if (id > 0) 
                Referent = kit.Entities[id - 1];
        }
    }
}