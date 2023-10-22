/******
공동 작성
작성일 : 23.03.29

최근 수정 일자 : 23.10.01
최근 수정 사항 : 로그인 매니저 추가
******/
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class GameManager : MonoBehaviour
    {
        static GameManager _instance;
        static GameManager Instance { get { Init(); return _instance; } }
        #region Managers
        LoginManager _loginManager = new LoginManager();
        NetworkManager _networkManager = new NetworkManager();
        PoolManager _poolManager = new PoolManager();
        ResourceManager _resourceManager = new ResourceManager();
        SoundManager _soundManager = new SoundManager();
        InGameDataManager _inGameDataManager = new InGameDataManager();
        UIManager _uiManager = new UIManager();
        RoomManager _roomManager = new RoomManager();
        ExistRoomManager _existRoomManager = new ExistRoomManager();

        public static LoginManager Login => Instance._loginManager;
        public static NetworkManager Network => Instance._networkManager;
        public static PoolManager Pool => Instance._poolManager;
        public static ResourceManager Resource => Instance._resourceManager;
        public static SoundManager Sound => Instance._soundManager;
        public static InGameDataManager InGameData => Instance._inGameDataManager;
        public static UIManager UI => Instance._uiManager;
        public static RoomManager Room => Instance._roomManager;
        public static ExistRoomManager ExistRoom => Instance._existRoomManager;
        #endregion

        /// <summary> instance 생성, 산하 매니저들 초기화 </summary>
        static void Init()
        {
            if (_instance == null)
            {
                GameObject gm = GameObject.Find("GameManager");
                if (gm == null)
                {
                    gm = new GameObject { name = "GameManager" };
                    gm.AddComponent<GameManager>();
                }
                _instance = gm.GetComponent<GameManager>();
                DontDestroyOnLoad(gm);

                var con = JsonUtility.FromJson<ConnectionInfo>(Resources.Load<TextAsset>("Jsons/ConnectionInfo").text);
                if (string.IsNullOrEmpty(con.Host))
                {
                    con.Host = System.Net.Dns.GetHostName();
                }

                _instance._networkManager.Init(con);
                _instance._loginManager.Init(con);

                _instance._poolManager.Init();
                _instance._soundManager.Init();
                _instance._inGameDataManager.Init();
            }
        }

        /// <summary> 게임 시작, 상태 변경, 인게임 정보 초기화 </summary>
        public static void GameStart(Dictionary<int, PlayerClassInfo> players)
        {   
            UI.CloseAllPopUpUI();

            Time.timeScale = 1;
            InGameData.StateChange(Define.State.Play);
            InGameData.GameStart(players);
        }
        /// <summary> 승리 또는 패배 시 호출, 시간 정지, 상태 변경, UI 띄우기 </summary>
        public static void GameOver(Define.State endState)
        {
            if (endState == Define.State.Win || endState == Define.State.Defeat)
            {
                Time.timeScale = 0;
                InGameData.StateChange(endState);

                _instance._uiManager.ShowPopUpUI<UI_GameOver>();
            }
        }

        /// <summary> 모든 정보 초기화 </summary>
        public static void Clear()
        {
            _instance._poolManager.Clear();
            _instance._resourceManager.Clear();
            _instance._soundManager.Clear();
        }

        private void Update()
        {
            _instance._networkManager.Flush();
        }

        public static Coroutine SetCoroutine(System.Collections.IEnumerator enumerator) => Instance.StartCoroutine(enumerator);
        public static void RemoveCoroutine(Coroutine coroutine) => Instance.StopCoroutine(coroutine);
    }
}
