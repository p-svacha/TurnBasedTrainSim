using MeshBuilding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideWorld : MonoBehaviour
{
    public static OutsideWorld Instance;

    private const int NUM_BOARDS_PER_SEGMENT = 200;
    private const float TRACK_Y = -2.5f;
    private const float BOARD_GAP = 1f;
    private const float BOARD_LENGTH = 5f;
    private const float BOARD_WIDTH = 0.8f;
    private const float BOARD_HEIGHT = 0.1f;
    private const float BEAM_HEIGHT = 0.4f;
    private const float BEAM_WIDTH = 0.1f;
    private const float BEAM_MARGIN = 0.8f;

    private const float SEGMENT_TOTAL_LENGTH_X = NUM_BOARDS_PER_SEGMENT * (BOARD_GAP + BOARD_WIDTH);

    private const float BACKGROUND_SEGMENT_LENGTH = 400;

    GameObject TrackSegment1;
    GameObject TrackSegment2;

    GameObject BackgroundSegment1;
    GameObject BackgroundSegment2;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateTracks()
    {
        float firstSegmentStartX = (-NUM_BOARDS_PER_SEGMENT) * (BOARD_GAP + BOARD_WIDTH);
        float segmentLength = NUM_BOARDS_PER_SEGMENT * (BOARD_GAP + BOARD_WIDTH);

        // We need two segments so we can move on back to the front when it's out of view to simulate infinite movement
        TrackSegment1 = CreateTrackSegment(NUM_BOARDS_PER_SEGMENT, firstSegmentStartX);
        TrackSegment2 = CreateTrackSegment(NUM_BOARDS_PER_SEGMENT, firstSegmentStartX + segmentLength);
    }

    private GameObject CreateTrackSegment(int numBoards, float xStart)
    {
        MeshBuilder trackMeshBuilder = new MeshBuilder("tracks");
        int boardSubmesh = trackMeshBuilder.GetSubmesh("Materials/Brown");
        int beamSubmesh = trackMeshBuilder.GetSubmesh("Materials/Silver");

        for (int i = 0; i < numBoards; i++)
        {
            // Board
            float boardStartX = xStart + (i * (BOARD_GAP + BOARD_WIDTH));
            float boardStartZ = -(BOARD_LENGTH / 2);
            trackMeshBuilder.BuildCube(boardSubmesh, new Vector3(boardStartX, TRACK_Y, boardStartZ), new Vector3(BOARD_WIDTH, BOARD_HEIGHT, BOARD_LENGTH));

            // BEAM
            trackMeshBuilder.BuildCube(beamSubmesh, new Vector3(boardStartX, TRACK_Y + BOARD_HEIGHT, boardStartZ + BEAM_MARGIN), new Vector3(BOARD_WIDTH + BOARD_GAP, BEAM_HEIGHT, BEAM_WIDTH));
            trackMeshBuilder.BuildCube(beamSubmesh, new Vector3(boardStartX, TRACK_Y + BOARD_HEIGHT, boardStartZ + BOARD_LENGTH - BEAM_MARGIN), new Vector3(BOARD_WIDTH + BOARD_GAP, BEAM_HEIGHT, BEAM_WIDTH));
        }

        GameObject trackObject = trackMeshBuilder.ApplyMesh(addCollider: false);
        trackObject.transform.SetParent(transform);
        return trackObject;
    }

    public void CreateBackground()
    {
        BackgroundSegment1 = CreateBackgroundSegment();
        BackgroundSegment1.transform.position += new Vector3(-BACKGROUND_SEGMENT_LENGTH, 0f, 0f);
        BackgroundSegment2 = CreateBackgroundSegment();
    }


    private GameObject CreateBackgroundSegment()
    {
        MeshBuilder backgroundMeshBuilder = new MeshBuilder("background");
        int groundSubmesh = backgroundMeshBuilder.GetSubmesh("Materials/Green");
        int rockSubmesh = backgroundMeshBuilder.GetSubmesh("Materials/LightGreen");

        float startX = (BACKGROUND_SEGMENT_LENGTH / 2);
        float startZ = -20;
        float dimX = BACKGROUND_SEGMENT_LENGTH;
        float dimZ = 200;
        backgroundMeshBuilder.BuildPlane(groundSubmesh, new Vector3(startX, TRACK_Y, startZ), new Vector2(dimX, dimZ));

        // Rocks
        int numRocks = Random.Range(10, 600);
        for(int i = 0; i < numRocks; i++)
        {
            float posX = Random.Range(startX, startX + dimX);
            float posZ = Random.Range(BOARD_LENGTH + 2f, startZ + dimZ);

            float sizeY = Random.Range(2f, 10f);
            float sizeXZ = sizeY / 3f;

            backgroundMeshBuilder.BuildCube(rockSubmesh, new Vector3(posX - (sizeXZ / 2), TRACK_Y, posZ - (sizeXZ / 2)), new Vector3(sizeXZ, sizeY, sizeXZ));
        }

        GameObject bgObject = backgroundMeshBuilder.ApplyMesh(addCollider: false);
        bgObject.transform.SetParent(transform);
        return bgObject;
    }

    public void MoveWorld(float trainSpeed)
    {
        float offsetX = -trainSpeed * Time.deltaTime;
        TrackSegment1.transform.position += new Vector3(offsetX, 0f, 0f);
        TrackSegment2.transform.position += new Vector3(offsetX, 0f, 0f);
        BackgroundSegment1.transform.position += new Vector3(offsetX, 0f, 0f);
        BackgroundSegment2.transform.position += new Vector3(offsetX, 0f, 0f);

        if (TrackSegment1.transform.position.x <= -(SEGMENT_TOTAL_LENGTH_X * 0.5f)) TrackSegment1.transform.position += new Vector3(2 * SEGMENT_TOTAL_LENGTH_X, 0f, 0f);
        if (TrackSegment2.transform.position.x <= -(SEGMENT_TOTAL_LENGTH_X * 1.5f)) TrackSegment2.transform.position += new Vector3(2 * SEGMENT_TOTAL_LENGTH_X, 0f, 0f);

        float backgroundLimit = -2 * BACKGROUND_SEGMENT_LENGTH;
        if (BackgroundSegment1.transform.position.x < backgroundLimit)
        {
            float offset = BackgroundSegment1.transform.position.x - backgroundLimit;
            GameObject.Destroy(BackgroundSegment1.gameObject);
            BackgroundSegment1 = CreateBackgroundSegment();
            BackgroundSegment1.transform.position += new Vector3(offset, 0f, 0f);
        }
        if (BackgroundSegment2.transform.position.x < backgroundLimit)
        {
            float offset = BackgroundSegment2.transform.position.x - backgroundLimit;
            GameObject.Destroy(BackgroundSegment2.gameObject);
            BackgroundSegment2 = CreateBackgroundSegment();
            BackgroundSegment2.transform.position += new Vector3(offset, 0f, 0f);
        }
    }
}
