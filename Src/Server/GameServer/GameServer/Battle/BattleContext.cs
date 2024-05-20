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
    public class BattleContext
    {
        public NSkillCastInfo CastSkill;
        public Creature Caster;
        public Creature Target;
        public Battle Battle;
        public NDamageInfo Damage;
        public SkillResult Result;
        public BattleContext(Battle battle)
        {
            Battle = battle;
        }
        
       
    }
}
