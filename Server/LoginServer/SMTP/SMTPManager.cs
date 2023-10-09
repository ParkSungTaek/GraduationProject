/******
작성자 : 이우열
작성 일자 : 23.10.03

최근 수정 일자 : 23.10.03
최근 수정 내용 : 메일 인증 관리자 클래스 생성
 ******/

using ServerCore;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace LoginServer.SMTP
{
    class AuthInfo
    {
        public string encryptedPW;
        public int jobId;
        public int authNo;
    }

    class SMTPManager
    {
        const string senderEmail = "teamphoton@naver.com";
        private System.Net.NetworkCredential credential;

        /// <summary> 이메일 보내고 기다리는 애들 </summary>
        private Dictionary<string, AuthInfo> _authWaiters = new Dictionary<string, AuthInfo>();
        public static SMTPManager Instance { get; } = new SMTPManager();
        private SMTPManager()
        {
            credential = new System.Net.NetworkCredential(senderEmail, "U8Q3QSQ299SV");
        }

        private object _lock = new object();

        /// <summary> 신규 회원가입 등록 </summary>
        public bool Regist(string email, string password)
        {
            AuthInfo authInfo = new AuthInfo { encryptedPW = password };
            System.Random rand = new System.Random();
            authInfo.authNo = rand.Next(100000, 999999);

            try
            {
                using (MailMessage mail = CreateMail(email, authInfo.authNo))
                {
                    SendMail(mail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"mail error : {ex}");
                return false;
            }

            lock (_lock)
            {
                AuthInfo prevInfo;
                if(_authWaiters.TryGetValue(email, out prevInfo))
                {
                    _authWaiters.Remove(email);
                    RemovableJobTimer.Instance.Remove(prevInfo.jobId);
                }

                int jobId = RemovableJobTimer.Instance.Push(() =>
                        {
                            lock (_lock)
                            {
                                _authWaiters.Remove(email);
                            }
                        }, 300000);

                authInfo.jobId = jobId;
                _authWaiters.Add(email, authInfo);
            }

            return true;
        }

        /// <summary> 클라에서 보낸 인증번호 처리 </summary>
        public void Auth(string email, int authNo, Action<bool> endCallback, Action<ushort> authErrorCallback)
        {
            AuthInfo authInfo;

            if (_authWaiters.TryGetValue(email, out authInfo))
            {
                if (authInfo.authNo == authNo)
                {
                    DB.DBManager.Instance.CreateUser(email, authInfo.encryptedPW, endCallback);
                    RemovableJobTimer.Instance.Remove(authInfo.jobId);
                    lock (_lock)
                    {
                        _authWaiters.Remove(email);
                    }

                    return;
                }
                else
                {
                    authErrorCallback.Invoke((ushort)Packet.LTC_RegistAuthAck.ErrorCode.WrongCode);
                    return;
                }
            }

            authErrorCallback.Invoke((ushort)Packet.LTC_RegistAuthAck.ErrorCode.Expired);
        }

        private MailMessage CreateMail(string email, int authNo)
        {
            MailMessage mail = new MailMessage { From = new MailAddress(senderEmail), 
                                                 Subject = "Castle Guardians 인증 코드", 
                                                 Body = $"인증 코드는 {authNo} 입니다.",
                                                 IsBodyHtml = true,
                                                 Priority = MailPriority.Normal,
                                                 DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                                                 SubjectEncoding = System.Text.Encoding.UTF8,
                                                 BodyEncoding = System.Text.Encoding.UTF8
            };

            mail.To.Add(email);

            return mail;
        }
        private void SendMail(MailMessage mail)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.naver.com", 587);
            smtpClient.Timeout = 10000;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = credential;

            smtpClient.Send(mail);
        }
    }
}
