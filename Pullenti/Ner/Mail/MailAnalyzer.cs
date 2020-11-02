/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Mail
{
    /// <summary>
    /// Анализатор текстов электронных писем и их блоков. Восстановление структуры, разбиение на блоки, 
    /// анализ блока подписи. 
    /// Специфический анализатор, то есть нужно явно создавать процессор через функцию CreateSpecificProcessor, 
    /// указав имя анализатора.
    /// </summary>
    public class MailAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("MAIL")
        /// </summary>
        public const string ANALYZER_NAME = "MAIL";
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
                return "Блок письма";
            }
        }
        public override string Description
        {
            get
            {
                return "Блоки писем (e-mail) и их атрибуты";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new MailAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Mail.Internal.MetaLetter.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Mail.Internal.MetaLetter.ImageId, Pullenti.Ner.Person.Internal.ResourceHelper.GetBytes("mail.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == MailReferent.OBJ_TYPENAME) 
                return new MailReferent();
            return null;
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {"ORGANIZATION", "GEO", "ADDRESS", "PERSON"};
            }
        }
        /// <summary>
        /// Этот анализатор является специфическим (IsSpecific = true)
        /// </summary>
        public override bool IsSpecific
        {
            get
            {
                return true;
            }
        }
        public override int ProgressWeight
        {
            get
            {
                return 1;
            }
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            List<Pullenti.Ner.Mail.Internal.MailLine> lines = new List<Pullenti.Ner.Mail.Internal.MailLine>();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.Mail.Internal.MailLine ml = Pullenti.Ner.Mail.Internal.MailLine.Parse(t, 0, 0);
                if (ml == null) 
                    continue;
                if (lines.Count == 91) 
                {
                }
                lines.Add(ml);
                t = ml.EndToken;
            }
            if (lines.Count == 0) 
                return;
            int i;
            List<List<Pullenti.Ner.Mail.Internal.MailLine>> blocks = new List<List<Pullenti.Ner.Mail.Internal.MailLine>>();
            List<Pullenti.Ner.Mail.Internal.MailLine> blk = null;
            for (i = 0; i < lines.Count; i++) 
            {
                Pullenti.Ner.Mail.Internal.MailLine ml = lines[i];
                if (ml.Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From) 
                {
                    bool isNew = ml.MustBeFirstLine || i == 0;
                    if (((i + 2) < lines.Count) && (((lines[i + 1].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From || lines[i + 2].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From || lines[i + 1].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.Hello) || lines[i + 2].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.Hello))) 
                        isNew = true;
                    if (!isNew) 
                    {
                        for (int j = i - 1; j >= 0; j--) 
                        {
                            if (lines[j].Typ != Pullenti.Ner.Mail.Internal.MailLine.Types.Undefined) 
                            {
                                if (lines[j].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.BestRegards) 
                                    isNew = true;
                                break;
                            }
                        }
                    }
                    if (!isNew) 
                    {
                        for (Pullenti.Ner.Token tt = ml.BeginToken; tt != null && tt.EndChar <= ml.EndChar; tt = tt.Next) 
                        {
                            if (tt.GetReferent() != null) 
                            {
                                if (tt.GetReferent().TypeName == "DATE" || tt.GetReferent().TypeName == "URI") 
                                    isNew = true;
                            }
                        }
                    }
                    if (isNew) 
                    {
                        blk = new List<Pullenti.Ner.Mail.Internal.MailLine>();
                        blocks.Add(blk);
                        for (; i < lines.Count; i++) 
                        {
                            if (lines[i].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From) 
                            {
                                if (blk.Count > 0 && lines[i].MustBeFirstLine) 
                                    break;
                                blk.Add(lines[i]);
                            }
                            else if (((i + 1) < lines.Count) && lines[i + 1].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From) 
                            {
                                int j;
                                for (j = 0; j < blk.Count; j++) 
                                {
                                    if (blk[j].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From) 
                                    {
                                        if (blk[j].IsRealFrom || blk[j].MustBeFirstLine || blk[j].MailAddr != null) 
                                            break;
                                    }
                                }
                                if (j >= blk.Count) 
                                {
                                    blk.Add(lines[i]);
                                    continue;
                                }
                                bool ok = false;
                                for (j = i + 1; j < lines.Count; j++) 
                                {
                                    if (lines[j].Typ != Pullenti.Ner.Mail.Internal.MailLine.Types.From) 
                                        break;
                                    if (lines[j].IsRealFrom || lines[j].MustBeFirstLine) 
                                    {
                                        ok = true;
                                        break;
                                    }
                                    if (lines[j].MailAddr != null) 
                                    {
                                        ok = true;
                                        break;
                                    }
                                }
                                if (ok) 
                                    break;
                                blk.Add(lines[i]);
                            }
                            else 
                                break;
                        }
                        i--;
                        continue;
                    }
                }
                if (blk == null) 
                    blocks.Add((blk = new List<Pullenti.Ner.Mail.Internal.MailLine>()));
                blk.Add(lines[i]);
            }
            if (blocks.Count == 0) 
                return;
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            for (int j = 0; j < blocks.Count; j++) 
            {
                lines = blocks[j];
                if (lines.Count == 0) 
                    continue;
                i = 0;
                if (lines[0].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From) 
                {
                    Pullenti.Ner.Token t1 = lines[0].EndToken;
                    for (; i < lines.Count; i++) 
                    {
                        if (lines[i].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From) 
                            t1 = lines[i].EndToken;
                        else if (((i + 1) < lines.Count) && lines[i + 1].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.From) 
                        {
                        }
                        else 
                            break;
                    }
                    MailReferent mail = new MailReferent() { Kind = MailKind.Head };
                    Pullenti.Ner.ReferentToken mt = new Pullenti.Ner.ReferentToken(mail, lines[0].BeginToken, t1);
                    mail.Text = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(mt, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                    ad.RegisterReferent(mail);
                    mail.AddOccurenceOfRefTok(mt);
                }
                int i0 = i;
                Pullenti.Ner.Token t2 = null;
                int err = 0;
                for (i = lines.Count - 1; i >= i0; i--) 
                {
                    Pullenti.Ner.Mail.Internal.MailLine li = lines[i];
                    if (li.Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.BestRegards) 
                    {
                        t2 = lines[i].BeginToken;
                        for (--i; i >= i0; i--) 
                        {
                            if (lines[i].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.BestRegards && (lines[i].Words < 2)) 
                                t2 = lines[i].BeginToken;
                            else if ((i > i0 && (lines[i].Words < 3) && lines[i - 1].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.BestRegards) && (lines[i - 1].Words < 2)) 
                            {
                                i--;
                                t2 = lines[i].BeginToken;
                            }
                            else 
                                break;
                        }
                        break;
                    }
                    if (li.Refs.Count > 0 && (li.Words < 3) && i > i0) 
                    {
                        err = 0;
                        t2 = li.BeginToken;
                        continue;
                    }
                    if (li.Words > 10) 
                    {
                        t2 = null;
                        continue;
                    }
                    if (li.Words > 2) 
                    {
                        if ((++err) > 2) 
                            t2 = null;
                    }
                }
                if (t2 == null) 
                {
                    for (i = lines.Count - 1; i >= i0; i--) 
                    {
                        Pullenti.Ner.Mail.Internal.MailLine li = lines[i];
                        if (li.Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.Undefined) 
                        {
                            if (li.Refs.Count > 0 && (li.Refs[0] is Pullenti.Ner.Person.PersonReferent)) 
                            {
                                if (li.Words == 0 && i > i0) 
                                {
                                    t2 = li.BeginToken;
                                    break;
                                }
                            }
                        }
                    }
                }
                for (int ii = i0; ii < lines.Count; ii++) 
                {
                    if (lines[ii].Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.Hello) 
                    {
                        MailReferent mail = new MailReferent() { Kind = MailKind.Hello };
                        Pullenti.Ner.ReferentToken mt = new Pullenti.Ner.ReferentToken(mail, lines[i0].BeginToken, lines[ii].EndToken);
                        if (mt.LengthChar > 0) 
                        {
                            mail.Text = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(mt, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                            ad.RegisterReferent(mail);
                            mail.AddOccurenceOfRefTok(mt);
                            i0 = ii + 1;
                        }
                        break;
                    }
                    else if (lines[ii].Typ != Pullenti.Ner.Mail.Internal.MailLine.Types.Undefined || lines[ii].Words > 0 || lines[ii].Refs.Count > 0) 
                        break;
                }
                if (i0 < lines.Count) 
                {
                    if (t2 != null && t2.Previous == null) 
                    {
                    }
                    else 
                    {
                        MailReferent mail = new MailReferent() { Kind = MailKind.Body };
                        Pullenti.Ner.ReferentToken mt = new Pullenti.Ner.ReferentToken(mail, lines[i0].BeginToken, (t2 != null && t2.Previous != null ? t2.Previous : lines[lines.Count - 1].EndToken));
                        if (mt.LengthChar > 0) 
                        {
                            mail.Text = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(mt, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                            ad.RegisterReferent(mail);
                            mail.AddOccurenceOfRefTok(mt);
                        }
                    }
                    if (t2 != null) 
                    {
                        MailReferent mail = new MailReferent() { Kind = MailKind.Tail };
                        Pullenti.Ner.ReferentToken mt = new Pullenti.Ner.ReferentToken(mail, t2, lines[lines.Count - 1].EndToken);
                        if (mt.LengthChar > 0) 
                        {
                            mail.Text = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(mt, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                            ad.RegisterReferent(mail);
                            mail.AddOccurenceOfRefTok(mt);
                        }
                        for (i = i0; i < lines.Count; i++) 
                        {
                            if (lines[i].BeginChar >= t2.BeginChar) 
                            {
                                foreach (Pullenti.Ner.Referent r in lines[i].Refs) 
                                {
                                    mail.AddRef(r, 0);
                                }
                            }
                        }
                    }
                }
            }
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            try 
            {
                Pullenti.Ner.Mail.Internal.MetaLetter.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Mail.Internal.MailLine.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new MailAnalyzer());
        }
    }
}