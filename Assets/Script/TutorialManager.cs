using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // 按T测试
        {
            FindObjectOfType<CameraManager>().PlaySunsetSequence(5f);
        }
    }

}