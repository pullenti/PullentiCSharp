/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Фрагмент блока (предложение)
    /// </summary>
    public class SemFragment : ISemContainer
    {
        public SemFragment(SemBlock blk)
        {
            m_Higher = blk;
        }
        /// <summary>
        /// Объекты фрагмента (отметим, что часть объектов, связанных с этим блоком, 
        /// могут находиться в графах вышележащих уровней).
        /// </summary>
        public SemGraph Graph
        {
            get
            {
                return m_Graph;
            }
        }
        SemGraph m_Graph = new SemGraph();
        public ISemContainer Higher
        {
            get
            {
                return m_Higher;
            }
        }
        public SemBlock m_Higher;
        /// <summary>
        /// Владелец фрагмента
        /// </summary>
        public SemBlock Block
        {
            get
            {
                return m_Higher;
            }
        }
        /// <summary>
        /// Тип фрагмента
        /// </summary>
        public SemFragmentType Typ = SemFragmentType.Undefined;
        /// <summary>
        /// Корневые объекты объединены как ИЛИ (иначе И)
        /// </summary>
        public bool IsOr;
        /// <summary>
        /// Список объектов SemObject, у которых нет связей. При нормальном разборе 
        /// такой объект должен быть один - это обычно предикат.
        /// </summary>
        public List<SemObject> RootObjects
        {
            get
            {
                List<SemObject> res = new List<SemObject>();
                foreach (SemObject o in m_Graph.Objects) 
                {
                    if (o.LinksTo.Count == 0) 
                        res.Add(o);
                }
                return res;
            }
        }
        public bool CanBeErrorStructure
        {
            get
            {
                int cou = 0;
                int vcou = 0;
                foreach (SemObject o in m_Graph.Objects) 
                {
                    if (o.LinksTo.Count == 0) 
                    {
                        if (o.Typ == SemObjectType.Verb) 
                            vcou++;
                        cou++;
                    }
                }
                if (cou <= 1) 
                    return false;
                return vcou < cou;
            }
        }
        /// <summary>
        /// Текст фрагмента
        /// </summary>
        public string Spelling
        {
            get
            {
                return Pullenti.Ner.Core.MiscHelper.GetTextValue(BeginToken, EndToken, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
            }
        }
        /// <summary>
        /// Начальный токен
        /// </summary>
        public Pullenti.Ner.Token BeginToken;
        /// <summary>
        /// Конечный токен
        /// </summary>
        public Pullenti.Ner.Token EndToken;
        public int BeginChar
        {
            get
            {
                return (BeginToken == null ? 0 : BeginToken.BeginChar);
            }
        }
        public int EndChar
        {
            get
            {
                return (EndToken == null ? 0 : EndToken.EndChar);
            }
        }
        /// <summary>
        /// Используйте произвольным образом
        /// </summary>
        public object Tag;
        public override string ToString()
        {
            if (Typ != SemFragmentType.Undefined) 
                return string.Format("{0}: {1}", Typ, Spelling ?? "?");
            else 
                return Spelling ?? "?";
        }
    }
}