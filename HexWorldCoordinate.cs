using UnityEngine;
using System.Collections;
namespace com.bloodinthepixels.HexLib{
public class HexWorldCoordinate 
{
	public float x;
	public float y;
	public float z;

	public HexWorldCoordinate(){
		x = 0;
		y = 0;
		z = 0;
	}
	public HexWorldCoordinate(float x, float y, float z){
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3 ToVector3 (){
		return new Vector3 (x, y, z);
	}

	public bool Equals(HexWorldCoordinate p){
		return this.x == p.x && this.y == p.y && this.z == p.z;
	}

	public string ToString(){
		return new Vector3(x,y,z).ToString ();
	}

	public float LengthSquared(){
		return this.x * this.x + this.y * this.y + this.z * this.z;
	}

	public float Length(){
		return Mathf.Sqrt(this.LengthSquared());
	}

	public HexWorldCoordinate Normalize(){
		float d = Length ();
		return new HexWorldCoordinate(this.x / d, this.y / d, this.z / d);
	}

	public HexWorldCoordinate Scale(float d){
		return new HexWorldCoordinate(this.x * d, this.y * d, this.z * d);
	}

	public HexWorldCoordinate RotateLeft(){
		// TODO: This may not be right...
		return new HexWorldCoordinate(this.y, -this.x, this.z);
	}

	public HexWorldCoordinate RotateRight(){
		// TODO: This may not be right...
		return new HexWorldCoordinate(-this.y, this.x, this.z);
	}

	public HexWorldCoordinate Add(HexWorldCoordinate p){
		return new HexWorldCoordinate(this.x + p.x, this.y + p.y, this.z + p.z);
	}

	public HexWorldCoordinate Subtract(HexWorldCoordinate p){
		return new HexWorldCoordinate(this.x - p.x, this.y - p.y, this.z - p.z);
	}

	public float Dot(HexWorldCoordinate p){
		return this.x * p.x + this.y * p.y + this.z + p.z;
	}

	public float Cross(HexWorldCoordinate p){
		return this.x * p.x - this.y * p.y - this.z - p.z;
	}

	public float Distance(HexWorldCoordinate p){
		return this.Subtract(p).Length();
	}


}

}