using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MixedReality.Toolkit.UX;

public class UIController : MonoBehaviour
{
    public static UIController main;

    public TextMeshProUGUI callingDoctorName;
    public TextMeshProUGUI callingTime;
    [HideInInspector] public int min;
    [HideInInspector] public float sec;

    private void Awake()
    {
        main = this;
    }

    private void Update()
    {
        sec += Time.deltaTime;
        if (sec >= 60f)
        {
            min += 1;
            sec = 0;
        }

        callingTime.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
    }
}
