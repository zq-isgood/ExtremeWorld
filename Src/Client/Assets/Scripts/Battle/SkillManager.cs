using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Battle
{
    public class SkillManager
    {
        Creature Owner;
        public delegate void SkillInfoUpdateHandle();
        public event SkillInfoUpdateHandle OnSkillInfoUpdate;
        public List<Skill> Skills { get; private set; }
        public SkillManager(Creature Owner)
        {
            this.Owner = Owner;
            Skills = new List<Skill>();
            InitSkills();
        }
        private void InitSkills()
        {
            Skills.Clear();
            foreach(var skillInfo in this.Owner.Info.Skills)
            {
                Skill skill = new Skill(skillInfo, this.Owner);
                AddSkill(skill);
            }
            if (OnSkillInfoUpdate != null)
            {
                OnSkillInfoUpdate();
            }
        }
        public void UpdateSkills()
        {
            foreach(var skillInfo in this.Owner.Info.Skills)
            {
                Skill skill = this.GetSkill(skillInfo.Id);
                if (skill != null)
                {
                    skill.Info = skillInfo; //更新skill
                }
                else
                {
                    AddSkill(skill);
                }
            }
            if (OnSkillInfoUpdate != null)
            {
                OnSkillInfoUpdate();
            }
        }

        private void AddSkill(Skill skill)
        {
            this.Skills.Add(skill);
        }
        public Skill GetSkill(int skillId)
        {
            for(int i = 0; i < Skills.Count; i++)
            {
                if (Skills[i].Define.ID == skillId)
                {
                    return Skills[i];
                }
            }
            return null;
        }
        public void OnUpdate(float delta)
        {
            for(int i = 0; i < Skills.Count; i++)
            {
                Skills[i].OnUpdate(delta);
            }
        }
    }
}
