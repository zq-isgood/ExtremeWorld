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
                MessageBox.Show("����" + this.skill.Define.Name + "Ŀ����Ч");
                return;
            case SkillResult.OutOfMp:
                MessageBox.Show("����" + this.skill.Define.Name + "MP����");
                return;
            case SkillResult.CoolDown:
                MessageBox.Show("����" + this.skill.Define.Name + "������ȴ");
                return;
            case SkillResult.OutOfRange:
                MessageBox.Show("����" + this.skill.Define.Name + "Ŀ�곬���ͷŷ�Χ");
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
