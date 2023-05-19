/******
작성자 : 박성택
작성 일자 : 23.04.29

최근 수정 일자 : 23.05.18
최근 수정 내용 : AttackCnt도입
 ******/

namespace Server
{
    public struct MonsterInfo
    {
        
        public ushort MonsterId;

        public int CurrHp;

        public ushort AttackCnt;


        public MonsterInfo(ushort monsterId, int currHp)
        {
            MonsterId = monsterId;
            CurrHp = currHp;
            AttackCnt = 0;
        }


        //우선 없이 한번 가본다.
        //public float PosX;
        //public float PosY;
    }
}