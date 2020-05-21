using System;
[Serializable]
public class PlayerDataProtocol
{
    public string nickName;

    public int coin = 0;

    public int win = 0;
    public int loss = 0;

    public PlayerDataProtocol()
    {
        nickName = "";
        coin = 100;
    }

    public PlayerDataProtocol(string name, int coin)
    {
        this.nickName = name;
        this.coin = coin;
    }

    public PlayerDataProtocol(string name, int coin, int win, int loss)
    {
        this.nickName = name;
        this.coin = coin;
        this.win = win;
        this.loss = loss;
    }
}
