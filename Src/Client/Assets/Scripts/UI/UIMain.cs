using Assets.Scripts.Manager;
using Entities;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    public TextMeshProUGUI avatarName;
    public TextMeshProUGUI avatarLevel;
    public UITeam TeamWindow;
    public UICreatureInfo targetUI;

    protected override void OnStart()
    {
        this.UpdateAvatar();
        targetUI.gameObject.SetActive(false);
        BattleManager.Instance.OnTargetChanged += OnTargetChanged;  //两个OnTargetChanged其实是调用这个
    }

    private void OnTargetChanged(Creature target)
    {
        //将当前的目标设置给UI,当前的目标是在BattleManager的SetTarget设置的
        if (target != null)
        {
            if (!targetUI.isActiveAndEnabled) targetUI.gameObject.SetActive(true);
            targetUI.Target = target;
        }
        else
        {
            targetUI.gameObject.SetActive(false);
        }
    }

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacterInfo.Name, User.Instance.CurrentCharacterInfo.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacterInfo.Level.ToString();
    }
 

    public void OnClickTest()
    {
        UITest test=UIManager.Instance.Show<UITest>();
        test.SetTitle("这是一个测试UI");
        test.OnClose += Test_OnClose;
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了对话框的： " + result, "对话框响应结果", MessageBoxType.Information);
    }
    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }
    public void OnClickSkill()
    {
        UIManager.Instance.Show<UISkill>();
    }
    public void OnClickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }
    public void OnClickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }
    public void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriends>();
    }
    public void ShowTeamUI(bool show)
    {
       //被TeamManager调用
        TeamWindow.ShowTeam(show);
    }
    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }
    public void OnClickRide()
    {
        UIManager.Instance.Show<UIRide>();
    }
    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();  //点击按钮加载界面的命令
    }



}
