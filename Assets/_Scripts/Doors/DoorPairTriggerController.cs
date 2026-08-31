using System.Collections;
using UnityEngine;

public class DoorPairTriggerController2D : MonoBehaviour
{
    [Header("Return door in connected room")]
    [SerializeField] private DoorPairTriggerController2D pairedObject;

    [Header("This door's collider")]
    [SerializeField] private Collider2D myCollider;

    private void Awake()
    {
        if (myCollider == null)
        {
            myCollider = GetComponent<Collider2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HandleTriggerSequence());
        }
    }

    private IEnumerator HandleTriggerSequence()
    {
        if (myCollider != null)
        {
            myCollider.enabled = false;
        }

        yield return new WaitForSeconds(1.0f);


        if (pairedObject != null && pairedObject.myCollider != null)
        {
            pairedObject.myCollider.enabled = true;
        }
    }
}