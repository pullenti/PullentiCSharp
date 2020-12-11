/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Instrument.Internal
{
    internal class MetaInstrumentBlock : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaInstrumentBlock();
            GlobalMeta.KindFeature = GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_KIND, "Класс", 0, 1);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Undefined.ToString(), "Неизвестный фрагмент", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Document.ToString(), "Документ", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.InternalDocument.ToString(), "Внутренний документ", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Appendix.ToString(), "Приложение", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Content.ToString(), "Содержимое", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Head.ToString(), "Заголовочная часть", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Tail.ToString(), "Хвостовая часть", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Name.ToString(), "Название", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Number.ToString(), "Номер", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.CaseNumber.ToString(), "Номер дела", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.CaseInfo.ToString(), "Информация дела", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Editions.ToString(), "Редакции", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Approved.ToString(), "Одобрен", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Organization.ToString(), "Организация", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.DocPart.ToString(), "Часть документа", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Place.ToString(), "Место", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Signer.ToString(), "Подписант", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Subitem.ToString(), "Подпункт", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Indention.ToString(), "Абзац", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Chapter.ToString(), "Глава", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Paragraph.ToString(), "Параграф", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Subparagraph.ToString(), "Подпараграф", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.ListHead.ToString(), "Заголовок списка", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.ListItem.ToString(), "Элемент списка", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Notice.ToString(), "Примечание", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Typ.ToString(), "Тип", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Section.ToString(), "Раздел", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Subsection.ToString(), "Подраздел", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Clause.ToString(), "Статья", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.ClausePart.ToString(), "Часть", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Date.ToString(), "Дата", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Directive.ToString(), "Директива", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Index.ToString(), "Оглавление", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.IndexItem.ToString(), "Элемент оглавления", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.DocReference.ToString(), "Ссылка на документ", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Initiator.ToString(), "Инициатор", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Preamble.ToString(), "Преамбула", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Item.ToString(), "Пункт", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Keyword.ToString(), "Ключевое слово", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Comment.ToString(), "Комментарий", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Question.ToString(), "Вопрос", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Citation.ToString(), "Цитата", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Contact.ToString(), "Контакт", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.Table.ToString(), "Таблица", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.TableRow.ToString(), "Строка таблицы", null, null);
            GlobalMeta.KindFeature.AddValue(Pullenti.Ner.Instrument.InstrumentKind.TableCell.ToString(), "Ячейка таблицы", null, null);
            Pullenti.Ner.Metadata.Feature fi2 = GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_KIND, "Класс (доп.)", 0, 1);
            for (int i = 0; i < GlobalMeta.KindFeature.InnerValues.Count; i++) 
            {
                fi2.AddValue(GlobalMeta.KindFeature.InnerValues[i], GlobalMeta.KindFeature.OuterValues[i], null, null);
            }
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_CHILD, "Внутренний элемент", 0, 0).ShowAsParent = true;
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NAME, "Наименование", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NUMBER, "Номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_MINNUMBER, "Минимальный номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_SUBNUMBER, "Подномер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_SUB2NUMBER, "Подномер второй", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_SUB3NUMBER, "Подномер третий", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_VALUE, "Значение", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_REF, "Ссылка на объект", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_EXPIRED, "Утратил силу", 0, 1);
        }
        public Pullenti.Ner.Metadata.Feature KindFeature;
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Instrument.InstrumentBlockReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Блок документа";
            }
        }
        public static string DocImageId = "decree";
        public static string PartImageId = "part";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return PartImageId;
        }
        public static MetaInstrumentBlock GlobalMeta;
    }
}