using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Infra.Gameplay;

namespace BatCave {
public class CaveGenerator : MonoBehaviour {
    [SerializeField] EndlessWorldScroller scroller;
    [SerializeField] float ceilingTop;
    [SerializeField] float floorBottom;
    [SerializeField] float minSpaceBetweenFloorAndCeiling;
    [SerializeField] float spaceBeforeGameStarts;
    [SerializeField] float lastCeilingY;
    [SerializeField] float lastFloorY;
    [SerializeField] int pointsToGenerate;

    protected void Start() {
        scroller.OnChunkMoved += OnChunkMoved;
        GameManager.OnGameReset += OnGameReset;
    }

    protected void OnDestroy() {
        scroller.OnChunkMoved -= OnChunkMoved;
        GameManager.OnGameReset -= OnGameReset;
    }

    protected void OnChunkMoved(Transform chunk) {
        var caveChunk = chunk.GetComponent<CaveChunk>();
        var ceilingPoints = new Vector2[pointsToGenerate + 3];
        var floorPoints = new Vector2[pointsToGenerate + 3];
        var index = 0;
        var width = scroller.ChunkSize;
        var segmentWidth = width / pointsToGenerate;
        ceilingPoints[index] = new Vector2(width, ceilingTop);
        floorPoints[index++] = new Vector2(width, floorBottom);
        ceilingPoints[index] = new Vector2(0f, ceilingTop);
        floorPoints[index++] = new Vector2(0f, floorBottom);
        ceilingPoints[index] = new Vector2(0f, lastCeilingY);
        floorPoints[index] = new Vector2(0f, lastFloorY);
        var minSpace = GameManager.Instance.HasStarted ? minSpaceBetweenFloorAndCeiling : spaceBeforeGameStarts;
        for (int i = 1; i <= pointsToGenerate; i++) {
            lastCeilingY = Random.Range(floorBottom + minSpace, ceilingTop);
            lastFloorY = Random.Range(floorBottom, lastCeilingY - minSpace);
            ceilingPoints[index + i] = new Vector2(segmentWidth * i, lastCeilingY);
            floorPoints[index + i] = new Vector2(segmentWidth * i, lastFloorY);
        }
        // Reverse the ceiling points because we need the points in a clockwise
        // direction.
        Array.Reverse(ceilingPoints);
        caveChunk.ceiling.SetPoints(ceilingPoints);
        caveChunk.floor.SetPoints(floorPoints);
    }

    private void OnGameReset() {
        lastCeilingY = ceilingTop - 0.5f;
        lastFloorY = floorBottom + 0.5f;
        scroller.ResetChunks();
    }
}
}
