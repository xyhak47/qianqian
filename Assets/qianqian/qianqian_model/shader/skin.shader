Shader "Custom/Skin"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_NormalTex("NormalTex",2D)="white"{}
		_SSSLUT("SSSLUT",2D)="white"{}
		_ThicknessTex("ThicknessTex",2D)="white"{}
		_CurveFactor("CurveFactor",Range(0,1))=1
		_Smoothness("Smoothness",Range(0,1))=0.5
		_FrontSurfaceDistortion("FrontSurfaceDistortion",float) = 1
		_BackSurfaceDistortion("BackSurfaceDistortion",float) = 1
		[HDR]_InteriorColor("InteriorColor",Color) = (1,1,1,1)
		_InteriorColorPower("InteriorColorPower",float) = 1
		_FrontSSSIntensity("FrontSSSIntensity",float) = 1
		_BackSSSIntensity("BackSSSIntensity",float)=1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "UnityStandardBRDF.cginc"
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
				float4 T2W0:TEXCOORD1;
				float4 T2W1:TEXCOORD2;
				float4 T2W2:TEXCOORD3;
				float3 worldOriNormal:TEXCOORD4;
				float3 frontDir:TEXCOORD5;
				float3 clipFrontDir:TEXOORD6;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _NormalTex;
			float4 _NormalTex_ST;
			float _CurveFactor;
			sampler2D _SSSLUT;
			sampler2D _ThicknessTex;
			float _Smoothness;

			float4 _InteriorColor;
			float _InteriorColorPower;
			float _FrontSurfaceDistortion;
			float _BackSurfaceDistortion;
			float _FrontSSSIntensity;
			float _BackSSSIntensity;

			float4 _CameraFront;

            v2f vert (appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.texcoord, _NormalTex);
				float3 worldPos=mul(unity_ObjectToWorld,v.vertex);
				float3 worldNormal=UnityObjectToWorldNormal(v.normal);
				o.worldOriNormal=worldNormal;
				float3 worldTangent=UnityObjectToWorldDir(v.tangent);
				float3 worldBitangent=cross(worldNormal,worldTangent)*v.tangent.w;
				o.T2W0 = float4 (worldTangent.x,worldBitangent.x,worldNormal.x,worldPos.x);
                o.T2W1 = float4 (worldTangent.y,worldBitangent.y,worldNormal.y,worldPos.y);
                o.T2W2 = float4 (worldTangent.z,worldBitangent.z,worldNormal.z,worldPos.z);
				o.frontDir=normalize(mul(unity_ObjectToWorld,float4(0,0,1,0)).xyz);
				o.clipFrontDir=normalize(mul(UNITY_MATRIX_MV,float4(0,0,1,0)).xyz);
                return o;
            }

			half3 BentNormalsDiffuseLighting(float3 normal, float3 L, float Curvature)
			{
				float NdotLBlurredUnclamped = dot(normal, L);
				return tex2D(_SSSLUT, float2(NdotLBlurredUnclamped * 0.5 + 0.5, Curvature)).xyz;
			}

			float fresnelReflectance( float3 H, float3 V, float F0 )
			{
  				float base = 1.0 - dot( V, H );
  				float exponential = pow( base, 5.0 );
  				return exponential + F0 * ( 1.0 - exponential );
			}

			float SubSurfaceScattering(float3 viewDir,float3 lightDir,float3 normalDir,float frontSubSurfaceDistortion,float backSubSurfaceDistortion,float frontSSSIntensity,float backSSSIntensity) {
				float3 frontLitDir = normalDir * frontSubSurfaceDistortion - lightDir;
				float3 backLitDir = normalDir * backSubSurfaceDistortion + lightDir;
				float frontSSS = saturate(dot(viewDir, -frontLitDir));
				float backSSS = saturate(dot(viewDir, -backLitDir));
				float result = saturate(frontSSS * frontSSSIntensity + backSSS*backSSSIntensity);
				return result;
			}

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				float4 tangentNormal=tex2D(_NormalTex,i.uv.zw);
				float3 bump=UnpackNormal(tangentNormal);
				float3 worldNormal=normalize(half3( dot(i.T2W0.xyz,bump),
                                                  dot(i.T2W1.xyz,bump),
                                                  dot(i.T2W2.xyz,bump)));
				float3 worldPos=float3(i.T2W0.w,i.T2W1.w,i.T2W2.w);
				float3 lightDir=normalize(UnityWorldSpaceLightDir(worldPos));
				float3 viewDir=normalize(UnityWorldSpaceViewDir(worldPos));
				float3 halfVector = normalize(lightDir + viewDir);

				float NdotL = saturate(dot(worldNormal, lightDir));
				float NdotV = saturate(dot(worldNormal, viewDir));
				float NdotH = saturate(dot(worldNormal, halfVector));
				float LdotH = saturate(dot(lightDir, halfVector));

				float smoothness=_Smoothness;
				float roughness=1-_Smoothness;
				roughness*=roughness;

				//curvature
				//float deltaWorldNormal = length(fwidth(worldNormal));
				//float deltaWorldPos = length(fwidth(worldPos));
				//float curvature = (deltaWorldNormal / deltaWorldPos) * _CurveFactor;

				//diffuse
				float3 directDiffuse=BentNormalsDiffuseLighting(normalize(i.worldOriNormal),lightDir,_CurveFactor);
				directDiffuse*=col.rgb;

				//specular
				float alpha = roughness;
				float G_L = NdotL + sqrt((NdotL - NdotL * alpha) * NdotL + alpha);
				float G_V = NdotV + sqrt((NdotV - NdotV * alpha) * NdotV + alpha);
				float G = G_L * G_V;
				//float3 F0 = 0.028;
				fixed F = fresnelReflectance(halfVector, viewDir, 0.028);
				float alpha2 = alpha * alpha;
				float denominator = (NdotH * NdotH) * (alpha2 - 1) + 1;
				float D = alpha2 / (UNITY_PI * denominator * denominator);
				half3 specularColor = D * G * NdotL * F;
				float3 ks=FresnelTerm(float3(1,1,1), LdotH);
				specularColor*=ks;

				//SSS Ear
				float SSS = SubSurfaceScattering(viewDir, lightDir, worldNormal, _FrontSurfaceDistortion,_BackSurfaceDistortion,_FrontSSSIntensity,_BackSSSIntensity);
				float3 SSSCol = lerp(_InteriorColor, float3(1,1,1), saturate(pow(SSS, _InteriorColorPower))).rgb*SSS;				
				float thickness=tex2D(_ThicknessTex,i.uv).r;
				SSSCol*=(1-thickness);
				float FdotL=saturate(dot(lightDir,i.frontDir));
				float CFdotF=abs(dot(float3(0,0,-1),i.clipFrontDir));
				SSSCol*=(1-FdotL)*pow(CFdotF,5);

				return float4(specularColor+directDiffuse+SSSCol,1);
            }
            ENDCG
        }
    }
}

