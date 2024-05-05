using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text timeText; // 
    private float startTime; 
    private bool timerActive = false; 

    // ��ư�� Ŭ���� �� ȣ��Ǵ� �޼���
    public void StartTimer()
    {
        startTime = Time.time;
        timerActive = true;
    }

    void Update()
    {
        if (timerActive)
        {
            float t = Time.time - startTime;

            // �ð��� �а� �ʷ� �и�
            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            // UI Text�� �ð��� ǥ��
            timeText.text = minutes + ":" + seconds;
        }
    }
}
