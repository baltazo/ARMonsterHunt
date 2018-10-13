using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class Monster
{
    public string Name { get; set; }
    public string PrefabName { get; set; }
    public string Color { get; set; }
    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Life { get; set; }

    public Monster(string name, string prefabName, string color, int strength, int intel, int life)
    {
        Name = name;
        PrefabName = prefabName;
        Color = color;
        Strength = strength;
        Intelligence = intel;
        Life = life;
    }
}

[Serializable]
public class MonsterData
{
    public List<Monster> unlockedMonsters = new List<Monster>();
}

public class MonsterCollector : MonoBehaviour {

    public static MonsterCollector sharedInstance = null;

    public GameObject[] blackMonsters;
    public GameObject[] whiteMonsters;
    public GameObject[] redMonsters;
    public GameObject[] blueMonsters;
    public GameObject[] greenMonsters;
    public GameObject[] yellowMonsters;
    public GameObject[] purpleMonsters;
    public GameObject[] aquaMonsters;
    public GameObject[] greyMonsters;

    public List<Monster> unlockedMonsters = new List<Monster>();
    public Dictionary<string, GameObject> monsterPrefabsList = new Dictionary<string, GameObject>();

    private GameObject[][] colorsOfMonsters = new GameObject[9][];
    private GameObject prefabToAdd;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() 
    {
        LoadList();
        foreach(Monster monster in unlockedMonsters)
        {
            Debug.Log(monster.PrefabName);
        }
    }

    private void Start()
    {
        colorsOfMonsters[0] = blackMonsters;
        colorsOfMonsters[1] = whiteMonsters;
        colorsOfMonsters[2] = redMonsters;
        colorsOfMonsters[3] = blueMonsters;
        colorsOfMonsters[4] = greenMonsters;
        colorsOfMonsters[5] = yellowMonsters;
        colorsOfMonsters[6] = purpleMonsters;
        colorsOfMonsters[7] = aquaMonsters;
        colorsOfMonsters[8] = greyMonsters;

        foreach (Monster monster in unlockedMonsters)
        {

            foreach (GameObject[] monsterColor in colorsOfMonsters)
            {
                for(int i = 0; i < monsterColor.Length; i++)
                {
                    if(monster.Name == monsterColor[i].name)
                    {
                        Debug.Log("Adding to list :" + monsterColor[i].name);
                        prefabToAdd = monsterColor[i];
                        break;
                    }
                }
            }

            UpdateDictionnary(monster, prefabToAdd);
        }

    }

    public void LoadList()
    {
        if(File.Exists(Application.persistentDataPath + "/monsters.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/monsters.data", FileMode.Open);

            MonsterData data = (MonsterData)bf.Deserialize(file);
            file.Close();

            unlockedMonsters = data.unlockedMonsters;
        }
    }

    private void OnDisable()
    {
        SaveList();
    }

    public void SaveList()
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/monsters.data", FileMode.OpenOrCreate);

        MonsterData data = new MonsterData();
        data.unlockedMonsters = unlockedMonsters;

        bf.Serialize(file, data);
        file.Close();

    }

    

    public GameObject MonsterChooser(Color scannedColor) // This chooses the monster to be summoned
    {

        if(scannedColor.r >= 0.65f) // If the r value is higher than 0.65f...
        {
            if(scannedColor.g >= 0.65f) // ... and the g value is higher than 0.65f...
            {
                if(scannedColor.b >= 0.65f) // ... and the b value is higher than 0.65f...
                {
                    Debug.Log("Summoned a White monster!");
                    GameObject summonedMonster = whiteMonsters[UnityEngine.Random.Range(0, whiteMonsters.Length)];
                    AddMonsterToList(summonedMonster, "white");
                    return summonedMonster; // Then the color is assumed to be white
                }
                else // If the r and g values are higher than 0.65f, but not the b value...
                {
                    Debug.Log("Summoned a Yellow monster!");
                    GameObject summonedMonster = yellowMonsters[UnityEngine.Random.Range(0, yellowMonsters.Length)];
                    AddMonsterToList(summonedMonster, "yellow");
                    return summonedMonster; // Then the color is assumed to be yellow
                }
            }
            else if(scannedColor.b >= 0.65f) // If the r and b values are higher than 0.65f, but not the g value...
            {
                Debug.Log("Summoned a Purple monster!");
                GameObject summonedMonster = purpleMonsters[UnityEngine.Random.Range(0, purpleMonsters.Length)];
                AddMonsterToList(summonedMonster, "purple");
                return summonedMonster; // Then the color is assumed to be purple
            }
            else // If the r value is higher than 0.65f but not the g and b values...
            {
                Debug.Log("Summoned a Red monster!");
                GameObject summonedMonster = redMonsters[UnityEngine.Random.Range(0, redMonsters.Length)];
                AddMonsterToList(summonedMonster, "red");
                return summonedMonster; // Then the color is assumed to be purple // Then the color is assumed to be red
            }
        }
        else if(scannedColor.g >= 0.65f) // If the r value is lower than 0.65f, but the g value is higher than 0.65f...
        {
            if(scannedColor.b >= 0.65f) // If the r value is lower than 0.65f, but the g and b values are highe than 0.65f...
            {
                Debug.Log("Summoned a Aqua monster!");
                GameObject summonedMonster = aquaMonsters[UnityEngine.Random.Range(0, aquaMonsters.Length)];
                AddMonsterToList(summonedMonster, "aqua");
                return summonedMonster; // Then the color is assumed to be purple // Then the color is assumed to be aqua
            }
            else // If only the g value is higher than 0.65f...
            {
                Debug.Log("Summoned a Green monster!");
                GameObject summonedMonster = greenMonsters[UnityEngine.Random.Range(0, greenMonsters.Length)];
                AddMonsterToList(summonedMonster, "green");
                return summonedMonster; // Then the color is assumed to be purple // Then the color is assumed to be green
            }
        }
        else if(scannedColor.b >= 0.65f) // If only the b value is higher than 0.65f...
        {
            Debug.Log("Summoned a Blue monster!");
            GameObject summonedMonster = blueMonsters[UnityEngine.Random.Range(0, blueMonsters.Length)];
            AddMonsterToList(summonedMonster, "blue");
            return summonedMonster; // Then the color is assumed to be purple // Then the color is assumed to be green
        }
        else if(scannedColor.r + scannedColor.g + scannedColor.b <= 1f) // If all the colors are below 0.33f...
        {
            Debug.Log("Summoned a Black monster!");
            GameObject summonedMonster = blackMonsters[UnityEngine.Random.Range(0, blackMonsters.Length)];
            AddMonsterToList(summonedMonster, "black");
            return summonedMonster; // Then the color is assumed to be purple // Then the color is assumed to be black
        }
        else // In all other cases...
        {
            Debug.Log("Summoned a Grey monster!");
            GameObject summonedMonster = greyMonsters[UnityEngine.Random.Range(0, greyMonsters.Length)];
            AddMonsterToList(summonedMonster, "grey");
            return summonedMonster; // Then the color is assumed to be purple // Then the color is assumed to be grey
        }
    }

    public void AddMonsterToList(GameObject monster, string monsterColor) //This adds the summonedMonster to the list of unlockedMonsters
    {
        string name = monster.name;

        while (monsterPrefabsList.ContainsKey(monster.name)) //Updates the monster name so that there are no monsters with the same name
        {
            int variation = 001;
            string monsterName = monster.name + variation;
            
            name = monsterName;
            variation += 001;
        }

        string prefabName = monster.name;

        string color = monsterColor;

        int[] stats = new int[3];
        for (int i = 0; i < 100; i++) // Distributes 100 points in the three attributes
        {
            stats[UnityEngine.Random.Range(0, stats.Length)] += 1;
        }

        int str = stats[0];
        int intel = stats[1];
        int life = stats[2];


        Monster newMonster = new Monster(name, prefabName, color, str, intel, life);
        unlockedMonsters.Add(newMonster);
        UpdateDictionnary(newMonster, monster);

    }

    private void UpdateDictionnary(Monster monster, GameObject monsterPrefab)
    {
        monsterPrefabsList.Add(monster.Name, monsterPrefab);
    }

    public void DebugClearList()
    {
        unlockedMonsters.Clear();
        SaveList();
    }

}
