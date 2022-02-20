using StaticSharpGears.Public;
using System.Drawing;

namespace StaticSharpGears;


public static partial class KeyCalculators {
    public static string GetKey(MaterialDesignIcons x) {
        return KeyUtils.Combine<MaterialDesignIcons>(x.ToString());
    }
}
/*
public class CacheableMaterialDesignIcon : Cacheable<CacheableMaterialDesignIcon.Constructor> {

    public record Constructor(Public.MaterialDesignIcons Name, Color Color) : Constructor<CacheableMaterialDesignIcon> {
        protected override CacheableMaterialDesignIcon Create() {
            return new CacheableMaterialDesignIcon(this);
        }
    }




    private CacheableHttpRequest httpRequest;

    public SecondaryTask<string> Data { get; init; } = new();

    private CacheableMaterialDesignIcon(Constructor arguments) : base(arguments) {}

    protected override async Task CreateAsync() {

        var httpRequest = new CacheableHttpRequest.Constructor(Arguments.Name.GetSvgUri()).CreateOrGetCached();

        try {
            var code = await httpRequest.ReadAllTextAsync();

            Code.SetResult(code);
        }
        catch (Exception ex) {
            Code_TaskCompletionSource.SetException(ex);
        }
    }
}*/



