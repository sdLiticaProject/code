FROM mono:6.12

RUN set -x \
        && addgroup --system --gid 102 sdcloud-admin \
        && adduser --system --disabled-login --ingroup sdcloud-admin --no-create-home --home /nonexistent --gecos "sdCloud admin user" --shell /bin/false --uid 102 sdcloud-admin \
	&& apt update \
        && apt install -y --no-install-recommends \
                mono-xsp4 gettext-base jq \
        && rm -rf /tmp/*


# ****************************************
#  Adding sdCloud models execution
#  service
# ****************************************

RUN mkdir -p /opt/sdcloud/SDCloud.WebApi \
	&& touch /opt/sdcloud/SDCloud.WebApi/application.log \
        && chown -R sdcloud-admin:sdcloud-admin /opt/sdcloud/SDCloud.WebApi \
        && mkdir -p /var/secret \
        && chown -R sdcloud-admin:sdcloud-admin /var/secret \
        && mkdir -p /opt/cloud \
        && chown -R sdcloud-admin:sdcloud-admin /opt/cloud

#COPY --chown=sdcloud-admin:sdcloud-admin *.dll /opt/sdcloud/SDCloud.WebApi/
#COPY --chown=sdcloud-admin:sdcloud-admin *.pdb /opt/sdcloud/SDCloud.WebApi/
#COPY --chown=sdcloud-admin:sdcloud-admin *.config /opt/sdcloud/SDCloud.WebApi/
#COPY --chown=sdcloud-admin:sdcloud-admin *.xml /opt/sdcloud/SDCloud.WebApi/
COPY --chown=sdcloud-admin:sdcloud-admin ./ /opt/sdcloud/SDCloud.WebApi/


COPY --chown=sdcloud-admin:sdcloud-admin token.file /var/secret/hc_vault_token
COPY --chown=sdcloud-admin:sdcloud-admin ./Docker/configResolver.sh ./Docker/resolveConfiguration.sh ./Docker/environment.template /opt/cloud/

ENV SDCloudRabbitMQHost=rabbitmq
ENV SDCloudRabbitMQPort=5672
ENV SDCloudRabbitMQVHost='/'
ENV SDCloudRabbitMQUserName='guest'
ENV SDCloudRabbitMQPassword='guest'

CMD /opt/cloud/resolveConfiguration.sh && \
	cd /opt/sdcloud/SDCloud.WebApi/ && \
	(touch /opt/sdcloud/SDCloud.WebApi/application.log; tail -f /opt/sdcloud/SDCloud.WebApi/application.log &) && \
	xsp4 --applications=/SDCloud.WebApi:. --port=80 --verbose --printlog --loglevels=Standard --nonstop