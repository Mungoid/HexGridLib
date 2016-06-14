using UnityEngine;
using System.Collections;
namespace com.bloodinthepixels.HexLib{
public class HexCoordinate 
{
	public float q;
	public float r;

	public HexCoordinate(float q, float r){
		this.q = q;
		this.r = r;
	}

	public string ToString()
	{
		return new Vector2(this.q, this.r).ToString ();
	}
}

}