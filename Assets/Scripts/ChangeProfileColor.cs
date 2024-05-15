using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeProfileColor : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI doctorName;
    [SerializeField] private Sprite changedProfileImage;
    [SerializeField] private Sprite originalProfileImage;
    public void ChangeUI()
    {
        Color color;
        ColorUtility.TryParseHtmlString("#72FF80", out color);

        profileImage.sprite = changedProfileImage;
        doctorName.color = color;
    }

    public void ResetUI()
    {
        Color color;
        ColorUtility.TryParseHtmlString("#FFFFFF", out color);

        profileImage.sprite = originalProfileImage;
        doctorName.color = color;
    }
}
