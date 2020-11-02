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
    /// Документ
    /// </summary>
    public class SemDocument : ISemContainer
    {
        /// <summary>
        /// Семантические объекты уровня документа
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
                return null;
            }
        }
        /// <summary>
        /// Блоки документа - список SemBlock
        /// </summary>
        public List<SemBlock> Blocks = new List<SemBlock>();
        public int BeginChar
        {
            get
            {
                return (Blocks.Count == 0 ? 0 : Blocks[0].BeginChar);
            }
        }
        public int EndChar
        {
            get
            {
                return (Blocks.Count == 0 ? 0 : Blocks[Blocks.Count - 1].EndChar);
            }
        }
        public void MergeAllBlocks()
        {
            if (Blocks.Count < 2) 
                return;
            for (int i = 1; i < Blocks.Count; i++) 
            {
                Blocks[0].MergeWith(Blocks[i]);
            }
            Blocks.RemoveRange(1, Blocks.Count - 1);
        }
    }
}