using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    public class Skill
    {
        public NSkillInfo Info;
        public Creature Owner;
        public SkillDefine Define;
        public float cd = 0;
        public float CD
        {
            get { return cd; }
        }
        public Skill(NSkillInfo info, Creature owner)
        {
            Info = info;
            Owner = owner;
            Define = DataManager.Instance.Skills[(int)this.Owner.Define.Class][this.Info.Id];
        }

        internal SkillResult Cast(BattleContext context)
        {
            SkillResult result = SkillResult.Ok;
            if (cd > 0)
            {
                return SkillResult.CoolDown;
            }
            if (context.Target != null)
            {
                DoSkillDamage(context);
            }
            else
            {
                result = SkillResult.InvalidTarget;
            }
            cd = Define.CD;
            return result;
        }

        private void DoSkillDamage(BattleContext context)
        {
            context.Damage = new NDamageInfo();
            context.Damage.entityId = context.Target.entityId;
            context.Damage.Damage = 100;
            context.Target.DoDamage(context.Damage);
        }

        internal void Update()
        {
            UpdateCD();
        }
        private void UpdateCD()
        {
            if (this.cd > 0) { this.cd -= Time.deltaTime; }
            if (cd < 0) { this.cd = 0; }
        }
    }
}
