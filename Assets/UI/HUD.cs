using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//
// Had to make a change to how the 3 health marks appeared on the UI
//Before they only deleted them from a set of pre-existing 3 icons
//now if you collect a heal power up it generates a new one in the right spot
//
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
        if (player.health > sprites.Count)
        {
            int x = (int)(player.health - sprites.Count);
            for(int i = 0; i < x; i++ )
            {
                GameObject imgObject = new GameObject("health");
                RectTransform trans = imgObject.AddComponent<RectTransform>();
                trans.transform.SetParent(imgObject.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.anchoredPosition = new Vector2(startPoint.position.x + sprites.Count * gap, startPoint.position.y);
                trans.sizeDelta = new Vector2(spriteWidth, spriteHeight); // custom size
                Image image = imgObject.AddComponent<Image>();
                image.sprite = healthSprite;
                imgObject.transform.SetParent(startPoint);
                sprites.Add(imgObject);
            }
            
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
