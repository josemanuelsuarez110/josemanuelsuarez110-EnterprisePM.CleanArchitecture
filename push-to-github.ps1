param(
    [string]$RepoUrl = "https://github.com/josemanuelsuarez110/EnterprisePM.CleanArchitecture.git",
    [string]$Branch = "main",
    [string]$CommitMessage = "Initial ProjectManagementERP solution",
    [string]$Token = "",
    [switch]$Force
)

function Ensure-Git {
    if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
        throw "git is not installed or not on PATH. Install Git and retry."
    }
}

function Ensure-Repo {
    if (-not (Test-Path .git)) {
        git init | Out-Null
    }
}

function Set-Remote($url) {
    $remoteName = "origin"
    if ((git remote) -contains $remoteName) {
        if (-not $Force) { return }
        git remote remove $remoteName | Out-Null
    }
    git remote add $remoteName $url | Out-Null
}

function Build-RemoteUrl($url, $token) {
    if ([string]::IsNullOrWhiteSpace($token)) { return $url }
    if ($url -match '^https://') {
        $cleanUrl = $url -replace '^https://', ''
        return "https://`$token@$cleanUrl".Replace("`$token", $token)
    }
    return $url
}

try {
    Ensure-Git
    Ensure-Repo

    # Stage & commit
    git add .
    if (-not (git status --short)) {
        Write-Host "Nothing to commit." -ForegroundColor Yellow
    } else {
        git commit -m $CommitMessage
    }

    git branch -M $Branch

    $remoteUrl = Build-RemoteUrl $RepoUrl $Token
    Set-Remote $remoteUrl

    git push -u origin $Branch
    Write-Host "Push completed to $RepoUrl" -ForegroundColor Green
} catch {
    Write-Error $_
    exit 1
}
