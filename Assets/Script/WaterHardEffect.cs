using System.Collections.Generic;
using UnityEngine;

public class WaterHardEffect : CombinationEffect
{
    [Header("通行控制")]
    public bool canPass = false;
    [Header("玩家标签")]
    public string playerTag = "Player";

    [Header("提醒信息")]
    public string blockMessage = "水流太急，试试其他办法吧";
    public string successMessage = "水变得坚硬了，可以通过这里";

    [Header("空气墙")]
    public GameObject airWall;

    [Header("河流碰撞体")]
    public Collider riverCollider;

    //[Header("阻挡设置")]
    //public float pushBackForce = 2f; // 减小推回力度
    //private bool playerIsNear = false;


    public override void TriggerEffect()
    {
        canPass = true;

        // 禁用空气墙
        if (airWall != null)
        {
            airWall.SetActive(false);
            Debug.Log("禁用空气墙，现在可以通过了");
        }

        // 禁用碰撞体 防止mesh collider bug
        if (riverCollider != null)
        {
            riverCollider.enabled = false;
            Debug.Log("禁用河流碰撞体，允许通行");
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(0.7f, 0.9f, 1f, 0.8f); // 冰蓝色
        }

        Debug.Log($"✅ {successMessage}");
    }

}