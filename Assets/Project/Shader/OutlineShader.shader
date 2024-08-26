Shader "Custom/OutlineShader"
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
            // 현재 나의 uv값을 가져온다. ( 색상 정보 )
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            // left right를 나누는 코드다
            // 0 ~ 1 -> -0.5 ~ 0.5 -> abs를 통해 0 ~ 0.5
            float2 uvDist = abs(IN.uv_MainTex - 0.5) * 2;

            // 어느 직각에 더 가까운지 체크한다.
            float maxDist = max(uvDist.x, uvDist.y);

            // 1 - 두께를 하면 이것 0.8 -> maxDist 0.7 -> 0반환
            // 0.8 -> maxDist 0.9 -> 1반환하는 함수
            float isBorder = step(1 - _LineWidth, maxDist);

            // 0 ~ 1사이 값을 반환한다.
            o.Albedo = lerp(_SurfaceColor.rgb, _LineColor.rgb, isBorder);
            
            // 그냥 투명도 있을시 넣어주는거다.
            o.Alpha = lerp(c.a, _LineColor.a, isBorder);
        }
        ENDCG
    }
    FallBack "Diffuse"
}   