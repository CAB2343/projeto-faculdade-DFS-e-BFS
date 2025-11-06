using UnityEngine;

public class LogRedirector : MonoBehaviour
{
    void OnEnable()
    {
        // Registra o evento que captura todos os Debug.Log
        Application.logMessageReceived += OnLogMessageReceived;
    }

    void OnDisable()
    {
        // Remove o evento ao desativar o objeto
        Application.logMessageReceived -= OnLogMessageReceived;
    }

    void OnLogMessageReceived(string logString, string stackTrace, LogType type)
    {
        // Mostra no UI se o UILogDisplay existir
        if (UILogDisplay.Instance != null)
        {
            string prefixo = "";
        switch (type)
        {
            case LogType.Warning: prefixo = "[WARN] "; break;
            case LogType.Error: prefixo = "[ERROR] "; break;
            case LogType.Assert: prefixo = "[ASSERT] "; break;
            case LogType.Exception: prefixo = "[EXC] "; break;
            default: prefixo = ""; break;
        }


            UILogDisplay.Instance.AddLog(prefixo + logString);
        }
    }
}
