using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTransfer : MonoBehaviour
{
    public Renderer sourceRenderer; // �ҽ� Material�� ����ǰ� �ִ� Mesh Renderer
    public Renderer targetRenderer; // Ÿ�� Renderer ������Ʈ

    // ��ư Ŭ���� ���� ȣ��� �޼���
    public void TransferTexture()
    {
        // ���� Material���� �ؽ�ó ��������
        Texture sourceTexture = sourceRenderer.material.mainTexture;

        // ���ο� Texture2D ����
        Texture2D newTexture = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.RGBA32, false);

        // RenderTexture ���� �� ����
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = new RenderTexture(sourceTexture.width, sourceTexture.height, 32);
        Graphics.Blit(sourceTexture, renderTexture);

        // ���ο� �ؽ�ó�� ����
        RenderTexture.active = renderTexture;
        newTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        newTexture.Apply();

        // RenderTexture�� �ٽ� ������� ����
        RenderTexture.active = currentRT;
        renderTexture.Release();

        // Ÿ�� ������Ʈ�� �� �ؽ�ó ����
        targetRenderer.material.mainTexture = newTexture;
    }
}
