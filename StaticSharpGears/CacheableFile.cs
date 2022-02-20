namespace StaticSharpGears;




public interface IMutableFile { 

}



public class CacheableFile : Cacheable<CacheableFile.Constructor> {

    public record Constructor(Uri Uri) : Constructor<CacheableFile> {
        protected override CacheableFile Create() {
            return new CacheableFile(this);
        }
    }

    private CacheableFile(Constructor arguments) : base(arguments) {}



    protected override Task CreateAsync() {
        if (Arguments.Uri.IsFile) { 
        
        }
        throw new NotImplementedException();
    }
}



