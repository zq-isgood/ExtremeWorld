using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Battle
{
    public class Attributes
    {
        AttributeData Initial = new AttributeData();
        AttributeData Growth = new AttributeData();
        AttributeData Equip = new AttributeData();
        AttributeData Basic = new AttributeData();
        AttributeData Buff = new AttributeData();
        public AttributeData Final = new AttributeData();
        int Level;
        public NAttributeDynamic DynamicAttr;
        public float HP { get { return DynamicAttr.Hp; } set { DynamicAttr.Hp = (int)Math.Min(MaxHP, value); } }
        public float MP { get { return DynamicAttr.Mp; } set { DynamicAttr.Mp = (int)Math.Min(MaxMP, value); } }
             
        public float MaxHP { get { return this.Final.MaxHP; }  }
        /// <summary>
        /// 最大法力
        /// </summary>
        public float MaxMP { get { return this.Final.MaxMP; } }
        
        /// <summary>
        /// 力量
        /// </summary>
        public float STR { get { return this.Final.STR; } }
        
        /// <summary>
        /// 智力
        /// </summary>
        public float INT { get { return this.Final.INT; } }
        
        /// <summary>
        /// 敏捷
        /// </summary>
        public float DEX { get { return this.Final.DEX; } }
        
        /// <summary>
        /// 物理攻击
        /// </summary>
        public float AD { get { return this.Final.AD; } }
        
        /// <summary>
        /// 法术攻击
        /// </summary>
        public float AP { get { return this.Final.AP; } }
        
        /// <summary>
        /// 物理防御
        /// </summary>
        public float DEF { get { return this.Final.DEF; } }
        
        /// <summary>
        /// 法术防御
        /// </summary>
        public float MDEF { get { return this.Final.MDEF; } }
        
        /// <summary>
        /// 攻击速度
        /// </summary>
        public float SPD { get { return this.Final.SPD; } }
        
        /// <summary>
        /// 暴击概率
        /// </summary>
        public float CRI { get { return this.Final.CRI; } }
        public void Init(CharacterDefine define,int level ,List<EquipDefine> equips,NAttributeDynamic dynamicAttr)
        {
            this.DynamicAttr = dynamicAttr;
            this.LoadInitAttribue(this.Initial,define);
            this.LoadGrowthAttribue(this.Growth,define);
            this.LoadEquipAttributes(this.Equip,equips);
            this.Level = level;
            this.InitBasicAttributes();
            this.InitSecondaryAttributes();

            this.InitFinalAttributes();
            //如果是空，那么是新角色：怪物角色
            if (this.DynamicAttr == null)
            {
                this.DynamicAttr = new NAttributeDynamic();
                this.HP = MaxHP;
                this.MP = MaxMP;
            }
            else
            {
                //代表是玩家
                this.HP = DynamicAttr.Hp;
                this.MP = DynamicAttr.Mp;
            }
        }

        private void InitFinalAttributes()
        {
            for(int i=(int)AttributeType.MaxHP;i< (int)AttributeType.MAX; i++)
            {
                Final.Data[i] = Basic.Data[i] + Buff.Data[i];
            }
        }

        private void InitSecondaryAttributes()
        {
            Basic.MaxHP = Basic.STR * 10 + Initial.MaxHP + Equip.MaxHP;
            Basic.MaxMP = Basic.INT * 10 + Initial.MaxMP + Equip.MaxMP;
            Basic.AD = Basic.STR * 5 + Initial.AD + Equip.AD;
            Basic.AP= Basic.INT * 5 + Initial.AP + Equip.AP;
            Basic.DEF= Basic.STR * 2 + Basic.DEX*1+ Initial.DEF+ Equip.DEF;
            Basic.MDEF = Basic.INT* 2 + Basic.DEX * 1 + Initial.MDEF + Equip.MDEF;
            Basic.SPD = Basic.DEX * 0.2f + Initial.SPD + Equip.SPD;
            Basic.CRI = Basic.DEX * 0.0002f + Initial.CRI + Equip.CRI;
        }

        private void InitBasicAttributes()
        {
            for(int i = (int)AttributeType.MaxHP; i < (int)AttributeType.MAX; i++)
            {
                Basic.Data[i] = Initial.Data[i];
            }
            for(int i=(int)AttributeType.STR;i<= (int)AttributeType.DEX; i++)
            {
                Basic.Data[i] = Initial.Data[i] + Growth.Data[i] * (Level - 1);
                Basic.Data[i] += Equip.Data[i];
            }
        }

        private void LoadEquipAttributes(AttributeData attr, List<EquipDefine> equips)
        {
            attr.Reset();
            if (equips == null) return;
            //当装备发生变化，重新算一遍属性
            foreach(var define in equips)
            {
                attr.MaxHP += define.MaxHP;
                attr.MaxMP += define.MaxMP;
                attr.STR += define.STR;
                attr.INT += define.INT;
                attr.DEX += define.DEX;
                attr.AD += define.AD;
                attr.AP += define.AP;
                attr.DEF += define.DEF;
                attr.MDEF+= define.MDEF;
                attr.SPD += define.SPD;
                attr.CRI += define.CRI;

            }
        }

        private void LoadGrowthAttribue(AttributeData attr, CharacterDefine define)
        {
            //只成长三个属性
            attr.STR = define.GrowthSTR;
            attr.INT = define.GrowthINT;
            attr.DEX = define.GrowthDEX;
        }

        private void LoadInitAttribue(AttributeData attr, CharacterDefine define)
        {
            attr.MaxHP = define.MaxHP;
            attr.MaxMP = define.MaxMP;
            attr.STR = define.STR;
            attr.INT = define.INT;
            attr.DEX = define.DEX;
            attr.AD = define.AD;
            attr.AP = define.AP;
            attr.DEF = define.DEF;
            attr.MDEF = define.MDEF;
            attr.SPD = define.SPD;
            attr.CRI = define.CRI;
        }
    }
}
