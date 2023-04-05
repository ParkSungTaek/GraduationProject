using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class MonsterSpawn : MonoBehaviour
    {
        GameObject[] SpawnPoint;
        GameObject Monster;
        int SpawnPointNum = 12;
        float Xradius = 10;
        float yradius = 5;

        float MonsterToMonster = 2.0f;
        float WaveToWave = 1.0f;
        Transform _monsterHPCanvas;
        public Transform MonsterHPCanvas { get { return _monsterHPCanvas; } }

        /// <summary>
        /// 
        /// </summary>
        List<MonsterController> _monsters = new List<MonsterController>();
        public List<MonsterController> Monsters { get { return _monsters; } }


        void init()
        {
            SpawnPoint = new GameObject[SpawnPointNum];
            Monster = Resources.Load<GameObject>("Prefabs/Monster/Monster");
            _monsterHPCanvas = GameManager.UI.ShowSceneUI<UI_MonsterHP>().transform;

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
            StartCoroutine(StartCreateMonster());
        }

        IEnumerator StartCreateMonster()
        {
            while (GameManager.InGameData.Stat() == Define.State.Play)
            {

                MonsterController mon = Instantiate(Monster, SpawnPoint[Random.Range(0, SpawnPointNum)].transform).GetComponent<MonsterController>();
                _monsters.Add(mon);
                yield return new WaitForSecondsRealtime(MonsterToMonster);
            }
        }



    }
}