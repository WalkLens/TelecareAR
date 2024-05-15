using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MixedReality.Toolkit.UX;

public class ProfileButtonUI : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI department;
    [SerializeField] private TextMeshProUGUI doctorName;

    private PressableButton profileButton;

    private void Start()
    {
        profileButton = this.gameObject.GetComponent<PressableButton>();
        profileButton.OnClicked.AddListener(SendCall);
    }

    private void SendCall()
    {
        UIController.main.callingDoctorName.text = $"가정의학과 {doctorName.text} 교수";
        UIController.main.min = 0;
        UIController.main.sec = 0.0f;
    }
}
