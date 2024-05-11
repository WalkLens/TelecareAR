using System;
using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.WebRTC.Unity;

public class WhiteBoardController : MonoBehaviour
{
    public static WhiteBoardController wbc ;
    public Image whiteboardImage; // RawImage에서 Image로 변경
    public Image whiteboardDrawedImage;
    
    public List<GameObject> captureImages;
    public List<bool> isCaptured;
    
    [Space] 
    [Header("Capture Image")]
    public GameObject capturedImagePrefab;
    public RectTransform capturedImageRoot;

    public int arraySize = 10;
    private void Awake()
    {
        captureImages = new List<GameObject>(arraySize);
        isCaptured = new List<bool>(new bool[arraySize]);
        wbc = this;
        //whiteboardImage.gameObject.GetComponent<Button>().onClick.AddListener(SetCaptureImage);
    }

    public GameObject SetCapturedImageProperty(int index)
    {
        // 동적으로 captured Image 등록

        // 캡처된 이미지의 버튼에 메서드 할당
        GameObject capturedImage = Instantiate(capturedImagePrefab, capturedImageRoot);
        captureImages.Add(capturedImage);
        Debug.Log($"Add Listener {index}");
        capturedImage.gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Set " + index);
                SetWhiteBoardImage(index);
            });
        

        return capturedImage;
    }
    
    public void SetWhiteBoardImage(int index)
    {
        // Texture 대신 Sprite 사용
        whiteboardImage.GetComponent<Image>().sprite = captureImages[index].GetComponent<Image>().sprite;
    }
    
    public void SetCaptureImage()
    {
        // 화이트보드 화면을 캡쳐해서 captureImage 스프라이트에 할당
        Debug.Log("Capture count" + captureImages.Count);

        if (captureImages.Count == 0)
        {
            Debug.Log("1");
            SetCapturedImageProperty(0).GetComponent<Image>().sprite = whiteboardImage.GetComponent<Image>().sprite;
            
            isCaptured[0] = true;
        }
        else
        {
            for (int i = 0; i < isCaptured.Count; i++)
            {
                if (!isCaptured[i])
                {
                    SetCapturedImageProperty(i).GetComponent<Image>().sprite = whiteboardImage.GetComponent<Image>().sprite;
                    isCaptured[i] = true;
                    return;
                }
            }
        }
    }

    // public void SetVideoCaptureImage()
    // {
    //     //var VR = new VideoRenderer();
    //     Texture2D texture2D = VideoRenderer.VR.GetArgb32Texture();
    //     //Texture2D texture2D = VideoRenderer.GetArgb32Texture();
    //     Rect rect = new Rect(0, 0, texture2D.width, texture2D.height);
    //     whiteboardImage.GetComponent<Image>().sprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f));
    // }
}

