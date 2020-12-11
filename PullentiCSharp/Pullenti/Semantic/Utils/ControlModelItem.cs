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

namespace Pullenti.Semantic.Utils
{
    /// <summary>
    /// Элемент модели управления
    /// </summary>
    public class ControlModelItem
    {
        /// <summary>
        /// Тип элемента
        /// </summary>
        public ControlModelItemType Typ = ControlModelItemType.Word;
        /// <summary>
        /// Возможное слово (если Typ == ControlModelItemType.Word)
        /// </summary>
        public string Word;
        /// <summary>
        /// Связи "вопрос - роль".
        /// </summary>
        public Dictionary<ControlModelQuestion, Pullenti.Semantic.Core.SemanticRole> Links = new Dictionary<ControlModelQuestion, Pullenti.Semantic.Core.SemanticRole>();
        /// <summary>
        /// Именительный падеж м.б. агентом и пациентом (для возвратных глаголов). 
        /// Например, "ЗАЩИЩАТЬСЯ"
        /// </summary>
        public bool NominativeCanBeAgentAndPacient;
        public bool Ignorable;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Ignorable) 
                res.Append("IGNORE ");
            if (Typ != ControlModelItemType.Word) 
                res.AppendFormat("{0}: ", Typ);
            else 
                res.AppendFormat("{0}: ", Word ?? "?");
            foreach (KeyValuePair<ControlModelQuestion, Pullenti.Semantic.Core.SemanticRole> li in Links) 
            {
                if (li.Value == Pullenti.Semantic.Core.SemanticRole.Agent) 
                    res.Append("аг:");
                else if (li.Value == Pullenti.Semantic.Core.SemanticRole.Pacient) 
                    res.Append("пац:");
                else if (li.Value == Pullenti.Semantic.Core.SemanticRole.Strong) 
                    res.Append("сильн:");
                res.AppendFormat("{0}? ", li.Key.Spelling);
            }
            return res.ToString();
        }
    }
}