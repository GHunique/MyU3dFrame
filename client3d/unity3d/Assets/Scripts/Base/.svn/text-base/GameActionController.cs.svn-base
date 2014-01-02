using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class GameActionData
{
	public string		name;
	public string		actionTrigger;
	public string		actionName;
	public string		actionLayer;
	public GameObject	effect;
	public Transform	effectBindTransform;
	public float		effectTime;
	public Vector3		effectLocalPos;
	public Vector3		effectLocalAngles;
	public Vector3		effectLocalScale;
	
	public AudioClip	audio;
	
	private int			nameHash;
	
	public int			getNameHash()
	{
		if ( nameHash == 0 )
		{
			nameHash = Animator.StringToHash(  actionLayer + "." + actionName );
		}
		
		return nameHash;
	}
}


public class GameActionController : MonoBehaviour		
{
	public delegate void OnEffectOverDelegate( string name );
	public delegate void OnActionOverDelegate( string name );
	public delegate void OnPlayActionDelegate( string name , int t , float per );


	public OnEffectOverDelegate onEffectOverDelegate = null;
	public OnActionOverDelegate onActionOverDelegate = null;
	public OnPlayActionDelegate onPlayActionDelegate = null;


	public int ActionLayerCount = 1;
	public Dictionary< string , GameActionData > actionDataDic = new Dictionary< string , GameActionData >();
	public GameActionData[] actionDataArray;


	private ArrayList effectArray = new ArrayList();
	private	 ArrayList effectTimeArray = new ArrayList();
	private Dictionary< string , GameObject > effectDic = new Dictionary< string , GameObject >();
	
	private Animator animator;
	
	private string playingAction = "";
	private int playingHash;
	private bool isPlaying;
	
	// Use this for initialization
	void Start () 
	{
		animator = GetComponent< Animator >();
		
		for ( int i = 0; i < actionDataArray.Length ; i++ )
		{
			actionDataDic[ actionDataArray[ i ].name ] = actionDataArray[ i ];
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		#if UNITY_EDITOR
		
		if ( animator == null ) 
		{
			return;
		}
		
		updateEditor();
		#endif
		if ( playingHash != 0 )
		{
			bool stop = true;
			
			for ( int i = 0 ; i < ActionLayerCount ; ++i )
			{
				AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo( i );
				
				if ( state.nameHash == playingHash )
				{
					isPlaying = true;
					stop = false;

					int t = (int)state.normalizedTime;
					onPlayingAction( playingAction , t , state.normalizedTime - t );
				}
				
			}
			
			if ( isPlaying && stop )
			{
				onActionOver( playingAction );
				playingAction = "";
				playingHash = 0;
				isPlaying = false;
			}
		}
		
		
		
		for ( int i = 0; i < effectArray.Count ;  )
		{
			GameObject obj = (GameObject)effectArray[ i ];
			GameActionData data = actionDataDic[ obj.name ];
			
			float time = (float)effectTimeArray[ i ];
			
			if ( time > data.effectTime )
			{
				Destroy( (GameObject)effectArray[ i ] );
				
				effectArray.RemoveAt( i );
				effectTimeArray.RemoveAt( i );
				
				onEffectOver( obj.name );
			}
			else
			{
				effectTimeArray[ i ] = time + GameSetting.GameSpeed * Time.deltaTime;
				++i;
			}
			
		}
		
		if ( animator.speed != GameSetting.GameSpeed ) 
		{
			animator.speed = GameSetting.GameSpeed;
		}
		
		
	}
	
	
	GameObject createEffect( string name )
	{
		if ( !actionDataDic.ContainsKey( name ) ) 
		{
			return null;
		}
		
		GameActionData data = actionDataDic[ name ];
		
		if ( data.effect == null )
		{
			return null;
		}
		
		GameObject obj = (GameObject)Instantiate( data.effect );
		obj.transform.parent = data.effectBindTransform;
		obj.transform.localPosition = data.effectLocalPos;
		obj.transform.localEulerAngles = data.effectLocalAngles;
		obj.transform.localScale = data.effectLocalScale;
		obj.name = name;
		
		return obj;
	}
	
	
	virtual public void onPlayingAction( string name , int t , float per )
	{
		Debug.Log( "action playing " + name + " t" + t + " per" + per );
		
		if ( onPlayActionDelegate != null ) 
		{
			onPlayActionDelegate( name , t , per );
		}
	}
	
	
	virtual public void onEffectOver( string name )
	{
		
		Debug.Log( "effect " + name + " over" );
		
		if ( onEffectOverDelegate != null ) 
		{
			onEffectOverDelegate( name );
		}
	}
	
	virtual public void onActionOver( string name )
	{
		
		Debug.Log( "action " + name + " over" );
		
		if ( onActionOverDelegate != null ) 
		{
			onActionOverDelegate( name );
		}
	}
	
	
	public void clearEffect()
	{
		for ( int i = 0 ; i < effectArray.Count ; ++i )
		{
			Destroy( (GameObject)effectArray[ i ] );
		}
		
		effectArray.Clear();
		effectTimeArray.Clear();
		
		foreach ( KeyValuePair<string, GameObject> a in effectDic )
		{ 
			Destroy( (GameObject)a.Value );
		}
		
		effectDic.Clear();
	}
	
	
	public void addEffectBuff( string name )
	{
		if ( effectDic.ContainsKey( name ) )
		{
			return;
		}
		
		GameObject obj = createEffect( name );
		
		if ( !obj )
		{
			return;
		}
		
		effectDic[ name ] = obj;
	}
	
	
	public void removeEffectBuff( string name )
	{
		GameObject obj = effectDic[ name ];
		
		if ( obj )
		{
			Destroy( obj );
			effectDic.Remove( name );
		}
	}
	
	
	public void playEffect( string name )
	{
		GameObject obj = createEffect( name );
		
		if ( !obj )
		{
			return;
		}
		
		effectArray.Add( obj );
		effectTimeArray.Add( 0.0f );
	}
	
	
	public void playAction( string name )
	{
		if ( !actionDataDic.ContainsKey( name ) ) 
		{
			return;
		}
		
		GameActionData data = actionDataDic[ name ];
		
		playingAction = name;
		playingHash = data.getNameHash();
		isPlaying = false;
		
		animator.SetTrigger( data.actionTrigger );
		
		playEffect( name );
	}
	
	
	#if UNITY_EDITOR
	
	
	void updateEditor()
	{
		if ( editorRefresh ) 
		{
			clearEffect();
			
			animator = GetComponent< Animator >();
			
			actionDataDic.Clear();
			
			for ( int i = 0; i < actionDataArray.Length ; i++ )
			{
				actionDataDic[ actionDataArray[ i ].name ] = actionDataArray[ i ];
			}
			
			editorRefresh = false;
		}
		
		if ( editorPlayOnce )
		{
			clearEffect();
			
			playAction( editorActionTrigger );
			playEffect( editorPlayEffect );
			
			for ( int i = 0; i < editorAddEffectBuff.Length; i++ )
			{
				addEffectBuff( editorAddEffectBuff[ i ] );
			}
			
			editorPlayOnce = false;
		}
		
		if ( editorPlayLoop )
		{
			if ( playingAction.Length > 0 )
			{
				return;
			}
			else
			{
				playAction( editorActionTrigger );
			}
			
			for ( int i = 0 ; i < effectArray.Count ; ++i )
			{
				GameObject obj = (GameObject)effectArray[ i ];
				
				if ( obj.name == editorPlayEffect ) 
				{
					return;
				}
			}
			
			playEffect( editorPlayEffect );
		}
	}
	
	void OnDrawGizmos ()
	{
		Update();
	}
	
	public bool editorRefresh = false;
	public bool editorPlayOnce = false;
	public bool editorPlayLoop = false;
	public string editorActionTrigger;
	public string editorPlayEffect;
	public string[] editorAddEffectBuff;

	#endif
}



