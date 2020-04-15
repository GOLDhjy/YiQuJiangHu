Shader "Custom/StandardHair"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("NormalMap",2D)="black"{}
		_MetallicMap("MetallicMap",2D)="white"{}
		_Metallic ("Metallic", Range(0,1)) = 1
        _Glossiness ("Smoothness", Range(0,1)) = 1
		_EmissionMap("EmissionMap",2D)="black"{}
		[HDR]_EmissionColor("EmissionColor",color)= (1,1,1,1)
    }
    SubShader
    {
        
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

        LOD 200
		cull off
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _MetallicMap;
		sampler2D _EmissionMap;
		half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		fixed4 _EmissionColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        

        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
			o.Alpha=c.a;
            // Metallic and smoothness come from slider variables
            fixed4 m = tex2D (_MetallicMap, IN.uv_MainTex);
			o.Metallic = m.rgb;
            o.Smoothness = m.a*_Glossiness;
			fixed4 e = tex2D (_EmissionMap, IN.uv_MainTex)* _EmissionColor;
			o.Emission = e.rgb;
            o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_NormalMap,IN.uv_MainTex));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
