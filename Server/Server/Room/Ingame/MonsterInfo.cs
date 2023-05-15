/******
작성자 : 박성택
작성 일자 : 23.04.29

최근 수정 일자 : 23.04.29
최근 수정 내용 : 클래스 생성
 ******/

namespace Server
{
    public struct MonsterInfo
    {
        
        public int MonsterId;

        public int CurrHp;

        public MonsterInfo(int monsterId, int currHp)
        {
            MonsterId = monsterId;
            CurrHp = currHp;
        }


        //우선 없이 한번 가본다.
        //public float PosX;
        //public float PosY;
    }
}