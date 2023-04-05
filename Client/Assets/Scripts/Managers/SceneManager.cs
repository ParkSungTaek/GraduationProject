using UnityEngine.SceneManagement;

namespace Client
{
    public class SceneManager
    {
        public static void LoadScene(Define.Scenes scene)
        {
            GameManager.UI.Clear();
            GameManager.InGameData.Clear();
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene);
        }
    }
}
