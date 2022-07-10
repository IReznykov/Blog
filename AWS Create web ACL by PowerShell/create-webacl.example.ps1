$RegionName = "eu-west-2";
$ResourceARN = "arn:aws:elasticloadbalancing:$($RegionName):123456789012:loadbalancer/app/load-balancer-EXAMPLE/0123456789abcdef";
 
$result = .\create-webacl.ps1 `
    -resourcearn $ResourceARN `
    -rulesfilename "webacl-rules.json" `
    -tagname 'Blog' `
    -tagnameprefix 'blog' `
    -regionname $RegionName `
    -verbose;

if ((-not $?) -or (-not $result)) {
    Write-Host "Web ACL or related resources are not created" -ForegroundColor Red;
    return $false;
}
else {
    Write-Host "Web ACL and related resources are created sucessfully. and is associated with the resource:`nweb ACL ARN=$webAclARN`nResource ARN=$ResourceARN";
    return $true;
    
}

