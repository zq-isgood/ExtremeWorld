using Assets.Scripts.Entities;
using Common.Data;
using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }

 
        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        //从网络传回的信息
        public SkillBridge.Message.NCharacterInfo CurrentCharacterInfo { get; set; }
        public MapDefine CurrentMapData { get; set; }
        //角色控制器
        public PlayerInputController CurrentCharacterObject { get; set; }
        public Character CurrentCharacter { get; set; }

        public NTeamInfo TeamInfo { get; set; }
        public void AddGold(int gold)
        {
            this.CurrentCharacterInfo.Gold += gold;
        }
        //直接用User.instance.ride传坐骑的信息
        public int CurrentRide = 0; //当前的ID
        internal void Ride(int id)
        {
            if (CurrentRide != id)
            {
                CurrentRide = id;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, CurrentRide);
            }
            else
            {
                CurrentRide = 0;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride,0);
            }
        }
    }
}
