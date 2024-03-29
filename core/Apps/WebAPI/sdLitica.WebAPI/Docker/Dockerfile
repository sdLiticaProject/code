FROM mcr.microsoft.com/dotnet/aspnet:6.0

# ****************************************************
#  Seeting up users/groups in container. In addition
#  installing dependencies which are required by
#  configuration resolution scripts
# ****************************************************
RUN set -x \
        && addgroup --system --gid 102 sdlitica-admin \
        && adduser --system --disabled-login --ingroup sdlitica-admin --no-create-home --home /nonexistent --gecos "sdlitica admin user" --shell /bin/false --uid 102 sdlitica-admin \
	&& apt update \
        && apt install -y --no-install-recommends curl gettext-base jq \
        && rm -rf /tmp/*


# ****************************************************
#  Adding setting up basic folders structure 
#  and adding proper permissiong
# ****************************************************

RUN mkdir -p /opt/sdlitica/sdlitica.WebApi \
	&& touch /opt/sdlitica/sdlitica.WebApi/application.log \
        && chown -R sdlitica-admin:sdlitica-admin /opt/sdlitica/sdlitica.WebApi \
        && mkdir -p /var/secret \
        && chown -R sdlitica-admin:sdlitica-admin /var/secret \
        && mkdir -p /opt/cloud \
        && chown -R sdlitica-admin:sdlitica-admin /opt/cloud


# ****************************************************
#  Copying application parts into the container
# ****************************************************
COPY --chown=sdlitica-admin:sdlitica-admin ./bin/Debug/net6.0/ /opt/sdlitica/sdlitica.WebApi/
COPY --chown=sdlitica-admin:sdlitica-admin ./appsettings.json.template /opt/sdlitica/sdlitica.WebApi/


# ****************************************************
#  Copying system files into the container
# ****************************************************
COPY --chown=sdlitica-admin:sdlitica-admin token.file /var/secret/hc_vault_token
COPY --chown=sdlitica-admin:sdlitica-admin host.file /var/secret/hc_vault_host
COPY --chown=sdlitica-admin:sdlitica-admin \
                ./Docker/configResolver.sh \
                ./Docker/resolveConfiguration.sh \
                ./Docker/environment.template \
                ./Docker/environment.template.wsl \
                /opt/cloud/

# ****************************************************
#  Command to start application in container:
#  1. Giving exec permission to config resolving script
#  2. Running config resolving script
#  3. Starting configured application
# ****************************************************
CMD chmod 777 /opt/cloud/resolveConfiguration.sh && \
    /opt/cloud/resolveConfiguration.sh && \
    cd /opt/sdlitica/sdlitica.WebApi/ && dotnet sdLitica.WebAPI.dll