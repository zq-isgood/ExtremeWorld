using Assets.Scripts.Battle;
using Assets.Scripts.Managers;
using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillItem : ListView.ListViewItem
{
    public Image icon;
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        background.overrideSprite = selected ?selectedBg : normalBg;
    }

    public Skill item;

    public void SetItem(Skill item,  UISkill owner, bool equiped)
    {
        
        this.item = item;
        if(this.title!=null) this.title.text = this.item.Define.Name;
        if (this.level != null) this.level.text = item.Define.UnlockLevel.ToString();    
        if (this.icon!= null) this.icon.overrideSprite = Resloader.Load<Sprite>(item.Define.Icon);
    }

}
