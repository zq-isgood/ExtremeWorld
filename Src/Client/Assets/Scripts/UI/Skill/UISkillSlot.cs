using Assets.Scripts.Battle;
using Assets.Scripts.Manager;
using Assets.Scripts.Managers;
using Common.Battle;
using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillSlot :MonoBehaviour,IPointerClickHandler
{
    public Image icon;
    public Image overlay;
    public TextMeshProUGUI cdText;
    public Skill skill;

    private void Start()
    {
        overlay.enabled = false;
        cdText.enabled = false;
    }

    private void Update()
    {
        if (this.skill.CD > 0)
        {
            if (!overlay.enabled) overlay.enabled = true;
            if (!cdText.enabled) cdText.enabled = true;
            overlay.fillAmount = skill.CD / skill.Define.CD;
            cdText.text = ((int)Math.Ceiling(skill.CD)).ToString();
        }     
        else
        {
            if (overlay.enabled) overlay.enabled = false;
            if (this.cdText.enabled) cdText.enabled = false;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        SkillResult result=skill.CanCast(BattleManager.Instance.currentTarget);
        switch (result)
        {
            case SkillResult.InvalidTarget:
                MessageBox.Show("技能" + this.skill.Define.Name + "目标无效");
                return;
            case SkillResult.OutOfMp:
                MessageBox.Show("技能" + this.skill.Define.Name + "MP不足");
                return;
            case SkillResult.CoolDown:
                MessageBox.Show("技能" + this.skill.Define.Name + "正在冷却");
                return;
            case SkillResult.OutOfRange:
                MessageBox.Show("技能" + this.skill.Define.Name + "目标超出释放范围");
                return;

        }
        BattleManager.Instance.CastSkill(this.skill);

    }
    internal void SetSkill(Skill value)
    {
        skill = value;
        if (icon != null) icon.overrideSprite = Resloader.Load<Sprite>(skill.Define.Icon);
        
    }
}
