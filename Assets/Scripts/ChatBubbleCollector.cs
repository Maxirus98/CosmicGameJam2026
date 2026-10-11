using UnityEngine;

public class ChatBubbleCollector : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject, 3f);
    }
}
