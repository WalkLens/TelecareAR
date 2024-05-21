using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverCtrl : MonoBehaviour
{
    public GameObject gridObj;
    public GameObject materialsObject; // materials 오브젝트 추가
    private List<Material> materialList;
    private int currentIndex = 0;
    private List<string> collisionSequence = new List<string>();
    private string[] sequence = new string[] { "Back-1", "Back0", "Back1" };
    private int direction = 1; // 1: 오른쪽, -1: 왼쪽

    void Start()
    {
        materialList = new List<Material>();

        if (materialsObject != null)
        {
            MeshRenderer renderer = materialsObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                foreach (var mat in renderer.materials)
                {
                    materialList.Add(mat);
                }
            }
            else
            {
                Debug.LogError("materialsObject에 MeshRenderer가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("materialsObject를 찾을 수 없습니다.");
        }

        if (materialList.Count > 0)
        {
            Debug.Log("Material list count: " + materialList.Count);
            Debug.Log("Current Material set to: " + materialList[currentIndex].name);
        }
        else
        {
            Debug.LogError("Material이 없습니다.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Back0"))
        {
            string tag = collision.gameObject.tag;
            collisionSequence.Add(tag);

            if (collisionSequence.Count > 2)
            {
                collisionSequence.RemoveAt(0);
            }

            DetermineDirection();
            ChangeMaterial();
        }
        else if (collision.gameObject.CompareTag("Back-1") || collision.gameObject.CompareTag("Back1"))
        {
            string tag = collision.gameObject.tag;
            collisionSequence.Add(tag);

            if (collisionSequence.Count > 2)
            {
                collisionSequence.RemoveAt(0);
            }

            DetermineDirection();
        }
    }

    private void DetermineDirection()
    {
        if (collisionSequence.Count < 2)
        {
            return;
        }

        string first = collisionSequence[0];
        string second = collisionSequence[1];

        int firstIndex = System.Array.IndexOf(sequence, first);
        int secondIndex = System.Array.IndexOf(sequence, second);

        if (firstIndex != -1 && secondIndex != -1)
        {
            direction = (secondIndex - firstIndex + sequence.Length) % sequence.Length == 1 ? 1 : -1;
        }
    }

    private void ChangeMaterial()
    {
        if (materialList.Count > 0)
        {
            currentIndex = (currentIndex + direction + materialList.Count) % materialList.Count;
            Debug.Log("Current Material changed to: " + materialList[currentIndex].name);

            if (gridObj != null)
            {
                var gridRenderer = gridObj.GetComponent<MeshRenderer>();
                if (gridRenderer != null)
                {
                    gridRenderer.material = materialList[currentIndex];
                    Debug.Log("Grid Material set to: " + materialList[currentIndex].name);
                }
                else
                {
                    Debug.LogError("GridObject에 MeshRenderer가 없습니다.");
                }
            }
            else
            {
                Debug.LogError("gridObj를 찾을 수 없습니다.");
            }
        }
    }
}
