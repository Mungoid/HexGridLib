using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace com.bloodinthepixels.HexLib{
	public enum TerrainType {
		Aquatic = 0,
		Desert = 1,
		Beach = 2,
		Forest = 3,
		Arctic = 4,
		Volcanic = 5,
		Swamp = 6,
		Mountain = 7,
		Plains = 8,
		Dirt = 9
	}
public class HexGrid 
{


	float scale;
	bool orientation;
	List<CubeCoordinate> hexes;

	float SQRT_3_2 = Mathf.Sqrt(3) / 2;

	public HexGrid(float scale, bool orientation, List<CubeCoordinate> shape){
		this.scale = scale;
		this.orientation = orientation;
		this.hexes = shape;
	}

	/*public ScreenCoordinate hexToCenter(CubeCoordinate cube){
		if(this.orientation) 
			return new ScreenCoordinate(this.scale * (Grid.SQRT_3_2 * (cube.v.x + 0.5 * cube.v.z)),this.scale * (0.75 * cube.v.z)); 
		else return new ScreenCoordinate(this.scale * (0.75 * cube.v.x),this.scale * (Grid.SQRT_3_2 * (cube.v.z + 0.5 * cube.v.x)));
	}*/

	public HexWorldCoordinate HexToCenter(CubeCoordinate cube){
		if(this.orientation){
			return new HexWorldCoordinate(this.scale * (SQRT_3_2 * (cube.v.x + 0.5f * cube.v.z)), this.scale * (0.75f * cube.v.z), cube.v.z );
		}
		else 
			return new HexWorldCoordinate(this.scale * (0.75f * cube.v.x), this.scale * (SQRT_3_2 * (cube.v.z + 0.5f * cube.v.x)), this.scale * cube.v.z);
	}

	public float[] HexBounds(){
		HexGrid me = this;
		List<HexWorldCoordinate> centers = hexes.Select (x => me.HexToCenter(x)).ToList<HexWorldCoordinate>();
		var b1 = BoundsOfPoints(PolygonVertices());
		var b2 = BoundsOfPoints(centers);

		return new float[] { b1[0] + b2[0],  b1[1] + b2[1], b1[2] + b2[2], b1[3] + b2[3]};

	}

	public List<HexWorldCoordinate> PolygonVertices() {
		List<HexWorldCoordinate> points = new List<HexWorldCoordinate>();
		int _g = 0;
		while(_g < 6) {
			int i = _g++;
			float angle = 2 * Mathf.PI * (2 * i - (this.orientation?1:0)) / 12;
			points.Add(new HexWorldCoordinate(0.5f * this.scale * Mathf.Cos(angle),0, 0.5f * this.scale * Mathf.Sin(angle)));
		}
		return points;
	}

	/// <summary>
	/// Bounds the of points.
	/// </summary>
	/// <returns>Returns min/max points in array [minX, maxX, minY, maxY</returns>
	/// <param name="points">Points.</param>
	public float[] BoundsOfPoints(List<HexWorldCoordinate> points){
		float minX = 0.0f, minY = 0.0f, maxX = 0.0f, maxY = 0.0f;
		int _g = 0;
		while(_g < points.Count) {
			HexWorldCoordinate p = points[_g];
			++_g;
			if(p.x < minX) minX = p.x;
			if(p.x > maxX) maxX = p.x;
			if(p.y < minY) minY = p.y;
			if(p.y > maxY) maxY = p.y;
		}

		float[] tempf = new float[] {minX, maxX, minY, maxY};
		return tempf;
	}


	public CubeCoordinate TwoAxisToCube (HexCoordinate hex){
		return new CubeCoordinate(hex.q, -hex.r - hex.q, hex.r );
	}

	public HexCoordinate CubeToTwoAxis(CubeCoordinate cube){
		return new HexCoordinate((int)cube.v.x | 0, (int)cube.v.z | 0);
	}

	public CubeCoordinate OddQToCube(HexCoordinate hex){
		float x = hex.q;
		float z = hex.r - ((int)hex.q - ((int)hex.q & 1) >> 1);

		return new CubeCoordinate((int)x,(int)-x - (int)z,(int)z);
	}

	public HexCoordinate CubeToOddQ(CubeCoordinate cube){
		float x = (int)cube.v.x | 0;
		float z = (int)cube.v.z | 0;

		return new HexCoordinate(x,z + ((int)x - ((int)x & 1) >> 1));
	}

	public CubeCoordinate EvenQToCube(HexCoordinate hex){
		float x = hex.q; 
		float z = hex.r - ((int)hex.q + ((int)hex.q & 1) >> 1);
		
		return new CubeCoordinate(x,-x - z,z);
	}

	public HexCoordinate CubeToEvenQ(CubeCoordinate cube){
		float x = (int)cube.v.x | 0; 
		float z = (int)cube.v.z | 0;
		return new HexCoordinate(x,z + ((int)x + ((int)x & 1) >> 1));
	}

	public CubeCoordinate OddRToCube(HexCoordinate hex){
		float z = hex.r; 
		float x = hex.q - ((int)hex.r - ((int)hex.r & 1) >> 1);
		return new CubeCoordinate(x,-x - z,z);
	}


	public HexCoordinate CubeToOddR(CubeCoordinate cube) {
		float x = (int)cube.v.x | 0;
		float z = (int)cube.v.z | 0;
		return new HexCoordinate(x + ((int)z - ((int)z & 1) >> 1),z);
	}
	public CubeCoordinate EvenRToCube(HexCoordinate hex) {
		float z = hex.r;
		float x = hex.q - ((int)hex.r + ((int)hex.r & 1) >> 1);
		return new CubeCoordinate(x,-x - z,z);
	}
	public HexCoordinate CubeToEvenR(CubeCoordinate cube) {
		float x = (int)cube.v.x | 0;
		float z = (int)cube.v.z | 0;
		return new HexCoordinate(x + ((int)z + ((int)z & 1) >> 1),z);
	}

	public List<CubeCoordinate> TrapezoidalShape(int minQ, int maxQ, int minR, int maxR) {
		List<CubeCoordinate> hexes = new List<CubeCoordinate>();
		int _g1 = minQ; 
		int _g = maxQ + 1;
		while(_g1 < _g) {
			int q = _g1++;
			int _g3 = minR; 
			int _g2 = maxR + 1;
			while(_g3 < _g2) {
				int r = _g3++;
				hexes.Add(TwoAxisToCube(new HexCoordinate(q,r))); // TODO: What is toCube in orig js using 
			}
		}
		return hexes;
	}
	public List<CubeCoordinate> TriangularShape(int size) {
		List<CubeCoordinate> hexes = new List<CubeCoordinate>();
		int _g1 = 0;
		int _g = size + 1;
		while(_g1 < _g) {
			int k = _g1++;
			int _g3 = 0; 
			int _g2 = k + 1;
			while(_g3 < _g2) {
				var i = _g3++;
				hexes.Add(new CubeCoordinate(i,-k,k - i));
			}
		}
		return hexes;
	}

	public static List<CubeCoordinate> HexagonalShape(int size) {
		List<CubeCoordinate> hexes = new List<CubeCoordinate>();
		int _g1 = -size;
		int _g = size + 1;
		while(_g1 < _g) {
			int x = _g1++;
			int _g3 = -size ;
			int _g2 = size + 1;
			while(_g3 < _g2) {
				int y = _g3++;
				int z = -x - y;
				if(Mathf.Abs(x) <= size && Mathf.Abs(y) <= size && Mathf.Abs(z) <= size) hexes.Add(new CubeCoordinate(x,y,z));
			}
		}
		return hexes;
	}

}
}
