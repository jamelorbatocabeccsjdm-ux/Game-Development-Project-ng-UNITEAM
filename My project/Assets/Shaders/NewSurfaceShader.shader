Shader "Custom/SpriteCutout"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _CutoutPos ("Cutout Position", Vector) = (0,0,0,0)
        _Radius ("Cutout Radius", Float) = 1.0
        _Softness ("Softness", Float) = 0.5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _Color;
            float4 _CutoutPos;
            float _Radius;
            float _Softness;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                
                // Calculate distance from this pixel to the player
                float dist = distance(i.worldPos.xyz, _CutoutPos.xyz);
                
                // Smoothly calculate alpha based on radius
                float mask = smoothstep(_Radius, _Radius + _Softness, dist);
                
                col.a *= mask;
                return col;
            }
            ENDCG
        }
    }
}