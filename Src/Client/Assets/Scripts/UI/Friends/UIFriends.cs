using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using System;
using Models;
using Assets.Scripts.Managers;

public class UIFriends: UIWindow
{
    public GameObject itemPrefab; //要绑定的列表元素
    public ListView listMain; //列表框
    public Transform itemRoot;
    public UIFriendItem selectedItem;
    private void Start()
    {
        FriendService.Instance.OnFriendUpdate = RefreshUI;
        this.listMain.onItemSelected += this.OnFriendSelected; //根据这个onItemSelected知道当前选中了哪一个
        RefreshUI();
    }

    public void OnFriendSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIFriendItem;
    }
    public void OnClickFriendAdd()
    {
        InputBox.Show("输入要添加的好友名称或ID", "添加好友").OnSubmit += OnFriendAddSubmit; //如果选择提交则执行OnFriendAddSubmit这个逻辑
    }

    private bool OnFriendAddSubmit(string input, out string tips)
    {
        tips = "";
        int friendId = 0;
        string friendName = "";
        //用于检验输入的是ID还是名字，是名字就换为ID
        if (!int.TryParse(input, out friendId))
            friendName = input;
        if (friendId == User.Instance.CurrentCharacterInfo.Id || friendName == User.Instance.CurrentCharacterInfo.Name)
        {
            tips = "开玩笑嘛？不能添加自己哦";
            return false;
        }
        FriendService.Instance.SendFriendAddRequest(friendId, friendName);
        return true;
    }
    public void OnClickFriendChat()
    {
        MessageBox.Show("暂未开放");
    }
    public void OnClickTeamInvite()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要组队的好友");
            return;
        }
        if (selectedItem.Info.Status == 0)
        {
            MessageBox.Show("请选择在线的好友");
            return;
        }
        MessageBox.Show(String.Format("你确定邀请【{0}】组队吗？", selectedItem.Info.friendInfo.Name), "邀请好友组队", MessageBoxType.Confirm, "邀请", "取消").OnYes = () =>
           { TeamService.Instance.SendTeamInviteRequest(this.selectedItem.Info.friendInfo.Id, this.selectedItem.Info.friendInfo.Name); };
    }
    public void OnClickFriendRemove()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要删除的好友");
            return;
        }
        MessageBox.Show(string.Format("确定要删除好友【{0}】吗？", selectedItem.Info.friendInfo.Name), "删除好友", MessageBoxType.Confirm, "删除", "取消").OnYes = () =>
        {
            FriendService.Instance.SendFriendRemoveRequest(this.selectedItem.Info.Id, this.selectedItem.Info.friendInfo.Id);
        };

    }
    void RefreshUI()
    {
        ClearFriendList();
        InitFriendItems();
    }

    private void InitFriendItems()
    {
        foreach(var item in FriendManager.Instance.allFriends)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIFriendItem ui = go.GetComponent<UIFriendItem>();
            ui.SetFriendInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    private void ClearFriendList()
    {
        this.listMain.RemoveAll();
    }
}