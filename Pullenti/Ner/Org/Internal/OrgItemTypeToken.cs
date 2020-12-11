/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Pullenti.Ner.Org.Internal
{
    public class OrgItemTypeToken : Pullenti.Ner.MetaToken
    {
        static Pullenti.Ner.Core.IntOntologyCollection m_Global;
        static OrgItemTypeTermin m_Bank;
        static OrgItemTypeTermin m_MO;
        static OrgItemTypeTermin m_IsprKolon;
        static OrgItemTypeTermin m_SberBank;
        static OrgItemTypeTermin m_SecServ;
        static OrgItemTypeTermin m_AkcionComp;
        static OrgItemTypeTermin m_SovmPred;
        internal static Pullenti.Ner.Core.TerminCollection m_PrefWords;
        internal static Pullenti.Ner.Core.TerminCollection m_KeyWordsForRefs;
        internal static Pullenti.Ner.Core.TerminCollection m_Markers;
        static Pullenti.Ner.Core.TerminCollection m_StdAdjs;
        static Pullenti.Ner.Core.TerminCollection m_StdAdjsUA;
        public static void Initialize()
        {
            if (m_Global != null) 
                return;
            m_Global = new Pullenti.Ner.Core.IntOntologyCollection();
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
            byte[] tdat = ResourceHelper.GetBytes("OrgTypes.dat");
            if (tdat == null) 
                throw new Exception("Can't file resource file OrgTypes.dat in Organization analyzer");
            tdat = Deflate(tdat);
            using (MemoryStream tmp = new MemoryStream(tdat)) 
            {
                tmp.Position = 0;
                XmlDocument xml = new XmlDocument();
                xml.Load(tmp);
                OrgItemTypeTermin set = null;
                foreach (XmlNode x in xml.DocumentElement.ChildNodes) 
                {
                    List<OrgItemTypeTermin> its = OrgItemTypeTermin.DeserializeSrc(x, set);
                    if (x.LocalName == "set") 
                    {
                        set = null;
                        if (its != null && its.Count > 0) 
                            set = its[0];
                    }
                    else if (its != null) 
                    {
                        foreach (OrgItemTypeTermin ii in its) 
                        {
                            m_Global.Add(ii);
                        }
                    }
                }
            }
            OrgItemTypeTermin t;
            string[] sovs = new string[] {"СОВЕТ БЕЗОПАСНОСТИ", "НАЦИОНАЛЬНЫЙ СОВЕТ", "ГОСУДАРСТВЕННЫЙ СОВЕТ", "ОБЛАСТНОЙ СОВЕТ", "РАЙОННЫЙ СОВЕТ", "ГОРОДСКОЙ СОВЕТ", "СЕЛЬСКИЙ СОВЕТ", "КРАЕВОЙ СОВЕТ", "СЛЕДСТВЕННЫЙ КОМИТЕТ", "СЛЕДСТВЕННОЕ УПРАВЛЕНИЕ", "ГОСУДАРСТВЕННОЕ СОБРАНИЕ", "МУНИЦИПАЛЬНОЕ СОБРАНИЕ", "ГОРОДСКОЕ СОБРАНИЕ", "ЗАКОНОДАТЕЛЬНОЕ СОБРАНИЕ", "НАРОДНОЕ СОБРАНИЕ", "ОБЛАСТНАЯ ДУМА", "ГОРОДСКАЯ ДУМА", "КРАЕВАЯ ДУМА", "КАБИНЕТ МИНИСТРОВ"};
            string[] sov2 = new string[] {"СОВБЕЗ", "НАЦСОВЕТ", "ГОССОВЕТ", "ОБЛСОВЕТ", "РАЙСОВЕТ", "ГОРСОВЕТ", "СЕЛЬСОВЕТ", "КРАЙСОВЕТ", null, null, "ГОССОБРАНИЕ", "МУНСОБРАНИЕ", "ГОРСОБРАНИЕ", "ЗАКСОБРАНИЕ", "НАРСОБРАНИЕ", "ОБЛДУМА", "ГОРДУМА", "КРАЙДУМА", "КАБМИН"};
            for (int i = 0; i < sovs.Length; i++) 
            {
                t = new OrgItemTypeTermin(sovs[i], Pullenti.Morph.MorphLang.RU, Pullenti.Ner.Org.OrgProfile.State) { Coeff = 4, Typ = OrgItemTypeTyp.Org, IsTop = true, CanBeSingleGeo = true };
                if (sov2[i] != null) 
                {
                    t.AddVariant(sov2[i], false);
                    if (sov2[i] == "ГОССОВЕТ" || sov2[i] == "НАЦСОВЕТ" || sov2[i] == "ЗАКСОБРАНИЕ") 
                        t.Coeff = 5;
                }
                m_Global.Add(t);
            }
            sovs = new string[] {"РАДА БЕЗПЕКИ", "НАЦІОНАЛЬНА РАДА", "ДЕРЖАВНА РАДА", "ОБЛАСНА РАДА", "РАЙОННА РАДА", "МІСЬКА РАДА", "СІЛЬСЬКА РАДА", "КРАЙОВИЙ РАДА", "СЛІДЧИЙ КОМІТЕТ", "СЛІДЧЕ УПРАВЛІННЯ", "ДЕРЖАВНІ ЗБОРИ", "МУНІЦИПАЛЬНЕ ЗБОРИ", "МІСЬКЕ ЗБОРИ", "ЗАКОНОДАВЧІ ЗБОРИ", "НАРОДНІ ЗБОРИ", "ОБЛАСНА ДУМА", "МІСЬКА ДУМА", "КРАЙОВА ДУМА", "КАБІНЕТ МІНІСТРІВ"};
            sov2 = new string[] {"РАДБЕЗ", null, null, "ОБЛРАДА", "РАЙРАДА", "МІСЬКРАДА", "СІЛЬРАДА", "КРАЙРАДА", null, null, "ДЕРЖЗБОРИ", "МУНЗБОРИ", "ГОРСОБРАНИЕ", "ЗАКЗБОРИ", "НАРСОБРАНИЕ", "ОБЛДУМА", "МІСЬКДУМА", "КРАЙДУМА", "КАБМІН"};
            for (int i = 0; i < sovs.Length; i++) 
            {
                t = new OrgItemTypeTermin(sovs[i], Pullenti.Morph.MorphLang.UA, Pullenti.Ner.Org.OrgProfile.State) { Coeff = 4, Typ = OrgItemTypeTyp.Org, IsTop = true, CanBeSingleGeo = true };
                if (sov2[i] != null) 
                    t.AddVariant(sov2[i], false);
                if (sov2[i] == "ГОССОВЕТ" || sov2[i] == "ЗАКЗБОРИ") 
                    t.Coeff = 5;
                m_Global.Add(t);
            }
            sovs = new string[] {"SECURITY COUNCIL", "NATIONAL COUNCIL", "STATE COUNCIL", "REGIONAL COUNCIL", "DISTRICT COUNCIL", "CITY COUNCIL", "RURAL COUNCIL", "INVESTIGATIVE COMMITTEE", "INVESTIGATION DEPARTMENT", "NATIONAL ASSEMBLY", "MUNICIPAL ASSEMBLY", "URBAN ASSEMBLY", "LEGISLATURE"};
            for (int i = 0; i < sovs.Length; i++) 
            {
                t = new OrgItemTypeTermin(sovs[i], Pullenti.Morph.MorphLang.EN, Pullenti.Ner.Org.OrgProfile.State) { Coeff = 4, Typ = OrgItemTypeTyp.Org, IsTop = true, CanBeSingleGeo = true };
                m_Global.Add(t);
            }
            t = new OrgItemTypeTermin("ГОСУДАРСТВЕННЫЙ КОМИТЕТ") { Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.State, Coeff = 2 };
            t.AddVariant("ГОСКОМИТЕТ", false);
            t.AddVariant("ГОСКОМ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ДЕРЖАВНИЙ КОМІТЕТ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.State, Coeff = 2 };
            t.AddVariant("ДЕРЖКОМІТЕТ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("КРАЕВОЙ КОМИТЕТ ГОСУДАРСТВЕННОЙ СТАТИСТИКИ") { Typ = OrgItemTypeTyp.Dep, Profile = Pullenti.Ner.Org.OrgProfile.State, Coeff = 3, CanBeSingleGeo = true };
            t.AddVariant("КРАЙКОМСТАТ", false);
            t.Profile = Pullenti.Ner.Org.OrgProfile.Unit;
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОБЛАСТНОЙ КОМИТЕТ ГОСУДАРСТВЕННОЙ СТАТИСТИКИ") { Typ = OrgItemTypeTyp.Dep, Profile = Pullenti.Ner.Org.OrgProfile.State, Coeff = 3, CanBeSingleGeo = true };
            t.AddVariant("ОБЛКОМСТАТ", false);
            t.Profile = Pullenti.Ner.Org.OrgProfile.Unit;
            m_Global.Add(t);
            t = new OrgItemTypeTermin("РАЙОННЫЙ КОМИТЕТ ГОСУДАРСТВЕННОЙ СТАТИСТИКИ") { Typ = OrgItemTypeTyp.Dep, Profile = Pullenti.Ner.Org.OrgProfile.State, Coeff = 3, CanBeSingleGeo = true };
            t.AddVariant("РАЙКОМСТАТ", false);
            t.Profile = Pullenti.Ner.Org.OrgProfile.Unit;
            m_Global.Add(t);
            sovs = new string[] {"ЦЕНТРАЛЬНЫЙ КОМИТЕТ", "РАЙОННЫЙ КОМИТЕТ", "ГОРОДСКОЙ КОМИТЕТ", "КРАЕВОЙ КОМИТЕТ", "ОБЛАСТНОЙ КОМИТЕТ", "ПОЛИТИЧЕСКОЕ БЮРО"};
            sov2 = new string[] {"ЦК", "РАЙКОМ", "ГОРКОМ", "КРАЙКОМ", "ОБКОМ", "ПОЛИТБЮРО"};
            for (int i = 0; i < sovs.Length; i++) 
            {
                t = new OrgItemTypeTermin(sovs[i]) { Coeff = 2, Typ = OrgItemTypeTyp.Dep, Profile = Pullenti.Ner.Org.OrgProfile.Unit };
                if (i == 0) 
                {
                    t.Acronym = "ЦК";
                    t.CanBeNormalDep = true;
                }
                else if (sov2[i] != null) 
                    t.AddVariant(sov2[i], false);
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"Standing Committee", "Political Bureau", "Central Committee"}) 
            {
                m_Global.Add(new OrgItemTypeTermin(s.ToUpper()) { Coeff = 3, Typ = OrgItemTypeTyp.Dep, Profile = Pullenti.Ner.Org.OrgProfile.Unit, CanBeNormalDep = true });
            }
            sovs = new string[] {"ЦЕНТРАЛЬНИЙ КОМІТЕТ", "РАЙОННИЙ КОМІТЕТ", "МІСЬКИЙ КОМІТЕТ", "КРАЙОВИЙ КОМІТЕТ", "ОБЛАСНИЙ КОМІТЕТ"};
            for (int i = 0; i < sovs.Length; i++) 
            {
                t = new OrgItemTypeTermin(sovs[i], Pullenti.Morph.MorphLang.UA) { Coeff = 2, Typ = OrgItemTypeTyp.Dep, Profile = Pullenti.Ner.Org.OrgProfile.Unit };
                if (i == 0) 
                {
                    t.Acronym = "ЦК";
                    t.CanBeNormalDep = true;
                }
                else if (sov2[i] != null) 
                    t.AddVariant(sov2[i], false);
                m_Global.Add(t);
            }
            t = new OrgItemTypeTermin("КАЗНАЧЕЙСТВО") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("КАЗНАЧЕЙСТВО") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("TREASURY") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ПОСОЛЬСТВО") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("EMNASSY") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ГОСУДАРСТВЕННЫЙ ДЕПАРТАМЕНТ") { Coeff = 5, Typ = OrgItemTypeTyp.Org, IsTop = true, CanBeSingleGeo = true };
            t.AddVariant("ГОСДЕПАРТАМЕНТ", false);
            t.AddVariant("ГОСДЕП", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("DEPARTMENT OF STATE") { Coeff = 5, Typ = OrgItemTypeTyp.Org, IsTop = true, CanBeSingleGeo = true };
            t.AddVariant("STATE DEPARTMENT", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ДЕРЖАВНИЙ ДЕПАРТАМЕНТ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 5, Typ = OrgItemTypeTyp.Org, IsTop = true, CanBeSingleGeo = true };
            t.AddVariant("ДЕРЖДЕПАРТАМЕНТ", false);
            t.AddVariant("ДЕРЖДЕП", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ДЕПАРТАМЕНТ") { Coeff = 2, Typ = OrgItemTypeTyp.Org });
            t = new OrgItemTypeTermin("DEPARTMENT") { Coeff = 2, Typ = OrgItemTypeTyp.Org };
            t.AddAbridge("DEPT.");
            m_Global.Add(t);
            t = new OrgItemTypeTermin("АГЕНТСТВО") { Coeff = 1, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true };
            t.AddVariant("АГЕНСТВО", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ADGENCY") { Coeff = 1, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true });
            t = new OrgItemTypeTermin("АКАДЕМИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education };
            t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Science);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("АКАДЕМІЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education };
            t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Science);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ACADEMY") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education };
            t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Science);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ГЕНЕРАЛЬНЫЙ ШТАБ") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanBeSingleGeo = true, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
            t.AddVariant("ГЕНЕРАЛЬНИЙ ШТАБ", false);
            t.AddVariant("ГЕНШТАБ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("GENERAL STAFF") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanBeSingleGeo = true, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ФРОНТ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_Global.Add(new OrgItemTypeTermin("ВОЕННЫЙ ОКРУГ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_Global.Add(new OrgItemTypeTermin("ВІЙСЬКОВИЙ ОКРУГ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_Global.Add(new OrgItemTypeTermin("ГРУППА АРМИЙ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_Global.Add(new OrgItemTypeTermin("ГРУПА АРМІЙ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_Global.Add(new OrgItemTypeTermin("АРМИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_Global.Add(new OrgItemTypeTermin("АРМІЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_Global.Add(new OrgItemTypeTermin("ARMY") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_Global.Add(new OrgItemTypeTermin("ГВАРДИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_Global.Add(new OrgItemTypeTermin("ГВАРДІЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_Global.Add(new OrgItemTypeTermin("GUARD") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            m_MilitaryUnit = (t = new OrgItemTypeTermin("ВОЙСКОВАЯ ЧАСТЬ") { Coeff = 3, Acronym = "ВЧ", Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army });
            t.AddAbridge("В.Ч.");
            t.AddVariant("ВОИНСКАЯ ЧАСТЬ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ВІЙСЬКОВА ЧАСТИНА") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true };
            t.AddAbridge("В.Ч.");
            m_Global.Add(t);
            foreach (string s in new string[] {"ДИВИЗИЯ", "ДИВИЗИОН", "ПОЛК", "БАТАЛЬОН", "РОТА", "ВЗВОД", "АВИАДИВИЗИЯ", "АВИАПОЛК", "АРТБРИГАДА", "МОТОМЕХБРИГАДА", "ТАНКОВЫЙ КОРПУС", "ГАРНИЗОН", "ДРУЖИНА"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
                if (s == "ГАРНИЗОН") 
                    t.CanBeSingleGeo = true;
                m_Global.Add(t);
            }
            t = new OrgItemTypeTermin("ПОГРАНИЧНЫЙ ОТРЯД") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
            t.AddVariant("ПОГРАНОТРЯД", false);
            t.AddAbridge("ПОГРАН. ОТРЯД");
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ПОГРАНИЧНЫЙ ПОЛК") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
            t.AddVariant("ПОГРАНПОЛК", false);
            t.AddAbridge("ПОГРАН. ПОЛК");
            m_Global.Add(t);
            foreach (string s in new string[] {"ДИВІЗІЯ", "ДИВІЗІОН", "ПОЛК", "БАТАЛЬЙОН", "РОТА", "ВЗВОД", "АВІАДИВІЗІЯ", "АВІАПОЛК", "ПОГРАНПОЛК", "АРТБРИГАДА", "МОТОМЕХБРИГАДА", "ТАНКОВИЙ КОРПУС", "ГАРНІЗОН", "ДРУЖИНА"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 3, Lang = Pullenti.Morph.MorphLang.UA, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
                if (s == "ГАРНІЗОН") 
                    t.CanBeSingleGeo = true;
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"КОРПУС", "БРИГАДА"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 1, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"КОРПУС", "БРИГАДА"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 1, Lang = Pullenti.Morph.MorphLang.UA, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
                m_Global.Add(t);
            }
            t = new OrgItemTypeTermin("ПРИКОРДОННИЙ ЗАГІН") { Coeff = 3, Lang = Pullenti.Morph.MorphLang.UA, Typ = OrgItemTypeTyp.Dep, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННЫЙ УНИВЕРСИТЕТ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ДЕРЖАВНИЙ УНІВЕРСИТЕТ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("STATE UNIVERSITY") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("УНИВЕРСИТЕТ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("УНІВЕРСИТЕТ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("UNIVERSITY") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanBeSingleGeo = true });
            m_Global.Add(new OrgItemTypeTermin("УЧРЕЖДЕНИЕ") { Coeff = 1, Typ = OrgItemTypeTyp.Org, IsDoubtWord = true });
            m_Global.Add(new OrgItemTypeTermin("УСТАНОВА") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 1, Typ = OrgItemTypeTyp.Org, IsDoubtWord = true });
            m_Global.Add(new OrgItemTypeTermin("INSTITUTION") { Coeff = 1, Typ = OrgItemTypeTyp.Org, IsDoubtWord = true });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ УЧРЕЖДЕНИЕ") { Coeff = 3, Typ = OrgItemTypeTyp.Org });
            m_Global.Add(new OrgItemTypeTermin("ДЕРЖАВНА УСТАНОВА") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org });
            m_Global.Add(new OrgItemTypeTermin("STATE INSTITUTION") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeSingleGeo = true });
            t = new OrgItemTypeTermin("ИНСТИТУТ") { Coeff = 2, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education };
            t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Science);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ІНСТИТУТ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 2, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education };
            t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Science);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("INSTITUTE") { Coeff = 2, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education };
            t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Science);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОТДЕЛ СУДЕБНЫХ ПРИСТАВОВ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ОСП", Profile = Pullenti.Ner.Org.OrgProfile.Unit, CanBeSingleGeo = true, CanHasNumber = true };
            t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Justice);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("МЕЖРАЙОННЫЙ ОТДЕЛ СУДЕБНЫХ ПРИСТАВОВ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МОСП", Profile = Pullenti.Ner.Org.OrgProfile.Unit, CanBeSingleGeo = true, CanHasNumber = true };
            t.AddVariant("МЕЖРАЙОННЫЙ ОСП", false);
            t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Justice);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОТДЕЛ ВНЕВЕДОМСТВЕННОЙ ОХРАНЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ОВО", Profile = Pullenti.Ner.Org.OrgProfile.Unit, CanBeSingleGeo = true, CanHasNumber = true };
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ЛИЦЕЙ") { Coeff = 2, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ЛІЦЕЙ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 2, Profile = Pullenti.Ner.Org.OrgProfile.Education, Typ = OrgItemTypeTyp.Org, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ИНТЕРНАТ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ІНТЕРНАТ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Profile = Pullenti.Ner.Org.OrgProfile.Education, Typ = OrgItemTypeTyp.Org, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("HIGH SCHOOL") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasNumber = true, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("SECONDARY SCHOOL") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasNumber = true, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("MIDDLE SCHOOL") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasNumber = true, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("PUBLIC SCHOOL") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasNumber = true, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("JUNIOR SCHOOL") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasNumber = true, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("GRAMMAR SCHOOL") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasNumber = true, CanHasLatinName = true });
            t = new OrgItemTypeTermin("СРЕДНЯЯ ШКОЛА") { Coeff = 3, Acronym = "СШ", Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanBeSingleGeo = true, CanHasNumber = true };
            t.AddVariant("СРЕДНЯЯ ОБРАЗОВАТЕЛЬНАЯ ШКОЛА", false);
            t.AddAbridge("СОШ");
            t.AddVariant("ОБЩЕОБРАЗОВАТЕЛЬНАЯ ШКОЛА", false);
            t.AddVariant("СРЕДНЯЯ ОБЩЕОБРАЗОВАТЕЛЬНАЯ ШКОЛА", false);
            t.AddVariant("ОСНОВНАЯ ОБЩЕОБРАЗОВАТЕЛЬНАЯ ШКОЛА", false);
            t.AddVariant("ОСНОВНАЯ ОБРАЗОВАТЕЛЬНАЯ ШКОЛА", false);
            t.AddAbridge("ООШ");
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("БИЗНЕС ШКОЛА") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeNormalDep = true, CanBeSingleGeo = true, CanHasSingleName = true, CanHasLatinName = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("БІЗНЕС ШКОЛА") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanBeNormalDep = true, CanBeSingleGeo = true, CanHasSingleName = true, CanHasLatinName = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("СЕРЕДНЯ ШКОЛА") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Profile = Pullenti.Ner.Org.OrgProfile.Education, Typ = OrgItemTypeTyp.Org, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ВЫСШАЯ ШКОЛА") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ВИЩА ШКОЛА") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("НАЧАЛЬНАЯ ШКОЛА") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ПОЧАТКОВА ШКОЛА") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("СЕМИНАРИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("СЕМІНАРІЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ГИМНАЗИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ГІМНАЗІЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            t = new OrgItemTypeTermin("ДЕТСКИЙ САД") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education };
            t.AddVariant("ДЕТСАД", false);
            t.AddAbridge("Д.С.");
            t.AddAbridge("Д/С");
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ДИТЯЧИЙ САДОК") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education };
            t.AddVariant("ДИТСАДОК", false);
            t.AddAbridge("Д.С.");
            t.AddAbridge("Д/З");
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ШКОЛА") { Coeff = 1, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("SCHOOL") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("УЧИЛИЩЕ") { Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("КОЛЛЕДЖ") { Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("COLLEGE") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("ЦЕНТР") { Typ = OrgItemTypeTyp.Org, IsDoubtWord = true });
            m_Global.Add(new OrgItemTypeTermin("НАУЧНЫЙ ЦЕНТР") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУКОВИЙ ЦЕНТР") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("УЧЕБНО ВОСПИТАТЕЛЬНЫЙ КОМПЛЕКС") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "УВК", CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Education, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("БОЛЬНИЦА") { Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ЛІКАРНЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("МОРГ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("МОРГ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ХОСПИС") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ХОСПІС") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            t = new OrgItemTypeTermin("ГОРОДСКАЯ БОЛЬНИЦА") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine };
            t.AddAbridge("ГОР.БОЛЬНИЦА");
            t.AddVariant("ГОРБОЛЬНИЦА", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("МІСЬКА ЛІКАРНЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ГОРОДСКАЯ КЛИНИЧЕСКАЯ БОЛЬНИЦА") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Acronym = "ГКБ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("МІСЬКА КЛІНІЧНА ЛІКАРНЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Acronym = "МКЛ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("КЛАДБИЩЕ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("КЛАДОВИЩЕ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true };
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ПОЛИКЛИНИКА") { Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ПОЛІКЛІНІКА") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ГОСПИТАЛЬ") { Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ГОСПІТАЛЬ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("КЛИНИКА") { Coeff = 1, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("КЛІНІКА") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 1, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            t = new OrgItemTypeTermin("МЕДИКО САНИТАРНАЯ ЧАСТЬ") { Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine };
            t.AddVariant("МЕДСАНЧАСТЬ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("МЕДИКО САНІТАРНА ЧАСТИНА") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine };
            t.AddVariant("МЕДСАНЧАСТИНА", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("МЕДИЦИНСКИЙ ЦЕНТР") { Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, CanHasLatinName = true, CanHasSingleName = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine };
            t.AddVariant("МЕДЦЕНТР", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("МЕДИЧНИЙ ЦЕНТР") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, CanHasLatinName = true, CanHasSingleName = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine };
            t.AddVariant("МЕДЦЕНТР", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("РОДИЛЬНЫЙ ДОМ") { Coeff = 1, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine };
            t.AddVariant("РОДДОМ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ПОЛОГОВИЙ БУДИНОК") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 1, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Medicine };
            m_Global.Add(t);
            m_Global.Add((t = new OrgItemTypeTermin("АЭРОПОРТ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, IsTop = true, CanHasSingleName = true, CanHasLatinName = true, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Transport }));
            m_Global.Add((t = new OrgItemTypeTermin("АЕРОПОРТ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, IsTop = true, CanHasSingleName = true, CanHasLatinName = true, CanBeSingleGeo = true }));
            t = new OrgItemTypeTermin("ТОРГОВЫЙ ПОРТ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, IsTop = true, CanHasSingleName = true, CanHasLatinName = true, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Transport };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("МОРСКОЙ ТОРГОВЫЙ ПОРТ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, IsTop = true, CanHasSingleName = true, CanHasLatinName = true, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Transport };
            m_Global.Add(t);
            foreach (string s in new string[] {"ТЕАТР", "ТЕАТР-СТУДИЯ", "КИНОТЕАТР", "МУЗЕЙ", "ГАЛЕРЕЯ", "КОНЦЕРТНЫЙ ЗАЛ", "ФИЛАРМОНИЯ", "КОНСЕРВАТОРИЯ", "ДОМ КУЛЬТУРЫ", "ДВОРЕЦ КУЛЬТУРЫ", "ДВОРЕЦ ПИОНЕРОВ"}) 
            {
                m_Global.Add(new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true });
            }
            foreach (string s in new string[] {"ТЕАТР", "ТЕАТР-СТУДІЯ", "КІНОТЕАТР", "МУЗЕЙ", "ГАЛЕРЕЯ", "КОНЦЕРТНИЙ ЗАЛ", "ФІЛАРМОНІЯ", "КОНСЕРВАТОРІЯ", "БУДИНОК КУЛЬТУРИ", "ПАЛАЦ КУЛЬТУРИ", "ПАЛАЦ ПІОНЕРІВ"}) 
            {
                m_Global.Add(new OrgItemTypeTermin(s) { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true });
            }
            m_Global.Add(new OrgItemTypeTermin("БИБЛИОТЕКА") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("БІБЛІОТЕКА") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasNumber = true });
            foreach (string s in new string[] {"ЦЕРКОВЬ", "ХРАМ", "СОБОР", "МЕЧЕТЬ", "СИНАГОГА", "МОНАСТЫРЬ", "ЛАВРА", "ПАТРИАРХАТ", "МЕДРЕСЕ", "СЕКТА", "РЕЛИГИОЗНАЯ ГРУППА", "РЕЛИГИОЗНОЕ ОБЪЕДИНЕНИЕ", "РЕЛИГИОЗНАЯ ОРГАНИЗАЦИЯ"}) 
            {
                m_Global.Add(new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, Profile = Pullenti.Ner.Org.OrgProfile.Religion });
            }
            foreach (string s in new string[] {"ЦЕРКВА", "ХРАМ", "СОБОР", "МЕЧЕТЬ", "СИНАГОГА", "МОНАСТИР", "ЛАВРА", "ПАТРІАРХАТ", "МЕДРЕСЕ", "СЕКТА", "РЕЛІГІЙНА ГРУПА", "РЕЛІГІЙНЕ ОБЄДНАННЯ", " РЕЛІГІЙНА ОРГАНІЗАЦІЯ"}) 
            {
                m_Global.Add(new OrgItemTypeTermin(s) { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, Profile = Pullenti.Ner.Org.OrgProfile.Religion });
            }
            foreach (string s in new string[] {"ФЕДЕРАЛЬНАЯ СЛУЖБА", "ГОСУДАРСТВЕННАЯ СЛУЖБА", "ФЕДЕРАЛЬНОЕ УПРАВЛЕНИЕ", "ГОСУДАРСТВЕННЫЙ КОМИТЕТ", "ГОСУДАРСТВЕННАЯ ИНСПЕКЦИЯ"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, MustBePartofName = true };
                m_Global.Add(t);
                t = new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanonicText = s };
                t.Terms.Insert(1, new Pullenti.Ner.Core.Termin.Term(null) { IsPatternAny = true });
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ФЕДЕРАЛЬНА СЛУЖБА", "ДЕРЖАВНА СЛУЖБА", "ФЕДЕРАЛЬНЕ УПРАВЛІННЯ", "ДЕРЖАВНИЙ КОМІТЕТ УКРАЇНИ", "ДЕРЖАВНА ІНСПЕКЦІЯ"}) 
            {
                t = new OrgItemTypeTermin(s) { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, MustBePartofName = true };
                m_Global.Add(t);
                t = new OrgItemTypeTermin(s) { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanonicText = s };
                t.Terms.Insert(1, new Pullenti.Ner.Core.Termin.Term(null) { IsPatternAny = true });
                m_Global.Add(t);
            }
            t = new OrgItemTypeTermin("СЛЕДСТВЕННЫЙ ИЗОЛЯТОР") { Coeff = 5, Typ = OrgItemTypeTyp.Org, CanHasNumber = true };
            t.AddVariant("СИЗО", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("СЛІДЧИЙ ІЗОЛЯТОР") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true };
            t.AddVariant("СІЗО", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("КОЛОНИЯ-ПОСЕЛЕНИЕ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("КОЛОНІЯ-ПОСЕЛЕННЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ТЮРЬМА") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, CanHasLatinName = true, CanHasSingleName = true });
            m_Global.Add(new OrgItemTypeTermin("ВЯЗНИЦЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true, CanHasLatinName = true, CanHasSingleName = true });
            m_Global.Add(new OrgItemTypeTermin("КОЛОНИЯ") { Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("КОЛОНІЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 2, Typ = OrgItemTypeTyp.Org, CanHasNumber = true });
            m_Global.Add((m_IsprKolon = new OrgItemTypeTermin("ИСПРАВИТЕЛЬНАЯ КОЛОНИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "ИК", CanHasNumber = true }));
            m_Global.Add(new OrgItemTypeTermin("ВИПРАВНА КОЛОНІЯ") { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasNumber = true });
            foreach (string s in new string[] {"ПОЛИЦИЯ", "МИЛИЦИЯ"}) 
            {
                t = new OrgItemTypeTermin(s) { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanBeSingleGeo = true, CanHasSingleName = false };
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ПОЛІЦІЯ", "МІЛІЦІЯ"}) 
            {
                t = new OrgItemTypeTermin(s) { Lang = Pullenti.Morph.MorphLang.UA, Typ = OrgItemTypeTyp.Org, Coeff = 3, CanBeSingleGeo = true, CanHasSingleName = false };
                m_Global.Add(t);
            }
            m_Global.Add(new OrgItemTypeTermin("ПАЕВЫЙ ИНВЕСТИЦИОННЫЙ ФОНД") { Coeff = 2, Typ = OrgItemTypeTyp.Org, Acronym = "ПИФ" });
            m_Global.Add(new OrgItemTypeTermin("РОССИЙСКОЕ ИНФОРМАЦИОННОЕ АГЕНТСТВО") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "РИА", Profile = Pullenti.Ner.Org.OrgProfile.Media });
            t = new OrgItemTypeTermin("ИНФОРМАЦИОННОЕ АГЕНТСТВО") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "ИА", Profile = Pullenti.Ner.Org.OrgProfile.Media };
            t.AddVariant("ИНФОРМАГЕНТСТВО", false);
            t.AddVariant("ИНФОРМАГЕНСТВО", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ОТДЕЛ") { Coeff = 1, Typ = OrgItemTypeTyp.Dep, IsDoubtWord = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ВІДДІЛ", Pullenti.Morph.MorphLang.UA) { Coeff = 1, Typ = OrgItemTypeTyp.Dep, IsDoubtWord = true, CanHasNumber = true });
            t = new OrgItemTypeTermin("РАЙОННЫЙ ОТДЕЛ") { Coeff = 2, Acronym = "РО", Typ = OrgItemTypeTyp.Dep, CanHasNumber = true };
            t.AddVariant("РАЙОТДЕЛ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("РАЙОННИЙ ВІДДІЛ", Pullenti.Morph.MorphLang.UA) { Coeff = 2, Acronym = "РВ", Typ = OrgItemTypeTyp.Dep, CanHasNumber = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ЦЕХ") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanHasNumber = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ФАКУЛЬТЕТ") { Coeff = 3, Typ = OrgItemTypeTyp.Dep };
            t.AddAbridge("ФАК.");
            m_Global.Add(t);
            t = new OrgItemTypeTermin("КАФЕДРА") { Coeff = 3, Typ = OrgItemTypeTyp.Dep };
            t.AddAbridge("КАФ.");
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ЛАБОРАТОРИЯ") { Coeff = 1, Typ = OrgItemTypeTyp.Dep };
            t.AddAbridge("ЛАБ.");
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ЛАБОРАТОРІЯ", Pullenti.Morph.MorphLang.UA) { Coeff = 1, Typ = OrgItemTypeTyp.Dep };
            t.AddAbridge("ЛАБ.");
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ПАТРИАРХИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Religion });
            m_Global.Add(new OrgItemTypeTermin("ПАТРІАРХІЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Religion });
            m_Global.Add(new OrgItemTypeTermin("ЕПАРХИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Religion });
            m_Global.Add(new OrgItemTypeTermin("ЄПАРХІЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanBeSingleGeo = true, Profile = Pullenti.Ner.Org.OrgProfile.Religion });
            m_Global.Add(new OrgItemTypeTermin("ПРЕДСТАВИТЕЛЬСТВО") { Typ = OrgItemTypeTyp.DepAdd });
            m_Global.Add(new OrgItemTypeTermin("ПРЕДСТАВНИЦТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.DepAdd });
            t = new OrgItemTypeTermin("ОТДЕЛЕНИЕ") { Typ = OrgItemTypeTyp.DepAdd, IsDoubtWord = true };
            t.AddAbridge("ОТД.");
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ВІДДІЛЕННЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.DepAdd, IsDoubtWord = true };
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ИНСПЕКЦИЯ") { Typ = OrgItemTypeTyp.DepAdd, IsDoubtWord = true });
            m_Global.Add(new OrgItemTypeTermin("ІНСПЕКЦІЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.DepAdd, IsDoubtWord = true });
            m_Global.Add(new OrgItemTypeTermin("ФИЛИАЛ") { Typ = OrgItemTypeTyp.DepAdd });
            m_Global.Add(new OrgItemTypeTermin("ФІЛІЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.DepAdd });
            t = new OrgItemTypeTermin("ОФИС") { Typ = OrgItemTypeTyp.DepAdd, IsDoubtWord = true, CanHasNumber = true };
            t.AddVariant("ОПЕРАЦИОННЫЙ ОФИС", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОФІС", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.DepAdd, IsDoubtWord = true, CanHasNumber = true };
            t.AddVariant("ОПЕРАЦІЙНИЙ ОФІС", false);
            m_Global.Add(t);
            foreach (string s in new string[] {"ОТДЕЛ ПОЛИЦИИ", "ОТДЕЛ МИЛИЦИИ", "ОТДЕЛЕНИЕ ПОЛИЦИИ", "ОТДЕЛЕНИЕ МИЛИЦИИ"}) 
            {
                m_Global.Add(new OrgItemTypeTermin(s) { Typ = OrgItemTypeTyp.Dep, Coeff = 1.5F, CanHasNumber = true, CanHasSingleName = true });
                if (s.StartsWith("ОТДЕЛ ")) 
                {
                    t = new OrgItemTypeTermin("ГОРОДСКОЙ " + s) { Typ = OrgItemTypeTyp.Dep, Coeff = 3F, CanHasNumber = true, CanHasSingleName = true };
                    t.AddVariant("ГОР" + s, false);
                    m_Global.Add(t);
                    t = new OrgItemTypeTermin("РАЙОННЫЙ " + s) { Acronym = "РО", Typ = OrgItemTypeTyp.Dep, Coeff = 3F, CanHasNumber = true, CanHasSingleName = true };
                    m_Global.Add(t);
                }
            }
            foreach (string s in new string[] {"ВІДДІЛ ПОЛІЦІЇ", "ВІДДІЛ МІЛІЦІЇ", "ВІДДІЛЕННЯ ПОЛІЦІЇ", "ВІДДІЛЕННЯ МІЛІЦІЇ"}) 
            {
                m_Global.Add(new OrgItemTypeTermin(s, Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Dep, Coeff = 1.5F, CanHasNumber = true, CanHasSingleName = true });
            }
            t = new OrgItemTypeTermin("ГЛАВНОЕ УПРАВЛЕНИЕ") { Acronym = "ГУ", Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ЛИНЕЙНОЕ УПРАВЛЕНИЕ") { Acronym = "ЛУ", Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ГОЛОВНЕ УПРАВЛІННЯ", Pullenti.Morph.MorphLang.UA) { Acronym = "ГУ", Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ГЛАВНОЕ ТЕРРИТОРИАЛЬНОЕ УПРАВЛЕНИЕ") { Acronym = "ГТУ", Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ГОЛОВНЕ ТЕРИТОРІАЛЬНЕ УПРАВЛІННЯ", Pullenti.Morph.MorphLang.UA) { Acronym = "ГТУ", Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОПЕРАЦИОННОЕ УПРАВЛЕНИЕ") { Acronym = "ОПЕРУ", Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОПЕРАЦІЙНЕ УПРАВЛІННЯ", Pullenti.Morph.MorphLang.UA) { Acronym = "ОПЕРУ", Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ТЕРРИТОРИАЛЬНОЕ УПРАВЛЕНИЕ") { Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ТЕРИТОРІАЛЬНЕ УПРАВЛІННЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("РЕГИОНАЛЬНОЕ УПРАВЛЕНИЕ") { Acronym = "РУ", Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("РЕГІОНАЛЬНЕ УПРАВЛІННЯ", Pullenti.Morph.MorphLang.UA) { Acronym = "РУ", Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("УПРАВЛЕНИЕ") { Typ = OrgItemTypeTyp.DepAdd, IsDoubtWord = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("УПРАВЛІННЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.DepAdd, IsDoubtWord = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ПОГРАНИЧНОЕ УПРАВЛЕНИЕ") { Acronym = "ПУ", Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true };
            m_Global.Add(t);
            foreach (string s in new string[] {"ПРЕСС-СЛУЖБА", "ПРЕСС-ЦЕНТР", "КОЛЛ-ЦЕНТР", "БУХГАЛТЕРИЯ", "МАГИСТРАТУРА", "АСПИРАНТУРА", "ДОКТОРАНТУРА", "ОРДИНАТУРА", "СОВЕТ ДИРЕКТОРОВ", "УЧЕНЫЙ СОВЕТ", "КОЛЛЕГИЯ", "ПЛЕНУМ", "АППАРАТ", "НАБЛЮДАТЕЛЬНЫЙ СОВЕТ", "ОБЩЕСТВЕННЫЙ СОВЕТ", "РУКОВОДСТВО", "ДИРЕКЦИЯ", "ПРАВЛЕНИЕ", "ЖЮРИ", "ПРЕЗИДИУМ", "СЕКРЕТАРИАТ", "СИНОД", "PRESS", "PRESS CENTER", "CLIENT CENTER", "CALL CENTER", "ACCOUNTING", "MASTER DEGREE", "POSTGRADUATE", "DOCTORATE", "RESIDENCY", "BOARD OF DIRECTORS", "DIRECTOR BOARD", "ACADEMIC COUNCIL", "BOARD", "PLENARY", "UNIT", "SUPERVISORY BOARD", "PUBLIC COUNCIL", "LEADERSHIP", "MANAGEMENT", "JURY", "BUREAU", "SECRETARIAT"}) 
            {
                m_Global.Add(new OrgItemTypeTermin(s) { Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Unit });
            }
            foreach (string s in new string[] {"ПРЕС-СЛУЖБА", "ПРЕС-ЦЕНТР", "БУХГАЛТЕРІЯ", "МАГІСТРАТУРА", "АСПІРАНТУРА", "ДОКТОРАНТУРА", "ОРДИНАТУРА", "РАДА ДИРЕКТОРІВ", "ВЧЕНА РАДА", "КОЛЕГІЯ", "ПЛЕНУМ", "АПАРАТ", "НАГЛЯДОВА РАДА", "ГРОМАДСЬКА РАДА", "КЕРІВНИЦТВО", "ДИРЕКЦІЯ", "ПРАВЛІННЯ", "ЖУРІ", "ПРЕЗИДІЯ", "СЕКРЕТАРІАТ"}) 
            {
                m_Global.Add(new OrgItemTypeTermin(s, Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Unit });
            }
            t = new OrgItemTypeTermin("ОТДЕЛ ИНФОРМАЦИОННОЙ БЕЗОПАСНОСТИ") { Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Unit };
            t.AddVariant("ОТДЕЛ ИБ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОТДЕЛ ИНФОРМАЦИОННЫХ ТЕХНОЛОГИЙ") { Typ = OrgItemTypeTyp.DepAdd, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Unit };
            t.AddVariant("ОТДЕЛ ИТ", false);
            t.AddVariant("ОТДЕЛ IT", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("СЕКТОР") { Typ = OrgItemTypeTyp.Dep, IsDoubtWord = true });
            m_Global.Add(new OrgItemTypeTermin("КУРС") { Typ = OrgItemTypeTyp.Dep, CanHasNumber = true, IsDoubtWord = true });
            m_Global.Add(new OrgItemTypeTermin("ГРУППА") { Typ = OrgItemTypeTyp.Dep, CanHasNumber = true, IsDoubtWord = true, CanHasLatinName = true, CanHasSingleName = true });
            m_Global.Add(new OrgItemTypeTermin("ГРУПА", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Dep, CanHasNumber = true, IsDoubtWord = true, CanHasLatinName = true, CanHasSingleName = true });
            m_Global.Add(new OrgItemTypeTermin("ДНЕВНОЕ ОТДЕЛЕНИЕ") { Typ = OrgItemTypeTyp.Dep, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ДЕННЕ ВІДДІЛЕННЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Dep, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ВЕЧЕРНЕЕ ОТДЕЛЕНИЕ") { Typ = OrgItemTypeTyp.Dep, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ВЕЧІРНЄ ВІДДІЛЕННЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Dep, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ДЕЖУРНАЯ ЧАСТЬ") { Typ = OrgItemTypeTyp.Dep, CanBeNormalDep = true });
            m_Global.Add(new OrgItemTypeTermin("ЧЕРГОВА ЧАСТИНА", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Dep, CanBeNormalDep = true });
            t = new OrgItemTypeTermin("ПАСПОРТНЫЙ СТОЛ") { Typ = OrgItemTypeTyp.Dep, CanHasNumber = true };
            t.AddAbridge("П/С");
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ПАСПОРТНИЙ СТІЛ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Dep, CanHasNumber = true };
            t.AddAbridge("П/С");
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ВЫСШЕЕ УЧЕБНОЕ ЗАВЕДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Profile = Pullenti.Ner.Org.OrgProfile.Education, Acronym = "ВУЗ" });
            m_Global.Add(new OrgItemTypeTermin("ВИЩИЙ НАВЧАЛЬНИЙ ЗАКЛАД", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Profile = Pullenti.Ner.Org.OrgProfile.Education, Acronym = "ВНЗ" });
            m_Global.Add(new OrgItemTypeTermin("ВЫСШЕЕ ПРОФЕССИОНАЛЬНОЕ УЧИЛИЩЕ") { Typ = OrgItemTypeTyp.Prefix, Profile = Pullenti.Ner.Org.OrgProfile.Education, Acronym = "ВПУ" });
            m_Global.Add(new OrgItemTypeTermin("ВИЩЕ ПРОФЕСІЙНЕ УЧИЛИЩЕ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Profile = Pullenti.Ner.Org.OrgProfile.Education, Acronym = "ВПУ" });
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ИССЛЕДОВАТЕЛЬСКИЙ ИНСТИТУТ") { Typ = OrgItemTypeTyp.Prefix, Profile = Pullenti.Ner.Org.OrgProfile.Science, Acronym = "НИИ" });
            m_Global.Add(new OrgItemTypeTermin("НАУКОВО ДОСЛІДНИЙ ІНСТИТУТ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Profile = Pullenti.Ner.Org.OrgProfile.Science, Acronym = "НДІ" });
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ИССЛЕДОВАТЕЛЬСКИЙ ЦЕНТР") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НИЦ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУКОВО ДОСЛІДНИЙ ЦЕНТР", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "НДЦ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("ЦЕНТРАЛЬНЫЙ НАУЧНО ИССЛЕДОВАТЕЛЬСКИЙ ИНСТИТУТ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЦНИИ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("ВСЕРОССИЙСКИЙ НАУЧНО ИССЛЕДОВАТЕЛЬСКИЙ ИНСТИТУТ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ВНИИ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("РОССИЙСКИЙ НАУЧНО ИССЛЕДОВАТЕЛЬСКИЙ ИНСТИТУТ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "РНИИ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            t = new OrgItemTypeTermin("ИННОВАЦИОННЫЙ ЦЕНТР") { Typ = OrgItemTypeTyp.Prefix, Profile = Pullenti.Ner.Org.OrgProfile.Science };
            t.AddVariant("ИННОЦЕНТР", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ТЕХНИЧЕСКИЙ ЦЕНТР") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НТЦ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУКОВО ТЕХНІЧНИЙ ЦЕНТР", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "НТЦ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ТЕХНИЧЕСКАЯ ФИРМА") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НТФ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУКОВО ВИРОБНИЧА ФІРМА", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "НВФ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ПРОИЗВОДСТВЕННОЕ ОБЪЕДИНЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НПО", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУКОВО ВИРОБНИЧЕ ОБЄДНАННЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "НВО", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ПРОИЗВОДСТВЕННЫЙ КООПЕРАТИВ") { Typ = OrgItemTypeTyp.Prefix, Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУКОВО-ВИРОБНИЧИЙ КООПЕРАТИВ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "НВК", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            t = new OrgItemTypeTermin("НАУЧНО ПРОИЗВОДСТВЕННАЯ КОРПОРАЦИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НПК", Profile = Pullenti.Ner.Org.OrgProfile.Science };
            t.AddVariant("НАУЧНО ПРОИЗВОДСТВЕННАЯ КОМПАНИЯ", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ТЕХНИЧЕСКИЙ КОМПЛЕКС") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НТК", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("МЕЖОТРАСЛЕВОЙ НАУЧНО ТЕХНИЧЕСКИЙ КОМПЛЕКС") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МНТК", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ПРОИЗВОДСТВЕННОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НПП", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУКОВО ВИРОБНИЧЕ ПІДПРИЄМСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "НВП", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ПРОИЗВОДСТВЕННЫЙ ЦЕНТР") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НПЦ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУКОВО ВИРОБНИЧЕ ЦЕНТР", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "НВЦ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ПРОИЗВОДСТВЕННОЕ УНИТАРНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НПУП", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("ИНДИВИДУАЛЬНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ИП" });
            m_Global.Add(new OrgItemTypeTermin("ЧАСТНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЧП" });
            m_Global.Add(new OrgItemTypeTermin("ПРИВАТНЕ ПІДПРИЄМСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ПП" });
            m_Global.Add(new OrgItemTypeTermin("ЧАСТНОЕ УНИТАРНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЧУП" });
            m_Global.Add(new OrgItemTypeTermin("ЧАСТНОЕ ПРОИЗВОДСТВЕННОЕ УНИТАРНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЧПУП" });
            m_Global.Add(new OrgItemTypeTermin("ЧАСТНОЕ ИНДИВИДУАЛЬНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЧИП" });
            m_Global.Add(new OrgItemTypeTermin("ЧАСТНОЕ ОХРАННОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЧОП" });
            m_Global.Add(new OrgItemTypeTermin("ЧАСТНАЯ ОХРАННАЯ ОРГАНИЗАЦИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЧОО" });
            m_Global.Add(new OrgItemTypeTermin("ЧАСТНОЕ ТРАНСПОРТНОЕ УНИТАРНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЧТУП" });
            m_Global.Add(new OrgItemTypeTermin("ЧАСТНОЕ ТРАНСПОРТНО ЭКСПЛУАТАЦИОННОЕ УНИТАРНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЧТЭУП" });
            m_Global.Add(new OrgItemTypeTermin("НАУЧНО ПРОИЗВОДСТВЕННОЕ КОРПОРАЦИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НПК" });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ ГОСУДАРСТВЕННОЕ УНИТАРНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФГУП" });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ УНИТАРНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГУП" });
            t = new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГП" };
            t.AddVariant("ГОСПРЕДПРИЯТИЕ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ДЕРЖАВНЕ ПІДПРИЄМСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ДП" };
            t.AddVariant("ДЕРЖПІДПРИЄМСТВО", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ ГОСУДАРСТВЕННОЕ НАУЧНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФГНУ", Profile = Pullenti.Ner.Org.OrgProfile.Science });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ ГОСУДАРСТВЕННОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФГУ" });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ ГОСУДАРСТВЕННОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФГКУ" });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ ГОСУДАРСТВЕННОЕ КАЗЕННОЕ ОБРАЗОВАТЕЛЬНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФГКОУ" });
            t = new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФГБУ" };
            t.AddVariant("ФЕДЕРАЛЬНОЕ ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ НАУКИ", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ВОЕННО ПРОМЫШЛЕННАЯ КОРПОРАЦИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ВПК" });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФБУ" });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ УНИТАРНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФУП" });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФКУ" });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ НЕКОММЕРЧЕСКОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МНУ" });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МБУ" });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ АВТОНОМНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МАУ" });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МКУ" });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ УНИТАРНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МУП" });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ УНИТАРНОЕ ПРОИЗВОДСТВЕННОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МУПП" });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ КАЗЕННОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МКП" });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МП" });
            m_Global.Add(new OrgItemTypeTermin("НЕБАНКОВСКАЯ КРЕДИТНАЯ ОРГАНИЗАЦИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НКО" });
            m_Global.Add(new OrgItemTypeTermin("РАСЧЕТНАЯ НЕБАНКОВСКАЯ КРЕДИТНАЯ ОРГАНИЗАЦИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "РНКО" });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГБУ" });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГКУ" });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ АВТОНОМНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГАУ" });
            m_Global.Add(new OrgItemTypeTermin("МАЛОЕ ИННОВАЦИОННОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix });
            m_Global.Add(new OrgItemTypeTermin("НЕГОСУДАРСТВЕННЫЙ ПЕНСИОННЫЙ ФОНД") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НПФ" });
            m_Global.Add(new OrgItemTypeTermin("ДЕРЖАВНА АКЦІОНЕРНА КОМПАНІЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ДАК" });
            m_Global.Add(new OrgItemTypeTermin("ДЕРЖАВНА КОМПАНІЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ДК" });
            m_Global.Add(new OrgItemTypeTermin("КОЛЕКТИВНЕ ПІДПРИЄМСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "КП" });
            m_Global.Add(new OrgItemTypeTermin("КОЛЕКТИВНЕ МАЛЕ ПІДПРИЄМСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "КМП" });
            m_Global.Add(new OrgItemTypeTermin("ВИРОБНИЧА ФІРМА", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ВФ" });
            m_Global.Add(new OrgItemTypeTermin("ВИРОБНИЧЕ ОБЄДНАННЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ВО" });
            m_Global.Add(new OrgItemTypeTermin("ВИРОБНИЧЕ ПІДПРИЄМСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ВП" });
            m_Global.Add(new OrgItemTypeTermin("ВИРОБНИЧИЙ КООПЕРАТИВ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ВК" });
            m_Global.Add(new OrgItemTypeTermin("СТРАХОВА КОМПАНІЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "СК" });
            m_Global.Add(new OrgItemTypeTermin("ТВОРЧЕ ОБЄДНАННЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ТО" });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ ГОСУДАРСТВЕННОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФГУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФКУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ АВТОНОМНОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГАУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГБУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ ОБЛАСТНОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГОБУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГКУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ ОБЛАСТНОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГОКУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("НЕГОСУДАРСТВЕННОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МБУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МКУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ ОБЛАСТНОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МОБУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ АВТОНОМНОЕ УЧРЕЖДЕНИЕ ЗДРАВООХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МАУЗ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ ГОСУДАРСТВЕННОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФГУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФКУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ АВТОНОМНОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГАУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГБУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ ОБЛАСТНОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГОБУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГКУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ ОБЛАСТНОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГОКУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("НЕГОСУДАРСТВЕННОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "НУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МБУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МКУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ ОБЛАСТНОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МОБУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ АВТОНОМНОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МАУК", Profile = Pullenti.Ner.Org.OrgProfile.Art });
            t = new OrgItemTypeTermin("ЧАСТНОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЧУК" };
            t.AddVariant("ЧАСТНОЕ УЧРЕЖДЕНИЕ КУЛЬТУРЫ ЛФП", false);
            t.AddVariant("ЧУК ЛФП", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ ОБРАЗОВАНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГБУО", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            t = new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ ПРОФЕССИОНАЛЬНОЕ ОБРАЗОВАТЕЛЬНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГБПОУ", Profile = Pullenti.Ner.Org.OrgProfile.Education };
            t.AddVariant("ГБ ПОУ", true);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ ОБЩЕОБРАЗОВАТЕЛЬНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГБОУ", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ УЧРЕЖДЕНИЕ ДОПОЛНИТЕЛЬНОГО ОБРАЗОВАНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГБУДО", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ ОБРАЗОВАТЕЛЬНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МОУ", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ КАЗЕННОЕ ОБРАЗОВАТЕЛЬНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МКОУ", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ АВТОНОМНОЕ ОБЩЕОБРАЗОВАТЕЛЬНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МАОУ", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("МУНИЦИПАЛЬНОЕ ЛЕЧЕБНО ПРОФИЛАКТИЧЕСКОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "МЛПУ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ КАЗЕННОЕ ЛЕЧЕБНО ПРОФИЛАКТИЧЕСКОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФКЛПУ", Profile = Pullenti.Ner.Org.OrgProfile.Medicine });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ ОБРАЗОВАТЕЛЬНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГОУ", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ ОБРАЗОВАТЕЛЬНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГБОУ", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ФЕДЕРАЛЬНОЕ ГОСУДАРСТВЕННОЕ БЮДЖЕТНОЕ ОБРАЗОВАТЕЛЬНОЕ УЧРЕЖДЕНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФГБОУ", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ВЫСШЕЕ ПРОФЕССИОНАЛЬНОЕ ОБРАЗОВАНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ВПО", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ДОПОЛНИТЕЛЬНОЕ ПРОФЕССИОНАЛЬНОЕ ОБРАЗОВАНИЕ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ДПО", Profile = Pullenti.Ner.Org.OrgProfile.Education });
            m_Global.Add(new OrgItemTypeTermin("ДЕПАРТАМЕНТ ЕДИНОГО ЗАКАЗЧИКА") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ДЕЗ", AcronymCanBeLower = true, CanBeSingleGeo = true });
            t = new OrgItemTypeTermin("СОЮЗ АРБИТРАЖНЫХ УПРАВЛЯЮЩИХ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "САУ", CanHasLatinName = true };
            t.AddVariant("САМОРЕГУЛИРУЕМАЯ ОРГАНИЗАЦИЯ АРБИТРАЖНЫХ УПРАВЛЯЮЩИХ", false);
            t.AddVariant("СОАУ", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("АКЦИОНЕРНОЕ ОБЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АО" });
            m_Global.Add(new OrgItemTypeTermin("АКЦІОНЕРНЕ ТОВАРИСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АТ" });
            m_Global.Add((m_SovmPred = new OrgItemTypeTermin("СОВМЕСТНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "СП" }));
            m_Global.Add(new OrgItemTypeTermin("СПІЛЬНЕ ПІДПРИЄМСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "СП" });
            m_Global.Add((m_AkcionComp = new OrgItemTypeTermin("АКЦИОНЕРНАЯ КОМПАНИЯ") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true }));
            m_Global.Add(new OrgItemTypeTermin("ЗАКРЫТОЕ АКЦИОНЕРНОЕ ОБЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ЗАО" });
            m_Global.Add(new OrgItemTypeTermin("РОССИЙСКОЕ АКЦИОНЕРНОЕ ОБЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "РАО", AcronymSmart = "PAO" });
            m_Global.Add(new OrgItemTypeTermin("РОССИЙСКОЕ ОТКРЫТОЕ АКЦИОНЕРНОЕ ОБЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "РОАО" });
            m_Global.Add(new OrgItemTypeTermin("АКЦИОНЕРНОЕ ОБЩЕСТВО ЗАКРЫТОГО ТИПА") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АОЗТ" });
            m_Global.Add(new OrgItemTypeTermin("АКЦІОНЕРНЕ ТОВАРИСТВО ЗАКРИТОГО ТИПУ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АТЗТ" });
            m_Global.Add(new OrgItemTypeTermin("АКЦИОНЕРНОЕ ОБЩЕСТВО ОТКРЫТОГО ТИПА") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АООТ" });
            m_Global.Add(new OrgItemTypeTermin("АКЦІОНЕРНЕ ТОВАРИСТВО ВІДКРИТОГО ТИПУ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АТВТ" });
            m_Global.Add(new OrgItemTypeTermin("ОБЩЕСТВЕННАЯ ОРГАНИЗАЦИЯ") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ОО" });
            m_Global.Add(new OrgItemTypeTermin("ГРОМАДСЬКА ОРГАНІЗАЦІЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ГО" });
            m_Global.Add(new OrgItemTypeTermin("АВТОНОМНАЯ НЕКОММЕРЧЕСКАЯ ОРГАНИЗАЦИЯ") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АНО" });
            m_Global.Add(new OrgItemTypeTermin("АВТОНОМНА НЕКОМЕРЦІЙНА ОРГАНІЗАЦІЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АНО" });
            m_Global.Add(new OrgItemTypeTermin("ОТКРЫТОЕ АКЦИОНЕРНОЕ ОБЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ОАО", AcronymSmart = "OAO" });
            m_Global.Add(new OrgItemTypeTermin("ВІДКРИТЕ АКЦІОНЕРНЕ ТОВАРИСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ВАТ", AcronymSmart = "ВАТ" });
            m_Global.Add(new OrgItemTypeTermin("ЧАСТНОЕ АКЦИОНЕРНОЕ ОБЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ЧАО", AcronymSmart = "ЧAO" });
            m_Global.Add(new OrgItemTypeTermin("ОТКРЫТОЕ СТРАХОВОЕ АКЦИОНЕРНОЕ ОБЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ОСАО" });
            t = new OrgItemTypeTermin("ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ООО", AcronymSmart = "OOO" };
            t.AddVariant("ОБЩЕСТВО C ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ТОВАРИСТВО З ОБМЕЖЕНОЮ ВІДПОВІДАЛЬНІСТЮ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ТОВ", AcronymSmart = "ТОВ" });
            m_Global.Add(new OrgItemTypeTermin("ТОВАРИСТВО З ПОВНОЮ ВІДПОВІДАЛЬНІСТЮ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ТПВ", AcronymSmart = "ТПВ" });
            m_Global.Add(new OrgItemTypeTermin("ТОВАРИСТВО З ОБМЕЖЕНОЮ ВІДПОВІДАЛЬНІСТЮ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ТЗОВ", AcronymSmart = "ТЗОВ" });
            m_Global.Add(new OrgItemTypeTermin("ТОВАРИСТВО З ДОДАТКОВОЮ ВІДПОВІДАЛЬНІСТЮ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ТДВ", AcronymSmart = "ТДВ" });
            m_Global.Add(new OrgItemTypeTermin("ЧАСТНОЕ АКЦИОНЕРНОЕ ТОВАРИЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("ПРИВАТНЕ АКЦІОНЕРНЕ ТОВАРИСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ПРАТ", AcronymSmart = "ПРАТ" });
            m_Global.Add(new OrgItemTypeTermin("ПУБЛИЧНОЕ АКЦИОНЕРНОЕ ТОВАРИЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("ПУБЛІЧНЕ АКЦІОНЕРНЕ ТОВАРИСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ПАТ", AcronymSmart = "ПАТ" });
            m_Global.Add(new OrgItemTypeTermin("ЗАКРЫТОЕ АКЦИОНЕРНОЕ ТОВАРИЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("ЗАКРИТЕ АКЦІОНЕРНЕ ТОВАРИСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ЗАТ", AcronymSmart = "ЗАТ" });
            m_Global.Add(new OrgItemTypeTermin("ОТКРЫТОЕ АКЦИОНЕРНОЕ ТОВАРИЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("ВІДКРИТЕ АКЦІОНЕРНЕ ТОВАРИСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ВАТ", AcronymSmart = "ВАТ" });
            m_Global.Add(new OrgItemTypeTermin("ПУБЛИЧНОЕ АКЦИОНЕРНОЕ ОБЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ПАО" });
            m_Global.Add(new OrgItemTypeTermin("СТРАХОВОЕ ПУБЛИЧНОЕ АКЦИОНЕРНОЕ ОБЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "СПАО" });
            t = new OrgItemTypeTermin("БЛАГОТВОРИТЕЛЬНАЯ ОБЩЕСТВЕННАЯ ОРГАНИЗАЦИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "БОО", AcronymSmart = "БОО" };
            t.AddVariant("ОБЩЕСТВЕННАЯ БЛАГОТВОРИТЕЛЬНАЯ ОРГАНИЗАЦИЯ", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ТОВАРИЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ТОО", AcronymSmart = "TOO" });
            m_Global.Add(new OrgItemTypeTermin("ПРЕДПРИНИМАТЕЛЬ БЕЗ ОБРАЗОВАНИЯ ЮРИДИЧЕСКОГО ЛИЦА") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ПБОЮЛ" });
            m_Global.Add(new OrgItemTypeTermin("АКЦИОНЕРНЫЙ КОММЕРЧЕСКИЙ БАНК") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АКБ" });
            m_Global.Add(new OrgItemTypeTermin("АКЦІОНЕРНИЙ КОМЕРЦІЙНИЙ БАНК", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АКБ" });
            m_Global.Add(new OrgItemTypeTermin("АКЦИОНЕРНЫЙ БАНК") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АБ" });
            m_Global.Add(new OrgItemTypeTermin("АКЦІОНЕРНИЙ БАНК", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АБ" });
            m_Global.Add(new OrgItemTypeTermin("КОММЕРЧЕСКИЙ БАНК") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("КОМЕРЦІЙНИЙ БАНК", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("КОНСТРУКТОРСКОЕ БЮРО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("КОНСТРУКТОРСЬКЕ БЮРО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("ОПЫТНО КОНСТРУКТОРСКОЕ БЮРО") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ОКБ" });
            m_Global.Add(new OrgItemTypeTermin("ДОСЛІДНО КОНСТРУКТОРСЬКЕ БЮРО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ДКБ" });
            m_Global.Add(new OrgItemTypeTermin("СПЕЦИАЛЬНОЕ КОНСТРУКТОРСКОЕ БЮРО") { Typ = OrgItemTypeTyp.Prefix, Acronym = "СКБ", CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("СПЕЦІАЛЬНЕ КОНСТРУКТОРСЬКЕ БЮРО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "СКБ", CanHasLatinName = true });
            m_Global.Add(new OrgItemTypeTermin("АКЦИОНЕРНАЯ СТРАХОВАЯ КОМПАНИЯ") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АСК" });
            m_Global.Add(new OrgItemTypeTermin("АКЦІОНЕРНА СТРАХОВА КОМПАНІЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "АСК" });
            m_Global.Add(new OrgItemTypeTermin("АВТОТРАНСПОРТНОЕ ПРЕДПРИЯТИЕ") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, CanHasNumber = true, Acronym = "АТП" });
            m_Global.Add(new OrgItemTypeTermin("АВТОТРАНСПОРТНЕ ПІДПРИЄМСТВО", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, CanHasNumber = true, Acronym = "АТП" });
            m_Global.Add(new OrgItemTypeTermin("ТЕЛЕРАДИОКОМПАНИЯ") { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ТРК" });
            m_Global.Add(new OrgItemTypeTermin("ТЕЛЕРАДІОКОМПАНІЯ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, CanHasLatinName = true, Acronym = "ТРК" });
            t = new OrgItemTypeTermin("ОРГАНИЗОВАННАЯ ПРЕСТУПНАЯ ГРУППИРОВКА") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ОПГ", CanHasLatinName = true };
            t.AddVariant("ОРГАНИЗОВАННАЯ ПРЕСТУПНАЯ ГРУППА", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОРГАНИЗОВАННОЕ ПРЕСТУПНОЕ СООБЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ОПС", CanHasLatinName = true };
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ПОДРОСТКОВО МОЛОДЕЖНЫЙ КЛУБ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ПМК", CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("СКЛАД ВРЕМЕННОГО ХРАНЕНИЯ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "СВХ", CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ЖИЛИЩНО СТРОИТЕЛЬНЫЙ КООПЕРАТИВ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ЖСК", CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ГАРАЖНО СТРОИТЕЛЬНЫЙ КООПЕРАТИВ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГСК", CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ГАРАЖНО ЭКСПЛУАТАЦИОННЫЙ КООПЕРАТИВ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГЭК", CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ГАРАЖНО ПОТРЕБИТЕЛЬСКИЙ КООПЕРАТИВ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГПК", CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ПОТРЕБИТЕЛЬСКИЙ ГАРАЖНО СТРОИТЕЛЬНЫЙ КООПЕРАТИВ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ПГСК", CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ГАРАЖНЫЙ СТРОИТЕЛЬНО ПОТРЕБИТЕЛЬСКИЙ КООПЕРАТИВ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ГСПК", CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ДАЧНО СТРОИТЕЛЬНЫЙ КООПЕРАТИВ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ДСК", CanHasLatinName = true, CanHasNumber = true });
            t = new OrgItemTypeTermin("САДОВОЕ НЕКОММЕРЧЕСКОЕ ТОВАРИЩЕСТВО") { Typ = OrgItemTypeTyp.Prefix, Acronym = "СНТ", CanHasLatinName = true, CanHasNumber = true };
            t.AddAbridge("САДОВОЕ НЕКОМ-Е ТОВАРИЩЕСТВО");
            t.AddVariant("СНТ ПМК", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ПРЕДПРИЯТИЕ ПОТРЕБИТЕЛЬСКОЙ КООПЕРАЦИИ") { Typ = OrgItemTypeTyp.Prefix, Acronym = "ППК", CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ПІДПРИЄМСТВО СПОЖИВЧОЇ КООПЕРАЦІЇ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ПСК", CanHasLatinName = true, CanHasNumber = true });
            m_Global.Add(new OrgItemTypeTermin("ФІЗИЧНА ОСОБА ПІДПРИЄМЕЦЬ", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Prefix, Acronym = "ФОП", CanHasLatinName = true, CanHasSingleName = true });
            t = new OrgItemTypeTermin("ЖЕЛЕЗНАЯ ДОРОГА") { Typ = OrgItemTypeTyp.Org, Profile = Pullenti.Ner.Org.OrgProfile.Transport, CanHasLatinName = true, Coeff = 3 };
            t.AddVariant("ЖЕЛЕЗНОДОРОЖНАЯ МАГИСТРАЛЬ", false);
            t.AddAbridge("Ж.Д.");
            t.AddAbridge("Ж/Д");
            t.AddAbridge("ЖЕЛ.ДОР.");
            m_Global.Add(t);
            foreach (string s in new string[] {"ЗАВОД", "ФАБРИКА", "БАНК", "КОМБИНАТ", "МЯСОКОМБИНАТ", "БАНКОВСКАЯ ГРУППА", "БИРЖА", "ФОНДОВАЯ БИРЖА", "FACTORY", "MANUFACTORY", "BANK"}) 
            {
                m_Global.Add((t = new OrgItemTypeTermin(s) { Coeff = (float)3.5, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true }));
                if (s == "БАНК" || s == "BANK" || s.EndsWith("БИРЖА")) 
                {
                    t.Profile = Pullenti.Ner.Org.OrgProfile.Finance;
                    t.Coeff = 2;
                    t.CanHasLatinName = true;
                    if (m_Bank == null) 
                        m_Bank = t;
                }
            }
            foreach (string s in new string[] {"ЗАВОД", "ФАБРИКА", "БАНК", "КОМБІНАТ", "БАНКІВСЬКА ГРУПА", "БІРЖА", "ФОНДОВА БІРЖА"}) 
            {
                m_Global.Add((t = new OrgItemTypeTermin(s, Pullenti.Morph.MorphLang.UA) { Coeff = (float)3.5, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true }));
                if (s == "БАНК" || s.EndsWith("БІРЖА")) 
                {
                    t.Coeff = 2;
                    t.CanHasLatinName = true;
                    if (m_Bank == null) 
                        m_Bank = t;
                }
            }
            foreach (string s in new string[] {"ТУРФИРМА", "ТУРАГЕНТСТВО", "ТУРКОМПАНИЯ", "АВИАКОМПАНИЯ", "КИНОСТУДИЯ", "БИЗНЕС-ЦЕНТР", "КООПЕРАТИВ", "РИТЕЙЛЕР", "ОНЛАЙН РИТЕЙЛЕР", "МЕДИАГИГАНТ", "МЕДИАКОМПАНИЯ", "МЕДИАХОЛДИНГ"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = (float)3.5, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true, MustHasCapitalName = true };
                if (s.StartsWith("МЕДИА")) 
                    t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Media);
                if (s.Contains("РИТЕЙЛЕР")) 
                    t.AddVariant(s.Replace("РИТЕЙЛЕР", "РЕТЕЙЛЕР"), false);
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ТУРФІРМА", "ТУРАГЕНТСТВО", "ТУРКОМПАНІЯ", "АВІАКОМПАНІЯ", "КІНОСТУДІЯ", "БІЗНЕС-ЦЕНТР", "КООПЕРАТИВ", "РІТЕЙЛЕР", "ОНЛАЙН-РІТЕЙЛЕР", "МЕДІАГІГАНТ", "МЕДІАКОМПАНІЯ", "МЕДІАХОЛДИНГ"}) 
            {
                t = new OrgItemTypeTermin(s, Pullenti.Morph.MorphLang.UA) { Coeff = (float)3.5, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true, MustHasCapitalName = true };
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ТУРОПЕРАТОР"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = (float)0.5, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true, MustHasCapitalName = true };
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ТУРОПЕРАТОР"}) 
            {
                t = new OrgItemTypeTermin(s, Pullenti.Morph.MorphLang.UA) { Coeff = (float)0.5, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true, MustHasCapitalName = true };
                m_Global.Add(t);
            }
            m_SberBank = (t = new OrgItemTypeTermin("СБЕРЕГАТЕЛЬНЫЙ БАНК") { Coeff = 4, Typ = OrgItemTypeTyp.Org, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Finance });
            t.AddVariant("СБЕРБАНК", false);
            m_Global.Add(t);
            m_SecServ = (t = new OrgItemTypeTermin("СЛУЖБА БЕЗОПАСНОСТИ") { Coeff = 4, Typ = OrgItemTypeTyp.Org, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.State });
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОЩАДНИЙ БАНК", Pullenti.Morph.MorphLang.UA) { Coeff = 4, Typ = OrgItemTypeTyp.Org, CanBeNormalDep = true, Profile = Pullenti.Ner.Org.OrgProfile.Finance };
            t.AddVariant("ОЩАДБАНК", false);
            m_Global.Add(t);
            foreach (string s in new string[] {"ОРГАНИЗАЦИЯ", "ПРЕДПРИЯТИЕ", "КОМИТЕТ", "КОМИССИЯ", "ПРОИЗВОДИТЕЛЬ", "ГИГАНТ", "ORGANIZATION", "ENTERPRISE", "COMMITTEE", "COMMISSION", "MANUFACTURER"}) 
            {
                m_Global.Add((t = new OrgItemTypeTermin(s) { Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true, IsDoubtWord = true }));
            }
            foreach (string s in new string[] {"ОБЩЕСТВО", "АССАМБЛЕЯ", "СЛУЖБА", "ОБЪЕДИНЕНИЕ", "ФЕДЕРАЦИЯ", "COMPANY", "ASSEMBLY", "SERVICE", "UNION", "FEDERATION"}) 
            {
                m_Global.Add((t = new OrgItemTypeTermin(s) { Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true, IsDoubtWord = true }));
                if (s == "СЛУЖБА") 
                    t.CanHasNumber = true;
            }
            foreach (string s in new string[] {"СООБЩЕСТВО", "ФОНД", "АССОЦИАЦИЯ", "АЛЬЯНС", "ГИЛЬДИЯ", "ОБЩИНА", "ОБЩЕСТВЕННОЕ ОБЪЕДИНЕНИЕ", "ОБЩЕСТВЕННАЯ ОРГАНИЗАЦИЯ", "ОБЩЕСТВЕННОЕ ФОРМИРОВАНИЕ", "СОЮЗ", "КЛУБ", "ГРУППИРОВКА", "ЛИГА", "COMMUNITY", "FOUNDATION", "ASSOCIATION", "ALLIANCE", "GUILD", "UNION", "CLUB", "GROUP", "LEAGUE"}) 
            {
                m_Global.Add((t = new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true, IsDoubtWord = true, Profile = Pullenti.Ner.Org.OrgProfile.Union }));
            }
            foreach (string s in new string[] {"ПАРТИЯ", "ДВИЖЕНИЕ", "PARTY", "MOVEMENT"}) 
            {
                m_Global.Add((t = new OrgItemTypeTermin(s) { Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true, IsDoubtWord = true, Profile = Pullenti.Ner.Org.OrgProfile.Union }));
            }
            foreach (string s in new string[] {"НОЧНОЙ КЛУБ", "NIGHTCLUB"}) 
            {
                m_Global.Add((t = new OrgItemTypeTermin(s) { Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true, Profile = Pullenti.Ner.Org.OrgProfile.Music }));
            }
            foreach (string s in new string[] {"ОРГАНІЗАЦІЯ", "ПІДПРИЄМСТВО", "КОМІТЕТ", "КОМІСІЯ", "ВИРОБНИК", "ГІГАНТ", "СУСПІЛЬСТВО", "СПІЛЬНОТА", "ФОНД", "СЛУЖБА", "АСОЦІАЦІЯ", "АЛЬЯНС", "АСАМБЛЕЯ", "ГІЛЬДІЯ", "ОБЄДНАННЯ", "СОЮЗ", "ПАРТІЯ", "РУХ", "ФЕДЕРАЦІЯ", "КЛУБ", "ГРУПУВАННЯ"}) 
            {
                m_Global.Add((t = new OrgItemTypeTermin(s, Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true, IsDoubtWord = true }));
            }
            t = new OrgItemTypeTermin("ДЕПУТАТСКАЯ ГРУППА") { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanHasSingleName = true };
            t.AddVariant("ГРУППА ДЕПУТАТОВ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ДЕПУТАТСЬКА ГРУПА", Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanHasSingleName = true };
            t.AddVariant("ГРУПА ДЕПУТАТІВ", false);
            m_Global.Add(t);
            foreach (string s in new string[] {"ФОНД", "СОЮЗ", "ОБЪЕДИНЕНИЕ", "ОРГАНИЗАЦИЯ", "ФЕДЕРАЦИЯ", "ДВИЖЕНИЕ"}) 
            {
                foreach (string ss in new string[] {"ВСЕМИРНЫЙ", "МЕЖДУНАРОДНЫЙ", "ВСЕРОССИЙСКИЙ", "ОБЩЕСТВЕННЫЙ", "НЕКОММЕРЧЕСКИЙ", "ЕВРОПЕЙСКИЙ", "ВСЕУКРАИНСКИЙ"}) 
                {
                    t = new OrgItemTypeTermin(string.Format("{0} {1}", ss, s)) { Coeff = (float)3.5, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true };
                    if (s == "ОБЪЕДИНЕНИЕ" || s == "ДВИЖЕНИЕ") 
                        t.CanonicText = string.Format("{0}ОЕ {1}", ss.Substring(0, ss.Length - 2), s);
                    else if (s == "ОРГАНИЗАЦИЯ" || s == "ФЕДЕРАЦИЯ") 
                    {
                        t.CanonicText = string.Format("{0}АЯ {1}", ss.Substring(0, ss.Length - 2), s);
                        t.Coeff = 3;
                    }
                    m_Global.Add(t);
                }
            }
            foreach (string s in new string[] {"ФОНД", "СОЮЗ", "ОБЄДНАННЯ", "ОРГАНІЗАЦІЯ", "ФЕДЕРАЦІЯ", "РУХ"}) 
            {
                foreach (string ss in new string[] {"СВІТОВИЙ", "МІЖНАРОДНИЙ", "ВСЕРОСІЙСЬКИЙ", "ГРОМАДСЬКИЙ", "НЕКОМЕРЦІЙНИЙ", "ЄВРОПЕЙСЬКИЙ", "ВСЕУКРАЇНСЬКИЙ"}) 
                {
                    t = new OrgItemTypeTermin(string.Format("{0} {1}", ss, s), Pullenti.Morph.MorphLang.UA) { Coeff = (float)3.5, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true };
                    Pullenti.Morph.MorphWordForm bi = Pullenti.Morph.MorphologyService.GetWordBaseInfo(s, Pullenti.Morph.MorphLang.UA, false, false);
                    if (bi != null && bi.Gender != Pullenti.Morph.MorphGender.Masculine) 
                    {
                        string adj = Pullenti.Morph.MorphologyService.GetWordform(ss, new Pullenti.Morph.MorphBaseInfo() { Class = Pullenti.Morph.MorphClass.Adjective, Gender = bi.Gender, Number = Pullenti.Morph.MorphNumber.Singular, Language = Pullenti.Morph.MorphLang.UA });
                        if (adj != null) 
                            t.CanonicText = string.Format("{0} {1}", adj, s);
                    }
                    if (s == "ОРГАНІЗАЦІЯ" || s == "ФЕДЕРАЦІЯ") 
                        t.Coeff = 3;
                    m_Global.Add(t);
                }
            }
            t = new OrgItemTypeTermin("ИНВЕСТИЦИОННЫЙ ФОНД") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true };
            t.AddVariant("ИНВЕСТФОНД", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ІНВЕСТИЦІЙНИЙ ФОНД", Pullenti.Morph.MorphLang.UA) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true };
            t.AddVariant("ІНВЕСТФОНД", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("СОЦИАЛЬНАЯ СЕТЬ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true };
            t.AddVariant("СОЦСЕТЬ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("СОЦІАЛЬНА МЕРЕЖА", Pullenti.Morph.MorphLang.UA) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true };
            t.AddVariant("СОЦМЕРЕЖА", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОФФШОРНАЯ КОМПАНИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true };
            t.AddVariant("ОФФШОР", false);
            t.AddVariant("ОФШОР", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОФШОРНА КОМПАНІЯ", Pullenti.Morph.MorphLang.UA) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasSingleName = true, CanHasLatinName = true };
            t.AddVariant("ОФШОР", false);
            m_Global.Add(t);
            m_Global.Add(new OrgItemTypeTermin("ТЕРРОРИСТИЧЕСКАЯ ОРГАНИЗАЦИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true });
            m_Global.Add(new OrgItemTypeTermin("ТЕРОРИСТИЧНА ОРГАНІЗАЦІЯ", Pullenti.Morph.MorphLang.UA) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true });
            m_Global.Add(new OrgItemTypeTermin("АТОМНАЯ ЭЛЕКТРОСТАНЦИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "АЭС", CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true });
            m_Global.Add(new OrgItemTypeTermin("АТОМНА ЕЛЕКТРОСТАНЦІЯ", Pullenti.Morph.MorphLang.UA) { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "АЕС", CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true });
            m_Global.Add(new OrgItemTypeTermin("ГИДРОЭЛЕКТРОСТАНЦИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "ГЭС", CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true });
            m_Global.Add(new OrgItemTypeTermin("ГІДРОЕЛЕКТРОСТАНЦІЯ", Pullenti.Morph.MorphLang.UA) { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "ГЕС", CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true });
            m_Global.Add(new OrgItemTypeTermin("ГИДРОРЕЦИРКУЛЯЦИОННАЯ ЭЛЕКТРОСТАНЦИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "ГРЭС", CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true });
            m_Global.Add(new OrgItemTypeTermin("ТЕПЛОВАЯ ЭЛЕКТРОСТАНЦИЯ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "ТЭС", CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true });
            m_Global.Add(new OrgItemTypeTermin("НЕФТЕПЕРЕРАБАТЫВАЮЩИЙ ЗАВОД") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "НПЗ", CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true });
            m_Global.Add(new OrgItemTypeTermin("НАФТОПЕРЕРОБНИЙ ЗАВОД", Pullenti.Morph.MorphLang.UA) { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "НПЗ", CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true });
            foreach (string s in new string[] {"ФИРМА", "КОМПАНИЯ", "КОРПОРАЦИЯ", "ГОСКОРПОРАЦИЯ", "КОНЦЕРН", "КОНСОРЦИУМ", "ХОЛДИНГ", "МЕДИАХОЛДИНГ", "ТОРГОВЫЙ ДОМ", "ТОРГОВЫЙ ЦЕНТР", "УЧЕБНЫЙ ЦЕНТР", "ИССЛЕДОВАТЕЛЬСКИЙ ЦЕНТР", "КОСМИЧЕСКИЙ ЦЕНТР", "АУКЦИОННЫЙ ДОМ", "ИЗДАТЕЛЬСТВО", "ИЗДАТЕЛЬСКИЙ ДОМ", "ТОРГОВЫЙ КОМПЛЕКС", "ТОРГОВО РАЗВЛЕКАТЕЛЬНЫЙ КОМПЛЕКС", "АГЕНТСТВО НЕДВИЖИМОСТИ", "ГРУППА КОМПАНИЙ", "МЕДИАГРУППА", "МАГАЗИН", "ТОРГОВЫЙ КОМПЛЕКС", "ГИПЕРМАРКЕТ", "СУПЕРМАРКЕТ", "КАФЕ", "РЕСТОРАН", "БАР", "ТРАКТИР", "ТАВЕРНА", "СТОЛОВАЯ", "АУКЦИОН", "АНАЛИТИЧЕСКИЙ ЦЕНТР", "COMPANY", "CORPORATION"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true };
                if (s == "ИЗДАТЕЛЬСТВО") 
                {
                    t.AddAbridge("ИЗД-ВО");
                    t.AddAbridge("ИЗ-ВО");
                    t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Media);
                    t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Press);
                    t.AddVariant("ИЗДАТЕЛЬСКИЙ ДОМ", false);
                }
                else if (s.StartsWith("ИЗДАТ")) 
                {
                    t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Press);
                    t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Media);
                }
                else if (s == "ТОРГОВЫЙ ДОМ") 
                    t.Acronym = "ТД";
                else if (s == "ТОРГОВЫЙ ЦЕНТР") 
                    t.Acronym = "ТЦ";
                else if (s == "ТОРГОВЫЙ КОМПЛКС") 
                    t.Acronym = "ТК";
                else if (s == "ГРУППА КОМПАНИЙ") 
                    t.Acronym = "ГК";
                else if (s == "СТОЛОВАЯ") 
                    t.CanHasNumber = true;
                if (s.StartsWith("МЕДИА")) 
                    t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Media);
                if (s.EndsWith(" ЦЕНТР")) 
                    t.Coeff = 3.5F;
                if (s == "КОМПАНИЯ" || s == "ФИРМА") 
                    t.Coeff = 1;
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ФІРМА", "КОМПАНІЯ", "КОРПОРАЦІЯ", "ДЕРЖКОРПОРАЦІЯ", "КОНЦЕРН", "КОНСОРЦІУМ", "ХОЛДИНГ", "МЕДІАХОЛДИНГ", "ТОРГОВИЙ ДІМ", "ТОРГОВИЙ ЦЕНТР", "НАВЧАЛЬНИЙ ЦЕНТР", "ВИДАВНИЦТВО", "ВИДАВНИЧИЙ ДІМ", "ТОРГОВИЙ КОМПЛЕКС", "ТОРГОВО-РОЗВАЖАЛЬНИЙ КОМПЛЕКС", "АГЕНТСТВО НЕРУХОМОСТІ", "ГРУПА КОМПАНІЙ", "МЕДІАГРУПА", "МАГАЗИН", "ТОРГОВИЙ КОМПЛЕКС", "ГІПЕРМАРКЕТ", "СУПЕРМАРКЕТ", "КАФЕ", "БАР", "АУКЦІОН", "АНАЛІТИЧНИЙ ЦЕНТР"}) 
            {
                t = new OrgItemTypeTermin(s, Pullenti.Morph.MorphLang.UA) { Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true };
                if (s == "ВИДАВНИЦТВО") 
                {
                    t.AddAbridge("ВИД-ВО");
                    t.AddVariant("ВИДАВНИЧИЙ ДІМ", false);
                }
                else if (s == "ТОРГОВИЙ ДІМ") 
                    t.Acronym = "ТД";
                else if (s == "ТОРГОВИЙ ЦЕНТР") 
                    t.Acronym = "ТЦ";
                else if (s == "ТОРГОВИЙ КОМПЛЕКС") 
                    t.Acronym = "ТК";
                else if (s == "ГРУПА КОМПАНІЙ") 
                    t.Acronym = "ГК";
                else if (s == "КОМПАНІЯ" || s == "ФІРМА") 
                    t.Coeff = 1;
                if (s.StartsWith("МЕДІА")) 
                    t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Media);
                m_Global.Add(t);
            }
            t = new OrgItemTypeTermin("ЭКОЛОГИЧЕСКАЯ ГРУППА", Pullenti.Morph.MorphLang.RU) { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanHasLatinName = true };
            t.AddVariant("ЭКОГРУППА", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("РОК ГРУППА", Pullenti.Morph.MorphLang.RU, Pullenti.Ner.Org.OrgProfile.Music) { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanHasLatinName = true };
            t.AddVariant("РОКГРУППА", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ПАНК ГРУППА", Pullenti.Morph.MorphLang.RU, Pullenti.Ner.Org.OrgProfile.Music) { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanHasLatinName = true };
            t.AddVariant("ПАНКГРУППА", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ОРКЕСТР", Pullenti.Morph.MorphLang.RU, Pullenti.Ner.Org.OrgProfile.Music) { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanHasLatinName = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ХОР", Pullenti.Morph.MorphLang.RU, Pullenti.Ner.Org.OrgProfile.Music) { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanHasLatinName = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("МУЗЫКАЛЬНЫЙ КОЛЛЕКТИВ", Pullenti.Morph.MorphLang.RU, Pullenti.Ner.Org.OrgProfile.Music) { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanHasLatinName = true };
            t.AddVariant("РОКГРУППА", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ВОКАЛЬНО ИНСТРУМЕНТАЛЬНЫЙ АНСАМБЛЬ", Pullenti.Morph.MorphLang.RU, Pullenti.Ner.Org.OrgProfile.Music) { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanHasLatinName = true, Acronym = "ВИА" };
            t.AddVariant("ИНСТРУМЕНТАЛЬНЫЙ АНСАМБЛЬ", false);
            m_Global.Add(t);
            foreach (string s in new string[] {"НОТАРИАЛЬНАЯ КОНТОРА", "АДВОКАТСКОЕ БЮРО", "СТРАХОВОЕ ОБЩЕСТВО", "ЮРИДИЧЕСКИЙ ДОМ"}) 
            {
                t = new OrgItemTypeTermin(s) { Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"НОТАРІАЛЬНА КОНТОРА", "АДВОКАТСЬКЕ БЮРО", "СТРАХОВЕ ТОВАРИСТВО"}) 
            {
                t = new OrgItemTypeTermin(s) { Lang = Pullenti.Morph.MorphLang.UA, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ГАЗЕТА", "ЕЖЕНЕДЕЛЬНИК", "ТАБЛОИД", "ЕЖЕНЕДЕЛЬНЫЙ ЖУРНАЛ", "NEWSPAPER", "WEEKLY", "TABLOID", "MAGAZINE"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, Profile = Pullenti.Ner.Org.OrgProfile.Media };
                t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Press);
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ГАЗЕТА", "ТИЖНЕВИК", "ТАБЛОЇД"}) 
            {
                t = new OrgItemTypeTermin(s) { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, Profile = Pullenti.Ner.Org.OrgProfile.Media };
                t.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Press);
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"РАДИОСТАНЦИЯ", "РАДИО", "ТЕЛЕКАНАЛ", "ТЕЛЕКОМПАНИЯ", "НОВОСТНОЙ ПОРТАЛ", "ИНТЕРНЕТ ПОРТАЛ", "ИНТЕРНЕТ ИЗДАНИЕ", "ИНТЕРНЕТ РЕСУРС"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, Profile = Pullenti.Ner.Org.OrgProfile.Media };
                if (s == "РАДИО") 
                {
                    t.CanonicText = "РАДИОСТАНЦИЯ";
                    t.IsDoubtWord = true;
                }
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"РАДІО", "РАДІО", "ТЕЛЕКАНАЛ", "ТЕЛЕКОМПАНІЯ", "НОВИННИЙ ПОРТАЛ", "ІНТЕРНЕТ ПОРТАЛ", "ІНТЕРНЕТ ВИДАННЯ", "ІНТЕРНЕТ РЕСУРС"}) 
            {
                t = new OrgItemTypeTermin(s) { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, Profile = Pullenti.Ner.Org.OrgProfile.Media };
                if (s == "РАДІО") 
                {
                    t.CanonicText = "РАДІОСТАНЦІЯ";
                    t.IsDoubtWord = true;
                }
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ПАНСИОНАТ", "САНАТОРИЙ", "ДОМ ОТДЫХА", "ОТЕЛЬ", "ГОСТИНИЦА", "SPA-ОТЕЛЬ", "ОЗДОРОВИТЕЛЬНЫЙ ЛАГЕРЬ", "ДЕТСКИЙ ЛАГЕРЬ", "ПИОНЕРСКИЙ ЛАГЕРЬ", "БАЗА ОТДЫХА", "СПОРТ-КЛУБ"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true };
                if (s == "САНАТОРИЙ") 
                    t.AddAbridge("САН.");
                else if (s == "ДОМ ОТДЫХА") 
                {
                    t.AddAbridge("Д.О.");
                    t.AddAbridge("ДОМ ОТД.");
                    t.AddAbridge("Д.ОТД.");
                }
                else if (s == "ПАНСИОНАТ") 
                    t.AddAbridge("ПАНС.");
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ПАНСІОНАТ", "САНАТОРІЙ", "БУДИНОК ВІДПОЧИНКУ", "ГОТЕЛЬ", "SPA-ГОТЕЛЬ", "ОЗДОРОВЧИЙ ТАБІР", "БАЗА ВІДПОЧИНКУ", "СПОРТ-КЛУБ"}) 
            {
                t = new OrgItemTypeTermin(s) { Lang = Pullenti.Morph.MorphLang.UA, Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true };
                if (s == "САНАТОРІЙ") 
                    t.AddAbridge("САН.");
                m_Global.Add(t);
            }
            m_Global.Add(new OrgItemTypeTermin("ДЕТСКИЙ ОЗДОРОВИТЕЛЬНЫЙ ЛАГЕРЬ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "ДОЛ", CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true });
            m_Global.Add(new OrgItemTypeTermin("ДЕТСКИЙ СПОРТИВНЫЙ ОЗДОРОВИТЕЛЬНЫЙ ЛАГЕРЬ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "ДСОЛ", CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true });
            foreach (string s in new string[] {"КОЛХОЗ", "САДОВО ОГОРОДНОЕ ТОВАРИЩЕСТВО", "КООПЕРАТИВ", "ФЕРМЕРСКОЕ ХОЗЯЙСТВО", "КРЕСТЬЯНСКО ФЕРМЕРСКОЕ ХОЗЯЙСТВО", "АГРОФИРМА", "КОНЕЗАВОД", "ПТИЦЕФЕРМА", "СВИНОФЕРМА", "ФЕРМА", "ЛЕСПРОМХОЗ"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"КОЛГОСП", "САДОВО ГОРОДНЄ ТОВАРИСТВО", "КООПЕРАТИВ", "ФЕРМЕРСЬКЕ ГОСПОДАРСТВО", "СЕЛЯНСЬКО ФЕРМЕРСЬКЕ ГОСПОДАРСТВО", "АГРОФІРМА", "КОНЕЗАВОД", "ПТАХОФЕРМА", "СВИНОФЕРМА", "ФЕРМА"}) 
            {
                t = new OrgItemTypeTermin(s, Pullenti.Morph.MorphLang.UA) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
                m_Global.Add(t);
            }
            t = new OrgItemTypeTermin("ЖИЛИЩНО КОММУНАЛЬНОЕ ХОЗЯЙСТВО") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "ЖКХ", CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ЖИТЛОВО КОМУНАЛЬНЕ ГОСПОДАРСТВО") { Coeff = 3, Typ = OrgItemTypeTyp.Org, Acronym = "ЖКГ", CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("КОММУНАЛЬНОЕ ПРЕДПРИЯТИЕ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("КОМУНАЛЬНЕ ПІДПРИЄМСТВО", Pullenti.Morph.MorphLang.UA) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, CanBeSingleGeo = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("АВТОМОБИЛЬНЫЙ ЗАВОД") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
            t.AddVariant("АВТОЗАВОД", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("АВТОМОБИЛЬНЫЙ ЦЕНТР") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
            t.AddVariant("АВТОЦЕНТР", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("СОВХОЗ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
            t.AddAbridge("С/Х");
            t.AddAbridge("С-З");
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ПЛЕМЕННОЕ ХОЗЯЙСТВО") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
            t.AddVariant("ПЛЕМХОЗ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ЛЕСНОЕ ХОЗЯЙСТВО") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
            t.AddVariant("ЛЕСХОЗ", false);
            m_Global.Add(t);
            string[] sads = new string[] {"Садоводческое некоммерческое товарищество", "СНТ", "Дачное некоммерческое товарищество", "ДНТ", "Огородническое некоммерческое товарищество", "ОНТ", "Садоводческое некоммерческое партнерство", "СНП", "Дачное некоммерческое партнерство", "ДНП", "Огородническое некоммерческое партнерство", "ОНП", "Садоводческий потребительский кооператив", "СПК", "Дачный потребительский кооператив", "ДПК", "Огороднический потребительский кооператив", "ОПК"};
            for (int i = 0; i < sads.Length; i += 2) 
            {
                t = new OrgItemTypeTermin(sads[i].ToUpper()) { Coeff = 3, Acronym = sads[i + 1], Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, CanHasNumber = true };
                t.AddAbridge(sads[i + 1]);
                if (t.Acronym == "СНТ") 
                    t.AddAbridge("САДОВ.НЕКОМ.ТОВ.");
                m_Global.Add(t);
            }
            t = new OrgItemTypeTermin("САДОВОДЧЕСКОЕ ТОВАРИЩЕСТВО") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
            t.AddAbridge("САДОВОДЧ.ТОВ.");
            t.AddAbridge("САДОВ.ТОВ.");
            t.AddAbridge("САД.ТОВ.");
            t.AddAbridge("С.Т.");
            t.AddVariant("САДОВОЕ ТОВАРИЩЕСТВО", false);
            t.AddVariant("САДОВ. ТОВАРИЩЕСТВО", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("САДОВОДЧЕСКИЙ КООПЕРАТИВ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
            t.AddAbridge("САДОВОДЧ.КООП.");
            t.AddAbridge("САДОВ.КООП.");
            t.AddVariant("САДОВЫЙ КООПЕРАТИВ", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ДАЧНОЕ ТОВАРИЩЕСТВО") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasSingleName = true, MustHasCapitalName = true, CanHasNumber = true };
            t.AddAbridge("ДАЧН.ТОВ.");
            t.AddAbridge("ДАЧ.ТОВ.");
            m_Global.Add(t);
            foreach (string s in new string[] {"ФЕСТИВАЛЬ", "ЧЕМПИОНАТ", "ОЛИМПИАДА", "КОНКУРС"}) 
            {
                t = new OrgItemTypeTermin(s) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true };
                m_Global.Add(t);
            }
            foreach (string s in new string[] {"ФЕСТИВАЛЬ", "ЧЕМПІОНАТ", "ОЛІМПІАДА"}) 
            {
                t = new OrgItemTypeTermin(s, Pullenti.Morph.MorphLang.UA) { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true };
                m_Global.Add(t);
            }
            t = new OrgItemTypeTermin("ПОГРАНИЧНЫЙ ПОСТ") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
            t.AddVariant("ПОГП", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ПОГРАНИЧНАЯ ЗАСТАВА") { Coeff = 3, Typ = OrgItemTypeTyp.Org, CanHasLatinName = true, CanHasNumber = true, Profile = Pullenti.Ner.Org.OrgProfile.Army };
            t.AddVariant("ПОГЗ", false);
            t.AddVariant("ПОГРАНЗАСТАВА", false);
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ТЕРРИТОРИАЛЬНЫЙ ПУНКТ") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanHasNumber = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("МИГРАЦИОННЫЙ ПУНКТ") { Coeff = 3, Typ = OrgItemTypeTyp.Dep, CanHasNumber = true };
            m_Global.Add(t);
            t = new OrgItemTypeTermin("ПРОПУСКНОЙ ПУНКТ") { Coeff = 3, CanBeNormalDep = true, Typ = OrgItemTypeTyp.Dep, CanHasNumber = true, CanBeSingleGeo = true };
            t.AddVariant("ПУНКТ ПРОПУСКА", false);
            t.AddVariant("КОНТРОЛЬНО ПРОПУСКНОЙ ПУНКТ", false);
            m_Global.Add(t);
            m_PrefWords = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"КАПИТАЛ", "РУКОВОДСТВО", "СЪЕЗД", "СОБРАНИЕ", "СОВЕТ", "УПРАВЛЕНИЕ", "ДЕПАРТАМЕНТ"}) 
            {
                m_PrefWords.Add(new Pullenti.Ner.Core.Termin(s));
            }
            foreach (string s in new string[] {"КАПІТАЛ", "КЕРІВНИЦТВО", "ЗЇЗД", "ЗБОРИ", "РАДА", "УПРАВЛІННЯ"}) 
            {
                m_PrefWords.Add(new Pullenti.Ner.Core.Termin(s) { Lang = Pullenti.Morph.MorphLang.UA });
            }
            foreach (string s in new string[] {"АКЦИЯ", "ВЛАДЕЛЕЦ", "ВЛАДЕЛИЦА", "СОВЛАДЕЛЕЦ", "СОВЛАДЕЛИЦА", "КОНКУРЕНТ"}) 
            {
                m_PrefWords.Add(new Pullenti.Ner.Core.Termin(s) { Tag = s });
            }
            foreach (string s in new string[] {"АКЦІЯ", "ВЛАСНИК", "ВЛАСНИЦЯ", "СПІВВЛАСНИК", "СПІВВЛАСНИЦЯ", "КОНКУРЕНТ"}) 
            {
                m_PrefWords.Add(new Pullenti.Ner.Core.Termin(s) { Tag = s, Lang = Pullenti.Morph.MorphLang.UA });
            }
            for (int k = 0; k < 3; k++) 
            {
                string name = (k == 0 ? "pattrs_ru.dat" : (k == 1 ? "pattrs_ua.dat" : "pattrs_en.dat"));
                byte[] dat = ResourceHelper.GetBytes(name);
                if (dat == null) 
                    throw new Exception(string.Format("Can't file resource file {0} in Organization analyzer", name));
                using (MemoryStream tmp = new MemoryStream(Deflate(dat))) 
                {
                    tmp.Position = 0;
                    XmlDocument xml = new XmlDocument();
                    xml.Load(tmp);
                    foreach (XmlNode x in xml.DocumentElement.ChildNodes) 
                    {
                        if (k == 0) 
                            m_PrefWords.Add(new Pullenti.Ner.Core.Termin(x.InnerText) { Tag = 1 });
                        else if (k == 1) 
                            m_PrefWords.Add(new Pullenti.Ner.Core.Termin(x.InnerText) { Tag = 1, Lang = Pullenti.Morph.MorphLang.UA });
                        else if (k == 2) 
                            m_PrefWords.Add(new Pullenti.Ner.Core.Termin(x.InnerText) { Tag = 1, Lang = Pullenti.Morph.MorphLang.EN });
                    }
                }
            }
            m_KeyWordsForRefs = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"КОМПАНИЯ", "ФИРМА", "ПРЕДПРИЯТИЕ", "КОРПОРАЦИЯ", "ВЕДОМСТВО", "УЧРЕЖДЕНИЕ", "КОМПАНІЯ", "ФІРМА", "ПІДПРИЄМСТВО", "КОРПОРАЦІЯ", "ВІДОМСТВО", "УСТАНОВА"}) 
            {
                m_KeyWordsForRefs.Add(new Pullenti.Ner.Core.Termin(s));
            }
            foreach (string s in new string[] {"ЧАСТЬ", "БАНК", "ЗАВОД", "ФАБРИКА", "АЭРОПОРТ", "БИРЖА", "СЛУЖБА", "МИНИСТЕРСТВО", "КОМИССИЯ", "КОМИТЕТ", "ГРУППА", "ЧАСТИНА", "БАНК", "ЗАВОД", "ФАБРИКА", "АЕРОПОРТ", "БІРЖА", "СЛУЖБА", "МІНІСТЕРСТВО", "КОМІСІЯ", "КОМІТЕТ", "ГРУПА"}) 
            {
                m_KeyWordsForRefs.Add(new Pullenti.Ner.Core.Termin(s) { Tag = s });
            }
            m_Markers = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"МОРСКОЙ", "ВОЗДУШНЫЙ;ВОЗДУШНО", "ДЕСАНТНЫЙ;ДЕСАНТНО", "ТАНКОВЫЙ", "АРТИЛЛЕРИЙСКИЙ", "АВИАЦИОННЫЙ", "КОСМИЧЕСКИЙ", "РАКЕТНЫЙ;РАКЕТНО", "БРОНЕТАНКОВЫЙ", "КАВАЛЕРИЙСКИЙ", "СУХОПУТНЫЙ", "ПЕХОТНЫЙ;ПЕХОТНО", "МОТОПЕХОТНЫЙ", "МИНОМЕТНЫЙ", "МОТОСТРЕЛКОВЫЙ", "СТРЕЛКОВЫЙ", "ПРОТИВОРАКЕТНЫЙ", "ПРОТИВОВОЗДУШНЫЙ", "ШТУРМОВОЙ"}) 
            {
                string[] ss = s.Split(';');
                t = new OrgItemTypeTermin(ss[0]);
                if (ss.Length > 1) 
                    t.AddVariant(ss[1], false);
                m_Markers.Add(t);
            }
            m_StdAdjs = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"РОССИЙСКИЙ", "ВСЕРОССИЙСКИЙ", "МЕЖДУНАРОДНЫЙ", "ВСЕМИРНЫЙ", "ЕВРОПЕЙСКИЙ", "ГОСУДАРСТВЕННЫЙ", "НЕГОСУДАРСТВЕННЫЙ", "ФЕДЕРАЛЬНЫЙ", "РЕГИОНАЛЬНЫЙ", "ОБЛАСТНОЙ", "ГОРОДСКОЙ", "МУНИЦИПАЛЬНЫЙ", "АВТОНОМНЫЙ", "НАЦИОНАЛЬНЫЙ", "МЕЖРАЙОННЫЙ", "РАЙОННЫЙ", "ОТРАСЛЕВОЙ", "МЕЖОТРАСЛЕВОЙ", "НАРОДНЫЙ", "ВЕРХОВНЫЙ", "УКРАИНСКИЙ", "ВСЕУКРАИНСКИЙ", "РУССКИЙ"}) 
            {
                m_StdAdjs.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.RU) { Tag = s });
            }
            m_StdAdjsUA = new Pullenti.Ner.Core.TerminCollection();
            foreach (string s in new string[] {"РОСІЙСЬКИЙ", "ВСЕРОСІЙСЬКИЙ", "МІЖНАРОДНИЙ", "СВІТОВИЙ", "ЄВРОПЕЙСЬКИЙ", "ДЕРЖАВНИЙ", "НЕДЕРЖАВНИЙ", "ФЕДЕРАЛЬНИЙ", "РЕГІОНАЛЬНИЙ", "ОБЛАСНИЙ", "МІСЬКИЙ", "МУНІЦИПАЛЬНИЙ", "АВТОНОМНИЙ", "НАЦІОНАЛЬНИЙ", "МІЖРАЙОННИЙ", "РАЙОННИЙ", "ГАЛУЗЕВИЙ", "МІЖГАЛУЗЕВИЙ", "НАРОДНИЙ", "ВЕРХОВНИЙ", "УКРАЇНСЬКИЙ", "ВСЕУКРАЇНСЬКИЙ", "РОСІЙСЬКА"}) 
            {
                m_StdAdjsUA.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.UA) { Tag = s });
            }
            foreach (string s in new string[] {"КОММЕРЧЕСКИЙ", "НЕКОММЕРЧЕСКИЙ", "БЮДЖЕТНЫЙ", "КАЗЕННЫЙ", "БЛАГОТВОРИТЕЛЬНЫЙ", "СОВМЕСТНЫЙ", "ИНОСТРАННЫЙ", "ИССЛЕДОВАТЕЛЬСКИЙ", "ОБРАЗОВАТЕЛЬНЫЙ", "ОБЩЕОБРАЗОВАТЕЛЬНЫЙ", "ВЫСШИЙ", "УЧЕБНЫЙ", "СПЕЦИАЛИЗИРОВАННЫЙ", "ГЛАВНЫЙ", "ЦЕНТРАЛЬНЫЙ", "ТЕХНИЧЕСКИЙ", "ТЕХНОЛОГИЧЕСКИЙ", "ВОЕННЫЙ", "ПРОМЫШЛЕННЫЙ", "ТОРГОВЫЙ", "СИНОДАЛЬНЫЙ", "МЕДИЦИНСКИЙ", "ДИАГНОСТИЧЕСКИЙ", "ДЕТСКИЙ", "АКАДЕМИЧЕСКИЙ", "ПОЛИТЕХНИЧЕСКИЙ", "ИНВЕСТИЦИОННЫЙ", "ТЕРРОРИСТИЧЕСКИЙ", "РАДИКАЛЬНЫЙ", "ИСЛАМИСТСКИЙ", "ЛЕВОРАДИКАЛЬНЫЙ", "ПРАВОРАДИКАЛЬНЫЙ", "ОППОЗИЦИОННЫЙ", "НАЛОГОВЫЙ", "КРИМИНАЛЬНЫЙ", "СПОРТИВНЫЙ", "НЕФТЯНОЙ", "ГАЗОВЫЙ", "ВЕЛИКИЙ"}) 
            {
                m_StdAdjs.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.RU));
            }
            foreach (string s in new string[] {"КОМЕРЦІЙНИЙ", "НЕКОМЕРЦІЙНИЙ", "БЮДЖЕТНИЙ", "КАЗЕННИМ", "БЛАГОДІЙНИЙ", "СПІЛЬНИЙ", "ІНОЗЕМНИЙ", "ДОСЛІДНИЦЬКИЙ", "ОСВІТНІЙ", "ЗАГАЛЬНООСВІТНІЙ", "ВИЩИЙ", "НАВЧАЛЬНИЙ", "СПЕЦІАЛІЗОВАНИЙ", "ГОЛОВНИЙ", "ЦЕНТРАЛЬНИЙ", "ТЕХНІЧНИЙ", "ТЕХНОЛОГІЧНИЙ", "ВІЙСЬКОВИЙ", "ПРОМИСЛОВИЙ", "ТОРГОВИЙ", "СИНОДАЛЬНИЙ", "МЕДИЧНИЙ", "ДІАГНОСТИЧНИЙ", "ДИТЯЧИЙ", "АКАДЕМІЧНИЙ", "ПОЛІТЕХНІЧНИЙ", "ІНВЕСТИЦІЙНИЙ", "ТЕРОРИСТИЧНИЙ", "РАДИКАЛЬНИЙ", "ІСЛАМІЗМ", "ЛІВОРАДИКАЛЬНИЙ", "ПРАВОРАДИКАЛЬНИЙ", "ОПОЗИЦІЙНИЙ", "ПОДАТКОВИЙ", "КРИМІНАЛЬНИЙ", " СПОРТИВНИЙ", "НАФТОВИЙ", "ГАЗОВИЙ", "ВЕЛИКИЙ"}) 
            {
                m_StdAdjsUA.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.UA));
            }
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
        }
        internal static byte[] Deflate(byte[] zip)
        {
            using (MemoryStream unzip = new MemoryStream()) 
            {
                MemoryStream data = new MemoryStream(zip);
                data.Position = 0;
                Pullenti.Morph.Internal.MorphDeserializer.DeflateGzip(data, unzip);
                data.Dispose();
                return unzip.ToArray();
            }
        }
        public static string[] m_EmptyTypWords = new string[] {"КРУПНЫЙ", "КРУПНЕЙШИЙ", "ИЗВЕСТНЫЙ", "ИЗВЕСТНЕЙШИЙ", "МАЛОИЗВЕСТНЫЙ", "ЗАРУБЕЖНЫЙ", "ВЛИЯТЕЛЬНЫЙ", "ВЛИЯТЕЛЬНЕЙШИЙ", "ЗНАМЕНИТЫЙ", "НАЙБІЛЬШИЙ", "ВІДОМИЙ", "ВІДОМИЙ", "МАЛОВІДОМИЙ", "ЗАКОРДОННИЙ"};
        static string[] m_DecreeKeyWords = new string[] {"УКАЗ", "УКАЗАНИЕ", "ПОСТАНОВЛЕНИЕ", "РАСПОРЯЖЕНИЕ", "ПРИКАЗ", "ДИРЕКТИВА", "ПИСЬМО", "ЗАКОН", "КОДЕКС", "КОНСТИТУЦИЯ", "РЕШЕНИЕ", "ПОЛОЖЕНИЕ", "РАСПОРЯЖЕНИЕ", "ПОРУЧЕНИЕ", "ДОГОВОР", "СУБДОГОВОР", "АГЕНТСКИЙ ДОГОВОР", "ОПРЕДЕЛЕНИЕ", "СОГЛАШЕНИЕ", "ПРОТОКОЛ", "УСТАВ", "ХАРТИЯ", "РЕГЛАМЕНТ", "КОНВЕНЦИЯ", "ПАКТ", "БИЛЛЬ", "ДЕКЛАРАЦИЯ", "ТЕЛЕФОНОГРАММА", "ТЕЛЕФАКСОГРАММА", "ФАКСОГРАММА", "ПРАВИЛО", "ПРОГРАММА", "ПЕРЕЧЕНЬ", "ПОСОБИЕ", "РЕКОМЕНДАЦИЯ", "НАСТАВЛЕНИЕ", "СТАНДАРТ", "СОГЛАШЕНИЕ", "МЕТОДИКА", "ТРЕБОВАНИЕ", "УКАЗ", "ВКАЗІВКА", "ПОСТАНОВА", "РОЗПОРЯДЖЕННЯ", "НАКАЗ", "ДИРЕКТИВА", "ЛИСТ", "ЗАКОН", "КОДЕКС", "КОНСТИТУЦІЯ", "РІШЕННЯ", "ПОЛОЖЕННЯ", "РОЗПОРЯДЖЕННЯ", "ДОРУЧЕННЯ", "ДОГОВІР", "СУБКОНТРАКТ", "АГЕНТСЬКИЙ ДОГОВІР", "ВИЗНАЧЕННЯ", "УГОДА", "ПРОТОКОЛ", "СТАТУТ", "ХАРТІЯ", "РЕГЛАМЕНТ", "КОНВЕНЦІЯ", "ПАКТ", "БІЛЛЬ", "ДЕКЛАРАЦІЯ", "ТЕЛЕФОНОГРАМА", "ТЕЛЕФАКСОГРАММА", "ФАКСОГРАМА", "ПРАВИЛО", "ПРОГРАМА", "ПЕРЕЛІК", "ДОПОМОГА", "РЕКОМЕНДАЦІЯ", "ПОВЧАННЯ", "СТАНДАРТ", "УГОДА", "МЕТОДИКА", "ВИМОГА"};
        public static bool IsDecreeKeyword(Pullenti.Ner.Token t, int cou = 1)
        {
            if (t == null) 
                return false;
            for (int i = 0; (i < cou) && t != null; i++,t = t.Previous) 
            {
                if (t.IsNewlineAfter) 
                    break;
                if (!t.Chars.IsCyrillicLetter) 
                    break;
                foreach (string d in m_DecreeKeyWords) 
                {
                    if (t.IsValue(d, null)) 
                        return true;
                }
            }
            return false;
        }
        internal OrgItemTypeToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public string Typ;
        public string Name;
        public string AltName;
        public bool NameIsName;
        public string AltTyp;
        public string Number;
        public List<Pullenti.Ner.Org.OrgProfile> Profiles
        {
            get
            {
                if (m_Profile == null) 
                {
                    m_Profile = new List<Pullenti.Ner.Org.OrgProfile>();
                    if (Root != null) 
                        m_Profile.AddRange(Root.Profiles);
                }
                return m_Profile;
            }
            set
            {
                m_Profile = value;
            }
        }
        List<Pullenti.Ner.Org.OrgProfile> m_Profile;
        public OrgItemTypeTermin Root;
        public bool IsDep
        {
            get
            {
                if (m_IsDep >= 0) 
                    return m_IsDep > 0;
                if (Root == null) 
                    return false;
                if (Root.Profiles.Contains(Pullenti.Ner.Org.OrgProfile.Unit)) 
                    return true;
                return false;
            }
            set
            {
                m_IsDep = (value ? 1 : 0);
            }
        }
        int m_IsDep = -1;
        public bool IsNotTyp = false;
        public float Coef
        {
            get
            {
                if (m_Coef >= 0) 
                    return m_Coef;
                if (Root != null) 
                    return Root.Coeff;
                return 0;
            }
            set
            {
                m_Coef = value;
            }
        }
        float m_Coef = -1;
        public Pullenti.Ner.ReferentToken Geo;
        public Pullenti.Ner.ReferentToken Geo2;
        public Pullenti.Morph.CharsInfo CharsRoot = new Pullenti.Morph.CharsInfo();
        public int NameWordsCount
        {
            get
            {
                int cou = 1;
                if (Name == null) 
                    return 1;
                for (int i = 0; i < Name.Length; i++) 
                {
                    if (Name[i] == ' ') 
                        cou++;
                }
                return cou;
            }
        }
        public bool CanBeDepBeforeOrganization;
        public bool IsDouterOrg;
        public bool IsDoubtRootWord
        {
            get
            {
                if (m_IsDoubtRootWord >= 0) 
                    return m_IsDoubtRootWord == 1;
                if (Root == null) 
                    return false;
                return Root.IsDoubtWord;
            }
            set
            {
                m_IsDoubtRootWord = (value ? 1 : 0);
            }
        }
        int m_IsDoubtRootWord = -1;
        public bool CanBeOrganization;
        public override string ToString()
        {
            if (Name != null) 
                return Name;
            else 
                return Typ;
        }
        public static OrgItemTypeToken TryAttach(Pullenti.Ner.Token t, bool canBeFirstLetterLower = false, Pullenti.Ner.Core.AnalyzerDataWithOntology ad = null)
        {
            if (t == null || (((t is Pullenti.Ner.ReferentToken) && !t.Chars.IsLatinLetter))) 
                return null;
            OrgItemTypeToken res = _TryAttach(t, canBeFirstLetterLower);
            if (res != null) 
            {
            }
            if ((res == null && (t is Pullenti.Ner.NumberToken) && (t.WhitespacesAfterCount < 3)) && t.Next != null && t.Next.IsValue("СЛУЖБА", null)) 
            {
                res = _TryAttach(t.Next, canBeFirstLetterLower);
                if (res == null) 
                    return null;
                res.Number = (t as Pullenti.Ner.NumberToken).Value;
                res.BeginToken = t;
                return res;
            }
            if (res == null && t.Chars.IsLatinLetter) 
            {
                if (t.IsValue("THE", null)) 
                {
                    OrgItemTypeToken res1 = TryAttach(t.Next, canBeFirstLetterLower, null);
                    if (res1 != null) 
                    {
                        res1.BeginToken = t;
                        return res1;
                    }
                    return null;
                }
                if ((t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && (t.Next is Pullenti.Ner.TextToken) && t.Next.Chars.IsLatinLetter) 
                {
                    OrgItemTypeToken res1 = TryAttach(t.Next, canBeFirstLetterLower, null);
                    if (res1 != null) 
                    {
                        res1.BeginToken = t;
                        res1.Geo = t as Pullenti.Ner.ReferentToken;
                        res1.Name = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(res1, Pullenti.Ner.Core.GetTextAttr.No);
                        return res1;
                    }
                }
                if (t.Chars.IsCapitalUpper) 
                {
                    Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                    if ((mc.IsConjunction || mc.IsPreposition || mc.IsMisc) || mc.IsPronoun || mc.IsPersonalPronoun) 
                    {
                    }
                    else 
                        for (Pullenti.Ner.Token ttt = t.Next; ttt != null; ttt = ttt.Next) 
                        {
                            if (!ttt.Chars.IsLatinLetter) 
                                break;
                            if (ttt.WhitespacesBeforeCount > 3) 
                                break;
                            if (Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(ttt.Next)) 
                            {
                                ttt = ttt.Next.Next.Next;
                                if (ttt == null) 
                                    break;
                            }
                            OrgItemTypeToken res1 = _TryAttach(ttt, true);
                            if (res1 != null) 
                            {
                                res1.Name = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, res1.EndToken, Pullenti.Ner.Core.GetTextAttr.IgnoreArticles);
                                if (res1.Coef < 5) 
                                    res1.Coef = 5;
                                res1.BeginToken = t;
                                return res1;
                            }
                            if (ttt.Chars.IsAllLower && !ttt.IsAnd) 
                                break;
                            if (ttt.WhitespacesBeforeCount > 1) 
                                break;
                        }
                }
            }
            if ((res != null && res.Name != null && res.Name.StartsWith("СОВМЕСТ")) && Pullenti.Morph.LanguageHelper.EndsWithEx(res.Name, "ПРЕДПРИЯТИЕ", "КОМПАНИЯ", null, null)) 
            {
                res.Root = m_SovmPred;
                res.Typ = "совместное предприятие";
                for (Pullenti.Ner.Token tt1 = t.Next; tt1 != null && tt1.EndChar <= res.EndToken.BeginChar; tt1 = tt1.Next) 
                {
                    Pullenti.Ner.ReferentToken rt = tt1.Kit.ProcessReferent("GEO", tt1);
                    if (rt != null) 
                    {
                        res.Coef += 0.5F;
                        if (res.Geo == null) 
                            res.Geo = rt;
                        else if (res.Geo.Referent.CanBeEquals(rt.Referent, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                        {
                        }
                        else if (res.Geo2 == null) 
                            res.Geo2 = rt;
                        tt1 = rt.EndToken;
                    }
                }
            }
            if (((((res != null && res.BeginToken.LengthChar <= 2 && !res.BeginToken.Chars.IsAllLower) && res.BeginToken.Next != null && res.BeginToken.Next.IsChar('.')) && res.BeginToken.Next.Next != null && res.BeginToken.Next.Next.LengthChar <= 2) && !res.BeginToken.Next.Next.Chars.IsAllLower && res.BeginToken.Next.Next.Next != null) && res.BeginToken.Next.Next.Next.IsChar('.') && res.EndToken == res.BeginToken.Next.Next.Next) 
                return null;
            if (res != null && res.Typ == "управление") 
            {
                if (res.Name != null && res.Name.Contains("ГОСУДАРСТВЕННОЕ")) 
                    return null;
                if (res.BeginToken.Previous != null && res.BeginToken.Previous.IsValue("ГОСУДАРСТВЕННЫЙ", null)) 
                    return null;
            }
            if (res != null && res.Geo == null && (res.BeginToken.Previous is Pullenti.Ner.TextToken)) 
            {
                Pullenti.Ner.ReferentToken rt = res.Kit.ProcessReferent("GEO", res.BeginToken.Previous);
                if (rt != null && rt.Morph.Class.IsAdjective) 
                {
                    if (res.BeginToken.Previous.Previous != null && res.BeginToken.Previous.Previous.IsValue("ОРДЕН", null)) 
                    {
                    }
                    else 
                    {
                        res.Geo = rt;
                        res.BeginToken = rt.BeginToken;
                    }
                }
            }
            if ((res != null && res.Typ == "комитет" && res.Geo == null) && res.EndToken.Next != null && (res.EndToken.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
            {
                res.Geo = res.EndToken.Next as Pullenti.Ner.ReferentToken;
                res.EndToken = res.EndToken.Next;
                res.Coef = 2;
                if (res.EndToken.Next != null && res.EndToken.Next.IsValue("ПО", null)) 
                    res.Coef += 1;
            }
            if ((res != null && res.Typ == "агентство" && res.Chars.IsCapitalUpper) && res.EndToken.Next != null && res.EndToken.Next.IsValue("ПО", null)) 
                res.Coef += 3;
            if (res != null && res.Geo != null) 
            {
                bool hasAdj = false;
                for (Pullenti.Ner.Token tt1 = res.BeginToken; tt1 != null && tt1.EndChar <= res.EndToken.BeginChar; tt1 = tt1.Next) 
                {
                    Pullenti.Ner.ReferentToken rt = tt1.Kit.ProcessReferent("GEO", tt1);
                    if (rt != null) 
                    {
                        if (res.Geo != null && res.Geo.Referent.CanBeEquals(rt.Referent, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            continue;
                        if (res.Geo2 != null && res.Geo2.Referent.CanBeEquals(rt.Referent, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            continue;
                        res.Coef += 0.5F;
                        if (res.Geo == null) 
                            res.Geo = rt;
                        else if (res.Geo2 == null) 
                            res.Geo2 = rt;
                        tt1 = rt.EndToken;
                    }
                    else if (tt1.GetMorphClassInDictionary().IsAdjective) 
                        hasAdj = true;
                }
                if ((res.Typ == "институт" || res.Typ == "академия" || res.Typ == "інститут") || res.Typ == "академія") 
                {
                    if (hasAdj) 
                    {
                        res.Coef += 2;
                        res.CanBeOrganization = true;
                    }
                }
            }
            if (res != null && res.Geo == null) 
            {
                Pullenti.Ner.Token tt2 = res.EndToken.Next;
                if (tt2 != null && !tt2.IsNewlineBefore && tt2.Morph.Class.IsPreposition) 
                {
                    if (((tt2.Next is Pullenti.Ner.TextToken) && (tt2.Next as Pullenti.Ner.TextToken).Term == "ВАШ" && res.Root != null) && res.Root.Profiles.Contains(Pullenti.Ner.Org.OrgProfile.Justice)) 
                    {
                        res.Coef = 5;
                        res.EndToken = tt2.Next;
                        tt2 = tt2.Next.Next;
                        res.Name = ((res.Name ?? res.Root.CanonicText)) + " ПО ВЗЫСКАНИЮ АДМИНИСТРАТИВНЫХ ШТРАФОВ";
                        res.Typ = "отдел";
                    }
                }
                if (tt2 != null && !tt2.IsNewlineBefore && tt2.Morph.Class.IsPreposition) 
                {
                    tt2 = tt2.Next;
                    if (tt2 != null && !tt2.IsNewlineBefore && (tt2.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                        res.EndToken = tt2;
                        res.Geo = tt2 as Pullenti.Ner.ReferentToken;
                        if ((tt2.Next != null && tt2.Next.IsAnd && (tt2.Next.Next is Pullenti.Ner.ReferentToken)) && (tt2.Next.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                        {
                            tt2 = tt2.Next.Next;
                            res.EndToken = tt2;
                            res.Geo2 = tt2 as Pullenti.Ner.ReferentToken;
                        }
                    }
                }
                else if (((tt2 != null && !tt2.IsNewlineBefore && tt2.IsHiphen) && (tt2.Next is Pullenti.Ner.TextToken) && tt2.Next.GetMorphClassInDictionary().IsNoun) && !tt2.Next.IsValue("БАНК", null)) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.EndToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt1 != null && npt1.EndToken == tt2.Next) 
                    {
                        res.AltTyp = npt1.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false).ToLower();
                        res.EndToken = npt1.EndToken;
                    }
                }
                else if (tt2 != null && (tt2.WhitespacesBeforeCount < 3)) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt2, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.Morph.Case.IsGenitive) 
                    {
                        Pullenti.Ner.ReferentToken rr = tt2.Kit.ProcessReferent("NAMEDENTITY", tt2);
                        if (rr != null && ((rr.Morph.Case.IsGenitive || rr.Morph.Case.IsUndefined)) && rr.Referent.FindSlot("KIND", "location", true) != null) 
                        {
                            if (((res.Root != null && res.Root.Typ == OrgItemTypeTyp.Dep)) || res.Typ == "департамент") 
                            {
                            }
                            else 
                                res.EndToken = rr.EndToken;
                        }
                        else if (res.Root != null && res.Root.Typ == OrgItemTypeTyp.Prefix && npt.EndToken.IsValue("ОБРАЗОВАНИЕ", null)) 
                        {
                            res.EndToken = npt.EndToken;
                            res.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Education);
                        }
                    }
                    if (((tt2.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && res.Root != null && res.Root.Typ == OrgItemTypeTyp.Prefix) && res.Geo == null) 
                    {
                        res.Geo = tt2 as Pullenti.Ner.ReferentToken;
                        res.EndToken = tt2;
                    }
                }
            }
            if (res != null && res.Typ != null && char.IsDigit(res.Typ[0])) 
            {
                int ii = res.Typ.IndexOf(' ');
                if (ii < (res.Typ.Length - 1)) 
                {
                    res.Number = res.Typ.Substring(0, ii);
                    res.Typ = res.Typ.Substring(ii + 1).Trim();
                }
            }
            if (res != null && res.Name != null && char.IsDigit(res.Name[0])) 
            {
                int ii = res.Name.IndexOf(' ');
                if (ii < (res.Name.Length - 1)) 
                {
                    res.Number = res.Name.Substring(0, ii);
                    res.Name = res.Name.Substring(ii + 1).Trim();
                }
            }
            if (res != null && res.Typ == "фонд") 
            {
                if (t.Previous != null && ((t.Previous.IsValue("ПРИЗОВОЙ", null) || t.Previous.IsValue("ЖИЛИЩНЫЙ", null)))) 
                    return null;
                if (res.BeginToken.IsValue("ПРИЗОВОЙ", null) || res.BeginToken.IsValue("ЖИЛИЩНЫЙ", null)) 
                    return null;
            }
            if (res != null && res.Typ == "милли меджлис") 
                res.Morph = new Pullenti.Ner.MorphCollection(res.EndToken.Morph);
            if (res != null && res.LengthChar == 2 && res.Typ == "АО") 
                res.IsDoubtRootWord = true;
            if (res != null && res.Typ == "администрация" && t.Next != null) 
            {
                if ((t.Next.IsChar('(') && t.Next.Next != null && ((t.Next.Next.IsValue("ПРАВИТЕЛЬСТВО", null) || t.Next.Next.IsValue("ГУБЕРНАТОР", null)))) && t.Next.Next.Next != null && t.Next.Next.Next.IsChar(')')) 
                {
                    res.EndToken = t.Next.Next.Next;
                    res.AltTyp = "правительство";
                    return res;
                }
                if (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                    res.AltTyp = "правительство";
            }
            if ((res != null && res.Typ == "ассоциация" && res.EndToken.Next != null) && (res.WhitespacesAfterCount < 2)) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    string str = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(npt, Pullenti.Ner.Core.GetTextAttr.No);
                    res.Name = string.Format("{0} {1}", res.Name ?? res.Typ.ToUpper(), str);
                    res.EndToken = npt.EndToken;
                    res.Coef += 1;
                }
            }
            if ((res != null && res.Typ == "представительство" && res.EndToken.Next != null) && (res.WhitespacesAfterCount < 2)) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    if (npt.EndToken.IsValue("ИНТЕРЕС", null)) 
                        return null;
                }
            }
            if (res != null && res.Name != null) 
            {
                if (res.Name.EndsWith(" ПОЛОК")) 
                    res.Name = res.Name.Substring(0, res.Name.Length - 5) + "ПОЛК";
            }
            if (res != null && ((res.Typ == "производитель" || res.Typ == "завод"))) 
            {
                Pullenti.Ner.Token tt1 = res.EndToken.Next;
                if (res.Typ == "завод") 
                {
                    if ((tt1 != null && tt1.IsValue("ПО", null) && tt1.Next != null) && tt1.Next.IsValue("ПРОИЗВОДСТВО", null)) 
                        tt1 = tt1.Next.Next;
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if ((npt != null && (res.WhitespacesAfterCount < 2) && tt1.Chars.IsAllLower) && npt.Morph.Case.IsGenitive) 
                {
                    string str = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(npt, Pullenti.Ner.Core.GetTextAttr.No);
                    res.Name = string.Format("{0} {1}", res.Name ?? res.Typ.ToUpper(), str);
                    if (res.Geo != null) 
                        res.Coef++;
                    res.EndToken = npt.EndToken;
                }
                else if (res.Typ != "завод") 
                    return null;
            }
            if (res != null && (res.BeginToken.Previous is Pullenti.Ner.TextToken) && ((res.Typ == "милиция" || res.Typ == "полиция"))) 
            {
            }
            if ((res != null && res.BeginToken == res.EndToken && (res.BeginToken is Pullenti.Ner.TextToken)) && (res.BeginToken as Pullenti.Ner.TextToken).Term == "ИП") 
            {
                if (!Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(res.EndToken.Next, true, false) && !Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(res.BeginToken.Previous, false, null, false)) 
                    return null;
            }
            if (res != null && res.Typ == "предприятие") 
            {
                if (res.AltTyp == "головное предприятие" || res.AltTyp == "дочернее предприятие") 
                    res.IsNotTyp = true;
                else if (t.Previous != null && ((t.Previous.IsValue("ГОЛОВНОЙ", null) || t.Previous.IsValue("ДОЧЕРНИЙ", null)))) 
                    return null;
            }
            if (res != null && res.IsDouterOrg) 
            {
                res.IsNotTyp = true;
                if (res.BeginToken != res.EndToken) 
                {
                    OrgItemTypeToken res1 = _TryAttach(res.BeginToken.Next, true);
                    if (res1 != null && !res1.IsDoubtRootWord) 
                        res.IsNotTyp = false;
                }
            }
            if (res != null && res.Typ == "суд") 
            {
                Pullenti.Ner.TextToken tt1 = res.EndToken as Pullenti.Ner.TextToken;
                if (tt1 != null && ((tt1.Term == "СУДА" || tt1.Term == "СУДОВ"))) 
                {
                    if (((res.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) != Pullenti.Morph.MorphNumber.Undefined) 
                        return null;
                }
            }
            if (res != null && res.Typ == "кафедра" && (t is Pullenti.Ner.TextToken)) 
            {
                if (t.IsValue("КАФЕ", null) && ((t.Next == null || !t.Next.IsChar('.')))) 
                    return null;
            }
            if (res != null && res.Typ == "компания") 
            {
                if ((t.Previous != null && t.Previous.IsHiphen && t.Previous.Previous != null) && t.Previous.Previous.IsValue("КАЮТ", null)) 
                    return null;
            }
            if (res != null && t.Previous != null) 
            {
                if (res.Morph.Case.IsGenitive) 
                {
                    if (t.Previous.IsValue("СТАНДАРТ", null)) 
                        return null;
                }
            }
            if (res != null && res.Typ == "радиостанция" && res.NameWordsCount > 1) 
                return null;
            if ((res != null && res.Typ == "предприятие" && res.AltTyp != null) && res.BeginToken.Morph.Class.IsAdjective && !res.Root.IsPurePrefix) 
            {
                res.Typ = res.AltTyp;
                res.AltTyp = null;
                res.Coef = 3;
            }
            if (res != null) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && ((npt.Noun.IsValue("ТИП", null) || npt.Noun.IsValue("РЕЖИМ", null))) && npt.Morph.Case.IsGenitive) 
                {
                    res.EndToken = npt.EndToken;
                    string s = string.Format("{0} {1}", res.Typ, Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(npt, Pullenti.Ner.Core.GetTextAttr.No)).ToLower();
                    if (res.Typ.Contains("колония") || res.Typ.Contains("тюрьма")) 
                    {
                        res.Coef = 3;
                        res.AltTyp = s;
                    }
                    else if (res.Name == null || res.Name.Length == res.Typ.Length) 
                        res.Name = s;
                    else 
                        res.AltTyp = s;
                }
            }
            if (res != null && res.Profiles.Contains(Pullenti.Ner.Org.OrgProfile.Education) && (res.EndToken.Next is Pullenti.Ner.TextToken)) 
            {
                Pullenti.Ner.Token tt1 = res.EndToken.Next;
                if ((tt1 as Pullenti.Ner.TextToken).Term == "ВПО" || (tt1 as Pullenti.Ner.TextToken).Term == "СПО") 
                    res.EndToken = res.EndToken.Next;
                else 
                {
                    Pullenti.Ner.Core.NounPhraseToken nnt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (nnt != null && nnt.EndToken.IsValue("ОБРАЗОВАНИЕ", "ОСВІТА")) 
                        res.EndToken = nnt.EndToken;
                }
            }
            if (res != null && res.Root != null && res.Root.IsPurePrefix) 
            {
                Pullenti.Ner.Token tt1 = res.EndToken.Next;
                if (tt1 != null && ((tt1.IsValue("С", null) || tt1.IsValue("C", null)))) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt1.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && ((npt.Noun.IsValue("ИНВЕСТИЦИЯ", null) || npt.Noun.IsValue("ОТВЕТСТВЕННОСТЬ", null)))) 
                        res.EndToken = npt.EndToken;
                }
            }
            if (res != null && res.Root == m_MilitaryUnit && res.EndToken.Next != null) 
            {
                if (res.EndToken.Next.IsValue("ПП", null)) 
                    res.EndToken = res.EndToken.Next;
                else if (res.EndToken.Next.IsValue("ПОЛЕВОЙ", null) && res.EndToken.Next.Next != null && res.EndToken.Next.Next.IsValue("ПОЧТА", null)) 
                    res.EndToken = res.EndToken.Next.Next;
            }
            if (res != null) 
            {
                if (res.NameWordsCount > 1 && res.Typ == "центр") 
                    res.CanBeDepBeforeOrganization = true;
                else if (Pullenti.Morph.LanguageHelper.EndsWith(res.Typ, " центр")) 
                    res.CanBeDepBeforeOrganization = true;
                if (t.IsValue("ГПК", null)) 
                {
                    if (res.Geo != null) 
                        return null;
                    Pullenti.Ner.ReferentToken gg = t.Kit.ProcessReferent("GEO", t.Next);
                    if (gg != null || !(t.Next is Pullenti.Ner.TextToken) || t.IsNewlineAfter) 
                        return null;
                    if (t.Next.Chars.IsAllUpper || Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Next, true, false)) 
                    {
                    }
                    else 
                        return null;
                }
            }
            if (res != null || !(t is Pullenti.Ner.TextToken)) 
                return res;
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            string term = tt.Term;
            if (tt.Chars.IsAllUpper && (((term == "CRM" || term == "IT" || term == "ECM") || term == "BPM" || term == "HR"))) 
            {
                Pullenti.Ner.Token tt2 = t.Next;
                if (tt2 != null && tt2.IsHiphen) 
                    tt2 = tt2.Next;
                res = _TryAttach(tt2, true);
                if (res != null && res.Root != null && res.Root.Profiles.Contains(Pullenti.Ner.Org.OrgProfile.Unit)) 
                {
                    res.Name = string.Format("{0} {1}", res.Name ?? res.Root.CanonicText, term);
                    res.BeginToken = t;
                    res.Coef = 5;
                    return res;
                }
            }
            if (term == "ВЧ") 
            {
                Pullenti.Ner.Token tt1 = t.Next;
                if (tt1 != null && tt1.IsValue("ПП", null)) 
                    res = new OrgItemTypeToken(t, tt1) { m_Coef = 3 };
                else if ((tt1 is Pullenti.Ner.NumberToken) && (tt1.WhitespacesBeforeCount < 3)) 
                    res = new OrgItemTypeToken(t, t);
                else if (Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(tt1) != null) 
                    res = new OrgItemTypeToken(t, t);
                else if (((tt1 is Pullenti.Ner.TextToken) && !tt1.IsWhitespaceAfter && tt1.Chars.IsLetter) && tt1.LengthChar == 1) 
                    res = new OrgItemTypeToken(t, t);
                if (res != null) 
                {
                    res.Root = m_MilitaryUnit;
                    res.Typ = m_MilitaryUnit.CanonicText.ToLower();
                    res.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Army);
                    return res;
                }
            }
            if (term == "КБ") 
            {
                int cou = 0;
                bool ok = false;
                for (Pullenti.Ner.Token ttt = t.Next; ttt != null && (cou < 30); ttt = ttt.Next,cou++) 
                {
                    if (ttt.IsValue("БАНК", null)) 
                    {
                        ok = true;
                        break;
                    }
                    Pullenti.Ner.Referent r = ttt.GetReferent();
                    if (r != null && r.TypeName == "URI") 
                    {
                        string vv = r.GetStringValue("SCHEME");
                        if ((vv == "БИК" || vv == "Р/С" || vv == "К/С") || vv == "ОКАТО") 
                        {
                            ok = true;
                            break;
                        }
                    }
                }
                if (ok) 
                {
                    res = new OrgItemTypeToken(t, t);
                    res.Typ = "коммерческий банк";
                    res.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Finance);
                    res.Coef = 3;
                    return res;
                }
            }
            if (term == "ТП" || term == "МП") 
            {
                OrgItemNumberToken num = OrgItemNumberToken.TryAttach(t.Next, true, null);
                if (num != null && num.EndToken.Next != null) 
                {
                    Pullenti.Ner.Token tt1 = num.EndToken.Next;
                    if (tt1.IsComma && tt1.Next != null) 
                        tt1 = tt1.Next;
                    Pullenti.Ner.Org.OrganizationReferent oo = tt1.GetReferent() as Pullenti.Ner.Org.OrganizationReferent;
                    if (oo != null) 
                    {
                        if (oo.ToString().ToUpper().Contains("МИГРАЦ")) 
                        {
                            res = new OrgItemTypeToken(t, t) { Typ = (term == "ТП" ? "территориальный пункт" : "миграционный пункт"), Coef = 4, IsDep = true };
                            return res;
                        }
                    }
                }
            }
            if (tt.Chars.IsAllUpper && term == "МГТУ") 
            {
                if (tt.Next.IsValue("БАНК", null) || (((tt.Next.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) && (tt.Next.GetReferent() as Pullenti.Ner.Org.OrganizationReferent).Kind == Pullenti.Ner.Org.OrganizationKind.Bank)) || ((tt.Previous != null && tt.Previous.IsValue("ОПЕРУ", null)))) 
                {
                    res = new OrgItemTypeToken(tt, tt) { Typ = "главное территориальное управление" };
                    res.AltTyp = "ГТУ";
                    res.Name = "МОСКОВСКОЕ";
                    res.NameIsName = true;
                    res.AltName = "МГТУ";
                    res.Coef = 3;
                    res.Root = new OrgItemTypeTermin(res.Name);
                    res.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Unit);
                    tt.Term = "МОСКОВСКИЙ";
                    res.Geo = tt.Kit.ProcessReferent("GEO", tt);
                    tt.Term = "МГТУ";
                    return res;
                }
            }
            if (tt.IsValue("СОВЕТ", "РАДА")) 
            {
                if (tt.Next != null && tt.Next.IsValue("ПРИ", null)) 
                {
                    Pullenti.Ner.ReferentToken rt = tt.Kit.ProcessReferent("PERSONPROPERTY", tt.Next.Next);
                    if (rt != null) 
                    {
                        res = new OrgItemTypeToken(tt, tt);
                        res.Typ = "совет";
                        res.IsDep = true;
                        res.Coef = 2;
                        return res;
                    }
                }
                if (tt.Next != null && (tt.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && !tt.Chars.IsAllLower) 
                {
                    res = new OrgItemTypeToken(tt, tt);
                    res.Geo = tt.Next as Pullenti.Ner.ReferentToken;
                    res.Typ = "совет";
                    res.IsDep = true;
                    res.Coef = 4;
                    res.Profiles.Add(Pullenti.Ner.Org.OrgProfile.State);
                    return res;
                }
            }
            bool say = false;
            if ((((term == "СООБЩАЕТ" || term == "СООБЩЕНИЮ" || term == "ПИШЕТ") || term == "ПЕРЕДАЕТ" || term == "ПОВІДОМЛЯЄ") || term == "ПОВІДОМЛЕННЯМ" || term == "ПИШЕ") || term == "ПЕРЕДАЄ") 
                say = true;
            if (((say || tt.IsValue("ОБЛОЖКА", "ОБКЛАДИНКА") || tt.IsValue("РЕДАКТОР", null)) || tt.IsValue("КОРРЕСПОНДЕНТ", "КОРЕСПОНДЕНТ") || tt.IsValue("ЖУРНАЛИСТ", "ЖУРНАЛІСТ")) || term == "ИНТЕРВЬЮ" || term == "ІНТЕРВЮ") 
            {
                if (m_PressRU == null) 
                    m_PressRU = new OrgItemTypeTermin("ИЗДАНИЕ", Pullenti.Morph.MorphLang.RU, Pullenti.Ner.Org.OrgProfile.Media) { CanHasLatinName = true, Coeff = 4 };
                if (m_PressUA == null) 
                    m_PressUA = new OrgItemTypeTermin("ВИДАННЯ", Pullenti.Morph.MorphLang.UA, Pullenti.Ner.Org.OrgProfile.Media) { CanHasLatinName = true, Coeff = 4 };
                OrgItemTypeTermin pres = (tt.Kit.BaseLanguage.IsUa ? m_PressUA : m_PressRU);
                Pullenti.Ner.Token t1 = t.Next;
                if (t1 == null) 
                    return null;
                if (t1.Chars.IsLatinLetter && !t1.Chars.IsAllLower) 
                {
                    if (tt.IsValue("РЕДАКТОР", null)) 
                        return null;
                    return new OrgItemTypeToken(t, t) { Typ = pres.CanonicText.ToLower(), Root = pres, IsNotTyp = true };
                }
                if (!say) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if ((br != null && br.IsQuoteType && !t1.Next.Chars.IsAllLower) && ((br.EndChar - br.BeginChar) < 40)) 
                        return new OrgItemTypeToken(t, t) { Typ = pres.CanonicText.ToLower(), Root = pres, IsNotTyp = true };
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.EndToken.Next != null) 
                {
                    t1 = npt.EndToken.Next;
                    string root = npt.Noun.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                    bool ok = t1.Chars.IsLatinLetter && !t1.Chars.IsAllLower;
                    if (!ok && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t1, true, false)) 
                        ok = true;
                    if (ok) 
                    {
                        if ((root == "ИЗДАНИЕ" || root == "ИЗДАТЕЛЬСТВО" || root == "ЖУРНАЛ") || root == "ВИДАННЯ" || root == "ВИДАВНИЦТВО") 
                        {
                            res = new OrgItemTypeToken(npt.BeginToken, npt.EndToken) { Typ = root.ToLower() };
                            res.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Media);
                            res.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Press);
                            if (npt.Adjectives.Count > 0) 
                            {
                                foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
                                {
                                    Pullenti.Ner.ReferentToken rt1 = res.Kit.ProcessReferent("GEO", a.BeginToken);
                                    if (rt1 != null && rt1.Morph.Class.IsAdjective) 
                                    {
                                        if (res.Geo == null) 
                                            res.Geo = rt1;
                                        else 
                                            res.Geo2 = rt1;
                                    }
                                }
                                res.AltTyp = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false).ToLower();
                            }
                            res.Root = new OrgItemTypeTermin(root) { CanHasLatinName = true, Coeff = 4 };
                            return res;
                        }
                    }
                }
                Pullenti.Ner.ReferentToken rt = t1.Kit.ProcessReferent("GEO", t1);
                if (rt != null && rt.Morph.Class.IsAdjective) 
                {
                    if (rt.EndToken.Next != null && rt.EndToken.Next.Chars.IsLatinLetter) 
                    {
                        res = new OrgItemTypeToken(t1, rt.EndToken) { Typ = pres.CanonicText.ToLower(), Root = pres };
                        res.Geo = rt;
                        return res;
                    }
                }
                Pullenti.Ner.Token tt1 = t1;
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt1, true, false)) 
                    tt1 = t1.Next;
                if ((((tt1.Chars.IsLatinLetter && tt1.Next != null && tt1.Next.IsChar('.')) && tt1.Next.Next != null && tt1.Next.Next.Chars.IsLatinLetter) && (tt1.Next.Next.LengthChar < 4) && tt1.Next.Next.LengthChar > 1) && !tt1.Next.IsWhitespaceAfter) 
                {
                    if (tt1 != t1 && !Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt1.Next.Next.Next, true, t1, false)) 
                    {
                    }
                    else 
                    {
                        res = new OrgItemTypeToken(t1, tt1.Next.Next) { Typ = pres.CanonicText.ToLower(), Root = pres };
                        res.Name = Pullenti.Ner.Core.MiscHelper.GetTextValue(t1, tt1.Next.Next, Pullenti.Ner.Core.GetTextAttr.No).Replace(" ", "");
                        if (tt1 != t1) 
                            res.EndToken = res.EndToken.Next;
                        res.Coef = 4;
                    }
                    return res;
                }
            }
            else if ((t.IsValue("ЖУРНАЛ", null) || t.IsValue("ИЗДАНИЕ", null) || t.IsValue("ИЗДАТЕЛЬСТВО", null)) || t.IsValue("ВИДАННЯ", null) || t.IsValue("ВИДАВНИЦТВО", null)) 
            {
                bool ok = false;
                if (ad != null) 
                {
                    List<Pullenti.Ner.Core.IntOntologyToken> otExLi = ad.LocalOntology.TryAttach(t.Next, null, false);
                    if (otExLi == null && t.Kit.Ontology != null) 
                        otExLi = t.Kit.Ontology.AttachToken(Pullenti.Ner.Org.OrganizationReferent.OBJ_TYPENAME, t.Next);
                    if ((otExLi != null && otExLi.Count > 0 && otExLi[0].Item != null) && (otExLi[0].Item.Referent is Pullenti.Ner.Org.OrganizationReferent)) 
                    {
                        if ((otExLi[0].Item.Referent as Pullenti.Ner.Org.OrganizationReferent).Kind == Pullenti.Ner.Org.OrganizationKind.Press) 
                            ok = true;
                    }
                }
                if (t.Next != null && t.Next.Chars.IsLatinLetter && !t.Next.Chars.IsAllLower) 
                    ok = true;
                if (ok) 
                {
                    res = new OrgItemTypeToken(t, t) { Typ = t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false).ToLower() };
                    res.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Media);
                    res.Profiles.Add(Pullenti.Ner.Org.OrgProfile.Press);
                    res.Root = new OrgItemTypeTermin(t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false)) { Typ = OrgItemTypeTyp.Org, Coeff = 3, CanHasLatinName = true };
                    res.Morph = t.Morph;
                    res.Chars = t.Chars;
                    if (t.Previous != null && t.Previous.Morph.Class.IsAdjective) 
                    {
                        Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("GEO", t.Previous);
                        if (rt != null && rt.EndToken == t.Previous) 
                        {
                            res.BeginToken = t.Previous;
                            res.Geo = rt;
                        }
                    }
                    return res;
                }
            }
            else if ((term == "МО" && t.Chars.IsAllUpper && (t.Next is Pullenti.Ner.ReferentToken)) && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
            {
                Pullenti.Ner.Geo.GeoReferent geo = t.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                if (geo != null && geo.IsState) 
                {
                    res = new OrgItemTypeToken(t, t) { Typ = "министерство", Name = "МИНИСТЕРСТВО ОБОРОНЫ", Coef = 4, Root = m_MO };
                    res.Profiles.Add(Pullenti.Ner.Org.OrgProfile.State);
                    res.CanBeOrganization = true;
                    return res;
                }
            }
            else if (term == "ИК" && t.Chars.IsAllUpper) 
            {
                Pullenti.Ner.Token et = null;
                if (OrgItemNumberToken.TryAttach(t.Next, false, null) != null) 
                    et = t;
                else if (t.Next != null && (t.Next is Pullenti.Ner.NumberToken)) 
                    et = t;
                else if ((t.Next != null && t.Next.IsHiphen && t.Next.Next != null) && (t.Next.Next is Pullenti.Ner.NumberToken)) 
                    et = t.Next;
                if (et != null) 
                    return new OrgItemTypeToken(t, et) { Typ = "исправительная колония", AltTyp = "колония", Root = m_IsprKolon, CanBeOrganization = true };
            }
            else if (t.IsValue("ПАКЕТ", null) && t.Next != null && t.Next.IsValue("АКЦИЯ", "АКЦІЯ")) 
                return new OrgItemTypeToken(t, t.Next) { Coef = 4, IsNotTyp = true, Typ = "" };
            else 
            {
                Pullenti.Ner.Core.TerminToken tok = m_PrefWords.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null && tok.Tag != null) 
                {
                    if ((tok.WhitespacesAfterCount < 2) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tok.EndToken.Next, true, false)) 
                        return new OrgItemTypeToken(t, tok.EndToken) { Coef = 4, IsNotTyp = true, Typ = "" };
                }
            }
            if (res == null && term == "АК" && t.Chars.IsAllUpper) 
            {
                if (TryAttach(t.Next, canBeFirstLetterLower, ad) != null) 
                    return new OrgItemTypeToken(t, t) { Root = m_AkcionComp, Typ = m_AkcionComp.CanonicText.ToLower() };
            }
            if (term == "В") 
            {
                if ((t.Next != null && t.Next.IsCharOf("\\/") && t.Next.Next != null) && t.Next.Next.IsValue("Ч", null)) 
                {
                    if (OrgItemNumberToken.TryAttach(t.Next.Next.Next, true, null) != null) 
                        return new OrgItemTypeToken(t, t.Next.Next) { Root = m_MilitaryUnit, Typ = m_MilitaryUnit.CanonicText.ToLower() };
                }
            }
            if (t.Morph.Class.IsAdjective && t.Next != null && ((t.Next.Chars.IsAllUpper || t.Next.Chars.IsLastLower))) 
            {
                if (t.Chars.IsCapitalUpper || (((t.Previous != null && t.Previous.IsHiphen && t.Previous.Previous != null) && t.Previous.Previous.Chars.IsCapitalUpper))) 
                {
                    OrgItemTypeToken res1 = _TryAttach(t.Next, true);
                    if ((res1 != null && res1.EndToken == t.Next && res1.Name == null) && res1.Root != null) 
                    {
                        res1.BeginToken = t;
                        res1.Coef = 5;
                        Pullenti.Morph.MorphGender gen = Pullenti.Morph.MorphGender.Undefined;
                        for (int ii = res1.Root.CanonicText.Length - 1; ii >= 0; ii--) 
                        {
                            if (ii == 0 || res1.Root.CanonicText[ii - 1] == ' ') 
                            {
                                Pullenti.Morph.MorphWordForm mm = Pullenti.Morph.MorphologyService.GetWordBaseInfo(res1.Root.CanonicText.Substring(ii), null, false, false);
                                gen = mm.Gender;
                                break;
                            }
                        }
                        string nam = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, gen, false);
                        if (((t.Previous != null && t.Previous.IsHiphen && (t.Previous.Previous is Pullenti.Ner.TextToken)) && t.Previous.Previous.Chars.IsCapitalUpper && !t.IsWhitespaceBefore) && !t.Previous.IsWhitespaceBefore) 
                        {
                            res1.BeginToken = t.Previous.Previous;
                            nam = string.Format("{0}-{1}", (res1.BeginToken as Pullenti.Ner.TextToken).Term, nam);
                        }
                        res1.Name = nam;
                        return res1;
                    }
                }
            }
            if ((t.Morph.Class.IsAdjective && !term.EndsWith("ВО") && !t.Chars.IsAllLower) && (t.WhitespacesAfterCount < 2)) 
            {
                OrgItemTypeToken res1 = _TryAttach(t.Next, true);
                if ((res1 != null && res1.Profiles.Contains(Pullenti.Ner.Org.OrgProfile.Transport) && res1.Name == null) && res1.Root != null) 
                {
                    string nam = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective, Pullenti.Morph.MorphNumber.Singular, (res1.Root.CanonicText.EndsWith("ДОРОГА") ? Pullenti.Morph.MorphGender.Feminie : Pullenti.Morph.MorphGender.Masculine), false);
                    if (nam != null) 
                    {
                        if (((t.Previous != null && t.Previous.IsHiphen && (t.Previous.Previous is Pullenti.Ner.TextToken)) && t.Previous.Previous.Chars.IsCapitalUpper && !t.IsWhitespaceBefore) && !t.Previous.IsWhitespaceBefore) 
                        {
                            t = t.Previous.Previous;
                            nam = string.Format("{0}-{1}", (t as Pullenti.Ner.TextToken).Term, nam);
                        }
                        res1.BeginToken = t;
                        res1.Coef = 5;
                        res1.Name = string.Format("{0} {1}", nam, res1.Root.CanonicText);
                        res1.CanBeOrganization = true;
                        return res1;
                    }
                }
            }
            return res;
        }
        static OrgItemTypeTermin m_PressRU;
        static OrgItemTypeTermin m_PressUA;
        static OrgItemTypeTermin m_PressIA;
        static OrgItemTypeTermin m_MilitaryUnit;
        static OrgItemTypeToken _TryAttach(Pullenti.Ner.Token t, bool canBeFirstLetterLower)
        {
            if (t == null) 
                return null;
            OrgItemTypeToken res;
            List<Pullenti.Ner.Core.IntOntologyToken> li = m_Global.TryAttach(t, null, false);
            if (li != null) 
            {
                if (t.Previous != null && t.Previous.IsHiphen && !t.IsWhitespaceBefore) 
                {
                    List<Pullenti.Ner.Core.IntOntologyToken> li1 = m_Global.TryAttach(t.Previous.Previous, null, false);
                    if (li1 != null && li1[0].EndToken == li[0].EndToken) 
                        return null;
                }
                res = new OrgItemTypeToken(li[0].BeginToken, li[0].EndToken);
                res.Root = li[0].Termin as OrgItemTypeTermin;
                Pullenti.Ner.Core.NounPhraseToken nn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(li[0].BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (nn != null && ((nn.EndToken.Next == null || !nn.EndToken.Next.IsChar('.')))) 
                    res.Morph = nn.Morph;
                else 
                    res.Morph = li[0].Morph;
                res.CharsRoot = res.Chars;
                if (res.Root.IsPurePrefix) 
                {
                    res.Typ = res.Root.Acronym;
                    if (res.Typ == null) 
                        res.Typ = res.Root.CanonicText.ToLower();
                }
                else 
                    res.Typ = res.Root.CanonicText.ToLower();
                if (res.BeginToken != res.EndToken && !res.Root.IsPurePrefix) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt0 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt0 != null && npt0.EndToken == res.EndToken && npt0.Adjectives.Count >= res.NameWordsCount) 
                    {
                        string s = npt0.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                        if (string.Compare(s, res.Typ, true) != 0) 
                        {
                            res.Name = s;
                            res.CanBeOrganization = true;
                        }
                    }
                }
                if (res.Typ == "сберегательный банк" && res.Name == null) 
                {
                    res.Name = res.Typ.ToUpper();
                    res.Typ = "банк";
                }
                if (res.IsDep && res.Typ.StartsWith("отдел ") && res.Name == null) 
                {
                    res.Name = res.Typ.ToUpper();
                    res.Typ = "отдел";
                }
                if (res.BeginToken == res.EndToken) 
                {
                    if (res.Chars.IsCapitalUpper) 
                    {
                        if ((res.LengthChar < 4) && !res.BeginToken.IsValue(res.Root.CanonicText, null)) 
                        {
                            if (!canBeFirstLetterLower) 
                                return null;
                        }
                    }
                    if (res.Chars.IsAllUpper) 
                    {
                        if (res.BeginToken.IsValue("САН", null)) 
                            return null;
                    }
                }
                if (res.EndToken.Next != null && res.EndToken.Next.IsChar('(')) 
                {
                    List<Pullenti.Ner.Core.IntOntologyToken> li22 = m_Global.TryAttach(res.EndToken.Next.Next, null, false);
                    if ((li22 != null && li22.Count > 0 && li22[0].Termin == li[0].Termin) && li22[0].EndToken.Next != null && li22[0].EndToken.Next.IsChar(')')) 
                        res.EndToken = li22[0].EndToken.Next;
                }
                return res;
            }
            if ((t is Pullenti.Ner.NumberToken) && t.Morph.Class.IsAdjective) 
            {
            }
            else if (t is Pullenti.Ner.TextToken) 
            {
            }
            else 
                return null;
            if (t.IsValue("СБ", null)) 
            {
                if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    Pullenti.Ner.Geo.GeoReferent geo = t.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                    if (geo.IsState) 
                    {
                        if (geo.Alpha2 != "RU") 
                            return new OrgItemTypeToken(t, t) { Typ = "управление", NameIsName = true, Root = m_SecServ, Name = m_SecServ.CanonicText };
                    }
                    return new OrgItemTypeToken(t, t) { Typ = "банк", NameIsName = true, Root = m_SberBank, Name = m_SberBank.CanonicText };
                }
            }
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.IgnoreAdjBest, 0, null);
            if (npt == null || npt.InternalNoun != null) 
            {
                if (((!t.Chars.IsAllLower && t.Next != null && t.Next.IsHiphen) && !t.IsWhitespaceAfter && !t.Next.IsWhitespaceAfter) && t.Next.Next != null && t.Next.Next.IsValue("БАНК", null)) 
                {
                    string s = t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                    res = new OrgItemTypeToken(t, t.Next.Next) { Name = s, Morph = t.Next.Next.Morph, Chars = t.Chars, CharsRoot = t.Next.Next.Chars };
                    res.Root = m_Bank;
                    res.Typ = "банк";
                    return res;
                }
                if ((t is Pullenti.Ner.NumberToken) && (t.WhitespacesAfterCount < 3) && (t.Next is Pullenti.Ner.TextToken)) 
                {
                    OrgItemTypeToken res11 = _TryAttach(t.Next, false);
                    if (res11 != null && res11.Root != null && res11.Root.CanHasNumber) 
                    {
                        res11.BeginToken = t;
                        res11.Number = (t as Pullenti.Ner.NumberToken).Value.ToString();
                        res11.Coef += 1;
                        return res11;
                    }
                }
                return null;
            }
            if (npt.Morph.Gender == Pullenti.Morph.MorphGender.Feminie && npt.Noun.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) == "БАНКА") 
                return null;
            if (npt.BeginToken == npt.EndToken) 
            {
                string s = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                if (Pullenti.Morph.LanguageHelper.EndsWithEx(s, "БАНК", "БАНКА", "БАНОК", null)) 
                {
                    if (Pullenti.Morph.LanguageHelper.EndsWith(s, "БАНКА")) 
                        s = s.Substring(0, s.Length - 1);
                    else if (Pullenti.Morph.LanguageHelper.EndsWith(s, "БАНОК")) 
                        s = s.Substring(0, s.Length - 2) + "К";
                    res = new OrgItemTypeToken(npt.BeginToken, npt.EndToken) { Name = s, Morph = npt.Morph, Chars = npt.Chars, CharsRoot = npt.Chars };
                    res.Root = m_Bank;
                    res.Typ = "банк";
                    return res;
                }
                return null;
            }
            for (Pullenti.Ner.Token tt = npt.EndToken; tt != null; tt = tt.Previous) 
            {
                if (tt == npt.BeginToken) 
                    break;
                List<Pullenti.Ner.Core.IntOntologyToken> lii = m_Global.TryAttach(tt, null, false);
                if (lii != null) 
                {
                    if (tt == npt.EndToken && tt.Previous != null && tt.Previous.IsHiphen) 
                        continue;
                    li = lii;
                    if (li[0].EndChar < npt.EndChar) 
                        npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.IgnoreAdjBest, li[0].EndChar, null);
                    break;
                }
            }
            if (li == null || npt == null) 
                return null;
            res = new OrgItemTypeToken(npt.BeginToken, li[0].EndToken);
            foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
            {
                if (a.IsValue("ДОЧЕРНИЙ", null) || a.IsValue("ДОЧІРНІЙ", null)) 
                {
                    res.IsDouterOrg = true;
                    break;
                }
            }
            foreach (string em in m_EmptyTypWords) 
            {
                foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
                {
                    if (a.IsValue(em, null)) 
                    {
                        npt.Adjectives.Remove(a);
                        break;
                    }
                }
            }
            while (npt.Adjectives.Count > 0) 
            {
                if (npt.Adjectives[0].BeginToken.GetMorphClassInDictionary().IsVerb) 
                    npt.Adjectives.RemoveAt(0);
                else if (npt.Adjectives[0].BeginToken is Pullenti.Ner.NumberToken) 
                {
                    res.Number = (npt.Adjectives[0].BeginToken as Pullenti.Ner.NumberToken).Value.ToString();
                    npt.Adjectives.RemoveAt(0);
                }
                else 
                    break;
            }
            if (npt.Adjectives.Count > 0) 
            {
                res.AltTyp = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
                if (li[0].EndChar > npt.EndChar) 
                    res.AltTyp = string.Format("{0} {1}", res.AltTyp, Pullenti.Ner.Core.MiscHelper.GetTextValue(npt.EndToken.Next, li[0].EndToken, Pullenti.Ner.Core.GetTextAttr.No));
            }
            if (res.Number == null) 
            {
                while (npt.Adjectives.Count > 0) 
                {
                    if (!npt.Adjectives[0].Chars.IsAllLower || canBeFirstLetterLower) 
                        break;
                    if (npt.Kit.ProcessReferent("GEO", npt.Adjectives[0].BeginToken) != null) 
                        break;
                    if (IsStdAdjective(npt.Adjectives[0], false)) 
                        break;
                    bool bad = false;
                    if (!npt.Noun.Chars.IsAllLower || !IsStdAdjective(npt.Adjectives[0], false)) 
                        bad = true;
                    else 
                        for (int i = 1; i < npt.Adjectives.Count; i++) 
                        {
                            if (npt.Kit.ProcessReferent("GEO", npt.Adjectives[i].BeginToken) != null) 
                                continue;
                            if (!npt.Adjectives[i].Chars.IsAllLower) 
                            {
                                bad = true;
                                break;
                            }
                        }
                    if (!bad) 
                        break;
                    npt.Adjectives.RemoveAt(0);
                }
            }
            foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
            {
                Pullenti.Ner.ReferentToken r = npt.Kit.ProcessReferent("GEO", a.BeginToken);
                if (r != null) 
                {
                    if (a == npt.Adjectives[0]) 
                    {
                        OrgItemTypeToken res2 = _TryAttach(a.EndToken.Next, true);
                        if (res2 != null && res2.EndChar > npt.EndChar && res2.Geo == null) 
                        {
                            res2.BeginToken = a.BeginToken;
                            res2.Geo = r;
                            return res2;
                        }
                    }
                    if (res.Geo == null) 
                        res.Geo = r;
                    else if (res.Geo2 == null) 
                        res.Geo2 = r;
                }
            }
            if (res.EndToken == npt.EndToken) 
                res.Name = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
            if (res.Name == res.AltTyp) 
                res.AltTyp = null;
            if (res.AltTyp != null) 
                res.AltTyp = res.AltTyp.ToLower().Replace('-', ' ');
            res.Root = li[0].Termin as OrgItemTypeTermin;
            if (res.Root.IsPurePrefix && (li[0].LengthChar < 7)) 
                return null;
            res.Typ = res.Root.CanonicText.ToLower();
            if (npt.Adjectives.Count > 0) 
            {
                for (int i = 0; i < npt.Adjectives.Count; i++) 
                {
                    string s = npt.GetNormalCaseTextWithoutAdjective(i);
                    List<Pullenti.Ner.Core.Termin> ctli = m_Global.FindTerminByCanonicText(s);
                    if (ctli != null && ctli.Count > 0 && (ctli[0] is OrgItemTypeTermin)) 
                    {
                        res.Root = ctli[0] as OrgItemTypeTermin;
                        if (res.AltTyp == null) 
                        {
                            res.AltTyp = res.Root.CanonicText.ToLower();
                            if (res.AltTyp == res.Typ) 
                                res.AltTyp = null;
                        }
                        break;
                    }
                }
                res.Coef = res.Root.Coeff;
                if (res.Coef == 0) 
                {
                    for (int i = 0; i < npt.Adjectives.Count; i++) 
                    {
                        if (IsStdAdjective(npt.Adjectives[i], true)) 
                        {
                            res.Coef++;
                            if (((i + 1) < npt.Adjectives.Count) && !IsStdAdjective(npt.Adjectives[i + 1], false)) 
                                res.Coef++;
                            if (npt.Adjectives[i].IsValue("ФЕДЕРАЛЬНЫЙ", "ФЕДЕРАЛЬНИЙ") || npt.Adjectives[i].IsValue("ГОСУДАРСТВЕННЫЙ", "ДЕРЖАВНИЙ")) 
                            {
                                res.IsDoubtRootWord = false;
                                if (res.IsDep) 
                                    res.IsDep = false;
                            }
                        }
                        else if (IsStdAdjective(npt.Adjectives[i], false)) 
                            res.Coef += 0.5F;
                    }
                }
                else 
                    for (int i = 0; i < (npt.Adjectives.Count - 1); i++) 
                    {
                        if (IsStdAdjective(npt.Adjectives[i], true)) 
                        {
                            if (((i + 1) < npt.Adjectives.Count) && !IsStdAdjective(npt.Adjectives[i + 1], true)) 
                            {
                                res.Coef++;
                                res.IsDoubtRootWord = false;
                                res.CanBeOrganization = true;
                                if (res.IsDep) 
                                    res.IsDep = false;
                            }
                        }
                    }
            }
            res.Morph = npt.Morph;
            res.Chars = npt.Chars;
            if (!res.Chars.IsAllUpper && !res.Chars.IsCapitalUpper && !res.Chars.IsAllLower) 
            {
                res.Chars = npt.Noun.Chars;
                if (res.Chars.IsAllLower) 
                    res.Chars = res.BeginToken.Chars;
            }
            if (npt.Noun != null) 
                res.CharsRoot = npt.Noun.Chars;
            return res;
        }
        public static bool IsStdAdjective(Pullenti.Ner.Token t, bool onlyFederal = false)
        {
            if (t == null) 
                return false;
            if (t is Pullenti.Ner.MetaToken) 
                t = (t as Pullenti.Ner.MetaToken).BeginToken;
            Pullenti.Ner.Core.TerminToken tt = (t.Morph.Language.IsUa ? m_StdAdjsUA.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No) : m_StdAdjs.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No));
            if (tt == null) 
                return false;
            if (onlyFederal) 
            {
                if (tt.Termin.Tag == null) 
                    return false;
            }
            return true;
        }
        public static bool CheckOrgSpecialWordBefore(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            if (t.IsCommaAnd && t.Previous != null) 
                t = t.Previous;
            int k = 0;
            OrgItemTypeToken ty;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Previous) 
            {
                Pullenti.Ner.Referent r = tt.GetReferent();
                if (r != null) 
                {
                    if (tt == t && (r is Pullenti.Ner.Org.OrganizationReferent)) 
                        return true;
                    return false;
                }
                if (!(tt is Pullenti.Ner.TextToken)) 
                {
                    if (!(tt is Pullenti.Ner.NumberToken)) 
                        break;
                    k++;
                    continue;
                }
                if (tt.IsNewlineAfter) 
                {
                    if (!tt.IsChar(',')) 
                        return false;
                    continue;
                }
                if (tt.IsValue("УПРАВЛЕНИЕ", null) || tt.IsValue("УПРАВЛІННЯ", null)) 
                {
                    ty = OrgItemTypeToken.TryAttach(tt.Next, true, null);
                    if (ty != null && ty.IsDoubtRootWord) 
                        return false;
                }
                if (tt == t && m_PrefWords.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                    return true;
                if (tt == t && tt.IsChar('.')) 
                    continue;
                ty = OrgItemTypeToken.TryAttach(tt, true, null);
                if (ty != null && ty.EndToken.EndChar <= t.EndChar && ty.EndToken == t) 
                {
                    if (!ty.IsDoubtRootWord) 
                        return true;
                }
                if (tt.Kit.RecurseLevel == 0) 
                {
                    Pullenti.Ner.ReferentToken rt = tt.Kit.ProcessReferent("PERSONPROPERTY", tt);
                    if (rt != null && rt.Referent != null && rt.Referent.TypeName == "PERSONPROPERTY") 
                    {
                        if (rt.EndChar >= t.EndChar) 
                            return true;
                    }
                }
                k++;
                if (k > 4) 
                    break;
            }
            return false;
        }
        public static bool CheckPersonProperty(Pullenti.Ner.Token t)
        {
            if (t == null || !t.Chars.IsCyrillicLetter) 
                return false;
            Pullenti.Ner.Core.TerminToken tok = m_PrefWords.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok == null) 
                return false;
            if (tok.Termin.Tag == null) 
                return false;
            return true;
        }
        public static Pullenti.Ner.ReferentToken TryAttachReferenceToExistOrg(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            Pullenti.Ner.Core.TerminToken tok = m_KeyWordsForRefs.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok == null && t.Morph.Class.IsPronoun) 
                tok = m_KeyWordsForRefs.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
            string abbr = null;
            if (tok == null) 
            {
                if (t.LengthChar > 1 && ((t.Chars.IsCapitalUpper || t.Chars.IsLastLower))) 
                    abbr = (t as Pullenti.Ner.TextToken).Lemma;
                else 
                {
                    OrgItemTypeToken ty1 = OrgItemTypeToken._TryAttach(t, true);
                    if (ty1 != null) 
                        abbr = ty1.Typ;
                    else 
                        return null;
                }
            }
            int cou = 0;
            for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
            {
                if (tt.IsNewlineAfter) 
                    cou += 10;
                cou++;
                if (cou > 500) 
                    break;
                if (!(tt is Pullenti.Ner.ReferentToken)) 
                    continue;
                List<Pullenti.Ner.Referent> refs = tt.GetReferents();
                if (refs == null) 
                    continue;
                foreach (Pullenti.Ner.Referent r in refs) 
                {
                    if (r is Pullenti.Ner.Org.OrganizationReferent) 
                    {
                        if (abbr != null) 
                        {
                            if (r.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_TYPE, abbr, true) == null) 
                                continue;
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(r, t, t);
                            Pullenti.Ner.Org.OrganizationReferent hi = r.GetSlotValue(Pullenti.Ner.Org.OrganizationReferent.ATTR_HIGHER) as Pullenti.Ner.Org.OrganizationReferent;
                            if (hi != null && t.Next != null) 
                            {
                                foreach (string ty in hi.Types) 
                                {
                                    if (t.Next.IsValue(ty.ToUpper(), null)) 
                                    {
                                        rt.EndToken = t.Next;
                                        break;
                                    }
                                }
                            }
                            return rt;
                        }
                        if (tok.Termin.Tag != null) 
                        {
                            bool ok = false;
                            foreach (string ty in (r as Pullenti.Ner.Org.OrganizationReferent).Types) 
                            {
                                if (ty.EndsWith(tok.Termin.CanonicText, StringComparison.OrdinalIgnoreCase)) 
                                {
                                    ok = true;
                                    break;
                                }
                            }
                            if (!ok) 
                                continue;
                        }
                        return new Pullenti.Ner.ReferentToken(r, t, tok.EndToken);
                    }
                }
            }
            return null;
        }
        public static bool IsTypesAntagonisticOO(Pullenti.Ner.Org.OrganizationReferent r1, Pullenti.Ner.Org.OrganizationReferent r2)
        {
            Pullenti.Ner.Org.OrganizationKind k1 = r1.Kind;
            Pullenti.Ner.Org.OrganizationKind k2 = r2.Kind;
            if (k1 != Pullenti.Ner.Org.OrganizationKind.Undefined && k2 != Pullenti.Ner.Org.OrganizationKind.Undefined) 
            {
                if (IsTypesAntagonisticKK(k1, k2)) 
                    return true;
            }
            List<string> types1 = r1.Types;
            List<string> types2 = r2.Types;
            foreach (string t1 in types1) 
            {
                if (types2.Contains(t1)) 
                    return false;
            }
            foreach (string t1 in types1) 
            {
                foreach (string t2 in types2) 
                {
                    if (IsTypesAntagonisticSS(t1, t2)) 
                        return true;
                }
            }
            return false;
        }
        public static bool IsTypeAccords(Pullenti.Ner.Org.OrganizationReferent r1, OrgItemTypeToken t2)
        {
            if (t2 == null || t2.Typ == null) 
                return false;
            if (t2.Typ == "министерство" || t2.Typ == "міністерство" || t2.Typ.EndsWith("штаб")) 
                return r1.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_TYPE, t2.Typ, true) != null;
            List<Pullenti.Ner.Org.OrgProfile> prs = r1.Profiles;
            foreach (Pullenti.Ner.Org.OrgProfile pr in prs) 
            {
                if (t2.Profiles.Contains(pr)) 
                    return true;
            }
            if (r1.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_TYPE, null, true) == null) 
            {
                if (prs.Count == 0) 
                    return true;
            }
            if (t2.Profiles.Count == 0) 
            {
                if (prs.Contains(Pullenti.Ner.Org.OrgProfile.Policy)) 
                {
                    if (t2.Typ == "группа" || t2.Typ == "организация") 
                        return true;
                }
                if (prs.Contains(Pullenti.Ner.Org.OrgProfile.Music)) 
                {
                    if (t2.Typ == "группа") 
                        return true;
                }
            }
            foreach (string t in r1.Types) 
            {
                if (t == t2.Typ) 
                    return true;
                if (t.EndsWith(t2.Typ)) 
                    return true;
                if (t2.Typ == "издание") 
                {
                    if (t.EndsWith("агентство")) 
                        return true;
                }
            }
            if ((t2.Typ == "компания" || t2.Typ == "корпорация" || t2.Typ == "company") || t2.Typ == "corporation") 
            {
                if (prs.Count == 0) 
                    return true;
                if (prs.Contains(Pullenti.Ner.Org.OrgProfile.Business) || prs.Contains(Pullenti.Ner.Org.OrgProfile.Finance) || prs.Contains(Pullenti.Ner.Org.OrgProfile.Industry)) 
                    return true;
            }
            return false;
        }
        public static bool IsTypesAntagonisticTT(OrgItemTypeToken t1, OrgItemTypeToken t2)
        {
            Pullenti.Ner.Org.OrganizationKind k1 = _getKind(t1.Typ, t1.Name ?? "", null);
            Pullenti.Ner.Org.OrganizationKind k2 = _getKind(t2.Typ, t2.Name ?? "", null);
            if (k1 == Pullenti.Ner.Org.OrganizationKind.Justice && t2.Typ.StartsWith("Ф")) 
                return false;
            if (k2 == Pullenti.Ner.Org.OrganizationKind.Justice && t1.Typ.StartsWith("Ф")) 
                return false;
            if (IsTypesAntagonisticKK(k1, k2)) 
                return true;
            if (IsTypesAntagonisticSS(t1.Typ, t2.Typ)) 
                return true;
            if (k1 == Pullenti.Ner.Org.OrganizationKind.Bank && k2 == Pullenti.Ner.Org.OrganizationKind.Bank) 
            {
                if (t1.Name != null && t2.Name != null && t1 != t2) 
                    return true;
            }
            return false;
        }
        public static bool IsTypesAntagonisticSS(string typ1, string typ2)
        {
            if (typ1 == typ2) 
                return false;
            string uni = string.Format("{0} {1} ", typ1, typ2);
            if ((((uni.Contains("служба") || uni.Contains("департамент") || uni.Contains("отделение")) || uni.Contains("отдел") || uni.Contains("відділення")) || uni.Contains("відділ") || uni.Contains("инспекция")) || uni.Contains("інспекція")) 
                return true;
            if (uni.Contains("министерство") || uni.Contains("міністерство")) 
                return true;
            if (uni.Contains("правительство") && !uni.Contains("администрация")) 
                return true;
            if (uni.Contains("уряд") && !uni.Contains("адміністрація")) 
                return true;
            if (typ1 == "управление" && ((typ2 == "главное управление" || typ2 == "пограничное управление"))) 
                return true;
            if (typ2 == "управление" && ((typ1 == "главное управление" || typ2 == "пограничное управление"))) 
                return true;
            if (typ1 == "керування" && typ2 == "головне управління") 
                return true;
            if (typ2 == "керування" && typ1 == "головне управління") 
                return true;
            if (typ1 == "university") 
            {
                if (typ2 == "school" || typ2 == "college") 
                    return true;
            }
            if (typ2 == "university") 
            {
                if (typ1 == "school" || typ1 == "college") 
                    return true;
            }
            return false;
        }
        public static bool IsTypesAntagonisticKK(Pullenti.Ner.Org.OrganizationKind k1, Pullenti.Ner.Org.OrganizationKind k2)
        {
            if (k1 == k2) 
                return false;
            if (k1 == Pullenti.Ner.Org.OrganizationKind.Department || k2 == Pullenti.Ner.Org.OrganizationKind.Department) 
                return false;
            if (k1 == Pullenti.Ner.Org.OrganizationKind.Govenment || k2 == Pullenti.Ner.Org.OrganizationKind.Govenment) 
                return true;
            if (k1 == Pullenti.Ner.Org.OrganizationKind.Justice || k2 == Pullenti.Ner.Org.OrganizationKind.Justice) 
                return true;
            if (k1 == Pullenti.Ner.Org.OrganizationKind.Party || k2 == Pullenti.Ner.Org.OrganizationKind.Party) 
            {
                if (k2 == Pullenti.Ner.Org.OrganizationKind.Federation || k1 == Pullenti.Ner.Org.OrganizationKind.Federation) 
                    return false;
                return true;
            }
            if (k1 == Pullenti.Ner.Org.OrganizationKind.Study) 
                k1 = Pullenti.Ner.Org.OrganizationKind.Science;
            if (k2 == Pullenti.Ner.Org.OrganizationKind.Study) 
                k2 = Pullenti.Ner.Org.OrganizationKind.Science;
            if (k1 == Pullenti.Ner.Org.OrganizationKind.Press) 
                k1 = Pullenti.Ner.Org.OrganizationKind.Media;
            if (k2 == Pullenti.Ner.Org.OrganizationKind.Press) 
                k2 = Pullenti.Ner.Org.OrganizationKind.Media;
            if (k1 == k2) 
                return false;
            if (k1 == Pullenti.Ner.Org.OrganizationKind.Undefined || k2 == Pullenti.Ner.Org.OrganizationKind.Undefined) 
                return false;
            return true;
        }
        public static Pullenti.Ner.Org.OrganizationKind CheckKind(Pullenti.Ner.Org.OrganizationReferent obj)
        {
            StringBuilder t = new StringBuilder();
            StringBuilder n = new StringBuilder();
            foreach (Pullenti.Ner.Slot s in obj.Slots) 
            {
                if (s.TypeName == Pullenti.Ner.Org.OrganizationReferent.ATTR_NAME) 
                    n.AppendFormat("{0};", s.Value);
                else if (s.TypeName == Pullenti.Ner.Org.OrganizationReferent.ATTR_TYPE) 
                    t.AppendFormat("{0};", s.Value);
            }
            return _getKind(t.ToString(), n.ToString(), obj);
        }
        internal static Pullenti.Ner.Org.OrganizationKind _getKind(string t, string n, Pullenti.Ner.Org.OrganizationReferent r = null)
        {
            if (!Pullenti.Morph.LanguageHelper.EndsWith(t, ";")) 
                t += ";";
            if (((((((((((((t.Contains("министерство") || t.Contains("правительство") || t.Contains("администрация")) || t.Contains("префектура") || t.Contains("мэрия;")) || t.Contains("муниципалитет") || Pullenti.Morph.LanguageHelper.EndsWith(t, "совет;")) || t.Contains("дума;") || t.Contains("собрание;")) || t.Contains("кабинет") || t.Contains("сенат;")) || t.Contains("палата") || t.Contains("рада;")) || t.Contains("парламент;") || t.Contains("конгресс")) || t.Contains("комиссия") || t.Contains("полиция;")) || t.Contains("милиция;") || t.Contains("хурал")) || t.Contains("суглан") || t.Contains("меджлис;")) || t.Contains("хасе;") || t.Contains("ил тумэн")) || t.Contains("курултай") || t.Contains("бундестаг")) || t.Contains("бундесрат")) 
                return Pullenti.Ner.Org.OrganizationKind.Govenment;
            if ((((((((((((t.Contains("міністерство") || t.Contains("уряд") || t.Contains("адміністрація")) || t.Contains("префектура") || t.Contains("мерія;")) || t.Contains("муніципалітет") || Pullenti.Morph.LanguageHelper.EndsWith(t, "рада;")) || t.Contains("дума;") || t.Contains("збори")) || t.Contains("кабінет;") || t.Contains("сенат;")) || t.Contains("палата") || t.Contains("рада;")) || t.Contains("парламент;") || t.Contains("конгрес")) || t.Contains("комісія") || t.Contains("поліція;")) || t.Contains("міліція;") || t.Contains("хурал")) || t.Contains("суглан") || t.Contains("хасе;")) || t.Contains("іл тумен") || t.Contains("курултай")) || t.Contains("меджліс;")) 
                return Pullenti.Ner.Org.OrganizationKind.Govenment;
            if (t.Contains("комитет") || t.Contains("комітет")) 
            {
                if (r != null && r.Higher != null && r.Higher.Kind == Pullenti.Ner.Org.OrganizationKind.Party) 
                    return Pullenti.Ner.Org.OrganizationKind.Department;
                return Pullenti.Ner.Org.OrganizationKind.Govenment;
            }
            if (t.Contains("штаб;")) 
            {
                if (r != null && r.Higher != null && r.Higher.Kind == Pullenti.Ner.Org.OrganizationKind.Military) 
                    return Pullenti.Ner.Org.OrganizationKind.Military;
                return Pullenti.Ner.Org.OrganizationKind.Govenment;
            }
            string tn = t;
            if (!string.IsNullOrEmpty(n)) 
                tn += n;
            tn = tn.ToLower();
            if (((((t.Contains("служба;") || t.Contains("инспекция;") || t.Contains("управление;")) || t.Contains("департамент") || t.Contains("комитет;")) || t.Contains("комиссия;") || t.Contains("інспекція;")) || t.Contains("керування;") || t.Contains("комітет;")) || t.Contains("комісія;")) 
            {
                if (tn.Contains("федеральн") || tn.Contains("государствен") || tn.Contains("державн")) 
                    return Pullenti.Ner.Org.OrganizationKind.Govenment;
                if (r != null && r.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_GEO, null, true) != null) 
                {
                    if (r.Higher == null && r.m_TempParentOrg == null) 
                    {
                        if (!t.Contains("управление;") && !t.Contains("департамент") && !t.Contains("керування;")) 
                            return Pullenti.Ner.Org.OrganizationKind.Govenment;
                    }
                }
            }
            if (((((((((((((((((((((((((((((((((t.Contains("подразделение") || t.Contains("отдел;") || t.Contains("отдел ")) || t.Contains("направление") || t.Contains("отделение")) || t.Contains("кафедра") || t.Contains("инспекция")) || t.Contains("факультет") || t.Contains("лаборатория")) || t.Contains("пресс центр") || t.Contains("пресс служба")) || t.Contains("сектор ") || t == "группа;") || ((t.Contains("курс;") && !t.Contains("конкурс"))) || t.Contains("филиал")) || t.Contains("главное управление") || t.Contains("пограничное управление")) || t.Contains("главное территориальное управление") || t.Contains("бухгалтерия")) || t.Contains("магистратура") || t.Contains("аспирантура")) || t.Contains("докторантура") || t.Contains("дирекция")) || t.Contains("руководство") || t.Contains("правление")) || t.Contains("пленум;") || t.Contains("президиум")) || t.Contains("стол;") || t.Contains("совет директоров")) || t.Contains("ученый совет") || t.Contains("коллегия")) || t.Contains("аппарат") || t.Contains("представительство")) || t.Contains("жюри;") || t.Contains("підрозділ")) || t.Contains("відділ;") || t.Contains("відділ ")) || t.Contains("напрямок") || t.Contains("відділення")) || t.Contains("інспекція") || t == "група;") || t.Contains("лабораторія") || t.Contains("прес центр")) || t.Contains("прес служба") || t.Contains("філія")) || t.Contains("головне управління") || t.Contains("головне територіальне управління")) || t.Contains("бухгалтерія") || t.Contains("магістратура")) || t.Contains("аспірантура") || t.Contains("докторантура")) || t.Contains("дирекція") || t.Contains("керівництво")) || t.Contains("правління") || t.Contains("президія")) || t.Contains("стіл") || t.Contains("рада директорів")) || t.Contains("вчена рада") || t.Contains("колегія")) || t.Contains("апарат") || t.Contains("представництво")) || t.Contains("журі;") || t.Contains("фракция")) || t.Contains("депутатская группа") || t.Contains("фракція")) || t.Contains("депутатська група")) 
                return Pullenti.Ner.Org.OrganizationKind.Department;
            if ((t.Contains("научн") || t.Contains("исследовательск") || t.Contains("науков")) || t.Contains("дослідн")) 
                return Pullenti.Ner.Org.OrganizationKind.Science;
            if (t.Contains("агенство") || t.Contains("агентство")) 
            {
                if (tn.Contains("федеральн") || tn.Contains("державн")) 
                    return Pullenti.Ner.Org.OrganizationKind.Govenment;
                if (tn.Contains("информацион") || tn.Contains("інформаційн")) 
                    return Pullenti.Ner.Org.OrganizationKind.Press;
            }
            if (t.Contains("холдинг") || t.Contains("группа компаний") || t.Contains("група компаній")) 
                return Pullenti.Ner.Org.OrganizationKind.Holding;
            if (t.Contains("академия") || t.Contains("академія")) 
            {
                if (tn.Contains("наук")) 
                    return Pullenti.Ner.Org.OrganizationKind.Science;
                return Pullenti.Ner.Org.OrganizationKind.Study;
            }
            if ((((((((((t.Contains("школа;") || t.Contains("университет") || tn.Contains("учебный ")) || t.Contains("лицей") || t.Contains("колледж")) || t.Contains("детский сад") || t.Contains("училище")) || t.Contains("гимназия") || t.Contains("семинария")) || t.Contains("образовательн") || t.Contains("интернат")) || t.Contains("університет") || tn.Contains("навчальний ")) || t.Contains("ліцей") || t.Contains("коледж")) || t.Contains("дитячий садок") || t.Contains("училище")) || t.Contains("гімназія") || t.Contains("семінарія")) || t.Contains("освітн") || t.Contains("інтернат")) 
                return Pullenti.Ner.Org.OrganizationKind.Study;
            if (((t.Contains("больница") || t.Contains("поликлиника") || t.Contains("клиника")) || t.Contains("госпиталь") || tn.Contains("санитарн")) || tn.Contains("медико") || tn.Contains("медицин")) 
                return Pullenti.Ner.Org.OrganizationKind.Medical;
            if ((((((t.Contains("церковь") || t.Contains("храм;") || t.Contains("собор")) || t.Contains("синагога") || t.Contains("мечеть")) || t.Contains("лавра") || t.Contains("монастырь")) || t.Contains("церква") || t.Contains("монастир")) || t.Contains("патриархия") || t.Contains("епархия")) || t.Contains("патріархія") || t.Contains("єпархія")) 
                return Pullenti.Ner.Org.OrganizationKind.Church;
            if (t.Contains("департамент") || t.Contains("управление") || t.Contains("керування")) 
            {
                if (r != null) 
                {
                    if (r.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_HIGHER, null, true) != null) 
                        return Pullenti.Ner.Org.OrganizationKind.Department;
                }
            }
            if ((t.Contains("академия") || t.Contains("институт") || t.Contains("академія")) || t.Contains("інститут")) 
            {
                if (n != null && (((n.Contains("НАУК") || n.Contains("НАУЧН") || n.Contains("НАУКОВ")) || n.Contains("ИССЛЕДОВАТ") || n.Contains("ДОСЛІДН")))) 
                    return Pullenti.Ner.Org.OrganizationKind.Science;
            }
            if (t.Contains("аэропорт") || t.Contains("аеропорт")) 
                return Pullenti.Ner.Org.OrganizationKind.Airport;
            if (t.Contains(" порт")) 
                return Pullenti.Ner.Org.OrganizationKind.Seaport;
            if (((t.Contains("фестиваль") || t.Contains("чемпионат") || t.Contains("олимпиада")) || t.Contains("конкурс") || t.Contains("чемпіонат")) || t.Contains("олімпіада")) 
                return Pullenti.Ner.Org.OrganizationKind.Festival;
            if (((((((((t.Contains("армия") || t.Contains("генеральный штаб") || t.Contains("войсковая часть")) || t.Contains("армія") || t.Contains("генеральний штаб")) || t.Contains("військова частина") || t.Contains("дивизия")) || t.Contains("полк") || t.Contains("батальон")) || t.Contains("рота") || t.Contains("взвод")) || t.Contains("дивізія") || t.Contains("батальйон")) || t.Contains("гарнизон") || t.Contains("гарнізон")) || t.Contains("бригада") || t.Contains("корпус")) || t.Contains("дивизион") || t.Contains("дивізіон")) 
                return Pullenti.Ner.Org.OrganizationKind.Military;
            if (((t.Contains("партия") || t.Contains("движение") || t.Contains("группировка")) || t.Contains("партія") || t.Contains("рух;")) || t.Contains("групування")) 
                return Pullenti.Ner.Org.OrganizationKind.Party;
            if (((((((t.Contains("газета") || t.Contains("издательство") || t.Contains("информационное агентство")) || tn.Contains("риа;") || t.Contains("журнал")) || t.Contains("издание") || t.Contains("еженедельник")) || t.Contains("таблоид") || t.Contains("видавництво")) || t.Contains("інформаційне агентство") || t.Contains("журнал")) || t.Contains("видання") || t.Contains("тижневик")) || t.Contains("таблоїд") || t.Contains("портал")) 
                return Pullenti.Ner.Org.OrganizationKind.Press;
            if (((t.Contains("телеканал") || t.Contains("телекомпания") || t.Contains("радиостанция")) || t.Contains("киностудия") || t.Contains("телекомпанія")) || t.Contains("радіостанція") || t.Contains("кіностудія")) 
                return Pullenti.Ner.Org.OrganizationKind.Media;
            if (((t.Contains("завод;") || t.Contains("фабрика") || t.Contains("комбинат")) || t.Contains("производитель") || t.Contains("комбінат")) || t.Contains("виробник")) 
                return Pullenti.Ner.Org.OrganizationKind.Factory;
            if ((((((t.Contains("театр;") || t.Contains("концертный зал") || t.Contains("музей")) || t.Contains("консерватория") || t.Contains("филармония")) || t.Contains("галерея") || t.Contains("театр студия")) || t.Contains("дом культуры") || t.Contains("концертний зал")) || t.Contains("консерваторія") || t.Contains("філармонія")) || t.Contains("театр студія") || t.Contains("будинок культури")) 
                return Pullenti.Ner.Org.OrganizationKind.Culture;
            if (((((((t.Contains("федерация") || t.Contains("союз") || t.Contains("объединение")) || t.Contains("фонд;") || t.Contains("ассоциация")) || t.Contains("клуб") || t.Contains("альянс")) || t.Contains("ассамблея") || t.Contains("федерація")) || t.Contains("обєднання") || t.Contains("фонд;")) || t.Contains("асоціація") || t.Contains("асамблея")) || t.Contains("гильдия") || t.Contains("гільдія")) 
                return Pullenti.Ner.Org.OrganizationKind.Federation;
            if ((((((t.Contains("пансионат") || t.Contains("санаторий") || t.Contains("дом отдыха")) || t.Contains("база отдыха") || t.Contains("гостиница")) || t.Contains("отель") || t.Contains("лагерь")) || t.Contains("пансіонат") || t.Contains("санаторій")) || t.Contains("будинок відпочинку") || t.Contains("база відпочинку")) || t.Contains("готель") || t.Contains("табір")) 
                return Pullenti.Ner.Org.OrganizationKind.Hotel;
            if ((((((t.Contains("суд;") || t.Contains("колония") || t.Contains("изолятор")) || t.Contains("тюрьма") || t.Contains("прокуратура")) || t.Contains("судебный") || t.Contains("трибунал")) || t.Contains("колонія") || t.Contains("ізолятор")) || t.Contains("вязниця") || t.Contains("судовий")) || t.Contains("трибунал")) 
                return Pullenti.Ner.Org.OrganizationKind.Justice;
            if (tn.Contains("банк") || tn.Contains("казначейство")) 
                return Pullenti.Ner.Org.OrganizationKind.Bank;
            if (tn.Contains("торгов") || tn.Contains("магазин") || tn.Contains("маркет;")) 
                return Pullenti.Ner.Org.OrganizationKind.Trade;
            if (t.Contains("УЗ;")) 
                return Pullenti.Ner.Org.OrganizationKind.Medical;
            if (t.Contains("центр;")) 
            {
                if ((tn.Contains("диагностический") || tn.Contains("медицинский") || tn.Contains("діагностичний")) || tn.Contains("медичний")) 
                    return Pullenti.Ner.Org.OrganizationKind.Medical;
                if ((r is Pullenti.Ner.Org.OrganizationReferent) && (r as Pullenti.Ner.Org.OrganizationReferent).Higher != null) 
                {
                    if ((r as Pullenti.Ner.Org.OrganizationReferent).Higher.Kind == Pullenti.Ner.Org.OrganizationKind.Department) 
                        return Pullenti.Ner.Org.OrganizationKind.Department;
                }
            }
            if (t.Contains("часть;") || t.Contains("частина;")) 
                return Pullenti.Ner.Org.OrganizationKind.Department;
            if (r != null) 
            {
                if (r.ContainsProfile(Pullenti.Ner.Org.OrgProfile.Policy)) 
                    return Pullenti.Ner.Org.OrganizationKind.Party;
                if (r.ContainsProfile(Pullenti.Ner.Org.OrgProfile.Media)) 
                    return Pullenti.Ner.Org.OrganizationKind.Media;
            }
            return Pullenti.Ner.Org.OrganizationKind.Undefined;
        }
    }
}