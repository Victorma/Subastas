using UnityEngine;
using System.Collections;

public class BidScript : MonoBehaviour {

	public PujadorScript bidParent;
	public float bidCuantity;



	public bool closed;
	public bool timeOut;
	private bool used = false;
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Collider col = GetComponentInChildren<Collider>();

			RaycastHit info = new RaycastHit();
			if(col.Raycast(ray, out info, Camera.main.farClipPlane)){
				if(!used){
					animator.SetBool("Closed",true);
					used = true;
					AuctioningScript auc = Auction.currentAuction.GetComponent<AuctioningScript>();
					auc.TakeBid(this);
					
					foreach(PujadorScript p in GameObject.FindObjectsOfType<PujadorScript>())
						p.TakeBid(this);
				}
			}
		}
		if(closed){
			DestroyImmediate(this.gameObject);
		}else if(timeOut){
			bidParent.PassBid(this);
			DestroyImmediate(this.gameObject);
		}else{
			animator.SetFloat("TimeAlive", animator.GetFloat("TimeAlive")+Time.deltaTime);
		}
	
	}
}
