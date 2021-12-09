using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsLatest,
    OnPullRequestBranches = new[] { MasterBranch, DevelopBranch },
    OnPushBranches = new [] { MasterBranch },
    PublishArtifacts = false,
    InvokedTargets = new[] { nameof(Cover), nameof(Pack) },
    ImportGitHubTokenAs = nameof(GitHubToken))]
[GitHubActions(
    "release",
    GitHubActionsImage.WindowsLatest,
    OnPushTags = new[] { "v0.*", "v1.*" },
    PublishArtifacts = true,
    InvokedTargets = new[] { nameof(Cover), nameof(Publish) },
    ImportGitHubTokenAs = nameof(GitHubToken),
    ImportSecrets = new[] {nameof(NuGetApiKey)})]
partial class Build
{
    [CI] readonly GitHubActions GitHubActions;

    [Parameter("GitHub auth token", Name = "github-token"), Secret]
    readonly string GitHubToken;
}
