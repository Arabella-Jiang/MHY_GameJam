using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string interactText = "交互";
    
    public abstract void OnInteract(Player player);
    
    public virtual void OnFocus()
    {
        // 高亮显示等逻辑
    }
    
    public virtual void OnLoseFocus()
    {
        // 取消高亮
    }
}