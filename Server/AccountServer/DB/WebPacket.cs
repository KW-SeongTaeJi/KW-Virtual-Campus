using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CreateAccountPacketReq
{
    public string AccountId { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}
public class CreateAccountPacketRes
{
    public bool CreateAccountOk { get; set; }
    public int ErrorCode { get; set; }
}

public class LoginPacketReq
{
    public string AccountId { get; set; }
    public string Password { get; set; }
}
public class LoginPakcetRes
{
    public bool LoginOk { get; set; }
    public int ErrorCode { get; set; }
    public string AccountId { get; set; }
    public string Token { get; set; }
    public string Name { get; set; }

    public ServerInfo LobbyServer { get; set; }
    public ServerInfo GameServer1 { get; set; }
}

public class ServerInfo
{
    public string IpAddress { get; set; }
    public int Port { get; set; }
}