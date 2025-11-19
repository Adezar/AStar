using System;
using UnityEngine;
using UnityEngine.UI;

namespace AStar.Views
{
    public class FindPathPanelView : MonoBehaviour
    {
        [SerializeField] private Button stopButton;

        public Action StopRequested;

        private void OnStopButtonClicked()
        {
            StopRequested?.Invoke();
        }
        
        private void OnEnable()
        {
            stopButton.onClick.AddListener(OnStopButtonClicked);
        }

        private void OnDisable()
        {
            stopButton.onClick.RemoveListener(OnStopButtonClicked);
        }
    }
}
