using System;
using System.Collections.Generic;
using System.Text;

namespace Ducker.Core
{
    /// <summary>
    /// Generates a markdown with Header Table of Contents.
    /// </summary>
    public class StandardWithTocMdDocGenerator : MarkDownDocGenerator
    {
        /// <summary>
        /// Creates the contents of the document based on components and the export settings
        /// </summary>
        /// <param name="plugin">The plugin details.</param>
        /// <param name="components">The components included in the gha.</param>
        /// <param name="settings">The output settings.</param>
        /// <returns>Content of the document.</returns>
        public override DocumentContent Create(DuckerPlugin plugin, List<DuckerComponent> components, ExportSettings settings)
        {
            DocumentContent docContent = new DocumentContent();
            StringBuilder builder = new StringBuilder();

            // Generate plugin header
            builder.AppendLine(Header(plugin.Name, 1));

            builder.AppendLine(Paragraph(Italic(plugin.Description)));
            builder.AppendLine(Paragraph(Bold(plugin.Copyright)));
            builder.AppendLine(Paragraph(Bold(plugin.Version)));
            
            builder.AppendLine(Paragraph(Divider()));
            
            // Generate components table of contents
            builder.AppendLine(Paragraph(Bold("Components:")));

            foreach (var component in components)
            {
                builder.AppendLine(($"- {HeaderLink(component.Name)}"));
            }
            builder.AppendLine(Paragraph(Divider()));

            // Generate each component
            foreach (var component in components)
            {
                if (component.Exposure == "hidden" && settings.IgnoreHidden)
                    continue;

                builder.AppendLine($"{Header(component.Name, 2)} {Image("",
                    docContent.RelativePathIcons, component.GetValidFileName())}");
                builder.Append(Paragraph(Bold(nameof(component.Name) + ":") + " " + component.Name));
                builder.Append(Paragraph(Bold(nameof(component.NickName) + ":") + " " + component.NickName));
                builder.Append(Paragraph(Bold(nameof(component.Description) + ":") + " " + component.Description));
                builder.Append(Environment.NewLine);

                if (component.Input.Count > 0)
                {
                    builder.AppendLine(Header(nameof(component.Input), 3));
                    string table = GenerateParamTable(component.Input);
                    builder.Append(table);
                }
                if (component.Output.Count > 0)
                {
                    builder.AppendLine(Header(nameof(component.Output), 3));
                    string table = GenerateParamTable(component.Output);
                    builder.Append(table);
                }
            }

            docContent.Document = builder.ToString();
            docContent.Icons = ReadIcons(components);
            return docContent;
        }
    }
}
