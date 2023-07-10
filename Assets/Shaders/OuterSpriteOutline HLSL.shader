Shader "Unlit/InnerSpriteOutline HLSL"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _EnableOutline("Enable Outline", Float) = 1.0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            fixed4 _OutlineColor;
            float _EnableOutline;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                if (_EnableOutline > 0.5)
                {
                    fixed leftPixel = tex2D(_MainTex, i.uv + float2(-_MainTex_TexelSize.x, 0)).a;
                    fixed upPixel = tex2D(_MainTex, i.uv + float2(0, _MainTex_TexelSize.y)).a;
                    fixed rightPixel = tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.x, 0)).a;
                    fixed bottomPixel = tex2D(_MainTex, i.uv + float2(0, -_MainTex_TexelSize.y)).a;

                    fixed outline = max(max(leftPixel, upPixel), max(rightPixel, bottomPixel)) - col.a;

                    return lerp(col, _OutlineColor, outline);
                }
                else
                {
                    return col;
                }
            }
            ENDCG
        }
    }
}


//Shader "Unlit/InnerSpriteOutline HLSL"
//{
//    Properties
//    {
//        _MainTex ("Texture", 2D) = "white" {}
//        _OutlineColor("Outline Color", Color) = (1,1,1,1)
//        _EnableOutline("Enable Outline", Float) = 1.0
//    }
//    SubShader
//    {
//        Tags
//        {
//            "RenderType" = "Transparent"
//        }

//        Blend SrcAlpha OneMinusSrcAlpha

//        Pass
//        {
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag

//            #include "UnityCG.cginc"

//            struct appdata
//            {
//                float4 vertex : POSITION;
//                float2 uv : TEXCOORD0;
//            };

//            struct v2f
//            {
//                float2 uv : TEXCOORD0;
//                float4 vertex : SV_POSITION;
//            };

//            sampler2D _MainTex;
//            float4 _MainTex_ST;
//            float4 _MainTex_TexelSize;

//            fixed4 _OutlineColor;
//            float _EnableOutline;

//            v2f vert (appdata v)
//            {
//                v2f o;
//                o.vertex = UnityObjectToClipPos(v.vertex);
//                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                return o;
//            }


//            fixed4 frag (v2f i) : SV_Target
//            {
//                fixed4 col = tex2D(_MainTex, i.uv);

//                if (_EnableOutline > 0.5)
//                {
//                    fixed leftPixel = tex2D(_MainTex, i.uv + float2(-2 * _MainTex_TexelSize.x, 0)).a;
//                    fixed upPixel = tex2D(_MainTex, i.uv + float2(0, 2 * _MainTex_TexelSize.y)).a;
//                    fixed rightPixel = tex2D(_MainTex, i.uv + float2(2 * _MainTex_TexelSize.x, 0)).a;
//                    fixed bottomPixel = tex2D(_MainTex, i.uv + float2(0, -2 * _MainTex_TexelSize.y)).a;

//                    fixed upLeftPixel = tex2D(_MainTex, i.uv + float2(-2 * _MainTex_TexelSize.x, 2 * _MainTex_TexelSize.y)).a;
//                    fixed upRightPixel = tex2D(_MainTex, i.uv + float2(2 * _MainTex_TexelSize.x, 2 * _MainTex_TexelSize.y)).a;
//                    fixed downLeftPixel = tex2D(_MainTex, i.uv + float2(-2 * _MainTex_TexelSize.x, -2 * _MainTex_TexelSize.y)).a;
//                    fixed downRightPixel = tex2D(_MainTex, i.uv + float2(2 * _MainTex_TexelSize.x, -2 * _MainTex_TexelSize.y)).a;

//                    fixed outline = max(max(max(max(max(leftPixel, upPixel), rightPixel), bottomPixel), upLeftPixel), max(max(upRightPixel, downLeftPixel), downRightPixel)) - col.a;

//                    return lerp(col, _OutlineColor, outline);
//                }
//                else
//                {
//                    return col;
//                }
//            }
//            ENDCG
//        }
//    }
//}
