/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Pullenti.Ner.Address.Internal
{
    /// <summary>
    /// Это для поддержки получения встроенных ресурсов
    /// </summary>
    static class ResourceHelper
    {
        /// <summary>
        /// Получить встроенный ресурс
        /// </summary>
        /// <param name="name">имя, на который оканчивается ресурс</param>
        public static byte[] GetBytes(string name)
        {
            Assembly assembly = typeof(ResourceHelper).Assembly;
            string[] names = assembly.GetManifestResourceNames();
            foreach (string n in names) 
            {
                if (n.EndsWith(name, StringComparison.OrdinalIgnoreCase)) 
                {
                    if (name.Length < n.Length) 
                    {
                        if (n[n.Length - name.Length - 1] != '.') 
                            continue;
                    }
                    try 
                    {
                        object inf = assembly.GetManifestResourceInfo(n);
                        if (inf == null) 
                            continue;
                        using (Stream stream = assembly.GetManifestResourceStream(n)) 
                        {
                            byte[] buf = new byte[(int)stream.Length];
                            stream.Read(buf, 0, buf.Length);
                            return buf;
                        }
                    }
                    catch(Exception ex) 
                    {
                    }
                }
            }
            return null;
        }
        public static string GetString(string name)
        {
            byte[] arr = GetBytes(name);
            if (arr == null) 
                return null;
            if ((arr.Length > 3 && arr[0] == 0xEF && arr[1] == 0xBB) && arr[2] == 0xBF) 
                return Encoding.UTF8.GetString(arr, 3, arr.Length - 3);
            else 
                return Encoding.UTF8.GetString(arr);
        }
    }
}