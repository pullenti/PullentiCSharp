/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Pullenti.Ner
{
    /// <summary>
    /// Внешняя "онтология". Содержит дополнительтную информацию для обработки (сущностей) - 
    /// это список элементов, связанных с внешними сущностями. 
    /// Подаётся необязательным параметром на вход методу Process() класса Processor.
    /// </summary>
    public class ExtOntology
    {
        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="extId">произвольный объект</param>
        /// <param name="typeName">имя типа сущности</param>
        /// <param name="definition">текстовое определение. Определение может содержать несколько 
        /// отдельных фрагментов, которые разделяются точкой с запятой. 
        /// Например, Министерство Обороны России; Минобороны</param>
        /// <return>если null, то не получилось...</return>
        public ExtOntologyItem Add(object extId, string typeName, string definition)
        {
            if (typeName == null || definition == null) 
                return null;
            List<Referent> rs = this._createReferent(typeName, definition);
            if (rs == null) 
                return null;
            m_Hash = null;
            ExtOntologyItem res = new ExtOntologyItem() { ExtId = extId, Referent = rs[0], TypeName = typeName };
            if (rs.Count > 1) 
            {
                rs.RemoveAt(0);
                res.Refs = rs;
            }
            Items.Add(res);
            return res;
        }
        /// <summary>
        /// Добавить готовую сущность
        /// </summary>
        /// <param name="extId">произвольный объект</param>
        /// <param name="referent">готовая сущность (например, сфомированная явно)</param>
        /// <return>новая запись словаря</return>
        public ExtOntologyItem AddReferent(object extId, Referent referent)
        {
            if (referent == null) 
                return null;
            m_Hash = null;
            ExtOntologyItem res = new ExtOntologyItem() { ExtId = extId, Referent = referent, TypeName = referent.TypeName };
            Items.Add(res);
            return res;
        }
        List<Referent> _createReferent(string typeName, string definition)
        {
            Analyzer analyzer = null;
            if (!m_AnalByType.TryGetValue(typeName, out analyzer)) 
                return null;
            SourceOfAnalysis sf = new SourceOfAnalysis(definition);
            AnalysisResult ar = m_Processor._process(sf, true, true, null, null);
            if (ar == null || ar.FirstToken == null) 
                return null;
            Referent r0 = ar.FirstToken.GetReferent();
            Token t = null;
            if (r0 != null) 
            {
                if (r0.TypeName != typeName) 
                    r0 = null;
            }
            if (r0 != null) 
                t = ar.FirstToken;
            else 
            {
                ReferentToken rt = analyzer.ProcessOntologyItem(ar.FirstToken);
                if (rt == null) 
                    return null;
                r0 = rt.Referent;
                t = rt.EndToken;
            }
            for (t = t.Next; t != null; t = t.Next) 
            {
                if (t.IsChar(';') && t.Next != null) 
                {
                    Referent r1 = t.Next.GetReferent();
                    if (r1 == null) 
                    {
                        ReferentToken rt = analyzer.ProcessOntologyItem(t.Next);
                        if (rt == null) 
                            continue;
                        t = rt.EndToken;
                        r1 = rt.Referent;
                    }
                    if (r1.TypeName == typeName) 
                    {
                        r0.MergeSlots(r1, true);
                        r1.Tag = r0;
                    }
                }
            }
            if (r0 == null) 
                return null;
            r0.Tag = r0;
            r0 = analyzer.PersistAnalizerData.RegisterReferent(r0);
            m_Processor._createRes(ar.FirstToken.Kit, ar, null, true);
            List<Referent> res = new List<Referent>();
            res.Add(r0);
            foreach (Referent e in ar.Entities) 
            {
                if (e.Tag == null) 
                    res.Add(e);
            }
            return res;
        }
        /// <summary>
        /// Обновить существующий элемент онтологии
        /// </summary>
        /// <param name="definition">новое определение</param>
        /// <return>признак успешности обновления</return>
        public bool Refresh(ExtOntologyItem item, string definition)
        {
            if (item == null) 
                return false;
            List<Referent> rs = this._createReferent(item.TypeName, definition);
            if (rs == null) 
                return false;
            return this.Refresh(item, rs[0]);
        }
        /// <summary>
        /// Обновить существующий элемент онтологии новой сущностью
        /// </summary>
        /// <param name="item">обновляемый элемент</param>
        /// <param name="newReferent">сущность</param>
        /// <return>признак успешности обновления</return>
        public bool Refresh(ExtOntologyItem item, Referent newReferent)
        {
            if (item == null) 
                return false;
            Analyzer analyzer = null;
            if (!m_AnalByType.TryGetValue(item.TypeName, out analyzer)) 
                return false;
            if (analyzer.PersistAnalizerData == null) 
                return true;
            if (item.Referent != null) 
                analyzer.PersistAnalizerData.RemoveReferent(item.Referent);
            Referent oldReferent = item.Referent;
            newReferent = analyzer.PersistAnalizerData.RegisterReferent(newReferent);
            item.Referent = newReferent;
            m_Hash = null;
            if (oldReferent != null && newReferent != null) 
            {
                foreach (Analyzer a in m_Processor.Analyzers) 
                {
                    if (a.PersistAnalizerData != null) 
                    {
                        foreach (Referent rr in a.PersistAnalizerData.Referents) 
                        {
                            foreach (Slot s in newReferent.Slots) 
                            {
                                if (s.Value == oldReferent) 
                                    newReferent.UploadSlot(s, rr);
                            }
                            foreach (Slot s in rr.Slots) 
                            {
                                if (s.Value == oldReferent) 
                                    rr.UploadSlot(s, newReferent);
                            }
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Список элементов внешней онтологии
        /// </summary>
        public List<ExtOntologyItem> Items = new List<ExtOntologyItem>();
        public ExtOntology(string specNames = null)
        {
            m_Specs = specNames;
            this._init();
        }
        void _init()
        {
            m_Processor = ProcessorService.CreateSpecificProcessor(m_Specs);
            m_AnalByType = new Dictionary<string, Analyzer>();
            foreach (Analyzer a in m_Processor.Analyzers) 
            {
                a.PersistReferentsRegim = true;
                if (a.Name == "DENOMINATION") 
                    a.IgnoreThisAnalyzer = true;
                else 
                    foreach (Pullenti.Ner.Metadata.ReferentClass t in a.TypeSystem) 
                    {
                        if (!m_AnalByType.ContainsKey(t.Name)) 
                            m_AnalByType.Add(t.Name, a);
                    }
            }
        }
        Processor m_Processor;
        string m_Specs;
        Dictionary<string, Analyzer> m_AnalByType;
        /// <summary>
        /// Сериализовать весь словарь в поток
        /// </summary>
        /// <param name="stream">поток для сериализации</param>
        public void Serialize(Stream stream)
        {
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, m_Specs);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, Items.Count);
            foreach (ExtOntologyItem it in Items) 
            {
                it.Serialize(stream);
            }
        }
        /// <summary>
        /// Восстановить словарь из потока
        /// </summary>
        /// <param name="stream">поток для десериализации</param>
        public void Deserialize(Stream stream)
        {
            m_Specs = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
            this._init();
            int cou = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            for (; cou > 0; cou--) 
            {
                ExtOntologyItem it = new ExtOntologyItem();
                it.Deserialize(stream);
                Items.Add(it);
            }
            this._initHash();
        }
        public Pullenti.Ner.Core.AnalyzerData _getAnalyzerData(string typeName)
        {
            Analyzer a;
            if (!m_AnalByType.TryGetValue(typeName, out a)) 
                return null;
            return a.PersistAnalizerData;
        }
        Dictionary<string, Pullenti.Ner.Core.IntOntologyCollection> m_Hash = null;
        void _initHash()
        {
            m_Hash = new Dictionary<string, Pullenti.Ner.Core.IntOntologyCollection>();
            foreach (ExtOntologyItem it in Items) 
            {
                if (it.Referent != null) 
                    it.Referent.OntologyItems = null;
            }
            foreach (ExtOntologyItem it in Items) 
            {
                if (it.Referent != null) 
                {
                    Pullenti.Ner.Core.IntOntologyCollection ont;
                    if (!m_Hash.TryGetValue(it.Referent.TypeName, out ont)) 
                        m_Hash.Add(it.Referent.TypeName, (ont = new Pullenti.Ner.Core.IntOntologyCollection() { IsExtOntology = true }));
                    if (it.Referent.OntologyItems == null) 
                        it.Referent.OntologyItems = new List<ExtOntologyItem>();
                    it.Referent.OntologyItems.Add(it);
                    it.Referent.IntOntologyItem = null;
                    ont.AddReferent(it.Referent);
                }
            }
        }
        /// <summary>
        /// Привязать сущность к существующей записи
        /// </summary>
        /// <param name="r">внешняя сущность</param>
        /// <return>null или список подходящих элементов</return>
        public List<ExtOntologyItem> AttachReferent(Referent r)
        {
            if (m_Hash == null) 
                this._initHash();
            Pullenti.Ner.Core.IntOntologyCollection onto;
            if (!m_Hash.TryGetValue(r.TypeName, out onto)) 
                return null;
            List<Referent> li = onto.TryAttachByReferent(r, null, false);
            if (li == null || li.Count == 0) 
                return null;
            List<ExtOntologyItem> res = null;
            foreach (Referent rr in li) 
            {
                if (rr.OntologyItems != null) 
                {
                    if (res == null) 
                        res = new List<ExtOntologyItem>();
                    res.AddRange(rr.OntologyItems);
                }
            }
            return res;
        }
        // Используется внутренним образом
        public List<Pullenti.Ner.Core.IntOntologyToken> AttachToken(string typeName, Token t)
        {
            if (m_Hash == null) 
                this._initHash();
            Pullenti.Ner.Core.IntOntologyCollection onto;
            if (!m_Hash.TryGetValue(typeName, out onto)) 
                return null;
            return onto.TryAttach(t, null, false);
        }
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag;
    }
}