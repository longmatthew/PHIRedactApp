using System.Text.RegularExpressions;

namespace PHIRedactApp.Services;

public interface IRedactService
{
    public string RedactPhiAsync(string labOrderInput);
}

public class RedactService : IRedactService
{
    public string RedactPhiAsync(string labOrderInput)
    {
        if (string.IsNullOrWhiteSpace(labOrderInput))
        {
            return labOrderInput;
        }

        var patterns = new (string pattern, string replacement)[]
        {
            (@"(?<=Patient Name:\s)(.+)", "[REDACTED]"),                                // Names
            (@"\b(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/\d{4}\b", "[REDACTED]"),    // Dates (MM/DD/YYYY)
            (@"\b\d{3}-\d{2}-\d{4}\b", "[REDACTED]"),                                   // SSN
            (@"(?<=Address:\s)(.+)", "[REDACTED]"),                                     // Street Address
            (@"\(\d{3}\) \d{3}-\d{4}", "[REDACTED]"),                                   // Phone Numbers
            (@"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b", "[REDACTED]"),      // Email
            (@"MRN-\d+", "[REDACTED]")                                                  // Medical Record Numbers
        };

        string[] lines = labOrderInput.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            foreach (var (pattern, replacement) in patterns)
            {
                lines[i] = Regex.Replace(lines[i], pattern, replacement);
            }
        }

        return string.Join(Environment.NewLine, lines);
    }
}
