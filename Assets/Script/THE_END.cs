//using UnityEngine;
//using System.Collections;

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class THE_END : BaseMeshEffect {

	float time = 0;
	float time2 = 0;
	bool clicked = false;
	int CharacterCount = 0;
	int count = 0;
	float[] vy;
	float[] y;
	public float interval = 0.3f;
	public float upperPower = 5f;
	public float downPower = 0.5f;
	[SerializeField] Fade fade;
	void Start() {
		//fade = GetComponent<Fade> ();
		Text text = GetComponent<Text> ();
		CharacterCount = text.text.Length;
		vy = new float[CharacterCount];
		y = new float[CharacterCount];
	}
	public override void ModifyMesh ( UnityEngine.UI.VertexHelper vh)
	{
		if (!IsActive())
			return;

		List<UIVertex> vertices = new List<UIVertex>();
		vh.GetUIVertexStream(vertices);

		TextMove(ref vertices);

		vh.Clear();
		vh.AddUIVertexTriangleStream(vertices);
	}

	void TextMove( ref List<UIVertex> vertices )
	{
		if (true) {
			time += Time.deltaTime;
			if (time > interval) {		//時間ごとに count を　増やす
				if (y [count] <= 0) {						//現在のcount の重力を設定。
					vy [count] = upperPower;
				}

				if (count < vy.Length - 1) {
					count++;
					time = 0;
				}

				if (count >= vy.Length - 1 && time > interval + 1f)
					count = 0;


			}
			//

			for (int i = 0; i < vy.Length; i++) {		//全ての文字に対して
				y [i] += vy [i];				//	重力をかける
				if (y [i] > 0) {				//	まだ浮いていたら
					vy [i] -= downPower;				//	重力を強める
				} else {						//	浮いていなかったら
					vy [i] = 0f;					//  重力を0にして、初期の位置に戻す
					y [i] = 0f;

				}
			}
			for (int c = 0; c < vertices.Count; c += 6) {		//全ての頂点に対して





				for (int i = 0; i < 6; i++) {

					var vert = vertices [c + i];
					Vector3 dir = new Vector3 (0, y [c / 6]);		
					//		Debug.Log (vert.position);
					vert.position += dir;
					vertices [c + i] = vert;
				}
			}
		} else {						//--------------------------------クリックした後のMove

			for (int i = 0; i < vy.Length; i++) {		//全ての文字に対して
				y [i] += vy [i];				//	重力をかける
				vy [i] -= downPower;				//	重力を強める

			}

			for (int c = 0; c < vertices.Count; c += 6) {		//全ての頂点に対して

				for (int i = 0; i < 6; i++) {

					var vert = vertices [c + i];
					Vector3 dir = new Vector3 (( c / 6 - (CharacterCount / 2) )* (time2 +20 ) * (time2 + 1), y [c / 6]);		
					//		Debug.Log (vert.position);
					vert.position += dir;
					vertices [c + i] = vert;
				}
			}



		}
	}

	void Update()
	{

		time2 += Time.deltaTime;
		if (Input.GetMouseButtonDown (0) && !clicked && time2 > 6) {
			clicked = true;
			fade.FadeIn (3f);
			time2 = 0;
		}
		if(time2 > 6f && clicked) {
			Application.Quit();
		}
		base.GetComponent<Graphic> ().SetVerticesDirty ();

	}


	void Explosion() {
		time2 += Time.deltaTime;
		if (time2 > 2) {
			GameManager.isTitle = false;
			Destroy (gameObject);
		}
	}
}