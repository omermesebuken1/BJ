using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResizeSprite : MonoBehaviour
{

    private void Update() {

         ResizeSpriteToScreen();
    }

    public void ResizeSpriteToScreen() {
     var sr = GetComponent<SpriteRenderer>();
     if (sr == null) return;
     
     transform.localScale = new Vector3(1,1,1);
     
     var width = sr.sprite.bounds.size.x;
     var height = sr.sprite.bounds.size.y;
     
     var worldScreenHeight = Camera.main.orthographicSize * 2.0;
     var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
     
     float lastWidth = (float)(worldScreenWidth / width);
     float lastHeight = (float)(worldScreenHeight / height);

     transform.localScale = new Vector3(lastWidth,lastHeight,0);

     transform.position = new Vector3(0,0,0);
 }

}
