using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAccountPacketReq
{
    public string AccountId;
    public string Password;
}
public class CreateAccountPacketRes
{
    public bool CreateAccountOk;
}

public class LoginPacketReq
{
    public string AccountId;
    public string Password;
}
public class LoginPakcetRes
{
    public bool LoginOk;
    public string AccountId;
    public string Token;
    public string Name;

    public ChannelInfo Channel;
}

public class ChannelInfo
{
    public string IpAddress;
    public int Port;
}