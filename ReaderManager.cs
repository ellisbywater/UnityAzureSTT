using UnityEngine;

namespace Reader
{
    public class ReaderManager : MonoBehaviour
    {
        private void Awake()
        {
            SttEventManager.RecorderStart.AddListener(UpdateStartRecordText);
            SttEventManager.RecorderStop.AddListener(UpdateStopRecordText);
        }

        public static bool micPermissionGranted;

        private void Start()
        {
            if (ReaderUI.Instance._recordedText == null)
            {
                ReaderUI.Instance.CreateNotification("_recordedText property is null! Assign a UI Text element to it.");
                Debug.LogError("_recordedText property is null! Assign a UI Text element to it.");
            }
            else if (ReaderUI.Instance._recorderButton == null)
            {
                ReaderUI.Instance.CreateNotification("startRecoButton property is null! Assign a UI Button to it."); 
                Debug.LogError("No Record Button");
            }
            else
            {
                // Continue with normal initialization, Text and Button objects are present.
#if PLATFORM_ANDROID
                // Request to use the microphone, cf.
                // https://docs.unity3d.com/Manual/android-RequestingPermissions.html
                message = "Waiting for mic permission";
                if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
                {
                    Permission.RequestUserPermission(Permission.Microphone);
                }
#elif PLATFORM_IOS
            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                Application.RequestUserAuthorization(UserAuthorization.Microphone);
            }
#else
                micPermissionGranted = true;
                ReaderUI.Instance.CreateNotification("Click button to recognize speech");
#endif
                ReaderUI.Instance._recorderButton.onClick.AddListener(STT.Instance.RunStt);
            }
        }


        private void Update()
        {
#if PLATFORM_ANDROID
            if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                micPermissionGranted = true;
                ReaderUI._recordedText = "Click button to recognize speech";
            }
#elif PLATFORM_IOS
        if (!micPermissionGranted && Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            micPermissionGranted = true;
            ReaderUI._recordedText = "Click button to recognize speech";
        }
#endif
            lock (STT.Instance.threadLocker)
            {
                if (ReaderUI.Instance._recorderButton != null)
                {
                    ReaderUI.Instance._recorderButton.interactable = !STT.WaitingForRecording && 
                        micPermissionGranted;
                }

                if (ReaderUI.Instance._recordedText != null)
                {
                    ReaderUI.Instance.UpdateText(SttResult.Message, ReaderUI.TextType.Recorded);
                }
            }

        }


        public void UpdateStartRecordText()
        {
            Debug.Log("IM RECORDING EVERYTHING!!!");
        }
        public void UpdateStopRecordText()
        {
            Debug.Log("I SHUT THE FUCK UP!!!!!!!!!!");
            ReaderUI.Instance.UpdateText(SttResult.Message, ReaderUI.TextType.Recorded);
        }
    }
}
