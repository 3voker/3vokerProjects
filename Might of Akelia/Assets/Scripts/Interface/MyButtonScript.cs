using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MyButtonScript : Button {

    // Use this for initialization

  
    public EventSystem eventSystem;

    protected override void Awake()
    {
        base.Awake();
        eventSystem = GetComponent<EventSystemProvider>().eventSystem;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Selection tracking
        if (IsInteractable() && navigation.mode != Navigation.Mode.None)
            eventSystem.SetSelectedGameObject(gameObject, eventData);

        base.OnPointerDown(eventData);
    }


    public override void Select()
    {
        if (eventSystem.alreadySelecting)
            return;

        eventSystem.SetSelectedGameObject(gameObject);
    }
}
