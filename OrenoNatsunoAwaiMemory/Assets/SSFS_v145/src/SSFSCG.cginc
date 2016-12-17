//	This is part of the Sinuous Sci-Fi Signs v1.45 package
//	Copyright (c) 2014-2017 Thomas Rasor
//	E-mail : thomas.ir.rasor@gmail.com
//
//	NOTE:
//	Do not delete this file.
//	This file contains the majority of the functionality for the SSFS shaders.
//	If you do delete this file, the SSFS shaders will no longer work and will fail to compile.

#ifndef SSFSCGinc
#define SSFSCGinc
#endif

//This looks like a comment... but it's not!
//Don't remove this, removal will prevent correct compilation for use with older Unity versions
//UNITY_SHADER_NO_UPGRADE

#if UNITY_VERSION < 540
#define o2w _Object2World
#else
#define o2w unity_ObjectToWorld
#endif

#include "UnityCG.cginc"

struct vdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
#ifndef lite
	float4 color : COLOR;
	float3 normal : NORMAL;
#endif
};

struct fdata
{
	float4 vertex : SV_POSITION;
	float2 uv : TEXCOORD0;
#ifndef lite
	float4 color : COLOR;
	float3 normal : TEXCOORD1;
	float2 texuv : TEXCOORD2;
	float4 scrpos : TEXCOORD3;
	float3 worldPos : TEXCOORD4;
	float3 worldNormal : TEXCOORD5;
#endif
};

#ifndef lite
int _CullMode,_BlendSrc,_BlendDest;
fixed4 _Color2;
fixed _InvertPhase, _InvertIdle;
fixed _Overbright, _Aberration, _EffectAberration, _ClippedTiles, _RoundClipping;
fixed _ScanlineIntensity;
#endif

#ifndef ultralite
fixed _IdleAmount, _IdleSpeed , _Radial;
#endif

fixed4 _Color;
sampler2D _MainTex , _Noise;
float4 _MainTex_ST;
float4 _TileCount, _Scaling, _ScaleCenter;
fixed _Phase , _PhaseRotation ,_Scattering , _PhaseSharpness;
fixed _ScaleAroundTile;
fixed _FlashAmount;

#ifndef lite
#define doScanlines (_ScanlineIntensity>0.02)
#endif

fdata vert(vdata v)
{
	fdata o;
	o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.uv;
#ifndef lite
	o.color = v.color;
	o.normal = v.normal;
	o.worldNormal = abs(normalize(mul(o2w, float4(v.normal, 0.0)).xyz));
	o.texuv = TRANSFORM_TEX(v.uv, _MainTex);
	o.worldPos = mul(o2w, v.vertex).xyz;
	if (doScanlines)
		o.scrpos = ComputeScreenPos(o.vertex);
#endif
	return o;
}

float noisetex (float2 uv)
{
	#ifndef lite
	return Luminance(tex2D(_Noise,uv).rgb);
	#endif
	return tex2D(_Noise,uv).r;
}

float2 rotuv(float2 uv, float r , float2 center = 0.5)
{
	r = r * 6.28318;
	float sinr = sin(r);
	float cosr = cos(r);
	float2x2 rm = float2x2(cosr, sinr, -sinr, cosr);
	return mul(uv - center, rm) + center;
}

#ifndef lite
half CalcFresnel( float3 worldNormal, float3 viewDirection )
{
	return 1.0 - max( 0.0 , abs(-dot( worldNormal , viewDirection) ));
}

half CalcScanlines(fdata i)
{
	if (!doScanlines) return 1.0;

	float y = (i.scrpos.xy / i.scrpos.w).y * _ScreenParams.y;
	y = i.worldPos.y * 200.0;
	y *= 0.2;
	y += _Time.y;
	return (1.0-_ScanlineIntensity) + _ScanlineIntensity * saturate( 1.0-pow(abs(frac(y)* 2.0 - 1.0),2.0) );
}

half2 FlattenVector(half3 vec, half3 norm)
{
	return (vec - dot(vec, norm) * norm).xy;
}

half2 CalcAberrationVector(half3 localSpaceViewDir, half3 localSpaceNormal)
{
	return FlattenVector(localSpaceViewDir, localSpaceNormal);
}

half4 tex2DWithAberration(sampler2D tex, float2 uv , half2 aberrationVector , fixed effect)
{
	half4 res = 0.0;
	half4 col = lerp(_Color, _Color2, effect);
	float2 uvr = uv.xy + aberrationVector;
	float2 uvg = uv.xy;
	float2 uvb = uv.xy - aberrationVector;
	half4 r = (col * tex2D(tex, uvr)) * half4(1.0, 0.0, 0.0, 1.0);
	half4 g = (col * tex2D(tex, uvg)) * half4(0.0, 1.0, 0.0, 1.0);
	half4 b = (col * tex2D(tex, uvb)) * half4(0.0, 0.0, 1.0, 1.0);
	res = r+g+b;
	res.a = 0.3333 * res.a;
	return res;
}

half4 tex2DWithAbberation(sampler2D tex, half2 uv, half3 localSpaceViewDir, half3 localSpaceNormal , fixed effect)
{
	half2 aberrationVector = CalcAberrationVector(localSpaceViewDir, localSpaceNormal);
	return tex2DWithAberration(tex, uv, aberrationVector , effect);
}
#endif

half CalcTransition( half2 uv , half scatter )
{
	fixed p = _Phase;
	half phasePos = saturate(0.7*rotuv(uv, _PhaseRotation , 0.5) + 0.15).x;
#ifndef ultralite
	half radialPos = 1.0 - saturate(0.7 * (2.0 * length(uv - 0.5)) + 0.15);
	phasePos = lerp(phasePos, radialPos, _Radial);
#endif
	phasePos = 1.0 - phasePos;
	phasePos += scatter * 0.25 * _Scattering;
	phasePos = saturate(phasePos);
#ifndef lite
	if (_InvertPhase > 0.5) phasePos = 1.0 - phasePos;
#endif
	half n = _PhaseSharpness * 40.0 * (phasePos.x - p) + p;
	half transition = 1.0 - (n - (2.0*p - 1.0));
	return saturate(transition);
}

#ifndef ultralite
half CalcIdle( half2 uv , half scatter )
{
	float t = _Time.y * _IdleSpeed * 3.0;
	fixed p = (3.0 * frac ( t ) - 1.0);
	fixed cleanup = 1.0 - (0.5 + 0.5 * cos(6.28318 * t));
	cleanup = 1.0 - pow( 1.0 - cleanup , 16.0 );
	
	half phasePos = saturate(0.7*rotuv(uv, _PhaseRotation,0.5) + 0.15).x;
	half radialPos = 1.0 - saturate(0.7 * (2.0 * length(uv - 0.5)) + 0.15);
	phasePos = lerp(phasePos, radialPos, _Radial);
	phasePos = 1.0 - phasePos;
	phasePos += scatter * 0.25 * _Scattering;
	phasePos = saturate(phasePos);

#ifndef lite
	if (_InvertPhase > 0.5) phasePos = 1.0 - phasePos;
	if (_InvertIdle > 0.5) phasePos = 1.0 - phasePos;
#endif

	half sharpness = _PhaseSharpness * 20.0;
	half n = sharpness * abs(phasePos.x - p) + p;
	half transition = 1.0 - (n - (2.0*p - 1.0));
	return saturate(transition) * cleanup * _IdleAmount;
}
#endif

half2 CalcScaledUV( half2 texUv , half2 tileCenter , half tileWidth , half effect )
{
	half2 tl = tileCenter - tileWidth;
	half2 br = tileCenter + tileWidth;
	half2 tileCenterPoint = 0.0;
	tileCenterPoint.x = lerp(tl.x,br.x,_ScaleCenter.x);
	tileCenterPoint.y = lerp(tl.y,br.y,_ScaleCenter.y);

	half2 scalingCenter = lerp( saturate(_ScaleCenter.xy) , tileCenterPoint, _ScaleAroundTile);
	half2 posScaling = 4.0 * (-1.0 / (1.0 + effect * _Scaling.xy) + 1.0) + 1.0;
	half2 negscaling = 1.0 / (1.0 - effect * _Scaling.xy);
	half2 resScaling = effect * _Scaling.xy + 1.0;
	return (texUv - scalingCenter) * resScaling + scalingCenter;
}
#ifndef lite
fixed CalcTileClipping(half2 tileWidth , half2 scaledUv , half2 tileCenter)
{
	fixed scaleClip = 1.0;
	if (_ClippedTiles)
	{
		tileWidth = tileWidth;
		fixed2 clipFadeDist = 1.0 + tileWidth * 1.0;
		if (_RoundClipping)
			scaleClip = smoothstep(clipFadeDist.x, 1.0, 0.7*length(scaledUv - tileCenter) / max(tileWidth.x, tileWidth.y));
		else
		{
			fixed2 xyclip = abs(scaledUv - tileCenter) / tileWidth;
			fixed xclip = smoothstep(clipFadeDist.x, 1.0, xyclip.x);
			fixed yclip = smoothstep(clipFadeDist.y, 1.0, xyclip.y);
			scaleClip = xclip * yclip;
		}
	}
	return scaleClip;
}
#endif