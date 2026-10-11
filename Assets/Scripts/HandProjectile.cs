using UnityEngine;

public class HandProjectile : MonoBehaviour
{
    public Vector3 MouseClickPos { get; set; }
    [SerializeField]
    private float speed = 10f;


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, MouseClickPos, speed * Time.deltaTime);

        // Stop moving if the destination is reached
        if (Vector3.Distance(transform.position, MouseClickPos) < 0.5f)
        {
            Destroy(gameObject);
        }
    }
}
