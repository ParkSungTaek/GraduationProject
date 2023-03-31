using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class ResourceManager
    {


        //Dictionary<string,Object> 암튼암튼 캐싱은 여기서

        Dictionary<Define.Item, Item> 인벤토리;



        public void init()
        {

        }
        public T Load<T>(string path) where T : Object
        {
            //풀링 안한다고 했던가?
            GameManager.의논이필요함();

            /*
            if (typeof(T) == typeof(GameObject))
            {
                string name = path;
                int index = name.LastIndexOf('/');
                if (index >= 0)
                    name = name.Substring(index + 1);

                GameObject go = GameManager.Pool.GetOriginal(name);
                if (go != null)
                    return go as T;
            }
            */

            return Resources.Load<T>(path);
        }

        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject original = Load<GameObject>($"Prefabs/{path}");
            if(original == null)
            {
                Debug.LogError($"Failed to load prefab : {path}");
                return null;
            }    

            GameObject go = Object.Instantiate(original, parent);
            go.name = original.name;
            return go;
        }

        public void Clear()
        {

        }
    }
}
