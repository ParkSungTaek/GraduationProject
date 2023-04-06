using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class MonsterSpawn : MonoBehaviour
    {

        /// <summary>
        /// 본인이 까먹어서 해두는 메모
        /// FantasyMonsters 폴더 내부 구조 바꾸면 타 프로그래머와 연동 안돌아감 
        /// </summary>
        GameObject[] SpawnPoint;
        GameObject Monster;
        int SpawnPointNum = 12;
        float Xradius = 10;
        float yradius = 5;


        IEnumerator _startCreateMonster;
        Define.MonsterName _nowMonster;
        float MonsterToMonster = 1.0f;
        float WaveToWave = 3.0f;
        int Wavenum = 3;
        int Count;
        Transform _monsterHPCanvas;
        public Transform MonsterHPCanvas { get { return _monsterHPCanvas; } }

        /// <summary>
        /// 
        /// </summary>
        List<MonsterController> _monsters = new List<MonsterController>();
        public List<MonsterController> Monsters { get { return _monsters; } }
        GameObject InstantiateMonster(Define.MonsterName monster,Transform SpawnPoint)
        {
            string monsterStr = Enum.GetName(typeof(Define.MonsterName), monster);
            return GameManager.Resource.Instantiate($"Monster/{monsterStr}", SpawnPoint);
        }


        void init()
        {
            SpawnPoint = new GameObject[SpawnPointNum];
            _nowMonster = Define.MonsterName.BlackBoar;
            _monsterHPCanvas = GameManager.UI.ShowSceneUI<UI_MonsterHP>().transform;
            _startCreateMonster = StartCreateMonster();
            Count = 0;
            for (int i = 0; i < SpawnPointNum; i++)
            {
                SpawnPoint[i] = new GameObject { name = $"SpawnPoint{i}" };
                SpawnPoint[i].transform.parent = transform;
                SpawnPoint[i].transform.position = new Vector3(Mathf.Cos(((2 * Mathf.PI) / SpawnPointNum) * i) * Xradius,
                    Mathf.Sin(((2 * Mathf.PI) / SpawnPointNum) * i) * yradius, 0);
            }

        }
        
        // Start is called before the first frame update
        void Start()
        {
            init();
            StartCoroutine(_startCreateMonster);
        }

        IEnumerator StartCreateMonster()
        {
            while (GameManager.InGameData.Stat() == Define.State.Play)
            {
                if (Count < Wavenum)
                {
                    MonsterController mon = InstantiateMonster(_nowMonster, SpawnPoint[UnityEngine.Random.Range(0, SpawnPointNum)].transform).GetComponent<MonsterController>();
                    _monsters.Add(mon);
                    yield return new WaitForSecondsRealtime(MonsterToMonster);
                    Count += 1;
                }
                else
                {
                    Count = 0;
                    _nowMonster += 1;
                    yield return new WaitForSecondsRealtime(WaveToWave);

                }
            }
        }



    }
}