using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = false,
    OnPullRequestBranches = new[] { MasterBranch, ReleaseBranch },
    OnPushBranches = new[] { MasterBranch, ReleaseBranch },
    PublishArtifacts = false,
    InvokedTargets = new[] { nameof(Cover), nameof(Pack) },
    ImportGitHubTokenAs = nameof(GitHubToken))]
[GitHubActions(
    "release",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = false,
    OnPushTags = new[] { "v*" },
    PublishArtifacts = true,
    InvokedTargets = new[] { nameof(Cover), nameof(Publish) },
    ImportGitHubTokenAs = nameof(GitHubToken),
    ImportSecrets = new[] { nameof(NuGetApiKey) })]
partial class Build
{
    [CI] readonly GitHubActions GitHubActions;

    [Parameter("GitHub auth token", Name = "github-token"), Secret]
    readonly string GitHubToken;
}
