using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    public GameObject material;

    public Material noneMat;

    public GameObject parent;

    public void SelectBtnMatChange()
    {
        //print("셀렉트 관련 함수 실행");
        transform.parent.GetComponent<MeshRenderer>().material = material.GetComponent<MeshRenderer>().material;


        Transform[] myChildren = parent.GetComponentsInChildren<Transform>();
        foreach (Transform child in myChildren)
        {
            if (child.name == "Select")
            {
                child.gameObject.SetActive(false);
            }
        }


    }

    public void DelImage()
    {
        transform.parent.GetComponent<MeshRenderer>().material = noneMat;
        this.gameObject.SetActive(false);



    }
}
