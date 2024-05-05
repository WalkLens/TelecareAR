using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelBtnSetActiveFalse : MonoBehaviour
{
    void Start()
    {
        // 5초 후에 DisableGameObject 함수를 호출합니다.
        //Invoke("DisableGameObject", 5f);
    }

    void DisableGameObject()
    {
        // 현재 게임 오브젝트를 비활성화합니다.
        gameObject.SetActive(false);
    }
}
