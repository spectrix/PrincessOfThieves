﻿using UnityEngine;
using System.Collections;

public class TresureChestController : JDMappableObject, InteractiveObject {

    public Sprite openedSprite;
    public Sprite closedSprite;
    public Sprite highlightedSprite;
    bool opened = false;
    [SerializeField]
    GameObject contents;
    SpriteRenderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        if (!opened)
        {
			UIManager.Instance.HideInteraction ();
            GameManager.Instance.Player.AddItem(contents);
			myRenderer.color = Color.white;
            myRenderer.sprite = openedSprite;
            opened = true;
			enabled = false;
        }
    }

    public void Highlight()
    {
        if (!opened)
        {
            UIManager.Instance.ShowInteraction("Open");
			myRenderer.color = Color.blue;
        }
    }

    public void Dehighlight()
    {
        if (!opened)
        {
			UIManager.Instance.HideInteraction ();
			myRenderer.color = Color.white;
        }
    }
}
