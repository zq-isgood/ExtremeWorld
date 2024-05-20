using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Battle;
using Assets.Scripts.Managers;
using Common.Battle;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

namespace Entities
{
    public class Creature : Entity
    {
        public NCharacterInfo Info;

        public Common.Data.CharacterDefine Define;
        public Attributes Attributes;
        public SkillManager SkillMgr;
        public bool battleState =false;
        public bool BattleState
        {
            get { return battleState; }
            set
            {
                if (battleState != value)
                {
                    battleState = value;
                    SetStandby(value);
                }
            }
        }
        public Skill CastringSkill = null;
        public int Id { get { return this.Info.Id; } }

        public string Name
        {
            get
            {
                if (this.Info.Type == CharacterType.Player)
                    return this.Info.Name;
                else
                    return this.Define.Name;
            }
        }

        public bool IsPlayer
        {
            
            get { return this.Info.Type == CharacterType.Player; } //判断是不是玩家本人
        }
        public bool IsCurrentPlayer
        {
            get
            {
                if (!IsPlayer) return false;
                return this.Info.Id == Models.User.Instance.CurrentCharacterInfo.Id;
            }
        }

        public Creature(NCharacterInfo info) : base(info.Entity)
        {
            this.Info = info;
            this.Define = DataManager.Instance.Characters[info.ConfigId];
            //角色创建时对属性初始化
            Attributes = new Attributes();
            Attributes.Init(Define, this.Info.Level, GetEquips(), this.Info.attrDynamic);

        }
        public virtual List<EquipDefine> GetEquips()
        {
            return null;
        }
        public void MoveForward()
        {
            Debug.LogFormat("MoveForward");
            this.speed = this.Define.Speed;
        }

        public void MoveBack()
        {
            Debug.LogFormat("MoveBack");
            this.speed = -this.Define.Speed;
        }

        public void Stop()
        {
            Debug.LogFormat("Stop");
            this.speed = 0;
        }

        public void SetDirection(Vector3Int direction)
        {
            Debug.LogFormat("SetDirection:{0}", direction);
            this.direction = direction;
        }

        public void SetPosition(Vector3Int position)
        {
            Debug.LogFormat("SetPosition:{0}", position);
            this.position = position;
        }
        public void CastSkill(int skillId,Creature target,NVector3 pos,NDamageInfo damage)
        {
            //设置为战斗状态
            SetStandby(true);
            var skill = this.SkillMgr.GetSkill(skillId);
            skill.BeginCast(damage);
        }
        public void SetStandby(bool standby)
        {
            if (this.Controller!= null)
            {
                this.Controller.SetStandby(standby); //本类的setstandby其实是调用的Ientitycontroller的
            }
        }
        public override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
            if (SkillMgr != null)
            {
                SkillMgr.OnUpdate(delta);
            }
            
        }
        public void PlayAnim(string name)
        {
            if (this.Controller != null)
            {
                this.Controller.PlayAnim(name);
            }
        }
        public void UpdateInfo(NCharacterInfo info)
        {
            SetEntityData(info.Entity);
            Info = info;
            Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic); //更新属性
            SkillMgr.UpdateSkills(); //更新技能
        }
        public void DoDamage(NDamageInfo damage)
        {
            Debug.LogFormat("DoDamage:{0}", damage);
            Attributes.HP -= damage.Damage;
            PlayAnim("Hurt");
        }
    }
}
