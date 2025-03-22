using UnityEngine;

public class MovingClouds : MonoBehaviour
{
  public GameObject cloudPrefab; // The cloud prefab
  public float spawnInterval = 2f; // Time between spawns
  public Transform[] leftSpawnPoints; // Array of spawn points for clouds moving right
  public Transform[] rightSpawnPoints; // Array of spawn points for clouds moving left

  private int lastLeftSpawnIndex = -1; // Track the last left spawn point
  private int lastRightSpawnIndex = -1; // Track the last right spawn point

  private void Start()
  {
    // Start spawning clouds
    InvokeRepeating("SpawnCloud", 0f, spawnInterval);
  }

  void SpawnCloud() 
  {
    bool spawnFromLeft = Random.value > 0.5f;
    Transform spawnPoint;
    bool isValidSpawn = false;

    if (spawnFromLeft && leftSpawnPoints.Length > 0)
    {
      int newIndex;
      do {
        newIndex = Random.Range(0, leftSpawnPoints.Length);
      } while (leftSpawnPoints.Length > 1 && newIndex == lastLeftSpawnIndex);

      lastLeftSpawnIndex = newIndex;
      spawnPoint = leftSpawnPoints[newIndex];
      isValidSpawn = true;
    }
    else if (!spawnFromLeft && rightSpawnPoints.Length > 0)
    {
      int newIndex;
      do {
        newIndex = Random.Range(0, rightSpawnPoints.Length);
      } while (rightSpawnPoints.Length > 1 && newIndex == lastRightSpawnIndex);

      lastRightSpawnIndex = newIndex;
      spawnPoint = rightSpawnPoints[newIndex];
      isValidSpawn = true;
    }
    else
    {
      Debug.Log("No spawn points available for the selected side.");
      return;
    }

    if (isValidSpawn)
    {
      // Instantiate the cloud and set its movement direction
      GameObject cloud = Instantiate(cloudPrefab, spawnPoint.position, Quaternion.identity);
      Cloud cloudScript = cloud.GetComponent<Cloud>();
      cloudScript.moveDirection = spawnFromLeft ? Vector3.right : Vector3.left;
     
    Debug.Log("Spawned cloud at: " + spawnPoint.position);
  }

  void OnDrawGizmos()
  {
    // Draw spawn points in the scene view
    Gizmos.color = Color.blue; // Left spawn points in blue
    foreach (Transform spawnPoint in leftSpawnPoints)
    {
      if (spawnPoint != null)
      {
        Gizmos.DrawSphere(spawnPoint.position, 0.5f);
      }
    }

    Gizmos.color = Color.red; // Right spawn points in red
    foreach (Transform spawnPoint in rightSpawnPoints)
    {
      if (spawnPoint != null)
      {
        Gizmos.DrawSphere(spawnPoint.position, 0.5f);
      }
    }
  }
}
}
