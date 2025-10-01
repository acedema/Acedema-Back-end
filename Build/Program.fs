module Program

open Fake.Core
open Fake.IO
open System.IO
open Fake.DotNet
open System
open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.Api

let findParent (fileName: string) (startDir: string) =
    let rec loop (dir: DirectoryInfo) =
        if isNull dir then
            None
        elif dir.EnumerateFiles(fileName) |> Seq.isEmpty |> not then
            Some dir
        else
            loop dir.Parent

    loop (DirectoryInfo startDir)

let path xs = Path.Combine(Array.ofList xs)

let solutionRoot = findParent "acedema-backcore.sln" __SOURCE_DIRECTORY__

let src = path [ solutionRoot.ToString(); "src" ]

// Remove all compiled stuff

let srcGlob = solutionRoot.Value.ToString() </> "*/*.csproj"

Trace.log srcGlob

let clean _ =

    !!srcGlob
    |> Seq.collect (fun p -> [ "bin"; "obj" ] |> Seq.map (fun sp -> IO.Path.GetDirectoryName p </> sp))
    |> Shell.cleanDirs

let disableBinLog (p: MSBuild.CliArguments) = { p with DisableInternalBinLog = true }

let configuration (targets: Target list) =
    let defaultVal = "Debug"

    match Environment.environVarOrDefault "CONFIGURATION" defaultVal with
    | "Debug" -> DotNet.BuildConfiguration.Debug
    | "Release" -> DotNet.BuildConfiguration.Release
    | config -> DotNet.BuildConfiguration.Custom config

let dotnetRestore _ =
    [ solutionRoot.Value.ToString() ]
    |> Seq.map (fun dir ->
        fun () ->
            let args = [] |> String.concat " "

            DotNet.restore
                (fun c ->
                    { c with
                        MSBuildParams = disableBinLog c.MSBuildParams
                        Common = c.Common |> DotNet.Options.withCustomParams (Some(args))
                    })
                dir)
    |> Seq.iter (fun fn -> fn ())

let dotnetBuild ctx =
    DotNet.build
        (fun c ->
            { c with
                MSBuildParams = disableBinLog c.MSBuildParams
                Configuration = configuration (ctx.Context.AllExecutingTargets)
                Common = c.Common

            })
        (solutionRoot.Value.ToString())

let initTargets () =
    // BuildServer.install [ GitHubActions.Installer ]

    /// Defines a dependency - y is dependent on x. Finishes the chain.
    let (==>!) x y = x ==> y |> ignore

    /// Defines a soft dependency. x must run before y, if it is present, but y does not require x to be run. Finishes the chain.
    let (?=>!) x y = x ?=> y |> ignore
    //-----------------------------------------------------------------------------
    // Hide Secrets in Logger
    //-----------------------------------------------------------------------------
    // Option.iter (TraceSecrets.register "<GITHUB_TOKEN>") githubToken
    // Option.iter (TraceSecrets.register "<NUGET_TOKEN>") nugetToken
    //-----------------------------------------------------------------------------
    // Target Declaration
    //-----------------------------------------------------------------------------

    Target.create "Clean" clean
    Target.create "DotnetRestore" dotnetRestore
    // Target.create "UpdateChangelog" updateChangelog
    // Target.createBuildFailure "RevertChangelog" revertChangelog // Do NOT put this in the dependency chain
    // Target.createFinal "DeleteChangelogBackupFile" deleteChangelogBackupFile // Do NOT put this in the dependency chain
    Target.create "DotnetBuild" dotnetBuild
    // Target.create "IntegrationTests" integrationTests
    // Target.create "DotnetPack" dotnetPack
    // Target.create "PublishToNuGet" publishToNuget
    // Target.create "GitRelease" gitRelease
    // Target.create "GitHubRelease" githubRelease
    // Target.create "FormatCode" formatCode
    // Target.create "CheckFormatCode" checkFormatCode
    // Target.create "Release" ignore // For local
    // Target.create "Publish" ignore //For CI
    // Target.create "CleanDocsCache" cleanDocsCache
    // Target.create "BuildDocs" buildDocs
    // Target.create "WatchDocs" watchDocs

    //-----------------------------------------------------------------------------
    // Target Dependencies
    //-----------------------------------------------------------------------------

    // // Only call Clean if DotnetPack was in the call chain
    // // Ensure Clean is called before DotnetRestore
    "Clean" ?=>! "DotnetRestore"

    //
    // "Clean"
    // ==>! "DotnetPack"
    //
    // // Only call UpdateChangelog if GitRelease was in the call chain
    // // Ensure UpdateChangelog is called after DotnetRestore and before DotnetBuild
    // "DotnetRestore"
    // ?=> "UpdateChangelog"
    // ?=>! "DotnetBuild"
    //
    // "CleanDocsCache"
    // ==>! "BuildDocs"
    //
    // "DotnetBuild"
    // ?=>! "BuildDocs"
    //
    // "DotnetBuild"
    // ==>! "BuildDocs"
    //
    // "DotnetBuild"
    // ==>! "WatchDocs"
    //
    // "UpdateChangelog"
    // ==> "GitRelease"
    // ==>! "Release"
    //
    "DotnetRestore" ==> "DotnetBuild"
//
// "DotnetPack"
// ==>! "IntegrationTests"
//
// "DotnetPack"
// ==> "PublishToNuGet"
// ==> "GithubRelease"
// ==>! "Publish"

[<EntryPoint>]
let main argv =
    argv
    |> Array.toList
    |> Context.FakeExecutionContext.Create false "build.fsx"
    |> Context.RuntimeContext.Fake
    |> Context.setExecutionContext

    initTargets ()
    Target.runOrDefaultWithArguments ("DotnetBuild")

    0 // return an integer exit code
