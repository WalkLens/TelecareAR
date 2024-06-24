using MRTK.Tutorials.MultiUserCapabilities;
using RealityCollective.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EyegazeUIManager : MonoBehaviour
{
    public static EyegazeUIManager main;
    public GameObject photonInfoUISample;
    public TextMeshProUGUI photonInfoUISampleMainText;
    public float yOffset = 0.0f;

    private void Awake()
    {
        main = this;
    }

    public void ActivateEyegazeUI(RaycastHit hit)
    {
        PhotonUser photonUserInfo = hit.collider.GetComponent<PhotonUser>();
        photonInfoUISampleMainText.text = photonUserInfo.GetNickName();
        Vector3 newPosition = hit.point + Vector3.up * yOffset;
        photonInfoUISample.transform.position = newPosition;
        photonInfoUISample.SetActive(true);
    }

    public void DeactivateEyegazeUI()
    {
        photonInfoUISample.SetActive(false);
    }
}