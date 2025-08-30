using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject Inventory;
    private bool menuActivated;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (menuActivated)
            {
                // ?óng Inventory
                Time.timeScale = 1f;
                Inventory.SetActive(false);
                menuActivated = false;
            }
            else
            {
                // M? Inventory
                Time.timeScale = 0f; // d?ng game khi m? (n?u mu?n)
                Inventory.SetActive(true);
                menuActivated = true;
            }
        }
    }
}
