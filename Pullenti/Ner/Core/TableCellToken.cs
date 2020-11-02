/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core
{
    // Токен - ячейка таблицы
    public class TableCellToken : Pullenti.Ner.MetaToken
    {
        public TableCellToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        /// <summary>
        /// Количество объединённых ячеек по горизонтали
        /// </summary>
        public int ColSpan = 1;
        /// <summary>
        /// Количество объединённых ячеек по вертикали
        /// </summary>
        public int RowSpan = 1;
        internal bool Eoc = false;
        internal List<TableCellToken> Lines
        {
            get
            {
                List<TableCellToken> res = new List<TableCellToken>();
                for (Pullenti.Ner.Token t = BeginToken; t != null && t.EndChar <= EndChar; t = t.Next) 
                {
                    Pullenti.Ner.Token t0 = t;
                    Pullenti.Ner.Token t1 = t;
                    for (; t != null && t.EndChar <= EndChar; t = t.Next) 
                    {
                        t1 = t;
                        if (t.IsNewlineAfter) 
                        {
                            if ((t.Next != null && t.Next.EndChar <= EndChar && t.Next.Chars.IsLetter) && t.Next.Chars.IsAllLower && !t0.Chars.IsAllLower) 
                                continue;
                            break;
                        }
                    }
                    res.Add(new TableCellToken(t0, t1));
                    t = t1;
                }
                return res;
            }
        }
    }
}