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

            DB.DBManager.Instance.CheckUser(registPacket.email, (isDuplicate) =>
            {
                bool mailSuccess = false;
                if (isDuplicate == false)
                {
                    mailSuccess = SMTP.SMTPManager.Instance.Regist(registPacket.email, registPacket.password);
                }

                LTC_RegistAck ackPacket = new LTC_RegistAck();
                if (isDuplicate)
                    ackPacket.errorCode = (ushort)LTC_RegistAck.ErrorCode.Duplicate;
                else if (mailSuccess == false)
                    ackPacket.errorCode = (ushort)LTC_RegistAck.ErrorCode.MailError;
                else
                    ackPacket.errorCode = (ushort)LTC_RegistAck.ErrorCode.Success;

                loginSession.Send(ackPacket.Write());

                JobTimer.Instance.Push(() => loginSession.Disconnect(), 100);
            });
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 회원탈퇴 패킷 처리
        /// </summary>
        public static void CTL_UnregistHandler(PacketSession session, IPacket packet)
        {
            Session.LoginSession loginSession = session as Session.LoginSession;
            CTL_Unregist unregistPacket = packet as CTL_Unregist;

            DB.DBManager.Instance.DeleteUser(unregistPacket.email, unregistPacket.password, (isSuccess) =>
            {
                LTC_UnregistAck ackPacket = new LTC_UnregistAck();
                if (isSuccess)
                    ackPacket.errorCode = (ushort)LTC_UnregistAck.ErrorCode.Success;
                else
                    ackPacket.errorCode = (ushort)LTC_UnregistAck.ErrorCode.AccountError;

                loginSession.Send(ackPacket.Write());

                JobTimer.Instance.Push(() => loginSession.Disconnect(), 100);
            });
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 디버깅 용 메일 인증 없는 회원가입
        /// </summary>
        public static void CTL_ForceRegistHandler(PacketSession session, IPacket packet)
        {
            Session.LoginSession loginSession = session as Session.LoginSession;
            CTL_ForceRegist registPacket = packet as CTL_ForceRegist;

            DB.DBManager.Instance.CreateUser(registPacket.email, registPacket.password, (isSuccess) =>
            {

                LTC_RegistAuthAck ackPacket = new LTC_RegistAuthAck();
                if (isSuccess)
                    ackPacket.errorCode = (ushort)LTC_RegistAuthAck.ErrorCode.Success;
                else
                    ackPacket.errorCode = (ushort)LTC_RegistAuthAck.ErrorCode.DBError;

                loginSession.Send(ackPacket.Write());

                JobTimer.Instance.Push(() => loginSession.Disconnect(), 100);
            });
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 회원가입 메일 인증 처리
        /// </summary>
        public static void CTL_RegistAuthHandler(PacketSession session, IPacket packet)
        {
            Session.LoginSession loginSession = session as Session.LoginSession;
            CTL_RegistAuth authPacket = packet as CTL_RegistAuth;

            SMTP.SMTPManager.Instance.Auth(authPacket.email, authPacket.authNo, (isSuccess) =>
            {
                LTC_RegistAuthAck ackPacket = new LTC_RegistAuthAck();
                ackPacket.errorCode = isSuccess ? (ushort)LTC_RegistAuthAck.ErrorCode.Success : (ushort)LTC_RegistAuthAck.ErrorCode.DBError;

                loginSession.Send(ackPacket.Write());
                JobTimer.Instance.Push(() => loginSession.Disconnect(), 100);
            },
            (errorCode) =>
            {
                LTC_RegistAuthAck ackPacket = new LTC_RegistAuthAck();
                ackPacket.errorCode = errorCode;

                loginSession.Send(ackPacket.Write());
                JobTimer.Instance.Push(()=>loginSession.Disconnect(), 100);
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