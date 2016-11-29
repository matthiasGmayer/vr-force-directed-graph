using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{
	public static Graph instance;
	public float repulsion;
	public float attraction;
	public float springLength;
	public float damping;

	public List<Node> nodes = new List<Node> ();
	public List<Edge> edges = new List<Edge> ();


	public void Awake ()
	{
		instance = this;

//		DataStore data_store = JsonUtility.FromJson<DataStore> (Resources.Load ("miserables"));
//		foreach (string node in data_store.nodes) {
//			Node new_node = Node.CreateFromJSON (node);
//		}
//		foreach (string edge in data_store.edges) {
//			Edge new_edge = Edge.CreateFromJSON (edge);
//		}
	}

}

public class DataStore
{
	public string[] nodes;
	public string[] edges;
}
