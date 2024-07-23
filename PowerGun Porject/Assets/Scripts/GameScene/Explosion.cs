using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public void doDestroy()
    {
        Destroy(gameObject);
    }

    public void ImageSize(float imgSize)
    {
        Vector3 scale = transform.localScale;
        scale *= imgSize / 125;
        transform.localScale = scale;
    }
    
}
