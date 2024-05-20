using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    public class BattleService:Singleton<BattleService>
    {
        public BattleService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillCastRequest>(this.OnSkillCast);
        }



        public void Init()
        {
           
        }
        private void OnSkillCast(NetConnection<NetSession> sender, SkillCastRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnSkillCast");
            //sender.Session.Response.skillCast = new SkillCastResponse();
            //sender.Session.Response.skillCast.Result = Result.Success;
            //sender.Session.Response.skillCast.castInfo = request.castInfo;
            //MapManager.Instance[character.Info.mapId].BroadcastBattleResponse(sender.Session.Response);
            BattleManager.Instance.ProcessBattleMessage(sender, request);
        }
     
    }
}
