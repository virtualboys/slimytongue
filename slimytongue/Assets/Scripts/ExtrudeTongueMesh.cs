using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExtrudeTongueMesh : ProcBase {

	public int numSegmentsPerLink;
	public int numRadialSegments;
	public float radius;

	private TongueController m_tongueController;
	private GameObject m_tongueTip;

	private List<Transform> cps;
	private List<Vector3> ps;
	private List<Vector3> ts;
	private List<Vector3> ms;

	private MeshBuilder meshBuilder;

	// Use this for initialization
	public override void Init () {
        m_tongueController = GetComponentInParent<TongueController>();

		cps = new List<Transform> ();
		ps = new List<Vector3> ();
		ts = new List<Vector3> ();
		ms = new List<Vector3> ();

		Mesh mesh = new Mesh ();
		meshBuilder = new MeshBuilder (mesh);
		SetMesh (mesh);

		m_tongueTip = m_tongueController.GetTongueTip ();
	}

	// Update is called once per frame
	void Update () {
		meshBuilder.ResetGeometry ();
        //mesh

		cps = m_tongueController.GetControlPoints ();
		if (cps.Count <= 1) {
			return;
		}

		ms.Clear ();
		ps.Clear ();
		ts.Clear ();

		// construct splines
		for (int i = 0; i < cps.Count; i++) {
			ms.Add (finiteDiffSlope (i));
		}

		//ms [0] = m_tongueTip.transform.forward;
			
		Vector3 xPrev, pPrev, d, x, p, cp, cp1, m, m1;
		for (int i = 0; i < cps.Count; i++) {

			cp = cps [i].localPosition;
			m = ms [i];
			if (i == cps.Count - 1) {
				cp1 = m_tongueTip.transform.localPosition;
				m1 = -(m_tongueTip.transform.localRotation * Vector3.forward);
			} else {
				cp1 = cps [i + 1].localPosition;
				m1 = ms [i + 1];
			}

			d = cp1 - cp;
			float l = d.magnitude / numSegmentsPerLink;
			d.Normalize ();

			xPrev = cp;
			pPrev = cp;

			for (int j = 0; j < numSegmentsPerLink; j++) {
				x = xPrev + d * l;
				p = evalSpline3D (x, cp, cp1, m, m1);

				ps.Add (p);
				ts.Add ((p - pPrev).normalized);

				xPrev = x;
				pPrev = p;
			}
		}

		UpdateMesh ();
	}

	//Build the mesh:
	/*public override Mesh BuildMesh() {
		MeshBuilder meshBuilder = new MeshBuilder ();

		Vector3 axis;
		Quaternion rot;
		for (int i = 0; i < ps.Count; i++) {
			axis = Vector3.Cross (Vector3.up, ts[i]);
			float angle = Vector3.Angle (Vector3.up, ts [i]);
			rot = Quaternion.AngleAxis (angle, axis);
			BuildRing (meshBuilder, numRadialSegments, ps [i] - transform.localPosition, radius, i / ((float)ps.Count), i > 0, rot);
		}

		return meshBuilder.CreateMesh ();
	}*/

	public void UpdateMesh() {

		BeginMeshEdit (meshBuilder.mesh);

		Vector3 axis;
		Quaternion rot;
		for (int i = 0; i < ps.Count; i++) {
			axis = Vector3.Cross (Vector3.up, ts[i]);
			float angle = Vector3.Angle (Vector3.up, ts [i]);
			rot = Quaternion.AngleAxis (angle, axis);
			BuildRing (meshBuilder, numRadialSegments, ps [i] - transform.localPosition, radius, i / ((float)numSegmentsPerLink), i > 0, rot);
			//UpdateRing (mesh, numRadialSegments, ps [i] - transform.localPosition, radius, i, rot);
		}

		EndMeshEdit (meshBuilder.mesh);
		meshBuilder.UpdateMesh ();
	}

	public Vector3 evalSpline3D(Vector3 pos, Vector3 cp, Vector3 cp1, Vector3 mk, Vector3 mk1) {
		return new Vector3 (evalSpline1D (pos [0], cp [0], cp1 [0], mk [0], mk1 [0]), 
							evalSpline1D (pos [1], cp [1], cp1 [1], mk [1], mk1 [1]), 
							evalSpline1D (pos [2], cp [2], cp1 [2], mk [2], mk1 [2]));
	}

	/// <param name="coord">0 for x, 1 for y, 2 for z</param>
	public float evalSpline1D(float x, float xk, float xk1, float mk, float mk1) {
		//float xk = cps [k].localPosition [coord];
		//float xk1 = cps [k + 1].localPosition [coord];
		float interval = xk1 - xk;

		if (interval == 0) {
			return xk;
		}

		float t = (x - xk) / interval;

		float h00 = (1 + 2 * t) * (1 - t) * (1 - t);
		float h10 = t * (1 - t) * (1 - t);
		float h01 = t * t * (3 - 2 * t);
		float h11 = t * t * (t - 1);

		float px = h00 * xk + h10  * mk
			+ h01 * xk1 + h11  * mk1;
		return px;
	}

	// is tk+1 - t necessarily 1??
	public Vector3 finiteDiffSlope(int index) {
		Vector3 m;
		if (index == 0) {
			m = cps [1].localPosition - cps [0].localPosition;
		} else if (index == cps.Count - 1) {
			m = cps [cps.Count - 1].localPosition - cps [cps.Count - 2].localPosition;
		} else {
			m = .5f * ((cps [index + 1].localPosition - cps [index].localPosition)
				+ (cps [index].localPosition - cps [index - 1].localPosition));
		}
		return m;
	}
}
