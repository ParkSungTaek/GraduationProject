using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Client
{
    public class BackGroundCreater : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Sound.Play(Define.SFX.MaxCount);
        }
        public void TmpTmp()
        {
            CTS_PlayerMove cTS_PlayerMove = new CTS_PlayerMove();

            ushort count = sizeof(ushort) + sizeof(ushort) + sizeof(float) + sizeof(float);

            ushort Protocol = (ushort)PacketID_Ingame.CTS_PlayerMove;
            float value3 = 1.25f;
            float value4 = 2.77f;

            // 값을 바이트 배열로 변환
            byte[] valueBytes1 = BitConverter.GetBytes(count);
            byte[] valueBytes2 = BitConverter.GetBytes(Protocol);
            byte[] valueBytes3 = BitConverter.GetBytes(value3);
            byte[] valueBytes4 = BitConverter.GetBytes(value4);


            // 바이트 배열을 참조하는 ArraySegment<byte> 생성
            byte[] buffer = new byte[valueBytes1.Length + valueBytes2.Length + valueBytes3.Length + valueBytes4.Length];

            Buffer.BlockCopy(valueBytes1, 0, buffer, 0, valueBytes1.Length);
            Buffer.BlockCopy(valueBytes2, 0, buffer, valueBytes1.Length, valueBytes2.Length);
            Buffer.BlockCopy(valueBytes3, 0, buffer, valueBytes1.Length + valueBytes2.Length, valueBytes3.Length);
            Buffer.BlockCopy(valueBytes4, 0, buffer, valueBytes1.Length + valueBytes2.Length + valueBytes3.Length, valueBytes4.Length);

            ArraySegment<byte> bytes = new ArraySegment<byte>(buffer);
            cTS_PlayerMove.Read(bytes);

            GameManager.Network.Send(cTS_PlayerMove.Write());

        }

    }
}
