using BeginEndServer;

namespace Server.Logic
{
    interface IMessageHandler
    {
        void  ProcessData(Conn conn, int subCode, object parm);
    }
}
