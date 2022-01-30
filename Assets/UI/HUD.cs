using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Transform startPoint;
    public Sprite healthSprite;
    public uint gap;
    public Player player;
    public float spriteWidth;
    public float spriteHeight;
    public TextMeshProUGUI text;
    private List<GameObject> sprites = new List<GameObject>();
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < player.health; i++)
        {
            GameObject imgObject = new GameObject("health");
            RectTransform trans = imgObject.AddComponent<RectTransform>();
            trans.transform.SetParent(imgObject.transform); // setting parent
            trans.localScale = Vector3.one;
            trans.anchoredPosition = new Vector2(startPoint.position.x + i * gap, startPoint.position.y);
            trans.sizeDelta = new Vector2(spriteWidth, spriteHeight); // custom size
            Image image = imgObject.AddComponent<Image>();
            image.sprite = healthSprite;
            imgObject.transform.SetParent(startPoint);
            sprites.Add(imgObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health < sprites.Count)
        {
            GameObject spriteToRemove = sprites[sprites.Count - 1];
            sprites.Remove(spriteToRemove);
            Destroy(spriteToRemove);
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 1)
        {
            timer += Time.fixedDeltaTime;
            // text.text = timer / 60 + ":" + timer % 60;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
            text.text = String.Format("{0:0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
