/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Phone.Internal
{
    static class PhoneHelper
    {
        public static void Initialize()
        {
            if (m_PhoneRoot != null) 
                return;
            m_PhoneRoot = new PhoneNode();
            m_AllCountryCodes = new Dictionary<string, string>();
            string str = Pullenti.Ner.Bank.Internal.ResourceHelper.GetString("CountryPhoneCodes.txt");
            if (str == null) 
                throw new Exception(string.Format("Can't file resource file {0} in Organization analyzer", "CountryPhoneCodes.txt"));
            foreach (string line0 in str.Split('\n')) 
            {
                string line = line0.Trim();
                if (string.IsNullOrEmpty(line)) 
                    continue;
                if (line.Length < 2) 
                    continue;
                string country = line.Substring(0, 2);
                string cod = line.Substring(2).Trim();
                if (cod.Length < 1) 
                    continue;
                if (!m_AllCountryCodes.ContainsKey(country)) 
                    m_AllCountryCodes.Add(country, cod);
                PhoneNode tn = m_PhoneRoot;
                for (int i = 0; i < cod.Length; i++) 
                {
                    char dig = cod[i];
                    PhoneNode nn;
                    if (!tn.Children.TryGetValue(dig, out nn)) 
                    {
                        nn = new PhoneNode();
                        nn.Pref = cod.Substring(0, i + 1);
                        tn.Children.Add(dig, nn);
                    }
                    tn = nn;
                }
                if (tn.Countries == null) 
                    tn.Countries = new List<string>();
                tn.Countries.Add(country);
            }
        }
        static Dictionary<string, string> m_AllCountryCodes;
        public static Dictionary<string, string> GetAllCountryCodes()
        {
            return m_AllCountryCodes;
        }
        class PhoneNode
        {
            public string Pref;
            public Dictionary<char, PhoneNode> Children = new Dictionary<char, PhoneNode>();
            public List<string> Countries;
            public override string ToString()
            {
                if (Countries == null) 
                    return Pref;
                StringBuilder res = new StringBuilder(Pref);
                foreach (string c in Countries) 
                {
                    res.AppendFormat(" {0}", c);
                }
                return res.ToString();
            }
        }

        static PhoneNode m_PhoneRoot;
        /// <summary>
        /// Выделить телефонный префикс из "полного" номера
        /// </summary>
        public static string GetCountryPrefix(string fullNumber)
        {
            if (fullNumber == null) 
                return null;
            PhoneNode nod = m_PhoneRoot;
            int maxInd = -1;
            for (int i = 0; i < fullNumber.Length; i++) 
            {
                char dig = fullNumber[i];
                PhoneNode nn;
                if (!nod.Children.TryGetValue(dig, out nn)) 
                    break;
                if (nn.Countries != null && nn.Countries.Count > 0) 
                    maxInd = i;
                nod = nn;
            }
            if (maxInd < 0) 
                return null;
            else 
                return fullNumber.Substring(0, maxInd + 1);
        }
    }
}