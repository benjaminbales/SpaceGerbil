using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 0.1f;
    
    void Update()
    {
        bool up = Input.GetKey("up");
        bool down = Input.GetKey("down");
        bool left = Input.GetKey("left");
        bool right = Input.GetKey("right");

        bool w = Input.GetKey("w");
        bool s = Input.GetKey("s");
        bool a = Input.GetKey("a");
        bool d = Input.GetKey("d");

        if (up || w)    transform.Translate(transform.up * sensitivity);
        if (down || s)  transform.Translate(-transform.up * sensitivity);
        if (left || a)  transform.Translate(-transform.right * sensitivity);
        if (right || d) transform.Translate(transform.right * sensitivity);
    }
}
