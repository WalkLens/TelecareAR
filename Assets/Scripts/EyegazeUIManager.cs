using MRTK.Tutorials.MultiUserCapabilities;
using RealityCollective.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EyegazeUIManager : MonoBehaviour
{
    public static EyegazeUIManager main;
    public GameObject photonInfoUI;
    public float yOffset = 0.0f;
    private GameObject photonInfoUIClone;

    private void Awake()
    {
        main = this;
    }

    public void ActivateEyegazeUI(RaycastHit hit)
    {
        PhotonUser photonUserInfo = hit.collider.GetComponent<PhotonUser>();
        Vector3 newPosition = hit.point + Vector3.up * yOffset;

        photonInfoUIClone = Instantiate(photonInfoUI, newPosition, Quaternion.identity);

        Transform cameraTransform = Camera.main.transform;
        photonInfoUIClone.transform.LookAt(cameraTransform);
        photonInfoUIClone.transform.Rotate(0, 180, 0);

        Transform canvas = photonInfoUIClone.transform.GetChild(0);
        Transform mainText = canvas.transform.GetChild(1);
        mainText.GetComponent<TextMeshProUGUI>().text = photonUserInfo.GetNickName();
    }

    public void DeactivateEyegazeUI()
    {
        if (photonInfoUIClone != null)
        {
            Destroy(photonInfoUIClone);
        }
    }
}
