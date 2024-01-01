$AZUSERNAME = $env:AZUSERNAME
$AZUSER_EMAIL = $env:AZUSER_EMAIL
$AZORG = $env:AZORG
$GHUSER = $env:GHUSER
$GHPAT = $env:GHPAT

git config --global user.email "$AZUSER_EMAIL"
git config --global user.name "$AZUSERNAME"

git clone "https://${GHUSER}:${GHPAT]@github.com/${GHUSER}/JobApplicationManager" "./JobApplicationManager"
$GIT_CMD_REPOSITORY = "https://${AZUSERNAME}:${AZUREPAT}@dev.azure.com/${AZORG}/JobApplicationManager/_git/JobApplicationManager"
git clone $GIT_CMD_REPOSITORY "./JobApplicationManager-az-develop"
git clone $GIT_CMD_REPOSITORY "./JobApplicationManager-az-master"

Write-Host "Checking out master"
Push-Location "./JobApplicationManager-az-master"
git checkout master
Remove-Item -Recurse -Force .git
Pop-Location

Write-Host "Checking out develop"
Push-Location "./JobApplicationManager-az-develop"
git checkout develop
Remove-Item -Recurse -Force .git
Pop-Location

Write-Host "Copying new stuff from Azure dev to Github"
Copy-Item -Recurse -Force "./JobApplicationManager-az-master/*" "./JobApplicationManager-gh/"

Push-Location "./JobApplicationManager-gh"
git checkout main
Write-Host "Copying new stuff from Azure master to Github"
git add .
git commit -m "sync master from azure to github"
git push

git checkout develop
Copy-Item -Recurse -Force "../JobApplicationManager-az-develop/*" .
Write-Host "Copying new stuff from Azure dev to Github"
git add .
git commit -m "sync master from azure to github"
git push
Pop-Location