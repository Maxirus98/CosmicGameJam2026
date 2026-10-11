using UnityEngine;

public class RotateBackground : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
