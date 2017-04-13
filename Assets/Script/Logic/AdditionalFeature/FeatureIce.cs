using UnityEngine;
using System.Collections;

public class FeatureIce : MonoBehaviour
{
	public BoxCollider2D col;

	void Start()
	{
		GameObject obj = gameObject;
		CharacterGroup cg = obj.GetComponent<CharacterGroup>();
		if(col == null)
			col = obj.GetComponent<BoxCollider2D>();
				
		if(col == null)
			col = obj.AddComponent<BoxCollider2D>();
		col.size = new Vector2(cg.textWidth, cg.analyse.lineHeight);
		col.offset = new Vector2((0.5f-cg.pivot.x)*cg.textWidth, (0.5f-cg.pivot.y)*cg.analyse.lineHeight);
		col.isTrigger = true;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		GameObject obj = other.gameObject;
		switch(obj.tag)
		{
			case Config.TAG_PLAYER:
				PhysicalPlayerController p = obj.GetComponent<PhysicalPlayerController>();
				p.onIce();
				break;
		}
	}

	public void OnTriggerExit2D(Collider2D coll)
	{
		GameObject obj = coll.gameObject;
		switch(obj.tag)
		{
			case Config.TAG_PLAYER:
				PhysicalPlayerController p = obj.GetComponent<PhysicalPlayerController>();
				p.leaveIce();
				break;
		}
	}
}
