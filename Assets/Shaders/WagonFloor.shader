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
        _TileSize("Tile Size", Float) = 0.5

        _FreeColor("Free Tile Color", Color) = (0,1,0,1)
        _BlockedColor("Blocked Color", Color) = (1,0,0,1)
        _InteractionSpotColor("Interaction Spot Color", Color) = (0,0,1,1)
        _ErrorColor("Error Color", Color) = (1,0,1,1)

        [Toggle] _ShowTileOccupation("Show Tile Occupation", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
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
        float _TileSize;

        fixed4 _FreeColor;
        fixed4 _BlockedColor;
        fixed4 _InteractionSpotColor;
        fixed4 _ErrorColor;

        float _ShowTileOccupation;
        float _TileOccupation[256];

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            
            // Calculate tile coordinates
            int tileCoordinatesX = int(IN.uv_MainTex.x * _WagonLength);
            int tileCoordinatesY = int(IN.uv_MainTex.y * _WagonWidth);
            int tileIndex = (tileCoordinatesX * _WagonWidth) + tileCoordinatesY;

            // Check facing direction
            float3 worldNormal = UnityObjectToWorldNormal(o.Normal);
            float isFacingUpwards = (dot(normalize(worldNormal), float3(0, 1, 0)) > 0.5);

            // Occupation Overlay
            if (_ShowTileOccupation && isFacingUpwards == 1)
            {
                int occupation = _TileOccupation[tileIndex];
                if (occupation == 0) c = _FreeColor;
                else if (occupation == 1) c = _BlockedColor;
                else if (occupation == 2) c = _InteractionSpotColor;
                else c = _ErrorColor;
            }

            // Grid Overlay
            if (_ShowGrid == 1 && isFacingUpwards == 1)
            {
                // Scale UV by wagon dimensions
                IN.uv_GridTex.x *= _WagonLength;
                IN.uv_GridTex.y *= _WagonWidth;

                fixed4 gridColor = tex2D(_GridTex, IN.uv_GridTex) * _GridColor;
                c = (gridColor.a * gridColor) + ((1 - gridColor.a) * c);
            }
            
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
        }
    FallBack "Diffuse"
}