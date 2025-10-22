Shader "Custom/SpriteWithOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Header(Outline)]
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _Thickness ("Outline Thickness", Range(0, 10)) = 1.5
        
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            Name "Outline"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _OutlineColor;
            float _Thickness;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif
                return OUT;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            fixed4 frag(v2f IN) : SV_Target
            {
                // Sample sprite texture
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                // If pixel is transparent, check for outline
                if (c.a < 0.1)
                {
                    float totalAlpha = 0.0;
                    float stepSize = _MainTex_TexelSize.x * _Thickness;
                    
                    // Sample surrounding pixels (8 directions)
                    totalAlpha += tex2D(_MainTex, IN.texcoord + float2(stepSize, 0)).a;
                    totalAlpha += tex2D(_MainTex, IN.texcoord + float2(-stepSize, 0)).a;
                    totalAlpha += tex2D(_MainTex, IN.texcoord + float2(0, stepSize)).a;
                    totalAlpha += tex2D(_MainTex, IN.texcoord + float2(0, -stepSize)).a;
                    totalAlpha += tex2D(_MainTex, IN.texcoord + float2(stepSize, stepSize)).a;
                    totalAlpha += tex2D(_MainTex, IN.texcoord + float2(-stepSize, stepSize)).a;
                    totalAlpha += tex2D(_MainTex, IN.texcoord + float2(stepSize, -stepSize)).a;
                    totalAlpha += tex2D(_MainTex, IN.texcoord + float2(-stepSize, -stepSize)).a;
                    
                    if (totalAlpha > 0.1)
                    {
                        c = _OutlineColor;
                        c.a = 1.0;
                    }
                }
                
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}