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
    /// Семантическая связь между объектами
    /// </summary>
    public class SemLink
    {
        internal SemLink(SemGraph gr, SemObject src, SemObject tgt)
        {
            Graph = gr;
            m_Source = src;
            m_Target = tgt;
            src.LinksFrom.Add(this);
            tgt.LinksTo.Add(this);
        }
        /// <summary>
        /// Граф, владеющий связью (кстати, сами объекты у связи могут принадлежать разным графам).
        /// </summary>
        public SemGraph Graph;
        /// <summary>
        /// Тип связи
        /// </summary>
        public SemLinkType Typ = SemLinkType.Undefined;
        /// <summary>
        /// Объект начала связи
        /// </summary>
        public SemObject Source
        {
            get
            {
                return m_Source;
            }
        }
        SemObject m_Source;
        /// <summary>
        /// Объект конца связи
        /// </summary>
        public SemObject Target
        {
            get
            {
                return m_Target;
            }
        }
        internal SemObject m_Target;
        /// <summary>
        /// Альтернативная ссылка (парная, а та в свою очередь ссылается на эту). 
        /// Используется для неоднозначных связях.
        /// </summary>
        public SemLink AltLink;
        /// <summary>
        /// Вопрос, соответствующий связи
        /// </summary>
        public string Question;
        /// <summary>
        /// Предлог, если есть
        /// </summary>
        public string Preposition;
        /// <summary>
        /// Для нескольких однотипных связей из одного Source или Target обозначает логическое "или". 
        /// Если false, то логическое "и".
        /// </summary>
        public bool IsOr;
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag;
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            if (AltLink != null) 
                tmp.Append("??? ");
            if (IsOr) 
                tmp.Append("OR ");
            if (Typ != SemLinkType.Undefined) 
                tmp.Append(Typ.ToString());
            if (Question != null) 
                tmp.AppendFormat(" {0}?", Question);
            if (Source != null) 
                tmp.AppendFormat(" {0}", Source.ToString());
            if (Target != null) 
                tmp.AppendFormat(" -> {0}", Target.ToString());
            return tmp.ToString();
        }
    }
}