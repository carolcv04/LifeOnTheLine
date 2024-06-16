using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DebugLogger
{
    private static StreamWriter sw;

    public static void Initialize()
    {
        sw = new StreamWriter("debugLog.txt");
        sw.AutoFlush = true;
    }

    public static void Log(string message)
    {
        if (isNotInitialized())
            Initialize();
        
        sw.WriteLine(message);
    }

    private static bool isNotInitialized()
    {
        if (sw == null)
            return true;
        return false;
    }
}
