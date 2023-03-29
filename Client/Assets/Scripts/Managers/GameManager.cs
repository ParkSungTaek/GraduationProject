using UnityEngine;

namespace Client
{
    public class GameManager : MonoBehaviour
    {
        static GameManager _instance;
        static GameManager Instance { get { init(); return _instance; } }
        #region Managers
        InputManager _inputManager = new InputManager();
        NetworkManager _networkManager = new NetworkManager();
        PoolManager _poolManager = new PoolManager();
        ResourceManager _resourceManager = new ResourceManager();
        SoundManager _soundManager = new SoundManager();
        InGameDataManager _inGameDataManager = new InGameDataManager();
        public static InputManager Input { get { return Instance._inputManager; } }
        public static NetworkManager Network { get { return Instance._networkManager; } }
        public static PoolManager Pool { get { return Instance._poolManager; } }
        public static ResourceManager Resource { get { return Instance._resourceManager; } }
        public static SoundManager Sound { get { return Instance._soundManager; } }
        public static InGameDataManager InGameData { get { return Instance._inGameDataManager; } }

        #endregion

        public static void 의논이필요함()
        {
            //
        }
        private void Start()
        {
            init();

        }

        static void init()
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

                _instance._inputManager.init();
                _instance._networkManager.init();
                _instance._poolManager.init();
                _instance._resourceManager.init();
                _instance._soundManager.init();
                _instance._inGameDataManager.init();

                _instance._inGameDataManager.StateChange(Define.State.Play);
            }

        }
        public static void Clear()
        {
            _instance._inputManager.Clear();
            _instance._networkManager.Clear();
            _instance._poolManager.Clear();
            _instance._resourceManager.Clear();
            _instance._soundManager.Clear();
        }

    }
}
