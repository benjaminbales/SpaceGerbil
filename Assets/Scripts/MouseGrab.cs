using UnityEngine;

//TODO: figure out why dragging lost object if too fast.
[RequireComponent(typeof(Rigidbody2D))]
public class MouseGrab : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    public bool isSelected = false;
    public Transform parent;
    public Transform dragObject;
    private static MouseGrab instance;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        instance = this;
    }

    public void BreakObject()
    {
        isSelected = false;
        parent = null;
        this.transform.parent = null;
        dragObject = null;
    }

    private void ZoomInOut()
    {
        float sensitivity = .1f;
        //print("Input.mouseScrollDelta: " + Input.mouseScrollDelta);
        cam.fieldOfView += Input.mouseScrollDelta.y * sensitivity;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 10f, 140f);
        //min 10 max 140
    }

    // Check and see if the mouse is over the gameobject (Works with the collider)
    private void OnMouseOver()
    {
        // hmmm
        isSelected = false;
        if (parent != null)
            parent.GetComponent<MashObject>().HasBeenSelected = false;
        // check and see if the left mouse button is down, while over the game object..
        if ( Input.GetMouseButtonDown(0) && dragObject != null )
        {
            dragObject = null;
            isSelected = false;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // make it true
            isSelected = true;
            if (parent != null)
                parent.GetComponent<MashObject>().HasBeenSelected = true;

            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Vector3 pos = GetWorldPositionOnPlane();
            dragObject = parent != null ? parent : this.transform;
        }
        else
        {
            Vector3 basePos = parent != null ? parent.position : transform.position;
            offset = GetWorldPositionOnPlane() - basePos;
            offset.z = 0;
        }
    }

    private Vector3 GetWorldPositionOnPlane()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, Camera.main.nearClipPlane));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    private void Update()
    {
        // hmm Ok why can't I not release him?
        if (dragObject != null && Input.GetMouseButtonDown(1))
        {
            // then we release the object
            dragObject = null;
            isSelected = false;
        }

        if (dragObject != null )
        {
            // it's giving me zero position world axis... hmm weird
            Vector3 pos = GetWorldPositionOnPlane();
            //Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;  // kinda worry about this one I need it to exact point at 0 coordinate from a prespective camera... 
            dragObject.position = pos - offset;
        }

        ZoomInOut();
    }

    private Transform CreateNewMesh(GameObject go = null )
    {
        Transform t = new GameObject("MashObject").AddComponent<MashObject>().transform;
        if( go != null)
        {
            t.transform.position = go.transform.position;
            go.transform.parent = t;
        }
        return t;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // in this case here we'll check and see if we're hitting another MouseGrabbing object. 
        // if so then we'll check and see if we have MashObjectManager, if not spawn it and set parent to that object instead
        // option; if the object itself has individual rotationscript, then we need to disable that. 
        //if (!isSelected && !(parent != null && parent.GetComponent<MashObject>().HasBeenSelected)) return;
        if (collision.gameObject.GetComponent<MouseGrab>() != null)
        {
            
            // see if the parent exist, if not create one and attach this child to the newly created parent object
            parent = transform.parent?.GetComponent<MashObject>()?.transform ?? CreateNewMesh(this.gameObject);

            //if (!isSelected && !parent.GetComponent<MashObject>().HasBeenSelected ) return;
            //parent.GetComponent<MashObject>().HasBeenSelected = true;

            if ( dragObject != null )
                dragObject = parent;

            // get the other object's parent we hit with.
            Transform otherParent = collision.transform.parent?.GetComponent<MashObject>()?.transform;
            // freeze constraint since we already hit a object that is a mousegrab. 
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            // also free the other object as well just in case.
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            
            // if the other object have parents then we need to merge all of their existing child to this parent.
            if (otherParent != null)
            {
                //if (isSelected || parent.GetComponent<MashObject>().HasBeenSelected)
                //{
                    // foreach gameobject down to 0, set it to null and then reassign to the parent.
                    for (int i = otherParent.childCount - 1; i >= 0; i--)
                    {
                        Transform go = otherParent.GetChild(i).transform;
                        go.parent = null;
                        go.parent = parent;
                        go.GetComponent<MouseGrab>().parent = parent;
                        go.GetComponent<MouseGrab>().dragObject = null;
                    }

                    // realign the offset
                    Vector3 basePos = parent != null ? parent.position : transform.position;
                    offset = GetWorldPositionOnPlane() - basePos;
                    offset.z = 0;
                //}
            }
            // otherwise, it's just a floatnig object, we'll merge it anyway.
            else
            {
                // in this case if the other parent doesn't have a parent, we'll acquire the child.
                collision.transform.parent = parent;   
            }

            // why is this script assigning nulls????
            //if( parent.GetComponent<MashObject>().HasBeenSelected)
            //{
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                    
                Transform go = parent.GetChild(i).transform;
                MouseGrab mg = go.GetComponent<MouseGrab>();
                // why does this one even reset everything???
                //if (mg.isSelected )
                //{
                    mg.parent = parent;
                    mg.dragObject = parent;
                //}
            }
        }
    }

    public static void OnGameWon()
    {
        instance.dragObject = null;
        //instance.isSelected = false;
    }
}
