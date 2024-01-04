FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN set -x \
        && addgroup --system --gid 102 sdlitica-admin \
        && adduser --system --disabled-login --ingroup sdlitica-admin --no-create-home --home /nonexistent --gecos "sdCloud admin user" --shell /bin/false --uid 102 sdlitica-admin \
	&& apt update \
        && apt install -y --no-install-recommends \
                mono-xsp4 gettext-base jq \
        && rm -rf /tmp/*


# ****************************************
#  Adding sdCloud models execution
#  service
# ****************************************

RUN mkdir -p /opt/sdlitica/SDLitica.WebApi \
	&& touch /opt/sdlitica/SDLitica.WebApi/application.log \
        && chown -R sdlitica-admin:sdlitica-admin /opt/sdlitica/SDLitica.WebApi \
        && mkdir -p /var/secret \
        && chown -R sdlitica-admin:sdlitica-admin /var/secret \
        && mkdir -p /opt/cloud \
        && chown -R sdlitica-admin:sdlitica-admin /opt/cloud

#COPY --chown=sdlitica-admin:sdlitica-admin *.dll /opt/sdlitica/SDLitica.WebApi/
#COPY --chown=sdlitica-admin:sdlitica-admin *.pdb /opt/sdlitica/SDLitica.WebApi/
#COPY --chown=sdlitica-admin:sdlitica-admin *.config /opt/sdlitica/SDLitica.WebApi/
#COPY --chown=sdlitica-admin:sdlitica-admin *.xml /opt/sdlitica/SDLitica.WebApi/
COPY --chown=sdlitica-admin:sdlitica-admin ./ /opt/sdlitica/SDLitica.WebApi/


COPY --chown=sdlitica-admin:sdlitica-admin token.file /var/secret/hc_vault_token
COPY --chown=sdlitica-admin:sdlitica-admin ./Docker/configResolver.sh ./Docker/resolveConfiguration.sh ./Docker/environment.template /opt/cloud/

CMD chmod 777 /opt/cloud/resolveConfiguration.sh && \
    /opt/cloud/resolveConfiguration.sh && \
    cd /opt/sdlitica/SDLitica.WebApi/ && dotnet sdLitica.WebAPI.dll