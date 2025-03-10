using MediatR;

namespace PHIRedactApp.Commands;

public class RedactLabOrderCommand(string labOrderText, string fileName) : IRequest<bool>
{
    public string LabOrderText { get; } = labOrderText;
    public string FileName { get; } = fileName;
}
