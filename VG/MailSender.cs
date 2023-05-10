using UnityEngine;
using UnityEngine.Networking;

namespace VG
{
    public static class MailSender
    {
        public static void OpenMailApp (string p_email, string p_subject = null, string p_body = null)
        {
            if (!string.IsNullOrEmpty (p_email))
            {
                p_subject = EscapeURL (p_subject);
                p_body = EscapeURL (p_body);

                Application.OpenURL ("mailto:" + p_email + "?subject=" + p_subject + "&body=" + p_body);
            }
        }

        private static string EscapeURL (string p_url)
        {
            if (!string.IsNullOrEmpty (p_url))
                return UnityWebRequest.EscapeURL (p_url).Replace ("+", "%20");
            else
                return string.Empty;
        }
    }
}