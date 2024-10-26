namespace Common
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class TranslationAttribute : Attribute
    {
        public string VietnameseTranslation { get; }
        public TranslationAttribute(string vietnameseTranslation)
        {
            VietnameseTranslation = vietnameseTranslation;
        }
    }
}
