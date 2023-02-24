using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public delegate void GetFieldString(string s, int itemID); 

public class EditorField : MonoBehaviour
{
    [SerializeField] private TMP_Text title; public TMP_Text GetTitle { get { return title; }}
    [SerializeField] private TMP_InputField[] fields; public TMP_InputField[] GetFields { get { return fields; } }

    private const string validNum = ".-0123456789";

    public void SetUp(EditorRequestField request, GetFieldString stringMethod, string[] initValue, int id)
    {
        title.text = request.fieldName;
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i].text = initValue[i];
            if (request.requestType != RequestType.String)
            {
                AddNumConstraint(fields[i], 10, request.requestType == RequestType.Int);
            }

            int t = i, tid = id;
            fields[i].onValueChanged.RemoveAllListeners();
            fields[i].onValueChanged.AddListener(s => stringMethod(s, tid + t));
        }
    }

    private void AddNumConstraint(TMP_InputField field, int maxLen, bool isInt)
    {
        field.onValidateInput = (string text, int charIndex, char addedChar) => { return ValidateCharacter(validNum, addedChar, isInt); };
        field.characterLimit = maxLen;
    }

    private char ValidateCharacter(string validCharacters, char addedChr, bool isInt)
    {
        //Debug.Log(validCharacters + " " + addedChr + " " + validCharacters.IndexOf(addedChr));
        if (validCharacters.IndexOf(addedChr) != -1 && (!isInt || validCharacters.IndexOf(addedChr) != 0))
        {
            return addedChr;
        }
        else
        {
            Debug.Log("Nonvalid!");
            return '\0'; //Null Character
        }

    }
}
