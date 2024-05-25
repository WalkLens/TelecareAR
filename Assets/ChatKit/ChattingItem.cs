using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChattingItem : MonoBehaviour
{
    [SerializeField] private TMP_Text chatText;

    public void SetText(string txt)
    {
        chatText.text = txt;
    }
}
