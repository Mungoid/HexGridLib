using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeCoordinate 
{
	public Vector3 v;
	static Vector3[] _direction = { new Vector3(1,-1,0),new Vector3(1,0,-1),new Vector3(0,1,-1),
		new Vector3(-1,1,0),new Vector3(-1,0,1),new Vector3(0,-1,1)};
	
	
	public CubeCoordinate(){
		v = new Vector3();
	}
	public CubeCoordinate(float x, float y, float z){
		v = new Vector3(x,y,z);


	}

	public CubeCoordinate(Vector3 coord){
		v = coord;
	}
	
	public CubeCoordinate Direction(HexDirection dir){
		return new CubeCoordinate(_direction[(int)dir].x + v.x,_direction[(int)dir].y + v.y,_direction[(int)dir].z + v.z);
	}

    public HexDirection GetDirectionFrom(CubeCoordinate from)
    {

        for (int i = 0; i < _direction.Length; ++i)
        {
			if ( v - from.v == _direction[i])
                return (HexDirection)i;
        }

        return HexDirection.East;
    }
	
	public CubeCoordinate[] GetNeighbors(){
		CubeCoordinate[] cc = {
			Direction (HexDirection.East),
			Direction (HexDirection.NorthEast),
			Direction (HexDirection.NorthWest),
			Direction (HexDirection.West),
			Direction (HexDirection.SouthWest),
			Direction (HexDirection.SouthEast)
		};

		return cc;
	}

	/// <summary>
	/// Calculates the range of hexes around the center one.
	/// </summary>
	/// <returns>The calculated list of cube coordinates for hexagons around the base hex. Note this returns a full range even if there are no tiles in game. Purely mathmatical</returns>
	/// <param name="dist">Distance from center hex</param>
	public List<CubeCoordinate> GetRange(int dist){
		int xmin = (int)v.x-dist;
		int xmax = (int)v.x+dist;
		int ymin = (int)v.y-dist;
		int ymax = (int)v.y+dist;
		int zmin = (int)v.z-dist;
		int zmax = (int)v.z+dist;

		List<CubeCoordinate> range = new List<CubeCoordinate> ();

		for (int x = xmin; x<= xmax; x++) {
			for(int y = Mathf.Max (ymin, -x-zmax); y<=Mathf.Min (ymax, -x-zmin); y++){
				int z = -x-y;
				range.Add (new CubeCoordinate(x,y,z));
			}
				
		}

		return range;
	}

	public List<CubeCoordinate> GetRing(int dist){
		int xmin = (int)v.x-dist;
		int xmax = (int)v.x+dist;
		int ymin = (int)v.y-dist;
		int ymax = (int)v.y+dist;
		int zmin = (int)v.z-dist;
		int zmax = (int)v.z+dist;
		
		List<CubeCoordinate> range = new List<CubeCoordinate> ();
		
		for (int x = xmin; x<= xmax; x++) {
			for(int y = Mathf.Max (ymin, -x-zmax); y<=Mathf.Min (ymax, -x-zmin); y++){
				int z = -x-y;
				range.Add (new CubeCoordinate(x,y,z));
			}
			
		}
		
		return range;
	}
	
	public static List<CubeCoordinate> MakeLine(CubeCoordinate a, CubeCoordinate b){
		CubeCoordinate d = a.Subtract(b);
		float[] tempVec = new float[3]{d.v.x, d.v.y, d.v.z};
		
		int N = 0; 
		int _g = 0;
		while(_g < 3) {
			int i = _g++;
			int j = (i + 1) % 3;
			int distance = Mathf.Abs((int)tempVec[i] - (int)tempVec[j]) | 0; 
			if(distance > N) N = distance;
		}
		List<CubeCoordinate> cubes = new List<CubeCoordinate>();
		CubeCoordinate prev = new CubeCoordinate(0,0,-999);
		int _g1 = 0;		
		_g = N + 1;
		
		while(_g1 < _g) {
			float i = _g1++;
			//Vector3 scale = new Vector3(i / N, i/ N, i/N);
			//d = Vector3.Scale (d, scale);
			Vector3 temp = new Vector3(d.v.x * (i/N), d.v.y * (i/N), d.v.z * (i/N));// A.add(d.scale(i / N)).round();
			CubeCoordinate c = a.Subtract( new CubeCoordinate(temp.x, temp.y, temp.z));
			
			c = c.Round();
			
			
			if(!c.Equals (prev)) {
				cubes.Add(c);
				prev = c;
			}
		}
		return cubes;
	}
	
	public string ToString(){
		return v.ToString ();
	}
	
	public bool Equals(CubeCoordinate other){
		return (v.x == other.v.x && v.y == other.v.y && v.z == other.v.z);
	}
	
	public CubeCoordinate Scale(float f) {
		return new CubeCoordinate(f * v.x, f * v.y,f * v.z);
	}
	public CubeCoordinate Add(CubeCoordinate other) {
		return new CubeCoordinate(v.x + other.v[0],v.y + other.v[1],v.z + other.v[2]);
	}
	public CubeCoordinate Subtract(CubeCoordinate other) {
		return new CubeCoordinate(v.x - other.v[0],v.y - other.v[1],v.z - other.v[2]);
	}
	public CubeCoordinate RotateLeft(CubeCoordinate other) {
		return new CubeCoordinate(-v.y,-v.z,-v.x);
	}
	public CubeCoordinate RotateRight(CubeCoordinate other) {
		return new CubeCoordinate(-v.z,-v.x,-v.y);
	}
	public float Length(CubeCoordinate other) {
		float len = 0.0f;
		//int _g = 0;
		
		if(Mathf.Abs(v.x) > len) len = Mathf.Abs (v.x);
		if(Mathf.Abs(v.y) > len) len = Mathf.Abs (v.y);
		if(Mathf.Abs(v.z) > len) len = Mathf.Abs (v.z);
		return len;
	}

	public CubeCoordinate Inverse(){
		return new CubeCoordinate (-v.x, -v.y, -v.z);
	}
	
	public CubeCoordinate Round(){
		float[] r = new float[3];
		float sum = 0;
		int _g = 0;
		
		float[] tempV = new float[3]{this.v.x, this.v.y, this.v.z};
		while(_g < 3) {
			int i = _g++;
			r[i] = Mathf.Round(tempV[i]);
			sum += r[i];
		}
		if(sum != 0) {
			float[] e = new float[3];
			int worst_i = 0;
			_g = 0;
			while(_g < 3) {
				int i = _g++;
				e[i] = Mathf.Abs(r[i] - tempV[i]);
				if(e[i] > e[worst_i]) worst_i = i;
			}
			r[worst_i] = -sum + r[worst_i];
		}
		return new CubeCoordinate(r[0],r[1],r[2]);
	}

	public string ToGameObjectName(){
		return "hex_" + v.x + "_" + v.y + "_" + v.z;
	}
	
	public enum HexDirection {
		East = 0,
		NorthEast = 1,
		NorthWest = 2,
		West = 3, 
		SouthWest = 4,
		SouthEast = 5
	}
}

