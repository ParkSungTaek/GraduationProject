/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : 로그인 서버 패킷 핸들러 클래스 생성
 ******/

using ServerCore;

namespace LoginServer.Packet.Manage
{
    class PacketHandler
    {
        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 회원가입 패킷 처리
        /// </summary>
        public static void CTL_RegistHandler(PacketSession session, IPacket packet)
        {
            Session.LoginSession loginSession = session as Session.LoginSession;
            CTL_Regist registPacket = packet as CTL_Regist;

            DB.DBManager.Instance.CreateUser(registPacket.email, registPacket.password, (isSuccess) =>
            {
                LTC_RegistAck ackPacket = new LTC_RegistAck();
                ackPacket.isSuccess = isSuccess;
                loginSession.Send(ackPacket.Write());

                JobTimer.Instance.Push(() => loginSession.Disconnect(), 100);
            });
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 로그인 패킷 처리
        /// </summary>
        public static void CTL_LoginHandler(PacketSession session, IPacket packet)
        {
            Session.LoginSession loginSession = session as Session.LoginSession;
            CTL_Login loginPacket = packet as CTL_Login;

            string email = loginPacket.email;
            DB.DBManager.Instance.LoginUser(email, loginPacket.password, (isSuccess) =>
            {
                if (isSuccess)
                {
                    var serverSession = Session.SessionManager.Instance.AliveServer;

                    if (serverSession != null)
                    {
                        Session.SessionManager.Instance.PushWaitingSession(email, loginSession);

                        LTS_Auth authPacket = new LTS_Auth();
                        authPacket.email = email;

                        serverSession.Send(authPacket.Write());

                        return;
                    }
                }

                LTC_LoginAck ackPacket = new LTC_LoginAck();
                ackPacket.isSuccess = false;

                loginSession.Send(ackPacket.Write());
                JobTimer.Instance.Push(() => loginSession.Disconnect(), 100);
            });
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 서버의 로그인 확인 처리
        /// </summary>
        public static void STL_AuthAckHandler(PacketSession session, IPacket packet)
        {
            STL_AuthAck authAckPacket = packet as STL_AuthAck;

            Session.LoginSession loginSession = Session.SessionManager.Instance.PopWaitingSession(authAckPacket.email);

            LTC_LoginAck ackPacket = new LTC_LoginAck();
            ackPacket.isSuccess = loginSession != null;

            loginSession.Send(ackPacket.Write());
            JobTimer.Instance.Push(() => loginSession.Disconnect(), 100);
        }
    }
}