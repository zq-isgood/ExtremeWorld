using Assets.Scripts.Manager;
using Common.Battle;
using Common.Data;
using Entities;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class Skill
    {
        public NSkillInfo Info;
        public Creature Owner;
        public SkillDefine Define;
        public float cd = 0;
        public NDamageInfo Damage;
        public float skillTime;
        public int hit;
        public float CD
        {
            get { return cd; }
        }
        public bool IsCasting = false;
        public float castTime = 0;
        public Skill(NSkillInfo info,Creature owner)
        {
            Info = info;
            Owner = owner;
            Define = DataManager.Instance.Skills[(int)this.Owner.Define.Class][this.Info.Id];
        }

        public SkillResult CanCast(Creature target)
        {
            if (Define.CastTarget == TargetType.Target)
            {
                if (target == null || target == this.Owner)
                    return SkillResult.InvalidTarget;
                int distance = (int)Vector3Int.Distance(this.Owner.position, target.position);
                if (distance > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }

            if (Define.CastTarget == TargetType.Position && BattleManager.Instance.currentPosition == null)
            {
                return SkillResult.InvalidTarget;
            }
            if (Owner.Attributes.MP<this.Define.MPCost)
            {
                return SkillResult.OutOfMp;
            }
            if (cd > 0)
            {
                return SkillResult.CoolDown;
            }
            return SkillResult.Ok;
        }
        public void OnUpdate(float delta)
        {
            if (this.IsCasting)
            {
                skillTime += delta;
                if (skillTime > 0.5f && hit == 0)
                {
                    DoHit();
                }
                if (skillTime >=Define.CD)
                {
                    skillTime = 0;
                }
            }
            UpdateCD(delta);
        }

        private void DoHit()
        {
            if (Damage != null)
            {
                var cha = CharacterManager.Instance.GetCharacter(this.Damage.entityId);
                cha.DoDamage(Damage);
            }
            hit++;
        }

        private void UpdateCD(float delta)
        {
            if (this.cd > 0) { this.cd -= delta; }
            if (cd < 0) { this.cd = 0; }
        }
        public void BeginCast(NDamageInfo damage)
        {
            IsCasting = true;
            this.castTime= 0;
            this.skillTime = 0;
            cd= Define.CD;
            this.Damage = damage;
            Owner.PlayAnim(Define.SkillAnim);

        }
    }
}
