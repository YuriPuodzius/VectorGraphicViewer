using System.Xml.Serialization;

namespace Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Dtos
{
    /// <summary>
    /// DTO de uma primitive no XML.
    /// </summary>
    public sealed class PrimitiveXmlDto
    {
        // ---- type: pode vir como atributo OU elemento ----
        [XmlAttribute("type")]
        public string? TypeAttr { get; set; }

        [XmlElement("type")]
        public string? TypeElem { get; set; }

        [XmlIgnore]
        public string? Type => TypeAttr ?? TypeElem;

        // ---- elementos comuns ----
        [XmlElement("a")] public string? A { get; set; }
        [XmlElement("b")] public string? B { get; set; }
        [XmlElement("c")] public string? C { get; set; }
        [XmlElement("center")] public string? Center { get; set; }
        [XmlElement("radius")] public double? Radius { get; set; }
        [XmlElement("filled")] public bool? Filled { get; set; }
        [XmlElement("color")] public string? Color { get; set; }
        [XmlElement("isActive")] public bool? IsActive { get; set; }
    }

    /// <summary>
    /// Envelope: <primitives><primitive>...</primitive>...</primitives>
    /// </summary>
    [XmlRoot("primitives")]
    public sealed class PrimitivesEnvelopeDto
    {
        [XmlElement("primitive")]
        public List<PrimitiveXmlDto>? Items { get; set; }
    }
}