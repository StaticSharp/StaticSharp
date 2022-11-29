using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace StaticSharp {

    public class FilePathEqualityComparer : IEqualityComparer<FilePath> {
        public bool Equals(FilePath? x, FilePath? y) {
            return x == y;
        }

        public int GetHashCode([DisallowNull] FilePath obj) {
            return obj.GetHashCode();
        }
    }

    public class FilePath: IEnumerable<string>, IEquatable<FilePath> {
        public string[] Items { get; init; }

        public FilePath(params string[] items) {
            Items = items;
        }
        public FilePath(IEnumerable<string> items) {
            Items = items.ToArray();
        }

        public static implicit operator bool(FilePath? value) {
            if (value==null)
                return false;

            if (value.Items==null)
                return false;
            return value.Items.Length > 0; 
        }

        public static FilePath operator + (FilePath a, IEnumerable<string> b) {
            FilePath result;
            if (a.Items == null) {
                result = new FilePath(b);
            } else {
                result = new FilePath(a.Items.Concat(b));
            }
            
            return result;
        }
        public static FilePath operator + (FilePath a, string b) {
            FilePath result;
            if (a.Items == null) {
                result = new FilePath(b);
            } else {
                result = new FilePath(a.Items.Append(b));
            }
            return result;
        }

        public IEnumerator<string> GetEnumerator() {
            return ((IEnumerable<string>)Items).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        

        public FilePath To(params string[] path) {
            return To(path.AsEnumerable());
        }
        public FilePath To(IEnumerable<string> path) {
            int commonPathElements = 0;
            bool common = true;
            List<string> result = new();
            foreach (var i in path) {
                if (common) {
                    if (commonPathElements >= Items.Length) {
                        common = false;
                    } else {
                        if (i == Items[commonPathElements]) {
                            commonPathElements++;
                        } else {
                            common = false;
                            for (int j = 0; j < Items.Length - commonPathElements; j++) {
                                result.Add("..");
                            }
                        }
                    }
                }
                if (!common) {
                    result.Add(i);
                }
            }
            return new FilePath(result);
        }

        public bool Equals(FilePath? other) {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (other.Items.Length != Items.Length) return false;
            for (int i = 0; i < Items.Length; i++) { 
                if (!Items[i].Equals(other.Items[i])) return false;
            }
            return true;
        }
        public override bool Equals(object? obj) {
            return Equals(obj as FilePath);
        }

        public FilePath WithoutLast {
            get { 
                return new FilePath(Items[0..^1]);
            }
        }


        public static bool operator ==(FilePath? a, FilePath? b) {
            if (a is null && b is null)
                return true;
            if (a is null || b is null)
                return false;
            return a.Equals(b);
        }
        public static bool operator !=(FilePath? a, FilePath? b) {
            return !(a == b);
        }

        public static FilePath FromOsPath(string osPath) {
            var parts = osPath.Split('\\', '/');
            return new FilePath(parts.Where(x=>!string.IsNullOrEmpty(x)));
        }

        public override string ToString() {
            return string.Join("/", this);
        }
        public string OsPath{
            get {
                return string.Join(System.IO.Path.DirectorySeparatorChar, this);
            }
        }

        public override int GetHashCode() {
            var hash = new HashCode();
            foreach (var i in Items) {
                hash.Add(i);
            }
            return hash.ToHashCode();
        }
    }
}