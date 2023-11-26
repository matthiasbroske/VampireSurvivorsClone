using UnityEngine;

namespace Vampire
{
    public class EasingUtils
    {
        public static float EaseInBack(float x, float c = 1.70158f)
        {
            float c3 = c + 1;
            return c3 * x * x * x - c * x * x;
        }

        public static float EaseOutBack(float x, float c = 1.70158f)
        {
            float c3 = c + 1;
            return 1 + c3 * Mathf.Pow(x - 1, 3) + c * Mathf.Pow(x - 1, 2);
        }

        public static float Arc(float x, float c = 1)
        {
            return -4*c*Mathf.Pow(x-0.5f,2)+c;
        }

        public static float Bounce(float x, float a = 1)
        {
            float b = 0.4f*a;
            float c = 0.1f*a;
            float d = 0.02f*a;
            if (x < 0.5f)
                return -16*a*Mathf.Pow(x-0.25f,2)+a;
            else if (x < 0.75f)
                return -64*b*Mathf.Pow(x-0.625f,2)+b;
            else if (x < 0.875f)
                return -256*c*Mathf.Pow(x-0.8125f,2)+c;
            else
                return -256*d*Mathf.Pow(x-0.9375f,2)+d;
        }

        public static float EaseInQuad(float x)
        {
            return x * x;
        }

        public static float EaseOutQuad(float x)
        {
            return 1 - Mathf.Pow(1-x, 2);
        }

        public static float EaseInQuart(float x)
        {
            return x * x * x * x;
        }

        public static float EaseOutQuart(float x)
        {
            return 1 - Mathf.Pow(1-x, 4);
        }

        public static float EaseIn6(float x)
        {
            return x * x * x * x * x * x;
        }

        public static float EaseOut6(float x)
        {
            return 1 - Mathf.Pow(1-x, 6);
        }
    }
}
