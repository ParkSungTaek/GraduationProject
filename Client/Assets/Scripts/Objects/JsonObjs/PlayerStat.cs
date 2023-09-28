/******
작성자 : 이우열
작성일 : 23.04.05

최근 수정 일자 : 23.04.14
최근 수정 내용 : 불러오는 스텟 종류 추가
 ******/

using System.Collections.Generic;
using System;

namespace Client
{
    [Serializable]
    public class PlayerStatHandler
    {
        public List<PlayerStat> playerstats = new List<PlayerStat>();

        public PlayerStat this[Define.Charcter idx]
        {
            get => playerstats[(int)idx];
        }
    }

    [Serializable]
    public class PlayerStat
    {
        public float AttackRatio;
        public float SkillRatio;

        public float AttackRange;
        public float SkillRange;
               
        public float AttackCool;
        public float SkillCool;

        public float Weight;
        public float Speed;

        public override string ToString()
        {
            return $"Attack : {AttackRatio}, {AttackRange}, {AttackCool} / Skill : {SkillRatio}, {SkillRange}, {SkillCool} / W : {Weight} / S : {Speed}";
        }
    }
}