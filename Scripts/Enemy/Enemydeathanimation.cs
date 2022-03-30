using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemydeathanimation : MonoBehaviour
{
    //AudioSource audioSource;
    //[SerializeField] AudioClip Pan;

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    public void OncompleteAnimation()
    {
        
        Destroy(this.gameObject);
    }
}
