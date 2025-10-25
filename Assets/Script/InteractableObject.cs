using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
//using Unity.VisualScripting;

public class InteractableObject : MonoBehaviour
{

    [Header("当前特性")]
    public List<ObjectProperty> currentProperties = new List<ObjectProperty>();

    [Header("可理解的特性列表")]
    public List<ObjectProperty> understandableProperties = new List<ObjectProperty>();

    [Header("特性组合效果")]
    public PropertyCombination[] propertyCombinations;

    [System.Serializable]
    public class PropertyCombination
    {
        public List<ObjectProperty> requiredProperties;
        public UnityEvent onCombinationTriggered;         // 触发的效果
    }

    //private bool isFocus = false;

    void Start()
    {
        /* UI
        objectRenderer = GetComponent<Renderer>();
        UpdateVisualAppearance();
        */
    }

    //获取当前可理解的特性列表
    public List<ObjectProperty> GetUnderstandableProperties()
    {
        return new List<ObjectProperty>(understandableProperties);
    }

    public bool ReceiveProperty(ObjectProperty newProperty)
    {
        Debug.Log($"{name} 尝试接收特性: {newProperty}");

        // 添加新特性到物体
        if (!currentProperties.Contains(newProperty))
        {
            currentProperties.Add(newProperty);
            Debug.Log($"{name} 现在拥有特性: {string.Join(", ", currentProperties)}");

            // 检查特性组合效果
            CheckPropertyCombinations();
            return true;
        }
        else
        {
            Debug.Log($"{name} 已经拥有特性 {newProperty}");
            return false;
        }
    }

    // 检查特性组合触发效果
    private void CheckPropertyCombinations()
    {
        foreach (var combination in propertyCombinations)
        {
            if (HasAllProperties(combination.requiredProperties))
            {
                combination.onCombinationTriggered?.Invoke();
                Debug.Log($"触发特性组合效果: {string.Join(" + ", combination.requiredProperties)}");
            }
        }
    }

    // 检查是否拥有所有指定特性
    private bool HasAllProperties(List<ObjectProperty> requiredProps)
    {
        foreach (var prop in requiredProps)
        {
            if (!currentProperties.Contains(prop))
                return false;
        }
        return true;
    }

    //TODO
    public void OnFocus()
    {
        // 简单的高亮效果
        // 比如：改变材质颜色、显示轮廓、显示UI提示等
        Debug.Log($"{name} 被聚焦");
    }

    public void OnLoseFocus()
    {
        // 恢复原状
        Debug.Log($"{name} 失去聚焦");
    }
}