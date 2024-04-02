using System.Reflection;

namespace EManagementVSA.Shared.Helper;

public class EmailTemplateLoader
{
    public static string LoadTemplate(string resourceName, string initialPassword)
    {
        // Get the assembly where the resource is embedded
        Assembly assembly = Assembly.GetExecutingAssembly();

        // Get the full name of the resource
        string fullResourceName = assembly.GetManifestResourceNames()
                                           .FirstOrDefault(name => name.EndsWith(resourceName));

        if (fullResourceName == null)
        {
            throw new Exception($"Resource '{resourceName}' not found.");
        }

        // Read the embedded resource as a stream
        using (Stream stream = assembly.GetManifestResourceStream(fullResourceName))
        {
            if (stream == null)
            {
                throw new Exception($"Failed to load resource '{resourceName}'.");
            }

            // Read the stream content into a string
            using (StreamReader reader = new StreamReader(stream))
            {
                // Read the HTML template content
                string template = reader.ReadToEnd();

                // Replace placeholders in the template with actual values
                template = template.Replace("{INITIAL_PASSWORD}", initialPassword);
                // Add more replacements as needed

                return template;
            }
        }
    }
}