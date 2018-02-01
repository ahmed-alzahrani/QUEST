using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewCardScript : MonoBehaviour
{
    public string textureName { get; set; }
    private Button button;

    // Use this for initialization
    void Start()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void ChangeTexture()
    {
        button.image.overrideSprite = Resources.Load<Sprite>(textureName);
    }
}
