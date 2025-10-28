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

    [Header("河流碰撞体")]
    public Collider riverBoxCollider;
    public Collider riverPlaneBoxCollider;

    [Header("水效果控制")]
    public Renderer waterRenderer;
    public Material hardenedWaterMaterial; // 硬化后的材质

    private bool hasShownBlockMessage = false;
    private bool effectTriggered = false; // 防止重复触发

    public override void TriggerEffect()
    {
        if (effectTriggered) return; // 防止重复触发

        canPass = true;
        effectTriggered = true;

        // 禁用碰撞体 防止mesh collider bug
        if (riverBoxCollider != null)
        {
            riverBoxCollider.enabled = false;
            Debug.Log("禁用河流碰撞体，允许通行");
        }

        if(riverPlaneBoxCollider != null)
        {
            riverPlaneBoxCollider.enabled = true;
        }

        // 替换材质
        ReplaceWaterMaterial();

        Debug.Log($"✅ {successMessage}");
    }
    
    private void ReplaceWaterMaterial()
    {
        if (waterRenderer != null && hardenedWaterMaterial != null)
        {
            waterRenderer.material = hardenedWaterMaterial;
            Debug.Log("水材质已替换为硬化版本");
        }
        else
        {
            Debug.LogError("无法替换材质：渲染器或硬化材质为空");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查是否是玩家且当前不能通行
        if (other.CompareTag(playerTag) && !canPass && !hasShownBlockMessage)
        {
            Debug.Log($"✅ {blockMessage}");
            hasShownBlockMessage = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 玩家离开区域时重置标记
        if (other.CompareTag(playerTag))
        {
            hasShownBlockMessage = false;
        }
    }

}