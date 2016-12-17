//	This is part of the Sinuous Sci-Fi Signs v1.45 package
//	Copyright (c) 2014-2017 Thomas Rasor
//	E-mail : thomas.ir.rasor@gmail.com
//
//	NOTE:
//	This is the reduced version of the base shader for the SSFS package.
//	This shader utilizes some of the functionality that the package offers to favor performance over fidelity.


Shader "Sci-Fi/SSFS [Lite]"
{
	Properties
	{
		_Color("Tint" , Color) = (1.0,1.0,1.0,1.0)
		_MainTex("Image", 2D) = "white" {}
		[NoScaleOffset]_Noise ("Noise", 2D) = "gray" {}
		_TileCount("Tile Count" , Vector) = (15.0,15.0,0.0,0.0)
		_Phase( "Phase" , Range( 0.0 , 1.0 ) ) = 1.0
		_IdleAmount( "Idle Amount" , Range( 0.0 , 1.0 ) ) = 0.5
		_IdleSpeed( "Idle Speed" , Range( 0.0 , 1.0 ) ) = 0.1
		_PhaseRotation("Phase Rotation" , Range(0.0,1.0)) = 0.0
		_Radial("Radial Phase" , Range(0.0,1.0)) = 0.0
		_PhaseSharpness("Phase Sharpness" , Range(0.0,1.0)) = 0.5
		_Scattering("Scatter Amount" , Float) = 0.5
		_Scaling("Scaling",Vector) = (1.0,1.0,0.0,0.0)
		_ScaleCenter("ScaleCenter",Vector) = (1.0,1.0,0.0,0.0)
		_FlashAmount("Transition Flash" , Range(0.0,1.0)) = 0.5
		[Toggle]_ScaleAroundTile("Scale Around Tile" , Float) = 1.0
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "PreviewType"="Plane" }
		Blend One One
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#define lite
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
				
				half2 tileWidth = 0.5 / _TileCount.xy;//half width ( in uv space ) of each tile
				half2 tileUv = floor(i.uv * _TileCount.xy) / _TileCount.xy;//bottom left corner of the current pixel's tile
				half2 tileCenter = tileUv + tileWidth;//center of the current pixel's tile
				
				half noise = noisetex(tileCenter);
				half2 tileScatter = noise * 2.0 - 1.0;//difference per-tile in transition phase

				half transition = CalcTransition( tileCenter , tileScatter );//what part of the transition animation this tile is on
				half idle = CalcIdle( tileCenter , tileScatter);//what part of the idle animation this tile is on
				half effect = pow( 1.0 - transition , 3.0 );//how much this tile is affected by only transition effects
				half effectall = min( 1.0 , effect + idle );//how much this tile is affected by transition+idle effects
				
				half2 scaledUv = CalcScaledUV( i.uv.xy, tileCenter, tileWidth , effectall );//the uv of this tile, scaled based on the effect

				fixed visibility = 1.0 - saturate(pow( 1.0 - transition , 16.0));//transparency result based on the transition
				col = tex2D(_MainTex, scaledUv * _MainTex_ST.xy + _MainTex_ST.zw) * (1.0 + effectall * 30.0 * _FlashAmount ) * visibility;//color result of image after all effects are applied
				return col;//final result
			}
			#endif
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "SSFS_Editor"
}