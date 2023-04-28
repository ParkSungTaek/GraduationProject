using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class BackGroundCreater : MonoBehaviour
    {
        GameObject[] TreesObj;
        List<GameObject> Objs = new List<GameObject>();
        int SpawnPointNum = 60;
        float Xradius = 40f;
        float yradius = 20f;
        float DX = 1f;
        float DY = 0.5f;
        float Mul = 1.1f;
        int Round = 40;
        
        enum Trees
        {
            AshTree,
            BlackOakTree,
            GreenAppleTree,
            IvyTree,
            ManticoreTree,
            MapleTree,
            OakTree,
            RedAppleTree,
            RedSandalTree,
            WillowTree,
            YewTree,
            MaxCount
        }
        void init()
        {

            TreesObj = new GameObject[(int)Trees.MaxCount];
            string[] TreesObjstr = Enum.GetNames(typeof(Trees));

            for (int i = 0; i < (int)Trees.MaxCount; i++)
            {
                TreesObj[i] = Resources.Load<GameObject>($"Prefabs/Trees/{TreesObjstr[i]}");
                Debug.Log(TreesObj[i].name);
            }

            for (int idx = 0; idx < Round; idx++)
            {
                for (int i = 0; i < SpawnPointNum; i++)
                {
                    GameObject tmp;
                    tmp = Instantiate(TreesObj[UnityEngine.Random.Range(0,(int)Trees.MaxCount)]);
                    tmp.transform.parent = transform;
                    tmp.transform.position = new Vector3(Mathf.Cos(((2 * Mathf.PI) / SpawnPointNum) * i) * Xradius,
                        Mathf.Sin(((2 * Mathf.PI) / SpawnPointNum) * i) * yradius, 0);
                }
                Xradius += DX;
                yradius += DY;
                SpawnPointNum = (int)(SpawnPointNum * Mul);
            }
            

        }

        private void Start()
        {
            init();
            string filename = "screenshot.png";
            
            ScreenCapture.CaptureScreenshot(filename, ScreenCapture.StereoScreenCaptureMode.RightEye);
        }
    }
}
