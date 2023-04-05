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
            StartCoroutine(GameManager.InGameData.Cooldown.CooldownCoroutine());

            TestjsonHandler handler = Util.ParseJson<TestjsonHandler>();
            foreach(Testjson j in handler.testjsons)
                Debug.Log($"{j.idx} : {j.name}");
        }
    }
}
