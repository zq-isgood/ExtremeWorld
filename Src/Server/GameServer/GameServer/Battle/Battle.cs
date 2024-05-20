using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    public class Battle
    {
        public Map Map;
        Dictionary<int, Creature> AllUnits = new Dictionary<int, Creature>();
        Queue<NSkillCastInfo> Actions = new Queue<NSkillCastInfo>();
        List<Creature> DeahPool = new List<Creature>();
        public Battle(Map map)
        {
            this.Map = map;
        }
        internal void ProcessBattleMessage(NetConnection<NetSession> sender, SkillCastRequest request)
        {
            Character character = sender.Session.Character;
            if (request.castInfo != null)
            {
                if (character.entityId != request.castInfo.casterId) return;
                Actions.Enqueue(request.castInfo);
            }
        }
        public void Update()
        {
            if (Actions.Count > 0)
            {
                NSkillCastInfo skillCast = Actions.Dequeue();
                ExecuteAction(skillCast);
            }
            UpdateUnits();
        }

        private void ExecuteAction(NSkillCastInfo cast)
        {
            BattleContext context=new BattleContext(this);
            context.Caster = EntityManager.Instance.GetCreature(cast.casterId);
            context.Target = EntityManager.Instance.GetCreature(cast.targetId);
            context.CastSkill = cast;
            if (context.Caster != null)
            {
                JoinBattle(context.Caster);
            }
            if (context.Target != null)
            {
                JoinBattle(context.Target);
            }
            context.Caster.CastSkill(context, cast.skillId);
            NetMessageResponse message = new NetMessageResponse();
            message.skillCast = new SkillCastResponse();
            message.skillCast.castInfo = context.CastSkill;
            message.skillCast.Damage= context.Damage;
            message.skillCast.Result = context.Result == SkillResult.Ok ? Result.Success : Result.Failed;
            message.skillCast.Errormsg = context.Result.ToString();
            Map.BroadcastBattleResponse(message);
        }

        private void UpdateUnits()
        {
            DeahPool.Clear();
            foreach(var kv in AllUnits)
            {
                kv.Value.Update();
                if (kv.Value.IsDeath)
                {
                    DeahPool.Add(kv.Value);
                }
            }
            foreach(var unit in DeahPool)
            {
                LeaveBattle(unit);
            }
        }
        public void JoinBattle(Creature unit)
        {
            AllUnits[unit.entityId] = unit;
        }
        public void LeaveBattle(Creature unit)
        {
            AllUnits.Remove(unit.entityId);
        }
    }
}
