using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSpacingScript : MonoBehaviour {

    public GridLayoutGroup layout;
    public Transform[] children;
    public int activeChildren;
    public int rowLimit;
    public int rowLength;
    public int manualRowLimit = -1;
	// Use this for initialization
	void Start () {
        layout = this.gameObject.GetComponent<GridLayoutGroup>();

        layout.spacing = new Vector2(0, 0);
        
        children = GetComponentsInChildren<Transform>();
        activeChildren = 0;

        if (manualRowLimit > 1)
            rowLimit = manualRowLimit;
        else if (this.name == "HandPanel")
            rowLimit = 9;
        else if (this.name == "ExtraPanel")
            rowLimit = 8;
        else
            rowLimit = 6;

        rowLength = rowLimit * (int)layout.cellSize.x;

    }
	
	// Update is called once per frame
	void Update () {
        int activeChildCounter = 0;

        if (GetComponentsInChildren<Transform>().Length != children.Length)
            children = GetComponentsInChildren<Transform>();

        for (int i = 1; i < children.Length; i++)
        {
            if (children[i].gameObject.activeSelf)
                activeChildCounter++;
        }
        activeChildren = activeChildCounter;
        if (activeChildren < rowLimit && activeChildren != 0)
        {
            layout.padding.left = (rowLimit - activeChildren) * (int)(layout.cellSize.x / 2.0f);
            layout.padding.right = layout.padding.left;
            layout.spacing = new Vector2(0, 0);
        }
        else
        {
            layout.padding.left = 0;
            layout.padding.right = 0;
            layout.spacing = new Vector2(-1.0f * (layout.cellSize.x - (layout.cellSize.x * (1.0f / (((float)activeChildren * layout.cellSize.x) / (float)rowLength)))), 0);
            //Alternate card spacing formula
            //float extraSpace = ((float) activeChildren * layout.cellSize.x) - (float) rowLength;
            //if (extraSpace > 0)
            //    layout.spacing = new Vector2(-1.0f * (extraSpace / (float) activeChildren), 0.0f);

        }



    }
}
