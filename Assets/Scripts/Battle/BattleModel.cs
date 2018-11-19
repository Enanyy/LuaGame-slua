using SUMessage;
using System;
using System.Collections.Generic;

public class BattleModel : Model<BattleModel>
{
    public override void RegisterReceiver()
    {  
        MessageDispatch.instance.RegisterReceiver(CLT_MSGID.CltGBattleWitResp, OnBattleWitRespone);
        MessageDispatch.instance.RegisterReceiver(CLT_MSGID.CltGBattleStatus, OnBattleStatus);
        MessageDispatch.instance.RegisterReceiver(CLT_MSGID.CltGBattleEnd, OnBattleEnd);
    }

    public override void UnRegisterReceiver()
    {
        MessageDispatch.instance.UnRegisterReceiver(CLT_MSGID.CltGBattleWitResp, OnBattleWitRespone);
        MessageDispatch.instance.UnRegisterReceiver(CLT_MSGID.CltGBattleStatus, OnBattleStatus);
        MessageDispatch.instance.UnRegisterReceiver(CLT_MSGID.CltGBattleEnd, OnBattleEnd);
    }
    
    private void OnBattleWitRespone(NetPacket packet)
    {
        Clt_G_BattleWitResp data = NetCode.Decode(Clt_G_BattleWitResp.Parser, packet);

    }
    private void OnBattleStatus(NetPacket packet)
    {
        Clt_G_BattleStatus data = NetCode.Decode(Clt_G_BattleStatus.Parser, packet);

    }

    private void OnBattleEnd(NetPacket packet)
    {
        Clt_G_BattleEnd data = NetCode.Decode(Clt_G_BattleEnd.Parser, packet);
    }
}

