using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{

    public static string RulesPath = Application.persistentDataPath + "/husregler.lasaron";

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
   
}
