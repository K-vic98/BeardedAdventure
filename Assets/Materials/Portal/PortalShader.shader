Shader "Unlit/PortalShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Pixelate ("Pixelate", Vector) = (1, 1, 1, 1)
        _MainColor ("Color", Color) = (1, 0.5, 0.2, 1)
        
        _NoiseSize ("Noise Size", Vector) = (1, 1, 1, 0)
        _DistorionForce ("Distorion Force", Vector) = (0.1, 0.1, 0.1, 0)
        _SwirlForce ("Swirl Force", Vector) = (0, 2, 1, 0)
        _Speed ("Speed", Vector) = (1, 1, 1, 0)
    }
    SubShader
    {
        Tags {
          "RenderType"="Transparent"
          "Queue" = "Transparent"
        }
        LOD 100
        
        ZWrite Off
     Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float2 _Pixelate;
            float4 _MainColor;
            
            float4 _NoiseSize;
            float3 _DistorionForce;
            float3 _SwirlForce;
            float3 _Speed;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed3 mod289(fixed3 x)
            {
                return x - floor(x * (1.0 / 289.0)) * 289.0;
            }
            
            fixed2 mod289(fixed2 x)
            {
                return x - floor(x * (1.0 / 289.0)) * 289.0;
            }
            
            fixed3 permute(fixed3 x)
            {
                return mod289((x * 34.0 + 1.0) * x);
            }
            
            fixed3 taylorInvSqrt(fixed3 r)
            {
                return 1.79284291400159 - 0.85373472095314 * r;
            }
            
            float snoise(fixed2 v)
            {
                const fixed4 C = fixed4( 0.211324865405187,  // (3.0-sqrt(3.0))/6.0
                                     0.366025403784439,  // 0.5*(sqrt(3.0)-1.0)
                                    -0.577350269189626,  // -1.0 + 2.0 * C.x
                                     0.024390243902439); // 1.0 / 41.0
            
                // First corner
                fixed2 i  = floor(v + dot(v, C.yy));
                fixed2 x0 = v -   i + dot(i, C.xx);
            
                // Other corners
                fixed2 i1;
                i1.x = step(x0.y, x0.x);
                i1.y = 1.0 - i1.x;
            
                // x1 = x0 - i1  + 1.0 * C.xx;
                // x2 = x0 - 1.0 + 2.0 * C.xx;
                fixed2 x1 = x0 + C.xx - i1;
                fixed2 x2 = x0 + C.zz;
            
                // Permutations
                i = mod289(i); // Avoid truncation effects in permutation
                fixed3 p =
                  permute(permute(i.y + fixed3(0.0, i1.y, 1.0))
                                + i.x + fixed3(0.0, i1.x, 1.0));
            
                fixed3 m = max(0.5 - fixed3(dot(x0, x0), dot(x1, x1), dot(x2, x2)), 0.0);
                m = m * m;
                m = m * m;
            
                // Gradients: 41 points uniformly over a line, mapped onto a diamond.
                // The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)
                fixed3 x = 2.0 * frac(p * C.www) - 1.0;
                fixed3 h = abs(x) - 0.5;
                fixed3 ox = floor(x + 0.5);
                fixed3 a0 = x - ox;
            
                // Normalise gradients implicitly by scaling m
                m *= taylorInvSqrt(a0 * a0 + h * h);
            
                // Compute final noise value at P
                fixed3 g;
                g.x = a0.x * x0.x + h.x * x0.y;
                g.y = a0.y * x1.x + h.y * x1.y;
                g.z = a0.z * x2.x + h.z * x2.y;
                return 130.0 * dot(m, g);
            }
            
            float PerlinNoise(fixed2 p) {
                return snoise(p);
            }
            
            fixed3 HSVToRGB(fixed3 c) {
                fixed4 K = fixed4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                fixed3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
            }

            fixed3 RGBToHSV(fixed3 c) {
                fixed4 K = fixed4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                fixed4 p = lerp(fixed4(c.bg, K.wz), fixed4(c.gb, K.xy), step(c.b, c.g));
                fixed4 q = lerp(fixed4(p.xyw, c.r), fixed4(c.r, p.yzx), step(p.x, c.r));
            
                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return fixed3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            float2x2 rotate2d(float _angle){
                return float2x2(cos(_angle),-sin(_angle),
                            sin(_angle),cos(_angle));
            }

            fixed2 pixelateUV(fixed2 originalUV) {
                fixed2 UV = originalUV;
                UV *= _Pixelate;
                UV = ceil(UV);
                UV /= _Pixelate;
                return UV;
            }

            fixed2 swirlUV(fixed2 UV, float force) {
                float effectRadius = .5;
                float effectAngle = force * 3.14;
                
                UV -= 0.5;
                float len = length(UV);
                float angle = atan2(UV.y, UV.x) + effectAngle * smoothstep(effectRadius, 0., len);
                float radius = length(UV);
            
                return fixed2(radius * cos(angle), radius * sin(angle)) + fixed2(0.5, 0.5);
            }

            fixed4 bgCircle(fixed2 UV) {
                fixed2 noiseUV = UV;
                noiseUV = pixelateUV(noiseUV);
                noiseUV -= fixed2(0.5, 0.5);
                noiseUV = mul(noiseUV, rotate2d(_Time.y*_Speed.x*3.14));
                noiseUV += fixed2(0.5, 0.5);
                
                noiseUV = swirlUV(noiseUV, _SwirlForce.x);
                
                float noise = PerlinNoise(noiseUV * _NoiseSize.x);
                noise *= _DistorionForce.x;
             
                UV = pixelateUV(UV);
                UV += noise;
             
                fixed4 color = fixed4(1, 1, 1, 1);
             
                float distanceToCenter = distance(fixed2(0.5, 0.5), UV); // black atCenter
                float stepCircle = 1.0 - step(0.4, distanceToCenter);
             
                color = fixed4(stepCircle, stepCircle, stepCircle, stepCircle);
                color *= _MainColor;
             
                return color;
            }

            fixed4 fgPortal(fixed2 UV) {
                fixed4 color = fixed4(1, 1, 1, 1);
             
                // Make drops
                fixed2 swirlUVmap = UV;
                swirlUVmap = pixelateUV(swirlUVmap);
                swirlUVmap -= fixed2(0.5, 0.5);
                swirlUVmap = mul(swirlUVmap, rotate2d(_Time.y*_Speed.y*3.14));
                swirlUVmap += fixed2(0.5, 0.5);
                
                swirlUVmap = swirlUV(swirlUVmap, _SwirlForce.y);
                float swirls = PerlinNoise(swirlUVmap * _NoiseSize.y);
                swirls *= _DistorionForce.y;
                swirls = step(0.1, swirls);
                
                // Make circle
                fixed2 circleUV = pixelateUV(UV);
                float distanceToCenter = distance(fixed2(0.5, 0.5), circleUV); // black atCenter
                float stepCircle = 1.0 - step(0.5, distanceToCenter);
             
                swirls *= stepCircle;

                color = fixed4(swirls, swirls, swirls, swirls);
                
                fixed3 darkenColor = RGBToHSV(_MainColor.rgb);
                darkenColor.b -= 0.5;
                darkenColor = HSVToRGB(darkenColor);
                
                color *= fixed4(darkenColor, _MainColor.a);
             
                return color;
            }

            fixed4 dots(fixed2 UV) {
                fixed4 color = fixed4(1, 1, 1, 1);
             
                // Make drops
                fixed2 dropsUV = UV;
                dropsUV = pixelateUV(dropsUV);
                dropsUV -= fixed2(0.5, 0.5);
                dropsUV = mul(dropsUV, rotate2d(_Time.y*_Speed.z*3.14));
                dropsUV += fixed2(0.5, 0.5);
                
                dropsUV = swirlUV(dropsUV, _SwirlForce.z);
                
                float drops = PerlinNoise(dropsUV * _NoiseSize.z);
                drops *= _DistorionForce.z;
                drops = step(0.4, drops);
                
                // Make Circle
                fixed2 circleUV = pixelateUV(UV);
                float distanceToCenter = distance(fixed2(0.5, 0.5), circleUV); // black atCenter
                float stepCircle = 1.0 - step(0.55, distanceToCenter);
                float stepCircleIn = step(0.2, distanceToCenter);
                float ringCircle = stepCircle * stepCircleIn;
             
                drops *= ringCircle;
            
                color = fixed4(drops, drops, drops, drops);
                
                fixed3 lightenColor = RGBToHSV(_MainColor.rgb);
                lightenColor.g -= 0.8;
                lightenColor = HSVToRGB(lightenColor);
                
                color *= fixed4(lightenColor, _MainColor.a);
             
                return color;
            }

            fixed4 over(fixed4 first, fixed4 second) {
                return lerp(second, first, first.a);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = bgCircle(i.uv.xy);
                col = over(fgPortal(i.uv.xy), col);
                col = over(dots(i.uv.xy), col);
                
                return col;
            }
            
            ENDCG
        }
    }
}
