using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Rect = UnityEngine.Rect;
public class get_zone_pos : MonoBehaviour
{
    public bool IsTransparent = false;
    private TextureFormat transp = TextureFormat.ARGB32;
    private TextureFormat nonTransp = TextureFormat.RGB24;
    public GameObject go;
    public GameObject[] go2;
    //private Transform[] tt = new Transform []{};
    private List<Transform> tt = new List<Transform>(); 
    // Start is called before the first frame update
    public Camera cam;

    public void getpos()
    {
        StartCoroutine(GetCOlorZones( go2));
        //test2(Screen.width, Screen.height, 1, vvs, tt, go2);
    }

    private Texture2D ScreenShot(int width, int height, int enlargeCOEF,List<Transform> IO)
    {
        TextureFormat textForm = nonTransp;
        //snake.gameObject.SetActive(false);
        if (IsTransparent)
            textForm = transp;
        RenderTexture rt = new RenderTexture(width * enlargeCOEF, height * enlargeCOEF, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width * enlargeCOEF, height * enlargeCOEF, textForm, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        byte[] bytes2 = screenShot.EncodeToPNG();
        
        String filename2 = "./Assets/SomeLevel2.png";
        System.IO.File.WriteAllBytes(filename2, bytes2);
        Camera.main.targetTexture = null;
        return screenShot;
        
    }

    private void setcolor(Texture2D texture,GameObject[]tgo)
    {
        List<Vector3> vectors= new List<Vector3>();

        for (int i = 0; i < tgo.Length; i++)
        {
            vectors.Add(cam.WorldToScreenPoint(tgo[i].transform.position));
        }
        Color[] colors = new Color[vectors.Count];
        for (int i = 0; i < vectors.Count; i++)
        {
            Debug.Log("Pixel "+i+" :"+"x :"+vectors[i].x+"y :"+vectors[i].y+"z :"+vectors[i].z);
            Debug.Log("x : "+(int) vectors[i].x+"y :"+(int) vectors[i].y);

            Color c = texture.GetPixel((int) vectors[i].x, (int) vectors[i].y);
            Circle(texture,(int) vectors[i].x,(int) vectors[i].y,5,Color.black);
            colors[i] = c;
        }
        byte[] bytes2 = texture.EncodeToPNG();
        String filename2 = "./Assets/SomeLevel2.png";
        System.IO.File.WriteAllBytes(filename2, bytes2);
        cam.targetTexture = null;
    }
    IEnumerator GetCOlorZones(GameObject[]tgo)
    {
            yield return new WaitForSeconds(1f);
            Texture2D t = ScreenShot(Screen.width, Screen.height, 1,tt);
            setcolor(t,tgo);
            
    }
    private void test2 (GameObject[]tgo)
    {
        Texture2D t = ScreenShot(Screen.width, Screen.height, 1,tt);
        setcolor(t,tgo);
    }
    public void Circle(Texture2D tex, int cx, int cy, int r, Color col)
    {
        int x, y, px, nx, py, ny, d;
         
        for (x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            for (y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;
                tex.SetPixel(px, py, col);
                tex.SetPixel(nx, py, col);
                tex.SetPixel(px, ny, col);
                tex.SetPixel(nx, ny, col);
 
            }
        }    
    }
    
}
