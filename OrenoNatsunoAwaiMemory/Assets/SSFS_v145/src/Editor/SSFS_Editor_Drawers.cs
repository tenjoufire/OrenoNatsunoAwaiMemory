/*
	This is part of the Sinuous Sci-Fi Signs v1.45 package
	Copyright (c) 2014-2017 Thomas Rasor
	E-mail : thomas.ir.rasor@gmail.com

	NOTE : 
	Do not delete this file, as it carries several functionalities vital to the SSFS_Editor.cs script.
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class SSFS_Editor_Drawers
{
	public static bool showHelp = false;

	public static void MaterialWithST (string title, ref Texture tex, ref Vector2 tiling, ref Vector2 offset)
	{
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.LabelField (title);

		EditorGUILayout.BeginHorizontal ();

		EditorGUILayout.BeginVertical (GUILayout.MaxHeight (72f));
		GUILayout.FlexibleSpace ();
		EditorGUIUtility.labelWidth = 50f;
		tiling = EditorGUILayout.Vector2Field ("Tiling:", tiling);
		offset = EditorGUILayout.Vector2Field ("Offset:", offset);
		EditorGUIUtility.labelWidth = 0f;
		GUILayout.FlexibleSpace ();
		EditorGUILayout.EndVertical ();
		
		tex = (Texture)EditorGUILayout.ObjectField ("", tex, typeof(Texture), true, GUILayout.MaxWidth (64f));

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}

	public static Vector2 GridVector2Field (Vector2 value, ref bool inUse, string title, float size, float knobSize = 16f, Color knobUseColor = default(Color))
	{
		GUILayout.BeginVertical ();
		GUILayout.Space (20f);
		Texture2D knob = Resources.Load ("Editor_Knob", typeof(Texture2D)) as Texture2D;
		Texture2D grid = Resources.Load ("Editor_Grid", typeof(Texture2D)) as Texture2D;
		if (knob == null)
			Debug.LogWarning ("Editor_Knob texture not found!");
		if (grid == null)
			Debug.LogWarning ("Editor_Grid texture not found!");
		
		Event e = Event.current;
		Vector2 msp = GUIUtility.GUIToScreenPoint (e.mousePosition);
		
		GUIStyle gs = new GUIStyle (GUI.skin.box);
		gs.normal.background = grid;
		gs.contentOffset = new Vector2 (0f, -20f);
		gs.fontStyle = FontStyle.Bold;
		gs.clipping = TextClipping.Overflow;
		gs.wordWrap = false;
		GUILayout.Box (title, gs, GUILayout.Width (size), GUILayout.Height (size));//make the grid
		
		Rect gr = GUILayoutUtility.GetLastRect ();//calculate the screen rect of the grid
		Vector2 gtls = GUIUtility.GUIToScreenPoint (gr.position);
		Vector2 gbrs = GUIUtility.GUIToScreenPoint (gr.position + gr.size);
		Rect gsr = new Rect (gtls, size * Vector2.one);
		
		if (gsr.Contains (msp) && e.rawType == EventType.mouseDown)//if the mouse clicked on the grid, start using this widget
			inUse = true;
		if (e.rawType == EventType.mouseUp)//if the mouse is released anywhere, stop using this widget
			inUse = false;
		
		GUI.color = inUse ? knobUseColor : Color.white;//color the knob if we're using this widget
		
		Vector2 mgp = V2Inverp (gtls, gbrs, msp);//percent across the grid that the mouse is

		if (!e.shift)
			mgp = snapVector2ToGrid (mgp, 4, 0.35f);
		
		if (inUse && e.rawType == EventType.Repaint)
			value = mgp;
		
		Vector2 ksp = V2Lerp (gtls, gbrs, value);//knob screen pixel position
		Vector2 kgp = GUIUtility.ScreenToGUIPoint (ksp);//the gui pixel position of the knob
		
		Rect kr = new Rect (kgp.x - knobSize * 0.5f, kgp.y - knobSize * 0.5f, knobSize, knobSize);
		GUI.DrawTexture (kr, knob);
		GUI.color = Color.white;
		
		GUILayout.EndVertical ();
		return value;
	}

	public static Vector2 RotationField (Vector2 value, bool useDistance, ref bool inUse, string title, float size, float knobSize = 16f, Color knobUseColor = default(Color))
	{
		GUILayout.BeginVertical ();
		GUILayout.Space (20f);
		Texture2D knob = Resources.Load ("Editor_Knob", typeof(Texture2D)) as Texture2D;
		Texture2D grid = Resources.Load ("Editor_RadialGrid", typeof(Texture2D)) as Texture2D;
		if (knob == null)
			Debug.LogWarning ("Editor_Knob texture not found!");
		if (grid == null)
			Debug.LogWarning ("Editor_RadialGrid texture not found!");
		
		Event e = Event.current;
		Vector2 msp = GUIUtility.GUIToScreenPoint (e.mousePosition);
		
		GUIStyle gs = new GUIStyle (GUI.skin.box);
		gs.normal.background = grid;
		gs.contentOffset = new Vector2 (0f, -20f);
		gs.fontStyle = FontStyle.Bold;
		gs.clipping = TextClipping.Overflow;
		gs.wordWrap = false;
		GUILayout.Box (title, gs, GUILayout.Width (size), GUILayout.Height (size));//make the grid
		
		Rect gr = GUILayoutUtility.GetLastRect ();//calculate the screen rect of the grid
		Vector2 gtls = GUIUtility.GUIToScreenPoint (gr.position);
		Rect gsr = new Rect (gtls, size * Vector2.one);
		
		if (gsr.Contains (msp) && e.rawType == EventType.mouseDown)//if the mouse clicked on the grid, start using this widget
			inUse = true;
		if (e.rawType == EventType.mouseUp)//if the mouse is released anywhere, stop using this widget
			inUse = false;
		
		
		Vector2 diff = gsr.center - msp;
		float middist = diff.magnitude / (size * 0.5f);

		float angle = Mathf.Atan2 (diff.y, diff.x);

		if (inUse && e.rawType == EventType.Repaint) {
			value.x = angle * 0.15915494309f + 0.5f;//convert from radians to percentage
			if (useDistance)
				value.y = Mathf.Clamp01 (1f - middist);
			
			if (!e.shift) {
				value.x = snapValueToGrid (value.x, 8, 0.35f);
				if (useDistance)
					value.y = snapValueToGrid (value.y, 2, 0.35f);
			}
		}

		float sinr = Mathf.Sin (value.x * Mathf.PI * 2f);
		float cosr = Mathf.Cos (value.x * Mathf.PI * 2f);
		Vector2 dir = new Vector2 (cosr, sinr);
		Vector2 ksp = gsr.center + (useDistance ? 1f - value.y : 1f) * dir * (size * 0.5f - knobSize * 0.25f);//knob screen pixel position
		Vector2 kgp = GUIUtility.ScreenToGUIPoint (ksp);//the gui pixel position of the knob
		
		Rect kr = new Rect (kgp.x - knobSize * 0.5f, kgp.y - knobSize * 0.5f, knobSize, knobSize);
		GUI.color = inUse ? knobUseColor : Color.white;//color the knob if we're using this widget

		GUI.DrawTexture (kr, knob);
		GUI.color = Color.white;

		GUILayout.EndVertical ();
		
		return value;
	}

	public static Vector2 V2Lerp (Vector2 a, Vector2 b, Vector2 t) //separate component lerping for a Vector2 based on a Vector2 interpolant
	{
		Vector2 v = Vector2.zero;
		v.x = Mathf.Lerp (a.x, b.x, t.x);
		v.y = Mathf.Lerp (a.y, b.y, t.y);
		return v;
	}

	public static Vector2 V2Inverp (Vector2 a, Vector2 b, Vector2 t) //separate component inverse lerping for a Vector2 based on a Vector2 interpolant
	{
		Vector2 v = Vector2.zero;
		v.x = Mathf.InverseLerp (a.x, b.x, t.x);
		v.y = Mathf.InverseLerp (a.y, b.y, t.y);
		return v;
	}

	public static Vector2 snapVector2ToGrid (Vector2 input, int sections = 4, float weight = 0.25f)
	{
		input.x = snapValueToGrid (input.x, sections, weight);
		input.y = snapValueToGrid (input.y, sections, weight);
		return input;
	}

	public static float snapValueToGrid (float input, int sections = 4, float weight = 0.25f)
	{
		float sectionWidth = 1f / sections;
		float w = sectionWidth * weight;

		for (float i = 0f; i <= 1f; i += sectionWidth) {
			if (Mathf.Abs (input - i) < w)
				input = i;
		}

		return Mathf.Clamp01 (input);
	}

	public static void DrawHelp (string propertyName)
	{
		if (showHelp && propertyName.Length > 0 && HelpText.ContainsKey (propertyName) && HelpText [propertyName].Length > 0) {
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox (HelpText [propertyName], MessageType.Info);
		}
	}

	#region Property Bleeding

	public static void BleedProperty (this Material m, string n, ref float v)
	{
		if (m.HasProperty (n))
			v = m.GetFloat (n);
	}

	public static void BleedProperty (this Material m, string n, ref bool v)
	{
		if (m.HasProperty (n))
			v = m.GetFloat (n) > 0.5f;
	}

	public static void BleedProperty (this Material m, string n, ref Texture t)
	{
		if (m.HasProperty (n))
			t = m.GetTexture (n);
	}

	public static void BleedProperty (this Material m, string n, ref Texture t, ref Vector2 s, ref Vector2 o)
	{
		if (m.HasProperty (n)) {
			t = m.GetTexture (n);
			s = m.GetTextureScale (n);
			o = m.GetTextureOffset (n);
		}
	}

	public static void BleedProperty (this Material m, string n, ref Color v)
	{
		if (m.HasProperty (n))
			v = m.GetColor (n);
	}

	public static void BleedProperty (this Material m, string n, ref Vector2 v)
	{
		if (m.HasProperty (n)) {
			Vector4 v4 = m.GetVector (n);
			v = (Vector2)v4;
		}
	}

	#endregion

	#region Property Patching

	public static void PatchProperty (this Material m, string n, float v)
	{
		if (m.HasProperty (n))
			m.SetFloat (n, v);
	}

	public static void PatchProperty (this Material m, string n, bool v)
	{
		if (m.HasProperty (n))
			m.SetFloat (n, v ? 1f : 0f);
	}

	public static void PatchProperty (this Material m, string n, Texture t)
	{
		if (m.HasProperty (n))
			m.SetTexture (n, t);
	}

	public static void PatchProperty (this Material m, string n, Texture t, Vector2 s, Vector2 o)
	{
		if (m.HasProperty (n)) {
			m.SetTexture (n, t);
			m.SetTextureScale (n, s);
			m.SetTextureOffset (n, o);
		}
	}

	public static void PatchProperty (this Material m, string n, Color v)
	{
		if (m.HasProperty (n))
			m.SetColor (n, v);
	}

	public static void PatchProperty (this Material m, string n, Vector2 v)
	{
		if (m.HasProperty (n))
			m.SetVector (n, (Vector4)v);
	}

	#endregion

	#region Property Drawers

	public static void DrawField (this Material m, string n, ref bool v, string title)
	{
		if (m.HasProperty (n)) {
			DrawHelp (n);
			v = EditorGUILayout.Toggle (title, v);
		}
	}

	public static void DrawField (this Material m, string n, ref float v, string title)
	{
		if (m.HasProperty (n)) {
			DrawHelp (n);
			v = EditorGUILayout.Slider (title, v, 0f, 1f);
		}
	}

	public static void DrawField (this Material m, string n, ref Color v, string title)
	{
		if (m.HasProperty (n)) {
			DrawHelp (n);
			v = EditorGUILayout.ColorField (title, v);
		}
	}

	public static void DrawField (this Material m, string n, ref Vector2 v, string title)
	{
		if (m.HasProperty (n)) {
			DrawHelp (n);
			v = EditorGUILayout.Vector2Field (title, v);
		}
	}

	public static void DrawField (this Material m, string n, ref Texture t, ref Vector2 s, ref Vector2 o, string title)
	{
		if (m.HasProperty (n)) {
			DrawHelp (n);
			MaterialWithST ("Main Texture", ref t, ref s, ref o);
		}
	}

	public static void DrawField (this Material m, string n, ref Texture v, string title)
	{
		if (m.HasProperty (n)) {
			DrawHelp (n);
			v = (Texture)EditorGUILayout.ObjectField (title, v, typeof(Texture), true);
		}
	}

	#endregion

	#region Property Help Dialogues

	public static Dictionary<string,string> HelpText = new Dictionary<string,string> () {
		{ "_Phase","" },

		{ "_CullMode","Whether or not to cull object back faces." },
		{ "_BlendSrc","The way this material blends with things drawn behind it." },
		{ "_Color","The overall color tint of this material." },
		{ "_MainTex","" },
		{ "_Noise","The texture that affects tile scattering. Experiment with this, as it can have a wide range of different effects on how your material looks." },
		{ "_TileCount","How many tiles there are on each axis. Generally these values should be integers." },
		{ "_Scaling","How the tiles individually scale during the effect. " +
			"Positive values shrink the tile, and negative values ( down to -1.0 ) will increase the tile image size. " +
			"Values beyond -1.0 scale the tile smaller, and flip it along that axis." },
		{ "_ScaleAroundTile","Whether the scaling should be done around tiles locally or around the entire UV." },
		{ "_IdleAmount","How much effect the idle animation has proportional to the transition effect." },
		{ "_IdleSpeed","How quickly the idle animation plays and repeats." },

		{ "_Color2","The color tint of tiles during their effect." },
		{ "_PhaseSharpness","How quickly individual tiles complete their effect animation." },
		{ "_InvertPhase","" },
		{ "_InvertIdle","" },
		{ "_Scattering","How much tiles' individual phases are offset by the scatter texture. (General Options)" },
		{ "_FlashAmount","Extra overbrightening added when tiles undergo the transition or idle effect. Works with the transition tint." },
		{ "_EffectAberration","Distance of color separation specific to tiles undergoing the animation effect." },

		{ "_Overbright","Extra brightness added to the base image. This will very heavily depending on the image used." },
		{ "_Aberration","Color separation seen at sheer viewing angles. This is separate from the Effect Color Separation ( Transition Details)." },
		{ "_ClippedTiles","Whether or not to maintain tile content despite image scaling." },
		{ "_RoundClipping","Option to clip tiles using circles instead of the normal square tile." },

		{ "_Radial","Start Location : Where the phase animation starts. Scaling Center : Where tiles scale towards or away from. When tiles scale around tiles ( General Options ), this is local to the tile's individual space." },

	};

	#endregion
}
