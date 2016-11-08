using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Utility;

namespace AvalonAssets.DataStructure.Graph
{
    public class ShadowCast
    {
        private readonly SortedList<double, Shadow> _shadowList;

        public ShadowCast()
        {
            _shadowList = new SortedList<double, Shadow>();
        }

        public void AddShadow(double minAngle, double maxAngle)
        {
            var shadow = new Shadow(minAngle, maxAngle);
            _shadowList.Add(minAngle, shadow);
            MergeShadow();
        }

        public bool Hide(double angle)
        {
            return _shadowList.Values.Any(shadow => shadow.Hide(angle));
        }

        private void MergeShadow()
        {
            bool merged;
            do
            {
                merged = false;
                for (var i = 0; i < _shadowList.Count; i++)
                {
                    if (i + 1 >= _shadowList.Count) continue;
                    Shadow shadow;
                    if (!_shadowList.Values[i].TryMerge(_shadowList.Values[i + 1], out shadow)) continue;
                    _shadowList[i] = shadow;
                    _shadowList.RemoveAt(i + 1);
                    merged = true;
                    break;
                }
            } while (merged);
        }

        private struct Shadow
        {
            private readonly double _minAngle;
            private readonly double _maxAngle;

            public Shadow(double minAngle, double maxAngle)
            {
                minAngle = DoubleUtils.NormalizeAngle(minAngle);
                maxAngle = DoubleUtils.NormalizeAngle(maxAngle);
                if (minAngle > maxAngle)
                {
                    _minAngle = maxAngle;
                    _maxAngle = minAngle;
                }
                else
                {
                    _minAngle = minAngle;
                    _maxAngle = maxAngle;
                }
            }

            public bool TryMerge(Shadow shadow, out Shadow result)
            {
                if (this >= shadow)
                {
                    result = this;
                    return true;
                }
                if (this < shadow)
                {
                    result = shadow;
                    return true;
                }
                if (_minAngle >= shadow._minAngle && _minAngle <= shadow._maxAngle)
                {
                    result = new Shadow(shadow._minAngle, _maxAngle);
                    return true;
                }
                if (_maxAngle >= shadow._minAngle && _maxAngle <= shadow._maxAngle)
                {
                    result = new Shadow(_minAngle, shadow._maxAngle);
                    return true;
                }
                result = this;
                return false;
            }

            public bool Hide(double angle)
            {
                angle = DoubleUtils.NormalizeAngle(angle);
                return _minAngle <= angle && angle <= _maxAngle;
            }

            public override bool Equals(object obj)
            {
                return obj is Shadow && Equals((Shadow) obj);
            }

            public override int GetHashCode()
            {
                return (_minAngle.GetHashCode()*397) ^ _maxAngle.GetHashCode();
            }

            private bool Equals(Shadow obj)
            {
                return _minAngle.AboutEqual(obj._minAngle) && _maxAngle.AboutEqual(obj._maxAngle);
            }

            public static bool operator >(Shadow left, Shadow right)
            {
                return left._maxAngle > right._maxAngle && left._minAngle < right._minAngle;
            }

            public static bool operator <(Shadow left, Shadow right)
            {
                return left._maxAngle < right._maxAngle && left._minAngle > right._minAngle;
            }

            public static bool operator >=(Shadow left, Shadow right)
            {
                return left > right || left == right;
            }

            public static bool operator <=(Shadow left, Shadow right)
            {
                return left < right || left == right;
            }

            public static bool operator ==(Shadow left, Shadow right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Shadow left, Shadow right)
            {
                return !left.Equals(right);
            }
        }
    }
}