using Assets.Scripts.Managers;
using Common.Data;
using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities
{
    public class Character : Creature
    {
        public Character(NCharacterInfo info) : base(info)
        {
        }
        public override List<EquipDefine> GetEquips()
        {
            return EquipManager.Instance.GetEquipedDefines();
        }
    }
}
