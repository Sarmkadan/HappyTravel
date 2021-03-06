#!groovy
pipeline {
    agent {
        label 'master' 
    }
    
    environment {
        APP_NAME="edo-api"
        NAMESPACE="dev"
        GIT_URL="git@bitbucket.org:happytravel/${APP_NAME}.git"
        GIT_CRED_ID='bitbucket'
        GIT_BRANCH="master"
        URL_REGISTRY="registry.dev.happytravel.com"
        IMAGE_NAME="${APP_NAME}:${NAMESPACE}"
    }
    
    stages {
        stage("Checkout") {
            steps {
                git branch: "${GIT_BRANCH}", credentialsId: "${GIT_CRED_ID}", url: "${GIT_URL}"
            }
        }
        stage("Force login at docker registry") {
            steps {
                sh 'docker login https://$URL_REGISTRY -u username -p password'
            }
        }
        stage('Build docker image') {
            steps {
                withCredentials([string(credentialsId: 'VAULT_TOKEN', variable: 'VAULT_TOKEN')]) {
                    sh 'docker build -t $URL_REGISTRY/$IMAGE_NAME-$BUILD_NUMBER --build-arg "VAULT_TOKEN=$VAULT_TOKEN" . --no-cache'
                    sh 'docker build -t $IMAGE_NAME-Migrations --build-arg "VAULT_TOKEN=$VAULT_TOKEN" -f ./Dockerfile.Migration . --no-cache'
                }
            }
        }
        stage("Run DB migrations") {
            steps {
                sh 'docker run -e HTDC_VAULT_ENDPOINT="https://vault.dev.happytravel.com/v1/" --rm $IMAGE_NAME-Migrations'
            }
        }
        stage('Push docker image to repository') {
            steps {
                sh 'docker push $URL_REGISTRY/$IMAGE_NAME-$BUILD_NUMBER'
                sh 'docker tag $URL_REGISTRY/$IMAGE_NAME-$BUILD_NUMBER $URL_REGISTRY/$IMAGE_NAME-latest'
                sh 'docker push $URL_REGISTRY/$IMAGE_NAME-latest'                
            }
        }
        stage('Deploy to k8s') {
            steps {
                dir('Helm') {
                    withCredentials([file(credentialsId: 'k8s', variable: 'k8s_cred')]) {
                        sh './setRevision.sh $BUILD_NUMBER'
                        sh 'helm --kubeconfig /$k8s_cred upgrade --install $NAMESPACE-$APP_NAME -f values_dev.yaml --wait --namespace $NAMESPACE ./'
                    }
                }
            }
        }  

    }

    post {
        always {
            echo "I AM ALWAYS first"
            notifyBuild("${currentBuild.currentResult}")
        }
        aborted {
            echo "BUILD ABORTED"
        }
        success {
            echo "BUILD SUCCESS"
            echo "Keep Current Build If branch is master"
        }
        unstable {
            echo "BUILD UNSTABLE"
        }
        failure {
            echo "BUILD FAILURE"
        }
    }
}
def getCurrentBranch () {
    return sh (
            script: 'git rev-parse --abbrev-ref HEAD',
            returnStdout: true
    ).trim()
}
def getShortCommitHash() {
    return sh(returnStdout: true, script: "git log -n 1 --pretty=format:'%h'").trim()
}
def getChangeAuthorName() {
    return sh(returnStdout: true, script: "git show -s --pretty=%an").trim()
}
def getChangeAuthorEmail() {
    return sh(returnStdout: true, script: "git show -s --pretty=%ae").trim()
}
def getChangeLog() {
    return sh(returnStdout: true, script: "git log -3 --date=short --pretty=format:'%ad %aN <%ae> %n%x09* %s%n'").trim()
}
def notifyBuild(String buildStatus = 'STARTED') {
    buildStatus = buildStatus ?: 'SUCCESS'

    def branchName = getCurrentBranch()
    def shortCommitHash = getShortCommitHash()
    def changeAuthorName = getChangeAuthorName()
    def changeAuthorEmail = getChangeAuthorEmail()
    def changeLog = getChangeLog()

    // Default values
    def subject = "${buildStatus}: '${env.JOB_NAME} [${env.BUILD_NUMBER}]'" + branchName + ", " + shortCommitHash
    def summary = "__**Build Number:**__ ${env.BUILD_NUMBER} " +
        "\n __**Build URL:**__ ${env.BUILD_URL} " +
        "\n __**Commit:**__ " + branchName + " " + shortCommitHash + 
        "\n __**Author:**__ " + changeAuthorName + " (" + changeAuthorEmail + ")" + 
        "\n \n __**ChangeLog:**__ " + " \n " + changeLog
        
    // Send message
    discordSend webhookURL: 'https://discordapp.com/api/webhooks/585188681892233239/eFnBXVIb-03zxCqOncAkCXvbnke02dWsDx2acpFDp1Lhe7JUyW5jGahAIH2VaiqzAbUQ',
    description: summary,
    result: currentBuild.currentResult,
    footer: currentBuild.currentResult,
    link: env.BUILD_URL,
    title: 'Started: '+JOB_NAME+' env: '+env.NAMESPACE,
    thumbnail: 'https://bitbucket-assetroot.s3.amazonaws.com/c/photos/2019/May/21/2707871814-0-happytravel-EDO_avatar.png'
    
    if (buildStatus == 'FAILURE') {
        emailext attachLog: true, body: summary, compressLog: true, recipientProviders: [brokenTestsSuspects(), brokenBuildSuspects(), culprits()], subject: subject, to: changeAuthorEmail
    }    
}