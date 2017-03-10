using UnityEngine;
using System.Collections;


namespace ProjZombie
{
	[ExecuteInEditMode]
	[RequireComponent(typeof (Camera))]
	public class Decal : MonoBehaviour{
		
		public int decalTexSize = 256;
		public string layerName = "Decal";
		int oldDecalTexSize;
		
		public Material[] decalMaterials;
		
		public Color decalBGColor = new Color(0.0f, 0.0f, 0.0f, 1f);
		
		Camera decalCamera;
		
		RenderTexture decalTexture;
		
		void Start()
		{
		}
		
		
		void OnPreRender()
		{
			if (decalTexture == null || oldDecalTexSize != decalTexSize) {
				if(decalTexture)
				{
					DestroyImmediate(decalTexture);
				}
				decalTexture = new RenderTexture(decalTexSize, decalTexSize, 0);
				decalTexture.hideFlags = HideFlags.HideAndDontSave;
				oldDecalTexSize = decalTexSize;
			}
			
			if (decalCamera == null) {
				GameObject cameraObj = new GameObject("decalCamera", typeof(Camera));
				cameraObj.hideFlags = HideFlags.HideAndDontSave;
				
				decalCamera = cameraObj.GetComponent<Camera>();
				decalCamera.orthographic = GetComponent<Camera>().orthographic;
				decalCamera.orthographicSize = GetComponent<Camera>().orthographicSize;
				decalCamera.aspect = GetComponent<Camera>().aspect;
				decalCamera.fieldOfView = GetComponent<Camera> ().fieldOfView;
				decalCamera.backgroundColor = decalBGColor;
				decalCamera.cullingMask = (1 << LayerMask.NameToLayer (layerName));
				decalCamera.targetTexture = decalTexture;
				decalCamera.clearFlags = CameraClearFlags.SolidColor;
			}
			
			decalCamera.backgroundColor = decalBGColor;
			
			decalCamera.transform.position = transform.position;
			decalCamera.transform.rotation = transform.rotation;
			
			decalCamera.Render ();
			
			for (int i = 0; i < decalMaterials.Length; ++i) {
				decalMaterials [i].SetTexture ("_DecalTex", decalTexture);
			}
		}

		public void OnDestroy()
		{
			
			if(decalCamera != null)
			{
				decalCamera.targetTexture = null;
				GameObject.DestroyImmediate( decalCamera.gameObject);
			}

			if(decalTexture != null)
				GameObject.DestroyImmediate(decalTexture);
		}


		public void setDecalLayer(string name)
		{
			layerName = name;
			if(decalCamera != null)
			decalCamera.cullingMask = (1 << LayerMask.NameToLayer (layerName));
		}
	}
}