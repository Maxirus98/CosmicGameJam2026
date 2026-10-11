using UnityEngine;

public class OnIntroEnd : MonoBehaviour
{
    public void HideIntro()
    {
        transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void HideCredit()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
