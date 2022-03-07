using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NormalizeMode { Local, Global };

[CreateAssetMenu()]
public class NoiseData : UpdatableData, IEquatable<NoiseData>
{
	public NormalizeMode normalizeMode;
	public float noiseScale;
	public int octaves;
	[Range(0, 1)]
	public float persistance;
	public float lacunarity;

	public int seed;

	public Vector2 offset;

	public bool Equals(NoiseData other)
	{
		if(other.normalizeMode == normalizeMode &&
		   other.noiseScale    == noiseScale    &&
		   other.octaves       == octaves       &&
		   other.persistance   == persistance   &&
		   other.lacunarity    == lacunarity    &&
		   other.seed          == seed)
		{
			return true;
		}
		return false;
	}

	protected override void OnValidate()
	{
		if (lacunarity < 1)
		{
			lacunarity = 1;
		}
		if (octaves < 0)
		{
			octaves = 0;
		}

		base.OnValidate();
	}
}
