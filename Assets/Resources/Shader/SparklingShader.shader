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

        // �⺻ Pass (���� �׸��� ó��)
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows // �׸��� ���� �߰�
        #pragma target 3.0

        float4 _Color;
        float4 _EmissionColor;
        float _SparkleSpeed;

        struct Input
        {
            float2 uv_MainTex; // UV ��ǥ�� ���ܵӴϴ� (�ٸ� �뵵�� ���� �� ����)
        };

        // ��¦�̴� ȿ�� ���
        float GetSparkle()
        {
            return sin(_Time.y * _SparkleSpeed) * 0.5 + 0.5; // �ð��� ���� ��¦�̴� ȿ��
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // ��¦�̴� ȿ�� �߰�
            float sparkle = GetSparkle();
            
            // ǥ�� ���
            o.Albedo = _Color.rgb; // �⺻ ����
            o.Emission = _EmissionColor.rgb * sparkle; // �߱� ȿ��
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Diffuse" // �׸��� ������ ���� Fallback
}