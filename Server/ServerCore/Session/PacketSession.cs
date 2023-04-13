/******
작성자 : 이우열
작성일 : 23.04.13

최근 수정 일자 : 23.04.13
최근 수정 내용 : PacketSession 클래스 설계
 ******/

namespace ServerCore
{
    public abstract class PacketSession : Session
    {
        /// <summary> 패킷 검사, 정상 패킷일 시 패킷 분석 </summary>
        public sealed override void OnRecv(ArraySegment<byte> buffer)
        {

        }

        /// <summary> 패킷 분석 후 해당하는 핸들러 호출 </summary>
        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
    }
}