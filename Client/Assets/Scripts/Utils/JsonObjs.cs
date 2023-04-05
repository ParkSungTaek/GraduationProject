using System;
using System.Collections.Generic;

namespace Client
{
    [Serializable]
    public class CharacterstatHandler
    {
        public List<Characterstat> characterstats = new List<Characterstat>();

        public Characterstat this[Define.Charcter idx]
        {
            get => characterstats[(int)idx];
        }
    }

    [Serializable]
    public class Characterstat
    {
        public int AttackRange;
        public int SkillRange;
        public int AttackCool;
        public int SkillCool;
    }

}
