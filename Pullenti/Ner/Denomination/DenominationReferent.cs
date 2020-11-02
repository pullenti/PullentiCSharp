/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Denomination
{
    /// <summary>
    /// Сущность, моделирующая буквенно-цифровые комбинации (например, Си++, СС-300)
    /// </summary>
    public class DenominationReferent : Pullenti.Ner.Referent
    {
        public DenominationReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Denomination.Internal.MetaDenom.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("DENOMINATION")
        /// </summary>
        public const string OBJ_TYPENAME = "DENOMINATION";
        /// <summary>
        /// Имя атрибута - значение
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Значение (одно или несколько)
        /// </summary>
        public string Value
        {
            get
            {
                return this.GetStringValue(ATTR_VALUE);
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            return Value ?? "?";
        }
        internal void AddValue(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            StringBuilder tmp = new StringBuilder();
            for (Pullenti.Ner.Token t = begin; t != null && t.Previous != end; t = t.Next) 
            {
                if (t is Pullenti.Ner.NumberToken) 
                {
                    tmp.Append(t.GetSourceText());
                    continue;
                }
                if (t is Pullenti.Ner.TextToken) 
                {
                    string s = (t as Pullenti.Ner.TextToken).Term;
                    if (t.IsCharOf("-\\/")) 
                        s = "-";
                    tmp.Append(s);
                }
            }
            for (int i = 0; i < tmp.Length; i++) 
            {
                if (tmp[i] == '-' && i > 0 && ((i + 1) < tmp.Length)) 
                {
                    char ch0 = tmp[i - 1];
                    char ch1 = tmp[i + 1];
                    if (char.IsLetterOrDigit(ch0) && char.IsLetterOrDigit(ch1)) 
                    {
                        if (char.IsDigit(ch0) && !char.IsDigit(ch1)) 
                            tmp.Remove(i, 1);
                        else if (!char.IsDigit(ch0) && char.IsDigit(ch1)) 
                            tmp.Remove(i, 1);
                    }
                }
            }
            this.AddSlot(ATTR_VALUE, tmp.ToString(), false, 0);
            m_Names = null;
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            DenominationReferent dr = obj as DenominationReferent;
            if (dr == null) 
                return false;
            foreach (string n in NameVars) 
            {
                if (dr.NameVars.Contains(n)) 
                    return true;
            }
            return false;
        }
        List<string> m_Names;
        List<string> NameVars
        {
            get
            {
                if (m_Names != null) 
                    return m_Names;
                m_Names = new List<string>();
                string nam = Value;
                if (nam == null) 
                    return m_Names;
                m_Names.Add(nam);
                List<List<string>> items = new List<List<string>>();
                int i;
                int ty0 = 0;
                int i0 = 0;
                for (i = 0; i <= nam.Length; i++) 
                {
                    int ty = 0;
                    if (i < nam.Length) 
                    {
                        if (char.IsDigit(nam[i])) 
                            ty = 1;
                        else if (char.IsLetter(nam[i])) 
                            ty = 2;
                        else 
                            ty = 3;
                    }
                    if (ty != ty0 || ty == 3) 
                    {
                        if (i > i0) 
                        {
                            List<string> vars = new List<string>();
                            string p = nam.Substring(i0, i - i0);
                            AddVars(p, vars);
                            items.Add(vars);
                            if (ty == 1 && ty0 == 2) 
                            {
                                vars = new List<string>();
                                vars.Add("");
                                vars.Add("-");
                                items.Add(vars);
                            }
                        }
                        i0 = i;
                        ty0 = ty;
                    }
                }
                int[] inds = new int[items.Count];
                for (i = 0; i < inds.Length; i++) 
                {
                    inds[i] = 0;
                }
                StringBuilder tmp = new StringBuilder();
                while (true) 
                {
                    tmp.Length = 0;
                    for (i = 0; i < items.Count; i++) 
                    {
                        tmp.Append(items[i][inds[i]]);
                    }
                    string v = tmp.ToString();
                    if (!m_Names.Contains(v)) 
                        m_Names.Add(v);
                    if (m_Names.Count > 20) 
                        break;
                    for (i = inds.Length - 1; i >= 0; i--) 
                    {
                        inds[i]++;
                        if (inds[i] < items[i].Count) 
                            break;
                    }
                    if (i < 0) 
                        break;
                    for (++i; i < inds.Length; i++) 
                    {
                        inds[i] = 0;
                    }
                }
                return m_Names;
            }
        }
        static void AddVars(string str, List<string> vars)
        {
            vars.Add(str);
            for (int k = 0; k < 2; k++) 
            {
                int i;
                StringBuilder tmp = new StringBuilder();
                for (i = 0; i < str.Length; i++) 
                {
                    string v;
                    if (!m_VarChars.TryGetValue(str[i], out v)) 
                        break;
                    if ((v.Length < 2) || v[k] == '-') 
                        break;
                    tmp.Append(v[k]);
                }
                if (i >= str.Length) 
                {
                    string v = tmp.ToString();
                    if (!vars.Contains(v)) 
                        vars.Add(v);
                }
            }
        }
        static Dictionary<char, string> m_VarChars;
        static DenominationReferent()
        {
            m_VarChars = new Dictionary<char, string>();
            m_VarChars.Add('A', "АА");
            m_VarChars.Add('B', "БВ");
            m_VarChars.Add('C', "ЦС");
            m_VarChars.Add('D', "ДД");
            m_VarChars.Add('E', "ЕЕ");
            m_VarChars.Add('F', "Ф-");
            m_VarChars.Add('G', "Г-");
            m_VarChars.Add('H', "ХН");
            m_VarChars.Add('I', "И-");
            m_VarChars.Add('J', "Ж-");
            m_VarChars.Add('K', "КК");
            m_VarChars.Add('L', "Л-");
            m_VarChars.Add('M', "ММ");
            m_VarChars.Add('N', "Н-");
            m_VarChars.Add('O', "ОО");
            m_VarChars.Add('P', "ПР");
            m_VarChars.Add('Q', "--");
            m_VarChars.Add('R', "Р-");
            m_VarChars.Add('S', "С-");
            m_VarChars.Add('T', "ТТ");
            m_VarChars.Add('U', "У-");
            m_VarChars.Add('V', "В-");
            m_VarChars.Add('W', "В-");
            m_VarChars.Add('X', "ХХ");
            m_VarChars.Add('Y', "УУ");
            m_VarChars.Add('Z', "З-");
            m_VarChars.Add('А', "AA");
            m_VarChars.Add('Б', "B-");
            m_VarChars.Add('В', "VB");
            m_VarChars.Add('Г', "G-");
            m_VarChars.Add('Д', "D-");
            m_VarChars.Add('Е', "EE");
            m_VarChars.Add('Ж', "J-");
            m_VarChars.Add('З', "Z-");
            m_VarChars.Add('И', "I-");
            m_VarChars.Add('Й', "Y-");
            m_VarChars.Add('К', "KK");
            m_VarChars.Add('Л', "L-");
            m_VarChars.Add('М', "MM");
            m_VarChars.Add('Н', "NH");
            m_VarChars.Add('О', "OO");
            m_VarChars.Add('П', "P-");
            m_VarChars.Add('Р', "RP");
            m_VarChars.Add('С', "SC");
            m_VarChars.Add('Т', "TT");
            m_VarChars.Add('У', "UY");
            m_VarChars.Add('Ф', "F-");
            m_VarChars.Add('Х', "HX");
            m_VarChars.Add('Ц', "C-");
            m_VarChars.Add('Ч', "--");
            m_VarChars.Add('Ш', "--");
            m_VarChars.Add('Щ', "--");
            m_VarChars.Add('Ы', "--");
            m_VarChars.Add('Э', "A-");
            m_VarChars.Add('Ю', "U-");
            m_VarChars.Add('Я', "--");
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            Pullenti.Ner.Core.IntOntologyItem oi = new Pullenti.Ner.Core.IntOntologyItem(this);
            foreach (string v in NameVars) 
            {
                oi.Termins.Add(new Pullenti.Ner.Core.Termin(v));
            }
            return oi;
        }
    }
}