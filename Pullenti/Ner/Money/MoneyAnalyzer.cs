/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Money
{
    /// <summary>
    /// Анализатор для денежных сумм
    /// </summary>
    public class MoneyAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("MONEY")
        /// </summary>
        public const string ANALYZER_NAME = "MONEY";
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Деньги";
            }
        }
        public override string Description
        {
            get
            {
                return "Деньги...";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new MoneyAnalyzer();
        }
        public override int ProgressWeight
        {
            get
            {
                return 1;
            }
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Money.Internal.MoneyMeta.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Money.Internal.MoneyMeta.ImageId, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("money2.png"));
                res.Add(Pullenti.Ner.Money.Internal.MoneyMeta.Image2Id, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("moneyerr.png"));
                return res;
            }
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {"GEO", "DATE"};
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == MoneyReferent.OBJ_TYPENAME) 
                return new MoneyReferent();
            return null;
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.ReferentToken mon = TryParse(t);
                if (mon != null) 
                {
                    mon.Referent = ad.RegisterReferent(mon.Referent);
                    kit.EmbedToken(mon);
                    t = mon;
                    continue;
                }
            }
        }
        public static Pullenti.Ner.ReferentToken TryParse(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if (!(t is Pullenti.Ner.NumberToken) && t.LengthChar != 1) 
                return null;
            Pullenti.Ner.Core.NumberExToken nex = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(t);
            if (nex == null || nex.ExTyp != Pullenti.Ner.Core.NumberExType.Money) 
            {
                if ((t is Pullenti.Ner.NumberToken) && (t.Next is Pullenti.Ner.TextToken) && (t.Next.Next is Pullenti.Ner.NumberToken)) 
                {
                    if (t.Next.IsHiphen || t.Next.Morph.Class.IsPreposition) 
                    {
                        Pullenti.Ner.Core.NumberExToken res1 = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(t.Next.Next);
                        if (res1 != null && res1.ExTyp == Pullenti.Ner.Core.NumberExType.Money) 
                        {
                            MoneyReferent res0 = new MoneyReferent();
                            if ((t.Next.IsHiphen && res1.RealValue == 0 && res1.EndToken.Next != null) && res1.EndToken.Next.IsChar('(')) 
                            {
                                Pullenti.Ner.Core.NumberExToken nex2 = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(res1.EndToken.Next.Next);
                                if ((nex2 != null && nex2.ExTypParam == res1.ExTypParam && nex2.EndToken.Next != null) && nex2.EndToken.Next.IsChar(')')) 
                                {
                                    if (nex2.Value == (t as Pullenti.Ner.NumberToken).Value) 
                                    {
                                        res0.Currency = nex2.ExTypParam;
                                        res0.AddSlot(MoneyReferent.ATTR_VALUE, nex2.Value, true, 0);
                                        return new Pullenti.Ner.ReferentToken(res0, t, nex2.EndToken.Next);
                                    }
                                    if (t.Previous is Pullenti.Ner.NumberToken) 
                                    {
                                        if (nex2.Value == ((((t.Previous as Pullenti.Ner.NumberToken).RealValue * 1000) + (t as Pullenti.Ner.NumberToken).Value))) 
                                        {
                                            res0.Currency = nex2.ExTypParam;
                                            res0.AddSlot(MoneyReferent.ATTR_VALUE, nex2.Value, true, 0);
                                            return new Pullenti.Ner.ReferentToken(res0, t.Previous, nex2.EndToken.Next);
                                        }
                                        else if (t.Previous.Previous is Pullenti.Ner.NumberToken) 
                                        {
                                            if (nex2.RealValue == ((((t.Previous.Previous as Pullenti.Ner.NumberToken).RealValue * 1000000) + ((t.Previous as Pullenti.Ner.NumberToken).RealValue * 1000) + (t as Pullenti.Ner.NumberToken).RealValue))) 
                                            {
                                                res0.Currency = nex2.ExTypParam;
                                                res0.AddSlot(MoneyReferent.ATTR_VALUE, nex2.Value, true, 0);
                                                return new Pullenti.Ner.ReferentToken(res0, t.Previous.Previous, nex2.EndToken.Next);
                                            }
                                        }
                                    }
                                }
                            }
                            res0.Currency = res1.ExTypParam;
                            res0.AddSlot(MoneyReferent.ATTR_VALUE, (t as Pullenti.Ner.NumberToken).Value, false, 0);
                            return new Pullenti.Ner.ReferentToken(res0, t, t);
                        }
                    }
                }
                return null;
            }
            MoneyReferent res = new MoneyReferent();
            res.Currency = nex.ExTypParam;
            string val = nex.Value;
            if (val.IndexOf('.') > 0) 
                val = val.Substring(0, val.IndexOf('.'));
            res.AddSlot(MoneyReferent.ATTR_VALUE, val, true, 0);
            int re = (int)Math.Round(((nex.RealValue - res.Value)) * 100, 6);
            if (re != 0) 
                res.AddSlot(MoneyReferent.ATTR_REST, re.ToString(), true, 0);
            if (nex.RealValue != nex.AltRealValue) 
            {
                if (Math.Floor(res.Value) != Math.Floor(nex.AltRealValue)) 
                {
                    val = Pullenti.Ner.Core.NumberHelper.DoubleToString(nex.AltRealValue);
                    if (val.IndexOf('.') > 0) 
                        val = val.Substring(0, val.IndexOf('.'));
                    res.AddSlot(MoneyReferent.ATTR_ALTVALUE, val, true, 0);
                }
                re = (int)Math.Round(((nex.AltRealValue - ((long)nex.AltRealValue))) * 100, 6);
                if (re != res.Rest && re != 0) 
                    res.AddSlot(MoneyReferent.ATTR_ALTREST, (re).ToString(), true, 0);
            }
            if (nex.AltRestMoney > 0) 
                res.AddSlot(MoneyReferent.ATTR_ALTREST, nex.AltRestMoney.ToString(), true, 0);
            Pullenti.Ner.Token t1 = nex.EndToken;
            if (t1.Next != null && t1.Next.IsChar('(')) 
            {
                Pullenti.Ner.ReferentToken rt = TryParse(t1.Next.Next);
                if ((rt != null && rt.Referent.CanBeEquals(res, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText) && rt.EndToken.Next != null) && rt.EndToken.Next.IsChar(')')) 
                    t1 = rt.EndToken.Next;
                else 
                {
                    rt = TryParse(t1.Next);
                    if (rt != null && rt.Referent.CanBeEquals(res, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                        t1 = rt.EndToken;
                }
            }
            if (res.AltValue != null && res.AltValue.Value > res.Value) 
            {
                if (t.WhitespacesBeforeCount == 1 && (t.Previous is Pullenti.Ner.NumberToken)) 
                {
                    int delt = (int)((res.AltValue.Value - res.Value));
                    if ((((res.Value < 1000) && ((delt % 1000)) == 0)) || (((res.Value < 1000000) && ((delt % 1000000)) == 0))) 
                    {
                        t = t.Previous;
                        res.AddSlot(MoneyReferent.ATTR_VALUE, res.GetStringValue(MoneyReferent.ATTR_ALTVALUE), true, 0);
                        res.AddSlot(MoneyReferent.ATTR_ALTVALUE, null, true, 0);
                    }
                }
            }
            return new Pullenti.Ner.ReferentToken(res, t, t1);
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            return TryParse(begin);
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Ner.Money.Internal.MoneyMeta.Initialize();
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new MoneyAnalyzer());
        }
    }
}