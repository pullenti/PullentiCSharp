/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Pullenti.Ner
{
    /// <summary>
    /// Базовый класс для всех лингвистических анализаторов. Игнорируйте, если не собираетесь делать свой анализатор.
    /// </summary>
    public abstract class Analyzer
    {
        /// <summary>
        /// Запустить анализ
        /// </summary>
        /// <param name="kit">контейнер с данными</param>
        public virtual void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
        }
        /// <summary>
        /// Уникальное наименование анализатора
        /// </summary>
        public virtual string Name
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// Заголовок анализатора
        /// </summary>
        public virtual string Caption
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// Описание анализатора
        /// </summary>
        public virtual string Description
        {
            get
            {
                return null;
            }
        }
        public override string ToString()
        {
            return string.Format("{0} ({1})", Caption, Name);
        }
        public virtual Analyzer Clone()
        {
            return null;
        }
        /// <summary>
        /// Список поддерживаемых типов объектов (сущностей), которые выделяет анализатор
        /// </summary>
        public virtual ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new List<Pullenti.Ner.Metadata.ReferentClass>();
            }
        }
        /// <summary>
        /// Список изображений объектов
        /// </summary>
        public virtual Dictionary<string, byte[]> Images
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// Признак специфического анализатора (предназначенного для конкретной предметной области). 
        /// Специфические анализаторы по умолчанию не добавляются в процессор (Processor)
        /// </summary>
        public virtual bool IsSpecific
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Создать сущность указанного типа
        /// </summary>
        /// <param name="type">тип сущности</param>
        /// <return>экземпляр</return>
        public virtual Referent CreateReferent(string type)
        {
            return null;
        }
        static List<string> emptyList = new List<string>();
        /// <summary>
        /// Список имён типов объектов из других картриджей, которые желательно предварительно выделить (для управления приоритетом применения правил)
        /// </summary>
        public virtual IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return emptyList;
            }
        }
        /// <summary>
        /// Сколько примерно времени работает анализатор по сравнению с другими (в условных единицах)
        /// </summary>
        public virtual int ProgressWeight
        {
            get
            {
                return 0;
            }
        }
        internal event ProgressChangedEventHandler Progress;
        internal event CancelEventHandler Cancel;
        protected bool OnProgress(int pos, int max, Pullenti.Ner.Core.AnalysisKit kit)
        {
            bool ret = true;
            if (Progress != null) 
            {
                if (pos >= 0 && pos <= max && max > 0) 
                {
                    int percent = pos;
                    if (max > 1000000) 
                        percent /= ((max / 1000));
                    else 
                        percent = ((100 * percent)) / max;
                    if (percent != lastPercent) 
                    {
                        ProgressChangedEventArgs arg = new ProgressChangedEventArgs(percent, null);
                        Progress(this, arg) /* error */;
                        if (Cancel != null) 
                        {
                            CancelEventArgs cea = new CancelEventArgs();
                            Cancel(kit, cea) /* error */;
                            ret = !cea.Cancel;
                        }
                    }
                    lastPercent = percent;
                }
            }
            return ret;
        }
        int lastPercent;
        protected bool OnMessage(object message)
        {
            if (Progress != null) 
                Progress(this, new ProgressChangedEventArgs(-1, message)) /* error */;
            return true;
        }
        /// <summary>
        /// Включить режим накопления выделяемых сущностей при обработке разных SourceOfText 
        /// (то есть локальные сущности будут накапливаться)
        /// </summary>
        internal bool PersistReferentsRegim
        {
            get;
            set;
        }
        /// <summary>
        /// При установке в true будет игнорироваться при обработке (для отладки)
        /// </summary>
        public bool IgnoreThisAnalyzer
        {
            get;
            set;
        }
        internal Pullenti.Ner.Core.AnalyzerData PersistAnalizerData;
        /// <summary>
        /// Используется внутренним образом
        /// </summary>
        public virtual Pullenti.Ner.Core.AnalyzerData CreateAnalyzerData()
        {
            return new Pullenti.Ner.Core.AnalyzerData();
        }
        /// <summary>
        /// Попытаться выделить сущность в указанном диапазоне (используется внутренним образом). 
        /// Кстати, выделенная сущность не сохраняется в локальной онтологии.
        /// </summary>
        /// <param name="begin">начало диапазона</param>
        /// <param name="end">конец диапазона (если null, то до конца)</param>
        /// <return>результат</return>
        public virtual ReferentToken ProcessReferent(Token begin, Token end)
        {
            return null;
        }
        /// <summary>
        /// Это используется внутренним образом для обработки внешних онтологий
        /// </summary>
        public virtual ReferentToken ProcessOntologyItem(Token begin)
        {
            return null;
        }
    }
}