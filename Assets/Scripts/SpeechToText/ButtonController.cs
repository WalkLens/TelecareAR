using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MRTK.Tutorials.GettingStarted.PlacementHintsController;

public class ButtonController : MonoBehaviour
{
    [Header("Reference Objects")]
    public RecognitionMode speechRecognitionMode = RecognitionMode.Disabled;
    public delegate void ButtonControllerDelegate();
    public event ButtonControllerDelegate OnTogglePunButtonController;

    [Space(6)]
    [Header("Button States")]
    public Sprite Default;
    public Sprite Activated;

    private Button button;
    private bool isSelected = false;
    public bool isPunEnabled = true;

    private SpeechToTextController speechToTextController;
    public bool IsPunEnabled
    {
        set => isPunEnabled = value;
    }

    private void Start()
    {
        speechToTextController = SpeechToTextController.speechToTextController;
        button = GetComponent<Button>();
    }

    public bool GetIsSelected()
    {
        return isSelected;
    }

    public void PunToggleSelected()
    {
        if (isPunEnabled)
            OnTogglePunButtonController?.Invoke();
        else
            ToggleSelected();
    }

    public void ToggleSelected()
    {
        if (isSelected)
        {
            DeselectButton();
        }
        else
        {
            //button.image.sprite = Activated;
            isSelected = true;
            speechToTextController.SetActiveButton(GetComponent<ButtonController>());

            if (speechToTextController.IsOfflineMode())
            {
                speechToTextController.SelectMode(RecognitionMode.Offline);
            }
            else
            {
                speechToTextController.SelectMode(speechRecognitionMode);
            }
        }
    }

    public void ShowNotSelected()
    {
        //button.image.sprite = Default;
        isSelected = false;
    }

    public void DeselectButton()
    {
        ShowNotSelected();
        speechToTextController.SelectMode(RecognitionMode.Disabled);
    }
}
