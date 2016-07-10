using UnityEngine;
using System.Collections;

// Provides server-side gameplay logic for the player cube.
//
// - Listens to WASD for input and responds by moving the parent entity.
// - Keep the player within the 2D bounds of the world
// - Changes the color of the player and static cubes if they're intersecting
// - FUTURE: Responds to client-side inputs
//
public class ServerPlayer : MonoBehaviour
{
	/// <summary>
	/// The (server-side) static cube to test against for collisions
	/// </summary>
	public Transform StaticCube;

	void Start() { }

	void Update()
	{
		var transform = GetComponent<Transform>();

		// Move if WASD are specified
		const float SPEED = 8.0f;

		Vector3 pos;
		pos.x = transform.position.x + SPEED * Input.GetAxis("ServerHoriz") * Time.deltaTime;
		pos.y = transform.position.y + SPEED * Input.GetAxis("ServerVert") * Time.deltaTime;
		pos.z = transform.position.z;

		// FUTURE: Move if client inputs are specified

		// Keep the player within the bounds of the world
		float w = transform.lossyScale.x * .5f;
		float h = transform.lossyScale.y * .5f;

		if (pos.y + h > 5.0f) pos.y = 5.0f - h;
		if (pos.y - h < 0.0f) pos.y = 0.0f + h;
		if (pos.x + w > 5.0f) pos.x = 5.0f - w;
		if (pos.x - w < -5.0f) pos.x = -5.0f + w;

		transform.position = pos;

		// Change colors to indicate whether the player and static cube are currently colliding
		Vector3 otherpos = StaticCube.GetComponent<Transform>().position;

		float t = pos.y + h; // my top
		float b = pos.y - h; // my bottom
		float l = pos.x - w; // my left
		float r = pos.x + w; // my right

		float ot = otherpos.y + h; // other top
		float ob = otherpos.y - h; // other bottom
		float ol = otherpos.x - w; // other left
		float or = otherpos.x + w; // other right

		Color c;
		if (l < or && r > ol && t > ob && b < ot) {
			c.r = 1.0f;
			c.g = 0.0f;
			c.b = 0.0f;
			c.a = 1.0f;
		}
		else {
			c.r = 0.0f;
			c.g = 0.0f;
			c.b = 1.0f;
			c.a = 1.0f;
		}

		GetComponent<Renderer>().material.color = c;
		StaticCube.GetComponent<Renderer>().material.color = c;
	}
}
