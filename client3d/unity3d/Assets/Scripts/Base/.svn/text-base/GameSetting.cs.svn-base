using System;
using UnityEngine;

public class GameSetting : MonoBehaviour 
{
	enum GameResType
	{
		RESPHONE3,
		RESPHONE4,
		RESPHONE5,
		RESPAD2,
		RESPAD3,
	};
	
	enum GameMultiple
	{
		GMX1 = 1,
		GMX2 = 2,
		GMX3 = 4,
	}

	struct GameResStruct
	{
		public Vector2 sizeInPixel;
		public Vector2 sizeDesign;
		public float scale;
		public int type;
		public string dir;

		public GameResStruct( Vector2 p , Vector2 d , float s , int t , string dd )
		{
			sizeInPixel = p;
			sizeDesign = d;
			scale = s;
			type = t;
			dir = dd;
		}
	}

	static bool pResPortrait;

	static GameResStruct pResPhone = new GameResStruct( new Vector2(320,480), new Vector2(320,480), 1.0f, (int)GameResType.RESPHONE3 , "pPhone" );
	static GameResStruct pResPhoneRetina35 = new GameResStruct( new Vector2(640 ,960), new Vector2(320,480), 2.0f, (int)GameResType.RESPHONE4 , "pPhone" );
	static GameResStruct pResPhoneRetina40 = new GameResStruct( new Vector2(640,1136), new Vector2(320,568), 2.0f, (int)GameResType.RESPHONE5 , "pPhone" );
	static GameResStruct pResTable = new GameResStruct( new Vector2(768,1024), new Vector2(768,1024), 1.0f, (int)GameResType.RESPAD2 , "pPad" );
	static GameResStruct pResTableRetina = new GameResStruct( new Vector2(1536,2048), new Vector2(768,1024), 2.0f, (int)GameResType.RESPAD3 , "pPadHD" );

	static GameResStruct lResPhone = new GameResStruct( new Vector2(480,320), new Vector2(480,320), 1.0f, (int)GameResType.RESPHONE3 , "lPhone" );
	static GameResStruct lResPhoneRetina35 = new GameResStruct( new Vector2(960,640), new Vector2(480,320), 2.0f, (int)GameResType.RESPHONE4 , "lPhone" );
	static GameResStruct lResPhoneRetina40 = new GameResStruct( new Vector2(1136,640), new Vector2(568,320), 2.0f, (int)GameResType.RESPHONE5 , "lPhone" );
	static GameResStruct lResTable = new GameResStruct( new Vector2(1024,768), new Vector2(1024,768), 1.0f, (int)GameResType.RESPAD2 , "lPad" );
	static GameResStruct lResTableRetina = new GameResStruct( new Vector2(2048,1536), new Vector2(1024,768), 2.0f, (int)GameResType.RESPAD3 , "lPadHD" );


	static GameResStruct activeRes;

	public void Awake()
	{
		initGameSetting();
	}

	static bool inited = false;
	
	public static void initGameSetting()
	{
		if ( !inited )
		{
			pResPortrait = Screen.height > Screen.width;

			if ( pResPortrait ) 
			{
				int actualHeight = Screen.height;

				if ( actualHeight > pResPhoneRetina40.sizeInPixel.y )
				{
					activeRes = pResTableRetina;
				}
				else if ( actualHeight > pResTable.sizeInPixel.y )
				{
					activeRes = pResPhoneRetina40;
				}
				else if ( actualHeight > pResPhoneRetina35.sizeInPixel.y )
				{
					activeRes = pResTable;
				}
				else if ( actualHeight > pResPhone.sizeInPixel.y )
				{
					activeRes = pResPhoneRetina35;
				}
				else
				{
					activeRes = pResPhone;
				}

//				float f = Screen.height / Screen.width;
//				float f1 = pResPhoneRetina40.sizeDesign.y / pResPhoneRetina40.sizeDesign.x;
//				float f2 = pResPhoneRetina35.sizeDesign.x / pResPhoneRetina35.sizeDesign.y;
//				float f3 = pResTable.sizeDesign.y / pResTable.sizeDesign.x;
//				
//				float f11 = f1 - f > 0 ? f1 - f : f - f1;
//				float f22 = f2 - f > 0 ? f2 - f : f - f2;
//				
//				if ( f22 > f11 )
//				{
//					activeRes = pResPhoneRetina40;
//				}
//				else
//				{
//					activeRes = pResPhoneRetina35;
//				}
			}
			else
			{
				int actualWidth = Screen.width;

				if ( actualWidth > lResPhoneRetina40.sizeInPixel.x )
				{
					activeRes = lResTableRetina;
				}
				else if ( actualWidth > lResTable.sizeInPixel.x )
				{
					activeRes = lResPhoneRetina40;
				}
				else if ( actualWidth > lResPhoneRetina35.sizeInPixel.x )
				{
					activeRes = lResTable;
				}
				else if ( actualWidth > lResPhone.sizeInPixel.x )
				{
					activeRes = lResPhoneRetina35;
				}
				else
				{
					activeRes = lResPhone;
				}
			}




			UIPath = "prefabs/" + activeRes.dir + "/UI/";
			SpritePath = "prefabs/" + activeRes.dir + "/Sprite/";

			Debug.Log( "Screen: " + Screen.width + " " + Screen.height );

			//Debug.Log( "Res: " + activeRes.type );
			Debug.Log( "UIPath: " + UIPath );
			Debug.Log( "SpritePath: " + SpritePath );

			inited = true;
		}
	}

	public const int INVALID_ID = -1;

	public static string	UIPath;
	public static string	SpritePath;
	public static string	ConfigPath = "/Config/";
	public static float		GameSpeed = 1.0f;

}

