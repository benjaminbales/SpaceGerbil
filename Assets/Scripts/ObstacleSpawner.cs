using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] obstacles;
    public float minSpawnDuration = 5f;
    public float maxSpawnDuration = 20f;
    private float _t = 999;

    void Start()
    {
        GetNewSpawnDuration();
    }

    private void GetNewSpawnDuration()
    {
        _t = Random.Range(minSpawnDuration, maxSpawnDuration);
    }

    // Update is called once per frame
    void Update()
    {
        // find a way to stop the spawner after the game has won..
        _t -= Time.deltaTime;
        if( _t < 0 )
        {
            int p = Random.Range(0, obstacles.Length);
            //float z = Random.Range(-360, 360);
            Instantiate(obstacles[p], Vector3.zero, Quaternion.identity);
            GetNewSpawnDuration();
        }
    }


}
