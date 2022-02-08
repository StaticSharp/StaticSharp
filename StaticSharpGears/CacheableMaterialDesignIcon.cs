using StaticSharpGears.Public;

namespace StaticSharpGears;


public static partial class KeyCalculators {
    public static string GetKey(MaterialDesignIcons x) {
        return KeyUtils.Combine<MaterialDesignIcons>(x.ToString());
    }
}
public partial class CacheableMaterialDesignIcon : Cacheable<CacheableMaterialDesignIcon.Constructor> {

    public record Constructor(Public.MaterialDesignIcons Name) : Constructor<CacheableMaterialDesignIcon> {
        protected override CacheableMaterialDesignIcon Create() {
            return new CacheableMaterialDesignIcon(this);
        }
    }

    private CacheableHttpRequest httpRequest;

    private TaskCompletionSource<string> Code_TaskCompletionSource = new();
    public Task<string> Code => Code_TaskCompletionSource.Task;

    public CacheableMaterialDesignIcon(Constructor arguments) : base(arguments) {
        //var kebabName = CaseConvert.PascalToKebabCase(Arguments.Name.ToString());

        var uri = new Uri($"https://raw.githubusercontent.com/Templarian/MaterialDesign/master/svg/{arguments.Name.GetName()}.svg");
        httpRequest = new CacheableHttpRequest.Constructor(uri).CreateOrGetCached();
    }

    protected override async Task CreateAsync() {
        try {
            var code = await httpRequest.ReadAllTextAsync();

            Code_TaskCompletionSource.SetResult(code);
        }
        catch (Exception ex) {
            Code_TaskCompletionSource.SetException(ex);
        }
    }
}



