using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerNameTagController : MonoBehaviour
{
    [SerializeField] private PlayerFlip flip;
    public TMP_Text nameText;
    public Vector3 tagWorldOffset = new Vector3(0f, 0.5f, 0f);
    Transform target;
    void Start()
    {
        target = transform.parent;
        string playerName = PlayerPrefs.GetString("PLAYER_NAME", "Player");
        nameText.text = playerName;
    }

    void LateUpdate()
    {
        if (target==null) return;
        transform.position = target.position + tagWorldOffset;
        Vector3 tagScale = transform.lossyScale;
        if (tagScale.x < 0f)
        {
            Vector3 temp = transform.localScale;
            temp.x = -temp.x;
            transform.localScale = temp;
        }

    }

}
