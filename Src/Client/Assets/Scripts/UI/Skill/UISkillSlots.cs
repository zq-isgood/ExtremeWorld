using Assets.Scripts.Battle;
using Assets.Scripts.Managers;
using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillSlots :MonoBehaviour
{
    public UISkillSlot[] slots;
    private void Start()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        User.Instance.CurrentCharacter.SkillMgr = new SkillManager(User.Instance.CurrentCharacter);
        var Skills = User.Instance.CurrentCharacter.SkillMgr.Skills;
        int skillIdx = 0;
        foreach(var skill in Skills)
        {
            slots[skillIdx].SetSkill(skill);
            skillIdx++;
        }
    }
}
