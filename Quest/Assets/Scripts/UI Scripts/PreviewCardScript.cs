using UnityEngine;
using UnityEngine.UI;

public class PreviewCardScript : MonoBehaviour
{
    public string textureName { get; set; }
    private Button button;

    // Use this for initialization
    void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void ChangeTexture()
    {
        Debug.Log("PREVIEW CARD: " + textureName);
        button.image.overrideSprite = Resources.Load<Sprite>(textureName); ;
    }
}
