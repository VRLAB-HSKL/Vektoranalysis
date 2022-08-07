Shader "Example/Diffuse Simple" {
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float4 color : COLOR;
          
      };
      void surf (Input IN, inout SurfaceOutput o) {
        o.Albedo = float4(1, 0, 0, 0);
        
        
          
        // var maxz = vertices.Max(v => v.z);
        // var minz = vertices.Min(v => v.z);
        // var rangeZ = math.abs(maxz - minz);
        // //var step = rangeZ / 255f;
        // var colorList = new List<Color>();
        //
        // var n = colors.Length;
        //
        // for (int i = 0; i < vertices.Count; i++)
        // {
        //     var z = vertices[i].z;
        //     float raw_ts = ((z - minz) / (maxz - minz)) * (n - 1);
        //     int ts = (int) raw_ts;
        //     colorList.Add(colors[ts]);
        // }
        //
        // return colorList;

          
      }

      void max()
      {
          
      }
      
      ENDCG
    }
    Fallback "Diffuse"
  }