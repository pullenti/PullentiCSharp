/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Semantic.Utils
{
    /// <summary>
    /// Модель управления
    /// </summary>
    public class ControlModel
    {
        /// <summary>
        /// Элементы модели
        /// </summary>
        public List<ControlModelItem> Items = new List<ControlModelItem>();
        /// <summary>
        /// Типовые пациенты (устойчивые словосочетания)
        /// </summary>
        public List<string> Pacients = new List<string>();
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            foreach (ControlModelItem it in Items) 
            {
                if (it.Ignorable) 
                    continue;
                if (res.Length > 0) 
                    res.Append("; ");
                if (it.Typ == ControlModelItemType.Word) 
                    res.AppendFormat("{0} = {1}", it.Word, it.Links.Count);
                else 
                    res.AppendFormat("{0} = {1}", it.Typ.ToString(), it.Links.Count);
            }
            foreach (string p in Pacients) 
            {
                res.AppendFormat(" ({0})", p);
            }
            return res.ToString();
        }
        public ControlModelItem FindItemByTyp(ControlModelItemType typ)
        {
            foreach (ControlModelItem it in Items) 
            {
                if (it.Typ == typ) 
                    return it;
            }
            return null;
        }
        internal void Deserialize(Pullenti.Morph.Internal.ByteArrayWrapper str, ref int pos)
        {
            int cou = str.DeserializeShort(ref pos);
            for (; cou > 0; cou--) 
            {
                ControlModelItem it = new ControlModelItem();
                byte b = str.DeserializeByte(ref pos);
                if (((b & 0x80)) != 0) 
                    it.NominativeCanBeAgentAndPacient = true;
                it.Typ = (ControlModelItemType)((b & 0x7F));
                if (it.Typ == ControlModelItemType.Word) 
                    it.Word = str.DeserializeString(ref pos);
                int licou = str.DeserializeShort(ref pos);
                for (; licou > 0; licou--) 
                {
                    byte bi = str.DeserializeByte(ref pos);
                    int i = (int)bi;
                    b = str.DeserializeByte(ref pos);
                    if (i >= 0 && (i < ControlModelQuestion.Items.Count)) 
                        it.Links.Add(ControlModelQuestion.Items[i], (Pullenti.Semantic.Core.SemanticRole)b);
                }
                Items.Add(it);
            }
            cou = str.DeserializeShort(ref pos);
            for (; cou > 0; cou--) 
            {
                string p = str.DeserializeString(ref pos);
                if (p != null) 
                    Pacients.Add(p);
            }
        }
    }
}