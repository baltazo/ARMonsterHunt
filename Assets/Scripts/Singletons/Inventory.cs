using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class InventoryData
{
    public int[] inventoryPieces;
    public int[] inventoryItems;
}

public class Inventory : MonoBehaviour {

    public static Inventory sharedInstance = null;

    // inventoryPieces[0] is Strength
    // inventoryPieces[1] is Intelligence
    // inventoryPieces[2] is Life
    public int[] inventoryPieces = new int[3];
    public int[] inventoryItems = new int[3];

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

    private void LoadIntentory()
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "inventory.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "inventory.data", FileMode.Open);

            InventoryData data = (InventoryData)bf.Deserialize(file);
            file.Close();

            inventoryPieces = data.inventoryPieces;
            inventoryItems = data.inventoryItems;
        }
    }

    private void SaveInventory()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "inventory.data", FileMode.OpenOrCreate);

        InventoryData data = new InventoryData();

        data.inventoryPieces = inventoryPieces;
        data.inventoryItems = inventoryItems;

        bf.Serialize(file, data);
        file.Close();
    }

    public string InventoryChooser(int category)
    {
        string messageToReturn;

        if (category == 0) // Category 0 is the strength item
        {
            messageToReturn = "You found a Punching Bag piece!";
            inventoryPieces[category]++;
            return messageToReturn;
        }
        else if (category == 1) // Category 1 is the intelligence item
        {
            messageToReturn = "You found a Book piece!";
            inventoryPieces[category]++;
            return messageToReturn;
        }
        else // Category 2 is the intelligence item
        {
            messageToReturn = "You found a Food piece!";
            inventoryPieces[category]++;
            return messageToReturn;
        }

    }

}
