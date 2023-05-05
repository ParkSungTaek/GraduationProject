/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.29
최근 수정 내용 : 필요한 패킷 목록 정리
 ******/

using ServerCore;
using System;

namespace Server
{
    //TODO : CTS_StartGame

    //TODO : CTS_SelectPlayerClass

    //TODO : CTS_PlayerAttack - 애니메이션 재생용 : 방향 표기, 피해받은 몬스터들 정보

    //TODO : CTS_PriestBuff - 버프

    /// <summary>
    /// 작성자 : 박성택
    /// 몬스터의 ItemInput , ItemPop (8번 이상의 뽑기를 수행하면 그 뒤로는 항상 Pop이 필요)
    /// </summary>
    public class CTS_ItemUpdate : IPacket
    {
        public ushort ItemInput;
        public ushort ItemPop;

        public ushort Protocol => (ushort)PacketID_Ingame.CTS_ItemUpdate;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            ItemInput = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            ItemPop = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)PacketID_Ingame.CTS_ItemUpdate), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(ItemInput), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(ItemPop), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 박성택
    /// 몬스터의 DMG
    /// </summary>
    public class CTS_TowerDamage : IPacket
    {
        /// <summary> 몬스터 데미지</summary>
        public ushort DMG;
        
        public ushort Protocol => (ushort)PacketID_Ingame.CTS_TowerDamage;


        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            DMG = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)PacketID_Ingame.CTS_TowerDamage), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(DMG), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }


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