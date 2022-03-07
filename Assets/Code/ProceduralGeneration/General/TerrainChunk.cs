using UnityEngine;
using System.Collections;
using Utils;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Linq;

public class TerrainChunk
{
	GameObject meshObject;
	Vector2 position;
	Bounds bounds;

	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	MeshCollider meshCollider;

	LODInfo[] detailLevels;
	LODMesh[] lodMeshes;
	LODMesh collisionLODMesh;

	MapData mapData;
	bool mapDataReceived;
	int previousLODIndex = -1;
	bool setParentToSpawnedObjs;

	List<GameObject> spawnedObjects = null;
	List<GameObjectDataHolder> dataHolder = null;

	float distanceToSpawnItems;

	float maxViewDst;

	public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material, 
		float maxViewDst, bool setParentToSpawnedObjs, float distanceToSpawnItems)
	{
		this.maxViewDst = maxViewDst;
		this.detailLevels = detailLevels;
		this.setParentToSpawnedObjs = setParentToSpawnedObjs;
		this.distanceToSpawnItems = distanceToSpawnItems;

		position = coord * size;
		bounds = new Bounds(position, Vector2.one * size);
		Vector3 positionV3 = new Vector3(position.x, 0, position.y);
		FillMeshObjectComponents(material, positionV3, parent);
		FillLODMeshes();
		MapGenerator.GetInstance().RequestMapData(position, OnMapDataReceived);
	}

	public bool HasSpawnedObjects()
	{
		return spawnedObjects != null;
	}

	public bool IsRendered()
	{
		return meshFilter.mesh != null && meshFilter.mesh.vertices.Length != 0;
	}

	public bool IsVisibleByPlayer(Vector2 playerPos)
	{
		float offsetDstToDisableObject = 0;

		if (meshObject.activeSelf)
		{
			offsetDstToDisableObject = 80;
		}

		float playerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPos));
		return playerDstFromNearestEdge <= maxViewDst + offsetDstToDisableObject;
	}

	public static bool IsVisibleByPlayer(Vector2 playerPos, int size, float maxViewDst, Vector2 coord)
	{
		Vector2 position = coord * size;
		Bounds bounds = new Bounds(position, Vector2.one * size);
		float playerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPos));
		return playerDstFromNearestEdge <= maxViewDst;
	}

	LODMesh GetMaxDetailLODMesh()
	{
		foreach(var lodMes in lodMeshes)
		{
			if(lodMes.lod == 0)
			{
				return lodMes;
			}
		}
		return null;
	}

	void FillMeshObjectComponents(Material material, Vector3 positionV3, Transform parent)
	{
		meshObject = new GameObject("Terrain Chunk");
		meshObject.layer = 9;
		meshObject.gameObject.tag = GameData.Tags.Terrain;
		meshRenderer = meshObject.AddComponent<MeshRenderer>();
		meshFilter = meshObject.AddComponent<MeshFilter>();
		meshCollider = meshObject.AddComponent<MeshCollider>();
		meshRenderer.material = material;

		meshObject.transform.position = positionV3 * MapGenerator.GetInstance().terrainData.uniformScale;
		meshObject.transform.parent = parent;
		meshObject.transform.localScale = Vector3.one * MapGenerator.GetInstance().terrainData.uniformScale;
	}

	void FillLODMeshes()
	{
		lodMeshes = new LODMesh[detailLevels.Length];
		for (int i = 0; i < detailLevels.Length; i++)
		{
			lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateLODMesh);
			if (detailLevels[i].useForCollider)
			{
				collisionLODMesh = lodMeshes[i];
			}
		}
	}

	void OnMapDataReceived(MapData mapData)
	{
		this.mapData = mapData;
		mapDataReceived = true;

		RequestGameObjectsSpawn();
		RequestItemObjectSpawn();
		UpdateTerrainChunk();
	}

	void RequestItemObjectSpawn()
	{
		Vector2 chunkPosition = GetChunkPosition();
		Transform parent = null;
		if (setParentToSpawnedObjs)
		{
			parent = meshObject.transform;
		}

		EnvironmentGenerator.GetInstance().RequestItemSpawn(chunkPosition, parent, GetMaxDetailLODMesh(), OnGameObjectsSpawned, distanceToSpawnItems);
	}

	int GetLODIndex(float playerDstFromNearestEdge)
	{
		int lodIndex = 0;

		for (int i = 0; i < detailLevels.Length - 1; ++i)
		{
			if (playerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
			{
				lodIndex += 1;
			}
			else
			{
				break;
			}
		}
		return lodIndex;
	}

	void UpdateLODMesh()
	{
		float playerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(EndlessTerrain.GetInstance().playerPosition));
		int lodIndex = GetLODIndex(playerDstFromNearestEdge);

		if (lodIndex != previousLODIndex)
		{
			LODMesh lodMesh = lodMeshes[lodIndex];
			if (lodMesh.hasMesh)
			{
				previousLODIndex = lodIndex;
				meshFilter.mesh = lodMesh.mesh;
			}
			else if (!lodMesh.hasRequestedMesh)
			{
				lodMesh.RequestMesh(mapData);
			}
		}

		if (lodIndex == 0)
		{
			SetCollisionMesh();
		}
	}

	void SetCollisionMesh()
	{
		if (collisionLODMesh.hasMesh)
		{
			meshCollider.sharedMesh = collisionLODMesh.mesh;
		}
		else if (!collisionLODMesh.hasRequestedMesh)
		{
			collisionLODMesh.RequestMesh(mapData);
		}
	}

	public void UpdateTerrainChunk()
	{
		if (!mapDataReceived)
		{
			return;
		}

		bool visible = IsVisibleByPlayer(EndlessTerrain.GetInstance().playerPosition);
		SetChunkVisibility(visible);

		if (!visible)
		{
			return;
		}

		UpdateLODMesh();
	}

	public void SetChunkVisibility(bool visible)
	{
		meshObject.SetActive(visible);
		
		if(visible && spawnedObjects == null && dataHolder != null)
		{
			ShowSpawnedObjects();
			return;
		}
		if(!visible && spawnedObjects != null)
		{
			HideSpawnedObjects();
			return;
		}
	}

	void HideSpawnedObjects()
	{
		if(spawnedObjects == null)
		{
			return;
		}

		ObjectPoolManager.GetInstance().ReturnListObjectToPoolDisabling(spawnedObjects);
		spawnedObjects.Clear();
		spawnedObjects = null;
	}

	public bool IsVisible()
	{
		return meshObject.activeSelf;
	}

	void ShowSpawnedObjects()
	{
		spawnedObjects = EnvironmentGenerator.GetInstance().SpawnObjectsWithDataHolder(meshObject.transform, dataHolder);
	}


	Vector2 GetChunkPosition()
	{
		return position * MapGenerator.GetInstance().terrainData.uniformScale; 
	}

	void RequestGameObjectsSpawn()
	{
		Vector2 chunkPosition = GetChunkPosition();
		Transform parent = null;
		if(setParentToSpawnedObjs)
		{
			parent = meshObject.transform;
		}

		EnvironmentGenerator.GetInstance().RequestEnvironmentSpawn(chunkPosition, parent, GetMaxDetailLODMesh(), OnGameObjectsSpawned);
	}

	void OnGameObjectsSpawned(List<GameObject> gameObjectList)
	{
		if (spawnedObjects == null)
		{
			spawnedObjects = gameObjectList;
		}
		else
		{
			spawnedObjects = spawnedObjects.Concat(gameObjectList).ToList();
		}

		dataHolder = GetSpawnDataHolder(spawnedObjects);
	}

	List<GameObjectDataHolder> GetSpawnDataHolder(List<GameObject> gameObjectList)
	{
		List<GameObjectDataHolder> dataHolder = new List<GameObjectDataHolder>();

		foreach (var obj in gameObjectList)
		{
			dataHolder.Add(new GameObjectDataHolder(obj.tag, obj.transform.position, obj.transform.rotation, obj.transform.lossyScale));
		}

		return dataHolder;
	}

}

[System.Serializable]
public struct GameObjectDataHolder
{
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;
	public string tag;

	public GameObjectDataHolder(string tag, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		this.tag = tag;
		this.position = position;
		this.rotation = rotation;
		this.scale = scale;
	}
}



public class LODMesh
{
	public Mesh mesh;
	public bool hasRequestedMesh;
	public bool hasMesh;
	public int lod;
	System.Action updateCallback;



	public LODMesh(int lod, System.Action updateCallback)
	{
		this.lod = lod;
		this.updateCallback = updateCallback;
	}


	void OnMeshDataReceived(MeshData meshData)
	{
		mesh = meshData.CreateMesh();
		hasMesh = true;
		updateCallback();
	}

	public void RequestMesh(MapData mapData)
	{
		hasRequestedMesh = true;
		MapGenerator.GetInstance().RequestMeshData(mapData, lod, OnMeshDataReceived);
	}
}

[System.Serializable]
public struct LODInfo
{
	public int lod;
	public float visibleDstThreshold;
	public bool useForCollider;
}

public struct ObjectsWithHeightFix
{
	public List<GameObject> objects;
	public float heightFix;

	public ObjectsWithHeightFix(List<GameObject> objects, float heightFix)
	{
		this.objects = objects;
		this.heightFix = heightFix;
	}
}