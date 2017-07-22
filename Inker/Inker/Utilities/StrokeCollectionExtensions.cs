using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Linq;
using Svg;

namespace Inker.Utilities
{
    public static class StrokeCollectionExtensions
    {
        public static string ToSvg(this StrokeCollection collection)
        {
            SvgDocument svgDocument = new SvgDocument();
            SvgGroup svgGroup = new SvgGroup();
            svgDocument.Children.Add(svgGroup);

            foreach (var stroke in collection)
            {
                PathGeometry geometry = stroke.GetGeometry(stroke.DrawingAttributes).GetOutlinedPathGeometry();
                string xamlPath = XamlWriter.Save(geometry); // Can we skip this step?
                if (!String.IsNullOrEmpty(xamlPath))
                {
                    string strokePathData = XElement.Parse(xamlPath).Attribute("Figures")?.Value;
                    if (!String.IsNullOrEmpty(strokePathData))
                    {
                        var inkColor = stroke.DrawingAttributes.Color;
                        svgGroup.Children.Add(new SvgPath
                        {
                            PathData = SvgPathBuilder.Parse(strokePathData),
                            Fill = new SvgColourServer(System.Drawing.Color.FromArgb(inkColor.A, inkColor.R, inkColor.G, inkColor.B))
                        });
                    }
                }
            }
            return svgDocument.GetXML();
        }
    }
}
