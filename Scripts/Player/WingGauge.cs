using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WingGauge : MonoBehaviour
{
    [SerializeField] PlayerManager player;
    Image image;

    // Start is called before the first frame update
    void Start()
    {

        image = GetComponent<Image>();
        image.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = Mathf.Lerp(image.fillAmount, player.Wing / 10, 0.1f);            
    }

    
}
