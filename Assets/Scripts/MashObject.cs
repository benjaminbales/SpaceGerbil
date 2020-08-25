using UnityEngine;

public class MashObject : MonoBehaviour
{
    // Start is called before the first frame update
    // we don't need a array of collection, we just need to use the transform.parent to manage collection of childs.
    public bool HasBeenSelected { get; set; }

    private void LateUpdate()
    {
        HasBeenSelected = false;
    }
}
