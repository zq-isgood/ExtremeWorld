using Assets.Scripts.Managers;
using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;

public class UISkill : UIWindow
{
    public TextMeshProUGUI decript;
    public ListView listMain;
    public GameObject itemPrefab;  //œ‘ æµƒÕº±Í
    public UISkillItem selectedItem;

    void Start()
    {
        RefreshUI();
        listMain.onItemSelected += this.OnItemSelected;
    }

    private void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    private void ClearItems()
    {
        listMain.RemoveAll();
    }

    private void InitItems()
    {
        var Skills = User.Instance.CurrentCharacter.SkillMgr.Skills;
        foreach(var skill in Skills)
        {           
            if (skill.Define.Type==SkillType.Skill)
            {
                GameObject go = Instantiate(itemPrefab, listMain.transform);
                UISkillItem ui = go.GetComponent<UISkillItem>();
                ui.SetItem(skill, this, false);
                listMain.AddItem(ui);
            }
        }
    }

    private void OnItemSelected(ListView.ListViewItem item)
    {
        selectedItem = item as UISkillItem;
        decript.text = selectedItem.item.Define.Description;
    }

    private void OnDestroy()
    {
     
    }
  
}

