using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Create new dialogue", order = 51)]
public class Dialogue : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _text;

    public Sprite Icon => _icon;
    public string Text => _text;
}