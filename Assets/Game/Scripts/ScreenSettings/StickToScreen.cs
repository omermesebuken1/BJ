using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class StickToScreen : MonoBehaviour
{
    [SerializeField] private bool StickToWidth;
    [SerializeField] private bool StickToHeight;

    [SerializeField] private float scaleX;
    [SerializeField] private float scaleY;

    private float lastWidth;
    private float lastHeight;


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
     
     

     lastHeight = (float)(worldScreenHeight / height);

     if(StickToWidth) 
     {
        lastWidth = (float)(worldScreenWidth / width);
     }
     else
     {
        lastWidth = scaleX;
     }

     if(StickToHeight) 
     {
        lastHeight = (float)(worldScreenHeight / height);
     }
     else
     {
        lastHeight = scaleY;
     }


     transform.localScale = new Vector3(lastWidth,lastHeight,0);

     //transform.position = new Vector3(0,0,0);
 }


}
