/******
작성자 : 박성택
작성 일자 : 23.03.29

최근 수정 일자 : 23.03.29
최근 수정 내용 : 사운드 매니저 클래스 생성
 ******/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class SoundManager
    {
        AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
        Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

        public void Init()
        {
            GameObject root = GameObject.Find("@Sound");

            if (root == null)
            {
                root = new GameObject { name = "@Sound" };
                UnityEngine.Object.DontDestroyOnLoad(root);

                for (Define.Sound s = Define.Sound.BGM; s < Define.Sound.MaxCount; s++)
                {
                    GameObject go = new GameObject { name = $"{s}" };
                    _audioSources[(int)s] = go.AddComponent<AudioSource>();
                    go.transform.parent = root.transform;
                }

                _audioSources[(int)Define.Sound.BGM].loop = true;
            }
            else
            {
                for (Define.Sound s = Define.Sound.BGM; s < Define.Sound.MaxCount; s++)
                {
                    GameObject go = root.transform.Find($"{s}").gameObject;
                    _audioSources[(int)s] = go.GetComponent<AudioSource>();
                }

                _audioSources[(int)Define.Sound.BGM].loop = true;
            }


        }

        /// <summary>
        /// SFX용 PlayOneShot으로 구현 
        /// </summary>
        /// <param name="SFXSound"> Define.SFX Enum 에서 가져오기를 바람 </param>
        /// <param name="pitch"></param>

        public void Play(Define.SFX SFXSound, float pitch = 1.0f)
        {
            string path = $"{SFXSound}";
            AudioClip audioClip = GetOrAddAudioClip(path, Define.Sound.SFX);
            Play(audioClip, Define.Sound.SFX, pitch);
        }
        /// <summary>
        /// BGM용 Play로 구현
        /// </summary>
        /// <param name="BGMSound">Define.BGM Enum 에서 가져오기를 바람 </param>
        /// <param name="pitch"></param>
        public void Play(Define.BGM BGMSound, float pitch = 1.0f)
        {
            string path = $"{BGMSound}";
            AudioClip audioClip = GetOrAddAudioClip(path, Define.Sound.BGM);
            Play(audioClip, Define.Sound.BGM, pitch);
        }

        void Play(AudioClip audioClip, Define.Sound type = Define.Sound.SFX, float pitch = 1.0f)
        {
            if (audioClip == null)
                return;

            if (type == Define.Sound.BGM)
            {
                AudioSource audioSource = _audioSources[(int)Define.Sound.BGM];
                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.pitch = pitch;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                AudioSource audioSource = _audioSources[(int)Define.Sound.SFX];
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(audioClip);
            }
        }

        AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.SFX)
        {
            if (path.Contains("Sounds/") == false)
                path = $"Sounds/{path}";

            AudioClip audioClip = null;

            if (type == Define.Sound.BGM)
            {
                audioClip = GameManager.Resource.Load<AudioClip>(path);
            }
            else
            {
                if (_audioClips.TryGetValue(path, out audioClip) == false)
                {
                    audioClip = GameManager.Resource.Load<AudioClip>(path);
                    _audioClips.Add(path, audioClip);
                }
            }

            if (audioClip == null)
                Debug.Log($"AudioClip Missing ! {path}");

            return audioClip;
        }

        public void Clear()
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                audioSource.clip = null;
                audioSource.Stop();
            }
            _audioClips.Clear();
        }
    }
}
