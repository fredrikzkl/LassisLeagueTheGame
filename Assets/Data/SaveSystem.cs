using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{

    public static string RulesPath = Application.persistentDataPath + "/husregler.lasaron";
    public static string SystemSettingsPath = Application.persistentDataPath + "/systemSettings.lasaron";
    public static string VSModeSettingsPath = Application.persistentDataPath + "/vsmode.lasaron";

    public static void SaveVSModeSettings(VSModeSettingsData vsModeData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = VSModeSettingsPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, vsModeData);
        stream.Close();
    }

    public static VSModeSettingsData LoadVsModeSettings()
    {
        string path = VSModeSettingsPath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            VSModeSettingsData data = formatter.Deserialize(stream) as VSModeSettingsData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("VSModeSettings does not exist - loading standard rules");
            return new VSModeSettingsData().Standard();
        }
    }



    public static void SaveRules(RulesData rulesData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = RulesPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, rulesData);
        stream.Close();
    }

    public static RulesData LoadRules()
    {
        string path = RulesPath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            RulesData data = formatter.Deserialize(stream) as RulesData;
            stream.Close();


            Debug.Log(data.ToString());

            return data;
        }
        else
        {
            Debug.Log("Rule file does not exist - loading standard rules");
            return new RulesData().GetStandardRules();
        }
    }

    public static void SaveSystemSettings(SystemSettingsData systemSettings)
    {
        string path = SystemSettingsPath;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, systemSettings);
        stream.Close();
    }

    public static SystemSettingsData LoadSystemSettings()
    {
        string path = SystemSettingsPath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SystemSettingsData data = formatter.Deserialize(stream) as SystemSettingsData;
            stream.Close();


            Debug.Log(data.ToString());

            return data;
        }
        else
        {
            Debug.Log("SystemSettingsFile not found - Returns a standard");
            SystemSettingsData newSettings = new SystemSettingsData();
            newSettings.ApplyStandardSettings();
            return newSettings;
        }
    }




}
