/*
	This is part of the Sinuous Sci-Fi Signs v1.45 package
	Copyright (c) 2014-2017 Thomas Rasor
	E-mail : thomas.ir.rasor@gmail.com

	NOTE : 
	This editor does more than simply set the values on the material it is inspecting, please do not delete this file.
	If you do delete this editor, your materials will not behave correctly through some property changes.
*/

using UnityEngine;
using UnityEditor;
using System.Collections;

using sed = SSFS_Editor_Drawers;
using rbm = UnityEngine.Rendering.BlendMode;

public class SSFS_Editor : MaterialEditor
{
	public enum BlendType
	{
		Additive,
		AlphaBlended,
		SoftAdditive,
		Multiply,
		Invert,
		Solid
	}

	public BlendType blendType = BlendType.Additive;

	#region shellvars

	public Color mainColor, effectColor;
	public Vector2 mainTextureOffset, mainTextureScale;
	public Texture mainTexture, noiseTexture;
	public Vector2 tileCount, scaling, scalingCenter = Vector2.one * 0.5f;
	public Vector2 RotationRadial;
	public float scattering , scanlineIntensity;

	public float phase, sharpness, overbright, aberration, effectAberration, flash;
	public float idleAmount, idleSpeed;
	public bool invertPhase, invertIdle, scaleAroundTile, clipTiles, roundClipping;
	public bool twoSided = true;

	#endregion

	public bool isChangingScalingCenter = false;
	public bool isChangingPhaseRotation = false;
	public bool showImages = false, showHelp = false;

	public int currentTab = 0;

	public Material m;

	#region PropertyBleedingAndPatching

	void ReadProperties ()
	{
		if (!m.shader.isSupported)
			return;

		m.BleedProperty ("_Color", ref mainColor);
		m.BleedProperty ("_Color2", ref effectColor);

		m.BleedProperty ("_MainTex", ref mainTexture, ref mainTextureScale, ref mainTextureOffset);
		m.BleedProperty ("_Noise", ref noiseTexture);

		m.BleedProperty ("_Phase", ref phase);
		m.BleedProperty ("_PhaseSharpness", ref sharpness);
		m.BleedProperty ("_InvertPhase", ref invertPhase);

		m.BleedProperty ("_Radial", ref RotationRadial.y);
		m.BleedProperty ("_PhaseRotation", ref RotationRadial.x);
		m.BleedProperty ("_Overbright", ref overbright);
		m.BleedProperty ("_Scattering", ref scattering);
		m.BleedProperty ("_Aberration", ref aberration);
		m.BleedProperty ("_EffectAberration", ref effectAberration);
		m.BleedProperty ("_FlashAmount", ref flash);

		m.BleedProperty ("_InvertIdle", ref invertIdle);
		m.BleedProperty ("_IdleAmount", ref idleAmount);
		m.BleedProperty ("_IdleSpeed", ref idleSpeed);

		m.BleedProperty ("_ScaleAroundTile", ref scaleAroundTile);
		m.BleedProperty ("_ClippedTiles", ref clipTiles);
		m.BleedProperty ("_RoundClipping", ref roundClipping);
		m.BleedProperty ("_ScanlineIntensity", ref scanlineIntensity);

		m.BleedProperty ("_TileCount", ref tileCount);
		m.BleedProperty ("_Scaling", ref scaling);
		m.BleedProperty ("_ScaleCenter", ref scalingCenter);

		ReadBlendMode ();
	}

	void SetProperties ()
	{
		if (!m.shader.isSupported)
			return;

		m.PatchProperty ("_Color", mainColor);
		m.PatchProperty ("_Color2", effectColor);
		  
		m.PatchProperty ("_MainTex", mainTexture, mainTextureScale, mainTextureOffset);
		m.PatchProperty ("_Noise", noiseTexture);
		  
		m.PatchProperty ("_Phase", phase);
		m.PatchProperty ("_PhaseSharpness", sharpness);
		m.PatchProperty ("_InvertPhase", invertPhase);
		  
		m.PatchProperty ("_Radial", RotationRadial.y);
		m.PatchProperty ("_PhaseRotation", RotationRadial.x);
		m.PatchProperty ("_Overbright", overbright);
		m.PatchProperty ("_Scattering", scattering);
		m.PatchProperty ("_Aberration", aberration);
		m.PatchProperty ("_EffectAberration", effectAberration);
		m.PatchProperty ("_FlashAmount", flash);
		  
		m.PatchProperty ("_InvertIdle", invertIdle);
		m.PatchProperty ("_IdleAmount", idleAmount);
		m.PatchProperty ("_IdleSpeed", idleSpeed);
		  
		m.PatchProperty ("_ScaleAroundTile", scaleAroundTile);
		m.PatchProperty ("_ClippedTiles", clipTiles);
		m.PatchProperty ("_RoundClipping", roundClipping);
		m.PatchProperty ("_ScanlineIntensity", scanlineIntensity);
		  
		m.PatchProperty ("_TileCount", tileCount);
		m.PatchProperty ("_Scaling", scaling);
		m.PatchProperty ("_ScaleCenter", scalingCenter);

		SetBlendMode ();
	}

	void ReadBlendMode ()
	{
		int cm = 99;
		if (m.HasProperty ("_CullMode"))
			cm = m.GetInt ("_CullMode");

		if (cm == 0)
			twoSided = true;
		else
			twoSided = false;

		int bs = 99, bd = 99;
		if (m.HasProperty ("_BlendSrc") && m.HasProperty ("_BlendDest")) {
			bs = m.GetInt ("_BlendSrc");
			bd = m.GetInt ("_BlendDest");

			if (bs == (int)rbm.SrcAlpha && bd == (int)rbm.OneMinusSrcAlpha)
				blendType = BlendType.AlphaBlended;
			else if (bs == (int)rbm.One && bd == (int)rbm.One)
				blendType = BlendType.Additive;
			else if (bs == (int)rbm.OneMinusDstColor && bd == (int)rbm.One)
				blendType = BlendType.SoftAdditive;
			else if (bs == (int)rbm.DstColor && bd == (int)rbm.Zero)
				blendType = BlendType.Multiply;
			else if (bs == (int)rbm.OneMinusDstColor && bd == (int)rbm.OneMinusSrcAlpha)
				blendType = BlendType.Invert;
			else if (bs == (int)rbm.One && bd == (int)rbm.Zero)
				blendType = BlendType.Solid;
			else
				blendType = BlendType.Additive;
		}
	}

	void SetBlendMode ()
	{
		if (m.HasProperty ("_CullMode"))
			m.SetInt ("_CullMode", (twoSided) ? 0 : 2);

		switch (blendType) {
		case BlendType.AlphaBlended:
			m.renderQueue = 3000;
			m.SetOverrideTag ("RenderType", "Transparent");
			m.SetOverrideTag ("IgnoreProjectors", "true");
			if (m.HasProperty ("_DoZWrite"))
				m.SetInt ("_DoZWrite", 0);
			if (m.HasProperty ("_BlendSrc"))
				m.SetInt ("_BlendSrc", (int)rbm.SrcAlpha);
			if (m.HasProperty ("_BlendDest"))
				m.SetInt ("_BlendDest", (int)rbm.OneMinusSrcAlpha);
			break;
		case BlendType.Additive:
			m.renderQueue = 3000;
			m.SetOverrideTag ("RenderType", "Transparent");
			m.SetOverrideTag ("IgnoreProjectors", "true");
			if (m.HasProperty ("_DoZWrite"))
				m.SetInt ("_DoZWrite", 0);
			if (m.HasProperty ("_BlendSrc"))
				m.SetInt ("_BlendSrc", (int)rbm.One);
			if (m.HasProperty ("_BlendDest"))
				m.SetInt ("_BlendDest", (int)rbm.One);
			break;
		case BlendType.SoftAdditive:
			m.renderQueue = 3000;
			m.SetOverrideTag ("RenderType", "Transparent");
			m.SetOverrideTag ("IgnoreProjectors", "true");
			if (m.HasProperty ("_DoZWrite"))
				m.SetInt ("_DoZWrite", 0);
			if (m.HasProperty ("_BlendSrc"))
				m.SetInt ("_BlendSrc", (int)rbm.OneMinusDstColor);
			if (m.HasProperty ("_BlendDest"))
				m.SetInt ("_BlendDest", (int)rbm.One);
			break;
		case BlendType.Multiply:
			m.renderQueue = 3000;
			m.SetOverrideTag ("RenderType", "Transparent");
			m.SetOverrideTag ("IgnoreProjectors", "true");
			if (m.HasProperty ("_DoZWrite"))
				m.SetInt ("_DoZWrite", 0);
			if (m.HasProperty ("_BlendSrc"))
				m.SetInt ("_BlendSrc", (int)rbm.DstColor);
			if (m.HasProperty ("_BlendDest"))
				m.SetInt ("_BlendDest", (int)rbm.Zero);
			break;
		case BlendType.Invert:
			m.renderQueue = 3000;
			m.SetOverrideTag ("RenderType", "Transparent");
			m.SetOverrideTag ("IgnoreProjectors", "true");
			if (m.HasProperty ("_DoZWrite"))
				m.SetInt ("_DoZWrite", 0);
			if (m.HasProperty ("_BlendSrc"))
				m.SetInt ("_BlendSrc", (int)rbm.OneMinusDstColor);
			if (m.HasProperty ("_BlendDest"))
				m.SetInt ("_BlendDest", (int)rbm.OneMinusSrcAlpha);
			break;
		case BlendType.Solid:
			m.renderQueue = 2000;
			m.SetOverrideTag ("RenderType", "Opaque");
			m.SetOverrideTag ("IgnoreProjectors", "false");
			if (m.HasProperty ("_DoZWrite"))
				m.SetInt ("_DoZWrite", 1);
			if (m.HasProperty ("_BlendSrc"))
				m.SetInt ("_BlendSrc", (int)rbm.One);
			if (m.HasProperty ("_BlendDest"))
				m.SetInt ("_BlendDest", (int)rbm.Zero);
			break;
		}
	}

	#endregion

	override public void OnInspectorGUI ()
	{
		if (!isVisible)
			return;

		m = target as Material;

		EditorGUI.BeginChangeCheck ();

		ReadProperties ();

		Texture2D box = Resources.Load ("Editor_Box_Transparent", typeof(Texture2D)) as Texture2D;
		Texture2D box_active = Resources.Load ("Editor_BoxActive_Transparent", typeof(Texture2D)) as Texture2D;
		Texture2D box_selected = Resources.Load ("Editor_BoxThick_Transparent", typeof(Texture2D)) as Texture2D;

		GUIStyle offtabstyle = new GUIStyle (GUI.skin.button);
		offtabstyle.padding = new RectOffset (4, 4, 8, 8);
		offtabstyle.margin = new RectOffset (0, 0, 0, 0);
		offtabstyle.normal.background = box;
		offtabstyle.active.background = box_active;
		GUIStyle ontabstyle = new GUIStyle (offtabstyle);
		ontabstyle.normal.background = box_selected;

		sed.showHelp = EditorGUILayout.ToggleLeft (" In-Context Help", sed.showHelp);
		m.DrawField ("_Phase", ref phase, "Phase");

		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("General Options", currentTab == 0 ? ontabstyle : offtabstyle))
			currentTab = 0;
		if (GUILayout.Button ("Transition Details", currentTab == 1 ? ontabstyle : offtabstyle))
			currentTab = 1;
		if (GUILayout.Button ("Effect Options", currentTab == 2 ? ontabstyle : offtabstyle))
			currentTab = 2;
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Space ();

		GUIStyle optionstyle = new GUIStyle (GUI.skin.box);
		optionstyle.normal.background = box;
		optionstyle.margin = new RectOffset (0, 0, 0, 0);
		optionstyle.padding = new RectOffset (16, 16, 8, 8);
		EditorGUILayout.BeginVertical (optionstyle);
		if (currentTab == 0)
			DrawGeneralTab ();
		if (currentTab == 1)
			DrawTransitionTab ();
		if (currentTab == 2)
			DrawEffectsTab ();
		EditorGUILayout.EndVertical ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		SetProperties ();

		if (EditorGUI.EndChangeCheck ())
			EditorUtility.SetDirty (m);
	}

	#region OptionTabs

	public void DrawGeneralTab ()
	{
		m.DrawField ("_CullMode", ref twoSided, "Two Sided");

		if (m.HasProperty ("_BlendSrc") && m.HasProperty ("_BlendDest")) {
			sed.DrawHelp ("_BlendSrc");
			blendType = (BlendType)EditorGUILayout.EnumPopup ("Blending Mode", blendType);
		}

		m.DrawField ("_Color", ref mainColor, "Global Tint");

		showImages = EditorGUI.Foldout (EditorGUILayout.GetControlRect (), showImages, "Textures", true);
		if (showImages) {
			
			m.DrawField ("_MainTex", ref mainTexture, ref mainTextureScale, ref mainTextureOffset, "Image Texture");
			m.DrawField ("_Noise", ref noiseTexture, "Scatter Texture");
		}

		m.DrawField ("_TileCount", ref tileCount, "Tile Count");
		m.DrawField ("_Scaling", ref scaling, "Scaling");
		m.DrawField ("_ScaleAroundTile", ref scaleAroundTile, "Scale Around Tiles");
		m.DrawField ("_IdleAmount", ref idleAmount, "Idle Amount");
		m.DrawField ("_IdleSpeed", ref idleSpeed, "Idle Speed");
	}

	public void DrawTransitionTab ()
	{
		m.DrawField ("_Color2", ref effectColor, "Transition Tint");
		m.DrawField ("_PhaseSharpness", ref sharpness, "Phase Sharpness");
		m.DrawField ("_InvertPhase", ref invertPhase, "Reverse Direction");
		m.DrawField ("_InvertIdle", ref invertIdle, "Reverse Idle Direction");
		m.DrawField ("_Scattering", ref scattering, "Scatter Distance");
		m.DrawField ("_FlashAmount", ref flash, "Flash Amount");
		m.DrawField ("_EffectAberration", ref effectAberration, "Flash Color Separation");

		EditorGUILayout.Space ();
		sed.DrawHelp ("_Radial");
		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label ("Hold Shift To Ignore Snapping On Grids.", EditorStyles.boldLabel);
		GUILayout.FlexibleSpace ();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (m.HasProperty ("_Radial") && m.HasProperty ("_PhaseRotation"))
			RotationRadial = sed.RotationField (RotationRadial, true, ref isChangingPhaseRotation, "Start Location", 100f, 16f, Color.cyan);
		if (m.HasProperty ("_ScaleCenter")) {
			GUILayout.FlexibleSpace ();
			scalingCenter = sed.GridVector2Field (scalingCenter, ref isChangingScalingCenter, "Scaling Center", 100f, 16f, Color.cyan);
		}
		GUILayout.FlexibleSpace ();
		if (isChangingPhaseRotation || isChangingScalingCenter)
			Repaint ();
		EditorGUILayout.EndHorizontal ();
	}

	public void DrawEffectsTab ()
	{
		m.DrawField ("_Overbright", ref overbright, "Overbright");
		m.DrawField ("_Aberration", ref aberration, "Fresnel Color Separation");
		m.DrawField ("_ClippedTiles", ref clipTiles, "Clip Tiles");
		m.DrawField ("_RoundClipping", ref roundClipping, "Round Clipping");
		m.DrawField ("_ScanlineIntensity", ref scanlineIntensity, "Scanline Intensity");
	}

	#endregion
}