using YamlDotNet.RepresentationModel;

namespace Logicality.GitHub.Actions.Workflow;

public class WorkflowDefaults : WorkflowKeyValueMap<WorkflowDefaults>
{
    private string? _shell;
    private string? _workingDirectory;

    public WorkflowDefaults(Workflow workflow) : base(workflow, "defaults")
    {
    }

    public WorkflowDefaults(Workflow workflow, IDictionary<string, string> properties)
        : base(workflow, "defaults", properties)
    {
    }

    public WorkflowDefaults Run(string shell, string workingDirectory)
    {
        _shell            = shell;
        _workingDirectory = workingDirectory;
        return this;
    }

    internal override void Build(YamlMappingNode yamlMappingNode)
    {
        if (Properties.Any() || !string.IsNullOrWhiteSpace(_shell))
        {
            var defaultsMappingNode = new YamlMappingNode();
            foreach (var property in Properties)
            {
                defaultsMappingNode.Add(property.Key, new YamlScalarNode(property.Value));
            }
            // Defaults Run
            if (!string.IsNullOrWhiteSpace(_shell) && !string.IsNullOrWhiteSpace(_workingDirectory))
            {
                var defaultsRunMappingNode = new YamlMappingNode
                {
                    { "shell", _shell },
                    { "working-directory", _workingDirectory }
                };
                defaultsMappingNode.Add("run", defaultsRunMappingNode);
            }
            yamlMappingNode.Add("defaults", defaultsMappingNode);
        }
    }
}