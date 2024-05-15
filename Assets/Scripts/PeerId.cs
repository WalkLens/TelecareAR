using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.WebRTC.Unity;
using TMPro;
using UnityEngine;

public class PeerId : MonoBehaviour
{
    public NodeDssSignaler signaler;
    public TextMeshProUGUI text;

    private void Start()
    {
        SetPeerId();
    }

    public void SetPeerId()
    {
        text.text = signaler.RemotePeerId;
    }

}
