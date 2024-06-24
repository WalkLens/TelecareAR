using UnityEngine;
using MixedReality.Toolkit;
using Photon.Pun;
using MRTK.Tutorials.MultiUserCapabilities;
using RealityCollective.Extensions;

public class EyegazeDetector : MonoBehaviour
{
    private PhotonUser photonUser;
    private bool isUIActivated = false;

    private void Start()
    {
        photonUser = this.gameObject.GetComponent<PhotonUser>();
    }

    void Update()
    {
        Ray ray = new Ray(photonUser.transform.position, photonUser.transform.transform.forward);
        RaycastHit hit;

        // 30 : Photon User
        int layerMask = 1 << 30;

        if (!isUIActivated)
        {
        // if Ray hits Photon User, Instantiate InfoUI
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                EyegazeUIManager.main.ActivateEyegazeUI(hit);
                isUIActivated = true;
            }
        }
        else
        {
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                EyegazeUIManager.main.DeactivateEyegazeUI();
                isUIActivated = false;
            }
        }
    }
}