using System;
using System.Linq;
using AvalonAssets.Utility;

namespace AvalonAssets.DataStructure.Geometry
{
    public class AngleInterval
    {
        private readonly double _from, _to;
        private readonly bool _full;

        public AngleInterval(double from, double to)
        {
            _from = DoubleUtils.NormalizeAngle(from);
            _to = DoubleUtils.NormalizeAngle(to);
            _full = false;
        }

        public AngleInterval(bool full)
        {
            _from = 0;
            _to = 0;
            _full = full;
        }

        public bool Inside(double target)
        {
            if (_full)
                return true;
            if (_from < _to)
                return _from.AboutLesserThanOrEqual(target) && target.AboutLesserThanOrEqual(_to);
            return _from.AboutLesserThanOrEqual(target) || target.AboutLesserThanOrEqual(_to);
        }

        public bool TryMerge(AngleInterval newShadow, out AngleInterval resultShadow)
        {
            if (newShadow._full)
            {
                resultShadow = newShadow;
                return true;
            }

            if (_full || Equals(newShadow))
            {
                resultShadow = this;
                return true;
            }

            var aStart = _from;
            var aEnd = aStart > _to ? _to + 360 : _to;
            var bStart = newShadow._from;
            var bEnd = bStart > newShadow._to ? newShadow._to + 360 : newShadow._to;

            var diffA = (aEnd - aStart)/2;
            var diffB = (bEnd - bStart)/2;
            var avgA = (aStart + aEnd)/2;
            var avgB = (bStart + bEnd)/2;
            var cosDiffA = Math.Cos(diffA.ToRadians());
            var cosDiffB = Math.Cos(diffB.ToRadians());

            var resultFlag = MergeFlag.None;

            if (Math.Cos((avgA - bStart).ToRadians()).AboutGreaterThanOrEqual(cosDiffA))
                resultFlag |= MergeFlag.BStartInsideA;
            if (Math.Cos((avgA - bEnd).ToRadians()).AboutGreaterThanOrEqual(cosDiffA))
                resultFlag |= MergeFlag.BEndInsideA;
            if (Math.Cos((avgB - aStart).ToRadians()).AboutGreaterThanOrEqual(cosDiffB))
                resultFlag |= MergeFlag.AStartInsideB;
            if (Math.Cos((avgB - aEnd).ToRadians()).AboutGreaterThanOrEqual(cosDiffB))
                resultFlag |= MergeFlag.AEndInsideB;


            if (NotHasFlags(resultFlag, MergeFlag.BStartInsideA, MergeFlag.BEndInsideA,
                MergeFlag.AEndInsideB, MergeFlag.AStartInsideB))
            {
                resultShadow = null;
                return false;
            }

            if (HasFlags(resultFlag, MergeFlag.BStartInsideA, MergeFlag.BEndInsideA,
                MergeFlag.AEndInsideB, MergeFlag.AStartInsideB))
            {
                resultShadow = new AngleInterval(true);
                return true;
            }

            if (HasFlags(resultFlag, MergeFlag.BStartInsideA, MergeFlag.BEndInsideA))
            {
                resultShadow = this;
                return true;
            }

            if (HasFlags(resultFlag, MergeFlag.AEndInsideB, MergeFlag.AStartInsideB))
            {
                resultShadow = newShadow;
                return true;
            }

            if (HasFlags(resultFlag, MergeFlag.AEndInsideB, MergeFlag.BStartInsideA) &&
                NotHasFlags(resultFlag, MergeFlag.AStartInsideB, MergeFlag.BEndInsideA))
            {
                resultShadow = new AngleInterval(aStart, bEnd);
                return true;
            }

            // Invert if
            if (!HasFlags(resultFlag, MergeFlag.AStartInsideB, MergeFlag.BEndInsideA) ||
                !NotHasFlags(resultFlag, MergeFlag.AEndInsideB, MergeFlag.BStartInsideA))
                throw new InvalidOperationException("This should never happen.");

            resultShadow = new AngleInterval(bStart, aEnd);
            return true;
        }

        public override bool Equals(object obj)
        {
            var shadow = obj as AngleInterval;
            return shadow != null && Equals(shadow);
        }

        public bool Equals(AngleInterval shadow)
        {
            return _from.AboutEqual(shadow._from) && _to.AboutEqual(shadow._to) && _full == shadow._full;
        }

        public override string ToString()
        {
            return string.Format("From: {0}, To: {1}, Full: {2}", _from, _to, _full);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_from.GetHashCode()*379 + _to.GetHashCode())*477 ^ _full.GetHashCode();
            }
        }

        private static bool HasFlags(MergeFlag resultFlag, params MergeFlag[] flags)
        {
            return flags.All(flag => (resultFlag & flag) == flag);
        }

        private static bool NotHasFlags(MergeFlag resultFlag, params MergeFlag[] flags)
        {
            return flags.All(flag => (resultFlag & flag) != flag);
        }


        [Flags]
        private enum MergeFlag : short
        {
            None = 0,
            BStartInsideA = 1 << 0,
            BEndInsideA = 1 << 1,
            AStartInsideB = 1 << 2,
            AEndInsideB = 1 << 3
        }
    }
}