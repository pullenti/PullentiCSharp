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

namespace Pullenti.Ner.Person.Internal
{
    class PersonMorphCollection
    {
        public string Head;
        public class PersonMorphVariant
        {
            public string Value;
            public string ShortValue;
            public Pullenti.Morph.MorphGender Gender;
            public override string ToString()
            {
                StringBuilder res = new StringBuilder();
                res.Append(Value);
                if (ShortValue != null) 
                    res.AppendFormat(" ({0})", ShortValue);
                if (Gender != Pullenti.Morph.MorphGender.Undefined) 
                    res.AppendFormat(" {0}", Gender);
                return res.ToString();
            }
        }

        public List<PersonMorphVariant> Items = new List<PersonMorphVariant>();
        public int Number;
        public bool CheckLatinVariant(string latin)
        {
            foreach (PersonMorphVariant it in Items) 
            {
                if (Pullenti.Ner.Core.MiscHelper.CanBeEqualCyrAndLatSS(latin, it.Value)) 
                    return true;
            }
            return false;
        }
        public void Correct()
        {
            foreach (PersonMorphVariant it in Items) 
            {
                if (it.Value.IndexOf(' ') > 0) 
                    it.Value = it.Value.Replace(" ", "");
            }
            for (int i = 0; i < (Items.Count - 1); i++) 
            {
                for (int k = 0; k < (Items.Count - 1); k++) 
                {
                    if (m_Comparer.Compare(Items[k], Items[k + 1]) > 0) 
                    {
                        PersonMorphVariant it = Items[k + 1];
                        Items[k + 1] = Items[k];
                        Items[k] = it;
                    }
                }
            }
        }
        class SortComparer : IComparer<Pullenti.Ner.Person.Internal.PersonMorphCollection.PersonMorphVariant>
        {
            public int Compare(Pullenti.Ner.Person.Internal.PersonMorphCollection.PersonMorphVariant x, Pullenti.Ner.Person.Internal.PersonMorphCollection.PersonMorphVariant y)
            {
                if (x.Value.IndexOf('-') > 0) 
                {
                    if ((y.Value.IndexOf('-') < 0) && (y.Value.Length < (x.Value.Length - 1))) 
                        return -1;
                }
                else if (y.Value.IndexOf('-') > 0 && (y.Value.Length - 1) > x.Value.Length) 
                    return 1;
                if (x.Value.Length < y.Value.Length) 
                    return -1;
                if (x.Value.Length > y.Value.Length) 
                    return 1;
                return 0;
            }
        }

        static SortComparer m_Comparer = new SortComparer();
        public bool HasLastnameStandardTail
        {
            get
            {
                foreach (PersonMorphVariant it in Items) 
                {
                    if (PersonItemToken.MorphPersonItem.EndsWithStdSurname(it.Value)) 
                        return true;
                }
                return false;
            }
        }
        public void Add(string val, string shortval, Pullenti.Morph.MorphGender gen, bool addOtherGenderVar = false)
        {
            if (val == null) 
                return;
            if (Head == null) 
            {
                if (val.Length > 3) 
                    Head = val.Substring(0, 3);
                else 
                    Head = val;
            }
            if (gen == Pullenti.Morph.MorphGender.Masculine || gen == Pullenti.Morph.MorphGender.Feminie) 
            {
                foreach (PersonMorphVariant it in Items) 
                {
                    if (it.Value == val && it.Gender == gen) 
                        return;
                }
                Items.Add(new PersonMorphVariant() { Value = val, Gender = gen, ShortValue = shortval });
                if (addOtherGenderVar) 
                {
                    Pullenti.Morph.MorphGender g0 = (gen == Pullenti.Morph.MorphGender.Feminie ? Pullenti.Morph.MorphGender.Masculine : Pullenti.Morph.MorphGender.Feminie);
                    string v = Pullenti.Morph.MorphologyService.GetWordform(val, new Pullenti.Morph.MorphBaseInfo() { Class = new Pullenti.Morph.MorphClass() { IsProperSurname = true }, Gender = g0 });
                    if (v != null) 
                        Items.Add(new PersonMorphVariant() { Value = v, Gender = g0, ShortValue = shortval });
                }
            }
            else 
            {
                this.Add(val, shortval, Pullenti.Morph.MorphGender.Masculine, false);
                this.Add(val, shortval, Pullenti.Morph.MorphGender.Feminie, false);
            }
        }
        public bool Remove(string val, Pullenti.Morph.MorphGender gen)
        {
            bool ret = false;
            for (int i = Items.Count - 1; i >= 0; i--) 
            {
                if (val != null && Items[i].Value != val) 
                    continue;
                if (gen != Pullenti.Morph.MorphGender.Undefined && Items[i].Gender != gen) 
                    continue;
                Items.RemoveAt(i);
                ret = true;
            }
            return ret;
        }
        public void AddPrefixStr(string prefix)
        {
            Head = string.Format("{0}{1}", prefix, Head);
            foreach (PersonMorphVariant it in Items) 
            {
                it.Value = string.Format("{0}{1}", prefix, it.Value);
                if (it.ShortValue != null) 
                    it.Value = string.Format("{0}{1}", prefix, it.ShortValue);
            }
        }
        public static PersonMorphCollection AddPrefix(PersonMorphCollection prefix, PersonMorphCollection body)
        {
            PersonMorphCollection res = new PersonMorphCollection();
            res.Head = string.Format("{0}-{1}", prefix.Head, body.Head);
            foreach (PersonMorphVariant pv in prefix.Items) 
            {
                foreach (PersonMorphVariant bv in body.Items) 
                {
                    Pullenti.Morph.MorphGender g = bv.Gender;
                    if (g == Pullenti.Morph.MorphGender.Undefined) 
                        g = pv.Gender;
                    else if (pv.Gender != Pullenti.Morph.MorphGender.Undefined && pv.Gender != g) 
                        g = Pullenti.Morph.MorphGender.Undefined;
                    res.Add(string.Format("{0}-{1}", pv.Value, bv.Value), null, g, false);
                }
            }
            return res;
        }
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Number > 0) 
                res.AppendFormat("Num={0};", Number);
            foreach (PersonMorphVariant it in Items) 
            {
                res.AppendFormat("{0}; ", it.ToString());
            }
            return res.ToString();
        }
        public List<string> Values
        {
            get
            {
                List<string> res = new List<string>();
                foreach (PersonMorphVariant it in Items) 
                {
                    if (!res.Contains(it.Value)) 
                        res.Add(it.Value);
                    if (it.ShortValue != null && !res.Contains(it.ShortValue)) 
                        res.Add(it.ShortValue);
                }
                return res;
            }
        }
        public Pullenti.Morph.MorphGender Gender
        {
            get
            {
                Pullenti.Morph.MorphGender res = Pullenti.Morph.MorphGender.Undefined;
                foreach (PersonMorphVariant it in Items) 
                {
                    res |= it.Gender;
                }
                if (res == Pullenti.Morph.MorphGender.Feminie || res == Pullenti.Morph.MorphGender.Masculine) 
                    return res;
                else 
                    return Pullenti.Morph.MorphGender.Undefined;
            }
        }
        bool ContainsItem(string v, Pullenti.Morph.MorphGender g)
        {
            foreach (PersonMorphVariant it in Items) 
            {
                if (it.Value == v && it.Gender == g) 
                    return true;
            }
            return false;
        }
        public static bool IsEquals(PersonMorphCollection col1, PersonMorphCollection col2)
        {
            if (col1.Head != col2.Head) 
                return false;
            foreach (PersonMorphVariant v in col1.Items) 
            {
                if (!col2.ContainsItem(v.Value, v.Gender)) 
                    return false;
            }
            foreach (PersonMorphVariant v in col2.Items) 
            {
                if (!col1.ContainsItem(v.Value, v.Gender)) 
                    return false;
            }
            return true;
        }
        static bool Intersect2(PersonMorphCollection col1, PersonMorphCollection col2)
        {
            if (col1.Head != col2.Head) 
                return false;
            bool ret = false;
            List<string> vals1 = col1.Values;
            List<string> vals2 = col2.Values;
            List<string> uni = new List<string>();
            foreach (string v in vals1) 
            {
                if (vals2.Contains(v)) 
                {
                    uni.Add(v);
                    continue;
                }
            }
            foreach (string v in vals1) 
            {
                if (!uni.Contains(v)) 
                {
                    col1.Remove(v, Pullenti.Morph.MorphGender.Undefined);
                    ret = true;
                }
            }
            foreach (string v in vals2) 
            {
                if (!uni.Contains(v)) 
                {
                    col2.Remove(v, Pullenti.Morph.MorphGender.Undefined);
                    ret = true;
                }
            }
            if (col1.Gender != Pullenti.Morph.MorphGender.Undefined) 
            {
                if (col2.Remove(null, (col1.Gender == Pullenti.Morph.MorphGender.Feminie ? Pullenti.Morph.MorphGender.Masculine : Pullenti.Morph.MorphGender.Feminie))) 
                    ret = true;
            }
            if (col2.Gender != Pullenti.Morph.MorphGender.Undefined) 
            {
                if (col1.Remove(null, (col2.Gender == Pullenti.Morph.MorphGender.Feminie ? Pullenti.Morph.MorphGender.Masculine : Pullenti.Morph.MorphGender.Feminie))) 
                    ret = true;
            }
            return ret;
        }
        public static bool Intersect(List<PersonMorphCollection> list)
        {
            bool ret = false;
            while (true) 
            {
                bool ch = false;
                for (int i = 0; i < (list.Count - 1); i++) 
                {
                    for (int j = i + 1; j < list.Count; j++) 
                    {
                        if (PersonMorphCollection.Intersect2(list[i], list[j])) 
                            ch = true;
                        if (PersonMorphCollection.IsEquals(list[i], list[j])) 
                        {
                            list.RemoveAt(j);
                            j--;
                            ch = true;
                        }
                    }
                }
                if (ch) 
                    ret = true;
                else 
                    break;
            }
            return ret;
        }
        public static void SetGender(List<PersonMorphCollection> list, Pullenti.Morph.MorphGender gen)
        {
            foreach (PersonMorphCollection li in list) 
            {
                li.Remove(null, (gen == Pullenti.Morph.MorphGender.Masculine ? Pullenti.Morph.MorphGender.Feminie : Pullenti.Morph.MorphGender.Masculine));
            }
        }
    }
}