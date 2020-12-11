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

namespace Pullenti.Semantic.Internal
{
    public class AlgoParams
    {
        public double TransitiveCoef = 1;
        public double NextModel = 1;
        public double NgLink = 1;
        public double List = 2;
        public double VerbPlural = 2;
        public double CaseAccord = 1;
        public double MorphAccord = 1;
        public void CopyFrom(AlgoParams src)
        {
            TransitiveCoef = src.TransitiveCoef;
            NextModel = src.NextModel;
            NgLink = src.NgLink;
            List = src.List;
            VerbPlural = src.VerbPlural;
            CaseAccord = src.CaseAccord;
            MorphAccord = src.MorphAccord;
        }
        public void CopyFromParams()
        {
            foreach (AlgoParam p in Params) 
            {
                if (p.Name == "TransitiveCoef") 
                    TransitiveCoef = p.Value;
                else if (p.Name == "NextModel") 
                    NextModel = p.Value;
                else if (p.Name == "NgLink") 
                    NgLink = p.Value;
                else if (p.Name == "List") 
                    List = p.Value;
                else if (p.Name == "VerbPlural") 
                    VerbPlural = p.Value;
                else if (p.Name == "CaseAccord") 
                    CaseAccord = p.Value;
                else if (p.Name == "MorphAccord") 
                    MorphAccord = p.Value;
            }
        }
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            tmp.AppendFormat("TransitiveCoef = {0} \r\n", TransitiveCoef);
            tmp.AppendFormat("NextModel = {0} \r\n", NextModel);
            tmp.AppendFormat("NgLink = {0} \r\n", NgLink);
            tmp.AppendFormat("List = {0} \r\n", List);
            tmp.AppendFormat("VerbPlural = {0} \r\n", VerbPlural);
            tmp.AppendFormat("CaseAccord = {0} \r\n", CaseAccord);
            tmp.AppendFormat("MorphAccord = {0} \r\n", MorphAccord);
            return tmp.ToString();
        }
        public static List<AlgoParam> Params;
        static AlgoParams()
        {
            Params = new List<AlgoParam>();
            Params.Add(new AlgoParam() { Name = "TransitiveCoef", Min = 1, Max = 4, Delta = 1 });
            Params.Add(new AlgoParam() { Name = "NextModel", Min = 1, Max = 4, Delta = 1 });
            Params.Add(new AlgoParam() { Name = "NgLink", Min = 1, Max = 3, Delta = 1 });
            Params.Add(new AlgoParam() { Name = "List", Min = 1, Max = 4, Delta = 1 });
            Params.Add(new AlgoParam() { Name = "VerbPlural", Min = 1, Max = 4, Delta = 1 });
            Params.Add(new AlgoParam() { Name = "CaseAccord", Min = 1, Max = 3, Delta = 1 });
            Params.Add(new AlgoParam() { Name = "MorphAccord", Min = 1, Max = 3, Delta = 1 });
        }
    }
}