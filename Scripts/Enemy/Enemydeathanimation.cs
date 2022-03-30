using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemydeathanimation : MonoBehaviour
{
    public void OncompleteAnimation()
    {
        Destroy(this.gameObject);
    }
}
