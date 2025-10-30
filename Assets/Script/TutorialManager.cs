using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public Player player;
    public EmpowermentAbility empowerment;

    [Header("关卡目标引用")]
    public InteractableObject rock;             // 可理解 Hard 的石头
    public BranchIgnition thinBranch;           // 细树枝（被点燃目标）
    public BranchIgnition thickBranch;          // 粗树枝（用于摩擦）
    public WaterHardEffect waterHard;           // 水流硬化效果
    public InteractableObject stoneTablet;      // 终点石碑

    private enum Step
    {
        Start,
        LearnHard,
        HardenWater,
        HardenBranches,
        Ignite,
        CarryBranchToTablet,
        Complete
    }

    private Step step = Step.Start;

    void Start()
    {
        if (player == null) player = FindObjectOfType<Player>();
        if (empowerment == null && player != null) empowerment = player.empowermentAbility;

        Log("教程开始：靠近石头长按E理解 Hard（数字1切换使用）");
        step = Step.LearnHard;
    }

    void Update()
    {
        
        switch (step)
        {
            case Step.LearnHard:
                if (InventoryHas(ObjectProperty.Hard))
                {
                    Log("已获得 Hard。先对水面短按E进行硬化，穿过河流");
                    step = Step.HardenWater;
                }
                break;

            case Step.HardenWater:
                if (waterHard != null && waterHard.canPass)
                {
                    Log("水已硬化，可通过。接着对两根树枝分别短按E赋予 Hard");
                    step = Step.HardenBranches;
                }
                break;

            case Step.HardenBranches:
                if (thinBranch != null && thickBranch != null && thinBranch.HasHardened() && thickBranch.HasHardened())
                {
                    Log("两根树枝已变硬。空手对着任意树枝短按E进行摩擦点火");
                    step = Step.Ignite;
                }
                break;

            case Step.Ignite:
                if (thinBranch != null && thinBranch.IsIgnited())
                {
                    Log("细树枝已点燃。拾起细树枝，带回石碑旁边并按E完成");
                    step = Step.CarryBranchToTablet;
                }
                break;

            case Step.CarryBranchToTablet:
                if (player != null && player.playerHoldItem != null && player.playerHoldItem.heldObject != null)
                {
                    bool holdingThin = player.playerHoldItem.heldObject == thinBranch.gameObject;
                    if (holdingThin && stoneTablet != null)
                    {
                        float dist = Vector3.Distance(player.transform.position, stoneTablet.transform.position);
                        if (dist < 3f && Input.GetKeyDown(KeyCode.E))
                        {
                            Log("已将树枝带回石碑。教程完成");
                            step = Step.Complete;
                        }
                    }
                }
                break;
        }
    }

    private bool HasProperty(InteractableObject obj, ObjectProperty prop)
    {
        return obj != null && obj.currentProperties != null && obj.currentProperties.Contains(prop);
    }

    private bool InventoryHas(ObjectProperty prop)
    {
        if (empowerment == null) return false;
        foreach (var p in empowerment.propertySlots)
        {
            if (p == prop) return true;
        }
        return false;
    }

    private void Log(string msg)
    {
        Debug.Log($"[Tutorial] {msg}");
    }
}