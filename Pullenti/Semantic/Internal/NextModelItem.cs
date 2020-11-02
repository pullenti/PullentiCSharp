/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic.Internal
{
    public class NextModelItem
    {
        public string Preposition;
        public Pullenti.Morph.MorphCase Case;
        public string Spelling;
        public Pullenti.Semantic.Utils.QuestionType Question;
        public int Id;
        public NextModelItem(string prep, Pullenti.Morph.MorphCase cas, string spel = null, Pullenti.Semantic.Utils.QuestionType typ = Pullenti.Semantic.Utils.QuestionType.Undefined)
        {
            Preposition = prep;
            Case = cas;
            Spelling = spel;
            Question = typ;
            if (spel != null) 
                return;
            if (!string.IsNullOrEmpty(prep)) 
            {
                if (cas.IsGenitive) 
                    Spelling = string.Format("{0} чего", prep.ToLower());
                else if (cas.IsDative) 
                    Spelling = string.Format("{0} чему", prep.ToLower());
                else if (cas.IsAccusative) 
                    Spelling = string.Format("{0} что", prep.ToLower());
                else if (cas.IsInstrumental) 
                    Spelling = string.Format("{0} чем", prep.ToLower());
                else if (cas.IsPrepositional) 
                    Spelling = string.Format("{0} чём", prep.ToLower());
            }
            else 
            {
                Preposition = "";
                if (cas.IsNominative) 
                    Spelling = "кто";
                else if (cas.IsGenitive) 
                    Spelling = "чего";
                else if (cas.IsDative) 
                    Spelling = "чему";
                else if (cas.IsAccusative) 
                    Spelling = "что";
                else if (cas.IsInstrumental) 
                    Spelling = "чем";
                else if (cas.IsPrepositional) 
                    Spelling = "чём";
            }
        }
        public override string ToString()
        {
            return Spelling;
        }
        public int CompareTo(NextModelItem other)
        {
            int i = string.Compare(Preposition, other.Preposition);
            if (i != 0) 
                return i;
            if (this._casRank() < other._casRank()) 
                return -1;
            if (this._casRank() > other._casRank()) 
                return 1;
            return 0;
        }
        int _casRank()
        {
            if (Case.IsGenitive) 
                return 1;
            if (Case.IsDative) 
                return 2;
            if (Case.IsAccusative) 
                return 3;
            if (Case.IsInstrumental) 
                return 4;
            if (Case.IsPrepositional) 
                return 5;
            return 0;
        }
        public bool Check(string prep, Pullenti.Morph.MorphCase cas)
        {
            if (((cas & Case)).IsUndefined) 
                return false;
            if (prep != null && Preposition != null) 
                return prep == Preposition;
            return string.IsNullOrEmpty(prep) && string.IsNullOrEmpty(Preposition);
        }
    }
}