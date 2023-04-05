using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class TitleScene : MonoBehaviour
    {
        void Start()
        {
            GameManager.UI.ShowSceneUI<UI_TitleScene>();
        }
    }
}
