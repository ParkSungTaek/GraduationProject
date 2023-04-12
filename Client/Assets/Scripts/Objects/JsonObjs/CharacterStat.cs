/******
작성자 : 이우열
작성일 : 23.04.05

최근 수정 일자 : 23.04.05
최근 수정 내용 : json 파싱용 캐릭터 스텟 클래스 생성
 ******/

using System.Collections.Generic;
using System;

namespace Client
{
    [Serializable]
    public class CharacterStatHandler
    {
        public List<CharacterStat> characterstats = new List<CharacterStat>();

        public CharacterStat this[Define.Charcter idx]
        {
            get => characterstats[(int)idx];
        }
    }

    [Serializable]
    public class CharacterStat
    {
        public int AttackRange;
        public int SkillRange;
        public int AttackCool;
        public int SkillCool;
    }
}