using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("组件引用")]
    public CharacterController controller;
    public MouseLook mouseLook;
    public PlayerMovement movement;
    public EmpowermentAbility empowermentAbility;
    public Transform holdPosition; //TODO：商议手持物品位置或者直接放进背包？

    [Header("交互设置")]
    public float interactDistance = 3f;
    public LayerMask interactableLayer;

    [Header("当前状态")]
    public PickupableObject heldObject; //当前手持物品
    private List<PickupableObject> nearbyPickupables = new List<PickupableObject>(); // 附近可捡起的物品

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;

        if (controller == null) controller = GetComponent<CharacterController>();
        if (mouseLook == null) mouseLook = GetComponentInChildren<MouseLook>();
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (empowermentAbility == null) empowermentAbility = GetComponent<EmpowermentAbility>();

        Cursor.lockState = CursorLockMode.Locked;

    }


    void Update()
    {
        HandleInteraction();
        HandleDropItem();
        HandleEmpowerment();
    }
    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject != null)
            {
                Debug.Log("已有手持物品， 尝试使用物品交互");
                return;
            }
            
            //捡起最近的物品
            if(nearbyPickupables.Count > 0)
            {
                PickupableObject closet = GetClosestPickupable();
                if(closet != null)
                {
                    PickupItem(closet);
                }
            }
            else
            {
                Debug.Log("附近没有可捡起的物品");
            }
        }
    }

    void HandleDropItem()
    {
        if (Input.GetKeyDown(KeyCode.Q) && heldObject != null)
        {
            heldObject.OnDrop();
            heldObject = null;
            Debug.Log("已丢弃物品");
        }
    }

    void HandleEmpowerment()
    {
        // 数字键1、2快速使用背包格中的特性
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryApplyPropertyToHeldObject(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryApplyPropertyToHeldObject(1);
        }
    }
    
  void TryApplyPropertyToHeldObject(int slotIndex)
    {
        if (heldObject != null)
        {
            bool success = empowermentAbility.ApplyProperty(heldObject, slotIndex);
            if (success)
            {
                Debug.Log($"已对手持物品赋予特性");
            }
        }
    }
    public void PickupItem(PickupableObject item)
    {
        if (heldObject != null)
        {
            heldObject.OnDrop(); // 如果已经拿着东西，先扔掉
        }

        heldObject = item;
        item.OnPickup(holdPosition);

        //从附近列表中移除，因为已经被捡起了
        nearbyPickupables.Remove(item);

        // 自动抽取特性到第一个可用背包格
        if (item.inherentProperty != ObjectProperty.None)
        {
            int freeSlot = FindFreeSlot();
            if (freeSlot != -1)
            {
                empowermentAbility.ExtractProperty(item.inherentProperty, freeSlot);
                Debug.Log($"已抽取特性 [{item.inherentProperty}] 到背包格 {freeSlot}");
            }
        }
    }

    private int FindFreeSlot()
    {
        for (int i = 0; i < empowermentAbility.propertySlots.Length; i++)
        {
            if (empowermentAbility.propertySlots[i] == ObjectProperty.None)
                return i;
        }
        return 0; // 如果没有空格，就覆盖第一个
    }

    PickupableObject GetClosestPickupable()
    {
        if (nearbyPickupables.Count == 0) return null;

        PickupableObject closest = null;
        float closestDistance = float.MaxValue;

        foreach (PickupableObject pickupable in nearbyPickupables)
        {
            if (pickupable == null) continue;

            float distance = Vector3.Distance(transform.position, pickupable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = pickupable;
            }
        }

        return closest;
    }

    void OnTriggerEnter(Collider other)
    {
        PickupableObject pickupable = other.GetComponent<PickupableObject>();
        if (pickupable != null && !nearbyPickupables.Contains(pickupable))
        {
            nearbyPickupables.Add(pickupable);
            Debug.Log($"进入捡起范围： {pickupable.gameObject.name}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        PickupableObject pickupable = other.GetComponent<PickupableObject>();
        if(pickupable != null && nearbyPickupables.Contains(pickupable))
        {
            nearbyPickupables.Remove(pickupable);
            Debug.Log($"离开捡起范围： {pickupable.gameObject.name}");
        }
    }
}