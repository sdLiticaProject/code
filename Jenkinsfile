String getReportMessage() {
        def message = 'Build result for **' + currentBuild.fullProjectName + '** with JenkinsId **' + currentBuild.id + '**'
        if (!currentBuild.changeSets.isEmpty()) {
             def changeLogSets = currentBuild.changeSets
             for (int i = 0; i < changeLogSets.size(); i++) {
                def entries = changeLogSets[i].items
                for (int j = 0; j < entries.length; j++) {
                        def entry = entries[j]
                        if (j==0){
                         message = message +'\n'+ "Committed by ${entry.author} :sunglasses:\n№${j}. Message: **${entry.msg}**\n"+"**ChangeList:**"
                        }else {
                            message = message + '\n'+" №${j}. Message: **${entry.msg}**" +'\n'+"**ChangeList:**"
                        }
                        def files = new ArrayList(entry.affectedFiles)
                        for (int k = 0; k < files.size(); k++) {
                            def file = files[k]
                            def parts = file.path.split('/')
                            def fileName = parts.size() == 1 ? file.path : parts[-1]
                            message = message +'\n' + "- ${file.editType.name} ${fileName}"
                        }
                }
             }
        } else {
            message += "\n- no changes"
        }
        return message
}
pipeline {

    agent { node { label 'sdCloud_docker_deploy' } }
    triggers {
        pollSCM('H/3 * * * *')
    }

    stages {
        stage('Preparation') {
            steps {
        step([$class: 'WsCleanup'])
            checkout scm
        }
    }

        stage('Build application') {
            steps {
                sh '''#!/bin/bash
                    mvn clean install
                '''
            }
        }

        stage('Deploy artifacts') {
            steps {
	    	withCredentials([string(credentialsId: 'sdcloud_vault_token', variable: 'HC_VAULT_TOKEN')]) {

                    sh '''#!/bin/bash
                        set -e
 		        GIT_REVISION=`git log -n 1 | grep "commit " | sed 's/commit //g'`
 		        ./Docker/buildAndRun.sh preprod "$GIT_REVISION"
                    '''
		}
             }
         }
    }

    post {
        always {
            script {
            statusColor = [SUCCESS: "green",
                           UNSTABLE: "yellow",
                           FAILURE: "red",
                           ABORTED: "pink"]

            logLines = currentBuild.rawBuild.getLog(1000);
            writeFile(file: 'build_log.txt', text: logLines.join('\n'))

            minio bucket: 'build-artifacts',
              credentialsId: 'jenkins-minio-credentials',
              excludes: '',
              includes: 'build_log.txt',
              host: 'https://minio.cloud.sdcloud.io',
              targetFolder: "${env.JOB_BASE_NAME}/${env.BUILD_NUMBER}"


            rocketSend channel: 'system_notifications_crossroads',
                    message: getReportMessage(),
                    rawMessage: true,
                    attachments: [[
                        title: 'Job build log',
                        color: statusColor[currentBuild.result],
                        text: "[LAST_COMMIT]("+env.GIT_URL.replaceAll(/\.git$/, '') + "/commit/"+env.GIT_COMMIT+
                        ")  ||  **Status**: " + currentBuild.result + "  ||  **Build time**: "
                        + currentBuild.durationString,
                        titleLinkDownload: true,
                        titleLink: "https://minio.cloud.sdcloud.io/build-artifacts/${env.JOB_BASE_NAME}/${env.BUILD_NUMBER}/build_log.txt",
                        ]]
            jacoco(
                    execPattern: '**/target/*.exec',
                    classPattern: '**/target/classes/**',
                    sourcePattern: '**/src/main/java/**',
                    exclusionPattern: '**/test/*'
            )
            }
        }
    }

 }