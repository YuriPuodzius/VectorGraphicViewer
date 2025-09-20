namespace Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Sources;

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using Wscad.VectorGraphicViewer.Domain.Contracts.Repositories;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Options;
using Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Dtos;
using Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Mappers;

public class PrimitivesJsonSource : IPrimitivesDataSource
{
    private readonly PrimitivesJsonOptions _opt;

    public PrimitivesJsonSource(IOptions<PrimitivesJsonOptions> options)
    {
        _opt = options.Value;
    }

    private IReadOnlyList<Primitive> ReadAll()
    {
        // Serializer configuration according to options
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture,
            ContractResolver = _opt.CaseInsensitive
                ? new DefaultContractResolver { NamingStrategy = new DefaultNamingStrategy() }
                : new DefaultContractResolver()
        };

        JsonSerializer serializer = JsonSerializer.Create(settings);

        using StreamReader sr = new StreamReader(Path.Combine(AppContext.BaseDirectory, _opt.PrimitivesPath));
        using JsonTextReader reader = new JsonTextReader(sr);

        // Configure how comments are handled
        JsonLoadSettings loadSettings = new JsonLoadSettings
        {
            CommentHandling = _opt.AllowComments ? CommentHandling.Load : CommentHandling.Ignore
        };

        JToken root = JToken.ReadFrom(reader, loadSettings);

        // Deserialize into DTOs
        List<PrimitiveJsonDto> dtos = root.ToObject<List<PrimitiveJsonDto>>(serializer) ?? new List<PrimitiveJsonDto>();

        // Map DTOs -> Domain
        List<Primitive> list = dtos.Select(dto => dto.ToDomain()).ToList();

        return list;
    }

    public IReadOnlyList<Primitive> GetAll()
    {
        return ReadAll();
    }

    public Primitive? GetByType(PrimitiveTypeEnum type)
    {
        return ReadAll().FirstOrDefault(p => p.Kind == type);
    }
}
