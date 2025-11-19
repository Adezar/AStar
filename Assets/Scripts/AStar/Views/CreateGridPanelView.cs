using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AStar.Views
{
    public class CreateGridPanelView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField widthInputField;
        [SerializeField] private TMP_InputField heightInputField;
        [SerializeField] private Button createGridButton;
        
        public Action<int, int> CreateGridRequested;
        
        private void OnCreateGridButtonClicked()
        {
            if (!int.TryParse(widthInputField.text, out var width)) return;
            if (!int.TryParse(heightInputField.text, out var height)) return;
            
            CreateGridRequested?.Invoke(width, height);
        }

        private void OnEnable()
        {
            createGridButton.onClick.AddListener(OnCreateGridButtonClicked);
        }
        private void OnDisable()
        {
            createGridButton.onClick.RemoveListener(OnCreateGridButtonClicked);
        }
    }
}
