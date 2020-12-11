/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Pullenti.Ner
{
    /// <summary>
    /// Элемент внешней онтологии
    /// </summary>
    public class ExtOntologyItem
    {
        public ExtOntologyItem(string caption = null)
        {
            m_Caption = caption;
        }
        /// <summary>
        /// Внешний идентификатор (ссылка на что угодно)
        /// </summary>
        public object ExtId;
        /// <summary>
        /// Имя типа сущности
        /// </summary>
        public string TypeName;
        /// <summary>
        /// Ссылка на сущность
        /// </summary>
        public Referent Referent;
        // Используется внутренним образом
        internal List<Referent> Refs = null;
        string m_Caption;
        public override string ToString()
        {
            if (m_Caption != null) 
                return m_Caption;
            else if (Referent == null) 
                return string.Format("{0}: ?", TypeName ?? "?");
            else 
            {
                string res = Referent.ToString();
                if (Referent.ParentReferent != null) 
                {
                    string str1 = Referent.ParentReferent.ToString();
                    if (!res.Contains(str1)) 
                        res = res + "; " + str1;
                }
                return res;
            }
        }
        internal void Serialize(Stream stream)
        {
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, (ExtId == null ? null : ExtId.ToString()));
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, m_Caption);
            if (Refs == null) 
                Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, 0);
            else 
            {
                Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, Refs.Count);
                int id = 1;
                foreach (Referent r in Refs) 
                {
                    r.Tag = id++;
                }
                foreach (Referent r in Refs) 
                {
                    r.Occurrence.Clear();
                    Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, r.TypeName);
                    r.Serialize(stream);
                }
            }
            Referent.Occurrence.Clear();
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, TypeName);
            Referent.Serialize(stream);
        }
        internal void Deserialize(Stream stream)
        {
            ExtId = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
            m_Caption = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
            int cou = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            if (cou > 0) 
            {
                Refs = new List<Referent>();
                for (; cou > 0; cou--) 
                {
                    string typ = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
                    Referent r = ProcessorService.CreateReferent(typ);
                    r.Deserialize(stream, Refs, null);
                    Refs.Add(r);
                }
            }
            TypeName = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
            Referent = ProcessorService.CreateReferent(TypeName);
            Referent.Deserialize(stream, Refs, null);
        }
    }
}