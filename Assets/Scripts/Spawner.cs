using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawnerObject;
    public int MaxObjectsSpawn = 15;
    public float Radii = 15f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Camera.main.transform.position, Radii);
    }

    // Start is called before the first frame update
    void Awake()
    {
        // in this case we need to make the object 
        Vector3 pos = Camera.main.transform.position;
        pos.z = 0;
        int i = 0;
        while( i++ < MaxObjectsSpawn)
        {
            float dirx = Random.Range(-Radii, Radii);
            float diry = Random.Range(-Radii, Radii);
            int p = Random.Range(0, spawnerObject.Length);
            Vector3 dir = new Vector3(dirx, diry, 0);
            GameObject go = Instantiate(spawnerObject[p], dir, Quaternion.identity);
        }
    }
}
