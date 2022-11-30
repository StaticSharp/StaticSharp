//using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharp {

    public class AbsoluteUrl {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public FilePath Path { get; }

        public AbsoluteUrl(AbsoluteUrl other) {
            Scheme = other.Scheme;
            Host = other.Host;
            Path = new(other.Path);
        }

        public AbsoluteUrl(string scheme, string host, IEnumerable<string> path) {
            Scheme = scheme;
            Host = host;
            Path = new(path);
        }
        public AbsoluteUrl(string scheme, string host, params string[] path) : this(scheme, host, path.AsEnumerable()) { }

        public override string ToString() {
            return $"{Scheme}://{Host}{string.Concat(Path.Select(x => "/" + x))}";
        }
        public static AbsoluteUrl operator +(AbsoluteUrl a, string b) {
            var result = new AbsoluteUrl(a.Scheme,a.Host,a.Path + b);
            return result;
        }
        public static AbsoluteUrl operator +(AbsoluteUrl a, IEnumerable<string> b) {
            var result = new AbsoluteUrl(a.Scheme, a.Host, a.Path + b);
            return result;
        }


        public AbsoluteUrl To(params string[] path) {
            return To(path.AsEnumerable());
        }
        public AbsoluteUrl To(IEnumerable<string> path) {
            return new AbsoluteUrl(Scheme,Host,Path.To(path));
        }

        


    }
}