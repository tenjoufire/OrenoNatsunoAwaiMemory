using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParameterFiddler : MonoBehaviour
{
	[System.Serializable]
    public class MaterialParameter
    {
		public string parameterName = "_Parameter";
        public string displayName = "Parameter Value";
		public int repeat = 2;

		public enum MaterialParamType { number , color , texture , vector }
		public MaterialParamType type = MaterialParamType.number;

		public float minimumValue = 0f;
		public float maximumValue = 1f;

		public Vector4 minimumValue4 = Vector4.zero;
		public Vector4 maximumValue4 = Vector4.one;

		public Texture[] potentialTextures;
	}

	public Material sourceMaterial;
    Material material;
	[Range(0.25f,5f)]
	public float fiddleTime = 3f;

	public List<MaterialParameter> parameters = new List<MaterialParameter>();

	int currentParameterIndex = 0;
	float t = 0f;
	float tf { get { return t / fiddleTime; } }
	bool fiddling;

	float displayValue = 0f;

	void Start ()
	{
		material = new Material( sourceMaterial );
		GetComponent<Renderer>().material = material;
		if ( parameters.Count > 0 )
			StartCoroutine( Unfiddle() );
	}

	void Update ()
	{
		if ( fiddling )
			t += Time.deltaTime;
	}

	public void FiddleParameter()
	{
		if ( currentParameterIndex < parameters.Count )
		{
			material.SetFloat( "_Phase" , 0.5f );

			MaterialParameter param = parameters[ currentParameterIndex ];
			if ( material.HasProperty( param.parameterName ) )
			{
				switch ( param.type )
				{
					case MaterialParameter.MaterialParamType.number: StartCoroutine( FiddleFloat( param ) ); break;
					case MaterialParameter.MaterialParamType.color: StartCoroutine( FiddleColor( param ) ); break;
					case MaterialParameter.MaterialParamType.texture: StartCoroutine( FiddleTexture( param ) ); break;
					case MaterialParameter.MaterialParamType.vector: StartCoroutine( FiddleVector( param ) ); break;
				}
			}
			else
			{
				Debug.Log( "Material Parameter Not Found: " + param.parameterName );
			}
		}
		else
		{
			material.SetFloat( "_Phase" , 1f );
		}
	}

	public IEnumerator FiddleFloat( MaterialParameter param )
	{ 
		float o = material.GetFloat( param.parameterName );
		float f = o;

		float i = 0f;
		while ( i < 1f )
		{
			i += Time.deltaTime * 6f;
			material.SetFloat( param.parameterName , Mathf.Lerp( f , 0 , i ) );
			yield return new WaitForEndOfFrame();
		}

		while ( t < fiddleTime )
		{
			float v = -Mathf.Cos( tf * Mathf.PI * 1.5f * param.repeat ) * 0.5f + 0.5f;
			f = Mathf.Lerp( param.minimumValue , param.maximumValue , v );
			displayValue = f;
			material.SetFloat( param.parameterName , f );
			yield return new WaitForEndOfFrame();
		}

		i = 0f;
		while (i < 1f)
		{
			i += Time.deltaTime * 6f;
			material.SetFloat( param.parameterName , Mathf.Lerp( f , o , i ) );
			yield return new WaitForEndOfFrame();
		}

		currentParameterIndex = currentParameterIndex + 1;
		StartCoroutine( Unfiddle() );
	}

	public IEnumerator FiddleColor( MaterialParameter param )
	{
		Color o = material.GetColor( param.parameterName );
		Color c = o;

		float i = 0f;
		while ( i < 1f )
		{
			i += Time.deltaTime * 6f;
			material.SetColor( param.parameterName , Color.HSVToRGB( 0f , 0.9f , 0.9f ) );
			yield return new WaitForEndOfFrame();
		}

		while ( t < fiddleTime )
		{
			c = Color.HSVToRGB( Mathf.Repeat( tf * param.repeat , 1f ) , 0.9f , 0.9f );
			displayValue = Mathf.Repeat( tf * param.repeat , 1f );
			material.SetColor( param.parameterName , c);
			yield return new WaitForEndOfFrame();
		}

		i = 0f;
		while ( i < 1f )
		{
			i += Time.deltaTime * 6f;
			material.SetColor( param.parameterName , Color.Lerp( c , o , i ) );
			yield return new WaitForEndOfFrame();
		}

		material.SetColor( param.parameterName , o );
		currentParameterIndex = currentParameterIndex + 1;
		StartCoroutine( Unfiddle() );
	}

	public IEnumerator FiddleTexture( MaterialParameter param )
	{
		Texture o = material.GetTexture( param.parameterName );
		while ( t < fiddleTime )
		{
			int id = ( int )Mathf.Floor( tf * param.potentialTextures.Length );
			displayValue = Mathf.Clamp01( (float)id / (float)param.potentialTextures.Length );
			material.SetTexture( param.parameterName , param.potentialTextures[ id ] );
			yield return new WaitForEndOfFrame();
		}
		material.SetTexture( param.parameterName , o );
		currentParameterIndex = currentParameterIndex + 1;
		StartCoroutine( Unfiddle() );
	}

	public IEnumerator FiddleVector( MaterialParameter param )
	{
		Vector4 o = material.GetVector( param.parameterName );
		while ( t < fiddleTime )
		{
			float v = -Mathf.Cos( tf * Mathf.PI * 1.5f * param.repeat ) * 0.5f + 0.5f;
			displayValue = v;
			material.SetVector( param.parameterName , Vector4.Lerp( param.minimumValue4 , param.maximumValue4 , v ) );
			yield return new WaitForEndOfFrame();
		}
		material.SetVector( param.parameterName , o );
		currentParameterIndex = currentParameterIndex + 1;
		StartCoroutine( Unfiddle() );
	}

	IEnumerator Unfiddle ()
	{
		fiddling = false;
		yield return new WaitForSeconds( 1f );
		t = 0f;
		fiddling = true;
		FiddleParameter();
	}

	void OnGUI()
	{
		if ( parameters.Count > 0 && currentParameterIndex < parameters.Count )
		{
			GUIStyle labelStyle = new GUIStyle( GUI.skin.label );
			labelStyle.alignment = TextAnchor.LowerCenter;
			labelStyle.fontStyle = FontStyle.Bold;
			labelStyle.fontSize = 24;
			labelStyle.clipping = TextClipping.Overflow;
			labelStyle.wordWrap = false;


			MaterialParameter p = parameters[ currentParameterIndex ];
			GUILayout.BeginArea( new Rect( 0f , 0f , Screen.width , Screen.height ) );
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label( fiddling ? p.displayName : "" , labelStyle );
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.Space(Screen.width*0.3f);
			labelStyle.fontSize = 14;
			GUILayout.Label( "0.0" , labelStyle ,GUILayout.Width( 24f ) );
			GUILayout.HorizontalSlider( fiddling ? displayValue : 0f , 0f , 1f );
			GUILayout.Label( "1.0" , labelStyle , GUILayout.Width( 24f ) );
			GUILayout.Space( Screen.width*0.3f);
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndArea();
		}
	}
}