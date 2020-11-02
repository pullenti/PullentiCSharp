/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Person.Internal
{
    public static class ShortNameHelper
    {
        static Dictionary<string, List<ShortnameVar>> m_Shorts_Names = new Dictionary<string, List<ShortnameVar>>();
        public class ShortnameVar
        {
            public string Name;
            public Pullenti.Morph.MorphGender Gender;
            public override string ToString()
            {
                return Name;
            }
        }

        public static List<string> GetShortnamesForName(string name)
        {
            List<string> res = new List<string>();
            foreach (KeyValuePair<string, List<ShortnameVar>> kp in m_Shorts_Names) 
            {
                foreach (ShortnameVar v in kp.Value) 
                {
                    if (v.Name == name) 
                    {
                        if (!res.Contains(kp.Key)) 
                            res.Add(kp.Key);
                    }
                }
            }
            return res;
        }
        public static List<ShortnameVar> GetNamesForShortname(string shortname)
        {
            List<ShortnameVar> res;
            if (!m_Shorts_Names.TryGetValue(shortname, out res)) 
                return null;
            else 
                return res;
        }
        static bool m_Inited = false;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            string obj = ResourceHelper.GetString("ShortNames.txt");
            if (obj != null) 
            {
                Pullenti.Ner.Core.AnalysisKit kit = new Pullenti.Ner.Core.AnalysisKit(new Pullenti.Ner.SourceOfAnalysis(obj));
                for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
                {
                    if (t.IsNewlineBefore) 
                    {
                        Pullenti.Morph.MorphGender g = (t.IsValue("F", null) ? Pullenti.Morph.MorphGender.Feminie : Pullenti.Morph.MorphGender.Masculine);
                        t = t.Next;
                        string nam = (t as Pullenti.Ner.TextToken).Term;
                        List<string> shos = new List<string>();
                        for (t = t.Next; t != null; t = t.Next) 
                        {
                            if (t.IsNewlineBefore) 
                                break;
                            else 
                                shos.Add((t as Pullenti.Ner.TextToken).Term);
                        }
                        foreach (string s in shos) 
                        {
                            List<ShortnameVar> li = null;
                            if (!m_Shorts_Names.TryGetValue(s, out li)) 
                                m_Shorts_Names.Add(s, (li = new List<ShortnameVar>()));
                            li.Add(new ShortnameVar() { Name = nam, Gender = g });
                        }
                        if (t == null) 
                            break;
                        t = t.Previous;
                    }
                }
            }
        }
    }
}