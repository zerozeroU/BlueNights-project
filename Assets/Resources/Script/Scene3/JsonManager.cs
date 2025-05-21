using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class JsonManager : MonoBehaviour
{
    private static JsonManager _instance;
    public static JsonManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<JsonManager>( );
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }
    private UnitDataPath UnitData_Update;
    private void Awake()
    {
        UnitData_Update = new UnitDataPath();
    }
     public List<UnitData> LoadUnitData()
    {
        string path="";
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "SelectScene")
        {
             path = "/UnitData.json";
        }
        else if(currentScene.name == "BattleScene")
        {
             path = "/SelectedUnitData.json";
        }
        else { Debug.Log("scene mismatch"); return null; }

        FileStream stream = new FileStream(Application.dataPath + path, FileMode.Open);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        List<UnitData> UnitDataList = JsonConvert.DeserializeObject<List<UnitData>>(jsonData);

        if (UnitDataList != null) { Debug.Log("UnitData Load Complete"); }

        return UnitDataList;
    }
    public void ExportSlotUnitData()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if(currentScene.name!= "SelectScene") { Debug.Log("Not SelectScene"); return; }

        GameObject[] slotSelectUnit = GameObject.FindGameObjectsWithTag("Slot");
        List<UnitData> unitData = new List<UnitData>();
        foreach (GameObject slot in slotSelectUnit) 
        {
            UnitData dataTemp = slot.GetComponent<Slot>().SlotUnitData;
            if (dataTemp != null)
            {
                unitData.Add(dataTemp);
                Debug.Log("unitData.Name : " + dataTemp.Name);
            }
        }

        if (unitData.Count == 0) { Debug.Log("UnitData Zero"); return; }
        Debug.Log("unitData.Count : "+unitData.Count);


        string path = "/SelectedUnitData.json";

        File.Delete(Application.dataPath + path);
        FileStream stream = new FileStream(Application.dataPath + path, FileMode.OpenOrCreate);
        string jsonData = JsonConvert.SerializeObject(unitData);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        stream.Write(data, 0, data.Length);
        stream.Close();

        Debug.Log("Export to SlotUnitData Complete");
    }
}

public class UnitData
{
    public string Name;
    public string EnName;
    public string Position;
    public bool[,] Area;
    public int Cost;
    public int Hp;
    public int Sp;
    public int Atk;
    public int Def;
    public float Dex;
}
public class UnitDataPath
{
    public UnitDataPath()
    {
        Unit_Attack_Field_Data_Path();
    }


    bool[,] aris;
    bool[,] Aru;
    bool[,] Asuna;
    bool[,] Azusa;
    bool[,] Eimi;
    bool[,] Chise;
    bool[,] Haruna;
    bool[,] Iori;
    bool[,] Izumi;
    bool[,] Izuna;
    bool[,] Kirino;
    bool[,] Koharu;
    bool[,] Kotori;
    bool[,] Maki;
    bool[,] Marina;
    bool[,] Midori;
    bool[,] Miyako;
    bool[,] Neru;
    bool[,] Sakurako;
    bool[,] Zunko;
    void Unit_Attack_Field_Data_Path()
    {
        aris = new bool[,]
       {
            {true,true,true,true,true,true,false,false,false},
            {true,true,true,true,true,true,false,false,false},
            {true,true,true,true,true,true,false,false,false},
            {true,true,true,true,true,true,false,false,false},

            {true,true,true,true,true,true,false,false,false},

            {true,true,true,true,true,true,false,false,false},
            {true,true,true,true,true,true,false,false,false},
            {true,true,true,true,true,true,false,false,false},
            {true,true,true,true,true,true,false,false,false}
       };

        Aru = new bool[,]
        {
            {true,true,true,false,false,false,false},
            {true,true,true,true,false,false,false},
            {true,true,true,true,true,false,false},

            {true,true,true,true,true,true,false},

            {true,true,true,true,true,false,false},
            {true,true,true,true,false,false,false},
            {true,true,true,false,false,false,false}
        };
        Asuna = new bool[,]
        {
            {false,false,false,false,false},
            {true,true,true,true,false},

            {true,true,true,true,false},

            {true,true,true,true,false},
            {false,false,false,false,false}
        };
        Azusa = new bool[,]
        {
            {false,false,false,false,false},
            {true,true,true,true,false},

            {true,true,true,true,true},

            {true,true,true,true,false},
            {false,false,false,false,false}
        };

        Eimi = new bool[,]
        {
            {true,false,false},

            {true,true,true},

            {true,false,false}
        };

        Chise = new bool[,]
        {
            {true,false,false,false,false,false,false},
            {true,true,false,false,false,false,false},
            {true,true,true,false,false,false,false},

            {true,true,true,true,false,false,false},

            {true,true,true,false,false,false,false},
            {true,true,false,false,false,false,false},
            {true,false,false,false,false,false,false}
        };
        Haruna = new bool[,]
        {
            {false,false,false,false,false,false,false},
            {true,true,true,true,true,false,false},
            {true,true,true,true,true,false,false},

            {true,true,true,true,true,true,false},

            {true,true,true,true,true,false,false},
            {true,true,true,true,true,false,false},
            {false,false,false,false,false,false,false}
        };

        Iori = new bool[,]
        {
            {true,true,true,true,true,false,false},
            {true,true,true,true,true,false,false},
            {true,true,true,true,true,true,false},

            {true,true,true,true,true,true,false},

            {true,true,true,true,true,true,false},
            {true,true,true,true,true,false,false},
            {true,true,true,true,true,false,false}
        };
        Izumi = new bool[,]
        {

            {false,false,false},
            {true,true,true},
            {false,false,false}

        };
        Izuna = new bool[,]
        {
            {false,false,false,false,false},
            {true,true,false,false,false},

            {true,true,true,true,false},

            {true,true,false,false,false},
            {false,false,false,false,false}
        };
        Kirino = new bool[,]
        {
            {true,true,false,false,false},
            {true,true,true,false,false},

            {true,true,true,true,false},

            {true,true,true,false,false},
            {true,true,false,false,false}
        };
        Koharu = new bool[,]
        {
            {true,true,false,false,false},
            {true,true,true,true,false},

            {true,true,true,true,false},

            {true,true,true,true,false},
            {true,true,false,false,false}
        };
        Kotori = new bool[,]
        {
            {true,true,true},

            {true,true,true},

            {true,true,true}
        };

        Maki = new bool[,]
        {
            {true,true,true,true,false},
            {true,true,true,true,false},

            {true,true,true,true,false},

            {true,true,true,true,false},
            {true,true,true,true,false}
        };
        Marina = new bool[,]
        {
            {false,false,false,false,false},
            {true,true,true,true,false},

            {true,true,true,true,false},

            {true,true,true,true,false},
            {false,false,false,false,false}
        };
        Midori = new bool[,]
        {
            {true,true,false,false,false},
            {true,true,true,true,false},

            {true,true,true,true,false},

            {true,true,true,true,false},
            {true,true,false,false,false},
        };
        Miyako = new bool[,]
        {
            {false,false,false,false,false},
            {true,true,false,false,false},

            {true,true,true,true,false},

            {true,true,false,false,false},
            {false,false,false,false,false},
        };
        Neru = new bool[,]
        {
            {false,false,false,false,false},
            {true,true,true,false,false},

            {true,true,true,true,false},

            {true,true,true,false,false},
            {false,false,false,false,false},
        };
        Sakurako = new bool[,]
        {
            {true,true,true,false,false},
            {true,true,true,false,false},

            {true,true,true,true,false},

            {true,true,true,false,false},
            {true,true,true,false,false},
        };

        Zunko = new bool[,]
        {
            {false,false,false,false,false},
            {true,true,true,true,false},

            {true,true,true,true,false},

            {true,true,true,true,false},
            {false,false,false,false,false},
        };


        DataSave();
        Debug.Log(" UnitData Save Complete ");
    }
    void DataSave()
    {
        string path = "/UnitData.json";

        File.Delete(Application.dataPath + path);
        FileStream stream = new FileStream(Application.dataPath + path, FileMode.OpenOrCreate);
        List<UnitData> UnitData = new List<UnitData>
        {
        new UnitData { Name = "아리스", EnName="Aris",Position="Striker",Area = aris, Cost=14, Hp= 200, Sp=100, Atk=150, Def=30, Dex=1f},
        new UnitData { Name = "아루", EnName="Aru",Position="Striker",Area = Aru,Cost=10, Hp= 100, Sp=100, Atk=75, Def=10, Dex=1.5f},
        new UnitData { Name = "아스나", EnName="Asuna",Position="Striker",Area = Asuna,Cost=7, Hp= 150, Sp=100, Atk=20, Def=20, Dex=4},
        new UnitData { Name = "아즈사", EnName="Azusa",Position="Striker",Area = Azusa,Cost=7, Hp= 150, Sp=100, Atk=30, Def=23, Dex=4},
        new UnitData { Name = "에이미", EnName="Eimi",Position="Guard",Area = Eimi,Cost=2, Hp= 300, Sp=100, Atk=10, Def=60, Dex=1},
        new UnitData { Name = "치세", EnName="Chise",Position="Striker",Area = Chise,Cost=5, Hp= 150, Sp=100, Atk=35, Def=20, Dex=2},
        new UnitData { Name = "하루나", EnName="Haruna",Position="Striker",Area = Haruna,Cost=10, Hp= 100, Sp=100, Atk=100, Def=12, Dex=1.5f},
        new UnitData { Name = "이오리", EnName="Iori",Position="Striker",Area = Iori,Cost=8, Hp= 120, Sp=100, Atk=50, Def=22, Dex=3},
        new UnitData { Name = "이즈미", EnName="Izumi",Position="Guard",Area = Izumi,Cost=2, Hp= 300, Sp=100, Atk=10, Def=70, Dex=1},
        new UnitData { Name = "이즈나", EnName="Izuna",Position="Guard",Area = Izuna,Cost=4, Hp= 200, Sp=100, Atk=30, Def=45, Dex=4},
        new UnitData { Name = "키리노", EnName="Kirino",Position="Striker",Area = Kirino,Cost=4, Hp= 150, Sp=100, Atk=20, Def=20, Dex=2},
        new UnitData { Name = "코하루", EnName="Koharu",Position="Striker",Area = Koharu,Cost=4, Hp= 150, Sp=100, Atk=15, Def=40, Dex=2.5f},
        new UnitData { Name = "코토리", EnName="Kotori",Position="Guard",Area = Kotori,Cost=10, Hp= 200, Sp=100, Atk=20, Def=45, Dex=6},
        new UnitData { Name = "마키", EnName="Maki",Position="Striker",Area = Maki,Cost=10, Hp= 150, Sp=100, Atk=25, Def=30, Dex=6},
        new UnitData { Name = "마리나", EnName="Marina",Position="Guard",Area = Marina,Cost=7, Hp= 130, Sp=100, Atk=25, Def=25, Dex=4},
        new UnitData { Name = "미도리", EnName="Midori",Position="Striker",Area = Midori,Cost=6, Hp= 120, Sp=100, Atk=25, Def=30, Dex=3},
        new UnitData { Name = "미야코", EnName="Miyako",Position="Striker",Area = Miyako,Cost=5, Hp= 120, Sp=100, Atk=25, Def=20, Dex=3},
        new UnitData { Name = "네루", EnName="Neru",Position="Guard",Area = Neru,Cost=10, Hp= 200, Sp=100, Atk=20, Def=35, Dex=8},
        new UnitData { Name = "사쿠라코", EnName="Sakurako",Position="Striker",Area = Sakurako,Cost=7, Hp= 150, Sp=100, Atk=30, Def=20, Dex=4},
        new UnitData { Name = "준코", EnName="Zunko",Position="Striker",Area = Zunko,Cost=5, Hp= 140, Sp=100, Atk=25, Def=20, Dex=3},
       };
        string jsonData = JsonConvert.SerializeObject(UnitData);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        stream.Write(data, 0, data.Length);
        stream.Close();
    }
}

