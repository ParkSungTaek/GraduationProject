using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Client
{
    public class GameScene : MonoBehaviour
    {
        private void Start()
        {
            GameManager.InGameData.StateChange(Define.State.Play);
            GameManager.UI.ShowSceneUI<UI_GameScene>();
        }
    }
}
