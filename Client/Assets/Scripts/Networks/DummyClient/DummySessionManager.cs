using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
	class DummySessionManager
	{
		static DummySessionManager _session = new DummySessionManager();
		public static DummySessionManager Instance { get { return _session; } }

		List<ServerSession> _sessions = new List<ServerSession>();
		object _lock = new object();
		Random _rand = new Random();

		public ServerSession Generate()
		{
			lock (_lock)
			{
				ServerSession session = new ServerSession();
				_sessions.Add(session);
				return session;
			}
		}
	}
}
