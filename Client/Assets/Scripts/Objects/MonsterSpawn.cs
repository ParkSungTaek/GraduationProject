/******
작성자 : 박성택
작성 일자 : 23.03.29

최근 수정 일자 : 23.04.16
최근 수정 내용 : 웨이브 생성 및 몬스터 종류 확대
 ******/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    /// <summary>
    /// 4/15 이전 박성택 구현부
    /// 4/16 박성택 업데이트 4/12일 협의 결과에 따른 몬스터 나오는 수치와 벨런스 반영 
    /// 
    /// </summary>
    public class MonsterSpawn : MonoBehaviour
    {
        #region 크게 바뀔 일 없는 데이터
        GameObject[] SpawnPoint;
        GameObject Monster;
        int SpawnPointNum = 12;
        float Xradius = 39f;
        float yradius = 19f;
        /// <summary> 현재 생성 Monster 종류 </summary>
        Define.MonsterName _nowMonster;
        /// <summary> 이번 Wave 몬스터 나온 숫자 </summary>
        int _count;                                  
        /// <summary> Monster HP 붙여줄 Canvas의 Transform </summary>
        Transform _monsterHPCanvas;
        /// <summary> Monster 의 HP를 그릴 Canvas</summary>
        public Transform MonsterHPCanvas { get { return _monsterHPCanvas; } }
        /// <summary> 0 ~ _cycle - 2 Wave 에는 일반 몬스터 _cycle - 1 Wave 에서는 보스 몬스터 </summary>
        const int _cycle = 10;
        #endregion

        #region 벨런싱 데이터 
        /// <summary>  tmp 변수 한 Wave에서 나올 몬스터 숫자 </summary>
        int _wavenum { get { return GameManager.InGameData.Wave % _cycle != _cycle - 1 ? (4 * (GameManager.InGameData.Wave / _cycle) + 2 * (GameManager.InGameData.Wave % _cycle) + 8) : 1 ; } }
        /// <summary> Monster 나오고 다음 Monster나올때까지 시간 텀  </summary>
        float _monsterToMonster { get { return (26.0f / (16.0f + GameManager.InGameData.Wave)); } }
        /// <summary>   Wave끝나고(==이번 Wave의 마지막 몬스터 나온 시점) 다음 Wave 시작 전까지 시간 텀 </summary>
        float _waveToWave = 8.0f;
        #endregion

        /// <summary> 소환된 몬스터 관리 </summary>
        List<MonsterController> _monsters = new List<MonsterController>();
        /// <summary> 소환된 몬스터 관리 </summary>
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
            _nowMonster = Define.MonsterName._0BlackBoar;
            _monsterHPCanvas = GameManager.UI.ShowSceneUI<UI_MonsterHP>().transform;
            _count = 0;
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
            //StartCoroutine(_startCreateMonster);
        }

        public bool WaveEnd()
        {
            return _nowMonster == Define.MonsterName.MaxCount;
        }



        public void CreateMonster(ushort createIDX, ushort typeNum, ushort ID)
        {
            MonsterController mon = InstantiateMonster((Define.MonsterName)(typeNum), SpawnPoint[createIDX].transform).GetComponent<MonsterController>();
            _monsters.Add(mon);
            _count += 1;

        }

        IEnumerator StartCreateMonster()
        {
            while (GameManager.InGameData.CurrState == Define.State.Play)
            {   
                //몬스터 생성
                if (_count < _wavenum)
                {
                    MonsterController mon = InstantiateMonster((Define.MonsterName)(GameManager.InGameData.Wave), SpawnPoint[UnityEngine.Random.Range(0, SpawnPointNum)].transform).GetComponent<MonsterController>();
                    _monsters.Add(mon);
                    yield return new WaitForSeconds(_monsterToMonster *0.1f);
                    

                }
                //다음 Wave
                else
                {
                    _count = 0;
                    GameManager.InGameData.Wave += 1;

                    if (_nowMonster == Define.MonsterName.MaxCount)
                    {
                        break;
                    }
                    yield return new WaitForSeconds(_waveToWave);

                }
            }
        }



    }
}