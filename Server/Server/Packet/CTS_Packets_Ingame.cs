﻿/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 플레이어 이동 패킷 추가
 ******/

using ServerCore;
using System;

namespace Server
{
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 클라 -> 서버 플레이어 이동 패킷
    /// </summary>
    public class CTS_PlayerMove : IPacket
    {
        /// <summary> x 좌표 </summary>
        public float posX;
        /// <summary> y 좌표 </summary>
        public float posY;

        public ushort Protocol => (ushort)PacketID_Ingame.CTS_PlayerMove;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
            count += sizeof(float);
            posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
            count += sizeof(float);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)PacketID_Ingame.CTS_PlayerMove), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);
            Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}