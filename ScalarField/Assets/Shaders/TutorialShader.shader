// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/TutorialShader"
{	
	Properties
	{
		_BaseValue("BaseValue", float) = 1.25
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
			
			float4 colorTransferFunction(float4 pos, float3 objPos, float3 worldPos)
			{
				float value = worldPos.y;
				float rawts = ((value - _MinValue) / (_MaxValue - _MinValue)) * (_ColorCount - 1);
				uint ts = (int) rawts;

				// if(value <= 0.25) ts = 0;
				//
				// if(value > 0.25 && value < 0.75) ts = 1;
				//
				// if(value <= 100) ts = 2;

				float range = _MaxValue - _MinValue;
				float val = value - _MinValue;
				float factor = val / range;
				
				switch(ts)
				{					
					case 0: return _Color01 * factor;
					case 1: return _Color02 * factor;
					case 2: return _Color03 * factor;
					case 3: return _Color04 * factor;

					default:
					case 4: return _Color05 * factor;
					//float4(1, 1, 1, 1);					
				}

				//
				// for (int i = 0; i < vertices.Count; i++)
				  //       {
				  //           var z = vertices[i].z;
				  //           float raw_ts = ((z - minz) / (maxz - minz)) * (n - 1);
				  //           int ts = (int) raw_ts;
				  //           colorList.Add(colors[ts]);
				  //       }



				

				// if(z < 0)
				// {
				// 	return _Color01;
				// }
				//
				// if(z == 0)
				// {
				// 	return _Color02;
				// }
				//
				// if(z > 0)
				// 	return _Color03;			
				//

				return float4(1, 1, 1, 1);
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
