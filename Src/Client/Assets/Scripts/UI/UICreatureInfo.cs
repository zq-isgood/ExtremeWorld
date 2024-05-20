using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICreatureInfo : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public Slider HPBar;
    public Slider MPBar;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI MPText;
    void Start()
    {
        
    }
    public Creature target;
    public Creature Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
            UpdateUI();
        }
    }
    // Update is called once per frame
    void Update()
    {
        //√ø÷°∂º≤È—Ø
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (target == null) return;
        Name.text = string.Format("{0} Lv.{1}",target.Name,target.Info.Level);
        HPBar.maxValue = target.Attributes.MaxHP;
        HPBar.value = target.Attributes.HP;
        HPText.text= string.Format("{0}/{1}", target.Attributes.HP, target.Attributes.MaxHP);
        MPBar.maxValue = target.Attributes.MaxMP;
        MPBar.value = target.Attributes.MP;
        MPText.text = string.Format("{0}/{1}", target.Attributes.MP, target.Attributes.MaxMP);
    }
}
