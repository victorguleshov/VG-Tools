using UnityEngine;

namespace VG
{
    public static class ColorString
    {
        public static string Paint(Color color, string header, string message)
        {
#if UNITY_EDITOR
            string colorString;

            if (color == Color.black)
                colorString = "black";
            else if (color == Color.green)
                colorString = "green";
            else if (color == Color.gray)
                colorString = "gray";
            else if (color == Color.white)
                colorString = "white";
            else if (color == Color.blue)
                colorString = "blue";
            else if (color == Color.yellow)
                colorString = "yellow";
            else if (color == Color.magenta)
                colorString = "magenta";
            else if (color == Color.red)
                colorString = "red";
            else if (color == Color.cyan)
                colorString = "cyan";
            else
                colorString = "white";

            var result = string.Format("<color={0}> {1} : </color> {2}", colorString, header, message);

#else
            string result = string.Format("{0} : {1}", header, message);
#endif
            return result;
        }
    }
}