using Domain.Core.ValueObjects.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Core.PropertyConverters
{
    public static class CommonConverterExtension
    {
        public static readonly ValueConverter<Description, string>
            DescriptionConverter = new(v => v.Value, v => Description.Create(v));

        public static readonly ValueConverter<Img, string>
            ImgConverter = new(v => v.Value, v => Img.Create(v));

        public static readonly ValueConverter<Email, string>
            EmailConverter = new(v => v.Value, v => Email.Create(v));

        public static readonly ValueConverter<IpAddress, string>
            IpAddressConverter = new(v => v.Value, v => IpAddress.Create(v));

        public static readonly ValueConverter<Slug, string>
            SlugConverter = new(v => v.Value, v => Slug.Create(v));

    }
}