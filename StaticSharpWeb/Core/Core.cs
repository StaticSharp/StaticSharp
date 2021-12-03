using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharpWeb {
     
    public class InvalidUsageException : Exception {
        public ICallerInfo CallerInfo { get; }

        public InvalidUsageException(ICallerInfo callerInfo) => CallerInfo = callerInfo;
    }


    public interface ILongHashProvider {

        string GetLongHash();
    }





}