Shader "Custom/ToonShaderWithMaskAndSpecular"
{
    Properties
    {
        _MainTex("Base Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "white" {} // ����ũ �ؽ�ó
        _SpecularTex("Specular Texture", 2D) = "white" {} // Specular �ؽ�ó
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1) // �ܰ��� ����
        _OutlineThickness("Outline Thickness", Range(0, 0.1)) = 0.01 // �ܰ��� �β�
        _ShadowThreshold("Shadow Threshold", Range(0, 1)) = 0.5 // ���� �Ӱ谪
        _ShadowSmoothness("Shadow Smoothness", Range(0, 1)) = 0.1 // ���� �ε巯��
        _MaskStrength("Mask Strength", Range(0, 1)) = 1.0 // ����ũ ����
        _SpecularIntensity("Specular Intensity", Range(0, 1)) = 0.5 // �ݻ籤 ����
        _SpecularSmoothness("Specular Smoothness", Range(0, 1)) = 0.1 // �ݻ籤 �ε巯��
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            // �ܰ��� �н�
            Pass
            {
                Cull Front // �ܰ����� ���� ���� ���� ������
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
                    float3 outlineOffset = normal * _OutlineThickness; // ���� �������� �ܰ��� Ȯ��
                    o.pos = UnityObjectToClipPos(v.vertex + outlineOffset);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    return _OutlineColor; // �ܰ��� ���� ��ȯ
                }
                ENDCG
            }

            // ���� �н� (ī�� ������ + ����ũ �ؽ�ó + Specular �ؽ�ó)
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
                    // �⺻ �ؽ�ó ����
                    fixed4 baseColor = tex2D(_MainTex, i.uv);

                // ����ũ �ؽ�ó �� (��� �ؽ�ó�� ����)
                float maskValue = tex2D(_MaskTex, i.uv).r; // ����ũ �ؽ�ó�� ���� ä�� ���
                maskValue = lerp(1.0, maskValue, _MaskStrength); // ����ũ ���� ����

                // �� ���� ���
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(normal, lightDir);

                // ���� ��� (������ �� ���)
                float shadow = smoothstep(_ShadowThreshold - _ShadowSmoothness, _ShadowThreshold + _ShadowSmoothness, NdotL);

                // ����ũ �ؽ�ó�� �����Ͽ� ���� ����
                shadow = lerp(shadow, 1.0, 1.0 - maskValue); // ����ũ ������ ���� ���� ���� ����

                // Specular ��� (�ݻ籤)
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 halfDir = normalize(lightDir + viewDir);
                float NdotH = dot(normal, halfDir);
                float specular = smoothstep(0.5 - _SpecularSmoothness, 0.5 + _SpecularSmoothness, NdotH);
                float specularMask = tex2D(_SpecularTex, i.uv).r; // Specular �ؽ�ó�� ���� ä�� ���
                specular *= specularMask * _SpecularIntensity; // Specular ���� ����

                // ���� ���� ��� (�⺻ ���� + ���� + Specular)
                fixed3 finalColor = baseColor.rgb * shadow + specular;
                return fixed4(finalColor, baseColor.a);
            }
            ENDCG
        }
        }
            FallBack "Diffuse"
}