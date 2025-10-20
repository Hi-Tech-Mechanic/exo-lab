using System.Collections;
using TMPro;
using UnityEngine;

public class DynamicText : MonoBehaviour
{
    TextMeshProUGUI text;
    string[] messagesArray = { "System Error Codes (0-499) (0x0-0x1f3)",
        "System Error Codes (12000-15999) (0x2ee0-0x3e7f)",
        "ERROR_DS_NO_RIDS_ALLOCATED",
        "8208 (0x2010)",
        "ERROR_DS_CANT_MOD_OBJ_CLASS",
        "ERROR_DS_GC_NOT_AVAILABLE",
        "UNREFERENCED_PARAMETER(UserContext);",
        "SymEnumerateModules64(hProcess, EnumModules, NULL)"}; 

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(AddSymbolInText());
    }

    IEnumerator AddSymbolInText()
    {
        int rndElement = Random.Range(0, messagesArray.Length);

        foreach (char symbol in messagesArray[rndElement])
        {
            if (text.isTextOverflowing == true && text.text != string.Empty)
            {
                text.text = string.Empty;
            }
            text.text += symbol;

            yield return new WaitForSeconds(0.05f);
        }
        text.text += "\n\n";

        StartCoroutine(AddSymbolInText());
    }
}
