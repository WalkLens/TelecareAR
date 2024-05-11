using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LylekGames
{
    public class VideoCapture : MonoBehaviour
    {

        [Header("Components")]
        public Canvas canvas;
        private Image ourImage;



        [Header("Save Data")]
        //Save a screenshot of ourImage to this location
        [Tooltip("The save name of ourImage. By default, the location will be Assets/ScribbleDrivel/Resources/. The image must be saved in the Resources folder if you wish to load it during play. You can save the image by calling the Save() method.")]
        public string savePath = "MyImage";
        [Tooltip("Loads our image from the savePath on start. Alternatively, you can call the Load(string); method and provide the image path/name.")]
        public bool loadImageOnStart = false;


        public void Load(string path)
        {
            Debug.Log("Loading image...");

            Texture2D loadImage = Resources.Load<Texture2D>(savePath);
            if (loadImage != null)
            {
                Sprite tempSprite = Sprite.Create(loadImage, new Rect(0, 0, (int)loadImage.width, (int)loadImage.height), Vector2.zero);
                ourImage.sprite = tempSprite;

                Debug.Log("Image found.");
            }
            else
            {
                Debug.Log("Image not found.");
            }
        }
        public void Save(string path = "")
        {
            if (path != "")
                savePath = path;

            StartCoroutine(BeginSaveImage());

            Debug.Log("Saving...");


        }
        private IEnumerator BeginSaveImage()
        {

            //Wait For End This Of Frame
            yield return new WaitForEndOfFrame();

            //Calculate Our Image Dimensions
            Vector3[] ourCoordinates = new Vector3[4];
            ourImage.rectTransform.GetWorldCorners(ourCoordinates);
            Vector3[] canvasCoordinates = new Vector3[4];
            canvas.GetComponent<RectTransform>().GetWorldCorners(canvasCoordinates);
            float posX = ourCoordinates[0].x / canvasCoordinates[3].x;
            float posY = ourCoordinates[0].y / canvasCoordinates[1].y;
            float posW = (ourCoordinates[3].x - ourCoordinates[0].x) / canvasCoordinates[3].x;
            float posH = (ourCoordinates[1].y - ourCoordinates[0].y) / canvasCoordinates[1].y;

            Vector2 actualPos = new Vector2(posX * Screen.width, posY * Screen.height);
            Vector2 actualSize = new Vector2(posW * Screen.width, posH * Screen.height);
            
            //Take A Screenshot
            Texture2D screenShot = new Texture2D((int)actualSize.x, (int)actualSize.y, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(actualPos.x + 1, actualPos.y + 1, (int)actualSize.x, (int)actualSize.y), 0, 0);
            screenShot.Apply();

            //SAVE Screenshot To Project
            byte[] bytes = screenShot.EncodeToPNG();
            string savepath = "Assets/ScribbleDrivel/Resources/" + savePath + ".png";
            System.IO.File.WriteAllBytes("Assets/ScribbleDrivel/Resources/" + savePath + ".png", bytes);
            UnityEditor.AssetDatabase.Refresh();

            Debug.Log("Image saved.");

            Load(savepath);
        }
    }
}

