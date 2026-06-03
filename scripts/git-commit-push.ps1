Param(
    [Parameter(Position=0)]
    [string]$Message
)

# Ensure Git is available
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Error "Git not found in PATH. Install Git or adjust PATH."
    exit 1
}

# Ensure we're inside a git repository and switch to repo root
$repoRoot = git rev-parse --show-toplevel 2>$null
if ($LASTEXITCODE -ne 0 -or -not $repoRoot) {
    Write-Error "Not a git repository (run this from inside a repo)."
    exit 1
}
Set-Location $repoRoot.Trim()

# Prompt for message if none provided
if (-not $Message -or $Message.Trim() -eq "") {
    $Message = Read-Host "Commit message"
    if (-not $Message -or $Message.Trim() -eq "") {
        Write-Error "Commit message is required. Aborting."
        exit 1
    }
}

Write-Host "Staging all changes..."
git add . 2>&1 | Write-Host

Write-Host "Committing with message: `"$Message`""
git commit -m $Message 2>&1
if ($LASTEXITCODE -ne 0) {
    $porcelain = git status --porcelain
    if (-not $porcelain) {
        Write-Host "Nothing to commit."
    } else {
        Write-Error "git commit failed."
        exit $LASTEXITCODE
    }
}

Write-Host "Pushing to remote..."
git push 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Error "git push failed."
    exit $LASTEXITCODE
}

Write-Host "Done."