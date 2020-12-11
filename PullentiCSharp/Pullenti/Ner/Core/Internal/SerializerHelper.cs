/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.IO;
using System.Text;

namespace Pullenti.Ner.Core.Internal
{
    static class SerializerHelper
    {
        public static void SerializeInt(Stream stream, int val)
        {
            stream.Write(BitConverter.GetBytes(val), 0, 4);
        }
        public static int DeserializeInt(Stream stream)
        {
            byte[] buf = new byte[(int)4];
            stream.Read(buf, 0, 4);
            return BitConverter.ToInt32(buf, 0);
        }
        public static void SerializeShort(Stream stream, short val)
        {
            stream.Write(BitConverter.GetBytes(val), 0, 2);
        }
        public static short DeserializeShort(Stream stream)
        {
            byte[] buf = new byte[(int)2];
            stream.Read(buf, 0, 2);
            return BitConverter.ToInt16(buf, 0);
        }
        public static void SerializeString(Stream stream, string val)
        {
            if (val == null) 
            {
                SerializeInt(stream, -1);
                return;
            }
            if (string.IsNullOrEmpty(val)) 
            {
                SerializeInt(stream, 0);
                return;
            }
            byte[] data = Encoding.UTF8.GetBytes(val);
            SerializeInt(stream, data.Length);
            stream.Write(data, 0, data.Length);
        }
        public static string DeserializeString(Stream stream)
        {
            int len = DeserializeInt(stream);
            if (len < 0) 
                return null;
            if (len == 0) 
                return "";
            byte[] data = new byte[(int)len];
            stream.Read(data, 0, data.Length);
            return Encoding.UTF8.GetString(data);
        }
        public static void SerializeTokens(Stream stream, Pullenti.Ner.Token t, int maxChar)
        {
            int cou = 0;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (maxChar > 0 && tt.EndChar > maxChar) 
                    break;
                cou++;
            }
            SerializeInt(stream, cou);
            for (; cou > 0; cou--,t = t.Next) 
            {
                SerializeToken(stream, t);
            }
        }
        public static Pullenti.Ner.Token DeserializeTokens(Stream stream, Pullenti.Ner.Core.AnalysisKit kit, int vers)
        {
            int cou = DeserializeInt(stream);
            if (cou == 0) 
                return null;
            Pullenti.Ner.Token res = null;
            Pullenti.Ner.Token prev = null;
            for (; cou > 0; cou--) 
            {
                Pullenti.Ner.Token t = DeserializeToken(stream, kit, vers);
                if (t == null) 
                    continue;
                if (res == null) 
                    res = t;
                if (prev != null) 
                    t.Previous = prev;
                prev = t;
            }
            for (Pullenti.Ner.Token t = res; t != null; t = t.Next) 
            {
                if (t is Pullenti.Ner.MetaToken) 
                    _corrPrevNext(t as Pullenti.Ner.MetaToken, t.Previous, t.Next);
            }
            return res;
        }
        static void _corrPrevNext(Pullenti.Ner.MetaToken mt, Pullenti.Ner.Token prev, Pullenti.Ner.Token next)
        {
            mt.BeginToken.m_Previous = prev;
            mt.EndToken.m_Next = next;
            for (Pullenti.Ner.Token t = mt.BeginToken; t != null && t.EndChar <= mt.EndChar; t = t.Next) 
            {
                if (t is Pullenti.Ner.MetaToken) 
                    _corrPrevNext(t as Pullenti.Ner.MetaToken, t.Previous, t.Next);
            }
        }
        public static void SerializeToken(Stream stream, Pullenti.Ner.Token t)
        {
            short typ = (short)0;
            if (t is Pullenti.Ner.TextToken) 
                typ = 1;
            else if (t is Pullenti.Ner.NumberToken) 
                typ = 2;
            else if (t is Pullenti.Ner.ReferentToken) 
                typ = 3;
            else if (t is Pullenti.Ner.MetaToken) 
                typ = 4;
            SerializeShort(stream, typ);
            if (typ == 0) 
                return;
            t.Serialize(stream);
            if (t is Pullenti.Ner.MetaToken) 
                SerializeTokens(stream, (t as Pullenti.Ner.MetaToken).BeginToken, t.EndChar);
        }
        static Pullenti.Ner.Token DeserializeToken(Stream stream, Pullenti.Ner.Core.AnalysisKit kit, int vers)
        {
            short typ = DeserializeShort(stream);
            if (typ == 0) 
                return null;
            Pullenti.Ner.Token t = null;
            if (typ == 1) 
                t = new Pullenti.Ner.TextToken(null, kit);
            else if (typ == 2) 
                t = new Pullenti.Ner.NumberToken(null, null, null, Pullenti.Ner.NumberSpellingType.Digit, kit);
            else if (typ == 3) 
                t = new Pullenti.Ner.ReferentToken(null, null, null, kit);
            else 
                t = new Pullenti.Ner.MetaToken(null, null, kit);
            t.Deserialize(stream, kit, vers);
            if (t is Pullenti.Ner.MetaToken) 
            {
                Pullenti.Ner.Token tt = DeserializeTokens(stream, kit, vers);
                if (tt != null) 
                {
                    (t as Pullenti.Ner.MetaToken).m_BeginToken = tt;
                    for (; tt != null; tt = tt.Next) 
                    {
                        (t as Pullenti.Ner.MetaToken).m_EndToken = tt;
                    }
                }
            }
            return t;
        }
    }
}