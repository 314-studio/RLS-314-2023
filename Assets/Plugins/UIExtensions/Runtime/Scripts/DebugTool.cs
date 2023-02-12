using UnityEngine;
public class DebugTool
{
    /// <summary>
    /// ��־
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Log(object message)
    {
        Debug.Log(message);
    }

    /// <summary>
    /// ��ɫ��־
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void LogWithHexColor(object message, string hexColor = "#c0ff80")
    {
        Debug.Log(HexColorMessage(message, hexColor));
    }



    /// <summary>
    /// ��־
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Log(object message, Object context = null)
    {
        Debug.Log(message, context);
    }

    /// <summary>
    /// ��־
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Log(object message, Color color)
    {
        Debug.Log(ColorMessage(message, color));
    }

    /// <summary>
    /// ��־
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void LogTrueFalse(object message, bool flag)
    {
        Debug.Log(ColorMessage(message, flag ? Color.green : Color.red));
    }


    /// <summary>
    /// ����
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Warning(object message)
    {
        Debug.LogWarning(message);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Warning(object message, Object context = null)
    {
        Debug.LogWarning(message);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Warning(object message, Color color)
    {
        Debug.LogWarning(ColorMessage(message, color));
    }



    /// <summary>
    /// ����
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Error(object message)
    {
        Debug.LogError(message);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Error(object message, Object context = null)
    {
        Debug.LogError(message);
    }


    /// <summary>
    /// ��־��ɫ
    /// </summary>
    /// <param name="message"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string ColorMessage(object message, Color color)
    {
        color = color == null ? Color.white : color;
        string msg = message.ToString();
        string colorTagStart = "<color=#{0}>";
        string colorTagEnd = "</color>";
        msg = string.Format(colorTagStart, ColorUtility.ToHtmlStringRGB(color)) + msg + colorTagEnd;
        return msg;
    }


    /// <summary>
    /// ֧�ָ��ָ�������־��ɫ
    /// </summary>
    /// <param name="message"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string HexColorMessage(object message, string hexColor)
    {

        string msg = message.ToString();
        string colorTagStart = "<color={0}>";
        string colorTagEnd = "</color>";
        msg = string.Format(colorTagStart, hexColor) + msg + colorTagEnd;
        return msg;
    }
}
