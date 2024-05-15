using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverActivator : MonoBehaviour
{
    [SerializeField] private GameObject callingProfile;
    [SerializeField] private GameObject callToOther;

    public void ActivateCallToOtherUI()
    {
        callingProfile.SetActive(false);
        callToOther.SetActive(true);
    }

    public void DeactivateCallToOtherUI()
    {
        callingProfile.SetActive(true);
        callToOther.SetActive(false);
    }
}
