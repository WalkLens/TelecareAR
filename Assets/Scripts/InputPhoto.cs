using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPhoto : MonoBehaviour
{

    public GameObject parent;

    public Material none;

    public int clickCount = 0;

    public bool NoneValue = false;

    //private Material mat; // mat 변수를 클래스 레벨로 이동

    public GameObject material;

    public int i = 0; //delete버튼 변수

    public void ActiveSelectBtn()
    {
        Transform[] myChildren = parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in myChildren)
        {
            if (child.name == "FrontPlate")
            {

                if (child.GetComponent<MeshRenderer>().material.name == none.name + " (Instance)")
                {

                    foreach (Transform subChild in child)
                    {
                        if (subChild.name == "Select")
                        {
                            print("select 인식");
                            subChild.gameObject.SetActive(true);
                            
                        }
                    }
                    
                }
            }

            material.GetComponent<MeshRenderer>().material = this.GetComponent<MeshRenderer>().material;
        }


        

    }


    public void DelBtnCtrl()
    {
        if (i % 2==0)
        {
            SelectDelBtn();
            i++;
        }

        else
        {
            DelBtnFalse();
            i++;
        }
    }

    public void SelectDelBtn()
    {
        Transform[] myChildren = parent.GetComponentsInChildren<Transform>();

        print("델리트 버튼 실행");
        foreach (Transform child in myChildren)
        {
            if (child.name == "FrontPlate")
            {

                if (child.GetComponent<MeshRenderer>().material.name != none.name + " (Instance)")
                {
                    foreach (Transform subChild in child)
                    {
                        if (subChild.name == "delete")
                        {
                            print("Delete 인식");
                            subChild.gameObject.SetActive(true);
                            

                        }
                    }

                }


            }
        }
    }

    public void DelBtnFalse()
    {
        Transform[] myChildren = parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in myChildren)
        {
            if (child.name == "FrontPlate")
            {

                if (child.GetComponent<MeshRenderer>().material.name != none.name + " (Instance)")
                {
                    foreach (Transform subChild in child)
                    {
                        if (subChild.name == "delete")
                        {
                            subChild.gameObject.SetActive(false);

                        }
                    }

                }


            }

        }

    }

    public void SelectBtnMatChange()
    {
        print(material.name);
        parent.GetComponent<MeshRenderer>().material = material.GetComponent<MeshRenderer>().material;
    }

    /*
    public void NoneMaterial()
    {
        Material mat = this.GetComponent<MeshRenderer>().material;

        print(mat.name);

        Transform[] myChildren = parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in myChildren)
        {



            if (child.name == "FrontPlate")
            {
                print("프론트 플레이트 감지");


                print(child.GetComponent<MeshRenderer>().material.name);
                print(mat.name + " (Instance)");


                if (child.GetComponent<MeshRenderer>().material.name==mat.name+" (Instance)")
                {
                    print("매트네임 감지");
                    child.GetComponent<MeshRenderer>().material = none;

                    clickCount = 0;

                    //break;

                }

            }

        }
    }

    public void ChangeMaterial()
    {

        if (clickCount == 0)
        {
            PhotoMaterial();
        }

        else
        {
            NoneMaterial();
        }
        



    }

    public void PhotoMaterial()
    {
        print("함수실행");

        Material mat = this.GetComponent<MeshRenderer>().material;

        //example.GetComponent<MeshRenderer>().material = mat;

        Transform[] myChildren = parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in myChildren)
        {


            if (child.name == "FrontPlate")
            {

                if (child.GetComponent<MeshRenderer>().material.name == "None (Instance)")
                {

                    child.GetComponent<MeshRenderer>().material = mat;

                   clickCount = 1;

                    break;

                }

            }



        }

    }

    */


}
