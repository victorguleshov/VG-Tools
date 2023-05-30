using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace VG.Extensions
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class VectorExtensions
    {
        /// <summary> Divides every component of this vector by the same component of scale. </summary>
        public static Vector3 Divide(this Vector3 a, Vector3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);

        /// <summary> Divides every component of this vector by the same component of scale. </summary>
        public static Vector2 Divide(this Vector2 a, Vector2 b) => new(a.x / b.x, a.y / b.y);

        /// <summary> Multiplies every component of this vector by the same component of scale. </summary>
        public static Vector3 Multiply(this Vector3 a, Vector3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);

        /// <summary> Multiplies every component of this vector by the same component of scale. </summary>
        public static Vector2 Multiply(this Vector2 a, Vector2 b) => new(a.x * b.x, a.y * b.y);

        public static Vector2 Abs(this Vector2 a) => new(Mathf.Abs(a.x), Mathf.Abs(a.y));

        public static Vector3 Abs(this Vector3 a) => new(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));

        /// <summary>
        ///     Преобразует направление из локального в мировое пространство;<br />
        ///     P.S. Работает так же, как transform.TransformDirection,
        ///     но применяется не к трансформу, а к направлению,
        ///     рассматриваемому, как относительный "верх";<br />
        ///     P.P.S. Результат вызовов<br />
        ///     transform.TransformDirection (direction) и<br />
        ///     transform.up.ConvertDirection2D (direction)<br />
        ///     будет одинаковым;
        /// </summary>
        public static Vector2 ConvertDirection2D(this Vector3 up, Vector2 direction) =>
            ConvertDirection2D((Vector2)up, direction);

        /// <summary>
        ///     Преобразует направление из локального в мировое пространство;<br />
        ///     P.S. Работает так же, как transform.TransformDirection,
        ///     но применяется не к трансформу, а к направлению,
        ///     рассматриваемому, как относительный "верх";<br />
        ///     P.P.S. Результат вызовов<br />
        ///     transform.TransformDirection (direction) и<br />
        ///     transform.up.ConvertDirection2D (direction)<br />
        ///     будет одинаковым;
        /// </summary>
        public static Vector2 ConvertDirection2D(this Vector2 up, Vector2 direction) =>
            direction.Rotate(Vector2.SignedAngle(Vector2.up, up));

        /// <summary>
        ///     Преобразует направление из мирового в локальное пространство;
        ///     Результат обратный методу ConvertDirection2D;<br />
        ///     P.S. Работает так же, как transform.InverseTransformDirection,
        ///     но применяется не к трансформу, а к направлению,
        ///     рассматриваемому, как относительный "верх";<br />
        ///     P.P.S. Результат вызовов<br />
        ///     transform.InverseTransformDirection (direction) и<br />
        ///     transform.up.InverseConvertDirection2D (direction)<br />
        ///     будет одинаковым;
        /// </summary>
        public static Vector2 InverseConvertDirection2D(this Vector3 up, Vector2 direction) =>
            InverseConvertDirection2D((Vector2)up, direction);

        /// <summary>
        ///     Преобразует направление из мирового в локальное пространство;
        ///     Результат обратный методу ConvertDirection2D;<br />
        ///     P.S. Работает так же, как transform.InverseTransformDirection,
        ///     но применяется не к трансформу, а к направлению,
        ///     рассматриваемому, как относительный "верх";<br />
        ///     P.P.S. Результат вызовов<br />
        ///     transform.InverseTransformDirection (direction) и<br />
        ///     transform.up.InverseConvertDirection2D (direction)<br />
        ///     будет одинаковым;
        /// </summary>
        public static Vector2 InverseConvertDirection2D(this Vector2 up, Vector2 direction)
        {
            var signedAngle = Vector2.SignedAngle(Vector2.up, direction) -
                              Vector2.SignedAngle(Vector2.up, up);

            if (Mathf.Abs(signedAngle) > 180.0f)
            {
                if (signedAngle < 0.0f)
                    signedAngle += 360.0f;
                else
                    signedAngle -= 360.0f;
            }

            return Vector2.up.Rotate(signedAngle);
        }

        /// <summary>
        ///     Возвращает новый direction, который равен результату поворота
        ///     <see cref="direction" /> на <see cref="signedAngle" /> градусов (с учетом знака);
        /// </summary>
        public static Vector2 Rotate(this Vector2 direction, float signedAngle)
        {
            var signedAngleInRadians = signedAngle * Mathf.Deg2Rad;

            return new Vector2(
                direction.x * Mathf.Cos(signedAngleInRadians) - direction.y * Mathf.Sin(signedAngleInRadians),
                direction.x * Mathf.Sin(signedAngleInRadians) + direction.y * Mathf.Cos(signedAngleInRadians)
            );
        }

        public static Vector2 PositionToScreenPoint(this Transform transform, Camera camera = null)
        {
            if (transform is RectTransform rectTransform)
            {
                if (camera == null ||
                    camera == false)
                {
                    var canvas = rectTransform.GetComponentInParent<Canvas>();

                    if (canvas)
                        camera = canvas.worldCamera;
                }

                return RectTransformUtility.WorldToScreenPoint(camera, rectTransform.position);
            }

            if (camera == null ||
                camera == false)
                camera = Camera.main;
            return camera ? camera.WorldToScreenPoint(transform.position) : Vector2.zero;
        }

        /// <summary>
        ///     Возвращает anchoredPosition для указанного <see cref="rectTransform" /> таким образом,<br />
        ///     чтобы transform.position последнего был равен указанному <see cref="worldPoint" />;
        /// </summary>
        public static Vector2 WorldPointToAnchorPos(this RectTransform rectTransform, Vector3 worldPoint,
            Camera camera = null)
        {
            if (rectTransform)
            {
                if (camera == null ||
                    camera == false)
                {
                    var canvas = rectTransform.GetComponentInParent<Canvas>();

                    if (canvas)
                        camera = canvas.worldCamera;
                }

                var screenPoint = RectTransformUtility.WorldToScreenPoint(camera, worldPoint);
                return rectTransform.ScreenPointToAnchorPos(screenPoint, camera);
            }

            return Vector2.zero;
        }

        /// <summary>
        ///     Возвращает anchoredPosition для указанного <see cref="rectTransform" /> таким образом,<br />
        ///     чтобы transform.position последнего был равен указанному <see cref="screenPoint" />;
        /// </summary>
        public static Vector2 ScreenPointToAnchorPos(this RectTransform rectTransform, Vector2 screenPoint,
            Camera camera = null)
        {
            if (rectTransform)
            {
                if (camera == null ||
                    camera == false)
                {
                    var canvas = rectTransform.GetComponentInParent<Canvas>();

                    if (canvas)
                        camera = canvas.worldCamera;
                }

                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, camera,
                    out var offset);
                return rectTransform.anchoredPosition + offset;
            }

            return Vector2.zero;
        }
    }
}