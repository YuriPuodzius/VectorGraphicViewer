using System.Xml.Serialization;

namespace Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Dtos;

/// <summary>
/// DTO for a single primitive entry in XML.
/// Attributes/elements match the XML workload structure.
/// </summary>
public sealed class PrimitiveXmlDto
{
    [XmlElement("type")]
    public string? Type { get; set; }

    [XmlElement("a")]
    public string? A { get; set; }

    [XmlElement("b")]
    public string? B { get; set; }

    [XmlElement("c")]
    public string? C { get; set; }

    [XmlElement("center")]
    public string? Center { get; set; }

    [XmlElement("radius")]
    public double? Radius { get; set; }

    [XmlElement("filled")]
    public bool? Filled { get; set; }

    [XmlElement("color")]
    public string? Color { get; set; }
}

/// <summary>
/// XML envelope:
/// <primitives><primitive>...</primitive> ...</primitives>
/// </summary>
[XmlRoot("primitives")]
public sealed class PrimitiveXmlEnvelope
{
    [XmlElement("primitive")]
    public List<PrimitiveXmlDto> Items { get; set; } = new();
}
