using UnityEngine;
using System.Collections;

public class Graph : MonoBehaviour
{

	public TextAsset miserables;
	public float repulsion;
	public float attraction;
	public float springLength;
	public float damping;


	public void Awake(){

		DataStore data_store = JsonUtility.FromJson<DataStore>(Resources.Load("miserables"));
		foreach (String node in data_store.nodes) {
			Node new_node = Node.CreateFromJSON (node);
		}
		foreach (String edge in data_store.edges) {
			Edge new_edge = Edge.CreateFromJSON (edge);
		}
	}

}

public class DataStore 
{
	public string[] nodes;
	public string[] edges;
}
