using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using Svg;

namespace Inker.Utilities
{
    public static class StrokeCollectionExtensions
    {
        private const string STROKE_TAG = "MSCANVASSTROKEDATA";

        public static string ToSvg(this StrokeCollection collection, bool embedRawData)
        {
            SvgDocument svgDocument = new SvgDocument();
            SvgGroup svgGroup = new SvgGroup();
            svgDocument.Children.Add(svgGroup);

            foreach (var stroke in collection.OrderBy(x => !x.DrawingAttributes.IsHighlighter))
            {
                PathGeometry geometry = stroke.GetGeometry(stroke.DrawingAttributes).GetOutlinedPathGeometry();
                string xamlPath = XamlWriter.Save(geometry); // Can we skip this step?
                if (!String.IsNullOrEmpty(xamlPath))
                {
                    string strokePathData = XElement.Parse(xamlPath).Attribute("Figures")?.Value;
                    if (!String.IsNullOrEmpty(strokePathData))
                    {
                        var inkColor = stroke.DrawingAttributes.Color;
                        if (stroke.DrawingAttributes.IsHighlighter)
                        {
                            inkColor.A = 255 / 2;
                        }
                        svgGroup.Children.Add(new SvgPath
                        {
                            PathData = SvgPathBuilder.Parse(strokePathData),
                            Fill = new SvgColourServer(System.Drawing.Color.FromArgb(inkColor.A, inkColor.R, inkColor.G, inkColor.B)),
                            Opacity = stroke.DrawingAttributes.IsHighlighter ? inkColor.A / 255.0f : 1.0f
                        });
                    }
                }
            }

            string svgXml = svgDocument.GetXML();

            if (!embedRawData)
            {
                return svgXml;
            }

            string metadata = string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                collection.Save(ms, true);
                ms.Seek(0, SeekOrigin.Begin);
                metadata = Convert.ToBase64String(ms.ToArray());
            }

            svgXml += $"{Environment.NewLine}<!-- {STROKE_TAG} \"{metadata}\"-->";
            return svgXml;
        }

        public static void LoadFromSvgFileMetadata(this StrokeCollection collection, string filePath)
        {
            XmlReaderSettings readerSettings = new XmlReaderSettings
            {
                IgnoreComments = false,
                DtdProcessing = DtdProcessing.Ignore,
                XmlResolver = null
            };

            using (XmlReader reader = XmlReader.Create(filePath, readerSettings))
            {
                var doc = XDocument.Load(reader);
                foreach (var node in doc.Nodes())
                {
                    if (node is XComment)
                    {
                        string commentTag = ((XComment)node).Value;
                        if (commentTag.Contains(STROKE_TAG))
                        {
                            byte[] savedStrokeCollection = Convert.FromBase64String(commentTag.Split('"')[1]);
                            using (MemoryStream ms = new MemoryStream(savedStrokeCollection))
                            {
                                ms.Seek(0, SeekOrigin.Begin);
                                StrokeCollection strokes = new StrokeCollection(ms);
                                collection.Clear();
                                collection.Add(strokes);
                                return;
                            }
                        }
                    }
                }
            }

            throw new Exception("No stroke metadata found.");
        }
    }
}
