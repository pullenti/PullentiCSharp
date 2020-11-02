/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Связь между фрагментами
    /// </summary>
    public class SemFraglink
    {
        /// <summary>
        /// Тип связи
        /// </summary>
        public SemFraglinkType Typ = SemFraglinkType.Undefined;
        /// <summary>
        /// Фрагмент-источник (откуда связь)
        /// </summary>
        public SemFragment Source;
        /// <summary>
        /// Фрагмент-приёмник (куда связь)
        /// </summary>
        public SemFragment Target;
        /// <summary>
        /// Возможный вопрос, на который даёт ответ связь
        /// </summary>
        public string Question;
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            if (Typ != SemFraglinkType.Undefined) 
                tmp.AppendFormat("{0} ", Typ);
            if (Question != null) 
                tmp.AppendFormat("{0}? ", Question);
            if (Source != null) 
                tmp.AppendFormat("{0} ", Source.ToString());
            if (Target != null) 
                tmp.AppendFormat("-> {0}", Target.ToString());
            return tmp.ToString();
        }
    }
}