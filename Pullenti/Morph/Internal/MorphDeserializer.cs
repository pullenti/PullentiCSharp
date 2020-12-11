/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.IO;
using System.IO.Compression;

namespace Pullenti.Morph.Internal
{
    public static class MorphDeserializer
    {
        public static void DeflateGzip(Stream str, Stream res)
        {
            using (GZipStream deflate = new GZipStream(str, CompressionMode.Decompress)) 
            {
                byte[] buf = new byte[(int)100000];
                int len = buf.Length;
                while (true) 
                {
                    int i = -1;
                    try 
                    {
                        for (int ii = 0; ii < len; ii++) 
                        {
                            buf[ii] = 0;
                        }
                        i = deflate.Read(buf, 0, len);
                    }
                    catch(Exception ex) 
                    {
                        for (i = len - 1; i >= 0; i--) 
                        {
                            if (buf[i] != 0) 
                            {
                                res.Write(buf, 0, i + 1);
                                break;
                            }
                        }
                        break;
                    }
                    if (i < 1) 
                        break;
                    res.Write(buf, 0, i);
                }
            }
        }
    }
}