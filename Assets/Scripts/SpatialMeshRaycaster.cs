using UnityEngine;
using MixedReality.Toolkit;

public class SpatialMeshRaycaster : MonoBehaviour
{
    public Camera mrtkXRRig;
    public GameObject targetObject;

    void Update()
    {
        Ray ray = new Ray(mrtkXRRig.transform.position, mrtkXRRig.transform.transform.forward);
        RaycastHit hit;

        // 31 : Spatial Mesh
        int layerMask = 1 << 31;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("Hit Spatial Mesh!");

            Instantiate(targetObject, hit.point, Quaternion.identity);
        }
        else
        {
            
        }
    }
}
