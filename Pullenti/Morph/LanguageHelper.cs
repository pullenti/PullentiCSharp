/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Morph
{
    // Служба подержки языков.
    // В качестве универсальных идентификаторов языков выступает 2-х символьный идентификатор ISO 639-1.
    // Также содержит некоторые полезные функции.
    public static class LanguageHelper
    {
        /// <summary>
        /// Определить язык для неструктурированного ткста
        /// </summary>
        /// <param name="text">текст</param>
        /// <return>код языка или null при ненахождении</return>
        public static string GetLanguageForText(string text)
        {
            if (string.IsNullOrEmpty(text)) 
                return null;
            int i;
            int j;
            int ruChars = 0;
            int enChars = 0;
            for (i = 0; i < text.Length; i++) 
            {
                char ch = text[i];
                if (!char.IsLetter(ch)) 
                    continue;
                j = (int)ch;
                if (j >= 0x400 && (j < 0x500)) 
                    ruChars++;
                else if (j < 0x80) 
                    enChars++;
            }
            if ((ruChars > (enChars * 2)) && ruChars > 10) 
                return "ru";
            if (ruChars > 0 && enChars == 0) 
                return "ru";
            if (enChars > 0) 
                return "en";
            return null;
        }
        /// <summary>
        /// Определение языка для одного слова
        /// </summary>
        /// <param name="word">слово (в верхнем регистре)</param>
        internal static MorphLang GetWordLang(string word)
        {
            int cyr = 0;
            int lat = 0;
            int undef = 0;
            foreach (char ch in word) 
            {
                Pullenti.Morph.Internal.UnicodeInfo ui = Pullenti.Morph.Internal.UnicodeInfo.AllChars[(int)ch];
                if (ui.IsLetter) 
                {
                    if (ui.IsCyrillic) 
                        cyr++;
                    else if (ui.IsLatin) 
                        lat++;
                    else 
                        undef++;
                }
            }
            if (undef > 0) 
                return MorphLang.Unknown;
            if (cyr == 0 && lat == 0) 
                return MorphLang.Unknown;
            if (cyr == 0) 
                return MorphLang.EN;
            if (lat > 0) 
                return MorphLang.Unknown;
            MorphLang lang = (MorphLang.UA | MorphLang.RU | MorphLang.BY) | MorphLang.KZ;
            foreach (char ch in word) 
            {
                Pullenti.Morph.Internal.UnicodeInfo ui = Pullenti.Morph.Internal.UnicodeInfo.AllChars[(int)ch];
                if (ui.IsLetter) 
                {
                    if (ch == 'Ґ' || ch == 'Є' || ch == 'Ї') 
                    {
                        lang.IsRu = false;
                        lang.IsBy = false;
                    }
                    else if (ch == 'І') 
                        lang.IsRu = false;
                    else if (ch == 'Ё' || ch == 'Э') 
                    {
                        lang.IsUa = false;
                        lang.IsKz = false;
                    }
                    else if (ch == 'Ы') 
                        lang.IsUa = false;
                    else if (ch == 'Ў') 
                    {
                        lang.IsRu = false;
                        lang.IsUa = false;
                    }
                    else if (ch == 'Щ') 
                        lang.IsBy = false;
                    else if (ch == 'Ъ') 
                    {
                        lang.IsBy = false;
                        lang.IsUa = false;
                        lang.IsKz = false;
                    }
                    else if ((((ch == 'Ә' || ch == 'Ғ' || ch == 'Қ') || ch == 'Ң' || ch == 'Ө') || ((ch == 'Ұ' && word.Length > 1)) || ch == 'Ү') || ch == 'Һ') 
                    {
                        lang.IsBy = false;
                        lang.IsUa = false;
                        lang.IsRu = false;
                    }
                    else if ((ch == 'В' || ch == 'Ф' || ch == 'Ц') || ch == 'Ч' || ch == 'Ь') 
                        lang.IsKz = false;
                }
            }
            return lang;
        }
        public static bool IsLatinChar(char ch)
        {
            Pullenti.Morph.Internal.UnicodeInfo ui = Pullenti.Morph.Internal.UnicodeInfo.AllChars[(int)ch];
            return ui.IsLatin;
        }
        public static bool IsLatin(string str)
        {
            if (str == null) 
                return false;
            for (int i = 0; i < str.Length; i++) 
            {
                if (!IsLatinChar(str[i])) 
                {
                    if (!char.IsWhiteSpace(str[i]) && str[i] != '-') 
                        return false;
                }
            }
            return true;
        }
        public static bool IsCyrillicChar(char ch)
        {
            Pullenti.Morph.Internal.UnicodeInfo ui = Pullenti.Morph.Internal.UnicodeInfo.AllChars[(int)ch];
            return ui.IsCyrillic;
        }
        public static bool IsCyrillic(string str)
        {
            if (str == null) 
                return false;
            for (int i = 0; i < str.Length; i++) 
            {
                if (!IsCyrillicChar(str[i])) 
                {
                    if (!char.IsWhiteSpace(str[i]) && str[i] != '-') 
                        return false;
                }
            }
            return true;
        }
        public static bool IsHiphen(char ch)
        {
            Pullenti.Morph.Internal.UnicodeInfo ui = Pullenti.Morph.Internal.UnicodeInfo.AllChars[(int)ch];
            return ui.IsHiphen;
        }
        /// <summary>
        /// Проверка, что это гласная на кириллице
        /// </summary>
        public static bool IsCyrillicVowel(char ch)
        {
            Pullenti.Morph.Internal.UnicodeInfo ui = Pullenti.Morph.Internal.UnicodeInfo.AllChars[(int)ch];
            return ui.IsCyrillic && ui.IsVowel;
        }
        /// <summary>
        /// Проверка, что это гласная на латинице
        /// </summary>
        public static bool IsLatinVowel(char ch)
        {
            Pullenti.Morph.Internal.UnicodeInfo ui = Pullenti.Morph.Internal.UnicodeInfo.AllChars[(int)ch];
            return ui.IsLatin && ui.IsVowel;
        }
        /// <summary>
        /// Получить для латинской буквы её возможный графический эквивалент на кириллице 
        /// (для тексто-графических замен)
        /// </summary>
        /// <return>0 - нет эквивалента</return>
        public static char GetCyrForLat(char lat)
        {
            int i = m_LatChars.IndexOf(lat);
            if (i >= 0 && (i < m_CyrChars.Length)) 
                return m_CyrChars[i];
            i = m_GreekChars.IndexOf(lat);
            if (i >= 0 && (i < m_CyrGreekChars.Length)) 
                return m_CyrGreekChars[i];
            return (char)0;
        }
        /// <summary>
        /// Получить для кириллической буквы её возможный графический эквивалент на латинице 
        /// (для тексто-графических замен)
        /// </summary>
        /// <return>0 - нет эквивалента</return>
        public static char GetLatForCyr(char cyr)
        {
            int i = m_CyrChars.IndexOf(cyr);
            if ((i < 0) || i >= m_LatChars.Length) 
                return (char)0;
            else 
                return m_LatChars[i];
        }
        /// <summary>
        /// Транслитеральная корректировка
        /// </summary>
        public static string TransliteralCorrection(string value, string prevValue, bool always = false)
        {
            int pureCyr = 0;
            int pureLat = 0;
            int quesCyr = 0;
            int quesLat = 0;
            int udarCyr = 0;
            bool y = false;
            bool udaren = false;
            for (int i = 0; i < value.Length; i++) 
            {
                char ch = value[i];
                Pullenti.Morph.Internal.UnicodeInfo ui = Pullenti.Morph.Internal.UnicodeInfo.AllChars[(int)ch];
                if (!ui.IsLetter) 
                {
                    if (ui.IsUdaren) 
                    {
                        udaren = true;
                        continue;
                    }
                    if (ui.IsApos && value.Length > 2) 
                        return TransliteralCorrection(value.Replace(string.Format("{0}", ch), ""), prevValue, false);
                    return value;
                }
                if (ui.IsCyrillic) 
                {
                    if (m_CyrChars.IndexOf(ch) >= 0) 
                        quesCyr++;
                    else 
                        pureCyr++;
                }
                else if (ui.IsLatin) 
                {
                    if (m_LatChars.IndexOf(ch) >= 0) 
                        quesLat++;
                    else 
                        pureLat++;
                }
                else if (m_UdarChars.IndexOf(ch) >= 0) 
                    udarCyr++;
                else 
                    return value;
                if (ch == 'Ь' && ((i + 1) < value.Length) && value[i + 1] == 'I') 
                    y = true;
            }
            bool toRus = false;
            bool toLat = false;
            if (pureLat > 0 && pureCyr > 0) 
                return value;
            if (((pureLat > 0 || always)) && quesCyr > 0) 
                toLat = true;
            else if (((pureCyr > 0 || always)) && quesLat > 0) 
                toRus = true;
            else if (pureCyr == 0 && pureLat == 0) 
            {
                if (quesCyr > 0 && quesLat > 0) 
                {
                    if (!string.IsNullOrEmpty(prevValue)) 
                    {
                        if (IsCyrillicChar(prevValue[0])) 
                            toRus = true;
                        else if (IsLatinChar(prevValue[0])) 
                            toLat = true;
                    }
                    if (!toLat && !toRus) 
                    {
                        if (quesCyr > quesLat) 
                            toRus = true;
                        else if (quesCyr < quesLat) 
                            toLat = true;
                    }
                }
            }
            if (!toRus && !toLat) 
            {
                if (!y && !udaren && udarCyr == 0) 
                    return value;
            }
            StringBuilder tmp = new StringBuilder(value);
            for (int i = 0; i < tmp.Length; i++) 
            {
                if (tmp[i] == 'Ь' && ((i + 1) < tmp.Length) && tmp[i + 1] == 'I') 
                {
                    tmp[i] = 'Ы';
                    tmp.Remove(i + 1, 1);
                    continue;
                }
                int cod = (int)tmp[i];
                if (cod >= 0x300 && (cod < 0x370)) 
                {
                    tmp.Remove(i, 1);
                    continue;
                }
                if (toRus) 
                {
                    int ii = m_LatChars.IndexOf(tmp[i]);
                    if (ii >= 0) 
                        tmp[i] = m_CyrChars[ii];
                    else if ((((ii = m_UdarChars.IndexOf(tmp[i])))) >= 0) 
                        tmp[i] = m_UdarCyrChars[ii];
                }
                else if (toLat) 
                {
                    int ii = m_CyrChars.IndexOf(tmp[i]);
                    if (ii >= 0) 
                        tmp[i] = m_LatChars[ii];
                }
                else 
                {
                    int ii = m_UdarChars.IndexOf(tmp[i]);
                    if (ii >= 0) 
                        tmp[i] = m_UdarCyrChars[ii];
                }
            }
            return tmp.ToString();
        }
        internal static string m_LatChars = "ABEKMHOPCTYXIaekmopctyxi";
        internal static string m_CyrChars = "АВЕКМНОРСТУХІаекморстухі";
        internal static string m_GreekChars = "ΑΒΓΕΗΙΚΛΜΟΠΡΤΥΦΧ";
        internal static string m_CyrGreekChars = "АВГЕНІКЛМОПРТУФХ";
        static string m_UdarChars = "ÀÁÈÉËÒÓàáèéëýÝòóЀѐЍѝỲỳ";
        static string m_UdarCyrChars = "ААЕЕЕООааеееуУооЕеИиУу";
        public static bool IsQuote(char ch)
        {
            Pullenti.Morph.Internal.UnicodeInfo ui = Pullenti.Morph.Internal.UnicodeInfo.AllChars[(int)ch];
            return ui.IsQuot;
        }
        public static bool IsApos(char ch)
        {
            Pullenti.Morph.Internal.UnicodeInfo ui = Pullenti.Morph.Internal.UnicodeInfo.AllChars[(int)ch];
            return ui.IsApos;
        }
        static string[] m_Preps = new string[] {("БЕЗ;ДО;ИЗ;ИЗЗА;ОТ;У;ДЛЯ;РАДИ;ВОЗЛЕ;ПОЗАДИ;ВПЕРЕДИ;БЛИЗ;ВБЛИЗИ;ВГЛУБЬ;ВВИДУ;ВДОЛЬ;ВЗАМЕН;ВКРУГ;ВМЕСТО;" + "ВНЕ;ВНИЗУ;ВНУТРИ;ВНУТРЬ;ВОКРУГ;ВРОДЕ;ВСЛЕД;ВСЛЕДСТВИЕ;ЗАМЕСТО;ИЗНУТРИ;КАСАТЕЛЬНО;КРОМЕ;" + "МИМО;НАВРОДЕ;НАЗАД;НАКАНУНЕ;НАПОДОБИЕ;НАПРОТИВ;НАСЧЕТ;ОКОЛО;ОТНОСИТЕЛЬНО;") + "ПОВЕРХ;ПОДЛЕ;ПОМИМО;ПОПЕРЕК;ПОРЯДКА;ПОСЕРЕДИНЕ;ПОСРЕДИ;ПОСЛЕ;ПРЕВЫШЕ;ПРЕЖДЕ;ПРОТИВ;СВЕРХ;" + "СВЫШЕ;СНАРУЖИ;СРЕДИ;СУПРОТИВ;ПУТЕМ;ПОСРЕДСТВОМ", "К;БЛАГОДАРЯ;ВОПРЕКИ;НАВСТРЕЧУ;СОГЛАСНО;СООБРАЗНО;ПАРАЛЛЕЛЬНО;ПОДОБНО;СООТВЕТСТВЕННО;СОРАЗМЕРНО", "ПРО;ЧЕРЕЗ;СКВОЗЬ;СПУСТЯ", "НАД;ПЕРЕД;ПРЕД", "ПРИ", "В;НА;О;ВКЛЮЧАЯ", "МЕЖДУ", "ЗА;ПОД", "ПО", "С"};
        static MorphCase[] m_Cases = new MorphCase[] {MorphCase.Genitive, MorphCase.Dative, MorphCase.Accusative, MorphCase.Instrumental, MorphCase.Prepositional, MorphCase.Accusative | MorphCase.Prepositional, MorphCase.Genitive | MorphCase.Instrumental, MorphCase.Accusative | MorphCase.Instrumental, MorphCase.Dative | MorphCase.Accusative | MorphCase.Prepositional, MorphCase.Genitive | MorphCase.Accusative | MorphCase.Instrumental};
        static Dictionary<string, MorphCase> m_PrepCases;
        /// <summary>
        /// Получить возможные падежи существительных после предлогов
        /// </summary>
        /// <param name="prep">предлог</param>
        public static MorphCase GetCaseAfterPreposition(string prep)
        {
            MorphCase mc;
            if (m_PrepCases.TryGetValue(prep, out mc)) 
                return mc;
            else 
                return MorphCase.Undefined;
        }
        static string[] m_PrepNormsSrc = new string[] {"БЕЗ;БЕЗО", "ВБЛИЗИ;БЛИЗ", "В;ВО", "ВОКРУГ;ВКРУГ", "ВНУТРИ;ВНУТРЬ;ВОВНУТРЬ", "ВПЕРЕДИ;ВПЕРЕД", "ВСЛЕД;ВОСЛЕД", "ВМЕСТО;ЗАМЕСТО", "ИЗ;ИЗО", "К;КО", "МЕЖДУ;МЕЖ;ПРОМЕЖДУ;ПРОМЕЖ", "НАД;НАДО", "О;ОБ;ОБО", "ОТ;ОТО", "ПЕРЕД;ПРЕД;ПРЕДО;ПЕРЕДО", "ПОД;ПОДО", "ПОСЕРЕДИНЕ;ПОСРЕДИ;ПОСЕРЕДЬ", "С;СО", "СРЕДИ;СРЕДЬ;СЕРЕДЬ", "ЧЕРЕЗ;ЧРЕЗ"};
        static Dictionary<string, string> m_PrepNorms;
        public static string NormalizePreposition(string prep)
        {
            string res;
            if (m_PrepNorms.TryGetValue(prep, out res)) 
                return res;
            else 
                return prep;
        }
        /// <summary>
        /// Замена стандартной функции string.EndsWith, которая относительно медленная
        /// </summary>
        public static bool EndsWith(string str, string substr)
        {
            if (str == null || substr == null) 
                return false;
            int i = str.Length - 1;
            int j = substr.Length - 1;
            if (j > i || (j < 0)) 
                return false;
            for (; j >= 0; j--,i--) 
            {
                if (str[i] != substr[j]) 
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Проверка окончания строки на одну из заданных подстрок
        /// </summary>
        public static bool EndsWithEx(string str, string substr, string substr2, string substr3 = null, string substr4 = null)
        {
            if (str == null) 
                return false;
            for (int k = 0; k < 4; k++) 
            {
                string s = substr;
                if (k == 1) 
                    s = substr2;
                else if (k == 2) 
                    s = substr3;
                else if (k == 3) 
                    s = substr4;
                if (s == null) 
                    continue;
                int i = str.Length - 1;
                int j = s.Length - 1;
                if (j > i || (j < 0)) 
                    continue;
                for (; j >= 0; j--,i--) 
                {
                    if (str[i] != s[j]) 
                        break;
                }
                if (j < 0) 
                    return true;
            }
            return false;
        }
        public static string ToStringMorphTense(MorphTense tense)
        {
            StringBuilder res = new StringBuilder();
            if (((tense & MorphTense.Past)) != MorphTense.Undefined) 
                res.Append("прошедшее|");
            if (((tense & MorphTense.Present)) != MorphTense.Undefined) 
                res.Append("настоящее|");
            if (((tense & MorphTense.Future)) != MorphTense.Undefined) 
                res.Append("будущее|");
            if (res.Length > 0) 
                res.Length--;
            return res.ToString();
        }
        public static string ToStringMorphPerson(MorphPerson person)
        {
            StringBuilder res = new StringBuilder();
            if (((person & MorphPerson.First)) != MorphPerson.Undefined) 
                res.Append("1лицо|");
            if (((person & MorphPerson.Second)) != MorphPerson.Undefined) 
                res.Append("2лицо|");
            if (((person & MorphPerson.Third)) != MorphPerson.Undefined) 
                res.Append("3лицо|");
            if (res.Length > 0) 
                res.Length--;
            return res.ToString();
        }
        public static string ToStringMorphGender(MorphGender gender)
        {
            StringBuilder res = new StringBuilder();
            if (((gender & MorphGender.Masculine)) != MorphGender.Undefined) 
                res.Append("муж.|");
            if (((gender & MorphGender.Feminie)) != MorphGender.Undefined) 
                res.Append("жен.|");
            if (((gender & MorphGender.Neuter)) != MorphGender.Undefined) 
                res.Append("средн.|");
            if (res.Length > 0) 
                res.Length--;
            return res.ToString();
        }
        public static string ToStringMorphNumber(MorphNumber number)
        {
            StringBuilder res = new StringBuilder();
            if (((number & MorphNumber.Singular)) != MorphNumber.Undefined) 
                res.Append("единств.|");
            if (((number & MorphNumber.Plural)) != MorphNumber.Undefined) 
                res.Append("множеств.|");
            if (res.Length > 0) 
                res.Length--;
            return res.ToString();
        }
        public static string ToStringMorphVoice(MorphVoice voice)
        {
            StringBuilder res = new StringBuilder();
            if (((voice & MorphVoice.Active)) != MorphVoice.Undefined) 
                res.Append("действит.|");
            if (((voice & MorphVoice.Passive)) != MorphVoice.Undefined) 
                res.Append("страдат.|");
            if (((voice & MorphVoice.Middle)) != MorphVoice.Undefined) 
                res.Append("средн.|");
            if (res.Length > 0) 
                res.Length--;
            return res.ToString();
        }
        public static string ToStringMorphMood(MorphMood mood)
        {
            StringBuilder res = new StringBuilder();
            if (((mood & MorphMood.Indicative)) != MorphMood.Undefined) 
                res.Append("изъявит.|");
            if (((mood & MorphMood.Imperative)) != MorphMood.Undefined) 
                res.Append("повелит.|");
            if (((mood & MorphMood.Subjunctive)) != MorphMood.Undefined) 
                res.Append("условн.|");
            if (res.Length > 0) 
                res.Length--;
            return res.ToString();
        }
        public static string ToStringMorphAspect(MorphAspect aspect)
        {
            StringBuilder res = new StringBuilder();
            if (((aspect & MorphAspect.Imperfective)) != MorphAspect.Undefined) 
                res.Append("несоверш.|");
            if (((aspect & MorphAspect.Perfective)) != MorphAspect.Undefined) 
                res.Append("соверш.|");
            if (res.Length > 0) 
                res.Length--;
            return res.ToString();
        }
        public static string ToStringMorphFinite(MorphFinite finit)
        {
            StringBuilder res = new StringBuilder();
            if (((finit & MorphFinite.Finite)) != MorphFinite.Undefined) 
                res.Append("finite|");
            if (((finit & MorphFinite.Gerund)) != MorphFinite.Undefined) 
                res.Append("gerund|");
            if (((finit & MorphFinite.Infinitive)) != MorphFinite.Undefined) 
                res.Append("инфинитив|");
            if (((finit & MorphFinite.Participle)) != MorphFinite.Undefined) 
                res.Append("participle|");
            if (res.Length > 0) 
                res.Length--;
            return res.ToString();
        }
        public static string ToStringMorphForm(MorphForm form)
        {
            StringBuilder res = new StringBuilder();
            if (((form & MorphForm.Short)) != MorphForm.Undefined) 
                res.Append("кратк.|");
            if (((form & MorphForm.Synonym)) != MorphForm.Undefined) 
                res.Append("синонимич.|");
            if (res.Length > 0) 
                res.Length--;
            return res.ToString();
        }
        static string m_Rus0 = "–ЁѐЀЍѝЎўӢӣ";
        static string m_Rus1 = "-ЕЕЕИИУУЙЙ";
        /// <summary>
        /// Откорректировать слово (перевод в верхний регистр и замена некоторых букв типа Ё->Е)
        /// </summary>
        /// <param name="w">исходное слово</param>
        /// <return>откорректированное слово</return>
        public static string CorrectWord(string w)
        {
            if (w == null) 
                return null;
            string res = w.ToUpper();
            foreach (char ch in res) 
            {
                if (m_Rus0.IndexOf(ch) >= 0) 
                {
                    StringBuilder tmp = new StringBuilder();
                    tmp.Append(res);
                    for (int i = 0; i < tmp.Length; i++) 
                    {
                        int j = m_Rus0.IndexOf(tmp[i]);
                        if (j >= 0) 
                            tmp[i] = m_Rus1[j];
                    }
                    res = tmp.ToString();
                    break;
                }
            }
            if (res.IndexOf((char)0x00AD) >= 0) 
                res = res.Replace((char)0x00AD, '-');
            if (res.StartsWith("АГЕНС")) 
                res = "АГЕНТС" + res.Substring(5);
            return res;
        }
        static LanguageHelper()
        {
            m_PrepCases = new Dictionary<string, MorphCase>();
            for (int i = 0; i < m_Preps.Length; i++) 
            {
                foreach (string v in m_Preps[i].Split(';')) 
                {
                    m_PrepCases.Add(v, m_Cases[i]);
                }
            }
            m_PrepNorms = new Dictionary<string, string>();
            foreach (string s in m_PrepNormsSrc) 
            {
                string[] vars = s.Split(';');
                for (int i = 1; i < vars.Length; i++) 
                {
                    m_PrepNorms.Add(vars[i], vars[0]);
                }
            }
        }
    }
}