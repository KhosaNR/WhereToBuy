namespace API.Helpers.Enums
{
    using System.ComponentModel;

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
