using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Node : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Renderer rend;
	public Graph graph;
	public Text namefield;

	public bool ready = false;
	public bool hidden;
	public bool hideconnections;
	public bool dontRepel;
	public bool toBeDestroyed;

	public List<Node> repulsionlist = new List<Node> ();
	public List<Edge> attractionlist = new List<Edge> ();
	public Vector3 velocity;

	#region showandhide

	public void Hide ()
	{
		hidden = true;
		rend.enabled = false;
		GetComponent<SphereCollider> ().enabled = false;

		CanvasGroup cv = GetComponent<CanvasGroup> ();
		cv.alpha = 0;
		cv.blocksRaycasts = false;
		cv.interactable = false;
	}

	public void Show ()
	{
		hidden = false;
		rend.enabled = true;

		GetComponent<SphereCollider> ().enabled = true;

		CanvasGroup cv = GetComponent<CanvasGroup> ();
		cv.alpha = 1;
		cv.blocksRaycasts = true;
		cv.interactable = true;
		namefield.transform.localPosition = Vector3.zero;			
	}

	public void HideConnections ()
	{
		hideconnections = true;
	}

	public void ShowConnections ()
	{
		hideconnections = false;
	}

	public void Remove ()
	{
		dontRepel = true;
		toBeDestroyed = true;
		HideConnections ();
		Invoke ("Destroy", 1.5f);
	}

	protected virtual void Destroy () // IS INVOKED
	{
		Destroy (gameObject);
	}

	#endregion

	#region events


	public virtual void OnPointerEnter (PointerEventData eventData)
	{

	}

	public virtual void OnPointerExit (PointerEventData eventData)
	{
		
	}

	public virtual void OnPointerClick (PointerEventData eventData)
	{
		
	}

	public virtual void OnBeginDrag (PointerEventData eventData)
	{
	}

	public virtual void OnDrag (PointerEventData eventData)
	{
		//transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
	}

	public virtual void OnEndDrag (PointerEventData eventData)
	{
		
	}

	#endregion events


	#region physics

	protected Vector3 CalcAttractionToCenter (float force)
	{
		Vector3 dir = -transform.parent.transform.position * force;
		Vector3 returnvector = dir * force;
		return returnvector;
	}

	protected Vector3 CalcAttraction (Node otherNode, float weight)
	{
		if (otherNode) {
			Vector3 a = transform.position;
			Vector3 b = otherNode.transform.position;
			return (b - a).normalized * (graph.attraction + weight) * (Vector3.Distance (a, b) / graph.springLength);
		} else
			return Vector3.zero;
	}

	protected Vector3 CalcRepulsion (Node otherNode)
	{
		if (otherNode) {
			
			// Coulomb's Law: F = k(Qq/r^2)
			float distance = Vector3.Distance (transform.position, otherNode.transform.position);
			Vector3 returnvector = ((transform.position - otherNode.transform.position).normalized * graph.repulsion) / (distance * distance);

			if (!float.IsNaN (returnvector.x) && !float.IsNaN (returnvector.y) && !float.IsNaN (returnvector.z))
				return returnvector;
			else
				return Vector3.zero;
		} else
			return Vector3.zero;
	}

	#endregion physics


	public void Reset ()
	{
		transform.position = new Vector3 (Random.value, Random.value, Random.value) * 0.01f;
	}

	public void Awake ()
	{
		rend = GetComponent<Renderer> ();
	}

	public void Start ()
	{
		Reset ();
		RefreshLists ();
	}

	public void RefreshLists ()
	{
		repulsionlist.Clear ();
		GameObject[] allnodes = GameObject.FindGameObjectsWithTag ("Node");
		foreach (GameObject go in allnodes) {
			if (go != gameObject)
				repulsionlist.Add (go.GetComponent<Node> ());
		}
		ready = true;
	}

	public void Update ()
	{		
		if (ready) {
			velocity = Vector3.zero;

			// REPULSION
			foreach (Node rn in repulsionlist) {
				velocity += CalcRepulsion (rn);
			}

			//ATTRACTION
			foreach (Edge e in attractionlist)
				velocity += CalcAttraction (e.Other (this), e.weight);

			//ATTRACTION TO CENTER
			velocity += CalcAttractionToCenter (graph.attraction);


			//APPLY FORCES
			if (!float.IsNaN (velocity.x) && !float.IsNaN (velocity.y) && !float.IsNaN (velocity.z)) {
				transform.position += velocity * graph.damping * Time.deltaTime;
				//Debug.Log (name + " " + velocity.ToString ());
			} else
				Debug.LogError (name + " " + velocity.ToString ());

		}
	}

}
