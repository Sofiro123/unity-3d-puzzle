using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject hologramModel;
    [SerializeField] private GameObject realModel;

    public static PuzzleManager Instance { get; private set; }

    private List<PuzzlePiece> allPieces = new List<PuzzlePiece>();

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        if (hologramModel)
            hologramModel.SetActive(true);

        if (realModel)
            realModel.SetActive(false);
    }

    private void Start()
    {
        if (allPieces.Count > 0)
        {
            PuzzlePiece randomStart = allPieces[Random.Range(0, allPieces.Count)];
            randomStart.ForceSnap();
        }
    }

    public void RegisterPiece(PuzzlePiece piece)
    {
        if (!allPieces.Contains(piece))
            allPieces.Add(piece);
    }

    public void FakeComplete()
    {
        foreach (var piece in allPieces)
            piece.ForceSnap();

        CheckPuzzleComplete();
    }

    public void CheckPuzzleComplete()
    {
        foreach (var piece in allPieces)
        {
            if (!piece.IsSnapped)
                return;
        }

        Debug.Log("Puzzle Completed!");
        UIController ui = FindObjectOfType<UIController>();

        if (ui)
            ui.ShowWin();

        if (hologramModel)
            hologramModel.SetActive(false);

        if (realModel)
            realModel.SetActive(true);

        FindObjectOfType<OrbitCamera>()?.StartAutoRotation();
    }
}
