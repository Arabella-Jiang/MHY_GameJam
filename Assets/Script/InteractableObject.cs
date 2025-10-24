using UnityEngine;
public class InteractableObject : Interactable
{
    public ObjectProperty inherentProperty; // 物体自身特性
    public ObjectProperty currentProperty;  // 当前被赋予的特性
    
    public override void OnInteract(Player player)
    {
        Debug.Log($"与 {gameObject.name} 交互");
    }
    
    public virtual bool ReceiveProperty(ObjectProperty property)
    {
        currentProperty = property;
        
        switch(property)
        {
            case ObjectProperty.Hard:
                return OnReceiveHardProperty();
                
            case ObjectProperty.Flammable:
                return OnReceiveFlammableProperty();
                
            default:
                return false;
        }
    }
    
    protected virtual bool OnReceiveHardProperty()
    {
        Debug.Log($"{name} 获得了坚硬特性");
        return true;
    }
    
    protected virtual bool OnReceiveFlammableProperty()
    {
        Debug.Log($"{name} 获得了可燃特性");
        return true;
    }
    
    public override void OnFocus()
    {
        // 高亮效果
    }
    
    public override void OnLoseFocus()
    {
        // 取消高亮
    }
}