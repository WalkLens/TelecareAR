using System;
using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.UI;

public class WhiteBoardController : MonoBehaviour
{
    public RawImage whiteboardImage;
    
    
    public List<GameObject> captureImagess;
    public List<bool> isCapturedd;
    
    [Space] 
    [Header("Capture Image")]
    public GameObject capturedImagePrefab;
    public RectTransform capturedImageRoot;

    public int arraySize = 10;
    private void Awake()
    {
        captureImagess = new List<GameObject>(arraySize);
        isCapturedd = new List<bool>(new bool[arraySize]);
        whiteboardImage.gameObject.GetComponent<Button>().onClick.AddListener(SetCaptureImage);
    }

    public GameObject SetCapturedImageProperty(int index)
    {
        // 동적으로 captured Image 등록

        // 캡처된 이미지의 버튼에 메서드 할당
        GameObject capturedImage = Instantiate(capturedImagePrefab, capturedImageRoot);
        captureImagess.Add(capturedImage);
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
        whiteboardImage.texture = captureImagess[index].GetComponent<RawImage>().mainTexture;
    }
    
    public void SetCaptureImage()
    {
        // 화이트보드 화면을 캡쳐해서 captureImage 텍스쳐에 할당
        Debug.Log("Capture count" + captureImagess.Count);

        if (captureImagess.Count == 0)
        {
            SetCapturedImageProperty(0).GetComponent<RawImage>().texture = whiteboardImage.texture;
            // captureImages[i].GetComponent<RawImage>().texture = whiteboardImage.texture;
            isCapturedd[0] = true;
        }
        else
        {
            for (int i = 0; i < isCapturedd.Count; i++)
            {
                if (!isCapturedd[i])
                {
                    SetCapturedImageProperty(i).GetComponent<RawImage>().texture = whiteboardImage.texture;
                    // captureImages[i].GetComponent<RawImage>().texture = whiteboardImage.texture;
                    isCapturedd[i] = true;
                    return;
                }
            }
        }
            
        
    }
}
