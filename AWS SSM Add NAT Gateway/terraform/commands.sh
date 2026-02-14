terraform plan -var="environment=dev" -out="deploy.tfplan"

terraform apply -var="environment=dev" deploy.tfplan

terraform destroy -var="environment=dev"