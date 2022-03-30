using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] PlayerManager player;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = Mathf.Round((float)player.HP * 1000 / (float)player.MaxHP) / 1000;
    }

    void Update()
    {
        image.fillAmount = Mathf.Round((float)player.HP * 1000 / (float)player.MaxHP) / 1000;
        

        if (image.fillAmount < 0.34f)
        {

            image.color = Color.red;

        }
        else if (image.fillAmount < 0.67f)
        {

            image.color = new Color(1f, 0.67f, 0f);

        }
        else
        {

            image.color = Color.green;

        }
    }


}