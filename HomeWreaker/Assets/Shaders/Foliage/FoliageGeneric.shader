// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Foliage/Simple"
{
	Properties
	{
		_Shininess("Shininess", Range( 0.01 , 1)) = 0.1
		_Albedo("Albedo", 2D) = "white" {}
		[Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_OpacityMask("Opacity Mask", 2D) = "white" {}
		_SSSMap("SSS Map", 2D) = "white" {}
		_SpecularIntensity("Specular Intensity", Range( 0 , 1)) = 0
		_SSDisortion("SS Disortion", Range( 0 , 2)) = 0
		_SSSIntensity("SSS Intensity", Range( 0 , 1)) = 0
		_WindMask("Wind Mask", 2D) = "white" {}
		_Perlin("Perlin", 2D) = "black" {}
		_WindSpeed("Wind Speed", Range( 0 , 2)) = 1
		_WindScaleLarge("Wind Scale Large", Float) = 50
		_WindIntensity("Wind Intensity", Range( 0 , 1.5)) = 0.5
		_WindScaleSmall("Wind Scale Small", Float) = 10
		_WindXZMult("Wind XZ Mult", Range( 0 , 1.5)) = 1
		_WindYMult("Wind Y Mult", Range( 0 , 1.5)) = 1
		[Toggle(_USEOPACITYMASK_ON)] _UseOpacityMask("UseOpacityMask", Float) = 1
		_AlbedoTint("Albedo Tint", Color) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _USEOPACITYMASK_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _Perlin;
		uniform float _WindSpeed;
		uniform float _WindScaleLarge;
		uniform float _WindScaleSmall;
		uniform float _WindIntensity;
		uniform float _WindXZMult;
		uniform float _WindYMult;
		uniform sampler2D _WindMask;
		uniform float4 _AlbedoTint;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _OpacityMask;
		uniform float4 _OpacityMask_ST;
		uniform float _SSDisortion;
		uniform sampler2D _SSSMap;
		uniform float4 _SSSMap_ST;
		uniform float _SSSIntensity;
		uniform float _SpecularIntensity;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _Shininess;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float windSpeed26_g12 = _WindSpeed;
			float2 appendResult55_g12 = (float2(windSpeed26_g12 , 0.0));
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult64_g12 = (float2(( windSpeed26_g12 * 1.4 ) , 0.0));
			float4 lerpResult32_g12 = lerp( tex2Dlod( _Perlin, float4( ( ( appendResult55_g12 * _Time.y ) + ( (ase_worldPos).xz / _WindScaleLarge ) ), 0, 0.0) ) , tex2Dlod( _Perlin, float4( ( ( appendResult64_g12 * _Time.y ) + ( (ase_worldPos).xz / _WindScaleSmall ) ), 0, 0.0) ) , 0.5);
			float temp_output_89_0_g12 = _WindXZMult;
			float4 appendResult94_g12 = (float4(temp_output_89_0_g12 , _WindYMult , temp_output_89_0_g12 , 0.0));
			v.vertex.xyz += ( ( ( lerpResult32_g12 * _WindIntensity ) * appendResult94_g12 ) * tex2Dlod( _WindMask, float4( v.texcoord.xy, 0, 0.0) ) ).rgb;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode1 = tex2D( _Albedo, uv_Albedo );
			float2 uv_OpacityMask = i.uv_texcoord * _OpacityMask_ST.xy + _OpacityMask_ST.zw;
			#ifdef _USEOPACITYMASK_ON
				float staticSwitch100 = tex2D( _OpacityMask, uv_OpacityMask ).r;
			#else
				float staticSwitch100 = tex2DNode1.a;
			#endif
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float dotResult4_g16 = dot( ase_worldViewDir , -( ase_worldlightDir + ( ase_worldNormal * _SSDisortion ) ) );
			float dotResult15_g16 = dot( pow( dotResult4_g16 , 1.0 ) , 1.0 );
			float2 uv_SSSMap = i.uv_texcoord * _SSSMap_ST.xy + _SSSMap_ST.zw;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 objToWorldDir40_g16 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 0 ) ).xyz;
			float3 normalizeResult41_g16 = normalize( objToWorldDir40_g16 );
			float4 Albedo25 = ( _AlbedoTint * tex2DNode1 );
			float4 temp_cast_1 = (_SpecularIntensity).xxxx;
			float4 temp_output_43_0_g17 = temp_cast_1;
			float3 normalizeResult4_g18 = normalize( ( ase_worldViewDir + ase_worldlightDir ) );
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 normalizeResult64_g17 = normalize( (WorldNormalVector( i , float4( UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) ) , 0.0 ).rgb )) );
			float dotResult19_g17 = dot( normalizeResult4_g18 , normalizeResult64_g17 );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_40_0_g17 = ( ase_lightColor * ase_lightAtten );
			float dotResult14_g17 = dot( normalizeResult64_g17 , ase_worldlightDir );
			UnityGI gi34_g17 = gi;
			float3 diffNorm34_g17 = normalizeResult64_g17;
			gi34_g17 = UnityGI_Base( data, 1, diffNorm34_g17 );
			float3 indirectDiffuse34_g17 = gi34_g17.indirect.diffuse + diffNorm34_g17 * 0.0001;
			float4 temp_output_42_0_g17 = Albedo25;
			c.rgb = ( ( ( saturate( dotResult15_g16 ) * tex2D( _SSSMap, uv_SSSMap ) * distance( normalizeResult41_g16 , float3( 0,0,0 ) ) ) * ( _SSSIntensity * Albedo25 ) ) + ( ( float4( (temp_output_43_0_g17).rgb , 0.0 ) * (temp_output_43_0_g17).a * pow( max( dotResult19_g17 , 0.0 ) , ( _Shininess * 128.0 ) ) * temp_output_40_0_g17 ) + ( ( ( temp_output_40_0_g17 * max( dotResult14_g17 , 0.0 ) ) + float4( indirectDiffuse34_g17 , 0.0 ) ) * float4( (temp_output_42_0_g17).rgb , 0.0 ) ) ) ).rgb;
			c.a = 1;
			clip( staticSwitch100 - _Cutoff );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode1 = tex2D( _Albedo, uv_Albedo );
			float4 Albedo25 = ( _AlbedoTint * tex2DNode1 );
			o.Albedo = Albedo25.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Standard"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
175;156;1700;724;2704.365;722.0736;2.085266;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;8;-1664,-640;Float;True;Property;_Albedo;Albedo;5;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ColorNode;110;-1280,-896;Float;False;Property;_AlbedoTint;Albedo Tint;23;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1408,-640;Inherit;True;Property;_SimplePlant_01_basecolor;SimplePlant_01_basecolor;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-896,-640;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;102;-1920,640;Float;True;Property;_SSSMap;SSS Map;9;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-512,-640;Float;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;10;-1664,-384;Float;True;Property;_OpacityMask;Opacity Mask;8;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;43;-1664,1152;Float;True;Property;_WindMask;Wind Mask;13;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1664,384;Float;False;Property;_SSSIntensity;SSS Intensity;12;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;82;-1664,1408;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;9;-1664,-128;Float;True;Property;_NormalMap;Normal Map;6;1;[Normal];Create;True;0;0;True;0;None;None;True;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;101;-1664,640;Inherit;True;Property;_TextureSample2;Texture Sample 2;17;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;26;-1920,64;Inherit;False;25;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;103;-1920,512;Inherit;False;25;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1664,128;Float;False;Property;_SpecularIntensity;Specular Intensity;10;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1280,1024;Float;False;Property;_WindSpeed;Wind Speed;16;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1664,192;Float;False;Property;_SSDisortion;SS Disortion;11;0;Create;True;0;0;False;0;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;34;-1424,80;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1280,384;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1280,1408;Float;False;Property;_WindScaleLarge;Wind Scale Large;17;0;Create;True;0;0;False;0;50;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1408,-384;Inherit;True;Property;_SimplePlant_01_mask;SimplePlant_01_mask;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;77;-1280,960;Float;False;Property;_WindIntensity;Wind Intensity;18;0;Create;True;0;0;False;0;0.5;0;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1664,320;Float;False;Constant;_SSSScale;SSS Scale;8;0;Create;True;0;0;False;0;1;0;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1664,256;Float;False;Constant;_SSSPower;SSS Power;8;0;Create;True;0;0;False;0;1;0;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1408,-128;Inherit;True;Property;_SimplePlant_01_normal;SimplePlant_01_normal;3;0;Create;True;0;0;False;0;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;105;-1136,528;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-1280,1664;Float;False;Property;_WindYMult;Wind Y Mult;21;0;Create;True;0;0;False;0;1;1;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;42;-1280,1152;Inherit;True;Property;_TextureSample1;Texture Sample 1;11;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;76;-1280,1472;Float;False;Property;_WindScaleSmall;Wind Scale Small;19;0;Create;True;0;0;False;0;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-1280,1600;Float;False;Property;_WindXZMult;Wind XZ Mult;20;0;Create;True;0;0;False;0;1;1;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;108;-896,128;Inherit;False;SubSurfaceScatering;0;;16;6afed577ed5089a4dbe7482f2151f87d;2,38,0,42,0;8;30;COLOR;1,1,1,0;False;36;COLOR;0,0,0,0;False;37;COLOR;0,0,0,0;False;33;FLOAT;0.5;False;34;FLOAT;1;False;35;FLOAT;1;False;31;COLOR;1,1,1,0;False;32;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;100;-1024,-512;Float;False;Property;_UseOpacityMask;UseOpacityMask;22;0;Create;True;0;0;False;0;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;99;-768,1024;Inherit;False;SimpleWind;14;;12;26683031a94aa994885e417727dd7f3d;0;7;78;FLOAT;0.5;False;4;FLOAT;0.35;False;33;COLOR;1,1,1,0;False;22;FLOAT;50;False;23;FLOAT;10;False;89;FLOAT;1;False;82;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Foliage/Simple;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;Standard;7;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;8;0
WireConnection;111;0;110;0
WireConnection;111;1;1;0
WireConnection;25;0;111;0
WireConnection;101;0;102;0
WireConnection;34;0;26;0
WireConnection;32;0;33;0
WireConnection;32;1;103;0
WireConnection;2;0;10;0
WireConnection;4;0;9;0
WireConnection;105;0;101;0
WireConnection;42;0;43;0
WireConnection;42;1;82;0
WireConnection;108;30;34;0
WireConnection;108;36;4;0
WireConnection;108;37;27;0
WireConnection;108;33;28;0
WireConnection;108;34;29;0
WireConnection;108;35;30;0
WireConnection;108;31;32;0
WireConnection;108;32;105;0
WireConnection;100;1;1;4
WireConnection;100;0;2;1
WireConnection;99;78;77;0
WireConnection;99;4;48;0
WireConnection;99;33;42;0
WireConnection;99;22;75;0
WireConnection;99;23;76;0
WireConnection;99;89;80;0
WireConnection;99;82;85;0
WireConnection;0;0;25;0
WireConnection;0;10;100;0
WireConnection;0;13;108;0
WireConnection;0;11;99;0
ASEEND*/
//CHKSM=EB0AF98C7BCF791683E2D5D1D8D952481DADCB67