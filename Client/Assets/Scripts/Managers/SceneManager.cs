using UnityEngine.SceneManagement;

namespace Client
{
    public class SceneManager
    {
        /// <summary> Enum으로 정의한 씬 전환 </summary>
        public static void LoadScene(Define.Scenes scene)
        {
            //ui popup 초기화
            GameManager.UI.Clear();
            //게임 진행사항 초기화
            GameManager.InGameData.Clear();
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene);
        }
    }
}
