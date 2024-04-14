using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPhoto : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject parent;
    //public GameObject example;

    public Material none;

    public int clickCount = 0;

    void Start()
    {      

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NoneMaterial()
    {
        Material mat = this.GetComponent<MeshRenderer>().material;

        print(mat.name);

        Transform[] myChildren = parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in myChildren)
        {
            // print(child.name);
            //print(child);
            //print(child.gameObject.activeSelf);


            if (child.name == "FrontPlate")
            {
                //print(child.GetComponent<MeshRenderer>().material.name);
                print("프론트 플레이트 감지");
                //print(mat.name);
                //print(child.GetComponent<MeshRenderer>().material.name);

                print(child.GetComponent<MeshRenderer>().material.name);
                print(mat.name + " (Instance)");

                //int index = child.name.IndexOf("(Instance)");
                //int index2 = mat.name.IndexOf("(Instance)");
                //child.GetComponent<MeshRenderer>().material.name.Substring(0,index) == mat.name.Substring(0,index))
                //mat.name += "(instance)";

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
            //clickCount = 1;
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
            // print(child.name);
            //print(child);
            //print(child.gameObject.activeSelf);


            if (child.name == "FrontPlate")
            {
                //print(child.GetComponent<MeshRenderer>().material.name);

                if (child.GetComponent<MeshRenderer>().material.name == "None (Instance)")
                {

                    child.GetComponent<MeshRenderer>().material = mat;

                   clickCount = 1;

                    break;

                }

            }



            /*
            if (child.gameObject.activeSelf == false)
            {
                child.gameObject.SetActive(true);
                child.GetComponent<MeshRenderer>().material = mat;

                break;
            }
            */
        }

        /*
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).ga
        }
        */

    }


}
