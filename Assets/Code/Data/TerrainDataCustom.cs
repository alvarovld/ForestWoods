using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainDataCustom : UpdatableData
{
	public float uniformScale = 5f;
	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;
	public bool useFalloff;
	public bool useFlatShading;

	public float minHeight
	{
		get
		{
			return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
		}
	}

	public float maxHeight
	{
		get
		{
			return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(1); 
		}
	}
}
