/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.05.16
최근 수정 내용 : 플레이어 애니메이션 동기화 패킷(PlayerAttack), 사제 버프(PriestBuff), ItemUpdate 생성
 ******/

using ServerCore;
using System;

namespace Server
{
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 클라이언트의 선택 정보 공유 패킷
    /// </summary>
    public class STC_SelectClass : IPacket
    {
        public ushort Protocol => (ushort)PacketID.STC_SelectClass;
        public int PlayerId;
        public ushort PlayerClass;

        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            PlayerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
            PlayerClass = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            int count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(PlayerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes(PlayerClass), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 모든 클라이언트가 캐릭터 선택 완료 -> 게임 시작 브로드캐스팅
    /// </summary>
    public class STC_StartGame : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.STC_StartGame;
    }

    #region Non-Playable

    //삭제 : STC_MonsterMove

    /// <summary>
    /// 작성자 : 박성택 
    /// Monster create위치, Monster type, Monster ID
    /// </summary>
    public class STC_MosterCreate : IPacket 
    {
        /// <summary> 시작 위치 12개</summary>
        public ushort createIDX;
        /// <summary> Monster 타입 번호 </summary>
        public ushort typeNum;
        /// <summary> Monster 식별 번호 </summary>
        public ushort ID;

        public ushort Protocol => (ushort)PacketID.STC_MosterCreate;


        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            createIDX = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            typeNum = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            ID = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
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

            Array.Copy(BitConverter.GetBytes(createIDX), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(typeNum), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(ID), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }


    /// <summary>
    /// 작성자 : 박성택 
    /// STC_MonsterDie -> STC_MonsterHPUpdate 로 class명 수정 모든 몬스터 OBJ는 체력이 0이하인 OBJ를 죽은것으로 간주한다. 따로 죽음을 처리해주지는 않는다.
    /// ID : 데미지를 입은 몬스터의 ID 
    /// updateHP : 후의 HP;
    /// </summary>
    public class STC_MonsterHPUpdate : IPacket
    {
        /// <summary> 몬스터의 ID</summary>
        public ushort ID;

        /// <summary> 몬스터의 HP 0 이하의 음수라면 죽음 -가능 할수도</summary>
        public short updateHP;
        
        public ushort Protocol => (ushort)PacketID.STC_MonsterHPUpdate;


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



    /// <summary>
    /// 작성자 : 박성택 
    /// TowerHP short범위를 넘어설 수도 있을듯 하여 int
    /// </summary>
    public class STC_TowerUpdate : IPacket
    {
        /// <summary> 몬스터가 받은 DMG</summary>
        public int updateHP;

        public ushort Protocol => (ushort)PacketID.STC_TowerUpdate;


        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            updateHP = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);

        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(updateHP), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    //TODO : STC_GameOver
    /// <summary>
    /// 작성자 : 박성택 
    /// GameOver패킷이 왔다 == 게임이 이미 끝났다. 
    /// 이 패킷 안에서 굳이 뭔가 게임이 진행중인지 더 설명해야하나? 아니다. 
    /// 그저 이겼냐 졌냐만 알려주면 된다.
    /// win 이 true면 이김 false 면 짐
    /// </summary>
    public class STC_GameOver : IPacket
    {
        /// <summary> 몬스터가 받은 DMG</summary>
        public bool win;

        public ushort Protocol => (ushort)PacketID.STC_TowerUpdate;


        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            win = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
            count += sizeof(bool);

        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(win), 0, segment.Array, segment.Offset + count, sizeof(bool));
            count += sizeof(bool);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    //TODO : STC_GameClear -- ?? 이게 무슨 패킷이지? 데이터를 초기화 하는 패킷인가? 아니면 위가 게임 진경우 이게 게임 이긴 경우인가? 
    
    #endregion Non-Playable

    #region Playable
    //TODO : STC_GetScoreAndMoney

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 플레이어 애니메이션 동기화를 위한 패킷
    /// </summary>
    public class STC_PlayerAttack : IPacket
    {
        /// <summary> 해당 플레이어 id </summary>
        public int playerId;
        /// <summary> 0 : 기본 공격, 1 : 스킬 </summary>
        public ushort skillType;
        /// <summary> 0 상, 1 우, 2 하, 3 좌 </summary>
        public ushort direction;

        public ushort Protocol => (ushort)PacketID.STC_PlayerAttack;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
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

            Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes(skillType), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(direction), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    //TODO : STC_ItemUpdate

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 서버 -> 클라 플레이어 이동 패킷
    /// </summary>
    public class STC_PlayerMove : IPacket
    {
        /// <summary> 플레이어 id </summary>
        public int playerId;
        /// <summary> x 좌표 </summary>
        public float posX;
        /// <summary> y 좌표 </summary>
        public float posY;

        public ushort Protocol => (ushort)PacketID.STC_PlayerMove;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
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

            Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);

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
    /// 사제 버프 패킷
    /// </summary>
    public class STC_PriestBuff : IPacket
    {
        /// <summary> 버프 수치 </summary>
        public float buffRate;
        public ushort Protocol => (ushort)PacketID.STC_PriestBuff;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

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

            Array.Copy(BitConverter.GetBytes(buffRate), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 아이템 정보 동기화 패킷
    /// </summary>
    public class STC_ItemUpdate : IPacket
    {
        public int playerId;
        /// <summary> 새로운 아이템 </summary>
        public ushort itemIdx;
        /// <summary> 해당 아이템의 위치 </summary>
        public ushort position;

        public ushort Protocol => (ushort)PacketID.STC_ItemUpdate;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
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
            Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes(itemIdx), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(position), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
    #endregion Playable
}