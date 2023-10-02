/******
작성자 : 이우열
작성 일자 : 23.05.03

최근 수정 일자 : 23.10.02
최근 수정 사항 : 연결 성공 콜백 추가
 ******/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
	/// <summary> 서버에 최초 연결을 설정하는 클래스 </summary>
	public class Connector
	{
		/// <summary> 연결 성공 시 세션 생성 함수 </summary>
		Func<Session> _sessionFactory;
		Client.IPacket _onConnect;

		public Connector(Client.IPacket onConnect = null)
		{
			_onConnect = onConnect;
		}

		/// <summary> 서버에 연결 시도 </summary>
		public void Connect(IPEndPoint endPoint, Func<Session> sessionFactory, int count = 1)
		{
			for (int i = 0; i < count; i++)
			{
				// 휴대폰 설정
				Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				_sessionFactory = sessionFactory;

				SocketAsyncEventArgs args = new SocketAsyncEventArgs();
				args.Completed += OnConnectCompleted;
				args.RemoteEndPoint = endPoint;
				args.UserToken = socket;

				RegisterConnect(args);
			}
		}

		void RegisterConnect(SocketAsyncEventArgs args)
		{
			Socket socket = args.UserToken as Socket;
			if (socket == null)
				return;

			bool pending = socket.ConnectAsync(args);
			if (pending == false)
				OnConnectCompleted(null, args);
		}

		void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
		{
			if (args.SocketError == SocketError.Success)
			{
				Session session = _sessionFactory.Invoke();
				session.Start(args.ConnectSocket);
				session.OnConnected(args.RemoteEndPoint);
				
				if (_onConnect != null)
				{
					session.Send(_onConnect.Write());
				}
			}
			else
			{
				Console.WriteLine($"OnConnectCompleted Fail: {args.SocketError}");
				Client.GameManager.Network.Push(() => 
				{ 
					Client.GameManager.UI.CloseAllPopUpUI();
					Client.GameManager.UI.ShowPopUpUI<Client.UI_ClosableLog>().SetLog("서버 연결에 실패했습니다.");
				});
				Client.GameManager.Login.RemoveTimer();
			}
		}
	}
}
