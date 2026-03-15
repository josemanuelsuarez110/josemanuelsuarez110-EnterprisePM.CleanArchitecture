param(
  [string]$RepoUrl = "https://github.com/josemanuelsuarez110/EnterprisePM.CleanArchitecture.git",
  [string]$Branch = "main"
)

if (-not (Get-Command git -ErrorAction SilentlyContinue)) { throw "Git not found" }

# prompt PAT
$secure = Read-Host "Introduce tu token PAT (scope repo)" -AsSecureString
$ptr = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($secure)
$token = [System.Runtime.InteropServices.Marshal]::PtrToStringBSTR($ptr)
[System.Runtime.InteropServices.Marshal]::ZeroFreeBSTR($ptr)
if ([string]::IsNullOrWhiteSpace($token)) { throw "Token vac?o" }

# stage/commit if needed
if (-not (git status --short)) {
    Write-Host "Sin cambios para commitear" -ForegroundColor Yellow
} else {
    git add .
    git commit -m "Initial ProjectManagementERP solution" | Out-Null
}

git branch -M $Branch | Out-Null

# build auth URL
$authUrl = $RepoUrl -replace '^https://', ''
$authUrl = "https://josemanuelsuarez110:$token@$authUrl"

# push
$prevUrl = git remote get-url origin 2>$null
if ($prevUrl) { git remote set-url origin $authUrl } else { git remote add origin $authUrl }

try {
    git push -u origin $Branch
    Write-Host "Push completado" -ForegroundColor Green
}
finally {
    # restore remote without token
    git remote set-url origin $RepoUrl | Out-Null
}
