/******
작성자 : 이우열
작성 일자 : 23.04.05

최근 수정 일자 : 23.04.05
최근 수정 사항 : 모든 씬 로드 기능은 이 매니저를 통해 수행
******/

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
            //방 입장 함수 연결 해제
            GameManager.Room.ClearActions();
            GameManager.ExistRoom.Clear();

            UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene);
        }
    }
}
