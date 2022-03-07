using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Utils;
using System.Threading;

public class EnvironmentGenerator : MonoBehaviour
{
	public float heightMargin;
	ProceduralObjectGenerator generator;
	public List<ProceduralObjectData> environmentObjectList;
	public List<ProceduralObjectData> itemObjectList;

	Queue<Action> spawnGameObjectsCallbackQueue = new Queue<Action>();

	ChunkNoiseMapCache cache;

	static EnvironmentGenerator instance;

	[Header("Debug")]
	public bool emptyEnvironmentPrefabs;
	public bool emptyItemPrefabs;

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
		cache = new ChunkNoiseMapCache();

		if(emptyEnvironmentPrefabs)
		{
			environmentObjectList = new List<ProceduralObjectData>();
		}
		if(emptyItemPrefabs)
		{
			itemObjectList = new List<ProceduralObjectData>();
		}	
	}

	public static EnvironmentGenerator GetInstance()
	{
		return instance;
	}

	private void Start()
	{
		generator = new ProceduralObjectGenerator((int)MapGenerator.GetInstance().GetRealWorldChunkSize());
	}

	private void Update()
	{
		if (spawnGameObjectsCallbackQueue.Count > 0)
		{
			for (int i = 0; i < spawnGameObjectsCallbackQueue.Count; i++)
			{
				Action callback = spawnGameObjectsCallbackQueue.Dequeue();
				callback();
			}
		}
	}

	List<GameObjectSpawnData> GetSpawnDataListFromSheet(Vector2 chunkPosition, Transform parent, List<ProceduralObjectData> sheet)
	{
		List<GameObjectSpawnData> spawnDataList = new List<GameObjectSpawnData>();
		foreach (var objData in sheet)
		{
			spawnDataList.Add(new GameObjectSpawnData(chunkPosition, parent, objData));
		}
		return spawnDataList;
	}


	IEnumerator WaitUntilPlayerIsInZoneThenRequestSpawnItemObjects(LODMesh lodMesh, Action<List<GameObject>> callback, 
		float distanceToSpawnItems, Vector2 chunkPosition, Transform parent)
	{
		yield return new WaitUntil(() => 
		{
			Vector2 playerPosV2 = new Vector2(GameObjectRefs.player.position.x, GameObjectRefs.player.position.z);
			if((playerPosV2 - chunkPosition).sqrMagnitude <= distanceToSpawnItems*distanceToSpawnItems)
			{
				return true;
			}
			return false;
		});

		List<GameObjectSpawnData> spawnDataList = GetSpawnDataListFromSheet(chunkPosition, parent, itemObjectList);
		StartCoroutine(WaitUntilLodMeshHasMeshThenSpawn(spawnDataList, lodMesh, callback));
	}


	public List<GameObject> SpawnObjectsWithDataHolder(Transform parent, List<GameObjectDataHolder> dataHolder)
	{
		return generator.Generate(parent, dataHolder);
	}

	public void RequestItemSpawn(Vector2 chunkPosition, Transform parent, LODMesh lodMesh, Action<List<GameObject>> callback,
		float distanceToSpawn)
	{
		StartCoroutine(WaitUntilPlayerIsInZoneThenRequestSpawnItemObjects(lodMesh, callback, distanceToSpawn, chunkPosition, parent));
	}

	public void RequestEnvironmentSpawn(Vector2 chunkPosition, Transform parent, LODMesh lodMesh, Action<List<GameObject>> callback)
	{
		List<GameObjectSpawnData> spawnDataList = GetSpawnDataListFromSheet(chunkPosition, parent, environmentObjectList);
		StartCoroutine(WaitUntilLodMeshHasMeshThenSpawn(spawnDataList, lodMesh, callback));
	}

	IEnumerator WaitUntilLodMeshHasMeshThenSpawn(List<GameObjectSpawnData> spawnDataList, LODMesh lodMesh, Action<List<GameObject>> callback)
	{
		yield return new WaitUntil(() => lodMesh.hasMesh);

		ThreadStart threadStart = delegate {
			SpawnGameObjectsThread(spawnDataList, callback);
		};

		new Thread(threadStart).Start();
	}


	void SpawnGameObjectsThread(List<GameObjectSpawnData> spawnDataList, Action<List<GameObject>> callback)
	{
		List<GameObjectSpawnData> spawnDataListFilled = GetNoiseMapForSpawnDataList(spawnDataList);

		Action wrappedCallback = () =>
		{
			List<GameObject> gameObjectList = GetSpawnedGameObjects(spawnDataListFilled);

			callback(gameObjectList);
		};

	
		lock (spawnGameObjectsCallbackQueue)
		{
			spawnGameObjectsCallbackQueue.Enqueue(wrappedCallback);
		}
	}

	List<GameObjectSpawnData> GetNoiseMapForSpawnDataList(List<GameObjectSpawnData> spawnDataList)
	{
		List<GameObjectSpawnData> spawnDataListFilled = new List<GameObjectSpawnData>();
		SystemRandom random = new SystemRandom(1);

		for (int i = 0; i < spawnDataList.Count; ++i)
		{
			NoiseData noiseData = spawnDataList[i].objectData.noiseData;
			Vector2 chunkPosition = spawnDataList[i].chunkPosition;

			int heightMapSideSize = ProceduralPointGenerator.GetHeightMapSizeBasedOnGrid(spawnDataList[i].objectData.radius,
				(int)MapGenerator.GetInstance().GetRealWorldChunkSize());

			float[,] map;

			/*if (cache.Contains(noiseData, chunkPosition))
			{
				map = cache.GetMap(noiseData, chunkPosition);
			}
			else
			{
				// This so expensive
				map = Noise.GenerateNoiseMap(noiseData,
					  heightMapSideSize, random, chunkPosition);
				cache.AddElement(noiseData, chunkPosition, map);
			}*/

			map = Noise.GenerateNoiseMap(noiseData,
				  heightMapSideSize, random, chunkPosition);

			// --------------- Testing ---------------
			//DeterminismComparer.GetInstance().AddNoiseMap(map);
			// ---------------------------------------

			GameObjectSpawnData spawnData = new GameObjectSpawnData(spawnDataList[i].chunkPosition, 
				spawnDataList[i].parent, spawnDataList[i].objectData);
			spawnData.heightMap = map;
			spawnDataListFilled.Add(spawnData);
		}

		cache.Clear();
		return spawnDataListFilled;
	}


    List<GameObject> GetSpawnedGameObjects(List<GameObjectSpawnData> spawnDataList)
	{
		List<ObjectsWithHeightFix> objectListWithHeightFix = new List<ObjectsWithHeightFix>();
		List<GameObject> spawnedGameObjects = new List<GameObject>();

		foreach (var spawnData in spawnDataList)
		{
			var list = generator.Generate(spawnData.chunkPosition, spawnData.parent, spawnData.objectData, spawnData.heightMap, 
				spawnData.objectData.noiseData.seed, heightMargin);
			objectListWithHeightFix.Add(new ObjectsWithHeightFix(list, spawnData.objectData.heightFix));
			spawnedGameObjects = spawnedGameObjects.Concat(list).ToList();
		}

		return spawnedGameObjects;
	}
}

public class ChunkNoiseMapCache
{
	List<ChunkNoiseMap> cache;

	public ChunkNoiseMapCache()
	{
		cache = new List<ChunkNoiseMap>();
	}

	public void Clear()
	{
		cache.Clear();
	}

	public void AddElement(NoiseData noiseData, Vector2 chunkPosition, float[,] heightMap)
	{
		lock (cache)
		{
			cache.Add(new ChunkNoiseMap(noiseData, chunkPosition, heightMap));
		}
	}

	public float[,] GetMap(NoiseData noiseData, Vector2 chunkPosition)
	{
		lock (cache)
		{
			foreach (var item in cache)
			{
				if (item.Match(noiseData, chunkPosition))
				{
					return item.heightMap;
				}
			}
		}

		Debug.LogError("[NoiseMap cache] cache does not contain heightMap");
		return null;
	}


	public bool Contains(NoiseData noiseData, Vector2 chunkPosition)
	{
		lock (cache)
		{
			foreach (var item in cache.ToList())
			{
				if (item.Match(noiseData, chunkPosition))
				{
					return true;
				}
			}
		}
		return false;
	}

	private class ChunkNoiseMap
	{
		NoiseData noiseData;
		Vector2 chunkPosition;
		public float[,] heightMap;

		public ChunkNoiseMap(NoiseData noiseData, Vector2 chunkPosition, float[,] heightMap)
		{
			this.noiseData = noiseData;
			this.chunkPosition = chunkPosition;
			this.heightMap = heightMap;
		}

		public bool Match(NoiseData noiseData, Vector2 chunkPosition)
		{
			if (this.noiseData.Equals(noiseData) && this.chunkPosition == chunkPosition)
			{
				return true;
			}
			return false;
		}
	}

}

public struct GameObjectSpawnData
{
	public float[,] heightMap;
	public Vector2 chunkPosition;
	public Transform parent;
	public ProceduralObjectData objectData;

	public GameObjectSpawnData(Vector2 chunkPosition, Transform parent, ProceduralObjectData objectData)
	{
		heightMap = null;
		this.chunkPosition = chunkPosition;
		this.parent = parent;
		this.objectData = objectData;
	}
}