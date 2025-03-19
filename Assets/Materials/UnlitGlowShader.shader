Shader"Custom/GlowSprite" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}

        _Brightness ("Brightness", Float) = 1 // 亮度
        _Saturation("Saturation", Float) = 1 // 饱和度
        _Contrast("Contrast", Float) = 1 // 对比度
    }

    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
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
            half _Brightness; // 亮度
            half _Saturation; // 饱和度
            half _Contrast; // 对比度

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv); // 纹理采样
                fixed3 finalColor = tex.rgb * _Brightness; // 应用亮度_Brightness
                fixed luminance = 0.2125 * tex.r + 0.7154 * tex.g + 0.0721 * tex.b; // 计算亮度
                fixed3 luminanceColor = fixed3(luminance, luminance, luminance); // 饱和度为0、亮度为luminance的颜色
                finalColor = lerp(luminanceColor, finalColor, _Saturation); // 应用饱和度_Saturation
                fixed3 avgColor = fixed3(0.5, 0.5, 0.5); // 饱和度为0、亮度为0.5的颜色
                finalColor = lerp(avgColor, finalColor, _Contrast); // 应用对比度_Contrast
                return fixed4(finalColor, tex.a);
            }
            ENDCG
        }
    }
}