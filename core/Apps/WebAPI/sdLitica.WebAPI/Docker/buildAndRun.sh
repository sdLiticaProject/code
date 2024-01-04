#!/bin/bash

set -e

PROJECT="sdlitica"
BASE_NAME="web-api-back-end"
export PREFIX=$1
export VERSION=$2


if [ -z "$HC_VAULT_TOKEN" ]
then
	echo "[Error]: Env variable HC_VAULT_TOKEN is not set"
	exit 1
fi
if [ -z "$PREFIX" ]
then
	echo "[Error]: PREFIX is not passed as a first argument"
        exit 1
else
	if [ "$PREFIX" != "prod" -a "$PREFIX" != "preprod"  -a "$PREFIX" != "wsl" ]
	then
		echo "[Error]: PREFIX value is incorrect. Expected 'prod' or 'preprod'"
	        exit 1	
	fi
fi
if [ -z "$VERSION" ]
then
        echo "[Error]: VERSION is not passed as a second argument"
        exit 1
fi

WSL_OPTS=""
WLS_RUN_OPTS=""
SUFFIX=""
if [ "$PREFIX" == "wsl" ]
then
          WSL_OPTS="--network host"
          WLS_RUN_OPTS="-p 5000:5000"
          SUFFIX=".wsl"
fi

echo "# *******************************************"
echo "#  Starting deployment of container"
echo "#"
echo "#  Deployment params:"
echo "#   - PROJECT:   $PROJECT"
echo "#   - BASE_NAME: $BASE_NAME"
echo "#   - PREFIX:    $PREFIX"
echo "#   - VERSION:   $VERSION"
echo "# *******************************************"
echo ""


echo $HC_VAULT_TOKEN > token.file

echo "# *******************************************"
echo "#  Building container image"
echo "# *******************************************"
docker build \
	--label "project=$PROJECT" \
	--label "version=$VERSION" \
	--tag   "$PROJECT/$BASE_NAME:$VERSION" $WSL_OPTS \
	--file  ./Docker/Dockerfile"$SUFFIX" . \
	| while read line; do echo "[docker build]: $line"; done
echo "# *******************************************"
echo "#  Completed: building container image"
echo "# *******************************************"
echo ""

echo "# *******************************************"
echo "#  Recording ids of running containers"
echo "# *******************************************"
EXISTING_ID=( `docker ps -q -a --filter "label=project=$PROJECT" --filter "label=prefix=$PREFIX" --filter "name=$PROJECT-$PREFIX-$BASE_NAME"` )
echo "#  Detected following IDs: ${EXISTING_ID[@]}"
echo "# *******************************************"
echo ""


if [ "X${EXISTING_ID[@]}" != "X"  ]
then
        echo "# *******************************************"
        echo "#  Stopping old containers"
        echo "# *******************************************"
        docker stop ${EXISTING_ID[@]}
        echo "# *******************************************"
        echo "#  Completed: stopping old containers"
        echo "# *******************************************"
        echo ""

        echo "# *******************************************"
        echo "#  Removing old containers"
        echo "# *******************************************"
        docker rm ${EXISTING_ID[@]}
        echo "# *******************************************"
        echo "#  Completed: removing old containers"
        echo "# *******************************************"
        echo ""
else
        echo "# *******************************************"
        echo "#  No running containers found "
        echo "#  nothing to stop and kill"
        echo "# *******************************************"
fi

echo "# *******************************************"
echo "#  Starting new container"
echo "# *******************************************"
docker run -d \
        --network main_bridge \
	--name="$PROJECT-$PREFIX-$BASE_NAME-$(date +%s)" \
	--label "project=$PROJECT" \
        --label "version=$VERSION" \
	--label "prefix=$PREFIX" \
        --env PREFIX=$PREFIX \
	$WLS_RUN_OPTS \
	--hostname $PROJECT-$PREFIX-$BASE_NAME \
        "$PROJECT/$BASE_NAME:$VERSION"
echo "# *******************************************"
echo "#  Completed: starting new container"
echo "# *******************************************"
echo ""