/*
공동 작성
작성일 : 23.03.29

최근 수정 일자 : 23.04.10
최근 수정 사항 : 주석 정리
*/
using UnityEngine;

namespace Client
{
    public class GameManager : MonoBehaviour
    {
        static GameManager _instance;
        static GameManager Instance { get { Init(); return _instance; } }
        #region Managers
        NetworkManager _networkManager = new NetworkManager();
        PoolManager _poolManager = new PoolManager();
        ResourceManager _resourceManager = new ResourceManager();
        SoundManager _soundManager = new SoundManager();
        InGameDataManager _inGameDataManager = new InGameDataManager();
        UIManager _uiManager = new UIManager();
        public static NetworkManager Network { get { return Instance._networkManager; } }
        public static PoolManager Pool { get { return Instance._poolManager; } }
        public static ResourceManager Resource { get { return Instance._resourceManager; } }
        public static SoundManager Sound { get { return Instance._soundManager; } }
        public static InGameDataManager InGameData { get { return Instance._inGameDataManager; } }
        public static UIManager UI { get { return Instance._uiManager; } }
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

                _instance._networkManager.Init();
                _instance._poolManager.Init();
                _instance._soundManager.Init();
                _instance._inGameDataManager.Init();
            }

        }
        /// <summary> 게임 시작, 상태 변경, 인게임 정보 초기화 </summary>
        public static void GameStart()
        {
            Time.timeScale = 1;
            InGameData.StateChange(Define.State.Play);
            _instance._inGameDataManager.GameStart();
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
            //_instance._inputManager.Clear();
            _instance._networkManager.Clear();
            _instance._poolManager.Clear();
            _instance._resourceManager.Clear();
            _instance._soundManager.Clear();
        }

    }
}
