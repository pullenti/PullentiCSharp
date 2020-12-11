/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Данные, полученные в ходе обработки одним анализатором. Каждый анализатор сохраняет в своём класса свои данные, 
    /// получаемые в ходе анализа. В конце процессор объединяет их все. Получить экземпляр, связанный с анализатором, 
    /// можно методом AnalyzerKit.GetAnalyzerDataByAnalyzerName.
    /// </summary>
    public class AnalyzerData
    {
        /// <summary>
        /// Ссылка на аналитический контейнер
        /// </summary>
        public AnalysisKit Kit;
        /// <summary>
        /// Список выделенных сущностей Referent
        /// </summary>
        public virtual ICollection<Pullenti.Ner.Referent> Referents
        {
            get
            {
                return m_Referents;
            }
            set
            {
                m_Referents.Clear();
                if (value != null) 
                    m_Referents.AddRange(value);
            }
        }
        protected List<Pullenti.Ner.Referent> m_Referents = new List<Pullenti.Ner.Referent>();
        int m_RegRefLevel = 0;
        /// <summary>
        /// Зарегистрировать новую сущность или привязать к существующей сущности. Сущности, получаемые в ходе анализа, 
        /// должны сохраняться через эту функцию. Именно здесь решается задача кореференции, то есть объединения 
        /// сущностей, соответствующих одному и тому же объекту текста.
        /// </summary>
        /// <param name="referent">сохраняемая сущность</param>
        /// <return>этот же экземпляр referent или другой, если удалось объединиться с ранее выделенной сущностью</return>
        public virtual Pullenti.Ner.Referent RegisterReferent(Pullenti.Ner.Referent referent)
        {
            if (referent == null) 
                return null;
            if (referent.m_ExtReferents != null) 
            {
                if (m_RegRefLevel > 2) 
                {
                }
                else 
                {
                    foreach (Pullenti.Ner.ReferentToken rt in referent.m_ExtReferents) 
                    {
                        Pullenti.Ner.Referent oldRef = rt.Referent;
                        m_RegRefLevel++;
                        rt.SaveToLocalOntology();
                        m_RegRefLevel--;
                        if (oldRef == rt.Referent || rt.Referent == null) 
                            continue;
                        foreach (Pullenti.Ner.Slot s in referent.Slots) 
                        {
                            if (s.Value == oldRef) 
                                referent.UploadSlot(s, rt.Referent);
                        }
                        if (referent.m_ExtReferents != null) 
                        {
                            foreach (Pullenti.Ner.ReferentToken rtt in referent.m_ExtReferents) 
                            {
                                foreach (Pullenti.Ner.Slot s in rtt.Referent.Slots) 
                                {
                                    if (s.Value == oldRef) 
                                        referent.UploadSlot(s, rt.Referent);
                                }
                            }
                        }
                    }
                    referent.m_ExtReferents = null;
                }
            }
            List<Pullenti.Ner.Referent> eq = null;
            if (m_Referents.Contains(referent)) 
                return referent;
            for (int i = m_Referents.Count - 1; i >= 0 && ((m_Referents.Count - i) < 1000); i--) 
            {
                Pullenti.Ner.Referent p = m_Referents[i];
                if (p.CanBeEquals(referent, ReferentsEqualType.WithinOneText)) 
                {
                    if (!p.CanBeGeneralFor(referent) && !referent.CanBeGeneralFor(p)) 
                    {
                        if (eq == null) 
                            eq = new List<Pullenti.Ner.Referent>();
                        eq.Add(p);
                    }
                }
            }
            if (eq != null) 
            {
                if (eq.Count == 1) 
                {
                    eq[0].MergeSlots(referent, true);
                    return eq[0];
                }
                if (eq.Count > 1) 
                {
                    foreach (Pullenti.Ner.Referent e in eq) 
                    {
                        if (e.Slots.Count != referent.Slots.Count) 
                            continue;
                        bool ok = true;
                        foreach (Pullenti.Ner.Slot s in referent.Slots) 
                        {
                            if (e.FindSlot(s.TypeName, s.Value, true) == null) 
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (ok) 
                        {
                            foreach (Pullenti.Ner.Slot s in e.Slots) 
                            {
                                if (referent.FindSlot(s.TypeName, s.Value, true) == null) 
                                {
                                    ok = false;
                                    break;
                                }
                            }
                        }
                        if (ok) 
                            return e;
                    }
                }
            }
            m_Referents.Add(referent);
            return referent;
        }
        /// <summary>
        /// Удалить сущность из списка
        /// </summary>
        /// <param name="r">удаляемая сущность</param>
        public virtual void RemoveReferent(Pullenti.Ner.Referent r)
        {
            if (m_Referents.Contains(r)) 
                m_Referents.Remove(r);
        }
        public int OverflowLevel = 0;
    }
}