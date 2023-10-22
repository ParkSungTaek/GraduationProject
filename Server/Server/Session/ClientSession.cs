/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.10.02
최근 수정 내용 : email 정보 추가
 ******/

using ServerCore;
using System;
using System.Net;

namespace Server
{
    public class ClientSession : PacketSession
    {
        public int SessionId;
        public string email;
        public Room Room { get; set; }

        public override void OnConnected(EndPoint endPoint)
        {
            ServerCore.Logger.Log($"OnConnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int byteTransfered)
        {
            //Do Nothing
        }

        public override void OnDisconnected(EndPoint endPoint, bool needRemove)
        {
            if(Room != null)
            {
                Room room = Room;
                room.Push(() => room.Leave(this));
                Room = null;
            }

            if (needRemove)
                ClientSessionManager.Instance.OnDisconnect(this);

            ServerCore.Logger.Log($"OnDisconnected : {endPoint}");
        }
    }

}