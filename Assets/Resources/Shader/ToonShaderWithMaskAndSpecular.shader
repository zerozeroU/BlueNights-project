Shader "Custom/ToonShaderWithMaskAndSpecular"
{
    Properties
    {
        _MainTex("Base Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "white" {} // 마스크 텍스처
        _SpecularTex("Specular Texture", 2D) = "white" {} // Specular 텍스처
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1) // 외곽선 색상
        _OutlineThickness("Outline Thickness", Range(0, 0.1)) = 0.01 // 외곽선 두께
        _ShadowThreshold("Shadow Threshold", Range(0, 1)) = 0.5 // 음영 임계값
        _ShadowSmoothness("Shadow Smoothness", Range(0, 1)) = 0.1 // 음영 부드러움
        _MaskStrength("Mask Strength", Range(0, 1)) = 1.0 // 마스크 강도
        _SpecularIntensity("Specular Intensity", Range(0, 1)) = 0.5 // 반사광 강도
        _SpecularSmoothness("Specular Smoothness", Range(0, 1)) = 0.1 // 반사광 부드러움
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            // 외곽선 패스
            Pass
            {
                Cull Front // 외곽선을 위해 뒤쪽 면을 렌더링
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                };

                float _OutlineThickness;
                float4 _OutlineColor;

                v2f vert(appdata v)
                {
                    v2f o;
                    float3 normal = normalize(v.normal);
                    float3 outlineOffset = normal * _OutlineThickness; // 법선 방향으로 외곽선 확장
                    o.pos = UnityObjectToClipPos(v.vertex + outlineOffset);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    return _OutlineColor; // 외곽선 색상 반환
                }
                ENDCG
            }

            // 메인 패스 (카툰 렌더링 + 마스크 텍스처 + Specular 텍스처)
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                #include "Lighting.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 pos : SV_POSITION;
                    float3 worldNormal : NORMAL;
                    float3 worldPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                sampler2D _MaskTex;
                sampler2D _SpecularTex;
                float _ShadowThreshold;
                float _ShadowSmoothness;
                float _MaskStrength;
                float _SpecularIntensity;
                float _SpecularSmoothness;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // 기본 텍스처 색상
                    fixed4 baseColor = tex2D(_MainTex, i.uv);

                // 마스크 텍스처 값 (흑백 텍스처로 가정)
                float maskValue = tex2D(_MaskTex, i.uv).r; // 마스크 텍스처의 빨간 채널 사용
                maskValue = lerp(1.0, maskValue, _MaskStrength); // 마스크 강도 조절

                // 빛 방향 계산
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(normal, lightDir);

                // 음영 계산 (간단한 빛 계산)
                float shadow = smoothstep(_ShadowThreshold - _ShadowSmoothness, _ShadowThreshold + _ShadowSmoothness, NdotL);

                // 마스크 텍스처를 적용하여 음영 조절
                shadow = lerp(shadow, 1.0, 1.0 - maskValue); // 마스크 영역에 따라 음영 강도 조절

                // Specular 계산 (반사광)
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 halfDir = normalize(lightDir + viewDir);
                float NdotH = dot(normal, halfDir);
                float specular = smoothstep(0.5 - _SpecularSmoothness, 0.5 + _SpecularSmoothness, NdotH);
                float specularMask = tex2D(_SpecularTex, i.uv).r; // Specular 텍스처의 빨간 채널 사용
                specular *= specularMask * _SpecularIntensity; // Specular 강도 조절

                // 최종 색상 계산 (기본 색상 + 음영 + Specular)
                fixed3 finalColor = baseColor.rgb * shadow + specular;
                return fixed4(finalColor, baseColor.a);
            }
            ENDCG
        }
        }
            FallBack "Diffuse"
}