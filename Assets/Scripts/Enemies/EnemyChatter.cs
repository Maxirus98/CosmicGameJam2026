using TMPro;
using UnityEngine;

public class EnemyChatter : MonoBehaviour
{
    private string[] badChatterLines = {
        "I'd like to talk to you about your car's extended warranty",
        "follow my OnlyFeet to keep chatting",
        "Help me step-chad, I'm stuck",
        "if u can’t handle my worst u don’t deserve my slightly better",
        "OwO What’s this?",
        "whats ur rank on league?",
        "ur so much hotter than my ex",
        "pls lemme lick ur abs Chaddy",
        "i put on my robe and wizard hat",
        "did it hurt? when u fell down the stairs?",
        "idk what i wanna eat, u choose",
        "asl?", "je suis du 82 et toi??",
        "u need to be 6 feet tall, 6 figures, 6 toes",
        "r u a gamecube controller?", "cuz i wanna super smash (melee)",
        "eggplant emoji",
        "ur lowkey a snack no cap on god six seven",
        "Date me and get 5000 gold on RAID: SHADOW LEGENDS!",
        "my bf doesn’t need to know",
        "wats on the menu? ME-N-U!",
        "ur cat won’t sleep with us, right?",
        "call me garbage cuz i wanna ride dat dump truck",
    };

    private string[] goodChatterLines =
    {
        "What’s your fave ramen restaurant?",
        "I’m just happy to talk.",
        "Wanna play board games with me?",
        "Let’s have a picnic next weekend!",
        "I wanna see your card binder!",
        "So happy you got your dream job as a hamster photographer!",
        "I know you love Celine Dion, I got you a ticket!",
        "Here’s a picture of my cat!",
        "Tell me about your D&D character!",
        "You do game jams? That’s so hot.",
        "I made you soup.",
        "You look tense. Want a massage?",
        "I love it when you talk about Shinkansen Series E5 trains.",
        "Take some of my fries!",
        "Sorry I’m late, I was saving orphans from a fire."
     };

    [SerializeField]
    [Tooltip("The chat bubble prefab to display the messages")]
    private GameObject chatBubble;

    private EnemyController enemyController;
    private bool willChat;
    private bool displayedMessage = false;
    private float minDistFromPlayerToChat = 10f; // Minimum distance from the player to display a message

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        willChat = Random.Range(0f, 1f) < (enemyController.isGood ? 0.3f : 0.2f); // 20% chance to chat if good, 10% if bad
    }

    // Update is called once per frame
    void Update()
    {
        // Have a 10% chance to display a message
        var distFromPlayer = Vector3.Distance(transform.position, enemyController.Player.transform.position);
        if (!displayedMessage && distFromPlayer <= minDistFromPlayerToChat && willChat)
        {
            DisplayMessage();
            displayedMessage = true;
        }
    }

    private void DisplayMessage()
    {
        AudioManager.Instance.PlayAudio(0, 0.5f);
        if (enemyController.isGood)
        {
            DisplayRandomMessage(goodChatterLines);
        }
        else
        {
            DisplayRandomMessage(badChatterLines);
        }
    }

    private void DisplayRandomMessage(string[] messageArray)
    {
        int randomIndex = Random.Range(0, messageArray.Length);
        string randomMessage = messageArray[randomIndex];

        GameObject bubble = Instantiate(chatBubble, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        bubble.GetComponentInChildren<TextMeshPro>().text = randomMessage;

        bubble.transform.SetParent(transform); // Set the chat bubble as a child of the enemy
    }
}
