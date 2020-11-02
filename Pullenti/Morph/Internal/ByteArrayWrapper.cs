/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Morph.Internal
{
    // Сделан специально для Питона - а то стандартым способом через Memory Stream
    // жутко тормозит, придётся делать самим
    public class ByteArrayWrapper
    {
        public byte[] m_Array;
        int m_Len;
        public ByteArrayWrapper(byte[] arr)
        {
            m_Array = arr;
            m_Len = m_Array.Length;
        }
        public bool IsEOF(int pos)
        {
            return pos >= m_Len;
        }
        public byte DeserializeByte(ref int pos)
        {
            if (pos >= m_Len) 
                return 0;
            return m_Array[pos++];
        }
        public int DeserializeShort(ref int pos)
        {
            if ((pos + 1) >= m_Len) 
                return 0;
            byte b0 = m_Array[pos++];
            byte b1 = m_Array[pos++];
            int res = (int)b1;
            res <<= 8;
            return (res | b0);
        }
        public int DeserializeInt(ref int pos)
        {
            if ((pos + 1) >= m_Len) 
                return 0;
            byte b0 = m_Array[pos++];
            byte b1 = m_Array[pos++];
            byte b2 = m_Array[pos++];
            byte b3 = m_Array[pos++];
            int res = (int)b3;
            res <<= 8;
            res |= b2;
            res <<= 8;
            res |= b1;
            res <<= 8;
            return (res | b0);
        }
        public string DeserializeString(ref int pos)
        {
            if (pos >= m_Len) 
                return null;
            byte len = m_Array[pos++];
            if (len == 0xFF) 
                return null;
            if (len == 0) 
                return "";
            if ((pos + len) > m_Len) 
                return null;
            string res = Encoding.UTF8.GetString(m_Array, pos, len);
            pos += len;
            return res;
        }
    }
}