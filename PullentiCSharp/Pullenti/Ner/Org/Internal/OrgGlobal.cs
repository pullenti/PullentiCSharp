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
using System.Xml;

namespace Pullenti.Ner.Org.Internal
{
    static class OrgGlobal
    {
        public static Pullenti.Ner.Core.IntOntologyCollection GlobalOrgs = null;
        public static Pullenti.Ner.Core.IntOntologyCollection GlobalOrgsUa = new Pullenti.Ner.Core.IntOntologyCollection();
        public static void Initialize()
        {
            if (GlobalOrgs != null) 
                return;
            GlobalOrgs = new Pullenti.Ner.Core.IntOntologyCollection();
            Pullenti.Ner.Org.OrganizationReferent org;
            Pullenti.Ner.Core.IntOntologyItem oi;
            using (Pullenti.Ner.Processor geoProc = Pullenti.Ner.ProcessorService.CreateEmptyProcessor()) 
            {
                geoProc.AddAnalyzer(new Pullenti.Ner.Geo.GeoAnalyzer());
                Dictionary<string, Pullenti.Ner.Geo.GeoReferent> geos = new Dictionary<string, Pullenti.Ner.Geo.GeoReferent>();
                for (int k = 0; k < 3; k++) 
                {
                    Pullenti.Morph.MorphLang lang = (k == 0 ? Pullenti.Morph.MorphLang.RU : (k == 1 ? Pullenti.Morph.MorphLang.EN : Pullenti.Morph.MorphLang.UA));
                    string name = (k == 0 ? "Orgs_ru.dat" : (k == 1 ? "Orgs_en.dat" : "Orgs_ua.dat"));
                    byte[] dat = ResourceHelper.GetBytes(name);
                    if (dat == null) 
                        throw new Exception(string.Format("Can't file resource file {0} in Organization analyzer", name));
                    using (MemoryStream tmp = new MemoryStream(OrgItemTypeToken.Deflate(dat))) 
                    {
                        tmp.Position = 0;
                        XmlDocument xml = new XmlDocument();
                        xml.Load(tmp);
                        foreach (XmlNode x in xml.DocumentElement.ChildNodes) 
                        {
                            org = new Pullenti.Ner.Org.OrganizationReferent();
                            string abbr = null;
                            foreach (XmlNode xx in x.ChildNodes) 
                            {
                                if (xx.LocalName == "typ") 
                                    org.AddSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_TYPE, xx.InnerText, false, 0);
                                else if (xx.LocalName == "nam") 
                                    org.AddSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_NAME, xx.InnerText, false, 0);
                                else if (xx.LocalName == "epo") 
                                    org.AddSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_EPONYM, xx.InnerText, false, 0);
                                else if (xx.LocalName == "prof") 
                                    org.AddSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_PROFILE, xx.InnerText, false, 0);
                                else if (xx.LocalName == "abbr") 
                                    abbr = xx.InnerText;
                                else if (xx.LocalName == "geo") 
                                {
                                    Pullenti.Ner.Geo.GeoReferent geo;
                                    if (!geos.TryGetValue(xx.InnerText, out geo)) 
                                    {
                                        Pullenti.Ner.AnalysisResult ar = geoProc.Process(new Pullenti.Ner.SourceOfAnalysis(xx.InnerText), null, lang);
                                        if (ar != null && ar.Entities.Count == 1 && (ar.Entities[0] is Pullenti.Ner.Geo.GeoReferent)) 
                                        {
                                            geo = ar.Entities[0] as Pullenti.Ner.Geo.GeoReferent;
                                            geos.Add(xx.InnerText, geo);
                                        }
                                        else 
                                        {
                                        }
                                    }
                                    if (geo != null) 
                                        org.AddSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_GEO, geo, false, 0);
                                }
                            }
                            oi = org.CreateOntologyItemEx(2, true, true);
                            if (oi == null) 
                                continue;
                            if (abbr != null) 
                                oi.Termins.Add(new Pullenti.Ner.Core.Termin(abbr, null, true));
                            if (k == 2) 
                                GlobalOrgsUa.AddItem(oi);
                            else 
                                GlobalOrgs.AddItem(oi);
                        }
                    }
                }
            }
            return;
        }
    }
}