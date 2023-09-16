using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject cursorPrefabP1;
    public GameObject cursorPrefabP2;
    public Text player1IndicatorText;
    public Text player2IndicatorText;
    public GameObject player1Avatar; // Reference to Player 1 Avatar Image
    public GameObject player2Avatar; // Reference to Player 2 Avatar Image

    private GameObject currentCursorP1;
    private GameObject currentCursorP2;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Mouse clicked on object with tag: " + hit.collider.tag);

                if (hit.collider.CompareTag("Player 1 Avatar"))
                {
                    Debug.Log("Player 1 Avatar selected");
                    HandleCharacterSelection(cursorPrefabP1, ref currentCursorP1, "P1 Select", player1IndicatorText, player1Avatar);
                }
                else if (hit.collider.CompareTag("Player 2 Avatar"))
                {
                    Debug.Log("Player 2 Avatar selected");
                    HandleCharacterSelection(cursorPrefabP2, ref currentCursorP2, "P2 Select", player2IndicatorText, player2Avatar);
                }
            }
        }
    }

    private void HandleCharacterSelection(GameObject cursorPrefab, ref GameObject currentCursor, string playerIndicator, Text indicatorText, GameObject avatarImage)
    {
        // Destroy the current cursor for the other player if it exists
        if (currentCursor != null)
        {
            Destroy(currentCursor);
        }

        // Instantiate the cursor for the selected player
        Vector3 characterPosition = cursorPrefab.transform.position;
        currentCursor = Instantiate(cursorPrefab, characterPosition, Quaternion.identity);
        Debug.Log(playerIndicator + " Cursor instantiated");

        // Set the indicator text
        indicatorText.text = playerIndicator;

        // You can do additional actions with the avatarImage GameObject here if needed.
    }
}
