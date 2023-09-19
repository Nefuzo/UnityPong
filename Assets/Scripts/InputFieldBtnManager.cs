using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldBtnManager : MonoBehaviour
{
    private TMP_InputField inputField;
    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }
    void OnEnable()
    {
        inputField.text = string.Empty;
    }
}
