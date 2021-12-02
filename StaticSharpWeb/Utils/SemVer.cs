using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#pragma warning disable IDE1006 // Do not warn about naming style violations

namespace StaticSharpWeb {
    public struct SemVer : IComparable<SemVer> {
        public uint major { get; private set; }
        public uint minor { get; private set; }
        public uint patch { get; private set; }
        private IEnumerable<string> _labels;
        public IEnumerable<string> labels {
            get => _labels;
            set {
                checkLabelsMeta(value);
                _labels = value;
            }
        }
        public string labelsString {
            get => (_labels == null) ? "" : string.Join('.', _labels);
            set => labels = value.Split('.');
        }
        private IEnumerable<string> _meta;
        public IEnumerable<string> meta {
            get => _meta;
            set {
                checkLabelsMeta(value);
                _meta = value;
            }
        }
        public string metaString {
            get => (_meta == null) ? "" : string.Join('.', _meta);
            set => meta = string.IsNullOrEmpty(value) ? null : value.Split('.');
        }


        static readonly Regex checkLabelsMetaRegex = new Regex("[0-9a-zA-Z-]+$");

        private void checkLabelsMeta(IEnumerable<string> value) {
            if (value == null) return;
            var invalidValue = value.SingleOrDefault(x => !checkLabelsMetaRegex.IsMatch(x));
            if (invalidValue != null) {
                throw new ArgumentException($"Element {invalidValue} is invalid.");
            }
        }

        public static implicit operator SemVer(string x) => new(x);
        public static implicit operator string(SemVer sv) => sv.ToString();


        public SemVer(string x) {
            _labels = null;
            _meta = null;
            try {
                var r = new Regex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$");
                var m = r.Match(x);
                major = Convert.ToUInt32(m.Groups[1].Value);
                minor = Convert.ToUInt32(m.Groups[2].Value);
                patch = Convert.ToUInt32(m.Groups[3].Value);
                if (m.Groups[4].Success) labelsString = m.Groups[4].Value;
                if (m.Groups[5].Success) metaString = m.Groups[5].Value;
            } catch (Exception e) {
                throw new ArgumentException($"SemVer \"{x}\" is not valid.", e);
            }
        }

        public SemVer noMeta() {
            SemVer result = this;
            result.meta = null;
            return result;
        }

        private int CompareLabels(SemVer other) {
            if (labels != null) {
                if (other.labels != null) {
                    var thisBlocks = labels.ToArray();
                    var otherBlocks = other.labels.ToArray();
                    var compareBlocksCount = Math.Min(thisBlocks.Length, otherBlocks.Length);

                    for (int i = 0; i < compareBlocksCount; ++i) {
                        var thisBlock = thisBlocks[i];
                        var otherBlock = otherBlocks[i];
                        bool thisBlockIsNumeric = uint.TryParse(thisBlock, out uint thisBlockNumeric);
                        bool otherBlockIsNumeric = uint.TryParse(otherBlock, out uint otherBlockNumeric);

                        if (thisBlockIsNumeric) {
                            if (otherBlockIsNumeric) {
                                int val = thisBlockNumeric.CompareTo(otherBlockNumeric);
                                if (val != 0) {
                                    return val;
                                }
                            } else {
                                return -1;
                            }
                        } else {
                            if (otherBlockIsNumeric) {
                                return 1;
                            } else {
                                int val = thisBlock.CompareTo(otherBlock);
                                if (val != 0) {
                                    return val;
                                }
                            }
                        }
                    }

                    if (thisBlocks.Length > otherBlocks.Length) {
                        return 1;
                    }

                    if (thisBlocks.Length < otherBlocks.Length) {
                        return -1;
                    }

                    return 0;
                } else {
                    return -1;
                }

            } else {
                if (other.labels != null) {
                    return 1;
                } else {
                    return 0;
                }
            }
        }

        public int CompareTo(SemVer other) {
            if (major > other.major) {
                return 1;
            }
            if (major < other.major) {
                return -1;
            }

            if (minor > other.minor) {
                return 1;
            }
            if (minor < other.minor) {
                return -1;
            }

            if (patch > other.patch) {
                return 1;
            }
            if (patch < other.patch) {
                return -1;
            }

            return CompareLabels(other);
        }

        public static bool operator <(SemVer l, SemVer r) => l.CompareTo(r) < 0;

        public static bool operator >(SemVer l, SemVer r) => l.CompareTo(r) > 0;

        public static bool operator ==(SemVer l, SemVer r) => l.CompareTo(r) == 0;

        public static bool operator !=(SemVer l, SemVer r) => l.CompareTo(r) != 0;

        public override bool Equals(Object obj) =>
            //Check for null and compare run-time types.
            obj != null && GetType().Equals(obj.GetType()) && this == (SemVer)obj;

        public override int GetHashCode() {
            var result = major.GetHashCode() ^ minor.GetHashCode() ^ patch.GetHashCode();
            result ^= labelsString.GetHashCode();
            result ^= metaString.GetHashCode();
            return result;
        }

        public override string ToString() {
            var result = new StringBuilder($"{major}.{minor}.{patch}");
            if (_labels != null) result.Append('-').Append(labelsString);
            if (_meta != null) result.Append('+').Append(metaString);
            return result.ToString();
        }
    }

}