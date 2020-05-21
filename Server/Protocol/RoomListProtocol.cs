
using System;
using System.Collections.Generic;

[Serializable]
public class RoomListProtocol
{
    public List<RoomProtocal>list = new List<RoomProtocal>();
    
}

[Serializable]
public class RoomProtocal
{
    public string roomName;

    public int maxNumber;

    public int current;

    public int roomid;

    public int status;  //0:等待 1:开始
}

