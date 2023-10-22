/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.05.16
최근 수정 내용 : 플레이어 애니메이션 동기화 패킷(PlayerAttack), 사제 버프(PriestBuff), ItemUpdate 생성
 ******/

using ServerCore;
using System;

namespace Client
{
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 플레이어 클래스 선택 정보 
    /// </summary>
    public class CTS_SelectClass : IPacket
    {
        public ushort Protocol => (ushort)PacketID.CTS_SelectClass;
        public ushort PlayerClass;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            PlayerClass = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(PlayerClass), 0, segment.Array, segment.Offset + count, sizeof(ushort));
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
        public ushort Protocol => (ushort)PacketID.CTS_TowerDamage;
        /// <summary> 몬스터 데미지</summary>
        public ushort DMG;
        public ushort MonsterID;
        public ushort AttackCnt;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            DMG = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            MonsterID = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            AttackCnt = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(DMG), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(MonsterID), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(AttackCnt), 0, segment.Array, segment.Offset + count, sizeof(ushort));
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
        public ushort Protocol => (ushort)PacketID.CTS_PlayerMove;
        /// <summary> x 좌표 </summary>
        public float posX;
        /// <summary> y 좌표 </summary>
        public float posY;

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

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);
            Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 플레이어 행동 애니메이션 동기화를 위한 패킷
    /// </summary>
    public class CTS_PlayerAttack : IPacket
    {
        public ushort Protocol => (ushort)PacketID.CTS_PlayerAttack;
        /// <summary> 0 : 기본 공격, 1 : 스킬 </summary>
        public ushort skillType;
        /// <summary> 0 상, 1 우, 2 하, 3 좌 </summary>
        public ushort direction;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            skillType = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            direction = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(skillType), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(direction), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 사제 버프 패킷
    /// </summary>
    public class CTS_PriestBuff : IPacket
    {
        public ushort Protocol => (ushort)PacketID.CTS_PriestBuff;
        /// <summary> 대상 플레이어 </summary>
        public int playerId;
        /// <summary> 버프 수치 </summary>
        public float buffRate;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
            buffRate = BitConverter.ToSingle(segment.Array, segment.Offset + count);
            count += sizeof(float);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes(buffRate), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 아이템 정보 업데이트 패킷
    /// </summary>
    public class CTS_ItemUpdate : IPacket
    {
        public ushort Protocol => (ushort)PacketID.CTS_ItemUpdate;
        /// <summary> 새로운 아이템 </summary>
        public ushort itemIdx;
        /// <summary> 해당 아이템의 위치 </summary>
        public ushort position;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            itemIdx = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            position = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(itemIdx), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(position), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 박성택 <br/>
    /// CTS_MonsterHPUpdate 로 class명 수정 모든 몬스터 OBJ는 체력이 0이하인 OBJ를 죽은것으로 간주한다. 따로 죽음을 처리해주지는 않는다.
    /// ID : 데미지를 입은 몬스터의 ID 
    /// updateHP : 후의 HP;

    /// </summary>
    public class CTS_MonsterHPUpdate : IPacket
    {
        public ushort Protocol => (ushort)PacketID.CTS_MonsterHPUpdate;
        /// <summary> 몬스터의 ID</summary>
        public ushort ID;
        /// <summary> 몬스터의 HP 0 이하의 음수라면 죽음 -가능 할수도</summary>
        public short updateHP;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            ID = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            updateHP = BitConverter.ToInt16(segment.Array, segment.Offset + count);
            count += sizeof(short);

        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(ID), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(updateHP), 0, segment.Array, segment.Offset + count, sizeof(short));
            count += sizeof(short);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}