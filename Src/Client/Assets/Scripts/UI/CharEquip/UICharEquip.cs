using Assets.Scripts.Managers;
using Common.Battle;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : UIWindow
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI money;
    public GameObject itemPrefab;
    public GameObject itemEquipedPrefab;
    //是左边的装备列表的根节点
    public Transform itemListRoot;
    public List<Transform> slots;

    public TextMeshProUGUI hp;
    public Slider hpBar;
    public Slider mpBar;
    public TextMeshProUGUI mp;
    public TextMeshProUGUI[] attrs;

    void Start()
    {
        //启动时先刷新UI
        RefreshUI();
        //只要穿或脱装备了UI实时刷新
        EquipManager.Instance.OnEquipChanged += RefreshUI;
    }
    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;
    }

    /// <summary>
    /// 初始化所有装备列表
    /// </summary>
    /// <returns></returns>
    void RefreshUI()
    {
        //清空左边的装备列表
        ClearAllEquipList();
        //初始化左边的装备列表
        InitAllEquipItems();
        //清空右边的装备列表
        ClearEquipedList();
        //初始化右边的装备列表
        InitEquipedItems();
        this.money.text = User.Instance.CurrentCharacterInfo.Gold.ToString();
        InitAttributes();
    }

    private void InitAttributes()
    {
        var charattr = User.Instance.CurrentCharacter.Attributes;
        hp.text = string.Format("{0}/{1}",charattr.HP,charattr.MaxHP);
        mp.text = string.Format("{0}/{1}", charattr.MP, charattr.MaxMP);
        hpBar.maxValue = charattr.MaxHP;
        mpBar.maxValue = charattr.MaxMP;
        hpBar.value = charattr.HP;
        mpBar.value = charattr.MP;
        for(int i = (int)AttributeType.STR; i < (int)AttributeType.MAX; i++)
        {
            if (i == (int)AttributeType.CRI)
                attrs[i - 2].text = String.Format("{0:f2}%", charattr.Final.Data[i] * 100);
            else
                attrs[i - 2].text = ((int)charattr.Final.Data[i]).ToString();
        }
    }

    void InitAllEquipItems()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == ItemType.Equip && kv.Value.Define.LimitClass == User.Instance.CurrentCharacterInfo.Class)
            {
                if (EquipManager.Instance.Contains(kv.Key))
                    continue;
                GameObject go = Instantiate(itemPrefab, itemListRoot);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                //布尔值区分是哪个装备列表
                ui.SetEquipItem(kv.Key, kv.Value, this, false);

            }
        }
    }
    void ClearAllEquipList()
    {
        foreach(var item in itemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
            Destroy(item.gameObject);
        }
    }
    void ClearEquipedList()
    {
        foreach(var item in slots)
        {
            if (item.childCount > 0)
                Destroy(item.GetChild(0).gameObject);
        }
    }

    void InitEquipedItems()
    {
        for(int i = 0; i < (int)EquipSlot.SlotMax; i++)
        {
            var item = EquipManager.Instance.Equips[i];
            {
                if(item!=null)
                {
                    GameObject go = Instantiate(itemEquipedPrefab, slots[i]);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(i, item, this, true);
                }
            }
        }
    }
    public void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }
    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }
}