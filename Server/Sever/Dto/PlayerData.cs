using BeginEndServer.Dto;
using System;

namespace Server.Dto
{
    /// <summary>
    /// 玩家虚拟角色数据
    /// </summary>
    [Serializable]
    public class PlayerData : PlayerDataBase
    {
        public string nickName;
        
        public int coin = 0;

        public int win = 0;
        public int loss = 0;
        public PlayerData()
        {
            nickName = "nullName";
            coin = 100;
            
        }

        public PlayerData(string name, int coin)
        {
            this.nickName = name;
            this.coin = coin;
        }

        public PlayerDataProtocol ToProtocol()
        {
            PlayerDataProtocol pdp = new PlayerDataProtocol();

            pdp.nickName = nickName;
            pdp.coin = coin;
            pdp.win = this.win;
            pdp.loss = this.loss;
            return pdp;
        }
    }
}
