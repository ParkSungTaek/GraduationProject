using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class TowerController : Entity
    {
        GameObject _towerHpBar;
        Transform Tower_UI_Scene;
        // Start is called before the first frame update

        protected override void init()
        {
            MaxHP = 1000;
            Nowhp = MaxHP;
            AttackDMG = 0;
            MoveSpeed = 0;

            _towerHpBar = Instantiate(Resources.Load<GameObject>("Prefabs/UI/TowerHP"));
            Tower_UI_Scene = GameManager.UI.ShowSceneUI<UI_Scene>("TowerHPCanvas").transform;

            _towerHpBar.transform.parent = Tower_UI_Scene;

            HpBarSlider = _towerHpBar.GetComponent<Slider>();

        }
        protected override void Dead()
        {
            GameManager.GameOver();
        }

        // Update is called once per frame
        void Update()
        {
            _towerHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.8f, 0));

        }
    }
}
