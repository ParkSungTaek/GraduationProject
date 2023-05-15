/*
작성자 : 박성택
작성일 : 23.04.05

최근 수정 일자 : 23.04.05
최근 수정 사항 : json 파싱용 몬스터 스텟 클래스 생성
*/

using System;
using System.Collections.Generic;

namespace Server
{

    [Serializable]
    public class MonsterStatHandler 
    {
        public List<MonsterStat> monsterstats = new List<MonsterStat>();

    }

    [Serializable]
    public class MonsterStat
    {
        public string Name;
        public int MaxHP;
        public int AttackDMG;
        public float MoveSpeed;
        public float AttackSpeed;
        public float _offsetCorrection;
        public float _monsterHpBarOffset;
    }

}
