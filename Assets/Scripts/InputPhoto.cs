using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPhoto : MonoBehaviour
{

    public GameObject parent;

    public Material none;

    public int clickCount = 0;

    public bool NoneValue = false;

    //private Material mat; // mat ������ Ŭ���� ������ �̵�

    public GameObject material;

    public int i = 0; //delete��ư ����

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
                            print("select �ν�");
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

        print("����Ʈ ��ư ����");
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
                            print("Delete �ν�");
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
                print("����Ʈ �÷���Ʈ ����");


                print(child.GetComponent<MeshRenderer>().material.name);
                print(mat.name + " (Instance)");


                if (child.GetComponent<MeshRenderer>().material.name==mat.name+" (Instance)")
                {
                    print("��Ʈ���� ����");
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
        print("�Լ�����");

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
