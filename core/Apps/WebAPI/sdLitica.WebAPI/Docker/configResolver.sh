#!/bin/bash

HC_VAULT_TOKEN=`cat /var/secret/hc_vault_token`
HC_VAULT_HOST=`cat /var/secret/hc_vault_host`

if [ "X$HC_VAULT_HOST" == "X" ]
then
	HC_VAULT_HOST="secrets.cloud.sdcloud.io"
	echo "WARN: Using hardcoded secrets server"
fi

function resolveConfig() 
{
	if [ -z $HC_VAULT_TOKEN -a "X$HC_VAULT_TOKEN" != "X" ]
	then
		echo "Error: HC Vault token is not set"
		exit 1
	fi
	SECRET=$1
	PROPERTY=$2
	curl -s -X GET -H "X-Vault-Token: $HC_VAULT_TOKEN" https://$HC_VAULT_HOST/v1/$SECRET | jq ".data.data.$PROPERTY"
}