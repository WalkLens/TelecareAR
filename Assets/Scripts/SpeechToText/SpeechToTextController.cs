using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum RecognitionMode { Speech_Recognizer, Intent_Recognizer, Tralation_Recognizer, Disabled, Offline };
public enum SimuilateOfflineMode { Enabled, Disabled };
public enum TranslateToLanguage { Russian, German, Chinese, Korean, English, Japanese }; // �߰� : Korean, Japanese

public class SpeechToTextController : MonoBehaviour
{
    public static SpeechToTextController speechToTextController = null;

    [Header("Speech SDK Credentials")]
    public string SpeechServiceAPIKey = "";
    public string SpeechServiceRegion = "";

    [Header("Object References")]
    public GameObject terminal;
    public TextMeshProUGUI outputText;
    public List<ButtonController> buttons;

    public delegate void OnSelectRecognitionMode(RecognitionMode selectedMode);
    public event OnSelectRecognitionMode onSelectRecognitionMode;

    RecognitionMode speechRecognitionMode = RecognitionMode.Disabled;
    ButtonController activeButton = null;
    WakeWordRecognizer wakeWordRecognizer = null;
    OfflineRecognizer offlineRecognizer = null;

    private void Awake()
    {
        if (speechToTextController == null)
            speechToTextController = this;
        // �ϳ��� �����Ǵ� ���� �����ֱ� ���� �Ʒ� �� �� �ּ�ó�� 
        //else if (speechToTextController != this)
        //    Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (GetComponent<WakeWordRecognizer>())
        {
            wakeWordRecognizer = GetComponent<WakeWordRecognizer>();
        }
        if (GetComponent<OfflineRecognizer>())
        {
            offlineRecognizer = GetComponent<OfflineRecognizer>();
            if (offlineRecognizer.simulateOfflineMode == SimuilateOfflineMode.Disabled)
            {
                SetupOnlineMode();
            }
            else
            {
                SetupOfflineMode();
            }
        }
        else
        {
            SetupOnlineMode();
        }
    }

    public bool IsOfflineMode()
    {
        if (offlineRecognizer != null)
        {
            return offlineRecognizer.simulateOfflineMode == SimuilateOfflineMode.Enabled;
        }
        else
        {
            return false;
        }
    }

    private void SetupOnlineMode()
    {
        if (wakeWordRecognizer != null)
        {
            if (wakeWordRecognizer.WakeWord == "")
            {
                wakeWordRecognizer.WakeWord = "*";
                wakeWordRecognizer.DismissWord = "*";
            }

            if (wakeWordRecognizer.DismissWord == "")
            {
                wakeWordRecognizer.DismissWord = "*";
            }
        }


        if (GetComponent<TranslationController>())
        {
            ActivateButtonNamed("SatelliteButton");
        }

        if (GetComponent<IntentRecognizer>())
        {
            ActivateButtonNamed("RocketButton");
        }
    }

    private void SetupOfflineMode()
    {
        if (wakeWordRecognizer != null)
        {
            wakeWordRecognizer.WakeWord = "*";
            wakeWordRecognizer.DismissWord = "*";
        }

        if (GetComponent<WakeWordRecognizer>())
        {
            GetComponent<WakeWordRecognizer>().enabled = false;
        }
        if (GetComponent<SpeechToTextRecognizer>())
        {
            GetComponent<SpeechToTextRecognizer>().enabled = false;
        }
        if (GetComponent<TranslationController>())
        {
            GetComponent<TranslationController>().enabled = false;
            ActivateButtonNamed("SatelliteButton", false);
        }
        if (GetComponent<IntentRecognizer>())
        {
            GetComponent<IntentRecognizer>().enabled = false;
            ActivateButtonNamed("RocketButton", false);
        }
    }

    private void ActivateButtonNamed(string name, bool makeActive = true)
    {
        foreach (ButtonController button in buttons)
        {
            if (button.gameObject.name == name)
            {
                button.gameObject.SetActive(makeActive);
            }
        }
    }

    public RecognitionMode CurrentRecognitionMode()
    {
        return speechRecognitionMode;
    }

    public void SetActiveButton(ButtonController buttonToSetActive)
    {
        activeButton = buttonToSetActive;
        foreach (ButtonController button in buttons)
        {
            if (button != activeButton && button.GetIsSelected())
            {
                button.ShowNotSelected();
            }
        }
    }

    public void SelectMode(RecognitionMode speechRecognitionModeToSet)
    {
        speechRecognitionMode = speechRecognitionModeToSet;
        onSelectRecognitionMode(speechRecognitionMode);
        if (speechRecognitionMode == RecognitionMode.Disabled)
        {
            if (outputText.text == "Say something..." || outputText.text == "")
            {
                outputText.text = "Select a mode to begin.";
            }
        }
    }

    public void ShowTerminal()
    {
        terminal.SetActive(true);
    }

    public void HideTerminal()
    {
        if (terminal.activeSelf)
        {
            foreach (ButtonController button in buttons)
            {
                if (button.GetIsSelected())
                {
                    button.ShowNotSelected();
                }
            }

            outputText.text = "Select a mode to begin.";
            terminal.SetActive(false);
            SelectMode(RecognitionMode.Disabled);
        }
    }

    public void UpdateRecognizedText(string textToUpdate)
    {
        outputText.text = textToUpdate;
    }
}