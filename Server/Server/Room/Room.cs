/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 인터페이스 설계
 ******/

using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    public class Room
    {
        public string RoomName;
        IngameData _ingameData = new IngameData();
        readonly int MAX_PLAYER_COUNT = 4;
        Random random = new Random();

        /// <summary> 현재 방에 존재하는 클라이언트들 </summary>
        List<ClientSession> _sessions = new List<ClientSession>();
        /// <summary> 작업 관리 queue </summary>
        JobQueue _jobQueue = new JobQueue();
        /// <summary> broadcasting 대기 중인 데이터들 </summary>
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
    

        /// <summary> 새로운 작업 수행 예약 </summary>
        public void Push(Action job) => _jobQueue.Push(job);

        /// <summary> 방 인원 수 </summary>
        public int Count => _sessions.Count;


        //job queue에서 수행하기 때문에 싱글 쓰레드 가정
        #region Jobs
        /// <summary> 대기 중인 broadcasting 모두 수행 </summary>
        public void Flush()
        {
            foreach (ClientSession session in _sessions)
                session.Send(_pendingList);

            _pendingList.Clear();
        }

        /// <summary> 브로드캐스팅 예약 </summary>
        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);
        }

        /// <summary> 새로운 클라이언트 입장 </summary>
        public void Enter(ClientSession session)
        {
            //풀방 -> 입장 불가
            if(_sessions.Count >= MAX_PLAYER_COUNT)
            {
                STC_RejectEnter_Full fullPacket = new STC_RejectEnter_Full();
                session.Send(fullPacket.Write());
                return;
            }

            _sessions.Add(session);
            session.Room = this;

            STC_PlayerEnter enterPacket = new STC_PlayerEnter();
            enterPacket.playerId = session.SessionId;

            Broadcast(enterPacket.Write());
        }

        /// <summary> 클라이언트 퇴장 </summary>
        public void Leave(ClientSession session)
        {
            //모든 유저가 나간 방 제거
            if (_sessions.Count <= 1)
            {
                RoomManager.Instance.Push(() => RoomManager.Instance.Remove(this));
                _sessions.Clear();
                RoomName = string.Empty;
                _ingameData = null;
                return;
            }

            //방장이 나갈 경우, 방장 변경
            if(_sessions.IndexOf(session) <= 0)
            {
                STC_SetSuper superPacket = new STC_SetSuper();
                _sessions[1].Send(superPacket.Write());
            }

            _sessions.Remove(session);

            STC_PlayerLeave leavePacket = new STC_PlayerLeave();
            leavePacket.playerId = session.SessionId;

            _jobQueue.Push(() => Broadcast(leavePacket.Write()));
        }

        /// <summary> 플레이어 이동 동기화 </summary>
        /// <param name="session"> 이동 패킷 보낸 세션 </param>
        public void Move(ClientSession session, CTS_PlayerMove movePacket)
        {
            STC_PlayerMove packet = new STC_PlayerMove();
            packet.playerId = session.SessionId;
            packet.posX = movePacket.posX;
            packet.posY = movePacket.posY;

            Broadcast(packet.Write());
        }

        /// <summary> Monster 생성 </summary>
        public void CreateMonster()
        {
            STC_MosterCreate packet = new STC_MosterCreate();
            packet.createIDX = (ushort)random.Next(0,12);
            packet.ID = _ingameData.MonsterControlInfo.NextMosterID;
            packet.typeNum = _ingameData.MonsterControlInfo.MonsterTypeNum;

            Console.WriteLine($"위치: {packet.createIDX} \t 몬스터 ID: {packet.ID} \t 몬스터 type: {packet.typeNum}");

            Broadcast(packet.Write());
        }

        public async void Start()
        {
            _ingameData.State = IngameData.state.Play;
            while (_ingameData.State == IngameData.state.Play)
            {

                //몬스터 생성
                CreateMonster();
                Console.WriteLine("_ingameData.MonsterControlInfo.MonsterToMonster: " + _ingameData.MonsterControlInfo.MonsterToMonster);
                await Task.Delay(TimeSpan.FromSeconds(_ingameData.MonsterControlInfo.MonsterToMonster * 0.1f));

                //다음 Wave
                if (_ingameData.MonsterControlInfo.NextWave)
                {
                    Console.WriteLine("------------------");
                    Console.WriteLine("_ingameData.MonsterControlInfo.WaveToWave; " + _ingameData.MonsterControlInfo.WaveToWave);
                    await Task.Delay(TimeSpan.FromSeconds(_ingameData.MonsterControlInfo.WaveToWave));
                    _ingameData.MonsterControlInfo.NextWave = false;
                    Console.WriteLine("------------------");
                }
                
            }


        }

        
        #endregion Jobs
    }
}