using System;
using System.Collections;
using UnityEngine;

public class IceMachineController : MonoBehaviour
{
    private bool ifPlayerEntered = false;
    [Header("Reference")]
    [SerializeField] private bool isActive;
    public Sprite onSprite;
    public Sprite offSprite;
    private SpriteRenderer sr;
    [SerializeField] private GameObject iceCubePrefab;
    [SerializeField] private Transform iceSpanwPoint;
    [SerializeField] private Animator iceMachineAnimator;

    [Header("Shake")]
    [SerializeField] private float shakeDuration = 2f;
    [SerializeField] private float shakeStrength = 0.2f;
    [SerializeField] private float shakeFrequency = 40f;
    private bool isBusy;
    public bool IsActive {get => isActive; set{isActive = value;}}
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (isActive && ifPlayerEntered && Input.GetKeyDown(KeyCode.E))
        {
            OnInteraction();
        }
    }
    public void OnProduceAnimationFinished()
    {
        Instantiate(iceCubePrefab, iceSpanwPoint.position, Quaternion.identity);
        isBusy = false;
    } 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            ifPlayerEntered = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            ifPlayerEntered = false;
        }
    }
    private void OnInteraction()
    {
        if(isBusy) return;
        isBusy = true;
        iceMachineAnimator.SetTrigger("ProduceIce");
        // StartCoroutine(ShakeAndDropIceCube());
    }
    private IEnumerator ShakeAndDropIceCube()
    {
        isBusy = true;
        Vector3 originalPos = transform.localPosition;
        float t = 0f;
        while (t < shakeDuration)
        {
            t += Time.deltaTime;
            float xPosDelta = (Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) - 0.5f) * 2f;
            float yPosDelta = (Mathf.PerlinNoise(0f, Time.time * shakeFrequency) - 0.5f) * 2f;

            transform.localPosition = originalPos + new Vector3(xPosDelta, yPosDelta, 0f) * shakeStrength;
            yield return null;
        }
        transform.localPosition = originalPos;
        Instantiate(iceCubePrefab, iceSpanwPoint.position, Quaternion.identity);
        isBusy = false;
    }

}