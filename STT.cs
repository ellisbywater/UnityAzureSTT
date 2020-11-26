using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.CognitiveServices.Speech;
using MiniJSON;
using Reader;
using SimpleJSON;
using UnityEngine;

public class STT : MonoBehaviour
{
    public static STT Instance;

    public STT()
    {
        Instance = this;
    }
    
    public object threadLocker = new object();
    public static bool WaitingForRecording;
    
    private string jsonCreds;
    private AzureConfig _azureConfig = new AzureConfig();
    
#if PLATFORM_ANDROID || PLATFORM_IOS
    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;
#endif
    
    // Creates an instance of a speech config with specified subscription key and service region.
    // Replace with your own subscription key and service region (e.g., "westus").
    private SpeechConfig _speechConfig;
    
    
    public async void RunStt()
    {
        // Make sure to dispose the recognizer after use!
        using (var recognizer = new SpeechRecognizer(_speechConfig))
        {
            lock (threadLocker)
            {
                SttEventManager.RecorderStart.Invoke();
                SttResult.IsRecording = WaitingForRecording = true;
            }
            // Starts speech recognition, and returns after a single utterance is recognized. The end of a
            // single utterance is determined by listening for silence at the end or until a maximum of 15
            // seconds of audio is processed.  The task returns the recognition text as result.
            // Note: Since RecognizeOnceAsync() returns only a single utterance, it is suitable only for single
            // shot recognition like command or query.
            // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.
            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);
            // Checks result
            string newMessage = String.Empty;

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                SttResult.Message = result.Text;
                SttResult.Reason = Reason.Recognized;
                Debug.Log("Speech Recognized: " + SttResult.Message);
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                SttResult.Message = "NO MATCH: Speech could not be recognized.";
                SttResult.Reason = Reason.NoMatch;
                Debug.Log(SttResult.Message);
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                SttResult.Message = $"CANCELED: Reason={cancellation.Reason} ERROR_DETAILS={cancellation.ErrorDetails}";
                SttResult.Reason = Reason.Canceled;
                Debug.Log(SttResult.Message);
            }

            lock (threadLocker)
            {
                SttResult.IsRecording = WaitingForRecording = false;
            }
            Debug.Log(SttResult.Message);
            SttEventManager.RecorderStop.Invoke();
        }
    }

    private void Awake()
    {
        string jsonPath = Path.Combine(Application.dataPath, "Scripts/Reader/config.json");
        Debug.Log(jsonPath);
        StreamReader reader = new StreamReader(jsonPath);
        string json = reader.ReadToEnd();
        Debug.Log(json);
        var N = JSON.Parse(json);
        reader.Close();
        Debug.Log(N);
        _azureConfig.SubscriptionKey = N["SubscriptionKey"].Value;
        _azureConfig.Region = N["Region"].Value;
        _speechConfig = SpeechConfig.FromSubscription(_azureConfig.SubscriptionKey, _azureConfig.Region);
    }
}
