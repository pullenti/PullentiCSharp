/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Morph
{
    /// <summary>
    /// Базовая часть морфологической информации
    /// </summary>
    public class MorphBaseInfo
    {
        /// <summary>
        /// Часть речи
        /// </summary>
        public virtual MorphClass Class
        {
            get
            {
                return m_Cla;
            }
            set
            {
                m_Cla = value;
            }
        }
        MorphClass m_Cla = new MorphClass();
        /// <summary>
        /// Род
        /// </summary>
        public virtual MorphGender Gender
        {
            get;
            set;
        }
        /// <summary>
        /// Число
        /// </summary>
        public virtual MorphNumber Number
        {
            get;
            set;
        }
        /// <summary>
        /// Падеж
        /// </summary>
        public virtual MorphCase Case
        {
            get
            {
                return m_Cas;
            }
            set
            {
                m_Cas = value;
            }
        }
        MorphCase m_Cas = new MorphCase();
        /// <summary>
        /// Язык
        /// </summary>
        public virtual MorphLang Language
        {
            get
            {
                return m_Lang;
            }
            set
            {
                m_Lang = value;
            }
        }
        MorphLang m_Lang = new MorphLang();
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (!Class.IsUndefined) 
                res.AppendFormat("{0} ", Class.ToString());
            if (Number != MorphNumber.Undefined) 
            {
                if (Number == MorphNumber.Singular) 
                    res.Append("ед.ч. ");
                else if (Number == MorphNumber.Plural) 
                    res.Append("мн.ч. ");
                else 
                    res.Append("ед.мн.ч. ");
            }
            if (Gender != MorphGender.Undefined) 
            {
                if (Gender == MorphGender.Masculine) 
                    res.Append("муж.р. ");
                else if (Gender == MorphGender.Neuter) 
                    res.Append("ср.р. ");
                else if (Gender == MorphGender.Feminie) 
                    res.Append("жен.р. ");
                else if (Gender == ((MorphGender.Masculine | MorphGender.Neuter))) 
                    res.AppendFormat("муж.ср.р. ");
                else if (Gender == ((MorphGender.Feminie | MorphGender.Neuter))) 
                    res.AppendFormat("жен.ср.р. ");
                else if (((int)Gender) == 7) 
                    res.AppendFormat("муж.жен.ср.р. ");
                else if (Gender == ((MorphGender.Feminie | MorphGender.Masculine))) 
                    res.AppendFormat("муж.жен.р. ");
            }
            if (!Case.IsUndefined) 
                res.AppendFormat("{0} ", Case.ToString());
            if (!Language.IsUndefined && Language != MorphLang.RU) 
                res.AppendFormat("{0} ", Language.ToString());
            return res.ToString().TrimEnd();
        }
        public void CopyFrom(MorphBaseInfo src)
        {
            MorphClass cla = new MorphClass();
            cla.Value = src.Class.Value;
            Class = cla;
            Gender = src.Gender;
            Number = src.Number;
            MorphCase cas = new MorphCase();
            cas.Value = src.Case.Value;
            Case = cas;
            MorphLang lng = new MorphLang();
            lng.Value = src.Language.Value;
            Language = lng;
        }
        public virtual bool ContainsAttr(string attrValue, MorphClass cla = null)
        {
            return false;
        }
        public virtual bool CheckAccord(MorphBaseInfo v, bool ignoreGender = false, bool ignoreNumber = false)
        {
            if (v.Language != Language) 
            {
                if (v.Language == MorphLang.Unknown && Language == MorphLang.Unknown) 
                    return false;
            }
            MorphNumber num = v.Number & Number;
            if (num == MorphNumber.Undefined && !ignoreNumber) 
            {
                if (v.Number != MorphNumber.Undefined && Number != MorphNumber.Undefined) 
                {
                    if (v.Number == MorphNumber.Singular && v.Case.IsGenitive) 
                    {
                        if (Number == MorphNumber.Plural && Case.IsGenitive) 
                        {
                            if (((v.Gender & MorphGender.Masculine)) == MorphGender.Masculine) 
                                return true;
                        }
                    }
                    return false;
                }
            }
            if (!ignoreGender && num != MorphNumber.Plural) 
            {
                if (((v.Gender & Gender)) == MorphGender.Undefined) 
                {
                    if (v.Gender != MorphGender.Undefined && Gender != MorphGender.Undefined) 
                        return false;
                }
            }
            if (((v.Case & Case)).IsUndefined) 
            {
                if (!v.Case.IsUndefined && !Case.IsUndefined) 
                    return false;
            }
            return true;
        }
    }
}