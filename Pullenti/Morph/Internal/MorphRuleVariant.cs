/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Morph.Internal
{
    public class MorphRuleVariant : Pullenti.Morph.MorphBaseInfo
    {
        public MorphRuleVariant()
        {
        }
        public void CopyFromVariant(MorphRuleVariant src)
        {
            if (src == null) 
                return;
            Tail = src.Tail;
            this.CopyFrom(src);
            MiscInfoId = src.MiscInfoId;
            NormalTail = src.NormalTail;
            FullNormalTail = src.FullNormalTail;
            RuleId = src.RuleId;
        }
        public string Tail;
        public short MiscInfoId;
        public short RuleId;
        public short Id;
        public string NormalTail;
        public string FullNormalTail;
        public object Tag;
        public override string ToString()
        {
            return this.ToStringEx(false);
        }
        public string ToStringEx(bool hideTails)
        {
            StringBuilder res = new StringBuilder();
            if (!hideTails) 
            {
                res.AppendFormat("-{0}", Tail);
                if (NormalTail != null) 
                    res.AppendFormat(" [-{0}]", NormalTail);
                if (FullNormalTail != null && FullNormalTail != NormalTail) 
                    res.AppendFormat(" [-{0}]", FullNormalTail);
            }
            res.AppendFormat(" {0}", base.ToString());
            return res.ToString().Trim();
        }
        public bool Compare(MorphRuleVariant mrv)
        {
            if ((mrv.Class != Class || mrv.Gender != Gender || mrv.Number != Number) || mrv.Case != Case) 
                return false;
            if (mrv.MiscInfoId != MiscInfoId) 
                return false;
            if (mrv.NormalTail != NormalTail) 
                return false;
            return true;
        }
        internal bool Deserialize(ByteArrayWrapper str, ref int pos)
        {
            int id = str.DeserializeShort(ref pos);
            if (id <= 0) 
                return false;
            MiscInfoId = (short)id;
            int iii = str.DeserializeShort(ref pos);
            Pullenti.Morph.MorphClass mc = new Pullenti.Morph.MorphClass();
            mc.Value = (short)iii;
            if (mc.IsMisc && mc.IsProper) 
                mc.IsMisc = false;
            Class = mc;
            byte bbb;
            bbb = str.DeserializeByte(ref pos);
            Gender = (Pullenti.Morph.MorphGender)bbb;
            bbb = str.DeserializeByte(ref pos);
            Number = (Pullenti.Morph.MorphNumber)bbb;
            bbb = str.DeserializeByte(ref pos);
            Pullenti.Morph.MorphCase mca = new Pullenti.Morph.MorphCase();
            mca.Value = bbb;
            Case = mca;
            string s = str.DeserializeString(ref pos);
            NormalTail = s;
            s = str.DeserializeString(ref pos);
            FullNormalTail = s;
            return true;
        }
    }
}