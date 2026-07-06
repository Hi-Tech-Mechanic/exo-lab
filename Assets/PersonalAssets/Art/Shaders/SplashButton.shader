Shader "CustomRenderTexture/SplashButton"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _RibbonColor ("Ribbon Color", Color) = (1,1,1,0.5)
        _FillAmount ("Fill Amount", Range(0, 1)) = 0
        _FillDirection ("Fill Direction", Int) = 0 // 0=Bottom-Up, 1=Top-Down, 2=Left-Right, 3=Right-Left
        _FillIntensity ("Fill Intensity", Range(0, 2)) = 1
        _EdgeSmoothness ("Edge Smoothness", Range(0, 0.1)) = 0.02
    }
    
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _RibbonColor;  // <-- Исправлено здесь
            float _FillAmount;
            int _FillDirection;
            float _FillIntensity;
            float _EdgeSmoothness;
            
            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color;
                
                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                float fillValue = 0;
                float edgeFactor = 0;
                
                // Определяем направление заполнения
                if (_FillDirection == 0) // Bottom to Top
                {
                    fillValue = IN.texcoord.y;
                }
                else if (_FillDirection == 1) // Top to Bottom
                {
                    fillValue = 1 - IN.texcoord.y;
                }
                else if (_FillDirection == 2) // Left to Right
                {
                    fillValue = IN.texcoord.x;
                }
                else if (_FillDirection == 3) // Right to Left
                {
                    fillValue = 1 - IN.texcoord.x;
                }
                
                // Применяем эффект если точка внутри зоны заполнения
                if (fillValue <= _FillAmount)
                {
                    // Создаем мягкий край
                    float distanceFromEdge = _FillAmount - fillValue;
                    float edgeBlend = smoothstep(0, _EdgeSmoothness, distanceFromEdge);
                    
                    // Смешиваем цвета
                    color.rgb = lerp(color.rgb, _RibbonColor.rgb, edgeBlend * _FillIntensity);
                    color.a = max(color.a, _RibbonColor.a * edgeBlend);
                }
                
                return color;
            }
            ENDCG
        }
    }
}
