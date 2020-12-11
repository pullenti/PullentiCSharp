/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Core.Internal
{
    public class RusLatAccord
    {
        RusLatAccord(string ru, string la, bool brus = true, bool blat = true)
        {
            Rus = ru.ToUpper();
            Lat = la.ToUpper();
            RusToLat = brus;
            LatToRus = blat;
        }
        public string Rus;
        public string Lat;
        public bool RusToLat;
        public bool LatToRus;
        public bool OnTail = false;
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            tmp.AppendFormat("'{0}'", Rus);
            if (RusToLat && LatToRus) 
                tmp.Append(" <-> ");
            else if (RusToLat) 
                tmp.Append(" -> ");
            else if (LatToRus) 
                tmp.Append(" <- ");
            tmp.AppendFormat("'{0}'", Lat);
            return tmp.ToString();
        }
        static List<RusLatAccord> m_Accords;
        static List<RusLatAccord> Accords
        {
            get
            {
                if (m_Accords != null) 
                    return m_Accords;
                m_Accords = new List<RusLatAccord>();
                m_Accords.Add(new RusLatAccord("а", "a"));
                m_Accords.Add(new RusLatAccord("а", "aa"));
                m_Accords.Add(new RusLatAccord("б", "b"));
                m_Accords.Add(new RusLatAccord("в", "v"));
                m_Accords.Add(new RusLatAccord("в", "w"));
                m_Accords.Add(new RusLatAccord("г", "g"));
                m_Accords.Add(new RusLatAccord("д", "d"));
                m_Accords.Add(new RusLatAccord("е", "e"));
                m_Accords.Add(new RusLatAccord("е", "yo"));
                m_Accords.Add(new RusLatAccord("е", "io"));
                m_Accords.Add(new RusLatAccord("е", "jo"));
                m_Accords.Add(new RusLatAccord("ж", "j"));
                m_Accords.Add(new RusLatAccord("дж", "j"));
                m_Accords.Add(new RusLatAccord("з", "z"));
                m_Accords.Add(new RusLatAccord("и", "e"));
                m_Accords.Add(new RusLatAccord("и", "i"));
                m_Accords.Add(new RusLatAccord("и", "y"));
                m_Accords.Add(new RusLatAccord("и", "ea"));
                m_Accords.Add(new RusLatAccord("й", "i"));
                m_Accords.Add(new RusLatAccord("й", "y"));
                m_Accords.Add(new RusLatAccord("к", "c"));
                m_Accords.Add(new RusLatAccord("к", "k"));
                m_Accords.Add(new RusLatAccord("к", "ck"));
                m_Accords.Add(new RusLatAccord("кс", "x"));
                m_Accords.Add(new RusLatAccord("л", "l"));
                m_Accords.Add(new RusLatAccord("м", "m"));
                m_Accords.Add(new RusLatAccord("н", "n"));
                m_Accords.Add(new RusLatAccord("о", "a"));
                m_Accords.Add(new RusLatAccord("о", "o"));
                m_Accords.Add(new RusLatAccord("о", "ow"));
                m_Accords.Add(new RusLatAccord("о", "oh"));
                m_Accords.Add(new RusLatAccord("п", "p"));
                m_Accords.Add(new RusLatAccord("р", "r"));
                m_Accords.Add(new RusLatAccord("с", "s"));
                m_Accords.Add(new RusLatAccord("с", "c"));
                m_Accords.Add(new RusLatAccord("т", "t"));
                m_Accords.Add(new RusLatAccord("у", "u"));
                m_Accords.Add(new RusLatAccord("у", "w"));
                m_Accords.Add(new RusLatAccord("ф", "f"));
                m_Accords.Add(new RusLatAccord("ф", "ph"));
                m_Accords.Add(new RusLatAccord("х", "h"));
                m_Accords.Add(new RusLatAccord("х", "kh"));
                m_Accords.Add(new RusLatAccord("ц", "ts"));
                m_Accords.Add(new RusLatAccord("ц", "c"));
                m_Accords.Add(new RusLatAccord("ч", "ch"));
                m_Accords.Add(new RusLatAccord("ш", "sh"));
                m_Accords.Add(new RusLatAccord("щ", "shch"));
                m_Accords.Add(new RusLatAccord("ы", "i"));
                m_Accords.Add(new RusLatAccord("э", "e"));
                m_Accords.Add(new RusLatAccord("э", "a"));
                m_Accords.Add(new RusLatAccord("ю", "iu"));
                m_Accords.Add(new RusLatAccord("ю", "ju"));
                m_Accords.Add(new RusLatAccord("ю", "yu"));
                m_Accords.Add(new RusLatAccord("ю", "ew"));
                m_Accords.Add(new RusLatAccord("я", "ia"));
                m_Accords.Add(new RusLatAccord("я", "ja"));
                m_Accords.Add(new RusLatAccord("я", "ya"));
                m_Accords.Add(new RusLatAccord("ъ", ""));
                m_Accords.Add(new RusLatAccord("ь", ""));
                m_Accords.Add(new RusLatAccord("", "gh"));
                m_Accords.Add(new RusLatAccord("", "h"));
                m_Accords.Add(new RusLatAccord("", "e") { OnTail = true });
                m_Accords.Add(new RusLatAccord("еи", "ei"));
                m_Accords.Add(new RusLatAccord("аи", "ai"));
                m_Accords.Add(new RusLatAccord("ай", "i"));
                m_Accords.Add(new RusLatAccord("уи", "ui"));
                m_Accords.Add(new RusLatAccord("уи", "w"));
                m_Accords.Add(new RusLatAccord("ои", "oi"));
                m_Accords.Add(new RusLatAccord("ей", "ei"));
                m_Accords.Add(new RusLatAccord("ей", "ey"));
                m_Accords.Add(new RusLatAccord("ай", "ai"));
                m_Accords.Add(new RusLatAccord("ай", "ay"));
                m_Accords.Add(new RusLatAccord(" ", " "));
                m_Accords.Add(new RusLatAccord("-", "-"));
                return m_Accords;
            }
        }
        static bool _isPref(string str, int i, string pref)
        {
            if ((pref.Length + i) > str.Length) 
                return false;
            for (int j = 0; j < pref.Length; j++) 
            {
                if (pref[j] != str[i + j]) 
                    return false;
            }
            return true;
        }
        static List<RusLatAccord> _getVarsPref(string rus, int ri, string lat, int li)
        {
            List<RusLatAccord> res = null;
            foreach (RusLatAccord a in Accords) 
            {
                if (_isPref(rus, ri, a.Rus) && _isPref(lat, li, a.Lat) && a.RusToLat) 
                {
                    if (a.OnTail) 
                    {
                        if ((ri + a.Rus.Length) < rus.Length) 
                            continue;
                        if ((li + a.Lat.Length) < lat.Length) 
                            continue;
                    }
                    if (res == null) 
                        res = new List<RusLatAccord>();
                    res.Add(a);
                }
            }
            return res;
        }
        public static List<string> GetVariants(string rusOrLat)
        {
            List<string> res = new List<string>();
            if (string.IsNullOrEmpty(rusOrLat)) 
                return res;
            rusOrLat = rusOrLat.ToUpper();
            bool isRus = Pullenti.Morph.LanguageHelper.IsCyrillicChar(rusOrLat[0]);
            List<List<RusLatAccord>> stack = new List<List<RusLatAccord>>();
            int i;
            for (i = 0; i < rusOrLat.Length; i++) 
            {
                List<RusLatAccord> li = new List<RusLatAccord>();
                int maxlen = 0;
                foreach (RusLatAccord a in Accords) 
                {
                    string pref = null;
                    if (isRus && a.Rus.Length > 0) 
                        pref = a.Rus;
                    else if (!isRus && a.Lat.Length > 0) 
                        pref = a.Lat;
                    else 
                        continue;
                    if (pref.Length < maxlen) 
                        continue;
                    if (!_isPref(rusOrLat, i, pref)) 
                        continue;
                    if (a.OnTail) 
                    {
                        if ((pref.Length + i) < rusOrLat.Length) 
                            continue;
                    }
                    if (pref.Length > maxlen) 
                    {
                        maxlen = pref.Length;
                        li.Clear();
                    }
                    li.Add(a);
                }
                if (li.Count == 0 || maxlen == 0) 
                    return res;
                stack.Add(li);
                i += (maxlen - 1);
            }
            if (stack.Count == 0) 
                return res;
            List<int> ind = new List<int>();
            for (i = 0; i < stack.Count; i++) 
            {
                ind.Add(0);
            }
            StringBuilder tmp = new StringBuilder();
            while (true) 
            {
                tmp.Length = 0;
                for (i = 0; i < ind.Count; i++) 
                {
                    RusLatAccord a = stack[i][ind[i]];
                    tmp.Append((isRus ? a.Lat : a.Rus));
                }
                bool ok = true;
                if (!isRus) 
                {
                    for (i = 0; i < tmp.Length; i++) 
                    {
                        if (tmp[i] == 'Й') 
                        {
                            if (i == 0) 
                            {
                                ok = false;
                                break;
                            }
                            if (!Pullenti.Morph.LanguageHelper.IsCyrillicVowel(tmp[i - 1])) 
                            {
                                ok = false;
                                break;
                            }
                        }
                    }
                }
                if (ok) 
                    res.Add(tmp.ToString());
                for (i = ind.Count - 1; i >= 0; i--) 
                {
                    if ((++ind[i]) < stack[i].Count) 
                        break;
                    else 
                        ind[i] = 0;
                }
                if (i < 0) 
                    break;
            }
            return res;
        }
        public static bool CanBeEquals(string rus, string lat)
        {
            if (string.IsNullOrEmpty(rus) || string.IsNullOrEmpty(lat)) 
                return false;
            rus = rus.ToUpper();
            lat = lat.ToUpper();
            List<RusLatAccord> vs = _getVarsPref(rus, 0, lat, 0);
            if (vs == null) 
                return false;
            List<List<RusLatAccord>> stack = new List<List<RusLatAccord>>();
            stack.Add(vs);
            while (stack.Count > 0) 
            {
                if (stack.Count == 0) 
                    break;
                int ri = 0;
                int li = 0;
                foreach (List<RusLatAccord> s in stack) 
                {
                    ri += s[0].Rus.Length;
                    li += s[0].Lat.Length;
                }
                if (ri >= rus.Length && li >= lat.Length) 
                    return true;
                vs = _getVarsPref(rus, ri, lat, li);
                if (vs != null) 
                {
                    stack.Insert(0, vs);
                    continue;
                }
                while (stack.Count > 0) 
                {
                    stack[0].RemoveAt(0);
                    if (stack[0].Count > 0) 
                        break;
                    stack.RemoveAt(0);
                }
            }
            return false;
        }
        public static int FindAccordsRusToLat(string txt, int pos, List<string> res)
        {
            if (pos >= txt.Length) 
                return 0;
            char ch0 = txt[pos];
            bool ok = false;
            if ((pos + 1) < txt.Length) 
            {
                char ch1 = txt[pos + 1];
                foreach (RusLatAccord a in Accords) 
                {
                    if ((a.RusToLat && a.Rus.Length == 2 && a.Rus[0] == ch0) && a.Rus[1] == ch1) 
                    {
                        res.Add(a.Lat);
                        ok = true;
                    }
                }
                if (ok) 
                    return 2;
            }
            foreach (RusLatAccord a in Accords) 
            {
                if (a.RusToLat && a.Rus.Length == 1 && a.Rus[0] == ch0) 
                {
                    res.Add(a.Lat);
                    ok = true;
                }
            }
            if (ok) 
                return 1;
            return 0;
        }
        public static int FindAccordsLatToRus(string txt, int pos, List<string> res)
        {
            if (pos >= txt.Length) 
                return 0;
            int i;
            int j;
            int maxLen = 0;
            foreach (RusLatAccord a in Accords) 
            {
                if (a.LatToRus && a.Lat.Length >= maxLen) 
                {
                    for (i = 0; i < a.Lat.Length; i++) 
                    {
                        if ((pos + i) >= txt.Length) 
                            break;
                        if (txt[pos + i] != a.Lat[i]) 
                            break;
                    }
                    if ((i < a.Lat.Length) || (a.Lat.Length < 1)) 
                        continue;
                    if (a.Lat.Length > maxLen) 
                    {
                        res.Clear();
                        maxLen = a.Lat.Length;
                    }
                    res.Add(a.Rus);
                }
            }
            return maxLen;
        }
    }
}