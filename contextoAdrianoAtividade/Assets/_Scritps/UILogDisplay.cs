using UnityEngine;
using TMPro;

public class UILogDisplay : MonoBehaviour
{
    public static UILogDisplay Instance;
    public TextMeshProUGUI logText;
    private string allLogs = "";

    void Awake()
    {
        Instance = this;
    }

    public void AddLog(string message)
    {
        allLogs += message + "\n";
        logText.text = allLogs;
    }

    public void ClearLog()
    {
        allLogs = "";
        logText.text = "";
    }
}