using System;

namespace VG.Extensions
{
    public static class AxesExtensions
    {
        public enum Axes
        {
            None,
            X,
            Y,
            Z,
            XY,
            XZ,
            YZ,
            XYZ
        }

        [Flags]
        public enum Axis
        {
            X = 1 << 0,
            Y = 1 << 1,
            Z = 1 << 2
        }

        public static Axes ToAxes(this Axis p_axis)
        {
            if (p_axis.HasFlag(Axis.X) &&
                p_axis.HasFlag(Axis.Y) &&
                p_axis.HasFlag(Axis.Z))
                return Axes.XYZ;

            if (p_axis.HasFlag(Axis.X) &&
                p_axis.HasFlag(Axis.Y))
                return Axes.XY;

            if (p_axis.HasFlag(Axis.X) &&
                p_axis.HasFlag(Axis.Z))
                return Axes.XZ;

            if (p_axis.HasFlag(Axis.Y) &&
                p_axis.HasFlag(Axis.Z))
                return Axes.YZ;

            return
                p_axis.HasFlag(Axis.X) ? Axes.X :
                p_axis.HasFlag(Axis.Y) ? Axes.Y :
                p_axis.HasFlag(Axis.Z) ? Axes.Z :
                Axes.None;
        }

        public static Axis ToAxis(this Axes p_axes)
        {
            Axis r_axis = 0;

            switch (p_axes)
            {
                case Axes.X:
                    r_axis |= Axis.X;
                    break;

                case Axes.Y:
                    r_axis |= Axis.Y;
                    break;

                case Axes.Z:
                    r_axis |= Axis.Z;
                    break;

                case Axes.XY:
                    r_axis |= Axis.X;
                    r_axis |= Axis.Y;
                    break;

                case Axes.XZ:
                    r_axis |= Axis.X;
                    r_axis |= Axis.Z;
                    break;

                case Axes.YZ:
                    r_axis |= Axis.Y;
                    r_axis |= Axis.Z;
                    break;

                case Axes.XYZ:
                    r_axis |= Axis.X;
                    r_axis |= Axis.Y;
                    r_axis |= Axis.Z;
                    break;
            }

            return r_axis;
        }
    }
}