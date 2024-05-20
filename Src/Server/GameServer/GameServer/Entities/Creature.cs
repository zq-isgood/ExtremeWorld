using Common.Battle;
using Common.Data;
using GameServer.Battle;
using GameServer.Core;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
     public class Creature : Entity
    {

        public int Id { get; set; }

        public string Name { get { return this.Info.Name; } }
        public NCharacterInfo Info;
        public CharacterDefine Define;
        public SkillManager SkillMgr;
        public Attributes Attributes;
        public bool IsDeath = false;

        public Creature(CharacterType type, int configId, int level, Vector3Int pos, Vector3Int dir) :
           base(pos, dir)
        {
            this.Define = DataManager.Instance.Characters[configId];
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Level = level;
            this.Info.ConfigId = configId;
            this.Info.Entity = this.EntityData;
            this.Info.EntityId = this.entityId;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];
            this.Info.Name = this.Define.Name;
            InitSkills();
            this.Attributes = new Attributes();
            Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic);
            this.Info.attrDynamic = this.Attributes.DynamicAttr;  //同步
        }

        internal void DoDamage(NDamageInfo damage)
        {
            Attributes.HP -= damage.Damage;
            if (this.Attributes.HP < 0)
            {
                IsDeath = true;
                damage.WillDead = true;
            }
        }

        private void InitSkills()
        {
            SkillMgr = new SkillManager(this);
            this.Info.Skills.AddRange(this.SkillMgr.Infos);
        }
        public virtual List<EquipDefine> GetEquips()
        {
            return null;
        }

        internal void CastSkill(BattleContext context, int skillId)
        {
            Skill skill = SkillMgr.GetSkill(skillId);
            context.Result = skill.Cast(context); //找到技能，释放技能
        }
        public override void Update()
        {
            SkillMgr.Update();
        }
    }
}
