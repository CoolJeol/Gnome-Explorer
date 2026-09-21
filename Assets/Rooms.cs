using UnityEngine;
using Unity.Cinemachine;

public class Rooms : MonoBehaviour
{

    [Tooltip("The Room name")]

    [SerializeField]
    private Vector2Int gridLocation;

    [SerializeField]
    private CinemachineCamera theCamera;

    [SerializeField]
    private bool startingRoom = false;

    private bool playerHasEntered = false;
       
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startingRoom)
        {
            playerHasEntered = true;
            theCamera.gameObject.SetActive(true);
        }
        else
        {
            playerHasEntered = false;
            theCamera.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            if (!playerHasEntered)
            {

                playerHasEntered = true;
                theCamera.gameObject.SetActive(true);

            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        playerHasEntered = false;
        theCamera.gameObject.SetActive(false);

    }


}
