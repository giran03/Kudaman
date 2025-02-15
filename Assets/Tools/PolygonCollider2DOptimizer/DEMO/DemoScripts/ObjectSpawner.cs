using UnityEngine;
using UnityEngine.UI;

namespace Unitycoder.Extras
{
    public class ObjectSpawner : MonoBehaviour
    {
        public Transform[] prefabs;
        public Text objectCounterText;
        int counter = 0;
        public float fireRate = 0.25F;
        private float nextFire = 0.0F;

        Camera cam;

        void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            // spawn on clickdown
            if (Input.GetMouseButton(0) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, Quaternion.identity);

                float posX = cam.ViewportToWorldPoint(new Vector3(Random.Range(0.2f, 0.8f), 0, 10)).x;
                transform.position = new Vector3(posX, transform.position.y, transform.position.z);
                counter++;
                objectCounterText.text = "" + counter;
            }
        }
    }
}