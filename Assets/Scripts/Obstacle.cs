using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    // in this case we want this object to hit near the object. Hmm how do we go around that? 
    Vector3 dir;
    public float speed = 2f;
    public float impact = 10f;  // should it be linear or quadratic? 
    public float impactDistance = 15f;
    public float spawnOffset = 24f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, dir * 10f);
    }

    void Awake()
    {
        // in this case we need to spawn this object outside the camera frustrum 
        // in any direction we want to spawn, I want this object to aim at the furthest Mash

        // first get the direction randomized
        // then have it spawn outside the camera frustrum by doing a distance
        // and then give it a direction to aim. 

        // in this case I would rather have this guy start from a known object...
        // otherwise use the camera instead.

        Vector3 targetPos = Vector3.zero;
        Vector3 spawnPoint = Camera.main.gameObject.transform.position;

        spawnPoint.z = 0;
        MashObject[] primaryTargets = GameObject.FindObjectsOfType<MashObject>();
        List<MashObject> mo = new List<MashObject>();
        foreach( MashObject m in primaryTargets )
        {
            if (m.transform.childCount > 0)
                mo.Add(m);
        }
        if ( mo?.Count > 0 )
        {
            targetPos = mo[Random.Range(0, mo.Count)].transform.position;
        }
        else
        {
            targetPos = Camera.main.gameObject.transform.position;
        }
        targetPos.z = 0;   // lock the pos to the 0 Z dimension
        float dirx = 0;
        float diry = 0;

        do { dirx = Random.Range(-1.0f, 1.01f); }
        while (dirx == 0);
        
        do { diry = Random.Range(-1.0f, 1.01f); }
        while (diry == 0);

        dir = new Vector3(dirx, diry, 0);
        this.transform.position = spawnPoint - ( dir * spawnOffset );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, impactDistance);
    }

    // Update is called once per frame
    void Update()
    {
        // we're going to try and move this object
        transform.Translate(dir * speed * Time.deltaTime);   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // in this case we want to annhiliate all object, apply explosion at point contact, and then split them so that they're no longer parented anymore.
        if( collision.gameObject.GetComponent<MouseGrab>() != null)
        {
            Transform hit = collision.gameObject.transform;
            Transform p = hit.parent;
            Vector2 point = collision.GetContact(0).point;
            
            if ( p?.GetComponent<MashObject>() != null )
            {
                p.GetComponent<MashObject>().HasBeenSelected = false;
                for (int i = p.childCount - 1; i >= 0; i--)
                {
                    Vector2 pos = p.transform.position;
                    Vector2 dir = Vector2.zero + (pos - point).normalized;
                    float dist = Vector2.Distance(pos, point);
                    float intense = (impact - dist) / impact;
                    Transform go = p.GetChild(i).transform;
                    Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                    MouseGrab mg = go.GetComponent<MouseGrab>();
                    mg.BreakObject();
                    rb.constraints = RigidbodyConstraints2D.None;
                    rb.AddForce(dir * intense * impact , ForceMode2D.Impulse); // see why does the impulse so got damn high??
                }

                Destroy(p.gameObject);           
            }
            else
            {
                Vector2 pos = hit.transform.position;
                Vector2 dir = Vector2.zero + (pos - point).normalized;
                float dist = Vector2.Distance(pos, point);
                float intense = (impact - dist) / impact;
                hit.GetComponent<Rigidbody2D>().AddForce(point * intense * impact, ForceMode2D.Impulse);
            }
        }
    }
}
