//	This is part of the Sinuous Sci-Fi Signs v1.45 package
//	Copyright (c) 2014-2017 Thomas Rasor
//	E-mail : thomas.ir.rasor@gmail.com
//
//	NOTE:
//	This is the base shader for the SSFS package.
//	This shader utilizes all functionality that the package offers.

Shader "Sci-Fi/SSFS [Full]"
{
	Properties
	{
		[HideInInspector]_BlendSrc("" , int) = 1
		[HideInInspector]_BlendDest("" , int) = 1
		[HideInInspector]_CullMode("" , int) = 2
		[HideInInspector]_DoZWrite("" , int) = 0
		_Color("Tint" , Color) = (1.0,1.0,1.0,1.0)
		_Color2 ("Transition Tint" , Color) = (1.0,1.0,1.0,1.0)
		_Overbright("Overbright",Range(0.0,1.0))=0.25
		_MainTex("Image", 2D) = "white" {}
		[NoScaleOffset]_Noise ("Noise", 2D) = "gray" {}
		_TileCount("Tile Count" , Vector) = (15.0,15.0,0.0,0.0)
		_Phase( "Phase" , Range( 0.0 , 1.0 ) ) = 1.0
		[Toggle]_InvertPhase("Invert Direction" , Float) = 0.0
		[Toggle]_InvertIdle("Invert Idle Direction" , Float) = 0.0
		_IdleAmount( "Idle Amount" , Range( 0.0 , 1.0 ) ) = 0.5
		_IdleSpeed( "Idle Speed" , Range( 0.0 , 1.0 ) ) = 0.1
		_PhaseRotation("Phase Rotation" , Range(0.0,1.0)) = 0.0
		_Radial("Radial Phase" , Range(0.0,1.0)) = 0.0
		_PhaseSharpness("Phase Sharpness" , Range(0.0,1.0)) = 0.5
		_Scattering("Scatter Amount" , Float) = 0.5
		_Scaling("Scaling",Vector) = (1.0,1.0,0.0,0.0)
		_ScaleCenter("ScaleCenter",Vector) = (1.0,1.0,0.0,0.0)
		_Aberration("Aberration Amount" , Range(0.0,1.0)) = 0.5
		_EffectAberration("Transition Aberration" , Range(0.0,1.0)) = 0.5
		_FlashAmount("Transition Flash" , Range(0.0,1.0)) = 0.5
		_ScanlineIntensity("Scanline Intensity" , Range(0.0,1.0)) = 0.5
		[Toggle]_ScaleAroundTile("Scale Around Tile" , Float) = 1.0
		[Toggle]_ClippedTiles("Clip Tiles" , Float) = 1.0
		[Toggle]_RoundClipping("Circle Clip Shape" , Float) = 0.0
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "PreviewType" = "Plane" }
		Blend [_BlendSrc] [_BlendDest]
		Cull [_CullMode]
		ZWrite [_DoZWrite]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#ifndef SSFSCGinc
			#include "SSFSCG.cginc"
			#endif

			#ifdef SSFSCGinc
			half4 frag (fdata i) : COLOR
			{
				half4 col = 0.0;//empty result
				float3 worldViewDirection = normalize(i.worldPos.xyz - _WorldSpaceCameraPos.xyz);//the direction from the camera to this pixel
				half3 localViewDirection = mul(o2w, float4(worldViewDirection, 0.0)).xyz;//needed to project onto the uv for aberration
				half fres = CalcFresnel(i.worldNormal, worldViewDirection);//calculated fresnel term for sheer view angle effects
				fixed scanlines = CalcScanlines(i);// screen space scan lines

				//clean up the edges if we are using certain blendmodes
				half2 edgedist = 2.0 * abs(i.uv.xy - 0.5);
				fixed edges = max(edgedist.x,edgedist.y);
				edges = smoothstep(1.0,0.85,edges);
				if (_BlendDest == 0)
					edges = 1.0;
				
				half2 tileWidth = 0.5 / _TileCount.xy;//half width ( in uv space ) of each tile
				half2 tileUv = floor(i.uv * _TileCount.xy) / _TileCount.xy;//bottom left corner of the current pixel's tile
				half2 tileCenter = tileUv + tileWidth;//center of the current pixel's tile
				
				half noise = noisetex(tileCenter);
				half2 tileScatter = noise * 2.0 - 1.0;//difference per-tile in transition phase

				half transition = CalcTransition( tileCenter , tileScatter );//what part of the transition animation this tile is on
				half idle = CalcIdle( tileCenter , tileScatter);//what part of the idle animation this tile is on
				half effect = saturate( pow( 1.0 - transition , 3.0 ) );//how much this tile is affected by only transition effects
				half effectall = saturate( effect + idle );//how much this tile is affected by transition+idle effects
				
				half2 scaledUv = CalcScaledUV( i.uv.xy, tileCenter, tileWidth , effectall );//the uv of this tile, scaled based on the effect

				fixed scaleClip = CalcTileClipping(tileWidth , scaledUv , tileCenter);//whether or not this pixel is still part of the tile after scaling

				fixed2 abOffset = CalcAberrationVector(localViewDirection, i.normal) * ( fres * _Aberration + effectall * 2.0 * _EffectAberration) * 0.05;//the uv offset with aberration applied
				fixed visibility = 1.0 - saturate(pow( 1.0 - transition , 16.0));//transparency result based on the transition
				col = tex2DWithAberration(_MainTex, scaledUv * _MainTex_ST.xy + _MainTex_ST.zw, abOffset , 2.0 * effectall) * (1.0 + effectall * 30.0 * _FlashAmount ) * visibility * scaleClip;//color result of image after all effects are applied
				col.a = saturate(col.a - fres * 0.5);//fade the image at sheer angles
				col.a *= scanlines;//multiply by the scanlines
				col.rgb *= (1.0+_Overbright*4.0+fres*0.25);//at overbrightening and sheer viewing angle fresnel
				col.rgb *= col.a;
				return edges * col;//final result
			}
			#endif
			ENDCG
		}
	}
	Fallback "Sci-Fi/SSFS [Lite]"
	CustomEditor "SSFS_Editor"
}