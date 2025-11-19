using System;
using UnityEngine;
using UnityEngine.UI;

namespace AStar.Views
{
    public class EditPanelView : MonoBehaviour
    {
        [SerializeField] private Button finishEditButton;
        
        [SerializeField] private Toggle setStartToggle;
        [SerializeField] private Toggle setEndToggle;
        [SerializeField] private Toggle setWalkableToggle;
        [SerializeField] private Toggle setUnwalkableToggle;
        
        public Action<EGridEditorMode> ChangeEditModeRequested;
        public Action FinishEditRequested;
        
        private void OnFinishEditButtonClicked()
        {
            FinishEditRequested?.Invoke();
        }
        
        private void OnSetStartToggled(bool isOn)
        {
            if (isOn)
                ChangeEditModeRequested?.Invoke(EGridEditorMode.Start);
        }
        
        private void OnSetEndToggled(bool isOn)
        {
            if (isOn)
                ChangeEditModeRequested?.Invoke(EGridEditorMode.End);
        }
        
        private void OnSetWalkableToggled(bool isOn)
        {
            if (isOn)
                ChangeEditModeRequested?.Invoke(EGridEditorMode.Walkable);
        }
        
        private void OnSetUnwalkableToggled(bool isOn)
        {
            if (isOn)
                ChangeEditModeRequested?.Invoke(EGridEditorMode.Unwalkable);
        }
        
        private void OnEnable()
        {
            setStartToggle.isOn = true;
            setEndToggle.isOn = false;
            setWalkableToggle.isOn = false;
            setUnwalkableToggle.isOn = false;
            
            finishEditButton.onClick.AddListener(OnFinishEditButtonClicked);
            setStartToggle.onValueChanged.AddListener(OnSetStartToggled);
            setEndToggle.onValueChanged.AddListener(OnSetEndToggled);
            setWalkableToggle.onValueChanged.AddListener(OnSetWalkableToggled);
            setUnwalkableToggle.onValueChanged.AddListener(OnSetUnwalkableToggled);
        }
        
        private void OnDisable()
        {
            finishEditButton.onClick.RemoveListener(OnFinishEditButtonClicked);
            setStartToggle.onValueChanged.RemoveListener(OnSetStartToggled);
            setEndToggle.onValueChanged.RemoveListener(OnSetEndToggled);
            setWalkableToggle.onValueChanged.RemoveListener(OnSetWalkableToggled);
            setUnwalkableToggle.onValueChanged.RemoveListener(OnSetUnwalkableToggled);
        }
    }
}
