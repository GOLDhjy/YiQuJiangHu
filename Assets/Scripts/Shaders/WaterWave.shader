Shader "PP/WaterWave" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
 
	CGINCLUDE
	#include "UnityCG.cginc"
	uniform sampler2D _MainTex;
	float4 _MainTex_TexelSize;
	uniform float _distanceFactor;
	uniform float _timeFactor;
	uniform float _totalFactor;
	uniform float _waveWidth;
	uniform float _curWaveDis0;
	uniform float _curWaveDis1;
	uniform float _curWaveDis2;
	uniform float _curWaveDis3;
	uniform float _curWaveDis4;
	uniform float4 _startPos0;
	uniform float4 _startPos1;
	uniform float4 _startPos2;
	uniform float4 _startPos3;
	uniform float4 _startPos4;
	uniform int count;
 
	fixed4 frag(v2f_img i) : SV_Target
	{
		//DX下纹理坐标反向问题
		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			_startPos0.y = 1 - _startPos0.y;
			//_startPos1.y = 1 - _startPos1.y;
		#endif
		//计算uv到中间点的向量(向外扩，反过来就是向里缩)
		float2 dv = _startPos0.xy - i.uv;
		float2 dv1 = _startPos1.xy - i.uv;
		float2 dv2 = _startPos2.xy - i.uv;
		float2 dv3 = _startPos3.xy - i.uv;
		float2 dv4 = _startPos4.xy - i.uv;
		
		
		//按照屏幕长宽比进行缩放
		dv = dv * float2(_ScreenParams.x / _ScreenParams.y, 1);
		dv1 = dv1 * float2(_ScreenParams.x / _ScreenParams.y, 1);		
		dv2 = dv2 * float2(_ScreenParams.x / _ScreenParams.y, 1);
		dv3 = dv3 * float2(_ScreenParams.x / _ScreenParams.y, 1);
		dv4 = dv4 * float2(_ScreenParams.x / _ScreenParams.y, 1);
		//计算像素点距中点的距离
		float dis = sqrt(dv.x * dv.x + dv.y * dv.y);
		float dis1 = sqrt(dv1.x * dv1.x + dv1.y * dv1.y);
		float dis2 = sqrt(dv2.x * dv2.x + dv2.y * dv2.y);
		float dis3 = sqrt(dv3.x * dv3.x + dv3.y * dv3.y);
		float dis4 = sqrt(dv4.x * dv4.x + dv4.y * dv4.y);
		//用sin函数计算出波形的偏移值factor
		//dis在这里都是小于1的，所以我们需要乘以一个比较大的数，比如60，这样就有多个波峰波谷
		//sin函数是（-1，1）的值域，我们希望偏移值很小，所以这里我们缩小100倍，据说乘法比较快,so...
		//float sinFactor = (sin(dis * _distanceFactor + _Time.y * _timeFactor) + sin(dis2 * _distanceFactor + _Time.y * _timeFactor) )* _totalFactor * 0.01;
		float sinFactor = sin(dis * _distanceFactor + _Time.y * _timeFactor)* _totalFactor * 0.01;
		float sinFactor1 = sin(dis1 * _distanceFactor + _Time.y * _timeFactor)* _totalFactor * 0.01;
		float sinFactor2 = sin(dis2 * _distanceFactor + _Time.y * _timeFactor)* _totalFactor * 0.01;
		float sinFactor3 = sin(dis3 * _distanceFactor + _Time.y * _timeFactor)* _totalFactor * 0.01;
		float sinFactor4 = sin(dis4 * _distanceFactor + _Time.y * _timeFactor)* _totalFactor * 0.01;
		//距离当前波纹运动点的距离，如果小于waveWidth才予以保留，否则已经出了波纹范围，factor通过clamp设置为0
		float discardFactor = clamp(_waveWidth - abs(_curWaveDis0 - dis), 0, 1) / _waveWidth;
		float discardFactor1 = clamp(_waveWidth - abs(_curWaveDis1 - dis1), 0, 1) / _waveWidth;
		float discardFactor2 = clamp(_waveWidth - abs(_curWaveDis2 - dis2), 0, 1) / _waveWidth;
		float discardFactor3 = clamp(_waveWidth - abs(_curWaveDis3 - dis3), 0, 1) / _waveWidth;
		float discardFactor4 = clamp(_waveWidth - abs(_curWaveDis4 - dis4), 0, 1) / _waveWidth;
		
		//归一化
		float2 dvv = normalize(dv);
		float2 dvv1 = normalize(dv1);
		float2 dvv2 = normalize(dv2);
        float2 dvv3 = normalize(dv3);
        float2 dvv4 = normalize(dv4);
		//计算每个像素uv的偏移值
		float2 offset = dvv  * sinFactor * discardFactor;
		float2 offset1 = dvv1  * sinFactor1 * discardFactor1;
		float2 offset2 = dvv2  * sinFactor2 * discardFactor2;
		float2 offset3 = dvv3  * sinFactor3 * discardFactor3;
		float2 offset4 = dvv4  * sinFactor4 * discardFactor4;

		//像素采样时偏移offset
		float2 uv;
		
		if(count == 1)
		{
			uv = offset + i.uv;
		}
		if(count == 2)
		{
            uv = offset + offset1 + i.uv ;			
		}
		if(count == 3)
		{
            uv = offset + offset1 + offset2 + i.uv;			
		}
		if(count == 4)
		{
			uv = i.uv + offset + offset1 + offset2 + offset3;
		}
		if(count == 0)
		{
			uv = i.uv + offset + offset1 + offset2 + offset3 + offset4;
		}
		
		return tex2D(_MainTex, uv);
	}
 
	ENDCG
 
	SubShader 
	{
		Pass
		{
			ZTest Always
			Cull Off
			ZWrite Off
			Fog { Mode off }
 
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			ENDCG
		}
	}
	Fallback off
}
