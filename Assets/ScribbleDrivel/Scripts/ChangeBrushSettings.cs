using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LylekGames
{
    public class ChangeBrushSettings : MonoBehaviour
    {

        private Image myImage;

        public void Start()
        {
            myImage = GetComponent<Image>();
        }
        public void ChangeBrushColor()
        {
            DrawScript.drawScript.SetBrushColor(myImage.color);
        }
        public void ChangeBrushSize()
        {
            DrawScript.drawScript.SetBrushSize((int)myImage.rectTransform.rect.height);
        }
        public void ChangeBrushShape()
        {
            DrawScript.drawScript.SetBrushShape(myImage.sprite);
        }
        public void Undo()
        {
            DrawScript.drawScript.Undo();
        }
    }
}
