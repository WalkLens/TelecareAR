using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextureTransfer : MonoBehaviour
{
    public Image whiteboardImage;
    public Renderer sourceRenderer; // 소스 Material이 변경되고 있는 Mesh Renderer
    public Renderer targetRenderer; // 타겟 Renderer 오브젝트

    // 버튼 클릭에 의해 호출될 메서드
    public void TransferTexture()
    {
        // 현재 Material에서 텍스처 가져오기
        Texture sourceTexture = sourceRenderer.material.mainTexture;

        // 새로운 Texture2D 생성
        Texture2D newTexture = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.RGBA32, false);

        // RenderTexture 생성 및 설정
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = new RenderTexture(sourceTexture.width, sourceTexture.height, 32);
        Graphics.Blit(sourceTexture, renderTexture);

        // 새로운 텍스처에 복사
        RenderTexture.active = renderTexture;
        newTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        newTexture.Apply();

        // RenderTexture를 다시 원래대로 설정
        RenderTexture.active = currentRT;
        renderTexture.Release();

        // 타겟 오브젝트에 새 텍스처 설정
        targetRenderer.material.mainTexture = newTexture;
        //Texture2D tex2D = (tex as Texture2D);
        Rect rect = new Rect(0, 1, newTexture.width, -newTexture.height);
        whiteboardImage.GetComponent<Image>().sprite = Sprite.Create(newTexture, rect, new Vector2(0, 0));
        
    }
    void TransferTexture1() {
        // RenderTexture 생성
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
        sourceRenderer.material.SetPass(0);
        Graphics.Blit(null, renderTexture, sourceRenderer.material);

        // RenderTexture에서 Texture2D로 변환
        Texture2D texture2D = RenderTextureToTexture2D(renderTexture);

        // Texture2D에서 Sprite 생성
        Sprite newSprite = Texture2DToSprite(texture2D);

        // Image 컴포넌트에 새로운 Sprite 적용
        whiteboardImage.sprite = newSprite;
    }

    Texture2D RenderTextureToTexture2D(RenderTexture rTex) {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null; // RenderTexture를 더 이상 사용하지 않으므로 해제
        return tex;
    }

    Sprite Texture2DToSprite(Texture2D tex) {
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
}
