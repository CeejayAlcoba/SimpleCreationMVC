using System.Text.RegularExpressions;

namespace SimpleCreation.Services
{
    public class TextService
    {
        public string ToCamelCase(string name)
        {
            return String.Format("{0}{1}", name.First().ToString().ToLowerInvariant(), name.Substring(1));
        }
        public string ToKebabCase(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            return Regex.Replace(name, @"([a-z0-9])([A-Z])", "$1-$2").ToLower();
        }
    }
}
