Shader "Custom/SparklingShader"
{
    Properties
    {
        _Color("Base Color", Color) = (1,1,1,1)
        _EmissionColor("Emission Color", Color) = (1,1,1,1)
        _SparkleSpeed("Sparkle Speed", Range(0.1, 5)) = 1
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        // 기본 Pass (빛과 그림자 처리)
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows // 그림자 지원 추가
        #pragma target 3.0

        float4 _Color;
        float4 _EmissionColor;
        float _SparkleSpeed;

        struct Input
        {
            float2 uv_MainTex; // UV 좌표는 남겨둡니다 (다른 용도로 사용될 수 있음)
        };

        // 반짝이는 효과 계산
        float GetSparkle()
        {
            return sin(_Time.y * _SparkleSpeed) * 0.5 + 0.5; // 시간에 따라 반짝이는 효과
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // 반짝이는 효과 추가
            float sparkle = GetSparkle();
            
            // 표면 출력
            o.Albedo = _Color.rgb; // 기본 색상
            o.Emission = _EmissionColor.rgb * sparkle; // 발광 효과
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Diffuse" // 그림자 지원을 위한 Fallback
}