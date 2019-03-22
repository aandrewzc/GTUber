using UnityEngine;

public class BuildingCollider : MonoBehaviour {

    private GameObject textBox;

	private void Start () 
    {
        textBox = GameObject.Find("/Canvas/TextBox");
	}
	
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Uber")
        {
            textBox.GetComponent<GameText>().DisplayMessage("You crashed into a building!");
            textBox.GetComponent<GameText>().Score(-4.5f);
        }
    }
}
