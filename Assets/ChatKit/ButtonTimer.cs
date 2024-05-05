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

    // 버튼이 클릭될 때 호출되는 메서드
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

            // 시간을 분과 초로 분리
            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            // UI Text에 시간을 표시
            timeText.text = minutes + ":" + seconds;
        }
    }
}
