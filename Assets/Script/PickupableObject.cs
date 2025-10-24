using UnityEngine;
public class PickupableObject : InteractableObject
{
    [Header("捡起设置")]
    public bool canBePickedUp = true;
    private Rigidbody rb;
    private Collider col;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }
    
    public override void OnInteract(Player player)
    {
        if (canBePickedUp)
        {
            player.PickupItem(this);
        }
    }
    
    public void OnPickup(Transform holdPosition)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        if (col != null) col.enabled = false;
        
        transform.SetParent(holdPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        Debug.Log($"已捡起 {gameObject.name}");
    }
    
    public void OnDrop()
    {
        transform.SetParent(null);
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        if (col != null) col.enabled = true;
        
        Debug.Log($"已放下 {gameObject.name}");
    }
}