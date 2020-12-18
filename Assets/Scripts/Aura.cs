using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{

	/*-------------------------------------------------------------------------------------------------------*/
	// DECLARING THE VARIABLES
	/*-------------------------------------------------------------------------------------------------------*/

	// debuff values
	public float drownThreshold = 2f;
	public float unparalyzeRate = 1f;

	// colors
	public Color red = new Color(1f, 0f, 0f, 0.5f);
	public Color blue = new Color(0f, 0f, 1f, 0.5f);
	public Color green = new Color(0f, 1f, 0f, 0.5f);
	public Color white = new Color(1f, 1f, 1f, 0.5f);
	public Color centre = new Color(0f, 1f, 1f, 0.5f);
	private Color[] colors;

	/*-------------------------------------------------------------------------------------------------------*/
	// INITIALIZING THE AURA
	/*-------------------------------------------------------------------------------------------------------*/

	void Awake()
	{
		colors = new Color[5];
		colors[0] = red;
		colors[1] = blue;
		colors[2] = green;
		colors[3] = white;
		colors[4] = centre;
	}


	/*-------------------------------------------------------------------------------------------------------*/
	// ARRANGING THE COLORS ON THE GRID
	/*-------------------------------------------------------------------------------------------------------*/

	public Color GetAuraColor(GameObject pieceObject)
	{
		string faction = pieceObject.GetComponent<Piece>().faction;
		if (faction.Equals("Dragon")) { return red; }
		else if (faction.Equals("Kraken")) { return green; }
		else if (faction.Equals("Night")) { return blue; }
		else if (faction.Equals("Angel")) { return white; }
		else if (faction.Equals("Centre")) { return centre; }
		return new Color(0f, 0f, 0f, 0f);
	}

	// gets the places the aura goes (finding the actual squares using these vectors is done in squares)
	public Vector2[] GetAuraVectors(GameObject pieceObject)
	{
		string faction = pieceObject.GetComponent<Piece>().faction;
		Vector2[] auraVectors = new Vector2[4];
		if (faction.Equals("Dragon"))
		{
			auraVectors[0] = new Vector2(1, 0);
			auraVectors[1] = new Vector2(0, 1);
			auraVectors[2] = new Vector2(-1, 0);
			auraVectors[3] = new Vector2(0, -1);
		}
		else if (faction.Equals("Kraken"))
		{
			auraVectors[0] = new Vector2(1, 1);
			auraVectors[1] = new Vector2(-1, 1);
			auraVectors[2] = new Vector2(-1, -1);
			auraVectors[3] = new Vector2(1, -1);
		}
		if (faction.Equals("Angel"))
		{
			auraVectors[0] = new Vector2(1, 2);
			auraVectors[1] = new Vector2(2, 1);
			auraVectors[2] = new Vector2(-2, 1);
			auraVectors[3] = new Vector2(1, -2);
		}
		else if (faction.Equals("Night"))
		{
			auraVectors[0] = new Vector2(-1, 2);
			auraVectors[1] = new Vector2(2, -1);
			auraVectors[2] = new Vector2(2, 1);
			auraVectors[3] = new Vector2(1, 2);
		}
		else if (faction.Equals("Centre"))
		{
			auraVectors = new Vector2[8];
			auraVectors[0] = new Vector2(1, 1);
			auraVectors[1] = new Vector2(-1, 1);
			auraVectors[2] = new Vector2(1, 0);
			auraVectors[3] = new Vector2(0, 1);
			auraVectors[4] = new Vector2(-1, -1);
			auraVectors[5] = new Vector2(1, -1);
			auraVectors[6] = new Vector2(-1, 0);
			auraVectors[7] = new Vector2(0, -1);
		}
		return auraVectors;
	}

	// puts the color on the squares
	public void AuraLayer(GameObject originSquare, List<GameObject> squares, List<float> distances)
	{
		GameObject pieceObject = originSquare.GetComponent<Square>().pieceObject;
		Color color = GetAuraColor(pieceObject);
		int numSquares = squares.Count;
		for (int i = 0; i < numSquares; i++)
        {
			SpriteRenderer spriteRenderer = squares[i].GetComponent<SpriteRenderer>();
			Color currentColor = spriteRenderer.color;
			Color newColor;
			// just accounting for the case where we're on the same square
			if (distances[i] != 0) { newColor = color / distances[i]; }
			else { newColor = color; }
			
			Debug.Log("current color opacity: " + currentColor.a.ToString() + "new color opacity: " + newColor.a.ToString());
			if (currentColor.a < newColor.a)
			{
				print("changing square color");
				spriteRenderer.color = newColor;
			}
			else if (currentColor.a == newColor.a)
			{
				float newR = (currentColor.r + newColor.r) / 2;
				float newG = (currentColor.g + newColor.g) / 2;
				float newB = (currentColor.b + newColor.b) / 2;
				float newA = (currentColor.a + newColor.a) / 2;
				spriteRenderer.color = new Color(newR, newG, newB, newA);
			}
		}
	}

	/*-------------------------------------------------------------------------------------------------------*/
	// APPLIES THE AURA TO THE PIECE
	/*-------------------------------------------------------------------------------------------------------*/

	public void ApplyAura(GameObject squareObject)
	{
		AuraBurn(squareObject);
		AuraDrown(squareObject);
		AuraFear(squareObject);
		AuraArmy(squareObject);
	}

	// get the opacity of a specific color on the square
	public float GetAuraValue(Color colorBase, GameObject squareObject)
	{
		// get the color of the square
		Color squareColor = squareObject.GetComponent<SpriteRenderer>().color;
		if (TheSameColor(squareColor, colorBase))
		{
			// output the opacity of the square
			return squareColor.a;
		}
		return 0f;
	}

	public bool TheSameColor(Color color0, Color color1)
	{
		if (color0.r == color1.r && color0.g == color1.g && color0.b == color1.b)
		{
			return true;
		}
		return false;
	}


	public void AuraBurn(GameObject squareObject)
	{
		Square square = squareObject.GetComponent<Square>();
		Piece piece = square.pieceObject.GetComponent<Piece>();
		Player player = piece.playerObject.GetComponent<Player>();
		if (!player.isTurn)
		{
			piece.burnDamage = GetAuraValue(red, squareObject);
			piece.damageTaken = piece.damageTaken + piece.burnDamage;
			piece.UpdateSprite();
			if (piece.health <= 0)
			{
				square.RemovePiece();
			}
		}
	}

	public void AuraDrown(GameObject squareObject)
	{
		Square square = squareObject.GetComponent<Square>();
		Piece piece = square.pieceObject.GetComponent<Piece>();
		Player player = piece.playerObject.GetComponent<Player>();
		if (!player.isTurn)
		{
			piece.drownThreshold = drownThreshold;
			float drownTick = GetAuraValue(green, squareObject);
			if (drownTick == 0)
			{
				piece.drownTicker = 0;
			}
			piece.drownTicker = piece.drownTicker + drownTick;
			if (piece.drownTicker > drownThreshold)
			{
				square.RemovePiece();
			}
		}
	}

	public void AuraArmy(GameObject squareObject)
	{
		Square square = squareObject.GetComponent<Square>();
		Piece piece = square.pieceObject.GetComponent<Piece>();
		Player player = piece.playerObject.GetComponent<Player>();
		if (player.isTurn)
		{
			piece.armyCount = GetAuraValue(white, squareObject);
			piece.baseHealth = piece.baseHealth + piece.armyCount;
			piece.UpdateSprite();
		}
	}

	public void AuraFear(GameObject squareObject)
	{
		Square square = squareObject.GetComponent<Square>();
		Piece piece = square.pieceObject.GetComponent<Piece>();
		Player player = piece.playerObject.GetComponent<Player>();
		if (!player.isTurn)
		{
			float fearCount = GetAuraValue(blue, squareObject);
			if (piece.paralyzed)
			{
				piece.paralyzeTicker = piece.paralyzeTicker - unparalyzeRate;
				if (piece.paralyzeTicker <= 0)
				{
					piece.paralyzed = false;
				}
				piece.UpdateSprite();
			}

			else if (!piece.paralyzed)
			{
				if (fearCount != 0)
				{
					piece.paralyzeTicker = fearCount;
					piece.paralyzed = true;
				}
				piece.UpdateSprite();
			}
		}
	}
}