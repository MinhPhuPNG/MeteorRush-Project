using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;

    public float spawnRate = 2.5f;
    public float minX = -2.5f;
    public float maxX = 2.5f;

    float nextSpawnTime = 0f;
    float spawnY = -5.5f;
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnMeteor();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnMeteor()
    {
        float x = Random.Range(minX, maxX);
        float y = spawnY;

        GameObject meteor = Instantiate(meteorPrefab, new Vector3(x, y, 0f), Quaternion.identity);

        Meteor meteorScript = meteor.GetComponent<Meteor>();

        if (meteorScript != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                meteorScript.moveDirection = (player.transform.position - meteor.transform.position).normalized;
            }
        }
    }
}