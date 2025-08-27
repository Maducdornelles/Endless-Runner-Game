using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    public float speed = 6f;
    public float destroyX = -15f;

    void Update()
    {
        
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }
}
