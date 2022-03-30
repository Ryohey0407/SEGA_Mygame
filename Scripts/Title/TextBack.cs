using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBack : MonoBehaviour
{
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float xvec = rectTransform.localScale.x;
        rectTransform.localScale = new Vector3(Mathf.Lerp(xvec,2f,0.05f), rectTransform.localScale.y, 0);
    }

}
