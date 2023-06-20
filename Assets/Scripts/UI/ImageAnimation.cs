using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
	public Sprite[] sprites;
	public Sprite[] switchSprites;
	public int spritePerFrame = 6;
	public bool loop = true;
	public bool pause = false;
	public bool destroyOnEnd = false;
	public bool switchOnEnd = false;

	private int index = 0;
	private Image image;
	private Sprite[] holdSprites;
	private int frame = 0;

	private bool switched;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	private void Update()
	{
		if (!pause && sprites != null && sprites.Length != 0)
		{
			if (!loop && index == sprites.Length) return;
			frame++;
			if (frame < spritePerFrame) return;
			image.sprite = sprites[index];
			frame = 0;
			index++;
			if (index >= sprites.Length)
			{
				if (loop) index = 0;
				if (destroyOnEnd) Destroy(gameObject);
				if (switchOnEnd) Switch();
			}
		}
	}

	private void Switch()
	{
		if (!switched)
		{
			holdSprites = sprites;
			sprites = switchSprites;
			spritePerFrame = 6;
			switched = true;
		}
		else
		{
			spritePerFrame = 15;
			sprites = holdSprites;
			switched = false;
			switchOnEnd = false;
		}
	}
}
