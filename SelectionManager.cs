using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SelectionManager : MonoBehaviour
{


    public static SelectionManager Instance { get; set; }

    private Camera cam;


    public Transform player; // Assign your player in the Inspector
    public GameObject interaction_Info_UI;
    TMP_Text interaction_text;




    public bool onTarget;

    public GameObject selectedObject;




    private void Start()
    {
        Debug.Log("Camera.main is: " + Camera.main);
        onTarget = false;
        cam = Camera.main;
        interaction_text = interaction_Info_UI.GetComponentInChildren<TMP_Text>();
    }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
    }


    void Update()
    {
        if (cam == null) Debug.LogError("cam is NULL");
        if (interaction_text == null) Debug.LogError("interaction_text is NULL");
        if (player == null) Debug.LogError("player is NULL");


        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("Hit: " + hit.transform.name);

            var selectionTransform = hit.transform;
            var interactable = selectionTransform.GetComponentInParent<InteractableObject>();


            if (interactable != null)
            {
                if (interactable.IsPlayerInRange(player))
                {
                    interaction_text.text = interactable.GetItemName();
                    interaction_Info_UI.SetActive(true);
                    selectedObject = interactable.gameObject;
                    onTarget = true;
                }
                else
                {
                    onTarget = false;
                    interaction_Info_UI.SetActive(false);
                }
                Debug.Log("Distance to object: " + Vector3.Distance(player.position, interactable.transform.position));
            }
            else
            {
                onTarget = false;
                Debug.Log("No Interactable on hit object or parents.");
                interaction_Info_UI.SetActive(false);
            }
        }
        else
        {
            onTarget = false;
            Debug.Log("Raycast hit nothing.");
            interaction_Info_UI.SetActive(false);
        }
    }



}