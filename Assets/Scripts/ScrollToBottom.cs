using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollToBottom : MonoBehaviour
{
    public void ScrollToBottomAuto()
    {
        Scrollbar scrollbar= GetComponent<Scrollbar>();
        scrollbar.value= 0;
    }
}
