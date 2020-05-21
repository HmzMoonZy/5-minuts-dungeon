namespace BeginEndServer.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Sys
    {
        private static Sys instance;
        private Sys() { }
        public static Sys _Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Sys();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// 获取当前持剑戳(Utc)
        /// </summary>
        /// <returns></returns>
        public static long GetUtcTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 遍历列表中的心跳时间, 若超时则断开连接
        /// </summary>
        /// <param name="list"></param>
        /// <param name="timeOut"></param>
        public static void CheckHertBeat(List<Conn> list, long timeOut)
        {
            long nowTimeStamp = Sys.GetUtcTimeStamp();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].hertBeatTime + timeOut < nowTimeStamp)
                {
                    Console.WriteLine(list[i].GetRemoteAddress() + "心跳超时!");
                    lock (list[i])
                    {
                        list[i].Disconnect();
                    }
                }
            }
        }

    }


}
