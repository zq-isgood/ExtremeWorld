using Assets.Scripts.Battle;
using Entities;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager
{
    class BattleManager:Singleton<BattleManager>
    {
        public delegate void TargetChangedHandler(Creature target);
        public event TargetChangedHandler OnTargetChanged;
        public Creature currentTarget;
        public Creature CurrentTarget
        { get { return currentTarget; }
            set { this.SetTarget(value); } }



        public NVector3 currentPosition;
        public NVector3 CurrentPosition 
        { get { return currentPosition; }
            set{ this.SetPosition(value); }
        }
        void Init() { }
        private void SetTarget(Creature target)
        {
            if (currentTarget != target && OnTargetChanged != null)
            {
                OnTargetChanged(target);
            }
            currentTarget = target;
        }

        private void SetPosition(NVector3 value)
        {
            currentPosition = value;
        }
        public void CastSkill(Skill skill)
        {
            int target = currentTarget != null ? currentTarget.entityId : 0;
            BattleService.Instance.SendSkillCast(skill.Define.ID, skill.Owner.entityId, target, currentPosition);
        }
    }
}
