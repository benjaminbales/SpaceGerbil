using UnityEngine;
using UnityEngine.UI;

public class FindGirbel : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform girbel;
    public Transform cam;
    public Text Icon;
    public Color iconColor = Color.green; 


    public float dist = 15f;
    public float lerpBetween = 5f;


    //private bool hasGirbelLeftViewPort = false;

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        if (girbel == null)
        {
            Debug.LogError("This gameobject does not have girbel assigned! Disabling!");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // make the rotation of this gameobject to face at the girbel at all times
        Quaternion rot = Quaternion.LookRotation(cam.position - girbel.position, Vector3.forward);
        rot.x = 0;
        rot.y = 0;
        this.transform.rotation = rot;
    }

    void LateUpdate()
    {
        // calculate the distance between the gerbil and the camera and then make the object visible to see when it goes out of those distance range. 
        Vector2 _cam = cam.position;
        Vector2 _gerbil = girbel.position;

        float _d = Vector2.Distance(_cam, _gerbil);
        float i = Mathf.Clamp01((_d - dist) / (dist + lerpBetween));
        Icon.color = Color.Lerp(Color.clear, iconColor, i);
    }
}
