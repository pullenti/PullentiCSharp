/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Pullenti.Ner.Org.Internal
{
    public class OrgItemTypeTermin : Pullenti.Ner.Core.Termin
    {
        public OrgItemTypeTermin(string s, Pullenti.Morph.MorphLang lang = null, Pullenti.Ner.Org.OrgProfile p1 = Pullenti.Ner.Org.OrgProfile.Undefined, Pullenti.Ner.Org.OrgProfile p2 = Pullenti.Ner.Org.OrgProfile.Undefined) : base(s, lang, false)
        {
            if (p1 != Pullenti.Ner.Org.OrgProfile.Undefined) 
                Profiles.Add(p1);
            if (p2 != Pullenti.Ner.Org.OrgProfile.Undefined) 
                Profiles.Add(p2);
        }
        public OrgItemTypeTyp Typ
        {
            get
            {
                if (IsPurePrefix) 
                    return OrgItemTypeTyp.Prefix;
                return m_Typ;
            }
            set
            {
                if (value == OrgItemTypeTyp.Prefix) 
                {
                    IsPurePrefix = true;
                    m_Typ = OrgItemTypeTyp.Org;
                }
                else 
                {
                    m_Typ = value;
                    if (m_Typ == OrgItemTypeTyp.Dep || m_Typ == OrgItemTypeTyp.DepAdd) 
                    {
                        if (!Profiles.Contains(Pullenti.Ner.Org.OrgProfile.Unit)) 
                            Profiles.Add(Pullenti.Ner.Org.OrgProfile.Unit);
                    }
                }
            }
        }
        OrgItemTypeTyp m_Typ;
        public bool MustBePartofName = false;
        public bool IsPurePrefix;
        public bool CanBeNormalDep;
        public bool CanHasNumber;
        public bool CanHasSingleName;
        public bool CanHasLatinName;
        public bool MustHasCapitalName;
        public bool IsTop;
        public bool CanBeSingleGeo;
        public bool IsDoubtWord;
        public float Coeff;
        public List<Pullenti.Ner.Org.OrgProfile> Profiles = new List<Pullenti.Ner.Org.OrgProfile>();
        internal Pullenti.Ner.Org.OrgProfile Profile
        {
            get
            {
                return Pullenti.Ner.Org.OrgProfile.Undefined;
            }
            set
            {
                Profiles.Add(value);
            }
        }
        void CopyFrom(OrgItemTypeTermin it)
        {
            Profiles.AddRange(it.Profiles);
            IsPurePrefix = it.IsPurePrefix;
            CanBeNormalDep = it.CanBeNormalDep;
            CanHasNumber = it.CanHasNumber;
            CanHasSingleName = it.CanHasSingleName;
            CanHasLatinName = it.CanHasLatinName;
            MustBePartofName = it.MustBePartofName;
            MustHasCapitalName = it.MustHasCapitalName;
            IsTop = it.IsTop;
            CanBeNormalDep = it.CanBeNormalDep;
            CanBeSingleGeo = it.CanBeSingleGeo;
            IsDoubtWord = it.IsDoubtWord;
            Coeff = it.Coeff;
        }
        public static List<OrgItemTypeTermin> DeserializeSrc(XmlNode xml, OrgItemTypeTermin set)
        {
            List<OrgItemTypeTermin> res = new List<OrgItemTypeTermin>();
            bool isSet = xml.LocalName == "set";
            if (isSet) 
                res.Add((set = new OrgItemTypeTermin(null)));
            if (xml.Attributes == null) 
                return res;
            foreach (XmlAttribute a in xml.Attributes) 
            {
                string nam = a.LocalName;
                if (!nam.StartsWith("name")) 
                    continue;
                Pullenti.Morph.MorphLang lang = Pullenti.Morph.MorphLang.RU;
                if (nam == "nameUa") 
                    lang = Pullenti.Morph.MorphLang.UA;
                else if (nam == "nameEn") 
                    lang = Pullenti.Morph.MorphLang.EN;
                OrgItemTypeTermin it = null;
                foreach (string s in a.Value.Split(';')) 
                {
                    if (!string.IsNullOrEmpty(s)) 
                    {
                        if (it == null) 
                        {
                            res.Add((it = new OrgItemTypeTermin(s, lang)));
                            if (set != null) 
                                it.CopyFrom(set);
                        }
                        else 
                            it.AddVariant(s, false);
                    }
                }
            }
            foreach (XmlAttribute a in xml.Attributes) 
            {
                string nam = a.LocalName;
                if (nam.StartsWith("name")) 
                    continue;
                if (nam.StartsWith("abbr")) 
                {
                    Pullenti.Morph.MorphLang lang = Pullenti.Morph.MorphLang.RU;
                    if (nam == "abbrUa") 
                        lang = Pullenti.Morph.MorphLang.UA;
                    else if (nam == "abbrEn") 
                        lang = Pullenti.Morph.MorphLang.EN;
                    foreach (OrgItemTypeTermin r in res) 
                    {
                        if (r.Lang == lang) 
                            r.Acronym = a.Value;
                    }
                    continue;
                }
                if (nam == "profile") 
                {
                    List<Pullenti.Ner.Org.OrgProfile> li = new List<Pullenti.Ner.Org.OrgProfile>();
                    foreach (string s in a.Value.Split(';')) 
                    {
                        try 
                        {
                            Pullenti.Ner.Org.OrgProfile p = (Pullenti.Ner.Org.OrgProfile)Enum.Parse(typeof(Pullenti.Ner.Org.OrgProfile), s, true);
                            if (p != Pullenti.Ner.Org.OrgProfile.Undefined) 
                                li.Add(p);
                        }
                        catch(Exception ex) 
                        {
                        }
                    }
                    foreach (OrgItemTypeTermin r in res) 
                    {
                        r.Profiles = li;
                    }
                    continue;
                }
                if (nam == "coef") 
                {
                    float v = float.Parse(a.Value);
                    foreach (OrgItemTypeTermin r in res) 
                    {
                        r.Coeff = v;
                    }
                    continue;
                }
                if (nam == "partofname") 
                {
                    foreach (OrgItemTypeTermin r in res) 
                    {
                        r.MustBePartofName = a.Value == "true";
                    }
                    continue;
                }
                if (nam == "top") 
                {
                    foreach (OrgItemTypeTermin r in res) 
                    {
                        r.IsTop = a.Value == "true";
                    }
                    continue;
                }
                if (nam == "geo") 
                {
                    foreach (OrgItemTypeTermin r in res) 
                    {
                        r.CanBeSingleGeo = a.Value == "true";
                    }
                    continue;
                }
                if (nam == "purepref") 
                {
                    foreach (OrgItemTypeTermin r in res) 
                    {
                        r.IsPurePrefix = a.Value == "true";
                    }
                    continue;
                }
                if (nam == "number") 
                {
                    foreach (OrgItemTypeTermin r in res) 
                    {
                        r.CanHasNumber = a.Value == "true";
                    }
                    continue;
                }
                throw new Exception("Unknown Org Type Tag: " + a.Name);
            }
            return res;
        }
    }
}