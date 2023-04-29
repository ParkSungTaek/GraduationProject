/******
작성자 : 공동 작성
작성 일자 : 23.03.29

최근 수정 일자 : 23.04.29
최근 수정 내용 : 캐시 추가
******/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class ResourceManager
    {
        /// <summary> 로드한 적 있는 object cache </summary>
        Dictionary<string, Object> _cache = new Dictionary<string, Object>();

        /// <summary> Resources.Load로 불러오기
        /// </summary>
        public T Load<T>(string path) where T : Object
        {
            int idx = path.LastIndexOf('/');
            string name = path.Substring(idx + 1);

            Object obj;
            //캐시에 존재 -> 캐시에서 반환
            if (_cache.TryGetValue(name, out obj))
                return obj as T;

            //캐시에 없음 -> 로드하여 캐시에 저장 후 반환
            obj = Resources.Load<T>(path);
            _cache.Add(name, obj);

            return obj as T;
        }

        /// <summary> GameObject 생성 </summary>
        public GameObject Instantiate(string path, Transform parent = null) => Instantiate<GameObject>(path, parent);
        /// <summary> T type object 생성 </summary>
        public T Instantiate<T>(string path, Transform parent = null) where T : UnityEngine.Object
        {
            T prefab = Load<T>($"Prefabs/{path}");
            if(prefab == null)
            {
                Debug.LogError($"Failed to load prefab : {path}");
                return null;
            }

            T instance = UnityEngine.Object.Instantiate<T>(prefab, parent);
            instance.name = prefab.name;

            return instance;
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}
