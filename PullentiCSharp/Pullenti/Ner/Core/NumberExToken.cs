/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    // Число с стандартный постфиксом (мерой длины, вес, деньги и т.п.)
    // Устарело, вместо этого лучше использовать MeasureReferent или NumbersWithUnitToken
    public class NumberExToken : Pullenti.Ner.NumberToken
    {
        public double AltRealValue;
        public int AltRestMoney = 0;
        public NumberExType ExTyp = NumberExType.Undefined;
        public NumberExType ExTyp2 = NumberExType.Undefined;
        public string ExTypParam;
        public bool MultAfter;
        internal NumberExToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end, string val, Pullenti.Ner.NumberSpellingType typ, NumberExType exTyp = NumberExType.Undefined) : base(begin, end, val, typ, null)
        {
            ExTyp = exTyp;
        }
        public double NormalizeValue(ref NumberExType ty)
        {
            double val = RealValue;
            NumberExType ety = ExTyp;
            if (ty == ety) 
                return val;
            if (ExTyp2 != NumberExType.Undefined) 
                return val;
            if (ty == NumberExType.Gramm) 
            {
                if (ExTyp == NumberExType.Kilogram) 
                {
                    val *= 1000;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Milligram) 
                {
                    val /= 1000;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Tonna) 
                {
                    val *= 1000000;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Kilogram) 
            {
                if (ExTyp == NumberExType.Gramm) 
                {
                    val /= 1000;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Tonna) 
                {
                    val *= 1000;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Tonna) 
            {
                if (ExTyp == NumberExType.Kilogram) 
                {
                    val /= 1000;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Gramm) 
                {
                    val /= 1000000;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Millimeter) 
            {
                if (ExTyp == NumberExType.Santimeter) 
                {
                    val *= 10;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Meter) 
                {
                    val *= 1000;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Santimeter) 
            {
                if (ExTyp == NumberExType.Millimeter) 
                {
                    val *= 10;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Meter) 
                {
                    val *= 100;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Meter) 
            {
                if (ExTyp == NumberExType.Kilometer) 
                {
                    val *= 1000;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Litr) 
            {
                if (ExTyp == NumberExType.Millilitr) 
                {
                    val /= 1000;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Millilitr) 
            {
                if (ExTyp == NumberExType.Litr) 
                {
                    val *= 1000;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Gektar) 
            {
                if (ExTyp == NumberExType.Meter2) 
                {
                    val /= 10000;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Ar) 
                {
                    val /= 100;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Kilometer2) 
                {
                    val *= 100;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Kilometer2) 
            {
                if (ExTyp == NumberExType.Gektar) 
                {
                    val /= 100;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Ar) 
                {
                    val /= 10000;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Meter2) 
                {
                    val /= 1000000;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Meter2) 
            {
                if (ExTyp == NumberExType.Ar) 
                {
                    val *= 100;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Gektar) 
                {
                    val *= 10000;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Kilometer2) 
                {
                    val *= 1000000;
                    ety = ty;
                }
            }
            else if (ty == NumberExType.Day) 
            {
                if (ExTyp == NumberExType.Year) 
                {
                    val *= 365;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Month) 
                {
                    val *= 30;
                    ety = ty;
                }
                else if (ExTyp == NumberExType.Week) 
                {
                    val *= 7;
                    ety = ty;
                }
            }
            ty = ety;
            return val;
        }
        public static string ExTypToString(NumberExType ty, NumberExType ty2 = NumberExType.Undefined)
        {
            if (ty2 != NumberExType.Undefined) 
                return string.Format("{0}/{1}", ExTypToString(ty, NumberExType.Undefined), ExTypToString(ty2, NumberExType.Undefined));
            string res;
            if (Pullenti.Ner.Core.Internal.NumberExHelper.m_NormalsTyps.TryGetValue(ty, out res)) 
                return res;
            return "?";
        }
        public override string ToString()
        {
            return string.Format("{0}{1}", RealValue, ExTypParam ?? ExTypToString(ExTyp, ExTyp2));
        }
    }
}