/******
작성자 : 이우열
작성 일자 : 23.04.29

최근 수정 일자 : 23.04.29
최근 수정 내용 : 클래스 생성
 ******/

namespace Server
{
    public struct PlayerInfo
    {
        public Define.PlayerClass PlayerClass;

        public PlayerInfo()
        {
            PlayerClass = Define.PlayerClass.UnSelected;
        }

        public PlayerInfo(Define.PlayerClass playerClass)
        {
            PlayerClass = playerClass;
        }
    }
}