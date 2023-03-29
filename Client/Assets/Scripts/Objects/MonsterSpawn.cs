using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    GameObject[] SpawnPoint;
    GameObject Monster;
    int SpawnPointNum = 12;
    float Xradius = 10;
    float yradius = 5;

    float MonsterToMonster = 1.0f;
    float WaveToWave = 1.0f;



    void init()
    {
        SpawnPoint = new GameObject[SpawnPointNum];
        Monster = Resources.Load<GameObject>("Prefabs/Monster");
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

            Instantiate(Monster, SpawnPoint[Random.Range(0, SpawnPointNum)].transform);
            yield return new WaitForSecondsRealtime(MonsterToMonster);
        }
    }



}
