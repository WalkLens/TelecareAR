using UnityEngine;
using MixedReality.Toolkit;
using Photon.Pun;
using MRTK.Tutorials.MultiUserCapabilities;
using RealityCollective.Extensions;

public class EyegazeDetector : MonoBehaviour
{
    public EyegazeUIManager photonInfoUI;
    public float yOffset = 0;
    private GameObject photonUser;
    private EyegazeUIManager photonInfoUISample;
    private bool isUIActivated = false;

    private void Start()
    {
        photonUser = this.gameObject;
    }

    void Update()
    {
        Ray ray = new Ray(photonUser.transform.position, photonUser.transform.transform.forward);
        RaycastHit hit;

        // 30 : Photon User
        int layerMask = 1 << 30;

        if (!isUIActivated)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                PhotonUser photonUserInfo = hit.collider.GetComponent<PhotonUser>();
                Vector3 hitPoint = hit.point;
                hitPoint.y += yOffset;
                photonInfoUISample.SetActive(true);
                photonInfoUISample.mainText.text = photonUserInfo.GetNickName();
                isUIActivated = true;
            }
        }
        else
        {
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                photonInfoUISample.SetActive(false);
                isUIActivated = false;
            }
        }
    }
}