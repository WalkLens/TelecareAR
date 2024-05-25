using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class YUVtoRGBConverter : MonoBehaviour
{
    public Image whiteboardImage;
    public Material yuvToRgbMaterial;
    public Renderer sourceRenderer;
    public Texture yTexture;
    public Texture uTexture;
    public Texture vTexture;

    void Start()
    {
        //ConvertYUVtoRGB();
    }

    public void ConvertYUVtoRGB()
    {

        //Texture sourceTexture = sourceRenderer.material.mainTexture;
        yTexture = sourceRenderer.material.GetTexture("_YPlane");
        uTexture = sourceRenderer.material.GetTexture("_UPlane");
        vTexture = sourceRenderer.material.GetTexture("_VPlane");


        // Create a RenderTexture to store the RGB output
        int width = yTexture.width;
        int height = yTexture.height;
        RenderTexture rgbRenderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
        rgbRenderTexture.Create();

        // Set the YUV textures to the material
        yuvToRgbMaterial.SetTexture("_YTex", yTexture);
        yuvToRgbMaterial.SetTexture("_UTex", uTexture);
        yuvToRgbMaterial.SetTexture("_VTex", vTexture);

        // Use a temporary RenderTexture for the conversion
        RenderTexture.active = rgbRenderTexture;
        GL.Clear(true, true, Color.clear);
        Graphics.Blit(null, rgbRenderTexture, yuvToRgbMaterial);

        // Read the RenderTexture into a Texture2D
        Texture2D rgbTexture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
        rgbTexture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        rgbTexture2D.Apply();

        Rect rect = new Rect(0,1, rgbTexture2D.width, -rgbTexture2D.height);
        whiteboardImage.GetComponent<Image>().sprite = Sprite.Create(rgbTexture2D, rect, new Vector2(0,0));



        // Save the Texture2D as PNG
        // byte[] bytes = rgbTexture2D.EncodeToPNG();
        // File.WriteAllBytes(Application.dataPath + "/ConvertedRGBTexture.png", bytes);

        // // Clean up
        // RenderTexture.active = null;
        // rgbRenderTexture.Release();
        // Destroy(rgbTexture2D);

        // Debug.Log("YUV to RGB conversion completed. Saved to: " + Application.dataPath + "/ConvertedRGBTexture.png");
    }
}


