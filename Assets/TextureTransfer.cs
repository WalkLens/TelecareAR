using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTransfer : MonoBehaviour
{
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
    }
}
