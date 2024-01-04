#!/bin/bash

cat /opt/cloud/environment.template | envsubst > /opt/cloud/environment.tmp

cat /opt/sdcloud/SDCloud.WebApi/Web.token.config | sed 's/__(/$/g' | sed 's/)__//g' > /opt/sdcloud/SDCloud.WebApi/Web.vars.config

source /opt/cloud/environment.tmp
source /opt/cloud/configResolver.sh

if [ -f "/opt/cloud/environment" ]
then
	rm /opt/cloud/environment
fi

set | grep SDCloud | sed 's|\([A-Za-z]*\)=.*|\1|' | while read varName
do
	echo "Value for $varName is ${!varName}"
	VALUE=${!varName}
	if [[ $VALUE = vault://* ]]
	then
		echo "Value is vault-based"
		IFS=':' read -ra PARTS <<< "$VALUE"
		SECRET=`echo ${PARTS[1]} | sed 's|//||g'`
		PROPERTY=${PARTS[2]}
		echo "Secret is $SECRET"
		echo "Property is $PROPERTY"
		RESOLVED_VALUE=`resolveConfig "$SECRET" "$PROPERTY"`
		echo "Resolved value: ${RESOLVED_VALUE:0:3}***************"
		echo ""
		echo "export $varName=$RESOLVED_VALUE" >> /opt/cloud/environment
	elif [[ $VALUE = vaultfile://* ]]
	then
		echo "Value is vault-based"
		IFS=':' read -ra PARTS <<< "$VALUE"
		SECRET=`echo ${PARTS[1]} | sed 's|//||g'`
		PROPERTY=${PARTS[2]}
		echo "Secret is $SECRET"
		echo "Property is $PROPERTY"
		RESOLVED_VALUE=`resolveConfig "$SECRET" "$PROPERTY"`
		mkdir -p /var/config
		timeStamp=`date +%s`
		echo "$RESOLVED_VALUE" > /var/config/$PROPERTY-$timeStamp
		echo "Resolved value: (was stored to file /var/config/$PROPERTY-$timeStamp)"
		ls -la /var/config/$PROPERTY-$timeStamp
		RESOLVED_VALUE="/var/config/$PROPERTY-$timeStamp"
		echo ""
		echo "export $varName=$RESOLVED_VALUE" >> /opt/cloud/environment
	else
		echo "Preserving value: ${VALUE:0:3}***************"
                echo ""
                echo "export $varName=$VALUE" >> /opt/cloud/environment
	fi
done

. /opt/cloud/environment

cat /opt/sdcloud/SDCloud.WebApi/Web.vars.config | envsubst > /opt/sdcloud/SDCloud.WebApi/Web.config
