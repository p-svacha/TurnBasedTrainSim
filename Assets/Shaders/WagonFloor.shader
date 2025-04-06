Shader "Custom/WagonFloor"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        _GridTex("Grid Texture", 2D) = "none" {}
        [Toggle] _ShowGrid("Show Grid", Float) = 1
        _GridColor("Grid Color", Color) = (0,0,0,1)

        _WagonLength("Wagon Length (Tiles)", Float) = 24
        _WagonWidth("Wagon Width (Tiles)", Float) = 6
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_GridTex;
        };

        sampler2D _MainTex;
        sampler2D _GridTex;
        fixed4 _GridColor;
        float _ShowGrid;

        float _WagonLength;
        float _WagonWidth;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Check facing direction
            float3 worldNormal = UnityObjectToWorldNormal(o.Normal);
            float isFacingUpwards = (dot(normalize(worldNormal), float3(0, 1, 0)) > 0.5);

            // Grid Overlay
            if (_ShowGrid == 1)
            {
                if (isFacingUpwards == 1)
                {
                    // Scale UV by wagon dimensions
                    IN.uv_GridTex.x *= _WagonLength;
                    IN.uv_GridTex.y *= _WagonWidth;

                    fixed4 gridColor = tex2D(_GridTex, IN.uv_GridTex) * _GridColor;
                    c = (gridColor.a * gridColor) + ((1 - gridColor.a) * c);
                }
            }

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
