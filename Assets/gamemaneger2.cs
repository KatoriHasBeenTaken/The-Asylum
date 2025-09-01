using System.Collections.Generic;
using UnityEngine;

public class gamemaneger2 : MonoBehaviour
{
    [SerializeField] private Transform gameTransfrom;
    [SerializeField] private Transform pieacePrefab;
    private List<Transform> pieces;
    private int emptylocation;
    public int size;
    private bool shuffling = false;
    private int count = 0;
    private void creatGamePice(float gapThickness)
    {
        float width = 1 / (float)size;
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(pieacePrefab, gameTransfrom);
                pieces.Add(piece);
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                                  +1 - (2 * width * row) - width
                                                  , 0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";
                if ((row == size ) && (col == size ))
                {
                    emptylocation = (size * size);
                   // piece.gameObject.SetActive(false);
                }
                else
                {
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    //uv coord order : (0,1) ;(1,1);(0,0);(1,0);
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - (width * row) + gap);

                    mesh.uv = uv;
                }
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pieces = new List<Transform>();

        creatGamePice(0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
