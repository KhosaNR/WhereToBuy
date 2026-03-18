using System.ComponentModel;

namespace API.Helpers.Enums
{
    public enum ProductType
    {
        [Description("Single Unit")]
        Unit,

        [Description("Pack of Products")]
        Pack,

        // [Description("Mixed Variety Pack")]
        // MixedPack
    }
}
