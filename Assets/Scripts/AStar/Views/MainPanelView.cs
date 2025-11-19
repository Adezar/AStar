using System;
using UnityEngine;
using UnityEngine.UI;

namespace AStar.Views
{
    public class MainPanelView : MonoBehaviour
    {
        [SerializeField] private Button newGridButton;
        [SerializeField] private Button editGridButton;
        [SerializeField] private Button findPathButton;
    
        [SerializeField] private Toggle setFourDirectionsToggle;
        [SerializeField] private Toggle setEightDirectionsToggle;
        
        public Action NewGridRequested;
        public Action EditGridRequested;
        public Action FindPathRequested;
        public Action<bool> ChooseEightDirectionsRequested;
    
        private void OnNewGridButtonClicked()
        {
            NewGridRequested?.Invoke();
        }
    
        private void OnEditGridButtonClicked()
        {
            EditGridRequested?.Invoke();
        }
    
        private void OnFindPathButtonClicked()
        {
            FindPathRequested?.Invoke();
        }
    
        private void OnChooseFourDirectionClicked(bool isOn)
        {
            if (isOn)
                ChooseEightDirectionsRequested?.Invoke(false);
        }
    
        private void OnChooseEightDirectionClicked(bool isOn)
        {
            if (isOn)
                ChooseEightDirectionsRequested?.Invoke(true);
        }
    
        private void OnEnable()
        {
            setFourDirectionsToggle.isOn = true;
            setEightDirectionsToggle.isOn = false;
            
            newGridButton.onClick.AddListener(OnNewGridButtonClicked);
            editGridButton.onClick.AddListener(OnEditGridButtonClicked);
            findPathButton.onClick.AddListener(OnFindPathButtonClicked);
        
            setFourDirectionsToggle.onValueChanged.AddListener(OnChooseFourDirectionClicked);
            setEightDirectionsToggle.onValueChanged.AddListener(OnChooseEightDirectionClicked);
        }

        private void OnDisable()
        {
            newGridButton.onClick.RemoveListener(OnNewGridButtonClicked);
            editGridButton.onClick.RemoveListener(OnEditGridButtonClicked);
            findPathButton.onClick.RemoveListener(OnFindPathButtonClicked);
        
            setFourDirectionsToggle.onValueChanged.RemoveListener(OnChooseFourDirectionClicked);
            setEightDirectionsToggle.onValueChanged.RemoveListener(OnChooseEightDirectionClicked);
        }
    }
}
