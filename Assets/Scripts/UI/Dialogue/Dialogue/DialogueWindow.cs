using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class DialogueWindow : MonoBehaviour
{
    [SerializeField] private DialoguePlate _dialoguePlate;
    [Header("Portals")]
    [SerializeField] private PortalTransmitter[] _trackingPortals;
    [Header("Dialogues")]
    [SerializeField] private Dialogue[] _dialogues;
    [Header("DefaultDialogue")]
    [SerializeField] private Dialogue _defaultDialogue;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _text;

    private int _currentLevelNumber = 0;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        SetDataToFields();
    }

    private void OnEnable()
    {
        _dialoguePlate.DialoguePlateMouseDown += MakeDialogVisible;
        foreach (var portal in _trackingPortals)
        {
            portal.PlayerTeleportedToSpaceShip += EnableNextDialog;
        }
    }

    private void OnDisable()
    {
        _dialoguePlate.DialoguePlateMouseDown -= MakeDialogVisible;
        foreach (var portal in _trackingPortals)
        {
            portal.PlayerTeleportedToSpaceShip -= EnableNextDialog;
        }
    }

    private void EnableNextDialog()
    {
        _currentLevelNumber += 1;
        if (_currentLevelNumber <= _trackingPortals.Length - 1)
        {
            SetDataToFields();
        }
        else
        {
            _icon.sprite = _defaultDialogue.Icon;
            _text.text = _defaultDialogue.Text;
        }
    }

    private void SetDataToFields()
    {
        _icon.sprite = _dialogues[_currentLevelNumber].Icon;
        _text.text = _dialogues[_currentLevelNumber].Text;
    }

    private void MakeDialogVisible()
    {
        _canvasGroup.alpha = 1;
    }
}