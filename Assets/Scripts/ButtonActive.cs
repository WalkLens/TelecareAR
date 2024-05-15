using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActive : MonoBehaviour
{
    public Material[] mat = new Material[2];
    int i = 0;
    //int i = 0;

    public void ChangeMat()
    {
        //Material[] mat = this.GetComponent<MeshRenderer>().materials;

        i++;

        if (i%2 == 0)
        {
            this.GetComponent<MeshRenderer>().material = mat[0];
            //i++;
            print("mat[0]½ÇÇàµÊ"+mat[0].name);
        }

        if (i % 2 == 1)
        {
            this.GetComponent<MeshRenderer>().material = mat[1];
            //i++;
            print("mat[1]½ÇÇàµÊ"+mat[1].name);
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
