/******
작성자 : 이우열
작성 일자 : 23.10.08

최근 수정 일자 : 23.10.08
최근 수정 내용 : 연결 정보 클래스 생성
 ******/

namespace Client
{
    [System.Serializable]
    public class ConnectionInfo
    {
        public string Host;
        public int GamePort;
        public int LoginPort;
    }
}