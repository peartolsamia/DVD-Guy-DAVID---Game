// --- OTHER SCRIPTS REACHES AND MANUPILATES PLAYER SUB SYSTEMS VIA THIS SINGLETON ------
// --------------------------------------------------------------------------------------


using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerMovement Movement { get; private set; }
    public PlayerHealth Health { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Movement = GetComponent<PlayerMovement>();
        Health = GetComponent<PlayerHealth>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}