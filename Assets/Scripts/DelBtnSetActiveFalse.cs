using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelBtnSetActiveFalse : MonoBehaviour
{
    void Start()
    {
        // 5�� �Ŀ� DisableGameObject �Լ��� ȣ���մϴ�.
        //Invoke("DisableGameObject", 5f);
    }

    void DisableGameObject()
    {
        // ���� ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        gameObject.SetActive(false);
    }
}
