using UnityEngine;
using UnityEngine.UI;

public class EmpowermentAbility : MonoBehaviour
{
    [Header("特性背包")]
    public ObjectProperty[] propertySlots = new ObjectProperty[2]; // 两个格子

    //TODO: UI
    /*
    [Header("UI 参考")]
    public Image[] slotIcons; // 在Inspector中绑定两个UI Image
    */
    
    
    // 从物体抽取特性到指定背包格
    public void ExtractProperty(ObjectProperty property, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= propertySlots.Length)
        {
            Debug.LogError($"无效的背包格索引: {slotIndex}");
            return;
        }

        propertySlots[slotIndex] = property;

        Debug.Log($"特性 [{property}] 已存入背包格 {slotIndex}");
        
        //TODO: UI: 
        //UpdateSlotUI(slotIndex);
        
        Debug.Log($"特性 [{property}] 已存入背包格 {slotIndex}");
    }
    
    // 对目标物体赋予特性
    public bool ApplyProperty(InteractableObject target, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= propertySlots.Length)
        {
            Debug.LogError($"无效的背包格索引: {slotIndex}");
            return false;
        }
            
        if (propertySlots[slotIndex] == ObjectProperty.None)
        {
            Debug.Log("该背包格为空");
            return false;
        }
            
        
        bool success = target.ReceiveProperty(propertySlots[slotIndex]);
        if (success)
        {
            Debug.Log($"特性 [{propertySlots[slotIndex]}] 已赋予 {target.name}");
        }
        return success;
    }
    
    // 更新UI显示
    private void UpdateSlotUI(int slotIndex)
    {
        //TODO: 连接UI
        /* 
        if (slotIcons[slotIndex] != null)
        {
            // 这里根据特性类型切换图标
            switch(propertySlots[slotIndex])
            {
                case ObjectProperty.Hard:
                    slotIcons[slotIndex].color = Color.gray;
                    break;
                case ObjectProperty.Flammable:
                    slotIcons[slotIndex].color = Color.red;
                    break;
                case ObjectProperty.None:
                    slotIcons[slotIndex].color = Color.white;
                    break;
            }
        }
        */
    }
}