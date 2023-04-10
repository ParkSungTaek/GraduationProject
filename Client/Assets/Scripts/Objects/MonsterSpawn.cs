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
        /// 

        GameObject[] SpawnPoint;
        GameObject Monster;
        int SpawnPointNum = 12;
        float Xradius = 10;
        float yradius = 5;

                                                    
        IEnumerator _startCreateMonster;            // 몬스터 생성 Coroutine
        Define.MonsterName _nowMonster;             // 현재 생성 Monster 종류
        float MonsterToMonster = 0.25f;              // Monster 나오고 다음 Monster나올때까지 시간 텀 
        float WaveToWave = 1.0f;                    // Wave끝나고(==이번 Wave의 마지막 몬스터 나온 시점) 다음 Wave 시작 전까지 시간 텀
        int Wavenum = 3;                            // tmp 변수 한 Wave에서 나올 몬스터 숫자 (차후 Wave수에 종속적으로 변경할 것)
        int Count;                                  // 이번 Wave 몬스터 나온 숫자
        Transform _monsterHPCanvas;                 // Monster HP 붙여줄 Canvas의 Transform
        public Transform MonsterHPCanvas { get { return _monsterHPCanvas; } }

        /// <summary>
        /// 
        /// </summary>
        List<MonsterController> _monsters = new List<MonsterController>();
        public List<MonsterController> Monsters { get { return _monsters; } }
        //InstantiateMonster을 내부적으로 더 쉽게 재정의 효과 (Enum을 활용하여 Monster의 이름을 외울 필요를 제거, 다음Wave를 ++식으로 편하게)
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

        public bool WaveEnd()
        {
            return _nowMonster == Define.MonsterName.MaxCount;
        }

        IEnumerator StartCreateMonster()
        {
            while (GameManager.InGameData.CurrState() == Define.State.Play)
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
                    if(_nowMonster == Define.MonsterName.MaxCount)
                    {
                        break;
                    }
                    yield return new WaitForSecondsRealtime(WaveToWave);

                }
            }
        }



    }
}