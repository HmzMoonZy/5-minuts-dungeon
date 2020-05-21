using BeginEndServer;
using Server.Dot;
using Server.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Logic
{

    public class AccountHandler : IMessageHandler
    {
        private static AccountHandler instance;
        public static AccountHandler _Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountHandler();
                }
                return instance;
            }
        }

        public void ProcessData(Conn conn, int subCode, object parm)
        {
            switch (subCode)
            {
                //用户注册
                case AccountSubCode.SignUp:
                    processSignUp(conn, parm);
                    break;

                //用户登录
                case AccountSubCode.LogIn:
                    processLogIn(conn, parm);
                    break;

                //用户登出
                case AccountSubCode.LogOut:
                    processLogOut(conn, parm);
                    break;
            }
        }

        /// <summary>
        /// 处理登出请求
        /// </summary>
        private void processLogOut(Conn conn, object parm)
        {
            PlayerDataProtocol pdp = parm as PlayerDataProtocol;
            PlayerData pd = new PlayerData(pdp.nickName, pdp.coin);
            Player player = conn.player as Player;
            player.playerdata = pd;
            player.LogOut();
        }

        /// <summary>
        /// 处理注册请求
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="parm"></param>
        private static void processSignUp(Conn conn, object parm)
        {
            AccountInfoProtocol info = parm as AccountInfoProtocol;
            int result = DatabaseMgr._Instance.Register(info.Username, info.Password);
            conn.Send(OpCode.ACCOUNT, AccountSubCode.SignUp, result);

            if (result == 1)
            {
                Console.WriteLine("注册成功");
            }

        }

        /// <summary>
        /// 处理登录请求, 并给客户端发送结果
        /// -1: 用户名密码错误
        /// 0: 用户无角色数据
        /// 1: 登陆成功
        /// </summary>
        private static void processLogIn(Conn conn, object parm)
        {
            AccountInfoProtocol ai = parm as AccountInfoProtocol;
            bool loginResult = DatabaseMgr._Instance.LoginCheck(ai.Username, ai.Password);

            //用户名密码正确
            if (loginResult)    
            {
                //TODO 是否已经登陆(踢下线)
                //for (int i = 0; i < Program.server.usingConns.Count; i++)
                //{
                //    if (Program.server.usingConns[i].player != null && Program.server.usingConns[i].player.username == ai.Username)
                //    {
                //        Player player = Program.server.usingConns[i].player as Player;
                //        player.LogOut();
                //    }
                //}

                conn.Send(OpCode.ACCOUNT, AccountSubCode.LogIn, 1);
                conn.player = new Player(ai.Username, conn);
            }
            else
            {
                conn.Send(OpCode.ACCOUNT, AccountSubCode.LogIn, -1);
            }
        }
    }
}
