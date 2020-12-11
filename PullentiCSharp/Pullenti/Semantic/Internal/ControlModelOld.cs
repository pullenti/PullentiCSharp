/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Semantic.Internal
{
    public class ControlModelOld
    {
        public bool Transitive;
        public Dictionary<string, Pullenti.Morph.MorphCase> Nexts;
        public Pullenti.Semantic.Utils.QuestionType Questions = Pullenti.Semantic.Utils.QuestionType.Undefined;
        public NextModelItem Agent;
        public NextModelItem Pacient;
        public NextModelItem Instrument;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Transitive) 
                res.Append("Перех.");
            if (Agent != null) 
                res.AppendFormat(" Агент:{0}", Agent);
            if (Pacient != null) 
                res.AppendFormat(" Пациент:{0}", Pacient);
            if (Instrument != null) 
                res.AppendFormat(" Инстр.:{0}", Instrument);
            if (Nexts != null) 
            {
                foreach (KeyValuePair<string, Pullenti.Morph.MorphCase> kp in Nexts) 
                {
                    res.AppendFormat(" [{0} {1}]", kp.Key ?? "", kp.Value);
                }
            }
            if (((Questions & Pullenti.Semantic.Utils.QuestionType.Where)) != Pullenti.Semantic.Utils.QuestionType.Undefined) 
                res.AppendFormat(" ГДЕ?");
            if (((Questions & Pullenti.Semantic.Utils.QuestionType.WhereFrom)) != Pullenti.Semantic.Utils.QuestionType.Undefined) 
                res.AppendFormat(" ОТКУДА?");
            if (((Questions & Pullenti.Semantic.Utils.QuestionType.WhereTo)) != Pullenti.Semantic.Utils.QuestionType.Undefined) 
                res.AppendFormat(" КУДА?");
            if (((Questions & Pullenti.Semantic.Utils.QuestionType.When)) != Pullenti.Semantic.Utils.QuestionType.Undefined) 
                res.AppendFormat(" КОГДА?");
            if (((Questions & Pullenti.Semantic.Utils.QuestionType.WhatToDo)) != Pullenti.Semantic.Utils.QuestionType.Undefined) 
                res.AppendFormat(" ЧТО ДЕЛАТЬ?");
            return res.ToString().Trim();
        }
        public bool CheckNext(string prep, Pullenti.Morph.MorphCase cas)
        {
            if (Nexts == null) 
                return false;
            Pullenti.Morph.MorphCase cas0;
            if (!Nexts.TryGetValue(prep ?? "", out cas0)) 
                return false;
            return !((cas0 & cas)).IsUndefined;
        }
        internal void Deserialize(Pullenti.Morph.Internal.ByteArrayWrapper str, ref int pos)
        {
            byte b = str.DeserializeByte(ref pos);
            Transitive = b != 0;
            int sh = str.DeserializeShort(ref pos);
            Questions = (Pullenti.Semantic.Utils.QuestionType)sh;
            sh = str.DeserializeShort(ref pos);
            if (sh != 0) 
            {
                string pr = str.DeserializeString(ref pos);
                Pullenti.Morph.MorphCase cas = new Pullenti.Morph.MorphCase();
                cas.Value = (short)sh;
                Agent = new NextModelItem(pr, cas);
            }
            sh = str.DeserializeShort(ref pos);
            if (sh != 0) 
            {
                string pr = str.DeserializeString(ref pos);
                Pullenti.Morph.MorphCase cas = new Pullenti.Morph.MorphCase();
                cas.Value = (short)sh;
                Pacient = new NextModelItem(pr, cas);
            }
            sh = str.DeserializeShort(ref pos);
            if (sh != 0) 
            {
                string pr = str.DeserializeString(ref pos);
                Pullenti.Morph.MorphCase cas = new Pullenti.Morph.MorphCase();
                cas.Value = (short)sh;
                Instrument = new NextModelItem(pr, cas);
            }
            int cou = str.DeserializeShort(ref pos);
            for (; cou > 0; cou--) 
            {
                string pref = str.DeserializeString(ref pos);
                if (pref == null) 
                    pref = "";
                Pullenti.Morph.MorphCase cas = new Pullenti.Morph.MorphCase();
                sh = str.DeserializeShort(ref pos);
                cas.Value = (short)sh;
                if (Nexts == null) 
                    Nexts = new Dictionary<string, Pullenti.Morph.MorphCase>();
                Nexts.Add(pref, cas);
            }
        }
    }
}