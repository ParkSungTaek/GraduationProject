/******
작성자 : 박성택
작성 일자 : 23.03.31

최근 수정 일자 : 23.04.16
최근 수정 내용 : wave 구현
 ******/

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

        protected override void Init()
        {
            MaxHP = 3000;
            Nowhp = MaxHP;
            AttackDMG = 0;
            MoveSpeed = 0;

            Tower_UI_Scene = GameManager.UI.ShowSceneUI<UI_TowerHP>().transform;
            _towerHpBar = GameManager.Resource.Instantiate("UI/TowerHP", Tower_UI_Scene);

            HpBarSlider = _towerHpBar.GetComponent<Slider>();

        }
        protected override void Dead()
        {
            GameManager.GameOver(Define.State.Defeat);
        }

        // Update is called once per frame
        void Update()
        {
            _towerHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.8f, 0));

        }
    }
}
