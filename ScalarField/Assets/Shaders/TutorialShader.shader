// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/TutorialShader"
{	
	Properties
	{
		_MinValue("MinValue", float) = 0
		_MaxValue("MaxValue", float) = 1
		
		_Color01("Color01", Color) = (0.01961, 0.44314, 0.69020, 1)
		_Color02("Color02", Color) = (0.57647, 0.77255, 0.87059, 1)
		_Color03("Color03", Color) = (0.96863, 0.96863, 0.96863, 1)
		_Color04("Color04", Color) = (0.95686, 0.64706, 0.50980, 1)
		_Color05("Color05", Color) = (0.79216, 0, 0.12549, 1)
		
		_ColorCount("ColorCount", int) = 5
	}

	SubShader
    {
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct input
			{
				float4 vertex : POSITION;
			};
 
		    struct FromVertToFrag
		    {
				float4 vertex : SV_POSITION;
		    	float3 worldPos : TEXCOORD0;
		    	float3 objPos : TEXCOORD1;
		    };
 
		    FromVertToFrag vert(input v)
		    {
				FromVertToFrag o;
				// compute world space position of the vertex
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				o.objPos = v.vertex;
		    	
		    	//float3 worldPos = mul (unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
		    }

			float4 _Color01;
			float4 _Color02;
			float4 _Color03;
			float4 _Color04;
			float4 _Color05;
			
			float _MaxValue;
			float _MinValue;

			int _ColorCount;

			float4 _FloorColor;
			float4 _CeilingColor;

			float4 CalcualteBlendedColor(float4 baseColor, float4 floorColor, float4 ceilingColor,
				float4 positionalFactor, float4 blendingFactor)
			{
				float4 blendingColor = float4(0, 0, 0, 1);
				blendingColor += (1.0 - positionalFactor) * floorColor;
				blendingColor += positionalFactor * ceilingColor;

				float4 retCol = (1.0 - blendingFactor) * baseColor;
				retCol += blendingFactor * blendingColor;
				
				return retCol;
			}
			
			float4 colorTransferFunction(float4 pos, float3 objPos, float3 worldPos)
			{
				// Determine color index
				float value = worldPos.y;

				//if(value < _MinValue || value > _MaxValue) return float4(0, 0, 0, 1);
				
				float rawts = ((value - _MinValue) / (_MaxValue - _MinValue)) * (_ColorCount - 1);
				uint ts = (uint) rawts;

				// Calculate blending factor
				float range = _MaxValue - _MinValue;
				float val = value - _MinValue;
				float factor = val / range;

				// Activate blending, i.e. interpolate neighbouring colors into color of current class
				bool blendColors = false;				
				float blendingFactor = 0.25f;
				
				switch(ts)
				{					
					case 0: return blendColors ?
						CalcualteBlendedColor(_Color01, _FloorColor, _Color02,
							factor, blendingFactor) : _Color01;
					case 1: return blendColors ? CalcualteBlendedColor(_Color02, _Color01,
						_Color03, factor, blendingFactor) : _Color02;
					case 2: return blendColors ? CalcualteBlendedColor(_Color03, _Color02,
						_Color04, factor, blendingFactor) : _Color03;
					//case 1: return _Color02 + (blendColors ? (1.0 - factor) * _Color01 + factor * _Color03 : 0);
					//case 2: return _Color03 + (blendColors ? (1.0 - factor) * _Color02 + factor * _Color04 : 0);
					case 3: return _Color04 + (blendColors ? (1.0 - factor) * _Color03 + factor * _Color05 : 0);
					case 4: return _Color05 + (blendColors ? (1.0 - factor) * _Color04 + factor * _CeilingColor : 0);
					default: return _Color05; //float4(0, 0, 0, 1);
				}

			}

			

			
		    float4 frag(FromVertToFrag i) : SV_Target
		    {				
			   return colorTransferFunction(i.vertex, i.objPos, i.worldPos); //float4(1, 1, 0, 1);
		    }
			
			
 
		   ENDCG
		
		}

	
		
    }
	
	Fallback "Diffuse"    
}
