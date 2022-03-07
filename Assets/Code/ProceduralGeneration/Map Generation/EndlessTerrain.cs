using UnityEngine;
using System.Collections.Generic;
using Utils;
using System.Collections;
using System.Collections.ObjectModel;
using System;

public class EndlessTerrain : MonoBehaviour
{
	const float viewerMoveThresholdForChunkUpdate = 25f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

	public LODInfo[] detailLevels;
	public static float maxViewDst;

	public Transform player;
	public Material mapMaterial;

	public bool setSpawnedObjectsParent;

	public float distanceToSpawnItems;

	public Vector2 playerPosition;
	Vector2 playerPositionOld;
	static MapGenerator mapGenerator;
	int chunkSize;
	int chunksVisibleInViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();

	public event EventHandler InitialMapLoaded;


	private static EndlessTerrain instance = null;


	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		mapGenerator = MapGenerator.GetInstance();
		maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
		chunkSize = mapGenerator.mapChunkSize - 1;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

#if !UNITY_EDITOR
		setSpawnedObjectsParent = false;
#endif


		UpdateVisibleChunks();
		StartCoroutine(InitalMapLoadCompleted());
	}

	void Update()
	{
		playerPosition = new Vector2(player.position.x, player.position.z) / mapGenerator.terrainData.uniformScale;

		if ((playerPositionOld - playerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
		{
			playerPositionOld = playerPosition;
			UpdateVisibleChunks();
		}
	}



	IEnumerator InitalMapLoadCompleted()
	{
		yield return new WaitUntil(() => IsMapLoaded());
		OnInitialMapLoaded(EventArgs.Empty);
	}

	void OnInitialMapLoaded(EventArgs e)
	{
		InitialMapLoaded?.Invoke(this, e);
	}


	public static EndlessTerrain GetInstance()
	{
		return instance;
	}



	void DisableNotVisibleChunks()
	{
		foreach (var key in terrainChunkDictionary.Keys)
		{
			TerrainChunk chunk = terrainChunkDictionary[key];
			if (!chunk.IsVisibleByPlayer(playerPosition))
			{
				chunk.SetChunkVisibility(false);
			}
		}
	}

	public bool IsMapLoaded()
	{
		if(terrainChunkDictionary.Count == 0)
		{
			return false;
		}
		foreach (var key in terrainChunkDictionary.Keys)
		{
			if (!terrainChunkDictionary[key].IsRendered())
			{
				return false;
			}
		}

		return true;
	}


	void UpdateVisibleChunks()
	{
		DisableNotVisibleChunks();

		Vector2 currentChunkCoordinates;

		currentChunkCoordinates.x = Mathf.RoundToInt(playerPosition.x / chunkSize);
		currentChunkCoordinates.y = Mathf.RoundToInt(playerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordinates.x + xOffset, currentChunkCoordinates.y + yOffset);

				if (!TerrainChunk.IsVisibleByPlayer(playerPosition, chunkSize, maxViewDst, viewedChunkCoord) &&
					!terrainChunkDictionary.ContainsKey(viewedChunkCoord))
				{
					continue;
				}

				if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
				{
					terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
				}
				else
				{
					terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, 
						transform, mapMaterial, maxViewDst, setSpawnedObjectsParent, distanceToSpawnItems));
				}
			}
		}
	}
}
