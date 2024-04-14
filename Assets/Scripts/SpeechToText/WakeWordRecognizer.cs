// Copyright (c) Microsoft Corporation. 
// Licensed under the MIT License.

using UnityEngine;
using Microsoft.CognitiveServices.Speech;

public class WakeWordRecognizer : MonoBehaviour
{
    public string WakeWord = string.Empty;
    public string DismissWord = string.Empty;

    private string recognizedString = "Select a mode to begin.";
    private object threadLocker = new object();

    private SpeechRecognizer recognizer;

    private bool micPermissionGranted = false;

    private string fromLanguage = "en-US";
    private SpeechToTextController speechToTextController;

    private void Start()
    {
        speechToTextController = SpeechToTextController.speechToTextController;
        if (speechToTextController.outputText == null)
        {
            Debug.LogError("outputText property is null! Assign a UI Text element to it.");
        }
        else
        {
            micPermissionGranted = true;
        }

        if (GetComponent<OfflineRecognizer>())
        {
            OfflineRecognizer OfflineRecognizer = GetComponent<OfflineRecognizer>();
            if (OfflineRecognizer.simulateOfflineMode != SimuilateOfflineMode.Enabled)
            {
                if (WakeWord != string.Empty && WakeWord != "*")
                {
                    speechToTextController.ShowTerminal();
                    BeginRecognizing();
                }
            }
        }
        else
        {
            if (WakeWord != string.Empty && WakeWord != "*")
            {
                speechToTextController.ShowTerminal();
                BeginRecognizing();
            }
        }
    }

    public async void BeginRecognizing()
    {
        if (micPermissionGranted)
        {
            CreateSpeechRecognizer();

            if (recognizer != null)
            {
                await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            }
        }
    }

    private void CreateSpeechRecognizer()
    {
        if (recognizer == null)
        {
            SpeechConfig config = SpeechConfig.FromSubscription(speechToTextController.SpeechServiceAPIKey, speechToTextController.SpeechServiceRegion);
            config.SpeechRecognitionLanguage = fromLanguage;
            recognizer = new SpeechRecognizer(config);
            if (recognizer != null)
            {
                recognizer.Recognizing += RecognizingHandler;
            }
        }
    }

    private void RecognizingHandler(object sender, SpeechRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.RecognizingSpeech)
        {
            lock (threadLocker)
            {
                recognizedString = $"{e.Result.Text}";
            }
        }
    }

    private void Update()
    {
        if (speechToTextController.terminal.activeSelf)
        {
            if (recognizedString.ToLower().Contains(DismissWord.ToLower()))
            {
                speechToTextController.HideTerminal();
            }
        }
        else
        {
            if (recognizedString.ToLower().Contains(WakeWord.ToLower()))
            {
                speechToTextController.ShowTerminal();
            }
        }
    }

    private void OnDestroy()
    {
        if (recognizer != null)
        {
            recognizer.Dispose();
        }
    }
}
