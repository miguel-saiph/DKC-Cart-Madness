using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PixelPerfect : MonoBehaviour {

	public enum AutoScaleMode {

		Floor,
		Ceil,
		Round
	}

	[SerializeField] private float pixelsPerUnit = 100.0f;
	[SerializeField] private float zoom = 1f;
	[SerializeField] private AutoScaleMode scaleMode;

	public float PixelsPerUnit {
		get { return pixelsPerUnit * zoom; }
	}

	public float PixelSize {
		get { return 1.0f / pixelsPerUnit / zoom; }

	}

	public float OrtographicSize {
		get {
			return Camera.main.pixelHeight / PixelsPerUnit / 2.0f;
		}
	}

	[ContextMenu("Execute")]

	public void Execute() {
		Camera.main.orthographicSize = OrtographicSize;
	}

	private Vector3 WorldPositionToPixel(Vector3 worldPosition, float pixelSize) {

		switch (scaleMode) {
		case AutoScaleMode.Floor:
			return new Vector3 (
				Mathf.Floor (worldPosition.x / pixelSize) * pixelSize,
				Mathf.Floor (worldPosition.y / pixelSize) * pixelSize,
				Mathf.Floor (worldPosition.z / pixelSize) * pixelSize);
		case AutoScaleMode.Ceil:
			return new Vector3 (
				Mathf.Ceil (worldPosition.x / pixelSize) * pixelSize,
				Mathf.Ceil (worldPosition.y / pixelSize) * pixelSize,
				Mathf.Ceil (worldPosition.z / pixelSize) * pixelSize);
		case AutoScaleMode.Round:
			return new Vector3 (
				Mathf.Round (worldPosition.x / pixelSize) * pixelSize,
				Mathf.Round (worldPosition.y / pixelSize) * pixelSize,
				Mathf.Round (worldPosition.z / pixelSize) * pixelSize);
		default:
			throw new UnityException ("ArgumentOutOfRangeException");
		}
	}

	public Vector3 WorldPositionToScreenPixel (Vector3 worldPosition) {
		return WorldPositionToPixel (worldPosition, PixelSize);
	}

	public Vector3 WorldPositionToSpritePixel(Vector3 worldPosition) {
		var spritePixelSize = PixelSize * zoom;
		return WorldPositionToPixel (worldPosition, spritePixelSize);
	}
}
