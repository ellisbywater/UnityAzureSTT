using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Reader
{
    public class ReaderUI : MonoBehaviour
    {
        public static ReaderUI Instance;
        public ReaderUI()
        {
            Instance = this;
        }
    
        [Header("UI Elements")]
        public GameObject _headerDisplay;
        public TextMeshProUGUI _headerText;
    
        public GameObject _chapterDisplay;
        public TextMeshProUGUI _chapterText;
    
        public GameObject _recordedTextDisplay;
        public TextMeshProUGUI _recordedText;

        public GameObject notificationPanel;
        public TextMeshProUGUI notificationText;
        public Button NotificationAcknowledgeButton;
        private static List<Image> notificationList = new List<Image>();
    
        public Button _recorderButton;
        public Color _recorderButtonOffColor;
        public Color _recorderButtonOnColor;
    
    


    
        public enum TextType
        {
            Recorded,
            Chapter,
            Header
        }


        public void UpdateText(string inputString, TextType textType)
        {
            switch (textType)
            {
                case TextType.Chapter:
                    _chapterText.text = inputString;
                    break;
                case TextType.Header:
                    _headerText.text = inputString;
                    break;
                case TextType.Recorded:
                    _recordedText.text = inputString;
                    break;
                default:
                    _chapterText.text = inputString;
                    break;   
            }
        }

        public void CreateNotification(string newNotification)
        {
            notificationPanel.SetActive(true);
            notificationText.text = newNotification;
        }

        public void AcknowledgeNotification()
        {
            notificationPanel.SetActive(false);
        }
    

        public static void ChangeButtonVisual(Button button, Color newBackgroundColor)
        {
        
        }

        public static string ColorCodeSTTResult(string orginal, [CanBeNull] string[] errors)
        {
            return String.Empty;
        }

        public static void Congratulate()
        {
            return;
        }

        private void Awake()
        {
            NotificationAcknowledgeButton.onClick.AddListener(AcknowledgeNotification);
        }
        
    }
}