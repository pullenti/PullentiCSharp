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
    // Поддержка работы с таблицами, расположенными в текстах.
    // Начало таблицы - символ 1Eh, конец - 1Fh, ячейки оканчиваются 07h,
    // комбинация 0D 0A 07 - конец строки.
    // Данную структуру формирует функция извлечения текстов (ExtractText), так что это - для
    // обратного восстановления таблицы в случае необходимости.
    public static class TableHelper
    {
        /// <summary>
        /// Получить список строк таблицы
        /// </summary>
        /// <param name="t">начальная позиция</param>
        /// <param name="maxChar">максимальная позиция (0 - не ограничена)</param>
        /// <param name="mustBeStartOfTable">при true первый символ должен быть 1Eh</param>
        /// <return>список строк</return>
        public static List<TableRowToken> TryParseRows(Pullenti.Ner.Token t, int maxChar, bool mustBeStartOfTable)
        {
            if (t == null) 
                return null;
            bool isTab = false;
            if (mustBeStartOfTable) 
            {
                if (!t.IsChar((char)0x1E)) 
                    return null;
                isTab = true;
            }
            TableRowToken rw = Parse(t, maxChar, null, ref isTab);
            if (rw == null) 
                return null;
            List<TableRowToken> res = new List<TableRowToken>();
            res.Add(rw);
            for (t = rw.EndToken.Next; t != null; t = t.Next) 
            {
                TableRowToken rw0 = Parse(t, maxChar, rw, ref isTab);
                if (rw0 == null) 
                    break;
                res.Add((rw = rw0));
                t = rw0.EndToken;
                if (rw0.LastRow) 
                    break;
            }
            TableRowToken rla = res[res.Count - 1];
            if (((rla.LastRow && rla.Cells.Count == 2 && rla.Cells[0].ColSpan == 1) && rla.Cells[0].RowSpan == 1 && rla.Cells[1].ColSpan == 1) && rla.Cells[1].RowSpan == 1) 
            {
                List<TableCellToken> lines0 = rla.Cells[0].Lines;
                List<TableCellToken> lines1 = rla.Cells[1].Lines;
                if (lines0.Count > 2 && lines1.Count == lines0.Count) 
                {
                    for (int ii = 0; ii < lines0.Count; ii++) 
                    {
                        rw = new TableRowToken((ii == 0 ? lines0[ii].BeginToken : lines1[ii].BeginToken), (ii == 0 ? lines0[ii].EndToken : lines1[ii].EndToken));
                        rw.Cells.Add(lines0[ii]);
                        rw.Cells.Add(lines1[ii]);
                        rw.Eor = rla.Eor;
                        if (ii == (lines0.Count - 1)) 
                        {
                            rw.LastRow = rla.LastRow;
                            rw.EndToken = rla.EndToken;
                        }
                        res.Add(rw);
                    }
                    res.Remove(rla);
                }
            }
            foreach (TableRowToken re in res) 
            {
                if (re.Cells.Count > 1) 
                    return res;
                if (re.Cells.Count == 1) 
                {
                    if (_containsTableChar(re.Cells[0])) 
                        return res;
                }
            }
            return null;
        }
        static bool _containsTableChar(Pullenti.Ner.MetaToken mt)
        {
            for (Pullenti.Ner.Token tt = mt.BeginToken; tt != null && tt.EndChar <= mt.EndChar; tt = tt.Next) 
            {
                if (tt is Pullenti.Ner.MetaToken) 
                {
                    if (_containsTableChar(tt as Pullenti.Ner.MetaToken)) 
                        return true;
                }
                else if (((tt.IsTableControlChar && tt.Previous != null && !tt.Previous.IsTableControlChar) && tt.Next != null && !tt.Next.IsTableControlChar) && tt.Previous.BeginChar >= mt.BeginChar && tt.Next.EndChar <= mt.EndChar) 
                    return true;
            }
            return false;
        }
        static TableRowToken Parse(Pullenti.Ner.Token t, int maxChar, TableRowToken prev, ref bool isTab)
        {
            if (t == null || ((t.EndChar > maxChar && maxChar > 0))) 
                return null;
            string txt = t.Kit.Sofa.Text;
            Pullenti.Ner.Token t0 = t;
            if (t.IsChar((char)0x1E) && t.Next != null) 
            {
                isTab = true;
                t = t.Next;
            }
            Pullenti.Ner.Token tt;
            TableInfo cellInfo = null;
            for (tt = t; tt != null && ((tt.EndChar <= maxChar || maxChar == 0)); tt = tt.Next) 
            {
                if (tt.IsTableControlChar) 
                {
                    cellInfo = new TableInfo(tt);
                    if (cellInfo.Typ != TableTypes.CellEnd) 
                        cellInfo = null;
                    break;
                }
                else if (tt.IsNewlineAfter) 
                {
                    if (!isTab && prev == null) 
                        break;
                    if ((tt.EndChar - t.BeginChar) > 100) 
                    {
                        if ((tt.EndChar - t.BeginChar) > 10000) 
                            break;
                        if (!isTab) 
                            break;
                    }
                    if (tt.WhitespacesAfterCount > 15) 
                    {
                        if (!isTab) 
                            break;
                    }
                }
            }
            if (cellInfo == null) 
                return null;
            TableRowToken res = new TableRowToken(t0, tt);
            res.Cells.Add(new TableCellToken(t, tt) { RowSpan = cellInfo.RowSpan, ColSpan = cellInfo.ColSpan });
            for (tt = tt.Next; tt != null && ((tt.EndChar <= maxChar || maxChar == 0)); tt = tt.Next) 
            {
                t0 = tt;
                cellInfo = null;
                for (; tt != null && ((tt.EndChar <= maxChar || maxChar == 0)); tt = tt.Next) 
                {
                    if (tt.IsTableControlChar) 
                    {
                        cellInfo = new TableInfo(tt);
                        break;
                    }
                    else if (tt.IsNewlineAfter) 
                    {
                        if (!isTab && prev == null) 
                            break;
                        if ((tt.EndChar - t0.BeginChar) > 400) 
                        {
                            if ((tt.EndChar - t0.BeginChar) > 20000) 
                                break;
                            if (!isTab) 
                                break;
                        }
                        if (tt.WhitespacesAfterCount > 15) 
                        {
                            if (!isTab) 
                                break;
                        }
                    }
                }
                if (cellInfo == null) 
                    break;
                if (cellInfo.Typ == TableTypes.RowEnd) 
                {
                    if (tt != t0) 
                        res.Cells.Add(new TableCellToken(t0, tt) { RowSpan = cellInfo.RowSpan, ColSpan = cellInfo.ColSpan });
                    res.EndToken = tt;
                    res.Eor = true;
                    break;
                }
                if (cellInfo.Typ != TableTypes.CellEnd) 
                    break;
                res.Cells.Add(new TableCellToken(t0, tt) { RowSpan = cellInfo.RowSpan, ColSpan = cellInfo.ColSpan });
                res.EndToken = tt;
            }
            if ((res.Cells.Count < 2) && !res.Eor) 
                return null;
            if (res.EndToken.Next != null && res.EndToken.Next.IsChar((char)0x1F)) 
            {
                res.LastRow = true;
                res.EndToken = res.EndToken.Next;
            }
            return res;
        }
        enum TableTypes : int
        {
            Undefined = 0,
            TableStart,
            TableEnd,
            RowEnd,
            CellEnd,
        }

        class TableInfo
        {
            public int ColSpan = 0;
            public int RowSpan = 0;
            public Pullenti.Ner.Core.TableHelper.TableTypes Typ = Pullenti.Ner.Core.TableHelper.TableTypes.Undefined;
            public Pullenti.Ner.Token Src;
            public override string ToString()
            {
                return string.Format("{0} ({1}-{2})", Typ, ColSpan, RowSpan);
            }
            public TableInfo(Pullenti.Ner.Token t)
            {
                Src = t;
                if (t == null) 
                    return;
                if (t.IsChar((char)0x1E)) 
                {
                    Typ = Pullenti.Ner.Core.TableHelper.TableTypes.TableStart;
                    return;
                }
                if (t.IsChar((char)0x1F)) 
                {
                    Typ = Pullenti.Ner.Core.TableHelper.TableTypes.TableEnd;
                    return;
                }
                if (!t.IsChar((char)7)) 
                    return;
                string txt = t.Kit.Sofa.Text;
                Typ = Pullenti.Ner.Core.TableHelper.TableTypes.CellEnd;
                int p = t.BeginChar - 1;
                if (p < 0) 
                    return;
                if (txt[p] == 0xD || txt[p] == 0xA) 
                {
                    Typ = Pullenti.Ner.Core.TableHelper.TableTypes.RowEnd;
                    return;
                }
                ColSpan = (RowSpan = 1);
                for (; p >= 0; p--) 
                {
                    if (!char.IsWhiteSpace(txt[p])) 
                        break;
                    else if (txt[p] == '\t') 
                        ColSpan++;
                    else if (txt[p] == '\f') 
                        RowSpan++;
                }
            }
        }

        public static bool IsCellEnd(Pullenti.Ner.Token t)
        {
            if (t != null && t.IsChar((char)7)) 
                return true;
            return false;
        }
        public static bool IsRowEnd(Pullenti.Ner.Token t)
        {
            if (t == null || !t.IsChar((char)7)) 
                return false;
            TableInfo ti = new TableInfo(t);
            return ti.Typ == TableTypes.RowEnd;
        }
    }
}