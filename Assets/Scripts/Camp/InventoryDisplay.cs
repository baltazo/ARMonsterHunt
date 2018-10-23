using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour {

    public GameObject inventoryPanel;

    // Array[0] is Strength
    // Array[1] is Intelligence
    // Array[2] is Life
    public Text[] inventoryPiecesDisplay;
    public Text[] inventoryItemsDisplay;
    public Button[] inventoryPiecesButton;

    public Image[] inventoryTabAlert;

    private void Start()
    {
        for (int i = 0; i < Inventory.sharedInstance.inventoryPieces.Length; i++)
        {
            int numberOfPieces = Inventory.sharedInstance.inventoryPieces[i];

            inventoryPiecesDisplay[i].text = numberOfPieces.ToString();
            inventoryItemsDisplay[i].text = Inventory.sharedInstance.inventoryItems[i].ToString();

            if (numberOfPieces >= 10)
            {
                inventoryPiecesButton[i].interactable = true;
                inventoryTabAlert[i].color = Color.green;
            }
            else
            {
                inventoryTabAlert[i].color = Color.white;
            }
        }
    }

    public void CombineItems(int category)
    {
        int numberOfPieces = Inventory.sharedInstance.inventoryPieces[category];
        int numberOfItems = Inventory.sharedInstance.inventoryItems[category];

        numberOfPieces -= 10;
        numberOfItems++;

        inventoryPiecesDisplay[category].text = numberOfPieces.ToString();
        inventoryItemsDisplay[category].text = numberOfItems.ToString();

        if (numberOfPieces < 10)
        {
            inventoryPiecesButton[category].interactable = false;
            inventoryTabAlert[category].color = Color.white;
        }

        Inventory.sharedInstance.inventoryPieces[category] = numberOfPieces;
        Inventory.sharedInstance.inventoryItems[category] = numberOfItems;

        Inventory.sharedInstance.SaveInventory();
    }

    public void ShowHideInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

}
