using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RainbowColor : MonoBehaviour
{

    public float speed;
    public Image image;
    private Color color;

    private void Awake()
    {
        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
        ColorHSV hsv = new ColorHSV(image.color);
        hsv.h += speed * Time.deltaTime;
        image.color = ColorHSV.ToColor(hsv);
    }
}
