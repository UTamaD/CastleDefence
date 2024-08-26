Shader "Custom/OutlineShader_2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LineColor ("Line Color", Color) = (0,0,0,1)
        _SurfaceColor ("Surface Color", Color) = (1,1,1,1)
        _LineWidth ("Line Width", Range(0, 0.1)) = 0.01
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
        fixed4 _LineColor;
        fixed4 _SurfaceColor;
        float _LineWidth;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Sample the texture color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Calculate the distance from the center of the UV space
            float2 uvDist = abs(IN.uv_MainTex - 0.5) * 2;
            float maxDist = max(uvDist.x, uvDist.y);

            // Determine whether the pixel is part of the border
            float isBorder = step(1 - _LineWidth, maxDist);

            // Set the Albedo using the color interpolation between surface and border colors
            o.Albedo = lerp(_SurfaceColor.rgb, _LineColor.rgb, isBorder);
            
            // Set the Alpha considering both the texture and surface color alpha values
            float alpha = lerp(c.a, _SurfaceColor.a, 1 - isBorder);  // Use the surface alpha for the interior
            o.Alpha = lerp(alpha, _LineColor.a, isBorder);  // Interpolate with line alpha for the border
        }
        ENDCG
    }
    FallBack "Diffuse"
}