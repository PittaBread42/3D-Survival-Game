using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CraftingSystem : MonoBehaviour
{

    public GameObject CraftingScreenUI;
    public GameObject CraftingToolsScreenUI;

    public List<string> InventoryItemList = new List<string>();

    //Category Buttons
    Button toolsButton;

    //Craft Buttons
    Button craftAxeButton;

    //Requirement Text
    TMP_Text AxeReq1, AxeReq2;

    public bool isOpenCraft;

    //All Blueprints
    public Blueprint AxeBLP = new Blueprint("Axe", 2, "Stone", 3, "Stick", 3);

    public static CraftingSystem Instance { get; set; }


    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;

        }
       
        Instance = this;

        CraftingScreenUI.SetActive(false);
        CraftingToolsScreenUI.SetActive(false);
    
    }
    


    void Start()
    {

        isOpenCraft = false;

        toolsButton = CraftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsButton.onClick.AddListener(delegate { OpenCraftingTools(); });

        //Axe
        AxeReq1 = CraftingToolsScreenUI.transform.Find("Axe").transform.Find("Req1").GetComponent<TMP_Text>();
        AxeReq2 = CraftingToolsScreenUI.transform.Find("Axe").transform.Find("Req2").GetComponent<TMP_Text>();

        craftAxeButton = CraftingToolsScreenUI.transform.Find("Axe").transform.Find("Craft").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
    }

    void OpenCraftingTools()
    {
        CraftingScreenUI.SetActive(false);
        CraftingToolsScreenUI.SetActive(true);


    }

   

    void CraftAnyItem(Blueprint blueprintToCraft)
    {

        InventorySystem.Instance.AddToInventory(blueprintToCraft.ItemName);

        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
        }

        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);
        }


        StartCoroutine(calculate());

        RefreshNeededItems();



    }

    public IEnumerator calculate()
    {

        yield return new WaitForSeconds(1f);

        InventorySystem.Instance.ReCalculateList();
    }

    void Update()
    {

        RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C)&& !isOpenCraft)
        {
            CraftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpenCraft = true;

        }
        else if (Input.GetKeyDown(KeyCode.C)&& isOpenCraft)
        {
            CraftingScreenUI.SetActive(false);
            CraftingToolsScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            isOpenCraft = false;
        }

      
        
    }




    private void RefreshNeededItems()
    {

        int stone_count = 0;
        int stick_count = 0;

        InventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in InventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;

                    break;

                case "Stick":
                    stick_count += 1;

                    break;


            }

        }

        //----AXE----//

        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3)
        {

            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }
    }
}
