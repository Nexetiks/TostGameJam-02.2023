using System;
using General;
using Interactions;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

[Serializable]
public class Gatherable : MonoBehaviour, IInteractable
{
    public Transform SnapPivot { get; }

    [SerializeField]
    private int priority = 0;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Collider collider;

    private bool isReady = false;

    private void Awake()
    {
        if (collider == null)
        {
            collider = gameObject.GetComponent<Collider>();
        }

        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        rb.AddForce(Random.Range(-100f, 100f), 200, Random.Range(-100f, 100f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        collider.isTrigger = true;
        rb.isKinematic = true;
        isReady = true;
    }

    public void OnCursorEnter(MultiMousePointerEventData eventData)
    {
        PickUp(eventData);
    }

    public void OnCursorExit(MultiMousePointerEventData eventData)
    {
    }

    private void PickUp(MultiMousePointerEventData eventData)
    {
        if (isReady == false)
        {
            return;
        }

        Debug.Log("PickUp");
        GameManager.Instance.Gold++;
        gameObject.SetActive(false);
        AudioManager.Instance.Sfxs.Pickup.Play();
    }
}