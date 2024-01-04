#!/bin/bash

HC_VAULT_TOKEN=`cat /var/secret/hc_vault_token`

function resolveConfig() 
{
	if [ -z $HC_VAULT_TOKEN -a "X$HC_VAULT_TOKEN" != "X" ]
	then
		echo "Error: HC Vault token is not set"
		exit 1
	fi
	SECRET=$1
	PROPERTY=$2
	curl -s -X GET -H "X-Vault-Token: $HC_VAULT_TOKEN" https://secrets.cloud.sdcloud.io/v1/$SECRET | jq ".data.data.$PROPERTY"
}